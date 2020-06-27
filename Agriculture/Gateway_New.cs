using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smarthome
{
    public class Gateway_New
    {
        #region Delegate&Event

        public delegate void DelegateSensorData(List<string[]> data);
        public DelegateSensorData EventSensorData;
        #endregion

        #region Variable



        /// <summary>
        /// 网关类
        /// </summary>
        private WSN_New_DLL.Gateway gateway;
        public bool IsConnect = false;

        /// <summary>
        /// 继电器1地址
        /// </summary>
        public string RelayName1 = "";
        /// <summary>
        /// 继电器2地址
        /// </summary>
        public string RelayName2 = "";

        /// <summary>
        /// 天窗
        /// </summary>
        public string Scuttle = "00";
        /// <summary>
        /// 水帘
        /// </summary>
        public string Nappe = "00";
        /// <summary>
        /// 喷灌
        /// </summary>
        public string SprinklingIrrigation = "00";
        /// <summary>
        /// 外遮阳
        /// </summary>
        public string ExternalShading = "00";
        /// <summary>
        /// 内遮阳
        /// </summary>
        public string InnerCurtain = "00";
        /// <summary>
        /// 外通风
        /// </summary>
        public string SeparateVentilation = "00";
        /// <summary>
        /// 内通风
        /// </summary>
        public string InternalVentilation = "00";

        /// <summary>
        /// 空气温度
        /// </summary>
        public string AirTemp = "";
        /// <summary>
        /// 空气湿度
        /// </summary>
        public string AirHumi = "";
        /// <summary>
        /// 土壤温度
        /// </summary>
        public string SoilTemp = "";
        /// <summary>
        /// //土壤湿度
        /// </summary>
        public string SoilHumi = "";
        /// <summary>
        /// 光照度
        /// </summary>
        public string Illuminance = "";
        /// <summary>
        /// 二氧化碳浓度
        /// </summary>
        public string C02Thickness = "";
        public string Hasfire = "";

        int _ID;
        string _Name;
        string _IP;
        string _Port = "4000";

        #endregion

        #region Property

        /// <summary>
        /// 编号
        /// </summary>
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        /// <summary>
        /// 网关名称
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        /// <summary>
        /// IP
        /// </summary>
        public string IP
        {
            get
            {
                return _IP;
            }
            set
            {
                _IP = value;
            }
        }

        /// <summary>
        /// 端口号
        /// </summary>
        public string Port
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
            }
        }

        #endregion

        #region Constructor

        public Gateway_New()
        {

        }

        public Gateway_New(int ID)
        {
            string sql = string.Format("select * from gateway where ID = '{0}'", ID);
            DataTable dt = ClassAll.dbo.GetDataTable(sql);
            DataRow row = dt.Rows[0];
            SetObject(row);
        }

        #endregion

        #region PublicMethod

        #region Database

        public void Insert()
        {
            string sql = string.Format("insert into gateway values ('{0}','{1}','{2}')", _Name, _IP, _Port);
            ClassAll.dbo.ExecuteSql(sql);
        }

        public void Update()
        {
            string sql = string.Format("update gateway set [name] = '{0}',ip = '{1}',port = '{2}' where id = {3}", _Name, _IP, _Port, 1);
            ClassAll.dbo.ExecuteSql(sql);
        }



        #endregion

        public bool Connect()
        {
            try
            {
                gateway = new WSN_New_DLL.Gateway(this._IP, Convert.ToInt32(this._Port));
                gateway.EventDataArrival += this.EventDataArrival;

                if (gateway.Connect())
                {
                    return IsConnect = true;
                }
                else
                {
                    return IsConnect = false;
                }
            }
            catch
            {
                return false;
            }
        }

        public string ConnectTest()
        {
            IPAddress ip = IPAddress.Parse(this._IP);
            IPEndPoint ipEndPoint = new IPEndPoint(ip, Convert.ToInt32(this._Port));
            bool result = WSN_New_DLL.TimeOutSocket.TryConnect(ipEndPoint, 1000);

            if (result)
            {
                return "连接网关成功！";
            }
            else
            {
                return "连接网关失败！";
            }
        }

        public void Disconnect()
        {
            if (IsConnect)
            {
                gateway.Disconnect();
            }
        }

        public string Ping()
        {
            if (IsConnect)
            {
                if (gateway.SendData(new string[] { "0000", "FFFFFFFFFF" }))
                {
                    return "";
                }
                else
                {
                    return "[Ping] Error";
                }
            }

            return "[Ping] 未连接网关";
        }

        public string ReadNodeData(string address)
        {
            if (IsConnect)
            {
                if (gateway.SendData(new string[] { address, "FFFFFFFFFF" }))
                {
                    return "";
                }
                else
                {
                    return "[ReadNodeData] Error";
                }
            }

            return "[ReadNodeData] 未连接网关";
        }

        public string SendNodeData(string[] command)
        {
            if (IsConnect)
            {
                if (gateway.SendData(command))
                {
                    return "";
                }
                else
                {
                    return "[ReadNodeData] Error";
                }
            }

            return "[ReadNodeData] 未连接网关";
        }

        #endregion

        #region PrivateMethod

        private void SetObject(DataRow row)
        {
            _IP = row["IP"].ToString();
            _Port = row["Port"].ToString();

        }

        private void EventDataArrival(List<string[]> data)
        {
            foreach (string[] s in data)
            {
                Console.WriteLine(s[1]);
                switch (s[0].Substring(0, 2))
                {
                    case "31":
                        AirTemp = (((double)Convert.ToInt32(s[1].Substring(0, 4), 16)) / 100).ToString();
                        AirHumi = (((double)Convert.ToInt32(s[1].Substring(4, 4), 16)) / 100).ToString();
                        break;
                    case "32":
                        SoilTemp = (((double)Convert.ToInt32(s[1].Substring(0, 4), 16)) / 100).ToString();
                        SoilHumi = (((double)Convert.ToInt32(s[1].Substring(4, 4), 16)) / 100).ToString();
                        break;
                    case "34":
                        Illuminance = Convert.ToInt32(s[1].Substring(0, 4), 16).ToString();
                        break;
                    case "33":
                        C02Thickness = Convert.ToInt32(s[1].Substring(0, 4), 16).ToString();
                        break;
                    case "20":
                        if (s[0] == RelayName1)
                        {
                            ExternalShading = s[1].Substring(0, 2);
                            InnerCurtain = s[1].Substring(2, 2);
                            SeparateVentilation = s[1].Substring(4, 2);
                            InternalVentilation = s[1].Substring(6, 2);
                        }
                        if (s[0] == RelayName2)
                        {
                            Scuttle = s[1].Substring(0, 2);
                            SprinklingIrrigation = s[1].Substring(2, 2);
                            Nappe = s[1].Substring(4, 2);
                        }

                        break;
                    case "51":
                        Hasfire =  (s[1].Substring(0,2).Equals("01")?"有火":"无火");
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
