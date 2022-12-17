namespace API_TRAINING_PROJECT
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
            this.clickToPlayMusic_button = new Sunny.UI.UIButton();
            this.clickToStopMusic_button = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // clickToPlayMusic_button
            // 
            this.clickToPlayMusic_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clickToPlayMusic_button.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clickToPlayMusic_button.Location = new System.Drawing.Point(12, 12);
            this.clickToPlayMusic_button.MinimumSize = new System.Drawing.Size(1, 1);
            this.clickToPlayMusic_button.Name = "clickToPlayMusic_button";
            this.clickToPlayMusic_button.Size = new System.Drawing.Size(141, 65);
            this.clickToPlayMusic_button.TabIndex = 0;
            this.clickToPlayMusic_button.Text = "点击播放音乐";
            this.clickToPlayMusic_button.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clickToPlayMusic_button.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.clickToPlayMusic_button.Click += new System.EventHandler(this.clickToPlayMusic_button_Click);
            // 
            // clickToStopMusic_button
            // 
            this.clickToStopMusic_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clickToStopMusic_button.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clickToStopMusic_button.Location = new System.Drawing.Point(159, 12);
            this.clickToStopMusic_button.MinimumSize = new System.Drawing.Size(1, 1);
            this.clickToStopMusic_button.Name = "clickToStopMusic_button";
            this.clickToStopMusic_button.Size = new System.Drawing.Size(141, 65);
            this.clickToStopMusic_button.TabIndex = 1;
            this.clickToStopMusic_button.Text = "点击停止音乐";
            this.clickToStopMusic_button.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clickToStopMusic_button.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.clickToStopMusic_button.Click += new System.EventHandler(this.clickToStopMusic_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.clickToStopMusic_button);
            this.Controls.Add(this.clickToPlayMusic_button);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "API测试项目_SEEVISION";
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIButton clickToPlayMusic_button;
        private Sunny.UI.UIButton clickToStopMusic_button;
    }
}

