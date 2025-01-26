using AgTwo.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AgTwo
{
    public partial class FormLoop : Form
    {
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWind, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool IsIconic(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        //key event to restore window
        private const int ALT = 0xA4;

        private const int EXTENDEDKEY = 0x1;
        private const int KEYUP = 0x2;

        //Stringbuilder
        public StringBuilder logNMEASentence = new StringBuilder();

        public StringBuilder logMonitorSentence = new StringBuilder();
        public StringBuilder logUDPSentence = new StringBuilder();
        public bool isLogNMEA, isLogMonitorOn, isUDPMonitorOn, isGPSLogOn, isNTRIPLogOn;

        private StringBuilder sbRTCM = new StringBuilder();

        public bool isKeyboardOn = true;

        public bool isGPSSentencesOn = false, isSendNMEAToUDP;

        //timer variables
        public double secondsSinceStart, twoSecondTimer, tenSecondTimer, threeMinuteTimer, pingSecondsStart;

        public string lastSentence;

        public bool lastHelloGPS, lastHelloAutoSteer, lastHelloMachine, lastHelloIMU;
        public bool isConnectedIMU, isConnectedSteer, isConnectedMachine;

        //is the fly out displayed
        public bool isViewAdvanced = false;

        //used to hide the window and not update text fields and most counters
        public bool isAppInFocus = true, isLostFocus;

        public int focusSkipCounter = 310;

        public FormLoop()
        {
            InitializeComponent();
        }

        //First run
        private void FormLoop_Load(object sender, EventArgs e)
        {
            if (Settings.Default.setUDP_isOn)
            {
                LoadUDPNetwork();
                Log.EventWriter("UDP Network Is On");
            }
            else
            {
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label9.Visible = false;

                lblSteerAngle.Visible = false;
                lblWASCounts.Visible = false;
                lblSwitchStatus.Visible = false;
                lblWorkSwitchStatus.Visible = false;

                label10.Visible = false;
                label12.Visible = false;
                lbl1To8.Visible = false;
                lbl9To16.Visible = false;

                btnUDP.BackColor = Color.Gainsboro;
                lblIP.Text = "Off";
            }

            //small view
            this.Width = 428;

            LoadLoopback();

            isSendNMEAToUDP = Properties.Settings.Default.setUDP_isSendNMEAToUDP;


            isConnectedIMU = cboxIsIMUModule.Checked = Properties.Settings.Default.setMod_isIMUConnected;
            isConnectedSteer = cboxIsSteerModule.Checked = Properties.Settings.Default.setMod_isSteerConnected;
            isConnectedMachine = cboxIsMachineModule.Checked = Properties.Settings.Default.setMod_isMachineConnected;

            SetModulesOnOff();

            oneSecondLoopTimer.Enabled = true;
            pictureBox1.Visible = true;
            pictureBox1.BringToFront();
            pictureBox1.Width = 430;
            pictureBox1.Height = 500;
            pictureBox1.Left = 0;
            pictureBox1.Top = 0;
            //pictureBox1.Dock = DockStyle.Fill;:

            //On or off the module rows
            SetModulesOnOff();

            this.Text =
            "AgTwo  v" + Application.ProductVersion.ToString(CultureInfo.InvariantCulture) + " Profile: " + RegistrySettings.profileName;

            if (RegistrySettings.profileName == "Default Profile")
            {
                Log.EventWriter("Using Default Profile At Start Warning");

                YesMessageBox("AgTwo - No Profile Open \r\n\r\n Create or Open a Profile");

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
                this.Text = "AgTwo  v" + Application.ProductVersion.ToString(CultureInfo.InvariantCulture) + " Profile: "
                    + RegistrySettings.profileName;
            }

            if (Properties.Settings.Default.setDisplay_isAutoRunGPS_Out)
            {
                StartGPS_Out();
                Log.EventWriter("Run GPS_Out");
            }
        }

        private void FormLoop_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (RegistrySettings.profileName != "Default Profile")
                RegistrySettings.Save();
            else
                YesMessageBox("Using Default Profile" + "\r\n\r\n" + "Changes will NOT be Saved");

            if (loopBackSocket != null)
            {
                try
                {
                    loopBackSocket.Shutdown(SocketShutdown.Both);
                }
                finally { loopBackSocket.Close(); }
            }

            if (UDPSocket != null)
            {
                try
                {
                    UDPSocket.Shutdown(SocketShutdown.Both);
                }
                finally { UDPSocket.Close(); }
            }

            Process[] processName = Process.GetProcessesByName("GPS_Out");
            if (processName.Length != 0)
            {
                processName[0].CloseMainWindow();
            }

            Log.EventWriter("Program Exit: " +
                DateTime.Now.ToString("f", CultureInfo.CreateSpecificCulture(RegistrySettings.culture)) + "\n\r");

            FileSaveSystemEvents();
        }

        public void FileSaveSystemEvents()
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(RegistrySettings.logsDirectory, "AgTwo_Events_Log.txt"), true))
            {
                writer.Write(Log.sbEvent);
                Log.sbEvent.Clear();
            }
        }

        private void oneSecondLoopTimer_Tick(object sender, EventArgs e)
        {
            if (oneSecondLoopTimer.Interval > 1200)
            {
                Controls.Remove(pictureBox1);
                pictureBox1.Dispose();
                oneSecondLoopTimer.Interval = 1000;
                this.Width = 428;
                this.Height = 530;
                return;
            }

            //to check if new data for subnet

            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            if (focusSkipCounter != 0)
            {
                lblCurentLon.Text = longitude.ToString("N7");
                lblCurrentLat.Text = latitude.ToString("N7");
            }

            #region Sleep

            //is this the active window
            isAppInFocus = FormLoop.ActiveForm != null;

            //start counting down to minimize
            if (!isAppInFocus && !isLostFocus)
            {
                focusSkipCounter = 310;
                isLostFocus = true;
            }

            // Is active window again
            if (isAppInFocus && isLostFocus)
            {
                isLostFocus = false;
                focusSkipCounter = int.MaxValue;
            }

            if (isLostFocus && focusSkipCounter != 0)
            {
                if (focusSkipCounter == 1)
                {
                    WindowState = FormWindowState.Minimized;
                }

                focusSkipCounter--;
            }

            #endregion Sleep

            //every couple or so seconds
            if ((secondsSinceStart - twoSecondTimer) > 2)
            {
                TwoSecondLoop();
                twoSecondTimer = secondsSinceStart;
            }

            //every 10 seconds
            if ((secondsSinceStart - tenSecondTimer) > 9.5)
            {
                TenSecondLoop();
                tenSecondTimer = secondsSinceStart;
            }

            //3 minute egg timer
            if ((secondsSinceStart - threeMinuteTimer) > 180)
            {
                ThreeMinuteLoop();
                threeMinuteTimer = secondsSinceStart;
            }
        }

        private void TwoSecondLoop()
        {
            //Hello Alarm logic
            DoHelloAlarmLogic();

            DoTraffic();

            if (isViewAdvanced)
            {
                pingSecondsStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;
                lblPing.Text = lblPingMachine.Text = "*";
            }

            //send a hello to modules
            SendUDPMessage(helloFromAgTwo, epModule);

            //if (isLogNMEA)
            //{
            //    using (StreamWriter writer = new StreamWriter("zAgTwo_log.txt", true))
            //    {
            //        writer.Write(logNMEASentence.ToString());
            //    }
            //    logNMEASentence.Clear();
            //}

            //if (focusSkipCounter < 310) lblSkipCounter.Text = focusSkipCounter.ToString();
            //else lblSkipCounter.Text = "On";
        }

        private void TenSecondLoop()
        {
            if (focusSkipCounter != 0 && WindowState == FormWindowState.Minimized)
            {
                focusSkipCounter = 0;
                isLostFocus = true;
            }

            if (focusSkipCounter != 0)
            {
                //update connections
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
        }

        private void ThreeMinuteLoop()
        {
            if (isViewAdvanced)
            {
                btnSlide.PerformClick();
            }
        }

        private void DoHelloAlarmLogic()
        {
            bool currentHello;

            if (isConnectedMachine)
            {
                currentHello = traffic.helloFromMachine < 3;

                if (currentHello != lastHelloMachine)
                {
                    if (currentHello) btnMachine.BackColor = Color.LimeGreen;
                    else btnMachine.BackColor = Color.Red;
                    lastHelloMachine = currentHello;
                    ShowAgTwo();
                }
            }

            if (isConnectedSteer)
            {
                currentHello = traffic.helloFromAutoSteer < 3;

                if (currentHello != lastHelloAutoSteer)
                {
                    if (currentHello) btnSteer.BackColor = Color.LimeGreen;
                    else btnSteer.BackColor = Color.Red;
                    lastHelloAutoSteer = currentHello;
                    ShowAgTwo();
                }
            }

            if (isConnectedIMU)
            {
                currentHello = traffic.helloFromIMU < 3;

                if (currentHello != lastHelloIMU)
                {
                    if (currentHello) btnIMU.BackColor = Color.LimeGreen;
                    else btnIMU.BackColor = Color.Red;
                    lastHelloIMU = currentHello;
                    ShowAgTwo();
                }
            }

            currentHello = traffic.cntrGPSOut != 0;

            if (currentHello != lastHelloGPS)
            {
                if (currentHello) btnGPS.BackColor = Color.LimeGreen;
                else btnGPS.BackColor = Color.Red;
                lastHelloGPS = currentHello;
                ShowAgTwo();
            }
        }

        private void FormLoop_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (isViewAdvanced) btnSlide.PerformClick();
                isLostFocus = true;
                focusSkipCounter = 0;
            }
        }

        private void ShowAgTwo()
        {
            Process[] processName = Process.GetProcessesByName("AgTwo");

            if (processName.Length != 0)
            {
                // Guard: check if window already has focus.
                if (processName[0].MainWindowHandle == GetForegroundWindow()) return;

                // Show window maximized.
                ShowWindow(processName[0].MainWindowHandle, 9);

                // Simulate an "ALT" key press.
                keybd_event((byte)ALT, 0x45, EXTENDEDKEY | 0, 0);

                // Simulate an "ALT" key release.
                keybd_event((byte)ALT, 0x45, EXTENDEDKEY | KEYUP, 0);

                // Show window in forground.
                SetForegroundWindow(processName[0].MainWindowHandle);
            }

            //{
            //    //Set foreground window
            //    if (IsIconic(processName[0].MainWindowHandle))
            //    {
            //        ShowWindow(processName[0].MainWindowHandle, 9);
            //    }
            //    SetForegroundWindow(processName[0].MainWindowHandle);
            //}
        }

        public void SetModulesOnOff()
        {
            if (isConnectedIMU)
            {
                btnIMU.Visible = true;
                cboxIsIMUModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnIMU.Visible = false;
                cboxIsIMUModule.BackgroundImage = Properties.Resources.AddNew;
            }

            if (isConnectedMachine)
            {
                btnMachine.Visible = true;
                cboxIsMachineModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnMachine.Visible = false;
                cboxIsMachineModule.BackgroundImage = Properties.Resources.AddNew;
            }

            if (isConnectedSteer)
            {
                btnSteer.Visible = true;
                cboxIsSteerModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnSteer.Visible = false;
                cboxIsSteerModule.BackgroundImage = Properties.Resources.AddNew;
            }

            Properties.Settings.Default.setMod_isIMUConnected = isConnectedIMU;
            Properties.Settings.Default.setMod_isSteerConnected = isConnectedSteer;
            Properties.Settings.Default.setMod_isMachineConnected = isConnectedMachine;
        }

        private void DoTraffic()
        {
            traffic.helloFromMachine++;
            traffic.helloFromAutoSteer++;
            traffic.helloFromIMU++;

            if (focusSkipCounter != 0)
            {
                lblFromGPS.Text = traffic.cntrGPSOut == 0 ? "---" : ((traffic.cntrGPSOut >> 1)).ToString();

                //reset all counters
                traffic.cntrGPSOut = 0;

                lblCurentLon.Text = longitude.ToString("N7");
                lblCurrentLat.Text = latitude.ToString("N7");
            }
        }
    }
}