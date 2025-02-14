﻿using AgOpenGPS.Culture;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
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

            btnFieldOpen.Text = gStr.gsOpen;
            btnFieldNew.Text = gStr.gsNew;
            btnFieldResume.Text = gStr.gsResume;
            //btnInField.Text = gStr.gsDriveIn;
            btnFromKML.Text = gStr.gsFromKml;
            btnFromISOXML.Text = "From ISOXML";
            btnFieldClose.Text = gStr.gsClose;

            this.Text = gStr.gsStartNewField;
        }

        private void FormField_Load(object sender, EventArgs e)
        {
            //check if directory and file exists, maybe was deleted etc
            if (String.IsNullOrEmpty(mf.currentFieldDirectory)) btnFieldResume.Enabled = false;
            string directoryFieldName = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

            string fileAndDirectory = Path.Combine(directoryFieldName, "Field.txt");

            if (!File.Exists(fileAndDirectory))
            {
                lblResumeField.Text = "Field: " + gStr.gsNone;
                btnFieldResume.Enabled = false;
                mf.currentFieldDirectory = "";
                Properties.Settings.Default.setF_CurrentFieldDir = "";
            }
            else
            {
                lblResumeField.Text = "Field: " + gStr.gsResume + "\r\n" +mf.currentFieldDirectory;

                if (mf.isFieldStarted)
                {
                    btnFieldResume.Enabled = false;
                    lblResumeField.Text = "Field: " + gStr.gsOpen + "\r\n" + mf.currentFieldDirectory;
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
                    lblResumeJob.Text = "Job: " + gStr.gsNone;
                    isResumeJob = false;
                    mf.currentJobDirectory = "";
                    Properties.Settings.Default.setF_CurrentJobDir = "";
                }
                else
                {
                    lblResumeJob.Text = "Job: " + gStr.gsResume + "\r\n" + mf.currentJobDirectory;

                    if (mf.isJobStarted)
                    {
                        lblResumeField.Text = "Job: " + gStr.gsOpen + "\r\n" + mf.currentJobDirectory;
                        isResumeJob = false;
                    }
                    else
                    {
                        btnJobClose.Enabled = false;
                        btnJobOpen.Enabled = false;
                        btnJobNew.Enabled = false;
                        isResumeJob = true;
                    }
                }
            }
            else
            {
                if (mf.isJobStarted)
                    lblResumeJob.Text = gStr.gsOpen + " Job: " + mf.currentJobDirectory;
                else
                    lblResumeJob.Text = " Job: " + gStr.gsNone;
            }

            Location = Properties.Settings.Default.setFieldMenu_location;
            Size = Properties.Settings.Default.setFieldMenu_size;

            mf.CloseTopMosts();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormField_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.setFieldMenu_location = Location;
            Properties.Settings.Default.setFieldMenu_size = Size;
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

            using (FormFilePicker form = new FormFilePicker(mf))
            {
                //returns full field.txt file dir name
                if (form.ShowDialog(this) == DialogResult.Yes)
                {
                    if (mf.isFieldStarted) mf.FileSaveEverythingBeforeClosingField();
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

                        //get the directory and make sure it exists, create if not
                        DirectoryInfo dirNewJob = new DirectoryInfo(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs", mf.jobPickerFileAndDirectory));

                        mf.currentJobDirectory = Path.Combine("Jobs", mf.jobPickerFileAndDirectory);

                        mf.JobNew();

                        mf.displayJobName = mf.currentJobDirectory;

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

            Close();
        }

        private void btnFieldClose_Click(object sender, EventArgs e)
        {
            if (mf.isFieldStarted) mf.FileSaveEverythingBeforeClosingField();
            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnFieldResume_Click(object sender, EventArgs e)
        {
            if (mf.isFieldStarted) mf.FileSaveEverythingBeforeClosingField();

            //open the Resume.txt and continue from last exit
            mf.FileOpenField("Resume");

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

                mf.displayJobName = mf.currentJobDirectory;

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
            //back to FormGPS
            DialogResult = DialogResult.Retry;
            Close();
        }

        #endregion

        #region Job Btns

        private void btnJobNew_Click(object sender, EventArgs e)
        {
            if (!mf.isFieldStarted) return;
            //back to FormGPS
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void btnJobOpen_Click(object sender, EventArgs e)
        {
            if (!mf.isFieldStarted)
            {
                mf.YesMessageBox(gStr.gsFieldNotOpen + "\r\n\r\n" + gStr.gsCreateNewField);
                return;
            }

            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs");

            if (string.IsNullOrEmpty(directoryName) || (!Directory.Exists(directoryName)))
            {
                mf.YesMessageBox("No Jobs Exist\r\n\r\n" + gStr.gsCreateNewJob);
                Log.EventWriter("Job Picker, No Jobs");
                DialogResult = DialogResult.None;
                return;
            }

            mf.jobPickerFileAndDirectory = "";

            using (FormJobPicker form = new FormJobPicker(mf))
            {
                //returns full field.txt file dir name
                if (form.ShowDialog(this) == DialogResult.Yes)
                {
                    mf.JobClose();

                    //get the directory and make sure it exists, create if not
                    //DirectoryInfo dirNewJob = new DirectoryInfo(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs", mf.jobPickerFileAndDirectory));

                    mf.currentJobDirectory = Path.Combine("Jobs", mf.jobPickerFileAndDirectory);

                    mf.JobNew();

                    mf.displayJobName = mf.currentJobDirectory;

                    //create the field file header info
                    mf.FileLoadSections(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, mf.currentJobDirectory, "Sections.txt"));
                    mf.FileLoadContour(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, mf.currentJobDirectory, "Contour.txt"));

                    Close();
                }
                else
                {
                    return;
                }
            }
        }

        private void btnJobClose_Click(object sender, EventArgs e)
        {
            mf.JobClose();
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.isCancelFieldMenu = true;
        }
    }
}