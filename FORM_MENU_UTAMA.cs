using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PENENTUAN_JALUR_TERPENDEK
{
    public partial class FORM_MENU_UTAMA : DevComponents.DotNetBar.Metro.MetroAppForm
    {
        private PENENTUAN_JALUR_TERPENDEK.FORM_ABOUT abt = new PENENTUAN_JALUR_TERPENDEK.FORM_ABOUT();


        public FORM_MENU_UTAMA()
        {
            InitializeComponent();
        }

        private void buttonItem2_Click(object sender, EventArgs e)
        {
            this.abt.ShowDialog();
        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
            PENENTUAN_JALUR_TERPENDEK.FORM_PENENTUAN_JALUR frm = new PENENTUAN_JALUR_TERPENDEK.FORM_PENENTUAN_JALUR();
            frm.ShowDialog();
        }
    }
}