using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CABLine
    {
        public double abHeading, abLength;

        public bool isABValid;

        //the current AB guidance line
        public vec3 currentLinePtA = new vec3(0.0, 0.0, 0.0);

        public vec3 currentLinePtB = new vec3(0.0, 1.0, 0.0);

        public double distanceFromCurrentLinePivot;
        public double distanceFromRefLine;

        //pure pursuit values
        public vec2 goalPointAB = new vec2(0, 0);

        public double howManyPathsAway, lastHowManyPathsAway;
        public bool isMakingABLine;
        public bool isHeadingSameWay = true, lastIsHeadingSameWay;

        //public bool isOnTramLine;
        //public int tramBasedOn;
        public double ppRadiusAB;

        public double rEastAB, rNorthAB;

        public double snapDistance, lastSecond = 0;
        public double steerAngleAB;

        //design
        public vec2 desPtA = new vec2(0.2, 0.15);

        public vec2 desPtB = new vec2(0.3, 0.3);

        public vec2 desLineEndA = new vec2(0.0, 0.0);
        public vec2 desLineEndB = new vec2(999997, 1.0);

        public double desHeading = 0;

        public string desName = "";

        //autosteer errors
        public double pivotDistanceError, pivotDistanceErrorLast, pivotDerivative, pivotDerivativeSmoothed;

        public double inty;
        public double steerAngleSmoothed, pivotErrorTotal;
        public double distSteerError, lastDistSteerError, derivativeDistError;

        //Color tramColor = Color.YellowGreen;
        public int tramPassEvery;

        //pointers to mainform controls
        private readonly FormGPS mf;

        public CABLine(FormGPS _f)
        {
            //constructor
            mf = _f;
            //isOnTramLine = true;
        }
    }
}