using AOG.Classes;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormJobPicker : Form
    {
        private readonly FormGPS mf = null;

        private readonly List<string> jobList = new List<string>();

        private ListViewItemSorter lvColumnSorterJobs;

        public FormJobPicker(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            btnOpenExistingLv.Text = gStr.Get(gs.gsUseSelected);

            lvColumnSorterJobs = new ListViewItemSorter(lvLinesJob);
            lvLinesJob.ListViewItemSorter = lvColumnSorterJobs;
        }

        private void FormJobPicker_Load(object sender, EventArgs e)
        {
            //old Version?
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs");

            if (string.IsNullOrEmpty(directoryName) || (!Directory.Exists(directoryName)))
            {
                mf.YesMessageBox("No Jobs Exist\r\n\r\n" + gStr.Get(gs.gsCreateNewJob));
                Log.EventWriter("Job Picker, No Jobs");
                Close();
                return;
            }

            LoadJobs();

            if (jobList.Count < 2)
            {
                mf.TimedMessageBox(2000, gStr.Get(gs.gsCreateNewJob), gStr.Get(gs.gsFileError));
                Log.EventWriter("Job Picker, No Jobs");
                Close();
                return;
            }
            ReloadList();
        }

        private void LoadJobs()
        {
            string[] dirs = Directory.GetDirectories(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs"));

            jobList?.Clear();

            foreach (string dir in dirs)
            {
                jobList.Add(Directory.GetCreationTime(dir).ToString("yyyy-M-dd HH:MM"));
                jobList.Add(Path.GetFileName(dir));
            }
        }

        private void ReloadList()
        {
            lvLinesJob.Items.Clear();

            for (int i = 0; i < jobList.Count; i += 2)
            {
                string[] fieldNames = { jobList[i], jobList[i + 1] };
                lvLinesJob.Items.Add(new ListViewItem(fieldNames));
            }
            lvLinesJob.Sort();
        }

        private void btnOpenExistingLv_Click(object sender, EventArgs e)
        {
            if (lvLinesJob.SelectedItems.Count > 0)
            {
                mf.jobPickerFileAndDirectory = lvLinesJob.SelectedItems[0].SubItems[1].Text;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.jobPickerFileAndDirectory = "";
        }

        private void btnDeleteJob_Click(object sender, EventArgs e)
        {
            if (lvLinesJob.SelectedItems.Count > 0)
            {
                string dir2Delete = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs", lvLinesJob.SelectedItems[0].SubItems[1].Text);

                DialogResult result3 = MessageBox.Show(
                    dir2Delete,
                    gStr.Get(gs.gsDeleteForSure),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result3 == DialogResult.Yes)
                {
                    System.IO.Directory.Delete(dir2Delete, true);
                    LoadJobs();
                    ReloadList();
                }
                else return;
            }
            else return;
        }
    }
}