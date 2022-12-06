namespace SXW0301_Production_line
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.vlcControl1 = new Vlc.DotNet.Forms.VlcControl();
            this.textBox1 = new Sunny.UI.UITextBox();
            this.label1 = new Sunny.UI.UILabel();
            this.button1 = new Sunny.UI.UIButton();
            this.button2 = new Sunny.UI.UIButton();
            this.panel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // vlcControl1
            // 
            this.vlcControl1.BackColor = System.Drawing.Color.Black;
            this.vlcControl1.Location = new System.Drawing.Point(4, 4);
            this.vlcControl1.Margin = new System.Windows.Forms.Padding(4);
            this.vlcControl1.Name = "vlcControl1";
            this.vlcControl1.Size = new System.Drawing.Size(973, 553);
            this.vlcControl1.Spu = -1;
            this.vlcControl1.TabIndex = 0;
            this.vlcControl1.Text = "vlcControl1";
            this.vlcControl1.VlcLibDirectory = null;
            this.vlcControl1.VlcMediaplayerOptions = null;
            this.vlcControl1.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcControl1_VlcLibDirectoryNeeded);
            this.vlcControl1.Click += new System.EventHandler(this.vlcControl1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(39, 569);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.MinimumSize = new System.Drawing.Size(1, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Radius = 20;
            this.textBox1.ShowText = false;
            this.textBox1.Size = new System.Drawing.Size(300, 41);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "rtsp://219.198.235.11/merge";
            this.textBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.textBox1.Watermark = "";
            this.textBox1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微软雅黑", 20F);
            this.label1.Location = new System.Drawing.Point(711, 564);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 43);
            this.label1.TabIndex = 1;
            this.label1.Text = "未检测";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(346, 567);
            this.button1.MinimumSize = new System.Drawing.Size(1, 1);
            this.button1.Name = "button1";
            this.button1.Radius = 20;
            this.button1.Size = new System.Drawing.Size(151, 43);
            this.button1.TabIndex = 1;
            this.button1.Text = "播放";
            this.button1.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(503, 567);
            this.button2.MinimumSize = new System.Drawing.Size(1, 1);
            this.button2.Name = "button2";
            this.button2.Radius = 20;
            this.button2.Size = new System.Drawing.Size(148, 43);
            this.button2.TabIndex = 1;
            this.button2.Text = "拍照检测";
            this.button2.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.vlcControl1);
            this.panel1.Location = new System.Drawing.Point(2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(973, 561);
            this.panel1.TabIndex = 1;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(979, 613);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(997, 660);
            this.MinimumSize = new System.Drawing.Size(997, 660);
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "测试工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingEvent);
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Vlc.DotNet.Forms.VlcControl vlcControl1;
        private Sunny.UI.UITextBox textBox1;
        private Sunny.UI.UIButton button1;
        private Sunny.UI.UIButton button2;
        private Sunny.UI.UILabel label1;
        private System.Windows.Forms.FlowLayoutPanel panel1;
    }
}