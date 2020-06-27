using BaiduAI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Smarthome
{
    public partial class Form2 : Form
    {
        public Form select_form;
        public Form2(Form select_form)
        {
            InitializeComponent();
            this.select_form = select_form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            select_form.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool mark = true;
            if (textBox1.Text == "")
            {
                label5.Text = "不能为空!";
                mark = false;
            }
            else { label5.Text = ""; }
            if(textBox2.Text=="")
            {
                label6.Text = "不能为空";
                mark = false;
            }
            else { label6.Text = ""; }
            if (mark == false)
                return;
            ///<summary>
            ///数据库连接
            ///</summary>
            SqlConnection sqlConnection = new Sql().sql();
            sqlConnection.Open();
            //if(objConnection.State == ConnectionState.Open)
            //{
            //    label4.Text = "连接成功！";
            //}
            string sqlStr = string.Format("select * from [user] where 用户名='{0}'", textBox1.Text);
            SqlCommand sqlCommand = new SqlCommand();

            sqlCommand.CommandText = sqlStr;
            sqlCommand.Connection = sqlConnection;
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if(sqlDataReader.Read())
            {
                User user = new User();
                user.uname = sqlDataReader[0].ToString();
                user.name = sqlDataReader[1].ToString();
                user.IDcard = sqlDataReader[2].ToString();
                user.pwd = sqlDataReader[3].ToString();
                user.FaceID = sqlDataReader.GetInt64(4);
                user.isadmin= sqlDataReader.GetBoolean(5);
                Console.WriteLine(user.uname);
                sqlConnection.Close();
                if (textBox1.Text.Equals(user.uname) && textBox2.Text.Equals(user.pwd))
                {
                    //this.Hide();
                    //Form Main = new Main(this, user);
                    //Main.ShowDialog();
                    MessageBox.Show("登陆成功");
                    this.Close();
                    Form form = new FormLogin(user);
                    form.Show();
                }
                else
                {
                    label3.Text = "密码错误！";
                }
            }
            else { label3.Text = "用户名不存在！"; }
            sqlConnection.Close();
        }
    }
}
