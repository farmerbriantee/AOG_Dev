using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using AOG.Classes;

namespace AOG.Logic
{
    public static class AgShareFieldParser
    {
        public static LocalFieldModel Parse(FieldDownloadDto dto)
        {
            var result = new LocalFieldModel
            {
                FieldId = dto.Id,
                Name = dto.Name,
                Origin = new Wgs84(dto.OriginLat, dto.OriginLon),
                Boundaries = new List<List<LocalPoint>>(),
                AbLines = new List<AbLineLocal>()
            };

            var converter = new GeoConverter(dto.OriginLat, dto.OriginLon);

            // Parse GeoJSON boundary
            if (!string.IsNullOrWhiteSpace(dto.BoundaryGeoJson))
            {
                var geoJson = JObject.Parse(dto.BoundaryGeoJson);
                var coordsArray = geoJson["coordinates"] as JArray;

                if (geoJson["type"]?.ToString() == "Polygon" && coordsArray != null)
                {
                    foreach (var ring in coordsArray)
                    {
                        var ringList = new List<LocalPoint>();
                        foreach (var point in ring)
                        {
                            double lon = point[0].Value<double>();
                            double lat = point[1].Value<double>();
                            var vec = converter.ToLocal(lat, lon);
                            ringList.Add(new LocalPoint(vec.Easting, vec.Northing));
                        }
                        result.Boundaries.Add(ringList);
                    }
                }
            }

            // Parse AB lines FeatureCollection
            if (dto.AbLinesRaw != null)
            {
                var fc = dto.AbLinesRaw;
                if (fc["type"]?.ToString() == "FeatureCollection" && fc["features"] is JArray features)
                {
                    foreach (var feature in features)
                    {
                        var geom = feature["geometry"];
                        if (geom?["type"]?.ToString() != "LineString") continue;
                        var coords = geom["coordinates"] as JArray;
                        if (coords == null || coords.Count < 2) continue;

                        var ptA = coords[0];
                        var ptB = coords[1];

                        var vA = converter.ToLocal(ptA[1].Value<double>(), ptA[0].Value<double>());
                        var vB = converter.ToLocal(ptB[1].Value<double>(), ptB[0].Value<double>());
                        double heading = GeoConverter.HeadingFromPoints(vA, vB);

                        var ab = new AbLineLocal
                        {
                            Name = feature["properties"]?["name"]?.ToString() ?? "Unnamed",
                            Heading = heading,
                            PtA = new LocalPoint(vA.Easting, vA.Northing),
                            PtB = new LocalPoint(vB.Easting, vB.Northing),
                            CurvePoints = new List<LocalPoint>()
                        };

                        // Parse optional curve points (after PtB)
                        for (int i = 2; i < coords.Count; i++)
                        {
                            var pt = coords[i];
                            var v = converter.ToLocal(pt[1].Value<double>(), pt[0].Value<double>());
                            ab.CurvePoints.Add(new LocalPoint(v.Easting, v.Northing));
                        }

                        result.AbLines.Add(ab);
                    }
                }
            }

            return result;
        }
    }
}
