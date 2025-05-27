using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using AOG.Classes;

namespace AOG
{
    public class CAgShareDownloader
    {
        // Download, open in FormGPS, and save to disk using AOG logic
        public static async Task<bool> DownloadAndOpenFieldAsync(FieldDownloadDto field, FormGPS gps)
        {
            try
            {
                string fieldPath = Path.Combine(RegistrySettings.fieldsDirectory, field.Name);

                // Prompt if field already exists
                if (Directory.Exists(fieldPath))
                {
                    var result = MessageBox.Show(
                        $"Field '{field.Name}' already exists. Overwrite?",
                        "Field Exists",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result != DialogResult.Yes)
                        return false;

                    Directory.Delete(fieldPath, true);
                }

                Directory.CreateDirectory(fieldPath);

                // Set origin for projection
                CNMEA.latStart = field.OriginLat;
                CNMEA.lonStart = field.OriginLon;

                gps.displayFieldName = field.Name;
                gps.currentFieldDirectory = field.Name;

                // Create new CBoundary and fill outer + holes
                var bnd = new CBoundary(gps);

                // Outer boundary
                var outerRing = new CBoundaryList();
                foreach (var pt in field.OuterBoundary)
                {
                    gps.pn.ConvertWGS84ToLocal(pt.Latitude, pt.Longitude, out double n, out double e);
                    outerRing.fenceLine.Add(new vec3(n, e, 0));
                }
                bnd.AddToBoundList(outerRing, 0); // 0 = outer ring

                // Inner boundaries (holes)
                if (field.InnerBoundaries != null)
                {
                    foreach (var inner in field.InnerBoundaries)
                    {
                        var innerRing = new CBoundaryList();
                        foreach (var pt in inner)
                        {
                            gps.pn.ConvertWGS84ToLocal(pt.Latitude, pt.Longitude, out double n, out double e);
                            innerRing.fenceLine.Add(new vec3(n, e, 0));
                        }
                        bnd.AddToBoundList(innerRing, 1); // 1 = inner ring
                    }
                }

                // Assign to GPS context
                gps.bnd = bnd;

                // Create new CTracks instance and add all AB/Curve tracks
                var tracks = new CTracks(gps);
                foreach (var ab in field.AbLines)
                {
                    if (ab.Type == "AB" && ab.Coords.Count == 2)
                    {
                        gps.pn.ConvertWGS84ToLocal(ab.Coords[0].Latitude, ab.Coords[0].Longitude, out double na, out double ea);
                        gps.pn.ConvertWGS84ToLocal(ab.Coords[1].Latitude, ab.Coords[1].Longitude, out double nb, out double eb);

                        var trk = new CTrk(TrackMode.AB)
                        {
                            name = ab.Name,
                            nudgeDistance = 0,
                            isVisible = true,
                            ptA = new vec2(ea, na),
                            ptB = new vec2(eb, nb)
                        };

                        tracks.AddTrack(trk);
                    }
                    else if (ab.Type == "Curve" && ab.Coords.Count > 1)
                    {
                        var trk = new CTrk(TrackMode.Curve)
                        {
                            name = ab.Name,
                            nudgeDistance = 0,
                            isVisible = true
                        };

                        foreach (var pt in ab.Coords)
                        {
                            gps.pn.ConvertWGS84ToLocal(pt.Latitude, pt.Longitude, out double n, out double e);
                            trk.curvePts.Add(new vec3(n, e, 0));
                        }

                        tracks.AddTrack(trk);
                    }
                }

                gps.trk = tracks;

                // Save everything to disk
                gps.FileNewFromAgShare();

                // Write agshare.txt separately
                File.WriteAllText(Path.Combine(fieldPath, "agshare.txt"), field.Id.ToString());

                return true;
            }
            catch (Exception ex)
            {
                Log.EventWriter("Error opening AgShare field: " + ex.Message);
                return false;
            }
        }
    }
}
