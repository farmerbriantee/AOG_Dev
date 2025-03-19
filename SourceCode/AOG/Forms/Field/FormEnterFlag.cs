using AOG.Classes;

using System;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormEnterFlag : Form
    {
        private readonly FormGPS mf = null;

        public FormEnterFlag(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            this.Text = gStr.Get(gs.gsEditABLine);

            nudLatitude.Value = mf.pn.latitude;
            nudLongitude.Value = mf.pn.longitude;
        }

        private void FormEnterAB_Load(object sender, EventArgs e)
        {
            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRed_Click(object sender, EventArgs e)
        {
            Button btnRed = (Button)sender;
            byte flagColor = 0;

            if (btnRed.Name == "btnRed")
            {
                flagColor = 0;
            }
            else if (btnRed.Name == "btnGreen")
            {
                flagColor = 1;
            }
            else if (btnRed.Name == "btnYellow")
            {
                flagColor = 2;
            }

            double east, nort;

            mf.pn.ConvertWGS84ToLocal((double)nudLatitude.Value, (double)nudLongitude.Value, out nort, out east);
            int nextflag = mf.flagPts.Count + 1;
            CFlag flagPt = new CFlag((double)nudLatitude.Value, (double)nudLongitude.Value, east, nort, 0, flagColor, nextflag, (nextflag).ToString());
            mf.flagPts.Add(flagPt);
            mf.FileSaveFlags();

            Form fc = Application.OpenForms["FormFlags"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            if (mf.flagPts.Count > 0)
            {
                mf.flagNumberPicked = nextflag;
                Form form = new FormFlags(mf);
                form.Show(mf);
            }

            Close();
        }
    }
}