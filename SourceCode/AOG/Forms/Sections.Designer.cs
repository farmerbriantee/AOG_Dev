using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AOG.Properties;
using System.Globalization;
using System.IO;
using System.Media;
using System.Collections.Generic;

namespace AOG
{
    public enum btnStates { Off, Auto, On }

    public partial class FormGPS
    {
        //Off, Manual, and Auto, 3 states possible
        public btnStates workState = btnStates.Off;

        public List<Button> sectionButtons = new List<Button>();
        public List<Label> sectionLbls = new List<Label>();

        public void SetNumOfSectionButtons(int numOfButtons)
        {
            if (sectionButtons.Count > numOfButtons)
            {
                for (int j = sectionButtons.Count - 1; j >= numOfButtons; j--)
                {
                    this.oglMain.Controls.Remove(sectionButtons[j]);
                    this.oglMain.Controls.Remove(sectionLbls[j]);
                    sectionButtons.RemoveAt(j);
                    sectionLbls.RemoveAt(j);
                }
                SetSectionButtonPositions();
            }
            else if (sectionButtons.Count < numOfButtons)
            {
                for (int j = sectionButtons.Count; j < numOfButtons; j++)
                {
                    var btn = new Button();
                    btn.Click += Butt_Click;
                    btn.Text = (j + 1).ToString();
                    this.oglMain.Controls.Add(btn);
                    btn.BringToFront();
                    btn.Visible = isJobStarted;

                    if (Settings.User.setDisplay_isDayMode)
                    {
                        btn.ForeColor = Color.Black;
                        btn.BackColor = Color.Red;
                    }
                    else
                    {
                        btn.BackColor = Color.Crimson;
                        btn.ForeColor = Color.White;
                    }

                    btn.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
                    btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btn.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
                    btn.Size = new System.Drawing.Size(34, 25);
                    btn.UseVisualStyleBackColor = false;
                    btn.Anchor = AnchorStyles.Bottom;
                    sectionButtons.Add(btn);

                    //labels
                    var lbl = new Label();
                    this.oglMain.Controls.Add(lbl);
                    lbl.BringToFront();
                    lbl.Visible = isJobStarted;

                    if (Settings.User.setDisplay_isDayMode)
                    {
                        lbl.ForeColor = Color.Black;
                        lbl.BackColor = Color.Red;
                    }
                    else
                    {
                        lbl.BackColor = Color.Crimson;
                        lbl.ForeColor = Color.White;
                    }

                    //lbl.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
                    //btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    //btn.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    //btn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
                    lbl.Size = new System.Drawing.Size(34, 25);
                    lbl.BorderStyle = BorderStyle.FixedSingle;
                    //btn.UseVisualStyleBackColor = false;
                    lbl.Anchor = AnchorStyles.Bottom;
                    sectionLbls.Add(lbl);
                }

                SetSectionButtonPositions();
            }
        }

        public void SetSectionButtonPositions()
        {
            if (sectionButtons.Count == 0) return;
            int top = oglMain.Height - (panelSim.Visible ? 100 : 40);

            int oglButtonWidth = oglMain.Width * 3 / 4;
            int oglCenter = oglMain.Width / 2;

            int buttonMaxWidth = 360, buttonHeight = 35;
            int buttonWidth = oglButtonWidth / sectionButtons.Count;
            if (buttonWidth > buttonMaxWidth) buttonWidth = buttonMaxWidth;

            int Left = oglCenter - (sectionButtons.Count * buttonWidth) / 2;

            for (int j = 0; j < sectionButtons.Count; j++)
            {
                sectionButtons[j].Top = top;
                sectionLbls[j].Top = top-18;
                sectionButtons[j].Size = new System.Drawing.Size(buttonWidth, buttonHeight);
                sectionLbls[j].Size = new System.Drawing.Size(buttonWidth, buttonHeight/2);

                sectionButtons[j].Left = Left;
                sectionLbls[j].Left = Left;
                Left += buttonWidth;
            }
        }

        public void SetSectionButtonVisible(bool visible)
        {
            for (int j = 0; j < sectionButtons.Count; j++)
            {
                sectionButtons[j].Visible = visible;
                sectionLbls[j].Visible = visible;
            }
        }

        private void Butt_Click(object sender, EventArgs e)
        {
            if (sender is Button butt && int.TryParse(butt.Text, out int val))
            {
                if (Settings.Tool.isSectionsNotZones)
                {
                    btnStates state = GetNextState(section[val - 1].sectionBtnState);
                    IndividualSectionAndButonToState(state, val - 1, butt);
                }
                else
                {
                    if (tool.zoneRanges[val] != 0)//???
                    {
                        btnStates state = GetNextState(section[tool.zoneRanges[val] - 1].sectionBtnState);
                        IndividualZoneAndButtonToState(state, val == 1 ? 0 : tool.zoneRanges[val - 1], tool.zoneRanges[val], butt);
                    }
                }
            }
        }

        //Section Manual and Auto buttons on right side
        private void btnSectionMasterManual_Click(object sender, EventArgs e)
        {
            SetWorkState(workState == btnStates.On ? btnStates.Off : btnStates.On);
            //System.Media.SystemSounds.Asterisk.Play();
            if (Settings.User.sound_isSectionsOn) sounds.sndSectionOff.Play();
        }

        private void btnSectionMasterAuto_Click(object sender, EventArgs e)
        {
            SetWorkState(workState == btnStates.Auto ? btnStates.Off : btnStates.Auto);

            if (Settings.User.sound_isSectionsOn) sounds.sndSectionOn.Play();
        }

        public void SetWorkState(btnStates state)
        {
            if (!isJobStarted) state = btnStates.Off;

            if (state != workState)
            {
                workState = state;

                btnSectionMasterManual.Image = state == btnStates.On ? Properties.Resources.ManualOn : Properties.Resources.ManualOff;
                btnSectionMasterAuto.Image = state == btnStates.Auto ? Properties.Resources.SectionMasterOn : Properties.Resources.SectionMasterOff;


                //go set the butons and section states
                if (Settings.Tool.isSectionsNotZones)
                    AllSectionsAndButtonsToState(workState);
                else
                    AllZonesAndButtonsToState(workState);
            }
        }

        //cycle thru states - Off,Auto,On
        private btnStates GetNextState(btnStates state)
        {
            if (state == btnStates.Off) return btnStates.Auto;
            else if (state == btnStates.Auto) return btnStates.On;
            else return btnStates.Off;
        }

        //cycle thru states - Off,Auto,On
        private btnStates GetPrevState(btnStates state)
        {
            if (state == btnStates.Off) return btnStates.On;
            else if (state == btnStates.Auto) return btnStates.Off;
            else return btnStates.Auto;
        }

        //Section buttons************************8
        public void AllSectionsAndButtonsToState(btnStates state)
        {
            for (int j = 0; j < sectionButtons.Count; j++)
            {
                IndividualSectionAndButonToState(state, j, sectionButtons[j]);
            }
        }

        private void IndividualSectionAndButonToState(btnStates state, int sectNumber, Button btn)
        {
            section[sectNumber].sectionBtnState = state;

            SetSectionButtonColor(state, btn);
        }

        //Zone buttons ************************************
        public void AllZonesAndButtonsToState(btnStates state)
        {
            for (int j = 0; j < sectionButtons.Count; j++)
            {
                if (int.TryParse(sectionButtons[j].Text, out int val))
                {
                    if (tool.zoneRanges[val] != 0)//???
                    {
                        IndividualZoneAndButtonToState(state, val == 1 ? 0 : tool.zoneRanges[val - 1], tool.zoneRanges[val], sectionButtons[j]);
                    }
                }
            }
        }

        private void IndividualZoneAndButtonToState(btnStates state, int sectionStartNumber, int sectionEndNumber, Button btn)
        {
            for (int i = sectionStartNumber; i < sectionEndNumber; i++)
            {
                section[i].sectionBtnState = state;
            }

            SetSectionButtonColor(state, btn);
        }

        private void SetSectionButtonColor(btnStates state, Button btn)
        {
            btn.ForeColor = Settings.User.setDisplay_isDayMode ? Color.Black : Color.White;

            //update zone buttons
            switch (state)
            {
                case btnStates.Auto:
                    btn.BackColor = Settings.User.setDisplay_isDayMode ? Color.Lime : Color.ForestGreen;
                    break;

                case btnStates.On:
                    btn.BackColor = Settings.User.setDisplay_isDayMode ? Color.Yellow : Color.DarkGoldenrod;
                    break;

                case btnStates.Off:
                    btn.BackColor = Settings.User.setDisplay_isDayMode ? Color.Red : Color.Crimson;
                    break;
            }
        }

        public void TurnOffSectionsSafely()
        {
            SetWorkState(btnStates.Off);

            //turn off all the sections
            for (int j = 0; j < tool.numOfSections; j++)
            {
                section[j].isSectionOn = false; ;
                section[j].sectionOffRequest = true;
                section[j].sectionOnRequest = false;
                section[j].sectionOffTimer = 0;
                section[j].isMappingOn = false;
                section[j].mappingOnTimer = 0;
                section[j].mappingOffTimer = 0;
            }

            //turn off patching
            foreach (var patch in triStrip)
            {
                if (patch.isDrawing) patch.TurnMappingOff();
            }
        }

        //function to set section positions
        public void SectionSetPosition()
        {
            if (Settings.Tool.isSectionsNotZones)
            {
                int count = tool.numOfSections;
                double position = 0;
                for (int j = 0; j < count; j++)
                {
                    position += Settings.Tool.setSection_Widths[j];
                }

                position *= -0.5;
                position += Settings.Tool.offset;

                for (int j = 0; j < count; j++)
                {
                    section[j].positionLeft = position;
                    position += Settings.Tool.setSection_Widths[j];
                    section[j].positionRight = position;

                    section[j].sectionWidth = (section[j].positionRight - section[j].positionLeft);
                    section[j].rpSectionPosition = 250 + (int)(Math.Round(section[j].positionLeft * 10, 0, MidpointRounding.AwayFromZero));
                    section[j].rpSectionWidth = (int)(Math.Round(section[j].sectionWidth * 10, 0, MidpointRounding.AwayFromZero));
                }

                //update the widths of sections and tool width in main
                //Calculate total width and each section width
                //calculate tool width based on extreme right and left values
                Settings.Tool.toolWidth = (section[tool.numOfSections - 1].positionRight) - (section[0].positionLeft);
            }
            else
            {
                double position = (Settings.Tool.toolWidth * -0.5) + Settings.Tool.offset;

                double defaultSectionWidth = Settings.Tool.sectionWidthMulti;

                for (int i = 0; i < tool.numOfSections; i++)
                {
                    section[i].positionLeft = position;
                    position += defaultSectionWidth;
                    section[i].positionRight = position;
                    section[i].sectionWidth = defaultSectionWidth;
                    section[i].rpSectionPosition = 250 + (int)(Math.Round(section[i].positionLeft * 10, 0, MidpointRounding.AwayFromZero));
                    section[i].rpSectionWidth = (int)(Math.Round(section[i].sectionWidth * 10, 0, MidpointRounding.AwayFromZero));
                }
            }

            //left and right tool position
            tool.farLeftPosition = section[0].positionLeft;
            tool.farRightPosition = section[tool.numOfSections - 1].positionRight;

            //find the right side pixel position
            tool.rpXPosition = 250 + (int)(Math.Round(tool.farLeftPosition * 10, 0, MidpointRounding.AwayFromZero));
            tool.rpWidth = (int)(Math.Round(Settings.Tool.toolWidth * 10, 0, MidpointRounding.AwayFromZero));
        }

        private void BuildMachineByte()
        {
            if (Settings.Tool.isSectionsNotZones)
            {
                PGN_254.pgn[PGN_254.sc1to8] = 0;
                PGN_254.pgn[PGN_254.sc9to16] = 0;

                int number = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (section[j].isSectionOn)
                        number |= 1 << j;
                }
                PGN_254.pgn[PGN_254.sc1to8] = unchecked((byte)number);
                number = 0;

                for (int j = 8; j < 16; j++)
                {
                    if (section[j].isSectionOn)
                        number |= 1 << (j-8);
                }
                PGN_254.pgn[PGN_254.sc9to16] = unchecked((byte)number);

                //machine pgn
                PGN_239.pgn[PGN_239.sc1to8] = PGN_254.pgn[PGN_254.sc1to8];
                PGN_239.pgn[PGN_239.sc9to16] = PGN_254.pgn[PGN_254.sc9to16];
                PGN_229.pgn[PGN_229.sc1to8] = PGN_254.pgn[PGN_254.sc1to8];
                PGN_229.pgn[PGN_229.sc9to16] = PGN_254.pgn[PGN_254.sc9to16];
                PGN_229.pgn[PGN_229.toolLSpeed] = unchecked((byte)(tool.farLeftSpeed * 10));
                PGN_229.pgn[PGN_229.toolRSpeed] = unchecked((byte)(tool.farRightSpeed * 10));
            }
            else
            {
                //zero all the bytes - set only if on
                for (int i = 5; i < 13; i++)
                {
                    PGN_229.pgn[i] = 0;
                }

                int number = 0;
                for (int k = 0; k < 8; k++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (section[j + k * 8].isSectionOn)
                            number |= 1 << j;
                    }
                    PGN_229.pgn[5 + k] = unchecked((byte)number);
                    number = 0;
                }

                //tool speed to calc ramp
                PGN_229.pgn[PGN_229.toolLSpeed] = unchecked((byte)(tool.farLeftSpeed * 10));
                PGN_229.pgn[PGN_229.toolRSpeed] = unchecked((byte)(tool.farRightSpeed * 10));

                PGN_239.pgn[PGN_239.sc1to8] = PGN_229.pgn[PGN_229.sc1to8];
                PGN_239.pgn[PGN_239.sc9to16] = PGN_229.pgn[PGN_229.sc9to16];

                PGN_254.pgn[PGN_254.sc1to8] = PGN_229.pgn[PGN_229.sc1to8];
                PGN_254.pgn[PGN_254.sc9to16] = PGN_229.pgn[PGN_229.sc9to16];

            }

            PGN_239.pgn[PGN_239.speed] = unchecked((byte)(avgSpeed * 10));
            PGN_239.pgn[PGN_239.tram] = unchecked((byte)tram.controlByte);

            SendPgnToLoop(PGN_229.pgn);
            SendPgnToLoop(PGN_239.pgn);
        }

        private void DoRemoteSwitches()
        {
            //MTZ8302 Feb 2020 
            if (isFieldStarted)
            {
                //MainSW was used
                if (mc.ss[mc.swMain] != mc.ssP[mc.swMain])
                {
                    //Main SW pressed
                    if ((mc.ss[mc.swMain] & 1) == 1)
                    {
                        SetWorkState(btnStates.Auto);
                    } // if Main SW ON

                    //if Main SW in Arduino is pressed OFF
                    if ((mc.ss[mc.swMain] & 2) == 2)
                    {
                        SetWorkState(btnStates.Off);
                    } // if Main SW OFF

                    mc.ssP[mc.swMain] = mc.ss[mc.swMain];
                }  //Main or shpList SW

                int Bit;

                if (Settings.Tool.isSectionsNotZones)
                {
                    #region NoZones
                    if (mc.ss[mc.swOnGr0] != 0)
                    {
                        // ON Signal from Arduino 
                        RemoteClickButtons(btnStates.On, 0, mc.ss[mc.swOnGr0]);
                        mc.ssP[mc.swOnGr0] = mc.ss[mc.swOnGr0];
                    } //if swONLo != 0 
                    else { if (mc.ssP[mc.swOnGr0] != 0) { mc.ssP[mc.swOnGr0] = 0; } }

                    if (mc.ss[mc.swOnGr1] != 0)
                    {
                        // ON Signal from Arduino 
                        RemoteClickButtons(btnStates.On, 8, mc.ss[mc.swOnGr1]);
                        mc.ssP[mc.swOnGr1] = mc.ss[mc.swOnGr1];
                    } //if swONHi != 0   
                    else { if (mc.ssP[mc.swOnGr1] != 0) { mc.ssP[mc.swOnGr1] = 0; } }

                    // Switches have changed
                    if (mc.ss[mc.swOffGr0] != mc.ssP[mc.swOffGr0])
                    {
                        //if Main = Auto then change section to Auto if Off signal from Arduino stopped
                        if (workState == btnStates.Auto)
                        {
                            RemoteClickButtons2(0, mc.ssP[mc.swOffGr0], mc.ss[mc.swOffGr0]);
                        }
                        mc.ssP[mc.swOffGr0] = mc.ss[mc.swOffGr0];
                    }

                    if (mc.ss[mc.swOffGr1] != mc.ssP[mc.swOffGr1])
                    {
                        //if Main = Auto then change section to Auto if Off signal from Arduino stopped
                        if (workState == btnStates.Auto)
                        {
                            RemoteClickButtons2(8, mc.ssP[mc.swOffGr1], mc.ss[mc.swOffGr1]);
                        }
                        mc.ssP[mc.swOffGr1] = mc.ss[mc.swOffGr1];
                    }

                    // OFF Signal from Arduino
                    if (mc.ss[mc.swOffGr0] != 0)
                    {
                        //if section SW in Arduino is switched to OFF; check always, if switch is locked to off GUI should not change
                        RemoteClickButtons(btnStates.Off, 0, mc.ss[mc.swOffGr0]);
                    } // if swOFFLo !=0
                    if (mc.ss[mc.swOffGr1] != 0)
                    {
                        //if section SW in Arduino is switched to OFF; check always, if switch is locked to off GUI should not change
                        RemoteClickButtons(btnStates.Off, 8, mc.ss[mc.swOffGr1]);
                    } // if swOFFHi !=0
                    #endregion
                }
                else
                {
                    // zones to on
                    if (mc.ss[mc.swOnGr0] != 0)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Bit = (int)Math.Pow(2, i);
                            if ((tool.zoneRanges[i + 1] > 0) && ((mc.ss[mc.swOnGr0] & Bit) == Bit))
                            {
                                if (section[tool.zoneRanges[i + 1] - 1].sectionBtnState != btnStates.Auto) section[tool.zoneRanges[i + 1] - 1].sectionBtnState = btnStates.Auto;
                                sectionButtons[i].PerformClick();
                            }
                        }

                        mc.ssP[mc.swOnGr0] = mc.ss[mc.swOnGr0];
                    }
                    else { if (mc.ssP[mc.swOnGr0] != 0) { mc.ssP[mc.swOnGr0] = 0; } }

                    // zones to auto
                    if (mc.ss[mc.swOffGr0] != mc.ssP[mc.swOffGr0])
                    {
                        if (workState == btnStates.Auto)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                Bit = (int)Math.Pow(2, i);
                                if ((tool.zoneRanges[i + 1] > 0) && ((mc.ssP[mc.swOffGr0] & Bit) == Bit)
                                    && ((mc.ss[mc.swOffGr0] & Bit) != Bit) && (section[tool.zoneRanges[i + 1] - 1].sectionBtnState == btnStates.Off))
                                {
                                    sectionButtons[i].PerformClick();
                                }
                            }
                        }
                        mc.ssP[mc.swOffGr0] = mc.ss[mc.swOffGr0];
                    }

                    // zones to off
                    if (mc.ss[mc.swOffGr0] != 0)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Bit = (int)Math.Pow(2, i);
                            if ((tool.zoneRanges[i + 1] > 0) && ((mc.ss[mc.swOffGr0] & Bit) == Bit) && (section[tool.zoneRanges[i + 1] - 1].sectionBtnState != btnStates.Off))
                            {
                                section[tool.zoneRanges[i + 1] - 1].sectionBtnState = btnStates.On;

                                sectionButtons[i].PerformClick();
                            }
                        }
                    }
                }
            }//if serial or udp port open
        }

        private void RemoteClickButtons(btnStates state,  int offset, byte value)
        {
            for (int i = 0; i < 8; i++)
            {
                byte Bit = (byte)Math.Pow(2, i);
                if ((value & Bit) == Bit && section[offset + i].sectionBtnState != state)
                {
                    section[offset + i].sectionBtnState = GetPrevState(state);
                    sectionButtons[offset + i].PerformClick();
                }
            }
        }

        private void RemoteClickButtons2(int offset, byte value, byte value2)
        {
            for (int i = 0; i < 8; i++)
            {
                byte Bit = (byte)Math.Pow(2, i);
                if ((value & Bit) == Bit && (value2 & Bit) != Bit && section[offset + i].sectionBtnState == btnStates.Off)
                {
                    section[offset + i].sectionBtnState = GetPrevState(btnStates.Auto);
                    sectionButtons[offset + i].PerformClick();
                }
            }
        }
    }
}
