using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormGPSOut : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        //constructor
        public FormGPSOut(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();
        }

        private void FormCommSet_Load(object sender, EventArgs e)
        {
            //check if GPS port is open or closed and set buttons accordingly
            if (mf.spGPSOut.IsOpen)
            {
                cboxBaud.Enabled = false;
                cboxPort.Enabled = false;
                btnCloseSerial.Enabled = true;
                btnOpenSerial.Enabled = false;
            }
            else
            {
                cboxBaud.Enabled = true;
                cboxPort.Enabled = true;
                btnCloseSerial.Enabled = false;
                btnOpenSerial.Enabled = true;
            }

            //load the port box with valid port names
            cboxPort.Items.Clear();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxPort.Items.Add(s);
            }

            lblCurrentBaud.Text = mf.spGPS.BaudRate.ToString();
        }


        #region PortSettings //----------------------------------------------------------------

        // GPSOut Serial Port
        private void cboxBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            mf.spGPSOut.BaudRate = Convert.ToInt32(cboxBaud.Text);
            Settings.User.setPort_baudRateGPSOut = Convert.ToInt32(cboxBaud.Text);
        }

        private void cboxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            mf.spGPSOut.PortName = cboxPort.Text;
            Settings.User.setPort_portNameGPSOut = cboxPort.Text;
        }

        private void btnOpenSerial_Click(object sender, EventArgs e)
        {
            mf.OpenGPSOutPort();
            if (mf.spGPSOut.IsOpen)
            {
                cboxBaud.Enabled = false;
                cboxPort.Enabled = false;
                btnCloseSerial.Enabled = true;
                btnOpenSerial.Enabled = false;
                lblCurrentBaud.Text = mf.spGPSOut.BaudRate.ToString();
                lblCurrentPort.Text = mf.spGPSOut.PortName;
            }
            else
            {
                cboxBaud.Enabled = true;
                cboxPort.Enabled = true;
                btnCloseSerial.Enabled = false;
                btnOpenSerial.Enabled = true;
                MessageBox.Show("Unable to connect to Port");
            }
        }

        private void btnCloseSerial_Click(object sender, EventArgs e)
        {
            mf.CloseGPSOutPort();
            if (mf.spGPSOut.IsOpen)
            {
                cboxBaud.Enabled = false;
                cboxPort.Enabled = false;
                btnCloseSerial.Enabled = true;
                btnOpenSerial.Enabled = false;
            }
            else
            {
                cboxBaud.Enabled = true;
                cboxPort.Enabled = true;
                btnCloseSerial.Enabled = false;
                btnOpenSerial.Enabled = true;
            }
        }

        #endregion PortSettings //----------------------------------------------------------------

        private void timer1_Tick(object sender, EventArgs e)
        {
            //GPS phrase
            lblSteer.Text = mf.spSteerModule.PortName;
            lblGPS.Text = mf.spGPS.PortName;
            lblIMU.Text = mf.spIMU.PortName;
            lblMachine.Text = mf.spMachineModule.PortName;

            lblFromGPS.Text = mf.traffic.cntrGPSIn == 0 ? "--" : (mf.traffic.cntrGPSIn).ToString();
        }

        private void btnSerialOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            cboxPort.Items.Clear();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxPort.Items.Add(s);
            }
        }

    } //class
} //namespace