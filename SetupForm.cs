using SharpPcap;
using SharpPcap.LibPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace MapleShark
{
    public partial class SetupForm : Form
    {
        public SetupForm()
        {
            InitializeComponent();

            Text = "MapleShark " + Program.AssemblyVersion + ", " + Program.AssemblyCopyright;
            bool selected = false;
            int localAreaConnection = -1;
            foreach (LibPcapLiveDevice device in LibPcapLiveDeviceList.Instance)
            {
                if (!device.Interface.Addresses.Exists(a => a != null && a.Addr != null && a.Addr.ipAddress != null)) continue;
                int index = mInterfaceCombo.Items.Add(device.Interface.FriendlyName);
                if (device.Interface.FriendlyName == "Local Area Connection") localAreaConnection = index;
                if (!selected && (selected = (device.Interface.FriendlyName == Config.Instance.Interface))) mInterfaceCombo.SelectedIndex = index;
            }
            if (!selected && localAreaConnection >= 0) mInterfaceCombo.SelectedIndex = localAreaConnection;
            else if (!selected && mInterfaceCombo.Items.Count > 0) mInterfaceCombo.SelectedIndex = 0;
            mLowPortNumeric.Value = Config.Instance.LowPort;
            mHighPortNumeric.Value = Config.Instance.HighPort;
        }

        private void SetupForm_Load(object pSender, EventArgs pArgs)
        {
            if (mInterfaceCombo.Items.Count == 0)
            {
                if (MessageBox.Show(this, "There are no usable interfaces. Did you install WinPcap? If you did, then try to run MapleShark in Administrator Mode, else press 'No' to go to the install page of WinPcap.", "Interface Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.No)
                {
                    System.Diagnostics.Process.Start("http://www.winpcap.org/install/default.htm");
                }
                Close();
                return;
            }
        }

        private void mInterfaceCombo_SelectedIndexChanged(object pSender, EventArgs pArgs)
        {
            mOKButton.Enabled = mInterfaceCombo.SelectedIndex >= 0;
        }

        private void mLowPortNumeric_ValueChanged(object pSender, EventArgs pArgs)
        {
            if (mLowPortNumeric.Value > mHighPortNumeric.Value) mLowPortNumeric.Value = mHighPortNumeric.Value;
        }

        private void mHighPortNumeric_ValueChanged(object pSender, EventArgs pArgs)
        {
            if (mHighPortNumeric.Value < mLowPortNumeric.Value) mHighPortNumeric.Value = mLowPortNumeric.Value;
        }

        private void mOKButton_Click(object pSender, EventArgs pArgs)
        {
            Config.Instance.Interface = (string)mInterfaceCombo.SelectedItem;
            Config.Instance.LowPort = (ushort)mLowPortNumeric.Value;
            Config.Instance.HighPort = (ushort)mHighPortNumeric.Value;
            Config.Instance.Save();
            DialogResult = DialogResult.OK;
        }
    }
}
