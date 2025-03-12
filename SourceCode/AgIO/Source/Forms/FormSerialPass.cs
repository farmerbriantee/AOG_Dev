using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormSerialPass : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormSerialPass(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();
        }

        private void FormSerialPass_Load(object sender, EventArgs e)
        {
            cboxSerialPassOn.Checked = Settings.User.setPass_isOn;
            cboxToSerial.Checked = Settings.User.setNTRIP_sendToSerial;
            cboxToUDP.Checked = Settings.User.setNTRIP_sendToUDP;
            nudSendToUDPPort.Value = Settings.User.setNTRIP_sendToUDPPort;

            cboxRadioPort.Items.Clear();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxRadioPort.Items.Add(s);
            }

            lblCurrentPort.Text = Settings.User.setPort_portNameRadio;
            cboxRadioPort.Text = Settings.User.setPort_portNameRadio;
            lblCurrentBaud.Text = Settings.User.setPort_baudRateRadio;
            cboxBaud.Text = Settings.User.setPort_baudRateRadio;

            if (mf.spRadio != null && mf.spRadio.IsOpen)
            {
                btnOpenSerial.Enabled = false;
                btnCloseSerial.Enabled = true;
                cboxRadioPort.Enabled = false;
                cboxBaud.Enabled = false;
            }
            else
            {
                btnOpenSerial.Enabled = true;
                btnCloseSerial.Enabled = false;
                cboxRadioPort.Enabled = true;
                cboxBaud.Enabled = true;
            }
        }

        private void btnSerialOK_Click(object sender, EventArgs e)
        {
            Settings.User.setPass_isOn = cboxSerialPassOn.Checked;

            if (cboxSerialPassOn.Checked)
            {
                Settings.User.setNTRIP_isOn = false;
                Settings.User.setRadio_isOn = false;
            }

            Settings.User.setNTRIP_sendToUDPPort = (int)nudSendToUDPPort.Value;

            Settings.User.setNTRIP_sendToSerial = cboxToSerial.Checked;
            Settings.User.setNTRIP_sendToUDP = cboxToUDP.Checked;

            Settings.User.setNTRIP_sendToSerial = cboxToSerial.Checked;
            Settings.User.setNTRIP_sendToUDP = cboxToUDP.Checked;

            Settings.User.setPort_portNameRadio = cboxRadioPort.Text;
            Settings.User.setPort_baudRateRadio = cboxBaud.Text;

            Log.EventWriter("Program Reset: Button OK on Serial Pass Form");

            Settings.User.Save();
            Program.Restart();
            Close();
        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
        }

        private void btnOpenSerial_Click(object sender, EventArgs e)
        {
            if (mf.spRadio != null && mf.spRadio.IsOpen)
            {
                mf.spRadio.Close();
                mf.spRadio.Dispose();
                mf.spRadio = null;
            }

            // Setup and open serial port
            mf.spRadio = new SerialPort(cboxRadioPort.Text, int.Parse(cboxBaud.Text));

            btnOpenSerial.Enabled = false;
            btnCloseSerial.Enabled = true;

            try
            {
                mf.spRadio.Open();

                lblCurrentPort.Text = Settings.User.setPort_portNameRadio;
                lblCurrentBaud.Text = Settings.User.setPort_baudRateRadio;

                cboxRadioPort.Enabled = false;
                cboxBaud.Enabled = false;
            }
            catch (Exception ex)
            {
                mf.TimedMessageBox(3000, "Error opening port", ex.Message);
                Log.EventWriter("Catch - > Error opening port" + ex.ToString());
            }
        }

        private void btnCloseSerial_Click(object sender, EventArgs e)
        {
            if (mf.spRadio != null && mf.spRadio.IsOpen)
            {
                mf.spRadio.Close();
                mf.spRadio.Dispose();
                mf.spRadio = null;

                btnOpenSerial.Enabled = true;
                btnCloseSerial.Enabled = false;

                cboxRadioPort.Enabled = true;
                cboxBaud.Enabled = true;
            }
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            cboxRadioPort.Items.Clear();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxRadioPort.Items.Add(s);
            }
        }

        private void cboxToSerial_Click(object sender, EventArgs e)
        {
            if (cboxToUDP.Checked) cboxToUDP.Checked = false;
        }

        private void cboxToUDP_Click(object sender, EventArgs e)
        {
            if (cboxToSerial.Checked) cboxToSerial.Checked = false;
        }

        private void cboxSerialPassOn_Click(object sender, EventArgs e)
        {
        }
    }
}