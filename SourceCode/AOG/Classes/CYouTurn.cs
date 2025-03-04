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
        public bool isYouTurnTriggered, isGoingStraightThrough = false;

        /// <summary>  /// turning right or left?/// </summary>
        public bool isTurnLeft;

        private int semiCircleIndex = -1;

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

        private bool isHeadingSameWay = true;
        public bool isTurnCreationTooClose = false, isTurnCreationNotCrossingError = false, turnTooCloseTrigger = false;

        //list of points for scaled and rotated YouTurn line, used for pattern, dubins, abcurve
        public List<vec3> ytList = new List<vec3>();

        private List<vec3> ytList2 = new List<vec3>();

        //next curve or line to build out turn and point over
        public List<vec3> nextCurve = new List<vec3>();

        //if we continue on the same line or change to the next one after the uTurn
        public bool isOutSameCurve;

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
        public CClose startOfTurnPt = new CClose();

        //how far should the distance between points on the uTurn be
        private double pointSpacing;
        public int onA;

        #endregion Fields

        //constructor
        public CYouTurn(FormGPS _f)
        {
            mf = _f;

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

        //Finds the point where an AB Curve crosses the turn line
        public bool BuildCurveDubinsYouTurn()
        {
            //TODO: is calculated many taimes after the priveous turn is complete
            //grab the vehicle widths and offsets
            double turnOffset = (mf.tool.width - mf.tool.overlap) * rowSkipsWidth + (isTurnLeft ? -mf.tool.offset * 2.0 : mf.tool.offset * 2.0);
            pointSpacing = youTurnRadius * 0.1;

            if (uTurnStyle == 0)
            {
                //Albin turn
                if (turnOffset > (youTurnRadius * 2.0))
                {
                    return CreateCurveWideTurn();
                }

                //Ohmega turn
                else
                {
                    return CreateCurveOmegaTurn();
                }
            }
            else if (uTurnStyle == 1)
            {
                return (KStyleTurnCurve());
            }

            //prgramming error if you got here
            return false;
        }

        #region CreateTurn

        private bool CreateCurveOmegaTurn()
        {
            //keep from making turns constantly - wait 1.5 seconds
            if (mf.makeUTurnCounter < 4)
            {
                youTurnPhase = 0;
                return true;
            }

            if (mf.trk.idx < 0 || mf.trk.gArr.Count < mf.trk.idx) return true;
            CTrk track = mf.trk.gArr[mf.trk.idx];

            //grab the vehicle widths and offsets
            double turnOffset = (mf.tool.width - mf.tool.overlap) * rowSkipsWidth + (isTurnLeft ? -mf.tool.offset * 2.0 : mf.tool.offset * 2.0);

            switch (youTurnPhase)
            {
                case 0: //find the crossing points
                    if (!FindCurveTurnPoint(mf.trk, false))
                    {
                        if (track.mode == TrackMode.waterPivot || track.mode == TrackMode.bndCurve)
                        {
                            youTurnPhase = 11;//ignore
                        }
                        else
                            FailCreate();
                        return false;
                    }

                    //save a copy
                    inClosestTurnPt = new CClose(closestTurnPt);

                    ytList?.Clear();

                    int count = mf.trk.isHeadingSameWay ? -1 : 1;
                    int curveIndex = inClosestTurnPt.curveIndex;

                    isOutOfBounds = true;
                    int stopIfWayOut = 0;

                    double head = 0; //gives error since isnt updated

                    while (isOutOfBounds)
                    {
                        stopIfWayOut++;
                        isOutOfBounds = false;

                        //creates half a circle starting at the crossing point
                        ytList.Clear();

                        curveIndex += count;

                        vec3 currentPos = new vec3(mf.trk.currentGuidanceTrack[curveIndex]);

                        if (!mf.trk.isHeadingSameWay) currentPos.heading += Math.PI;
                        if (currentPos.heading >= glm.twoPI) currentPos.heading -= glm.twoPI;
                        head = currentPos.heading;

                        CDubins dubYouTurnPath = new CDubins();
                        CDubins.turningRadius = youTurnRadius;

                        //now we go the other way to turn round
                        double invertHead = currentPos.heading - Math.PI;
                        if (invertHead <= -Math.PI) invertHead += glm.twoPI;
                        if (invertHead >= Math.PI) invertHead -= glm.twoPI;

                        vec3 goal = new vec3();

                        //neat trick to not have to add pi/2
                        if (isTurnLeft)
                        {
                            goal.easting = mf.trk.currentGuidanceTrack[curveIndex - count].easting + (Math.Cos(-invertHead) * turnOffset);
                            goal.northing = mf.trk.currentGuidanceTrack[curveIndex - count].northing + (Math.Sin(-invertHead) * turnOffset);
                        }
                        else
                        {
                            goal.easting = mf.trk.currentGuidanceTrack[curveIndex - count].easting - (Math.Cos(-invertHead) * turnOffset);
                            goal.northing = mf.trk.currentGuidanceTrack[curveIndex - count].northing - (Math.Sin(-invertHead) * turnOffset);
                        }

                        goal.heading = invertHead;

                        //generate the turn points
                        ytList = dubYouTurnPath.GenerateDubins(currentPos, goal);
                        if (ytList.Count == 0)
                        {
                            FailCreate();
                            return false;
                        }

                        if (stopIfWayOut == 300 || curveIndex < 1 || curveIndex > (mf.trk.currentGuidanceTrack.Count - 2))
                        {
                            //for some reason it doesn't go inside boundary
                            FailCreate();
                            return false;
                        }

                        for (int i = 0; i < ytList.Count; i++)
                        {
                            if (mf.bnd.IsPointInsideTurnArea(ytList[i]) != 0)
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }
                    }
                    inClosestTurnPt.curveIndex = curveIndex;

                    //too many points from Dubins - so cut
                    double distance;
                    int cnt = ytList.Count;
                    for (int i = 1; i < cnt - 2; i++)
                    {
                        distance = glm.DistanceSquared(ytList[i], ytList[i + 1]);
                        if (distance < pointSpacing)
                        {
                            ytList.RemoveAt(i + 1);
                            i--;
                            cnt = ytList.Count;
                        }
                    }

                    //move the turn to exact at the turnline
                    ytList = MoveTurnInsideTurnLine(ytList, head, false, false);
                    if (ytList.Count == 0)
                    {
                        FailCreate();
                        return false;
                    }

                    youTurnPhase = 1;
                    break;

                case 1:
                    //build the next line to add sequencelines
                    double widthMinusOverlap = mf.tool.width - mf.tool.overlap;

                    double distAway = widthMinusOverlap * (mf.trk.howManyPathsAway + ((isTurnLeft ^ mf.trk.isHeadingSameWay) ? rowSkipsWidth : -rowSkipsWidth)) + (mf.trk.isHeadingSameWay ? -mf.tool.offset : mf.tool.offset) + track.nudgeDistance;

                    distAway += (0.5 * widthMinusOverlap);

                    //create the next line
                    nextCurve = mf.trk.BuildCurrentGuidanceTrack(distAway, track);

                    //get the index of the last yt point
                    double dis = double.MaxValue;
                    if (nextCurve.Count > 1)
                    {
                        for (int i = 1; i < nextCurve.Count; i++)
                        {
                            double newdis = glm.Distance(nextCurve[i], ytList[ytList.Count - 1]);
                            if (newdis < dis)
                            {
                                dis = newdis;
                                if (mf.trk.isHeadingSameWay) outClosestTurnPt.curveIndex = i - 1;
                                else outClosestTurnPt.curveIndex = i;
                            }
                        }

                        if (outClosestTurnPt.curveIndex >= 0)
                        {
                            outClosestTurnPt.closePt = new vec3(nextCurve[outClosestTurnPt.curveIndex]);
                            inClosestTurnPt.closePt = new vec3(mf.trk.currentGuidanceTrack[inClosestTurnPt.curveIndex]);

                            if (!AddCurveSequenceLines(3)) return false;
                        }
                    }

                    //fill in the gaps
                    double distanc;

                    int cnt4 = ytList.Count;
                    for (int i = 1; i < cnt4 - 2; i++)
                    {
                        int j = i + 1;
                        if (j == cnt4 - 1) continue;
                        distanc = glm.DistanceSquared(ytList[i], ytList[j]);
                        if (distanc > 1)
                        {
                            vec3 pointB = new vec3((ytList[i].easting + ytList[j].easting) / 2.0,
                                (ytList[i].northing + ytList[j].northing) / 2.0, ytList[i].heading);

                            ytList.Insert(j, pointB);
                            cnt4 = ytList.Count;
                            i--;
                        }
                    }

                    //calculate the new points headings based on fore and aft of point - smoother turns
                    cnt4 = ytList.Count;
                    vec3[] arr = new vec3[cnt4];
                    cnt4 -= 2;
                    ytList.CopyTo(arr);
                    ytList.Clear();

                    for (int i = 2; i < cnt4; i++)
                    {
                        vec3 pt3 = new vec3(arr[i]);
                        pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting,
                            arr[i + 1].northing - arr[i - 1].northing);
                        if (pt3.heading < 0) pt3.heading += glm.twoPI;
                        ytList.Add(pt3);
                    }

                    //check to close
                    if (glm.Distance(ytList[0], mf.pivotAxlePos) < 3)
                    {
                        FailCreate();
                        return false;
                    }

                    isOutOfBounds = false;
                    youTurnPhase = 10;
                    turnTooCloseTrigger = false;
                    isTurnCreationTooClose = false;
                    return true;
            }
            return true;
        }

        private bool CreateCurveWideTurn()
        {
            //keep from making turns constantly - wait 1.5 seconds
            if (mf.makeUTurnCounter < 4)
            {
                youTurnPhase = 0;
                return true;
            }

            if (mf.trk.idx < 0 || mf.trk.gArr.Count < mf.trk.idx) return true;
            CTrk track = mf.trk.gArr[mf.trk.idx];

            //we are doing a wide turn
            double head = 0;
            int count = mf.trk.isHeadingSameWay ? -1 : 1;
            switch (youTurnPhase)
            {
                case 0:
                    //Create first semicircle
                    if (!FindCurveTurnPoint(mf.trk, false))
                    {
                        if (track.mode == TrackMode.waterPivot || track.mode == TrackMode.bndCurve)
                        {
                            youTurnPhase = 11;//ignore
                        }
                        else
                            FailCreate();
                        return false;
                    }
                    inClosestTurnPt = new CClose(closestTurnPt);
                    startOfTurnPt = new CClose(inClosestTurnPt);

                    int stopIfWayOut = 0;
                    isOutOfBounds = true;

                    while (isOutOfBounds)
                    {
                        isOutOfBounds = false;
                        stopIfWayOut++;

                        vec3 currentPos = new vec3(mf.trk.currentGuidanceTrack[inClosestTurnPt.curveIndex]);

                        head = currentPos.heading;
                        if (!mf.trk.isHeadingSameWay) head += Math.PI;
                        if (head > glm.twoPI) head -= glm.twoPI;
                        currentPos.heading = head;

                        // creates half a circle starting at the crossing point
                        ytList.Clear();
                        ytList.Add(currentPos);

                        //Taken from Dubbins
                        while (Math.Abs(head - currentPos.heading) < Math.PI)
                        {
                            //Update the position of the car
                            currentPos.easting += pointSpacing * Math.Sin(currentPos.heading);
                            currentPos.northing += pointSpacing * Math.Cos(currentPos.heading);

                            //Which way are we turning?
                            double turnParameter = isTurnLeft ? -1.0 : 1.0;

                            //Update the heading
                            currentPos.heading += (pointSpacing / youTurnRadius) * turnParameter;

                            //Add the new coordinate to the path
                            ytList.Add(currentPos);
                        }

                        int cnt4 = ytList.Count;
                        if (cnt4 == 0)
                        {
                            FailCreate();
                            return false;
                        }

                        //Are we out of bounds?
                        for (int j = 0; j < cnt4; j += 2)
                        {
                            if (mf.bnd.IsPointInsideTurnArea(ytList[j]) != 0)
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }

                        //first check if not out of bounds, add a bit more to clear turn line, set to phase 2
                        if (!isOutOfBounds)
                        {
                            ytList = MoveTurnInsideTurnLine(ytList, head, true, false);
                            if (ytList.Count == 0)
                            {
                                FailCreate();
                                return false;
                            }
                            youTurnPhase = 1;
                            return true;
                        }

                        if (stopIfWayOut == 300 || inClosestTurnPt.curveIndex < 1 || inClosestTurnPt.curveIndex > (mf.trk.currentGuidanceTrack.Count - 2))
                        {
                            //for some reason it doesn't go inside boundary
                            FailCreate();
                            return false;
                        }

                        //keep moving infield till pattern is all inside
                        inClosestTurnPt.curveIndex = inClosestTurnPt.curveIndex + count;
                        inClosestTurnPt.closePt = new vec3(mf.trk.currentGuidanceTrack[inClosestTurnPt.curveIndex]);

                        //set the flag to Critical stop machine
                        if (glm.Distance(ytList[0], mf.pivotAxlePos) < 3)
                        {
                            FailCreate();
                            return false;
                        }
                    }

                    return false;

                case 1:
                    //build the next line to add sequencelines
                    double widthMinusOverlap = mf.tool.width - mf.tool.overlap;

                    double distAway = widthMinusOverlap * (mf.trk.howManyPathsAway + ((isTurnLeft ^ mf.trk.isHeadingSameWay) ? rowSkipsWidth : -rowSkipsWidth)) + (mf.trk.isHeadingSameWay ? -mf.tool.offset : mf.tool.offset) + track.nudgeDistance;

                    distAway += (0.5 * widthMinusOverlap);

                    //create the next line
                    nextCurve = mf.trk.BuildCurrentGuidanceTrack(distAway, track);

                    //going with or against boundary?
                    bool isTurnLineSameWay = true;
                    double headingDifference = Math.Abs(inClosestTurnPt.turnLineHeading - ytList[ytList.Count - 1].heading);
                    if (headingDifference > glm.PIBy2 && headingDifference < 3 * glm.PIBy2) isTurnLineSameWay = false;

                    if (!FindCurveOutTurnPoint(mf.trk, ref nextCurve, startOfTurnPt, isTurnLineSameWay))
                    {
                        //error
                        FailCreate();
                        return false;
                    }
                    outClosestTurnPt = new CClose(closestTurnPt);

                    //move the turn inside of turnline with help from the crossingCurvePoints
                    isOutOfBounds = true;
                    while (isOutOfBounds)
                    {
                        isOutOfBounds = false;
                        vec3 currentPos = new vec3(nextCurve[outClosestTurnPt.curveIndex]);

                        head = currentPos.heading;
                        if ((!mf.trk.isHeadingSameWay && !isOutSameCurve) || (mf.trk.isHeadingSameWay && isOutSameCurve)) head += Math.PI;
                        if (head > glm.twoPI) head -= glm.twoPI;
                        currentPos.heading = head;

                        // creates half a circle starting at the crossing point
                        ytList2?.Clear();
                        ytList2.Add(currentPos);

                        //Taken from Dubbins
                        while (Math.Abs(head - currentPos.heading) < Math.PI)
                        {
                            //Update the position of the car
                            currentPos.easting += pointSpacing * Math.Sin(currentPos.heading);
                            currentPos.northing += pointSpacing * Math.Cos(currentPos.heading);

                            //Which way are we turning?
                            double turnParameter = isTurnLeft ? 1.0 : -1.0;

                            //Update the heading
                            currentPos.heading += (pointSpacing / youTurnRadius) * turnParameter;

                            //Add the new coordinate to the path
                            ytList2.Add(currentPos);
                        }

                        int cnt3 = ytList2.Count;
                        if (cnt3 == 0)
                        {
                            FailCreate();
                            return false;
                        }

                        //Are we out of bounds?
                        for (int j = 0; j < cnt3; j += 2)
                        {
                            if (mf.bnd.IsPointInsideTurnArea(ytList2[j]) != 0)
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }

                        //first check if not out of bounds, add a bit more to clear turn line, set to phase 2
                        if (!isOutOfBounds)
                        {
                            ytList2 = MoveTurnInsideTurnLine(ytList2, head, true, true);
                            if (ytList2.Count == 0)
                            {
                                FailCreate();
                                return false;
                            }
                            youTurnPhase = 2;
                            return true;
                        }

                        if (outClosestTurnPt.curveIndex < 1 || outClosestTurnPt.curveIndex > (nextCurve.Count - 2))
                        {
                            //for some reason it doesn't go inside boundary
                            FailCreate();
                            return false;
                        }

                        //keep moving infield till pattern is all inside
                        if (!isOutSameCurve) outClosestTurnPt.curveIndex = outClosestTurnPt.curveIndex + count;
                        else outClosestTurnPt.curveIndex = outClosestTurnPt.curveIndex - count;
                        outClosestTurnPt.closePt = new vec3(nextCurve[outClosestTurnPt.curveIndex]);
                    }
                    return false;

                case 2:
                    //Bind the two turns together
                    int cnt1 = ytList.Count;
                    int cnt2 = ytList2.Count;

                    //Find if the turn goes same way as turnline heading
                    bool isFirstTurnLineSameWay = true;
                    double firstHeadingDifference = Math.Abs(inClosestTurnPt.turnLineHeading - ytList[ytList.Count - 1].heading);
                    if (firstHeadingDifference > glm.PIBy2 && firstHeadingDifference < 3 * glm.PIBy2) isFirstTurnLineSameWay = false;

                    //finds out start and goal point along the tunline
                    FindInnerTurnPoints(ytList[cnt1 - 1], ytList[0].heading, inClosestTurnPt, isFirstTurnLineSameWay);
                    CClose startClosestTurnPt = new CClose(closestTurnPt);

                    FindInnerTurnPoints(ytList2[cnt2 - 1], ytList2[0].heading + Math.PI, outClosestTurnPt, !isFirstTurnLineSameWay);
                    CClose goalClosestTurnPt = new CClose(closestTurnPt);

                    //we have 2 different turnLine crossings
                    if (startClosestTurnPt.turnLineNum != goalClosestTurnPt.turnLineNum)
                    {
                        FailCreate();
                        return false;
                    }

                    //segment index is the "A" of the segment. segmentIndex+1 would be the "B"
                    //is in and out on same segment? so only 1 segment
                    if (startClosestTurnPt.turnLineIndex == goalClosestTurnPt.turnLineIndex)
                    {
                        for (int a = 0; a < cnt2; cnt2--)
                        {
                            ytList.Add(new vec3(ytList2[cnt2 - 1]));
                        }
                    }
                    else
                    {
                        //mulitple segments
                        vec3 tPoint = new vec3();
                        int turnCount = mf.bnd.bndList[startClosestTurnPt.turnLineNum].turnLine.Count;

                        //how many points from turnline do we add
                        int loops = Math.Abs(startClosestTurnPt.turnLineIndex - goalClosestTurnPt.turnLineIndex);

                        //are we crossing a border?
                        if (loops > (mf.bnd.bndList[startClosestTurnPt.turnLineNum].turnLine.Count / 2))
                        {
                            if (startClosestTurnPt.turnLineIndex < goalClosestTurnPt.turnLineIndex)
                            {
                                loops = (turnCount - goalClosestTurnPt.turnLineIndex) + startClosestTurnPt.turnLineIndex;
                            }
                            else
                            {
                                loops = (turnCount - startClosestTurnPt.turnLineIndex) + goalClosestTurnPt.turnLineIndex;
                            }
                        }

                        //countExit up - start with B which is next A
                        if (isFirstTurnLineSameWay)
                        {
                            for (int i = 0; i < loops; i++)
                            {
                                if ((startClosestTurnPt.turnLineIndex + 1) >= turnCount) startClosestTurnPt.turnLineIndex = -1;

                                tPoint = mf.bnd.bndList[startClosestTurnPt.turnLineNum].turnLine[startClosestTurnPt.turnLineIndex + 1];
                                startClosestTurnPt.turnLineIndex++;
                                if (startClosestTurnPt.turnLineIndex >= turnCount)
                                    startClosestTurnPt.turnLineIndex = 0;
                                ytList.Add(tPoint);
                            }
                        }
                        else //countExit down = start with A
                        {
                            for (int i = 0; i < loops; i++)
                            {
                                tPoint = mf.bnd.bndList[startClosestTurnPt.turnLineNum].turnLine[startClosestTurnPt.turnLineIndex];
                                startClosestTurnPt.turnLineIndex--;
                                if (startClosestTurnPt.turnLineIndex == -1)
                                    startClosestTurnPt.turnLineIndex = turnCount - 1;
                                ytList.Add(tPoint);
                            }
                        }

                        //add the out from ytList2
                        for (int a = 0; a < cnt2; cnt2--)
                        {
                            ytList.Add(new vec3(ytList2[cnt2 - 1]));
                        }
                    }

                    if (!AddCurveSequenceLines(5)) return false;

                    //fill in the gaps
                    double distance;

                    int cnt = ytList.Count;
                    for (int i = 1; i < cnt - 2; i++)
                    {
                        int j = i + 1;
                        if (j == cnt - 1) continue;
                        distance = glm.DistanceSquared(ytList[i], ytList[j]);
                        if (distance > 1)
                        {
                            vec3 pointB = new vec3((ytList[i].easting + ytList[j].easting) / 2.0,
                                (ytList[i].northing + ytList[j].northing) / 2.0, ytList[i].heading);

                            ytList.Insert(j, pointB);
                            cnt = ytList.Count;
                            i--;
                        }
                    }

                    //calculate the new points headings based on fore and aft of point - smoother turns
                    cnt = ytList.Count;
                    vec3[] arr = new vec3[cnt];
                    cnt -= 2;
                    ytList.CopyTo(arr);
                    ytList.Clear();

                    for (int i = 2; i < cnt; i++)
                    {
                        vec3 pt3 = new vec3(arr[i]);
                        pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting,
                            arr[i + 1].northing - arr[i - 1].northing);
                        if (pt3.heading < 0) pt3.heading += glm.twoPI;
                        ytList.Add(pt3);
                    }

                    //check to close
                    if (glm.Distance(ytList[0], mf.pivotAxlePos) < 3)
                    {
                        FailCreate();
                        return false;
                    }

                    //are we continuing the same way?
                    isGoingStraightThrough = Math.PI - Math.Abs(Math.Abs(ytList[ytList.Count - 2].heading - ytList[1].heading) - Math.PI) < glm.PIBy2;
                    ytList2?.Clear();
                    isOutOfBounds = false;
                    youTurnPhase = 10;
                    turnTooCloseTrigger = false;
                    isTurnCreationTooClose = false;
                    return true;
            }

            // just in case
            return false;
        }

        #endregion CreateTurn

        #region KStyle turns

        public bool KStyleTurnCurve()
        {
            //grab the vehicle widths and offsets
            double turnOffset = (mf.tool.width - mf.tool.overlap) * rowSkipsWidth + (isTurnLeft ? -mf.tool.offset * 2.0 : mf.tool.offset * 2.0);
            double pointSpacing = youTurnRadius * 0.1;

            isHeadingSameWay = mf.trk.isHeadingSameWay;

            int turnIndex = mf.bnd.IsPointInsideTurnArea(mf.pivotAxlePos);
            if (mf.makeUTurnCounter < 4 || turnIndex != 0)
            {
                youTurnPhase = 0;
                return true;
            }

            mf.makeUTurnCounter = 0;

            if (!FindCurveTurnPoint(mf.trk, true))
            {
                FailCreate();
                return false;
            }

            //save a copy
            inClosestTurnPt = new CClose(closestTurnPt);

            ytList?.Clear();

            int count = isHeadingSameWay ? -1 : 1;
            int curveIndex = inClosestTurnPt.curveIndex + count;

            bool pointOutOfBnd = true;
            int stopIfWayOut = 0;

            double head = 0;

            while (pointOutOfBnd)
            {
                stopIfWayOut++;
                pointOutOfBnd = false;

                //creates half a circle starting at the crossing point
                ytList.Clear();
                if (curveIndex >= mf.trk.currentGuidanceTrack.Count || curveIndex < 0)
                {
                    FailCreate();
                    return false;
                }
                vec3 currentPos = new vec3(mf.trk.currentGuidanceTrack[curveIndex]);

                curveIndex += count;

                if (!isHeadingSameWay) currentPos.heading += Math.PI;
                if (currentPos.heading >= glm.twoPI) currentPos.heading -= glm.twoPI;

                ytList.Add(currentPos);

                while (Math.Abs(ytList[0].heading - currentPos.heading) < 2.2)
                {
                    //Update the position of the car
                    currentPos.easting += pointSpacing * Math.Sin(currentPos.heading);
                    currentPos.northing += pointSpacing * Math.Cos(currentPos.heading);

                    //Which way are we turning?
                    double turnParameter = isTurnLeft ? -1.0 : 1.0;

                    //Update the heading
                    currentPos.heading += (pointSpacing / youTurnRadius) * turnParameter;

                    //Add the new coordinate to the path
                    ytList.Add(currentPos);
                }

                for (int i = 0; i < ytList.Count; i++)
                {
                    if (mf.bnd.IsPointInsideTurnArea(ytList[i]) != 0)
                    {
                        pointOutOfBnd = true;
                        break;
                    }
                }
            }

            //move out
            head = ytList[0].heading;
            double cosHead = Math.Cos(head) * 0.1;
            double sinHead = Math.Sin(head) * 0.1;
            vec3[] arr2 = new vec3[ytList.Count];
            ytList.CopyTo(arr2);
            ytList.Clear();

            //step 2 move the turn inside with steps of 0.1 meter
            int j = 0;
            pointOutOfBnd = false;

            while (!pointOutOfBnd)
            {
                stopIfWayOut++;
                pointOutOfBnd = false;
                mf.distancePivotToTurnLine = glm.DistanceSquared(arr2[0], mf.pivotAxlePos);

                for (int i = 0; i < arr2.Length; i++)
                {
                    arr2[i].easting += (sinHead);
                    arr2[i].northing += (cosHead);
                }

                for (j = 0; j < arr2.Length; j++)
                {
                    int bob = mf.bnd.IsPointInsideTurnArea(arr2[j]);
                    if (bob != 0)
                    {
                        pointOutOfBnd = true;
                        break;
                    }
                }

                if (stopIfWayOut == 300 || (mf.distancePivotToTurnLine < 6))
                {
                    //for some reason it doesn't go inside boundary, return empty list
                    return false;
                }
            }

            ytList.AddRange(arr2);

            //add start extension from curve points
            curveIndex -= count;

            //point used to set next guidance line

            //now we go the other way to turn round
            head = ytList[0].heading;
            head -= Math.PI;
            if (head < -Math.PI) head += glm.twoPI;
            if (head > Math.PI) head -= glm.twoPI;

            if (head >= glm.twoPI) head -= glm.twoPI;
            else if (head < 0) head += glm.twoPI;

            //add the tail to first turn
            head = ytList[ytList.Count - 1].heading;

            vec3 pt;
            for (int i = 1; i <= (int)(3 * turnOffset); i++)
            {
                pt.easting = ytList[ytList.Count - 1].easting + (Math.Sin(head) * 0.5);
                pt.northing = ytList[ytList.Count - 1].northing + (Math.Cos(head) * 0.5);
                pt.heading = 0;
                ytList.Add(pt);
            }

            //leading in line of turn
            for (int i = 0; i < 4; i++)
            {
                ytList.Insert(0, new vec3(mf.trk.currentGuidanceTrack[curveIndex + i * count]));
            }

            //fill in the gaps
            double distance;

            int cnt = ytList.Count;
            for (int i = 1; i < cnt - 2; i++)
            {
                j = i + 1;
                if (j == cnt - 1) continue;
                distance = glm.DistanceSquared(ytList[i], ytList[j]);
                if (distance > 1)
                {
                    vec3 pointB = new vec3((ytList[i].easting + ytList[j].easting) / 2.0,
                        (ytList[i].northing + ytList[j].northing) / 2.0, ytList[i].heading);

                    ytList.Insert(j, pointB);
                    cnt = ytList.Count;
                    i--;
                }
            }

            //calculate line headings
            vec3[] arr = new vec3[ytList.Count];
            ytList.CopyTo(arr);
            ytList.Clear();

            for (int i = 0; i < arr.Length - 1; i++)
            {
                arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                ytList.Add(arr[i]);
            }

            mf.distancePivotToTurnLine = glm.Distance(ytList[0], mf.pivotAxlePos);

            isOutOfBounds = false;
            youTurnPhase = 10;
            turnTooCloseTrigger = false;
            isTurnCreationTooClose = false;

            return true;
        }

        #endregion KStyle turns

        #region FindTurnPoint

        public bool FindCurveOutTurnPoint(CTracks thisCurve, ref List<vec3> nextCurve, CClose inPt, bool isTurnLineSameWay)
        {
            int a = isTurnLineSameWay ? 1 : -1;

            int turnLineIndex = inPt.turnLineIndex;
            int turnLineNum = inPt.turnLineNum;
            int stopTurnLineIndex = inPt.turnLineIndex - a;
            if (stopTurnLineIndex < 0) stopTurnLineIndex = mf.bnd.bndList[turnLineNum].turnLine.Count - 3;
            if (stopTurnLineIndex > mf.bnd.bndList[turnLineNum].turnLine.Count - 1) turnLineIndex = 3;

            for (; turnLineIndex != stopTurnLineIndex; turnLineIndex += a)
            {
                if (turnLineIndex < 0) turnLineIndex = mf.bnd.bndList[turnLineNum].turnLine.Count - 2; //AAA could be less than 0???
                if (turnLineIndex > mf.bnd.bndList[turnLineNum].turnLine.Count - 2) turnLineIndex = 0;

                for (int i = 0; i < nextCurve.Count - 2; i++)
                {
                    int res = GetLineIntersection(
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].easting,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].northing,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].easting,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].northing,

                                    nextCurve[i].easting,
                                    nextCurve[i].northing,
                                    nextCurve[i + 1].easting,
                                    nextCurve[i + 1].northing,
                                     ref iE, ref iN);
                    if (res == 1)
                    {
                        closestTurnPt = new CClose();
                        closestTurnPt.closePt.easting = iE;
                        closestTurnPt.closePt.northing = iN;
                        closestTurnPt.closePt.heading = nextCurve[i].heading;
                        closestTurnPt.turnLineIndex = turnLineIndex;
                        closestTurnPt.curveIndex = i;
                        closestTurnPt.turnLineHeading = mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].heading;
                        closestTurnPt.turnLineNum = turnLineNum;
                        return true;
                    }
                }

                for (int i = 0; i < thisCurve.currentGuidanceTrack.Count - 2; i++)
                {
                    int res = GetLineIntersection(
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].easting,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].northing,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].easting,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].northing,

                                    thisCurve.currentGuidanceTrack[i].easting,
                                    thisCurve.currentGuidanceTrack[i].northing,
                                    thisCurve.currentGuidanceTrack[i + 1].easting,
                                    thisCurve.currentGuidanceTrack[i + 1].northing,

                                     ref iE, ref iN);
                    if (res == 1)
                    {
                        if ((i < inPt.curveIndex && thisCurve.isHeadingSameWay) || (i > inPt.curveIndex && !thisCurve.isHeadingSameWay))
                        {
                            return false; //hitting the curve behind us
                        }
                        else if (i == inPt.curveIndex)
                        {
                            //do nothing hitting the curve at the same place as in
                        }
                        else
                        {
                            closestTurnPt = new CClose();
                            closestTurnPt.closePt.easting = iE;
                            closestTurnPt.closePt.northing = iN;
                            closestTurnPt.closePt.heading = thisCurve.currentGuidanceTrack[i].heading; //ändrad nyss till this curve
                            closestTurnPt.turnLineIndex = turnLineIndex;
                            closestTurnPt.curveIndex = i;
                            closestTurnPt.turnLineHeading = mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].heading;
                            closestTurnPt.turnLineNum = turnLineNum;
                            isOutSameCurve = true;
                            nextCurve = thisCurve.currentGuidanceTrack;//???? already created the line so why change it
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool FindInnerTurnPoints(vec3 fromPt, double inDirection, CClose refClosePt, bool isTurnLineSameWay)
        {
            double eP, nP;

            eP = fromPt.easting + Math.Sin(inDirection);
            nP = fromPt.northing + Math.Cos(inDirection);

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

                int res = GetLineIntersection(
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].easting,
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].northing,
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].easting,
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].northing,

                                fromPt.easting,
                                fromPt.northing, eP, nP, ref iE, ref iN);
                if (res == 1)
                {
                    closestTurnPt = new CClose();
                    closestTurnPt.closePt.easting = iE;
                    closestTurnPt.closePt.northing = iN;
                    closestTurnPt.closePt.heading = -1; //isnt needed but could be calculated
                    closestTurnPt.turnLineIndex = turnLineIndex;
                    closestTurnPt.curveIndex = -1;
                    closestTurnPt.turnLineHeading = mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].heading;
                    closestTurnPt.turnLineNum = turnLineNum;
                    return true;
                }
            }
            return false;
        }

        private bool FindCurveTurnPoint(CTracks thisCurve, bool noIdea)
        {
            //AAA Is updated but not tested....
            //find closet AB Curve point that will cross and go out of bounds
            int Count = mf.trk.isHeadingSameWay ? 1 : -1;
            int turnNum = 99;
            int j;

            closestTurnPt = new CClose();

            if (mf.trk.idx < 0 || mf.trk.gArr.Count < mf.trk.idx) return true;
            CTrk track = mf.trk.gArr[mf.trk.idx];

            bool loop = track.mode == TrackMode.bndCurve || track.mode == TrackMode.waterPivot;

            for (j = mf.gyd.currentLocationIndex; j > 0 && j < thisCurve.currentGuidanceTrack.Count; j += Count)
            {
                if (j < 0)
                {
                    if (loop)
                    {
                        loop = false;
                        j = thisCurve.currentGuidanceTrack.Count;
                        continue;
                    }
                    break;
                }
                else if (j >= thisCurve.currentGuidanceTrack.Count)
                {
                    if (loop)
                    {
                        loop = false;
                        j = -1;
                        continue;
                    }
                    break;
                }

                int turnIndex = mf.bnd.IsPointInsideTurnArea(thisCurve.currentGuidanceTrack[j]);
                if (turnIndex != 0)
                {
                    closestTurnPt.curveIndex = j - Count;
                    closestTurnPt.turnLineNum = turnIndex;
                    turnNum = turnIndex;
                    break;
                }
            }

            if (turnNum < 0) //uturn will be on outer boundary turn
            {
                closestTurnPt.turnLineNum = 0;
                turnNum = 0;
            }
            else if (turnNum == 99)
            {
                //curve does not cross a boundary - oops
                isTurnCreationNotCrossingError = true;
                return false;
            }

            if (closestTurnPt.curveIndex == -1)
            {
                isTurnCreationNotCrossingError = true;
                return false;
            }

            for (int i = 0; i < mf.bnd.bndList[turnNum].turnLine.Count - 1; i++)
            {
                int res = GetLineIntersection(
                        mf.bnd.bndList[turnNum].turnLine[i].easting,
                        mf.bnd.bndList[turnNum].turnLine[i].northing,
                        mf.bnd.bndList[turnNum].turnLine[i + 1].easting,
                        mf.bnd.bndList[turnNum].turnLine[i + 1].northing,

                        thisCurve.currentGuidanceTrack[closestTurnPt.curveIndex].easting,
                        thisCurve.currentGuidanceTrack[closestTurnPt.curveIndex].northing,
                        thisCurve.currentGuidanceTrack[closestTurnPt.curveIndex + Count].easting,
                        thisCurve.currentGuidanceTrack[closestTurnPt.curveIndex + Count].northing,

                         ref iE, ref iN);

                if (res == 1)
                {
                    closestTurnPt.closePt.easting = iE;
                    closestTurnPt.closePt.northing = iN;
                    if (noIdea)
                    {
                        double hed = Math.Atan2(mf.bnd.bndList[turnNum].turnLine[i + 1].easting - mf.bnd.bndList[turnNum].turnLine[i].easting,
                            mf.bnd.bndList[turnNum].turnLine[i + 1].northing - mf.bnd.bndList[turnNum].turnLine[i].northing);
                        if (hed < 0) hed += glm.twoPI;
                        closestTurnPt.closePt.heading = hed;
                        closestTurnPt.turnLineIndex = i;
                    }
                    else
                    {
                        closestTurnPt.closePt.heading = thisCurve.currentGuidanceTrack[closestTurnPt.curveIndex].heading;
                        closestTurnPt.turnLineIndex = i;
                        closestTurnPt.turnLineNum = turnNum;
                        closestTurnPt.turnLineHeading = mf.bnd.bndList[turnNum].turnLine[i].heading;
                        if (!thisCurve.isHeadingSameWay && closestTurnPt.curveIndex > 0) closestTurnPt.curveIndex--;
                    }
                    break;
                }
            }

            return closestTurnPt.turnLineIndex != -1 && closestTurnPt.curveIndex != -1;
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
            if (isOutSameCurve) sameWay = !sameWay;
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
        public int GetLineIntersection(double p0x, double p0y, double p1x, double p1y,
                double p2x, double p2y, double p3x, double p3y, ref double iEast, ref double iNorth)
        {
            double s1x, s1y, s2x, s2y;
            s1x = p1x - p0x;
            s1y = p1y - p0y;

            s2x = p3x - p2x;
            s2y = p3y - p2y;

            double s, t;
            s = (-s1y * (p0x - p2x) + s1x * (p0y - p2y)) / (-s2x * s1y + s1x * s2y);

            if (s >= 0 && s <= 1)
            {
                //check oher side
                t = (s2x * (p0y - p2y) - s2y * (p0x - p2x)) / (-s2x * s1y + s1x * s2y);
                if (t >= 0 && t <= 1)
                {
                    // Collision detected
                    iEast = p0x + (t * s1x);
                    iNorth = p0y + (t * s1y);
                    return 1;
                }
            }

            return 0; // No collision
        }

        private List<vec3> MoveTurnInsideTurnLine(List<vec3> uTurnList, double head, bool deleteSecondHalf, bool invertHeading)
        {
            //step 1 make array out of the list so that we can modify the position
            double cosHead = Math.Cos(head);
            double sinHead = Math.Sin(head);
            int cnt = uTurnList.Count;
            vec3[] arr2 = new vec3[cnt];
            uTurnList.CopyTo(arr2);
            uTurnList.Clear();

            semiCircleIndex = -1;
            //step 2 move the turn inside with steps of 1 meter
            bool pointOutOfBnd = isOutOfBounds;
            int j = 0;
            int stopIfWayOut = 0;
            while (pointOutOfBnd)
            {
                stopIfWayOut++;
                pointOutOfBnd = false;
                mf.distancePivotToTurnLine = glm.Distance(arr2[0], mf.pivotAxlePos);

                for (int i = 0; i < cnt; i++)
                {
                    arr2[i].easting -= (sinHead);
                    arr2[i].northing -= (cosHead);
                }

                for (; j < cnt; j += 1)
                {
                    if (mf.bnd.IsPointInsideTurnArea(arr2[j]) != 0)
                    {
                        pointOutOfBnd = true;
                        if (j > 0) j--;
                        break;
                    }
                }

                if (stopIfWayOut == 1000 || (mf.distancePivotToTurnLine < 3))
                {
                    //for some reason it doesn't go inside boundary, return empty list
                    return uTurnList;
                }
            }

            //step 3, we ar now inside turnline, move the turn forward until it hits the turnfence in steps of 0.1 meters
            while (!pointOutOfBnd)
            {
                for (int i = 0; i < cnt; i++)
                {
                    arr2[i].easting += (sinHead * 0.1);
                    arr2[i].northing += (cosHead * 0.1);
                }

                for (int a = 0; a < cnt; a++)
                {
                    if (mf.bnd.IsPointInsideTurnArea(arr2[a]) != 0)
                    {
                        semiCircleIndex = a;
                        pointOutOfBnd = true;
                        break;
                    }
                }
            }

            //step 4, Should we delete the points after the one that is outside? and where the points made in the wrong direction?
            for (int i = 0; i < cnt; i++)
            {
                if (i == semiCircleIndex && deleteSecondHalf)
                    break;
                if (invertHeading) arr2[i].heading += Math.PI;
                if (arr2[i].heading >= glm.twoPI) arr2[i].heading -= glm.twoPI;
                else if (arr2[i].heading < 0) arr2[i].heading += glm.twoPI;
                uTurnList.Add(arr2[i]);
            }

            //we have succesfully moved the turn inside
            isOutOfBounds = false;

            //if empty - no creation.
            return uTurnList;
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
            mf.sounds.isBoundAlarming = false;
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
            //fix you turn
            isYouTurnTriggered = false;
            mf.makeUTurnCounter = 0;
            ytList?.Clear();
            ResetCreatedYouTurn();
            mf.sounds.isBoundAlarming = false;
            isTurnCreationTooClose = false;
            isTurnCreationNotCrossingError = false;
            PGN_239.pgn[PGN_239.uturn] = 0;
        }

        public void ResetCreatedYouTurn()
        {
            youTurnPhase = 0;
            ytList?.Clear();
            mf.makeUTurnCounter = 0;
            PGN_239.pgn[PGN_239.uturn] = 0;
            isOutSameCurve = false;
            isGoingStraightThrough = false;
        }

        public void FailCreate()
        {
            //fail
            isOutOfBounds = true;
            isTurnCreationTooClose = true;
            mf.mc.isOutOfBounds = true;
            youTurnPhase = 11;
        }

        public void BuildManualYouLateral(bool isTurnLeft)
        {
            //point on AB line closest to pivot axle point from AB Line PurePursuit
            if (mf.trk.idx > -1 && mf.trk.gArr.Count > 0)
            {
                isHeadingSameWay = mf.trk.isHeadingSameWay;
                if (isHeadingSameWay == isTurnLeft)
                {
                    mf.trk.howManyPathsAway += 1;
                }
                else
                {
                    mf.trk.howManyPathsAway -= 1;
                }
                return;
            }
            else return;
        }

        //build the points and path of youturn to be scaled and transformed
        public void BuildManualYouTurn(bool isTurnRight, bool isTurnButtonTriggered)
        {
            double head;
            //point on AB line closest to pivot axle point from AB Line PurePursuit
            //if (mf.trk.idx > -1 && mf.trk.gArr.Count > 0)
            //{
            double rEastYT = mf.gyd.rEastTrk;
            double rNorthYT = mf.gyd.rNorthTrk;
            isHeadingSameWay = mf.trk.isHeadingSameWay;
            head = mf.gyd.manualUturnHeading;
            //}
            //else return;

            //grab the vehicle widths and offsets
            double turnOffset = (mf.tool.width - mf.tool.overlap) * rowSkipsWidth + (isTurnRight ? mf.tool.offset * 2.0 : -mf.tool.offset * 2.0);

            CDubins dubYouTurnPath = new CDubins();
            CDubins.turningRadius = youTurnRadius;

            //if its straight across it makes 2 loops instead so goal is a little lower then start
            if (!isHeadingSameWay) head += 3.14;
            else head -= 0.01;

            //move the start forward 2 meters, this point is critical to formation of uturn
            rEastYT += (Math.Sin(head) * 4);
            rNorthYT += (Math.Cos(head) * 4);

            //now we have our start point
            vec3 start = new vec3(rEastYT, rNorthYT, head);
            vec3 goal = new vec3();

            //now we go the other way to turn round
            head -= Math.PI;
            if (head < 0) head += glm.twoPI;

            //set up the goal point for Dubins
            goal.heading = head;
            if (isTurnButtonTriggered)
            {
                if (isTurnRight)
                {
                    goal.easting = rEastYT - (Math.Cos(-head) * turnOffset);
                    goal.northing = rNorthYT - (Math.Sin(-head) * turnOffset);
                }
                else
                {
                    goal.easting = rEastYT + (Math.Cos(-head) * turnOffset);
                    goal.northing = rNorthYT + (Math.Sin(-head) * turnOffset);
                }
            }

            //generate the turn points
            ytList = dubYouTurnPath.GenerateDubins(start, goal);

            isTurnLeft = !isTurnRight;
            YouTurnTrigger();
        }

        //Duh.... What does this do....
        public void DrawYouTurn()
        {
            if (ytList.Count < 3) return;

            GL.PointSize(Settings.User.setDisplay_lineWidth + 2);

            if (isYouTurnTriggered)
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
            public double turnLineHeading;
            public int curveIndex;

            public CClose()
            {
                closePt = new vec3();
                turnLineNum = -1;
                turnLineIndex = -1;
                turnLineHeading = -1;
                curveIndex = -1;
            }

            public CClose(CClose _clo)
            {
                closePt = new vec3(_clo.closePt);
                turnLineNum = _clo.turnLineNum;
                turnLineIndex = _clo.turnLineIndex;
                turnLineHeading = _clo.turnLineHeading;
                curveIndex = _clo.curveIndex;
            }
        }
    }
}