using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AOG.Classes;

namespace AOG
{
    public partial class FormAgShareSettings : Form
    {
        private readonly AgShareClient _agShareClient;
        private Timer clipboardCheckTimer;

        // Constructor: initialize AgShareClient with current user settings
        public FormAgShareSettings()
        {
            _agShareClient = new AgShareClient(Settings.User.AgShareServer, Settings.User.AgShareApiKey);
            InitializeComponent();
        }

        // Load current settings into form and start clipboard monitoring
        private void FormAgShareSettings_Load(object sender, EventArgs e)
        {
            textBoxServer.Text = Settings.User.AgShareServer;
            textBoxApiKey.Text = Settings.User.AgShareApiKey;

            UpdateAgShareToggleButton();

            // Start clipboard check timer to enable/disable paste button
            btnPaste.Enabled = Clipboard.ContainsText();
            clipboardCheckTimer = new Timer();
            clipboardCheckTimer.Interval = 500;
            clipboardCheckTimer.Tick += ClipboardCheckTimer_Tick;
            clipboardCheckTimer.Start();
        }

        // Enable or disable the paste button based on clipboard content
        private void ClipboardCheckTimer_Tick(object sender, EventArgs e)
        {
            btnPaste.Enabled = Clipboard.ContainsText();
        }

        // Test the current server and API key by contacting the AgShare API
        private async void buttonTestConnection_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Connecting...";
            labelStatus.ForeColor = Color.Gray;

            _agShareClient.SetBaseUrl(textBoxServer.Text);
            _agShareClient.SetApiKey(textBoxApiKey.Text);

            (bool success, string message) = await _agShareClient.CheckApiAsync();

            if (success)
            {
                labelStatus.Text = "✔ Connection successful";
                labelStatus.ForeColor = Color.Green;
                buttonSave.Enabled = true;
            }
            else
            {
                labelStatus.Text = $"❌ {message}";
                labelStatus.ForeColor = Color.Red;
            }
        }

        // Save the updated server and API key to user settings
        private void buttonSave_Click(object sender, EventArgs e)
        {
            _agShareClient.SetBaseUrl(textBoxServer.Text);
            _agShareClient.SetApiKey(textBoxApiKey.Text);

            Settings.User.AgShareServer = textBoxServer.Text;
            Settings.User.AgShareApiKey = textBoxApiKey.Text;
            Settings.User.Save();
        }

        // Update the toggle upload button UI based on current setting
        private void UpdateAgShareToggleButton()
        {
            if (Settings.User.AgShareUploadEnabled)
            {
                btnToggleUpload.Image = Properties.Resources.UploadOn;
                btnToggleUpload.Text = "Activated";
            }
            else
            {
                btnToggleUpload.Image = Properties.Resources.UploadOff;
                btnToggleUpload.Text = "Deactivated";
                buttonSave.Enabled = true;
            }
        }

        // Toggle AgShare upload setting and update UI
        private void btnToggleUpload_Click(object sender, EventArgs e)
        {
            Settings.User.AgShareUploadEnabled = !Settings.User.AgShareUploadEnabled;
            UpdateAgShareToggleButton();
            Settings.User.Save();
        }

        // Paste clipboard content into the API key textbox
        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                textBoxApiKey.Text = Clipboard.GetText();
                Clipboard.Clear();
            }
        }
        // Show onscreen keyboard when textBoxServer is clicked (if enabled in settings)
        private void textBoxServer_Click(object sender, EventArgs e)
        {
            if (Settings.User.setDisplay_isKeyboardOn)
            {
                // Attempt to cast owner form to FormGPS
                FormGPS mf = this.Owner as FormGPS;
                if (mf != null)
                {
                    mf.KeyboardToText((TextBox)sender, this); // Open onscreen keyboard
                    btnPaste.Focus(); // Move focus to avoid key capture overlap
                }
            }
        }

        // Open registration URL in default browser
        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://agshare.agopengps.com/register",
                UseShellExecute = true
            });
        }

    }
}
