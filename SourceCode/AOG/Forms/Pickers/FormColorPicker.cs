﻿using MechanikaDesign.WinForms.UI.ColorPicker;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormColorPicker : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private readonly Color inColor;
        public Color useThisColor { get; set; }

        private bool isUse = true;

        private HslColor colorHsl = HslColor.FromAhsl(0xff);
        private Color colorRgb = Color.Empty;

        public FormColorPicker(Form callingForm, Color _inColor)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            inColor = _inColor;

            btnNight.BackColor = inColor;
            btnDay.BackColor = inColor;

            useThisColor = inColor;

            UpdateColor(inColor);

            //this.bntOK.Text = gStr.Get(gs.gsForNow;
            //this.btnSave.Text = gStr.Get(gs.gsToFile;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void colorBox2D_ColorChanged(object sender, ColorChangedEventArgs args)
        {
            this.colorRgb = colorBox2D.ColorRGB.CheckColorFor255();
            HslColor colorHSL = HslColor.FromColor(colorRgb);

            this.colorHsl = colorHSL;
            this.colorSlider.ColorHSL = this.colorHsl;

            useThisColor = colorRgb;
            btnNight.BackColor = colorRgb;
            btnDay.BackColor = colorRgb;
        }

        private void colorSlider_ColorChanged(object sender, MechanikaDesign.WinForms.UI.ColorPicker.ColorChangedEventArgs args)
        {
            HslColor colorHSL = this.colorSlider.ColorHSL;
            this.colorHsl = colorHSL;
            this.colorRgb = this.colorHsl.RgbValue.CheckColorFor255();
            this.colorBox2D.ColorHSL = this.colorHsl;

            useThisColor = colorRgb;
            btnNight.BackColor = colorRgb;
            btnDay.BackColor = colorRgb;
        }

        private void UpdateColor(Color col)
        {
            col = col.CheckColorFor255();
            this.colorHsl = HslColor.FromColor(col);
            this.colorRgb = col;
            this.colorSlider.ColorHSL = this.colorHsl;
            this.colorBox2D.ColorHSL = this.colorHsl;

            useThisColor = col;
            btnNight.BackColor = col;
            btnDay.BackColor = col;
        }

        private void FormColorPicker_Load(object sender, EventArgs e)
        {
            btn00.BackColor = (Color.FromArgb(mf.customColorsList[0]));
            btn01.BackColor = (Color.FromArgb(mf.customColorsList[1]));
            btn02.BackColor = (Color.FromArgb(mf.customColorsList[2]));
            btn03.BackColor = (Color.FromArgb(mf.customColorsList[3]));
            btn04.BackColor = (Color.FromArgb(mf.customColorsList[4]));
            btn05.BackColor = (Color.FromArgb(mf.customColorsList[5]));
            btn06.BackColor = (Color.FromArgb(mf.customColorsList[6]));
            btn07.BackColor = (Color.FromArgb(mf.customColorsList[7]));
            btn08.BackColor = (Color.FromArgb(mf.customColorsList[8]));
            btn09.BackColor = (Color.FromArgb(mf.customColorsList[9]));
            btn10.BackColor = (Color.FromArgb(mf.customColorsList[10]));
            btn11.BackColor = (Color.FromArgb(mf.customColorsList[11]));
            btn12.BackColor = (Color.FromArgb(mf.customColorsList[12]));
            btn13.BackColor = (Color.FromArgb(mf.customColorsList[13]));
            btn14.BackColor = (Color.FromArgb(mf.customColorsList[14]));
            btn15.BackColor = (Color.FromArgb(mf.customColorsList[15]));

            //save them just in case
            SaveCustomColor();
        }

        private void SaveCustomColor()
        {
            Settings.Vehicle.setDisplay_customColors = "";
            for (int i = 0; i < 15; i++)
                Settings.Vehicle.setDisplay_customColors += mf.customColorsList[i].ToString() + ",";
            Settings.Vehicle.setDisplay_customColors += mf.customColorsList[15].ToString();
        }

        private void btn00_Click(object sender, EventArgs e)
        {
            Button butt = (Button)sender;
            if (isUse)
            {
                useThisColor = butt.BackColor.CheckColorFor255();
                UpdateColor(useThisColor);
            }
            else
            {
                int.TryParse(butt.Name.Substring(3, 2), out int buttNumber);

                useThisColor = useThisColor.CheckColorFor255();
                mf.customColorsList[buttNumber] = useThisColor.ToArgb();
                butt.BackColor = useThisColor;

                SaveCustomColor();
            }
        }

        private void chkUse_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUse.Checked)
            {
                groupBox1.Text = "Pick New Color and Select Square Below to Save Preset";
                chkUse.Image = Properties.Resources.ColorUnlocked;
                isUse = false;
            }
            else
            {
                isUse = true;
                groupBox1.Text = "Select Preset Color";
                chkUse.Image = Properties.Resources.ColorLocked;
            }
        }
    }
}