using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormBoundaryLines : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private double maxFieldX, maxFieldY, minFieldX, minFieldY,
            fieldCenterX, fieldCenterY, maxFieldDistance;

        //the tracks in temp
        private List<CTrk> gTemp = new List<CTrk>();

        private int indx = 0;

        // temp bnd list
        private List<vec3> buildList = new List<vec3>();

        public FormBoundaryLines(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
        }

        private void FormBoundaryLines_Load(object sender, EventArgs e)
        {
            LoadAndSegmentABLines();
            if (gTemp.Count < 2) return;

            CalculateMinMax();

            Size = Settings.User.setWindow_HeadAcheSize;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            this.Top = (area.Height - this.Height) / 2;
            this.Left = (area.Width - this.Width) / 2;
            FormBoundaryLines_ResizeEnd(this, e);

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }

            FixLabels();
        }

        private void FormBoundaryLines_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.User.setWindow_HeadAcheSize = Size;
        }

        private void FormBoundaryLines_ResizeEnd(object sender, EventArgs e)
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

        #region Functions

        //reverse the point built lines - both AB and Curve since AB is segmented
        private void ReverseLine(int idx)
        {
            int cnt = gTemp[idx].curvePts.Count;
            if (cnt > 0)
            {
                gTemp[idx].curvePts.Reverse();

                vec3[] arr = new vec3[cnt];
                cnt--;
                gTemp[idx].curvePts.CopyTo(arr);
                gTemp[idx].curvePts.Clear();

                gTemp[idx].heading += Math.PI;
                if (gTemp[idx].heading < 0) gTemp[idx].heading += glm.twoPI;
                if (gTemp[idx].heading > glm.twoPI) gTemp[idx].heading -= glm.twoPI;

                for (int i = 1; i < cnt; i++)
                {
                    vec3 pt3 = arr[i];
                    pt3.heading += Math.PI;
                    if (pt3.heading > glm.twoPI) pt3.heading -= glm.twoPI;
                    if (pt3.heading < 0) pt3.heading += glm.twoPI;
                    gTemp[idx].curvePts.Add(new vec3(pt3));
                }

                vec2 temp = new vec2(gTemp[idx].ptA);

                (gTemp[idx].ptA) = new vec2(gTemp[idx].ptB);
                (gTemp[idx].ptB) = new vec2(temp);
            }
        }

        //determine mins maxs of lines for OGL
        private void CalculateMinMax()
        {
            minFieldX = 9999999; minFieldY = 9999999;
            maxFieldX = -9999999; maxFieldY = -9999999;

            indx = 0;

            double x;
            double y;

            if (gTemp.Count > 0)
            {
                //each guidance Line
                for (int i = 0; i < gTemp.Count; i++)
                {
                    x = gTemp[i].ptA.easting;
                    y = gTemp[i].ptA.northing;

                    if (minFieldX > x) minFieldX = x;
                    if (maxFieldX < x) maxFieldX = x;
                    if (minFieldY > y) minFieldY = y;
                    if (maxFieldY < y) maxFieldY = y;

                    x = gTemp[i].ptB.easting;
                    y = gTemp[i].ptB.northing;

                    if (minFieldX > x) minFieldX = x;
                    if (maxFieldX < x) maxFieldX = x;
                    if (minFieldY > y) minFieldY = y;
                    if (maxFieldY < y) maxFieldY = y;

                    for (int j = 0; j < gTemp[i].curvePts.Count; j++)
                    {
                        x = gTemp[i].curvePts[j].easting;
                        y = gTemp[i].curvePts[j].northing;

                        if (minFieldX > x) minFieldX = x;
                        if (maxFieldX < x) maxFieldX = x;
                        if (minFieldY > y) minFieldY = y;
                        if (maxFieldY < y) maxFieldY = y;
                    }
                }

                if (maxFieldX == -9999999 | minFieldX == 9999999 | maxFieldY == -9999999 | minFieldY == 9999999)
                {
                    maxFieldX = 0; minFieldX = 0; maxFieldY = 0; minFieldY = 0; maxFieldDistance = 1500;
                }
                else
                {
                    //the largest distancew across field
                    double dist = Math.Abs(minFieldX - maxFieldX);
                    double dist2 = Math.Abs(minFieldY - maxFieldY);

                    if (dist > dist2) maxFieldDistance = (dist);
                    else maxFieldDistance = (dist2);

                    if (maxFieldDistance < 100) maxFieldDistance = 100;
                    if (maxFieldDistance > 5000) maxFieldDistance = 5000;
                    //lblMax.Text = ((int)maxFieldDistance).ToString();

                    fieldCenterX = (maxFieldX + minFieldX) / 2.0;
                    fieldCenterY = (maxFieldY + minFieldY) / 2.0;
                }
            }
        }

        private void LoadAndSegmentABLines()
        {
            foreach (var item in mf.trk.gArr)
            {
                if ((item.mode == TrackMode.AB || item.mode == TrackMode.Curve) && item.isVisible)
                {
                    //default side assuming built in AB Draw - isVisible is used for side to draw
                    gTemp.Add(new CTrk(item));
                }
            }

            if (gTemp.Count < 2)
            {
                mf.YesMessageBox("Need at least 2 Guidance lines" + "\r\n\r\n  Exiting");
                Close();
                return;
            }
            else
            {
                indx = 0;
            }

            //clip the extended curve to only PtA and PtB segments
            for (int k = 0; k < gTemp.Count; k++)
            {
                if (gTemp[k].mode == TrackMode.AB) continue;

                int aClose = 0, bClose = 0;
                double minDist = double.MaxValue;

                for (int i = 0; i < gTemp[k].curvePts.Count; i++)
                {
                    double dist = glm.DistanceSquared(gTemp[k].curvePts[i], gTemp[k].ptA);
                    if (dist < minDist)
                    {
                        aClose = i;
                        minDist = dist;
                    }
                }
                minDist = double.MaxValue;
                for (int i = 0; i < gTemp[k].curvePts.Count; i++)
                {
                    double dist = glm.DistanceSquared(gTemp[k].curvePts[i], gTemp[k].ptB);
                    if (dist < minDist)
                    {
                        bClose = i;
                        minDist = dist;
                    }
                }

                //make a bit longer
                try
                {
                    bClose += 2;
                    if (bClose > gTemp[k].curvePts.Count - 1) bClose = gTemp[k].curvePts.Count - 1;
                    gTemp[k].curvePts.RemoveRange(bClose, (gTemp[k].curvePts.Count - bClose));

                    aClose -= 2;
                    if (aClose < 2) aClose = 2;
                    gTemp[k].curvePts.RemoveRange(0, aClose);
                }
                catch
                {
                    Log.EventWriter("Error clipping curve lines to PtA and PtB for Boundary Lines");
                }
            }

            //divide up the AB line into segments
            for (int k = 0; k < gTemp.Count; k++)
            {
                if (gTemp[k].mode == TrackMode.Curve) continue;

                double abHeading = gTemp[k].heading;

                gTemp[k].curvePts.Add(new vec3(gTemp[k].ptA, abHeading));
                gTemp[k].curvePts.Add(new vec3(gTemp[k].ptB, abHeading));
            }
        }

        private void FixLabels()
        {
            tboxLineName.Text = indx.ToString() + " -> " + gTemp[indx].name;
            cboxIsVisible.Checked = true;
        }

        #endregion Functions

        #region OGL

        private void oglSelf_Load(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {
            Point ptt = oglSelf.PointToClient(Cursor.Position);

            //zoom out
            if (ptt.Y > oglSelf.Height * 2 / 3)
            {
                maxFieldDistance *= 1.1;
            }
            else if (ptt.Y > oglSelf.Height / 3)
            {
                CalculateMinMax();
            }
            else
            {
                maxFieldDistance *= 0.9;
            }

            FixLabels();
        }

        private void oglSelf_Paint(object sender, PaintEventArgs e)
        {
            oglSelf.MakeCurrent();

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();                  // Reset The View

            //back the camera up
            GL.Translate(0, 0, -maxFieldDistance);

            //translate to that spot in the world
            GL.Translate(-fieldCenterX, -fieldCenterY, 0);

            GL.LineWidth(2);

            DrawBuiltLines();

            DrawBoundary();

            GL.Flush();
            oglSelf.SwapBuffers();
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

        private void DrawBuiltLines()
        {
            double viz = 1.0;
            if (!cboxIsVisible.Checked)
            {
                viz = 0.4;
            }

            double fontSize = maxFieldDistance / 400;

            for (int i = 0; i < gTemp.Count; i++)
            {
                GL.PointSize(8);
                GL.LineWidth(8);

                GL.Color3(0.982 * viz, 0.52 * viz, 0.50 * viz);
                if (i == indx)
                {
                    GL.PointSize(16);
                    GL.LineWidth(12);
                    GL.Color3(0.52 * viz, 0.982 * viz, 0.50 * viz);
                }

                gTemp[i].curvePts.DrawPolygon(PrimitiveType.Points);

                int ptIndex = gTemp[i].curvePts.Count / 2 - gTemp[i].curvePts.Count / 15;
                double length = gTemp[i].curvePts.Count / 20;

                vec2 pointL = new vec2(
                gTemp[i].curvePts[ptIndex].easting + (Math.Sin(glm.PIBy2 + gTemp[i].curvePts[ptIndex].heading) * length),
                gTemp[i].curvePts[ptIndex].northing + (Math.Cos(glm.PIBy2 + gTemp[i].curvePts[ptIndex].heading) * length));

                vec2 pointR = new vec2(
                gTemp[i].curvePts[ptIndex].easting + (Math.Sin(-glm.PIBy2 + gTemp[i].curvePts[ptIndex].heading) * length),
                gTemp[i].curvePts[ptIndex].northing + (Math.Cos(-glm.PIBy2 + gTemp[i].curvePts[ptIndex].heading) * length));

                int tip = gTemp[i].curvePts.Count / 2;
                GL.Begin(PrimitiveType.LineStrip);

                GL.Vertex3(pointL.easting, pointL.northing, 0);
                GL.Vertex3(gTemp[i].curvePts[tip].easting, gTemp[i].curvePts[tip].northing, 0);
                GL.Vertex3(pointR.easting, pointR.northing, 0);

                GL.End();

                if (i == indx) GL.PointSize(24);
                else GL.PointSize(20);

                GL.Color3(1.0f * viz, 1.0f * viz, 0.0f * viz);
                GL.Begin(PrimitiveType.Points);

                GL.Vertex3(gTemp[i].curvePts[0].easting,
                            gTemp[i].curvePts[0].northing,
                            0);

                GL.Color3(0.4f * viz, 0.75f * viz, 1.0f * viz);
                GL.Vertex3(gTemp[i].curvePts[gTemp[i].curvePts.Count - 1].easting,
                            gTemp[i].curvePts[gTemp[i].curvePts.Count - 1].northing,
                            0);
                GL.End();

                GL.Enable(EnableCap.Blend);

                GL.Color3(1.0f * viz, 1.0f * viz, 0.0f * viz);
                mf.font.DrawText3D(gTemp[i].curvePts[0].easting,
                            gTemp[i].curvePts[0].northing, "A", false, fontSize);

                GL.Color3(0.4f * viz, 0.75f * viz, 1.0f * viz);
                mf.font.DrawText3D(gTemp[i].curvePts[gTemp[i].curvePts.Count - 1].easting,
                            gTemp[i].curvePts[gTemp[i].curvePts.Count - 1].northing, "B", false, fontSize);

                GL.Color3(1.0f * viz, 1.0f * viz, 1.0f * viz);

                int drawSpot = gTemp[i].curvePts.Count / 2;
                drawSpot = drawSpot * 5 / 8;

                mf.font.DrawText3D(gTemp[i].curvePts[drawSpot].easting,
                            gTemp[i].curvePts[drawSpot].northing, (i + 1).ToString(), false, fontSize);

                GL.Disable(EnableCap.Blend);
            }
        }

        private void DrawBoundary()
        {
            if (buildList.Count > 0)
            {
                GL.Color3(1.0f, 1.0f, 0.0f);
                GL.LineWidth(4);
                buildList.DrawPolygon(PrimitiveType.LineStrip);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oglSelf.Refresh();
        }
        #endregion OGL

        private void btnBuildBnd_Click(object sender, EventArgs e)
        {
            indx = 0;

            if (gTemp.Count < 2)
            {
                mf.YesMessageBox("Create Error. Is there maybe only 1 line?");
                Log.EventWriter("Build Boundary From Lines, Only 1 Line.");

                return;
            }

            int numOfLines = gTemp.Count;
            int isCross = 0;

            //test direction
            int startLine = 0;
            int crossingLine = 0;

            for (startLine = 0; startLine < numOfLines; startLine++)
            {
                crossingLine++;
                if (crossingLine > gTemp.Count - 1) crossingLine = 0;

                for (int i = gTemp[startLine].curvePts.Count / 2; i < gTemp[startLine].curvePts.Count - 2; i++)
                {
                    //look for crossing with next line
                    for (int k = 0; k < gTemp[crossingLine].curvePts.Count - 2; k++)
                    {
                        bool res = glm.GetLineIntersection(
                        gTemp[startLine].curvePts[i],
                        gTemp[startLine].curvePts[i + 1],

                        gTemp[crossingLine].curvePts[k],
                        gTemp[crossingLine].curvePts[k + 1], out _, out _, out _);
                        if (res)
                        {
                            isCross++;

                            if (isCross > 1)
                            {
                                mf.YesMessageBox("Problem with Line: " + (startLine + 1) + ". Possibly more then 1 crossing?");
                                return;
                            }
                        }
                    }
                }
                if (isCross != 1)
                {
                    mf.YesMessageBox("Direction problem or not crossing\r\n\r\nLine: " + (startLine + 1) + " and Line: " + (crossingLine + 1));
                    return;
                }
                isCross = 0;
            }

            //build the headland
            buildList?.Clear();

            int startPt = 0;
            int endPt = 0;

            startLine = 0;
            crossingLine = 0;

            for (int i = 0; i < gTemp[startLine].curvePts.Count - 1; i++)
            {
                int cntr = 0;
                crossingLine = startLine;

                //loop thru crossing lines
                for (cntr = 0; cntr < numOfLines - 1; cntr++)
                {
                    crossingLine++;
                    if (crossingLine > gTemp.Count - 1) crossingLine = 0;

                    if (crossingLine == startLine)
                    {
                        crossingLine++;
                        if (crossingLine > gTemp.Count - 1) crossingLine = 0;
                        cntr++;
                    }

                    //lines to look for crossings
                    for (int j = 0; j < gTemp[crossingLine].curvePts.Count - 2; j++)
                    {
                        bool res = glm.GetLineIntersection(
                        gTemp[startLine].curvePts[i],
                        gTemp[startLine].curvePts[i + 1],

                        gTemp[crossingLine].curvePts[j],
                        gTemp[crossingLine].curvePts[j + 1], out var crossing, out _, out _);

                        if (res)
                        {
                            isCross++;

                            if (isCross == 1)
                            {
                                buildList.Add(crossing);
                                startPt = i + 1;
                                goto nextStartPts;
                            }
                            else if (isCross == 2)
                            {
                                endPt = i;
                                for (int k = startPt; k < endPt; k++)
                                {
                                    vec3 pt = new vec3(gTemp[startLine].curvePts[k]);
                                    buildList.Add(pt);
                                }

                                startLine++;
                                if (startLine == gTemp.Count) goto theEnd;
                                crossingLine = 0;
                                i = 0;
                                isCross = 0;

                                goto nextStartPts;
                            }
                            else
                            {
                                //oops more then 2 crossings??
                            }
                        }
                    }
                }

            nextStartPts:
                if (crossingLine > gTemp.Count - 1) crossingLine = 0;
            }

        theEnd:
            cboxIsVisible.Checked = false;
            return;
        }

        #region Buttons

        private void btnCycleForward_Click(object sender, EventArgs e)
        {
            if (gTemp.Count > 0)
            {
                indx++;
                if (indx > (gTemp.Count - 1)) indx = 0;
            }
            FixLabels();
        }

        private void btnCycleBackward_Click(object sender, EventArgs e)
        {
            if (gTemp.Count > 0)
            {
                indx--;
                if (indx < 0) indx = gTemp.Count - 1;
            }

            FixLabels();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (buildList.Count > 5)
            {
                CBoundaryList newBnd = new CBoundaryList();

                newBnd.fenceLine = buildList;

                mf.bnd.AddToBoundList(newBnd, mf.bnd.bndList.Count);

                mf.FileSaveBoundary();

                Log.EventWriter("Guidance Line Boundary Created");
            }

            Close();
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            ReverseLine(indx);
        }

        private void btnDeleteBoundary_Click(object sender, EventArgs e)
        {
            buildList?.Clear();
            cboxIsVisible.Checked = true;
        }

        private void btnHeadlandOff_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBLength_Click(object sender, EventArgs e)
        {
            int ptCnt = gTemp[indx].curvePts.Count - 1;

            for (int i = 1; i < 30; i++)
            {
                vec3 pt = new vec3(gTemp[indx].curvePts[ptCnt]);
                pt.easting += (Math.Sin(pt.heading) * i);
                pt.northing += (Math.Cos(pt.heading) * i);
                gTemp[indx].curvePts.Add(pt);
            }
        }

        private void btnBShrink_Click(object sender, EventArgs e)
        {
            if (indx > -1)
            {
                if (gTemp[indx].curvePts.Count > 15)
                    gTemp[indx].curvePts.RemoveRange(gTemp[indx].curvePts.Count - 10, 10);
            }
        }

        private void btnALength_Click(object sender, EventArgs e)
        {
            if (indx > -1)
            {
                //and the beginning
                vec3 start = new vec3(gTemp[indx].curvePts[0]);

                for (int i = 1; i < 30; i++)
                {
                    vec3 pt = new vec3(start);
                    pt.easting -= (Math.Sin(pt.heading) * i);
                    pt.northing -= (Math.Cos(pt.heading) * i);
                    gTemp[indx].curvePts.Insert(0, pt);
                }
            }
        }

        private void btnAShrink_Click(object sender, EventArgs e)
        {
            if (indx > -1)
            {
                if (gTemp[indx].curvePts.Count > 15)
                    gTemp[indx].curvePts.RemoveRange(0, 10);
            }
        }

        #endregion Buttons
    }
}