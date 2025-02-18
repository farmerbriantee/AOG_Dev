﻿using AgOpenGPS.Culture;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace AgOpenGPS
{
    public partial class FormFieldISOXML : Form
    {
        //class variables
        private readonly FormGPS mf;

        private double easting, norting, lonK, latK;

        private XmlDocument iso;

        private string xmlFilename;
        private XmlNodeList pfd;

        private int idxFieldSelected;

        public FormFieldISOXML(Form _callingForm)
        {
            //get copy of the calling main form
            mf = _callingForm as FormGPS;

            InitializeComponent();
        }

        private void FormFieldISOXML_Load(object sender, EventArgs e)
        {
            tboxFieldName.Text = "";
            btnBuildFields.Enabled = false;

            label1.Text = gStr.gsEditFieldName;

            this.Text = gStr.gsCreateNewField;

            lblField.Text = gStr.gsBasedOnField;

            tree.Nodes?.Clear();

            //create the dialog instance
            OpenFileDialog ofd = new OpenFileDialog
            {
                //set the filter to text KML only
                Filter = "XML files (*.XML)|*.XML",

                //the initial directory, fields, for the open dialog
                InitialDirectory = RegistrySettings.fieldsDirectory
            };

            //was a file selected
            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                xmlFilename = ofd.FileName;
                //xmlFilename = "C:\\Users\\Grizs\\Documents\\AgOpenGPS\\Fields\\xml\\TASKDATARich3.XML";

                iso = new XmlDocument
                {
                    PreserveWhitespace = false
                };
                iso.Load(xmlFilename);

                //Partial Field Group
                //PFD - A = Field ID, B = , C = Field Name, D = Area
                pfd = iso.GetElementsByTagName("PFD");

                try
                {
                    //scan thru all the fields
                    foreach (XmlNode nodePFD in pfd)
                    {
                        double.TryParse(nodePFD.Attributes["D"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double area);
                        area *= 0.0001;

                        // PFD - A=ID, C=FieldName, D = Area in sq m
                        tree.Nodes.Add(nodePFD.Attributes["C"].Value + " Area: " + area + " Ha  " + nodePFD.Attributes["A"].Value);

                        //nodes in current Partial Field like PLN, GGP, LSG etc
                        XmlNodeList fieldParts = nodePFD.ChildNodes;

                        //do the boundary first - find all the polygons (PLN)
                        foreach (XmlNode nodePart in fieldParts)
                        {
                            // PLN/LSG/PNT Count the points
                            //grab the polygons
                            if (nodePart.Name == "PLN")
                            {
                                string bndType = "Unidentified Boundary";

                                if (nodePart.Attributes["A"].Value == "1")
                                {
                                    bndType = "Outer Boundary:";
                                }
                                else if (nodePart.Attributes["A"].Value == "3" || nodePart.Attributes["A"].Value == "4" ||
                                    nodePart.Attributes["A"].Value == "6")
                                {
                                    bndType = "Inner Boundary:";
                                }
                                else if (nodePart.Attributes["A"].Value == "10")
                                {
                                    bndType = "Headland:";
                                }
                                // "A" is the Polygon Type - usually 1 or 2
                                tree.Nodes[tree.Nodes.Count - 1].Nodes.Add(bndType +
                                    " " + nodePart.Attributes["A"].Value +
                                    "   Points: " + nodePart.ChildNodes[0].ChildNodes.Count);
                            }
                        }

                        //First kind of Gudance  GGP\GPN\LSG\PNT
                        foreach (XmlNode nodePart in fieldParts)
                        {
                            if (nodePart.Name == "GGP")
                            {
                                //in GPN "B" is the name and "C" is the type
                                if (nodePart.ChildNodes[0].Attributes["C"].Value == "1")
                                    tree.Nodes[tree.Nodes.Count - 1].Nodes.Add("ABLine: " + nodePart.ChildNodes[0].Attributes["B"].Value);
                                else if (nodePart.ChildNodes[0].Attributes["C"].Value == "2")
                                    tree.Nodes[tree.Nodes.Count - 1].Nodes.Add("A+: " + nodePart.ChildNodes[0].Attributes["B"].Value);
                                else if (nodePart.ChildNodes[0].Attributes["C"].Value == "3")
                                    tree.Nodes[tree.Nodes.Count - 1].Nodes.Add("Curve: " + nodePart.ChildNodes[0].Attributes["B"].Value);
                                else if (nodePart.ChildNodes[0].Attributes["C"].Value == "4")
                                    tree.Nodes[tree.Nodes.Count - 1].Nodes.Add("Pivot: " + nodePart.ChildNodes[0].Attributes["B"].Value);
                                else if (nodePart.ChildNodes[0].Attributes["C"].Value == "5")
                                    tree.Nodes[tree.Nodes.Count - 1].Nodes.Add("Spiral: " + nodePart.ChildNodes[0].Attributes["B"].Value);
                            }
                        }

                        //Second type of Gudance LSG\PNT
                        foreach (XmlNode nodePart in fieldParts)
                        {
                            //LSG with a "5" in [A] means Guidance line [B] is the name of line
                            if (nodePart.Name == "LSG" && nodePart.Attributes["A"].Value == "5")
                            {
                                if (nodePart.ChildNodes.Count < 3)
                                    tree.Nodes[tree.Nodes.Count - 1].Nodes.Add("ABLine: " + nodePart.Attributes["B"].Value);
                                else
                                    tree.Nodes[tree.Nodes.Count - 1].Nodes.Add("Curve: " + nodePart.Attributes["B"].Value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.EventWriter("Creating new iso field " + ex.ToString());
                    MessageBox.Show(gStr.gsError, ex.ToString());
                    return;
                }

                if (tree.Nodes.Count == 0) btnBuildFields.Enabled = false;
            }
            else
            {
                Close();
            }

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //top node selected (ie the field)
            if (tree.SelectedNode.Parent == null)
            {
                idxFieldSelected = tree.SelectedNode.Index;
                lblField.Text = idxFieldSelected.ToString() + " " + pfd[idxFieldSelected].Attributes["C"].Value;
                tboxFieldName.Text = pfd[idxFieldSelected].Attributes["C"].Value;
            }

            //one of the lines or bnds selected - so set the field selected
            else
            {
                idxFieldSelected = tree.SelectedNode.Parent.Index;
                lblField.Text = idxFieldSelected.ToString() + " " + pfd[idxFieldSelected].Attributes["C"].Value;
                tboxFieldName.Text = pfd[idxFieldSelected].Attributes["C"].Value;
            }

            if (idxFieldSelected == -1)
            {
                btnBuildFields.Enabled = false;
                btnAddDate.Enabled = false;
                btnAddTime.Enabled = false;
                tboxFieldName.Enabled = false;
            }
            else
            {
                btnBuildFields.Enabled = true;
                btnAddDate.Enabled = true;
                btnAddTime.Enabled = true;
                tboxFieldName.Enabled = true;
            }
        }

        private void btnBuildFields_Click(object sender, EventArgs e)
        {
            mf.currentFieldDirectory = tboxFieldName.Text.Trim();
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

            //create new field files.
            if ((!string.IsNullOrEmpty(directoryName)) && (Directory.Exists(directoryName)))
            {
                MessageBox.Show(gStr.gsChooseADifferentName, gStr.gsDirectoryExists, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                mf.currentFieldDirectory = "";
                return;
            }

            //this is all the PLN and GGC and LSG roots of selected field
            //PFD A = "Field ID" B = "Code" C = "Name" D = "Area sq m" E = "Customer Ref" F = "Farm Ref" >
            XmlNodeList fieldParts = pfd[idxFieldSelected].ChildNodes;

            double counter = 0;
            try
            {
                //Find the PLN in the field
                /*
                < PLN A = "1" C="Area in Sq M like 12568" >
                    < LSG A = "1" >
                        < PNT A = "2" C = "51.61918340" D = "4.51054560" />
                        < PNT A = "2" C = "51.61915460" D = "4.51056120" />
                    </ LSG >
                </ PLN >
                */

                double lat = 0, lon = 0;
                foreach (XmlNode nodePart in fieldParts)
                {
                    //grab the polygons
                    if (nodePart.Name == "PLN" && nodePart.Attributes["A"].Value == "1")
                    {
                        foreach (XmlNode pnt in nodePart.ChildNodes[0].ChildNodes) //PNT
                        {
                            double.TryParse(pnt.Attributes["C"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out latK);
                            double.TryParse(pnt.Attributes["D"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);

                            lat += latK;
                            lon += lonK;
                            counter += 1;
                        }
                    }
                }

                if (counter == 0)
                {
                    mf.YesMessageBox("Field Requires Outer Boundary.");
                    return;
                }

                lonK = lon / counter;
                latK = lat / counter;
            }
            catch (Exception ei)
            {
                mf.TimedMessageBox(2000, "Exception", "Catch Exception");
                Log.EventWriter("ISOXML Exception Loading " + ei.ToString());

                return;
            }

            //reset sim and world to kml position
            //append date time to name

            mf.menustripLanguage.Enabled = false;

            //create new field files.

            try
            {
                //start a new job
                mf.JobNew();
                mf.menustripLanguage.Enabled = false;

                //double check
                if ((!string.IsNullOrEmpty(directoryName)) && (Directory.Exists(directoryName)))
                {
                    MessageBox.Show(gStr.gsChooseADifferentName, gStr.gsDirectoryExists, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    CNMEA.latStart = latK;
                    CNMEA.lonStart = lonK;

                    if (mf.timerSim.Enabled)
                    {
                        mf.sim.latitude = Properties.Settings.Default.setGPS_SimLatitude = latK;
                        mf.sim.longitude = Properties.Settings.Default.setGPS_SimLongitude = lonK;

                        mf.pn.latitude = latK;
                        mf.pn.longitude = lonK;
                    }

                    mf.pn.SetLocalMetersPerDegree(true);

                    //make sure directory exists, or create it
                    if ((!string.IsNullOrEmpty(directoryName)) && (!Directory.Exists(directoryName)))
                    { Directory.CreateDirectory(directoryName); }

                    mf.displayFieldName = mf.currentFieldDirectory;

                    //create the field file header info

                    if (!mf.isJobStarted)
                    {
                        mf.TimedMessageBox(3000, gStr.gsFieldNotOpen, gStr.gsCreateNewField);
                        return;
                    }
                    string myFileName;

                    //get the directory and make sure it exists, create if not
                    directoryName = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

                    if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
                    { Directory.CreateDirectory(directoryName); }

                    myFileName = "Field.txt";

                    using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
                    {
                        //Write out the date
                        writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));

                        writer.WriteLine("$FieldDir");
                        writer.WriteLine("XML Derived");

                        //write out the easting and northing Offsets
                        writer.WriteLine("$Offsets");
                        writer.WriteLine("0,0");

                        writer.WriteLine("Convergence");
                        writer.WriteLine("0");

                        writer.WriteLine("StartFix");
                        writer.WriteLine(CNMEA.latStart.ToString(CultureInfo.InvariantCulture) + "," + CNMEA.lonStart.ToString(CultureInfo.InvariantCulture));
                    }

                    mf.FileSaveFlags();
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Creating new iso field " + ex.ToString());

                MessageBox.Show(gStr.gsError, ex.ToString());
                mf.currentFieldDirectory = "";
            }

            //Load the outer boundary first
            /*
            < PLN A = "1" C="Area in Sq M like 12568" >
                < LSG A = "1" >
                    < PNT A = "2" C = "51.61918340" D = "4.51054560" />
                    < PNT A = "2" C = "51.61915460" D = "4.51056120" />
                </ LSG >
            </ PLN >
            */

            try
            {
                CBoundaryList newBnd = new CBoundaryList();
                foreach (XmlNode nodePart in fieldParts)
                {
                    //grab the polygons
                    if (nodePart.Name == "PLN")
                    {
                        if (nodePart.Attributes["A"].Value == "1" || nodePart.Attributes["A"].Value == "9")
                        {
                            if (nodePart.ChildNodes[0].Attributes["A"].Value == "1") //LSG
                            {
                                foreach (XmlNode pnt in nodePart.ChildNodes[0].ChildNodes) //PNT
                                {
                                    double.TryParse(pnt.Attributes["C"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out latK);
                                    double.TryParse(pnt.Attributes["D"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);

                                    mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                                    //add the point to boundary
                                    newBnd.fenceLine.Add(new vec3(easting, norting, 0));
                                }
                            }
                        }
                    }

                    //we have outer bnd
                    if (newBnd.fenceLine.Count > 0) break;
                }

                mf.bnd.AddToBoundList(newBnd, mf.bnd.bndList.Count);
            }
            catch (Exception ew)
            {
                Log.EventWriter("Creating new iso field " + ew.ToString());

                MessageBox.Show(gStr.gsError, ew.ToString());
            }

            //load inner boundaries next only if outer existed

            //Load the outer boundary first
            if (mf.bnd.bndList.Count > 0)
            {
                try
                {
                    foreach (XmlNode nodePart in fieldParts)
                    {
                        //grab the polygons
                        if (nodePart.Name == "PLN")
                        {
                            if (nodePart.Attributes["A"].Value == "3" ||
                                nodePart.Attributes["A"].Value == "4" ||
                                nodePart.Attributes["A"].Value == "6")
                            {
                                if (nodePart.ChildNodes[0].Attributes["A"].Value == "1") //LSG
                                {
                                    CBoundaryList NewBnd = new CBoundaryList();
                                    foreach (XmlNode pnt in nodePart.ChildNodes[0].ChildNodes) //PNT
                                    {
                                        double.TryParse(pnt.Attributes["C"].Value, NumberStyles.Float,
                                            CultureInfo.InvariantCulture, out latK);
                                        double.TryParse(pnt.Attributes["D"].Value, NumberStyles.Float,
                                            CultureInfo.InvariantCulture, out lonK);

                                        mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                                        //add the point to boundary
                                        NewBnd.fenceLine.Add(new vec3(easting, norting, 0));
                                    }

                                    mf.bnd.AddToBoundList(NewBnd, mf.bnd.bndList.Count);
                                }
                            }
                        }
                    }
                }
                catch (Exception ed)
                {
                    Log.EventWriter("Creating new iso field " + ed.ToString());

                    MessageBox.Show(gStr.gsError, ed.ToString());
                }
            }
            //Headland

            //Load the outer boundary first
            if (mf.bnd.bndList.Count > 0 && mf.bnd.bndList[0].hdLine.Count == 0)
            {
                try
                {
                    foreach (XmlNode nodePart in fieldParts)
                    {
                        //grab the polygons
                        if (nodePart.Name == "PLN")
                        {
                            if (nodePart.Attributes["A"].Value == "10")
                            {
                                if (nodePart.ChildNodes[0].Attributes["A"].Value == "1") //LSG
                                {
                                    List<vec3> desList = new List<vec3>();

                                    foreach (XmlNode pnt in nodePart.ChildNodes[0].ChildNodes) //PNT
                                    {
                                        double.TryParse(pnt.Attributes["C"].Value, NumberStyles.Float,
                                            CultureInfo.InvariantCulture, out latK);
                                        double.TryParse(pnt.Attributes["D"].Value, NumberStyles.Float,
                                            CultureInfo.InvariantCulture, out lonK);

                                        mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                                        //add the point to boundary
                                        desList.Add(new vec3(easting, norting, 0));
                                    }

                                    //build the boundary, make sure is clockwise for outer counter clockwise for inner
                                    mf.trk.CalculateHeadings(ref desList);

                                    mf.bnd.bndList[0].hdLine = desList;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.EventWriter("Creating new iso field " + ex.ToString());
                    MessageBox.Show(gStr.gsError, ex.ToString());
                }
            }

            mf.bnd.isOkToAddPoints = false;
            mf.trk.designPtsList?.Clear();

            //load lines GGC ------------------------------------------------------------------------
            /*
            < GGP A = "GGP2" B = "Line name" >
                < GPN A = "GPN2" B = "Line name" C = "1,2,3,4,5 line type" E = "1" F = "1" I = "16" >
                    < LSG A = "5" > 5 = guidance
                        < PNT A = "6" C = "52.7811184647917" D = "6.71506979296218" E = "0" />
                        < PNT A = "7" C = "52.7813929252626" D = "6.71985880403741" E = "0" />
                    </ LSG >
                </ GPN >
            </ GGP >
            */
            try
            {
                foreach (XmlNode nodePart in fieldParts)
                {
                    //nodePart = GGP / GPN / LSG / PNT
                    if (nodePart.Name == "GGP")
                    {
                        //GPN B=Name, C=lineType: 1 is AB 3 is Curve
                        //AB Line ----------------------------------------------------------------
                        if (nodePart.ChildNodes[0].Attributes["C"].Value == "1") //AB Line
                        {
                            if (nodePart.ChildNodes[0].ChildNodes[0].Name == "LSG")
                            {
                                if (nodePart.ChildNodes[0].ChildNodes[0].Attributes["A"].Value == "5") //Guidance Pattern
                                {
                                    //get the name
                                    mf.trk.designName = nodePart.ChildNodes[0].Attributes["B"].Value;

                                    double.TryParse(nodePart.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["C"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out latK);
                                    double.TryParse(nodePart.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["D"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);

                                    mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                                    mf.trk.designPtA.easting = easting;
                                    mf.trk.designPtA.northing = norting;

                                    double.TryParse(nodePart.ChildNodes[0].ChildNodes[0].ChildNodes[1].Attributes["C"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out latK);
                                    double.TryParse(nodePart.ChildNodes[0].ChildNodes[0].ChildNodes[1].Attributes["D"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);

                                    mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                                    mf.trk.designPtB.easting = easting;
                                    mf.trk.designPtB.northing = norting;

                                    // heading based on AB points
                                    mf.trk.designHeading = Math.Atan2(mf.trk.designPtB.easting - mf.trk.designPtA.easting,
                                        mf.trk.designPtB.northing - mf.trk.designPtA.northing);
                                    if (mf.trk.designHeading < 0) mf.trk.designHeading += glm.twoPI;

                                    mf.trk.gArr.Add(new CTrk());

                                    //index to last one.
                                    int idx = mf.trk.gArr.Count - 1;

                                    mf.trk.gArr[idx].heading = mf.trk.designHeading;
                                    mf.trk.gArr[idx].mode = TrackMode.AB;

                                    ////calculate the new points for the reference line and points
                                    mf.trk.gArr[idx].ptA = new vec2(mf.trk.designPtA);
                                    mf.trk.gArr[idx].ptB = new vec2(mf.trk.designPtB);

                                    mf.trk.gArr[idx].name = mf.trk.designName.Trim();
                                }
                            } //LSG
                        }

                        //curve ------------------------------------------------------------------
                        else if (nodePart.ChildNodes[0].Attributes["C"].Value == "3") //curve
                        {
                            if (nodePart.ChildNodes[0].ChildNodes[0].Name == "LSG")
                            {
                                if (nodePart.ChildNodes[0].ChildNodes[0].Attributes["A"].Value == "5") //Guidance Pattern
                                {
                                    //get the name
                                    mf.trk.designName = nodePart.ChildNodes[0].Attributes["B"].Value;

                                    double.TryParse(nodePart.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["C"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out latK);
                                    double.TryParse(nodePart.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["D"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);

                                    mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                                    if (nodePart.ChildNodes[0].ChildNodes[0].ChildNodes.Count > 2)
                                    {
                                        mf.trk.designPtsList?.Clear();
                                        //GGP / GPN / LSG / PNT
                                        int cnt = nodePart.ChildNodes[0].ChildNodes[0].ChildNodes.Count;

                                        for (int i = 0; i < cnt; i++)
                                        {
                                            vec3 pt3;
                                            //calculate the point inside the boundary
                                            double.TryParse(nodePart.ChildNodes[0].ChildNodes[0].ChildNodes[i].Attributes["C"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out latK);
                                            double.TryParse(nodePart.ChildNodes[0].ChildNodes[0].ChildNodes[i].Attributes["D"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);
                                            mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                                            pt3.easting = easting;
                                            pt3.northing = norting;
                                            pt3.heading = 0;

                                            mf.trk.designPtsList.Add(pt3);
                                        }

                                        cnt = mf.trk.designPtsList.Count;
                                        if (cnt > 3)
                                        {
                                            mf.trk.gArr.Add(new CTrk());
                                            int idx = mf.trk.gArr.Count - 1;

                                            //make sure point distance isn't too big
                                            mf.trk.MakePointMinimumSpacing(ref mf.trk.designPtsList, 1.6);
                                            mf.trk.CalculateHeadings(ref mf.trk.designPtsList);

                                            //calculate average heading of line
                                            double x = 0, y = 0;

                                            foreach (vec3 pt in mf.trk.designPtsList)
                                            {
                                                x += Math.Cos(pt.heading);
                                                y += Math.Sin(pt.heading);
                                            }
                                            x /= mf.trk.designPtsList.Count;
                                            y /= mf.trk.designPtsList.Count;
                                            mf.trk.gArr[idx].heading = Math.Atan2(y, x);
                                            if (mf.trk.gArr[idx].heading < 0) mf.trk.gArr[idx].heading += glm.twoPI;

                                            //build the tail extensions
                                            mf.trk.AddFirstLastPoints(ref mf.trk.designPtsList,100);
                                            mf.trk.CalculateHeadings(ref mf.trk.designPtsList);

                                            if (string.IsNullOrEmpty(mf.trk.designName))
                                            {
                                                //create a name
                                                mf.trk.gArr[idx].name = (Math.Round(glm.toDegrees(mf.trk.gArr[idx].heading), 1)).ToString(CultureInfo.InvariantCulture)
                                                     + "\u00B0" + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
                                            }
                                            else
                                            {
                                                mf.trk.gArr[idx].name = mf.trk.designName;
                                            }

                                            mf.trk.gArr[idx].mode = TrackMode.Curve;

                                            //write out the trk Points
                                            foreach (vec3 item in mf.trk.designPtsList)
                                            {
                                                mf.trk.gArr[idx].curvePts.Add(item);
                                            }
                                        }
                                    }
                                }
                            } //LSG
                        }
                    }//is GGP
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Creating new iso field " + ex.ToString());
                MessageBox.Show(gStr.gsError, ex.ToString());
            }

            //AB Lines or curves when > 2 PNT's
            /*
            LSG A = "5" B = "Line Name" >
                < PNT A = "2" C = "51.61851540" D = "4.51137030" />
                < PNT A = "2" C = "51.61912230" D = "4.51056060" />
            </ LSG >
            */

            try
            {
                foreach (XmlNode nodePart in fieldParts)
                {
                    //nodePart = LSG / PNT - v3 guidance line type
                    if (nodePart.Name == "LSG")
                    {
                        //GPN B=Name, C=lineType: 1 is AB 3 is Curve
                        //AB Line ----------------------------------------------------------------
                        if (nodePart.Attributes["A"].Value == "5" && nodePart.ChildNodes.Count < 3) //Guidance Pattern
                        {
                            //get the name
                            mf.trk.designName = nodePart.Attributes["B"].Value;

                            double.TryParse(nodePart.ChildNodes[0].Attributes["C"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out latK);
                            double.TryParse(nodePart.ChildNodes[0].Attributes["D"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);

                            mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                            mf.trk.designPtA.easting = easting;
                            mf.trk.designPtA.northing = norting;

                            double.TryParse(nodePart.ChildNodes[1].Attributes["C"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out latK);
                            double.TryParse(nodePart.ChildNodes[1].Attributes["D"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);

                            mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                            mf.trk.designPtB.easting = easting;
                            mf.trk.designPtB.northing = norting;

                            // heading based on AB points
                            mf.trk.designHeading = Math.Atan2(mf.trk.designPtB.easting - mf.trk.designPtA.easting,
                                mf.trk.designPtB.northing - mf.trk.designPtA.northing);
                            if (mf.trk.designHeading < 0) mf.trk.designHeading += glm.twoPI;

                            mf.trk.gArr.Add(new CTrk());

                            //index to last one.
                            int idx = mf.trk.gArr.Count - 1;

                            mf.trk.gArr[idx].heading = mf.trk.designHeading;
                            mf.trk.gArr[idx].mode = TrackMode.AB;

                            ////calculate the new points for the reference line and points
                            mf.trk.gArr[idx].ptA = new vec2(mf.trk.designPtA);
                            mf.trk.gArr[idx].ptB = new vec2(mf.trk.designPtB);

                            mf.trk.gArr[idx].name = mf.trk.designName.Trim();
                        }
                        //curve ------------------------------------------------------------------
                        else if (nodePart.Attributes["A"].Value == "5" && nodePart.ChildNodes.Count > 2) //Guidance Pattern

                        {
                            //get the name
                            mf.trk.designName = nodePart.Attributes["B"].Value;

                            double.TryParse(nodePart.ChildNodes[0].Attributes["C"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out latK);
                            double.TryParse(nodePart.ChildNodes[0].Attributes["D"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);

                            mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                            if (nodePart.ChildNodes.Count > 2)
                            {
                                mf.trk.designPtsList?.Clear();
                                //GGP / GPN / LSG / PNT
                                int cnt = nodePart.ChildNodes.Count;

                                for (int i = 0; i < cnt; i++)
                                {
                                    vec3 pt3;
                                    //calculate the point inside the boundary
                                    double.TryParse(nodePart.ChildNodes[i].Attributes["C"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out latK);
                                    double.TryParse(nodePart.ChildNodes[i].Attributes["D"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);
                                    mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                                    pt3.easting = easting;
                                    pt3.northing = norting;
                                    pt3.heading = 0;

                                    mf.trk.designPtsList.Add(pt3);
                                }

                                cnt = mf.trk.designPtsList.Count;
                                if (cnt > 3)
                                {
                                    mf.trk.gArr.Add(new CTrk());
                                    int idx = mf.trk.gArr.Count - 1;

                                    //make sure point distance isn't too big
                                    mf.trk.MakePointMinimumSpacing(ref mf.trk.designPtsList, 1.6);
                                    mf.trk.CalculateHeadings(ref mf.trk.designPtsList);

                                    //calculate average heading of line
                                    double x = 0, y = 0;

                                    foreach (vec3 pt in mf.trk.designPtsList)
                                    {
                                        x += Math.Cos(pt.heading);
                                        y += Math.Sin(pt.heading);
                                    }
                                    x /= mf.trk.designPtsList.Count;
                                    y /= mf.trk.designPtsList.Count;
                                    mf.trk.gArr[idx].heading = Math.Atan2(y, x);
                                    if (mf.trk.gArr[idx].heading < 0) mf.trk.gArr[idx].heading += glm.twoPI;

                                    //build the tail extensions
                                    mf.trk.AddFirstLastPoints(ref mf.trk.designPtsList,100);
                                    mf.trk.CalculateHeadings(ref mf.trk.designPtsList);

                                    //array number is 1 less since it starts at zero

                                    //create a name
                                    if (!string.IsNullOrEmpty(mf.trk.designName))
                                        mf.trk.gArr[idx].name = mf.trk.designName;
                                    else mf.trk.gArr[idx].name =
                                            (Math.Round(glm.toDegrees(mf.trk.gArr[idx].heading), 1)).ToString(CultureInfo.InvariantCulture)
                                            + "\u00B0"
                                            + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

                                    mf.trk.gArr[idx].mode = TrackMode.Curve;

                                    //write out the Curve Points
                                    foreach (vec3 item in mf.trk.designPtsList)
                                    {
                                        mf.trk.gArr[idx].curvePts.Add(new vec3(item));
                                    }
                                }
                            }
                        }
                    }//is LSG
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Creating new iso field " + ex.ToString());
                MessageBox.Show(gStr.gsError, ex.ToString());
            }

            mf.FileSaveBoundary();
            mf.FileSaveHeadland();

            mf.FileSaveTracks();

            //close out window
            if (mf.bnd.bndList.Count > 0) mf.btnABDraw.Visible = true;

            mf.FieldMenuButtonEnableDisable(mf.bnd.bndList[0].hdLine.Count > 0);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void tboxFieldName_TextChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox textboxSender = (System.Windows.Forms.TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;
        }

        private void tboxFieldName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((System.Windows.Forms.TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAddDate_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + DateTime.Now.ToString("HH-mm", CultureInfo.InvariantCulture);
        }
    }
}