
namespace ModSimTool
{
    partial class FormSim
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSim));
            this.lblIP = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblIsSteer = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.lblMaxAngle_Tool = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.lblInvertWAS_Tool = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.lblIntegral = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.lblInvertSteer_Tool = new System.Windows.Forms.Label();
            this.lblAckerman = new System.Windows.Forms.Label();
            this.lblMinPWM = new System.Windows.Forms.Label();
            this.lblWAS_Offset = new System.Windows.Forms.Label();
            this.lblKp = new System.Windows.Forms.Label();
            this.lblHighPWM = new System.Windows.Forms.Label();
            this.lblWAS_CPD = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblScanReply = new System.Windows.Forms.Label();
            this.lblIPSet3 = new System.Windows.Forms.Label();
            this.lblIPSet2 = new System.Windows.Forms.Label();
            this.lblIPSet1 = new System.Windows.Forms.Label();
            this.cboxKSXT = new System.Windows.Forms.CheckBox();
            this.cboxNDA = new System.Windows.Forms.CheckBox();
            this.lblWAS = new System.Windows.Forms.Label();
            this.tbarSteerAngleWAS = new System.Windows.Forms.TrackBar();
            this.cboxOGI = new System.Windows.Forms.CheckBox();
            this.cboxGGA = new System.Windows.Forms.CheckBox();
            this.lblKmh = new System.Windows.Forms.Label();
            this.mSec = new System.Windows.Forms.Label();
            this.cboxHDT = new System.Windows.Forms.CheckBox();
            this.cboxAVR = new System.Windows.Forms.CheckBox();
            this.cboxVTG = new System.Windows.Forms.CheckBox();
            this.tbarSpeed = new System.Windows.Forms.TrackBar();
            this.simTimer = new System.Windows.Forms.Timer();
            this.lblHeading = new System.Windows.Forms.Label();
            this.cboxRMC = new System.Windows.Forms.CheckBox();
            this.tbarRoll = new System.Windows.Forms.TrackBar();
            this.lblRoll = new System.Windows.Forms.Label();
            this.nudLat = new System.Windows.Forms.NumericUpDown();
            this.nudLon = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblCurrentLat = new System.Windows.Forms.Label();
            this.lblLatt = new System.Windows.Forms.Label();
            this.lblCurrentLon = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblXTE = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.btn10Hz = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbarSteerAngleWAS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarRoll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIP
            // 
            this.lblIP.BackColor = System.Drawing.Color.Transparent;
            this.lblIP.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIP.ForeColor = System.Drawing.Color.Black;
            this.lblIP.Location = new System.Drawing.Point(5, 181);
            this.lblIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(143, 59);
            this.lblIP.TabIndex = 464;
            this.lblIP.Text = "288.288.288.288\r\n288\r\n288";
            this.lblIP.Click += new System.EventHandler(this.lblIP_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblIsSteer);
            this.groupBox1.Controls.Add(this.label49);
            this.groupBox1.Controls.Add(this.label36);
            this.groupBox1.Controls.Add(this.lblMaxAngle_Tool);
            this.groupBox1.Controls.Add(this.label41);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.label40);
            this.groupBox1.Controls.Add(this.lblInvertWAS_Tool);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label38);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.label37);
            this.groupBox1.Controls.Add(this.lblIntegral);
            this.groupBox1.Controls.Add(this.label35);
            this.groupBox1.Controls.Add(this.lblInvertSteer_Tool);
            this.groupBox1.Controls.Add(this.lblAckerman);
            this.groupBox1.Controls.Add(this.lblMinPWM);
            this.groupBox1.Controls.Add(this.lblWAS_Offset);
            this.groupBox1.Controls.Add(this.lblKp);
            this.groupBox1.Controls.Add(this.lblHighPWM);
            this.groupBox1.Controls.Add(this.lblWAS_CPD);
            this.groupBox1.Location = new System.Drawing.Point(448, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 339);
            this.groupBox1.TabIndex = 516;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tool Steer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(1, 315);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 19);
            this.label2.TabIndex = 597;
            this.label2.Text = "isSteerNoSld";
            // 
            // lblIsSteer
            // 
            this.lblIsSteer.AutoSize = true;
            this.lblIsSteer.BackColor = System.Drawing.Color.Transparent;
            this.lblIsSteer.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIsSteer.ForeColor = System.Drawing.Color.Black;
            this.lblIsSteer.Location = new System.Drawing.Point(106, 315);
            this.lblIsSteer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIsSteer.Name = "lblIsSteer";
            this.lblIsSteer.Size = new System.Drawing.Size(14, 19);
            this.lblIsSteer.TabIndex = 598;
            this.lblIsSteer.Text = ".";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.BackColor = System.Drawing.Color.Transparent;
            this.label49.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label49.ForeColor = System.Drawing.Color.Black;
            this.label49.Location = new System.Drawing.Point(21, 286);
            this.label49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(78, 19);
            this.label49.TabIndex = 571;
            this.label49.Text = "MaxAngle";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.BackColor = System.Drawing.Color.Transparent;
            this.label36.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.ForeColor = System.Drawing.Color.Black;
            this.label36.Location = new System.Drawing.Point(5, 163);
            this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(94, 19);
            this.label36.TabIndex = 593;
            this.label36.Text = "WAS_Offset";
            // 
            // lblMaxAngle_Tool
            // 
            this.lblMaxAngle_Tool.AutoSize = true;
            this.lblMaxAngle_Tool.BackColor = System.Drawing.Color.Transparent;
            this.lblMaxAngle_Tool.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxAngle_Tool.ForeColor = System.Drawing.Color.Black;
            this.lblMaxAngle_Tool.Location = new System.Drawing.Point(106, 286);
            this.lblMaxAngle_Tool.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaxAngle_Tool.Name = "lblMaxAngle_Tool";
            this.lblMaxAngle_Tool.Size = new System.Drawing.Size(14, 19);
            this.lblMaxAngle_Tool.TabIndex = 572;
            this.lblMaxAngle_Tool.Text = ".";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.BackColor = System.Drawing.Color.Transparent;
            this.label41.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.ForeColor = System.Drawing.Color.Black;
            this.label41.Location = new System.Drawing.Point(72, 23);
            this.label41.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(27, 19);
            this.label41.TabIndex = 596;
            this.label41.Text = "Kp";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.Black;
            this.label25.Location = new System.Drawing.Point(14, 230);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(85, 19);
            this.label25.TabIndex = 575;
            this.label25.Text = "InvertWAS";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.BackColor = System.Drawing.Color.Transparent;
            this.label40.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.ForeColor = System.Drawing.Color.Black;
            this.label40.Location = new System.Drawing.Point(22, 107);
            this.label40.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(77, 19);
            this.label40.TabIndex = 595;
            this.label40.Text = "HighPWM";
            // 
            // lblInvertWAS_Tool
            // 
            this.lblInvertWAS_Tool.AutoSize = true;
            this.lblInvertWAS_Tool.BackColor = System.Drawing.Color.Transparent;
            this.lblInvertWAS_Tool.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvertWAS_Tool.ForeColor = System.Drawing.Color.Black;
            this.lblInvertWAS_Tool.Location = new System.Drawing.Point(106, 230);
            this.lblInvertWAS_Tool.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInvertWAS_Tool.Name = "lblInvertWAS_Tool";
            this.lblInvertWAS_Tool.Size = new System.Drawing.Size(14, 19);
            this.lblInvertWAS_Tool.TabIndex = 576;
            this.lblInvertWAS_Tool.Text = ".";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(35, 51);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 19);
            this.label1.TabIndex = 578;
            this.label1.Text = "Integral";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.BackColor = System.Drawing.Color.Transparent;
            this.label38.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.ForeColor = System.Drawing.Color.Black;
            this.label38.Location = new System.Drawing.Point(30, 79);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(69, 19);
            this.label38.TabIndex = 591;
            this.label38.Text = "MinPWM";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.Black;
            this.label23.Location = new System.Drawing.Point(12, 258);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(87, 19);
            this.label23.TabIndex = 573;
            this.label23.Text = "InvertSteer";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.BackColor = System.Drawing.Color.Transparent;
            this.label37.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.ForeColor = System.Drawing.Color.Black;
            this.label37.Location = new System.Drawing.Point(17, 135);
            this.label37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(82, 19);
            this.label37.TabIndex = 592;
            this.label37.Text = "WAS_CPD";
            // 
            // lblIntegral
            // 
            this.lblIntegral.AutoSize = true;
            this.lblIntegral.BackColor = System.Drawing.Color.Transparent;
            this.lblIntegral.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIntegral.ForeColor = System.Drawing.Color.Black;
            this.lblIntegral.Location = new System.Drawing.Point(106, 51);
            this.lblIntegral.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIntegral.Name = "lblIntegral";
            this.lblIntegral.Size = new System.Drawing.Size(14, 19);
            this.lblIntegral.TabIndex = 577;
            this.lblIntegral.Text = ".";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.BackColor = System.Drawing.Color.Transparent;
            this.label35.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.Black;
            this.label35.Location = new System.Drawing.Point(10, 191);
            this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(89, 19);
            this.label35.TabIndex = 594;
            this.label35.Text = "Ackermann";
            // 
            // lblInvertSteer_Tool
            // 
            this.lblInvertSteer_Tool.AutoSize = true;
            this.lblInvertSteer_Tool.BackColor = System.Drawing.Color.Transparent;
            this.lblInvertSteer_Tool.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvertSteer_Tool.ForeColor = System.Drawing.Color.Black;
            this.lblInvertSteer_Tool.Location = new System.Drawing.Point(106, 258);
            this.lblInvertSteer_Tool.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInvertSteer_Tool.Name = "lblInvertSteer_Tool";
            this.lblInvertSteer_Tool.Size = new System.Drawing.Size(14, 19);
            this.lblInvertSteer_Tool.TabIndex = 574;
            this.lblInvertSteer_Tool.Text = ".";
            // 
            // lblAckerman
            // 
            this.lblAckerman.AutoSize = true;
            this.lblAckerman.BackColor = System.Drawing.Color.Transparent;
            this.lblAckerman.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAckerman.ForeColor = System.Drawing.Color.Black;
            this.lblAckerman.Location = new System.Drawing.Point(106, 191);
            this.lblAckerman.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAckerman.Name = "lblAckerman";
            this.lblAckerman.Size = new System.Drawing.Size(14, 19);
            this.lblAckerman.TabIndex = 583;
            this.lblAckerman.Text = ".";
            // 
            // lblMinPWM
            // 
            this.lblMinPWM.AutoSize = true;
            this.lblMinPWM.BackColor = System.Drawing.Color.Transparent;
            this.lblMinPWM.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinPWM.ForeColor = System.Drawing.Color.Black;
            this.lblMinPWM.Location = new System.Drawing.Point(106, 79);
            this.lblMinPWM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMinPWM.Name = "lblMinPWM";
            this.lblMinPWM.Size = new System.Drawing.Size(14, 19);
            this.lblMinPWM.TabIndex = 588;
            this.lblMinPWM.Text = ".";
            // 
            // lblWAS_Offset
            // 
            this.lblWAS_Offset.AutoSize = true;
            this.lblWAS_Offset.BackColor = System.Drawing.Color.Transparent;
            this.lblWAS_Offset.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWAS_Offset.ForeColor = System.Drawing.Color.Black;
            this.lblWAS_Offset.Location = new System.Drawing.Point(106, 163);
            this.lblWAS_Offset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWAS_Offset.Name = "lblWAS_Offset";
            this.lblWAS_Offset.Size = new System.Drawing.Size(14, 19);
            this.lblWAS_Offset.TabIndex = 584;
            this.lblWAS_Offset.Text = ".";
            // 
            // lblKp
            // 
            this.lblKp.AutoSize = true;
            this.lblKp.BackColor = System.Drawing.Color.Transparent;
            this.lblKp.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKp.ForeColor = System.Drawing.Color.Black;
            this.lblKp.Location = new System.Drawing.Point(106, 23);
            this.lblKp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKp.Name = "lblKp";
            this.lblKp.Size = new System.Drawing.Size(14, 19);
            this.lblKp.TabIndex = 587;
            this.lblKp.Text = ".";
            // 
            // lblHighPWM
            // 
            this.lblHighPWM.AutoSize = true;
            this.lblHighPWM.BackColor = System.Drawing.Color.Transparent;
            this.lblHighPWM.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHighPWM.ForeColor = System.Drawing.Color.Black;
            this.lblHighPWM.Location = new System.Drawing.Point(106, 107);
            this.lblHighPWM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHighPWM.Name = "lblHighPWM";
            this.lblHighPWM.Size = new System.Drawing.Size(14, 19);
            this.lblHighPWM.TabIndex = 585;
            this.lblHighPWM.Text = ".";
            // 
            // lblWAS_CPD
            // 
            this.lblWAS_CPD.AutoSize = true;
            this.lblWAS_CPD.BackColor = System.Drawing.Color.Transparent;
            this.lblWAS_CPD.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWAS_CPD.ForeColor = System.Drawing.Color.Black;
            this.lblWAS_CPD.Location = new System.Drawing.Point(106, 135);
            this.lblWAS_CPD.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWAS_CPD.Name = "lblWAS_CPD";
            this.lblWAS_CPD.Size = new System.Drawing.Size(14, 19);
            this.lblWAS_CPD.TabIndex = 586;
            this.lblWAS_CPD.Text = ".";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(201, 327);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 19);
            this.label6.TabIndex = 562;
            this.label6.Text = "SubNet:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScanReply
            // 
            this.lblScanReply.BackColor = System.Drawing.Color.Transparent;
            this.lblScanReply.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScanReply.ForeColor = System.Drawing.Color.Black;
            this.lblScanReply.Location = new System.Drawing.Point(397, 327);
            this.lblScanReply.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScanReply.Name = "lblScanReply";
            this.lblScanReply.Size = new System.Drawing.Size(40, 19);
            this.lblScanReply.TabIndex = 561;
            this.lblScanReply.Text = "No";
            this.lblScanReply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblIPSet3
            // 
            this.lblIPSet3.BackColor = System.Drawing.Color.Transparent;
            this.lblIPSet3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIPSet3.ForeColor = System.Drawing.Color.Black;
            this.lblIPSet3.Location = new System.Drawing.Point(349, 327);
            this.lblIPSet3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIPSet3.Name = "lblIPSet3";
            this.lblIPSet3.Size = new System.Drawing.Size(40, 19);
            this.lblIPSet3.TabIndex = 557;
            this.lblIPSet3.Text = "255";
            // 
            // lblIPSet2
            // 
            this.lblIPSet2.BackColor = System.Drawing.Color.Transparent;
            this.lblIPSet2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIPSet2.ForeColor = System.Drawing.Color.Black;
            this.lblIPSet2.Location = new System.Drawing.Point(306, 327);
            this.lblIPSet2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIPSet2.Name = "lblIPSet2";
            this.lblIPSet2.Size = new System.Drawing.Size(40, 19);
            this.lblIPSet2.TabIndex = 557;
            this.lblIPSet2.Text = "255";
            // 
            // lblIPSet1
            // 
            this.lblIPSet1.BackColor = System.Drawing.Color.Transparent;
            this.lblIPSet1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIPSet1.ForeColor = System.Drawing.Color.Black;
            this.lblIPSet1.Location = new System.Drawing.Point(263, 327);
            this.lblIPSet1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIPSet1.Name = "lblIPSet1";
            this.lblIPSet1.Size = new System.Drawing.Size(40, 19);
            this.lblIPSet1.TabIndex = 557;
            this.lblIPSet1.Text = "255";
            // 
            // cboxKSXT
            // 
            this.cboxKSXT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxKSXT.Location = new System.Drawing.Point(112, 295);
            this.cboxKSXT.Name = "cboxKSXT";
            this.cboxKSXT.Size = new System.Drawing.Size(80, 24);
            this.cboxKSXT.TabIndex = 537;
            this.cboxKSXT.Text = "KSXT";
            this.cboxKSXT.UseVisualStyleBackColor = true;
            // 
            // cboxNDA
            // 
            this.cboxNDA.Checked = true;
            this.cboxNDA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxNDA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxNDA.Location = new System.Drawing.Point(7, 323);
            this.cboxNDA.Name = "cboxNDA";
            this.cboxNDA.Size = new System.Drawing.Size(69, 24);
            this.cboxNDA.TabIndex = 536;
            this.cboxNDA.Text = "NDA";
            this.cboxNDA.UseVisualStyleBackColor = true;
            // 
            // lblWAS
            // 
            this.lblWAS.BackColor = System.Drawing.Color.White;
            this.lblWAS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWAS.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWAS.Location = new System.Drawing.Point(216, 10);
            this.lblWAS.Name = "lblWAS";
            this.lblWAS.Size = new System.Drawing.Size(119, 21);
            this.lblWAS.TabIndex = 535;
            this.lblWAS.Text = "Steer: 0.0°";
            this.lblWAS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblWAS.Click += new System.EventHandler(this.lblWAS_Click);
            // 
            // tbarSteerAngleWAS
            // 
            this.tbarSteerAngleWAS.LargeChange = 1;
            this.tbarSteerAngleWAS.Location = new System.Drawing.Point(203, 41);
            this.tbarSteerAngleWAS.Maximum = 6000;
            this.tbarSteerAngleWAS.Minimum = -6000;
            this.tbarSteerAngleWAS.Name = "tbarSteerAngleWAS";
            this.tbarSteerAngleWAS.Size = new System.Drawing.Size(241, 45);
            this.tbarSteerAngleWAS.TabIndex = 534;
            this.tbarSteerAngleWAS.TickFrequency = 500;
            this.tbarSteerAngleWAS.Scroll += new System.EventHandler(this.tbarSteerAngleWAS_Scroll);
            // 
            // cboxOGI
            // 
            this.cboxOGI.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxOGI.Location = new System.Drawing.Point(112, 321);
            this.cboxOGI.Name = "cboxOGI";
            this.cboxOGI.Size = new System.Drawing.Size(69, 24);
            this.cboxOGI.TabIndex = 532;
            this.cboxOGI.Text = "OGI";
            this.cboxOGI.UseVisualStyleBackColor = true;
            // 
            // cboxGGA
            // 
            this.cboxGGA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxGGA.Location = new System.Drawing.Point(7, 243);
            this.cboxGGA.Name = "cboxGGA";
            this.cboxGGA.Size = new System.Drawing.Size(69, 24);
            this.cboxGGA.TabIndex = 530;
            this.cboxGGA.Text = "GGA";
            this.cboxGGA.UseVisualStyleBackColor = true;
            // 
            // lblKmh
            // 
            this.lblKmh.BackColor = System.Drawing.Color.White;
            this.lblKmh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblKmh.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKmh.Location = new System.Drawing.Point(211, 86);
            this.lblKmh.Name = "lblKmh";
            this.lblKmh.Size = new System.Drawing.Size(104, 24);
            this.lblKmh.TabIndex = 529;
            this.lblKmh.Text = "Kmh: 0.0";
            this.lblKmh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblKmh.Click += new System.EventHandler(this.lblKmh_Click);
            // 
            // mSec
            // 
            this.mSec.AutoSize = true;
            this.mSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mSec.Location = new System.Drawing.Point(317, 90);
            this.mSec.Name = "mSec";
            this.mSec.Size = new System.Drawing.Size(60, 16);
            this.mSec.TabIndex = 528;
            this.mSec.Text = "m/s: 0.0";
            // 
            // cboxHDT
            // 
            this.cboxHDT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxHDT.Location = new System.Drawing.Point(112, 269);
            this.cboxHDT.Name = "cboxHDT";
            this.cboxHDT.Size = new System.Drawing.Size(69, 24);
            this.cboxHDT.TabIndex = 527;
            this.cboxHDT.Text = "HDT";
            this.cboxHDT.UseVisualStyleBackColor = true;
            // 
            // cboxAVR
            // 
            this.cboxAVR.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxAVR.Location = new System.Drawing.Point(112, 243);
            this.cboxAVR.Name = "cboxAVR";
            this.cboxAVR.Size = new System.Drawing.Size(69, 24);
            this.cboxAVR.TabIndex = 526;
            this.cboxAVR.Text = "AVR";
            this.cboxAVR.UseVisualStyleBackColor = true;
            // 
            // cboxVTG
            // 
            this.cboxVTG.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxVTG.Location = new System.Drawing.Point(7, 269);
            this.cboxVTG.Name = "cboxVTG";
            this.cboxVTG.Size = new System.Drawing.Size(69, 24);
            this.cboxVTG.TabIndex = 525;
            this.cboxVTG.Text = "VTG";
            this.cboxVTG.UseVisualStyleBackColor = true;
            // 
            // tbarSpeed
            // 
            this.tbarSpeed.LargeChange = 10;
            this.tbarSpeed.Location = new System.Drawing.Point(203, 117);
            this.tbarSpeed.Maximum = 500;
            this.tbarSpeed.Minimum = -200;
            this.tbarSpeed.Name = "tbarSpeed";
            this.tbarSpeed.Size = new System.Drawing.Size(241, 45);
            this.tbarSpeed.TabIndex = 519;
            this.tbarSpeed.TickFrequency = 50;
            this.tbarSpeed.Scroll += new System.EventHandler(this.tbarSpeed_Scroll);
            // 
            // simTimer
            // 
            this.simTimer.Enabled = true;
            this.simTimer.Interval = 95;
            this.simTimer.Tick += new System.EventHandler(this.simTimer_Tick);
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.BackColor = System.Drawing.Color.Transparent;
            this.lblHeading.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.ForeColor = System.Drawing.Color.Black;
            this.lblHeading.Location = new System.Drawing.Point(336, 11);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(18, 18);
            this.lblHeading.TabIndex = 557;
            this.lblHeading.Text = "0";
            // 
            // cboxRMC
            // 
            this.cboxRMC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxRMC.Location = new System.Drawing.Point(7, 295);
            this.cboxRMC.Name = "cboxRMC";
            this.cboxRMC.Size = new System.Drawing.Size(69, 24);
            this.cboxRMC.TabIndex = 531;
            this.cboxRMC.Text = "RMC";
            this.cboxRMC.UseVisualStyleBackColor = true;
            // 
            // tbarRoll
            // 
            this.tbarRoll.LargeChange = 10;
            this.tbarRoll.Location = new System.Drawing.Point(203, 198);
            this.tbarRoll.Maximum = 200;
            this.tbarRoll.Minimum = -200;
            this.tbarRoll.Name = "tbarRoll";
            this.tbarRoll.Size = new System.Drawing.Size(241, 45);
            this.tbarRoll.TabIndex = 558;
            this.tbarRoll.TickFrequency = 50;
            this.tbarRoll.Scroll += new System.EventHandler(this.tbarRoll_Scroll);
            // 
            // lblRoll
            // 
            this.lblRoll.BackColor = System.Drawing.Color.White;
            this.lblRoll.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRoll.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRoll.Location = new System.Drawing.Point(241, 169);
            this.lblRoll.Name = "lblRoll";
            this.lblRoll.Size = new System.Drawing.Size(114, 21);
            this.lblRoll.TabIndex = 559;
            this.lblRoll.Text = "Roll: 0°";
            this.lblRoll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRoll.Click += new System.EventHandler(this.lblRoll_Click);
            // 
            // nudLat
            // 
            this.nudLat.DecimalPlaces = 7;
            this.nudLat.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLat.Location = new System.Drawing.Point(11, 52);
            this.nudLat.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.nudLat.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.nudLat.Name = "nudLat";
            this.nudLat.Size = new System.Drawing.Size(136, 27);
            this.nudLat.TabIndex = 561;
            this.nudLat.Value = new decimal(new int[] {
            534360564,
            0,
            0,
            458752});
            // 
            // nudLon
            // 
            this.nudLon.DecimalPlaces = 7;
            this.nudLon.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLon.Location = new System.Drawing.Point(11, 81);
            this.nudLon.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nudLon.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.nudLon.Name = "nudLon";
            this.nudLon.Size = new System.Drawing.Size(136, 27);
            this.nudLon.TabIndex = 562;
            this.nudLon.Value = new decimal(new int[] {
            1111600471,
            0,
            0,
            -2147024896});
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(20, 112);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(101, 28);
            this.btnSave.TabIndex = 559;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblCurrentLat
            // 
            this.lblCurrentLat.AutoSize = true;
            this.lblCurrentLat.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentLat.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentLat.ForeColor = System.Drawing.Color.Black;
            this.lblCurrentLat.Location = new System.Drawing.Point(36, 4);
            this.lblCurrentLat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentLat.Name = "lblCurrentLat";
            this.lblCurrentLat.Size = new System.Drawing.Size(111, 19);
            this.lblCurrentLat.TabIndex = 560;
            this.lblCurrentLat.Text = "-88.8888888";
            // 
            // lblLatt
            // 
            this.lblLatt.AutoSize = true;
            this.lblLatt.BackColor = System.Drawing.Color.Transparent;
            this.lblLatt.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLatt.ForeColor = System.Drawing.Color.Black;
            this.lblLatt.Location = new System.Drawing.Point(6, 4);
            this.lblLatt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLatt.Name = "lblLatt";
            this.lblLatt.Size = new System.Drawing.Size(30, 19);
            this.lblLatt.TabIndex = 559;
            this.lblLatt.Text = "Lat";
            // 
            // lblCurrentLon
            // 
            this.lblCurrentLon.AutoSize = true;
            this.lblCurrentLon.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentLon.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentLon.ForeColor = System.Drawing.Color.Black;
            this.lblCurrentLon.Location = new System.Drawing.Point(36, 26);
            this.lblCurrentLon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentLon.Name = "lblCurrentLon";
            this.lblCurrentLon.Size = new System.Drawing.Size(121, 19);
            this.lblCurrentLon.TabIndex = 564;
            this.lblCurrentLon.Text = "-166.8888888";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Black;
            this.label22.Location = new System.Drawing.Point(1, 26);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(35, 19);
            this.label22.TabIndex = 563;
            this.label22.Text = "Lon";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(339, 246);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 23);
            this.label3.TabIndex = 581;
            this.label3.Text = "XTE";
            // 
            // lblXTE
            // 
            this.lblXTE.AutoSize = true;
            this.lblXTE.BackColor = System.Drawing.Color.Transparent;
            this.lblXTE.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXTE.ForeColor = System.Drawing.Color.Black;
            this.lblXTE.Location = new System.Drawing.Point(391, 246);
            this.lblXTE.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblXTE.Name = "lblXTE";
            this.lblXTE.Size = new System.Drawing.Size(16, 23);
            this.lblXTE.TabIndex = 582;
            this.lblXTE.Text = ".";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(315, 276);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 23);
            this.label5.TabIndex = 579;
            this.label5.Text = "Status";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Location = new System.Drawing.Point(391, 276);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(16, 23);
            this.lblStatus.TabIndex = 580;
            this.lblStatus.Text = ".";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(211, 246);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 23);
            this.label8.TabIndex = 577;
            this.label8.Text = "Speed";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblSpeed.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeed.ForeColor = System.Drawing.Color.Black;
            this.lblSpeed.Location = new System.Drawing.Point(283, 246);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(16, 23);
            this.lblSpeed.TabIndex = 578;
            this.lblSpeed.Text = ".";
            // 
            // btn10Hz
            // 
            this.btn10Hz.Location = new System.Drawing.Point(20, 146);
            this.btn10Hz.Name = "btn10Hz";
            this.btn10Hz.Size = new System.Drawing.Size(101, 28);
            this.btn10Hz.TabIndex = 599;
            this.btn10Hz.Text = "10 Hz";
            this.btn10Hz.UseVisualStyleBackColor = true;
            this.btn10Hz.Click += new System.EventHandler(this.btn10Hz_Click);
            // 
            // FormSim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(607, 352);
            this.Controls.Add(this.btn10Hz);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblXTE);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblCurrentLon);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.lblCurrentLat);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblLatt);
            this.Controls.Add(this.lblScanReply);
            this.Controls.Add(this.lblIPSet3);
            this.Controls.Add(this.nudLon);
            this.Controls.Add(this.lblIPSet2);
            this.Controls.Add(this.nudLat);
            this.Controls.Add(this.lblRoll);
            this.Controls.Add(this.tbarRoll);
            this.Controls.Add(this.lblIPSet1);
            this.Controls.Add(this.cboxKSXT);
            this.Controls.Add(this.cboxNDA);
            this.Controls.Add(this.lblWAS);
            this.Controls.Add(this.cboxOGI);
            this.Controls.Add(this.cboxRMC);
            this.Controls.Add(this.cboxGGA);
            this.Controls.Add(this.lblKmh);
            this.Controls.Add(this.mSec);
            this.Controls.Add(this.cboxHDT);
            this.Controls.Add(this.cboxAVR);
            this.Controls.Add(this.cboxVTG);
            this.Controls.Add(this.tbarSpeed);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.tbarSteerAngleWAS);
            this.Controls.Add(this.label6);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FormSim";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Text = "Module Sim Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSim_FormClosing);
            this.Load += new System.EventHandler(this.FormSim_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbarSteerAngleWAS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarRoll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cboxKSXT;
        private System.Windows.Forms.CheckBox cboxNDA;
        private System.Windows.Forms.Label lblWAS;
        private System.Windows.Forms.TrackBar tbarSteerAngleWAS;
        private System.Windows.Forms.CheckBox cboxOGI;
        private System.Windows.Forms.CheckBox cboxGGA;
        private System.Windows.Forms.Label lblKmh;
        private System.Windows.Forms.Label mSec;
        private System.Windows.Forms.CheckBox cboxHDT;
        private System.Windows.Forms.CheckBox cboxAVR;
        private System.Windows.Forms.CheckBox cboxVTG;
        private System.Windows.Forms.TrackBar tbarSpeed;
        private System.Windows.Forms.Timer simTimer;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.CheckBox cboxRMC;
        private System.Windows.Forms.TrackBar tbarRoll;
        private System.Windows.Forms.Label lblRoll;
        private System.Windows.Forms.Label lblIPSet1;
        private System.Windows.Forms.Label lblIPSet2;
        private System.Windows.Forms.Label lblIPSet3;
        private System.Windows.Forms.NumericUpDown nudLat;
        private System.Windows.Forms.NumericUpDown nudLon;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblCurrentLat;
        private System.Windows.Forms.Label lblLatt;
        private System.Windows.Forms.Label lblCurrentLon;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label lblScanReply;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label lblInvertWAS_Tool;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label lblInvertSteer_Tool;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label lblMaxAngle_Tool;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblIntegral;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblXTE;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label lblAckerman;
        private System.Windows.Forms.Label lblMinPWM;
        private System.Windows.Forms.Label lblWAS_Offset;
        private System.Windows.Forms.Label lblKp;
        private System.Windows.Forms.Label lblHighPWM;
        private System.Windows.Forms.Label lblWAS_CPD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblIsSteer;
        private System.Windows.Forms.Button btn10Hz;
    }
}

