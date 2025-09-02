namespace AOG
{
    partial class FormGPS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGPS));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.menustripLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageDanish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageDeutsch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageSpanish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageFrench = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageItalian = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageLatvian = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageLithuanian = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageHungarian = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageDutch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguagePolish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguagePortugese = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageRussian = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageSerbian = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageFinnish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageSlovak = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageUkranian = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageTurkish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageChinese = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageTest = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGenerateLanguageReference = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.simulatorOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enterSimCoordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.kioskModeToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.resetALLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetEverythingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nozzleAppToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.agShareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrWatchdog = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStripFlag = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemFlagRed = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuFlagGrn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuFlagYel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuFlagForm = new System.Windows.Forms.ToolStripMenuItem();
            this.cboxpRowWidth = new System.Windows.Forms.ComboBox();
            this.oglZoom = new OpenTK.GLControl();
            this.btnResetSim = new System.Windows.Forms.Button();
            this.btnResetSteerAngle = new System.Windows.Forms.Button();
            this.timerSim = new System.Windows.Forms.Timer(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.hsbarSteerAngle = new System.Windows.Forms.HScrollBar();
            this.oglMain = new OpenTK.GLControl();
            this.oglBack = new OpenTK.GLControl();
            this.lblHz = new System.Windows.Forms.Label();
            this.statusStripLeft = new System.Windows.Forms.StatusStrip();
            this.distanceToolBtn = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripDropDownButton4 = new System.Windows.Forms.ToolStripDropDownButton();
            this.wizardsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.steerWizardMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.steerChartStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.steerChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headingChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xTEChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rollCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boundaryToolToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.eventViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guidelinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SmoothABtoolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteContourPathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webcamToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.offsetFixToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSim = new System.Windows.Forms.TableLayoutPanel();
            this.btnSpeedDn = new AOG.RepeatButton();
            this.btnSimSpeedUp = new AOG.RepeatButton();
            this.btnSimSetSpeedToZero = new System.Windows.Forms.Button();
            this.btnSimReverseDirection = new System.Windows.Forms.Button();
            this.panelNavigation = new System.Windows.Forms.TableLayoutPanel();
            this.btnTiltDn = new System.Windows.Forms.Button();
            this.btnTiltUp = new System.Windows.Forms.Button();
            this.btnBrightnessDn = new System.Windows.Forms.Button();
            this.btnBrightnessUp = new System.Windows.Forms.Button();
            this.btnDayNightMode = new System.Windows.Forms.Button();
            this.btn3D = new System.Windows.Forms.Button();
            this.btn2D = new System.Windows.Forms.Button();
            this.btnGrid = new System.Windows.Forms.Button();
            this.btnN2D = new System.Windows.Forms.Button();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblFix = new System.Windows.Forms.Label();
            this.panelLeft = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSteerSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSteerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAllSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripWorkingDirectories = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripGPSData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripColors = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSectionColors = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripHotkeys = new System.Windows.Forms.ToolStripMenuItem();
            this.btnNavigationSettings = new System.Windows.Forms.Button();
            this.btnAutoSteerConfig = new System.Windows.Forms.Button();
            this.btnStartAgIO = new System.Windows.Forms.Button();
            this.btnFieldMenu = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripBtnFieldTools = new System.Windows.Forms.ToolStripDropDownButton();
            this.boundariesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headlandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headlandBuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tramsMultiMenuField = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAppliedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flagByLatLonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordedPathStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.lblCurrentField = new System.Windows.Forms.Label();
            this.lblGuidanceLine = new System.Windows.Forms.Label();
            this.flp1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNudge = new System.Windows.Forms.Button();
            this.btnABDraw = new System.Windows.Forms.Button();
            this.btnPlusAB = new System.Windows.Forms.Button();
            this.btnBuildTracks = new System.Windows.Forms.Button();
            this.btnTracksOff = new System.Windows.Forms.Button();
            this.btnRefNudge = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTrack = new System.Windows.Forms.Button();
            this.btnSnapToPivot = new System.Windows.Forms.Button();
            this.btnAdjRight = new System.Windows.Forms.Button();
            this.btnAdjLeft = new System.Windows.Forms.Button();
            this.btnFlag = new System.Windows.Forms.Button();
            this.btnHeadlandOnOff = new System.Windows.Forms.Button();
            this.cboxIsSectionControlled = new System.Windows.Forms.CheckBox();
            this.btnHydLift = new System.Windows.Forms.Button();
            this.btnTramDisplayMode = new System.Windows.Forms.Button();
            this.btnResetToolHeading = new System.Windows.Forms.Button();
            this.btnChangeMappingColor = new System.Windows.Forms.Button();
            this.btnYouSkipEnable = new System.Windows.Forms.Button();
            this.panelRight = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAutoSteer = new System.Windows.Forms.Button();
            this.btnAutoYouTurn = new System.Windows.Forms.Button();
            this.btnSectionMasterAuto = new System.Windows.Forms.Button();
            this.btnSectionMasterManual = new System.Windows.Forms.Button();
            this.btnCycleLinesBk = new System.Windows.Forms.Button();
            this.btnCycleLines = new System.Windows.Forms.Button();
            this.btnContour = new System.Windows.Forms.Button();
            this.btnContourLock = new System.Windows.Forms.Button();
            this.lblNumCu = new System.Windows.Forms.Label();
            this.panelControlBox = new System.Windows.Forms.Panel();
            this.btnChargeStatus = new System.Windows.Forms.Button();
            this.btnGPSData = new System.Windows.Forms.Button();
            this.btnShutdown = new System.Windows.Forms.Button();
            this.btnMaximizeMainForm = new System.Windows.Forms.Button();
            this.btnMinimizeMainForm = new System.Windows.Forms.Button();
            this.btnFieldStats = new System.Windows.Forms.Button();
            this.lblHardwareMessage = new System.Windows.Forms.Label();
            this.tlpNozzle = new System.Windows.Forms.TableLayoutPanel();
            this.btnSprayPSI = new System.Windows.Forms.Button();
            this.lblPressure = new System.Windows.Forms.Label();
            this.btnSprayVolumeTotal = new System.Windows.Forms.Button();
            this.lbl_Volume = new System.Windows.Forms.Label();
            this.btnSprayGalPerAcre = new System.Windows.Forms.Button();
            this.cboxSprayAutoManual = new System.Windows.Forms.CheckBox();
            this.cboxRate1Rate2Select = new System.Windows.Forms.CheckBox();
            this.btnSprayRateUp = new System.Windows.Forms.Button();
            this.btnSprayRateDn = new System.Windows.Forms.Button();
            this.btnNozConfig = new System.Windows.Forms.Button();
            this.lblIsFlowing = new System.Windows.Forms.Label();
            this.lblGPM_Set = new System.Windows.Forms.Label();
            this.btnSprayGalPerMinActual = new System.Windows.Forms.Button();
            this.lblPWM_Nozz = new System.Windows.Forms.Label();
            this.lblFlowHz_Nozz = new System.Windows.Forms.Label();
            this.layersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStripFlag.SuspendLayout();
            this.statusStripLeft.SuspendLayout();
            this.panelSim.SuspendLayout();
            this.panelNavigation.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.flp1.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelControlBox.SuspendLayout();
            this.tlpNozzle.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 22F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Size = new System.Drawing.Size(73, 44);
            this.menuStrip1.TabIndex = 49;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator9,
            this.menustripLanguage,
            this.toolStripSeparator11,
            this.simulatorOnToolStripMenuItem,
            this.enterSimCoordsToolStripMenuItem,
            this.toolStripSeparator4,
            this.kioskModeToolStrip,
            this.resetALLToolStripMenuItem,
            this.nozzleAppToolStripMenuItem,
            this.agShareToolStripMenuItem});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileToolStripMenuItem.Image = global::AOG.Properties.Resources.fileMenu;
            this.fileToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.fileToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(55, 44);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(379, 6);
            // 
            // menustripLanguage
            // 
            this.menustripLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLanguageDanish,
            this.menuLanguageDeutsch,
            this.menuLanguageEnglish,
            this.menuLanguageSpanish,
            this.menuLanguageFrench,
            this.menuLanguageItalian,
            this.menuLanguageLatvian,
            this.menuLanguageLithuanian,
            this.menuLanguageHungarian,
            this.menuLanguageDutch,
            this.menuLanguagePolish,
            this.menuLanguagePortugese,
            this.menuLanguageRussian,
            this.menuLanguageSerbian,
            this.menuLanguageFinnish,
            this.menuLanguageSlovak,
            this.menuLanguageUkranian,
            this.menuLanguageTurkish,
            this.menuLanguageChinese,
            this.menuLanguageTest,
            this.menuGenerateLanguageReference});
            this.menustripLanguage.Name = "menustripLanguage";
            this.menustripLanguage.Size = new System.Drawing.Size(382, 50);
            this.menustripLanguage.Text = "Language";
            // 
            // menuLanguageDanish
            // 
            this.menuLanguageDanish.Name = "menuLanguageDanish";
            this.menuLanguageDanish.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageDanish.Text = "Dansk (Denmark)";
            this.menuLanguageDanish.Click += new System.EventHandler(this.menuLanguageDanish_Click);
            // 
            // menuLanguageDeutsch
            // 
            this.menuLanguageDeutsch.CheckOnClick = true;
            this.menuLanguageDeutsch.Name = "menuLanguageDeutsch";
            this.menuLanguageDeutsch.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageDeutsch.Text = "Deutsch (Germany)";
            this.menuLanguageDeutsch.Click += new System.EventHandler(this.menuLanguageDeutsch_Click);
            // 
            // menuLanguageEnglish
            // 
            this.menuLanguageEnglish.CheckOnClick = true;
            this.menuLanguageEnglish.Name = "menuLanguageEnglish";
            this.menuLanguageEnglish.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageEnglish.Text = "English (Canada)";
            this.menuLanguageEnglish.Click += new System.EventHandler(this.menuLanguageEnglish_Click);
            // 
            // menuLanguageSpanish
            // 
            this.menuLanguageSpanish.CheckOnClick = true;
            this.menuLanguageSpanish.Name = "menuLanguageSpanish";
            this.menuLanguageSpanish.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageSpanish.Text = "Español (Spanish)";
            this.menuLanguageSpanish.Click += new System.EventHandler(this.menuLanguageSpanish_Click);
            // 
            // menuLanguageFrench
            // 
            this.menuLanguageFrench.CheckOnClick = true;
            this.menuLanguageFrench.Name = "menuLanguageFrench";
            this.menuLanguageFrench.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageFrench.Text = "Français (France)";
            this.menuLanguageFrench.Click += new System.EventHandler(this.menuLanguageFrench_Click);
            // 
            // menuLanguageItalian
            // 
            this.menuLanguageItalian.Name = "menuLanguageItalian";
            this.menuLanguageItalian.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageItalian.Text = "Italiano (Italy)";
            this.menuLanguageItalian.Click += new System.EventHandler(this.menuLanguageItalian_Click);
            // 
            // menuLanguageLatvian
            // 
            this.menuLanguageLatvian.Name = "menuLanguageLatvian";
            this.menuLanguageLatvian.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageLatvian.Text = "Latviski (Latvia)";
            this.menuLanguageLatvian.Click += new System.EventHandler(this.menuLanguageLatvian_Click);
            // 
            // menuLanguageLithuanian
            // 
            this.menuLanguageLithuanian.Name = "menuLanguageLithuanian";
            this.menuLanguageLithuanian.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageLithuanian.Text = "Lietuvių (Lithuania)";
            this.menuLanguageLithuanian.Click += new System.EventHandler(this.menuLanguageLithuanian_Click);
            // 
            // menuLanguageHungarian
            // 
            this.menuLanguageHungarian.Name = "menuLanguageHungarian";
            this.menuLanguageHungarian.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageHungarian.Text = "Magyar (Hungary)";
            this.menuLanguageHungarian.Click += new System.EventHandler(this.menuLanguageHungarian_Click);
            // 
            // menuLanguageDutch
            // 
            this.menuLanguageDutch.CheckOnClick = true;
            this.menuLanguageDutch.Name = "menuLanguageDutch";
            this.menuLanguageDutch.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageDutch.Text = "Nederlands (Holland)";
            this.menuLanguageDutch.Click += new System.EventHandler(this.menuLanguageDutch_Click);
            // 
            // menuLanguagePolish
            // 
            this.menuLanguagePolish.Name = "menuLanguagePolish";
            this.menuLanguagePolish.Size = new System.Drawing.Size(487, 50);
            this.menuLanguagePolish.Text = "Polski (Poland)";
            this.menuLanguagePolish.Click += new System.EventHandler(this.menuLanguagesPolski_Click);
            // 
            // menuLanguagePortugese
            // 
            this.menuLanguagePortugese.Name = "menuLanguagePortugese";
            this.menuLanguagePortugese.Size = new System.Drawing.Size(487, 50);
            this.menuLanguagePortugese.Text = "Português (Portuguese)";
            this.menuLanguagePortugese.Click += new System.EventHandler(this.menuLanguagesPortugese_Click);
            // 
            // menuLanguageRussian
            // 
            this.menuLanguageRussian.CheckOnClick = true;
            this.menuLanguageRussian.Name = "menuLanguageRussian";
            this.menuLanguageRussian.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageRussian.Text = "русский (Russia)";
            this.menuLanguageRussian.Click += new System.EventHandler(this.menuLanguageRussian_Click);
            // 
            // menuLanguageSerbian
            // 
            this.menuLanguageSerbian.Name = "menuLanguageSerbian";
            this.menuLanguageSerbian.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageSerbian.Text = "српски (Serbian)";
            this.menuLanguageSerbian.Click += new System.EventHandler(this.menuLanguageSerbian_Click);
            // 
            // menuLanguageFinnish
            // 
            this.menuLanguageFinnish.Name = "menuLanguageFinnish";
            this.menuLanguageFinnish.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageFinnish.Text = "Suomalainen (Finland)";
            this.menuLanguageFinnish.Click += new System.EventHandler(this.menuLanguageFinnish_Click);
            // 
            // menuLanguageSlovak
            // 
            this.menuLanguageSlovak.Name = "menuLanguageSlovak";
            this.menuLanguageSlovak.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageSlovak.Text = "Slovenčina (Slovakia)";
            this.menuLanguageSlovak.Click += new System.EventHandler(this.menuLanguageSlovak_Click);
            // 
            // menuLanguageUkranian
            // 
            this.menuLanguageUkranian.Name = "menuLanguageUkranian";
            this.menuLanguageUkranian.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageUkranian.Text = "Yкраїнська (Ukraine)";
            this.menuLanguageUkranian.Click += new System.EventHandler(this.menuLanguageUkranian_Click);
            // 
            // menuLanguageTurkish
            // 
            this.menuLanguageTurkish.Name = "menuLanguageTurkish";
            this.menuLanguageTurkish.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageTurkish.Text = "Türkçe (Turkey)";
            this.menuLanguageTurkish.Click += new System.EventHandler(this.menuLanguageTurkish_Click);
            // 
            // menuLanguageChinese
            // 
            this.menuLanguageChinese.Name = "menuLanguageChinese";
            this.menuLanguageChinese.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageChinese.Text = "中国人 (Chinese)";
            this.menuLanguageChinese.Click += new System.EventHandler(this.menuLanguageChinese_Click);
            // 
            // menuLanguageTest
            // 
            this.menuLanguageTest.Name = "menuLanguageTest";
            this.menuLanguageTest.Size = new System.Drawing.Size(487, 50);
            this.menuLanguageTest.Text = "Test";
            this.menuLanguageTest.Click += new System.EventHandler(this.menuLanguageTest_Click);
            // 
            // menuGenerateLanguageReference
            // 
            this.menuGenerateLanguageReference.Name = "menuGenerateLanguageReference";
            this.menuGenerateLanguageReference.Size = new System.Drawing.Size(487, 50);
            this.menuGenerateLanguageReference.Text = "Generate Reference File";
            this.menuGenerateLanguageReference.Click += new System.EventHandler(this.menuGenerateLanguageReference_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(379, 6);
            // 
            // simulatorOnToolStripMenuItem
            // 
            this.simulatorOnToolStripMenuItem.CheckOnClick = true;
            this.simulatorOnToolStripMenuItem.Name = "simulatorOnToolStripMenuItem";
            this.simulatorOnToolStripMenuItem.Size = new System.Drawing.Size(382, 50);
            this.simulatorOnToolStripMenuItem.Text = "Simulator On";
            this.simulatorOnToolStripMenuItem.Click += new System.EventHandler(this.simulatorOnToolStripMenuItem_Click);
            // 
            // enterSimCoordsToolStripMenuItem
            // 
            this.enterSimCoordsToolStripMenuItem.Name = "enterSimCoordsToolStripMenuItem";
            this.enterSimCoordsToolStripMenuItem.Size = new System.Drawing.Size(382, 50);
            this.enterSimCoordsToolStripMenuItem.Text = "Enter Sim Coords";
            this.enterSimCoordsToolStripMenuItem.Click += new System.EventHandler(this.enterSimCoordsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(379, 6);
            // 
            // kioskModeToolStrip
            // 
            this.kioskModeToolStrip.Name = "kioskModeToolStrip";
            this.kioskModeToolStrip.Size = new System.Drawing.Size(382, 50);
            this.kioskModeToolStrip.Text = "Kiosk Mode";
            this.kioskModeToolStrip.Click += new System.EventHandler(this.kioskModeToolStriPGN_Click);
            // 
            // resetALLToolStripMenuItem
            // 
            this.resetALLToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetEverythingToolStripMenuItem});
            this.resetALLToolStripMenuItem.Name = "resetALLToolStripMenuItem";
            this.resetALLToolStripMenuItem.Size = new System.Drawing.Size(382, 50);
            this.resetALLToolStripMenuItem.Text = "Reset All";
            // 
            // resetEverythingToolStripMenuItem
            // 
            this.resetEverythingToolStripMenuItem.Name = "resetEverythingToolStripMenuItem";
            this.resetEverythingToolStripMenuItem.Size = new System.Drawing.Size(372, 50);
            this.resetEverythingToolStripMenuItem.Text = "Reset To Default";
            this.resetEverythingToolStripMenuItem.Click += new System.EventHandler(this.resetALLToolStripMenuItem_Click);
            // 
            // nozzleAppToolStripMenuItem
            // 
            this.nozzleAppToolStripMenuItem.Name = "nozzleAppToolStripMenuItem";
            this.nozzleAppToolStripMenuItem.Size = new System.Drawing.Size(382, 50);
            this.nozzleAppToolStripMenuItem.Text = "Nozzle App";
            this.nozzleAppToolStripMenuItem.Click += new System.EventHandler(this.nozzleAppToolStripMenuItem_Click);
            // 
            // agShareToolStripMenuItem
            // 
            this.agShareToolStripMenuItem.Name = "agShareToolStripMenuItem";
            this.agShareToolStripMenuItem.Size = new System.Drawing.Size(382, 50);
            this.agShareToolStripMenuItem.Text = "AgShare Setup";
            this.agShareToolStripMenuItem.Click += new System.EventHandler(this.agShareToolStripMenuItem_Click);
            // 
            // tmrWatchdog
            // 
            this.tmrWatchdog.Interval = 250;
            this.tmrWatchdog.Tick += new System.EventHandler(this.tmrWatchdog_tick);
            // 
            // contextMenuStripFlag
            // 
            this.contextMenuStripFlag.AutoSize = false;
            this.contextMenuStripFlag.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.contextMenuStripFlag.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFlagRed,
            this.toolStripMenuFlagGrn,
            this.toolStripMenuFlagYel,
            this.toolStripSeparator12,
            this.toolStripMenuFlagForm});
            this.contextMenuStripFlag.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.contextMenuStripFlag.Name = "contextMenuStripFlag";
            this.contextMenuStripFlag.Size = new System.Drawing.Size(72, 312);
            // 
            // toolStripMenuItemFlagRed
            // 
            this.toolStripMenuItemFlagRed.AutoSize = false;
            this.toolStripMenuItemFlagRed.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripMenuItemFlagRed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMenuItemFlagRed.Image = global::AOG.Properties.Resources.FlagYel;
            this.toolStripMenuItemFlagRed.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemFlagRed.Name = "toolStripMenuItemFlagRed";
            this.toolStripMenuItemFlagRed.Size = new System.Drawing.Size(70, 70);
            this.toolStripMenuItemFlagRed.Text = ".";
            this.toolStripMenuItemFlagRed.Click += new System.EventHandler(this.toolStripMenuYel_Click);
            // 
            // toolStripMenuFlagGrn
            // 
            this.toolStripMenuFlagGrn.AutoSize = false;
            this.toolStripMenuFlagGrn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripMenuFlagGrn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMenuFlagGrn.Image = global::AOG.Properties.Resources.FlagGrn;
            this.toolStripMenuFlagGrn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuFlagGrn.Name = "toolStripMenuFlagGrn";
            this.toolStripMenuFlagGrn.Size = new System.Drawing.Size(70, 70);
            this.toolStripMenuFlagGrn.Text = ".";
            this.toolStripMenuFlagGrn.Click += new System.EventHandler(this.toolStripMenuGrn_Click);
            // 
            // toolStripMenuFlagYel
            // 
            this.toolStripMenuFlagYel.AutoSize = false;
            this.toolStripMenuFlagYel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripMenuFlagYel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMenuFlagYel.Image = global::AOG.Properties.Resources.FlagRed;
            this.toolStripMenuFlagYel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuFlagYel.Name = "toolStripMenuFlagYel";
            this.toolStripMenuFlagYel.Size = new System.Drawing.Size(70, 70);
            this.toolStripMenuFlagYel.Text = ".";
            this.toolStripMenuFlagYel.Click += new System.EventHandler(this.toolStripMenuItemFlagRed_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(68, 6);
            // 
            // toolStripMenuFlagForm
            // 
            this.toolStripMenuFlagForm.Image = global::AOG.Properties.Resources.FileEditName;
            this.toolStripMenuFlagForm.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuFlagForm.Name = "toolStripMenuFlagForm";
            this.toolStripMenuFlagForm.Size = new System.Drawing.Size(267, 70);
            this.toolStripMenuFlagForm.Text = "toolStripMenuItem3";
            this.toolStripMenuFlagForm.Click += new System.EventHandler(this.toolStripMenuFlagForm_Click);
            // 
            // cboxpRowWidth
            // 
            this.cboxpRowWidth.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxpRowWidth.BackColor = System.Drawing.Color.Lavender;
            this.cboxpRowWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxpRowWidth.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxpRowWidth.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxpRowWidth.FormattingEnabled = true;
            this.cboxpRowWidth.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.cboxpRowWidth.Location = new System.Drawing.Point(2, 7);
            this.cboxpRowWidth.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxpRowWidth.Name = "cboxpRowWidth";
            this.cboxpRowWidth.Size = new System.Drawing.Size(52, 47);
            this.cboxpRowWidth.TabIndex = 247;
            this.cboxpRowWidth.SelectedIndexChanged += new System.EventHandler(this.cboxpRowWidth_SelectedIndexChanged);
            // 
            // oglZoom
            // 
            this.oglZoom.BackColor = System.Drawing.Color.Black;
            this.oglZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.oglZoom.Location = new System.Drawing.Point(122, 63);
            this.oglZoom.Margin = new System.Windows.Forms.Padding(0);
            this.oglZoom.Name = "oglZoom";
            this.oglZoom.Size = new System.Drawing.Size(41, 44);
            this.oglZoom.TabIndex = 182;
            this.oglZoom.VSync = false;
            this.oglZoom.Load += new System.EventHandler(this.oglZoom_Load);
            this.oglZoom.Paint += new System.Windows.Forms.PaintEventHandler(this.oglZoom_Paint);
            // 
            // btnResetSim
            // 
            this.btnResetSim.BackColor = System.Drawing.Color.Transparent;
            this.btnResetSim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnResetSim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetSim.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnResetSim.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetSim.Location = new System.Drawing.Point(4, 4);
            this.btnResetSim.Name = "btnResetSim";
            this.btnResetSim.Size = new System.Drawing.Size(59, 37);
            this.btnResetSim.TabIndex = 164;
            this.btnResetSim.Text = "Reset";
            this.btnResetSim.UseVisualStyleBackColor = false;
            this.btnResetSim.Click += new System.EventHandler(this.btnResetSim_Click);
            // 
            // btnResetSteerAngle
            // 
            this.btnResetSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.btnResetSteerAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnResetSteerAngle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetSteerAngle.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnResetSteerAngle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetSteerAngle.Location = new System.Drawing.Point(70, 4);
            this.btnResetSteerAngle.Name = "btnResetSteerAngle";
            this.btnResetSteerAngle.Size = new System.Drawing.Size(59, 37);
            this.btnResetSteerAngle.TabIndex = 162;
            this.btnResetSteerAngle.Text = ">0<";
            this.btnResetSteerAngle.UseVisualStyleBackColor = false;
            this.btnResetSteerAngle.Click += new System.EventHandler(this.btnResetSteerAngle_Click);
            // 
            // timerSim
            // 
            this.timerSim.Interval = 43;
            this.timerSim.Tick += new System.EventHandler(this.timerSim_Tick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(334, 62);
            this.toolStripMenuItem2.Text = "toolStripMenuItem2";
            // 
            // hsbarSteerAngle
            // 
            this.hsbarSteerAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hsbarSteerAngle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.hsbarSteerAngle.LargeChange = 20;
            this.hsbarSteerAngle.Location = new System.Drawing.Point(158, 1);
            this.hsbarSteerAngle.Maximum = 819;
            this.hsbarSteerAngle.Name = "hsbarSteerAngle";
            this.hsbarSteerAngle.Size = new System.Drawing.Size(167, 43);
            this.hsbarSteerAngle.TabIndex = 179;
            this.hsbarSteerAngle.Value = 400;
            this.hsbarSteerAngle.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarSteerAngle_Scroll);
            // 
            // oglMain
            // 
            this.oglMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.oglMain.BackColor = System.Drawing.Color.Black;
            this.oglMain.Location = new System.Drawing.Point(250, 50);
            this.oglMain.Margin = new System.Windows.Forms.Padding(0);
            this.oglMain.Name = "oglMain";
            this.oglMain.Size = new System.Drawing.Size(681, 632);
            this.oglMain.TabIndex = 180;
            this.oglMain.VSync = false;
            this.oglMain.Load += new System.EventHandler(this.oglMain_Load);
            this.oglMain.Paint += new System.Windows.Forms.PaintEventHandler(this.oglMain_Paint);
            this.oglMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.oglMain_MouseDown);
            this.oglMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OglMain_MouseMove);
            this.oglMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.oglMain_MouseUp);
            this.oglMain.Resize += new System.EventHandler(this.oglMain_Resize);
            // 
            // oglBack
            // 
            this.oglBack.BackColor = System.Drawing.Color.Black;
            this.oglBack.ForeColor = System.Drawing.Color.Transparent;
            this.oglBack.Location = new System.Drawing.Point(122, 70);
            this.oglBack.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.oglBack.Name = "oglBack";
            this.oglBack.Size = new System.Drawing.Size(500, 300);
            this.oglBack.TabIndex = 181;
            this.oglBack.VSync = false;
            this.oglBack.Load += new System.EventHandler(this.oglBack_Load);
            this.oglBack.Paint += new System.Windows.Forms.PaintEventHandler(this.oglBack_Paint);
            // 
            // lblHz
            // 
            this.lblHz.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblHz.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHz.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblHz.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHz.Location = new System.Drawing.Point(92, 276);
            this.lblHz.Name = "lblHz";
            this.lblHz.Size = new System.Drawing.Size(84, 79);
            this.lblHz.TabIndex = 249;
            this.lblHz.Text = "5 Hz 32\r\nPPS";
            this.lblHz.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHz.Click += new System.EventHandler(this.lblHz_Click);
            // 
            // statusStripLeft
            // 
            this.statusStripLeft.AllowMerge = false;
            this.statusStripLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.statusStripLeft.AutoSize = false;
            this.statusStripLeft.BackColor = System.Drawing.Color.Transparent;
            this.statusStripLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.statusStripLeft.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStripLeft.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStripLeft.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStripLeft.ImageScalingSize = new System.Drawing.Size(60, 60);
            this.statusStripLeft.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.distanceToolBtn,
            this.toolStripDropDownButton4});
            this.statusStripLeft.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStripLeft.Location = new System.Drawing.Point(1, 73);
            this.statusStripLeft.Name = "statusStripLeft";
            this.statusStripLeft.Size = new System.Drawing.Size(67, 109);
            this.statusStripLeft.SizingGrip = false;
            this.statusStripLeft.TabIndex = 324;
            this.statusStripLeft.Text = "UDP";
            // 
            // distanceToolBtn
            // 
            this.distanceToolBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.distanceToolBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.distanceToolBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.distanceToolBtn.Name = "distanceToolBtn";
            this.distanceToolBtn.ShowDropDownArrow = false;
            this.distanceToolBtn.Size = new System.Drawing.Size(54, 20);
            this.distanceToolBtn.Text = "2345m";
            this.distanceToolBtn.Click += new System.EventHandler(this.toolStripDropDownButtonDistance_Click);
            // 
            // toolStripDropDownButton4
            // 
            this.toolStripDropDownButton4.AutoSize = false;
            this.toolStripDropDownButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wizardsMenu,
            this.steerChartStripMenu,
            this.boundaryToolToolStripMenu,
            this.layersToolStripMenuItem,
            this.eventViewerToolStripMenuItem,
            this.guidelinesToolStripMenuItem,
            this.SmoothABtoolStripMenu,
            this.deleteContourPathsToolStripMenuItem,
            this.webcamToolStrip,
            this.offsetFixToolStrip});
            this.toolStripDropDownButton4.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripDropDownButton4.Image = global::AOG.Properties.Resources.SpecialFunctions;
            this.toolStripDropDownButton4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripDropDownButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton4.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            this.toolStripDropDownButton4.ShowDropDownArrow = false;
            this.toolStripDropDownButton4.Size = new System.Drawing.Size(66, 80);
            this.toolStripDropDownButton4.Text = "toolStripDropDownButton3";
            // 
            // wizardsMenu
            // 
            this.wizardsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.steerWizardMenuItem});
            this.wizardsMenu.Image = global::AOG.Properties.Resources.WizardWand;
            this.wizardsMenu.Name = "wizardsMenu";
            this.wizardsMenu.Size = new System.Drawing.Size(450, 70);
            this.wizardsMenu.Text = "Wizards";
            // 
            // steerWizardMenuItem
            // 
            this.steerWizardMenuItem.Image = global::AOG.Properties.Resources.AutoSteerOn;
            this.steerWizardMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.steerWizardMenuItem.Name = "steerWizardMenuItem";
            this.steerWizardMenuItem.Size = new System.Drawing.Size(301, 44);
            this.steerWizardMenuItem.Text = "Steer Wizard";
            this.steerWizardMenuItem.Click += new System.EventHandler(this.steerWizardMenuItem_Click);
            // 
            // steerChartStripMenu
            // 
            this.steerChartStripMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.steerChartToolStripMenuItem,
            this.headingChartToolStripMenuItem,
            this.xTEChartToolStripMenuItem,
            this.rollCheckToolStripMenuItem});
            this.steerChartStripMenu.Image = global::AOG.Properties.Resources.Chart;
            this.steerChartStripMenu.Name = "steerChartStripMenu";
            this.steerChartStripMenu.Size = new System.Drawing.Size(450, 70);
            this.steerChartStripMenu.Text = "Charts";
            // 
            // steerChartToolStripMenuItem
            // 
            this.steerChartToolStripMenuItem.Image = global::AOG.Properties.Resources.AutoSteerOn;
            this.steerChartToolStripMenuItem.Name = "steerChartToolStripMenuItem";
            this.steerChartToolStripMenuItem.Size = new System.Drawing.Size(322, 44);
            this.steerChartToolStripMenuItem.Text = "Steer Chart";
            this.steerChartToolStripMenuItem.Click += new System.EventHandler(this.toolStripAutoSteerChart_Click);
            // 
            // headingChartToolStripMenuItem
            // 
            this.headingChartToolStripMenuItem.Image = global::AOG.Properties.Resources.ConS_SourcesHeading;
            this.headingChartToolStripMenuItem.Name = "headingChartToolStripMenuItem";
            this.headingChartToolStripMenuItem.Size = new System.Drawing.Size(322, 44);
            this.headingChartToolStripMenuItem.Text = "Heading Chart";
            this.headingChartToolStripMenuItem.Click += new System.EventHandler(this.headingChartToolStripMenuItem_Click);
            // 
            // xTEChartToolStripMenuItem
            // 
            this.xTEChartToolStripMenuItem.Image = global::AOG.Properties.Resources.AutoManualIsAuto;
            this.xTEChartToolStripMenuItem.Name = "xTEChartToolStripMenuItem";
            this.xTEChartToolStripMenuItem.Size = new System.Drawing.Size(322, 44);
            this.xTEChartToolStripMenuItem.Text = "XTE Chart";
            this.xTEChartToolStripMenuItem.Click += new System.EventHandler(this.xTEChartToolStripMenuItem_Click);
            // 
            // rollCheckToolStripMenuItem
            // 
            this.rollCheckToolStripMenuItem.Name = "rollCheckToolStripMenuItem";
            this.rollCheckToolStripMenuItem.Size = new System.Drawing.Size(322, 44);
            this.rollCheckToolStripMenuItem.Text = "Roll Check";
            this.rollCheckToolStripMenuItem.Click += new System.EventHandler(this.correctionToolStriPGN_Click);
            // 
            // boundaryToolToolStripMenu
            // 
            this.boundaryToolToolStripMenu.Image = global::AOG.Properties.Resources.Boundary;
            this.boundaryToolToolStripMenu.Name = "boundaryToolToolStripMenu";
            this.boundaryToolToolStripMenu.Size = new System.Drawing.Size(450, 70);
            this.boundaryToolToolStripMenu.Text = "Boundary Tool";
            this.boundaryToolToolStripMenu.Click += new System.EventHandler(this.boundaryToolToolStripMenu_Click);
            // 
            // eventViewerToolStripMenuItem
            // 
            this.eventViewerToolStripMenuItem.Image = global::AOG.Properties.Resources.ABTracks;
            this.eventViewerToolStripMenuItem.Name = "eventViewerToolStripMenuItem";
            this.eventViewerToolStripMenuItem.Size = new System.Drawing.Size(450, 70);
            this.eventViewerToolStripMenuItem.Text = "Log Viewer";
            this.eventViewerToolStripMenuItem.Click += new System.EventHandler(this.eventViewerToolStripMenuItem_Click);
            // 
            // guidelinesToolStripMenuItem
            // 
            this.guidelinesToolStripMenuItem.Image = global::AOG.Properties.Resources.ConD_ExtraGuides;
            this.guidelinesToolStripMenuItem.Name = "guidelinesToolStripMenuItem";
            this.guidelinesToolStripMenuItem.Size = new System.Drawing.Size(450, 70);
            this.guidelinesToolStripMenuItem.Text = "Extra Guides";
            this.guidelinesToolStripMenuItem.Click += new System.EventHandler(this.guidelinesToolStripMenuItem_Click);
            // 
            // SmoothABtoolStripMenu
            // 
            this.SmoothABtoolStripMenu.Image = global::AOG.Properties.Resources.ABSmooth;
            this.SmoothABtoolStripMenu.Name = "SmoothABtoolStripMenu";
            this.SmoothABtoolStripMenu.Size = new System.Drawing.Size(450, 70);
            this.SmoothABtoolStripMenu.Text = "Smooth AB Curve";
            this.SmoothABtoolStripMenu.Click += new System.EventHandler(this.SmoothABtoolStripMenu_Click);
            // 
            // deleteContourPathsToolStripMenuItem
            // 
            this.deleteContourPathsToolStripMenuItem.Image = global::AOG.Properties.Resources.TrashContourRef;
            this.deleteContourPathsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteContourPathsToolStripMenuItem.Name = "deleteContourPathsToolStripMenuItem";
            this.deleteContourPathsToolStripMenuItem.Size = new System.Drawing.Size(450, 70);
            this.deleteContourPathsToolStripMenuItem.Text = "Hide Contour Paths";
            this.deleteContourPathsToolStripMenuItem.Click += new System.EventHandler(this.deleteContourPathsToolStripMenuItem_Click);
            // 
            // webcamToolStrip
            // 
            this.webcamToolStrip.Image = global::AOG.Properties.Resources.Webcam;
            this.webcamToolStrip.Name = "webcamToolStrip";
            this.webcamToolStrip.Size = new System.Drawing.Size(450, 70);
            this.webcamToolStrip.Text = "WebCam";
            this.webcamToolStrip.Click += new System.EventHandler(this.webcamToolStriPGN_Click);
            // 
            // offsetFixToolStrip
            // 
            this.offsetFixToolStrip.Image = global::AOG.Properties.Resources.YouTurnReverse;
            this.offsetFixToolStrip.Name = "offsetFixToolStrip";
            this.offsetFixToolStrip.Size = new System.Drawing.Size(450, 70);
            this.offsetFixToolStrip.Text = "Offset Fix";
            this.offsetFixToolStrip.Click += new System.EventHandler(this.offsetFixToolStriPGN_Click);
            // 
            // panelSim
            // 
            this.panelSim.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panelSim.BackColor = System.Drawing.Color.Transparent;
            this.panelSim.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.panelSim.ColumnCount = 9;
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 167F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.panelSim.Controls.Add(this.btnSpeedDn, 5, 0);
            this.panelSim.Controls.Add(this.btnSimSpeedUp, 7, 0);
            this.panelSim.Controls.Add(this.btnResetSim, 0, 0);
            this.panelSim.Controls.Add(this.btnSimSetSpeedToZero, 6, 0);
            this.panelSim.Controls.Add(this.btnResetSteerAngle, 1, 0);
            this.panelSim.Controls.Add(this.hsbarSteerAngle, 3, 0);
            this.panelSim.Controls.Add(this.btnSimReverseDirection, 8, 0);
            this.panelSim.Location = new System.Drawing.Point(299, 625);
            this.panelSim.Name = "panelSim";
            this.panelSim.RowCount = 1;
            this.panelSim.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelSim.Size = new System.Drawing.Size(615, 45);
            this.panelSim.TabIndex = 325;
            // 
            // btnSpeedDn
            // 
            this.btnSpeedDn.BackColor = System.Drawing.Color.Transparent;
            this.btnSpeedDn.BackgroundImage = global::AOG.Properties.Resources.DnArrow64;
            this.btnSpeedDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSpeedDn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSpeedDn.FlatAppearance.BorderSize = 0;
            this.btnSpeedDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSpeedDn.Location = new System.Drawing.Point(342, 4);
            this.btnSpeedDn.Name = "btnSpeedDn";
            this.btnSpeedDn.Size = new System.Drawing.Size(7, 37);
            this.btnSpeedDn.TabIndex = 533;
            this.btnSpeedDn.UseVisualStyleBackColor = false;
            this.btnSpeedDn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSpeedDn_MouseDown);
            // 
            // btnSimSpeedUp
            // 
            this.btnSimSpeedUp.BackColor = System.Drawing.Color.Transparent;
            this.btnSimSpeedUp.BackgroundImage = global::AOG.Properties.Resources.UpArrow64;
            this.btnSimSpeedUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSimSpeedUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSimSpeedUp.FlatAppearance.BorderSize = 0;
            this.btnSimSpeedUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimSpeedUp.Location = new System.Drawing.Point(432, 4);
            this.btnSimSpeedUp.Name = "btnSimSpeedUp";
            this.btnSimSpeedUp.Size = new System.Drawing.Size(7, 37);
            this.btnSimSpeedUp.TabIndex = 532;
            this.btnSimSpeedUp.UseVisualStyleBackColor = false;
            this.btnSimSpeedUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSimSpeedUPGN_MouseDown);
            // 
            // btnSimSetSpeedToZero
            // 
            this.btnSimSetSpeedToZero.BackColor = System.Drawing.Color.Transparent;
            this.btnSimSetSpeedToZero.BackgroundImage = global::AOG.Properties.Resources.AutoStop;
            this.btnSimSetSpeedToZero.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSimSetSpeedToZero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimSetSpeedToZero.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnSimSetSpeedToZero.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSimSetSpeedToZero.Location = new System.Drawing.Point(356, 4);
            this.btnSimSetSpeedToZero.Name = "btnSimSetSpeedToZero";
            this.btnSimSetSpeedToZero.Size = new System.Drawing.Size(69, 34);
            this.btnSimSetSpeedToZero.TabIndex = 453;
            this.btnSimSetSpeedToZero.UseVisualStyleBackColor = false;
            this.btnSimSetSpeedToZero.Click += new System.EventHandler(this.btnSimSetSpeedToZero_Click);
            // 
            // btnSimReverseDirection
            // 
            this.btnSimReverseDirection.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSimReverseDirection.BackColor = System.Drawing.Color.Transparent;
            this.btnSimReverseDirection.BackgroundImage = global::AOG.Properties.Resources.Youturn80;
            this.btnSimReverseDirection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSimReverseDirection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimReverseDirection.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnSimReverseDirection.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSimReverseDirection.Location = new System.Drawing.Point(511, 5);
            this.btnSimReverseDirection.Name = "btnSimReverseDirection";
            this.btnSimReverseDirection.Size = new System.Drawing.Size(35, 34);
            this.btnSimReverseDirection.TabIndex = 537;
            this.btnSimReverseDirection.UseVisualStyleBackColor = false;
            this.btnSimReverseDirection.Click += new System.EventHandler(this.btnSimReverseDirection_Click);
            // 
            // panelNavigation
            // 
            this.panelNavigation.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelNavigation.ColumnCount = 2;
            this.panelNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelNavigation.Controls.Add(this.btnTiltDn, 0, 0);
            this.panelNavigation.Controls.Add(this.btnTiltUp, 1, 0);
            this.panelNavigation.Controls.Add(this.btnBrightnessDn, 0, 4);
            this.panelNavigation.Controls.Add(this.btnBrightnessUp, 1, 4);
            this.panelNavigation.Controls.Add(this.btnDayNightMode, 0, 3);
            this.panelNavigation.Controls.Add(this.lblHz, 1, 3);
            this.panelNavigation.Controls.Add(this.btn3D, 1, 1);
            this.panelNavigation.Controls.Add(this.btn2D, 0, 1);
            this.panelNavigation.Controls.Add(this.btnGrid, 1, 2);
            this.panelNavigation.Controls.Add(this.btnN2D, 0, 2);
            this.panelNavigation.Location = new System.Drawing.Point(505, 63);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.RowCount = 5;
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.Size = new System.Drawing.Size(179, 460);
            this.panelNavigation.TabIndex = 468;
            this.panelNavigation.Visible = false;
            // 
            // btnTiltDn
            // 
            this.btnTiltDn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTiltDn.BackColor = System.Drawing.Color.Transparent;
            this.btnTiltDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTiltDn.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnTiltDn.FlatAppearance.BorderSize = 0;
            this.btnTiltDn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTiltDn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTiltDn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTiltDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTiltDn.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTiltDn.Image = global::AOG.Properties.Resources.TiltDown;
            this.btnTiltDn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTiltDn.Location = new System.Drawing.Point(16, 18);
            this.btnTiltDn.Name = "btnTiltDn";
            this.btnTiltDn.Size = new System.Drawing.Size(57, 55);
            this.btnTiltDn.TabIndex = 543;
            this.btnTiltDn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTiltDn.UseVisualStyleBackColor = false;
            this.btnTiltDn.Click += new System.EventHandler(this.btnTiltDn_Click);
            // 
            // btnTiltUp
            // 
            this.btnTiltUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTiltUp.BackColor = System.Drawing.Color.Transparent;
            this.btnTiltUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTiltUp.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnTiltUp.FlatAppearance.BorderSize = 0;
            this.btnTiltUp.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTiltUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTiltUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTiltUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTiltUp.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTiltUp.Image = global::AOG.Properties.Resources.TiltUp;
            this.btnTiltUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTiltUp.Location = new System.Drawing.Point(105, 18);
            this.btnTiltUp.Name = "btnTiltUp";
            this.btnTiltUp.Size = new System.Drawing.Size(57, 55);
            this.btnTiltUp.TabIndex = 544;
            this.btnTiltUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTiltUp.UseVisualStyleBackColor = false;
            this.btnTiltUp.Click += new System.EventHandler(this.btnTiltUPGN_Click);
            // 
            // btnBrightnessDn
            // 
            this.btnBrightnessDn.BackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBrightnessDn.FlatAppearance.BorderSize = 0;
            this.btnBrightnessDn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessDn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessDn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrightnessDn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrightnessDn.Image = global::AOG.Properties.Resources.BrightnessDn;
            this.btnBrightnessDn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrightnessDn.Location = new System.Drawing.Point(3, 371);
            this.btnBrightnessDn.Name = "btnBrightnessDn";
            this.btnBrightnessDn.Size = new System.Drawing.Size(83, 75);
            this.btnBrightnessDn.TabIndex = 474;
            this.btnBrightnessDn.Text = "20%";
            this.btnBrightnessDn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBrightnessDn.UseVisualStyleBackColor = false;
            this.btnBrightnessDn.Click += new System.EventHandler(this.btnBrightnessDn_Click);
            // 
            // btnBrightnessUp
            // 
            this.btnBrightnessUp.BackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBrightnessUp.FlatAppearance.BorderSize = 0;
            this.btnBrightnessUp.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrightnessUp.Image = global::AOG.Properties.Resources.BrightnessUp;
            this.btnBrightnessUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrightnessUp.Location = new System.Drawing.Point(92, 371);
            this.btnBrightnessUp.Name = "btnBrightnessUp";
            this.btnBrightnessUp.Size = new System.Drawing.Size(84, 75);
            this.btnBrightnessUp.TabIndex = 473;
            this.btnBrightnessUp.UseVisualStyleBackColor = false;
            this.btnBrightnessUp.Click += new System.EventHandler(this.btnBrightnessUPGN_Click);
            // 
            // btnDayNightMode
            // 
            this.btnDayNightMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDayNightMode.BackColor = System.Drawing.Color.Transparent;
            this.btnDayNightMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDayNightMode.FlatAppearance.BorderSize = 0;
            this.btnDayNightMode.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnDayNightMode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDayNightMode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDayNightMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDayNightMode.Image = global::AOG.Properties.Resources.WindowNightMode;
            this.btnDayNightMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDayNightMode.Location = new System.Drawing.Point(16, 294);
            this.btnDayNightMode.Name = "btnDayNightMode";
            this.btnDayNightMode.Size = new System.Drawing.Size(57, 55);
            this.btnDayNightMode.TabIndex = 452;
            this.btnDayNightMode.UseVisualStyleBackColor = false;
            this.btnDayNightMode.Click += new System.EventHandler(this.btnDayNightMode_Click);
            // 
            // btn3D
            // 
            this.btn3D.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn3D.BackColor = System.Drawing.Color.Transparent;
            this.btn3D.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn3D.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btn3D.FlatAppearance.BorderSize = 0;
            this.btn3D.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btn3D.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn3D.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn3D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn3D.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3D.Image = global::AOG.Properties.Resources.Camera3D64;
            this.btn3D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn3D.Location = new System.Drawing.Point(105, 110);
            this.btn3D.Name = "btn3D";
            this.btn3D.Size = new System.Drawing.Size(57, 55);
            this.btn3D.TabIndex = 471;
            this.btn3D.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn3D.UseVisualStyleBackColor = false;
            this.btn3D.Click += new System.EventHandler(this.btn3D_Click);
            // 
            // btn2D
            // 
            this.btn2D.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn2D.BackColor = System.Drawing.Color.Transparent;
            this.btn2D.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn2D.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btn2D.FlatAppearance.BorderSize = 0;
            this.btn2D.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btn2D.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn2D.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn2D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn2D.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2D.Image = global::AOG.Properties.Resources.Camera2D64;
            this.btn2D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn2D.Location = new System.Drawing.Point(16, 110);
            this.btn2D.Name = "btn2D";
            this.btn2D.Size = new System.Drawing.Size(57, 55);
            this.btn2D.TabIndex = 469;
            this.btn2D.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn2D.UseVisualStyleBackColor = false;
            this.btn2D.Click += new System.EventHandler(this.btn2D_Click);
            // 
            // btnGrid
            // 
            this.btnGrid.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnGrid.BackColor = System.Drawing.Color.Transparent;
            this.btnGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGrid.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnGrid.FlatAppearance.BorderSize = 0;
            this.btnGrid.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnGrid.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGrid.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGrid.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGrid.Image = global::AOG.Properties.Resources.GridRotate;
            this.btnGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGrid.Location = new System.Drawing.Point(94, 188);
            this.btnGrid.Name = "btnGrid";
            this.btnGrid.Size = new System.Drawing.Size(80, 83);
            this.btnGrid.TabIndex = 543;
            this.btnGrid.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGrid.UseVisualStyleBackColor = false;
            this.btnGrid.Click += new System.EventHandler(this.btnGrid_Click);
            // 
            // btnN2D
            // 
            this.btnN2D.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnN2D.BackColor = System.Drawing.Color.Transparent;
            this.btnN2D.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnN2D.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnN2D.FlatAppearance.BorderSize = 0;
            this.btnN2D.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnN2D.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnN2D.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnN2D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnN2D.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnN2D.Image = global::AOG.Properties.Resources.CameraNorth2D;
            this.btnN2D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnN2D.Location = new System.Drawing.Point(16, 202);
            this.btnN2D.Name = "btnN2D";
            this.btnN2D.Size = new System.Drawing.Size(57, 55);
            this.btnN2D.TabIndex = 469;
            this.btnN2D.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnN2D.UseVisualStyleBackColor = false;
            this.btnN2D.Click += new System.EventHandler(this.btnN2D_Click);
            // 
            // lblSpeed
            // 
            this.lblSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSpeed.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeed.ForeColor = System.Drawing.Color.Black;
            this.lblSpeed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSpeed.Location = new System.Drawing.Point(193, 7);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(87, 35);
            this.lblSpeed.TabIndex = 116;
            this.lblSpeed.Text = "88.88";
            this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblFix
            // 
            this.lblFix.AutoSize = true;
            this.lblFix.BackColor = System.Drawing.Color.Transparent;
            this.lblFix.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFix.Location = new System.Drawing.Point(84, -1);
            this.lblFix.Name = "lblFix";
            this.lblFix.Size = new System.Drawing.Size(87, 19);
            this.lblFix.TabIndex = 489;
            this.lblFix.Text = "Fix       sec";
            this.lblFix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelLeft
            // 
            this.panelLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelLeft.ColumnCount = 1;
            this.panelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelLeft.Controls.Add(this.statusStrip2, 0, 3);
            this.panelLeft.Controls.Add(this.btnNavigationSettings, 0, 1);
            this.panelLeft.Controls.Add(this.btnAutoSteerConfig, 0, 6);
            this.panelLeft.Controls.Add(this.btnStartAgIO, 0, 7);
            this.panelLeft.Controls.Add(this.btnFieldMenu, 0, 4);
            this.panelLeft.Controls.Add(this.statusStripLeft, 0, 2);
            this.panelLeft.Controls.Add(this.statusStrip1, 0, 5);
            this.panelLeft.Location = new System.Drawing.Point(5, 50);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.RowCount = 8;
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.40268F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.63087F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.91946F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.9396F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.10068F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.75167F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.25503F));
            this.panelLeft.Size = new System.Drawing.Size(70, 631);
            this.panelLeft.TabIndex = 529;
            // 
            // statusStrip2
            // 
            this.statusStrip2.AllowMerge = false;
            this.statusStrip2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.statusStrip2.AutoSize = false;
            this.statusStrip2.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.statusStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip2.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip2.ImageScalingSize = new System.Drawing.Size(60, 60);
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.statusStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip2.Location = new System.Drawing.Point(1, 192);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(67, 74);
            this.statusStrip2.SizingGrip = false;
            this.statusStrip2.TabIndex = 544;
            this.statusStrip2.Text = "UDP";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.AutoSize = false;
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripConfig,
            this.toolStripSteerSettings,
            this.toolSteerToolStripMenuItem,
            this.toolStripAllSettings,
            this.toolStripWorkingDirectories,
            this.toolStripGPSData,
            this.toolStripColors,
            this.toolStripSectionColors,
            this.toolStripHotkeys});
            this.toolStripDropDownButton1.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripDropDownButton1.Image = global::AOG.Properties.Resources.Settings48;
            this.toolStripDropDownButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.ShowDropDownArrow = false;
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(66, 80);
            this.toolStripDropDownButton1.Text = "All";
            // 
            // toolStripConfig
            // 
            this.toolStripConfig.Image = global::AOG.Properties.Resources.Settings48;
            this.toolStripConfig.Name = "toolStripConfig";
            this.toolStripConfig.Size = new System.Drawing.Size(419, 44);
            this.toolStripConfig.Text = "Configuration";
            this.toolStripConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // toolStripSteerSettings
            // 
            this.toolStripSteerSettings.Image = global::AOG.Properties.Resources.AutoSteerOff;
            this.toolStripSteerSettings.Name = "toolStripSteerSettings";
            this.toolStripSteerSettings.Size = new System.Drawing.Size(419, 44);
            this.toolStripSteerSettings.Text = "Auto Steer";
            this.toolStripSteerSettings.Click += new System.EventHandler(this.btnAutoSteerConfig_Click);
            // 
            // toolSteerToolStripMenuItem
            // 
            this.toolSteerToolStripMenuItem.Image = global::AOG.Properties.Resources.ToolSteer;
            this.toolSteerToolStripMenuItem.Name = "toolSteerToolStripMenuItem";
            this.toolSteerToolStripMenuItem.Size = new System.Drawing.Size(419, 44);
            this.toolSteerToolStripMenuItem.Text = "Tool Steer";
            this.toolSteerToolStripMenuItem.Click += new System.EventHandler(this.toolSteerToolStripMenuItem_Click);
            // 
            // toolStripAllSettings
            // 
            this.toolStripAllSettings.Image = global::AOG.Properties.Resources.ScreenShot;
            this.toolStripAllSettings.Name = "toolStripAllSettings";
            this.toolStripAllSettings.Size = new System.Drawing.Size(419, 44);
            this.toolStripAllSettings.Text = "View All Settings";
            this.toolStripAllSettings.Click += new System.EventHandler(this.allSettingsMenuItem_Click);
            // 
            // toolStripWorkingDirectories
            // 
            this.toolStripWorkingDirectories.Image = global::AOG.Properties.Resources.FileOpen;
            this.toolStripWorkingDirectories.Name = "toolStripWorkingDirectories";
            this.toolStripWorkingDirectories.Size = new System.Drawing.Size(419, 44);
            this.toolStripWorkingDirectories.Text = "Directories";
            this.toolStripWorkingDirectories.Click += new System.EventHandler(this.setWorkingDirectoryToolStripMenuItem_Click);
            // 
            // toolStripGPSData
            // 
            this.toolStripGPSData.Image = global::AOG.Properties.Resources.GPSQuality;
            this.toolStripGPSData.Name = "toolStripGPSData";
            this.toolStripGPSData.Size = new System.Drawing.Size(419, 44);
            this.toolStripGPSData.Text = "GPS Data";
            this.toolStripGPSData.Click += new System.EventHandler(this.btnGPSData_Click);
            // 
            // toolStripColors
            // 
            this.toolStripColors.Image = global::AOG.Properties.Resources.ColourPick;
            this.toolStripColors.Name = "toolStripColors";
            this.toolStripColors.Size = new System.Drawing.Size(419, 44);
            this.toolStripColors.Text = "Colors";
            this.toolStripColors.Click += new System.EventHandler(this.colorsToolStripMenuItem_Click);
            // 
            // toolStripSectionColors
            // 
            this.toolStripSectionColors.Image = global::AOG.Properties.Resources.SectionMapping;
            this.toolStripSectionColors.Name = "toolStripSectionColors";
            this.toolStripSectionColors.Size = new System.Drawing.Size(419, 44);
            this.toolStripSectionColors.Text = "Multi-Section Colors";
            this.toolStripSectionColors.Click += new System.EventHandler(this.colorsSectionToolStripMenuItem_Click);
            // 
            // toolStripHotkeys
            // 
            this.toolStripHotkeys.Image = global::AOG.Properties.Resources.ConD_KeyBoard;
            this.toolStripHotkeys.Name = "toolStripHotkeys";
            this.toolStripHotkeys.Size = new System.Drawing.Size(419, 44);
            this.toolStripHotkeys.Text = "HotKeys";
            this.toolStripHotkeys.Click += new System.EventHandler(this.hotKeysToolStripMenuItem_Click);
            // 
            // btnNavigationSettings
            // 
            this.btnNavigationSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnNavigationSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNavigationSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNavigationSettings.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnNavigationSettings.FlatAppearance.BorderSize = 0;
            this.btnNavigationSettings.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNavigationSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNavigationSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNavigationSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavigationSettings.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavigationSettings.ForeColor = System.Drawing.Color.DarkGray;
            this.btnNavigationSettings.Image = global::AOG.Properties.Resources.NavigationSettings;
            this.btnNavigationSettings.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNavigationSettings.Location = new System.Drawing.Point(0, 1);
            this.btnNavigationSettings.Margin = new System.Windows.Forms.Padding(0);
            this.btnNavigationSettings.Name = "btnNavigationSettings";
            this.btnNavigationSettings.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnNavigationSettings.Size = new System.Drawing.Size(70, 65);
            this.btnNavigationSettings.TabIndex = 542;
            this.btnNavigationSettings.UseVisualStyleBackColor = false;
            this.btnNavigationSettings.Click += new System.EventHandler(this.btnNavigationSettings_Click);
            // 
            // btnAutoSteerConfig
            // 
            this.btnAutoSteerConfig.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteerConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAutoSteerConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAutoSteerConfig.FlatAppearance.BorderSize = 0;
            this.btnAutoSteerConfig.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteerConfig.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteerConfig.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteerConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSteerConfig.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoSteerConfig.ForeColor = System.Drawing.Color.Black;
            this.btnAutoSteerConfig.Image = global::AOG.Properties.Resources.AutoSteerConf;
            this.btnAutoSteerConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAutoSteerConfig.Location = new System.Drawing.Point(0, 465);
            this.btnAutoSteerConfig.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoSteerConfig.Name = "btnAutoSteerConfig";
            this.btnAutoSteerConfig.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnAutoSteerConfig.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAutoSteerConfig.Size = new System.Drawing.Size(70, 80);
            this.btnAutoSteerConfig.TabIndex = 475;
            this.btnAutoSteerConfig.Text = "-38.8.";
            this.btnAutoSteerConfig.UseVisualStyleBackColor = false;
            this.btnAutoSteerConfig.Click += new System.EventHandler(this.btnAutoSteerConfig_Click);
            // 
            // btnStartAgIO
            // 
            this.btnStartAgIO.BackColor = System.Drawing.Color.Transparent;
            this.btnStartAgIO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStartAgIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStartAgIO.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnStartAgIO.FlatAppearance.BorderSize = 0;
            this.btnStartAgIO.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnStartAgIO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStartAgIO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStartAgIO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartAgIO.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartAgIO.ForeColor = System.Drawing.Color.Black;
            this.btnStartAgIO.Image = global::AOG.Properties.Resources.AgIO;
            this.btnStartAgIO.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStartAgIO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStartAgIO.Location = new System.Drawing.Point(0, 545);
            this.btnStartAgIO.Margin = new System.Windows.Forms.Padding(0);
            this.btnStartAgIO.Name = "btnStartAgIO";
            this.btnStartAgIO.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnStartAgIO.Size = new System.Drawing.Size(70, 86);
            this.btnStartAgIO.TabIndex = 467;
            this.btnStartAgIO.Text = "12.3";
            this.btnStartAgIO.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStartAgIO.UseVisualStyleBackColor = false;
            this.btnStartAgIO.Click += new System.EventHandler(this.btnStartAgIO_Click);
            // 
            // btnFieldMenu
            // 
            this.btnFieldMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFieldMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFieldMenu.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnFieldMenu.FlatAppearance.BorderSize = 0;
            this.btnFieldMenu.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnFieldMenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFieldMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFieldMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldMenu.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFieldMenu.ForeColor = System.Drawing.Color.DarkGray;
            this.btnFieldMenu.Image = global::AOG.Properties.Resources.JobActive;
            this.btnFieldMenu.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFieldMenu.Location = new System.Drawing.Point(0, 270);
            this.btnFieldMenu.Margin = new System.Windows.Forms.Padding(0);
            this.btnFieldMenu.Name = "btnFieldMenu";
            this.btnFieldMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnFieldMenu.Size = new System.Drawing.Size(70, 100);
            this.btnFieldMenu.TabIndex = 536;
            this.btnFieldMenu.UseVisualStyleBackColor = false;
            this.btnFieldMenu.Click += new System.EventHandler(this.btnFieldMenu_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AllowMerge = false;
            this.statusStrip1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(60, 60);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnFieldTools});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(1, 375);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(67, 84);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 543;
            this.statusStrip1.Text = "UDP";
            // 
            // toolStripBtnFieldTools
            // 
            this.toolStripBtnFieldTools.AutoSize = false;
            this.toolStripBtnFieldTools.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStripBtnFieldTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnFieldTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boundariesToolStripMenuItem,
            this.headlandToolStripMenuItem,
            this.headlandBuildToolStripMenuItem,
            this.tramsMultiMenuField,
            this.deleteAppliedToolStripMenuItem,
            this.flagByLatLonToolStripMenuItem,
            this.recordedPathStripMenu});
            this.toolStripBtnFieldTools.Enabled = false;
            this.toolStripBtnFieldTools.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripBtnFieldTools.Image = global::AOG.Properties.Resources.FieldTools;
            this.toolStripBtnFieldTools.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripBtnFieldTools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnFieldTools.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnFieldTools.Name = "toolStripBtnFieldTools";
            this.toolStripBtnFieldTools.ShowDropDownArrow = false;
            this.toolStripBtnFieldTools.Size = new System.Drawing.Size(65, 80);
            // 
            // boundariesToolStripMenuItem
            // 
            this.boundariesToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boundariesToolStripMenuItem.Image = global::AOG.Properties.Resources.Boundary;
            this.boundariesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.boundariesToolStripMenuItem.Name = "boundariesToolStripMenuItem";
            this.boundariesToolStripMenuItem.Size = new System.Drawing.Size(445, 70);
            this.boundariesToolStripMenuItem.Text = "Boundary";
            this.boundariesToolStripMenuItem.Click += new System.EventHandler(this.boundariesToolStripMenuItem_Click);
            // 
            // headlandToolStripMenuItem
            // 
            this.headlandToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headlandToolStripMenuItem.Image = global::AOG.Properties.Resources.HeadlandBuild;
            this.headlandToolStripMenuItem.Name = "headlandToolStripMenuItem";
            this.headlandToolStripMenuItem.Size = new System.Drawing.Size(445, 70);
            this.headlandToolStripMenuItem.Text = "Headland";
            this.headlandToolStripMenuItem.Click += new System.EventHandler(this.headlandToolStripMenuItem_Click);
            // 
            // headlandBuildToolStripMenuItem
            // 
            this.headlandBuildToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headlandBuildToolStripMenuItem.Image = global::AOG.Properties.Resources.Headache;
            this.headlandBuildToolStripMenuItem.Name = "headlandBuildToolStripMenuItem";
            this.headlandBuildToolStripMenuItem.Size = new System.Drawing.Size(445, 70);
            this.headlandBuildToolStripMenuItem.Text = "Headland (Build)";
            this.headlandBuildToolStripMenuItem.Click += new System.EventHandler(this.headlandBuildToolStripMenuItem_Click);
            // 
            // tramsMultiMenuField
            // 
            this.tramsMultiMenuField.Image = global::AOG.Properties.Resources.TramMulti;
            this.tramsMultiMenuField.Name = "tramsMultiMenuField";
            this.tramsMultiMenuField.Size = new System.Drawing.Size(445, 70);
            this.tramsMultiMenuField.Text = "Multi Tramlines";
            this.tramsMultiMenuField.Click += new System.EventHandler(this.tramLinesMenuMulti_Click);
            // 
            // deleteAppliedToolStripMenuItem
            // 
            this.deleteAppliedToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteAppliedToolStripMenuItem.Image = global::AOG.Properties.Resources.TrashApplied;
            this.deleteAppliedToolStripMenuItem.Name = "deleteAppliedToolStripMenuItem";
            this.deleteAppliedToolStripMenuItem.Size = new System.Drawing.Size(445, 70);
            this.deleteAppliedToolStripMenuItem.Text = "Delete Applied";
            this.deleteAppliedToolStripMenuItem.Click += new System.EventHandler(this.toolStripAreYouSure_Click);
            // 
            // flagByLatLonToolStripMenuItem
            // 
            this.flagByLatLonToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flagByLatLonToolStripMenuItem.Image = global::AOG.Properties.Resources.FlagRed;
            this.flagByLatLonToolStripMenuItem.Name = "flagByLatLonToolStripMenuItem";
            this.flagByLatLonToolStripMenuItem.Size = new System.Drawing.Size(445, 70);
            this.flagByLatLonToolStripMenuItem.Text = "Flag By Lat Lon";
            this.flagByLatLonToolStripMenuItem.Click += new System.EventHandler(this.flagByLatLonToolStripMenuItem_Click);
            // 
            // recordedPathStripMenu
            // 
            this.recordedPathStripMenu.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recordedPathStripMenu.Image = global::AOG.Properties.Resources.RecPath;
            this.recordedPathStripMenu.Name = "recordedPathStripMenu";
            this.recordedPathStripMenu.Size = new System.Drawing.Size(445, 70);
            this.recordedPathStripMenu.Text = "Recorded Path";
            // 
            // lblCurrentField
            // 
            this.lblCurrentField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentField.AutoSize = true;
            this.lblCurrentField.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentField.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentField.Location = new System.Drawing.Point(82, 25);
            this.lblCurrentField.Name = "lblCurrentField";
            this.lblCurrentField.Size = new System.Drawing.Size(140, 24);
            this.lblCurrentField.TabIndex = 534;
            this.lblCurrentField.Text = "Current Field";
            this.lblCurrentField.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblCurrentField.Click += new System.EventHandler(this.lblCurrentField_Click);
            // 
            // lblGuidanceLine
            // 
            this.lblGuidanceLine.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblGuidanceLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGuidanceLine.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuidanceLine.Location = new System.Drawing.Point(214, 66);
            this.lblGuidanceLine.Name = "lblGuidanceLine";
            this.lblGuidanceLine.Size = new System.Drawing.Size(583, 83);
            this.lblGuidanceLine.TabIndex = 538;
            this.lblGuidanceLine.Text = "This is the line description";
            this.lblGuidanceLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGuidanceLine.Visible = false;
            // 
            // flp1
            // 
            this.flp1.AutoSize = true;
            this.flp1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flp1.Controls.Add(this.btnNudge);
            this.flp1.Controls.Add(this.btnABDraw);
            this.flp1.Controls.Add(this.btnPlusAB);
            this.flp1.Controls.Add(this.btnBuildTracks);
            this.flp1.Controls.Add(this.btnTracksOff);
            this.flp1.Controls.Add(this.btnRefNudge);
            this.flp1.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.flp1.Location = new System.Drawing.Point(800, 48);
            this.flp1.Name = "flp1";
            this.flp1.Size = new System.Drawing.Size(86, 488);
            this.flp1.TabIndex = 539;
            this.flp1.Visible = false;
            // 
            // btnNudge
            // 
            this.btnNudge.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnNudge.BackColor = System.Drawing.Color.Transparent;
            this.btnNudge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNudge.FlatAppearance.BorderSize = 0;
            this.btnNudge.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNudge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNudge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNudge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNudge.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNudge.Image = global::AOG.Properties.Resources.ABSnapNudgeMenu;
            this.btnNudge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNudge.Location = new System.Drawing.Point(3, 413);
            this.btnNudge.Name = "btnNudge";
            this.btnNudge.Size = new System.Drawing.Size(80, 72);
            this.btnNudge.TabIndex = 489;
            this.btnNudge.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNudge.UseVisualStyleBackColor = false;
            this.btnNudge.Click += new System.EventHandler(this.btnNudge_Click);
            // 
            // btnABDraw
            // 
            this.btnABDraw.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnABDraw.BackColor = System.Drawing.Color.Transparent;
            this.btnABDraw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnABDraw.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnABDraw.FlatAppearance.BorderSize = 0;
            this.btnABDraw.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnABDraw.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnABDraw.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnABDraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnABDraw.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnABDraw.Image = global::AOG.Properties.Resources.ABDraw;
            this.btnABDraw.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnABDraw.Location = new System.Drawing.Point(3, 335);
            this.btnABDraw.Name = "btnABDraw";
            this.btnABDraw.Size = new System.Drawing.Size(80, 72);
            this.btnABDraw.TabIndex = 250;
            this.btnABDraw.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnABDraw.UseVisualStyleBackColor = false;
            this.btnABDraw.Visible = false;
            this.btnABDraw.Click += new System.EventHandler(this.btnABDraw_Click);
            // 
            // btnPlusAB
            // 
            this.btnPlusAB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPlusAB.BackColor = System.Drawing.Color.Transparent;
            this.btnPlusAB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPlusAB.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnPlusAB.FlatAppearance.BorderSize = 0;
            this.btnPlusAB.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPlusAB.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPlusAB.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPlusAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlusAB.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnPlusAB.Image = global::AOG.Properties.Resources.AddNew;
            this.btnPlusAB.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPlusAB.Location = new System.Drawing.Point(3, 252);
            this.btnPlusAB.Name = "btnPlusAB";
            this.btnPlusAB.Size = new System.Drawing.Size(80, 77);
            this.btnPlusAB.TabIndex = 545;
            this.btnPlusAB.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPlusAB.UseVisualStyleBackColor = false;
            this.btnPlusAB.Click += new System.EventHandler(this.btnPlusAB_Click);
            // 
            // btnBuildTracks
            // 
            this.btnBuildTracks.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBuildTracks.BackColor = System.Drawing.Color.Transparent;
            this.btnBuildTracks.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBuildTracks.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnBuildTracks.FlatAppearance.BorderSize = 0;
            this.btnBuildTracks.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBuildTracks.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBuildTracks.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBuildTracks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuildTracks.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnBuildTracks.Image = global::AOG.Properties.Resources.ABTracks;
            this.btnBuildTracks.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBuildTracks.Location = new System.Drawing.Point(3, 169);
            this.btnBuildTracks.Name = "btnBuildTracks";
            this.btnBuildTracks.Size = new System.Drawing.Size(80, 77);
            this.btnBuildTracks.TabIndex = 539;
            this.btnBuildTracks.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBuildTracks.UseVisualStyleBackColor = false;
            this.btnBuildTracks.Click += new System.EventHandler(this.btnBuildTracks_Click);
            // 
            // btnTracksOff
            // 
            this.btnTracksOff.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTracksOff.BackColor = System.Drawing.Color.Transparent;
            this.btnTracksOff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTracksOff.FlatAppearance.BorderSize = 0;
            this.btnTracksOff.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTracksOff.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTracksOff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTracksOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTracksOff.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTracksOff.Image = global::AOG.Properties.Resources.SwitchOff;
            this.btnTracksOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTracksOff.Location = new System.Drawing.Point(3, 86);
            this.btnTracksOff.Name = "btnTracksOff";
            this.btnTracksOff.Size = new System.Drawing.Size(80, 77);
            this.btnTracksOff.TabIndex = 539;
            this.btnTracksOff.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnTracksOff.UseVisualStyleBackColor = false;
            this.btnTracksOff.Click += new System.EventHandler(this.btnTracksOff_Click);
            // 
            // btnRefNudge
            // 
            this.btnRefNudge.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRefNudge.BackColor = System.Drawing.Color.Transparent;
            this.btnRefNudge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRefNudge.FlatAppearance.BorderSize = 0;
            this.btnRefNudge.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRefNudge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefNudge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefNudge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefNudge.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefNudge.Image = global::AOG.Properties.Resources.ABSnapNudgeMenuRef;
            this.btnRefNudge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRefNudge.Location = new System.Drawing.Point(3, 3);
            this.btnRefNudge.Name = "btnRefNudge";
            this.btnRefNudge.Size = new System.Drawing.Size(80, 77);
            this.btnRefNudge.TabIndex = 538;
            this.btnRefNudge.Text = "Ref";
            this.btnRefNudge.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRefNudge.UseVisualStyleBackColor = false;
            this.btnRefNudge.Click += new System.EventHandler(this.btnRefNudge_Click);
            // 
            // panelBottom
            // 
            this.panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBottom.AutoSize = true;
            this.panelBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBottom.Controls.Add(this.btnTrack);
            this.panelBottom.Controls.Add(this.btnSnapToPivot);
            this.panelBottom.Controls.Add(this.btnAdjRight);
            this.panelBottom.Controls.Add(this.btnAdjLeft);
            this.panelBottom.Controls.Add(this.btnFlag);
            this.panelBottom.Controls.Add(this.btnHeadlandOnOff);
            this.panelBottom.Controls.Add(this.cboxIsSectionControlled);
            this.panelBottom.Controls.Add(this.btnHydLift);
            this.panelBottom.Controls.Add(this.btnTramDisplayMode);
            this.panelBottom.Controls.Add(this.btnResetToolHeading);
            this.panelBottom.Controls.Add(this.btnChangeMappingColor);
            this.panelBottom.Controls.Add(this.btnYouSkipEnable);
            this.panelBottom.Controls.Add(this.cboxpRowWidth);
            this.panelBottom.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelBottom.Location = new System.Drawing.Point(-43, 684);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(974, 62);
            this.panelBottom.TabIndex = 540;
            // 
            // btnTrack
            // 
            this.btnTrack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTrack.BackColor = System.Drawing.Color.Transparent;
            this.btnTrack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnTrack.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnTrack.FlatAppearance.BorderSize = 0;
            this.btnTrack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTrack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrack.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrack.Image = global::AOG.Properties.Resources.TrackOn;
            this.btnTrack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTrack.Location = new System.Drawing.Point(902, 3);
            this.btnTrack.Margin = new System.Windows.Forms.Padding(0);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(72, 56);
            this.btnTrack.TabIndex = 173;
            this.btnTrack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTrack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnTrack.UseVisualStyleBackColor = false;
            this.btnTrack.Click += new System.EventHandler(this.btnTrack_Click);
            // 
            // btnSnapToPivot
            // 
            this.btnSnapToPivot.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSnapToPivot.BackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSnapToPivot.FlatAppearance.BorderSize = 0;
            this.btnSnapToPivot.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSnapToPivot.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSnapToPivot.Image = global::AOG.Properties.Resources.SnapToPivot;
            this.btnSnapToPivot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSnapToPivot.Location = new System.Drawing.Point(827, 3);
            this.btnSnapToPivot.Name = "btnSnapToPivot";
            this.btnSnapToPivot.Size = new System.Drawing.Size(72, 56);
            this.btnSnapToPivot.TabIndex = 544;
            this.btnSnapToPivot.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSnapToPivot.UseVisualStyleBackColor = false;
            this.btnSnapToPivot.Click += new System.EventHandler(this.btnSnapToPivot_Click);
            // 
            // btnAdjRight
            // 
            this.btnAdjRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdjRight.BackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnAdjRight.FlatAppearance.BorderSize = 0;
            this.btnAdjRight.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdjRight.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAdjRight.Image = global::AOG.Properties.Resources.SnapRight;
            this.btnAdjRight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdjRight.Location = new System.Drawing.Point(749, 3);
            this.btnAdjRight.Name = "btnAdjRight";
            this.btnAdjRight.Size = new System.Drawing.Size(72, 56);
            this.btnAdjRight.TabIndex = 546;
            this.btnAdjRight.UseVisualStyleBackColor = false;
            this.btnAdjRight.Click += new System.EventHandler(this.btnAdjRight_Click);
            // 
            // btnAdjLeft
            // 
            this.btnAdjLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdjLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnAdjLeft.FlatAppearance.BorderSize = 0;
            this.btnAdjLeft.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdjLeft.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAdjLeft.Image = global::AOG.Properties.Resources.SnapLeft;
            this.btnAdjLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdjLeft.Location = new System.Drawing.Point(671, 3);
            this.btnAdjLeft.Name = "btnAdjLeft";
            this.btnAdjLeft.Size = new System.Drawing.Size(72, 56);
            this.btnAdjLeft.TabIndex = 545;
            this.btnAdjLeft.UseVisualStyleBackColor = false;
            this.btnAdjLeft.Click += new System.EventHandler(this.btnAdjLeft_Click);
            // 
            // btnFlag
            // 
            this.btnFlag.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFlag.BackColor = System.Drawing.Color.Transparent;
            this.btnFlag.ContextMenuStrip = this.contextMenuStripFlag;
            this.btnFlag.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnFlag.FlatAppearance.BorderSize = 0;
            this.btnFlag.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnFlag.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFlag.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFlag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFlag.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFlag.Image = global::AOG.Properties.Resources.FlagRed;
            this.btnFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFlag.Location = new System.Drawing.Point(593, 3);
            this.btnFlag.Name = "btnFlag";
            this.btnFlag.Size = new System.Drawing.Size(72, 56);
            this.btnFlag.TabIndex = 121;
            this.btnFlag.Text = "P";
            this.btnFlag.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFlag.UseVisualStyleBackColor = false;
            this.btnFlag.Click += new System.EventHandler(this.btnFlag_Click);
            // 
            // btnHeadlandOnOff
            // 
            this.btnHeadlandOnOff.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHeadlandOnOff.BackColor = System.Drawing.Color.Transparent;
            this.btnHeadlandOnOff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnHeadlandOnOff.FlatAppearance.BorderSize = 0;
            this.btnHeadlandOnOff.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHeadlandOnOff.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHeadlandOnOff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHeadlandOnOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHeadlandOnOff.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHeadlandOnOff.Image = global::AOG.Properties.Resources.HeadlandOff;
            this.btnHeadlandOnOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHeadlandOnOff.Location = new System.Drawing.Point(515, 3);
            this.btnHeadlandOnOff.Name = "btnHeadlandOnOff";
            this.btnHeadlandOnOff.Size = new System.Drawing.Size(72, 56);
            this.btnHeadlandOnOff.TabIndex = 447;
            this.btnHeadlandOnOff.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHeadlandOnOff.UseVisualStyleBackColor = false;
            this.btnHeadlandOnOff.Visible = false;
            this.btnHeadlandOnOff.Click += new System.EventHandler(this.btnHeadlandOnOff_Click);
            // 
            // cboxIsSectionControlled
            // 
            this.cboxIsSectionControlled.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxIsSectionControlled.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsSectionControlled.BackColor = System.Drawing.Color.Transparent;
            this.cboxIsSectionControlled.Checked = true;
            this.cboxIsSectionControlled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxIsSectionControlled.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.cboxIsSectionControlled.FlatAppearance.BorderSize = 0;
            this.cboxIsSectionControlled.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cboxIsSectionControlled.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cboxIsSectionControlled.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cboxIsSectionControlled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsSectionControlled.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsSectionControlled.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cboxIsSectionControlled.Image = global::AOG.Properties.Resources.HeadlandSectionOn;
            this.cboxIsSectionControlled.Location = new System.Drawing.Point(437, 3);
            this.cboxIsSectionControlled.Name = "cboxIsSectionControlled";
            this.cboxIsSectionControlled.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsSectionControlled.Size = new System.Drawing.Size(72, 56);
            this.cboxIsSectionControlled.TabIndex = 544;
            this.cboxIsSectionControlled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsSectionControlled.UseVisualStyleBackColor = false;
            this.cboxIsSectionControlled.Click += new System.EventHandler(this.cboxIsSectionControlled_Click);
            // 
            // btnHydLift
            // 
            this.btnHydLift.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHydLift.BackColor = System.Drawing.Color.Transparent;
            this.btnHydLift.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHydLift.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnHydLift.FlatAppearance.BorderSize = 0;
            this.btnHydLift.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHydLift.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHydLift.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHydLift.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHydLift.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnHydLift.Image = global::AOG.Properties.Resources.HydraulicLiftOff;
            this.btnHydLift.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHydLift.Location = new System.Drawing.Point(359, 3);
            this.btnHydLift.Name = "btnHydLift";
            this.btnHydLift.Size = new System.Drawing.Size(72, 56);
            this.btnHydLift.TabIndex = 453;
            this.btnHydLift.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnHydLift.UseVisualStyleBackColor = false;
            this.btnHydLift.Visible = false;
            this.btnHydLift.Click += new System.EventHandler(this.btnHydLift_Click);
            // 
            // btnTramDisplayMode
            // 
            this.btnTramDisplayMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTramDisplayMode.BackColor = System.Drawing.Color.Transparent;
            this.btnTramDisplayMode.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnTramDisplayMode.FlatAppearance.BorderSize = 0;
            this.btnTramDisplayMode.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTramDisplayMode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTramDisplayMode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTramDisplayMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTramDisplayMode.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTramDisplayMode.Image = global::AOG.Properties.Resources.TramOff;
            this.btnTramDisplayMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTramDisplayMode.Location = new System.Drawing.Point(281, 3);
            this.btnTramDisplayMode.Name = "btnTramDisplayMode";
            this.btnTramDisplayMode.Size = new System.Drawing.Size(72, 56);
            this.btnTramDisplayMode.TabIndex = 480;
            this.btnTramDisplayMode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTramDisplayMode.UseVisualStyleBackColor = false;
            this.btnTramDisplayMode.Click += new System.EventHandler(this.btnTramDisplayMode_Click);
            // 
            // btnResetToolHeading
            // 
            this.btnResetToolHeading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnResetToolHeading.BackColor = System.Drawing.Color.Transparent;
            this.btnResetToolHeading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnResetToolHeading.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnResetToolHeading.FlatAppearance.BorderSize = 0;
            this.btnResetToolHeading.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnResetToolHeading.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnResetToolHeading.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnResetToolHeading.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetToolHeading.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetToolHeading.ForeColor = System.Drawing.Color.Black;
            this.btnResetToolHeading.Image = global::AOG.Properties.Resources.ResetTool;
            this.btnResetToolHeading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetToolHeading.Location = new System.Drawing.Point(206, 3);
            this.btnResetToolHeading.Margin = new System.Windows.Forms.Padding(0);
            this.btnResetToolHeading.Name = "btnResetToolHeading";
            this.btnResetToolHeading.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnResetToolHeading.Size = new System.Drawing.Size(72, 56);
            this.btnResetToolHeading.TabIndex = 491;
            this.btnResetToolHeading.UseVisualStyleBackColor = false;
            this.btnResetToolHeading.Click += new System.EventHandler(this.btnResetToolHeading_Click);
            // 
            // btnChangeMappingColor
            // 
            this.btnChangeMappingColor.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnChangeMappingColor.BackColor = System.Drawing.Color.Transparent;
            this.btnChangeMappingColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnChangeMappingColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnChangeMappingColor.FlatAppearance.BorderSize = 0;
            this.btnChangeMappingColor.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnChangeMappingColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChangeMappingColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChangeMappingColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeMappingColor.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeMappingColor.ForeColor = System.Drawing.Color.Black;
            this.btnChangeMappingColor.Image = global::AOG.Properties.Resources.SectionMapping;
            this.btnChangeMappingColor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChangeMappingColor.Location = new System.Drawing.Point(134, 3);
            this.btnChangeMappingColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnChangeMappingColor.Name = "btnChangeMappingColor";
            this.btnChangeMappingColor.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnChangeMappingColor.Size = new System.Drawing.Size(72, 56);
            this.btnChangeMappingColor.TabIndex = 476;
            this.btnChangeMappingColor.Text = "5.8.4";
            this.btnChangeMappingColor.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnChangeMappingColor.UseVisualStyleBackColor = false;
            this.btnChangeMappingColor.Click += new System.EventHandler(this.btnChangeMappingColor_Click);
            // 
            // btnYouSkipEnable
            // 
            this.btnYouSkipEnable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnYouSkipEnable.BackColor = System.Drawing.Color.Transparent;
            this.btnYouSkipEnable.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnYouSkipEnable.FlatAppearance.BorderSize = 0;
            this.btnYouSkipEnable.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnYouSkipEnable.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYouSkipEnable.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYouSkipEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYouSkipEnable.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYouSkipEnable.Image = global::AOG.Properties.Resources.YouSkipOff;
            this.btnYouSkipEnable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnYouSkipEnable.Location = new System.Drawing.Point(59, 3);
            this.btnYouSkipEnable.Name = "btnYouSkipEnable";
            this.btnYouSkipEnable.Size = new System.Drawing.Size(72, 56);
            this.btnYouSkipEnable.TabIndex = 490;
            this.btnYouSkipEnable.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnYouSkipEnable.UseVisualStyleBackColor = false;
            this.btnYouSkipEnable.Click += new System.EventHandler(this.btnYouSkipEnable_Click);
            // 
            // panelRight
            // 
            this.panelRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRight.AutoSize = true;
            this.panelRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelRight.Controls.Add(this.btnAutoSteer);
            this.panelRight.Controls.Add(this.btnAutoYouTurn);
            this.panelRight.Controls.Add(this.btnSectionMasterAuto);
            this.panelRight.Controls.Add(this.btnSectionMasterManual);
            this.panelRight.Controls.Add(this.btnCycleLinesBk);
            this.panelRight.Controls.Add(this.btnCycleLines);
            this.panelRight.Controls.Add(this.btnContour);
            this.panelRight.Controls.Add(this.btnContourLock);
            this.panelRight.Controls.Add(this.lblNumCu);
            this.panelRight.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.panelRight.Location = new System.Drawing.Point(931, 172);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(70, 574);
            this.panelRight.TabIndex = 541;
            this.panelRight.Visible = false;
            // 
            // btnAutoSteer
            // 
            this.btnAutoSteer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAutoSteer.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteer.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnAutoSteer.FlatAppearance.BorderSize = 0;
            this.btnAutoSteer.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSteer.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnAutoSteer.Image = global::AOG.Properties.Resources.AutoSteerOff;
            this.btnAutoSteer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAutoSteer.Location = new System.Drawing.Point(3, 510);
            this.btnAutoSteer.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoSteer.Name = "btnAutoSteer";
            this.btnAutoSteer.Size = new System.Drawing.Size(64, 64);
            this.btnAutoSteer.TabIndex = 128;
            this.btnAutoSteer.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnAutoSteer.UseVisualStyleBackColor = false;
            this.btnAutoSteer.Click += new System.EventHandler(this.btnAutoSteer_Click);
            // 
            // btnAutoYouTurn
            // 
            this.btnAutoYouTurn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAutoYouTurn.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoYouTurn.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnAutoYouTurn.FlatAppearance.BorderSize = 0;
            this.btnAutoYouTurn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAutoYouTurn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAutoYouTurn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAutoYouTurn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoYouTurn.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAutoYouTurn.Image = global::AOG.Properties.Resources.YouTurnNo;
            this.btnAutoYouTurn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAutoYouTurn.Location = new System.Drawing.Point(3, 443);
            this.btnAutoYouTurn.Name = "btnAutoYouTurn";
            this.btnAutoYouTurn.Size = new System.Drawing.Size(64, 64);
            this.btnAutoYouTurn.TabIndex = 132;
            this.btnAutoYouTurn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAutoYouTurn.UseVisualStyleBackColor = false;
            this.btnAutoYouTurn.Click += new System.EventHandler(this.btnAutoYouTurn_Click);
            // 
            // btnSectionMasterAuto
            // 
            this.btnSectionMasterAuto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSectionMasterAuto.BackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterAuto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSectionMasterAuto.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSectionMasterAuto.FlatAppearance.BorderSize = 0;
            this.btnSectionMasterAuto.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterAuto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterAuto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterAuto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSectionMasterAuto.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSectionMasterAuto.Image = global::AOG.Properties.Resources.SectionMasterOff;
            this.btnSectionMasterAuto.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSectionMasterAuto.Location = new System.Drawing.Point(3, 376);
            this.btnSectionMasterAuto.Margin = new System.Windows.Forms.Padding(0);
            this.btnSectionMasterAuto.Name = "btnSectionMasterAuto";
            this.btnSectionMasterAuto.Size = new System.Drawing.Size(64, 64);
            this.btnSectionMasterAuto.TabIndex = 152;
            this.btnSectionMasterAuto.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSectionMasterAuto.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSectionMasterAuto.UseVisualStyleBackColor = false;
            this.btnSectionMasterAuto.Click += new System.EventHandler(this.btnSectionMasterAuto_Click);
            // 
            // btnSectionMasterManual
            // 
            this.btnSectionMasterManual.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSectionMasterManual.BackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterManual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSectionMasterManual.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSectionMasterManual.FlatAppearance.BorderSize = 0;
            this.btnSectionMasterManual.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterManual.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterManual.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSectionMasterManual.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSectionMasterManual.Image = global::AOG.Properties.Resources.ManualOff;
            this.btnSectionMasterManual.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSectionMasterManual.Location = new System.Drawing.Point(3, 309);
            this.btnSectionMasterManual.Name = "btnSectionMasterManual";
            this.btnSectionMasterManual.Size = new System.Drawing.Size(64, 64);
            this.btnSectionMasterManual.TabIndex = 98;
            this.btnSectionMasterManual.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSectionMasterManual.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSectionMasterManual.UseVisualStyleBackColor = false;
            this.btnSectionMasterManual.Click += new System.EventHandler(this.btnSectionMasterManual_Click);
            // 
            // btnCycleLinesBk
            // 
            this.btnCycleLinesBk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCycleLinesBk.BackColor = System.Drawing.Color.Transparent;
            this.btnCycleLinesBk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCycleLinesBk.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnCycleLinesBk.FlatAppearance.BorderSize = 0;
            this.btnCycleLinesBk.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCycleLinesBk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCycleLinesBk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCycleLinesBk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycleLinesBk.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCycleLinesBk.Image = global::AOG.Properties.Resources.ABLineCycleBk;
            this.btnCycleLinesBk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCycleLinesBk.Location = new System.Drawing.Point(3, 239);
            this.btnCycleLinesBk.Name = "btnCycleLinesBk";
            this.btnCycleLinesBk.Size = new System.Drawing.Size(64, 64);
            this.btnCycleLinesBk.TabIndex = 252;
            this.btnCycleLinesBk.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCycleLinesBk.UseVisualStyleBackColor = false;
            this.btnCycleLinesBk.Click += new System.EventHandler(this.btnCycleLinesBk_Click);
            // 
            // btnCycleLines
            // 
            this.btnCycleLines.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCycleLines.BackColor = System.Drawing.Color.Transparent;
            this.btnCycleLines.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCycleLines.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnCycleLines.FlatAppearance.BorderSize = 0;
            this.btnCycleLines.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCycleLines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCycleLines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCycleLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycleLines.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCycleLines.Image = global::AOG.Properties.Resources.ABLineCycle;
            this.btnCycleLines.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCycleLines.Location = new System.Drawing.Point(3, 169);
            this.btnCycleLines.Name = "btnCycleLines";
            this.btnCycleLines.Size = new System.Drawing.Size(64, 64);
            this.btnCycleLines.TabIndex = 251;
            this.btnCycleLines.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCycleLines.UseVisualStyleBackColor = false;
            this.btnCycleLines.Click += new System.EventHandler(this.btnCycleLines_Click);
            // 
            // btnContour
            // 
            this.btnContour.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnContour.BackColor = System.Drawing.Color.Transparent;
            this.btnContour.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnContour.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnContour.FlatAppearance.BorderSize = 0;
            this.btnContour.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnContour.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnContour.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnContour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContour.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContour.Image = global::AOG.Properties.Resources.ContourOff;
            this.btnContour.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnContour.Location = new System.Drawing.Point(3, 99);
            this.btnContour.Name = "btnContour";
            this.btnContour.Size = new System.Drawing.Size(64, 64);
            this.btnContour.TabIndex = 105;
            this.btnContour.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnContour.UseVisualStyleBackColor = false;
            this.btnContour.Click += new System.EventHandler(this.btnContour_Click);
            // 
            // btnContourLock
            // 
            this.btnContourLock.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnContourLock.BackColor = System.Drawing.Color.Transparent;
            this.btnContourLock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnContourLock.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnContourLock.FlatAppearance.BorderSize = 0;
            this.btnContourLock.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnContourLock.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnContourLock.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnContourLock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContourLock.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContourLock.Image = global::AOG.Properties.Resources.ColorUnlocked;
            this.btnContourLock.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnContourLock.Location = new System.Drawing.Point(3, 29);
            this.btnContourLock.Name = "btnContourLock";
            this.btnContourLock.Size = new System.Drawing.Size(64, 64);
            this.btnContourLock.TabIndex = 542;
            this.btnContourLock.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnContourLock.UseVisualStyleBackColor = false;
            this.btnContourLock.Click += new System.EventHandler(this.btnContourLock_Click);
            // 
            // lblNumCu
            // 
            this.lblNumCu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblNumCu.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumCu.ForeColor = System.Drawing.Color.Black;
            this.lblNumCu.Location = new System.Drawing.Point(6, 0);
            this.lblNumCu.Margin = new System.Windows.Forms.Padding(0);
            this.lblNumCu.Name = "lblNumCu";
            this.lblNumCu.Size = new System.Drawing.Size(58, 26);
            this.lblNumCu.TabIndex = 542;
            this.lblNumCu.Text = "12/24";
            this.lblNumCu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNumCu.Visible = false;
            // 
            // panelControlBox
            // 
            this.panelControlBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControlBox.BackColor = System.Drawing.Color.Transparent;
            this.panelControlBox.Controls.Add(this.btnChargeStatus);
            this.panelControlBox.Controls.Add(this.btnGPSData);
            this.panelControlBox.Controls.Add(this.lblSpeed);
            this.panelControlBox.Controls.Add(this.btnShutdown);
            this.panelControlBox.Controls.Add(this.btnMaximizeMainForm);
            this.panelControlBox.Controls.Add(this.btnMinimizeMainForm);
            this.panelControlBox.Controls.Add(this.btnFieldStats);
            this.panelControlBox.Location = new System.Drawing.Point(514, 4);
            this.panelControlBox.Name = "panelControlBox";
            this.panelControlBox.Size = new System.Drawing.Size(489, 43);
            this.panelControlBox.TabIndex = 542;
            // 
            // btnChargeStatus
            // 
            this.btnChargeStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChargeStatus.BackColor = System.Drawing.Color.Silver;
            this.btnChargeStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnChargeStatus.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnChargeStatus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChargeStatus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChargeStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChargeStatus.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChargeStatus.Image = global::AOG.Properties.Resources.ChargeIndicator;
            this.btnChargeStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChargeStatus.Location = new System.Drawing.Point(86, 5);
            this.btnChargeStatus.Name = "btnChargeStatus";
            this.btnChargeStatus.Size = new System.Drawing.Size(25, 32);
            this.btnChargeStatus.TabIndex = 537;
            this.btnChargeStatus.UseVisualStyleBackColor = false;
            // 
            // btnGPSData
            // 
            this.btnGPSData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGPSData.BackColor = System.Drawing.Color.Yellow;
            this.btnGPSData.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnGPSData.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnGPSData.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGPSData.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGPSData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGPSData.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGPSData.Image = global::AOG.Properties.Resources.GPSQuality;
            this.btnGPSData.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGPSData.Location = new System.Drawing.Point(124, 2);
            this.btnGPSData.Name = "btnGPSData";
            this.btnGPSData.Size = new System.Drawing.Size(52, 38);
            this.btnGPSData.TabIndex = 536;
            this.btnGPSData.UseVisualStyleBackColor = false;
            this.btnGPSData.Click += new System.EventHandler(this.btnGPSData_Click);
            // 
            // btnShutdown
            // 
            this.btnShutdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShutdown.BackColor = System.Drawing.Color.Transparent;
            this.btnShutdown.BackgroundImage = global::AOG.Properties.Resources.WindowClose;
            this.btnShutdown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnShutdown.FlatAppearance.BorderSize = 0;
            this.btnShutdown.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShutdown.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShutdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShutdown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnShutdown.Location = new System.Drawing.Point(434, 3);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(52, 38);
            this.btnShutdown.TabIndex = 447;
            this.btnShutdown.UseVisualStyleBackColor = false;
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // btnMaximizeMainForm
            // 
            this.btnMaximizeMainForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximizeMainForm.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximizeMainForm.BackgroundImage = global::AOG.Properties.Resources.WindowMaximize;
            this.btnMaximizeMainForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnMaximizeMainForm.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnMaximizeMainForm.FlatAppearance.BorderSize = 0;
            this.btnMaximizeMainForm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMaximizeMainForm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMaximizeMainForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximizeMainForm.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaximizeMainForm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMaximizeMainForm.Location = new System.Drawing.Point(361, 3);
            this.btnMaximizeMainForm.Name = "btnMaximizeMainForm";
            this.btnMaximizeMainForm.Size = new System.Drawing.Size(52, 38);
            this.btnMaximizeMainForm.TabIndex = 482;
            this.btnMaximizeMainForm.UseVisualStyleBackColor = false;
            this.btnMaximizeMainForm.Click += new System.EventHandler(this.btnMaximizeMainForm_Click);
            // 
            // btnMinimizeMainForm
            // 
            this.btnMinimizeMainForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizeMainForm.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimizeMainForm.BackgroundImage = global::AOG.Properties.Resources.WindowMinimize;
            this.btnMinimizeMainForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMinimizeMainForm.FlatAppearance.BorderSize = 0;
            this.btnMinimizeMainForm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMinimizeMainForm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMinimizeMainForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizeMainForm.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimizeMainForm.ForeColor = System.Drawing.Color.DimGray;
            this.btnMinimizeMainForm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMinimizeMainForm.Location = new System.Drawing.Point(290, 3);
            this.btnMinimizeMainForm.Name = "btnMinimizeMainForm";
            this.btnMinimizeMainForm.Size = new System.Drawing.Size(52, 38);
            this.btnMinimizeMainForm.TabIndex = 481;
            this.btnMinimizeMainForm.UseVisualStyleBackColor = false;
            this.btnMinimizeMainForm.Click += new System.EventHandler(this.btnMinimizeMainForm_Click);
            // 
            // btnFieldStats
            // 
            this.btnFieldStats.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFieldStats.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldStats.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFieldStats.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnFieldStats.FlatAppearance.BorderSize = 0;
            this.btnFieldStats.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnFieldStats.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFieldStats.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFieldStats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldStats.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFieldStats.Image = global::AOG.Properties.Resources.FieldStats;
            this.btnFieldStats.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFieldStats.Location = new System.Drawing.Point(2, 2);
            this.btnFieldStats.Name = "btnFieldStats";
            this.btnFieldStats.Size = new System.Drawing.Size(75, 38);
            this.btnFieldStats.TabIndex = 535;
            this.btnFieldStats.UseVisualStyleBackColor = false;
            this.btnFieldStats.Visible = false;
            this.btnFieldStats.Click += new System.EventHandler(this.btnFieldStats_Click);
            // 
            // lblHardwareMessage
            // 
            this.lblHardwareMessage.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblHardwareMessage.BackColor = System.Drawing.Color.Bisque;
            this.lblHardwareMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblHardwareMessage.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHardwareMessage.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblHardwareMessage.Location = new System.Drawing.Point(65, 87);
            this.lblHardwareMessage.Name = "lblHardwareMessage";
            this.lblHardwareMessage.Size = new System.Drawing.Size(888, 43);
            this.lblHardwareMessage.TabIndex = 543;
            this.lblHardwareMessage.Text = "This message is very long and boring no one should read 01234";
            this.lblHardwareMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHardwareMessage.Visible = false;
            this.lblHardwareMessage.Click += new System.EventHandler(this.lblHardwareMessage_Click);
            // 
            // tlpNozzle
            // 
            this.tlpNozzle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tlpNozzle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(60)))));
            this.tlpNozzle.ColumnCount = 3;
            this.tlpNozzle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tlpNozzle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.625F));
            this.tlpNozzle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.375F));
            this.tlpNozzle.Controls.Add(this.btnSprayPSI, 1, 1);
            this.tlpNozzle.Controls.Add(this.lblPressure, 1, 2);
            this.tlpNozzle.Controls.Add(this.btnSprayVolumeTotal, 1, 3);
            this.tlpNozzle.Controls.Add(this.lbl_Volume, 1, 4);
            this.tlpNozzle.Controls.Add(this.btnSprayGalPerAcre, 1, 5);
            this.tlpNozzle.Controls.Add(this.cboxSprayAutoManual, 1, 9);
            this.tlpNozzle.Controls.Add(this.cboxRate1Rate2Select, 1, 7);
            this.tlpNozzle.Controls.Add(this.btnSprayRateUp, 2, 8);
            this.tlpNozzle.Controls.Add(this.btnSprayRateDn, 1, 8);
            this.tlpNozzle.Controls.Add(this.btnNozConfig, 1, 10);
            this.tlpNozzle.Controls.Add(this.lblIsFlowing, 0, 3);
            this.tlpNozzle.Controls.Add(this.lblGPM_Set, 1, 6);
            this.tlpNozzle.Controls.Add(this.btnSprayGalPerMinActual, 2, 6);
            this.tlpNozzle.Controls.Add(this.lblPWM_Nozz, 1, 0);
            this.tlpNozzle.Controls.Add(this.lblFlowHz_Nozz, 2, 0);
            this.tlpNozzle.Location = new System.Drawing.Point(75, 50);
            this.tlpNozzle.Name = "tlpNozzle";
            this.tlpNozzle.RowCount = 11;
            this.tlpNozzle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.17553F));
            this.tlpNozzle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.734513F));
            this.tlpNozzle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.840708F));
            this.tlpNozzle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.380531F));
            this.tlpNozzle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.309734F));
            this.tlpNozzle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.893993F));
            this.tlpNozzle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tlpNozzle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.94276F));
            this.tlpNozzle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.68687F));
            this.tlpNozzle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.31096F));
            this.tlpNozzle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.31096F));
            this.tlpNozzle.Size = new System.Drawing.Size(175, 631);
            this.tlpNozzle.TabIndex = 545;
            this.tlpNozzle.Visible = false;
            // 
            // btnSprayPSI
            // 
            this.btnSprayPSI.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSprayPSI.AutoSize = true;
            this.btnSprayPSI.BackColor = System.Drawing.Color.Transparent;
            this.btnSprayPSI.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tlpNozzle.SetColumnSpan(this.btnSprayPSI, 2);
            this.btnSprayPSI.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnSprayPSI.FlatAppearance.BorderSize = 0;
            this.btnSprayPSI.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSprayPSI.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSprayPSI.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSprayPSI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSprayPSI.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSprayPSI.ForeColor = System.Drawing.Color.Yellow;
            this.btnSprayPSI.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSprayPSI.Location = new System.Drawing.Point(30, 33);
            this.btnSprayPSI.Name = "btnSprayPSI";
            this.btnSprayPSI.Size = new System.Drawing.Size(126, 51);
            this.btnSprayPSI.TabIndex = 554;
            this.btnSprayPSI.Text = "10";
            this.btnSprayPSI.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSprayPSI.UseVisualStyleBackColor = false;
            // 
            // lblPressure
            // 
            this.lblPressure.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPressure.BackColor = System.Drawing.Color.Transparent;
            this.tlpNozzle.SetColumnSpan(this.lblPressure, 2);
            this.lblPressure.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPressure.ForeColor = System.Drawing.Color.White;
            this.lblPressure.Location = new System.Drawing.Point(30, 87);
            this.lblPressure.Name = "lblPressure";
            this.lblPressure.Size = new System.Drawing.Size(126, 19);
            this.lblPressure.TabIndex = 553;
            this.lblPressure.Text = "Pressure";
            this.lblPressure.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnSprayVolumeTotal
            // 
            this.btnSprayVolumeTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSprayVolumeTotal.BackColor = System.Drawing.Color.Transparent;
            this.btnSprayVolumeTotal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tlpNozzle.SetColumnSpan(this.btnSprayVolumeTotal, 2);
            this.btnSprayVolumeTotal.FlatAppearance.BorderColor = System.Drawing.Color.Olive;
            this.btnSprayVolumeTotal.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSprayVolumeTotal.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSprayVolumeTotal.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSprayVolumeTotal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSprayVolumeTotal.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSprayVolumeTotal.ForeColor = System.Drawing.Color.Aqua;
            this.btnSprayVolumeTotal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSprayVolumeTotal.Location = new System.Drawing.Point(12, 124);
            this.btnSprayVolumeTotal.Margin = new System.Windows.Forms.Padding(0);
            this.btnSprayVolumeTotal.Name = "btnSprayVolumeTotal";
            this.btnSprayVolumeTotal.Size = new System.Drawing.Size(163, 52);
            this.btnSprayVolumeTotal.TabIndex = 552;
            this.btnSprayVolumeTotal.Text = "9999.9";
            this.btnSprayVolumeTotal.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSprayVolumeTotal.UseVisualStyleBackColor = false;
            this.btnSprayVolumeTotal.Click += new System.EventHandler(this.btnSprayVolumeTotal_Click);
            // 
            // lbl_Volume
            // 
            this.lbl_Volume.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbl_Volume.BackColor = System.Drawing.Color.Transparent;
            this.tlpNozzle.SetColumnSpan(this.lbl_Volume, 2);
            this.lbl_Volume.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Volume.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.lbl_Volume.Location = new System.Drawing.Point(30, 176);
            this.lbl_Volume.Name = "lbl_Volume";
            this.lbl_Volume.Size = new System.Drawing.Size(126, 24);
            this.lbl_Volume.TabIndex = 551;
            this.lbl_Volume.Text = "Tank Gallons";
            this.lbl_Volume.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnSprayGalPerAcre
            // 
            this.btnSprayGalPerAcre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSprayGalPerAcre.BackColor = System.Drawing.Color.Transparent;
            this.btnSprayGalPerAcre.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tlpNozzle.SetColumnSpan(this.btnSprayGalPerAcre, 2);
            this.btnSprayGalPerAcre.FlatAppearance.BorderColor = System.Drawing.Color.Olive;
            this.btnSprayGalPerAcre.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSprayGalPerAcre.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSprayGalPerAcre.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSprayGalPerAcre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSprayGalPerAcre.Font = new System.Drawing.Font("Tahoma", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSprayGalPerAcre.ForeColor = System.Drawing.Color.White;
            this.btnSprayGalPerAcre.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSprayGalPerAcre.Location = new System.Drawing.Point(12, 211);
            this.btnSprayGalPerAcre.Margin = new System.Windows.Forms.Padding(0);
            this.btnSprayGalPerAcre.Name = "btnSprayGalPerAcre";
            this.btnSprayGalPerAcre.Size = new System.Drawing.Size(163, 54);
            this.btnSprayGalPerAcre.TabIndex = 546;
            this.btnSprayGalPerAcre.Text = "999.9";
            this.btnSprayGalPerAcre.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSprayGalPerAcre.UseVisualStyleBackColor = false;
            this.btnSprayGalPerAcre.Click += new System.EventHandler(this.btnSprayRate_Click);
            // 
            // cboxSprayAutoManual
            // 
            this.cboxSprayAutoManual.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxSprayAutoManual.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxSprayAutoManual.BackColor = System.Drawing.Color.DarkRed;
            this.cboxSprayAutoManual.Checked = true;
            this.cboxSprayAutoManual.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tlpNozzle.SetColumnSpan(this.cboxSprayAutoManual, 2);
            this.cboxSprayAutoManual.FlatAppearance.BorderColor = System.Drawing.Color.Olive;
            this.cboxSprayAutoManual.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(50)))), ((int)(((byte)(20)))));
            this.cboxSprayAutoManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxSprayAutoManual.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxSprayAutoManual.ForeColor = System.Drawing.Color.White;
            this.cboxSprayAutoManual.Location = new System.Drawing.Point(30, 493);
            this.cboxSprayAutoManual.Name = "cboxSprayAutoManual";
            this.cboxSprayAutoManual.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxSprayAutoManual.Size = new System.Drawing.Size(126, 55);
            this.cboxSprayAutoManual.TabIndex = 582;
            this.cboxSprayAutoManual.Text = "Auto";
            this.cboxSprayAutoManual.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxSprayAutoManual.UseVisualStyleBackColor = false;
            this.cboxSprayAutoManual.Click += new System.EventHandler(this.cboxSprayAutoManual_Click);
            // 
            // cboxRate1Rate2Select
            // 
            this.cboxRate1Rate2Select.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxRate1Rate2Select.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxRate1Rate2Select.BackColor = System.Drawing.Color.Transparent;
            this.tlpNozzle.SetColumnSpan(this.cboxRate1Rate2Select, 2);
            this.cboxRate1Rate2Select.FlatAppearance.BorderColor = System.Drawing.Color.Olive;
            this.cboxRate1Rate2Select.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cboxRate1Rate2Select.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cboxRate1Rate2Select.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cboxRate1Rate2Select.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxRate1Rate2Select.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxRate1Rate2Select.ForeColor = System.Drawing.Color.White;
            this.cboxRate1Rate2Select.Location = new System.Drawing.Point(12, 327);
            this.cboxRate1Rate2Select.Margin = new System.Windows.Forms.Padding(0);
            this.cboxRate1Rate2Select.Name = "cboxRate1Rate2Select";
            this.cboxRate1Rate2Select.Size = new System.Drawing.Size(163, 49);
            this.cboxRate1Rate2Select.TabIndex = 602;
            this.cboxRate1Rate2Select.Text = "Rate: 1";
            this.cboxRate1Rate2Select.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxRate1Rate2Select.UseVisualStyleBackColor = false;
            this.cboxRate1Rate2Select.Click += new System.EventHandler(this.cboxRate1Rate2Select_Click);
            // 
            // btnSprayRateUp
            // 
            this.btnSprayRateUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSprayRateUp.BackColor = System.Drawing.Color.Transparent;
            this.btnSprayRateUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSprayRateUp.FlatAppearance.BorderColor = System.Drawing.Color.Olive;
            this.btnSprayRateUp.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSprayRateUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PowderBlue;
            this.btnSprayRateUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSprayRateUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSprayRateUp.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSprayRateUp.ForeColor = System.Drawing.Color.Black;
            this.btnSprayRateUp.Image = global::AOG.Properties.Resources.UpArrow64;
            this.btnSprayRateUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSprayRateUp.Location = new System.Drawing.Point(103, 399);
            this.btnSprayRateUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnSprayRateUp.Name = "btnSprayRateUp";
            this.btnSprayRateUp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSprayRateUp.Size = new System.Drawing.Size(62, 62);
            this.btnSprayRateUp.TabIndex = 615;
            this.btnSprayRateUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSprayRateUp.UseVisualStyleBackColor = false;
            this.btnSprayRateUp.Click += new System.EventHandler(this.btnSprayRateUPGN_Click);
            // 
            // btnSprayRateDn
            // 
            this.btnSprayRateDn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSprayRateDn.BackColor = System.Drawing.Color.Transparent;
            this.btnSprayRateDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSprayRateDn.FlatAppearance.BorderColor = System.Drawing.Color.Olive;
            this.btnSprayRateDn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSprayRateDn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PowderBlue;
            this.btnSprayRateDn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSprayRateDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSprayRateDn.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSprayRateDn.ForeColor = System.Drawing.Color.Black;
            this.btnSprayRateDn.Image = global::AOG.Properties.Resources.DnArrow64;
            this.btnSprayRateDn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSprayRateDn.Location = new System.Drawing.Point(22, 399);
            this.btnSprayRateDn.Margin = new System.Windows.Forms.Padding(0);
            this.btnSprayRateDn.Name = "btnSprayRateDn";
            this.btnSprayRateDn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSprayRateDn.Size = new System.Drawing.Size(62, 62);
            this.btnSprayRateDn.TabIndex = 616;
            this.btnSprayRateDn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSprayRateDn.UseVisualStyleBackColor = false;
            this.btnSprayRateDn.Click += new System.EventHandler(this.btnSprayRateDn_Click);
            // 
            // btnNozConfig
            // 
            this.btnNozConfig.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnNozConfig.BackColor = System.Drawing.Color.Transparent;
            this.btnNozConfig.BackgroundImage = global::AOG.Properties.Resources.SpecialFunctions;
            this.btnNozConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlpNozzle.SetColumnSpan(this.btnNozConfig, 2);
            this.btnNozConfig.FlatAppearance.BorderColor = System.Drawing.Color.Olive;
            this.btnNozConfig.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNozConfig.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNozConfig.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNozConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNozConfig.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNozConfig.ForeColor = System.Drawing.Color.Aqua;
            this.btnNozConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNozConfig.Location = new System.Drawing.Point(51, 570);
            this.btnNozConfig.Name = "btnNozConfig";
            this.btnNozConfig.Size = new System.Drawing.Size(84, 48);
            this.btnNozConfig.TabIndex = 553;
            this.btnNozConfig.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNozConfig.UseVisualStyleBackColor = false;
            this.btnNozConfig.Click += new System.EventHandler(this.btnNozConfig_Click);
            // 
            // lblIsFlowing
            // 
            this.lblIsFlowing.AutoSize = true;
            this.lblIsFlowing.BackColor = System.Drawing.Color.Orange;
            this.lblIsFlowing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIsFlowing.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIsFlowing.Location = new System.Drawing.Point(3, 121);
            this.lblIsFlowing.Name = "lblIsFlowing";
            this.tlpNozzle.SetRowSpan(this.lblIsFlowing, 5);
            this.lblIsFlowing.Size = new System.Drawing.Size(6, 255);
            this.lblIsFlowing.TabIndex = 617;
            this.lblIsFlowing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGPM_Set
            // 
            this.lblGPM_Set.BackColor = System.Drawing.Color.Transparent;
            this.lblGPM_Set.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGPM_Set.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGPM_Set.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(220)))), ((int)(((byte)(70)))));
            this.lblGPM_Set.Location = new System.Drawing.Point(12, 265);
            this.lblGPM_Set.Margin = new System.Windows.Forms.Padding(0);
            this.lblGPM_Set.Name = "lblGPM_Set";
            this.lblGPM_Set.Size = new System.Drawing.Size(82, 47);
            this.lblGPM_Set.TabIndex = 612;
            this.lblGPM_Set.Text = "Flow";
            this.lblGPM_Set.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSprayGalPerMinActual
            // 
            this.btnSprayGalPerMinActual.BackColor = System.Drawing.Color.Transparent;
            this.btnSprayGalPerMinActual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSprayGalPerMinActual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSprayGalPerMinActual.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnSprayGalPerMinActual.FlatAppearance.BorderSize = 0;
            this.btnSprayGalPerMinActual.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSprayGalPerMinActual.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSprayGalPerMinActual.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSprayGalPerMinActual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSprayGalPerMinActual.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSprayGalPerMinActual.ForeColor = System.Drawing.Color.Yellow;
            this.btnSprayGalPerMinActual.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSprayGalPerMinActual.Location = new System.Drawing.Point(94, 265);
            this.btnSprayGalPerMinActual.Margin = new System.Windows.Forms.Padding(0);
            this.btnSprayGalPerMinActual.Name = "btnSprayGalPerMinActual";
            this.btnSprayGalPerMinActual.Size = new System.Drawing.Size(81, 47);
            this.btnSprayGalPerMinActual.TabIndex = 613;
            this.btnSprayGalPerMinActual.Text = "10.9";
            this.btnSprayGalPerMinActual.UseVisualStyleBackColor = false;
            // 
            // lblPWM_Nozz
            // 
            this.lblPWM_Nozz.BackColor = System.Drawing.Color.Transparent;
            this.lblPWM_Nozz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPWM_Nozz.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPWM_Nozz.ForeColor = System.Drawing.Color.White;
            this.lblPWM_Nozz.Location = new System.Drawing.Point(12, 0);
            this.lblPWM_Nozz.Margin = new System.Windows.Forms.Padding(0);
            this.lblPWM_Nozz.Name = "lblPWM_Nozz";
            this.lblPWM_Nozz.Size = new System.Drawing.Size(82, 30);
            this.lblPWM_Nozz.TabIndex = 618;
            this.lblPWM_Nozz.Text = "PWM";
            this.lblPWM_Nozz.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFlowHz_Nozz
            // 
            this.lblFlowHz_Nozz.BackColor = System.Drawing.Color.Transparent;
            this.lblFlowHz_Nozz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFlowHz_Nozz.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlowHz_Nozz.ForeColor = System.Drawing.Color.White;
            this.lblFlowHz_Nozz.Location = new System.Drawing.Point(94, 0);
            this.lblFlowHz_Nozz.Margin = new System.Windows.Forms.Padding(0);
            this.lblFlowHz_Nozz.Name = "lblFlowHz_Nozz";
            this.lblFlowHz_Nozz.Size = new System.Drawing.Size(81, 30);
            this.lblFlowHz_Nozz.TabIndex = 619;
            this.lblFlowHz_Nozz.Text = "Hz";
            this.lblFlowHz_Nozz.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layersToolStripMenuItem
            // 
            this.layersToolStripMenuItem.Name = "layersToolStripMenuItem";
            this.layersToolStripMenuItem.Size = new System.Drawing.Size(450, 70);
            this.layersToolStripMenuItem.Text = "Layers";
            this.layersToolStripMenuItem.Click += new System.EventHandler(this.layersToolStripMenuItem_Click);
            // 
            // FormGPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1006, 751);
            this.Controls.Add(this.tlpNozzle);
            this.Controls.Add(this.panelControlBox);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.flp1);
            this.Controls.Add(this.panelSim);
            this.Controls.Add(this.lblFix);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelNavigation);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.oglMain);
            this.Controls.Add(this.oglZoom);
            this.Controls.Add(this.lblCurrentField);
            this.Controls.Add(this.lblGuidanceLine);
            this.Controls.Add(this.lblHardwareMessage);
            this.Controls.Add(this.oglBack);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 720);
            this.Name = "FormGPS";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "AOG";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGPS_FormClosing);
            this.Load += new System.EventHandler(this.FormGPS_Load);
            this.ResizeEnd += new System.EventHandler(this.FormGPS_ResizeEnd);
            this.Move += new System.EventHandler(this.FormGPS_Move);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStripFlag.ResumeLayout(false);
            this.statusStripLeft.ResumeLayout(false);
            this.statusStripLeft.PerformLayout();
            this.panelSim.ResumeLayout(false);
            this.panelNavigation.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.flp1.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelControlBox.ResumeLayout(false);
            this.tlpNozzle.ResumeLayout(false);
            this.tlpNozzle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Timer tmrWatchdog;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFlag;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFlagRed;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuFlagGrn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuFlagYel;
        private System.Windows.Forms.Button btnFlag;
        private System.Windows.Forms.Button btnResetSteerAngle;
        private System.Windows.Forms.Button btnResetSim;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        public System.Windows.Forms.Button btnSectionMasterAuto;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageEnglish;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageDeutsch;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageRussian;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageDutch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageSpanish;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageFrench;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageItalian;
        public System.Windows.Forms.Button btnTrack;
        private System.Windows.Forms.ToolStripMenuItem enterSimCoordsToolStripMenuItem;
        public System.Windows.Forms.Button btnAutoYouTurn;
        public System.Windows.Forms.Button btnAutoSteer;
        private System.Windows.Forms.HScrollBar hsbarSteerAngle;
        private OpenTK.GLControl oglZoom;
        private OpenTK.GLControl oglMain;
        private OpenTK.GLControl oglBack;
        private System.Windows.Forms.ComboBox cboxpRowWidth;
        private System.Windows.Forms.Label lblHz;
        public System.Windows.Forms.Button btnContour;
        public System.Windows.Forms.Timer timerSim;
        public System.Windows.Forms.Button btnSectionMasterManual;
        public System.Windows.Forms.Button btnABDraw;
        public System.Windows.Forms.ToolStripMenuItem menustripLanguage;
        public System.Windows.Forms.Button btnCycleLines;
        private System.Windows.Forms.StatusStrip statusStripLeft;
        private System.Windows.Forms.TableLayoutPanel panelSim;
        private System.Windows.Forms.Button btnHeadlandOnOff;
        private System.Windows.Forms.Button btnShutdown;
        private System.Windows.Forms.Button btnSimSetSpeedToZero;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageUkranian;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageSlovak;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuFlagForm;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageTest;
        public System.Windows.Forms.Button btnHydLift;
        private System.Windows.Forms.ToolStripMenuItem menuLanguagePolish;
        private System.Windows.Forms.ToolStripDropDownButton distanceToolBtn;
        public System.Windows.Forms.Button btnDayNightMode;
        public System.Windows.Forms.Button btnStartAgIO;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton4;
        public System.Windows.Forms.ToolStripMenuItem steerChartStripMenu;
        private System.Windows.Forms.ToolStripMenuItem webcamToolStrip;
        private System.Windows.Forms.ToolStripMenuItem offsetFixToolStrip;
        private System.Windows.Forms.TableLayoutPanel panelNavigation;
        public System.Windows.Forms.Button btn2D;
        public System.Windows.Forms.Button btn3D;
        private System.Windows.Forms.ToolStripMenuItem deleteContourPathsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SmoothABtoolStripMenu;
        public System.Windows.Forms.Button btnAutoSteerConfig;
        public System.Windows.Forms.Button btnChangeMappingColor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem simulatorOnToolStripMenuItem;
        private System.Windows.Forms.Button btnMinimizeMainForm;
        private System.Windows.Forms.Button btnMaximizeMainForm;
        private System.Windows.Forms.ToolStripMenuItem resetALLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetEverythingToolStripMenuItem;
        public System.Windows.Forms.Button btnTramDisplayMode;
        public System.Windows.Forms.Button btnYouSkipEnable;
        private System.Windows.Forms.Label lblFix;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageDanish;
        private System.Windows.Forms.Button btnNudge;
        public System.Windows.Forms.Button btnResetToolHeading;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageTurkish;
        private System.Windows.Forms.ToolStripMenuItem wizardsMenu;
        private System.Windows.Forms.ToolStripMenuItem steerWizardMenuItem;
        public System.Windows.Forms.Button btnBrightnessDn;
        public System.Windows.Forms.Button btnBrightnessUp;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageHungarian;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageFinnish;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageLatvian;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageLithuanian;
        private System.Windows.Forms.ToolStripMenuItem rollCheckToolStripMenuItem;
        public System.Windows.Forms.Button btnCycleLinesBk;
        private System.Windows.Forms.TableLayoutPanel panelLeft;
        private RepeatButton btnSimSpeedUp;
        private RepeatButton btnSpeedDn;
        private System.Windows.Forms.Label lblCurrentField;
        private System.Windows.Forms.Button btnFieldStats;
        public System.Windows.Forms.Button btnFieldMenu;
        private System.Windows.Forms.Button btnGPSData;
        private System.Windows.Forms.Button btnSimReverseDirection;
        public System.Windows.Forms.Button btnBuildTracks;
        private System.Windows.Forms.Button btnRefNudge;
        private System.Windows.Forms.Label lblGuidanceLine;
        private System.Windows.Forms.Button btnTracksOff;
        private System.Windows.Forms.FlowLayoutPanel flp1;
        private System.Windows.Forms.FlowLayoutPanel panelBottom;
        public System.Windows.Forms.Button btnContourLock;
        public System.Windows.Forms.FlowLayoutPanel panelRight;
        internal System.Windows.Forms.Label lblNumCu;
        public System.Windows.Forms.Button btnNavigationSettings;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem boundariesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem headlandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem headlandBuildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flagByLatLonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recordedPathStripMenu;
        private System.Windows.Forms.Panel panelControlBox;
        public System.Windows.Forms.Button btnTiltDn;
        public System.Windows.Forms.Button btnTiltUp;
        private System.Windows.Forms.ToolStripMenuItem boundaryToolToolStripMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteAppliedToolStripMenuItem;
        public System.Windows.Forms.Button btnPlusAB;
        private System.Windows.Forms.CheckBox cboxIsSectionControlled;
        public System.Windows.Forms.Button btnSnapToPivot;
        private System.Windows.Forms.Button btnAdjLeft;
        private System.Windows.Forms.Button btnAdjRight;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageChinese;
        public System.Windows.Forms.Button btnGrid;
        public System.Windows.Forms.Button btnN2D;
        private System.Windows.Forms.ToolStripMenuItem kioskModeToolStrip;
        private System.Windows.Forms.ToolStripMenuItem menuLanguagePortugese;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem toolStripConfig;
        private System.Windows.Forms.ToolStripMenuItem toolStripSteerSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripAllSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripWorkingDirectories;
        private System.Windows.Forms.ToolStripMenuItem toolStripGPSData;
        private System.Windows.Forms.ToolStripMenuItem toolStripColors;
        private System.Windows.Forms.ToolStripMenuItem toolStripSectionColors;
        private System.Windows.Forms.ToolStripMenuItem toolStripHotkeys;
        private System.Windows.Forms.Button btnChargeStatus;
        private System.Windows.Forms.Label lblHardwareMessage;
        private System.Windows.Forms.ToolStripMenuItem eventViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guidelinesToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tlpNozzle;
        public System.Windows.Forms.Button btnSprayGalPerMinActual;
        private System.Windows.Forms.Label lblGPM_Set;
        public System.Windows.Forms.Button btnSprayPSI;
        private System.Windows.Forms.Label lblPressure;
        public System.Windows.Forms.Button btnSprayVolumeTotal;
        public System.Windows.Forms.Label lbl_Volume;
        public System.Windows.Forms.Button btnSprayGalPerAcre;
        private System.Windows.Forms.CheckBox cboxSprayAutoManual;
        private System.Windows.Forms.CheckBox cboxRate1Rate2Select;
        public System.Windows.Forms.Button btnSprayRateUp;
        public System.Windows.Forms.Button btnSprayRateDn;
        public System.Windows.Forms.Button btnNozConfig;
        private System.Windows.Forms.ToolStripMenuItem nozzleAppToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tramsMultiMenuField;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageSerbian;
        private System.Windows.Forms.ToolStripMenuItem menuGenerateLanguageReference;
        private System.Windows.Forms.Label lblIsFlowing;
        public System.Windows.Forms.ToolStripDropDownButton toolStripBtnFieldTools;
        private System.Windows.Forms.Label lblPWM_Nozz;
        private System.Windows.Forms.Label lblFlowHz_Nozz;
        private System.Windows.Forms.ToolStripMenuItem toolSteerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem agShareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layersToolStripMenuItem;
    }
}

