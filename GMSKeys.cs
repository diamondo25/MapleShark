using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace MapleShark
{
    class GMSKeys
    {
        private static Dictionary<KeyValuePair<ushort, byte>, byte[]> MapleStoryGlobalKeys;

        private static void InitByContents(string pContents)
        {
            string[] lines = pContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i += 2)
            {
                var firstLine = lines[i];
                ushort version = ushort.Parse(firstLine.Substring(0, firstLine.IndexOf('.')));
                byte subversion = byte.Parse(firstLine.Substring(firstLine.IndexOf('.') + 1));
                string tmpkey = lines[i + 1];
                byte[] realkey = new byte[8];
                int tmp = 0;
                for (int j = 0; j < 4 * 8 * 2; j += 4 * 2)
                {
                    realkey[tmp++] = byte.Parse(tmpkey[j] + "" + tmpkey[j + 1], System.Globalization.NumberStyles.HexNumber);
                }
                MapleStoryGlobalKeys.Add(new KeyValuePair<ushort, byte>(version, subversion), realkey);
            }

        }

        public static void Initialize()
        {
            Console.WriteLine("Initializing keys...");
            MapleStoryGlobalKeys = new Dictionary<KeyValuePair<ushort, byte>, byte[]>();
            try
            {
                if (File.Exists("noupdate.txt")) throw new Exception(); // Trigger offline file loading

                HttpWebRequest req = WebRequest.Create("http://direct.craftnet.nl/app_updates/get_keys.php?source=SRK&version=2") as HttpWebRequest;
                req.Proxy = null;

                using (HttpWebResponse response = req.GetResponse() as HttpWebResponse)
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    string responseText = sr.ReadToEnd();
                    InitByContents(responseText);
                    File.WriteAllText("cached_keys.txt", responseText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // Fail, w/e
                if (File.Exists("cached_keys.txt"))
                    InitByContents(File.ReadAllText("cached_keys.txt"));
                else
                    MessageBox.Show("Unable to load GMS Keys, because there were no cached keys stored and I failed retrieving them from the webserver! D:\r\nYou might want to check your internet connection and see if you can access http://direct.craftnet.nl/ directly.", "Problems!");
            }

            MapleStoryGlobalKeys.Add(new KeyValuePair<ushort, byte>(118, 1), new byte[] {
                0x5A, // Full key's lost
                0x22, 
                0xFB, 
                0xD1, 
                0x8F, 
                0x93, 
                0xCD, 
                0xE6, 
            });

            Console.WriteLine("Done. {0} keys loaded", MapleStoryGlobalKeys.Count);
        }

        public static byte[] GetKeyForVersion(ushort pVersion, byte pSubVersion)
        {
            if (MapleStoryGlobalKeys == null) Initialize();
            // Get first version known
            for (; pVersion > 0; pVersion--)
            {
                for (byte subVersion = pSubVersion; subVersion > 0; subVersion--)
                {
                    var tuple = new KeyValuePair<ushort, byte>(pVersion, subVersion);
                    if (MapleStoryGlobalKeys.ContainsKey(tuple))
                    {
                        byte[] key = MapleStoryGlobalKeys[tuple];
                        byte[] ret = new byte[32];
                        for (int i = 0; i < 8; i++)
                            ret[i * 4] = key[i];

                        return ret;
                    }
                }
            }
            Console.WriteLine("Version not found!");
            return null;
        }
    }
}
