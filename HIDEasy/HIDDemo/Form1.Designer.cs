namespace HIDDemo
{
    partial class Form1
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
            this.GetCameraModelBtn = new System.Windows.Forms.Button();
            this.GetVersionBtn = new System.Windows.Forms.Button();
            this.GetUpgradeMethodBtn = new System.Windows.Forms.Button();
            this.GetOTAKeyBtn = new System.Windows.Forms.Button();
            this.OpenEPTZBtn = new System.Windows.Forms.Button();
            this.CloseEPTZBtn = new System.Windows.Forms.Button();
            this.GetDeviceNameBtn = new System.Windows.Forms.Button();
            this.GetCPUTempBtn = new System.Windows.Forms.Button();
            this.RebootBootloaderBtn = new System.Windows.Forms.Button();
            this.GetAppVersionBtn = new System.Windows.Forms.Button();
            this.IsEPTZSupportedBtn = new System.Windows.Forms.Button();
            this.GetEPTZEnableBtn = new System.Windows.Forms.Button();
            this.IsCaptureSupportedBtn = new System.Windows.Forms.Button();
            this.StartRecordLogBtn = new System.Windows.Forms.Button();
            this.StopRecordLogBtn = new System.Windows.Forms.Button();
            this.GetRecordLogBtn = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // GetCameraModelBtn
            // 
            this.GetCameraModelBtn.Location = new System.Drawing.Point(20, 107);
            this.GetCameraModelBtn.Margin = new System.Windows.Forms.Padding(4);
            this.GetCameraModelBtn.Name = "GetCameraModelBtn";
            this.GetCameraModelBtn.Size = new System.Drawing.Size(204, 75);
            this.GetCameraModelBtn.TabIndex = 0;
            this.GetCameraModelBtn.Text = "获取模组信息";
            this.GetCameraModelBtn.UseVisualStyleBackColor = true;
            this.GetCameraModelBtn.Visible = false;
            this.GetCameraModelBtn.Click += new System.EventHandler(this.GetCameraModelBtn_Click);
            // 
            // GetVersionBtn
            // 
            this.GetVersionBtn.Location = new System.Drawing.Point(20, 13);
            this.GetVersionBtn.Margin = new System.Windows.Forms.Padding(4);
            this.GetVersionBtn.Name = "GetVersionBtn";
            this.GetVersionBtn.Size = new System.Drawing.Size(204, 75);
            this.GetVersionBtn.TabIndex = 1;
            this.GetVersionBtn.Text = "获取版本号";
            this.GetVersionBtn.UseVisualStyleBackColor = true;
            this.GetVersionBtn.Click += new System.EventHandler(this.GetVersionBtn_Click);
            // 
            // GetUpgradeMethodBtn
            // 
            this.GetUpgradeMethodBtn.Location = new System.Drawing.Point(20, 200);
            this.GetUpgradeMethodBtn.Margin = new System.Windows.Forms.Padding(4);
            this.GetUpgradeMethodBtn.Name = "GetUpgradeMethodBtn";
            this.GetUpgradeMethodBtn.Size = new System.Drawing.Size(204, 75);
            this.GetUpgradeMethodBtn.TabIndex = 2;
            this.GetUpgradeMethodBtn.Text = "获取升级方式";
            this.GetUpgradeMethodBtn.UseVisualStyleBackColor = true;
            this.GetUpgradeMethodBtn.Visible = false;
            this.GetUpgradeMethodBtn.Click += new System.EventHandler(this.GetUpgradeMethodBtn_Click);
            // 
            // GetOTAKeyBtn
            // 
            this.GetOTAKeyBtn.Location = new System.Drawing.Point(20, 294);
            this.GetOTAKeyBtn.Margin = new System.Windows.Forms.Padding(4);
            this.GetOTAKeyBtn.Name = "GetOTAKeyBtn";
            this.GetOTAKeyBtn.Size = new System.Drawing.Size(204, 75);
            this.GetOTAKeyBtn.TabIndex = 3;
            this.GetOTAKeyBtn.Text = "获取OTA Key";
            this.GetOTAKeyBtn.UseVisualStyleBackColor = true;
            this.GetOTAKeyBtn.Visible = false;
            this.GetOTAKeyBtn.Click += new System.EventHandler(this.GetOTAKeyBtn_Click);
            // 
            // OpenEPTZBtn
            // 
            this.OpenEPTZBtn.Location = new System.Drawing.Point(473, 200);
            this.OpenEPTZBtn.Margin = new System.Windows.Forms.Padding(4);
            this.OpenEPTZBtn.Name = "OpenEPTZBtn";
            this.OpenEPTZBtn.Size = new System.Drawing.Size(204, 75);
            this.OpenEPTZBtn.TabIndex = 4;
            this.OpenEPTZBtn.Text = "打开电子云台";
            this.OpenEPTZBtn.UseVisualStyleBackColor = true;
            this.OpenEPTZBtn.Visible = false;
            this.OpenEPTZBtn.Click += new System.EventHandler(this.OpenEPTZBtn_Click);
            // 
            // CloseEPTZBtn
            // 
            this.CloseEPTZBtn.Location = new System.Drawing.Point(473, 294);
            this.CloseEPTZBtn.Margin = new System.Windows.Forms.Padding(4);
            this.CloseEPTZBtn.Name = "CloseEPTZBtn";
            this.CloseEPTZBtn.Size = new System.Drawing.Size(204, 75);
            this.CloseEPTZBtn.TabIndex = 5;
            this.CloseEPTZBtn.Text = "关闭电子云台";
            this.CloseEPTZBtn.UseVisualStyleBackColor = true;
            this.CloseEPTZBtn.Visible = false;
            this.CloseEPTZBtn.Click += new System.EventHandler(this.CloseEPTZBtn_Click);
            // 
            // GetDeviceNameBtn
            // 
            this.GetDeviceNameBtn.Location = new System.Drawing.Point(248, 200);
            this.GetDeviceNameBtn.Margin = new System.Windows.Forms.Padding(4);
            this.GetDeviceNameBtn.Name = "GetDeviceNameBtn";
            this.GetDeviceNameBtn.Size = new System.Drawing.Size(204, 75);
            this.GetDeviceNameBtn.TabIndex = 6;
            this.GetDeviceNameBtn.Text = "获取设备名";
            this.GetDeviceNameBtn.UseVisualStyleBackColor = true;
            this.GetDeviceNameBtn.Visible = false;
            this.GetDeviceNameBtn.Click += new System.EventHandler(this.GetDeviceNameBtn_Click);
            // 
            // GetCPUTempBtn
            // 
            this.GetCPUTempBtn.Location = new System.Drawing.Point(248, 294);
            this.GetCPUTempBtn.Margin = new System.Windows.Forms.Padding(4);
            this.GetCPUTempBtn.Name = "GetCPUTempBtn";
            this.GetCPUTempBtn.Size = new System.Drawing.Size(204, 75);
            this.GetCPUTempBtn.TabIndex = 7;
            this.GetCPUTempBtn.Text = "获取CPU温度";
            this.GetCPUTempBtn.UseVisualStyleBackColor = true;
            this.GetCPUTempBtn.Visible = false;
            this.GetCPUTempBtn.Click += new System.EventHandler(this.GetCPUTempBtn_Click);
            // 
            // RebootBootloaderBtn
            // 
            this.RebootBootloaderBtn.Location = new System.Drawing.Point(248, 13);
            this.RebootBootloaderBtn.Margin = new System.Windows.Forms.Padding(4);
            this.RebootBootloaderBtn.Name = "RebootBootloaderBtn";
            this.RebootBootloaderBtn.Size = new System.Drawing.Size(204, 75);
            this.RebootBootloaderBtn.TabIndex = 8;
            this.RebootBootloaderBtn.Text = "启动Bootloader模式";
            this.RebootBootloaderBtn.UseVisualStyleBackColor = true;
            this.RebootBootloaderBtn.Click += new System.EventHandler(this.RebootBootloaderBtn_Click);
            // 
            // GetAppVersionBtn
            // 
            this.GetAppVersionBtn.Location = new System.Drawing.Point(248, 107);
            this.GetAppVersionBtn.Margin = new System.Windows.Forms.Padding(4);
            this.GetAppVersionBtn.Name = "GetAppVersionBtn";
            this.GetAppVersionBtn.Size = new System.Drawing.Size(204, 75);
            this.GetAppVersionBtn.TabIndex = 9;
            this.GetAppVersionBtn.Text = "获取APP版本号";
            this.GetAppVersionBtn.UseVisualStyleBackColor = true;
            this.GetAppVersionBtn.Visible = false;
            this.GetAppVersionBtn.Click += new System.EventHandler(this.GetAppVersionBtn_Click);
            // 
            // IsEPTZSupportedBtn
            // 
            this.IsEPTZSupportedBtn.Location = new System.Drawing.Point(473, 13);
            this.IsEPTZSupportedBtn.Margin = new System.Windows.Forms.Padding(4);
            this.IsEPTZSupportedBtn.Name = "IsEPTZSupportedBtn";
            this.IsEPTZSupportedBtn.Size = new System.Drawing.Size(204, 75);
            this.IsEPTZSupportedBtn.TabIndex = 10;
            this.IsEPTZSupportedBtn.Text = "是否支持电子云台";
            this.IsEPTZSupportedBtn.UseVisualStyleBackColor = true;
            this.IsEPTZSupportedBtn.Visible = false;
            this.IsEPTZSupportedBtn.Click += new System.EventHandler(this.IsEPTZSupportedBtn_Click);
            // 
            // GetEPTZEnableBtn
            // 
            this.GetEPTZEnableBtn.Location = new System.Drawing.Point(473, 107);
            this.GetEPTZEnableBtn.Margin = new System.Windows.Forms.Padding(4);
            this.GetEPTZEnableBtn.Name = "GetEPTZEnableBtn";
            this.GetEPTZEnableBtn.Size = new System.Drawing.Size(204, 75);
            this.GetEPTZEnableBtn.TabIndex = 11;
            this.GetEPTZEnableBtn.Text = "获取电子云台开关状态";
            this.GetEPTZEnableBtn.UseVisualStyleBackColor = true;
            this.GetEPTZEnableBtn.Visible = false;
            this.GetEPTZEnableBtn.Click += new System.EventHandler(this.GetEPTZEnableBtn_Click);
            // 
            // IsCaptureSupportedBtn
            // 
            this.IsCaptureSupportedBtn.Location = new System.Drawing.Point(685, 13);
            this.IsCaptureSupportedBtn.Margin = new System.Windows.Forms.Padding(4);
            this.IsCaptureSupportedBtn.Name = "IsCaptureSupportedBtn";
            this.IsCaptureSupportedBtn.Size = new System.Drawing.Size(204, 75);
            this.IsCaptureSupportedBtn.TabIndex = 12;
            this.IsCaptureSupportedBtn.Text = "是否支持48M拍照";
            this.IsCaptureSupportedBtn.UseVisualStyleBackColor = true;
            this.IsCaptureSupportedBtn.Visible = false;
            this.IsCaptureSupportedBtn.Click += new System.EventHandler(this.IsCaptureSupportedBtn_Click);
            // 
            // StartRecordLogBtn
            // 
            this.StartRecordLogBtn.Location = new System.Drawing.Point(685, 107);
            this.StartRecordLogBtn.Margin = new System.Windows.Forms.Padding(4);
            this.StartRecordLogBtn.Name = "StartRecordLogBtn";
            this.StartRecordLogBtn.Size = new System.Drawing.Size(204, 75);
            this.StartRecordLogBtn.TabIndex = 11;
            this.StartRecordLogBtn.Text = "开始录制log";
            this.StartRecordLogBtn.UseVisualStyleBackColor = true;
            this.StartRecordLogBtn.Visible = false;
            this.StartRecordLogBtn.Click += new System.EventHandler(this.StartRecordLogBtn_Click);
            // 
            // StopRecordLogBtn
            // 
            this.StopRecordLogBtn.Location = new System.Drawing.Point(685, 200);
            this.StopRecordLogBtn.Margin = new System.Windows.Forms.Padding(4);
            this.StopRecordLogBtn.Name = "StopRecordLogBtn";
            this.StopRecordLogBtn.Size = new System.Drawing.Size(204, 75);
            this.StopRecordLogBtn.TabIndex = 11;
            this.StopRecordLogBtn.Text = "停止录制log";
            this.StopRecordLogBtn.UseVisualStyleBackColor = true;
            this.StopRecordLogBtn.Visible = false;
            this.StopRecordLogBtn.Click += new System.EventHandler(this.StopRecordLogBtn_Click);
            // 
            // GetRecordLogBtn
            // 
            this.GetRecordLogBtn.Location = new System.Drawing.Point(685, 294);
            this.GetRecordLogBtn.Margin = new System.Windows.Forms.Padding(4);
            this.GetRecordLogBtn.Name = "GetRecordLogBtn";
            this.GetRecordLogBtn.Size = new System.Drawing.Size(204, 75);
            this.GetRecordLogBtn.TabIndex = 11;
            this.GetRecordLogBtn.Text = "获取log";
            this.GetRecordLogBtn.UseVisualStyleBackColor = true;
            this.GetRecordLogBtn.Visible = false;
            this.GetRecordLogBtn.Click += new System.EventHandler(this.GetRecordLogBtn_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 94);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(466, 23);
            this.progressBar1.TabIndex = 13;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(466, 117);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.IsCaptureSupportedBtn);
            this.Controls.Add(this.GetRecordLogBtn);
            this.Controls.Add(this.StopRecordLogBtn);
            this.Controls.Add(this.StartRecordLogBtn);
            this.Controls.Add(this.GetEPTZEnableBtn);
            this.Controls.Add(this.IsEPTZSupportedBtn);
            this.Controls.Add(this.GetAppVersionBtn);
            this.Controls.Add(this.RebootBootloaderBtn);
            this.Controls.Add(this.GetCPUTempBtn);
            this.Controls.Add(this.GetDeviceNameBtn);
            this.Controls.Add(this.CloseEPTZBtn);
            this.Controls.Add(this.OpenEPTZBtn);
            this.Controls.Add(this.GetOTAKeyBtn);
            this.Controls.Add(this.GetUpgradeMethodBtn);
            this.Controls.Add(this.GetVersionBtn);
            this.Controls.Add(this.GetCameraModelBtn);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HIDDemo V2.7";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GetCameraModelBtn;
        private System.Windows.Forms.Button GetVersionBtn;
        private System.Windows.Forms.Button GetUpgradeMethodBtn;
        private System.Windows.Forms.Button GetOTAKeyBtn;
        private System.Windows.Forms.Button OpenEPTZBtn;
        private System.Windows.Forms.Button CloseEPTZBtn;
        private System.Windows.Forms.Button GetDeviceNameBtn;
        private System.Windows.Forms.Button GetCPUTempBtn;
        private System.Windows.Forms.Button RebootBootloaderBtn;
        private System.Windows.Forms.Button GetAppVersionBtn;
        private System.Windows.Forms.Button IsEPTZSupportedBtn;
        private System.Windows.Forms.Button GetEPTZEnableBtn;
        private System.Windows.Forms.Button IsCaptureSupportedBtn;
        private System.Windows.Forms.Button StartRecordLogBtn;
        private System.Windows.Forms.Button StopRecordLogBtn;
        private System.Windows.Forms.Button GetRecordLogBtn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

