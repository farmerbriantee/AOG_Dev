﻿//Please, if you use this, share the improvements

using AgOpenGPS.Classes;

using AgOpenGPS.Properties;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormColor : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private bool daySet;

        //constructor
        public FormColor(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            this.Text = gStr.Get(gs.gsColors);
        }

        private void FormDisplaySettings_Load(object sender, EventArgs e)
        {
            daySet = mf.isDay;
            hsbarSmooth.Value = Settings.Vehicle.setDisplay_camSmooth;
            lblSmoothCam.Text = hsbarSmooth.Value.ToString() + "%";

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            if (daySet != mf.isDay) mf.SwapDayNightMode();
            Settings.Vehicle.setDisplay_camSmooth = hsbarSmooth.Value;

            mf.camera.camSmoothFactor = ((double)(hsbarSmooth.Value) * 0.004) + 0.15;

            Close();
        }

        private void btnFrameDay_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.User.colorDayFrame))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorDayFrame = form.useThisColor;
                }
            }

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnFrameNight_Click(object sender, EventArgs e)
        {
            if (mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.User.colorNightFrame))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorNightFrame = form.useThisColor;
                }
            }

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnFieldDay_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.User.colorFieldDay))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorFieldDay = form.useThisColor;
                }
            }

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnFieldNight_Click(object sender, EventArgs e)
        {
            if (mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.User.colorFieldNight))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorFieldNight = form.useThisColor;
                }
            }

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnSwaPGN_Click(object sender, EventArgs e)
        {
            mf.SwapDayNightMode();
        }

        private void btnNightText_Click(object sender, EventArgs e)
        {
            if (mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.User.colorTextNight))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorTextNight = form.useThisColor;
                }
            }

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnDayText_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.User.colorTextDay))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorTextDay = form.useThisColor;
                }
            }

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void hsbarSmooth_ValueChanged(object sender, EventArgs e)
        {
            lblSmoothCam.Text = hsbarSmooth.Value.ToString() + "%";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();

            Settings.User.colorDayFrame = Color.FromArgb(210, 210, 230);
            Settings.User.colorNightFrame = Color.FromArgb(50, 50, 65);
            Settings.User.colorSectionsDay = Color.FromArgb(27, 151, 160);
            Settings.User.colorFieldDay = Color.FromArgb(100, 100, 125);
            Settings.User.colorFieldNight = Color.FromArgb(60, 60, 60);

            Settings.User.colorTextDay = Color.FromArgb(10, 10, 20);
            Settings.User.colorTextNight = Color.FromArgb(230, 230, 230);

            Settings.Vehicle.setDisplay_customColors = "-62208,-12299010,-16190712,-1505559,-3621034,-16712458,-7330570,-1546731,-24406,-3289866,-2756674,-538377,-134768,-4457734,-1848839,-530985";

            string[] words = Settings.Vehicle.setDisplay_customColors.Split(',');
            for (int i = 0; i < 16; i++)
            {
                mf.customColorsList[i] = int.Parse(words[i], CultureInfo.InvariantCulture);
                Color test = Color.FromArgb(mf.customColorsList[i]).CheckColorFor255();
                mf.customColorsList[i] = test.ToArgb();
            }

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }
    }
}