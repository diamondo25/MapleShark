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

        private void frmLocale_Load(object sender, EventArgs e)
        {
            cbLocale.Items.AddRange(new object[] {
                new Locale("Korea", MapleLocale.KOREA),
                new Locale("Korea Test", MapleLocale.KOREA_TEST),
                new Locale("Japan", MapleLocale.JAPAN),
                new Locale("China", MapleLocale.CHINA),
                new Locale("Tespia", MapleLocale.TESPIA),
                new Locale("Taiwan", MapleLocale.TAIWAN),
                new Locale("South-East Asia", MapleLocale.SOUTH_EAST_ASIA),
                new Locale("Global", MapleLocale.GLOBAL),
                new Locale("Europe", MapleLocale.EUROPE),
                new Locale("Brazil", MapleLocale.BRAZIL),
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
