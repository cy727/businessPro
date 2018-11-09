using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSCSPCX : Form
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
        
        public FormSCSPCX()
        {
            InitializeComponent();
        }

        private void FormSCSPCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            toolStripButtonGD_Click(null, null);

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "ɾ����Ʒ��ѯ;��ǰ���ڣ�" + labelSCJZRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMM, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "ɾ����Ʒ��ѯ;��ǰ���ڣ�" + labelSCJZRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMM, strT, false, intUserLimit);
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            //��ʼ�б�
            sqlComm.CommandText = "SELECT ��Ʒ���, ��Ʒ����, ������, ��С������λ AS ��λ, ��Ʒ��� FROM ��Ʒ�� WHERE (beactive = 0)";
            if (textBoxMC.Text.Trim() != "")
                sqlComm.CommandText += " AND ((��Ʒ���� LIKE N'%" + textBoxMC.Text.Trim() + "%') OR (������ LIKE N'%" + textBoxMC.Text.Trim() + "%'))";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMM.DataSource = dSet.Tables["��Ʒ��"];

            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelSCJZRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            toolStripStatusLabelC.Text = "����" + dSet.Tables["��Ʒ��"].Rows.Count.ToString() + "����¼";
        }
    }
}