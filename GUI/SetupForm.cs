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
                var devInterface = device.Interface;
                var friendlyName = devInterface.FriendlyName;
                var description = devInterface.Description;

                int index = mInterfaceCombo.Items.Add(friendlyName);
                if ((friendlyName == "Local Area Connection" || friendlyName.Contains("LAN")) && !description.Contains("TAP") && !description.Contains("VPN")) localAreaConnection = index;

                if (!selected && (selected = (friendlyName == Config.Instance.Interface))) mInterfaceCombo.SelectedIndex = index;
            }

            if (!selected && localAreaConnection >= 0) mInterfaceCombo.SelectedIndex = localAreaConnection;
            else if (!selected && mInterfaceCombo.Items.Count > 0) mInterfaceCombo.SelectedIndex = 0;
            mLowPortNumeric.Value = Config.Instance.LowPort;
            mHighPortNumeric.Value = Config.Instance.HighPort;
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

        private void SetupForm_Load(object sender, EventArgs e)
        {

        }
    }
}
