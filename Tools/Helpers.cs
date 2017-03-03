using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapleShark
{
    class Helpers
    {
        public static string GetScriptFolder(byte locale, ushort build)
        {
            return string.Format(
                "{1}{0}Scripts{0}{2}{0}{3}{0}", 
                Path.DirectorySeparatorChar, 
                Application.StartupPath, 
                locale,
                build
            );
        }

        public static string GetScriptPath(byte locale, ushort build, bool outbound, ushort opcode)
        {
            return string.Format(
                "{1}{2}{0}0x{3:X4}.txt",
                Path.DirectorySeparatorChar,
                GetScriptFolder(locale, build),
                outbound ? "Outbound" : "Inbound",
                opcode
            );
        }

        public static string GetCommonScriptPath(byte locale, ushort build)
        {
            return Path.Combine(GetScriptFolder(locale, build), "Common.txt");
        }

        public static void MakeSureFileDirectoryExists(string path)
        {
            var dirname = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirname))
                Directory.CreateDirectory(dirname);
        }
        
    }
}
