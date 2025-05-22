using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AOG.Classes;
using System.Linq;
using System.Diagnostics;

namespace AOG
{
    public class CAgShareUploader
    {
        public class FieldSnapshot
        {
            public string FieldName { get; set; }
            public string FieldDirectory { get; set; }
            public Guid FieldId { get; set; }
            public double OriginLat { get; set; }
            public double OriginLon { get; set; }
            public double Convergence { get; set; }
            public List<vec3> Boundary { get; set; }
            public List<CTrk> Tracks { get; set; }
            public CNMEA Converter { get; set; }
        }

        private class CoordinateDto
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        private class AgShareFieldDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public bool IsPublic { get; set; }
        }

        public static FieldSnapshot CreateSnapshot(FormGPS gps)
        {
            string dir = Path.Combine(RegistrySettings.fieldsDirectory, gps.currentFieldDirectory);
            string idPath = Path.Combine(dir, "agshare.txt");

            Guid fieldId;
            if (File.Exists(idPath))
            {
                string raw = File.ReadAllText(idPath).Trim();
                fieldId = Guid.Parse(raw);
            }
            else
            {
                fieldId = Guid.NewGuid();
            }

            List<vec3> boundary = gps.bnd.bndList.Count > 0
                ? gps.bnd.bndList[0].fenceLine.ToList()
                : new List<vec3>();

            List<CTrk> tracks = gps.trk.gArr.ToList();

            FieldSnapshot snapshot = new FieldSnapshot
            {
                FieldName = gps.displayFieldName,
                FieldDirectory = dir,
                FieldId = fieldId,
                OriginLat = CNMEA.latStart,
                OriginLon = CNMEA.lonStart,
                Convergence = 0,
                Boundary = boundary,
                Tracks = tracks,
                Converter = gps.pn
            };
            Debug.WriteLine($"FieldSnapshot created: {snapshot.FieldName}, ID: {snapshot.FieldId}");

            return snapshot;
        }

        public static async Task UploadAsync(FieldSnapshot snapshot, AgShareClient client)
        {
            try
            {
                List<CoordinateDto> boundary = ConvertBoundary(snapshot.Boundary, snapshot.Converter);
                if (boundary == null || boundary.Count < 3)
                {
                    return;
                }

                List<object> abLines = ConvertAbLines(snapshot.Tracks, snapshot.Converter);

                bool isPublic = false;
                try
                {
                    string json = await client.DownloadFieldAsync(snapshot.FieldId);
                    AgShareFieldDto field = JsonConvert.DeserializeObject<AgShareFieldDto>(json);
                    if (field != null)
                    {
                        isPublic = field.IsPublic;
                    }
                }
                catch { }

                var payload = new
                {
                    name = snapshot.FieldName,
                    isPublic,
                    origin = new { latitude = snapshot.OriginLat, longitude = snapshot.OriginLon },
                    boundary,
                    abLines,
                    convergence = snapshot.Convergence,
                    sourceId = (string)null
                };

                var (ok, message) = await client.UploadFieldAsync(snapshot.FieldId, payload);
                if (ok)
                {
                    File.WriteAllText(Path.Combine(snapshot.FieldDirectory, "agshare.txt"), snapshot.FieldId.ToString());
                }
                Debug.WriteLine("AgShare Uploaded Successfully");
            }
            catch (Exception)
            {
                // Optional: log or notify
            }
        }

        private static List<CoordinateDto> ConvertBoundary(List<vec3> localFence, CNMEA converter)
        {
            List<CoordinateDto> coords = new List<CoordinateDto>();
            for (int i = 0; i < localFence.Count; i++)
            {
                converter.ConvertLocalToWGS84(localFence[i].northing, localFence[i].easting, out double lat, out double lon);
                CoordinateDto p = new CoordinateDto
                {
                    Latitude = lat,
                    Longitude = lon
                };
                coords.Add(p);
            }

            if (coords.Count > 1)
            {
                CoordinateDto first = coords[0];
                CoordinateDto last = coords[coords.Count - 1];
                if (first.Latitude != last.Latitude || first.Longitude != last.Longitude)
                {
                    coords.Add(first);
                }
            }

            return coords;
        }

        private static List<object> ConvertAbLines(List<CTrk> tracks, CNMEA converter)
        {
            List<object> result = new List<object>();

            for (int i = 0; i < tracks.Count; i++)
            {
                CTrk ab = tracks[i];

                if (ab.mode == TrackMode.AB)
                {
                    converter.ConvertLocalToWGS84(ab.ptA.northing, ab.ptA.easting, out double latA, out double lonA);
                    converter.ConvertLocalToWGS84(ab.ptB.northing, ab.ptB.easting, out double latB, out double lonB);

                    result.Add(new
                    {
                        ab.name,
                        type = "AB",
                        coords = new[]
                        {
                            new { latitude = latA, longitude = lonA },
                            new { latitude = latB, longitude = lonB }
                        }
                    });
                }
                else if (ab.mode == TrackMode.Curve)
                {
                    List<object> coords = new List<object>();
                    for (int j = 0; j < ab.curvePts.Count; j++)
                    {
                        converter.ConvertLocalToWGS84(ab.curvePts[j].northing, ab.curvePts[j].easting, out double lat, out double lon);
                        coords.Add(new { latitude = lat, longitude = lon });
                    }

                    result.Add(new
                    {
                        ab.name,
                        type = "Curve",
                        coords
                    });
                }
            }

            return result;
        }
    }
}
