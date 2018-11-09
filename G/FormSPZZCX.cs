using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPZZCX : Form
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

        public FormSPZZCX()
        {
            InitializeComponent();
        }

        private void FormSPZZCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;
            //�õ���ʼʱ��
            sqlConn.Open();
            //sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
            sqlComm.CommandText = "SELECT GETDATE()";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");

            }
            sqldr.Close();
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����Ʒ��װ���ܱ�.���ݱ��, �����Ʒ��װ���ܱ�.����, �����Ʒ��װ���ܱ�.��Ʒ����, ����Ա.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա FROM �����Ʒ��װ���ܱ� INNER JOIN �ⷿ�� ON �����Ʒ��װ���ܱ�.��Ʒ�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON �����Ʒ��װ���ܱ�.��ƷID = ��Ʒ��.ID INNER JOIN ְԱ�� ����Ա ON �����Ʒ��װ���ܱ�.����ԱID = ����Ա.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����Ʒ��װ���ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (�����Ʒ��װ���ܱ�.���� >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (�����Ʒ��װ���ܱ�.���� <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102))";

            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");

            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �����Ʒ��װ���ܱ�.���ݱ��, �����Ʒ��װ���ܱ�.����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����Ʒ��װ��ϸ��.�������, �����Ʒ��װ��ϸ��.�ɱ�����, �����Ʒ��װ��ϸ��.�ɱ���� FROM �����Ʒ��װ���ܱ� INNER JOIN �����Ʒ��װ��ϸ�� ON �����Ʒ��װ���ܱ�.ID = �����Ʒ��װ��ϸ��.����ID INNER JOIN ��Ʒ�� ON �����Ʒ��װ��ϸ��.���ID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON �����Ʒ��װ��ϸ��.�ⷿID = �ⷿ��.ID WHERE (�����Ʒ��װ���ܱ�.���� >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (�����Ʒ��װ���ܱ�.���� <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102))";

            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");

            sqlConn.Close();

            dataGridView1.DataSource = dSet.Tables["��Ʒ��1"];
            dataGridView2.DataSource = dSet.Tables["��Ʒ��2"];
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "��Ʒ��װ��ѯ����װ��Ʒ��ѯ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "��Ʒ��װ��ѯ�������ɲ�ѯ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "��Ʒ��װ��ѯ����װ��Ʒ��ѯ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "��Ʒ��װ��ѯ�������ɲ�ѯ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
            }
        }

    }
}