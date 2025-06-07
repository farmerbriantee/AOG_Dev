using System;
using System.Collections.Generic;
using System.Diagnostics;
using AOG.Classes;

namespace AOG.Logic
{
    public static class AgShareFieldParser
    {
        public static LocalFieldModel Parse(AgShareFieldDto dto)
        {
            var result = new LocalFieldModel
            {
                FieldId = dto.Id,
                Name = dto.Name,
                Origin = new Wgs84(dto.Latitude, dto.Longitude),
                Boundaries = new List<List<LocalPoint>>(),
                AbLines = new List<AbLineLocal>()
            };

            var converter = new GeoConverter(dto.Latitude, dto.Longitude);

            // Boundaries (outer + holes)
            foreach (var ring in dto.Boundaries)
            {
                var ringList = new List<LocalPoint>();
                foreach (var point in ring)
                {
                    var local = converter.ToLocal(point.Latitude, point.Longitude);
                    ringList.Add(new LocalPoint(local.Easting, local.Northing));
                }
                result.Boundaries.Add(ringList);
            }

            // AB-lines & curves
            foreach (var ab in dto.AbLines)
            {
                if (ab.Coords == null || ab.Coords.Count < 2) continue;

                var vA = converter.ToLocal(ab.Coords[0].Latitude, ab.Coords[0].Longitude);
                var vB = converter.ToLocal(ab.Coords[1].Latitude, ab.Coords[1].Longitude);
                double heading = GeoConverter.HeadingFromPoints(vA, vB);

                var abLine = new AbLineLocal
                {
                    Name = ab.Name ?? "Unnamed",
                    Heading = heading,
                    PtA = new LocalPoint(vA.Easting, vA.Northing),
                    PtB = new LocalPoint(vB.Easting, vB.Northing),
                    CurvePoints = new List<LocalPoint>()
                };

                if (ab.Coords.Count > 2)
                {
                    var rawCurve = new List<vec3>();
                    for (int i = 2; i < ab.Coords.Count; i++)
                    {
                        var p = ab.Coords[i];
                        var local = converter.ToLocal(p.Latitude, p.Longitude);
                        rawCurve.Add(new vec3(local.Easting, local.Northing));
                    }

                    var computed = CurveHelper.CalculateHeadings(rawCurve);
                    foreach (var pt in computed)
                    {
                        abLine.CurvePoints.Add(new LocalPoint(pt.easting, pt.northing, pt.heading));
                    }
                }

                Debug.WriteLine($"Parsed AB '{ab.Name}', type: {(ab.Coords.Count > 2 ? "Curve" : "AB")}, points: {ab.Coords.Count}");
                result.AbLines.Add(abLine);
            }

            return result;
        }
    }
}
