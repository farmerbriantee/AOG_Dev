using AgTwo.Properties;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace AgTwo
{
    public partial class FormLoop
    {
        public void TimedMessageBox(int timeout, string title, string message)
        {
            var form = new FormTimedMessage(timeout, title, message);
            form.Show();
        }

        public void YesMessageBox(string s1)
        {
            var form = new FormYes(s1);
            form.ShowDialog(this);
        }

        #region Buttons

        private void btnGPS_Out_Click(object sender, EventArgs e)
        {
            StartGPS_Out();
        }

        private void btnSlide_Click(object sender, EventArgs e)
        {
            if (this.Width < 600)
            {
                this.Width = 760;
                isViewAdvanced = true;
                btnSlide.BackgroundImage = Properties.Resources.ArrowGrnLeft;
                sbRTCM.Clear();
                threeMinuteTimer = secondsSinceStart;
            }
            else
            {
                this.Width = 428;
                isViewAdvanced = false;
                btnSlide.BackgroundImage = Properties.Resources.ArrowGrnRight;
            }
        }

        private void btnMinimizeMainForm_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnGPSData_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                isGPSSentencesOn = false;
                return;
            }

            isGPSSentencesOn = true;

            Form form = new FormGPSData(this);
            form.Show(this);
        }

        private void btnBringUpCommSettings_Click(object sender, EventArgs e)
        {
        }

        private void btnUDP_Click(object sender, EventArgs e)
        {
            if (RegistrySettings.profileName == "Default Profile")
            {
                TimedMessageBox(3000, "Using Default Profile", "Choose Existing or Create New Profile");
                return;
            }
            if (!Settings.Default.setUDP_isOn) SettingsEthernet();
            else SettingsUDP();
        }

        private void btnRunAby_Click(object sender, EventArgs e)
        {
            StartAby();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region labels
        private void lblIP_Click(object sender, EventArgs e)
        {
            lblIP.Text = "";
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    _ = IPA.ToString();
                    lblIP.Text += IPA.ToString() + "\r\n";
                }
            }
        }

        #endregion

        #region CheckBoxes

        private void cboxIsSteerModule_Click(object sender, EventArgs e)
        {
            isConnectedSteer = cboxIsSteerModule.Checked;
            SetModulesOnOff();
        }

        private void cboxIsMachineModule_Click(object sender, EventArgs e)
        {
            isConnectedMachine = cboxIsMachineModule.Checked;
            SetModulesOnOff();
        }

        private void cboxIsIMUModule_Click(object sender, EventArgs e)
        {
            isConnectedIMU = cboxIsIMUModule.Checked;
            SetModulesOnOff();
        }

        #endregion

        #region Menu Strip Items

        private void toolStripLogViewer_Click(object sender, EventArgs e)
        {
            Form form = new FormEventViewer(Path.Combine(RegistrySettings.logsDirectory, "AgTwo_Events_Log.txt"));
            form.Show(this);
            this.Activate();
        }

        private void toolStripUDPMonitor_Click(object sender, EventArgs e)
        {
            ShowUDPMonitor();
        }

        private void deviceManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("devmgmt.msc");
        }

        private void toolStripMenuProfiles_Click(object sender, EventArgs e)
        {
            if (RegistrySettings.profileName == "Default Profile")
            {
                TimedMessageBox(3000, "AgTwo Default Profile Used", "Create or Choose a Profile");
            }

            using (var form = new FormProfiles(this))
            {
                form.ShowDialog(this);
                if (form.DialogResult == DialogResult.Yes)
                {
                    Log.EventWriter("Program Reset: Saving or Selecting Profile");

                    RegistrySettings.Save();
                    Application.Restart();
                    Environment.Exit(0);
                }
            }
            this.Text = "AgTwo  v" + Application.ProductVersion.ToString(CultureInfo.InvariantCulture) + "   Using Profile: " 
                + RegistrySettings.profileName;
        }

        private void modSimToolStrip_Click(object sender, EventArgs e)
        {
            Process[] processName = Process.GetProcessesByName("ModSim");
            if (processName.Length == 0)
            {
                //Start application here
                string strPath = Path.Combine(Application.StartupPath, "ModSim.exe");

                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.FileName = strPath;
                    processInfo.WorkingDirectory = Path.GetDirectoryName(strPath);
                    Process proc = Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find Simulator");
                    Log.EventWriter("Catch -> Failed to load ModSim - Not Found");
                }
            }
            else
            {
                //Set foreground window
                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);
            }

        }

        private void toolStripEthernet_Click(object sender, EventArgs e)
        {
            SettingsEthernet();
        }

        #endregion

        public void ShowUDPMonitor()
        {
            var form = new FormUDPMonitor(this);
            form.Show(this);
        }

        private void SettingsEthernet()
        {
            using (FormEthernet form = new FormEthernet(this))
            {
                form.ShowDialog(this);
            }
        }

        private void SettingsUDP()
        {
            FormUDP formEth = new FormUDP(this);
            {
                formEth.Show(this);
            }
        }

        private void StartAby()
        {
            Process[] processName = Process.GetProcessesByName("AgOpenGPS");
            if (processName.Length == 0)
            {
                //Start application here
                string strPath = Path.Combine(Application.StartupPath, "AgOpenGPS.exe");

                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.FileName = strPath;
                    processInfo.WorkingDirectory = Path.GetDirectoryName(strPath);
                    Process proc = Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find AgOpenGPS");
                    Log.EventWriter("Can't Find AgOpenGPS - File Not Found");
                }
            }
            else
            {
                //Set foreground window
                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);
            }
        }

        private void StartGPS_Out()
        {
            Process[] processName = Process.GetProcessesByName("GPS_Out");
            if (processName.Length == 0)
            {
                //Start application here
                string strPath = Path.Combine(Application.StartupPath, "GPS_Out.exe");

                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.FileName = strPath;
                    processInfo.WorkingDirectory = Path.GetDirectoryName(strPath);
                    Process proc = Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find GPS_Out");
                    Log.EventWriter("No File Found, Can't Find GPS_Out");
                }
            }
            else
            {
                //Set foreground window
                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);
            }
        }

        public void KeypadToNUD(NumericUpDown sender, Form owner)
        {
            sender.BackColor = System.Drawing.Color.Red;
            using (var form = new FormNumeric((double)sender.Minimum, (double)sender.Maximum, (double)sender.Value))
            {
                if (form.ShowDialog(owner) == DialogResult.OK)
                {
                    sender.Value = (decimal)form.ReturnValue;
                }
            }
            sender.BackColor = System.Drawing.Color.AliceBlue;
        }

        public void KeyboardToText(TextBox sender, Form owner)
        {
            TextBox tbox = (TextBox)sender;
            tbox.BackColor = System.Drawing.Color.Red;
            using (var form = new FormKeyboard((string)tbox.Text))
            {
                if (form.ShowDialog(owner) == DialogResult.OK)
                {
                    tbox.Text = (string)form.ReturnString;
                }
            }
            tbox.BackColor = System.Drawing.Color.AliceBlue;
        }

        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem toolStripMenuProfiles;
        private ToolStripMenuItem deviceManagerToolStripMenuItem;
    }
}
