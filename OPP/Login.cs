using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OPP
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new Form1();
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Show(); };
            frm.Show();
            this.Hide();
        }
    }
}
