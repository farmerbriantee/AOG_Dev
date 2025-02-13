using AgOpenGPS.Culture;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormJobPicker : Form
    {
        private readonly FormGPS mf = null;

        private readonly List<string> jobList = new List<string>();

        ListViewItemComparer sorter = new ListViewItemComparer(new Type[] { typeof(DateTime), typeof(string) });

        public FormJobPicker(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            btnOpenExistingLv.Text = gStr.gsUseSelected;
        }

        private void FormJobPicker_Load(object sender, EventArgs e)
        {
            //old Version?
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs");

            if (string.IsNullOrEmpty(directoryName) || (!Directory.Exists(directoryName)))
            {
                mf.YesMessageBox("No Jobs Exist\r\n\r\n" + gStr.gsCreateNewJob);
                Log.EventWriter("Job Picker, No Jobs");
                Close();
                return;
            }
            lvLinesJob.ListViewItemSorter = sorter;

            LoadJobs();

            if (jobList.Count < 2)
            {
                mf.TimedMessageBox(2000, gStr.gsCreateNewJob, gStr.gsFileError);
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
                jobList.Add(Directory.GetCreationTime(dir).ToString(CultureInfo.CurrentUICulture));
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
            int count = lvLinesJob.SelectedItems.Count;
            if (count > 0)
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
            int count = lvLinesJob.SelectedItems.Count;
            if (count > 0)
            {
                string dir2Delete = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs", lvLinesJob.SelectedItems[0].SubItems[1].Text);

                DialogResult result3 = MessageBox.Show(
                    dir2Delete,
                    gStr.gsDeleteForSure,
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

        private void lvLinesJob_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == sorter.ColumnIndex)
            {
                // Reverse the current sort direction for this column.
                if (sorter.SortDirection == SortOrder.Ascending)
                {
                    sorter.SortDirection = SortOrder.Descending;
                }
                else
                {
                    sorter.SortDirection = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sorter.ColumnIndex = e.Column;
                sorter.SortDirection = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lvLinesJob.Sort();
        }
    }


    public class ListViewItemComparer : IComparer
    {
        private int _columnIndex;
        public int ColumnIndex
        {
            get
            {
                return _columnIndex;
            }
            set
            {
                _columnIndex = value;
            }
        }

        private SortOrder _sortDirection = SortOrder.Ascending;
        public SortOrder SortDirection
        {
            get
            {
                return _sortDirection;
            }
            set
            {
                _sortDirection = value;
            }
        }

        private Type[] _columnType;

        public Type[] ColumnType
        {
            get
            {
                return _columnType;
            }
        }

        public ListViewItemComparer(Type[] type)
        {
            _columnType = type;
        }

        public int Compare(object x, object y)
        {
            ListViewItem lviX = x as ListViewItem;
            ListViewItem lviY = y as ListViewItem;

            int result;

            if (lviX == null && lviY == null)
            {
                result = 0;
            }
            else if (lviX == null)
            {
                result = -1;
            }
            else if (lviY == null)
            {
                result = 1;
            }

            if (ColumnType.Length > ColumnIndex)
            {
                if (ColumnType[ColumnIndex] == typeof(DateTime))
                {
                    DateTime xDt = DateTime.Parse(lviX.SubItems[ColumnIndex].Text);
                    DateTime yDt = DateTime.Parse(lviY.SubItems[ColumnIndex].Text);
                    result = -DateTime.Compare(xDt, yDt);
                }
                else if (ColumnType[ColumnIndex] == typeof(double))
                {
                    double xDt = double.Parse(lviX.SubItems[ColumnIndex].Text);
                    double yDt = double.Parse(lviY.SubItems[ColumnIndex].Text);
                    result = xDt.CompareTo(yDt);
                }
                else
                {
                    result = string.Compare(
                        lviX.SubItems[ColumnIndex].Text,
                        lviY.SubItems[ColumnIndex].Text,
                        true);
                }
            }
            else
            {
                result = string.Compare(
                    lviX.SubItems[ColumnIndex].Text,
                    lviY.SubItems[ColumnIndex].Text,
                    true);
            }

            if (SortDirection == SortOrder.Descending)
            {
                return -result;
            }
            else
            {
                return result;
            }
        }
    }

}