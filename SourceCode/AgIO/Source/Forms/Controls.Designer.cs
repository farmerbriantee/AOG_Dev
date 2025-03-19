using AgIO.Properties;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace AgIO
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
            GPS_OutSettings();
        }

        private void btnSlide_Click(object sender, EventArgs e)
        {
            if (this.Width < 600)
            {
                this.Width = 710;
                isViewAdvanced = true;
                btnSlide.BackgroundImage = Properties.Resources.ArrowGrnLeft;
                threeMinuteTimer = secondsSinceStart;
            }
            else
            {
                this.Width = 530;
                isViewAdvanced = false;
                btnSlide.BackgroundImage = Properties.Resources.ArrowGrnRight;
            }
        }

        private void btnStartStopNtrip_Click(object sender, EventArgs e)
        {
            if (Settings.User.setNTRIP_isOn || Settings.User.setRadio_isOn)
            {
                if (Settings.User.setNTRIP_isOn || Settings.User.setRadio_isOn)
                {
                    ShutDownNTRIP();
                    lblWatch.Text = "Stopped";
                    btnStartStopNtrip.Text = "OffLine";
                    Settings.User.setNTRIP_isOn = false;
                    Settings.User.setRadio_isOn = false;
                    lblNTRIP_IP.Text = "--";
                    lblMount.Text = "--";
                }
                else
                {
                    lblWatch.Text = "Waiting";
                    lblNTRIP_IP.Text = "--";
                    lblMount.Text= "--";
                }
            }
            else
            {
                TimedMessageBox(2000, "Turn on NTRIP", "NTRIP Client Not Set Up");
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
            SettingsCommunicationGPS();
            RescanPorts();
        }

        private void btnUDP_Click(object sender, EventArgs e)
        {
            if (RegistrySettings.profileName == "Default Profile")
            {
                TimedMessageBox(3000, "Using Default Profile", "Choose Existing or Create New Profile");
                return;
            }
            if (!Settings.User.setUDP_isOn) SettingsEthernet();
            else SettingsUDP();
        }

        private void btnRunAOG_Click(object sender, EventArgs e)
        {
            StartAOG();
        }

        private void btnModSim_Click(object sender, EventArgs e)
        {
            StartModsim();
        }

        private void btnNTRIP_Click(object sender, EventArgs e)
        {
            if (RegistrySettings.profileName == "Default Profile")
            {
                TimedMessageBox(3000, "Using Default Profile", "Choose Existing or Create New Profile");
                return;
            }

            SettingsNTRIP();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRadio_Click(object sender, EventArgs e)
        {
            if (RegistrySettings.profileName == "Default Profile")
            {
                TimedMessageBox(3000, "Using Default Profile", "Choose Existing or Create New Profile");
                return;
            }

            SettingsRadio();
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

        private void lblNTRIPBytes_Click(object sender, EventArgs e)
        {
            tripBytes = 0;
        }

        #endregion

        #region CheckBoxes

        private void cboxIsSteerModule_Click(object sender, EventArgs e)
        {
            Settings.User.setMod_isSteerConnected = cboxIsSteerModule.Checked;
            SetModulesOnOff();
        }

        private void cboxIsMachineModule_Click(object sender, EventArgs e)
        {
            Settings.User.setMod_isMachineConnected = cboxIsMachineModule.Checked;
            SetModulesOnOff();
        }

        private void cboxIsIMUModule_Click(object sender, EventArgs e)
        {
            Settings.User.setMod_isIMUConnected = cboxIsIMUModule.Checked;
            SetModulesOnOff();
        }

        #endregion

        #region Menu Strip Items

        private void toolStripLogViewer_Click(object sender, EventArgs e)
        {
            Form form = new FormEventViewer(Path.Combine(RegistrySettings.logsDirectory, "AgIO_Events_Log.txt"));
            form.Show(this);
            this.Activate();
        }

        private void toolStripUDPMonitor_Click(object sender, EventArgs e)
        {
            ShowUDPMonitor();
        }

        private void toolStripSerialMonitor_Click(object sender, EventArgs e)
        {
            ShowSerialMonitor();
        }

        private void deviceManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("devmgmt.msc");
        }

        private void serialPassThroughToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RegistrySettings.profileName == "Default Profile")
            {
                TimedMessageBox(3000, "Using Default Profile", "Choose Existing or Create New Profile");
                return;
            }

            if (Settings.User.setRadio_isOn)
            {
                TimedMessageBox(2000, "Radio NTRIP ON", "Turn it off before using Serial Pass Thru");
                return;
            }

            if (Settings.User.setNTRIP_isOn)
            {
                TimedMessageBox(2000, "Air NTRIP ON", "Turn it off before using Serial Pass Thru");
                return;
            }

            using (var form = new FormSerialPass(this))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    ////Clicked Save
                    //Program.Restart();
                }
            }
        }

        private void toolStripMenuProfiles_Click(object sender, EventArgs e)
        {
            if (RegistrySettings.profileName == "Default Profile")
            {
                TimedMessageBox(3000, "AgIO Default Profile Used", "Create or Choose a Profile");
            }

            using (var form = new FormProfiles(this))
            {
                form.ShowDialog(this);
                if (form.DialogResult == DialogResult.Yes)
                {
                    Log.EventWriter("Program Reset: Saving or Selecting Profile");

                    Settings.User.Save();
                    Program.Restart();
                }
            }
            this.Text = "AgIO  v" + Application.ProductVersion.ToString(CultureInfo.InvariantCulture) + "   Using Profile: " 
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

        private void toolStripGPSData_Click(object sender, EventArgs e)
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

        #endregion

        public void ShowUDPMonitor()
        {
            var form = new FormUDPMonitor(this);
            form.Show(this);
        }

        public void ShowSerialMonitor()
        {
            var form = new FormSerialMonitor(this);
            form.Show(this);
        }

        private void SettingsCommunicationGPS()
        {
            isGPSCommOpen = true;

            using (FormCommSetGPS form = new FormCommSetGPS(this))
            {
                form.ShowDialog(this);
            }
            isGPSCommOpen = false;
        }

        private void SettingsEthernet()
        {
            using (FormEthernet form = new FormEthernet(this))
            {
                form.ShowDialog(this);
            }
        }

        private void SettingsNTRIP()
        {
            if (Settings.User.setRadio_isOn)
            {
                TimedMessageBox(2000, "Radio NTRIP ON", "Turn it off before using NTRIP");
                return;
            }

            if (Settings.User.setPass_isOn)
            {
                TimedMessageBox(2000, "Serial NTRIP ON", "Turn it off before using NTRIP");
                return;
            }


            using (var form = new FormNtrip(this))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    if (isNTRIP_Connected)
                    {
                        SettingsShutDownNTRIP();
                    }
                }
            }
        }

        private void SettingsRadio()
        {
            if (Settings.User.setPass_isOn)
            {
                TimedMessageBox(2000, "Serial Pass NTRIP ON", "Turn it off before using Radio NTRIP");
                return;
            }

            if (Settings.User.setNTRIP_isOn)
            {
                TimedMessageBox(2000, "Air NTRIP ON", "Turn it off before using Radio NTRIP");
                return;
            }

            if (Settings.User.setRadio_isOn && isNTRIP_Connected)
            {
                ShutDownNTRIP();
                lblWatch.Text = "Stopped";
                btnStartStopNtrip.Text = "OffLine";
                Settings.User.setRadio_isOn = false;
            }

            using (var form = new FormRadio(this))
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

        private void StartAOG()
        {
            Process[] processName = Process.GetProcessesByName("AOG");
            if (processName.Length == 0)
            {
                //Start application here
                string strPath = Path.Combine(Application.StartupPath, "AOG.exe");

                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.FileName = strPath;
                    processInfo.WorkingDirectory = Path.GetDirectoryName(strPath);
                    Process proc = Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find AOG");
                    Log.EventWriter("Can't Find AOG - File Not Found");
                }
            }
            else
            {
                //Set foreground window
                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);
            }
        }

        private void StartModsim()
        {
            Process[] processName = Process.GetProcessesByName("Modsim");
            if (processName.Length == 0)
            {
                //Start application here
                string strPath = Path.Combine(Application.StartupPath, "Modsim.exe");

                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.FileName = strPath;
                    processInfo.WorkingDirectory = Path.GetDirectoryName(strPath);
                    Process proc = Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find Modsim");
                    Log.EventWriter("Can't Find Modsim - File Not Found");
                }
            }
            else
            {
                //Set foreground window
                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);
            }
        }


        private void GPS_OutSettings()
        {
            using (FormGPSOut form = new FormGPSOut(this))
            {
                form.ShowDialog(this);
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
