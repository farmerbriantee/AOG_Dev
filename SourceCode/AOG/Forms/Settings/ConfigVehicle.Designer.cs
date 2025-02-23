using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgOpenGPS.Classes;

using AgOpenGPS.Properties;
using Microsoft.Win32;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS
{
    public partial class FormConfig
    {
        public  void TurnOffSectionsSafely()
        {
            if (mf.autoBtnState == btnStates.Auto)
                mf.btnSectionMasterAuto.PerformClick();

            if (mf.manualBtnState == btnStates.On)
                mf.btnSectionMasterManual.PerformClick();

            //turn off all the sections
            for (int j = 0; j < mf.tool.numOfSections; j++)
            {
                mf.section[j].sectionOffRequest = true;
                mf.section[j].sectionOnRequest = false;
            }

            //turn off patching
            foreach (var patch in mf.triStrip)
            {
                if (patch.isDrawing) patch.TurnMappingOff();
            }
        }
        #region Vehicle Save---------------------------------------------

        private void btnVehicleLoad_Click(object sender, EventArgs e)
        {
            if (lvVehicles.SelectedItems.Count > 0)
            {
                DialogResult result3 = MessageBox.Show(
                    "Open: " + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML ?",
                    gStr.Get(gs.gsSaveAndReturn),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (result3 == DialogResult.Yes)
                {
                    TurnOffSectionsSafely();

                    Properties.Settings.Default.Save();//necessary???
                    RegistrySettings.Save("VehicleFileName", lvVehicles.SelectedItems[0].SubItems[0].Text);
                    if (Properties.Settings.Default.Load() != Settings.LoadResult.Ok) return;

                    LoadBrandImage();

                    mf.vehicle = new CVehicle(mf);
                    mf.tool = new CTool(mf);

                    //reset AgOpenGPS
                    mf.LoadSettings();

                    //Form Steer Settings
                    PGN_252.pgn[PGN_252.countsPerDegree] = unchecked((byte)Properties.Settings.Default.setAS_countsPerDegree);
                    PGN_252.pgn[PGN_252.ackerman] = unchecked((byte)Properties.Settings.Default.setAS_ackerman);

                    PGN_252.pgn[PGN_252.wasOffsetHi] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset >> 8));
                    PGN_252.pgn[PGN_252.wasOffsetLo] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset));

                    PGN_252.pgn[PGN_252.highPWM] = unchecked((byte)Properties.Settings.Default.setAS_highSteerPWM);
                    PGN_252.pgn[PGN_252.lowPWM] = unchecked((byte)Properties.Settings.Default.setAS_lowSteerPWM);
                    PGN_252.pgn[PGN_252.gainProportional] = unchecked((byte)Properties.Settings.Default.setAS_Kp);
                    PGN_252.pgn[PGN_252.minPWM] = unchecked((byte)Properties.Settings.Default.setAS_minSteerPWM);

                    mf.SendPgnToLoop(PGN_252.pgn);

                    //steer config
                    PGN_251.pgn[PGN_251.set0] = Properties.Settings.Default.setArdSteer_setting0;
                    PGN_251.pgn[PGN_251.set1] = Properties.Settings.Default.setArdSteer_setting1;
                    PGN_251.pgn[PGN_251.maxPulse] = Properties.Settings.Default.setArdSteer_maxPulseCounts;
                    PGN_251.pgn[PGN_251.minSpeed] = unchecked((byte)(Properties.Settings.Default.setAS_minSteerSpeed * 10));

                    if (Properties.Settings.Default.setAS_isConstantContourOn)
                        PGN_251.pgn[PGN_251.angVel] = 1;
                    else PGN_251.pgn[PGN_251.angVel] = 0;

                    mf.SendPgnToLoop(PGN_251.pgn);

                    //machine settings    
                    PGN_238.pgn[PGN_238.set0] = Properties.Settings.Default.setArdMac_setting0;
                    PGN_238.pgn[PGN_238.raiseTime] = Properties.Settings.Default.setArdMac_hydRaiseTime;
                    PGN_238.pgn[PGN_238.lowerTime] = Properties.Settings.Default.setArdMac_hydLowerTime;

                    PGN_238.pgn[PGN_238.user1] = Properties.Settings.Default.setArdMac_user1;
                    PGN_238.pgn[PGN_238.user2] = Properties.Settings.Default.setArdMac_user2;
                    PGN_238.pgn[PGN_238.user3] = Properties.Settings.Default.setArdMac_user3;
                    PGN_238.pgn[PGN_238.user4] = Properties.Settings.Default.setArdMac_user4;

                    mf.SendPgnToLoop(PGN_238.pgn);

                    //Send Pin configuration
                    SendRelaySettingsToMachineModule();

                    ///Remind the user
                    mf.TimedMessageBox(2500, "Steer and Machine Settings Sent", "Were Modules Connected?");

                    Log.EventWriter("Vehicle Loaded: " + RegistrySettings.vehicleFileName + ".XML");

                    Close();
                }
            }
        }

        private void btnVehicleDelete_Click(object sender, EventArgs e)
        {
            if (lvVehicles.SelectedItems.Count > 0)
            {
                if (lvVehicles.SelectedItems[0].SubItems[0].Text != RegistrySettings.vehicleFileName)
                {
                    DialogResult result3 = MessageBox.Show(
                    "Delete: " + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML",
                    gStr.Get(gs.gsSaveAndReturn),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button2);
                    if (result3 == DialogResult.Yes)
                    {
                        File.Delete(Path.Combine(RegistrySettings.vehiclesDirectory, lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML"));
                    }
                }
                else
                {
                    mf.TimedMessageBox(2000, "Vehicle In Use", "Select Different Vehicle");
                }
            }
            UpdateVehicleListView();
        }

        //Save As Vehicle
        private void btnVehicleSave_Click(object sender, EventArgs e)
        {
            TurnOffSectionsSafely();

            btnVehicleSave.BackColor = Color.Transparent;
            btnVehicleSave.Enabled = false;            

            string vehicleName = SanitizeFileName(tboxVehicleNameSave.Text.Trim()).Trim();
            tboxVehicleNameSave.Text = "";

            if (vehicleName.Length > 0)
            {
                Settings.Default.Save();//necessary???
                RegistrySettings.Save("VehicleFileName", vehicleName);
                Settings.Default.Save();

                UpdateVehicleListView();
            }
        }

        private void tboxVehicleNameSave_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            btnVehicleLoad.Enabled = false;
            btnVehicleDelete.Enabled = false;

            lvVehicles.SelectedItems.Clear();

            if (String.IsNullOrEmpty(tboxVehicleNameSave.Text.Trim()))
            {
                btnVehicleSave.Enabled = false;
                btnVehicleSave.BackColor = Color.Transparent;
            }
            else
            {
                btnVehicleSave.Enabled = true;
                btnVehicleSave.BackColor = Color.LimeGreen;
            }
        }
        private void tboxVehicleNameSave_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
            }
        }

        private void tboxVehicleNameSave_Enter(object sender, EventArgs e)
        {
            //btnVehicleSaveAs.Enabled = false;
            btnVehicleLoad.Enabled = false;
            btnVehicleDelete.Enabled = false;

            lvVehicles.SelectedItems.Clear();
        }

        //New Vehicle
        private void tboxCreateNewVehicle_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            btnVehicleSave.Enabled = false;
            btnVehicleLoad.Enabled = false;
            btnVehicleDelete.Enabled = false;

            lvVehicles.SelectedItems.Clear();

            if (String.IsNullOrEmpty(tboxCreateNewVehicle.Text.Trim()))
            {
                btnVehicleNewSave.Enabled = false;
                btnVehicleNewSave.BackColor = Color.Transparent;
            }
            else
            {
                btnVehicleNewSave.Enabled = true;
                btnVehicleNewSave.BackColor = Color.LimeGreen;
            }
        }
        private void tboxCreateNewVehicle_Click(object sender, EventArgs e)
        {
            TurnOffSectionsSafely();

            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
            }
        }

        private void btnVehicleNewSave_Click(object sender, EventArgs e)
        {
            TurnOffSectionsSafely();

            btnVehicleNewSave.BackColor = Color.Transparent;
            btnVehicleNewSave.Enabled = false;

            string vehicleName = SanitizeFileName(tboxCreateNewVehicle.Text.Trim()).Trim();
            tboxCreateNewVehicle.Text = "";

            if (vehicleName.Length > 0)
            {
                Properties.Settings.Default.Save();//necessary???
                RegistrySettings.Save("VehicleFileName", vehicleName);
                Settings.Default.Reset();

                LoadBrandImage();

                mf.vehicle = new CVehicle(mf);
                mf.tool = new CTool(mf);

                //reset AgOpenGPS
                mf.LoadSettings();

                SectionFeetInchesTotalWidthLabelUpdate();

                //Form Steer Settings
                PGN_252.pgn[PGN_252.countsPerDegree] = unchecked((byte)Properties.Settings.Default.setAS_countsPerDegree);
                PGN_252.pgn[PGN_252.ackerman] = unchecked((byte)Properties.Settings.Default.setAS_ackerman);

                PGN_252.pgn[PGN_252.wasOffsetHi] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset >> 8));
                PGN_252.pgn[PGN_252.wasOffsetLo] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset));

                PGN_252.pgn[PGN_252.highPWM] = unchecked((byte)Properties.Settings.Default.setAS_highSteerPWM);
                PGN_252.pgn[PGN_252.lowPWM] = unchecked((byte)Properties.Settings.Default.setAS_lowSteerPWM);
                PGN_252.pgn[PGN_252.gainProportional] = unchecked((byte)Properties.Settings.Default.setAS_Kp);
                PGN_252.pgn[PGN_252.minPWM] = unchecked((byte)Properties.Settings.Default.setAS_minSteerPWM);

                mf.SendPgnToLoop(PGN_252.pgn);

                //machine module settings
                PGN_238.pgn[PGN_238.set0] = Properties.Settings.Default.setArdMac_setting0;
                PGN_238.pgn[PGN_238.raiseTime] = Properties.Settings.Default.setArdMac_hydRaiseTime;
                PGN_238.pgn[PGN_238.lowerTime] = Properties.Settings.Default.setArdMac_hydLowerTime;

                mf.SendPgnToLoop(PGN_238.pgn);

                //steer config
                PGN_251.pgn[PGN_251.set0] = Properties.Settings.Default.setArdSteer_setting0;
                PGN_251.pgn[PGN_251.set1] = Properties.Settings.Default.setArdSteer_setting1;
                PGN_251.pgn[PGN_251.maxPulse] = Properties.Settings.Default.setArdSteer_maxPulseCounts;
                PGN_251.pgn[PGN_251.minSpeed] = unchecked((byte)(Properties.Settings.Default.setAS_minSteerSpeed * 10));

                if (Properties.Settings.Default.setAS_isConstantContourOn)
                    PGN_251.pgn[PGN_251.angVel] = 1;
                else PGN_251.pgn[PGN_251.angVel] = 0;

                mf.SendPgnToLoop(PGN_251.pgn);

                //machine settings    
                PGN_238.pgn[PGN_238.set0] = Properties.Settings.Default.setArdMac_setting0;
                PGN_238.pgn[PGN_238.raiseTime] = Properties.Settings.Default.setArdMac_hydRaiseTime;
                PGN_238.pgn[PGN_238.lowerTime] = Properties.Settings.Default.setArdMac_hydLowerTime;

                PGN_238.pgn[PGN_238.user1] = Properties.Settings.Default.setArdMac_user1;
                PGN_238.pgn[PGN_238.user2] = Properties.Settings.Default.setArdMac_user2;
                PGN_238.pgn[PGN_238.user3] = Properties.Settings.Default.setArdMac_user3;
                PGN_238.pgn[PGN_238.user4] = Properties.Settings.Default.setArdMac_user4;

                mf.SendPgnToLoop(PGN_238.pgn);

                //Send Pin configuration
                SendRelaySettingsToMachineModule();

                ///Remind the user
                mf.TimedMessageBox(2500, "Steer and Machine Settings Sent", "Were Modules Connected?");

                Log.EventWriter("New Vehicle Loaded: " + RegistrySettings.vehicleFileName + ".XML");

                UpdateVehicleListView();
            }
        }        

        //Functions
        private static readonly Regex InvalidFileRegex = new Regex(string.Format("[{0}]", Regex.Escape(@"<>:""/\|?*")));
        public static string SanitizeFileName(string fileName)
        {
            return InvalidFileRegex.Replace(fileName, string.Empty);
        }

        private void UpdateVehicleListView()
        {
            DirectoryInfo dinfo = new DirectoryInfo(RegistrySettings.vehiclesDirectory);
            FileInfo[] Files = dinfo.GetFiles("*.XML");

            //load the listbox
            lvVehicles.Items.Clear();
            foreach (FileInfo file in Files)
            {
                lvVehicles.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
            }

            //deselect everything
            lvVehicles.SelectedItems.Clear();
            lblSummaryVehicleName.Text = RegistrySettings.workingDirectory;

            //tboxCreateNewVehicle.Text = "";
            //tboxVehicleNameSave.Text = "";
        }

        private void SaveDisplaySettings()
        {
            mf.isTextureOn = chkDisplayFloor.Checked;
            mf.isGridOn = chkDisplayGrid.Checked;
            mf.isSpeedoOn = chkDisplaySpeedo.Checked;
            mf.isSideGuideLines = chkDisplayExtraGuides.Checked;

            mf.isDrawPolygons = chkDisplayPolygons.Checked;
            mf.isKeyboardOn = chkDisplayKeyboard.Checked;

            mf.isBrightnessOn = chkDisplayBrightness.Checked;
            mf.isSvennArrowOn = chkSvennArrow.Checked;
            mf.isLogElevation = chkDisplayLogElevation.Checked;

            mf.isDirectionMarkers = chkDirectionMarkers.Checked;
            mf.isSectionlinesOn = chkSectionLines.Checked;
            mf.isLineSmooth = chkLineSmooth.Checked;

            //mf.timeToShowMenus = (int)nudMenusOnTime.Value;

            Properties.Settings.Default.setDisplay_isBrightnessOn = mf.isBrightnessOn;
            Properties.Settings.Default.setDisplay_isTextureOn = mf.isTextureOn;
            Properties.Settings.Default.setMenu_isGridOn = mf.isGridOn;
            Properties.Settings.Default.setMenu_isCompassOn = mf.isCompassOn;

            Properties.Settings.Default.setDisplay_isSvennArrowOn = mf.isSvennArrowOn;
            Properties.Settings.Default.setMenu_isSpeedoOn = mf.isSpeedoOn;
            Properties.Settings.Default.setDisplay_isStartFullScreen = chkDisplayStartFullScreen.Checked;
            Properties.Settings.Default.setMenu_isSideGuideLines = mf.isSideGuideLines;

            Properties.Settings.Default.setMenu_isPureOn = mf.isPureDisplayOn;
            Properties.Settings.Default.setMenu_isLightbarOn = mf.isLightbarOn;
            Properties.Settings.Default.setDisplay_isKeyboardOn = mf.isKeyboardOn;
            Properties.Settings.Default.setDisplay_isLogElevation = mf.isLogElevation;

            if (rbtnDisplayMetric.Checked) { Properties.Settings.Default.setMenu_isMetric = true; mf.isMetric = true; }
            else { Properties.Settings.Default.setMenu_isMetric = false; mf.isMetric = false; }

            Properties.Settings.Default.setTool_isDirectionMarkers = mf.isDirectionMarkers;

            Properties.Settings.Default.setAS_numGuideLines = mf.trk.numGuideLines;
            Properties.Settings.Default.setDisplay_isSectionLinesOn = mf.isSectionlinesOn;
            Properties.Settings.Default.setDisplay_isLineSmooth = mf.isLineSmooth;

            
        }

        #endregion

        #region Antenna Enter/Leave
        private void tabVAntenna_Enter(object sender, EventArgs e)
        {
            nudAntennaHeight.Value = (int)(Properties.Settings.Default.setVehicle_antennaHeight * mf.m2InchOrCm);

            nudAntennaPivot.Value = (int)((Properties.Settings.Default.setVehicle_antennaPivot) * mf.m2InchOrCm);

            //negative is to the right
            nudAntennaOffset.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_antennaOffset) * mf.m2InchOrCm);

            rbtnAntennaLeft.Checked = false;
            rbtnAntennaRight.Checked = false;
            rbtnAntennaCenter.Checked = false;
            rbtnAntennaLeft.Checked = Properties.Settings.Default.setVehicle_antennaOffset > 0;
            rbtnAntennaRight.Checked = Properties.Settings.Default.setVehicle_antennaOffset < 0;
            rbtnAntennaCenter.Checked = Properties.Settings.Default.setVehicle_antennaOffset == 0;

            if (Properties.Settings.Default.setVehicle_vehicleType == 0)
                pboxAntenna.BackgroundImage = Properties.Resources.AntennaTractor;

            else if (Properties.Settings.Default.setVehicle_vehicleType == 1)
                pboxAntenna.BackgroundImage = Properties.Resources.AntennaHarvester;

            else if (Properties.Settings.Default.setVehicle_vehicleType == 2)
                pboxAntenna.BackgroundImage = Properties.Resources.Antenna4WD;

            label98.Text = mf.unitsInCm;
            label99.Text = mf.unitsInCm;
            label100.Text = mf.unitsInCm;
        }

        private void tabVAntenna_Leave(object sender, EventArgs e)
        {
            
        }

        private void rbtnAntennaLeft_Click(object sender, EventArgs e)
        {
            if (rbtnAntennaRight.Checked)
                mf.vehicle.antennaOffset = (double)nudAntennaOffset.Value * -mf.inchOrCm2m;
            else if (rbtnAntennaLeft.Checked)
                mf.vehicle.antennaOffset = (double)nudAntennaOffset.Value * mf.inchOrCm2m;
            else
            {
                mf.vehicle.antennaOffset = 0;
                nudAntennaOffset.Value = 0;
            }

            Properties.Settings.Default.setVehicle_antennaOffset = mf.vehicle.antennaOffset;
        }

        private void nudAntennaOffset_ValueChanged(object sender, EventArgs e)
        {
            if ((double)nudAntennaOffset.Value == 0)
            {
                rbtnAntennaLeft.Checked = false;
                rbtnAntennaRight.Checked = false;
                rbtnAntennaCenter.Checked = true;
                mf.vehicle.antennaOffset = 0;
            }
            else
            {
                if (!rbtnAntennaLeft.Checked && !rbtnAntennaRight.Checked)
                    rbtnAntennaRight.Checked = true;

                if (rbtnAntennaRight.Checked)
                    mf.vehicle.antennaOffset = (double)nudAntennaOffset.Value * -mf.inchOrCm2m;
                else
                    mf.vehicle.antennaOffset = (double)nudAntennaOffset.Value * mf.inchOrCm2m;
            }

            Properties.Settings.Default.setVehicle_antennaOffset = mf.vehicle.antennaOffset;

            //rbtnAntennaLeft.Checked = false;
            //rbtnAntennaRight.Checked = false;
            //rbtnAntennaLeft.Checked = Properties.Settings.Default.setVehicle_antennaOffset > 0;
            //rbtnAntennaRight.Checked = Properties.Settings.Default.setVehicle_antennaOffset < 0;
        }

        private void nudAntennaPivot_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.setVehicle_antennaPivot = (double)nudAntennaPivot.Value * mf.inchOrCm2m;
            mf.vehicle.antennaPivot = Properties.Settings.Default.setVehicle_antennaPivot;
        }

        private void nudAntennaHeight_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.setVehicle_antennaHeight = (double)nudAntennaHeight.Value * mf.inchOrCm2m;
            mf.vehicle.antennaHeight = Properties.Settings.Default.setVehicle_antennaHeight;
        }

        #endregion

        #region Vehicle Dimensions

        private void tabVDimensions_Enter(object sender, EventArgs e)
        {
            nudWheelbase.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_wheelbase) * mf.m2InchOrCm);

            nudVehicleTrack.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_trackWidth) * mf.m2InchOrCm);

            nudTractorHitchLength.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_hitchLength) * mf.m2InchOrCm);

            if (mf.vehicle.vehicleType == 0)
            {
                pictureBox1.Image = Properties.Resources.RadiusWheelBase;
                nudTractorHitchLength.Visible = true;
            }
            else if (mf.vehicle.vehicleType == 1)
            {
                pictureBox1.Image = Properties.Resources.RadiusWheelBaseHarvester;
                nudTractorHitchLength.Visible = false;
            }
            else if (mf.vehicle.vehicleType == 2)
            {
                pictureBox1.Image = Properties.Resources.RadiusWheelBase4WD;
                nudTractorHitchLength.Visible = true;
            }

            if (Properties.Settings.Default.setTool_isToolTrailing || Properties.Settings.Default.setTool_isToolTBT)
            {
                nudTractorHitchLength.Visible = true;
                label94.Visible = true;
                lblHitchLength.Visible = true;
            }
            else
            {
                nudTractorHitchLength.Visible = false;
                label94.Visible = false;
                lblHitchLength.Visible = false;
            }

            label94.Text = mf.unitsInCm;
            label95.Text = mf.unitsInCm;
            label97.Text = mf.unitsInCm;
        }

        private void nudTractorHitchLength_ValueChanged(object sender, EventArgs e)
        {
            mf.tool.hitchLength = (double)nudTractorHitchLength.Value * mf.inchOrCm2m;
            if (!Properties.Settings.Default.setTool_isToolFront)
            {
                mf.tool.hitchLength *= -1;
            }
            Properties.Settings.Default.setVehicle_hitchLength = mf.tool.hitchLength;
        }

        private void nudWheelbase_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.setVehicle_wheelbase = (double)nudWheelbase.Value * mf.inchOrCm2m;
            mf.vehicle.wheelbase = Properties.Settings.Default.setVehicle_wheelbase;
        }

        private void nudVehicleTrack_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.setVehicle_trackWidth = (double)nudVehicleTrack.Value * mf.inchOrCm2m;
            mf.vehicle.trackWidth = Properties.Settings.Default.setVehicle_trackWidth;
            mf.tram.halfWheelTrack = mf.vehicle.trackWidth * 0.5;
        }

        #endregion

        #region Vehicle Guidance

        private void tabVGuidance_Enter(object sender, EventArgs e)
        {
        }

        private void tabVGuidance_Leave(object sender, EventArgs e)
        {
        }
        #endregion

        #region VConfig Enter/Leave

        private void tabVConfig_Enter(object sender, EventArgs e)
        {
            if (mf.vehicle.vehicleType == 0) rbtnTractor.Checked = true;
            else if (mf.vehicle.vehicleType == 1) rbtnHarvester.Checked = true;
            else if (mf.vehicle.vehicleType == 2) rbtn4WD.Checked = true;

            original = null;
            TabImageSetup();
        }

        private void tabVConfig_Leave(object sender, EventArgs e)
        {
            if (rbtnTractor.Checked)
            {
                mf.vehicle.vehicleType = 0;
                Properties.Settings.Default.setVehicle_vehicleType = 0;
            }
            if (rbtnHarvester.Checked)
            {
                mf.vehicle.vehicleType = 1;
                Properties.Settings.Default.setVehicle_vehicleType = 1;

                if ( mf.tool.hitchLength < 0) mf.tool.hitchLength *= -1;

                Properties.Settings.Default.setTool_isToolFront = true;
                Properties.Settings.Default.setTool_isToolTBT = false;
                Properties.Settings.Default.setTool_isToolTrailing = false;
                Properties.Settings.Default.setTool_isToolRearFixed = false;
            }
            if (rbtn4WD.Checked)
            {
                mf.vehicle.vehicleType = 2;
                Properties.Settings.Default.setVehicle_vehicleType = 2;
            }

            //the old brand code
            if (cboxIsImage.Checked)
                Properties.Settings.Default.setDisplay_isVehicleImage = false;
            else
                Properties.Settings.Default.setDisplay_isVehicleImage = true;

            mf.vehicleOpacityByte = (byte)(255 * (mf.vehicleOpacity));
            Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicleOpacity * 100);

            Properties.Settings.Default.setDisplay_colorVehicle = mf.vehicleColor;

            if (rbtnTractor.Checked)
            {
                Settings.Default.setBrand_TBrand = brand;

                Bitmap bitmap = mf.GetTractorBrand(brand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Tractor]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

            }

            if (rbtnHarvester.Checked)

            {
                Settings.Default.setBrand_HBrand = brandH;
                Bitmap bitmap = mf.GetHarvesterBrand(brandH);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Harvester]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

            }

            if (rbtn4WD.Checked)
            {
                Settings.Default.setBrand_WDBrand = brand4WD;
                Bitmap bitmap = mf.Get4WDBrandFront(brand4WD);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.FourWDFront]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

                Settings.Default.setBrand_WDBrand = brand4WD;
                bitmap = mf.Get4WDBrandRear(brand4WD);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.FourWDRear]);
                bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
            }
        }

        //brand variables
        TBrand brand;
        HBrand brandH;
        WDBrand brand4WD;

        //Opacity Bar

        Image original = null;

        private void rbtnVehicleType_Click(object sender, EventArgs e)
        {
            if (rbtnTractor.Checked)
            {
                mf.vehicle.vehicleType = 0;
                Properties.Settings.Default.setVehicle_vehicleType = 0;
            }
            if (rbtnHarvester.Checked)
            {
                mf.vehicle.vehicleType = 1;
                Properties.Settings.Default.setVehicle_vehicleType = 1;
            }
            if (rbtn4WD.Checked)
            {
                mf.vehicle.vehicleType = 2;
                Properties.Settings.Default.setVehicle_vehicleType = 2;
            }

            original = null;
            TabImageSetup();
        }

        private void SetOpacity()
        {
            if (original == null) original = (Bitmap)pboxAlpha.BackgroundImage.Clone();
            pboxAlpha.BackColor = Color.Transparent;
            pboxAlpha.BackgroundImage = SetAlpha((Bitmap)original, (byte)(mf.vehicleOpacityByte));
        }

        private void btnOpacityUPGN_Click(object sender, EventArgs e)
        {
            mf.vehicleOpacity = Math.Min(mf.vehicleOpacity + 0.2, 1);
            lblOpacityPercent.Text = ((int)(mf.vehicleOpacity * 100)).ToString() + "%";
            mf.vehicleOpacityByte = (byte)(255 * (mf.vehicleOpacity));
            Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicleOpacity * 100);
            
            SetOpacity();
        }

        private void btnOpacityDn_Click(object sender, EventArgs e)
        {
            mf.vehicleOpacity = Math.Max(mf.vehicleOpacity - 0.2, 0.2);
            lblOpacityPercent.Text = ((int)(mf.vehicleOpacity * 100)).ToString() + "%";
            mf.vehicleOpacityByte = (byte)(255 * (mf.vehicleOpacity));
            Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicleOpacity * 100);
            
            SetOpacity();
        }

        private void cboxIsImage_Click(object sender, EventArgs e)
        {
            //mf.vehicleOpacity = (hsbarOpacity.Value * 0.01);
            mf.vehicleOpacityByte = (byte)(255 * (mf.vehicleOpacity));
            Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicleOpacity * 100);

            mf.isVehicleImage = (!cboxIsImage.Checked);
            Properties.Settings.Default.setDisplay_isVehicleImage = mf.isVehicleImage;
            
            //original = null;
            TabImageSetup();
        }

        private void TabImageSetup()
        {
            panel4WdBrands.Visible = false;
            panelTractorBrands.Visible = false;
            panelHarvesterBrands.Visible = false;

            if (mf.isVehicleImage)
            {
                if (mf.vehicle.vehicleType == 0)
                {
                    panelTractorBrands.Visible = true;

                    brand = Settings.Default.setBrand_TBrand;

                    if (brand == TBrand.AgOpenGPS)
                        rbtnBrandTAgOpenGPS.Checked = true;
                    else if (brand == TBrand.Case)
                        rbtnBrandTCase.Checked = true;
                    else if (brand == TBrand.Claas)
                        rbtnBrandTClaas.Checked = true;
                    else if (brand == TBrand.Deutz)
                        rbtnBrandTDeutz.Checked = true;
                    else if (brand == TBrand.Fendt)
                        rbtnBrandTFendt.Checked = true;
                    else if (brand == TBrand.JDeere)
                        rbtnBrandTJDeere.Checked = true;
                    else if (brand == TBrand.Kubota)
                        rbtnBrandTKubota.Checked = true;
                    else if (brand == TBrand.Massey)
                        rbtnBrandTMassey.Checked = true;
                    else if (brand == TBrand.NewHolland)
                        rbtnBrandTNH.Checked = true;
                    else if (brand == TBrand.Same)
                        rbtnBrandTSame.Checked = true;
                    else if (brand == TBrand.Steyr)
                        rbtnBrandTSteyr.Checked = true;
                    else if (brand == TBrand.Ursus)
                        rbtnBrandTUrsus.Checked = true;
                    else if (brand == TBrand.Valtra)
                        rbtnBrandTValtra.Checked = true;

                    pboxAlpha.BackgroundImage = mf.GetTractorBrand(Settings.Default.setBrand_TBrand);
                }
                else if (mf.vehicle.vehicleType == 1)
                {
                    panelHarvesterBrands.Visible = true;

                    brandH = Settings.Default.setBrand_HBrand;

                    if (brandH == HBrand.AgOpenGPS)
                        rbtnBrandHAgOpenGPS.Checked = true;
                    else if (brandH == HBrand.Case)
                        rbtnBrandHCase.Checked = true;
                    else if (brandH == HBrand.Claas)
                        rbtnBrandHClaas.Checked = true;
                    else if (brandH == HBrand.JDeere)
                        rbtnBrandHJDeere.Checked = true;
                    else if (brandH == HBrand.NewHolland)
                        rbtnBrandHNH.Checked = true;

                    pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(Settings.Default.setBrand_HBrand);
                }
                else if (mf.vehicle.vehicleType == 2)
                {
                    panel4WdBrands.Visible = true;

                    brand4WD = Settings.Default.setBrand_WDBrand;

                    if (brand4WD == WDBrand.AgOpenGPS)
                        rbtnBrand4WDAgOpenGPS.Checked = true;
                    else if (brand4WD == WDBrand.Case)
                        rbtnBrand4WDCase.Checked = true;
                    else if (brand4WD == WDBrand.Challenger)
                        rbtnBrand4WDChallenger.Checked = true;
                    else if (brand4WD == WDBrand.JDeere)
                        rbtnBrand4WDJDeere.Checked = true;
                    else if (brand4WD == WDBrand.NewHolland)
                        rbtnBrand4WDNH.Checked = true;
                    else if (brand4WD == WDBrand.Holder)
                        rbtnBrand4WDHolder.Checked = true;

                    pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(Settings.Default.setBrand_WDBrand);
                }

                mf.vehicleOpacityByte = (byte)(255 * (mf.vehicleOpacity));
                Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicleOpacity * 100);
                lblOpacityPercent.Text = ((int)(mf.vehicleOpacity * 100)).ToString() + "%";
                mf.vehicleColor = Color.FromArgb(254, 254, 254);
            }
            else
            {
                pboxAlpha.BackgroundImage = Properties.Resources.TriangleVehicle;
                lblOpacityPercent.Text = ((int)(mf.vehicleOpacity * 100)).ToString() + "%";
                mf.vehicleColor = Color.FromArgb(254, 254, 254);
            }

            cboxIsImage.Checked = !mf.isVehicleImage;

            original = null;
            SetOpacity();
        }

        static Bitmap SetAlpha(Bitmap bmpIn, int alpha)
        {
            Bitmap bmpOut = new Bitmap(bmpIn.Width, bmpIn.Height);
            float a = alpha / 255f;
            Rectangle r = new Rectangle(0, 0, bmpIn.Width, bmpIn.Height);

            float[][] matrixItems = {
                            new float[] {1, 0, 0, 0, 0},
                            new float[] {0, 1, 0, 0, 0},
                            new float[] {0, 0, 1, 0, 0},
                            new float[] {0, 0, 0, a, 0},
                            new float[] {0, 0, 0, 0, 1}};

            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

            ImageAttributes imageAtt = new ImageAttributes();
            imageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(bmpOut))
                g.DrawImage(bmpIn, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, imageAtt);

            return bmpOut;
        }

        private void LoadBrandImage()
        {
            if (rbtnTractor.Checked)
            {
                Bitmap bitmap = mf.GetTractorBrand(Settings.Default.setBrand_TBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Tractor]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
            }
            else if (rbtnHarvester.Checked)
            {
                Bitmap bitmap = mf.GetHarvesterBrand(Settings.Default.setBrand_HBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Harvester]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

            }
            else if (rbtn4WD.Checked)
            {
                Bitmap bitmap = mf.Get4WDBrandFront(Settings.Default.setBrand_WDBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.FourWDFront]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
                bitmap = mf.Get4WDBrandRear(Settings.Default.setBrand_WDBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.FourWDRear]);
                bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
            }
        }

        //Check Brand is changed

        private void rbtnBrandTAgOpenGPS_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                {
                    brand = TBrand.AgOpenGPS;
                    pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                    original = null;
                    SetOpacity();
                }
            }
        }

        private void rbtnBrandTCase_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                {
                    brand = TBrand.Case;
                    pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                    original = null;
                    SetOpacity();
                }
            }
        }

        private void rbtnBrandTClaas_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                {
                    brand = TBrand.Claas;
                    pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                    original = null;
                    SetOpacity();
                }
            }
        }

        private void rbtnBrandTDeutz_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Deutz;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTFendt_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Fendt;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTJDeere_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.JDeere;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTKubota_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Kubota;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTMassey_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Massey;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTNH_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.NewHolland;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTSame_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Same;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTSteyr_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Steyr;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTUrsus_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Ursus;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTValtra_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Valtra;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandHAgOpenGPS_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brandH = HBrand.AgOpenGPS;
                pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(brandH);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandHCase_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brandH = HBrand.Case;
                pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(brandH);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandHClaas_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brandH = HBrand.Claas;
                pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(brandH);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandHJDeere_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                {
                    brandH = HBrand.JDeere;
                    pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(brandH);
                    original = null;
                    SetOpacity();
                }
            }
        }

        private void rbtnBrandHNH_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brandH = HBrand.NewHolland;
                pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(brandH);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrand4WDAgOpenGPS_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand4WD = WDBrand.AgOpenGPS;
                pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(brand4WD);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrand4WDCase_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand4WD = WDBrand.Case;
                pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(brand4WD);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrand4WDChallenger_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand4WD = WDBrand.Challenger;
                pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(brand4WD);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrand4WDJDeere_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand4WD = WDBrand.JDeere;
                pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(brand4WD);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrand4WDNH_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand4WD = WDBrand.NewHolland;
                pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(brand4WD);
                original = null;
                SetOpacity();
            }
        }
        private void rbtnBrand4WDHolder_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand4WD = WDBrand.Holder;
                pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(brand4WD);
                original = null;
                SetOpacity();
            }
        }

        #endregion
    }
}

