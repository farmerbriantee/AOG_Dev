namespace AOG
{
    partial class FormFieldNew
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
            this.tboxFieldName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddTime = new System.Windows.Forms.Button();
            this.btnAddDate = new System.Windows.Forms.Button();
            this.btnAddDateJob = new System.Windows.Forms.Button();
            this.btnAddTimeJob = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tboxJobName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnJobCancel = new System.Windows.Forms.Button();
            this.btnFieldNew = new System.Windows.Forms.Button();
            this.btnJobNew = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tboxFieldName
            // 
            this.tboxFieldName.BackColor = System.Drawing.Color.AliceBlue;
            this.tboxFieldName.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxFieldName.Location = new System.Drawing.Point(27, 51);
            this.tboxFieldName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tboxFieldName.Name = "tboxFieldName";
            this.tboxFieldName.Size = new System.Drawing.Size(634, 36);
            this.tboxFieldName.TabIndex = 0;
            this.tboxFieldName.Click += new System.EventHandler(this.tboxFieldName_Click);
            this.tboxFieldName.TextChanged += new System.EventHandler(this.tboxFieldName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(201, 140);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 23);
            this.label3.TabIndex = 150;
            this.label3.Text = "+";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(44, 140);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 23);
            this.label2.TabIndex = 149;
            this.label2.Text = "+";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(34, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enter Field Name";
            // 
            // btnAddTime
            // 
            this.btnAddTime.BackColor = System.Drawing.Color.Transparent;
            this.btnAddTime.FlatAppearance.BorderSize = 0;
            this.btnAddTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTime.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddTime.Image = global::AOG.Properties.Resources.JobNameTime;
            this.btnAddTime.Location = new System.Drawing.Point(232, 117);
            this.btnAddTime.Name = "btnAddTime";
            this.btnAddTime.Size = new System.Drawing.Size(83, 79);
            this.btnAddTime.TabIndex = 151;
            this.btnAddTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddTime.UseVisualStyleBackColor = false;
            this.btnAddTime.Click += new System.EventHandler(this.btnAddTime_Click);
            // 
            // btnAddDate
            // 
            this.btnAddDate.BackColor = System.Drawing.Color.Transparent;
            this.btnAddDate.FlatAppearance.BorderSize = 0;
            this.btnAddDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddDate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddDate.Image = global::AOG.Properties.Resources.JobNameCalendar;
            this.btnAddDate.Location = new System.Drawing.Point(75, 116);
            this.btnAddDate.Name = "btnAddDate";
            this.btnAddDate.Size = new System.Drawing.Size(83, 79);
            this.btnAddDate.TabIndex = 152;
            this.btnAddDate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddDate.UseVisualStyleBackColor = false;
            this.btnAddDate.Click += new System.EventHandler(this.btnAddDate_Click);
            // 
            // btnAddDateJob
            // 
            this.btnAddDateJob.BackColor = System.Drawing.Color.Transparent;
            this.btnAddDateJob.FlatAppearance.BorderSize = 0;
            this.btnAddDateJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddDateJob.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddDateJob.Image = global::AOG.Properties.Resources.JobNameCalendar;
            this.btnAddDateJob.Location = new System.Drawing.Point(75, 345);
            this.btnAddDateJob.Name = "btnAddDateJob";
            this.btnAddDateJob.Size = new System.Drawing.Size(83, 79);
            this.btnAddDateJob.TabIndex = 160;
            this.btnAddDateJob.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddDateJob.UseVisualStyleBackColor = false;
            this.btnAddDateJob.Click += new System.EventHandler(this.btnAddDateJob_Click);
            // 
            // btnAddTimeJob
            // 
            this.btnAddTimeJob.BackColor = System.Drawing.Color.Transparent;
            this.btnAddTimeJob.FlatAppearance.BorderSize = 0;
            this.btnAddTimeJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTimeJob.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddTimeJob.Image = global::AOG.Properties.Resources.JobNameTime;
            this.btnAddTimeJob.Location = new System.Drawing.Point(232, 346);
            this.btnAddTimeJob.Name = "btnAddTimeJob";
            this.btnAddTimeJob.Size = new System.Drawing.Size(83, 79);
            this.btnAddTimeJob.TabIndex = 159;
            this.btnAddTimeJob.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddTimeJob.UseVisualStyleBackColor = false;
            this.btnAddTimeJob.Click += new System.EventHandler(this.btnAddTimeJob_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(201, 369);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 23);
            this.label4.TabIndex = 158;
            this.label4.Text = "+";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(44, 369);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 23);
            this.label5.TabIndex = 157;
            this.label5.Text = "+";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(34, 254);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(162, 23);
            this.label6.TabIndex = 155;
            this.label6.Text = "Enter Job Name";
            // 
            // tboxJobName
            // 
            this.tboxJobName.BackColor = System.Drawing.Color.AliceBlue;
            this.tboxJobName.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxJobName.Location = new System.Drawing.Point(27, 280);
            this.tboxJobName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tboxJobName.Name = "tboxJobName";
            this.tboxJobName.Size = new System.Drawing.Size(634, 36);
            this.tboxJobName.TabIndex = 153;
            this.tboxJobName.Click += new System.EventHandler(this.tboxJobName_Click);
            this.tboxJobName.TextChanged += new System.EventHandler(this.tboxJobName_TextChanged);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSave.Image = global::AOG.Properties.Resources.OK64;
            this.btnSave.Location = new System.Drawing.Point(583, 480);
            this.btnSave.Name = "btnSaveJob";
            this.btnSave.Size = new System.Drawing.Size(83, 59);
            this.btnSave.TabIndex = 154;
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnJobCancel
            // 
            this.btnJobCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnJobCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnJobCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnJobCancel.FlatAppearance.BorderSize = 0;
            this.btnJobCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobCancel.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnJobCancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJobCancel.Image = global::AOG.Properties.Resources.Cancel64;
            this.btnJobCancel.Location = new System.Drawing.Point(5, 482);
            this.btnJobCancel.Name = "btnJobCancel";
            this.btnJobCancel.Size = new System.Drawing.Size(77, 59);
            this.btnJobCancel.TabIndex = 156;
            this.btnJobCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnJobCancel.UseVisualStyleBackColor = false;
            this.btnJobCancel.Click += new System.EventHandler(this.btnCancelJob_Click);
            // 
            // btnFieldNew
            // 
            this.btnFieldNew.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldNew.Enabled = false;
            this.btnFieldNew.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFieldNew.FlatAppearance.BorderSize = 0;
            this.btnFieldNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldNew.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFieldNew.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFieldNew.Image = global::AOG.Properties.Resources.FileNew;
            this.btnFieldNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldNew.Location = new System.Drawing.Point(401, 122);
            this.btnFieldNew.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFieldNew.Name = "btnFieldNew";
            this.btnFieldNew.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFieldNew.Size = new System.Drawing.Size(223, 74);
            this.btnFieldNew.TabIndex = 161;
            this.btnFieldNew.Text = "New Field";
            this.btnFieldNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFieldNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFieldNew.UseVisualStyleBackColor = false;
            this.btnFieldNew.Click += new System.EventHandler(this.btnFieldNew_Click);
            // 
            // btnJobNew
            // 
            this.btnJobNew.BackColor = System.Drawing.Color.Transparent;
            this.btnJobNew.Enabled = false;
            this.btnJobNew.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnJobNew.FlatAppearance.BorderSize = 0;
            this.btnJobNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobNew.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJobNew.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJobNew.Image = global::AOG.Properties.Resources.FileNew;
            this.btnJobNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobNew.Location = new System.Drawing.Point(401, 351);
            this.btnJobNew.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnJobNew.Name = "btnJobNew";
            this.btnJobNew.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnJobNew.Size = new System.Drawing.Size(206, 74);
            this.btnJobNew.TabIndex = 162;
            this.btnJobNew.Text = "New Job";
            this.btnJobNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJobNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnJobNew.UseVisualStyleBackColor = false;
            this.btnJobNew.Click += new System.EventHandler(this.btnJobNew_Click);
            // 
            // FormFieldNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(672, 546);
            this.ControlBox = false;
            this.Controls.Add(this.btnJobNew);
            this.Controls.Add(this.btnFieldNew);
            this.Controls.Add(this.btnAddDateJob);
            this.Controls.Add(this.btnAddTimeJob);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tboxJobName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnJobCancel);
            this.Controls.Add(this.btnAddDate);
            this.Controls.Add(this.btnAddTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tboxFieldName);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormFieldNew";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New Field ";
            this.Load += new System.EventHandler(this.FormFieldDir_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tboxFieldName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddTime;
        private System.Windows.Forms.Button btnAddDate;
        private System.Windows.Forms.Button btnAddDateJob;
        private System.Windows.Forms.Button btnAddTimeJob;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tboxJobName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnJobCancel;
        private System.Windows.Forms.Button btnFieldNew;
        private System.Windows.Forms.Button btnJobNew;
    }
}