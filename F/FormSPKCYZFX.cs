using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPKCYZFX : Form
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
        private int intCommID = 0;

        private string strDT0="",strDT1="",strDT2="",strDT3="";
        private ClassGetInformation cGetInformation;

        public FormSPKCYZFX()
        {
            InitializeComponent();
        }

        private void FormSPKCYZFX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            string strTTemp = "";

            //�õ���ʼʱ��
            sqlConn.Open();
            //sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
            sqlComm.CommandText = "SELECT GETDATE()";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
                //dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");
                dateTimePickerS.Value = DateTime.Parse(sqldr.GetValue(0).ToString()).AddDays(-30);

            }
            sqldr.Close();
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            strDT0 = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT0).ToString("yyyy��M��dd��");

            strDT0 = dateTimePickerE.Value.ToShortDateString();
            strTTemp = dateTimePickerS.Value.ToShortDateString();

            strDT1 = Convert.ToDateTime(strDT0).AddDays(-5).ToShortDateString();
            if (Convert.ToDateTime(strDT1) < Convert.ToDateTime(strTTemp))
                strDT1 = strTTemp;
            strDT2 = Convert.ToDateTime(strDT0).AddDays(-10).ToShortDateString();
            if (Convert.ToDateTime(strDT2) < Convert.ToDateTime(strTTemp))
                strDT2 = strTTemp;
            strDT3 = Convert.ToDateTime(strDT0).AddDays(-30).ToShortDateString();
            if (Convert.ToDateTime(strDT3) < Convert.ToDateTime(strTTemp))
                strDT3 = strTTemp;
            labelCZY.Text = strUserName;


        }

        private void checkBoxALL_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxALL.Checked)
            {
                intClassID = 0;
            }
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
                checkBoxALL.Checked = false;

            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i;
            string strTTemp = "";

            cGetInformation.getSystemDateTime();
            strDT0 = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT0).ToString("yyyy��M��dd��");

            strDT0 = dateTimePickerE.Value.ToShortDateString();
            strTTemp = dateTimePickerS.Value.ToShortDateString();

            strDT1 = Convert.ToDateTime(strDT0).AddDays(-5).ToShortDateString();
            if (Convert.ToDateTime(strDT1) < Convert.ToDateTime(strTTemp))
                strDT1 = strTTemp;
            strDT2 = Convert.ToDateTime(strDT0).AddDays(-10).ToShortDateString();
            if (Convert.ToDateTime(strDT2) < Convert.ToDateTime(strTTemp))
                strDT2 = strTTemp;
            strDT3 = Convert.ToDateTime(strDT0).AddDays(-30).ToShortDateString();
            if (Convert.ToDateTime(strDT3) < Convert.ToDateTime(strTTemp))
                strDT3 = strTTemp;


            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                cGetInformation.getUnderClassInformation(intClassID);
            }

            string strA = "SELECT SUM(������Ʒ�Ƶ���ϸ��.����) AS ����, ������Ʒ�Ƶ���ϸ��.��ƷID, MIN(��Ʒ��.������) AS ������ FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '"+strDT0+" 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '"+strDT1+" 23:59:59', 102)) AND (��Ʒ��.beactive = 1) ";
            strA +=" GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

            string strB = "SELECT SUM(������Ʒ�Ƶ���ϸ��.����) AS ����, ������Ʒ�Ƶ���ϸ��.��ƷID, MIN(��Ʒ��.������) AS ������ FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + strDT1 + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + strDT2 + " 23:59:59', 102)) AND (��Ʒ��.beactive = 1) ";
            strB += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

            string strC = "SELECT SUM(������Ʒ�Ƶ���ϸ��.����) AS ����, ������Ʒ�Ƶ���ϸ��.��ƷID, MIN(��Ʒ��.������) AS ������ FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + strDT2 + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + strDT3 + " 23:59:59', 102)) AND (��Ʒ��.beactive = 1) ";
            strC += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

            string strD = "SELECT SUM(������Ʒ�Ƶ���ϸ��.����) AS ����, ������Ʒ�Ƶ���ϸ��.��ƷID, MIN(��Ʒ��.������) AS ������ FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + strDT3 + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (��Ʒ��.beactive = 1) ";
            strD += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";



            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.�����, ��Ʒ��.�������, [5��].����  AS [0-5��], [10��].���� AS [6-10��], [30��].���� AS [11-30��], [30������].���� AS [30������], ��Ʒ��.������ FROM ��Ʒ�� LEFT OUTER JOIN (" + strD + ") [30������] ON  ��Ʒ��.ID = [30������].��ƷID LEFT OUTER JOIN (" + strC + ") [30��] ON ��Ʒ��.ID = [30��].��ƷID LEFT OUTER JOIN (" + strB + ") [10��] ON ��Ʒ��.ID = [10��].��ƷID LEFT OUTER JOIN (" + strA + ") [5��] ON ��Ʒ��.ID = [5��].��ƷID WHERE (��Ʒ��.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                sqlComm.CommandText += " AND (��Ʒ��.ID = " + intCommID.ToString() + ")";
            }
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                sqlComm.CommandText += " AND ((��Ʒ��.������ = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlConn.Open();
            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            sqlConn.Close();

            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];

            
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[9].Visible = false;
            

            adjust();
        }

        private void adjust()
        {
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
        }

        private void adjustDataView()
        {
            int i; 
            decimal dTemp0, dTemp1, dTemp2, dTemp3;

            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��"].Rows[i][4].ToString() == "")
                {
                    dSet.Tables["��Ʒ��"].Rows[i][4] = 0;
                    dSet.Tables["��Ʒ��"].Rows[i][5] = 0;
                    dSet.Tables["��Ʒ��"].Rows[i][6] = 0;
                    dSet.Tables["��Ʒ��"].Rows[i][7] = 0;
                    dSet.Tables["��Ʒ��"].Rows[i][8] = 0;
                    continue;
                }

                dTemp1 = Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][4]);
                if (dSet.Tables["��Ʒ��"].Rows[i][5].ToString() == "")
                {
                    dSet.Tables["��Ʒ��"].Rows[i][5] = 0;
                }
                dTemp0 = dTemp1 - Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][5]);
                if (dTemp0 < 0)
                {
                    dSet.Tables["��Ʒ��"].Rows[i][5] = dTemp1;
                    dSet.Tables["��Ʒ��"].Rows[i][6] = 0;
                    dSet.Tables["��Ʒ��"].Rows[i][7] = 0;
                    dSet.Tables["��Ʒ��"].Rows[i][8] = 0;
                    continue;
                }

                dTemp1 = dTemp0;
                if (dSet.Tables["��Ʒ��"].Rows[i][6].ToString() == "")
                {
                    dSet.Tables["��Ʒ��"].Rows[i][6] = 0;
                }
                dTemp0 = dTemp1 - Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][6]);
                if (dTemp0 < 0)
                {
                    dSet.Tables["��Ʒ��"].Rows[i][6] = dTemp1;
                    dSet.Tables["��Ʒ��"].Rows[i][7] = 0;
                    dSet.Tables["��Ʒ��"].Rows[i][8] = 0;
                    continue;
                }

                dTemp1 = dTemp0;
                if (dSet.Tables["��Ʒ��"].Rows[i][7].ToString() == "")
                {
                    dSet.Tables["��Ʒ��"].Rows[i][7] = 0;
                }
                dTemp0 = dTemp1 - Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][7]);
                if (dTemp0 < 0)
                {
                    dSet.Tables["��Ʒ��"].Rows[i][7] = dTemp1;
                    dSet.Tables["��Ʒ��"].Rows[i][8] = 0;
                    continue;
                }

                dSet.Tables["��Ʒ��"].Rows[i][8] = dTemp0;

            }
        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            bool bMX = true;
            int i,j,k;
            int iRow0,iRow1;
            decimal []dSum= new decimal[6];
            decimal[] dSum1 = new decimal[6];

            string strTTemp = "";

            cGetInformation.getSystemDateTime();
            strDT0 = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT0).ToString("yyyy��M��dd��");

            strDT0 = dateTimePickerE.Value.ToShortDateString();
            strTTemp = dateTimePickerS.Value.ToShortDateString();

            strDT1 = Convert.ToDateTime(strDT0).AddDays(-5).ToShortDateString();
            if (Convert.ToDateTime(strDT1) < Convert.ToDateTime(strTTemp))
                strDT1 = strTTemp;
            strDT2 = Convert.ToDateTime(strDT0).AddDays(-10).ToShortDateString();
            if (Convert.ToDateTime(strDT2) < Convert.ToDateTime(strTTemp))
                strDT2 = strTTemp;
            strDT3 = Convert.ToDateTime(strDT0).AddDays(-30).ToShortDateString();
            if (Convert.ToDateTime(strDT3) < Convert.ToDateTime(strTTemp))
                strDT3 = strTTemp;

            dSum1[0] = 0; dSum1[2] = 0; dSum1[3] = 0; dSum1[4] = 0; dSum1[5] = 0;

            if (MessageBox.Show("�Ƿ������ϸ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }


            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                cGetInformation.getUnderClassInformation(intClassID);
            }

            string strA = "SELECT SUM(������Ʒ�Ƶ���ϸ��.����) AS ����, ������Ʒ�Ƶ���ϸ��.��ƷID, MIN(��Ʒ��.������) AS ������ FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + strDT0 + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + strDT1 + " 23:59:59', 102)) AND (��Ʒ��.beactive = 1) ";
            strA += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

            string strB = "SELECT SUM(������Ʒ�Ƶ���ϸ��.����) AS ����, ������Ʒ�Ƶ���ϸ��.��ƷID, MIN(��Ʒ��.������) AS ������ FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + strDT1 + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + strDT2 + " 23:59:59', 102)) AND (��Ʒ��.beactive = 1) ";
            strB += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

            string strC = "SELECT SUM(������Ʒ�Ƶ���ϸ��.����) AS ����, ������Ʒ�Ƶ���ϸ��.��ƷID, MIN(��Ʒ��.������) AS ������ FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + strDT2 + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + strDT3 + " 23:59:59', 102)) AND (��Ʒ��.beactive = 1) ";
            strC += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

            string strD = "SELECT SUM(������Ʒ�Ƶ���ϸ��.����) AS ����, ������Ʒ�Ƶ���ϸ��.��ƷID, MIN(��Ʒ��.������) AS ������ FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + strDT3 + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (��Ʒ��.beactive = 1) ";
            strD += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";



            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.�����, ��Ʒ��.�������, [5��].����  AS [0-5��], [10��].���� AS [6-10��], [30��].���� AS [11-30��], [30������].���� AS [30������], ��Ʒ��.������ FROM ��Ʒ�� LEFT OUTER JOIN (" + strD + ") [30������] ON  ��Ʒ��.ID = [30������].��ƷID LEFT OUTER JOIN (" + strC + ") [30��] ON ��Ʒ��.ID = [30��].��ƷID LEFT OUTER JOIN (" + strB + ") [10��] ON ��Ʒ��.ID = [10��].��ƷID LEFT OUTER JOIN (" + strA + ") [5��] ON ��Ʒ��.ID = [5��].��ƷID WHERE (��Ʒ��.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                sqlComm.CommandText += " AND (��Ʒ��.ID = " + intCommID.ToString() + ")";
            }
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                sqlComm.CommandText += " AND ((��Ʒ��.������ = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlConn.Open();
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
            dTable.Columns.Add("0-5��", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("6-10��", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("11-30��", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("30������", System.Type.GetType("System.Int32"));
            //dTable.Columns.Add("������", System.Type.GetType("System.Int32"));

            DataRow []dtC=dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[8];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];
                //oTemp[8] = dtC[i][0];
                oTemp[2] = 0; oTemp[3] = 0; oTemp[4] = 0; oTemp[5] = 0; oTemp[6] = 0; oTemp[7] = 0;


                dTable.Rows.Add(oTemp);
                iRow0=dTable.Rows.Count-1;
                
                DataRow []dtC1=dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0,"+dtC[i][0]+"'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[8];
                    oTemp1[0] = dtC1[j][1];
                    oTemp1[1] = "����" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["��Ʒ��"].Select("������ = " + dtC1[j][0]);

                    dSum[0] = 0; dSum[1] = 0; dSum[2] = 0; dSum[3] = 0; dSum[4] = 0; dSum[5] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {
                        dSum[0] += Convert.ToDecimal(dtC2[k][2].ToString());
                        dSum[1] += Convert.ToDecimal(dtC2[k][3].ToString());
                        dSum[2] += Convert.ToDecimal(dtC2[k][4].ToString());
                        dSum[3] += Convert.ToDecimal(dtC2[k][5].ToString());
                        dSum[4] += Convert.ToDecimal(dtC2[k][6].ToString());
                        dSum[5] += Convert.ToDecimal(dtC2[k][7].ToString());

                        if (bMX)
                        {
                            object[] oTemp2 = new object[8];
                            oTemp2[0] = dtC2[k][0];
                            oTemp2[1] = "��������" + dtC2[k][1];
                            oTemp2[2] = dtC2[k][2];
                            oTemp2[3] = dtC2[k][3];
                            oTemp2[4] = dtC2[k][4];
                            oTemp2[5] = dtC2[k][5];
                            oTemp2[6] = dtC2[k][6];
                            oTemp2[7] = dtC2[k][7];
                            //oTemp2[8] = dtC2[k][8];

                            dTable.Rows.Add(oTemp2);
                        }
                    }
                    dTable.Rows[iRow1][2] = dSum[0];
                    dTable.Rows[iRow1][3] = dSum[1];
                    dTable.Rows[iRow1][4] = dSum[2];
                    dTable.Rows[iRow1][5] = dSum[3];
                    dTable.Rows[iRow1][6] = dSum[4];
                    dTable.Rows[iRow1][7] = dSum[5];

                    dSum1[0] += dSum[0]; dSum1[1] += dSum[1]; dSum1[2] += dSum[2]; dSum1[3] += dSum[3]; dSum1[4] += dSum[4];
                    dSum1[5] += dSum[5];



                    dTable.Rows[iRow0][2] = Convert.ToDecimal(dTable.Rows[iRow0][2]) + Convert.ToDecimal(dTable.Rows[iRow1][2]);
                    dTable.Rows[iRow0][3] = Convert.ToDecimal(dTable.Rows[iRow0][3]) + Convert.ToDecimal(dTable.Rows[iRow1][3]);
                    dTable.Rows[iRow0][4] = Convert.ToDecimal(dTable.Rows[iRow0][4]) + Convert.ToDecimal(dTable.Rows[iRow1][4]);
                    dTable.Rows[iRow0][5] = Convert.ToDecimal(dTable.Rows[iRow0][5]) + Convert.ToDecimal(dTable.Rows[iRow1][5]);
                    dTable.Rows[iRow0][6] = Convert.ToDecimal(dTable.Rows[iRow0][6]) + Convert.ToDecimal(dTable.Rows[iRow1][6]);
                    dTable.Rows[iRow0][7] = Convert.ToDecimal(dTable.Rows[iRow0][7]) + Convert.ToDecimal(dTable.Rows[iRow1][7]);
                }


            }

            object[] oTemp3 = new object[8];
            oTemp3[0] = "�ϼ�";
            oTemp3[1] = "";
            oTemp3[2] = dSum1[0];
            oTemp3[3] = dSum1[1];
            oTemp3[4] = dSum1[2];
            oTemp3[5] = dSum1[3];
            oTemp3[6] = dSum1[4];
            oTemp3[7] = dSum1[5];
            dTable.Rows.Add(oTemp3);


            dataGridViewDJMX.DataSource = dTable;
            adjust();

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��Ʒ���ѹռ����;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��Ʒ���ѹռ����;��ǰ���ڣ�" + labelZDRQ.Text;
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
                    checkBoxALL.Checked = true;

                }
                else
                {
                    intClassID = cGetInformation.iClassNumber;
                    textBoxSPLB.Text = cGetInformation.strClassName;
                    checkBoxALL.Checked = false;
                }
            }
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