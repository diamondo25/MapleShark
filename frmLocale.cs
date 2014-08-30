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
            if (ChosenLocale == 0)
            {
                ChosenLocale = ((Locale)cbLocale.SelectedItem).ID;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void frmLocale_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void frmLocale_Load(object sender, EventArgs e)
        {
            cbLocale.Items.AddRange(new object[] {
                new Locale("Korea", 1),
                new Locale("Korea Test", 2),
                new Locale("Japan", 3),
                new Locale("China", 4),
                new Locale("Global Test", 5),
                new Locale("Taiwan", 6),
                new Locale("South-East Asia", 7),
                new Locale("Global", 8),
                new Locale("Europe", 9),
                new Locale("Brazil", 9),
            });
            cbLocale.SelectedIndex = 0;
        }


        class Locale
        {
            public string Name { get; set; }
            public byte ID { get; set; }

            public Locale(string pName, byte pId)
            {
                Name = pName;
                ID = pId;
            }

            public override string ToString()
            {
                return Name + " (" + ID + ")";
            }
        }
    }
}
