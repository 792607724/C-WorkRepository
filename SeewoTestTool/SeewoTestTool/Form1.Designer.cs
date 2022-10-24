namespace SeewoTestTool
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.device_reset_button = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.array_mic_audio_level_test_button = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.audioin2_result_label = new System.Windows.Forms.Label();
            this.audioin1_result_label = new System.Windows.Forms.Label();
            this.audioin2_test_button = new System.Windows.Forms.Button();
            this.audioin1_test_button = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.get_poe_mic_info_button = new System.Windows.Forms.Button();
            this.poe_mic_hardware_info_label = new System.Windows.Forms.Label();
            this.poe_mic_firmware_info_label = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.stop_rg_flicker_button = new System.Windows.Forms.Button();
            this.start_rg_flicker_button = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.upgrade_progressbar = new System.Windows.Forms.ProgressBar();
            this.upgrade_button = new System.Windows.Forms.Button();
            this.choose_upgrade_firmware_button = new System.Windows.Forms.Button();
            this.upgrade_firmware_textbox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.check_current_firmware_button = new System.Windows.Forms.Button();
            this.checked_firmware_textbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.getSeewoDevice = new System.Windows.Forms.Button();
            this.device_status_label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.device_disconnect_button = new System.Windows.Forms.Button();
            this.device_connect_button = new System.Windows.Forms.Button();
            this.device_port_textbox = new System.Windows.Forms.TextBox();
            this.device_ip_textbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.output_rich_textbox = new System.Windows.Forms.RichTextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.clear_output_button = new System.Windows.Forms.Button();
            this.backgroundworker_firmwareupgrade = new System.ComponentModel.BackgroundWorker();
            this.rememberCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.device_reset_button);
            this.groupBox1.Controls.Add(this.groupBox7);
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 476);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "希沃测试工具面板";
            // 
            // device_reset_button
            // 
            this.device_reset_button.Location = new System.Drawing.Point(234, 430);
            this.device_reset_button.Name = "device_reset_button";
            this.device_reset_button.Size = new System.Drawing.Size(527, 37);
            this.device_reset_button.TabIndex = 6;
            this.device_reset_button.Text = "设备复位";
            this.device_reset_button.UseVisualStyleBackColor = true;
            this.device_reset_button.Click += new System.EventHandler(this.device_reset_button_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label8);
            this.groupBox7.Controls.Add(this.array_mic_audio_level_test_button);
            this.groupBox7.Location = new System.Drawing.Point(228, 180);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(540, 244);
            this.groupBox7.TabIndex = 5;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "阵列MIC测试";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(179, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(164, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "各路MIC接收音频音量值展示";
            // 
            // array_mic_audio_level_test_button
            // 
            this.array_mic_audio_level_test_button.Location = new System.Drawing.Point(6, 201);
            this.array_mic_audio_level_test_button.Name = "array_mic_audio_level_test_button";
            this.array_mic_audio_level_test_button.Size = new System.Drawing.Size(527, 37);
            this.array_mic_audio_level_test_button.TabIndex = 0;
            this.array_mic_audio_level_test_button.Text = "阵列MIC音量值测试";
            this.array_mic_audio_level_test_button.UseVisualStyleBackColor = true;
            this.array_mic_audio_level_test_button.Click += new System.EventHandler(this.array_mic_audio_level_test_button_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.audioin2_result_label);
            this.groupBox6.Controls.Add(this.audioin1_result_label);
            this.groupBox6.Controls.Add(this.audioin2_test_button);
            this.groupBox6.Controls.Add(this.audioin1_test_button);
            this.groupBox6.Location = new System.Drawing.Point(14, 379);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 88);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Audio IN测试";
            // 
            // audioin2_result_label
            // 
            this.audioin2_result_label.AutoSize = true;
            this.audioin2_result_label.Location = new System.Drawing.Point(15, 57);
            this.audioin2_result_label.Name = "audioin2_result_label";
            this.audioin2_result_label.Size = new System.Drawing.Size(37, 17);
            this.audioin2_result_label.TabIndex = 3;
            this.audioin2_result_label.Text = "PASS";
            // 
            // audioin1_result_label
            // 
            this.audioin1_result_label.AutoSize = true;
            this.audioin1_result_label.Location = new System.Drawing.Point(15, 25);
            this.audioin1_result_label.Name = "audioin1_result_label";
            this.audioin1_result_label.Size = new System.Drawing.Size(37, 17);
            this.audioin1_result_label.TabIndex = 2;
            this.audioin1_result_label.Text = "PASS";
            // 
            // audioin2_test_button
            // 
            this.audioin2_test_button.Location = new System.Drawing.Point(79, 54);
            this.audioin2_test_button.Name = "audioin2_test_button";
            this.audioin2_test_button.Size = new System.Drawing.Size(115, 23);
            this.audioin2_test_button.TabIndex = 1;
            this.audioin2_test_button.Text = "Audio IN2口测试";
            this.audioin2_test_button.UseVisualStyleBackColor = true;
            this.audioin2_test_button.Click += new System.EventHandler(this.audioin2_test_button_Click);
            // 
            // audioin1_test_button
            // 
            this.audioin1_test_button.Location = new System.Drawing.Point(79, 22);
            this.audioin1_test_button.Name = "audioin1_test_button";
            this.audioin1_test_button.Size = new System.Drawing.Size(115, 23);
            this.audioin1_test_button.TabIndex = 0;
            this.audioin1_test_button.Text = "Audio IN1口测试";
            this.audioin1_test_button.UseVisualStyleBackColor = true;
            this.audioin1_test_button.Click += new System.EventHandler(this.audioin1_test_button_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.get_poe_mic_info_button);
            this.groupBox5.Controls.Add(this.poe_mic_hardware_info_label);
            this.groupBox5.Controls.Add(this.poe_mic_firmware_info_label);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Location = new System.Drawing.Point(14, 270);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 103);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "PoE 输出信息";
            // 
            // get_poe_mic_info_button
            // 
            this.get_poe_mic_info_button.Location = new System.Drawing.Point(7, 63);
            this.get_poe_mic_info_button.Name = "get_poe_mic_info_button";
            this.get_poe_mic_info_button.Size = new System.Drawing.Size(180, 31);
            this.get_poe_mic_info_button.TabIndex = 4;
            this.get_poe_mic_info_button.Text = "获取吊麦信息";
            this.get_poe_mic_info_button.UseVisualStyleBackColor = true;
            this.get_poe_mic_info_button.Click += new System.EventHandler(this.get_poe_mic_info_button_Click);
            // 
            // poe_mic_hardware_info_label
            // 
            this.poe_mic_hardware_info_label.AutoSize = true;
            this.poe_mic_hardware_info_label.Location = new System.Drawing.Point(105, 43);
            this.poe_mic_hardware_info_label.Name = "poe_mic_hardware_info_label";
            this.poe_mic_hardware_info_label.Size = new System.Drawing.Size(56, 17);
            this.poe_mic_hardware_info_label.TabIndex = 3;
            this.poe_mic_hardware_info_label.Text = "xxxxxxxx";
            // 
            // poe_mic_firmware_info_label
            // 
            this.poe_mic_firmware_info_label.AutoSize = true;
            this.poe_mic_firmware_info_label.Location = new System.Drawing.Point(105, 24);
            this.poe_mic_firmware_info_label.Name = "poe_mic_firmware_info_label";
            this.poe_mic_firmware_info_label.Size = new System.Drawing.Size(56, 17);
            this.poe_mic_firmware_info_label.TabIndex = 2;
            this.poe_mic_firmware_info_label.Text = "xxxxxxxx";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 17);
            this.label7.TabIndex = 1;
            this.label7.Text = "吊麦硬件型号：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "吊麦固件版本：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.stop_rg_flicker_button);
            this.groupBox4.Controls.Add(this.start_rg_flicker_button);
            this.groupBox4.Location = new System.Drawing.Point(14, 204);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 60);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "红绿指示灯控制";
            // 
            // stop_rg_flicker_button
            // 
            this.stop_rg_flicker_button.Location = new System.Drawing.Point(101, 22);
            this.stop_rg_flicker_button.Name = "stop_rg_flicker_button";
            this.stop_rg_flicker_button.Size = new System.Drawing.Size(93, 31);
            this.stop_rg_flicker_button.TabIndex = 1;
            this.stop_rg_flicker_button.Text = "关闭交替闪烁";
            this.stop_rg_flicker_button.UseVisualStyleBackColor = true;
            this.stop_rg_flicker_button.Click += new System.EventHandler(this.stop_rg_flicker_button_Click);
            // 
            // start_rg_flicker_button
            // 
            this.start_rg_flicker_button.Location = new System.Drawing.Point(3, 22);
            this.start_rg_flicker_button.Name = "start_rg_flicker_button";
            this.start_rg_flicker_button.Size = new System.Drawing.Size(93, 31);
            this.start_rg_flicker_button.TabIndex = 0;
            this.start_rg_flicker_button.Text = "打开交替闪烁";
            this.start_rg_flicker_button.UseVisualStyleBackColor = true;
            this.start_rg_flicker_button.Click += new System.EventHandler(this.start_rg_flicker_button_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.upgrade_progressbar);
            this.groupBox3.Controls.Add(this.upgrade_button);
            this.groupBox3.Controls.Add(this.choose_upgrade_firmware_button);
            this.groupBox3.Controls.Add(this.upgrade_firmware_textbox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.check_current_firmware_button);
            this.groupBox3.Controls.Add(this.checked_firmware_textbox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(228, 28);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(542, 144);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "固件升级校验";
            // 
            // upgrade_progressbar
            // 
            this.upgrade_progressbar.Location = new System.Drawing.Point(12, 76);
            this.upgrade_progressbar.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.upgrade_progressbar.Name = "upgrade_progressbar";
            this.upgrade_progressbar.Size = new System.Drawing.Size(521, 25);
            this.upgrade_progressbar.TabIndex = 7;
            // 
            // upgrade_button
            // 
            this.upgrade_button.Location = new System.Drawing.Point(12, 107);
            this.upgrade_button.Name = "upgrade_button";
            this.upgrade_button.Size = new System.Drawing.Size(521, 29);
            this.upgrade_button.TabIndex = 6;
            this.upgrade_button.Text = "开始升级";
            this.upgrade_button.UseVisualStyleBackColor = true;
            this.upgrade_button.Click += new System.EventHandler(this.upgrade_button_Click);
            // 
            // choose_upgrade_firmware_button
            // 
            this.choose_upgrade_firmware_button.Location = new System.Drawing.Point(421, 45);
            this.choose_upgrade_firmware_button.Name = "choose_upgrade_firmware_button";
            this.choose_upgrade_firmware_button.Size = new System.Drawing.Size(116, 29);
            this.choose_upgrade_firmware_button.TabIndex = 5;
            this.choose_upgrade_firmware_button.Text = "选择升级固件";
            this.choose_upgrade_firmware_button.UseVisualStyleBackColor = true;
            this.choose_upgrade_firmware_button.Click += new System.EventHandler(this.choose_upgrade_firmware_button_Click);
            // 
            // upgrade_firmware_textbox
            // 
            this.upgrade_firmware_textbox.Location = new System.Drawing.Point(119, 49);
            this.upgrade_firmware_textbox.Name = "upgrade_firmware_textbox";
            this.upgrade_firmware_textbox.Size = new System.Drawing.Size(295, 23);
            this.upgrade_firmware_textbox.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "固件升级：";
            // 
            // check_current_firmware_button
            // 
            this.check_current_firmware_button.Location = new System.Drawing.Point(420, 16);
            this.check_current_firmware_button.Name = "check_current_firmware_button";
            this.check_current_firmware_button.Size = new System.Drawing.Size(116, 29);
            this.check_current_firmware_button.TabIndex = 2;
            this.check_current_firmware_button.Text = "校验当前设备固件";
            this.check_current_firmware_button.UseVisualStyleBackColor = true;
            this.check_current_firmware_button.Click += new System.EventHandler(this.check_current_firmware_button_Click);
            // 
            // checked_firmware_textbox
            // 
            this.checked_firmware_textbox.Location = new System.Drawing.Point(119, 20);
            this.checked_firmware_textbox.Name = "checked_firmware_textbox";
            this.checked_firmware_textbox.Size = new System.Drawing.Size(295, 23);
            this.checked_firmware_textbox.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "请输入目标固件：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rememberCheckBox);
            this.groupBox2.Controls.Add(this.getSeewoDevice);
            this.groupBox2.Controls.Add(this.device_status_label);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.device_disconnect_button);
            this.groupBox2.Controls.Add(this.device_connect_button);
            this.groupBox2.Controls.Add(this.device_port_textbox);
            this.groupBox2.Controls.Add(this.device_ip_textbox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(14, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 170);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "连接设备";
            // 
            // getSeewoDevice
            // 
            this.getSeewoDevice.Location = new System.Drawing.Point(124, 103);
            this.getSeewoDevice.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.getSeewoDevice.Name = "getSeewoDevice";
            this.getSeewoDevice.Size = new System.Drawing.Size(61, 25);
            this.getSeewoDevice.TabIndex = 8;
            this.getSeewoDevice.Text = "刷新网口";
            this.getSeewoDevice.UseVisualStyleBackColor = true;
            this.getSeewoDevice.Click += new System.EventHandler(this.getSeewoDevice_Click);
            // 
            // device_status_label
            // 
            this.device_status_label.AutoSize = true;
            this.device_status_label.Location = new System.Drawing.Point(77, 107);
            this.device_status_label.Name = "device_status_label";
            this.device_status_label.Size = new System.Drawing.Size(44, 17);
            this.device_status_label.TabIndex = 7;
            this.device_status_label.Text = "已断开";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "设备状态：";
            // 
            // device_disconnect_button
            // 
            this.device_disconnect_button.Enabled = false;
            this.device_disconnect_button.Location = new System.Drawing.Point(112, 133);
            this.device_disconnect_button.Name = "device_disconnect_button";
            this.device_disconnect_button.Size = new System.Drawing.Size(75, 31);
            this.device_disconnect_button.TabIndex = 5;
            this.device_disconnect_button.Text = "断开设备";
            this.device_disconnect_button.UseVisualStyleBackColor = true;
            this.device_disconnect_button.Click += new System.EventHandler(this.device_disconnect_button_Click);
            // 
            // device_connect_button
            // 
            this.device_connect_button.Location = new System.Drawing.Point(6, 133);
            this.device_connect_button.Name = "device_connect_button";
            this.device_connect_button.Size = new System.Drawing.Size(75, 31);
            this.device_connect_button.TabIndex = 4;
            this.device_connect_button.Text = "连接设备";
            this.device_connect_button.UseVisualStyleBackColor = true;
            this.device_connect_button.Click += new System.EventHandler(this.device_connect_button_Click);
            // 
            // device_port_textbox
            // 
            this.device_port_textbox.Location = new System.Drawing.Point(87, 49);
            this.device_port_textbox.Name = "device_port_textbox";
            this.device_port_textbox.Size = new System.Drawing.Size(100, 23);
            this.device_port_textbox.TabIndex = 3;
            // 
            // device_ip_textbox
            // 
            this.device_ip_textbox.Location = new System.Drawing.Point(87, 20);
            this.device_ip_textbox.Name = "device_ip_textbox";
            this.device_ip_textbox.Size = new System.Drawing.Size(100, 23);
            this.device_ip_textbox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "设备端口号：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备IP地址：";
            // 
            // output_rich_textbox
            // 
            this.output_rich_textbox.Location = new System.Drawing.Point(6, 29);
            this.output_rich_textbox.Name = "output_rich_textbox";
            this.output_rich_textbox.Size = new System.Drawing.Size(765, 120);
            this.output_rich_textbox.TabIndex = 1;
            this.output_rich_textbox.Text = "";
            this.output_rich_textbox.TextChanged += new System.EventHandler(this.richTextChanged_to);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.clear_output_button);
            this.groupBox8.Controls.Add(this.output_rich_textbox);
            this.groupBox8.Location = new System.Drawing.Point(12, 494);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(776, 156);
            this.groupBox8.TabIndex = 2;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "输出：";
            // 
            // clear_output_button
            // 
            this.clear_output_button.Location = new System.Drawing.Point(725, 0);
            this.clear_output_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.clear_output_button.Name = "clear_output_button";
            this.clear_output_button.Size = new System.Drawing.Size(38, 27);
            this.clear_output_button.TabIndex = 2;
            this.clear_output_button.Text = "清空";
            this.clear_output_button.UseVisualStyleBackColor = true;
            this.clear_output_button.Click += new System.EventHandler(this.clear_output_button_Click);
            // 
            // backgroundworker_firmwareupgrade
            // 
            this.backgroundworker_firmwareupgrade.WorkerReportsProgress = true;
            this.backgroundworker_firmwareupgrade.WorkerSupportsCancellation = true;
            this.backgroundworker_firmwareupgrade.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundworker_firmwareupgrade_DoWork);
            this.backgroundworker_firmwareupgrade.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundworker_firmwareupgrade_ProgressChanged);
            this.backgroundworker_firmwareupgrade.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundworker_firmwareupgrade_RunWorkerCompleted);
            // 
            // rememberCheckBox
            // 
            this.rememberCheckBox.AutoSize = true;
            this.rememberCheckBox.Checked = true;
            this.rememberCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rememberCheckBox.Location = new System.Drawing.Point(7, 78);
            this.rememberCheckBox.Name = "rememberCheckBox";
            this.rememberCheckBox.Size = new System.Drawing.Size(158, 21);
            this.rememberCheckBox.TabIndex = 9;
            this.rememberCheckBox.Text = "记住当前IP地址和端口号";
            this.rememberCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 656);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(818, 695);
            this.MinimumSize = new System.Drawing.Size(818, 695);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Seewo测试工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingEvent);
            this.groupBox1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private TextBox device_ip_textbox;
        private TextBox device_port_textbox;
        private Button device_connect_button;
        private Button device_disconnect_button;
        private Label label3;
        private Label device_status_label;
        private GroupBox groupBox3;
        private Label label4;
        private TextBox checked_firmware_textbox;
        private Button check_current_firmware_button;
        private Label label5;
        private TextBox upgrade_firmware_textbox;
        private Button choose_upgrade_firmware_button;
        private Button upgrade_button;
        private GroupBox groupBox4;
        private Button start_rg_flicker_button;
        private Button stop_rg_flicker_button;
        private GroupBox groupBox5;
        private Label label6;
        private Label label7;
        private Label poe_mic_firmware_info_label;
        private Label poe_mic_hardware_info_label;
        private Button get_poe_mic_info_button;
        private GroupBox groupBox6;
        private Button audioin1_test_button;
        private Button audioin2_test_button;
        private Label audioin1_result_label;
        private Label audioin2_result_label;
        private GroupBox groupBox7;
        private Button array_mic_audio_level_test_button;
        private Label label8;
        private Button device_reset_button;
        private RichTextBox richTextBox1;
        private GroupBox groupBox8;
        private RichTextBox output_rich_textbox;
        private Button clear_output_button;
        private System.ComponentModel.BackgroundWorker backgroundworker_firmwareupgrade;
        private ProgressBar upgrade_progressbar;
        private Button getSeewoDevice;
        private CheckBox rememberCheckBox;
    }
}