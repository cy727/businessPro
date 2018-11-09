using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPDPFX : Form
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

        public int intCommID = 0;

        private ClassGetInformation cGetInformation;

        private decimal[] cTemp = new decimal[14] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0,0};
        private decimal[] cTemp1 = new decimal[14] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0, 0, 0, 0};

        public FormSPDPFX()
        {
            InitializeComponent();
        }

        private void FormSPDPFX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

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
            /*
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            sqlConn.Close();
             */

            if (intCommID != 0) //���ڳ�ʼ��Ʒ
            {
                if (cGetInformation.getCommInformation(40, intCommID.ToString()) == 0) //ʧ��
                {
                    intCommID = 0;
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                    toolStripButtonGD_Click(null, null);

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
                toolStripButtonGD_Click(null, null);


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
                    toolStripButtonGD_Click(null, null);

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
                    toolStripButtonGD_Click(null, null);

                }

            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {

            //�������
            if (intCommID == 0)
            {
                MessageBox.Show("��ѡ��Ҫ��ѯ����Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ������, ������λ, �������, ��С������λ, ����, ������, ��¼���� FROM ��Ʒ�� WHERE (ID = "+intCommID.ToString()+")";
            sqldr = sqlComm.ExecuteReader();
            while(sqldr.Read())
            {
                labelZJM.Text = sqldr.GetValue(0).ToString();
                labelJLDW.Text = sqldr.GetValue(1).ToString();
                labelJLGG.Text = sqldr.GetValue(2).ToString();
                labelZXJLDW.Text = sqldr.GetValue(3).ToString();
                labelJJ.Text = sqldr.GetValue(4).ToString();
                labelPFJ.Text = sqldr.GetValue(5).ToString();
                if (sqldr.GetValue(6).ToString()!="")
                    labelDLRQ.Text = Convert.ToDateTime(sqldr.GetValue(6).ToString()).ToString("yyyy��M��dd��");
                else
                    labelDLRQ.Text = sqldr.GetValue(6).ToString();
            }
            sqldr.Close();
            sqlConn.Close();

            initGJ();
            initXS();
            iniKC();
            iniDataView();
            tabControl1_SelectedIndexChanged(null, null);

        }

        private void iniKC()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT �������, ���ɱ���, ����� FROM ��Ʒ�� WHERE (ID = "+intCommID.ToString()+")";
            sqldr = sqlComm.ExecuteReader();
            labelZKCSL.Text = "";
            labelZKCCBJ.Text = "";
            labelZKCJE.Text = "";
            while (sqldr.Read())
            {
                labelZKCSL.Text = sqldr.GetValue(0).ToString() ;
                labelZKCCBJ.Text = sqldr.GetValue(1).ToString();
                labelZKCJE.Text = sqldr.GetValue(2).ToString();
            }
            if (labelZKCSL.Text == "")
                labelZKCSL.Text = "0";
            if (labelZKCCBJ.Text == "")
                labelZKCCBJ.Text = "0";
            if (labelZKCJE.Text == "")
                labelZKCJE.Text = "0";

            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(��������) AS Expr1, SUM(������) AS Expr2 FROM ����̵���ϸ�� WHERE (��ƷID =  " + intCommID.ToString() + ") ";
            sqldr = sqlComm.ExecuteReader();
            labelPSSL.Text = "";
            labelPSJE.Text = "";
            while (sqldr.Read())
            {
                labelPSSL.Text = sqldr.GetValue(0).ToString();
                labelPSJE.Text = sqldr.GetValue(1).ToString();
            }
            if (labelPSSL.Text == "")
                labelPSSL.Text = "0";
            if (labelPSJE.Text == "")
                labelPSJE.Text = "0";
            sqldr.Close();


            sqlComm.CommandText = "SELECT SUM(��������) AS Expr1, SUM(������) AS Expr2 FROM ��汨����ϸ�� WHERE (��ƷID = " + intCommID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            labelBSSL.Text = "";
            labelBSJE.Text = "";
            while (sqldr.Read())
            {
                labelBSSL.Text = sqldr.GetValue(0).ToString();
                labelBSJE.Text = sqldr.GetValue(1).ToString();
            }
            if (labelBSSL.Text == "")
                labelBSSL.Text = "0";
            if (labelBSJE.Text == "")
                labelBSJE.Text = "0";
            sqldr.Close();

            sqlConn.Close();
        }

        private void iniDataView()
        {
            int i, j;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ����.�������, ����.����� FROM ���� INNER JOIN �ⷿ�� ON ����.�ⷿID = �ⷿ��.ID WHERE (����.��ƷID = "+intCommID.ToString()+")";


            if (dSet.Tables.Contains("����")) dSet.Tables.Remove("����");
            sqlDA.Fill(dSet, "����");
            dataGridViewKCFB.DataSource = dSet.Tables["����"];

            cTemp[3] = 0; cTemp1[3] = 0;
            for (i = 0; i <= dSet.Tables["����"].Rows.Count; i++)
            {
                try
                {
                    cTemp[3] += decimal.Parse(dSet.Tables["����"].Rows[i][2].ToString());
                    cTemp1[3] += decimal.Parse(dSet.Tables["����"].Rows[i][3].ToString());
                }
                catch
                {
                }
            }



            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.��� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID WHERE (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY ������Ʒ�Ƶ���.���� DESC";


            if (dSet.Tables.Contains("������Ʒ�Ƶ���")) dSet.Tables.Remove("������Ʒ�Ƶ���");
            sqlDA.Fill(dSet, "������Ʒ�Ƶ���");
            dataGridViewGJMX.DataSource = dSet.Tables["������Ʒ�Ƶ���"];
            dataGridViewGJMX.Columns[0].Visible = false;
            dataGridViewGJMX.Columns[7].DefaultCellStyle.Format = "f0";

            cTemp[4] = 0; cTemp1[4] = 0;
            for (i = 0; i <= dSet.Tables["������Ʒ�Ƶ���"].Rows.Count; i++)
            {
                try
                {
                    cTemp[4] += decimal.Parse(dSet.Tables["������Ʒ�Ƶ���"].Rows[i][7].ToString());
                    cTemp1[4] += decimal.Parse(dSet.Tables["������Ʒ�Ƶ���"].Rows[i][9].ToString());
                }
                catch
                {
                }
            }


            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ�� FROM ��λ�� INNER JOIN �ⷿ�� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON �ⷿ��.ID = ������Ʒ�Ƶ���ϸ��.�ⷿID INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID ON  ��λ��.ID = ������Ʒ�Ƶ���.��λID WHERE (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY ������Ʒ�Ƶ���.���� DESC";


            if (dSet.Tables.Contains("������Ʒ�Ƶ���")) dSet.Tables.Remove("������Ʒ�Ƶ���");
            sqlDA.Fill(dSet, "������Ʒ�Ƶ���");
            dataGridViewXSMX.DataSource = dSet.Tables["������Ʒ�Ƶ���"];
            dataGridViewXSMX.Columns[0].Visible = false;
            dataGridViewXSMX.Columns[7].DefaultCellStyle.Format = "f0";

            cTemp[5] = 0; cTemp1[5] = 0;
            for (i = 0; i <= dSet.Tables["������Ʒ�Ƶ���"].Rows.Count; i++)
            {
                try
                {
                    cTemp[5] += decimal.Parse(dSet.Tables["������Ʒ�Ƶ���"].Rows[i][7].ToString());
                    cTemp1[5] += decimal.Parse(dSet.Tables["������Ʒ�Ƶ���"].Rows[i][11].ToString());
                }
                catch
                {
                }
            }

            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, [ְԱ��_1].ְԱ���� AS ����Ա, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.ë�� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ������Ʒ�Ƶ���.����ԱID = [ְԱ��_1].ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID WHERE (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���ϸ��.ë�� <= 0) ORDER BY ������Ʒ�Ƶ���.���� DESC";


            if (dSet.Tables.Contains("��Ʒ��ʷ�˱�")) dSet.Tables.Remove("��Ʒ��ʷ�˱�");
            sqlDA.Fill(dSet, "��Ʒ��ʷ�˱�");
            for (i = 0; i < dSet.Tables["��Ʒ��ʷ�˱�"].Rows.Count; i++)
                for (j = 3; j < dSet.Tables["��Ʒ��ʷ�˱�"].Columns.Count; j++)
                {
                    if (dSet.Tables["��Ʒ��ʷ�˱�"].Rows[i][j].ToString() == "")
                        dSet.Tables["��Ʒ��ʷ�˱�"].Rows[i][j] = 0;
                }
            dataGridViewCRK.DataSource = dSet.Tables["��Ʒ��ʷ�˱�"];
            dataGridViewCRK.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewCRK.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewCRK.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewCRK.Columns[0].Visible = false;
        


            sqlComm.CommandText = "SELECT �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ����.�������, ����.�������, ����.����������, ����.����������, ����.������� FROM ���� INNER JOIN �ⷿ�� ON ����.�ⷿID = �ⷿ��.ID WHERE (����.��ƷID = "+intCommID.ToString()+")";


            if (dSet.Tables.Contains("�ⷿ��")) dSet.Tables.Remove("�ⷿ��");
            sqlDA.Fill(dSet, "�ⷿ��");
            for (i = 0; i < dSet.Tables["�ⷿ��"].Rows.Count; i++)
                for (j = 2; j < dSet.Tables["�ⷿ��"].Columns.Count; j++)
                {
                    if (dSet.Tables["�ⷿ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["�ⷿ��"].Rows[i][j] = 0;
                }
            dataGridViewCHFX.DataSource = dSet.Tables["�ⷿ��"];
            dataGridViewCHFX.Columns[2].DefaultCellStyle.Format = "f0";
            dataGridViewCHFX.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridViewCHFX.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridViewCHFX.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridViewCHFX.Columns[6].DefaultCellStyle.Format = "f0";


            cTemp[7] = 0; cTemp1[7] = 0;
            for (i = 0; i <= dSet.Tables["�ⷿ��"].Rows.Count; i++)
            {
                try
                {
                    cTemp[7] += decimal.Parse(dSet.Tables["�ⷿ��"].Rows[i][2].ToString());
                }
                catch
                {
                }
            }


            sqlComm.CommandText = "SELECT ���������ϸ��.��ID, ���������ܱ�.���ݱ��, ���������ܱ�.����,��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ���������ϸ��.����, ���������ϸ��.����, ���������ϸ��.��� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID WHERE (���������ϸ��.��ƷID = " + intCommID.ToString() + ") AND (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.��ֵ���ID IS NULL) ORDER BY ���������ܱ�.���� DESC";


            if (dSet.Tables.Contains("���������ܱ�")) dSet.Tables.Remove("���������ܱ�");
            sqlDA.Fill(dSet, "���������ܱ�");
            dataGridViewJW.DataSource = dSet.Tables["���������ܱ�"];
            dataGridViewJW.Columns[0].Visible = false;
            dataGridViewJW.Columns[7].DefaultCellStyle.Format = "f0";

            cTemp[8] = 0; cTemp1[8] = 0;
            for (i = 0; i <= dSet.Tables["���������ܱ�"].Rows.Count; i++)
            {
                try
                {
                    cTemp[8] += decimal.Parse(dSet.Tables["���������ܱ�"].Rows[i][7].ToString());
                }
                catch
                {
                }
            }

            sqlComm.CommandText = "SELECT ���������ϸ��.��ID, ���������ܱ�.���ݱ��, ���������ܱ�.����,��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ���������ϸ��.����, ���������ϸ��.����, ���������ϸ��.��� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID WHERE (���������ϸ��.��ƷID = " + intCommID.ToString() + ") AND (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.��ֵ���ID = -1) ORDER BY ���������ܱ�.���� DESC";


            if (dSet.Tables.Contains("���������ܱ�1")) dSet.Tables.Remove("���������ܱ�1");
            sqlDA.Fill(dSet, "���������ܱ�1");
            dataGridViewJW1.DataSource = dSet.Tables["���������ܱ�1"];
            dataGridViewJW1.Columns[0].Visible = false;
            dataGridViewJW1.Columns[7].DefaultCellStyle.Format = "f0";

            cTemp[9] = 0; cTemp1[9] = 0;
            for (i = 0; i <= dSet.Tables["���������ܱ�1"].Rows.Count; i++)
            {
                try
                {
                    cTemp[9] += decimal.Parse(dSet.Tables["���������ܱ�1"].Rows[i][7].ToString());
                }
                catch
                {
                }
            }

            sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����˳���ϸ��.����, �����˳���ϸ��.����, �����˳���ϸ��.��� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN �ⷿ�� ON �����˳���ϸ��.�ⷿID = �ⷿ��.ID WHERE (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY �����˳����ܱ�.���� DESC";


            if (dSet.Tables.Contains("�����˳����ܱ�")) dSet.Tables.Remove("�����˳����ܱ�");
            sqlDA.Fill(dSet, "�����˳����ܱ�");
            dataGridViewGJTC.DataSource = dSet.Tables["�����˳����ܱ�"];
            dataGridViewGJTC.Columns[0].Visible = false;
            dataGridViewGJTC.Columns[7].DefaultCellStyle.Format = "f0";

            cTemp[10] = 0; cTemp1[10] = 0;
            for (i = 0; i <= dSet.Tables["�����˳����ܱ�"].Rows.Count; i++)
            {
                try
                {
                    cTemp[10] += decimal.Parse(dSet.Tables["�����˳����ܱ�"].Rows[i][7].ToString());
                    cTemp1[10] += decimal.Parse(dSet.Tables["�����˳����ܱ�"].Rows[i][9].ToString());
                }
                catch
                {
                }
            }

            sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����˳���ϸ��.����, �����˳���ϸ��.����, �����˳���ϸ��.��� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN �ⷿ�� ON �����˳���ϸ��.�ⷿID = �ⷿ��.ID WHERE (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY �����˳����ܱ�.���� DESC";


            if (dSet.Tables.Contains("�����˳����ܱ�")) dSet.Tables.Remove("�����˳����ܱ�");
            sqlDA.Fill(dSet, "�����˳����ܱ�");
            dataGridViewXSTH.DataSource = dSet.Tables["�����˳����ܱ�"];
            dataGridViewXSTH.Columns[0].Visible = false;
            dataGridViewXSTH.Columns[7].DefaultCellStyle.Format = "f0";

            cTemp[11] = 0; cTemp1[11] = 0;
            for (i = 0; i <= dSet.Tables["�����˳����ܱ�"].Rows.Count; i++)
            {
                try
                {
                    cTemp[11] += decimal.Parse(dSet.Tables["�����˳����ܱ�"].Rows[i][7].ToString());
                    cTemp1[11] += decimal.Parse(dSet.Tables["�����˳����ܱ�"].Rows[i][9].ToString());
                }
                catch
                {
                }
            }
            sqlComm.CommandText = "SELECT �����˲���ۻ��ܱ�.ID, �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����˲������ϸ��.��������, �����˲������ϸ��.���, �����˲������ϸ��.��� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID INNER JOIN �ⷿ�� ON �����˲������ϸ��.�ⷿID = �ⷿ��.ID WHERE (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") AND (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY �����˲���ۻ��ܱ�.���� DESC";


            if (dSet.Tables.Contains("�����˲���ۻ��ܱ�")) dSet.Tables.Remove("�����˲���ۻ��ܱ�");
            sqlDA.Fill(dSet, "�����˲���ۻ��ܱ�");
            dataGridViewGJTBJ.DataSource = dSet.Tables["�����˲���ۻ��ܱ�"];
            dataGridViewGJTBJ.Columns[0].Visible = false;
            dataGridViewGJTBJ.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewGJTBJ.Columns[8].DefaultCellStyle.Format = "f2";

            cTemp[12] = 0; cTemp1[12] = 0;
            for (i = 0; i <= dSet.Tables["�����˲���ۻ��ܱ�"].Rows.Count; i++)
            {
                try
                {
                    cTemp[12] += decimal.Parse(dSet.Tables["�����˲���ۻ��ܱ�"].Rows[i][7].ToString());
                    cTemp1[12] += decimal.Parse(dSet.Tables["�����˲���ۻ��ܱ�"].Rows[i][9].ToString());
                }
                catch
                {
                }
            }

            sqlComm.CommandText = "SELECT �����˲���ۻ��ܱ�.ID, �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����˲������ϸ��.��������, �����˲������ϸ��.���, �����˲������ϸ��.��� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID INNER JOIN �ⷿ�� ON �����˲������ϸ��.�ⷿID = �ⷿ��.ID WHERE (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") AND (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY �����˲���ۻ��ܱ�.���� DESC";


            if (dSet.Tables.Contains("�����˲���ۻ��ܱ�")) dSet.Tables.Remove("�����˲���ۻ��ܱ�");
            sqlDA.Fill(dSet, "�����˲���ۻ��ܱ�");
            dataGridViewXSTBJ.DataSource = dSet.Tables["�����˲���ۻ��ܱ�"];
            dataGridViewXSTBJ.Columns[0].Visible = false;
            dataGridViewXSTBJ.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewXSTBJ.Columns[8].DefaultCellStyle.Format = "f2";

            cTemp[13] = 0; cTemp1[13] = 0;
            for (i = 0; i <= dSet.Tables["�����˲���ۻ��ܱ�"].Rows.Count; i++)
            {
                try
                {
                    cTemp[13] += decimal.Parse(dSet.Tables["�����˲���ۻ��ܱ�"].Rows[i][7].ToString());
                    cTemp1[12] += decimal.Parse(dSet.Tables["�����˲���ۻ��ܱ�"].Rows[i][9].ToString());
                }
                catch
                {
                }
            }

            sqlConn.Close();
        }
        private void iniCHFX()
        {
        }


        private void initGJ()
        {
            int i, j;
            string dTemp = "0", dTemp1 = "0",dTemp2 = "0";
            decimal dt1 = 0, dt2 = 0;

            dataGridViewGJ.Rows.Clear();


            sqlConn.Open();
            sqlComm.CommandText = "SELECT SUM(������Ʒ�Ƶ���ϸ��.����), SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(��������) AS ��������, SUM(�������) AS ������� FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%AKP%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp=sqldr.GetValue(0).ToString();
                dTemp1=sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";

            sqldr.Close();

            object[] objTemp = new object[3];
            objTemp[0] = "����";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");

            dataGridViewGJ.Rows.Add(objTemp);


            sqlComm.CommandText = "SELECT SUM(���������ϸ��.����), SUM(���������ϸ��.���) FROM ���������ܱ� INNER JOIN ���������ϸ�� ON ���������ܱ�.ID = ���������ϸ��.����ID WHERE (���������ϸ��.��ƷID = " + intCommID.ToString() + ") AND (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(�������), SUM(�����) FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%ADH%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            objTemp[0] = "����";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewGJ.Rows.Add(objTemp);


            sqlComm.CommandText = "SELECT SUM(����) , SUM(�Ѹ�����), SUM(δ������) FROM  ������ϸ��ͼ WHERE (��ƷID = " + intCommID.ToString() + ") AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(��������) , SUM(������) FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%AYF%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
                dTemp2 = sqldr.GetValue(2).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            if (dTemp2 == "")
                dTemp2 = "0";
            sqldr.Close();
            objTemp[0] = "����";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewGJ.Rows.Add(objTemp);

            dt1 = Convert.ToDecimal(dataGridViewGJ.Rows[0].Cells[1].Value.ToString()) - Convert.ToDecimal(dataGridViewGJ.Rows[2].Cells[1].Value.ToString());
            dt2 = Convert.ToDecimal(dataGridViewGJ.Rows[0].Cells[2].Value.ToString()) - Convert.ToDecimal(dataGridViewGJ.Rows[2].Cells[2].Value.ToString());
            objTemp[0] = "��ǰӦ��";
            objTemp[1] = "";
            objTemp[2] = decimal.Parse(dTemp2).ToString("f2");
            dataGridViewGJ.Rows.Add(objTemp);


            sqlComm.CommandText = "SELECT SUM(�����˳���ϸ��.����), SUM(�����˳���ϸ��.ʵ�ƽ��) FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(�˳�����) , SUM(�˳����) FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%ATH%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            objTemp[0] = "�˳�";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewGJ.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT SUM(�����˲������ϸ��.��������), SUM(�����˲������ϸ��.���) FROM �����˲������ϸ�� INNER JOIN �����˲���ۻ��ܱ� ON �����˲������ϸ��.����ID = �����˲���ۻ��ܱ�.ID WHERE (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") AND (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(�˳�����) , SUM(�˳����) FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%ATH%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            objTemp[0] = "�˲���";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewGJ.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT COUNT(*) FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            //sqlComm.CommandText = "SELECT COUNT(*) AS Expr1 FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%AKP%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            sqldr.Close();
            labelGJPC.Text = dTemp;


            sqlComm.CommandText = "SELECT  COUNT(*) FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            //sqlComm.CommandText = "SELECT COUNT(*) AS Expr1 FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%ATH%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            sqldr.Close();
            labelTUPC.Text = dTemp;


            sqlComm.CommandText = "SELECT MIN(������Ʒ�Ƶ���ϸ��.����), MAX(������Ʒ�Ƶ���ϸ��.����) FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            //sqlComm.CommandText = "SELECT MIN(��������) AS Expr1, MAX(��������) AS Expr2 FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%AKP%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            labelZDJJ.Text = dTemp;
            labelZGJJ.Text = dTemp1;

            dt1 = Convert.ToDecimal(dataGridViewGJ.Rows[0].Cells[2].Value.ToString());
            dt2 = Convert.ToDecimal(dataGridViewGJ.Rows[0].Cells[1].Value.ToString());
            if (dt2 == 0)
                dt1 = 0;
            else
                dt1 = dt1 / dt2;

            labelPJGJDJ.Text = dt1.ToString("f2");

            dt2 = Convert.ToDecimal(labelGJPC.Text);
            dt1 = Convert.ToDecimal(labelTUPC.Text);
            if (dt2 == 0)
                dt1 = 0;
            else
                dt1 = dt1 / dt2*100;
            labelGJTCL.Text = dt1.ToString();

            sqlConn.Close();

            dataGridViewGJ.Columns[1].DefaultCellStyle.Format = "f0";
            dataGridViewGJ.Columns[2].DefaultCellStyle.Format = "f2";
        }

        private void initXS()
        {
            int i, j;
            string dTemp = "0", dTemp1 = "0",dTemp2="0";
            decimal dt1 = 0, dt2 = 0;

            dataGridViewXS.Rows.Clear();


            sqlConn.Open();
            //sqlComm.CommandText = "SELECT SUM(��������) , SUM(������) FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%BKP%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqlComm.CommandText = "SELECT SUM(������Ʒ�Ƶ���ϸ��.����) AS Expr1, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS Expr2 FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID WHERE (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";

            sqldr.Close();

            object[] objTemp = new object[3];
            objTemp[0] = "����";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");

            dataGridViewXS.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT SUM(����) AS Expr1, SUM(ʵ�ƽ��) AS Expr2 FROM ������ͼ    WHERE (��ƷID = " + intCommID.ToString() + ")AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";

            sqldr.Close();

            objTemp[0] = "����";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewXS.Rows.Add(objTemp);


            sqlComm.CommandText = "SELECT SUM(����) , SUM(�Ѹ�����),  SUM(δ������) FROM  �տ���ϸ��ͼ WHERE (��ƷID = " + intCommID.ToString() + ") AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(��������) , SUM(������) FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%BYS%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
                dTemp2 = sqldr.GetValue(2).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            if (dTemp2 == "")
                dTemp2 = "0";
            sqldr.Close();
            objTemp[0] = "����";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewXS.Rows.Add(objTemp);

            dt1 = Convert.ToDecimal(dataGridViewXS.Rows[0].Cells[1].Value.ToString()) - Convert.ToDecimal(dataGridViewXS.Rows[2].Cells[1].Value.ToString());
            dt2 = Convert.ToDecimal(dataGridViewXS.Rows[0].Cells[2].Value.ToString()) - Convert.ToDecimal(dataGridViewXS.Rows[2].Cells[2].Value.ToString());
            objTemp[0] = "��ǰӦ��";
            objTemp[1] = "";
            objTemp[2] = decimal.Parse(dTemp2).ToString("f2"); 
            dataGridViewXS.Rows.Add(objTemp);


            sqlComm.CommandText = "SELECT SUM(�����˳���ϸ��.����), SUM(�����˳���ϸ��.ʵ�ƽ��) FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            //sqlComm.CommandText = "SELECT SUM(�˻�����) , SUM(�˻ؽ��) FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%BTH%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            objTemp[0] = "�˻�";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewXS.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT SUM(�����˲������ϸ��.��������), SUM(�����˲������ϸ��.���) FROM �����˲������ϸ�� INNER JOIN �����˲���ۻ��ܱ� ON �����˲������ϸ��.����ID = �����˲���ۻ��ܱ�.ID WHERE (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") AND (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(�˳�����) , SUM(�˳����) FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%ATH%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            objTemp[0] = "�˲���";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewXS.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT COUNT(*) FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID WHERE (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT COUNT(*) AS Expr1 FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%BKP%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            sqldr.Close();
            labelXSPC.Text = dTemp;

            sqlComm.CommandText = "SELECT COUNT(*) FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            //sqlComm.CommandText = "SELECT COUNT(*) AS Expr1 FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%BTH%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            sqldr.Close();
            labelTHPC.Text = dTemp;


            sqlComm.CommandText = "SELECT MIN(������Ʒ�Ƶ���ϸ��.����) AS Expr1, MAX(������Ʒ�Ƶ���ϸ��.����) AS Expr2 FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID WHERE (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT MIN(���ⵥ��) AS Expr1, MAX(���ⵥ��) AS Expr2 FROM ��Ʒ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (���ݱ�� LIKE N'%BKP%')  AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            labelZGSJ.Text = dTemp1;
            labelZDSJ.Text = dTemp;

            dt1 = Convert.ToDecimal(dataGridViewXS.Rows[0].Cells[2].Value.ToString());
            dt2 = Convert.ToDecimal(dataGridViewXS.Rows[0].Cells[1].Value.ToString());
            if (dt2 == 0)
                dt1 = 0;
            else
                dt1 = dt1 / dt2;

            labelPJXSDJ.Text = dt1.ToString("f2");

            dt2 = Convert.ToDecimal(labelXSPC.Text);
            dt1 = Convert.ToDecimal(labelTUPC.Text);
            if (dt2 == 0)
                dt1 = 0;
            else
                dt1 = dt1 / dt2 * 100;
            labelXSTHL.Text = dt1.ToString();

            sqlConn.Close();
            dataGridViewXS.Columns[1].DefaultCellStyle.Format = "f0";
            dataGridViewXS.Columns[2].DefaultCellStyle.Format = "f2";

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "��Ʒ��Ʒ������������;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJ, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "��Ʒ��Ʒ���������ۣ�;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXS, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "��Ʒ��Ʒ���������ֲ���;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewKCFB, strT, true, intUserLimit);
                    break;
                case 4:
                    strT = "��Ʒ��Ʒ������������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJMX, strT, true, intUserLimit);
                    break;
                case 5:
                    strT = "��Ʒ��Ʒ������������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXSMX, strT, true, intUserLimit);
                    break;
                case 6:
                    strT = "��Ʒ��Ʒ���������ڳɱ����ۣ�;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewCRK, strT, true, intUserLimit);
                    break;
                case 7:
                    strT = "��Ʒ��Ʒ���������������;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewCHFX, strT, true, intUserLimit);
                    break;
                case 8:
                    strT = "��Ʒ��Ʒ���������������;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewJW, strT, true, intUserLimit);
                    break;
                case 9:
                    strT = "��Ʒ��Ʒ�����������˻ط�����;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJTC, strT, true, intUserLimit);
                    break;
                case 10:
                    strT = "��Ʒ��Ʒ�����������˻ط�����;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXSTH, strT, true, intUserLimit);
                    break;
            }

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "��Ʒ��Ʒ������������;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJ, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "��Ʒ��Ʒ���������ۣ�;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXS, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "��Ʒ��Ʒ���������ֲ���;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewKCFB, strT, false, intUserLimit);
                    break;
                case 4:
                    strT = "��Ʒ��Ʒ������������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJMX, strT, false, intUserLimit);
                    break;
                case 5:
                    strT = "��Ʒ��Ʒ������������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXSMX, strT, false, intUserLimit);
                    break;
                case 6:
                    strT = "��Ʒ��Ʒ���������ڳɱ����ۣ�;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewCRK, strT, false, intUserLimit);
                    break;
                case 7:
                    strT = "��Ʒ��Ʒ���������������;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewCHFX, strT, false, intUserLimit);
                    break;
                case 8:
                    strT = "��Ʒ��Ʒ���������������;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewJW, strT, false, intUserLimit);
                    break;
                case 9:
                    strT = "��Ʒ��Ʒ�����������˻ط�����;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJTC, strT, false, intUserLimit);
                    break;
                case 10:
                    strT = "��Ʒ��Ʒ�����������˻ط�����;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXSTH, strT, false, intUserLimit);
                    break;
            }

        }

        private void dataGridViewMX_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgV = (DataGridView)sender;
            if (dgV.RowCount < 1)
                return;

            if (dgV.SelectedRows.Count < 1)
                return;

            string sTemp = "", sTemp1 = "";
            sTemp = dgV.SelectedRows[0].Cells[1].Value.ToString().ToUpper();
            sTemp1 = dgV.SelectedRows[0].Cells[0].Value.ToString();

            switch (sTemp.Substring(0, 2))
            {
                case "CG":
                    // �������Ӵ����һ����ʵ����
                    FormCGHT childFormCGHT = new FormCGHT();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormCGHT.MdiParent = this.MdiParent;

                    childFormCGHT.strConn = strConn;
                    childFormCGHT.iDJID = Convert.ToInt32(sTemp1);
                    childFormCGHT.isSaved = true;

                    childFormCGHT.intUserID = intUserID;
                    childFormCGHT.intUserLimit = intUserLimit;
                    childFormCGHT.strUserLimit = strUserLimit;
                    childFormCGHT.strUserName = strUserName;
                    childFormCGHT.Show();
                    break;
                case "XS":
                    // �������Ӵ����һ����ʵ����
                    FormXSHT childFormXSHT = new FormXSHT();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSHT.MdiParent = this.MdiParent;

                    childFormXSHT.strConn = strConn;
                    childFormXSHT.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSHT.isSaved = true;

                    childFormXSHT.intUserID = intUserID;
                    childFormXSHT.intUserLimit = intUserLimit;
                    childFormXSHT.strUserLimit = strUserLimit;
                    childFormXSHT.strUserName = strUserName;
                    childFormXSHT.Show();
                    break;
            }

            switch (sTemp.Substring(0, 3))
            {
                case "AKP":
                    // �������Ӵ����һ����ʵ����
                    FormGJSPZD childFormGJSPZD = new FormGJSPZD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormGJSPZD.MdiParent = this.MdiParent;

                    childFormGJSPZD.strConn = strConn;
                    childFormGJSPZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormGJSPZD.isSaved = true;

                    childFormGJSPZD.intUserID = intUserID;
                    childFormGJSPZD.intUserLimit = intUserLimit;
                    childFormGJSPZD.strUserLimit = strUserLimit;
                    childFormGJSPZD.strUserName = strUserName;
                    childFormGJSPZD.Show();
                    break;

                case "ADH":
                    // �������Ӵ����һ����ʵ����
                    FormJHRKYHD childFormJHRKYHD = new FormJHRKYHD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormJHRKYHD.MdiParent = this.MdiParent;

                    childFormJHRKYHD.strConn = strConn;
                    childFormJHRKYHD.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHRKYHD.isSaved = true;

                    childFormJHRKYHD.intUserID = intUserID;
                    childFormJHRKYHD.intUserLimit = intUserLimit;
                    childFormJHRKYHD.strUserLimit = strUserLimit;
                    childFormJHRKYHD.strUserName = strUserName;
                    childFormJHRKYHD.Show();
                    break;

                case "ATH":
                    // �������Ӵ����һ����ʵ����
                    FormJHTCZD childFormJHTCZD = new FormJHTCZD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormJHTCZD.MdiParent = this.MdiParent;

                    childFormJHTCZD.strConn = strConn;
                    childFormJHTCZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHTCZD.isSaved = true;

                    childFormJHTCZD.intUserID = intUserID;
                    childFormJHTCZD.intUserLimit = intUserLimit;
                    childFormJHTCZD.strUserLimit = strUserLimit;
                    childFormJHTCZD.strUserName = strUserName;
                    childFormJHTCZD.Show();
                    break;

                case "ATB":
                    // �������Ӵ����һ����ʵ����
                    FormJHTBJDJ childFormJHTBJDJ = new FormJHTBJDJ();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormJHTBJDJ.MdiParent = this.MdiParent;

                    childFormJHTBJDJ.strConn = strConn;
                    childFormJHTBJDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHTBJDJ.isSaved = true;

                    childFormJHTBJDJ.intUserID = intUserID;
                    childFormJHTBJDJ.intUserLimit = intUserLimit;
                    childFormJHTBJDJ.strUserLimit = strUserLimit;
                    childFormJHTBJDJ.strUserName = strUserName;
                    childFormJHTBJDJ.Show();
                    break;

                case "AYF":
                    // �������Ӵ����һ����ʵ����
                    FormYFZKJS childFormYFZKJS = new FormYFZKJS();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormYFZKJS.MdiParent = this.MdiParent;

                    childFormYFZKJS.strConn = strConn;
                    childFormYFZKJS.iDJID = Convert.ToInt32(sTemp1);
                    childFormYFZKJS.isSaved = true;

                    childFormYFZKJS.intUserID = intUserID;
                    childFormYFZKJS.intUserLimit = intUserLimit;
                    childFormYFZKJS.strUserLimit = strUserLimit;
                    childFormYFZKJS.strUserName = strUserName;
                    childFormYFZKJS.Show();
                    break;

                case "BKP":
                    // �������Ӵ����һ����ʵ����
                    FormXSCKZD childFormXSCKZD = new FormXSCKZD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSCKZD.MdiParent = this.MdiParent;

                    childFormXSCKZD.strConn = strConn;
                    childFormXSCKZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSCKZD.isSaved = true;

                    childFormXSCKZD.intUserID = intUserID;
                    childFormXSCKZD.intUserLimit = intUserLimit;
                    childFormXSCKZD.strUserLimit = strUserLimit;
                    childFormXSCKZD.strUserName = strUserName;
                    childFormXSCKZD.Show();
                    break;

                case "BCK":
                    // �������Ӵ����һ����ʵ����
                    FormXSCKJD childFormXSCKJD = new FormXSCKJD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSCKJD.MdiParent = this.MdiParent;

                    childFormXSCKJD.strConn = strConn;
                    childFormXSCKJD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSCKJD.isSaved = true;

                    childFormXSCKJD.intUserID = intUserID;
                    childFormXSCKJD.intUserLimit = intUserLimit;
                    childFormXSCKJD.strUserLimit = strUserLimit;
                    childFormXSCKJD.strUserName = strUserName;
                    childFormXSCKJD.Show();
                    break;

                case "BTH":
                    // �������Ӵ����һ����ʵ����
                    FormXSTHZD childFormXSTHZD = new FormXSTHZD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSTHZD.MdiParent = this.MdiParent;

                    childFormXSTHZD.strConn = strConn;
                    childFormXSTHZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSTHZD.isSaved = true;

                    childFormXSTHZD.intUserID = intUserID;
                    childFormXSTHZD.intUserLimit = intUserLimit;
                    childFormXSTHZD.strUserLimit = strUserLimit;
                    childFormXSTHZD.strUserName = strUserName;
                    childFormXSTHZD.Show();
                    break;

                case "BTB":
                    // �������Ӵ����һ����ʵ����
                    FormXSTBJDJ childFormXSTBJDJ = new FormXSTBJDJ();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSTBJDJ.MdiParent = this.MdiParent;

                    childFormXSTBJDJ.strConn = strConn;
                    childFormXSTBJDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSTBJDJ.isSaved = true;

                    childFormXSTBJDJ.intUserID = intUserID;
                    childFormXSTBJDJ.intUserLimit = intUserLimit;
                    childFormXSTBJDJ.strUserLimit = strUserLimit;
                    childFormXSTBJDJ.strUserName = strUserName;
                    childFormXSTBJDJ.Show();
                    break;

                case "BYS":
                    // �������Ӵ����һ����ʵ����
                    FormYSZKJS childFormYSZKJS = new FormYSZKJS();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormYSZKJS.MdiParent = this.MdiParent;

                    childFormYSZKJS.strConn = strConn;
                    childFormYSZKJS.iDJID = Convert.ToInt32(sTemp1);
                    childFormYSZKJS.isSaved = true;

                    childFormYSZKJS.intUserID = intUserID;
                    childFormYSZKJS.intUserLimit = intUserLimit;
                    childFormYSZKJS.strUserLimit = strUserLimit;
                    childFormYSZKJS.strUserName = strUserName;
                    childFormYSZKJS.Show();
                    break;

                case "CPD":
                    // �������Ӵ����һ����ʵ����
                    FormKCSPPD2 childFormKCSPPD2 = new FormKCSPPD2();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormKCSPPD2.MdiParent = this.MdiParent;

                    childFormKCSPPD2.strConn = strConn;
                    childFormKCSPPD2.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCSPPD2.isSaved = true;

                    childFormKCSPPD2.intUserID = intUserID;
                    childFormKCSPPD2.intUserLimit = intUserLimit;
                    childFormKCSPPD2.strUserLimit = strUserLimit;
                    childFormKCSPPD2.strUserName = strUserName;
                    childFormKCSPPD2.Show();
                    break;

                case "CBS":
                    // �������Ӵ����һ����ʵ����
                    FormKCSPBSCL childFormKCSPBSCL = new FormKCSPBSCL();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormKCSPBSCL.MdiParent = this.MdiParent;

                    childFormKCSPBSCL.strConn = strConn;
                    childFormKCSPBSCL.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCSPBSCL.isSaved = true;

                    childFormKCSPBSCL.intUserID = intUserID;
                    childFormKCSPBSCL.intUserLimit = intUserLimit;
                    childFormKCSPBSCL.strUserLimit = strUserLimit;
                    childFormKCSPBSCL.strUserName = strUserName;
                    childFormKCSPBSCL.Show();
                    break;

                case "CCK":
                    // �������Ӵ����һ����ʵ����
                    FormKCJWCKDJ childFormKCJWCKDJ = new FormKCJWCKDJ();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormKCJWCKDJ.MdiParent = this.MdiParent;

                    childFormKCJWCKDJ.strConn = strConn;
                    childFormKCJWCKDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCJWCKDJ.isSaved = true;

                    childFormKCJWCKDJ.intUserID = intUserID;
                    childFormKCJWCKDJ.intUserLimit = intUserLimit;
                    childFormKCJWCKDJ.strUserLimit = strUserLimit;
                    childFormKCJWCKDJ.strUserName = strUserName;
                    childFormKCJWCKDJ.Show();
                    break;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 3:
                    toolStripStatusLabelC.Text = "�����ϼƣ�"+cTemp[3].ToString("f0")+" ���ϼƣ�"+cTemp1[3].ToString("f2");
                    break;
                case 4:
                    toolStripStatusLabelC.Text = "�����ϼƣ�" + cTemp[4].ToString("f0") + " ���ϼƣ�" + cTemp1[4].ToString("f2");
                    break;
                case 5:
                    toolStripStatusLabelC.Text = "�����ϼƣ�" + cTemp[5].ToString("f0") + " ���ϼƣ�" + cTemp1[5].ToString("f2");
                    break;
                case 7:
                    toolStripStatusLabelC.Text = "�����ϼƣ�" + cTemp[7].ToString("f0");
                    break;
                case 8:
                    toolStripStatusLabelC.Text = "�����ϼƣ�" + cTemp[8].ToString("f0");
                    break;
                case 9:
                    toolStripStatusLabelC.Text = "�����ϼƣ�" + cTemp[9].ToString("f0");
                    break;
                case 10:
                    toolStripStatusLabelC.Text = "�����ϼƣ�" + cTemp[10].ToString("f0") + " ���ϼƣ�" + cTemp1[10].ToString("f2");
                    break;
                case 11:
                    toolStripStatusLabelC.Text = "�����ϼƣ�" + cTemp[11].ToString("f0") + " ���ϼƣ�" + cTemp1[11].ToString("f2");
                    break;
                case 12:
                    toolStripStatusLabelC.Text = "�����ϼƣ�" + cTemp[12].ToString("f0") + " ���ϼƣ�" + cTemp1[12].ToString("f2");
                    break;
                case 13:
                    toolStripStatusLabelC.Text = "�����ϼƣ�" + cTemp[13].ToString("f0") + " ���ϼƣ�" + cTemp1[13].ToString("f2");
                    break;
                default:
                    toolStripStatusLabelC.Text = "";
                    break;
            }
        }



    }
}