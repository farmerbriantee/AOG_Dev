using System;
using System.Collections.Generic;

namespace AOG.Classes
{
    // Local representation of a downloaded field after geo-conversion
    public class LocalFieldModel
    {
        public Guid FieldId;
        public string Name;
        public Wgs84 Origin; // StartFix in lat/lon
        public List<List<LocalPoint>> Boundaries; // Outer and holes in local plane
        public List<AbLineLocal> AbLines; // Tracks in local plane with heading
    }

    // Point in local plane (easting/northing)
    public struct LocalPoint
    {
        public double Easting;
        public double Northing;
        public double Heading; // Nieuw toegevoegd veld

        public LocalPoint(double e, double n, double heading = 0)
        {
            Easting = e;
            Northing = n;
            Heading = heading;
        }
    }


    // AB Line in local plane
    public class AbLineLocal
    {
        public string Name;
        public LocalPoint PtA;
        public LocalPoint PtB;
        public double Heading;
        public List<LocalPoint> CurvePoints; // Optional, only filled if Curve
    }

    // Simple WGS84 lat/lon
    public struct Wgs84
    {
        public double Lat;
        public double Lon;

        public Wgs84(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }
    }
}
