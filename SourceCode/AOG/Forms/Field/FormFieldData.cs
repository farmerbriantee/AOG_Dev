//Please, if you use this give me some credit
//Copyright BrianTee, copy right out of it.

using System;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormFieldData : Form
    {
        private readonly FormGPS mf = null;

        public FormFieldData(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void FormFieldData_Load(object sender, EventArgs e)
        {
            timer1_Tick(this, EventArgs.Empty);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //lblEastingField.Text = Math.Round(mf.pn.fix.easting, 1).ToString();
            //lblNorthingField.Text = Math.Round(mf.pn.fix.northing, 1).ToString();

            lblOverlapPercent.Text = mf.fd.ActualOverlapPercent;

            lblWorkRate.Text = mf.fd.WorkRateHour;
            lblApplied.Text = mf.fd.WorkedArea;
            lblActualLessOverlap.Text = mf.fd.ActualAreaWorked;

            if (mf.bnd.bndList.Count > 0)
            {
                lblTimeRemaining.Text = mf.fd.TimeTillFinished;
                lblRemainPercent.Text = mf.fd.WorkedAreaRemainPercentage;

                lblTotalArea.Text = mf.fd.AreaBoundaryLessInners;
                lblAreaRemain.Text = mf.fd.WorkedAreaRemain;
                lblActualRemain.Text = mf.fd.ActualRemain;
            }
            else
            {
                lblTotalArea.Text = "-";
                lblAreaRemain.Text = "-";
                lblTimeRemaining.Text = "-";
                lblRemainPercent.Text = "-";
            }
        }
    }
}

//lblLookOnLeft.Text = mf.tool.lookAheadDistanceOnPixelsLeft.ToString("N0");
//lblLookOnRight.Text = mf.tool.lookAheadDistanceOnPixelsRight.ToString("N0");
//lblLookOffLeft.Text = mf.tool.lookAheadDistanceOffPixelsLeft.ToString("N0");
//lblLookOffRight.Text = mf.tool.lookAheadDistanceOffPixelsRight.ToString("N0");

//lblLeftToolSpd.Text = (mf.tool.toolFarLeftSpeed*3.6).ToString("N1");
//lblRightToolSpd.Text = (mf.tool.toolFarRightSpeed*3.6).ToString("N1");

//lblSectSpdLeft.Text = (mf.section[0].speedPixels*0.36).ToString("N1");
//lblSectSpdRight.Text = (mf.section[mf.tool.numOfSections-1].speedPixels*0.36).ToString("N1");