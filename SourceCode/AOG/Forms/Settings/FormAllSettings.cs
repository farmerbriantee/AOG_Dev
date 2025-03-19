//Please, if you use this, share the improvements

using AOG.Properties;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormAllSettings : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        //Nozzz constructor
        public FormAllSettings(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            this.Text = "Name *****";
        }

        private void LoadLabels()
        {
            label4.Text = Settings.Vehicle.setVehicle_maxSteerAngle.ToString();
            label6.Text = Settings.Vehicle.setAS_countsPerDegree.ToString();
            label8.Text = Settings.Vehicle.setAS_ackerman.ToString();
            label10.Text = Settings.Vehicle.setAS_wasOffset.ToString();
            label12.Text = Settings.Vehicle.setAS_highSteerPWM.ToString();
            label14.Text = Settings.Vehicle.setAS_lowSteerPWM.ToString();
            label16.Text = Settings.Vehicle.setAS_minSteerPWM.ToString();
            label18.Text = Settings.Vehicle.setAS_Kp.ToString();
            label20.Text = Settings.Vehicle.setVehicle_panicStopSpeed.ToString();

            label22.Text = Settings.Vehicle.setVehicle_goalPointAcquireFactor.ToString("N2");
            label24.Text = Settings.Vehicle.setVehicle_goalPointLookAheadHold.ToString();
            label168.Text = Settings.Vehicle.setVehicle_goalPointLookAheadMult.ToString();
            label26.Text = Settings.Vehicle.stanleyHeadingErrorGain.ToString();
            label28.Text = Settings.Vehicle.stanleyDistanceErrorGain.ToString();
            label30.Text = Settings.Vehicle.stanleyIntegralGainAB.ToString();
            label32.Text = Settings.Vehicle.setAS_sideHillComp.ToString();
            label34.Text = Settings.Vehicle.setVehicle_wheelbase.ToString();
            label36.Text = Settings.Vehicle.setVehicle_trackWidth.ToString();
            label38.Text = Settings.Vehicle.setVehicle_antennaPivot.ToString();
            label40.Text = Settings.Vehicle.setVehicle_antennaHeight.ToString();
            label42.Text = Settings.Vehicle.setVehicle_antennaOffset.ToString();
            label46.Text = Settings.Vehicle.setIMU_rollZero.ToString();
            label48.Text = Settings.Vehicle.setAS_purePursuitIntegralGain.ToString();
            label50.Text = (Settings.Vehicle.setAS_snapDistance * glm.m2InchOrCm).ToString();
            label52.Text = (Settings.Vehicle.setAS_snapDistanceRef * glm.m2InchOrCm).ToString();
            label56.Text = Settings.User.isAutoStartAgIO.ToString();
            label58.Text = Settings.User.isAutoOffAgIO.ToString();

            label60.Text = RegistrySettings.culture;
            label62.Text = Settings.Vehicle.setF_CurrentFieldDir;
            label64.Text = "~~~";
            label66.Text = Settings.Vehicle.setF_isSteerWorkSwitchEnabled.ToString();
            label68.Text = Settings.Vehicle.setF_isSteerWorkSwitchManualSections.ToString();
            label70.Text = Settings.Vehicle.setF_isWorkSwitchActiveLow.ToString();
            label72.Text = Settings.Vehicle.setF_isWorkSwitchEnabled.ToString();
            label74.Text = Settings.Vehicle.setF_isWorkSwitchManualSections.ToString();
            label76.Text = Settings.Vehicle.setF_minHeadingStepDistance.ToString();
            label78.Text = RegistrySettings.workingDirectory;
            label80.Text = Settings.Vehicle.setGPS_ageAlarm.ToString();
            label82.Text = Settings.Vehicle.setGPS_dualHeadingOffset.ToString();
            label84.Text = Settings.Vehicle.setGPS_dualReverseDetectionDistance.ToString();
            label88.Text = Settings.Vehicle.setGPS_headingFromWhichSource.ToString();
            label92.Text = Settings.Vehicle.setGPS_isRTK.ToString();
            label94.Text = Settings.Vehicle.setGPS_isRTK_KillAutoSteer.ToString();
            label96.Text = Settings.Vehicle.setGPS_minimumStepLimit.ToString();
            label98.Text = Settings.Vehicle.setHeadland_isSectionControlled.ToString();
            label100.Text = Settings.Vehicle.setIMU_fusionWeight2.ToString();
            label102.Text = Settings.Vehicle.setIMU_invertRoll.ToString();
            label106.Text = Settings.Vehicle.setIMU_isReverseOn.ToString();
            label108.Text = Settings.Vehicle.setIMU_rollFilter.ToString();
            label114.Text = Settings.Tool.isSectionsNotZones.ToString();
            label116.Text = Settings.Tool.isToolFront.ToString();
            label118.Text = Settings.Tool.isToolRearFixed.ToString();
            label120.Text = Settings.Tool.isToolTBT.ToString();
            label122.Text = Settings.Tool.isToolTrailing.ToString();

            label124.Text = Settings.Tool.toolTrailingHitchLength.ToString();
            label126.Text = Settings.Tool.trailingToolToPivotLength.ToString();
            label128.Text = Settings.Tool.hitchLength.ToString();
            label130.Text = Settings.Tool.hydraulicLiftLookAhead.ToString();
            label132.Text = "~~~";
            label134.Text = Settings.Vehicle.setVehicle_isStanleyUsed.ToString();
            label136.Text = "~~~";
            label138.Text = Settings.Vehicle.setVehicle_maxAngularVelocity.ToString();
            label140.Text = Settings.Vehicle.set_youTurnRadius.ToString();
            label142.Text = Settings.Tool.numSections.ToString();
            label144.Text = Settings.Tool.slowSpeedCutoff.ToString();
            label146.Text = Settings.Tool.tankTrailingHitchLength.ToString();
            label148.Text = Settings.Tool.lookAheadOn.ToString();
            label150.Text = Settings.Tool.lookAheadOff.ToString();
            label152.Text = Settings.Tool.offDelay.ToString();
            label154.Text = Settings.Tool.offset.ToString();
            label156.Text = Settings.Tool.overlap.ToString();
            label158.Text = Settings.Tool.toolWidth.ToString();
            label160.Text = RegistrySettings.workingDirectory;
            label162.Text = Settings.Vehicle.setVehicle_vehicleType.ToString();
            label164.Text = Settings.Vehicle.setAS_isSteerInReverse.ToString();

            label251.Text = Settings.Vehicle.setAS_deadZoneDelay.ToString();
            label252.Text = Settings.Vehicle.setAS_deadZoneHeading.ToString();

            lblFrameTime.Text = mf.frameTime.ToString("N1");
            lblTimeSlice.Text = (1 / mf.timeSliceOfLastFix).ToString("N3");
            lblHz.Text = mf.gpsHz.ToString("N1");

            lblEastingField.Text = Math.Round(mf.pivotAxlePos.easting, 2).ToString();
            lblNorthingField.Text = Math.Round(mf.pivotAxlePos.northing, 2).ToString();

            //lblLatitude.Text = mf.Latitude;
            //lblLongitude.Text = mf.Longitude;

            //lblEastingField2.Text = Math.Round(mf.pnTool.fix.easting, 2).ToString();
            //lblNorthingField2.Text = Math.Round(mf.pnTool.fix.northing, 2).ToString();

            //lblLatitude2.Text = mf.pnTool.latitude.ToString("N7");
            //lblLongitude2.Text = mf.pnTool.longitude.ToString("N7");

            //other sat and GPS info
            lblSatsTracked.Text = mf.SatsTracked;
            lblHDOP.Text = mf.HDOP;

            lblIMUHeading.Text = mf.GyroInDegrees;
            lblFix2FixHeading.Text = mf.GPSHeading;
            lblFuzeHeading.Text = glm.toDegrees(mf.fixHeading).ToString("N1");

            lblAngularVelocity.Text = mf.ahrs.imuYawRate.ToString("N2");

            lbludpWatchCounts.Text = mf.missedSentenceCount.ToString();

            lblAltitude.Text = mf.Altitude;

            label254.Text = mf.FixQuality;
        }

        private void btnScreenShot_Click(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bm, new Rectangle(0, 0, this.Width, this.Height));
            Clipboard.SetImage(bm);
            mf.TimedMessageBox(2000, "Captured", "Copied to Clipboard, Paste (CTRL-V) in Telegram");
            Log.EventWriter("View All Settings to Clipboard");
        }

        private void btnCreatePNG_Click(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bm, new Rectangle(0, 0, this.Width, this.Height));
            bm.Save(Path.Combine(RegistrySettings.baseDirectory, "AllSet.PNG"), ImageFormat.Png);
            System.Diagnostics.Process.Start("explorer.exe", RegistrySettings.baseDirectory);
            Log.EventWriter("View All Settings to PNG");
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadLabels();
        }
    }
}