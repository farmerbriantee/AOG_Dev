using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;
using OpenTK.Graphics;
using System.Threading;

namespace AOG
{
    public partial class FormGPS
    {
        enum bbColors
        {
            fence = 75, headland = 105, innerFence = 25,  //red
            section = 127, tram = 240, //grn
        }

        //extracted Near, Far, Right, Left clipping planes of frustum
        public double[] frustum = new double[24];

        //
        private bool isInit = false;
        private double fovy = 0.7;
        private double camDistanceFactor = -4;

        int mouseX = 0, mouseY = 0;
        public int steerModuleConnectedCounter = 0;

        //data buffer for pixels read from off screen buffer

        byte[] grnPixels = new byte[150001];
        byte[] redPixels = new byte[150001];

        int deadCam = 0;

        StringBuilder sb = new StringBuilder();

        vec2 left = new vec2();
        vec2 right = new vec2();
        vec2 ptTip = new vec2();

        public double avgPivDistance, avgPivDistanceTool, longAvgPivDistance;

        Thread thread_oglBack;
        AutoResetEvent pauseOglBack = new AutoResetEvent(false);

        //mapping change occured
        private ulong[] number = new ulong[4], lastNumber = new ulong[4];

        private double aTime;

        void SetBit(int bitIndex, bool value = true)
        {
            int index = bitIndex / 64;
            int offset = bitIndex % 64;
            if (value)
                number[index] |= (1UL << offset);
            else
                number[index] &= ~(1UL << offset);
        }
        bool HasChanged()
        {
            for (int i = 0; i < 4; i++)
            {
                if (number[i] != lastNumber[i])
                    return true;
            }
            return false;
        }

        public void StartATimer()
        {
            algoTimer.Restart();
        }

        public void StopAtimer()
        {
            double newTime = ((double)(algoTimer.ElapsedTicks * 1000) / (double)System.Diagnostics.Stopwatch.Frequency);
            aTime = newTime * 0.1 + aTime * 0.9;
            //lblAlgo.Text = aTime.ToString("N3");
        }

        // When oglMain is created
        private void oglMain_Load(object sender, EventArgs e)
        {
            oglMain.MakeCurrent();
            LoadGLTextures();
            GL.ClearColor(0.14f, 0.14f, 0.37f, 1.0f);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.CullFace(CullFaceMode.Back);
            SetZoom();
            tmrWatchdog.Enabled = true;
        }

        //oglMain needs a resize
        private void oglMain_Resize(object sender, EventArgs e)
        {
            ChangePerspective(true);
            SetControlButtonPositions();
            SetControlLabelPositions();
            if (Settings.User.setDisplay_isLineSmooth) GL.Enable(EnableCap.LineSmooth);
            else GL.Disable(EnableCap.LineSmooth);
        }

        private void ChangePerspective(bool changeViewport = false)
        {
            oglMain.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            if (changeViewport)
                GL.Viewport(0, 0, oglMain.Width, oglMain.Height);
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView((float)fovy, oglMain.AspectRatio, 1.0f, (float)(camDistanceFactor * camera.camSetDistance));
            GL.LoadMatrix(ref mat);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        //oglMain rendering, Draw
        private void oglMain_Paint(object sender, PaintEventArgs e)
        {
            if (uint.MaxValue == sentenceCounter) return;

            if (true || sentenceCounter < 299)
            {
                if (true || isGPSPositionInitialized)
                {
                    #region Initialize

                    oglMain.MakeCurrent();

                    if (!isInit)
                    {
                        oglMain_Resize(oglMain, EventArgs.Empty);
                    }
                    isInit = true;

                    //  Clear the color and depth buffer.
                    GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

                    if (Settings.User.setDisplay_isDayMode) GL.ClearColor(0.27f, 0.4f, 0.7f, 1.0f);
                    else GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

                    GL.LoadIdentity();

                    //position the camera
                    camera.SetWorldCam(pivotAxlePos.easting, pivotAxlePos.northing, camHeading);

                    //the bounding box of the camera for cullling.
                    CalcFrustum();

                    #endregion

                    #region Field Surface

                    GL.Disable(EnableCap.Blend);

                    worldGrid.DrawFieldSurface();

                    ////if grid is on draw it
                    if (Settings.User.isGridOn) worldGrid.DrawWorldGrid(camera.gridZoom);

                    GL.Enable(EnableCap.Blend);

                    if (isDrawPolygons) GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);

                    #endregion

                    #region Draw Boundary and Headland highlight areas


                    if (!isFieldStarted)
                    {

                        GL.LineWidth(2);

                        foreach (var field in fieldFilesList.fieldArr)
                        {
                            for (int i = 1; i < field.bndPts.Count; i += 3)
                            {
                                //determine if point is in frustum or not, if < 0, its outside so abort, z always is 0                            
                                if (frustum[0] * field.bndPts[i].easting + frustum[1] * field.bndPts[i].northing + frustum[3] <= 0)
                                    continue;//right
                                if (frustum[4] * field.bndPts[i].easting + frustum[5] * field.bndPts[i].northing + frustum[7] <= 0)
                                    continue;//left
                                if (frustum[16] * field.bndPts[i].easting + frustum[17] * field.bndPts[i].northing + frustum[19] <= 0)
                                    continue;//bottom
                                if (frustum[20] * field.bndPts[i].easting + frustum[21] * field.bndPts[i].northing + frustum[23] <= 0)
                                    continue;//top
                                if (frustum[8] * field.bndPts[i].easting + frustum[9] * field.bndPts[i].northing + frustum[11] <= 0)
                                    continue;//far
                                if (frustum[12] * field.bndPts[i].easting + frustum[13] * field.bndPts[i].northing + frustum[15] <= 0)
                                    continue;//near

                                //point is in frustum so draw the entire patch. The downside of triangle strips.
                                GL.Color4(0.9, 0.9, 0.9, 1.0);
                                field.bndPts.DrawPolygon();

                                GL.PointSize(8.0f);
                                GL.Color3(0.3, 0.9, 0.3);
                                GL.Begin(PrimitiveType.Points);

                                GL.Vertex3(field.start.easting, field.start.northing, 0);
                                GL.End();

                                font.DrawText3D(field.start.easting, field.start.northing, field.name, true, 2);

                                break;
                            }
                        }
                    }

                    if (bnd.bndList.Count > 0)
                    {
                        if (bnd.isHeadlandOn && bnd.bndList.Count > 0)
                        {
                            //draw whole outer field polygon
                            GL.Color4(0.1, 0.1, 0.351, 0.25);

                            bnd.bndList[0].fenceTriangleList.DrawPolygon(PrimitiveType.Triangles);

                            //draw headland polygon
                            GL.Color4(0.1, 0.3, 0.1, 0.25);

                            bnd.bndList[0].hdLineTriangleList.DrawPolygon(PrimitiveType.Triangles);

                            //if we would have inner boundary headline draw them here
                        }
                        else //no headland excists
                        {
                            //draw outer field polygon (fence)
                            GL.Color4(0.1, 0.3, 0.1, 0.25);

                            bnd.bndList[0].fenceTriangleList.DrawPolygon(PrimitiveType.Triangles);
                        }

                        //draw red in inner boundary of field
                        if (bnd.bndList.Count > 1)
                        {
                            GL.Color4(0.351, 0.1, 0.1, 0.32);
                            for (int a = 1; a < bnd.bndList.Count; a++)
                            {
                                bnd.bndList[a].fenceTriangleList.DrawPolygon(PrimitiveType.Triangles);
                            }
                        }
                    }

                    #endregion

                    #region Draw patches of sections, section lines, section dir markers
                    //direction marker width
                    double factor = 0.37;

                    GL.LineWidth(2);

                    //initialize the steps for mipmap of triangles (skipping detail while zooming out)
                    int mipmap = 0;
                    if (camera.camSetDistance < -800) mipmap = 2;
                    if (camera.camSetDistance < -1500) mipmap = 4;
                    if (camera.camSetDistance < -2400) mipmap = 8;
                    if (camera.camSetDistance < -5000) mipmap = 16;

                    if (patchListLayer.Count > 0)
                    {

                        //for every new chunk of patch
                        foreach (var triList in patchListLayer)
                        {
                            //check for even
                            if (triList.Count % 2 == 0)
                                break;

                            bool isDraw = false;
                            int count2 = triList.Count;
                            for (int i = 1; i < count2; i += 3)
                            {
                                //determine if point is in frustum or not, if < 0, its outside so abort, z always is 0                            
                                if (frustum[0] * triList[i].easting + frustum[1] * triList[i].northing + frustum[3] <= 0)
                                    continue;//right
                                if (frustum[4] * triList[i].easting + frustum[5] * triList[i].northing + frustum[7] <= 0)
                                    continue;//left
                                if (frustum[16] * triList[i].easting + frustum[17] * triList[i].northing + frustum[19] <= 0)
                                    continue;//bottom
                                if (frustum[20] * triList[i].easting + frustum[21] * triList[i].northing + frustum[23] <= 0)
                                    continue;//top
                                if (frustum[8] * triList[i].easting + frustum[9] * triList[i].northing + frustum[11] <= 0)
                                    continue;//far
                                if (frustum[12] * triList[i].easting + frustum[13] * triList[i].northing + frustum[15] <= 0)
                                    continue;//near

                                //point is in frustum so draw the entire patch. The downside of triangle strips.
                                isDraw = true;
                                break;
                            }

                            if (isDraw)
                            {
                                if (Settings.User.setDisplay_isDayMode) GL.Color4((byte)triList[0].easting, (byte)triList[0].northing, (byte)triList[0].heading, (byte)70);
                                else GL.Color4((byte)triList[0].easting, (byte)triList[0].northing, (byte)triList[0].heading, (byte)(35));


                                triList.DrawPolygon(mipmap, 1, PrimitiveType.TriangleStrip);

                                if (Settings.User.setDisplay_isSectionLinesOn)
                                {
                                    //highlight lines
                                    GL.Color4(0.2, 0.2, 0.2, 1.0);

                                    triList.DrawPolygon(mipmap, 1, PrimitiveType.LineStrip);
                                    triList.DrawPolygon(mipmap, 2, PrimitiveType.LineStrip);
                                }

                            }
                        }
                    }

                    //for every new chunk of patch
                    foreach (var triList in patchList)
                    {
                        //check for even
                        if (triList.Count % 2 == 0)
                            break;

                        bool isDraw = false;
                        int count2 = triList.Count;
                        for (int i = 1; i < count2; i += 3)
                        {
                            //determine if point is in frustum or not, if < 0, its outside so abort, z always is 0                            
                            if (frustum[0] * triList[i].easting + frustum[1] * triList[i].northing + frustum[3] <= 0)
                                continue;//right
                            if (frustum[4] * triList[i].easting + frustum[5] * triList[i].northing + frustum[7] <= 0)
                                continue;//left
                            if (frustum[16] * triList[i].easting + frustum[17] * triList[i].northing + frustum[19] <= 0)
                                continue;//bottom
                            if (frustum[20] * triList[i].easting + frustum[21] * triList[i].northing + frustum[23] <= 0)
                                continue;//top
                            if (frustum[8] * triList[i].easting + frustum[9] * triList[i].northing + frustum[11] <= 0)
                                continue;//far
                            if (frustum[12] * triList[i].easting + frustum[13] * triList[i].northing + frustum[15] <= 0)
                                continue;//near

                            //point is in frustum so draw the entire patch. The downside of triangle strips.
                            isDraw = true;
                            break;
                        }

                        if (isDraw)
                        {
                            if (Settings.User.setDisplay_isDayMode) GL.Color4((byte)triList[0].easting, (byte)triList[0].northing, (byte)triList[0].heading, (byte)152);
                            else GL.Color4((byte)triList[0].easting, (byte)triList[0].northing, (byte)triList[0].heading, (byte)(152 * 0.5));


                            triList.DrawPolygon(mipmap, 1, PrimitiveType.TriangleStrip);

                            if (Settings.User.setDisplay_isSectionLinesOn)
                            {
                                //highlight lines
                                GL.Color4(0.2, 0.2, 0.2, 1.0);

                                triList.DrawPolygon(mipmap, 1, PrimitiveType.LineStrip);
                                triList.DrawPolygon(mipmap, 2, PrimitiveType.LineStrip);
                            }


                            if (Settings.User.isDirectionMarkers)
                            {
                                if (triList.Count > 31)
                                {
                                    double headz =
                                        Math.Atan2(triList[29].easting - triList[27].easting, triList[29].northing - triList[27].northing);

                                    left = new vec2(
                                        (triList[27].easting + factor * (triList[28].easting - triList[27].easting)),
                                        (triList[27].northing + factor * (triList[28].northing - triList[27].northing)));

                                    factor = 1 - factor;

                                    right = new vec2(
                                        (triList[27].easting + factor * (triList[28].easting - triList[27].easting)),
                                        (triList[27].northing + factor * (triList[28].northing - triList[27].northing)));

                                    double disst = glm.Distance(left, right);
                                    disst *= 1.5;

                                    ptTip = new vec2((left.easting + right.easting) / 2, (left.northing + right.northing) / 2);

                                    ptTip = new vec2(ptTip.easting + (Math.Sin(headz) * disst), ptTip.northing + (Math.Cos(headz) * disst));

                                    GL.Color4((byte)(255 - triList[0].easting), (byte)(255 - triList[0].northing), (byte)(255 - triList[0].heading), (byte)150);
                                    //GL.LineWidth(3.0f);

                                    GL.Begin(PrimitiveType.Triangles);
                                    GL.Vertex3(left.easting, left.northing, 0);
                                    GL.Vertex3(right.easting, right.northing, 0);

                                    GL.Color4(0.85, 0.85, 1, 1.0);
                                    GL.Vertex3(ptTip.easting, ptTip.northing, 0);
                                    GL.End();
                                }
                            }
                        }
                    }

                    if (patchCounter > 0)
                    {
                        foreach (var patch in triStrip)
                        {
                            if (patch.isDrawing)
                            {
                                try
                                {
                                    GL.Color4((byte)patch.triangleList[0].easting, (byte)patch.triangleList[0].northing, (byte)patch.triangleList[0].heading, (byte)(Settings.User.setDisplay_isDayMode ? 152 : 76));

                                    //draw the triangle in each triangle strip
                                    GL.Begin(PrimitiveType.TriangleStrip);

                                    for (int i = 1; i < patch.triangleList.Count; i++)
                                        GL.Vertex3(patch.triangleList[i].easting, patch.triangleList[i].northing, 0);

                                    //left side of triangle
                                    vec2 pt = new vec2((cosSectionHeading * section[patch.currentStartSectionNum].positionLeft) + toolPos.easting,
                                            (sinSectionHeading * section[patch.currentStartSectionNum].positionLeft) + toolPos.northing);

                                    GL.Vertex3(pt.easting, pt.northing, 0);

                                    //Right side of triangle
                                    pt = new vec2((cosSectionHeading * section[patch.currentEndSectionNum].positionRight) + toolPos.easting,
                                       (sinSectionHeading * section[patch.currentEndSectionNum].positionRight) + toolPos.northing);

                                    GL.Vertex3(pt.easting, pt.northing, 0);

                                    GL.End();
                                }
                                catch
                                {
                                }
                            }
                        }
                    }





                   if (tram.displayMode != 0) tram.DrawTram();

                    GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);

                    #endregion

                    #region Draw Boundaries - Headland and Fence

                    if (bnd.bndList.Count > 0 || bnd.isFenceBeingMade == true)
                    {
                        //draw Boundaries
                        bnd.DrawBnds();
                    }

                    #endregion

                    #region Draw Tracks
                    //draw contour line if button on 
                    if (ct.isContourBtnOn)
                    {
                        ct.DrawContourLine();
                    }
                    else if (trk.currTrk != null)// draw the current and reference AB Lines or CurveAB Ref and line
                    {
                        //when switching lines, draw the ghost
                        trk.DrawTrack();
                    }

                    //draw line creations
                    if (trk.isMakingTrack) trk.DrawNewTrack();

                    #endregion

                    #region Flags

                    if (flagPts.Count > 0) DrawFlags();

                    if (leftMouseDownOnOpenGL) MakeFlagMark();

                    //Direct line to flag if flag selected
                    try
                    {
                        if (flagNumberPicked > 0)
                        {
                            GL.LineWidth(Settings.User.setDisplay_lineWidth);
                            GL.Enable(EnableCap.LineStipple);
                            GL.LineStipple(1, 0x0707);
                            GL.Begin(PrimitiveType.Lines);
                            GL.Color3(0.930f, 0.72f, 0.32f);
                            GL.Vertex3(pivotAxlePos.easting, pivotAxlePos.northing, 0);
                            GL.Vertex3(flagPts[flagNumberPicked - 1].easting, flagPts[flagNumberPicked - 1].northing, 0);
                            GL.End();
                            GL.Disable(EnableCap.LineStipple);
                        }
                    }
                    catch { }

                    #endregion

                    #region Vehicle and Tool

                    //draw the vehicle/implement
                    GL.PushMatrix();
                    {
                        tool.DrawTool();
                        vehicle.DrawVehicle();
                    }
                    GL.PopMatrix();

                    if (camera.camSetDistance > -550)
                    {

                        //Draw Tool antenna
                        if (isGPSToolActive)
                        {
                            GL.PointSize(16);
                            GL.Begin(PrimitiveType.Points);
                            GL.Color3(0.0, 0.0, 0.0);
                            GL.Vertex3(pnTool.fix.easting, pnTool.fix.northing, 0.0);
                            GL.End();

                            GL.PointSize(10);
                            GL.Begin(PrimitiveType.Points);
                            GL.Color3(0.20, 0.78, 0.98);
                            GL.Vertex3(pnTool.fix.easting, pnTool.fix.northing, 0.0);
                            GL.End();
                        }

                        if (!Settings.Vehicle.setVehicle_isStanleyUsed && trk.currentGuidanceTrack.Count > 1)
                        {
                            GL.PointSize(16);
                            GL.Begin(PrimitiveType.Points);
                            GL.Color3(0, 0, 0);
                            GL.Vertex3(gyd.goalPoint.easting, gyd.goalPoint.northing, 0.0);
                            GL.End();

                            GL.PointSize(10);
                            GL.Begin(PrimitiveType.Points);
                            GL.Color3(0.98, 0.98, 0.098);
                            GL.Vertex3(gyd.goalPoint.easting, gyd.goalPoint.northing, 0.0);
                            GL.End();
                        }

                        if (Settings.Vehicle.setVehicle_isStanleyUsed)
                        {
                            GL.PointSize(16);
                            GL.Begin(PrimitiveType.Points);
                            GL.Color3(0.0, 0.0, 0.0);
                            GL.Vertex3(steerAxlePos.easting, steerAxlePos.northing, 0.0);
                            GL.End();

                            GL.PointSize(10);
                            GL.Begin(PrimitiveType.Points);
                            GL.Color3(0.920, 0.978, 0.2);
                            GL.Vertex3(steerAxlePos.easting, steerAxlePos.northing, 0.0);
                            GL.End();
                        }
                    }

                    //Draw gps points and corrected
                    if (Settings.User.isGPSCorrectionLineOn)
                    {
                        GL.LineWidth(4);
                        GL.Begin(PrimitiveType.LineStrip);
                        GL.Color3(1.0, 0.6, 0.3);

                        for (int i = 0; i < gpsPts.Count; i++)
                        {
                            GL.Vertex3(gpsPts[i].easting, gpsPts[i].northing, 0.0);
                        }
                        GL.End();

                        GL.LineWidth(4);
                        GL.Begin(PrimitiveType.LineStrip);
                        GL.Color3(0.30, 1.0, 0.0);

                        for (int i = 0; i < gpsPtsCorr.Count; i++)
                        {
                            GL.Vertex3(gpsPtsCorr[i].easting, gpsPtsCorr[i].northing, 0.0);
                        }
                        GL.End();

                        if (gpsPtsCorr.Count > 50)
                        {
                            gpsPtsCorr.RemoveRange(0, 10);
                            gpsPts.RemoveRange(0, 10);
                        }
                    }

                    #endregion

                    #region Text Lightbar Steer Circle etc - Ortho Mode
                    // 2D Ortho ---------------------------------------////////-------------------------------------------------

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.PushMatrix();
                    GL.LoadIdentity();

                    //negative and positive on width, 0 at top to bottom ortho view
                    GL.Ortho(-(double)oglMain.Width / 2, (double)oglMain.Width / 2, (double)oglMain.Height, 0, -1, 1);

                    //  Create the appropriate modelview matrix.
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.PushMatrix();
                    GL.LoadIdentity();

                    //LightBar if AB Line is set and turned on or contour
                    if (Settings.User.isLightbarNotSteerBar)
                    {
                        DrawLightBarText();
                    }
                    else if (Settings.User.isLightbarOn)
                    {
                        DrawSteerBarText();
                    }

                    if (!ct.isContourBtnOn && trk.currTrk != null) DrawTrackInfo();


                    if (bnd.bndList.Count > 0 && yt.isYouTurnBtnOn && !ct.isContourBtnOn) DrawUTurnBtn();

                    if ((isBtnAutoSteerOn || yt.isYouTurnBtnOn) && !ct.isContourBtnOn) DrawManUTurnBtn();

                    if (Settings.User.isCompassOn) DrawCompass();
                    DrawCompassText();

                    if (Settings.User.isSpeedoOn) DrawSpeedo();

                    DrawSteerCircle();

                    if (Settings.Tool.isDisplayTramControl && tram.displayMode != 0) { DrawTramMarkers(); }

                    if (vehicle.isHydLiftOn) DrawLiftIndicator();

                    if (isReverse)
                        DrawReverse();

                    if (Settings.Vehicle.setGPS_isRTK)
                    {
                        if (pn.fixQuality != 4)
                        {
                            if (!sounds.isRTKAlarming)
                            {
                                if (Settings.Vehicle.setGPS_isRTK_KillAutoSteer && isBtnAutoSteerOn)
                                {
                                    SetAutoSteerButton(false, "RTK Fix Alarm");
                                    Log.EventWriter("Autosteer Off, RTK Fix Alarm");
                                }

                                Log.EventWriter("RTK Alarm Fix is Lost");
                                sounds.sndRTKAlarm.Play();
                            }
                            sounds.isRTKAlarming = true;
                            DrawLostRTK();
                        }
                        else
                        {
                            sounds.isRTKAlarming = false;
                        }
                    }

                    if (pn.age > Settings.Vehicle.setGPS_ageAlarm) DrawAge();

                    //at least one track
                    if (guideLineCounter > 0) DrawGuidanceLineText();

                    //if hardware messages
                    if (isHardwareMessages) DrawHardwareMessageText();

                    double leftPos = 170;
                    double size = 150;
                    double topPos = oglMain.Height * 0.5 - 50;

                    if (sentenceCounter > 299 || !isGPSPositionInitialized)
                    {
                        GL.Enable(EnableCap.Texture2D);
                        GL.Color4(1.25f, 1.25f, 1.275f, 0.75);
                        GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.NoGPS]);        // Select Our Texture

                        GL.PushMatrix();

                        GL.Translate(leftPos, topPos, 0.0f);
                        //GL.Rotate(deadCam, 0.0f, 1.0f, 0.0f);
                        //deadCam += 5;
                        GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                        GL.TexCoord2(1, 0); GL.Vertex2(size, -size); // Top Right
                        GL.TexCoord2(0, 0); GL.Vertex2(-size, -size); // Top Left
                        GL.TexCoord2(1, 1); GL.Vertex2(size, size); // Bottom Right
                        GL.TexCoord2(0, 1); GL.Vertex2(-size, size); // Bottom Left
                        GL.End();                       // Done Building Triangle Strip
                        GL.PopMatrix();

                        GL.Color3(0.98f, 0.98f, 0.70f);

                        int edge = -oglMain.Width / 2 + 10;
                        font.DrawText(edge, oglMain.Height - 120, "<-- AgIO ?");

                        lblSpeed.Text = "???";
                        lblHz.Text = " ???? \r\n Not Connected";
                    }
                    else if (!isFirstHeadingSet)
                    {
                        GL.Enable(EnableCap.Texture2D);
                        GL.Color4(1, 1, 1, 0.75);
                        GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.QuestionMark]);        // Select Our Texture
                        GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                        GL.TexCoord2(1, 0); GL.Vertex2(leftPos + size, topPos - size); // Top Right
                        GL.TexCoord2(0, 0); GL.Vertex2(leftPos - size, topPos - size); // Top Left
                        GL.TexCoord2(1, 1); GL.Vertex2(leftPos + size, topPos + size); // Bottom Right
                        GL.TexCoord2(0, 1); GL.Vertex2(leftPos - size, topPos + size); // Bottom Left
                        GL.End();                       // Done Building Triangle Strip
                        GL.Disable(EnableCap.Texture2D);
                    }

                    //just in case
                    GL.Disable(EnableCap.LineStipple);

                    GL.LineWidth(8);
                    GL.Color3(0, 0, 0);

                    if (mc.isOutOfBounds)
                    {
                        GL.Color3(1.0, 0.66, 0.33);
                        GL.LineWidth(16);
                    }
                    if ((Settings.Vehicle.setGPS_isRTK && sounds.isRTKAlarming) || (yt.isYouTurnBtnOn && yt.youTurnPhase == 250))
                    {
                        if (isFlashOnOff)
                        {
                            GL.Color3(1.0, 0.25, 0.25);
                            GL.LineWidth(16);
                        }
                        else
                        {
                            GL.Color3(0.8, 0.250, 0.25);
                            GL.LineWidth(16);
                        }
                    }

                    GL.Begin(PrimitiveType.LineLoop);

                    GL.Vertex3(-oglMain.Width / 2, 0, 0);
                    GL.Vertex3(oglMain.Width / 2, 0, 0);
                    GL.Vertex3(oglMain.Width / 2, oglMain.Height, 0);
                    GL.Vertex3(-oglMain.Width / 2, oglMain.Height, 0);

                    GL.End();

                    GL.PopMatrix();//  Pop the modelview.

                    #endregion Ortho

                    #region Flush and Swap
                    //  back to the projection and pop it, then back to the model view.
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.PopMatrix();
                    GL.MatrixMode(MatrixMode.Modelview);

                    GL.Flush();
                    oglMain.SwapBuffers();

                    #endregion

                    #region FileSave and oglZoom

                    //file writer that runs all the time
                    if (fileSaveAlwaysCounter > 60)
                    {
                        fileSaveAlwaysCounter = 0;
                        //if (sbMissedSentence.Length > 0) FileSaveMissedEvents();
                    }

                    //if a minute has elapsed save the field in case of crash and to be able to resume            
                    if (fileSaveCounter > 30 && sentenceCounter < 20)
                    {
                        tmrWatchdog.Enabled = false;
                        fileSaveCounter = 0;

                        DistanceToFieldOriginCheck();

                        //don't save if no gps
                        if (isJobStarted)
                        {
                            //auto save the field patches, contours accumulated so far
                            FileSaveSections();
                            FileSaveContour();

                            //NMEA elevation file
                            if (Settings.User.isLogElevation && sbElevationString.Length > 0) FileSaveElevation();
                        }

                        //go see if data ready for draw and position updates
                        tmrWatchdog.Enabled = true;

                        //calc overlap
                        oglZoom.Refresh();
                    }

                    #endregion
                }
            }

            #region No GPS

            else
            {
                //sentenceCounter = 0;
                oglMain.MakeCurrent();
                GL.Enable(EnableCap.Blend);
                GL.ClearColor(0.122f, 0.1258f, 0.1275f, 1.0f);

                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                GL.LoadIdentity();
                GL.Translate(0.0, 0.3, -10);
                GL.Rotate(deadCam, 0.0, 1.0, 0.0);

                deadCam += 5;

                GL.Enable(EnableCap.Texture2D);
                GL.Color4(1.25f, 1.25f, 1.275f, 0.75);
                GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.NoGPS]);        // Select Our Texture
                GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                GL.TexCoord2(1, 0); GL.Vertex2(2.5, 2.5); // Top Right
                GL.TexCoord2(0, 0); GL.Vertex2(-2.5, 2.5); // Top Left
                GL.TexCoord2(1, 1); GL.Vertex2(2.5, -2.5); // Bottom Right
                GL.TexCoord2(0, 1); GL.Vertex2(-2.5, -2.5); // Bottom Left
                GL.End();                       // Done Building Triangle Strip

                GL.Disable(EnableCap.Texture2D);

                // 2D Ortho ---------------------------------------////////-------------------------------------------------

                GL.MatrixMode(MatrixMode.Projection);
                GL.PushMatrix();
                GL.LoadIdentity();

                //negative and positive on width, 0 at top to bottom ortho view
                GL.Ortho(-(double)oglMain.Width / 2, (double)oglMain.Width / 2, (double)oglMain.Height, 0, -1, 1);

                //  Create the appropriate modelview matrix.
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                GL.LoadIdentity();

                GL.Color3(0.98f, 0.98f, 0.70f);

                int edge = -oglMain.Width / 2 + 10;

                font.DrawText(edge, oglMain.Height - 80, "<-- AgIO ?");

                GL.Flush();//finish openGL commands
                GL.PopMatrix();//  Pop the modelview.

                ////-------------------------------------------------ORTHO END---------------------------------------

                //  back to the projection and pop it, then back to the model view.
                GL.MatrixMode(MatrixMode.Projection);
                GL.PopMatrix();
                GL.MatrixMode(MatrixMode.Modelview);

                //reset point size
                GL.PointSize(1.0f);

                GL.Flush();
                oglMain.SwapBuffers();

                lblSpeed.Text = "???";
                lblHz.Text = " ???? \r\n Not Connected";
            }

            #endregion
        }

        private void oglBack_Load(object sender, EventArgs e)
        {
            oglBack.MakeCurrent();
            oglBack.Width = 500;
            oglBack.Height = 300;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Viewport(0, 0, 500, 300);
            Matrix4 mat = Matrix4.CreateOrthographicOffCenter(-25, 25, 0, 30, -1, 1);
            //Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(0.06f, 1.6666666666f, 50.0f, 520.0f);
            GL.LoadMatrix(ref mat);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.PixelStore(PixelStoreParameter.PackAlignment, 1);
        }

        private void oglBack_Paint(object sender, PaintEventArgs e)
        {
            #region OGL translate rotate

            oglBack.MakeCurrent();

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();                  // Reset The View

            //back the camera up
            //GL.Translate(0, 0, -500);

            //rotate camera so heading matched fix heading in the world
            GL.Rotate(glm.toDegrees(toolPos.heading), 0, 0, 1);

            GL.Translate(-toolPos.easting, -toolPos.northing, 0);

            #endregion

            #region Draw Red and Grn to Back Buffer           

            // field triangulation color qqq
            GL.ColorMask(true, false, false, false); //Draw only in red

            if (bnd.bndList.Count > 0)
            {
                if (bnd.isHeadlandOn && bnd.isSectionControlledByHeadland && bnd.bndList[0].hdLineTriangleList.Count > 0)
                {
                    //draw red in whole outer field polygon
                    GL.Color3((byte)bbColors.fence, (byte)0, (byte)0);

                    bnd.bndList[0].fenceTriangleList.DrawPolygon(PrimitiveType.Triangles);

                    //draw red in headland polygon
                    GL.Color3((byte)bbColors.headland, (byte)0, (byte)0);

                    bnd.bndList[0].hdLineTriangleList.DrawPolygon(PrimitiveType.Triangles);
                    //if we would have inner boundary headline draw them here
                }
                else //no headland excists
                {
                    //draw  red in whole outer field polygon, whole field represents headland
                    GL.Color3((byte)bbColors.headland, (byte)0, (byte)0);

                    bnd.bndList[0].fenceTriangleList.DrawPolygon(PrimitiveType.Triangles);
                }

                //draw red in inner boundary of field
                if (bnd.bndList.Count > 1)
                {
                    GL.Color3((byte)bbColors.innerFence, (byte)0, (byte)0);
                    for (int a = 1; a < bnd.bndList.Count; a++)
                    {
                        bnd.bndList[a].fenceTriangleList.DrawPolygon(PrimitiveType.Triangles);
                    }
                }
            }

            //patch color
            GL.ColorMask(false, true, false, false); //Draw only in green
            GL.Color3((byte)0, (byte)bbColors.section, (byte)0);


            double pivEplus = toolPos.easting + 50;
            double pivEminus = toolPos.easting - 50;
            double pivNplus = toolPos.northing + 50;
            double pivNminus = toolPos.northing - 50;

            //for every new chunk of patch
            foreach (var triList in patchList)
            {
                int count2 = triList.Count;
                for (int i = 1; i < count2; i += 3)
                {
                    //determine if point is in frustum or not
                    if (triList[i].easting > pivEplus)
                        continue;
                    if (triList[i].easting < pivEminus)
                        continue;
                    if (triList[i].northing > pivNplus)
                        continue;
                    if (triList[i].northing < pivNminus)
                        continue;

                    //point is in frustum so draw the entire patch
                    triList.DrawPolygon(0, 1, PrimitiveType.TriangleStrip);
                    break;
                }
            }

            //Draw currently being made patch
            if (patchCounter > 0)
            {
                foreach (var patch in triStrip)
                {
                    if (patch.isDrawing)
                    {
                        try
                        {
                            //draw the triangle in each triangle strip
                            GL.Begin(PrimitiveType.TriangleStrip);

                            for (int i = 1; i < patch.triangleList.Count; i++)
                                GL.Vertex3(patch.triangleList[i].easting, patch.triangleList[i].northing, 0);

                            GL.End();
                        }
                        catch
                        {
                        }
                    }
                }
            }

            //tram tracks
            GL.Color3((byte)0, (byte)bbColors.tram, (byte)0);

            if (Settings.Tool.isDisplayTramControl && tram.displayMode != 0)
            {
                GL.LineWidth(4);

                if ((tram.displayMode == 1 || tram.displayMode == 2))
                {
                    for (int i = 0; i < tram.tramList.Count; i++)
                    {
                        GL.Begin(PrimitiveType.LineStrip);
                        for (int h = 0; h < tram.tramList[i].Count; h++)
                            GL.Vertex3(tram.tramList[i][h].easting, tram.tramList[i][h].northing, 0);
                        GL.End();
                    }
                }

                if (tram.displayMode == 1 || tram.displayMode == 3)
                {
                    //boundary tram list
                    tram.tramBndOuterArr.DrawPolygon(PrimitiveType.LineStrip);
                    tram.tramBndInnerArr.DrawPolygon(PrimitiveType.LineStrip);
                }
            }

            GL.ColorMask(true, true, true, true);

            //finish it up - we need to read the ram of video card
            GL.Flush();

            #endregion

            #region Lookahead and ReadPixel

            //set the look ahead for hyd Lift in pixels per second
            vehicle.hydLiftLookAheadDistanceLeft = tool.farLeftSpeed * Settings.Tool.hydraulicLiftLookAhead * 10;
            vehicle.hydLiftLookAheadDistanceRight = tool.farRightSpeed * Settings.Tool.hydraulicLiftLookAhead * 10;

            if (vehicle.hydLiftLookAheadDistanceLeft > 200) vehicle.hydLiftLookAheadDistanceLeft = 200;
            if (vehicle.hydLiftLookAheadDistanceRight > 200) vehicle.hydLiftLookAheadDistanceRight = 200;

            tool.lookAheadDistanceOnPixelsLeft = tool.farLeftSpeed * Settings.Tool.lookAheadOn * 10;
            tool.lookAheadDistanceOnPixelsRight = tool.farRightSpeed * Settings.Tool.lookAheadOn * 10;

            if (tool.lookAheadDistanceOnPixelsLeft > 200) tool.lookAheadDistanceOnPixelsLeft = 200;
            if (tool.lookAheadDistanceOnPixelsRight > 200) tool.lookAheadDistanceOnPixelsRight = 200;

            tool.lookAheadDistanceOffPixelsLeft = tool.farLeftSpeed * Settings.Tool.lookAheadOff * 10;
            tool.lookAheadDistanceOffPixelsRight = tool.farRightSpeed * Settings.Tool.lookAheadOff * 10;

            if (tool.lookAheadDistanceOffPixelsLeft > 200) tool.lookAheadDistanceOffPixelsLeft = 200;
            if (tool.lookAheadDistanceOffPixelsRight > 200) tool.lookAheadDistanceOffPixelsRight = 200;

            if (Settings.Tool.lookAheadDistanceOff != 0)
            {
                tool.lookAheadDistanceOffPixelsLeft = Settings.Tool.lookAheadDistanceOff * 10;
                tool.lookAheadDistanceOffPixelsRight = Settings.Tool.lookAheadDistanceOff * 10;
            }
            if (Settings.Tool.lookAheadDistanceOn != 0)
            {
                tool.lookAheadDistanceOnPixelsLeft = Settings.Tool.lookAheadDistanceOn * 10;
                tool.lookAheadDistanceOnPixelsRight = Settings.Tool.lookAheadDistanceOn * 10;
            }

            //determine farthest ahead lookahead - is the height of the readpixel line
            double rpHeight = 0;
            double rpOnHeight = 0;
            double rpToolHeight = 0;

            //pick the larger side
            if (vehicle.hydLiftLookAheadDistanceLeft > vehicle.hydLiftLookAheadDistanceRight) rpToolHeight = vehicle.hydLiftLookAheadDistanceLeft;
            else rpToolHeight = vehicle.hydLiftLookAheadDistanceRight;

            if (tool.lookAheadDistanceOnPixelsLeft > tool.lookAheadDistanceOnPixelsRight) rpOnHeight = tool.lookAheadDistanceOnPixelsLeft;
            else rpOnHeight = tool.lookAheadDistanceOnPixelsRight;

            //clamp the height after looking way ahead, this is for switching off super section only
            rpOnHeight = Math.Abs(rpOnHeight);
            rpToolHeight = Math.Abs(rpToolHeight);

            if ((rpOnHeight < rpToolHeight && bnd.isHeadlandOn && bnd.isSectionControlledByHeadland)) rpHeight = rpToolHeight + 2;
            else rpHeight = rpOnHeight + 2;

            if (rpHeight > 290) rpHeight = 290;
            if (rpHeight < 8) rpHeight = 8;

            byte[] rgbPixels = new byte[tool.rpWidth * (int)rpHeight * 2];

            //read the whole block of pixels up to max lookahead, one read only qqq
            GL.ReadPixels(tool.rpXPosition, 0, tool.rpWidth, (int)rpHeight, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.UnsignedByte, rgbPixels);

            for (int i = 0, j = 0; i < rgbPixels.Length; i += 2, j++)
            {
                redPixels[j] = rgbPixels[i];
                grnPixels[j] = rgbPixels[i + 1];
            }


            //Paint to context for troubleshooting qqq
            //oglBack.BringToFront();
            //oglBack.SwapBuffers();
            #endregion

            #region Tram Painting
            ///////////////////////////////////////////  Tram control  ///////////////////////////////////////////

            if (tram.displayMode > 0 && Settings.Tool.toolWidth > vehicle.trackWidth)
            {
                tram.controlByte = 0;
                //1 pixels in is there a tram line?
                if (tram.isOuter)
                {
                    if (grnPixels[tool.rpWidth - (int)(tram.halfWheelTrack * 10)] == (byte)bbColors.tram || tram.isRightManualOn) tram.controlByte += 1;
                    if (grnPixels[(int)(tram.halfWheelTrack * 10)] == (byte)bbColors.tram || tram.isLeftManualOn) tram.controlByte += 2;
                }
                else
                {
                    if (grnPixels[tool.rpWidth / 2 + (int)(tram.halfWheelTrack * 10)] == (byte)bbColors.tram || tram.isRightManualOn) tram.controlByte += 1;
                    if (grnPixels[tool.rpWidth / 2 - (int)(tram.halfWheelTrack * 10)] == (byte)bbColors.tram || tram.isLeftManualOn) tram.controlByte += 2;
                }
            }
            else tram.controlByte = 0;

            #endregion

            #region Hydraulic Control
            ///////////////////////////////////////////  Hydraulic control  ///////////////////////////////////////////

            //determine if headland is in read pixel buffer left middle and right. 
            int start = 0, end = tool.rpWidth;

            //the lookahead on and off lines
            int dwnHeight = 1, upHeight = 1;

            bnd.isToolInHeadland = true;

            //calculate the slope
            double m = (vehicle.hydLiftLookAheadDistanceRight - vehicle.hydLiftLookAheadDistanceLeft) / tool.rpWidth;

            for (int pos = start; pos <= end; pos++)
            {
                dwnHeight = (int)(vehicle.hydLiftLookAheadDistanceLeft + (m * pos)) * tool.rpWidth + pos;
                upHeight = pos;
                if (bnd.isHeadlandOn && bnd.isSectionControlledByHeadland)
                {
                    if (redPixels[dwnHeight] == (byte)bbColors.headland || redPixels[upHeight] == (byte)bbColors.headland)
                    {
                        bnd.isToolInHeadland = false;
                        break;
                    }
                }
                else
                {
                    //TODO
                    //Maybe when we are not using headland we should lower the tool if there are som unaplied area?
                    //For example when we are driving the headland

                    if (redPixels[dwnHeight] == (byte)bbColors.headland && grnPixels[dwnHeight] != (byte)bbColors.section
                        || redPixels[upHeight] == (byte)bbColors.headland && grnPixels[upHeight] != (byte)bbColors.section)
                    {
                        bnd.isToolInHeadland = false;
                        break;
                    }
                }
            }

            bnd.SetHydPosition();

            #endregion

            #region Section On Off Requests
            ///////////////////////////////////////////   Section control   ///////////////////////////////////////////

            //slope of the look ahead line
            double mOn = 0, mOff = 0;

            //the lookahead on and off lines
            int onHeight = 1, offHeight = 1;

            //headland and boundary counts
            int onCount = 0, offCount = 0;

            if (!tool.remoteSectionControl)
            {
                //loop thru each section for section control
                for (int j = 0; j < section.Count; j++)
                {
                    // Manual on, force the section On
                    if (section[j].sectionBtnState == btnStates.On)
                    {
                        section[j].sectionOnRequest = true;
                        continue;
                    }

                    //Off or too slow or going backwards
                    if (section[j].sectionBtnState == btnStates.Off || avgSpeed < Settings.Tool.slowSpeedCutoff || section[j].speedPixels < 0)
                    {
                        section[j].sectionOnRequest = false;
                        continue;
                    }

                    //AutoSection - If any nowhere applied, send OnRequest, if its all green send an offRequest

                    //calculate the slopes of the lines
                    mOn = (tool.lookAheadDistanceOnPixelsRight - tool.lookAheadDistanceOnPixelsLeft) / tool.rpWidth;
                    mOff = (tool.lookAheadDistanceOffPixelsRight - tool.lookAheadDistanceOffPixelsLeft) / tool.rpWidth;

                    start = section[j].rpSectionPosition - section[0].rpSectionPosition;
                    end = section[j].rpSectionWidth - 1 + start;

                    if (end > tool.rpWidth)
                        end = tool.rpWidth;

                    offCount = 0;
                    onCount = 0;

                    for (int pos = start; pos <= end; pos++)
                    {
                        offHeight = (int)(tool.lookAheadDistanceOffPixelsLeft + (mOff * pos)) * tool.rpWidth + pos;
                        onHeight = (int)(tool.lookAheadDistanceOnPixelsLeft + (mOn * pos)) * tool.rpWidth + pos;

                        //logic if in or out of boundaries or headland
                        //Has a fence and... a headland but headland not control sections OR no headland. No headland was drawn
                        if (bnd.bndList.Count > 0)
                        {
                            if (redPixels[onHeight] == (byte)bbColors.headland && grnPixels[onHeight] == 0)
                                onCount++;
                            if (redPixels[offHeight] != (byte)bbColors.headland ||
                                (grnPixels[offHeight] == (byte)bbColors.section || grnPixels[offHeight] == (byte)bbColors.tram))
                                offCount++;
                        }
                        else
                        {
                            if (grnPixels[onHeight] == 0) onCount++;

                            if (grnPixels[offHeight] == (byte)bbColors.section)
                                offCount++;
                        }
                    }

                    //check for off
                    int coverage = (end - start + 1) - ((end - start + 1) * Settings.Tool.minCoverage) / 100;

                    if (onCount > coverage) section[j].sectionOnRequest = true;
                    else section[j].sectionOnRequest = false;

                    coverage = ((end - start + 1) * Settings.Tool.minCoverage) / 100;
                    if (offCount < (coverage) && section[j].sectionOnRequest == false)
                        section[j].sectionOnRequest = true;
                } // end of go thru all sections "for"
            }
            #endregion

            #region Section delays and mapping

            int mappingFactor = (int)(gpsHz / 9 * 2);

            //Set all the on and off times based from on off section requests
            for (int j = 0; j < section.Count; j++)
            {
                //SECTION timers

                if (!tool.remoteSectionControl)
                {
                    if (section[j].sectionOnRequest)
                        section[j].isSectionOn = true;

                    //turn off delay
                    if (Settings.Tool.offDelay > 0)
                    {
                        if (section[j].sectionOnRequest) section[j].sectionOffTimer = (int)(gpsHz * Settings.Tool.offDelay);

                        if (section[j].sectionOffTimer > 0) section[j].sectionOffTimer--;

                        if (!section[j].sectionOnRequest && section[j].sectionOffTimer == 0)
                        {
                            if (section[j].isSectionOn) section[j].isSectionOn = false;
                        }
                    }
                    else
                    {
                        if (!section[j].sectionOnRequest)
                            section[j].isSectionOn = false;
                    }

                    //Mapping timers
                    if (section[j].sectionOnRequest && !section[j].isMappingOn && section[j].mappingOnTimer == 0)
                    {
                        section[j].mappingOnTimer = (int)(Settings.Tool.lookAheadOn * gpsHz) - mappingFactor;
                    }
                    else if (section[j].sectionOnRequest && section[j].isMappingOn && section[j].mappingOffTimer > 1)
                    {
                        section[j].mappingOffTimer = 0;
                        section[j].mappingOnTimer = (int)(Settings.Tool.lookAheadOn * gpsHz) - mappingFactor;
                    }

                    if (Settings.Tool.lookAheadOff > 0)
                    {
                        if (!section[j].sectionOnRequest && section[j].isMappingOn && section[j].mappingOffTimer == 0)
                        {
                            section[j].mappingOffTimer = (int)(Settings.Tool.lookAheadOff * gpsHz) + mappingFactor;
                        }
                    }
                    else if (Settings.Tool.offDelay > 0)
                    {
                        if (!section[j].sectionOnRequest && section[j].isMappingOn && section[j].mappingOffTimer == 0)
                            section[j].mappingOffTimer = (int)(Settings.Tool.offDelay * gpsHz) + mappingFactor;
                    }
                    else
                    {
                        section[j].mappingOffTimer = 0;
                    }
                }

                //MAPPING - Not the making of triangle patches - only status - on or off
                if (section[j].sectionOnRequest)
                {
                    section[j].mappingOffTimer = 0;
                    if (section[j].mappingOnTimer > 1)
                        section[j].mappingOnTimer--;
                    else
                    {
                        section[j].isMappingOn = true;
                    }
                }
                else if (!section[j].sectionOnRequest)
                {
                    section[j].mappingOnTimer = 0;
                    if (section[j].mappingOffTimer > 1)
                        section[j].mappingOffTimer--;
                    else
                    {
                        section[j].isMappingOn = false;
                    }
                }
            }

            #endregion

            #region Workswitch control

            //Checks the workswitch or steerSwitch if required
            mc.CheckWorkAndSteerSwitch();

            #endregion

            #region Combine section patches

            // check if any sections have changed status
            number = new ulong[4];
            bool atLeastOne = false;

            for (int j = 0; j < section.Count; j++)
            {
                if (section[j].isMappingOn)
                {
                    atLeastOne = true;
                    SetBit(j);
                }
            }

            //there has been a status change of section on/off
            if (HasChanged())
            {
                int sectionOnOffZones = 0, patchingZones = 0;

                //everything off
                if (!atLeastOne)
                {
                    foreach (var patch in triStrip)
                    {
                        if (patch.isDrawing)
                            patch.TurnMappingOff();
                    }
                }
                else if (!Settings.Tool.setColor_isMultiColorSections)
                {
                    //set the start and end positions from section points
                    for (int j = 0; j < section.Count; j++)
                    {
                        //skip till first mapping section
                        if (!section[j].isMappingOn) continue;

                        //do we need more patches created
                        if (triStrip.Count < sectionOnOffZones + 1)
                            triStrip.Add(new CPatches(this));

                        //set this strip start edge to edge of this section
                        triStrip[sectionOnOffZones].newStartSectionNum = j;

                        while ((j + 1) < section.Count && section[j + 1].isMappingOn)
                        {
                            j++;
                        }

                        //set the edge of this section to be end edge of strp
                        triStrip[sectionOnOffZones].newEndSectionNum = j;
                        sectionOnOffZones++;
                    }

                    //countExit current patch strips being made
                    for (int j = 0; j < triStrip.Count; j++)
                    {
                        if (triStrip[j].isDrawing) patchingZones++;
                    }

                    //tests for creating new strips or continuing
                    bool isOk = (patchingZones == sectionOnOffZones && sectionOnOffZones < 3);

                    if (isOk)
                    {
                        for (int j = 0; j < sectionOnOffZones; j++)
                        {
                            if (triStrip[j].newStartSectionNum > triStrip[j].currentEndSectionNum
                                || triStrip[j].newEndSectionNum < triStrip[j].currentStartSectionNum)
                                isOk = false;
                        }
                    }

                    if (isOk)
                    {
                        for (int j = 0; j < sectionOnOffZones; j++)
                        {
                            if (triStrip[j].newStartSectionNum != triStrip[j].currentStartSectionNum
                                || triStrip[j].newEndSectionNum != triStrip[j].currentEndSectionNum)
                            {
                                triStrip[j].AddMappingPoint();

                                triStrip[j].currentStartSectionNum = triStrip[j].newStartSectionNum;
                                triStrip[j].currentEndSectionNum = triStrip[j].newEndSectionNum;
                                triStrip[j].AddMappingPoint();
                            }
                        }
                    }
                    else
                    {
                        //too complicated, just make new strips
                        for (int j = 0; j < triStrip.Count; j++)
                        {
                            if (triStrip[j].isDrawing)
                                triStrip[j].TurnMappingOff();
                        }

                        for (int j = 0; j < sectionOnOffZones; j++)
                        {
                            triStrip[j].currentStartSectionNum = triStrip[j].newStartSectionNum;
                            triStrip[j].currentEndSectionNum = triStrip[j].newEndSectionNum;
                            triStrip[j].TurnMappingOn(0);
                        }
                    }
                }
                else if (Settings.Tool.setColor_isMultiColorSections) //could be else only but this is more clear
                {
                    //set the start and end positions from section points
                    for (int j = 0; j < section.Count; j++)
                    {
                        //do we need more patches created
                        if (triStrip.Count < sectionOnOffZones + 1)
                            triStrip.Add(new CPatches(this));

                        //set this strip start edge to edge of this section
                        triStrip[sectionOnOffZones].newStartSectionNum = j;

                        //set the edge of this section to be end edge of strp
                        triStrip[sectionOnOffZones].newEndSectionNum = j;
                        sectionOnOffZones++;

                        if (!section[j].isMappingOn)
                        {
                            if (triStrip[j].isDrawing)
                                triStrip[j].TurnMappingOff();
                        }
                        else
                        {
                            triStrip[j].currentStartSectionNum = triStrip[j].newStartSectionNum;
                            triStrip[j].currentEndSectionNum = triStrip[j].newEndSectionNum;
                            triStrip[j].TurnMappingOn(j);
                        }
                    }
                }
                lastNumber = number;
            }

            #endregion
        }

        private void oglZoom_Load(object sender, EventArgs e)
        {
            oglZoom.MakeCurrent();
            oglZoom.Width = 500;
            oglZoom.Height = 500;
            oglZoom.Left = 100;
            oglZoom.Top = 80;
            oglZoom.SendToBack();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Viewport(0, 0, oglZoom.Width, oglZoom.Height);
            //58 degrees view
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(1.0f, 1.0f, 100.0f, 5000.0f);
            GL.LoadMatrix(ref mat);

            GL.MatrixMode(MatrixMode.Modelview);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.PixelStore(PixelStoreParameter.PackAlignment, 1);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ClearColor(0, 0, 0, 1.0f);
        }

        private void oglZoom_Paint(object sender, PaintEventArgs e)
        {

            if (isFieldStarted)
            {
                #region Draw Sections

                oglZoom.MakeCurrent();

                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                GL.LoadIdentity();                  // Reset The View
                if (bnd.bndList.Count == 0)
                    CalculateSectionPatchesMinMax();
                //back the camera up
                GL.Translate(0, 0, -maxFieldDistance);
                GL.Enable(EnableCap.Blend);

                //translate to that spot in the world 
                GL.Translate(-fieldCenterX, -fieldCenterY, 0);

                GL.Color4(0.5, 0.5, 0.5, 0.5);
                //draw patches
                //for every new chunk of patch
                foreach (var triList in patchList)
                {
                    //draw the triangle in each triangle strip
                    GL.Begin(PrimitiveType.TriangleStrip);
                    int count2 = triList.Count;
                    int mipmap = 2;

                    //if large enough patch and camera zoomed out, fake mipmap the patches, skip triangles
                    if (count2 >= (mipmap))
                    {
                        int step = mipmap;
                        for (int i = 1; i < count2; i += step)
                        {
                            GL.Vertex3(triList[i].easting, triList[i].northing, 0); i++;
                            GL.Vertex3(triList[i].easting, triList[i].northing, 0); i++;

                            //too small to mipmap it
                            if (count2 - i <= (mipmap))
                                break;
                        }
                    }

                    else
                    {
                        for (int i = 1; i < count2; i++) GL.Vertex3(triList[i].easting, triList[i].northing, 0);
                    }
                    GL.End();
                }

                GL.Flush();

                //oglZoom.SwapBuffers();

                #endregion

                #region Calculate Overlap
                int grnHeight = oglZoom.Height;
                int grnWidth = oglZoom.Width;
                byte[] overPix = new byte[grnHeight * grnWidth + 1];

                GL.ReadPixels(0, 0, grnWidth, grnWidth, OpenTK.Graphics.OpenGL.PixelFormat.Green, PixelType.UnsignedByte, overPix);

                int once = 0;
                int twice = 0;
                int more = 0;
                double total = 0;
                double total2 = 0;

                //50, 96, 112                
                for (int i = 0; i < grnHeight * grnWidth; i++)
                {

                    if (overPix[i] > 105)
                    {
                        more++;
                    }
                    else if (overPix[i] > 85)
                    {
                        twice++;
                    }
                    else if (overPix[i] > 50)
                    {
                        once++;
                    }
                }
                total = once + twice + more;
                total2 = total + twice + more + more;

                if (total2 > 0)
                {
                    fd.actualAreaCovered = (total / total2 * fd.workedAreaTotal);
                    fd.overlapPercent = Math.Round(((1 - total / total2) * 100), 2);
                }
                else
                {
                    fd.actualAreaCovered = fd.overlapPercent = 0;
                }
            }

            #endregion
        }

        private void DrawManUTurnBtn()
        {
            GL.Enable(EnableCap.Texture2D);

            int bottomSide = 90;

            if (Settings.User.setFeatures.isUTurnOn)
            {
                GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.TurnManual]);        // Select Our Texture
                GL.Color3(0.90f, 0.90f, 0.293f);

                int two3 = oglMain.Width / 4;
                GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                {
                    //GL.TexCoord2(0, 0); GL.Vertex2(-82 - two3, bottomSide); // TL
                    //GL.TexCoord2(1, 0); GL.Vertex2(82 - two3,  bottomSide); // TR
                    //GL.TexCoord2(1, 1); GL.Vertex2(82 - two3,  bottomSide+60); // BR
                    //GL.TexCoord2(0, 1); GL.Vertex2(-82 - two3, bottomSide+60); // BL

                    GL.TexCoord2(1, 0); GL.Vertex2(82 - two3, bottomSide); // Top Right
                    GL.TexCoord2(0, 0); GL.Vertex2(-82 - two3, bottomSide); // Top Left
                    GL.TexCoord2(1, 1); GL.Vertex2(82 - two3, bottomSide + 60); // Bottom Right
                    GL.TexCoord2(0, 1); GL.Vertex2(-82 - two3, bottomSide + 60); // Bottom Left

                }
                GL.End();
            }

            //lateral line move

            bottomSide += 80;
            if (Settings.User.setFeatures.isLateralOn && isBtnAutoSteerOn)
            {
                GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.Lateral]);        // Select Our Texture
                GL.Color3(0.590f, 0.90f, 0.93f);
                int two3 = oglMain.Width / 4;
                GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                {
                    GL.TexCoord2(1, 0); GL.Vertex2(100 - two3, bottomSide); // 
                    GL.TexCoord2(0, 0); GL.Vertex2(-100 - two3, bottomSide); // 
                    GL.TexCoord2(1, 1); GL.Vertex2(100 - two3, bottomSide + 60); // 
                    GL.TexCoord2(0, 1); GL.Vertex2(-100 - two3, bottomSide + 60); //
                }
                GL.End();
            }

            GL.Disable(EnableCap.Texture2D);
        }

        private void DrawUTurnBtn()
        {
            GL.Enable(EnableCap.Texture2D);

            if (!yt.isYouTurnTriggered)
            {
                GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.Turn]);        // Select Our Texture
                if (distancePivotToTurnLine > 0 && !yt.isOutOfBounds && yt.youTurnPhase == 255) GL.Color3(0.3f, 0.95f, 0.3f);
                else GL.Color3(0.97f, 0.635f, 0.4f);
                //mc.autoSteerData[mc.sdX] = 0;
                PGN_239.pgn[PGN_239.uturn] = 0;
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.TurnCancel]);        // Select Our Texture
                GL.Color3(0.90f, 0.90f, 0.293f);
                //mc.autoSteerData[mc.sdX] = 0;
                PGN_239.pgn[PGN_239.uturn] = 1;
            }

            int bottom = 90;
            int two3 = oglMain.Width / 5;
            GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
            if (!yt.isTurnLeft)
            {
                GL.TexCoord2(1, 0); GL.Vertex2(62 + two3, bottom); // 
                GL.TexCoord2(0, 0); GL.Vertex2(-62 + two3, bottom); // 
                GL.TexCoord2(1, 1); GL.Vertex2(62 + two3, bottom + 60); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-62 + two3, bottom + 60); //
            }
            else
            {
                GL.TexCoord2(0, 0); GL.Vertex2(62 + two3, bottom); // 
                GL.TexCoord2(1, 0); GL.Vertex2(-62 + two3, bottom); // 
                GL.TexCoord2(0, 1); GL.Vertex2(62 + two3, bottom + 60); // 
                GL.TexCoord2(1, 1); GL.Vertex2(-62 + two3, bottom + 60); //
            }
            //
            GL.End();

            //draw K turn/ normal turn button
            two3 += 140;

            GL.Color3(1.0f, 1.0f, 1.0f);
            if (yt.uTurnStyle == 0)
            {
                GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.YouTurnU]);        // Select Our Texture
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.YouTurnH]);        // Select Our Texture
            }

            GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(1, 0); GL.Vertex2(32 + two3, 100); // 
                GL.TexCoord2(0, 0); GL.Vertex2(-32 + two3, 100); // 
                GL.TexCoord2(1, 1); GL.Vertex2(32 + two3, 160); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-32 + two3, 160); //
            }
            GL.End();

            GL.Disable(EnableCap.Texture2D);
            // Done Building Triangle Strip

            two3 -= 140;
            GL.Color3(0.927f, 0.9635f, 0.74f);

            if (!yt.isYouTurnTriggered)
            {
                font.DrawText(-40 + two3, 120, DistPivot);
            }
            else
            {
                font.DrawText(-40 + two3, 120, ((yt.totalUTurnLength - yt.onA) * glm.m2FtOrM).ToString("0.0") + glm.unitsFtM);
            }
        }

        private void DrawSteerCircle()
        {
            int sizer = oglMain.Width / 15;
            int center = oglMain.Width / 2 - sizer;
            int bottomSide = oglMain.Height - sizer / 2;

            //draw the clock
            GL.Color4(0.9752f, 0.80f, 0.3f, 0.98);
            font.DrawText(center - 210, oglMain.Height - 26, DateTime.Now.ToString("T"), 0.8);

            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.SteerPointer]);        // Select Our Texture

            if (mc.steerSwitchHigh)
            {
                GL.Color4(0.9752f, 0.0f, 0.03f, 0.98);
            }
            else if (isBtnAutoSteerOn)
            {
                GL.Color4(0.052f, 0.970f, 0.03f, 0.97);
            }
            else
            {
                GL.Color4(0.952f, 0.750f, 0.03f, 0.97);
            }

            //we have lost connection to steer module
            if (steerModuleConnectedCounter++ > 30)
            {
                GL.Color4(0.952f, 0.093570f, 0.93f, 0.97);
            }

            GL.Translate(center, bottomSide, 0);
            GL.Rotate(ahrs.imuRoll, 0, 0, 1);

            GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(1, 0); GL.Vertex2(sizer, -sizer); // 
                GL.TexCoord2(0, 0); GL.Vertex2(-sizer, -sizer); // 
                GL.TexCoord2(1, 1); GL.Vertex2(sizer, sizer); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-sizer, sizer); //
            }
            GL.End();

            if ((ahrs.imuRoll != 88888))
            {
                string head = Math.Round(ahrs.imuRoll, 1).ToString();
                font.DrawText((int)(((head.Length) * -9)), -45, head, 1.2);
            }

            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);

            // stationary part
            GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.SteerDot]);        // Select Our Pinion
            GL.PushMatrix();

            GL.Translate(center, bottomSide, 0);

            GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(1, 0); GL.Vertex2(sizer, -sizer); // 
                GL.TexCoord2(0, 0); GL.Vertex2(-sizer, -sizer); // 
                GL.TexCoord2(1, 1); GL.Vertex2(sizer, sizer); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-sizer, sizer); //
            }
            GL.End();

            GL.Disable(EnableCap.Texture2D);

            GL.PopMatrix();
        }

        private void DrawTramMarkers()
        {
            //int sizer = 60;
            int center = -50;
            int bottomSide = oglMain.Height / 5;

            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, texture[(int)textures.TramDot]);        // Select Our Texture

            if (((tram.controlByte) & 2) == 2) GL.Color4(0.29f, 0.990f, 0.290f, 0.983f);
            else GL.Color4(0.9f, 0.0f, 0.0f, 0.53f);

            if (tram.isLeftManualOn)
            {
                if (isFlashOnOff) GL.Color4(0.0f, 0.0f, 0.0f, 0.993f);
                else GL.Color4(0.99f, 0.990f, 0.0f, 0.993f);
            }

            GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(1, 0); GL.Vertex2(center + 24, bottomSide - 24); // 
                GL.TexCoord2(0, 0); GL.Vertex2(center - 24, bottomSide - 24); // 
                GL.TexCoord2(1, 1); GL.Vertex2(center + 24, bottomSide + 24); // 
                GL.TexCoord2(0, 1); GL.Vertex2(center - 24, bottomSide + 24); //
            }
            GL.End();

            if (((tram.controlByte) & 1) == 1) GL.Color4(0.29f, 0.990f, 0.290f, 0.983f);
            else GL.Color4(0.9f, 0.0f, 0.0f, 0.53f);

            if (tram.isRightManualOn)
            {
                if (isFlashOnOff) GL.Color4(0.0f, 0.0f, 0.0f, 0.993f);
                else GL.Color4(0.99f, 0.990f, 0.0f, 0.993f);
            }

            center += 100;

            GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(1, 0); GL.Vertex2(center + 24, bottomSide - 24); // 
                GL.TexCoord2(0, 0); GL.Vertex2(center - 24, bottomSide - 24); // 
                GL.TexCoord2(1, 1); GL.Vertex2(center + 24, bottomSide + 24); // 
                GL.TexCoord2(0, 1); GL.Vertex2(center - 24, bottomSide + 24); //
            }
            GL.End();

            GL.Disable(EnableCap.Texture2D);

            //if (mf.tram.displayMode != 0)
            //{
            //    if (mf.camera.camSetDistance > -300)
            //    {
            //        if (mf.camera.camSetDistance > -100)
            //            GL.PointSize(16);
            //        else GL.PointSize(12);

            //        if (mf.tram.isOuter)
            //        {
            //            //section markers
            //            GL.Begin(PrimitiveType.Points);

            //            //right side
            //            if (((mf.tram.controlByte) & 1) == 1) GL.Color3(0.0f, 0.900f, 0.39630f);
            //            else GL.Color3(0, 0, 0);
            //            GL.Vertex3(farRightPosition - mf.tram.halfWheelTrack, trailingTool, 0);

            //            //left side
            //            if ((mf.tram.controlByte & 2) == 2) GL.Color3(0.0f, 0.900f, 0.3930f);
            //            else GL.Color3(0, 0, 0);
            //            GL.Vertex3(farLeftPosition + mf.tram.halfWheelTrack, trailingTool, 0);
            //            GL.End();
            //        }
            //        else
            //        {
            //            GL.Begin(PrimitiveType.Points);

            //            //right side
            //            if (((mf.tram.controlByte) & 1) == 1) GL.Color3(0.0f, 0.900f, 0.39630f);
            //            else GL.Color3(0, 0, 0);
            //            GL.Vertex3(mf.tram.halfWheelTrack, trailingTool, 0);

            //            //left side
            //            if ((mf.tram.controlByte & 2) == 2) GL.Color3(0.0f, 0.900f, 0.3930f);
            //            else GL.Color3(0, 0, 0);
            //            GL.Vertex3(-mf.tram.halfWheelTrack, trailingTool, 0);
            //            GL.End();
            //        }
            //    }
            //}
        }

        private void MakeFlagMark()
        {

            leftMouseDownOnOpenGL = false;

            try
            {
                byte[] data1 = new byte[768];

                //scan the center of click and a set of square points around
                GL.ReadPixels(mouseX - 8, mouseY - 8, 16, 16, PixelFormat.Rgb, PixelType.UnsignedByte, data1);

                //made it here so no flag found
                flagNumberPicked = 0;

                for (int ctr = 0; ctr < 768; ctr += 3)
                {
                    if (data1[ctr] == 255 | data1[ctr + 1] == 255)
                    {
                        flagNumberPicked = data1[ctr + 2];
                        break;
                    }
                }

                if (flagNumberPicked > 0)
                {
                    Form fc = Application.OpenForms["FormFlags"];

                    if (fc != null)
                    {
                        fc.Focus();
                        return;
                    }

                    if (flagPts.Count > 0)
                    {
                        Form form = new FormFlags(this);
                        form.Show(this);
                    }
                }
            }
            catch
            {
                flagNumberPicked = 0;
            }
        }

        private void DrawFlags()
        {
            try
            {
                int flagCnt = flagPts.Count;
                for (int f = 0; f < flagCnt; f++)
                {
                    GL.PointSize(8.0f);
                    GL.Begin(PrimitiveType.Points);
                    string flagColor = "&";
                    if (flagPts[f].color == 0)
                    {
                        GL.Color3((byte)255, (byte)0, (byte)flagPts[f].ID);
                    }
                    if (flagPts[f].color == 1)
                    {
                        GL.Color3((byte)0, (byte)255, (byte)flagPts[f].ID);
                        flagColor = "|";
                    }
                    if (flagPts[f].color == 2)
                    {
                        GL.Color3((byte)255, (byte)255, (byte)flagPts[f].ID);
                        flagColor = "~";
                    }

                    GL.Vertex3(flagPts[f].easting, flagPts[f].northing, 0);
                    GL.End();

                    font.DrawText3D(flagPts[f].easting, flagPts[f].northing, flagColor + flagPts[f].notes, true);
                    //else
                    //    font.DrawText3D(flagPts[f].easting, flagPts[f].northing, "&");
                }

                if (flagNumberPicked != 0)
                {
                    ////draw the box around flag
                    double offSet = (Settings.User.setDisplay_camZoom * Settings.User.setDisplay_camZoom * 0.01);
                    GL.LineWidth(4);
                    GL.Color3(0.980f, 0.0f, 0.980f);
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Vertex3(flagPts[flagNumberPicked - 1].easting, flagPts[flagNumberPicked - 1].northing + offSet, 0);
                    GL.Vertex3(flagPts[flagNumberPicked - 1].easting - offSet, flagPts[flagNumberPicked - 1].northing, 0);
                    GL.Vertex3(flagPts[flagNumberPicked - 1].easting, flagPts[flagNumberPicked - 1].northing - offSet, 0);
                    GL.Vertex3(flagPts[flagNumberPicked - 1].easting + offSet, flagPts[flagNumberPicked - 1].northing, 0);
                    GL.Vertex3(flagPts[flagNumberPicked - 1].easting, flagPts[flagNumberPicked - 1].northing + offSet, 0);
                    GL.End();

                    //draw the flag with a black dot inside
                    //GL.PointSize(4.0f);
                    //GL.Color3(0, 0, 0);
                    //GL.Begin(PrimitiveType.Points);
                    //GL.Vertex3(flagPts[flagNumberPicked - 1].easting, flagPts[flagNumberPicked - 1].northing, 0);
                    //GL.End();
                }
            }
            catch { }
        }

        private void DrawLightBar(double offlineDistance)
        {
            double down = 25;
            GL.LineWidth(1);
            //GL.Translate(0, 0, 0.01);
            //offlineDistance *= -1;
            //  Dot distance is representation of how far from AB Line
            int dotDistance = (int)(offlineDistance);
            int limit = (int)Settings.User.setDisplay_lightbarCmPerPixel * 8;
            if (dotDistance < -limit) dotDistance = -limit;
            if (dotDistance > limit) dotDistance = limit;

            //if (dotDistance < -10) dotDistance -= 30;
            //if (dotDistance > 10) dotDistance += 30;

            // dot background
            GL.PointSize(8.0f);
            GL.Color3(0.00f, 0.0f, 0.0f);
            GL.Begin(PrimitiveType.Points);
            for (int i = -8; i < -1; i++) GL.Vertex2((i * 32), down);
            for (int i = 2; i < 9; i++) GL.Vertex2((i * 32), down);
            GL.End();

            GL.PointSize(4.0f);
            GL.Translate(0, 0, 0.01);

            //red left side
            GL.Color3(0.750f, 0.0f, 0.0f);
            GL.Begin(PrimitiveType.Points);
            for (int i = -8; i < -1; i++) GL.Vertex2((i * 32), down);

            //green right side
            GL.Color3(0.0f, 0.750f, 0.0f);
            for (int i = 2; i < 9; i++) GL.Vertex2((i * 32), down);
            GL.End();

            //Are you on the right side of line? So its green.
            if ((offlineDistance) < 0.0)
            {
                int dots = (dotDistance * -1 / Settings.User.setDisplay_lightbarCmPerPixel) + 1;

                GL.PointSize(24.0f);
                GL.Color3(0.0f, 0.0f, 0.0f);
                GL.Begin(PrimitiveType.Points);
                for (int i = 2; i < dots + 1; i++) GL.Vertex2((i * 32), down);
                GL.End();

                GL.PointSize(16.0f);
                GL.Color3(0.0f, 0.980f, 0.0f);
                GL.Begin(PrimitiveType.Points);
                for (int i = 1; i < dots; i++) GL.Vertex2((i * 32 + 32), down);
                GL.End();
                //return;
            }

            else //red side
            {
                int dots = (int)(dotDistance / Settings.User.setDisplay_lightbarCmPerPixel) + 1;

                GL.PointSize(24.0f);
                GL.Color3(0.0f, 0.0f, 0.0f);
                GL.Begin(PrimitiveType.Points);
                for (int i = 2; i < dots + 1; i++) GL.Vertex2((i * -32), down);
                GL.End();

                GL.PointSize(16.0f);
                GL.Color3(0.980f, 0.30f, 0.0f);
                GL.Begin(PrimitiveType.Points);
                for (int i = 1; i < dots; i++) GL.Vertex2((i * -32 - 32), down);
                GL.End();
                //return;
            }
        }

        private void DrawLightBarText()
        {
            if (trk.currentGuidanceTrack.Count > 1 && !double.IsNaN(guidanceLineDistanceOff))
            {
                avgPivDistance = avgPivDistance * 0.5 + guidanceLineDistanceOff * 0.5;

                double avgPivotDistance = avgPivDistance * glm.m2InchOrCm;

                if (Settings.User.isLightbarOn) DrawLightBar(avgPivotDistance);

                if (avgPivotDistance > 999) avgPivotDistance = 999;
                if (avgPivotDistance < -999) avgPivotDistance = -999;

                string hede = ".0.";

                if (avgPivotDistance > 0.99)
                {
                    //GL.Color3(0.9752f, 0.50f, 0.3f);
                    hede = (Math.Abs(avgPivotDistance)).ToString("N0");
                }
                else if (avgPivotDistance < -0.99)
                {
                    //GL.Color3(0.50f, 0.952f, 0.3f);
                    hede = (Math.Abs(avgPivotDistance)).ToString("N0");
                }

                int center = -(int)(((double)(hede.Length) * 0.5) * 22);

                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.CrossTrackBkgrnd]);        // Select Our Texture

                double green = Math.Abs(avgPivDistance);
                double red = green;
                if (green > 0.4) green = 0.4;
                green = (0.4 - green) + 0.58;

                if (red > 0.4) red = 0.4;
                red = 2 * red;

                GL.Color4(red, green, 0.3, 1.0);

                GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip

                //int wide = (int)((double)oglMain.Width / 12);
                //if (wide < 75) wide = 75;
                int wide = 50;

                GL.TexCoord2(1, 1); GL.Vertex2(wide, 50); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-wide, 50); // 
                GL.TexCoord2(1, 0); GL.Vertex2(wide, 2); // 
                GL.TexCoord2(0, 0); GL.Vertex2(-wide, 2); //

                GL.End();
                GL.Disable(EnableCap.Texture2D);

                GL.Color4(0.0, 0.0, 0.0, 1.0);
                font.DrawText(center, 2, hede, 1.5);

                if (avgPivDistance > 0.15) longAvgPivDistance = 0.15;
                longAvgPivDistance = longAvgPivDistance * 0.97 + Math.Abs(avgPivDistance) * 0.03;

                if (longAvgPivDistance < 150)
                {
                    hede = (Math.Abs(longAvgPivDistance * glm.m2InchOrCm)).ToString("N1");

                    GL.Color3(0.950f, 0.952f, 0.3f);
                    center = -(int)(((double)(hede.Length) * 0.5) * 16);
                    font.DrawText(center, 45, hede, 1);
                }
            }
        }

        private void DrawSteerBarText()
        {
            if (trk.currentGuidanceTrack.Count > 1 && !double.IsNaN(guidanceLineDistanceOff))
            {
                int spacing = oglMain.Width / 50;
                if (spacing < 28) spacing = 28;
                int offset = (int)((double)oglMain.Height / 40);
                int line = 12;
                int line2 = 8;

                //int down = (int)((double)oglMain.Height/38);
                int down = 58 + (int)((double)(oglMain.Height - 600) / 17);

                double textSize = ((100 + (double)(oglMain.Height - 600)) * 0.0012) + 1;

                int pointy = 24;

                double alphaBar = 1.0;
                if (isBtnAutoSteerOn) alphaBar = 0.5;

                avgPivDistance = avgPivDistance * 0.8 + guidanceLineDistanceOff * 0.2;

                // in millimeters
                double avgPivotDistance = avgPivDistance * glm.m2InchOrCm;
                double err = mc.actualSteerAngleDegrees - guidanceLineSteerAngle;


                if (isBtnAutoSteerOn)
                {
                    if (Math.Abs(err) < 0.5) err = 0;
                    offset = (int)((double)oglMain.Height / 60);
                    line /= 2;
                    line2 /= 2;
                }
                else
                {
                    if (Math.Abs(err) < 0.2) err = 0;
                }

                double errLine = err;
                if (errLine > 12) errLine = 12;
                if (errLine < -12) errLine = -12;
                errLine *= spacing;

                if (errLine > 0) errLine += 35;
                else errLine -= 35;

                if (err != 0)
                {
                    GL.Color4(0, 0, 0, alphaBar);
                    GL.LineWidth(line);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex2(0, down);
                    GL.Vertex2(errLine, down);
                    GL.End();
                    GL.Color4(0.950f, 0.986530f, 0.40f, alphaBar);
                    GL.LineWidth(line2);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex2(0, down);
                    GL.Vertex2(errLine, down);
                    GL.End();


                    if ((err) > 0.0)
                    {
                        spacing *= -1;
                        offset *= -1;
                        pointy *= -1;
                    }

                    GL.Color4(0, 0.99, 0, alphaBar);
                    GL.Begin(PrimitiveType.TriangleStrip);
                    GL.Vertex2((errLine), down - offset);
                    GL.Vertex2((errLine + offset + pointy), down);
                    GL.Vertex2((errLine), down + offset);
                    GL.End();

                    GL.Color4(0.79, 0.79, 0, alphaBar);

                    GL.Begin(PrimitiveType.TriangleStrip);
                    GL.Vertex2((0), down - offset);
                    GL.Vertex2((0 + offset + pointy), down);
                    GL.Vertex2((0), down + offset);
                    GL.End();

                    GL.LineWidth(3);
                    GL.Color4(0, 0, 0, alphaBar);

                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex2((errLine), down - offset);
                    GL.Vertex2((errLine + offset + pointy), down);
                    GL.Vertex2((errLine), down + offset);
                    GL.End();

                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex2((0), down - offset);
                    GL.Vertex2((0 + offset + pointy), down);
                    GL.Vertex2((0), down + offset);
                    GL.End();
                }

                int center = 0;
                string hede = "> 0 <";

                if (avgPivotDistance > 999) avgPivotDistance = 999;
                if (avgPivotDistance < -999) avgPivotDistance = -999;

                if (Math.Abs(avgPivotDistance) > 0.9999)
                {
                    if (avgPivotDistance < 0.0)
                    {
                        hede = (Math.Abs(avgPivotDistance)).ToString("N0") + " >";
                        center = -(int)(((double)(hede.Length) * 0.5) * (18 * (textSize)));
                    }
                    else
                    {
                        hede = "< " + (Math.Abs(avgPivotDistance)).ToString("N0");
                        center = -(int)(((double)(hede.Length) * 0.5) * (18 * (textSize)));
                    }
                }
                else
                {
                    center = (int)(-40 * (textSize));
                }


                int wide = (int)((double)oglMain.Width / 18);
                if (wide < 64) wide = 64;

                // Select Our Texture
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.CrossTrackBkgrnd]);

                double green = Math.Abs(avgPivDistance);
                double red = green;
                if (green > 0.4) green = 0.4;
                green = (0.7 - green) + 0.28;

                if (red > 0.4) red = 0.4;
                red = 2 * red;

                GL.Color4(red, green, 0.13, 1.0);

                GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                GL.TexCoord2(1, 1); GL.Vertex2(wide, 3); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-wide, 3); // 
                GL.TexCoord2(1, 0); GL.Vertex2(wide, 35 * (textSize)); // 
                GL.TexCoord2(0, 0); GL.Vertex2(-wide, 35 * (textSize)); //
                GL.End();

                GL.Disable(EnableCap.Texture2D);

                GL.Color4(0.12f, 0.12770f, 0.120f, 1);

                font.DrawText(center, 2, hede, textSize);

                if (isGPSToolActive && !double.IsNaN(gyd.distanceFromCurrentLineTool))
                {
                    //tool xte
                    avgPivDistanceTool = avgPivDistanceTool * 0.5 + gyd.distanceFromCurrentLineTool * 0.5;
                    double avgPivotDistanceTool = avgPivDistanceTool * glm.m2InchOrCm;
                    if (avgPivotDistanceTool > 999) avgPivotDistanceTool = 999;
                    if (avgPivotDistanceTool < -999) avgPivotDistanceTool = -999;

                    hede = "> 0 <";

                    if (Math.Abs(avgPivotDistanceTool) > 0.9999)
                    {
                        if (avgPivotDistanceTool < 0.0)
                        {
                            hede = (Math.Abs(avgPivotDistanceTool)).ToString("N0") + " >";
                            center = -(int)(((double)(hede.Length) * 0.5) * (18 * (textSize)) - 0);
                        }
                        else
                        {
                            hede = "< " + (Math.Abs(avgPivotDistanceTool)).ToString("N0");
                            center = -(int)(((double)(hede.Length) * 0.5) * (18 * (textSize)));
                        }
                    }
                    else
                    {
                        center = (int)(-40 * (textSize));
                    }

                    center += (int)(120 * (textSize));

                    // Select Our Texture
                    GL.Enable(EnableCap.Texture2D);
                    GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.CrossTrackBkgrnd]);

                    green = Math.Abs(avgPivDistanceTool);
                    red = green;
                    if (green > 0.4) green = 0.4;
                    green = (0.7 - green) + 0.28;

                    if (red > 0.4) red = 0.4;
                    red = 2 * red;

                    GL.Color4(red, green, 0.3, 1.0);

                    double moveOver = 120 * (textSize);

                    GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                    GL.TexCoord2(1, 1); GL.Vertex2(moveOver + wide, 3); // 
                    GL.TexCoord2(0, 1); GL.Vertex2(moveOver + -wide, 3); // 
                    GL.TexCoord2(1, 0); GL.Vertex2(moveOver + wide, 35 * (textSize)); // 
                    GL.TexCoord2(0, 0); GL.Vertex2(moveOver + -wide, 35 * (textSize)); //
                    GL.End();

                    GL.Disable(EnableCap.Texture2D);

                    GL.Color4(0.12f, 0.12770f, 0.120f, 1);

                    font.DrawText(center, 2, hede, textSize);
                }

                if (vehicle.isInDeadZone)
                {
                    GL.Color4(0.512f, 0.9712770f, 0.5120f, 1);
                    GL.LineWidth(4);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex2(-wide, 36 * (textSize));
                    GL.Vertex2(wide, 36 * (textSize));
                    GL.End();
                }
            }
        }

        private void DrawTrackInfo()
        {
            string offs = "";

            if (trk.currTrk.nudgeDistance != 0)
                offs = ((int)(trk.currTrk.nudgeDistance * glm.m2InchOrCm)).ToString() + glm.unitsInCmNS;

            string dire;

            if (trk.isHeadingSameWay) dire = "{";
            else dire = "}";

            GL.Color4(1.269, 1.25, 1.2510, 0.87);
            if (trk.howManyPathsAway > -1)
                dire = dire + (trk.howManyPathsAway + 1).ToString() + "R " + offs;
            else
                dire = dire + (-trk.howManyPathsAway).ToString() + "L " + offs;

            int start = -(int)(((double)(dire.Length) * 0.45) * (20 * (1.0)));
            int down = 68 + (int)((double)(oglMain.Height - 600) / 12);
            double textSize = (100 + (double)(oglMain.Height - 600)) * 0.0012 + 1;

            GL.Color4(0.9, 0.9, 0.9, 0.8);

            font.DrawText(start, down, dire, textSize);
        }

        private void DrawCompassText()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.ZoomIn48]);        // Select Our Texture
            GL.Color3(0.90f, 0.90f, 0.93f);

            int center = oglMain.Width / 2 - 60;

            GL.Begin(PrimitiveType.TriangleStrip);             // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(1, 0); GL.Vertex2(center + 32, 50); // 
                GL.TexCoord2(0, 0); GL.Vertex2(center, 50); // 
                GL.TexCoord2(1, 1); GL.Vertex2(center + 32, 82); // 
                GL.TexCoord2(0, 1); GL.Vertex2(center, 82); //
            }
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.ZoomOut48]);        // Select Our Texture
            GL.Begin(PrimitiveType.TriangleStrip);             // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(1, 0); GL.Vertex2(center + 32, 200); // 
                GL.TexCoord2(0, 0); GL.Vertex2(center, 200); // 
                GL.TexCoord2(1, 1); GL.Vertex2(center + 32, 232); // 
                GL.TexCoord2(0, 1); GL.Vertex2(center, 232); //
            }
            GL.End();

            //Pan
            if (isFieldStarted)
            {
                center = oglMain.Width / -2 + 30;
                if (!isPanFormVisible)
                {
                    GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.Pan]);        // Select Our Texture
                    GL.Begin(PrimitiveType.TriangleStrip);             // Build Quad From A Triangle Strip
                    {
                        GL.TexCoord2(1, 0); GL.Vertex2(center + 32, 50); // 
                        GL.TexCoord2(0, 0); GL.Vertex2(center, 50); // 
                        GL.TexCoord2(1, 1); GL.Vertex2(center + 32, 82); // 
                        GL.TexCoord2(0, 1); GL.Vertex2(center, 82); //
                    }
                    GL.End();
                }

                //hide show bottom menu
                int hite = oglMain.Height - 30;
                GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.MenuHideShow]);        // Select Our Texture
                GL.Begin(PrimitiveType.TriangleStrip);             // Build Quad From A Triangle Strip
                {
                    GL.TexCoord2(1, 0); GL.Vertex2(center + 32, hite - 32); // 
                    GL.TexCoord2(0, 0); GL.Vertex2(center, hite - 32); // 
                    GL.TexCoord2(1, 1); GL.Vertex2(center + 32, hite); // 
                    GL.TexCoord2(0, 1); GL.Vertex2(center, hite); //
                }
                GL.End();

                center += 50;
                font.DrawText(center - 56, hite - 72, "x" + gridToolSpacing.ToString(), 1);
            }

            center = oglMain.Width / -2 + 10;
            double deg = glm.toDegrees(fixHeading);
            if (deg > 359.9) deg = 359.9;
            string strHeading = (deg).ToString("N1");
            int lenth = 18 * strHeading.Length;

            GL.Disable(EnableCap.Texture2D);
            GL.Color3(0.9852f, 0.982f, 0.983f);
            font.DrawText(oglMain.Width / 2 - lenth, 10, strHeading, 1);

            //angular velocity
            strHeading = ahrs.angVel.ToString();
            font.DrawText(center, 80, strHeading, 1);

            //GPS Step
            if (distanceCurrentStepFixDisplay < 0.03 * 100)
                GL.Color3(0.98f, 0.82f, 0.653f);
            font.DrawText(center, 10, distanceCurrentStepFixDisplay.ToString("N1") + "cm", 1);

            if (isMaxAngularVelocity)
            {
                GL.Color3(0.98f, 0.4f, 0.4f);
                font.DrawText(center - 10, oglMain.Height - 260, "*", 2);
            }
        }

        private void DrawCompass()
        {
            //Heading text
            int center = oglMain.Width / 2 - 48;

            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.Compass]);        // Select Our Texture
            GL.Color4(0.952f, 0.870f, 0.73f, 0.8);


            GL.Translate(center, 140, 0);

            GL.Rotate(camHeading, 0, 0, 1);
            GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(1, 0); GL.Vertex2(42, -42.0); // 
                GL.TexCoord2(0, 0); GL.Vertex2(-42, -42); // 
                GL.TexCoord2(1, 1); GL.Vertex2(42, 42); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-42, 42); //
            }
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
        }

        private void DrawReverse()
        {
            //if (isReverse)// && ahrs.imuHeading != 99999)
            GL.Color3(0.952f, 0.9520f, 0.0f);
            //else if (isReverse) GL.Color3(0.952f, 0.0f, 0.0f);
            //else GL.Color3(0.952f, 0.990f, 0.0f);//isChangingDirection

            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.Lift]);        // Select Our Texture

            GL.Translate(-oglMain.Width / 12, oglMain.Height / 2 - 20, 0);
            GL.Rotate(180, 0, 0, 1);

            GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(1, 0.15); GL.Vertex2(32, -32.0); // 
                GL.TexCoord2(0, 0.15); GL.Vertex2(-32, -32); // 
                GL.TexCoord2(1, 1); GL.Vertex2(32, 32); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-32, 32); //
            }
            GL.End();

            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
        }

        private void DrawLiftIndicator()
        {
            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.Lift]);        // Select Our Texture

            GL.Translate(oglMain.Width / 2 - 35, oglMain.Height / 2, 0);

            if (PGN_239.pgn[PGN_239.hydLift] == 2)
            {
                GL.Color3(0.0f, 0.950f, 0.0f);
            }
            else
            {
                GL.Rotate(180, 0, 0, 1);
                GL.Color3(0.952f, 0.40f, 0.0f);
            }

            GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(1, 0); GL.Vertex2(48, -64.0); // 
                GL.TexCoord2(0, 0); GL.Vertex2(-48, -64); // 
                GL.TexCoord2(1, 1); GL.Vertex2(48, 64); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-48, 64); //
            }
            GL.End();

            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();

        }

        private void DrawSpeedo()
        {
            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.Speedo]);        // Select Our Texture
            GL.Color4(0.952f, 0.980f, 0.98f, 0.99);

            GL.Translate(oglMain.Width / 2 - 130, 65, 0);

            GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(1, 0); GL.Vertex2(58, -58.0); // 
                GL.TexCoord2(0, 0); GL.Vertex2(-58, -58); // 
                GL.TexCoord2(1, 1); GL.Vertex2(58, 58); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-58, 58); //
            }
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.SpeedoNeedle]);        // Select Our Texture

            double aveSpd = Math.Abs(avgSpeed * glm.kmhToMphOrKmh);
            if (aveSpd > 20) aveSpd = 20;
            double angle = (aveSpd - 10) * 15;

            if (avgSpeed > -0.1) GL.Color3(0.850f, 0.950f, 0.30f);
            else GL.Color3(0.952f, 0.0f, 0.0f);

            GL.Rotate(angle, 0, 0, 1);
            GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(1, 0); GL.Vertex2(48, -48.0); // 
                GL.TexCoord2(0, 0); GL.Vertex2(-48, -48); // 
                GL.TexCoord2(1, 1); GL.Vertex2(48, 48); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-48, 48); //
            }
            GL.End();

            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();

        }

        private void DrawLostRTK()
        {
            GL.Color3(0.9752f, 0.752f, 0.40f);
            font.DrawText(-oglMain.Width / 3, oglMain.Height / 3, "RTK Fix Lost", 2);
        }

        private void DrawAge()
        {
            GL.Color3(0.9752f, 0.52f, 0.0f);
            font.DrawText(oglMain.Width / 4, 60, "Age:" + pn.age.ToString("N1"), 1.5);
        }

        private void DrawGuidanceLineText()
        {
            if (guideLineCounter > 0)
            {
                guideLineCounter--;

                if (guideLineCounter == 0)
                {
                    lblGuidanceLine.Visible = false;
                }
            }
        }

        private void DrawHardwareMessageText()
        {
            if (hardwareLineCounter > 0)
            {
                hardwareLineCounter--;

                if (hardwareLineCounter == 0)
                {
                    lblHardwareMessage.Visible = false;
                }
            }
        }

        private void DistanceToFieldOriginCheck()
        {
            if (Math.Abs(pivotAxlePos.easting) > 20000 || Math.Abs(pivotAxlePos.northing) > 20000)
            {
                YesMessageBox("Serious Field Origin Error" + "\r\n\r\n" +
                    "Field Origin is More Then 20 km from your current GPS Position" +
                    " Delete this field and create a new one as Accuracy will be poor" + "\r\n\r\n" +
                    "Or you may have a field open and drove far away");
            }
        }

        private void CalcFrustum()
        {
            float[] proj = new float[16];							// For Grabbing The PROJECTION Matrix
            float[] modl = new float[16];							// For Grabbing The MODELVIEW Matrix
            float[] clip = new float[16];							// Result Of Concatenating PROJECTION and MODELVIEW

            GL.GetFloat(GetPName.ProjectionMatrix, proj);	// Grab The Current PROJECTION Matrix
            GL.GetFloat(GetPName.Modelview0MatrixExt, modl);   // Grab The Current MODELVIEW Matrix  

            // Concatenate (Multiply) The Two Matricies
            clip[0] = modl[0] * proj[0] + modl[1] * proj[4] + modl[2] * proj[8] + modl[3] * proj[12];
            clip[1] = modl[0] * proj[1] + modl[1] * proj[5] + modl[2] * proj[9] + modl[3] * proj[13];
            clip[2] = modl[0] * proj[2] + modl[1] * proj[6] + modl[2] * proj[10] + modl[3] * proj[14];
            clip[3] = modl[0] * proj[3] + modl[1] * proj[7] + modl[2] * proj[11] + modl[3] * proj[15];

            clip[4] = modl[4] * proj[0] + modl[5] * proj[4] + modl[6] * proj[8] + modl[7] * proj[12];
            clip[5] = modl[4] * proj[1] + modl[5] * proj[5] + modl[6] * proj[9] + modl[7] * proj[13];
            clip[6] = modl[4] * proj[2] + modl[5] * proj[6] + modl[6] * proj[10] + modl[7] * proj[14];
            clip[7] = modl[4] * proj[3] + modl[5] * proj[7] + modl[6] * proj[11] + modl[7] * proj[15];

            clip[8] = modl[8] * proj[0] + modl[9] * proj[4] + modl[10] * proj[8] + modl[11] * proj[12];
            clip[9] = modl[8] * proj[1] + modl[9] * proj[5] + modl[10] * proj[9] + modl[11] * proj[13];
            clip[10] = modl[8] * proj[2] + modl[9] * proj[6] + modl[10] * proj[10] + modl[11] * proj[14];
            clip[11] = modl[8] * proj[3] + modl[9] * proj[7] + modl[10] * proj[11] + modl[11] * proj[15];

            clip[12] = modl[12] * proj[0] + modl[13] * proj[4] + modl[14] * proj[8] + modl[15] * proj[12];
            clip[13] = modl[12] * proj[1] + modl[13] * proj[5] + modl[14] * proj[9] + modl[15] * proj[13];
            clip[14] = modl[12] * proj[2] + modl[13] * proj[6] + modl[14] * proj[10] + modl[15] * proj[14];
            clip[15] = modl[12] * proj[3] + modl[13] * proj[7] + modl[14] * proj[11] + modl[15] * proj[15];


            // Extract the RIGHT clipping plane
            frustum[0] = clip[3] - clip[0];
            frustum[1] = clip[7] - clip[4];
            frustum[2] = clip[11] - clip[8];
            frustum[3] = clip[15] - clip[12];

            // Extract the LEFT clipping plane
            frustum[4] = clip[3] + clip[0];
            frustum[5] = clip[7] + clip[4];
            frustum[6] = clip[11] + clip[8];
            frustum[7] = clip[15] + clip[12];

            // Extract the FAR clipping plane
            frustum[8] = clip[3] - clip[2];
            frustum[9] = clip[7] - clip[6];
            frustum[10] = clip[11] - clip[10];
            frustum[11] = clip[15] - clip[14];


            // Extract the NEAR clipping plane.  This is last on purpose (see pointinfrustum() for reason)
            frustum[12] = clip[3] + clip[2];
            frustum[13] = clip[7] + clip[6];
            frustum[14] = clip[11] + clip[10];
            frustum[15] = clip[15] + clip[14];

            // Extract the BOTTOM clipping plane
            frustum[16] = clip[3] + clip[1];
            frustum[17] = clip[7] + clip[5];
            frustum[18] = clip[11] + clip[9];
            frustum[19] = clip[15] + clip[13];

            // Extract the TOP clipping plane
            frustum[20] = clip[3] - clip[1];
            frustum[21] = clip[7] - clip[5];
            frustum[22] = clip[11] - clip[9];
            frustum[23] = clip[15] - clip[13];
        }

        public double maxFieldX, maxFieldY, minFieldX, minFieldY, fieldCenterX, fieldCenterY, maxFieldDistance, maxCrossFieldLength;

        //determine mins maxs of patches and whole field.
        public void CalculateSectionPatchesMinMax()
        {

            minFieldX = 9999999; minFieldY = 9999999;
            maxFieldX = -9999999; maxFieldY = -9999999;


            //min max of the boundary
            //min max of the boundary
            if (bnd.bndList.Count > 0)
            {
                foreach (vec3 point in bnd.bndList[0].fenceLine)
                {
                    double x = point.easting;
                    double y = point.northing;

                    //also tally the max/min of field x and z
                    if (minFieldX > x) minFieldX = x;
                    if (maxFieldX < x) maxFieldX = x;
                    if (minFieldY > y) minFieldY = y;
                    if (maxFieldY < y) maxFieldY = y;
                }
            }
            else
            {
                //for every new chunk of patch
                foreach (var triList in patchList)
                {
                    int count2 = triList.Count;
                    for (int i = 1; i < count2; i += 3)
                    {
                        double x = triList[i].easting;
                        double y = triList[i].northing;

                        //also tally the max/min of field x and z
                        if (minFieldX > x) minFieldX = x;
                        if (maxFieldX < x) maxFieldX = x;
                        if (minFieldY > y) minFieldY = y;
                        if (maxFieldY < y) maxFieldY = y;
                    }
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

                maxCrossFieldLength = Math.Sqrt(dist * dist + dist2 * dist2) * 1.0;

                if (dist > dist2) maxFieldDistance = (dist);
                else maxFieldDistance = (dist2);

                if (maxFieldDistance < 100) maxFieldDistance = 100;
                if (maxFieldDistance > 5000) maxFieldDistance = 5000;
                //lblMax.Text = ((int)maxFieldDistance).ToString();

                fieldCenterX = (maxFieldX + minFieldX) / 2.0;
                fieldCenterY = (maxFieldY + minFieldY) / 2.0;
            }
        }
        private void oglBackStart()
        {
            Stopwatch BBtimer = new Stopwatch();
            oglBack.Context.MakeCurrent(null); //Unbinds the context from the current thread.
            thread_oglBack = new Thread(() =>
            {
                oglBack.Context.MakeCurrent(oglBack.WindowInfo); //Bimds the OpenGL context to this new thread
                while (true)
                {
                    BBtimer.Restart();

                    BBtimer.Stop();
                    Debug.WriteLine(BBtimer.ElapsedTicks * 1_000_000 / Stopwatch.Frequency);
                    // moderate speed
                    pauseOglBack.WaitOne();
                }
            });

            thread_oglBack.Start();

        }

        private void oglBackPGN_FileSave()
        {
        }

    }
}

//in section patch math code
/*
 *                 //to draw or not the triangle patch
                pivEplus = pivotAxlePos.easting + 50;
                pivEminus = pivotAxlePos.easting - 50;
                pivNplus = pivotAxlePos.northing + 50;
                pivNminus = pivotAxlePos.northing - 50;

                int bob = 0;
                //draw patches j= # of sections
                for (int k = 0; k < triStrip.Count; k++)
                {
                    //every time the section turns off and on is a new patch
                    int patchCount = triStrip[k].patchList.Count;

                    if (patchCount > 0)
                    {
                        //for every new chunk of patch
                        foreach (var triList in triStrip[k].patchList)
                        {
                            isDraw = false;
                            int count2 = triList.Count;
                            for (int i = 1; i < count2; i += 3)
                            {
                                //determine if point is in frustum or not
                                if (triList[i].easting > pivEplus)
                                    continue;
                                if (triList[i].easting < pivEminus)
                                    continue;
                                if (triList[i].northing > pivNplus)
                                    continue;
                                if (triList[i].northing < pivNminus)
                                    continue;

                                //point is in frustum so draw the entire patch
                                isDraw = true;
                                break;
                            }

                            if (isDraw)
                            {
                                ////draw the triangles in each triangle strip
                                //GL.Begin(PrimitiveType.TriangleStrip);
                                for (int i = 1; i < count2-3; i+=2)
                                {
                                    // D = (x2 - x1) * (yp - y1) - (xp - x1) * (y2 - y1)

                                    double D = (triList[i + 1].easting - triList[i].easting) * (pivotAxlePos.northing - triList[i].northing)
                                        - (pivotAxlePos.easting - triList[i].easting) * (triList[i + 1].northing - triList[i].northing);
                                    if (D < 0) continue;

                                        D = (triList[i + 3].easting - triList[i + 1].easting) * (pivotAxlePos.northing - triList[i + 1].northing)
                                        - (pivotAxlePos.easting - triList[i + 1].easting) * (triList[i + 3].northing - triList[i + 1].northing);
                                    if (D < 0) continue;

                                    D = (triList[i + 2].easting - triList[i + 3].easting) * (pivotAxlePos.northing - triList[i + 3].northing)
                                        - (pivotAxlePos.easting - triList[i + 3].easting) * (triList[i + 2].northing - triList[i + 3].northing);
                                    if (D < 0) continue;

                                    D = (triList[i].easting - triList[i + 2].easting) * (pivotAxlePos.northing - triList[i + 2].northing)
                                        - (pivotAxlePos.easting - triList[i + 2].easting) * (triList[i].northing - triList[i + 2].northing);
                                    if (D < 0) continue;

                                    bob++;
                                    section[j].isSectionRequiredOn = false;
                                }
                                //GL.End();
                            }
                        }
                    }
                }


                label1.Text = bob.ToString();
*/
