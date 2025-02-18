﻿//Please, if you use this, share the improvements

using AgOpenGPS.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormConfig : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private bool isClosing = false;

        //constructor
        public FormConfig(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            tab1.Appearance = TabAppearance.FlatButtons;
            tab1.ItemSize = new Size(0, 1);
            tab1.SizeMode = TabSizeMode.Fixed;

            lblSaveAs.Text = Lang.Get(ggStr.gsSaveAs);
            lblNew.Text = Lang.Get(ggStr.gsNew);
            lblToolWidth.Text = Lang.Get(ggStr.gsWidth);
            lblOpen.Text = Lang.Get(ggStr.gsOpen);
            lblDelete.Text = Lang.Get(ggStr.gsDelete);
            lblPivotDistance.Text = Lang.Get(ggStr.gsPivotDistance);
            lblAntennaHeight.Text = Lang.Get(ggStr.gsAntennaHeight);
            groupBox5.Text = Lang.Get(ggStr.gsAntennaOffset);
            lblHitchLength.Text = Lang.Get(ggStr.gsHitchLength);
            lblWheelBase.Text = Lang.Get(ggStr.gsWheelbase);
            lblTrack.Text = Lang.Get(ggStr.gsTrack);
            gboxAttachment.Text = Lang.Get(ggStr.gsAttachmentStyle);

            lblUnitsHitch.Text = Lang.Get(ggStr.gsUnits);
            groupBox2.Text = Lang.Get(ggStr.gsToolOffset);
            groupBox3.Text = Lang.Get(ggStr.gsOverlapGap);
            lblZonesBox.Text = Lang.Get(ggStr.gsZones);
            lblSectionWidth.Text = Lang.Get(ggStr.gsWidth);
            lblCoverage.Text = Lang.Get(ggStr.gsCoverage);
            lblChoose.Text = Lang.Get(ggStr.gsChoose);
            lblBoundary.Text = Lang.Get(ggStr.gsBoundary);
            lblSections.Text = Lang.Get(ggStr.gsSections);

            grpSwitch.Text = Lang.Get(ggStr.gsWorkSwitch);
            grpControls.Text = Lang.Get(ggStr.gsSteerSwitch);

            lblLookAheadTimeSettings.Text = Lang.Get(ggStr.gsLookAheadTiming);
            lblOnSecs.Text = Lang.Get(ggStr.gsOn);
            lblOffSecs.Text = Lang.Get(ggStr.gsOff);
            lblTurnOffDelay.Text = Lang.Get(ggStr.gsTurnOffDelay);

            gboxSingle.Text = Lang.Get(ggStr.gsSingleAntennaSetting);
            gboxDual.Text = Lang.Get(ggStr.gsDualAntennaSetting);

            lblHeadingOffset.Text = Lang.Get(ggStr.gsHeadingOffset);
            lblReverseDistance.Text = Lang.Get(ggStr.gsReverseDistance);
            lblRTKFixAlarm.Text = Lang.Get(ggStr.gsFixAlarm);
            lblAlarmStopsAutoSteer.Text = Lang.Get(ggStr.gsFixAlarmStop);
            lblMinGPSStep.Text = Lang.Get(ggStr.gsGpsStep);
            lblFixToFixDistance.Text = Lang.Get(ggStr.gsFix2Fix);
            lblIMUFusion.Text = Lang.Get(ggStr.gsImuFusion);
            cboxIsReverseOn.Text = Lang.Get(ggStr.gsSteerInReverse);

            lblRemoveOffset.Text = Lang.Get(ggStr.gsRemoveOffset);
            lblZeroRoll.Text = Lang.Get(ggStr.gsZeroRoll);
            lblRollFilter.Text = Lang.Get(ggStr.gsRollFilter);
            lblInvertRoll.Text = Lang.Get(ggStr.gsInvertRoll);

            lblUturnExtension.Text = Lang.Get(ggStr.gsUturnExtension);
            lblUturnSmoothing.Text = Lang.Get(ggStr.gsUturnSmooth);

            lblMachineModule.Text = Lang.Get(ggStr.gsMachineModule);
            groupBox4.Text = Lang.Get(ggStr.gsHydraulicLiftConfig);
            lblHydLookAhead.Text = Lang.Get(ggStr.gsHydraulicLiftLookAhead);
            lblHydLowerTime.Text = Lang.Get(ggStr.gsLowerTime);
            lblEnable.Text = Lang.Get(ggStr.gsEnable);
            lblHydInvertRelays.Text = Lang.Get(ggStr.gsInvertRelays);
            lblRaiseTime.Text = Lang.Get(ggStr.gsRaiseTime);

            lblUser1.Text = Lang.Get(ggStr.gsUser1);
            lblUser2.Text = Lang.Get(ggStr.gsUser2);
            lblUser3.Text = Lang.Get(ggStr.gsUser3);
            lblUser4.Text = Lang.Get(ggStr.gsUser4);
            lblSendAndSave.Text = Lang.Get(ggStr.gsSendAndSave);

            lblFieldMenu.Text = Lang.Get(ggStr.gsFieldMenu);
            lblToolsMenu.Text = Lang.Get(ggStr.gsToolsMenu);
            lblScreenButtons.Text = Lang.Get(ggStr.gsScreenButtons);

            lblBottomMenu.Text = Lang.Get(ggStr.gsBottomMenu);
            lblRightMenu.Text = Lang.Get(ggStr.gsRightMenu);
            lblPowerLoss.Text = Lang.Get(ggStr.gsPowerLoss);
            lblAutoStartAgio.Text = Lang.Get(ggStr.gsAutoStartAgIO);
            lblAutoOffAgio.Text = Lang.Get(ggStr.gsAutoOffAgIO);

            lblPolygons.Text = Lang.Get(ggStr.gsPolygons);
            lblBrightness.Text = Lang.Get(ggStr.gsBrightness);
            lblFieldTexture.Text = Lang.Get(ggStr.gsFieldTexture);
            lblLineSmooth.Text = Lang.Get(ggStr.gsLineSmooth);
            lblSpeedo.Text = Lang.Get(ggStr.gsSpeedo);
            lblSvennArrow.Text = Lang.Get(ggStr.gsSvennArrow);
            lblGrid.Text = Lang.Get(ggStr.gsGrid);
            lblDirectionMarkers.Text = Lang.Get(ggStr.gsDirectionMarkers);
            lblKeyboard.Text = Lang.Get(ggStr.gsKeyboard);
            lblStartFullScreen.Text = Lang.Get(ggStr.gsStartFullscreen);
            lblExtraGuides.Text = Lang.Get(ggStr.gsExtraGuideLines);
            lblSectionLines.Text = Lang.Get(ggStr.gsSectionLines);
            label79.Text = Lang.Get(ggStr.gsElevationlog);
            unitsGroupBox.Text = Lang.Get(ggStr.gsUnits);

            HideSubMenu();

            nudTrailingHitchLength.Controls[0].Enabled = false;
            nudDrawbarLength.Controls[0].Enabled = false;
            nudTankHitch.Controls[0].Enabled = false;
            nudTractorHitchLength.Controls[0].Enabled = false;

            nudLookAhead.Controls[0].Enabled = false;
            nudLookAheadOff.Controls[0].Enabled = false;
            nudTurnOffDelay.Controls[0].Enabled = false;
            nudOffset.Controls[0].Enabled = false;
            nudOverlap.Controls[0].Enabled = false;
            nudCutoffSpeed.Controls[0].Enabled = false;

            nudAntennaHeight.Controls[0].Enabled = false;
            nudAntennaOffset.Controls[0].Enabled = false;
            nudAntennaPivot.Controls[0].Enabled = false;
            nudVehicleTrack.Controls[0].Enabled = false;
            nudWheelbase.Controls[0].Enabled = false;

            nudMinCoverage.Controls[0].Enabled = false;
            nudDefaultSectionWidth.Controls[0].Enabled = false;

            nudSection01.Controls[0].Enabled = false;
            nudSection02.Controls[0].Enabled = false;
            nudSection03.Controls[0].Enabled = false;
            nudSection04.Controls[0].Enabled = false;
            nudSection05.Controls[0].Enabled = false;
            nudSection06.Controls[0].Enabled = false;
            nudSection07.Controls[0].Enabled = false;
            nudSection08.Controls[0].Enabled = false;
            nudSection09.Controls[0].Enabled = false;
            nudSection10.Controls[0].Enabled = false;
            nudSection11.Controls[0].Enabled = false;
            nudSection12.Controls[0].Enabled = false;
            nudSection13.Controls[0].Enabled = false;
            nudSection14.Controls[0].Enabled = false;
            nudSection15.Controls[0].Enabled = false;
            nudSection16.Controls[0].Enabled = false;
            nudNumberOfSections.Controls[0].Enabled = false;

            nudZone1To.Controls[0].Enabled = false;
            nudZone2To.Controls[0].Enabled = false;
            nudZone3To.Controls[0].Enabled = false;
            nudZone4To.Controls[0].Enabled = false;
            nudZone5To.Controls[0].Enabled = false;
            nudZone6To.Controls[0].Enabled = false;

            nudRaiseTime.Controls[0].Enabled = false;
            nudLowerTime.Controls[0].Enabled = false;

            nudUser1.Controls[0].Enabled = false;
            nudUser2.Controls[0].Enabled = false;
            nudUser3.Controls[0].Enabled = false;
            nudUser4.Controls[0].Enabled = false;

            nudTramWidth.Controls[0].Enabled = false;

            nudDualHeadingOffset.Controls[0].Enabled = false;
            nudDualReverseDistance.Controls[0].Enabled = false;

            nudOverlap.Controls[0].Enabled = false;
            nudOffset.Controls[0].Enabled = false;

            nudTrailingToolToPivotLength.Controls[0].Enabled = false;

            nudFixJumpDistance.Controls[0].Enabled = false;
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {
            //since we reset, save current state
            mf.SaveFormGPSWindowSettings();

            //metric or imp on spinners min/maxes
            if (!mf.isMetric) FixMinMaxSpinners();

            //the pick a saved vehicle box
            UpdateVehicleListView();

            //tabTSections_Enter(this, e);
            lblVehicleToolWidth.Text = Convert.ToString((int)(mf.tool.width * 100 * mf.cm2CmOrIn));
            SectionFeetInchesTotalWidthLabelUpdate();

            tab1.SelectedTab = tabSummary;
            tboxVehicleNameSave.Focus();

            lblSaveAs.Text = Lang.Get(ggStr.gsSaveAs);
            lblNew.Text = Lang.Get(ggStr.gsNew);
            UpdateSummary();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }

            //reload all the settings from default and user.config
            mf.LoadSettings();

            //save current vehicle
            RegistrySettings.Save();
        }

        private void FixMinMaxSpinners()
        {
            nudTankHitch.Maximum = (Math.Round(nudTankHitch.Maximum / 2.54M));
            nudTankHitch.Minimum = Math.Round(nudTankHitch.Minimum / 2.54M);

            nudDrawbarLength.Maximum = Math.Round(nudDrawbarLength.Maximum / 2.54M);
            nudDrawbarLength.Minimum = Math.Round(nudDrawbarLength.Minimum / 2.54M);

            nudTrailingHitchLength.Maximum = Math.Round(nudTrailingHitchLength.Maximum / 2.54M);
            nudTrailingHitchLength.Minimum = Math.Round(nudTrailingHitchLength.Minimum / 2.54M);

            nudTractorHitchLength.Maximum = Math.Round(nudTractorHitchLength.Maximum / 2.54M);
            nudTractorHitchLength.Minimum = Math.Round(nudTractorHitchLength.Minimum / 2.54M);

            nudVehicleTrack.Maximum = Math.Round(nudVehicleTrack.Maximum / 2.54M);
            nudVehicleTrack.Minimum = Math.Round(nudVehicleTrack.Minimum / 2.54M);

            nudWheelbase.Maximum = Math.Round(nudWheelbase.Maximum / 2.54M);
            nudWheelbase.Minimum = Math.Round(nudWheelbase.Minimum / 2.54M);

            nudOverlap.Maximum = Math.Round(nudOverlap.Maximum / 2.54M);
            nudOverlap.Minimum = Math.Round(nudOverlap.Minimum / 2.54M);

            nudOffset.Maximum = Math.Round(nudOffset.Maximum / 2.54M);
            nudOffset.Minimum = Math.Round(nudOffset.Minimum / 2.54M);

            nudDefaultSectionWidth.Maximum = Math.Round(nudDefaultSectionWidth.Maximum / 2.54M);
            nudDefaultSectionWidth.Minimum = Math.Round(nudDefaultSectionWidth.Minimum / 3.0M);

            nudSection01.Maximum = Math.Round(nudSection01.Maximum / 2.54M);
            nudSection01.Minimum = Math.Round(nudSection01.Minimum / 2.54M);
            nudSection02.Maximum = Math.Round(nudSection02.Maximum / 2.54M);
            nudSection02.Minimum = Math.Round(nudSection02.Minimum / 2.54M);
            nudSection03.Maximum = Math.Round(nudSection03.Maximum / 2.54M);
            nudSection03.Minimum = Math.Round(nudSection03.Minimum / 2.54M);
            nudSection04.Maximum = Math.Round(nudSection04.Maximum / 2.54M);
            nudSection04.Minimum = Math.Round(nudSection04.Minimum / 2.54M);
            nudSection05.Maximum = Math.Round(nudSection05.Maximum / 2.54M);
            nudSection05.Minimum = Math.Round(nudSection05.Minimum / 2.54M);
            nudSection06.Maximum = Math.Round(nudSection06.Maximum / 2.54M);
            nudSection06.Minimum = Math.Round(nudSection06.Minimum / 2.54M);
            nudSection07.Maximum = Math.Round(nudSection07.Maximum / 2.54M);
            nudSection07.Minimum = Math.Round(nudSection07.Minimum / 2.54M);
            nudSection08.Maximum = Math.Round(nudSection08.Maximum / 2.54M);
            nudSection08.Minimum = Math.Round(nudSection08.Minimum / 2.54M);
            nudSection09.Maximum = Math.Round(nudSection09.Maximum / 2.54M);
            nudSection09.Minimum = Math.Round(nudSection09.Minimum / 2.54M);
            nudSection10.Maximum = Math.Round(nudSection10.Maximum / 2.54M);
            nudSection10.Minimum = Math.Round(nudSection10.Minimum / 2.54M);
            nudSection11.Maximum = Math.Round(nudSection11.Maximum / 2.54M);
            nudSection11.Minimum = Math.Round(nudSection11.Minimum / 2.54M);
            nudSection12.Maximum = Math.Round(nudSection12.Maximum / 2.54M);
            nudSection12.Minimum = Math.Round(nudSection12.Minimum / 2.54M);
            nudSection13.Maximum = Math.Round(nudSection13.Maximum / 2.54M);
            nudSection13.Minimum = Math.Round(nudSection13.Minimum / 2.54M);
            nudSection14.Maximum = Math.Round(nudSection14.Maximum / 2.54M);
            nudSection14.Minimum = Math.Round(nudSection14.Minimum / 2.54M);
            nudSection15.Maximum = Math.Round(nudSection15.Maximum / 2.54M);
            nudSection15.Minimum = Math.Round(nudSection15.Minimum / 2.54M);
            nudSection16.Maximum = Math.Round(nudSection16.Maximum / 2.54M);
            nudSection16.Minimum = Math.Round(nudSection16.Minimum / 2.54M);

            nudTramWidth.Minimum = Math.Round(nudTramWidth.Minimum / 2.54M);
            nudTramWidth.Maximum = Math.Round(nudTramWidth.Maximum / 2.54M);

            //Meters to feet
            nudTurnDistanceFromBoundary.Minimum = Math.Round(nudTurnDistanceFromBoundary.Minimum * 3.28M);
            nudTurnDistanceFromBoundary.Maximum = Math.Round(nudTurnDistanceFromBoundary.Maximum * 3.28M);

            nudOffset.Maximum = Math.Round(nudOffset.Maximum / 2.54M);
            nudOffset.Minimum = Math.Round(nudOffset.Minimum / 2.54M);
            nudOverlap.Maximum = Math.Round(nudOverlap.Maximum / 2.54M);
            nudOverlap.Minimum = Math.Round(nudOverlap.Minimum / 2.54M);

            nudTrailingToolToPivotLength.Maximum = Math.Round(nudTrailingToolToPivotLength.Maximum / 2.54M);
            nudTrailingToolToPivotLength.Minimum = Math.Round(nudTrailingToolToPivotLength.Minimum / 2.54M);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            isClosing = true;
            Close();
        }

        private void tabSummary_Enter(object sender, EventArgs e)
        {
            SectionFeetInchesTotalWidthLabelUpdate();
            lblSummaryVehicleName.Text = RegistrySettings.workingDirectory;
            UpdateSummary();
        }

        private void tabSummary_Leave(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lvVehicles.SelectedItems.Count > 0)
            {
                //btnVehicleSaveAs.Enabled = true;
                btnVehicleLoad.Enabled = true;
                btnVehicleDelete.Enabled = true;
            }
            else
            {
                //btnVehicleSaveAs.Enabled = false;
                btnVehicleLoad.Enabled = false;
                btnVehicleDelete.Enabled = false;
            }
        }

        private void tabDisplay_Enter(object sender, EventArgs e)
        {
            chkDisplayBrightness.Checked = mf.isBrightnessOn;
            chkDisplayFloor.Checked = mf.isTextureOn;
            chkDisplayGrid.Checked = mf.isGridOn;
            chkDisplaySpeedo.Checked = mf.isSpeedoOn;
            chkDisplayStartFullScreen.Checked = Properties.Settings.Default.setDisplay_isStartFullScreen;
            chkSvennArrow.Checked = mf.isSvennArrowOn;
            chkDisplayExtraGuides.Checked = mf.isSideGuideLines;
            chkDisplayPolygons.Checked = mf.isDrawPolygons;
            chkDisplayKeyboard.Checked = mf.isKeyboardOn;
            chkDisplayLogElevation.Checked = mf.isLogElevation;
            chkDirectionMarkers.Checked = Properties.Settings.Default.setTool_isDirectionMarkers;
            chkSectionLines.Checked = Properties.Settings.Default.setDisplay_isSectionLinesOn;
            chkLineSmooth.Checked = Properties.Settings.Default.setDisplay_isLineSmooth;

            if (mf.isMetric) rbtnDisplayMetric.Checked = true;
            else rbtnDisplayImperial.Checked = true;

            nudNumGuideLines.Value = mf.trk.numGuideLines;
        }

        private void tabDisplay_Leave(object sender, EventArgs e)
        {
            SaveDisplaySettings();
        }

        private void rbtnDisplayImperial_Click(object sender, EventArgs e)
        {
            mf.TimedMessageBox(2000, "Units Set", "Imperial");
            Log.EventWriter("Units To Imperial");

            mf.isMetric = false;
            Properties.Settings.Default.setMenu_isMetric = mf.isMetric;

            isClosing = true;
            Close();
        }

        private void rbtnDisplayMetric_Click(object sender, EventArgs e)
        {
            mf.TimedMessageBox(2000, "Units Set", "Metric");
            Log.EventWriter("Units to Metric");

            mf.isMetric = true;
            Properties.Settings.Default.setMenu_isMetric = mf.isMetric;

            isClosing = true;
            Close();
            //FormConfig_Load(this, e);
        }

        private void nudNumGuideLines_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.trk.numGuideLines = (int)nudNumGuideLines.Value;
            }
        }
    }
}