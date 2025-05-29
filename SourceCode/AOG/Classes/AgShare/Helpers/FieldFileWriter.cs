using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using AOG.Classes;

namespace AOG.Logic
{
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
    }
}
