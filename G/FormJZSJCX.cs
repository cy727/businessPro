using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJZSJCX : Form
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

        private int intKFID = 0;
        private int iJZID = 0;

        private string sSCJZSJ = "", sBCJZSJ = "";
 
        public FormJZSJCX()
        {
            InitializeComponent();
        }

        private void FormJZSJCX_Load(object sender, EventArgs e)
        {
            this.Left = 1;
            this.Top = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            //�õ��ϴν�ת
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ����ʱ��,ID FROM ��ת���ܱ� ORDER BY ����ʱ�� DESC";
            if (dSet.Tables.Contains("��ת���ܱ�")) dSet.Tables.Remove("��ת���ܱ�");
            sqlDA.Fill(dSet, "��ת���ܱ�");
            comboBoxJZSJ.DataSource = dSet.Tables["��ת���ܱ�"];
            comboBoxJZSJ.DisplayMember = "����ʱ��";
            comboBoxJZSJ.ValueMember = "ID";
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            labelZDRQ.Text = Convert.ToDateTime(cGetInformation.strSYSDATATIME).ToString("yyyy��M��dd��");


        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(1, "") == 0)
            {
                //return;
            }
            else
            {
                intKFID = cGetInformation.iKFNumber;
                //textBoxKFMC.Text = cGetInformation.strKFName;
                textBoxKFBH.Text = cGetInformation.strKFCode;
            }
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFBH.Text) == 0)
                {
                    intKFID = 0;
                    textBoxKFBH.Text = "";
                    //return;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    //extBoxKFMC.Text = cGetInformation.strKFName;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i;

            if (comboBoxJZSJ.Items.Count<1)
                return;

            if (comboBoxJZSJ.SelectedValue.ToString()=="")
                return;


            iJZID = int.Parse(comboBoxJZSJ.SelectedValue.ToString());

            //�õ�ʱ������
            sqlConn.Open();
            if (iJZID == 1) //��һ��ת��
            {
                sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                    break;
                }
                sqldr.Close();
            }
            else
            {
                i = iJZID - 1;
                sqlComm.CommandText = "SELECT ����ʱ��,ID FROM ��ת���ܱ� WHERE ID = " + i.ToString();
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                    break;
                }
                sqldr.Close();
            }
            sBCJZSJ=comboBoxJZSJ.Text;
            sqlConn.Close();

            initdataGridViewJXCHZ();
            //initdataGridViewCBLRB();
            initdataGridViewGJWLHZ();

            /*
            
            sqlComm.CommandText = "SELECT ��ת���ܱ�.����ʱ��, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��ת��������ܱ�.��ת����, ��ת��������ܱ�.��ת����, ��ת��������ܱ�.��ת���, ��ת��������ܱ�.�������, ��ת��������ܱ�.�����, ��ת��������ܱ�.�����������, ��ת��������ܱ�.���������, ��ת��������ܱ�.��������, ��ת��������ܱ�.������, ��ת��������ܱ�.��������, ��ת��������ܱ�.���۽�� FROM ��ת��������ܱ� INNER JOIN ��ת���ܱ� ON ��ת��������ܱ�.��תID = ��ת���ܱ�.ID INNER JOIN ��Ʒ�� ON ��ת��������ܱ�.��ƷID = ��Ʒ��.ID WHERE (��ת���ܱ�.ID = "+iJZID.ToString()+")";

            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");


            sqlComm.CommandText = "SELECT ��ת���ܱ�.����ʱ��, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��ת��������ܱ�.��������, ��ת��������ܱ�.������, ��ת��������ܱ�.��������, ��ת��������ܱ�.���۽��, ��ת��������ܱ�.����ë��, ��ת��������ܱ�.����ë���� FROM ��ת��������ܱ� INNER JOIN ��ת���ܱ� ON ��ת��������ܱ�.��תID = ��ת���ܱ�.ID INNER JOIN ��Ʒ�� ON ��ת��������ܱ�.��ƷID = ��Ʒ��.ID WHERE (��ת���ܱ�.ID = " + iJZID.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");


            sqlComm.CommandText = "SELECT ��ת���ܱ�.����ʱ��, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��ת�ⷿ���ܱ�.��ת����, ��ת�ⷿ���ܱ�.��ת����, ��ת�ⷿ���ܱ�.��ת���, ��ת�ⷿ���ܱ�.�������, ��ת�ⷿ���ܱ�.�����, ��ת�ⷿ���ܱ�.�����������, ��ת�ⷿ���ܱ�.���������, ��ת�ⷿ���ܱ�.��������, ��ת�ⷿ���ܱ�.������, ��ת�ⷿ���ܱ�.��������, ��ת�ⷿ���ܱ�.���۽��, ��ת�ⷿ���ܱ�.����ë��, ��ת�ⷿ���ܱ�.����ë���� FROM ��ת�ⷿ���ܱ� INNER JOIN ��ת���ܱ� ON ��ת�ⷿ���ܱ�.��תID = ��ת���ܱ�.ID INNER JOIN ��Ʒ�� ON ��ת�ⷿ���ܱ�.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ��ת�ⷿ���ܱ�.�ⷿID = �ⷿ��.ID WHERE (��ת���ܱ�.ID = " + iJZID.ToString() + ")";

            if (intKFID!= 0)
                sqlComm.CommandText += " AND (�ⷿ��.ID = "+intKFID.ToString()+")";

            if (dSet.Tables.Contains("��Ʒ��3")) dSet.Tables.Remove("��Ʒ��3");
            sqlDA.Fill(dSet, "��Ʒ��3");

            sqlComm.CommandText = "SELECT ��ת���ܱ�.����ʱ��, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��ת�ⷿ���ܱ�.��������, ��ת�ⷿ���ܱ�.������, ��ת�ⷿ���ܱ�.��������, ��ת�ⷿ���ܱ�.���۽��, ��ת�ⷿ���ܱ�.����ë��, ��ת�ⷿ���ܱ�.����ë���� FROM ��ת�ⷿ���ܱ� INNER JOIN ��ת���ܱ� ON ��ת�ⷿ���ܱ�.��תID = ��ת���ܱ�.ID INNER JOIN ��Ʒ�� ON ��ת�ⷿ���ܱ�.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ��ת�ⷿ���ܱ�.�ⷿID = �ⷿ��.ID WHERE (��ת���ܱ�.ID = " + iJZID.ToString() + ") ";

            if (intKFID != 0)
                sqlComm.CommandText += " AND (�ⷿ��.ID = " + intKFID.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��4")) dSet.Tables.Remove("��Ʒ��4");
            sqlDA.Fill(dSet, "��Ʒ��4");


            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["��Ʒ��1"];
            dataGridView2.DataSource = dSet.Tables["��Ʒ��2"];
            dataGridView3.DataSource = dSet.Tables["��Ʒ��3"];
            dataGridView4.DataSource = dSet.Tables["��Ʒ��4"];

            dataGridView1.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[10].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[11].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[10].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[12].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[13].DefaultCellStyle.Format = "f0";
            dataGridView4.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridView4.Columns[7].DefaultCellStyle.Format = "f0";
             */


        }

        /*
        private void initdataGridViewJXCHZ()
        {
            int i, j;
            try
            {
                string strA = "", strB = "", strC = "", strD = "", strE = "";

                strA = "SELECT * FROM ��ת��������ܱ� WHERE (��תID =  " + iJZID.ToString() + ")";

                strB = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, SUM(������Ʒ�Ƶ���ϸ��.����) AS ��������, SUM(������Ʒ�Ƶ���ϸ��.���ɱ��� * ������Ʒ�Ƶ���ϸ��.����)  AS �����ɱ�, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������, SUM(������Ʒ�Ƶ���ϸ��.ë��) AS ����ë�� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID WHERE (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

                strC = "SELECT ��ƷID, SUM(����) AS ��������, SUM(ʵ�ƽ��) AS ������ FROM ������ͼ WHERE (���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��ƷID";

                strD = "SELECT ��ƷID, SUM(����) AS �������, SUM(���) AS ����� FROM �����ͼ WHERE (���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��ƷID";

                strE = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.���) AS ��������� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.����ID = ���������ܱ�.ID WHERE (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102) AND (���������ܱ�.BeActive = 1)) GROUP BY ���������ϸ��.��ƷID";

                sqlComm.CommandText = "SELECT ��Ʒ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��ת��������ܱ�1.��ת���� , ��ת��������ܱ�1.��ת���� , ��ת��������ܱ�1.��ת���, ����.�������, ����.�����, ��������.�����������, ��������.���������, �����.��������, �����.������, ������.��������, ������.�����ɱ�, ������.�������, ������.����ë�� FROM ��Ʒ�� LEFT OUTER JOIN (" + strA + ") ��ת��������ܱ�1 ON ��Ʒ��.ID = ��ת��������ܱ�1.��ƷID LEFT OUTER JOIN (" + strB + ") ������ ON ��Ʒ��.ID = ������.��ƷID LEFT OUTER JOIN (" + strC + ") ����� ON ��Ʒ��.ID = �����.��ƷID LEFT OUTER JOIN (" + strD + ") ���� ON ��Ʒ��.ID = ����.��ƷID LEFT OUTER JOIN (" + strE + ") �������� ON ��Ʒ��.ID = ��������.��ƷID WHERE (��Ʒ��.beactive = 1)";

                sqlConn.Open();

                if (dSet.Tables.Contains("��ת��������ܱ�")) dSet.Tables.Remove("��ת��������ܱ�");
                sqlDA.Fill(dSet, "��ת��������ܱ�");

                //����ϼ�
                object[] rowVals = new object[16];
                decimal[] rowDTemp = new decimal[16];

                rowVals[0] = 0;
                rowVals[2] = "";
                rowVals[1] = "�ϼ�";
                for (i = 0; i < dSet.Tables["��ת��������ܱ�"].Columns.Count; i++)
                    rowDTemp[i] = 0;

                for (i = 0; i < dSet.Tables["��ת��������ܱ�"].Rows.Count; i++)
                    for (j = 3; j < dSet.Tables["��ת��������ܱ�"].Columns.Count; j++)
                    {
                        if (dSet.Tables["��ת��������ܱ�"].Rows[i][j].ToString() == "")
                            dSet.Tables["��ת��������ܱ�"].Rows[i][j] = 0;
                        rowDTemp[j] += Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][j]);

                    }

                for (i = 3; i < dSet.Tables["��ת��������ܱ�"].Columns.Count; i++)
                    rowVals[i] = rowDTemp[i];


                dSet.Tables["��ת��������ܱ�"].Rows.Add(rowVals);

                dataGridViewJXCHZ.DataSource = dSet.Tables["��ת��������ܱ�"];


                dataGridViewJXCHZ.Columns[0].Visible = false;


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
            dataGridViewJXCHZ.Columns[3].DefaultCellStyle.Format = "f0";
        }
        */
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

        /*
        private void initdataGridViewJXCHZ()
        {
            int i, j;
            try
            {
                string strA = "", strB = "", strC = "", strD = "", strE = "", strF = "", strG = "", strH = "", strI = "", strJ = "";

                strA = "SELECT * FROM ��ת��������ܱ� WHERE (��תID =  " + iJZID.ToString() + ")";

                strB = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, SUM(������Ʒ�Ƶ���ϸ��.����) AS ��������, SUM(������Ʒ�Ƶ���ϸ��.���ɱ��� * ������Ʒ�Ƶ���ϸ��.����)  AS �����ɱ�, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������, SUM(������Ʒ�Ƶ���ϸ��.ë��) AS ����ë�� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID WHERE (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

                strC = "SELECT ��ƷID, SUM(����) AS ��������, SUM(ʵ�ƽ��) AS ������ FROM ������ͼ WHERE (���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��ƷID";

                strD = "SELECT ��ƷID, SUM(����) AS �������, SUM(���) AS ����� FROM �����ͼ WHERE (���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��ƷID";

                strE = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.���) AS ��������� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.����ID = ���������ܱ�.ID WHERE (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102) AND (���������ܱ�.BeActive = 1)) GROUP BY ���������ϸ��.��ƷID";

                strF = "SELECT �����˳���ϸ��.��ƷID, SUM(�����˳���ϸ��.����) AS �����˳�����, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳���� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY �����˳���ϸ��.��ƷID";

                strG = "SELECT �����˳���ϸ��.��ƷID, SUM(�����˳���ϸ��.����) AS �����˳�����, SUM(�����˳���ϸ��.����*�����˳���ϸ��.���ɱ���) AS �����˳��ɱ�, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳���� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY �����˳���ϸ��.��ƷID";

                strH = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.������) AS ��������� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ���������ϸ��.��ƷID";

                strI = "SELECT  ��汨����ϸ��.��ƷID, SUM(��汨����ϸ��.��������) AS ��汨������, SUM(��汨����ϸ��.������) AS ��汨���� FROM ��汨����ϸ�� INNER JOIN ��汨����ܱ� ON ��汨����ϸ��.����ID = ��汨����ܱ�.ID WHERE (��汨����ܱ�.BeActive = 1) AND (��汨����ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (��汨����ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��汨����ϸ��.��ƷID";

                sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��ת��������ܱ�1.��ת���� AS ��ת����, ��ת��������ܱ�1.��ת���� AS ��ת����, ��ת��������ܱ�1.��ת��� AS ��ת���, ��������.�����������, ��������.���������,�����˳���.�����˳�����,�����˳���.�����˳����,����.�������, ����.�����,������.��������, ������.�����ɱ�, ������.�������,�����˳���.�����˳�����,�����˳���.�����˳��ɱ�,�����˳���.�����˳����,������.����ë��,�����.��������, �����.������, ��������.�����������,��������.���������,��汨���.��汨������,��汨���.��汨����,��Ʒ��.������� AS ���ڽ�ת����, ��Ʒ��.���ɱ��� AS ���ڽ�ת����, ��Ʒ��.�������*��Ʒ��.���ɱ��� AS ���ڽ�ת���, ��Ʒ��.ID FROM ��Ʒ�� LEFT OUTER JOIN (" + strA + ") ��ת��������ܱ�1 ON ��Ʒ��.ID = ��ת��������ܱ�1.��ƷID LEFT OUTER JOIN (" + strB + ") ������ ON ��Ʒ��.ID = ������.��ƷID LEFT OUTER JOIN (" + strC + ") ����� ON ��Ʒ��.ID = �����.��ƷID LEFT OUTER JOIN (" + strD + ") ���� ON ��Ʒ��.ID = ����.��ƷID LEFT OUTER JOIN (" + strE + ") �������� ON ��Ʒ��.ID = ��������.��ƷID LEFT OUTER JOIN (" + strF + ") �����˳��� ON ��Ʒ��.ID = �����˳���.��ƷID LEFT OUTER JOIN (" + strG + ") �����˳��� ON ��Ʒ��.ID = �����˳���.��ƷID LEFT OUTER JOIN (" + strH + ") �������� ON ��Ʒ��.ID = ��������.��ƷID LEFT OUTER JOIN (" + strI + ") ��汨��� ON ��Ʒ��.ID = ��汨���.��ƷID WHERE (��Ʒ��.beactive = 1)";

                sqlConn.Open();

                if (dSet.Tables.Contains("��ת��������ܱ�")) dSet.Tables.Remove("��ת��������ܱ�");
                sqlDA.Fill(dSet, "��ת��������ܱ�");

                //����ϼ�
                object[] rowVals = new object[28];
                decimal[] rowDTemp = new decimal[28];

                rowVals[0] = "";
                rowVals[2] = "";
                rowVals[1] = "�ϼ�";
                for (i = 0; i < dSet.Tables["��ת��������ܱ�"].Columns.Count - 1; i++)
                    rowDTemp[i] = 0;

                for (i = 0; i < dSet.Tables["��ת��������ܱ�"].Rows.Count; i++)
                    for (j = 3; j < dSet.Tables["��ת��������ܱ�"].Columns.Count - 1; j++)
                    {
                        if (dSet.Tables["��ת��������ܱ�"].Rows[i][j].ToString() == "")
                            dSet.Tables["��ת��������ܱ�"].Rows[i][j] = 0;
                    }

                //ë��
                for (i = 0; i < dSet.Tables["��ת��������ܱ�"].Rows.Count; i++)
                    dSet.Tables["��ת��������ܱ�"].Rows[i][18] = Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][14]) - Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][17]) - (Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][13]) - Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][16]));

                for (i = 0; i < dSet.Tables["��ת��������ܱ�"].Rows.Count; i++)
                    for (j = 3; j < dSet.Tables["��ת��������ܱ�"].Columns.Count - 1; j++)
                    {
                        rowDTemp[j] += Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][j]);

                    }

                for (i = 3; i < dSet.Tables["��ת��������ܱ�"].Columns.Count - 1; i++)
                    rowVals[i] = rowDTemp[i];


                dSet.Tables["��ת��������ܱ�"].Rows.Add(rowVals);

                dataGridViewJXCHZ.DataSource = dSet.Tables["��ת��������ܱ�"];




                dataGridViewJXCHZ.Columns[28].Visible = false;
                dataGridViewJXCHZ.Columns[25].Visible = false;
                dataGridViewJXCHZ.Columns[26].Visible = false;
                dataGridViewJXCHZ.Columns[27].Visible = false;

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
                dataGridViewJXCHZ.Columns[27].DefaultCellStyle.Format = "f2";


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
         * */
        private void initdataGridViewJXCHZ()
        {
            int i, j;
            try
            {
                string strAA = "", strA = "", strB = "", strC = "", strD = "", strE = "", strF = "", strG = "", strH = "", strI = "", strJ = "", strK = "", strL = "", strM = "";

                strA = "SELECT * FROM ��ת��������ܱ� WHERE (��תID =  " + iJZID.ToString() + ")";

                strAA = "SELECT * FROM ��ת��������ܱ� WHERE (��תID =  " + (iJZID-1).ToString() + ")";

                strB = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, SUM(������Ʒ�Ƶ���ϸ��.����) AS ��������, SUM(������Ʒ�Ƶ���ϸ��.���ɱ��� * ������Ʒ�Ƶ���ϸ��.����)  AS �����ɱ�, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������, SUM(������Ʒ�Ƶ���ϸ��.ë��) AS ����ë�� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID WHERE (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

                strC = "SELECT ��ƷID, SUM(����) AS ��������, SUM(ʵ�ƽ��) AS ������ FROM ������ͼ WHERE (���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��ƷID";

                strD = "SELECT ��ƷID, SUM(����) AS �������, SUM(���) AS ����� FROM �����ͼ WHERE (���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��ƷID";

                strE = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.���) AS ��������� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.����ID = ���������ܱ�.ID WHERE (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102) AND (���������ܱ�.BeActive = 1)) GROUP BY ���������ϸ��.��ƷID";

                strF = "SELECT �����˳���ϸ��.��ƷID, SUM(�����˳���ϸ��.����) AS �����˳�����, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳���� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY �����˳���ϸ��.��ƷID";

                strG = "SELECT �����˳���ϸ��.��ƷID, SUM(�����˳���ϸ��.����) AS �����˳�����, SUM(�����˳���ϸ��.����*�����˳���ϸ��.���ɱ���) AS �����˳��ɱ�, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳���� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY �����˳���ϸ��.��ƷID";

                strH = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.������) AS ��������� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (���������ϸ��.����>0) AND ((���������ܱ�.��ֵ���ID IS NULL) OR (���������ܱ�.��ֵ���ID <> -1)) GROUP BY ���������ϸ��.��ƷID";

                strJ = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS �����������, SUM(���������ϸ��.������) AS ��������� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (���������ϸ��.����<0) AND ((���������ܱ�.��ֵ���ID IS NULL) OR (���������ܱ�.��ֵ���ID <> -1)) GROUP BY ���������ϸ��.��ƷID";

                strI = "SELECT  ��汨����ϸ��.��ƷID, SUM(��汨����ϸ��.��������) AS ��汨������, SUM(��汨����ϸ��.������) AS ��汨���� FROM ��汨����ϸ�� INNER JOIN ��汨����ܱ� ON ��汨����ϸ��.����ID = ��汨����ܱ�.ID WHERE (��汨����ܱ�.BeActive = 1) AND (��汨����ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (��汨����ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY ��汨����ϸ��.��ƷID";

                strL = "SELECT �����˲������ϸ��.��ƷID, SUM(�����˲������ϸ��.��������) AS ���۲�������, SUM(�����˲������ϸ��.���) AS ���۲��۽�� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY �����˲������ϸ��.��ƷID";

                strM = "SELECT ���������ϸ��.��ƷID, SUM(���������ϸ��.����) AS ����������, SUM(���������ϸ��.������) AS �����ֽ�� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (���������ܱ�.��ֵ���ID = -1) GROUP BY ���������ϸ��.��ƷID";

                strK = "SELECT �����˲������ϸ��.��ƷID, SUM(�����˲������ϸ��.��������) AS ������������, SUM(�����˲������ϸ��.���) AS �������۽�� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY �����˲������ϸ��.��ƷID";


                sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��ת��������ܱ�1.��ת���� AS ���ڽ�ת����, ��ת��������ܱ�1.��ת���� AS ���ڽ�ת����, ��ת��������ܱ�1.��ת��� AS ���ڽ�ת���, ��������.�����������, ��������.���������,�����˳���.�����˳�����,�����˳���.�����˳����,����.�������, ����.�����,������.��������, ������.�����ɱ�, ������.�������,�����˳���.�����˳�����,�����˳���.�����˳��ɱ�,�����˳���.�����˳����,������.����ë��,�����.��������, �����.������, ��������.�����������,��������.���������,��������.�����������,��������.���������,�����ֱ�.����������,�����ֱ�.�����ֽ��,��汨���.��汨������,��汨���.��汨����,��ת��������ܱ�2.��ת���� AS ���ڽ�ת����, ��ת��������ܱ�2.��ת���� AS ���ڽ�ת����, ��ת��������ܱ�2.��ת��� AS ���ڽ�ת���, �����˲���۱�.������������,�����˲���۱�.�������۽��,�����˲���۱�.���۲�������,�����˲���۱�.���۲��۽��,��Ʒ��.ID FROM ��Ʒ�� LEFT OUTER JOIN (" + strAA + ") ��ת��������ܱ�1 ON ��Ʒ��.ID = ��ת��������ܱ�1.��ƷID LEFT OUTER JOIN (" + strB + ") ������ ON ��Ʒ��.ID = ������.��ƷID LEFT OUTER JOIN (" + strC + ") ����� ON ��Ʒ��.ID = �����.��ƷID LEFT OUTER JOIN (" + strD + ") ���� ON ��Ʒ��.ID = ����.��ƷID LEFT OUTER JOIN (" + strE + ") �������� ON ��Ʒ��.ID = ��������.��ƷID LEFT OUTER JOIN (" + strF + ") �����˳��� ON ��Ʒ��.ID = �����˳���.��ƷID LEFT OUTER JOIN (" + strG + ") �����˳��� ON ��Ʒ��.ID = �����˳���.��ƷID LEFT OUTER JOIN (" + strH + ") �������� ON ��Ʒ��.ID = ��������.��ƷID LEFT OUTER JOIN (" + strI + ") ��汨��� ON ��Ʒ��.ID = ��汨���.��ƷID LEFT OUTER JOIN (" + strJ + ") �������� ON ��Ʒ��.ID = ��������.��ƷID LEFT OUTER JOIN (" + strK + ") �����˲���۱� ON ��Ʒ��.ID = �����˲���۱�.��ƷID LEFT OUTER JOIN (" + strL + ") �����˲���۱� ON ��Ʒ��.ID = �����˲���۱�.��ƷID  LEFT OUTER JOIN (" + strA + ") ��ת��������ܱ�2 ON ��Ʒ��.ID = ��ת��������ܱ�2.��ƷID LEFT OUTER JOIN (" + strM + ") �����ֱ� ON ��Ʒ��.ID = �����ֱ�.��ƷID WHERE (��Ʒ��.beactive = 1)";

                sqlConn.Open();

                if (dSet.Tables.Contains("��ת��������ܱ�")) dSet.Tables.Remove("��ת��������ܱ�");
                sqlDA.Fill(dSet, "��ת��������ܱ�");

                //����ϼ�
                object[] rowVals = new object[37];
                decimal[] rowDTemp = new decimal[37];

                rowVals[0] = "";
                rowVals[2] = "";
                rowVals[1] = "�ϼ�";
                for (i = 0; i < dSet.Tables["��ת��������ܱ�"].Columns.Count - 1; i++)
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
                    for (j = 3; j < dSet.Tables["��ת��������ܱ�"].Columns.Count - 1; j++)
                    {
                        rowDTemp[j] += Convert.ToDecimal(dSet.Tables["��ת��������ܱ�"].Rows[i][j]);

                    }

                for (i = 3; i < dSet.Tables["��ת��������ܱ�"].Columns.Count - 1; i++)
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


        private void initdataGridViewGJWLHZ()
        {
            int i, j;
            string strA = "", strAA = "", strC = "", strD = "", strE = "";

            try
            {

                strA = "SELECT * FROM ��ת�������ܱ� WHERE (��תID = " + iJZID.ToString() + ")";

                strAA = "SELECT * FROM ��ת�������ܱ� WHERE (��תID = " + (iJZID-1).ToString() + ")";



                sqlComm.CommandText = "SELECT ��λ��.ID, ��λ��.��λ���, ��λ��.��λ����,��ת�������ܱ�2.Ӧ����� AS ����Ӧ�����, ��ת�������ܱ�2.Ӧ����� AS ����Ӧ�����, ��ת�������ܱ�1.Ӧ����� AS ����Ӧ�����, ��ת�������ܱ�1.Ӧ����� AS ����Ӧ�ս�� FROM ��λ�� LEFT OUTER JOIN (" + strA + ") ��ת�������ܱ�1 ON  ��λ��.ID = ��ת�������ܱ�1.��λID  LEFT OUTER JOIN (" + strAA + ") ��ת�������ܱ�2 ON  ��λ��.ID = ��ת�������ܱ�2.��λID WHERE (��λ��.BeActive = 1)";


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
                dataGridViewGJWLHZ.Columns[4].DefaultCellStyle.Format = "f2";
                dataGridViewGJWLHZ.Columns[6].DefaultCellStyle.Format = "f2";

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

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "��ת���ݲ�ѯ����������ܱ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewJXCHZ, strT, true, intUserLimit);
                    break;

                case 1:
                    strT = "��ת���ݲ�ѯ����Ʒ��������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJWLHZ, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "��ת���ݲ�ѯ����Ʒ��������ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXSWLHZ, strT, true, intUserLimit);
                    break;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "��ת���ݲ�ѯ����������ܱ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewJXCHZ, strT, false, intUserLimit);
                    break;

                case 1:
                    strT = "��ת���ݲ�ѯ����Ʒ��������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJWLHZ, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "��ת���ݲ�ѯ����Ʒ��������ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXSWLHZ, strT, false, intUserLimit);
                    break;
            }
        }


    }
}