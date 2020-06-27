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
    public partial class ForgetPwd : Form
    {

        public Form select_form;
        public ForgetPwd(Form select_form)
        {
            InitializeComponent();
            this.select_form = select_form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            select_form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool mark = true;
            if (textBox1.Text == "")
            {
                label5.Text = "不可为空!";
                mark = false;
            }
            else
                label5.Text = "";
            if (textBox2.Text == "")
            {
                label6.Text = "不可为空!";
                mark = false;
            }
            else
                label6.Text = "";
            if (textBox3.Text == "")
            {
                label7.Text = "不可为空!";
                mark = false;
            }
            else
                label7.Text = "";
            if(!mark)
                return;

            /*连接数据库验证身份信息*/
            ///<summary>
            ///数据库连接
            ///</summary>
            SqlConnection sqlConnection = new Sql().sql();
            sqlConnection.Open();
            //if(objConnection.State == ConnectionState.Open)
            //{
            //    label4.Text = "连接成功！";
            //}
            string sqlStr = string.Format("select * from [user] where 用户名='{0}' and 姓名='{1}' and 身份证号='{2}'", textBox1.Text,textBox2.Text,textBox3.Text);
            SqlCommand sqlCommand = new SqlCommand();
            Console.WriteLine(sqlStr);
            sqlCommand.CommandText = sqlStr;
            sqlCommand.Connection = sqlConnection;
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            User user = new User();
            if (sqlDataReader.Read())
            {
                label8.Text = "";
                user.uname = sqlDataReader[0].ToString();
                user.name = sqlDataReader[1].ToString();
                user.IDcard = sqlDataReader[2].ToString();
                user.pwd = sqlDataReader[3].ToString();
                user.FaceID = sqlDataReader.GetInt64(4);
                user.isadmin = sqlDataReader.GetBoolean(5);
                Console.WriteLine(user.uname);
                sqlConnection.Close();
            }
            else
            {
                label8.Text = "验证失败！信息有误！";
                return;
            }


            this.Hide();
            Form ForgetPwd2 = new ForgetPwd2(this, user); ;
            ForgetPwd2.Show();
        }

        private void ForgetPwd_Load(object sender, EventArgs e)
        {

        }

        private void ForgetPwd_FormClosed(object sender, FormClosedEventArgs e)
        {
            select_form.Show();
        }
    }
}
