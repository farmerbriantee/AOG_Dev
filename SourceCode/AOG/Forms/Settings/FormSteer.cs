using AOG.Classes;

using AOG.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormSteer : Form
    {
        private readonly FormGPS mf = null;

        private bool toSend = false, isSA = false;
        private int counter = 0, secondCntr = 0, cntr;
        private vec3 startFix;
        private double diameter, steerAngleRight, dist;
        private int windowSizeState = 0;

        //Form stuff
        public FormSteer(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.lblDistance.Text = gStr.Get(gs.gsAgressiveness);
            this.lblHeading.Text = gStr.Get(gs.gsOvershootReduction);
            this.Text = gStr.Get(gs.gsAutoSteerConfiguration);
            this.Width = 388;
            this.Height = 490;
            label19.Text = gStr.Get(gs.gsSpeedFactor);
            label82.Text = gStr.Get(gs.gsAquireFactor);
            label51.Text = gStr.Get(gs.gsDeadzone);
            label49.Text = gStr.Get(gs.gsHeading);
            label54.Text = gStr.Get(gs.gsOnDelay);

            lblTurnSensor.Text = gStr.Get(gs.gsTurnSensor);
            labelCurrentTurnSensor.Text = gStr.Get(gs.gsCurrentTurnSensor);
            labelPressureTurnSensor.Text = gStr.Get(gs.gsPressureTurnSensor);

            lblSteerResponse.Text = gStr.Get(gs.gsSteerResponse);
            lblIntegral.Text = gStr.Get(gs.gsIntegral);
            lblDistance.Text = gStr.Get(gs.gsDistance);
            lblHeading.Text = gStr.Get(gs.gsHeading);
            lblPropGain.Text = gStr.Get(gs.gsProportionalGain);
            lblMaxLimit.Text = gStr.Get(gs.gsMaxLimit);
            lblMin2Move.Text = gStr.Get(gs.gsMinToMove);
            lblGain.Text = gStr.Get(gs.gsProportionalGain);
            lblWasZero.Text = gStr.Get(gs.gsWasZero);
            lblMaxSteerAng.Text = gStr.Get(gs.gsMaxSteerAngle);
            lblCPD.Text = gStr.Get(gs.gsCountsPerDegree);
            lblAckermann.Text = gStr.Get(gs.gsAckermann);
            lblInvertWAS.Text = gStr.Get(gs.gsInvertWas);
            lblInvertMotor.Text = gStr.Get(gs.gsInvertMotor);
            lblInvertRelays.Text = gStr.Get(gs.gsInvertRelays);
            lblMotorDriver.Text = gStr.Get(gs.gsMotorDriver);
            lblA2D.Text = gStr.Get(gs.gsADConverter);
            lblIMUXY.Text = gStr.Get(gs.gsIMUAxis);
            lblSteerEnable.Text = gStr.Get(gs.gsSteerEnable);
            lblSendSave.Text = gStr.Get(gs.gsSendAndSave);
            lblResetToDefaults.Text = gStr.Get(gs.gsResetAll);
            lblWizard.Text = gStr.Get(gs.gsWizard);
            lblUturnComp.Text = gStr.Get(gs.gsUturnCompensation);
            lblSidehillDeg.Text = gStr.Get(gs.gsSideHillComp);
            lblSteerInReverse.Text = gStr.Get(gs.gsSteerInReverse);
            lblManualTurns.Text = gStr.Get(gs.gsManualTurns);
            lblMinSpeed.Text = gStr.Get(gs.gsMinSpeed);
            lblMaxSpeed.Text = gStr.Get(gs.gsMaxSpeed);
            lblLightbar.Text = gStr.Get(gs.gsLightbar);
            lblSteerBar.Text = gStr.Get(gs.gsSteerBar);
            lblNextGuideLine.Text = gStr.Get(gs.gsNextGuidanceLine);

        }

        private void FormSteer_Load(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadHold = Settings.Vehicle.setVehicle_goalPointLookAheadHold;
            cboxSteerInReverse.Checked = Settings.Vehicle.setAS_isSteerInReverse;

            if (Settings.Vehicle.setVehicle_isStanleyUsed)
            {
                btnStanleyPure.Image = Resources.ModeStanley;
            }
            else
            {
                btnStanleyPure.Image = Resources.ModePurePursuit;
            }

            if (Settings.Vehicle.setVehicle_isStanleyUsed)
            {
                tabControl1.TabPages.Remove(tabPP);
                tabControl1.TabPages.Remove(tabPPAdv);
                this.tabControl1.ItemSize = new System.Drawing.Size(105, 48);
            }
            else
            {
                tabControl1.TabPages.Remove(tabStan);
                this.tabControl1.ItemSize = new System.Drawing.Size(89, 48);
            }

            Location = Settings.User.setWindow_steerSettingsLocation;
            //WAS Zero, CPD
            hsbarWasOffset.ValueChanged -= hsbarSteerAngleSensorZero_ValueChanged;
            hsbarCountsPerDegree.ValueChanged -= hsbarCountsPerDegree_ValueChanged;

            hsbarWasOffset.Value = Settings.Vehicle.setAS_wasOffset;
            hsbarCountsPerDegree.Value = Settings.Vehicle.setAS_countsPerDegree;

            lblCountsPerDegree.Text = hsbarCountsPerDegree.Value.ToString();
            lblSteerAngleSensorZero.Text = (hsbarWasOffset.Value / (double)(hsbarCountsPerDegree.Value)).ToString("N2");

            hsbarWasOffset.ValueChanged += hsbarSteerAngleSensorZero_ValueChanged;
            hsbarCountsPerDegree.ValueChanged += hsbarCountsPerDegree_ValueChanged;

            hsbarAckerman.ValueChanged -= hsbarAckerman_ValueChanged;
            hsbarAckerman.Value = Settings.Vehicle.setAS_ackerman;
            lblAckerman.Text = hsbarAckerman.Value.ToString();
            hsbarAckerman.ValueChanged += hsbarAckerman_ValueChanged;

            //min pwm, kP
            hsbarMinPWM.ValueChanged -= hsbarMinPWM_ValueChanged;
            hsbarProportionalGain.ValueChanged -= hsbarProportionalGain_ValueChanged;

            hsbarMinPWM.Value = Settings.Vehicle.setAS_minSteerPWM;
            lblMinPWM.Text = hsbarMinPWM.Value.ToString();

            hsbarProportionalGain.Value = Settings.Vehicle.setAS_Kp;
            lblProportionalGain.Text = hsbarProportionalGain.Value.ToString();

            hsbarMinPWM.ValueChanged += hsbarMinPWM_ValueChanged;
            hsbarProportionalGain.ValueChanged += hsbarProportionalGain_ValueChanged;

            //low steer, high steer
            //hsbarLowSteerPWM.ValueChanged -= hsbarLowSteerPWM_ValueChanged;
            hsbarHighSteerPWM.ValueChanged -= hsbarHighSteerPWM_ValueChanged;

            //hsbarLowSteerPWM.Value = Properties.Settings.Default.setAS_lowSteerPWM;
            //lblLowSteerPWM.Text = hsbarLowSteerPWM.Value.ToString();

            hsbarHighSteerPWM.Value = Settings.Vehicle.setAS_highSteerPWM;
            lblHighSteerPWM.Text = hsbarHighSteerPWM.Value.ToString();

            //hsbarLowSteerPWM.ValueChanged += hsbarLowSteerPWM_ValueChanged;
            hsbarHighSteerPWM.ValueChanged += hsbarHighSteerPWM_ValueChanged;

            hsbarMaxSteerAngle.Value = (Int16)Settings.Vehicle.setVehicle_maxSteerAngle;
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();

            mf.vehicle.stanleyDistanceErrorGain = Settings.Vehicle.stanleyDistanceErrorGain;
            hsbarStanleyGain.Value = (Int16)(mf.vehicle.stanleyDistanceErrorGain * 10);
            lblStanleyGain.Text = mf.vehicle.stanleyDistanceErrorGain.ToString();

            mf.vehicle.stanleyHeadingErrorGain = Settings.Vehicle.stanleyHeadingErrorGain;
            hsbarHeadingErrorGain.Value = (Int16)(mf.vehicle.stanleyHeadingErrorGain * 10);
            lblHeadingErrorGain.Text = mf.vehicle.stanleyHeadingErrorGain.ToString();

            mf.vehicle.stanleyIntegralGainAB = Settings.Vehicle.stanleyIntegralGainAB;

            mf.vehicle.purePursuitIntegralGain = Settings.Vehicle.setAS_purePursuitIntegralGain;
            hsbarIntegralPurePursuit.Value = (int)(Settings.Vehicle.setAS_purePursuitIntegralGain * 100);
            lblPureIntegral.Text = ((int)(mf.vehicle.purePursuitIntegralGain * 100)).ToString();

            hsbarSideHillComp.Value = (int)(Settings.Vehicle.setAS_sideHillComp * 100);

            mf.vehicle.goalPointLookAheadHold = Settings.Vehicle.setVehicle_goalPointLookAheadHold;
            hsbarHoldLookAhead.Value = (Int16)(mf.vehicle.goalPointLookAheadHold * 10);
            lblHoldLookAhead.Text = mf.vehicle.goalPointLookAheadHold.ToString();

            hsbarLookAheadMult.Value = (Int16)(Settings.Vehicle.setVehicle_goalPointLookAheadMult * 10);
            lblLookAheadMult.Text = mf.vehicle.goalPointLookAheadMult.ToString();

            hsbarAcquireFactor.Value = (int)(Settings.Vehicle.setVehicle_goalPointAcquireFactor * 100);
            lblAcquireFactor.Text = mf.vehicle.goalPointAcquireFactor.ToString();

            lblAcquirePP.Text = (mf.vehicle.goalPointLookAheadHold * mf.vehicle.goalPointAcquireFactor).ToString("N1");

            hsbarUTurnCompensation.Value = (Int16)(Settings.Vehicle.setAS_uTurnCompensation * 10);
            lblUTurnCompensation.Text = (hsbarUTurnCompensation.Value - 10).ToString();

            //make sure free drive is off
            btnFreeDrive.Image = Properties.Resources.SteerDriveOff;
            mf.vehicle.isInFreeDriveMode = false;
            btnSteerAngleDown.Enabled = false;
            btnSteerAngleUp.Enabled = false;
            mf.vehicle.driveFreeSteerAngle = 0;

            //nudDeadZoneDistance.Value = (decimal)((double)(Properties.Settings.Default.setAS_deadZoneDistance)/10);
            nudDeadZoneHeading.Value = Settings.Vehicle.setAS_deadZoneHeading * 0.01;
            nudDeadZoneDelay.Value = mf.vehicle.deadZoneDelay;

            toSend = false;

            int sett = Settings.Vehicle.setArdSteer_setting0;

            if ((sett & 1) == 0) chkInvertWAS.Checked = false;
            else chkInvertWAS.Checked = true;

            if ((sett & 2) == 0) chkSteerInvertRelays.Checked = false;
            else chkSteerInvertRelays.Checked = true;

            if ((sett & 4) == 0) chkInvertSteer.Checked = false;
            else chkInvertSteer.Checked = true;

            if ((sett & 8) == 0) cboxConv.Text = "Differential";
            else cboxConv.Text = "Single";

            if ((sett & 16) == 0) cboxMotorDrive.Text = "IBT2";
            else cboxMotorDrive.Text = "Cytron";

            if ((sett & 32) == 32) cboxSteerEnable.Text = "Switch";
            else if ((sett & 64) == 64) cboxSteerEnable.Text = "Button";
            else cboxSteerEnable.Text = "None";

            if ((sett & 128) == 0) cboxEncoder.Checked = false;
            else cboxEncoder.Checked = true;

            nudMaxCounts.Value = Settings.Vehicle.setArdSteer_maxPulseCounts;
            hsbarSensor.Value = Settings.Vehicle.setArdSteer_maxPulseCounts;
            lblhsbarSensor.Text = ((int)(hsbarSensor.Value * 0.3921568627)).ToString() + "%";

            sett = Settings.Vehicle.setArdSteer_setting1;

            if ((sett & 1) == 0) cboxDanfoss.Checked = false;
            else cboxDanfoss.Checked = true;

            if ((sett & 8) == 0) cboxXY.Text = "X";
            else cboxXY.Text = "Y";

            if ((sett & 2) == 0) cboxPressureSensor.Checked = false;
            else cboxPressureSensor.Checked = true;

            if ((sett & 4) == 0) cboxCurrentSensor.Checked = false;
            else cboxCurrentSensor.Checked = true;

            if (cboxEncoder.Checked)
            {
                cboxPressureSensor.Checked = false;
                cboxCurrentSensor.Checked = false;
                lblTurnSensor.Visible = true;
                lblPercentFS.Visible = true;
                nudMaxCounts.Visible = true;
                pbarSensor.Visible = false;
                hsbarSensor.Visible = false;
                lblhsbarSensor.Visible = false;
                lblTurnSensor.Text = gStr.Get(gs.gsEncoderCounts);
            }
            else if (cboxPressureSensor.Checked)
            {
                cboxEncoder.Checked = false;
                cboxCurrentSensor.Checked = false;
                lblTurnSensor.Visible = true;
                lblPercentFS.Visible = true;
                nudMaxCounts.Visible = false;
                pbarSensor.Visible = true;
                hsbarSensor.Visible = true;
                lblhsbarSensor.Visible = true;

                lblTurnSensor.Text = "Off at %";
            }
            else if (cboxCurrentSensor.Checked)
            {
                cboxPressureSensor.Checked = false;
                cboxEncoder.Checked = false;
                lblTurnSensor.Visible = true;
                lblPercentFS.Visible = true;
                nudMaxCounts.Visible = false;
                pbarSensor.Visible = true;
                hsbarSensor.Visible = true;
                lblhsbarSensor.Visible = true;

                lblTurnSensor.Text = "Off at %";
            }
            else
            {
                cboxPressureSensor.Checked = false;
                cboxCurrentSensor.Checked = false;
                cboxEncoder.Checked = false;
                lblTurnSensor.Visible = false;
                lblPercentFS.Visible = false;
                nudMaxCounts.Visible = false;
                pbarSensor.Visible = false;
                hsbarSensor.Visible = false;
                lblhsbarSensor.Visible = false;
            }

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }

            if (Settings.User.isLightbarNotSteerBar) rbtnLightBar.Checked = true;
            else rbtnSteerBar.Checked = true;
        }

        private void FormSteer_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.vehicle.isInFreeDriveMode = false;

            Settings.Vehicle.setVehicle_goalPointLookAheadHold = mf.vehicle.goalPointLookAheadHold;
            Settings.Vehicle.setVehicle_goalPointLookAheadMult = mf.vehicle.goalPointLookAheadMult;
            Settings.Vehicle.setVehicle_goalPointAcquireFactor = mf.vehicle.goalPointAcquireFactor;

            Settings.Vehicle.stanleyHeadingErrorGain = mf.vehicle.stanleyHeadingErrorGain;
            Settings.Vehicle.stanleyDistanceErrorGain = mf.vehicle.stanleyDistanceErrorGain;
            Settings.Vehicle.stanleyIntegralGainAB = mf.vehicle.stanleyIntegralGainAB;
            Settings.Vehicle.setAS_purePursuitIntegralGain = mf.vehicle.purePursuitIntegralGain;
            Settings.Vehicle.setVehicle_maxSteerAngle = mf.vehicle.maxSteerAngle;

            Settings.Vehicle.setAS_countsPerDegree = PGN_252.pgn[PGN_252.countsPerDegree] = unchecked((byte)hsbarCountsPerDegree.Value);
            Settings.Vehicle.setAS_ackerman = PGN_252.pgn[PGN_252.ackerman] = unchecked((byte)hsbarAckerman.Value);

            Settings.Vehicle.setAS_wasOffset = hsbarWasOffset.Value;
            PGN_252.pgn[PGN_252.wasOffsetHi] = unchecked((byte)(hsbarWasOffset.Value >> 8));
            PGN_252.pgn[PGN_252.wasOffsetLo] = unchecked((byte)(hsbarWasOffset.Value));

            Settings.Vehicle.setAS_highSteerPWM = PGN_252.pgn[PGN_252.highPWM] = unchecked((byte)hsbarHighSteerPWM.Value);
            Settings.Vehicle.setAS_lowSteerPWM = PGN_252.pgn[PGN_252.lowPWM] = unchecked((byte)(hsbarHighSteerPWM.Value / 3));
            Settings.Vehicle.setAS_Kp = PGN_252.pgn[PGN_252.gainProportional] = unchecked((byte)hsbarProportionalGain.Value);
            Settings.Vehicle.setAS_minSteerPWM = PGN_252.pgn[PGN_252.minPWM] = unchecked((byte)hsbarMinPWM.Value);

            Settings.Vehicle.setAS_deadZoneHeading = (int)(mf.vehicle.deadZoneHeading * 100);
            Settings.Vehicle.setAS_deadZoneDelay = mf.vehicle.deadZoneDelay;

            Settings.User.setWindow_steerSettingsLocation = Location;

            Settings.Vehicle.setAS_uTurnCompensation = mf.vehicle.uturnCompensation;

            //save current vehicle
            Settings.Vehicle.Save();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (isSA)
            {
                //bool isSame = Math.PI - Math.Abs(Math.Abs(cH - mf.gpsHeading) - Math.PI) < (glm.PIBy2+0.1);
                dist = glm.Distance(startFix, mf.pivotAxlePos);
                cntr++;
                if (dist > diameter)
                {
                    diameter = dist;
                    cntr = 0;
                }
                lblDiameter.Text = diameter.ToString("N2") + " m";

                if (cntr > 9)
                {
                    steerAngleRight = Math.Atan(mf.vehicle.wheelbase / ((diameter - mf.vehicle.trackWidth * 0.5) / 2));
                    steerAngleRight = glm.toDegrees(steerAngleRight);
                    //steerAngleLeft = Math.Atan(mf.vehicle.wheelbase / (diameter / 2 ));
                    //steerAngleLeft = glm.toDegrees(steerAngleLeft);

                    lblCalcSteerAngleInner.Text = steerAngleRight.ToString("N1") + "\u00B0";
                    //lblCalcSteerAngleOuter.Text = steerAngleLeft.ToString("N1") + "\u00B0";
                    lblDiameter.Text = diameter.ToString("N2") + " m";
                    btnStartSA.Image = Properties.Resources.BoundaryRecord;
                    isSA = false;
                }
            }

            double actAng = mf.mc.actualSteerAngleDegrees * 5;
            if (actAng > 0)
            {
                if (actAng > 49) actAng = 49;
                CExtensionMethods.SetProgressNoAnimation(pbarRight, (int)actAng);
                pbarLeft.Value = 0;
            }
            else
            {
                if (actAng < -49) actAng = -49;
                pbarRight.Value = 0;
                CExtensionMethods.SetProgressNoAnimation(pbarLeft, (int)-actAng);
            }

            lblSteerAngle.Text = mf.SetSteerAngle;
            lblSteerAngleActual.Text = mf.mc.actualSteerAngleDegrees.ToString("N1") + "\u00B0";
            lblActualSteerAngleUpper.Text = lblSteerAngleActual.Text;
            double err = mf.mc.actualSteerAngleDegrees - mf.guidanceLineSteerAngle;
            lblError.Text = Math.Abs(err).ToString("N1") + "\u00B0";
            if (err > 0) lblError.ForeColor = Color.Red;
            else lblError.ForeColor = Color.DarkGreen;

            lblAV_Act.Text = mf.actAngVel.ToString("N1");
            lblAV_Set.Text = mf.setAngVel.ToString("N1");

            lblPWMDisplay.Text = mf.mc.pwmDisplay.ToString();

            //limit how many pgns are set when doing the settings
            counter++;

            if (toSend && counter > 4)
            {
                PGN_252.pgn[PGN_252.countsPerDegree] = unchecked((byte)hsbarCountsPerDegree.Value);
                PGN_252.pgn[PGN_252.ackerman] = unchecked((byte)hsbarAckerman.Value);

                PGN_252.pgn[PGN_252.wasOffsetHi] = unchecked((byte)(hsbarWasOffset.Value >> 8));
                PGN_252.pgn[PGN_252.wasOffsetLo] = unchecked((byte)(hsbarWasOffset.Value));

                PGN_252.pgn[PGN_252.highPWM] = unchecked((byte)hsbarHighSteerPWM.Value);
                PGN_252.pgn[PGN_252.lowPWM] = unchecked((byte)(hsbarHighSteerPWM.Value / 3));
                PGN_252.pgn[PGN_252.gainProportional] = unchecked((byte)hsbarProportionalGain.Value);
                PGN_252.pgn[PGN_252.minPWM] = unchecked((byte)hsbarMinPWM.Value);

                mf.SendPgnToLoop(PGN_252.pgn);
                toSend = false;
                counter = 0;
            }

            if (secondCntr++ > 2)
            {
                secondCntr = 0;

                if (tabControl1.SelectedTab == tabPPAdv)
                {
                    lblHoldAdv.Text = mf.vehicle.goalPointLookAheadHold.ToString("N1");
                    lblAcqAdv.Text = (mf.vehicle.goalPointLookAheadHold * mf.vehicle.goalPointAcquireFactor).ToString("N1");
                    lblDistanceAdv.Text = mf.vehicle.goalDistance.ToString("N1");
                    lblAcquirePP.Text = lblAcqAdv.Text;
                }
                //else if (tabControl1.SelectedTab == tabPP)
                //{
                //    lblHoldAdv.Text = mf.vehicle.goalPointLookAheadHold.ToString("N1");
                //    lblAcqAdv.Text = (mf.vehicle.goalPointLookAheadHold * mf.vehicle.goalPointAcquireFactor).ToString("N1");
                //    lblDistanceAdv.Text = mf.vehicle.goalDistance.ToString("N1");
                //}
            }

            //if (hsbarMinPWM.Value > hsbarLowSteerPWM.Value) lblMinPWM.ForeColor = Color.OrangeRed;
            //else lblMinPWM.ForeColor = SystemColors.ControlText;

            if (mf.mc.sensorData != -1)
            {
                if (mf.mc.sensorData < 0 || mf.mc.sensorData > 255) mf.mc.sensorData = 0;
                CExtensionMethods.SetProgressNoAnimation(pbarSensor, mf.mc.sensorData);
                if (nudMaxCounts.Visible == false)
                    lblPercentFS.Text = ((int)((double)mf.mc.sensorData * 0.3921568627)).ToString() + "%";
                else
                    lblPercentFS.Text = mf.mc.sensorData.ToString();
            }
        }

        //flyout form
        #region Tab Sensors

        private void EnableAlert_Click(object sender, EventArgs e)
        {
            pboxSendSteer.Visible = true;

            if (sender is CheckBox checkbox)
            {
                if (checkbox.Name == "cboxEncoder" || checkbox.Name == "cboxPressureSensor"
                    || checkbox.Name == "cboxCurrentSensor")
                {
                    if (!checkbox.Checked)
                    {
                        cboxPressureSensor.Checked = false;
                        cboxCurrentSensor.Checked = false;
                        cboxEncoder.Checked = false;
                        lblTurnSensor.Visible = false;
                        lblPercentFS.Visible = false;
                        nudMaxCounts.Visible = false;
                        pbarSensor.Visible = false;
                        hsbarSensor.Visible = false;
                        lblhsbarSensor.Visible = false;
                        return;
                    }

                    if (checkbox == cboxPressureSensor)
                    {
                        cboxEncoder.Checked = false;
                        cboxCurrentSensor.Checked = false;
                        lblTurnSensor.Visible = true;
                        lblPercentFS.Visible = true;
                        nudMaxCounts.Visible = false;
                        pbarSensor.Visible = true;
                        lblTurnSensor.Text = "Off at %";
                        hsbarSensor.Visible = true;
                        lblhsbarSensor.Visible = true;
                    }
                    else if (checkbox == cboxCurrentSensor)
                    {
                        cboxPressureSensor.Checked = false;
                        cboxEncoder.Checked = false;
                        lblTurnSensor.Visible = true;
                        lblPercentFS.Visible = true;
                        nudMaxCounts.Visible = false;
                        hsbarSensor.Visible = true;
                        pbarSensor.Visible = true;
                        lblTurnSensor.Text = "Off at %";
                        lblhsbarSensor.Visible = true;
                    }
                    else if (checkbox == cboxEncoder)
                    {
                        cboxPressureSensor.Checked = false;
                        cboxCurrentSensor.Checked = false;
                        lblTurnSensor.Visible = true;
                        lblPercentFS.Visible = false;
                        nudMaxCounts.Visible = true;
                        pbarSensor.Visible = false;
                        hsbarSensor.Visible = false;
                        lblhsbarSensor.Visible = false;
                        lblTurnSensor.Text = gStr.Get(gs.gsEncoderCounts);
                    }
                }
            }
        }

        private void nudMaxCounts_ValueChanged(object sender, EventArgs e)
        {
            pboxSendSteer.Visible = true;
        }

        private void hsbarSensor_Scroll(object sender, ScrollEventArgs e)
        {
            pboxSendSteer.Visible = true;
            lblhsbarSensor.Text = ((int)((double)hsbarSensor.Value * 0.3921568627)).ToString() + "%";
        }

        #endregion Tab Sensors

        #region Tab Settings

        private void tabSettings_Enter(object sender, EventArgs e)
        {
            cboxSteerInReverse.Checked = Settings.Vehicle.setAS_isSteerInReverse;

            if (Settings.Vehicle.setVehicle_isStanleyUsed)
            {
                btnStanleyPure.Image = Resources.ModeStanley;
            }
            else
            {
                btnStanleyPure.Image = Resources.ModePurePursuit;
            }
        }

        private void tabSettings_Leave(object sender, EventArgs e)
        {
            Settings.Vehicle.setAS_isSteerInReverse = cboxSteerInReverse.Checked;
        }

        private void hsbarUTurnCompensation_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.uturnCompensation = hsbarUTurnCompensation.Value * 0.1;
            lblUTurnCompensation.Text = (hsbarUTurnCompensation.Value - 10).ToString();
        }

        private void cboxSteerInReverse_Click(object sender, EventArgs e)
        {
            Settings.Vehicle.setAS_isSteerInReverse = cboxSteerInReverse.Checked;
        }

        private void hsbarSideHillComPGN_ValueChanged(object sender, EventArgs e)
        {
            double deg = hsbarSideHillComp.Value;
            deg *= 0.01;
            lblSideHillComp.Text = (deg.ToString("N2") + "\u00B0");
            Settings.Vehicle.setAS_sideHillComp = deg;
        }

        private void btnStanleyPure_Click(object sender, EventArgs e)
        {
            Settings.Vehicle.setVehicle_isStanleyUsed = !Settings.Vehicle.setVehicle_isStanleyUsed;

            if (Settings.Vehicle.setVehicle_isStanleyUsed)
            {
                btnStanleyPure.Image = Resources.ModeStanley;
                Log.EventWriter("Stanley Steer Mode Selectede");
            }
            else
            {
                btnStanleyPure.Image = Resources.ModePurePursuit;
                Log.EventWriter("Pure Pursuit Steer Mode Selected");
            }

            tabControl1.TabPages.Remove(tabPP);
            tabControl1.TabPages.Remove(tabPPAdv);
            tabControl1.TabPages.Remove(tabGain);
            tabControl1.TabPages.Remove(tabSteer);
            tabControl1.TabPages.Remove(tabStan);


            if (Settings.Vehicle.setVehicle_isStanleyUsed)
            {
                this.tabControl1.ItemSize = new System.Drawing.Size(105, 48);
                tabControl1.TabPages.Add(tabStan);
                tabControl1.TabPages.Add(tabGain);
                tabControl1.TabPages.Add(tabSteer);
            }
            else
            {
                tabControl1.TabPages.Add(tabPP);
                tabControl1.TabPages.Add(tabGain);
                tabControl1.TabPages.Add(tabSteer);
                tabControl1.TabPages.Add(tabPPAdv);

                this.tabControl1.ItemSize = new System.Drawing.Size(89, 48);
            }
        }

        #endregion Tab Settings

        #region Alarms Tab

        private void tabAlarm_Enter(object sender, EventArgs e)
        {
            nudMaxSteerSpeed.Value = Settings.Vehicle.setAS_maxSteerSpeed;
            nudMinSteerSpeed.Value = Settings.Vehicle.setAS_minSteerSpeed;
            nudGuidanceSpeedLimit.Value = Settings.Vehicle.setAS_functionSpeedLimit;

            label160.Text = label163.Text = label166.Text = glm.unitsKmhMph;
            label20.Text = glm.unitsInCm;
        }

        private void tabAlarm_Leave(object sender, EventArgs e)
        {
        }

        private void nudMinSteerSpeed_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setAS_minSteerSpeed = nudMinSteerSpeed.Value;
        }

        private void nudMaxSteerSpeed_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setAS_maxSteerSpeed = nudMaxSteerSpeed.Value;
        }

        private void nudGuidanceSpeedLimit_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setAS_functionSpeedLimit = nudGuidanceSpeedLimit.Value;
        }

        #endregion Alarms Tab

        #region Tab On the Line

        private void tabOnTheLine_Enter(object sender, EventArgs e)
        {
            chkDisplayLightbar.Checked = Settings.User.isLightbarOn;
            chkDisplayLightbar.Image = chkDisplayLightbar.Checked ? Resources.SwitchOn : Resources.SwitchOff;

            nudSnapDistance.DecimalPlaces = Settings.User.isMetric ? 0 : 1;
            nudSnapDistance.Value = Settings.Vehicle.setAS_snapDistance;

            nudGuidanceLookAhead.Value = Settings.Vehicle.setAS_guidanceLookAheadTime;

            nudLineWidth.Value = Settings.User.setDisplay_lineWidth;

            nudcmPerPixel.Value = Settings.User.setDisplay_lightbarCmPerPixel;

            label20.Text = glm.unitsInCm;
        }

        private void tabOnTheLine_Leave(object sender, EventArgs e)
        {
        }

        private void nudcmPerPixel_ValueChanged(object sender, EventArgs e)
        {
            Settings.User.setDisplay_lightbarCmPerPixel = ((int)nudcmPerPixel.Value);
        }

        private void nudLineWidth_ValueChanged(object sender, EventArgs e)
        {
            Settings.User.setDisplay_lineWidth = (int)nudLineWidth.Value;
        }

        private void nudSnapDistance_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setAS_snapDistance = nudSnapDistance.Value;
        }

        private void nudGuidanceLookAhead_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setAS_guidanceLookAheadTime = ((double)nudGuidanceLookAhead.Value);
        }

        private void rbtnLightBar_Click(object sender, EventArgs e)
        {
            Settings.User.isLightbarNotSteerBar = true;
        }

        private void rbtnSteerBar_Click(object sender, EventArgs e)
        {
            Settings.User.isLightbarNotSteerBar = false;
        }

        private void chkDisplayLightbar_Click(object sender, EventArgs e)
        {
            if (chkDisplayLightbar.Checked) { chkDisplayLightbar.Image = Resources.SwitchOn; }
            else { chkDisplayLightbar.Image = Resources.SwitchOff; }

            Settings.User.isLightbarOn = chkDisplayLightbar.Checked;

            Settings.User.isLightbarOn = chkDisplayLightbar.Checked;
        }

        #endregion Tab On the Line

        //main first tabform

        #region Gain

        private void hsbarMinPWM_ValueChanged(object sender, EventArgs e)
        {
            lblMinPWM.Text = unchecked((byte)hsbarMinPWM.Value).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarProportionalGain_ValueChanged(object sender, EventArgs e)
        {
            lblProportionalGain.Text = unchecked((byte)hsbarProportionalGain.Value).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarHighSteerPWM_ValueChanged(object sender, EventArgs e)
        {
            //if (hsbarLowSteerPWM.Value > hsbarHighSteerPWM.Value) hsbarLowSteerPWM.Value = hsbarHighSteerPWM.Value;
            lblHighSteerPWM.Text = unchecked((byte)hsbarHighSteerPWM.Value).ToString();
            toSend = true;
            counter = 0;
        }

        //private void hsbarLowSteerPWM_ValueChanged(object sender, EventArgs e)
        //{
        //    if (hsbarLowSteerPWM.Value > hsbarHighSteerPWM.Value) hsbarHighSteerPWM.Value = hsbarLowSteerPWM.Value;
        //    lblLowSteerPWM.Text = unchecked((byte)hsbarLowSteerPWM.Value).ToString();
        //    toSend = true;
        //    counter = 0;
        //}

        #endregion Gain

        #region Steer

        private void hsbarAckerman_ValueChanged(object sender, EventArgs e)
        {
            lblAckerman.Text = unchecked((byte)hsbarAckerman.Value).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarMaxSteerAngle_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.maxSteerAngle = hsbarMaxSteerAngle.Value;
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();
        }

        private void hsbarCountsPerDegree_ValueChanged(object sender, EventArgs e)
        {
            lblCountsPerDegree.Text = unchecked((byte)hsbarCountsPerDegree.Value).ToString();
            lblSteerAngleSensorZero.Text = (hsbarWasOffset.Value / (double)(hsbarCountsPerDegree.Value)).ToString("N2");
            toSend = true;
            counter = 0;
        }

        private void hsbarSteerAngleSensorZero_ValueChanged(object sender, EventArgs e)
        {
            lblSteerAngleSensorZero.Text = (hsbarWasOffset.Value / (double)(hsbarCountsPerDegree.Value)).ToString("N2");
            toSend = true;
            counter = 0;
        }

        private void btnZeroWAS_Click(object sender, EventArgs e)
        {
            int offset = (int)(hsbarCountsPerDegree.Value * -mf.mc.actualSteerAngleDegrees + hsbarWasOffset.Value);
            if (Math.Abs(offset) > 3900)
            {
                mf.TimedMessageBox(2000, "Exceeded Range", "Excessive Steer Angle - Cannot Zero");
                Log.EventWriter("Excessive Steer Angle, No Zero " + offset);
            }
            else
            {
                hsbarWasOffset.Value += (int)(hsbarCountsPerDegree.Value * -mf.mc.actualSteerAngleDegrees);
            }
        }

        private void btnWASZeroReset_Click(object sender, EventArgs e)
        {
            hsbarWasOffset.Value = 0;
        }

        private void btnStartSA_Click(object sender, EventArgs e)
        {
            if (!isSA)
            {
                isSA = true;
                startFix = mf.pivotAxlePos;
                dist = 0;
                diameter = 0;
                cntr = 0;
                btnStartSA.Image = Properties.Resources.boundaryStop;
                lblDiameter.Text = "0";
                lblCalcSteerAngleInner.Text = "Drive Steady";
                //lblCalcSteerAngleOuter.Text = "Consistent Steering Angle!!";
            }
            else
            {
                isSA = false;
                lblCalcSteerAngleInner.Text = "0.0" + "\u00B0";
                //lblCalcSteerAngleOuter.Text = "0.0" + "\u00B0";
                btnStartSA.Image = Properties.Resources.BoundaryRecord;
            }
        }

        #endregion Steer

        # region Stanley

        private void hsbarStanleyGain_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyDistanceErrorGain = hsbarStanleyGain.Value * 0.1;
            lblStanleyGain.Text = mf.vehicle.stanleyDistanceErrorGain.ToString();
        }

        private void hsbarHeadingErrorGain_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyHeadingErrorGain = hsbarHeadingErrorGain.Value * 0.1;
            lblHeadingErrorGain.Text = mf.vehicle.stanleyHeadingErrorGain.ToString();
        }

        #endregion

        #region Pure

        private void hsbarHoldLookAhead_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadHold = hsbarHoldLookAhead.Value * 0.1;
            lblHoldLookAhead.Text = mf.vehicle.goalPointLookAheadHold.ToString();
            lblAcquirePP.Text = (mf.vehicle.goalPointLookAheadHold * mf.vehicle.goalPointAcquireFactor).ToString("N1");
        }

        private void hsbarIntegralPurePursuit_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.purePursuitIntegralGain = hsbarIntegralPurePursuit.Value * 0.01;
            lblPureIntegral.Text = hsbarIntegralPurePursuit.Value.ToString();
        }

        private void hsbarLookAheadMult_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadMult = hsbarLookAheadMult.Value * 0.1;
            lblLookAheadMult.Text = mf.vehicle.goalPointLookAheadMult.ToString();
        }

        private void hsbarAcquireFactor_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointAcquireFactor = hsbarAcquireFactor.Value * 0.01;
            lblAcquireFactor.Text = mf.vehicle.goalPointAcquireFactor.ToString();
        }

        private void nudDeadZoneHeading_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.deadZoneHeading = nudDeadZoneHeading.Value;
        }

        private void nudDeadZoneDelay_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.deadZoneDelay = (int)nudDeadZoneDelay.Value;
        }

        private void expandWindow_Click(object sender, EventArgs e)
        {
            if (windowSizeState++ > 0) windowSizeState = 0;
            if (windowSizeState == 1)
            {
                this.Size = new System.Drawing.Size(1002, 673);
                btnExpand.Image = Properties.Resources.ArrowLeft;
            }
            else if (windowSizeState == 0)
            {
                this.Size = new System.Drawing.Size(392, 492);
                btnExpand.Image = Properties.Resources.ArrowRight;
            }
        }

        #endregion

        #region Free Drive

        private void btnFreeDrive_Click(object sender, EventArgs e)
        {
            if (mf.vehicle.isInFreeDriveMode)
            {
                //turn OFF free drive mode
                btnFreeDrive.Image = Properties.Resources.SteerDriveOff;
                btnFreeDrive.BackColor = Color.FromArgb(50, 50, 70);
                mf.vehicle.isInFreeDriveMode = false;
                btnSteerAngleDown.Enabled = false;
                btnSteerAngleUp.Enabled = false;
                //hSBarFreeDrive.Value = 0;
                mf.vehicle.driveFreeSteerAngle = 0;
            }
            else
            {
                //turn ON free drive mode
                btnFreeDrive.Image = Properties.Resources.SteerDriveOn;
                btnFreeDrive.BackColor = Color.LightGreen;
                mf.vehicle.isInFreeDriveMode = true;
                btnSteerAngleDown.Enabled = true;
                btnSteerAngleUp.Enabled = true;
                //hSBarFreeDrive.Value = 0;
                mf.vehicle.driveFreeSteerAngle = 0;
                lblSteerAngle.Text = "0";
            }
        }

        private void btnFreeDriveZero_Click(object sender, EventArgs e)
        {
            if (mf.vehicle.driveFreeSteerAngle == 0)
                mf.vehicle.driveFreeSteerAngle = 5;
            else mf.vehicle.driveFreeSteerAngle = 0;
            //hSBarFreeDrive.Value = mf.driveFreeSteerAngle;
        }

        private void btnSteerAngleUPGN_MouseDown(object sender, MouseEventArgs e)
        {
            mf.vehicle.driveFreeSteerAngle++;
            if (mf.vehicle.driveFreeSteerAngle > 40) mf.vehicle.driveFreeSteerAngle = 40;
        }

        private void btnSteerAngleDown_MouseDown(object sender, MouseEventArgs e)
        {
            mf.vehicle.driveFreeSteerAngle--;
            if (mf.vehicle.driveFreeSteerAngle < -40) mf.vehicle.driveFreeSteerAngle = -40;
        }

        #endregion

        #region bottom buttons of flyout

        private void btnSendSteerConfigPGN_Click(object sender, EventArgs e)
        {
            SaveSettings();
            mf.SendPgnToLoop(PGN_251.pgn);
            pboxSendSteer.Visible = false;
            Log.EventWriter("Steer Form, Send and Save Pressed");

            mf.TimedMessageBox(2000, gStr.Get(gs.gsAutoSteerPort), "Settings Sent To Steer Module");
        }

        private void SaveSettings()
        {
            int set = 1;
            int reset = 2046;
            int sett = 0;

            if (chkInvertWAS.Checked) sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (chkSteerInvertRelays.Checked) sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (chkInvertSteer.Checked) sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxConv.Text == "Single") sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxMotorDrive.Text == "Cytron") sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxSteerEnable.Text == "Switch") sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxSteerEnable.Text == "Button") sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxEncoder.Checked) sett |= set;
            else sett &= reset;

            //set = (set << 1);
            //reset = (reset << 1);
            //reset = (reset + 1);
            //if ( ) sett |= set;
            //else sett &= reset;

            Settings.Vehicle.setArdSteer_setting0 = (byte)sett;
            Settings.Vehicle.setArdMac_isDanfoss = cboxDanfoss.Checked;

            if (cboxCurrentSensor.Checked || cboxPressureSensor.Checked)
            {
                Settings.Vehicle.setArdSteer_maxPulseCounts = (byte)hsbarSensor.Value;
            }
            else
            {
                Settings.Vehicle.setArdSteer_maxPulseCounts = (byte)nudMaxCounts.Value;
            }

            // Settings1
            set = 1;
            reset = 2046;
            sett = 0;

            if (cboxDanfoss.Checked) sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxPressureSensor.Checked) sett |= set;
            else sett &= reset;

            //bit 2
            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxCurrentSensor.Checked) sett |= set;
            else sett &= reset;

            //bit 3
            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxXY.Text == "Y") sett |= set;
            else sett &= reset;

            Settings.Vehicle.setArdSteer_setting1 = (byte)sett;

            PGN_251.pgn[PGN_251.set0] = Settings.Vehicle.setArdSteer_setting0;
            PGN_251.pgn[PGN_251.set1] = Settings.Vehicle.setArdSteer_setting1;
            PGN_251.pgn[PGN_251.maxPulse] = Settings.Vehicle.setArdSteer_maxPulseCounts;
            PGN_251.pgn[PGN_251.minSpeed] = unchecked((byte)(Settings.Vehicle.setAS_minSteerSpeed * 10));

            if (Settings.Vehicle.setAS_isConstantContourOn)
                PGN_251.pgn[PGN_251.angVel] = 1;
            else PGN_251.pgn[PGN_251.angVel] = 0;

            pboxSendSteer.Visible = false;
        }

        private void btnSteerWizard_Click(object sender, EventArgs e)
        {
            Close();
            Form form = new FormSteerWiz(mf);
            form.Show(mf);
        }

        private void btnVehicleReset_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show("Reset This Page to Defaults",
                "Are you Sure",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result3 == DialogResult.Yes)
            {
                Log.EventWriter("Steer Form - Steer Settings Set to Default");

                mf.TimedMessageBox(2000, "Reset To Default", "Values Set to Inital Default");
                Settings.Vehicle.setVehicle_maxSteerAngle = mf.vehicle.maxSteerAngle
                    = 45;

                Settings.Vehicle.setAS_countsPerDegree = 110;

                Settings.Vehicle.setAS_ackerman = 100;
                
                Settings.Vehicle.setAS_wasOffset = 3;

                Settings.Vehicle.setAS_highSteerPWM = 180;
                Settings.Vehicle.setAS_Kp = 50;
                Settings.Vehicle.setAS_minSteerPWM = 25;

                Settings.Vehicle.setArdSteer_setting0 = 56;
                Settings.Vehicle.setArdSteer_setting1 = 0;
                Settings.Vehicle.setArdMac_isDanfoss = false;

                Settings.Vehicle.setArdSteer_maxPulseCounts = 3;

                Settings.Vehicle.setVehicle_goalPointAcquireFactor = 0.85;
                Settings.Vehicle.setVehicle_goalPointLookAheadHold = 3;
                Settings.Vehicle.setVehicle_goalPointLookAheadMult = 1.5;

                Settings.Vehicle.stanleyHeadingErrorGain = 1;
                Settings.Vehicle.stanleyDistanceErrorGain = 1;
                Settings.Vehicle.stanleyIntegralGainAB = 0;

                Settings.Vehicle.setAS_purePursuitIntegralGain = 0;

                Settings.Vehicle.setAS_sideHillComp = 0;

                Settings.Vehicle.setAS_uTurnCompensation = 1;

                Settings.Vehicle.setIMU_invertRoll = false;

                Settings.Vehicle.setIMU_rollZero = 0;

                Settings.Vehicle.setAS_minSteerSpeed = 0;
                Settings.Vehicle.setAS_maxSteerSpeed = 15;
                Settings.Vehicle.setAS_functionSpeedLimit = 12;
                Settings.Vehicle.setAS_snapDistance = 0.2;
                Settings.Vehicle.setAS_guidanceLookAheadTime = 1.5;
                Settings.Vehicle.setAS_uTurnCompensation = 1;

                Settings.Vehicle.setVehicle_isStanleyUsed = false;

                Settings.Vehicle.setAS_isSteerInReverse = false;

                mf.vehicle.LoadSettings();

                FormSteer_Load(this, e);

                toSend = true; counter = 6;

                pboxSendSteer.Visible = true;

                tabControl1.SelectTab(1);
                tabControl1.SelectTab(0);
                tabSteerSettings.SelectTab(1);
                tabSteerSettings.SelectTab(0);
            }
        }

        #endregion
    }
}