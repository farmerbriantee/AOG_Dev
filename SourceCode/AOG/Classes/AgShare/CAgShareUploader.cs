using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace AOG
{
    public class CAgShareUploader
    {
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
        public static async Task<bool> UploadAsync(FieldSnapshot snapshot)
        {
            try
            {
                //Init AgShareClient
                var client = new AgShareClient(Settings.User.AgShareServer, Settings.User.AgShareApiKey);

                if (snapshot.Boundaries == null || snapshot.Boundaries.Count == 0)
                    return false;

                // First ring = outer
                List<CoordinateDto> outer = ConvertBoundary(snapshot.Boundaries[0], snapshot.Converter);

                if (outer == null || outer.Count < 3)
                    return false;

                // Remaining = holes
                List<List<CoordinateDto>> holes = new List<List<CoordinateDto>>();
                for (int i = 1; i < snapshot.Boundaries.Count; i++)
                {
                    var hole = ConvertBoundary(snapshot.Boundaries[i], snapshot.Converter);
                    if (hole.Count >= 4) holes.Add(hole);
                }

                var abLines = ConvertAbLines(snapshot.Tracks, snapshot.Converter);
                    
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
                return ok;
            }
            catch (Exception ex)
            {
                Log.EventWriter("Error uploading field to AgShare: " + ex.Message);
            }
            return false;
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

        private static List<AbLineUploadDto> ConvertAbLines(List<CTrk> tracks, CNMEA converter)
        {
            var result = new List<AbLineUploadDto>();

            foreach (var ab in tracks)
            {
                if (ab.mode == TrackMode.AB)
                {
                    converter.ConvertLocalToWGS84(ab.ptA.northing, ab.ptA.easting, out double latA, out double lonA);
                    converter.ConvertLocalToWGS84(ab.ptB.northing, ab.ptB.easting, out double latB, out double lonB);

                    result.Add(new AbLineUploadDto
                    {
                        Name = ab.name,
                        Type = "AB",
                        Coords = new List<CoordinateDto>
                {
                    new CoordinateDto { Latitude = latA, Longitude = lonA },
                    new CoordinateDto { Latitude = latB, Longitude = lonB }
                }
                    });
                }
                else if (ab.mode == TrackMode.Curve && ab.curvePts.Count >= 2)
                {
                    var coords = new List<CoordinateDto>();
                    foreach (var pt in ab.curvePts)
                    {
                        converter.ConvertLocalToWGS84(pt.northing, pt.easting, out double lat, out double lon);
                        coords.Add(new CoordinateDto { Latitude = lat, Longitude = lon });
                    }

                    result.Add(new AbLineUploadDto
                    {
                        Name = ab.name,
                        Type = "Curve",
                        Coords = coords
                    });
                }
            }

            return result;
        }



    }
}
