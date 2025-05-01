using System;
using System.Linq;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormConfig
    {
        #region Heading
        private void tabDHeading_Enter(object sender, EventArgs e)
        {
            //heading
            if (Settings.Vehicle.setGPS_headingFromWhichSource == "Fix") rbtnHeadingFix.Checked = true;
            else if (Settings.Vehicle.setGPS_headingFromWhichSource == "VTG") rbtnHeadingVTG.Checked = true;
            else if (Settings.Vehicle.setGPS_headingFromWhichSource == "Dual") rbtnHeadingHDT.Checked = true;

            if (rbtnHeadingHDT.Checked)
            {
                if (Settings.Vehicle.setAutoSwitchDualFixOn)
                {
                    rbtnHeadingFix.Enabled = false;
                    rbtnHeadingVTG.Enabled = false;

                    gboxSingle.Enabled = true;
                    gboxDual.Enabled = true;
                }
                else
                {
                    gboxSingle.Enabled = false;
                    gboxDual.Enabled = true;
                }
            }
            else
            {
                gboxSingle.Enabled = true;
                gboxDual.Enabled = false;
            }

            nudDualHeadingOffset.Value = Settings.Vehicle.setGPS_dualHeadingOffset;
            nudDualReverseDistance.Value = Settings.Vehicle.setGPS_dualReverseDetectionDistance;

            nudAutoSwitchDualFixSpeed.Value = Settings.Vehicle.setAutoSwitchDualFixSpeed;
            cboxIsAutoSwitchDualFixOn.Checked = Settings.Vehicle.setAutoSwitchDualFixOn;

            hsbarFusion.Value = (int)(Settings.Vehicle.setIMU_fusionWeight2 * 500);
            lblFusion.Text = (hsbarFusion.Value).ToString();
            lblFusionIMU.Text = (100 - hsbarFusion.Value).ToString();

            cboxIsRTK.Checked = Settings.Vehicle.setGPS_isRTK;
            cboxIsRTK_KillAutoSteer.Checked = Settings.Vehicle.setGPS_isRTK_KillAutoSteer;

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
            Settings.Vehicle.setGPS_isRTK = cboxIsRTK.Checked;

            Settings.Vehicle.setIMU_isReverseOn = cboxIsReverseOn.Checked;
            Settings.Vehicle.setGPS_isRTK_KillAutoSteer = cboxIsRTK_KillAutoSteer.Checked;

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

        private void cboxIsAutoSwitchDualFixOn_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxIsAutoSwitchDualFixOn.Checked)
            {
                rbtnHeadingFix.Enabled = false;
                rbtnHeadingVTG.Enabled = false;
                gboxSingle.Enabled = true;
                gboxDual.Enabled = true;
            }
            else
            {
                rbtnHeadingFix.Enabled = true;
                rbtnHeadingVTG.Enabled = true;
                gboxSingle.Enabled = false;
                gboxDual.Enabled = true;
            }

            Settings.Vehicle.setAutoSwitchDualFixOn = cboxIsAutoSwitchDualFixOn.Checked;
        }

        private void nudAutoSwitchDualFixSpeed_Click(object sender, EventArgs e)
        {
                Settings.Vehicle.setAutoSwitchDualFixSpeed = nudAutoSwitchDualFixSpeed.Value;
        }

        private void nudDualHeadingOffset_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setGPS_dualHeadingOffset = nudDualHeadingOffset.Value;
        }

        private void nudDualReverseDistance_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setGPS_dualReverseDetectionDistance = nudDualReverseDistance.Value;
        }

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

            Settings.Vehicle.setIMU_fusionWeight2 = (double)hsbarFusion.Value * 0.002;
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
            lblRollZeroOffset.Text = Settings.Vehicle.setIMU_rollZero.ToString("N2");
            hsbarRollFilter.Value = (int)(Settings.Vehicle.setIMU_rollFilter * 100);
            cboxDataInvertRoll.Checked = Settings.Vehicle.setIMU_invertRoll;
        }

        private void tabDRoll_Leave(object sender, EventArgs e)
        {
        }

        private void cboxDataInvertRoll_Click(object sender, EventArgs e)
        {
            Settings.Vehicle.setIMU_invertRoll = !Settings.Vehicle.setIMU_invertRoll;
        }

        private void hsbarRollFilter_ValueChanged(object sender, EventArgs e)
        {
            lblRollFilterPercent.Text = hsbarRollFilter.Value.ToString();
            Settings.Vehicle.setIMU_rollFilter = hsbarRollFilter.Value * 0.01;
        }

        private void btnRollOffsetDown_Click(object sender, EventArgs e)
        {
            Settings.Vehicle.setIMU_rollZero -= 0.1;
            lblRollZeroOffset.Text = Settings.Vehicle.setIMU_rollZero.ToString("N2");
        }

        private void btnRollOffsetUPGN_Click(object sender, EventArgs e)
        {
            Settings.Vehicle.setIMU_rollZero += 0.1;
            lblRollZeroOffset.Text = Settings.Vehicle.setIMU_rollZero.ToString("N2");
        }

        private void btnZeroRoll_Click(object sender, EventArgs e)
        {
            if (mf.ahrs.imuRoll != 88888)
            {
                mf.ahrs.imuRoll += Settings.Vehicle.setIMU_rollZero;
                Settings.Vehicle.setIMU_rollZero = mf.ahrs.imuRoll;
                lblRollZeroOffset.Text = Settings.Vehicle.setIMU_rollZero.ToString("N2");
                Log.EventWriter("Roll Zeroed with " + Settings.Vehicle.setIMU_rollZero.ToString());
            }
            else
            {
                lblRollZeroOffset.Text = "***";
            }
        }

        private void btnRemoveZeroOffset_Click(object sender, EventArgs e)
        {
            Settings.Vehicle.setIMU_rollZero = 0;
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
            cboxFeatureTram.Checked = Settings.User.setFeatures.isTramOn;
            cboxFeatureHeadland.Checked = Settings.User.setFeatures.isHeadlandOn;
            cboxFeatureBoundary.Checked = Settings.User.setFeatures.isBoundaryOn;

            //the nudge controls at bottom menu
            cboxFeatureNudge.Checked = Settings.User.setFeatures.isABLineOn;
            //cboxFeatureBoundaryContour.Checked = Properties.Settings.Default.setFeatures.isBndContourOn;
            cboxFeatureABSmooth.Checked = Settings.User.setFeatures.isABSmoothOn;
            cboxFeatureHideContour.Checked = Settings.User.setFeatures.isHideContourOn;
            cboxFeatureWebcam.Checked = Settings.User.setFeatures.isWebCamOn;
            cboxFeatureOffsetFix.Checked = Settings.User.setFeatures.isOffsetFixOn;

            cboxFeatureUTurn.Checked = Settings.User.setFeatures.isUTurnOn;
            cboxFeatureLateral.Checked = Settings.User.setFeatures.isLateralOn;

            cboxTurnSound.Checked = Settings.User.sound_isUturnOn;
            cboxSteerSound.Checked = Settings.User.sound_isAutoSteerOn;
            cboxHydLiftSound.Checked = Settings.User.sound_isHydLiftOn;
            cboxSectionsSound.Checked = Settings.User.sound_isSectionsOn;

            cboxAutoStartAgIO.Checked = Settings.User.isAutoStartAgIO;
            cboxAutoOffAgIO.Checked = Settings.User.isAutoOffAgIO;
            cboxShutdownWhenNoPower.Checked = Settings.User.setDisplay_isShutdownWhenNoPower;
            cboxHardwareMessages.Checked = Settings.User.setDisplay_isHardwareMessages;
        }

        private void tabBtns_Leave(object sender, EventArgs e)
        {
            Settings.User.setFeatures.isTramOn = cboxFeatureTram.Checked;
            Settings.User.setFeatures.isHeadlandOn = cboxFeatureHeadland.Checked;

            Settings.User.setFeatures.isABLineOn = cboxFeatureNudge.Checked;

            Settings.User.setFeatures.isBoundaryOn = cboxFeatureBoundary.Checked;
            Settings.User.setFeatures.isABSmoothOn = cboxFeatureABSmooth.Checked;
            Settings.User.setFeatures.isHideContourOn = cboxFeatureHideContour.Checked;
            Settings.User.setFeatures.isWebCamOn = cboxFeatureWebcam.Checked;
            Settings.User.setFeatures.isOffsetFixOn = cboxFeatureOffsetFix.Checked;

            Settings.User.setFeatures.isLateralOn = cboxFeatureLateral.Checked;
            Settings.User.setFeatures.isUTurnOn = cboxFeatureUTurn.Checked;

            mf.SetFeatureSettings();

            Settings.User.sound_isUturnOn = cboxTurnSound.Checked;
            Settings.User.sound_isAutoSteerOn = cboxSteerSound.Checked;
            Settings.User.sound_isSectionsOn = cboxSectionsSound.Checked;
            Settings.User.sound_isHydLiftOn = cboxHydLiftSound.Checked;

            Settings.User.isAutoStartAgIO = cboxAutoStartAgIO.Checked;
            Settings.User.isAutoOffAgIO = cboxAutoOffAgIO.Checked;

            Settings.User.setDisplay_isShutdownWhenNoPower = cboxShutdownWhenNoPower.Checked;
            Settings.User.setDisplay_isHardwareMessages = cboxHardwareMessages.Checked;            
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
