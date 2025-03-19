namespace AOG
{
    partial class FormJobNew
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
            this.btnJobCancel = new System.Windows.Forms.Button();
            this.btnSaveJob = new System.Windows.Forms.Button();
            this.tboxJobName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAddTimeJob = new System.Windows.Forms.Button();
            this.btnAddDateJob = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnJobCancel
            // 
            this.btnJobCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJobCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnJobCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnJobCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnJobCancel.FlatAppearance.BorderSize = 0;
            this.btnJobCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobCancel.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnJobCancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJobCancel.Image = global::AOG.Properties.Resources.Cancel64;
            this.btnJobCancel.Location = new System.Drawing.Point(457, 161);
            this.btnJobCancel.Name = "btnJobCancel";
            this.btnJobCancel.Size = new System.Drawing.Size(77, 79);
            this.btnJobCancel.TabIndex = 4;
            this.btnJobCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnJobCancel.UseVisualStyleBackColor = false;
            this.btnJobCancel.Click += new System.EventHandler(this.btnCancelJob_Click);
            // 
            // btnSave
            // 
            this.btnSaveJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveJob.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveJob.FlatAppearance.BorderSize = 0;
            this.btnSaveJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveJob.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSaveJob.Image = global::AOG.Properties.Resources.OK64;
            this.btnSaveJob.Location = new System.Drawing.Point(573, 162);
            this.btnSaveJob.Name = "btnSaveJob";
            this.btnSaveJob.Size = new System.Drawing.Size(83, 79);
            this.btnSaveJob.TabIndex = 3;
            this.btnSaveJob.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSaveJob.UseVisualStyleBackColor = false;
            this.btnSaveJob.Click += new System.EventHandler(this.btnSaveJob_Click);
            // 
            // tboxJobName
            // 
            this.tboxJobName.BackColor = System.Drawing.Color.AliceBlue;
            this.tboxJobName.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxJobName.Location = new System.Drawing.Point(27, 51);
            this.tboxJobName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tboxJobName.Name = "tboxJobName";
            this.tboxJobName.Size = new System.Drawing.Size(634, 36);
            this.tboxJobName.TabIndex = 0;
            this.tboxJobName.Click += new System.EventHandler(this.tboxJobName_Click);
            this.tboxJobName.TextChanged += new System.EventHandler(this.tboxJobName_TextChanged);
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
            // btnAddTimeJob
            // 
            this.btnAddTimeJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTimeJob.BackColor = System.Drawing.Color.Transparent;
            this.btnAddTimeJob.FlatAppearance.BorderSize = 0;
            this.btnAddTimeJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTimeJob.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddTimeJob.Image = global::AOG.Properties.Resources.JobNameTime;
            this.btnAddTimeJob.Location = new System.Drawing.Point(232, 117);
            this.btnAddTimeJob.Name = "btnAddTimeJob";
            this.btnAddTimeJob.Size = new System.Drawing.Size(83, 79);
            this.btnAddTimeJob.TabIndex = 151;
            this.btnAddTimeJob.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddTimeJob.UseVisualStyleBackColor = false;
            this.btnAddTimeJob.Click += new System.EventHandler(this.btnAddTimeJob_Click);
            // 
            // btnAddDateJob
            // 
            this.btnAddDateJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddDateJob.BackColor = System.Drawing.Color.Transparent;
            this.btnAddDateJob.FlatAppearance.BorderSize = 0;
            this.btnAddDateJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddDateJob.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddDateJob.Image = global::AOG.Properties.Resources.JobNameCalendar;
            this.btnAddDateJob.Location = new System.Drawing.Point(75, 116);
            this.btnAddDateJob.Name = "btnAddDateJob";
            this.btnAddDateJob.Size = new System.Drawing.Size(83, 79);
            this.btnAddDateJob.TabIndex = 152;
            this.btnAddDateJob.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddDateJob.UseVisualStyleBackColor = false;
            this.btnAddDateJob.Click += new System.EventHandler(this.btnAddDateJob_Click);
            // 
            // FormJobNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(676, 260);
            this.ControlBox = false;
            this.Controls.Add(this.btnAddDateJob);
            this.Controls.Add(this.btnAddTimeJob);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tboxJobName);
            this.Controls.Add(this.btnSaveJob);
            this.Controls.Add(this.btnJobCancel);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormJobNew";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New Field ";
            this.Load += new System.EventHandler(this.FormJobNew_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnJobCancel;
        private System.Windows.Forms.Button btnSaveJob;
        private System.Windows.Forms.TextBox tboxJobName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAddTimeJob;
        private System.Windows.Forms.Button btnAddDateJob;
    }
}