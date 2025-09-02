using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Text;
using System.Diagnostics;

using AOG.Classes;

namespace AOG
{
    public class CFieldFile
    {
        public List<vec2> bndPts = new List<vec2>();
        public vec2 start;
        public string name;
    }

    public class CFieldFiles
    {
        public List<CFieldFile> fieldArr = new List<CFieldFile>();

    }

    public partial class FormGPS
    {
        //list of the list of patch data individual triangles for contour tracking
        public List<List<vec3>> contourSaveList = new List<List<vec3>>();

        //list of the list of patch data individual triangles for bndPts sections
        public List<List<vec3>> patchSaveList = new List<List<vec3>>();

        //list of the list of patch data individual triangles for that entire section activity
        public List<List<vec3>> patchList = new List<List<vec3>>();

        //list of the list of patch data individual triangles for background
        public List<List<vec3>> patchListLayer = new List<List<vec3>>();

        public CFieldFiles fieldFilesList = new CFieldFiles();

        #region Create Files

        //Create contour file
        public void FileCreateSections()
        {
            //$Sections
            //10 - points in this patch
            //10.1728031317344,0.723157039771303 -easting, northing

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, currentJobDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Sections.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
            { }
        }

        public void FileCreateBoundary()
        {
            //$Sections
            //10 - points in this patch
            //10.1728031317344,0.723157039771303 -easting, northing

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Boundary.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
            {
                //write paths # of sections
                writer.WriteLine("$Boundary");
            }
        }

        //Create contour file
        public void FileCreateContour()
        {
            //12  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, currentJobDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Contour.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
            {
                writer.WriteLine("$Contour");
            }
        }

        public void FileCreateElevation()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //if (!isFieldStarted)
            //{
            //    using (TimedMessageBox(3000, "Ooops, Job Not Started", "Start a Job First"))
            //    {  }
            //    return;
            //}

            string myFileName;

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, currentJobDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            myFileName = "Elevation.txt";

            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));

                writer.WriteLine("$FieldDir");
                writer.WriteLine("Elevation");

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine("0,0");

                writer.WriteLine("Convergence");
                writer.WriteLine("0");

                writer.WriteLine("StartFix");
                writer.WriteLine(pn.latitude.ToString(CultureInfo.InvariantCulture) + "," + pn.longitude.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("Latitude,Longitude,Elevation,Quality,Easting,Northing,Heading,Roll");
            }
        }

        //creates the bndPts file when starting new bndPts
        public void FileCreateField()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            if (!isFieldStarted)
            {
                TimedMessageBox(3000, gStr.Get(gs.gsFieldNotOpen), gStr.Get(gs.gsCreateNewField));
                return;
            }
            string myFileName;

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            myFileName = "Field.txt";

            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));

                writer.WriteLine("$FieldDir");
                writer.WriteLine("FieldNew");

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine("0,0");

                writer.WriteLine("Convergence");
                writer.WriteLine("0");

                writer.WriteLine("StartFix");
                writer.WriteLine(pn.latitude.ToString(CultureInfo.InvariantCulture) + "," + pn.longitude.ToString(CultureInfo.InvariantCulture));
            }

            directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "Jobs");

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }
        }

        //Create Flag file
        public void FileCreateFlags()
        {
            //$Sections
            //10 - points in this patch
            //10.1728031317344,0.723157039771303 -easting, northing

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Flags.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
            {
                //write paths # of sections
                //writer.WriteLine("$Sectionsv4");
            }
        }

        //Create contour file
        public void FileCreateRecPath()
        {
            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "RecPath.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
            {
                //write paths # of sections
                writer.WriteLine("$RecPath");
                writer.WriteLine("0");
            }
        }

        #endregion 

        #region Field Open Resume

        //function to open a previously saved bndPts, resume, open exisiting, open named bndPts
        public void FileOpenField(string fileAndDirectory)
        {
            //close the existing job and reset everything
            if (isFieldStarted) FileSaveEverythingBeforeClosingField();

            if (!File.Exists(fileAndDirectory)) return;

            //and open a new job
            this.FieldNew();

            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //start to read the file
            string line;
            using (StreamReader reader = new StreamReader(fileAndDirectory))
            {
                try
                {
                    //Date time line
                    line = reader.ReadLine();

                    //dir header $FieldDir
                    line = reader.ReadLine();

                    //read bndPts directory
                    line = reader.ReadLine();

                    currentFieldDirectory = Path.GetDirectoryName(fileAndDirectory);
                    currentFieldDirectory = new DirectoryInfo(currentFieldDirectory).Name;

                    displayFieldName = currentFieldDirectory;

                    //Offset header
                    line = reader.ReadLine();

                    //read the Offsets 
                    line = reader.ReadLine();
                    string[] offs = line.Split(',');

                    //convergence angle update
                    if (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                    }

                    //start positions
                    if (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        offs = line.Split(',');

                        pn.SetLocalMetersPerDegree(double.Parse(offs[0], CultureInfo.InvariantCulture), double.Parse(offs[1], CultureInfo.InvariantCulture));
                    }
                }

                catch (Exception e)
                {
                    Log.EventWriter("While Opening Field" + e.ToString());

                    TimedMessageBox(2000, gStr.Get(gs.gsFieldFileIsCorrupt), gStr.Get(gs.gsChooseADifferentField));

                    FileSaveEverythingBeforeClosingField();
                    return;
                }
            }

            // Tracks -------------------------------------------------------------------------------------------------
            FileLoadTracks();

            ////section patches
            //FileLoadSections();

            //// Contour points ----------------------------------------------------------------------------
            //FileLoadContour();

            // Flags -------------------------------------------------------------------------------------------------
            FileLoadFlags();

            //Boundaries
            FileLoadBoundaries();

            // Headland  -------------------------------------------------------------------------------------------------
            FileLoadHeadlands();

            //trams ---------------------------------------------------------------------------------
            FileLoadTrams();

            //backgound grid image
            FileLoadBackground();

            PanelsAndOGLSize();

            //update bndPts data
            oglZoom.Refresh();

            PanelUpdateRightAndBottom();
        }

        public void FileLoadFields()
        {
            fieldFilesList.fieldArr.Clear();
            int idx;

            string[] dirs = Directory.GetDirectories(RegistrySettings.fieldsDirectory);


            foreach (string dir in dirs)
            {
                double latStart = 0;
                double lonStart = 0;
                string fieldDirectory = Path.GetFileName(dir);
                string filename = Path.Combine(dir, "Field.txt");
                string line;

                //make sure directory has a bndPts.txt in it
                if (File.Exists(filename))
                {
                    fieldFilesList.fieldArr.Add(new CFieldFile());
                    idx = fieldFilesList.fieldArr.Count - 1;

                    //add name to the list
                    fieldFilesList.fieldArr[idx].name = fieldDirectory;

                    fieldFilesList.fieldArr[idx].start = new vec2(0, 0);

                    using (StreamReader reader = new StreamReader(filename))
                    {
                        try
                        {
                            //Date time line
                            for (int i = 0; i < 8; i++)
                            {
                                line = reader.ReadLine();
                            }

                            //start positions
                            if (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();
                                string[] offs = line.Split(',');

                                latStart = (double.Parse(offs[0], CultureInfo.InvariantCulture));
                                lonStart = (double.Parse(offs[1], CultureInfo.InvariantCulture));

                                double mPerDegreeLat = 111132.92 - 559.82 * Math.Cos(2.0 * latStart * 0.01745329251994329576923690766743) + 1.175
                                * Math.Cos(4.0 * latStart * 0.01745329251994329576923690766743) - 0.0023
                                * Math.Cos(6.0 * latStart * 0.01745329251994329576923690766743);

                                //get start coords
                                pn.GetLocalToLocal(latStart, lonStart, mPerDegreeLat, latStart, lonStart, out double northing, out double easting);
                                fieldFilesList.fieldArr[idx].start.northing = northing;
                                fieldFilesList.fieldArr[idx].start.easting = easting;

                                //grab the boundary area
                                filename = Path.Combine(dir, "Boundary.txt");
                                if (File.Exists(filename))
                                {
                                    var bndPts = new List<vec3>();

                                    using (StreamReader reader2 = new StreamReader(filename))
                                    {
                                        try
                                        {
                                            //read header
                                            line = reader2.ReadLine();//Boundary

                                            if (!reader2.EndOfStream)
                                            {
                                                //True or False OR points from older boundary files
                                                line = reader2.ReadLine();

                                                //Check for older boundary files, then above line string is num of points
                                                if (line == "True" || line == "False")
                                                {
                                                    line = reader2.ReadLine(); //number of points
                                                }

                                                //Check for latest boundary files, then above line string is num of points
                                                if (line == "True" || line == "False")
                                                {
                                                    line = reader2.ReadLine(); //number of points
                                                }

                                                int numPoints = int.Parse(line);

                                                if (numPoints > 0)
                                                {
                                                    //load the line
                                                    for (int i = 0; i < numPoints; i++)
                                                    {
                                                        line = reader2.ReadLine();
                                                        string[] words = line.Split(',');
                                                        pn.GetLocalToLocal(double.Parse(words[0], CultureInfo.InvariantCulture), double.Parse(words[1], CultureInfo.InvariantCulture), mPerDegreeLat, latStart, lonStart, out double nort, out double east);

                                                        fieldFilesList.fieldArr[idx].bndPts.Add(new vec2(east, nort));
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Log.EventWriter("Field.txt is Broken" + e.ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(fieldDirectory + " is Damaged, Missing Boundary.Txt " +
                                        "               \r\n Delete Field or Fix ", gStr.Get(gs.gsFileError), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                        }
                        catch (Exception eg)
                        {
                            Log.EventWriter("Field.txt is Broken" + eg.ToString());
                        }
                    }
                }
            }
        }

        public void FileLoadBoundaries()
        {
            string fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "Boundary.txt");
            if (!File.Exists(fileAndDirectory))
            {
                TimedMessageBox(2000, gStr.Get(gs.gsMissingBoundaryFile), gStr.Get(gs.gsButFieldIsLoaded));
            }
            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {

                        //read header
                        string line = reader.ReadLine();//Boundary

                        while (!reader.EndOfStream)
                        {
                            CBoundaryList newBnd = new CBoundaryList();

                            //True or False OR points from older boundary files
                            line = reader.ReadLine();

                            //Check for older boundary files, then above line string is num of points
                            if (line == "True" || line == "False")
                            {
                                newBnd.isDriveThru = bool.Parse(line);
                                line = reader.ReadLine(); //number of points
                            }

                            //Check for latest boundary files, then above line string is num of points
                            if (line == "True" || line == "False")
                            {
                                line = reader.ReadLine(); //number of points
                            }

                            int numPoints = int.Parse(line);

                            if (numPoints > 0)
                            {
                                //load the line
                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vec3 vecPt = new vec3(
                                    double.Parse(words[0], CultureInfo.InvariantCulture),
                                    double.Parse(words[1], CultureInfo.InvariantCulture),
                                    double.Parse(words[2], CultureInfo.InvariantCulture));

                                    newBnd.fenceLine.Add(vecPt);
                                }

                                bnd.AddToBoundList(newBnd, bnd.bndList.Count);
                            }
                        }

                        CalculateSectionPatchesMinMax();
                        fd.UpdateFieldBoundaryGUIAreas();
                    }
                    catch (Exception e)
                    {
                        TimedMessageBox(2000, gStr.Get(gs.gsBoundaryLineFilesAreCorrupt), gStr.Get(gs.gsButFieldIsLoaded));

                        Log.EventWriter("Load Boundary Line" + e.ToString());
                    }
                }
            }

        }

        public void FileLoadBackground()
        {
            worldGrid.isGeoMap = false;

            //Back Image
            string fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "BackPic.txt");
            if (File.Exists(fileAndDirectory))
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        string line = reader.ReadLine();

                        line = reader.ReadLine();
                        worldGrid.isGeoMap = bool.Parse(line);

                        line = reader.ReadLine();
                        worldGrid.eastingMaxGeo = double.Parse(line, CultureInfo.InvariantCulture);
                        line = reader.ReadLine();
                        worldGrid.eastingMinGeo = double.Parse(line, CultureInfo.InvariantCulture);
                        line = reader.ReadLine();
                        worldGrid.northingMaxGeo = double.Parse(line, CultureInfo.InvariantCulture);
                        line = reader.ReadLine();
                        worldGrid.northingMinGeo = double.Parse(line, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        worldGrid.isGeoMap = false;
                    }

                    if (worldGrid.isGeoMap)
                    {
                        fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "BackPic.png");
                        if (File.Exists(fileAndDirectory))
                        {
                            var bitmap = new Bitmap(Image.FromFile(fileAndDirectory));

                            GL.BindTexture(TextureTarget.Texture2D, texture[(int)textures.bingGrid]);
                            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                            bitmap.UnlockBits(bitmapData);
                        }
                        else
                        {
                            worldGrid.isGeoMap = false;
                        }
                    }
                }
            }
        }

        public void FileLoadFlags()
        {
            string fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "Flags.txt");
            if (!File.Exists(fileAndDirectory))
            {
                TimedMessageBox(2000, gStr.Get(gs.gsMissingFlagsFile), gStr.Get(gs.gsButFieldIsLoaded));

            }

            else
            {
                flagPts?.Clear();
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        string line = reader.ReadLine();

                        //number of flags
                        line = reader.ReadLine();
                        int points = int.Parse(line);

                        if (points > 0)
                        {
                            double lat;
                            double longi;
                            double east;
                            double nort;
                            double head;
                            int color, ID;
                            string notes;

                            for (int v = 0; v < points; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');

                                if (words.Length == 8)
                                {
                                    lat = double.Parse(words[0], CultureInfo.InvariantCulture);
                                    longi = double.Parse(words[1], CultureInfo.InvariantCulture);
                                    east = double.Parse(words[2], CultureInfo.InvariantCulture);
                                    nort = double.Parse(words[3], CultureInfo.InvariantCulture);
                                    head = double.Parse(words[4], CultureInfo.InvariantCulture);
                                    color = int.Parse(words[5]);
                                    ID = int.Parse(words[6]);
                                    notes = words[7].Trim();
                                }
                                else
                                {
                                    lat = double.Parse(words[0], CultureInfo.InvariantCulture);
                                    longi = double.Parse(words[1], CultureInfo.InvariantCulture);
                                    east = double.Parse(words[2], CultureInfo.InvariantCulture);
                                    nort = double.Parse(words[3], CultureInfo.InvariantCulture);
                                    head = 0;
                                    color = int.Parse(words[4]);
                                    ID = int.Parse(words[5]);
                                    notes = "";
                                }

                                CFlag flagPt = new CFlag(lat, longi, east, nort, head, color, ID, notes);
                                flagPts.Add(flagPt);
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        TimedMessageBox(2000, gStr.Get(gs.gsFlagFileIsCorrupt), gStr.Get(gs.gsButFieldIsLoaded));

                        Log.EventWriter("FieldOpen, Loading Flags, Corrupt Flag File" + e.ToString());
                    }
                }
            }
        }

        public void FileLoadHeadlands()
        {
            string fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "Headland.txt");

            if (File.Exists(fileAndDirectory))
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        string line = reader.ReadLine();

                        for (int k = 0; true; k++)
                        {
                            if (reader.EndOfStream) break;

                            if (bnd.bndList.Count > k)
                            {
                                bnd.bndList[k].hdLine.Clear();

                                //read the number of points
                                line = reader.ReadLine();
                                int numPoints = int.Parse(line);

                                if (numPoints > 0)
                                {
                                    //load the line
                                    for (int i = 0; i < numPoints; i++)
                                    {
                                        line = reader.ReadLine();
                                        string[] words = line.Split(',');
                                        vec3 vecPt = new vec3(
                                            double.Parse(words[0], CultureInfo.InvariantCulture),
                                            double.Parse(words[1], CultureInfo.InvariantCulture),
                                            double.Parse(words[2], CultureInfo.InvariantCulture));
                                        bnd.bndList[k].hdLine.Add(vecPt);
                                    }
                                }
                            }
                            else break;
                        }
                    }

                    catch (Exception e)
                    {
                        TimedMessageBox(2000, "Headland File is Corrupt", "But Field is Loaded");

                        Log.EventWriter("Load Headland Loop" + e.ToString());
                    }
                }
            }

            if (bnd.bndList.Count > 0 && bnd.bndList[0].hdLine.Count > 0)
            {
                //Triangulate headland polygon
                CPolygon hdLinePolygon = new CPolygon(bnd.bndList[0].hdLine.ToArray());
                bnd.bndList[0].hdLineTriangleList = hdLinePolygon.Triangulate();

                bnd.isHeadlandOn = true;
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
                btnHeadlandOnOff.Visible = true;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
            }
            else
            {
                bnd.isHeadlandOn = false;
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
                btnHeadlandOnOff.Visible = false;
            }

            int sett = Settings.Vehicle.setArdMac_setting0;
            btnHydLift.Visible = (((sett & 2) == 2) && bnd.isHeadlandOn);
        }

        public void FileLoadHeadLines()
        {
            hdl.tracksArr?.Clear();

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = Path.Combine(directoryName, "Headlines.txt");

            if (!File.Exists(filename))
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine("$Headlines");
                }
            }

            if (!File.Exists(filename))
            {
                TimedMessageBox(2000, gStr.Get(gs.gsFileError), "Missing Headlines File");
                Log.EventWriter("Load Field, Missing Headlines File");
            }
            else
            {
                try
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        string line;

                        //read header $CurveLine
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            var headPath = new CHeadPath();

                            //read header $CurveLine
                            headPath.name = reader.ReadLine();

                            line = reader.ReadLine();
                            headPath.moveDistance = double.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            headPath.mode = (TrackMode)int.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            headPath.a_point = int.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            int numPoints = int.Parse(line);

                            if (numPoints > 3)
                            {
                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vec3 vecPt = new vec3(double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture),
                                        double.Parse(words[2], CultureInfo.InvariantCulture));
                                    headPath.trackPts.Add(vecPt);
                                }

                                hdl.tracksArr.Add(headPath);
                            }
                        }
                    }
                }
                catch (Exception er)
                {
                    hdl.tracksArr?.Clear();

                    TimedMessageBox(2000, "Headline Error", "Lines Deleted");

                    using (StreamWriter writer = new StreamWriter(filename, false))
                    {
                        try
                        {
                            writer.WriteLine("$Headlines");
                            return;

                        }
                        catch { }
                    }
                    Log.EventWriter("Load Head Lines" + er.ToString());
                }
            }
        }

        public void FileLoadContour(string dir)
        {
            if (!File.Exists(dir))
            {
                //write out the file
                using (StreamWriter writer = new StreamWriter(dir))
                {
                    //write paths # of sections
                    //writer.WriteLine("$Sectionsv4");
                }
                return;
            }

            //Points in Patch followed by easting, heading, northing, altitude
            else
            {
                using (StreamReader reader = new StreamReader(dir))
                {
                    try
                    {
                        //read header
                        string line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            //read how many vertices in the following patch
                            line = reader.ReadLine();
                            int verts = int.Parse(line);

                            vec3 vecFix = new vec3(0, 0, 0);

                            var ptList = new List<vec3>();
                            ptList.Capacity = verts + 1;

                            for (int v = 0; v < verts; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                vecFix.easting = double.Parse(words[0], CultureInfo.InvariantCulture);
                                vecFix.northing = double.Parse(words[1], CultureInfo.InvariantCulture);
                                vecFix.heading = double.Parse(words[2], CultureInfo.InvariantCulture);
                                ptList.Add(vecFix);
                            }

                            ct.stripList.Add(ptList);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.EventWriter("Loading Contour file" + e.ToString());

                        TimedMessageBox(2000, gStr.Get(gs.gsContourFileIsCorrupt), gStr.Get(gs.gsButFieldIsLoaded));
                    }
                }
            }
        }

        public void FileLoadSections(string dir)
        {
            if (!File.Exists(dir))
            {
                //write out the file
                using (StreamWriter writer = new StreamWriter(dir))
                {
                    //write paths # of sections
                    //writer.WriteLine("$Sectionsv4");
                }
                return;
            }
            else
            {
                using (StreamReader reader = new StreamReader(dir))
                {
                    try
                    {
                        fd.workedAreaTotal = 0;
                        fd.distanceUser = 0;
                        vec3 vecFix = new vec3();

                        //read header
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            int verts = int.Parse(line);

                            var triangleList = new List<vec3>(verts + 1);

                            for (int v = 0; v < verts; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                vecFix.easting = double.Parse(words[0], CultureInfo.InvariantCulture);
                                vecFix.northing = double.Parse(words[1], CultureInfo.InvariantCulture);
                                vecFix.heading = double.Parse(words[2], CultureInfo.InvariantCulture);
                                triangleList.Add(vecFix);
                            }

                            //calculate area of this patch - AbsoluteValue of (Ax(By-Cy) + Bx(Cy-Ay) + Cx(Ay-By)/2)
                            verts -= 2;
                            if (verts >= 2)
                            {
                                for (int j = 1; j < verts; j++)
                                {
                                    double temp = 0;
                                    temp = triangleList[j].easting * (triangleList[j + 1].northing - triangleList[j + 2].northing) +
                                              triangleList[j + 1].easting * (triangleList[j + 2].northing - triangleList[j].northing) +
                                                  triangleList[j + 2].easting * (triangleList[j].northing - triangleList[j + 1].northing);

                                    fd.workedAreaTotal += Math.Abs((temp * 0.5));
                                }
                                patchList.Add(triangleList);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.EventWriter("Section file" + e.ToString());

                        TimedMessageBox(2000, "Section File is Corrupt", gStr.Get(gs.gsButFieldIsLoaded));
                    }
                }
            }
        }

        public void FileLoadTracks()
        {
            trk.ResetTrack();

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = Path.Combine(directoryName, "TrackLines.txt");

            //get the file of previous AB Lines
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            if (!File.Exists(filename))
            {
                TimedMessageBox(2000, gStr.Get(gs.gsFileError), "Missing Tracks File");
                Log.EventWriter("Load Field, Missing Tracks File");
            }
            else
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    try
                    {
                        string line;

                        //read header $CurveLine
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            var track = new CTrk(TrackMode.None);
                            //read header $CurveLine
                            track.name = reader.ReadLine();
                            // get the average heading
                            line = reader.ReadLine();
                            track.heading = double.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            string[] words = line.Split(',');
                            vec2 vecPt = new vec2(double.Parse(words[0], CultureInfo.InvariantCulture),
                                double.Parse(words[1], CultureInfo.InvariantCulture));
                            track.ptA = (vecPt);

                            line = reader.ReadLine();
                            words = line.Split(',');
                            vecPt = new vec2(double.Parse(words[0], CultureInfo.InvariantCulture),
                                double.Parse(words[1], CultureInfo.InvariantCulture));
                            track.ptB = (vecPt);

                            line = reader.ReadLine();
                            track.nudgeDistance = double.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            track.mode = (TrackMode)int.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            track.isVisible = bool.Parse(line);

                            line = reader.ReadLine();
                            int numPoints = int.Parse(line);

                            if (numPoints > 3)
                            {
                                track.curvePts?.Clear();

                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    words = line.Split(',');
                                    vec3 vecPtt = new vec3(double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture),
                                        double.Parse(words[2], CultureInfo.InvariantCulture));
                                    track.curvePts.Add(vecPtt);
                                }
                            }

                            if (track.mode == TrackMode.AB && track.curvePts.Count == 0)
                            {
                                double designHeading = track.heading;

                                double hsin = Math.Sin(designHeading);
                                double hcos = Math.Cos(designHeading);

                                //fill in the dots between A and B
                                double len = glm.Distance(track.ptA, track.ptB);
                                if (len < 30)
                                {
                                    track.ptB.easting = track.ptA.easting + (Math.Sin(designHeading) * 30);
                                    track.ptB.northing = track.ptA.northing + (Math.Cos(designHeading) * 30);
                                }
                                track.curvePts.Add(new vec3(track.ptA, designHeading));
                                track.curvePts.Add(new vec3(track.ptB, designHeading));

                                trk.AddFirstLastPoints(ref track.curvePts, 100);
                            }

                            trk.AddTrack(track);
                        }

                        trk.GetNextTrack();
                    }
                    catch (Exception er)
                    {
                        TimedMessageBox(2000, gStr.Get(gs.gsCurveLineFileIsCorrupt), gStr.Get(gs.gsButFieldIsLoaded));
                        Log.EventWriter("Load Curve Line" + er.ToString());
                    }
                }
            }
        }

        public void FileLoadTrams()
        {
            string fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "Tram.txt");

            tram.tramBndOuterArr?.Clear();
            tram.tramBndInnerArr?.Clear();
            tram.tramList?.Clear();
            tram.displayMode = 0;
            btnTramDisplayMode.Visible = false;

            if (File.Exists(fileAndDirectory))
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        string line = reader.ReadLine();//$Tram

                        //outer track of boundary tram
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            int numPoints = int.Parse(line);

                            if (numPoints > 0)
                            {
                                //load the line
                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vec2 vecPt = new vec2(
                                    double.Parse(words[0], CultureInfo.InvariantCulture),
                                    double.Parse(words[1], CultureInfo.InvariantCulture));

                                    tram.tramBndOuterArr.Add(vecPt);
                                }
                                tram.displayMode = 1;
                            }

                            //inner track of boundary tram
                            line = reader.ReadLine();
                            numPoints = int.Parse(line);

                            if (numPoints > 0)
                            {
                                //load the line
                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vec2 vecPt = new vec2(
                                    double.Parse(words[0], CultureInfo.InvariantCulture),
                                    double.Parse(words[1], CultureInfo.InvariantCulture));

                                    tram.tramBndInnerArr.Add(vecPt);
                                }
                            }

                            if (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();
                                int numLines = int.Parse(line);

                                for (int k = 0; k < numLines; k++)
                                {
                                    line = reader.ReadLine();
                                    numPoints = int.Parse(line);
                                    if (numPoints > 1)
                                    {
                                        var tram = new List<vec2>(numPoints);
                                        for (int i = 0; i < numPoints; i++)
                                        {
                                            line = reader.ReadLine();
                                            string[] words = line.Split(',');
                                            vec2 vecPt = new vec2(
                                            double.Parse(words[0], CultureInfo.InvariantCulture),
                                            double.Parse(words[1], CultureInfo.InvariantCulture));

                                            tram.Add(vecPt);
                                        }
                                        this.tram.tramList.Add(tram);
                                    }
                                }
                            }
                        }

                        FixTramModeButton();
                    }

                    catch (Exception e)
                    {
                        TimedMessageBox(2000, "Tram is corrupt", gStr.Get(gs.gsButFieldIsLoaded));

                        Log.EventWriter("Load Boundary Line" + e.ToString());
                    }
                }
            }

        }

        #endregion

        #region Save Files

        public void ExportFieldAs_ISOXMLv3()
        {
            //if (bnd.bndList.Count < 1) return;//If no Bnd, Quit

            //get the directory and make sure it exists, create if not
            string dirField = Path.Combine(
                RegistrySettings.fieldsDirectory,
                currentFieldDirectory,
                "zISOXML",
                "v3"
                );

            if (!Directory.Exists(dirField))
            {
                Directory.CreateDirectory(dirField);
            }

            string myFileName = Path.Combine(dirField, "TASKDATA.xml");

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "  ";
                XmlWriter xml = XmlWriter.Create(myFileName, settings);

                xml.WriteStartElement("ISO11783_TaskData");//Settings
                xml.WriteAttributeString("DataTransferOrigin", "1");
                xml.WriteAttributeString("ManagementSoftwareManufacturer", "AOG");
                xml.WriteAttributeString("ManagementSoftwareVersion", "1.4.0");
                xml.WriteAttributeString("VersionMajor", "3");
                xml.WriteAttributeString("VersionMinor", "3");

                {
                    //PFD A = "Field ID" B = "Code" C = "Name" D = "Area sq m" E = "Customer Ref" F = "Farm Ref" >
                    xml.WriteStartElement("PFD");//Field
                    xml.WriteAttributeString("A", "PFD-1");
                    xml.WriteAttributeString("C", currentFieldDirectory);
                    xml.WriteAttributeString("D", ((int)(fd.areaOuterBoundary)).ToString());

                    double lat = 0;
                    double lon = 0;

                    {
                        //all the boundaries
                        /*
                        < PLN A = "1" C="Area in Sq M like 12568" >
                            < LSG A = "1" >
                                < PNT A = "2" C = "51.61918340" D = "4.51054560" />
                                < PNT A = "2" C = "51.61915460" D = "4.51056120" />
                            </ LSG >
                        </ PLN >
                        */
                        for (int i = 0; i < bnd.bndList.Count; i++)
                        {
                            xml.WriteStartElement("PLN");//BND

                            if (i == 0) xml.WriteAttributeString("A", "1"); //outerBnd
                            else xml.WriteAttributeString("A", "6");  //innerBnd

                            xml.WriteStartElement("LSG");//Polygon
                            xml.WriteAttributeString("A", "1");

                            for (int j = 0; j < bnd.bndList[i].fenceLineEar.Count; j++)
                            {

                                pn.ConvertLocalToWGS84(bnd.bndList[i].fenceLineEar[j].northing, bnd.bndList[i].fenceLineEar[j].easting, out lat, out lon);
                                xml.WriteStartElement("PNT");//Boundary Points
                                xml.WriteAttributeString("A", "2");
                                xml.WriteAttributeString("C", lat.ToString());
                                xml.WriteAttributeString("D", lon.ToString());
                                xml.WriteEndElement(); //Boundary Points                   
                            }

                            xml.WriteEndElement();//Polygon
                            xml.WriteEndElement();//BND
                        }

                        //all the headlands A=10
                        if (bnd.bndList.Count > 0)
                        {
                            foreach (var boundary in bnd.bndList)
                            {
                                if (boundary.hdLine.Count < 1) continue;

                                xml.WriteStartElement("PLN");//BND

                                xml.WriteAttributeString("A", "10"); //headland

                                xml.WriteStartElement("LSG");//Polygon
                                xml.WriteAttributeString("A", "1");

                                foreach (var hdPoints in boundary.hdLine)
                                {
                                    pn.ConvertLocalToWGS84(hdPoints.northing, hdPoints.easting, out lat, out lon);
                                    xml.WriteStartElement("PNT");//Boundary Points
                                    xml.WriteAttributeString("A", "2");
                                    xml.WriteAttributeString("C", lat.ToString());
                                    xml.WriteAttributeString("D", lon.ToString());
                                    xml.WriteEndElement(); //Boundary Points                   
                                }

                                xml.WriteEndElement();//Polygon
                                xml.WriteEndElement();//BND
                            }
                        }

                        //AB Lines
                        /*
                        LSG A = "5" B = "Line Name" >
                            < PNT A = "2" C = "51.61851540" D = "4.51137030" />
                            < PNT A = "2" C = "51.61912230" D = "4.51056060" />
                        </ LSG >
                        */

                        foreach (var track in trk.gArr)
                        {
                            if (track.mode != TrackMode.AB) continue;

                            xml.WriteStartElement("LSG");//Line
                            xml.WriteAttributeString("A", "5");
                            xml.WriteAttributeString("B", track.name);
                            ///xml.WriteAttributeString("C", (Settings.Tool.toolWidth).ToString());
                            {
                                xml.WriteStartElement("PNT");//A

                                pn.ConvertLocalToWGS84(track.ptA.northing - (Math.Cos(track.heading) * 1000),
                                    track.ptA.easting - (Math.Sin(track.heading) * 1000), out lat, out lon);

                                xml.WriteAttributeString("A", "2");
                                xml.WriteAttributeString("C", lat.ToString());
                                xml.WriteAttributeString("D", lon.ToString());

                                xml.WriteEndElement();//A
                                xml.WriteStartElement("PNT");//B

                                pn.ConvertLocalToWGS84(track.ptA.northing + (Math.Cos(track.heading) * 1000),
                                    track.ptA.easting + (Math.Sin(track.heading) * 1000), out lat, out lon);

                                xml.WriteAttributeString("A", "2");

                                xml.WriteAttributeString("C", lat.ToString());
                                xml.WriteAttributeString("D", lon.ToString());
                            }
                            xml.WriteEndElement();//B
                            xml.WriteEndElement();//Line
                        }

                        //curves
                        /*
                        LSG A = "5" B = "Name Here" >
                            < PNT A = "2" C = "51.61851540" D = "4.51137030" />
                            < PNT A = "2" C = "51.61912230" D = "4.51056060" />
                            < PNT A = "2" C = "51.61962230" D = "4.51056760" />
                        </ LSG >
                        */
                        foreach (var track in trk.gArr)
                        {
                            if (track.mode != TrackMode.Curve) continue;

                            xml.WriteStartElement("LSG");//Curve
                            xml.WriteAttributeString("A", "5"); //denotes guidance
                            xml.WriteAttributeString("B", track.name);
                            //xml.WriteAttributeString("C", (Settings.Tool.toolWidth).ToString());

                            for (int j = 0; j < track.curvePts.Count; j++)
                            {
                                xml.WriteStartElement("PNT");//point
                                pn.ConvertLocalToWGS84(track.curvePts[j].northing,
                                    track.curvePts[j].easting, out lat, out lon);

                                xml.WriteAttributeString("A", "2");
                                xml.WriteAttributeString("C", lat.ToString());
                                xml.WriteAttributeString("D", lon.ToString());

                                xml.WriteEndElement();//point
                            }
                            xml.WriteEndElement(); //Curve   
                        }
                    }

                    //Last
                    xml.WriteEndElement();//End Field
                }

                xml.WriteEndElement();//ISO11783_TaskData Settings

                xml.Flush();

                //Write the XML to file and close the kml
                xml.Close();

            }
            catch (Exception)
            {
                //throw;
            }
        }

        public void ExportFieldAs_ISOXMLv4()
        {
            int lineCounter = 0;

            //get the directory and make sure it exists, create if not
            string dirField = Path.Combine(
                RegistrySettings.fieldsDirectory,
                currentFieldDirectory,
                "zISOXML",
                "v4"
                );

            if (!Directory.Exists(dirField))
            {
                Directory.CreateDirectory(dirField);
            }

            string myFileName = Path.Combine(dirField, "TASKDATA.xml");

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "  ";
                XmlWriter xml = XmlWriter.Create(myFileName, settings);

                xml.WriteStartElement("ISO11783_TaskData");//Settings
                xml.WriteAttributeString("DataTransferOrigin", "1");
                xml.WriteAttributeString("ManagementSoftwareManufacturer", "AOG");
                xml.WriteAttributeString("ManagementSoftwareVersion", "1.4.0");
                xml.WriteAttributeString("VersionMajor", "4");
                xml.WriteAttributeString("VersionMinor", "2");

                {
                    //PFD A = "Field ID" B = "Code" C = "Name" D = "Area sq m" E = "Customer Ref" F = "Farm Ref" >
                    xml.WriteStartElement("PFD");//Field
                    xml.WriteAttributeString("A", "PFD-1");
                    xml.WriteAttributeString("C", currentFieldDirectory);
                    xml.WriteAttributeString("D", ((int)(fd.areaOuterBoundary)).ToString());

                    double lat = 0;
                    double lon = 0;

                    {
                        //all the boundaries
                        /*
                        < PLN A = "1" C="Area in Sq M like 12568" >
                            < LSG A = "1" >
                                < PNT A = "2" C = "51.61918340" D = "4.51054560" />
                                < PNT A = "2" C = "51.61915460" D = "4.51056120" />
                            </ LSG >
                        </ PLN >
                        */
                        for (int i = 0; i < bnd.bndList.Count; i++)
                        {
                            xml.WriteStartElement("PLN");//BND

                            if (i == 0) xml.WriteAttributeString("A", "1"); //outerBnd
                            else xml.WriteAttributeString("A", "6");  //innerBnd

                            xml.WriteStartElement("LSG");//Polygon
                            xml.WriteAttributeString("A", "1");

                            for (int j = 0; j < bnd.bndList[i].fenceLineEar.Count; j++)
                            {

                                pn.ConvertLocalToWGS84(bnd.bndList[i].fenceLineEar[j].northing, bnd.bndList[i].fenceLineEar[j].easting, out lat, out lon);
                                xml.WriteStartElement("PNT");//Boundary Points
                                xml.WriteAttributeString("A", "10");
                                xml.WriteAttributeString("C", lat.ToString());
                                xml.WriteAttributeString("D", lon.ToString());
                                xml.WriteEndElement(); //Boundary Points                   
                            }

                            xml.WriteEndElement();//Polygon
                            xml.WriteEndElement();//BND
                        }

                        //all the headlands A=10
                        foreach (var boundary in bnd.bndList)
                        {
                            if (boundary.hdLine.Count < 1) continue;

                            xml.WriteStartElement("PLN");//BND

                            xml.WriteAttributeString("A", "10"); //headland

                            xml.WriteStartElement("LSG");//Polygon
                            xml.WriteAttributeString("A", "1");

                            foreach (var hdPoint in boundary.hdLine)
                            {
                                pn.ConvertLocalToWGS84(hdPoint.northing, hdPoint.easting, out lat, out lon);
                                xml.WriteStartElement("PNT");//Boundary Points
                                xml.WriteAttributeString("A", "10");
                                xml.WriteAttributeString("C", lat.ToString());
                                xml.WriteAttributeString("D", lon.ToString());
                                xml.WriteEndElement(); //Boundary Points                   
                            }

                            xml.WriteEndElement();//Polygon
                            xml.WriteEndElement();//BND
                        }

                        //AB Lines
                        /*
                        LSG A = "5" B = "Line Name" >
                            < PNT A = "2" C = "51.61851540" D = "4.51137030" />
                            < PNT A = "2" C = "51.61912230" D = "4.51056060" />
                        </ LSG >
                        */

                        foreach (var track in trk.gArr)
                        {
                            if (track.mode != TrackMode.AB) continue;

                            xml.WriteStartElement("GGP");//Guide-P
                            string name = "GGP" + lineCounter.ToString();
                            lineCounter++;
                            xml.WriteAttributeString("A", name);
                            xml.WriteAttributeString("B", track.name);
                            {
                                xml.WriteStartElement("GPN");//Guide-N
                                xml.WriteAttributeString("A", name);
                                xml.WriteAttributeString("B", track.name);
                                xml.WriteAttributeString("C", "1");
                                xml.WriteAttributeString("E", "1");
                                xml.WriteAttributeString("F", "1");
                                xml.WriteAttributeString("I", "16");
                                {
                                    xml.WriteStartElement("LSG");//Line
                                    xml.WriteAttributeString("A", "5");
                                    {
                                        xml.WriteStartElement("PNT");//A

                                        pn.ConvertLocalToWGS84(track.ptA.northing - (Math.Cos(track.heading) * 1000),
                                            track.ptA.easting - (Math.Sin(track.heading) * 1000), out lat, out lon);

                                        xml.WriteAttributeString("A", "6");
                                        xml.WriteAttributeString("C", lat.ToString());
                                        xml.WriteAttributeString("D", lon.ToString());

                                        xml.WriteEndElement();//A
                                        xml.WriteStartElement("PNT");//B

                                        pn.ConvertLocalToWGS84(track.ptA.northing + (Math.Cos(track.heading) * 1000),
                                            track.ptA.easting + (Math.Sin(track.heading) * 1000), out lat, out lon);

                                        xml.WriteAttributeString("A", "7");

                                        xml.WriteAttributeString("C", lat.ToString());
                                        xml.WriteAttributeString("D", lon.ToString());
                                        xml.WriteEndElement();//B
                                    }
                                    xml.WriteEndElement();//Line
                                }
                                xml.WriteEndElement(); //Guide-N
                            }
                            xml.WriteEndElement(); //Guide-P
                        }

                        //curves
                        /*
                        LSG A = "5" B = "Name Here" >
                            < PNT A = "2" C = "51.61851540" D = "4.51137030" />
                            < PNT A = "2" C = "51.61912230" D = "4.51056060" />
                            < PNT A = "2" C = "51.61962230" D = "4.51056760" />
                        </ LSG >
                        */

                        foreach (var track in trk.gArr)
                        {
                            if (track.mode != TrackMode.Curve) continue;

                            xml.WriteStartElement("GGP");//Guide-P
                            string name = "GGP" + lineCounter.ToString();
                            lineCounter++;
                            xml.WriteAttributeString("A", name);
                            xml.WriteAttributeString("B", track.name);
                            {
                                xml.WriteStartElement("GPN");//Guide-N
                                xml.WriteAttributeString("A", name);
                                xml.WriteAttributeString("B", track.name);
                                xml.WriteAttributeString("C", "3");
                                xml.WriteAttributeString("E", "1");
                                xml.WriteAttributeString("F", "1");
                                xml.WriteAttributeString("I", "16");
                                {
                                    xml.WriteStartElement("LSG");//Curve
                                    xml.WriteAttributeString("A", "5"); //denotes guidance

                                    for (int j = 0; j < track.curvePts.Count; j++)
                                    {
                                        xml.WriteStartElement("PNT");//point
                                        pn.ConvertLocalToWGS84(track.curvePts[j].northing,
                                            track.curvePts[j].easting, out lat, out lon);
                                        if (j == 0)
                                        {
                                            xml.WriteAttributeString("A", "6");
                                        }
                                        else if (j == track.curvePts.Count - 1)
                                        {
                                            xml.WriteAttributeString("A", "7");
                                        }
                                        else
                                        {
                                            xml.WriteAttributeString("A", "9");
                                        }
                                        xml.WriteAttributeString("C", lat.ToString());
                                        xml.WriteAttributeString("D", lon.ToString());

                                        xml.WriteEndElement();//point
                                    }
                                    xml.WriteEndElement(); //end LSG curve
                                }
                                xml.WriteEndElement(); //Guide-N
                            }
                            xml.WriteEndElement(); //Guide-P
                        }
                    }

                    //Last
                    xml.WriteEndElement();//End Field
                }

                xml.WriteEndElement();//ISO11783_TaskData Settings

                xml.Flush();

                //Write the XML to file and close the kml
                xml.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            /*
                //xml.WriteStartElement("TSK");//Task
                //xml.WriteAttributeString("A", "TSK1");
                //xml.WriteAttributeString("B", "Tractor Work");
                //xml.WriteAttributeString("C", "CTR1");
                //xml.WriteAttributeString("E", "PFD-1");
                //xml.WriteAttributeString("G", "1");
                //xml.WriteAttributeString("I", "1");
                //xml.WriteAttributeString("J", "0");
                //xml.WriteEndElement();//Task
            */

        }


        //save the boundary
        public void FileSaveBoundary()
        {
            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "Boundary.Txt")))
            {
                writer.WriteLine("$Boundary");
                foreach (var boundary in bnd.bndList)
                {
                    writer.WriteLine(boundary.isDriveThru);
                    //writer.WriteLine(bnd.buildList[i].isDriveAround);

                    writer.WriteLine(boundary.fenceLine.Count.ToString(CultureInfo.InvariantCulture));
                    if (boundary.fenceLine.Count > 0)
                    {
                        foreach (var fence in boundary.fenceLine)
                            writer.WriteLine(Math.Round(fence.easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                             Math.Round(fence.northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                             Math.Round(fence.heading, 5).ToString(CultureInfo.InvariantCulture));
                    }
                }
            }

            fd.UpdateFieldBoundaryGUIAreas();

            CalculateSectionPatchesMinMax();
        }

        //save the contour points
        public void FileSaveContour()
        {
            //1  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude

            //make sure there is something to save
            if (contourSaveList.Count() > 0)
            {
                //Append the current list to the bndPts file
                using (StreamWriter writer = new StreamWriter(Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, currentJobDirectory, "Contour.txt"), true))
                {

                    //for every new chunk of patch in the whole section
                    foreach (var triList in contourSaveList)
                    {
                        writer.WriteLine(triList.Count.ToString(CultureInfo.InvariantCulture));

                        for (int i = 0; i < triList.Count; i++)
                        {
                            writer.WriteLine(Math.Round((triList[i].easting), 3).ToString(CultureInfo.InvariantCulture) + "," +
                                Math.Round(triList[i].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                Math.Round(triList[i].heading, 3).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }

                contourSaveList.Clear();
            }
        }

        //save nmea sentences
        public void FileSaveElevation()
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, currentJobDirectory, "Elevation.txt"), true))
            {
                writer.Write(sbElevationString.ToString());
            }
            sbElevationString.Clear();
        }

        //save all the flag markers
        public void FileSaveFlags()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FlagsDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //use Streamwriter to create and overwrite existing flag file
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "Flags.txt")))
            {
                try
                {
                    writer.WriteLine("$Flags");

                    writer.WriteLine(flagPts.Count);

                    foreach (var flag in flagPts)
                    {
                        writer.WriteLine(
                            flag.latitude.ToString(CultureInfo.InvariantCulture) + "," +
                            flag.longitude.ToString(CultureInfo.InvariantCulture) + "," +
                            flag.easting.ToString(CultureInfo.InvariantCulture) + "," +
                            flag.northing.ToString(CultureInfo.InvariantCulture) + "," +
                            flag.heading.ToString(CultureInfo.InvariantCulture) + "," +
                            flag.color.ToString(CultureInfo.InvariantCulture) + "," +
                            flag.ID.ToString(CultureInfo.InvariantCulture) + "," +
                            flag.notes);
                    }
                }

                catch (Exception e)
                {
                    TimedMessageBox(2000, "Error", e.Message + "\n Cannot write to file.");
                    Log.EventWriter("Saving Flags" + e.ToString());
                    return;
                }
            }
        }

        //save the headland
        public void FileSaveHeadland()
        {
            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "Headland.Txt")))
            {
                writer.WriteLine("$Headland");

                foreach (var boundary in bnd.bndList)
                {
                    writer.WriteLine(boundary.hdLine.Count.ToString(CultureInfo.InvariantCulture));
                    for (int j = 0; j < boundary.hdLine.Count; j++)
                        writer.WriteLine(Math.Round(boundary.hdLine[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                         Math.Round(boundary.hdLine[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                         Math.Round(boundary.hdLine[j].heading, 3).ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        public void FileSaveHeadLines()
        {
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if (!string.IsNullOrEmpty(directoryName) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = Path.Combine(directoryName, "Headlines.txt");

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                try
                {
                    writer.WriteLine("$HeadLines");

                    foreach (var headPath in hdl.tracksArr)
                    {
                        //write out the name
                        writer.WriteLine(headPath.name);

                        //write out the moveDistance
                        writer.WriteLine(headPath.moveDistance.ToString(CultureInfo.InvariantCulture));

                        //write out the mode
                        writer.WriteLine(((int)headPath.mode).ToString(CultureInfo.InvariantCulture));

                        //write out the A_Point index
                        writer.WriteLine(headPath.a_point.ToString(CultureInfo.InvariantCulture));

                        //write out the points of ref line
                        int cnt2 = headPath.trackPts.Count;

                        writer.WriteLine(cnt2.ToString(CultureInfo.InvariantCulture));
                        if (headPath.trackPts.Count > 0)
                        {
                            for (int j = 0; j < cnt2; j++)
                                writer.WriteLine(Math.Round(headPath.trackPts[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                 Math.Round(headPath.trackPts[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                 Math.Round(headPath.trackPts[j].heading, 5).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }
                catch (Exception er)
                {
                    Log.EventWriter("Saving Head Lines" + er.ToString());

                    return;
                }
            }
        }

        public void FileSaveSections()
        {
            //make sure there is something to save
            if (patchSaveList.Count() > 0)
            {
                //Append the current list to the bndPts file
                using (StreamWriter writer = new StreamWriter(Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, currentJobDirectory, "Sections.txt"), true))
                {
                    //for each patch, write out the list of triangles to the file
                    foreach (var triList in patchSaveList)
                    {
                        int count2 = triList.Count();
                        writer.WriteLine(count2.ToString(CultureInfo.InvariantCulture));

                        for (int i = 0; i < count2; i++)
                            writer.WriteLine((Math.Round(triList[i].easting, 3)).ToString(CultureInfo.InvariantCulture) +
                                "," + (Math.Round(triList[i].northing, 3)).ToString(CultureInfo.InvariantCulture) +
                                 "," + (Math.Round(triList[i].heading, 3)).ToString(CultureInfo.InvariantCulture));
                    }
                }

                //clear out that patchList and begin adding new ones for next save
                patchSaveList.Clear();
            }
        }

        public void FileSaveSystemEvents()
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(RegistrySettings.logsDirectory, "AOG_Events_Log.txt"), true))
            {
                writer.Write(Log.sbEvents);
                Log.sbEvents.Clear();
            }
        }

        public void FileSaveTracks()
        {
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if (!string.IsNullOrEmpty(directoryName) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = Path.Combine(directoryName, "TrackLines.txt");

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                try
                {
                    writer.WriteLine("$TrackLines");

                    foreach (var track in trk.gArr)
                    {
                        //write out the name
                        writer.WriteLine(track.name);

                        //write out the heading
                        writer.WriteLine(track.heading.ToString(CultureInfo.InvariantCulture));

                        //A nd B
                        writer.WriteLine(Math.Round(track.ptA.easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                            Math.Round(track.ptA.northing, 3).ToString(CultureInfo.InvariantCulture));
                        writer.WriteLine(Math.Round(track.ptB.easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                            Math.Round(track.ptB.northing, 3).ToString(CultureInfo.InvariantCulture));

                        //write out the nudgeDistance
                        writer.WriteLine(track.nudgeDistance.ToString(CultureInfo.InvariantCulture));

                        //write out the mode
                        writer.WriteLine(((int)track.mode).ToString(CultureInfo.InvariantCulture));

                        //visible?
                        writer.WriteLine(track.isVisible.ToString(CultureInfo.InvariantCulture));

                        //write out the points of ref line
                        int cnt2 = track.curvePts.Count;

                        writer.WriteLine(cnt2.ToString(CultureInfo.InvariantCulture));
                        if (track.curvePts.Count > 0)
                        {
                            for (int j = 0; j < cnt2; j++)
                                writer.WriteLine(Math.Round(track.curvePts[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                 Math.Round(track.curvePts[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                 Math.Round(track.curvePts[j].heading, 5).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }
                catch (Exception er)
                {
                    Log.EventWriter("Saving Curve Line" + er.ToString());

                    return;
                }
            }
        }

        //save tram
        public void FileSaveTram()
        {
            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "Tram.Txt")))
            {
                writer.WriteLine("$Tram");

                if (tram.tramBndOuterArr.Count > 0)
                {
                    //outer track of outer boundary tram
                    writer.WriteLine(tram.tramBndOuterArr.Count.ToString(CultureInfo.InvariantCulture));

                    for (int i = 0; i < tram.tramBndOuterArr.Count; i++)
                    {
                        writer.WriteLine(Math.Round(tram.tramBndOuterArr[i].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(tram.tramBndOuterArr[i].northing, 3).ToString(CultureInfo.InvariantCulture));
                    }

                    //inner track of outer boundary tram
                    writer.WriteLine(tram.tramBndInnerArr.Count.ToString(CultureInfo.InvariantCulture));

                    for (int i = 0; i < tram.tramBndInnerArr.Count; i++)
                    {
                        writer.WriteLine(Math.Round(tram.tramBndInnerArr[i].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(tram.tramBndInnerArr[i].northing, 3).ToString(CultureInfo.InvariantCulture));
                    }
                }

                //no outer bnd
                else
                {
                    writer.WriteLine("0");
                    writer.WriteLine("0");
                }

                if (tram.tramList.Count > 0)
                {
                    writer.WriteLine(tram.tramList.Count.ToString(CultureInfo.InvariantCulture));
                    for (int i = 0; i < tram.tramList.Count; i++)
                    {
                        writer.WriteLine(tram.tramList[i].Count.ToString(CultureInfo.InvariantCulture));
                        for (int h = 0; h < tram.tramList[i].Count; h++)
                        {
                            writer.WriteLine(Math.Round(tram.tramList[i][h].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                Math.Round(tram.tramList[i][h].northing, 3).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }
            }
        }

        #endregion

        #region KML

        //generate KML file from flags
        public void ExportFieldAs_KML()
        {
            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Field.kml";

            XmlTextWriter kml = new XmlTextWriter(Path.Combine(directoryName, myFileName), Encoding.UTF8);

            kml.Formatting = Formatting.Indented;
            kml.Indentation = 3;

            kml.WriteStartDocument();
            kml.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            kml.WriteStartElement("Document");

            //Description  ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Field Stats");
            kml.WriteElementString("description", fd.GetDescription());
            kml.WriteEndElement(); // <Folder>
            //End of Desc

            //Boundary  ----------------------------------------------------------------------
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Boundaries");

            for (int i = 0; i < bnd.bndList.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                if (i == 0) kml.WriteElementString("name", currentFieldDirectory);

                //lineStyle
                kml.WriteStartElement("Style");
                kml.WriteStartElement("LineStyle");
                if (i == 0) kml.WriteElementString("color", "ffdd00dd");
                else kml.WriteElementString("color", "ff4d3ffd");
                kml.WriteElementString("width", "4");
                kml.WriteEndElement(); // <LineStyle>

                kml.WriteStartElement("PolyStyle");
                if (i == 0) kml.WriteElementString("color", "407f3f55");
                else kml.WriteElementString("color", "703f38f1");
                kml.WriteEndElement(); // <PloyStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("Polygon");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("outerBoundaryIs");
                kml.WriteStartElement("LinearRing");

                //coords
                kml.WriteStartElement("coordinates");
                string bndPts = "";
                if (bnd.bndList[i].fenceLine.Count > 3)
                    bndPts = GetBoundaryPointsLatLon(bnd.bndList[i].fenceLine);
                kml.WriteRaw(bndPts);
                kml.WriteEndElement(); // <coordinates>

                kml.WriteEndElement(); // <Linear>
                kml.WriteEndElement(); // <OuterBoundary>
                kml.WriteEndElement(); // <Polygon>
                kml.WriteEndElement(); // <Placemark>
            }

            kml.WriteEndElement(); // <Folder>  
            //End of Boundary

            //guidance lines AB
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "AB_Lines");
            kml.WriteElementString("visibility", "0");

            string linePts = "";

            foreach (var track in trk.gArr)
            {
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("visibility", "0");

                kml.WriteElementString("name", track.name);
                kml.WriteStartElement("Style");

                kml.WriteStartElement("LineStyle");
                kml.WriteElementString("color", "ff0000ff");
                kml.WriteElementString("width", "2");
                kml.WriteEndElement(); // <LineStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("LineString");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("coordinates");

                if (track.mode == TrackMode.AB)
                {
                    for (int j = 0; j < track.curvePts.Count; j++)
                    {
                        linePts += pn.GetLocalToWSG84_KML(track.curvePts[j].easting, track.curvePts[j].northing);
                    }
                    kml.WriteRaw(linePts);
                }

                kml.WriteEndElement(); // <coordinates>
                kml.WriteEndElement(); // <LineString>

                kml.WriteEndElement(); // <Placemark>

            }
            kml.WriteEndElement(); // <Folder>   

            //guidance lines Curve
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Curve_Lines");
            kml.WriteElementString("visibility", "0");

            foreach (var track in trk.gArr)
            {
                linePts = "";
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("visibility", "0");

                kml.WriteElementString("name", track.name);
                kml.WriteStartElement("Style");

                kml.WriteStartElement("LineStyle");
                kml.WriteElementString("color", "ff6699ff");
                kml.WriteElementString("width", "2");
                kml.WriteEndElement(); // <LineStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("LineString");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("coordinates");

                if (track.mode != TrackMode.AB)
                {
                    for (int j = 0; j < track.curvePts.Count; j++)
                    {
                        linePts += pn.GetLocalToWSG84_KML(track.curvePts[j].easting, track.curvePts[j].northing);
                    }
                    kml.WriteRaw(linePts);
                }

                kml.WriteEndElement(); // <coordinates>
                kml.WriteEndElement(); // <LineString>

                kml.WriteEndElement(); // <Placemark>
            }
            kml.WriteEndElement(); // <Folder>   

            //Recorded Path
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Recorded Path");
            kml.WriteElementString("visibility", "1");

            linePts = "";
            kml.WriteStartElement("Placemark");
            kml.WriteElementString("visibility", "1");

            kml.WriteElementString("name", "Path " + 1);
            kml.WriteStartElement("Style");

            kml.WriteStartElement("LineStyle");
            kml.WriteElementString("color", "ff44ffff");
            kml.WriteElementString("width", "2");
            kml.WriteEndElement(); // <LineStyle>
            kml.WriteEndElement(); //Style

            kml.WriteStartElement("LineString");
            kml.WriteElementString("tessellate", "1");
            kml.WriteStartElement("coordinates");

            kml.WriteRaw(linePts);

            kml.WriteEndElement(); // <coordinates>
            kml.WriteEndElement(); // <LineString>

            kml.WriteEndElement(); // <Placemark>
            kml.WriteEndElement(); // <Folder>

            //flags  *************************************************************************
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Flags");

            for (int i = 0; i < flagPts.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("name", "Flag_" + i.ToString());

                kml.WriteStartElement("Style");
                kml.WriteStartElement("IconStyle");

                if (flagPts[i].color == 0)  //red - xbgr
                    kml.WriteElementString("color", "ff4400ff");
                if (flagPts[i].color == 1)  //grn - xbgr
                    kml.WriteElementString("color", "ff44ff00");
                if (flagPts[i].color == 2)  //yel - xbgr
                    kml.WriteElementString("color", "ff44ffff");

                kml.WriteEndElement(); //IconStyle
                kml.WriteEndElement(); //Style

                kml.WriteElementString("name", ((i + 1).ToString() + " " + flagPts[i].notes));
                kml.WriteStartElement("Point");
                kml.WriteElementString("coordinates", flagPts[i].longitude.ToString(CultureInfo.InvariantCulture) +
                    "," + flagPts[i].latitude.ToString(CultureInfo.InvariantCulture) + ",0");
                kml.WriteEndElement(); //Point
                kml.WriteEndElement(); // <Placemark>
            }
            kml.WriteEndElement(); // <Folder>   
            //End of Flags

            //Sections  ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Sections");
            //kml.WriteElementString("description", fd.GetDescription() );

            string secPts = "";
            int cntr = 0;

            //for every new chunk of patch
            foreach (var triList in patchList)
            {
                if (triList.Count > 0)
                {
                    kml.WriteStartElement("Placemark");
                    kml.WriteElementString("name", "Sections_" + cntr.ToString());
                    cntr++;

                    string collor = "F0" + ((byte)(triList[0].heading)).ToString("X2") +
                        ((byte)(triList[0].northing)).ToString("X2") + ((byte)(triList[0].easting)).ToString("X2");

                    //lineStyle
                    kml.WriteStartElement("Style");

                    kml.WriteStartElement("LineStyle");
                    kml.WriteElementString("color", collor);
                    //kml.WriteElementString("width", "6");
                    kml.WriteEndElement(); // <LineStyle>

                    kml.WriteStartElement("PolyStyle");
                    kml.WriteElementString("color", collor);
                    kml.WriteEndElement(); // <PloyStyle>
                    kml.WriteEndElement(); //Style

                    kml.WriteStartElement("Polygon");
                    kml.WriteElementString("tessellate", "1");
                    kml.WriteStartElement("outerBoundaryIs");
                    kml.WriteStartElement("LinearRing");

                    //coords
                    kml.WriteStartElement("coordinates");
                    secPts = "";
                    for (int i = 1; i < triList.Count; i += 2)
                    {
                        secPts += pn.GetLocalToWSG84_KML(triList[i].easting, triList[i].northing);
                    }
                    for (int i = triList.Count - 1; i > 1; i -= 2)
                    {
                        secPts += pn.GetLocalToWSG84_KML(triList[i].easting, triList[i].northing);
                    }
                    secPts += pn.GetLocalToWSG84_KML(triList[1].easting, triList[1].northing);

                    kml.WriteRaw(secPts);
                    kml.WriteEndElement(); // <coordinates>

                    kml.WriteEndElement(); // <LinearRing>
                    kml.WriteEndElement(); // <outerBoundaryIs>
                    kml.WriteEndElement(); // <Polygon>

                    kml.WriteEndElement(); // <Placemark>
                }
            }
            kml.WriteEndElement(); // <Folder>
            //End of sections

            //end of document
            kml.WriteEndElement(); // <Document>
            kml.WriteEndElement(); // <kml>

            //The end
            kml.WriteEndDocument();

            kml.Flush();

            //Write the XML to file and close the kml
            kml.Close();
        }

        public string GetBoundaryPointsLatLon(List<vec3> points)
        {
            StringBuilder sb = new StringBuilder();

            foreach (vec3 point in points)
            {
                double lat = 0;
                double lon = 0;

                pn.ConvertLocalToWGS84(point.northing, point.easting, out lat, out lon);

                sb.Append(lon.ToString("N7", CultureInfo.InvariantCulture) + ',' + lat.ToString("N7", CultureInfo.InvariantCulture) + ",0 ");
            }
            return sb.ToString();
        }

        //generate KML file from flag
        public void FileMakeKMLFromCurrentPosition(double lat, double lon)
        {
            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }


            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "CurrentPosition.kml")))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                writer.WriteLine(@"<color>ff4400ff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> Your Current Position </name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                lon.ToString(CultureInfo.InvariantCulture) + "," + lat.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }

        //generate KML file from flag
        public void FileSaveSingleFlagKML(int flagNumber)
        {

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flag.kml";

            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
            {
                //match new fix to current position

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                if (flagPts[flagNumber - 1].color == 0)  //red - xbgr
                    writer.WriteLine(@"<color>ff4400ff</color>");
                if (flagPts[flagNumber - 1].color == 1)  //grn - xbgr
                    writer.WriteLine(@"<color>ff44ff00</color>");
                if (flagPts[flagNumber - 1].color == 2)  //yel - xbgr
                    writer.WriteLine(@"<color>ff44ffff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> " + flagNumber.ToString(CultureInfo.InvariantCulture) + @"</name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                flagPts[flagNumber - 1].longitude.ToString(CultureInfo.InvariantCulture) + "," + flagPts[flagNumber - 1].latitude.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }
        private void FileUpdateAllFieldsKML()
        {

            //get the directory and make sure it exists, create if not
            string directoryName = RegistrySettings.fieldsDirectory;

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            {
                return; //We have no fields to aggregate.
            }

            string myFileName;
            myFileName = "AllFields.kml";

            XmlTextWriter kml = new XmlTextWriter(Path.Combine(directoryName, myFileName), Encoding.UTF8);

            kml.Formatting = Formatting.Indented;
            kml.Indentation = 3;

            kml.WriteStartDocument();
            kml.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            kml.WriteStartElement("Document");

            foreach (String dir in Directory.EnumerateDirectories(directoryName).OrderBy(d => new DirectoryInfo(d).Name).ToArray())
            //loop
            {
                if (!File.Exists(Path.Combine(dir, "Field.kml"))) continue;

                directoryName = Path.GetFileName(dir);
                kml.WriteStartElement("Folder");
                kml.WriteElementString("name", directoryName);

                var lines = File.ReadAllLines(Path.Combine(dir, "Field.kml"));
                LinkedList<string> linebuffer = new LinkedList<string>();
                for (var i = 3; i < lines.Length - 2; i++)  //We want to skip the first 3 and last 2 lines
                {
                    linebuffer.AddLast(lines[i]);
                    if (linebuffer.Count > 2)
                    {
                        kml.WriteRaw("   ");
                        kml.WriteRaw(Environment.NewLine);
                        kml.WriteRaw(linebuffer.First.Value);
                        linebuffer.RemoveFirst();
                    }
                }
                kml.WriteRaw("   ");
                kml.WriteRaw(Environment.NewLine);
                kml.WriteRaw(linebuffer.First.Value);
                linebuffer.RemoveFirst();
                kml.WriteRaw("   ");
                kml.WriteRaw(Environment.NewLine);
                kml.WriteRaw(linebuffer.First.Value);
                kml.WriteRaw(Environment.NewLine);

                kml.WriteEndElement(); // <Folder>
                kml.WriteComment("End of " + directoryName);
            }

            //end of document
            kml.WriteEndElement(); // <Document>
            kml.WriteEndElement(); // <kml>

            //The end
            kml.WriteEndDocument();

            kml.Flush();

            //Write the XML to file and close the kml
            kml.Close();

        }

        #endregion
        public void FileNewFromAgShare()
        {
            FileCreateField();
            FileCreateBoundary();
            FileCreateFlags();
            FileSaveTracks();
        }
    }
}