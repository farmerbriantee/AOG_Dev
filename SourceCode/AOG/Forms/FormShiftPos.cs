using AOG.Classes;

using System;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormShiftPos : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormShiftPos(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            label27.Text = gStr.Get(gs.gsNorth);
            label2.Text = gStr.Get(gs.gsWest);
            label3.Text = gStr.Get(gs.gsEast);
            label4.Text = gStr.Get(gs.gsSouth);
            this.Text = gStr.Get(gs.gsShiftGPSPosition);
        }

        private void FormShiftPos_Load(object sender, EventArgs e)
        {
            nudEast.Value = mf.pn.fixOffset.easting;
            nudNorth.Value = mf.pn.fixOffset.northing;
            chkOffsetsOn.Checked = mf.isKeepOffsetsOn;
            if (chkOffsetsOn.Checked) chkOffsetsOn.Text = "On";
            else chkOffsetsOn.Text = "Off";
        }

        private void btnNorth_MouseDown(object sender, MouseEventArgs e)
        {
            nudNorth.Value++;
        }

        private void btnSouth_MouseDown(object sender, MouseEventArgs e)
        {
            nudNorth.Value--;
        }

        private void btnWest_MouseDown(object sender, MouseEventArgs e)
        {
            nudEast.Value--;
        }

        private void btnEast_MouseDown(object sender, MouseEventArgs e)
        {
            nudEast.Value++;
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            nudEast.Value = 0;
            nudNorth.Value = 0;
            mf.pn.fixOffset.easting = 0;
            mf.pn.fixOffset.northing = 0;
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            mf.isKeepOffsetsOn = chkOffsetsOn.Checked;
            Close();
        }

        private void chkOffsetsOn_Click(object sender, EventArgs e)
        {
            if (chkOffsetsOn.Checked) chkOffsetsOn.Text = "On";
            else chkOffsetsOn.Text = "Off";
        }

        private void nudNorth_ValueChanged(object sender, EventArgs e)
        {
            mf.pn.fixOffset.northing = nudNorth.Value;
        }

        private void nudEast_ValueChanged(object sender, EventArgs e)
        {
            mf.pn.fixOffset.easting = nudEast.Value;
        }
    }
}