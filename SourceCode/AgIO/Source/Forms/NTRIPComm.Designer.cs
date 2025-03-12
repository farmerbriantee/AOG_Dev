using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Globalization;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;

// Declare the delegate prototype to send data back to the form
delegate void UpdateRTCM_Data(byte[] data);

namespace AgIO
{
    public partial class FormLoop
    {
        //for the NTRIP CLient counting
        private int ntripCounter = 10;

        private Socket clientSocket;                      // Server connection
        private byte[] casterRecBuffer = new byte[2800];    // Recieved data buffer

        //Send GGA back timer
        Timer tmr;

        private string GGASentence;

        public uint tripBytes = 0;
        private int NTRIP_Watchdog = 100;

        public bool isNTRIP_Connected = false;
        public bool isNTRIP_Starting = false;
        public bool isNTRIP_Connecting = false;
        public bool isNTRIP_Sending = false;
        public bool isRunGGAInterval = false;

        internal SerialPort spRadio = new SerialPort("Radio", 9600, Parity.None, 8, StopBits.One);

        //NTRIP metering
        Queue<byte> rawTrip = new Queue<byte>();

        //set up connection to Caster
        private void DoNTRIPSecondRoutine()
        {
            //count up the ntrip clock only if everything is alive
            if (Settings.User.setNTRIP_isOn || Settings.User.setRadio_isOn || Settings.User.setPass_isOn)
            {
                IncrementNTRIPWatchDog();
            }

            //Have we NTRIP connection
            if (Settings.User.setNTRIP_isOn && !isNTRIP_Connected && !isNTRIP_Connecting)
            {
                if (!isNTRIP_Starting && ntripCounter > 20)
                {
                    StartNTRIP();
                }
            }

            if ((Settings.User.setRadio_isOn || Settings.User.setPass_isOn) && !isNTRIP_Connected && !isNTRIP_Connecting)
            {
                if (!isNTRIP_Starting)
                {
                    StartNTRIP();
                }
            }

            if (isNTRIP_Connecting)
            {
                if (ntripCounter > 29)
                {
                    TimedMessageBox(1500, "Connection Problem", "Not Connecting To Caster");
                    ReconnectRequest();
                }
                if (clientSocket != null && clientSocket.Connected)
                {
                    SendAuthorization();
                }
            }

            if (Settings.User.setNTRIP_isOn || Settings.User.setRadio_isOn)
            {
                //pbarNtripMenu.Value = unchecked((byte)(tripBytes * 0.02));
                lblNTRIPBytes.Text = ((tripBytes >> 10)).ToString("###,###,### kb");

                //Bypass if sleeping
                if (focusSkipCounter != 0)
                {
                    //update byte counter and up counter
                    if (ntripCounter > 59) btnStartStopNtrip.Text = (ntripCounter >> 6) + " Min";
                    else if (ntripCounter < 60 && ntripCounter > 25) btnStartStopNtrip.Text = ntripCounter + " Secs";
                    else btnStartStopNtrip.Text = "In " + (Math.Abs(ntripCounter - 25)) + " secs";

                    //watchdog for Ntrip
                    if (isNTRIP_Connecting)
                    {
                        lblWatch.Text = "Authourizing";
                    }
                    else
                    {
                        if (Settings.User.setNTRIP_isOn && NTRIP_Watchdog > 10)
                        {
                            lblWatch.Text = "Waiting";
                        }
                        else
                        {
                            lblWatch.Text = "Listening";

                            if (Settings.User.setNTRIP_isOn)
                            {
                                lblWatch.Text += " NTRIP";
                            }
                            else if (Settings.User.setRadio_isOn)
                            {
                                lblWatch.Text += " Radio";
                            }
                        }
                    }

                    if (Settings.User.setNTRIP_sendGGAInterval > 0 && isNTRIP_Sending)
                    {
                        lblWatch.Text = "Send GGA";
                        isNTRIP_Sending = false;
                    }
                }
            }
            else if (Settings.User.setPass_isOn)
            {
                //pbarNtripMenu.Value = unchecked((byte)(tripBytes * 0.02));
                lblNTRIPBytes.Text = ((tripBytes >> 10)).ToString("###,###,### kb");

                //update byte counter and up counter
                if (ntripCounter > 59) btnStartStopNtrip.Text = (ntripCounter >> 6) + " Min";
                else if (ntripCounter < 60 && ntripCounter > 22) btnStartStopNtrip.Text = ntripCounter + " Secs";
                else btnStartStopNtrip.Text = "In " + (Math.Abs(ntripCounter - 22)) + " secs";
            }
        }

        public void ConfigureNTRIP()
        {
            lblWatch.Text = "Wait GPS";
            lblNTRIP_IP.Text = "";
            lblMount.Text = "";

            //start NTRIP if required
            if (Settings.User.setRadio_isOn || Settings.User.setPass_isOn)
            {
                // Immediatly connect radio
                ntripCounter = 20;
            }

            if (Settings.User.setNTRIP_isOn || Settings.User.setRadio_isOn || Settings.User.setPass_isOn)
            {
                btnStartStopNtrip.Visible = true;
                btnStartStopNtrip.Visible = true;
                lblWatch.Visible = true;
                lblNTRIPBytes.Visible = true;
                lblToGPS.Visible = true;
                lblMount.Visible = true;
                lblNTRIP_IP.Visible = true;
            }
            else
            {
                btnStartStopNtrip.Visible = false;
                btnStartStopNtrip.Visible = false;
                lblWatch.Visible = false;
                lblNTRIPBytes.Visible = false;
                lblToGPS.Visible = false;
                lblMount.Visible = false;
                lblNTRIP_IP.Visible = false;
            }

            btnStartStopNtrip.Text = "Off";
        }

        public void StartNTRIP()
        {
            if (Settings.User.setNTRIP_isOn)
            {
                //if we had a timer already, kill it
                if (tmr != null)
                {
                    tmr.Dispose();
                }

                //create new timer at fast rate to start
                if (Settings.User.setNTRIP_sendGGAInterval > 0)
                {
                    this.tmr = new System.Windows.Forms.Timer();
                    this.tmr.Interval = 5000;
                    this.tmr.Tick += new EventHandler(NTRIPtick);
                }

                try
                {
                    // Close the socket if it is still open
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        System.Threading.Thread.Sleep(100);
                        clientSocket.Close();
                    }

                    //NTRIP endpoint
                    epNtrip = new IPEndPoint(IPAddress.Parse(
                        Settings.User.etIP_SubnetOne.ToString() + "." +
                        Settings.User.etIP_SubnetTwo.ToString() + "." +
                        Settings.User.etIP_SubnetThree.ToString() + ".255"), Settings.User.setNTRIP_sendToUDPPort);

                    // Create the socket object
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    clientSocket.NoDelay = true;
                    clientSocket.Blocking = false;
                    clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(Settings.User.setNTRIP_casterIP), Settings.User.setNTRIP_casterPort), new AsyncCallback(OnConnect), null);

                    Log.EventWriter("NTRIP - IP: " + Settings.User.setNTRIP_casterIP.ToString() + ":" + Settings.User.setNTRIP_casterPort.ToString()
                        + " To Port: " + Settings.User.setNTRIP_sendToUDPPort.ToString() + " Mount: " + Settings.User.setNTRIP_mount);
                }
                catch (Exception ex)
                {
                    ReconnectRequest();
                    Log.EventWriter("Catch - > NTRIP Reconnect Request: " + ex.ToString());

                    return;
                }

                isNTRIP_Connecting = true;
                lblNTRIP_IP.Text = Settings.User.setNTRIP_casterIP;
                lblMount.Text = Settings.User.setNTRIP_mount;
            }
            else if (Settings.User.setRadio_isOn)
            {
                if (!string.IsNullOrEmpty(Settings.User.setPort_portNameRadio))
                {
                    // Disconnect when already connected
                    if (spRadio != null)
                    {
                        spRadio.Close();
                        spRadio.Dispose();
                    }

                    // Setup and open serial port
                    spRadio = new SerialPort(Settings.User.setPort_portNameRadio);
                    spRadio.BaudRate = int.Parse(Settings.User.setPort_baudRateRadio);
                    spRadio.DataReceived += NtripPort_DataReceived;
                    isNTRIP_Connecting = false;
                    isNTRIP_Connected = true;

                    try
                    {
                        spRadio.Open();
                    }
                    catch (Exception ex)
                    {
                        isNTRIP_Connecting = false;
                        isNTRIP_Connected = false;
                        Settings.User.setRadio_isOn = false;
                        Log.EventWriter("Catch - > Error connecting to radio" + ex.ToString());

                        TimedMessageBox(2000, "Error connecting to radio", $"{ex.Message}");
                    }
                }
            }
            else if (Settings.User.setPass_isOn)
            {
                epNtrip = new IPEndPoint(IPAddress.Parse(
                    Settings.User.etIP_SubnetOne.ToString() + "." +
                    Settings.User.etIP_SubnetTwo.ToString() + "." +
                    Settings.User.etIP_SubnetThree.ToString() + ".255"), Settings.User.setNTRIP_sendToUDPPort);

                if (!string.IsNullOrEmpty(Settings.User.setPort_portNameRadio))
                {
                    // Disconnect when already connected
                    if (spRadio != null)
                    {
                        spRadio.Close();
                        spRadio.Dispose();
                    }

                    // Setup and open serial port
                    spRadio = new SerialPort(Settings.User.setPort_portNameRadio);
                    spRadio.BaudRate = int.Parse(Settings.User.setPort_baudRateRadio);
                    spRadio.DataReceived += NtripPort_DataReceived;
                    isNTRIP_Connecting = false;
                    isNTRIP_Connected = true;
                    lblWatch.Text = "RTCM Serial";


                    try
                    {
                        spRadio.Open();
                    }
                    catch (Exception ex)
                    {
                        isNTRIP_Connecting = false;
                        isNTRIP_Connected = false;
                        Settings.User.setPass_isOn = false;
                        Log.EventWriter("Catch - > Serial Pass Radio: " + ex.ToString());

                        TimedMessageBox(2000, "Error connecting to Serial Pass", $"{ex.Message}");
                    }
                }
            }
        }

        private void ReconnectRequest()
        {
            //TimedMessageBox(2000, "NTRIP Not Connected", " Reconnect Request");
            ntripCounter = 15;
            isNTRIP_Connected = false;
            isNTRIP_Starting = false;
            isNTRIP_Connecting = false;

            //if we had a timer already, kill it
            if (tmr != null)
            {
                tmr.Dispose();
            }
        }

        private void IncrementNTRIPWatchDog()
        {
            //increment once every second
            ntripCounter++;

            //Thinks is connected but not receiving anything
            if (NTRIP_Watchdog++ > 30 && isNTRIP_Connected) 
                ReconnectRequest();

            //Once all connected set the timer GGA to NTRIP Settings
            if (Settings.User.setNTRIP_sendGGAInterval > 0 && ntripCounter == 40) tmr.Interval = Settings.User.setNTRIP_sendGGAInterval * 1000;
        }

        private void SendAuthorization()
        {
            // Check we are connected
            if (clientSocket == null || !clientSocket.Connected)
            {
                //TimedMessageBox(2000, gStr.gsNTRIPNotConnected, " At the StartNTRIP() ");
                ReconnectRequest();
                return;
            }

            // Read the message from settings and send it
            try
            {
                if (!Settings.User.setNTRIP_isTCP)
                {
                    //encode user and password
                    string auth = ToBase64(Settings.User.setNTRIP_userName + ":" + Settings.User.setNTRIP_userPassword);

                    //grab location sentence
                    BuildGGA();
                    GGASentence = sbGGA.ToString();

                    string htt;
                    if (Settings.User.setNTRIP_isHTTP10) htt = "1.0";
                    else htt = "1.1";

                    //Build authorization string
                    string str = "GET /" + Settings.User.setNTRIP_mount + " HTTP/" + htt + "\r\n";
                    str += "User-Agent: NTRIP AgOpenGPSClient/6.4\r\n";
                    str += "Authorization: Basic " + auth + "\r\n"; //This line can be removed if no authorization is needed
                                                                    //str += GGASentence; //this line can be removed if no position feedback is needed
                    str += "Accept: */*\r\nConnection: close\r\n";
                    str += "\r\n";

                    // Convert to byte array and send.
                    Byte[] byteDateLine = Encoding.ASCII.GetBytes(str.ToCharArray());
                    clientSocket.Send(byteDateLine, byteDateLine.Length, 0);

                    //enable to periodically send GGA sentence to server.
                    if (Settings.User.setNTRIP_sendGGAInterval > 0) tmr.Enabled = true;
                }
                //say its connected
                isNTRIP_Connected = true;
                isNTRIP_Starting = false;
                isNTRIP_Connecting = false;
            }
            catch (Exception ex)
            {
                ReconnectRequest();
                Log.EventWriter("Catch - > NTRIP Send Authourization: " + ex.ToString());
            }
        }

        public void OnAddMessage(byte[] data)
        {
            //update gui with stats
            tripBytes += (uint)data.Length;
            
            //reset watchdog since we have updated data
            NTRIP_Watchdog = 0;

            if (Settings.User.setNTRIP_isOn)
            {
                //move the ntrip stream to queue
                for (int i = 0; i < data.Length; i++)
                {
                    rawTrip.Enqueue(data[i]);
                }

                ntripMeterTimer.Enabled = true;
            }
            else
            {
                lblToGPS.Text = data.Length.ToString();
                //send it
                SendNTRIP(data);
            }
        }

        private void ntripMeterTimer_Tick(object sender, EventArgs e)
        {
            //we really should get here, but have to check
            if (rawTrip.Count == 0) return;

            //how many bytes in the Queue
            int cnt = rawTrip.Count;

            //how many sends have occured
            traffic.cntrGPSIn++;

            //128 bytes chunks max
            if (cnt > Settings.User.setNTRIP_packetSize) cnt = Settings.User.setNTRIP_packetSize;

            //new data array to send
            byte[] trip = new byte[cnt];

            traffic.cntrGPSInBytes += cnt;

            //dequeue into the array
            for (int i = 0; i < cnt; i++) trip[i] = rawTrip.Dequeue();

            //send it
            SendNTRIP(trip);

            //Are we done?
            if (rawTrip.Count == 0)
            {
                ntripMeterTimer.Enabled = false;

                if (focusSkipCounter != 0)
                {
                    lblToGPS.Text = traffic.cntrGPSInBytes == 0 ? "---" : (traffic.cntrGPSInBytes).ToString();
                    traffic.cntrGPSInBytes = 0;
                }
            }

            //Can't keep up as internet dumped a shit load so clear
            if (rawTrip.Count > 10000) rawTrip.Clear();

            ////show how many bytes left in the queue
            if (isViewAdvanced)
                lblCount.Text = rawTrip.Count.ToString();
        }

        public void SendNTRIP(byte[] data)
        {
            //serial send out GPS port
            if (Settings.User.setNTRIP_sendToSerial)
            {
                SendGPSPort(data);
            }

            //send out UDP Port
            if (Settings.User.setNTRIP_sendToUDP)
            {
                SendUDPMessage(data, epNtrip);
            }
        }

        public void SendGGA()
        {
            //timer may have brought us here so return if not connected
            if (!isNTRIP_Connected)
                return;
            // Check we are connected
            if (clientSocket == null || !clientSocket.Connected)
            {
                ReconnectRequest();
                return;
            }

            // Read the message from the text box and send it
            try
            {
                isNTRIP_Sending = true;
                BuildGGA();
                string str = sbGGA.ToString();

                Byte[] byteDateLine = Encoding.ASCII.GetBytes(str.ToCharArray());
                clientSocket.Send(byteDateLine, byteDateLine.Length, 0);
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch - > Send GGA" + ex.ToString());

                ReconnectRequest();
            }
        }
        private void NTRIPtick(object o, EventArgs e)
        {
            SendGGA();
        }

        public void OnConnect(IAsyncResult ar)
        {
            // Check if we were sucessfull
            try
            {
                if (clientSocket.Connected)
                    clientSocket.BeginReceive(casterRecBuffer, 0, casterRecBuffer.Length, SocketFlags.None, new AsyncCallback(OnRecievedData), null);
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message, "Unusual error during Connect!");
            }
        }

        public void OnRecievedData(IAsyncResult ar)
        {
            // Check if we got any data
            try
            {
                int nBytesRec = clientSocket.EndReceive(ar);
                if (nBytesRec > 0)
                {
                    byte[] localMsg = new byte[nBytesRec];
                    Array.Copy(casterRecBuffer, localMsg, nBytesRec);

                    BeginInvoke((MethodInvoker)(() => OnAddMessage(localMsg)));
                    clientSocket.BeginReceive(casterRecBuffer, 0, casterRecBuffer.Length, SocketFlags.None, new AsyncCallback(OnRecievedData), null);
                }
                else
                {
                    // If no data was recieved then the connection is probably dead
                    Console.WriteLine("Client {0}, disconnected", clientSocket.RemoteEndPoint);
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show( this, ex.Message, "Unusual error druing Recieve!" );
            }
        }

        private void NtripPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Check if we got any data
            try
            {
                SerialPort comport = (SerialPort)sender;
                if (comport.BytesToRead < 32) 
                    return;

                int nBytesRec = comport.BytesToRead;

                if (nBytesRec > 0)
                {
                    byte[] localMsg = new byte[nBytesRec];
                    comport.Read(localMsg, 0, nBytesRec);

                    BeginInvoke((MethodInvoker)(() => OnAddMessage(localMsg)));
                }
                else
                {
                    // If no data was recieved then the connection is probably dead
                    // TODO: What can we do?
                }
            }
            catch (Exception)
            {
                //MessageBox.Show( this, ex.Message, "Unusual error druing Recieve!" );
            }
        }

        private string ToBase64(string str)
        {
            Encoding asciiEncoding = Encoding.ASCII;
            byte[] byteArray = new byte[asciiEncoding.GetByteCount(str)];
            byteArray = asciiEncoding.GetBytes(str);
            return Convert.ToBase64String(byteArray, 0, byteArray.Length);
        }

        private void ShutDownNTRIP()
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                //shut it down
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                System.Threading.Thread.Sleep(500);

                //start it up again
                ReconnectRequest();

                //Also stop the requests now
                Settings.User.setNTRIP_isOn = false;
            }
            else if(spRadio != null)
            {
                spRadio.Close();
                spRadio.Dispose();
                spRadio = null;

                ReconnectRequest();

                //Also stop the requests now
                Settings.User.setRadio_isOn = false;
            }
        }

        private void SettingsShutDownNTRIP()
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                System.Threading.Thread.Sleep(500);
                ReconnectRequest();
            }

            if (spRadio != null && spRadio.IsOpen)
            {
                spRadio.Close();
                spRadio.Dispose();
                spRadio = null;
                ReconnectRequest();
            }
        }

        //calculate the NMEA checksum to stuff at the end
        public string CalculateChecksum(string Sentence)
        {
            int sum = 0, inx;
            char[] sentence_chars = Sentence.ToCharArray();
            char tmp;

            // All character xor:ed results in the trailing hex checksum
            // The checksum calc starts after '$' and ends before '*'
            for (inx = 1; ; inx++)
            {
                tmp = sentence_chars[inx];

                // Indicates end of data and start of checksum
                if (tmp == '*')
                    break;
                sum ^= tmp;    // Build checksum
            }

            // Calculated checksum converted to a 2 digit hex string
            return String.Format("{0:X2}", sum);
        }

        private readonly StringBuilder sbGGA = new StringBuilder();

        private void BuildGGA()
        {
            double latitude = 0;
            double longitude = 0;

            if (Settings.User.setNTRIP_isGGAManual)
            {
                latitude = Settings.User.setNTRIP_manualLat;
                longitude = Settings.User.setNTRIP_manualLon;
            }
            else
            {
                latitude = this.pnGPS.latitude;
                longitude = this.pnGPS.longitude;
            }

            //convert to DMS from Degrees
            double latMinu = latitude;
            double longMinu = longitude;

            double latDeg = (int)latitude;
            double longDeg = (int)longitude;

            latMinu -= latDeg;
            longMinu -= longDeg;

            latMinu = Math.Round(latMinu * 60.0, 7);
            longMinu = Math.Round(longMinu * 60.0, 7);

            latDeg *= 100.0;
            longDeg *= 100.0;

            double latNMEA = latMinu + latDeg;
            double longNMEA = longMinu + longDeg;

            char NS = 'W';
            char EW = 'N';
            if (latitude >= 0) NS = 'N';
            else NS = 'S';
            if (longitude >= 0) EW = 'E';
            else EW = 'W';

            sbGGA.Clear();
            sbGGA.Append("$GPGGA,");
            sbGGA.Append(DateTime.Now.ToString("HHmmss.00,", CultureInfo.InvariantCulture));
            sbGGA.Append(Math.Abs(latNMEA).ToString("0000.000", CultureInfo.InvariantCulture)).Append(',').Append(NS).Append(',');
            sbGGA.Append(Math.Abs(longNMEA).ToString("00000.000", CultureInfo.InvariantCulture)).Append(',').Append(EW);
            sbGGA.Append(',').Append(pnGPS.fixQualityData.ToString()).Append(',');
            sbGGA.Append(pnGPS.satellitesData.ToString()).Append(',');

            if (pnGPS.hdopData > 0) sbGGA.Append(pnGPS.hdopData.ToString("0.##", CultureInfo.InvariantCulture)).Append(',');

            else sbGGA.Append("1,");

            sbGGA.Append(pnGPS.altitudeData.ToString("0.###", CultureInfo.InvariantCulture)).Append(',');
            sbGGA.Append("M,");
            sbGGA.Append("46.4,M,");  //udulation
            sbGGA.Append(pnGPS.ageData.ToString("0.#", CultureInfo.InvariantCulture)).Append(','); //age
            sbGGA.Append("0*");

            sbGGA.Append(CalculateChecksum(sbGGA.ToString()));
            sbGGA.Append("\r\n");
            /*
        $GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,5,0*47
           0     1      2      3    4      5 6  7  8   9    10 11  12 13  14
                Time      Lat       Lon     FixSatsOP Alt */
        }
    }
}