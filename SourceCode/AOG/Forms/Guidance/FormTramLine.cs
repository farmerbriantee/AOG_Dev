using AOG.Classes;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormTramLine : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private bool isCancel = false;

        private int indx = -1;

        private vec2 ptA = new vec2(9999999, 9999999);
        private vec2 ptB = new vec2(9999999, 9999999);
        private vec2 ptCut = new vec2(9999999, 9999999);

        private int step = 0;

        //tramTrams
        private List<vec2> tramArr = new List<vec2>();

        private List<List<vec2>> tramList = new List<List<vec2>>();

        private List<CTrk> gTemp = new List<CTrk>();

        private int passes, startPass;

        #region Form

        public FormTramLine(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
        }

        private void FormTramLine_Load(object sender, EventArgs e)
        {
            //trams
            lblAplha.Text = ((int)(mf.tram.alpha * 100)).ToString();

            FixLabelsCurve();

            //Window Properties
            Size = Settings.User.setWindow_tramLineSize;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            this.Top = (area.Height - this.Height) / 2;
            this.Left = (area.Width - this.Width) / 2;
            FormTramLine_ResizeEnd(this, e);

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }

            ResetStartNumLabels();
            LoadAndFixLines();
        }

        private void FormTramLine_ResizeEnd(object sender, EventArgs e)
        {
            Width = (Height + 300);

            oglSelf.Height = oglSelf.Width = Height - 40;

            oglSelf.Left = 1;
            oglSelf.Top = 1;

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

        private void FormTramLine_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isCancel)
            {
                mf.tram.tramList?.Clear();
                mf.tram.tramBndOuterArr?.Clear();
                mf.tram.tramBndInnerArr?.Clear();

                mf.tram.displayMode = 0;
            }

            mf.FileSaveTram();
            mf.PanelUpdateRightAndBottom();
            mf.FixTramModeButton();

            Settings.User.setWindow_tramLineSize = Size;
            Settings.Tool.tram_alpha = mf.tram.alpha;
        }

        private void LoadAndFixLines()
        {
            //load the lines from Trks
            gTemp?.Clear();

            foreach (var track in mf.trk.gArr)
            {
                if ((track.mode == TrackMode.AB || track.mode == TrackMode.Curve) && track.isVisible)
                {
                    //default side assuming built in AB Draw - isVisible is used for side to draw
                    gTemp.Add(new CTrk(track));
                    gTemp[gTemp.Count - 1].isVisible = true;
                }
            }

            if (gTemp == null || gTemp.Count == 0)
            {
                mf.YesMessageBox(gStr.Get(gs.gsNoGuidanceLines) + "\r\n\r\n  Exiting");
                isCancel = true;
                Close();
            }
            else
            {
                indx = 0;
            }

            for (indx = 0; indx < gTemp.Count; indx++)
            {
                BuildTram();
                if (tramList[0].Count == 0)
                {
                    gTemp[indx].isVisible = !gTemp[indx].isVisible;
                    tramList.Clear();
                    tramArr.Clear();
                }
            }

            indx = 0;
            FixLabelsCurve();
            BuildTram();
        }

        private void ResetStartNumLabels()
        {
            if (cboxIsOuter.Checked)
            {
                startPass = 1;
            }
            else
            {
                startPass = 0;
            }

            if (mf.tram.tramBndOuterArr.Count > 0)
            {
                startPass = 1;
            }

            passes = 2;
            lblStartPass.Text = "Start\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
        }

        #endregion Form

        #region Building Lines

        private void btnAddLines_Click(object sender, EventArgs e)
        {
            if (tramList.Count > 0)
            {
                for (int i = 0; i < tramList.Count; i++)
                {
                    var tram = new List<vec2>(tramList[i].Count);

                    for (int j = 0; j < tramList[i].Count; j++)
                    {
                        tram.Add(new vec2(tramList[i][j]));
                    }
                    mf.tram.tramList.Add(tram);
                }
            }

            tramList?.Clear();
            tramArr?.Clear();
        }

        private void BuildTram()
        {
            if (gTemp[indx].mode == TrackMode.Curve || gTemp[indx].mode == TrackMode.AB)
            {
                BuildCurveTram();
            }
            else
            {
                mf.TimedMessageBox(2000, "Invalid Line", "Use AB LIne or Curve Only");
            }
        }

        private void BuildCurveTram()
        {
            tramList?.Clear();

            int cntr = startPass;

            double widd;

            for (int i = cntr; i <= (passes + startPass) - 1; i++)
            {
                widd = (Settings.Tool.tram_Width * 0.5) - mf.tram.halfWheelTrack;
                widd += (Settings.Tool.tram_Width * i);

                var temp = gTemp[indx].curvePts.OffsetLine(gTemp[indx].isVisible ? -widd : widd, 1.2, gTemp[indx].mode > TrackMode.Curve);

                tramArr = new List<vec2>(temp.Count);

                for (int j = 0; j < temp.Count; j++)
                {
                    //if inside the boundary, add
                    if (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(temp[j]))
                    {
                        tramArr.Add(new vec2(temp[j]));
                    }
                }

                tramList.Add(tramArr);
            }

            for (int i = cntr; i <= (passes + startPass) - 1; i++)
            {
                widd = (Settings.Tool.tram_Width * 0.5) + mf.tram.halfWheelTrack;
                widd += (Settings.Tool.tram_Width * i);

                var temp = gTemp[indx].curvePts.OffsetLine(gTemp[indx].isVisible ? -widd : widd, 1.2, gTemp[indx].mode > TrackMode.Curve);

                tramArr = new List<vec2>(temp.Count);

                for (int j = 0; j < temp.Count; j++)
                {
                    //if inside the boundary, add
                    if (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(temp[j]))
                    {
                        tramArr.Add(new vec2(temp[j]));
                    }
                }

                tramList.Add(tramArr);
            }
        }

        #endregion Building Lines

        #region OpenGL and Drawing

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {
            step++;

            Point ptt = oglSelf.PointToClient(Cursor.Position);

            int wid = oglSelf.Width;
            int halfWid = oglSelf.Width / 2;
            double scale = (double)wid * 0.903;

            //Convert to Origin in the center of window, 800 pixels
            int X = ptt.X - halfWid;
            int Y = (wid - ptt.Y - halfWid);
            vec2 plotPt = new vec2
            {
                //convert screen coordinates to field coordinates
                easting = X * mf.maxFieldDistance / scale,
                northing = Y * mf.maxFieldDistance / scale,
            };

            plotPt.easting += mf.fieldCenterX;
            plotPt.northing += mf.fieldCenterY;

            if (step == 1)
            {
                ptA = plotPt;
            }
            else if (step == 2)
            {
                ptB = plotPt;
            }
            else
            {
                ptCut = plotPt;

                bool isLeft = (ptB.easting - ptA.easting) * (ptCut.northing - ptA.northing)
                    > (ptB.northing - ptA.northing) * (ptCut.easting - ptA.easting);

                bool isIntersect = false;

                if (tramList.Count > 0)
                {
                    for (int i = 0; i < tramList.Count; i++)
                    {
                        ////check for line intersection
                        for (int j = 0; j < tramList[i].Count - 1; j++)
                        {
                            if (glm.GetLineIntersection(tramList[i][j], tramList[i][j + 1], ptA, ptB, out _, out _, out _))
                            {
                                isIntersect = true;
                                break;
                            }
                        }

                        if (isIntersect)
                        {
                            for (int h = 0; h < tramList[i].Count; h++)
                            {
                                if (isLeft)
                                {
                                    if ((ptB.easting - ptA.easting) * (tramList[i][h].northing - ptA.northing)
                                        > (ptB.northing - ptA.northing) * (tramList[i][h].easting - ptA.easting))
                                    {
                                        tramList[i].RemoveAt(h);
                                        h = -1;
                                    }
                                }
                                else
                                {
                                    if ((ptB.easting - ptA.easting) * (tramList[i][h].northing - ptA.northing)
                                        < (ptB.northing - ptA.northing) * (tramList[i][h].easting - ptA.easting))
                                    {
                                        tramList[i].RemoveAt(h);
                                        h = -1;
                                    }
                                }
                            }
                        }
                        isIntersect = false;
                    }
                }

                ptB.easting = 9999999;
                ptB.northing = 9999999;
                ptA.easting = 9999999;
                ptA.northing = 9999999;
                step = 0;
            }
        }

        private void oglSelf_Paint(object sender, PaintEventArgs e)
        {
            oglSelf.MakeCurrent();

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();                  // Reset The View

            //back the camera up
            GL.Translate(0, 0, -mf.maxFieldDistance);

            //translate to that spot in the world
            GL.Translate(-mf.fieldCenterX, -mf.fieldCenterY, 0);

            GL.LineWidth(3);

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                if (j == 0)
                    GL.Color3(1.0f, 1.0f, 1.0f);
                else
                    GL.Color3(0.62f, 0.635f, 0.635f);

                mf.bnd.bndList[j].fenceLineEar.DrawPolygon();
            }

            DrawBuiltLines();

            DrawTrams();

            DrawNewTrams();

            GL.PointSize(18);

            GL.Begin(PrimitiveType.Points);
            GL.Color3(1.0, 0, 0);
            GL.Vertex3(ptA.easting, ptA.northing, 0);
            GL.Color3(0, 1.0, 0);
            GL.Vertex3(ptB.easting, ptB.northing, 0);
            GL.End();

            if (step == 2)
            {
                GL.LineWidth(6);

                GL.Begin(PrimitiveType.Lines);
                GL.Color3(1.0, 0, 0);
                GL.Vertex3(ptA.easting, ptA.northing, 0);
                GL.Color3(0, 1.0, 0);
                GL.Vertex3(ptB.easting, ptB.northing, 0);

                GL.End();
            }

            GL.Flush();
            oglSelf.SwapBuffers();
        }

        private void DrawTrams()
        {
            GL.LineWidth(6);

            GL.Color4(0.730f, 0.52f, 0.63530f, mf.tram.alpha);

            if (mf.tram.tramList.Count > 0)
            {
                for (int i = 0; i < mf.tram.tramList.Count; i++)
                {
                    mf.tram.tramList[i].DrawPolygon(PrimitiveType.LineStrip);
                }
            }

            if (mf.tram.tramBndOuterArr.Count > 0)
            {
                GL.Color4(0.830f, 0.72f, 0.3530f, mf.tram.alpha);

                mf.tram.tramBndOuterArr.DrawPolygon(PrimitiveType.LineLoop);
                mf.tram.tramBndInnerArr.DrawPolygon(PrimitiveType.LineLoop);
            }
        }

        private void DrawNewTrams()
        {
            GL.LineWidth(2);

            GL.Color4(0.97530f, 0.972f, 0.973530f, 1.0);

            if (tramList.Count > 0)
            {
                for (int i = 0; i < tramList.Count; i++)
                {
                    tramList[i].DrawPolygon(PrimitiveType.LineStrip);
                }
            }
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
                if (gTemp[i].mode == TrackMode.AB) GL.Color3(0.9730f, 0.37f, 0.30f);
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            oglSelf.Refresh();
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

        #endregion OpenGL and Drawing

        #region Buttons and misc functions

        private void btnSelectCurve_Click(object sender, EventArgs e)
        {
            tramList?.Clear();
            tramArr?.Clear();

            if (gTemp.Count > 0)
            {
                indx++;
                if (indx > (gTemp.Count - 1)) indx = 0;
            }
            else
            {
                indx = -1;
            }

            ResetStartNumLabels();
            BuildTram();
        }

        private void btnSelectCurveBk_Click(object sender, EventArgs e)
        {
            tramList?.Clear();
            tramArr?.Clear();

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
            lblStartPass.Text = "Start\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
            BuildTram();
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

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            gTemp[indx].isVisible = !gTemp[indx].isVisible;
            ResetStartNumLabels();
            BuildTram();
        }

        private void btnDeleteAllTrams_Click(object sender, EventArgs e)
        {
            tramList?.Clear();
            tramArr?.Clear();
            mf.tram.tramList?.Clear();

            mf.tram.tramBndOuterArr?.Clear();
            mf.tram.tramBndInnerArr?.Clear();

            ResetStartNumLabels();

            cboxIsOuter.Checked = false;
            BuildTram();
        }

        private void btnCancelTouch_Click(object sender, EventArgs e)
        {
            ptB.easting = 9999999;
            ptB.northing = 9999999;
            ptA.easting = 9999999;
            ptA.northing = 9999999;
            step = 0;
        }

        private void FixLabelsCurve()
        {
            this.Text = gStr.Get(gs.gsTramLines);
            this.Text += "    Track: " + (mf.vehicle.trackWidth * glm.m2FtOrM).ToString("N2") + glm.unitsFtM;
            this.Text += "    Tram: " + (Settings.Tool.tram_Width * glm.m2FtOrM).ToString("N2") + glm.unitsFtM;
            this.Text += "    Seed: " + (Settings.Tool.toolWidth * glm.m2FtOrM).ToString("N2") + glm.unitsFtM;

            if (indx > -1 && gTemp.Count > 0)
            {
                this.Text += "   " + gTemp[indx].name;
                lblCurveSelected.Text = (indx + 1).ToString() + " / " + gTemp.Count.ToString();
            }
            else
            {
                this.Text += "   Line ***";
                lblCurveSelected.Text = "*";
            }
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            Screen myScreen = Screen.PrimaryScreen;
            Rectangle area = myScreen.WorkingArea;
            this.Height = area.Height;

            FormTramLine_ResizeEnd(this, e);
        }

        private void btnDnAlpha_Click(object sender, EventArgs e)
        {
            mf.tram.alpha -= 0.1;
            if (mf.tram.alpha < 0.2) mf.tram.alpha = 0.2;
            lblAplha.Text = ((int)(mf.tram.alpha * 100)).ToString();
        }

        private void btnUpAlpha_Click(object sender, EventArgs e)
        {
            mf.tram.alpha += 0.1;
            if (mf.tram.alpha > 1.0) mf.tram.alpha = 1.0;
            lblAplha.Text = ((int)(mf.tram.alpha * 100)).ToString();
        }

        #endregion Buttons and misc functions

        #region Outer Tram

        private void cboxIsOuter_Click(object sender, EventArgs e)
        {
            mf.tram.tramBndOuterArr?.Clear();
            mf.tram.tramBndInnerArr?.Clear();
            
            if (cboxIsOuter.Checked)//build outer tram
            {
                mf.tram.displayMode = 1;

                if (mf.bnd.bndList.Count > 0)
                {
                    var output = mf.bnd.bndList[0].fenceLine.OffsetLine(Settings.Tool.tram_Width * 0.5 - mf.tram.halfWheelTrack, 1.2, true);

                    for (int i = 0; i < output.Count; i++)
                    {
                        mf.tram.tramBndOuterArr.Add(new vec2(output[i]));
                    }

                    var output2 = mf.bnd.bndList[0].fenceLine.OffsetLine(Settings.Tool.tram_Width * 0.5 + mf.tram.halfWheelTrack, 1.2, true);

                    for (int i = 0; i < output2.Count; i++)
                    {
                        mf.tram.tramBndInnerArr.Add(new vec2(output2[i]));
                    }
                }
            }

            ResetStartNumLabels();
            BuildTram();
        }

        #endregion Outer Tram

        #region Start And Passes Controls

        private void btnUpTrams_Click(object sender, EventArgs e)
        {
            passes++;
            lblStartPass.Text = "Start\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
            BuildTram();
        }

        private void btnDnTrams_Click(object sender, EventArgs e)
        {
            passes--;
            if (passes < 1) passes = 1;
            lblStartPass.Text = "Start\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
            BuildTram();
        }

        private void btnUpStartTram_Click(object sender, EventArgs e)
        {
            startPass++;
            lblStartPass.Text = "Start\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
            BuildTram();
        }

        private void btnDnStartTram_Click(object sender, EventArgs e)
        {
            startPass--;
            if (startPass < 0) startPass = 0;
            lblStartPass.Text = "Start\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
            BuildTram();
        }

        #endregion Start And Passes Controls
    }
}