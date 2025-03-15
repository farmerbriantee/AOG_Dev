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
        static StringBuilder sbZDA = new StringBuilder();
        static StringBuilder sbGSA = new StringBuilder();

        public static int counterGGA = 10, counterVTG = 10, counterRMC = 10, counterZDA, counterGSA;

        public static int trafficBytes = 0;

        //GPS related properties
        static int fixQuality = 8, satellitesTracked = 12;
        static double HDOP = 0.9, AGE = 1;
        static double altitude = 300;
        static char EW = 'W';
        static char NS = 'N';
        static double latDeg, latMinu, longDeg, longMinu, latNMEA, longNMEA;
        static double speed = 0.6, headingTrue;

        public static int BuildSentences(int gpsRate)
        {
            try
            {
                int retCount = 0;

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
                    return 0;
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
                        sbGGA.Append(Settings.User.sendPrefixGPGN).Append("GGA,");
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
                            FormLoop.spGPSOut.Write(sbGGA.ToString());
                        }

                        counterGGA = Settings.User.sendRateGGA * gpsRate;
                        retCount += sbGGA.Length;
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
                        sbVTG.Append(Settings.User.sendPrefixGPGN).Append("VTG,");
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
                            FormLoop.spGPSOut.Write(sbVTG.ToString());
                        }

                        counterVTG = Settings.User.sendRateVTG * gpsRate;
                        retCount += sbVTG.Length;
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
                        sbRMC.Append(Settings.User.sendPrefixGPGN).Append("RMC,");

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
                            FormLoop.spGPSOut.Write(sbRMC.ToString());
                        }

                        counterRMC = Settings.User.sendRateRMC * gpsRate;
                        retCount += sbRMC.Length;
                    }
                }

                #endregion RMC Message


                #region ZDA Message

                /*/$GPZDA,160012.71,11,03,2004,-01,00*7D

                //ZDA       time and date
                //160012    hhmmss.ss
                //11        day, 01 to 31
                //03        month, 01 to 12
                //2004      year, 4 digits
                //-01       local time zone description, 00 to +-13 hours
                //00        local time zone description, 00 to 59, same sign as local hours
                //*7D       checksum */

                if (Settings.User.sendRateZDA != 0)
                {
                    counterZDA--;

                    if (counterZDA < 1)
                    {
                        sbZDA.Clear();
                        sbZDA.Append(Settings.User.sendPrefixGPGN).Append("ZDA,");

                        DateTime daat = DateTime.UtcNow;

                        sbZDA.Append(daat.ToString("HHmmss.fff", CultureInfo.InvariantCulture)).Append(",");
                        sbZDA.Append(daat.Day.ToString("00")).Append(",");
                        sbZDA.Append(daat.Month.ToString("00")).Append(",");
                        sbZDA.Append(daat.Year.ToString("0000")).Append(",");

                        var offset = TimeZoneInfo.Local.GetUtcOffset(daat);
                        sbZDA.Append(offset.Hours.ToString("00")).Append(",");
                        sbZDA.Append(offset.Minutes.ToString("00")).Append("*");

                        sbZDA.Append(CalculateChecksum(sbZDA.ToString()));
                        sbZDA.Append("\r\n");

                        if (FormLoop.spGPSOut.IsOpen)
                        {
                            FormLoop.spGPSOut.Write(sbZDA.ToString());
                        }

                        counterZDA = Settings.User.sendRateZDA * gpsRate;
                        retCount += sbZDA.Length;
                    }
                }


                #endregion ZDA Message

                #region GSA Message

                if (Settings.User.sendRateGSA != 0)
                {
                    counterGSA--;

                    if (counterGSA < 1)
                    {
                        sbGSA.Clear();
                        sbGSA.Append(Settings.User.sendPrefixGPGN).Append("GSA,");

                        sbGSA.Append("GSA,A,3,01,02,03,,,,,,,,,,2,");
                        sbGSA.Append(HDOP.ToString("N2", CultureInfo.InvariantCulture));
                        sbGSA.Append(",2*");

                        sbGSA.Append(CalculateChecksum(sbGSA.ToString()));
                        sbGSA.Append("\r\n");

                        if (FormLoop.spGPSOut.IsOpen)
                        {
                            FormLoop.spGPSOut.Write(sbGSA.ToString());
                        }

                        counterGSA = Settings.User.sendRateGSA * gpsRate;
                        retCount += sbGSA.Length;
                    }
                }

                #endregion GSA

                return retCount;
            }

            catch (System.IO.IOException)
            {
                if (FormLoop.spGPSOut.IsOpen)
                {
                    FormLoop.spGPSOut.DiscardOutBuffer();
                }
                return -1;
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
