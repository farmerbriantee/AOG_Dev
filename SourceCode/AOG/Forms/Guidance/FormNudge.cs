using System;
using System.Drawing;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormNudge : Form
    {
        private readonly FormGPS mf = null;

        private double snapAdj = 0;
        private CTrk track;

        public FormNudge(Form callingForm, CTrk _track)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            track = _track;
            this.Text = "";
        }

        private void FormEditTrack_Load(object sender, EventArgs e)
        {
            nudSnapDistance.DecimalPlaces = Settings.User.isMetric ? 0 : 1;
            nudSnapDistance.Value = Settings.Vehicle.setAS_snapDistance;

            snapAdj = Settings.Vehicle.setAS_snapDistance;

            Location = Settings.User.setWindow_formNudgeLocation;
            Size = Settings.User.setWindow_formNudgeSize;
            UpdateMoveLabel();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormEditTrack_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.User.setWindow_formNudgeLocation = Location;
            Settings.User.setWindow_formNudgeSize = Size;

            //save entire list
            mf.FileSaveTracks();
        }

        private void UpdateMoveLabel()
        {
            if (track.nudgeDistance == 0)
                lblOffset.Text = ((int)(track.nudgeDistance * glm.m2InchOrCm * -1)).ToString() + glm.unitsInCm;
            else if (track.nudgeDistance < 0)
                lblOffset.Text = "< " + ((int)(track.nudgeDistance * glm.m2InchOrCm * -1)).ToString() + glm.unitsInCm;
            else
                lblOffset.Text = ((int)(track.nudgeDistance * glm.m2InchOrCm)).ToString() + " >" + glm.unitsInCm;

            mf.Activate();
        }

        private void btnZeroMove_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeDistanceReset(track);
            UpdateMoveLabel();
        }

        private void nudSnapDistance_ValueChanged(object sender, EventArgs e)
        {
            Settings.Vehicle.setAS_snapDistance = snapAdj = nudSnapDistance.Value;

            mf.Activate();
        }

        private void btnAdjRight_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeTrack(track, snapAdj);
            UpdateMoveLabel();
        }

        private void btnAdjLeft_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeTrack(track, -snapAdj);
            UpdateMoveLabel();
        }

        private void btnSnapToPivot_Click(object sender, EventArgs e)
        {
            mf.trk.SnapToPivot(track);
            UpdateMoveLabel();
        }

        private void bntOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnHalfToolRight_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeTrack(track, (Settings.Tool.toolWidth - Settings.Tool.overlap) * 0.5);
            UpdateMoveLabel();
        }

        private void btnHalfToolLeft_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeTrack(track, (Settings.Tool.toolWidth - Settings.Tool.overlap) * -0.5);
            UpdateMoveLabel();
        }

        protected override void WndProc(ref Message m)
        {
            const int RESIZE_HANDLE_SIZE = 10;

            switch (m.Msg)
            {
                case 0x0084/*NCHITTEST*/ :
                    base.WndProc(ref m);

                    if ((int)m.Result == 0x01/*HTCLIENT*/)
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32());
                        Point clientPoint = this.PointToClient(screenPoint);
                        if (clientPoint.Y <= RESIZE_HANDLE_SIZE)
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)13/*HTTOPLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)12/*HTTOP*/ ;
                            else
                                m.Result = (IntPtr)14/*HTTOPRIGHT*/ ;
                        }
                        else if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE))
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)10/*HTLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)2/*HTCAPTION*/ ;
                            else
                                m.Result = (IntPtr)11/*HTRIGHT*/ ;
                        }
                        else
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)16/*HTBOTTOMLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)15/*HTBOTTOM*/ ;
                            else
                                m.Result = (IntPtr)17/*HTBOTTOMRIGHT*/ ;
                        }
                    }
                    return;
            }
            base.WndProc(ref m);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                //drop shadow
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x20000; // <--- use 0x20000
                return cp;
            }
        }
    }
}