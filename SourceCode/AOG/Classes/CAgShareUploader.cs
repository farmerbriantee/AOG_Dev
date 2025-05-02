using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AOG.Classes;

namespace AOG
{
    // DTO for reading cloud field info
    public class AgShareFieldDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
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
                // Step 1: Get or create field ID
                string idPath = Path.Combine(fieldDirectory, "agshare.txt");
                Guid fieldId = File.Exists(idPath)
                    ? Guid.Parse(File.ReadAllText(idPath).Trim())
                    : Guid.NewGuid();

                // Step 2: Check if field exists in the cloud
                bool isPublic = false;

                try
                {
                    string json = await client.DownloadFieldAsync(fieldId);
                    var field = JsonConvert.DeserializeObject<AgShareFieldDto>(json);
                    isPublic = field?.IsPublic ?? false;
                }
                catch
                {
                    // Not found or invalid → isPublic remains false
                }

                // Step 3: Build payload
                var payload = new
                {
                    name = Path.GetFileName(fieldDirectory),
                    isPublic = isPublic,
                    origin = GetOrigin(),
                    boundary = GetBoundary(),
                    abLines = GetAbLines(),
                    convergence = GetConvergence(),
                    sourceId = (string)null
                };

                // Step 4: Upload
                var (ok, message) = await client.UploadFieldAsync(fieldId, payload);
                if (!ok)
                {
                    System.Windows.Forms.MessageBox.Show("Upload failed: " + message);
                    return;
                }

                // Step 5: Write agshare.txt
                File.WriteAllText(idPath, fieldId.ToString());

                System.Windows.Forms.MessageBox.Show("Upload complete!");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Upload error: " + ex.Message);
            }
        }

        private object GetOrigin()
        {
            string fieldTxt = Path.Combine(fieldDirectory, "Field.txt");
            if (!File.Exists(fieldTxt)) return new { latitude = 0.0, longitude = 0.0 };

            using (var reader = new StreamReader(fieldTxt))
            {
                for (int i = 0; i < 8; i++) reader.ReadLine(); // skip to startfix line
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
                for (int i = 0; i < 6; i++) reader.ReadLine(); // skip to convergence
                string value = reader.ReadLine();
                return double.Parse(value, CultureInfo.InvariantCulture);
            }
        }

        private List<object> GetBoundary()
        {
            var result = new List<object>();
            foreach (var b in gps.bnd.bndList)
            {
                foreach (var pt in b.fenceLine)
                {
                    gps.pn.ConvertLocalToWGS84(pt.northing, pt.easting, out double lat, out double lon);
                    result.Add(new { latitude = lat, longitude = lon });
                }
            }
            return result;
        }

        private List<object> GetAbLines()
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
                        coords = new[]
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
