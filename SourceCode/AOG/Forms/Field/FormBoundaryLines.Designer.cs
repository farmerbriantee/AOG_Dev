namespace AOG
{
    partial class FormBoundaryLines
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.oglSelf = new OpenTK.GLControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnAShrink = new System.Windows.Forms.Button();
            this.btnBShrink = new System.Windows.Forms.Button();
            this.btnHeadlandOff = new System.Windows.Forms.Button();
            this.btnCycleBackward = new System.Windows.Forms.Button();
            this.btnBuildBnd = new System.Windows.Forms.Button();
            this.btnDeleteBoundary = new System.Windows.Forms.Button();
            this.btnCycleForward = new System.Windows.Forms.Button();
            this.btnALength = new System.Windows.Forms.Button();
            this.btnBLength = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tlp1 = new System.Windows.Forms.TableLayoutPanel();
            this.tboxLineName = new System.Windows.Forms.TextBox();
            this.btnSwapAB = new System.Windows.Forms.Button();
            this.cboxIsVisible = new System.Windows.Forms.CheckBox();
            this.tlp1.SuspendLayout();
            this.SuspendLayout();
            // 
            // oglSelf
            // 
            this.oglSelf.BackColor = System.Drawing.Color.Black;
            this.oglSelf.Cursor = System.Windows.Forms.Cursors.Cross;
            this.oglSelf.Location = new System.Drawing.Point(0, 1);
            this.oglSelf.Margin = new System.Windows.Forms.Padding(0);
            this.oglSelf.Name = "oglSelf";
            this.oglSelf.Size = new System.Drawing.Size(700, 700);
            this.oglSelf.TabIndex = 183;
            this.oglSelf.VSync = false;
            this.oglSelf.Load += new System.EventHandler(this.oglSelf_Load);
            this.oglSelf.Paint += new System.Windows.Forms.PaintEventHandler(this.oglSelf_Paint);
            this.oglSelf.MouseDown += new System.Windows.Forms.MouseEventHandler(this.oglSelf_MouseDown);
            this.oglSelf.Resize += new System.EventHandler(this.oglSelf_Resize);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnAShrink
            // 
            this.btnAShrink.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAShrink.BackColor = System.Drawing.Color.Transparent;
            this.btnAShrink.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnAShrink, 3);
            this.btnAShrink.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnAShrink.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAShrink.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAShrink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAShrink.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAShrink.Image = global::AOG.Properties.Resources.APlusMinusA;
            this.btnAShrink.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAShrink.Location = new System.Drawing.Point(29, 133);
            this.btnAShrink.Name = "btnAShrink";
            this.btnAShrink.Size = new System.Drawing.Size(92, 52);
            this.btnAShrink.TabIndex = 525;
            this.btnAShrink.UseVisualStyleBackColor = false;
            this.btnAShrink.Click += new System.EventHandler(this.btnAShrink_Click);
            // 
            // btnBShrink
            // 
            this.btnBShrink.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBShrink.BackColor = System.Drawing.Color.Transparent;
            this.btnBShrink.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnBShrink, 3);
            this.btnBShrink.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnBShrink.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBShrink.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBShrink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBShrink.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBShrink.Image = global::AOG.Properties.Resources.APlusMinusB;
            this.btnBShrink.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBShrink.Location = new System.Drawing.Point(180, 133);
            this.btnBShrink.Name = "btnBShrink";
            this.btnBShrink.Size = new System.Drawing.Size(92, 52);
            this.btnBShrink.TabIndex = 524;
            this.btnBShrink.UseVisualStyleBackColor = false;
            this.btnBShrink.Click += new System.EventHandler(this.btnBShrink_Click);
            // 
            // btnHeadlandOff
            // 
            this.btnHeadlandOff.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHeadlandOff.BackColor = System.Drawing.Color.Transparent;
            this.btnHeadlandOff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnHeadlandOff, 2);
            this.btnHeadlandOff.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnHeadlandOff.FlatAppearance.BorderSize = 0;
            this.btnHeadlandOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHeadlandOff.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnHeadlandOff.Image = global::AOG.Properties.Resources.SwitchOff;
            this.btnHeadlandOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHeadlandOff.Location = new System.Drawing.Point(18, 622);
            this.btnHeadlandOff.Name = "btnHeadlandOff";
            this.btnHeadlandOff.Size = new System.Drawing.Size(63, 70);
            this.btnHeadlandOff.TabIndex = 519;
            this.btnHeadlandOff.UseVisualStyleBackColor = false;
            this.btnHeadlandOff.Click += new System.EventHandler(this.btnHeadlandOff_Click);
            // 
            // btnCycleBackward
            // 
            this.btnCycleBackward.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCycleBackward.BackColor = System.Drawing.Color.Transparent;
            this.btnCycleBackward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnCycleBackward, 3);
            this.btnCycleBackward.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnCycleBackward.FlatAppearance.BorderSize = 0;
            this.btnCycleBackward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycleBackward.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCycleBackward.Image = global::AOG.Properties.Resources.ABLineCycleBk;
            this.btnCycleBackward.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCycleBackward.Location = new System.Drawing.Point(43, 260);
            this.btnCycleBackward.Name = "btnCycleBackward";
            this.btnCycleBackward.Size = new System.Drawing.Size(63, 68);
            this.btnCycleBackward.TabIndex = 507;
            this.btnCycleBackward.UseVisualStyleBackColor = false;
            this.btnCycleBackward.Click += new System.EventHandler(this.btnCycleBackward_Click);
            // 
            // btnBuildBnd
            // 
            this.btnBuildBnd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBuildBnd.BackColor = System.Drawing.Color.Transparent;
            this.btnBuildBnd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnBuildBnd, 3);
            this.btnBuildBnd.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnBuildBnd.FlatAppearance.BorderSize = 0;
            this.btnBuildBnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuildBnd.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBuildBnd.Image = global::AOG.Properties.Resources.BoundaryOuter;
            this.btnBuildBnd.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBuildBnd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBuildBnd.Location = new System.Drawing.Point(32, 474);
            this.btnBuildBnd.Name = "btnBuildBnd";
            this.btnBuildBnd.Size = new System.Drawing.Size(85, 102);
            this.btnBuildBnd.TabIndex = 504;
            this.btnBuildBnd.Text = "Build";
            this.btnBuildBnd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBuildBnd.UseVisualStyleBackColor = false;
            this.btnBuildBnd.Click += new System.EventHandler(this.btnBuildBnd_Click);
            // 
            // btnDeleteBoundary
            // 
            this.btnDeleteBoundary.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteBoundary.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteBoundary.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnDeleteBoundary, 3);
            this.btnDeleteBoundary.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnDeleteBoundary.FlatAppearance.BorderSize = 0;
            this.btnDeleteBoundary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteBoundary.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDeleteBoundary.Image = global::AOG.Properties.Resources.HeadlandReset;
            this.btnDeleteBoundary.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDeleteBoundary.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeleteBoundary.Location = new System.Drawing.Point(180, 474);
            this.btnDeleteBoundary.Name = "btnDeleteBoundary";
            this.btnDeleteBoundary.Size = new System.Drawing.Size(92, 102);
            this.btnDeleteBoundary.TabIndex = 465;
            this.btnDeleteBoundary.Text = "Reset";
            this.btnDeleteBoundary.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteBoundary.UseVisualStyleBackColor = false;
            this.btnDeleteBoundary.Click += new System.EventHandler(this.btnDeleteBoundary_Click);
            // 
            // btnCycleForward
            // 
            this.btnCycleForward.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCycleForward.BackColor = System.Drawing.Color.Transparent;
            this.btnCycleForward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnCycleForward, 3);
            this.btnCycleForward.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnCycleForward.FlatAppearance.BorderSize = 0;
            this.btnCycleForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycleForward.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCycleForward.Image = global::AOG.Properties.Resources.ABLineCycle;
            this.btnCycleForward.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCycleForward.Location = new System.Drawing.Point(195, 260);
            this.btnCycleForward.Name = "btnCycleForward";
            this.btnCycleForward.Size = new System.Drawing.Size(63, 68);
            this.btnCycleForward.TabIndex = 5;
            this.btnCycleForward.UseVisualStyleBackColor = false;
            this.btnCycleForward.Click += new System.EventHandler(this.btnCycleForward_Click);
            // 
            // btnALength
            // 
            this.btnALength.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnALength.BackColor = System.Drawing.Color.Transparent;
            this.btnALength.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnALength, 3);
            this.btnALength.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnALength.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnALength.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnALength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnALength.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnALength.Image = global::AOG.Properties.Resources.APlusPlusA;
            this.btnALength.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnALength.Location = new System.Drawing.Point(30, 27);
            this.btnALength.Name = "btnALength";
            this.btnALength.Size = new System.Drawing.Size(90, 52);
            this.btnALength.TabIndex = 352;
            this.btnALength.UseVisualStyleBackColor = false;
            this.btnALength.Click += new System.EventHandler(this.btnALength_Click);
            // 
            // btnBLength
            // 
            this.btnBLength.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBLength.BackColor = System.Drawing.Color.Transparent;
            this.btnBLength.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnBLength, 3);
            this.btnBLength.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnBLength.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBLength.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBLength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBLength.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBLength.Image = global::AOG.Properties.Resources.APlusPlusB;
            this.btnBLength.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBLength.Location = new System.Drawing.Point(181, 27);
            this.btnBLength.Name = "btnBLength";
            this.btnBLength.Size = new System.Drawing.Size(90, 52);
            this.btnBLength.TabIndex = 351;
            this.btnBLength.UseVisualStyleBackColor = false;
            this.btnBLength.Click += new System.EventHandler(this.btnBLength_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.tlp1.SetColumnSpan(this.btnExit, 2);
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnExit.Image = global::AOG.Properties.Resources.FileSave;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.Location = new System.Drawing.Point(220, 622);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(63, 70);
            this.btnExit.TabIndex = 0;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tlp1
            // 
            this.tlp1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.tlp1.ColumnCount = 6;
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlp1.Controls.Add(this.btnExit, 4, 7);
            this.tlp1.Controls.Add(this.btnHeadlandOff, 0, 7);
            this.tlp1.Controls.Add(this.btnBShrink, 3, 1);
            this.tlp1.Controls.Add(this.tboxLineName, 0, 2);
            this.tlp1.Controls.Add(this.btnALength, 0, 0);
            this.tlp1.Controls.Add(this.btnAShrink, 0, 1);
            this.tlp1.Controls.Add(this.btnBLength, 3, 0);
            this.tlp1.Controls.Add(this.btnSwapAB, 0, 4);
            this.tlp1.Controls.Add(this.btnCycleForward, 3, 3);
            this.tlp1.Controls.Add(this.btnCycleBackward, 0, 3);
            this.tlp1.Controls.Add(this.cboxIsVisible, 3, 4);
            this.tlp1.Controls.Add(this.btnBuildBnd, 0, 5);
            this.tlp1.Controls.Add(this.btnDeleteBoundary, 3, 5);
            this.tlp1.Location = new System.Drawing.Point(701, 1);
            this.tlp1.Name = "tlp1";
            this.tlp1.RowCount = 8;
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.12968F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.12968F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.051874F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.42857F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.85714F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.285714F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.95965F));
            this.tlp1.Size = new System.Drawing.Size(303, 700);
            this.tlp1.TabIndex = 565;
            // 
            // tboxLineName
            // 
            this.tboxLineName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxLineName.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tboxLineName.CausesValidation = false;
            this.tlp1.SetColumnSpan(this.tboxLineName, 6);
            this.tboxLineName.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxLineName.Location = new System.Drawing.Point(0, 215);
            this.tboxLineName.Margin = new System.Windows.Forms.Padding(0);
            this.tboxLineName.MaxLength = 100;
            this.tboxLineName.Name = "tboxLineName";
            this.tboxLineName.Size = new System.Drawing.Size(303, 36);
            this.tboxLineName.TabIndex = 570;
            this.tboxLineName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSwapAB
            // 
            this.btnSwapAB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSwapAB.BackColor = System.Drawing.Color.Transparent;
            this.tlp1.SetColumnSpan(this.btnSwapAB, 3);
            this.btnSwapAB.FlatAppearance.BorderSize = 0;
            this.btnSwapAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapAB.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwapAB.ForeColor = System.Drawing.Color.White;
            this.btnSwapAB.Image = global::AOG.Properties.Resources.ABSwapPoints;
            this.btnSwapAB.Location = new System.Drawing.Point(47, 369);
            this.btnSwapAB.Name = "btnSwapAB";
            this.btnSwapAB.Size = new System.Drawing.Size(56, 62);
            this.btnSwapAB.TabIndex = 569;
            this.btnSwapAB.UseVisualStyleBackColor = false;
            this.btnSwapAB.Click += new System.EventHandler(this.btnSwapAB_Click);
            // 
            // cboxIsVisible
            // 
            this.cboxIsVisible.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxIsVisible.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsVisible.BackColor = System.Drawing.Color.Transparent;
            this.tlp1.SetColumnSpan(this.cboxIsVisible, 3);
            this.cboxIsVisible.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxIsVisible.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(250)))), ((int)(((byte)(220)))));
            this.cboxIsVisible.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsVisible.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsVisible.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cboxIsVisible.Image = global::AOG.Properties.Resources.TrackVisible;
            this.cboxIsVisible.Location = new System.Drawing.Point(190, 366);
            this.cboxIsVisible.Name = "cboxIsVisible";
            this.cboxIsVisible.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsVisible.Size = new System.Drawing.Size(72, 68);
            this.cboxIsVisible.TabIndex = 571;
            this.cboxIsVisible.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsVisible.UseVisualStyleBackColor = false;
            // 
            // FormBoundaryLines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1006, 726);
            this.ControlBox = false;
            this.Controls.Add(this.tlp1);
            this.Controls.Add(this.oglSelf);
            this.ForeColor = System.Drawing.Color.Black;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1022, 742);
            this.Name = "FormBoundaryLines";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Click 2 points on the Boundary to Begin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBoundaryLines_FormClosing);
            this.Load += new System.EventHandler(this.FormBoundaryLines_Load);
            this.ResizeEnd += new System.EventHandler(this.FormBoundaryLines_ResizeEnd);
            this.tlp1.ResumeLayout(false);
            this.tlp1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl oglSelf;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCycleForward;
        private System.Windows.Forms.Button btnBLength;
        private System.Windows.Forms.Button btnALength;
        private System.Windows.Forms.Button btnDeleteBoundary;
        private System.Windows.Forms.Button btnBuildBnd;
        private System.Windows.Forms.Button btnCycleBackward;
        private System.Windows.Forms.Button btnHeadlandOff;
        private System.Windows.Forms.Button btnAShrink;
        private System.Windows.Forms.Button btnBShrink;
        private System.Windows.Forms.TableLayoutPanel tlp1;
        private System.Windows.Forms.Button btnSwapAB;
        private System.Windows.Forms.TextBox tboxLineName;
        private System.Windows.Forms.CheckBox cboxIsVisible;
    }
}