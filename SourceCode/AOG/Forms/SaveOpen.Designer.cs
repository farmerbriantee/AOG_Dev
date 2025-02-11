﻿using OpenTK.Graphics.OpenGL;
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
using AgOpenGPS.Culture;

namespace AgOpenGPS
{
    
    public partial class FormGPS
    {
        //list of the list of patch data individual triangles for contour tracking
        public List<List<vec3>> contourSaveList = new List<List<vec3>>();

        //list of the list of patch data individual triangles for field sections
        public List<List<vec3>> patchSaveList = new List<List<vec3>>();

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
            {
                //write paths # of sections
                //writer.WriteLine("$Sectionsv4");
            }
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

        //creates the field file when starting new field
        public void FileCreateField()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            if (!isFieldStarted)
            {
                TimedMessageBox(3000, gStr.gsFieldNotOpen, gStr.gsCreateNewField);
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

        //function to open a previously saved field, resume, open exisiting, open named field
        public void FileOpenField(string _openType)
        {
            string fileAndDirectory = "";
            if (_openType.Contains("Field.txt"))
            {
                fileAndDirectory = _openType;
                _openType = "Load";
            }

            else fileAndDirectory = "Cancel";

            //get the directory where the fields are stored
            //string directoryName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+ "\\fields\\";
            switch (_openType)
            {
                case "Resume":
                    {
                        //Either exit or update running save
                        fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "Field.txt");
                        if (!File.Exists(fileAndDirectory)) fileAndDirectory = "Cancel";
                        break;
                    }

                case "Open":
                    {
                        //create the dialog instance
                        OpenFileDialog ofd = new OpenFileDialog();

                        //the initial directory, fields, for the open dialog
                        ofd.InitialDirectory = RegistrySettings.fieldsDirectory;

                        //When leaving dialog put windows back where it was
                        ofd.RestoreDirectory = true;

                        //set the filter to text files only
                        ofd.Filter = "Field files (Field.txt)|Field.txt";

                        //was a file selected
                        if (ofd.ShowDialog(this) == DialogResult.Cancel) fileAndDirectory = "Cancel";
                        else fileAndDirectory = ofd.FileName;
                        break;
                    }
            }

            if (fileAndDirectory == "Cancel") return;

            //close the existing job and reset everything
            this.FieldClose();

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

                    //read field directory
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

                        CNMEA.latStart = double.Parse(offs[0], CultureInfo.InvariantCulture);
                        CNMEA.lonStart = double.Parse(offs[1], CultureInfo.InvariantCulture);

                        pn.SetLocalMetersPerDegree(true);
                    }
                }

                catch (Exception e)
                {
                    Log.EventWriter("While Opening Field" + e.ToString());

                    TimedMessageBox(2000, gStr.gsFieldFileIsCorrupt, gStr.gsChooseADifferentField);


                    FieldClose();
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
            SetZoom();

            //update field data
            oglZoom.Refresh();

        }

        public void FileLoadBoundaries()
        {
            string fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "Boundary.txt");
            if (!File.Exists(fileAndDirectory))
            {
                TimedMessageBox(2000, gStr.gsMissingBoundaryFile, gStr.gsButFieldIsLoaded);

            }
            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {

                        //read header
                        string line = reader.ReadLine();//Boundary

                        for (int k = 0; true; k++)
                        {
                            if (reader.EndOfStream) break;

                            CBoundaryList New = new CBoundaryList();

                            //True or False OR points from older boundary files
                            line = reader.ReadLine();

                            //Check for older boundary files, then above line string is num of points
                            if (line == "True" || line == "False")
                            {
                                New.isDriveThru = bool.Parse(line);
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

                                    New.fenceLine.Add(vecPt);
                                }

                                New.CalculateFenceArea(k);

                                double delta = 0;
                                New.fenceLineEar?.Clear();

                                for (int i = 0; i < New.fenceLine.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        New.fenceLineEar.Add(new vec2(New.fenceLine[i].easting, New.fenceLine[i].northing));
                                        continue;
                                    }
                                    delta += (New.fenceLine[i - 1].heading - New.fenceLine[i].heading);
                                    if (Math.Abs(delta) > 0.005)
                                    {
                                        New.fenceLineEar.Add(new vec2(New.fenceLine[i].easting, New.fenceLine[i].northing));
                                        delta = 0;
                                    }
                                }

                                //Triangulate the bundary polygon
                                CPolygon bndPolygon = new CPolygon(New.fenceLineEar.ToArray());
                                New.fenceTriangleList = bndPolygon.Triangulate();

                                bnd.bndList.Add(New);
                            }
                        }

                        CalculateSectionPatchesMinMax();
                        bnd.BuildTurnLines();
                        if (bnd.bndList.Count > 0) btnABDraw.Visible = true;
                    }

                    catch (Exception e)
                    {
                        TimedMessageBox(2000, gStr.gsBoundaryLineFilesAreCorrupt, gStr.gsButFieldIsLoaded);

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
                TimedMessageBox(2000, gStr.gsMissingFlagsFile, gStr.gsButFieldIsLoaded);

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
                        TimedMessageBox(2000, gStr.gsFlagFileIsCorrupt, gStr.gsButFieldIsLoaded);

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

            int sett = Properties.Settings.Default.setArdMac_setting0;
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

            //get the file of previous AB Lines
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            if (!File.Exists(filename))
            {
                TimedMessageBox(2000, gStr.gsFileError, "Missing Headlines File");
                Log.EventWriter("Load Field, Missing Headlines File");
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

                            hdl.tracksArr.Add(new CHeadPath());
                            hdl.idx = hdl.tracksArr.Count - 1;

                            //read header $CurveLine
                            hdl.tracksArr[hdl.idx].name = reader.ReadLine();

                            line = reader.ReadLine();
                            hdl.tracksArr[hdl.idx].moveDistance = double.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            hdl.tracksArr[hdl.idx].mode = int.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            hdl.tracksArr[hdl.idx].a_point = int.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            int numPoints = int.Parse(line);

                            if (numPoints > 3)
                            {
                                hdl.tracksArr[hdl.idx].trackPts?.Clear();

                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vec3 vecPt = new vec3(double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture),
                                        double.Parse(words[2], CultureInfo.InvariantCulture));
                                    hdl.tracksArr[hdl.idx].trackPts.Add(vecPt);
                                }
                            }
                            else
                            {
                                if (hdl.tracksArr.Count > 0)
                                {
                                    hdl.tracksArr.RemoveAt(hdl.idx);
                                }
                            }
                        }
                    }

                    catch (Exception er)
                    {
                        hdl.tracksArr?.Clear();

                        TimedMessageBox(2000, "Headline Error", "Lines Deleted");

                        directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

                        if (!string.IsNullOrEmpty(directoryName) && (!Directory.Exists(directoryName)))
                        { Directory.CreateDirectory(directoryName); }

                        filename = Path.Combine(directoryName, "Headlines.txt");

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

            hdl.idx = -1;
        }

        public void FileLoadContour(string dir)
        {
            if (!File.Exists(dir))
            {
                string myFileName = "Contour.txt";

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

                            ct.ptList = new List<vec3>();
                            ct.ptList.Capacity = verts + 1;
                            ct.stripList.Add(ct.ptList);

                            for (int v = 0; v < verts; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                vecFix.easting = double.Parse(words[0], CultureInfo.InvariantCulture);
                                vecFix.northing = double.Parse(words[1], CultureInfo.InvariantCulture);
                                vecFix.heading = double.Parse(words[2], CultureInfo.InvariantCulture);
                                ct.ptList.Add(vecFix);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.EventWriter("Loading Contour file" + e.ToString());

                        TimedMessageBox(2000, gStr.gsContourFileIsCorrupt, gStr.gsButFieldIsLoaded);

                    }
                }
            }
        }

        public void FileLoadSections(string dir)
        {
            if (!File.Exists(dir))
            {
                string myFileName = "Sections.txt";

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
                bool isv3 = false;
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
                            if (line.Contains("ect"))
                            {
                                isv3 = true;
                                break;
                            }
                            int verts = int.Parse(line);

                            triStrip[0].triangleList = new List<vec3>();
                            triStrip[0].triangleList.Capacity = verts + 1;

                            triStrip[0].patchList.Add(triStrip[0].triangleList);


                            for (int v = 0; v < verts; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                vecFix.easting = double.Parse(words[0], CultureInfo.InvariantCulture);
                                vecFix.northing = double.Parse(words[1], CultureInfo.InvariantCulture);
                                vecFix.heading = double.Parse(words[2], CultureInfo.InvariantCulture);
                                triStrip[0].triangleList.Add(vecFix);
                            }

                            //calculate area of this patch - AbsoluteValue of (Ax(By-Cy) + Bx(Cy-Ay) + Cx(Ay-By)/2)
                            verts -= 2;
                            if (verts >= 2)
                            {
                                for (int j = 1; j < verts; j++)
                                {
                                    double temp = 0;
                                    temp = triStrip[0].triangleList[j].easting * (triStrip[0].triangleList[j + 1].northing - triStrip[0].triangleList[j + 2].northing) +
                                              triStrip[0].triangleList[j + 1].easting * (triStrip[0].triangleList[j + 2].northing - triStrip[0].triangleList[j].northing) +
                                                  triStrip[0].triangleList[j + 2].easting * (triStrip[0].triangleList[j].northing - triStrip[0].triangleList[j + 1].northing);

                                    fd.workedAreaTotal += Math.Abs((temp * 0.5));
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.EventWriter("Section file" + e.ToString());

                        TimedMessageBox(2000, "Section File is Corrupt", gStr.gsButFieldIsLoaded);

                    }

                }
            }
        }

        public void FileLoadTracks()
        {
            trk.gArr?.Clear();

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
                TimedMessageBox(2000, gStr.gsFileError, "Missing Tracks File");
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

                            trk.gArr.Add(new CTrk());
                            trk.idx = trk.gArr.Count - 1;

                            //read header $CurveLine
                            trk.gArr[trk.idx].name = reader.ReadLine();
                            // get the average heading
                            line = reader.ReadLine();
                            trk.gArr[trk.idx].heading = double.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            string[] words = line.Split(',');
                            vec2 vecPt = new vec2(double.Parse(words[0], CultureInfo.InvariantCulture),
                                double.Parse(words[1], CultureInfo.InvariantCulture));
                            trk.gArr[trk.idx].ptA = (vecPt);

                            line = reader.ReadLine();
                            words = line.Split(',');
                            vecPt = new vec2(double.Parse(words[0], CultureInfo.InvariantCulture),
                                double.Parse(words[1], CultureInfo.InvariantCulture));
                            trk.gArr[trk.idx].ptB = (vecPt);

                            line = reader.ReadLine();
                            trk.gArr[trk.idx].nudgeDistance = double.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            trk.gArr[trk.idx].mode = (TrackMode)int.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            trk.gArr[trk.idx].isVisible = bool.Parse(line);

                            line = reader.ReadLine();
                            int numPoints = int.Parse(line);

                            if (numPoints > 3)
                            {
                                trk.gArr[trk.idx].curvePts?.Clear();

                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    words = line.Split(',');
                                    vec3 vecPtt = new vec3(double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture),
                                        double.Parse(words[2], CultureInfo.InvariantCulture));
                                    trk.gArr[trk.idx].curvePts.Add(vecPtt);
                                }
                            }

                            if (trk.gArr[trk.idx].mode == TrackMode.AB && trk.gArr[trk.idx].curvePts.Count == 0)
                            {
                                double designHeading = trk.gArr[trk.idx].heading;

                                double hsin = Math.Sin(designHeading);
                                double hcos = Math.Cos(designHeading);

                                //fill in the dots between A and B
                                double len = glm.Distance(trk.gArr[trk.idx].ptA, trk.gArr[trk.idx].ptB);
                                if (len < 30)
                                {
                                    trk.gArr[trk.idx].ptB.easting = trk.gArr[trk.idx].ptA.easting + (Math.Sin(designHeading) * 30);
                                    trk.gArr[trk.idx].ptB.northing = trk.gArr[trk.idx].ptA.northing + (Math.Cos(designHeading) * 30);
                                }
                                len = glm.Distance(trk.gArr[trk.idx].ptA, trk.gArr[trk.idx].ptB);

                                vec3 P1 = new vec3();
                                for (int i = 0; i < (int)len; i += 1)
                                {
                                    P1.easting = (hsin * i) + trk.gArr[trk.idx].ptA.easting;
                                    P1.northing = (hcos * i) + trk.gArr[trk.idx].ptA.northing;
                                    P1.heading = designHeading;
                                    trk.gArr[trk.idx].curvePts.Add(P1);
                                }

                                trk.AddFirstLastPoints(ref trk.gArr[trk.idx].curvePts, 50);
                            }
                        }
                    }
                    catch (Exception er)
                    {
                        TimedMessageBox(2000, gStr.gsCurveLineFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        Log.EventWriter("Load Curve Line" + er.ToString());
                    }
                }
            }

            trk.idx = -1;
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

                                    tram.tramArr = new List<vec2>();
                                    tram.tramList.Add(tram.tramArr);

                                    for (int i = 0; i < numPoints; i++)
                                    {
                                        line = reader.ReadLine();
                                        string[] words = line.Split(',');
                                        vec2 vecPt = new vec2(
                                        double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture));

                                        tram.tramArr.Add(vecPt);
                                    }
                                }
                            }
                        }

                        FixTramModeButton();
                    }

                    catch (Exception e)
                    {
                        TimedMessageBox(2000, "Tram is corrupt", gStr.gsButFieldIsLoaded);

                        Log.EventWriter("Load Boundary Line" + e.ToString());
                    }
                }
            }

        }

        #endregion

        #region Save Files

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
                for (int i = 0; i < bnd.bndList.Count; i++)
                {
                    writer.WriteLine(bnd.bndList[i].isDriveThru);
                    //writer.WriteLine(bnd.buildList[i].isDriveAround);

                    writer.WriteLine(bnd.bndList[i].fenceLine.Count.ToString(CultureInfo.InvariantCulture));
                    if (bnd.bndList[i].fenceLine.Count > 0)
                    {
                        for (int j = 0; j < bnd.bndList[i].fenceLine.Count; j++)
                            writer.WriteLine(Math.Round(bnd.bndList[i].fenceLine[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                Math.Round(bnd.bndList[i].fenceLine[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                    Math.Round(bnd.bndList[i].fenceLine[j].heading, 5).ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
        }

        //save the contour points
        public void FileSaveContour()
        {
            //1  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude

            //make sure there is something to save
            if (contourSaveList.Count() > 0)
            {
                //Append the current list to the field file
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
                writer.Write(sbGrid.ToString());
            }
            sbGrid.Clear();
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

                    int count2 = flagPts.Count;
                    writer.WriteLine(count2);

                    for (int i = 0; i < count2; i++)
                    {
                        writer.WriteLine(
                            flagPts[i].latitude.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].longitude.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].easting.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].northing.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].heading.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].color.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].ID.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].notes);
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

                if (bnd.bndList.Count > 0 && bnd.bndList[0].hdLine.Count > 0)
                {
                    for (int i = 0; i < bnd.bndList.Count; i++)
                    {
                        writer.WriteLine(bnd.bndList[i].hdLine.Count.ToString(CultureInfo.InvariantCulture));
                        if (bnd.bndList[0].hdLine.Count > 0)
                        {
                            for (int j = 0; j < bnd.bndList[i].hdLine.Count; j++)
                                writer.WriteLine(Math.Round(bnd.bndList[i].hdLine[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                    Math.Round(bnd.bndList[i].hdLine[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                    Math.Round(bnd.bndList[i].hdLine[j].heading, 3).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }
            }
        }

        public void FileSaveHeadLines()
        {
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if (!string.IsNullOrEmpty(directoryName) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = Path.Combine(directoryName, "Headlines.txt");

            int cnt = hdl.tracksArr.Count;

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                try
                {
                    if (cnt > 0)
                    {
                        writer.WriteLine("$HeadLines");

                        for (int i = 0; i < cnt; i++)
                        {
                            //write out the name
                            writer.WriteLine(hdl.tracksArr[i].name);


                            //write out the moveDistance
                            writer.WriteLine(hdl.tracksArr[i].moveDistance.ToString(CultureInfo.InvariantCulture));

                            //write out the mode
                            writer.WriteLine(hdl.tracksArr[i].mode.ToString(CultureInfo.InvariantCulture));
                            
                            //write out the A_Point index
                            writer.WriteLine(hdl.tracksArr[i].a_point.ToString(CultureInfo.InvariantCulture));

                            //write out the points of ref line
                            int cnt2 = hdl.tracksArr[i].trackPts.Count;

                            writer.WriteLine(cnt2.ToString(CultureInfo.InvariantCulture));
                            if (hdl.tracksArr[i].trackPts.Count > 0)
                            {
                                for (int j = 0; j < cnt2; j++)
                                    writer.WriteLine(Math.Round(hdl.tracksArr[i].trackPts[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                        Math.Round(hdl.tracksArr[i].trackPts[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                            Math.Round(hdl.tracksArr[i].trackPts[j].heading, 5).ToString(CultureInfo.InvariantCulture));
                            }
                        }
                    }
                    else
                    {
                        writer.WriteLine("$HeadLines");
                        return;
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
                //Append the current list to the field file
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
            using (StreamWriter writer = new StreamWriter(Path.Combine(RegistrySettings.logsDirectory, "AgOpenGPS_Events_Log.txt"), true))
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

            int cnt = trk.gArr.Count;

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                try
                {
                    if (cnt > 0)
                    {
                        writer.WriteLine("$TrackLines");

                        for (int i = 0; i < cnt; i++)
                        {
                            //write out the name
                            writer.WriteLine(trk.gArr[i].name);

                            //write out the heading
                            writer.WriteLine(trk.gArr[i].heading.ToString(CultureInfo.InvariantCulture));

                            //A nd B
                            writer.WriteLine(Math.Round(trk.gArr[i].ptA.easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                Math.Round(trk.gArr[i].ptA.northing, 3).ToString(CultureInfo.InvariantCulture));
                            writer.WriteLine(Math.Round(trk.gArr[i].ptB.easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                Math.Round(trk.gArr[i].ptB.northing, 3).ToString(CultureInfo.InvariantCulture));

                            //write out the nudgeDistance
                            writer.WriteLine(trk.gArr[i].nudgeDistance.ToString(CultureInfo.InvariantCulture));

                            //write out the mode
                            writer.WriteLine(((int)trk.gArr[i].mode).ToString(CultureInfo.InvariantCulture));

                            //visible?
                            writer.WriteLine(trk.gArr[i].isVisible.ToString(CultureInfo.InvariantCulture));

                            //write out the points of ref line
                            int cnt2 = trk.gArr[i].curvePts.Count;

                            writer.WriteLine(cnt2.ToString(CultureInfo.InvariantCulture));
                            if (trk.gArr[i].curvePts.Count > 0)
                            {
                                for (int j = 0; j < cnt2; j++)
                                    writer.WriteLine(Math.Round(trk.gArr[i].curvePts[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                        Math.Round(trk.gArr[i].curvePts[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                            Math.Round(trk.gArr[i].curvePts[j].heading, 5).ToString(CultureInfo.InvariantCulture));
                            }
                        }
                    }
                    else
                    {
                        writer.WriteLine("$TrackLines");
                        return;
                    }
                }
                catch (Exception er)
                {
                    Log.EventWriter("Saving Curve Line" + er.ToString());

                    return;
                }
            }

            if (trk.idx > (trk.gArr.Count - 1)) trk.idx = trk.gArr.Count - 1;

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
                    bndPts = GetBoundaryPointsLatLon(i);
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

            for (int i = 0; i < trk.gArr.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("visibility", "0");

                kml.WriteElementString("name", trk.gArr[i].name);
                kml.WriteStartElement("Style");

                kml.WriteStartElement("LineStyle");
                kml.WriteElementString("color", "ff0000ff");
                kml.WriteElementString("width", "2");
                kml.WriteEndElement(); // <LineStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("LineString");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("coordinates");

                if (trk.gArr[i].mode == TrackMode.AB)
                {
                    for (int j = 0; j < trk.gArr[i].curvePts.Count; j++)
                    {
                        linePts += pn.GetLocalToWSG84_KML(trk.gArr[i].curvePts[j].easting, trk.gArr[i].curvePts[j].northing);
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

            for (int i = 0; i < trk.gArr.Count; i++)
            {
                linePts = "";
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("visibility", "0");

                kml.WriteElementString("name", trk.gArr[i].name);
                kml.WriteStartElement("Style");

                kml.WriteStartElement("LineStyle");
                kml.WriteElementString("color", "ff6699ff");
                kml.WriteElementString("width", "2");
                kml.WriteEndElement(); // <LineStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("LineString");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("coordinates");

                if (trk.gArr[i].mode != TrackMode.AB)
                {
                    for (int j = 0; j < trk.gArr[i].curvePts.Count; j++)
                    {
                        linePts += pn.GetLocalToWSG84_KML(trk.gArr[i].curvePts[j].easting, trk.gArr[i].curvePts[j].northing);
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

            for (int j = 0; j < triStrip.Count; j++)
            {
                int patches = triStrip[j].patchList.Count;

                if (patches > 0)
                {
                    //for every new chunk of patch
                    foreach (var triList in triStrip[j].patchList)
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

        public string GetBoundaryPointsLatLon(int bndNum)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bnd.bndList[bndNum].fenceLine.Count; i++)
            {
                double lat = 0;
                double lon = 0;

                pn.ConvertLocalToWGS84(bnd.bndList[bndNum].fenceLine[i].northing, bnd.bndList[bndNum].fenceLine[i].easting, out lat, out lon);

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

            foreach(String dir in Directory.EnumerateDirectories(directoryName).OrderBy(d => new DirectoryInfo(d).Name).ToArray())
            //loop
            {
                if (!File.Exists(Path.Combine(dir, "Field.kml"))) continue;

                directoryName = Path.GetFileName(dir);
                kml.WriteStartElement("Folder");
                kml.WriteElementString("name", directoryName);

                var lines = File.ReadAllLines(Path.Combine(dir, "Field.kml"));
                LinkedList<string> linebuffer = new LinkedList<string>();
                for( var i = 3; i < lines.Length-2; i++)  //We want to skip the first 3 and last 2 lines
                {
                    linebuffer.AddLast(lines[i]);
                    if(linebuffer.Count > 2)
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
                kml.WriteComment("End of " +directoryName);
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
    }
}