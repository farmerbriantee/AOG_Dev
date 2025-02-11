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
            btnSave.Enabled = false;

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
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddDate_Click(object sender, EventArgs e)
        {
            tboxJobName.Text += " " + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            tboxJobName.Text += " " + DateTime.Now.ToString("HH-mm", CultureInfo.InvariantCulture);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //fill something in
            if (String.IsNullOrEmpty(tboxJobName.Text.Trim()))
            {
                return;
            }

            //append date time to name


            //get the directory and make sure it exists, create if not
            DirectoryInfo dirNewJob = new DirectoryInfo(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Jobs", tboxJobName.Text.Trim()));

            mf.currentJobDirectory = Path.Combine("Jobs", tboxJobName.Text.Trim());

            mf.menustripLanguage.Enabled = false;
            //if no template set just make a new file.
            try
            {
                //start a new field
                //mf.FieldNew();

                //create it for first save
                if (dirNewJob.Exists)
                {
                    MessageBox.Show(gStr.gsChooseADifferentName, gStr.gsDirectoryExists, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    dirNewJob.Create();

                    mf.JobbNew();

                    mf.displayJobName = mf.currentJobDirectory;

                    //create the field file header info
                    mf.FileCreateSections();
                    mf.FileCreateContour();
                    mf.FileCreateElevation();
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Creating new Job " + ex);

                MessageBox.Show(gStr.gsError, ex.ToString());
                mf.currentFieldDirectory = "";
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void tboxJobName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }
    }
}