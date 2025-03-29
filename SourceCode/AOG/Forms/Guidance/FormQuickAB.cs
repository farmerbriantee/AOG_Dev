using AOG.Classes;

using System;
using System.Globalization;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormQuickAB : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;

        private double aveLineHeading;

        private bool isRefRightSide = true; //left side 0 middle 1 right 2

        public FormQuickAB(Form _mf)
        {
            mf = _mf as FormGPS;
            InitializeComponent();

            //btnPausePlay.Text = gStr.Get(gs.gsPause;
            this.Text = "Tracks";
        }

        private void FormQuickAB_Load(object sender, EventArgs e)
        {
            panelCurve.Top = 3; panelCurve.Left = 3;
            panelName.Top = 3; panelName.Left = 3;
            panelChoose.Top = 3; panelChoose.Left = 3;
            panelABLine.Top = 3; panelABLine.Left = 3;
            panelAPlus.Top = 3; panelAPlus.Left = 3;

            panelChoose.Visible = true;
            panelCurve.Visible = false;
            panelName.Visible = false;
            panelABLine.Visible = false;
            panelAPlus.Visible = false;

            this.Size = new System.Drawing.Size(270, 360);

            Location = Settings.User.setWindow_QuickABLocation;

            nudHeading.Value = 0;

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormQuickAB_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.User.setWindow_QuickABLocation = Location;
            mf.PanelUpdateRightAndBottom();
        }

        #region Pick

        private void btnzABCurve_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelCurve.Visible = true;

            btnACurve.Enabled = true;
            btnBCurve.Enabled = false;
            btnPausePlay.Enabled = false;
            mf.trk.designPtsList?.Clear();
            mf.Activate();
        }

        private void btnzAPlus_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelAPlus.Visible = true;

            btnAPlus.Enabled = true;
            mf.trk.designPtsList?.Clear();
            nudHeading.Enabled = false;
            mf.Activate();
        }

        private void btnzABLine_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelABLine.Visible = true;

            btnALine.Enabled = true;
            btnBLine.Enabled = false;
            btnPausePlay.Enabled = false;
            mf.trk.designPtsList?.Clear();
            mf.Activate();
        }

        #endregion Pick

        #region Curve

        private void btnRefSideCurve_Click(object sender, EventArgs e)
        {
            isRefRightSide = !isRefRightSide;
            btnRefSideCurve.Image = isRefRightSide ?
            Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            mf.Activate();
        }

        private void btnACurve_Click(object sender, System.EventArgs e)
        {
            if (mf.trk.isMakingCurveTrack)
            {
                mf.trk.designPtsList.Add(new vec3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, mf.pivotAxlePos.heading));
                btnBCurve.Enabled = mf.trk.designPtsList.Count > 3;
            }
            else
            {
                mf.trk.designPtA.easting = mf.pivotAxlePos.easting;
                mf.trk.designPtA.northing = mf.pivotAxlePos.northing;
                lblCurveExists.Text = gStr.Get(gs.gsDriving);

                btnBCurve.Enabled = true;
                btnACurve.Enabled = false;
                btnACurve.Image = Properties.Resources.PointAdd;

                btnPausePlay.Enabled = true;
                btnPausePlay.Visible = true;

                mf.trk.isMakingCurveTrack = true;
                mf.trk.isRecordingCurveTrack = true;
            }
            mf.Activate();
        }

        private void btnBCurve_Click(object sender, System.EventArgs e)
        {
            aveLineHeading = 0;
            mf.trk.isMakingCurveTrack = false;
            mf.trk.isRecordingCurveTrack = false;
            panelCurve.Visible = false;
            panelName.Visible = true;

            mf.trk.designPtB.easting = mf.pivotAxlePos.easting;
            mf.trk.designPtB.northing = mf.pivotAxlePos.northing;

            int cnt = mf.trk.designPtsList.Count;
            if (cnt > 3)
            {
                //make sure point distance isn't too big
                mf.trk.MakePointMinimumSpacing(ref mf.trk.designPtsList, 1.6);
                mf.trk.designPtsList.CalculateHeadings(false);
                var track = new CTrk(TrackMode.Curve);

                //calculate average heading of line
                double x = 0, y = 0;
                foreach (vec3 pt in mf.trk.designPtsList)
                {
                    x += Math.Cos(pt.heading);
                    y += Math.Sin(pt.heading);
                }
                x /= mf.trk.designPtsList.Count;
                y /= mf.trk.designPtsList.Count;
                aveLineHeading = Math.Atan2(y, x);
                if (aveLineHeading < 0) aveLineHeading += glm.twoPI;

                track.heading = aveLineHeading;

                mf.trk.SmoothAB(ref mf.trk.designPtsList, 4, false);
                mf.trk.designPtsList.CalculateHeadings(false);

                //write out the Curve Points
                foreach (vec3 item in mf.trk.designPtsList)
                {
                    track.curvePts.Add(item);
                }

                textBox1.Text = "Cu " +
                    (Math.Round(glm.toDegrees(aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

                panelCurve.Visible = false;
                panelName.Visible = true;

                double dist = (Settings.Tool.toolWidth - Settings.Tool.overlap) * (isRefRightSide ? 0.5 : -0.5) + Settings.Tool.offset;
                mf.trk.NudgeRefTrack(track, dist);

                track.ptA = new vec2(track.curvePts[0]);
                track.ptB = new vec2(track.curvePts[track.curvePts.Count - 1]);

                //build the tail extensions
                mf.trk.AddFirstLastPoints(ref track.curvePts, 200);

                mf.trk.gArr.Add(track);
            }
            else
            {
                mf.trk.designPtsList?.Clear();
                panelCurve.Visible = false;
                panelName.Visible = false;
                panelChoose.Visible = false;
            }
            mf.Activate();
        }

        private void btnPausePlayCurve_Click(object sender, EventArgs e)
        {
            if (mf.trk.isRecordingCurveTrack)
            {
                mf.trk.isRecordingCurveTrack = false;
                btnPausePlay.Image = Properties.Resources.BoundaryRecord;
                //btnPausePlay.Text = gStr.Get(gs.gsRecord;
                btnACurve.Enabled = true;
            }
            else
            {
                mf.trk.isRecordingCurveTrack = true;
                btnPausePlay.Image = Properties.Resources.boundaryPause;
                //btnPausePlay.Text = gStr.Get(gs.gsPause;
                btnACurve.Enabled = false;
            }

            btnBCurve.Enabled = mf.trk.designPtsList.Count > 3;
            mf.Activate();
        }

        #endregion Curve

        #region AB Line

        private void btnRefSideAB_Click(object sender, EventArgs e)
        {
            isRefRightSide = !isRefRightSide;
            btnRefSideAB.Image = isRefRightSide ?
            Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            mf.Activate();
        }

        private void btnALine_Click(object sender, EventArgs e)
        {
            mf.trk.isMakingABLine = true;
            btnALine.Enabled = false;

            mf.trk.designPtA = new vec2(mf.pivotAxlePos);

            mf.trk.designPtB.easting = mf.trk.designPtA.easting - (Math.Sin(mf.pivotAxlePos.heading) * 1);
            mf.trk.designPtB.northing = mf.trk.designPtA.northing - (Math.Cos(mf.pivotAxlePos.heading) * 1);

            mf.trk.designLineEndA.easting = mf.trk.designPtA.easting - (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.trk.designLineEndA.northing = mf.trk.designPtA.northing - (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            mf.trk.designLineEndB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.trk.designLineEndB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            timer1.Enabled = true;

            btnBLine.Enabled = true;
            btnALine.Enabled = false;

            btnEnter_AB.Enabled = true;
            mf.Activate();
        }

        private void btnBLine_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            mf.trk.designPtB = new vec2(mf.pivotAxlePos);
            btnBLine.BackColor = System.Drawing.Color.Teal;

            mf.trk.designHeading = Math.Atan2(mf.trk.designPtB.easting - mf.trk.designPtA.easting,
               mf.trk.designPtB.northing - mf.trk.designPtA.northing);
            if (mf.trk.designHeading < 0) mf.trk.designHeading += glm.twoPI;

            //make sure line is long enough
            double len = glm.Distance(mf.trk.designPtA, mf.trk.designPtB);
            if (len < 20)
            {
                mf.trk.designPtB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 30);
                mf.trk.designPtB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 30);
            }
            len = glm.Distance(mf.trk.designPtA, mf.trk.designPtB);

            mf.trk.designLineEndA.easting = mf.trk.designPtA.easting - (Math.Sin(mf.trk.designHeading) * 1000);
            mf.trk.designLineEndA.northing = mf.trk.designPtA.northing - (Math.Cos(mf.trk.designHeading) * 1000);

            mf.trk.designLineEndB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 1000);
            mf.trk.designLineEndB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 1000);
            mf.Activate();
        }

        private void btnEnter_AB_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            mf.trk.isMakingABLine = false;

            mf.trk.CreateDesignedABTrack(isRefRightSide);

            textBox1.Text = "AB: " +
                (Math.Round(glm.toDegrees(mf.trk.designHeading), 5)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

            panelABLine.Visible = false;
            panelName.Visible = true;
            mf.Activate();
        }

        #endregion AB Line

        #region A Plus

        private void btnRefSideAPlus_Click(object sender, EventArgs e)
        {
            isRefRightSide = !isRefRightSide;
            btnRefSideAPlus.Image = isRefRightSide ?
            Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            mf.Activate();
        }

        private void btnAPlus_Click(object sender, EventArgs e)
        {
            mf.trk.isMakingABLine = true;

            mf.trk.designPtA = new vec2(mf.pivotAxlePos);

            mf.trk.designPtB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.pivotAxlePos.heading) * 30);
            mf.trk.designPtB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.pivotAxlePos.heading) * 30);

            mf.trk.designLineEndA.easting = mf.trk.designPtA.easting - (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.trk.designLineEndA.northing = mf.trk.designPtA.northing - (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            mf.trk.designLineEndB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.trk.designLineEndB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            mf.trk.designHeading = mf.pivotAxlePos.heading;

            btnEnter_AB.Enabled = true;
            nudHeading.Enabled = true;

            nudHeading.Value = glm.toDegrees(mf.trk.designHeading);
            timer1.Enabled = true;
            mf.Activate();
        }

        private void nudHeading_ValueChanged(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            //original A pt.
            mf.trk.designHeading = glm.toRadians((double)nudHeading.Value);

            //start end of line
            mf.trk.designPtB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 30);
            mf.trk.designPtB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 30);

            mf.trk.designLineEndA.easting = mf.trk.designPtA.easting - (Math.Sin(mf.trk.designHeading) * 1000);
            mf.trk.designLineEndA.northing = mf.trk.designPtA.northing - (Math.Cos(mf.trk.designHeading) * 1000);

            mf.trk.designLineEndB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 1000);
            mf.trk.designLineEndB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 1000);

            mf.Activate();
        }

        private void btnEnter_APlus_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            mf.trk.isMakingABLine = false;

            mf.trk.CreateDesignedABTrack(isRefRightSide);

            textBox1.Text = "A+" +
                (Math.Round(glm.toDegrees(mf.trk.designHeading), 5)).ToString(CultureInfo.InvariantCulture)
                + "\u00B0 ";

            panelAPlus.Visible = false;
            panelName.Visible = true;
            mf.Activate();
        }

        #endregion A Plus

        private void timer1_Tick(object sender, EventArgs e)
        {
            mf.trk.designPtB = new vec2(mf.pivotAxlePos);

            mf.trk.designHeading = Math.Atan2(mf.trk.designPtB.easting - mf.trk.designPtA.easting,
               mf.trk.designPtB.northing - mf.trk.designPtA.northing);
            if (mf.trk.designHeading < 0) mf.trk.designHeading += glm.twoPI;

            mf.trk.designLineEndA.easting = mf.trk.designPtA.easting - (Math.Sin(mf.trk.designHeading) * 1000);
            mf.trk.designLineEndA.northing = mf.trk.designPtA.northing - (Math.Cos(mf.trk.designHeading) * 1000);

            mf.trk.designLineEndB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 1000);
            mf.trk.designLineEndB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 1000);
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            textBox1.Text += DateTime.Now.ToString(" hh:mm:ss", CultureInfo.InvariantCulture);
        }

        private void btnCancelCurve_Click(object sender, EventArgs e)
        {
            mf.trk.designPtsList?.Clear();

            mf.trk.isMakingABLine = false;
            mf.trk.isMakingCurveTrack = false;
            mf.trk.isRecordingCurveTrack = false;

            Close();
            mf.Activate();
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            if (Settings.User.setDisplay_isKeyboardOn)
                mf.KeyboardToText((TextBox)sender, this);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0) textBox1.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            int idx = mf.trk.gArr.Count - 1;
            if (idx >= 0)
            {
                mf.trk.currTrk = mf.trk.gArr[idx];//fix this!!

                mf.trk.currTrk.name = textBox1.Text.Trim();
            }
            panelName.Visible = false;

            mf.FileSaveTracks();

            mf.trk.isMakingABLine = false;
            mf.trk.designPtsList?.Clear();

            Close();
        }
    }
}