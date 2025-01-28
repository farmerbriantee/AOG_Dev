using AgOpenGPS.Culture;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CGuidance
    {
        private readonly FormGPS mf;

        //steer, pivot, and ref indexes
        private int sA, sB, C, pA, pB;
        private int A, B;

        //private int rA, rB;

        public double distanceFromCurrentLineSteer, distanceFromCurrentLinePivot;
        public double steerAngleGu, rEastSteer, rNorthSteer, rEastPivot, rNorthPivot;
        public double steerAngleTrk;
        private vec2 goalPointTrk = new vec2();

        public double rEastTrk, rNorthTrk, manualUturnHeading;

        public double inty, xTrackSteerCorrection = 0;
        public double steerHeadingError, steerHeadingErrorDegrees;

        public double distSteerError, lastDistSteerError, derivativeDistError;

        public double pivotDistanceError, stanleyModeMultiplier;

        //public int modeTimeCounter = 0;

        //for adding steering angle based on side slope hill
        public double sideHillCompFactor;

        //derivative counter
        private int counter;

        //derivative counters
        private int counter2;

        // Should we find the global nearest curve point (instead of local) on the next search.
        public bool findGlobalNearestTrackPoint = true;

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

            //derivative of steer distance error
            distSteerError = (distSteerError * 0.95) + ((xTrackSteerCorrection * 60) * 0.05);
            if (counter++ > 5)
            {
                derivativeDistError = distSteerError - lastDistSteerError;
                lastDistSteerError = distSteerError;
                counter = 0;
            }

            steerAngleGu = glm.toDegrees((xTrackSteerCorrection + steerHeadingError) * -1.0);

            if (Math.Abs(distanceFromCurrentLineSteer) > 0.5) steerAngleGu *= 0.5;
            else steerAngleGu *= (1 - Math.Abs(distanceFromCurrentLineSteer));

            //pivot PID
            pivotDistanceError = (pivotDistanceError * 0.6) + (distanceFromCurrentLinePivot * 0.4);
            //pivotDistanceError = Math.Atan((distanceFromCurrentLinePivot) / (sped)) * 0.2;
            //pivotErrorTotal = pivotDistanceError + pivotDerivative;

            if (mf.avgSpeed > 1
                && mf.isBtnAutoSteerOn
                && Math.Abs(derivativeDistError) < 1
                && Math.Abs(pivotDistanceError) < 0.25)
            {
                //if over the line heading wrong way, rapidly decrease integral
                if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                {
                    inty += pivotDistanceError * mf.vehicle.stanleyIntegralGainAB * -0.03;
                }
                else
                {
                    inty += pivotDistanceError * mf.vehicle.stanleyIntegralGainAB * -0.01;
                }

                //integral slider is set to 0
                if (mf.vehicle.stanleyIntegralGainAB == 0) inty = 0;
            }
            else inty *= 0.7;

            if (mf.isReverse) inty = 0;

            if (mf.ahrs.imuRoll != 88888)
                steerAngleGu += mf.ahrs.imuRoll * -sideHillCompFactor;

            if (steerAngleGu < -mf.vehicle.maxSteerAngle) steerAngleGu = -mf.vehicle.maxSteerAngle;
            else if (steerAngleGu > mf.vehicle.maxSteerAngle) steerAngleGu = mf.vehicle.maxSteerAngle;

            //used for smooth mode
            mf.vehicle.modeActualXTE = (distanceFromCurrentLinePivot);

            //Convert to millimeters from meters
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngleGu * 100);
        }

        /// <summary>
        /// Find the steer angle for a curve list, curvature and integral
        /// </summary>
        /// <param name="pivot">Pivot position vector</param>
        /// <param name="steer">Steer position vector</param>
        /// <param name="curList">the current list of guidance points</param>
        public void StanleyGuidance(vec3 steer, ref List<vec3> curList)
        {            //calculate required steer angle
            //find the closest point roughly
            int cc = 0, dd;
            int ptCount = curList.Count;
            if (ptCount > 5)
            {
                double minDistA = 1000000, minDistB;

                for (int j = 0; j < ptCount; j += 10)
                {
                    double dist = ((steer.easting - curList[j].easting) * (steer.easting - curList[j].easting))
                                    + ((steer.northing - curList[j].northing) * (steer.northing - curList[j].northing));
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        cc = j;
                    }
                }

                minDistA = minDistB = 1000000;
                dd = cc + 7; if (dd > ptCount - 1) dd = ptCount;
                cc -= 7; if (cc < 0) cc = 0;

                //find the closest 2 points to current close call
                for (int j = cc; j < dd; j++)
                {
                    double dist = ((steer.easting - curList[j].easting) * (steer.easting - curList[j].easting))
                                    + ((steer.northing - curList[j].northing) * (steer.northing - curList[j].northing));
                    if (dist < minDistA)
                    {
                        minDistB = minDistA;
                        sB = sA;
                        minDistA = dist;
                        sA = j;
                    }
                    else if (dist < minDistB)
                    {
                        minDistB = dist;
                        sB = j;
                    }
                }

                ////too far from guidance line? Lost? Fresh delete of ref?
                //if (minDistA < (1.5 * (mf.tool.toolWidth * mf.tool.toolWidth)))
                //{
                //    if (minDistA == 100000000)
                //        return;
                //}
                //else
                //{
                //    curList.Clear();
                //    return;
                //}

                //just need to make sure the points continue ascending or heading switches all over the place
                if (sA > sB) { C = sA; sA = sB; sB = C; }

                //currentLocationIndex = sA;
                if (sA > ptCount - 1 || sB > ptCount - 1) return;

                minDistA = minDistB = 1000000;

                if (mf.trk.isHeadingSameWay)
                {
                    dd = sB; cc = dd - 12; if (cc < 0) cc = 0;
                }
                else
                {
                    cc = sA; dd = sA + 12; if (dd >= ptCount) dd = ptCount - 1;
                }

                //find the closest 2 points of pivot back from steer
                for (int j = cc; j < dd; j++)
                {
                    double dist = ((steer.easting - curList[j].easting) * (steer.easting - curList[j].easting))
                                    + ((steer.northing - curList[j].northing) * (steer.northing - curList[j].northing));
                    if (dist < minDistA)
                    {
                        minDistB = minDistA;
                        pB = pA;
                        minDistA = dist;
                        pA = j;
                    }
                    else if (dist < minDistB)
                    {
                        minDistB = dist;
                        pB = j;
                    }
                }

                //just need to make sure the points continue ascending or heading switches all over the place
                if (pA > pB) { C = pA; pA = pB; pB = C; }

                if (pA > ptCount - 1 || pB > ptCount - 1)
                {
                    pA = ptCount - 2;
                    pB = ptCount - 1;
                }

                vec3 pivA = new vec3(curList[pA]);
                vec3 pivB = new vec3(curList[pB]);

                if (!mf.trk.isHeadingSameWay)
                {
                    pivA = curList[pB];
                    pivB = curList[pA];

                    pivA.heading += Math.PI;
                    if (pivA.heading > glm.twoPI) pivA.heading -= glm.twoPI;
                }

                manualUturnHeading = pivA.heading;

                //get the pivot distance from currently active AB segment   ///////////  Pivot  ////////////
                double dx = pivB.easting - pivA.easting;
                double dz = pivB.northing - pivA.northing;

                if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLinePivot = ((dz * steer.easting) - (dx * steer.northing) + (pivB.easting
                            * pivA.northing) - (pivB.northing * pivA.easting))
                                / Math.Sqrt((dz * dz) + (dx * dx));

                mf.trk.distanceFromCurrentLinePivot = distanceFromCurrentLinePivot;
                double U = (((steer.easting - pivA.easting) * dx)
                                + ((steer.northing - pivA.northing) * dz))
                                / ((dx * dx) + (dz * dz));

                rEastPivot = pivA.easting + (U * dx);
                rNorthPivot = pivA.northing + (U * dz);

                rEastTrk = rEastPivot;
                rNorthTrk = rNorthPivot;

                currentLocationIndex = pA;

                //get the distance from currently active AB segment of steer axle //////// steer /////////////
                vec3 steerA = new vec3(curList[sA]);
                vec3 steerB = new vec3(curList[sB]);

                if (!mf.trk.isHeadingSameWay)
                {
                    steerA = curList[sB];
                    steerA.heading += Math.PI;
                    if (steerA.heading > glm.twoPI) steerA.heading -= glm.twoPI;

                    steerB = curList[sA];
                    steerB.heading += Math.PI;
                    if (steerB.heading > glm.twoPI) steerB.heading -= glm.twoPI;
                }

                //double curvature = pivA.heading - steerA.heading;
                //if (curvature > Math.PI) curvature -= Math.PI; else if (curvature < Math.PI) curvature += Math.PI;
                //if (curvature > glm.PIBy2) curvature -= Math.PI; else if (curvature < -glm.PIBy2) curvature += Math.PI;

                ////because of draft
                //curvature = Math.Sin(curvature) * mf.vehicle.wheelbase * 0.8;
                //pivotCurvatureOffset = (pivotCurvatureOffset * 0.7) + (curvature * 0.3);
                //pivotCurvatureOffset = 0;

                //create the AB segment to offset
                steerA.easting += (Math.Sin(steerA.heading + glm.PIBy2) * (inty));
                steerA.northing += (Math.Cos(steerA.heading + glm.PIBy2) * (inty));

                steerB.easting += (Math.Sin(steerB.heading + glm.PIBy2) * (inty));
                steerB.northing += (Math.Cos(steerB.heading + glm.PIBy2) * (inty));

                dx = steerB.easting - steerA.easting;
                dz = steerB.northing - steerA.northing;

                if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLineSteer = ((dz * steer.easting) - (dx * steer.northing) + (steerB.easting
                            * steerA.northing) - (steerB.northing * steerA.easting))
                                / Math.Sqrt((dz * dz) + (dx * dx));

                // calc point on AB Line closest to current position - for display only
                U = (((steer.easting - steerA.easting) * dx)
                                + ((steer.northing - steerA.northing) * dz))
                                / ((dx * dx) + (dz * dz));

                rEastSteer = steerA.easting + (U * dx);
                rNorthSteer = steerA.northing + (U * dz);

                //double segHeading = Math.Atan2(rEastSteer - rEastPivot, rNorthSteer - rNorthPivot);

                //steerHeadingError = Math.PI - Math.Abs(Math.Abs(pivot.heading - segHeading) - Math.PI);
                steerHeadingError = steer.heading - steerB.heading;

                //Fix the circular error
                if (steerHeadingError > Math.PI) steerHeadingError -= Math.PI;
                else if (steerHeadingError < Math.PI) steerHeadingError += Math.PI;

                if (steerHeadingError > glm.PIBy2) steerHeadingError -= Math.PI;
                else if (steerHeadingError < -glm.PIBy2) steerHeadingError += Math.PI;

                DoSteerAngleCalc();
            }
            else
            {
                //invalid distance so tell AS module
                distanceFromCurrentLineSteer = 32000;
                mf.guidanceLineDistanceOff = 32000;
            }
        }


        public void PurePursuitGuidance(vec3 pivot, ref List<vec3> curList)
        {
            double minDistA;
            double minDistB;
            double dist, dx, dz;

            //close call hit

            bool isAddStart = false, isAddEnd = false;

            double goalPointDistance = mf.vehicle.UpdateGoalPointDistance();
            bool ReverseHeading = mf.isReverse ? !mf.trk.isHeadingSameWay : mf.trk.isHeadingSameWay;

            //If is a curve or an AB made into curve
            if (mf.trk.gArr[mf.trk.idx].mode <= TrackMode.Curve)
            {
                minDistB = double.MaxValue;
                //close call hit
                int cc, dd;

                if (findGlobalNearestTrackPoint)
                {
                    // When not already following some line, find the globally nearest point

                    cc = findNearestGlobalCurvePoint(pivot, ref curList, 10);

                    findGlobalNearestTrackPoint = false;
                }
                else
                {
                    // When already "locked" to follow some line, try to find the "local" nearest point
                    // based on the last one. This prevents jumping between lines close to each other (or crossing lines).
                    // As this is prone to find a "local minimum", this should only be used when already following some line.

                    cc = findNearestLocalCurvePoint(pivot, currentLocationIndex, goalPointDistance, ReverseHeading, ref curList);
                }

                minDistA = double.MaxValue;

                dd = cc + 8; if (dd > curList.Count - 1) dd = curList.Count;
                cc -= 8; if (cc < 0) cc = 0;

                //find the closest 2 points to current close call
                for (int j = cc; j < dd; j++)
                {
                    dist = glm.DistanceSquared(pivot, curList[j]);
                    if (dist < minDistA)
                    {
                        minDistB = minDistA;
                        B = A;
                        minDistA = dist;
                        A = j;
                    }
                    else if (dist < minDistB)
                    {
                        minDistB = dist;
                        B = j;
                    }
                }

                //just need to make sure the points continue ascending or heading switches all over the place
                if (A > B) { C = A; A = B; B = C; }

                currentLocationIndex = A;

                if (A > curList.Count - 1 || B > curList.Count - 1)
                    return;

                if (A > curList.Count - 50)
                    isAddEnd = true;
                else if (A < 50)
                    isAddStart = true;
            }
            else
            {
                if (findGlobalNearestTrackPoint)
                {
                    // When not already following some line, find the globally nearest point

                    A = findNearestGlobalCurvePoint(pivot, ref curList);

                    findGlobalNearestTrackPoint = false;
                }
                else
                {
                    // When already "locked" to follow some line, try to find the "local" nearest point
                    // based on the last one. This prevents jumping between lines close to each other (or crossing lines).
                    // As this is prone to find a "local minimum", this should only be used when already following some line.

                    A = findNearestLocalCurvePoint(pivot, currentLocationIndex, goalPointDistance, ReverseHeading, ref curList);
                }

                currentLocationIndex = A;

                if (A > curList.Count - 1)
                    return;

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
                    goto SegmentFound;

                //realy really lost
                return;
            }

        SegmentFound:

            //get the distance from currently active AB line

            dx = curList[B].easting - curList[A].easting;
            dz = curList[B].northing - curList[A].northing;

            if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

            //abHeading = Math.Atan2(dz, dx);

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

            if (isAddStart)
            {
                mf.trk.AddStartPoints(ref curList, 100);
            }

            if (isAddEnd)
            {
                mf.trk.AddEndPoints(ref curList, 100);
            }
        }


        // Searches for the nearest "global" curve point to the refPoint by checking all points of the trk.
        // Parameter "increment" added here to give possibility to make a "sparser" search (to speed it up?)
        // Return: index to the nearest point
        private int findNearestGlobalCurvePoint(vec3 refPoint, ref List<vec3> curList, int increment = 1)
        {
            double minDist = double.MaxValue;
            int minDistIndex = 0;

            for (int i = 0; i < curList.Count; i += increment)
            {
                double dist = glm.DistanceSquared(refPoint, curList[i]);
                if (dist < minDist)
                {
                    minDist = dist;
                    minDistIndex = i;
                }
            }
            return minDistIndex;
        }

        // Searches for the nearest "local" curve point to the refPoint by traversing forward and backward on the curve
        // startIndex means the starting point (index to curList) of the search.
        // Return: index to the nearest (local) point
        private int findNearestLocalCurvePoint(vec3 refPoint, int startIndex, double minSearchDistance, bool reverseSearchDirection, ref List<vec3> curList)
        {
            double minDist = glm.DistanceSquared(refPoint, curList[(startIndex + curList.Count) % curList.Count]);
            int minDistIndex = startIndex;

            int directionMultiplier = reverseSearchDirection ? 1 : -1;
            double distSoFar = 0;
            vec3 start = curList[startIndex];

            // Check all points' distances from the pivot inside the "look ahead"-distance and find the nearest
            int offset = 1;

            while (offset < curList.Count)
            {
                int pointIndex = (startIndex + (offset * directionMultiplier) + curList.Count) % curList.Count;  // Wrap around
                double dist = glm.DistanceSquared(refPoint, curList[pointIndex]);

                if (dist < minDist)
                {
                    minDist = dist;
                    minDistIndex = pointIndex;
                }

                distSoFar += glm.Distance(start, curList[pointIndex]);
                start = curList[pointIndex];

                offset++;

                if (distSoFar > minSearchDistance)
                {
                    break;
                }
            }

            // Continue traversing until the distance starts growing
            while (offset < curList.Count)
            {
                int pointIndex = (startIndex + (offset * directionMultiplier) + curList.Count) % curList.Count;  // Wrap around
                double dist = glm.DistanceSquared(refPoint, curList[pointIndex]);
                if (dist < minDist)
                {
                    // Getting closer
                    minDist = dist;
                    minDistIndex = pointIndex;
                }
                else
                {
                    // Getting farther, no point to continue
                    break;
                }
                offset++;
            }

            // Traverse from the start point also into another direction to be sure we choose the minimum local distance.
            // (This is also needed due to the way AB-curve is handled (the search may start one off from the last known nearest point)).
            for (offset = 1; offset < curList.Count; offset++)
            {
                int pointIndex = (startIndex + (offset * (-directionMultiplier)) + curList.Count) % curList.Count;  // Wrap around
                double dist = glm.DistanceSquared(refPoint, curList[pointIndex]);
                if (dist < minDist)
                {
                    // Getting closer
                    minDist = dist;
                    minDistIndex = pointIndex;
                }
                else
                {
                    // Getting farther, no point to continue
                    break;
                }
            }

            return minDistIndex;
        }

        #endregion Stanley
    }
}