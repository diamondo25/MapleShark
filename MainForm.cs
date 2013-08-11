﻿using SharpPcap;
using SharpPcap.LibPcap;
using PacketDotNet;
using PacketDotNet.Utils;
using PacketDotNet.LLDP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MapleShark
{
    public partial class MainForm : Form
    {
        private bool mClosed = false;
        private LibPcapLiveDevice mDevice = null;
        private SearchForm mSearchForm = new SearchForm();
        private DataForm mDataForm = new DataForm();
        private StructureForm mStructureForm = new StructureForm();
        private PropertyForm mPropertyForm = new PropertyForm();

        public MainForm(string[] pArgs)
        {
            InitializeComponent();
            Text = "MapleShark " + Program.AssemblyVersion;
            foreach (string arg in pArgs)
            {
                SessionForm session = NewSession();
                session.OpenReadOnly(arg);
                session.Show(mDockPanel, DockState.Document);
            }
        }

        public SearchForm SearchForm { get { return mSearchForm; } }
        public DataForm DataForm { get { return mDataForm; } }
        public StructureForm StructureForm { get { return mStructureForm; } }
        public PropertyForm PropertyForm { get { return mPropertyForm; } }

        PcapDevice device;

        private SessionForm NewSession()
        {
            SessionForm session = new SessionForm();
            return session;
        }

        public void CloseSession(SessionForm form)
        {
            mDockPanel.Contents.Remove(form);
        }

        public void CopyPacketHex(KeyEventArgs pArgs)
        {
            if (mDataForm.HexBox.SelectionLength > 0 && pArgs.Modifiers == Keys.Control && pArgs.KeyCode == Keys.C)
            {
                Clipboard.SetText(BitConverter.ToString((mDataForm.HexBox.ByteProvider as DynamicByteProvider).Bytes.ToArray(), (int)mDataForm.HexBox.SelectionStart, (int)mDataForm.HexBox.SelectionLength).Replace("-", " "));
                pArgs.SuppressKeyPress = true;
            }
            else if (mDataForm.HexBox.SelectionLength > 0 && pArgs.Control && pArgs.Shift && pArgs.KeyCode == Keys.C)
            {
                byte[] buffer = new byte[mDataForm.HexBox.SelectionLength];
                Buffer.BlockCopy((mDataForm.HexBox.ByteProvider as DynamicByteProvider).Bytes.ToArray(), (int)mDataForm.HexBox.SelectionStart, buffer, 0, (int)mDataForm.HexBox.SelectionLength);
                mSearchForm.HexBox.ByteProvider.DeleteBytes(0, mSearchForm.HexBox.ByteProvider.Length);
                mSearchForm.HexBox.ByteProvider.InsertBytes(0, buffer);
                pArgs.SuppressKeyPress = true;
            }
        }

        private void MainForm_Load(object pSender, EventArgs pArgs)
        {
            if (new SetupForm().ShowDialog(this) != DialogResult.OK) { Close(); return; }
            foreach (LibPcapLiveDevice device in LibPcapLiveDeviceList.Instance)
            {
                if (device.Interface.FriendlyName == Config.Instance.Interface)
                {
                    mDevice = device;
                    break;
                }
            }
            try
            {
                mDevice.Open(DeviceMode.Promiscuous, 1);
            }
            catch
            {
                MessageBox.Show("Failed to set the device in Promiscuous mode! But that doesn't really matter lol.");
                mDevice.Open();
            }
            mDevice.Filter = string.Format("tcp portrange {0}-{1}", Config.Instance.LowPort, Config.Instance.HighPort);
            mTimer.Enabled = true;

            mSearchForm.Show(mDockPanel);
            mDataForm.Show(mDockPanel);
            mStructureForm.Show(mDockPanel);
            mPropertyForm.Show(mDockPanel);
            DockPane rightPane1 = new DockPane(mStructureForm, DockState.DockRight, true);
            DockPane rightPane2 = new DockPane(mPropertyForm, DockState.DockRight, true);
            rightPane1.Show();
            rightPane2.Show();
        }

        private void MainForm_FormClosed(object pSender, FormClosedEventArgs pArgs)
        {
            mTimer.Enabled = false;
            if (mDevice != null) mDevice.Close();
            mClosed = true;
        }

        private void mDockPanel_ActiveDocumentChanged(object pSender, EventArgs pArgs)
        {
            if (!mClosed)
            {
                SessionForm session = mDockPanel.ActiveDocument as SessionForm;
                mSearchForm.ComboBox.Items.Clear();
                if (session != null) session.RefreshPackets();
                else
                {
                    if (mDataForm.HexBox.ByteProvider != null) mDataForm.HexBox.ByteProvider.DeleteBytes(0, mDataForm.HexBox.ByteProvider.Length);
                    mStructureForm.Tree.Nodes.Clear();
                    mPropertyForm.Properties.SelectedObject = null;
                }
            }
        }

        private void mFileImportMenu_Click(object pSender, EventArgs pArgs)
        {
            if (mImportDialog.ShowDialog(this) != DialogResult.OK) return;
            device = new CaptureFileReaderDevice(mImportDialog.FileName);
            device.Open();
            new Thread(ParseImportedFile).Start();
        }

        void ParseImportedFile()
        {
			while (device.Opened) {
                RawCapture packet = null;
				SessionForm session = null;
				while ((packet = device.GetNextPacket()) != null) {
                    if (!started) 
                        continue;
					TcpPacket tcpPacket = TcpPacket.GetEncapsulated(Packet.ParsePacket(packet.LinkLayerType, packet.Data));
					if (tcpPacket == null)
						continue;

					if ((tcpPacket.SourcePort < Config.Instance.LowPort || tcpPacket.SourcePort > Config.Instance.HighPort) &&
						(tcpPacket.DestinationPort < Config.Instance.LowPort || tcpPacket.DestinationPort > Config.Instance.HighPort))
						continue;
					this.Invoke((MethodInvoker)delegate {
                        try
                        {
                            if (tcpPacket.Syn && !tcpPacket.Ack)
                            {
                                session = NewSession();
                                var res = session.BufferTCPPacket(tcpPacket, packet.Timeval.Date);
                                if (res == SessionForm.Results.Continue)
                                    session.Show(mDockPanel, DockState.Document);
                            }
                            else if (session != null && session.MatchTCPPacket(tcpPacket))
                            {
                                var res = session.BufferTCPPacket(tcpPacket, packet.Timeval.Date);
                                if (res == SessionForm.Results.CloseMe)
                                {
                                    session.Close();
                                }
                            }

                        }
                        catch (Exception)
                        {
                            session.Close();
                            session = null;
                        }
					});
				}
				this.Invoke((MethodInvoker)delegate {
					mSearchForm.RefreshOpcodes(false);
				});
			}
        }

        private void mFileOpenMenu_Click(object pSender, EventArgs pArgs)
        {
            if (mOpenDialog.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string path in mOpenDialog.FileNames)
                {
                    SessionForm session = NewSession();
                    session.OpenReadOnly(mOpenDialog.FileName);
                    session.Show(mDockPanel, DockState.Document);
                }
                mSearchForm.RefreshOpcodes(false);
            }
        }

        private void mFileQuit_Click(object pSender, EventArgs pArgs)
        {
            Close();
        }

        private void mViewMenu_DropDownOpening(object pSender, EventArgs pArgs)
        {
            mViewSearchMenu.Checked = mSearchForm.Visible;
            mViewDataMenu.Checked = mDataForm.Visible;
            mViewStructureMenu.Checked = mStructureForm.Visible;
            mViewPropertiesMenu.Checked = mPropertyForm.Visible;
        }

        private void mViewSearchMenu_CheckedChanged(object pSender, EventArgs pArgs)
        {
            if (mViewSearchMenu.Checked) mSearchForm.Show();
            else mSearchForm.Hide();

        }

        private void mViewDataMenu_CheckedChanged(object pSender, EventArgs pArgs)
        {
            if (mViewDataMenu.Checked) mDataForm.Show();
            else mDataForm.Hide();
        }

        private void mViewStructureMenu_CheckedChanged(object pSender, EventArgs pArgs)
        {
            if (mViewStructureMenu.Checked) mStructureForm.Show();
            else mStructureForm.Hide();
        }

        private void mViewPropertiesMenu_CheckedChanged(object pSender, EventArgs pArgs)
        {
            if (mViewPropertiesMenu.Checked) mPropertyForm.Show();
            else mPropertyForm.Hide();
        }

        List<SessionForm> closes = new List<SessionForm>();
        private void mTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                RawCapture packet = null;
                mTimer.Enabled = false;

                DateTime now = DateTime.Now;
                foreach (SessionForm ses in MdiChildren) 
                {
                    if (ses.CloseMe(now))
                        closes.Add(ses);
                }
                closes.ForEach((a) => { a.Close(); });
                closes.Clear();

                while ((packet = mDevice.GetNextPacket()) != null)
                {
                    if (!started)
                        continue;
                    TcpPacket tcpPacket = TcpPacket.GetEncapsulated(Packet.ParsePacket(packet.LinkLayerType, packet.Data));
                    SessionForm session = null;
                    try
                    {
                        if (tcpPacket.Syn && !tcpPacket.Ack && tcpPacket.DestinationPort >= Config.Instance.LowPort && tcpPacket.DestinationPort <= Config.Instance.HighPort)
                        {
                            session = NewSession();
                            var res = session.BufferTCPPacket(tcpPacket, packet.Timeval.Date);
                            if (res == SessionForm.Results.Continue)
                            {
                                session.Show(mDockPanel, DockState.Document);
                            }
                        }
                        else
                        {
                            session = Array.Find(MdiChildren, f => (f as SessionForm).MatchTCPPacket(tcpPacket)) as SessionForm;
                            if (session != null)
                            {
                               var res =  session.BufferTCPPacket(tcpPacket, packet.Timeval.Date);

                               if (res == SessionForm.Results.CloseMe)
                               {
                                   session.Close();
                               }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        session.Close();
                        session = null;
                    }
                }
                mTimer.Enabled = true;
            }
            catch (Exception)
            {
                if (!mDevice.Opened)
                    mDevice.Open(DeviceMode.Promiscuous, 1);
            }
        }

        bool started = true;
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (started)
            {
                started = false;
                mStopStartButton.Image = Properties.Resources.Button_Blank_Green_icon;
                mStopStartButton.Text = "Start sniffing";
            }
            else
            {
                started = true;
                mStopStartButton.Image = Properties.Resources.Button_Blank_Red_icon;
                mStopStartButton.Text = "Stop sniffing";
            }
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("Readme.txt"))
            {
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + @"\Readme.txt");
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SessionForm session = mDockPanel.ActiveDocument as SessionForm;
            if (session != null)
            {
                session.RunSaveCMD();
            }
        }

        private void importJavapropertiesFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmImportProps().ShowDialog();
        }
    }
}
