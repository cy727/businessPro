using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJWCX : Form
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

        public int iSupplyCompany = 0;
        public int intCommID = 0;

        private decimal[] cTemp = new decimal[6] { 0, 0, 0, 0, 0, 0 };
        private decimal[] cTemp1 = new decimal[6] { 0, 0, 0, 0, 0, 0 };
        
        public FormJWCX()
        {
            InitializeComponent();
        }

        private void FormJWCX_Load(object sender, EventArgs e)
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
            if (cGetInformation.getCompanyInformation(1000, "") == 0)
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
                if (cGetInformation.getCompanyInformation(1200, textBoxDWBH.Text.Trim()) == 0)
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
                if (cGetInformation.getCompanyInformation(1200, textBoxDWMC.Text.Trim()) == 0)
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

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0)
            {
                //return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPMC.Text = cGetInformation.strCommName;
                textBoxSPBH.Text = cGetInformation.strCommCode;
            }
        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0)
                {
                    //return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                }
            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0)
                {
                    //return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            if(intCommID==0)
                sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ���������ܱ�.��˰�ϼ� AS ���ϼ�, ���������ܱ�.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.������ >= 0) AND (���������ܱ�.BeActive = 1) AND ((���������ܱ�.��ֵ���ID <> -1) OR (���������ܱ�.��ֵ���ID IS NULL))";
            else
                sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ���������ܱ�.��˰�ϼ� AS ���ϼ�, ���������ܱ�.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ���������ϸ�� ON ���������ܱ�.ID = ���������ϸ��.��ID AND (���������ϸ��.��ƷID = " + intCommID + ") AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.������ >= 0) AND (���������ܱ�.BeActive = 1) AND ((���������ܱ�.��ֵ���ID <> -1) OR (���������ܱ�.��ֵ���ID IS NULL))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " ORDER BY ���������ܱ�.���� DESC";

            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");

            if (intCommID == 0)
                sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ���������ܱ�.��˰�ϼ� AS ���ϼ�, ���������ܱ�.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.��ֵ���ID IS NULL) AND (���������ܱ�.������ < 0) AND (���������ܱ�.BeActive = 1)";                else
                sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ���������ܱ�.��˰�ϼ� AS ���ϼ�, ���������ܱ�.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ���������ϸ�� ON ���������ܱ�.ID = ���������ϸ��.��ID AND (���������ϸ��.��ƷID = " + intCommID + ") AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (���������ܱ�.��ֵ���ID IS NULL) AND (���������ܱ�.������ < 0) AND (���������ܱ�.BeActive = 1)";


            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " ORDER BY ���������ܱ�.���� DESC";

            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");

            if (intCommID == 0)
                sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ���������ܱ�.��˰�ϼ� AS ���ϼ�, ���������ܱ�.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.��ֵ���ID IS NULL) AND (���������ܱ�.������ >= 0) AND (���������ܱ�.BeActive = 1)";
            else
                sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ���������ܱ�.��˰�ϼ� AS ���ϼ�, ���������ܱ�.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ���������ϸ�� ON ���������ܱ�.ID = ���������ϸ��.��ID AND (���������ϸ��.��ƷID = " + intCommID + ") AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (���������ܱ�.��ֵ���ID IS NULL) AND (���������ܱ�.������ >= 0) AND (���������ܱ�.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " ORDER BY ���������ܱ�.���� DESC";

            if (dSet.Tables.Contains("��Ʒ��3")) dSet.Tables.Remove("��Ʒ��3");
            sqlDA.Fill(dSet, "��Ʒ��3");

            if (intCommID == 0)
                sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ���������ܱ�.��˰�ϼ� AS ���ϼ�, ���������ܱ�.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.��ֵ���ID =-1) AND (���������ܱ�.BeActive = 1)";
            else
                sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ���������ܱ�.��˰�ϼ� AS ���ϼ�, ���������ܱ�.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ���������ϸ�� ON ���������ܱ�.ID = ���������ϸ��.��ID AND (���������ϸ��.��ƷID = " + intCommID + ") AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (���������ܱ�.��ֵ���ID =-1) AND (���������ܱ�.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " ORDER BY ���������ܱ�.���� DESC";

            if (dSet.Tables.Contains("��Ʒ��4")) dSet.Tables.Remove("��Ʒ��4");
            sqlDA.Fill(dSet, "��Ʒ��4");


            if (intCommID == 0)
                sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ���������ܱ�.��˰�ϼ� AS ���ϼ�, ���������ܱ�.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.������ < 0) AND (���������ܱ�.BeActive = 1) AND ((���������ܱ�.��ֵ���ID <> -1) OR (���������ܱ�.��ֵ���ID IS NULL))";
            else
                sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ���������ܱ�.��˰�ϼ� AS ���ϼ�, ���������ܱ�.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ���������ϸ�� ON ���������ܱ�.ID = ���������ϸ��.��ID AND (���������ϸ��.��ƷID = " + intCommID + ") AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.������ < 0) AND (���������ܱ�.BeActive = 1) AND ((���������ܱ�.��ֵ���ID <> -1) OR (���������ܱ�.��ֵ���ID IS NULL))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " ORDER BY ���������ܱ�.���� DESC";

            if (dSet.Tables.Contains("��Ʒ��5")) dSet.Tables.Remove("��Ʒ��5");
            sqlDA.Fill(dSet, "��Ʒ��5");

            if (intCommID == 0)
                sqlComm.CommandText = "SELECT ��λ��.��λ����, SUM(���������ܱ�.��˰�ϼ�) AS ���ϼ�,SUM(���������ܱ�.������) AS ������ϼ� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID WHERE (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.��ֵ���ID IS NULL) AND (���������ܱ�.BeActive = 1)";
            else
                sqlComm.CommandText = "SELECT ��λ��.��λ����, SUM(���������ϸ��.���) AS ���ϼ�, SUM(���������ϸ��.������) AS ������ϼ� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ���������ϸ�� ON ���������ܱ�.ID = ���������ϸ��.��ID WHERE (���������ܱ�.��ֵ���ID IS NULL) AND (���������ϸ��.��ƷID = " + intCommID + ") AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " GROUP BY ��λ��.��λ���� ORDER BY ���ϼ� DESC";

            if (dSet.Tables.Contains("��Ʒ��6")) dSet.Tables.Remove("��Ʒ��6");
            sqlDA.Fill(dSet, "��Ʒ��6");


            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["��Ʒ��1"];
            dataGridView2.DataSource = dSet.Tables["��Ʒ��2"];
            dataGridView3.DataSource = dSet.Tables["��Ʒ��3"];
            dataGridView4.DataSource = dSet.Tables["��Ʒ��4"];
            dataGridView5.DataSource = dSet.Tables["��Ʒ��5"];
            dataGridView6.DataSource = dSet.Tables["��Ʒ��6"];

            dataGridView1.Columns[0].Visible = false;
            dataGridView2.Columns[0].Visible = false;
            dataGridView3.Columns[0].Visible = false;
            dataGridView4.Columns[0].Visible = false;
            dataGridView5.Columns[0].Visible = false;

            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);
            
        }

        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= 6; i++)
            {
                cTemp[i - 1] = 0;
                cTemp1[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 5;
                        c1 = 6;
                        break;
                    case 2:
                        c = 5;
                        c1 = 6;
                        break;
                    case 3:
                        c = 5;
                        c1 = 6;
                        break;
                    case 4:
                        c = 5;
                        c1 = 6;
                        break;
                    case 5:
                        c = 5;
                        c1 = 6;
                        break;
                    case 6:
                        c = 1;
                        c1 = 2;
                        break;
                    default:
                        c = 0;
                        c1 = 0;
                        break;
                }

                if (c != 0)
                {

                    for (j = 0; j < dSet.Tables["��Ʒ��" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp[i - 1] += decimal.Parse(dSet.Tables["��Ʒ��" + i.ToString()].Rows[j][c].ToString());
                            cTemp1[i - 1] += decimal.Parse(dSet.Tables["��Ʒ��" + i.ToString()].Rows[j][c1].ToString());
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

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "�����ѯ�����������ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "�����ѯ������δ��ֲ�ѯ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "�����ѯ�����δ��ֲ�ѯ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "�����ѯ����ֲ�ѯ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, true, intUserLimit);
                    break;
                case 4:
                    strT = "�����ѯ�����������ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, true, intUserLimit);
                    break;
                case 5:
                    strT = "�����ѯ��δ��ֽ������ⵥλ���ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, true, intUserLimit);
                    break;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "�����ѯ�����������ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "�����ѯ������δ��ֲ�ѯ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "�����ѯ�����δ��ֲ�ѯ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "�����ѯ����ֲ�ѯ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, false, intUserLimit);
                    break;
                case 4:
                    strT = "�����ѯ�����������ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, false, intUserLimit);
                    break;
                case 5:
                    strT = "�����ѯ��δ��ֽ������ⵥλ���ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, false, intUserLimit);
                    break;
            }

        }

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            iSupplyCompany = 0;
            textBoxDWBH.Text = "";
            textBoxDWMC.Text = "";
        }

        private void toolStripButtonASP_Click(object sender, EventArgs e)
        {
            intCommID = 0;
            textBoxSPBH.Text = "";
            textBoxSPMC.Text = "";
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;

            if (!dSet.Tables.Contains("��Ʒ��" + c1.ToString()))
                return;


            //toolStripStatusLabelC.Text = "����" + dSet.Tables["��Ʒ��" + c1.ToString()].Rows.Count.ToString() + "����¼ ���ϼ�" + cTemp[tabControl1.SelectedIndex].ToString("f2") + "Ԫ ������ϼ�" + cTemp1[tabControl1.SelectedIndex].ToString("f2") + "Ԫ";
            toolStripStatusLabelC.Text = "����" + dSet.Tables["��Ʒ��" + c1.ToString()].Rows.Count.ToString() + "����¼ ������ϼ�" + cTemp1[tabControl1.SelectedIndex].ToString("f2") + "Ԫ";

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridView dv = (DataGridView)sender; ;

            if (dv.SelectedRows.Count < 1)
                return;

            if (dv.SelectedRows[0].Cells[0].Value.ToString() == "")
                return;

            // �������Ӵ����һ����ʵ����
            FormKCJWCKDJ childFormKCJWCKDJ = new FormKCJWCKDJ();
            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
            childFormKCJWCKDJ.MdiParent = this.MdiParent; 

            childFormKCJWCKDJ.strConn = strConn;

            childFormKCJWCKDJ.intUserID = intUserID;
            childFormKCJWCKDJ.intUserLimit = intUserLimit;
            childFormKCJWCKDJ.strUserLimit = strUserLimit;
            childFormKCJWCKDJ.strUserName = strUserName;
            childFormKCJWCKDJ.isSaved = true;
            childFormKCJWCKDJ.iDJID = int.Parse(dv.SelectedRows[0].Cells[0].Value.ToString());
            childFormKCJWCKDJ.Show();

        }


    }
}