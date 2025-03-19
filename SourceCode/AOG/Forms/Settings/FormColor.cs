//Please, if you use this, share the improvements

using AOG.Classes;

using AOG.Properties;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormColor : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private bool originDaySet;

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
            originDaySet = Settings.User.setDisplay_isDayMode;
            hsbarSmooth.Value = Settings.User.setDisplay_camSmooth;
            lblSmoothCam.Text = hsbarSmooth.Value.ToString() + "%";

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            if (originDaySet != Settings.User.setDisplay_isDayMode) mf.SwapDayNightMode();//reset to old value
            Settings.User.setDisplay_camSmooth = hsbarSmooth.Value;

            mf.camera.camSmoothFactor = ((double)(hsbarSmooth.Value) * 0.004) + 0.15;

            Close();
        }

        private void btnFrameDay_Click(object sender, EventArgs e)
        {
            if (!Settings.User.setDisplay_isDayMode) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.User.colorDayFrame))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorDayFrame = form.useThisColor;
                    mf.SwapDayNightMode(false);
                }
            }
        }

        private void btnFrameNight_Click(object sender, EventArgs e)
        {
            if (Settings.User.setDisplay_isDayMode) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.User.colorNightFrame))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorNightFrame = form.useThisColor;
                    mf.SwapDayNightMode(false);
                }
            }
        }

        private void btnFieldDay_Click(object sender, EventArgs e)
        {
            if (!Settings.User.setDisplay_isDayMode) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.User.colorFieldDay))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorFieldDay = form.useThisColor;
                    mf.SwapDayNightMode(false);
                }
            }
        }

        private void btnFieldNight_Click(object sender, EventArgs e)
        {
            if (Settings.User.setDisplay_isDayMode) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.User.colorFieldNight))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorFieldNight = form.useThisColor;
                    mf.SwapDayNightMode(false);
                }
            }
        }

        private void btnSwaPGN_Click(object sender, EventArgs e)
        {
            mf.SwapDayNightMode();
        }

        private void btnNightText_Click(object sender, EventArgs e)
        {
            if (Settings.User.setDisplay_isDayMode) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.User.colorTextNight))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorTextNight = form.useThisColor;
                    mf.SwapDayNightMode(false);
                }
            }
        }

        private void btnDayText_Click(object sender, EventArgs e)
        {
            if (!Settings.User.setDisplay_isDayMode) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.User.colorTextDay))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.User.colorTextDay = form.useThisColor;
                    mf.SwapDayNightMode(false);
                }
            }
        }

        private void hsbarSmooth_ValueChanged(object sender, EventArgs e)
        {
            lblSmoothCam.Text = hsbarSmooth.Value.ToString() + "%";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (!Settings.User.setDisplay_isDayMode) mf.SwapDayNightMode();

            Settings.User.colorDayFrame = Color.FromArgb(210, 210, 230);
            Settings.User.colorNightFrame = Color.FromArgb(50, 50, 65);
            Settings.User.colorSectionsDay = Color.FromArgb(27, 151, 160);
            Settings.User.colorFieldDay = Color.FromArgb(100, 100, 125);
            Settings.User.colorFieldNight = Color.FromArgb(60, 60, 60);

            Settings.User.colorTextDay = Color.FromArgb(10, 10, 20);
            Settings.User.colorTextNight = Color.FromArgb(230, 230, 230);

            Settings.User.setDisplay_customColors = "-62208,-12299010,-16190712,-1505559,-3621034,-16712458,-7330570,-1546731,-24406,-3289866,-2756674,-538377,-134768,-4457734,-1848839,-530985";

            mf.SwapDayNightMode(false);
        }
    }
}