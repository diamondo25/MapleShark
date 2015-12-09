using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace MapleShark
{
    static class MapleKeys
    {
        private static Dictionary<byte, Dictionary<KeyValuePair<ushort, byte>, byte[]>> MapleStoryKeys;

        private static void InitByContents(string pContents)
        {
            string[] lines = pContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i += 2)
            {
                var firstLine = lines[i];
                var semicolonPos = firstLine.IndexOf(':');
                var dotPos = firstLine.IndexOf('.');

                byte locale = byte.Parse(firstLine.Substring(0, semicolonPos));
                ushort version = ushort.Parse(firstLine.Substring(semicolonPos + 1, dotPos - (semicolonPos + 1)));
                byte subversion = byte.Parse(firstLine.Substring(dotPos + 1));

                string tmpkey = lines[i + 1];
                byte[] realkey = new byte[8];
                int tmp = 0;
                for (int j = 0; j < 4 * 8 * 2; j += 4 * 2)
                    realkey[tmp++] = byte.Parse(tmpkey[j] + "" + tmpkey[j + 1], System.Globalization.NumberStyles.HexNumber);

                AddKey(locale, version, subversion, realkey);
            }

        }

        private static void AddKey(byte locale, ushort version, byte subversion, byte[] key)
        {
            if (!MapleStoryKeys.ContainsKey(locale))
                MapleStoryKeys.Add(locale, new Dictionary<KeyValuePair<ushort, byte>, byte[]>());
            MapleStoryKeys[locale].Add(new KeyValuePair<ushort, byte>(version, subversion), key);
        }

        public static void Initialize()
        {
            Console.WriteLine("Initializing keys...");
            MapleStoryKeys = new Dictionary<byte, Dictionary<KeyValuePair<ushort, byte>, byte[]>>();
            try
            {
                if (File.Exists("noupdate.txt")) throw new Exception(); // Trigger offline file loading

                var req = WebRequest.Create("http://direct.craftnet.nl/app_updates/get_keys.php?source=SRK&version=4") as HttpWebRequest;
                req.Proxy = null;

                using (var response = (HttpWebResponse)req.GetResponse())
                using (var sr = new StreamReader(response.GetResponseStream()))
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
                    MessageBox.Show("Unable to load MapleStory Keys, because there were no cached keys stored and I failed retrieving them from the webserver! D:\r\nYou might want to check your internet connection and see if you can access http://direct.craftnet.nl/ directly.", "Problems!");
            }

            AddKey(8, 118, 1, new byte[] {
                0x5A, // Full key's lost
                0x22, 
                0xFB, 
                0xD1, 
                0x8F, 
                0x93, 
                0xCD, 
                0xE6, 
            });

            // Quickly count amount of keys
            int keyCount = 0;
            foreach (var kvp in MapleStoryKeys)
                keyCount += kvp.Value.Count;

            Console.WriteLine("Done. {1} keys loaded for {0} locales", MapleStoryKeys.Count, keyCount);
        }

        public static byte[] GetKeyForVersion(byte locale, ushort version, byte subversion)
        {
            if (MapleStoryKeys == null) Initialize();
            if (!MapleStoryKeys.ContainsKey(locale))
            {
                Console.WriteLine("Locale {0} not found!", locale);
                return null;
            }

            // Get first version known
            for (ushort v = version; v > 0; v--)
            {
                for (byte sv = subversion; sv >= 0; sv--)
                {
                    var tuple = new KeyValuePair<ushort, byte>(v, sv);
                    if (MapleStoryKeys[locale].ContainsKey(tuple))
                    {
                        byte[] key = MapleStoryKeys[locale][tuple];
                        byte[] ret = new byte[32];
                        for (int i = 0; i < 8; i++)
                            ret[i * 4] = key[i];

                        Console.WriteLine("Using key for version {0}.{1}", v, sv);
                        return ret;
                    }
                    if (sv == 0) break;
                }
            }
            Console.WriteLine("Version {0}.{1} for locale {2} not found!", version, subversion, locale);
            return null;
        }
    }
}
