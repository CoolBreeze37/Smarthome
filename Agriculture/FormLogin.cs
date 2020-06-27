using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Smarthome
{
    public partial class FormLogin : Form
    {
        private User user;
        public FormLogin(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        /// <summary>
        /// 连接网关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                ClassAll.listgateway = new Gateway_New();
                ClassAll.listgateway.IP = txtIP.Text;
                ClassAll.listgateway.Port = TxtPort.Text;
                if (ClassAll.listgateway.Connect())
                {
                    FrmMain frmMain = new FrmMain(user);
                    frmMain.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("连接网关失败！请检查连接");
                }
            }
            catch 
            {
            }
        }

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}


        /// <summary>
        /// 加载数据 获取本机的ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormLogin_Load(object sender, EventArgs e)
        {
            try
            {
                string strHostName = Dns.GetHostName();
                IPHostEntry ipEntry = Dns.GetHostByName(strHostName);
                this.txtIP.Text = ipEntry.AddressList[0].ToString();
            }
            catch (Exception err)
            {
                this.txtIP.Text = "";
                MessageBox.Show(err.Message);
            }
        }
    }
}
