namespace HIDTool
{
    partial class HIDToolForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GetIQVersionBtn = new System.Windows.Forms.Button();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.GetRecordLogBtn = new System.Windows.Forms.Button();
            this.StopRecordBtn = new System.Windows.Forms.Button();
            this.StartRecordLogBtn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.GetCPUFreqBtn = new System.Windows.Forms.Button();
            this.IsCaptureSupportedBtn = new System.Windows.Forms.Button();
            this.GetAppVersionBtn = new System.Windows.Forms.Button();
            this.RebootBootloaderBtn = new System.Windows.Forms.Button();
            this.GetCPUTempBtn = new System.Windows.Forms.Button();
            this.GetDeviceNameBtn = new System.Windows.Forms.Button();
            this.GetOTAKeyBtn = new System.Windows.Forms.Button();
            this.GetUpgradeMethodBtn = new System.Windows.Forms.Button();
            this.GetVersionBtn = new System.Windows.Forms.Button();
            this.GetCameraModelBtn = new System.Windows.Forms.Button();
            this.comboBoxDevices = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.GetSNBtn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.comboBoxPreviewMode = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnUpdateDeviceList = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tabControlEPTZ = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.eptz_move_textbox_length = new System.Windows.Forms.TextBox();
            this.down_narrow = new System.Windows.Forms.Button();
            this.right_narrow = new System.Windows.Forms.Button();
            this.left_narrow = new System.Windows.Forms.Button();
            this.up_narrow = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.eptz_size_textbox_length = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkBoxCheckAck = new System.Windows.Forms.CheckBox();
            this.btnSimulateSound = new System.Windows.Forms.Button();
            this.lblReceiveCnt = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSendCnt = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.combBoxAngles = new System.Windows.Forms.ComboBox();
            this.GetEPTZEnableBtn = new System.Windows.Forms.Button();
            this.GetEPTZModeBtn = new System.Windows.Forms.Button();
            this.IsEPTZSupportedBtn = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.clearAgingTestButton = new System.Windows.Forms.Button();
            this.activateAgingTestButton = new System.Windows.Forms.Button();
            this.agingTestTimeTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabControlEPTZ.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // GetIQVersionBtn
            // 
            this.GetIQVersionBtn.Location = new System.Drawing.Point(6, 110);
            this.GetIQVersionBtn.Name = "GetIQVersionBtn";
            this.GetIQVersionBtn.Size = new System.Drawing.Size(124, 36);
            this.GetIQVersionBtn.TabIndex = 53;
            this.GetIQVersionBtn.Text = "获取IQ版本号";
            this.GetIQVersionBtn.UseVisualStyleBackColor = true;
            this.GetIQVersionBtn.Click += new System.EventHandler(this.GetIQVersionBtn_Click);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(880, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 379);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(944, 22);
            this.statusStrip1.TabIndex = 51;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Visible = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // GetRecordLogBtn
            // 
            this.GetRecordLogBtn.Location = new System.Drawing.Point(6, 155);
            this.GetRecordLogBtn.Name = "GetRecordLogBtn";
            this.GetRecordLogBtn.Size = new System.Drawing.Size(124, 36);
            this.GetRecordLogBtn.TabIndex = 50;
            this.GetRecordLogBtn.Text = "获取Log";
            this.GetRecordLogBtn.UseVisualStyleBackColor = true;
            this.GetRecordLogBtn.Click += new System.EventHandler(this.GetRecordLogBtn_Click);
            // 
            // StopRecordBtn
            // 
            this.StopRecordBtn.Location = new System.Drawing.Point(6, 110);
            this.StopRecordBtn.Name = "StopRecordBtn";
            this.StopRecordBtn.Size = new System.Drawing.Size(124, 36);
            this.StopRecordBtn.TabIndex = 49;
            this.StopRecordBtn.Text = "停止录制log";
            this.StopRecordBtn.UseVisualStyleBackColor = true;
            this.StopRecordBtn.Click += new System.EventHandler(this.StopRecordLogBtn_Click);
            // 
            // StartRecordLogBtn
            // 
            this.StartRecordLogBtn.Location = new System.Drawing.Point(6, 65);
            this.StartRecordLogBtn.Name = "StartRecordLogBtn";
            this.StartRecordLogBtn.Size = new System.Drawing.Size(124, 36);
            this.StartRecordLogBtn.TabIndex = 48;
            this.StartRecordLogBtn.Text = "开始录制log";
            this.StartRecordLogBtn.UseVisualStyleBackColor = true;
            this.StartRecordLogBtn.Click += new System.EventHandler(this.StartRecordLogBtn_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // GetCPUFreqBtn
            // 
            this.GetCPUFreqBtn.Location = new System.Drawing.Point(6, 65);
            this.GetCPUFreqBtn.Name = "GetCPUFreqBtn";
            this.GetCPUFreqBtn.Size = new System.Drawing.Size(124, 36);
            this.GetCPUFreqBtn.TabIndex = 52;
            this.GetCPUFreqBtn.Text = "获取CPU频率";
            this.GetCPUFreqBtn.UseVisualStyleBackColor = true;
            this.GetCPUFreqBtn.Click += new System.EventHandler(this.GetCPUFreqBtn_Click);
            // 
            // IsCaptureSupportedBtn
            // 
            this.IsCaptureSupportedBtn.Location = new System.Drawing.Point(6, 200);
            this.IsCaptureSupportedBtn.Name = "IsCaptureSupportedBtn";
            this.IsCaptureSupportedBtn.Size = new System.Drawing.Size(124, 36);
            this.IsCaptureSupportedBtn.TabIndex = 46;
            this.IsCaptureSupportedBtn.Text = "是否支持48M拍照";
            this.IsCaptureSupportedBtn.UseVisualStyleBackColor = true;
            this.IsCaptureSupportedBtn.Click += new System.EventHandler(this.IsCaptureSupportedBtn_Click);
            // 
            // GetAppVersionBtn
            // 
            this.GetAppVersionBtn.Location = new System.Drawing.Point(6, 65);
            this.GetAppVersionBtn.Name = "GetAppVersionBtn";
            this.GetAppVersionBtn.Size = new System.Drawing.Size(124, 36);
            this.GetAppVersionBtn.TabIndex = 43;
            this.GetAppVersionBtn.Text = "获取APP版本号";
            this.GetAppVersionBtn.UseVisualStyleBackColor = true;
            this.GetAppVersionBtn.Click += new System.EventHandler(this.GetAppVersionBtn_Click);
            // 
            // RebootBootloaderBtn
            // 
            this.RebootBootloaderBtn.Location = new System.Drawing.Point(6, 20);
            this.RebootBootloaderBtn.Name = "RebootBootloaderBtn";
            this.RebootBootloaderBtn.Size = new System.Drawing.Size(124, 36);
            this.RebootBootloaderBtn.TabIndex = 42;
            this.RebootBootloaderBtn.Text = "启动Bootloader模式";
            this.RebootBootloaderBtn.UseVisualStyleBackColor = true;
            this.RebootBootloaderBtn.Click += new System.EventHandler(this.RebootBootloaderBtn_Click);
            // 
            // GetCPUTempBtn
            // 
            this.GetCPUTempBtn.Location = new System.Drawing.Point(6, 20);
            this.GetCPUTempBtn.Name = "GetCPUTempBtn";
            this.GetCPUTempBtn.Size = new System.Drawing.Size(124, 36);
            this.GetCPUTempBtn.TabIndex = 41;
            this.GetCPUTempBtn.Text = "获取CPU温度";
            this.GetCPUTempBtn.UseVisualStyleBackColor = true;
            this.GetCPUTempBtn.Click += new System.EventHandler(this.GetCPUTempBtn_Click);
            // 
            // GetDeviceNameBtn
            // 
            this.GetDeviceNameBtn.Location = new System.Drawing.Point(136, 155);
            this.GetDeviceNameBtn.Name = "GetDeviceNameBtn";
            this.GetDeviceNameBtn.Size = new System.Drawing.Size(124, 36);
            this.GetDeviceNameBtn.TabIndex = 40;
            this.GetDeviceNameBtn.Text = "获取设备名";
            this.GetDeviceNameBtn.UseVisualStyleBackColor = true;
            this.GetDeviceNameBtn.Click += new System.EventHandler(this.GetDeviceNameBtn_Click);
            // 
            // GetOTAKeyBtn
            // 
            this.GetOTAKeyBtn.Location = new System.Drawing.Point(136, 110);
            this.GetOTAKeyBtn.Name = "GetOTAKeyBtn";
            this.GetOTAKeyBtn.Size = new System.Drawing.Size(124, 36);
            this.GetOTAKeyBtn.TabIndex = 37;
            this.GetOTAKeyBtn.Text = "获取OTA Key";
            this.GetOTAKeyBtn.UseVisualStyleBackColor = true;
            this.GetOTAKeyBtn.Click += new System.EventHandler(this.GetOTAKeyBtn_Click);
            // 
            // GetUpgradeMethodBtn
            // 
            this.GetUpgradeMethodBtn.Location = new System.Drawing.Point(136, 65);
            this.GetUpgradeMethodBtn.Name = "GetUpgradeMethodBtn";
            this.GetUpgradeMethodBtn.Size = new System.Drawing.Size(124, 36);
            this.GetUpgradeMethodBtn.TabIndex = 36;
            this.GetUpgradeMethodBtn.Text = "获取升级方式";
            this.GetUpgradeMethodBtn.UseVisualStyleBackColor = true;
            this.GetUpgradeMethodBtn.Click += new System.EventHandler(this.GetUpgradeMethodBtn_Click);
            // 
            // GetVersionBtn
            // 
            this.GetVersionBtn.Location = new System.Drawing.Point(6, 20);
            this.GetVersionBtn.Name = "GetVersionBtn";
            this.GetVersionBtn.Size = new System.Drawing.Size(124, 36);
            this.GetVersionBtn.TabIndex = 35;
            this.GetVersionBtn.Text = "获取版本号";
            this.GetVersionBtn.UseVisualStyleBackColor = true;
            this.GetVersionBtn.Click += new System.EventHandler(this.GetVersionBtn_Click);
            // 
            // GetCameraModelBtn
            // 
            this.GetCameraModelBtn.Location = new System.Drawing.Point(136, 20);
            this.GetCameraModelBtn.Name = "GetCameraModelBtn";
            this.GetCameraModelBtn.Size = new System.Drawing.Size(124, 36);
            this.GetCameraModelBtn.TabIndex = 34;
            this.GetCameraModelBtn.Text = "获取模组信息";
            this.GetCameraModelBtn.UseVisualStyleBackColor = true;
            this.GetCameraModelBtn.Click += new System.EventHandler(this.GetCameraModelBtn_Click);
            // 
            // comboBoxDevices
            // 
            this.comboBoxDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDevices.FormattingEnabled = true;
            this.comboBoxDevices.Location = new System.Drawing.Point(76, 16);
            this.comboBoxDevices.Name = "comboBoxDevices";
            this.comboBoxDevices.Size = new System.Drawing.Size(197, 20);
            this.comboBoxDevices.TabIndex = 54;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 55;
            this.label4.Text = "相机设备";
            // 
            // GetSNBtn
            // 
            this.GetSNBtn.Location = new System.Drawing.Point(6, 155);
            this.GetSNBtn.Name = "GetSNBtn";
            this.GetSNBtn.Size = new System.Drawing.Size(124, 36);
            this.GetSNBtn.TabIndex = 56;
            this.GetSNBtn.Text = "获取SN号";
            this.GetSNBtn.UseVisualStyleBackColor = true;
            this.GetSNBtn.Click += new System.EventHandler(this.GetSNBtn_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.GetCameraModelBtn);
            this.groupBox4.Controls.Add(this.GetVersionBtn);
            this.groupBox4.Controls.Add(this.GetUpgradeMethodBtn);
            this.groupBox4.Controls.Add(this.GetOTAKeyBtn);
            this.groupBox4.Controls.Add(this.GetSNBtn);
            this.groupBox4.Controls.Add(this.GetDeviceNameBtn);
            this.groupBox4.Controls.Add(this.GetAppVersionBtn);
            this.groupBox4.Controls.Add(this.GetIQVersionBtn);
            this.groupBox4.Controls.Add(this.IsCaptureSupportedBtn);
            this.groupBox4.Location = new System.Drawing.Point(13, 58);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(267, 311);
            this.groupBox4.TabIndex = 59;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "获取基本信息";
            this.groupBox4.Visible = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.GetCPUTempBtn);
            this.groupBox5.Controls.Add(this.GetCPUFreqBtn);
            this.groupBox5.Location = new System.Drawing.Point(12, 52);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(135, 311);
            this.groupBox5.TabIndex = 60;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "获取运行状态";
            this.groupBox5.Visible = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.RebootBootloaderBtn);
            this.groupBox6.Controls.Add(this.StartRecordLogBtn);
            this.groupBox6.Controls.Add(this.StopRecordBtn);
            this.groupBox6.Controls.Add(this.GetRecordLogBtn);
            this.groupBox6.Location = new System.Drawing.Point(162, 52);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(135, 311);
            this.groupBox6.TabIndex = 61;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "刷机与调试";
            this.groupBox6.Visible = false;
            // 
            // comboBoxPreviewMode
            // 
            this.comboBoxPreviewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPreviewMode.FormattingEnabled = true;
            this.comboBoxPreviewMode.Location = new System.Drawing.Point(79, 46);
            this.comboBoxPreviewMode.Name = "comboBoxPreviewMode";
            this.comboBoxPreviewMode.Size = new System.Drawing.Size(272, 20);
            this.comboBoxPreviewMode.TabIndex = 62;
            this.comboBoxPreviewMode.Visible = false;
            this.comboBoxPreviewMode.SelectionChangeCommitted += new System.EventHandler(this.comboBoxPreviewMode_SelectionChangeCommitted);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(13, 48);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 16);
            this.label7.TabIndex = 1;
            this.label7.Text = "预览模式";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Visible = false;
            // 
            // btnUpdateDeviceList
            // 
            this.btnUpdateDeviceList.Location = new System.Drawing.Point(295, 14);
            this.btnUpdateDeviceList.Name = "btnUpdateDeviceList";
            this.btnUpdateDeviceList.Size = new System.Drawing.Size(95, 24);
            this.btnUpdateDeviceList.TabIndex = 64;
            this.btnUpdateDeviceList.Text = "刷新设备列表";
            this.btnUpdateDeviceList.UseVisualStyleBackColor = true;
            this.btnUpdateDeviceList.Click += new System.EventHandler(this.btnUpdateDeviceList_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.groupBox7.Controls.Add(this.tabControlEPTZ);
            this.groupBox7.Controls.Add(this.GetEPTZEnableBtn);
            this.groupBox7.Controls.Add(this.GetEPTZModeBtn);
            this.groupBox7.Controls.Add(this.IsEPTZSupportedBtn);
            this.groupBox7.Location = new System.Drawing.Point(12, 44);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(378, 325);
            this.groupBox7.TabIndex = 65;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "老化测试页面";
            // 
            // tabControlEPTZ
            // 
            this.tabControlEPTZ.Controls.Add(this.tabPage1);
            this.tabControlEPTZ.Controls.Add(this.tabPage2);
            this.tabControlEPTZ.Location = new System.Drawing.Point(7, 20);
            this.tabControlEPTZ.Name = "tabControlEPTZ";
            this.tabControlEPTZ.SelectedIndex = 0;
            this.tabControlEPTZ.Size = new System.Drawing.Size(365, 283);
            this.tabControlEPTZ.TabIndex = 63;
            this.tabControlEPTZ.Visible = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(357, 257);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "手动";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.eptz_move_textbox_length);
            this.groupBox3.Controls.Add(this.down_narrow);
            this.groupBox3.Controls.Add(this.right_narrow);
            this.groupBox3.Controls.Add(this.left_narrow);
            this.groupBox3.Controls.Add(this.up_narrow);
            this.groupBox3.Location = new System.Drawing.Point(11, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(150, 158);
            this.groupBox3.TabIndex = 58;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "移动控制";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 119);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 33);
            this.label6.TabIndex = 9;
            this.label6.Text = "步长（1步32像素 0~255）";
            // 
            // eptz_move_textbox_length
            // 
            this.eptz_move_textbox_length.Location = new System.Drawing.Point(108, 123);
            this.eptz_move_textbox_length.MaxLength = 3;
            this.eptz_move_textbox_length.Name = "eptz_move_textbox_length";
            this.eptz_move_textbox_length.Size = new System.Drawing.Size(38, 21);
            this.eptz_move_textbox_length.TabIndex = 10;
            this.eptz_move_textbox_length.Text = "1";
            // 
            // down_narrow
            // 
            this.down_narrow.BackColor = System.Drawing.Color.Transparent;
            this.down_narrow.BackgroundImage = global::HIDTool.Properties.Resources.down_narrow;
            this.down_narrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.down_narrow.FlatAppearance.BorderSize = 0;
            this.down_narrow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.down_narrow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.down_narrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.down_narrow.Location = new System.Drawing.Point(56, 79);
            this.down_narrow.Name = "down_narrow";
            this.down_narrow.Size = new System.Drawing.Size(33, 33);
            this.down_narrow.TabIndex = 3;
            this.down_narrow.UseVisualStyleBackColor = false;
            this.down_narrow.Click += new System.EventHandler(this.EPTZ_MOVE_DOWN_Click);
            // 
            // right_narrow
            // 
            this.right_narrow.BackColor = System.Drawing.Color.Transparent;
            this.right_narrow.BackgroundImage = global::HIDTool.Properties.Resources.right_narrow;
            this.right_narrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.right_narrow.FlatAppearance.BorderSize = 0;
            this.right_narrow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.right_narrow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.right_narrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.right_narrow.Location = new System.Drawing.Point(91, 46);
            this.right_narrow.Name = "right_narrow";
            this.right_narrow.Size = new System.Drawing.Size(33, 33);
            this.right_narrow.TabIndex = 2;
            this.right_narrow.UseVisualStyleBackColor = false;
            this.right_narrow.Click += new System.EventHandler(this.EPTZ_MOVE_RIGHT_Click);
            // 
            // left_narrow
            // 
            this.left_narrow.BackColor = System.Drawing.Color.Transparent;
            this.left_narrow.BackgroundImage = global::HIDTool.Properties.Resources.left_narrow;
            this.left_narrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.left_narrow.FlatAppearance.BorderSize = 0;
            this.left_narrow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.left_narrow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.left_narrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.left_narrow.Location = new System.Drawing.Point(23, 46);
            this.left_narrow.Name = "left_narrow";
            this.left_narrow.Size = new System.Drawing.Size(33, 33);
            this.left_narrow.TabIndex = 1;
            this.left_narrow.UseVisualStyleBackColor = false;
            this.left_narrow.Click += new System.EventHandler(this.EPTZ_MOVE_LEFT_Click);
            // 
            // up_narrow
            // 
            this.up_narrow.BackColor = System.Drawing.Color.Transparent;
            this.up_narrow.BackgroundImage = global::HIDTool.Properties.Resources.up_narrow;
            this.up_narrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.up_narrow.FlatAppearance.BorderSize = 0;
            this.up_narrow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.up_narrow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.up_narrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.up_narrow.Location = new System.Drawing.Point(56, 13);
            this.up_narrow.Name = "up_narrow";
            this.up_narrow.Size = new System.Drawing.Size(33, 33);
            this.up_narrow.TabIndex = 0;
            this.up_narrow.UseVisualStyleBackColor = false;
            this.up_narrow.Click += new System.EventHandler(this.EPTZ_MOVE_UP_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.eptz_size_textbox_length);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Location = new System.Drawing.Point(10, 172);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(152, 80);
            this.groupBox2.TabIndex = 57;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "缩放控制";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 30);
            this.label5.TabIndex = 4;
            this.label5.Text = "步长（1步64像素 0~255）：";
            // 
            // eptz_size_textbox_length
            // 
            this.eptz_size_textbox_length.Location = new System.Drawing.Point(106, 22);
            this.eptz_size_textbox_length.MaxLength = 3;
            this.eptz_size_textbox_length.Name = "eptz_size_textbox_length";
            this.eptz_size_textbox_length.Size = new System.Drawing.Size(38, 21);
            this.eptz_size_textbox_length.TabIndex = 8;
            this.eptz_size_textbox_length.Text = "1";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(100, 50);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(50, 23);
            this.button4.TabIndex = 2;
            this.button4.Text = "复位";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.EPTZ_ENLARGE_NARROW_RESET_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(50, 50);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(50, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "缩小";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.EPTZ_NARROW_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1, 50);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "放大";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.EPTZ_ENLARGE_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(357, 257);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "主讲者模式";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkBoxCheckAck);
            this.groupBox1.Controls.Add(this.btnSimulateSound);
            this.groupBox1.Controls.Add(this.lblReceiveCnt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblSendCnt);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.combBoxAngles);
            this.groupBox1.Location = new System.Drawing.Point(10, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(153, 180);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "模拟声源角度";
            // 
            // chkBoxCheckAck
            // 
            this.chkBoxCheckAck.AutoSize = true;
            this.chkBoxCheckAck.Location = new System.Drawing.Point(70, 61);
            this.chkBoxCheckAck.Margin = new System.Windows.Forms.Padding(2);
            this.chkBoxCheckAck.Name = "chkBoxCheckAck";
            this.chkBoxCheckAck.Size = new System.Drawing.Size(72, 16);
            this.chkBoxCheckAck.TabIndex = 3;
            this.chkBoxCheckAck.Text = "检查应答";
            this.chkBoxCheckAck.UseVisualStyleBackColor = true;
            // 
            // btnSimulateSound
            // 
            this.btnSimulateSound.Location = new System.Drawing.Point(50, 140);
            this.btnSimulateSound.Margin = new System.Windows.Forms.Padding(2);
            this.btnSimulateSound.Name = "btnSimulateSound";
            this.btnSimulateSound.Size = new System.Drawing.Size(54, 30);
            this.btnSimulateSound.TabIndex = 2;
            this.btnSimulateSound.Text = "开始";
            this.btnSimulateSound.UseVisualStyleBackColor = true;
            this.btnSimulateSound.Click += new System.EventHandler(this.btnSimulateSound_Click);
            // 
            // lblReceiveCnt
            // 
            this.lblReceiveCnt.Location = new System.Drawing.Point(70, 117);
            this.lblReceiveCnt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblReceiveCnt.Name = "lblReceiveCnt";
            this.lblReceiveCnt.Size = new System.Drawing.Size(67, 16);
            this.lblReceiveCnt.TabIndex = 1;
            this.lblReceiveCnt.Text = "0";
            this.lblReceiveCnt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 117);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "应答次数";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSendCnt
            // 
            this.lblSendCnt.Location = new System.Drawing.Point(70, 89);
            this.lblSendCnt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSendCnt.Name = "lblSendCnt";
            this.lblSendCnt.Size = new System.Drawing.Size(67, 16);
            this.lblSendCnt.TabIndex = 1;
            this.lblSendCnt.Text = "0";
            this.lblSendCnt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 89);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "发送次数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "声源角度";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // combBoxAngles
            // 
            this.combBoxAngles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combBoxAngles.FormattingEnabled = true;
            this.combBoxAngles.Location = new System.Drawing.Point(70, 29);
            this.combBoxAngles.Margin = new System.Windows.Forms.Padding(2);
            this.combBoxAngles.Name = "combBoxAngles";
            this.combBoxAngles.Size = new System.Drawing.Size(68, 20);
            this.combBoxAngles.TabIndex = 0;
            // 
            // GetEPTZEnableBtn
            // 
            this.GetEPTZEnableBtn.Location = new System.Drawing.Point(6, 110);
            this.GetEPTZEnableBtn.Name = "GetEPTZEnableBtn";
            this.GetEPTZEnableBtn.Size = new System.Drawing.Size(124, 36);
            this.GetEPTZEnableBtn.TabIndex = 48;
            this.GetEPTZEnableBtn.Text = "获取电子云台状态";
            this.GetEPTZEnableBtn.UseVisualStyleBackColor = true;
            this.GetEPTZEnableBtn.Visible = false;
            this.GetEPTZEnableBtn.Click += new System.EventHandler(this.GetEPTZEnableBtn_Click);
            // 
            // GetEPTZModeBtn
            // 
            this.GetEPTZModeBtn.Location = new System.Drawing.Point(6, 65);
            this.GetEPTZModeBtn.Name = "GetEPTZModeBtn";
            this.GetEPTZModeBtn.Size = new System.Drawing.Size(124, 36);
            this.GetEPTZModeBtn.TabIndex = 48;
            this.GetEPTZModeBtn.Text = "获取电子云台模式";
            this.GetEPTZModeBtn.UseVisualStyleBackColor = true;
            this.GetEPTZModeBtn.Visible = false;
            this.GetEPTZModeBtn.Click += new System.EventHandler(this.GetEPTZModeBtn_Click);
            // 
            // IsEPTZSupportedBtn
            // 
            this.IsEPTZSupportedBtn.Location = new System.Drawing.Point(6, 20);
            this.IsEPTZSupportedBtn.Name = "IsEPTZSupportedBtn";
            this.IsEPTZSupportedBtn.Size = new System.Drawing.Size(124, 36);
            this.IsEPTZSupportedBtn.TabIndex = 48;
            this.IsEPTZSupportedBtn.Text = "是否支持电子云台";
            this.IsEPTZSupportedBtn.UseVisualStyleBackColor = true;
            this.IsEPTZSupportedBtn.Visible = false;
            this.IsEPTZSupportedBtn.Click += new System.EventHandler(this.IsEPTZSupportedBtn_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.groupBox8.Controls.Add(this.tabControl1);
            this.groupBox8.Controls.Add(this.button12);
            this.groupBox8.Controls.Add(this.button13);
            this.groupBox8.Controls.Add(this.button14);
            this.groupBox8.Location = new System.Drawing.Point(12, 42);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(378, 315);
            this.groupBox8.TabIndex = 66;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "老化测试页面";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(7, 20);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(365, 283);
            this.tabControl1.TabIndex = 63;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.clearAgingTestButton);
            this.tabPage3.Controls.Add(this.activateAgingTestButton);
            this.tabPage3.Controls.Add(this.agingTestTimeTextBox);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(357, 257);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "测试程序入口";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // clearAgingTestButton
            // 
            this.clearAgingTestButton.Location = new System.Drawing.Point(80, 120);
            this.clearAgingTestButton.Name = "clearAgingTestButton";
            this.clearAgingTestButton.Size = new System.Drawing.Size(177, 23);
            this.clearAgingTestButton.TabIndex = 3;
            this.clearAgingTestButton.Text = "清除老化测试";
            this.clearAgingTestButton.UseVisualStyleBackColor = true;
            this.clearAgingTestButton.Click += new System.EventHandler(this.clearAgingTestButton_Click);
            // 
            // activateAgingTestButton
            // 
            this.activateAgingTestButton.Location = new System.Drawing.Point(80, 84);
            this.activateAgingTestButton.Name = "activateAgingTestButton";
            this.activateAgingTestButton.Size = new System.Drawing.Size(177, 23);
            this.activateAgingTestButton.TabIndex = 2;
            this.activateAgingTestButton.Text = "激活老化测试";
            this.activateAgingTestButton.UseVisualStyleBackColor = true;
            this.activateAgingTestButton.Click += new System.EventHandler(this.activateAgingTestButton_Click);
            // 
            // agingTestTimeTextBox
            // 
            this.agingTestTimeTextBox.Location = new System.Drawing.Point(82, 51);
            this.agingTestTimeTextBox.Name = "agingTestTimeTextBox";
            this.agingTestTimeTextBox.Size = new System.Drawing.Size(175, 21);
            this.agingTestTimeTextBox.TabIndex = 1;
            this.agingTestTimeTextBox.Text = "240";
            this.agingTestTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.agingTestTimeTextBox.TextChanged += new System.EventHandler(this.agingTestTimeTextBox_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(58, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(215, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "请输入测试时间：(单位/M - 分)整数倍";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(6, 110);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(124, 36);
            this.button12.TabIndex = 48;
            this.button12.Text = "获取电子云台状态";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Visible = false;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(6, 65);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(124, 36);
            this.button13.TabIndex = 48;
            this.button13.Text = "获取电子云台模式";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Visible = false;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(6, 20);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(124, 36);
            this.button14.TabIndex = 48;
            this.button14.Text = "是否支持电子云台";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Visible = false;
            // 
            // HIDToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(404, 361);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.btnUpdateDeviceList);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.comboBoxPreviewMode);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxDevices);
            this.Controls.Add(this.statusStrip1);
            this.MaximumSize = new System.Drawing.Size(420, 400);
            this.MinimumSize = new System.Drawing.Size(420, 400);
            this.Name = "HIDToolForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HIDTool_2.3";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HIDToolForm_FormClosing);
            this.Load += new System.EventHandler(this.HIDToolForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.tabControlEPTZ.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetIQVersionBtn;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button GetRecordLogBtn;
        private System.Windows.Forms.Button StopRecordBtn;
        private System.Windows.Forms.Button StartRecordLogBtn;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button GetCPUFreqBtn;
        private System.Windows.Forms.Button IsCaptureSupportedBtn;
        private System.Windows.Forms.Button GetAppVersionBtn;
        private System.Windows.Forms.Button RebootBootloaderBtn;
        private System.Windows.Forms.Button GetCPUTempBtn;
        private System.Windows.Forms.Button GetDeviceNameBtn;
        private System.Windows.Forms.Button GetOTAKeyBtn;
        private System.Windows.Forms.Button GetUpgradeMethodBtn;
        private System.Windows.Forms.Button GetVersionBtn;
        private System.Windows.Forms.Button GetCameraModelBtn;
        private System.Windows.Forms.ComboBox comboBoxDevices;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button GetSNBtn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox comboBoxPreviewMode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnUpdateDeviceList;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button GetEPTZEnableBtn;
        private System.Windows.Forms.Button GetEPTZModeBtn;
        private System.Windows.Forms.Button IsEPTZSupportedBtn;
        private System.Windows.Forms.TabControl tabControlEPTZ;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox eptz_move_textbox_length;
        private System.Windows.Forms.Button down_narrow;
        private System.Windows.Forms.Button right_narrow;
        private System.Windows.Forms.Button left_narrow;
        private System.Windows.Forms.Button up_narrow;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox eptz_size_textbox_length;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkBoxCheckAck;
        private System.Windows.Forms.Button btnSimulateSound;
        private System.Windows.Forms.Label lblReceiveCnt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSendCnt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox combBoxAngles;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox agingTestTimeTextBox;
        private System.Windows.Forms.Button activateAgingTestButton;
        private System.Windows.Forms.Button clearAgingTestButton;
    }
}
