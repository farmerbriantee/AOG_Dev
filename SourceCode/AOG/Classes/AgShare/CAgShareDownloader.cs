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
    // Central helper class for downloading, parsing and saving AgShare fields
    public class CAgShareDownloader
    {
        private readonly AgShareClient client;

        public CAgShareDownloader()
        {
            client = new AgShareClient(Settings.User.AgShareServer, Settings.User.AgShareApiKey);
        }

        public async Task<bool> DownloadAndSaveAsync(Guid fieldId)
        {
            try
            {
                string json = await client.DownloadFieldAsync(fieldId);
                var field = JsonConvert.DeserializeObject<FieldDownloadDto>(json);
                var model = AgShareFieldParser.Parse(field); // now gives LocalFieldModel
                string fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, model.Name);
                FieldFileWriter.WriteAllFiles(model, fieldDir);

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Get list of user-owned fields
        public async Task<List<AgShareGetOwnFieldDto>> GetOwnFieldsAsync()
        {
            return await client.GetOwnFieldsAsync();
        }

        // Download full DTO (for preview purposes)
        public async Task<FieldDownloadDto> DownloadFieldPreviewAsync(Guid fieldId)
        {
            string json = await client.DownloadFieldAsync(fieldId);
            return JsonConvert.DeserializeObject<FieldDownloadDto>(json);
        }
    }
}
// Writes a LocalFieldModel to standard AgOpenGPS files
public static class FieldFileWriter
{
    public static void WriteAllFiles(LocalFieldModel field, string fieldDir)
    {
        if (!Directory.Exists(fieldDir))
            Directory.CreateDirectory(fieldDir);

        WriteAgShareId(fieldDir, field.FieldId);
        WriteFieldTxt(fieldDir, field.Origin);
        WriteBoundaryTxt(fieldDir, field.Boundaries);
        WriteTrackLinesTxt(fieldDir, field.AbLines);
        WriteStaticFiles(fieldDir); // e.g., Flags.txt, Headland.txt
    }

    private static void WriteAgShareId(string fieldDir, Guid fieldId)
    {
        File.WriteAllText(Path.Combine(fieldDir, "agshare.txt"), fieldId.ToString());
    }

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
                "0", // Always 0 as agreed
                "StartFix",
                $"{origin.Lat.ToString(CultureInfo.InvariantCulture)},{origin.Lon.ToString(CultureInfo.InvariantCulture)}"
            };

        File.WriteAllLines(Path.Combine(fieldDir, "Field.txt"), fieldTxt);
        Debug.WriteLine(fieldDir);
    }

    // Writes full Boundary.txt including heading and hole indicators
    private static void WriteBoundaryTxt(string fieldDir, List<List<LocalPoint>> boundaries)
    {
        if (boundaries == null || boundaries.Count == 0) return;

        var lines = new List<string> { "$Boundary" };

        for (int i = 0; i < boundaries.Count; i++)
        {
            var ring = boundaries[i];
            bool isHole = i != 0; // first = outer → False, rest = holes → True

            lines.Add(isHole ? "True" : "False");
            lines.Add(ring.Count.ToString());

            foreach (var pt in ring)
            {
                lines.Add($"{pt.Easting.ToString("0.###", CultureInfo.InvariantCulture)}," +
                          $"{pt.Northing.ToString("0.###", CultureInfo.InvariantCulture)}," +
                          $"{pt.Heading.ToString("0.#####", CultureInfo.InvariantCulture)}");
            }
        }

        string path = Path.Combine(fieldDir, "Boundary.txt");
        File.WriteAllLines(path, lines);
    }


    private static void WriteTrackLinesTxt(string fieldDir, List<AbLineLocal> abLines)
    {
        var lines = new List<string> { "$TrackLines" };

        foreach (var ab in abLines)
        {
            lines.Add(ab.Name ?? "Unnamed");
            lines.Add(ab.Heading.ToString("0.###", CultureInfo.InvariantCulture));
            lines.Add($"{ab.PtA.Easting.ToString("0.###", CultureInfo.InvariantCulture)},{ab.PtA.Northing.ToString("0.###", CultureInfo.InvariantCulture)}");
            lines.Add($"{ab.PtB.Easting.ToString("0.###", CultureInfo.InvariantCulture)},{ab.PtB.Northing.ToString("0.###", CultureInfo.InvariantCulture)}");
            lines.Add("0"); // nudge

            if (ab.CurvePoints != null && ab.CurvePoints.Count > 0)
            {
                lines.Add("4"); //   Mode = Curve
                lines.Add("True");
                lines.Add(ab.CurvePoints.Count.ToString());
                foreach (var pt in ab.CurvePoints)
                {
                    lines.Add($"{pt.Easting.ToString("0.###", CultureInfo.InvariantCulture)},{pt.Northing.ToString("0.###", CultureInfo.InvariantCulture)}");
                }
            }
            else
            {
                lines.Add("2"); // Mode = AB
                lines.Add("True");
                lines.Add("0");
            }
        }


        File.WriteAllLines(Path.Combine(fieldDir, "TrackLines.txt"), lines);
    }

    private static void WriteStaticFiles(string fieldDir)
    {
        File.WriteAllLines(Path.Combine(fieldDir, "Flags.txt"), new[] { "$Flags", "0" });
        File.WriteAllLines(Path.Combine(fieldDir, "Headland.txt"), new[] { "$Headland", "0" });
    }


    // Coordinate for lat/lon
    public class Coordinate
    {
        public double lat;
        public double lon;
    }

    // AB Line structure
    public class AbLine
    {
        public string name;
        public string type;
        public List<Coordinate> points;
    }

    // Local model of a field
    public class FieldModel
    {
        public Guid Id = Guid.NewGuid();
        public string Name;
        public double OriginLat;
        public double OriginLon;
        public List<List<Coordinate>> Boundaries;
        public List<AbLine> AbLines;
    }
}
