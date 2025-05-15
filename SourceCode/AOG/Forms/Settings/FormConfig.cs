//Please, if you use this, share the improvements

using AOG.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormConfig : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormConfig(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            tab1.Appearance = TabAppearance.FlatButtons;
            tab1.ItemSize = new Size(0, 1);
            tab1.SizeMode = TabSizeMode.Fixed;

            groupBox1.Text = gStr.Get(gs.gsVehiclegroupbox);
            label70.Text = gStr.Get(gs.gsOpacity);
            label122.Text = gStr.Get(gs.gsSendAndSave);
            label103.Text = gStr.Get(gs.gsSound);

            lblSaveAs.Text = gStr.Get(gs.gsCopyCurrentVehicleAs);
            lblNew.Text = gStr.Get(gs.gsNewDefaultVehicle);
            lblSaveAsTool.Text = gStr.Get(gs.gsCopyCurrentToolAs);
            lblNewTool.Text = gStr.Get(gs.gsNewDefaultTool);
            lblToolWidth.Text = gStr.Get(gs.gsWidth);
            //lblOpen.Text = gStr.Get(gs.gsOpen);
            //lblDelete.Text = gStr.Get(gs.gsDelete);
            lblPivotDistance.Text = gStr.Get(gs.gsPivotDistance);
            lblAntennaHeight.Text = gStr.Get(gs.gsAntennaHeight);
            groupBox5.Text = gStr.Get(gs.gsAntennaOffset);
            lblHitchLength.Text = gStr.Get(gs.gsHitchLength);
            lblWheelBase.Text = gStr.Get(gs.gsWheelbase);
            lblTrack.Text = gStr.Get(gs.gsTrack);
            gboxAttachment.Text = gStr.Get(gs.gsAttachmentStyle);

            lblUnitsHitch.Text = gStr.Get(gs.gsUnits);
            groupBox2.Text = gStr.Get(gs.gsToolOffset);
            groupBox3.Text = gStr.Get(gs.gsOverlapGap);
            lblZonesBox.Text = gStr.Get(gs.gsZones);
            lblSectionWidth.Text = gStr.Get(gs.gsWidth);
            lblCoverage.Text = gStr.Get(gs.gsCoverage);
            lblChoose.Text = gStr.Get(gs.gsChoose);
            //lblBoundary.Text = gStr.Get(gs.gsBoundary);
            lblSections.Text = gStr.Get(gs.gsSections);

            grpSwitch.Text = gStr.Get(gs.gsWorkSwitch);
            grpControls.Text = gStr.Get(gs.gsSteerSwitch);

            lblLookAheadTimeSettings.Text = gStr.Get(gs.gsLookAheadTiming);
            lblOnSecs.Text = gStr.Get(gs.gsOn);
            lblOffSecs.Text = gStr.Get(gs.gsOff);
            lblTurnOffDelay.Text = gStr.Get(gs.gsTurnOffDelay);

            gboxSingle.Text = gStr.Get(gs.gsSingleAntennaSetting);
            gboxDual.Text = gStr.Get(gs.gsDualAntennaSetting);

            lblHeadingOffset.Text = gStr.Get(gs.gsHeadingOffset);
            lblReverseDistance.Text = gStr.Get(gs.gsReverseDistance);
            lblRTKFixAlarm.Text = gStr.Get(gs.gsFixAlarm);
            lblAlarmStopsAutoSteer.Text = gStr.Get(gs.gsFixAlarmStop);
            lblMinGPSStep.Text = gStr.Get(gs.gsGpsStep);
            lblFixToFixDistance.Text = gStr.Get(gs.gsFix2Fix);
            lblIMUFusion.Text = gStr.Get(gs.gsImuFusion);
            cboxIsReverseOn.Text = gStr.Get(gs.gsSteerInReverse);

            lblRemoveOffset.Text = gStr.Get(gs.gsRemoveOffset);
            lblZeroRoll.Text = gStr.Get(gs.gsZeroRoll);
            lblRollFilter.Text = gStr.Get(gs.gsRollFilter);
            lblInvertRoll.Text = gStr.Get(gs.gsInvertRoll);

            lblUturnExtension.Text = gStr.Get(gs.gsUturnExtension);

            lblMachineModule.Text = gStr.Get(gs.gsMachineModule);
            groupBox4.Text = gStr.Get(gs.gsHydraulicLiftConfig);
            lblHydLookAhead.Text = gStr.Get(gs.gsHydraulicLiftLookAhead);
            lblHydLowerTime.Text = gStr.Get(gs.gsLowerTime);
            lblEnable.Text = gStr.Get(gs.gsEnable);
            lblHydInvertRelays.Text = gStr.Get(gs.gsInvertRelays);
            lblRaiseTime.Text = gStr.Get(gs.gsRaiseTime);

            lblUser1.Text = gStr.Get(gs.gsUser1);
            lblUser2.Text = gStr.Get(gs.gsUser2);
            lblUser3.Text = gStr.Get(gs.gsUser3);
            lblUser4.Text = gStr.Get(gs.gsUser4);
            lblSendAndSave.Text = gStr.Get(gs.gsSendAndSave);

            lblFieldMenu.Text = gStr.Get(gs.gsFieldMenu);
            lblToolsMenu.Text = gStr.Get(gs.gsToolsMenu);
            lblScreenButtons.Text = gStr.Get(gs.gsScreenButtons);

            lblBottomMenu.Text = gStr.Get(gs.gsBottomMenu);
            lblRightMenu.Text = gStr.Get(gs.gsRightMenu);
            lblPowerLoss.Text = gStr.Get(gs.gsPowerLoss);
            lblAutoStartAgio.Text = gStr.Get(gs.gsAutoStartAgIO);
            lblAutoOffAgio.Text = gStr.Get(gs.gsAutoOffAgIO);

            lblPolygons.Text = gStr.Get(gs.gsPolygons);
            lblBrightness.Text = gStr.Get(gs.gsBrightness);
            lblFieldTexture.Text = gStr.Get(gs.gsFieldTexture);
            lblLineSmooth.Text = gStr.Get(gs.gsLineSmooth);
            lblSpeedo.Text = gStr.Get(gs.gsSpeedo);
            lblSvennArrow.Text = gStr.Get(gs.gsSvennArrow);
            lblGrid.Text = gStr.Get(gs.gsGrid);
            lblDirectionMarkers.Text = gStr.Get(gs.gsDirectionMarkers);
            lblKeyboard.Text = gStr.Get(gs.gsKeyboard);
            lblStartFullScreen.Text = gStr.Get(gs.gsStartFullscreen);
            lblExtraGuides.Text = gStr.Get(gs.gsExtraGuideLines);
            lblSectionLines.Text = gStr.Get(gs.gsSectionLines);
            label79.Text = gStr.Get(gs.gsElevationlog);
            unitsGroupBox.Text = gStr.Get(gs.gsUnits);
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {
            lblVehicleToolWidth.Text = Convert.ToString((int)(Settings.Tool.toolWidth * glm.m2InchOrCm));
            SectionFeetInchesTotalWidthLabelUpdate();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
            SetTab(null, null, true);

            UpdateSummary();
        }

        private void FormConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            //since we reset, save current state
            //mf.SaveFormGPSWindowSettings();

            tab1.SelectedTab = null;// make sure tabPage_Leave is called!
            SelectedTabChanged();

            //reload all the settings from default and user.config
            //mf.LoadSettings();

            //save current vehicle
            Settings.Vehicle.Save();
            Settings.Tool.Save();
            Settings.User.Save();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void UpdateSummary()
        {
            lblSumWheelbase.Text = (Settings.User.isMetric ?
                (Settings.Vehicle.setVehicle_wheelbase * glm.m2InchOrCm).ToString("N0") :
                (Settings.Vehicle.setVehicle_wheelbase * glm.m2InchOrCm).ToString("N0"))
                + glm.unitsInCm;

            lblSumNumSections.Text = mf.section.Count.ToString();

            string snapDist = Settings.User.isMetric ?
                Settings.Vehicle.setAS_snapDistance.ToString() :
                (Settings.Vehicle.setAS_snapDistance * glm.cm2CmOrIn).ToString("N1");

            lblNudgeDistance.Text = snapDist + glm.unitsInCm.ToString();
            lblUnits.Text = Settings.User.isMetric ? "Metric" : "Imperial";

            //labelCurrentVehicle.Text = gStr.gsCurrent + ": " + RegistrySettings.vehicleFileName;
            //lblSummaryVehicleName.Text = labelCurrentVehicle.Text;

            lblSumTramWidth.Text = Settings.User.isMetric ?
                ((Settings.Tool.tram_Width).ToString() + " m") :
                ConvertMeterToFeet(Settings.Tool.tram_Width);

            lblSumToolOffset.Text = (Settings.User.isMetric ?
                (Settings.Tool.offset * glm.m2InchOrCm).ToString() :
                (Settings.Tool.offset * glm.m2InchOrCm).ToString("N1")) +
                glm.unitsInCm;

            lblSumOverlap.Text = (Settings.User.isMetric ?
                (Settings.Tool.overlap * glm.m2InchOrCm).ToString() :
                (Settings.Tool.overlap * glm.m2InchOrCm).ToString("N1")) +
                glm.unitsInCm;

            lblSumLookaheadOn.Text = $"{Settings.Tool.lookAheadOn} sec";
            lblSumLookAheadOff.Text = $"{Settings.Tool.lookAheadOff} sec";

            lblSummaryWidth.Text = $"{Settings.Tool.toolWidth} {glm.unitsFtM}";
            SectionFeetInchesTotalWidthLabelUpdate();
        }

        private void tabSummary_Enter(object sender, EventArgs e)
        {
            UpdateVehicleListView(); 
            UpdateToolListView();

            UpdateSummary();

            //lblSumWheelbase.Text = (Settings.Vehicle.setVehicle_wheelbase * glm.m2InchOrCm).ToString("N0")
            //    + glm.unitsInCm;

            //lblSumNumSections.Text = mf.tool.numOfSections.ToString();

            //string snapDist = (Settings.Vehicle.setAS_snapDistance * glm.m2InchOrCm).ToString("N1");

            //lblNudgeDistance.Text = snapDist + glm.unitsInCm.ToString();
            //lblUnits.Text = mf.isMetric ? "Metric" : "Imperial";

            lblCurrentVehicle.Text = "Vehicle" + ": " + RegistrySettings.vehicleFileName;
            lblSummaryVehicleName.Text = lblCurrentVehicle.Text;

            lblCurrentTool.Text = "Tool" + ": " + RegistrySettings.toolFileName;
            lblSummaryToolName.Text = lblCurrentTool.Text;

            //lblSumTramWidth.Text = mf.isMetric ?
            //    ((Settings.Tool.tram_Width).ToString() + " m") :
            //    ConvertMeterToFeet(Settings.Tool.tram_Width);

            //lblSumToolOffset.Text = (Settings.Tool.toolOffset * glm.m2InchOrCm).ToString("N1") + glm.unitsInCm;

            //lblSumOverlap.Text = (Settings.Tool.toolOverlap * glm.m2InchOrCm).ToString("N1") + glm.unitsInCm;

            //lblSumLookaheadOn.Text = Settings.Tool.toolLookAheadOn.ToString() + " sec";
        }

        public string ConvertMeterToFeet(double meter)
        {
            double toFeet = meter * glm.m2FtOrM;
            string feetInch = Convert.ToString((int)toFeet) + "' ";
            double temp = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0);
            feetInch += Convert.ToString(temp) + '"';
            return feetInch;
        }

        private void tabSummary_Leave(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lvVehicles.SelectedItems.Count > 0)
            {
                btnVehicleLoad.Enabled = true;
                btnVehicleDelete.Enabled = true;
            }
            else
            {
                btnVehicleLoad.Enabled = false;
                btnVehicleDelete.Enabled = false;
            }

            if (lvTools.SelectedItems.Count > 0)
            {
                btnToolLoad.Enabled = true;
                btnToolDelete.Enabled = true;
            }
            else
            {
                btnToolLoad.Enabled = false;
                btnToolDelete.Enabled = false;
            }
        }

        private void tabDisplay_Enter(object sender, EventArgs e)
        {
            chkDisplayBrightness.Checked = Settings.User.setDisplay_isBrightnessOn;
            chkDisplayFloor.Checked = Settings.User.setDisplay_isTextureOn;
            chkDisplayGrid.Checked = Settings.User.isGridOn;
            chkDisplaySpeedo.Checked = Settings.User.isSpeedoOn;
            chkGPSCorrection.Checked = Settings.User.isGPSCorrectionLineOn;

            chkSvennArrow.Checked = Settings.User.setDisplay_isSvennArrowOn;
            chkDisplayExtraGuides.Checked = Settings.User.isSideGuideLines;
            chkDisplayPolygons.Checked = mf.isDrawPolygons;
            chkDisplayKeyboard.Checked = Settings.User.setDisplay_isKeyboardOn;
            chkDisplayLogElevation.Checked = Settings.User.isLogElevation;

            chkDisplayStartFullScreen.Checked = Settings.User.setDisplay_isStartFullScreen;
            chkDirectionMarkers.Checked = Settings.User.isDirectionMarkers;
            chkSectionLines.Checked = Settings.User.setDisplay_isSectionLinesOn;
            chkLineSmooth.Checked = Settings.User.setDisplay_isLineSmooth;

            rbtnDisplayMetric.Checked = Settings.User.isMetric;
            rbtnDisplayImperial.Checked = !rbtnDisplayMetric.Checked;

            nudNumGuideLines.Value = Settings.Vehicle.setAS_numGuideLines;
        }

        private void tabDisplay_Leave(object sender, EventArgs e)
        {
            SaveDisplaySettings();
        }

        private void rbtnDisplayImperial_Click(object sender, EventArgs e)
        {
            mf.TimedMessageBox(2000, "Units Set", "Imperial");
            Log.EventWriter("Units To Imperial");

            Settings.User.isMetric = false;
            mf.ChangeMetricImperial();

            lblVehicleToolWidth.Text = Convert.ToString((int)(Settings.Tool.toolWidth * glm.m2InchOrCm));
            SectionFeetInchesTotalWidthLabelUpdate();
        }

        private void rbtnDisplayMetric_Click(object sender, EventArgs e)
        {
            mf.TimedMessageBox(2000, "Units Set", "Metric");
            Log.EventWriter("Units to Metric");

            Settings.User.isMetric = true;
            mf.ChangeMetricImperial();

            lblVehicleToolWidth.Text = Convert.ToString((int)(Settings.Tool.toolWidth * glm.m2InchOrCm));
            SectionFeetInchesTotalWidthLabelUpdate();
        }

        private void nudNumGuideLines_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setAS_numGuideLines = (int)nudNumGuideLines.Value;
        }
    }
}