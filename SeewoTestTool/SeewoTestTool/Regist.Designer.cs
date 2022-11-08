namespace SeewoTestTool
{
    partial class Regist
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
            this.onlineSoftwareUse_button = new System.Windows.Forms.Button();
            this.offlineSoftwareUse_button = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // onlineSoftwareUse_button
            // 
            this.onlineSoftwareUse_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.onlineSoftwareUse_button.Location = new System.Drawing.Point(11, 12);
            this.onlineSoftwareUse_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.onlineSoftwareUse_button.Name = "onlineSoftwareUse_button";
            this.onlineSoftwareUse_button.Size = new System.Drawing.Size(405, 358);
            this.onlineSoftwareUse_button.TabIndex = 0;
            this.onlineSoftwareUse_button.Text = "限制次数使用打开软件";
            this.onlineSoftwareUse_button.UseVisualStyleBackColor = true;
            this.onlineSoftwareUse_button.Click += new System.EventHandler(this.registAndActivateSoftware_button_Click);
            // 
            // offlineSoftwareUse_button
            // 
            this.offlineSoftwareUse_button.Location = new System.Drawing.Point(421, 143);
            this.offlineSoftwareUse_button.Name = "offlineSoftwareUse_button";
            this.offlineSoftwareUse_button.Size = new System.Drawing.Size(189, 41);
            this.offlineSoftwareUse_button.TabIndex = 1;
            this.offlineSoftwareUse_button.Text = "【开发者模式】显示隐私文件";
            this.offlineSoftwareUse_button.UseVisualStyleBackColor = true;
            this.offlineSoftwareUse_button.Click += new System.EventHandler(this.offlineSoftwareUse_button_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(421, 190);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(189, 41);
            this.button1.TabIndex = 2;
            this.button1.Text = "【开发者模式】隐藏隐私文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(421, 96);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(189, 41);
            this.button2.TabIndex = 3;
            this.button2.Text = "【开发者模式】激活使用次数";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(421, 67);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(189, 23);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(421, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "填写使用次数：";
            // 
            // Regist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 382);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.offlineSoftwareUse_button);
            this.Controls.Add(this.onlineSoftwareUse_button);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximumSize = new System.Drawing.Size(638, 421);
            this.MinimumSize = new System.Drawing.Size(638, 421);
            this.Name = "Regist";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Regist";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingEvent);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button onlineSoftwareUse_button;
        private Button offlineSoftwareUse_button;
        private Button button1;
        private Button button2;
        private TextBox textBox1;
        private Label label1;
    }
}