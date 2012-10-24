using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MapleShark
{
    public partial class DataForm : DockContent
    {
        public DataForm()
        {
            InitializeComponent();
        }

        public MainForm MainForm { get { return ParentForm as MainForm; } }
        public HexBox HexBox { get { return mHex; } }

        private void mHex_SelectionLengthChanged(object pSender, EventArgs pArgs)
        {
            if (mHex.SelectionLength == 0) MainForm.PropertyForm.Properties.SelectedObject = null;
            else
            {
                byte[] buffer = null;
                StructureNode match = null;
                foreach (TreeNode node in MainForm.StructureForm.Tree.Nodes)
                {
                    StructureNode realNode = node as StructureNode;
                    buffer = realNode.Buffer;
                    if (mHex.SelectionStart == realNode.Cursor && mHex.SelectionLength == realNode.Length)
                    {
                        match = realNode;
                        break;
                    }
                }
                MainForm.StructureForm.Tree.SelectedNode = match;
                if (buffer != null) MainForm.PropertyForm.Properties.SelectedObject = new StructureSegment(buffer, (int)mHex.SelectionStart, (int)mHex.SelectionLength);
                else MainForm.PropertyForm.Properties.SelectedObject = null;
            }
        }

        private void mHex_KeyDown(object pSender, KeyEventArgs pArgs)
        {
            MainForm.CopyPacketHex(pArgs);
        }
    }
}
