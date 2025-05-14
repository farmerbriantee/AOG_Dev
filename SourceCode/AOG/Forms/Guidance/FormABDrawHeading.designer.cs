namespace AOG
{
    partial class FormABDrawHeading
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
            this.btnSerialOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn0 = new System.Windows.Forms.Button();
            this.btn90 = new System.Windows.Forms.Button();
            this.btn180 = new System.Windows.Forms.Button();
            this.btn270 = new System.Windows.Forms.Button();
            this.nudHeading = new AOG.NudlessNumericUpDown();
            this.SuspendLayout();
            // 
            // btnSerialOK
            // 
            this.btnSerialOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSerialOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSerialOK.FlatAppearance.BorderSize = 0;
            this.btnSerialOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerialOK.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSerialOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSerialOK.Image = global::AOG.Properties.Resources.OK64;
            this.btnSerialOK.Location = new System.Drawing.Point(501, 321);
            this.btnSerialOK.Name = "btnSerialOK";
            this.btnSerialOK.Size = new System.Drawing.Size(105, 64);
            this.btnSerialOK.TabIndex = 96;
            this.btnSerialOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnSerialOK.UseVisualStyleBackColor = true;
            this.btnSerialOK.Click += new System.EventHandler(this.btnSerialOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel.Image = global::AOG.Properties.Resources.Cancel64;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(364, 321);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 64);
            this.btnCancel.TabIndex = 470;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(162, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 29);
            this.label1.TabIndex = 471;
            this.label1.Text = "Enter New AB Line Heading";
            // 
            // btn0
            // 
            this.btn0.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn0.BackColor = System.Drawing.Color.Transparent;
            this.btn0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn0.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn0.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn0.Location = new System.Drawing.Point(64, 206);
            this.btn0.Name = "btn0";
            this.btn0.Size = new System.Drawing.Size(105, 64);
            this.btn0.TabIndex = 473;
            this.btn0.Text = "0";
            this.btn0.UseVisualStyleBackColor = false;
            this.btn0.Click += new System.EventHandler(this.btn0_Click);
            // 
            // btn90
            // 
            this.btn90.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn90.BackColor = System.Drawing.Color.Transparent;
            this.btn90.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn90.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn90.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn90.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn90.Location = new System.Drawing.Point(198, 206);
            this.btn90.Name = "btn90";
            this.btn90.Size = new System.Drawing.Size(105, 64);
            this.btn90.TabIndex = 474;
            this.btn90.Text = "90";
            this.btn90.UseVisualStyleBackColor = false;
            this.btn90.Click += new System.EventHandler(this.btn90_Click);
            // 
            // btn180
            // 
            this.btn180.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn180.BackColor = System.Drawing.Color.Transparent;
            this.btn180.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn180.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn180.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn180.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn180.Location = new System.Drawing.Point(332, 206);
            this.btn180.Name = "btn180";
            this.btn180.Size = new System.Drawing.Size(105, 64);
            this.btn180.TabIndex = 475;
            this.btn180.Text = "180";
            this.btn180.UseVisualStyleBackColor = false;
            this.btn180.Click += new System.EventHandler(this.btn180_Click);
            // 
            // btn270
            // 
            this.btn270.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn270.BackColor = System.Drawing.Color.Transparent;
            this.btn270.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn270.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn270.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn270.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn270.Location = new System.Drawing.Point(466, 206);
            this.btn270.Name = "btn270";
            this.btn270.Size = new System.Drawing.Size(105, 64);
            this.btn270.TabIndex = 476;
            this.btn270.Text = "270";
            this.btn270.UseVisualStyleBackColor = false;
            this.btn270.Click += new System.EventHandler(this.btn270_Click);
            // 
            // nudHeading
            // 
            this.nudHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudHeading.DecimalPlaces = 6;
            this.nudHeading.Location = new System.Drawing.Point(198, 71);
            this.nudHeading.Maximum = 359.999999D;
            this.nudHeading.Name = "nudHeading";
            this.nudHeading.Size = new System.Drawing.Size(257, 69);
            this.nudHeading.TabIndex = 472;
            this.nudHeading.ValueChanged += new System.EventHandler(this.nudHeading_ValueChanged);
            // 
            // FormABDrawHeading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(618, 393);
            this.ControlBox = false;
            this.Controls.Add(this.btn270);
            this.Controls.Add(this.btn180);
            this.Controls.Add(this.btn90);
            this.Controls.Add(this.btn0);
            this.Controls.Add(this.nudHeading);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSerialOK);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormABDrawHeading";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enter Heading";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSerialOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private NudlessNumericUpDown nudHeading;
        private System.Windows.Forms.Button btn0;
        private System.Windows.Forms.Button btn90;
        private System.Windows.Forms.Button btn180;
        private System.Windows.Forms.Button btn270;
    }
}