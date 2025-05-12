using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AOG
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

        public double youTurnRadius, totalUTurnLength;

        public int rowSkipsWidth = 1;

        public bool alternateSkips = false, previousBigSkip = true;
        public int rowSkipsWidth2 = 3, turnSkips = 2;

        /// <summary>  /// distance from headland as offset where to start turn shape /// </summary>
        public int youTurnStartOffset;

        //guidance values
        public double uturnDistanceFromBoundary;

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

        CClose movePoint = new CClose();
        CClose movePoint2 = new CClose();

        CClose exitPoint = new CClose();
        CClose exitPoint2 = new CClose();
        CClose entryPoint = new CClose();
        CClose entryPoint2 = new CClose();


        public CClose startOfTurnPt = new CClose();

        public double onA;

        #endregion Fields

        //constructor
        public CYouTurn(FormGPS _f)
        {
            mf = _f;
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
        }


        #region CreateTurn
        //Finds the point where an AB Curve crosses the turn line
        public void BuildCurveDubinsYouTurn()
        {
            double turnOffset = (Settings.Tool.toolWidth - Settings.Tool.overlap) * rowSkipsWidth + (isTurnLeft ? -Settings.Tool.offset * 2.0 : Settings.Tool.offset * 2.0);
            bool isTurnRight = turnOffset > 0 ^ isTurnLeft;


            CTrk track = mf.trk.currTrk;

            bool loop = track.mode == TrackMode.bndCurve || track.mode == TrackMode.waterPivot;


            if (youTurnPhase < 10)
            {
                youTurnPhase++;
                if (youTurnPhase == 10)
                    youTurnPhase = 10;
            }
            else if (youTurnPhase == 10)
            {
                #region FindExitPoint
                ytList.Clear();

                bool Loop = true;
                int Count = mf.trk.isHeadingSameWay ? 1 : -1;
                exitPoint = new CClose();

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

                    double time = 2;

                    End = new vec3(mf.trk.currentGuidanceTrack[i].easting, mf.trk.currentGuidanceTrack[i].northing);
                    for (int j = 0; j < mf.bnd.bndList.Count; j++)
                    {
                        if (mf.bnd.bndList[j].isDriveThru) continue;

                        int k = mf.bnd.bndList[j].turnLine.Count - 1;
                        for (int l = 0; l < mf.bnd.bndList[j].turnLine.Count; l++)
                        {
                            if (glm.GetLineIntersection(Start, End, mf.bnd.bndList[j].turnLine[k], mf.bnd.bndList[j].turnLine[l], out vec3 _Crossing, out double timeA, out _))
                            {
                                if (timeA < time)
                                {
                                    time = timeA;
                                    exitPoint = new CClose(_Crossing, j, i - (mf.trk.isHeadingSameWay ? Count : 0), k);
                                }
                            }
                            k = l;
                        }
                    }

                    if (exitPoint.turnLineNum >= 0)
                    {
                        movePoint = new CClose(exitPoint);
                        break;
                    }
                    Start = End;
                }

                if (exitPoint.turnLineNum == -1)//didnt hit any turn line
                {
                    if (track.mode == TrackMode.waterPivot || track.mode == TrackMode.bndCurve)
                    {
                        youTurnPhase = 251;//ignore
                    }
                    else//curve does not cross a boundary - oops
                    {
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

                movePoint = MoveTurnLine(mf.trk.currentGuidanceTrack, movePoint, step, mf.trk.isHeadingSameWay ^ youTurnPhase % 20 == 0, loop);

                // creates half a circle starting at the crossing point
                double extraSagitta = 0;
                if (Math.Abs(turnOffset) < youTurnRadius * 2)
                    extraSagitta = (youTurnRadius * 2 - Math.Abs(turnOffset)) * 0.5;

                int A = movePoint.curveIndex;
                double head = Math.Atan2(mf.trk.currentGuidanceTrack[A + 1].easting - mf.trk.currentGuidanceTrack[A].easting, mf.trk.currentGuidanceTrack[A + 1].northing - mf.trk.currentGuidanceTrack[A].northing);

                ytList = GetOffsetSemicirclePoints(movePoint.closePt, head + (mf.trk.isHeadingSameWay ? 0 : Math.PI), isTurnRight, youTurnRadius, extraSagitta, uTurnStyle == 1 ? 2.2 : Math.PI);

                mf.distancePivotToTurnLine = glm.Distance(ytList[0], mf.pivotAxlePos);

                if (mf.distancePivotToTurnLine < Settings.Vehicle.set_youTurnExtensionLength + 1)
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
                if (ytList.Count > 1)
                {
                    var pt3 = ytList[ytList.Count - 1];
                    pt3.heading = Math.Atan2(ytList[ytList.Count - 1].easting - ytList[ytList.Count - 2].easting,
                         ytList[ytList.Count - 1].northing - ytList[ytList.Count - 2].northing);
                    if (pt3.heading < 0) pt3.heading += glm.twoPI;
                    ytList[ytList.Count - 1] = pt3;

                    mf.trk.AddEndPoints(ref ytList, 50);
                }

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
                        if (glm.GetLineIntersection(turnLine[j], turnLine[k], ytList[i - 1], ytList[i], out vec3 _crossing, out _, out _))
                        {
                            found = true;
                            ytList.RemoveRange(i, ytList.Count - i);
                            exitPoint2 = new CClose(_crossing, movePoint.turnLineNum, 0, j);
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
                double widthMinusOverlap = Settings.Tool.toolWidth - Settings.Tool.overlap;

                double distAway = widthMinusOverlap * (mf.trk.howManyPathsAway + (isTurnLeft ^ mf.trk.isHeadingSameWay ? rowSkipsWidth : -rowSkipsWidth)) + (mf.trk.isHeadingSameWay ? Settings.Tool.offset : -Settings.Tool.offset) + track.nudgeDistance;

                distAway += 0.5 * widthMinusOverlap;

                //create the next line
                nextCurve = mf.trk.BuildCurrentGuidanceTrack(distAway, track);


                bool isTurnLineSameWay = !isTurnRight ^ movePoint.turnLineNum == 0;
                if (!FindCurveOutTurnPoint(mf.trk, exitPoint, isTurnLineSameWay))
                {
                    //error
                    FailCreate();
                }
                else
                {
                    youTurnPhase += 10;
                    movePoint2 = new CClose(entryPoint);
                }
            }
            else if (youTurnPhase < 120)//step 2 move the turn inside with steps of 1 meter
            {
                double step = youTurnPhase == 80 ? 5.0 : youTurnPhase == 90 ? 1.0 : youTurnPhase == 100 ? 0.2 : 0.04;

                movePoint2 = MoveTurnLine(nextCurve, movePoint2, step, mf.trk.isHeadingSameWay ^ isGoingStraightThrough ^ youTurnPhase % 20 == 0, loop);

                // creates half a circle starting at the crossing point
                double extraSagitta = 0;
                if (Math.Abs(turnOffset) < youTurnRadius * 2)
                    extraSagitta = (youTurnRadius * 2 - Math.Abs(turnOffset)) * 0.5;

                int A = movePoint2.curveIndex;
                double head = Math.Atan2(nextCurve[A + 1].easting - nextCurve[A].easting, nextCurve[A + 1].northing - nextCurve[A].northing);

                ytList2 = GetOffsetSemicirclePoints(movePoint2.closePt, head + (mf.trk.isHeadingSameWay ^ isGoingStraightThrough ? 0 : Math.PI), !isTurnRight, youTurnRadius, extraSagitta, uTurnStyle == 1 ? 2.2 : Math.PI);

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
                var turnLine = mf.bnd.bndList[movePoint2.turnLineNum].turnLine;
                //Are we out of bounds?
                for (int i = 1; i < ytList2.Count; i++)
                {
                    int j = turnLine.Count - 1;
                    for (int k = 0; k < turnLine.Count; j = k++)
                    {
                        if (glm.GetLineIntersection(turnLine[j], turnLine[k], ytList2[i - 1], ytList2[i], out vec3 _crossing, out _, out _))
                        {
                            found = true;
                            ytList2.RemoveRange(i, ytList2.Count - i);
                            entryPoint2 = new CClose(_crossing, movePoint2.turnLineNum, 0, j);
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
            else if (youTurnPhase == 130)//join the two halves
            {
                if (exitPoint2.turnLineIndex != entryPoint2.turnLineIndex)
                {
                    var turnLine = mf.bnd.bndList[movePoint2.turnLineNum].turnLine;
                    bool isTurnLineSameWay = isTurnRight ^ movePoint2.turnLineNum != 0;
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

                int upDownCount = mf.trk.isHeadingSameWay ? -1 : 1;

                int currIdx = movePoint.curveIndex + (mf.trk.isHeadingSameWay ? 0 : 1);
                var curList = mf.trk.currentGuidanceTrack;

                AddCurveSequenceLines(curList, ytList[0], currIdx, upDownCount, Settings.Vehicle.set_youTurnExtensionLength, true);


                int offsetIdx = movePoint2.curveIndex + (mf.trk.isHeadingSameWay ^ isGoingStraightThrough ? 0 : + 1);
                upDownCount = mf.trk.isHeadingSameWay ^ isGoingStraightThrough ? -1 : 1;

                AddCurveSequenceLines(nextCurve, ytList[ytList.Count - 1], offsetIdx, upDownCount, Settings.Vehicle.set_youTurnExtensionLength, false);

                isOutOfBounds = false;
                youTurnPhase = 240;
            }
            else if (youTurnPhase == 240)
            {
                //if (uTurnSmoothing > 0)
                //    SmoothYouTurn(6);// uTurnSmoothing????
                youTurnPhase = 255;
            }
        }

        #endregion CreateTurn

        private List<vec3> GetOffsetSemicirclePoints(vec3 currentPos, double theta, bool isTurningRight, double turningRadius, double offsetDistance, double angle = Math.PI)
        {
            List<vec3> points = new List<vec3>() { currentPos };

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

        public bool FindCurveOutTurnPoint(CTracks thisCurve, CClose inPt, bool isTurnLineCW)
        {
            int a = isTurnLineCW ? 1 : -1;

            int turnLineNum = inPt.turnLineNum;
            var turnLine = mf.bnd.bndList[turnLineNum].turnLine;
            int stopTurnLineIndex = isTurnLineCW ? inPt.turnLineIndex : inPt.turnLineIndex + 1;
            vec3 from = new vec3(inPt.closePt);
            entryPoint = new CClose();

            bool first = true;
            for (int turnLineIndex = isTurnLineCW ? inPt.turnLineIndex + 1 : inPt.turnLineIndex; turnLineIndex != stopTurnLineIndex; turnLineIndex += a)
            {
                if (turnLineIndex < 0) turnLineIndex = turnLine.Count - 1; //AAA could be less than 0???
                if (turnLineIndex >= turnLine.Count) turnLineIndex = 0;


                double time = 2;

                for (int i = 0; i < nextCurve.Count - 1; i++)
                {
                    if (glm.GetLineIntersection(from, turnLine[turnLineIndex], nextCurve[i], nextCurve[i + 1], out vec3 _crossing, out double time_, out _))
                    {
                        if (time_ < time)
                        {
                            time = time_;
                            entryPoint.closePt = _crossing;
                            entryPoint.turnLineIndex = turnLineIndex;
                            entryPoint.curveIndex = i;
                            entryPoint.turnLineNum = turnLineNum;
                            isGoingStraightThrough = false;
                        }
                    }
                }
                
                for (int i = 0; i < thisCurve.currentGuidanceTrack.Count - 1; i++)
                {
                    if (glm.GetLineIntersection(from, turnLine[turnLineIndex], thisCurve.currentGuidanceTrack[i], thisCurve.currentGuidanceTrack[i + 1], out vec3 _crossing, out double time_, out double time2))
                    {
                        if ((i + time2 < mf.gyd.rTimeTrk && thisCurve.isHeadingSameWay) || (i + time2 > mf.gyd.rTimeTrk && !thisCurve.isHeadingSameWay))
                        {
                            return false; //hitting the curve behind us
                        }
                        else if ((!first || time_ > 0.00001) && time_ < time)
                        {
                            time = time_;
                            entryPoint.closePt = _crossing;
                            entryPoint.turnLineIndex = turnLineIndex;
                            entryPoint.curveIndex = i;
                            entryPoint.turnLineNum = turnLineNum;
                            isGoingStraightThrough = true;
                        }
                    }
                }
                first = false;
                if (time <= 1)
                {
                    if (isGoingStraightThrough)
                        nextCurve = thisCurve.currentGuidanceTrack;//???? already created the line so why change it
                    return true;
                }

                from = turnLine[turnLineIndex];
            }
            return false;
        }

        #endregion FindTurnPoint

        #region SequenceLines

        private bool AddCurveSequenceLines(List<vec3> points, vec3 point, int idx, int upDownCount, double Length, bool insert)
        {
            double distSoFar = 0;
            //cycle thru segments and keep adding lengths. check if start and break if so.
            while (true)
            {
                if (idx == -1 || idx == points.Count)
                {
                    FailCreate();
                    return false;
                }

                double dx1 = point.northing - points[idx].northing;
                double dy1 = point.easting - points[idx].easting;
                double length1 = Math.Sqrt(dx1 * dx1 + dy1 * dy1);

                //will we go too far?
                if ((length1 + distSoFar) > Length)
                {
                    double factor = (Length - distSoFar) / length1;
                    if (insert)
                        ytList.Insert(0, new vec3(point.easting - dy1 * factor, point.northing - dx1 * factor));
                    else 
                        ytList.Add(new vec3(point.easting - dy1 * factor, point.northing - dx1 * factor));
                    return true; //tempDist contains the full length of next segment
                }
                distSoFar += length1;
                if (insert)
                    ytList.Insert(0, new vec3(points[idx].easting, points[idx].northing));
                else
                    ytList.Add(new vec3(points[idx].easting, points[idx].northing));

                point = new vec3(points[idx].easting, points[idx].northing);
                idx += upDownCount;
            }
        }

        #endregion SequenceLines
        private CClose MoveTurnLine(List<vec3> curList, CClose startPoint, double stepSize, bool CountUp, bool loop)
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
            for (int i = 0; i < cnt - 1; i++)
            {
                arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                ytList.Add(arr[i]);
            }

            ytList.Add(arr[cnt - 1]);
        }

        //called to initiate turn
        public void YouTurnTrigger()
        {
            //trigger pulled
            isYouTurnTriggered = true;
            totalUTurnLength = 0;
            for (int k = 0; k < ytList.Count - 1; k++)
            {
                totalUTurnLength += glm.Distance(ytList[k], ytList[k + 1]);
            }
        }

        public void NextPath()
        {
            isGoingStraightThrough = true;

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

        //Normal copmpletion of youturn
        public void CompleteYouTurn()
        {
            ResetCreatedYouTurn();
        }

        public void Set_Alternate_skips()
        {
            rowSkipsWidth2 = rowSkipsWidth;
            turnSkips = rowSkipsWidth2 * 2 - 1;
            previousBigSkip = false;
        }

        public void ResetCreatedYouTurn()
        {
            mf.sounds.isBoundAlarming = false;
            isYouTurnTriggered = false;
            youTurnPhase = 0;
            ytList?.Clear();
            PGN_239.pgn[PGN_239.uturn] = 0;
            isGoingStraightThrough = false;
            nextCurve = new List<vec3>();
        }

        public void FailCreate()
        {
            //fail
            ytList?.Clear();
            isOutOfBounds = true;
            mf.mc.isOutOfBounds = true;
            youTurnPhase = 250;//error

            Log.EventWriter("U Turn Creation Failure");

            if (Settings.User.sound_isUturnOn)
                mf.sounds.sndUTurnTooClose.Play();
        }

        public void BuildManualYouLateral(bool isTurnLeft)
        {
            //point on AB line closest to pivot axle point from AB Line PurePursuit
            mf.trk.howManyPathsAway += mf.trk.isHeadingSameWay == isTurnLeft ? 1 : -1;
        }

        //build the points and path of youturn to be scaled and transformed
        public void BuildManualYouTurn(bool isTurnRight)
        {
            double head;
            head = mf.gyd.manualUturnHeading;

            //grab the vehicle widths and offsets
            double turnOffset = (Settings.Tool.toolWidth - Settings.Tool.overlap) * rowSkipsWidth + (isTurnRight ? Settings.Tool.offset * 2.0 : -Settings.Tool.offset * 2.0);

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
                goal.northing = start.northing - (Math.Sin(head) * turnOffset);
            }
            else
            {
                goal.easting = start.easting - (Math.Cos(head) * turnOffset);
                goal.northing = start.northing + (Math.Sin(head) * turnOffset);
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
            if (!isGoingStraightThrough && youTurnPhase > 70)
            {
                GL.LineWidth(Settings.User.setDisplay_lineWidth * 4);
                GL.Color3(0, 0, 0);
                nextCurve.DrawPolygon(PrimitiveType.LineStrip);

                GL.LineWidth(Settings.User.setDisplay_lineWidth);
                GL.Color3(0.0f, 1.0f, 1.0f);
                nextCurve.DrawPolygon(PrimitiveType.LineStrip);
            }


            GL.PointSize(Settings.User.setDisplay_lineWidth + 2);

            if (youTurnPhase < 130)
                GL.Color3(1.0f, 1.0f, 0.0f);
            else if (isYouTurnTriggered)
                GL.Color3(0.95f, 0.5f, 0.95f);
            else if (isOutOfBounds)
                GL.Color3(0.9495f, 0.395f, 0.325f);
            else
                GL.Color3(0.395f, 0.925f, 0.30f);

            ytList.DrawPolygon(PrimitiveType.LineStrip);

            if (youTurnPhase > 70 && youTurnPhase < 130)
            {
                ytList2.DrawPolygon(PrimitiveType.LineStrip);
            }

            //GL.PointSize(12.0f);
            //GL.Begin(PrimitiveType.Points);
            //GL.Color3(0.95f, 0.73f, 1.0f);
            //GL.Vertex3(inClosestTurnPt.closePt.easting, inClosestTurnPt.closePt.northing, 0);
            //GL.Color3(0.395f, 0.925f, 0.30f);
            //GL.Vertex3(outClosestTurnPt.closePt.easting, outClosestTurnPt.closePt.northing, 0);
            //GL.End();
            //GL.PointSize(1.0f);
        }

        [System.Diagnostics.DebuggerDisplay("{ToString()}")]
        public class CClose
        {
            public vec3 closePt = new vec3();
            public int turnLineNum;
            public int turnLineIndex;
            public int curveIndex;

            public CClose()
            {
                closePt = new vec3();
                turnLineNum = -1;
                turnLineIndex = -1;
                curveIndex = -1;
            }

            public CClose(vec3 crossing, int j, int i, int k)
            {
                closePt = crossing;
                turnLineIndex = k;
                turnLineNum = j;
                curveIndex = i;
            }

            public CClose(CClose _clo)
            {
                closePt = new vec3(_clo.closePt);
                turnLineNum = _clo.turnLineNum;
                turnLineIndex = _clo.turnLineIndex;
                curveIndex = _clo.curveIndex;
            }

            public override string ToString()
            {
                return closePt.ToString();
            }
        }
    }
}