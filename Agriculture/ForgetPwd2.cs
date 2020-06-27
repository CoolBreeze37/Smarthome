using AForge.Video.DirectShow;
using Baidu.Aip.Face;
using BaiduAI;
using BaiduAI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Smarthome
{
    public partial class ForgetPwd2 : Form
    {
        private string APP_ID = "20072922";
        private string API_KEY = "MbhLBd10ufcmOn31GEFWKhGK";
        private string SECRET_KEY = "NivR8FXiMRRBVkqlE8Voo9IVq75cdzt4";
        private Face client = null;
        public Form pre_form;
        public Form Firstform;
        public User user;
        /// <summary>
        /// 是否可以检测人脸
        /// </summary>
        private bool IsStart = false;
        /// <summary>
        /// 人脸在图像中的位置
        /// </summary>
        private FaceLocation location = null;

        private FilterInfoCollection videoDevices = null;
        public ForgetPwd2(Form pre_form, User  user)
        {
            InitializeComponent();
            client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            this.pre_form = pre_form;
            this.user = user;
            this.Firstform = Firstform;
        }
        public ForgetPwd2()
        {
            InitializeComponent();
            client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
        }
        private void ForgetPwd2_Load(object sender, EventArgs e)
        {
            /// 获取电脑已经安装的视频设备
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices != null && videoDevices.Count > 0)
            {
                foreach (FilterInfo device in videoDevices)
                {
                    comboBox1.Items.Add(device.Name);
                }
                comboBox1.SelectedIndex = 0;
            }
            videoSourcePlayer1.NewFrame += VideoSourcePlayer1_NewFrame;

            // 开发者在百度AI平台人脸识别接口只能1秒中调用2次，所以需要做 定时开始检测，每个一秒检测2次
            ThreadPool.QueueUserWorkItem(new WaitCallback(p => {
                while (true)
                {
                    IsStart = true;
                    Thread.Sleep(500);
                }
            }));
        }
        /// <summary>
        /// 新场景的事件获取单帧图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="image"></param>
        private void VideoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
        {
            try
            {
                if (IsStart)
                {
                    IsStart = false;
                    // 在线程池中另起一个线程进行人脸检测,这样不会造成界面视频卡顿现象
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.Detect), image.Clone());
                }
                if (location != null)
                {
                    try
                    {
                        // 绘制方框套住人脸
                        Graphics g = Graphics.FromImage(image);
                        g.DrawLine(new Pen(Color.Black), new System.Drawing.Point(location.left, location.top), new System.Drawing.Point(location.left + location.width, location.top));
                        g.DrawLine(new Pen(Color.Black), new System.Drawing.Point(location.left, location.top), new System.Drawing.Point(location.left, location.top + location.height));
                        g.DrawLine(new Pen(Color.Black), new System.Drawing.Point(location.left, location.top + location.height), new System.Drawing.Point(location.left + location.width, location.top + location.height));
                        g.DrawLine(new Pen(Color.Black), new System.Drawing.Point(location.left + location.width, location.top), new System.Drawing.Point(location.left + location.width, location.top + location.height));
                        g.Dispose();

                    }
                    catch (Exception ex)
                    {
                        ClassLoger.Error("VideoSourcePlayer1_NewFrame", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ClassLoger.Error("VideoSourcePlayer1_NewFrame1", ex);
            }

        }
        /// <summary>
        /// 连接并且打开摄像头
        /// </summary>
        [Obsolete]
        private void CameraConn()
        {
            if (comboBox1.Items.Count <= 0)
            {
                MessageBox.Show("请插入视频设备");
                return;
            }
            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[comboBox1.SelectedIndex].MonikerString);
            videoSource.DesiredFrameSize = new System.Drawing.Size(320, 240);
            videoSource.DesiredFrameRate = 1;

            videoSourcePlayer1.VideoSource = videoSource;
            videoSourcePlayer1.Start();
        }
        /// <summary>
        /// 重新检测连接视频设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            /// 获取电脑已经安装的视频设备
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices != null && videoDevices.Count > 0)
            {
                foreach (FilterInfo device in videoDevices)
                {
                    comboBox1.Items.Add(device.Name);
                }
                comboBox1.SelectedIndex = 0;
            }
        }




        [Obsolete]
        private void button4_Click(object sender, EventArgs e)
        {
            CameraConn();
        }

        /// <summary>
        /// Bitmap 转byte[]
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public byte[] Bitmap2Byte(Bitmap bitmap)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Jpeg);
                    byte[] data = new byte[stream.Length];
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Read(data, 0, Convert.ToInt32(stream.Length));
                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }
        public byte[] BitmapSource2Byte(BitmapSource source)
        {
            try
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.QualityLevel = 100;
                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Frames.Add(BitmapFrame.Create(source));
                    encoder.Save(stream);
                    byte[] bit = stream.ToArray();
                    stream.Close();
                    return bit;
                }
            }
            catch (Exception ex)
            {
                ClassLoger.Error("BitmapSource2Byte", ex);
            }
            return null;
        }

        /// <summary>
        /// 人脸检测
        /// </summary>
        public void Detect(object image)
        {
            if (image != null && image is Bitmap)
            {
                try
                {
                    var img = (Bitmap)image;
                    var imgByte = Convert.ToBase64String(Bitmap2Byte(img));
                    var imageType = "BASE64";
                    if (imgByte != null)
                    {
                        // 如果有可选参数
                        var options = new Dictionary<string, object>{
                            {"max_face_num", 2},
                            {"face_fields", "quality,beauty,eye_status"},
                            {"liveness_control", "NORMAL"}
                        };
                        var result = client.Detect(imgByte, imageType, options);

                        string text = null;
                        if (result.ToString().Contains("\"error_code\": 222202"))
                        {
                            text = "未识别到人脸!";
                            Action<string> actionDelegate = (x) => { this.textBox4.Text = x.ToString(); };
                            this.textBox4.Invoke(actionDelegate, text);


                        }
                        else
                        {
                            text = "ok！";
                            Action<string> actionDelegate = (x) => { this.textBox4.Text = x.ToString(); };
                            this.textBox4.Invoke(actionDelegate, text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClassLoger.Error("Form1.image", ex);
                }
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            pre_form.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {

            if (comboBox1.Items.Count <= 0)
            {
                MessageBox.Show("请插入视频设备");
                return;
            }
            try
            {
                if (videoSourcePlayer1.IsRunning)
                {
                    BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                    videoSourcePlayer1.GetCurrentVideoFrame().GetHbitmap(),
                                    IntPtr.Zero,
                                     Int32Rect.Empty,
                                    BitmapSizeOptions.FromEmptyOptions());
                    var img = Convert.ToBase64String(BitmapSource2Byte(bitmapSource));

                    var imageType = "BASE64";

                    var groupIdList = "1";

                    // 调用人脸搜索，可能会抛出网络等异常，请使用try/catch捕获
                    var result = client.Search(img, imageType, groupIdList);
                    Int64 FaceID = Convert.ToInt64(result["result"]["user_list"][0].Value<string>("user_id"));
                    Console.WriteLine(result);
                    Console.WriteLine(FaceID);
                    Console.WriteLine(user.FaceID);
                    Console.WriteLine(FaceID==user.FaceID);
                    if (result.ToString().Contains("\"error_code\": 0"))
                    {
                        if (FaceID == user.FaceID)
                        {
                            MessageBox.Show("验证成功！");
                            textBox1.Text = user.pwd;
                        }
                        else
                        {
                            MessageBox.Show("验证失败！信息不匹配！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("人脸识别失败"+result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            pre_form.Show();
        }

        private void ForgetPwd2_FormClosed(object sender, FormClosedEventArgs e)
        {
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            pre_form.Show();

        }
    }
}

