using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AOG
{
    public enum btnStates { Off, Auto, On }

    public partial class FormGPS
    {
        //Off, Manual, and Auto, 3 states possible
        public btnStates workState = btnStates.Off;

        public List<SectionButton> controlButtons = new List<SectionButton>();
        public List<Label> controlLbls = new List<Label>();

        /// <summary>
        /// list of sections
        /// </summary>
        public List<CSection> section = new List<CSection>();

        public void SetNumOfControlButtons(int numOfButtons, int numOfSections)
        {
            if (controlButtons.Count > numOfButtons)
            {
                for (int j = controlButtons.Count - 1; j >= numOfButtons; j--)
                {
                    this.oglMain.Controls.Remove(controlButtons[j]);
                    controlButtons.RemoveAt(j);
                }
                SetControlButtonPositions();
            }
            else if (controlButtons.Count < numOfButtons)
            {
                for (int j = controlButtons.Count; j < numOfButtons; j++)
                {
                    var btn = new SectionButton();
                    btn.Click += Butt_Click;
                    btn.Text = (j + 1).ToString();
                    btn.Index = j + 1;
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
                    controlButtons.Add(btn);
                }

                SetControlButtonPositions();
            }


            if (controlLbls.Count > numOfSections)
            {
                for (int j = controlLbls.Count - 1; j >= numOfSections; j--)
                {
                    this.oglMain.Controls.Remove(controlLbls[j]);
                    controlLbls.RemoveAt(j);
                }
                SetControlLabelPositions();
            }
            else if (controlLbls.Count < numOfSections)
            {
                for (int j = controlLbls.Count; j < numOfSections; j++)
                {
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

                    lbl.Size = new System.Drawing.Size(34, 25);
                    lbl.BorderStyle = BorderStyle.FixedSingle;
                    lbl.Anchor = AnchorStyles.Bottom;
                    controlLbls.Add(lbl);
                }

                SetControlLabelPositions();
            }
        }

        public void SetControlButtonPositions()
        {
            if (controlButtons.Count == 0) return;
            int top = oglMain.Height - (panelSim.Visible ? 100 : 40);

            int oglButtonWidth = oglMain.Width * 3 / 4;
            int oglCenter = oglMain.Width / 2;

            int buttonMaxWidth = 360, buttonHeight = 35;
            int buttonWidth = oglButtonWidth / controlButtons.Count;
            if (buttonWidth > buttonMaxWidth) buttonWidth = buttonMaxWidth;

            int Left = oglCenter - (controlButtons.Count * buttonWidth) / 2;

            for (int j = 0; j < controlButtons.Count; j++)
            {
                controlButtons[j].Top = top;
                controlButtons[j].Size = new System.Drawing.Size(buttonWidth, buttonHeight);

                controlButtons[j].Left = Left;
                Left += buttonWidth;
            }
        }
        public void SetControlLabelPositions()
        {
            if (controlLbls.Count == 0) return;
            int top = oglMain.Height - (panelSim.Visible ? 118 : 58);

            int oglButtonWidth = oglMain.Width * 3 / 4;
            int oglCenter = oglMain.Width / 2;

            int buttonMaxWidth = 360, buttonHeight = 35;
            int buttonWidth = oglButtonWidth / controlLbls.Count;
            if (buttonWidth > buttonMaxWidth) buttonWidth = buttonMaxWidth;

            int Left = oglCenter - (controlLbls.Count * buttonWidth) / 2;

            for (int j = 0; j < controlLbls.Count; j++)
            {
                controlLbls[j].Top = top;
                controlLbls[j].Size = new System.Drawing.Size(buttonWidth, buttonHeight / 2);

                controlLbls[j].Left = Left;
                Left += buttonWidth;
            }
        }

        public void SetSectionButtonVisible(bool visible)
        {
            for (int j = 0; j < controlButtons.Count; j++)
            {
                controlButtons[j].Visible = visible;
            }
            for (int j = 0; j < controlLbls.Count; j++)
            {
                controlLbls[j].Visible = visible;
            }
        }

        private void Butt_Click(object sender, EventArgs e)
        {
            if (sender is SectionButton butt)
            {
                int val = butt.Index;
                btnStates state = GetNextState(butt.state);

                if (Settings.Tool.isSectionsNotZones)
                {
                    IndividualZoneAndButtonToState(state, val - 1, val, butt);
                }
                else if (tool.zoneRanges[val] != 0)
                {
                    IndividualZoneAndButtonToState(state, val == 1 ? 0 : tool.zoneRanges[val - 1], tool.zoneRanges[val], butt);
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

        //Zone buttons ************************************
        public void AllZonesAndButtonsToState(btnStates state)
        {
            for (int j = 0; j < controlButtons.Count; j++)
            {
                if (Settings.Tool.isSectionsNotZones)
                {
                    IndividualZoneAndButtonToState(state, j, j + 1, controlButtons[j]);
                }
                else
                {
                    IndividualZoneAndButtonToState(state, j == 0 ? 0 : tool.zoneRanges[j], tool.zoneRanges[j + 1], controlButtons[j]);
                }
            }
        }

        private void IndividualZoneAndButtonToState(btnStates state, int sectionStartNumber, int sectionEndNumber, SectionButton btn)
        {
            for (int i = sectionStartNumber; i < sectionEndNumber; i++)
            {
                if (i < section.Count)
                    section[i].sectionBtnState = state;
            }
            btn.state = state;
            btn.ForeColor = Settings.User.setDisplay_isDayMode ? Color.Black : Color.White;

            //set control button color
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
            for (int j = 0; j < section.Count; j++)
            {
                section[j].isSectionOn = false;
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
            int sectionsCount = Settings.Tool.isSectionsNotZones ? Settings.Tool.numSections : Settings.Tool.numSectionsMulti;

            if (section.Count > sectionsCount)
            {
                for (int j = section.Count - 1; j >= sectionsCount; j--)
                {
                    section.RemoveAt(j);
                }
            }
            else if (section.Count < sectionsCount)
            {
                for (int j = section.Count; j < sectionsCount; j++)
                {
                    section.Add(new CSection());
                }
            }

            if (Settings.Tool.isSectionsNotZones)
            {
                int count = section.Count;
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
                Settings.Tool.toolWidth = (section[count - 1].positionRight) - (section[0].positionLeft);
            }
            else
            {
                double position = (Settings.Tool.toolWidth * -0.5) + Settings.Tool.offset;

                double defaultSectionWidth = Settings.Tool.sectionWidthMulti;

                for (int i = 0; i < section.Count; i++)
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
            if (section.Count > 0)
            {
                tool.farLeftPosition = section[0].positionLeft;
                tool.farRightPosition = section[section.Count - 1].positionRight;
            }

            //find the right side pixel position
            tool.rpXPosition = 250 + (int)(Math.Round(tool.farLeftPosition * 10, 0, MidpointRounding.AwayFromZero));
            tool.rpWidth = (int)(Math.Round(Settings.Tool.toolWidth * 10, 0, MidpointRounding.AwayFromZero));
        }

        private void BuildMachineByte()
        {
            for (int k = 0; k < 8; k++)
            {
                int number = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (j + k * 8 < section.Count && section[j + k * 8].isSectionOn)
                        number |= 1 << j;
                }
                PGN_229.pgn[5 + k] = unchecked((byte)number);
            }

            //tool speed to calc ramp
            PGN_229.pgn[PGN_229.toolLSpeed] = unchecked((byte)(tool.farLeftSpeed * 10));
            PGN_229.pgn[PGN_229.toolRSpeed] = unchecked((byte)(tool.farRightSpeed * 10));

            PGN_239.pgn[PGN_239.speed] = unchecked((byte)(avgSpeed * 10));
            PGN_239.pgn[PGN_239.tram] = unchecked((byte)tram.controlByte);
            PGN_239.pgn[PGN_239.sc1to8] = PGN_229.pgn[PGN_229.sc1to8];
            PGN_239.pgn[PGN_239.sc9to16] = PGN_229.pgn[PGN_229.sc9to16];

            PGN_254.pgn[PGN_254.sc1to8] = PGN_229.pgn[PGN_229.sc1to8];
            PGN_254.pgn[PGN_254.sc9to16] = PGN_229.pgn[PGN_229.sc9to16];

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

                if (mc.ss[mc.swOnGr0] != mc.ssP[mc.swOnGr0])
                {
                    // ON Signal from Arduino
                    RemoteClickButtons(btnStates.On, 0, mc.ss[mc.swOnGr0]);
                    mc.ssP[mc.swOnGr0] = mc.ss[mc.swOnGr0];
                }

                if (mc.ss[mc.swOnGr1] != mc.ssP[mc.swOnGr1])
                {
                    // ON Signal from Arduino
                    RemoteClickButtons(btnStates.On, 8, mc.ss[mc.swOnGr1]);
                    mc.ssP[mc.swOnGr1] = mc.ss[mc.swOnGr1];
                }

                // Switches have changed
                if (mc.ss[mc.swOffGr0] != mc.ssP[mc.swOffGr0])
                {
                    //if Main = Auto then change section to Auto if Off signal from Arduino stopped
                    if (workState == btnStates.Auto)
                    {
                        RemoteClickButtons2(0, mc.ssP[mc.swOffGr0], mc.ss[mc.swOffGr0]);
                    }

                    //if section SW in Arduino is switched to OFF; check always, if switch is locked to off GUI should not change
                    RemoteClickButtons(btnStates.Off, 0, mc.ss[mc.swOffGr0]);
                    mc.ssP[mc.swOffGr0] = mc.ss[mc.swOffGr0];
                }

                if (mc.ss[mc.swOffGr1] != mc.ssP[mc.swOffGr1])
                {
                    //if Main = Auto then change section to Auto if Off signal from Arduino stopped
                    if (workState == btnStates.Auto)
                    {
                        RemoteClickButtons2(8, mc.ssP[mc.swOffGr1], mc.ss[mc.swOffGr1]);
                    }

                    //if section SW in Arduino is switched to OFF; check always, if switch is locked to off GUI should not change
                    RemoteClickButtons(btnStates.Off, 8, mc.ss[mc.swOffGr1]);

                    mc.ssP[mc.swOffGr1] = mc.ss[mc.swOffGr1];
                }
            }
        }

        private void RemoteClickButtons(btnStates state, int offset, byte value)
        {
            for (int i = 0; i < 8; i++)
            {
                byte Bit = (byte)Math.Pow(2, i);

                if ((value & Bit) == Bit && offset + i < controlButtons.Count && controlButtons[offset + i].state != state)
                {
                    controlButtons[offset + i].state = GetPrevState(state);
                    controlButtons[offset + i].PerformClick();
                }
            }
        }

        private void RemoteClickButtons2(int offset, byte value, byte value2)
        {
            for (int i = 0; i < 8; i++)
            {
                byte Bit = (byte)Math.Pow(2, i);

                if ((value & Bit) == Bit && (value2 & Bit) != Bit && offset + i < controlButtons.Count && controlButtons[offset + i].state == btnStates.Off)
                {
                    controlButtons[offset + i].state = GetPrevState(btnStates.Auto);
                    controlButtons[offset + i].PerformClick();
                }
            }
        }
    }
    public class SectionButton : Button
    {
        public btnStates state = btnStates.Off;
        public int Index = 0;
        public SectionButton()
        {
        }
    }
}