﻿using OpenTK.Graphics.OpenGL;

namespace AOG
{
    public class CCamera
    {
        private double camPosX;
        private double camPosY;
        private readonly double camPosZ;

        //private double fixHeading;
        private double camYaw;

        public double panX = 0, panY = 0;
        public double camSetDistance = -75;

        public double gridZoom;

        public double previousZoom = 25;

        public bool camFollowing;
        public int camMode = 0;
        public double camSmoothFactor;

        //private double camDelta = 0;

        public CCamera()
        {
            //get the pitch of camera from settings
            camPosZ = 0.0;
            camFollowing = true;
            camSmoothFactor = (Settings.User.setDisplay_camSmooth * 0.004) + 0.2;
        }

        public void SetWorldCam(double _fixPosX, double _fixPosY, double _fixHeading)
        {
            camPosX = _fixPosX;
            camPosY = _fixPosY;
            camYaw = _fixHeading;

            //back the camera up
            GL.Translate(0, 0, camSetDistance * 0.5);

            //rotate the camera down to look at fix
            GL.Rotate(Settings.User.setDisplay_camPitch, 1.0, 0.0, 0.0);

            //pan if set
            //GL.Translate(0, camSetDistance * -0.04, 0);
            GL.Translate(panX, panY, 0);

            ////draw the guide
            //GL.Begin(PrimitiveType.Triangles);
            //GL.Color3(0.98f, 0.0f, 0.0f);
            //GL.Vertex3(0.0f, -2.0f, 0.0f);
            //GL.Color3(0.0f, 0.98f, 0.0f);
            //GL.Vertex3(-2.0f, -3.0f, 0.0f);
            //GL.Color3(0.98f, 0.98f, 0.0f);
            //GL.Vertex3(2.0f, -3.0f, 0.0f);
            //GL.End();						// Done Drawing Reticle

            //following game style or N fixed cam
            if (camFollowing)
            {
                GL.Rotate(camYaw, 0.0, 0.0, 1.0);
                GL.Translate(-camPosX, -camPosY, -camPosZ);
                //GL.Translate(-60, -60,0);
            }
            else
            {
                GL.Translate(-camPosX, -camPosY, -camPosZ);
            }
        }
    }
}