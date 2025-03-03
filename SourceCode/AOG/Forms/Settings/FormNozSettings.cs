﻿//Please, if you use this, share the improvements

using System;
using System.Windows.Forms;

namespace AgOpenGPS
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
            nudSprayRateSet1.Value = Settings.Tool.setNozzleSettings.volumePerAreaSet1;
            nudSprayRateSet2.Value = Settings.Tool.setNozzleSettings.volumePerAreaSet2;
            nudSprayMinPressure.Value = Settings.Tool.setNozzleSettings.pressureMin;

            if (mf.isMetric)
            {
                lblRateSet1.Text = mf.nozz.unitsPerArea;
                lblRateSet2.Text = mf.nozz.unitsPerArea;
                nudSprayRateSet1.Maximum = 999;
                nudSprayRateSet1.Minimum = 5;
                nudSprayRateSet1.DecimalPlaces = 0;
                nudSprayRateSet2.Maximum = 999;
                nudSprayRateSet2.Minimum = 5;
                nudSprayRateSet2.DecimalPlaces = 0;

                lblVolumeTank.Text = mf.nozz.volumeTankStart.ToString();
                lblVolumeApplied.Text = mf.nozz.volumeApplied.ToString();
                lblRateSet.Text = mf.nozz.unitsApplied + " Applied";
                lblStatArea.Text = "Ha";
            }
            else
            {
                lblRateSet1.Text = mf.nozz.unitsPerArea;
                lblRateSet2.Text = mf.nozz.unitsPerArea;
                nudSprayRateSet1.Maximum = 99.9;
                nudSprayRateSet1.Minimum = 1;
                nudSprayRateSet1.DecimalPlaces = 1;
                nudSprayRateSet2.Maximum = 99.9;
                nudSprayRateSet2.Minimum = 1;
                nudSprayRateSet2.DecimalPlaces = 1;
                lblRateSet.Text = mf.nozz.unitsApplied + "Applied";
                lblStatArea.Text = "Acre";
            }

            nudTankVolume.Value = Settings.Tool.setNozzleSettings.volumeTankStart;
            nudZeroVolume.Value = mf.nozz.volumeApplied;

            lblVolumeTank.Text = mf.nozz.volumeTankStart.ToString();
            lblVolumeApplied.Text = mf.nozz.volumeApplied.ToString("N1");
            lblTankRemain.Text = (mf.nozz.volumeTankStart - mf.nozz.volumeApplied).ToString("N1");
            lblAcresAvailable.Text = ((mf.nozz.volumeTankStart - mf.nozz.volumeApplied) / mf.nozz.volumePerAreaSetSelected).ToString("N1");

            nudNudge.Value = Settings.Tool.setNozzleSettings.rateNudge;
            nudRateAlarmPercent.Value = mf.nozz.rateAlarmPercent * 100;
        }

        private void nudSprayRateSet1_ValueChanged(object sender, EventArgs e)
        {
            mf.nozz.volumePerAreaSet1 = nudSprayRateSet1.Value;
            Settings.Tool.setNozzleSettings.volumePerAreaSet1 = nudSprayRateSet1.Value;
        }

        private void nudSprayRateSet2_ValueChanged(object sender, EventArgs e)
        {
            mf.nozz.volumePerAreaSet2 = nudSprayRateSet2.Value;
            Settings.Tool.setNozzleSettings.volumePerAreaSet2 = nudSprayRateSet2.Value;
        }

        private void nudSprayMinPressure_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozzleSettings.pressureMin = (int)nudSprayMinPressure.Value;

            PGN_226.pgn[PGN_226.minPressure] = unchecked((byte)(Settings.Tool.setNozzleSettings.pressureMin));

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
            mf.nozz.volumeTankStart = (int)nudTankVolume.Value;
            Settings.Tool.setNozzleSettings.volumeTankStart = mf.nozz.volumeTankStart;
        }

        private void nudZeroVolume_ValueChanged(object sender, EventArgs e)
        {
            if (nudZeroVolume.Value < 2)
            {
                mf.nozz.volumeApplied = 0;

                PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 1;
                PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;

                mf.SendPgnToLoop(PGN_225.pgn);

                PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 0;
                PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;
            }
            else
            {
                mf.nozz.volumeApplied = (double)nudZeroVolume.Value;

                int vol = (int)nudZeroVolume.Value;

                PGN_226.pgn[PGN_226.flowCalHi] = unchecked((byte)(vol >> 8));
                PGN_226.pgn[PGN_226.flowCaLo] = unchecked((byte)(vol));

                PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 1;
                PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;

                mf.SendPgnToLoop(PGN_225.pgn);

                PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 0;
                PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;
            }
        }

        private void btnZeroVolume_Click(object sender, EventArgs e)
        {
            mf.nozz.volumeApplied = 0;
            nudZeroVolume.Value = 0;

            PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 1;
            PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;

            mf.SendPgnToLoop(PGN_225.pgn);

            PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 0;
            PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblVolumeTank.Text = mf.nozz.volumeTankStart.ToString();
            lblVolumeApplied.Text = mf.nozz.volumeApplied.ToString("N1");
            lblTankRemain.Text = (mf.nozz.volumeTankStart - mf.nozz.volumeApplied).ToString("N1");
            lblAcresAvailable.Text = ((mf.nozz.volumeTankStart - mf.nozz.volumeApplied) / mf.nozz.volumePerAreaSetSelected).ToString("N1");
        }

        private void nudNudge_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.setNozzleSettings.rateNudge = nudNudge.Value;
            mf.nozz.rateNudge = nudNudge.Value;
        }

        private void nudRateAlarmPercent_ValueChanged(object sender, EventArgs e)
        {
            mf.nozz.rateAlarmPercent = nudRateAlarmPercent.Value * 0.01;
            Settings.Tool.setNozzleSettings.rateAlarmPercent = mf.nozz.rateAlarmPercent;
        }
    }
}