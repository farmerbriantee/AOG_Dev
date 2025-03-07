namespace AgIO
{
    partial class FormGPSOut
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblIMU = new System.Windows.Forms.Label();
            this.lblSteer = new System.Windows.Forms.Label();
            this.lblMachine = new System.Windows.Forms.Label();
            this.lblGPS = new System.Windows.Forms.Label();
            this.lblFromGPS = new System.Windows.Forms.Label();
            this.lblFromMU = new System.Windows.Forms.Label();
            this.lblFromSteerModule = new System.Windows.Forms.Label();
            this.lblFromMachineModule = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cboxPort = new System.Windows.Forms.ComboBox();
            this.lblCurrentPort = new System.Windows.Forms.Label();
            this.btnOpenSerial = new System.Windows.Forms.Button();
            this.btnCloseSerial = new System.Windows.Forms.Button();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.btnSerialOK = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.cboxBaud = new System.Windows.Forms.ComboBox();
            this.lblCurrentBaud = new System.Windows.Forms.Label();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(547, 203);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 23);
            this.label1.TabIndex = 73;
            this.label1.Text = "GPS:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Location = new System.Drawing.Point(506, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 23);
            this.label4.TabIndex = 76;
            this.label4.Text = "Machine:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label5.Location = new System.Drawing.Point(535, 290);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 23);
            this.label5.TabIndex = 77;
            this.label5.Text = "Steer:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label6.Location = new System.Drawing.Point(545, 247);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 23);
            this.label6.TabIndex = 78;
            this.label6.Text = "IMU:";
            // 
            // lblIMU
            // 
            this.lblIMU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIMU.AutoSize = true;
            this.lblIMU.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIMU.Location = new System.Drawing.Point(601, 247);
            this.lblIMU.Name = "lblIMU";
            this.lblIMU.Size = new System.Drawing.Size(50, 23);
            this.lblIMU.TabIndex = 83;
            this.lblIMU.Text = "IMU";
            // 
            // lblSteer
            // 
            this.lblSteer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSteer.AutoSize = true;
            this.lblSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteer.Location = new System.Drawing.Point(601, 290);
            this.lblSteer.Name = "lblSteer";
            this.lblSteer.Size = new System.Drawing.Size(60, 23);
            this.lblSteer.TabIndex = 82;
            this.lblSteer.Text = "Steer";
            // 
            // lblMachine
            // 
            this.lblMachine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMachine.AutoSize = true;
            this.lblMachine.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMachine.Location = new System.Drawing.Point(601, 332);
            this.lblMachine.Name = "lblMachine";
            this.lblMachine.Size = new System.Drawing.Size(60, 23);
            this.lblMachine.TabIndex = 81;
            this.lblMachine.Text = "Mach";
            // 
            // lblGPS
            // 
            this.lblGPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGPS.AutoSize = true;
            this.lblGPS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGPS.Location = new System.Drawing.Point(601, 203);
            this.lblGPS.Name = "lblGPS";
            this.lblGPS.Size = new System.Drawing.Size(48, 23);
            this.lblGPS.TabIndex = 79;
            this.lblGPS.Text = "GPS";
            // 
            // lblFromGPS
            // 
            this.lblFromGPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFromGPS.BackColor = System.Drawing.Color.Transparent;
            this.lblFromGPS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromGPS.ForeColor = System.Drawing.Color.Black;
            this.lblFromGPS.Location = new System.Drawing.Point(680, 201);
            this.lblFromGPS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromGPS.Name = "lblFromGPS";
            this.lblFromGPS.Size = new System.Drawing.Size(64, 27);
            this.lblFromGPS.TabIndex = 172;
            this.lblFromGPS.Text = "---";
            this.lblFromGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromMU
            // 
            this.lblFromMU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFromMU.BackColor = System.Drawing.Color.Transparent;
            this.lblFromMU.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromMU.ForeColor = System.Drawing.Color.Black;
            this.lblFromMU.Location = new System.Drawing.Point(680, 244);
            this.lblFromMU.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromMU.Name = "lblFromMU";
            this.lblFromMU.Size = new System.Drawing.Size(64, 27);
            this.lblFromMU.TabIndex = 175;
            this.lblFromMU.Text = "---";
            this.lblFromMU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromSteerModule
            // 
            this.lblFromSteerModule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFromSteerModule.BackColor = System.Drawing.Color.Transparent;
            this.lblFromSteerModule.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromSteerModule.ForeColor = System.Drawing.Color.Black;
            this.lblFromSteerModule.Location = new System.Drawing.Point(680, 288);
            this.lblFromSteerModule.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromSteerModule.Name = "lblFromSteerModule";
            this.lblFromSteerModule.Size = new System.Drawing.Size(64, 27);
            this.lblFromSteerModule.TabIndex = 173;
            this.lblFromSteerModule.Text = "---";
            this.lblFromSteerModule.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromMachineModule
            // 
            this.lblFromMachineModule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFromMachineModule.BackColor = System.Drawing.Color.Transparent;
            this.lblFromMachineModule.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromMachineModule.ForeColor = System.Drawing.Color.Black;
            this.lblFromMachineModule.Location = new System.Drawing.Point(680, 330);
            this.lblFromMachineModule.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromMachineModule.Name = "lblFromMachineModule";
            this.lblFromMachineModule.Size = new System.Drawing.Size(64, 27);
            this.lblFromMachineModule.TabIndex = 174;
            this.lblFromMachineModule.Text = "---";
            this.lblFromMachineModule.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.Transparent;
            this.groupBox7.Controls.Add(this.cboxBaud);
            this.groupBox7.Controls.Add(this.lblCurrentBaud);
            this.groupBox7.Controls.Add(this.cboxPort);
            this.groupBox7.Controls.Add(this.lblCurrentPort);
            this.groupBox7.Controls.Add(this.btnOpenSerial);
            this.groupBox7.Controls.Add(this.btnCloseSerial);
            this.groupBox7.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox7.Location = new System.Drawing.Point(104, 12);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(616, 101);
            this.groupBox7.TabIndex = 72;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "GPS Out";
            // 
            // cboxPort
            // 
            this.cboxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxPort.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxPort.FormattingEnabled = true;
            this.cboxPort.Location = new System.Drawing.Point(27, 53);
            this.cboxPort.Name = "cboxPort";
            this.cboxPort.Size = new System.Drawing.Size(124, 37);
            this.cboxPort.TabIndex = 64;
            this.cboxPort.SelectedIndexChanged += new System.EventHandler(this.cboxPort_SelectedIndexChanged);
            // 
            // lblCurrentPort
            // 
            this.lblCurrentPort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPort.Location = new System.Drawing.Point(33, 32);
            this.lblCurrentPort.Name = "lblCurrentPort";
            this.lblCurrentPort.Size = new System.Drawing.Size(112, 18);
            this.lblCurrentPort.TabIndex = 70;
            this.lblCurrentPort.Text = "Port";
            this.lblCurrentPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOpenSerial
            // 
            this.btnOpenSerial.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSerial.FlatAppearance.BorderSize = 0;
            this.btnOpenSerial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSerial.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSerial.Image = global::AgIO.Properties.Resources.USB_Connect;
            this.btnOpenSerial.Location = new System.Drawing.Point(368, 43);
            this.btnOpenSerial.Name = "btnOpenSerial";
            this.btnOpenSerial.Size = new System.Drawing.Size(101, 58);
            this.btnOpenSerial.TabIndex = 53;
            this.btnOpenSerial.UseVisualStyleBackColor = false;
            this.btnOpenSerial.Click += new System.EventHandler(this.btnOpenSerial_Click);
            // 
            // btnCloseSerial
            // 
            this.btnCloseSerial.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseSerial.FlatAppearance.BorderSize = 0;
            this.btnCloseSerial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseSerial.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseSerial.Image = global::AgIO.Properties.Resources.USB_Disconnect;
            this.btnCloseSerial.Location = new System.Drawing.Point(483, 43);
            this.btnCloseSerial.Name = "btnCloseSerial";
            this.btnCloseSerial.Size = new System.Drawing.Size(101, 58);
            this.btnCloseSerial.TabIndex = 52;
            this.btnCloseSerial.UseVisualStyleBackColor = false;
            this.btnCloseSerial.Click += new System.EventHandler(this.btnCloseSerial_Click);
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackgroundImage = global::AgIO.Properties.Resources.GPS_Out;
            this.pictureBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox5.Location = new System.Drawing.Point(12, 25);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(73, 59);
            this.pictureBox5.TabIndex = 71;
            this.pictureBox5.TabStop = false;
            // 
            // btnSerialOK
            // 
            this.btnSerialOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSerialOK.BackColor = System.Drawing.Color.Transparent;
            this.btnSerialOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSerialOK.FlatAppearance.BorderSize = 0;
            this.btnSerialOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerialOK.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnSerialOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSerialOK.Image = global::AgIO.Properties.Resources.OK64;
            this.btnSerialOK.Location = new System.Drawing.Point(672, 475);
            this.btnSerialOK.Name = "btnSerialOK";
            this.btnSerialOK.Size = new System.Drawing.Size(91, 63);
            this.btnSerialOK.TabIndex = 59;
            this.btnSerialOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnSerialOK.UseVisualStyleBackColor = false;
            this.btnSerialOK.Click += new System.EventHandler(this.btnSerialOK_Click);
            // 
            // btnRescan
            // 
            this.btnRescan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRescan.BackColor = System.Drawing.Color.Transparent;
            this.btnRescan.FlatAppearance.BorderSize = 0;
            this.btnRescan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRescan.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRescan.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRescan.Image = global::AgIO.Properties.Resources.ScanPorts;
            this.btnRescan.Location = new System.Drawing.Point(511, 476);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(89, 63);
            this.btnRescan.TabIndex = 58;
            this.btnRescan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRescan.UseVisualStyleBackColor = false;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // cboxBaud
            // 
            this.cboxBaud.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxBaud.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxBaud.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxBaud.FormattingEnabled = true;
            this.cboxBaud.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cboxBaud.Location = new System.Drawing.Point(192, 53);
            this.cboxBaud.Name = "cboxBaud";
            this.cboxBaud.Size = new System.Drawing.Size(127, 37);
            this.cboxBaud.TabIndex = 73;
            this.cboxBaud.SelectedIndexChanged += new System.EventHandler(this.cboxBaud_SelectedIndexChanged);
            // 
            // lblCurrentBaud
            // 
            this.lblCurrentBaud.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentBaud.Location = new System.Drawing.Point(192, 32);
            this.lblCurrentBaud.Name = "lblCurrentBaud";
            this.lblCurrentBaud.Size = new System.Drawing.Size(124, 18);
            this.lblCurrentBaud.TabIndex = 72;
            this.lblCurrentBaud.Text = "GPS Baud";
            this.lblCurrentBaud.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormGPSOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(775, 550);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.lblFromGPS);
            this.Controls.Add(this.lblFromMU);
            this.Controls.Add(this.lblFromSteerModule);
            this.Controls.Add(this.lblFromMachineModule);
            this.Controls.Add(this.lblIMU);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.lblSteer);
            this.Controls.Add(this.lblMachine);
            this.Controls.Add(this.lblGPS);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRescan);
            this.Controls.Add(this.btnSerialOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormGPSOut";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect External GPS";
            this.Load += new System.EventHandler(this.FormCommSet_Load);
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblIMU;
        private System.Windows.Forms.Label lblSteer;
        private System.Windows.Forms.Label lblMachine;
        private System.Windows.Forms.Label lblGPS;
        private System.Windows.Forms.Label lblFromGPS;
        private System.Windows.Forms.Label lblFromMU;
        private System.Windows.Forms.Label lblFromSteerModule;
        private System.Windows.Forms.Label lblFromMachineModule;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ComboBox cboxPort;
        private System.Windows.Forms.Label lblCurrentPort;
        private System.Windows.Forms.Button btnOpenSerial;
        private System.Windows.Forms.Button btnCloseSerial;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Button btnSerialOK;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.ComboBox cboxBaud;
        private System.Windows.Forms.Label lblCurrentBaud;
    }
}