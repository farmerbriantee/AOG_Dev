using AOG.Classes;

using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormFieldNew : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormFieldNew(Form _callingForm)
        {
            //get copy of the calling main form
            mf = _callingForm as FormGPS;

            InitializeComponent();

            label1.Text = gStr.Get(gs.gsEnterFieldName);

            label6.Text = gStr.Get(gs.gsEnterJobName);
        }

        private void FormFieldDir_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            if (mf.isFieldStarted)
            {
                tboxFieldName.Text = mf.currentFieldDirectory;
                tboxFieldName.Enabled = false;

                btnFieldNew.Enabled = true;
                btnAddDate.Enabled = false;
                btnAddTime.Enabled = false;
            }

            if (mf.isJobStarted)
            {
                tboxJobName.Text = Path.GetFileName(mf.currentJobDirectory); 
                tboxJobName.Enabled = false;

                btnJobNew.Enabled = true;
                btnAddDateJob.Enabled = false;
                btnAddTimeJob.Enabled = false;
            }

            if (!mf.isFieldStarted && !mf.isJobStarted)
            {
                btnFieldNew.Enabled = false;
                btnJobNew.Enabled = false;
                tboxJobName.Enabled = false;
                btnAddDateJob.Enabled = false;
                btnAddTimeJob.Enabled = false;
            }

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void tboxFieldName_TextChanged(object sender, EventArgs e)
        {
            if (mf.isFieldStarted) return;

            TextBox textboxSender = (TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            if (String.IsNullOrEmpty(tboxFieldName.Text.Trim()))
            {
                btnSave.Enabled = false;
                tboxJobName.Enabled = false;
                btnAddDateJob.Enabled = false;
                btnAddTimeJob.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
                tboxJobName.Enabled = true;
                btnAddDateJob.Enabled = true;
                btnAddTimeJob.Enabled = true;
            }
        }

        private void tboxFieldName_Click(object sender, EventArgs e)
        {
            if (Settings.User.setDisplay_isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnJobCancel.Focus();
            }
        }

        private void btnAddDate_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + DateTime.Now.ToString("HH-mm", CultureInfo.InvariantCulture);
        }

        private void CreateNewField()
        {
            //fill something in
            if (String.IsNullOrEmpty(tboxFieldName.Text.Trim()))
            {
                return;
            }

            //append date time to name

            mf.currentFieldDirectory = tboxFieldName.Text.Trim();

            //get the directory and make sure it exists, create if not
            DirectoryInfo dirNewField = new DirectoryInfo(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory));

            mf.menustripLanguage.Enabled = false;
            //if no template set just make a new file.
            try
            {
                //start a new field
                mf.FieldNew();

                //create it for first save
                if (dirNewField.Exists)
                {
                    MessageBox.Show(gStr.Get(gs.gsChooseADifferentName), gStr.Get(gs.gsDirectoryExists), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    mf.pn.SetLocalMetersPerDegree(mf.pn.latitude, mf.pn.longitude);

                    dirNewField.Create();

                    mf.displayFieldName = mf.currentFieldDirectory;

                    //create the field file header info
                    mf.FileCreateField();
                    mf.FileSaveFlags();
                    mf.FileCreateBoundary();
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Creating new field " + ex);

                MessageBox.Show(gStr.Get(gs.gsError), ex.ToString());
                mf.currentFieldDirectory = "";
            }
        }

        private void CreateNewJob()
        {
            //fill something in
            if (String.IsNullOrEmpty(tboxJobName.Text.Trim()))
            {
                return;
            }

            //get the directory and make sure it exists, create if not
            DirectoryInfo dirNewJob = new DirectoryInfo(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs", tboxJobName.Text.Trim()));

            mf.menustripLanguage.Enabled = false;

            try
            {
                //create it for first save
                if (dirNewJob.Exists)
                {
                    MessageBox.Show(gStr.Get(gs.gsChooseADifferentName), gStr.Get(gs.gsDirectoryExists), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    mf.JobClose();

                    //create the job directory
                    dirNewJob.Create();

                    mf.JobNew();

                    mf.currentJobDirectory = Path.Combine("Jobs", tboxJobName.Text.Trim());
                    mf.displayJobName = Path.GetFileName(mf.currentJobDirectory);

                    //create the field file header info
                    mf.FileCreateSections();
                    mf.FileCreateContour();
                    mf.FileCreateElevation();
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Creating new Job " + ex);

                MessageBox.Show(gStr.Get(gs.gsError), ex.ToString());
                mf.currentFieldDirectory = "";
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!mf.isFieldStarted)
            {
                //fill something in
                if (String.IsNullOrEmpty(tboxFieldName.Text.Trim()))
                {
                    mf.YesMessageBox("No Field Name Entered");
                    return;
                }

                try
                {
                    //get the directory and make sure it exists, create if not
                    DirectoryInfo dirNewField = new DirectoryInfo(Path.Combine(RegistrySettings.fieldsDirectory, tboxFieldName.Text.Trim()));

                    //create it for first save
                    if (dirNewField.Exists)
                    {
                        mf.YesMessageBox($"{gStr.Get(gs.gsChooseADifferentName)} \r\n\r\n {gStr.Get(gs.gsDirectoryExists)}");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Log.EventWriter($"Catch: {ex.ToString()} ");
                    mf.YesMessageBox("Serious Error CreatingField");
                }

                CreateNewField();
            }

            //job
            if (mf.isFieldStarted && !mf.isJobStarted && tboxJobName.Text != "")
            {
                //get the directory and make sure it exists, create if not
                DirectoryInfo dirNewField = new DirectoryInfo(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs", tboxJobName.Text.Trim()));

                //create it for first save
                if (dirNewField.Exists)
                {
                    mf.YesMessageBox($"Job Creation Error \r\n\r\n{gStr.Get(gs.gsChooseADifferentName)}\r\n\r\n{gStr.Get(gs.gsDirectoryExists)}");
                    return;
                }

                CreateNewJob();
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnFieldNew_Click(object sender, EventArgs e)
        {
            if (mf.isFieldStarted) mf.FileSaveEverythingBeforeClosingField();
            tboxFieldName.Text = "";
            tboxFieldName.Enabled = true;

            tboxJobName.Text = "";
            tboxJobName.Enabled = false;

            btnFieldNew.Enabled = false;
            btnAddDate.Enabled = true;
            btnAddTime.Enabled = true;

            btnJobNew.Enabled = false;
        }

        private void btnJobNew_Click(object sender, EventArgs e)
        {
            mf.JobClose();
            tboxJobName.Text = "";
            tboxJobName.Enabled = true;

            btnFieldNew.Enabled = false;
            btnJobNew.Enabled = false;

            btnAddDateJob.Enabled = true;
            btnAddTimeJob.Enabled = true;
        }

        #region Job
        private void tboxJobName_TextChanged(object sender, EventArgs e)
        {
            if (mf.isJobStarted) return;

            TextBox textboxSender = (TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            if (tboxFieldName.Text == "" && String.IsNullOrEmpty(tboxJobName.Text.Trim()))
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        private void tboxJobName_Click(object sender, EventArgs e)
        {
            if (Settings.User.setDisplay_isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnJobCancel.Focus();
            }
        }

        private void btnCancelJob_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddDateJob_Click(object sender, EventArgs e)
        {
            tboxJobName.Text += " " + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void btnAddTimeJob_Click(object sender, EventArgs e)
        {
            tboxJobName.Text += " " + DateTime.Now.ToString("HH-mm", CultureInfo.InvariantCulture);
        }

        #endregion

    }
}