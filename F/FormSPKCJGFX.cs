using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPKCJGFX : Form
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

        private int intDays = 0;
        private int intMonths = 0;


        private string sDT = "";
        private ClassGetInformation cGetInformation;

        private int intCommID = 0;
        decimal dSumJE = 0;


        public FormSPKCJGFX()
        {
            InitializeComponent();
        }

        private void FormSPKCJGFX_Load(object sender, EventArgs e)
        {
            int i;

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
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-1-1");

            }
            sqldr.Close();
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i;
            //�õ�����ʱ��
            string sDTE = Convert.ToDateTime(sDT).Year.ToString() + "-" + Convert.ToDateTime(sDT).Month.ToString() + "-1";

            TimeSpan dtTemp = Convert.ToDateTime(sDTE) - dateTimePickerS.Value;
            intDays = dtTemp.Days;
            
            intMonths = (Convert.ToDateTime(sDTE).Year - dateTimePickerS.Value.Year) * 12 + (Convert.ToDateTime(sDTE).Month - dateTimePickerS.Value.Month); //�õ�����

            //intDays--;
            if (intMonths <= 0)
            {
                MessageBox.Show("�������ʼʱ�䵽һ��������");
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT SUM(�����) AS �ܼ� FROM ��Ʒ�� WHERE (beactive = 1)";
            sqldr = sqlComm.ExecuteReader();

            sqldr.Read();
            dSumJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());//�����ϼ�
            sqldr.Close();


            if (checkBoxALLSP.Checked || intCommID == 0)
                sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.�������, ��Ʒ��.�����, 0.0 AS ��������, ��Ʒ��.����� AS [ռ������(%)], 0.0 AS �վ���������, ������.���� FROM ��Ʒ�� LEFT OUTER JOIN (SELECT SUM(������Ʒ�Ƶ���ϸ��.����) AS ����, ������Ʒ�Ƶ���ϸ��.��ƷID FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + sDTE + " 00:00:00', 102)) GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID) ������ ON ��Ʒ��.ID = ������.��ƷID WHERE (��Ʒ��.beactive = 1)";
            else
                sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.�������, ��Ʒ��.�����, 0.0 AS ��������, ��Ʒ��.����� AS [ռ������(%)], 0.0 AS �վ���������, ������.���� FROM ��Ʒ�� INNER JOIN (SELECT SUM(������Ʒ�Ƶ���ϸ��.����) AS ����, ������Ʒ�Ƶ���ϸ��.��ƷID FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + sDTE + " 00:00:00', 102)) GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID) ������ ON ��Ʒ��.ID = ������.��ƷID WHERE (��Ʒ��.beactive = 1) AND (��Ʒ��.ID=" + intCommID.ToString() + ") ";


            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            sqlConn.Close();

            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];

            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString();

            dataGridViewDJMX.Columns[7].Visible=false;
            dataGridViewDJMX.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f1";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f3";

            
            
        }

        private void adjustDataView()
        {
            int i;
            decimal dTemp;

            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��"].Rows[i][2].ToString() == "0") //����
                    dSet.Tables["��Ʒ��"].Rows[i][3] = 0;

                if (dSet.Tables["��Ʒ��"].Rows[i][7].ToString() == "") //����
                    dSet.Tables["��Ʒ��"].Rows[i][7]=0;

                if(dSet.Tables["��Ʒ��"].Rows[i][7].ToString()!="0")
                    dSet.Tables["��Ʒ��"].Rows[i][4]=(Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][2].ToString())*Convert.ToDecimal(intMonths)/Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][7].ToString()));
                else
                    dSet.Tables["��Ʒ��"].Rows[i][4]=0;

                if(dSumJE!=0)
                    dSet.Tables["��Ʒ��"].Rows[i][5]=Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][3].ToString())/dSumJE*100;
                else
                    dSet.Tables["��Ʒ��"].Rows[i][5]=0;


                if (intDays != 0)
                    //dSet.Tables["��Ʒ��"].Rows[i][6]=(Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][7])/Convert.ToDecimal(intDays)); ����һ��22��
                    dSet.Tables["��Ʒ��"].Rows[i][6] = (Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][7]) / Convert.ToDecimal(intDays))*30/22;
                else
                    dSet.Tables["��Ʒ��"].Rows[i][6] = 0;



            }
        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            bool bMX = true;
            int i, j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[10];
            decimal[] dSum1 = new decimal[10];

            for(t=0;t<dSum1.Length;t++)
                dSum1[t] = 0; 

            if (Convert.ToDateTime(sDT).AddDays(-1) < dateTimePickerS.Value)
            {
                MessageBox.Show("��ʼʱ�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("�Ƿ������ϸ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }
            TimeSpan dtTemp = Convert.ToDateTime(sDT) - dateTimePickerS.Value;
            intDays = dtTemp.Days;//�õ�����

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.�������, ��Ʒ��.�����,��Ʒ��.��ת��� AS ��������, ��Ʒ��.��ת��� AS ռ������, ��Ʒ��.��ת��� AS ռѹ�ʽ�, ��Ʒ��.��ת��� AS ƽ�����, ��Ʒ��.��ת��� AS �վ���������, ��Ʒ��.��ת��� AS �վ���������, ��Ʒ��.��ת��� AS ��ת����, ��Ʒ��.��ת��� AS ����ת����, ��Ʒ��.������, �����.��������, �����.��������, �����.Ӧ�����, �����.Ӧ�ս��, �ܿ��.������� AS Expr1, �ܿ��.����� AS Expr2, �����.�ܽ������, �����.���� FROM ��Ʒ�� LEFT OUTER JOIN (SELECT SUM(��������) AS ��������, SUM(��������) AS ��������, SUM(Ӧ�����) AS Ӧ�����, SUM(Ӧ�ս��) AS Ӧ�ս��, ��ƷID, SUM(�ܽ������) AS �ܽ������, COUNT(*) AS ���� FROM ��Ʒ��ʷ�˱� WHERE (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) GROUP BY ��ƷID) ����� ON ��Ʒ��.ID = �����.��ƷID CROSS JOIN (SELECT SUM(�������) AS �������, SUM(�����) AS ����� FROM ��Ʒ��) �ܿ�� WHERE (��Ʒ��.beactive = 1)";


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
            dTable.Columns.Add("�����", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("�������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("��������", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("ռ������", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("ռѹ�ʽ�", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("ƽ�����", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("�վ���������", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("�վ���������", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("��ת����", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("����ת����", System.Type.GetType("System.Int32"));

            DataRow[] dtC = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[12];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 2; t < oTemp.Length;t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[12];
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
                            dSum[t] += Convert.ToDecimal(dtC2[k][t+2].ToString()); 


                        if (bMX)
                        {
                            object[] oTemp2 = new object[12];
                            for (t = 0; t < oTemp2.Length; t++)
                                oTemp2[t] = dtC2[k][t];
                            oTemp2[1] = "��������" + dtC2[k][1];

                            dTable.Rows.Add(oTemp2);
                        }
                    }

                    for (t = 2; t < dSum.Length; t++)
                        dTable.Rows[iRow1][t] = dSum[t-2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }

            object[] oTemp3 = new object[12];
            oTemp3[0] = "�ϼ�";
            oTemp3[1] = "";
            for (t = 2; t < oTemp3.Length; t++)
                oTemp3[t] = dSum1[t-2];
            dTable.Rows.Add(oTemp3);


            dataGridViewDJMX.DataSource = dTable;
 

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��Ʒ���ṹ����;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��Ʒ���ṹ����;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
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
    }
}