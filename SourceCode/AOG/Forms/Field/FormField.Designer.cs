namespace AgOpenGPS
{
    partial class FormField
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblResumeJob = new System.Windows.Forms.Label();
            this.btnFromKML = new System.Windows.Forms.Button();
            this.btnFieldOpen = new System.Windows.Forms.Button();
            this.btnFieldClose = new System.Windows.Forms.Button();
            this.btnInField = new System.Windows.Forms.Button();
            this.btnFieldResume = new System.Windows.Forms.Button();
            this.btnFromExisting = new System.Windows.Forms.Button();
            this.btnFieldNew = new System.Windows.Forms.Button();
            this.btnJobOpen = new System.Windows.Forms.Button();
            this.btnJobClose = new System.Windows.Forms.Button();
            this.btnJobNew = new System.Windows.Forms.Button();
            this.lblResumeField = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.lblResumeJob, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblResumeField, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnFromKML, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnFieldClose, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFromExisting, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnFieldNew, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnJobOpen, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnJobClose, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnJobNew, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnInField, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFieldOpen, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnFieldResume, 2, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(740, 436);
            this.tableLayoutPanel1.TabIndex = 106;
            // 
            // lblResumeJob
            // 
            this.lblResumeJob.BackColor = System.Drawing.Color.Transparent;
            this.lblResumeJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResumeJob.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumeJob.ForeColor = System.Drawing.Color.Black;
            this.lblResumeJob.Location = new System.Drawing.Point(511, 327);
            this.lblResumeJob.Name = "lblResumeJob";
            this.lblResumeJob.Size = new System.Drawing.Size(226, 109);
            this.lblResumeJob.TabIndex = 109;
            this.lblResumeJob.Text = "Previous Job";
            this.lblResumeJob.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFromKML
            // 
            this.btnFromKML.BackColor = System.Drawing.Color.Transparent;
            this.btnFromKML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFromKML.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFromKML.FlatAppearance.BorderSize = 0;
            this.btnFromKML.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFromKML.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFromKML.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFromKML.Image = global::AgOpenGPS.Properties.Resources.BoundaryLoadFromGE;
            this.btnFromKML.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFromKML.Location = new System.Drawing.Point(5, 113);
            this.btnFromKML.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFromKML.Name = "btnFromKML";
            this.btnFromKML.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFromKML.Size = new System.Drawing.Size(221, 101);
            this.btnFromKML.TabIndex = 91;
            this.btnFromKML.Text = "From KML";
            this.btnFromKML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFromKML.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFromKML.UseVisualStyleBackColor = false;
            this.btnFromKML.Click += new System.EventHandler(this.btnFromKML_Click);
            // 
            // btnFieldOpen
            // 
            this.btnFieldOpen.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldOpen.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFieldOpen.FlatAppearance.BorderSize = 0;
            this.btnFieldOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldOpen.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFieldOpen.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFieldOpen.Image = global::AgOpenGPS.Properties.Resources.FileOpen;
            this.btnFieldOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldOpen.Location = new System.Drawing.Point(259, 113);
            this.btnFieldOpen.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFieldOpen.Name = "btnFieldOpen";
            this.btnFieldOpen.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFieldOpen.Size = new System.Drawing.Size(221, 101);
            this.btnFieldOpen.TabIndex = 3;
            this.btnFieldOpen.Text = "Open";
            this.btnFieldOpen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFieldOpen.UseVisualStyleBackColor = false;
            this.btnFieldOpen.Click += new System.EventHandler(this.btnFieldOpen_Click);
            // 
            // btnFieldClose
            // 
            this.btnFieldClose.AllowDrop = true;
            this.btnFieldClose.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFieldClose.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFieldClose.FlatAppearance.BorderSize = 0;
            this.btnFieldClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldClose.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFieldClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFieldClose.Image = global::AgOpenGPS.Properties.Resources.FileClose;
            this.btnFieldClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldClose.Location = new System.Drawing.Point(259, 4);
            this.btnFieldClose.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFieldClose.Name = "btnFieldClose";
            this.btnFieldClose.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFieldClose.Size = new System.Drawing.Size(221, 101);
            this.btnFieldClose.TabIndex = 105;
            this.btnFieldClose.Text = "Close";
            this.btnFieldClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFieldClose.UseVisualStyleBackColor = false;
            this.btnFieldClose.Click += new System.EventHandler(this.btnFieldClose_Click);
            // 
            // btnInField
            // 
            this.btnInField.BackColor = System.Drawing.Color.Transparent;
            this.btnInField.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnInField.FlatAppearance.BorderSize = 0;
            this.btnInField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInField.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInField.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnInField.Image = global::AgOpenGPS.Properties.Resources.AutoManualIsAuto;
            this.btnInField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInField.Location = new System.Drawing.Point(5, 4);
            this.btnInField.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnInField.Name = "btnInField";
            this.btnInField.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnInField.Size = new System.Drawing.Size(221, 101);
            this.btnInField.TabIndex = 89;
            this.btnInField.Text = "Drive In";
            this.btnInField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInField.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInField.UseVisualStyleBackColor = false;
            this.btnInField.Click += new System.EventHandler(this.btnInField_Click);
            // 
            // btnFieldResume
            // 
            this.btnFieldResume.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldResume.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFieldResume.FlatAppearance.BorderSize = 0;
            this.btnFieldResume.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldResume.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFieldResume.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFieldResume.Image = global::AgOpenGPS.Properties.Resources.FilePrevious;
            this.btnFieldResume.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldResume.Location = new System.Drawing.Point(259, 222);
            this.btnFieldResume.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFieldResume.Name = "btnFieldResume";
            this.btnFieldResume.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFieldResume.Size = new System.Drawing.Size(221, 101);
            this.btnFieldResume.TabIndex = 1;
            this.btnFieldResume.Text = "Resume";
            this.btnFieldResume.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldResume.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFieldResume.UseVisualStyleBackColor = false;
            this.btnFieldResume.Click += new System.EventHandler(this.btnFieldResume_Click);
            // 
            // btnFromExisting
            // 
            this.btnFromExisting.BackColor = System.Drawing.Color.Transparent;
            this.btnFromExisting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFromExisting.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFromExisting.FlatAppearance.BorderSize = 0;
            this.btnFromExisting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFromExisting.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFromExisting.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFromExisting.Image = global::AgOpenGPS.Properties.Resources.FileExisting;
            this.btnFromExisting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFromExisting.Location = new System.Drawing.Point(5, 222);
            this.btnFromExisting.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFromExisting.Name = "btnFromExisting";
            this.btnFromExisting.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFromExisting.Size = new System.Drawing.Size(221, 101);
            this.btnFromExisting.TabIndex = 104;
            this.btnFromExisting.Text = "Existing";
            this.btnFromExisting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFromExisting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFromExisting.UseVisualStyleBackColor = false;
            this.btnFromExisting.Click += new System.EventHandler(this.btnFromExisting_Click);
            // 
            // btnFieldNew
            // 
            this.btnFieldNew.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFieldNew.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFieldNew.FlatAppearance.BorderSize = 0;
            this.btnFieldNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldNew.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFieldNew.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFieldNew.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnFieldNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldNew.Location = new System.Drawing.Point(5, 331);
            this.btnFieldNew.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFieldNew.Name = "btnFieldNew";
            this.btnFieldNew.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFieldNew.Size = new System.Drawing.Size(221, 101);
            this.btnFieldNew.TabIndex = 2;
            this.btnFieldNew.Text = "New Field";
            this.btnFieldNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFieldNew.UseVisualStyleBackColor = false;
            this.btnFieldNew.Click += new System.EventHandler(this.btnFieldNew_Click);
            // 
            // btnJobOpen
            // 
            this.btnJobOpen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnJobOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(254)))), ((int)(((byte)(230)))));
            this.btnJobOpen.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnJobOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobOpen.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJobOpen.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJobOpen.Image = global::AgOpenGPS.Properties.Resources.FilePrevious;
            this.btnJobOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobOpen.Location = new System.Drawing.Point(514, 237);
            this.btnJobOpen.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnJobOpen.Name = "btnJobOpen";
            this.btnJobOpen.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnJobOpen.Size = new System.Drawing.Size(220, 71);
            this.btnJobOpen.TabIndex = 108;
            this.btnJobOpen.Text = "Open Job";
            this.btnJobOpen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnJobOpen.UseVisualStyleBackColor = false;
            this.btnJobOpen.Click += new System.EventHandler(this.btnJobOpen_Click);
            // 
            // btnJobClose
            // 
            this.btnJobClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnJobClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnJobClose.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnJobClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobClose.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJobClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJobClose.Image = global::AgOpenGPS.Properties.Resources.FilePrevious;
            this.btnJobClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobClose.Location = new System.Drawing.Point(514, 19);
            this.btnJobClose.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnJobClose.Name = "btnJobClose";
            this.btnJobClose.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnJobClose.Size = new System.Drawing.Size(220, 71);
            this.btnJobClose.TabIndex = 112;
            this.btnJobClose.Text = "Close Job";
            this.btnJobClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnJobClose.UseVisualStyleBackColor = false;
            this.btnJobClose.Click += new System.EventHandler(this.btnJobClose_Click);
            // 
            // btnJobNew
            // 
            this.btnJobNew.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnJobNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(254)))), ((int)(((byte)(230)))));
            this.btnJobNew.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnJobNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobNew.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJobNew.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJobNew.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnJobNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobNew.Location = new System.Drawing.Point(514, 128);
            this.btnJobNew.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnJobNew.Name = "btnJobNew";
            this.btnJobNew.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnJobNew.Size = new System.Drawing.Size(220, 71);
            this.btnJobNew.TabIndex = 107;
            this.btnJobNew.Text = "New Job";
            this.btnJobNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnJobNew.UseVisualStyleBackColor = false;
            this.btnJobNew.Click += new System.EventHandler(this.btnJobNew_Click);
            // 
            // lblResumeField
            // 
            this.lblResumeField.BackColor = System.Drawing.Color.Transparent;
            this.lblResumeField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResumeField.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumeField.ForeColor = System.Drawing.Color.Black;
            this.lblResumeField.Location = new System.Drawing.Point(257, 327);
            this.lblResumeField.Name = "lblResumeField";
            this.lblResumeField.Size = new System.Drawing.Size(225, 109);
            this.lblResumeField.TabIndex = 106;
            this.lblResumeField.Text = "Previous Field";
            this.lblResumeField.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.Location = new System.Drawing.Point(642, 452);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 57);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(756, 513);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Location = new System.Drawing.Point(200, 200);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(776, 533);
            this.Name = "FormField";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormField_FormClosing);
            this.Load += new System.EventHandler(this.FormField_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnFieldOpen;
        private System.Windows.Forms.Button btnFieldClose;
        private System.Windows.Forms.Button btnFieldResume;
        private System.Windows.Forms.Label lblResumeField;
        private System.Windows.Forms.Button btnFromExisting;
        private System.Windows.Forms.Button btnFieldNew;
        private System.Windows.Forms.Button btnInField;
        private System.Windows.Forms.Button btnFromKML;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnJobNew;
        private System.Windows.Forms.Button btnJobOpen;
        private System.Windows.Forms.Label lblResumeJob;
        private System.Windows.Forms.Button btnJobClose;
    }
}