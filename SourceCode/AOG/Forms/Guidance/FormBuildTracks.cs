﻿using AOG.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace AOG
{
    public partial class FormBuildTracks : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;

        private CTrk originalLine;
        private CTrk selectedItem;
        private bool isSaving = false;
        public List<CTrk> gTemp = new List<CTrk>();

        private bool isRefRightSide = true; //left side 0 middle 1 right 2

        private bool isOn = true;

        public FormBuildTracks(Form _mf)
        {
            mf = _mf as FormGPS;
            InitializeComponent();

            //btnPausePlay.Text = gStr.Get(gs.gsPause;
            this.Text = "Tracks";
        }

        private void FormBuildTracks_Load(object sender, EventArgs e)
        {
            originalLine = mf.trk.currTrk;

            gTemp.Clear();

            foreach (var item in mf.trk.gArr)
            {
                gTemp.Add(new CTrk(item));

                if (item == mf.trk.currTrk)
                    selectedItem = item;
            }

            panelMain.Top = 3; panelMain.Left = 3;
            panelCurve.Top = 3; panelCurve.Left = 3;
            panelName.Top = 3; panelName.Left = 3;
            panelKML.Top = 3; panelKML.Left = 3;
            panelEditName.Top = 3; panelEditName.Left = 3;
            panelChoose.Top = 3; panelChoose.Left = 3;
            panelABLine.Top = 3; panelABLine.Left = 3;
            panelAPlus.Top = 3; panelAPlus.Left = 3;
            panelLatLonPlus.Top = 3; panelLatLonPlus.Left = 3;
            panelLatLonLatLon.Top = 3; panelLatLonLatLon.Left = 3;
            panelPivot.Top = 3; panelPivot.Left = 3;
            panelPivot3Pt.Top = 3; panelPivot3Pt.Left = 3;

            panelEditName.Visible = false;
            panelMain.Visible = true;
            panelCurve.Visible = false;
            panelName.Visible = false;
            panelKML.Visible = false;
            panelChoose.Visible = false;
            panelABLine.Visible = false;
            panelAPlus.Visible = false;
            panelLatLonPlus.Visible = false;
            panelLatLonLatLon.Visible = false;
            panelPivot.Visible = false;
            panelPivot3Pt.Visible = false;

            this.Size = new System.Drawing.Size(650, 480);

            Location = Settings.User.setWindow_buildTracksLocation;


            nudLatitudeA.Value = mf.pn.latitude;
            nudLatitudeB.Value = mf.pn.latitude + 0.000005;
            nudLongitudeA.Value = mf.pn.longitude;
            nudLongitudeB.Value = mf.pn.longitude + 0.000005;
            nudLatitudePlus.Value = mf.pn.latitude;
            nudLongitudePlus.Value = mf.pn.longitude;
            nudHeading.Value = 0;
            nudHeadingLatLonPlus.Value = 0;

            UpdateTable();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormBuildTracks_FormClosing(object sender, FormClosingEventArgs e)
        {
            //reset to generate new reference
            mf.trk.designPtsList?.Clear();

            if (isSaving)
            {
                mf.FileSaveTracks();

                if (selectedItem != null && selectedItem.isVisible)
                    mf.trk.currTrk = selectedItem;
                else
                    mf.trk.GetNextTrack();
            }
            else
            {
                mf.trk.gArr = gTemp;
                mf.trk.currTrk = originalLine;//test this!
            }

            Settings.User.setWindow_buildTracksLocation = Location;

            mf.PanelUpdateRightAndBottom();
        }

        private void btnCancelMain_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnListUse_Click(object sender, EventArgs e)
        {
            isSaving = true;
            Close();
        }

        #region Main Controls

        private void UpdateTable()
        {
            int scrollPixels = flp.VerticalScroll.Value;

            Font backupfont = new Font(Font.FontFamily, 18F, FontStyle.Regular);
            flp.Controls.Clear();

            foreach (var track in mf.trk.gArr)
            {
                //outer inner
                Button a = new Button
                {
                    Margin = new Padding(20, 10, 2, 10),
                    Size = new Size(40, 25),
                    Tag = track,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                a.Click += A_Click;

                if (track.isVisible)
                    a.BackColor = System.Drawing.Color.Green;
                else
                    a.BackColor = System.Drawing.Color.Red;

                Button b = new Button
                {
                    Margin = new Padding(1, 10, 3, 10),
                    Size = new Size(35, 25),
                    Tag = track,
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatStyle = FlatStyle.Flat,
                };

                if (track.mode == TrackMode.AB)
                    b.Image = Properties.Resources.TrackLine;
                else if (track.mode == TrackMode.waterPivot)
                    b.Image = Properties.Resources.TrackPivot;
                else
                    b.Image = Properties.Resources.TrackCurve;

                b.FlatAppearance.BorderSize = 0;

                TextBox t = new TextBox
                {
                    Margin = new Padding(3),
                    Size = new Size(330, 35),
                    Text = track.name,
                    Tag = track,
                    Font = backupfont,
                    ReadOnly = true
                };
                t.GotFocus += (s, e) => t.Parent.Focus(); // Remove focus when clicked
                t.Click += LineSelected_Click;
                t.Cursor = System.Windows.Forms.Cursors.Default;
                t.ForeColor = track.isVisible ? Color.Black : Color.Gray;
                t.BackColor = track == selectedItem ? Color.LightBlue : Color.AliceBlue;

                flp.Controls.Add(b);
                flp.Controls.Add(t);
                flp.Controls.Add(a);
            }

            flp.VerticalScroll.Value = 1;
            flp.VerticalScroll.Value = scrollPixels;
            flp.PerformLayout();
        }

        private void A_Click(object sender, EventArgs e)
        {
            if (sender is Button b && b.Tag is CTrk track)
            {
                int line = mf.trk.gArr.IndexOf(track);
                track.isVisible = !track.isVisible;

                for (int i = 0; i < mf.trk.gArr.Count; i++)
                {
                    flp.Controls[(i) * 3 + 1].BackColor = Color.AliceBlue;
                }

                b.BackColor = track.isVisible ? System.Drawing.Color.Green : System.Drawing.Color.Red;

                flp.Controls[(line) * 3 + 1].ForeColor = track.isVisible ? System.Drawing.Color.Black : System.Drawing.Color.Gray;
            }
        }

        private void LineSelected_Click(object sender, EventArgs e)
        {
            if (sender is TextBox t && t.Tag is CTrk track)
            {
                int line = mf.trk.gArr.IndexOf(track);
                int numLines = mf.trk.gArr.Count;

                //un highlight selected item
                for (int i = 0; i < numLines; i++)
                {
                    flp.Controls[(i) * 3 + 1].BackColor = Color.AliceBlue;
                }

                if (track.isVisible)
                {
                    //a different line was selcted and one already was
                    selectedItem = track;
                    flp.Controls[(line) * 3 + 1].BackColor = Color.LightBlue;
                }
            }
        }

        private void btnMoveUPGN_Click(object sender, EventArgs e)
        {
            int index = mf.trk.gArr.IndexOf(selectedItem);
            if (selectedItem == null || index == 0)
                return;

            mf.trk.gArr.Reverse(index - 1, 2);

            int scrollPixels = flp.VerticalScroll.Value;

            scrollPixels -= 45;
            if (scrollPixels < 0) scrollPixels = 0;

            flp.VerticalScroll.Value = 1;
            flp.VerticalScroll.Value = scrollPixels;
            flp.PerformLayout();

            UpdateTable();
        }

        private void btnMoveDn_Click(object sender, EventArgs e)
        {
            int index = mf.trk.gArr.IndexOf(selectedItem);

            if (selectedItem == null || index == (mf.trk.gArr.Count - 1))
                return;

            mf.trk.gArr.Reverse(index, 2);

            int scrollPixels = flp.VerticalScroll.Value;

            scrollPixels += 45;
            if (scrollPixels > flp.VerticalScroll.Maximum) scrollPixels = flp.VerticalScroll.Maximum;

            flp.VerticalScroll.Value = 1;
            flp.VerticalScroll.Value = scrollPixels;
            flp.PerformLayout();

            UpdateTable();
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            if (selectedItem != null)
            {
                vec2 temp = selectedItem.ptA;
                selectedItem.ptA = selectedItem.ptB;
                selectedItem.ptB = temp;

                selectedItem.heading += Math.PI;
                if (selectedItem.heading > glm.twoPI) selectedItem.heading -= glm.twoPI;

                if (selectedItem.mode != TrackMode.AB)
                {
                    int cnt = selectedItem.curvePts.Count;
                    if (cnt > 0)
                    {
                        selectedItem.curvePts.Reverse();

                        vec3[] arr = new vec3[cnt];
                        cnt--;
                        selectedItem.curvePts.CopyTo(arr);
                        selectedItem.curvePts.Clear();

                        selectedItem.heading += Math.PI;
                        if (selectedItem.heading < 0) selectedItem.heading += glm.twoPI;
                        if (selectedItem.heading > glm.twoPI) selectedItem.heading -= glm.twoPI;

                        for (int i = 1; i < cnt; i++)
                        {
                            vec3 pt3 = arr[i];
                            pt3.heading += Math.PI;
                            if (pt3.heading > glm.twoPI) pt3.heading -= glm.twoPI;
                            if (pt3.heading < 0) pt3.heading += glm.twoPI;
                            selectedItem.curvePts.Add(new vec3(pt3));
                        }
                    }
                }

                UpdateTable();
                flp.Focus();

                mf.TimedMessageBox(1500, "A B Swapped", "Curve is Reversed");
            }
        }

        private void btnHideShow_Click(object sender, EventArgs e)
        {
            isOn = !isOn;

            for (int i = 0; i < mf.trk.gArr.Count; i++)
            {
                mf.trk.gArr[i].isVisible = isOn;
            }

            UpdateTable();
        }

        private void btnNewTrack_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelMain.Visible = false;
            panelCurve.Visible = false;
            panelName.Visible = false;
            panelABLine.Visible = false;
            panelAPlus.Visible = false;
            panelKML.Visible = false;

            mf.trk.designPtsList?.Clear();
            panelChoose.Visible = true;
        }

        private void btnListDelete_Click(object sender, EventArgs e)
        {
            if (selectedItem != null)
            {
                mf.trk.gArr.Remove(selectedItem);
                selectedItem = null;

                UpdateTable();
                flp.Focus();
            }
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (selectedItem != null)
            {
                panelMain.Visible = false;
                panelName.Visible = true;
                this.Size = new System.Drawing.Size(270, 360);

                selectedItem = new CTrk(selectedItem);
                mf.trk.gArr.Add(selectedItem);

                textBox1.Text = selectedItem.name + " Copy";
            }
        }

        private void btnEditName_Click(object sender, EventArgs e)
        {
            if (selectedItem != null)
            {
                textBox2.Text = selectedItem.name;

                panelMain.Visible = false;
                panelEditName.Visible = true;

                this.Size = new System.Drawing.Size(270, 360);
            }
        }

        #endregion Main Controls

        #region Pick

        private void btnzABCurve_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelCurve.Visible = true;

            btnACurve.Enabled = true;
            btnBCurve.Enabled = false;
            btnPausePlay.Enabled = false;
            mf.trk.designPtsList?.Clear();

            this.Size = new System.Drawing.Size(270, 360);
            mf.Activate();
        }

        private void btnzAPlus_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelAPlus.Visible = true;

            btnAPlus.Enabled = true;
            mf.trk.designPtsList?.Clear();
            nudHeading.Enabled = false;

            this.Size = new System.Drawing.Size(270, 360);
            mf.Activate();
        }

        private void btnzABLine_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelABLine.Visible = true;

            btnALine.Enabled = true;
            btnBLine.Enabled = false;
            btnPausePlay.Enabled = false;
            mf.trk.designPtsList?.Clear();

            this.Size = new System.Drawing.Size(270, 360);
            mf.Activate();
        }

        private void btnzLatLonPlusHeading_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelLatLonPlus.Visible = true;
            this.Size = new System.Drawing.Size(370, 460);

            nudLatitudePlus.Value = mf.pn.latitude;
            nudLongitudePlus.Value = mf.pn.longitude;
            mf.Activate();
        }

        private void btnzLatLon_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelLatLonLatLon.Visible = true;
            this.Size = new System.Drawing.Size(370, 460);
            mf.Activate();
        }

        private void btnLatLonPivot_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelPivot.Visible = true;
            this.Size = new System.Drawing.Size(370, 360);

            nudLatitudePivot.Value = mf.pn.latitude;
            nudLongitudePivot.Value = mf.pn.longitude;
            mf.Activate();
        }

        private void btnPivot3Pt_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelPivot3Pt.Visible = true;

            mf.trk.designPtsList?.Clear();
            mf.trk.designPtA.easting = 20000;
            mf.trk.designPtB.easting = 20000;

            this.Size = new System.Drawing.Size(270, 360);
            mf.Activate();
        }

        #endregion Pick

        #region Curve

        private void btnRefSideCurve_Click(object sender, EventArgs e)
        {
            isRefRightSide = !isRefRightSide;
            btnRefSideCurve.Image = isRefRightSide ?
            Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            mf.Activate();
        }

        private void btnACurve_Click(object sender, System.EventArgs e)
        {
            if (mf.trk.isMakingCurveTrack)
            {
                mf.trk.designPtsList.Add(new vec3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, mf.pivotAxlePos.heading));
                btnBCurve.Enabled = mf.trk.designPtsList.Count > 3;
            }
            else
            {
                mf.trk.designPtA.easting = mf.pivotAxlePos.easting;
                mf.trk.designPtA.northing = mf.pivotAxlePos.northing;

                lblCurveExists.Text = gStr.Get(gs.gsDriving);

                btnBCurve.Enabled = true;
                btnACurve.Enabled = false;
                btnACurve.Image = Properties.Resources.PointAdd;

                btnPausePlay.Enabled = true;
                btnPausePlay.Visible = true;

                mf.trk.isMakingCurveTrack = true;
                mf.trk.isRecordingCurveTrack = true;
            }
            mf.Activate();
        }

        private void btnBCurve_Click(object sender, System.EventArgs e)
        {
            mf.trk.isMakingCurveTrack = false;
            mf.trk.isRecordingCurveTrack = false;
            panelCurve.Visible = false;
            panelName.Visible = true;

            int cnt = mf.trk.designPtsList.Count;
            if (cnt > 3)
            {
                //make sure point distance isn't too big
                mf.trk.MakePointMinimumSpacing(ref mf.trk.designPtsList, 1.6);
                mf.trk.CalculateHeadings(ref mf.trk.designPtsList);

                var track = new CTrk(TrackMode.Curve);

                //calculate average heading of line
                double x = 0, y = 0;
                foreach (vec3 pt in mf.trk.designPtsList)
                {
                    x += Math.Cos(pt.heading);
                    y += Math.Sin(pt.heading);
                }
                x /= mf.trk.designPtsList.Count;
                y /= mf.trk.designPtsList.Count;
                double aveLineHeading = Math.Atan2(y, x);
                if (aveLineHeading < 0) aveLineHeading += glm.twoPI;

                track.heading = aveLineHeading;

                mf.trk.SmoothAB(ref mf.trk.designPtsList, 4, false);
                mf.trk.CalculateHeadings(ref mf.trk.designPtsList);

                //write out the Curve Points
                foreach (vec3 item in mf.trk.designPtsList)
                {
                    track.curvePts.Add(item);
                }

                textBox1.Text = "Cu " +
                    (Math.Round(glm.toDegrees(aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

                double dist = (Settings.Tool.toolWidth - Settings.Tool.overlap) * (isRefRightSide ? 0.5 : -0.5) + Settings.Tool.offset;

                mf.trk.NudgeRefTrack(track, dist);

                track.ptA = new vec2(track.curvePts[0]);
                track.ptB = new vec2(track.curvePts[track.curvePts.Count - 1]);

                //build the tail extensions
                mf.trk.AddFirstLastPoints(ref track.curvePts, 100);

                mf.trk.gArr.Add(track);
                selectedItem = track;

                panelCurve.Visible = false;
                panelName.Visible = true;
            }
            else
            {
                panelMain.Visible = true;
                panelCurve.Visible = false;
                panelName.Visible = false;
                panelChoose.Visible = false;
                this.Size = new System.Drawing.Size(650, 480);
            }

            mf.trk.designPtsList?.Clear();
            mf.Activate();
        }

        private void btnPausePlayCurve_Click(object sender, EventArgs e)
        {
            if (mf.trk.isRecordingCurveTrack)
            {
                mf.trk.isRecordingCurveTrack = false;
                btnPausePlay.Image = Properties.Resources.BoundaryRecord;
                //btnPausePlay.Text = gStr.Get(gs.gsRecord;
                btnACurve.Enabled = true;
            }
            else
            {
                mf.trk.isRecordingCurveTrack = true;
                btnPausePlay.Image = Properties.Resources.boundaryPause;
                //btnPausePlay.Text = gStr.Get(gs.gsPause;
                btnACurve.Enabled = false;
            }
            btnBCurve.Enabled = mf.trk.designPtsList.Count > 3;
            mf.Activate();
        }

        #endregion Curve

        #region AB Line

        private void btnRefSideAB_Click(object sender, EventArgs e)
        {
            isRefRightSide = !isRefRightSide;
            btnRefSideAB.Image = isRefRightSide ?
            Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            mf.Activate();
        }

        private void btnALine_Click(object sender, EventArgs e)
        {
            mf.trk.isMakingABLine = true;
            btnALine.Enabled = false;

            mf.trk.designPtA = new vec2(mf.pivotAxlePos);

            mf.trk.designPtB.easting = mf.trk.designPtA.easting - (Math.Sin(mf.pivotAxlePos.heading) * 1);
            mf.trk.designPtB.northing = mf.trk.designPtA.northing - (Math.Cos(mf.pivotAxlePos.heading) * 1);

            mf.trk.designLineEndA.easting = mf.trk.designPtA.easting - (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.trk.designLineEndA.northing = mf.trk.designPtA.northing - (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            mf.trk.designLineEndB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.trk.designLineEndB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            btnBLine.Enabled = true;
            btnALine.Enabled = false;

            btnEnter_AB.Enabled = true;

            timer1.Enabled = true;
            mf.Activate();
        }

        private void btnBLine_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            mf.trk.designPtB = new vec2(mf.pivotAxlePos);
            btnBLine.BackColor = System.Drawing.Color.Teal;

            mf.trk.designHeading = Math.Atan2(mf.trk.designPtB.easting - mf.trk.designPtA.easting,
               mf.trk.designPtB.northing - mf.trk.designPtA.northing);
            if (mf.trk.designHeading < 0) mf.trk.designHeading += glm.twoPI;

            //make sure line is long enough
            double len = glm.Distance(mf.trk.designPtA, mf.trk.designPtB);
            if (len < 20)
            {
                mf.trk.designPtB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 30);
                mf.trk.designPtB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 30);
            }

            mf.trk.designLineEndA.easting = mf.trk.designPtA.easting - (Math.Sin(mf.trk.designHeading) * 1000);
            mf.trk.designLineEndA.northing = mf.trk.designPtA.northing - (Math.Cos(mf.trk.designHeading) * 1000);

            mf.trk.designLineEndB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 1000);
            mf.trk.designLineEndB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 1000);
            mf.Activate();
        }

        private void btnEnter_AB_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            mf.trk.isMakingABLine = false;

            mf.trk.CreateDesignedABTrack(isRefRightSide);

            textBox1.Text = "AB: " +
                (Math.Round(glm.toDegrees(mf.trk.designHeading), 5)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

            panelABLine.Visible = false;
            panelName.Visible = true;
            mf.Activate();
        }

        #endregion AB Line

        #region A Plus

        private void btnRefSideAPlus_Click(object sender, EventArgs e)
        {
            isRefRightSide = !isRefRightSide;
            btnRefSideAPlus.Image = isRefRightSide ?
            Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            mf.Activate();
        }

        private void btnAPlus_Click(object sender, EventArgs e)
        {
            mf.trk.isMakingABLine = true;

            mf.trk.designPtA = new vec2(mf.pivotAxlePos);

            mf.trk.designPtB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.pivotAxlePos.heading) * 30);
            mf.trk.designPtB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.pivotAxlePos.heading) * 30);

            mf.trk.designLineEndA.easting = mf.trk.designPtA.easting - (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.trk.designLineEndA.northing = mf.trk.designPtA.northing - (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            mf.trk.designLineEndB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.trk.designLineEndB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            mf.trk.designHeading = mf.pivotAxlePos.heading;

            btnEnter_AB.Enabled = true;
            nudHeading.Enabled = true;

            nudHeading.Value = glm.toDegrees(mf.trk.designHeading);
            timer1.Enabled = true;
            mf.Activate();
        }

        private void nudHeading_ValueChanged(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            //original A pt.
            mf.trk.designHeading = glm.toRadians((double)nudHeading.Value);

            //start end of line
            mf.trk.designPtB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 30);
            mf.trk.designPtB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 30);

            mf.trk.designLineEndA.easting = mf.trk.designPtA.easting - (Math.Sin(mf.trk.designHeading) * 1000);
            mf.trk.designLineEndA.northing = mf.trk.designPtA.northing - (Math.Cos(mf.trk.designHeading) * 1000);

            mf.trk.designLineEndB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 1000);
            mf.trk.designLineEndB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 1000);

            mf.Activate();
        }

        private void btnEnter_APlus_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            mf.trk.isMakingABLine = false;

            mf.trk.CreateDesignedABTrack(isRefRightSide);

            textBox1.Text = "A+" +
                (Math.Round(glm.toDegrees(mf.trk.designHeading), 5)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

            panelAPlus.Visible = false;
            panelName.Visible = true;
            mf.Activate();
        }

        #endregion A Plus

        private void timer1_Tick(object sender, EventArgs e)
        {
            mf.trk.designPtB = new vec2(mf.pivotAxlePos);

            mf.trk.designHeading = Math.Atan2(mf.trk.designPtB.easting - mf.trk.designPtA.easting,
               mf.trk.designPtB.northing - mf.trk.designPtA.northing);
            if (mf.trk.designHeading < 0) mf.trk.designHeading += glm.twoPI;

            mf.trk.designLineEndA.easting = mf.trk.designPtA.easting - (Math.Sin(mf.trk.designHeading) * 1000);
            mf.trk.designLineEndA.northing = mf.trk.designPtA.northing - (Math.Cos(mf.trk.designHeading) * 1000);

            mf.trk.designLineEndB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 1000);
            mf.trk.designLineEndB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 1000);
        }

        #region KML Curve and line

        private void btnLoadABFromKML_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelKML.Visible = true;

            this.Size = new System.Drawing.Size(270, 360);

            string fileAndDirectory;
            {
                //create the dialog instance
                OpenFileDialog ofd = new OpenFileDialog
                {
                    //set the filter to text KML only
                    Filter = "KML files (*.KML)|*.KML",

                    //the initial directory, fields, for the open dialog
                    InitialDirectory = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory)
                };

                //was a file selected
                if (ofd.ShowDialog(this) == DialogResult.Cancel) return;
                else fileAndDirectory = ofd.FileName;
            }

            XmlDocument doc = new XmlDocument
            {
                PreserveWhitespace = false
            };

            try
            {
                doc.Load(fileAndDirectory);
                string trackName = Path.GetFileName(fileAndDirectory);
                trackName = trackName.Substring(0, trackName.Length - 4);

                XmlElement root = doc.DocumentElement;
                XmlNodeList trackList = root.GetElementsByTagName("coordinates");
                XmlNodeList namelist = root.GetElementsByTagName("name");

                if (namelist.Count > 1)
                {
                    trackName = namelist[1].InnerText;
                }

                //each element in the list is a track
                for (int i = 0; i < trackList.Count; i++)
                {
                    string line = trackList[i].InnerText;
                    line.Trim();
                    //line = coordinates;
                    char[] delimiterChars = { ' ', '\t', '\r', '\n' };
                    string[] numberSets = line.Split(delimiterChars);

                    var designPtsList = new List<vec3>();
                    //at least 3 points
                    if (numberSets.Length > 1)
                    {
                        foreach (string item in numberSets)
                        {
                            string[] fix = item.Split(',');
                            if (fix.Length != 3) continue;
                            double.TryParse(fix[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double lonK);
                            double.TryParse(fix[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double latK);

                            mf.pn.ConvertWGS84ToLocal(latK, lonK, out double norting, out double easting);

                            designPtsList.Add(new vec3(easting, norting, 0));
                        }
                    }

                    //2 points
                    if (designPtsList.Count == 2)
                    {
                        mf.trk.designPtA.easting = designPtsList[0].easting;
                        mf.trk.designPtA.northing = designPtsList[0].northing;

                        mf.trk.designPtB.easting = designPtsList[1].easting;
                        mf.trk.designPtB.northing = designPtsList[1].northing;

                        mf.trk.designHeading = Math.Atan2(mf.trk.designPtB.easting - mf.trk.designPtA.easting,
                           mf.trk.designPtB.northing - mf.trk.designPtA.northing);
                        if (mf.trk.designHeading < 0) mf.trk.designHeading += glm.twoPI;

                        if (namelist.Count > i)
                        {
                            trackName = namelist[i + 1].InnerText;
                        }
                        else trackName = "AB: " +
                            (Math.Round(glm.toDegrees(mf.trk.designHeading), 5)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

                        mf.trk.CreateDesignedABTrack(isRefRightSide);

                        //create a name
                        mf.trk.gArr[mf.trk.gArr.Count - 1].name = trackName;
                    }
                    else if (designPtsList.Count > 2)
                    {
                        //make sure point distance isn't too big
                        mf.trk.MakePointMinimumSpacing(ref designPtsList, 1.6);
                        mf.trk.CalculateHeadings(ref designPtsList);

                        var track = new CTrk(TrackMode.Curve);

                        track.ptA = new vec2(designPtsList[0]);
                        track.ptB = new vec2(designPtsList[designPtsList.Count - 1]);

                        //calculate average heading of line
                        double x = 0, y = 0;
                        foreach (vec3 pt in designPtsList)
                        {
                            x += Math.Cos(pt.heading);
                            y += Math.Sin(pt.heading);
                        }
                        x /= designPtsList.Count;
                        y /= designPtsList.Count;
                        double aveLineHeading = Math.Atan2(y, x);
                        if (aveLineHeading < 0) aveLineHeading += glm.twoPI;
                        track.heading = aveLineHeading;

                        //build the tail extensions
                        mf.trk.AddFirstLastPoints(ref designPtsList, 100);
                        //mf.trk.SmoothAB(ref designPtsList, 4, false);
                        mf.trk.CalculateHeadings(ref designPtsList);

                        //write out the Curve Points
                        track.curvePts = designPtsList;

                        if (namelist.Count > i)
                        {
                            trackName = namelist[i + 1].InnerText;
                        }
                        else trackName = "Cu " +
                                 (Math.Round(glm.toDegrees(aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

                        track.name = trackName;

                        mf.trk.gArr.Add(track);
                        selectedItem = track;
                    }
                    else
                    {
                        mf.TimedMessageBox(2000, gStr.Get(gs.gsErrorreadingKML), gStr.Get(gs.gsMissingABLinesFile));
                    }
                }
            }
            catch (Exception ed)
            {
                Log.EventWriter("Catch Error: Tracks from KML " + ed.ToString());
                return;
            }

            panelKML.Visible = false;
            panelName.Visible = false;
            panelMain.Visible = true;

            this.Size = new System.Drawing.Size(650, 480);

            UpdateTable();
            flp.Focus();
        }

        #endregion KML Curve and line

        #region LatLon LatLon

        private void btnFillLatLonLatLonA_Click(object sender, EventArgs e)
        {
            nudLatitudeA.Value = mf.pn.latitude;
            nudLongitudeA.Value = mf.pn.longitude;
        }

        private void btnFillLatLonLatLonB_Click(object sender, EventArgs e)
        {
            nudLatitudeB.Value = mf.pn.latitude;
            nudLongitudeB.Value = mf.pn.longitude;
        }

        public void CalcHeadingAB()
        {
            mf.pn.ConvertWGS84ToLocal((double)nudLatitudeA.Value, (double)nudLongitudeA.Value, out double nort, out double east);

            mf.trk.designPtA.easting = east;
            mf.trk.designPtA.northing = nort;

            mf.pn.ConvertWGS84ToLocal((double)nudLatitudeB.Value, (double)nudLongitudeB.Value, out nort, out east);
            mf.trk.designPtB.easting = east;
            mf.trk.designPtB.northing = nort;

            //calc heading
            mf.trk.designHeading = Math.Atan2(mf.trk.designPtB.easting - mf.trk.designPtA.easting,
               mf.trk.designPtB.northing - mf.trk.designPtA.northing);
            if (mf.trk.designHeading < 0) mf.trk.designHeading += glm.twoPI;

            //make sure line is long enough
            double len = glm.Distance(mf.trk.designPtA, mf.trk.designPtB);
            if (len < 20)
            {
                mf.trk.designPtB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.trk.designHeading) * 30);
                mf.trk.designPtB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.trk.designHeading) * 30);
            }
        }

        private void btnEnter_LatLonLatLon_Click(object sender, EventArgs e)
        {
            CalcHeadingAB();

            mf.trk.isMakingABLine = false;

            mf.trk.CreateDesignedABTrack(isRefRightSide);

            textBox1.Text = "AB: " +
                (Math.Round(glm.toDegrees(mf.trk.designHeading), 5)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

            panelLatLonLatLon.Visible = false;
            panelName.Visible = true;

            this.Size = new System.Drawing.Size(270, 360);
        }

        #endregion LatLon LatLon

        #region LatLon +

        private void btnFillLatLonPlus_Click(object sender, EventArgs e)
        {
            nudLatitudePlus.Value = mf.pn.latitude;
            nudLongitudePlus.Value = mf.pn.longitude;
        }

        public void CalcHeadingAPlus()
        {
            mf.pn.ConvertWGS84ToLocal((double)nudLatitudePlus.Value, (double)nudLongitudePlus.Value, out double nort, out double east);

            mf.trk.designHeading = glm.toRadians((double)nudHeadingLatLonPlus.Value);
            mf.trk.designPtA.easting = east;
            mf.trk.designPtA.northing = nort;

            mf.trk.designPtB.easting = mf.trk.designPtA.easting + (Math.Sin(mf.pivotAxlePos.heading) * 30);
            mf.trk.designPtB.northing = mf.trk.designPtA.northing + (Math.Cos(mf.pivotAxlePos.heading) * 30);
        }

        private void btnEnter_LatLonPlus_Click(object sender, EventArgs e)
        {
            CalcHeadingAPlus();

            mf.trk.isMakingABLine = false;

            mf.trk.CreateDesignedABTrack(isRefRightSide);

            textBox1.Text = "A+" +
                (Math.Round(glm.toDegrees(mf.trk.designHeading), 5)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

            panelLatLonPlus.Visible = false;
            panelName.Visible = true;

            this.Size = new System.Drawing.Size(270, 360);
        }

        #endregion LatLon +

        #region Pivot 3 Point


        private void btnPivot1_Click(object sender, EventArgs e)
        {
            mf.trk.designPtA = new vec2(mf.pivotAxlePos);
            mf.trk.isMakingABLine = true;
            btnPivot2.Enabled = true;
            btnPivot1.Enabled = false;

        }

        private void btnPivot2_Click(object sender, EventArgs e)
        {
            mf.trk.designPtB = new vec2(mf.pivotAxlePos);
            btnPivot3.Enabled = true;
            btnPivot2.Enabled = false;
        }

        private void btnPivot3_Click(object sender, EventArgs e)
        {
            mf.trk.isMakingABLine = false;

            var track = new CTrk(TrackMode.waterPivot);

            track.ptA = FindCircleCenter(mf.pivotAxlePos, mf.trk.designPtA, mf.trk.designPtB);

            mf.trk.gArr.Add(track);
            selectedItem = track;

            textBox1.Text = "Piv";

            btnPivot1.Enabled = true;
            btnPivot2.Enabled = false;
            btnPivot3.Enabled = false;

            panelPivot3Pt.Visible = false;
            panelName.Visible = true;

            this.Size = new System.Drawing.Size(270, 360);
            mf.Activate();
        }

        private vec2 FindCircleCenter(vec3 p1, vec2 p2, vec2 p3)
        {
            var d2 = p2.northing * p2.northing + p2.easting * p2.easting;
            var bc = (p1.northing * p1.northing + p1.easting * p1.easting - d2) / 2;
            var cd = (d2 - p3.northing * p3.northing - p3.easting * p3.easting) / 2;
            var det = (p1.northing - p2.northing) * (p2.easting - p3.easting) - (p2.northing - p3.northing) * (p1.easting - p2.easting);
            if (Math.Abs(det) > 1e-10) return new vec2(
              ((p1.northing - p2.northing) * cd - (p2.northing - p3.northing) * bc) / det,
              (bc * (p2.easting - p3.easting) - cd * (p1.easting - p2.easting)) / det
            );
            else return new vec2();
        }

        //Dim t As Double = p2.X * p2.X + p2.Y * p2.Y
        //Dim b As Double = (p1.X * p1.X + p1.Y * p1.Y - t) / 2
        //Dim c As Double = (t - p3.X * p3.X - p3.Y * p3.Y) / 2
        //Dim d As Double = 1 / ((p1.X - p2.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p2.Y))
        //Dim x As Double = (b * (p2.Y - p3.Y) - c * (p1.Y - p2.Y)) * d
        //Dim y As Double = ((p1.X - p2.X) * c - (p2.X - p3.X) * b) * d

        //def circle_thru_pts(x1, y1, x2, y2, x3, y3):
        //s1 = x1**2 + y1**2
        //s2 = x2**2 + y2**2
        //s3 = x3**2 + y3**2
        //M11 = x1* y2 + x2* y3 + x3* y1 - (x2* y1 + x3* y2 + x1* y3)
        //M12 = s1* y2 + s2* y3 + s3* y1 - (s2* y1 + s3* y2 + s1* y3)
        //M13 = s1* x2 + s2* x3 + s3* x1 - (s2* x1 + s3* x2 + s1* x3)
        //x0 =  0.5*M12/M11
        //y0 = -0.5 * M13 / M11
        //r0 = ((x1 - x0)**2 + (y1 - y0)**2)**0.5
        //return (x0, y0, r0)

        #endregion

        #region Lat Lon Pivot

        private void btnEnter_Pivot_Click(object sender, EventArgs e)
        {
            mf.pn.ConvertWGS84ToLocal((double)nudLatitudePivot.Value, (double)nudLongitudePivot.Value, out double nort, out double east);

            var track = new CTrk(TrackMode.waterPivot);

            track.ptA = new vec2(east, nort);

            mf.trk.gArr.Add(track);
            selectedItem = track;

            textBox1.Text = "Piv";

            panelPivot.Visible = false;
            panelName.Visible = true;

            this.Size = new System.Drawing.Size(270, 360);
            mf.Activate();
        }

        private void btnFillLAtLonPivot_Click(object sender, EventArgs e)
        {
            nudLatitudePivot.Value = mf.pn.latitude;
            nudLongitudePivot.Value = mf.pn.longitude;
        }

        #endregion Lat Lon Pivot

        private void btnCancelCurve_Click(object sender, EventArgs e)
        {
            mf.trk.isMakingCurveTrack = false;
            mf.trk.isRecordingCurveTrack = false;
            mf.trk.designPtsList?.Clear();
            mf.trk.isMakingABLine = false;

            panelMain.Visible = true;
            panelEditName.Visible = false;
            panelName.Visible = false;
            panelChoose.Visible = false;
            panelCurve.Visible = false;
            panelABLine.Visible = false;
            panelAPlus.Visible = false;
            panelLatLonLatLon.Visible = false;
            panelLatLonPlus.Visible = false;
            panelKML.Visible = false;
            panelPivot.Visible = false;

            this.Size = new System.Drawing.Size(650, 480);
            mf.Activate();
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            if (Settings.User.setDisplay_isKeyboardOn)
                mf.KeyboardToText((TextBox)sender, this);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0) textBox2.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            int idx = mf.trk.gArr.Count - 1;
            if (idx >= 0)
                mf.trk.gArr[idx].name = textBox1.Text.Trim();

            panelMain.Visible = true;
            panelName.Visible = false;

            this.Size = new System.Drawing.Size(650, 480);

            mf.trk.designPtsList?.Clear();
            UpdateTable();
            mf.Activate();
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            textBox1.Text += DateTime.Now.ToString(" hh:mm:ss", CultureInfo.InvariantCulture);
            mf.Activate();
        }

        private void btnAddTimeEdit_Click(object sender, EventArgs e)
        {
            textBox2.Text += DateTime.Now.ToString(" hh:mm:ss", CultureInfo.InvariantCulture);
            mf.Activate();
        }

        private void btnSaveEditName_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "") textBox2.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            panelEditName.Visible = false;
            panelMain.Visible = true;

            mf.trk.designPtsList?.Clear();
            if (selectedItem != null)
                selectedItem.name = textBox2.Text.Trim();

            this.Size = new System.Drawing.Size(650, 480);

            UpdateTable();
            flp.Focus();
            mf.Activate();
        }
    }
}