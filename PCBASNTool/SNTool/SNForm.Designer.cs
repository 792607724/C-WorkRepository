namespace SNTool
{
    partial class SNForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SetSNBtn = new System.Windows.Forms.Button();
            this.GetSNBtn = new System.Windows.Forms.Button();
            this.sngroup = new System.Windows.Forms.GroupBox();
            this.sn3 = new System.Windows.Forms.RadioButton();
            this.sn2 = new System.Windows.Forms.RadioButton();
            this.sn1 = new System.Windows.Forms.RadioButton();
            this.pcbagroup = new System.Windows.Forms.GroupBox();
            this.pcba3 = new System.Windows.Forms.RadioButton();
            this.pcba1 = new System.Windows.Forms.RadioButton();
            this.pcba2 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.sngroup.SuspendLayout();
            this.pcbagroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBox1.Location = new System.Drawing.Point(280, 42);
            this.textBox1.MaxLength = 17;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(376, 29);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.Visible = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // SetSNBtn
            // 
            this.SetSNBtn.Location = new System.Drawing.Point(713, 42);
            this.SetSNBtn.Name = "SetSNBtn";
            this.SetSNBtn.Size = new System.Drawing.Size(125, 35);
            this.SetSNBtn.TabIndex = 1;
            this.SetSNBtn.Text = "写入整机序列号";
            this.SetSNBtn.UseVisualStyleBackColor = true;
            this.SetSNBtn.Visible = false;
            this.SetSNBtn.Click += new System.EventHandler(this.SetSNBtn_Click);
            // 
            // GetSNBtn
            // 
            this.GetSNBtn.Location = new System.Drawing.Point(12, 38);
            this.GetSNBtn.Name = "GetSNBtn";
            this.GetSNBtn.Size = new System.Drawing.Size(116, 28);
            this.GetSNBtn.TabIndex = 2;
            this.GetSNBtn.Text = "读取整机序列号";
            this.GetSNBtn.UseVisualStyleBackColor = true;
            this.GetSNBtn.Visible = false;
            this.GetSNBtn.Click += new System.EventHandler(this.GetSNBtn_Click);
            // 
            // sngroup
            // 
            this.sngroup.Controls.Add(this.sn3);
            this.sngroup.Controls.Add(this.sn2);
            this.sngroup.Controls.Add(this.sn1);
            this.sngroup.Location = new System.Drawing.Point(140, 12);
            this.sngroup.Name = "sngroup";
            this.sngroup.Size = new System.Drawing.Size(112, 95);
            this.sngroup.TabIndex = 5;
            this.sngroup.TabStop = false;
            this.sngroup.Text = "整机序列号组";
            this.sngroup.Visible = false;
            // 
            // sn3
            // 
            this.sn3.AutoSize = true;
            this.sn3.Location = new System.Drawing.Point(6, 70);
            this.sn3.Name = "sn3";
            this.sn3.Size = new System.Drawing.Size(101, 16);
            this.sn3.TabIndex = 2;
            this.sn3.Text = "备用数据区块2";
            this.sn3.UseVisualStyleBackColor = true;
            this.sn3.CheckedChanged += new System.EventHandler(this.sn3_CheckedChanged);
            // 
            // sn2
            // 
            this.sn2.AutoSize = true;
            this.sn2.Location = new System.Drawing.Point(6, 48);
            this.sn2.Name = "sn2";
            this.sn2.Size = new System.Drawing.Size(101, 16);
            this.sn2.TabIndex = 1;
            this.sn2.Text = "备用数据区块1";
            this.sn2.UseVisualStyleBackColor = true;
            this.sn2.CheckedChanged += new System.EventHandler(this.sn2_CheckedChanged);
            // 
            // sn1
            // 
            this.sn1.AutoSize = true;
            this.sn1.Checked = true;
            this.sn1.Location = new System.Drawing.Point(6, 26);
            this.sn1.Name = "sn1";
            this.sn1.Size = new System.Drawing.Size(95, 16);
            this.sn1.TabIndex = 0;
            this.sn1.TabStop = true;
            this.sn1.Text = "默认数据区块";
            this.sn1.UseVisualStyleBackColor = true;
            this.sn1.CheckedChanged += new System.EventHandler(this.sn1_CheckedChanged);
            // 
            // pcbagroup
            // 
            this.pcbagroup.Controls.Add(this.pcba3);
            this.pcbagroup.Controls.Add(this.pcba1);
            this.pcbagroup.Controls.Add(this.pcba2);
            this.pcbagroup.Location = new System.Drawing.Point(134, 109);
            this.pcbagroup.Name = "pcbagroup";
            this.pcbagroup.Size = new System.Drawing.Size(112, 94);
            this.pcbagroup.TabIndex = 9;
            this.pcbagroup.TabStop = false;
            this.pcbagroup.Text = "PCBA序列号组";
            this.pcbagroup.Enter += new System.EventHandler(this.pcbagroup_Enter);
            // 
            // pcba3
            // 
            this.pcba3.AutoSize = true;
            this.pcba3.Location = new System.Drawing.Point(6, 70);
            this.pcba3.Name = "pcba3";
            this.pcba3.Size = new System.Drawing.Size(101, 16);
            this.pcba3.TabIndex = 2;
            this.pcba3.Text = "备用数据区块2";
            this.pcba3.UseVisualStyleBackColor = true;
            this.pcba3.CheckedChanged += new System.EventHandler(this.pcba3_CheckedChanged);
            // 
            // pcba1
            // 
            this.pcba1.AutoSize = true;
            this.pcba1.Checked = true;
            this.pcba1.Location = new System.Drawing.Point(6, 22);
            this.pcba1.Name = "pcba1";
            this.pcba1.Size = new System.Drawing.Size(95, 16);
            this.pcba1.TabIndex = 1;
            this.pcba1.TabStop = true;
            this.pcba1.Text = "默认数据区块";
            this.pcba1.UseVisualStyleBackColor = true;
            this.pcba1.CheckedChanged += new System.EventHandler(this.pcba1_CheckedChanged);
            // 
            // pcba2
            // 
            this.pcba2.AutoSize = true;
            this.pcba2.Location = new System.Drawing.Point(6, 44);
            this.pcba2.Name = "pcba2";
            this.pcba2.Size = new System.Drawing.Size(101, 16);
            this.pcba2.TabIndex = 0;
            this.pcba2.Text = "备用数据区块1";
            this.pcba2.UseVisualStyleBackColor = true;
            this.pcba2.CheckedChanged += new System.EventHandler(this.pcba2_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 28);
            this.button1.TabIndex = 8;
            this.button1.Text = "读取PCBA序列号";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(713, 145);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 28);
            this.button2.TabIndex = 7;
            this.button2.Text = "写PCBA列号";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBox2.Location = new System.Drawing.Point(280, 147);
            this.textBox2.MaxLength = 19;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(376, 26);
            this.textBox2.TabIndex = 6;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // SNForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 311);
            this.Controls.Add(this.pcbagroup);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.sngroup);
            this.Controls.Add(this.GetSNBtn);
            this.Controls.Add(this.SetSNBtn);
            this.Controls.Add(this.textBox1);
            this.MaximumSize = new System.Drawing.Size(900, 350);
            this.MinimumSize = new System.Drawing.Size(816, 350);
            this.Name = "SNForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PCBA序列号读写工具_V2.1";
            this.Load += new System.EventHandler(this.SNForm_Load);
            this.sngroup.ResumeLayout(false);
            this.sngroup.PerformLayout();
            this.pcbagroup.ResumeLayout(false);
            this.pcbagroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button SetSNBtn;
        private System.Windows.Forms.Button GetSNBtn;
        private System.Windows.Forms.GroupBox sngroup;
        private System.Windows.Forms.RadioButton sn3;
        private System.Windows.Forms.RadioButton sn2;
        private System.Windows.Forms.RadioButton sn1;
        private System.Windows.Forms.GroupBox pcbagroup;
        private System.Windows.Forms.RadioButton pcba3;
        private System.Windows.Forms.RadioButton pcba1;
        private System.Windows.Forms.RadioButton pcba2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
    }
}

