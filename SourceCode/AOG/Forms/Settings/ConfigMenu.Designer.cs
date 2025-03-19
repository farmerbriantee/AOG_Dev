using AOG.Classes;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormConfig
    {
        private TabPage oldTabPage = null;
        private Button oldbutton = null;
        private Panel oldPanel = null;


        #region menu Buttons

        private void btnVehicle_Click(object sender, EventArgs e)
        {
            SetTab(btnSubVehicleType, tabVConfig, true, panelVehicleSubMenu);
        }

        private void btnTool_Click(object sender, EventArgs e)
        {
            SetTab(btnSubToolType, tabTConfig, true, panelToolSubMenu);
        }

        private void btnDataSources_Click(object sender, EventArgs e)
        {
            SetTab(btnSubHeading, tabDHeading, true, panelDataSourcesSubMenu);
        }
        private void btnUTurn_Click(object sender, EventArgs e)
        {
            SetTab(btnUTurn, tabUTurn, true);
        }

        private void btnArduino_Click(object sender, EventArgs e)
        {
            SetTab(btnMachineModule, tabAMachine, true, panelArduinoSubMenu);
        }

        private void btnTram_Click(object sender, EventArgs e)
        {
            SetTab(btnTram, tabTram, true);
        }

        private void btnFeatureHides_Click(object sender, EventArgs e)
        {
            SetTab(btnFeatureHides, tabBtns, true);
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            SetTab(btnDisplay, tabDisplay, true);
        }
        #endregion

        #region Vehicle Sub Menu Btns
        private void btnSubVehicleType_Click(object sender, EventArgs e)
        {
            SetTab(btnSubVehicleType, tabVConfig, false);
        }

        private void btnSubDimensions_Click(object sender, EventArgs e)
        {
            SetTab(btnSubDimensions, tabVDimensions, false);
        }

        private void btnSubAntenna_Click(object sender, EventArgs e)
        {
            SetTab(btnSubAntenna, tabVAntenna, false);
        }

        #endregion Region

        #region Tool Sub Menu

        private void btnSubToolType_Click(object sender, EventArgs e)
        {
            SetTab(btnSubToolType, tabTConfig, false);
        }

        private void btnSubHitch_Click(object sender, EventArgs e)
        {
            SetTab(btnSubHitch, tabTHitch, false);
        }

        private void btnSubToolOffset_Click (object sender, EventArgs e)
        {
            SetTab(btnSubToolOffset, tabToolOffset, false);
        }

        private void btnSubPivot_Click(object sender, EventArgs e)
        {
            SetTab(btnSubPivot, tabToolPivot, false);
        }

        private void btnSubSections_Click(object sender, EventArgs e)
        {
            SetTab(btnSubSections, tabTSections, false);
        }

        private void btnSubSwitches_Click(object sender, EventArgs e)
        {
            SetTab(btnSubSwitches, tabTSwitches, false);
        }

        private void btnSubToolSettings_Click(object sender, EventArgs e)
        {
            SetTab(btnSubToolSettings, tabTSettings, false);
        }
        #endregion

        #region SubMenu Data Sources


        private void btnSubHeading_Click(object sender, EventArgs e)
        {
            SetTab(btnSubHeading, tabDHeading, false);
        }

        private void btnSubRoll_Click(object sender, EventArgs e)
        {
            SetTab(btnSubRoll, tabDRoll, false);
        }

        #endregion

        #region Module

        private void btnMachineModule_Click(object sender, EventArgs e)
        {
            SetTab(btnMachineModule, tabAMachine, false);
        }

        private void btnMachineRelay_Click(object sender, EventArgs e)
        {
            SetTab(btnMachineRelay, tabRelay, false);
        }

        private void SetTab(Button btn, TabPage newTabPage, bool back, Panel panel = null)
        {
            if (back && oldPanel != null)
                oldPanel.Visible = false;

            if (panel != null && panel == oldPanel)
            {
                if (oldbutton != null)
                    oldbutton.BackColor = SystemColors.GradientInactiveCaption;

                tab1.SelectedTab = tabSummary;
                oldPanel = null;
                SelectedTabChanged();
            }
            else if (oldTabPage != newTabPage)
            {
                tab1.SelectedTab = newTabPage;
                if (panel != null)
                {
                    panel.Visible = true;
                    oldPanel = panel;
                }
                if (oldbutton != null)
                    oldbutton.BackColor = SystemColors.GradientInactiveCaption;

                btn.BackColor = SystemColors.GradientActiveCaption;
                oldbutton = btn;
                SelectedTabChanged();
            }
            else if (back)
            {
                if (oldbutton != null)
                    oldbutton.BackColor = SystemColors.GradientInactiveCaption;

                tab1.SelectedTab = tabSummary;
                SelectedTabChanged();
            }
        }

        private void SelectedTabChanged()
        {
            //this is what we actually want
            //The Enter event is raised when the tab page gains focus,
            //but focus can behave unpredictably.
            TabPage newTabPage = tab1.SelectedTab;
            if (newTabPage != oldTabPage)
            {
                if (oldTabPage != null)
                {
                    if (oldTabPage == tabSummary)
                    {
                        tabSummary_Leave(null, null);
                    }
                    else if (oldTabPage == tabDHeading)
                    {
                        tabDHeading_Leave(null, null);
                    }
                    else if (oldTabPage == tabVConfig)
                    {
                        tabVConfig_Leave(null, null);
                    }
                    else if (oldTabPage == tabDisplay)
                    {
                        tabDisplay_Leave(null, null);
                    }
                    else if (oldTabPage == tabBtns)
                    {
                        tabBtns_Leave(null, null);
                    }
                    else if (oldTabPage == tabTram)
                    {
                        tabTram_Leave(null, null);
                    }
                    else if (oldTabPage == tabAMachine)
                    {
                        tabAMachine_Leave(null, null);
                    }
                    else if (oldTabPage == tabRelay)
                    {
                        tabRelay_Leave(null, null);
                    }
                    else if (oldTabPage == tabUTurn)
                    {
                        tabUTurn_Leave(null, null);
                    }
                    else if (oldTabPage == tabDRoll)
                    {
                        tabDRoll_Leave(null, null);
                    }
                    else if (oldTabPage == tabTSettings)
                    {
                        tabTSettings_Leave(null, null);
                    }
                    else if (oldTabPage == tabTSwitches)
                    {
                        tabTSwitches_Leave(null, null);
                    }
                    else if (oldTabPage == tabTSections)
                    {
                        tabTSections_Leave(null, null);
                    }
                    else if (oldTabPage == tabToolPivot)
                    {
                        tabToolPivot_Leave(null, null);
                    }
                    else if (oldTabPage == tabToolOffset)
                    {
                        tabToolOffset_Leave(null, null);
                    }
                    else if (oldTabPage == tabTHitch)
                    {
                        tabTHitch_Leave(null, null);
                    }
                    else if (oldTabPage == tabTConfig)
                    {
                        tabTConfig_Leave(null, null);
                    }
                    else if (oldTabPage == tabVDimensions)
                    {
                        tabVDimensions_Leave(null, null);
                    }
                    else if (oldTabPage == tabVAntenna)
                    {
                        tabVAntenna_Leave(null, null);
                    }
                    else
                    {
                        throw new Exception($"Unknown tab left: {oldTabPage.Text}");
                    }
                }

                if (newTabPage != null)
                {
                    if (newTabPage == tabSummary)
                    {
                        tabSummary_Enter(null, null);
                    }
                    else if (newTabPage == tabDHeading)
                    {
                        tabDHeading_Enter(null, null);
                    }
                    else if (newTabPage == tabVConfig)
                    {
                        tabVConfig_Enter(null, null);
                    }
                    else if (newTabPage == tabDisplay)
                    {
                        tabDisplay_Enter(null, null);
                    }
                    else if (newTabPage == tabBtns)
                    {
                        tabBtns_Enter(null, null);
                    }
                    else if (newTabPage == tabTram)
                    {
                        tabTram_Enter(null, null);
                    }
                    else if (newTabPage == tabAMachine)
                    {
                        tabAMachine_Enter(null, null);
                    }
                    else if (newTabPage == tabRelay)
                    {
                        tabRelay_Enter(null, null);
                    }
                    else if (newTabPage == tabUTurn)
                    {
                        tabUTurn_Enter(null, null);
                    }
                    else if (newTabPage == tabDRoll)
                    {
                        tabDRoll_Enter(null, null);
                    }
                    else if (newTabPage == tabTSettings)
                    {
                        tabTSettings_Enter(null, null);
                    }
                    else if (newTabPage == tabTSwitches)
                    {
                        tabTSwitches_Enter(null, null);
                    }
                    else if (newTabPage == tabTSections)
                    {
                        tabTSections_Enter(null, null);
                    }
                    else if (newTabPage == tabToolPivot)
                    {
                        tabToolPivot_Enter(null, null);
                    }
                    else if (newTabPage == tabToolOffset)
                    {
                        tabToolOffset_Enter(null, null);
                    }
                    else if (newTabPage == tabTHitch)
                    {
                        tabTHitch_Enter(null, null);
                    }
                    else if (newTabPage == tabTConfig)
                    {
                        tabTConfig_Enter(null, null);
                    }
                    else if (newTabPage == tabVDimensions)
                    {
                        tabVDimensions_Enter(null, null);
                    }
                    else if (newTabPage == tabVAntenna)
                    {
                        tabVAntenna_Enter(null, null);
                    }
                    else
                    {
                        throw new Exception($"Unknown tab entered: {newTabPage.Text}");
                    }
                }

                oldTabPage = newTabPage;
            }
        }
        #endregion
    }
}