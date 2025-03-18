using AgIO.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AgIO
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

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr Handle, int x, int y, int w, int h, bool repaint);

        //static readonly int GWL_STYLE = -16;
        //static readonly int WS_VISIBLE = 0x10000000;
        //key event to restore window

        private readonly Stopwatch algoTimer = new Stopwatch();

        private const int ALT = 0xA4;

        private const int EXTENDEDKEY = 0x1;
        private const int KEYUP = 0x2;

        //Stringbuilder
        public StringBuilder logNMEASentence = new StringBuilder();

        public StringBuilder logMonitorSentence = new StringBuilder();
        public StringBuilder logUDPSentence = new StringBuilder();

        public bool isLogNMEA, isLogMonitorOn, isUDPMonitorOn, isGPSLogOn, isNTRIPLogOn;

        public bool isKeyboardOn = true, isFlash = false;
        public int gpsOutCount = 0;

        public bool isGPSSentencesOn = false; //, isSendNMEAToUDP;

        //timer variables
        public double secondsSinceStart, twoSecondTimer, tenSecondTimer, threeMinuteTimer, pingSecondsStart;

        public string lastSentence;

        public bool isNTRIPToggle;

        public bool lastHelloGPS, lastHelloAutoSteer, lastHelloMachine, lastHelloIMU, lastHelloGPSTool, lastHelloGPSOutSerial;

        //is the fly out displayed
        public bool isViewAdvanced = false;

        //used to hide the window and not update text fields and most counters
        public bool isAppInFocus = true, isLostFocus;

        public int focusSkipCounter = 310;

        public CNMEA pnGPS;

        public CNMEA_Tool pnGPSTool;


        public FormLoop()
        {
            InitializeComponent();
            bgGPSOut.DoWork += bgGPSOut_DoWork;
            bgGPSOut.ProgressChanged += bgGPSOut_ProgressChanged;
            bgGPSOut.WorkerReportsProgress = true;
        }

        //First run
        private void FormLoop_Load(object sender, EventArgs e)
        {
            pnGPS = new CNMEA(this);
            pnGPSTool = new CNMEA_Tool(this);

            if (Settings.User.setUDP_isOn)
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
            this.Width = 500;

            LoadLoopback();

            lblGPS1Comm.Text = "";
            lblIMUComm.Text = "";
            lblMod1Comm.Text = "";
            lblMod2Comm.Text = "";

            //set baud and port from last time run
            if (Settings.User.setPort_wasGPSConnected)
            {
                OpenGPSPort();
                if (spGPS.IsOpen) lblGPS1Comm.Text = Settings.User.setPort_portNameGPS;
            }

            //set baud and port from last time run
            if (Settings.User.setPort_wasGPSOutConnected)
            {
                OpenGPSOutPort();
                if (spGPSOut.IsOpen) lblGPSOut1Comm.Text = Settings.User.setPort_portNameGPSOut;
            }

            // set baud and port for rtcm from last time run
            if (Settings.User.setPort_wasRtcmConnected)
            {
                OpenRtcmPort();
            }

            //Open IMU
            if (Settings.User.setPort_wasIMUConnected)
            {
                OpenIMUPort();
                if (spIMU.IsOpen) lblIMUComm.Text = Settings.User.setPort_portNameIMU;
            }

            //same for SteerModule port
            if (Settings.User.setPort_wasSteerModuleConnected)
            {
                OpenSteerModulePort();
                if (spSteerModule.IsOpen) lblMod1Comm.Text = Settings.User.setPort_portNameSteer;
            }

            //same for MachineModule port
            if (Settings.User.setPort_wasMachineModuleConnected)
            {
                OpenMachineModulePort();
                if (spMachineModule.IsOpen) lblMod2Comm.Text = Settings.User.setPort_portNameMachine;
            }

            ConfigureNTRIP();

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                lblSerialPorts.Text = "None";
            }
            else
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    lblSerialPorts.Text = ports[i] + "\r\n";
                }
            }

            cboxIsIMUModule.Checked = Settings.User.setMod_isIMUConnected;
            cboxIsSteerModule.Checked = Settings.User.setMod_isSteerConnected;
            cboxIsMachineModule.Checked = Settings.User.setMod_isMachineConnected;

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

            //update Caster IP from URL, just use the old one if can't find
            if (Settings.User.setNTRIP_isOn)
            {
                //broadCasterIP = Settings.User.setNTRIP_casterIP; //Select correct Address
                Settings.User.setNTRIP_casterIP = null;
                string actualIP = Settings.User.setNTRIP_casterURL.Trim();

                try
                {
                    IPAddress[] addresslist = Dns.GetHostAddresses(actualIP);
                    foreach (IPAddress address in addresslist)
                    {
                        if (address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            Settings.User.setNTRIP_casterIP = address.ToString().Trim();

                            break;
                        }
                    }

                    if (Settings.User.setNTRIP_casterIP == null) throw new NullReferenceException();
                }
                catch (Exception ex)
                {
                    Log.EventWriter(ex.ToString());
                    TimedMessageBox(1500, "URL Not Located, Network Down?", "Cannot Find: " + Settings.User.setNTRIP_casterURL);
                    //if we had a timer already, kill it
                    tmr?.Dispose();

                    //use last known
                    Settings.User.setNTRIP_casterIP = Settings.User.setNTRIP_casterIP; //Select correct Address

                    // Close the socket if it is still open
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        System.Threading.Thread.Sleep(100);
                        clientSocket.Close();
                    }

                    //TimedMessageBox(2000, "NTRIP Not Connected", " Reconnect Request");
                    ntripCounter = 15;
                    isNTRIP_Connected = false;
                    isNTRIP_Starting = false;
                    isNTRIP_Connecting = false;
                    return;
                }
            }

            this.Text =
            "AgIO  v" + Application.ProductVersion.ToString(CultureInfo.InvariantCulture) + "   Profile: " + RegistrySettings.profileName;

            if (RegistrySettings.profileName == "Default Profile")
            {
                Log.EventWriter("Using Default Profile At Start Warning");

                YesMessageBox("AgIO - No Profile Open \r\n\r\n Create or Open a Profile");

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
                this.Text = "AgIO  v" + Application.ProductVersion.ToString(CultureInfo.InvariantCulture) + "  Profile: "
                    + RegistrySettings.profileName;
            }

            if (Settings.User.setDisplay_isAutoRunGPS_Out)
            {
                GPS_OutSettings();
                Log.EventWriter("Run GPS_Out");
            }
        }

        private void FormLoop_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (RegistrySettings.profileName != "Default Profile")
                Settings.User.Save();
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
            using (StreamWriter writer = new StreamWriter(Path.Combine(RegistrySettings.logsDirectory, "AgIO_Events_Log.txt"), true))
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
                this.Width = 500;
                this.Height = 530;
                return;
            }

            //to check if new data for subnet

            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            if (focusSkipCounter != 0)
            {
                lblCurentLon.Text = pnGPS.longitude.ToString("N7");
                lblCurrentLat.Text = pnGPS.latitude.ToString("N7");
            }

            lblGPSHz.Text = gpsHz.ToString("N2");

            //do all the NTRIP routines
            DoNTRIPSecondRoutine();

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

            if (focusSkipCounter != 0)
            {
                if (ntripCounter > 30)
                {
                    isNTRIPToggle = !isNTRIPToggle;
                    if (isNTRIPToggle) lblNTRIPBytes.BackColor = Color.CornflowerBlue;
                    else lblNTRIPBytes.BackColor = Color.DarkOrange;
                }
                else
                {
                    lblNTRIPBytes.BackColor = Color.Transparent;
                }
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
                lblPing.Text = lblPingMachine.Text = lblPingTool.Text = "*";
            }

            //send a hello to modules
            SendUDPMessage(helloFromAgIO, epModule);

            if (isViewAdvanced) pingSecondsStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;
            SendUDPMessageTool(helloFromAgIO, epModuleTool);
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

                #region Serial update

                if (Settings.User.setPort_wasIMUConnected)
                {
                    if (!spIMU.IsOpen)
                    {
                        byte[] imuClose = new byte[] { 0x80, 0x81, 0x7C, 0xD4, 2, 1, 0, 83 };

                        //tell AOG IMU is disconnected
                        SendToLoopBackMessageAOG(imuClose);
                        Settings.User.setPort_wasIMUConnected = false;
                        lblIMUComm.Text = "";
                    }
                }

                if (Settings.User.setPort_wasGPSConnected)
                {
                    if (!spGPS.IsOpen)
                    {
                        Settings.User.setPort_wasGPSConnected = false;
                        lblGPS1Comm.Text = "";
                    }
                }

                if (Settings.User.setPort_wasGPSOutConnected)
                {
                    if (!spGPSOut.IsOpen)
                    {
                        Settings.User.setPort_wasGPSOutConnected = false;
                        lblGPSOut1Comm.Text = "";
                    }
                }

                if (Settings.User.setPort_wasSteerModuleConnected)
                {
                    if (!spSteerModule.IsOpen)
                    {
                        Settings.User.setPort_wasSteerModuleConnected = false;
                        lblMod1Comm.Text = "";
                    }
                }

                if (Settings.User.setPort_wasMachineModuleConnected)
                {
                    if (!spMachineModule.IsOpen)
                    {
                        Settings.User.setPort_wasMachineModuleConnected = false;
                        lblMod2Comm.Text = "";
                    }
                }

                #endregion Serial update
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

            if (Settings.User.setMod_isMachineConnected)
            {
                currentHello = traffic.helloFromMachine < 3;

                if (currentHello != lastHelloMachine)
                {
                    if (currentHello) btnMachine.BackColor = Color.LimeGreen;
                    else btnMachine.BackColor = Color.Red;
                    lastHelloMachine = currentHello;
                    ShowAgIO();
                }
            }

            if (Settings.User.setMod_isSteerConnected)
            {
                currentHello = traffic.helloFromAutoSteer < 3;

                if (currentHello != lastHelloAutoSteer)
                {
                    if (currentHello) btnSteer.BackColor = Color.LimeGreen;
                    else btnSteer.BackColor = Color.Red;
                    lastHelloAutoSteer = currentHello;
                    ShowAgIO();
                }
            }

            if (Settings.User.setMod_isIMUConnected)
            {
                currentHello = traffic.helloFromIMU < 3;

                if (currentHello != lastHelloIMU)
                {
                    if (currentHello) btnIMU.BackColor = Color.LimeGreen;
                    else btnIMU.BackColor = Color.Red;
                    lastHelloIMU = currentHello;
                    ShowAgIO();
                }
            }

            currentHello = traffic.cntrGPSOut != 0;

            if (currentHello != lastHelloGPS)
            {
                if (currentHello) btnGPS.BackColor = Color.LimeGreen;
                else btnGPS.BackColor = Color.Red;
                lastHelloGPS = currentHello;
                ShowAgIO();
            }

            currentHello = traffic.cntrGPSOutTool != 0;

            if (currentHello != lastHelloGPSTool)
            {
                if (currentHello) btnGPSTool.BackColor = Color.LimeGreen;
                else btnGPSTool.BackColor = Color.Red;
                lastHelloGPSTool = currentHello;
                ShowAgIO();
            }

            currentHello = traffic.cntrGPS_OutSerial != 0;

            if (currentHello != lastHelloGPSOutSerial)
            {
                if (currentHello) btnGPS_Out.BackColor = Color.LimeGreen;
                else btnGPS_Out.BackColor = Color.Red;
                lastHelloGPSOutSerial = currentHello;
                ShowAgIO();
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

        private void ShowAgIO()
        {
            Process[] processName = Process.GetProcessesByName("AgIO");

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
            if (Settings.User.setMod_isIMUConnected)
            {
                btnIMU.Visible = true;
                lblIMUComm.Visible = true;
                cboxIsIMUModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnIMU.Visible = false;
                lblIMUComm.Visible = false;
                cboxIsIMUModule.BackgroundImage = Properties.Resources.AddNew;
            }

            if (Settings.User.setMod_isMachineConnected)
            {
                btnMachine.Visible = true;
                lblMod2Comm.Visible = true;
                cboxIsMachineModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnMachine.Visible = false;
                lblMod2Comm.Visible = false;
                cboxIsMachineModule.BackgroundImage = Properties.Resources.AddNew;
            }

            if (Settings.User.setMod_isSteerConnected)
            {
                btnSteer.Visible = true;
                lblMod1Comm.Visible = true;
                cboxIsSteerModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnSteer.Visible = false;
                lblMod1Comm.Visible = false;
                cboxIsSteerModule.BackgroundImage = Properties.Resources.AddNew;
            }
        }

        private void DoTraffic()
        {
            traffic.helloFromMachine++;
            traffic.helloFromAutoSteer++;
            traffic.helloFromIMU++;

            if (focusSkipCounter != 0)
            {
                lblFromGPS.Text = traffic.cntrGPSOut == 0 ? "---" : ((traffic.cntrGPSOut >> 1)).ToString();
                lblFromGPSTool.Text = traffic.cntrGPSOutTool == 0 ? "---" : ((traffic.cntrGPSOutTool >> 1)).ToString();
                lblGPSOutSerial.Text = traffic.cntrGPS_OutSerial == 0 ? "---" : ((traffic.cntrGPS_OutSerial)).ToString();

                //reset all counters
                traffic.cntrGPSOut = 0;
                traffic.cntrGPSOutTool = 0;
                traffic.cntrGPS_OutSerial = 0;

                lblSlowGPSOut.Text = "";

                lblCurentLon.Text = pnGPS.longitude.ToString("N7");
                lblCurrentLat.Text = pnGPS.latitude.ToString("N7");
            }
        }

        // Buttons, Checkboxes and Clicks  ***************************************************

        private void RescanPorts()
        {
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                lblSerialPorts.Text = "None";
            }
            else
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    lblSerialPorts.Text = ports[i] + " ";
                }
            }
        }
    }
}
