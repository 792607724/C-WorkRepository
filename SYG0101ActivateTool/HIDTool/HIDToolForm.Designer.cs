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
            this.acticateCurrentDevice_button = new System.Windows.Forms.Button();
            this.GetSNBtn = new System.Windows.Forms.Button();
            this.SetSNBtn = new System.Windows.Forms.Button();
            this.sngroup = new System.Windows.Forms.GroupBox();
            this.sn3 = new System.Windows.Forms.RadioButton();
            this.sn2 = new System.Windows.Forms.RadioButton();
            this.sn1 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.sngroup.SuspendLayout();
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
            this.label4.Size = new System.Drawing.Size(103, 19);
            this.label4.TabIndex = 55;
            this.label4.Text = "连接设备：";
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
            this.btnUpdateDeviceList.Text = "刷新设备列表";
            this.btnUpdateDeviceList.UseVisualStyleBackColor = true;
            this.btnUpdateDeviceList.Click += new System.EventHandler(this.btnUpdateDeviceList_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 15);
            this.label1.TabIndex = 65;
            this.label1.Text = "当前设备UUID：";
            // 
            // currentDeviceUUID_label
            // 
            this.currentDeviceUUID_label.AutoSize = true;
            this.currentDeviceUUID_label.Location = new System.Drawing.Point(268, 67);
            this.currentDeviceUUID_label.Name = "currentDeviceUUID_label";
            this.currentDeviceUUID_label.Size = new System.Drawing.Size(15, 15);
            this.currentDeviceUUID_label.TabIndex = 66;
            this.currentDeviceUUID_label.Text = "*";
            // 
            // getCurrentDeviceUUID_button
            // 
            this.getCurrentDeviceUUID_button.Location = new System.Drawing.Point(422, 59);
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
            this.getCurrentDeviceActivateStatus_button.Location = new System.Drawing.Point(422, 106);
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
            this.currentDeviceActivateStatus_label.Location = new System.Drawing.Point(268, 118);
            this.currentDeviceActivateStatus_label.Name = "currentDeviceActivateStatus_label";
            this.currentDeviceActivateStatus_label.Size = new System.Drawing.Size(15, 15);
            this.currentDeviceActivateStatus_label.TabIndex = 71;
            this.currentDeviceActivateStatus_label.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 15);
            this.label3.TabIndex = 70;
            this.label3.Text = "当前设备激活状态：";
            // 
            // acticateCurrentDevice_button
            // 
            this.acticateCurrentDevice_button.Location = new System.Drawing.Point(20, 155);
            this.acticateCurrentDevice_button.Name = "acticateCurrentDevice_button";
            this.acticateCurrentDevice_button.Size = new System.Drawing.Size(582, 35);
            this.acticateCurrentDevice_button.TabIndex = 72;
            this.acticateCurrentDevice_button.Text = "激活当前连接的设备";
            this.acticateCurrentDevice_button.UseVisualStyleBackColor = true;
            this.acticateCurrentDevice_button.Click += new System.EventHandler(this.acticateCurrentDevice_button_Click);
            // 
            // GetSNBtn
            // 
            this.GetSNBtn.Location = new System.Drawing.Point(189, 225);
            this.GetSNBtn.Margin = new System.Windows.Forms.Padding(4);
            this.GetSNBtn.Name = "GetSNBtn";
            this.GetSNBtn.Size = new System.Drawing.Size(413, 28);
            this.GetSNBtn.TabIndex = 73;
            this.GetSNBtn.Text = "读取整机序列号";
            this.GetSNBtn.UseVisualStyleBackColor = true;
            this.GetSNBtn.Click += new System.EventHandler(this.GetSNBtn_Click_1);
            // 
            // SetSNBtn
            // 
            this.SetSNBtn.Location = new System.Drawing.Point(189, 305);
            this.SetSNBtn.Margin = new System.Windows.Forms.Padding(4);
            this.SetSNBtn.Name = "SetSNBtn";
            this.SetSNBtn.Size = new System.Drawing.Size(413, 28);
            this.SetSNBtn.TabIndex = 75;
            this.SetSNBtn.Text = "写入整机序列号";
            this.SetSNBtn.UseVisualStyleBackColor = true;
            this.SetSNBtn.Click += new System.EventHandler(this.SetSNBtn_Click);
            // 
            // sngroup
            // 
            this.sngroup.Controls.Add(this.sn3);
            this.sngroup.Controls.Add(this.sn2);
            this.sngroup.Controls.Add(this.sn1);
            this.sngroup.Location = new System.Drawing.Point(20, 214);
            this.sngroup.Margin = new System.Windows.Forms.Padding(4);
            this.sngroup.Name = "sngroup";
            this.sngroup.Padding = new System.Windows.Forms.Padding(4);
            this.sngroup.Size = new System.Drawing.Size(149, 119);
            this.sngroup.TabIndex = 76;
            this.sngroup.TabStop = false;
            this.sngroup.Text = "整机序列号组";
            // 
            // sn3
            // 
            this.sn3.AutoSize = true;
            this.sn3.Location = new System.Drawing.Point(8, 88);
            this.sn3.Margin = new System.Windows.Forms.Padding(4);
            this.sn3.Name = "sn3";
            this.sn3.Size = new System.Drawing.Size(126, 19);
            this.sn3.TabIndex = 2;
            this.sn3.Text = "备用数据区块2";
            this.sn3.UseVisualStyleBackColor = true;
            // 
            // sn2
            // 
            this.sn2.AutoSize = true;
            this.sn2.Location = new System.Drawing.Point(8, 60);
            this.sn2.Margin = new System.Windows.Forms.Padding(4);
            this.sn2.Name = "sn2";
            this.sn2.Size = new System.Drawing.Size(126, 19);
            this.sn2.TabIndex = 1;
            this.sn2.Text = "备用数据区块1";
            this.sn2.UseVisualStyleBackColor = true;
            // 
            // sn1
            // 
            this.sn1.AutoSize = true;
            this.sn1.Checked = true;
            this.sn1.Location = new System.Drawing.Point(8, 32);
            this.sn1.Margin = new System.Windows.Forms.Padding(4);
            this.sn1.Name = "sn1";
            this.sn1.Size = new System.Drawing.Size(118, 19);
            this.sn1.TabIndex = 0;
            this.sn1.TabStop = true;
            this.sn1.Text = "默认数据区块";
            this.sn1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 359);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 19);
            this.label2.TabIndex = 77;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox1.Location = new System.Drawing.Point(189, 263);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(413, 30);
            this.textBox1.TabIndex = 78;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // HIDToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(492, 269);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sngroup);
            this.Controls.Add(this.SetSNBtn);
            this.Controls.Add(this.GetSNBtn);
            this.Controls.Add(this.acticateCurrentDevice_button);
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
            this.MaximumSize = new System.Drawing.Size(633, 384);
            this.MinimumSize = new System.Drawing.Size(633, 384);
            this.Name = "HIDToolForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "雅工激活写号工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HIDToolForm_FormClosing);
            this.sngroup.ResumeLayout(false);
            this.sngroup.PerformLayout();
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
        private System.Windows.Forms.Button acticateCurrentDevice_button;
        private System.Windows.Forms.Button GetSNBtn;
        private System.Windows.Forms.Button SetSNBtn;
        private System.Windows.Forms.GroupBox sngroup;
        private System.Windows.Forms.RadioButton sn3;
        private System.Windows.Forms.RadioButton sn2;
        private System.Windows.Forms.RadioButton sn1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
    }
}
