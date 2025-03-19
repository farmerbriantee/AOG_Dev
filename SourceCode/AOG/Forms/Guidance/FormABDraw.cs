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
        private int bndSelect = 0;
        private CTrk selectedLine;
        private bool isCancel = false;

        private bool zoomToggle;

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
                mf.trk.gArr = gTemp;
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
            btnDrawSections.Image = isDrawSections ? Properties.Resources.MappingOn : Properties.Resources.MappingOff;
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
                    mf.trk.CalculateHeadings(ref designPtsList);

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
            btnMakeCurve.Enabled = false;
            start = 99999; end = 99999;

            FixLabelsCurve();

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
                mf.trk.CalculateHeadings(ref designPtsList);

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
                mf.trk.AddFirstLastPoints(ref designPtsList, 100);
                //mf.trk.SmoothAB(ref designPtsList, 2, false);
                mf.trk.CalculateHeadings(ref designPtsList);

                //create a name
                track.name = "Cu " +
                    (Math.Round(glm.toDegrees(track.heading), 1)).ToString(CultureInfo.InvariantCulture)
                    + "\u00B0";

                //write out the Curve Points
                track.curvePts = designPtsList;

                //update the arrays
                btnMakeABLine.Enabled = false;
                btnMakeCurve.Enabled = false;
                start = 99999; end = 99999;

                FixLabelsCurve();

                gTemp.Add(track);
                selectedLine = track;
            }

            btnExit.Focus();
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

            var track = new CTrk(TrackMode.AB);

            track.heading = abHead;
            double hsin = Math.Sin(abHead);
            double hcos = Math.Cos(abHead);

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
            len = glm.Distance(track.ptA, track.ptB);

            vec3 P1 = new vec3();
            for (int i = 0; i < (int)len; i += 1)
            {
                P1.easting = (hsin * i) + track.ptA.easting;
                P1.northing = (hcos * i) + track.ptA.northing;
                P1.heading = abHead;
                track.curvePts.Add(P1);
            }

            //build the tail extensions
            mf.trk.AddFirstLastPoints(ref track.curvePts, 50);

            //create a name
            track.name = "AB: " +
                Math.Round(glm.toDegrees(track.heading), 1).ToString(CultureInfo.InvariantCulture) + "\u00B0";

            //clean up gui
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;

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

                GL.Begin(PrimitiveType.LineStrip);
                foreach (vec3 pts in track.curvePts)
                {
                    GL.Vertex3(pts.easting, pts.northing, 0);
                }
                GL.End();

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
            if (selectedLine != null && selectedLine.mode == TrackMode.Curve)
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
            }
        }

        private void btnBLength_Click(object sender, EventArgs e)
        {
            if (selectedLine != null && selectedLine.mode == TrackMode.Curve)
            {
                int ptCnt = selectedLine.curvePts.Count - 1;

                for (int i = 1; i < 50; i++)
                {
                    vec3 pt = new vec3(selectedLine.curvePts[ptCnt]);
                    pt.easting += (Math.Sin(pt.heading) * i);
                    pt.northing += (Math.Cos(pt.heading) * i);
                    selectedLine.curvePts.Add(pt);
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