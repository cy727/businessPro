using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormPFXSRBB : Form
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

        public FormPFXSRBB()
        {
            InitializeComponent();
        }

        private void FormPFXSRBB_Load(object sender, EventArgs e)
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

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            string strA = "", strB = "", strC = "", strD = "", strE="";

            strA = "SELECT ������Ʒ�Ƶ���.����, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS ���۽��, SUM(������Ʒ�Ƶ���ϸ��.���ɱ��� * ������Ʒ�Ƶ���ϸ��.����) AS ��������ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID  WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive=1)";
            strA += " GROUP BY ������Ʒ�Ƶ���.����";

            strB = "SELECT �����˳����ܱ�.����, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �˻ؽ��, SUM(�����˳���ϸ��.���ɱ��� * �����˳���ϸ��.����) AS �˻سɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID  WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˳����ܱ�.BeActive=1)";
            strB += " GROUP BY �����˳����ܱ�.����";

            strC = "SELECT �����˲���ۻ��ܱ�.����, SUM(�����˲������ϸ��.���) AS �����˲���۽�� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID  WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1)";
            strC += " GROUP BY �����˲���ۻ��ܱ�.����";


            strD = "SELECT SUM(�Ѹ�����) AS ������, ���� FROM �տ������ͼ  WHERE (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            strD += " GROUP BY ����";

            strE = "SELECT DISTINCT A.���� FROM ((SELECT ���� FROM �տ������ͼ WHERE (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ) UNION (SELECT ���� FROM ������Ʒ�Ƶ��� WHERE (���� >= CONVERT (DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))) UNION (SELECT ���� FROM �����˳����ܱ� WHERE (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))) UNION (SELECT ���� FROM �����˲���ۻ��ܱ� WHERE (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)))) A ";
            
            sqlConn.Open();
            sqlComm.CommandText = "SELECT CONVERT(varchar(100), ���ڱ�.����, 23) AS ����, �����.���۽��, �˻ر�.�˻ؽ��, �����˲���۱�.�����˲���۽��,0.0 AS ������, �����.������, 0.0 AS ����Ӧ�տ�, 0.0 AS ë��, 0.0 AS [ë����(%)], 0.0 AS ����ɱ�, �����.��������ɱ�, �˻ر�.�˻سɱ� FROM (" + strE + ") ���ڱ� LEFT OUTER JOIN (" + strA + ") ����� ON ���ڱ�.���� = �����.���� LEFT OUTER JOIN (" + strB + ") �˻ر� ON ���ڱ�.���� = �˻ر�.���� LEFT OUTER JOIN (" + strC + ") �����˲���۱� ON ���ڱ�.���� = �����˲���۱�.���� LEFT OUTER JOIN (" + strD + ") ����� ON ���ڱ�.���� = �����.���� ORDER BY ���ڱ�.���� DESC";


            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            sqlConn.Close();

            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];

            //dataGridViewDJMX.Columns[10].Visible = false;
            //dataGridViewDJMX.Columns[11].Visible = false;

            dataGridViewDJMX.Columns[1].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";

            dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            int i,j;
            for (i = 0; i < dataGridViewDJMX.ColumnCount; i++)
            {
                dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            
            decimal[] oTemp = new decimal[11];
            for (i = 0; i < oTemp.Length; i++)
                oTemp[i] = 0;

            for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                for (j = 0; j < oTemp.Length; j++)
                {
                    oTemp[j] += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[j+1].Value.ToString());
                }
            }
            if (oTemp[3] == 0)
                oTemp[7] = 0;
            else
                oTemp[7] = oTemp[6] / oTemp[3] * 100;
            object[] oT = new object[12];
            for (j = 0; j < oTemp.Length; j++)
            {
                oT[j + 1] = oTemp[j];
            }
            dSet.Tables["��Ʒ��"].Rows.Add(oT);

            dataGridViewDJMX.Rows[dataGridViewDJMX.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Gray;


            toolStripStatusLabelMXJLS.Text = dSet.Tables["��Ʒ��"].Rows.Count.ToString();
        }

        private void adjustDataView()
        {
            int i, j;
            TimeSpan ts;
            decimal[] dSUM = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0 };
            decimal dt = 0;

            //��Ʒ��

            for (i = 0; i < dSUM.Length; i++)
            {
                dSUM[i] = 0;
            }
            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {

                for (j = 1; j <= 11; j++)
                {
                    if (dSet.Tables["��Ʒ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["��Ʒ��"].Rows[i][j] = 0;
                }
                
                dSet.Tables["��Ʒ��"].Rows[i][4] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][1].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][2].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][3].ToString());//����

                dSet.Tables["��Ʒ��"].Rows[i][6] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][4].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][5].ToString());//����Ӧ��

                dSet.Tables["��Ʒ��"].Rows[i][9] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][10].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][11].ToString());//����ɱ�

                dSet.Tables["��Ʒ��"].Rows[i][7] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][4].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][9].ToString());

                if (decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][4].ToString()) == 0 || decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][7].ToString()) <= 0)
                    dSet.Tables["��Ʒ��"].Rows[i][8] = 0;
                else
                    dSet.Tables["��Ʒ��"].Rows[i][8]=decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][7].ToString())/decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][4].ToString())*100;

                for (j = 1; j <= 11; j++)
                {
                    dSUM[j - 1] += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][j].ToString());
                }


            }
            /*
            DataRow drT3 = dSet.Tables["��Ʒ��"].NewRow();
            drT3[0] = "�ϼ�";
            for (j = 1; j <= 11; j++)
            {
                drT3[j] = dSUM[j - 1];
            }

            drT3[4] = decimal.Parse(drT3[1].ToString()) - decimal.Parse(drT3[2].ToString()) - decimal.Parse(drT3[3].ToString());//����

           drT3[6] = decimal.Parse(drT3[4].ToString()) - decimal.Parse(drT3[5].ToString());//����Ӧ��
           drT3[9] = decimal.Parse(drT3[10].ToString()) + decimal.Parse(drT3[11].ToString());//����ɱ�
           drT3[7] = decimal.Parse(drT3[4].ToString()) - decimal.Parse(drT3[9].ToString());
           if (decimal.Parse(drT3[4].ToString()) == 0 || decimal.Parse(drT3[7].ToString()) <= 0)
               drT3[8] = 0;
           else
               drT3[8] = decimal.Parse(drT3[7].ToString()) / decimal.Parse(drT3[4].ToString()) * 100;

            dSet.Tables["��Ʒ��"].Rows.Add(drT3);
             * */

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "���������ձ���;��ǰ���ڣ�" + labelZDRQ.Text ;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "���������ձ���;��ǰ���ڣ�" + labelZDRQ.Text ;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }



    }
}