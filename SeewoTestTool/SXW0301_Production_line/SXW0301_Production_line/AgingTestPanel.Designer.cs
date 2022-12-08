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
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl3)).BeginInit();
            this.SuspendLayout();
            // 
            // liveVideoPlay_button
            // 
            this.liveVideoPlay_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.liveVideoPlay_button.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.liveVideoPlay_button.Location = new System.Drawing.Point(491, 649);
            this.liveVideoPlay_button.MinimumSize = new System.Drawing.Size(1, 1);
            this.liveVideoPlay_button.Name = "liveVideoPlay_button";
            this.liveVideoPlay_button.Radius = 20;
            this.liveVideoPlay_button.Size = new System.Drawing.Size(470, 43);
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
            this.vlcControl1.Size = new System.Drawing.Size(484, 294);
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
            this.vlcControl2.Size = new System.Drawing.Size(484, 294);
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
            this.ip_textBox.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ip_textBox.Location = new System.Drawing.Point(195, 678);
            this.ip_textBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ip_textBox.MinimumSize = new System.Drawing.Size(1, 16);
            this.ip_textBox.Name = "ip_textBox";
            this.ip_textBox.Radius = 20;
            this.ip_textBox.ShowText = false;
            this.ip_textBox.Size = new System.Drawing.Size(240, 48);
            this.ip_textBox.TabIndex = 16;
            this.ip_textBox.Text = "219.198.235.11";
            this.ip_textBox.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ip_textBox.Watermark = "";
            this.ip_textBox.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // vlcControl3
            // 
            this.vlcControl3.BackColor = System.Drawing.Color.Black;
            this.vlcControl3.Location = new System.Drawing.Point(2, 296);
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
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(12, 678);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(145, 38);
            this.uiLabel1.TabIndex = 17;
            this.uiLabel1.Text = "当前设备IP：";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // stopAgingTest_button
            // 
            this.stopAgingTest_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.stopAgingTest_button.Enabled = false;
            this.stopAgingTest_button.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stopAgingTest_button.Location = new System.Drawing.Point(491, 700);
            this.stopAgingTest_button.MinimumSize = new System.Drawing.Size(1, 1);
            this.stopAgingTest_button.Name = "stopAgingTest_button";
            this.stopAgingTest_button.Radius = 20;
            this.stopAgingTest_button.Size = new System.Drawing.Size(470, 43);
            this.stopAgingTest_button.TabIndex = 18;
            this.stopAgingTest_button.Text = "停止老化测试";
            this.stopAgingTest_button.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stopAgingTest_button.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.stopAgingTest_button.Click += new System.EventHandler(this.stopAgingTest_button_Click);
            // 
            // AgingTestPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 749);
            this.Controls.Add(this.stopAgingTest_button);
            this.Controls.Add(this.uiLabel1);
            this.Controls.Add(this.ip_textBox);
            this.Controls.Add(this.vlcControl3);
            this.Controls.Add(this.vlcControl2);
            this.Controls.Add(this.liveVideoPlay_button);
            this.Controls.Add(this.vlcControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(991, 796);
            this.MinimumSize = new System.Drawing.Size(991, 796);
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
    }
}