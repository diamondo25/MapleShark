using ScriptNET;
using PacketDotNet;
using PacketDotNet.Utils;
using PacketDotNet.LLDP;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

using MapleLib.PacketLib;

namespace MapleShark
{
    public partial class SessionForm : DockContent
    {
        public enum Results
        {
            Continue,
            Terminated,
            CloseMe
        }

        private string mFilename = null;
        private bool mTerminated = false;
        //private bool mSavedAfterTermination = false;
        private ushort mLocalPort = 0;
        private ushort mRemotePort = 0;
        private uint mOutboundSequence = 0;
        private uint mInboundSequence = 0;
		private ushort mBuild = 0;
		private byte mLocale = 0;
		private string mPatchLocation = "";
        private Dictionary<uint, byte[]> mOutboundBuffer = new Dictionary<uint, byte[]>();
        private Dictionary<uint, byte[]> mInboundBuffer = new Dictionary<uint, byte[]>();
        private MapleStream mOutboundStream = null;
        private MapleStream mInboundStream = null;
        private List<MaplePacket> mPackets = new List<MaplePacket>();
        private List<Pair<bool, ushort>> mOpcodes = new List<Pair<bool, ushort>>();

		private string mRemoteEndpoint = "???";
		private string mLocalEndpoint = "???";

        internal SessionForm()
        {
            InitializeComponent();
        }

        public MainForm MainForm { get { return ParentForm as MainForm; } }
        public ListView ListView { get { return mPacketList; } }
		public ushort Build { get { return mBuild; } }
		public ushort Locale { get { return mLocale; } }
        public List<Pair<bool, ushort>> Opcodes { get { return mOpcodes; } }

        private DateTime startTime;

        public void UpdateOpcodeList()
        {
            mOpcodes = mOpcodes.OrderBy(a => a.Second).ToList();
        }


        internal bool MatchTCPPacket(TcpPacket pTCPPacket)
        {
            if (mTerminated) return false;
            if (pTCPPacket.SourcePort == mLocalPort && pTCPPacket.DestinationPort == mRemotePort) return true;
            if (pTCPPacket.SourcePort == mRemotePort && pTCPPacket.DestinationPort == mLocalPort) return true;
            return false;
        }

        internal bool CloseMe(DateTime pTime)
        {
            if (mPackets.Count == 0 && (pTime - startTime).TotalSeconds >= 5)
            {
                return true;
            }
            return false;
        }

        internal Results BufferTCPPacket(TcpPacket pTCPPacket, DateTime pArrivalTime)
        {
            if (pTCPPacket.Fin || pTCPPacket.Rst)
            {
                mTerminated = true;
                Text += " (Terminated)";
                if (mPackets.Count == 0)
                {
                    // fuck
                    return Results.CloseMe;
                }
                else
                {
                    return Results.Terminated;
                }
            }
            if (pTCPPacket.Syn && !pTCPPacket.Ack)
            {
                mLocalPort = (ushort)pTCPPacket.SourcePort;
                mRemotePort = (ushort)pTCPPacket.DestinationPort;
                mOutboundSequence = (uint)(pTCPPacket.SequenceNumber + 1);
                Text = "Port " + mLocalPort.ToString();
                startTime = DateTime.Now;

                mRemoteEndpoint = ((PacketDotNet.IPv4Packet)pTCPPacket.ParentPacket).SourceAddress.ToString() + ":" + pTCPPacket.SourcePort.ToString();
                mLocalEndpoint = ((PacketDotNet.IPv4Packet)pTCPPacket.ParentPacket).DestinationAddress.ToString() + ":" + pTCPPacket.DestinationPort.ToString();
                Console.WriteLine("[CONNECTION] From {0} to {1}", mLocalEndpoint, mRemoteEndpoint);

                return Results.Continue;
            }
            if (pTCPPacket.Syn && pTCPPacket.Ack) { mInboundSequence = (uint)(pTCPPacket.SequenceNumber + 1); return Results.Continue; }
            if (pTCPPacket.PayloadData.Length == 0) return Results.Continue;
            if (mBuild == 0)
            {
                if (pTCPPacket.PayloadData.Length < 13) return Results.CloseMe;
                byte[] tcpData = pTCPPacket.PayloadData;
                //mBuild = (ushort)(tcpData[2] | (tcpData[3] << 8));

                bool mIsKMS = false;

                PacketReader pr = new PacketReader(tcpData);
                pr.ReadShort();
                ushort version = pr.ReadUShort();
                var pos = pr.Position;
                {
                    var shrt = pr.ReadShort();
                    if (shrt < 0 || shrt > 0x0020)
                    {
                        return Results.CloseMe;
                    }
                }
                pr.Reset(pos);
                string patchLocation = pr.ReadMapleString();
                byte[] localIV = pr.ReadBytes(4);
                byte[] remoteIV = pr.ReadBytes(4);
                byte serverLocale = pr.ReadByte();
                if (serverLocale == 0x07 && pr.Remaining > 0)
                {
                    ushort unk = pr.ReadUShort();
                }

                if (pr.Remaining > 0 || serverLocale > 0x12)
                {
                    //MessageBox.Show("Connection closing. pr.remaining > 0 | ServerLocale > 0x12: " + (pr.Remaining > 0) + " - " + (serverLocale > 0x12));
                    //MessageBox.Show(string.Format("Version {0} patch location {1} serverlocale {4}", version, patchLocation, localIV, remoteIV, serverLocale));
                    return Results.CloseMe;
                }

                if (serverLocale == 0x02 || (serverLocale == 0x01 && version > 255)) mIsKMS = true;
                else mIsKMS = false;

                if (mIsKMS)
                {
                    int test = int.Parse(patchLocation);
                    ushort t1 = (ushort)(test & 0x7FFF);
                    int t2 = (test >> 15) & 1;
                    int t3 = (test >> 16) & 0xFF;
                    Console.WriteLine("Logging KMS connection. Version {0} | {1} | {2}", t1, t2, t3);
                    mBuild = t1;
                }
                else
                {
                    mBuild = version;
                }

                mLocale = serverLocale;
                mPatchLocation = patchLocation;

                mOutboundStream = new MapleStream(true, mBuild, mLocale, localIV);
                mInboundStream = new MapleStream(false, (ushort)(0xFFFF - mBuild), mLocale, remoteIV);
                mInboundSequence += (uint)tcpData.Length;

                // Generate HandShake packet
                Definition definition = Config.Instance.GetDefinition(mBuild, mLocale, false, 0xFFFF);
                if (definition == null)
                {
                    definition = new Definition();
                    definition.Outbound = false;
                    definition.Locale = mLocale;
                    definition.Opcode = 0xFFFF;
                    definition.Name = "Maple Handshake";
                    definition.Build = mBuild;
                    Config.Instance.Definitions.Add(definition);
                }

                {
                    string filename = "Scripts" +
                        Path.DirectorySeparatorChar + mLocale.ToString() +
                        Path.DirectorySeparatorChar + mBuild.ToString() +
                        Path.DirectorySeparatorChar + "Inbound" +
                        Path.DirectorySeparatorChar + "0xFFFF.txt";
                    if (!Directory.Exists(Path.GetDirectoryName(filename))) Directory.CreateDirectory(Path.GetDirectoryName(filename));
                    if (!File.Exists(filename))
                    {
                        string contents = "";
                        contents += "using (ScriptAPI) {\r\n";
                        contents += "\tAddShort(\"Packet Size\");\r\n";
                        contents += "\tAddUShort(\"MapleStory Version\");\r\n";
                        contents += "\tAddString(\"MapleStory Patch Location\");\r\n";
                        contents += "\tAddField(\"Local Initializing Vector (IV)\", 4);\r\n";
                        contents += "\tAddField(\"Remote Initializing Vector (IV)\", 4);\r\n";
                        contents += "\tAddByte(\"MapleStory Locale\");\r\n";
                        contents += "}";
                        File.WriteAllText(filename, contents);
                    }
                }

                MaplePacket packet = new MaplePacket(pArrivalTime, false, mBuild, mLocale, 0xFFFF, definition == null ? "" : definition.Name, tcpData);
                if (!mOpcodes.Exists(kv => kv.First == packet.Outbound && kv.Second == packet.Opcode))
                { // Should be false, but w/e
                    mOpcodes.Add(new Pair<bool, ushort>(packet.Outbound, packet.Opcode));
                }

                mPacketList.Items.Add(packet);
                mPackets.Add(packet);
                MainForm.SearchForm.RefreshOpcodes(true);
                Console.WriteLine("[CONNECTION] MapleStory V{2}.{3} Locale {4}", mLocalEndpoint, mRemoteEndpoint, mBuild, patchLocation, serverLocale);
            }
            if (pTCPPacket.SourcePort == mLocalPort) ProcessTCPPacket(pTCPPacket, ref mOutboundSequence, mOutboundBuffer, mOutboundStream, pArrivalTime);
            else ProcessTCPPacket(pTCPPacket, ref mInboundSequence, mInboundBuffer, mInboundStream, pArrivalTime);
            return Results.Continue;
        }

        private void ProcessTCPPacket(TcpPacket pTCPPacket, ref uint pSequence, Dictionary<uint, byte[]> pBuffer, MapleStream pStream, DateTime pArrivalDate)
        {
            if (pTCPPacket.SequenceNumber > pSequence)
            {
                byte[] data;
                while ((data = pBuffer.GetOrDefault(pSequence, null)) != null)
                {
                    pBuffer.Remove(pSequence);
                    pStream.Append(data);
                    pSequence += (uint)data.Length;
                }
                if (pTCPPacket.SequenceNumber > pSequence) pBuffer[(uint)pTCPPacket.SequenceNumber] = pTCPPacket.PayloadData;
            }
            if (pTCPPacket.SequenceNumber < pSequence)
            {
                int difference = (int)(pSequence - pTCPPacket.SequenceNumber);
                if (difference > 0)
                {
                    byte[] data = pTCPPacket.PayloadData;
                    if (data.Length > difference)
                    {
                        pStream.Append(data, difference, data.Length - difference);
                        pSequence += (uint)(data.Length - difference);
                    }
                }
            }
            else if (pTCPPacket.SequenceNumber == pSequence)
            {
                byte[] data = pTCPPacket.PayloadData;
                pStream.Append(data);
                pSequence += (uint)data.Length;
            }

            MaplePacket packet;
            bool refreshOpcodes = false;
            try
            {
                while ((packet = pStream.Read(pArrivalDate, mBuild, mLocale)) != null)
                {
                    mPackets.Add(packet);
					Definition definition = Config.Instance.GetDefinition(mBuild, mLocale, packet.Outbound, packet.Opcode);
                    if (!mOpcodes.Exists(kv => kv.First == packet.Outbound && kv.Second == packet.Opcode))
                    {
                        mOpcodes.Add(new Pair<bool, ushort>(packet.Outbound, packet.Opcode));
                        refreshOpcodes = true;
                    }
                    if (definition != null && !mViewIgnoredMenu.Checked && definition.Ignore) continue;
                    mPacketList.Items.Add(packet);
                    if (mPacketList.SelectedItems.Count == 0) packet.EnsureVisible();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                mTerminated = true;
                Text += " (Terminated)";
                MainForm.CloseSession(this);
                return;
            }
            
            if (DockPanel.ActiveDocument == this && refreshOpcodes) MainForm.SearchForm.RefreshOpcodes(true);
        }

        public void OpenReadOnly(string pFilename)
        {
            // mFileSaveMenu.Enabled = false;

            mTerminated = true;
            using (FileStream stream = new FileStream(pFilename, FileMode.Open, FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(stream);
                mBuild = reader.ReadUInt16();
                ushort version = mBuild;
                if (mBuild == 0x2012)
                {
                    mLocale = (byte)reader.ReadUInt16();
                    mBuild = reader.ReadUInt16();
                    mLocalPort = reader.ReadUInt16();
                }
                else if (mBuild == 0x2014)
                {
                    mLocalEndpoint = reader.ReadString();
                    mLocalPort = reader.ReadUInt16();
                    mRemoteEndpoint = reader.ReadString();
                    mRemotePort = reader.ReadUInt16();

                    mLocale = (byte)reader.ReadUInt16();
                    mBuild = reader.ReadUInt16();
                }
                else if (mBuild == 0x2015 || mBuild == 0x2020)
                {
                    mLocalEndpoint = reader.ReadString();
                    mLocalPort = reader.ReadUInt16();
                    mRemoteEndpoint = reader.ReadString();
                    mRemotePort = reader.ReadUInt16();

                    mLocale = reader.ReadByte();
                    mBuild = reader.ReadUInt16();
                }
                else
                {
                    mLocalPort = reader.ReadUInt16();
                    // Old version
                    frmLocale loc = new frmLocale();
                    var res = loc.ShowDialog();
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        mLocale = loc.ChosenLocale;
                    }
                }
				mPacketList.BeginUpdate();
                while (stream.Position < stream.Length)
                {
                    long timestamp = reader.ReadInt64();
                    ushort size = reader.ReadUInt16();
                    ushort opcode = reader.ReadUInt16();
                    bool outbound;
                    if (version >= 0x2020) {
                        outbound = reader.ReadBoolean();
                    }
                    else {
                        outbound = (size & 0x8000) != 0;
                        size = (ushort)(size & 0x7FFF);
                    }
                    byte[] buffer = reader.ReadBytes(size);
					Definition definition = Config.Instance.GetDefinition(mBuild, mLocale, outbound, opcode);
                    MaplePacket packet = new MaplePacket(new DateTime(timestamp), outbound, mBuild, mLocale, opcode, definition == null ? "" : definition.Name, buffer);
                    mPackets.Add(packet);
                    if (!mOpcodes.Exists(kv => kv.First == packet.Outbound && kv.Second == packet.Opcode)) mOpcodes.Add(new Pair<bool, ushort>(packet.Outbound, packet.Opcode));
                    if (definition != null && definition.Ignore) continue;
                    mPacketList.Items.Add(packet);
                }
				mPacketList.EndUpdate();
                if (mPacketList.Items.Count > 0) mPacketList.EnsureVisible(0);
            }
            
            Text = string.Format("{0} (ReadOnly)", Path.GetFileName(pFilename));
            Console.WriteLine("Loaded file: {0}", pFilename);
        }

        public void RefreshPackets()
        {
            Pair<bool, ushort> search = (MainForm.SearchForm.ComboBox.SelectedIndex >= 0 ? mOpcodes[MainForm.SearchForm.ComboBox.SelectedIndex] : null);
            MaplePacket previous = mPacketList.SelectedItems.Count > 0 ? mPacketList.SelectedItems[0] as MaplePacket : null;
            mOpcodes.Clear();
            mPacketList.Items.Clear();

            MainForm.DataForm.HexBox.ByteProvider = null;
            MainForm.StructureForm.Tree.Nodes.Clear();
            MainForm.PropertyForm.Properties.SelectedObject = null;

            if (!mViewOutboundMenu.Checked && !mViewInboundMenu.Checked) return;
			mPacketList.BeginUpdate();
            for (int index = 0; index < mPackets.Count; ++index)
            {
                MaplePacket packet = mPackets[index];
                if (packet.Outbound && !mViewOutboundMenu.Checked) continue;
                if (!packet.Outbound && !mViewInboundMenu.Checked) continue;
				Definition definition = Config.Instance.GetDefinition(mBuild, mLocale, packet.Outbound, packet.Opcode);
                packet.Name = definition == null ? "" : definition.Name;
                if (!mOpcodes.Exists(kv => kv.First == packet.Outbound && kv.Second == packet.Opcode)) mOpcodes.Add(new Pair<bool, ushort>(packet.Outbound, packet.Opcode));
                if (definition != null && !mViewIgnoredMenu.Checked && definition.Ignore) continue;
                mPacketList.Items.Add(packet);
                if (packet == previous) packet.Selected = true;
			}
			mPacketList.EndUpdate();
            MainForm.SearchForm.RefreshOpcodes(true);
        }

        public void RunSaveCMD()
        {
            mFileSaveMenu.PerformClick();
        }

        private void mFileSaveMenu_Click(object pSender, EventArgs pArgs)
        {
			if (!mTerminated) {
				MessageBox.Show("Cannot save this session when it's still open.");
			}
            if (mFilename == null)
            {
                mSaveDialog.FileName = string.Format("Port {0}", mLocalPort);
                if (mSaveDialog.ShowDialog(this) == DialogResult.OK) mFilename = mSaveDialog.FileName;
                else return;
            }
            using (FileStream stream = new FileStream(mFilename, FileMode.Create, FileAccess.Write))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write((ushort)0x2020);
                writer.Write(mLocalEndpoint);
                writer.Write(mLocalPort);
                writer.Write(mRemoteEndpoint);
                writer.Write(mRemotePort);
                writer.Write(mLocale);
                writer.Write(mBuild);
				foreach (MaplePacket packet in mPackets) {
					writer.Write(packet.Dump());
				}
                stream.Flush();
            }
            if (mTerminated)
            {
                mFileSaveMenu.Enabled = false;
                Text = string.Format("Port {0} (ReadOnly)", mLocalPort);
            }
        }

        private void mFileExportMenu_Click(object pSender, EventArgs pArgs)
        {
            mExportDialog.FileName = string.Format("Port {0}", mLocalPort);
            if (mExportDialog.ShowDialog(this) != DialogResult.OK) return;

            string tmp = "";
            tmp += string.Format("=== MapleStory Version: {0}; Locale: {1} ===\r\n", mBuild, mLocale);
            tmp += string.Format("Endpoint From: {0}\r\n", mLocalEndpoint);
            tmp += string.Format("Endpoint To: {0}\r\n", mRemoteEndpoint);
            tmp += string.Format("- Packets: {0}\r\n", mPackets.Count);

            long dataSize = 0;
            foreach (var packet in mPackets)
                dataSize += 2 + packet.InnerBuffer.Length;

            tmp += string.Format("- Data: {0:N0} bytes\r\n", dataSize);
            tmp += string.Format("================================================\r\n");
            File.WriteAllText(mExportDialog.FileName, tmp);

            tmp = "";

            int outboundCount = 0;
            int inboundCount = 0;
            int i = 0;
            foreach (var packet in mPackets)
            {
                if (packet.Outbound) ++outboundCount;
                else ++inboundCount;
                tmp += string.Format("[{0}][{2}] [{3:X4}] {4}\r\n", packet.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"), (packet.Outbound ? outboundCount : inboundCount), (packet.Outbound ? "Outbound" : "Inbound "), packet.Opcode, BitConverter.ToString(packet.InnerBuffer).Replace('-', ' '));
                i++;
                if (i % 1000 == 0)
                {
                    File.AppendAllText(mExportDialog.FileName, tmp);
                    tmp = "";
                }
            }
            File.AppendAllText(mExportDialog.FileName, tmp);
        }

        private void mViewCommonScriptMenu_Click(object pSender, EventArgs pArgs)
        {
			string scriptPath = "Scripts" + Path.DirectorySeparatorChar + mLocale.ToString() + Path.DirectorySeparatorChar + mBuild.ToString() + Path.DirectorySeparatorChar + "Common.txt";
            if (!Directory.Exists(Path.GetDirectoryName(scriptPath))) Directory.CreateDirectory(Path.GetDirectoryName(scriptPath));
            ScriptForm script = new ScriptForm(scriptPath, null);
            script.FormClosed += CommonScript_FormClosed;
            script.Show(DockPanel, new Rectangle(MainForm.Location, new Size(600, 300)));
        }

        private void CommonScript_FormClosed(object pSender, FormClosedEventArgs pArgs)
        {
            if (mPacketList.SelectedIndices.Count == 0) return;
            MaplePacket packet = mPacketList.SelectedItems[0] as MaplePacket;
            MainForm.StructureForm.ParseMaplePacket(packet);
            Activate();
        }

        private void mViewRefreshMenu_Click(object pSender, EventArgs pArgs) { RefreshPackets(); }
        private void mViewOutboundMenu_CheckedChanged(object pSender, EventArgs pArgs) { RefreshPackets(); }
        private void mViewInboundMenu_CheckedChanged(object pSender, EventArgs pArgs) { RefreshPackets(); }
        private void mViewIgnoredMenu_CheckedChanged(object pSender, EventArgs pArgs) { RefreshPackets(); }

        private void mPacketList_SelectedIndexChanged(object pSender, EventArgs pArgs)
        {
            if (mPacketList.SelectedItems.Count == 0) { MainForm.DataForm.HexBox.ByteProvider = null; MainForm.StructureForm.Tree.Nodes.Clear(); MainForm.PropertyForm.Properties.SelectedObject = null; return; }
            MainForm.DataForm.HexBox.ByteProvider = new DynamicByteProvider((mPacketList.SelectedItems[0] as MaplePacket).InnerBuffer);
            MainForm.StructureForm.ParseMaplePacket(mPacketList.SelectedItems[0] as MaplePacket);
        }

        private void mPacketList_ItemActivate(object pSender, EventArgs pArgs)
        {
            if (mPacketList.SelectedIndices.Count == 0) return;
            MaplePacket packet = mPacketList.SelectedItems[0] as MaplePacket;
			string scriptPath = "Scripts" + Path.DirectorySeparatorChar + mLocale.ToString() + Path.DirectorySeparatorChar + mBuild.ToString() + Path.DirectorySeparatorChar + (packet.Outbound ? "Outbound" : "Inbound") + Path.DirectorySeparatorChar + "0x" + packet.Opcode.ToString("X4") + ".txt";
            if (!Directory.Exists(Path.GetDirectoryName(scriptPath))) Directory.CreateDirectory(Path.GetDirectoryName(scriptPath));
            ScriptForm script = new ScriptForm(scriptPath, packet);
            script.FormClosed += Script_FormClosed;
            script.Show(DockPanel, new Rectangle(MainForm.Location, new Size(600, 300)));
        }

        private void Script_FormClosed(object pSender, FormClosedEventArgs pArgs)
        {
            ScriptForm script = pSender as ScriptForm;
            script.Packet.Selected = true;
            MainForm.StructureForm.ParseMaplePacket(script.Packet);
            Activate();
        }

        private void mPacketContextMenu_Opening(object pSender, CancelEventArgs pArgs)
        {
            mPacketContextNameBox.Text = "";
            mPacketContextIgnoreMenu.Checked = false;
            if (mPacketList.SelectedItems.Count == 0) pArgs.Cancel = true;
            else
            {
                MaplePacket packet = mPacketList.SelectedItems[0] as MaplePacket;
				Definition definition = Config.Instance.GetDefinition(mBuild, mLocale, packet.Outbound, packet.Opcode);
                if (definition != null)
                {
                    mPacketContextNameBox.Text = definition.Name;
                    mPacketContextIgnoreMenu.Checked = definition.Ignore;
                }
            }
        }

        private void mPacketContextMenu_Opened(object pSender, EventArgs pArgs)
        {
            mPacketContextNameBox.Focus();
            mPacketContextNameBox.SelectAll();
        }

        private void mPacketContextNameBox_KeyDown(object pSender, KeyEventArgs pArgs)
        {
            if (pArgs.Modifiers == Keys.None && pArgs.KeyCode == Keys.Enter)
            {
                MaplePacket packet = mPacketList.SelectedItems[0] as MaplePacket;
				Definition definition = Config.Instance.GetDefinition(mBuild, mLocale, packet.Outbound, packet.Opcode);
                if (definition == null)
                {
                    definition = new Definition();
                    definition.Build = mBuild;
                    definition.Outbound = packet.Outbound;
					definition.Opcode = packet.Opcode;
					definition.Locale = mLocale;
                    Config.Instance.Definitions.Add(definition);
                }
                definition.Name = mPacketContextNameBox.Text;
                Config.Instance.Save();
                pArgs.SuppressKeyPress = true;
                mPacketContextMenu.Close();
                RefreshPackets();
            }
        }

        private void mPacketContextIgnoreMenu_CheckedChanged(object pSender, EventArgs pArgs)
        {
            MaplePacket packet = mPacketList.SelectedItems[0] as MaplePacket;
			Definition definition = Config.Instance.GetDefinition(mBuild, mLocale, packet.Outbound, packet.Opcode);
            if (definition == null)
            {
                definition = new Definition();
				definition.Locale = mLocale;
                definition.Build = mBuild;
                definition.Outbound = packet.Outbound;
				definition.Opcode = packet.Opcode;
				definition.Locale = mLocale;
                Config.Instance.Definitions.Add(definition);
            }
            definition.Ignore = mPacketContextIgnoreMenu.Checked;
            Config.Instance.Save();
            RefreshPackets();
        }

        private void SessionForm_Load(object sender, EventArgs e)
        {

        }

		private void mMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

		}

		private void sessionInformationToolStripMenuItem_Click(object sender, EventArgs e) {
			SessionInformation si = new SessionInformation();
			si.txtVersion.Text = mBuild.ToString();
			si.txtPatchLocation.Text = mPatchLocation;
			si.txtLocale.Text = mLocale.ToString();
			si.txtAdditionalInfo.Text = "Connection info:\r\n" + mLocalEndpoint + " to " + mRemoteEndpoint;
			if ((mLocale == 1 && mBuild != 255) || mLocale == 2) {
				si.txtAdditionalInfo.Text += "\r\nRecording session of a MapleStory Korea" + (mLocale == 1 ? " Test" : "") + " server.\r\nAdditional KMS info:\r\n";

				int test = int.Parse(mPatchLocation);
				ushort maplerVersion = (ushort)(test & 0x7FFF);
				int subVersion = (test >> 15) & 1;
				int extraOption = (test >> 16) & 0xFF;
                si.txtAdditionalInfo.Text += "Real Version: " + maplerVersion + "\r\nSubversion: " + subVersion + "\r\nRemove cookie: " + extraOption;
			}

			si.Show();
		}

        private void sendpropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.Instance.SaveProperties();
            string tmp = Config.GetPropertiesFile(true, (byte)mLocale, mBuild);
            System.Diagnostics.Process.Start(tmp);
        }

        private void recvpropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.Instance.SaveProperties();
            string tmp = Config.GetPropertiesFile(false, (byte)mLocale, mBuild);
            System.Diagnostics.Process.Start(tmp);
        }

        private void removeLoggedPacketsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all logged packets?", "!!", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) return;

            mPackets.Clear();
            ListView.Items.Clear();
            mOpcodes.Clear();
        }
    }
}
