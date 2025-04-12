using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormGrid : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private bool isA = true;
        private int start = 99999, end = 99999;

        private double zoom = 1, sX = 0, sY = 0;

        public vec3 pntA = new vec3(0.0, 1.0, 0.0);
        public vec3 pntB = new vec3(0.0, 1.0, 0.0);

        public FormGrid(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
        }

        private void FormABDraw_Load(object sender, EventArgs e)
        {
            Size = Settings.User.setWindow_gridSize;

            Location = Settings.User.setWindow_gridLocation;
            FormABDraw_ResizeEnd(this, e);

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormABDraw_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.User.setWindow_gridSize = Size;
            Settings.User.setWindow_gridLocation = Location;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.worldGrid.gridRotation = 0;
            Close();
        }

        private void btnCancelTouch_Click(object sender, EventArgs e)
        {
            //update the arrays
            start = 99999; end = 99999;
            isA = true;

            zoom = 1;
            sX = 0;
            sY = 0;

            btnExit.Focus();
        }

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = oglSelf.PointToClient(Cursor.Position);

            int wid = oglSelf.Width;
            int halfWid = oglSelf.Width / 2;
            double scale = (double)wid * 0.903;

            //if (cboxIsZoom.Checked && !zoomToggle)
            //{
            //    sX = (( halfWid - (double)pt.X) / wid)*1.1;
            //    sY = ((halfWid - (double)pt.Y) / -wid)*1.1;
            //    zoom = 0.1;
            //    return;
            //}

            //Convert to Origin in the center of window, 800 pixels
            int X = pt.X - halfWid;
            int Y = (wid - pt.Y - halfWid);
            vec3 plotPt = new vec3
            {
                //convert screen coordinates to field coordinates
                easting = X * mf.maxFieldDistance / scale * zoom,
                northing = Y * mf.maxFieldDistance / scale * zoom,
                heading = 0
            };

            plotPt.easting += mf.fieldCenterX + mf.maxFieldDistance * -sX;
            plotPt.northing += mf.fieldCenterY + mf.maxFieldDistance * -sY;

            zoom = 1;
            sX = 0;
            sY = 0;

            if (isA)
            {
                start = 99999; end = 99999;
                start = 1;
                pntA = new vec3(plotPt);
                isA = false;
            }
            else
            {
                isA = true;
                pntB = new vec3(plotPt);
                end = 1;

                mf.worldGrid.gridRotation =
            Math.Atan2(
                pntB.easting - pntA.easting,
                pntB.northing - pntA.northing);
                if (mf.worldGrid.gridRotation < 0) mf.worldGrid.gridRotation += glm.twoPI;

                mf.worldGrid.gridRotation = glm.toDegrees(mf.worldGrid.gridRotation);
            }

            oglSelf.Refresh();
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

            GL.Color3(0.9f, 0.9f, 0.8f);

            //DrawSections
            foreach (var triList in mf.patchList)
            {
                triList.DrawPolygon(8, 1, PrimitiveType.TriangleStrip);
            }

            GL.LineWidth(3);

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                if (j == 0)
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

            GL.Flush();
            oglSelf.SwapBuffers();
        }

        private void DrawABTouchPoints()
        {
            GL.Color3(0.65, 0.650, 0.0);
            GL.PointSize(24);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(0, 0, 0);
            if (start != 99999) GL.Vertex3(pntA.easting, pntA.northing, 0);
            if (end != 99999) GL.Vertex3(pntB.easting, pntB.northing, 0);
            GL.End();

            GL.PointSize(16);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(1.0f, 0.75f, 0.350f);
            if (start != 99999) GL.Vertex3(pntA.easting, pntA.northing, 0);

            GL.Color3(0.5f, 0.5f, 1.0f);
            if (end != 99999) GL.Vertex3(pntB.easting, pntB.northing, 0);
            GL.End();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oglSelf.Refresh();
        }

        private void btnAlignToTrack_Click(object sender, EventArgs e)
        {
            mf.worldGrid.gridRotation = glm.toDegrees(mf.gyd.manualUturnHeading);
            Close();
        }

        private void FormABDraw_ResizeEnd(object sender, EventArgs e)
        {
            Width = (int)((double)Height * 1.09);

            oglSelf.Height = oglSelf.Width = Height - 40;

            oglSelf.Left = 1;
            oglSelf.Top = 0;

            oglSelf.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            //58 degrees view
            GL.Viewport(0, 0, oglSelf.Width, oglSelf.Height);
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(1.01f, 1.0f, 1.0f, 20000);
            GL.LoadMatrix(ref mat);

            GL.MatrixMode(MatrixMode.Modelview);

            tlp1.Width = Width - oglSelf.Width - 10;
            tlp1.Left = oglSelf.Width - 2;
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