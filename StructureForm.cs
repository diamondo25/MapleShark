using ScriptNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MapleShark
{
    public partial class StructureForm : DockContent
    {
        private MaplePacket mParsing = null;
        private Stack<StructureNode> mSubNodes = new Stack<StructureNode>();

        public StructureForm()
        {
            InitializeComponent();
        }

        public MainForm MainForm { get { return ParentForm as MainForm; } }
        public TreeView Tree { get { return mTree; } }

        public void ParseMaplePacket(MaplePacket pPacket)
        {
            mTree.Nodes.Clear();
            mSubNodes.Clear();
            pPacket.Rewind();

			string scriptPath = Application.StartupPath + Path.DirectorySeparatorChar + "Scripts" + Path.DirectorySeparatorChar + pPacket.Locale.ToString() + Path.DirectorySeparatorChar + pPacket.Build.ToString() + Path.DirectorySeparatorChar + (pPacket.Outbound ? "Outbound" : "Inbound") + Path.DirectorySeparatorChar + "0x" + pPacket.Opcode.ToString("X4") + ".txt";
			string commonPath = Application.StartupPath + Path.DirectorySeparatorChar + "Scripts" + Path.DirectorySeparatorChar + pPacket.Locale.ToString() + Path.DirectorySeparatorChar + pPacket.Build.ToString() + Path.DirectorySeparatorChar + "Common.txt";
            if (File.Exists(scriptPath))
            {
                mParsing = pPacket;

                try
                {
                    StringBuilder scriptCode = new StringBuilder();
                    scriptCode.Append(File.ReadAllText(scriptPath));
                    if (File.Exists(commonPath)) scriptCode.Append(File.ReadAllText(commonPath));
                    Script script = Script.Compile(scriptCode.ToString());
                    script.Context.SetItem("ScriptAPI", new ScriptAPI(this));
                    script.Execute();
                }
                catch (Exception exc)
                {
                    OutputForm output = new OutputForm("Script Error");
                    output.Append(exc.ToString());
                    output.Show(DockPanel, new Rectangle(MainForm.Location, new Size(400, 400)));
                }

                mParsing = null;
            }
            if (pPacket.Remaining > 0) mTree.Nodes.Add(new StructureNode("Undefined", pPacket.InnerBuffer, pPacket.Cursor, pPacket.Remaining));
        }

        private TreeNodeCollection CurrentNodes { get { return mSubNodes.Count > 0 ? mSubNodes.Peek().Nodes : mTree.Nodes; } }
        internal string APIGetFiletime()
        {
            string ret = DateTime.Now.ToFileTime().ToString().Substring(12);
            return ret;
        }
        internal byte APIAddByte(string pName)
        {
            byte value;
            if (!mParsing.ReadByte(out value)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - 1, 1));
            return value;
        }
        internal sbyte APIAddSByte(string pName)
        {
            sbyte value;
            if (!mParsing.ReadSByte(out value)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - 1, 1));
            return value;
        }
        internal ushort APIAddUShort(string pName)
        {
            ushort value;
            if (!mParsing.ReadUShort(out value)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - 2, 2));
            return value;
        }
        internal short APIAddShort(string pName)
        {
            short value;
            if (!mParsing.ReadShort(out value)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - 2, 2));
            return value;
        }
        internal uint APIAddUInt(string pName)
        {
            uint value;
            if (!mParsing.ReadUInt(out value)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - 4, 4));
            return value;
        }
        internal int APIAddInt(string pName)
        {
            int value;
            if (!mParsing.ReadInt(out value)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - 4, 4));
            return value;
        }
        internal float APIAddFloat(string pName)
        {
            float value;
            if (!mParsing.ReadFloat(out value)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - 4, 4));
            return value;
        }
        internal bool APIAddBool(string pName)
        {
            byte value;
            if (!mParsing.ReadByte(out value)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - 1, 1));
            return Convert.ToBoolean(value);
        }
        internal long APIAddLong(string pName)
        {
            long value;
            if (!mParsing.ReadLong(out value)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - 8, 8));
            return value;
        }
        internal long APIAddFlippedLong(string pName)
        {
            long value;
            if (!mParsing.ReadFlippedLong(out value)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - 8, 8));
            return value;
        }
        internal double APIAddDouble(string pName)
        {
            double value;
            if (!mParsing.ReadDouble(out value)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - 8, 8));
            return value;
        }
        internal string APIAddString(string pName)
        {
            APIStartNode(pName);
            short size = APIAddShort("Size");
            string value = APIAddPaddedString("String", size);
            APIEndNode(false);
            return value;
        }
        internal string APIAddPaddedString(string pName, int pLength)
        {
            string value;
            if (!mParsing.ReadPaddedString(out value, pLength)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - pLength, pLength));
            return value;
        }
        internal void APIAddField(string pName, int pLength)
        {
            byte[] buffer = new byte[pLength];
            if (!mParsing.ReadBytes(buffer)) throw new Exception("Insufficient packet data");
            CurrentNodes.Add(new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor - pLength, pLength));
        }
        internal void APIAddComment(string pComment)
        {
            CurrentNodes.Add(new StructureNode(pComment, mParsing.InnerBuffer, mParsing.Cursor, 0));
        }
        internal void APIStartNode(string pName)
        {
            StructureNode node = new StructureNode(pName, mParsing.InnerBuffer, mParsing.Cursor, 0);
            if (mSubNodes.Count > 0) mSubNodes.Peek().Nodes.Add(node);
            else mTree.Nodes.Add(node);
            mSubNodes.Push(node);
        }
        internal void APIEndNode(bool pExpand)
        {
            if (mSubNodes.Count > 0)
            {
                StructureNode node = mSubNodes.Pop();
                node.Length = mParsing.Cursor - node.Cursor;
                if (pExpand) node.Expand();
            }
        }
        internal int APIRemaining() { return mParsing.Remaining; }


        private void mTree_AfterSelect(object pSender, TreeViewEventArgs pArgs)
        {
            StructureNode node = pArgs.Node as StructureNode;
            if (node == null) { MainForm.DataForm.HexBox.SelectionLength = 0; MainForm.PropertyForm.Properties.SelectedObject = null; return; }
            MainForm.DataForm.HexBox.SelectionStart = node.Cursor;
            MainForm.DataForm.HexBox.SelectionLength = node.Length;
            MainForm.PropertyForm.Properties.SelectedObject = new StructureSegment(node.Buffer, node.Cursor, node.Length);
        }

        private void mTree_KeyDown(object pSender, KeyEventArgs pArgs)
        {
            MainForm.CopyPacketHex(pArgs);
        }
    }
}
