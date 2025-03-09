//Please, if you use this, share the improvements

using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AgOpenGPS
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

            nudSprayKp.Value = Settings.Tool.setNozz.Kp;
            //nudSprayKi.Value = Settings.Tool.setNozz.Ki;

            nudFastPWM.Value = Settings.Tool.setNozz.fastPWM;
            nudSlowPWM.Value = Settings.Tool.setNozz.slowPWM;
            nudDeadbandError.Value = Settings.Tool.setNozz.deadbandError * 0.01;
            nudSwitchAtFlowError.Value = Settings.Tool.setNozz.switchAtFlowError * 0.01;

            cboxBypass.Checked = Settings.Tool.setNozz.isBypass;
            if (cboxBypass.Checked)
            {
                cboxBypass.Text = "Bypass";
            }
            else
            {
                cboxBypass.Text = "Closed";
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
        }

        private void nudSprayFlowCal_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.flowCal = (int)nudSprayFlowCal.Value;

            PGN_226.pgn[PGN_226.flowCalHi] = unchecked((byte)(Settings.Tool.setNozz.flowCal >> 8)); ;
            PGN_226.pgn[PGN_226.flowCaLo] = unchecked((byte)(Settings.Tool.setNozz.flowCal));

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
            Settings.Tool.setNozz.deadbandError = (byte)(nudDeadbandError.Value * 100);

            PGN_226.pgn[PGN_226.deadbandError] = unchecked((byte)(Settings.Tool.setNozz.deadbandError));

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        private void nudSwitchAtFlowError_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.switchAtFlowError = (byte)(nudSwitchAtFlowError.Value * 100);

            PGN_226.pgn[PGN_226.switchAtFlowError] = unchecked((byte)(Settings.Tool.setNozz.switchAtFlowError));

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        private void nudSprayKp_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.Kp = (byte)nudSprayKp.Value;

            PGN_226.pgn[PGN_226.Kp] = unchecked((byte)(Settings.Tool.setNozz.Kp));

            mf.SendPgnToLoop(PGN_226.pgn);
        }

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
                PGN_226.pgn[PGN_226.isBypass] = 1;
            }
            else
            {
                cboxBypass.Text = "Closed";
                Settings.Tool.setNozz.isBypass = false;
                PGN_226.pgn[PGN_226.isBypass] = 0;
            }

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnSprayAcres.Text = mf.nozz.pwmDriveActual.ToString();
            DrawChart();
        }

        private void DrawChart()
        {
            unitsSet = lblUnitsSet.Text = mf.nozz.volumePerMinuteSet.ToString();
            unitsActual = lblUnitsActual.Text = mf.nozz.volumePerMinuteActual.ToString();
            //chart data
            Series s = unoChart.Series["S"];
            Series w = unoChart.Series["PWM"];

            double nextX = 1;
            double nextX5 = 1;

            if (s.Points.Count > 0) nextX = s.Points[s.Points.Count - 1].XValue + 1;
            if (w.Points.Count > 0) nextX5 = w.Points[w.Points.Count - 1].XValue + 1;

            unoChart.Series["S"].Points.AddXY(nextX, unitsSet);
            unoChart.Series["PWM"].Points.AddXY(nextX5, unitsActual);

            while (s.Points.Count > 100)
            {
                s.Points.RemoveAt(0);
            }
            while (w.Points.Count > 100)
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