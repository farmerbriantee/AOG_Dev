//Please, if you use this, share the improvements

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AOG.Properties;
using System.Globalization;
using System.IO;
using System.Media;
using System.Reflection;
using System.Collections.Generic;

using System.Text;
using AOG.Classes;

namespace AOG
{
    public enum TBrand { AOG, Case, Claas, Deutz, Fendt, JDeere, Kubota, Massey, NewHolland, Same, Steyr, Ursus, Valtra }
    public enum HBrand { Aog, Case, Claas, JDeere, NewHolland }
    public enum WDBrand { Aog, Case, Challenger, JDeere, NewHolland, Holder }

    public partial class FormGPS
    {
        //ABLines directory
        public string ablinesDirectory;
        public string fieldData, guidanceLineText;

        //colors for sections and field background
        public byte flagColor = 0;

        //polygon mode for section drawing
        public bool isDrawPolygons = false, isPauseFieldTextCounter = false;

        public Color vehicleColor;
        public double vehicleOpacity;
        public byte vehicleOpacityByte;

        public bool isFlashOnOff = false, isPanFormVisible = false;
        public bool isPanelBottomHidden = false;

        public int makeUTurnCounter = 0;

        //makes nav panel disappear after 6 seconds
        private int navPanelCounter = 0, trackMethodPanelCounter = 0;
        public uint sentenceCounter = uint.MaxValue;
        public int guideLineCounter = 0;
        public int hardwareLineCounter = 0;
        public bool isHardwareMessages = false;

        private int currentFieldTextCounter = 0;

        //For field saving in background
        private int fileSaveCounter = 1;
        private int fileSaveAlwaysCounter = 1;
        private int fourSecondCounter = 0;
        public int twoSecondCounter = 0;
        private int oneSecondCounter = 0;
        private int oneHalfSecondCounter = 0;

        //Timer triggers at 125 msec
        private void tmrWatchdog_tick(object sender, EventArgs e)
        {
            if (sentenceCounter == 19)
            {
                Log.EventWriter("No GPS Warning - Lost GPS");
            }

            //Check for a newline char, if none then just return
            if (++sentenceCounter > 20)
            {
                ShowNoGPSWarning();
                return;
            }

            ////////////////////////////////////////////// 10 second ///////////////////////////////////////////////////////
            //every 3 second update status
            if (fourSecondCounter >= 3)
            {
                if (!isPauseFieldTextCounter)
                {
                    if (++currentFieldTextCounter > 3) currentFieldTextCounter = 0;
                }

                if (isBtnAutoSteerOn || workState != btnStates.Off)
                {
                    if (this.WindowState == FormWindowState.Minimized)
                    {
                        this.WindowState = FormWindowState.Normal;
                    }
                }

                //reset the counter
                fourSecondCounter = 0;

                if (isFieldStarted)
                {
                    switch (currentFieldTextCounter)
                    {
                        case 0:
                            lblCurrentField.Text = gStr.Get(gs.gsField) + ": " + displayFieldName + " * Job: " + displayJobName;
                            break;

                        case 1:
                            lblCurrentField.Text = (bnd.bndList.Count > 0 ? fd.AreaBoundaryLessInners
                                + "  " : "") + "App: " + fd.WorkedArea
                                + "  Actual: " + fd.ActualAreaWorked
                                + "  " + fd.WorkedAreaRemainPercentage
                                + "  " + fd.WorkRateHour;
                            break;

                        case 2:
                            if (trk.currTrk != null)
                                lblCurrentField.Text = "Line: " + trk.currTrk.name;
                            else
                                lblCurrentField.Text = "Line: " + gStr.Get(gs.gsNoGuidanceLines);
                            break;

                        case 3:
                            lblCurrentField.Text = "";
                            break;


                        default:
                            break;
                    }

                    if (tram.displayMode == 0)
                        tram.isRightManualOn = tram.isLeftManualOn = false;
                }
                else
                {
                    switch (currentFieldTextCounter)
                    {
                        case 0:
                            lblCurrentField.Text = (Settings.Tool.toolWidth * glm.m2FtOrM).ToString("N2") + glm.unitsFtM + " - " + RegistrySettings.vehicleFileName;
                            break;

                        case 1:
                            lblCurrentField.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss ");
                            break;

                        case 2:
                            lblCurrentField.Text = "Lat: " + pn.latitude.ToString("N7") + "   Lon: " + pn.longitude.ToString("N7");
                            break;

                        case 3:
                            lblCurrentField.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss ");
                            break;

                        case 4:
                            lblCurrentField.Text = "";
                            break;

                        default:
                            break;
                    }
                }

                if (isPauseFieldTextCounter)
                {
                    lblCurrentField.Text = "\u23F8" + " " + lblCurrentField.Text;
                }
                else
                {
                    lblCurrentField.Text = "\u25B6" + " " + lblCurrentField.Text;
                }

                //fix
                if (timerSim.Enabled && pn.fixQuality++ > 5) pn.fixQuality = 2;

                fileSaveAlwaysCounter += 3;
            }

            /////////////////////////////////////////////////////////   2 second  ////////////////////////////////////////
            //every 2 second update status
            if (twoSecondCounter >= 2)
            {
                //reset the counter
                twoSecondCounter = 0;

                //hide the Nav panel in 6  secs
                if (panelNavigation.Visible)
                {
                    if (navPanelCounter-- <= 0)
                    {
                        panelNavigation.Visible = false;
                    }
                    lblHz.Text = gpsHz.ToString("N1") + " ~ " + (frameTime.ToString("N1")) + " " + FixQuality;
                }
            }//end every 2 seconds

            //every second update all status ///////////////////////////   1 1 1 1 1 1 ////////////////////////////
            if (oneSecondCounter >= 4)
            {
                //reset the counter
                oneSecondCounter = 0;

                //counter used for saving field in background - is actually 30 second
                fileSaveCounter++;

                //general counters
                twoSecondCounter++;
                fourSecondCounter++;

                vehicle.deadZoneDelayCounter++;

                lblFix.Text = FixQuality + "Age: " + pn.age.ToString("N1");

                switch (pn.fixQuality)
                {
                    case 4:
                        btnGPSData.BackColor = Color.PaleGreen;
                        break;
                    case 5:
                        btnGPSData.BackColor = Color.Orange;
                        break;
                    case 2:
                        btnGPSData.BackColor = Color.Yellow;
                        break;
                    default:
                        btnGPSData.BackColor = Color.Red;
                        break;
                }

                if (flp1.Visible)
                {
                    if (trackMethodPanelCounter-- < 1) flp1.Visible = false;
                }

                if (Settings.Vehicle.setApp_isNozzleApp)
                {
                    if (nozz.isFlowingFlag) lblIsFlowing.BackColor = Color.LightGreen;
                    else lblIsFlowing.BackColor = Color.Orange;

                    lblPWM_Nozz.Text = nozz.pwmDriveActual.ToString();
                    lblFlowHz_Nozz.Text = nozz.frequency.ToString() + " Hz";
                }

            }

            //every half of a second update all status  ////////////////    0.5  0.5   0.5    0.5    /////////////////
            if (oneHalfSecondCounter >= 2)
            {
                //reset the counter
                oneHalfSecondCounter = 0;

                isFlashOnOff = !isFlashOnOff;

                //the main formgps window
                //status strip values
                distanceToolBtn.Text = fd.DistanceUser + "\r\n" + fd.WorkedUserArea;

                lblSpeed.Text = Speed;

                //Nozzz
                if (Settings.Vehicle.setApp_isNozzleApp)
                {
                    //nozz.tankVolumeTotal += 1;
                    if (Settings.Tool.setNozz.isAppliedUnitsNotTankDisplayed)
                        btnSprayVolumeTotal.Text = Settings.Tool.setNozz.volumeApplied.ToString("#0.0");
                    else
                        btnSprayVolumeTotal.Text = (Settings.Tool.setNozz.volumeTankStart - Settings.Tool.setNozz.volumeApplied).ToString("#0.0");

                    //pressure reading
                    btnSprayPSI.Text = nozz.pressureActual.ToString();

                    //volume per minute displays at top of panel
                    lblGPM_Set.Text = ((double)(nozz.volumePerMinuteSet) * 0.01).ToString("N1");
                    btnSprayGalPerMinActual.Text = (((double)(nozz.volumePerMinuteActual)) * 0.01).ToString("N2");

                    //the main GPA display and button
                    if (nozz.currentSectionsWidthMeters < 0.2)
                    {
                        btnSprayGalPerAcre.Text = "Off";
                        btnSprayGalPerAcre.BackColor = Color.Transparent;
                    }
                    else
                    {
                        //volume per area calcs - GPM and L/Ha
                        if (Settings.User.isMetric)
                        {
                            //Liters * 0.00167 𝑥 𝑠𝑤𝑎𝑡ℎ 𝑤𝑖𝑑𝑡ℎ 𝑥 𝐾mh
                            nozz.volumePerAreaActualFiltered = (nozz.volumePerAreaActualFiltered * 0.6) +
                                (nozz.volumePerMinuteActual * 6) / (nozz.currentSectionsWidthMeters * avgSpeed + 0.01) * 0.6;
                        }
                        else
                        {
                            //(GPM x 5,940) / (MPH x Width in inches)
                            nozz.volumePerAreaActualFiltered = (nozz.volumePerAreaActualFiltered * 0.6)
                                + ((nozz.volumePerMinuteActual * 59.4) / (nozz.currentSectionsWidthMeters * glm.m2InchOrCm * avgSpeed * glm.kmhToMphOrKmh + 0.01) * 0.4);
                        }

                        //display actual rate
                        if (nozz.volumePerAreaActualFiltered < 100)
                            btnSprayGalPerAcre.Text = (nozz.volumePerAreaActualFiltered).ToString("N1");
                        else
                            btnSprayGalPerAcre.Text = (nozz.volumePerAreaActualFiltered).ToString("N0");

                        //flow error alarm
                        if ((Math.Abs(nozz.volumePerAreaSetSelected - nozz.volumePerAreaActualFiltered)) > (nozz.volumePerAreaSetSelected * Settings.Tool.setNozz.rateAlarmPercent))
                        {
                            if (isFlashOnOff) btnSprayGalPerAcre.BackColor = Color.DarkRed;
                            else btnSprayGalPerAcre.BackColor = Color.Transparent;
                        }
                        else
                        {
                            btnSprayGalPerAcre.BackColor = Color.DarkGreen;
                        }
                    }
                }

                if (mc.actualToolAngleDegrees != double.MaxValue)
                {
                    btnStartAgIO.Text = mc.actualToolAngleDegrees.ToString("N1") + "°";
                }

                for (int j = 0; j < controlLbls.Count; j++)
                {
                    //if section is on, green, if off, red color
                    if (section[j].isSectionOn)
                    {
                        if (section[j].sectionBtnState == btnStates.Auto)
                        {
                            //GL.Color3(0.0f, 0.9f, 0.0f);
                            if (section[j].isMappingOn) controlLbls[j].BackColor = Settings.User.setDisplay_isDayMode ? Color.Lime : Color.ForestGreen;
                            else controlLbls[j].BackColor = Color.DeepPink;
                        }
                        else controlLbls[j].BackColor = Settings.User.setDisplay_isDayMode ? Color.Yellow : Color.DarkGoldenrod;
                    }
                    else
                    {
                        if (!section[j].isMappingOn) controlLbls[j].BackColor = Settings.User.setDisplay_isDayMode ? Color.Red : Color.Crimson;
                        else controlLbls[j].BackColor = Color.RoyalBlue;
                        //GL.Color3(0.7f, 0.2f, 0.2f);
                    }
                }

            } //end every 1/2 second

            //every fourth second update  ///////////////////////////   Fourth  ////////////////////////////
            {
                //reset the counter
                oneHalfSecondCounter++;
                oneSecondCounter++;
                makeUTurnCounter++;

                btnAutoSteerConfig.Text = SetSteerAngle + "\r\n" + ActualSteerAngle;

                secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;
            }

        }//wait till timer fires again.         

        public void SetFeatureSettings()
        {
            //field menu
            boundariesToolStripMenuItem.Visible = Settings.User.setFeatures.isBoundaryOn;
            headlandToolStripMenuItem.Visible = Settings.User.setFeatures.isHeadlandOn;
            headlandBuildToolStripMenuItem.Visible = Settings.User.setFeatures.isHeadlandOn;
            tramsMultiMenuField.Visible = Settings.User.setFeatures.isTramOn;
            recordedPathStripMenu.Visible = Settings.User.setFeatures.isRecPathOn;

            //tools menu
            SmoothABtoolStripMenu.Visible = Settings.User.setFeatures.isABSmoothOn;
            deleteContourPathsToolStripMenuItem.Visible = Settings.User.setFeatures.isHideContourOn;
            webcamToolStrip.Visible = Settings.User.setFeatures.isWebCamOn;
            offsetFixToolStrip.Visible = Settings.User.setFeatures.isOffsetFixOn;

            //left side
            btnStartAgIO.Visible = Settings.User.setFeatures.isAgIOOn;
        }

        public void SetNozzleSettings()
        {
            //Nozzle Spray Controller

            nozzleAppToolStripMenuItem.Checked = Settings.Vehicle.setApp_isNozzleApp;

            //if (Settings.Vehicle.setApp_isNozzleApp)
            {
                PGN_226.pgn[PGN_226.flowCalHi] = unchecked((byte)(Settings.Tool.setNozz.flowCal >> 8)); ;
                PGN_226.pgn[PGN_226.flowCalLo] = unchecked((byte)(Settings.Tool.setNozz.flowCal));
                PGN_226.pgn[PGN_226.pressureCalHi] = unchecked((byte)(Settings.Tool.setNozz.pressureCal >> 8));
                PGN_226.pgn[PGN_226.pressureCalLo] = unchecked((byte)(Settings.Tool.setNozz.pressureCal));
                PGN_226.pgn[PGN_226.Kp] = Settings.Tool.setNozz.Kp;
                PGN_226.pgn[PGN_226.Ki] = Settings.Tool.setNozz.Ki;
                PGN_226.pgn[PGN_226.minPressure] = unchecked((byte)(Settings.Tool.setNozz.pressureMin));
                PGN_226.pgn[PGN_226.fastPWM] = Settings.Tool.setNozz.fastPWM;
                PGN_226.pgn[PGN_226.slowPWM] = Settings.Tool.setNozz.slowPWM;
                PGN_226.pgn[PGN_226.deadbandError] = Settings.Tool.setNozz.deadbandError;
                PGN_226.pgn[PGN_226.switchAtFlowError] = Settings.Tool.setNozz.switchAtFlowError;

                if (Settings.Tool.setNozz.isBypass)
                    PGN_226.pgn[PGN_226.isBypass] = 1;
                else
                    PGN_226.pgn[PGN_226.isBypass] = 0;

                if (Settings.Tool.setNozz.isMeter)
                    PGN_226.pgn[PGN_226.isBypass] += 2;
                else
                    PGN_226.pgn[PGN_226.isBypass] = 0;


                //manual rate setting
                PGN_225.pgn[PGN_225.rate] = Settings.Tool.setNozz.manualRate;

                //units
                if (cboxRate1Rate2Select.Checked)
                {
                    cboxRate1Rate2Select.Text = Settings.Tool.setNozz.volumePerAreaSet2 + Settings.Tool.setNozz.unitsPerArea;
                    nozz.volumePerAreaSetSelected = Settings.Tool.setNozz.volumePerAreaSet2;
                }
                else
                {
                    cboxRate1Rate2Select.Text = Settings.Tool.setNozz.volumePerAreaSet1 + Settings.Tool.setNozz.unitsPerArea;
                    nozz.volumePerAreaSetSelected = Settings.Tool.setNozz.volumePerAreaSet1;
                }

                btnSprayVolumeTotal.Text = Settings.Tool.setNozz.volumeApplied.ToString("#0.0");

                if (!Settings.Tool.setNozz.isAppliedUnitsNotTankDisplayed)
                    lbl_Volume.Text = "Tank " + Settings.Tool.setNozz.unitsApplied;
                else
                    lbl_Volume.Text = "App " + Settings.Tool.setNozz.unitsApplied;
            }

            //Tool GPS on
            isGPSToolActive = Settings.Tool.setToolSteer.isGPSToolActive;

            if (isGPSToolActive)
            {
                PGN_232.pgn[PGN_232.gainP] = Settings.Tool.setToolSteer.gainP;
                PGN_232.pgn[PGN_232.integral] = Settings.Tool.setToolSteer.integral;
                PGN_232.pgn[PGN_232.minPWM] = Settings.Tool.setToolSteer.minPWM;
                PGN_232.pgn[PGN_232.countsPerDegree] = Settings.Tool.setToolSteer.countsPerDegree;
                PGN_232.pgn[PGN_232.ackerman] = Settings.Tool.setToolSteer.ackermann;

                PGN_232.pgn[PGN_232.wasOffsetHi] = unchecked((byte)(Settings.Tool.setToolSteer.wasOffset >> 8));
                PGN_232.pgn[PGN_232.wasOffsetLo] = unchecked((byte)(Settings.Tool.setToolSteer.wasOffset));

                PGN_231.pgn[PGN_231.invertWAS] = Settings.Tool.setToolSteer.isInvertWAS;
                PGN_231.pgn[PGN_231.invertSteer] = Settings.Tool.setToolSteer.isInvertSteer;
                PGN_231.pgn[PGN_231.maxSteerAngle] = Settings.Tool.setToolSteer.maxSteerAngle;
            }
        }

        public void LoadSettings()
        {
            //kiosk mode
            kioskModeToolStrip.Checked = Settings.User.setWindow_isKioskMode;
            guidelinesToolStripMenuItem.Checked = Settings.User.isSideGuideLines;

            SetFeatureSettings();

            //OGL control
            cboxpRowWidth.SelectedIndex = (Settings.Vehicle.set_youSkipWidth - 1);
            btnYouSkipEnable.Image = Resources.YouSkipOff;

            ChangeMetricImperial();

            SetNozzleSettings();

            vehicleOpacity = ((double)(Settings.Vehicle.vehicleOpacity) * 0.01);
            vehicleOpacityByte = (byte)(255 * ((double)(Settings.Vehicle.vehicleOpacity) * 0.01));

            panelSim.Visible = timerSim.Enabled = simulatorOnToolStripMenuItem.Checked = Settings.User.isSimulatorOn;

            //set the flag mark button to red dot
            btnFlag.Image = Properties.Resources.FlagRed;

            vehicleColor = Settings.User.colorVehicle;

            btnHeadlandOnOff.Image = bnd.isHeadlandOn ? Properties.Resources.HeadlandOn : Properties.Resources.HeadlandOff;

            //btnChangeMappingColor.BackColor = sectionColorDay;
            btnChangeMappingColor.Text = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);

            yt.ResetCreatedYouTurn();

            //workswitch stuff

            fd.workedAreaTotalUser = Settings.Vehicle.setF_UserTotalArea;

            if (Settings.User.setDisplay_isStartFullScreen)
            {
                this.WindowState = FormWindowState.Maximized;
            }

            //main window first
            if (!Settings.User.setWindow_isKioskMode)
            {
                //main window first
                if (Settings.User.setWindow_Maximized)
                {
                    WindowState = FormWindowState.Normal;
                    Location = Settings.User.setWindow_Location;
                    Size = Settings.User.setWindow_Size;
                }
                else if (Settings.User.setWindow_Minimized)
                {
                    //WindowState = FormWindowState.Minimized;
                    Location = Settings.User.setWindow_Location;
                    Size = Settings.User.setWindow_Size;
                }
                else
                {
                    Location = Settings.User.setWindow_Location;
                    Size = Settings.User.setWindow_Size;
                }
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                Settings.User.setDisplay_isStartFullScreen = true;
                btnMaximizeMainForm.Visible = false;
                btnMinimizeMainForm.Visible = false;
            }

            if (!IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }

            //night mode
            SwapDayNightMode(false);

            //load uturn properties
            yt.LoadSettings();

            bnd.isSectionControlledByHeadland = true;
            cboxIsSectionControlled.Image = Properties.Resources.HeadlandSectionOn;

            //right side build
            PanelBuildRightMenu(Settings.User.setDisplay_buttonOrder.Split(','));

            PanelsAndOGLSize();
            PanelUpdateRightAndBottom();

            SetZoom();

            lblGuidanceLine.BringToFront();
            lblHardwareMessage.BringToFront();
            isHardwareMessages = Settings.User.setDisplay_isHardwareMessages;

            if (SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Online)
            {
                btnChargeStatus.BackColor = Color.YellowGreen;
            }
            else
            {
                btnChargeStatus.BackColor = Color.LightCoral;
            }

            SetText();
        }

        public void SetText()
        {
            enterSimCoordsToolStripMenuItem.Text = gStr.Get(gs.gsEnterSimCoords);
            menustripLanguage.Text = gStr.Get(gs.gsLanguage);

            simulatorOnToolStripMenuItem.Text = gStr.Get(gs.gsSimulatorOn);
            resetALLToolStripMenuItem.Text = gStr.Get(gs.gsResetAll);

            toolStripColors.Text = gStr.Get(gs.gsColors);
            toolStripSectionColors.Text = "Section " + gStr.Get(gs.gsColors);
            toolStripConfig.Text = gStr.Get(gs.gsConfiguration);
            toolStripSteerSettings.Text = gStr.Get(gs.gsAutoSteer);
            toolStripWorkingDirectories.Text = gStr.Get(gs.gsDirectories);

            resetEverythingToolStripMenuItem.Text = gStr.Get(gs.gsResetAllForSure);

            steerChartStripMenu.Text = gStr.Get(gs.gsCharts);

            //Tools Menu
            SmoothABtoolStripMenu.Text = gStr.Get(gs.gsSmoothABCurve);
            boundariesToolStripMenuItem.Text = gStr.Get(gs.gsBoundary);
            headlandToolStripMenuItem.Text = gStr.Get(gs.gsHeadland);
            headlandBuildToolStripMenuItem.Text = gStr.Get(gs.gsHeadland) + " Builder";
            deleteContourPathsToolStripMenuItem.Text = gStr.Get(gs.gsDeleteContourPaths);
            deleteAppliedToolStripMenuItem.Text = gStr.Get(gs.gsDeleteAppliedArea);
            tramsMultiMenuField.Text = gStr.Get(gs.gsTramLines) + " Multi";

            recordedPathStripMenu.Text = gStr.Get(gs.gsRecordedPathMenu);
            flagByLatLonToolStripMenuItem.Text = gStr.Get(gs.gsFlagByLatLon);
            boundaryToolToolStripMenu.Text = gStr.Get(gs.gsBoundary) + " Tool";

            webcamToolStrip.Text = gStr.Get(gs.gsWebCam);
            offsetFixToolStrip.Text = gStr.Get(gs.gsOffsetFix);
            wizardsMenu.Text = gStr.Get(gs.gsWizards);
            steerWizardMenuItem.Text = gStr.Get(gs.gsSteerWizard);
            steerChartToolStripMenuItem.Text = gStr.Get(gs.gsSteerChart);
            headingChartToolStripMenuItem.Text = gStr.Get(gs.gsHeadingChart);
            xTEChartToolStripMenuItem.Text = gStr.Get(gs.gsXTEChart);
        }

        public void ChangeMetricImperial()
        {
            if (Settings.User.isMetric)
            {
                glm.inchOrCm2m = 0.01;
                glm.m2InchOrCm = 100.0;

                glm.cm2CmOrIn = 1;

                glm.m2FtOrM = 1.0;
                glm.ftOrMtoM = 1.0;

                glm.kmhToMphOrKmh = 1;
                glm.mphOrKmhToKmh = 1;

                glm.unitsFtM = " m";
                glm.unitsInCm = " cm";
                glm.unitsInCmNS = "cm";
                glm.unitsKmhMph = gStr.Get(gs.gsKMH);
                glm.unitsHaOrAc = " Ha";
                glm.unitsHaOrAcHr = " ha/hr";

                //m2 to Hectare
                glm.m22HaOrAc = 0.0001;
            }
            else
            {
                //inches to meters
                glm.inchOrCm2m = 0.0254;
                //meters to inches
                glm.m2InchOrCm = 39.3701;

                //meters to feet
                glm.m2FtOrM = 3.28084;
                //feet to meters
                glm.ftOrMtoM = 0.3048;

                //cm to inch
                glm.cm2CmOrIn = 0.3937;


                glm.kmhToMphOrKmh = 0.621371;//Km/H to mph
                glm.mphOrKmhToKmh = 1.60934;//mph to Km/H

                glm.unitsInCm = " in";
                glm.unitsInCmNS = "in";
                glm.unitsFtM = " ft";
                glm.unitsKmhMph = gStr.Get(gs.gsMPH);
                glm.unitsHaOrAc = " Ac";
                glm.unitsHaOrAcHr = " ac/hr";

                //Meters to Acres
                glm.m22HaOrAc = 0.000247105;
            }
        }

        public void PanelUpdateRightAndBottom()
        {
            if (isFieldStarted)
            {
                bool isBnd = bnd.bndList.Count > 0;
                bool isHdl = isBnd && bnd.bndList[0].hdLine.Count > 0;

                bool istram = (tram.tramList.Count + tram.tramBndOuterArr.Count) > 0;

                int tracksVisible = trk.GetVisibleTracks();

                btnContourLock.Visible = ct.isContourBtnOn;

                btnAutoSteer.Enabled = trk.currTrk != null || ct.isContourBtnOn;

                bool validTrk = trk.currTrk != null && !ct.isContourBtnOn;

                btnAutoYouTurn.Visible = validTrk && isBnd;
                btnCycleLines.Visible = tracksVisible > 1 && validTrk;
                btnCycleLinesBk.Visible = tracksVisible > 1 && validTrk;

                cboxpRowWidth.Visible = validTrk;
                btnYouSkipEnable.Visible = validTrk;

                btnSnapToPivot.Visible = validTrk && Settings.User.setFeatures.isABLineOn;
                btnAdjLeft.Visible = validTrk && Settings.User.setFeatures.isABLineOn;
                btnAdjRight.Visible = validTrk && Settings.User.setFeatures.isABLineOn;

                btnTramDisplayMode.Visible = istram;
                btnHeadlandOnOff.Visible = isHdl;

                int sett = Settings.Vehicle.setArdMac_setting0;
                btnHydLift.Visible = (((sett & 2) == 2) && isHdl);

                cboxIsSectionControlled.Visible = isHdl;

                PanelSizeRightAndBottom();
            }
        }

        public void PanelBuildRightMenu(string[] buttonOrder)
        {
            panelRight.Controls.Clear();

            for (int i = 0; i < buttonOrder.Length; i++)
            {
                switch (buttonOrder[i])
                {
                    case "0":
                        panelRight.Controls.Add(btnAutoSteer);
                        break;

                    case "1":
                        panelRight.Controls.Add(btnAutoYouTurn);
                        break;

                    case "2":
                        panelRight.Controls.Add(btnSectionMasterAuto);
                        break;

                    case "3":
                        panelRight.Controls.Add(btnSectionMasterManual);
                        break;

                    case "4":
                        panelRight.Controls.Add(btnCycleLinesBk);
                        break;

                    case "5":
                        panelRight.Controls.Add(btnCycleLines);
                        break;

                    case "6":
                        panelRight.Controls.Add(btnContour);
                        panelRight.Controls.Add(btnContourLock);
                        break;

                    default:
                        break;
                }
            }

            panelRight.Controls.Add(lblNumCu);
        }

        public void PanelSizeRightAndBottom()
        {
            btnResetToolHeading.Visible = false;
            int viz = 0;
            for (int i = 0; i < panelRight.Controls.Count; i++)
            {
                if (panelRight.Controls[i].Visible && panelRight.Controls[i] is Button) viz++;
            }

            if (viz == 0) return;

            int sizer = (Height - 140) / (viz);
            if (sizer > 120) { sizer = 120; }

            for (int i = 0; i < panelRight.Controls.Count; i++)
            {
                if (panelRight.Controls[i].Visible && panelRight.Controls[i] is Button)
                {
                    panelRight.Controls[i].Height = sizer;
                }
            }

            if (panelBottom.Visible)
            {
                viz = 0;
                for (int i = 0; i < panelBottom.Controls.Count; i++)
                {
                    if (panelBottom.Controls[i].Visible && panelBottom.Controls[i] is Button)
                        viz++;
                    if (panelBottom.Controls[i].Visible && panelBottom.Controls[i] is CheckBox)
                        viz++;
                }

                if (viz == 0) return;
                if (viz > 9 && Width < 1190)
                {
                    btnResetToolHeading.Visible = false;
                }
                else
                {
                    btnResetToolHeading.Visible = true;
                    viz++;
                }

                sizer = (Width - 185) / (viz);
                if (sizer > 150) { sizer = 150; }

                for (int i = 0; i < panelBottom.Controls.Count; i++)
                {
                    if (panelBottom.Controls[i].Visible && panelBottom.Controls[i] is Button)
                        panelBottom.Controls[i].Width = sizer;
                    if (panelBottom.Controls[i].Visible && panelBottom.Controls[i] is CheckBox)
                        panelBottom.Controls[i].Width = sizer;
                }

            }

            btnFlag.Text = Settings.Vehicle.setVehicle_isStanleyUsed ? "S" : "P";
        }

        private void PanelsAndOGLSize()
        {
            bool visible = isJobStarted && Settings.Vehicle.setApp_isNozzleApp;

            tlpNozzle.Visible = visible;

            GPSDataWindowLeft = (isPanelBottomHidden ? 10 : 85) + (visible ? tlpNozzle.Width : 0);

            oglMain.Left = (isPanelBottomHidden ? 5 : 80) + (visible ? tlpNozzle.Width : 0);
            oglMain.Width = this.Width - (oglMain.Left + (isJobStarted ? 75 : 5));
            oglMain.Height = this.Height - (55 + (!isJobStarted || isPanelBottomHidden ? 0 : 70));

            tlpNozzle.Left = (isPanelBottomHidden ? 5 : 80);
            tlpNozzle.Height = oglMain.Height;


            panelSim.Top = Height - (!isJobStarted || isPanelBottomHidden ? 60 : 130);
            panelSim.Left = Width / 2 - 330;
            panelSim.Width = 700;


            panelRight.Visible = isJobStarted;
            panelBottom.Visible = isJobStarted && !isPanelBottomHidden;
            panelLeft.Visible = !isPanelBottomHidden;

            PanelSizeRightAndBottom();
        }

        private void ZoomByMouseWheel(object sender, MouseEventArgs e)
        {
            if (Settings.User.setDisplay_camZoom <= 20) Settings.User.setDisplay_camZoom -= Settings.User.setDisplay_camZoom * 0.06 * Math.Sign(e.Delta);
            else Settings.User.setDisplay_camZoom -= Settings.User.setDisplay_camZoom * 0.02 * Math.Sign(e.Delta);

            SetZoom();
        }

        public void SwapDayNightMode(bool swap = true)
        {
            if (swap)
                Settings.User.setDisplay_isDayMode = !Settings.User.setDisplay_isDayMode;

            Color foreColor = Settings.User.setDisplay_isDayMode ? Settings.User.colorTextDay : Settings.User.colorTextNight;
            btnDayNightMode.Image = Settings.User.setDisplay_isDayMode ? Properties.Resources.WindowNightMode : Properties.Resources.WindowDayMode;
            this.BackColor = Settings.User.setDisplay_isDayMode ? Settings.User.colorDayFrame : Settings.User.colorNightFrame;

            foreach (Control c in this.Controls)
            {
                //if (c is Label || c is Button)
                {
                    c.ForeColor = foreColor;
                }
            }

            foreach (Control c in panelRight.Controls)
            {
                //if (c is Label || c is Button)
                {
                    c.ForeColor = foreColor;
                }
            }

            foreach (Control c in panelNavigation.Controls)
            {
                //if (c is Label || c is Button)
                {
                    c.ForeColor = foreColor;
                }
            }
            foreach (Control c in panelControlBox.Controls)
            {
                //if (c is Label || c is Button)
                {
                    c.ForeColor = foreColor;
                }
            }

            btnChangeMappingColor.ForeColor = foreColor;
        }

        public void SaveFormGPSWindowSettings()
        {
            //save window settings
            if (WindowState == FormWindowState.Maximized)
            {
                Settings.User.setWindow_Location = RestoreBounds.Location;
                Settings.User.setWindow_Size = RestoreBounds.Size;
                Settings.User.setWindow_Maximized = false;
                Settings.User.setWindow_Minimized = false;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Settings.User.setWindow_Location = Location;
                Settings.User.setWindow_Size = Size;
                Settings.User.setWindow_Maximized = false;
                Settings.User.setWindow_Minimized = false;
            }
            else
            {
                Settings.User.setWindow_Location = RestoreBounds.Location;
                Settings.User.setWindow_Size = RestoreBounds.Size;
                Settings.User.setWindow_Maximized = false;
                Settings.User.setWindow_Minimized = true;
            }

            Settings.Vehicle.setF_UserTotalArea = fd.workedAreaTotalUser;
        }

        public string FindDirection(double heading)
        {
            if (heading < 0) heading += glm.twoPI;

            heading = glm.toDegrees(heading);

            if (heading > 337.5 || heading < 22.5)
            {
                return (" " + gStr.Get(gs.gsNorth) + " ");
            }
            if (heading > 22.5 && heading < 67.5)
            {
                return (" " + gStr.Get(gs.gsN_East) + " ");
            }
            if (heading > 67.5 && heading < 111.5)
            {
                return (" " + gStr.Get(gs.gsEast) + " ");
            }
            if (heading > 111.5 && heading < 157.5)
            {
                return (" " + gStr.Get(gs.gsS_East) + " ");
            }
            if (heading > 157.5 && heading < 202.5)
            {
                return (" " + gStr.Get(gs.gsSouth) + " ");
            }
            if (heading > 202.5 && heading < 247.5)
            {
                return (" " + gStr.Get(gs.gsS_West) + " ");
            }
            if (heading > 247.5 && heading < 292.5)
            {
                return (" " + gStr.Get(gs.gsWest) + " ");
            }
            if (heading > 292.5 && heading < 337.5)
            {
                return (" " + gStr.Get(gs.gsN_West) + " ");
            }
            return (" ?? ");
        }

        //Mouse Clicks 
        private void oglMain_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                int centerX = oglMain.Width / 2;
                int centerY = oglMain.Height / 2;

                //0 at bottom for opengl, 0 at top for windows, so invert Y value
                Point point = oglMain.PointToClient(Cursor.Position);

                if (isFieldStarted)
                {
                    if (!ct.isContourBtnOn && trk.currentGuidanceTrack.Count > 1)
                    {
                        //uturn and swap uturn direction
                        if (point.Y < 150 && point.Y > 90)
                        {
                            int middle = centerX + oglMain.Width / 5;
                            if (yt.isYouTurnBtnOn && point.X > middle - 80 && point.X < middle + 80)
                            {
                                SwapDirection();
                                return;
                            }

                            //k turn or u turn
                            middle += 140;
                            if (point.X > middle - 25 && point.X < middle + 25)
                            {
                                yt.uTurnStyle++;
                                if (yt.uTurnStyle > 1) yt.uTurnStyle = 0;
                                yt.ResetCreatedYouTurn();

                                Settings.Vehicle.set_uTurnStyle = yt.uTurnStyle;

                                return;
                            }

                            if ( Settings.User.setFeatures.isUTurnOn && isBtnAutoSteerOn)
                            {
                                //manual uturn triggering
                                middle = centerX - oglMain.Width / 4;
                                if (point.X > middle - 100 && point.X < middle + 100)
                                {
                                    if (yt.isYouTurnTriggered)
                                    {
                                        yt.ResetCreatedYouTurn();
                                    }
                                    else
                                    {
                                        if (Settings.Vehicle.setAS_functionSpeedLimit > avgSpeed)
                                        {
                                            yt.BuildManualYouTurn(point.X > middle);
                                        }
                                        else
                                        {
                                            SpeedLimitExceeded();
                                        }
                                        return;
                                    }
                                }
                            }
                        }

                        //lateral
                        else if (Settings.User.setFeatures.isLateralOn && isBtnAutoSteerOn && point.Y < 240 && point.Y > 170)
                        {
                            int middle = centerX - oglMain.Width / 4;
                            if (point.X > middle - 100 && point.X < middle + 100)
                            {
                                if (Settings.Vehicle.setAS_functionSpeedLimit > avgSpeed)
                                {
                                    yt.BuildManualYouLateral(point.X > middle);
                                }
                                else
                                {
                                    SpeedLimitExceeded();
                                }
                            }
                        }
                    }

                    //pan and hide menus
                    if (point.X > 30 && point.X < 60)
                    {
                        if (point.Y > 50 && point.Y < 80)
                        {
                            isPanFormVisible = true;
                            Form f = Application.OpenForms["FormPan"];

                            if (f != null)
                            {
                                f.Focus();
                                return;
                            }

                            Form form = new FormPan(this);
                            form.Show(this);

                            form.Top = this.Height / 3 + this.Top;
                            form.Left = this.Width - 400 + this.Left;
                        }

                        if (isFieldStarted)
                        {
                            if (point.Y > oglMain.Height - 60 && point.Y < oglMain.Height - 30)
                            {
                                isPanelBottomHidden = !isPanelBottomHidden;
                                PanelsAndOGLSize();
                                return;
                            }
                        }
                    }

                    //tram override
                    int bottomSide = oglMain.Height / 5 + 25;

                    if (Settings.Tool.isDisplayTramControl && (point.Y > (bottomSide - 50) && point.Y < bottomSide))
                    {
                        if (point.X > centerX - 100 && point.X < centerX - 20)
                        {
                            tram.isLeftManualOn = !tram.isLeftManualOn;
                        }
                        if (point.X > centerX + 20 && point.X < centerX + 100)
                        {
                            tram.isRightManualOn = !tram.isRightManualOn;
                        }
                    }
                }

                //zoom buttons
                if (point.X > oglMain.Width - 80)
                {
                    int zoom = 0;
                    //---
                    if (point.Y < 260 && point.Y > 170)
                    {
                        zoom = 1;
                    }

                    //++
                    if (point.Y < 120 && point.Y > 30)
                    {
                        zoom = -1;
                    }

                    if (zoom != 0)
                    {
                        if (Settings.User.setDisplay_camZoom <= 20) Settings.User.setDisplay_camZoom += Settings.User.setDisplay_camZoom * 0.2 * zoom;
                        else Settings.User.setDisplay_camZoom += Settings.User.setDisplay_camZoom * 0.1 * zoom;
                        SetZoom();
                        return;
                    }
                }

                //vehicle direcvtion reset
                if (point.X > centerX - 40 && point.X < centerX + 40
                    && point.Y > centerY - 60 && point.Y < centerY + 60)
                {
                    if (!Settings.Vehicle.setIMU_isReverseOn || Settings.Vehicle.setGPS_headingFromWhichSource == "Dual") return;

                    imuGPS_Offset += Math.PI;
                    TimedMessageBox(2000, "Reverse Direction", "");
                    Log.EventWriter("Direction Reset, Drive Forward");

                    return;
                }

                mouseX = point.X;
                mouseY = oglMain.Height - point.Y;

                //prevent flag selection if flag form is up
                Form fc = Application.OpenForms["Flags"];
                if (fc != null)
                {
                    fc.Focus();
                    return;
                }

                leftMouseDownOnOpenGL = true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (!isPanFormVisible)
                {
                    isPanFormVisible = true;
                    Form f = Application.OpenForms["FormPan"];

                    if (f != null)
                    {
                        f.Focus();
                        return;
                    }

                    Form form = new FormPan(this);
                    form.Show(this);

                    form.Top = this.Height / 3 + this.Top;
                    form.Left = this.Width - 400 + this.Left;
                }
                    
                rightMouseDown = true;
                _iPrevMoveX = e.X;
                _iPrevMoveY = e.Y;
            }
        }

        private int _iPrevMoveX, _iPrevMoveY;
        private void OglMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (rightMouseDown)
            {
                int distanceX = e.X - _iPrevMoveX;
                int distanceY = e.Y - _iPrevMoveY;

                _iPrevMoveX = e.X;
                _iPrevMoveY = e.Y;

                camera.panX += distanceX * (Settings.User.setDisplay_camZoom / 100);
                camera.panY -= distanceY * (Settings.User.setDisplay_camZoom / 100);
            }
        }

        private void oglMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                rightMouseDown = false;
        }

        private void SpeedLimitExceeded()
        {
            TimedMessageBox(2000, gStr.Get(gs.gsTooFast), gStr.Get(gs.gsSlowDownBelow) + " "
                + (Settings.Vehicle.setAS_functionSpeedLimit * glm.kmhToMphOrKmh).ToString("N1") + " " + glm.unitsKmhMph);

            Log.EventWriter("UTurn or Lateral Speed exceeded");

        }

        public void SwapDirection()
        {
            if (!yt.isYouTurnTriggered)
                yt.isTurnLeft = !yt.isTurnLeft;

            yt.ResetCreatedYouTurn();
        }

        //Function to delete flag
        public void DeleteSelectedFlag()
        {
            //delete selected flag and set selected to none
            flagPts.RemoveAt(flagNumberPicked - 1);
            flagNumberPicked = 0;

            // re-sort the id's based on how many flags left
            int flagCnt = flagPts.Count;
            if (flagCnt > 0)
            {
                for (int i = 0; i < flagCnt; i++) flagPts[i].ID = i + 1;
            }
        }

        private void ShowNoGPSWarning()
        {
            //update main window
            sentenceCounter = 300;
            oglMain.MakeCurrent();
            oglMain.Refresh();
        }

        #region Properties // ---------------------------------------------------------------------

        public string Latitude { get { return Convert.ToString(Math.Round(pn.latitude, 7)); } }
        public string Longitude { get { return Convert.ToString(Math.Round(pn.longitude, 7)); } }
        public string SatsTracked { get { return Convert.ToString(pn.satellitesTracked); } }
        public string HDOP { get { return Convert.ToString(pn.hdop); } }
        public string Heading { get { return Convert.ToString(Math.Round(glm.toDegrees(fixHeading), 1)) + "\u00B0"; } }
        public string GPSHeading { get { return (Math.Round(glm.toDegrees(gpsHeading), 1)) + "\u00B0"; } }
        public string FixQuality
        {
            get
            {
                if (pn.fixQuality == 0) return "Invalid: ";
                else if (pn.fixQuality == 1) return "GPS single: ";
                else if (pn.fixQuality == 2) return "DGPS: ";
                else if (pn.fixQuality == 3) return "PPS: ";
                else if (pn.fixQuality == 4) return "RTK fix: ";
                else if (pn.fixQuality == 5) return "RTK Float: ";
                else if (pn.fixQuality == 6) return "Estimate: ";
                else if (pn.fixQuality == 7) return "Man IP: ";
                else if (pn.fixQuality == 8) return "Sim: ";
                else return "Unknown: ";
            }
        }
        public string GyroInDegrees
        {
            get
            {
                if (ahrs.imuHeading != 99999)
                    return Math.Round(ahrs.imuHeading, 1) + "\u00B0";
                else return "-";
            }
        }
        public string RollInDegrees
        {
            get
            {
                if (ahrs.imuRoll != 88888)
                    return Math.Round((ahrs.imuRoll), 1) + "\u00B0";
                else return "-";
            }
        }
        public string SetSteerAngle { get { return (guidanceLineSteerAngle).ToString("N1"); } }
        public string ActualSteerAngle { get { return (mc.actualSteerAngleDegrees).ToString("N1"); } }

        //Metric and Imperial Properties
        public string Speed
        {
            get
            {
                if (avgSpeed > 2)
                    return (avgSpeed * glm.kmhToMphOrKmh).ToString("N1");
                else
                    return (avgSpeed * glm.kmhToMphOrKmh).ToString("N2");
            }
        }

        public string Altitude { get { return Convert.ToString((Math.Round((pn.altitude * glm.m2FtOrM), 2))); } }

        public string DistPivot
        {
            get
            {
                if (distancePivotToTurnLine > 0)
                    return (glm.m2FtOrM * distancePivotToTurnLine).ToString("0") + glm.unitsFtM;
                else return "--";
            }
        }

        #endregion properties 

        //Load Bitmaps brand
        public Bitmap GetTractorBrand(TBrand brand)
        {
            Bitmap bitmap;
            if (brand == TBrand.Case)
                bitmap = Resources.z_TractorCase;
            else if (brand == TBrand.Claas)
                bitmap = Resources.z_TractorClaas;
            else if (brand == TBrand.Deutz)
                bitmap = Resources.z_TractorDeutz;
            else if (brand == TBrand.Fendt)
                bitmap = Resources.z_TractorFendt;
            else if (brand == TBrand.JDeere)
                bitmap = Resources.z_TractorJDeere;
            else if (brand == TBrand.Kubota)
                bitmap = Resources.z_TractorKubota;
            else if (brand == TBrand.Massey)
                bitmap = Resources.z_TractorMassey;
            else if (brand == TBrand.NewHolland)
                bitmap = Resources.z_TractorNH;
            else if (brand == TBrand.Same)
                bitmap = Resources.z_TractorSame;
            else if (brand == TBrand.Steyr)
                bitmap = Resources.z_TractorSteyr;
            else if (brand == TBrand.Ursus)
                bitmap = Resources.z_TractorUrsus;
            else if (brand == TBrand.Valtra)
                bitmap = Resources.z_TractorValtra;
            else
                bitmap = Resources.z_TractorAoG;

            return bitmap;
        }

        public Bitmap GetHarvesterBrand(HBrand brandH)
        {
            Bitmap harvesterbitmap;
            if (brandH == HBrand.Case)
                harvesterbitmap = Resources.z_HarvesterCase;
            else if (brandH == HBrand.Claas)
                harvesterbitmap = Resources.z_HarvesterClaas;
            else if (brandH == HBrand.JDeere)
                harvesterbitmap = Resources.z_HarvesterJD;
            else if (brandH == HBrand.NewHolland)
                harvesterbitmap = Resources.z_HarvesterNH;
            else
                harvesterbitmap = Resources.z_HarvesterAoG;

            return harvesterbitmap;
        }

        public Bitmap Get4WDBrandFront(WDBrand brandWDF)
        {
            Bitmap bitmap4WDFront;
            if (brandWDF == WDBrand.Case)
                bitmap4WDFront = Resources.z_4WDFrontCase;
            else if (brandWDF == WDBrand.Challenger)
                bitmap4WDFront = Resources.z_4WDFrontChallenger;
            else if (brandWDF == WDBrand.JDeere)
                bitmap4WDFront = Resources.z_4WDFrontJDeere;
            else if (brandWDF == WDBrand.NewHolland)
                bitmap4WDFront = Resources.z_4WDFrontNH;
            else if (brandWDF == WDBrand.Holder)
                bitmap4WDFront = Resources.z_4WDFrontHolder;
            else
                bitmap4WDFront = Resources.z_4WDFrontAoG;

            return bitmap4WDFront;
        }

        public Bitmap Get4WDBrandRear(WDBrand brandWDR)
        {
            Bitmap bitmap4WDRear;
            if (brandWDR == WDBrand.Case)
                bitmap4WDRear = Resources.z_4WDRearCase;
            else if (brandWDR == WDBrand.Challenger)
                bitmap4WDRear = Resources.z_4WDRearChallenger;
            else if (brandWDR == WDBrand.JDeere)
                bitmap4WDRear = Resources.z_4WDRearJDeere;
            else if (brandWDR == WDBrand.NewHolland)
                bitmap4WDRear = Resources.z_4WDRearNH;
            else if (brandWDR == WDBrand.Holder)
                bitmap4WDRear = Resources.z_4WDRearHolder;
            else
                bitmap4WDRear = Resources.z_4WDRearAoG;

            return bitmap4WDRear;
        }

    }//end class
}//end namespace