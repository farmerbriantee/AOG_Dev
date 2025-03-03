using AgOpenGPS.Classes;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormABDraw : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private Point fixPt;

        private bool isA = true;
        private int start = 99999, end = 99999;
        private int bndSelect = 0, originalLine;
        private bool isCancel = false;

        private bool zoomToggle;

        private int indx = -1;

        private double zoom = 1, sX = 0, sY = 0;

        public List<CTrk> gTemp = new List<CTrk>();

        public vec3 pint = new vec3(0.0, 1.0, 0.0);

        private bool isDrawSections = false;

        public FormABDraw(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
        }

        private void FormABDraw_Load(object sender, EventArgs e)
        {
            if (isDrawSections) btnDrawSections.Image = Properties.Resources.MappingOn;
            else btnDrawSections.Image = Properties.Resources.MappingOff;

            originalLine = mf.trk.idx;

            gTemp.Clear();

            foreach (var item in mf.trk.gArr)
            {
                gTemp.Add(new CTrk(item));
            }

            if (gTemp.Count != 0)
            {
                if (mf.trk.idx > -1 && mf.trk.idx <= gTemp.Count)
                {
                    indx = mf.trk.idx;
                }
                else
                    indx = 0;
            }

            FixLabelsCurve();

            cboxIsZoom.Checked = false;
            zoomToggle = false;

            Size = Settings.Interface.setWindow_abDrawSize;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            this.Top = (area.Height - this.Height) / 2;
            this.Left = (area.Width - this.Width) / 2;
            FormABDraw_ResizeEnd(this, e);

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormABDraw_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isCancel)
            {
                if (gTemp.Count == 0)
                {
                    mf.trk.idx = -1;
                    mf.trk.gArr.Clear();
                    mf.FileSaveTracks();
                    if (mf.isBtnAutoSteerOn)
                    {
                        mf.btnAutoSteer.PerformClick();
                        mf.TimedMessageBox(2000, gStr.Get(gs.gsGuidanceStopped), "Return From Editing");
                    }
                    if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                }
                else
                {
                    //load tracks from temp
                    mf.trk.gArr.Clear();
                    foreach (var item in gTemp)
                    {
                        mf.trk.gArr.Add(new CTrk(item));
                    }

                    mf.FileSaveTracks();

                    if (gTemp[indx].isVisible)
                    {
                        mf.trk.idx = indx;
                        if (mf.trk.idx != originalLine)
                        {
                            if (mf.isBtnAutoSteerOn) mf.btnAutoSteer.PerformClick();
                            mf.TimedMessageBox(2000, gStr.Get(gs.gsGuidanceStopped), "Return From Editing");
                            if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                        }
                    }
                    else
                    {
                        bool isOneVis = false;

                        foreach (var item in gTemp)
                        {
                            if (item.isVisible) isOneVis = true;
                            break;
                        }

                        if (isOneVis)
                        {
                            if (gTemp.Count > 1)
                            {
                                while (true)
                                {
                                    indx++;
                                    if (indx == gTemp.Count) indx = 0;

                                    if (gTemp[indx].isVisible)
                                    {
                                        mf.trk.idx = indx;
                                        break;
                                    }
                                }
                            }

                            if (mf.trk.idx != originalLine)
                            {
                                if (mf.isBtnAutoSteerOn)
                                {
                                    mf.btnAutoSteer.PerformClick();
                                    mf.TimedMessageBox(2000, gStr.Get(gs.gsGuidanceStopped), "Return From Editing");
                                }
                                if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                            }
                        }
                        else
                        {
                            mf.trk.idx = -1;

                            mf.TimedMessageBox(2000, gStr.Get(gs.gsEditABLine), gStr.Get(gs.gsNoABLineActive));
                            if (mf.isBtnAutoSteerOn) mf.btnAutoSteer.PerformClick();
                            if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                        }
                    }
                }
            }

            mf.trk.isTrackValid = false;
            mf.twoSecondCounter = 100;

            Settings.Interface.setWindow_abDrawSize = Size;
        }

        private void cboxIsZoom_CheckedChanged(object sender, EventArgs e)
        {
            zoomToggle = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            isCancel = false;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isCancel = true;
            Close();
        }

        private void btnCancelTouch_Click(object sender, EventArgs e)
        {
            //update the arrays
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;
            start = 99999; end = 99999;
            isA = true;

            FixLabelsCurve();

            mf.trk.designPtsList?.Clear();

            zoom = 1;
            sX = 0;
            sY = 0;
            zoomToggle = false;

            btnExit.Focus();
        }

        private void FixLabelsCurve()
        {
            if (indx > -1 && gTemp.Count > 0)
            {
                tboxNameCurve.Text = gTemp[indx].name;
                tboxNameCurve.Enabled = true;
                lblCurveSelected.Text = (indx + 1).ToString() + " / " + gTemp.Count.ToString();
                cboxIsVisible.Visible = true;
                cboxIsVisible.Checked = gTemp[indx].isVisible;
                if (gTemp[indx].isVisible)
                    cboxIsVisible.Image = Properties.Resources.TrackVisible;
                else cboxIsVisible.Image = Properties.Resources.TracksInvisible;
            }
            else
            {
                tboxNameCurve.Text = "***";
                tboxNameCurve.Enabled = false;
                lblCurveSelected.Text = "*";
                cboxIsVisible.Visible = false;
            }
        }

        private void btnSelectCurve_Click(object sender, EventArgs e)
        {
            if (gTemp.Count > 0)
            {
                indx++;
                if (indx > (gTemp.Count - 1)) indx = 0;
            }
            else
            {
                indx = -1;
            }

            FixLabelsCurve();

            //if (mf.trk.numCurveLines > 0)
            //{
            //    mf.trk.numCurveLineSelected++;
            //    if (mf.trk.numCurveLineSelected > mf.trk.numCurveLines) mf.trk.numCurveLineSelected = 1;
            //}
            //else
            //{
            //    mf.trk.numCurveLineSelected = 0;
            //}

            //FixLabels();
        }

        private void btnSelectCurveBk_Click(object sender, EventArgs e)
        {
            if (gTemp.Count > 0)
            {
                indx--;
                if (indx < 0) indx = gTemp.Count - 1;
            }
            else
            {
                indx = -1;
            }

            FixLabelsCurve();
        }

        private void cboxIsVisible_Click(object sender, EventArgs e)
        {
            gTemp[indx].isVisible = cboxIsVisible.Checked;
            if (gTemp[indx].isVisible)
                cboxIsVisible.Image = Properties.Resources.TrackVisible;
            else cboxIsVisible.Image = Properties.Resources.TracksInvisible;
        }

        private void btnDeleteCurve_Click(object sender, EventArgs e)
        {
            if (indx > -1)
            {
                gTemp.RemoveAt(indx);
            }

            if (gTemp.Count > 0)
            {
                if (indx > gTemp.Count - 1)
                {
                    indx = gTemp.Count - 1;
                }
            }
            else
            {
                indx = -1;
            }

            originalLine = -2;

            FixLabelsCurve();
        }

        private void btnDrawSections_Click(object sender, EventArgs e)
        {
            isDrawSections = !isDrawSections;
            if (isDrawSections) btnDrawSections.Image = Properties.Resources.MappingOn;
            else btnDrawSections.Image = Properties.Resources.MappingOff;
        }

        private void tboxNameCurve_Leave(object sender, EventArgs e)
        {
            if (indx > -1)
                gTemp[indx].name = tboxNameCurve.Text.Trim();
            btnExit.Focus();
        }

        private void tboxNameCurve_Enter(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((System.Windows.Forms.TextBox)sender, this);

                if (indx > -1)
                    gTemp[indx].name = tboxNameCurve.Text.Trim();
                btnExit.Focus();
            }
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            if (indx > -1)
            {
                gTemp[indx].name += DateTime.Now.ToString(" hh:mm:ss", CultureInfo.InvariantCulture);
                FixLabelsCurve();
            }
        }

        private void btnMakeBoundaryCurve_Click(object sender, EventArgs e)
        {            //countExit the points from the boundary
            for (int q = 0; q < mf.bnd.bndList.Count; q++)
            {
                vec3 pt3;
                mf.trk.designPtsList?.Clear();
                for (int i = 0; i < mf.bnd.bndList[bndSelect].fenceLine.Count; i++)
                {
                    //calculate the point inside the boundary
                    pt3 = new vec3(mf.bnd.bndList[bndSelect].fenceLine[i]);

                    mf.trk.designPtsList.Add(new vec3(pt3));
                }

                gTemp.Add(new CTrk());
                //array number is 1 less since it starts at zero
                indx = gTemp.Count - 1;

                gTemp[indx].ptA = new vec2(mf.trk.designPtsList[0].easting, mf.trk.designPtsList[0].northing);
                gTemp[indx].ptB = new vec2(mf.trk.designPtsList[mf.trk.designPtsList.Count - 1].easting, mf.trk.designPtsList[mf.trk.designPtsList.Count - 1].northing);

                pt3 = new vec3(mf.trk.designPtsList[0]);
                mf.trk.designPtsList.Add(pt3);

                int cnt = mf.trk.designPtsList.Count;
                if (cnt > 3)
                {
                    pt3 = new vec3(mf.trk.designPtsList[0]);
                    mf.trk.designPtsList.Add(pt3);

                    //make sure point distance isn't too big
                    mf.trk.MakePointMinimumSpacing(ref mf.trk.designPtsList, 1.6);
                    mf.trk.CalculateHeadings(ref mf.trk.designPtsList);

                    //create a name
                    gTemp[indx].name = "Boundary Curve";

                    if (q > 0) gTemp[indx].name = "Inner Boundary Curve " + q.ToString();

                    gTemp[indx].heading = 0;
                    gTemp[indx].mode = TrackMode.bndCurve;

                    //write out the Curve Points
                    foreach (vec3 item in mf.trk.designPtsList)
                    {
                        gTemp[indx].curvePts.Add(item);
                    }
                }
            }

            //update the arrays
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;
            start = 99999; end = 99999;

            FixLabelsCurve();

            mf.trk.designPtsList?.Clear();

            btnExit.Focus();
        }

        private void BtnMakeCurve_Click(object sender, EventArgs e)
        {
            bool isLoop = false;
            int limit = end;

            if ((Math.Abs(start - end)) > (mf.bnd.bndList[bndSelect].fenceLine.Count * 0.5))
            {
                isLoop = true;
                if (start < end)
                {
                    (end, start) = (start, end);
                }

                limit = end;
                end = mf.bnd.bndList[bndSelect].fenceLine.Count;
            }
            else //normal
            {
                if (start > end)
                {
                    (end, start) = (start, end);
                }
            }

            mf.trk.designPtsList?.Clear();
            vec3 pt3;

            for (int i = start; i < end; i++)
            {
                //calculate the point inside the boundary
                pt3 = new vec3(mf.bnd.bndList[bndSelect].fenceLine[i]);

                mf.trk.designPtsList.Add(new vec3(pt3));

                if (isLoop && i == mf.bnd.bndList[bndSelect].fenceLine.Count - 1)
                {
                    i = -1;
                    isLoop = false;
                    end = limit;
                }
            }

            gTemp.Add(new CTrk());
            //array number is 1 less since it starts at zero
            indx = gTemp.Count - 1;

            gTemp[indx].ptA =
                new vec2(mf.trk.designPtsList[0].easting, mf.trk.designPtsList[0].northing);
            gTemp[indx].ptB =
                new vec2(mf.trk.designPtsList[mf.trk.designPtsList.Count - 1].easting,
                mf.trk.designPtsList[mf.trk.designPtsList.Count - 1].northing);

            int cnt = mf.trk.designPtsList.Count;
            if (cnt > 3)
            {
                //make sure point distance isn't too big
                mf.trk.MakePointMinimumSpacing(ref mf.trk.designPtsList, 1.6);
                mf.trk.CalculateHeadings(ref mf.trk.designPtsList);

                //calculate average heading of line
                double x = 0, y = 0;

                foreach (vec3 pt in mf.trk.designPtsList)
                {
                    x += Math.Cos(pt.heading);
                    y += Math.Sin(pt.heading);
                }
                x /= mf.trk.designPtsList.Count;
                y /= mf.trk.designPtsList.Count;
                gTemp[indx].heading = Math.Atan2(y, x);
                if (gTemp[indx].heading < 0) gTemp[indx].heading += glm.twoPI;

                //build the tail extensions
                mf.trk.AddFirstLastPoints(ref mf.trk.designPtsList, 100);
                //mf.trk.SmoothAB(2);
                mf.trk.CalculateHeadings(ref mf.trk.designPtsList);

                //array number is 1 less since it starts at zero
                indx = gTemp.Count - 1;

                //create a name
                gTemp[indx].name = "Cu " +
                    (Math.Round(glm.toDegrees(gTemp[indx].heading), 1)).ToString(CultureInfo.InvariantCulture)
                    + "\u00B0";

                gTemp[indx].mode = TrackMode.Curve;

                //write out the Curve Points
                foreach (vec3 item in mf.trk.designPtsList)
                {
                    gTemp[indx].curvePts.Add(item);
                }

                //update the arrays
                btnMakeABLine.Enabled = false;
                btnMakeCurve.Enabled = false;
                start = 99999; end = 99999;

                FixLabelsCurve();
            }
            else
            {
            }
            btnExit.Focus();
            mf.trk.designPtsList?.Clear();
        }

        private void BtnMakeABLine_Click(object sender, EventArgs e)
        {
            //if more then half way around, it crosses start finish
            if ((Math.Abs(start - end)) <= (mf.bnd.bndList[bndSelect].fenceLine.Count * 0.5))
            {
                if (start < end)
                {
                    (end, start) = (start, end);
                }
            }
            else
            {
                if (start > end)
                {
                    (end, start) = (start, end);
                }
            }

            //calculate the AB Heading
            double abHead = Math.Atan2(
                mf.bnd.bndList[bndSelect].fenceLine[start].easting - mf.bnd.bndList[bndSelect].fenceLine[end].easting,
                mf.bnd.bndList[bndSelect].fenceLine[start].northing - mf.bnd.bndList[bndSelect].fenceLine[end].northing);
            if (abHead < 0) abHead += glm.twoPI;

            gTemp.Add(new CTrk());

            indx = gTemp.Count - 1;

            gTemp[indx].heading = abHead;
            double hsin = Math.Sin(abHead);
            double hcos = Math.Cos(abHead);

            gTemp[indx].mode = TrackMode.AB;

            //calculate the new points for the reference line and points
            gTemp[indx].ptA.easting = mf.bnd.bndList[bndSelect].fenceLine[end].easting;
            gTemp[indx].ptA.northing = mf.bnd.bndList[bndSelect].fenceLine[end].northing;

            gTemp[indx].ptB.easting = mf.bnd.bndList[bndSelect].fenceLine[start].easting;
            gTemp[indx].ptB.northing = mf.bnd.bndList[bndSelect].fenceLine[start].northing;

            //fill in the dots between A and B
            double len = glm.Distance(gTemp[indx].ptA, gTemp[indx].ptB);
            if (len < 20)
            {
                gTemp[indx].ptB.easting = gTemp[indx].ptA.easting + (Math.Sin(abHead) * 30);
                gTemp[indx].ptB.northing = gTemp[indx].ptA.northing + (Math.Cos(abHead) * 30);
            }
            len = glm.Distance(gTemp[indx].ptA, gTemp[indx].ptB);

            vec3 P1 = new vec3();
            for (int i = 0; i < (int)len; i += 1)
            {
                P1.easting = (hsin * i) + gTemp[indx].ptA.easting;
                P1.northing = (hcos * i) + gTemp[indx].ptA.northing;
                P1.heading = abHead;
                gTemp[indx].curvePts.Add(P1);
            }

            //build the tail extensions
            mf.trk.AddFirstLastPoints(ref gTemp[indx].curvePts, 50);

            //create a name
            gTemp[indx].name = "AB: " +
                (Math.Round(glm.toDegrees(gTemp[indx].heading), 1)).ToString(CultureInfo.InvariantCulture) + "\u00B0";

            //clean up gui
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;

            start = 99999; end = 99999;

            FixLabelsCurve();
        }

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = oglSelf.PointToClient(Cursor.Position);

            int wid = oglSelf.Width;
            int halfWid = oglSelf.Width / 2;
            double scale = (double)wid * 0.903;

            if (cboxIsZoom.Checked && !zoomToggle)
            {
                sX = ((halfWid - (double)pt.X) / wid) * 1.1;
                sY = ((halfWid - (double)pt.Y) / -wid) * 1.1;
                zoom = 0.1;
                zoomToggle = true;
                return;
            }

            zoomToggle = false;
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;

            //Convert to Origin in the center of window, 800 pixels
            fixPt.X = pt.X - halfWid;
            fixPt.Y = (wid - pt.Y - halfWid);
            vec3 plotPt = new vec3
            {
                //convert screen coordinates to field coordinates
                easting = fixPt.X * mf.maxFieldDistance / scale * zoom,
                northing = fixPt.Y * mf.maxFieldDistance / scale * zoom,
                heading = 0
            };

            plotPt.easting += mf.fieldCenterX + mf.maxFieldDistance * -sX;
            plotPt.northing += mf.fieldCenterY + mf.maxFieldDistance * -sY;

            pint.easting = plotPt.easting;
            pint.northing = plotPt.northing;

            zoom = 1;
            sX = 0;
            sY = 0;

            if (isA)
            {
                double minDistA = double.MaxValue;
                start = 99999; end = 99999;

                for (int j = 0; j < mf.bnd.bndList.Count; j++)
                {
                    for (int i = 0; i < mf.bnd.bndList[j].fenceLine.Count; i++)
                    {
                        double dist = ((pint.easting - mf.bnd.bndList[j].fenceLine[i].easting) * (pint.easting - mf.bnd.bndList[j].fenceLine[i].easting))
                                        + ((pint.northing - mf.bnd.bndList[j].fenceLine[i].northing) * (pint.northing - mf.bnd.bndList[j].fenceLine[i].northing));
                        if (dist < minDistA)
                        {
                            minDistA = dist;
                            bndSelect = j;
                            start = i;
                        }
                    }
                }

                isA = false;
            }
            else
            {
                double minDistA = double.MaxValue;
                int j = bndSelect;

                for (int i = 0; i < mf.bnd.bndList[j].fenceLine.Count; i++)
                {
                    double dist = ((pint.easting - mf.bnd.bndList[j].fenceLine[i].easting) * (pint.easting - mf.bnd.bndList[j].fenceLine[i].easting))
                                    + ((pint.northing - mf.bnd.bndList[j].fenceLine[i].northing) * (pint.northing - mf.bnd.bndList[j].fenceLine[i].northing));
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        end = i;
                    }
                }

                isA = true;

                btnMakeABLine.Enabled = true;
                btnMakeCurve.Enabled = true;
            }
        }

        private void oglSelf_Paint(object sender, PaintEventArgs e)
        {
            oglSelf.MakeCurrent();

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();                  // Reset The View

            //back the camera up
            GL.Translate(0, 0, -mf.maxFieldDistance * zoom);

            //translate to that spot in the world
            GL.Translate(-mf.fieldCenterX + sX * mf.maxFieldDistance, -mf.fieldCenterY + sY * mf.maxFieldDistance, 0);

            if (isDrawSections) DrawSections();

            GL.LineWidth(3);

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                if (j == bndSelect)
                    GL.Color3(1.0f, 1.0f, 1.0f);
                else
                    GL.Color3(0.62f, 0.635f, 0.635f);

                GL.Begin(PrimitiveType.LineLoop);
                for (int i = 0; i < mf.bnd.bndList[j].fenceLineEar.Count; i++)
                {
                    GL.Vertex3(mf.bnd.bndList[j].fenceLineEar[i].easting, mf.bnd.bndList[j].fenceLineEar[i].northing, 0);
                }
                GL.End();
            }

            //the vehicle
            GL.PointSize(16.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(1.0f, 0.00f, 0.0f);
            GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0.0);
            GL.End();

            GL.PointSize(8.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(0.00f, 0.0f, 0.0f);
            GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0.0);
            GL.End();

            //draw the line building graphics
            if (start != 99999 || end != 99999) DrawABTouchPoints();

            //draw the actual built lines
            if (start == 99999 && end == 99999)
            {
                DrawBuiltLines();
            }

            GL.Flush();
            oglSelf.SwapBuffers();
        }

        private void DrawBuiltLines()
        {
            GL.LineStipple(1, 0x0707);
            for (int i = 0; i < gTemp.Count; i++)
            {
                GL.Enable(EnableCap.LineStipple);
                GL.LineWidth(5);

                if (gTemp[i].mode == TrackMode.bndCurve) GL.LineStipple(1, 0x0007);
                else GL.LineStipple(1, 0x0707);

                if (i == indx)
                {
                    GL.LineWidth(8);
                    GL.Disable(EnableCap.LineStipple);
                }

                GL.Color3(0.30f, 0.97f, 0.30f);
                if (gTemp[i].mode == TrackMode.AB) GL.Color3(1.0f, 0.20f, 0.20f);
                if (gTemp[i].mode == TrackMode.bndCurve) GL.Color3(0.70f, 0.5f, 0.2f);

                GL.Begin(PrimitiveType.LineStrip);
                foreach (vec3 pts in gTemp[i].curvePts)
                {
                    GL.Vertex3(pts.easting, pts.northing, 0);
                }
                GL.End();

                GL.Disable(EnableCap.LineStipple);

                if (i == indx) GL.PointSize(16);
                else GL.PointSize(8);

                GL.Color3(1.0f, 0.75f, 0.350f);
                GL.Begin(PrimitiveType.Points);

                GL.Vertex3(gTemp[i].curvePts[0].easting,
                            gTemp[i].curvePts[0].northing,
                            0);

                GL.Color3(0.5f, 0.5f, 1.0f);
                GL.Vertex3(gTemp[i].curvePts[gTemp[i].curvePts.Count - 1].easting,
                            gTemp[i].curvePts[gTemp[i].curvePts.Count - 1].northing,
                            0);
                GL.End();
            }
        }

        private void DrawABTouchPoints()
        {
            GL.Color3(0.65, 0.650, 0.0);
            GL.PointSize(24);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(0, 0, 0);
            if (start != 99999) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[start].easting, mf.bnd.bndList[bndSelect].fenceLine[start].northing, 0);
            if (end != 99999) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[end].easting, mf.bnd.bndList[bndSelect].fenceLine[end].northing, 0);
            GL.End();

            GL.PointSize(16);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(1.0f, 0.75f, 0.350f);
            if (start != 99999) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[start].easting, mf.bnd.bndList[bndSelect].fenceLine[start].northing, 0);

            GL.Color3(0.5f, 0.5f, 1.0f);
            if (end != 99999) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[end].easting, mf.bnd.bndList[bndSelect].fenceLine[end].northing, 0);
            GL.End();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oglSelf.Refresh();

            btnMakeBoundaryCurve.Enabled = true;
            for (int i = 0; i < gTemp.Count; i++)
            {
                if (gTemp[i].mode == TrackMode.bndCurve)
                {
                    btnMakeBoundaryCurve.Enabled = false;
                    break;
                }
            }

            if (indx > -1 && gTemp[indx].mode != TrackMode.Curve)
            {
                btnALength.Enabled = false;
                btnBLength.Enabled = false;
            }
            else
            {
                btnALength.Enabled = true;
                btnBLength.Enabled = true;
            }
        }

        private void btnALength_Click(object sender, EventArgs e)
        {
            if (indx > -1 && gTemp[indx].mode == TrackMode.Curve)
            {
                //and the beginning
                vec3 start = new vec3(gTemp[indx].curvePts[0]);

                for (int i = 1; i < 50; i++)
                {
                    vec3 pt = new vec3(start);
                    pt.easting -= (Math.Sin(pt.heading) * i);
                    pt.northing -= (Math.Cos(pt.heading) * i);
                    gTemp[indx].curvePts.Insert(0, pt);
                }
            }
        }

        private void btnBLength_Click(object sender, EventArgs e)
        {
            if (indx > -1 && gTemp[indx].mode == TrackMode.Curve)
            {
                int ptCnt = gTemp[indx].curvePts.Count - 1;

                for (int i = 1; i < 50; i++)
                {
                    vec3 pt = new vec3(gTemp[indx].curvePts[ptCnt]);
                    pt.easting += (Math.Sin(pt.heading) * i);
                    pt.northing += (Math.Cos(pt.heading) * i);
                    gTemp[indx].curvePts.Add(pt);
                }
            }
        }

        private void FormABDraw_ResizeEnd(object sender, EventArgs e)
        {
            Width = (Height * 4 / 3);

            oglSelf.Height = oglSelf.Width = Height - 50;

            oglSelf.Left = 2;
            oglSelf.Top = 2;

            oglSelf.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            //58 degrees view
            GL.Viewport(0, 0, oglSelf.Width, oglSelf.Height);
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(1.01f, 1.0f, 1.0f, 20000);
            GL.LoadMatrix(ref mat);

            GL.MatrixMode(MatrixMode.Modelview);

            tlp1.Width = Width - oglSelf.Width - 4;
            tlp1.Left = oglSelf.Width;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            this.Top = (area.Height - this.Height) / 2;
            this.Left = (area.Width - this.Width) / 2;
        }

        private void oglSelf_Resize(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            //58 degrees view
            GL.Viewport(0, 0, oglSelf.Width, oglSelf.Height);

            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(1.01f, 1.0f, 1.0f, 20000);
            GL.LoadMatrix(ref mat);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void oglSelf_Load(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        }

        private void DrawSections()
        {
            int cnt, step;
            int mipmap = 8;

            GL.Color3(0.9f, 0.9f, 0.8f);

            //for every new chunk of patch
            foreach (var triList in mf.patchList)
            {
                //draw the triangle in each triangle strip
                GL.Begin(PrimitiveType.TriangleStrip);
                cnt = triList.Count;

                //if large enough patch and camera zoomed out, fake mipmap the patches, skip triangles
                if (cnt >= (mipmap))
                {
                    step = mipmap;
                    for (int i = 1; i < cnt; i += step)
                    {
                        GL.Vertex3(triList[i].easting, triList[i].northing, 0); i++;
                        GL.Vertex3(triList[i].easting, triList[i].northing, 0); i++;

                        //too small to mipmap it
                        if (cnt - i <= (mipmap + 2))
                            step = 0;
                    }
                }
                else { for (int i = 1; i < cnt; i++) GL.Vertex3(triList[i].easting, triList[i].northing, 0); }
                GL.End();
            }
        }
    }
}