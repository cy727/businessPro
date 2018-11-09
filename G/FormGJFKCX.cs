using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormGJFKCX : Form
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

        private int iSupplyCompany = 0;
        private ClassGetInformation cGetInformation;
        private decimal[] cTemp = new decimal[2] { 0, 0 };

        public FormGJFKCX()
        {
            InitializeComponent();
        }

        private void FormGJFKCX_Load(object sender, EventArgs e)
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

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1, "") == 0)
            {
                //return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(10, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                }
            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(12, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            sqlComm.CommandText = "SELECT ���������ͼ.ID, ���������ͼ.���ݱ��, ���������ͼ.����, ��λ��.��λ���,��λ��.��λ����, ���������ͼ.��˰�ϼ�, ���������ͼ.δ������, ���������ͼ.��ע FROM ��λ�� INNER JOIN ���������ͼ ON ��λ��.ID = ���������ͼ.��λID WHERE (���������ͼ.δ������ <> 0) AND (���������ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ͼ.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");


            sqlComm.CommandText = "SELECT ���������ͼ.���ݱ��, ���������ͼ.����, ��λ��.��λ���, ��λ��.��λ����,ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ���������ͼ.��˰�ϼ�, ���������ͼ.δ������, ���������ͼ.��ע FROM ���������ͼ INNER JOIN ��λ�� ON ���������ͼ.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ͼ.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ���������ͼ.����ԱID = ����Ա.ID WHERE (���������ͼ.BeActive = 1) AND (���������ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ͼ.�Ѹ����� <> 0)";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");


            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["��Ʒ��1"];
            //dataGridView1.Columns[0].Visible = false;
            dataGridView2.DataSource = dSet.Tables["��Ʒ��2"];
            dataGridView2.Columns[0].Visible = false;
            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);  
        }
        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= cTemp.Length; i++)
            {
                cTemp[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 6;
                        break;
                    case 2:
                        c = 5;
                        break;
                    default:
                        c = 0;
                        break;
                }

                if (c != 0)
                {

                    for (j = 0; j < dSet.Tables["��Ʒ��" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp[i - 1] += decimal.Parse(dSet.Tables["��Ʒ��" + i.ToString()].Rows[j][c].ToString());
                        }
                        catch
                        {
                        }
                    }
                }


            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";
            strT = "���������ѯ��" + tabControl1.SelectedTab.Text + "��;��ǰ���ڣ�" + labelZDRQ.Text;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {

            string strT = "";
            strT = "���������ѯ��" + tabControl1.SelectedTab.Text + "��;��ǰ���ڣ�" + labelZDRQ.Text;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
            }
        }

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            iSupplyCompany = 0;
            textBoxDWMC.Text = "";
            textBoxDWBH.Text = "";
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;
            if (!dSet.Tables.Contains("��Ʒ��" + c1.ToString()))
                return;
            toolStripStatusLabelC.Text = "����" + dSet.Tables["��Ʒ��" + c1.ToString()].Rows.Count.ToString() + "����¼ ���ϼ�" + cTemp[tabControl1.SelectedIndex].ToString("f2") + "Ԫ";
        }

    }
}