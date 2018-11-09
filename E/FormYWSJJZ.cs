using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormYWSJJZ : Form
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

        public bool isSaved = false;
        public int iJZID = 0;

        private string sSCJZSJ = "", sBCJZSJ = "";


        public FormYWSJJZ()
        {
            InitializeComponent();
        }

        private void FormYWSJJZ_Load(object sender, EventArgs e)
        {
            int i;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;


            //if (isSaved)
            //{
            //    initDJ();
            //    return;
            //}

            //�õ��ϴν�תʱ��
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ����ʱ��,ID FROM ��ת���ܱ� ORDER BY ����ʱ�� DESC";
            sqldr=sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString(); 
                iJZID = Convert.ToInt32(sqldr.GetValue(1).ToString());
            }
            sqldr.Close();

            if (sSCJZSJ == "") //û�н���
            {
                sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString(); 
                }
                iJZID = 0;
                sqldr.Close();
            }
            labelSCJZRQ.Text = Convert.ToDateTime(sSCJZSJ).ToString("yyyy��M��dd��");

            sqlConn.Close();

            //initHTDefault();
            cGetInformation.getSystemDateTime();
            //sBCJZSJ = cGetInformation.strSYSDATATIME;
            sBCJZSJ = Convert.ToDateTime(cGetInformation.strSYSDATATIME).AddDays(-1).ToShortDateString();
            labelBCJZRQ.Text = Convert.ToDateTime(sBCJZSJ).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            initdataGridViewJXCHZ();
            //initdataGridViewCBLRB();
            initdataGridViewKF();
            initdataGridViewKFHZ();
            dataGridViewKF_Click(null,null);
            initdataGridViewGJWLHZ();

        }

        private void initdataGridViewKF()
        {
            int i, j;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ID, �ⷿ���, �ⷿ���� FROM �ⷿ�� WHERE (BeActive = 1)";

            if (dSet.Tables.Contains("�ⷿ��")) dSet.Tables.Remove("�ⷿ��");
            sqlDA.Fill(dSet, "�ⷿ��");
            dataGridViewKF.DataSource = dSet.Tables["�ⷿ��"];
            sqlConn.Close();


            dataGridViewKF.Columns[0].Visible = false;
            for (i = 1; i < dataGridViewKF.ColumnCount; i++)
            {
                dataGridViewKF.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            
        }
        private void initdataGridViewJXCHZ()
        {
            int i, j;
            try
            {
                string strA = "", strB = "", strC = "", strD = "", strE = "", strF = "", strG = "", strH = "", strI = "", strJ = "", strK = "", strL = "", strM = "";

                strA = "SELECT * FROM ��ת��������ܱ� WHERE (��תID =  " + iJZID.ToString() + ")";

                strB = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, SUM(������Ʒ�Ƶ���ϸ��.����) AS ��������, SUM(������Ʒ�Ƶ���ϸ��.���ɱ��� * ������Ʒ�Ƶ���ϸ��.����)  AS �����ɱ�, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������, SUM(������Ʒ�Ƶ���ϸ��.ë��) AS ����ë�� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID WHERE (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

                strC = "SELECT ��ƷID, SUM(����) AS ��������, SUM(ʵ�ƽ��) AS ������ FROM ������ͼ WHERE (���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��ƷID";

                strD = "SELECT ��ƷID, SUM(����) AS �������, SUM(���) AS ����� FROM �����ͼ WHERE (���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��ƷID";

                strE = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.���) AS ��������� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.����ID = ���������ܱ�.ID WHERE (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102) AND (���������ܱ�.BeActive = 1)) GROUP BY ���������ϸ��.��ƷID";

                strF = "SELECT �����˳���ϸ��.��ƷID, SUM(�����˳���ϸ��.����) AS �����˳�����, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳���� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY �����˳���ϸ��.��ƷID";

                strG = "SELECT �����˳���ϸ��.��ƷID, SUM(�����˳���ϸ��.����) AS �����˳�����, SUM(�����˳���ϸ��.����*�����˳���ϸ��.���ɱ���) AS �����˳��ɱ�, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳���� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY �����˳���ϸ��.��ƷID";


                strH = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.������) AS ��������� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (���������ϸ��.����>0) AND ((���������ܱ�.��ֵ���ID IS NULL) OR (���������ܱ�.��ֵ���ID <> -1)) GROUP BY ���������ϸ��.��ƷID";

                strJ = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.������) AS ��������� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (���������ϸ��.����<0) AND ((���������ܱ�.��ֵ���ID IS NULL) OR (���������ܱ�.��ֵ���ID <> -1)) GROUP BY ���������ϸ��.��ƷID";

                /*

                strH = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.������) AS ��������� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (���������ϸ��.����>0) GROUP BY ���������ϸ��.��ƷID";

                strJ = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.������) AS ��������� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (���������ϸ��.����<0) GROUP BY ���������ϸ��.��ƷID";
                */

                strM = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS ����������, SUM(���������ϸ��.������) AS �����ֽ�� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.��ֵ���ID = -1) GROUP BY ���������ϸ��.��ƷID";

                strI = "SELECT  ��汨����ϸ��.��ƷID, SUM(��汨����ϸ��.��������) AS ��汨������, SUM(��汨����ϸ��.������) AS ��汨���� FROM ��汨����ϸ�� INNER JOIN ��汨����ܱ� ON ��汨����ϸ��.����ID = ��汨����ܱ�.ID WHERE (��汨����ܱ�.BeActive = 1) AND (��汨����ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (��汨����ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��汨����ϸ��.��ƷID";

                strL = "SELECT �����˲������ϸ��.��ƷID, SUM(�����˲������ϸ��.��������) AS ���۲�������, SUM(�����˲������ϸ��.���) AS ���۲��۽�� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY �����˲������ϸ��.��ƷID";

                strK = "SELECT �����˲������ϸ��.��ƷID, SUM(�����˲������ϸ��.��������) AS ������������, SUM(�����˲������ϸ��.���) AS �������۽�� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY �����˲������ϸ��.��ƷID";


                sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��ת��������ܱ�1.��ת���� AS ���ڽ�ת����, ��ת��������ܱ�1.��ת���� AS ���ڽ�ת����, ��ת��������ܱ�1.��ת��� AS ���ڽ�ת���, ��������.�����������, ��������.���������,�����˳���.�����˳�����,�����˳���.�����˳����,����.�������, ����.�����,������.��������, ������.�����ɱ�, ������.�������,�����˳���.�����˳�����,�����˳���.�����˳��ɱ�,�����˳���.�����˳����,������.����ë��,�����.��������, �����.������, ��������.�����������,��������.���������,��������.�����������,��������.���������,�����ֱ�.����������,�����ֱ�.�����ֽ��,��汨���.��汨������,��汨���.��汨����,��Ʒ��.������� AS ���ڽ�ת����, ��Ʒ��.���ɱ��� AS ���ڽ�ת����, ��Ʒ��.�������*��Ʒ��.���ɱ��� AS ���ڽ�ת���, �����˲���۱�.������������,�����˲���۱�.�������۽��,�����˲���۱�.���۲�������,�����˲���۱�.���۲��۽��,��Ʒ��.ID FROM ��Ʒ�� LEFT OUTER JOIN (" + strA + ") ��ת��������ܱ�1 ON ��Ʒ��.ID = ��ת��������ܱ�1.��ƷID LEFT OUTER JOIN (" + strB + ") ������ ON ��Ʒ��.ID = ������.��ƷID LEFT OUTER JOIN (" + strC + ") ����� ON ��Ʒ��.ID = �����.��ƷID LEFT OUTER JOIN (" + strD + ") ���� ON ��Ʒ��.ID = ����.��ƷID LEFT OUTER JOIN (" + strE + ") �������� ON ��Ʒ��.ID = ��������.��ƷID LEFT OUTER JOIN (" + strF + ") �����˳��� ON ��Ʒ��.ID = �����˳���.��ƷID LEFT OUTER JOIN (" + strG + ") �����˳��� ON ��Ʒ��.ID = �����˳���.��ƷID LEFT OUTER JOIN (" + strH + ") �������� ON ��Ʒ��.ID = ��������.��ƷID LEFT OUTER JOIN (" + strI + ") ��汨��� ON ��Ʒ��.ID = ��汨���.��ƷID LEFT OUTER JOIN (" + strJ + ") �������� ON ��Ʒ��.ID = ��������.��ƷID LEFT OUTER JOIN (" + strK + ") �����˲���۱� ON ��Ʒ��.ID = �����˲���۱�.��ƷID LEFT OUTER JOIN (" + strL + ") �����˲���۱� ON ��Ʒ��.ID = �����˲���۱�.��ƷID LEFT OUTER JOIN (" + strM + ") �����ֱ� ON ��Ʒ��.ID = �����ֱ�.��ƷID WHERE (��Ʒ��.beactive = 1)";

                sqlConn.Open();
                
                if (dSet.Tables.Contains("��ת��������ܱ�")) dSet.Tables.Remove("��ת��������ܱ�");
                sqlDA.Fill(dSet, "��ת��������ܱ�");

                //����ϼ�
                object[] rowVals = new object[37];
                decimal[] rowDTemp = new decimal[37];

                rowVals[0] = "";
                rowVals[2] = "";
                rowVals[1] = "�ϼ�";
                for (i = 0; i < dSet.Tables["��ת��������ܱ�"].Columns.Count-1; i++)
                    rowDTemp[i] = 0;

                for (i = 0; i < dSet.Tables["��ת��������ܱ�"].Rows.Count; i++)
                    for (j = 3; j < dSet.Tables["��ת��������ܱ�"].Columns.Count - 1; j++)
                    {
                        if (dSet.Tables["��ת��������ܱ�"].Rows[i][j].ToString() == "")
                            dSet.Tables["��ת��������ܱ�"].Rows[i][j] = 0;
                    }

                //ë��
                for (i = 0; i < dSet.Tables["��ת��������ܱ�"].Rows.Count; i++)
                    dSet.Tables["��ת��������ܱ�"].Rows[i][18] = Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][14]) - Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][17]) - (Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][13]) - Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][16])) + Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][35]);

                for (i = 0; i < dSet.Tables["��ת��������ܱ�"].Rows.Count; i++)
                    for (j = 3; j < dSet.Tables["��ת��������ܱ�"].Columns.Count-1; j++)
                    {
                        rowDTemp[j] += Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][j]);

                    }

                for (i = 3; i < dSet.Tables["��ת��������ܱ�"].Columns.Count-1; i++)
                    rowVals[i] = rowDTemp[i];


                dSet.Tables["��ת��������ܱ�"].Rows.Add(rowVals);

                dataGridViewJXCHZ.DataSource = dSet.Tables["��ת��������ܱ�"];


                

                dataGridViewJXCHZ.Columns[36].Visible = false;
                dataGridViewJXCHZ.Columns[3].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[6].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[8].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[10].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[12].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[15].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[19].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[21].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[23].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[25].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[27].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[29].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[32].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[34].DefaultCellStyle.Format = "f0";

                dataGridViewJXCHZ.Columns[4].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[5].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[7].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[9].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[11].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[13].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[14].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[16].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[17].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[18].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[20].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[22].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[24].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[26].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[28].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[29].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[30].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[31].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[33].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[35].DefaultCellStyle.Format = "f2";


                for (i = 1; i < dataGridViewJXCHZ.ColumnCount; i++)
                {
                    dataGridViewJXCHZ.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ݿ����" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlConn.Close();
            }
        }

        /*
        private void initdataGridViewCBLRB()
        {
            DataView dvtemp = new DataView(dSet.Tables["��ת��������ܱ�"]);
            dataGridViewCBLRB.DataSource = dvtemp;

            dataGridViewCBLRB.Columns[0].Visible = false;
            dataGridViewCBLRB.Columns[3].Visible = false;
            dataGridViewCBLRB.Columns[4].Visible = false;
            dataGridViewCBLRB.Columns[5].Visible = false;
            dataGridViewCBLRB.Columns[6].Visible = false;
            dataGridViewCBLRB.Columns[7].Visible = false;
            dataGridViewCBLRB.Columns[8].Visible = false;
            dataGridViewCBLRB.Columns[9].Visible = false;
            dataGridViewCBLRB.Columns[12].Visible = false;
            dataGridViewCBLRB.Columns[13].Visible = false;
            dataGridViewCBLRB.Columns[14].Visible = false;


            for (int i = 1; i < dataGridViewCBLRB.ColumnCount; i++)
            {
                dataGridViewCBLRB.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

        }*/

        private void initdataGridViewKFHZ()
        {
            int i, j;

           try
            {

                string strA = "", strB = "", strC = "", strD = "", strE = "";

                strA = "SELECT * FROM ��ת�ⷿ���ܱ� WHERE (��תID = " + iJZID.ToString() + ")";

                strB = "SELECT ��ƷID, SUM(����) AS ��������, SUM(ʵ�ƽ��) AS ������, �ⷿID FROM ������ͼ WHERE (���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��ƷID, �ⷿID";

                strC = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.���) AS ���������, ���������ϸ��.�ⷿID FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.����ID = ���������ܱ�.ID WHERE (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.BeActive = 1) GROUP BY ���������ϸ��.��ƷID, ���������ϸ��.�ⷿID";

                strD = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, SUM(������Ʒ�Ƶ���ϸ��.����) AS ��������, SUM(������Ʒ�Ƶ���ϸ��.���ɱ��� * ������Ʒ�Ƶ���ϸ��.����) / SUM(������Ʒ�Ƶ���ϸ��.����) AS �����ɱ�, SUM(������Ʒ�Ƶ���.��˰�ϼ�) AS �������, SUM(������Ʒ�Ƶ���ϸ��.ë��) AS ����ë��, ������Ʒ�Ƶ���ϸ��.�ⷿID FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID WHERE (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ��.�ⷿID";

                strE = "SELECT ��ƷID, SUM(����) AS �������, SUM(���) AS �����, �ⷿID FROM �����ͼ WHERE (���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��ƷID, �ⷿID";

                sqlComm.CommandText = "SELECT ����.�ⷿID, ��Ʒ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��ת�ⷿ���ܱ�1.��ת���� AS ���ڽ�ת����, ��ת�ⷿ���ܱ�1.��ת���� AS ���ڽ�ת����, ��ת�ⷿ���ܱ�1.��ת��� AS ���ڽ�ת���, ����.�������, ����.�����, ��������.�����������, ��������.���������, �����.��������, �����.������, ������.��������, ������.�����ɱ�, ������.�������, ����.������� AS ���ڽ�ת����, ����.����� AS ���ڽ�ת���, ����.���ɱ��� AS ���ڽ�ת����, ������.����ë�� FROM ��Ʒ�� INNER JOIN ���� ON ��Ʒ��.ID = ����.��ƷID LEFT OUTER JOIN ("+strA+") ��ת�ⷿ���ܱ�1 ON ����.�ⷿID = ��ת�ⷿ���ܱ�1.�ⷿID AND ����.��ƷID = ��ת�ⷿ���ܱ�1.��ƷID LEFT OUTER JOIN ("+strB+") ����� ON ����.��ƷID = �����.��ƷID AND ����.�ⷿID = �����.�ⷿID LEFT OUTER JOIN ("+strC+") �������� ON ����.�ⷿID = ��������.�ⷿID AND ����.��ƷID = ��������.��ƷID LEFT OUTER JOIN ("+strD+") ������ ON ����.��ƷID = ������.��ƷID AND ����.�ⷿID = ������.�ⷿID LEFT OUTER JOIN ("+strE+") ���� ON ����.�ⷿID = ����.�ⷿID AND ����.��ƷID = ����.��ƷID WHERE (��Ʒ��.beactive = 1)";


            sqlConn.Open();
            if (dSet.Tables.Contains("��ת�����ܱ�")) dSet.Tables.Remove("��ת�����ܱ�");
            sqlDA.Fill(dSet, "��ת�����ܱ�");

            for (i = 0; i < dSet.Tables["��ת�����ܱ�"].Rows.Count; i++)
                for (j = 3; j < dSet.Tables["��ת�����ܱ�"].Columns.Count; j++)
                {
                    if (dSet.Tables["��ת�����ܱ�"].Rows[i][j].ToString() == "")
                        dSet.Tables["��ת�����ܱ�"].Rows[i][j] = 0;
                }
            //sqlConn.Close();
            }
           catch (Exception ex)
           {
               MessageBox.Show("���ݿ����" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
           finally
           {
               sqlConn.Close();
           }

        }

        private void dataGridViewKF_Click(object sender, EventArgs e)
        {
            int i, j;

            int iSelectKF = 0;
            if (dataGridViewKF.SelectedRows.Count < 1)
            {
                iSelectKF = Convert.ToInt32(dataGridViewKF.Rows[0].Cells[0].Value.ToString());
            }
            else
            {
                iSelectKF = Convert.ToInt32(dataGridViewKF.SelectedRows[0].Cells[0].Value.ToString());
            }

            DataView dt = new DataView(dSet.Tables["��ת�����ܱ�"], "�ⷿID=" + iSelectKF.ToString(), "", DataViewRowState.CurrentRows);
            dataGridViewKFHZ.DataSource=dt;

            dataGridViewKFHZ.Columns[0].Visible = false;
            dataGridViewKFHZ.Columns[1].Visible = false;
            dataGridViewKFHZ.Columns[7].Visible = false;
            dataGridViewKFHZ.Columns[8].Visible = false;
            dataGridViewKFHZ.Columns[9].Visible = false;
            dataGridViewKFHZ.Columns[10].Visible = false;
            dataGridViewKFHZ.Columns[11].Visible = false;
            dataGridViewKFHZ.Columns[12].Visible = false;
            dataGridViewKFHZ.Columns[13].Visible = false;
            dataGridViewKFHZ.Columns[14].Visible = false;
            dataGridViewKFHZ.Columns[15].Visible = false;
            dataGridViewKFHZ.Columns[19].Visible = false;
            for (i = 1; i < dataGridViewKFHZ.ColumnCount; i++)
            {
                dataGridViewKFHZ.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

        }

        private void initdataGridViewGJWLHZ()
        {
            int i, j;

            try		

            {

                string strA = "", strB = "", strC = "", strD = "", strE = "";
                /*
                strA = "SELECT * FROM ��ת�������ܱ� WHERE (��תID = " + iJZID.ToString() + ")";

                strB = "SELECT SUM(ʵ�ƽ��) AS �տ���, ��λID FROM �����տ���ܱ� WHERE (���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (BeActive = 1) GROUP BY ��λID";

                strC = "SELECT SUM(������Ʒ�Ƶ���.��˰�ϼ�) AS �����������, ������Ʒ�Ƶ���.��λID FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID WHERE (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) GROUP BY ������Ʒ�Ƶ���.��λID";

                strD = "SELECT SUM(ʵ�ƽ��) AS ���ڸ�����, ��λID FROM �����տ���ܱ� WHERE (���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (BeActive = 1) GROUP BY ��λID";

                strE = "SELECT SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.���) AS ���ڹ������, ���������ܱ�.��λID FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.����ID = ���������ܱ�.ID WHERE (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.BeActive = 1) GROUP BY ���������ܱ�.��λID";
               
                sqlComm.CommandText = "SELECT ��λ��.ID, ��λ��.�Ƿ����, ��λ��.�Ƿ�����, ��λ��.��λ���, ��λ��.��λ����,��ת�������ܱ�1.Ӧ����� AS ����Ӧ�����, ��������.���ڹ������, �����.���ڸ�����, ��λ��.Ӧ���˿� AS ����Ӧ�����, ��ת�������ܱ�1.Ӧ����� AS ����Ӧ�����, ������.�����������, �տ��.�տ���, ��λ��.Ӧ���˿� AS ����Ӧ����� FROM ��λ�� LEFT OUTER JOIN ("+strA+") ��ת�������ܱ�1 ON  ��λ��.ID = ��ת�������ܱ�1.��λID LEFT OUTER JOIN ("+strB+") �տ�� ON ��λ��.ID = �տ��.��λID LEFT OUTER JOIN ("+strC+") ������ ON ��λ��.ID = ������.��λID LEFT OUTER JOIN ("+strD+") ����� ON ��λ��.ID = �����.��λID LEFT OUTER JOIN ("+strE+") �������� ON ��λ��.ID = ��������.��λID WHERE (��λ��.BeActive = 1)";
                */

                strA = "SELECT * FROM ��ת�������ܱ� WHERE (��תID = " + iJZID.ToString() + ")";

                strB = "SELECT ��λID, SUM(δ������) AS Ӧ����� FROM �տ������ͼ WHERE (BeActive = 1) GROUP BY ��λID";

                strC = "SELECT ��λID, SUM(δ������) AS Ӧ����� FROM ���������ͼ WHERE (BeActive = 1) GROUP BY ��λID";

                sqlComm.CommandText = "SELECT ��λ��.ID, ��λ��.��λ���, ��λ��.��λ����,��ת�������ܱ�1.Ӧ����� AS ����Ӧ�����, ��ת�������ܱ�1.Ӧ����� AS ����Ӧ�����, �����.Ӧ����� AS ����Ӧ�����, �տ��.Ӧ����� AS ����Ӧ�ս�� FROM ��λ�� LEFT OUTER JOIN (" + strA + ") ��ת�������ܱ�1 ON  ��λ��.ID = ��ת�������ܱ�1.��λID LEFT OUTER JOIN (" + strB + ") �տ�� ON ��λ��.ID = �տ��.��λID LEFT OUTER JOIN (" + strC + ") ����� ON ��λ��.ID = �����.��λID WHERE (��λ��.BeActive = 1)";


                sqlConn.Open();
                if (dSet.Tables.Contains("��ת�������ܱ�")) dSet.Tables.Remove("��ת�������ܱ�");
                sqlDA.Fill(dSet, "��ת�������ܱ�");

                //decimal dt1 = 0, dt2 = 0, dt3 = 0, dt4 = 0;

                //����ϼ�
                object[] rowVals = new object[7];
                decimal[] rowDTemp = new decimal[7];

                rowVals[0] = 0;
                rowVals[2] = "�ϼ�"; 
                for (i = 0; i < dSet.Tables["��ת�������ܱ�"].Columns.Count; i++)
                    rowDTemp[i] = 0;

                for (i = 0; i < dSet.Tables["��ת�������ܱ�"].Rows.Count; i++)
                    for (j = 3; j < dSet.Tables["��ת�������ܱ�"].Columns.Count; j++)
                    {
                        if (dSet.Tables["��ת�������ܱ�"].Rows[i][j].ToString() == "")
                            dSet.Tables["��ת�������ܱ�"].Rows[i][j] = 0;

                        rowDTemp[j] += decimal.Parse(dSet.Tables["��ת�������ܱ�"].Rows[i][j].ToString());

                    }
                for (i = 3; i < dSet.Tables["��ת�������ܱ�"].Columns.Count; i++)
                    rowVals[i] = rowDTemp[i];


                dSet.Tables["��ת�������ܱ�"].Rows.Add(rowVals);
                //sqlConn.Close();

                //DataView dt = new DataView(dSet.Tables["��ת�������ܱ�"], "�Ƿ����=1", "", DataViewRowState.CurrentRows);
                DataView dt = new DataView(dSet.Tables["��ת�������ܱ�"]);
                dataGridViewGJWLHZ.DataSource = dt;


                dataGridViewGJWLHZ.Columns[0].Visible = false;
                dataGridViewGJWLHZ.Columns[4].Visible = false;
                dataGridViewGJWLHZ.Columns[6].Visible = false;
                dataGridViewGJWLHZ.Columns[3].DefaultCellStyle.Format = "f2";
                dataGridViewGJWLHZ.Columns[5].DefaultCellStyle.Format = "f2";

                for (i = 1; i < dataGridViewGJWLHZ.ColumnCount; i++)
                {
                    dataGridViewGJWLHZ.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                //DataView dt1 = new DataView(dSet.Tables["��ת�������ܱ�"], "�Ƿ�����=1", "", DataViewRowState.CurrentRows);
                DataView dt1 = new DataView(dSet.Tables["��ת�������ܱ�"]);
                dataGridViewXSWLHZ.DataSource = dt1;

                dataGridViewXSWLHZ.Columns[0].Visible = false;
                dataGridViewXSWLHZ.Columns[3].Visible = false;
                dataGridViewXSWLHZ.Columns[5].Visible = false;
                dataGridViewXSWLHZ.Columns[4].DefaultCellStyle.Format = "f2";
                dataGridViewXSWLHZ.Columns[6].DefaultCellStyle.Format = "f2";

                for (i = 1; i < dataGridViewXSWLHZ.ColumnCount; i++)
                {
                    dataGridViewXSWLHZ.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("���ݿ����" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlConn.Close();
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i,j;

            //�������
            if (isSaved)
            {
                MessageBox.Show("����ת���Ѿ���ɣ�ת��ʱ��Ϊ:"+labelBCJZRQ.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("�Ƿ����ת�᣿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;


            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                //������
                sqlComm.CommandText = "INSERT INTO ��ת���ܱ� (����ʱ��, ����ԱID) VALUES ('"+sBCJZSJ+"', "+intUserID.ToString()+")";
                sqlComm.ExecuteNonQuery();

                //ȡ�õ��ݺ� 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //��ת��������ܱ�
                for (i = 0; i < dSet.Tables["��ת��������ܱ�"].Rows.Count; i++)
                {
                    if (dSet.Tables["��ת��������ܱ�"].Rows[i][36].ToString() == "")
                        continue;
                    if (dSet.Tables["��ת��������ܱ�"].Rows[i][36].ToString() == "0")
                        continue;

                    sqlComm.CommandText = "INSERT INTO ��ת��������ܱ� (��תID, ��ƷID, ��ת����, ��ת����, ��ת���) VALUES (" + sBillNo + ", " + dSet.Tables["��ת��������ܱ�"].Rows[i][36].ToString() + ", " + dSet.Tables["��ת��������ܱ�"].Rows[i][29].ToString() + ", " + dSet.Tables["��ת��������ܱ�"].Rows[i][30].ToString() + ", " + dSet.Tables["��ת��������ܱ�"].Rows[i][31].ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                //��ת�ⷿ���ܱ�
                for (i = 0; i < dSet.Tables["��ת�����ܱ�"].Rows.Count ; i++)
                {
                    if (dSet.Tables["��ת��������ܱ�"].Rows[i][1].ToString() == "")
                        continue;
                    if (dSet.Tables["��ת��������ܱ�"].Rows[i][1].ToString() == "0")
                        continue;
                    if (dSet.Tables["��ת��������ܱ�"].Rows[i][0].ToString() == "")
                        continue;
                    if (dSet.Tables["��ת��������ܱ�"].Rows[i][0].ToString() == "0")
                        continue;

                    sqlComm.CommandText = "INSERT INTO ��ת�ⷿ���ܱ� (��תID, ��ƷID, �ⷿID, ��ת����, ��ת����, ��ת���) VALUES (" + sBillNo + ", " + dSet.Tables["��ת�����ܱ�"].Rows[i][1].ToString() + ", " + dSet.Tables["��ת�����ܱ�"].Rows[i][0].ToString() + ", " + dSet.Tables["��ת�����ܱ�"].Rows[i][16].ToString() + ", " + dSet.Tables["��ת�����ܱ�"].Rows[i][18].ToString() + ", " + dSet.Tables["��ת�����ܱ�"].Rows[i][17].ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                //��ת�������ܱ�
                for (i = 0; i < dSet.Tables["��ת�������ܱ�"].Rows.Count ; i++)
                {
                    if (dSet.Tables["��ת�������ܱ�"].Rows[i][0].ToString() == "")
                        continue;
                    if (dSet.Tables["��ת�������ܱ�"].Rows[i][0].ToString() == "0")
                        continue;

                    sqlComm.CommandText = "INSERT INTO ��ת�������ܱ� (��תID, ��λID, Ӧ�����, Ӧ�����) VALUES (" + sBillNo + ", " + dSet.Tables["��ת�������ܱ�"].Rows[i][0].ToString() + ", " + dSet.Tables["��ת�������ܱ�"].Rows[i][5].ToString() + ", " + dSet.Tables["��ת�������ܱ�"].Rows[i][6].ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                sqlComm.CommandText = "UPDATE ��Ʒ�� SET ����� = ������� * ���ɱ���";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE ���� SET ����� = ������� * ���ɱ���";
                sqlComm.ExecuteNonQuery();



                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ݿ����" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                return;
            }
            finally
            {
                sqlConn.Close();
            }



            MessageBox.Show("����ת����ɣ�ת��ʱ��Ϊ:" + labelBCJZRQ.Text, "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isSaved = true;

        }

        private void FormYWSJJZ_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
            {
                return;
            }

            DialogResult dr = MessageBox.Show(this, "��δ����ҵ�����ݽ�ת��ȷ��Ҫ�˳���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";            
            switch (tabControlDJMX.SelectedIndex)
            {
                case 0:
                    strT = "ҵ�����ݽ�ת����������ܣ�;��ת���ڣ�" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewJXCHZ, strT, true, intUserLimit);
                    break;

                case 1:
                    strT = "ҵ�����ݽ�ת���ⷿ���ܣ�;��ת���ڣ�" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewKFHZ, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "ҵ�����ݽ�ת�������������ܣ�;��ת���ڣ�" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewGJWLHZ, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "ҵ�����ݽ�ת�������������ܣ�;��ת���ڣ�" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewXSWLHZ, strT, true, intUserLimit);
                    break;


            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";
            switch (tabControlDJMX.SelectedIndex)
            {
                case 0:
                    strT = "ҵ�����ݽ�ת����������ܣ�;��ת���ڣ�" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewJXCHZ, strT, false, intUserLimit);
                    break;

                case 1:
                    strT = "ҵ�����ݽ�ת���ⷿ���ܣ�;��ת���ڣ�" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewKFHZ, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "ҵ�����ݽ�ת�������������ܣ�;��ת���ڣ�" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewGJWLHZ, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "ҵ�����ݽ�ת�������������ܣ�;��ת���ڣ�" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewXSWLHZ, strT, false, intUserLimit);
                    break;


            }
        }

        private void dateTimePickerJZ_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerJZ.Value < DateTime.Parse(sSCJZSJ))
             return;

            sBCJZSJ = dateTimePickerJZ.Value.ToShortDateString();
            labelBCJZRQ.Text = Convert.ToDateTime(sBCJZSJ).ToString("yyyy��M��dd��");

            initdataGridViewJXCHZ();
            //initdataGridViewCBLRB();
            initdataGridViewKF();
            initdataGridViewKFHZ();
            dataGridViewKF_Click(null, null);
            initdataGridViewGJWLHZ();
            
        }

        private void buttonBR_Click(object sender, EventArgs e)
        {
            //�õ��ϴν�תʱ��
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ����ʱ��,ID FROM ��ת���ܱ� ORDER BY ����ʱ�� DESC";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                iJZID = Convert.ToInt32(sqldr.GetValue(1).ToString());
            }
            sqldr.Close();

            if (sSCJZSJ == "") //û�н���
            {
                sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                iJZID = 0;
                sqldr.Close();
            }
            labelSCJZRQ.Text = Convert.ToDateTime(sSCJZSJ).ToString("yyyy��M��dd��");

            sqlConn.Close();

            //initHTDefault();
            cGetInformation.getSystemDateTime();
            //sBCJZSJ = cGetInformation.strSYSDATATIME;
            sBCJZSJ = Convert.ToDateTime(cGetInformation.strSYSDATATIME).ToShortDateString();
            labelBCJZRQ.Text = Convert.ToDateTime(sBCJZSJ).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            initdataGridViewJXCHZ();
            //initdataGridViewCBLRB();
            initdataGridViewKF();
            initdataGridViewKFHZ();
            dataGridViewKF_Click(null, null);
            initdataGridViewGJWLHZ();
        }

    }
}