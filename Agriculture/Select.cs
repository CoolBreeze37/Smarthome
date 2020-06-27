using BaiduAI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Smarthome
{
    public partial class Select : Form
    {
        public Select()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form form = new Form2(this);
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form form = new Form1(this);
            form.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form form = new Regist(this);
            form.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form form = new ForgetPwd(this);
            form.Show();
        }

        private void Select_Load(object sender, EventArgs e)
        {

        }
    }
}
