using AOG.Classes;

using AOG.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormToolSteer : Form
    {
        private readonly FormGPS mf = null;

        private bool toSend = false, toolSend = false, toolSend2 = false, isSA = false;
        private int counter = 0, secondCntr = 0, cntr, toolCounterSettings = 0, toolCounterConfig = 0;
        private vec3 startFix;
        private double diameter, steerAngleRight, dist;
        private int windowSizeState = 0;

        //Form stuff
        public FormToolSteer(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.Text = gStr.Get(gs.gsToolSteerConfiguration);
            this.Width = 390;
            this.Height = 550;

            label19.Text = gStr.Get(gs.gsSpeedFactor);
            label82.Text = gStr.Get(gs.gsAquireFactor);
            label51.Text = gStr.Get(gs.gsDeadzone);
            label49.Text = gStr.Get(gs.gsHeading);
            label54.Text = gStr.Get(gs.gsOnDelay);

            lblGain.Text = gStr.Get(gs.gsProportionalGain);
        }

        private void FormSteer_Load(object sender, EventArgs e)
        {
            Location = Settings.User.setWindow_steerSettingsLocation;
            //WAS Zero, CPD

            //hsbarIntegral_Tool.Value = (int)(Settings.Tool.stanleyIntegralGainAB * 100);
            //lblIntegral_Tool.Text = ((int)(mf.vehicle.stanleyIntegralGainAB * 100)).ToString();

            //nudDeadZoneDistance.Value = (decimal)((double)(Properties.Settings.Default.setAS_deadZoneDistance)/10);
            nudDeadZoneHeading.Value = Settings.Vehicle.setAS_deadZoneHeading * 0.01;
            nudDeadZoneDelay.Value = mf.vehicle.deadZoneDelay;

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }

            cboxGPSTool.Checked = mf.isGPSToolActive;

            //settings
            hsbarPGain_Tool.Value = Settings.Tool.setToolSteer.gainP;
            hsbarIntegral_Tool.Value = Settings.Tool.setToolSteer.integral;
            hsbarMinPWM_Tool.Value = Settings.Tool.setToolSteer.minPWM;
            hsbarHighPWM_Tool.Value = Settings.Tool.setToolSteer.highPWM;
            hsbarAckermann_Tool.Value = Settings.Tool.setToolSteer.ackermann;
            hsbarCPD_Tool.Value = Settings.Tool.setToolSteer.countsPerDegree;
            hsbarZeroWAS_Tool.Value = Settings.Tool.setToolSteer.wasOffset;

            //config
            hsbarMaxSteerAngle_Tool.Value = Settings.Tool.setToolSteer.maxSteerAngle;
            cboxInvertSteer_Tool.Checked = (Settings.Tool.setToolSteer.isInvertSteer == 1);
            cboxInvertWAS_Tool.Checked = (Settings.Tool.setToolSteer.isInvertWAS == 1);
            cboxIsSteerNotSlide_Tool.Checked = (Settings.Tool.setToolSteer.isSteerNotSlide == 1);

            //settings
            lblPGain_Tool.Text = hsbarPGain_Tool.Value.ToString();
            lblIntegral_Tool.Text = hsbarIntegral_Tool.Value.ToString();
            lblMinPWM_Tool.Text = hsbarMinPWM_Tool.Value.ToString();
            lblHighPWM_Tool.Text = hsbarHighPWM_Tool.Value.ToString();
            lblAckermann_Tool.Text = hsbarAckermann_Tool.Value.ToString();
            lblZeroWAS_Tool.Text = (hsbarZeroWAS_Tool.Value / (double)(hsbarCPD_Tool.Value)).ToString("N2");
            lblCPD_Tool.Text = hsbarCPD_Tool.Value.ToString();

            //config
            lblMaxSteerAngle_Tool.Text = hsbarMaxSteerAngle_Tool.Value.ToString();

            //antenna
            nudAntennaHeight_Tool.Value = Settings.Tool.setToolSteer.antennaHeight;
            nudAntennaOffset_Tool.Value = Settings.Tool.setToolSteer.antennaOffset;

        }

        private void FormSteer_FormClosing(object sender, FormClosingEventArgs e)
        {

            //settings
            Settings.Tool.setToolSteer.gainP = (byte)hsbarPGain_Tool.Value;
            Settings.Tool.setToolSteer.integral = (byte)hsbarIntegral_Tool.Value;
            Settings.Tool.setToolSteer.minPWM = (byte)hsbarMinPWM_Tool.Value;
            Settings.Tool.setToolSteer.highPWM = (byte)hsbarHighPWM_Tool.Value;
            Settings.Tool.setToolSteer.countsPerDegree = (byte)hsbarCPD_Tool.Value;
            Settings.Tool.setToolSteer.ackermann = (byte)hsbarAckermann_Tool.Value;
            Settings.Tool.setToolSteer.wasOffset = hsbarZeroWAS_Tool.Value;

            //config
            Settings.Tool.setToolSteer.maxSteerAngle = (byte)hsbarMaxSteerAngle_Tool.Value;

            if (cboxInvertSteer_Tool.Checked) Settings.Tool.setToolSteer.isInvertWAS = 1;
            else Settings.Tool.setToolSteer.isInvertWAS = 0;

            if (cboxInvertWAS_Tool.Checked) Settings.Tool.setToolSteer.isInvertSteer = 1;
            else Settings.Tool.setToolSteer.isInvertSteer = 0;

            if (cboxIsSteerNotSlide_Tool.Checked) Settings.Tool.setToolSteer.isSteerNotSlide = 1;
            else Settings.Tool.setToolSteer.isSteerNotSlide = 0;

            PGN_232.pgn[PGN_232.gainP] = Settings.Tool.setToolSteer.gainP;
            PGN_232.pgn[PGN_232.integral] = Settings.Tool.setToolSteer.integral;
            PGN_232.pgn[PGN_232.minPWM] = Settings.Tool.setToolSteer.minPWM;
            PGN_232.pgn[PGN_232.highPWM] = Settings.Tool.setToolSteer.highPWM;
            PGN_232.pgn[PGN_232.countsPerDegree] = Settings.Tool.setToolSteer.countsPerDegree;
            PGN_232.pgn[PGN_232.ackerman] = Settings.Tool.setToolSteer.ackermann;

            PGN_232.pgn[PGN_232.wasOffsetHi] = unchecked((byte)(Settings.Tool.setToolSteer.wasOffset >> 8));
            PGN_232.pgn[PGN_232.wasOffsetLo] = unchecked((byte)(Settings.Tool.setToolSteer.wasOffset));

            //config
            PGN_231.pgn[PGN_231.maxSteerAngle] = Settings.Tool.setToolSteer.maxSteerAngle;
            PGN_231.pgn[PGN_231.invertWAS] = Settings.Tool.setToolSteer.isInvertWAS;
            PGN_231.pgn[PGN_231.invertSteer] = Settings.Tool.setToolSteer.isInvertSteer;
            PGN_231.pgn[PGN_231.isSteer] = Settings.Tool.setToolSteer.isSteerNotSlide;

            //save current vehicle
            Settings.Tool.Save();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //limit how many pgns are set when doing the settings
            counter++;
            toolCounterSettings++;
            toolCounterConfig++;

            lblPWMDisplay.Text = mf.mc.pwmToolDisplay.ToString();


            //tool settings
            if (toolSend && toolCounterSettings > 4)
            {
                PGN_232.pgn[PGN_232.gainP] = (byte)hsbarPGain_Tool.Value;
                PGN_232.pgn[PGN_232.integral] = (byte)hsbarIntegral_Tool.Value;
                PGN_232.pgn[PGN_232.minPWM] = (byte)hsbarMinPWM_Tool.Value;
                PGN_232.pgn[PGN_232.highPWM] = (byte)hsbarHighPWM_Tool.Value;
                PGN_232.pgn[PGN_232.countsPerDegree] = (byte)hsbarCPD_Tool.Value;
                PGN_232.pgn[PGN_232.ackerman] = (byte) hsbarAckermann_Tool.Value;

                PGN_232.pgn[PGN_232.wasOffsetHi] = unchecked((byte)(hsbarZeroWAS_Tool.Value >> 8));
                PGN_232.pgn[PGN_232.wasOffsetLo] = unchecked((byte)(hsbarZeroWAS_Tool.Value));

                mf.SendPgnToLoopTool(PGN_232.pgn);
                toolCounterSettings = 0;
                toolSend = false;
            }

            //tool config
            if (toolSend2 && toolCounterConfig > 4)
            {
                PGN_231.pgn[PGN_231.maxSteerAngle] = unchecked((byte)hsbarMaxSteerAngle_Tool.Value);

                if (cboxInvertSteer_Tool.Checked) PGN_231.pgn[PGN_231.invertSteer] = 1;
                else PGN_231.pgn[PGN_231.invertSteer] = 0;

                if (cboxInvertWAS_Tool.Checked) PGN_231.pgn[PGN_231.invertWAS] = 1;
                else PGN_231.pgn[PGN_231.invertWAS] = 0;

                if (cboxIsSteerNotSlide_Tool.Checked) PGN_231.pgn[PGN_231.isSteer] = 1;
                else PGN_231.pgn[PGN_231.isSteer] = 0;

                mf.SendPgnToLoopTool(PGN_231.pgn);

                toolCounterConfig = 0;
                toolSend2 = false;
            }
        }



        #region Tab On the Line

        private void tabOnTheLine_Enter(object sender, EventArgs e)
        {

            label20.Text = glm.unitsInCm;
        }

        private void tabOnTheLine_Leave(object sender, EventArgs e)
        {
        }

        #endregion Tab On the Line

        #region Tab Tool Steer

        private void tabTool_Enter(object sender, EventArgs e)
        {
        }

        private void tabTool_Leave(object sender, EventArgs e)
        { 
        }

        private void cboxGPSTool_Click(object sender, EventArgs e)
        {
            mf.isGPSToolActive = cboxGPSTool.Checked;
            mf.YesMessageBox("You must restart AOG to make changes to the networking");
            Log.EventWriter("GPS Tool set to: " + cboxGPSTool.Checked.ToString());
            Settings.Tool.setToolSteer.isGPSToolActive = mf.isGPSToolActive;
        }

        //config tool 
        private void cboxInvertWAS_Tool_Click(object sender, EventArgs e)
        {
            toolSend2 = true;
            toolCounterConfig = 0;
        }

        private void cboxInvertSteer_Tool_Click(object sender, EventArgs e)
        {
            toolSend2 = true;
            toolCounterConfig = 0;
        }
        private void cboxIsSteerNotSlide_Click(object sender, EventArgs e)
        {
            toolSend2 = true;
            toolCounterConfig = 0;
        }

        private void nudAntennaHeight_Tool_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setToolSteer.antennaHeight = nudAntennaHeight_Tool.Value;
        }

        private void nudAntennaOffset_Tool_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setToolSteer.antennaOffset = nudAntennaOffset_Tool.Value;
        }

        private void hsbarMaxSteerAngle_Tool_Scroll(object sender, ScrollEventArgs e)
        {
            lblMaxSteerAngle_Tool.Text = e.NewValue.ToString();
            toolSend2 = true;
            toolCounterConfig = 0;
        }

        //settings tool
        private void btnZeroWAS_Tool_Click(object sender, EventArgs e)
        {
            hsbarZeroWAS_Tool.Value += (int)(hsbarCPD_Tool.Value * -mf.mc.actualToolAngleDegrees);
            lblZeroWAS_Tool.Text = (hsbarZeroWAS_Tool.Value / (double)(hsbarCPD_Tool.Value)).ToString("N2");
            toolSend = true;
            toolCounterSettings = 0;
        }

        private void hsbarPGain_Tool_Scroll(object sender, ScrollEventArgs e)
        {
            lblPGain_Tool.Text = e.NewValue.ToString();
            toolSend = true;
            toolCounterSettings = 0;
        }

        private void hsbarHighPWM_Tool_Scroll(object sender, ScrollEventArgs e)
        {
            lblHighPWM_Tool.Text = e.NewValue.ToString();
            toolSend = true;
            toolCounterSettings = 0;
        }

        private void hsbarMinPWM_Tool_Scroll(object sender, ScrollEventArgs e)
        {
            lblMinPWM_Tool.Text = e.NewValue.ToString();
            toolSend = true;
            toolCounterSettings = 0;
        }

        private void hsbarZeroWAS_Tool_Scroll(object sender, ScrollEventArgs e)
        {
            lblZeroWAS_Tool.Text = (e.NewValue / (double)(hsbarCPD_Tool.Value)).ToString("N2");
            toolSend = true;
            toolCounterSettings = 0;
        }

        private void hsbarCPD_Tool_Scroll(object sender, ScrollEventArgs e)
        {
            lblCPD_Tool.Text = e.NewValue.ToString();

            lblCPD_Tool.Text = hsbarCPD_Tool.Value.ToString();

            lblZeroWAS_Tool.Text = (hsbarZeroWAS_Tool.Value / (double)(hsbarCPD_Tool.Value)).ToString("N2");
            toolSend = true;
            toolCounterSettings = 0;
        }

        private void hsbarAckermann_Tool_Scroll(object sender, ScrollEventArgs e)
        {
            lblAckermann_Tool.Text = e.NewValue.ToString();
            toolSend = true;
            toolCounterSettings = 0;
        }

        private void hsbarIntegral_Tool_Scroll(object sender, ScrollEventArgs e)
        {
            lblIntegral_Tool.Text = e.NewValue.ToString();
            toolSend = true;
            toolCounterSettings = 0;
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
                this.Size = new System.Drawing.Size(910,550);
                btnExpand.Image = Properties.Resources.ArrowLeft;
            }
            else if (windowSizeState == 0)
            {
                this.Size = new System.Drawing.Size(390, 550);
                btnExpand.Image = Properties.Resources.ArrowRight;
            }
        }

        #endregion
    }
}
