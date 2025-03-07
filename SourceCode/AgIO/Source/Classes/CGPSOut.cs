using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgIO
{
    public static class GPSOut
    {
        public static byte[] nmeaPGN = new byte[57];

        static StringBuilder sbGGA = new StringBuilder();
        static StringBuilder sbVTG = new StringBuilder();
        static StringBuilder sbRMC = new StringBuilder();

        public static string strGGA = "";
        public static string strVTG = "";
        public static string strOGI = "";
        public static string strNDA = "";

        public static double latitude = 0;

        public static bool isValidGGA = false;
        public static bool isValidNDA = false;
        public static bool isValidOGI = false;


        //public static void BuildSentences()
        //{
        //    //GGA
        //    sbGGA.Clear();
        //    sbGGA.Append("$GPGGA,");
        //    sbGGA.Append(DateTime.Now.ToString("HHmmss"));
        //    sbGGA.Append(".000,");
        //    sbGGA.Append(pn.latitude.ToString("00.000000"));
        //    sbGGA.Append(",");
        //    sbGGA.Append(pn.longitude.ToString("000.000000"));
        //    sbGGA.Append(",1,04,");
        //    sbGGA.Append(pn.hdop.ToString("0.0"));
        //    sbGGA.Append(",");
        //    sbGGA.Append(pn.altitude.ToString("0.0"));
        //    sbGGA.Append(",M,0.0,M,,");
        //    sbGGA.Append(pn.age.ToString("0.0"));
        //    sbGGA.Append(",0000");
        //    sbGGA.Append("*");
        //    sbGGA.Append(CalcNMEAChecksum(sbGGA.ToString()));
        //    sbGGA.Append("\r\n");
        //    //VTG
        //    sbVTG.Clear();
        //    sbVTG.Append("$GPVTG,");
        //    sbVTG.Append(pn.headingTrue.ToString("0.0"));
        //    sbVTG.Append(",T,,M,");
        //    sbVTG.Append(pn.vtgSpeed.ToString("0.0"));
        //    sbVTG.Append(",N,,K");
        //    sbVTG.Append("*");
        //    sbVTG.Append(CalcNMEAChecksum(sbVTG.ToString()));
        //    sbVTG.Append("\r\n");
        //    //RMC
        //    sbRMC.Clear();
        //    sbRMC.Append("$GPRMC,");
        //    sbRMC.Append(DateTime.Now.ToString("HHmmss"));
        //    sbRMC.Append(".000,A,");
        //    sbRMC.Append(pn.latitude.ToString("00.000000"));
        //    sbRMC.Append(",N,");
        //    sbRMC.Append(pn.longitude.ToString("000.000000"));
        //    sbRMC.Append(",W,");
        //    sbRMC.Append(pn.vtgSpeed.ToString("0.0"));
        //    sbRMC.Append(",");
        //    sbRMC.Append(pn.headingTrue.ToString("0.0"));
        //    sbRMC.Append(",");
        //    sbRMC.Append(DateTime.Now.ToString("ddMMyy"));
        //    sbRMC.Append(",,,A");
        //    sbRMC.Append("*");
        //    sbRMC.Append(CalcNMEAChecksum(sbRMC.ToString()));
        //    sbRMC.Append("\r\n");
        //    //copy

        //public static void DecodeNMEA()
        //{
        //    double Lon = BitConverter.ToDouble(nmeaPGN, 5);
        //    double Lat = BitConverter.ToDouble(nmeaPGN, 13);

        //    if (Lon != double.MaxValue && Lat != double.MaxValue)
        //    {

        //        pn.longitude = Lon;
        //        pn.latitude = Lat;

        //        if (!isGPSPositionInitialized)
        //            pn.SetLocalMetersPerDegree(pn.latitude, pn.longitude);

        //        pn.ConvertWGS84ToLocal(Lat, Lon, out pn.fix.northing, out pn.fix.easting);

        //        //From dual antenna heading sentences
        //        float temp = BitConverter.ToSingle(nmeaPGN, 21);
        //        if (temp != float.MaxValue)
        //        {
        //            pn.headingTrueDual = temp + Settings.Vehicle.setGPS_dualHeadingOffset;
        //            if (pn.headingTrueDual >= 360) pn.headingTrueDual -= 360;
        //            else if (pn.headingTrueDual < 0) pn.headingTrueDual += 360;
        //        }

        //        //from single antenna sentences (VTG,RMC)
        //        pn.headingTrue = BitConverter.ToSingle(nmeaPGN, 25);

        //        //always save the speed.
        //        temp = BitConverter.ToSingle(nmeaPGN, 29);
        //        if (temp != float.MaxValue)
        //        {
        //            pn.vtgSpeed = temp;
        //        }

        //        //roll in degrees
        //        temp = BitConverter.ToSingle(nmeaPGN, 33);
        //        if (temp != float.MaxValue)
        //        {
        //            if (Settings.Vehicle.setIMU_invertRoll) temp *= -1;
        //            ahrs.imuRoll = temp - Settings.Vehicle.setIMU_rollZero;
        //        }
        //        if (temp == float.MinValue)
        //            ahrs.imuRoll = 0;

        //        //altitude in meters
        //        temp = BitConverter.ToSingle(nmeaPGN, 37);
        //        if (temp != float.MaxValue)
        //            pn.altitude = temp;

        //        ushort sats = BitConverter.ToUInt16(nmeaPGN, 41);
        //        if (sats != ushort.MaxValue)
        //            pn.satellitesTracked = sats;

        //        byte fix = data[43];
        //        if (fix != byte.MaxValue)
        //            pn.fixQuality = fix;

        //        ushort hdop = BitConverter.ToUInt16(nmeaPGN, 44);
        //        if (hdop != ushort.MaxValue)
        //            pn.hdop = hdop * 0.01;

        //        ushort age = BitConverter.ToUInt16(nmeaPGN, 46);
        //        if (age != ushort.MaxValue)
        //            pn.age = age * 0.01;

        //        ushort imuHead = BitConverter.ToUInt16(nmeaPGN, 48);
        //        if (imuHead != ushort.MaxValue)
        //        {
        //            ahrs.imuHeading = imuHead;
        //            ahrs.imuHeading *= 0.1;
        //        }

        //        short imuRol = BitConverter.ToInt16(nmeaPGN, 50);
        //        if (imuRol != short.MaxValue)
        //        {
        //            double rollK = imuRol;
        //            if (Settings.Vehicle.setIMU_invertRoll) rollK *= -0.1;
        //            else rollK *= 0.1;
        //            rollK -= Settings.Vehicle.setIMU_rollZero;
        //            ahrs.imuRoll = ahrs.imuRoll * Settings.Vehicle.setIMU_rollFilter + rollK * (1 - Settings.Vehicle.setIMU_rollFilter);
        //        }

        //        short imuPich = BitConverter.ToInt16(nmeaPGN, 52);
        //        if (imuPich != short.MaxValue)
        //        {
        //            ahrs.imuPitch = imuPich;
        //        }

        //        short imuYaw = BitConverter.ToInt16(nmeaPGN, 54);
        //        if (imuYaw != short.MaxValue)
        //        {
        //            ahrs.imuYawRate = imuYaw;
        //        }


        //    }

    }
}
