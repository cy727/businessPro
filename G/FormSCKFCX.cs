using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSCKFCX : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;



        private ClassGetInformation cGetInformation;

        public FormSCKFCX()
        {
            InitializeComponent();
        }

        private void FormSCKFCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            toolStripButtonGD_Click(null, null);
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {

            sqlConn.Open();
            //��ʼ�б�
            sqlComm.CommandText = "SELECT �ⷿ���, �ⷿ����, ������, ��� FROM �ⷿ�� WHERE (BeActive = 0)";

            if (textBoxMC.Text.Trim() != "")
                sqlComm.CommandText += " AND ((�ⷿ���� LIKE N'%" + textBoxMC.Text.Trim() + "%') OR (������ LIKE N'%" + textBoxMC.Text.Trim() + "%'))";

            if (dSet.Tables.Contains("�ⷿ��")) dSet.Tables.Remove("�ⷿ��");
            sqlDA.Fill(dSet, "�ⷿ��");
            dataGridViewDJMM.DataSource = dSet.Tables["�ⷿ��"];



            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelSCJZRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            toolStripStatusLabelC.Text = "����" + dSet.Tables["�ⷿ��"].Rows.Count.ToString() + "����¼";

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {

        }
    }
}