using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormHeadLine : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private Point fixPt;

        private bool isA = true;
        private int start = -1, end = -1;
        private int bndSelect = 0;
        private TrackMode mode = TrackMode.None;
        public List<vec3> sliceArr = new List<vec3>();
        public List<vec3> backupList = new List<vec3>();

        private bool zoomToggle;

        private double zoom = 1, sX = 0, sY = 0;

        public vec3 pint = new vec3(0.0, 1.0, 0.0);

        public FormHeadLine(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
        }

        private void FormHeadLine_Load(object sender, EventArgs e)
        {
            this.Text = "1: Set distance, 2: Tap Build, 3: Create Clip Lines";
            //label3.Text = glm.unitsFtM +"       Tool: ";

            lblToolWidth.Text = "( " + glm.unitsFtM + " )           Tool: " + ((Settings.Tool.toolWidth - Settings.Tool.overlap) * glm.m2FtOrM).ToString("N1") + " " + glm.unitsFtM;

            nudSetDistance.Value = 0;
            start = -1; end = -1;
            isA = true;
            sliceArr?.Clear();
            backupList?.Clear();

            btnSlice.Enabled = false;

            if (mf.bnd.bndList[0].hdLine.Count == 0)
            {
                if (mf.bnd.bndList[0].fenceLine.Count > 0)
                {
                    for (int i = 0; i < mf.bnd.bndList[0].fenceLine.Count; i++)
                    {
                        mf.bnd.bndList[0].hdLine.Add(new vec3(mf.bnd.bndList[0].fenceLine[i]));
                    }
                }
            }
            else
            {
                //make sure point distance isn't too big
                mf.trk.MakePointMinimumSpacing(ref mf.bnd.bndList[0].hdLine, 1.2);
                mf.bnd.bndList[0].hdLine.CalculateHeadings(true);
            }

            cboxIsZoom.Checked = false;

            Size = Settings.User.setWindow_HeadlineSize;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            this.Top = (area.Height - this.Height) / 2;
            this.Left = (area.Width - this.Width) / 2;
            FormHeadLine_ResizeEnd(this, e);

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormHeadLine_ResizeEnd(object sender, EventArgs e)
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

        private void FormHeadLine_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.User.setWindow_HeadlineSize = Size;
        }

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {
            if (nudSetDistance.Value == 0 && rbtnCurve.Checked)
            {
                mf.TimedMessageBox(3000, "Distance Error", "Distance Set to 0, Nothing to Move");
                Log.EventWriter("Headland, Distance=0, Can't Move");
                return;
            }
            sliceArr?.Clear();

            Point ptt = oglSelf.PointToClient(Cursor.Position);

            int wid = oglSelf.Width;
            int halfWid = oglSelf.Width / 2;
            double scale = (double)wid * 0.903;

            if (cboxIsZoom.Checked && !zoomToggle)
            {
                sX = ((halfWid - (double)ptt.X) / wid) * 1.1;
                sY = ((halfWid - (double)ptt.Y) / -wid) * 1.1;
                zoom = 0.1;
                zoomToggle = true;
                return;
            }

            //Convert to Origin in the center of window, 800 pixels
            fixPt.X = ptt.X - halfWid;
            fixPt.Y = (wid - ptt.Y - halfWid);
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

            zoomToggle = false;
            zoom = 1;
            sX = 0;
            sY = 0;

            double minDistA = double.MaxValue;
            int idx = -1;
            for (int j = isA ? 0 : bndSelect; j < mf.bnd.bndList.Count; j++)
            {
                for (int i = 0; i < mf.bnd.bndList[j].fenceLine.Count; i++)
                {
                    double dist = glm.DistanceSquared(pint, mf.bnd.bndList[j].fenceLine[i]);

                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        bndSelect = j;
                        idx = i;
                    }
                }
                if (!isA) break;
            }

            if (isA)
            {
                start = idx;
                end = -1;
                isA = false;
            }
            else
            {
                end = idx;
                isA = true;

                //build the lines
                if (rbtnCurve.Checked)
                {
                    bool isLoop = false;
                    int limit = end;

                    if ((Math.Abs(start - end)) > (mf.bnd.bndList[bndSelect].fenceLine.Count * 0.5))
                    {
                        if (start < end)
                        {
                            (start, end) = (end, start);
                        }

                        isLoop = true;
                        if (start < end)
                        {
                            limit = end;
                            end = 0;
                        }
                        else
                        {
                            limit = end;
                            end = mf.bnd.bndList[bndSelect].fenceLine.Count;
                        }
                    }
                    else
                    {
                        if (start > end)
                        {
                            (start, end) = (end, start);
                        }
                    }

                    sliceArr?.Clear();
                    vec3 pt3 = new vec3();

                    if (start < end)
                    {
                        for (int i = start; i <= end; i++)
                        {
                            //calculate the point inside the boundary
                            pt3 = mf.bnd.bndList[bndSelect].fenceLine[i];
                            sliceArr.Add(pt3);

                            if (isLoop && i == mf.bnd.bndList[bndSelect].fenceLine.Count - 1)
                            {
                                i = -1;
                                isLoop = false;
                                end = limit;
                            }
                        }
                    }
                    else
                    {
                        for (int i = start; i >= end; i--)
                        {
                            //calculate the point inside the boundary
                            pt3 = mf.bnd.bndList[bndSelect].fenceLine[i];
                            sliceArr.Add(pt3);

                            if (isLoop && i == 0)
                            {
                                i = mf.bnd.bndList[bndSelect].fenceLine.Count - 1;
                                isLoop = false;
                                end = limit;
                            }
                        }
                    }

                    int ptCnt = sliceArr.Count - 1;

                    if (ptCnt > 0)
                    {
                        //who knows which way it actually goes
                        sliceArr.CalculateHeadings(false);

                        for (int i = 1; i < 30; i++)
                        {
                            vec3 pt = new vec3(sliceArr[ptCnt]);
                            pt.easting += (Math.Sin(pt.heading) * i);
                            pt.northing += (Math.Cos(pt.heading) * i);
                            sliceArr.Add(pt);
                        }

                        vec3 stat = new vec3(sliceArr[0]);

                        for (int i = 1; i < 30; i++)
                        {
                            vec3 pt = new vec3(stat);
                            pt.easting -= (Math.Sin(pt.heading) * i);
                            pt.northing -= (Math.Cos(pt.heading) * i);
                            sliceArr.Insert(0, pt);
                        }

                        mode = TrackMode.Curve;
                    }
                    else
                    {
                        start = -1; end = -1;
                        return;
                    }

                    //update the arrays
                    start = -1; end = -1;

                    btnExit.Focus();
                }
                else if (rbtnLine.Checked)
                {
                    if ((Math.Abs(start - end)) > (mf.bnd.bndList[bndSelect].fenceLine.Count * 0.5))
                    {
                        if (start < end)
                        {
                            (start, end) = (end, start);
                        }
                    }
                    else
                    {
                        if (start > end)
                        {
                            (start, end) = (end, start);
                        }
                    }

                    vec3 ptA = new vec3(mf.bnd.bndList[bndSelect].fenceLine[start]);
                    vec3 ptB = new vec3(mf.bnd.bndList[bndSelect].fenceLine[end]);

                    //calculate the AB Heading
                    double abHead = Math.Atan2(
                        mf.bnd.bndList[bndSelect].fenceLine[end].easting - mf.bnd.bndList[bndSelect].fenceLine[start].easting,
                        mf.bnd.bndList[bndSelect].fenceLine[end].northing - mf.bnd.bndList[bndSelect].fenceLine[start].northing);
                    if (abHead < 0) abHead += glm.twoPI;

                    sliceArr?.Clear();

                    ptA.heading = abHead;
                    ptB.heading = abHead;

                    for (int i = 0; i <= (int)(glm.Distance(ptA, ptB)); i++)
                    {
                        vec3 ptC = new vec3(ptA)
                        {
                            easting = (Math.Sin(abHead) * i) + ptA.easting,
                            northing = (Math.Cos(abHead) * i) + ptA.northing,
                            heading = abHead
                        };
                        sliceArr.Add(ptC);
                    }

                    int ptCnt = sliceArr.Count - 1;

                    for (int i = 1; i < 30; i++)
                    {
                        vec3 pt = new vec3(sliceArr[ptCnt]);
                        pt.easting += (Math.Sin(pt.heading) * i);
                        pt.northing += (Math.Cos(pt.heading) * i);
                        sliceArr.Add(pt);
                    }

                    vec3 stat = new vec3(sliceArr[0]);

                    for (int i = 1; i < 30; i++)
                    {
                        vec3 pt = new vec3(stat);
                        pt.easting -= (Math.Sin(pt.heading) * i);
                        pt.northing -= (Math.Cos(pt.heading) * i);
                        sliceArr.Insert(0, pt);
                    }

                    mode = TrackMode.AB;

                    start = -1; end = -1;
                }

                //Move the line
                if (nudSetDistance.Value != 0)
                    SetLineDistance();

                btnSlice.Enabled = true;
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

            //draw all the boundaries
            GL.LineWidth(4);

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                if (j == bndSelect)
                    GL.Color3(0.75f, 0.75f, 0.750f);
                else
                    GL.Color3(0.0f, 0.25f, 0.10f);
                mf.bnd.bndList[j].fenceLine.DrawPolygon(PrimitiveType.LineLoop);
            }

            //the vehicle
            GL.PointSize(16.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(0.95f, 0.190f, 0.20f);
            GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0.0);
            GL.End();

            //draw the line building graphics
            if (start != -1 || end != -1) DrawABTouchLine();

            //draw the actual built lines
            //if (start == -1 && end == -1)
            {
                DrawBuiltLines();
            }

            GL.Disable(EnableCap.Blend);

            GL.Flush();
            oglSelf.SwapBuffers();
        }

        private void DrawBuiltLines()
        {
            //GL.LineWidth(8);
            //GL.Color3(0.03f, 0.0f, 0.150f);
            //GL.Begin(PrimitiveType.LineLoop);

            //for (int i = 0; i < mf.bnd.buildList[0].hdLine.Count; i++)
            //{
            //    GL.Vertex3(mf.bnd.buildList[0].hdLine[i].easting, mf.bnd.buildList[0].hdLine[i].northing, 0);
            //}
            //GL.End();

            GL.LineWidth(8);
            GL.Color3(0.943f, 0.9083f, 0.09150f);
            mf.bnd.bndList[0].hdLine.DrawPolygon(PrimitiveType.LineLoop);

            if (sliceArr.Count > 0)
            {
                //GL.Enable(EnableCap.LineStipple);
                //GL.LineStipple(1, 0x7070);
                GL.PointSize(8);

                if (mode == TrackMode.AB)
                {
                    GL.Color3(0.95f, 0.09f, 0.0f);
                }
                else
                {
                    GL.Color3(0.13f, 0.95f, 0.020f);
                }

                GL.Begin(PrimitiveType.LineStrip);
                foreach (vec3 item in sliceArr)
                {
                    GL.Vertex3(item.easting, item.northing, 0);
                }
                GL.End();

                int cnt = sliceArr.Count - 1;
                GL.PointSize(24);
                GL.Color3(1.0f, 0.6f, 0.3f);
                GL.Begin(PrimitiveType.Points);
                GL.Vertex3(sliceArr[0].easting, sliceArr[0].northing, 0);
                GL.Color3(0.5f, 0.73f, 0.99f);
                GL.Vertex3(sliceArr[cnt].easting, sliceArr[cnt].northing, 0);
                GL.End();
            }
        }

        private void DrawABTouchLine()
        {
            GL.PointSize(24);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(0, 0, 0);
            if (start != -1) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[start].easting, mf.bnd.bndList[bndSelect].fenceLine[start].northing, 0);
            if (end != -1) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[end].easting, mf.bnd.bndList[bndSelect].fenceLine[end].northing, 0);
            GL.End();

            GL.PointSize(18);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(1.0f, 0.75f, 0.350f);
            if (start != -1) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[start].easting, mf.bnd.bndList[bndSelect].fenceLine[start].northing, 0);

            GL.Color3(0.5f, 0.75f, 1.0f);
            if (end != -1) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[end].easting, mf.bnd.bndList[bndSelect].fenceLine[end].northing, 0);
            GL.End();
        }

        private void oglSelf_Load(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            oglSelf.Refresh();
            if (sliceArr.Count == 0)
            {
                btnSlice.Enabled = false;
                btnALength.Enabled = false;
                btnBLength.Enabled = false;
                btnAShrink.Enabled = false;
                btnBShrink.Enabled = false;
            }
            else
            {
                btnSlice.Enabled = true;
                btnBLength.Enabled = true;
                btnALength.Enabled = true;
                btnAShrink.Enabled = true;
                btnBShrink.Enabled = true;
            }

            if (backupList.Count == 0) btnUndo.Enabled = false; else btnUndo.Enabled = true;
            if (nudSetDistance.Value == 0) btnBndLoop.Enabled = false; else btnBndLoop.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            vec3[] hdArr;

            if (mf.bnd.bndList[0].hdLine.Count > 0)
            {
                hdArr = new vec3[mf.bnd.bndList[0].hdLine.Count];
                mf.bnd.bndList[0].hdLine.CopyTo(hdArr);
                mf.bnd.bndList[0].hdLine?.Clear();

                //does headland control sections
                mf.bnd.isSectionControlledByHeadland = true;

                //middle points
                for (int i = 1; i < hdArr.Length; i++)
                {
                    hdArr[i - 1].heading = Math.Atan2(hdArr[i - 1].easting - hdArr[i].easting, hdArr[i - 1].northing - hdArr[i].northing);
                    if (hdArr[i].heading < 0) hdArr[i].heading += glm.twoPI;
                }

                double delta = 0;
                for (int i = 0; i < hdArr.Length; i++)
                {
                    if (i == 0)
                    {
                        mf.bnd.bndList[0].hdLine.Add(new vec3(hdArr[i].easting, hdArr[i].northing, hdArr[i].heading));
                        continue;
                    }
                    delta += (hdArr[i - 1].heading - hdArr[i].heading);

                    if (Math.Abs(delta) > 0.005)
                    {
                        vec3 pt = new vec3(hdArr[i].easting, hdArr[i].northing, hdArr[i].heading);

                        mf.bnd.bndList[0].hdLine.Add(pt);
                        delta = 0;
                    }
                }
                vec3 ptEnd = new vec3(hdArr[hdArr.Length - 1].easting, hdArr[hdArr.Length - 1].northing, hdArr[hdArr.Length - 1].heading);

                mf.bnd.bndList[0].hdLine.Add(ptEnd);

                //triangulate headland area
                CPolygon hdLinePolygon = new CPolygon(mf.bnd.bndList[0].hdLine.ToArray());
                mf.bnd.bndList[0].hdLineTriangleList = hdLinePolygon.Triangulate();
                mf.bnd.isHeadlandOn = true;
            }

            mf.FileSaveHeadland();
            Close();
        }

        private void SetLineDistance()
        {
            sliceArr = sliceArr.OffsetLine(nudSetDistance.Value, 1, true);
        }

        public List<int> crossings = new List<int>(1);

        private void btnBndLooPGN_Click(object sender, EventArgs e)
        {
            if (nudSetDistance.Value == 0)
            {
                int ptCount = mf.bnd.bndList[0].fenceLine.Count;

                mf.bnd.bndList[0].hdLine?.Clear();

                for (int i = 0; i < ptCount; i++)
                {
                    mf.bnd.bndList[0].hdLine.Add(new vec3(mf.bnd.bndList[0].fenceLine[i]));
                }
            }
            else
            {
                double moveDist = nudSetDistance.Value;

                var desList = mf.bnd.bndList[0].fenceLine.OffsetLine(moveDist, 1, true);

                int cnt = desList.Count;
                if (cnt > 3)
                {
                    //make sure point distance isn't too big
                    mf.trk.MakePointMinimumSpacing(ref desList, 1.2);
                    desList.CalculateHeadings(true);

                    //write out the Points
                    mf.bnd.bndList[0].hdLine = desList;
                }
            }

            mf.FileSaveHeadland();
        }

        private void btnSlice_Click(object sender, EventArgs e)
        {
            int startBnd = 0, endBnd = 0, startLine = 0, endLine = 0;
            int isStart = 0;

            if (sliceArr.Count == 0) return;

            //save a backup
            backupList?.Clear();
            foreach (var item in mf.bnd.bndList[0].hdLine)
            {
                backupList.Add(item);
            }

            for (int i = 0; i < sliceArr.Count - 2; i++)
            {
                for (int k = 0; k < mf.bnd.bndList[0].hdLine.Count - 2; k++)
                {
                    bool res = glm.GetLineIntersection(
                    sliceArr[i],
                    sliceArr[i + 1],
                    mf.bnd.bndList[0].hdLine[k],
                    mf.bnd.bndList[0].hdLine[k + 1], out _, out _, out _);
                    if (res)
                    {
                        if (isStart == 0)
                        {
                            startBnd = k + 1;
                            startLine = i + 1;
                        }
                        else
                        {
                            endBnd = k + 1;
                            endLine = i;
                        }
                        isStart++;
                    }
                }
            }

            if (isStart < 2)
            {
                mf.TimedMessageBox(2000, "Error", "Crossings not Found");
                Log.EventWriter("Headland, Crossings Not Found");

                return;
            }

            var desList = new List<vec3>();

            //overlaps start finish
            if ((Math.Abs(startBnd - endBnd)) > (mf.bnd.bndList[bndSelect].fenceLine.Count * 0.5))
            {
                if (startBnd < endBnd)
                {
                    (startBnd, endBnd) = (endBnd, startBnd);
                }

                //first bnd segment
                for (int i = endBnd; i < startBnd; i++)
                {
                    desList.Add(mf.bnd.bndList[0].hdLine[i]);
                }

                for (int i = startLine; i < endLine; i++)
                {
                    desList.Add(sliceArr[i]);
                }

                mf.bnd.bndList[0].hdLine = desList;
            }
            // completely in between start finish
            else
            {
                if (startBnd > endBnd)
                {
                    (startBnd, endBnd) = (endBnd, startBnd);
                }

                //first bnd segment
                for (int i = 0; i < startBnd; i++)
                {
                    desList.Add(mf.bnd.bndList[0].hdLine[i]);
                }

                //line segment
                for (int i = startLine; i < endLine; i++)
                {
                    desList.Add(sliceArr[i]);
                }

                //final bnd segment
                for (int i = endBnd; i < mf.bnd.bndList[0].hdLine.Count; i++)
                {
                    desList.Add(mf.bnd.bndList[0].hdLine[i]);
                }

                mf.bnd.bndList[0].hdLine = desList;
            }

            sliceArr?.Clear();
        }

        private void btnDeletePoints_Click(object sender, EventArgs e)
        {
            start = -1; end = -1;
            isA = true;
            sliceArr?.Clear();
            backupList?.Clear();
            mf.bnd.bndList[0].hdLine?.Clear();

            int ptCount = mf.bnd.bndList[0].fenceLine.Count;

            for (int i = 0; i < ptCount; i++)
            {
                mf.bnd.bndList[0].hdLine.Add(new vec3(mf.bnd.bndList[0].fenceLine[i]));
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            mf.bnd.bndList[0].hdLine?.Clear();
            foreach (var item in backupList)
            {
                mf.bnd.bndList[0].hdLine.Add(item);
            }
            backupList?.Clear();
        }

        private void cboxToolWidths_SelectedIndexChanged(object sender, EventArgs e)
        {
            nudSetDistance.Value = Math.Round((Settings.Tool.toolWidth - Settings.Tool.overlap) * cboxToolWidths.SelectedIndex, 1);
        }

        private void btnALength_Click(object sender, EventArgs e)
        {
            if (sliceArr.Count > 0)
            {
                //and the beginning
                vec3 start = new vec3(sliceArr[0]);

                for (int i = 1; i < 10; i++)
                {
                    vec3 pt = new vec3(start);
                    pt.easting -= (Math.Sin(pt.heading) * i);
                    pt.northing -= (Math.Cos(pt.heading) * i);
                    sliceArr.Insert(0, pt);
                }
            }
        }

        private void btnBLength_Click(object sender, EventArgs e)
        {
            if (sliceArr.Count > 0)
            {
                int ptCnt = sliceArr.Count - 1;

                for (int i = 1; i < 10; i++)
                {
                    vec3 pt = new vec3(sliceArr[ptCnt]);
                    pt.easting += (Math.Sin(pt.heading) * i);
                    pt.northing += (Math.Cos(pt.heading) * i);
                    sliceArr.Add(pt);
                }
            }
        }

        private void btnBShrink_Click(object sender, EventArgs e)
        {
            if (sliceArr.Count > 8)
                sliceArr.RemoveRange(sliceArr.Count - 5, 5);
        }

        private void btnAShrink_Click(object sender, EventArgs e)
        {
            if (sliceArr.Count > 8)
                sliceArr.RemoveRange(0, 5);
        }

        private void cboxIsZoom_CheckedChanged(object sender, EventArgs e)
        {
            zoomToggle = false;
        }

        private void btnHeadlandOff_Click(object sender, EventArgs e)
        {
            mf.bnd.bndList[0].hdLine?.Clear();
            mf.bnd.bndList[0].hdLineTriangleList?.Clear();

            mf.FileSaveHeadland();
            mf.bnd.isHeadlandOn = false;
            mf.vehicle.isHydLiftOn = false;
            Close();
        }
    }
}