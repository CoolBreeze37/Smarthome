using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using AForge.Video.DirectShow;

namespace Smarthome
{
    public partial class FrmMain : Form
    {

        private int count = 0;
        private User user;

        /// <summary>
        /// 继电器1地址设置状态
        /// </summary>
        private bool relay1Status;


        /// <summary>
        /// 智能控制值设置状态
        /// </summary>
        private bool smartValueStatus;

        /// <summary>
        /// 继电器2地址设置状态
        /// </summary>
        private bool relay2Status;

        private bool isSmartControl;
        /// <summary>
        /// 智能控制线程
        /// </summary>
        Thread thread;
        Thread thread1;
        bool State;

        private int hHwnd;
        private const int port = 2000;

        public delegate bool CallBack(int hwnd, int lParam);
        ///   <summary>   
        ///   必需的设计器变量。   
        ///   </summary>   
        //private System.ComponentModel.Container components = null;
        [DllImport("avicap32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int capCreateCaptureWindowA([MarshalAs(UnmanagedType.VBByRefStr)]   ref string lpszWindowName, int dwStyle, int x, int y, int nWidth, short nHeight, int hWndParent, int nID);
        [DllImport("avicap32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool capGetDriverDescriptionA(short wDriver, [MarshalAs(UnmanagedType.VBByRefStr)]   ref string lpszName, int cbName, [MarshalAs(UnmanagedType.VBByRefStr)]   ref string lpszVer, int cbVer);
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool DestroyWindow(int hndw);
        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SendMessage(int hwnd, int wMsg, int wParam, [MarshalAs(UnmanagedType.AsAny)]   object lParam);
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        [DllImport("vfw32.dll")]
        public static extern string capVideoStreamCallback(int hwnd, videohdr_tag videohdr_tag);
        [DllImport("vicap32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool capSetCallbackOnFrame(int hwnd, string s);

        /// <summary>
        /// 最大空气温度
        /// </summary>
        double MaxAirTemp = 35;
        /// <summary>
        /// 最小空气温度
        /// </summary>
        double MinAirTemp = 15;
        /// <summary>
        /// 最大空气湿度
        /// </summary>
        double MaxAirHumi = 50;
        /// <summary>
        /// 最小空气湿度
        /// </summary>
        double MinAirHumi = 20;
       
        /// <summary>
        /// 最大光照度
        /// </summary>
        double MaxIlluminance = 2500;
        /// <summary>
        /// 最小光照度
        /// </summary>
        double MinIlluminance = 200;
        /// <summary>
        /// 最大二氧化碳浓度
        /// </summary>
        double MaxC02Thickness = 700;
        /// <summary>
        /// 最小二氧化碳浓度
        /// </summary>
        double MinC02Thickness = 365;


        // 继电器设备状态
        private bool statusNTF;
        private bool statusWTF;
        private bool statusWZY;
        private bool statusNZY;
        private bool statusTC;
        private bool statusPG;
        private bool statusSL;




        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private int Indexof = 0;

        //添加所有的摄像头到combobox列表里
        private void Camlist()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
            {
                MessageBox.Show("未找到摄像头设备");
            }
            foreach (FilterInfo device in videoDevices)
            {
                comboBox1.Items.Add(device.Name);
                comboBox2.Items.Add(device.Name);
                comboBox3.Items.Add(device.Name);
                comboBox4.Items.Add(device.Name);
            }
        }
        //选择要调用的摄像头，捕获视频并展示到videoSourcePlayer1
        private void Device_Click(object sender, EventArgs e)
        {
            Indexof = comboBox1.SelectedIndex;
            if (Indexof < 0)
            {
                MessageBox.Show("请选择一个摄像头");
                return;
            }
            
            this.videoSourcePlayer1.Visible = true;
            //videoDevices[Indexof]确定出用哪个摄像头了。
            videoSource = new VideoCaptureDevice(videoDevices[Indexof].MonikerString);
            //设置下像素，这句话不写也可以正常运行：
            videoSource.VideoResolution = videoSource.VideoCapabilities[Indexof];
            //在videoSourcePlayer1里显示
            videoSourcePlayer1.VideoSource = videoSource;
            videoSourcePlayer1.Start();
        }
        private void Device_Click1(object sender, EventArgs e)
        {
            Indexof = comboBox2.SelectedIndex;
            if (Indexof < 0)
            {
                MessageBox.Show("请选择一个摄像头");
                return;
            }

            this.videoSourcePlayer2.Visible = true;
            //videoDevices[Indexof]确定出用哪个摄像头了。
            videoSource = new VideoCaptureDevice(videoDevices[Indexof].MonikerString);
            //设置下像素，这句话不写也可以正常运行：
            videoSource.VideoResolution = videoSource.VideoCapabilities[Indexof];
            //在videoSourcePlayer1里显示
            videoSourcePlayer2.VideoSource = videoSource;
            videoSourcePlayer2.Start();
        }
        private void Device_Click2(object sender, EventArgs e)
        {
            Indexof = comboBox3.SelectedIndex;
            if (Indexof < 0)
            {
                MessageBox.Show("请选择一个摄像头");
                return;
            }

            this.videoSourcePlayer3.Visible = true;
            //videoDevices[Indexof]确定出用哪个摄像头了。
            videoSource = new VideoCaptureDevice(videoDevices[Indexof].MonikerString);
            //设置下像素，这句话不写也可以正常运行：
            videoSource.VideoResolution = videoSource.VideoCapabilities[Indexof];
            //在videoSourcePlayer1里显示
            videoSourcePlayer3.VideoSource = videoSource;
            videoSourcePlayer3.Start();
        }
        private void Device_Click3(object sender, EventArgs e)
        {
            Indexof = comboBox4.SelectedIndex;
            if (Indexof < 0)
            {
                MessageBox.Show("请选择一个摄像头");
                return;
            }

            this.videoSourcePlayer4.Visible = true;
            //videoDevices[Indexof]确定出用哪个摄像头了。
            videoSource = new VideoCaptureDevice(videoDevices[Indexof].MonikerString);
            //设置下像素，这句话不写也可以正常运行：
            videoSource.VideoResolution = videoSource.VideoCapabilities[Indexof];
            //在videoSourcePlayer1里显示
            videoSourcePlayer4.VideoSource = videoSource;
            videoSourcePlayer4.Start();
        }



        public FrmMain(User user)
        {
            
            InitializeComponent();
            FrmMain.CheckForIllegalCrossThreadCalls = false;
            this.user = user;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            
            
            if(!user.isadmin)
            {
                tabControl2.TabPages.Remove(tabPage2);
            }

            // TODO: 这行代码将数据加载到表“usersDataSet.user”中。您可以根据需要移动或删除它。
            this.userTableAdapter.Fill(this.usersDataSet.user);
            textBox1.Text = user.uname;
            textBox2.Text = user.name;
            textBox3.Text = user.IDcard;
            textBox4.Text = user.FaceID.ToString();
            textBox5.Text = user.pwd;
            
            Camlist();
            webBrowser1.Navigate("https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=%E5%A4%A9%E6%B0%94%E9%A2%84%E6%8A%A5&fenlei=256&oq=sqlserver%25E5%2588%259B%25E5%25BB%25BA%25E7%259A%2584%25E6%2595%25B0%25E6%258D%25AE%25E5%25BA%2593%25E4%25BF%259D%25E5%25AD%2598%25E5%259C%25A8%25E5%2593%25AA%25E9%2587%258C&rsv_pq=ec3c27d6000031a7&rsv_t=9017czOuU8YxYqn4PDBIVqp3238thzEpcIZ2BEUusgQ6oZT%2B00jUm%2FEw8Sc&rqlang=cn&rsv_enter=1&rsv_dl=tb&rsv_sug3=14&rsv_sug1=7&rsv_sug7=101&rsv_sug2=0&rsv_btype=t&inputT=3170&rsv_sug4=3170");
            ClassAll.GetRelay();
            thread1 = new Thread(Getdata);
            thread1.IsBackground = true;
            thread1.Start();
            Thread.Sleep(100);
            txtaddr.Text = ClassAll.listgateway.RelayName1;
            txtaddr1.Text = ClassAll.listgateway.RelayName2;

            timer1.Start();
            timer2.Start();
            this.timer3.Start();
            //摄像头功能
            //  OpenCapture();

            // 获取当前继电器状态


        }


        public struct videohdr_tag
        {
            public byte[] lpData;
            public int dwBufferLength;
            public int dwBytesUsed;
            public int dwTimeCaptured;
            public int dwUser;
            public int dwFlags;
            public int[] dwReserved;

        }


        /// <summary>
        /// 连接网关
        /// </summary>
        public void Connect()
        {
            if (ClassAll.listgateway.Connect())
            {
                ClassAll.listgateway.Ping();
            }
            else
            {
                MessageBox.Show("网关连接失败！");
            }
        }

        /// <summary>
        /// 获取当前继电器状态
        /// </summary>
        public void GetState()
        {

            Thread.Sleep(500);

            //外遮阳
            if (ClassAll.listgateway.ExternalShading == "01")
            {
                btnWZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                // btnWZY.BackgroundImage = Image.FromFile(@"Resources\on.png");
                //  picWZY.Image = Image.FromFile(@"Resources\外遮阳开.png");
                picWZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("kaideng");
                btnWZY.Tag = 1;
            }
            else if (ClassAll.listgateway.ExternalShading == "00")
            {
                btnWZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                // btnWZY.BackgroundImage = Image.FromFile(@"Resources\off.png");
                //  picWZY.Image = Image.FromFile(@"Resources\外遮阳关.png");
                picWZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("guandeng");
                btnWZY.Tag = 2;
            }

            //内遮阳
            if (ClassAll.listgateway.InnerCurtain == "01")
            {
                btnNZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                //btnNZY.BackgroundImage = Image.FromFile(@"Resources\on.png");
                //picNZY.Image = Image.FromFile(@"Resources\内遮阳开.png");
                picNZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("chuangliankai");
                btnNZY.Tag = 1;
            }
            else if (ClassAll.listgateway.InnerCurtain == "00")
            {
                btnNZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                // btnNZY.BackgroundImage = Image.FromFile(@"Resources\off.png");
                //picNZY.Image = Image.FromFile(@"Resources\内遮阳关.png");
                picNZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("chuanglianguan");
                btnNZY.Tag = 2;
            }

            //外通风
            if (ClassAll.listgateway.SeparateVentilation == "01")
            {
                btnWTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                //btnWTF.BackgroundImage = Image.FromFile(@"Resources\on.png");
                picWTF.Enabled = true;
                btnWTF.Tag = 1;
            }
            else if (ClassAll.listgateway.SeparateVentilation == "00")
            {
                btnWTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                //btnWTF.BackgroundImage = Image.FromFile(@"Resources\off.png");
                picWTF.Enabled = false;
                btnWTF.Tag = 2;
            }

            //内通风
            if (ClassAll.listgateway.InternalVentilation == "01")
            {
                btnNTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                //  btnNTF.BackgroundImage = Image.FromFile(@"Resources\on.png");
                picNTF.Enabled = true;
                btnNTF.Tag = 1;
            }
            else if (ClassAll.listgateway.InternalVentilation == "00")
            {
                btnNTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                // btnNTF.BackgroundImage = Image.FromFile(@"Resources\off.png");
                picNTF.Enabled = false;
                btnNTF.Tag = 2;
            }

            //天窗
            if (ClassAll.listgateway.Scuttle == "01")
            {
                btnTC.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                // btnTC.BackgroundImage = Image.FromFile(@"Resources\on.png");
                //picTC.Image = Image.FromFile(@"Resources\天窗开.png");
                picTC.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("kaichuang");
                btnTC.Tag = 1;
            }
            else if (ClassAll.listgateway.Scuttle == "00")
            {
                btnTC.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                //btnTC.BackgroundImage = Image.FromFile(@"Resources\off.png");
                // picTC.Image = Image.FromFile(@"Resources\天窗关.png");
                picTC.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("guanchuang");
                btnTC.Tag = 2;
            }

            //喷灌
            if (ClassAll.listgateway.SprinklingIrrigation == "01")
            {
                btnPG.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                //btnPG.BackgroundImage = Image.FromFile(@"Resources\on.png");
                picPG.Enabled = true;
                btnPG.Tag = 1;
            }
            else if (ClassAll.listgateway.SprinklingIrrigation == "00")
            {
                btnPG.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                //btnPG.BackgroundImage = Image.FromFile(@"Resources\off.png");
                picPG.Enabled = false;
                btnPG.Tag = 2;
            }

            //水帘
            if (ClassAll.listgateway.Nappe == "01")
            {
                btnSL.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                // btnSL.BackgroundImage = Image.FromFile(@"Resources\on.png");
                picSL.Enabled = true;
                btnSL.Tag = 1;
            }
            else if (ClassAll.listgateway.Nappe == "00")
            {
                btnSL.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                // btnSL.BackgroundImage = Image.FromFile(@"Resources\off.png");
                picSL.Enabled = false;
                btnSL.Tag = 2;
            }



            Thread.Sleep(1);
            #region 废弃

            /*Thread.Sleep(2000);
            if (ClassAll.listgateway.Scuttle == "01")
            {
                btnTC.BackgroundImage = Image.FromFile(@"Resources\on.png");
                picTC.Image = Image.FromFile(@"Resources\天窗开.png");
                btnTC.Tag = 1;
            }
            else if (ClassAll.listgateway.Scuttle == "00")
            {
                btnTC.BackgroundImage = Image.FromFile(@"Resources\off.png");
                picTC.Image = Image.FromFile(@"Resources\天窗关.png");
                btnTC.Tag = 2;
            }
            if (ClassAll.listgateway.SprinklingIrrigation == "01")
            {
                btnPG.BackgroundImage = Image.FromFile(@"Resources\on.png");
                picPG.Enabled = true;
                btnPG.Tag = 1;
            }
            else if (ClassAll.listgateway.SprinklingIrrigation == "00")
            {
                btnPG.BackgroundImage = Image.FromFile(@"Resources\off.png");
                picPG.Enabled = false;
                btnPG.Tag = 2;
            }
  
            if (ClassAll.listgateway.Nappe == "01")
            {
                btnSL.BackgroundImage = Image.FromFile(@"Resources\on.png");
                picSL.Enabled = true;
                btnSL.Tag = 1;
            }
            else if (ClassAll.listgateway.Nappe == "00")
            {
                btnSL.BackgroundImage = Image.FromFile(@"Resources\off.png");
                picSL.Enabled = false;
                btnSL.Tag = 2;
            }
          
            if (ClassAll.listgateway.ExternalShading == "01")
            {
                btnWZY.BackgroundImage = Image.FromFile(@"Resources\on.png");
                picWTF.Enabled = true;
                btnWZY.Tag = 1;
            }
            else if (ClassAll.listgateway.ExternalShading == "00")
            {
                btnWZY.BackgroundImage = Image.FromFile(@"Resources\off.png");
                picWTF.Enabled = false;
                btnWZY.Tag = 2;
            }
            if (ClassAll.listgateway.InnerCurtain == "01")
            {
                btnNZY.BackgroundImage = Image.FromFile(@"Resources\on.png");
                picNTF.Enabled = true;
                btnNZY.Tag = 1;
            }
            else if (ClassAll.listgateway.InnerCurtain == "00")
            {
                btnNZY.BackgroundImage = Image.FromFile(@"Resources\off.png");
                picNTF.Enabled = false;
                btnNZY.Tag = 2;
            }


            if (ClassAll.listgateway.InnerCurtain == "01")
            {
                btnWTF.BackgroundImage = Image.FromFile(@"Resources\on.png");
                picNZY.Image = Image.FromFile(@"Resources\内遮阳开.png");
                btnWTF.Tag = 1;
            }
            else if (ClassAll.listgateway.InnerCurtain == "00")
            {
                btnWTF.BackgroundImage = Image.FromFile(@"Resources\off.png");
                picNZY.Image = Image.FromFile(@"Resources\内遮阳关.png");
                btnWTF.Tag = 2;
            }
            if (ClassAll.listgateway.ExternalShading == "01")
            {
                btnNTF.BackgroundImage = Image.FromFile(@"Resources\on.png");
                picWZY.Image = Image.FromFile(@"Resources\外遮阳开.png");
                btnNTF.Tag = 1;
            }
            else if (ClassAll.listgateway.ExternalShading == "00")
            {
                btnNTF.BackgroundImage = Image.FromFile(@"Resources\off.png");
                picWZY.Image = Image.FromFile(@"Resources\外遮阳关.png");
                btnNTF.Tag = 2;
            }*/

            #endregion
        }

        /// <summary>
        /// 获取当前环境数据
        /// </summary>
        public void Getdata()
        {

            while (true)
            {
                lblAirTemp.Text = ClassAll.listgateway.AirTemp;
                lblAirHumi.Text = ClassAll.listgateway.AirHumi;
                //lblSoilTemp.Text = ClassAll.listgateway.SoilTemp;
                //lblSoilHumi.Text = ClassAll.listgateway.SoilHumi;
                lblIlluminance.Text = ClassAll.listgateway.Illuminance;
                lblC02Thickness.Text = ClassAll.listgateway.C02Thickness;
                lblhasfire.Text = ClassAll.listgateway.Hasfire;
                Thread.Sleep(1000);
            }

        }

        /// <summary>
        /// 继电器编号
        /// </summary>
        /// <param name="i"></param>
        public void SendData(int i)
        {
            Thread.Sleep(100);
            if (ClassAll.listgateway.IsConnect)
            {
                string[] str = new string[2];

                if (i == 1)
                {
                    str[0] = ClassAll.listgateway.RelayName1;
                    str[1] = ClassAll.listgateway.ExternalShading + ClassAll.listgateway.InnerCurtain + ClassAll.listgateway.SeparateVentilation + ClassAll.listgateway.InternalVentilation + "FF";
                }
                else
                {
                    
                    str[0] = ClassAll.listgateway.RelayName2;
                    str[1] = ClassAll.listgateway.Scuttle + ClassAll.listgateway.SprinklingIrrigation + ClassAll.listgateway.Nappe + "00FF";
                    
                }
                Console.WriteLine(str[0]+"###############");
                ClassAll.listgateway.SendNodeData(str);
                Thread.Sleep(100);
            }
        }

        //智能控制
        public void SmartSet()
        {
            while (State)
            {
                // 空气温度
                if (Convert.ToDouble(ClassAll.listgateway.AirTemp) > MaxAirTemp)
                {
                    if (!statusTC)
                    {
                        statusTC = true;
                        ClassAll.listgateway.Scuttle = "01";
                        picTC.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("kaichuang");
                        // picTC.Image = Image.FromFile(@"Resources\天窗开.png");
                        btnTC.Tag = 1;
                        btnTC.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                        //   btnTC.BackgroundImage = Image.FromFile(@"Resources\on.png");
                    }


                }
                else if (Convert.ToDouble(ClassAll.listgateway.AirTemp) < MinAirTemp)
                {
                    if (statusTC)
                    {
                        statusTC = false;
                        ClassAll.listgateway.Scuttle = "00";
                        picTC.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("guanchuang");
                        //picTC.Image = Image.FromFile(@"Resources\天窗关.png");
                        btnTC.Tag = 2;
                        btnTC.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                        //  btnTC.BackgroundImage = Image.FromFile(@"Resources\off.png");
                    }


                }

                // 空气湿度
                if (Convert.ToDouble(ClassAll.listgateway.AirHumi) < MinAirHumi)
                {
                    if (!statusSL)
                    {
                        statusSL = true;
                        ClassAll.listgateway.Nappe = "01";
                        picSL.Enabled = true;
                        btnSL.Tag = 1;
                      //  btnSL.BackgroundImage = Image.FromFile(@"Resources\on.png");
                        btnSL.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    }


                }
                else if (Convert.ToDouble(ClassAll.listgateway.AirHumi) > MaxAirHumi)
                {
                    if (statusSL)
                    {
                        statusSL = false;
                        ClassAll.listgateway.Nappe = "00";
                        picSL.Enabled = false;
                        btnSL.Tag = 2;
                      //  btnSL.BackgroundImage = Image.FromFile(@"Resources\off.png");
                        btnSL.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    }
;

                }

                // 土壤温度
                //if (Convert.ToDouble(ClassAll.listgateway.SoilTemp) > MaxSoilTemp)
                //{
                //    if (!statusNTF)
                //    {
                //        statusNTF = true;
                //        ClassAll.listgateway.InternalVentilation = "01";
                //        picNTF.Enabled = true;
                //        btnNTF.Tag = 1;
                //        btnNTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                //        // btnNTF.BackgroundImage = Image.FromFile(@"Resources\on.png");
                //    }


                //}
                //else if (Convert.ToDouble(ClassAll.listgateway.SoilTemp) < MinSoilTemp)
                //{
                //    if (statusNTF)
                //    {
                //        statusNTF = false;
                //        ClassAll.listgateway.InternalVentilation = "00";
                //        picNTF.Enabled = false;
                //        btnNTF.Tag = 2;
                //        btnNTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                //        //  btnNTF.BackgroundImage = Image.FromFile(@"Resources\off.png");
                //    }

                //}

                //土壤湿度
                //if (Convert.ToDouble(ClassAll.listgateway.SoilHumi) < MinSoilHumi)
                //{
                //    if (!statusPG)
                //    {
                //        statusPG = true;
                //        ClassAll.listgateway.SprinklingIrrigation = "01";
                //        picPG.Enabled = true;
                //        btnPG.Tag = 1;
                //        btnPG.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                //        // btnPG.BackgroundImage = Image.FromFile(@"Resources\on.png");
                //    }


                //}
                //else if (Convert.ToDouble(ClassAll.listgateway.SoilHumi) > MaxSoilHumi)
                //{
                //    if (statusPG)
                //    {
                //        statusPG = false;
                //        ClassAll.listgateway.SprinklingIrrigation = "00";
                //        picPG.Enabled = false;
                //        btnPG.Tag = 2;
                //        btnPG.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                //        //   btnPG.BackgroundImage = Image.FromFile(@"Resources\off.png");
                //    }

                //}

                // 二氧化碳浓度
                if (Convert.ToDouble(ClassAll.listgateway.C02Thickness) > MaxC02Thickness)
                {
                    if (!statusWTF)
                    {
                        statusWTF = true;
                        ClassAll.listgateway.SeparateVentilation = "01";
                        picWTF.Enabled = true;
                        btnWTF.Tag = 1;
                        btnWTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                        //  btnWTF.BackgroundImage = Image.FromFile(@"Resources\on.png");
                    }


                }
                else if (Convert.ToDouble(ClassAll.listgateway.C02Thickness) < MinC02Thickness)
                {
                    if (statusWTF)
                    {
                        statusWTF = false;
                        ClassAll.listgateway.SeparateVentilation = "00";
                        picWTF.Enabled = false;
                        btnWTF.Tag = 2;
                        btnWTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                        //   btnWTF.BackgroundImage = Image.FromFile(@"Resources\off.png");
                    }

                }

                // 光照强度
                if (Convert.ToDouble(ClassAll.listgateway.Illuminance) < MinIlluminance)
                {
                    //if (!statusNZY)
                    //{
                    //    statusNZY = true;
                    //    ClassAll.listgateway.InnerCurtain = "01";
                    //    btnNZY.Tag = 1;
                    //    btnNZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    //    // btnNZY.BackgroundImage = Image.FromFile(@"Resources\on.png");
                    //    //  picNZY.Image = Image.FromFile(@"Resources\内遮阳开.png");
                    //    picNZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("chuangliankai");

                    //}
                    if (!statusWZY)
                    {
                        statusWZY = true;
                        ClassAll.listgateway.ExternalShading = "01";
                        btnWZY.Tag = 1;
                        btnWZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                        //  btnWZY.BackgroundImage = Image.FromFile(@"Resources\on.png");
                        // picWZY.Image = Image.FromFile(@"Resources\外遮阳开.png");
                        picWZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("kaideng");
                    }


                }
                else if (Convert.ToDouble(ClassAll.listgateway.Illuminance) > MaxIlluminance)
                {
                    //if (statusNZY)
                    //{
                    //    statusNZY = false;
                    //    ClassAll.listgateway.InnerCurtain = "00";

                    //    btnNZY.Tag = 2;
                    //    btnNZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    //    // btnNZY.BackgroundImage = Image.FromFile(@"Resources\off.png");
                    //    // picNZY.Image = Image.FromFile(@"Resources\内遮阳关.png");
                    //    picNZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("chuanglianguan");
                    //}
                    if (statusWZY)
                    {
                        statusWZY = false;
                        ClassAll.listgateway.ExternalShading = "00";
                        btnWZY.Tag = 2;
                        btnWZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                        // btnWZY.BackgroundImage = Image.FromFile(@"Resources\off.png");
                        //  picWZY.Image = Image.FromFile(@"Resources\外遮阳关.png");
                        picWZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("guandeng");
                    }

                }

                SendData(1);
                SendData(2);
                Thread.Sleep(100);
            }
        }


        private void btnGetDefault_Click(object sender, EventArgs e)
        {
            txtHMin.Text = MinAirHumi.ToString();
            txtHMax.Text = MaxAirHumi.ToString();
            txtTMin.Text = MinAirTemp.ToString();
            txtTMax.Text = MaxAirTemp.ToString();
            //txtSHMax.Text = MaxSoilHumi.ToString();
            //txtSHMin.Text = MinSoilHumi.ToString();
            //txtSTMax.Text = MaxSoilTemp.ToString();
            //txtSTMin.Text = MinSoilTemp.ToString();
            txtLuxMax.Text = MaxIlluminance.ToString();
            txtLuxMin.Text = MinIlluminance.ToString();
            txtPPMMax.Text = MaxC02Thickness.ToString();
            txtPPMMin.Text = MinC02Thickness.ToString();
        }


        /// <summary>
        /// 智能控制保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnkeep_Click(object sender, EventArgs e)
        {
            try
            {
                MinAirHumi = Convert.ToDouble(txtHMin.Text);
                MaxAirHumi = Convert.ToDouble(txtHMax.Text);
                MinAirTemp = Convert.ToDouble(txtTMin.Text);
                MaxAirTemp = Convert.ToDouble(txtTMax.Text);
                //MaxSoilHumi = Convert.ToDouble(txtSHMax.Text);
                //MinSoilHumi = Convert.ToDouble(txtSHMin.Text);
                //MaxSoilTemp = Convert.ToDouble(txtSTMax.Text);
                //MinSoilTemp = Convert.ToDouble(txtSTMin.Text);
                MaxC02Thickness = Convert.ToDouble(txtPPMMax.Text);
                MinC02Thickness = Convert.ToDouble(txtPPMMin.Text);
                MaxIlluminance = Convert.ToDouble(txtLuxMax.Text);
                MinIlluminance = Convert.ToDouble(txtLuxMin.Text);

                smartValueStatus = true;
                MessageBox.Show("保存成功！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show("数值输入错误！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// 手动控制中智能控制按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSmartSet_Click(object sender, EventArgs e)
        {
            if (smartValueStatus)
            {
                if (btnSmartSet.Tag.ToString() == "2")
                {
                    btnSmartSet.Tag = 1;
                    btnSmartSet.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    //btnSmartSet.BackgroundImage = Image.FromFile(@"Resources\on.png");
                    State = true;
                    thread = new Thread(SmartSet);
                    thread.IsBackground = true;
                    thread.Start();
                    isSmartControl = true;
                }
                else
                {
                    btnSmartSet.Tag = 2;
                    btnSmartSet.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    // btnSmartSet.BackgroundImage = Image.FromFile(@"Resources\off.png");
                    State = false;
                    thread.Abort();
                    isSmartControl = false;
                }
            }
            else
            {
                MessageBox.Show("请设置智能控制阈值！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        /// <summary>
        ///灯泡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWZY_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnWZY.Tag.ToString() == "2")
                {
                    ClassAll.listgateway.ExternalShading = "01";
                    btnWZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    //btnWZY.BackgroundImage = Image.FromFile(@"Resources\on.png");
                    // picWZY.Image = Image.FromFile(@"Resources\外遮阳开.png");
                    picWZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("kaideng");
                    btnWZY.Tag = 1;
                }
                else
                {
                    ClassAll.listgateway.ExternalShading = "00";
                    btnWZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    // btnWZY.BackgroundImage = Image.FromFile(@"Resources\off.png");
                    //  picWZY.Image = Image.FromFile(@"Resources\外遮阳关.png");
                    picWZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("guandeng");
                    btnWZY.Tag = 2;
                }
                SendData(1);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        /// <summary>
        /// 窗帘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNZY_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnNZY.Tag.ToString() == "2")
                {
                    ClassAll.listgateway.InnerCurtain = "01";
                    btnNZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on"); 
                   // btnNZY.BackgroundImage = Image.FromFile(@"Resources\on.png");
                  //  picNZY.Image = Image.FromFile(@"Resources\内遮阳开.png");
                    picNZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("chuangliankai");
                    btnNZY.Tag = 1;
                    statusNZY = true;
                }
                else
                {
                    ClassAll.listgateway.InnerCurtain = "00";
                    btnNZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    // btnNZY.BackgroundImage = Image.FromFile(@"Resources\off.png");
                    // picNZY.Image = Image.FromFile(@"Resources\内遮阳关.png");
                     picNZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("chuanglianguan");
                    btnNZY.Tag = 2;
                    statusNZY = false;
                }
                SendData(1);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        /// <summary>
        /// 外通风
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWTF_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnWTF.Tag.ToString() == "2")
                {
                    ClassAll.listgateway.SeparateVentilation = "01";
                    btnWTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    // btnWTF.BackgroundImage = Image.FromFile(@"Resources\on.png");
                    picWTF.Enabled = true;
                    statusWTF = true;
                    btnWTF.Tag = 1;
                }
                else
                {
                    ClassAll.listgateway.SeparateVentilation = "00";
                    btnWTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    // btnWTF.BackgroundImage = Image.FromFile(@"Resources\off.png");
                    picWTF.Enabled = false;
                    statusWTF = false;
                    btnWTF.Tag = 2;
                }
                SendData(1);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 洗衣机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNTF_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnNTF.Tag.ToString() == "2")
                {
                    ClassAll.listgateway.InternalVentilation = "01";
                    btnNTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    //  btnNTF.BackgroundImage = Image.FromFile(@"Resources\on.png");
                    picNTF.Enabled = true;
                    statusNTF = true;
                    btnNTF.Tag = 1;
                }
                else
                {
                    ClassAll.listgateway.InternalVentilation = "00";
                    btnNTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    //   btnNTF.BackgroundImage = Image.FromFile(@"Resources\off.png");
                    picNTF.Enabled = false;
                    statusNTF = false;
                    btnNTF.Tag = 2;
                }
                SendData(1);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        /// <summary>
        /// 窗户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTC_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnTC.Tag.ToString() == "2")
                {
                    ClassAll.listgateway.Scuttle = "01";
                    btnTC.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    // btnTC.BackgroundImage = Image.FromFile(@"Resources\on.png");
                    //   picTC.Image = Image.FromFile(@"Resources\天窗开.png");
                    picTC.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("kaichuang");
                    statusTC = true;
                    btnTC.Tag = 1;
                }
                else
                {
                    ClassAll.listgateway.Scuttle = "00";
                    btnTC.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    //btnTC.BackgroundImage = Image.FromFile(@"Resources\off.png");
                    //picTC.Image = Image.FromFile(@"Resources\天窗关.png");
                    picTC.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("guanchuang");
                    statusTC = false;
                    btnTC.Tag = 2;
                }
                SendData(2);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 灭火
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPG_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnPG.Tag.ToString() == "2")
                {
                    ClassAll.listgateway.SprinklingIrrigation = "01";
                    btnPG.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    // btnPG.BackgroundImage = Image.FromFile(@"Resources\on.png");
                    picPG.Enabled = true;
                    statusPG = true;
                    btnPG.Tag = 1;
                }
                else
                {
                    ClassAll.listgateway.SprinklingIrrigation = "00";
                    btnPG.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    // btnPG.BackgroundImage = Image.FromFile(@"Resources\off.png");
                    picPG.Enabled = false;
                    statusPG = false;
                    btnPG.Tag = 2;
                }
                SendData(2);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 浇花
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSL_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnSL.Tag.ToString() == "2")
                {
                    ClassAll.listgateway.Nappe = "01";
                    btnSL.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    //  btnSL.BackgroundImage = Image.FromFile(@"Resources\on.png");
                    picSL.Enabled = true;
                    statusSL = true;
                    btnSL.Tag = 1;
                }
                else
                {
                    ClassAll.listgateway.Nappe = "00";
                     btnSL.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    //    btnSL.BackgroundImage = Image.FromFile(@"Resources\off.png");
                    picSL.Enabled = false;
                    statusSL = false;
                    btnSL.Tag = 2;
                }
                SendData(2);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ClassAll.listgateway.IsConnect)
            {
                ClassAll.listgateway.Ping();
            }

            count++;
            if (count == 2)
            {
                GetState();
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            videoSourcePlayer2.SignalToStop();
            videoSourcePlayer2.WaitForStop();
            videoSourcePlayer3.SignalToStop();
            videoSourcePlayer3.WaitForStop();
            videoSourcePlayer4.SignalToStop();
            videoSourcePlayer4.WaitForStop();
            System.Environment.Exit(0);
           
        }

        /// <summary>
        /// 继电器1修改按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string txtaddrText = txtaddr.Text;

            if (txtaddrText.Length == 4 && txtaddrText.StartsWith("20"))
            {
                ClassAll.listgateway.RelayName1 = txtaddrText;
            }
            else
            {
                MessageBox.Show("继电器地址输入错误！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            relay1Status = true;
            ClassAll.SetRelay();
        }


        /// <summary>
        /// 继电器2修改按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string txtaddr1Text = txtaddr1.Text;

            if (txtaddr1Text.Length == 4 && txtaddr1Text.StartsWith("20"))
            {
                ClassAll.listgateway.RelayName2 = txtaddr1Text;
            }
            else
            {
                MessageBox.Show("继电器地址输入错误！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            relay2Status = true;
            ClassAll.SetRelay();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.Window.ScrollTo(120, 120);
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                checkBox1.Text = "显示";
                textBox1.PasswordChar = new char();
            }
            else
            {
                checkBox1.Text = "隐藏";
                textBox1.PasswordChar = '*';
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                checkBox2.Text = "显示";
                textBox2.PasswordChar = new char();
            }
            else
            {
                checkBox2.Text = "隐藏";
                textBox2.PasswordChar = '*';
            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox3.Checked)
            {
                checkBox3.Text = "显示";
                textBox3.PasswordChar = new char();
            }
            else
            {
                checkBox3.Text = "隐藏";
                textBox3.PasswordChar = '*';
            }
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox4.Checked)
            {
                checkBox4.Text = "显示";
                textBox4.PasswordChar = new char();
            }
            else
            {
                checkBox4.Text = "隐藏";
                textBox4.PasswordChar = '*';
            }
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox5.Checked)
            {
                checkBox5.Text = "显示";
                textBox5.PasswordChar = new char();
            }
            else
            {
                checkBox5.Text = "隐藏";
                textBox5.PasswordChar = '*';
            }
        }
        public void GetTime()
        {
            DateTime Time = DateTime.Now;
            label6.Text = Time.ToString("T");
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            this.GetTime();
        }

        private void userBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.userBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.usersDataSet);

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }
    }
}
