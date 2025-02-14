using AgOpenGPS.Culture;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormJobNew : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormJobNew(Form _callingForm)
        {
            //get copy of the calling main form
            mf = _callingForm as FormGPS;

            InitializeComponent();

            label1.Text = gStr.gsEnterJobName;
            this.Text = gStr.gsCreateNewJob;
        }

        private void FormJobNew_Load(object sender, EventArgs e)
        {
            btnSaveJob.Enabled = false;

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void tboxJobName_TextChanged(object sender, EventArgs e)
        {
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

        private void tboxJobName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnJobCancel.Focus();
            }
        }
    }
}