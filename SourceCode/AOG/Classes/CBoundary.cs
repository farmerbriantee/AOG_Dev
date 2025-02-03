using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace AgOpenGPS
{
    public partial class CBoundary
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        //the class of all the individual lists like headland, turnline, fence
        public List<CBoundaryList> bndList = new List<CBoundaryList>();

        //create a new fence and show it
        public List<vec3> fenceBeingMadePts = new List<vec3>(128);

        //boundary record properties
        public double createFenceOffset;
        public bool isFenceBeingMade;
        public bool isDrawRightSide = true, isDrawAtPivot = true, isOkToAddPoints = false;
        public bool isRecFenceWhenSectionOn = false;

        //headland properties
        public bool isHeadlandOn;
        public bool isToolInHeadland, isSectionControlledByHeadland;

        //turnline props
        public int turnSelected, closestTurnNum;
        public double iE = 0, iN = 0;

        //constructor
        public CBoundary(FormGPS _f)
        {
            mf = _f;
            turnSelected = 0;
            isHeadlandOn = false;
            isSectionControlledByHeadland = true;
        }

        public int IsPointInsideTurnArea(vec3 pt)
        {
            if (bndList.Count > 0 && bndList[0].turnLine.IsPointInPolygon(pt))
            {
                for (int i = 1; i < bndList.Count; i++)
                {
                    if (bndList[i].isDriveThru) continue;
                    if (bndList[i].turnLine.IsPointInPolygon(pt))
                    {
                        return i;
                    }
                }
                return 0;
            }
            return -1; //is outside border turn
        }

        public bool IsPointInsideFenceArea(vec3 testPoint)
        {
            //first where are we, must be inside outer and outside of inner geofence non drive thru turn borders
            if (bndList[0].fenceLineEar.IsPointInPolygon(testPoint))
            {
                for (int i = 1; i < bndList.Count; i++)
                {
                    //make sure not inside a non drivethru boundary
                    //if (buildList[i].isDriveThru) continue;
                    if (bndList[i].fenceLineEar.IsPointInPolygon(testPoint))
                    {
                        return false;
                    }
                }
                //we are safely inside outer, outside inner boundaries
                return true;
            }
            return false;
        }

        public void BuildTurnLines()
        {
            if (bndList.Count == 0)
            {
                //mf.TimedMessageBox(1500, " No Boundaries", "No Turn Lines Made");
                return;
            }

            //update the GUI values for boundaries
            mf.fd.UpdateFieldBoundaryGUIAreas();

            //to fill the list of line points
            vec3 point = new vec3();

            //determine how wide a headland space
            double totalHeadWidth = mf.yt.uturnDistanceFromBoundary;

            //inside boundaries
            for (int j = 0; j < bndList.Count; j++)
            {
                bndList[j].turnLine.Clear();
                if (bndList[j].isDriveThru) continue;

                int ptCount = bndList[j].fenceLine.Count;

                for (int i = ptCount - 1; i >= 0; i--)
                {
                    //calculate the point outside the boundary
                    point.easting = bndList[j].fenceLine[i].easting + (-Math.Sin(glm.PIBy2 + bndList[j].fenceLine[i].heading) * totalHeadWidth);
                    point.northing = bndList[j].fenceLine[i].northing + (-Math.Cos(glm.PIBy2 + bndList[j].fenceLine[i].heading) * totalHeadWidth);
                    point.heading = bndList[j].fenceLine[i].heading;
                    if (point.heading < -glm.twoPI) point.heading += glm.twoPI;

                    //only add if outside actual field boundary
                    if (j == 0 == bndList[j].fenceLineEar.IsPointInPolygon(point))
                    {
                        vec3 tPnt = new vec3(point.easting, point.northing, point.heading);
                        bndList[j].turnLine.Add(tPnt);
                    }
                }
                bndList[j].FixTurnLine(totalHeadWidth, 2);

                //countExit the reference list of original curve
                int cnt = bndList[j].turnLine.Count;

                //the temp array
                vec3[] arr = new vec3[cnt];

                for (int s = 0; s < cnt; s++)
                {
                    arr[s] = bndList[j].turnLine[s];
                }

                double delta = 0;
                bndList[j].turnLine?.Clear();

                for (int i = 0; i < arr.Length; i++)
                {
                    if (i == 0)
                    {
                        bndList[j].turnLine.Add(arr[i]);
                        continue;
                    }
                    delta += (arr[i - 1].heading - arr[i].heading);
                    if (Math.Abs(delta) > 0.005)
                    {
                        bndList[j].turnLine.Add(arr[i]);
                        delta = 0;
                    }
                }

                if (bndList[j].turnLine.Count > 0)
                {
                    vec3 end = new vec3(bndList[j].turnLine[0].easting,
                        bndList[j].turnLine[0].northing, bndList[j].turnLine[0].heading);
                    bndList[j].turnLine.Add(end);
                }
            }
        }

        public void SetHydPosition()
        {
            if (mf.vehicle.isHydLiftOn && mf.avgSpeed > 0.2 && mf.autoBtnState == btnStates.Auto && !mf.isReverse)
            {
                if (isToolInHeadland)
                {
                    mf.p_239.pgn[mf.p_239.hydLift] = 2;
                    if (mf.sounds.isHydLiftChange != isToolInHeadland)
                    {
                        if (mf.sounds.isHydLiftSoundOn) mf.sounds.sndHydLiftUp.Play();
                        mf.sounds.isHydLiftChange = isToolInHeadland;
                    }
                }
                else
                {
                    mf.p_239.pgn[mf.p_239.hydLift] = 1;
                    if (mf.sounds.isHydLiftChange != isToolInHeadland)
                    {
                        if (mf.sounds.isHydLiftSoundOn) mf.sounds.sndHydLiftDn.Play();
                        mf.sounds.isHydLiftChange = isToolInHeadland;
                    }
                }
            }
        }

        public void DrawBnds()
        {
            DrawFenceLines();
            DrawTurnLines();
            DrawHeadlands();
        }

        public void DrawHeadlands()
        {
            //Draw headland
            if (mf.bnd.isHeadlandOn)
            {
                GL.LineWidth(mf.trk.lineWidth * 4);

                GL.Color4(0, 0, 0, 0.80f);
                mf.bnd.bndList[0].hdLine.DrawPolygon();

                GL.LineWidth(mf.trk.lineWidth);
                GL.Color4(0.960f, 0.96232f, 0.30f, 1.0f);
                mf.bnd.bndList[0].hdLine.DrawPolygon();
            }
        }

        public void DrawTurnLines()
        {
            //draw the turnLines
            if (mf.yt.isYouTurnBtnOn && !mf.ct.isContourBtnOn)
            {
                GL.LineWidth(mf.trk.lineWidth * 4);
                GL.Color4(0, 0, 0, 0.80f);

                for (int i = 0; i < mf.bnd.bndList.Count; i++)
                {
                    mf.bnd.bndList[i].turnLine.DrawPolygon();
                }

                GL.Color3(0.76f, 0.6f, 0.95f);
                GL.LineWidth(mf.trk.lineWidth);
                for (int i = 0; i < mf.bnd.bndList.Count; i++)
                {
                    mf.bnd.bndList[i].turnLine.DrawPolygon();
                }
            }
        }

        public void DrawFenceLines()
        {
            GL.Color4(0, 0, 0, 0.8);
            GL.LineWidth(mf.trk.lineWidth * 4);

            for (int i = 0; i < bndList.Count; i++)
            {
                bndList[i].fenceLineEar.DrawPolygon();
            }

            GL.Color4(0.95f, 0.5f, 0.50f, 1.0f);
            GL.LineWidth(mf.trk.lineWidth);

            for (int i = 0; i < bndList.Count; i++)
            {
                if (i > 0) GL.Color4(0.85f, 0.34f, 0.3f, 1.0f);
                bndList[i].fenceLineEar.DrawPolygon();
            }

            if (fenceBeingMadePts.Count > 0)
            {
                //the boundary so far
                vec3 pivot = mf.pivotAxlePos;
                GL.LineWidth(mf.trk.lineWidth);
                GL.Color3(0.825f, 0.22f, 0.90f);
                GL.Begin(PrimitiveType.LineStrip);
                for (int h = 0; h < fenceBeingMadePts.Count; h++) GL.Vertex3(fenceBeingMadePts[h].easting, fenceBeingMadePts[h].northing, 0);
                GL.Color3(0.295f, 0.972f, 0.290f);
                GL.Vertex3(fenceBeingMadePts[0].easting, fenceBeingMadePts[0].northing, 0);
                GL.End();

                //line from last point to pivot marker
                GL.Color3(0.825f, 0.842f, 0.0f);
                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(1, 0x0700);
                GL.Begin(PrimitiveType.LineStrip);

                if (isDrawAtPivot)
                {
                    if (isDrawRightSide)
                    {
                        GL.Vertex3(fenceBeingMadePts[0].easting, fenceBeingMadePts[0].northing, 0);

                        GL.Vertex3(pivot.easting + (Math.Sin(pivot.heading - glm.PIBy2) * -createFenceOffset),
                                pivot.northing + (Math.Cos(pivot.heading - glm.PIBy2) * -createFenceOffset), 0);
                        GL.Vertex3(fenceBeingMadePts[fenceBeingMadePts.Count - 1].easting, fenceBeingMadePts[fenceBeingMadePts.Count - 1].northing, 0);
                    }
                    else
                    {
                        GL.Vertex3(fenceBeingMadePts[0].easting, fenceBeingMadePts[0].northing, 0);

                        GL.Vertex3(pivot.easting + (Math.Sin(pivot.heading - glm.PIBy2) * createFenceOffset),
                                pivot.northing + (Math.Cos(pivot.heading - glm.PIBy2) * createFenceOffset), 0);
                        GL.Vertex3(fenceBeingMadePts[fenceBeingMadePts.Count - 1].easting, fenceBeingMadePts[fenceBeingMadePts.Count - 1].northing, 0);
                    }
                }
                else //draw from tool
                {
                    if (isDrawRightSide)
                    {
                        GL.Vertex3(fenceBeingMadePts[0].easting, fenceBeingMadePts[0].northing, 0);
                        GL.Vertex3(mf.section[mf.tool.numOfSections - 1].rightPoint.easting, mf.section[mf.tool.numOfSections - 1].rightPoint.northing, 0);
                        GL.Vertex3(fenceBeingMadePts[fenceBeingMadePts.Count - 1].easting, fenceBeingMadePts[fenceBeingMadePts.Count - 1].northing, 0);
                    }
                    else
                    {
                        GL.Vertex3(fenceBeingMadePts[0].easting, fenceBeingMadePts[0].northing, 0);
                        GL.Vertex3(mf.section[0].leftPoint.easting, mf.section[0].leftPoint.northing, 0);
                        GL.Vertex3(fenceBeingMadePts[fenceBeingMadePts.Count - 1].easting, fenceBeingMadePts[fenceBeingMadePts.Count - 1].northing, 0);
                    }
                }
                GL.End();
                GL.Disable(EnableCap.LineStipple);

                //boundary points
                GL.Color3(0.0f, 0.95f, 0.95f);
                GL.PointSize(6.0f);
                GL.Begin(PrimitiveType.Points);
                for (int h = 0; h < fenceBeingMadePts.Count; h++) GL.Vertex3(fenceBeingMadePts[h].easting, fenceBeingMadePts[h].northing, 0);
                GL.End();
            }
        }
    }
}