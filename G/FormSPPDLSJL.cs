using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPPDLSJL : Form
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

        public FormSPPDLSJL()
        {
            InitializeComponent();
        }

        private void FormSPPDLSJL_Load(object sender, EventArgs e)
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

            sqlComm.CommandText = "SELECT ����̵���ܱ�.���ݱ��, ����̵���ܱ�.����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ����̵���ϸ��.�������, ����̵���ϸ��.�����, ����̵���ϸ��.ʵ������, ����̵���ϸ��.��������, ����̵���ϸ��.������, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ���� FROM ����̵���ܱ� INNER JOIN ����̵���ϸ�� ON ����̵���ܱ�.ID = ����̵���ϸ��.����ID INNER JOIN ��Ʒ�� ON ����̵���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ����̵���ϸ��.�ⷿID = �ⷿ��.ID WHERE (����̵���ܱ�.�̵�ʱ�� IS NOT NULL) AND (����̵���ܱ�.���� >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (����̵���ܱ�.���� <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 23:59:59', 102))";

            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");

            sqlComm.CommandText = "SELECT ����̵���ܱ�.���ݱ��, ����̵���ܱ�.����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ����̵���ϸ��.�������, ����̵���ϸ��.�����, ����̵���ϸ��.ʵ������, ����̵���ϸ��.��������, ����̵���ϸ��.������, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ���� FROM ����̵���ܱ� INNER JOIN ����̵���ϸ�� ON ����̵���ܱ�.ID = ����̵���ϸ��.����ID INNER JOIN ��Ʒ�� ON ����̵���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ����̵���ϸ��.�ⷿID = �ⷿ��.ID WHERE (����̵���ܱ�.�̵�ʱ�� IS NULL) AND (����̵���ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (����̵���ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");

            sqlConn.Close();
            adjustDataView1();
            adjustDataView2();
            dataGridView1.DataSource = dSet.Tables["��Ʒ��1"];
            dataGridView2.DataSource = dSet.Tables["��Ʒ��2"];
            dataGridView1.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[7].DefaultCellStyle.Format = "f0";

        }

        private void adjustDataView1()
        {
            int i,j;
            decimal dTemp0 = 0, dTemp1 = 0;
            decimal dTemp10 = 0, dTemp11 = 0;

            for (i = 0; i < dSet.Tables["��Ʒ��1"].Rows.Count; i++)
            {
                for (j = 4; j <= 9; j++)
                {
                    if (dSet.Tables["��Ʒ��1"].Rows[i][j].ToString() == "")
                        dSet.Tables["��Ʒ��1"].Rows[i][j] = 0;
                }
                dTemp0 += Convert.ToDecimal(dSet.Tables["��Ʒ��1"].Rows[i][8]);
                dTemp1 += Convert.ToDecimal(dSet.Tables["��Ʒ��1"].Rows[i][5]);

                dTemp10 += Convert.ToDecimal(dSet.Tables["��Ʒ��1"].Rows[i][7]);
                dTemp11 += Convert.ToDecimal(dSet.Tables["��Ʒ��1"].Rows[i][4]);
            }
            labelPSJEHJ.Text = dTemp0.ToString("f2");
            labelPDJEHJ.Text = dTemp1.ToString("f2");

            labelPSSLHJ.Text = dTemp10.ToString("f0");
            labelPDSLHJ.Text = dTemp11.ToString("f0");
        }

        private void adjustDataView2()
        {
            int i, j;

            for (i = 0; i < dSet.Tables["��Ʒ��2"].Rows.Count; i++)
            {
                for (j = 4; j <= 9; j++)
                {
                    if (dSet.Tables["��Ʒ��2"].Rows[i][j].ToString() == "")
                        dSet.Tables["��Ʒ��2"].Rows[i][j] = 0;
                }
            }
        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            dataViewHZ1();
            dataViewHZ2();
        }

        private void dataViewHZ1()
        {
            bool bMX = true;
            int i, j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[5];
            decimal[] dSum1 = new decimal[5];

            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ����̵���ܱ�.���ݱ��, ����̵���ܱ�.����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ����̵���ϸ��.�������, ����̵���ϸ��.�����, ����̵���ϸ��.ʵ������, ����̵���ϸ��.��������, ����̵���ϸ��.������, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����,��Ʒ��.������ FROM ����̵���ܱ� INNER JOIN ����̵���ϸ�� ON ����̵���ܱ�.ID = ����̵���ϸ��.����ID INNER JOIN �ⷿ�� ON ����̵���ܱ�.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON ����̵���ϸ��.��ƷID = ��Ʒ��.ID WHERE (����̵���ܱ�.�̵�ʱ�� IS NOT NULL) AND (����̵���ܱ�.���� >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (����̵���ܱ�.���� <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102))";

            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");
            sqlConn.Close();

            adjustDataView1();

            sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� ORDER BY �ϼ�����";
            if (dSet.Tables.Contains("��Ʒ�����")) dSet.Tables.Remove("��Ʒ�����");
            sqlDA.Fill(dSet, "��Ʒ�����");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("���ݱ��", System.Type.GetType("System.String"));
            dTable.Columns.Add("����", System.Type.GetType("System.String"));
            dTable.Columns.Add("��Ʒ���", System.Type.GetType("System.String"));
            dTable.Columns.Add("��Ʒ����", System.Type.GetType("System.String"));
            dTable.Columns.Add("�������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("�����", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("ʵ������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("��������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("�ⷿ���", System.Type.GetType("System.String"));
            dTable.Columns.Add("�ⷿ����", System.Type.GetType("System.String"));


            DataRow[] dtC = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[11];
                oTemp[2] = dtC[i][1];
                oTemp[3] = dtC[i][2];
                oTemp[0] = "";
                oTemp[1] = "";

                for (t = 4; t < oTemp.Length-2; t++)
                    oTemp[t] = 0;
                oTemp[9] = "";
                oTemp[10] = "";


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[11];
                    oTemp1[2] = dtC1[j][1];
                    oTemp1[3] = "����" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    //oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;
                    for (t = 4; t < oTemp1.Length-2; t++)
                        oTemp1[t] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["��Ʒ��1"].Select("������ = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 4].ToString());

                            object[] oTemp2 = new object[11];
                            for (t = 0; t < oTemp2.Length; t++)
                                oTemp2[t] = dtC2[k][t];
                            oTemp2[3] = "��������" + dtC2[k][1];

                            dTable.Rows.Add(oTemp2);
                    }

                    for (t = 4; t < dSum.Length+4; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 4];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 4; t < dSum.Length+4; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }

            dataGridView1.DataSource = dTable;

        }

        private void dataViewHZ2()
        {
            bool bMX = true;
            int i, j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[5];
            decimal[] dSum1 = new decimal[5];

            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ����̵���ܱ�.���ݱ��, ����̵���ܱ�.����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ����̵���ϸ��.�������, ����̵���ϸ��.�����, ����̵���ϸ��.ʵ������, ����̵���ϸ��.��������, ����̵���ϸ��.������, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����,��Ʒ��.������ FROM ����̵���ܱ� INNER JOIN ����̵���ϸ�� ON ����̵���ܱ�.ID = ����̵���ϸ��.����ID INNER JOIN �ⷿ�� ON ����̵���ܱ�.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON ����̵���ϸ��.��ƷID = ��Ʒ��.ID WHERE (����̵���ܱ�.�̵�ʱ�� IS NULL) AND (����̵���ܱ�.���� >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (����̵���ܱ�.���� <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102))";

            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");
            sqlConn.Close();

            adjustDataView2();

            sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� ORDER BY �ϼ�����";
            if (dSet.Tables.Contains("��Ʒ�����")) dSet.Tables.Remove("��Ʒ�����");
            sqlDA.Fill(dSet, "��Ʒ�����");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("���ݱ��", System.Type.GetType("System.String"));
            dTable.Columns.Add("����", System.Type.GetType("System.String"));
            dTable.Columns.Add("��Ʒ���", System.Type.GetType("System.String"));
            dTable.Columns.Add("��Ʒ����", System.Type.GetType("System.String"));
            dTable.Columns.Add("�������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("�����", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("ʵ������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("��������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("�ⷿ���", System.Type.GetType("System.String"));
            dTable.Columns.Add("�ⷿ����", System.Type.GetType("System.String"));


            DataRow[] dtC = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[11];
                oTemp[2] = dtC[i][1];
                oTemp[3] = dtC[i][2];
                oTemp[0] = "";
                oTemp[1] = "";

                for (t = 4; t < oTemp.Length - 2; t++)
                    oTemp[t] = 0;
                oTemp[9] = "";
                oTemp[10] = "";


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[11];
                    oTemp1[2] = dtC1[j][1];
                    oTemp1[3] = "����" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    //oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;
                    for (t = 4; t < oTemp1.Length - 2; t++)
                        oTemp1[t] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["��Ʒ��2"].Select("������ = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 4].ToString());

                        object[] oTemp2 = new object[11];
                        for (t = 0; t < oTemp2.Length; t++)
                            oTemp2[t] = dtC2[k][t];
                        oTemp2[3] = "��������" + dtC2[k][1];

                        dTable.Rows.Add(oTemp2);
                    }

                    for (t = 4; t < dSum.Length + 4; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 4];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 4; t < dSum.Length + 4; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }

            dataGridView2.DataSource = dTable;

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "��Ʒ�̵���ʷ��¼����ʷ�̵���ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "��Ʒ�̵���ʷ��¼��δ����̵���ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
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
                    strT = "��Ʒ�̵���ʷ��¼����ʷ�̵���ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "��Ʒ�̵���ʷ��¼��δ����̵���ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;

            }
        }



    }
}