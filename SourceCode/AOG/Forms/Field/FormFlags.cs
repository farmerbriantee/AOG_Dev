﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormFlags : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormFlags(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void UpdateLabels()
        {
            if (mf.flagNumberPicked > mf.flagPts.Count)
            {
                mf.flagNumberPicked = mf.flagPts.Count - 1;
            }
            lblLatStart.Text = mf.flagPts[mf.flagNumberPicked - 1].latitude.ToString();
            lblLonStart.Text = mf.flagPts[mf.flagNumberPicked - 1].longitude.ToString();
            lblEasting.Text = mf.flagPts[mf.flagNumberPicked - 1].easting.ToString("N2");
            lblNorthing.Text = mf.flagPts[mf.flagNumberPicked - 1].northing.ToString("N2");
            lblHeading.Text = glm.toDegrees(mf.flagPts[mf.flagNumberPicked - 1].heading).ToString("N2");
            lblFlagSelected.Text = mf.flagPts[mf.flagNumberPicked - 1].ID.ToString();
            tboxFlagNotes.Text = mf.flagPts[mf.flagNumberPicked - 1].notes;
        }

        private void FormFlags_Load(object sender, EventArgs e)
        {
            UpdateLabels();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        //need to add button to this form
        private void OpenGoogleEarthFlag_Click(object sender, EventArgs e)
        {
            //save new copy of kml with selected flag and view in GoogleEarth
            mf.FileSaveSingleFlagKML(mf.flagNumberPicked);

            Process.Start(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "Flag.KML"));
        }

        private void btnNorth_MouseDown(object sender, MouseEventArgs e)
        {
            mf.flagNumberPicked++;
            if (mf.flagNumberPicked > mf.flagPts.Count) mf.flagNumberPicked = 1;
            UpdateLabels();
        }

        private void btnSouth_MouseDown(object sender, MouseEventArgs e)
        {
            mf.flagNumberPicked--;
            if (mf.flagNumberPicked < 1) mf.flagNumberPicked = mf.flagPts.Count;
            UpdateLabels();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            mf.flagNumberPicked = 0;
            mf.FileSaveFlags();
            Close();
        }

        private void btnDeleteFlag_Click(object sender, EventArgs e)
        {
            int flag = mf.flagNumberPicked;
            if (mf.flagPts.Count > 0) mf.DeleteSelectedFlag();
            if (mf.flagPts.Count == 0)
            {
                mf.FileSaveFlags();
                Close();
                return;
            }
            if (flag > mf.flagPts.Count) mf.flagNumberPicked = mf.flagPts.Count;
            else mf.flagNumberPicked = flag;
            UpdateLabels();
        }

        private void tboxFlagNotes_Leave(object sender, EventArgs e)
        {
            if (mf.flagNumberPicked > 0)
                mf.flagPts[mf.flagNumberPicked - 1].notes = tboxFlagNotes.Text;
        }

        private void tboxFlagNotes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') e.Handled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //MakeDubinsLineFromPivotToFlag();
            if (mf.flagPts.Count > 0)
            {
                if (mf.flagNumberPicked == 0) mf.flagNumberPicked = mf.flagPts.Count;

                if (mf.flagNumberPicked > mf.flagPts.Count) mf.flagNumberPicked = mf.flagPts.Count;

                lblDistanceToFlag.Text = (glm.Distance(mf.pivotAxlePos,
                    mf.flagPts[mf.flagNumberPicked - 1].easting, mf.flagPts[mf.flagNumberPicked - 1].northing) * glm.m2FtOrM).ToString("N2") + " m";

                UpdateLabels();
            }
        }

        private void tboxFlagNotes_Click(object sender, EventArgs e)
        {
            if (Settings.User.setDisplay_isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnExit.Focus();
            }
        }
    }
}