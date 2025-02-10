using AgOpenGPS.Culture;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AgOpenGPS
{
    public class CGuidance
    {
        private readonly FormGPS mf;

        public int A, B;

        //private int rA, rB;

        public double distanceFromCurrentLineSteer, distanceFromCurrentLinePivot, distanceFromCurrentLineTool;
        public double steerAngleGu, rEastSteer, rNorthSteer, rEastPivot, rNorthPivot;
        public double steerAngleTrk;

        public vec2 goalPointTrk = new vec2();

        public double rEastTrk, rNorthTrk, manualUturnHeading;

        public double inty, xTrackSteerCorrection = 0;
        public double steerHeadingError, steerHeadingErrorDegrees;

        public double distSteerError, lastDistSteerError, derivativeDistError;

        public double pivotDistanceError, stanleyModeMultiplier;

        //public int modeTimeCounter = 0;

        //for adding steering angle based on side slope hill
        public double sideHillCompFactor;

        //derivative counters
        private int counter2;

        // Should we find the global nearest curve point (instead of local) on the next search.
        public bool isFindGlobalNearestTrackPoint = true;

        public int currentLocationIndex;
        public double pivotDistanceErrorLast, pivotDerivative, pivotDerivativeSmoothed, lastTrackDistance = 10000;

        public CGuidance(FormGPS _f)
        {
            //constructor
            mf = _f;
            sideHillCompFactor = Properties.Settings.Default.setAS_sideHillComp;
        }

        public void UTurnGuidance()
        {
            // Update based on autosteer settings and distance from line
            mf.vehicle.UpdateGoalPointDistance();

            //now substitute what it thinks are AB line values with auto turn values
            steerAngleTrk = mf.yt.steerAngleYT;
            distanceFromCurrentLinePivot = mf.yt.distanceFromCurrentLine;

            goalPointTrk = mf.yt.goalPointYT;
            mf.vehicle.modeActualXTE = (distanceFromCurrentLinePivot);
        }

        #region Stanley

        public void StanleyGuidance(vec3 steer, ref List<vec3> curList)
        {           
            //find the closest point roughly
            int cc = 0, dd;
            double dist, dx, dz;
            double minDistA = double.MaxValue;

            if (curList.Count > 3)
            {
                if (isFindGlobalNearestTrackPoint)
                {
                    for (int j = 0; j < curList.Count; j += 5)
                    {
                        dist = glm.DistanceSquared(steer, curList[j]);
                        if (dist < minDistA)
                        {
                            minDistA = dist;
                            cc = j;
                        }
                    }
                    isFindGlobalNearestTrackPoint = false;
                }
                else
                {
                    cc = currentLocationIndex;
                }

                minDistA = double.MaxValue;

                //long enough line?
                if (mf.trk.gArr[mf.trk.idx].mode <= TrackMode.Curve)
                {
                    if (cc > curList.Count - 30)
                    {
                        mf.trk.AddEndPoints(ref curList, 100);
                        currentLocationIndex = cc;

                        for (int j = 0; j < curList.Count; j += 5)
                        {
                            dist = glm.DistanceSquared(steer, curList[j]);
                            if (dist < minDistA)
                            {
                                minDistA = dist;
                                cc = j;
                            }
                        }
                        isFindGlobalNearestTrackPoint = false;
                        minDistA = double.MaxValue;
                    }

                    if (cc < 30)
                    {
                        mf.trk.AddStartPoints(ref curList, 100);
                        currentLocationIndex = cc;

                        for (int j = 0; j < curList.Count; j += 5)
                        {
                            dist = glm.DistanceSquared(steer, curList[j]);
                            if (dist < minDistA)
                            {
                                minDistA = dist;
                                cc = j;
                            }
                        }
                        isFindGlobalNearestTrackPoint = false;
                        minDistA = double.MaxValue;
                    }
                }

                //find the local point
                dd = cc + 6; if (dd > curList.Count - 1) dd = curList.Count;
                cc -= 6; if (cc < 0) cc = 0;

                //find the closest point to current close call
                for (int j = cc; j < dd; j++)
                {
                    dist = glm.DistanceSquared(steer, curList[j]);
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        A = j;
                    }
                }

                if (A > curList.Count - 1 || A < 0)
                {
                    isFindGlobalNearestTrackPoint = false;
                    return;
                }

                //initial forward Test if pivot InRange AB
                if (A == curList.Count - 1) 
                    B = 0;
                else B = A + 1;

                if (glm.InRangeBetweenAB(curList[A].easting, curList[A].northing,
                     curList[B].easting, curList[B].northing, steer.easting, steer.northing))
                    goto SegmentFound;

                //step back one
                if (A == 0)
                {
                    A = curList.Count - 1;
                    B = 0;
                }
                else
                {
                    A--;
                    B = A + 1;
                }

                if (glm.InRangeBetweenAB(curList[A].easting, curList[A].northing,
                    curList[B].easting, curList[B].northing, steer.easting, steer.northing))
                {
                    goto SegmentFound;
                }
                else
                {
                    isFindGlobalNearestTrackPoint = true;
                    return;
                }

            SegmentFound:

                currentLocationIndex = A;

                manualUturnHeading = curList[A].heading;

                //get the distance from currently active AB segment of steer axle //////// steer /////////////
                vec3 steerA = new vec3(curList[A]);
                vec3 steerB = new vec3(curList[B]);

                if (!mf.trk.isHeadingSameWay)
                {
                    steerA = curList[B];
                    steerA.heading += Math.PI;
                    if (steerA.heading > glm.twoPI) steerA.heading -= glm.twoPI;

                    steerB = curList[A];
                    steerB.heading += Math.PI;
                    if (steerB.heading > glm.twoPI) steerB.heading -= glm.twoPI;
                }

                dx = steerB.easting - steerA.easting;
                dz = steerB.northing - steerA.northing;

                if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLineSteer = ((dz * steer.easting) - (dx * steer.northing) + (steerB.easting
                            * steerA.northing) - (steerB.northing * steerA.easting))
                                / Math.Sqrt((dz * dz) + (dx * dx));

                // calc point on AB Line closest to current position - for display only
                double U = (((steer.easting - steerA.easting) * dx)
                                + ((steer.northing - steerA.northing) * dz))
                                / ((dx * dx) + (dz * dz));

                rEastSteer = steerA.easting + (U * dx);
                rNorthSteer = steerA.northing + (U * dz);

                double delta = 0;
                double abDist = glm.DistanceSquared(steerA, steerB);
                double rDist = glm.DistanceSquared(rNorthSteer, rEastSteer, steerA.northing, steerA.easting);
                rDist /= abDist;
                if (Math.Abs(steerA.heading - steerB.heading) > Math.PI)
                {
                    if (steerA.heading < Math.PI) delta = (1 - rDist) * (steerA.heading + glm.twoPI) + (rDist) * steerB.heading;
                    else delta = (1 - rDist) * steerA.heading + (rDist) * (steerB.heading + glm.twoPI);
                }
                else
                {
                    delta = (1 - rDist) * steerA.heading + (rDist) * steerB.heading;
                }
                steerHeadingError = steer.heading - delta;

                steerHeadingError = steer.heading - delta;

                mf.lblAlgo.Text = steerHeadingError.ToString();

                //Fix the circular error
                if (steerHeadingError > Math.PI) steerHeadingError -= Math.PI;
                else if (steerHeadingError < Math.PI) steerHeadingError += Math.PI;

                if (steerHeadingError > glm.PIBy2) steerHeadingError -= Math.PI;
                else if (steerHeadingError < -glm.PIBy2) steerHeadingError += Math.PI;

                DoSteerAngleCalc();

                //Tool GPS
                if (mf.isGPSToolActive)
                {
                    minDistA = double.MaxValue;
                    //close call hit
                    cc = 0;

                    for (int j = 0; j < curList.Count; j += 5)
                    {
                        dist = glm.DistanceSquared(mf.pnTool.fix, curList[j]);
                        if (dist < minDistA)
                        {
                            minDistA = dist;
                            cc = j;
                        }
                    }

                    minDistA = double.MaxValue;

                    dd = cc + 5; if (dd > curList.Count - 1) dd = curList.Count;
                    cc -= 5; if (cc < 0) cc = 0;

                    //find the closest 2 points to current close call
                    for (int j = cc; j < dd; j++)
                    {
                        dist = glm.DistanceSquared(mf.pnTool.fix, curList[j]);
                        if (dist < minDistA)
                        {
                            minDistA = dist;
                            A = j;
                        }
                    }

                    if (A > curList.Count - 1)
                        return;

                    //initial forward Test if gps2 InRange AB
                    if (A == curList.Count - 1) B = 0;
                    else B = A + 1;

                    if (glm.InRangeBetweenAB(curList[A].easting, curList[A].northing,
                         curList[B].easting, curList[B].northing, mf.pnTool.fix.easting, mf.pnTool.fix.northing))
                        goto Segment2Found;

                    //step back one
                    if (A == 0)
                    {
                        A = curList.Count - 1;
                        B = 0;
                    }
                    else
                    {
                        A--;
                        B = A + 1;
                    }

                    if (glm.InRangeBetweenAB(curList[A].easting, curList[A].northing,
                        curList[B].easting, curList[B].northing, mf.pnTool.fix.easting, mf.pnTool.fix.northing))
                    {
                        goto Segment2Found;
                    }

                Segment2Found:

                    //get the distance from currently active AB line
                    dx = curList[B].easting - curList[A].easting;
                    dz = curList[B].northing - curList[A].northing;

                    if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

                    //how far from current AB Line is fix
                    distanceFromCurrentLineTool = ((dz * mf.pnTool.fix.easting) - (dx * mf.pnTool.fix.northing) + (curList[B].easting
                                * curList[A].northing) - (curList[B].northing * curList[A].easting))
                                    / Math.Sqrt((dz * dz) + (dx * dx));

                    if (!mf.trk.isHeadingSameWay)
                        distanceFromCurrentLineTool *= -1.0;

                    mf.guidanceLineDistanceOffTool = (short)Math.Round(distanceFromCurrentLineTool * 1000.0, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                //invalid distance so tell AS module
                distanceFromCurrentLineSteer = 32000;
                mf.guidanceLineDistanceOff = 32000;
                mf.guidanceLineDistanceOffTool = 32000;
            }
        }

        private void DoSteerAngleCalc()
        {
            if (mf.isReverse) steerHeadingError *= -1;
            //Overshoot setting on Stanley tab
            steerHeadingError *= mf.vehicle.stanleyHeadingErrorGain;

            double sped = Math.Abs(mf.avgSpeed);
            if (sped > 1) sped = 1 + 0.277 * (sped - 1);
            else sped = 1;
            double XTEc = Math.Atan((distanceFromCurrentLineSteer * mf.vehicle.stanleyDistanceErrorGain)
                / (sped));

            xTrackSteerCorrection = (xTrackSteerCorrection * 0.5) + XTEc * (0.5);

            ////derivative of steer distance error
            //distSteerError = (distSteerError * 0.95) + ((xTrackSteerCorrection * 60) * 0.05);
            //if (counter++ > 5)
            //{
            //    derivativeDistError = distSteerError - lastDistSteerError;
            //    lastDistSteerError = distSteerError;
            //    counter = 0;
            //}

            steerAngleGu = glm.toDegrees((xTrackSteerCorrection + steerHeadingError) * -1.0);

            if (Math.Abs(distanceFromCurrentLineSteer) > 0.5) steerAngleGu *= 0.5;
            else steerAngleGu *= (1 - Math.Abs(distanceFromCurrentLineSteer));

            //if (mf.isReverse) inty = 0;

            if (mf.ahrs.imuRoll != 88888)
                steerAngleGu += mf.ahrs.imuRoll * -sideHillCompFactor;

            if (steerAngleGu < -mf.vehicle.maxSteerAngle) steerAngleGu = -mf.vehicle.maxSteerAngle;
            else if (steerAngleGu > mf.vehicle.maxSteerAngle) steerAngleGu = mf.vehicle.maxSteerAngle;

            //used for smooth mode
            mf.vehicle.modeActualXTE = (distanceFromCurrentLineSteer);

            //Convert to millimeters from meters
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLineSteer * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngleGu * 100);
        }

        #endregion Stanley

        #region PurePursuit

        public void PurePursuitGuidance(vec3 pivot, ref List<vec3> curList)
        {
            double minDistA;
            double dist, dx, dz;

            double goalPointDistance = mf.vehicle.UpdateGoalPointDistance();
            bool ReverseHeading = mf.isReverse ? !mf.trk.isHeadingSameWay : mf.trk.isHeadingSameWay;

            //close call hit
            minDistA = double.MaxValue;
            int cc = 0, dd;

            if (isFindGlobalNearestTrackPoint)
            {
                for (int j = 0; j < curList.Count; j += 5)
                {
                    dist = glm.DistanceSquared(pivot, curList[j]);
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        cc = j;
                    }
                }
                isFindGlobalNearestTrackPoint = false;
            }
            else
            {
                cc = currentLocationIndex;
            }

            minDistA = double.MaxValue;

            if (mf.trk.gArr[mf.trk.idx].mode <= TrackMode.Curve)
            {
                if (cc > curList.Count - 30)
                {
                    mf.trk.AddEndPoints(ref curList, 100);
                    currentLocationIndex = cc;

                    for (int j = 0; j < curList.Count; j += 5)
                    {
                        dist = glm.DistanceSquared(pivot, curList[j]);
                        if (dist < minDistA)
                        {
                            minDistA = dist;
                            cc = j;
                        }
                    }
                    isFindGlobalNearestTrackPoint = false;
                    minDistA = double.MaxValue;
                }
                
                if (cc < 30)
                {
                    mf.trk.AddStartPoints(ref curList, 100);
                    currentLocationIndex = cc;

                    for (int j = 0; j < curList.Count; j += 5)
                    {
                        dist = glm.DistanceSquared(pivot, curList[j]);
                        if (dist < minDistA)
                        {
                            minDistA = dist;
                            cc = j;
                        }
                    }
                    isFindGlobalNearestTrackPoint = false;
                    minDistA = double.MaxValue;
                }
            }

            //find the local point
            dd = cc + 6; if (dd > curList.Count - 1) dd = curList.Count;
            cc -= 6; if (cc < 0) cc = 0;

            //find the closest 2 points to current close call
            for (int j = cc; j < dd; j++)
            {
                dist = glm.DistanceSquared(pivot, curList[j]);
                if (dist < minDistA)
                {
                    minDistA = dist;
                    A = j;
                }
            }

            if (A > curList.Count - 1 || A < 0)
            {
                isFindGlobalNearestTrackPoint = false;
                return;
            }

            //initial forward Test if pivot InRange AB
            if (A == curList.Count - 1) B = 0;
            else B = A + 1;

            if (glm.InRangeBetweenAB(curList[A].easting, curList[A].northing,
                 curList[B].easting, curList[B].northing, pivot.easting, pivot.northing))
                goto SegmentFound;

            //step back one
            if (A == 0)
            {
                A = curList.Count - 1;
                B = 0;
            }
            else
            {
                A--;
                B = A + 1;
            }

            if (glm.InRangeBetweenAB(curList[A].easting, curList[A].northing,
                curList[B].easting, curList[B].northing, pivot.easting, pivot.northing))
            {
                goto SegmentFound;
            }
            else
            {
                isFindGlobalNearestTrackPoint = true;
                return;
            }

        SegmentFound:

            currentLocationIndex = A;

            //get the distance from currently active AB line
            dx = curList[B].easting - curList[A].easting;
            dz = curList[B].northing - curList[A].northing;

            if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

            //how far from current AB Line is fix
            distanceFromCurrentLinePivot = ((dz * pivot.easting) - (dx * pivot.northing) + (curList[B].easting
                        * curList[A].northing) - (curList[B].northing * curList[A].easting))
                            / Math.Sqrt((dz * dz) + (dx * dx));

            //integral slider is set to 0
            if (mf.vehicle.purePursuitIntegralGain != 0 && !mf.isReverse)
            {
                pivotDistanceError = distanceFromCurrentLinePivot * 0.2 + pivotDistanceError * 0.8;

                if (counter2++ > 4)
                {
                    pivotDerivative = pivotDistanceError - pivotDistanceErrorLast;
                    pivotDistanceErrorLast = pivotDistanceError;
                    counter2 = 0;
                    pivotDerivative *= 2;

                    //limit the derivative
                    //if (pivotDerivative > 0.03) pivotDerivative = 0.03;
                    //if (pivotDerivative < -0.03) pivotDerivative = -0.03;
                    //if (Math.Abs(pivotDerivative) < 0.01) pivotDerivative = 0;
                }

                //pivotErrorTotal = pivotDistanceError + pivotDerivative;

                if (mf.isBtnAutoSteerOn && mf.avgSpeed > 2.5 && Math.Abs(pivotDerivative) < 0.1)
                {
                    //if over the line heading wrong way, rapidly decrease integral
                    if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                    {
                        inty += pivotDistanceError * mf.vehicle.purePursuitIntegralGain * -0.04;
                    }
                    else
                    {
                        if (Math.Abs(distanceFromCurrentLinePivot) > 0.02)
                        {
                            inty += pivotDistanceError * mf.vehicle.purePursuitIntegralGain * -0.02;
                            if (inty > 0.2) inty = 0.2;
                            else if (inty < -0.2) inty = -0.2;
                        }
                    }
                }
                else inty *= 0.95;
            }
            else inty = 0;

            // ** Pure pursuit ** - calc point on AB Line closest to current position
            double U = (((pivot.easting - curList[A].easting) * dx)
                        + ((pivot.northing - curList[A].northing) * dz))
                        / ((dx * dx) + (dz * dz));

            rEastTrk = curList[A].easting + (U * dx);
            rNorthTrk = curList[A].northing + (U * dz);
            manualUturnHeading = curList[A].heading;

            int count = ReverseHeading ? 1 : -1;
            vec3 start = new vec3(rEastTrk, rNorthTrk, 0);
            double distSoFar = 0;

            for (int i = ReverseHeading ? B : A; i < curList.Count && i >= 0;)
            {
                // used for calculating the length squared of next segment.
                double tempDist = glm.Distance(start, curList[i]);

                //will we go too far?
                if ((tempDist + distSoFar) > goalPointDistance)
                {
                    double j = (goalPointDistance - distSoFar) / tempDist; // the remainder to yet travel

                    goalPointTrk.easting = (((1 - j) * start.easting) + (j * curList[i].easting));
                    goalPointTrk.northing = (((1 - j) * start.northing) + (j * curList[i].northing));

                    double size = goalPointTrk.northing - rNorthTrk + 0.05;
                    if (size < goalPointDistance)
                        break;

                    break;
                }
                else distSoFar += tempDist;
                start = curList[i];
                i += count;
                if (i < 0) i = curList.Count - 1;
                if (i > curList.Count - 1) i = 0;
            }

            if (mf.trk.gArr[mf.trk.idx].mode <= TrackMode.Curve)
            {
                if (mf.isBtnAutoSteerOn && !mf.isReverse)
                {
                    if (mf.trk.isHeadingSameWay)
                    {
                        if (glm.Distance(goalPointTrk, curList[(curList.Count - 1)]) < 0.5)
                        {
                            mf.btnAutoSteer.PerformClick();
                            mf.TimedMessageBox(2000, gStr.gsGuidanceStopped, gStr.gsPastEndOfCurve);
                            Log.EventWriter("Autosteer Stop, Past End of Curve");
                        }
                    }
                    else
                    {
                        if (glm.Distance(goalPointTrk, curList[0]) < 0.5)
                        {
                            mf.btnAutoSteer.PerformClick();
                            mf.TimedMessageBox(2000, gStr.gsGuidanceStopped, gStr.gsPastEndOfCurve);
                            Log.EventWriter("Autosteer Stop, Past End of Curve");
                        }
                    }
                }
            }

            //calc "D" the distance from pivot axle to lookahead point
            double goalPointDistanceSquared = glm.DistanceSquared(goalPointTrk.northing, goalPointTrk.easting, pivot.northing, pivot.easting);

            //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
            //double localHeading = glm.twoPI - mf.fixHeading;

            double localHeading;
            if (ReverseHeading) localHeading = glm.twoPI - mf.fixHeading + inty;
            else localHeading = glm.twoPI - mf.fixHeading - inty;

            steerAngleTrk = glm.toDegrees(Math.Atan(2 * (((goalPointTrk.easting - pivot.easting) * Math.Cos(localHeading))
                + ((goalPointTrk.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

            if (mf.ahrs.imuRoll != 88888)
                steerAngleTrk += mf.ahrs.imuRoll * -mf.gyd.sideHillCompFactor;

            if (steerAngleTrk < -mf.vehicle.maxSteerAngle) steerAngleTrk = -mf.vehicle.maxSteerAngle;
            if (steerAngleTrk > mf.vehicle.maxSteerAngle) steerAngleTrk = mf.vehicle.maxSteerAngle;

            if (!mf.trk.isHeadingSameWay)
                distanceFromCurrentLinePivot *= -1.0;

            //used for acquire/hold mode
            mf.vehicle.modeActualXTE = (distanceFromCurrentLinePivot);

            double steerHeadingError = (pivot.heading - curList[A].heading);
            //Fix the circular error
            if (steerHeadingError > Math.PI)
                steerHeadingError -= Math.PI;
            else if (steerHeadingError < -Math.PI)
                steerHeadingError += Math.PI;

            if (steerHeadingError > glm.PIBy2)
                steerHeadingError -= Math.PI;
            else if (steerHeadingError < -glm.PIBy2)
                steerHeadingError += Math.PI;

            mf.vehicle.modeActualHeadingError = glm.toDegrees(steerHeadingError);

            //Convert to centimeters
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngleTrk * 100);

            //Tool GPS
            if (mf.isGPSToolActive)
            {
                minDistA = double.MaxValue;
                //close call hit
                cc = 0;

                for (int j = 0; j < curList.Count; j += 5)
                {
                    dist = glm.DistanceSquared(mf.pnTool.fix, curList[j]);
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        cc = j;
                    }
                }

                minDistA = double.MaxValue;

                dd = cc + 5; if (dd > curList.Count - 1) dd = curList.Count;
                cc -= 5; if (cc < 0) cc = 0;

                //find the closest 2 points to current close call
                for (int j = cc; j < dd; j++)
                {
                    dist = glm.DistanceSquared(mf.pnTool.fix, curList[j]);
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        A = j;
                    }
                }

                if (A > curList.Count - 1)
                    return;

                //initial forward Test if gps2 InRange AB
                if (A == curList.Count - 1) B = 0;
                else B = A + 1;

                if (glm.InRangeBetweenAB(curList[A].easting, curList[A].northing,
                     curList[B].easting, curList[B].northing, mf.pnTool.fix.easting, mf.pnTool.fix.northing))
                    goto Segment2Found;

                //step back one
                if (A == 0)
                {
                    A = curList.Count - 1;
                    B = 0;
                }
                else
                {
                    A--;
                    B = A + 1;
                }

                if (glm.InRangeBetweenAB(curList[A].easting, curList[A].northing,
                    curList[B].easting, curList[B].northing, mf.pnTool.fix.easting, mf.pnTool.fix.northing))
                {
                    goto Segment2Found;
                }

            Segment2Found:

                //get the distance from currently active AB line
                dx = curList[B].easting - curList[A].easting;
                dz = curList[B].northing - curList[A].northing;

                if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLineTool = ((dz * mf.pnTool.fix.easting) - (dx * mf.pnTool.fix.northing) + (curList[B].easting
                            * curList[A].northing) - (curList[B].northing * curList[A].easting))
                                / Math.Sqrt((dz * dz) + (dx * dx));

                if (!mf.trk.isHeadingSameWay)
                    distanceFromCurrentLineTool *= -1.0;

                mf.guidanceLineDistanceOffTool = (short)Math.Round(distanceFromCurrentLineTool * 1000.0, MidpointRounding.AwayFromZero);
            }
        }

        #endregion PurePursuit
    }
}