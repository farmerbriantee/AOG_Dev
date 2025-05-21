using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormABDraw : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private Point fixPt;

        private bool isA = true;
        private int start = 99999, end = 99999;
        private int bndSelect = 0;
        private CTrk selectedLine;
        private bool isCancel = false;

        private bool zoomToggle;

        private double zoom = 1, sX = 0, sY = 0;

        public List<CTrk> gTemp = new List<CTrk>();

        public vec3 pint = new vec3(0.0, 1.0, 0.0);

        private bool isDrawSections = true;

        public double remoteHeading = 0.0;

        public FormABDraw(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
        }

        private void FormABDraw_Load(object sender, EventArgs e)
        {
            foreach (var item in mf.trk.gArr)
            {
                gTemp.Add(new CTrk(item));
            }

            FixLabelsCurve();

            cboxIsZoom.Checked = false;
            zoomToggle = false;

            Size = Settings.User.setWindow_abDrawSize;

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
                //load tracks from temp
                mf.trk.SetTracks(gTemp);
                mf.FileSaveTracks();

                if (selectedLine != null && selectedLine.isVisible)
                {
                    mf.trk.currTrk = selectedLine;
                }
            }
            Settings.User.setWindow_abDrawSize = Size;
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
            btnMakeAPlus.Enabled = false;
            btnEdgeAB.Enabled = false;
            btnMakeCurve.Enabled = false;
            start = 99999; end = 99999;
            isA = true;

            FixLabelsCurve();

            zoom = 1;
            sX = 0;
            sY = 0;
            zoomToggle = false;

            btnExit.Focus();
        }

        private void FixLabelsCurve()
        {
            if (selectedLine != null)
            {
                tboxNameCurve.Text = selectedLine.name;
                tboxNameCurve.Enabled = true;

                int index = gTemp.FindIndex(x => x == selectedLine);

                lblCurveSelected.Text = (index + 1).ToString() + " / " + gTemp.Count.ToString();
                cboxIsVisible.Visible = true;
                cboxIsVisible.Checked = selectedLine.isVisible;
                cboxIsVisible.Image = selectedLine.isVisible ? Properties.Resources.TrackVisible : Properties.Resources.TracksInvisible;
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
            selectedLine = mf.trk.GetNextTrack(selectedLine, gTemp, true, true);
            FixLabelsCurve();
        }

        private void btnSelectCurveBk_Click(object sender, EventArgs e)
        {
            selectedLine = mf.trk.GetNextTrack(selectedLine, gTemp, false, true);
            FixLabelsCurve();
        }

        private void cboxIsVisible_Click(object sender, EventArgs e)
        {
            selectedLine.isVisible = cboxIsVisible.Checked;
            cboxIsVisible.Image = selectedLine.isVisible ? Properties.Resources.TrackVisible : Properties.Resources.TracksInvisible;
        }

        private void btnDeleteCurve_Click(object sender, EventArgs e)
        {
            if (selectedLine != null)
            {
                gTemp.Remove(selectedLine);
                mf.trk.GetNextTrack(selectedLine, gTemp, true, true);
                FixLabelsCurve();
            }
        }

        private void btnDrawSections_Click(object sender, EventArgs e)
        {
            isDrawSections = !isDrawSections;
            //btnDrawSections.Image = isDrawSections ? Properties.Resources.MappingOn : Properties.Resources.MappingOff;
        }

        private void tboxNameCurve_Leave(object sender, EventArgs e)
        {
            if (selectedLine != null)
                selectedLine.name = tboxNameCurve.Text.Trim();
            btnExit.Focus();
        }

        private void tboxNameCurve_Enter(object sender, EventArgs e)
        {
            if (Settings.User.setDisplay_isKeyboardOn)
            {
                mf.KeyboardToText((System.Windows.Forms.TextBox)sender, this);

                if (selectedLine != null)
                    selectedLine.name = tboxNameCurve.Text.Trim();
                btnExit.Focus();
            }
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            if (selectedLine != null)
            {
                selectedLine.name += DateTime.Now.ToString(" hh:mm:ss", CultureInfo.InvariantCulture);
                FixLabelsCurve();
            }
        }

        private void btnMakeBoundaryCurve_Click(object sender, EventArgs e)
        {
            for (int q = 0; q < mf.bnd.bndList.Count; q++)
            {
                var designPtsList = new List<vec3>();

                for (int i = 0; i < mf.bnd.bndList[q].fenceLine.Count; i++)
                {
                    designPtsList.Add(new vec3(mf.bnd.bndList[q].fenceLine[i]));
                }

                int cnt = designPtsList.Count;
                if (cnt > 3)
                {
                    var track = new CTrk(TrackMode.bndCurve);

                    designPtsList.Add(new vec3(designPtsList[0]));//WUT

                    track.ptA = new vec2(designPtsList[0]);
                    track.ptB = new vec2(designPtsList[designPtsList.Count - 1]);

                    //make sure point distance isn't too big
                    mf.trk.MakePointMinimumSpacing(ref designPtsList, 1.6);
                    designPtsList.CalculateHeadings(true);

                    //create a name
                    track.name = q == 0 ? "Boundary Curve" : "Inner Boundary Curve " + q.ToString();

                    track.heading = 0;

                    //write out the Curve Points
                    track.curvePts = designPtsList;

                    gTemp.Add(track);
                    selectedLine = track;
                }
            }

            //update the arrays
            btnMakeABLine.Enabled = false;
            btnEdgeAB.Enabled = false;
            btnMakeAPlus.Enabled = false;
            btnMakeCurve.Enabled = false;
            start = 99999; end = 99999;

            FixLabelsCurve();

            btnExit.Focus();
        }

        private void btnMakeCurve_Click(object sender, EventArgs e)
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

            var designPtsList = new List<vec3>();

            for (int i = start; i < end; i++)
            {
                //calculate the point inside the boundary
                designPtsList.Add(new vec3(mf.bnd.bndList[bndSelect].fenceLine[i]));

                if (isLoop && i == mf.bnd.bndList[bndSelect].fenceLine.Count - 1)
                {
                    i = -1;
                    isLoop = false;
                    end = limit;
                }
            }

            int cnt = designPtsList.Count;
            if (cnt > 3)
            {
                var track = new CTrk(TrackMode.Curve);

                track.ptA = new vec2(designPtsList[0]);
                track.ptB = new vec2(designPtsList[designPtsList.Count - 1]);

                //make sure point distance isn't too big
                mf.trk.MakePointMinimumSpacing(ref designPtsList, 1.6);
                designPtsList.CalculateHeadings(false);

                //calculate average heading of line
                double x = 0, y = 0;

                foreach (vec3 pt in designPtsList)
                {
                    x += Math.Cos(pt.heading);
                    y += Math.Sin(pt.heading);
                }
                x /= designPtsList.Count;
                y /= designPtsList.Count;
                track.heading = Math.Atan2(y, x);
                if (track.heading < 0) track.heading += glm.twoPI;

                //build the tail extensions
                mf.trk.AddFirstLastPoints(ref designPtsList, 200);
                //mf.trk.SmoothAB(ref designPtsList, 2, false);

                //create a name
                track.name = "Cu " +
                    (Math.Round(glm.toDegrees(track.heading), 1)).ToString(CultureInfo.InvariantCulture)
                    + "\u00B0";

                //write out the Curve Points
                track.curvePts = designPtsList;

                //update the arrays
                btnMakeABLine.Enabled = false;
                btnMakeAPlus.Enabled = false;
                btnEdgeAB.Enabled = false;
                btnMakeCurve.Enabled = false;
                start = 99999; end = 99999;

                FixLabelsCurve();

                gTemp.Add(track);
                selectedLine = track;
            }

            btnExit.Focus();
        }

        private void btnMakeABLine_Click(object sender, EventArgs e)
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

            var track = new CTrk(TrackMode.AB);

            track.heading = abHead;

            //calculate the new points for the reference line and points
            track.ptA.easting = mf.bnd.bndList[bndSelect].fenceLine[end].easting;
            track.ptA.northing = mf.bnd.bndList[bndSelect].fenceLine[end].northing;

            track.ptB.easting = mf.bnd.bndList[bndSelect].fenceLine[start].easting;
            track.ptB.northing = mf.bnd.bndList[bndSelect].fenceLine[start].northing;

            //fill in the dots between A and B
            double len = glm.Distance(track.ptA, track.ptB);
            if (len < 20)
            {
                track.ptB.easting = track.ptA.easting + (Math.Sin(abHead) * 30);
                track.ptB.northing = track.ptA.northing + (Math.Cos(abHead) * 30);
            }

            track.curvePts.Add(new vec3(track.ptA, abHead));
            track.curvePts.Add(new vec3(track.ptB, abHead));

            //build the tail extensions
            mf.trk.AddFirstLastPoints(ref track.curvePts, 100);

            //create a name
            track.name = "AB: " +
                Math.Round(glm.toDegrees(track.heading), 1).ToString(CultureInfo.InvariantCulture) + "\u00B0";

            //clean up gui
            btnMakeABLine.Enabled = false;
            btnMakeAPlus.Enabled = false;
            btnEdgeAB.Enabled = false;
            btnMakeCurve.Enabled = false;

            start = 99999; end = 99999;

            gTemp.Add(track);
            selectedLine = track;
            FixLabelsCurve();
        }

        private void btnMakeAPlus_Click(object sender, EventArgs e)
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

            var track = new CTrk(TrackMode.AB);

            var form = new FormABDrawHeading(this,abHead);
            form.ShowDialog(this);

            double len = glm.Distance(mf.bnd.bndList[bndSelect].fenceLine[end], mf.bnd.bndList[bndSelect].fenceLine[start]);
            
            if (len < 20)
            {
                len = 30;
            }
            if (remoteHeading != -1)
            {
                abHead = remoteHeading;
            }

            //calculate the new points for the reference line and points
            track.ptA.easting = mf.bnd.bndList[bndSelect].fenceLine[start].easting;
            track.ptA.northing = mf.bnd.bndList[bndSelect].fenceLine[start].northing;

            track.ptB.easting = track.ptA.easting - (Math.Sin(abHead) * len);
            track.ptB.northing = track.ptA.northing - (Math.Cos(abHead) * len);

            track.heading = abHead;

            track.curvePts.Add(new vec3(track.ptA, abHead));
            track.curvePts.Add(new vec3(track.ptB, abHead));

            //build the tail extensions
            mf.trk.AddFirstLastPoints(ref track.curvePts, 100);

            //create a name
            track.name = "AB: " +
                Math.Round(glm.toDegrees(track.heading), 1).ToString(CultureInfo.InvariantCulture) + "\u00B0";

            //clean up gui
            btnMakeABLine.Enabled = false;
            btnMakeAPlus.Enabled = false;
            btnEdgeAB.Enabled = false;
            btnMakeCurve.Enabled = false;

            start = 99999; end = 99999;

            gTemp.Add(track);
            selectedLine = track;
            FixLabelsCurve();
        }

        private void btnEdgeAB_Click(object sender, EventArgs e)
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

            var designPtsList = new List<vec3>();

            for (int i = start; i < end; i++)
            {
                //calculate the point inside the boundary
                designPtsList.Add(new vec3(mf.bnd.bndList[bndSelect].fenceLine[i]));

                if (isLoop && i == mf.bnd.bndList[bndSelect].fenceLine.Count - 1)
                {
                    i = -1;
                    isLoop = false;
                    end = limit;
                }
            }

            //calculate the AB Heading
            double abHead = Math.Atan2(
                mf.bnd.bndList[bndSelect].fenceLine[end].easting - mf.bnd.bndList[bndSelect].fenceLine[start].easting,
                mf.bnd.bndList[bndSelect].fenceLine[end].northing - mf.bnd.bndList[bndSelect].fenceLine[start].northing);
            if (abHead < 0) abHead += glm.twoPI;

            var track = new CTrk(TrackMode.AB);

            var form = new FormABDrawHeading(this, abHead);
            form.ShowDialog(this);

            double len = glm.Distance(mf.bnd.bndList[bndSelect].fenceLine[end], mf.bnd.bndList[bndSelect].fenceLine[start]);

            if (len < 20)
            {
                len = 30;
            }
            if (remoteHeading != -1)
            {
                abHead = remoteHeading;
            }

            //calculate the new points for the reference line and points
            track.ptA.easting = mf.bnd.bndList[bndSelect].fenceLine[start].easting;
            track.ptA.northing = mf.bnd.bndList[bndSelect].fenceLine[start].northing;

            track.ptB.easting = track.ptA.easting + (Math.Sin(abHead) * len);
            track.ptB.northing = track.ptA.northing + (Math.Cos(abHead) * len);

            track.heading = abHead;

            //get the pivot distance from currently active AB segment   ///////////  Pivot  ////////////
            double dx = track.ptB.easting - track.ptA.easting;
            double dy = track.ptB.northing - track.ptA.northing;
            if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dy) < Double.Epsilon)
                return;

            double maxDistance = double.MaxValue;

            for (int i = 0; i < mf.bnd.bndList[bndSelect].fenceLine.Count; i++)
            {
                //how far from current AB Line is fix
                double distanceFromBnd = ((dy * mf.bnd.bndList[bndSelect].fenceLine[i].easting) - (dx * mf.bnd.bndList[bndSelect].fenceLine[i].northing) + (track.ptB.easting
                            * track.ptA.northing) - (track.ptB.northing * track.ptA.easting))
                                / Math.Sqrt((dy * dy) + (dx * dx));
                if (distanceFromBnd > 0) continue;

                if (distanceFromBnd < maxDistance)
                    maxDistance = distanceFromBnd;
            }

            track.ptA.easting += (Math.Sin(abHead + glm.PIBy2) * maxDistance);
            track.ptB.easting += (Math.Sin(abHead + glm.PIBy2) * maxDistance);
            track.ptA.northing += (Math.Cos(abHead + glm.PIBy2) * maxDistance);
            track.ptB.northing += (Math.Cos(abHead + glm.PIBy2) * maxDistance);

            track.curvePts.Add(new vec3(track.ptA, abHead));
            track.curvePts.Add(new vec3(track.ptB, abHead));

            //build the tail extensions
            mf.trk.AddFirstLastPoints(ref track.curvePts, 100);

            //create a name
            track.name = "AB: " +
                Math.Round(glm.toDegrees(track.heading), 1).ToString(CultureInfo.InvariantCulture) + "\u00B0";

            //clean up gui
            btnMakeABLine.Enabled = false;
            btnMakeAPlus.Enabled = false;
            btnMakeCurve.Enabled = false;
            btnEdgeAB.Enabled = false;

            start = 99999; end = 99999;

            gTemp.Add(track);
            selectedLine = track;
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
            btnMakeAPlus.Enabled = false;
            btnMakeCurve.Enabled = false;
            btnEdgeAB.Enabled = false;

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
                btnEdgeAB.Enabled = true;
                btnMakeAPlus.Enabled = true;
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

            if (isDrawSections)
            {
                GL.Color3(0.2f, 0.2f, 0.2f);

                //for every new chunk of patch
                foreach (var triList in mf.patchList)
                {
                    triList.DrawPolygon(8, 1, PrimitiveType.TriangleStrip);
                }
            }

            GL.LineWidth(3);

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                if (j == bndSelect)
                    GL.Color3(1.0f, 1.0f, 1.0f);
                else
                    GL.Color3(0.62f, 0.635f, 0.635f);

                mf.bnd.bndList[j].fenceLineEar.DrawPolygon(PrimitiveType.LineLoop);
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
            foreach (var track in gTemp)
            {
                GL.Enable(EnableCap.LineStipple);
                GL.LineWidth(5);

                if (track.mode == TrackMode.bndCurve) GL.LineStipple(1, 0x0007);
                else GL.LineStipple(1, 0x0707);

                if (track == selectedLine)
                {
                    GL.LineWidth(8);
                    GL.Disable(EnableCap.LineStipple);
                }

                GL.Color3(0.30f, 0.97f, 0.30f);
                if (track.mode == TrackMode.AB) GL.Color3(1.0f, 0.20f, 0.20f);
                if (track.mode == TrackMode.bndCurve) GL.Color3(0.70f, 0.5f, 0.2f);

                track.curvePts.DrawPolygon(PrimitiveType.LineStrip);

                GL.Disable(EnableCap.LineStipple);

                if (track == selectedLine) GL.PointSize(16);
                else GL.PointSize(8);

                GL.Color3(1.0f, 0.75f, 0.350f);
                GL.Begin(PrimitiveType.Points);

                GL.Vertex3(track.curvePts[0].easting, track.curvePts[0].northing, 0);

                GL.Color3(0.5f, 0.5f, 1.0f);
                GL.Vertex3(track.curvePts[track.curvePts.Count - 1].easting,
                            track.curvePts[track.curvePts.Count - 1].northing,
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
            foreach (var track in gTemp)
            {
                if (track.mode == TrackMode.bndCurve)
                {
                    btnMakeBoundaryCurve.Enabled = false;
                    break;
                }
            }

            if (selectedLine != null && selectedLine.mode != TrackMode.Curve)
            {
                btnALength.Enabled = true;
                btnBLength.Enabled = true;
            }
            else
            {
                btnALength.Enabled = false;
                btnBLength.Enabled = false;
            }
        }

        private void btnALength_Click(object sender, EventArgs e)
        {
            if (selectedLine != null && (selectedLine.mode == TrackMode.Curve || selectedLine.mode == TrackMode.AB))
            {
                //and the beginning
                vec3 start = new vec3(selectedLine.curvePts[0]);

                for (int i = 1; i < 50; i++)
                {
                    vec3 pt = new vec3(start);
                    pt.easting -= (Math.Sin(pt.heading) * i);
                    pt.northing -= (Math.Cos(pt.heading) * i);
                    selectedLine.curvePts.Insert(0, pt);
                }

                selectedLine.ptA = new vec2(selectedLine.curvePts[0]);
            }
        }

        private void btnBLength_Click(object sender, EventArgs e)
        {
            if (selectedLine != null && (selectedLine.mode == TrackMode.Curve || selectedLine.mode == TrackMode.AB))
            {
                int ptCnt = selectedLine.curvePts.Count - 1;

                for (int i = 1; i < 50; i++)
                {
                    vec3 pt = new vec3(selectedLine.curvePts[ptCnt]);
                    pt.easting += (Math.Sin(pt.heading) * i);
                    pt.northing += (Math.Cos(pt.heading) * i);
                    selectedLine.curvePts.Add(pt);
                }

                selectedLine.ptB = new vec2(selectedLine.curvePts[selectedLine.curvePts.Count - 1]);
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
    }
}