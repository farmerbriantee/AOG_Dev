using AgOpenGPS.Culture;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormJobPicker : Form
    {
        private readonly FormGPS mf = null;

        private int order;

        private readonly List<string> jobList = new List<string>();

        public FormJobPicker(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            btnByDistance.Text = gStr.gsSort;
            btnOpenExistingLv.Text = gStr.gsUseSelected;
        }

        private void FormJobPicker_Load(object sender, EventArgs e)
        {
            order = 0;
            ListViewItem itm;

            //old Version?
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs");

            if (!string.IsNullOrEmpty(directoryName) && (!Directory.Exists(directoryName)))
            {
                mf.YesMessageBox("No Jobs Exist\r\n\r\n" + gStr.gsCreateNewJob);
                Log.EventWriter("Job Picker, No Jobs");
                Close();
                return;
            }

            //list of jobs
            string[] dirs = Directory.GetDirectories(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs"));

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
                itm = new ListViewItem(jobNames);
                lvLines.Items.Add(itm);
            }

            //string fieldName = Path.GetDirectoryName(dir).ToString(CultureInfo.InvariantCulture);

            if (lvLines.Items.Count > 0)
            {
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
            if (order == 2) order = 0;

            for (int i = 0; i < jobList.Count; i += 2)
            {
                if (order == 0)
                {
                    string[] fieldNames = { jobList[i], jobList[i + 1], jobList[i + 2] };
                    itm = new ListViewItem(fieldNames);
                }
                else 
                {
                    string[] fieldNames = { jobList[i + 1], jobList[i], jobList[i + 2] };
                    itm = new ListViewItem(fieldNames);
                }

                lvLines.Items.Add(itm);
            }

            if (lvLines.Items.Count > 0)
            {
                if (order == 0)
                {
                    this.chName.Text = gStr.gsField;
                    this.chName.Width = 700;


                    this.chDate.Text = gStr.gsArea;
                    this.chDate.Width = 200;
                }
                else if (order == 1)
                {
                    this.chName.Text = gStr.gsDistance;
                    this.chName.Width = 200;

                    this.chDate.Text = gStr.gsArea;
                    this.chDate.Width = 200;
                }
            }
        }

        private void btnOpenExistingLv_Click(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            if (count > 0)
            {
                    if (order == 0) mf.jobPickerFileAndDirectory =lvLines.SelectedItems[0].SubItems[1].Text;
                    else mf.jobPickerFileAndDirectory = lvLines.SelectedItems[0].SubItems[0].Text;
                    Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.jobPickerFileAndDirectory = "";
        }

        private void btnDeleteJob_Click(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            string dir2Delete;
            if (count > 0)
            {
                if (order == 0)
                    dir2Delete = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs", lvLines.SelectedItems[0].SubItems[0].Text);
                else
                    dir2Delete = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs", lvLines.SelectedItems[0].SubItems[1].Text);

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

            string[] dirs = Directory.GetDirectories(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs"));

            jobList?.Clear();

            lvLines.Items.Clear();

            for (int i = 0; i < jobList.Count; i += 2)
            {
                string[] JobNames = { jobList[i], jobList[i + 1] };
                itm = new ListViewItem(JobNames);
                lvLines.Items.Add(itm);
            }

            //string fieldName = Path.GetDirectoryName(dir).ToString(CultureInfo.InvariantCulture);

            if (lvLines.Items.Count > 0)
            {
                this.chName.Text = gStr.gsField;
                this.chName.Width = 700;

                this.chDate.Text = gStr.gsArea;
                this.chDate.Width = 200;
            }
            else
            {
                //var form2 = new FormTimedMessage(2000, gStr.gsNoFieldsCreated, gStr.gsCreateNewFieldFirst);
                //form2.Show(this);
            }
        }
    }
}