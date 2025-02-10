﻿namespace AgOpenGPS
{
    partial class FormJob
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFromKML = new System.Windows.Forms.Button();
            this.btnFromISOXML = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnJobOpen = new System.Windows.Forms.Button();
            this.btnJobClose = new System.Windows.Forms.Button();
            this.btnInField = new System.Windows.Forms.Button();
            this.btnJobResume = new System.Windows.Forms.Button();
            this.btnFromExisting = new System.Windows.Forms.Button();
            this.btnJobNew = new System.Windows.Forms.Button();
            this.lblResumeField = new System.Windows.Forms.Label();
            this.btnDeleteAB = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnFromKML, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnFromISOXML, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnJobOpen, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnJobClose, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnInField, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnJobResume, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnFromExisting, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnJobNew, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(637, 469);
            this.tableLayoutPanel1.TabIndex = 106;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(310, 351);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 118);
            this.label4.TabIndex = 111;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(310, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 117);
            this.label3.TabIndex = 110;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(310, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 117);
            this.label2.TabIndex = 109;
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
            this.btnFromKML.Location = new System.Drawing.Point(5, 121);
            this.btnFromKML.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFromKML.Name = "btnFromKML";
            this.btnFromKML.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFromKML.Size = new System.Drawing.Size(297, 109);
            this.btnFromKML.TabIndex = 91;
            this.btnFromKML.Text = "From KML";
            this.btnFromKML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFromKML.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFromKML.UseVisualStyleBackColor = false;
            this.btnFromKML.Click += new System.EventHandler(this.btnFromKML_Click);
            // 
            // btnFromISOXML
            // 
            this.btnFromISOXML.BackColor = System.Drawing.Color.Transparent;
            this.btnFromISOXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFromISOXML.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFromISOXML.FlatAppearance.BorderSize = 0;
            this.btnFromISOXML.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFromISOXML.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFromISOXML.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFromISOXML.Image = global::AgOpenGPS.Properties.Resources.ISOXML;
            this.btnFromISOXML.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFromISOXML.Location = new System.Drawing.Point(5, 4);
            this.btnFromISOXML.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFromISOXML.Name = "btnFromISOXML";
            this.btnFromISOXML.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFromISOXML.Size = new System.Drawing.Size(297, 109);
            this.btnFromISOXML.TabIndex = 107;
            this.btnFromISOXML.Text = "ISO-XML";
            this.btnFromISOXML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFromISOXML.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFromISOXML.UseVisualStyleBackColor = false;
            this.btnFromISOXML.Click += new System.EventHandler(this.btnFromISOXML_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(310, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 117);
            this.label1.TabIndex = 108;
            // 
            // btnJobOpen
            // 
            this.btnJobOpen.BackColor = System.Drawing.Color.Transparent;
            this.btnJobOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJobOpen.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnJobOpen.FlatAppearance.BorderSize = 0;
            this.btnJobOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobOpen.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJobOpen.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJobOpen.Image = global::AgOpenGPS.Properties.Resources.FileOpen;
            this.btnJobOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobOpen.Location = new System.Drawing.Point(335, 238);
            this.btnJobOpen.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnJobOpen.Name = "btnJobOpen";
            this.btnJobOpen.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnJobOpen.Size = new System.Drawing.Size(297, 109);
            this.btnJobOpen.TabIndex = 3;
            this.btnJobOpen.Text = "Open";
            this.btnJobOpen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnJobOpen.UseVisualStyleBackColor = false;
            this.btnJobOpen.Click += new System.EventHandler(this.btnJobOpen_Click);
            // 
            // btnJobClose
            // 
            this.btnJobClose.AllowDrop = true;
            this.btnJobClose.BackColor = System.Drawing.Color.Transparent;
            this.btnJobClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJobClose.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnJobClose.FlatAppearance.BorderSize = 0;
            this.btnJobClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobClose.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJobClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJobClose.Image = global::AgOpenGPS.Properties.Resources.FileClose;
            this.btnJobClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobClose.Location = new System.Drawing.Point(335, 4);
            this.btnJobClose.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnJobClose.Name = "btnJobClose";
            this.btnJobClose.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnJobClose.Size = new System.Drawing.Size(297, 109);
            this.btnJobClose.TabIndex = 105;
            this.btnJobClose.Text = "Close";
            this.btnJobClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnJobClose.UseVisualStyleBackColor = false;
            this.btnJobClose.Click += new System.EventHandler(this.btnJobClose_Click);
            // 
            // btnInField
            // 
            this.btnInField.BackColor = System.Drawing.Color.Transparent;
            this.btnInField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInField.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnInField.FlatAppearance.BorderSize = 0;
            this.btnInField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInField.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInField.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnInField.Image = global::AgOpenGPS.Properties.Resources.AutoManualIsAuto;
            this.btnInField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInField.Location = new System.Drawing.Point(335, 121);
            this.btnInField.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnInField.Name = "btnInField";
            this.btnInField.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnInField.Size = new System.Drawing.Size(297, 109);
            this.btnInField.TabIndex = 89;
            this.btnInField.Text = "Drive In";
            this.btnInField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInField.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInField.UseVisualStyleBackColor = false;
            this.btnInField.Click += new System.EventHandler(this.btnInField_Click);
            // 
            // btnJobResume
            // 
            this.btnJobResume.BackColor = System.Drawing.Color.Transparent;
            this.btnJobResume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJobResume.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnJobResume.FlatAppearance.BorderSize = 0;
            this.btnJobResume.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobResume.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJobResume.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJobResume.Image = global::AgOpenGPS.Properties.Resources.FilePrevious;
            this.btnJobResume.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobResume.Location = new System.Drawing.Point(335, 355);
            this.btnJobResume.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnJobResume.Name = "btnJobResume";
            this.btnJobResume.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnJobResume.Size = new System.Drawing.Size(297, 110);
            this.btnJobResume.TabIndex = 1;
            this.btnJobResume.Text = "Resume";
            this.btnJobResume.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobResume.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnJobResume.UseVisualStyleBackColor = false;
            this.btnJobResume.Click += new System.EventHandler(this.btnJobResume_Click);
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
            this.btnFromExisting.Location = new System.Drawing.Point(5, 238);
            this.btnFromExisting.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFromExisting.Name = "btnFromExisting";
            this.btnFromExisting.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFromExisting.Size = new System.Drawing.Size(297, 109);
            this.btnFromExisting.TabIndex = 104;
            this.btnFromExisting.Text = "Existing";
            this.btnFromExisting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFromExisting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFromExisting.UseVisualStyleBackColor = false;
            this.btnFromExisting.Click += new System.EventHandler(this.btnFromExisting_Click);
            // 
            // btnJobNew
            // 
            this.btnJobNew.BackColor = System.Drawing.Color.Transparent;
            this.btnJobNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJobNew.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnJobNew.FlatAppearance.BorderSize = 0;
            this.btnJobNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobNew.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJobNew.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJobNew.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnJobNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobNew.Location = new System.Drawing.Point(5, 355);
            this.btnJobNew.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnJobNew.Name = "btnJobNew";
            this.btnJobNew.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnJobNew.Size = new System.Drawing.Size(297, 110);
            this.btnJobNew.TabIndex = 2;
            this.btnJobNew.Text = "New Field";
            this.btnJobNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnJobNew.UseVisualStyleBackColor = false;
            this.btnJobNew.Click += new System.EventHandler(this.btnJobNew_Click);
            // 
            // lblResumeField
            // 
            this.lblResumeField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResumeField.BackColor = System.Drawing.Color.Transparent;
            this.lblResumeField.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumeField.ForeColor = System.Drawing.Color.Black;
            this.lblResumeField.Location = new System.Drawing.Point(9, 481);
            this.lblResumeField.Name = "lblResumeField";
            this.lblResumeField.Size = new System.Drawing.Size(500, 28);
            this.lblResumeField.TabIndex = 106;
            this.lblResumeField.Text = "Previous Field";
            this.lblResumeField.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnDeleteAB
            // 
            this.btnDeleteAB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteAB.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteAB.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDeleteAB.FlatAppearance.BorderSize = 0;
            this.btnDeleteAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteAB.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnDeleteAB.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDeleteAB.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnDeleteAB.Location = new System.Drawing.Point(542, 592);
            this.btnDeleteAB.Name = "btnDeleteAB";
            this.btnDeleteAB.Size = new System.Drawing.Size(107, 57);
            this.btnDeleteAB.TabIndex = 4;
            this.btnDeleteAB.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteAB.UseVisualStyleBackColor = false;
            this.btnDeleteAB.Click += new System.EventHandler(this.btnDeleteAB_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(14, 548);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.button1.Size = new System.Drawing.Size(206, 72);
            this.button1.TabIndex = 107;
            this.button1.Text = "New Job";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button2.Image = global::AgOpenGPS.Properties.Resources.FilePrevious;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(250, 548);
            this.button2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button2.Name = "button2";
            this.button2.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.button2.Size = new System.Drawing.Size(206, 72);
            this.button2.TabIndex = 108;
            this.button2.Text = "Resume Job";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(4, 624);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(500, 28);
            this.label5.TabIndex = 109;
            this.label5.Text = "Previous Job";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(656, 656);
            this.ControlBox = false;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnDeleteAB);
            this.Controls.Add(this.lblResumeField);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Location = new System.Drawing.Point(200, 200);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 530);
            this.Name = "FormJob";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormJob_FormClosing);
            this.Load += new System.EventHandler(this.FormJob_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnJobOpen;
        private System.Windows.Forms.Button btnJobClose;
        private System.Windows.Forms.Button btnJobResume;
        private System.Windows.Forms.Label lblResumeField;
        private System.Windows.Forms.Button btnFromExisting;
        private System.Windows.Forms.Button btnJobNew;
        private System.Windows.Forms.Button btnInField;
        private System.Windows.Forms.Button btnFromKML;
        private System.Windows.Forms.Button btnFromISOXML;
        private System.Windows.Forms.Button btnDeleteAB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
    }
}