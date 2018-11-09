using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormBQJYGK : Form
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
        private string SDT = "";

        private ClassGetInformation cGetInformation;

        public FormBQJYGK()
        {
            InitializeComponent();
        }

        private void FormBQJYGK_Load(object sender, EventArgs e)
        {
            decimal dTemp = 0, dTemp1 = 0,dtt=0;
            decimal dtt1=0, dtt2=0, dtt3=0;


            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            //�õ��ϴν�ת
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ����ʱ��,ID FROM ��ת���ܱ� ORDER BY ����ʱ�� DESC";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                iJZID = Convert.ToInt32(sqldr.GetValue(1).ToString());
                SDT = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
            }

            sqldr.Close();


            if (SDT == "") //û�н���
            {
                sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    SDT = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                iJZID = 0;
                sqldr.Close();
            }


            dTemp = 0;
            //sqlComm.CommandText = "SELECT SUM(��ת���) AS Expr1 FROM ��ת�ⷿ���ܱ� WHERE (��תID = " + iJZID.ToString() + ")";
            sqlComm.CommandText = "SELECT SUM(��ת���) AS Expr1 FROM ��ת��������ܱ� WHERE (��תID = " + iJZID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString()!="")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            object[] objTemp = new object[2];
            objTemp[0] = "���ڿ���ת���";
            objTemp[1] = dTemp;
            dataGridViewDJMX.Rows.Add(objTemp);

            dTemp = 0; dTemp1 = 0;
            sqlComm.CommandText = "SELECT SUM(Ӧ�����) AS Expr1, SUM(Ӧ�����) AS Expr2 FROM ��ת�������ܱ� WHERE (��תID = " + iJZID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                if (sqldr.GetValue(1).ToString() != "")
                    dTemp1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
            }
            sqldr.Close();
            objTemp[0] = "����Ӧ����ת���";
            objTemp[1] = dTemp;
            dataGridViewDJMX.Rows.Add(objTemp);

            objTemp[0] = "����Ӧ�ս�ת���";
            objTemp[1] = dTemp1;
            dataGridViewDJMX.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT SUM(���������ϸ��.ʵ�ƽ��) AS ��� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.����ID = ���������ܱ�.ID WHERE (���������ܱ�.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (���������ܱ�.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "���ڹ��������";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(�����˳���ϸ��.ʵ�ƽ��) AS ���  FROM �����˳���ϸ�� INNER JOIN �����˳����ܱ� ON �����˳���ϸ��.����ID = �����˳����ܱ�.ID WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "���ڹ����˳����";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();
            sqlComm.CommandText = "SELECT SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��),SUM(������Ʒ�Ƶ���ϸ��.ë��) AS ��� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND  (������Ʒ�Ƶ���.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                if (sqldr.GetValue(1).ToString() != "")
                    dtt = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                objTemp[0] = "�������۽��";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();
            sqlComm.CommandText = "SELECT SUM(�����˳���ϸ��.ʵ�ƽ��) AS ���  FROM �����˳���ϸ�� INNER JOIN �����˳����ܱ� ON �����˳���ϸ��.����ID = �����˳����ܱ�.ID WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "���������˳����";
                objTemp[1] = dTemp;

                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();
            decimal dt1 = 0;
            objTemp[0] = "ʵ�����۽��";
            dTemp=Convert.ToDecimal(dataGridViewDJMX.Rows[dataGridViewDJMX.Rows.Count - 2].Cells[1].Value.ToString()) - Convert.ToDecimal(dataGridViewDJMX.Rows[dataGridViewDJMX.Rows.Count - 1].Cells[1].Value.ToString());
            objTemp[1] =dTemp;
            dt1 = dTemp;

            sqlComm.CommandText = "SELECT SUM(��Ʒ��.�������*��Ʒ��.���ɱ���) AS Expr1 FROM ��Ʒ�� WHERE (beactive = 1)";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "���ڿ�����";
                objTemp[1] = dTemp;
                
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            //sqlComm.CommandText = "SELECT SUM(Ӧ���˿�) AS Expr1, SUM(Ӧ���˿�) AS Expr2 FROM ��λ�� WHERE (BeActive = 1)";
            sqlComm.CommandText = "SELECT SUM(δ������) FROM ������ϸ��ͼ WHERE (������ϸ��ͼ.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "����Ӧ�����";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(δ������) FROM �տ���ϸ��ͼ WHERE (�տ���ϸ��ͼ.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "����Ӧ�����";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();


            sqlComm.CommandText = "SELECT SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1)";
            dTemp = 0;
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) FROM �����˳���ϸ�� INNER JOIN �����˳����ܱ� ON �����˳���ϸ��.����ID = �����˳����ܱ�.ID WHERE (�����˳����ܱ�.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (�����˳����ܱ�.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp -= Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();


            objTemp[0] = "���ڳ���ɱ�";
            objTemp[1] = dTemp;
            decimal dt2 = dTemp;
            dataGridViewDJMX.Rows.Add(objTemp);


            sqlComm.CommandText = "SELECT SUM(��汨����ϸ��.������) FROM ��汨����ϸ�� INNER JOIN ��汨����ܱ� ON ��汨����ϸ��.����ID = ��汨����ܱ�.ID WHERE (��汨����ܱ�.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (��汨����ܱ�.BeActive = 1)";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "���ڱ�����";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();


            sqlComm.CommandText = "SELECT SUM(���������ܱ�.������) AS Expr1 FROM ���������ܱ� WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (���������ܱ�.������ > 0) AND ((���������ܱ�.��ֵ���ID IS NULL) OR (���������ܱ�.��ֵ���ID <> -1))";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "���ڽ��������";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();



            sqlComm.CommandText = "SELECT SUM(���������ܱ�.������) AS Expr1 FROM ���������ܱ� WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (���������ܱ�.������ <= 0) AND ((���������ܱ�.��ֵ���ID IS NULL) OR (���������ܱ�.��ֵ���ID <> -1))";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "���ڽ��������";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(���������ܱ�.������) AS Expr1 FROM ���������ܱ� WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (���������ܱ�.��ֵ���ID = -1)";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "���ڽ����ֽ��";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();
           
           

            objTemp[0] = "��������ë��";
            //objTemp[1] = dt1-dt2;
            objTemp[1] = dtt;
            dataGridViewDJMX.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT SUM(�����˲���ۻ��ܱ�.��˰�ϼ�) AS Expr1 FROM �����˲���ۻ��ܱ� WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) ";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "���ڹ����˲����";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(�����˲���ۻ��ܱ�.��˰�ϼ�) AS Expr1 FROM �����˲���ۻ��ܱ� WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102))";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dtt1 = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dtt1 = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "���������˲����";
                objTemp[1] = dtt1;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(�����˳���ϸ��.ʵ�ƽ��-�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) FROM �����˳���ϸ�� INNER JOIN �����˳����ܱ� ON �����˳���ϸ��.����ID = �����˳����ܱ�.ID WHERE (�����˳����ܱ�.���� > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (�����˳����ܱ�.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dtt2 = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dtt2 = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "���������˳�ë��";
                objTemp[1] = dtt2;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            dtt3 = dtt + dtt1 - dtt2;
            objTemp[0] = "����ë��";
            objTemp[1] = dtt3;
            dataGridViewDJMX.Rows.Add(objTemp);


            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].DefaultCellStyle.Format = "f2";
            sqlConn.Close();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;
            if (SDT == "")
                labelJZRQ.Text = "";
            else
                labelJZRQ.Text = Convert.ToDateTime(SDT).ToString("yyyy��M��dd��");

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "���ھ�Ӫ�ſ�;��ǰ���ڣ�" + labelZDRQ.Text ;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "���ھ�Ӫ�ſ�;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}