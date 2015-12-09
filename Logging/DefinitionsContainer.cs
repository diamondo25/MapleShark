using System;
using System.Collections.Generic;

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MapleShark
{
    public sealed class DefinitionsContainer
    {
        public static DefinitionsContainer Instance { get; private set; }
        private Dictionary<byte, Dictionary<ushort, List<Definition>>> _definitions = new Dictionary<byte, Dictionary<ushort, List<Definition>>>();

        public DefinitionsContainer()
        {
            LoadDefinitions();
        }

        public Definition GetDefinition(byte pLocale, ushort pVersion, ushort pOpcode, bool pOutbound)
        {
            if (!_definitions.ContainsKey(pLocale)) return null;
            if (!_definitions[pLocale].ContainsKey(pVersion)) return null;

            return _definitions[pLocale][pVersion].Find(d => { return d.Outbound == pOutbound && d.Opcode == pOpcode; });
        }

        public void SaveDefinition(Definition pDefinition)
        {
            if (!_definitions.ContainsKey(pDefinition.Locale)) _definitions.Add(pDefinition.Locale, new Dictionary<ushort, List<Definition>>());
            if (!_definitions[pDefinition.Locale].ContainsKey(pDefinition.Build)) _definitions[pDefinition.Locale].Add(pDefinition.Build, new List<Definition>());

            _definitions[pDefinition.Locale][pDefinition.Build].RemoveAll(d =>
            {
                return d.Outbound == pDefinition.Outbound && d.Opcode == pDefinition.Opcode;
            });

            _definitions[pDefinition.Locale][pDefinition.Build].Add(pDefinition);
        }

        public static void Load()
        {
            Instance = new DefinitionsContainer();
        }

        private void LoadDefinitions()
        {
            if (!Directory.Exists("Scripts"))
                return;

            foreach (var localePath in Directory.GetDirectories(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Scripts"))
            {
                string localeDirName = localePath.Remove(0, localePath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                byte locale = 0;
                if (!byte.TryParse(localeDirName, out locale))
                    continue;

                _definitions.Add(locale, new Dictionary<ushort, List<Definition>>());

                foreach (var versionPath in Directory.GetDirectories(localePath))
                {
                    string versionDirName = versionPath.Remove(0, versionPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                    ushort version = 0;
                    if (!ushort.TryParse(versionDirName, out version))
                        continue;


                    if (!File.Exists(versionPath + Path.DirectorySeparatorChar + "PacketDefinitions.xml")) continue;

                    try
                    {
                        using (XmlReader xr = XmlReader.Create(versionPath + Path.DirectorySeparatorChar + "PacketDefinitions.xml"))
                        {
                            XmlSerializer xs = new XmlSerializer(typeof(List<Definition>));
                            _definitions[locale].Add(version, xs.Deserialize(xr) as List<Definition>);
                        }
                    }
                    catch { }
                }
            }
        }

        public void Save()
        {
            foreach (var kvpLocale in _definitions)
            {
                foreach (var kvpVersion in kvpLocale.Value)
                {
                    var path = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Scripts" + Path.DirectorySeparatorChar + kvpLocale.Key + Path.DirectorySeparatorChar + kvpVersion.Key;

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    XmlWriterSettings xws = new XmlWriterSettings()
                    {
                        Indent = true,
                        IndentChars = "  ",
                        NewLineOnAttributes = true,
                        OmitXmlDeclaration = true
                    };
                    using (XmlWriter xw = XmlWriter.Create(path + Path.DirectorySeparatorChar + "PacketDefinitions.xml", xws))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(List<Definition>));
                        xs.Serialize(xw, kvpVersion.Value);
                    }
                }
            }

            SaveProperties();
        }


        internal void SaveProperties()
        {
            Dictionary<ushort, Dictionary<ushort, SortedDictionary<ushort, string>>>[] headerList = new Dictionary<ushort, Dictionary<ushort, SortedDictionary<ushort, string>>>[2];

            for (int i = 0; i < 2; i++)
            {
                headerList[i] = new Dictionary<ushort, Dictionary<ushort, SortedDictionary<ushort, string>>>();
            }

            foreach (var kvpLocale in _definitions)
            {
                foreach (var kvpVersion in kvpLocale.Value)
                {
                    foreach (var d in kvpVersion.Value)
                    {
                        if (d.Opcode == 0xFFFF) return;
                        byte outbound = (byte)(d.Outbound ? 1 : 0);

                        if (!headerList[outbound].ContainsKey(d.Locale))
                            headerList[outbound].Add(d.Locale, new Dictionary<ushort, SortedDictionary<ushort, string>>());
                        if (!headerList[outbound][d.Locale].ContainsKey(d.Build))
                            headerList[outbound][d.Locale].Add(d.Build, new SortedDictionary<ushort, string>());
                        if (!headerList[outbound][d.Locale][d.Build].ContainsKey(d.Opcode))
                            headerList[outbound][d.Locale][d.Build].Add(d.Opcode, d.Name);
                        else
                            headerList[outbound][d.Locale][d.Build][d.Opcode] = d.Name;
                    }
                }
            }

            for (int i = 0; i < 2; i++)
            {
                foreach (var dict in headerList[i])
                {
                    string map = "Scripts" + Path.DirectorySeparatorChar + dict.Key.ToString();
                    if (!Directory.Exists(map))
                        Directory.CreateDirectory(map);

                    foreach (KeyValuePair<ushort, SortedDictionary<ushort, string>> kvp in dict.Value)
                    {
                        string map2 = map + Path.DirectorySeparatorChar + kvp.Key;
                        if (!Directory.Exists(map2))
                            Directory.CreateDirectory(map2);

                        string buff = "";
                        buff += "# Generated by MapleShark\r\n";
                        foreach (KeyValuePair<ushort, string> kvp2 in kvp.Value)
                        {
                            buff += string.Format("{0} = 0x{1:X4}\r\n", kvp2.Value == "" ? "# NOT SET: " : kvp2.Value.Replace(' ', '_'), kvp2.Key);
                        }
                        File.WriteAllText(map2 + Path.DirectorySeparatorChar + (i == 0 ? "send" : "recv") + ".properties", buff);
                    }

                }
            }
        }
    }
}
