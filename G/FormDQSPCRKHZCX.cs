using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormDQSPCRKHZCX : Form
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

        private int iJZID = 0;
        private int intCommID = 0;

        private string SDTS0 = "", SDTS1 = "";

        private ClassGetInformation cGetInformation;
        
        public FormDQSPCRKHZCX()
        {
            InitializeComponent();
        }

        private void FormDQSPCRKHZCX_Load(object sender, EventArgs e)
        {
            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            intCommID = 0;
            textBoxSPMC.Text = "";
            textBoxSPBH.Text = "";

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            //�õ��ϴν�ת
            //�õ���ʼʱ��
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                SDTS0 = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT ����ʱ��,ID FROM ��ת���ܱ� ORDER BY ����ʱ�� DESC";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                iJZID = Convert.ToInt32(sqldr.GetValue(1).ToString());
                SDTS1 = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
            }
            if (iJZID == 0)
                SDTS1 = SDTS0;

            sqlConn.Close();

        }

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0)
            {
                intCommID = 0;
                textBoxSPMC.Text = "";
                textBoxSPBH.Text = "";
                //return;
            }
            else
            {
                intCommID = 0;
                textBoxSPMC.Text = "";
                textBoxSPBH.Text = "";

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
                    intCommID = 0;
                    textBoxSPMC.Text = "";
                    textBoxSPBH.Text = "";

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
                    intCommID = 0;
                    textBoxSPMC.Text = "";
                    textBoxSPBH.Text = "";

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
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��ת.��ת����, ��ת.��ת���, ��Ʒ��.������� AS �������, ��Ʒ��.����� AS �����, �����.�������, �����.�����, �����.��������, �����.������ FROM ��Ʒ�� LEFT OUTER JOIN (SELECT SUM(�������) AS �������, SUM(�����) AS �����, SUM(��������) AS ��������, SUM(������) AS ������, ��ƷID FROM ��Ʒ��ʷ�˱� WHERE (���� > CONVERT(DATETIME, '" + SDTS1 + " 00:00:00', 102)) GROUP BY ��ƷID) ����� ON ��Ʒ��.ID = �����.��ƷID LEFT OUTER JOIN (SELECT ��ת����, ��ת���, ��ƷID FROM ��ת��������ܱ� WHERE (ID = " + iJZID .ToString()+ ")) ��ת ON ��Ʒ��.ID = ��ת.��ƷID WHERE (��Ʒ��.beactive = 1)";

            if (intCommID != 0)
                sqlComm.CommandText += " AND (��Ʒ��.ID = " + intCommID.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");

            sqlConn.Close();
            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];

            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
        }

        private void adjustDataView()
        {

            decimal dTemp0 = 0, dTemp1 = 0, dTemp2 = 0, dTemp3 = 0;
            
            for (int i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                for (int j = 2; j < 10; j++)
                {
                    if (dSet.Tables["��Ʒ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["��Ʒ��"].Rows[i][j] = 0;
                }

                dTemp0 += Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][7]);
                dTemp1 += Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][9]);
                dTemp2 += Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][5]);
                dTemp3 += Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][3]);

            }
            labelRKJEHJ.Text = dTemp0.ToString("f2");
            labelCKJEHJ.Text = dTemp1.ToString("f0");
            labelJCJEHJ.Text = dTemp2.ToString("f0");
            labelJZJEHJ.Text = dTemp3.ToString("f0");
            toolStripStatusLabelMXJLS.Text=dSet.Tables["��Ʒ��"].Rows.Count.ToString();


        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            int i;
            bool bMX = true;

            if (MessageBox.Show("�Ƿ������ϸ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��ת.��ת����, ��ת.��ת���, ��Ʒ��.������� AS �������, ��Ʒ��.����� AS �����, �����.�������, �����.�����, �����.��������, �����.������, ��Ʒ��.������ FROM ��Ʒ�� LEFT OUTER JOIN (SELECT SUM(�������) AS �������, SUM(�����) AS �����, SUM(��������) AS ��������, SUM(������) AS ������, ��ƷID FROM ��Ʒ��ʷ�˱� WHERE (���� > CONVERT(DATETIME, '" + SDTS1 + " 00:00:00', 102)) GROUP BY ��ƷID) ����� ON ��Ʒ��.ID = �����.��ƷID LEFT OUTER JOIN (SELECT ��ת����, ��ת���, ��ƷID FROM ��ת��������ܱ� WHERE (ID = " + iJZID.ToString() + ")) ��ת ON ��Ʒ��.ID = ��ת.��ƷID WHERE (��Ʒ��.beactive = 1)";


            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");

            sqlConn.Close();
            adjustDataView();

            int j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[8];
            decimal[] dSum1 = new decimal[8];

            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;


            sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� ORDER BY �ϼ�����";
            if (dSet.Tables.Contains("��Ʒ�����")) dSet.Tables.Remove("��Ʒ�����");
            sqlDA.Fill(dSet, "��Ʒ�����");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("��Ʒ���", System.Type.GetType("System.String"));
            dTable.Columns.Add("��Ʒ����", System.Type.GetType("System.String"));
            dTable.Columns.Add("��ת����", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("��ת���", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("�������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("�����", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("�������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("�����", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("��������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("������", System.Type.GetType("System.Decimal"));



            DataRow[] dtC = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[10];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 2; t < oTemp.Length; t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[10];
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
                        {
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 2].ToString());
                        }


                        if (bMX)
                        {
                            object[] oTemp2 = new object[10];
                            for (t = 0; t < oTemp2.Length; t++)
                                oTemp2[t] = dtC2[k][t];
                            oTemp2[1] = "��������" + dtC2[k][1];

                            dTable.Rows.Add(oTemp2);
                        }
                    }

                    for (t = 2; t < dSum.Length + 2; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length + 2; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }



            dataGridViewDJMX.DataSource = dTable;
 
        }

       private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "������Ʒ�������ܱ�;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "������Ʒ�������ܱ�;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

    }
}