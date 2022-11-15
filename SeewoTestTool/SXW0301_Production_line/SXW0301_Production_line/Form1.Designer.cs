namespace SXW0301_Production_line
{
    partial class Fom1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fom1));
            this.vlcControl1 = new Vlc.DotNet.Forms.VlcControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.vlcControl2 = new Vlc.DotNet.Forms.VlcControl();
            this.textBox1 = new Sunny.UI.UITextBox();
            this.button2 = new Sunny.UI.UIButton();
            this.button1 = new Sunny.UI.UIButton();
            this.button3 = new Sunny.UI.UIButton();
            this.textBox2 = new Sunny.UI.UITextBox();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // vlcControl1
            // 
            this.vlcControl1.BackColor = System.Drawing.Color.Black;
            this.vlcControl1.Location = new System.Drawing.Point(0, 0);
            this.vlcControl1.Margin = new System.Windows.Forms.Padding(4);
            this.vlcControl1.Name = "vlcControl1";
            this.vlcControl1.Size = new System.Drawing.Size(743, 615);
            this.vlcControl1.Spu = -1;
            this.vlcControl1.TabIndex = 0;
            this.vlcControl1.Text = "vlcControl1";
            this.vlcControl1.VlcLibDirectory = null;
            this.vlcControl1.VlcMediaplayerOptions = null;
            this.vlcControl1.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcControl1_VlcLibDirectoryNeeded);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.vlcControl2);
            this.panel1.Controls.Add(this.vlcControl1);
            this.panel1.Location = new System.Drawing.Point(0, 15);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1485, 619);
            this.panel1.TabIndex = 6;
            // 
            // vlcControl2
            // 
            this.vlcControl2.BackColor = System.Drawing.Color.Black;
            this.vlcControl2.Location = new System.Drawing.Point(742, 0);
            this.vlcControl2.Margin = new System.Windows.Forms.Padding(4);
            this.vlcControl2.Name = "vlcControl2";
            this.vlcControl2.Size = new System.Drawing.Size(743, 615);
            this.vlcControl2.Spu = -1;
            this.vlcControl2.TabIndex = 1;
            this.vlcControl2.Text = "vlcControl2";
            this.vlcControl2.VlcLibDirectory = null;
            this.vlcControl2.VlcMediaplayerOptions = null;
            this.vlcControl2.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcControl2_VlcLibDirectoryNeeded);
            this.vlcControl2.Click += new System.EventHandler(this.vlcControl2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(4, 633);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.MinimumSize = new System.Drawing.Size(1, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.ShowText = false;
            this.textBox1.Size = new System.Drawing.Size(438, 60);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "rtsp://219.198.235.11/sec0";
            this.textBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.textBox1.Watermark = "";
            this.textBox1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(511, 637);
            this.button2.MinimumSize = new System.Drawing.Size(1, 1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 52);
            this.button2.TabIndex = 9;
            this.button2.Text = "播放sec0";
            this.button2.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(650, 641);
            this.button1.MinimumSize = new System.Drawing.Size(1, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(201, 52);
            this.button1.TabIndex = 2;
            this.button1.Text = "标定";
            this.button1.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.Location = new System.Drawing.Point(870, 637);
            this.button3.MinimumSize = new System.Drawing.Size(1, 1);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 52);
            this.button3.TabIndex = 2;
            this.button3.Text = "播放sec1";
            this.button3.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox2
            // 
            this.textBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(1039, 633);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox2.MinimumSize = new System.Drawing.Size(1, 16);
            this.textBox2.Name = "textBox2";
            this.textBox2.ShowText = false;
            this.textBox2.Size = new System.Drawing.Size(446, 60);
            this.textBox2.TabIndex = 2;
            this.textBox2.Text = "rtsp://219.198.235.11/sec1";
            this.textBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.textBox2.Watermark = "";
            this.textBox2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // Fom1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1489, 701);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(1507, 748);
            this.MinimumSize = new System.Drawing.Size(1507, 748);
            this.Name = "Fom1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标定";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Vlc.DotNet.Forms.VlcControl vlcControl1;
        private System.Windows.Forms.Panel panel1;
        private Vlc.DotNet.Forms.VlcControl vlcControl2;
        private Sunny.UI.UITextBox textBox1;
        private Sunny.UI.UIButton button2;
        private Sunny.UI.UIButton button1;
        private Sunny.UI.UIButton button3;
        private Sunny.UI.UITextBox textBox2;
    }
}

