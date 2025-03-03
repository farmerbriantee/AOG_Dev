//Please, if you use this, share the improvements


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        //very first fix to setup grid etc
        public bool isGPSPositionInitialized = false, isFirstHeadingSet = false,
            isReverse = false, isSteerInReverse = true;

        //string to record fixes for elevation maps
        public StringBuilder sbElevationString = new StringBuilder();

        // autosteer variables for sending serial
        public double guidanceLineDistanceOff;
        public double guidanceLineSteerAngle;

        public double setAngVel, actAngVel;
        public bool isConstantContourOn;

        //guidance line look ahead
        public double guidanceLookAheadTime = 2;
        public vec2 guidanceLookPos = new vec2(0, 0);
        public double dualReverseDetectionDistance;

        //for heading or Atan2 as camera
        public string headingFromSource, headingFromSourceBak;

        public vec3 pivotAxlePos = new vec3(0, 0, 0);
        public vec3 steerAxlePos = new vec3(0, 0, 0);
        public vec3 toolPivotPos = new vec3(0, 0, 0);
        public vec3 toolPos = new vec3(0, 0, 0);
        public vec3 tankPos = new vec3(0, 0, 0);
        public vec2 hitchPos = new vec2(0, 0);

        //history
        public vec2 prevFix = new vec2(0, 0);
        public vec2 prevJumpFix = new vec2(0, 0);
        public vec2 prevDistFix = new vec2(0, 0);
        public vec2 lastReverseFix = new vec2(0, 0);

        //headings
        public double fixHeading = 0.0, camHeading = 0.0, smoothCamHeading = 0, gpsHeading = 10.0, prevGPSHeading = 0.0;

        //storage for the cos and sin of heading
        public double cosSectionHeading = 1.0, sinSectionHeading = 0.0;

        //how far travelled since last section was added, section points
        double sectionTriggerDistance = 0, contourTriggerDistance = 0, sectionTriggerStepDistance = 0, gridTriggerDistance = 0;

        public vec2 prevSectionPos = new vec2(0, 0);
        public vec2 prevContourPos = new vec2(0, 0);
        public vec2 prevGridPos = new vec2(0, 0);
        public int patchCounter = 0;

        public vec2 prevBoundaryPos = new vec2(0, 0);

        //Everything is so wonky at the start
        int startCounter = 0;

        //individual points for the flags in a list
        public List<CFlag> flagPts = new List<CFlag>();

        //tally counters for display
        //public double totalSquareMetersWorked = 0, totalUserSquareMeters = 0, userSquareMetersAlarm = 0;

        public double avgSpeed, previousSpeed;//for average speed
        public double crossTrackError;

        //youturn
        public double distancePivotToTurnLine = -2222;
        public double distanceToolToTurnLine = -2222;

        //the value to fill in you turn progress bar
        public int youTurnProgressBar = 0;

        //IMU 
        public double rollCorrectionDistance = 0;
        public double imuGPS_Offset, imuCorrected;

        //step position - slow speed spinner killer
        private int currentStepFix = 0;
        private const int totalFixSteps = 10;
        public vecFix2Fix[] stepFixPts = new vecFix2Fix[totalFixSteps];
        public double distanceCurrentStepFix = 0, distanceCurrentStepFixDisplay = 0, minHeadingStepDist = 1, startSpeed = 0.5;
        public double fixToFixHeadingDistance = 0, gpsMinimumStepDistance = 0.05;

        private double nowHz = 0;

        public bool isRTK_AlarmOn, isRTK_KillAutosteer;

        public double uncorrectedEastingGraph = 0;
        public double correctionDistanceGraph = 0;

        double frameTimeRough = 3;
        public double timeSliceOfLastFix = 0;

        public bool isMaxAngularVelocity = false;

        public int minSteerSpeedTimer = 0;

        private double _steerAngleScrollBar;
        public double steerAngleScrollBar
        {
            get => _steerAngleScrollBar;
            set
            {
                _steerAngleScrollBar = value;

                hsbarSteerAngle.Value = 400;

                if (_steerAngleScrollBar > 40) _steerAngleScrollBar = 40;
                if (_steerAngleScrollBar < -40) _steerAngleScrollBar = -40;
                btnResetSteerAngle.Text = _steerAngleScrollBar.ToString("N1");
                hsbarSteerAngle.Value = (int)(10 * _steerAngleScrollBar) + 400;
            }
        }

        //public vec2 jumpFix = new vec2(0, 0);
        //public double jumpDistance = 0, jumpDistanceMax;
        //public double jumpDistanceAlarm = 20;
        //public int jumpCounter = 0;

        public void UpdateFixPosition()
        {
            //swFrame.Stop();
            //Measure the frequency of the GPS updates
            timeSliceOfLastFix = (double)(swFrame.ElapsedTicks) / (double)System.Diagnostics.Stopwatch.Frequency;
            swFrame.Restart();

            //get Hz from timeslice
            nowHz = 1 / timeSliceOfLastFix;
            if (nowHz > 35) nowHz = 35;
            if (nowHz < 5) nowHz = 5;

            //simple comp filter
            gpsHz = 0.98 * gpsHz + 0.02 * nowHz;

            if (timerSim.Enabled) gpsHz = 20;

            //Initialization counter
            startCounter++;

            pn.AverageTheSpeed();

            distanceCurrentStepFixDisplay = glm.Distance(prevDistFix, pn.fix);
            distanceCurrentStepFixDisplay *= 100;
            prevDistFix = pn.fix;

            if (!isFirstHeadingSet)
            {
                prevFix = pn.fix;
                lastReverseFix = pn.fix;
            }

            #region Heading
            switch (headingFromSource)
            {
                //calculate current heading only when moving, otherwise use last
                case "Fix":
                    
                    if (isFirstHeadingSet)
                        AddRoll();

                    #region Fix Heading

                    //how far since last fix
                    distanceCurrentStepFix = glm.Distance(stepFixPts[0], pn.fix);

                    if (distanceCurrentStepFix > gpsMinimumStepDistance)// 0.1 or 0.05 
                    {
                        //save a copy of previous for jump test
                        //jumpFix.easting = stepFixPts[0].easting; jumpFix.northing = stepFixPts[0].northing;

                        if ((fd.distanceUser += distanceCurrentStepFix) > 9999) fd.distanceUser = 0;

                        double minFixHeadingDistSquared = minHeadingStepDist * minHeadingStepDist;
                        fixToFixHeadingDistance = 0;

                        for (int i = 0; i < totalFixSteps; i++)
                        {
                            if (stepFixPts[i].isSet)
                            {
                                fixToFixHeadingDistance = glm.DistanceSquared(stepFixPts[i], pn.fix);
                                currentStepFix = i;

                                if (fixToFixHeadingDistance > minFixHeadingDistSquared)
                                {
                                    break;
                                }
                            }
                            else break;
                        }

                        if (fixToFixHeadingDistance > minFixHeadingDistSquared * 0.5)//1 or 0.5 meter * 0.5??
                        {
                            gpsHeading = Math.Atan2(pn.fix.easting - stepFixPts[currentStepFix].easting,
                                                    pn.fix.northing - stepFixPts[currentStepFix].northing);

                            if (gpsHeading < 0) gpsHeading += glm.twoPI;

                            if (!isFirstHeadingSet)
                            {
                                #region Start

                                //set the imu to gps heading offset
                                if (ahrs.imuHeading != 99999)
                                    IMUFusion(1);

                                if (ahrs.imuRoll != 88888)
                                {
                                    //change for roll to the right is positive times -1
                                    rollCorrectionDistance = Math.Tan(glm.toRadians((ahrs.imuRoll))) * -vehicle.antennaHeight;

                                    // roll to left is positive  **** important!!
                                    // not any more - April 30, 2019 - roll to right is positive Now! Still Important
                                    for (int i = 0; i < 3; i++)
                                    {
                                        stepFixPts[i].easting = (Math.Cos(-gpsHeading) * rollCorrectionDistance) + stepFixPts[i].easting;
                                        stepFixPts[i].northing = (Math.Sin(-gpsHeading) * rollCorrectionDistance) + stepFixPts[i].northing;
                                    }
                                }

                                isFirstHeadingSet = true;
                                TimedMessageBox(2000, "Direction Reset", "Forward is Set");
                                Log.EventWriter("Forward Is Set");
                                #endregion
                            }
                            else
                            {
                                ////what is angle between the last valid heading and one just now
                                double delta = Math.Abs(Math.PI - Math.Abs(Math.Abs(gpsHeading - fixHeading) - Math.PI));

                                isReverse = ahrs.isReverseOn && delta > glm.PIBy2;
                                
                                if (isReverse)
                                {
                                    gpsHeading -= glm.toRadians(vehicle.antennaPivot / 1
                                        * mc.actualSteerAngleDegrees * Properties.Settings.Default.setGPS_reverseComp);
                                }
                                else
                                    gpsHeading -= glm.toRadians(vehicle.antennaPivot / 1
                                        * mc.actualSteerAngleDegrees * Properties.Settings.Default.setGPS_forwardComp);
                                
                                if (gpsHeading < 0) gpsHeading += glm.twoPI;
                                else if (gpsHeading >= glm.twoPI) gpsHeading -= glm.twoPI;
                            }
                        }

                        //save current fix and set as valid
                        for (int i = totalFixSteps - 1; i > 0; i--) stepFixPts[i] = stepFixPts[i - 1];
                        stepFixPts[0].easting = pn.fix.easting;
                        stepFixPts[0].northing = pn.fix.northing;
                        stepFixPts[0].isSet = true;
                    }

                    if (isFirstHeadingSet)
                    {
                        if (ahrs.imuHeading != 99999)//imu on board
                        {
                            IMUFusion(2);
                        }
                        else
                            fixHeading = (isReverse ? Math.PI : 0) + gpsHeading;
                    }

                    #endregion
                    break;

                case "VTG":
                    {
                        isFirstHeadingSet = true;
                        if (avgSpeed > 1)
                        {
                            //use NMEA headings for camera and tractor graphic
                            gpsHeading = glm.toRadians(pn.headingTrue);
                        }

                        //grab the most current fix to last fix distance
                        distanceCurrentStepFix = glm.Distance(pn.fix, prevFix);
                        if (distanceCurrentStepFix > 0.1)
                        {
                            if ((fd.distanceUser += distanceCurrentStepFix) > 9999) fd.distanceUser = 0;
                            prevFix = pn.fix;
                        }

                        //an IMU with heading correction, add the correction
                        if (ahrs.imuHeading != 99999)
                            IMUFusion(3);
                        else
                            fixHeading = (isReverse ? Math.PI : 0) + gpsHeading;

                        AddRoll();
                        break;
                    }

                case "Dual":
                    {
                        isFirstHeadingSet = true;
                        //use Dual Antenna heading for camera and tractor graphic
                        fixHeading = gpsHeading = glm.toRadians(pn.headingTrueDual);

                        distanceCurrentStepFix = glm.Distance(pn.fix, prevFix);

                        if (distanceCurrentStepFix > 0.1)
                        {
                            if ((fd.distanceUser += distanceCurrentStepFix) > 9999) fd.distanceUser = 0;
                            prevFix = pn.fix;
                        }

                        if (glm.Distance(lastReverseFix, pn.fix) > dualReverseDetectionDistance)
                        {
                            //most recent heading
                            double newHeading = Math.Atan2(pn.fix.easting - lastReverseFix.easting,
                                                        pn.fix.northing - lastReverseFix.northing);

                            if (newHeading < 0) newHeading += glm.twoPI;

                            //what is angle between the last reverse heading and current dual heading
                            double delta = Math.Abs(Math.PI - Math.Abs(Math.Abs(newHeading - fixHeading) - Math.PI));

                            //are we going backwards
                            isReverse = delta > 2;

                            //save for next meter check
                            lastReverseFix = pn.fix;
                        }

                        AddRoll();
                        break;
                    }

                default:
                    break;
            }
            
            SmoothCamera();
            TheRest();

            if (fixHeading > glm.twoPI) fixHeading -= glm.twoPI;
            if (fixHeading < 0) fixHeading += glm.twoPI;

            //vec2 ptA = new vec2(jumpFix.easting - (Math.Sin(gpsHeading) * 10), jumpFix.northing - (Math.Cos(gpsHeading) * 10));
            //vec2 ptB = new vec2(jumpFix.easting + (Math.Sin(gpsHeading) * 10), jumpFix.northing + (Math.Cos(gpsHeading) * 10));

            //double dx = ptB.easting - ptA.easting;
            ////z2-z1
            //double dy = ptB.northing - ptA.northing;

            ////how far from current AB Line is fix
            //jumpDistance = ((dy * pn.fix.easting) - (dx * pn.fix.northing) 
            //                + (ptB.easting * ptA.northing) - (ptB.northing * ptA.easting))
            //                / Math.Sqrt((dy * dy) + (dx * dx));

            //jumpDistance = Math.Abs(jumpDistance) * 100;

            //if (jumpDistance > jumpDistanceMax) jumpDistanceMax = jumpDistance;

            //if (jumpCounter++ > 200)
            //{
            //    jumpDistanceMax = jumpCounter = 0;
            //    lblJumpDistanceMax.Text = "*";
            //}

            //if (jumpDistance > 200) jumpDistance = 0;

            //if (isFirstHeadingSet && jumpDistanceAlarm > 0 && jumpDistance > jumpDistanceAlarm)
            //{
            //    Log.EventWriter(": " + jumpDistance.ToString("N0") + " cm");

            //    if (isBtnAutoSteerOn)
            //    {
            //        btnAutoSteer.PerformClick();
            //        TimedMessageBox(3000, gStr.Get(gs.gsAutoSteer, "Big Jump in GPS position:" + jumpDistance.ToString("N0") + " cm");
            //        Log.EventWriter("Autosteer Off, Jump in GPS position: " + jumpDistance.ToString("N0") + " cm");
            //    }

            //}

            //jumpFix.easting = pn.fix.easting;
            //jumpFix.northing = pn.fix.northing;

            #endregion

            #region Corrected Position
            double latitud;
            double longitud;

            pn.ConvertLocalToWGS84(pn.fix.northing, pn.fix.easting, out latitud, out longitud);
            byte[] correctedPosition = new byte[30];
            correctedPosition[0] = 0x80;
            correctedPosition[1] = 0x81;
            correctedPosition[2] = 0x7F;
            correctedPosition[3] = 0x64;
            correctedPosition[4] = 24;
            Buffer.BlockCopy(BitConverter.GetBytes(longitud), 0, correctedPosition, 5, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(latitud), 0, correctedPosition, 13, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(glm.toDegrees(gpsHeading)), 0, correctedPosition, 21, 8);
            SendPgnToLoop(correctedPosition);
            #endregion

            #region AutoSteer

            //preset the values
            guidanceLineDistanceOff = double.NaN;

            if (ct.isContourBtnOn)
            {
                //quick hack will change later
                trk.currentGuidanceTrack = ct.ctList;
            }

            //like normal
            if (trk.gArr.Count > 0 && trk.idx > -1)
            {
                //build new current ref line if required
                trk.GetDistanceFromRefTrack(pivotAxlePos);

            }

            if (trk.currentGuidanceTrack.Count > 0)
            {
                gyd.Guidance(pivotAxlePos, steerAxlePos, yt.isYouTurnTriggered, yt.isYouTurnTriggered ? yt.ytList : trk.currentGuidanceTrack);
            }
            else
            {
                //invalid distance so tell AS module
                gyd.distanceFromCurrentLine = 0;
                guidanceLineDistanceOff = double.NaN;
            }
            

            // autosteer at full speed of updates

            // If Drive button off - normal autosteer 
            if (!vehicle.isInFreeDriveMode)
            {
                //fill up0 the appropriate arrays with new values
                PGN_254.pgn[PGN_254.speedHi] = unchecked((byte)((int)(Math.Abs(avgSpeed) * 10.0) >> 8));
                PGN_254.pgn[PGN_254.speedLo] = unchecked((byte)((int)(Math.Abs(avgSpeed) * 10.0)));
                //mc.machineControlData[mc.cnSpeed] = mc.autoSteerData[mc.sdSpeed];

                //convert to cm from mm and divide by 2 - lightbar
                int distanceX2;
                if (!isBtnAutoSteerOn || double.IsNaN(guidanceLineDistanceOff))
                {
                    distanceX2 = 255;
                    PGN_254.pgn[PGN_254.status] = 0;
                }
                else
                {
                    distanceX2 = (int)((guidanceLineDistanceOff * glm.m2InchOrCm) / lightbarCmPerPixel);

                    if (distanceX2 < -127) distanceX2 = -127;
                    else if (distanceX2 > 127) distanceX2 = 127;
                    distanceX2 += 127;
                    PGN_254.pgn[PGN_254.status] = 1;
                }

                PGN_254.pgn[PGN_254.lineDistance] = unchecked((byte)distanceX2);

                if (!timerSim.Enabled)
                {
                    if (isBtnAutoSteerOn && avgSpeed > vehicle.maxSteerSpeed)
                    {
                        btnAutoSteer.PerformClick();
                    }

                    if (isBtnAutoSteerOn && avgSpeed < vehicle.minSteerSpeed)
                    {
                        minSteerSpeedTimer++;
                        if (minSteerSpeedTimer > 80)
                        {
                            btnAutoSteer.PerformClick();
                            TimedMessageBox(3000, "AutoSteer Disabled", "Below Minimum Safe Steering Speed: " + (vehicle.minSteerSpeed * glm.kmhToMphOrKmh).ToString("N1") + glm.unitsKmhMph);
                            Log.EventWriter("Steer Off, Below Min Steering Speed");
                        }
                    }
                    else
                    {
                        minSteerSpeedTimer = 0;
                    }
                }

                //for now if backing up, turn off autosteer
                if (!isSteerInReverse && isReverse)
                {
                    PGN_254.pgn[PGN_254.status] = 0;
                }

                // delay on dead zone.
                if (PGN_254.pgn[PGN_254.status] == 1 && !isReverse
                    && Math.Abs(guidanceLineSteerAngle - mc.actualSteerAngleDegrees) < vehicle.deadZoneHeading)
                {
                    if (vehicle.deadZoneDelayCounter > vehicle.deadZoneDelay)
                    {
                        vehicle.isInDeadZone = true;
                    }
                }
                else
                {
                    vehicle.deadZoneDelayCounter = 0;
                    vehicle.isInDeadZone = false;
                }

                if (!vehicle.isInDeadZone)
                {
                    var angleX100 = (Int16)(guidanceLineSteerAngle * 100);

                    PGN_254.pgn[PGN_254.steerAngleHi] = unchecked((byte)(angleX100 >> 8));
                    PGN_254.pgn[PGN_254.steerAngleLo] = unchecked((byte)(angleX100));
                }

                if (isGPSToolActive)
                {
                    PGN_233.pgn[PGN_233.speedHi] = unchecked((byte)((int)(Math.Abs(avgSpeed) * 10.0) >> 8));
                    PGN_233.pgn[PGN_233.speedLo] = unchecked((byte)((int)(Math.Abs(avgSpeed) * 10.0)));

                    var distX1000 = (Int16)(gyd.distanceFromCurrentLineTool * 1000);
                    PGN_233.pgn[PGN_233.xteHi] = unchecked((byte)(distX1000 >> 8));
                    PGN_233.pgn[PGN_233.xteLo] = unchecked((byte)(distX1000));
                    
                    if (!vehicle.isInFreeDriveMode)
                    {
                        if (PGN_254.pgn[PGN_254.status] == 1) PGN_233.pgn[PGN_233.status] = 1;
                        else PGN_233.pgn[PGN_233.status] = 0;
                    }
                    else
                    {
                        PGN_233.pgn[PGN_233.status] = 0;
                    }

                    //send to tool steer
                    SendPgnToLoopTool(PGN_233.pgn);
                }
            }

            else //Drive button is on
            {
                //fill up the auto steer array with free drive values
                PGN_254.pgn[PGN_254.speedHi] = unchecked((byte)((int)(80) >> 8));
                PGN_254.pgn[PGN_254.speedLo] = unchecked((byte)((int)(80)));

                //turn on status to operate
                PGN_254.pgn[PGN_254.status] = 1;

                //send the steer angle
                var angleX100 = (Int16)(vehicle.driveFreeSteerAngle * 100);

                PGN_254.pgn[PGN_254.steerAngleHi] = unchecked((byte)(angleX100 >> 8));
                PGN_254.pgn[PGN_254.steerAngleLo] = unchecked((byte)(angleX100));
            }

            //out serial to autosteer module  //indivdual classes load the distance and heading deltas 
            SendPgnToLoop(PGN_254.pgn);



            //for average cross track error
            if (!double.IsNaN(guidanceLineDistanceOff))
            {
                crossTrackError = crossTrackError * 0.90 + Math.Abs(guidanceLineDistanceOff) * 0.1;
            }

            #endregion

            #region Youturn

            //if an outer boundary is set, then apply critical stop logic
            if (bnd.bndList.Count > 0)
            {
                //check if inside all fence
                if (!yt.isYouTurnBtnOn)
                {
                    mc.isOutOfBounds = !bnd.IsPointInsideFenceArea(pivotAxlePos);
                }
                else //Youturn is on
                {
                    bool isInTurnBounds = bnd.IsPointInsideTurnArea(pivotAxlePos) != -1;
                    //Are we inside outer and outside inner all turn boundaries, no turn creation problems
                    //if we are too much off track > 1.3m, kill the diagnostic creation, start again
                    //if (!yt.isYouTurnTriggered) 
                    if (isInTurnBounds)
                    {
                        mc.isOutOfBounds = false;
                        //now check to make sure we are not in an inner turn boundary - drive thru is ok
                        if (yt.youTurnPhase != 10)
                        {
                            if (crossTrackError > 1)
                            {
                                yt.ResetCreatedYouTurn();
                            }
                            else
                            {
                                yt.BuildCurveDubinsYouTurn();
                            }

                            if (yt.uTurnStyle == 0 && yt.youTurnPhase == 10)
                            {
                                yt.SmoothYouTurn(6);
                            }

                            if (yt.isTurnCreationTooClose && !yt.turnTooCloseTrigger)
                            {
                                yt.turnTooCloseTrigger = true;
                                if (sounds.isTurnSoundOn)
                                {
                                    sounds.sndUTurnTooClose.Play();
                                    Log.EventWriter("U Turn Creation Failure");
                                }
                            }
                        }
                        else if (yt.ytList.Count > 5)//wait to trigger the actual turn since its made and waiting
                        {
                            //distance from current pivot or steer to first point of youturn pattern
                            if (isStanleyUsed) distancePivotToTurnLine = glm.Distance(yt.ytList[2], steerAxlePos);
                            else distancePivotToTurnLine = glm.Distance(yt.ytList[2], pivotAxlePos);

                            if ((distancePivotToTurnLine <= 20.0) && (distancePivotToTurnLine >= 18.0) && !yt.isYouTurnTriggered)

                                if (!sounds.isBoundAlarming)
                                {
                                    if (sounds.isTurnSoundOn) sounds.sndBoundaryAlarm.Play();
                                    sounds.isBoundAlarming = true;
                                }

                            //if we are close enough to pattern, trigger.
                            if ((distancePivotToTurnLine <= 1 || (!isStanleyUsed && glm.Distance(yt.ytList[2], gyd.goalPoint) <= 1.0)) && !yt.isYouTurnTriggered)
                            {
                                yt.YouTurnTrigger();
                                sounds.isBoundAlarming = false;
                            }

                            //if (isBtnAutoSteerOn && guidanceLineDistanceOff > 300 && !yt.isYouTurnTriggered)
                            //{
                            //    yt.ResetCreatedYouTurn();
                            //}
                        }
                    }
                    else
                    {
                        if (!yt.isYouTurnTriggered)
                        {
                            yt.ResetCreatedYouTurn();
                            mc.isOutOfBounds = !bnd.IsPointInsideFenceArea(pivotAxlePos);
                        }

                    }

                    //}
                    //// here is stop logic for out of bounds - in an inner or out the outer turn border.
                    //else
                    //{
                    //    //mc.isOutOfBounds = true;
                    //    if (isBtnAutoSteerOn)
                    //    {
                    //        if (yt.isYouTurnBtnOn)
                    //        {
                    //            yt.ResetCreatedYouTurn();
                    //            //sim.stepDistance = 0 / 17.86;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        yt.isTurnCreationTooClose = false;
                    //    }

                    //}
                }
            }
            else
            {
                mc.isOutOfBounds = false;
            }

            #endregion

            //do section control
            oglBack.Refresh();

            //stop the timer and calc how long it took to do calcs and draw
            frameTimeRough = (double)(swFrame.ElapsedTicks * 1000) / (double)System.Diagnostics.Stopwatch.Frequency;

            if (frameTimeRough > 80) frameTimeRough = 80;
            frameTime = frameTime * 0.96 + frameTimeRough * 0.04;

            //Don't care about time from here on - update main window
            oglMain.Refresh();

            //Albin - get the section control started here already. 
            //end of UppdateFixPosition
        }

        private void AddRoll()
        {
            uncorrectedEastingGraph = pn.fix.easting;

            if (ahrs.imuRoll != 88888 && vehicle.antennaHeight != 0)
            {
                //change for roll to the right is positive times -1
                rollCorrectionDistance = Math.Sin(glm.toRadians((ahrs.imuRoll))) * -vehicle.antennaHeight;
                correctionDistanceGraph = rollCorrectionDistance;

                pn.fix.easting = (Math.Cos(-fixHeading) * rollCorrectionDistance) + pn.fix.easting;
                pn.fix.northing = (Math.Sin(-fixHeading) * rollCorrectionDistance) + pn.fix.northing;
            }
        }

        private void IMUFusion(int idx)
        {
            // IMU Fusion with heading correction, add the correction
            //current gyro angle in radians
            double imuHeading = glm.toRadians(ahrs.imuHeading);//0-2pi

            //Difference between the IMU heading and the GPS heading
            double gyroDelta = (imuHeading - imuGPS_Offset) - gpsHeading;
            if (isReverse) gyroDelta += Math.PI;
            //gyroDelta = 0;
            while (gyroDelta < -Math.PI) gyroDelta += glm.twoPI;
            while (gyroDelta > Math.PI) gyroDelta -= glm.twoPI;

            if (idx == 1)
            {
                //line up imu with gps
                imuGPS_Offset += gyroDelta;
            }
            else if (idx == 2)//stepfix
            {
                //move the offset to line up imu with gps
                //if (!isReverseWithIMU)
                    imuGPS_Offset += gyroDelta * ahrs.fusionWeight;
                //else
                //    imuGPS_Offset += gyroDelta * 0.02;
            }
            else if (idx == 3)//VTG
            {
                //if the gyro and last corrected fix is < 10 degrees, super low pass for gps
                if (Math.Abs(gyroDelta) < 0.18)
                {
                    //a bit of delta and add to correction to current gyro
                    imuGPS_Offset += gyroDelta * 0.1;
                }
                else
                {
                    //a bit of delta and add to correction to current gyro
                    imuGPS_Offset += gyroDelta * 0.2;
                }
            }

            if (imuGPS_Offset > Math.PI) imuGPS_Offset -= glm.twoPI;
            if (imuGPS_Offset < -Math.PI) imuGPS_Offset += glm.twoPI;

            //determine the Corrected heading based on gyro and GPS
            imuCorrected = imuHeading - imuGPS_Offset;
            if (imuCorrected < 0) imuCorrected += glm.twoPI;
            if (imuCorrected > glm.twoPI) imuCorrected -= glm.twoPI;

            fixHeading = imuCorrected;
        }

        private void SmoothCamera()
        {
            double camDelta = fixHeading - smoothCamHeading;

            if (camDelta < 0) camDelta += glm.twoPI;
            else if (camDelta > glm.twoPI) camDelta -= glm.twoPI;

            //calculate delta based on circular data problem 0 to 360 to 0, clamp to +- 2 Pi
            if (camDelta >= -glm.PIBy2 && camDelta <= glm.PIBy2) camDelta *= -1.0;
            else
            {
                if (camDelta > glm.PIBy2) { camDelta = glm.twoPI - camDelta; }
                else { camDelta = (glm.twoPI + camDelta) * -1.0; }
            }
            if (camDelta > glm.twoPI) camDelta -= glm.twoPI;
            else if (camDelta < -glm.twoPI) camDelta += glm.twoPI;

            smoothCamHeading -= camDelta * camera.camSmoothFactor;

            if (smoothCamHeading > glm.twoPI) smoothCamHeading -= glm.twoPI;
            else if (smoothCamHeading < -glm.twoPI) smoothCamHeading += glm.twoPI;

            camHeading = glm.toDegrees(smoothCamHeading);
        }

        private void TheRest()
        {
            CalculateTrailingAndTBTHitch();

            //positions and headings 
            CalculateSectionTriggerStepDistance();

            //calculate lookahead at full speed, no sentence misses
            CalculateSectionLookAhead(toolPos.northing, toolPos.easting, cosSectionHeading, sinSectionHeading);

            //To prevent drawing high numbers of triangles, determine and test before drawing vertex
            sectionTriggerDistance = glm.Distance(pivotAxlePos, prevSectionPos);
            contourTriggerDistance = glm.Distance(pivotAxlePos, prevContourPos);
            gridTriggerDistance = glm.DistanceSquared(pivotAxlePos, prevGridPos);

            if ( isLogElevation && gridTriggerDistance > 2.9 && patchCounter !=0 && isFieldStarted)
            {
                //grab fix and elevation
                sbElevationString.Append(
                      pn.latitude.ToString("N7", CultureInfo.InvariantCulture) + ","
                    + pn.longitude.ToString("N7", CultureInfo.InvariantCulture) + ","
                    + Math.Round((pn.altitude - vehicle.antennaHeight),3).ToString(CultureInfo.InvariantCulture) + ","
                    + pn.fixQuality.ToString(CultureInfo.InvariantCulture) + ","
                    + pn.fix.easting.ToString("N2", CultureInfo.InvariantCulture) + ","
                    + pn.fix.northing.ToString("N2", CultureInfo.InvariantCulture) + ","
                    + pivotAxlePos.heading.ToString("N3", CultureInfo.InvariantCulture) + ","
                    + Math.Round(ahrs.imuRoll,3).ToString(CultureInfo.InvariantCulture) + 
                    "\r\n");

                prevGridPos.easting = pivotAxlePos.easting;
                prevGridPos.northing = pivotAxlePos.northing;
            }

            //contour points
            if (isFieldStarted &&(contourTriggerDistance > tool.contourWidth 
                || contourTriggerDistance > sectionTriggerStepDistance))
            {
                AddContourPoints();
            }

            //section on off and points
            if (sectionTriggerDistance > sectionTriggerStepDistance && isFieldStarted)
            {
                AddSectionOrPathPoints();
            }

            //test if travelled far enough for new boundary point
            if (bnd.isOkToAddPoints)
            {
                double boundaryDistance = glm.Distance(pivotAxlePos, prevBoundaryPos);
                if (boundaryDistance > 1) AddBoundaryPoint();
            }
        }

        //all the hitch, pivot, section, trailing hitch, headings and fixes
        private void CalculateTrailingAndTBTHitch()
        {
            //translate from pivot position to steer axle and pivot axle position
            //translate world to the pivot axle
            pivotAxlePos.easting = pn.fix.easting - (Math.Sin(fixHeading) * vehicle.antennaPivot);
            pivotAxlePos.northing = pn.fix.northing - (Math.Cos(fixHeading) * vehicle.antennaPivot);
            pivotAxlePos.heading = fixHeading;

            if (vehicle.antennaOffset != 0)
            {
                pivotAxlePos.easting += Math.Cos(fixHeading) * vehicle.antennaOffset;
                pivotAxlePos.northing -= Math.Sin(fixHeading) * vehicle.antennaOffset;
            }

            steerAxlePos.easting = pivotAxlePos.easting + (Math.Sin(fixHeading) * vehicle.wheelbase*0.6);
            steerAxlePos.northing = pivotAxlePos.northing + (Math.Cos(fixHeading) * vehicle.wheelbase*0.6);
            steerAxlePos.heading = fixHeading;

            //guidance look ahead distance based on time or tool width at least 
            
            double guidanceLookDist = (Math.Max(tool.width * 0.5, avgSpeed * 0.277777 * guidanceLookAheadTime));
            guidanceLookPos.easting = pivotAxlePos.easting + (Math.Sin(fixHeading) * guidanceLookDist);
            guidanceLookPos.northing = pivotAxlePos.northing + (Math.Cos(fixHeading) * guidanceLookDist);
            
            //determine where the rigid vehicle hitch ends
            hitchPos.easting = pivotAxlePos.easting + Math.Sin(fixHeading) * tool.hitchLength;
            hitchPos.northing = pivotAxlePos.northing + Math.Cos(fixHeading) * tool.hitchLength;

            //tool attached via a trailing hitch
            if (isGPSToolActive && tool.isToolTrailing && !tool.isToolTBT && !timerSim.Enabled)
            {
                tankPos.heading = fixHeading;
                tankPos.easting = hitchPos.easting;
                tankPos.northing = hitchPos.northing;

                toolPivotPos.easting = pnTool.fix.easting * 0.5 + toolPivotPos.easting * 0.5;
                toolPivotPos.northing = pnTool.fix.northing * 0.5 + toolPivotPos.northing * 0.5;

                toolPivotPos.heading = Math.Atan2(tankPos.easting - toolPivotPos.easting, tankPos.northing - toolPivotPos.northing);

                if (toolPivotPos.heading < 0) toolPivotPos.heading += glm.twoPI;

                toolPos.heading = toolPivotPos.heading;
                toolPos.easting = toolPivotPos.easting +
                    (Math.Sin(toolPivotPos.heading) * (tool.trailingToolToPivotLength));
                toolPos.northing = toolPivotPos.northing +
                    (Math.Cos(toolPivotPos.heading) * (tool.trailingToolToPivotLength));
            }

            else if (tool.isToolTrailing)
            {
                double over;
                if (tool.isToolTBT)
                {
                    //Torriem rules!!!!! Oh yes, this is all his. Thank-you
                    if (distanceCurrentStepFix != 0)
                    {
                        tankPos.heading = Math.Atan2(hitchPos.easting - tankPos.easting, hitchPos.northing - tankPos.northing);
                        if (tankPos.heading < 0) tankPos.heading += glm.twoPI;
                    }

                    ////the tool is seriously jacknifed or just starting out so just spring it back.
                    over = Math.Abs(Math.PI - Math.Abs(Math.Abs(tankPos.heading - fixHeading) - Math.PI));

                    if (over < 2.0 && startCounter > 50)
                    {
                        tankPos.easting = hitchPos.easting + (Math.Sin(tankPos.heading) * (tool.tankTrailingHitchLength));
                        tankPos.northing = hitchPos.northing + (Math.Cos(tankPos.heading) * (tool.tankTrailingHitchLength));
                    }

                    //criteria for a forced reset to put tool directly behind vehicle
                    if (over > 2.0 | startCounter < 51)
                    {
                        tankPos.heading = fixHeading;
                        tankPos.easting = hitchPos.easting + (Math.Sin(tankPos.heading) * (tool.tankTrailingHitchLength));
                        tankPos.northing = hitchPos.northing + (Math.Cos(tankPos.heading) * (tool.tankTrailingHitchLength));
                    }
                }
                else
                {
                    tankPos.heading = fixHeading;
                    tankPos.easting = hitchPos.easting;
                    tankPos.northing = hitchPos.northing;
                }

                //Torriem rules!!!!! Oh yes, this is all his. Thank-you
                if (distanceCurrentStepFix != 0)
                {
                    toolPivotPos.heading = Math.Atan2(tankPos.easting - toolPivotPos.easting, tankPos.northing - toolPivotPos.northing);
                    if (toolPivotPos.heading < 0) toolPivotPos.heading += glm.twoPI;
                }

                ////the tool is seriously jacknifed or just starting out so just spring it back.
                over = Math.Abs(Math.PI - Math.Abs(Math.Abs(toolPivotPos.heading - tankPos.heading) - Math.PI));

                if (over < 1.9 && startCounter > 50)
                {
                    toolPivotPos.easting = tankPos.easting + (Math.Sin(toolPivotPos.heading) * (tool.trailingHitchLength));
                    toolPivotPos.northing = tankPos.northing + (Math.Cos(toolPivotPos.heading) * (tool.trailingHitchLength));
                }

                //criteria for a forced reset to put tool directly behind vehicle
                if (over > 1.9 | startCounter < 51)
                {
                    toolPivotPos.heading = tankPos.heading;
                    toolPivotPos.easting = tankPos.easting + (Math.Sin(toolPivotPos.heading) * (tool.trailingHitchLength));
                    toolPivotPos.northing = tankPos.northing + (Math.Cos(toolPivotPos.heading) * (tool.trailingHitchLength));
                }

                toolPos.heading = toolPivotPos.heading;
                toolPos.easting = tankPos.easting + 
                    (Math.Sin(toolPivotPos.heading) * (tool.trailingHitchLength - tool.trailingToolToPivotLength));
                toolPos.northing = tankPos.northing + 
                    (Math.Cos(toolPivotPos.heading) * (tool.trailingHitchLength - tool.trailingToolToPivotLength));
            }

            //rigidly connected to vehicle
            else
            {
                toolPivotPos.heading = fixHeading;
                toolPivotPos.easting = hitchPos.easting;
                toolPivotPos.northing = hitchPos.northing;

                toolPos.heading = fixHeading;
                toolPos.easting = hitchPos.easting;
                toolPos.northing = hitchPos.northing;
            }
        }


        //used to increase triangle countExit when going around corners, less on straight
        private void CalculateSectionTriggerStepDistance()
        {
            double distance = tool.width*0.75;
            if (distance > 6) distance = 6;

            double twist = 0.2;
            //whichever is less
            if (tool.farLeftSpeed < tool.farRightSpeed)
            {
                twist = tool.farLeftSpeed * (tool.width / 50) / tool.farRightSpeed * (50/ tool.width);
            }
            else
            {
                twist = tool.farRightSpeed * (tool.width / 50) / tool.farLeftSpeed * (50 / tool.width);
            }

            twist *= twist;
            if (twist < 0.2) twist = 0.2;
            sectionTriggerStepDistance = distance * twist;

            if (sectionTriggerStepDistance < 1.5) sectionTriggerStepDistance = 1.5;

            //finally fixed distance for making a curve line
            if (trk.isRecordingCurveTrack) sectionTriggerStepDistance *= 0.5;

            //precalc the sin and cos of heading * -1
            sinSectionHeading = Math.Sin(-toolPivotPos.heading);
            cosSectionHeading = Math.Cos(-toolPivotPos.heading);
        }

        //calculate the extreme tool left, right velocities, each section lookahead, and whether or not its going backwards
        public void CalculateSectionLookAhead(double northing, double easting, double cosHeading, double sinHeading)
        {
            //calculate left side of section 1
            vec2 left = new vec2();
            vec2 right = left;
            double leftSpeed = 0, rightSpeed = 0;

            //speed max for section kmh*0.277 to m/s * 10 cm per pixel * 1.7 max speed
            double meterPerSecPerPixel = Math.Abs(avgSpeed) * 4.5;

            //now loop all the section rights and the one extreme left
            for (int j = 0; j < tool.numOfSections; j++)
            {
                if (j == 0)
                {
                    //only one first left point, the rest are all rights moved over to left
                    section[j].leftPoint = new vec2(cosHeading * (section[j].positionLeft) + easting,
                                       sinHeading * (section[j].positionLeft) + northing);

                    left = section[j].leftPoint - section[j].lastLeftPoint;

                    //save a copy for next time
                    section[j].lastLeftPoint = section[j].leftPoint;

                    //get the speed for left side only once

                    leftSpeed = left.GetLength() * gpsHz * 10;
                    if (leftSpeed > meterPerSecPerPixel) leftSpeed = meterPerSecPerPixel;
                }
                else
                {
                    //right point from last section becomes this left one
                    section[j].leftPoint = section[j - 1].rightPoint;
                    left = section[j].leftPoint - section[j].lastLeftPoint;

                    //save a copy for next time
                    section[j].lastLeftPoint = section[j].leftPoint;
                    
                    //Save the slower of the 2
                    if (leftSpeed > rightSpeed) leftSpeed = rightSpeed;                    
                }

                section[j].rightPoint = new vec2(cosHeading * (section[j].positionRight) + easting,
                                    sinHeading * (section[j].positionRight) + northing);

                //now we have left and right for this section
                right = section[j].rightPoint - section[j].lastRightPoint;

                //save a copy for next time
                section[j].lastRightPoint = section[j].rightPoint;

                //grab vector length and convert to meters/sec/10 pixels per meter                
                rightSpeed = right.GetLength() * gpsHz * 10;
                if (rightSpeed > meterPerSecPerPixel) rightSpeed = meterPerSecPerPixel;

                //Is section outer going forward or backward
                double head = left.HeadingXZ();

                if (head < 0) head += glm.twoPI;

                if (Math.PI - Math.Abs(Math.Abs(head - toolPivotPos.heading) - Math.PI) > glm.PIBy2)
                {
                    if (leftSpeed > 0) leftSpeed *= -1;
                }

                head = right.HeadingXZ();
                if (head < 0) head += glm.twoPI;
                if (Math.PI - Math.Abs(Math.Abs(head - toolPivotPos.heading) - Math.PI) > glm.PIBy2)
                {
                    if (rightSpeed > 0) rightSpeed *= -1;
                }

                double sped = 0;
                //save the far left and right speed in m/sec averaged over 20%
                if (j==0)
                {
                    sped = (leftSpeed * 0.1);
                    if (sped < 0.1) sped = 0.1;
                    tool.farLeftSpeed = tool.farLeftSpeed * 0.7 + sped * 0.3;
                }
                if (j == tool.numOfSections - 1)
                {
                    sped = (rightSpeed * 0.1);
                    if (sped < 0.1) sped = 0.1;
                    tool.farRightSpeed = tool.farRightSpeed * 0.7 + sped * 0.3;
                }

                //choose fastest speed and filter
                if (leftSpeed > rightSpeed)
                {
                    sped = leftSpeed;
                    leftSpeed = rightSpeed;
                }
                else sped = rightSpeed;
                section[j].speedPixels = section[j].speedPixels * 0.7 + sped * 0.3;
            }
        }

        //perimeter and boundary point generation
        public void AddBoundaryPoint()
        {
            //save the north & east as previous
            prevBoundaryPos.easting = pivotAxlePos.easting;
            prevBoundaryPos.northing = pivotAxlePos.northing;

            //build the boundary line

            if (bnd.isOkToAddPoints && (!bnd.isRecFenceWhenSectionOn || (bnd.isRecFenceWhenSectionOn && (manualBtnState == btnStates.On || autoBtnState == btnStates.Auto))))
            {
                if (bnd.isDrawAtPivot)
                {
                    if (bnd.isDrawRightSide)
                    {
                        //Right side
                        vec3 point = new vec3(
                            pivotAxlePos.easting + (Math.Sin(pivotAxlePos.heading - glm.PIBy2) * -bnd.createFenceOffset),
                            pivotAxlePos.northing + (Math.Cos(pivotAxlePos.heading - glm.PIBy2) * -bnd.createFenceOffset),
                            pivotAxlePos.heading);
                        bnd.fenceBeingMadePts.Add(point);
                    }

                    //draw on left side
                    else
                    {
                        //Right side
                        vec3 point = new vec3(
                            pivotAxlePos.easting + (Math.Sin(pivotAxlePos.heading - glm.PIBy2) * bnd.createFenceOffset),
                            pivotAxlePos.northing + (Math.Cos(pivotAxlePos.heading - glm.PIBy2) * bnd.createFenceOffset),
                            pivotAxlePos.heading);
                        bnd.fenceBeingMadePts.Add(point);
                    }
                }
                else
                {
                    //draw at tool
                    if (bnd.isDrawRightSide)
                    {
                        //Right side
                        vec3 point = new vec3(section[tool.numOfSections-1].rightPoint.easting, section[tool.numOfSections - 1].rightPoint.northing, 0);
                        bnd.fenceBeingMadePts.Add(point);
                    }

                    //draw on left side
                    else
                    {
                        //Right side
                        vec3 point = new vec3(section[0].leftPoint.easting, section[0].leftPoint.northing, 0);
                        bnd.fenceBeingMadePts.Add(point);
                    }
                }
            }
        }

        private void AddContourPoints()
        {
            //record contour all the time
            //Contour Base Track.... At least One section on, turn on if not
            if (patchCounter != 0)
            {
                //keep the line going, everything is on for recording path
                if (ct.isContourOn) ct.AddPoint(pivotAxlePos);
                else
                {
                    ct.StartContourLine();
                    ct.AddPoint(pivotAxlePos);
                }
            }

            //All sections OFF so if on, turn off
            else
            {
                if (ct.isContourOn)
                { ct.StopContourLine(); }
            }

            //Build contour line if close enough to a patch
            if (ct.isContourBtnOn) ct.BuildContourGuidanceLine(pivotAxlePos);

            //save the north & east as previous
            prevContourPos.northing = pivotAxlePos.northing;
            prevContourPos.easting = pivotAxlePos.easting;
        }

        //add the points for section, contour line points, Area Calc feature
        private void AddSectionOrPathPoints()
        {
            if (trk.isRecordingCurveTrack)
            {
                trk.designPtsList.Add(new vec3(pivotAxlePos.easting, pivotAxlePos.northing, pivotAxlePos.heading));
            }

            //save the north & east as previous
            prevSectionPos.northing = pivotAxlePos.northing;
            prevSectionPos.easting = pivotAxlePos.easting;

            // if non zero, at least one section is on.
            patchCounter = 0;

            //send the current and previous GPS fore/aft corrected fix to each section
            foreach (var patch in triStrip)
            {
                if (patch.isDrawing)
                {
                    if (isPatchesChangingColor)
                    {
                        patch.numTriangles = 64;
                        isPatchesChangingColor = false;
                    }

                    patch.AddMappingPoint();
                    patchCounter++;
                }
            }
        }
    }//end class
}//end namespace