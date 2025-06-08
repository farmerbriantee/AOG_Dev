using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AOG.Logic;
using AOG.Classes;
using System.Diagnostics;

namespace AOG
{
    /// <summary>
    /// Central helper class for downloading, parsing and saving AgShare fields locally.
    /// </summary>
    public class CAgShareDownloader
    {
        private readonly AgShareClient client;

        public CAgShareDownloader()
        {
            // Initialize AgShare client using stored settings
            client = new AgShareClient(Settings.User.AgShareServer, Settings.User.AgShareApiKey);
        }

        // Downloads a field and saves it to disk
        public async Task<bool> DownloadAndSaveAsync(Guid fieldId)
        {
            try
            {
                string json = await client.DownloadFieldAsync(fieldId);
                var dto = JsonConvert.DeserializeObject<AgShareFieldDto>(json);
                var model = AgShareFieldParser.Parse(dto);
                string fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, model.Name);
                FieldFileWriter.WriteAllFiles(model, fieldDir);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during download: {ex.Message}");
                return false;
            }
        }

        // Retrieves a list of user-owned fields
        public async Task<List<AgShareGetOwnFieldDto>> GetOwnFieldsAsync()
        {
            return await client.GetOwnFieldsAsync();
        }

        // Downloads a field DTO for preview only
        public async Task<AgShareFieldDto> DownloadFieldPreviewAsync(Guid fieldId)
        {
            string json = await client.DownloadFieldAsync(fieldId);
            return JsonConvert.DeserializeObject<AgShareFieldDto>(json);
        }
    }

    /// <summary>
    /// Utility class that writes a LocalFieldModel to standard AgOpenGPS-compatible files.
    /// </summary>
    public static class FieldFileWriter
    {
        // Writes all files required for a field
        public static void WriteAllFiles(LocalFieldModel field, string fieldDir)
        {
            if (!Directory.Exists(fieldDir))
                Directory.CreateDirectory(fieldDir);

            WriteAgShareId(fieldDir, field.FieldId);
            WriteFieldTxt(fieldDir, field.Origin);
            WriteBoundaryTxt(fieldDir, field.Boundaries);
            WriteTrackLinesTxt(fieldDir, field.AbLines);
            WriteStaticFiles(fieldDir); // Flags, Headland
        }

        // Writes agshare.txt with the field ID
        private static void WriteAgShareId(string fieldDir, Guid fieldId)
        {
            File.WriteAllText(Path.Combine(fieldDir, "agshare.txt"), fieldId.ToString());
        }

        // Writes origin and metadata to Field.txt
        private static void WriteFieldTxt(string fieldDir, Wgs84 origin)
        {
            var fieldTxt = new List<string>
            {
                DateTime.Now.ToString("yyyy-MMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture),
                "$FieldDir",
                "AgShare Downloaded",
                "$Offsets",
                "0,0",
                "Convergence",
                "0", // Always 0
                "StartFix",
                origin.Lat.ToString(CultureInfo.InvariantCulture) + "," + origin.Lon.ToString(CultureInfo.InvariantCulture)
            };

            File.WriteAllLines(Path.Combine(fieldDir, "Field.txt"), fieldTxt);
            Debug.WriteLine("Saved field to: " + fieldDir);
        }

        // Writes outer and inner boundary rings to Boundary.txt
        private static void WriteBoundaryTxt(string fieldDir, List<List<LocalPoint>> boundaries)
        {
            if (boundaries == null || boundaries.Count == 0) return;

            var lines = new List<string> { "$Boundary" };

            for (int i = 0; i < boundaries.Count; i++)
            {
                var ring = boundaries[i];
                bool isHole = i != 0;

                lines.Add(isHole ? "True" : "False");
                lines.Add(ring.Count.ToString());

                foreach (var pt in ring)
                {
                    lines.Add(
                        pt.Easting.ToString("0.###", CultureInfo.InvariantCulture) + "," +
                        pt.Northing.ToString("0.###", CultureInfo.InvariantCulture) + "," +
                        pt.Heading.ToString("0.#####", CultureInfo.InvariantCulture)
                    );
                }
            }

            File.WriteAllLines(Path.Combine(fieldDir, "Boundary.txt"), lines);
        }

        // Writes AB-lines and optional curve points to TrackLines.txt
        private static void WriteTrackLinesTxt(string fieldDir, List<AbLineLocal> abLines)
        {
            var lines = new List<string> { "$TrackLines" };

            foreach (var ab in abLines)
            {
                lines.Add(ab.Name ?? "Unnamed");

                bool isCurve = ab.CurvePoints != null && ab.CurvePoints.Count > 1;

                LocalPoint ptA = ab.PtA;
                LocalPoint ptB = ab.PtB;
                double heading = ab.Heading;

                if (isCurve)
                {
                    ptA = ab.CurvePoints[0];
                    ptB = ab.CurvePoints[ab.CurvePoints.Count - 1];
                    heading = GeoConverter.HeadingFromPoints(
                        new Vec2(ptA.Easting, ptA.Northing),
                        new Vec2(ptB.Easting, ptB.Northing)
                    );
                }

                lines.Add(heading.ToString("0.###", CultureInfo.InvariantCulture));
                lines.Add(ptA.Easting.ToString("0.###", CultureInfo.InvariantCulture) + "," + ptA.Northing.ToString("0.###", CultureInfo.InvariantCulture));
                lines.Add(ptB.Easting.ToString("0.###", CultureInfo.InvariantCulture) + "," + ptB.Northing.ToString("0.###", CultureInfo.InvariantCulture));
                lines.Add("0"); // Nudge

                if (isCurve)
                {
                    lines.Add("4"); // Curve mode
                    lines.Add("True");
                    lines.Add(ab.CurvePoints.Count.ToString());

                    foreach (var pt in ab.CurvePoints)
                    {
                        lines.Add(
                            pt.Easting.ToString("0.###", CultureInfo.InvariantCulture) + "," +
                            pt.Northing.ToString("0.###", CultureInfo.InvariantCulture) + "," +
                            pt.Heading.ToString("0.#####", CultureInfo.InvariantCulture)
                        );
                    }
                }
                else
                {
                    lines.Add("2"); // AB mode
                    lines.Add("True");
                    lines.Add("0");
                }
            }

            File.WriteAllLines(Path.Combine(fieldDir, "TrackLines.txt"), lines);
        }

        // Writes default placeholder files like Flags.txt and Headland.txt
        private static void WriteStaticFiles(string fieldDir)
        {
            File.WriteAllLines(Path.Combine(fieldDir, "Flags.txt"), new[] { "$Flags", "0" });
            File.WriteAllLines(Path.Combine(fieldDir, "Headland.txt"), new[] { "$Headland", "0" });
        }
    }

}
