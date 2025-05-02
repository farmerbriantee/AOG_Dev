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
    }
}