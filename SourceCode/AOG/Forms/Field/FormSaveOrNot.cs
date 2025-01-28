﻿using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSaveOrNot : Form
    {
        //class variables

        private int countExit = 2;
        private int countShutdown = 10;

        public FormSaveOrNot()
        {
            InitializeComponent();
        }

        private void FormSaveOrNot_Load(object sender, EventArgs e)
        {
            lblExit.Visible = !Properties.Settings.Default.setWindow_isShutdownComputer;
            lblExitCtr.Visible = !Properties.Settings.Default.setWindow_isShutdownComputer;
            lblShut.Visible = Properties.Settings.Default.setWindow_isShutdownComputer;
            lblShutCtr.Visible = Properties.Settings.Default.setWindow_isShutdownComputer;

            lblExitCtr.Text = countExit.ToString();
            lblShutCtr.Text = countShutdown.ToString();
        }

        //exit to windows
        private void btnOk_Click(object sender, EventArgs e)
        {
            //back to FormGPS
            DialogResult = DialogResult.OK;
            Properties.Settings.Default.setWindow_isShutdownComputer = false;
            Close();
        }

        //just cancel and return to AgOpenGPS
        private void btnReturn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Ignore;
            Close();
        }

        //turn off computer
        private void btnShutDown_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Properties.Settings.Default.setWindow_isShutdownComputer = true;
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.setWindow_isShutdownComputer)
            {
                countShutdown--;
                lblShutCtr.Text = countShutdown.ToString();
                if (countShutdown < 0)
                {
                    DialogResult = DialogResult.Yes;
                    Close();
                }
            }
            else
            {
                countExit--;
                lblExitCtr.Text = countExit.ToString();
                if (countExit < 0)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }
    }
}