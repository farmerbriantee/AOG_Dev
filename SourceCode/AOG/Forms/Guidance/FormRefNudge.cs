using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormRefNudge : Form
    {
        private readonly FormGPS mf = null;
        public List<CTrk> gTemp = new List<CTrk>();

        private double distanceMoved = 0;

        public FormRefNudge(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            this.Text = "Ref Adjust";
        }

        private void FormEditTrack_Load(object sender, EventArgs e)
        {
            mf.panelRight.Enabled = false;
            nudSnapDistance.DecimalPlaces = mf.isMetric ? 0 : 1;
            nudSnapDistance.Value = Settings.Vehicle.setAS_snapDistanceRef;


            foreach (var item in mf.trk.gArr)
            {
                gTemp.Add(new CTrk(item));
            }

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
            mf.trk.NudgeRefTrack(nudSnapDistance.Value);
            distanceMoved += nudSnapDistance.Value;
            DistanceMovedLabel();
            mf.Activate();
        }

        private void btnAdjLeft_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeRefTrack(-nudSnapDistance.Value);
            distanceMoved += -nudSnapDistance.Value;
            DistanceMovedLabel();
            mf.Activate();
        }

        private void btnHalfToolRight_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeRefTrack((mf.tool.width - mf.tool.overlap) * 0.5);
            distanceMoved += (mf.tool.width - mf.tool.overlap) * 0.5;
            DistanceMovedLabel();
            mf.Activate();
        }

        private void btnHalfToolLeft_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeRefTrack((mf.tool.width - mf.tool.overlap) * -0.5);
            distanceMoved += (mf.tool.width - mf.tool.overlap) * -0.5;
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
            //if (mf.trk.gArr.Count > 0)
            {
                //save entire list
                mf.FileSaveTracks();
            }
            Close();
        }

        private void btnCancelMain_Click(object sender, EventArgs e)
        {
            mf.trk.gArr.Clear();

            foreach (var item in gTemp)
            {
                mf.trk.gArr.Add(new CTrk(item));
            }

            mf.trk.isTrackValid = false;

            //mf.FileSaveTracks();
            Close();
        }
    }
}