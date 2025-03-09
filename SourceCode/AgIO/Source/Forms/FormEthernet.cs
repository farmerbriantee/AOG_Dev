using System;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormEthernet : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormEthernet(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();

            nudFirstIP.Controls[0].Enabled = false;
            nudSecndIP.Controls[0].Enabled = false;
            nudThirdIP.Controls[0].Enabled = false;
        }

        private void FormUDp_Load(object sender, EventArgs e)
        {
            cboxIsUDPOn.Checked = Settings.User.setUDP_isOn;
            cboxIsUDPOn.Text = cboxIsUDPOn.Checked ? "UDP On" : "UDP Off";

            //cboxIsSendNMEAToUDP.Checked = Settings.User.setUDP_isSendNMEAToUDP;

            //nudSub1.Value = Settings.User.etIP_SubnetOne;
            //nudSub2.Value = Settings.User.etIP_SubnetTwo;
            //nudSub3.Value = Settings.User.etIP_SubnetThree;

            nudFirstIP.Value = Settings.User.eth_loopOne;
            nudSecndIP.Value = Settings.User.eth_loopTwo;
            nudThirdIP.Value = Settings.User.eth_loopThree;
            nudFourthIP.Value = Settings.User.eth_loopFour;

            if (!cboxIsUDPOn.Checked) cboxIsUDPOn.BackColor = System.Drawing.Color.Salmon;
        }

        private void nudFirstIP_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Settings.User.etIP_SubnetOne = (byte)nudSub1.Value;
            //Settings.User.etIP_SubnetTwo = (byte)nudSub2.Value;
            //Settings.User.etIP_SubnetThree = (byte)nudSub3.Value;

            Settings.User.eth_loopOne = (byte)nudFirstIP.Value;
            Settings.User.eth_loopTwo = (byte)nudSecndIP.Value;
            Settings.User.eth_loopThree = (byte)nudThirdIP.Value;
            Settings.User.eth_loopFour = (byte)nudFourthIP.Value;

            Settings.User.setUDP_isOn = cboxIsUDPOn.Checked;
            //Settings.User.setUDP_isSendNMEAToUDP = cboxIsSendNMEAToUDP.Checked;

            mf.YesMessageBox("AgIO will Restart to Enable UDP Networking Features");
            Log.EventWriter("Program Reset: Start Ethernet Selected");

            Settings.User.Save();
            Program.Restart(); Close();
        }

        private void cboxIsUDPOn_Click(object sender, EventArgs e)
        {
            cboxIsUDPOn.Text = cboxIsUDPOn.Checked ? "UDP Is On" : "UDP Is Off";
            Log.EventWriter("UDP Turned on, Etherent Form");
        }
    }
}