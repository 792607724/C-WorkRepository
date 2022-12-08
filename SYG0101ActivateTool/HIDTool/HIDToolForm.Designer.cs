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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.comboBoxDevices = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnUpdateDeviceList = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.currentDeviceUUID_label = new System.Windows.Forms.Label();
            this.getCurrentDeviceUUID_button = new System.Windows.Forms.Button();
            this.getCurrentDeviceActivateStatus_button = new System.Windows.Forms.Button();
            this.currentDeviceActivateStatus_label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // comboBoxDevices
            // 
            this.comboBoxDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDevices.FormattingEnabled = true;
            this.comboBoxDevices.Location = new System.Drawing.Point(165, 20);
            this.comboBoxDevices.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxDevices.Name = "comboBoxDevices";
            this.comboBoxDevices.Size = new System.Drawing.Size(242, 23);
            this.comboBoxDevices.TabIndex = 54;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 25);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 15);
            this.label4.TabIndex = 55;
            this.label4.Text = "接続デバイス：";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // btnUpdateDeviceList
            // 
            this.btnUpdateDeviceList.Location = new System.Drawing.Point(422, 17);
            this.btnUpdateDeviceList.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdateDeviceList.Name = "btnUpdateDeviceList";
            this.btnUpdateDeviceList.Size = new System.Drawing.Size(180, 30);
            this.btnUpdateDeviceList.TabIndex = 64;
            this.btnUpdateDeviceList.Text = "デバイスリストの更新";
            this.btnUpdateDeviceList.UseVisualStyleBackColor = true;
            this.btnUpdateDeviceList.Click += new System.EventHandler(this.btnUpdateDeviceList_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 15);
            this.label1.TabIndex = 65;
            this.label1.Text = "当前设备UUID：";
            // 
            // currentDeviceUUID_label
            // 
            this.currentDeviceUUID_label.AutoSize = true;
            this.currentDeviceUUID_label.Location = new System.Drawing.Point(268, 73);
            this.currentDeviceUUID_label.Name = "currentDeviceUUID_label";
            this.currentDeviceUUID_label.Size = new System.Drawing.Size(15, 15);
            this.currentDeviceUUID_label.TabIndex = 66;
            this.currentDeviceUUID_label.Text = "*";
            // 
            // getCurrentDeviceUUID_button
            // 
            this.getCurrentDeviceUUID_button.Location = new System.Drawing.Point(422, 65);
            this.getCurrentDeviceUUID_button.Margin = new System.Windows.Forms.Padding(4);
            this.getCurrentDeviceUUID_button.Name = "getCurrentDeviceUUID_button";
            this.getCurrentDeviceUUID_button.Size = new System.Drawing.Size(180, 30);
            this.getCurrentDeviceUUID_button.TabIndex = 68;
            this.getCurrentDeviceUUID_button.Text = "获取当前设备UUID";
            this.getCurrentDeviceUUID_button.UseVisualStyleBackColor = true;
            this.getCurrentDeviceUUID_button.Click += new System.EventHandler(this.getCurrentDeviceUUID_button_Click);
            // 
            // getCurrentDeviceActivateStatus_button
            // 
            this.getCurrentDeviceActivateStatus_button.Location = new System.Drawing.Point(422, 115);
            this.getCurrentDeviceActivateStatus_button.Margin = new System.Windows.Forms.Padding(4);
            this.getCurrentDeviceActivateStatus_button.Name = "getCurrentDeviceActivateStatus_button";
            this.getCurrentDeviceActivateStatus_button.Size = new System.Drawing.Size(180, 30);
            this.getCurrentDeviceActivateStatus_button.TabIndex = 69;
            this.getCurrentDeviceActivateStatus_button.Text = "获取当前设备激活状态";
            this.getCurrentDeviceActivateStatus_button.UseVisualStyleBackColor = true;
            this.getCurrentDeviceActivateStatus_button.Click += new System.EventHandler(this.getCurrentDeviceActivateStatus_button_Click);
            // 
            // currentDeviceActivateStatus_label
            // 
            this.currentDeviceActivateStatus_label.AutoSize = true;
            this.currentDeviceActivateStatus_label.Location = new System.Drawing.Point(268, 127);
            this.currentDeviceActivateStatus_label.Name = "currentDeviceActivateStatus_label";
            this.currentDeviceActivateStatus_label.Size = new System.Drawing.Size(15, 15);
            this.currentDeviceActivateStatus_label.TabIndex = 71;
            this.currentDeviceActivateStatus_label.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 19);
            this.label3.TabIndex = 70;
            this.label3.Text = "当前设备激活状态：";
            // 
            // HIDToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(615, 175);
            this.Controls.Add(this.currentDeviceActivateStatus_label);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.getCurrentDeviceActivateStatus_button);
            this.Controls.Add(this.getCurrentDeviceUUID_button);
            this.Controls.Add(this.currentDeviceUUID_label);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnUpdateDeviceList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxDevices);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(633, 222);
            this.MinimumSize = new System.Drawing.Size(633, 222);
            this.Name = "HIDToolForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "雅工活性化ツール";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HIDToolForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBoxDevices;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button btnUpdateDeviceList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label currentDeviceUUID_label;
        private System.Windows.Forms.Button getCurrentDeviceUUID_button;
        private System.Windows.Forms.Button getCurrentDeviceActivateStatus_button;
        private System.Windows.Forms.Label currentDeviceActivateStatus_label;
        private System.Windows.Forms.Label label3;
    }
}
