using System;
using System.Linq;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormConfig
    {
        #region Heading
        private void tabDHeading_Enter(object sender, EventArgs e)
        {
            //heading
            if (Settings.Vehicle.setGPS_headingFromWhichSource == "Fix") rbtnHeadingFix.Checked = true;
            //else if (Properties.Settings.Default.setGPS_headingFromWhichSource == "VTG") rbtnHeadingGPS.Checked = true;
            else if (Settings.Vehicle.setGPS_headingFromWhichSource == "Dual") rbtnHeadingHDT.Checked = true;
            
            if (rbtnHeadingHDT.Checked)
            {
                gboxSingle.Enabled = false;
                gboxDual.Enabled = true;
            }
            else
            {
                gboxSingle.Enabled = true;
                gboxDual.Enabled = false;
            }

            nudDualHeadingOffset.Value = Settings.Vehicle.setGPS_dualHeadingOffset;
            nudDualReverseDistance.Value = Settings.Vehicle.setGPS_dualReverseDetectionDistance;

            hsbarFusion.Value = (int)(Settings.Vehicle.setIMU_fusionWeight2 * 500);
            lblFusion.Text = (hsbarFusion.Value).ToString();
            lblFusionIMU.Text = (100 - hsbarFusion.Value).ToString();

            cboxIsRTK.Checked = Settings.Vehicle.setGPS_isRTK;
            cboxIsRTK_KillAutoSteer.Checked = Settings.Vehicle.setGPS_isRTK_KillAutoSteer;

            nudFixJumpDistance.Value = Settings.Vehicle.setGPS_jumpFixAlarmDistance;

            cboxIsReverseOn.Checked = Settings.Vehicle.setIMU_isReverseOn;

            if (Settings.Vehicle.setF_minHeadingStepDistance == 1.0)
                cboxMinGPSStep.Checked = true;
            else
                cboxMinGPSStep.Checked = false;

            if (cboxMinGPSStep.Checked)
            {
                Settings.Vehicle.setF_minHeadingStepDistance = 1.0;
                Settings.Vehicle.setGPS_minimumStepLimit = 0.1;
                cboxMinGPSStep.Text = "10 cm";
                lblHeadingDistance.Text = "100 cm";
            }
            else
            {
                Settings.Vehicle.setF_minHeadingStepDistance = 0.5;
                Settings.Vehicle.setGPS_minimumStepLimit = 0.05;
                cboxMinGPSStep.Text = "5 cm";
                lblHeadingDistance.Text = "50 cm";
            }

            if (mf.ahrs.imuHeading != 99999)
            {
                hsbarFusion.Enabled = true;
            }
            else
            {
                hsbarFusion.Enabled = false;
            }

            //nudMinimumFrameTime.Value = Properties.Settings.Default.SetGPS_udpWatchMsec;

            //nudForwardComp.Value = (decimal)(Properties.Settings.Default.setGPS_forwardComp);
            //nudReverseComp.Value = (decimal)(Properties.Settings.Default.setGPS_reverseComp);
            //nudAgeAlarm.Value = Properties.Settings.Default.setGPS_ageAlarm;
        }

        private void tabDHeading_Leave(object sender, EventArgs e)
        {
            Settings.Vehicle.setIMU_fusionWeight2 = (double)hsbarFusion.Value * 0.002;
            mf.ahrs.fusionWeight = (double)hsbarFusion.Value * 0.002;

            Settings.Vehicle.setGPS_isRTK = mf.isRTK_AlarmOn = cboxIsRTK.Checked;

            Settings.Vehicle.setIMU_isReverseOn = mf.ahrs.isReverseOn = cboxIsReverseOn.Checked;
            Settings.Vehicle.setGPS_isRTK_KillAutoSteer = mf.isRTK_KillAutosteer = cboxIsRTK_KillAutoSteer.Checked;

            if (cboxMinGPSStep.Checked)
            {
                Settings.Vehicle.setF_minHeadingStepDistance = 1.0;
                Settings.Vehicle.setGPS_minimumStepLimit = 0.1;
            }
            else
            {
                Settings.Vehicle.setF_minHeadingStepDistance = 0.5;
                Settings.Vehicle.setGPS_minimumStepLimit = 0.05;
            }

            
        }
        private void rbtnHeadingFix_CheckedChanged(object sender, EventArgs e)
        {
            var checkedButton = headingGroupBox.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            Settings.Vehicle.setGPS_headingFromWhichSource = checkedButton.Text;
            mf.headingFromSource = checkedButton.Text;

            if (rbtnHeadingHDT.Checked)
            {
                gboxSingle.Enabled = false;
                gboxDual.Enabled = true;
            }
            else
            {
                gboxSingle.Enabled = true;
                gboxDual.Enabled= false;
            }
        }

        private void nudFixJumpDistance_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setGPS_jumpFixAlarmDistance = ((int)nudFixJumpDistance.Value);
            //mf.jumpDistanceAlarm = Properties.Settings.Default.setGPS_dualHeadingOffset;
        }

        private void nudDualHeadingOffset_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setGPS_dualHeadingOffset = nudDualHeadingOffset.Value;
            mf.pn.headingTrueDualOffset = Settings.Vehicle.setGPS_dualHeadingOffset;
        }

        private void nudDualReverseDistance_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setGPS_dualReverseDetectionDistance = nudDualReverseDistance.Value;
            mf.dualReverseDetectionDistance = Settings.Vehicle.setGPS_dualReverseDetectionDistance;
        }
        //private void nudMinimumFrameTime_Click(object sender, EventArgs e)
        //{
        //    if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
        //    {
        //        Properties.Settings.Default.SetGPS_udpWatchMsec = ((int)nudMinimumFrameTime.Value);
        //        mf.udpWatchLimit = Properties.Settings.Default.SetGPS_udpWatchMsec;
        //    }
        //}
        private void cboxMinGPSStePGN_Click(object sender, EventArgs e)
        {
            if (cboxMinGPSStep.Checked)
            {
                Settings.Vehicle.setF_minHeadingStepDistance = 1;
                Settings.Vehicle.setGPS_minimumStepLimit = 0.1;
                cboxMinGPSStep.Text = "10 cm";
                lblHeadingDistance.Text = "100 cm";
            }
            else
            {
                Settings.Vehicle.setF_minHeadingStepDistance = 0.5;
                Settings.Vehicle.setGPS_minimumStepLimit = 0.05;
                cboxMinGPSStep.Text = "5 cm";
                lblHeadingDistance.Text = "50 cm";
            }

        }

        private void hsbarFusion_ValueChanged(object sender, EventArgs e)
        {
            lblFusion.Text = (hsbarFusion.Value).ToString()+"%";
            lblFusionIMU.Text = (100 - hsbarFusion.Value).ToString()+"%";

            mf.ahrs.fusionWeight = (double)hsbarFusion.Value * 0.002;
        }

        //private void nudForwardComPGN_Click(object sender, EventArgs e)
        //{
        //    if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
        //    {
        //        Properties.Settings.Default.setGPS_forwardComp = (double)nudForwardComp.Value;
        //    }
        //}

        //private void nudReverseComPGN_Click(object sender, EventArgs e)
        //{
        //    if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
        //    {
        //        Properties.Settings.Default.setGPS_reverseComp = (double)nudReverseComp.Value;
        //    }
        //}

        //private void nudAgeAlarm_Click(object sender, EventArgs e)
        //{
        //    if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
        //    {
        //        Properties.Settings.Default.setGPS_ageAlarm = (int)nudAgeAlarm.Value;
        //    }
        //}

        #endregion

        #region Roll

        private void tabDRoll_Enter(object sender, EventArgs e)
        {
            //Roll
            lblRollZeroOffset.Text = ((double)Settings.Vehicle.setIMU_rollZero).ToString("N2");
            hsbarRollFilter.Value = (int)(Settings.Vehicle.setIMU_rollFilter * 100);
            cboxDataInvertRoll.Checked = Settings.Vehicle.setIMU_invertRoll;
        }

        private void tabDRoll_Leave(object sender, EventArgs e)
        {
            Settings.Vehicle.setIMU_rollFilter = (double)hsbarRollFilter.Value * 0.01;
            Settings.Vehicle.setIMU_rollZero = mf.ahrs.rollZero;
            Settings.Vehicle.setIMU_invertRoll = cboxDataInvertRoll.Checked;

            mf.ahrs.rollFilter = Settings.Vehicle.setIMU_rollFilter;
            mf.ahrs.isRollInvert = Settings.Vehicle.setIMU_invertRoll;

            
        }

        private void hsbarRollFilter_ValueChanged(object sender, EventArgs e)
        {
            lblRollFilterPercent.Text = hsbarRollFilter.Value.ToString();
        }

        private void btnRollOffsetDown_Click(object sender, EventArgs e)
        {
            if (mf.ahrs.imuRoll != 88888)
            {
                mf.ahrs.rollZero -= 0.1;
                lblRollZeroOffset.Text = (mf.ahrs.rollZero).ToString("N2");
            }
            else
            {
                lblRollZeroOffset.Text = "***";
            }
        }

        private void btnRollOffsetUPGN_Click(object sender, EventArgs e)
        {
            if (mf.ahrs.imuRoll != 88888)
            {
                mf.ahrs.rollZero += 0.1;
                lblRollZeroOffset.Text = (mf.ahrs.rollZero).ToString("N2");
            }
            else
            {
                lblRollZeroOffset.Text = "***";
            }
        }
        private void btnZeroRoll_Click(object sender, EventArgs e)
        {
            if (mf.ahrs.imuRoll != 88888)
            {
                mf.ahrs.imuRoll += mf.ahrs.rollZero;
                mf.ahrs.rollZero = mf.ahrs.imuRoll;
                lblRollZeroOffset.Text = (mf.ahrs.rollZero).ToString("N2");
                Log.EventWriter("Roll Zeroed with " + mf.ahrs.rollZero.ToString());
            }
            else
            {
                lblRollZeroOffset.Text = "***";
            }
        }

        private void btnRemoveZeroOffset_Click(object sender, EventArgs e)
        {
            mf.ahrs.rollZero = 0;
            lblRollZeroOffset.Text = "0.00";
            Log.EventWriter("Roll Zero Offset Removed");
        }

        private void btnResetIMU_Click(object sender, EventArgs e)
        {
            mf.ahrs.imuHeading = 99999;
            mf.ahrs.imuRoll = 88888;
        }

        #endregion

        #region Features On Off

        private void tabBtns_Enter(object sender, EventArgs e)
        {
            cboxFeatureTram.Checked = Settings.Interface.setFeatures.isTramOn;
            cboxFeatureHeadland.Checked = Settings.Interface.setFeatures.isHeadlandOn;
            cboxFeatureBoundary.Checked = Settings.Interface.setFeatures.isBoundaryOn;

            //the nudge controls at bottom menu
            cboxFeatureNudge.Checked = Settings.Interface.setFeatures.isABLineOn;
            //cboxFeatureBoundaryContour.Checked = Properties.Settings.Default.setFeatures.isBndContourOn;
            cboxFeatureABSmooth.Checked = Settings.Interface.setFeatures.isABSmoothOn;
            cboxFeatureHideContour.Checked = Settings.Interface.setFeatures.isHideContourOn;
            cboxFeatureWebcam.Checked = Settings.Interface.setFeatures.isWebCamOn;
            cboxFeatureOffsetFix.Checked = Settings.Interface.setFeatures.isOffsetFixOn;

            cboxFeatureUTurn.Checked = Settings.Interface.setFeatures.isUTurnOn;
            cboxFeatureLateral.Checked = Settings.Interface.setFeatures.isLateralOn;

            cboxTurnSound.Checked = Settings.Vehicle.setSound_isUturnOn;
            cboxSteerSound.Checked = Settings.Vehicle.setSound_isAutoSteerOn;
            cboxHydLiftSound.Checked = Settings.Vehicle.setSound_isHydLiftOn;
            cboxSectionsSound.Checked = Settings.Vehicle.setSound_isSectionsOn;

            cboxAutoStartAgIO.Checked = Settings.Interface.setDisplay_isAutoStartAgIO;
            cboxAutoOffAgIO.Checked = Settings.Interface.setDisplay_isAutoOffAgIO;
            cboxShutdownWhenNoPower.Checked = Settings.Vehicle.setDisplay_isShutdownWhenNoPower;
            cboxHardwareMessages.Checked = Settings.Vehicle.setDisplay_isHardwareMessages;
        }

        private void tabBtns_Leave(object sender, EventArgs e)
        {
            Settings.Interface.setFeatures.isTramOn = cboxFeatureTram.Checked;
            Settings.Interface.setFeatures.isHeadlandOn = cboxFeatureHeadland.Checked;

            Settings.Interface.setFeatures.isABLineOn = cboxFeatureNudge.Checked;

            Settings.Interface.setFeatures.isBoundaryOn = cboxFeatureBoundary.Checked;
            Settings.Interface.setFeatures.isABSmoothOn = cboxFeatureABSmooth.Checked;
            Settings.Interface.setFeatures.isHideContourOn = cboxFeatureHideContour.Checked;
            Settings.Interface.setFeatures.isWebCamOn = cboxFeatureWebcam.Checked;
            Settings.Interface.setFeatures.isOffsetFixOn = cboxFeatureOffsetFix.Checked;

            Settings.Interface.setFeatures.isLateralOn = cboxFeatureLateral.Checked;
            Settings.Interface.setFeatures.isUTurnOn = cboxFeatureUTurn.Checked;

            Settings.Vehicle.setSound_isUturnOn = cboxTurnSound.Checked;
            mf.sounds.isTurnSoundOn = cboxTurnSound.Checked;
            Settings.Vehicle.setSound_isAutoSteerOn = cboxSteerSound.Checked;
            mf.sounds.isSteerSoundOn = cboxSteerSound.Checked;
            Settings.Vehicle.setSound_isSectionsOn = cboxSectionsSound.Checked;
            mf.sounds.isSectionsSoundOn = cboxSectionsSound.Checked;
            Settings.Vehicle.setSound_isHydLiftOn = cboxHydLiftSound.Checked;
            mf.sounds.isHydLiftSoundOn = cboxHydLiftSound.Checked;

            Settings.Interface.setDisplay_isAutoStartAgIO = cboxAutoStartAgIO.Checked;
            mf.isAutoStartAgIO = cboxAutoStartAgIO.Checked;

            Settings.Interface.setDisplay_isAutoOffAgIO = cboxAutoOffAgIO.Checked;

            Settings.Vehicle.setDisplay_isShutdownWhenNoPower = cboxShutdownWhenNoPower.Checked;

            Settings.Vehicle.setDisplay_isHardwareMessages = cboxHardwareMessages.Checked;            
        }

        private void btnRightMenuOrder_Click(object sender, EventArgs e)
        {
            using (var form = new FormButtonsRightPanel(mf))
            {
                form.ShowDialog(mf);
            }
        }

        #endregion
    }
}
