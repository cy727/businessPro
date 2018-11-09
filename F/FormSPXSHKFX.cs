using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPXSHKFX : Form
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

        private int intClassID = 0;
        private ClassGetInformation cGetInformation;

        public FormSPXSHKFX()
        {
            InitializeComponent();
        }

        private void FormSPXSHKFX_Load(object sender, EventArgs e)
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
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;
        }

        private void textBoxSPLB_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getClassInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                intClassID = cGetInformation.iClassNumber;
                textBoxSPLB.Text = cGetInformation.strClassName;
                checkBoxAll.Checked = false;

            }
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAll.Checked)
            {
                intClassID = 0;
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.�������, ��Ʒ��.�������*��Ʒ��.���ɱ��� AS �����, �����.���۽��, �����.�ؿ���, �����.Ӧ����� FROM ��Ʒ�� LEFT OUTER JOIN (SELECT ��ƷID, SUM(ʵ�ƽ��) AS ���۽��, SUM(�Ѹ�����) AS �ؿ���, SUM(δ������) AS Ӧ����� FROM �տ���ϸ��ͼ WHERE (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) GROUP BY ��ƷID) ����� ON ��Ʒ��.ID = �����.��ƷID WHERE (��Ʒ��.beactive = 1)";

            if (intClassID != 0)
            {
                sqlComm.CommandText += " AND (��Ʒ��.������ = " + intClassID.ToString() + ")";
            }

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            sqlConn.Close();
            
            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];
            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f2";

            toolStripStatusLabelMXJLS.Text = (dataGridViewDJMX.RowCount-1).ToString();
        }

        private void adjustDataView()
        {
            int i;
            decimal dT1 = 0, dT2 = 0, dT3 = 0, dT4 = 0, dT5 = 0;

            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��"].Rows[i][4].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][4] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][5].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][5] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][6].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][6] = 0;

                dT1 += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][2].ToString());
                dT2 += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][3].ToString());
                dT3 += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][4].ToString());
                dT4 += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][5].ToString());
                dT5 += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][6].ToString());
            }

            object[] oTemp = new object[7];
            oTemp[0] = "�ϼ�";
            oTemp[1] = "";
            oTemp[2] = dT1;
            oTemp[3] = dT2;
            oTemp[4] = dT3;
            oTemp[5] = dT4;
            oTemp[6] = dT5;
            dSet.Tables["��Ʒ��"].Rows.Add(oTemp);
        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            bool bMX = true;
            int i, j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[5];
            decimal[] dSum1 = new decimal[5];

            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;

            if (MessageBox.Show("�Ƿ������ϸ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }

            sqlConn.Open();
            //sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.�������, ��Ʒ��.�����,��Ʒ��.��ת��� AS ��������, ��Ʒ��.��ת��� AS ռ������, ��Ʒ��.��ת��� AS ռѹ�ʽ�, ��Ʒ��.��ת��� AS ƽ�����, ��Ʒ��.��ת��� AS �վ���������, ��Ʒ��.��ת��� AS �վ���������, ��Ʒ��.��ת��� AS ��ת����, ��Ʒ��.��ת��� AS ����ת����, ��Ʒ��.������, �����.��������, �����.��������, �����.Ӧ�����, �����.Ӧ�ս��, �ܿ��.������� AS Expr1, �ܿ��.����� AS Expr2, �����.�ܽ������, �����.���� FROM ��Ʒ�� LEFT OUTER JOIN (SELECT SUM(��������) AS ��������, SUM(��������) AS ��������, SUM(Ӧ�����) AS Ӧ�����, SUM(Ӧ�ս��) AS Ӧ�ս��, ��ƷID, SUM(�ܽ������) AS �ܽ������, COUNT(*) AS ���� FROM ��Ʒ��ʷ�˱� WHERE (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) GROUP BY ��ƷID) ����� ON ��Ʒ��.ID = �����.��ƷID CROSS JOIN (SELECT SUM(�������) AS �������, SUM(�����) AS ����� FROM ��Ʒ��) �ܿ�� WHERE (��Ʒ��.beactive = 1)";
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.�������, ��Ʒ��.�����, �����.���۽��, �����.�ؿ���, �����.Ӧ�����,  ��Ʒ��.������ FROM ��Ʒ�� LEFT OUTER JOIN (SELECT ��ƷID, SUM(ʵ�ƽ��) AS ���۽��, SUM(�Ѹ�����) AS �ؿ���, SUM(δ������) AS Ӧ����� FROM �տ���ϸ��ͼ WHERE (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) GROUP BY ��ƷID) ����� ON ��Ʒ��.ID = �����.��ƷID WHERE (��Ʒ��.beactive = 1)";



            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            sqlConn.Close();

            adjustDataView();

            sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� ORDER BY �ϼ�����";
            if (dSet.Tables.Contains("��Ʒ�����")) dSet.Tables.Remove("��Ʒ�����");
            sqlDA.Fill(dSet, "��Ʒ�����");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("��Ʒ���", System.Type.GetType("System.String"));
            dTable.Columns.Add("��Ʒ����", System.Type.GetType("System.String"));
            dTable.Columns.Add("�������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("�����", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("���۽��", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("�ؿ���", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("Ӧ�����", System.Type.GetType("System.Decimal"));

            DataRow[] dtC = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[7];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 2; t < oTemp.Length; t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[7];
                    oTemp1[0] = dtC1[j][1];
                    oTemp1[1] = "����" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    //oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;
                    for (t = 2; t < oTemp1.Length; t++)
                        oTemp1[t] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["��Ʒ��"].Select("������ = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 2].ToString());


                        if (bMX)
                        {
                            object[] oTemp2 = new object[7];
                            for (t = 0; t < oTemp2.Length; t++)
                                oTemp2[t] = dtC2[k][t];
                            oTemp2[1] = "��������" + dtC2[k][1];

                            dTable.Rows.Add(oTemp2);
                        }
                    }

                    for (t = 2; t < dSum.Length+2; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length+2; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }

            object[] oTemp3 = new object[7];
            oTemp3[0] = "�ϼ�";
            oTemp3[1] = "";
            for (t = 2; t < oTemp3.Length; t++)
                oTemp3[t] = dSum1[t - 2];
            dTable.Rows.Add(oTemp3);


            dataGridViewDJMX.DataSource = dTable;
            toolStripStatusLabelMXJLS.Text = dSet.Tables["��Ʒ��"].Rows.Count.ToString();
 
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��Ʒ���ۻؿ����;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��Ʒ���ۻؿ����;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void textBoxSPLB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getClassInformation(10, textBoxSPLB.Text) == 0) //ʧ��
                {
                    textBoxSPLB.Text = "";
                    intClassID = 0;
                    checkBoxAll.Checked = true;

                }
                else
                {
                    intClassID = cGetInformation.iClassNumber;
                    textBoxSPLB.Text = cGetInformation.strClassName;
                    checkBoxAll.Checked = false;
                }
            }
        }


    }
}