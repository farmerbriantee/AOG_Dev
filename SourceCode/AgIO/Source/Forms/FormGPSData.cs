//Please, if you use this give me some credit
//Copyright BrianTee, copy right out of it.

using System;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormGPSData : Form
    {
        private readonly FormLoop mf = null;

        public FormGPSData(Form callingForm)
        {
            mf = callingForm as FormLoop;
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblLatitude.Text = mf.pnGPS.latitude.ToString("N7");
            lblLongitude.Text = mf.pnGPS.longitude.ToString("N7");

            ////other sat and GPS info
            lblFixQuality.Text = mf.pnGPS.FixQuality;
            lblSatsTracked.Text = mf.pnGPS.satellitesData.ToString();
            lblHDOP.Text = mf.pnGPS.hdopData.ToString();
            lblSpeed.Text = mf.pnGPS.speedData.ToString("N1");

            lblRoll.Text = mf.pnGPS.rollData.ToString("N2");
            lblIMURoll.Text = mf.pnGPS.imuRollData.ToString();
            lblIMUPitch.Text = mf.pnGPS.imuPitchData.ToString();
            lblIMUYawRate.Text = mf.pnGPS.imuYawRateData.ToString();
            lblIMUHeading.Text = mf.pnGPS.imuHeadingData.ToString();

            lblAge.Text = mf.pnGPS.ageData.ToString("N1");

            lblGPSHeading.Text = mf.pnGPS.headingTrueData.ToString("N2");
            lblDualHeading.Text = mf.pnGPS.headingTrueDualData.ToString("N2");

            lblAltitude.Text = mf.pnGPS.altitudeData.ToString("N1");

            tboxVTG.Text = mf.pnGPS.vtgSentence;
            tboxGGA.Text = mf.pnGPS.ggaSentence;
            tboxPAOGI.Text = mf.pnGPS.paogiSentence;
            tboxAVR.Text = mf.pnGPS.avrSentence;
            tboxHDT.Text = mf.pnGPS.hdtSentence;
            tboxHPD.Text = mf.pnGPS.hpdSentence;
            tboxPANDA.Text = mf.pnGPS.pandaSentence;
            tboxKSXT.Text = mf.pnGPS.ksxtSentence;
        }

        private void FormGPSData_Load(object sender, EventArgs e)
        {
            tboxGGA.Text = "";
            tboxVTG.Text = "";
            tboxHDT.Text = "";
            tboxAVR.Text = "";
            tboxPAOGI.Text = "";
            tboxHPD.Text = "";
            tboxPANDA.Text = "";
            tboxKSXT.Text = "";
        }

        private void FormGPSData_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.isGPSSentencesOn = false;
        }
    }
}