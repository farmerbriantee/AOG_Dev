//Please, if you use this, share the improvements

using AOG.Properties;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormColorSection : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public Color[] customSectionColorsList = new Color[16];

        private bool isUse = true, isChange = false, isClosing = false;

        //constructor
        public FormColorSection(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            //this.Text = gStr.Get(gs.gsColors;

            string[] words = Settings.User.setDisplay_customSectionColors.Split(',');
            for (int i = 0; i < 16; i++)
            {
                customSectionColorsList[i] = Color.FromArgb(int.Parse(words[i], CultureInfo.InvariantCulture)).CheckColorFor255();
            }
        }

        private void FormDisplaySettings_Load(object sender, EventArgs e)
        {
            cb01.BackColor = Settings.Tool.setColor_sec01;
            cb02.BackColor = Settings.Tool.setColor_sec02;
            cb03.BackColor = Settings.Tool.setColor_sec03;
            cb04.BackColor = Settings.Tool.setColor_sec04;
            cb05.BackColor = Settings.Tool.setColor_sec05;
            cb06.BackColor = Settings.Tool.setColor_sec06;
            cb07.BackColor = Settings.Tool.setColor_sec07;
            cb08.BackColor = Settings.Tool.setColor_sec08;
            cb09.BackColor = Settings.Tool.setColor_sec09;
            cb10.BackColor = Settings.Tool.setColor_sec10;
            cb11.BackColor = Settings.Tool.setColor_sec11;
            cb12.BackColor = Settings.Tool.setColor_sec12;
            cb13.BackColor = Settings.Tool.setColor_sec13;
            cb14.BackColor = Settings.Tool.setColor_sec14;
            cb15.BackColor = Settings.Tool.setColor_sec15;
            cb16.BackColor = Settings.Tool.setColor_sec16;

            if (Settings.Tool.setColor_isMultiColorSections) cboxIsMulti.Checked = true;
            else cboxIsMulti.Checked = false;

            btnC01.BackColor = customSectionColorsList[0];
            btnC02.BackColor = customSectionColorsList[1];
            btnC03.BackColor = customSectionColorsList[2];
            btnC04.BackColor = customSectionColorsList[3];
            btnC05.BackColor = customSectionColorsList[4];
            btnC06.BackColor = customSectionColorsList[5];
            btnC07.BackColor = customSectionColorsList[6];
            btnC08.BackColor = customSectionColorsList[7];
            btnC09.BackColor = customSectionColorsList[8];
            btnC10.BackColor = customSectionColorsList[9];
            btnC11.BackColor = customSectionColorsList[10];
            btnC12.BackColor = customSectionColorsList[11];
            btnC13.BackColor = customSectionColorsList[12];
            btnC14.BackColor = customSectionColorsList[13];
            btnC15.BackColor = customSectionColorsList[14];
            btnC16.BackColor = customSectionColorsList[15];

            if (!cboxIsMulti.Checked)
            {
                SetGui(false);
            }

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void SetGui(bool set)
        {
            btnC01.Enabled = set;
            btnC02.Enabled = set;
            btnC03.Enabled = set;
            btnC04.Enabled = set;
            btnC05.Enabled = set;
            btnC06.Enabled = set;
            btnC07.Enabled = set;
            btnC08.Enabled = set;
            btnC09.Enabled = set;
            btnC10.Enabled = set;
            btnC11.Enabled = set;
            btnC12.Enabled = set;
            btnC13.Enabled = set;
            btnC14.Enabled = set;
            btnC15.Enabled = set;
            btnC16.Enabled = set;

            cb01.Enabled = set;
            cb02.Enabled = set;
            cb03.Enabled = set;
            cb04.Enabled = set;
            cb05.Enabled = set;
            cb06.Enabled = set;
            cb07.Enabled = set;
            cb08.Enabled = set;
            cb09.Enabled = set;
            cb10.Enabled = set;
            cb11.Enabled = set;
            cb12.Enabled = set;
            cb13.Enabled = set;
            cb14.Enabled = set;
            cb15.Enabled = set;
            cb16.Enabled = set;

            chkUse.Enabled = set;
        }

        private void SaveCustomColor()
        {
            Settings.User.setDisplay_customSectionColors = "";
            for (int i = 0; i < 15; i++)
            {
                Settings.User.setDisplay_customSectionColors += customSectionColorsList[i].ToArgb() + ",";
            }
            Settings.User.setDisplay_customSectionColors += customSectionColorsList[15].ToArgb();
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            mf.tool.secColors[0] = Settings.Tool.setColor_sec01 = cb01.BackColor;
            mf.tool.secColors[1] = Settings.Tool.setColor_sec02 = cb02.BackColor;
            mf.tool.secColors[2] = Settings.Tool.setColor_sec03 = cb03.BackColor;
            mf.tool.secColors[3] = Settings.Tool.setColor_sec04 = cb04.BackColor;
            mf.tool.secColors[4] = Settings.Tool.setColor_sec05 = cb05.BackColor;
            mf.tool.secColors[5] = Settings.Tool.setColor_sec06 = cb06.BackColor;
            mf.tool.secColors[6] = Settings.Tool.setColor_sec07 = cb07.BackColor;
            mf.tool.secColors[7] = Settings.Tool.setColor_sec08 = cb08.BackColor;
            mf.tool.secColors[8] = Settings.Tool.setColor_sec09 = cb09.BackColor;
            mf.tool.secColors[9] = Settings.Tool.setColor_sec10 = cb10.BackColor;
            mf.tool.secColors[10] = Settings.Tool.setColor_sec11 = cb11.BackColor;
            mf.tool.secColors[11] = Settings.Tool.setColor_sec12 = cb12.BackColor;
            mf.tool.secColors[12] = Settings.Tool.setColor_sec13 = cb13.BackColor;
            mf.tool.secColors[13] = Settings.Tool.setColor_sec14 = cb14.BackColor;
            mf.tool.secColors[14] = Settings.Tool.setColor_sec15 = cb15.BackColor;
            mf.tool.secColors[15] = Settings.Tool.setColor_sec16 = cb16.BackColor;

            Settings.Tool.setColor_isMultiColorSections = cboxIsMulti.Checked;

            isClosing = true;
            Close();
        }

        private void chkUse_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUse.Checked)
            {
                groupBox1.Text = "Select Square Below And Pick New Color";
                chkUse.Image = Properties.Resources.ColorUnlocked;
                isChange = true;
                isUse = false;
            }
            else
            {
                isChange = false;
                isUse = false;
                groupBox1.Text = "Select Preset Color";
                chkUse.Image = Properties.Resources.ColorLocked;
            }
        }

        private void UpdateColor(Color col)
        {
            col = col.CheckColorFor255();
            if (cb01.Checked) cb01.BackColor = col;
            else if (cb02.Checked) cb02.BackColor = col;
            else if (cb03.Checked) cb03.BackColor = col;
            else if (cb04.Checked) cb04.BackColor = col;
            else if (cb05.Checked) cb05.BackColor = col;
            else if (cb06.Checked) cb06.BackColor = col;
            else if (cb07.Checked) cb07.BackColor = col;
            else if (cb08.Checked) cb08.BackColor = col;
            else if (cb09.Checked) cb09.BackColor = col;
            else if (cb10.Checked) cb10.BackColor = col;
            else if (cb11.Checked) cb11.BackColor = col;
            else if (cb12.Checked) cb12.BackColor = col;
            else if (cb13.Checked) cb13.BackColor = col;
            else if (cb14.Checked) cb14.BackColor = col;
            else if (cb15.Checked) cb15.BackColor = col;
            else if (cb16.Checked) cb16.BackColor = col;

            cb01.Checked =
                cb02.Checked =
                cb03.Checked =
                cb04.Checked =
                cb05.Checked =
                cb06.Checked =
                cb07.Checked =
                cb08.Checked =
                cb09.Checked =
                cb10.Checked =
                cb11.Checked =
                cb12.Checked =
                cb13.Checked =
                cb14.Checked =
                cb15.Checked =
                cb16.Checked = false;

            isUse = false;
        }

        private void btnC01_Click(object sender, EventArgs e)
        {
            Button butt = (Button)sender;
            if (isUse)
            {
                UpdateColor(butt.BackColor);
                isUse = false;
            }
            else if (isChange)
            {
                using (FormColorPicker form = new FormColorPicker(mf, butt.BackColor))
                {
                    int.TryParse((butt.Name.Substring(4, 2)), out int buttNumber);

                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        (customSectionColorsList[buttNumber - 1]) = form.useThisColor;
                        butt.BackColor = form.useThisColor;
                    }
                }

                SaveCustomColor();
                isChange = false;
            }
            chkUse.Checked = false;
        }

        private void cboxIsMulti_Click(object sender, EventArgs e)
        {
            if (cboxIsMulti.Checked) SetGui(true);
            else SetGui(false);
        }

        private void FormSectionColor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
                e.Cancel = true;
        }

        private void cb01_Click(object sender, EventArgs e)
        {
            CheckBox cbox = (CheckBox)sender;
            cb01.Checked =
            cb02.Checked =
            cb03.Checked =
            cb04.Checked =
            cb05.Checked =
            cb06.Checked =
            cb07.Checked =
            cb08.Checked =
            cb09.Checked =
            cb10.Checked =
            cb11.Checked =
            cb12.Checked =
            cb13.Checked =
            cb14.Checked =
            cb15.Checked =
            cb16.Checked = false;

            cbox.Checked = true;
            isUse = true;
            isChange = false;
        }
    }
}