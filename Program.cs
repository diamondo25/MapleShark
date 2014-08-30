using Microsoft.Win32;
using ScriptNET.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Principal;

using SharpPcap.LibPcap;

namespace MapleShark
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] pArgs)
        {
            try
            {
                if (LibPcapLiveDeviceList.Instance.Count == 0) throw new Exception();
            }
            catch
            {
                if (MessageBox.Show(null, "Did you install WinPcap first? If you did, then try to run MapleShark in Administrator Mode, else press 'No' to go to the install page of WinPcap.", "Interface Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.No)
                {
                    System.Diagnostics.Process.Start("http://www.winpcap.org/install/default.htm");
                }
                Environment.Exit(2);
                return;
            }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += (sender, args) =>
            {
                Exception e = (Exception)args.ExceptionObject;
                System.IO.File.WriteAllText("MapleShark Error.txt", e.ToString());
                if (MessageBox.Show("Exception occurred. Open error in notepad?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("notepad", "\"MapleShark Error.txt\"");
                }
            };


            MainForm.StartupArguments = pArgs;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (new frmSplash().ShowDialog() == DialogResult.OK)
                Application.Run(new MainForm());
        }


        internal static string AssemblyVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        internal static string AssemblyCopyright { get { return ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright; } }
    }
}
