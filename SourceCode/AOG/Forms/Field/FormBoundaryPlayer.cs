using AOG.Classes;

using System;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormBoundaryPlayer : Form
    {
        //properties
        private readonly FormGPS mf = null;

        private bool isClosing;

        //constructor
        public FormBoundaryPlayer(Form callingForm)
        {
            mf = callingForm as FormGPS;

            InitializeComponent();

            label1.Text = gStr.Get(gs.gsArea) + ":";
            this.Text = gStr.Get(gs.gsStopRecordPauseBoundary);
        }

        private void FormBoundaryPlayer_Load(object sender, EventArgs e)
        {
            nudOffset.Value = Settings.Tool.toolWidth * 0.5;

            if (Settings.User.isMetric)
            {
                lblMetersInches.Text = "cm";
            }
            else
            {
                double ftInches = nudOffset.Value;
                lblMetersInches.Text = ((int)(ftInches / 12)).ToString() + "' " + ((int)(ftInches % 12)).ToString() + '"';
            }

            btnPausePlay.Image = Properties.Resources.BoundaryRecord;

            mf.bnd.isDrawAtPivot = Settings.Vehicle.setBnd_isDrawPivot;

            btnLeftRight.Image = mf.bnd.isDrawRightSide ? Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            btnAntennaTool.Image = mf.bnd.isDrawAtPivot ? Properties.Resources.BoundaryRecordPivot : Properties.Resources.BoundaryRecordTool;

            mf.bnd.createFenceOffset = (Settings.Tool.toolWidth * 0.5);
            mf.bnd.isFenceBeingMade = true;
            mf.Focus();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormBoundaryPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }
        }

        private void nudOffset_ValueChanged(object sender, EventArgs e)
        {
            btnPausePlay.Focus();
            mf.bnd.createFenceOffset = nudOffset.Value;

            if (!Settings.User.isMetric)
            {
                double ftInches = (double)nudOffset.Value;
                lblMetersInches.Text = ((int)(ftInches / 12)).ToString() + "' " + (ftInches % 12).ToString("N1") + '"';
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int ptCount = mf.bnd.fenceBeingMadePts.Count;
            double area = 0;

            if (ptCount > 0)
            {
                int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

                for (int i = 0; i < ptCount; j = i++)
                {
                    area += (mf.bnd.fenceBeingMadePts[j].easting + mf.bnd.fenceBeingMadePts[i].easting) * (mf.bnd.fenceBeingMadePts[j].northing - mf.bnd.fenceBeingMadePts[i].northing);
                }
                area = Math.Abs(area / 2);
            }

            lblArea.Text = Math.Round(area * glm.m22HaOrAc, 2).ToString();
            lblPoints.Text = mf.bnd.fenceBeingMadePts.Count.ToString();
        }

        private void btnStoPGN_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show("Done?", gStr.Get(gs.gsBoundary),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result3 == DialogResult.Yes)
            {
                if (mf.bnd.fenceBeingMadePts.Count > 2)
                {
                    CBoundaryList newBnd = new CBoundaryList();

                    for (int i = 0; i < mf.bnd.fenceBeingMadePts.Count; i++)
                    {
                        newBnd.fenceLine.Add(new vec3(mf.bnd.fenceBeingMadePts[i]));
                    }

                    mf.bnd.AddToBoundList(newBnd, mf.bnd.bndList.Count);

                    mf.FileSaveBoundary();

                    Log.EventWriter("Driven Boundary Created, Area: " + lblArea.Text);
                }

                //stop it all for adding
                mf.bnd.isOkToAddPoints = false;
                mf.bnd.isFenceBeingMade = false;
                mf.bnd.fenceBeingMadePts.Clear();

                //close window
                isClosing = true;
                Close();
            }
        }

        //actually the record button
        private void btnPausePlay_Click(object sender, EventArgs e)
        {
            if (mf.bnd.isOkToAddPoints)
            {
                mf.bnd.isOkToAddPoints = false;
                btnPausePlay.Image = Properties.Resources.BoundaryRecord;
                btnAddPoint.Enabled = true;
                btnDeleteLast.Enabled = true;
            }
            else
            {
                mf.bnd.isOkToAddPoints = true;
                btnPausePlay.Image = Properties.Resources.boundaryPause;
                btnAddPoint.Enabled = false;
                btnDeleteLast.Enabled = false;
            }
        }

        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            mf.bnd.isOkToAddPoints = true;
            mf.AddBoundaryPoint();
            mf.bnd.isOkToAddPoints = false;
            lblPoints.Text = mf.bnd.fenceBeingMadePts.Count.ToString();
        }

        private void btnDeleteLast_Click(object sender, EventArgs e)
        {
            int ptCount = mf.bnd.fenceBeingMadePts.Count;
            if (ptCount > 0)
                mf.bnd.fenceBeingMadePts.RemoveAt(ptCount - 1);
            lblPoints.Text = mf.bnd.fenceBeingMadePts.Count.ToString();
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show(gStr.Get(gs.gsCompletelyDeleteBoundary),
                                    gStr.Get(gs.gsDeleteForSure),
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button2);
            if (result3 == DialogResult.Yes)
            {
                mf.bnd.fenceBeingMadePts?.Clear();
            }
            lblPoints.Text = mf.bnd.fenceBeingMadePts.Count.ToString();
        }

        private void btnLeftRight_Click(object sender, EventArgs e)
        {
            mf.bnd.isDrawRightSide = !mf.bnd.isDrawRightSide;
            btnLeftRight.Image = mf.bnd.isDrawRightSide ? Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
        }

        private void btnAntennaTool_Click(object sender, EventArgs e)
        {
            mf.bnd.isDrawAtPivot = !mf.bnd.isDrawAtPivot;
            btnAntennaTool.Image = mf.bnd.isDrawAtPivot ? Properties.Resources.BoundaryRecordPivot : Properties.Resources.BoundaryRecordTool;
            Settings.Vehicle.setBnd_isDrawPivot = mf.bnd.isDrawAtPivot;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.B) //autosteer button on off
            {
                mf.bnd.isOkToAddPoints = true;
                mf.AddBoundaryPoint();
                mf.bnd.isOkToAddPoints = false;
                lblPoints.Text = mf.bnd.fenceBeingMadePts.Count.ToString();
            }

            if (keyData == Keys.D) //autosteer button on off
            {
                int ptCount = mf.bnd.fenceBeingMadePts.Count;
                if (ptCount > 0)
                    mf.bnd.fenceBeingMadePts.RemoveAt(ptCount - 1);
                lblPoints.Text = mf.bnd.fenceBeingMadePts.Count.ToString();
            }

            if (keyData == Keys.R) //autosteer button on off
            {
                if (mf.bnd.isOkToAddPoints)
                {
                    mf.bnd.isOkToAddPoints = false;
                    btnPausePlay.Image = Properties.Resources.BoundaryRecord;
                    //btnPausePlay.Text = gStr.Get(gs.gsRecord;
                    btnAddPoint.Enabled = true;
                    btnDeleteLast.Enabled = true;
                }
                else
                {
                    mf.bnd.isOkToAddPoints = true;
                    btnPausePlay.Image = Properties.Resources.boundaryPause;
                    //btnPausePlay.Text = gStr.Get(gs.gsPause;
                    btnAddPoint.Enabled = false;
                    btnDeleteLast.Enabled = false;
                }
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void cboxIsRecBoundaryWhenSectionOn_Click(object sender, EventArgs e)
        {
            mf.bnd.isRecFenceWhenSectionOn = cboxIsRecBoundaryWhenSectionOn.Checked;
        }
    }
}