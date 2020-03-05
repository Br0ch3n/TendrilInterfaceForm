namespace TendrilInterfaceForm
{
    partial class TendrilInterface
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.comPortTxtLabel = new System.Windows.Forms.Label();
            this.brLabel = new System.Windows.Forms.Label();
            this.tbComPort = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphTensionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trackingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kalmanFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simTxtLabel = new System.Windows.Forms.Label();
            this.simCurrTxLabel = new System.Windows.Forms.Label();
            this.connectLabel = new System.Windows.Forms.Label();
            this.cntTxtLabel = new System.Windows.Forms.Label();
            this.tenTxtLabel = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.cntStatusLabel = new System.Windows.Forms.Label();
            this.tenStatusLabel = new System.Windows.Forms.Label();
            this.btSerialPort = new System.IO.Ports.SerialPort(this.components);
            this.btnRun = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.tenProgressBar7 = new System.Windows.Forms.ProgressBar();
            this.tenProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.tenProgressBar2 = new System.Windows.Forms.ProgressBar();
            this.tenProgressBar3 = new System.Windows.Forms.ProgressBar();
            this.tenProgressBar4 = new System.Windows.Forms.ProgressBar();
            this.tenProgressBar5 = new System.Windows.Forms.ProgressBar();
            this.tenProgressBar6 = new System.Windows.Forms.ProgressBar();
            this.tenProgressBar0 = new System.Windows.Forms.ProgressBar();
            this.tenProgressBar8 = new System.Windows.Forms.ProgressBar();
            this.labTenMot0 = new System.Windows.Forms.Label();
            this.labTenMot3 = new System.Windows.Forms.Label();
            this.labTenMot4 = new System.Windows.Forms.Label();
            this.labTenMot1 = new System.Windows.Forms.Label();
            this.labTenMot2 = new System.Windows.Forms.Label();
            this.labTenMot5 = new System.Windows.Forms.Label();
            this.labTenMot6 = new System.Windows.Forms.Label();
            this.labTenMot7 = new System.Windows.Forms.Label();
            this.labTenMot8 = new System.Windows.Forms.Label();
            this.pnlMnlControl = new System.Windows.Forms.Panel();
            this.btnMnlClose = new System.Windows.Forms.Button();
            this.btMnlSend = new System.Windows.Forms.Button();
            this.ddmMnlCntrlSelect = new System.Windows.Forms.ComboBox();
            this.tbMnlMot8 = new System.Windows.Forms.TextBox();
            this.tbMnlMot7 = new System.Windows.Forms.TextBox();
            this.tbMnlMot6 = new System.Windows.Forms.TextBox();
            this.tbMnlMot5 = new System.Windows.Forms.TextBox();
            this.tbMnlMot4 = new System.Windows.Forms.TextBox();
            this.tbMnlMot3 = new System.Windows.Forms.TextBox();
            this.tbMnlMot2 = new System.Windows.Forms.TextBox();
            this.tbMnlMot1 = new System.Windows.Forms.TextBox();
            this.lblMnlMot8 = new System.Windows.Forms.Label();
            this.lblMnlMot7 = new System.Windows.Forms.Label();
            this.lblMnlMot6 = new System.Windows.Forms.Label();
            this.lblMnlMot5 = new System.Windows.Forms.Label();
            this.lblMnlMot4 = new System.Windows.Forms.Label();
            this.lblMnlMot3 = new System.Windows.Forms.Label();
            this.lblMnlMot2 = new System.Windows.Forms.Label();
            this.lblMnlMot1 = new System.Windows.Forms.Label();
            this.lblMnlMot0 = new System.Windows.Forms.Label();
            this.lblMnl = new System.Windows.Forms.Label();
            this.tbMnlMot0 = new System.Windows.Forms.TextBox();
            this.ddmSectionSelect = new System.Windows.Forms.ComboBox();
            this.btnEvenTension = new System.Windows.Forms.Button();
            this.btnStopEven = new System.Windows.Forms.Button();
            this.pnlCSVWriter = new System.Windows.Forms.Panel();
            this.btnCSVWriterEnd = new System.Windows.Forms.Button();
            this.btnCSVWriterBegin = new System.Windows.Forms.Button();
            this.lblCSVWriter = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.pnlMnlControl.SuspendLayout();
            this.pnlCSVWriter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(342, 24);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnConnect_MouseClick);
            // 
            // comPortTxtLabel
            // 
            this.comPortTxtLabel.AutoSize = true;
            this.comPortTxtLabel.Location = new System.Drawing.Point(11, 27);
            this.comPortTxtLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.comPortTxtLabel.Name = "comPortTxtLabel";
            this.comPortTxtLabel.Size = new System.Drawing.Size(164, 13);
            this.comPortTxtLabel.TabIndex = 1;
            this.comPortTxtLabel.Text = "BlueTooth COM Port (i.e. COM8):\r\n";
            // 
            // brLabel
            // 
            this.brLabel.AutoSize = true;
            this.brLabel.Location = new System.Drawing.Point(259, 28);
            this.brLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.brLabel.Name = "brLabel";
            this.brLabel.Size = new System.Drawing.Size(80, 13);
            this.brLabel.TabIndex = 2;
            this.brLabel.Text = "Speed: 115200\r\n";
            // 
            // tbComPort
            // 
            this.tbComPort.Location = new System.Drawing.Point(179, 24);
            this.tbComPort.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbComPort.Name = "tbComPort";
            this.tbComPort.Size = new System.Drawing.Size(76, 20);
            this.tbComPort.TabIndex = 3;
            this.tbComPort.Text = "COM4";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.manualControlToolStripMenuItem,
            this.displayToolStripMenuItem,
            this.trackingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(820, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveOutputToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveOutputToolStripMenuItem
            // 
            this.saveOutputToolStripMenuItem.Name = "saveOutputToolStripMenuItem";
            this.saveOutputToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.saveOutputToolStripMenuItem.Text = "Save Output";
            this.saveOutputToolStripMenuItem.Click += new System.EventHandler(this.saveOutputToolStripMenuItem_Click);
            // 
            // manualControlToolStripMenuItem
            // 
            this.manualControlToolStripMenuItem.Name = "manualControlToolStripMenuItem";
            this.manualControlToolStripMenuItem.Size = new System.Drawing.Size(102, 20);
            this.manualControlToolStripMenuItem.Text = "Manual Control";
            this.manualControlToolStripMenuItem.Click += new System.EventHandler(this.manualControlToolStripMenuItem_Click);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graphTensionToolStripMenuItem,
            this.simulationToolStripMenuItem});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // graphTensionToolStripMenuItem
            // 
            this.graphTensionToolStripMenuItem.Name = "graphTensionToolStripMenuItem";
            this.graphTensionToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.graphTensionToolStripMenuItem.Text = "Graph Tension";
            // 
            // simulationToolStripMenuItem
            // 
            this.simulationToolStripMenuItem.Name = "simulationToolStripMenuItem";
            this.simulationToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.simulationToolStripMenuItem.Text = "Simulation";
            this.simulationToolStripMenuItem.Click += new System.EventHandler(this.simulationToolStripMenuItem_Click);
            // 
            // trackingToolStripMenuItem
            // 
            this.trackingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kalmanFilterToolStripMenuItem});
            this.trackingToolStripMenuItem.Name = "trackingToolStripMenuItem";
            this.trackingToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.trackingToolStripMenuItem.Text = "Tracking";
            // 
            // kalmanFilterToolStripMenuItem
            // 
            this.kalmanFilterToolStripMenuItem.Checked = true;
            this.kalmanFilterToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.kalmanFilterToolStripMenuItem.Name = "kalmanFilterToolStripMenuItem";
            this.kalmanFilterToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.kalmanFilterToolStripMenuItem.Text = "Kalman Filter";
            this.kalmanFilterToolStripMenuItem.Click += new System.EventHandler(this.kalmanFilterToolStripMenuItem_Click);
            // 
            // simTxtLabel
            // 
            this.simTxtLabel.AutoSize = true;
            this.simTxtLabel.Location = new System.Drawing.Point(11, 248);
            this.simTxtLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.simTxtLabel.Name = "simTxtLabel";
            this.simTxtLabel.Size = new System.Drawing.Size(58, 13);
            this.simTxtLabel.TabIndex = 6;
            this.simTxtLabel.Text = "Simulation:";
            // 
            // simCurrTxLabel
            // 
            this.simCurrTxLabel.AutoSize = true;
            this.simCurrTxLabel.Location = new System.Drawing.Point(11, 270);
            this.simCurrTxLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.simCurrTxLabel.Name = "simCurrTxLabel";
            this.simCurrTxLabel.Size = new System.Drawing.Size(128, 13);
            this.simCurrTxLabel.TabIndex = 7;
            this.simCurrTxLabel.Text = "No simulation file opened.";
            // 
            // connectLabel
            // 
            this.connectLabel.AutoSize = true;
            this.connectLabel.Location = new System.Drawing.Point(48, 41);
            this.connectLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.connectLabel.Name = "connectLabel";
            this.connectLabel.Size = new System.Drawing.Size(79, 13);
            this.connectLabel.TabIndex = 8;
            this.connectLabel.Text = "Not Connected";
            // 
            // cntTxtLabel
            // 
            this.cntTxtLabel.AutoSize = true;
            this.cntTxtLabel.Location = new System.Drawing.Point(11, 66);
            this.cntTxtLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cntTxtLabel.Name = "cntTxtLabel";
            this.cntTxtLabel.Size = new System.Drawing.Size(73, 13);
            this.cntTxtLabel.TabIndex = 9;
            this.cntTxtLabel.Text = "Motor Counts:";
            // 
            // tenTxtLabel
            // 
            this.tenTxtLabel.AutoSize = true;
            this.tenTxtLabel.Location = new System.Drawing.Point(11, 91);
            this.tenTxtLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tenTxtLabel.Name = "tenTxtLabel";
            this.tenTxtLabel.Size = new System.Drawing.Size(83, 13);
            this.tenTxtLabel.TabIndex = 10;
            this.tenTxtLabel.Text = "Tension Values:";
            // 
            // btnNext
            // 
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(88, 297);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 11;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnNext_MouseClick);
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(169, 297);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 12;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnSend_MouseClick);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(422, 24);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 13;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnDisconnect_MouseClick);
            // 
            // cntStatusLabel
            // 
            this.cntStatusLabel.AutoSize = true;
            this.cntStatusLabel.Location = new System.Drawing.Point(117, 66);
            this.cntStatusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cntStatusLabel.Name = "cntStatusLabel";
            this.cntStatusLabel.Size = new System.Drawing.Size(93, 13);
            this.cntStatusLabel.TabIndex = 14;
            this.cntStatusLabel.Text = "Nothing Received";
            // 
            // tenStatusLabel
            // 
            this.tenStatusLabel.AutoSize = true;
            this.tenStatusLabel.Location = new System.Drawing.Point(117, 91);
            this.tenStatusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tenStatusLabel.Name = "tenStatusLabel";
            this.tenStatusLabel.Size = new System.Drawing.Size(93, 13);
            this.tenStatusLabel.TabIndex = 15;
            this.tenStatusLabel.Text = "Nothing Received";
            // 
            // btSerialPort
            // 
            this.btSerialPort.BaudRate = 115200;
            this.btSerialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.btSerialPort_DataReceived);
            // 
            // btnRun
            // 
            this.btnRun.Enabled = false;
            this.btnRun.Location = new System.Drawing.Point(249, 297);
            this.btnRun.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(74, 23);
            this.btnRun.TabIndex = 16;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnRun_MouseClick);
            // 
            // btnPause
            // 
            this.btnPause.Enabled = false;
            this.btnPause.Location = new System.Drawing.Point(328, 297);
            this.btnPause.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(71, 23);
            this.btnPause.TabIndex = 17;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Enabled = false;
            this.btnPrev.Location = new System.Drawing.Point(14, 297);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(69, 23);
            this.btnPrev.TabIndex = 18;
            this.btnPrev.Text = "Previous";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseClick);
            // 
            // btnReset
            // 
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(404, 297);
            this.btnReset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(73, 23);
            this.btnReset.TabIndex = 19;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnReset_MouseClick);
            // 
            // tenProgressBar7
            // 
            this.tenProgressBar7.Location = new System.Drawing.Point(175, 215);
            this.tenProgressBar7.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tenProgressBar7.Maximum = 2000;
            this.tenProgressBar7.Name = "tenProgressBar7";
            this.tenProgressBar7.Size = new System.Drawing.Size(130, 19);
            this.tenProgressBar7.Step = 1;
            this.tenProgressBar7.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tenProgressBar7.TabIndex = 20;
            // 
            // tenProgressBar1
            // 
            this.tenProgressBar1.Location = new System.Drawing.Point(175, 141);
            this.tenProgressBar1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tenProgressBar1.Maximum = 10000;
            this.tenProgressBar1.Name = "tenProgressBar1";
            this.tenProgressBar1.Size = new System.Drawing.Size(130, 19);
            this.tenProgressBar1.Step = 1;
            this.tenProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tenProgressBar1.TabIndex = 21;
            // 
            // tenProgressBar2
            // 
            this.tenProgressBar2.Location = new System.Drawing.Point(328, 141);
            this.tenProgressBar2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tenProgressBar2.Maximum = 10000;
            this.tenProgressBar2.Name = "tenProgressBar2";
            this.tenProgressBar2.Size = new System.Drawing.Size(130, 19);
            this.tenProgressBar2.Step = 1;
            this.tenProgressBar2.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tenProgressBar2.TabIndex = 22;
            // 
            // tenProgressBar3
            // 
            this.tenProgressBar3.Location = new System.Drawing.Point(14, 178);
            this.tenProgressBar3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tenProgressBar3.Maximum = 10000;
            this.tenProgressBar3.Name = "tenProgressBar3";
            this.tenProgressBar3.Size = new System.Drawing.Size(130, 19);
            this.tenProgressBar3.Step = 1;
            this.tenProgressBar3.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tenProgressBar3.TabIndex = 23;
            // 
            // tenProgressBar4
            // 
            this.tenProgressBar4.Location = new System.Drawing.Point(175, 178);
            this.tenProgressBar4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tenProgressBar4.Maximum = 10000;
            this.tenProgressBar4.Name = "tenProgressBar4";
            this.tenProgressBar4.Size = new System.Drawing.Size(130, 19);
            this.tenProgressBar4.Step = 1;
            this.tenProgressBar4.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tenProgressBar4.TabIndex = 24;
            // 
            // tenProgressBar5
            // 
            this.tenProgressBar5.Location = new System.Drawing.Point(328, 178);
            this.tenProgressBar5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tenProgressBar5.Maximum = 10000;
            this.tenProgressBar5.Name = "tenProgressBar5";
            this.tenProgressBar5.Size = new System.Drawing.Size(130, 19);
            this.tenProgressBar5.Step = 1;
            this.tenProgressBar5.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tenProgressBar5.TabIndex = 25;
            // 
            // tenProgressBar6
            // 
            this.tenProgressBar6.Location = new System.Drawing.Point(14, 215);
            this.tenProgressBar6.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tenProgressBar6.Maximum = 2000;
            this.tenProgressBar6.Name = "tenProgressBar6";
            this.tenProgressBar6.Size = new System.Drawing.Size(130, 19);
            this.tenProgressBar6.Step = 1;
            this.tenProgressBar6.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tenProgressBar6.TabIndex = 26;
            // 
            // tenProgressBar0
            // 
            this.tenProgressBar0.Location = new System.Drawing.Point(14, 141);
            this.tenProgressBar0.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tenProgressBar0.Maximum = 10000;
            this.tenProgressBar0.Name = "tenProgressBar0";
            this.tenProgressBar0.Size = new System.Drawing.Size(130, 19);
            this.tenProgressBar0.Step = 1;
            this.tenProgressBar0.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tenProgressBar0.TabIndex = 27;
            // 
            // tenProgressBar8
            // 
            this.tenProgressBar8.Location = new System.Drawing.Point(328, 215);
            this.tenProgressBar8.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tenProgressBar8.Maximum = 2000;
            this.tenProgressBar8.Name = "tenProgressBar8";
            this.tenProgressBar8.Size = new System.Drawing.Size(130, 19);
            this.tenProgressBar8.Step = 1;
            this.tenProgressBar8.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tenProgressBar8.TabIndex = 28;
            // 
            // labTenMot0
            // 
            this.labTenMot0.AutoSize = true;
            this.labTenMot0.Location = new System.Drawing.Point(38, 124);
            this.labTenMot0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labTenMot0.Name = "labTenMot0";
            this.labTenMot0.Size = new System.Drawing.Size(87, 13);
            this.labTenMot0.TabIndex = 29;
            this.labTenMot0.Text = "Motor 0 Tension:";
            // 
            // labTenMot3
            // 
            this.labTenMot3.AutoSize = true;
            this.labTenMot3.Location = new System.Drawing.Point(38, 162);
            this.labTenMot3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labTenMot3.Name = "labTenMot3";
            this.labTenMot3.Size = new System.Drawing.Size(84, 13);
            this.labTenMot3.TabIndex = 30;
            this.labTenMot3.Text = "Motor 3 Tension";
            // 
            // labTenMot4
            // 
            this.labTenMot4.AutoSize = true;
            this.labTenMot4.Location = new System.Drawing.Point(199, 162);
            this.labTenMot4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labTenMot4.Name = "labTenMot4";
            this.labTenMot4.Size = new System.Drawing.Size(87, 13);
            this.labTenMot4.TabIndex = 31;
            this.labTenMot4.Text = "Motor 4 Tension:";
            // 
            // labTenMot1
            // 
            this.labTenMot1.AutoSize = true;
            this.labTenMot1.Location = new System.Drawing.Point(199, 124);
            this.labTenMot1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labTenMot1.Name = "labTenMot1";
            this.labTenMot1.Size = new System.Drawing.Size(87, 13);
            this.labTenMot1.TabIndex = 32;
            this.labTenMot1.Text = "Motor 1 Tension:";
            // 
            // labTenMot2
            // 
            this.labTenMot2.AutoSize = true;
            this.labTenMot2.Location = new System.Drawing.Point(352, 124);
            this.labTenMot2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labTenMot2.Name = "labTenMot2";
            this.labTenMot2.Size = new System.Drawing.Size(87, 13);
            this.labTenMot2.TabIndex = 33;
            this.labTenMot2.Text = "Motor 2 Tension:";
            // 
            // labTenMot5
            // 
            this.labTenMot5.AutoSize = true;
            this.labTenMot5.Location = new System.Drawing.Point(352, 162);
            this.labTenMot5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labTenMot5.Name = "labTenMot5";
            this.labTenMot5.Size = new System.Drawing.Size(87, 13);
            this.labTenMot5.TabIndex = 34;
            this.labTenMot5.Text = "Motor 5 Tension:";
            // 
            // labTenMot6
            // 
            this.labTenMot6.AutoSize = true;
            this.labTenMot6.Location = new System.Drawing.Point(38, 199);
            this.labTenMot6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labTenMot6.Name = "labTenMot6";
            this.labTenMot6.Size = new System.Drawing.Size(87, 13);
            this.labTenMot6.TabIndex = 35;
            this.labTenMot6.Text = "Motor 6 Tension:";
            // 
            // labTenMot7
            // 
            this.labTenMot7.AutoSize = true;
            this.labTenMot7.Location = new System.Drawing.Point(199, 199);
            this.labTenMot7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labTenMot7.Name = "labTenMot7";
            this.labTenMot7.Size = new System.Drawing.Size(87, 13);
            this.labTenMot7.TabIndex = 36;
            this.labTenMot7.Text = "Motor 7 Tension:";
            // 
            // labTenMot8
            // 
            this.labTenMot8.AutoSize = true;
            this.labTenMot8.Location = new System.Drawing.Point(352, 199);
            this.labTenMot8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labTenMot8.Name = "labTenMot8";
            this.labTenMot8.Size = new System.Drawing.Size(87, 13);
            this.labTenMot8.TabIndex = 37;
            this.labTenMot8.Text = "Motor 8 Tension:";
            // 
            // pnlMnlControl
            // 
            this.pnlMnlControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMnlControl.Controls.Add(this.btnMnlClose);
            this.pnlMnlControl.Controls.Add(this.btMnlSend);
            this.pnlMnlControl.Controls.Add(this.ddmMnlCntrlSelect);
            this.pnlMnlControl.Controls.Add(this.tbMnlMot8);
            this.pnlMnlControl.Controls.Add(this.tbMnlMot7);
            this.pnlMnlControl.Controls.Add(this.tbMnlMot6);
            this.pnlMnlControl.Controls.Add(this.tbMnlMot5);
            this.pnlMnlControl.Controls.Add(this.tbMnlMot4);
            this.pnlMnlControl.Controls.Add(this.tbMnlMot3);
            this.pnlMnlControl.Controls.Add(this.tbMnlMot2);
            this.pnlMnlControl.Controls.Add(this.tbMnlMot1);
            this.pnlMnlControl.Controls.Add(this.lblMnlMot8);
            this.pnlMnlControl.Controls.Add(this.lblMnlMot7);
            this.pnlMnlControl.Controls.Add(this.lblMnlMot6);
            this.pnlMnlControl.Controls.Add(this.lblMnlMot5);
            this.pnlMnlControl.Controls.Add(this.lblMnlMot4);
            this.pnlMnlControl.Controls.Add(this.lblMnlMot3);
            this.pnlMnlControl.Controls.Add(this.lblMnlMot2);
            this.pnlMnlControl.Controls.Add(this.lblMnlMot1);
            this.pnlMnlControl.Controls.Add(this.lblMnlMot0);
            this.pnlMnlControl.Controls.Add(this.lblMnl);
            this.pnlMnlControl.Controls.Add(this.tbMnlMot0);
            this.pnlMnlControl.Location = new System.Drawing.Point(520, 35);
            this.pnlMnlControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlMnlControl.Name = "pnlMnlControl";
            this.pnlMnlControl.Size = new System.Drawing.Size(284, 286);
            this.pnlMnlControl.TabIndex = 38;
            // 
            // btnMnlClose
            // 
            this.btnMnlClose.Location = new System.Drawing.Point(194, 246);
            this.btnMnlClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnMnlClose.Name = "btnMnlClose";
            this.btnMnlClose.Size = new System.Drawing.Size(57, 27);
            this.btnMnlClose.TabIndex = 43;
            this.btnMnlClose.Text = "Close";
            this.btnMnlClose.UseVisualStyleBackColor = true;
            this.btnMnlClose.Click += new System.EventHandler(this.btnMnlClose_Click);
            // 
            // btMnlSend
            // 
            this.btMnlSend.Location = new System.Drawing.Point(125, 246);
            this.btMnlSend.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btMnlSend.Name = "btMnlSend";
            this.btMnlSend.Size = new System.Drawing.Size(54, 27);
            this.btMnlSend.TabIndex = 42;
            this.btMnlSend.Text = "Send";
            this.btMnlSend.UseVisualStyleBackColor = true;
            this.btMnlSend.Click += new System.EventHandler(this.btMnlSend_Click);
            // 
            // ddmMnlCntrlSelect
            // 
            this.ddmMnlCntrlSelect.FormattingEnabled = true;
            this.ddmMnlCntrlSelect.Items.AddRange(new object[] {
            "Encoder Counts",
            "Tension Values"});
            this.ddmMnlCntrlSelect.Location = new System.Drawing.Point(104, 7);
            this.ddmMnlCntrlSelect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ddmMnlCntrlSelect.Name = "ddmMnlCntrlSelect";
            this.ddmMnlCntrlSelect.Size = new System.Drawing.Size(114, 21);
            this.ddmMnlCntrlSelect.TabIndex = 41;
            this.ddmMnlCntrlSelect.Text = "Encoder Counts";
            // 
            // tbMnlMot8
            // 
            this.tbMnlMot8.Location = new System.Drawing.Point(184, 208);
            this.tbMnlMot8.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbMnlMot8.Name = "tbMnlMot8";
            this.tbMnlMot8.Size = new System.Drawing.Size(76, 20);
            this.tbMnlMot8.TabIndex = 18;
            // 
            // tbMnlMot7
            // 
            this.tbMnlMot7.Location = new System.Drawing.Point(104, 208);
            this.tbMnlMot7.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbMnlMot7.Name = "tbMnlMot7";
            this.tbMnlMot7.Size = new System.Drawing.Size(76, 20);
            this.tbMnlMot7.TabIndex = 17;
            // 
            // tbMnlMot6
            // 
            this.tbMnlMot6.Location = new System.Drawing.Point(25, 208);
            this.tbMnlMot6.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbMnlMot6.Name = "tbMnlMot6";
            this.tbMnlMot6.Size = new System.Drawing.Size(76, 20);
            this.tbMnlMot6.TabIndex = 16;
            // 
            // tbMnlMot5
            // 
            this.tbMnlMot5.Location = new System.Drawing.Point(184, 142);
            this.tbMnlMot5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbMnlMot5.Name = "tbMnlMot5";
            this.tbMnlMot5.Size = new System.Drawing.Size(76, 20);
            this.tbMnlMot5.TabIndex = 15;
            // 
            // tbMnlMot4
            // 
            this.tbMnlMot4.Location = new System.Drawing.Point(104, 142);
            this.tbMnlMot4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbMnlMot4.Name = "tbMnlMot4";
            this.tbMnlMot4.Size = new System.Drawing.Size(76, 20);
            this.tbMnlMot4.TabIndex = 14;
            // 
            // tbMnlMot3
            // 
            this.tbMnlMot3.Location = new System.Drawing.Point(25, 142);
            this.tbMnlMot3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbMnlMot3.Name = "tbMnlMot3";
            this.tbMnlMot3.Size = new System.Drawing.Size(76, 20);
            this.tbMnlMot3.TabIndex = 13;
            // 
            // tbMnlMot2
            // 
            this.tbMnlMot2.Location = new System.Drawing.Point(184, 72);
            this.tbMnlMot2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbMnlMot2.Name = "tbMnlMot2";
            this.tbMnlMot2.Size = new System.Drawing.Size(76, 20);
            this.tbMnlMot2.TabIndex = 12;
            // 
            // tbMnlMot1
            // 
            this.tbMnlMot1.Location = new System.Drawing.Point(104, 72);
            this.tbMnlMot1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbMnlMot1.Name = "tbMnlMot1";
            this.tbMnlMot1.Size = new System.Drawing.Size(76, 20);
            this.tbMnlMot1.TabIndex = 11;
            // 
            // lblMnlMot8
            // 
            this.lblMnlMot8.AutoSize = true;
            this.lblMnlMot8.Location = new System.Drawing.Point(206, 171);
            this.lblMnlMot8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMnlMot8.Name = "lblMnlMot8";
            this.lblMnlMot8.Size = new System.Drawing.Size(46, 13);
            this.lblMnlMot8.TabIndex = 10;
            this.lblMnlMot8.Text = "Motor 8:";
            // 
            // lblMnlMot7
            // 
            this.lblMnlMot7.AutoSize = true;
            this.lblMnlMot7.Location = new System.Drawing.Point(129, 171);
            this.lblMnlMot7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMnlMot7.Name = "lblMnlMot7";
            this.lblMnlMot7.Size = new System.Drawing.Size(46, 13);
            this.lblMnlMot7.TabIndex = 9;
            this.lblMnlMot7.Text = "Motor 7:";
            // 
            // lblMnlMot6
            // 
            this.lblMnlMot6.AutoSize = true;
            this.lblMnlMot6.Location = new System.Drawing.Point(46, 171);
            this.lblMnlMot6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMnlMot6.Name = "lblMnlMot6";
            this.lblMnlMot6.Size = new System.Drawing.Size(46, 13);
            this.lblMnlMot6.TabIndex = 8;
            this.lblMnlMot6.Text = "Motor 6:";
            // 
            // lblMnlMot5
            // 
            this.lblMnlMot5.AutoSize = true;
            this.lblMnlMot5.Location = new System.Drawing.Point(206, 97);
            this.lblMnlMot5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMnlMot5.Name = "lblMnlMot5";
            this.lblMnlMot5.Size = new System.Drawing.Size(46, 13);
            this.lblMnlMot5.TabIndex = 7;
            this.lblMnlMot5.Text = "Motor 5:";
            // 
            // lblMnlMot4
            // 
            this.lblMnlMot4.AutoSize = true;
            this.lblMnlMot4.Location = new System.Drawing.Point(123, 97);
            this.lblMnlMot4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMnlMot4.Name = "lblMnlMot4";
            this.lblMnlMot4.Size = new System.Drawing.Size(46, 13);
            this.lblMnlMot4.TabIndex = 6;
            this.lblMnlMot4.Text = "Motor 4:";
            // 
            // lblMnlMot3
            // 
            this.lblMnlMot3.AutoSize = true;
            this.lblMnlMot3.Location = new System.Drawing.Point(40, 97);
            this.lblMnlMot3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMnlMot3.Name = "lblMnlMot3";
            this.lblMnlMot3.Size = new System.Drawing.Size(46, 13);
            this.lblMnlMot3.TabIndex = 5;
            this.lblMnlMot3.Text = "Motor 3:";
            // 
            // lblMnlMot2
            // 
            this.lblMnlMot2.AutoSize = true;
            this.lblMnlMot2.Location = new System.Drawing.Point(206, 37);
            this.lblMnlMot2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMnlMot2.Name = "lblMnlMot2";
            this.lblMnlMot2.Size = new System.Drawing.Size(46, 13);
            this.lblMnlMot2.TabIndex = 4;
            this.lblMnlMot2.Text = "Motor 2:";
            // 
            // lblMnlMot1
            // 
            this.lblMnlMot1.AutoSize = true;
            this.lblMnlMot1.Location = new System.Drawing.Point(123, 37);
            this.lblMnlMot1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMnlMot1.Name = "lblMnlMot1";
            this.lblMnlMot1.Size = new System.Drawing.Size(46, 13);
            this.lblMnlMot1.TabIndex = 3;
            this.lblMnlMot1.Text = "Motor 1:";
            // 
            // lblMnlMot0
            // 
            this.lblMnlMot0.AutoSize = true;
            this.lblMnlMot0.Location = new System.Drawing.Point(40, 37);
            this.lblMnlMot0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMnlMot0.Name = "lblMnlMot0";
            this.lblMnlMot0.Size = new System.Drawing.Size(46, 13);
            this.lblMnlMot0.TabIndex = 2;
            this.lblMnlMot0.Text = "Motor 0:";
            // 
            // lblMnl
            // 
            this.lblMnl.AutoSize = true;
            this.lblMnl.Location = new System.Drawing.Point(25, 10);
            this.lblMnl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMnl.Name = "lblMnl";
            this.lblMnl.Size = new System.Drawing.Size(84, 13);
            this.lblMnl.TabIndex = 1;
            this.lblMnl.Text = "Manual Control: ";
            // 
            // tbMnlMot0
            // 
            this.tbMnlMot0.Location = new System.Drawing.Point(25, 72);
            this.tbMnlMot0.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbMnlMot0.Name = "tbMnlMot0";
            this.tbMnlMot0.Size = new System.Drawing.Size(76, 20);
            this.tbMnlMot0.TabIndex = 0;
            // 
            // ddmSectionSelect
            // 
            this.ddmSectionSelect.FormattingEnabled = true;
            this.ddmSectionSelect.Items.AddRange(new object[] {
            "Tip Section",
            "Middle Section"});
            this.ddmSectionSelect.Location = new System.Drawing.Point(329, 60);
            this.ddmSectionSelect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ddmSectionSelect.Name = "ddmSectionSelect";
            this.ddmSectionSelect.Size = new System.Drawing.Size(92, 21);
            this.ddmSectionSelect.TabIndex = 1;
            this.ddmSectionSelect.Text = "Tip Section";
            // 
            // btnEvenTension
            // 
            this.btnEvenTension.Location = new System.Drawing.Point(328, 85);
            this.btnEvenTension.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEvenTension.Name = "btnEvenTension";
            this.btnEvenTension.Size = new System.Drawing.Size(92, 28);
            this.btnEvenTension.TabIndex = 39;
            this.btnEvenTension.Text = "Even Tensions";
            this.btnEvenTension.UseVisualStyleBackColor = true;
            this.btnEvenTension.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnEvenTension_MouseClick);
            // 
            // btnStopEven
            // 
            this.btnStopEven.Location = new System.Drawing.Point(424, 85);
            this.btnStopEven.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStopEven.Name = "btnStopEven";
            this.btnStopEven.Size = new System.Drawing.Size(73, 28);
            this.btnStopEven.TabIndex = 40;
            this.btnStopEven.Text = "Stop";
            this.btnStopEven.UseVisualStyleBackColor = true;
            this.btnStopEven.Click += new System.EventHandler(this.btnStopEven_Click);
            // 
            // pnlCSVWriter
            // 
            this.pnlCSVWriter.Controls.Add(this.btnCSVWriterEnd);
            this.pnlCSVWriter.Controls.Add(this.btnCSVWriterBegin);
            this.pnlCSVWriter.Controls.Add(this.lblCSVWriter);
            this.pnlCSVWriter.Location = new System.Drawing.Point(14, 338);
            this.pnlCSVWriter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlCSVWriter.Name = "pnlCSVWriter";
            this.pnlCSVWriter.Size = new System.Drawing.Size(463, 46);
            this.pnlCSVWriter.TabIndex = 41;
            // 
            // btnCSVWriterEnd
            // 
            this.btnCSVWriterEnd.Enabled = false;
            this.btnCSVWriterEnd.Location = new System.Drawing.Point(408, 6);
            this.btnCSVWriterEnd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCSVWriterEnd.Name = "btnCSVWriterEnd";
            this.btnCSVWriterEnd.Size = new System.Drawing.Size(50, 29);
            this.btnCSVWriterEnd.TabIndex = 2;
            this.btnCSVWriterEnd.Text = "End";
            this.btnCSVWriterEnd.UseVisualStyleBackColor = true;
            this.btnCSVWriterEnd.Click += new System.EventHandler(this.btnCSVWriterEnd_Click);
            // 
            // btnCSVWriterBegin
            // 
            this.btnCSVWriterBegin.Location = new System.Drawing.Point(356, 6);
            this.btnCSVWriterBegin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCSVWriterBegin.Name = "btnCSVWriterBegin";
            this.btnCSVWriterBegin.Size = new System.Drawing.Size(50, 29);
            this.btnCSVWriterBegin.TabIndex = 1;
            this.btnCSVWriterBegin.Text = "Begin";
            this.btnCSVWriterBegin.UseVisualStyleBackColor = true;
            this.btnCSVWriterBegin.Click += new System.EventHandler(this.btnCSVWriterBegin_Click);
            // 
            // lblCSVWriter
            // 
            this.lblCSVWriter.AutoSize = true;
            this.lblCSVWriter.Location = new System.Drawing.Point(3, 14);
            this.lblCSVWriter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCSVWriter.Name = "lblCSVWriter";
            this.lblCSVWriter.Size = new System.Drawing.Size(0, 13);
            this.lblCSVWriter.TabIndex = 0;
            // 
            // TendrilInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 393);
            this.Controls.Add(this.pnlCSVWriter);
            this.Controls.Add(this.btnStopEven);
            this.Controls.Add(this.btnEvenTension);
            this.Controls.Add(this.ddmSectionSelect);
            this.Controls.Add(this.pnlMnlControl);
            this.Controls.Add(this.labTenMot8);
            this.Controls.Add(this.labTenMot7);
            this.Controls.Add(this.labTenMot6);
            this.Controls.Add(this.labTenMot5);
            this.Controls.Add(this.labTenMot2);
            this.Controls.Add(this.labTenMot1);
            this.Controls.Add(this.labTenMot4);
            this.Controls.Add(this.labTenMot3);
            this.Controls.Add(this.labTenMot0);
            this.Controls.Add(this.tenProgressBar8);
            this.Controls.Add(this.tenProgressBar0);
            this.Controls.Add(this.tenProgressBar6);
            this.Controls.Add(this.tenProgressBar5);
            this.Controls.Add(this.tenProgressBar4);
            this.Controls.Add(this.tenProgressBar3);
            this.Controls.Add(this.tenProgressBar2);
            this.Controls.Add(this.tenProgressBar1);
            this.Controls.Add(this.tenProgressBar7);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.tenStatusLabel);
            this.Controls.Add(this.cntStatusLabel);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.tenTxtLabel);
            this.Controls.Add(this.cntTxtLabel);
            this.Controls.Add(this.connectLabel);
            this.Controls.Add(this.simCurrTxLabel);
            this.Controls.Add(this.simTxtLabel);
            this.Controls.Add(this.tbComPort);
            this.Controls.Add(this.brLabel);
            this.Controls.Add(this.comPortTxtLabel);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "TendrilInterface";
            this.Text = "Tendril Interface";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TendrilInterface_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlMnlControl.ResumeLayout(false);
            this.pnlMnlControl.PerformLayout();
            this.pnlCSVWriter.ResumeLayout(false);
            this.pnlCSVWriter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label comPortTxtLabel;
        private System.Windows.Forms.Label brLabel;
        private System.Windows.Forms.TextBox tbComPort;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.Label simTxtLabel;
        private System.Windows.Forms.Label simCurrTxLabel;
        private System.Windows.Forms.Label connectLabel;
        private System.Windows.Forms.Label cntTxtLabel;
        private System.Windows.Forms.Label tenTxtLabel;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label cntStatusLabel;
        private System.Windows.Forms.Label tenStatusLabel;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label labTenMot0;
        private System.Windows.Forms.Label labTenMot3;
        private System.Windows.Forms.Label labTenMot4;
        private System.Windows.Forms.Label labTenMot1;
        private System.Windows.Forms.Label labTenMot2;
        private System.Windows.Forms.Label labTenMot5;
        private System.Windows.Forms.Label labTenMot6;
        private System.Windows.Forms.Label labTenMot7;
        private System.Windows.Forms.Label labTenMot8;
        public System.Windows.Forms.ProgressBar tenProgressBar7;
        public System.Windows.Forms.ProgressBar tenProgressBar1;
        public System.Windows.Forms.ProgressBar tenProgressBar2;
        public System.Windows.Forms.ProgressBar tenProgressBar3;
        public System.Windows.Forms.ProgressBar tenProgressBar4;
        public System.Windows.Forms.ProgressBar tenProgressBar5;
        public System.Windows.Forms.ProgressBar tenProgressBar6;
        public System.Windows.Forms.ProgressBar tenProgressBar0;
        public System.Windows.Forms.ProgressBar tenProgressBar8;
        private System.Windows.Forms.ToolStripMenuItem manualControlToolStripMenuItem;
        private System.Windows.Forms.Panel pnlMnlControl;
        private System.Windows.Forms.TextBox tbMnlMot0;
        private System.Windows.Forms.ComboBox ddmSectionSelect;
        private System.Windows.Forms.Button btnEvenTension;
        public System.IO.Ports.SerialPort btSerialPort;
        private System.Windows.Forms.Button btnStopEven;
        private System.Windows.Forms.TextBox tbMnlMot8;
        private System.Windows.Forms.TextBox tbMnlMot7;
        private System.Windows.Forms.TextBox tbMnlMot6;
        private System.Windows.Forms.TextBox tbMnlMot5;
        private System.Windows.Forms.TextBox tbMnlMot4;
        private System.Windows.Forms.TextBox tbMnlMot3;
        private System.Windows.Forms.TextBox tbMnlMot2;
        private System.Windows.Forms.TextBox tbMnlMot1;
        private System.Windows.Forms.Label lblMnlMot8;
        private System.Windows.Forms.Label lblMnlMot7;
        private System.Windows.Forms.Label lblMnlMot6;
        private System.Windows.Forms.Label lblMnlMot5;
        private System.Windows.Forms.Label lblMnlMot4;
        private System.Windows.Forms.Label lblMnlMot3;
        private System.Windows.Forms.Label lblMnlMot2;
        private System.Windows.Forms.Label lblMnlMot1;
        private System.Windows.Forms.Label lblMnlMot0;
        private System.Windows.Forms.Label lblMnl;
        private System.Windows.Forms.ComboBox ddmMnlCntrlSelect;
        private System.Windows.Forms.Button btMnlSend;
        private System.Windows.Forms.Button btnMnlClose;
        private System.Windows.Forms.ToolStripMenuItem saveOutputToolStripMenuItem;
        private System.Windows.Forms.Panel pnlCSVWriter;
        private System.Windows.Forms.Label lblCSVWriter;
        private System.Windows.Forms.Button btnCSVWriterBegin;
        private System.Windows.Forms.Button btnCSVWriterEnd;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphTensionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simulationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trackingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kalmanFilterToolStripMenuItem;
    }
}

