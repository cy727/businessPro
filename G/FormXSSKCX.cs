using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXSSKCX : Form
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

        public int LIMITACCESS1 = 15;

        public FormXSSKCX()
        {
            InitializeComponent();
        }

        private void FormXSSKCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            //�õ���ʼʱ��
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��˾����, ����Ŀ��1, ����Ŀ��2, ����Ŀ��3, ����Ŀ��4, ����ԱȨ��, �ܾ���Ȩ��, ְԱȨ��, ����Ȩ��, ҵ��ԱȨ�� FROM ϵͳ������";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {

                try
                {
                    LIMITACCESS1 = int.Parse(sqldr.GetValue(8).ToString());
                }
                catch
                {
                    LIMITACCESS1 = 15;
                }
            }
            sqldr.Close();
            //��ʼ��Ա���б�
            sqlComm.CommandText = "SELECT ID, ְԱ���, ְԱ���� FROM ְԱ�� WHERE (beactive = 1)";

            if (dSet.Tables.Contains("ְԱ��")) dSet.Tables.Remove("ְԱ��");
            sqlDA.Fill(dSet, "ְԱ��");

            object[] OTemp = new object[3];
            OTemp[0] = 0;
            OTemp[1] = "ȫ��";
            OTemp[2] = "ȫ��";
            dSet.Tables["ְԱ��"].Rows.Add(OTemp);

            comboBoxYWY.DataSource = dSet.Tables["ְԱ��"];
            comboBoxYWY.DisplayMember = "ְԱ����";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.SelectedIndex = comboBoxYWY.Items.Count - 1;

            if (intUserLimit <= LIMITACCESS1)
            {
                comboBoxYWY.SelectedValue = intUserID;
                comboBoxYWY.Enabled = false;
            }

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
            if (cGetInformation.getCompanyInformation(2, "") == 0)
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
                if (cGetInformation.getCompanyInformation(20, textBoxDWBH.Text.Trim()) == 0)
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
                if (cGetInformation.getCompanyInformation(22, textBoxDWMC.Text.Trim()) == 0)
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

            /*
            sqlComm.CommandText = "SELECT �����տ���ܱ�.���ݱ��, �����տ���ܱ�.����, �˲���.�˲����, �˲���.�˲�����, �����տ���ϸ��.��Ӧ����, �����տ���ϸ��.����, �����տ���ϸ��.������, �����տ���ϸ��.֧Ʊ��, �����տ���ϸ��.��ע, �����տ���ϸ��.ժҪ, ְԱ��.ְԱ���� AS ҵ��Ա FROM �����տ���ܱ� INNER JOIN �����տ���ϸ�� ON �����տ���ܱ�.ID = �����տ���ϸ��.����ID INNER JOIN �˲��� ON �����տ���ϸ��.�˲�ID = �˲���.ID INNER JOIN ְԱ�� ON �����տ���ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ��λ�� ON �����տ���ܱ�.��λID = ��λ��.ID WHERE (�����տ���ܱ�.BeActive = 1) AND (�����տ���ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����տ���ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");
            */


            //δ�������
            //sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���,��λ��.��λ����, ������Ʒ�Ƶ���.��˰�ϼ�, ������Ʒ�Ƶ���.δ������, ������Ʒ�Ƶ���.��ע FROM ��λ�� INNER JOIN ������Ʒ�Ƶ��� ON ��λ��.ID = ������Ʒ�Ƶ���.��λID WHERE (������Ʒ�Ƶ���.δ������ > 0) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1)";
            //sqlComm.CommandText = "SELECT �տ������ͼ.ID, �տ������ͼ.���ݱ��, �տ������ͼ.����, ��λ��.��λ���,��λ��.��λ����, �տ������ͼ.��˰�ϼ�, �տ������ͼ.δ������, �տ������ͼ.��ע FROM ��λ�� INNER JOIN �տ������ͼ ON ��λ��.ID = �տ������ͼ.��λID WHERE (�տ������ͼ.δ������ > 0) AND (�տ������ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�տ������ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�տ������ͼ.BeActive = 1)";
            sqlComm.CommandText = "SELECT �տ������ͼ.���ݱ��, �տ������ͼ.����, ��λ��.��λ���, ��λ��.��λ����,ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, �տ������ͼ.��˰�ϼ�, �տ������ͼ.��ע,�տ������ͼ.δ������,�տ������ͼ.�Ѹ����� FROM �տ������ͼ INNER JOIN ��λ�� ON �տ������ͼ.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �տ������ͼ.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON �տ������ͼ.����ԱID = ����Ա.ID WHERE (�տ������ͼ.BeActive = 1) AND (�տ������ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�տ������ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�տ������ͼ.δ������ <> 0)";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (intUserLimit <= LIMITACCESS1)
            {
                sqlComm.CommandText += " AND ((�տ������ͼ.ҵ��ԱID = " + intUserID.ToString() + ") OR (�տ������ͼ.����ԱID = " + intUserID.ToString() + "))";
            }
            else
            {
                if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                    sqlComm.CommandText += " AND �տ������ͼ.ҵ��ԱID = " + comboBoxYWY.SelectedValue.ToString();
            }

            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");


            //�������
            //sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��������, ������Ʒ�Ƶ���.����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ������Ʒ�Ƶ���.��˰�ϼ�, ������Ʒ�Ƶ���.��ע FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ������Ʒ�Ƶ���.����ԱID = ����Ա.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.�Ѹ����� > 0)";
            sqlComm.CommandText = "SELECT �տ������ͼ.���ݱ��, �տ������ͼ.����, ��λ��.��λ���, ��λ��.��λ����,ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, �տ������ͼ.��˰�ϼ�, �տ������ͼ.��ע,�տ������ͼ.δ������,�տ������ͼ.�Ѹ����� FROM �տ������ͼ INNER JOIN ��λ�� ON �տ������ͼ.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �տ������ͼ.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON �տ������ͼ.����ԱID = ����Ա.ID WHERE (�տ������ͼ.BeActive = 1) AND (�տ������ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�տ������ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�տ������ͼ.�Ѹ����� <> 0)";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            //if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
            //    sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
            if (intUserLimit <= LIMITACCESS1)
            {
                sqlComm.CommandText += " AND ((�տ������ͼ.ҵ��ԱID = " + intUserID.ToString() + ") OR (�տ������ͼ.����ԱID = " + intUserID.ToString() + "))";
            }
            else
            {
                if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                    sqlComm.CommandText += " AND �տ������ͼ.ҵ��ԱID = " + comboBoxYWY.SelectedValue.ToString();
            }


            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");


            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["��Ʒ��1"];
            //dataGridView1.Columns[0].Visible = false;
            dataGridView2.DataSource = dSet.Tables["��Ʒ��2"];
            //dataGridView2.Columns[0].Visible = false;
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
                        c = 6;
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
            strT = "����ҵ���ѯ��" + tabControl1.SelectedTab.Text + "��;��ǰ���ڣ�" + labelZDRQ.Text;
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
            strT = "����ҵ���ѯ��" + tabControl1.SelectedTab.Text + "��;��ǰ���ڣ�" + labelZDRQ.Text;
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;
            if (!dSet.Tables.Contains("��Ʒ��" + c1.ToString()))
                return;
            toolStripStatusLabelC.Text = "����" + dSet.Tables["��Ʒ��" + c1.ToString()].Rows.Count.ToString() + "����¼ ���ϼ�" + cTemp[tabControl1.SelectedIndex].ToString("f2") + "Ԫ";
        }

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            iSupplyCompany = 0;
            textBoxDWMC.Text = "";
            textBoxDWBH.Text = "";
        }
    }
}