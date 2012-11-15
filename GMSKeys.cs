using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.IO;
using System.Windows.Forms;

using System.Diagnostics;
using System.Threading;


namespace MapleShark
{
    class GMSKeys
    {
        private static Dictionary<ushort, byte[]> MapleStoryGlobalKeys = new Dictionary<ushort, byte[]>();

        private static void InitByContents(string pContents)
        {
            string[] lines = pContents.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i += 2)
            {
                ushort version = ushort.Parse(lines[i]);
                string tmpkey = lines[i + 1];
                byte[] realkey = new byte[8];
                int tmp = 0;
                for (int j = 0; j < 4 * 8 * 2; j += 4 * 2)
                {
                    realkey[tmp++] = byte.Parse(tmpkey[j] + "" + tmpkey[j + 1], System.Globalization.NumberStyles.HexNumber);
                }
                MapleStoryGlobalKeys.Add(version, realkey);
            }
            
        }

        public static void Initialize()
        {
            try
            {
                if (File.Exists("noupdate.txt")) throw new Exception(); // Trigger offline file loading

                HttpWebRequest req = WebRequest.Create("http://direct.craftnet.nl/app_updates/get_keys.php") as HttpWebRequest;
                req.Proxy = null;

                using (HttpWebResponse response = req.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        string responseText = sr.ReadToEnd();
                        InitByContents(responseText);
                        File.WriteAllText("cached_keys.txt", responseText);
                    }
                }
            }
            catch
            {
                // Fail, w/e
                MapleStoryGlobalKeys.Clear();
                if (File.Exists("cached_keys.txt"))
                    InitByContents(File.ReadAllText("cached_keys.txt"));
                else
                    MessageBox.Show("Unable to load GMS Keys, because there were no cached keys stored and I failed retrieving them from the webserver! D:\r\nYou might want to check your internet connection and see if you can access http://direct.craftnet.nl/ directly.", "Problems!");
            }

            MapleStoryGlobalKeys.Add(118, new byte[] {
                0x5A, // Full key's lost
                0x22, 
                0xFB, 
                0xD1, 
                0x8F, 
                0x93, 
                0xCD, 
                0xE6, 
            });

        }

        public static byte[] GetKeyForVersion(ushort pVersion)
        {
            // Get first version known
            for (; pVersion > 0; pVersion--)
            {
                if (MapleStoryGlobalKeys.ContainsKey(pVersion))
                {
                    byte[] key = MapleStoryGlobalKeys[pVersion];
                    byte[] ret = new byte[32];
                    for (int i = 0; i < 8; i++)
                        ret[i * 4] = key[i];

                    return ret;
                }
            }
            return null;
        }
    }
}
