﻿namespace AgOpenGPS
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFromKML = new System.Windows.Forms.Button();
            this.btnFromISOXML = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFieldOpen = new System.Windows.Forms.Button();
            this.btnFieldClose = new System.Windows.Forms.Button();
            this.btnInField = new System.Windows.Forms.Button();
            this.btnFieldResume = new System.Windows.Forms.Button();
            this.btnFromExisting = new System.Windows.Forms.Button();
            this.btnFieldNew = new System.Windows.Forms.Button();
            this.lblResumeField = new System.Windows.Forms.Label();
            this.btnDeleteAB = new System.Windows.Forms.Button();
            this.btnJobNew = new System.Windows.Forms.Button();
            this.btnJobResume = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.Controls.Add(this.btnFieldOpen, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnFieldClose, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnInField, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnFieldResume, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnFromExisting, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnFieldNew, 0, 3);
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
            // btnFieldOpen
            // 
            this.btnFieldOpen.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFieldOpen.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFieldOpen.FlatAppearance.BorderSize = 0;
            this.btnFieldOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldOpen.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFieldOpen.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFieldOpen.Image = global::AgOpenGPS.Properties.Resources.FileOpen;
            this.btnFieldOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldOpen.Location = new System.Drawing.Point(335, 238);
            this.btnFieldOpen.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFieldOpen.Name = "btnFieldOpen";
            this.btnFieldOpen.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFieldOpen.Size = new System.Drawing.Size(297, 109);
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
            this.btnFieldClose.Location = new System.Drawing.Point(335, 4);
            this.btnFieldClose.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFieldClose.Name = "btnFieldClose";
            this.btnFieldClose.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFieldClose.Size = new System.Drawing.Size(297, 109);
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
            // btnFieldResume
            // 
            this.btnFieldResume.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldResume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFieldResume.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFieldResume.FlatAppearance.BorderSize = 0;
            this.btnFieldResume.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldResume.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFieldResume.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFieldResume.Image = global::AgOpenGPS.Properties.Resources.FilePrevious;
            this.btnFieldResume.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldResume.Location = new System.Drawing.Point(335, 355);
            this.btnFieldResume.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFieldResume.Name = "btnFieldResume";
            this.btnFieldResume.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFieldResume.Size = new System.Drawing.Size(297, 110);
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
            this.btnFieldNew.Location = new System.Drawing.Point(5, 355);
            this.btnFieldNew.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFieldNew.Name = "btnFieldNew";
            this.btnFieldNew.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFieldNew.Size = new System.Drawing.Size(297, 110);
            this.btnFieldNew.TabIndex = 2;
            this.btnFieldNew.Text = "New Field";
            this.btnFieldNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFieldNew.UseVisualStyleBackColor = false;
            this.btnFieldNew.Click += new System.EventHandler(this.btnFieldNew_Click);
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
            // btnJobNew
            // 
            this.btnJobNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJobNew.BackColor = System.Drawing.Color.Transparent;
            this.btnJobNew.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnJobNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobNew.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJobNew.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJobNew.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnJobNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobNew.Location = new System.Drawing.Point(14, 548);
            this.btnJobNew.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnJobNew.Name = "btnJobNew";
            this.btnJobNew.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnJobNew.Size = new System.Drawing.Size(206, 72);
            this.btnJobNew.TabIndex = 107;
            this.btnJobNew.Text = "New Job";
            this.btnJobNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnJobNew.UseVisualStyleBackColor = false;
            // 
            // btnJobResume
            // 
            this.btnJobResume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJobResume.BackColor = System.Drawing.Color.Transparent;
            this.btnJobResume.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnJobResume.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobResume.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJobResume.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJobResume.Image = global::AgOpenGPS.Properties.Resources.FilePrevious;
            this.btnJobResume.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobResume.Location = new System.Drawing.Point(250, 548);
            this.btnJobResume.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnJobResume.Name = "btnJobResume";
            this.btnJobResume.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnJobResume.Size = new System.Drawing.Size(206, 72);
            this.btnJobResume.TabIndex = 108;
            this.btnJobResume.Text = "Resume Job";
            this.btnJobResume.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobResume.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnJobResume.UseVisualStyleBackColor = false;
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
            this.Controls.Add(this.btnJobResume);
            this.Controls.Add(this.btnJobNew);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormField_FormClosing);
            this.Load += new System.EventHandler(this.FormField_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.Button btnFromISOXML;
        private System.Windows.Forms.Button btnDeleteAB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnJobNew;
        private System.Windows.Forms.Button btnJobResume;
        private System.Windows.Forms.Label label5;
    }
}