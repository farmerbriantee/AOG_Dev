using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AOG
{
    public class CContour
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        public bool isContourOn, isContourBtnOn;

        private int stripNum;

        public vec2 boxA = new vec2(0, 0), boxB = new vec2(0, 2);

        private double lastSecond;
        private int pt = 0;

        //list of strip data individual points
        public List<vec3> ptList = new List<vec3>();

        //list of the list of individual Lines for entire field
        public List<List<vec3>> stripList = new List<List<vec3>>();

        //list of points for the new contour line
        public List<vec3> ctList = new List<vec3>();

        //constructor
        public CContour(FormGPS _f)
        {
            mf = _f;
            ctList.Capacity = 128;
            ptList.Capacity = 128;
        }

        public bool isLocked = false;

        //hitting the cycle lines buttons lock to current line
        public bool SetLockToLine()
        {
            if (ctList.Count > 5) isLocked = !isLocked;
            mf.SetContourLockImage(isLocked);
            return isLocked;
        }

        public void BuildContourGuidanceLine(vec3 pivot)
        {
            if (ctList.Count == 0)
            {
                if ((mf.secondsSinceStart - lastSecond) < 0.3) return;
            }
            else
            {
                if ((mf.secondsSinceStart - lastSecond) < 2) return;
            }

            lastSecond = mf.secondsSinceStart;
            int ptCount;
            double minDistance = double.MaxValue;
            int start, stop;

            double toolContourDistance = (Settings.Tool.toolWidth * 3 + Math.Abs(Settings.Tool.offset));

            //check if no strips yet, return
            int stripCount = stripList.Count;

            //if making a new strip ignore it or it will win always
            //stripCount--;
            if (stripCount < 1) return;

            double sinH = Math.Sin(pivot.heading) * 0.2;
            double cosH = Math.Cos(pivot.heading) * 0.2;

            double sin2HL = Math.Sin(pivot.heading + glm.PIBy2);
            double cos2HL = Math.Cos(pivot.heading + glm.PIBy2);

            boxA.easting = pivot.easting - sin2HL + sinH;
            boxA.northing = pivot.northing - cos2HL + cosH;

            boxB.easting = pivot.easting + sin2HL + sinH;
            boxB.northing = pivot.northing + cos2HL + cosH;

            if (!isLocked)
                stripNum = -1;

            for (int s = isLocked ? stripNum : 0; s < stripCount; s++)
            {
                ptCount = stripList[s].Count;
                if (ptCount == 0) continue;
                double dist;
                for (int p = 0; p < ptCount; p += 3)
                {
                    //if (s == stripCount - 1)
                    if ((((boxA.easting - boxB.easting) * (stripList[s][p].northing - boxB.northing))
                            - ((boxA.northing - boxB.northing) * (stripList[s][p].easting - boxB.easting))) > 0)
                    {
                        continue;
                    }

                    dist = ((pivot.easting - stripList[s][p].easting) * (pivot.easting - stripList[s][p].easting))
                        + ((pivot.northing - stripList[s][p].northing) * (pivot.northing - stripList[s][p].northing));
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        stripNum = s;
                        pt = p;
                        //B = p;
                    }
                }
                if (isLocked) break;
            }

            if (!isLocked && (stripNum < 0 || Math.Sqrt(minDistance) > toolContourDistance || stripList[stripNum].Count < 4))
            {
                //no points in the box, exit
                ctList.Clear();
                isLocked = false;
                mf.SetContourLockImage(isLocked);
                return;
            }
            vec2 vec2pivot = new vec2(pivot);

            if (mf.gyd.FindClosestSegment(stripList[stripNum], false, vec2pivot, out int A, out int B, pt - 10, pt + 10))
            {
                double distanceFromRefLine = mf.gyd.FindDistanceToSegment(vec2pivot, stripList[stripNum][A], stripList[stripNum][B], out _, out double Time, true, false, false);

                //are we going same direction as stripList was created?
                bool isSameWay = Math.PI - Math.Abs(Math.Abs(mf.fixHeading - stripList[stripNum][A].heading) - Math.PI) < 1.57;

                double howManyPathsAway = Math.Round((distanceFromRefLine + (isSameWay ? Settings.Tool.offset : -Settings.Tool.offset))
                                    / (Settings.Tool.toolWidth - Settings.Tool.overlap), MidpointRounding.AwayFromZero);

                //beside what is done
                if (howManyPathsAway < -1) howManyPathsAway = -1;
                else if (howManyPathsAway > 1) howManyPathsAway = 1;


                ctList.Clear();

                //make the new guidance line list called guideList
                ptCount = stripList[stripNum].Count;

                //shorter behind you
                if (isSameWay)
                {
                    start = pt - 20; if (start < 0) start = 0;
                    stop = pt + 70; if (stop > ptCount) stop = ptCount;
                }
                else
                {
                    start = pt - 70; if (start < 0) start = 0;
                    stop = pt + 20; if (stop > ptCount) stop = ptCount;
                }

                double distAway = (Settings.Tool.toolWidth - Settings.Tool.overlap) * howManyPathsAway
                    + (isSameWay ? -Settings.Tool.offset : Settings.Tool.offset);
                double distSqAway = (distAway * distAway) * 0.97;

                for (int i = start; i < stop; i++)
                {
                    vec3 point = new vec3(
                        stripList[stripNum][i].easting + (Math.Cos(stripList[stripNum][i].heading) * distAway),
                        stripList[stripNum][i].northing - (Math.Sin(stripList[stripNum][i].heading) * distAway),
                        stripList[stripNum][i].heading);

                    bool isOkToAdd = true;
                    //make sure its not closer then 1 eq width
                    for (int j = start; j < stop; j++)
                    {
                        double check = glm.DistanceSquared(point.northing, point.easting,
                            stripList[stripNum][j].northing, stripList[stripNum][j].easting);
                        if (check < distSqAway)
                        {
                            isOkToAdd = false;
                            break;
                        }
                    }

                    if (isOkToAdd)
                    {
                        if (ctList.Count > 0)
                        {
                            double dist =
                                ((point.easting - ctList[ctList.Count - 1].easting) * (point.easting - ctList[ctList.Count - 1].easting))
                                + ((point.northing - ctList[ctList.Count - 1].northing) * (point.northing - ctList[ctList.Count - 1].northing));
                            if (dist > 0.2)
                                ctList.Add(point);
                        }
                        else ctList.Add(point);
                    }
                }

                mf.gyd.isFindGlobalNearestTrackPoint = true;
                mf.trk.isHeadingSameWay = isSameWay;
            }
        }

        //start stop and add points to list
        public void StartContourLine()
        {
            //if (stripList.Count == 0)
            //{
            //make new ptList
            ptList = new List<vec3>(16);
            //ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading)
            //    * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));
            stripList.Add(ptList);
            isContourOn = true;
            return;
            //}
            //else
            //{
            //    //reuse ptList
            //    ptList?.Clear();
            //    //ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading)
            //    //    * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));
            //    isContourOn = true;
            //}
        }

        //Add current position to stripList
        public void AddPoint(vec3 pivot)
        {
            ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading) * Settings.Tool.offset,
                pivot.northing - Math.Sin(pivot.heading) * Settings.Tool.offset,
                pivot.heading));
        }

        //End the strip
        public void StopContourLine()
        {
            //make sure its long enough to bother
            if (ptList.Count > 5)
            {
                //add the point list to the save list for appending to contour file
                mf.contourSaveList.Add(ptList);
            }
            //delete ptList
            else
            {
                ptList.Clear();
            }

            //turn it off
            isContourOn = false;
        }

        //build contours for boundaries
        public void BuildFenceContours(double spacingInt)
        {
            spacingInt *= 0.01;
            if (mf.bnd.bndList.Count == 0)
            {
                mf.TimedMessageBox(1500, "Boundary Contour Error", "No Boundaries Made");
                return;
            }

            if (mf.patchCounter != 0)
            {
                mf.TimedMessageBox(1500, "Section Control On", "Turn Off Section Control");
                return;
            }

            //determine how wide a headland space
            double totalHeadWidth = ((Settings.Tool.toolWidth - Settings.Tool.overlap) * 0.5) - spacingInt;

            //totalHeadWidth = (mf.tool.toolWidth - mf.tool.toolOverlap) * 0.5 + 0.2 + (mf.tool.toolWidth - mf.tool.toolOverlap);

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                ptList = mf.bnd.bndList[j].fenceLine.OffsetLine(totalHeadWidth, 1, true);
                stripList.Add(ptList);
            }

            mf.TimedMessageBox(1500, "Boundary Contour", "Contour Path Created");
        }

        //draw the red follow me line
        public void DrawContourLine()
        {
            //GL.Color3(0.98f, 0.98f, 0.50f);
            //GL.Begin(PrimitiveType.Lines);
            //GL.Vertex3(boxA.easting, boxA.northing, 0);
            //GL.Vertex3(boxB.easting, boxB.northing, 0);
            //GL.End();

            ////draw the guidance line
            int ptCount = ctList.Count;
            if (ptCount < 2) return;
            GL.LineWidth(Settings.User.setDisplay_lineWidth);
            GL.Color3(0.98f, 0.2f, 0.980f);
            ctList.DrawPolygon(PrimitiveType.LineStrip);

            GL.PointSize(Settings.User.setDisplay_lineWidth);
            GL.Color3(0.87f, 08.7f, 0.25f);

            ctList.DrawPolygon(PrimitiveType.Points);

            //Draw the captured ref strip, red if locked
            if (isLocked)
            {
                GL.Color3(0.983f, 0.92f, 0.420f);
                GL.LineWidth(4);
            }
            else
            {
                GL.Color3(0.3f, 0.982f, 0.0f);
                GL.LineWidth(Settings.User.setDisplay_lineWidth);
            }

            //GL.PointSize(6.0f);
            if (stripNum > -1)
            {
                stripList[stripNum].DrawPolygon(PrimitiveType.Points);

                GL.Color3(0.35f, 0.30f, 0.90f);
                GL.PointSize(6.0f);
                GL.Begin(PrimitiveType.Points);
                GL.Vertex3(stripList[stripNum][pt].easting, stripList[stripNum][pt].northing, 0);
                GL.End();
            }

            ////draw the reference line
            //GL.PointSize(3.0f);
            ////if (isContourBtnOn)
            //{
            //    ptCount = stripList.Count;
            //    if (ptCount > 0)
            //    {
            //        ptCount = stripList[closestRefPatch].Count;
            //        GL.Begin(PrimitiveType.Points);
            //        for (int i = 0; i < ptCount; i++)
            //        {
            //            GL.Vertex2(stripList[closestRefPatch][i].easting, stripList[closestRefPatch][i].northing);
            //        }
            //        GL.End();
            //    }
            //}
        }

        //Reset the contour to zip
        public void ResetContour()
        {
            stripList.Clear();
            ptList?.Clear();
            ctList?.Clear();
        }
    }//class
}//namespace