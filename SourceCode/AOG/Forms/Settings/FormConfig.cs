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

            lblSaveAs.Text = gStr.Get(gs.gsSaveAs);
            lblNew.Text = gStr.Get(gs.gsNew);
            lblToolWidth.Text = gStr.Get(gs.gsWidth);
            lblOpen.Text = gStr.Get(gs.gsOpen);
            lblDelete.Text = gStr.Get(gs.gsDelete);
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
            lblBoundary.Text = gStr.Get(gs.gsBoundary);
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
            lblUturnSmoothing.Text = gStr.Get(gs.gsUturnSmooth);

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

            HideSubMenu();
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

            lblSaveAs.Text = gStr.Get(gs.gsSaveAs);
            lblNew.Text = gStr.Get(gs.gsNew);
            UpdateSummary();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            //reload all the settings from default and user.config
            mf.LoadSettings();

            //save current vehicle
            Properties.Settings.Default.Save();
        }

        private void FixMinMaxSpinners()
        {
            nudTankHitch.Maximum = (Math.Round(nudTankHitch.Maximum / 2.54));
            nudTankHitch.Minimum = Math.Round(nudTankHitch.Minimum / 2.54);

            nudDrawbarLength.Maximum = Math.Round(nudDrawbarLength.Maximum / 2.54);
            nudDrawbarLength.Minimum = Math.Round(nudDrawbarLength.Minimum / 2.54);

            nudTrailingHitchLength.Maximum = Math.Round(nudTrailingHitchLength.Maximum / 2.54);
            nudTrailingHitchLength.Minimum = Math.Round(nudTrailingHitchLength.Minimum / 2.54);

            nudTractorHitchLength.Maximum = Math.Round(nudTractorHitchLength.Maximum / 2.54);
            nudTractorHitchLength.Minimum = Math.Round(nudTractorHitchLength.Minimum / 2.54);

            nudVehicleTrack.Maximum = Math.Round(nudVehicleTrack.Maximum / 2.54);
            nudVehicleTrack.Minimum = Math.Round(nudVehicleTrack.Minimum / 2.54);

            nudWheelbase.Maximum = Math.Round(nudWheelbase.Maximum / 2.54);
            nudWheelbase.Minimum = Math.Round(nudWheelbase.Minimum / 2.54);

            nudOverlap.Maximum = Math.Round(nudOverlap.Maximum / 2.54);
            nudOverlap.Minimum = Math.Round(nudOverlap.Minimum / 2.54);

            nudOffset.Maximum = Math.Round(nudOffset.Maximum / 2.54);
            nudOffset.Minimum = Math.Round(nudOffset.Minimum / 2.54);

            nudDefaultSectionWidth.Maximum = Math.Round(nudDefaultSectionWidth.Maximum / 2.54);
            nudDefaultSectionWidth.Minimum = Math.Round(nudDefaultSectionWidth.Minimum / 2.54);

            nudSection01.Maximum = Math.Round(nudSection01.Maximum / 2.54);
            nudSection01.Minimum = Math.Round(nudSection01.Minimum / 2.54);
            nudSection02.Maximum = Math.Round(nudSection02.Maximum / 2.54);
            nudSection02.Minimum = Math.Round(nudSection02.Minimum / 2.54);
            nudSection03.Maximum = Math.Round(nudSection03.Maximum / 2.54);
            nudSection03.Minimum = Math.Round(nudSection03.Minimum / 2.54);
            nudSection04.Maximum = Math.Round(nudSection04.Maximum / 2.54);
            nudSection04.Minimum = Math.Round(nudSection04.Minimum / 2.54);
            nudSection05.Maximum = Math.Round(nudSection05.Maximum / 2.54);
            nudSection05.Minimum = Math.Round(nudSection05.Minimum / 2.54);
            nudSection06.Maximum = Math.Round(nudSection06.Maximum / 2.54);
            nudSection06.Minimum = Math.Round(nudSection06.Minimum / 2.54);
            nudSection07.Maximum = Math.Round(nudSection07.Maximum / 2.54);
            nudSection07.Minimum = Math.Round(nudSection07.Minimum / 2.54);
            nudSection08.Maximum = Math.Round(nudSection08.Maximum / 2.54);
            nudSection08.Minimum = Math.Round(nudSection08.Minimum / 2.54);
            nudSection09.Maximum = Math.Round(nudSection09.Maximum / 2.54);
            nudSection09.Minimum = Math.Round(nudSection09.Minimum / 2.54);
            nudSection10.Maximum = Math.Round(nudSection10.Maximum / 2.54);
            nudSection10.Minimum = Math.Round(nudSection10.Minimum / 2.54);
            nudSection11.Maximum = Math.Round(nudSection11.Maximum / 2.54);
            nudSection11.Minimum = Math.Round(nudSection11.Minimum / 2.54);
            nudSection12.Maximum = Math.Round(nudSection12.Maximum / 2.54);
            nudSection12.Minimum = Math.Round(nudSection12.Minimum / 2.54);
            nudSection13.Maximum = Math.Round(nudSection13.Maximum / 2.54);
            nudSection13.Minimum = Math.Round(nudSection13.Minimum / 2.54);
            nudSection14.Maximum = Math.Round(nudSection14.Maximum / 2.54);
            nudSection14.Minimum = Math.Round(nudSection14.Minimum / 2.54);
            nudSection15.Maximum = Math.Round(nudSection15.Maximum / 2.54);
            nudSection15.Minimum = Math.Round(nudSection15.Minimum / 2.54);
            nudSection16.Maximum = Math.Round(nudSection16.Maximum / 2.54);
            nudSection16.Minimum = Math.Round(nudSection16.Minimum / 2.54);

            nudTramWidth.Minimum = Math.Round(nudTramWidth.Minimum / 2.54);
            nudTramWidth.Maximum = Math.Round(nudTramWidth.Maximum / 2.54);

            //Meters to feet
            nudTurnDistanceFromBoundary.Minimum = Math.Round(nudTurnDistanceFromBoundary.Minimum * 3.28);
            nudTurnDistanceFromBoundary.Maximum = Math.Round(nudTurnDistanceFromBoundary.Maximum * 3.28);

            nudOffset.Maximum = Math.Round(nudOffset.Maximum / 2.54);
            nudOffset.Minimum = Math.Round(nudOffset.Minimum / 2.54);
            nudOverlap.Maximum = Math.Round(nudOverlap.Maximum / 2.54);
            nudOverlap.Minimum = Math.Round(nudOverlap.Minimum / 2.54);

            nudTrailingToolToPivotLength.Maximum = Math.Round(nudTrailingToolToPivotLength.Maximum / 2.54);
            nudTrailingToolToPivotLength.Minimum = Math.Round(nudTrailingToolToPivotLength.Minimum / 2.54);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
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

            Close();
        }

        private void rbtnDisplayMetric_Click(object sender, EventArgs e)
        {
            mf.TimedMessageBox(2000, "Units Set", "Metric");
            Log.EventWriter("Units to Metric");

            mf.isMetric = true;
            Properties.Settings.Default.setMenu_isMetric = mf.isMetric;

            Close();
            //FormConfig_Load(this, e);
        }

        private void nudNumGuideLines_ValueChanged(object sender, EventArgs e)
        {
            mf.trk.numGuideLines = (int)nudNumGuideLines.Value;
        }
    }
}