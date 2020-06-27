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
    public partial class Regist : Form
    {
        public Form select_form;
        public Regist(Form select_form)
        {
            InitializeComponent();
            this.select_form = select_form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            select_form.Show();
        }

        private void Regist_Load(object sender, EventArgs e)
        {

        }

        private void Regist_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                select_form.Show();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
      
            
            /*判断是否可以注册进入下一步*/
            bool mark = true;
            if (textBox1.Text == "")
            {
                label9.Text = "不可为空!";
                mark = false;
            }
            else
                label9.Text = "";
            if (textBox2.Text == "")
            {
                label10.Text = "不可为空!";
                mark = false;
            }
            else
                label10.Text = "";
            if (textBox3.Text == "")
            {
                label11.Text = "不可为空!";
                mark = false;
            }
            else
                label11.Text = "";
            if (textBox4.Text == "")
            {
                label12.Text = "不可为空!";
                mark = false;
            }
            else
                label12.Text = "";
            if (textBox5.Text!=textBox4.Text)
            {
                label13.Text = "两次密码不一致！";
                mark = false;
            }
            else
                label13.Text = "";
            if (!mark)
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
            string sqlStr = string.Format("insert into [user] values('{0}','{1}','{2}','{3}',default);", textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
            SqlCommand sqlCommand = new SqlCommand();
            Console.WriteLine(sqlStr);
            sqlCommand.CommandText = sqlStr;
            sqlCommand.Connection = sqlConnection;

            
            try
            {
                sqlCommand.ExecuteNonQuery();
                label15.Text = "";
                MessageBox.Show("注册成功！");
                sqlConnection.Close();
                
                button1.Enabled = false;
                User user = new User();
                //赋值操作//
                user.uname = textBox1.Text;
                user.name = textBox2.Text;
                user.IDcard = textBox3.Text;
                user.pwd = textBox4.Text;
                user.isadmin = false;


                SqlConnection sqlConnection1 = new Sql().sql();
                sqlConnection1.Open();


                sqlStr = string.Format("select 人脸ID from [user] where 用户名 = '{0}';",user.uname);
                Console.WriteLine(sqlStr);
                SqlCommand sqlCommand1 = new SqlCommand();
                sqlCommand1.CommandText = sqlStr;
                sqlCommand1.Connection = sqlConnection1;
                
                try
                {
                    SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Console.WriteLine("这里！");
                        user.FaceID = sqlDataReader.GetInt64(0);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }


                sqlConnection1.Close();
                this.Hide();
                Form form = new Regist2(this, user);
                form.Show();
                
            }
            catch
            {
                label15.Text = "用户名已存在！";
            };
            sqlConnection.Close();




            //end#####################




        }
    }
}
