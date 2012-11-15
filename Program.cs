using Microsoft.Win32;
using ScriptNET.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Principal;


namespace MapleShark
{
    internal static class Program
    {
        public const bool CRACKCRYPTO = true;
        [STAThread]
        private static void Main(string[] pArgs)
        {
            RegisterFileAssociation(".msb", "MapleShark", "MapleShark Binary File", Assembly.GetExecutingAssembly().Location, string.Empty, 0);

            CraftNetTools.AppUpdates.Check();
            GMSKeys.Initialize();

            RuntimeHost.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(pArgs));
        }
        internal static string AssemblyVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        internal static string AssemblyCopyright { get { return ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright; } }

        private static void RegisterFileAssociation(string pExtension, string pProgramId, string pDescription, string pEXE, string pIconPath, int pIconIndex)
        {
            try
            {
                if (pExtension.Length != 0)
                {
                    if (pExtension[0] != '.') pExtension = "." + pExtension;

                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(pExtension)) if (key == null) using (RegistryKey extKey = Registry.ClassesRoot.CreateSubKey(pExtension)) extKey.SetValue(string.Empty, pProgramId);

                    using (RegistryKey extKey = Registry.ClassesRoot.OpenSubKey(pExtension))
                    {
                        using (RegistryKey key = extKey.OpenSubKey(pProgramId))
                        {
                            if (key == null)
                            {
                                using (RegistryKey progIdKey = Registry.ClassesRoot.CreateSubKey(pProgramId))
                                {
                                    progIdKey.SetValue(string.Empty, pDescription);
                                    using (RegistryKey defaultIcon = progIdKey.CreateSubKey("DefaultIcon")) defaultIcon.SetValue(string.Empty, String.Format("\"{0}\",{1}", pIconPath, pIconIndex));

                                    using (RegistryKey command = progIdKey.CreateSubKey("shell\\open\\command")) command.SetValue(string.Empty, String.Format("\"{0}\" \"%1\"", pEXE));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
