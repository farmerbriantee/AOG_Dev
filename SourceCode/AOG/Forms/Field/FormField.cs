using AOG.Classes;

using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormField : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        bool isResumeJob = false;

        public FormField(Form callingForm)
        {
            //get ref of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            btnFieldOpen.Text = gStr.Get(gs.gsOpen);
            btnFieldNew.Text = gStr.Get(gs.gsNew);
            btnFieldResume.Text = gStr.Get(gs.gsResume);
            btnFromKML.Text = gStr.Get(gs.gsFromKml);
            btnFromISOXML.Text = "From ISOXML";
            btnFieldClose.Text = gStr.Get(gs.gsClose);
            this.Text = gStr.Get(gs.gsStartNewField);
            btnFromAgShare.Enabled = Settings.User.AgShareUploadEnabled;
        }

        private void FormField_Load(object sender, EventArgs e)
        {
            //check if directory and file exists, maybe was deleted etc
            if (String.IsNullOrEmpty(mf.currentFieldDirectory)) btnFieldResume.Enabled = false;
            string directoryFieldName = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

            string fileAndDirectory = Path.Combine(directoryFieldName, "Field.txt");

            if (!File.Exists(fileAndDirectory))
            {
                lblResumeField.Text = "Field: " + gStr.Get(gs.gsNone);
                btnFieldResume.Enabled = false;
                mf.currentFieldDirectory = "";
                Settings.Vehicle.setF_CurrentFieldDir = "";
            }
            else
            {
                lblResumeField.Text = $"Field: {gStr.Get(gs.gsResume)}: {mf.currentFieldDirectory}";

                if (mf.isFieldStarted)
                {
                    btnFieldResume.Enabled = false;
                    lblResumeField.Text = $"Field: {gStr.Get(gs.gsOpen)}: {mf.currentFieldDirectory}";
                }
                else
                {
                    btnFieldClose.Enabled = false;
                }
            }

            if (btnFieldResume.Enabled)
            {
                fileAndDirectory = Path.Combine(directoryFieldName, mf.currentJobDirectory, "Sections.txt");

                if (!File.Exists(fileAndDirectory))
                {
                    lblResumeJob.Text = $"Job: {gStr.Get(gs.gsNone)}";
                    isResumeJob = false;
                    mf.currentJobDirectory = "";
                    Settings.Vehicle.setF_CurrentJobDir = "";
                }
                else
                {
                    lblResumeJob.Text = $"Job: { gStr.Get(gs.gsResume)}: {Path.GetFileName(mf.currentJobDirectory)}";

                    if (mf.isJobStarted)
                    {
                        lblResumeField.Text = $"Job: {gStr.Get(gs.gsOpen)}: {Path.GetFileName(mf.currentJobDirectory)}";
                        isResumeJob = false;
                    }
                    else
                    {
                        isResumeJob = true;
                    }
                }
            }
            else
            {
                if (mf.isJobStarted)
                    lblResumeJob.Text = $"Job: { gStr.Get(gs.gsOpen)}: {Path.GetFileName(mf.currentJobDirectory)}";
                else
                    lblResumeJob.Text = $" Job: {gStr.Get(gs.gsNone)}";
            }

            Location = Settings.User.setWindow_FieldMenulocation;
            Size = Settings.User.setWindow_FieldMenuSize;

            mf.CloseTopMosts();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormField_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.User.setWindow_FieldMenulocation = Location;
            Settings.User.setWindow_FieldMenuSize = Size;
        }

        #region Field Btns
        private void btnFieldNew_Click(object sender, EventArgs e)
        {
            //back to FormGPS
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnFieldOpen_Click(object sender, EventArgs e)
        {
            mf.filePickerFileAndDirectory = "";
            mf.jobPickerFileAndDirectory = "";
            this.Hide();

            using (FormFieldOpen form = new FormFieldOpen(mf))
            {
                //returns full field.txt file dir name
                if (form.ShowDialog(this) == DialogResult.Yes)
                {
                    mf.FileOpenField(mf.filePickerFileAndDirectory);

                    if (!mf.isFieldStarted)
                    {
                        mf.YesMessageBox("Field Not Loaded - \r\n\r\n This is really bad. Field is corrupt ");
                        return;
                    }

                    if (mf.jobPickerFileAndDirectory == "Newww") //create new job
                    {
                        using (var form2 = new FormJobNew(mf))
                        { form2.ShowDialog(mf); }
                    }
                    else if (mf.jobPickerFileAndDirectory != "" )
                    {
                        mf.JobClose();

                        mf.currentJobDirectory = Path.Combine("Jobs", mf.jobPickerFileAndDirectory);

                        mf.JobNew();

                        mf.displayJobName = Path.GetFileName(mf.currentJobDirectory);

                        //create the field file header info
                        mf.FileLoadSections(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, mf.currentJobDirectory, "Sections.txt"));
                        mf.FileLoadContour(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, mf.currentJobDirectory, "Contour.txt"));
                    }
                }
                else
                {
                    //todo all closed still
                    return;
                }
            }

            mf.FieldMenuButtonEnableDisable(mf.isJobStarted);

            mf.toolStripBtnFieldTools.Enabled = mf.isJobStarted;


            Close();
        }

        private void btnFieldClose_Click(object sender, EventArgs e)
        {
            if (mf.isFieldStarted)
            {
                mf.FileSaveEverythingBeforeClosingField();
            }
            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnFieldResume_Click(object sender, EventArgs e)
        {
            //open the Resume.txt and continue from last exit
            string fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Field.txt");

            mf.FileOpenField(fileAndDirectory);

            Log.EventWriter("Field Form, Field Resume");

            if (isResumeJob)
            {
                string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs");

                if (string.IsNullOrEmpty(directoryName) || (!Directory.Exists(directoryName)))
                {
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                mf.JobNew();

                mf.displayJobName = Path.GetFileName(mf.currentJobDirectory);

                //create the field file header info
                mf.FileLoadSections(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, mf.currentJobDirectory, "Sections.txt"));
                mf.FileLoadContour(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, mf.currentJobDirectory, "Contour.txt"));
            }

            isResumeJob = false;

            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnFromKML_Click(object sender, EventArgs e)
        {
            if (mf.isFieldStarted) mf.FileSaveEverythingBeforeClosingField();
            //back to FormGPS
            DialogResult = DialogResult.No;
            Close();
        }

        private void btnFromISOXML_Click(object sender, EventArgs e)
        {
            if (mf.isFieldStarted) mf.FileSaveEverythingBeforeClosingField();
            //back to FormGPS
            DialogResult = DialogResult.Retry;
            Close();
        }
        private void btnFromAgShare_Click(object sender, EventArgs e)
        {
            this.Close();

            using (var form = new AOG.Forms.Field.FormAgShareDownloader(mf))
            {
                form.ShowDialog(this);
            }
        }

        #endregion

        #region Job Btns

        private void btnJobClose_Click(object sender, EventArgs e)
        {
            mf.JobClose();
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}