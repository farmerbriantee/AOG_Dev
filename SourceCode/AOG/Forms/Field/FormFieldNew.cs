using AgOpenGPS.Culture;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
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

            label1.Text = gStr.gsEnterFieldName;

            label6.Text = gStr.gsEnterJobName;
        }

        private void FormFieldDir_Load(object sender, EventArgs e)
        {
            btnSaveJob.Enabled = false;

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
                btnSaveJob.Enabled = false;
            }
            else
            {
                btnSaveJob.Enabled = true;
            }
        }

        private void tboxFieldName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
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
                mf.JobClose();

                //start a new field
                mf.FieldNew();

                //create it for first save
                if (dirNewField.Exists)
                {
                    MessageBox.Show(gStr.gsChooseADifferentName, gStr.gsDirectoryExists, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    CNMEA.latStart = mf.pn.latitude; 
                    CNMEA.lonStart = mf.pn.longitude;

                    mf.pn.SetLocalMetersPerDegree(false);

                    dirNewField.Create();

                    mf.displayFieldName = mf.currentFieldDirectory;

                    //create the field file header info
                    mf.FileCreateField();
                    mf.FileCreateRecPath();
                    mf.FileCreateElevation();
                    mf.FileSaveFlags();
                    mf.FileCreateBoundary();

                    //mf.FileCreateSections();
                    //mf.FileCreateContour();
                    //mf.FileSaveABLine();
                    //mf.FileSaveCurveLine();
                    //mf.FileSaveHeadland();
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Creating new field " + ex);

                MessageBox.Show(gStr.gsError, ex.ToString());
                mf.currentFieldDirectory = "";
            }

            DialogResult = DialogResult.OK;
            Close();
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
                    MessageBox.Show(gStr.gsChooseADifferentName, gStr.gsDirectoryExists, MessageBoxButtons.OK, MessageBoxIcon.Stop);
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

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Creating new Job " + ex);

                MessageBox.Show(gStr.gsError, ex.ToString());
                mf.currentFieldDirectory = "";
            }

        }


        #region Job
        private void tboxJobName_TextChanged(object sender, EventArgs e)
        {
            if (mf.isJobStarted) return;

            TextBox textboxSender = (TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            if (String.IsNullOrEmpty(tboxJobName.Text.Trim()))
            {
                btnSaveJob.Enabled = false;
            }
            else
            {
                btnSaveJob.Enabled = true;
            }
        }

        private void tboxJobName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
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

        private void btnSaveJob_Click(object sender, EventArgs e)
        {
            if (!mf.isFieldStarted) CreateNewField();
            if (mf.isFieldStarted && !mf.isJobStarted) CreateNewJob();

            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion

        private void btnFieldNew_Click(object sender, EventArgs e)
        {
            if (mf.isFieldStarted) mf.FileSaveEverythingBeforeClosingField();
            tboxFieldName.Text = "";
            tboxJobName.Text = "";
            tboxJobName.Enabled = true;
            tboxFieldName.Enabled = true;

            btnFieldNew.Enabled = false;
            btnAddDate.Enabled = true;
            btnAddTime.Enabled = true;

            btnJobNew.Enabled = false;
        }

        private void btnJobNew_Click(object sender, EventArgs e)
        {
            mf.JobClose();
            tboxFieldName.Text = "";
            tboxJobName.Text = "";
            tboxJobName.Enabled = true;

            btnFieldNew.Enabled = false;
            btnJobNew.Enabled = false;

            btnAddDateJob.Enabled = true;
            btnAddTimeJob.Enabled = true;



        }
    }
}