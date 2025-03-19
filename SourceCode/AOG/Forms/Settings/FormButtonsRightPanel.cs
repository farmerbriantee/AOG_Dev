using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormButtonsRightPanel : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        List<string> buttonOrder = new List<string>();

        public FormButtonsRightPanel(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void FormToolPivot_Load(object sender, EventArgs e)
        {
            flpRight.Controls.Clear();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void btnAutoSteer_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(autoSteer);
            btnAutoSteer.Enabled = false;
            buttonOrder.Add("0");
        }

        private void btnAutoYouTurn_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(youTurn);
            btnAutoYouTurn.Enabled = false;
            buttonOrder.Add("1");
        }

        private void btnSectionMasterAuto_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(autoSection);
            btnSectionMasterAuto.Enabled = false;
            buttonOrder.Add("2");
        }

        private void btnSectionMasterManual_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(manualSection);
            btnSectionMasterManual.Enabled = false;
            buttonOrder.Add("3");
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            flpRight.Controls?.Clear();
            flpRight.Controls.Add(autoSteer);
            flpRight.Controls.Add(youTurn);
            flpRight.Controls.Add(autoSection);
            flpRight.Controls.Add(manualSection);

            flpRight.Controls.Add(skipPrev);
            flpRight.Controls.Add(skipNext);
            flpRight.Controls.Add(contour);

            buttonOrder?.Clear();
            for (int i = 0; i < flpRight.Controls.Count; i++)
            {
                buttonOrder.Add(i.ToString());
            }

            btnAutoSteer.Enabled = false;
            btnAutoYouTurn.Enabled = false;
            btnSectionMasterManual.Enabled = false;
            btnSectionMasterAuto.Enabled = false;
            btnCycleLinesBk.Enabled = false;
            btnCycleLines.Enabled = false;
            btnContour.Enabled = false;
        }

        private void btnCycleLinesBk_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(skipPrev);
            btnCycleLinesBk.Enabled = false;
            buttonOrder.Add("5");
        }

        private void btnCycleLines_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(skipNext);
            btnCycleLines.Enabled = false;
            buttonOrder.Add("6");
        }

        private void btnContour_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(contour);
            btnContour.Enabled = false;
            buttonOrder.Add("7");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnAutoSteer.Enabled = true;
            btnAutoYouTurn.Enabled = true;
            btnSectionMasterManual.Enabled = true;
            btnSectionMasterAuto.Enabled = true;
            btnCycleLinesBk.Enabled = true;
            btnCycleLines.Enabled = true;
            btnContour.Enabled = true;

            flpRight.Controls.Clear();
            buttonOrder?.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (buttonOrder.Count < 2)
            {
                mf.TimedMessageBox(2000, "Button Error", "Not Enough Buttons Added");
                Log.EventWriter("Button Picker, Not Enough Buttons");
                return;
            }
            else
            {
                Settings.User.setDisplay_buttonOrder = string.Join(",", buttonOrder);

                mf.PanelBuildRightMenu(buttonOrder.ToArray());
                mf.PanelUpdateRightAndBottom();
                Close();
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            mf.PanelBuildRightMenu(buttonOrder.ToArray());
            mf.PanelUpdateRightAndBottom();
        }
    }
}