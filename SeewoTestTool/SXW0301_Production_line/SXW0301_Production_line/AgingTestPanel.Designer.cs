namespace SXW0301_Production_line
{
    partial class AgingTestPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgingTestPanel));
            this.liveVideoPlay_button = new Sunny.UI.UIButton();
            this.vlcControl1 = new Vlc.DotNet.Forms.VlcControl();
            this.vlcControl2 = new Vlc.DotNet.Forms.VlcControl();
            this.ip_textBox = new Sunny.UI.UITextBox();
            this.vlcControl3 = new Vlc.DotNet.Forms.VlcControl();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.stopAgingTest_button = new Sunny.UI.UIButton();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.volume5_value_label = new Sunny.UI.UILabel();
            this.volume1_value_label = new Sunny.UI.UILabel();
            this.uiLabel6 = new Sunny.UI.UILabel();
            this.volume6_value_label = new Sunny.UI.UILabel();
            this.uiLabel8 = new Sunny.UI.UILabel();
            this.volume4_value_label = new Sunny.UI.UILabel();
            this.uiLabel10 = new Sunny.UI.UILabel();
            this.volume8_value_label = new Sunny.UI.UILabel();
            this.uiLabel12 = new Sunny.UI.UILabel();
            this.volume2_value_label = new Sunny.UI.UILabel();
            this.uiLabel14 = new Sunny.UI.UILabel();
            this.audioIn1_label = new Sunny.UI.UILabel();
            this.uiLabel20 = new Sunny.UI.UILabel();
            this.audioIn2_label = new Sunny.UI.UILabel();
            this.uiLabel16 = new Sunny.UI.UILabel();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl3)).BeginInit();
            this.SuspendLayout();
            // 
            // liveVideoPlay_button
            // 
            this.liveVideoPlay_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.liveVideoPlay_button.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.liveVideoPlay_button.Location = new System.Drawing.Point(607, 664);
            this.liveVideoPlay_button.MinimumSize = new System.Drawing.Size(1, 1);
            this.liveVideoPlay_button.Name = "liveVideoPlay_button";
            this.liveVideoPlay_button.Radius = 19;
            this.liveVideoPlay_button.Size = new System.Drawing.Size(349, 30);
            this.liveVideoPlay_button.TabIndex = 12;
            this.liveVideoPlay_button.Text = "开始老化测试";
            this.liveVideoPlay_button.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.liveVideoPlay_button.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.liveVideoPlay_button.Click += new System.EventHandler(this.liveVideoPlay_button_Click);
            // 
            // vlcControl1
            // 
            this.vlcControl1.BackColor = System.Drawing.Color.Black;
            this.vlcControl1.Location = new System.Drawing.Point(2, 2);
            this.vlcControl1.Margin = new System.Windows.Forms.Padding(4);
            this.vlcControl1.Name = "vlcControl1";
            this.vlcControl1.Size = new System.Drawing.Size(484, 242);
            this.vlcControl1.Spu = -1;
            this.vlcControl1.TabIndex = 10;
            this.vlcControl1.Text = "vlcControl1";
            this.vlcControl1.VlcLibDirectory = null;
            this.vlcControl1.VlcMediaplayerOptions = null;
            this.vlcControl1.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcControl1_VlcLibDirectoryNeeded);
            // 
            // vlcControl2
            // 
            this.vlcControl2.BackColor = System.Drawing.Color.Black;
            this.vlcControl2.Location = new System.Drawing.Point(486, 2);
            this.vlcControl2.Margin = new System.Windows.Forms.Padding(4);
            this.vlcControl2.Name = "vlcControl2";
            this.vlcControl2.Size = new System.Drawing.Size(484, 242);
            this.vlcControl2.Spu = -1;
            this.vlcControl2.TabIndex = 13;
            this.vlcControl2.Text = "vlcControl2";
            this.vlcControl2.VlcLibDirectory = null;
            this.vlcControl2.VlcMediaplayerOptions = null;
            this.vlcControl2.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcControl1_VlcLibDirectoryNeeded);
            // 
            // ip_textBox
            // 
            this.ip_textBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ip_textBox.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.ip_textBox.Location = new System.Drawing.Point(148, 690);
            this.ip_textBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ip_textBox.MinimumSize = new System.Drawing.Size(1, 16);
            this.ip_textBox.Name = "ip_textBox";
            this.ip_textBox.Radius = 20;
            this.ip_textBox.ShowText = false;
            this.ip_textBox.Size = new System.Drawing.Size(338, 33);
            this.ip_textBox.TabIndex = 16;
            this.ip_textBox.Text = "219.198.235.11";
            this.ip_textBox.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.ip_textBox.Watermark = "";
            this.ip_textBox.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // vlcControl3
            // 
            this.vlcControl3.BackColor = System.Drawing.Color.Black;
            this.vlcControl3.Location = new System.Drawing.Point(2, 244);
            this.vlcControl3.Margin = new System.Windows.Forms.Padding(4);
            this.vlcControl3.Name = "vlcControl3";
            this.vlcControl3.Size = new System.Drawing.Size(968, 344);
            this.vlcControl3.Spu = -1;
            this.vlcControl3.TabIndex = 15;
            this.vlcControl3.Text = "vlcControl3";
            this.vlcControl3.VlcLibDirectory = null;
            this.vlcControl3.VlcMediaplayerOptions = null;
            this.vlcControl3.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcControl1_VlcLibDirectoryNeeded);
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel1.Location = new System.Drawing.Point(21, 685);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(100, 38);
            this.uiLabel1.TabIndex = 17;
            this.uiLabel1.Text = "当前设备IP：";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // stopAgingTest_button
            // 
            this.stopAgingTest_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.stopAgingTest_button.Enabled = false;
            this.stopAgingTest_button.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.stopAgingTest_button.Location = new System.Drawing.Point(607, 699);
            this.stopAgingTest_button.MinimumSize = new System.Drawing.Size(1, 1);
            this.stopAgingTest_button.Name = "stopAgingTest_button";
            this.stopAgingTest_button.Radius = 20;
            this.stopAgingTest_button.Size = new System.Drawing.Size(347, 29);
            this.stopAgingTest_button.TabIndex = 18;
            this.stopAgingTest_button.Text = "停止老化测试";
            this.stopAgingTest_button.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stopAgingTest_button.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.stopAgingTest_button.Click += new System.EventHandler(this.stopAgingTest_button_Click);
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel2.Location = new System.Drawing.Point(21, 655);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(465, 30);
            this.uiLabel2.TabIndex = 19;
            this.uiLabel2.Text = "提示：在进行老化测试时，请勿进行录音测试";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel3
            // 
            this.uiLabel3.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel3.Location = new System.Drawing.Point(21, 590);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(51, 38);
            this.uiLabel3.TabIndex = 20;
            this.uiLabel3.Text = "MIC1:";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel3.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // volume5_value_label
            // 
            this.volume5_value_label.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.volume5_value_label.Location = new System.Drawing.Point(78, 590);
            this.volume5_value_label.Name = "volume5_value_label";
            this.volume5_value_label.Size = new System.Drawing.Size(97, 38);
            this.volume5_value_label.TabIndex = 21;
            this.volume5_value_label.Text = "0";
            this.volume5_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.volume5_value_label.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // volume1_value_label
            // 
            this.volume1_value_label.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.volume1_value_label.Location = new System.Drawing.Point(241, 590);
            this.volume1_value_label.Name = "volume1_value_label";
            this.volume1_value_label.Size = new System.Drawing.Size(88, 38);
            this.volume1_value_label.TabIndex = 23;
            this.volume1_value_label.Text = "0";
            this.volume1_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.volume1_value_label.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel6
            // 
            this.uiLabel6.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel6.Location = new System.Drawing.Point(181, 590);
            this.uiLabel6.Name = "uiLabel6";
            this.uiLabel6.Size = new System.Drawing.Size(54, 38);
            this.uiLabel6.TabIndex = 22;
            this.uiLabel6.Text = "MIC2:";
            this.uiLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel6.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // volume6_value_label
            // 
            this.volume6_value_label.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.volume6_value_label.Location = new System.Drawing.Point(392, 590);
            this.volume6_value_label.Name = "volume6_value_label";
            this.volume6_value_label.Size = new System.Drawing.Size(94, 38);
            this.volume6_value_label.TabIndex = 25;
            this.volume6_value_label.Text = "0";
            this.volume6_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.volume6_value_label.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel8
            // 
            this.uiLabel8.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel8.Location = new System.Drawing.Point(335, 590);
            this.uiLabel8.Name = "uiLabel8";
            this.uiLabel8.Size = new System.Drawing.Size(51, 38);
            this.uiLabel8.TabIndex = 24;
            this.uiLabel8.Text = "MIC3:";
            this.uiLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel8.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // volume4_value_label
            // 
            this.volume4_value_label.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.volume4_value_label.Location = new System.Drawing.Point(867, 590);
            this.volume4_value_label.Name = "volume4_value_label";
            this.volume4_value_label.Size = new System.Drawing.Size(94, 38);
            this.volume4_value_label.TabIndex = 31;
            this.volume4_value_label.Text = "0";
            this.volume4_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.volume4_value_label.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel10
            // 
            this.uiLabel10.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel10.Location = new System.Drawing.Point(812, 590);
            this.uiLabel10.Name = "uiLabel10";
            this.uiLabel10.Size = new System.Drawing.Size(49, 38);
            this.uiLabel10.TabIndex = 30;
            this.uiLabel10.Text = "MIC6:";
            this.uiLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel10.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // volume8_value_label
            // 
            this.volume8_value_label.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.volume8_value_label.Location = new System.Drawing.Point(717, 590);
            this.volume8_value_label.Name = "volume8_value_label";
            this.volume8_value_label.Size = new System.Drawing.Size(89, 38);
            this.volume8_value_label.TabIndex = 29;
            this.volume8_value_label.Text = "0";
            this.volume8_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.volume8_value_label.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel12
            // 
            this.uiLabel12.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel12.Location = new System.Drawing.Point(658, 590);
            this.uiLabel12.Name = "uiLabel12";
            this.uiLabel12.Size = new System.Drawing.Size(53, 38);
            this.uiLabel12.TabIndex = 28;
            this.uiLabel12.Text = "MIC5:";
            this.uiLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel12.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // volume2_value_label
            // 
            this.volume2_value_label.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.volume2_value_label.Location = new System.Drawing.Point(555, 590);
            this.volume2_value_label.Name = "volume2_value_label";
            this.volume2_value_label.Size = new System.Drawing.Size(97, 38);
            this.volume2_value_label.TabIndex = 27;
            this.volume2_value_label.Text = "0";
            this.volume2_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.volume2_value_label.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel14
            // 
            this.uiLabel14.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel14.Location = new System.Drawing.Point(498, 590);
            this.uiLabel14.Name = "uiLabel14";
            this.uiLabel14.Size = new System.Drawing.Size(51, 38);
            this.uiLabel14.TabIndex = 26;
            this.uiLabel14.Text = "MIC4:";
            this.uiLabel14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel14.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // audioIn1_label
            // 
            this.audioIn1_label.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.audioIn1_label.Location = new System.Drawing.Point(144, 623);
            this.audioIn1_label.Name = "audioIn1_label";
            this.audioIn1_label.Size = new System.Drawing.Size(80, 38);
            this.audioIn1_label.TabIndex = 33;
            this.audioIn1_label.Text = "0";
            this.audioIn1_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.audioIn1_label.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel20
            // 
            this.uiLabel20.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel20.Location = new System.Drawing.Point(21, 623);
            this.uiLabel20.Name = "uiLabel20";
            this.uiLabel20.Size = new System.Drawing.Size(84, 38);
            this.uiLabel20.TabIndex = 32;
            this.uiLabel20.Text = "Audio IN1:";
            this.uiLabel20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel20.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // audioIn2_label
            // 
            this.audioIn2_label.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.audioIn2_label.Location = new System.Drawing.Point(458, 628);
            this.audioIn2_label.Name = "audioIn2_label";
            this.audioIn2_label.Size = new System.Drawing.Size(80, 28);
            this.audioIn2_label.TabIndex = 35;
            this.audioIn2_label.Text = "0";
            this.audioIn2_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.audioIn2_label.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel16
            // 
            this.uiLabel16.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel16.Location = new System.Drawing.Point(335, 623);
            this.uiLabel16.Name = "uiLabel16";
            this.uiLabel16.Size = new System.Drawing.Size(84, 38);
            this.uiLabel16.TabIndex = 34;
            this.uiLabel16.Text = "Audio IN2:";
            this.uiLabel16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel16.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // AgingTestPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 587);
            this.Controls.Add(this.audioIn2_label);
            this.Controls.Add(this.uiLabel16);
            this.Controls.Add(this.audioIn1_label);
            this.Controls.Add(this.uiLabel20);
            this.Controls.Add(this.volume4_value_label);
            this.Controls.Add(this.uiLabel10);
            this.Controls.Add(this.volume8_value_label);
            this.Controls.Add(this.uiLabel12);
            this.Controls.Add(this.volume2_value_label);
            this.Controls.Add(this.uiLabel14);
            this.Controls.Add(this.volume6_value_label);
            this.Controls.Add(this.uiLabel8);
            this.Controls.Add(this.volume1_value_label);
            this.Controls.Add(this.uiLabel6);
            this.Controls.Add(this.volume5_value_label);
            this.Controls.Add(this.uiLabel3);
            this.Controls.Add(this.uiLabel2);
            this.Controls.Add(this.stopAgingTest_button);
            this.Controls.Add(this.uiLabel1);
            this.Controls.Add(this.ip_textBox);
            this.Controls.Add(this.vlcControl3);
            this.Controls.Add(this.vlcControl2);
            this.Controls.Add(this.liveVideoPlay_button);
            this.Controls.Add(this.vlcControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(991, 781);
            this.MinimumSize = new System.Drawing.Size(991, 781);
            this.Name = "AgingTestPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "老化测试面板";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingEvent);
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIButton liveVideoPlay_button;
        private Vlc.DotNet.Forms.VlcControl vlcControl1;
        private Vlc.DotNet.Forms.VlcControl vlcControl2;
        private Sunny.UI.UITextBox ip_textBox;
        private Vlc.DotNet.Forms.VlcControl vlcControl3;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIButton stopAgingTest_button;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UILabel volume5_value_label;
        private Sunny.UI.UILabel volume1_value_label;
        private Sunny.UI.UILabel uiLabel6;
        private Sunny.UI.UILabel volume6_value_label;
        private Sunny.UI.UILabel uiLabel8;
        private Sunny.UI.UILabel volume4_value_label;
        private Sunny.UI.UILabel uiLabel10;
        private Sunny.UI.UILabel volume8_value_label;
        private Sunny.UI.UILabel uiLabel12;
        private Sunny.UI.UILabel volume2_value_label;
        private Sunny.UI.UILabel uiLabel14;
        private Sunny.UI.UILabel audioIn1_label;
        private Sunny.UI.UILabel uiLabel20;
        private Sunny.UI.UILabel audioIn2_label;
        private Sunny.UI.UILabel uiLabel16;
    }
}