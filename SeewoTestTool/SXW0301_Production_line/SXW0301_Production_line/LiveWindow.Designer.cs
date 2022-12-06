namespace SXW0301_Production_line
{
    partial class LiveWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiveWindow));
            this.vlcControl1 = new Vlc.DotNet.Forms.VlcControl();
            this.textBox1 = new Sunny.UI.UITextBox();
            this.liveVideoPlay_button = new Sunny.UI.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // vlcControl1
            // 
            this.vlcControl1.BackColor = System.Drawing.Color.Black;
            this.vlcControl1.Location = new System.Drawing.Point(4, -1);
            this.vlcControl1.Margin = new System.Windows.Forms.Padding(4);
            this.vlcControl1.Name = "vlcControl1";
            this.vlcControl1.Size = new System.Drawing.Size(973, 553);
            this.vlcControl1.Spu = -1;
            this.vlcControl1.TabIndex = 1;
            this.vlcControl1.Text = "vlcControl1";
            this.vlcControl1.VlcLibDirectory = null;
            this.vlcControl1.VlcMediaplayerOptions = null;
            this.vlcControl1.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcControl1_VlcLibDirectoryNeeded);
            // 
            // textBox1
            // 
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(4, 561);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.MinimumSize = new System.Drawing.Size(1, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Radius = 20;
            this.textBox1.ShowText = false;
            this.textBox1.Size = new System.Drawing.Size(475, 41);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "rtsp://219.198.235.11/live";
            this.textBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.textBox1.Watermark = "";
            this.textBox1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // liveVideoPlay_button
            // 
            this.liveVideoPlay_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.liveVideoPlay_button.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.liveVideoPlay_button.Location = new System.Drawing.Point(486, 561);
            this.liveVideoPlay_button.MinimumSize = new System.Drawing.Size(1, 1);
            this.liveVideoPlay_button.Name = "liveVideoPlay_button";
            this.liveVideoPlay_button.Radius = 20;
            this.liveVideoPlay_button.Size = new System.Drawing.Size(481, 43);
            this.liveVideoPlay_button.TabIndex = 9;
            this.liveVideoPlay_button.Text = "播放";
            this.liveVideoPlay_button.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.liveVideoPlay_button.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.liveVideoPlay_button.Click += new System.EventHandler(this.liveVideoPlay_button_Click);
            // 
            // LiveWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 613);
            this.Controls.Add(this.liveVideoPlay_button);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.vlcControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(997, 660);
            this.MinimumSize = new System.Drawing.Size(997, 660);
            this.Name = "LiveWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Live视频出流";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingEvent);
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Vlc.DotNet.Forms.VlcControl vlcControl1;
        private Sunny.UI.UITextBox textBox1;
        private Sunny.UI.UIButton liveVideoPlay_button;
    }
}