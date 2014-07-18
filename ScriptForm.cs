using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MapleShark
{
    public partial class ScriptForm : DockContent
    {
        private string mPath = "";
        private MaplePacket mPacket = null;
        private StructureForm mStructureForm = new StructureForm();

        public ScriptForm(string pPath, MaplePacket pPacket)
        {
            mPath = pPath;
            mPacket = pPacket;
            InitializeComponent();
            if (pPacket != null)
                Text = "Script 0x" + pPacket.Opcode.ToString("X4") + ", " + (pPacket.Outbound ? "Outbound" : "Inbound");
            else
                Text = "Common Script";
        }

        internal MaplePacket Packet { get { return mPacket; } }
        public StructureForm StructureForm { get { return mStructureForm; } }

        private void ScriptForm_Load(object pSender, EventArgs pArgs)
        {
            mScriptEditor.Document.SetSyntaxFromEmbeddedResource(Assembly.GetExecutingAssembly(), "MapleShark.ScriptSyntax.txt");
            if (File.Exists(mPath)) mScriptEditor.Open(mPath);

        }

        private void mSaveButton_Click(object pSender, EventArgs pArgs)
        {
            if (mScriptEditor.Document.Text.Length == 0) File.Delete(mPath);
            else mScriptEditor.Save(mPath);
            //Close();
            //Save and refresh structure without closing form By EventBackColorChanged
            if (this.BackColor == SystemColors.ButtonFace)
            {
                this.BackColor = SystemColors.Control;
            }
            else
            {
                this.BackColor = SystemColors.ButtonFace;
            }
        }

        private void mImportButton_Click(object sender, EventArgs e)
        {

            if (FileImporter.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(FileImporter.FileName))
                {
                    if (mScriptEditor.Document.Text.Length > 0 && MessageBox.Show("Are you sure you want to open this file? The current script will be replaced with the one from the file you selected.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                    mScriptEditor.Open(FileImporter.FileName);
                }
            }
        }


        private void importScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (FileImporter.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(FileImporter.FileName))
                {
                    if (mScriptEditor.Document.Text.Length > 0 && MessageBox.Show("Are you sure you want to open this file? The current script will be replaced with the one from the file you selected.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                    mScriptEditor.Open(FileImporter.FileName);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mScriptEditor.Document.Text.Length == 0) File.Delete(mPath);
            else mScriptEditor.Save(mPath);
            //Close();
            //Save and refresh structure without closing form By EventBackColorChanged
            if (this.BackColor == SystemColors.ButtonFace)
            {
                this.BackColor = SystemColors.Control;
            }
            else
            {
                this.BackColor = SystemColors.ButtonFace;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ImportButton_Click(object sender, EventArgs e)
        {

            if (FileImporter.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(FileImporter.FileName))
                {
                    if (mScriptEditor.Document.Text.Length > 0 && MessageBox.Show("Are you sure you want to open this file? The current script will be replaced with the one from the file you selected.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                    mScriptEditor.Open(FileImporter.FileName);
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (mScriptEditor.Document.Text.Length == 0) File.Delete(mPath);
            else mScriptEditor.Save(mPath);
            //Close();
            //Save and refresh structure without closing form By EventBackColorChanged
            if (this.BackColor == SystemColors.ButtonFace)
            {
                this.BackColor = SystemColors.Control;
            }
            else
            {
                this.BackColor = SystemColors.ButtonFace;
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            ExportScript.InitialDirectory = @"C:\";
            ExportScript.Title = "Expoer Mapleshark script";
            ExportScript.CheckFileExists = false;
            ExportScript.CheckPathExists = true;
            ExportScript.DefaultExt = "txt";
            ExportScript.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            ExportScript.FileName = "0x"+mPacket.Opcode.ToString("X4");
            ExportScript.FilterIndex = 2;
            ExportScript.RestoreDirectory = true;
            if(ExportScript.ShowDialog() == DialogResult.OK)
            {
                mScriptEditor.Export(ExportScript.FileName);
                MessageBox.Show("Export success\r\nPath : "+ExportScript.FileName);
            }
        }

        private void exportScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportScript.InitialDirectory = @"C:\";
            ExportScript.Title = "Expoer Mapleshark script";
            ExportScript.CheckFileExists = false;
            ExportScript.CheckPathExists = true;
            ExportScript.DefaultExt = "txt";
            ExportScript.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            ExportScript.FileName = "0x" + mPacket.Opcode.ToString("X4");
            ExportScript.FilterIndex = 2;
            ExportScript.RestoreDirectory = true;
            if (ExportScript.ShowDialog() == DialogResult.OK)
            {
                mScriptEditor.Export(ExportScript.FileName);
                MessageBox.Show("Export success\r\nPath : " + ExportScript.FileName);
            }
        }
    }
}
