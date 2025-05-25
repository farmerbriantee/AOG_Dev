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
            public List<List<vec3>> Boundaries { get; set; }
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

            List<List<vec3>> boundaries = new List<List<vec3>>();
            foreach (var b in gps.bnd.bndList)
            {
                boundaries.Add(b.fenceLine.ToList());
            }

            List<CTrk> tracks = gps.trk.gArr.ToList();

            FieldSnapshot snapshot = new FieldSnapshot
            {
                FieldName = gps.displayFieldName,
                FieldDirectory = dir,
                FieldId = fieldId,
                OriginLat = CNMEA.latStart,
                OriginLon = CNMEA.lonStart,
                Convergence = 0,
                Boundaries = boundaries,
                Tracks = tracks,
                Converter = gps.pn
            };
            return snapshot;
        }

        // Upload snapshot to AgShare using boundary with holes
        public static async Task UploadAsync(FieldSnapshot snapshot, AgShareClient client)
        {
            try
            {
                if (snapshot.Boundaries == null || snapshot.Boundaries.Count == 0)
                    return;

                // First ring = outer
                List<CoordinateDto> outer = ConvertBoundary(snapshot.Boundaries[0], snapshot.Converter);

                if (outer == null || outer.Count < 3)
                    return;

                // Remaining = holes
                List<List<CoordinateDto>> holes = new List<List<CoordinateDto>>();
                for (int i = 1; i < snapshot.Boundaries.Count; i++)
                {
                    var hole = ConvertBoundary(snapshot.Boundaries[i], snapshot.Converter);
                    if (hole.Count >= 4) holes.Add(hole);
                }

                List<object> abLines = ConvertAbLines(snapshot.Tracks, snapshot.Converter);

                bool isPublic = false;
                try
                {
                    string json = await client.DownloadFieldAsync(snapshot.FieldId);
                    AgShareFieldDto field = JsonConvert.DeserializeObject<AgShareFieldDto>(json);
                    if (field != null) isPublic = field.IsPublic;
                }
                catch (Exception)
                {
                    Log.EventWriter("Failed to check field visibility on AgShare, defaulting to private.");
                }

                // Prepare boundary object with outer and holes
                var boundary = new
                {
                    outer = outer,
                    holes = holes
                };

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
                    string txtPath = Path.Combine(snapshot.FieldDirectory, "agshare.txt");
                    File.WriteAllText(txtPath, snapshot.FieldId.ToString());
                }

                snapshot = null;
            }
            catch (Exception ex)
            {
                Log.EventWriter("Error uploading field to AgShare: " + ex.Message);
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
