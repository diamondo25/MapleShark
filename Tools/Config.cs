using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace MapleShark
{
    public sealed class Config
    {
        public string Interface = "";
        public ushort LowPort = 8484;
        public ushort HighPort = 8989;

        [XmlIgnore]
        public bool LoadedFromFile = false;

        private static Config sInstance = null;
        internal static Config Instance
        {
            get
            {
                if (sInstance == null)
                {
                    if (!File.Exists("Config.xml"))
                    {
                        sInstance = new Config();
                        sInstance.Save();
                    }
                    else
                    {
                        try
                        {
                            using (XmlReader xr = XmlReader.Create("Config.xml"))
                            {
                                XmlSerializer xs = new XmlSerializer(typeof(Config));
                                sInstance = xs.Deserialize(xr) as Config;
                                sInstance.LoadedFromFile = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("The configuration file is broken and could not be read. You'll have to reconfigure MapleShark... Sorry!\r\nAdditional exception info:\r\n" + ex.ToString(), "MapleShark", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            sInstance = new Config();
                        }
                    }
                }
                return sInstance;
            }
        }

        internal Definition GetDefinition(ushort pBuild, byte pLocale, bool pOutbound, ushort pOpcode)
        {
            return DefinitionsContainer.Instance.GetDefinition(pLocale, pBuild, pOpcode, pOutbound);
            // return Definitions.Find(d => d.Locale == pLocale && d.Build == pBuild && d.Outbound == pOutbound && d.Opcode == pOpcode);
        }


        internal static string GetPropertiesFile(bool pOutbound, byte pLocale, ushort pVersion)
        {
            return System.Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Scripts" + Path.DirectorySeparatorChar + pLocale.ToString() + Path.DirectorySeparatorChar + pVersion.ToString() + Path.DirectorySeparatorChar + (pOutbound ? "send" : "recv") + ".properties";
        }

        internal void Save()
        {
            XmlWriterSettings xws = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "  ",
                NewLineOnAttributes = true,
                OmitXmlDeclaration = true
            };
            using (XmlWriter xw = XmlWriter.Create("Config.xml", xws))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Config));
                xs.Serialize(xw, this);
            }
            if (!Directory.Exists("Scripts")) Directory.CreateDirectory("Scripts");
        }
    }
}
