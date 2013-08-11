using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace MapleShark
{
    public partial class frmImportProps : Form
    {
        public frmImportProps()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ofdPropFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPropFile.Text = ofdPropFile.FileName;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtPropFile.Text))
            {
                MessageBox.Show("The file you selected doesn't exist!");
                return;
            }

            txtLog.Text = "";
            byte locale = Convert.ToByte(nudLocale.Value);
            ushort version = Convert.ToUInt16(nudVersion.Value);

            string[] opcodes = File.ReadAllLines(txtPropFile.Text);
            List<ushort> loadedOps = new List<ushort>();
            foreach (string opcode in opcodes)
            {
                var val = opcode;
                if (val.Contains("#"))
                    val = val.Remove(val.IndexOf('#'));

                val = val.Trim();
                if (val == "") continue;

                string[] splitted = val.Split('=');
                if (splitted.Length != 2) continue;

                string name = splitted[0].Trim();
                ushort header = 0;

                string headerval = splitted[1].Trim();
                if (headerval.StartsWith("0x"))
                    header = ushort.Parse(headerval.Substring(2), System.Globalization.NumberStyles.HexNumber);
                else
                    header = ushort.Parse(headerval, System.Globalization.NumberStyles.Integer);

                AddOpcode(version, locale, !chkIsSend.Checked, header, name);
            }
            Config.Instance.Save();
        }

        private void AddOpcode(ushort pBuild, ushort pLocale, bool pOutbound, ushort pOpcode, string pName)
        {
            Definition def = Config.Instance.GetDefinition(pBuild, pLocale, pOutbound, pOpcode);
            if (def == null)
            {
                def = new Definition();
                txtLog.AppendText(string.Format("Adding opcode {1}: 0x{0:X4}\r\n", pOpcode, pName));
            }
            else
            {
                txtLog.AppendText(string.Format("Replacing opcode {1} 0x{0:X4} for {2}\r\n", pOpcode, def.Name, pName));
                Config.Instance.Definitions.Remove(def);
            }

            def.Build = pBuild;
            def.Locale = pLocale;
            def.Opcode = pOpcode;
            def.Outbound = pOutbound;
            def.Name = pName;
            def.Ignore = false;

            Config.Instance.Definitions.Add(def);
        }
    }
}
