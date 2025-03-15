using System;

namespace AOG
{
    public class CSim
    {
        private readonly FormGPS mf;

        #region properties sim

        public double altitude = 300;

        public double stepDistance = 0.0, steerangleAve = 0.0;

        public bool isAccelForward, isAccelBack;

        private double easting = 0, northing = 0, heading = 0;
        private double sinH = Math.Sin(0), cosH = Math.Cos(0);

        #endregion properties sim

        public CSim(FormGPS _f)
        {
            mf = _f;
        }

        public void DoSimTick(double steerAngle)
        {
            double diff = Math.Abs(steerAngle - steerangleAve);

            if (diff > 11)
            {
                if (steerangleAve >= steerAngle)
                {
                    steerangleAve -= 6;
                }
                else steerangleAve += 6;
            }
            else if (diff > 5)
            {
                if (steerangleAve >= steerAngle)
                {
                    steerangleAve -= 2;
                }
                else steerangleAve += 2;
            }
            else if (diff > 1)
            {
                if (steerangleAve >= steerAngle)
                {
                    steerangleAve -= 0.5;
                }
                else steerangleAve += 0.5;
            }
            else
            {
                steerangleAve = steerAngle;
            }

            mf.mc.actualSteerAngleDegrees = steerangleAve;

            if (!mf.isGPSPositionInitialized)
            {
                mf.pn.SetLocalMetersPerDegree(Settings.Vehicle.setGPS_SimLatitude, Settings.Vehicle.setGPS_SimLongitude);
            }

            double SteerRadius = mf.vehicle.wheelbase / Math.Tan(glm.toRadians(steerangleAve));
            double tickdist = stepDistance * 0.5;
            if (steerangleAve != 0 && Math.Abs(SteerRadius) < 10000000)
            {
                double SteerAngle = (tickdist / (glm.twoPI * SteerRadius)) * glm.twoPI;

                easting += cosH * SteerRadius;
                northing -= sinH * SteerRadius;
                heading += SteerAngle;
                heading %= glm.twoPI;
                sinH = Math.Sin(heading);
                cosH = Math.Cos(heading);
                easting -= cosH * SteerRadius;
                northing += sinH * SteerRadius;
            }
            else
            {
                easting += sinH * tickdist;
                northing += cosH * tickdist;
            }


            mf.pn.fix.northing = northing + cosH * mf.vehicle.antennaPivot + sinH * mf.vehicle.antennaOffset;
            mf.pn.fix.easting = easting + sinH * mf.vehicle.antennaPivot - cosH * mf.vehicle.antennaOffset;

            mf.pn.ConvertLocalToWGS84(mf.pn.fix.northing, mf.pn.fix.easting, out mf.pn.latitude, out mf.pn.longitude);

            if (heading < 0) heading += glm.twoPI;
            mf.ahrs.imuHeading = mf.pn.headingTrue = mf.pn.headingTrueDual = glm.toDegrees(heading);

            mf.pn.vtgSpeed = Math.Abs(Math.Round(stepDistance * 36, 2));

            mf.pn.hdop = 0.7;

            double temp = Math.Abs(mf.pn.latitude * 100);
            temp -= ((int)(temp));
            temp *= 100;
            mf.pn.altitude = temp + 200;

            temp = Math.Abs(mf.pn.longitude * 100);
            temp -= ((int)(temp));
            temp *= 100;
            mf.pn.altitude += temp;

            mf.pn.satellitesTracked = 12;

            mf.sentenceCounter = 0;

            if (mf.isGPSToolActive)
            {
                mf.pnTool.fix.easting = mf.toolPivotPos.easting;
                mf.pnTool.fix.northing = mf.toolPivotPos.northing;

                mf.pnTool.ConvertLocalToWGS84(mf.pnTool.fix.northing, mf.pnTool.fix.easting, out mf.pnTool.latitude, out mf.pnTool.longitude);
            }

            mf.UpdateFixPosition();

            if (isAccelForward)
            {
                if (stepDistance < -0.06) stepDistance = -0.06;

                isAccelBack = false;
                stepDistance += 0.02;
                if (stepDistance > 0.12) isAccelForward = false;
            }

            if (isAccelBack)
            {
                if (stepDistance > 0.12) stepDistance = 0.12;
                isAccelForward = false;
                stepDistance -= 0.01;
                if (stepDistance < -0.06) isAccelBack = false;
            }
        }

        public void Reset()
        {
            easting = northing = heading = 0;

            mf.stepFixPts[0].isSet = false;
            mf.gpsHeading = 0;

            sinH = Math.Sin(0);
            cosH = Math.Cos(0);
            if (mf.timerSim.Enabled)
                mf.pn.ConvertLocalToWGS84(0, 0, out mf.pn.latitude, out mf.pn.longitude);
        }

        public void Reverse()
        {
            heading -= Math.PI;

            mf.stepFixPts[0].isSet = false;

            mf.gpsHeading += Math.PI;
            mf.gpsHeading %= glm.twoPI;

            sinH = Math.Sin(heading);
            cosH = Math.Cos(heading);
            mf.trk.isHeadingSameWay = !mf.trk.isHeadingSameWay;

            //if (isBtnAutoSteerOn)
            {
                //mf.SetAutoSteerButton(false, "Sim Reverse Touched");
                //Log.EventWriter("Steer Off, Sim Reverse Activated");
            }
        }
    }
}