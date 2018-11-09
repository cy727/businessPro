using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormLACCESS : Form
    {
        public bool isAccept=false;
        public string strPass = "";

        public FormLACCESS()
        {
            InitializeComponent();
        }

        private void FormLACCESS_Load(object sender, EventArgs e)
        {
            isAccept = false;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (strPass == textBoxPASS.Text)
            {
                isAccept = true;
                this.Close();
                return;
            }
            else
            {
                MessageBox.Show("授权码错误，无法保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                isAccept = false;
            }


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isAccept = false;
        }
    }
}
