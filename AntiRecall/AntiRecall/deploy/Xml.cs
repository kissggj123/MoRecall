using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
namespace AntiRecall.deploy
{
    class Xml
    {
        public static string QQ_ori_path;
        public static SortedDictionary<string, string> antiRElement;

        public static void init_xml()
        {
            antiRElement = new SortedDictionary<string, string>();
            MainWindow window = (MainWindow)System.Windows.Application.Current.MainWindow;
            antiRElement.Add("PortText", "");
            antiRElement.Add("PortText_Copy", "");
            antiRElement.Add("QQPath", "");
            antiRElement.Add("TIMPath", "");
            //antiRElement.Add("NewUser", "new");
            antiRElement.Add("Mode", "");
            antiRElement.Add("Descript", "MoRecall v1.3.1");
        }

        public static bool CheckXml()
        {
            return System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\setting.xml");
        }

        public static void CreateXml(SortedDictionary<string, string> dict)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null));
            XmlElement XmlConfig = xmlDoc.CreateElement("XmlConfig");
            xmlDoc.AppendChild(XmlConfig);
            XmlElement QQPath = xmlDoc.CreateElement("QQPath");
            QQPath.InnerText = dict["QQPath"];
            XmlElement PortText = xmlDoc.CreateElement("PortText", dict["PortText"]);
            PortText.InnerText = dict["PortText"];
            XmlElement TIMPath = xmlDoc.CreateElement("TIMPath", dict["TIMPath"]);
            TIMPath.InnerText = dict["TIMPath"];
            //XmlElement NewUser = xmlDoc.CreateElement("NewUser", dict["NewUser"]);
            //NewUser.InnerText = dict["NewUser"];
            XmlElement PortText_Copy = xmlDoc.CreateElement("PortText_Copy", dict["PortText_Copy"]);
            PortText_Copy.InnerText = dict["PortText_Copy"];
            XmlElement Mode = xmlDoc.CreateElement("Mode", dict["Mode"]);
            Mode.InnerText = dict["Mode"];
            XmlConfig.AppendChild(QQPath);
            XmlConfig.AppendChild(PortText);
            XmlConfig.AppendChild(TIMPath);
            //XmlConfig.AppendChild(NewUser);
            XmlConfig.AppendChild(PortText_Copy);
            XmlConfig.AppendChild(Mode);
            xmlDoc.Save(System.IO.Directory.GetCurrentDirectory() + @"\setting.xml");
        }

        public static void ModifyXml(SortedDictionary<string, string> dict)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.IO.Directory.GetCurrentDirectory() + @"\setting.xml");
            XmlNode memberlist = xmlDoc.SelectSingleNode("XmlConfig");
            XmlNodeList nodelist = memberlist.ChildNodes;
            // XmlNodeList nodelist=xmlDoc.GetElementsByTagName("MEMBER");
            foreach (XmlNode node in nodelist)
            {
                node.InnerText = dict[node.Name];
            }
            xmlDoc.Save(System.IO.Directory.GetCurrentDirectory() + @"\setting.xml");

        }

        public static string QueryXml(string attr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.IO.Directory.GetCurrentDirectory() + @"\setting.xml");
            XmlNode memberlist = xmlDoc.SelectSingleNode("XmlConfig/"+attr);
            string ResultAttrStr = memberlist.InnerText;
            return ResultAttrStr;
            
        }
    }
}
