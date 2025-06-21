using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AgIO
{
    public class CTraffic
    {
        public int cntrGPSIn = 0;
        public int cntrGPSInBytes = 0;
        public int cntrGPSOut = 0;
        public int cntrGPSOutTool = 0;
        public int cntrGPS_OutSerial = 0;

        public uint helloFromMachine = 99, helloFromAutoSteer = 99, helloFromIMU = 99;
    }

    public class CScanReply
    {
        public string steerIP =   "";
        public string machineIP = "";
        public string GPS_IP =    "";
        public string IMU_IP =    "";
        public string subnetStr = "";

        public byte[] subnet = { 0, 0, 0 };

        public bool isNewSteer, isNewMachine, isNewGPS, isNewIMU, isNewGPSTool;

        public bool isNewData = false;
    }

    public partial class FormLoop
    {
        private readonly Stopwatch swFrame = new Stopwatch();
        public double frameTime = 0;
        public static double gpsHz = 10;
        public double nowHz = 0;

        // loopback Socket
        private Socket loopBackSocket, loopBackSocketTool;
        private EndPoint endPointLoopBack = new IPEndPoint(IPAddress.Loopback, 0);
        private EndPoint endPointLoopBackTool = new IPEndPoint(IPAddress.Loopback, 0);

        // UDP Socket
        public Socket UDPSocket, UDPSocketTool;
        private EndPoint endPointUDP = new IPEndPoint(IPAddress.Any, 0);
        private EndPoint endPointUDPTool = new IPEndPoint(IPAddress.Any, 0);

        public bool isUDPNetworkConnected,isUDPNetworkConnectedTool;

        //2 endpoints for local and 2 udp
        private IPEndPoint epAgOpen = new IPEndPoint(IPAddress.Parse(
            Settings.User.eth_loopOne.ToString() + "." +
            Settings.User.eth_loopTwo.ToString() + "." +
            Settings.User.eth_loopThree.ToString() + "." +
            Settings.User.eth_loopFour.ToString()), 15555);

        private IPEndPoint epAgOpenTool = new IPEndPoint(IPAddress.Parse(
            Settings.User.eth_loopOne.ToString() + "." +
            Settings.User.eth_loopTwo.ToString() + "." +
            Settings.User.eth_loopThree.ToString() + "." +
            Settings.User.eth_loopFour.ToString()), 25555);

        public IPEndPoint epModule = new IPEndPoint(IPAddress.Parse(
            Settings.User.etIP_SubnetOne.ToString() + "." +
            Settings.User.etIP_SubnetTwo.ToString() + "." +
            Settings.User.etIP_SubnetThree.ToString() + ".255"), 8888);

        public IPEndPoint epModuleTool = new IPEndPoint(IPAddress.Parse(
            Settings.User.etIP_SubnetOne.ToString() + "." +
            Settings.User.etIP_SubnetTwo.ToString() + "." +
            Settings.User.etIP_SubnetThree.ToString() + ".255"), 18888);

        public IPEndPoint epModuleSet = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 8888);
        public IPEndPoint epModuleSetTool = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 18888);

        private IPEndPoint epNtrip;

        public byte[] ipAutoSet = { 192, 168, 5 };

        //public IPEndPoint epHello = new IPEndPoint(IPAddress.Parse(
        //        Settings.User.etIP_SubnetOne.ToString() + "." +
        //        Settings.User.etIP_SubnetTwo.ToString() + "." +
        //        Settings.User.etIP_SubnetThree.ToString() + ".255"), 27777);

        //class for counting bytes
        public CTraffic traffic = new CTraffic();
        public CScanReply scanReply = new CScanReply();
        public CScanReply scanReplyTool = new CScanReply();

        //scan results placed here
        public string scanReturn = "Scanning...";

        // Data stream
        private byte[] buffer = new byte[1024];
        private byte[] bufferTool = new byte[1024];

        //used to send communication check pgn= C8 or 200
        private byte[] helloFromAgIO = { 0x80, 0x81, 0x7F, 200, 3, 56, 0, 0, 0x47 };

        //initialize loopback and udp network
        public void LoadUDPNetwork()
        {
            helloFromAgIO[5] = 56;

            lblIP.Text = "";
            try //udp network
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily == AddressFamily.InterNetwork)
                    {
                        string  data = IPA.ToString();
                        lblIP.Text += IPA.ToString().Trim() + "\r\n";
                    }
                }

                // Initialise the socket
                UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                UDPSocket.Bind(new IPEndPoint(IPAddress.Any, 9999));
                UDPSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPointUDP,
                    new AsyncCallback(ReceiveDataUDPAsync), null);

                isUDPNetworkConnected = true;

                // Initialise the Tool socket
                UDPSocketTool = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                UDPSocketTool.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                UDPSocketTool.Bind(new IPEndPoint(IPAddress.Any, 19999));
                UDPSocketTool.BeginReceiveFrom(bufferTool, 0, bufferTool.Length, SocketFlags.None, ref endPointUDPTool,
                    new AsyncCallback(ReceiveDataUDPAsyncTool), null);

                isUDPNetworkConnectedTool = true;

                if (isUDPNetworkConnected)
                {
                    Log.EventWriter("UDP Network is connected: " + epModule.ToString());
                }
                else
                {
                    Log.EventWriter("UDP Network Failed to Connect");
                }

                if (isUDPNetworkConnectedTool)
                {
                    Log.EventWriter("UDP Tool Network is connected: " + epModuleTool.ToString());
                }
                else
                {
                    Log.EventWriter("UDP Tool Network Failed to Connect");
                }

                btnUDP.BackColor = Color.LimeGreen;

                //if (!isFound)
                //{
                //    MessageBox.Show("Network Address of Modules -> " + Settings.User.setIP_localAOG+"[2 - 254] May not exist. \r\n"
                //    + "Are you sure ethernet is connected?\r\n" + "Go to UDP Settings to fix.\r\n\r\n", "Network Connection Error",
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    //btnUDP.BackColor = Color.Red;
                //    lblIP.Text = "Not Connected";
                //}
            }
            catch (Exception e)
            {
                Log.EventWriter("Catch -> Load UDP Server" + e);
                MessageBox.Show(e.Message, "Serious Network Connection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnUDP.BackColor = Color.Red;
                lblIP.Text = "Error";
            }
        }

        private void LoadLoopback()
        { 
            try //loopback
            {
                loopBackSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                loopBackSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                loopBackSocket.Bind(new IPEndPoint(IPAddress.Loopback, 17777));
                loopBackSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPointLoopBack,
                    new AsyncCallback(ReceiveDataLoopAsync), null);
                Log.EventWriter("Loopback is Connected: " + IPAddress.Loopback.ToString() + ":17777");

                loopBackSocketTool = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                loopBackSocketTool.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                loopBackSocketTool.Bind(new IPEndPoint(IPAddress.Loopback, 27777));
                loopBackSocketTool.BeginReceiveFrom(bufferTool, 0, bufferTool.Length, SocketFlags.None, ref endPointLoopBackTool,
                    new AsyncCallback(ReceiveDataLoopAsyncTool), null);
                Log.EventWriter("Loopback is Connected: " + IPAddress.Loopback.ToString() + ":27777");

            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch - > Load UDP Loopback Failed: " + ex.ToString());
                MessageBox.Show("Load Error: " + ex.Message, "Loopback Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Send LoopBack

        public void SendToLoopBackMessageAOG(byte[] byteData)
        {
            SendDataToLoopBack(byteData, epAgOpen);
        }

        private void SendDataToLoopBack(byte[] byteData, IPEndPoint endPoint)
        {
            try
            {
                if (byteData.Length != 0)
                {
                    // Send packet to AgVR
                    loopBackSocket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, endPoint,
                        new AsyncCallback(SendDataLoopAsync), null);
                }
            }
            catch
            {
            }
        }

        public void SendDataLoopAsync(IAsyncResult asyncResult)
        {
            try
            {
                loopBackSocket.EndSend(asyncResult);
            }
            catch
            {
            }
        }

        #endregion

        #region Receive LoopBack

        private void ReceiveFromLoopBack(byte[] data)
        {
            //Send out to udp network
            SendUDPMessage(data, epModule);

            if (data[0] == 0x80 && data[1] == 0x81)
            {
                switch (data[3])
                {
                    case 0xFE: //254 AutoSteer Data
                        {
                            //serList.AddRange(data);
                            SendSteerModulePort(data, data.Length);
                            SendMachineModulePort(data, data.Length);
                            break;
                        }
                    case 0xEF: //239 machine pgn
                        {
                            SendMachineModulePort(data, data.Length);
                            SendSteerModulePort(data, data.Length);
                            break;
                        }
                    case 227: //227 Spray sections and rate Data
                        {
                            SendMachineModulePort(data, data.Length);
                            break;
                        }
                    case 226: //227 Spray config
                        {
                            SendMachineModulePort(data, data.Length);
                            break;
                        }
                    case 225: //227 Spray functions
                        {
                            SendMachineModulePort(data, data.Length);
                            break;
                        }
                    case 229: //229 Symmetric Sections - Zones
                        {
                            SendMachineModulePort(data, data.Length);
                            //SendSteerModulePort(data, data.Length);
                            break;
                        }
                    case 0xFC: //252 steer settings
                        {
                            SendSteerModulePort(data, data.Length);
                            break;
                        }
                    case 0xFB: //251 steer config
                        {
                            SendSteerModulePort(data, data.Length);
                            break;
                        }

                    case 0xEE: //238 machine config
                        {
                            SendMachineModulePort(data, data.Length);
                            SendSteerModulePort(data, data.Length);
                            break;
                        }

                    case 0xEC: //236 machine config
                        {
                            SendMachineModulePort(data, data.Length);
                            SendSteerModulePort(data, data.Length);
                            break;
                        }
                }
            }                            
        }

        private void ReceiveDataLoopAsync(IAsyncResult asyncResult)
        {
            try
            {
                // Receive all data
                int msgLen = loopBackSocket.EndReceiveFrom(asyncResult, ref endPointLoopBack);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(buffer, localMsg, msgLen);

                // Listen for more connections again...
                loopBackSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPointLoopBack, 
                    new AsyncCallback(ReceiveDataLoopAsync), null);

                BeginInvoke((MethodInvoker)(() => ReceiveFromLoopBack(localMsg)));
            }
            catch
            {
            }
        }

        #endregion

        #region Send UDP

        public void SendUDPMessage(byte[] byteData, IPEndPoint endPoint)
        {
            if (isUDPNetworkConnected)
            {
                if (isUDPMonitorOn)
                {
                    if (epNtrip != null && endPoint.Port == epNtrip.Port)
                    {
                        if (isNTRIPLogOn)
                            logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + endPoint.ToString() + "\t" + " > NTRIP\r\n");
                    }
                    else
                    {
                        logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + endPoint.ToString() + "\t" + " > " + byteData[3].ToString() + "\r\n");
                    }
                }

                try
                {
                    // Send packet to the zero
                    if (byteData.Length != 0)
                    {
                        UDPSocket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None,
                           endPoint, new AsyncCallback(SendDataUDPAsync), null);
                    }
                }
                catch (Exception)
                {
                    //WriteErrorLog("Sending UDP Message" + e.ToString());
                    //MessageBox.Show("Send Error: " + e.Message, "UDP Client", MessageBoxButtons.OK,
                    //MessageBoxIcon.Error);
                }
            }
        }

        private void SendDataUDPAsync(IAsyncResult asyncResult)
        {
            try
            {
                UDPSocket.EndSend(asyncResult);
            }
            catch
            {
            }
        }

        #endregion

        #region Receive UDP

        private void ReceiveDataUDPAsync(IAsyncResult asyncResult)
        {
            try
            {
                // Receive all data
                int msgLen = UDPSocket.EndReceiveFrom(asyncResult, ref endPointUDP);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(buffer, localMsg, msgLen);

                // Listen for more connections again...
                UDPSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPointUDP, 
                    new AsyncCallback(ReceiveDataUDPAsync), null);

                BeginInvoke((MethodInvoker)(() => ReceiveFromUDP(localMsg)));

            }
            catch 
            {
            }
        }

        private void ReceiveFromUDP(byte[] data)
        {
            try
            {
                if (data[0] == 0x80 && data[1] == 0x81)
                {
                    //module return via udp sent to AgOpenGPS
                    SendToLoopBackMessageAOG(data);

                    //check for Scan and Hello
                    if (data[3] == 126 && data.Length == 11)
                    {

                        traffic.helloFromAutoSteer = 0;
                        if (isViewAdvanced)
                        {
                            lblPing.Text = (((DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds - pingSecondsStart) * 1000).ToString("N0");
                            double actualSteerAngle = (Int16)((data[6] << 8) + data[5]);
                            lblSteerAngle.Text = (actualSteerAngle * 0.01).ToString("N1");
                            lblWASCounts.Text = ((Int16)((data[8] << 8) + data[7])).ToString();

                            lblSwitchStatus.Text = ((data[9] & 2) == 2).ToString();
                            lblWorkSwitchStatus.Text = ((data[9] & 1) == 1).ToString();
                        }
                    }

                    else if (data[3] == 123 && data.Length == 11)
                    {

                        traffic.helloFromMachine = 0;

                        if (isViewAdvanced)
                        {
                            lblPingMachine.Text = (((DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds - pingSecondsStart) * 1000).ToString("N0");
                            lbl1To8.Text = Convert.ToString(data[5], 2).PadLeft(8, '0');
                            lbl9To16.Text = Convert.ToString(data[6], 2).PadLeft(8, '0');
                        }
                    }

                    else if (data[3] == 121 && data.Length == 11)
                        traffic.helloFromIMU = 0;

                    //scan Reply
                    else if (data[3] == 203 && data.Length == 13) //
                    {
                        if (data[2] == 126)  //steer module
                        {
                            scanReply.steerIP = data[5].ToString() + "." + data[6].ToString() + "." + data[7].ToString() + "." + data[8].ToString();

                            scanReply.subnet[0] = data[09];
                            scanReply.subnet[1] = data[10];
                            scanReply.subnet[2] = data[11];

                            scanReply.subnetStr = data[9].ToString() + "." + data[10].ToString() + "." + data[11].ToString();

                            scanReply.isNewData = true;
                            scanReply.isNewSteer = true;
                        }
                        //
                        else if (data[2] == 123)   //machine module
                        {
                            scanReply.machineIP = data[5].ToString() + "." + data[6].ToString() + "." + data[7].ToString() + "." + data[8].ToString();

                            scanReply.subnet[0] = data[09];
                            scanReply.subnet[1] = data[10];
                            scanReply.subnet[2] = data[11];

                            scanReply.subnetStr = data[9].ToString() + "." + data[10].ToString() + "." + data[11].ToString();

                            scanReply.isNewData = true;
                            scanReply.isNewMachine = true;

                        }
                        else if (data[2] == 121)   //IMU Module
                        {
                            scanReply.IMU_IP = data[5].ToString() + "." + data[6].ToString() + "." + data[7].ToString() + "." + data[8].ToString();

                            scanReply.subnet[0] = data[09];
                            scanReply.subnet[1] = data[10];
                            scanReply.subnet[2] = data[11];

                            scanReply.subnetStr = data[9].ToString() + "." + data[10].ToString() + "." + data[11].ToString();

                            scanReply.isNewData = true;
                            scanReply.isNewIMU = true;
                        }

                        else if (data[2] == 120)    //GPS module
                        {
                            scanReply.GPS_IP = data[5].ToString() + "." + data[6].ToString() + "." + data[7].ToString() + "." + data[8].ToString();

                            scanReply.subnet[0] = data[09];
                            scanReply.subnet[1] = data[10];
                            scanReply.subnet[2] = data[11];

                            scanReply.subnetStr = data[9].ToString() + "." + data[10].ToString() + "." + data[11].ToString();

                            scanReply.isNewData = true;
                            scanReply.isNewGPS = true;
                        }
                    }

                    if (isUDPMonitorOn)
                    {
                        logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + endPointUDP.ToString() + "\t" + " < " + data[3].ToString() + "\r\n");
                    }

                } // end of pgns

                else if (data[0] == 36 && (data[1] == 71 || data[1] == 80 || data[1] == 75))
                {
                    double timeSliceOfLastFix = (double)(swFrame.ElapsedTicks) / (double)System.Diagnostics.Stopwatch.Frequency;
                    swFrame.Restart();

                    //get Hz from timeslice
                    nowHz = 1 / timeSliceOfLastFix;
                    if (nowHz > 35) nowHz = 35;
                    if (nowHz < 5) nowHz = 5;

                    //simple comp filter
                    gpsHz = 0.98 * gpsHz + 0.02 * nowHz;

                    traffic.cntrGPSOut += data.Length;
                    pnGPS.rawBuffer += Encoding.ASCII.GetString(data);
                    pnGPS.ParseNMEA(ref pnGPS.rawBuffer);

                    if (isUDPMonitorOn && isGPSLogOn)
                    {
                        logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + System.Text.Encoding.ASCII.GetString(data));
                    }
                }
            }
            catch
            {
            }
        }

        #endregion


        #region Send LoopBack Tool

        public void SendToLoopBackMessageAOGTool(byte[] byteData)
        {
            SendDataToLoopBackTool(byteData, epAgOpenTool);
        }

        private void SendDataToLoopBackTool(byte[] byteData, IPEndPoint endPoint)
        {
            try
            {
                if (byteData.Length != 0)
                {
                    // Send packet to AgVR
                    loopBackSocketTool.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, endPoint,
                        new AsyncCallback(SendDataLoopAsyncTool), null);
                }
            }
            catch
            {
            }
        }

        public void SendDataLoopAsyncTool(IAsyncResult asyncResult)
        {
            try
            {
                loopBackSocketTool.EndSend(asyncResult);
            }
            catch
            {
            }
        }

        #endregion

        #region Receive LoopBack Tool

        private void ReceiveFromLoopBackTool(byte[] data)
        {
            //Send out to udp network
            SendUDPMessageTool(data, epModuleTool);
        }

        private void ReceiveDataLoopAsyncTool(IAsyncResult asyncResult)
        {
            try
            {
                // Receive all data
                int msgLen = loopBackSocketTool.EndReceiveFrom(asyncResult, ref endPointLoopBackTool);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(bufferTool, localMsg, msgLen);

                // Listen for more connections again...
                loopBackSocketTool.BeginReceiveFrom(bufferTool, 0, bufferTool.Length, SocketFlags.None, ref endPointLoopBackTool,
                    new AsyncCallback(ReceiveDataLoopAsyncTool), null);

                BeginInvoke((MethodInvoker)(() => ReceiveFromLoopBackTool(localMsg)));
            }
            catch
            {
            }
        }

        #endregion

        #region Send_Tool_UDP

        public void SendUDPMessageTool(byte[] byteData, IPEndPoint endPoint)
        {
            if (isUDPNetworkConnectedTool)
            {
                if (isUDPMonitorOn)
                {
                        logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + endPoint.ToString() + "\t" + " > " + byteData[3].ToString() + "\r\n");
                }

                try
                {
                    // Send packet to the zero
                    if (byteData.Length != 0)
                    {
                        UDPSocketTool.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None,
                           endPoint, new AsyncCallback(SendDataUDPAsyncTool), null);
                    }
                }
                catch (Exception)
                {
                    //WriteErrorLog("Sending UDP Message" + e.ToString());
                    //MessageBox.Show("Send Error: " + e.Message, "UDP Client", MessageBoxButtons.OK,
                    //MessageBoxIcon.Error);
                }
            }
        }

        private void SendDataUDPAsyncTool(IAsyncResult asyncResult)
        {
            try
            {
                UDPSocketTool.EndSend(asyncResult);
            }
            catch
            {
            }
        }

        #endregion

        #region Recv_UDP_Tool

        private void ReceiveDataUDPAsyncTool(IAsyncResult asyncResult)
        {
            try
            {
                // Receive all data
                int msgLen = UDPSocketTool.EndReceiveFrom(asyncResult, ref endPointUDPTool);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(bufferTool, localMsg, msgLen);

                // Listen for more connections again...
                UDPSocketTool.BeginReceiveFrom(bufferTool, 0, bufferTool.Length, SocketFlags.None, ref endPointUDPTool,
                    new AsyncCallback(ReceiveDataUDPAsyncTool), null);

                BeginInvoke((MethodInvoker)(() => ReceiveFromUDPTool(localMsg)));

            }
            catch
            {
            }
        }

        private void ReceiveFromUDPTool(byte[] data)
        {
            try
            {
                if (data[0] == 0x80 && data[1] == 0x81)
                {
                    //module return via udp sent to AgOpenGPS
                    SendToLoopBackMessageAOGTool(data);

                    //    check for Scan and Hello
                    if (data[3] == 226 && data.Length == 11)
                    {

                        traffic.helloFromAutoSteer = 0;
                        if (isViewAdvanced)
                        {
                            lblPingTool.Text = (((DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds - pingSecondsStart) ).ToString("N0");
                            //double actualSteerAngle = (Int16)((data[6] << 8) + data[5]);
                            //lblSteerAngle.Text = (actualSteerAngle * 0.01).ToString("N1");
                            //lblWASCounts.Text = ((Int16)((data[8] << 8) + data[7])).ToString();

                            //lblSwitchStatus.Text = ((data[9] & 2) == 2).ToString();
                            //lblWorkSwitchStatus.Text = ((data[9] & 1) == 1).ToString();
                        }
                    }

                    // scan Reply
                    else if (data[3] == 203 && data.Length == 13) //
                    {
                        if (data[2] == 226)  //steer module
                        {
                            //scanReplyTool.steerIP = data[5].ToString() + "." + data[6].ToString() + "." + data[7].ToString() + "." + data[8].ToString();

                            //scanReplyTool.subnet[0] = data[09];
                            //scanReplyTool.subnet[1] = data[10];
                            //scanReplyTool.subnet[2] = data[11];

                            //scanReplyTool.subnetStr = data[9].ToString() + "." + data[10].ToString() + "." + data[11].ToString();

                            //scanReplyTool.isNewData = true;
                            //scanReplyTool.isNewSteer = true;
                        }

                        else if (data[2] == 220)    //GPS module
                        {
                            scanReplyTool.GPS_IP = data[5].ToString() + "." + data[6].ToString() + "." + data[7].ToString() + "." + data[8].ToString();

                            scanReplyTool.subnet[0] = data[09];
                            scanReplyTool.subnet[1] = data[10];
                            scanReplyTool.subnet[2] = data[11];

                            scanReplyTool.subnetStr = data[9].ToString() + "." + data[10].ToString() + "." + data[11].ToString();

                            scanReplyTool.isNewData = true;
                            scanReplyTool.isNewGPS = true;
                        }
                    } 
                }// end of pgns

                //gps data
                else if (data[0] == 36 && (data[1] == 71 || data[1] == 80 || data[1] == 75))
                {
                    traffic.cntrGPSOutTool += data.Length;
                    pnGPSTool.rawBuffer += Encoding.ASCII.GetString(data);
                    pnGPSTool.ParseNMEA(ref pnGPSTool.rawBuffer);

                    //if (isUDPMonitorOn && isGPSLogOn)
                    //{
                    //    logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + System.Text.Encoding.ASCII.GetString(data));
                    //}
                }

            }
            catch
            {
            }
        }

        #endregion
    }
}
