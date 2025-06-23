namespace AOG
{
    partial class FormToolSteer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormToolSteer));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblSteerAngle = new System.Windows.Forms.Label();
            this.lblSteerAngleActual = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGain = new System.Windows.Forms.TabPage();
            this.lblGain = new System.Windows.Forms.Label();
            this.hsbarMinPWM_Tool = new System.Windows.Forms.HScrollBar();
            this.hsbarHighPWM_Tool = new System.Windows.Forms.HScrollBar();
            this.lblMinPWM_Tool = new System.Windows.Forms.Label();
            this.lblHighPWM_Tool = new System.Windows.Forms.Label();
            this.lblPGain_Tool = new System.Windows.Forms.Label();
            this.hsbarPGain_Tool = new System.Windows.Forms.HScrollBar();
            this.label89 = new System.Windows.Forms.Label();
            this.label88 = new System.Windows.Forms.Label();
            this.label87 = new System.Windows.Forms.Label();
            this.tabSteer = new System.Windows.Forms.TabPage();
            this.lblAV_Set = new System.Windows.Forms.Label();
            this.lblAV_Act = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.lblActualSteerAngleUpper = new System.Windows.Forms.Label();
            this.btnZeroWAS_Tool = new System.Windows.Forms.Button();
            this.label96 = new System.Windows.Forms.Label();
            this.hsbarZeroWAS_Tool = new System.Windows.Forms.HScrollBar();
            this.lblMaxSteerAngle_Tool = new System.Windows.Forms.Label();
            this.label101 = new System.Windows.Forms.Label();
            this.lblAckermann_Tool = new System.Windows.Forms.Label();
            this.hsbarCPD_Tool = new System.Windows.Forms.HScrollBar();
            this.lblCPD_Tool = new System.Windows.Forms.Label();
            this.lblZeroWAS_Tool = new System.Windows.Forms.Label();
            this.hsbarMaxSteerAngle_Tool = new System.Windows.Forms.HScrollBar();
            this.label98 = new System.Windows.Forms.Label();
            this.hsbarAckermann_Tool = new System.Windows.Forms.HScrollBar();
            this.label99 = new System.Windows.Forms.Label();
            this.tabPP = new System.Windows.Forms.TabPage();
            this.label82 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.lblIntegral_Tool = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.hsbarIntegral_Tool = new System.Windows.Forms.HScrollBar();
            this.label27 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.hsbarAcquireFactor = new System.Windows.Forms.HScrollBar();
            this.lblAcquireFactor = new System.Windows.Forms.Label();
            this.hsbarLookAheadMult = new System.Windows.Forms.HScrollBar();
            this.label19 = new System.Windows.Forms.Label();
            this.lblLookAheadMult = new System.Windows.Forms.Label();
            this.tabPPAdv = new System.Windows.Forms.TabPage();
            this.label67 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.nudDeadZoneDelay = new AOG.NudlessNumericUpDown();
            this.nudDeadZoneHeading = new AOG.NudlessNumericUpDown();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblPWMDisplay = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnExpand = new AOG.RepeatButton();
            this.tabSteerSettings = new System.Windows.Forms.TabControl();
            this.tabOnTheLine = new System.Windows.Forms.TabPage();
            this.cboxIsSteerNotSlide_Tool = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudAntennaHeight_Tool = new AOG.NudlessNumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.nudAntennaOffset_Tool = new AOG.NudlessNumericUpDown();
            this.cboxGPSTool = new System.Windows.Forms.CheckBox();
            this.tabConfig = new System.Windows.Forms.TabPage();
            this.label83 = new System.Windows.Forms.Label();
            this.cboxInvertWAS_Tool = new System.Windows.Forms.CheckBox();
            this.label31 = new System.Windows.Forms.Label();
            this.cboxInvertSteer_Tool = new System.Windows.Forms.CheckBox();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1.SuspendLayout();
            this.tabGain.SuspendLayout();
            this.tabSteer.SuspendLayout();
            this.tabPP.SuspendLayout();
            this.tabPPAdv.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabSteerSettings.SuspendLayout();
            this.tabOnTheLine.SuspendLayout();
            this.tabConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // lblSteerAngle
            // 
            this.lblSteerAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.lblSteerAngle.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteerAngle.ForeColor = System.Drawing.Color.Black;
            this.lblSteerAngle.Location = new System.Drawing.Point(32, 65);
            this.lblSteerAngle.Name = "lblSteerAngle";
            this.lblSteerAngle.Size = new System.Drawing.Size(71, 23);
            this.lblSteerAngle.TabIndex = 306;
            this.lblSteerAngle.Text = "-55.5";
            this.lblSteerAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSteerAngle.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // lblSteerAngleActual
            // 
            this.lblSteerAngleActual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSteerAngleActual.BackColor = System.Drawing.Color.Transparent;
            this.lblSteerAngleActual.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteerAngleActual.ForeColor = System.Drawing.Color.Black;
            this.lblSteerAngleActual.Location = new System.Drawing.Point(131, 65);
            this.lblSteerAngleActual.Name = "lblSteerAngleActual";
            this.lblSteerAngleActual.Size = new System.Drawing.Size(71, 23);
            this.lblSteerAngleActual.TabIndex = 311;
            this.lblSteerAngleActual.Text = "-55.5";
            this.lblSteerAngleActual.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSteerAngleActual.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Black;
            this.lblError.Location = new System.Drawing.Point(225, 65);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(71, 23);
            this.lblError.TabIndex = 312;
            this.lblError.Text = "-30.0";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblError.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(101, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 16);
            this.label11.TabIndex = 319;
            this.label11.Text = "Act:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label11.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(4, 68);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 16);
            this.label12.TabIndex = 320;
            this.label12.Text = "Set:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label12.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(198, 68);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 16);
            this.label13.TabIndex = 321;
            this.label13.Text = "Err:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label13.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tabGain);
            this.tabControl1.Controls.Add(this.tabSteer);
            this.tabControl1.Controls.Add(this.tabPP);
            this.tabControl1.Controls.Add(this.tabPPAdv);
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.ItemSize = new System.Drawing.Size(90, 52);
            this.tabControl1.Location = new System.Drawing.Point(2, 5);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(373, 402);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 347;
            // 
            // tabGain
            // 
            this.tabGain.AutoScroll = true;
            this.tabGain.BackColor = System.Drawing.Color.LightGray;
            this.tabGain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabGain.Controls.Add(this.lblGain);
            this.tabGain.Controls.Add(this.hsbarMinPWM_Tool);
            this.tabGain.Controls.Add(this.hsbarHighPWM_Tool);
            this.tabGain.Controls.Add(this.lblMinPWM_Tool);
            this.tabGain.Controls.Add(this.lblHighPWM_Tool);
            this.tabGain.Controls.Add(this.lblPGain_Tool);
            this.tabGain.Controls.Add(this.hsbarPGain_Tool);
            this.tabGain.Controls.Add(this.label89);
            this.tabGain.Controls.Add(this.label88);
            this.tabGain.Controls.Add(this.label87);
            this.tabGain.ImageIndex = 1;
            this.tabGain.Location = new System.Drawing.Point(4, 56);
            this.tabGain.Name = "tabGain";
            this.tabGain.Size = new System.Drawing.Size(365, 342);
            this.tabGain.TabIndex = 13;
            // 
            // lblGain
            // 
            this.lblGain.AutoSize = true;
            this.lblGain.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGain.ForeColor = System.Drawing.Color.Black;
            this.lblGain.Location = new System.Drawing.Point(35, 14);
            this.lblGain.Name = "lblGain";
            this.lblGain.Size = new System.Drawing.Size(60, 25);
            this.lblGain.TabIndex = 339;
            this.lblGain.Text = "Gain";
            this.lblGain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarMinPWM_Tool
            // 
            this.hsbarMinPWM_Tool.LargeChange = 1;
            this.hsbarMinPWM_Tool.Location = new System.Drawing.Point(67, 272);
            this.hsbarMinPWM_Tool.Maximum = 200;
            this.hsbarMinPWM_Tool.Name = "hsbarMinPWM_Tool";
            this.hsbarMinPWM_Tool.Size = new System.Drawing.Size(292, 41);
            this.hsbarMinPWM_Tool.TabIndex = 557;
            this.hsbarMinPWM_Tool.Value = 10;
            this.hsbarMinPWM_Tool.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarMinPWM_Tool_Scroll);
            // 
            // hsbarHighPWM_Tool
            // 
            this.hsbarHighPWM_Tool.LargeChange = 2;
            this.hsbarHighPWM_Tool.Location = new System.Drawing.Point(67, 182);
            this.hsbarHighPWM_Tool.Maximum = 255;
            this.hsbarHighPWM_Tool.Name = "hsbarHighPWM_Tool";
            this.hsbarHighPWM_Tool.Size = new System.Drawing.Size(292, 41);
            this.hsbarHighPWM_Tool.TabIndex = 555;
            this.hsbarHighPWM_Tool.Value = 50;
            this.hsbarHighPWM_Tool.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarHighPWM_Tool_Scroll);
            // 
            // lblMinPWM_Tool
            // 
            this.lblMinPWM_Tool.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinPWM_Tool.ForeColor = System.Drawing.Color.Black;
            this.lblMinPWM_Tool.Location = new System.Drawing.Point(7, 273);
            this.lblMinPWM_Tool.Name = "lblMinPWM_Tool";
            this.lblMinPWM_Tool.Size = new System.Drawing.Size(61, 35);
            this.lblMinPWM_Tool.TabIndex = 558;
            this.lblMinPWM_Tool.Text = "888";
            this.lblMinPWM_Tool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHighPWM_Tool
            // 
            this.lblHighPWM_Tool.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHighPWM_Tool.ForeColor = System.Drawing.Color.Black;
            this.lblHighPWM_Tool.Location = new System.Drawing.Point(7, 183);
            this.lblHighPWM_Tool.Name = "lblHighPWM_Tool";
            this.lblHighPWM_Tool.Size = new System.Drawing.Size(61, 35);
            this.lblHighPWM_Tool.TabIndex = 556;
            this.lblHighPWM_Tool.Text = "888";
            this.lblHighPWM_Tool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPGain_Tool
            // 
            this.lblPGain_Tool.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPGain_Tool.ForeColor = System.Drawing.Color.Black;
            this.lblPGain_Tool.Location = new System.Drawing.Point(7, 93);
            this.lblPGain_Tool.Name = "lblPGain_Tool";
            this.lblPGain_Tool.Size = new System.Drawing.Size(61, 35);
            this.lblPGain_Tool.TabIndex = 554;
            this.lblPGain_Tool.Text = "888";
            this.lblPGain_Tool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarPGain_Tool
            // 
            this.hsbarPGain_Tool.LargeChange = 1;
            this.hsbarPGain_Tool.Location = new System.Drawing.Point(67, 92);
            this.hsbarPGain_Tool.Maximum = 200;
            this.hsbarPGain_Tool.Minimum = 1;
            this.hsbarPGain_Tool.Name = "hsbarPGain_Tool";
            this.hsbarPGain_Tool.Size = new System.Drawing.Size(292, 40);
            this.hsbarPGain_Tool.TabIndex = 553;
            this.hsbarPGain_Tool.Value = 4;
            this.hsbarPGain_Tool.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarPGain_Tool_Scroll);
            // 
            // label89
            // 
            this.label89.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label89.ForeColor = System.Drawing.Color.Black;
            this.label89.Location = new System.Drawing.Point(82, 64);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(262, 24);
            this.label89.TabIndex = 559;
            this.label89.Text = "Proportional Gain";
            this.label89.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label88
            // 
            this.label88.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label88.ForeColor = System.Drawing.Color.Black;
            this.label88.Location = new System.Drawing.Point(82, 155);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(262, 24);
            this.label88.TabIndex = 560;
            this.label88.Text = "Maximum PWM Limit";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label87
            // 
            this.label87.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label87.ForeColor = System.Drawing.Color.Black;
            this.label87.Location = new System.Drawing.Point(82, 244);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(262, 24);
            this.label87.TabIndex = 561;
            this.label87.Text = "Minimum PWM to Move";
            this.label87.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabSteer
            // 
            this.tabSteer.AutoScroll = true;
            this.tabSteer.BackColor = System.Drawing.Color.LightGray;
            this.tabSteer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabSteer.Controls.Add(this.lblAV_Set);
            this.tabSteer.Controls.Add(this.lblAV_Act);
            this.tabSteer.Controls.Add(this.label36);
            this.tabSteer.Controls.Add(this.label38);
            this.tabSteer.Controls.Add(this.lblActualSteerAngleUpper);
            this.tabSteer.Controls.Add(this.btnZeroWAS_Tool);
            this.tabSteer.Controls.Add(this.label96);
            this.tabSteer.Controls.Add(this.hsbarZeroWAS_Tool);
            this.tabSteer.Controls.Add(this.lblMaxSteerAngle_Tool);
            this.tabSteer.Controls.Add(this.label101);
            this.tabSteer.Controls.Add(this.lblAckermann_Tool);
            this.tabSteer.Controls.Add(this.hsbarCPD_Tool);
            this.tabSteer.Controls.Add(this.lblCPD_Tool);
            this.tabSteer.Controls.Add(this.lblZeroWAS_Tool);
            this.tabSteer.Controls.Add(this.hsbarMaxSteerAngle_Tool);
            this.tabSteer.Controls.Add(this.label98);
            this.tabSteer.Controls.Add(this.hsbarAckermann_Tool);
            this.tabSteer.Controls.Add(this.label99);
            this.tabSteer.ImageIndex = 4;
            this.tabSteer.Location = new System.Drawing.Point(4, 56);
            this.tabSteer.Name = "tabSteer";
            this.tabSteer.Size = new System.Drawing.Size(365, 342);
            this.tabSteer.TabIndex = 5;
            // 
            // lblAV_Set
            // 
            this.lblAV_Set.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAV_Set.AutoSize = true;
            this.lblAV_Set.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAV_Set.Location = new System.Drawing.Point(-334, 18);
            this.lblAV_Set.Name = "lblAV_Set";
            this.lblAV_Set.Size = new System.Drawing.Size(51, 19);
            this.lblAV_Set.TabIndex = 529;
            this.lblAV_Set.Text = "-55.8";
            this.lblAV_Set.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAV_Act
            // 
            this.lblAV_Act.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAV_Act.AutoSize = true;
            this.lblAV_Act.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAV_Act.Location = new System.Drawing.Point(-334, 42);
            this.lblAV_Act.Name = "lblAV_Act";
            this.lblAV_Act.Size = new System.Drawing.Size(54, 19);
            this.lblAV_Act.TabIndex = 528;
            this.lblAV_Act.Text = "66.89";
            this.lblAV_Act.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label36
            // 
            this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.Location = new System.Drawing.Point(-378, 44);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(50, 16);
            this.label36.TabIndex = 530;
            this.label36.Text = "AV Act:";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.Location = new System.Drawing.Point(-379, 20);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(51, 16);
            this.label38.TabIndex = 531;
            this.label38.Text = "AV Set:";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblActualSteerAngleUpper
            // 
            this.lblActualSteerAngleUpper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblActualSteerAngleUpper.AutoSize = true;
            this.lblActualSteerAngleUpper.BackColor = System.Drawing.Color.Transparent;
            this.lblActualSteerAngleUpper.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActualSteerAngleUpper.ForeColor = System.Drawing.Color.Black;
            this.lblActualSteerAngleUpper.Location = new System.Drawing.Point(-573, 16);
            this.lblActualSteerAngleUpper.Name = "lblActualSteerAngleUpper";
            this.lblActualSteerAngleUpper.Size = new System.Drawing.Size(39, 19);
            this.lblActualSteerAngleUpper.TabIndex = 324;
            this.lblActualSteerAngleUpper.Text = "255";
            this.lblActualSteerAngleUpper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnZeroWAS_Tool
            // 
            this.btnZeroWAS_Tool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnZeroWAS_Tool.FlatAppearance.BorderSize = 0;
            this.btnZeroWAS_Tool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZeroWAS_Tool.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZeroWAS_Tool.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnZeroWAS_Tool.Image = global::AOG.Properties.Resources.SteerZero;
            this.btnZeroWAS_Tool.Location = new System.Drawing.Point(31, 16);
            this.btnZeroWAS_Tool.Name = "btnZeroWAS_Tool";
            this.btnZeroWAS_Tool.Size = new System.Drawing.Size(70, 30);
            this.btnZeroWAS_Tool.TabIndex = 572;
            this.btnZeroWAS_Tool.UseVisualStyleBackColor = true;
            this.btnZeroWAS_Tool.Click += new System.EventHandler(this.btnZeroWAS_Tool_Click);
            // 
            // label96
            // 
            this.label96.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label96.ForeColor = System.Drawing.Color.Black;
            this.label96.Location = new System.Drawing.Point(116, 270);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(200, 24);
            this.label96.TabIndex = 577;
            this.label96.Text = "Max Steer Angle";
            this.label96.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarZeroWAS_Tool
            // 
            this.hsbarZeroWAS_Tool.LargeChange = 20;
            this.hsbarZeroWAS_Tool.Location = new System.Drawing.Point(76, 70);
            this.hsbarZeroWAS_Tool.Maximum = 4000;
            this.hsbarZeroWAS_Tool.Minimum = -4000;
            this.hsbarZeroWAS_Tool.Name = "hsbarZeroWAS_Tool";
            this.hsbarZeroWAS_Tool.Size = new System.Drawing.Size(281, 30);
            this.hsbarZeroWAS_Tool.SmallChange = 2;
            this.hsbarZeroWAS_Tool.TabIndex = 565;
            this.hsbarZeroWAS_Tool.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarZeroWAS_Tool_Scroll);
            // 
            // lblMaxSteerAngle_Tool
            // 
            this.lblMaxSteerAngle_Tool.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxSteerAngle_Tool.ForeColor = System.Drawing.Color.Black;
            this.lblMaxSteerAngle_Tool.Location = new System.Drawing.Point(12, 301);
            this.lblMaxSteerAngle_Tool.Name = "lblMaxSteerAngle_Tool";
            this.lblMaxSteerAngle_Tool.Size = new System.Drawing.Size(52, 35);
            this.lblMaxSteerAngle_Tool.TabIndex = 569;
            this.lblMaxSteerAngle_Tool.Text = "888";
            this.lblMaxSteerAngle_Tool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label101
            // 
            this.label101.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label101.ForeColor = System.Drawing.Color.Black;
            this.label101.Location = new System.Drawing.Point(116, 42);
            this.label101.Name = "label101";
            this.label101.Size = new System.Drawing.Size(200, 24);
            this.label101.TabIndex = 566;
            this.label101.Text = "WAS Zero";
            this.label101.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAckermann_Tool
            // 
            this.lblAckermann_Tool.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAckermann_Tool.ForeColor = System.Drawing.Color.Black;
            this.lblAckermann_Tool.Location = new System.Drawing.Point(9, 228);
            this.lblAckermann_Tool.Name = "lblAckermann_Tool";
            this.lblAckermann_Tool.Size = new System.Drawing.Size(55, 35);
            this.lblAckermann_Tool.TabIndex = 574;
            this.lblAckermann_Tool.Text = "888";
            this.lblAckermann_Tool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarCPD_Tool
            // 
            this.hsbarCPD_Tool.LargeChange = 1;
            this.hsbarCPD_Tool.Location = new System.Drawing.Point(76, 146);
            this.hsbarCPD_Tool.Maximum = 255;
            this.hsbarCPD_Tool.Minimum = 1;
            this.hsbarCPD_Tool.Name = "hsbarCPD_Tool";
            this.hsbarCPD_Tool.Size = new System.Drawing.Size(281, 30);
            this.hsbarCPD_Tool.TabIndex = 570;
            this.hsbarCPD_Tool.Value = 20;
            this.hsbarCPD_Tool.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarCPD_Tool_Scroll);
            // 
            // lblCPD_Tool
            // 
            this.lblCPD_Tool.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCPD_Tool.ForeColor = System.Drawing.Color.Black;
            this.lblCPD_Tool.Location = new System.Drawing.Point(9, 154);
            this.lblCPD_Tool.Name = "lblCPD_Tool";
            this.lblCPD_Tool.Size = new System.Drawing.Size(55, 35);
            this.lblCPD_Tool.TabIndex = 571;
            this.lblCPD_Tool.Text = "888";
            this.lblCPD_Tool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblZeroWAS_Tool
            // 
            this.lblZeroWAS_Tool.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZeroWAS_Tool.ForeColor = System.Drawing.Color.Black;
            this.lblZeroWAS_Tool.Location = new System.Drawing.Point(6, 70);
            this.lblZeroWAS_Tool.Name = "lblZeroWAS_Tool";
            this.lblZeroWAS_Tool.Size = new System.Drawing.Size(70, 35);
            this.lblZeroWAS_Tool.TabIndex = 567;
            this.lblZeroWAS_Tool.Text = "-55.88";
            this.lblZeroWAS_Tool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarMaxSteerAngle_Tool
            // 
            this.hsbarMaxSteerAngle_Tool.LargeChange = 1;
            this.hsbarMaxSteerAngle_Tool.Location = new System.Drawing.Point(76, 298);
            this.hsbarMaxSteerAngle_Tool.Maximum = 80;
            this.hsbarMaxSteerAngle_Tool.Minimum = 10;
            this.hsbarMaxSteerAngle_Tool.Name = "hsbarMaxSteerAngle_Tool";
            this.hsbarMaxSteerAngle_Tool.Size = new System.Drawing.Size(281, 30);
            this.hsbarMaxSteerAngle_Tool.TabIndex = 568;
            this.hsbarMaxSteerAngle_Tool.Value = 10;
            this.hsbarMaxSteerAngle_Tool.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarMaxSteerAngle_Tool_Scroll);
            // 
            // label98
            // 
            this.label98.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label98.ForeColor = System.Drawing.Color.Black;
            this.label98.Location = new System.Drawing.Point(116, 195);
            this.label98.Name = "label98";
            this.label98.Size = new System.Drawing.Size(200, 24);
            this.label98.TabIndex = 576;
            this.label98.Text = "Ackermann";
            this.label98.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarAckermann_Tool
            // 
            this.hsbarAckermann_Tool.LargeChange = 1;
            this.hsbarAckermann_Tool.Location = new System.Drawing.Point(76, 222);
            this.hsbarAckermann_Tool.Maximum = 200;
            this.hsbarAckermann_Tool.Minimum = 1;
            this.hsbarAckermann_Tool.Name = "hsbarAckermann_Tool";
            this.hsbarAckermann_Tool.Size = new System.Drawing.Size(281, 30);
            this.hsbarAckermann_Tool.TabIndex = 573;
            this.hsbarAckermann_Tool.Value = 100;
            this.hsbarAckermann_Tool.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarAckermann_Tool_Scroll);
            // 
            // label99
            // 
            this.label99.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label99.ForeColor = System.Drawing.Color.Black;
            this.label99.Location = new System.Drawing.Point(116, 118);
            this.label99.Name = "label99";
            this.label99.Size = new System.Drawing.Size(200, 24);
            this.label99.TabIndex = 575;
            this.label99.Text = "Counts per Degree";
            this.label99.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPP
            // 
            this.tabPP.BackColor = System.Drawing.Color.LightGray;
            this.tabPP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabPP.Controls.Add(this.label82);
            this.tabPP.Controls.Add(this.label21);
            this.tabPP.Controls.Add(this.lblIntegral_Tool);
            this.tabPP.Controls.Add(this.label20);
            this.tabPP.Controls.Add(this.hsbarIntegral_Tool);
            this.tabPP.Controls.Add(this.label27);
            this.tabPP.Controls.Add(this.label18);
            this.tabPP.Controls.Add(this.hsbarAcquireFactor);
            this.tabPP.Controls.Add(this.lblAcquireFactor);
            this.tabPP.Controls.Add(this.hsbarLookAheadMult);
            this.tabPP.Controls.Add(this.label19);
            this.tabPP.Controls.Add(this.lblLookAheadMult);
            this.tabPP.ForeColor = System.Drawing.Color.Black;
            this.tabPP.ImageIndex = 0;
            this.tabPP.Location = new System.Drawing.Point(4, 56);
            this.tabPP.Name = "tabPP";
            this.tabPP.Size = new System.Drawing.Size(365, 342);
            this.tabPP.TabIndex = 16;
            // 
            // label82
            // 
            this.label82.Enabled = false;
            this.label82.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label82.ForeColor = System.Drawing.Color.Black;
            this.label82.Location = new System.Drawing.Point(78, 113);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(273, 26);
            this.label82.TabIndex = 548;
            this.label82.Text = "Acquire Factor";
            this.label82.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.Black;
            this.label21.Location = new System.Drawing.Point(100, 288);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(238, 46);
            this.label21.TabIndex = 361;
            this.label21.Text = "It will slowly increase steer angle to reduce cross track error";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblIntegral_Tool
            // 
            this.lblIntegral_Tool.Enabled = false;
            this.lblIntegral_Tool.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIntegral_Tool.ForeColor = System.Drawing.Color.Black;
            this.lblIntegral_Tool.Location = new System.Drawing.Point(1, 243);
            this.lblIntegral_Tool.Name = "lblIntegral_Tool";
            this.lblIntegral_Tool.Size = new System.Drawing.Size(60, 35);
            this.lblIntegral_Tool.TabIndex = 352;
            this.lblIntegral_Tool.Text = "888";
            this.lblIntegral_Tool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label20.Location = new System.Drawing.Point(579, 228);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(204, 30);
            this.label20.TabIndex = 302;
            this.label20.Text = "Look Ahead Min";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarIntegral_Tool
            // 
            this.hsbarIntegral_Tool.Enabled = false;
            this.hsbarIntegral_Tool.LargeChange = 1;
            this.hsbarIntegral_Tool.Location = new System.Drawing.Point(68, 242);
            this.hsbarIntegral_Tool.Name = "hsbarIntegral_Tool";
            this.hsbarIntegral_Tool.Size = new System.Drawing.Size(292, 40);
            this.hsbarIntegral_Tool.TabIndex = 351;
            this.hsbarIntegral_Tool.Value = 5;
            this.hsbarIntegral_Tool.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarIntegral_Tool_Scroll);
            // 
            // label27
            // 
            this.label27.Enabled = false;
            this.label27.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.Black;
            this.label27.Location = new System.Drawing.Point(78, 213);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(273, 26);
            this.label27.TabIndex = 350;
            this.label27.Text = "Integral";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label27.UseCompatibleTextRendering = true;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label18.Location = new System.Drawing.Point(561, 13);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(204, 30);
            this.label18.TabIndex = 300;
            this.label18.Text = "Look Ahead";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarAcquireFactor
            // 
            this.hsbarAcquireFactor.Enabled = false;
            this.hsbarAcquireFactor.LargeChange = 1;
            this.hsbarAcquireFactor.Location = new System.Drawing.Point(68, 142);
            this.hsbarAcquireFactor.Maximum = 300;
            this.hsbarAcquireFactor.Minimum = 20;
            this.hsbarAcquireFactor.Name = "hsbarAcquireFactor";
            this.hsbarAcquireFactor.Size = new System.Drawing.Size(292, 40);
            this.hsbarAcquireFactor.TabIndex = 508;
            this.hsbarAcquireFactor.Value = 75;
            this.hsbarAcquireFactor.ValueChanged += new System.EventHandler(this.hsbarAcquireFactor_ValueChanged);
            // 
            // lblAcquireFactor
            // 
            this.lblAcquireFactor.Enabled = false;
            this.lblAcquireFactor.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAcquireFactor.ForeColor = System.Drawing.Color.Black;
            this.lblAcquireFactor.Location = new System.Drawing.Point(3, 146);
            this.lblAcquireFactor.Name = "lblAcquireFactor";
            this.lblAcquireFactor.Size = new System.Drawing.Size(60, 35);
            this.lblAcquireFactor.TabIndex = 509;
            this.lblAcquireFactor.Text = "888";
            this.lblAcquireFactor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarLookAheadMult
            // 
            this.hsbarLookAheadMult.Enabled = false;
            this.hsbarLookAheadMult.LargeChange = 1;
            this.hsbarLookAheadMult.Location = new System.Drawing.Point(68, 42);
            this.hsbarLookAheadMult.Maximum = 60;
            this.hsbarLookAheadMult.Minimum = 5;
            this.hsbarLookAheadMult.Name = "hsbarLookAheadMult";
            this.hsbarLookAheadMult.Size = new System.Drawing.Size(292, 40);
            this.hsbarLookAheadMult.TabIndex = 298;
            this.hsbarLookAheadMult.Value = 6;
            // 
            // label19
            // 
            this.label19.Enabled = false;
            this.label19.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.Location = new System.Drawing.Point(78, 13);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(273, 26);
            this.label19.TabIndex = 301;
            this.label19.Text = "Speed Factor";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLookAheadMult
            // 
            this.lblLookAheadMult.Enabled = false;
            this.lblLookAheadMult.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLookAheadMult.ForeColor = System.Drawing.Color.Black;
            this.lblLookAheadMult.Location = new System.Drawing.Point(8, 44);
            this.lblLookAheadMult.Name = "lblLookAheadMult";
            this.lblLookAheadMult.Size = new System.Drawing.Size(55, 35);
            this.lblLookAheadMult.TabIndex = 299;
            this.lblLookAheadMult.Text = "888";
            this.lblLookAheadMult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPPAdv
            // 
            this.tabPPAdv.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPPAdv.Controls.Add(this.label67);
            this.tabPPAdv.Controls.Add(this.label66);
            this.tabPPAdv.Controls.Add(this.label51);
            this.tabPPAdv.Controls.Add(this.label54);
            this.tabPPAdv.Controls.Add(this.label49);
            this.tabPPAdv.Controls.Add(this.nudDeadZoneDelay);
            this.tabPPAdv.Controls.Add(this.nudDeadZoneHeading);
            this.tabPPAdv.ImageIndex = 2;
            this.tabPPAdv.Location = new System.Drawing.Point(4, 56);
            this.tabPPAdv.Name = "tabPPAdv";
            this.tabPPAdv.Padding = new System.Windows.Forms.Padding(3);
            this.tabPPAdv.Size = new System.Drawing.Size(365, 342);
            this.tabPPAdv.TabIndex = 17;
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.BackColor = System.Drawing.Color.Transparent;
            this.label67.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label67.ForeColor = System.Drawing.Color.Black;
            this.label67.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label67.Location = new System.Drawing.Point(336, 81);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(14, 16);
            this.label67.TabIndex = 547;
            this.label67.Text = "5";
            this.label67.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.BackColor = System.Drawing.Color.Transparent;
            this.label66.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label66.ForeColor = System.Drawing.Color.Black;
            this.label66.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label66.Location = new System.Drawing.Point(26, 81);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(25, 16);
            this.label66.TabIndex = 546;
            this.label66.Text = "0.1";
            this.label66.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label51
            // 
            this.label51.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label51.ForeColor = System.Drawing.Color.Black;
            this.label51.Location = new System.Drawing.Point(20, 30);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(329, 25);
            this.label51.TabIndex = 541;
            this.label51.Text = " ------ Dead Zone -----";
            this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label54
            // 
            this.label54.BackColor = System.Drawing.Color.Transparent;
            this.label54.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label54.ForeColor = System.Drawing.Color.Black;
            this.label54.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label54.Location = new System.Drawing.Point(195, 105);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(165, 22);
            this.label54.TabIndex = 543;
            this.label54.Text = "On Delay (sec)";
            this.label54.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label49
            // 
            this.label49.BackColor = System.Drawing.Color.Transparent;
            this.label49.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label49.ForeColor = System.Drawing.Color.Black;
            this.label49.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label49.Location = new System.Drawing.Point(26, 105);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(165, 22);
            this.label49.TabIndex = 539;
            this.label49.Text = "Heading (Degree)";
            this.label49.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // nudDeadZoneDelay
            // 
            this.nudDeadZoneDelay.Location = new System.Drawing.Point(224, 70);
            this.nudDeadZoneDelay.Maximum = 10D;
            this.nudDeadZoneDelay.Minimum = 1D;
            this.nudDeadZoneDelay.Name = "nudDeadZoneDelay";
            this.nudDeadZoneDelay.Size = new System.Drawing.Size(107, 36);
            this.nudDeadZoneDelay.TabIndex = 542;
            this.nudDeadZoneDelay.ValueChanged += new System.EventHandler(this.nudDeadZoneDelay_ValueChanged);
            // 
            // nudDeadZoneHeading
            // 
            this.nudDeadZoneHeading.DecimalPlaces = 1;
            this.nudDeadZoneHeading.Location = new System.Drawing.Point(56, 71);
            this.nudDeadZoneHeading.Maximum = 5D;
            this.nudDeadZoneHeading.Minimum = 0.1D;
            this.nudDeadZoneHeading.Name = "nudDeadZoneHeading";
            this.nudDeadZoneHeading.Size = new System.Drawing.Size(107, 36);
            this.nudDeadZoneHeading.TabIndex = 538;
            this.nudDeadZoneHeading.ValueChanged += new System.EventHandler(this.nudDeadZoneHeading_ValueChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Sf_PPTab.png");
            this.imageList1.Images.SetKeyName(1, "ST_GainTab.png");
            this.imageList1.Images.SetKeyName(2, "ST_NerdAdv.png");
            this.imageList1.Images.SetKeyName(3, "ST_StanleyTab.png");
            this.imageList1.Images.SetKeyName(4, "ST_SteerTab.png");
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.lblPWMDisplay);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.btnExpand);
            this.panel2.Controls.Add(this.lblError);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.lblSteerAngle);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.lblSteerAngleActual);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Location = new System.Drawing.Point(4, 403);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(366, 100);
            this.panel2.TabIndex = 324;
            // 
            // lblPWMDisplay
            // 
            this.lblPWMDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPWMDisplay.BackColor = System.Drawing.Color.Transparent;
            this.lblPWMDisplay.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPWMDisplay.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblPWMDisplay.Location = new System.Drawing.Point(64, 17);
            this.lblPWMDisplay.Name = "lblPWMDisplay";
            this.lblPWMDisplay.Size = new System.Drawing.Size(61, 23);
            this.lblPWMDisplay.TabIndex = 330;
            this.lblPWMDisplay.Text = "255";
            this.lblPWMDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Location = new System.Drawing.Point(9, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 23);
            this.label9.TabIndex = 331;
            this.label9.Text = "PWM:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnExpand
            // 
            this.btnExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExpand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExpand.FlatAppearance.BorderColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnExpand.FlatAppearance.BorderSize = 0;
            this.btnExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpand.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpand.Image = global::AOG.Properties.Resources.ArrowRight;
            this.btnExpand.Location = new System.Drawing.Point(299, 60);
            this.btnExpand.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(62, 37);
            this.btnExpand.TabIndex = 329;
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // tabSteerSettings
            // 
            this.tabSteerSettings.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabSteerSettings.Controls.Add(this.tabOnTheLine);
            this.tabSteerSettings.Controls.Add(this.tabConfig);
            this.tabSteerSettings.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabSteerSettings.ImageList = this.imageList2;
            this.tabSteerSettings.ItemSize = new System.Drawing.Size(200, 54);
            this.tabSteerSettings.Location = new System.Drawing.Point(373, 5);
            this.tabSteerSettings.Name = "tabSteerSettings";
            this.tabSteerSettings.SelectedIndex = 0;
            this.tabSteerSettings.Size = new System.Drawing.Size(516, 502);
            this.tabSteerSettings.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabSteerSettings.TabIndex = 528;
            // 
            // tabOnTheLine
            // 
            this.tabOnTheLine.BackColor = System.Drawing.Color.Gainsboro;
            this.tabOnTheLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabOnTheLine.Controls.Add(this.cboxIsSteerNotSlide_Tool);
            this.tabOnTheLine.Controls.Add(this.label4);
            this.tabOnTheLine.Controls.Add(this.nudAntennaHeight_Tool);
            this.tabOnTheLine.Controls.Add(this.label2);
            this.tabOnTheLine.Controls.Add(this.label3);
            this.tabOnTheLine.Controls.Add(this.label74);
            this.tabOnTheLine.Controls.Add(this.nudAntennaOffset_Tool);
            this.tabOnTheLine.Controls.Add(this.cboxGPSTool);
            this.tabOnTheLine.ImageIndex = 1;
            this.tabOnTheLine.Location = new System.Drawing.Point(4, 58);
            this.tabOnTheLine.Name = "tabOnTheLine";
            this.tabOnTheLine.Padding = new System.Windows.Forms.Padding(3);
            this.tabOnTheLine.Size = new System.Drawing.Size(508, 440);
            this.tabOnTheLine.TabIndex = 4;
            this.tabOnTheLine.Enter += new System.EventHandler(this.tabOnTheLine_Enter);
            this.tabOnTheLine.Leave += new System.EventHandler(this.tabOnTheLine_Leave);
            // 
            // cboxIsSteerNotSlide_Tool
            // 
            this.cboxIsSteerNotSlide_Tool.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsSteerNotSlide_Tool.BackColor = System.Drawing.Color.White;
            this.cboxIsSteerNotSlide_Tool.BackgroundImage = global::AOG.Properties.Resources.ConS_ModulesSteer;
            this.cboxIsSteerNotSlide_Tool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cboxIsSteerNotSlide_Tool.Checked = true;
            this.cboxIsSteerNotSlide_Tool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxIsSteerNotSlide_Tool.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxIsSteerNotSlide_Tool.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxIsSteerNotSlide_Tool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsSteerNotSlide_Tool.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsSteerNotSlide_Tool.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxIsSteerNotSlide_Tool.Location = new System.Drawing.Point(321, 71);
            this.cboxIsSteerNotSlide_Tool.Name = "cboxIsSteerNotSlide_Tool";
            this.cboxIsSteerNotSlide_Tool.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsSteerNotSlide_Tool.Size = new System.Drawing.Size(82, 78);
            this.cboxIsSteerNotSlide_Tool.TabIndex = 578;
            this.cboxIsSteerNotSlide_Tool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsSteerNotSlide_Tool.UseVisualStyleBackColor = false;
            this.cboxIsSteerNotSlide_Tool.Click += new System.EventHandler(this.cboxIsSteerNotSlide_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(85, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 38);
            this.label4.TabIndex = 584;
            this.label4.Text = "Tool Steer \r\nOn/Off";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudAntennaHeight_Tool
            // 
            this.nudAntennaHeight_Tool.Location = new System.Drawing.Point(53, 324);
            this.nudAntennaHeight_Tool.Maximum = 5D;
            this.nudAntennaHeight_Tool.Mode = AOG.UnitMode.Small;
            this.nudAntennaHeight_Tool.Name = "nudAntennaHeight_Tool";
            this.nudAntennaHeight_Tool.Size = new System.Drawing.Size(143, 56);
            this.nudAntennaHeight_Tool.TabIndex = 580;
            this.nudAntennaHeight_Tool.ValueChanged += new System.EventHandler(this.nudAntennaHeight_Tool_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(68, 301);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 19);
            this.label2.TabIndex = 581;
            this.label2.Text = "Antenna Height";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(308, 301);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 19);
            this.label3.TabIndex = 583;
            this.label3.Text = "Antenna Offset";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label74.ForeColor = System.Drawing.Color.Black;
            this.label74.Location = new System.Drawing.Point(329, 35);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(74, 32);
            this.label74.TabIndex = 579;
            this.label74.Text = "Steering\r\nNot Sliding";
            this.label74.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudAntennaOffset_Tool
            // 
            this.nudAntennaOffset_Tool.Location = new System.Drawing.Point(305, 324);
            this.nudAntennaOffset_Tool.Maximum = 5D;
            this.nudAntennaOffset_Tool.Minimum = -5D;
            this.nudAntennaOffset_Tool.Mode = AOG.UnitMode.Small;
            this.nudAntennaOffset_Tool.Name = "nudAntennaOffset_Tool";
            this.nudAntennaOffset_Tool.Size = new System.Drawing.Size(143, 56);
            this.nudAntennaOffset_Tool.TabIndex = 582;
            this.nudAntennaOffset_Tool.ValueChanged += new System.EventHandler(this.nudAntennaOffset_Tool_ValueChanged);
            // 
            // cboxGPSTool
            // 
            this.cboxGPSTool.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxGPSTool.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxGPSTool.Checked = true;
            this.cboxGPSTool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxGPSTool.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightGreen;
            this.cboxGPSTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxGPSTool.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxGPSTool.ForeColor = System.Drawing.Color.Black;
            this.cboxGPSTool.Location = new System.Drawing.Point(67, 90);
            this.cboxGPSTool.Name = "cboxGPSTool";
            this.cboxGPSTool.Size = new System.Drawing.Size(120, 47);
            this.cboxGPSTool.TabIndex = 485;
            this.cboxGPSTool.Text = "Tool On";
            this.cboxGPSTool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxGPSTool.UseVisualStyleBackColor = false;
            this.cboxGPSTool.Click += new System.EventHandler(this.cboxGPSTool_Click);
            // 
            // tabConfig
            // 
            this.tabConfig.BackColor = System.Drawing.Color.LightGray;
            this.tabConfig.Controls.Add(this.label83);
            this.tabConfig.Controls.Add(this.cboxInvertWAS_Tool);
            this.tabConfig.Controls.Add(this.label31);
            this.tabConfig.Controls.Add(this.cboxInvertSteer_Tool);
            this.tabConfig.ImageIndex = 3;
            this.tabConfig.Location = new System.Drawing.Point(4, 58);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfig.Size = new System.Drawing.Size(508, 440);
            this.tabConfig.TabIndex = 1;
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label83.ForeColor = System.Drawing.Color.Black;
            this.label83.Location = new System.Drawing.Point(184, 306);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(68, 32);
            this.label83.TabIndex = 518;
            this.label83.Text = "Invert \r\nMotor Dir";
            this.label83.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxInvertWAS_Tool
            // 
            this.cboxInvertWAS_Tool.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxInvertWAS_Tool.BackColor = System.Drawing.Color.White;
            this.cboxInvertWAS_Tool.BackgroundImage = global::AOG.Properties.Resources.ConSt_InvertWAS;
            this.cboxInvertWAS_Tool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cboxInvertWAS_Tool.Checked = true;
            this.cboxInvertWAS_Tool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxInvertWAS_Tool.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxInvertWAS_Tool.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxInvertWAS_Tool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxInvertWAS_Tool.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxInvertWAS_Tool.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxInvertWAS_Tool.Location = new System.Drawing.Point(369, 340);
            this.cboxInvertWAS_Tool.Name = "cboxInvertWAS_Tool";
            this.cboxInvertWAS_Tool.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxInvertWAS_Tool.Size = new System.Drawing.Size(82, 78);
            this.cboxInvertWAS_Tool.TabIndex = 516;
            this.cboxInvertWAS_Tool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxInvertWAS_Tool.UseVisualStyleBackColor = false;
            this.cboxInvertWAS_Tool.Click += new System.EventHandler(this.cboxInvertWAS_Tool_Click);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.Black;
            this.label31.Location = new System.Drawing.Point(369, 320);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(83, 16);
            this.label31.TabIndex = 519;
            this.label31.Text = "Invert WAS";
            // 
            // cboxInvertSteer_Tool
            // 
            this.cboxInvertSteer_Tool.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxInvertSteer_Tool.BackColor = System.Drawing.Color.White;
            this.cboxInvertSteer_Tool.BackgroundImage = global::AOG.Properties.Resources.ConSt_InvertDirection;
            this.cboxInvertSteer_Tool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cboxInvertSteer_Tool.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxInvertSteer_Tool.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxInvertSteer_Tool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxInvertSteer_Tool.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxInvertSteer_Tool.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxInvertSteer_Tool.Location = new System.Drawing.Point(182, 340);
            this.cboxInvertSteer_Tool.Name = "cboxInvertSteer_Tool";
            this.cboxInvertSteer_Tool.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxInvertSteer_Tool.Size = new System.Drawing.Size(82, 78);
            this.cboxInvertSteer_Tool.TabIndex = 517;
            this.cboxInvertSteer_Tool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxInvertSteer_Tool.UseVisualStyleBackColor = false;
            this.cboxInvertSteer_Tool.Click += new System.EventHandler(this.cboxInvertSteer_Tool_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "ConS_Alarm.png");
            this.imageList2.Images.SetKeyName(1, "ConS_ImplementConfig.png");
            this.imageList2.Images.SetKeyName(2, "ConS_ModulesSteer.png");
            this.imageList2.Images.SetKeyName(3, "ConS_Pins.png");
            this.imageList2.Images.SetKeyName(4, "Sensors.png");
            this.imageList2.Images.SetKeyName(5, "Con_ImplementMenu.png");
            // 
            // FormToolSteer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(889, 510);
            this.Controls.Add(this.tabSteerSettings);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormToolSteer";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tool Steer Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSteer_FormClosing);
            this.Load += new System.EventHandler(this.FormSteer_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabGain.ResumeLayout(false);
            this.tabGain.PerformLayout();
            this.tabSteer.ResumeLayout(false);
            this.tabSteer.PerformLayout();
            this.tabPP.ResumeLayout(false);
            this.tabPPAdv.ResumeLayout(false);
            this.tabPPAdv.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabSteerSettings.ResumeLayout(false);
            this.tabOnTheLine.ResumeLayout(false);
            this.tabOnTheLine.PerformLayout();
            this.tabConfig.ResumeLayout(false);
            this.tabConfig.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.HScrollBar hsbarLookAheadMult;
        private System.Windows.Forms.Label lblLookAheadMult;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblSteerAngle;
        private System.Windows.Forms.Label lblSteerAngleActual;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGain;
        private System.Windows.Forms.TabPage tabPP;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TabPage tabSteer;
        private System.Windows.Forms.Label lblActualSteerAngleUpper;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.HScrollBar hsbarIntegral_Tool;
        private System.Windows.Forms.Label lblIntegral_Tool;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblGain;
        private System.Windows.Forms.Label lblAV_Set;
        private System.Windows.Forms.Label lblAV_Act;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label38;
        private RepeatButton btnExpand;
        private System.Windows.Forms.TabControl tabSteerSettings;
        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.Label label49;
        private NudlessNumericUpDown nudDeadZoneHeading;
        private System.Windows.Forms.TabPage tabOnTheLine;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label54;
        private NudlessNumericUpDown nudDeadZoneDelay;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label lblAcquireFactor;
        private System.Windows.Forms.HScrollBar hsbarAcquireFactor;
        private System.Windows.Forms.TabPage tabPPAdv;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.CheckBox cboxGPSTool;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.HScrollBar hsbarMinPWM_Tool;
        private System.Windows.Forms.HScrollBar hsbarPGain_Tool;
        private System.Windows.Forms.Label lblPGain_Tool;
        private System.Windows.Forms.Label lblHighPWM_Tool;
        private System.Windows.Forms.Label lblMinPWM_Tool;
        private System.Windows.Forms.HScrollBar hsbarHighPWM_Tool;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.CheckBox cboxInvertWAS_Tool;
        private System.Windows.Forms.CheckBox cboxInvertSteer_Tool;
        private System.Windows.Forms.Label label96;
        private System.Windows.Forms.Label lblMaxSteerAngle_Tool;
        private System.Windows.Forms.Label label98;
        private System.Windows.Forms.Label label99;
        private System.Windows.Forms.HScrollBar hsbarAckermann_Tool;
        private System.Windows.Forms.HScrollBar hsbarMaxSteerAngle_Tool;
        private System.Windows.Forms.Label lblAckermann_Tool;
        private System.Windows.Forms.Button btnZeroWAS_Tool;
        private System.Windows.Forms.HScrollBar hsbarCPD_Tool;
        private System.Windows.Forms.Label label101;
        private System.Windows.Forms.Label lblCPD_Tool;
        private System.Windows.Forms.HScrollBar hsbarZeroWAS_Tool;
        private System.Windows.Forms.Label lblZeroWAS_Tool;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.CheckBox cboxIsSteerNotSlide_Tool;
        private System.Windows.Forms.Label label2;
        private NudlessNumericUpDown nudAntennaHeight_Tool;
        private System.Windows.Forms.Label label3;
        private NudlessNumericUpDown nudAntennaOffset_Tool;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPWMDisplay;
        private System.Windows.Forms.Label label9;
    }
}