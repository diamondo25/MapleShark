using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;
using ScriptNET.Runtime;
using System.Reflection;
using System.Security.Principal;

namespace MapleShark
{
    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();

            centerX = this.Size.Width / 2;
            centerY = this.Size.Height / 2;

            centerX -= pictureBox1.Size.Width / 2;
            centerY -= pictureBox1.Size.Height / 2;
        }

        int centerX = 0, centerY = 0;
        int lastX = -1;
        int timer = 0;
        bool lastOrientation = true;

        private void frmSplash_Load(object sender, EventArgs e)
        {
            initialisator.RunWorkerAsync();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer += 25;
            var pic = pictureBox1;
            var time = timer++;

            var tmp = time / 360.0f;
            var x = Math.Sin(tmp) * 90;
            x += centerX;

            var y = pic.Location.Y;

            pic.Location = new Point((int)x, (int)y);

            bool goLeft = lastX > (int)x;

            if (goLeft != lastOrientation)
            {
                Image img = pic.Image.Clone() as Image;
                img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pic.Image = img;
                lastOrientation = goLeft;
            }

            lastX = (int)x;
        }

        private void initialisator_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void initialisator_DoWork(object sender, DoWorkEventArgs e)
        {
            var filepath = Assembly.GetExecutingAssembly().Location;
            Environment.CurrentDirectory = filepath.Remove(filepath.LastIndexOf(System.IO.Path.DirectorySeparatorChar));

            initialisator.ReportProgress(0, "Checking for updates");
            CraftNetTools.AppUpdates.Check();

            initialisator.ReportProgress(0, "Initializing MapleStory AES Keys");
            MapleKeys.Initialize();

            initialisator.ReportProgress(0, "Loading Script.NET context");
            RuntimeHost.Initialize();

            initialisator.ReportProgress(0, "Loading packet definitions");
            DefinitionsContainer.Load();

            initialisator.ReportProgress(0, "Loading + saving config file");
            Config.Instance.Save();

            initialisator.ReportProgress(0, "Registering .msb extension");
            RegisterFileAssociation(".msb", "MapleShark", "MapleShark Binary File", filepath, string.Empty, 0);
        }


        private static void RegisterFileAssociation(string pExtension, string pProgramId, string pDescription, string pEXE, string pIconPath, int pIconIndex)
        {
            try
            {
                if (pExtension.Length != 0)
                {
                    if (pExtension[0] != '.') pExtension = "." + pExtension;

                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(pExtension)) if (key == null) using (RegistryKey extKey = Registry.ClassesRoot.CreateSubKey(pExtension)) extKey.SetValue(string.Empty, pProgramId);

                    using (RegistryKey extKey = Registry.ClassesRoot.OpenSubKey(pExtension))
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
            catch (Exception ex)
            {
                Console.WriteLine("Error registering file association: {0}", ex.ToString());
            }
        }

        private void initialisator_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label2.Text = (string)e.UserState;
        }
    }
}
