using Access_Dll;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Smarthome
{
    public static class ClassAll
    {
        public static DBOperate dbo;
        public static Gateway_New listgateway;
        public static string Path = AppDomain.CurrentDomain.BaseDirectory + "Config.xml";
        public static XmlDocument xd;
        public static XmlNode NodeRoot;

        public static bool GetRelay()
        {
            if (File.Exists(Path))
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(Path);
                NodeRoot = xd.SelectSingleNode("/Root");
                listgateway.RelayName1 = NodeRoot.ChildNodes[0].Attributes["Address"].Value;
                listgateway.RelayName2 = NodeRoot.ChildNodes[1].Attributes["Address"].Value;
                return true;
            }
            else
            {
                MessageBox.Show("未找到继电器配置文件，请检查文件是否正确");
                return false;
            }
        }

        public static bool SetRelay()
        {
            if (File.Exists(Path))
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(Path);
                NodeRoot = xd.SelectSingleNode("/Root");
                NodeRoot.ChildNodes[0].Attributes["Address"].Value=listgateway.RelayName1 ;
                NodeRoot.ChildNodes[1].Attributes["Address"].Value=listgateway.RelayName2 ;
                xd.Save(Path);
                return true;
            }
            else
            {
                MessageBox.Show("未找到继电器配置文件，请检查文件是否正确");
                return false;
            }
        }

    }
}
