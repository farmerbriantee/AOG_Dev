using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CYouTurn
    {
        #region Fields

        //copy of the mainform address
        private readonly FormGPS mf;

        /// <summary>/// triggered right after youTurnTriggerPoint is set /// </summary>
        public bool isYouTurnTriggered;

        /// <summary>  /// turning right or left?/// </summary>
        public bool isTurnLeft;

        /// <summary> /// Is the youturn button enabled? /// </summary>
        public bool isYouTurnBtnOn;

        public double youTurnRadius;

        public int rowSkipsWidth = 1, uTurnSmoothing;

        public bool alternateSkips = false, previousBigSkip = true;
        public int rowSkipsWidth2 = 3, turnSkips = 2;

        /// <summary>  /// distance from headland as offset where to start turn shape /// </summary>
        public int youTurnStartOffset;

        //guidance values
        public double uturnDistanceFromBoundary;

        public bool isTurnCreationTooClose = false, isTurnCreationNotCrossingError = false, turnTooCloseTrigger = false;

        //list of points for scaled and rotated YouTurn line, used for pattern, dubins, abcurve
        public List<vec3> ytList = new List<vec3>();

        private List<vec3> ytList2 = new List<vec3>();

        //next curve or line to build out turn and point over
        public List<vec3> nextCurve = new List<vec3>();

        //if we continue on the same line or change to the next one after the uTurn
        public bool isGoingStraightThrough;

        public int uTurnStyle = 0;

        //is UTurn pattern in or out of bounds
        public bool isOutOfBounds = false;

        //sequence of operations of finding the next turn 0 to 3
        public int youTurnPhase;

        // Returns 1 if the lines intersect, otherwis
        public double iE = 0, iN = 0;

        // the list of possible bounds points
        public List<CClose> turnClosestList = new List<CClose>();

        //point at the farthest turn segment from pivotAxle
        public CClose closestTurnPt = new CClose();

        //where the in and out tangents cross for Albin curve
        public CClose inClosestTurnPt = new CClose();

        public CClose outClosestTurnPt = new CClose();



        CClose movePoint = new CClose();

        CClose exitPoint = new CClose();
        CClose exitPoint2 = new CClose();
        CClose entryPoint = new CClose();
        CClose entryPoint2 = new CClose();


        public CClose startOfTurnPt = new CClose();

        public int onA;

        #endregion Fields

        //constructor
        public CYouTurn(FormGPS _f)
        {
            mf = _f;
            LoadSettings();
        }

        public void LoadSettings()
        {
            uturnDistanceFromBoundary = Settings.Vehicle.set_youTurnDistanceFromBoundary;

            //how far before or after boundary line should turn happen
            youTurnStartOffset = Settings.Vehicle.set_youTurnExtensionLength;

            rowSkipsWidth = Settings.Vehicle.set_youSkipWidth;
            Set_Alternate_skips();

            ytList.Capacity = 128;

            youTurnRadius = Settings.Vehicle.set_youTurnRadius;

            uTurnStyle = Settings.Vehicle.set_uTurnStyle;

            uTurnSmoothing = Settings.Vehicle.setAS_uTurnSmoothing;
        }


        #region CreateTurn
        //Finds the point where an AB Curve crosses the turn line
        public void BuildCurveDubinsYouTurn()
        {
            double turnOffset = (Settings.Tool.toolWidth - Settings.Tool.maxOverlap) * rowSkipsWidth + (isTurnLeft ? -Settings.Tool.offset * 2.0 : Settings.Tool.offset * 2.0);
            bool isTurnRight = turnOffset > 0 ^ isTurnLeft;


            if (mf.trk.idx < 0 || mf.trk.gArr.Count < mf.trk.idx) return;
            CTrk track = mf.trk.gArr[mf.trk.idx];

            bool loop = track.mode == TrackMode.bndCurve || track.mode == TrackMode.waterPivot;


            if (youTurnPhase < 4)
            {
                youTurnPhase++;
                if (youTurnPhase == 4)
                    youTurnPhase = 10;
            }
            else if (youTurnPhase == 10)
            {
                #region FindExitPoint
                ytList.Clear();

                bool Loop = true;
                int Count = mf.trk.isHeadingSameWay ? 1 : -1;
                CClose Crossing = new CClose();

                vec3 Start = new vec3(mf.gyd.rEastTrk, mf.gyd.rNorthTrk), End;

                for (int i = mf.trk.isHeadingSameWay ? mf.gyd.B : mf.gyd.A; (mf.trk.isHeadingSameWay ? i < mf.gyd.B : i > mf.gyd.A) || Loop; i += Count)
                {
                    if ((mf.trk.isHeadingSameWay && i >= mf.trk.currentGuidanceTrack.Count) || (!mf.trk.isHeadingSameWay && i < 0))
                    {
                        if (loop && Loop)
                        {
                            if (i < 0)
                                i = mf.trk.currentGuidanceTrack.Count;
                            else
                                i = -1;
                            Loop = false;
                            continue;
                        }
                        else break;
                    }

                    End = new vec3(mf.trk.currentGuidanceTrack[i].easting, mf.trk.currentGuidanceTrack[i].northing);
                    for (int j = 0; j < mf.bnd.bndList.Count; j++)
                    {
                        if (mf.bnd.bndList[j].isDriveThru) continue;

                        int k = mf.bnd.bndList[j].turnLine.Count - 1;
                        for (int l = 0; l < mf.bnd.bndList[j].turnLine.Count; l++)
                        {
                            if (GetLineIntersection(Start, End, mf.bnd.bndList[j].turnLine[k], mf.bnd.bndList[j].turnLine[l], out vec3 _Crossing, out double time, out _))
                            {
                                if (time < Crossing.time)
                                    Crossing = new CClose(_Crossing, time, j, i, k);
                            }
                            k = l;
                        }
                    }

                    if (Crossing.turnLineNum >= 0)
                    {
                        movePoint = new CClose(exitPoint = Crossing);
                        break;
                    }
                    Start = End;
                }

                if (Crossing.turnLineNum == -1)//didnt hit any turn line
                {
                    if (track.mode == TrackMode.waterPivot || track.mode == TrackMode.bndCurve)
                    {
                        youTurnPhase = 251;//ignore
                    }
                    else//curve does not cross a boundary - oops
                    {
                        isTurnCreationNotCrossingError = true;
                        FailCreate();
                    }
                }
                else
                    youTurnPhase = 20;
                #endregion FindExitPoint
            }
            else if (youTurnPhase < 60)//step 2 move the turn inside with steps of 1 meter
            {
                double step = youTurnPhase == 20 ? 5.0 : youTurnPhase == 30 ? 1.0 : youTurnPhase == 40 ? 0.2 : 0.04;

                movePoint = MoveTurnLine(mf.trk.currentGuidanceTrack, movePoint, step, mf.trk.isHeadingSameWay ^ youTurnPhase % 20 == 0, false, loop);

                // creates half a circle starting at the crossing point
                double extraSagitta = 0;
                if (Math.Abs(turnOffset) < youTurnRadius * 2)
                    extraSagitta = (youTurnRadius * 2 - Math.Abs(turnOffset)) * 0.5;

                int A = movePoint.curveIndex;
                double head = Math.Atan2(mf.trk.currentGuidanceTrack[A + 1].easting - mf.trk.currentGuidanceTrack[A].easting, mf.trk.currentGuidanceTrack[A + 1].northing - mf.trk.currentGuidanceTrack[A].northing);

                ytList = GetOffsetSemicirclePoints(movePoint.closePt, head + (mf.trk.isHeadingSameWay ? 0 : Math.PI), isTurnRight, youTurnRadius, extraSagitta, uTurnStyle == 1 ? 2.2 : Math.PI);

                mf.distancePivotToTurnLine = glm.Distance(ytList[0], mf.pivotAxlePos);

                if (mf.distancePivotToTurnLine < 3)
                {
                    FailCreate();
                }
                else
                {
                    isOutOfBounds = false;
                    //Are we out of bounds?
                    for (int j = 0; j < ytList.Count; j++)
                    {
                        if (mf.bnd.IsPointInsideTurnArea(ytList[j]) != 0)
                        {
                            isOutOfBounds = true;
                            break;
                        }
                    }
                    if (isOutOfBounds ^ youTurnPhase % 20 == 0)
                        youTurnPhase += 10;

                    isOutOfBounds = true;
                }
            }
            else if (uTurnStyle == 1)
            {
                isOutOfBounds = false;
                youTurnPhase = 255;
            }
            else if (youTurnPhase == 60)//remove part outside
            {
                bool found = false;
                var turnLine = mf.bnd.bndList[movePoint.turnLineNum].turnLine;
                //Are we out of bounds?
                for (int i = 1; i < ytList.Count; i++)
                {
                    int j = turnLine.Count - 1;
                    for (int k = 0; k < turnLine.Count; j = k++)
                    {
                        if (GetLineIntersection(turnLine[j], turnLine[k], ytList[i - 1], ytList[i], out vec3 _crossing, out double time, out _))
                        {
                            found = true;
                            ytList.RemoveRange(i, ytList.Count - i);
                            exitPoint2 = new CClose(_crossing, time, movePoint.turnLineNum, 0, j);
                            break;
                        }
                    }
                    if (found) break;
                }

                if (!found)
                {
                    FailCreate();
                }
                else
                    youTurnPhase += 10;
            }
            else if (youTurnPhase == 70)//build the next line to add sequencelines
            {
                //build the next line to add sequencelines
                double widthMinusOverlap = Settings.Tool.toolWidth - Settings.Tool.maxOverlap;

                double distAway = widthMinusOverlap * (mf.trk.howManyPathsAway + (isTurnLeft ^ mf.trk.isHeadingSameWay ? rowSkipsWidth : -rowSkipsWidth)) + (mf.trk.isHeadingSameWay ? -Settings.Tool.offset : Settings.Tool.offset) + track.nudgeDistance;

                distAway += 0.5 * widthMinusOverlap;

                //create the next line
                nextCurve = mf.trk.BuildCurrentGuidanceTrack(distAway, track);


                bool isTurnLineSameWay = !isTurnRight ^ movePoint.turnLineNum == 0;
                if (!FindCurveOutTurnPoint(mf.trk, ref nextCurve, exitPoint, isTurnLineSameWay))
                {
                    //error
                    FailCreate();
                }
                else
                {
                    youTurnPhase += 10;
                    movePoint = new CClose(entryPoint = closestTurnPt);
                }
            }
            else if (youTurnPhase < 120)//step 2 move the turn inside with steps of 1 meter
            {
                double step = youTurnPhase == 80 ? 5.0 : youTurnPhase == 90 ? 1.0 : youTurnPhase == 100 ? 0.2 : 0.04;

                movePoint = MoveTurnLine(nextCurve, movePoint, step, mf.trk.isHeadingSameWay ^ isGoingStraightThrough ^ youTurnPhase % 20 == 0, false, loop);

                // creates half a circle starting at the crossing point
                double extraSagitta = 0;
                if (Math.Abs(turnOffset) < youTurnRadius * 2)
                    extraSagitta = (youTurnRadius * 2 - Math.Abs(turnOffset)) * 0.5;

                int A = movePoint.curveIndex;
                double head = Math.Atan2(nextCurve[A + 1].easting - nextCurve[A].easting, nextCurve[A + 1].northing - nextCurve[A].northing);

                ytList2 = GetOffsetSemicirclePoints(movePoint.closePt, head + (mf.trk.isHeadingSameWay ^ isGoingStraightThrough ? 0 : Math.PI), !isTurnRight, youTurnRadius, extraSagitta, uTurnStyle == 1 ? 2.2 : Math.PI);

                isOutOfBounds = false;
                //Are we out of bounds?
                for (int j = 0; j < ytList2.Count; j++)
                {
                    if (mf.bnd.IsPointInsideTurnArea(ytList2[j]) != 0)
                    {
                        isOutOfBounds = true;
                        break;
                    }
                }
                if (isOutOfBounds ^ youTurnPhase % 20 == 0)
                    youTurnPhase += 10;

                isOutOfBounds = true;
            }
            else if (youTurnPhase == 120)//remove part outside
            {
                bool found = false;
                var turnLine = mf.bnd.bndList[movePoint.turnLineNum].turnLine;
                //Are we out of bounds?
                for (int i = 1; i < ytList2.Count; i++)
                {
                    int j = turnLine.Count - 1;
                    for (int k = 0; k < turnLine.Count; j = k++)
                    {
                        if (GetLineIntersection(turnLine[j], turnLine[k], ytList2[i - 1], ytList2[i], out vec3 _crossing, out double time, out _))
                        {
                            found = true;
                            ytList2.RemoveRange(i, ytList2.Count - i);
                            entryPoint2 = new CClose(_crossing, time, movePoint.turnLineNum, 0, j);
                            break;
                        }
                    }
                    if (found) break;
                }

                if (!found)
                {
                    FailCreate();
                }
                else
                    youTurnPhase += 10;
            }
            else//join the two halves
            {
                if (exitPoint2.turnLineIndex != entryPoint2.turnLineIndex)
                {
                    var turnLine = mf.bnd.bndList[movePoint.turnLineNum].turnLine;
                    bool isTurnLineSameWay = isTurnRight ^ movePoint.turnLineNum != 0;
                    bool loop2 = isTurnLineSameWay == exitPoint2.turnLineIndex > entryPoint2.turnLineIndex;
                    int cc = isTurnLineSameWay ? 1 : -1;

                    for (int i = exitPoint2.turnLineIndex + cc; loop2 || (isTurnLineSameWay ? i < entryPoint2.turnLineIndex : i > entryPoint2.turnLineIndex); i += cc)
                    {
                        if (i < 0)
                        {
                            i = turnLine.Count - 1;
                            loop2 = false;
                        }
                        else if (i >= turnLine.Count)
                        {
                            i = 0;
                            loop2 = false;

                        }
                        //add the points between
                        ytList.Add(turnLine[i]);
                    }
                }

                ytList2.Reverse();
                ytList.AddRange(ytList2);
                ytList2.Clear();

                //start at ytList[0] and ytList[count -1]!!!!
                //AddCurveSequenceLines(Settings.Vehicle.set_youTurnExtensionLength);


                isOutOfBounds = false;
                youTurnPhase = 255;

            }
        }

        #endregion CreateTurn

        private List<vec3> GetOffsetSemicirclePoints(vec3 currentPos, double theta, bool isTurningRight, double turningRadius, double offsetDistance, double angle = Math.PI)
        {
            List<vec3> points = new List<vec3>();
            points.Add(currentPos);

            double firstArcAngle = Math.Acos(1 - (offsetDistance * 0.5) / turningRadius);

            if (offsetDistance > 0)
            {
                AddCoordinatesToPath(ref currentPos, ref theta, points, firstArcAngle * turningRadius, !isTurningRight, turningRadius);
            }

            // Calculate the total remaining angle to complete the semicircle
            double remainingAngle = angle + firstArcAngle;

            AddCoordinatesToPath(ref currentPos, ref theta, points, remainingAngle * turningRadius, isTurningRight, turningRadius);
            return points;
        }

        private void AddCoordinatesToPath(ref vec3 currentPos, ref double theta, List<vec3> finalPath, double length, bool isTurningRight, double turningRadius)
        {
            int segments = (int)Math.Ceiling(length / (youTurnRadius * 0.1));

            double dist = length / segments;

            //Which way are we turning?
            double turnParameter = (dist / turningRadius) * (isTurningRight ? 1.0 : -1.0);
            double radius = isTurningRight ? turningRadius : -turningRadius;

            double sinH = Math.Sin(theta);
            double cosH = Math.Cos(theta);

            for (int i = 0; i < segments; i++)
            {
                currentPos.easting += cosH * radius;
                currentPos.northing -= sinH * radius;
                //Update the heading
                theta += turnParameter;
                theta %= glm.twoPI;
                sinH = Math.Sin(theta);
                cosH = Math.Cos(theta);
                currentPos.easting -= cosH * radius;
                currentPos.northing += sinH * radius;

                finalPath.Add(currentPos);
            }
        }

        #region FindTurnPoint

        public bool FindCurveOutTurnPoint(CTracks thisCurve, ref List<vec3> nextCurve, CClose inPt, bool isTurnLineSameWay)
        {
            int a = isTurnLineSameWay ? 1 : -1;

            int turnLineNum = inPt.turnLineNum;//ss
            var turnLine = mf.bnd.bndList[turnLineNum].turnLine;

            int stopTurnLineIndex = isTurnLineSameWay ? inPt.turnLineIndex : inPt.turnLineIndex + 1;

            vec3 from = new vec3(inPt.closePt);

            //int turnLineIndex = inPt.turnLineIndex;


            closestTurnPt = new CClose();


            for (int turnLineIndex = isTurnLineSameWay ? inPt.turnLineIndex + 1 : inPt.turnLineIndex; turnLineIndex != stopTurnLineIndex; turnLineIndex += a)
            {
                if (turnLineIndex < 0) turnLineIndex = turnLine.Count - 1; //AAA could be less than 0???
                if (turnLineIndex >= turnLine.Count) turnLineIndex = 0;




                for (int i = 0; i < nextCurve.Count - 2; i++)
                {
                    if (GetLineIntersection(from, turnLine[turnLineIndex], nextCurve[i], nextCurve[i + 1], out vec3 _crossing, out double time, out _))
                    {
                        if (time < closestTurnPt.time)
                        {
                            closestTurnPt.time = time;
                            closestTurnPt.closePt = _crossing;
                            closestTurnPt.turnLineIndex = turnLineIndex;
                            closestTurnPt.curveIndex = i;
                            closestTurnPt.turnLineNum = turnLineNum;
                            isGoingStraightThrough = false;
                        }
                    }
                }

                for (int i = 0; i < thisCurve.currentGuidanceTrack.Count - 2; i++)
                {
                    if (GetLineIntersection(from, turnLine[turnLineIndex], thisCurve.currentGuidanceTrack[i], thisCurve.currentGuidanceTrack[i + 1], out vec3 _crossing, out double time, out _))
                    {
                        if ((i < inPt.curveIndex && thisCurve.isHeadingSameWay) || (i > inPt.curveIndex && !thisCurve.isHeadingSameWay))
                        {
                            return false; //hitting the curve behind us
                        }
                        else if (time < closestTurnPt.time)
                        {
                            closestTurnPt.time = time;
                            closestTurnPt.closePt = _crossing;
                            closestTurnPt.turnLineIndex = turnLineIndex;
                            closestTurnPt.curveIndex = i;
                            closestTurnPt.turnLineNum = turnLineNum;
                            isGoingStraightThrough = true;
                        }

                        else if (i == inPt.curveIndex)
                        {
                            //do nothing hitting the curve at the same place as in
                        }
                        else
                        {
                            closestTurnPt.closePt = _crossing;
                            closestTurnPt.turnLineIndex = turnLineIndex;
                            closestTurnPt.curveIndex = i;
                            closestTurnPt.turnLineNum = turnLineNum;
                            isGoingStraightThrough = true;
                            return true;
                        }
                    }
                }

                if (closestTurnPt.time <= 1)
                {
                    if (isGoingStraightThrough)
                        nextCurve = thisCurve.currentGuidanceTrack;//???? already created the line so why change it
                    return true;
                }

                from = turnLine[turnLineIndex];
            }
            return false;
        }

        private bool FindInnerTurnPoints(vec3 fromPt, double inDirection, CClose refClosePt, bool isTurnLineSameWay)
        {
            vec3 toPt = new vec3(fromPt.easting + Math.Sin(inDirection), fromPt.northing + Math.Cos(inDirection), 0);

            int a = isTurnLineSameWay ? 1 : -1;

            int turnLineIndex = refClosePt.turnLineIndex;
            int turnLineNum = refClosePt.turnLineNum;
            int stopTurnLineIndex = refClosePt.turnLineIndex - a;
            if (stopTurnLineIndex < 0) stopTurnLineIndex = mf.bnd.bndList[turnLineNum].turnLine.Count - 3;
            if (stopTurnLineIndex > mf.bnd.bndList[turnLineNum].turnLine.Count - 1) turnLineIndex = 3;

            for (; turnLineIndex != stopTurnLineIndex; turnLineIndex += a)
            {
                if (turnLineIndex < 0) turnLineIndex = mf.bnd.bndList[turnLineNum].turnLine.Count - 2;
                if (turnLineIndex > mf.bnd.bndList[turnLineNum].turnLine.Count - 2) turnLineIndex = 0;

                if (GetLineIntersection(
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex],
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1],

                                fromPt, toPt, out iE, out iN))
                {
                    closestTurnPt = new CClose();
                    closestTurnPt.closePt.easting = iE;
                    closestTurnPt.closePt.northing = iN;
                    closestTurnPt.closePt.heading = -1; //isnt needed but could be calculated
                    closestTurnPt.turnLineIndex = turnLineIndex;
                    closestTurnPt.curveIndex = -1;
                    closestTurnPt.time = mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].heading;
                    closestTurnPt.turnLineNum = turnLineNum;
                    return true;
                }
            }
            return false;
        }

        #endregion FindTurnPoint

        #region SequenceLines

        //TODO: is for some reason making longer for omegaturn....
        private bool AddCurveSequenceLines(double lenny)
        {
            //how many points striaght out
            bool sameWay = mf.trk.isHeadingSameWay;
            int a = sameWay ? -1 : 1;

            for (int i = 0; i < lenny && i > -lenny; i += a)
            {
                ytList.Insert(0, new vec3(mf.trk.currentGuidanceTrack[inClosestTurnPt.curveIndex]));
                inClosestTurnPt.curveIndex += a;
                if (inClosestTurnPt.curveIndex < 2 || inClosestTurnPt.curveIndex > mf.trk.currentGuidanceTrack.Count - 3)
                {
                    FailCreate();
                    return false;
                }
            }
            if (isGoingStraightThrough) sameWay = !sameWay;
            a = sameWay ? -1 : 1;

            for (int i = 0; i < lenny && i > -lenny; i += a)
            {
                ytList.Add(new vec3(nextCurve[outClosestTurnPt.curveIndex]));
                outClosestTurnPt.curveIndex += a;
                if (outClosestTurnPt.curveIndex < 2 || outClosestTurnPt.curveIndex > mf.trk.currentGuidanceTrack.Count - 3)
                {
                    FailCreate();
                    return false;
                }
            }

            return true;
        }

        #endregion SequenceLines

        /// <summary>/// Calculates the crosing point between the two lines and returns the easting and northing in the reference vaiables
        /// returns 0 for no collision and 1 for collision /// </summary>
        public bool GetLineIntersection(vec3 p0, vec3 p1, vec3 p2, vec3 p3, out double iEast, out double iNorth)
        {
            double dx1 = p1.northing - p0.northing;
            double dy1 = p1.easting - p0.easting;

            double dx2 = p3.northing - p2.northing;
            double dy2 = p3.easting - p2.easting;

            double s, t;
            s = (-dy1 * (p0.northing - p2.northing) + dx1 * (p0.easting - p2.easting)) / (-dx2 * dy1 + dx1 * dy2);

            if (s >= 0 && s <= 1)
            {
                //check oher side
                t = (dx2 * (p0.easting - p2.easting) - dy2 * (p0.northing - p2.northing)) / (-dx2 * dy1 + dx1 * dy2);
                if (t >= 0 && t <= 1)
                {
                    // Collision detected
                    iNorth = p0.northing + (t * dx1);
                    iEast = p0.easting + (t * dy1);
                    return true;
                }
            }

            iEast = 0; iNorth = 0;
            return false; // No collision
        }


        private const double Epsilon = 1.0E-15;
        public static bool GetLineIntersection(vec3 PointAA, vec3 PointAB, vec3 PointBA, vec3 PointBB, out vec3 Crossing, out double TimeA, out double TimeB, bool Limit = false, bool enableEnd = false)
        {
            TimeA = -1;
            TimeB = -1;
            Crossing = new vec3();
            double denominator = (PointAB.northing - PointAA.northing) * (PointBB.easting - PointBA.easting) - (PointBB.northing - PointBA.northing) * (PointAB.easting - PointAA.easting);

            if (denominator < -0.00000001 || denominator > 0.00000001)
            {
                TimeA = ((PointBB.northing - PointBA.northing) * (PointAA.easting - PointBA.easting) - (PointAA.northing - PointBA.northing) * (PointBB.easting - PointBA.easting)) / denominator;

                if (Limit || (enableEnd && (TimeA > 0.0 - Epsilon || TimeA < 1.0 + Epsilon)) || (TimeA > Epsilon && TimeA < 1.0 - Epsilon))
                {
                    TimeB = ((PointAB.northing - PointAA.northing) * (PointAA.easting - PointBA.easting) - (PointAA.northing - PointBA.northing) * (PointAB.easting - PointAA.easting)) / denominator;
                    if (Limit || (enableEnd && (TimeB == 0.0 || TimeB == 1.0)) || (TimeB > 0.0 && TimeB < 1.0))
                    {
                        Crossing = PointAA + (PointAB - PointAA) * TimeA;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            return false;
        }

        private CClose MoveTurnLine(List<vec3> curList, CClose startPoint, double stepSize, bool CountUp, bool deleteSecondHalf, bool loop)
        {
            int A = startPoint.curveIndex;
            int B = (startPoint.curveIndex + 1) % curList.Count;

            var goalPoint = startPoint;

            if (A == 0 && !CountUp)
            {
                double head = Math.Atan2(curList[B].easting - curList[A].easting, curList[B].northing - curList[A].northing);
                goalPoint.closePt.easting = startPoint.closePt.easting - (Math.Sin(head) * stepSize);
                goalPoint.closePt.northing = startPoint.closePt.northing - (Math.Cos(head) * stepSize);
            }
            else if (B == curList.Count - 1 && CountUp)
            {
                double head = Math.Atan2(curList[B].easting - curList[A].easting, curList[B].northing - curList[A].northing);
                goalPoint.closePt.easting = startPoint.closePt.easting + (Math.Sin(head) * stepSize);
                goalPoint.closePt.northing = startPoint.closePt.northing + (Math.Cos(head) * stepSize);
            }
            else
            {
                int count = CountUp ? 1 : -1;
                double distSoFar = 0;

                vec3 start = startPoint.closePt;

                for (int i = CountUp ? B : A; i < curList.Count && i >= 0;)
                {
                    // used for calculating the length squared of next segment.
                    double tempDist = glm.Distance(start, curList[i]);

                    //will we go too far?
                    if ((tempDist + distSoFar) > stepSize)
                    {
                        double j = (stepSize - distSoFar) / tempDist; // the remainder to yet travel

                        goalPoint.closePt.easting = (((1 - j) * start.easting) + (j * curList[i].easting));
                        goalPoint.closePt.northing = (((1 - j) * start.northing) + (j * curList[i].northing));

                        break;
                    }
                    else distSoFar += tempDist;
                    start = curList[i];

                    i += count;

                    if (i < 0)
                    {
                        if (!loop)
                        {
                            double j = stepSize - distSoFar;
                            double head = Math.Atan2(curList[i + 2].easting - curList[i + 1].easting, curList[i + 2].northing - curList[i + 1].northing);
                            goalPoint.closePt.northing = start.northing - (Math.Cos(head) * j);
                            goalPoint.closePt.easting = start.easting - (Math.Sin(head) * j);
                            break;
                        }
                        else
                            i = curList.Count - 1;
                    }
                    if (i > curList.Count - 1)
                    {
                        if (!loop)
                        {
                            double j = stepSize - distSoFar;
                            double head = Math.Atan2(curList[i - 1].easting - curList[i - 2].easting, curList[i - 1].northing - curList[i - 2].northing);
                            goalPoint.closePt.northing = start.northing + (Math.Cos(head) * j);
                            goalPoint.closePt.easting = start.easting + (Math.Sin(head) * j);

                            break;
                        }
                        else
                            i = 0;
                    }
                    startPoint.curveIndex = CountUp ? i - 1 : i;
                }
            }
            return goalPoint;
        }

        public void SmoothYouTurn(int smPts)
        {
            //countExit the reference list of original curve
            int cnt = ytList.Count;

            //the temp array
            vec3[] arr = new vec3[cnt];

            //read the points before and after the setpoint
            for (int s = 0; s < smPts / 2; s++)
            {
                arr[s].easting = ytList[s].easting;
                arr[s].northing = ytList[s].northing;
                arr[s].heading = ytList[s].heading;
            }

            for (int s = cnt - (smPts / 2); s < cnt; s++)
            {
                arr[s].easting = ytList[s].easting;
                arr[s].northing = ytList[s].northing;
                arr[s].heading = ytList[s].heading;
            }

            //average them - center weighted average
            for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
            {
                for (int j = -smPts / 2; j < smPts / 2; j++)
                {
                    arr[i].easting += ytList[j + i].easting;
                    arr[i].northing += ytList[j + i].northing;
                }
                arr[i].easting /= smPts;
                arr[i].northing /= smPts;
                arr[i].heading = ytList[i].heading;
            }

            ytList?.Clear();

            //calculate new headings on smoothed line
            for (int i = 1; i < cnt - 1; i++)
            {
                arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                ytList.Add(arr[i]);
            }
        }

        //called to initiate turn
        public void YouTurnTrigger()
        {
            //trigger pulled
            isYouTurnTriggered = true;

            if (!isGoingStraightThrough)
            {
                mf.trk.howManyPathsAway += (isTurnLeft ^ mf.trk.isHeadingSameWay) ? rowSkipsWidth : -rowSkipsWidth;
                mf.trk.isHeadingSameWay = !mf.trk.isHeadingSameWay;

                if (alternateSkips && rowSkipsWidth2 > 1)
                {
                    if (--turnSkips == 0)
                    {
                        isTurnLeft = !isTurnLeft;
                        turnSkips = rowSkipsWidth2 * 2 - 1;
                    }
                    else if (previousBigSkip = !previousBigSkip)
                        rowSkipsWidth = rowSkipsWidth2 - 1;
                    else
                        rowSkipsWidth = rowSkipsWidth2;
                }
                else isTurnLeft = !isTurnLeft;
            }
        }

        //Normal copmpletion of youturn
        public void CompleteYouTurn()
        {
            isYouTurnTriggered = false;
            ResetCreatedYouTurn();
        }

        public void Set_Alternate_skips()
        {
            rowSkipsWidth2 = rowSkipsWidth;
            turnSkips = rowSkipsWidth2 * 2 - 1;
            previousBigSkip = false;
        }

        //something went seriously wrong so reset everything
        public void ResetYouTurn()
        {
            turnTooCloseTrigger = false;
            isTurnCreationTooClose = false;
            isTurnCreationNotCrossingError = false;
            ResetCreatedYouTurn();
        }

        public void ResetCreatedYouTurn()
        {
            mf.sounds.isBoundAlarming = false;
            isYouTurnTriggered = false;
            youTurnPhase = 0;
            ytList?.Clear();
            PGN_239.pgn[PGN_239.uturn] = 0;
            isGoingStraightThrough = false;
        }

        public void FailCreate()
        {
            //fail
            isOutOfBounds = true;
            isTurnCreationTooClose = true;
            mf.mc.isOutOfBounds = true;
            youTurnPhase = 250;//error
        }

        public void BuildManualYouLateral(bool isTurnLeft)
        {
            //point on AB line closest to pivot axle point from AB Line PurePursuit
            if (mf.trk.idx > -1 && mf.trk.gArr.Count > 0)
            {
                mf.trk.howManyPathsAway += mf.trk.isHeadingSameWay == isTurnLeft ? 1 : -1;
            }
        }

        //build the points and path of youturn to be scaled and transformed
        public void BuildManualYouTurn(bool isTurnRight)
        {
            double head;
            head = mf.gyd.manualUturnHeading;

            //grab the vehicle widths and offsets
            double turnOffset = (Settings.Tool.toolWidth - Settings.Tool.maxOverlap) * rowSkipsWidth + (isTurnRight ? Settings.Tool.offset * 2.0 : -Settings.Tool.offset * 2.0);

            //if its straight across it makes 2 loops instead so goal is a little lower then start
            if (!mf.trk.isHeadingSameWay) head += Math.PI;

            //move the start forward 2 meters, this point is critical to formation of uturn
            double rEastYT = mf.gyd.rEastTrk + Math.Sin(head) * (4 + Settings.Vehicle.set_youTurnExtensionLength);
            double rNorthYT = mf.gyd.rNorthTrk + Math.Cos(head) * (4 + Settings.Vehicle.set_youTurnExtensionLength);

            //now we have our start point
            vec3 start = new vec3(rEastYT, rNorthYT, head);
            vec3 goal = new vec3();

            if (isTurnRight)
            {
                goal.easting = start.easting + (Math.Cos(head) * turnOffset);
                goal.northing = start.northing + (Math.Sin(head) * turnOffset);
            }
            else
            {
                goal.easting = start.easting - (Math.Cos(head) * turnOffset);
                goal.northing = start.northing - (Math.Sin(head) * turnOffset);
            }

            //generate the turn points
            double extraSagitta = 0;
            if (Math.Abs(turnOffset) < youTurnRadius * 2)
                extraSagitta = (youTurnRadius * 2 - Math.Abs(turnOffset)) * 0.5;

            ytList = GetOffsetSemicirclePoints(start, head, isTurnRight, youTurnRadius, extraSagitta, glm.PIBy2);
            ytList2 = GetOffsetSemicirclePoints(goal, head, !isTurnRight, youTurnRadius, extraSagitta, glm.PIBy2);

            ytList2.Reverse();
            ytList.AddRange(ytList2);

            ytList.Insert(0, new vec3(start.easting - Math.Sin(head) * Settings.Vehicle.set_youTurnExtensionLength, start.northing - Math.Cos(head) * Settings.Vehicle.set_youTurnExtensionLength, 0));
            ytList.Add(new vec3(goal.easting - Math.Sin(head) * Settings.Vehicle.set_youTurnExtensionLength, goal.northing - Math.Cos(head) * Settings.Vehicle.set_youTurnExtensionLength, 0));

            isTurnLeft = !isTurnRight;
            YouTurnTrigger();
        }

        //Duh.... What does this do....
        public void DrawYouTurn()
        {
            if (ytList.Count < 3) return;

            GL.PointSize(Settings.User.setDisplay_lineWidth + 2);

            if (youTurnPhase < 130)
                GL.Color3(1.0f, 1.0f, 0.0f);
            else if (isYouTurnTriggered)
                GL.Color3(0.95f, 0.5f, 0.95f);
            else if (isOutOfBounds)
                GL.Color3(0.9495f, 0.395f, 0.325f);
            else
                GL.Color3(0.395f, 0.925f, 0.30f);

            GL.Begin(PrimitiveType.Points);
            for (int i = 0; i < ytList.Count; i++)
            {
                GL.Vertex3(ytList[i].easting, ytList[i].northing, 0);
            }
            GL.End();

            if (youTurnPhase > 70 && youTurnPhase < 130)
            {
                //GL.Color3(1.0f, 1.0f, 0.0f);
                GL.Begin(PrimitiveType.Points);
                for (int i = 0; i < ytList2.Count; i++)
                {
                    GL.Vertex3(ytList2[i].easting, ytList2[i].northing, 0);
                }
                GL.End();
            }

            /*
            GL.Color3(0.0f, 1.0f, 1.0f);
            GL.Begin(PrimitiveType.Points);
            for (int i = 0; i < nextCurve.Count; i++)
            {
                GL.Vertex3(nextCurve[i].easting, nextCurve[i].northing, 0);
            }
            GL.End();
            */

            //GL.PointSize(12.0f);
            //GL.Begin(PrimitiveType.Points);
            //GL.Color3(0.95f, 0.73f, 1.0f);
            //GL.Vertex3(inClosestTurnPt.closePt.easting, inClosestTurnPt.closePt.northing, 0);
            //GL.Color3(0.395f, 0.925f, 0.30f);
            //GL.Vertex3(outClosestTurnPt.closePt.easting, outClosestTurnPt.closePt.northing, 0);
            //GL.End();
            //GL.PointSize(1.0f);

            //if (nextCurve != null)
            //{
            //    GL.Begin(PrimitiveType.Points);
            //    GL.Color3(0.95f, 0.41f, 0.980f);
            //    for (int i = 0; i < nextCurve.currentGuidanceTrack.Count; i++)
            //    {
            //        GL.Vertex3(nextCurve.currentGuidanceTrack[i].easting, nextCurve.currentGuidanceTrack[i].northing, 0);
            //    }
            //    GL.End();
            //}

            //if (ytList2?.Count > 0)
            //{
            //    GL.PointSize(Settings.User.setDisplay_lineWidth + 2);
            //    GL.Color3(0.3f, 0.941f, 0.980f);
            //    GL.Begin(PrimitiveType.Points);
            //    for (int i = 0; i < ytList2.Count; i++)
            //    {
            //        GL.Vertex3(ytList2[i].easting, ytList2[i].northing, 0);
            //    }
            //    GL.End();
            //}
        }

        public class CClose
        {
            public vec3 closePt = new vec3();
            public int turnLineNum;
            public int turnLineIndex;
            public double time;
            public int curveIndex;

            public CClose()
            {
                closePt = new vec3();
                turnLineNum = -1;
                turnLineIndex = -1;
                time = double.MaxValue;
                curveIndex = -1;
            }

            public CClose(vec3 crossing, double _time, int j, int i, int k)
            {
                closePt = crossing;
                time = _time;
                turnLineIndex = k;
                turnLineNum = j;
                curveIndex = i;
            }

            public CClose(CClose _clo)
            {
                closePt = new vec3(_clo.closePt);
                turnLineNum = _clo.turnLineNum;
                turnLineIndex = _clo.turnLineIndex;
                time = _clo.time;
                curveIndex = _clo.curveIndex;
            }

            public override string ToString()
            {
                return "east:" + closePt.easting.ToString("0.0") + ", north:" + closePt.northing.ToString("0.0") + ", time:" + time.ToString("0.0");
            }
        }
    }
}