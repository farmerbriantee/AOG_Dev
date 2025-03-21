using AOG.Classes;

using System;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormSmoothAB : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private int smoothCount = 20;
        private CTrk track;

        public FormSmoothAB(Form callingForm, CTrk _track)
        {
            track = _track;
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.bntOK.Text = gStr.Get(gs.gsForNow);
            this.btnSave.Text = gStr.Get(gs.gsToFile);

            this.Text = gStr.Get(gs.gsSmoothABCurve);
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            mf.trk.isSmoothWindowOpen = false;
            mf.trk.SaveSmoothList(track);
            Close();
        }

        private void FormSmoothAB_Load(object sender, EventArgs e)
        {
            mf.trk.isSmoothWindowOpen = true;
            smoothCount = 20;
            lblSmooth.Text = "**";

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.trk.isSmoothWindowOpen = false;
            mf.trk.smooList?.Clear();
            Close();
        }

        private void btnNorth_MouseDown(object sender, MouseEventArgs e)
        {
            if (smoothCount++ > 100) smoothCount = 100;
            mf.trk.SmoothAB(ref track.curvePts, smoothCount * 2);
            lblSmooth.Text = smoothCount.ToString();
        }

        private void btnSouth_MouseDown(object sender, MouseEventArgs e)
        {
            smoothCount--;
            if (smoothCount < 2) smoothCount = 2;
            mf.trk.SmoothAB(ref track.curvePts, smoothCount * 2);
            lblSmooth.Text = smoothCount.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            mf.trk.isSmoothWindowOpen = false;
            mf.trk.SaveSmoothList(track);

            //save entire list
            mf.FileSaveTracks();

            //mf.FileSaveCurveLines();
            Close();
        }
    }
}