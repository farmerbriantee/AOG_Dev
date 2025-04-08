using System;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormRefNudge : Form
    {
        private readonly FormGPS mf = null;
        public CTrk gBack;
        public CTrk track;

        private double distanceMoved = 0;

        public FormRefNudge(Form callingForm, CTrk _track)
        {
            mf = callingForm as FormGPS;
            track = _track;
            gBack = new CTrk(_track);

            InitializeComponent();

            this.Text = "Ref Adjust";
        }

        private void FormEditTrack_Load(object sender, EventArgs e)
        {
            mf.panelRight.Enabled = false;
            nudSnapDistance.DecimalPlaces = Settings.User.isMetric ? 0 : 1;
            nudSnapDistance.Value = Settings.Vehicle.setAS_snapDistanceRef;

            lblOffset.Text = ((int)(distanceMoved * glm.m2InchOrCm)).ToString("N1") + " " + glm.unitsInCm;

            //Location = Properties.Settings.Default.setWindow_formNudgeLocation;
            //Size = Properties.Settings.Default.setWindow_formNudgeSize;

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormEditTrack_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.panelRight.Enabled = true;
        }

        private void nudSnapDistance_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setAS_snapDistanceRef = nudSnapDistance.Value;

            mf.Activate();
        }

        private void btnAdjRight_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeRefTrack(track, nudSnapDistance.Value);
            distanceMoved += nudSnapDistance.Value;
            DistanceMovedLabel();
            mf.Activate();
        }

        private void btnAdjLeft_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeRefTrack(track, -nudSnapDistance.Value);
            distanceMoved += -nudSnapDistance.Value;
            DistanceMovedLabel();
            mf.Activate();
        }

        private void btnHalfToolRight_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeRefTrack(track, (Settings.Tool.toolWidth - Settings.Tool.overlap) * 0.5);
            distanceMoved += (Settings.Tool.toolWidth - Settings.Tool.overlap) * 0.5;
            DistanceMovedLabel();
            mf.Activate();
        }

        private void btnHalfToolLeft_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeRefTrack(track, (Settings.Tool.toolWidth - Settings.Tool.overlap) * -0.5);
            distanceMoved += (Settings.Tool.toolWidth - Settings.Tool.overlap) * -0.5;
            DistanceMovedLabel();
            mf.Activate();
        }

        private void DistanceMovedLabel()
        {
            lblOffset.Text = ((int)(distanceMoved * glm.m2InchOrCm)).ToString("N1") + " " + glm.unitsInCm;
            mf.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (distanceMoved != 0)
            {
                //save entire list
                mf.FileSaveTracks();
            }
            Close();
        }

        private void btnCancelMain_Click(object sender, EventArgs e)
        {
            mf.trk.setTrack(gBack);
            //mf.FileSaveTracks();
            Close();
        }
    }
}