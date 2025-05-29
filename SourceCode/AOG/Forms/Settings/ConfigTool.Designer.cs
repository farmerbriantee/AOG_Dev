using AOG.Classes;
using AOG.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormConfig
    {
        private double[] sectionWidthArr = new double[16];

        private double defaultSectionWidth;

        private int numberOfSections;

        #region Config
        private void tabTConfig_Enter(object sender, EventArgs e)
        {
            lblInchCm2.Text = glm.unitsInCm.ToString();

            if (mf.vehicle.vehicleType != 1)
            {
                pboxConfigHarvester.Visible = false;

                rbtnTBT.Visible = true;
                rbtnTrailing.Visible = true;
                rbtnFixedRear.Visible = true;
                rbtnFront.Visible = true;

                if (Settings.Tool.isToolFront)
                {
                    rbtnTBT.Checked = false;
                    rbtnTrailing.Checked = false;
                    rbtnFixedRear.Checked = false;
                    rbtnFront.Checked = true;
                }
                else if (Settings.Tool.isToolTBT)
                {
                    rbtnTBT.Checked = true;
                    rbtnTrailing.Checked = false;
                    rbtnFixedRear.Checked = false;
                    rbtnFront.Checked = false;
                }
                else if (Settings.Tool.isToolTrailing)
                {
                    rbtnTBT.Checked = false;
                    rbtnTrailing.Checked = true;
                    rbtnFixedRear.Checked = false;
                    rbtnFront.Checked = false;
                }
                else if (Settings.Tool.isToolRearFixed)
                {
                    rbtnTBT.Checked = false;
                    rbtnTrailing.Checked = false;
                    rbtnFixedRear.Checked = true;
                    rbtnFront.Checked = false;
                }
            }
            else
            {
                pboxConfigHarvester.Visible = true;

                rbtnTBT.Visible = false;
                rbtnTrailing.Visible = false;
                rbtnFixedRear.Visible = false;
                rbtnFront.Visible = false;

            }
        }
        private void tabTConfig_Leave(object sender, EventArgs e)
        {
            if (mf.vehicle.vehicleType != 1)
            {
                Settings.Tool.isToolFront = rbtnFront.Checked;
                Settings.Tool.isToolTBT = rbtnTBT.Checked;
                Settings.Tool.isToolTrailing = rbtnTrailing.Checked || rbtnTBT.Checked;
                Settings.Tool.isToolRearFixed = rbtnFixedRear.Checked;
            }
            else
            {
                Settings.Tool.isToolFront = true;
                Settings.Tool.isToolTBT = false;
                Settings.Tool.isToolTrailing = false;
                Settings.Tool.isToolRearFixed = false;
            }

            //Settings.Tool.hitchLength = (double)nudDrawbarLength.Value * glm.inchOrCm2m;
            if (Settings.Tool.isToolFront && Settings.Tool.hitchLength < 0)
                Settings.Tool.hitchLength *= -1;
            else if (!Settings.Tool.isToolFront && Settings.Tool.hitchLength > 0)
                Settings.Tool.hitchLength *= -1;
        }

        #endregion

        #region  Hitch

        private void tabTHitch_Enter(object sender, EventArgs e)
        {
            if (mf.vehicle.vehicleType != 1)
            {
                //fixed -hitch only on vehicle
                if (Settings.Tool.isToolFront)
                {
                    nudTrailingHitchLength.Visible = false;
                    nudDrawbarLength.Visible = true;
                    nudTankHitch.Visible = false;

                    nudDrawbarLength.Left = 401;

                    picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageFront;
                }
                else if (Settings.Tool.isToolRearFixed)
                {
                    nudTrailingHitchLength.Visible = false;
                    nudDrawbarLength.Visible = true;
                    nudTankHitch.Visible = false;

                    nudDrawbarLength.Left = 259;

                    picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageRear;
                }

                //trailing
                else if (Settings.Tool.isToolTBT)
                {
                    nudTrailingHitchLength.Visible = true;
                    nudDrawbarLength.Visible = false;
                    nudTankHitch.Visible = true;

                    nudTrailingHitchLength.Left = 326;
                    nudTankHitch.Left = 643;

                    picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageTBT;
                }
                else if (Settings.Tool.isToolTrailing)
                {
                    nudTrailingHitchLength.Visible = true;
                    nudDrawbarLength.Visible = false;
                    nudTankHitch.Visible = false;

                    nudTrailingHitchLength.Left = 438;

                    picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageTrailing;
                }

                label112.Text = glm.unitsInCm;
            }
            else
            {
                nudTrailingHitchLength.Visible = false;
                nudDrawbarLength.Visible = true;
                nudTankHitch.Visible = false;

                nudDrawbarLength.Left = 580;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageFrontHarvester;
            }

            nudDrawbarLength.Value = Math.Abs(Settings.Tool.hitchLength);
            nudTrailingHitchLength.Value = Math.Abs(Settings.Tool.toolTrailingHitchLength);
            nudTankHitch.Value = Math.Abs(Settings.Tool.tankTrailingHitchLength);
        }

        private void tabTHitch_Leave(object sender, EventArgs e)
        {
            
        }

        private void nudDrawbarLength_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.hitchLength = nudDrawbarLength.Value;
            if (!Settings.Tool.isToolFront)
            {
                Settings.Tool.hitchLength *= -1;
            }
        }

        private void nudTankHitch_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.tankTrailingHitchLength = -nudTankHitch.Value;
        }

        private void nudTrailingHitchLength_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.toolTrailingHitchLength = -nudTrailingHitchLength.Value;
        }

        #endregion

        #region Settings

        private void tabTSettings_Enter(object sender, EventArgs e)
        {
            nudLookAhead.Value = Settings.Tool.lookAheadOn;
            nudLookAheadOff.Value = Settings.Tool.lookAheadOff;
            nudTurnOffDelay.Value = Settings.Tool.offDelay;
            nudLookAheadDistanceOn.Value = Settings.Tool.lookAheadDistanceOn;
            nudLookAheadDistanceOff.Value = Settings.Tool.lookAheadDistanceOff;

            //pictureBox3.Image = Resources.ToolLookaheadOn;
            //pictureBox4.Image = Resources.ToolLookaheadOff;
        }

        private void tabTSettings_Leave(object sender, EventArgs e)
        {
            ////line up manual buttons based on # of sections
            //mf.LineUpManualBtns();

            ////update the sections to newly configured widths and positions in main
            //mf.SectionSetPosition();

            ////update the widths of sections and tool width in main
            //mf.SectionCalcWidths();

            
        }
        private void nudLookAhead_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.lookAheadOn = nudLookAhead.Value;
        }

        private void nudLookAheadOff_ValueChanged(object sender, EventArgs e)
        {
            if (nudLookAheadOff.Value > 0)
            {
                Settings.Tool.offDelay = nudTurnOffDelay.Value = 0;
            }
            Settings.Tool.lookAheadOff = nudLookAheadOff.Value;
        }

        private void nudTurnOffDelay_ValueChanged(object sender, EventArgs e)
        {
            if (nudTurnOffDelay.Value > 0)
            {
                Settings.Tool.lookAheadOff = nudLookAheadOff.Value = 0;
            }
            Settings.Tool.offDelay = nudTurnOffDelay.Value;
        }

        private void nudLookAheadDistanceOn_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.lookAheadDistanceOn = nudLookAheadDistanceOn.Value;

        }

        private void nudLookAheadDistanceOff_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.lookAheadDistanceOff = nudLookAheadDistanceOff.Value;

        }

        #endregion

        #region offset
        private void tabToolOffset_Enter(object sender, EventArgs e)
        {
            nudOffset.Value = Math.Abs(Settings.Tool.offset);
            rbtnToolRightPositive.Checked = Settings.Tool.offset > 0;
            rbtnLeftNegative.Checked = Settings.Tool.offset < 0;

            nudOverlap.Value = Math.Abs(Settings.Tool.overlap);
            rbtnToolOverlap.Checked = Settings.Tool.overlap > 0;
            rbtnToolGap.Checked = Settings.Tool.overlap < 0;

            label175.Text = glm.unitsInCm;
            label176.Text = glm.unitsInCm;
        }

        private void tabToolOffset_Leave(object sender, EventArgs e)
        {
            if (Settings.Tool.offset != (rbtnToolRightPositive.Checked ? nudOffset.Value : -nudOffset.Value))
            {
                Settings.Tool.offset = rbtnToolRightPositive.Checked ? nudOffset.Value : -nudOffset.Value;
                mf.SectionSetPosition();
            }
        }

        private void nudOffset_ValueChanged(object sender, EventArgs e)
        {
            if (!rbtnToolRightPositive.Checked && !rbtnLeftNegative.Checked)
                rbtnToolRightPositive.Checked = true;
        }

        private void btnZeroToolOffset_Click(object sender, EventArgs e)
        {
            nudOffset.Value = 0;
            rbtnToolRightPositive.Checked = false;
            rbtnLeftNegative.Checked = false;
        }

        private void rbtnToolRightPositive_Click(object sender, EventArgs e)
        {
            rbtnToolRightPositive.Checked = nudOffset.Value > 0;
            rbtnLeftNegative.Checked = false;
        }

        private void rbtnToolLeftNegative_Click(object sender, EventArgs e)
        {
            rbtnToolRightPositive.Checked = false;
            rbtnLeftNegative.Checked = nudOffset.Value > 0;
        }

        private void nudOverlaPGN_ValueChanged(object sender, EventArgs e)
        {
            if (!rbtnToolOverlap.Checked && !rbtnToolGap.Checked)
                rbtnToolOverlap.Checked = true;

            if (rbtnToolOverlap.Checked)
                Settings.Tool.overlap = nudOverlap.Value;
            else
                Settings.Tool.overlap = -nudOverlap.Value;

            Settings.Tool.overlap = Settings.Tool.overlap;

            rbtnToolOverlap.Checked = false;
            rbtnToolGap.Checked = false;
            rbtnToolOverlap.Checked = Settings.Tool.overlap > 0;
            rbtnToolGap.Checked = Settings.Tool.overlap < 0;
        }

        private void btnZeroOverlaPGN_Click(object sender, EventArgs e)
        {
            nudOverlap.Value = 0;
            rbtnToolOverlap.Checked = false;
            rbtnToolGap.Checked = false;

            Settings.Tool.overlap = 0;
            Settings.Tool.overlap = Settings.Tool.overlap;
        }

        private void rbtnToolOverlaPGN_Click(object sender, EventArgs e)
        {
            if (rbtnToolOverlap.Checked)
                Settings.Tool.overlap = nudOverlap.Value;
            else
                Settings.Tool.overlap = -nudOverlap.Value;
            Settings.Tool.overlap = Settings.Tool.overlap;
            

            rbtnToolOverlap.Checked = false;
            rbtnToolGap.Checked = false;
            rbtnToolOverlap.Checked = Settings.Tool.overlap > 0;
            rbtnToolGap.Checked = Settings.Tool.overlap < 0;
        }

        #endregion

        #region PivotDistance

        private void tabToolPivot_Enter(object sender, EventArgs e)
        {
            nudTrailingToolToPivotLength.Value = Math.Abs(Settings.Tool.trailingToolToPivotLength);

            rbtnPivotBehindPos.Checked = false;
            rbtnPivotAheadNeg.Checked = false;
            rbtnPivotBehindPos.Checked = Settings.Tool.trailingToolToPivotLength > 0;
            rbtnPivotAheadNeg.Checked = Settings.Tool.trailingToolToPivotLength < 0;

            label177.Text = glm.unitsInCm;
        }

        private void tabToolPivot_Leave(object sender, EventArgs e)
        {
            
        }


        private void btnPivotOffsetZero_Click(object sender, EventArgs e)
        {
            nudTrailingToolToPivotLength.Value = 0;
            rbtnPivotBehindPos.Checked = false;
            rbtnPivotAheadNeg.Checked = false;

            Settings.Tool.trailingToolToPivotLength = 0;
        }


        private void rbtnPivotBehindPos_Click(object sender, EventArgs e)
        {
            if (rbtnPivotBehindPos.Checked)
                Settings.Tool.trailingToolToPivotLength = nudTrailingToolToPivotLength.Value;
            else
                Settings.Tool.trailingToolToPivotLength = -nudTrailingToolToPivotLength.Value;
            Settings.Tool.trailingToolToPivotLength = Settings.Tool.trailingToolToPivotLength;

            rbtnPivotBehindPos.Checked = false;
            rbtnPivotAheadNeg.Checked = false;
            rbtnPivotBehindPos.Checked = Settings.Tool.trailingToolToPivotLength > 0;
            rbtnPivotAheadNeg.Checked = Settings.Tool.trailingToolToPivotLength < 0;
        }

        private void nudTrailingToolToPivotLength_ValueChanged(object sender, EventArgs e)
        {
            if (rbtnPivotBehindPos.Checked)
                Settings.Tool.trailingToolToPivotLength = nudTrailingToolToPivotLength.Value;
            else
                Settings.Tool.trailingToolToPivotLength = -nudTrailingToolToPivotLength.Value;

            Settings.Tool.trailingToolToPivotLength = Settings.Tool.trailingToolToPivotLength;

            rbtnPivotBehindPos.Checked = false;
            rbtnPivotAheadNeg.Checked = false;
            rbtnPivotBehindPos.Checked = Settings.Tool.trailingToolToPivotLength > 0;
            rbtnPivotAheadNeg.Checked = Settings.Tool.trailingToolToPivotLength < 0;
        }


        #endregion

        #region Sections 

        private void tabTSections_Enter(object sender, EventArgs e)
        {
            mf.TurnOffSectionsSafely();

            //cboxSectionBoundaryControl.Checked = Settings.Tool.isSectionOffWhenOut;
            //if (cboxSectionBoundaryControl.Checked)
            //{
            //    cboxSectionBoundaryControl.BackgroundImage = Properties.Resources.SectionOffBoundary;
            //}
            //else
            //{
            //    cboxSectionBoundaryControl.BackgroundImage = Properties.Resources.SectionOnBoundary;
            //}

            nudCutoffSpeed.Value = Settings.Tool.slowSpeedCutoff;

            nudNumberOfSections.Maximum = FormGPS.MAXSECTIONS;

            nudMinCoverage.Value = Settings.Tool.minCoverage;

            label178.Text = glm.unitsInCm;
            cboxIsUnique.Checked = !Settings.Tool.isSectionsNotZones;
            nudDefaultSectionWidth.DecimalPlaces = 1;

            if (!cboxIsUnique.Checked)
            {
                cboxIsUnique.BackgroundImage = Properties.Resources.ConT_Asymmetric;
                cboxNumberOfZones.Visible = lblZonesBox.Visible = false;

                numberOfSections = Settings.Tool.numSections;

                cboxNumSections.Text = numberOfSections.ToString();
                defaultSectionWidth = Settings.Tool.defaultSectionWidth;
                nudDefaultSectionWidth.Value = defaultSectionWidth;

                panelSymmetricSections.Visible = false;

                nudNumberOfSections.Visible = false;
                cboxNumSections.Visible = true;

                foreach (var item in tabTSections.Controls)
                {
                    if (item is NudlessNumericUpDown numeric)
                    {
                        if (numeric.Name.Substring(0, 6) == "nudSec")
                        {
                            //grab the number from nudSection01
                            string bob = numeric.Name.Substring(10, 2);
                            int nudNum = Convert.ToInt32(bob);
                            if (nudNum > 0 && nudNum <= 16 && nudNum <= Settings.Tool.numSections)
                            {
                                numeric.Value = Settings.Tool.setSection_Widths[nudNum - 1];
                            }
                        }
                    }
                }

                //based on number of sections and values update the page before displaying
                UpdateSpinners();
            }
            else
            {
                cboxIsUnique.BackgroundImage = Properties.Resources.ConT_Symmetric;
                cboxNumberOfZones.Visible = lblZonesBox.Visible = true;

                cboxNumSections.Visible = false;

                panelSymmetricSections.Visible = true;
                nudNumberOfSections.Visible = true;

                numberOfSections = Settings.Tool.numSectionsMulti;
                nudNumberOfSections.Value = numberOfSections;

                defaultSectionWidth = Settings.Tool.sectionWidthMulti;
                nudDefaultSectionWidth.Value = defaultSectionWidth;

                SetNudZoneMinMax();

                nudZone1To.Value = mf.tool.zoneRanges[1];
                nudZone2To.Value = mf.tool.zoneRanges[2];
                nudZone3To.Value = mf.tool.zoneRanges[3];
                nudZone4To.Value = mf.tool.zoneRanges[4];
                nudZone5To.Value = mf.tool.zoneRanges[5];
                nudZone6To.Value = mf.tool.zoneRanges[6];
                nudZone7To.Value = mf.tool.zoneRanges[7];
                nudZone8To.Value = mf.tool.zoneRanges[8];

                cboxNumberOfZones.SelectedIndexChanged -= cboxNumberOfZones_SelectedIndexChanged;
                cboxNumberOfZones.Text = mf.tool.zones.ToString();
                cboxNumberOfZones.SelectedIndexChanged += cboxNumberOfZones_SelectedIndexChanged;

                words = Settings.Tool.zones.Split(',');
                lblVehicleToolWidth.Text = Convert.ToString((int)(numberOfSections * defaultSectionWidth * glm.m2InchOrCm));

                SetNudZoneVisibility();
            }
        }

        private void tabTSections_Leave(object sender, EventArgs e)
        {
            Settings.Tool.isSectionsNotZones = !cboxIsUnique.Checked;

            if (!cboxIsUnique.Checked)
            {
                //take the section widths and convert to meters and positions along tool.
                CalculateSectionPositions();

                //Settings.Tool.isSectionOffWhenOut = cboxSectionBoundaryControl.Checked;

                Settings.Tool.setSection_Widths = sectionWidthArr;

                Settings.Tool.numSections = numberOfSections;

            }
            else
            {
                //no multi color zones
                Settings.Tool.setColor_isMultiColorSections = false;

                Settings.Tool.numSectionsMulti = numberOfSections;

                Settings.Tool.toolWidth = numberOfSections * defaultSectionWidth;
        

                for (int i = 0; i < 9; i++)
                {
                    mf.tool.zoneRanges[i] = 0;
                }

                mf.tool.zoneRanges[0] = mf.tool.zones;

                if (mf.tool.zones == 2)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                }
                else if (mf.tool.zones == 3)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                    mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                }
                else if (mf.tool.zones == 4)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                    mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                    mf.tool.zoneRanges[4] = (int)nudZone4To.Value;
                }
                else if (mf.tool.zones == 5)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                    mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                    mf.tool.zoneRanges[4] = (int)nudZone4To.Value;
                    mf.tool.zoneRanges[5] = (int)nudZone5To.Value;
                }
                else if (mf.tool.zones == 6)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                    mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                    mf.tool.zoneRanges[4] = (int)nudZone4To.Value;
                    mf.tool.zoneRanges[5] = (int)nudZone5To.Value;
                    mf.tool.zoneRanges[6] = (int)nudZone6To.Value;
                }
                else if (mf.tool.zones == 7)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                    mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                    mf.tool.zoneRanges[4] = (int)nudZone4To.Value;
                    mf.tool.zoneRanges[5] = (int)nudZone5To.Value;
                    mf.tool.zoneRanges[6] = (int)nudZone6To.Value;
                    mf.tool.zoneRanges[7] = (int)nudZone7To.Value;
                }
                else if (mf.tool.zones == 8)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                    mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                    mf.tool.zoneRanges[4] = (int)nudZone4To.Value;
                    mf.tool.zoneRanges[5] = (int)nudZone5To.Value;
                    mf.tool.zoneRanges[6] = (int)nudZone6To.Value;
                    mf.tool.zoneRanges[7] = (int)nudZone7To.Value;
                    mf.tool.zoneRanges[8] = (int)nudZone8To.Value;
                }

                String str = "";
                str = String.Join(",",mf.tool.zoneRanges);
                Settings.Tool.zones = str;
            }


            //update the sections to newly configured widths and positions in main
            mf.SectionSetPosition();
            mf.tram.IsTramOuterOrInner();
            SendRelaySettingsToMachineModule();

            mf.SetNumOfControlButtons(!cboxIsUnique.Checked ? numberOfSections : mf.tool.zones, numberOfSections);
        }

        private void nudZone1To_ValueChanged(object sender, EventArgs e)
        {
            mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
            SetNudZoneVisibility();
        }

        private void nudZone2To_ValueChanged(object sender, EventArgs e)
        {
            mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
            SetNudZoneVisibility();
        }

        private void nudZone3To_ValueChanged(object sender, EventArgs e)
        {
            mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
            SetNudZoneVisibility();
        }

        private void nudZone4To_ValueChanged(object sender, EventArgs e)
        {
            mf.tool.zoneRanges[4] = (int)nudZone4To.Value;
            SetNudZoneVisibility();
        }

        private void nudZone5To_ValueChanged(object sender, EventArgs e)
        {
            mf.tool.zoneRanges[5] = (int)nudZone5To.Value;
            SetNudZoneVisibility();
        }

        private void nudZone6To_ValueChanged(object sender, EventArgs e)
        {
            mf.tool.zoneRanges[6] = (int)nudZone6To.Value;
            SetNudZoneVisibility();
        }

        private void nudZone7To_ValueChanged(object sender, EventArgs e)
        {
            mf.tool.zoneRanges[7] = (int)nudZone7To.Value;
            SetNudZoneVisibility();
        }

        private void nudZone8To_ValueChanged(object sender, EventArgs e)
        {
            mf.tool.zoneRanges[8] = (int)nudZone8To.Value;
            SetNudZoneVisibility();
        }

        private void cboxNumberOfZones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cboxNumberOfZones.SelectedIndex+2) > (int)nudNumberOfSections.Value)
            {
                mf.YesMessageBox("You can't have more zones then sections");
                cboxNumberOfZones.SelectedIndex = mf.tool.zones - 2;

                return;
            }

            mf.tool.zones = cboxNumberOfZones.SelectedIndex + 2;
            //if (mf.tool.zones == 1) mf.tool.zones = 0;


            SetNudZoneMinMax();
            FillZoneNudsWithDefaultValues();
            SetNudZoneVisibility();
        }

        private void FillZoneNudsWithDefaultValues()
        {
            nudZone1To.Value = 0;
            nudZone2To.Value = 0;
            nudZone3To.Value = 0;
            nudZone4To.Value = 0;
            nudZone5To.Value = 0;
            nudZone6To.Value = 0;
            nudZone7To.Value = 0;
            nudZone8To.Value = 0;

            if (mf.tool.zones != 0)
            {
                int defa = numberOfSections / mf.tool.zones;
                if (mf.tool.zones == 2)
                {
                    nudZone1To.Value = defa;
                    nudZone2To.Value = numberOfSections;
                }
                else if (mf.tool.zones == 3)
                {
                    nudZone1To.Value = defa;
                    nudZone2To.Value = 2 * defa;
                    nudZone3To.Value = numberOfSections;
                }
                else if (mf.tool.zones == 4)
                {
                    nudZone1To.Value = defa;
                    nudZone2To.Value = 2 * defa;
                    nudZone3To.Value = 3 * defa;
                    nudZone4To.Value = numberOfSections;
                }
                else if (mf.tool.zones == 5)
                {
                    nudZone1To.Value = defa;
                    nudZone2To.Value = 2 * defa;
                    nudZone3To.Value = 3 * defa;
                    nudZone4To.Value = 4 * defa;
                    nudZone5To.Value = numberOfSections;
                }
                else if (mf.tool.zones == 6)
                {
                    nudZone1To.Value = defa;
                    nudZone2To.Value = 2 * defa;
                    nudZone3To.Value = 3 * defa;
                    nudZone4To.Value = 4 * defa;
                    nudZone5To.Value = 5 * defa;
                    nudZone6To.Value = numberOfSections;
                }
                else if (mf.tool.zones == 7)
                {
                    nudZone1To.Value = defa;
                    nudZone2To.Value = 2 * defa;
                    nudZone3To.Value = 3 * defa;
                    nudZone4To.Value = 4 * defa;
                    nudZone5To.Value = 5 * defa;
                    nudZone6To.Value = 6 * defa;
                    nudZone7To.Value = numberOfSections;
                }
                else if (mf.tool.zones == 8)
                {
                    nudZone1To.Value = defa;
                    nudZone2To.Value = 2 * defa;
                    nudZone3To.Value = 3 * defa;
                    nudZone4To.Value = 4 * defa;
                    nudZone5To.Value = 5 * defa;
                    nudZone6To.Value = 6 * defa;
                    nudZone7To.Value = 7 * defa;
                    nudZone8To.Value = numberOfSections;
                }
            }
        }

        private void SetNudZoneMinMax()
        {
            nudZone1To.Maximum = numberOfSections;
            nudZone2To.Maximum = numberOfSections;
            nudZone3To.Maximum = numberOfSections;
            nudZone4To.Maximum = numberOfSections;
            nudZone5To.Maximum = numberOfSections;
            nudZone6To.Maximum = numberOfSections;
            nudZone7To.Maximum = numberOfSections;
            nudZone8To.Maximum = numberOfSections;
        }

        private void SetNudZoneVisibility()
        {
            nudZone1To.Visible = false;
            nudZone2To.Visible = false;
            nudZone3To.Visible = false;
            nudZone4To.Visible = false;
            nudZone5To.Visible = false;
            nudZone6To.Visible = false;
            nudZone7To.Visible = false;
            nudZone8To.Visible = false;

            nudZone1To.Enabled = true;
            nudZone2To.Enabled = true;
            nudZone3To.Enabled = true;
            nudZone4To.Enabled = true;
            nudZone5To.Enabled = true;
            nudZone6To.Enabled = true;
            nudZone7To.Enabled = true;
            nudZone8To.Enabled = true;

            lblZoneStart1.Visible = false;
            lblZoneStart2.Visible = false;
            lblZoneStart3.Visible = false;
            lblZoneStart4.Visible = false;
            lblZoneStart5.Visible = false;
            lblZoneStart6.Visible = false;
            lblZoneStart7.Visible = false;
            lblZoneStart8.Visible = false;

            if (mf.tool.zones > 1)
            {
                nudZone2To.Visible = true;
                nudZone1To.Visible = true;
                lblZoneStart1.Visible = true;
                lblZoneStart2.Visible = true;
                lblZoneStart2.Text = (nudZone1To.Value + 1).ToString();
                if (mf.tool.zones == 2) nudZone2To.Enabled = false;
            }

            if (mf.tool.zones > 2)
            {
                nudZone3To.Visible = true;
                lblZoneStart3.Visible = true;
                lblZoneStart3.Text = (nudZone2To.Value + 1).ToString();
                if (mf.tool.zones == 3) nudZone3To.Enabled = false;
            }

            if (mf.tool.zones > 3)
            {
                nudZone4To.Visible = true;
                lblZoneStart4.Visible = true;
                lblZoneStart4.Text = (nudZone3To.Value + 1).ToString();
                if (mf.tool.zones == 4) nudZone4To.Enabled = false;
            }

            if (mf.tool.zones > 4)
            {
                nudZone5To.Visible = true;
                lblZoneStart5.Visible = true;
                lblZoneStart5.Text = (nudZone4To.Value + 1).ToString();
                if (mf.tool.zones == 5) nudZone5To.Enabled = false;
            }

            if (mf.tool.zones > 5)
            {
                nudZone6To.Visible = true;
                lblZoneStart6.Visible = true;
                lblZoneStart6.Text = (nudZone5To.Value + 1).ToString();
                if (mf.tool.zones == 6) nudZone6To.Enabled = false;
            }

            if (mf.tool.zones > 6)
            {
                nudZone7To.Visible = true;
                lblZoneStart7.Visible = true;
                lblZoneStart7.Text = (nudZone6To.Value + 1).ToString();
                if (mf.tool.zones == 7) nudZone7To.Enabled = false;
            }

            if (mf.tool.zones > 7)
            {
                nudZone8To.Visible = true;
                lblZoneStart8.Visible = true;
                lblZoneStart8.Text = (nudZone7To.Value + 1).ToString();
                if (mf.tool.zones == 8) nudZone8To.Enabled = false;
            }
        }

        private void cboxIsUnique_Click(object sender, EventArgs e)
        {
            Settings.Tool.isSectionsNotZones = !cboxIsUnique.Checked;
            tabTSections_Enter(null, null);
            tabTSections_Leave(null, null);
        }

        private void nudNumberOfSections_ValueChanged(object sender, EventArgs e)
        {
            if ((int)nudNumberOfSections.Value < mf.tool.zones)
            {
                mf.YesMessageBox("You can't have more zones then sections");
                nudNumberOfSections.Value = numberOfSections;
                return;
            }
            numberOfSections = (int)nudNumberOfSections.Value;
            SetNudZoneMinMax();

            Settings.Tool.numSectionsMulti = numberOfSections;


            lblVehicleToolWidth.Text = Convert.ToString((int)(numberOfSections * defaultSectionWidth * glm.m2InchOrCm));
            SectionFeetInchesTotalWidthLabelUpdate();
            FillZoneNudsWithDefaultValues();
            SetNudZoneVisibility();
        }

        private void nudDefaultSectionWidth_ValueChanged(object sender, EventArgs e)
        {
            defaultSectionWidth = nudDefaultSectionWidth.Value;

            if (!cboxIsUnique.Checked)
                Settings.Tool.defaultSectionWidth = defaultSectionWidth;
            else
            {
                Settings.Tool.sectionWidthMulti = defaultSectionWidth;
                lblVehicleToolWidth.Text = Convert.ToString((int)(numberOfSections * defaultSectionWidth * glm.m2InchOrCm));
            }
            //SectionFeetInchesTotalWidthLabelUpdate();
        }

        private void cboxNumSections_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cboxIsUnique.Checked && numberOfSections != cboxNumSections.SelectedIndex + 1)
            {
                numberOfSections = cboxNumSections.SelectedIndex + 1;

                double wide = nudDefaultSectionWidth.Value;

                if (numberOfSections * wide > 5000)
                {
                    wide = 99;
                    mf.TimedMessageBox(3000, "Too Wide", "Set to 99, " + (Settings.User.isMetric ? "Max 50 Meters" : "Max 164 Feet"));
                    Log.EventWriter("Sections, Tool Set Too Wide");
                }

                nudSection01.Value = wide;
                nudSection02.Value = wide;
                nudSection03.Value = wide;
                nudSection04.Value = wide;
                nudSection05.Value = wide;
                nudSection06.Value = wide;
                nudSection07.Value = wide;
                nudSection08.Value = wide;
                nudSection09.Value = wide;
                nudSection10.Value = wide;
                nudSection11.Value = wide;
                nudSection12.Value = wide;
                nudSection13.Value = wide;
                nudSection14.Value = wide;
                nudSection15.Value = wide;
                nudSection16.Value = wide;

                UpdateSpinners();
            }
        }

        private void NudSection_ValueChanged(object sender, EventArgs e)
        {
            UpdateSpinners();
        }

        private void nudCutoffSpeed_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.slowSpeedCutoff = nudCutoffSpeed.Value;
        }

        private void nudMinCoverage_ValueChanged(object sender, EventArgs e)
        {
            Settings.Tool.minCoverage = (int)nudMinCoverage.Value;
        }

        public void UpdateSpinners()
        {
            int i = numberOfSections;

            double toolWidth = 0;

            foreach (var item in tabTSections.Controls)
            {
                if (item is NudlessNumericUpDown)
                {
                    var item2 = (NudlessNumericUpDown)item;
                    if (item2.Name.Substring(0, 6) == "nudSec")
                    {
                        //grab the number from nudSection01
                        string bob = item2.Name.Substring(10, 2);
                        int nudNum = Convert.ToInt32(bob);
                        if (nudNum <= i)
                        {
                            item2.Enabled = true;
                            item2.Visible = true;
                            toolWidth += item2.Value;

                            if (toolWidth > 5000)
                            {
                                mf.TimedMessageBox(3000, "Too Wide", "Set to 99, " + (Settings.User.isMetric ? "Max 50 Meters" : "Max 164 Feet") );
                                Log.EventWriter("Sections, Tool Set Too Wide");

                                toolWidth = 0;
                                nudSection01.Value = 99;
                                nudSection02.Value = 99;
                                nudSection03.Value = 99;
                                nudSection04.Value = 99;
                                nudSection05.Value = 99;
                                nudSection06.Value = 99;
                                nudSection07.Value = 99;
                                nudSection08.Value = 99;
                                nudSection09.Value = 99;
                                nudSection10.Value = 99;
                                nudSection11.Value = 99;
                                nudSection12.Value = 99;
                                nudSection13.Value = 99;
                                nudSection14.Value = 99;
                                nudSection15.Value = 99;
                                nudSection16.Value = 99;
                            }
                        }
                        else
                        {
                            item2.Enabled = false;
                            item2.Visible = false;
                        }
                    }
                }                
            }

            lblVehicleToolWidth.Text = (toolWidth * glm.m2InchOrCm).ToString("0");

            SectionFeetInchesTotalWidthLabelUpdate();
        }

        //update tool width label at bottom of window
        private void SectionFeetInchesTotalWidthLabelUpdate()
        {
            if (Settings.User.isMetric)
            {
                lblUnits.Text = "Metric";
                lblSecTotalWidthMeters.Text = (Settings.Tool.toolWidth * 100).ToString("0") + " cm";
                lblSummaryWidth.Text = Settings.Tool.toolWidth.ToString("N2") + " m";
            }
            else
            {
                lblUnits.Text = "Imperial";
                double toFeet = (Settings.Tool.toolWidth * glm.m2InchOrCm) * 0.08334;
                double temp = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0);
                lblSecTotalWidthMeters.Text = Convert.ToString((int)toFeet) + "'   " + Convert.ToString(temp) + '"';
                lblSummaryWidth.Text = lblSecTotalWidthMeters.Text;
            }
        }

        //Convert section width to positions along toolbar
        private void CalculateSectionPositions()
        {
            int i = numberOfSections;

            //convert to meters spinner value
            sectionWidthArr[0] = nudSection01.Value;
            sectionWidthArr[1] = nudSection02.Value;
            sectionWidthArr[2] = nudSection03.Value;
            sectionWidthArr[3] = nudSection04.Value;
            sectionWidthArr[4] = nudSection05.Value;
            sectionWidthArr[5] = nudSection06.Value;
            sectionWidthArr[6] = nudSection07.Value;
            sectionWidthArr[7] = nudSection08.Value;
            sectionWidthArr[8] = nudSection09.Value;
            sectionWidthArr[9] = nudSection10.Value;
            sectionWidthArr[10] = nudSection11.Value;
            sectionWidthArr[11] = nudSection12.Value;
            sectionWidthArr[12] = nudSection13.Value;
            sectionWidthArr[13] = nudSection14.Value;
            sectionWidthArr[14] = nudSection15.Value;
            sectionWidthArr[15] = nudSection16.Value;
        }

        #endregion

        #region Switch

        private void tabTSwitches_Enter(object sender, EventArgs e)
        {
            //set accordingly
            chkSelectSteerSwitch.Checked = Settings.Vehicle.setF_isSteerWorkSwitchEnabled;
            chkSelectWorkSwitch.Checked = Settings.Vehicle.setF_isWorkSwitchEnabled;

            if (Settings.Vehicle.setF_isWorkSwitchManualSections)
            {
                chkSetManualSections.Checked = true;
                chkSetAutoSections.Checked = false;
            }
            else
            {
                chkSetManualSections.Checked = false;
                chkSetAutoSections.Checked = true;
            }

            if (Settings.Vehicle.setF_isSteerWorkSwitchManualSections)
            {
                chkSetManualSectionsSteer.Checked = true;
                chkSetAutoSectionsSteer.Checked = false;
            }
            else
            {
                chkSetManualSectionsSteer.Checked = false;
                chkSetAutoSectionsSteer.Checked = true;
            }

            chkSetManualSections.Enabled = chkSetAutoSections.Enabled = chkWorkSwActiveLow.Enabled = chkSelectWorkSwitch.Checked;
            chkSetManualSectionsSteer.Enabled = chkSetAutoSectionsSteer.Enabled = chkSelectSteerSwitch.Checked;


            chkWorkSwActiveLow.Checked = Settings.Vehicle.setF_isWorkSwitchActiveLow;
            if (chkWorkSwActiveLow.Checked) chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveClosed;
            else chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveOpen;
        }

        private void tabTSwitches_Leave(object sender, EventArgs e)
        {
            //active low on work switch
            Settings.Vehicle.setF_isWorkSwitchActiveLow = chkWorkSwActiveLow.Checked;

            //is work switch enabled
            Settings.Vehicle.setF_isWorkSwitchEnabled = chkSelectWorkSwitch.Checked;

            //Are auto or manual sections controlled. 
            Settings.Vehicle.setF_isWorkSwitchManualSections = chkSetManualSections.Checked;

            //Are auto or manual sections controlled for steer
            Settings.Vehicle.setF_isSteerWorkSwitchEnabled = chkSelectSteerSwitch.Checked;

            //does steer switch control manual or auto sections
            Settings.Vehicle.setF_isSteerWorkSwitchManualSections = chkSetManualSectionsSteer.Checked;

            //save
            
        }

        private void chkSelectWorkSwitch_Click(object sender, EventArgs e)
        {
            chkSetManualSections.Enabled = chkSetAutoSections.Enabled= chkWorkSwActiveLow.Enabled = chkSelectWorkSwitch.Checked;
        }

        private void chkSelectSteerSwitch_Click(object sender, EventArgs e)
        {
            chkSetManualSectionsSteer.Enabled = chkSetAutoSectionsSteer.Enabled = chkSelectSteerSwitch.Checked;
        }

        private void chkWorkSwActiveLow_Click(object sender, EventArgs e)
        {
            if (chkWorkSwActiveLow.Checked) chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveClosed;
            else chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveOpen;
        }

        private void chkSetManualSections_Click(object sender, EventArgs e)
        {
            chkSetAutoSections.Checked = false;
            chkSetManualSections.Checked = true;
        }

        private void chkSetAutoSections_Click(object sender, EventArgs e)
        {
            chkSetManualSections.Checked = false;
            chkSetAutoSections.Checked = true;
        }

        private void chkSetAutoSectionsSteer_Click(object sender, EventArgs e)
        {
            chkSetManualSectionsSteer.Checked = false;
            chkSetAutoSectionsSteer.Checked = true;
        }

        private void chkSetManualSectionsSteer_Click(object sender, EventArgs e)
        {
            chkSetAutoSectionsSteer.Checked = false;
            chkSetManualSectionsSteer.Checked = true;
        }
        #endregion
    }
}
