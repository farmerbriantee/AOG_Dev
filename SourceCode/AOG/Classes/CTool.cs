using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace AOG
{
    public class CTool
    {
        private readonly FormGPS mf;

        public double farLeftPosition = 0;
        public double farLeftSpeed = 0;
        public double farRightPosition = 0;
        public double farRightSpeed = 0;

        public double lookAheadDistanceOnPixelsLeft, lookAheadDistanceOnPixelsRight;
        public double lookAheadDistanceOffPixelsLeft, lookAheadDistanceOffPixelsRight;

        //read pixel values
        public int rpXPosition;

        public int rpWidth;

        private double textRotate;

        public Color[] secColors = new Color[16];

        public int zones;
        public int[] zoneRanges = new int[9];

        public bool remoteSectionControl = true;
        //Constructor called by FormGPS
        public CTool(FormGPS _f)
        {
            mf = _f;
        }

        public void LoadSettings()
        {
            remoteSectionControl = false;

            secColors[0] = Settings.Tool.setColor_sec01;
            secColors[1] = Settings.Tool.setColor_sec02;
            secColors[2] = Settings.Tool.setColor_sec03;
            secColors[3] = Settings.Tool.setColor_sec04;
            secColors[4] = Settings.Tool.setColor_sec05;
            secColors[5] = Settings.Tool.setColor_sec06;
            secColors[6] = Settings.Tool.setColor_sec07;
            secColors[7] = Settings.Tool.setColor_sec08;
            secColors[8] = Settings.Tool.setColor_sec09;
            secColors[9] = Settings.Tool.setColor_sec10;
            secColors[10] = Settings.Tool.setColor_sec11;
            secColors[11] = Settings.Tool.setColor_sec12;
            secColors[12] = Settings.Tool.setColor_sec13;
            secColors[13] = Settings.Tool.setColor_sec14;
            secColors[14] = Settings.Tool.setColor_sec15;
            secColors[15] = Settings.Tool.setColor_sec16;

            string[] words = Settings.Tool.zones.Split(',');
            zones = int.Parse(words[0]);

            for (int i = 0; i < words.Length; i++)
            {
                zoneRanges[i] = int.Parse(words[i]);
            }

            mf.SetNumOfControlButtons(Settings.Tool.isSectionsNotZones ? Settings.Tool.numSections : zones, Settings.Tool.isSectionsNotZones ? Settings.Tool.numSections : Settings.Tool.numSectionsMulti);

            //Set width of section and positions for each section
            mf.SectionSetPosition();
        }

        public void DrawTool()
        {
            if (!mf.isGPSToolActive)
            {
                GL.Translate(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
                //translate and rotate at pivot axle
                GL.PushMatrix();

                //translate down to the hitch pin
                GL.Translate(Math.Sin(mf.fixHeading) * (Settings.Tool.hitchLength),
                                Math.Cos(mf.fixHeading) * (Settings.Tool.hitchLength), 0);

                //settings doesn't change trailing hitch length if set to rigid, so do it here
                double trailingTank, trailingTool;
                if (Settings.Tool.isToolTrailing)
                {
                    trailingTank = Settings.Tool.tankTrailingHitchLength;
                    trailingTool = Settings.Tool.toolTrailingHitchLength;
                }
                else { trailingTank = 0; trailingTool = 0; }

                //there is a trailing tow between hitch
                if (Settings.Tool.isToolTBT && Settings.Tool.isToolTrailing)
                {
                    //rotate to tank heading
                    GL.Rotate(glm.toDegrees(-mf.tankPos.heading), 0.0, 0.0, 1.0);

                    //draw the tank hitch
                    GL.LineWidth(6);
                    //draw the rigid hitch
                    GL.Color3(0, 0, 0);
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(-0.57, trailingTank, 0);
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(0.57, trailingTank, 0);

                    GL.End();

                    GL.LineWidth(1);
                    //draw the rigid hitch
                    GL.Color3(0.765f, 0.76f, 0.32f);
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(-0.57, trailingTank, 0);
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(0.57, trailingTank, 0);

                    GL.End();

                    GL.Enable(EnableCap.Texture2D);
                    GL.Color4(1, 1, 1, 0.75);
                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.ToolWheels]);        // Select Our Texture
                    GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                    GL.TexCoord2(1, 0); GL.Vertex2(1.5, trailingTank + 1); // Top Right
                    GL.TexCoord2(0, 0); GL.Vertex2(-1.5, trailingTank + 1); // Top Left
                    GL.TexCoord2(1, 1); GL.Vertex2(1.5, trailingTank - 1); // Bottom Right
                    GL.TexCoord2(0, 1); GL.Vertex2(-1.5, trailingTank - 1); // Bottom Left
                    GL.End();                       // Done Building Triangle Strip
                    GL.Disable(EnableCap.Texture2D);

                    //move down the tank hitch, unwind, rotate to section heading
                    GL.Translate(0.0, trailingTank, 0.0);
                    GL.Rotate(glm.toDegrees(mf.tankPos.heading), 0.0, 0.0, 1.0);
                    GL.Rotate(glm.toDegrees(-mf.toolPivotPos.heading), 0.0, 0.0, 1.0);
                }

                //no tow between hitch
                else
                {
                    GL.Rotate(glm.toDegrees(-mf.toolPivotPos.heading), 0.0, 0.0, 1.0);
                }

                //draw the hitch if trailing
                if (Settings.Tool.isToolTrailing)
                {
                    GL.LineWidth(6);
                    GL.Color3(0, 0, 0);
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(-0.65 + Settings.Tool.offset, trailingTool, 0);
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(0.65 + Settings.Tool.offset, trailingTool, 0);

                    GL.End();

                    GL.LineWidth(1);
                    //draw the rigid hitch
                    GL.Color3(0.7f, 0.4f, 0.2f);
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(-0.65 + Settings.Tool.offset, trailingTool, 0);
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(0.65 + Settings.Tool.offset, trailingTool, 0);

                    GL.End();

                    if (Math.Abs(Settings.Tool.trailingToolToPivotLength) > 1 && mf.camera.camSetDistance > -100)
                    {
                        textRotate += (mf.sim.stepDistance);
                        GL.Enable(EnableCap.Texture2D);
                        GL.Color4(1, 1, 1, 0.75);
                        GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Tire]);        // Select Our Texture
                        GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                        GL.TexCoord2(1, 0 + textRotate); GL.Vertex2(1.4 + Settings.Tool.offset, trailingTool + 0.51); // Top Right
                        GL.TexCoord2(0, 0 + textRotate); GL.Vertex2(0.75 + Settings.Tool.offset, trailingTool + 0.51); // Top Left
                        GL.TexCoord2(1, 1 + textRotate); GL.Vertex2(1.4 + Settings.Tool.offset, trailingTool - 0.51); // Bottom Right
                        GL.TexCoord2(0, 1 + textRotate); GL.Vertex2(0.75 + Settings.Tool.offset, trailingTool - 0.51); // Bottom Left
                        GL.End();                       // Done Building Triangle Strip
                        GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                        GL.TexCoord2(1, 0 + textRotate); GL.Vertex2(-1.4 + Settings.Tool.offset, trailingTool + 0.51); // Top Right
                        GL.TexCoord2(0, 0 + textRotate); GL.Vertex2(-0.75 + Settings.Tool.offset, trailingTool + 0.51); // Top Left
                        GL.TexCoord2(1, 1 + textRotate); GL.Vertex2(-1.4 + Settings.Tool.offset, trailingTool - 0.51); // Bottom Right
                        GL.TexCoord2(0, 1 + textRotate); GL.Vertex2(-0.75 + Settings.Tool.offset, trailingTool - 0.51); // Bottom Left
                        GL.End();                       // Done Building Triangle Strip
                        GL.Disable(EnableCap.Texture2D);
                    }

                    trailingTool -= Settings.Tool.trailingToolToPivotLength;
                }

                if (!remoteSectionControl)
                {
                    //look ahead lines
                    GL.LineWidth(3);
                    GL.Begin(PrimitiveType.Lines);

                    //lookahead section on
                    GL.Color3(0.20f, 0.7f, 0.2f);
                    GL.Vertex3(mf.tool.farLeftPosition, (mf.tool.lookAheadDistanceOnPixelsLeft) * 0.1 + trailingTool, 0);
                    GL.Vertex3(mf.tool.farRightPosition, (mf.tool.lookAheadDistanceOnPixelsRight) * 0.1 + trailingTool, 0);

                    //lookahead section off
                    GL.Color3(0.70f, 0.2f, 0.2f);
                    GL.Vertex3(mf.tool.farLeftPosition, (mf.tool.lookAheadDistanceOffPixelsLeft) * 0.1 + trailingTool, 0);
                    GL.Vertex3(mf.tool.farRightPosition, (mf.tool.lookAheadDistanceOffPixelsRight) * 0.1 + trailingTool, 0);

                    if (mf.vehicle.isHydLiftOn && mf.section.Count > 0)
                    {
                        GL.Color3(0.70f, 0.2f, 0.72f);
                        GL.Vertex3(mf.section[0].positionLeft, (mf.vehicle.hydLiftLookAheadDistanceLeft * 0.1) + trailingTool, 0);
                        GL.Vertex3(mf.section[mf.section.Count - 1].positionRight, (mf.vehicle.hydLiftLookAheadDistanceRight * 0.1) + trailingTool, 0);
                    }

                    GL.End();
                }

                //draw the sections
                GL.LineWidth(2);

                double hite = mf.camera.camSetDistance / -150;
                if (hite > 1.25) hite = 1.25;
                if (hite < 0.5) hite = 0.5;

                //TooDoo
                hite = 0.2;

                for (int j = 0; j < mf.section.Count; j++)
                {
                    //if section is on, green, if off, red color
                    if (mf.section[j].isSectionOn)
                    {
                        if (mf.section[j].sectionBtnState == btnStates.Auto)
                        {
                            //GL.Color3(0.0f, 0.9f, 0.0f);
                            if (mf.section[j].isMappingOn) GL.Color3(0.0f, 0.95f, 0.0f);
                            else GL.Color3(0.970f, 0.30f, 0.970f);
                        }
                        else GL.Color3(0.97, 0.97, 0);
                    }
                    else
                    {
                        if (!mf.section[j].isMappingOn) GL.Color3(0.950f, 0.2f, 0.2f);
                        else GL.Color3(0.00f, 0.250f, 0.97f);
                        //GL.Color3(0.7f, 0.2f, 0.2f);
                    }

                    double mid = (mf.section[j].positionRight - mf.section[j].positionLeft) / 2 + mf.section[j].positionLeft;

                    GL.Begin(PrimitiveType.TriangleFan);
                    {
                        GL.Vertex3(mf.section[j].positionLeft, trailingTool, 0);
                        GL.Vertex3(mf.section[j].positionLeft, trailingTool - hite, 0);

                        GL.Vertex3(mid, trailingTool - hite * 1.5, 0);

                        GL.Vertex3(mf.section[j].positionRight, trailingTool - hite, 0);
                        GL.Vertex3(mf.section[j].positionRight, trailingTool, 0);
                    }
                    GL.End();

                    if (mf.camera.camSetDistance > -Settings.Tool.toolWidth * 200)
                    {
                        GL.Begin(PrimitiveType.LineLoop);
                        {
                            GL.Color3(0.0, 0.0, 0.0);
                            GL.Vertex3(mf.section[j].positionLeft, trailingTool, 0);
                            GL.Vertex3(mf.section[j].positionLeft, trailingTool - hite, 0);

                            GL.Vertex3(mid, trailingTool - hite * 1.5, 0);

                            GL.Vertex3(mf.section[j].positionRight, trailingTool - hite, 0);
                            GL.Vertex3(mf.section[j].positionRight, trailingTool, 0);
                        }
                        GL.End();
                    }
                }

                //zones

                if (!Settings.Tool.isSectionsNotZones && zones > 0 && mf.camera.camSetDistance > -150)
                {
                    //GL.PointSize(8);

                    GL.Begin(PrimitiveType.Lines);
                    for (int i = 1; i < zones; i++)
                    {
                        GL.Color3(0.5f, 0.80f, 0.950f);
                        if (zoneRanges[i] < mf.section.Count)
                        {
                            GL.Vertex3(mf.section[zoneRanges[i]].positionLeft, trailingTool - 0.4, 0);
                            GL.Vertex3(mf.section[zoneRanges[i]].positionLeft, trailingTool + 0.2, 0);
                        }
                    }

                    GL.End();
                }

                //tram Dots
                if (Settings.Tool.isDisplayTramControl && mf.tram.displayMode != 0)
                {
                    if (mf.camera.camSetDistance > -300)
                    {
                        if (mf.camera.camSetDistance > -100)
                            GL.PointSize(12);
                        else GL.PointSize(8);

                        if (mf.tram.isOuter)
                        {
                            //section markers
                            GL.Begin(PrimitiveType.Points);

                            //right side
                            if (((mf.tram.controlByte) & 1) == 1) GL.Color3(0.0f, 0.900f, 0.39630f);
                            else GL.Color3(0, 0, 0);
                            GL.Vertex3(farRightPosition - mf.tram.halfWheelTrack, trailingTool, 0);

                            //left side
                            if ((mf.tram.controlByte & 2) == 2) GL.Color3(0.0f, 0.900f, 0.3930f);
                            else GL.Color3(0, 0, 0);
                            GL.Vertex3(farLeftPosition + mf.tram.halfWheelTrack, trailingTool, 0);
                            GL.End();
                        }
                        else
                        {
                            GL.Begin(PrimitiveType.Points);

                            //right side
                            if (((mf.tram.controlByte) & 1) == 1) GL.Color3(0.0f, 0.900f, 0.39630f);
                            else GL.Color3(0, 0, 0);
                            GL.Vertex3(mf.tram.halfWheelTrack, trailingTool, 0);

                            //left side
                            if ((mf.tram.controlByte & 2) == 2) GL.Color3(0.0f, 0.900f, 0.3930f);
                            else GL.Color3(0, 0, 0);
                            GL.Vertex3(-mf.tram.halfWheelTrack, trailingTool, 0);
                            GL.End();
                        }
                    }
                }

                GL.PopMatrix();
            }
            else //gpsTool Position
            {
                //translate and rotate at pivot axle
                GL.PushMatrix();

                //move to Tool and rotate
                GL.Translate(mf.toolPos.easting, mf.toolPos.northing, 0);
                GL.Rotate(glm.toDegrees(-mf.toolPos.heading), 0.0, 0.0, 1.0);

                //draw the sections.

                GL.LineWidth(3);
                GL.Begin(PrimitiveType.Lines);

                //lookahead section on
                GL.Color3(0.20f, 0.7f, 0.2f);
                GL.Vertex3(mf.tool.farLeftPosition, (mf.tool.lookAheadDistanceOnPixelsLeft) * 0.1, 0);
                GL.Vertex3(mf.tool.farRightPosition, (mf.tool.lookAheadDistanceOnPixelsRight) * 0.1, 0);

                //lookahead section off
                GL.Color3(0.70f, 0.2f, 0.2f);
                GL.Vertex3(mf.tool.farLeftPosition, (mf.tool.lookAheadDistanceOffPixelsLeft) * 0.1, 0);
                GL.Vertex3(mf.tool.farRightPosition, (mf.tool.lookAheadDistanceOffPixelsRight) * 0.1, 0);

                if (mf.vehicle.isHydLiftOn && mf.section.Count > 0)
                {
                    GL.Color3(0.70f, 0.2f, 0.72f);
                    GL.Vertex3(mf.section[0].positionLeft, (mf.vehicle.hydLiftLookAheadDistanceLeft * 0.1), 0);
                    GL.Vertex3(mf.section[mf.section.Count - 1].positionRight, (mf.vehicle.hydLiftLookAheadDistanceRight * 0.1), 0);
                }

                GL.End();

                //draw the sections
                GL.LineWidth(2);

                double hite = mf.camera.camSetDistance / -150;
                if (hite > 1.25) hite = 1.25;
                if (hite < 0.5) hite = 0.5;

                //TooDoo
                hite = 0.2;

                for (int j = 0; j < mf.section.Count; j++)
                {
                    //if section is on, green, if off, red color
                    if (mf.section[j].isSectionOn)
                    {
                        if (mf.section[j].sectionBtnState == btnStates.Auto)
                        {
                            //GL.Color3(0.0f, 0.9f, 0.0f);
                            if (mf.section[j].isMappingOn) GL.Color3(0.0f, 0.95f, 0.0f);
                            else GL.Color3(0.970f, 0.30f, 0.970f);
                        }
                        else GL.Color3(0.97, 0.97, 0);
                    }
                    else
                    {
                        if (!mf.section[j].isMappingOn) GL.Color3(0.950f, 0.2f, 0.2f);
                        else GL.Color3(0.00f, 0.250f, 0.97f);
                        //GL.Color3(0.7f, 0.2f, 0.2f);
                    }

                    double mid = (mf.section[j].positionRight - mf.section[j].positionLeft) / 2 + mf.section[j].positionLeft;

                    GL.Begin(PrimitiveType.TriangleFan);
                    {
                        GL.Vertex3(mf.section[j].positionLeft, 0, 0);
                        GL.Vertex3(mf.section[j].positionLeft, 0 - hite, 0);

                        GL.Vertex3(mid, 0 - hite * 1.5, 0);

                        GL.Vertex3(mf.section[j].positionRight, 0 - hite, 0);
                        GL.Vertex3(mf.section[j].positionRight, 0, 0);
                    }
                    GL.End();

                    if (mf.camera.camSetDistance > -Settings.Tool.toolWidth * 200)
                    {
                        GL.Begin(PrimitiveType.LineLoop);
                        {
                            GL.Color3(0.0, 0.0, 0.0);
                            GL.Vertex3(mf.section[j].positionLeft, 0, 0);
                            GL.Vertex3(mf.section[j].positionLeft, 0 - hite, 0);

                            GL.Vertex3(mid, 0 - hite * 1.5, 0);

                            GL.Vertex3(mf.section[j].positionRight, 0 - hite, 0);
                            GL.Vertex3(mf.section[j].positionRight, 0, 0);
                        }
                        GL.End();
                    }
                }

                //zones

                if (!Settings.Tool.isSectionsNotZones && zones > 0 && mf.camera.camSetDistance > -150)
                {
                    //GL.PointSize(8);

                    GL.Begin(PrimitiveType.Lines);
                    for (int i = 1; i < zones; i++)
                    {
                        GL.Color3(0.5f, 0.80f, 0.950f);
                        GL.Vertex3(mf.section[zoneRanges[i]].positionLeft, 0 - 0.4, 0);
                        GL.Vertex3(mf.section[zoneRanges[i]].positionLeft, 0 + 0.2, 0);
                    }

                    GL.End();
                }

                //tram Dots
                if (Settings.Tool.isDisplayTramControl && mf.tram.displayMode != 0)
                {
                    if (mf.camera.camSetDistance > -300)
                    {
                        if (mf.camera.camSetDistance > -100)
                            GL.PointSize(12);
                        else GL.PointSize(8);

                        if (mf.tram.isOuter)
                        {
                            //section markers
                            GL.Begin(PrimitiveType.Points);

                            //right side
                            if (((mf.tram.controlByte) & 1) == 1) GL.Color3(0.0f, 0.900f, 0.39630f);
                            else GL.Color3(0, 0, 0);
                            GL.Vertex3(farRightPosition - mf.tram.halfWheelTrack, 0, 0);

                            //left side
                            if ((mf.tram.controlByte & 2) == 2) GL.Color3(0.0f, 0.900f, 0.3930f);
                            else GL.Color3(0, 0, 0);
                            GL.Vertex3(farLeftPosition + mf.tram.halfWheelTrack, 0, 0);
                            GL.End();
                        }
                        else
                        {
                            GL.Begin(PrimitiveType.Points);

                            //right side
                            if (((mf.tram.controlByte) & 1) == 1) GL.Color3(0.0f, 0.900f, 0.39630f);
                            else GL.Color3(0, 0, 0);
                            GL.Vertex3(mf.tram.halfWheelTrack, 0, 0);

                            //left side
                            if ((mf.tram.controlByte & 2) == 2) GL.Color3(0.0f, 0.900f, 0.3930f);
                            else GL.Color3(0, 0, 0);
                            GL.Vertex3(-mf.tram.halfWheelTrack, 0, 0);
                            GL.End();
                        }
                    }
                }


                GL.PopMatrix();

                GL.Translate(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
            }
        }
    }
}