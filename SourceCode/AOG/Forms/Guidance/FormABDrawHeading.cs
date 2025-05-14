using System.Windows.Forms;

namespace AOG
{
    public partial class FormABDrawHeading : Form
    {
        private readonly FormABDraw mf = null;

        public FormABDrawHeading(Form callingForm, double heading)
        {
            mf = callingForm as FormABDraw;

            InitializeComponent();

            nudHeading.Value = glm.toDegrees(heading);
        }

        private void nudHeading_ValueChanged(object sender, System.EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            mf.remoteHeading = -1;
            Close();
        }

        private void btnSerialOK_Click(object sender, System.EventArgs e)
        {
            mf.remoteHeading = (double)glm.toRadians(nudHeading.Value);
            Close();
        }

        private void btn0_Click(object sender, System.EventArgs e)
        {
            nudHeading.Value = 0;
        }

        private void btn90_Click(object sender, System.EventArgs e)
        {
            nudHeading.Value = 90;
        }

        private void btn180_Click(object sender, System.EventArgs e)
        {
            nudHeading.Value = 180;
        }

        private void btn270_Click(object sender, System.EventArgs e)
        {
            nudHeading.Value = 270;
        }
    }
}