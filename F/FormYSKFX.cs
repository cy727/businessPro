using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormYSKFX : Form
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

        private int intCommID = 0;
        private int iCompanyID = 0;

        private decimal[] cTemp = new decimal[4] { 0, 0, 0, 0 };

        public FormYSKFX()
        {
            InitializeComponent();
        }

        private void FormYSKFX_Load(object sender, EventArgs e)
        {
            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            //�õ���ʼʱ��
            sqlConn.Open();
            //sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
            sqlComm.CommandText = "SELECT GETDATE()";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            comboBoxJE.SelectedIndex = 0;
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            string strA = "";
            string strB = "";
            sqlConn.Open();
            sqlComm.CommandText = "SELECT �տ���ϸ��ͼ.����, �տ���ϸ��ͼ.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �տ���ϸ��ͼ.δ������ AS Ӧ�ս�� FROM �տ���ϸ��ͼ INNER JOIN ��Ʒ�� ON �տ���ϸ��ͼ.��ƷID = ��Ʒ��.ID INNER JOIN ְԱ�� ON �տ���ϸ��ͼ.ҵ��ԱID = ְԱ��.ID INNER JOIN ��λ�� ON �տ���ϸ��ͼ.��λID = ��λ��.ID WHERE (�տ���ϸ��ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�տ���ϸ��ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (δ������ <> 0) ";

            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                sqlComm.CommandText += " AND (�տ���ϸ��ͼ.��λID = " + iCompanyID.ToString() + ") ";
            }

            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                sqlComm.CommandText += " AND (�տ���ϸ��ͼ.��ƷID = " + intCommID.ToString() + ") ";
            }

            switch (comboBoxJE.SelectedIndex)
            {
                case 1:
                    sqlComm.CommandText += " AND (�տ���ϸ��ͼ.δ������ > 0) ";
                    break;
                case 2:
                    sqlComm.CommandText += " AND (�տ���ϸ��ͼ.δ������ < 0) ";
                    break;

            }

            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");

            strB = "SELECT SUM(δ������) AS Ӧ����� FROM �տ���ϸ��ͼ WHERE (δ������ <> 0)  AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (��λID = " + iCompanyID.ToString() + ") ";
            }
            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                strB += " AND (��ƷID = " + intCommID.ToString() + ") ";
            }
            switch (comboBoxJE.SelectedIndex)
            {
                case 1:
                    strB += " AND (δ������ > 0) ";
                    break;
                case 2:
                    strB += " AND (δ������ < 0) ";
                    break;

            }
            strA = "SELECT ��λID, SUM(δ������) AS Ӧ����� FROM �տ���ϸ��ͼ WHERE (δ������ <> 0)  AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (��λID = " + iCompanyID.ToString() + ") ";
            }
            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                strA += " AND (��ƷID = " + intCommID.ToString() + ") ";
            }
            switch (comboBoxJE.SelectedIndex)
            {
                case 1:
                    strA += " AND (δ������ > 0) ";
                    break;
                case 2:
                    strA += " AND (δ������ < 0) ";
                    break;
            }

            strA += " GROUP BY ��λID";

            sqlComm.CommandText = "SELECT ��λ��.��λ���, ��λ��.��λ����, Ӧ����.Ӧ����� AS Ӧ�ս��, Ӧ����.Ӧ����� AS [��ռ����(%)], ��Ӧ����.Ӧ����� AS ��Ӧ���� FROM ��λ�� INNER JOIN ("+strA+") Ӧ���� ON ��λ��.ID = Ӧ����.��λID CROSS JOIN ("+strB+") ��Ӧ����";

            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");

            strA = "SELECT ҵ��ԱID, SUM(δ������) AS Ӧ����� FROM �տ���ϸ��ͼ WHERE (δ������ <> 0)  AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (��λID = " + iCompanyID.ToString() + ") ";
            }
            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                strA += " AND (��ƷID = " + intCommID.ToString() + ") ";
            }
            switch (comboBoxJE.SelectedIndex)
            {
                case 1:
                    strA += " AND (δ������ > 0) ";
                    break;
                case 2:
                    strA += " AND (δ������ < 0) ";
                    break;
            }

            strA += " GROUP BY ҵ��ԱID";

            sqlComm.CommandText = "SELECT ְԱ��.ְԱ���, ְԱ��.ְԱ����, Ӧ����.Ӧ����� AS Ӧ�ս��, Ӧ����.Ӧ����� AS [��ռ����(%)], ��Ӧ�����.Ӧ����� FROM ְԱ�� INNER JOIN (" + strA + ") Ӧ���� ON ְԱ��.ID = Ӧ����.ҵ��ԱID CROSS JOIN (" + strB + ") ��Ӧ����� WHERE (ְԱ��.beactive = 1)";

            if (dSet.Tables.Contains("��Ʒ��3")) dSet.Tables.Remove("��Ʒ��3");
            sqlDA.Fill(dSet, "��Ʒ��3");

            strA = "SELECT ��ƷID, SUM(δ������) AS Ӧ����� FROM �տ���ϸ��ͼ WHERE (δ������ <> 0)  AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (��λID = " + iCompanyID.ToString() + ") ";
            }
            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                strA += " AND (��ƷID = " + intCommID.ToString() + ") ";
            }
            switch (comboBoxJE.SelectedIndex)
            {
                case 1:
                    strA += " AND (δ������ > 0) ";
                    break;
                case 2:
                    strA += " AND (δ������ < 0) ";
                    break;
            }

            strA += " GROUP BY ��ƷID";

            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, Ӧ�����.Ӧ����� AS Ӧ�ս��, Ӧ�����.Ӧ����� AS [��ռ����(%)], ��Ӧ�����.Ӧ����� FROM ��Ʒ�� INNER JOIN (" + strA + ") Ӧ����� ON ��Ʒ��.ID = Ӧ�����.��ƷID CROSS JOIN (" + strB + ") ��Ӧ����� WHERE (��Ʒ��.beactive = 1)";

            if (dSet.Tables.Contains("��Ʒ��4")) dSet.Tables.Remove("��Ʒ��4");
            sqlDA.Fill(dSet, "��Ʒ��4");


            sqlConn.Close();

            adjustDataView();
            dataGridViewYSKMX.DataSource = dSet.Tables["��Ʒ��1"];
            dataGridViewDW.DataSource = dSet.Tables["��Ʒ��2"];
            dataGridViewDW.Columns[4].Visible = false;
            dataGridViewYWY.DataSource = dSet.Tables["��Ʒ��3"];
            dataGridViewYWY.Columns[4].Visible = false;
            dataGridViewSP.DataSource = dSet.Tables["��Ʒ��4"];
            dataGridViewSP.Columns[4].Visible = false;

            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);
        }

        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= 4; i++)
            {
                cTemp[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 7;
                        break;
                    case 2:
                        c = 2;
                        break;
                    case 3:
                        c = 2;
                        break;
                    case 4:
                        c = 2;
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


        private void adjustDataView()
        {
            int i;


            for (i = 0; i < dSet.Tables["��Ʒ��1"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��1"].Rows[i][7].ToString() == "")
                    dSet.Tables["��Ʒ��1"].Rows[i][7] = 0;
            }

            for (i = 0; i < dSet.Tables["��Ʒ��2"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��2"].Rows[i][4].ToString() == "")
                    dSet.Tables["��Ʒ��2"].Rows[i][4] = 0;
                if (dSet.Tables["��Ʒ��2"].Rows[i][2].ToString() == "")
                    dSet.Tables["��Ʒ��2"].Rows[i][2] = 0;
                if (Convert.ToDecimal(dSet.Tables["��Ʒ��2"].Rows[i][4].ToString()) == 0)
                    dSet.Tables["��Ʒ��2"].Rows[i][3] = 0;
                else
                    dSet.Tables["��Ʒ��2"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["��Ʒ��2"].Rows[i][2].ToString()) / Convert.ToDecimal(dSet.Tables["��Ʒ��2"].Rows[i][4].ToString()) * 100;

                dSet.Tables["��Ʒ��2"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["��Ʒ��2"].Rows[i][3]).ToString("f2");
            }

            for (i = 0; i < dSet.Tables["��Ʒ��3"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��3"].Rows[i][4].ToString() == "")
                    dSet.Tables["��Ʒ��3"].Rows[i][4] = 0;
                if (dSet.Tables["��Ʒ��3"].Rows[i][2].ToString() == "")
                    dSet.Tables["��Ʒ��3"].Rows[i][2] = 0;
                if (Convert.ToDecimal(dSet.Tables["��Ʒ��3"].Rows[i][4].ToString()) == 0)
                    dSet.Tables["��Ʒ��3"].Rows[i][3] = 0;
                else
                    dSet.Tables["��Ʒ��3"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["��Ʒ��3"].Rows[i][2].ToString()) / Convert.ToDecimal(dSet.Tables["��Ʒ��3"].Rows[i][4].ToString()) * 100;

                dSet.Tables["��Ʒ��3"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["��Ʒ��3"].Rows[i][3]).ToString("f2");
            }

            for (i = 0; i < dSet.Tables["��Ʒ��4"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��4"].Rows[i][4].ToString() == "")
                    dSet.Tables["��Ʒ��4"].Rows[i][4] = 0;
                if (dSet.Tables["��Ʒ��4"].Rows[i][2].ToString() == "")
                    dSet.Tables["��Ʒ��4"].Rows[i][2] = 0;
                if (Convert.ToDecimal(dSet.Tables["��Ʒ��4"].Rows[i][4].ToString()) == 0)
                    dSet.Tables["��Ʒ��4"].Rows[i][3] = 0;
                else
                    dSet.Tables["��Ʒ��4"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["��Ʒ��4"].Rows[i][2].ToString()) / Convert.ToDecimal(dSet.Tables["��Ʒ��4"].Rows[i][4].ToString())*100;

                dSet.Tables["��Ʒ��4"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["��Ʒ��4"].Rows[i][3]).ToString("f2");

            }


        }

        private void adjustDataView1()
        {
            int i;

            for (i = 0; i < dSet.Tables["��Ʒ��4"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��4"].Rows[i][4].ToString() == "")
                    dSet.Tables["��Ʒ��4"].Rows[i][4] = 0;
                if (dSet.Tables["��Ʒ��4"].Rows[i][2].ToString() == "")
                    dSet.Tables["��Ʒ��4"].Rows[i][2] = 0;
                if (Convert.ToDecimal(dSet.Tables["��Ʒ��4"].Rows[i][4].ToString()) == 0)
                    dSet.Tables["��Ʒ��4"].Rows[i][3] = 0;
                else
                    dSet.Tables["��Ʒ��"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["��Ʒ��4"].Rows[i][2].ToString()) / Convert.ToDecimal(dSet.Tables["��Ʒ��4"].Rows[i][4].ToString()) * 100;

                dSet.Tables["��Ʒ��4"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["��Ʒ��4"].Rows[i][3]).ToString("f2");

            }

        }


        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            bool bMX = true;
            int i, j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[2];
            decimal[] dSum1 = new decimal[2];

            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;

            if (MessageBox.Show("�Ƿ������ϸ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, Ӧ�����.Ӧ�����, Ӧ�����.Ӧ����� AS [��ռ����(%)], ��Ӧ�����.��Ӧ�����, ��Ʒ��.������ FROM ��Ʒ�� LEFT OUTER JOIN (SELECT SUM(������Ʒ�Ƶ���ϸ��.δ������) AS Ӧ�����, ������Ʒ�Ƶ���ϸ��.��ƷID FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT (DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID) Ӧ����� ON ��Ʒ��.ID = Ӧ�����.��ƷID CROSS JOIN (SELECT SUM(������Ʒ�Ƶ���ϸ��.δ������) AS ��Ӧ����� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive = 1)) ��Ӧ����� WHERE (��Ʒ��.beactive = 1)";
            

            if (dSet.Tables.Contains("��Ʒ��4")) dSet.Tables.Remove("��Ʒ��4");
            sqlDA.Fill(dSet, "��Ʒ��4");
            sqlConn.Close();

            adjustDataView1();

            sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� ORDER BY �ϼ�����";
            if (dSet.Tables.Contains("��Ʒ�����")) dSet.Tables.Remove("��Ʒ�����");
            sqlDA.Fill(dSet, "��Ʒ�����");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("��Ʒ���", System.Type.GetType("System.String"));
            dTable.Columns.Add("��Ʒ����", System.Type.GetType("System.String"));
            dTable.Columns.Add("Ӧ�ս��", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("[��ռ����(%)]", System.Type.GetType("System.Decimal"));


            DataRow[] dtC = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[4];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 2; t < oTemp.Length; t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[4];
                    oTemp1[0] = dtC1[j][1];
                    oTemp1[1] = "����" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    //oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;
                    for (t = 2; t < oTemp1.Length; t++)
                        oTemp1[t] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["��Ʒ��4"].Select("������ = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 2].ToString());


                        if (bMX)
                        {
                            object[] oTemp2 = new object[4];
                            for (t = 0; t < oTemp2.Length; t++)
                                oTemp2[t] = dtC2[k][t];
                            oTemp2[1] = "��������" + dtC2[k][1];

                            dTable.Rows.Add(oTemp2);
                        }
                    }

                    for (t = 2; t < dSum.Length; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }

            object[] oTemp3 = new object[4];
            oTemp3[0] = "�ϼ�";
            oTemp3[1] = "";
            for (t = 2; t < oTemp3.Length; t++)
                oTemp3[t] = dSum1[t - 2];
            dTable.Rows.Add(oTemp3);


            dataGridViewSP.DataSource = dTable;

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "Ӧ�տ������Ӧ�տ���ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewYSKMX, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "Ӧ�տ��������λӦ�տ���ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDW, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "Ӧ�տ������ҵ��ԱӦ�տ���ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewYWY, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "Ӧ�տ��������ƷӦ�տ���ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewSP, strT, true, intUserLimit);
                    break;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "Ӧ�տ������Ӧ�տ���ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewYSKMX, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "Ӧ�տ��������λӦ�տ���ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDW, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "Ӧ�տ������ҵ��ԱӦ�տ���ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewYWY, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "Ӧ�տ��������ƷӦ�տ���ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewSP, strT, false, intUserLimit);
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

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPBH.Text = cGetInformation.strCommCode;
                textBoxSPMC.Text = cGetInformation.strCommName;
            }
        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                }

            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                }

            }
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                iCompanyID = cGetInformation.iCompanyNumber;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1100, textBoxDWBH.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    iCompanyID = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }

            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1300, textBoxDWMC.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    iCompanyID = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }

            }
        }
    }
}