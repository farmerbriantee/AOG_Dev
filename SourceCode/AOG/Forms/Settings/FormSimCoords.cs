using AOG.Classes;

using System;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormSimCoords : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormSimCoords(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.Text = gStr.Get(gs.gsEnterCoordinatesForSimulator);
        }

        private void FormSimCoords_Load(object sender, EventArgs e)
        {
            nudLatitude.Value = Settings.Vehicle.setGPS_SimLatitude;
            nudLongitude.Value = Settings.Vehicle.setGPS_SimLongitude;
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            if (mf.isFieldStarted)
            {
                mf.TimedMessageBox(2000, gStr.Get(gs.gsFieldIsOpen), gStr.Get(gs.gsCloseFieldFirst));
                Close();
            }

            if (!mf.timerSim.Enabled)
            {
                mf.TimedMessageBox(2000, "Simulator is off", "Go Back To Work, No Time For Games");
                Close();
            }

            mf.pn.SetLocalMetersPerDegree(nudLatitude.Value, nudLongitude.Value);

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}