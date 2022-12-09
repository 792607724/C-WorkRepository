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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HIDToolForm));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxDevices = new Sunny.UI.UIComboBox();
            this.btnUpdateDeviceList = new Sunny.UI.UIButton();
            this.getCurrentDeviceUUID_button = new Sunny.UI.UIButton();
            this.getCurrentDeviceActivateStatus_button = new Sunny.UI.UIButton();
            this.acticateCurrentDevice_button = new Sunny.UI.UIButton();
            this.uiRadioButtonGroup1 = new Sunny.UI.UIRadioButtonGroup();
            this.sn1 = new Sunny.UI.UIRadioButton();
            this.sn2 = new Sunny.UI.UIRadioButton();
            this.sn3 = new Sunny.UI.UIRadioButton();
            this.GetSNBtn = new Sunny.UI.UIButton();
            this.textBox1 = new Sunny.UI.UITextBox();
            this.SetSNBtn = new Sunny.UI.UIButton();
            this.label4 = new Sunny.UI.UILabel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.currentDeviceUUID_label = new Sunny.UI.UILabel();
            this.currentDeviceActivateStatus_label = new Sunny.UI.UILabel();
            this.uiRadioButtonGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 359);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 15);
            this.label2.TabIndex = 77;
            // 
            // comboBoxDevices
            // 
            this.comboBoxDevices.DataSource = null;
            this.comboBoxDevices.FillColor = System.Drawing.Color.White;
            this.comboBoxDevices.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.comboBoxDevices.Location = new System.Drawing.Point(151, 22);
            this.comboBoxDevices.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxDevices.MinimumSize = new System.Drawing.Size(63, 0);
            this.comboBoxDevices.Name = "comboBoxDevices";
            this.comboBoxDevices.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.comboBoxDevices.Size = new System.Drawing.Size(249, 25);
            this.comboBoxDevices.TabIndex = 79;
            this.comboBoxDevices.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxDevices.Watermark = "";
            this.comboBoxDevices.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // btnUpdateDeviceList
            // 
            this.btnUpdateDeviceList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdateDeviceList.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnUpdateDeviceList.Location = new System.Drawing.Point(422, 20);
            this.btnUpdateDeviceList.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnUpdateDeviceList.Name = "btnUpdateDeviceList";
            this.btnUpdateDeviceList.Size = new System.Drawing.Size(180, 31);
            this.btnUpdateDeviceList.TabIndex = 80;
            this.btnUpdateDeviceList.Text = "刷新设备列表";
            this.btnUpdateDeviceList.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdateDeviceList.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btnUpdateDeviceList.Click += new System.EventHandler(this.btnUpdateDeviceList_Click);
            // 
            // getCurrentDeviceUUID_button
            // 
            this.getCurrentDeviceUUID_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.getCurrentDeviceUUID_button.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.getCurrentDeviceUUID_button.Location = new System.Drawing.Point(422, 63);
            this.getCurrentDeviceUUID_button.MinimumSize = new System.Drawing.Size(1, 1);
            this.getCurrentDeviceUUID_button.Name = "getCurrentDeviceUUID_button";
            this.getCurrentDeviceUUID_button.Size = new System.Drawing.Size(180, 31);
            this.getCurrentDeviceUUID_button.TabIndex = 81;
            this.getCurrentDeviceUUID_button.Text = "获取当前设备UUID";
            this.getCurrentDeviceUUID_button.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.getCurrentDeviceUUID_button.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.getCurrentDeviceUUID_button.Click += new System.EventHandler(this.getCurrentDeviceUUID_button_Click);
            // 
            // getCurrentDeviceActivateStatus_button
            // 
            this.getCurrentDeviceActivateStatus_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.getCurrentDeviceActivateStatus_button.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.getCurrentDeviceActivateStatus_button.Location = new System.Drawing.Point(422, 107);
            this.getCurrentDeviceActivateStatus_button.MinimumSize = new System.Drawing.Size(1, 1);
            this.getCurrentDeviceActivateStatus_button.Name = "getCurrentDeviceActivateStatus_button";
            this.getCurrentDeviceActivateStatus_button.Size = new System.Drawing.Size(180, 31);
            this.getCurrentDeviceActivateStatus_button.TabIndex = 82;
            this.getCurrentDeviceActivateStatus_button.Text = "获取当前设备激活状态";
            this.getCurrentDeviceActivateStatus_button.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.getCurrentDeviceActivateStatus_button.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.getCurrentDeviceActivateStatus_button.Click += new System.EventHandler(this.getCurrentDeviceActivateStatus_button_Click);
            // 
            // acticateCurrentDevice_button
            // 
            this.acticateCurrentDevice_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.acticateCurrentDevice_button.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.acticateCurrentDevice_button.Location = new System.Drawing.Point(20, 155);
            this.acticateCurrentDevice_button.MinimumSize = new System.Drawing.Size(1, 1);
            this.acticateCurrentDevice_button.Name = "acticateCurrentDevice_button";
            this.acticateCurrentDevice_button.Size = new System.Drawing.Size(582, 31);
            this.acticateCurrentDevice_button.TabIndex = 83;
            this.acticateCurrentDevice_button.Text = "激活当前连接的设备";
            this.acticateCurrentDevice_button.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.acticateCurrentDevice_button.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.acticateCurrentDevice_button.Click += new System.EventHandler(this.acticateCurrentDevice_button_Click);
            // 
            // uiRadioButtonGroup1
            // 
            this.uiRadioButtonGroup1.Controls.Add(this.sn3);
            this.uiRadioButtonGroup1.Controls.Add(this.sn2);
            this.uiRadioButtonGroup1.Controls.Add(this.sn1);
            this.uiRadioButtonGroup1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.uiRadioButtonGroup1.ForeColor = System.Drawing.Color.Black;
            this.uiRadioButtonGroup1.Location = new System.Drawing.Point(20, 194);
            this.uiRadioButtonGroup1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiRadioButtonGroup1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiRadioButtonGroup1.Name = "uiRadioButtonGroup1";
            this.uiRadioButtonGroup1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiRadioButtonGroup1.Size = new System.Drawing.Size(161, 139);
            this.uiRadioButtonGroup1.Style = Sunny.UI.UIStyle.Custom;
            this.uiRadioButtonGroup1.TabIndex = 84;
            this.uiRadioButtonGroup1.Text = "整机序列号组";
            this.uiRadioButtonGroup1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiRadioButtonGroup1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // sn1
            // 
            this.sn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.sn1.Checked = true;
            this.sn1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sn1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.sn1.ForeColor = System.Drawing.Color.Black;
            this.sn1.Location = new System.Drawing.Point(8, 30);
            this.sn1.MinimumSize = new System.Drawing.Size(1, 1);
            this.sn1.Name = "sn1";
            this.sn1.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.sn1.Size = new System.Drawing.Size(131, 29);
            this.sn1.Style = Sunny.UI.UIStyle.Custom;
            this.sn1.TabIndex = 0;
            this.sn1.Text = "默认数据区块";
            this.sn1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // sn2
            // 
            this.sn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.sn2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sn2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.sn2.ForeColor = System.Drawing.Color.Black;
            this.sn2.Location = new System.Drawing.Point(8, 65);
            this.sn2.MinimumSize = new System.Drawing.Size(1, 1);
            this.sn2.Name = "sn2";
            this.sn2.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.sn2.Size = new System.Drawing.Size(131, 29);
            this.sn2.Style = Sunny.UI.UIStyle.Custom;
            this.sn2.TabIndex = 1;
            this.sn2.Text = "备用数据区块1";
            this.sn2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // sn3
            // 
            this.sn3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.sn3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sn3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.sn3.ForeColor = System.Drawing.Color.Black;
            this.sn3.Location = new System.Drawing.Point(8, 100);
            this.sn3.MinimumSize = new System.Drawing.Size(1, 1);
            this.sn3.Name = "sn3";
            this.sn3.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.sn3.Size = new System.Drawing.Size(131, 31);
            this.sn3.Style = Sunny.UI.UIStyle.Custom;
            this.sn3.TabIndex = 2;
            this.sn3.Text = "备用数据区块2";
            this.sn3.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // GetSNBtn
            // 
            this.GetSNBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.GetSNBtn.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.GetSNBtn.Location = new System.Drawing.Point(189, 211);
            this.GetSNBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.GetSNBtn.Name = "GetSNBtn";
            this.GetSNBtn.Size = new System.Drawing.Size(413, 31);
            this.GetSNBtn.TabIndex = 85;
            this.GetSNBtn.Text = "读取PCBA序列号";
            this.GetSNBtn.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GetSNBtn.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.GetSNBtn.Click += new System.EventHandler(this.GetSNBtn_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(189, 259);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.MinimumSize = new System.Drawing.Size(1, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.ShowText = false;
            this.textBox1.Size = new System.Drawing.Size(413, 29);
            this.textBox1.TabIndex = 86;
            this.textBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.textBox1.Watermark = "";
            this.textBox1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // SetSNBtn
            // 
            this.SetSNBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SetSNBtn.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.SetSNBtn.Location = new System.Drawing.Point(190, 302);
            this.SetSNBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.SetSNBtn.Name = "SetSNBtn";
            this.SetSNBtn.Size = new System.Drawing.Size(413, 31);
            this.SetSNBtn.TabIndex = 87;
            this.SetSNBtn.Text = "写入PCBA号";
            this.SetSNBtn.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SetSNBtn.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.SetSNBtn.Click += new System.EventHandler(this.SetSNBtn_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(15, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 32);
            this.label4.Style = Sunny.UI.UIStyle.Custom;
            this.label4.TabIndex = 88;
            this.label4.Text = "连接设备：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.uiLabel1.ForeColor = System.Drawing.Color.Black;
            this.uiLabel1.Location = new System.Drawing.Point(15, 63);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(144, 32);
            this.uiLabel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiLabel1.TabIndex = 89;
            this.uiLabel1.Text = "当前设备UUID：";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.uiLabel2.ForeColor = System.Drawing.Color.Black;
            this.uiLabel2.Location = new System.Drawing.Point(16, 107);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(144, 32);
            this.uiLabel2.Style = Sunny.UI.UIStyle.Custom;
            this.uiLabel2.TabIndex = 90;
            this.uiLabel2.Text = "当前设备激活状态：";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // currentDeviceUUID_label
            // 
            this.currentDeviceUUID_label.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.currentDeviceUUID_label.ForeColor = System.Drawing.Color.Black;
            this.currentDeviceUUID_label.Location = new System.Drawing.Point(151, 62);
            this.currentDeviceUUID_label.Name = "currentDeviceUUID_label";
            this.currentDeviceUUID_label.Size = new System.Drawing.Size(249, 32);
            this.currentDeviceUUID_label.Style = Sunny.UI.UIStyle.Custom;
            this.currentDeviceUUID_label.TabIndex = 91;
            this.currentDeviceUUID_label.Text = "*";
            this.currentDeviceUUID_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.currentDeviceUUID_label.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // currentDeviceActivateStatus_label
            // 
            this.currentDeviceActivateStatus_label.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.currentDeviceActivateStatus_label.ForeColor = System.Drawing.Color.Black;
            this.currentDeviceActivateStatus_label.Location = new System.Drawing.Point(151, 106);
            this.currentDeviceActivateStatus_label.Name = "currentDeviceActivateStatus_label";
            this.currentDeviceActivateStatus_label.Size = new System.Drawing.Size(249, 32);
            this.currentDeviceActivateStatus_label.Style = Sunny.UI.UIStyle.Custom;
            this.currentDeviceActivateStatus_label.TabIndex = 92;
            this.currentDeviceActivateStatus_label.Text = "*";
            this.currentDeviceActivateStatus_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.currentDeviceActivateStatus_label.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // HIDToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(615, 356);
            this.Controls.Add(this.currentDeviceActivateStatus_label);
            this.Controls.Add(this.currentDeviceUUID_label);
            this.Controls.Add(this.uiLabel2);
            this.Controls.Add(this.uiLabel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SetSNBtn);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.GetSNBtn);
            this.Controls.Add(this.uiRadioButtonGroup1);
            this.Controls.Add(this.acticateCurrentDevice_button);
            this.Controls.Add(this.getCurrentDeviceActivateStatus_button);
            this.Controls.Add(this.getCurrentDeviceUUID_button);
            this.Controls.Add(this.btnUpdateDeviceList);
            this.Controls.Add(this.comboBoxDevices);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(633, 403);
            this.MinimumSize = new System.Drawing.Size(633, 403);
            this.Name = "HIDToolForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "雅工激活写号工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HIDToolForm_FormClosing);
            this.uiRadioButtonGroup1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label2;
        private Sunny.UI.UIComboBox comboBoxDevices;
        private Sunny.UI.UIButton btnUpdateDeviceList;
        private Sunny.UI.UIButton getCurrentDeviceUUID_button;
        private Sunny.UI.UIButton getCurrentDeviceActivateStatus_button;
        private Sunny.UI.UIButton acticateCurrentDevice_button;
        private Sunny.UI.UIRadioButtonGroup uiRadioButtonGroup1;
        private Sunny.UI.UIRadioButton sn1;
        private Sunny.UI.UIRadioButton sn2;
        private Sunny.UI.UIRadioButton sn3;
        private Sunny.UI.UIButton GetSNBtn;
        private Sunny.UI.UITextBox textBox1;
        private Sunny.UI.UIButton SetSNBtn;
        private Sunny.UI.UILabel label4;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel currentDeviceUUID_label;
        private Sunny.UI.UILabel currentDeviceActivateStatus_label;
    }
}
