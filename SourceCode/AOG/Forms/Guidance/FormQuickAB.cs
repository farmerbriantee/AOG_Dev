using AgOpenGPS.Culture;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormQuickAB : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;

        private double aveLineHeading;
        public List<CTrk> gTemp = new List<CTrk>();

        private bool isRefRightSide = true; //left side 0 middle 1 right 2

        //used throughout to acces the master Track list
        private int idx;

        public FormQuickAB(Form _mf)
        {
            mf = _mf as FormGPS;
            InitializeComponent();

            //btnPausePlay.Text = gStr.gsPause;
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

            Location = Properties.Settings.Default.setWindow_QuickABLocation;

            nudHeading.Controls[0].Enabled = false;
            nudHeading.Value = 0;

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormQuickAB_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.setWindow_QuickABLocation = Location;

            mf.twoSecondCounter = 100;

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
                lblCurveExists.Text = gStr.gsDriving;

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
                mf.trk.CalculateHeadings(ref mf.trk.designPtsList);

                mf.trk.gArr.Add(new CTrk());
                //array number is 1 less since it starts at zero
                idx = mf.trk.gArr.Count - 1;

                mf.trk.gArr[idx].mode = TrackMode.Curve;

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

                mf.trk.gArr[idx].heading = aveLineHeading;

                SmoothAB(4);
                mf.trk.CalculateHeadings(ref mf.trk.designPtsList);

                //write out the Curve Points
                foreach (vec3 item in mf.trk.designPtsList)
                {
                    mf.trk.gArr[idx].curvePts.Add(item);
                }

                mf.trk.designName = "Cu " +
                    (Math.Round(glm.toDegrees(aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

                textBox1.Text = mf.trk.designName;

                panelCurve.Visible = false;
                panelName.Visible = true;

                double dist;

                if (isRefRightSide)
                {
                    dist = (mf.tool.width - mf.tool.overlap) * 0.5 + mf.tool.offset;
                    mf.trk.idx = idx;
                    mf.trk.NudgeRefTrack(dist);
                }
                else
                {
                    dist = (mf.tool.width - mf.tool.overlap) * -0.5 + mf.tool.offset;
                    mf.trk.idx = idx;
                    mf.trk.NudgeRefTrack(dist);
                }

                mf.trk.gArr[idx].ptA.easting = (mf.trk.gArr[idx].curvePts[0].easting);
                mf.trk.gArr[idx].ptA.northing = (mf.trk.gArr[idx].curvePts[0].northing);
                mf.trk.gArr[idx].ptB.easting = (mf.trk.gArr[idx].curvePts[mf.trk.gArr[idx].curvePts.Count-1].easting);
                mf.trk.gArr[idx].ptB.northing = (mf.trk.gArr[idx].curvePts[mf.trk.gArr[idx].curvePts.Count - 1].northing);

                //build the tail extensions
                mf.trk.AddFirstLastPoints(ref mf.trk.gArr[idx].curvePts, 100);
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
                //btnPausePlay.Text = gStr.gsRecord;
                btnACurve.Enabled = true;
            }
            else
            {
                mf.trk.isRecordingCurveTrack = true;
                btnPausePlay.Image = Properties.Resources.boundaryPause;
                //btnPausePlay.Text = gStr.gsPause;
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

            mf.trk.designPtA = new vec2(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing);

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
            mf.trk.designPtB = new vec2(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing);
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

            mf.trk.designName = "AB: " +
                (Math.Round(glm.toDegrees(mf.trk.designHeading), 5)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";
            textBox1.Text = mf.trk.designName;

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

            mf.trk.designPtA = new vec2(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing);

            mf.trk.designPtB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.pivotAxlePos.heading) * 30);
            mf.trk.designPtB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.pivotAxlePos.heading) * 30);

            mf.trk.designLineEndA.easting = mf.trk.designPtA.easting - (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.trk.designLineEndA.northing = mf.trk.designPtA.northing - (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            mf.trk.designLineEndB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.trk.designLineEndB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            mf.trk.designHeading = mf.pivotAxlePos.heading;

            btnEnter_AB.Enabled = true;
            nudHeading.Enabled = true;

            nudHeading.Value = (decimal)(glm.toDegrees(mf.trk.designHeading));
            timer1.Enabled = true;
            mf.Activate();
        }

        private void nudHeading_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                //original A pt.
                mf.trk.designHeading = glm.toRadians((double)nudHeading.Value);

                //start end of line
                mf.trk.designPtB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 30);
                mf.trk.designPtB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 30);

                mf.trk.designLineEndA.easting = mf.trk.designPtA.easting - (Math.Sin(mf.trk.designHeading) * 1000);
                mf.trk.designLineEndA.northing = mf.trk.designPtA.northing - (Math.Cos(mf.trk.designHeading) * 1000);

                mf.trk.designLineEndB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 1000);
                mf.trk.designLineEndB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 1000);
            }
            mf.Activate();
        }

        private void btnEnter_APlus_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            mf.trk.isMakingABLine = false;

            mf.trk.CreateDesignedABTrack(isRefRightSide);

            mf.trk.designName = "A+" +
                (Math.Round(glm.toDegrees(mf.trk.designHeading), 5)).ToString(CultureInfo.InvariantCulture) 
                + "\u00B0 ";
            textBox1.Text = mf.trk.designName;

            panelAPlus.Visible = false;
            panelName.Visible = true;
            mf.Activate();
        }

        #endregion A Plus

        private void timer1_Tick(object sender, EventArgs e)
        {
            mf.trk.designPtB = new vec2(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing);

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
            mf.trk.designName = textBox1.Text;
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
            if (mf.isKeyboardOn)
                mf.KeyboardToText((TextBox)sender, this);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0) textBox1.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            int idx = mf.trk.gArr.Count - 1;

            mf.trk.gArr[idx].name = textBox1.Text.Trim();

            panelName.Visible = false;

            mf.trk.designPtsList?.Clear();

            mf.FileSaveTracks();

            if (mf.isBtnAutoSteerOn)
            {
                mf.btnAutoSteer.PerformClick();
                mf.TimedMessageBox(2000, gStr.gsGuidanceStopped, "Return From Editing");
            }
            if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();

            mf.trk.isMakingABLine = false;
            mf.trk.designPtsList?.Clear();
            mf.trk.idx = idx;

            Close();
        }

        public void SmoothAB(int smPts)
        {
            //countExit the reference list of original curve
            int cnt = mf.trk.designPtsList.Count;

            //the temp array
            vec3[] arr = new vec3[cnt];

            //read the points before and after the setpoint
            for (int s = 0; s < smPts / 2; s++)
            {
                arr[s].easting = mf.trk.designPtsList[s].easting;
                arr[s].northing = mf.trk.designPtsList[s].northing;
                arr[s].heading = mf.trk.designPtsList[s].heading;
            }

            for (int s = cnt - (smPts / 2); s < cnt; s++)
            {
                arr[s].easting = mf.trk.designPtsList[s].easting;
                arr[s].northing = mf.trk.designPtsList[s].northing;
                arr[s].heading = mf.trk.designPtsList[s].heading;
            }

            //average them - center weighted average
            for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
            {
                for (int j = -smPts / 2; j < smPts / 2; j++)
                {
                    arr[i].easting += mf.trk.designPtsList[j + i].easting;
                    arr[i].northing += mf.trk.designPtsList[j + i].northing;
                }
                arr[i].easting /= smPts;
                arr[i].northing /= smPts;
                arr[i].heading = mf.trk.designPtsList[i].heading;
            }

            //make a list to draw
            mf.trk.designPtsList?.Clear();
            for (int i = 0; i < cnt; i++)
            {
                mf.trk.designPtsList.Add(arr[i]);
            }
            mf.Activate();
        }
    }
}