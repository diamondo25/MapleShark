using ScriptNET.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MapleShark
{
    public sealed class ScriptAPI
    {
        private StructureForm mStructure = null;

        [Bindable(false)]
        internal ScriptAPI(StructureForm pStructure) { mStructure = pStructure; }

        public long AddByte(string pName) { return mStructure.APIAddByte(pName); }
        public long AddSByte(string pName) { return mStructure.APIAddSByte(pName); }
        public long AddUShort(string pName) { return mStructure.APIAddUShort(pName); }
        public long AddShort(string pName) { return mStructure.APIAddShort(pName); }
        public long AddUInt(string pName) { return mStructure.APIAddUInt(pName); }
        public long AddInt(string pName) { return mStructure.APIAddInt(pName); }
        public long AddLong(string pName) { return mStructure.APIAddLong(pName); }
        public double AddFloat(string pName) { return mStructure.APIAddFloat(pName); }
        public double AddDouble(string pName) { return mStructure.APIAddDouble(pName); }
        public bool AddBool(string pName) { return mStructure.APIAddBool(pName); }
        public string AddString(string pName) { return mStructure.APIAddString(pName); }
        public string AddPaddedString(string pName, int pLength) { return mStructure.APIAddPaddedString(pName, pLength); }
        public void AddField(string pName, int pLength) { mStructure.APIAddField(pName, pLength); }
        public void AddComment(string pComment) { mStructure.APIAddComment(pComment); }
        public void StartNode(string pName) { mStructure.APIStartNode(pName); }
        public void EndNode(bool pExpand) { mStructure.APIEndNode(pExpand); }
        public string GetFiletime() { return mStructure.APIGetFiletime(); }
        public void Write(string pPath, string pLine) {
			pLine = pLine.Replace("\r\n", "\\r\\n");
			using (StreamWriter writer = File.AppendText(pPath)) {

				writer.WriteLine(pLine);
			}
        }
        public long Remaining() { return mStructure.APIRemaining(); }

        //Below add by KururuLABO
        public string AddPaddedString(int pLength) { return mStructure.APIAddPaddedString(pLength); }
        public void AddComment(string pComment,int pLength) { mStructure.APIAddComment(pComment,pLength); }
        public void StartNodeWithVariable(string pName, int pLength) { mStructure.StartNodeWithVariable(pName, pLength); }
        public long AddByte() { return mStructure.APIAddByte(); }
        public long AddSByte() { return mStructure.APIAddSByte(); }
        public long AddUShort() { return mStructure.APIAddUShort(); }
        public long AddShort() { return mStructure.APIAddShort(); }
        public long AddUInt() { return mStructure.APIAddUInt(); }
        public long AddInt() { return mStructure.APIAddInt(); }
        public long AddLong() { return mStructure.APIAddLong(); }
        public double AddFloat() { return mStructure.APIAddFloat(); }
        public double AddDouble() { return mStructure.APIAddDouble(); }
        public bool AddBool() { return mStructure.APIAddBool(); }

    }
}
