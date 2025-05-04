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
        private Settings.UserSettings _userSettings;

        public FormAgShareSettings(AgShareClient agShareClient, Settings.UserSettings userSettings)
        {
            _agShareClient = agShareClient;
            _userSettings = userSettings;
            InitializeComponent();
        }

        private void FormAgShareSettings_Load(object sender, EventArgs e)
        {
            textBoxServer.Text = _userSettings.AgShareServer;
            textBoxApiKey.Text = _userSettings.AgShareApiKey;
            UpdateAgShareToggleButton();

        }

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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //_agShareClient.SetServer(textBoxServer.Text);
            //_agShareClient.SetApiKey(textBoxApiKey.Text);

            _userSettings.AgShareServer = textBoxServer.Text;
            _userSettings.AgShareApiKey = textBoxApiKey.Text;
            Settings.User.Save();
        }
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

        private void btnToggleUpload_Click(object sender, EventArgs e)
        {
            Settings.User.AgShareUploadEnabled = !Settings.User.AgShareUploadEnabled;
            UpdateAgShareToggleButton();
            Settings.User.Save();
        }

    }
}