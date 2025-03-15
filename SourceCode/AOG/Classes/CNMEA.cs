using System;
using System.Globalization;

namespace AOG
{
    public class CNMEA
    {
        //WGS84 Lat Long
        public double latitude, longitude;

        //local plane geometry
        public static double latStart, lonStart;

        public static double mPerDegreeLat;

        //our current fix
        public vec2 fix = new vec2(0, 0);

        //used to offset the antenna position to compensate for drift
        public vec2 fixOffset = new vec2(0, 0);

        //other GIS Info
        public double altitude, vtgSpeed = 0;

        public double headingTrueDual, headingTrue, hdop, age;

        public int fixQuality;
        public int satellitesTracked;

        private readonly FormGPS mf;

        public CNMEA(FormGPS f)
        {
            //constructor, grab the main form reference
            mf = f;
            latStart = 0;
            lonStart = 0;
        }

        public void AverageTheSpeed()
        {
            //average the speed
            //if (speed > 70) speed = 70;
            mf.avgSpeed = (mf.avgSpeed * 0.75) + (vtgSpeed * 0.25);
        }

        public void SetLocalMetersPerDegree(double lat, double lon)
        {
            mf.isGPSPositionInitialized = true;
            latStart = lat;
            lonStart = lon;

            Settings.Vehicle.setGPS_SimLatitude = lat;//this is actuallly the last field position
            Settings.Vehicle.setGPS_SimLongitude = lon;
            
            mPerDegreeLat = 111132.92 - 559.82 * Math.Cos(2.0 * latStart * 0.01745329251994329576923690766743) + 1.175
            * Math.Cos(4.0 * latStart * 0.01745329251994329576923690766743) - 0.0023
            * Math.Cos(6.0 * latStart * 0.01745329251994329576923690766743);

            mf.stepFixPts[0].isSet = false;
            mf.sim.Reset();

            mf.FileLoadFields();
        }

        public void ConvertWGS84ToLocal(double Lat, double Lon, out double Northing, out double Easting)
        {
            var rad = Lat * 0.01745329251994329576923690766743;
            double mPerDegreeLon = 111412.84 * Math.Cos(rad) - 93.5 * Math.Cos(3.0 * rad) + 0.118 * Math.Cos(5.0 * rad);

            Northing = (Lat - latStart) * mPerDegreeLat;
            Easting = (Lon - lonStart) * mPerDegreeLon;

            //Northing += mf.RandomNumber(-0.02, 0.02);
            //Easting += mf.RandomNumber(-0.02, 0.02);
        }

        public void ConvertLocalToWGS84(double Northing, double Easting, out double Lat, out double Lon)
        {
            Lat = ((Northing + fixOffset.northing) / mPerDegreeLat) + latStart;
            var rad = Lat * 0.01745329251994329576923690766743;
            double mPerDegreeLon = 111412.84 * Math.Cos(rad) - 93.5 * Math.Cos(3.0 * rad) + 0.118 * Math.Cos(5.0 * rad);
            Lon = ((Easting + fixOffset.easting) / mPerDegreeLon) + lonStart;
        }

        public string GetLocalToWSG84_KML(double Easting, double Northing)
        {
            double Lat = (Northing / mPerDegreeLat) + latStart;
            var rad = Lat * 0.01745329251994329576923690766743;
            double mPerDegreeLon = 111412.84 * Math.Cos(rad) - 93.5 * Math.Cos(3.0 * rad) + 0.118 * Math.Cos(5.0 * rad);
            double Lon = (Easting / mPerDegreeLon) + lonStart;

            return Lon.ToString("N7", CultureInfo.InvariantCulture) + ',' + Lat.ToString("N7", CultureInfo.InvariantCulture) + ",0 ";
        }

        public void GetLocalToLocal(double Easting, double Northing, double mPerDegreeLat2, double latStart2, double lonStart2, out double Northing2, out double Easting2)
        {
            double Lat = (Northing / mPerDegreeLat2) + latStart2;
            var rad = Lat * 0.01745329251994329576923690766743;
            double mPerDegreeLon = 111412.84 * Math.Cos(rad) - 93.5 * Math.Cos(3.0 * rad) + 0.118 * Math.Cos(5.0 * rad);
            double Lon = (Easting / mPerDegreeLon) + lonStart2;

            Northing2 = (Lat - latStart) * mPerDegreeLat;
            Easting2 = (Lon - lonStart) * mPerDegreeLon;
        }
    }
}