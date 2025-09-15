using AOG.Classes;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormLayer : Form
    {
        private readonly FormGPS mf = null;

        public FormLayer(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            btnOpenExistingLv.Text = gStr.Get(gs.gsUseSelected);
            btnOpenExistingLv.Enabled = false;
            btnDeleteJob.Enabled = mf.patchListLayer.Count > 0;

            lvLinesJob.ListViewItemSorter = new ListViewItemSorter(lvLinesJob);
            lvLinesJob.HideSelection = false;
            lvLinesJob.AllowColumnReorder = true;
        }

        private void FormFilePicker_Load(object sender, EventArgs e)
        {
            PopulateJobsListView();
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

                lvLinesJob.SelectedItems.Clear();
            }

            else
            {
                return;
            }
        }


        private void PopulateJobsListView()
        {
            lvLinesJob.Items.Clear();

                string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory,"Jobs");

                if (string.IsNullOrEmpty(directoryName) || (!Directory.Exists(directoryName)))
                {
                    return;
                }

                //list of jobs
                string[] dirs = Directory.GetDirectories(directoryName);

                foreach (string dir in dirs)
                {
                if (dir == Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, mf.currentJobDirectory)) continue;

                    var time = Directory.GetCreationTime(dir).ToString("yyyy-M-dd HH:MM");
                    var path = Path.GetFileName(dir);
                    var itmJob = new string[] { time, path };
                    lvLinesJob.Items.Add(new ListViewItem(itmJob));
                }
                if (lvLinesJob.Items.Count > 0)
                    lvLinesJob.Items[lvLinesJob.Items.Count - 1].EnsureVisible();

            lvLinesJob_SelectedIndexChanged(null, null);
        }

        private void lvLinesJob_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOpenExistingLv.Enabled = lvLinesJob.SelectedItems.Count > 0;
        }

        private void btnOpenExistingLv_Click(object sender, EventArgs e)
        {
            string dir = "";

            if (lvLinesJob.SelectedItems.Count > 0)
            {
                dir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory,
                    "Jobs", lvLinesJob.SelectedItems[0].SubItems[1].Text, "Sections.txt");
            }
            else
            {
                Close();
            }

            mf.patchListLayer.Clear();

            if (!File.Exists(dir))
            {
                return;
            }
            else
            {
                using (StreamReader reader = new StreamReader(dir))
                {
                    try
                    {
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
                                mf.patchListLayer.Add(triangleList);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        Log.EventWriter("Section file" + err.ToString());

                        mf.TimedMessageBox(2000, "Section File is Corrupt", gStr.Get(gs.gsButFieldIsLoaded));
                    }
                }

                Close();
            }
        }

        private void btnDeleteJob_Click(object sender, EventArgs e)
        {
            mf.patchListLayer.Clear();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }
    }
}