using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AOG
{
    public class CTram
    {
        private readonly FormGPS mf;

        //the triangle strip of the outer tram highlight
        public List<vec2> tramBndOuterArr = new List<vec2>();

        public List<vec2> tramBndInnerArr = new List<vec2>();

        //tram settings

        public double halfWheelTrack, alpha;
        public int passes;
        public bool isOuter;

        public bool isLeftManualOn, isRightManualOn;

        public List<List<vec2>> tramList = new List<List<vec2>>();

        // 0 off, 1 All, 2, Lines, 3 Outer
        public int displayMode;

        internal int controlByte;

        public CTram(FormGPS _f)
        {
            //constructor
            mf = _f;

            halfWheelTrack = Settings.Vehicle.setVehicle_trackWidth * 0.5;

            IsTramOuterOrInner();

            passes = Settings.Tool.tram_passes;
            displayMode = 0;

            alpha = Settings.Tool.tram_alpha;
        }

        public void IsTramOuterOrInner()
        {
            isOuter = ((int)(Settings.Tool.tram_Width / Settings.Tool.toolWidth + 0.5)) % 2 == 0;
            if (Settings.Tool.isTramOuterInverted) isOuter = !isOuter;
        }

        public void DrawTram()
        {
            if (mf.camera.camSetDistance > -500) GL.LineWidth(10);
            else GL.LineWidth(6);

            GL.Color4(0, 0, 0, alpha);

            if (mf.tram.displayMode == 1 || mf.tram.displayMode == 2)
            {
                if (tramList.Count > 0)
                {
                    for (int i = 0; i < tramList.Count; i++)
                    {
                        tramList[i].DrawPolygon(PrimitiveType.LineStrip);
                    }
                }
            }

            if (mf.tram.displayMode == 1 || mf.tram.displayMode == 3)
            {
                if (tramBndOuterArr.Count > 0)
                {
                    tramBndOuterArr.DrawPolygon(PrimitiveType.LineStrip);
                    tramBndInnerArr.DrawPolygon(PrimitiveType.LineStrip);
                }
            }

            if (mf.camera.camSetDistance > -500) GL.LineWidth(4);
            else GL.LineWidth(2);

            GL.Color4(0.930f, 0.72f, 0.73530f, alpha);

            if (mf.tram.displayMode == 1 || mf.tram.displayMode == 2)
            {
                if (tramList.Count > 0)
                {
                    for (int i = 0; i < tramList.Count; i++)
                    {
                        tramList[i].DrawPolygon(PrimitiveType.LineStrip);
                    }
                }
            }

            if (mf.tram.displayMode == 1 || mf.tram.displayMode == 3)
            {
                if (tramBndOuterArr.Count > 0)
                {
                    tramBndOuterArr.DrawPolygon(PrimitiveType.LineStrip);
                    tramBndInnerArr.DrawPolygon(PrimitiveType.LineStrip);
                }
            }
        }
    }
}