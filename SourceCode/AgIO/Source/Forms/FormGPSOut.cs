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
            if (FormLoop.spGPSOut.IsOpen)
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

            if (Settings.User.sendPrefixGPGN == "$GP")
            {
                rbGP.Checked = true;
                rbGN.Checked = false;
            }
            else
            {
                rbGP.Checked = false;
                rbGN.Checked = true;
            }

            this.cboGGA.SelectedIndexChanged -= cboGGA_SelectedIndexChanged;
            if (Settings.User.sendRateGGA == 0) cboGGA.Text = "0";
            if (Settings.User.sendRateGGA == 10) cboGGA.Text = "1";
            if (Settings.User.sendRateGGA == 5) cboGGA.Text = "5";
            if (Settings.User.sendRateGGA == 1) cboGGA.Text = "10";
            this.cboGGA.SelectedIndexChanged += cboGGA_SelectedIndexChanged;

            this.cboVTG.SelectedIndexChanged -= cboVTG_SelectedIndexChanged;
            if (Settings.User.sendRateVTG == 0) cboVTG.Text = "0";
            if (Settings.User.sendRateVTG == 10) cboVTG.Text = "1";
            if (Settings.User.sendRateVTG == 5) cboVTG.Text = "5";
            if (Settings.User.sendRateVTG == 1) cboVTG.Text = "10";
            this.cboVTG.SelectedIndexChanged += cboVTG_SelectedIndexChanged;

            this.cboRMC.SelectedIndexChanged -= cboRMC_SelectedIndexChanged;
            if (Settings.User.sendRateRMC == 0) cboRMC.Text = "0";
            if (Settings.User.sendRateRMC == 10) cboRMC.Text = "1";
            if (Settings.User.sendRateRMC == 5) cboRMC.Text = "5";
            if (Settings.User.sendRateRMC == 1) cboRMC.Text = "10";
            this.cboRMC.SelectedIndexChanged += cboRMC_SelectedIndexChanged;

            this.cboZDA.SelectedIndexChanged -= cboZDA_SelectedIndexChanged;
            if (Settings.User.sendRateZDA == 0) cboZDA.Text = "0";
            if (Settings.User.sendRateZDA == 10) cboZDA.Text = "1";
            if (Settings.User.sendRateZDA == 5) cboZDA.Text = "5";
            if (Settings.User.sendRateZDA == 1) cboZDA.Text = "10";
            this.cboZDA.SelectedIndexChanged += cboZDA_SelectedIndexChanged;

            this.cboGSA.SelectedIndexChanged -= cboGSA_SelectedIndexChanged;
            if (Settings.User.sendRateGSA == 0) cboGSA.Text = "0";
            if (Settings.User.sendRateGSA == 10) cboGSA.Text = "1";
            if (Settings.User.sendRateGSA == 5) cboGSA.Text = "5";
            if (Settings.User.sendRateGSA == 1) cboGSA.Text = "10";
            this.cboGSA.SelectedIndexChanged += cboGSA_SelectedIndexChanged;

        }

        #region PortSettings //----------------------------------------------------------------

        // GPSOut Serial Port
        private void cboxBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormLoop.spGPSOut.BaudRate = Convert.ToInt32(cboxBaud.Text);
            Settings.User.setPort_baudRateGPSOut = Convert.ToInt32(cboxBaud.Text);
        }

        private void cboxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormLoop.spGPSOut.PortName = cboxPort.Text;
            Settings.User.setPort_portNameGPSOut = cboxPort.Text;
        }

        private void btnOpenSerial_Click(object sender, EventArgs e)
        {
            mf.OpenGPSOutPort();
            if (FormLoop.spGPSOut.IsOpen)
            {
                cboxBaud.Enabled = false;
                cboxPort.Enabled = false;
                btnCloseSerial.Enabled = true;
                btnOpenSerial.Enabled = false;
                lblCurrentBaud.Text = FormLoop.spGPSOut.BaudRate.ToString();
                lblCurrentPort.Text = FormLoop.spGPSOut.PortName;
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
            if (FormLoop.spGPSOut.IsOpen)
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

        private void cboGGA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGGA.SelectedIndex == 0) Settings.User.sendRateGGA = 0;
            if (cboGGA.SelectedIndex == 1) Settings.User.sendRateGGA = 10;
            if (cboGGA.SelectedIndex == 2) Settings.User.sendRateGGA = 5;
            if (cboGGA.SelectedIndex == 3) Settings.User.sendRateGGA = 1;
        }

        private void cboVTG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboVTG.SelectedIndex == 0) Settings.User.sendRateVTG = 0;
            if (cboVTG.SelectedIndex == 1) Settings.User.sendRateVTG = 10;
            if (cboVTG.SelectedIndex == 2) Settings.User.sendRateVTG = 5;
            if (cboVTG.SelectedIndex == 3) Settings.User.sendRateVTG = 1;
        }

        private void cboRMC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRMC.SelectedIndex == 0) Settings.User.sendRateRMC = 0;
            if (cboRMC.SelectedIndex == 1) Settings.User.sendRateRMC = 10;
            if (cboRMC.SelectedIndex == 2) Settings.User.sendRateRMC = 5;
            if (cboRMC.SelectedIndex == 3) Settings.User.sendRateRMC = 1;
        }

        private void cboZDA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboZDA.SelectedIndex == 0) Settings.User.sendRateZDA = 0;
            if (cboZDA.SelectedIndex == 1) Settings.User.sendRateZDA = 10;
            if (cboZDA.SelectedIndex == 2) Settings.User.sendRateZDA = 5;
            if (cboZDA.SelectedIndex == 3) Settings.User.sendRateZDA = 1;
        }

        private void cboGSA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGSA.SelectedIndex == 0) Settings.User.sendRateGSA = 0;
            if (cboGSA.SelectedIndex == 1) Settings.User.sendRateGSA = 10;
            if (cboGSA.SelectedIndex == 2) Settings.User.sendRateGSA = 5;
            if (cboGSA.SelectedIndex == 3) Settings.User.sendRateGSA = 1;
        }

        private void btnAllOff_Click(object sender, EventArgs e)
        {
            cboVTG.Text = "0";
            cboRMC.Text = "0";
            cboGGA.Text = "0";
            cboZDA.Text = "0";
            cboGSA.Text = "0";

            mf.CloseGPSOutPort();
            if (FormLoop.spGPSOut.IsOpen)
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

        private void rbGP_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGP.Checked) Settings.User.sendPrefixGPGN = "$GP";
            else Settings.User.sendPrefixGPGN = "$GN";
        }
    } //class
} //namespace