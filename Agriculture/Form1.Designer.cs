namespace BaiduAI
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.videoSourcePlayer1 = new AForge.Controls.VideoSourcePlayer();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackgroundImage = global::Smarthome.Properties.Resources._2;
            this.panel1.Controls.Add(this.textBox4);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.videoSourcePlayer1);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(12, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(630, 553);
            this.panel1.TabIndex = 7;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textBox4.ForeColor = System.Drawing.Color.Red;
            this.textBox4.Location = new System.Drawing.Point(268, 90);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(116, 23);
            this.textBox4.TabIndex = 19;
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(456, 24);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(113, 27);
            this.button6.TabIndex = 5;
            this.button6.Text = "重新检测设备";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // videoSourcePlayer1
            // 
            this.videoSourcePlayer1.Location = new System.Drawing.Point(112, 119);
            this.videoSourcePlayer1.Name = "videoSourcePlayer1";
            this.videoSourcePlayer1.Size = new System.Drawing.Size(422, 401);
            this.videoSourcePlayer1.TabIndex = 3;
            this.videoSourcePlayer1.Text = "videoSourcePlayer1";
            this.videoSourcePlayer1.VideoSource = null;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(354, 24);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(87, 27);
            this.button4.TabIndex = 2;
            this.button4.Text = "连接";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(188, 24);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(140, 22);
            this.comboBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(82, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "视频输入设备:";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(411, 624);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(87, 27);
            this.button8.TabIndex = 18;
            this.button8.Text = "登录";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(170, 624);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 27);
            this.button1.TabIndex = 20;
            this.button1.Text = "返回";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Smarthome.Properties.Resources._2;
            this.ClientSize = new System.Drawing.Size(654, 728);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button6;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}