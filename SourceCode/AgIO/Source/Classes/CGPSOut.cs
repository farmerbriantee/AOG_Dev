using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgIO
{
    public static class GPSOut
    {

        public static byte[] nmeaPGN = new byte[57];

        static StringBuilder sbGGA = new StringBuilder();
        static StringBuilder sbVTG = new StringBuilder();
        static StringBuilder sbRMC = new StringBuilder();

        public static int counterGGA = 10, counterVTG = 10, counterRMC = 10, counterStatus = 0;


        //GPS related properties
        static int fixQuality = 8, satellitesTracked = 12;
        static double HDOP = 0.9, AGE = 1;
        static double altitude = 300;
        static char EW = 'W';
        static char NS = 'N';
        static double latDeg, latMinu, longDeg, longMinu, latNMEA, longNMEA;
        static double speed = 0.6, headingTrue;

        public static void BuildSentences()
        {
            try
            {
                double latitude = BitConverter.ToDouble(nmeaPGN, 5);
                double longitude = BitConverter.ToDouble(nmeaPGN, 13);

                if (longitude != double.MaxValue && latitude != double.MaxValue)
                {

                    //convert to DMS from Degrees
                    latMinu = latitude;
                    longMinu = longitude;

                    latDeg = (int)latitude;
                    longDeg = (int)longitude;

                    latMinu -= latDeg;
                    longMinu -= longDeg;

                    latMinu = Math.Round(latMinu * 60.0, 7);
                    longMinu = Math.Round(longMinu * 60.0, 7);

                    latDeg *= 100.0;
                    longDeg *= 100.0;

                    latNMEA = latMinu + latDeg;
                    longNMEA = longMinu + longDeg;

                    if (latitude >= 0) NS = 'N';
                    else NS = 'S';
                    if (longitude >= 0) EW = 'E';
                    else EW = 'W';

                    //From dual antenna heading sentences
                    float temp = BitConverter.ToSingle(nmeaPGN, 21);
                    if (temp != float.MaxValue)
                    {
                        headingTrue = temp;
                        if (headingTrue >= 360) headingTrue -= 360;
                        else if (headingTrue < 0) headingTrue += 360;
                    }

                    //from single antenna sentences (VTG,RMC)
                    if (temp != float.MaxValue)
                        headingTrue = BitConverter.ToSingle(nmeaPGN, 25);

                    //always save the speed.
                    temp = BitConverter.ToSingle(nmeaPGN, 29);
                    if (temp != float.MaxValue)
                    {
                        speed = temp;
                    }

                    //altitude in meters
                    temp = BitConverter.ToSingle(nmeaPGN, 37);
                    if (temp != float.MaxValue)
                        altitude = temp;

                    ushort sats = BitConverter.ToUInt16(nmeaPGN, 41);
                    if (sats != ushort.MaxValue)
                        satellitesTracked = sats;

                    byte fix = nmeaPGN[43];
                    if (fix != byte.MaxValue)
                        fixQuality = fix;

                    ushort hdop = BitConverter.ToUInt16(nmeaPGN, 44);
                    if (hdop != ushort.MaxValue)
                        HDOP = (double)hdop * 0.01;

                    ushort age = BitConverter.ToUInt16(nmeaPGN, 46);
                    if (age != ushort.MaxValue)
                        AGE = (double)age * 0.01;
                }
                else
                {
                    return;
                }

                /*    //GGA
                //$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M ,  ,*47
                //   0     1      2      3    4      5 6  7  8   9    10 11  12 13  14
                //        Time Lat       Lon FixSatsOP Alt
                //Where:
                //GGA Global Positioning System Fix Data
                // 123519       Fix taken at 12:35:19 UTC
                // 4807.038,N Latitude 48 deg 07.038' N
                // 01131.000,E Longitude 11 deg 31.000' E
                // 1            Fix quality: 0 = invalid
                //                           1 = GPS fix(SPS)
                //                           2 = DGPS fix
                //                           3 = PPS fix
                //                           4 = Real Time Kinematic
                //                           5 = Float RTK
                //                           6 = estimated(dead reckoning) (2.3 feature)
                //                           7 = Manual input mode
                //                           8 = Simulation mode
                // 08           Number of satellites being tracked
                // 0.9          Horizontal dilution of position
                // 545.4, M      Altitude, Meters, above mean sea level
                // 46.9, M       Height of geoid(mean sea level) above WGS84
                //                  ellipsoid
                // (empty field) time in seconds since last DGPS update
                // (empty field) DGPS station ID number
                // *47          the checksum data, always begins with*
                */

                if (Settings.User.sendRateGGA != 0)
                {
                    counterGGA--;
                    if (counterGGA < 1)
                    {
                        sbGGA.Clear();
                        sbGGA.Append("$").Append(Settings.User.sendPrefixGPGN).Append("GGA,");
                        sbGGA.Append(DateTime.Now.ToString("HHmmss.ss"));
                        sbGGA.Append(".000,");

                        sbGGA.Append(Math.Abs(latNMEA).ToString("0000.0000000", CultureInfo.InvariantCulture))
                            .Append(',').Append(NS).Append(',');
                        sbGGA.Append(Math.Abs(longNMEA).ToString("00000.0000000", CultureInfo.InvariantCulture))
                            .Append(',').Append(EW).Append(',');

                        sbGGA.Append(fixQuality.ToString(CultureInfo.InvariantCulture)).Append(',')
                            .Append(satellitesTracked.ToString(CultureInfo.InvariantCulture)).Append(',')
                            .Append(HDOP.ToString(CultureInfo.InvariantCulture)).Append(',')
                            .Append(altitude.ToString(CultureInfo.InvariantCulture)).Append(',');

                        sbGGA.Append("M,46.9,M,");
                        sbGGA.Append(AGE.ToString(CultureInfo.InvariantCulture)).Append(",37,,*");

                        sbGGA.Append(CalculateChecksum(sbGGA.ToString()));
                        sbGGA.Append("\r\n");

                        if (FormLoop.spGPSOut.IsOpen)
                        {
                            FormLoop.spGPSOut.WriteLine(sbGGA.ToString());
                        }

                        counterGGA = Settings.User.sendRateGGA;
                    }
                }

                /* / $GPVTG,054.7,T,034.4,M,005.5,N,010.2,K*48
                //
                //   VTG          Track made good and ground speed
                //   054.7,T True track made good(degrees)
                //   034.4,M Magnetic track made good
                //   005.5,N Ground speed, knots
                //   010.2,K Ground speed, Kilometers per hour
                //   *48          Checksum
                */

                if (Settings.User.sendRateVTG != 0)
                {
                    counterVTG--;

                    if (counterVTG < 1)
                    {
                        sbVTG.Clear();
                        sbVTG.Append("$").Append(Settings.User.sendPrefixGPGN).Append("VTG,");
                        sbVTG.Append(headingTrue.ToString("N5", CultureInfo.InvariantCulture));
                        sbVTG.Append(",T,034.4,M,");
                        sbVTG.Append(speed.ToString(CultureInfo.InvariantCulture));
                        sbVTG.Append(",N,");
                        sbVTG.Append(Math.Round((speed * 1.852), 1).ToString(CultureInfo.InvariantCulture));
                        sbVTG.Append(",K*");
                        sbVTG.Append(CalculateChecksum(sbVTG.ToString()));
                        sbVTG.Append("\r\n");

                        if (FormLoop.spGPSOut.IsOpen)
                        {
                            FormLoop.spGPSOut.WriteLine(sbVTG.ToString());
                        }

                        counterVTG = Settings.User.sendRateVTG;
                    }
                }

                #region RMC Message

                /* /$GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W*6A

                //RMC          Recommended Minimum sentence C
                //123519       Fix taken at 12:35:19 UTC
                //A            Status A=active or V=Void.
                //4807.038,N   Latitude 48 deg 07.038' N
                //01131.000,E  Longitude 11 deg 31.000' E
                //022.4        Speed over the ground in knots
                //084.4        Track angle in degrees True
                //230394       Date - 23rd of March 1994
                //003.1,W      Magnetic Variation
                //*6A          * Checksum
                */

                if (Settings.User.sendRateRMC != 0)
                {
                    counterRMC--;

                    if (counterRMC < 1)
                    {
                        sbRMC.Clear();
                        sbRMC.Append("$").Append(Settings.User.sendPrefixGPGN).Append("RMC,");

                        sbRMC.Append(DateTime.Now.ToString("HHmmss"));
                        sbRMC.Append(".000,");

                        sbRMC.Append(Math.Abs(latNMEA).ToString("0000.0000000", CultureInfo.InvariantCulture)).Append(',').Append(NS).Append(',')
                        .Append(Math.Abs(longNMEA).ToString("0000.0000000", CultureInfo.InvariantCulture)).Append(',').Append(EW).Append(',');

                        sbRMC.Append((speed).ToString(CultureInfo.InvariantCulture)).Append(',')
                        .Append(headingTrue.ToString("N5", CultureInfo.InvariantCulture))

                        .Append(",230394,1.0,W,D*");

                        sbRMC.Append(CalculateChecksum(sbRMC.ToString()));
                        sbRMC.Append("\r\n");

                        if (FormLoop.spGPSOut.IsOpen)
                        {
                            FormLoop.spGPSOut.WriteLine(sbRMC.ToString());
                        }

                        counterRMC = Settings.User.sendRateRMC;
                    }
                }

                #endregion RMC Message
            }

            catch
            {
            }
        }

        static string CalculateChecksum(string Sentence)
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
    }
}
