﻿using AgOpenGPS.Culture;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AgOpenGPS
{
    public partial class FormFilePicker : Form
    {
        private readonly FormGPS mf = null;

        private int order;

        private readonly List<string> fieldList = new List<string>();
        private readonly List<string> jobList = new List<string>();

        public FormFilePicker(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            btnByDistance.Text = gStr.gsSort;
            btnOpenExistingLv.Text = gStr.gsUseSelected;
        }

        private void FormFilePicker_Load(object sender, EventArgs e)
        {
            order = 0;
            ListViewItem itm;

            string[] dirs = Directory.GetDirectories(RegistrySettings.fieldsDirectory);

            //jobList?.Clear();

            if (dirs == null || dirs.Length < 1)
            {
                mf.TimedMessageBox(2000, gStr.gsCreateNewField, gStr.gsFileError);
                Log.EventWriter("File Picker, No Fields");
                Close();
                return;
            }

            foreach (string dir in dirs)
            {
                double latStart = 0;
                double lonStart = 0;
                double distance = 0;
                string fieldDirectory = Path.GetFileName(dir);
                string filename = Path.Combine(dir, "Field.txt");
                string line;

                //make sure directory has a field.txt in it
                if (File.Exists(filename))
                {
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

                                distance = Math.Pow((latStart - mf.pn.latitude), 2) + Math.Pow((lonStart - mf.pn.longitude), 2);
                                distance = Math.Sqrt(distance);
                                distance *= 100;

                                fieldList.Add(fieldDirectory);
                                fieldList.Add(Math.Round(distance, 2).ToString("N2").PadLeft(10));
                            }
                            else
                            {
                                MessageBox.Show(fieldDirectory + " is Damaged, Please Delete This Field", gStr.gsFileError,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                                fieldList.Add(fieldDirectory);
                                fieldList.Add("Error");
                            }
                        }
                        catch (Exception eg)
                        {
                            MessageBox.Show(fieldDirectory + " is Damaged, Please Delete, Field.txt is Broken", gStr.gsFileError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Log.EventWriter("Field.txt is Broken" + eg.ToString());
                            fieldList.Add(fieldDirectory);
                            fieldList.Add("Error");
                        }
                    }
                }
                else continue;

                //grab the boundary area
                filename = Path.Combine(dir, "Boundary.txt");
                if (File.Exists(filename))
                {
                    List<vec3> pointList = new List<vec3>();
                    double area = 0;

                    using (StreamReader reader = new StreamReader(filename))
                    {
                        try
                        {
                            //read header
                            line = reader.ReadLine();//Boundary

                            if (!reader.EndOfStream)
                            {
                                //True or False OR points from older boundary files
                                line = reader.ReadLine();

                                //Check for older boundary files, then above line string is num of points
                                if (line == "True" || line == "False")
                                {
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

                                        pointList.Add(vecPt);
                                    }

                                    int ptCount = pointList.Count;
                                    if (ptCount > 5)
                                    {
                                        area = 0;         // Accumulates area in the loop
                                        int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

                                        for (int i = 0; i < ptCount; j = i++)
                                        {
                                            area += (pointList[j].easting + pointList[i].easting) * (pointList[j].northing - pointList[i].northing);
                                        }
                                        if (mf.isMetric)
                                        {
                                            area = (Math.Abs(area / 2)) * 0.0001;
                                        }
                                        else
                                        {
                                            area = (Math.Abs(area / 2)) * 0.00024711;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            area = 0;
                            Log.EventWriter("Field.txt is Broken" + e.ToString());
                        }
                    }
                    if (area == 0) fieldList.Add("No Bndry");
                    else fieldList.Add(Math.Round(area, 1).ToString("N1").PadLeft(10));
                }
                else
                {
                    fieldList.Add("Error");
                    MessageBox.Show(fieldDirectory + " is Damaged, Missing Boundary.Txt " +
                        "               \r\n Delete Field or Fix ", gStr.gsFileError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                filename = Path.Combine(dir, "Field.txt");
            }

            if (fieldList == null || fieldList.Count < 1)
            {
                mf.TimedMessageBox(2000, gStr.gsNoFieldsFound, gStr.gsCreateNewField);
                Log.EventWriter("File Picker, No fields Sorted");
                Close();
                return;
            }
            for (int i = 0; i < fieldList.Count; i += 3)
            {
                string[] fieldNames = { fieldList[i], fieldList[i + 1], fieldList[i + 2] };
                itm = new ListViewItem(fieldNames);
                lvLines.Items.Add(itm);
            }

            //string fieldName = Path.GetDirectoryName(dir).ToString(CultureInfo.InvariantCulture);

            if (lvLines.Items.Count > 0)
            {
                this.chName.Text = gStr.gsField;
                this.chName.Width = 680;

                this.chDistance.Text = gStr.gsDistance;
                this.chDistance.Width = 140;

                this.chArea.Text = gStr.gsArea;
                this.chArea.Width = 140;
            }
            else
            {
                mf.TimedMessageBox(2000, gStr.gsNoFieldsFound, gStr.gsCreateNewField);
                Log.EventWriter("File Picker, No Line items");
                Close();
                return;
            }
        }

        private void lvLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count < 1) return;

            ListViewItem itmJob;
            lvLinesJob.Items.Clear();

            string chosenDir;
            if (order == 0) chosenDir =
                Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[0].Text);
            else chosenDir =
                Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[1].Text);

            string directoryName = Path.Combine(chosenDir, "Jobs");

            if (!string.IsNullOrEmpty(directoryName) && (!Directory.Exists(directoryName)))
            {
                return;
            }


            //list of jobs
            string[] dirs = Directory.GetDirectories(directoryName);

            jobList?.Clear();

            if (dirs == null || dirs.Length < 1)
            {
                mf.TimedMessageBox(2000, gStr.gsCreateNewJob, gStr.gsFileError);
                Log.EventWriter("Job Picker, No Jobs");
                Close();
                return;
            }

            foreach (string dir in dirs)
            {
                jobList.Add(Directory.GetCreationTime(dir).ToString());
                jobList.Add(Path.GetFileName(dir));
            }

            for (int i = 0; i < jobList.Count; i += 2)
            {
                string[] jobNames = { jobList[i], jobList[i + 1] };
                itmJob = new ListViewItem(jobNames);
                lvLinesJob.Items.Add(itmJob);
            }

            if (lvLinesJob.Items.Count > 0)
            {
                lvLinesJob.Items[lvLinesJob.Items.Count - 1].EnsureVisible();
            }
            else
            {
                mf.TimedMessageBox(2000, gStr.gsNoFieldsFound, gStr.gsCreateNewField);
                Log.EventWriter("File Picker, No Line items");
                Close();
                return;
            }
        }

        private void btnByDistance_Click(object sender, EventArgs e)
        {
            ListViewItem itm;

            lvLines.Items.Clear();
            order += 1;
            if (order == 3) order = 0;

            for (int i = 0; i < fieldList.Count; i += 3)
            {
                if (order == 0)
                {
                    string[] fieldNames = { fieldList[i], fieldList[i + 1], fieldList[i + 2] };
                    itm = new ListViewItem(fieldNames);
                }
                else if (order == 1)
                {
                    string[] fieldNames = { fieldList[i + 1], fieldList[i], fieldList[i + 2] };
                    itm = new ListViewItem(fieldNames);
                }
                else
                {
                    string[] fieldNames = { fieldList[i + 2], fieldList[i], fieldList[i + 1] };
                    itm = new ListViewItem(fieldNames);
                }

                lvLines.Items.Add(itm);
            }

            if (lvLines.Items.Count > 0)
            {
                if (order == 0)
                {
                    this.chName.Text = gStr.gsField;
                    this.chName.Width = 680;

                    this.chDistance.Text = gStr.gsDistance;
                    this.chDistance.Width = 140;

                    this.chArea.Text = gStr.gsArea;
                    this.chArea.Width = 140;
                }
                else if (order == 1)
                {
                    this.chName.Text = gStr.gsDistance;
                    this.chName.Width = 140;

                    this.chDistance.Text = gStr.gsField;
                    this.chDistance.Width = 680;

                    this.chArea.Text = gStr.gsArea;
                    this.chArea.Width = 140;
                }
                else
                {
                    this.chName.Text = gStr.gsArea;
                    this.chName.Width = 140;

                    this.chDistance.Text = gStr.gsField;
                    this.chDistance.Width = 680;

                    this.chArea.Text = gStr.gsDistance;
                    this.chArea.Width = 140;
                }
            }
        }

        private void btnOpenExistingLv_Click(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            if (count > 0)
            {
                if (lvLines.SelectedItems[0].SubItems[0].Text == "Error" ||
                    lvLines.SelectedItems[0].SubItems[1].Text == "Error" ||
                    lvLines.SelectedItems[0].SubItems[2].Text == "Error")
                {
                    MessageBox.Show("This Field is Damaged, Please Delete \r\n ALREADY TOLD YOU THAT :)", gStr.gsFileError,
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (order == 0) mf.filePickerFileAndDirectory =
                            Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[0].Text, "Field.txt");
                    else mf.filePickerFileAndDirectory =
                            Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[1].Text, "Field.txt");

                    if (lvLinesJob.SelectedItems.Count > 0)
                    {
                        mf.jobPickerFileAndDirectory = lvLinesJob.SelectedItems[0].SubItems[1].Text;
                    }
                    else
                    {
                        mf.jobPickerFileAndDirectory = "";
                    }

                    Close();
                }
            }
        }

        private void btnDeleteAB_Click(object sender, EventArgs e)
        {
            mf.filePickerFileAndDirectory = "";
        }

        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            string dir2Delete;
            if (count > 0)
            {
                if (order == 0)
                    dir2Delete = Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[0].Text);
                else
                    dir2Delete = Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[1].Text);

                DialogResult result3 = MessageBox.Show(
                    dir2Delete,
                    gStr.gsDeleteForSure,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result3 == DialogResult.Yes)
                {
                    System.IO.Directory.Delete(dir2Delete, true);
                }
                else return;
            }
            else return;

            ListViewItem itm;

            string[] dirs = Directory.GetDirectories(RegistrySettings.fieldsDirectory);

            fieldList?.Clear();

            foreach (string dir in dirs)
            {
                double latStart = 0;
                double lonStart = 0;
                double distance = 0;
                string fieldDirectory = Path.GetFileName(dir);
                string filename = Path.Combine(dir, "Field.txt");
                string line;

                //make sure directory has a field.txt in it
                if (File.Exists(filename))
                {
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

                                distance = Math.Pow((latStart - mf.pn.latitude), 2) + Math.Pow((lonStart - mf.pn.longitude), 2);
                                distance = Math.Sqrt(distance);
                                distance *= 100;

                                fieldList.Add(fieldDirectory);
                                fieldList.Add(Math.Round(distance, 2).ToString("N2").PadLeft(10));
                            }
                            else
                            {
                                MessageBox.Show(fieldDirectory + " is Damaged, Please Delete This Field", gStr.gsFileError,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                                fieldList.Add(fieldDirectory);
                                fieldList.Add("Error");
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(fieldDirectory + " is Damaged, Please Delete, Field.txt is Broken", gStr.gsFileError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Log.EventWriter("Field.txt is Broken" + e.ToString());
                            fieldList.Add(fieldDirectory);
                            fieldList.Add("Error");
                        }
                    }

                    //grab the boundary area
                    filename = Path.Combine(dir, "Boundary.txt");
                    if (File.Exists(filename))
                    {
                        List<vec3> pointList = new List<vec3>();
                        double area = 0;

                        using (StreamReader reader = new StreamReader(filename))
                        {
                            try
                            {
                                //read header
                                line = reader.ReadLine();//Boundary

                                if (!reader.EndOfStream)
                                {
                                    //True or False OR points from older boundary files
                                    line = reader.ReadLine();

                                    //Check for older boundary files, then above line string is num of points
                                    if (line == "True" || line == "False")
                                    {
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

                                            pointList.Add(vecPt);
                                        }

                                        int ptCount = pointList.Count;
                                        if (ptCount > 5)
                                        {
                                            area = 0;         // Accumulates area in the loop
                                            int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

                                            for (int i = 0; i < ptCount; j = i++)
                                            {
                                                area += (pointList[j].easting + pointList[i].easting) * (pointList[j].northing - pointList[i].northing);
                                            }
                                            if (mf.isMetric)
                                            {
                                                area = (Math.Abs(area / 2)) * 0.0001;
                                            }
                                            else
                                            {
                                                area = (Math.Abs(area / 2)) * 0.00024711;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                area = 0;
                                Log.EventWriter("Field.txt is Broken" + e.ToString());
                            }
                        }
                        if (area == 0) fieldList.Add("No Bndry");
                        else fieldList.Add(Math.Round(area, 1).ToString("N1").PadLeft(10));
                    }
                    else
                    {
                        fieldList.Add("Error");
                        MessageBox.Show(fieldDirectory + " is Damaged, Missing Boundary.Txt " +
                            "               \r\n Delete Field or Fix ", gStr.gsFileError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            lvLines.Items.Clear();

            for (int i = 0; i < fieldList.Count; i += 3)
            {
                string[] fieldNames = { fieldList[i], fieldList[i + 1], fieldList[i + 2] };
                itm = new ListViewItem(fieldNames);
                lvLines.Items.Add(itm);
            }

            //string fieldName = Path.GetDirectoryName(dir).ToString(CultureInfo.InvariantCulture);

            if (lvLines.Items.Count > 0)
            {
                this.chName.Text = gStr.gsField;
                this.chName.Width = 680;

                this.chDistance.Text = gStr.gsDistance;
                this.chDistance.Width = 140;

                this.chArea.Text = gStr.gsArea;
                this.chArea.Width = 140;
            }
            else
            {
                //var form2 = new FormTimedMessage(2000, gStr.gsNoFieldsCreated, gStr.gsCreateNewFieldFirst);
                //form2.Show(this);
            }
        }

        private void bntNewJob_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                if (lvLines.SelectedItems[0].SubItems[0].Text == "Error" ||
                    lvLines.SelectedItems[0].SubItems[1].Text == "Error" ||
                    lvLines.SelectedItems[0].SubItems[2].Text == "Error")
                {
                    MessageBox.Show("This Field is Damaged, Please Delete \r\n ALREADY TOLD YOU THAT :)", gStr.gsFileError,
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (order == 0) mf.filePickerFileAndDirectory =
                            Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[0].Text, "Field.txt");
                    else mf.filePickerFileAndDirectory =
                            Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[1].Text, "Field.txt");

                        mf.jobPickerFileAndDirectory = "Newww";

                    Close();
                }
            }
            else
            {
                mf.YesMessageBox("Pick a field");
            }
        }
    }
}