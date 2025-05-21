using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AOG.Classes;

namespace AOG
{
    public class AgShareFieldDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }

    public class CoordinateDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class CAgShareUploader
    {
        private readonly AgShareClient client;
        private readonly string fieldDirectory;
        private readonly FormGPS gps;

        public CAgShareUploader(AgShareClient agShareClient, string fieldDir, FormGPS gpsContext)
        {
            client = agShareClient;
            fieldDirectory = fieldDir;
            gps = gpsContext;
        }

        public async Task UploadAsync()
        {
            try
            {
                string idPath = Path.Combine(fieldDirectory, "agshare.txt");
                Guid fieldId = File.Exists(idPath)
                    ? Guid.Parse(File.ReadAllText(idPath).Trim())
                    : Guid.NewGuid();

                bool isPublic = false;
                try
                {
                    string json = await client.DownloadFieldAsync(fieldId);
                    var field = JsonConvert.DeserializeObject<AgShareFieldDto>(json);
                    isPublic = field?.IsPublic ?? false;
                }
                catch
                {
                    // Field does not exist yet
                }

                if (gps.bnd.bndList == null || gps.bnd.bndList.Count == 0)
                {
                    gps.TimedMessageBox(2000, "AgShare", "Upload stopped: No boundary.");
                    return;
                }

                gps.bnd.bndList[0].FixFenceLine(0);
                var boundary = GetBoundary();

                if (boundary == null || boundary.Count < 3)
                {
                    gps.TimedMessageBox(2000, "AgShare", "Upload Failed: boundary is empty or corrupt.");
                    return;
                }

                var payload = new
                {
                    name = Path.GetFileName(fieldDirectory),
                    isPublic = isPublic,
                    origin = GetOrigin(),
                    boundary = boundary,
                    abLines = BuildAbLinesAsDto(),
                    convergence = GetConvergence(),
                    sourceId = (string)null
                };

                var (ok, message) = await client.UploadFieldAsync(fieldId, payload);
                if (!ok)
                {
                    gps.TimedMessageBox(2000, "AgShare", "Upload failed: " + message);
                    return;
                }

                File.WriteAllText(idPath, fieldId.ToString());
                gps.TimedMessageBox(2000, "AgShare", "Upload Success!");
            }
            catch (Exception ex)
            {
                gps.TimedMessageBox(2000, "AgShare", "Upload error: " + ex.Message);
            }
        }

        private object GetOrigin()
        {
            string fieldTxt = Path.Combine(fieldDirectory, "Field.txt");
            if (!File.Exists(fieldTxt)) return new { latitude = 0.0, longitude = 0.0 };

            using (var reader = new StreamReader(fieldTxt))
            {
                for (int i = 0; i < 8; i++) reader.ReadLine();
                var parts = reader.ReadLine()?.Split(',');
                return new
                {
                    latitude = double.Parse(parts[0], CultureInfo.InvariantCulture),
                    longitude = double.Parse(parts[1], CultureInfo.InvariantCulture)
                };
            }
        }

        private double GetConvergence()
        {
            string fieldTxt = Path.Combine(fieldDirectory, "Field.txt");
            if (!File.Exists(fieldTxt)) return 0.0;

            using (var reader = new StreamReader(fieldTxt))
            {
                for (int i = 0; i < 6; i++) reader.ReadLine();
                string value = reader.ReadLine();
                return double.Parse(value, CultureInfo.InvariantCulture);
            }
        }

        private List<CoordinateDto> GetBoundary()
        {
            var coords = new List<CoordinateDto>();

            foreach (var pt in gps.bnd.bndList[0].fenceLine)
            {
                gps.pn.ConvertLocalToWGS84(pt.northing, pt.easting, out double lat, out double lon);
                coords.Add(new CoordinateDto { Latitude = lat, Longitude = lon });
            }

            if (coords.Count > 1)
            {
                var first = coords[0];
                var last = coords[coords.Count - 1];
                if (first.Latitude != last.Latitude || first.Longitude != last.Longitude)
                {
                    coords.Add(new CoordinateDto { Latitude = first.Latitude, Longitude = first.Longitude });
                }
            }

            return coords;
        }

        private List<object> BuildAbLinesAsDto()
        {
            var result = new List<object>();

            foreach (var ab in gps.trk.gArr)
            {
                if (ab.mode == TrackMode.AB)
                {
                    gps.pn.ConvertLocalToWGS84(ab.ptA.northing, ab.ptA.easting, out double latA, out double lonA);
                    gps.pn.ConvertLocalToWGS84(ab.ptB.northing, ab.ptB.easting, out double latB, out double lonB);

                    result.Add(new
                    {
                        name = ab.name,
                        type = "AB",
                        coords = new List<object>
                {
                    new { latitude = latA, longitude = lonA },
                    new { latitude = latB, longitude = lonB }
                }
                    });
                }
                else if (ab.mode == TrackMode.Curve)
                {
                    var coords = new List<object>();
                    foreach (var pt in ab.curvePts)
                    {
                        gps.pn.ConvertLocalToWGS84(pt.northing, pt.easting, out double lat, out double lon);
                        coords.Add(new { latitude = lat, longitude = lon });
                    }

                    result.Add(new
                    {
                        name = ab.name,
                        type = "Curve",
                        coords = coords
                    });
                }
            }

            return result;
        }

    }
}
