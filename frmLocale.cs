using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapleShark
{
    public partial class frmLocale : Form
    {
        public byte ChosenLocale { get; set; }
        public frmLocale()
        {
            InitializeComponent();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            ChosenLocale = (byte)nudLocale.Value;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void frmLocale_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
