using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormMessage : Form
    {
        public FormMessage()
        {
            InitializeComponent();
        }

        public bool bOK = false;

        private void FormMessage_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bOK = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bOK = false;
            this.Close();
        }
    }
}
