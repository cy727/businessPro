using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class Form1 : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlCommand sqlComm1 = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            sqlConn.ConnectionString = "workstation id=CY;packet size=4096;user id=sa;password=biadcoop;data source=\"172.16.5.183\";;initial catalog=bbb";
            sqlComm.Connection = sqlConn;
            sqlComm1.Connection = sqlConn;

            sqlDA.SelectCommand = sqlComm;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i,j;
            string st,stt,stt1;
            string st1, st2;
            sqlConn.Open();

            sqlComm.CommandText = "UPDATE ��Ʒ�� SET Ӧ����� = 0, �Ѹ���� = 0, Ӧ�ս�� = 0, ���ս�� = 0, ������� = 0";
            sqlComm.ExecuteNonQuery();


            sqlComm.CommandText = "UPDATE ���� SET ������� = 0, Ӧ����� = 0, �Ѹ���� = 0, Ӧ�ս�� = 0, ���ս�� = 0";
            sqlComm.ExecuteNonQuery();


            sqlComm.CommandText = "SELECT ��Ʒ��.ID, bbb.name, bbb.num, bbb.cb FROM bbb INNER JOIN ��Ʒ�� ON bbb.name = ��Ʒ��.��Ʒ����";
            if (dSet.Tables.Contains("temp")) dSet.Tables.Remove("temp");
            sqlDA.Fill(dSet, "temp");

            for (i = 0; i < dSet.Tables["temp"].Rows.Count; i++)
            {
                sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dSet.Tables["temp"].Rows[i][2].ToString() + ", ���ɱ���= " + dSet.Tables["temp"].Rows[i][3].ToString() + " WHERE (ID = " + dSet.Tables["temp"].Rows[i][0].ToString() + ")";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE ���� SET ������� = " + dSet.Tables["temp"].Rows[i][2].ToString() + ", ���ɱ���= " + dSet.Tables["temp"].Rows[i][3].ToString() + " WHERE (��ƷID = " + dSet.Tables["temp"].Rows[i][0].ToString() + ")";
                sqlComm.ExecuteNonQuery();





            }

            sqlComm.CommandText = "UPDATE ��Ʒ�� SET ����� = ������� * ���ɱ���";
            sqlComm.ExecuteNonQuery();

            sqlComm.CommandText = "UPDATE ���� SET ����� = ������� * ���ɱ���";
            sqlComm.ExecuteNonQuery();

            MessageBox.Show("����");



                sqlConn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT JG.ID, ��Ʒ�����.ID AS ����ID FROM JG INNER JOIN ��Ʒ����� ON JG.�������� = ��Ʒ�����.��������";
            if (dSet.Tables.Contains("��Ʒ�����")) dSet.Tables.Remove("��Ʒ�����");
            sqlDA.Fill(dSet, "��Ʒ�����");

            int i;

            for (i = 0; i < dSet.Tables["��Ʒ�����"].Rows.Count; i++)
            {
                sqlComm.CommandText = "UPDATE JG SET ������ = N'" + dSet.Tables["��Ʒ�����"].Rows[i][1].ToString() + "' WHERE (ID = " + dSet.Tables["��Ʒ�����"].Rows[i][0].ToString() + ")";
                sqlComm.ExecuteNonQuery();

            }

            sqlComm.CommandText = "UPDATE JG SET ������� = 0 WHERE (������� IS NULL)";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "UPDATE JG SET ���ɱ��� = 0 WHERE (���ɱ��� IS NULL)";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "UPDATE JG SET ��߽��� = 0 WHERE (��߽��� IS NULL)";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "UPDATE JG SET ��ͽ��� = 0 WHERE (��ͽ��� IS NULL)";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "UPDATE JG SET ���ս��� = 0 WHERE (���ս��� IS NULL)";
            sqlComm.ExecuteNonQuery();

            sqlComm.CommandText = "SELECT COUNT(*) AS Expr1 FROM JG WHERE (������ = N'')";
            sqldr = sqlComm.ExecuteReader();
            sqldr.Read();



            MessageBox.Show(sqldr.GetValue(0).ToString());

            sqlConn.Close();



        }

        private void button3_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            sqlComm.CommandText = "SELECT ��Ʒ���, ��Ʒ����, ������, �������, ���ɱ���, ��߽���, ��ͽ���, ���ս���, ������ FROM JG";
            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");


            sqlComm.CommandText = "DELETE FROM ��Ʒ��";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "DELETE FROM ����";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "dbcc checkident(����,reseed,0)";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "dbcc checkident(��Ʒ��,reseed,0)";
            sqlComm.ExecuteNonQuery();

            int i, iSelect;

            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                sqlComm.CommandText = "INSERT INTO ��Ʒ�� (��Ʒ���, ��Ʒ����, ������, �������, ���ɱ���, ��߽���, ��ͽ���, ���ս���, ������, beactive) VALUES (N'" + dSet.Tables["��Ʒ��"].Rows[i][0].ToString() + "', N'" + dSet.Tables["��Ʒ��"].Rows[i][1].ToString() + "', N'" + dSet.Tables["��Ʒ��"].Rows[i][2].ToString() + "', " + dSet.Tables["��Ʒ��"].Rows[i][3].ToString() + ", " + dSet.Tables["��Ʒ��"].Rows[i][4].ToString() + ", " + dSet.Tables["��Ʒ��"].Rows[i][5].ToString() + ", " + dSet.Tables["��Ʒ��"].Rows[i][6].ToString() + ", " + dSet.Tables["��Ʒ��"].Rows[i][7].ToString() + ", " + dSet.Tables["��Ʒ��"].Rows[i][8].ToString() + ", 1)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                iSelect = Convert.ToInt32(sqldr.GetValue(0).ToString());
                sqldr.Close();

                //���ӿ��
                sqlComm.CommandText = "SELECT �ⷿID FROM ��Ʒ����� WHERE (ID = " + dSet.Tables["��Ʒ��"].Rows[i][8].ToString() + ")";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sKF = sqldr.GetValue(0).ToString();
                sqldr.Close();

                if (sKF != "")
                {
                    sqlComm.CommandText = "INSERT INTO ���� (�ⷿID, ��ƷID, �������, �����, ���ɱ���, ����ɱ���, �������, �������, ����������, ����������, Ӧ�����, �Ѹ����, Ӧ�ս��, ���ս��, BeActive) VALUES (" + sKF + ", " + iSelect.ToString() + ", " + dSet.Tables["��Ʒ��"].Rows[i][3].ToString() + ", 0, "+dSet.Tables["��Ʒ��"].Rows[i][4].ToString()+", 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)";
                    sqlComm.ExecuteNonQuery();
                }

            }

            sqlComm.CommandText = "UPDATE ��Ʒ�� SET ����� = ������� * ���ɱ���";
            sqlComm.ExecuteNonQuery();

            sqlComm.CommandText = "UPDATE ���� SET ����� = ������� * ���ɱ���";
            sqlComm.ExecuteNonQuery();

            sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������λ = N'0', ������� = 1, ��С������λ = N'��', ����˰�� = 0, ���ۼ� = 0, ���� = ���ս���, ��˰���� = ���ս���, ������ = ���ս���, ��˰������ = ���ս���, ������ = 0, ����ɱ��� = ���ɱ���, ��ת���� = 0, ��ת���� = 0, ��ת��� = 0, ��ת���� = 0, ��¼���� = '2006-08-09', ��ɱ��� = 0, ���������� = 0, �����ڷ�ʽ = N'û��', ������� = 0, ������� = 0, ���������� = 0, ���������� = 0, ���� = N'����', Ԥ������ = 0, �Ƿ��ؼ���Ʒ = 0, �Ƿ��Ա��Ʒ = 0, ��Ա�ؼ� = 0, ���۷�ʽ = 1, �޶������� = 0, ��װ���� = 1, ��װ��Ʒ = 0, Ӧ����� = 0, �Ѹ���� = 0, Ӧ�ս�� = 0, ���ս�� = 0";
            sqlComm.ExecuteNonQuery();


            MessageBox.Show("OVER");

            sqlConn.Close();
        }

   }
}