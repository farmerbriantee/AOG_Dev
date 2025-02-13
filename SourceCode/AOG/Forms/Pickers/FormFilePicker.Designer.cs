namespace AgOpenGPS
{
    partial class FormFilePicker
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
            this.btnByDistance = new System.Windows.Forms.Button();
            this.btnOpenExistingLv = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDeleteField = new System.Windows.Forms.Button();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDistance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chArea = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvLines = new System.Windows.Forms.ListView();
            this.lvLinesJob = new System.Windows.Forms.ListView();
            this.chDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chJobName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDeleteJob = new System.Windows.Forms.Button();
            this.btnNewJob = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnByDistance
            // 
            this.btnByDistance.BackColor = System.Drawing.Color.Transparent;
            this.btnByDistance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnByDistance.Image = global::AgOpenGPS.Properties.Resources.Sort;
            this.btnByDistance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnByDistance.Location = new System.Drawing.Point(798, 257);
            this.btnByDistance.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnByDistance.Name = "btnByDistance";
            this.btnByDistance.Size = new System.Drawing.Size(147, 63);
            this.btnByDistance.TabIndex = 93;
            this.btnByDistance.Text = "Sort";
            this.btnByDistance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnByDistance.UseVisualStyleBackColor = false;
            this.btnByDistance.Click += new System.EventHandler(this.btnByDistance_Click);
            // 
            // btnOpenExistingLv
            // 
            this.btnOpenExistingLv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenExistingLv.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenExistingLv.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnOpenExistingLv.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenExistingLv.Image = global::AgOpenGPS.Properties.Resources.FileOpen;
            this.btnOpenExistingLv.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenExistingLv.Location = new System.Drawing.Point(684, 581);
            this.btnOpenExistingLv.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnOpenExistingLv.Name = "btnOpenExistingLv";
            this.btnOpenExistingLv.Size = new System.Drawing.Size(261, 63);
            this.btnOpenExistingLv.TabIndex = 92;
            this.btnOpenExistingLv.Text = "Use Selected";
            this.btnOpenExistingLv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenExistingLv.UseVisualStyleBackColor = false;
            this.btnOpenExistingLv.Click += new System.EventHandler(this.btnOpenExistingLv_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.Location = new System.Drawing.Point(197, 581);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 63);
            this.btnCancel.TabIndex = 91;
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDeleteField
            // 
            this.btnDeleteField.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnDeleteField.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDeleteField.Image = global::AgOpenGPS.Properties.Resources.Trash;
            this.btnDeleteField.Location = new System.Drawing.Point(25, 257);
            this.btnDeleteField.Name = "btnDeleteField";
            this.btnDeleteField.Size = new System.Drawing.Size(71, 63);
            this.btnDeleteField.TabIndex = 94;
            this.btnDeleteField.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteField.Click += new System.EventHandler(this.btnDeleteField_Click);
            // 
            // chName
            // 
            this.chName.Text = "Field";
            this.chName.Width = 670;
            // 
            // chDistance
            // 
            this.chDistance.Text = "Distance";
            this.chDistance.Width = 140;
            // 
            // chArea
            // 
            this.chArea.Text = "Area";
            this.chArea.Width = 140;
            // 
            // lvLines
            // 
            this.lvLines.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lvLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chDistance,
            this.chArea});
            this.lvLines.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLines.FullRowSelect = true;
            this.lvLines.GridLines = true;
            this.lvLines.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvLines.HideSelection = false;
            this.lvLines.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lvLines.Location = new System.Drawing.Point(4, 3);
            this.lvLines.MultiSelect = false;
            this.lvLines.Name = "lvLines";
            this.lvLines.Size = new System.Drawing.Size(976, 246);
            this.lvLines.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvLines.TabIndex = 86;
            this.lvLines.UseCompatibleStateImageBehavior = false;
            this.lvLines.View = System.Windows.Forms.View.Details;
            this.lvLines.SelectedIndexChanged += new System.EventHandler(this.lvLines_SelectedIndexChanged);
            // 
            // lvLinesJob
            // 
            this.lvLinesJob.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLinesJob.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lvLinesJob.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDate,
            this.chJobName});
            this.lvLinesJob.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLinesJob.FullRowSelect = true;
            this.lvLinesJob.GridLines = true;
            this.lvLinesJob.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvLinesJob.HideSelection = false;
            this.lvLinesJob.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lvLinesJob.Location = new System.Drawing.Point(5, 328);
            this.lvLinesJob.MultiSelect = false;
            this.lvLinesJob.Name = "lvLinesJob";
            this.lvLinesJob.Size = new System.Drawing.Size(976, 244);
            this.lvLinesJob.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvLinesJob.TabIndex = 97;
            this.lvLinesJob.UseCompatibleStateImageBehavior = false;
            this.lvLinesJob.View = System.Windows.Forms.View.Details;
            // 
            // chDate
            // 
            this.chDate.Text = "Date Created";
            this.chDate.Width = 370;
            // 
            // chJobName
            // 
            this.chJobName.Text = "Job";
            this.chJobName.Width = 590;
            // 
            // btnDeleteJob
            // 
            this.btnDeleteJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteJob.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnDeleteJob.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDeleteJob.Image = global::AgOpenGPS.Properties.Resources.Trash;
            this.btnDeleteJob.Location = new System.Drawing.Point(25, 581);
            this.btnDeleteJob.Name = "btnDeleteJob";
            this.btnDeleteJob.Size = new System.Drawing.Size(71, 63);
            this.btnDeleteJob.TabIndex = 101;
            this.btnDeleteJob.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteJob.Click += new System.EventHandler(this.btnDeleteJob_Click);
            // 
            // btnNewJob
            // 
            this.btnNewJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewJob.BackColor = System.Drawing.Color.Transparent;
            this.btnNewJob.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewJob.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnNewJob.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewJob.Location = new System.Drawing.Point(372, 581);
            this.btnNewJob.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnNewJob.Name = "btnNewJob";
            this.btnNewJob.Size = new System.Drawing.Size(277, 63);
            this.btnNewJob.TabIndex = 102;
            this.btnNewJob.Text = "Use + New Job";
            this.btnNewJob.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNewJob.UseVisualStyleBackColor = false;
            this.btnNewJob.Click += new System.EventHandler(this.bntNewJob_Click);
            // 
            // FormFilePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(984, 655);
            this.ControlBox = false;
            this.Controls.Add(this.btnNewJob);
            this.Controls.Add(this.btnDeleteJob);
            this.Controls.Add(this.lvLinesJob);
            this.Controls.Add(this.btnDeleteField);
            this.Controls.Add(this.btnByDistance);
            this.Controls.Add(this.btnOpenExistingLv);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lvLines);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.MinimumSize = new System.Drawing.Size(1000, 536);
            this.Name = "FormFilePicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Field and Job Picker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFilePicker_FormClosing);
            this.Load += new System.EventHandler(this.FormFilePicker_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnByDistance;
        private System.Windows.Forms.Button btnOpenExistingLv;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDeleteField;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chDistance;
        private System.Windows.Forms.ColumnHeader chArea;
        private System.Windows.Forms.ListView lvLines;
        private System.Windows.Forms.ListView lvLinesJob;
        private System.Windows.Forms.ColumnHeader chDate;
        private System.Windows.Forms.ColumnHeader chJobName;
        private System.Windows.Forms.Button btnDeleteJob;
        private System.Windows.Forms.Button btnNewJob;
    }
}