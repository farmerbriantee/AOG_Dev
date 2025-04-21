//Please, if you use this, share the improvements

using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AOG
{
    public partial class FormNozConfig : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private string unitsSet = "10";
        private string unitsActual = "0";

        //Nozzz constructor
        public FormNozConfig(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            this.Text = "Controller Configuration";
        }

        private void FormDisplaySettings_Load(object sender, EventArgs e)
        {
            nudSprayFlowCal.Value = Settings.Tool.setNozz.flowCal;
            nudSprayPressureCal.Value = Settings.Tool.setNozz.pressureCal;

            nudManualPWM.Value = Settings.Tool.setNozz.Kp;
            //nudSprayKi.Value = Settings.Tool.setNozz.Ki;

            nudFastPWM.Value = Settings.Tool.setNozz.fastPWM;
            nudSlowPWM.Value = Settings.Tool.setNozz.slowPWM;
            nudDeadbandError.Value = Settings.Tool.setNozz.deadbandError;
            nudSwitchAtFlowError.Value = Settings.Tool.setNozz.switchAtFlowError;
            
            nudKp.Value = Settings.Tool.setNozz.Kp;
            //nudKi.Value = Settings.Tool.setNozz.Ki;

            cboxBypass.Checked = Settings.Tool.setNozz.isBypass;
            if (cboxBypass.Checked)
            {
                cboxBypass.Text = "Bypass";
            }
            else
            {
                cboxBypass.Text = "Closed";
            }

            cboxMeteringOrFlow.Checked = Settings.Tool.setNozz.isMeter;
            if (cboxMeteringOrFlow.Checked)
            {
                cboxMeteringOrFlow.Text = "Metering";
            }
            else
            {
                cboxMeteringOrFlow.Text = "Flow";
            }

            tboxUnitsApplied.Text = Settings.Tool.setNozz.unitsApplied.Trim();
            tboxUnitsPerArea.Text = Settings.Tool.setNozz.unitsPerArea.Trim();

            cboxSectionValve3Wire.Checked = Settings.Tool.setNozz.isSectionValve3Wire;
            if (cboxSectionValve3Wire.Checked)
            {
                cboxSectionValve3Wire.Text = "3 Wire";
            }
            else
            {
                cboxSectionValve3Wire.Text = "Reverse";
            }

            if(Settings.Tool.setNozz.isMeter)
            {
                nudFastPWM.Enabled = false;
                nudSlowPWM.Enabled = false;
                nudSwitchAtFlowError.Enabled = false;
                nudKp.Enabled = true;
            }
            else
            {
                nudFastPWM.Enabled = true;
                nudSlowPWM.Enabled = true;
                nudSwitchAtFlowError.Enabled = true;
                nudKp.Enabled = false;
            }
        }

        private void nudSprayFlowCal_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.flowCal = (int)nudSprayFlowCal.Value;

            PGN_226.pgn[PGN_226.flowCalHi] = unchecked((byte)(Settings.Tool.setNozz.flowCal >> 8));
            PGN_226.pgn[PGN_226.flowCalLo] = unchecked((byte)(Settings.Tool.setNozz.flowCal));

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        private void nudSprayPressureCal_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.pressureCal = (int)nudSprayPressureCal.Value;

            PGN_226.pgn[PGN_226.pressureCalHi] = unchecked((byte)(Settings.Tool.setNozz.pressureCal >> 8)); ;
            PGN_226.pgn[PGN_226.pressureCalLo] = unchecked((byte)(Settings.Tool.setNozz.pressureCal));

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        private void nudFastPWM_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.fastPWM = (byte)nudFastPWM.Value;

            PGN_226.pgn[PGN_226.fastPWM] = unchecked((byte)(Settings.Tool.setNozz.fastPWM));

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        private void nudSlowPWM_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.slowPWM = (byte)nudSlowPWM.Value;

            PGN_226.pgn[PGN_226.slowPWM] = unchecked((byte)(Settings.Tool.setNozz.slowPWM));

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        private void nudDeadbandError_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.deadbandError = (byte)(nudDeadbandError.Value);

            PGN_226.pgn[PGN_226.deadbandError] = unchecked((byte)(Settings.Tool.setNozz.deadbandError));

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        private void nudSwitchAtFlowError_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.switchAtFlowError = (byte)(nudSwitchAtFlowError.Value);

            PGN_226.pgn[PGN_226.switchAtFlowError] = unchecked((byte)(Settings.Tool.setNozz.switchAtFlowError));

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        private void nudManualPWM_ValueChanged(object sender, EventArgs e)
        {
            //Manual PWM up and down value
            Settings.Tool.setNozz.manualRate = (byte)nudManualPWM.Value;

            PGN_225.pgn[PGN_225.rate] = unchecked((byte)(Settings.Tool.setNozz.manualRate));

            mf.SendPgnToLoop(PGN_225.pgn);
        }
        private void nudKp_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.Kp = (byte)(nudKp.Value);

            PGN_226.pgn[PGN_226.Kp] = unchecked((byte)(Settings.Tool.setNozz.Kp));

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        ////private void nudKi_ValueChanged(object sender, EventArgs e)
        ////{
        ////    Settings.Tool.setNozz.Ki = (byte)(nudKi.Value);

        ////    PGN_226.pgn[PGN_226.Ki] = unchecked((byte)(Settings.Tool.setNozz.Ki));

        ////    mf.SendPgnToLoop(PGN_226.pgn);
        ////}

        private void cboxSectionValve3Wire_Click(object sender, EventArgs e)
        {
            if (cboxSectionValve3Wire.Checked)
            {
                cboxSectionValve3Wire.Text = "3 Wire";
                Settings.Tool.setNozz.isSectionValve3Wire = true;
                PGN_226.pgn[PGN_226.isSectionValve3Wire] = 1;
            }
            else
            {
                cboxSectionValve3Wire.Text = "Reverse";
                Settings.Tool.setNozz.isSectionValve3Wire = false;
                PGN_226.pgn[PGN_226.isSectionValve3Wire] = 0;
            }

            Settings.Tool.setNozz.isSectionValve3Wire = Settings.Tool.setNozz.isSectionValve3Wire;

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        private void cboxBypass_Click(object sender, EventArgs e)
        {
            if (cboxBypass.Checked)
            {
                cboxBypass.Text = "Bypass";
                Settings.Tool.setNozz.isBypass = true;

                if (Settings.Tool.setNozz.isBypass)
                    PGN_226.pgn[PGN_226.isBypass] = 1;
                else
                    PGN_226.pgn[PGN_226.isBypass] = 0;

                if (Settings.Tool.setNozz.isMeter)
                    PGN_226.pgn[PGN_226.isBypass] += 2;
            }
            else
            {
                cboxBypass.Text = "Closed";
                Settings.Tool.setNozz.isBypass = false;
            }

            if (Settings.Tool.setNozz.isBypass)
                PGN_226.pgn[PGN_226.isBypass] = 1;
            else
                PGN_226.pgn[PGN_226.isBypass] = 0;

            if (Settings.Tool.setNozz.isMeter)
                PGN_226.pgn[PGN_226.isBypass] += 2;

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        private void cboxMeteringOrFlow_Click(object sender, EventArgs e)
        {
            if (cboxMeteringOrFlow.Checked)
            {
                cboxMeteringOrFlow.Text = "Metering";
                Settings.Tool.setNozz.isMeter = true;

                if (Settings.Tool.setNozz.isBypass)
                    PGN_226.pgn[PGN_226.isBypass] = 1;
                else
                    PGN_226.pgn[PGN_226.isBypass] = 0;

                if (Settings.Tool.setNozz.isMeter)
                    PGN_226.pgn[PGN_226.isBypass] += 2;
            }
            else
            {
                cboxMeteringOrFlow.Text = "Flow";
                Settings.Tool.setNozz.isMeter = false;
            }

            if (Settings.Tool.setNozz.isBypass)
                PGN_226.pgn[PGN_226.isBypass] = 1;
            else
                PGN_226.pgn[PGN_226.isBypass] = 0;

            if (Settings.Tool.setNozz.isMeter)
                PGN_226.pgn[PGN_226.isBypass] += 2;

            mf.SendPgnToLoop(PGN_226.pgn);

            if (Settings.Tool.setNozz.isMeter)
            {
                nudFastPWM.Enabled = false;
                nudSlowPWM.Enabled = false;
                nudSwitchAtFlowError.Enabled = false;
                nudKp.Enabled = true;
            }
            else
            {
                nudFastPWM.Enabled = true;
                nudSlowPWM.Enabled = true;
                nudSwitchAtFlowError.Enabled = true;
                nudKp.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnPWM.Text = mf.nozz.pwmDriveActual.ToString();
            btnFreq.Text = mf.nozz.frequency.ToString();
            DrawChart();
        }

        private void DrawChart()
        {
            unitsSet = lblUnitsSet.Text = mf.nozz.volumePerMinuteSet.ToString();
            unitsActual = lblUnitsActual.Text = mf.nozz.volumePerMinuteActual.ToString();

            double errorText = (((double)(mf.nozz.volumePerMinuteSet - mf.nozz.volumePerMinuteActual) / (double)mf.nozz.volumePerMinuteSet) * -100);
            if (mf.nozz.volumePerMinuteSet > 0.05)
                lblFlowError.Text = errorText.ToString("N1") + "%";
            else
                lblFlowError.Text = "Off";

            //chart data
            Series s = unoChart.Series["S"];
            Series w = unoChart.Series["PWM"];

            double nextX = 1;
            double nextX5 = 1;

            if (s.Points.Count > 0) nextX = s.Points[s.Points.Count - 1].XValue + 1;
            if (w.Points.Count > 0) nextX5 = w.Points[w.Points.Count - 1].XValue + 1;

            unoChart.Series["S"].Points.AddXY(nextX, unitsSet);
            unoChart.Series["PWM"].Points.AddXY(nextX5, unitsActual);

            while (s.Points.Count > 50)
            {
                s.Points.RemoveAt(0);
            }
            while (w.Points.Count > 50)
            {
                w.Points.RemoveAt(0);
            }
            unoChart.ChartAreas[0].RecalculateAxesScale();
        }

        private void tboxUnitsApplied_TextChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.unitsApplied = " " + tboxUnitsApplied.Text.Trim();
        }

        private void tboxUnitsPerArea_TextChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.unitsPerArea = " " + tboxUnitsPerArea.Text.Trim();
        }

        private void FormNozConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Settings.Tool.setNozz.isAppliedUnitsNotTankDisplayed)
                mf.lbl_Volume.Text = "Tank " + Settings.Tool.setNozz.unitsApplied;
            else
                mf.lbl_Volume.Text = "App " + Settings.Tool.setNozz.unitsApplied;
        }

    }
}