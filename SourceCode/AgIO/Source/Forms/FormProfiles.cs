using AgIO.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormProfiles : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormProfiles(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();
        }

        private void FormCommSaver_Load(object sender, EventArgs e)
        {
            btnSaveAs.BackColor = Color.Transparent;
            btnSaveNewProfile.BackColor = Color.Transparent;

            lblLast.Text = "Using Profile: " + RegistrySettings.profileName;
            DirectoryInfo dinfo = new DirectoryInfo(RegistrySettings.profileDirectory);
            FileInfo[] Files = dinfo.GetFiles("*.xml");

            foreach (FileInfo file in Files)
            {
                string temp = Path.GetFileNameWithoutExtension(file.Name);
                if (temp.Trim() != "Default Profile")
                    cboxOverWrite.Items.Add(temp);
            }

            if (cboxOverWrite.Items.Count == 0)
            {
                cboxOverWrite.Enabled = false;
            }

            lblCurrentProfile.Text = RegistrySettings.profileName;

            DirectoryInfo dinfo2 = new DirectoryInfo(RegistrySettings.profileDirectory);
            FileInfo[] Files2 = dinfo2.GetFiles("*.xml");
            if (Files2.Length == 0)
            {
                cboxChooseExisting.Enabled = false;
            }
            else
            {
                foreach (FileInfo file in Files2)
                {
                    string temp = Path.GetFileNameWithoutExtension(file.Name);
                    if (temp.Trim() != "Default Profile")
                    {
                        cboxChooseExisting.Items.Add(temp);
                    }
                }
            }
        }

        private void cboxOverWrite_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show(
                "Overwrite: " + cboxOverWrite.SelectedItem.ToString() + ".xml",
                "Save And Return",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result3 == DialogResult.Yes)
            {
                //save profile in registry
                Settings.User.Save();

                RegistrySettings.profileName = SanitizeFileName(cboxOverWrite.SelectedItem.ToString().Trim());
                
                RegistrySettings.Save("ProfileName", RegistrySettings.profileName);
                Settings.User.Save();

                Close();
            }
        }

        private void tboxNewProfile_TextChanged(object sender, EventArgs e)
        {
            TextBox textboxSender = (TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;
            if (textboxSender.Text.Length > 0)
            {
                btnSaveNewProfile.BackColor = Color.LightGreen;
            }
            else
            {
                btnSaveNewProfile.BackColor = Color.Transparent;
            }
        }

        private void btnSaveNewProfile_Click(object sender, EventArgs e)
        {
            if (tboxCreateNew.Text.Trim().Length > 0 && tboxCreateNew.Text.Trim() != "Default Profile")
            {
                RegistrySettings.profileName = SanitizeFileName(tboxCreateNew.Text.ToString().Trim());

                Settings.User.Save();

                //reset to Default Profile and save
                Settings.User.Reset();

                RegistrySettings.Save("ProfileName", RegistrySettings.profileName);
                Settings.User.Save();

                DialogResult = DialogResult.Yes;
                Close();
            }
            else
            {
                _ = MessageBox.Show("Enter a File Name To Save...",
                "Save And Return", MessageBoxButtons.OK);
            }
        }

        private void tboxNewProfile_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSaveNewProfile.Focus();
            }
        }

        //save As
        private void tboxSaveAs_TextChanged(object sender, EventArgs e)
        {
            TextBox textboxSender = (TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");

            textboxSender.SelectionStart = cursorPosition;
            if (textboxSender.Text.Length > 0)
            {
                btnSaveAs.BackColor = Color.LightGreen;
            }
            else
            {
                btnSaveAs.BackColor = Color.Transparent;
            }
        }

        private void tboxSaveAs_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSaveNewProfile.Focus();
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (tboxSaveAs.Text.Trim().Length > 0 && tboxSaveAs.Text.Trim() != "Default Profile")
            {
                RegistrySettings.profileName = SanitizeFileName(tboxSaveAs.Text.ToString().Trim());

                RegistrySettings.Save("ProfileName", RegistrySettings.profileName);
                Settings.User.Save();

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                if (tboxSaveAs.Text.Trim() != "Default Profile")
                {
                    _ = MessageBox.Show("Enter a File Name To Save...",
                    "Save And Return", MessageBoxButtons.OK);
                }
                else
                {
                    _ = MessageBox.Show("Enter a File Name To Save...",
                    "You Cannot Use Default Profile", MessageBoxButtons.OK);
                }
            }
        }

        //Load Existing Profile
        private void cboxChooseExisting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxChooseExisting.SelectedItem.ToString().Trim() == "Default Profile")
            {
                mf.YesMessageBox("Choose a Different Profile, Or Create a New One");
            }
            else
            {
                //save current profile
                Settings.User.Save();

                RegistrySettings.profileName = cboxChooseExisting.SelectedItem.ToString().Trim();

                RegistrySettings.Save("ProfileName", RegistrySettings.profileName);
                Settings.User.Load();

                DialogResult = DialogResult.Yes;
                Close();
            }
        }

        //functions
        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static readonly Regex InvalidFileRegex = new Regex(string.Format("[{0}]", Regex.Escape(@"<>:""/\|?*")));

        public static string SanitizeFileName(string fileName)
        {
            return InvalidFileRegex.Replace(fileName, string.Empty);
        }
    }
}