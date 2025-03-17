using AOG.Classes;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormFieldOpen : Form
    {
        private readonly FormGPS mf = null;

        public FormFieldOpen(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            btnOpenExistingLv.Text = gStr.Get(gs.gsUseSelected);
            btnOpenExistingLv.Enabled = false;
            btnDeleteJob.Enabled = false;
            btnDeleteField.Enabled = false;

            lvLines.Columns[0].Text = gStr.Get(gs.gsField);
            lvLines.Columns[1].Text = gStr.Get(gs.gsDistance);
            lvLines.Columns[2].Text = gStr.Get(gs.gsArea);

            lvLinesJob.ListViewItemSorter = new ListViewItemSorter(lvLinesJob);
            lvLinesJob.HideSelection = false;
            lvLinesJob.AllowColumnReorder = true;

            lvLines.ListViewItemSorter = new ListViewItemSorter(lvLines);
            lvLines.HideSelection = false;
            lvLines.AllowColumnReorder = true;
        }

        private void FormFilePicker_Load(object sender, EventArgs e)
        {
            PopulateFieldsListView();

            if (mf.isFieldStarted)
            {
                for (int i = 0; i < lvLines.Items.Count; i++)
                {
                    if (lvLines.Items[i].SubItems[0].Text == mf.currentFieldDirectory)
                    {
                        lvLines.Items[i].Selected = true;
                        lvLines.Select();
                        lvLines.Items[i].EnsureVisible();
                        break;
                    }
                }
            }

            if (mf.isFieldStarted && mf.isJobStarted)
            {
                for (int i = 0; i < lvLinesJob.Items.Count; i++)
                {
                    if (lvLinesJob.Items[i].SubItems[1].Text == mf.displayJobName)
                    {          
                        lvLinesJob.Items[i].Selected = true;
                        lvLinesJob.Select();
                        lvLinesJob.Items[i].EnsureVisible();
                        break;
                    }
                }
            }
        }

        private void PopulateFieldsListView()
        {
            lvLines.Items.Clear();

            string[] dirs = Directory.GetDirectories(RegistrySettings.fieldsDirectory);

            if (dirs == null || dirs.Length < 1)
            {
                mf.TimedMessageBox(2000, gStr.Get(gs.gsCreateNewField), gStr.Get(gs.gsFileError));
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
                string distanceString = "";
                string areaString = "";
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

                                distanceString = Math.Round(distance, 2).ToString("N2").PadLeft(10);
                            }
                            else
                            {
                                MessageBox.Show(fieldDirectory + " is Damaged, Please Delete This Field", gStr.Get(gs.gsFileError),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                                distanceString = "Error";
                            }
                        }
                        catch (Exception eg)
                        {
                            MessageBox.Show(fieldDirectory + " is Damaged, Please Delete, Field.txt is Broken", gStr.Get(gs.gsFileError),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Log.EventWriter("Field.txt is Broken" + eg.ToString());
                            distanceString = "Error";
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
                                        area = Math.Abs(area / 2) * glm.m22HaOrAc;
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            area = 0;
                            Log.EventWriter("Field.txt is Broken" + e.ToString());
                        }
                    }
                    if (area == 0) areaString = "No Bndry";
                    else areaString = Math.Round(area, 1).ToString("N1").PadLeft(10);
                }
                else
                {
                    areaString = "Error";
                    MessageBox.Show(fieldDirectory + " is Damaged, Missing Boundary.Txt " +
                        "               \r\n Delete Field or Fix ", gStr.Get(gs.gsFileError),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                string[] fieldNames = { fieldDirectory, distanceString, areaString };
                lvLines.Items.Add(new ListViewItem(fieldNames));
            }

            if (lvLines.Items.Count == 0)
            {
                mf.TimedMessageBox(2000, gStr.Get(gs.gsNoFieldsFound), gStr.Get(gs.gsCreateNewField));
                Log.EventWriter("File Picker, No field items");
                Close();
                return;
            }
        }

        private void lvLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDeleteField.Enabled = btnOpenExistingLv.Enabled = lvLines.SelectedItems.Count > 0;
            PopulateJobsListView();
        }

        private void PopulateJobsListView()
        {
            lvLinesJob.Items.Clear();

            if (lvLines.SelectedItems.Count > 0)
            {
                string chosenDir = Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[0].Text);

                string directoryName = Path.Combine(chosenDir, "Jobs");

                if (string.IsNullOrEmpty(directoryName) || (!Directory.Exists(directoryName)))
                {
                    return;
                }

                //list of jobs
                string[] dirs = Directory.GetDirectories(directoryName);

                foreach (string dir in dirs)
                {
                    var time = Directory.GetCreationTime(dir).ToString("yyyy-M-dd HH:MM");
                    var path = Path.GetFileName(dir);
                    var itmJob = new string[] { time, path };
                    lvLinesJob.Items.Add(new ListViewItem(itmJob));
                }
                if (lvLinesJob.Items.Count > 0)
                    lvLinesJob.Items[lvLinesJob.Items.Count - 1].EnsureVisible();
            }
            lvLinesJob_SelectedIndexChanged(null, null);
        }

        private void lvLinesJob_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDeleteJob.Enabled = lvLinesJob.SelectedItems.Count > 0;
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
                    MessageBox.Show("This Field is Damaged, Please Delete \r\n ALREADY TOLD YOU THAT :)", gStr.Get(gs.gsFileError),
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    mf.filePickerFileAndDirectory =
                            Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[0].Text, "Field.txt");

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
            else
            {
                mf.YesMessageBox("Pick a field");
                this.DialogResult = DialogResult.None;
                return;
            }
        }

        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                string dir2Delete = Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[0].Text);

                DialogResult result3 = MessageBox.Show(
                    dir2Delete,
                    gStr.Get(gs.gsDeleteForSure),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result3 == DialogResult.Yes)
                {
                    //close field and job
                    mf.FileSaveEverythingBeforeClosingField();

                    System.IO.Directory.Delete(dir2Delete, true);

                    PopulateFieldsListView();
                    lvLinesJob.Items.Clear();
                }
            }
        }

        private void btnDeleteJob_Click(object sender, EventArgs e)
        {
            string dir2Delete;
            if (lvLinesJob.SelectedItems.Count > 0)
            {
                dir2Delete = Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[0].Text);

                dir2Delete = Path.Combine(dir2Delete, "Jobs", lvLinesJob.SelectedItems[0].SubItems[1].Text);
                DialogResult result3 = MessageBox.Show(
                    dir2Delete,
                    gStr.Get(gs.gsDeleteForSure),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result3 == DialogResult.Yes)
                {
                    //close field and job
                    mf.JobClose();
                    System.IO.Directory.Delete(dir2Delete, true);
                    PopulateJobsListView();
                }
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
                    MessageBox.Show("This Field is Damaged, Please Delete \r\n ALREADY TOLD YOU THAT :)", gStr.Get(gs.gsFileError),
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    mf.filePickerFileAndDirectory =
                            Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[0].Text, "Field.txt");

                    mf.jobPickerFileAndDirectory = "Newww";
                    this.DialogResult = DialogResult.Yes;

                    Close();
                }
            }
            else
            {
                mf.YesMessageBox("Pick a field");
                this.DialogResult = DialogResult.None;
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.filePickerFileAndDirectory = "";
        }
    }
}