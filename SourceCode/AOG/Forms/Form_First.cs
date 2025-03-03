﻿using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class Form_First : Form
    {
        private readonly FormGPS mf = null;

        public Form_First(Form callingForm)
        {
            mf = callingForm as FormGPS;

            InitializeComponent();
        }

        private void Form_About_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "Version ";

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }

            label1.Text = RegistrySettings.vehiclesDirectory + RegistrySettings.vehicleFileName + ".xml";

            if (RegistrySettings.workingDirectory == "Default")
            {
                label7.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
            }
            else
            {
                label7.Text = RegistrySettings.workingDirectory.ToString();
            }
            label8.Text = RegistrySettings.culture;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Settings.User.setDisplay_isTermsAccepted = true;
            DialogResult = DialogResult.OK;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}