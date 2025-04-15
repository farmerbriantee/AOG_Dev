//Please, if you use this, share the improvements

using System;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormNozSettings : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        //Nozzz constructor
        public FormNozSettings(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            this.Text = "Sprayer Settings";
        }

        private void FormDisplaySettings_Load(object sender, EventArgs e)
        {
            nudSprayRateSet1.Value = Settings.Tool.setNozz.volumePerAreaSet1;
            nudSprayRateSet2.Value = Settings.Tool.setNozz.volumePerAreaSet2;
            nudSprayMinPressure.Value = Settings.Tool.setNozz.pressureMin;

            if (Settings.User.isMetric)
            {
                lblRateSet1.Text = Settings.Tool.setNozz.unitsPerArea;
                lblRateSet2.Text = Settings.Tool.setNozz.unitsPerArea;
                nudSprayRateSet1.Maximum = 999;
                nudSprayRateSet1.Minimum = 5;
                nudSprayRateSet1.DecimalPlaces = 0;
                nudSprayRateSet2.Maximum = 999;
                nudSprayRateSet2.Minimum = 5;
                nudSprayRateSet2.DecimalPlaces = 0;

                lblVolumeTank.Text = Settings.Tool.setNozz.volumeTankStart.ToString();
                lblVolumeApplied.Text = Settings.Tool.setNozz.volumeApplied.ToString();
                lblRateSet.Text = Settings.Tool.setNozz.unitsApplied + " Applied";
                lblStatArea.Text = "Ha";
            }
            else
            {
                lblRateSet1.Text = Settings.Tool.setNozz.unitsPerArea;
                lblRateSet2.Text = Settings.Tool.setNozz.unitsPerArea;
                nudSprayRateSet1.Maximum = 99.9;
                nudSprayRateSet1.Minimum = 1;
                nudSprayRateSet1.DecimalPlaces = 1;
                nudSprayRateSet2.Maximum = 99.9;
                nudSprayRateSet2.Minimum = 1;
                nudSprayRateSet2.DecimalPlaces = 1;
                lblRateSet.Text = Settings.Tool.setNozz.unitsApplied + "Applied";
                lblStatArea.Text = "Acre";
            }

            nudTankVolume.Value = Settings.Tool.setNozz.volumeTankStart;
            nudZeroVolume.Value = Settings.Tool.setNozz.volumeApplied;

            lblVolumeTank.Text = Settings.Tool.setNozz.volumeTankStart.ToString();
            lblVolumeApplied.Text = Settings.Tool.setNozz.volumeApplied.ToString("N1");
            lblTankRemain.Text = (Settings.Tool.setNozz.volumeTankStart - Settings.Tool.setNozz.volumeApplied).ToString("N1");
            lblAcresAvailable.Text = ((Settings.Tool.setNozz.volumeTankStart - Settings.Tool.setNozz.volumeApplied) / mf.nozz.volumePerAreaSetSelected).ToString("N1");

            nudNudge.Value = Settings.Tool.setNozz.rateNudge;
            nudRateAlarmPercent.Value = Settings.Tool.setNozz.rateAlarmPercent * 100;
        }

        private void nudSprayRateSet1_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.volumePerAreaSet1 = nudSprayRateSet1.Value;
        }

        private void nudSprayRateSet2_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.volumePerAreaSet2 = nudSprayRateSet2.Value;
        }

        private void nudSprayMinPressure_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.pressureMin = (int)nudSprayMinPressure.Value;

            PGN_226.pgn[PGN_226.minPressure] = unchecked((byte)(Settings.Tool.setNozz.pressureMin));

            mf.SendPgnToLoop(PGN_226.pgn);
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 0;
            PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;

            Close();
        }

        private void nudTankVolume_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.volumeTankStart = (int)nudTankVolume.Value;
        }

        private void nudZeroVolume_ValueChanged(object sender, EventArgs e)
        {
            if (nudZeroVolume.Value < 2)
            {
                Settings.Tool.setNozz.volumeApplied = 0;

                PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 1;
                PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;

                mf.SendPgnToLoop(PGN_225.pgn);

                PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 0;
                PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;
            }
            else
            {
                Settings.Tool.setNozz.volumeApplied = (double)nudZeroVolume.Value;

                int vol = (int)nudZeroVolume.Value;

                PGN_225.pgn[PGN_225.zeroTankVolumeHi] = unchecked((byte)(vol >> 8));
                PGN_225.pgn[PGN_225.zeroTankVolumeLo] = unchecked((byte)(vol));

                mf.SendPgnToLoop(PGN_225.pgn);

                PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 0;
                PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;
            }
        }

        private void btnZeroVolume_Click(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.volumeApplied = 0;
            nudZeroVolume.Value = 0;

            PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 1;
            PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;

            mf.SendPgnToLoop(PGN_225.pgn);

            PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 0;
            PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblVolumeTank.Text = Settings.Tool.setNozz.volumeTankStart.ToString();
            lblVolumeApplied.Text = Settings.Tool.setNozz.volumeApplied.ToString("N1");
            lblTankRemain.Text = (Settings.Tool.setNozz.volumeTankStart - Settings.Tool.setNozz.volumeApplied).ToString("N1");
            lblAcresAvailable.Text = ((Settings.Tool.setNozz.volumeTankStart - Settings.Tool.setNozz.volumeApplied) / mf.nozz.volumePerAreaSetSelected).ToString("N1");
        }

        private void nudNudge_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.rateNudge = nudNudge.Value;
        }

        private void nudRateAlarmPercent_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.rateAlarmPercent = nudRateAlarmPercent.Value * 0.01;
        }
    }
}