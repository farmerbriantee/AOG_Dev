//Please, if you use this, share the improvements

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AOG.Classes;

using AOG.Properties;
using Microsoft.Win32;
using OpenTK.Input;

namespace AOG
{
    public partial class FormGPS
    {
        #region Nozzzle
        private void cboxRate1Rate2Select_Click(object sender, EventArgs e)
        {
            if (cboxRate1Rate2Select.Checked)
            {
                nozz.volumePerAreaSetSelected = Settings.Tool.setNozz.volumePerAreaSet2;
            }
            else
            {
                nozz.volumePerAreaSetSelected = Settings.Tool.setNozz.volumePerAreaSet1;
            }

            cboxRate1Rate2Select.Text = nozz.volumePerAreaSetSelected + Settings.Tool.setNozz.unitsPerArea;
        }

        private void cboxSprayAutoManual_Click(object sender, EventArgs e)
        {
            if (cboxSprayAutoManual.Checked)
            {
                cboxSprayAutoManual.Text = "Auto";
                nozz.isSprayAutoMode = true;

                PGN_225.pgn[PGN_225.auto] = 1;
            }
            else
            {
                cboxSprayAutoManual.Text = "Manual";
                nozz.isSprayAutoMode = false;

                //manual mode 
                PGN_225.pgn[PGN_225.auto] = 0;
            }

            SendPgnToLoop(PGN_225.pgn);
        }

        private void btnSprayVolumeTotal_Click(object sender, EventArgs e)
        {
            Settings.Tool.setNozz.isAppliedUnitsNotTankDisplayed = !Settings.Tool.setNozz.isAppliedUnitsNotTankDisplayed;
            if (!Settings.Tool.setNozz.isAppliedUnitsNotTankDisplayed)
                lbl_Volume.Text = "Tank " + Settings.Tool.setNozz.unitsApplied;
            else
                lbl_Volume.Text = "App " + Settings.Tool.setNozz.unitsApplied;
        }

        private void btnSprayRate_Click(object sender, EventArgs e)
        {
            using (var form = new FormNozSettings(this))
            {
                form.ShowDialog(this);
            }

            if (cboxRate1Rate2Select.Checked)
            {
                nozz.volumePerAreaSetSelected = Settings.Tool.setNozz.volumePerAreaSet2;
            }
            else
            {
                nozz.volumePerAreaSetSelected = Settings.Tool.setNozz.volumePerAreaSet1;
            }

            cboxRate1Rate2Select.Text = nozz.volumePerAreaSetSelected + Settings.Tool.setNozz.unitsPerArea;
        }

        private void btnNozConfig_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormNozConfig"];

            if (f != null)
            {
                f.Focus();
                return;
            }

            Form form = new FormNozConfig(this);
            form.Show(this);
        }


        private void btnSprayFlyout_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormNozConfig"];

            if (f != null)
            {
                f.Focus();
                return;
            }

            Form form = new FormNozConfig(this);
            form.Show(this);
        }

        private void btnSprayRateDn_Click(object sender, EventArgs e)
        {
            if (!nozz.isSprayAutoMode)
            {
                PGN_225.pgn[PGN_225.dn] = 1;

                SendPgnToLoop(PGN_225.pgn);

                PGN_225.pgn[PGN_225.dn] = 0;
            }
            else
            {
                nozz.volumePerAreaSetSelected -= Settings.Tool.setNozz.rateNudge;
                if (nozz.volumePerAreaSetSelected < 2) nozz.volumePerAreaSetSelected = 2;
                cboxRate1Rate2Select.Text = nozz.volumePerAreaSetSelected.ToString("N1") + Settings.Tool.setNozz.unitsPerArea;
            }
        }

        private void btnSprayRateUPGN_Click(object sender, EventArgs e)
        {
            if (!nozz.isSprayAutoMode)
            {
                PGN_225.pgn[PGN_225.up] = 1;

                SendPgnToLoop(PGN_225.pgn);

                PGN_225.pgn[PGN_225.up] = 0;
            }
            else
            {
                nozz.volumePerAreaSetSelected += Settings.Tool.setNozz.rateNudge;
                cboxRate1Rate2Select.Text = nozz.volumePerAreaSetSelected.ToString("N1") + Settings.Tool.setNozz.unitsPerArea;
            }
        }

        #endregion


        #region Right Menu
        public bool isABCyled = false;
        private void btnContour_Click(object sender, EventArgs e)
        {
            SetContourButton(!ct.isContourBtnOn);
        }

        internal void SetContourButton(bool state)
        {
            if (ct.isContourBtnOn != state)
            {
                ct.isContourBtnOn = state;
                btnContour.Image = state ? Properties.Resources.ContourOn : Properties.Resources.ContourOff;

                btnContourLock.Image = Resources.ColorUnlocked;
                ct.isLocked = false;

                //SetYouTurnButton(false);
                if (!state)
                {
                    trk.isTrackValid = false;
                    SetAutoSteerButton(false, gStr.Get(gs.gsContourOn));
                }

                PanelUpdateRightAndBottom();
            }
        }

        private void btnContourLock_Click(object sender, EventArgs e)
        {
            if (ct.isContourBtnOn)
            {
                ct.SetLockToLine();
            }
        }
        public void SetContourLockImage(bool isOn)
        {
            btnContourLock.Image = isOn ? Resources.ColorLocked : Resources.ColorUnlocked;
        }
        private void btnTrack_Click(object sender, EventArgs e)
        {
            SetContourButton(false);

            if (trk.gArr.Count > 0)
            {
                //SetYouTurnButton(false);
                if (trk.currTrk == null)
                {
                    trk.GetNextTrack();
                    PanelUpdateRightAndBottom();
                    return;
                }
            }

            if (flp1.Visible)
            {
                flp1.Visible = false;
            }
            else
            {
                flp1.Visible = true;

                //build the flyout based on properties of program
                int tracksVisible = trk.GetVisibleTracks();
                bool isBnd = bnd.bndList.Count > 0;

                int btnCount = 0;
                //nudge closest
                flp1.Controls[0].Visible = tracksVisible > 0;

                //always these 3 - Build and if a bnd then ABDraw
                flp1.Controls[1].Visible = isBnd;

                flp1.Controls[2].Visible = true;
                flp1.Controls[3].Visible = true;

                //auto snap to pivot
                //flp1.Controls[4].Visible = tracksVisible > 0;

                //off button
                flp1.Controls[4].Visible = tracksVisible > 0;

                //ref nudge
                flp1.Controls[5].Visible = tracksVisible > 0;

                for (int i = 0; i < flp1.Controls.Count; i++)
                {
                    if (flp1.Controls[i].Visible) btnCount++;
                }

                //position of panel
                flp1.Top = this.Height - 120 - (btnCount*75);
                flp1.Left = this.Width - 120 - flp1.Width;
                trackMethodPanelCounter = 4;
            }

            PanelUpdateRightAndBottom();
        }

        private void btnAutoSteer_Click(object sender, EventArgs e)
        {
            SetAutoSteerButton(!isBtnAutoSteerOn, "");
        }

        internal void SetAutoSteerButton(bool state, string reason)
        {
            var triggerstate = state;
            if (state && (!ct.isContourBtnOn && trk.currTrk == null))
            {
                state = false;
                reason = gStr.Get(gs.gsTurnOnContourOrMakeABLine);
            }

            if (state && !timerSim.Enabled && avgSpeed > Settings.Vehicle.setAS_maxSteerSpeed)
            {
                state = false;
                reason = "Above Maximum Safe Steering Speed: " + (Settings.Vehicle.setAS_maxSteerSpeed * glm.kmhToMphOrKmh).ToString("N1") + glm.unitsKmhMph;
            }

            if (state && trk.currentGuidanceTrack.Count == 0)
            {
                state = false;
                reason = gStr.Get(gs.gsNoGuidanceLines);
            }

            longAvgPivDistance = 0;

            if (isBtnAutoSteerOn != state)
            {
                isBtnAutoSteerOn = state;
                btnAutoSteer.Image = state ? Properties.Resources.AutoSteerOn : Properties.Resources.AutoSteerOff;

                if (Settings.User.sound_isAutoSteerOn)
                {
                    if (state) sounds.sndAutoSteerOn.Play();
                    else sounds.sndAutoSteerOff.Play();
                }
            }

            if (!state && reason != "" && (isBtnAutoSteerOn || triggerstate))
                TimedMessageBox(2000, gStr.Get(gs.gsGuidanceStopped), reason);

            if (ct.isContourBtnOn)
                ct.SetLockToLine();
        }

        private void btnAutoYouTurn_Click(object sender, EventArgs e)
        {
            SetYouTurnButton(!yt.isYouTurnBtnOn);
        }

        internal void SetYouTurnButton(bool state)
        {
            if (yt.isYouTurnBtnOn != state)
            {
                if (state)
                {
                    if (bnd.bndList.Count == 0)
                    {
                        state = false;
                        TimedMessageBox(2000, gStr.Get(gs.gsNoBoundary), gStr.Get(gs.gsCreateABoundaryFirst));
                        Log.EventWriter("Uturn attempted without boundary");
                    }
                    //if (trk.currTrk == null)
                    //    state = false;
                }

                yt.isYouTurnBtnOn = state;
                btnAutoYouTurn.Image = state ? Properties.Resources.Youturn80 : Properties.Resources.YouTurnNo;
                yt.ResetCreatedYouTurn();
            }
        }

        private void btnCycleLines_Click(object sender, EventArgs e)
        {
            trk.GetNextTrack();

            if (trk.currTrk != null)
            {
                guideLineCounter = 20;
                lblGuidanceLine.Visible = true;
                lblGuidanceLine.Text = trk.currTrk.name;
            }
        }

        private void btnCycleLinesBk_Click(object sender, EventArgs e)
        {
            if (ct.isContourBtnOn)
            {
                ct.SetLockToLine();
            }
            else
            {
                trk.GetNextTrack(false);

                if (trk.currTrk != null)
                {
                    guideLineCounter = 20;
                    lblGuidanceLine.Visible = true;
                    lblGuidanceLine.Text = trk.currTrk.name;
                }
            }
        }

        #endregion

        #region Track Flyout

        private void btnRefNudge_Click(object sender, EventArgs e)
        {
            Form fcc = Application.OpenForms["FormNudge"];

            if (fcc != null)
            {
                fcc.Focus();
                TimedMessageBox(2000, "Nudge Window Open", "Close Nudge Window");
                return;
            }


            if (trk.currTrk != null)
            {
                Form form = new FormRefNudge(this, trk.currTrk);
                form.Show(this);
            }
            else
            {
                TimedMessageBox(1500, gStr.Get(gs.gsNoABLineActive), gStr.Get(gs.gsPleaseEnterABLine));
                return;
            }
            if (flp1.Visible)
            {
                flp1.Visible = false;
            }

            this.Activate();
        }

        private void btnTracksOff_Click(object sender, EventArgs e)
        {
            trk.currTrk = null;

            if (flp1.Visible)
            {
                flp1.Visible = false;
            }
            PanelUpdateRightAndBottom();
        }

        private void btnNudge_Click(object sender, EventArgs e)
        {
            Form fcc = Application.OpenForms["FormNudge"];

            if (fcc != null)
            {
                fcc.Focus();
                return;
            }

            if (trk.currTrk != null)
            {
                Form form = new FormNudge(this, trk.currTrk);
                form.Show(this);
            }
            else
            {
                TimedMessageBox(1500, gStr.Get(gs.gsNoABLineActive), gStr.Get(gs.gsPleaseEnterABLine));
                return;
            }

            if (flp1.Visible)
            {
                flp1.Visible = false;
            }

            this.Activate();
        }

        private void btnBuildTracks_Click(object sender, EventArgs e)
        {
            SetContourButton(false);

            //check if window already exists
            Form fc = Application.OpenForms["FormBuildTracks"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            Form form = new FormBuildTracks(this, false);
            form.Show(this);

            if (flp1.Visible)
            {
                flp1.Visible = false;
            }
        }

        private void btnPlusAB_Click(object sender, EventArgs e)
        {
            SetContourButton(false);

            //check if window already exists
            Form fc = Application.OpenForms["FormBuildTracks"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            Form form = new FormBuildTracks(this, true);
            form.Show(this);

            if (flp1.Visible)
            {
                flp1.Visible = false;
            }
            this.Activate();
        }

        private void btnABDraw_Click(object sender, EventArgs e)
        {
            if (bnd.bndList.Count == 0)
            {
                TimedMessageBox(2000, gStr.Get(gs.gsNoBoundary), gStr.Get(gs.gsCreateABoundaryFirst));
                return;
            }

            SetContourButton(false);

            if (flp1.Visible)
            {
                flp1.Visible = false;
            }

            using (var form = new FormABDraw(this))
            {
                form.ShowDialog(this);
            }

            PanelUpdateRightAndBottom();
        }

        #endregion

        #region Field Menu
        private void toolStripBtnFieldTools_Click(object sender, EventArgs e)
        {
            headlandToolStripMenuItem.Enabled = (bnd.bndList.Count > 0);
            headlandBuildToolStripMenuItem.Enabled = (bnd.bndList.Count > 0);
        }

        private void btnFieldMenu_Click(object sender, EventArgs e)
        {
            if (!isGPSPositionInitialized || sentenceCounter > 299)
            {
                if (isFieldStarted)
                {
                    FileSaveEverythingBeforeClosingField();
                    TimedMessageBox(2500, gStr.Get(gs.gsField), "Field is now closed");
                }
                else
                {
                    TimedMessageBox(2500, "No GPS", "No GPS Position Found");

                }
                Log.EventWriter("No GPS Position, Field Closed");
                return;
            }

            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
            }

            f = null;
            f = Application.OpenForms["FormFieldData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
            }

            f = null;
            f = Application.OpenForms["FormEventViewer"];

            if (f != null)
            {
                f.Focus();
                f.Close();
            }

            f = null;
            f = Application.OpenForms["FormPan"];

            if (f != null)
            {
                isPanFormVisible = false;
                f.Focus();
                f.Close();
            }

            if (this.OwnedForms.Any())
            {
                TimedMessageBox(1000, gStr.Get(gs.gsWindowsStillOpen), gStr.Get(gs.gsCloseAllWindowsFirst));
                return;
            }

            using (var form = new FormField(this))
            {
                var result = form.ShowDialog(this);

                if (result == DialogResult.Cancel)
                {
                    return;
                }

                SetWorkState(btnStates.Off);

                if (result == DialogResult.Yes)
                {
                    //new field - ask for a directory name
                    using (var form2 = new FormFieldNew(this))
                    { form2.ShowDialog(this); }
                }

                //load from  KML
                else if (result == DialogResult.No)
                {
                    //ask for a directory name
                    using (var form2 = new FormFieldKML(this))
                    { form2.ShowDialog(this); }
                }

                //load from ISOXML
                else if (result == DialogResult.Retry)
                {
                    //ask for a field to copy
                    using (var form2 = new FormFieldISOXML(this))
                    { form2.ShowDialog(this); }
                }

                //New Job
                else if (result == DialogResult.Abort)
                {
                    //ask for a field to copy
                    using (var form2 = new FormJobNew(this))
                    { form2.ShowDialog(this); }
                }

                //continue job
                else if (result == DialogResult.Ignore)
                {
                    //ask for a field to copy
                    using (var form2 = new FormJobNew(this))
                    { form2.ShowDialog(this); }
                }


                if (isFieldStarted)
                {
                    double distance = Math.Pow((CNMEA.latStart - pn.latitude), 2) + Math.Pow((CNMEA.lonStart - pn.longitude), 2);
                    distance = Math.Sqrt(distance);
                    distance *= 100;
                    if (distance > 10)
                    {
                        TimedMessageBox(2500, "High Field Start Distance Warning", "Field Start is "
                        + distance.ToString("N1") + " km From current position");

                        Log.EventWriter("High Field Start Distance Warning");
                    }

                    Log.EventWriter("** Opened **  " + currentFieldDirectory + "   " 
                        + (DateTime.Now.ToString("f", CultureInfo.CreateSpecificCulture(RegistrySettings.culture))));
                }
            }

            FieldMenuButtonEnableDisable(isJobStarted);

            toolStripBtnFieldTools.Enabled = isJobStarted;

            bnd.isHeadlandOn = (bnd.bndList.Count > 0 && bnd.bndList[0].hdLine.Count > 0);

            PanelUpdateRightAndBottom();
        }

        private void tramLinesMenuMulti_Click(object sender, EventArgs e)
        {
            SetContourButton(false);

            if (trk.gArr.Count < 1)
            {
                TimedMessageBox(1500, gStr.Get(gs.gsNoGuidanceLines), gStr.Get(gs.gsNoGuidanceLines));
                return;
            }
            if (bnd.bndList.Count < 1)
            {
                TimedMessageBox(1500, gStr.Get(gs.gsNoBoundary), gStr.Get(gs.gsCreateABoundaryFirst));
                return;
            }

            Form form99 = new FormTramLine(this);
            form99.ShowDialog(this);
        }

        private void headlandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bnd.bndList.Count == 0)
            {
                TimedMessageBox(2000, gStr.Get(gs.gsNoBoundary), gStr.Get(gs.gsCreateABoundaryFirst));
                return;
            }

            using (var form = new FormHeadLine(this))
            {
                form.ShowDialog(this);
            }

            bnd.isHeadlandOn = (bnd.bndList.Count > 0 && bnd.bndList[0].hdLine.Count > 0);

            PanelUpdateRightAndBottom();
        }
        private void headlandBuildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bnd.bndList.Count == 0)
            {
                TimedMessageBox(2000, gStr.Get(gs.gsNoBoundary), gStr.Get(gs.gsCreateABoundaryFirst));
                return;
            }

            using (var form = new FormHeadAche(this))
            {
                form.ShowDialog(this);
            }

            bnd.isHeadlandOn = (bnd.bndList.Count > 0 && bnd.bndList[0].hdLine.Count > 0);

            PanelUpdateRightAndBottom();
        }
        private void boundariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isFieldStarted)
            {
                DialogResult diaRes = DialogResult.None;

                using (var form = new FormBoundary(this))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        Form form2 = new FormBoundaryPlayer(this);
                        form2.Show(this);
                    }
                    diaRes = form.DialogResult;
                }
                if (diaRes == DialogResult.Yes)
                {
                    var form3 = new FormMap(this);
                    form3.Show(this);
                }
                else if (diaRes == DialogResult.Ignore)
                {
                    var form3 = new FormBoundaryLines(this);
                    form3.Show(this);
                }

            }

            PanelUpdateRightAndBottom();
        }

        #endregion

        #region Left Panel Menu
        private void steerWizardMenuItem_Click(object sender, EventArgs e)
        {
            Form fcs = Application.OpenForms["FormSteer"];

            if (fcs != null)
            {
                fcs.Focus();
                fcs.Close();
            }

            //check if window already exists
            Form fc = Application.OpenForms["FormSteerWiz"];

            if (fc != null)
            {
                fc.Focus();
                //fc.Close();
                return;
            }

            //
            Form form = new FormSteerWiz(this);
            form.Show(this);

        }
        private void toolStripDropDownButtonDistance_Click(object sender, EventArgs e)
        {
            fd.distanceUser = 0;
            fd.workedAreaTotalUser = 0;
        }          
        private void btnNavigationSettings_Click(object sender, EventArgs e)
        {
            //buttonPanelCounter = 0;
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
            }

            Form f1 = Application.OpenForms["FormFieldData"];

            if (f1 != null)
            {
                f1.Focus();
                f1.Close();
            }

            if (panelNavigation.Visible)
            {
                panelNavigation.Visible = false;
            }
            else
            {
                panelNavigation.Visible = true;
                panelNavigation.Left = GPSDataWindowLeft;

                navPanelCounter = 2;
                if (displayBrightness.isWmiMonitor) btnBrightnessDn.Text = (displayBrightness.GetBrightness().ToString()) + "%";
                else btnBrightnessDn.Text = "??";
            }

            if (isFieldStarted) btnGrid.Enabled = true;
            else btnGrid.Enabled = false;

        }
        private void btnStartAgIO_Click(object sender, EventArgs e)
        {
            Log.EventWriter("AgIO Manually Started");

            Process[] processName = Process.GetProcessesByName("AgIO");
            if (processName.Length == 0)
            {
                //Start application here
                string strPath = Path.Combine(Application.StartupPath, "AgIO.exe");

                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.FileName = strPath;
                    processInfo.WorkingDirectory = Path.GetDirectoryName(strPath);
                    Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find AgIO");
                    Log.EventWriter("AgIO Not Found");

                }
            }
            else
            {
                //Set foreground window
                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);
            }
        }
        private void btnAutoSteerConfig_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fc = Application.OpenForms["FormSteer"];

            if (fc != null)
            {
                fc.Focus();
                fc.Close();

                return;
            }

            //
            Form form = new FormSteer(this);
            //form.Top = 0;
            //form.Left = 0;
            form.Show(this);
            this.Activate();

        }

        private void toolSteerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fc = Application.OpenForms["FormToolSteer"];

            if (fc != null)
            {
                fc.Focus();
                fc.Close();
                return;
            }

            Form form = new FormToolSteer(this);
            form.Show(this);
            this.Activate();
        }


        private void btnConfig_Click(object sender, EventArgs e)
        {
            using (FormConfig form = new FormConfig(this))
            {
                form.ShowDialog(this);
            }
        }

        #endregion

        #region Flags
        private void toolStripMenuItemFlagRed_Click(object sender, EventArgs e)
        {
            flagColor = 0;
            btnFlag.Image = Properties.Resources.FlagRed;
        }
        private void toolStripMenuGrn_Click(object sender, EventArgs e)
        {
            flagColor = 1;
            btnFlag.Image = Properties.Resources.FlagGrn;
        }
        private void toolStripMenuYel_Click(object sender, EventArgs e)
        {
            flagColor = 2;
            btnFlag.Image = Properties.Resources.FlagYel;
        }
        private void toolStripMenuFlagForm_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FormFlags"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            if (flagPts.Count > 0)
            {
                flagNumberPicked = 1;
                Form form = new FormFlags(this);
                form.Show(this);
            }            
        }
        private void btnFlag_Click(object sender, EventArgs e)
        {
            int nextflag = flagPts.Count + 1;
            CFlag flagPt = new CFlag(pn.latitude, pn.longitude, pn.fix.easting, pn.fix.northing, 
                fixHeading, flagColor, nextflag, nextflag.ToString());
            flagPts.Add(flagPt);
            FileSaveFlags();

            Form fc = Application.OpenForms["FormFlags"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            if (flagPts.Count > 0)
            {
                flagNumberPicked = nextflag;
                Form form = new FormFlags(this);
                form.Show(this);
            }
        }

        private void btnSnapToPivot_Click(object sender, EventArgs e)
        {
            trk.SnapToPivot(trk.currTrk);
        }

        private void btnAdjRight_Click(object sender, EventArgs e)
        {
            trk.NudgeTrack(trk.currTrk, Settings.Vehicle.setAS_snapDistance);
        }

        private void btnAdjLeft_Click(object sender, EventArgs e)
        {
            trk.NudgeTrack(trk.currTrk, -Settings.Vehicle.setAS_snapDistance);
        }

        #endregion

        #region Top Panel
        private void btnFieldStats_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
            }

            f = null;
            f = Application.OpenForms["FormFieldData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                return;
            }

            if (!isFieldStarted) return;

            Form form = new FormFieldData(this);
            form.Show(this);

            form.Top = this.Top + this.Height / 2 - GPSDataWindowTopOffset;
            form.Left = this.Left + GPSDataWindowLeft;


            Form ff = Application.OpenForms["FormGPS"];
            ff.Focus();

            btnAutoSteerConfig.Focus();
        }

        private void btnGPSData_Click(object sender, EventArgs e)
        {            
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                return;
            }

            f = null;
            f = Application.OpenForms["FormFieldData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
            }

            Form form = new FormGPSData(this);
            form.Show(this);

            form.Top = this.Top + this.Height / 2 - GPSDataWindowTopOffset;
            form.Left = this.Left + GPSDataWindowLeft;

            Form ff = Application.OpenForms["FormGPS"];
            ff.Focus();
        }
        private void btnShutdown_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
            }

            f = null;
            f = Application.OpenForms["FormFieldData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
            }

            f = null;
            f = Application.OpenForms["FormEventViewer"];

            if (f != null)
            {
                f.Focus();
                f.Close();
            }

            f = null;
            f = Application.OpenForms["FormPan"];

            if (f != null)
            {
                isPanFormVisible = false;
                f.Focus();
                f.Close();
            }

            Close();
        }
        private void btnMinimizeMainForm_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnMaximizeMainForm_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else this.WindowState = FormWindowState.Maximized;

            FormGPS_ResizeEnd(this, e);
        }
        private void lblCurrentField_Click(object sender, EventArgs e)
        {
            isPauseFieldTextCounter = !isPauseFieldTextCounter;
            if (isPauseFieldTextCounter)
            {
                //lblCurrentField.Text = "\u23F8";
                fourSecondCounter = 4;
            }
            else
            {
                fourSecondCounter = 4;
            }
        }

        #endregion

        #region File Menu

        //File drop down items
        
        private void flagByLatLonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormEnterFlag(this))
            {
                form.ShowDialog(this);
                this.Activate();
            }
        }
        private void setWorkingDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YesMessageBox("Disabled For Now");
            return;

            if (isFieldStarted)
            {
                TimedMessageBox(2000, gStr.Get(gs.gsFieldIsOpen), gStr.Get(gs.gsCloseFieldFirst));
                return;
            }

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.Description = "Currently: " + RegistrySettings.workingDirectory;

            if (RegistrySettings.workingDirectory == "Default") fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else fbd.SelectedPath = RegistrySettings.workingDirectory;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                RegistrySettings.Save("WorkingDirectory", fbd.SelectedPath);
                //RegistrySettings.CreateDirectories();

                //restart program
                MessageBox.Show(gStr.Get(gs.gsProgramWillExitPleaseRestart));
                Close();
            }
        }
        private void enterSimCoordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormSimCoords(this))
            {
                form.ShowDialog(this);
            }
        }                
        private void hotKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new Form_Keys(this))
            {
                form.ShowDialog(this);
            }
        }

        private void nozzleAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isFieldStarted)
            {
                //TimedMessageBox(2000, gStr.Get(gs.gsFieldIsOpen), gStr.Get(gs.gsCloseFieldFirst));
                //Log.EventWriter("Turning Nozzle on or off while open field");
                //return;
            }

            Settings.Vehicle.setApp_isNozzleApp = !Settings.Vehicle.setApp_isNozzleApp;

            if (Settings.Vehicle.setApp_isNozzleApp)
            {
                TimedMessageBox(2000, "", "Nozzle App On");
                Log.EventWriter("Turning Nozzle App On");
            }
            else
            {
                TimedMessageBox(2000, "", "Nozzle App Off");
                Log.EventWriter("Turning Nozzle App Off");
            }

            nozzleAppToolStripMenuItem.Checked = Settings.Vehicle.setApp_isNozzleApp;

            PanelsAndOGLSize();
        }
        private void agShareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormAgShareSettings();
            {
                form.ShowDialog(this);
            }
        }

        private void kioskModeToolStriPGN_Click(object sender, EventArgs e)
        {
            Settings.User.setWindow_isKioskMode = !Settings.User.setWindow_isKioskMode;

            if (Settings.User.setWindow_isKioskMode)
            {
                kioskModeToolStrip.Checked = true;
                this.WindowState = FormWindowState.Maximized;
                Settings.User.setDisplay_isStartFullScreen = true;
                btnMaximizeMainForm.Visible = false;
                btnMinimizeMainForm.Visible = false;
            }
            else
            {
                kioskModeToolStrip.Checked = false;
                btnMaximizeMainForm.Visible = true;
                btnMinimizeMainForm.Visible = true;
            }
        }

        private void resetALLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isFieldStarted)
            {
                MessageBox.Show(gStr.Get(gs.gsCloseFieldFirst));
            }
            else
            {
                DialogResult result2 = MessageBox.Show(gStr.Get(gs.gsReallyResetEverything), gStr.Get(gs.gsResetAll),
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result2 == DialogResult.Yes)
                {
                    //save event
                    Log.EventWriter("*****");
                    Log.EventWriter("Registry set to default - Reset ALL event occured");
                    Log.EventWriter("*****");
                    FileSaveSystemEvents();

                    RegistrySettings.Reset();

                    MessageBox.Show(gStr.Get(gs.gsProgramWillExitPleaseRestart));
                    Close();
                }
            }
        }

        private void simulatorOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerSim.Enabled = panelSim.Visible = simulatorOnToolStripMenuItem.Checked;
            isGPSPositionInitialized = false;
            isFirstHeadingSet = false;
            startCounter = 0;

            SetControlButtonPositions();
            SetControlLabelPositions();

            Settings.User.isSimulatorOn = simulatorOnToolStripMenuItem.Checked;
        }
        private void colorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormColor(this))
            {
                form.ShowDialog(this);
            }
        }
        private void colorsSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Settings.Tool.isSectionsNotZones)
            {
                using (var form = new FormColorSection(this))
                {
                    form.ShowDialog(this);
                }
            }
            else
            {
                TimedMessageBox(2000, "Cannot use with zones", "Only for Sections");
            }
        }

        //Languages
        private void menuLanguageEnglish_Click(object sender, EventArgs e)
        {
            SetLanguage("en");
        }
        private void menuLanguageDanish_Click(object sender, EventArgs e)
        {
            SetLanguage("da");
        }
        private void menuLanguageDeutsch_Click(object sender, EventArgs e)
        {
            SetLanguage("de");
        }
        private void menuLanguageRussian_Click(object sender, EventArgs e)
        {
            SetLanguage("ru");
        }
        private void menuLanguageDutch_Click(object sender, EventArgs e)
        {
            SetLanguage("nl");
        }
        private void menuLanguageSpanish_Click(object sender, EventArgs e)
        {
            SetLanguage("es");
        }
        private void menuLanguageFrench_Click(object sender, EventArgs e)
        {
            SetLanguage("fr");
        }
        private void menuLanguageItalian_Click(object sender, EventArgs e)
        {
            SetLanguage("it");
        }
        private void menuLanguageHungarian_Click(object sender, EventArgs e)
        {
            SetLanguage("hu");
        }
        private void menuLanguageUkranian_Click(object sender, EventArgs e)
        {
            SetLanguage("uk");
        }
        private void menuLanguageSlovak_Click(object sender, EventArgs e)
        {
            SetLanguage("sk");
        }
        private void menuLanguagesPolski_Click(object sender, EventArgs e)
        {
            SetLanguage("pl");
        }
        private void menuLanguagesPortugese_Click(object sender, EventArgs e)
        {
            SetLanguage("pt");
        }
        private void menuLanguageTest_Click(object sender, EventArgs e)
        {
            SetLanguage("af");
        }
        private void menuLanguageTurkish_Click(object sender, EventArgs e)
        {
            SetLanguage("tr");
        }
        private void menuLanguageFinnish_Click(object sender, EventArgs e)
        {
            SetLanguage("fi");
        }
        private void menuLanguageLatvian_Click(object sender, EventArgs e)
        {
            SetLanguage("lv");
        }
        private void menuLanguageLithuanian_Click(object sender, EventArgs e)
        {
            SetLanguage("lt");
        }
        private void menuLanguageChinese_Click(object sender, EventArgs e)
        {
            SetLanguage("zh-CHS");
        }
        private void menuLanguageSerbian_Click(object sender, EventArgs e)
        {
            SetLanguage("sr");

        }

        private void menuGenerateLanguageReference_Click(object sender, EventArgs e)
        {
            gStr.GenerateReferenceKeys();
        }

        private void SetLanguage(string lang)
        {
            //reset them all to false
            menuLanguageEnglish.Checked = false;
            menuLanguageDeutsch.Checked = false;
            menuLanguageRussian.Checked = false;
            menuLanguageDutch.Checked = false;
            menuLanguageSpanish.Checked = false;
            menuLanguageFrench.Checked = false;
            menuLanguageItalian.Checked = false;
            menuLanguageUkranian.Checked = false;
            menuLanguageSlovak.Checked = false;
            menuLanguagePolish.Checked = false;
            menuLanguageDanish.Checked = false;
            menuLanguageTurkish.Checked = false;
            menuLanguageHungarian.Checked = false;
            menuLanguageLithuanian.Checked = false;
            menuLanguageFinnish.Checked = false;
            menuLanguageLatvian.Checked = false;
            menuLanguageChinese.Checked = false;
            menuLanguagePortugese.Checked = false;
            menuLanguageSerbian.Checked = false;

            menuLanguageTest.Checked = false;

            switch (lang)
            {
                case "en":
                    menuLanguageEnglish.Checked = true;
                    break;

                case "ru":
                    menuLanguageRussian.Checked = true;
                    break;

                case "da":
                    menuLanguageDanish.Checked = true;
                    break;

                case "de":
                    menuLanguageDeutsch.Checked = true;
                    break;

                case "nl":
                    menuLanguageDutch.Checked = true;
                    break;

                case "it":
                    menuLanguageItalian.Checked = true;
                    break;

                case "es":
                    menuLanguageSpanish.Checked = true;
                    break;

                case "fr":
                    menuLanguageFrench.Checked = true;
                    break;

                case "uk":
                    menuLanguageUkranian.Checked = true;
                    break;

                case "sk":
                    menuLanguageSlovak.Checked = true;
                    break;

                case "pl":
                    menuLanguagePolish.Checked = true;
                    break;

                case "pt":
                    menuLanguagePortugese.Checked = true;
                    break;

                case "af":
                    menuLanguageTest.Checked = true;
                    break;

                case "tr":
                    menuLanguageTurkish.Checked = true;
                    break;

                case "hu":
                    menuLanguageHungarian.Checked = true;
                    break;

                case "lt":
                    menuLanguageLithuanian.Checked = true;
                    break;

                case "lv":
                    menuLanguageLatvian.Checked = true;
                    break;

                case "fi":
                    menuLanguageFinnish.Checked = true;
                    break;

                case "sr":
                    menuLanguageSerbian.Checked = true;
                    break;

                case "zh-CHS":
                    menuLanguageChinese.Checked = true;
                    break;

                default:
                    menuLanguageEnglish.Checked = true;
                    lang = "en";
                    break;
            }

            if (RegistrySettings.culture != lang)
            {
                RegistrySettings.Save("Language", lang);

                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(RegistrySettings.culture);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(RegistrySettings.culture);

                //load language file Translations.xlsx
                if (!gStr.Load()) YesMessageBox("Serious error loading languages");

                SetText();
            }
        }

        #endregion

        #region Bottom Menu
        public void CloseTopMosts()
        {
            Form fc = Application.OpenForms["FormSteer"];

            if (fc != null)
            {
                fc.Focus();
                fc.Close();
            }

            fc = Application.OpenForms["FormSteerGraph"];

            if (fc != null)
            {
                fc.Focus();
                fc.Close();
            }

            fc = Application.OpenForms["FormGPSData"];

            if (fc != null)
            {
                fc.Focus();
                fc.Close();
            }

        }

        private void btnResetToolHeading_Click(object sender, EventArgs e)
        {
            tankPos.heading = fixHeading;
            tankPos.easting = hitchPos.easting + (Math.Sin(tankPos.heading) * (Settings.Tool.tankTrailingHitchLength));
            tankPos.northing = hitchPos.northing + (Math.Cos(tankPos.heading) * (Settings.Tool.tankTrailingHitchLength));
            
            toolPivotPos.heading = tankPos.heading;
            toolPivotPos.easting = tankPos.easting + (Math.Sin(toolPivotPos.heading) * (Settings.Tool.toolTrailingHitchLength));
            toolPivotPos.northing = tankPos.northing + (Math.Cos(toolPivotPos.heading) * (Settings.Tool.toolTrailingHitchLength));
        }
        private void btnTramDisplayMode_Click(object sender, EventArgs e)
        {
            tram.isLeftManualOn = false;
            tram.isRightManualOn = false;

            //if only lines cycle on off
            if (tram.tramList.Count > 0 && tram.tramBndOuterArr.Count == 0)
            {
                if (tram.displayMode != 0) tram.displayMode = 0;
                else tram.displayMode = 2;
            }
            else
            {
                tram.displayMode++;
                if (tram.displayMode > 3) tram.displayMode = 0;
            }

            switch (tram.displayMode)
            {
                case 0:
                    btnTramDisplayMode.Image = Properties.Resources.TramOff;
                    break;
                case 1:
                    btnTramDisplayMode.Image = Properties.Resources.TramAll;
                    break;
                case 2:
                    btnTramDisplayMode.Image = Properties.Resources.TramLines;
                    break;
                case 3:
                    btnTramDisplayMode.Image = Properties.Resources.TramOuter;
                    break;

                default:
                    break;
            }
        }
        public bool isPatchesChangingColor = false;
        private void btnChangeMappingColor_Click(object sender, EventArgs e)
        {
            using (var form = new FormColorPicker(this, Settings.User.colorSectionsDay))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorSectionsDay = form.useThisColor;
                }
            }

            isPatchesChangingColor = true;
        }
        private void btnYouSkipEnable_Click(object sender, EventArgs e)
        {
            yt.alternateSkips = !yt.alternateSkips;
            if (yt.alternateSkips)
            {
                btnYouSkipEnable.Image = Resources.YouSkipOn;
                //make sure at least 1
                if (yt.rowSkipsWidth < 2)
                {
                    yt.rowSkipsWidth = 2;
                    cboxpRowWidth.Text = "1";
                }
                yt.Set_Alternate_skips();
                SetYouTurnButton(true);
            }
            else
            {
                btnYouSkipEnable.Image = Resources.YouSkipOff;
            }
        }
        private void cboxpRowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            yt.rowSkipsWidth = cboxpRowWidth.SelectedIndex + 1;
            yt.Set_Alternate_skips();
            if (!yt.isYouTurnTriggered) yt.ResetCreatedYouTurn();
            Settings.Vehicle.set_youSkipWidth = yt.rowSkipsWidth;
            
        }
        private void btnHeadlandOnOff_Click(object sender, EventArgs e)
        {
            bnd.isHeadlandOn = !bnd.isHeadlandOn;
            if (bnd.isHeadlandOn)
            {
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
            }
            else
            {
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
            }

            if (vehicle.isHydLiftOn && !bnd.isHeadlandOn) vehicle.isHydLiftOn = false;

            if (!bnd.isHeadlandOn)
            {
                PGN_239.pgn[PGN_239.hydLift] = 0;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
            }

            PanelUpdateRightAndBottom();
        }
        private void cboxIsSectionControlled_Click(object sender, EventArgs e)
        {
            if (cboxIsSectionControlled.Checked) cboxIsSectionControlled.Image = Properties.Resources.HeadlandSectionOn;
            else cboxIsSectionControlled.Image = Properties.Resources.HeadlandSectionOff;
            bnd.isSectionControlledByHeadland = cboxIsSectionControlled.Checked;            
        }
        private void btnHydLift_Click(object sender, EventArgs e)
        {
            if (bnd.isHeadlandOn)
            {
                vehicle.isHydLiftOn = !vehicle.isHydLiftOn;
                if (vehicle.isHydLiftOn)
                {
                    btnHydLift.Image = Properties.Resources.HydraulicLiftOn;
                }
                else
                {
                    btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
                    PGN_239.pgn[PGN_239.hydLift] = 0;
                }
            }
            else
            {
                PGN_239.pgn[PGN_239.hydLift] = 0;
                vehicle.isHydLiftOn = false;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
            }
        }

        #endregion

        #region Tools Menu

        private void allSettingsMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new FormAllSettings(this);
            form.Show(this);
        }
        private void guidelinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.User.isSideGuideLines = !Settings.User.isSideGuideLines;
            if (Settings.User.isSideGuideLines) guidelinesToolStripMenuItem.Checked = true;
            else guidelinesToolStripMenuItem.Checked = false;            
        }

        private void boundaryToolToolStripMenu_Click(object sender, EventArgs e)
        {
            if (isFieldStarted)
            {
                using (var form = new FormBndTool(this))
                {
                    form.ShowDialog(this);
                }
            }
        }

        private void layersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormLayer form = new FormLayer(this))
            {
                //returns full field.txt file dir name
                form.ShowDialog(this);
            }
        }

        private void SmoothABtoolStripMenu_Click(object sender, EventArgs e)
        {
            if (isFieldStarted && trk.currTrk != null && trk.currTrk.mode > TrackMode.AB)
            {
                using (var form = new FormSmoothAB(this, trk.currTrk))
                {
                    form.ShowDialog(this);
                }
            }
            else
            {
                if (!isFieldStarted) TimedMessageBox(2000, gStr.Get(gs.gsFieldNotOpen), gStr.Get(gs.gsStartNewField));
                else TimedMessageBox(2000, gStr.Get(gs.gsCurveNotOn), gStr.Get(gs.gsTurnABCurveOn));
            }
        }
         private void deleteContourPathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FileCreateContour();
            ct.stripList?.Clear();
            ct.ptList?.Clear();
            ct.ctList?.Clear();
            contourSaveList?.Clear();
        }
        private void toolStripAreYouSure_Click(object sender, EventArgs e)
        {
            if (isFieldStarted && isJobStarted)
            {
                if (workState == btnStates.Off)
                {

                    DialogResult result3 = MessageBox.Show(gStr.Get(gs.gsDeleteAllContoursAndSections),
                        gStr.Get(gs.gsDeleteForSure),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
                    if (result3 == DialogResult.Yes)
                    {
                        //FileCreateElevation();
                        TurnOffSectionsSafely();

                        //clear out the contour Lists
                        ct.StopContourLine();
                        ct.ResetContour();
                        fd.workedAreaTotal = 0;

                        //clear the section lists
                        triStrip?.Clear();
                        patchList?.Clear();
                        patchSaveList?.Clear();

                        FileCreateContour();
                        FileCreateSections();

                        Log.EventWriter("All Section Mapping Deleted");
                    }
                    else
                    {
                        TimedMessageBox(1500, gStr.Get(gs.gsNothingDeleted), gStr.Get(gs.gsActionHasBeenCancelled));
                    }
                }
                else
                {
                   TimedMessageBox(1500, "Sections are on", "Turn Auto or Manual Off First");
                }
            }
        }
        private void headingChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fh = Application.OpenForms["FormHeadingGraph"];

            if (fh != null)
            {
                fh.Focus();
                return;
            }

            //
            Form formH = new FormGraphHeading(this);
            formH.Show(this);
        }
        private void toolStripAutoSteerChart_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fcg = Application.OpenForms["FormSteerGraph"];

            if (fcg != null)
            {
                fcg.Focus();
                return;
            }

            //
            Form formG = new FormGraphSteer(this);
            formG.Show(this);
        }               
        private void xTEChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fx = Application.OpenForms["FormXTEGraph"];

            if (fx != null)
            {
                fx.Focus();
                return;
            }

            //
            Form formX = new FormGraphXTE(this);
            formX.Show(this);
        }
        private void eventViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new FormEventViewer(Path.Combine(RegistrySettings.logsDirectory, "AOG_Events_Log.txt"));
            form.Show(this);
            this.Activate();
        }
        private void webcamToolStriPGN_Click(object sender, EventArgs e)
        {
            Form form = new FormWebCam();
            form.Show(this);
            this.Activate();
        }
        private void offsetFixToolStriPGN_Click(object sender, EventArgs e)
        {
            using (var form = new FormShiftPos(this))
            {
                form.ShowDialog(this);
            }
        }
        private void correctionToolStriPGN_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fcc = Application.OpenForms["FormCorrection"];

            if (fcc != null)
            {
                fcc.Focus();
                return;
            }

            //
            Form formC = new FormCorrection(this);
            formC.Show(this);
        }

        #endregion

        #region Nav Panel

        private void btnTiltUPGN_Click(object sender, EventArgs e)
        {
            Settings.User.setDisplay_camPitch -= ((Settings.User.setDisplay_camPitch * 0.012) - 1);
            if (Settings.User.setDisplay_camPitch > -58) Settings.User.setDisplay_camPitch = 0;
            navPanelCounter = 2;
        }
        private void btnTiltDn_Click(object sender, EventArgs e)
        {
            if (Settings.User.setDisplay_camPitch > -59) Settings.User.setDisplay_camPitch = -60;
            Settings.User.setDisplay_camPitch += ((Settings.User.setDisplay_camPitch * 0.012) - 1);
            if (Settings.User.setDisplay_camPitch < -70) Settings.User.setDisplay_camPitch = -70;
            navPanelCounter = 2;
        }
        private void btnN2D_Click(object sender, EventArgs e)
        {
            camera.camFollowing = false;
            Settings.User.setDisplay_camPitch = 0;
            navPanelCounter = 0;
        }
        private void btn2D_Click(object sender, EventArgs e)
        {
            camera.camFollowing = true;
            Settings.User.setDisplay_camPitch = 0;
            navPanelCounter = 0;
        }
        private void btn3D_Click(object sender, EventArgs e)
        {
            camera.camFollowing = true;
            Settings.User.setDisplay_camPitch = -65;
            navPanelCounter = 0;
        }
        //private void btnN2D_Click(object sender, EventArgs e)
        //{
        //    camera.camFollowing = false;
        //    Settings.User.setDisplay_camPitch = 0;
        //    navPanelCounter = 0;
        //}
        //private void btnN3D_Click(object sender, EventArgs e)
        //{
        //    Settings.User.setDisplay_camPitch = -65;
        //    camera.camFollowing = false;
        //    navPanelCounter = 0;
        //}

        private void btnGrid_Click(object sender, EventArgs e)
        {
            var form = new FormGrid(this);
                form.Show(this);
            navPanelCounter = 0;
        }
        private void btnBrightnessUPGN_Click(object sender, EventArgs e)
        {
            if (displayBrightness.isWmiMonitor)
            {
                displayBrightness.BrightnessIncrease();
                btnBrightnessDn.Text = displayBrightness.GetBrightness().ToString() + "%";
                Settings.User.setDisplay_brightness = displayBrightness.GetBrightness();
                
            }
            navPanelCounter = 3;
        }
        private void btnBrightnessDn_Click(object sender, EventArgs e)
        {
            if (displayBrightness.isWmiMonitor)
            {
                displayBrightness.BrightnessDecrease();
                btnBrightnessDn.Text = displayBrightness.GetBrightness().ToString() + "%";
                Settings.User.setDisplay_brightness = displayBrightness.GetBrightness();
                
            }
            navPanelCounter = 3;
        }
        private void lblHz_Click(object sender, EventArgs e)
        {
            string strPath = Path.Combine(Application.StartupPath, "OGL.exe");

            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.FileName = strPath;
                processInfo.WorkingDirectory = Path.GetDirectoryName(strPath);
                Process.Start(processInfo);
            }
            catch
            {
                TimedMessageBox(2000, "No File Found", "Can't Find OGL");
            }
        }
        private void btnDayNightMode_Click(object sender, EventArgs e)
        {
            SwapDayNightMode();
            navPanelCounter = 0;
        }

        #endregion

        #region OpenGL Window context Menu and functions

        private void lblHardwareMessage_Click(object sender, EventArgs e)
        {
            hardwareLineCounter = 1;
        }

        #endregion

        #region Sim controls

        private void btnSimSpeedUPGN_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (sim.stepDistance < 0)
            {
                sim.stepDistance = 0;
                return;
            }
            if (sim.stepDistance < 0.2 ) sim.stepDistance += 0.02;
            else 
                sim.stepDistance *= 1.15;

            if (sim.stepDistance > 7.5) sim.stepDistance = 7.5;
        }
        private void btnSpeedDn_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (sim.stepDistance < 0.2 && sim.stepDistance > -0.51) sim.stepDistance -= 0.02;
            else sim.stepDistance *= 0.8;
            if (sim.stepDistance < -0.5) sim.stepDistance = -0.5;
        }

        double lastSimGuidanceAngle = 0;
        private void timerSim_Tick(object sender, EventArgs e)
        {
            try
            {
                if (isBtnAutoSteerOn && !double.IsNaN(guidanceLineDistanceOff))
                {
                    if (vehicle.isInDeadZone)
                    {
                        sim.DoSimTick(lastSimGuidanceAngle);
                    }
                    else
                    {
                        lastSimGuidanceAngle = guidanceLineSteerAngle * 0.9;
                        sim.DoSimTick(lastSimGuidanceAngle);
                    }
                }
                else sim.DoSimTick(steerAngleScrollBar);
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Catch timerSim_Tick error: {ex.Message}");
            }
        }
        private void btnSimReverseDirection_Click(object sender, EventArgs e)
        {
            sim.Reverse();
        }
        private void hsbarSteerAngle_Scroll(object sender, ScrollEventArgs e)
        {
            steerAngleScrollBar = (hsbarSteerAngle.Value - 400) * 0.1;
        }
        private void btnResetSteerAngle_Click(object sender, EventArgs e)
        {
            steerAngleScrollBar = 0.0;
        }
        private void btnResetSim_Click(object sender, EventArgs e)
        {
            sim.Reset();
        }
        private void btnSimSetSpeedToZero_Click(object sender, EventArgs e)
        {
            sim.stepDistance = 0;
        }

        #endregion

        public void FixTramModeButton()
        {
            if (tram.tramList.Count > 0 && tram.tramBndOuterArr.Count > 0)
            {
                tram.displayMode = 1;
            }
            else if (tram.tramList.Count == 0 && tram.tramBndOuterArr.Count > 0)
            {
                tram.displayMode = 3;
            }
            else if (tram.tramList.Count > 0 && tram.tramBndOuterArr.Count == 0)
            {
                tram.displayMode = 2;
            }

            switch (tram.displayMode)
            {
                case 1:
                    btnTramDisplayMode.Image = Properties.Resources.TramAll;
                    break;
                case 2:
                    btnTramDisplayMode.Image = Properties.Resources.TramLines;
                    break;
                case 3:
                    btnTramDisplayMode.Image = Properties.Resources.TramOuter;
                    break;

                default:
                    break;
            }
        }

        private ToolStripMenuItem steerChartToolStripMenuItem;
        private ToolStripMenuItem headingChartToolStripMenuItem;
        private ToolStripMenuItem xTEChartToolStripMenuItem;
    }//end class

}//end namespace