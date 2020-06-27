namespace Smarthome
{
    partial class Regist2
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
            this.button7 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.videoSourcePlayer1 = new AForge.Controls.VideoSourcePlayer();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
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
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.textBox5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.videoSourcePlayer1);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(587, 676);
            this.panel1.TabIndex = 8;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textBox4.ForeColor = System.Drawing.Color.Red;
            this.textBox4.Location = new System.Drawing.Point(238, 87);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(116, 23);
            this.textBox4.TabIndex = 19;
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(248, 602);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(87, 27);
            this.button7.TabIndex = 13;
            this.button7.Text = "人脸注册";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(238, 557);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(116, 23);
            this.textBox5.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(181, 560);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 16);
            this.label6.TabIndex = 11;
            this.label6.Text = "人脸ID";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(426, 21);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(113, 27);
            this.button6.TabIndex = 5;
            this.button6.Text = "重新检测设备";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // videoSourcePlayer1
            // 
            this.videoSourcePlayer1.Location = new System.Drawing.Point(82, 116);
            this.videoSourcePlayer1.Name = "videoSourcePlayer1";
            this.videoSourcePlayer1.Size = new System.Drawing.Size(422, 401);
            this.videoSourcePlayer1.TabIndex = 3;
            this.videoSourcePlayer1.Text = "videoSourcePlayer1";
            this.videoSourcePlayer1.VideoSource = null;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(324, 21);
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
            this.comboBox1.Location = new System.Drawing.Point(158, 21);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(140, 22);
            this.comboBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(52, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "视频输入设备:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(157, 706);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 27);
            this.button1.TabIndex = 20;
            this.button1.Text = "上一步";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(386, 706);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 27);
            this.button2.TabIndex = 21;
            this.button2.Text = "完成";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Regist2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Smarthome.Properties.Resources._2;
            this.ClientSize = new System.Drawing.Size(611, 756);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Name = "Regist2";
            this.Text = "Regist2";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Regist2_FormClosed);
            this.Load += new System.EventHandler(this.Regist2_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button6;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}