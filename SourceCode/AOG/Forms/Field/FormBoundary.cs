using AOG.Classes;

using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormBoundary : Form
    {
        private readonly FormGPS mf = null;

        private double easting, norting, latK, lonK;
        private int fenceSelected = -1;

        private bool isClosing;

        public FormBoundary(Form callingForm)
        {
            mf = callingForm as FormGPS;

            //winform initialization
            InitializeComponent();

            this.Text = gStr.Get(gs.gsStartDeleteABoundary);

            //Column Header
            Boundary.Text = "Bounds";
            Thru.Text = gStr.Get(gs.gsDriveThru);
            Area.Text = gStr.Get(gs.gsArea);
            btnDelete.Enabled = false;
        }

        private void FormBoundary_Load(object sender, EventArgs e)
        {
            this.Size = new Size(770, 310);

            //update the list view with real data
            UpdateChart();

            panelMain.Dock = DockStyle.Fill;
            panelChoose.Dock = DockStyle.Fill;
            panelKML.Dock = DockStyle.Fill;

            panelMain.Visible = true;
            panelChoose.Visible = false;
            panelKML.Visible = false;
            mf.CloseTopMosts();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormBoundary_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }
        }

        private void UpdateChart()
        {
            int inner = 1;

            flp1.Controls.Clear();

            for (int i = 0; i < mf.bnd.bndList.Count; i++)
            {
                //outer inner
                Button a = new Button
                {
                    Margin = new Padding(10),
                    Size = new Size(180, 35),
                    Name = i.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    //ForeColor = System.Drawing.SystemColors.ButtonFace
                };
                a.Click += B_Click;
                a.BackColor = System.Drawing.SystemColors.ButtonFace;

                //area
                Button b = new Button
                {
                    Margin = new Padding(10),
                    Size = new System.Drawing.Size(180, 35),
                    Name = i.ToString(),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    //ForeColor = System.Drawing.SystemColors.ButtonFace
                };
                b.Click += B_Click;
                b.BackColor = System.Drawing.SystemColors.ButtonFace;

                //drive thru
                Button d = new Button
                {
                    Margin = new Padding(10),
                    Size = new System.Drawing.Size(110, 35),
                    Name = i.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    //ForeColor = System.Drawing.SystemColors.ButtonFace
                    //Font = backupfont
                };
                d.Click += DriveThru_Click;
                d.BackColor = System.Drawing.SystemColors.ButtonFace;
                d.Visible = true;

                flp1.Controls.Add(a);
                flp1.Controls.Add(b);
                flp1.Controls.Add(d);

                if (i == 0)
                {
                    //cc.Text = "Outer";
                    mf.bnd.bndList[i].isDriveThru = false;
                    a.Text = string.Format(gStr.Get(gs.gsOuter));
                    //a.Font = backupfont;
                    d.Text = "--";
                    d.Enabled = false;
                    d.Anchor = System.Windows.Forms.AnchorStyles.None;
                    a.Anchor = System.Windows.Forms.AnchorStyles.None;
                    b.Anchor = System.Windows.Forms.AnchorStyles.None;
                }
                else
                {
                    //cc.Text = "Inner";
                    inner += 1;
                    a.Text = string.Format(gStr.Get(gs.gsInner) + " {0}", inner);
                    //a.Font = backupfont;
                    d.Text = mf.bnd.bndList[i].isDriveThru ? "Yes" : "No";
                    d.Anchor = System.Windows.Forms.AnchorStyles.None;
                    a.Anchor = System.Windows.Forms.AnchorStyles.None;
                    b.Anchor = System.Windows.Forms.AnchorStyles.None;
                }

                int length = (mf.bnd.bndList[i].area * glm.m22HaOrAc).ToString("0").Length;
                if (length > 10) length = 10;
                if (length < 3) length = 3;
                b.Text = (mf.bnd.bndList[i].area * glm.m22HaOrAc).ToString("0.########".Substring(0, 11 - length)) + glm.unitsHaOrAc;

                if (i == fenceSelected)
                {
                    a.ForeColor = Color.OrangeRed;
                    b.ForeColor = Color.OrangeRed;
                }
                else
                {
                    a.ForeColor = System.Drawing.SystemColors.ControlText;
                    b.ForeColor = System.Drawing.SystemColors.ControlText;
                }
            }
        }

        private void DriveThru_Click(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                mf.bnd.bndList[Convert.ToInt32(b.Name)].isDriveThru = !mf.bnd.bndList[Convert.ToInt32(b.Name)].isDriveThru;
                UpdateChart();
            }
        }

        private void B_Click(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                int oldfenceSelected = fenceSelected;
                fenceSelected = Convert.ToInt32(b.Name);

                if (fenceSelected == oldfenceSelected)
                    fenceSelected = -1;
                else if (fenceSelected == 0)
                    btnDelete.Enabled = mf.bnd.bndList.Count == 1;
                else if (fenceSelected > 0)
                    btnDelete.Enabled = true;

                //btnDeleteAll.Enabled = fenceSelected == -1;
            }
            UpdateChart();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show(gStr.Get(gs.gsCompletelyDeleteBoundary),
                gStr.Get(gs.gsDeleteForSure),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result3 == DialogResult.Yes)
            {
                btnDelete.Enabled = false;

                if (mf.bnd.bndList.Count > fenceSelected)
                {
                    mf.bnd.bndList[fenceSelected].hdLine?.Clear();
                    mf.bnd.bndList.RemoveAt(fenceSelected);
                }
                fenceSelected = -1;

                mf.FileSaveBoundary();
                mf.FileSaveHeadland();
                UpdateChart();
            }
            else
            {
                mf.TimedMessageBox(1500, gStr.Get(gs.gsNothingDeleted), gStr.Get(gs.gsActionHasBeenCancelled));
            }
        }

        private void ResetAllBoundary()
        {
            fenceSelected = -1;
            mf.bnd.bndList.Clear();
            mf.FileSaveHeadland();

            flp1.Controls.Clear();

            UpdateChart();
            btnDelete.Enabled = false;
        }

        private void btnOpenGoogleEarth_Click(object sender, EventArgs e)
        {
            //save new copy of kml with selected flag and view in GoogleEarth

            mf.FileMakeKMLFromCurrentPosition(mf.pn.latitude, mf.pn.longitude);
            System.Diagnostics.Process.Start(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "CurrentPosition.KML"));
            isClosing = true;
            Close();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            mf.bnd.isOkToAddPoints = false;

            panelMain.Visible = true;
            panelChoose.Visible = false;
            panelKML.Visible = false;

            this.Size = new System.Drawing.Size(770, 310);
            isClosing = true;
            UpdateChart();
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Settings.Tool.toolWidth < 0.2)
            {
                mf.TimedMessageBox(2000, "Tool Error", "Your tool is too small");
                Log.EventWriter("Boundary, Tool is too narrow");

                return;
            }
            panelMain.Visible = false;
            panelKML.Visible = false;
            panelChoose.Visible = true;
            panelChoose.Dock = DockStyle.Fill;

            this.Size = new Size(245, 350);
        }

        private void btnLoadBoundaryFromGE_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string fileAndDirectory;
                {
                    //create the dialog instance
                    OpenFileDialog ofd = new OpenFileDialog
                    {
                        //set the filter to text KML only
                        Filter = "KML files (*.KML)|*.KML",

                        //the initial directory, fields, for the open dialog
                        InitialDirectory = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory)
                    };

                    //was a file selected
                    if (ofd.ShowDialog(this) == DialogResult.Cancel) return;
                    else fileAndDirectory = ofd.FileName;
                }

                string coordinates = null;
                int startIndex;

                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    if (button.Name == "btnLoadMultiBoundaryFromGE") ResetAllBoundary();

                    try
                    {
                        while (!reader.EndOfStream)
                        {
                            //start to read the file
                            string line = reader.ReadLine();

                            startIndex = line.IndexOf("<coordinates>");

                            if (startIndex != -1)
                            {
                                while (true)
                                {
                                    int endIndex = line.IndexOf("</coordinates>");

                                    if (endIndex == -1)
                                    {
                                        //just add the line
                                        if (startIndex == -1) coordinates += line.Substring(0);
                                        else coordinates += line.Substring(startIndex + 13);
                                    }
                                    else
                                    {
                                        if (startIndex == -1) coordinates += line.Substring(0, endIndex);
                                        else coordinates += line.Substring(startIndex + 13, endIndex - (startIndex + 13));
                                        break;
                                    }
                                    line = reader.ReadLine();
                                    line = line.Trim();
                                    startIndex = -1;
                                }

                                line = coordinates;
                                char[] delimiterChars = { ' ', '\t', '\r', '\n' };
                                string[] numberSets = line.Split(delimiterChars);

                                //at least 3 points
                                if (numberSets.Length > 2)
                                {
                                    CBoundaryList newBnd = new CBoundaryList();

                                    foreach (string item in numberSets)
                                    {
                                        string[] fix = item.Split(',');
                                        double.TryParse(fix[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);
                                        double.TryParse(fix[1], NumberStyles.Float, CultureInfo.InvariantCulture, out latK);

                                        mf.pn.ConvertWGS84ToLocal(latK, lonK, out norting, out easting);

                                        //add the point to boundary
                                        newBnd.fenceLine.Add(new vec3(easting, norting, 0));
                                    }
                                    mf.bnd.AddToBoundList(newBnd, mf.bnd.bndList.Count);

                                    coordinates = "";
                                }
                                else
                                {
                                    mf.TimedMessageBox(2000, gStr.Get(gs.gsErrorreadingKML), gStr.Get(gs.gsChooseBuildDifferentone));
                                    Log.EventWriter("KML Read Error to make new field");
                                }
                                if (button.Name == "btnLoadBoundaryFromGE")
                                {
                                    break;
                                }
                            }
                        }
                        mf.FileSaveBoundary();
                        UpdateChart();
                    }
                    catch (Exception ed)
                    {
                        Log.EventWriter("Load Boundary from GE " + ed.ToString());
                        return;
                    }
                }
            }
            mf.bnd.isOkToAddPoints = false;

            panelMain.Visible = true;
            panelChoose.Visible = false;
            panelKML.Visible = false;

            this.Size = new Size(770, 310);

            UpdateChart();
        }

        private void btnDriveOrExt_Click(object sender, EventArgs e)
        {
            panelMain.Visible = false;
            panelChoose.Visible = false;
            panelKML.Visible = false;
            isClosing = true;
        }

        private void btnBndLines_Click(object sender, EventArgs e)
        {
            panelMain.Visible = false;
            panelChoose.Visible = false;
            panelKML.Visible = false;
            isClosing = true;
        }

        private void btnGetKML_Click(object sender, EventArgs e)
        {
            panelMain.Visible = false;
            panelChoose.Visible = false;
            panelKML.Visible = true;
        }

        private void btnBingMaps_Click(object sender, EventArgs e)
        {
            panelMain.Visible = false;
            panelChoose.Visible = false;
            panelKML.Visible = false;
            isClosing = true;
        }

        private void FormBoundary_ResizeEnd(object sender, EventArgs e)
        {
            this.Width = 770;
        }
    }
}