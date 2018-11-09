using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPZHCX : Form
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


        private int intCommID = 0, iJZID=0;
        private string SDTS0 = "", SDTS1 = "";

        private decimal[] cTemp = new decimal[8] { 0, 0, 0, 0, 0, 0, 0, 0};
        private decimal[] cTemp1 = new decimal[8] { 0, 0, 0, 0, 0, 0, 0, 0};
        private decimal[] cTemp2 = new decimal[8] { 0, 0, 0, 0, 0, 0, 0, 0};

        private ClassGetInformation cGetInformation;

        public FormSPZHCX()
        {
            InitializeComponent();
        }

        private void FormSPZHCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            //�õ��ϴν�ת

            //�õ���ʼʱ��
            sqlConn.Open();
            //sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
            sqlComm.CommandText = "SELECT GETDATE()";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");

            }
            sqldr.Close();

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
                //return;
            }
            else
            {
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
            if (intCommID == 0)
            {
                MessageBox.Show("��ѡ���ѯ��Ʒ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��ת.��ת����, ��ת.��ת���, ��Ʒ��.�������, ��Ʒ��.�����, ��Ʒ��.���ɱ��� FROM ��Ʒ�� LEFT OUTER JOIN (SELECT ��ת����, ��ת���, ��ƷID FROM ��ת��������ܱ� WHERE (ID = " + iJZID.ToString()+ ") AND (��ƷID = "+intCommID.ToString()+")) ��ת ON ��Ʒ��.ID = ��ת.��ƷID WHERE (��Ʒ��.ID = "+intCommID.ToString()+")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelSQJZSL.Text = sqldr.GetValue(0).ToString();
                labelSQJZJE.Text = sqldr.GetValue(1).ToString();
                labelZKCSL.Text = sqldr.GetValue(2).ToString();
                labelJZKCJE.Text = sqldr.GetValue(3).ToString();
                labelCBDJ.Text = sqldr.GetValue(4).ToString();
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.��Ʒ, ������Ʒ�Ƶ���ϸ��.ë�� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID INNER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (��Ʒ��.ID = " + intCommID.ToString() + ") AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY ������Ʒ�Ƶ���.���� DESC";

            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");


            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ�� FROM ��Ʒ�� INNER JOIN ��λ�� INNER JOIN �ⷿ�� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON �ⷿ��.ID = ������Ʒ�Ƶ���ϸ��.�ⷿID INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID ON ��λ��.ID = ������Ʒ�Ƶ���.��λID ON ��Ʒ��.ID = ������Ʒ�Ƶ���ϸ��.��ƷID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (��Ʒ��.ID = " + intCommID.ToString() + ") AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY ������Ʒ�Ƶ���.���� DESC";

            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");



            sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����˳���ϸ��.����, �����˳���ϸ��.����, �����˳���ϸ��.ʵ�ƽ��, (�����˳���ϸ��.ʵ�ƽ��-�����˳���ϸ��.����*�����˳���ϸ��.���ɱ���) AS ë�� FROM �����˳���ϸ�� INNER JOIN �����˳����ܱ� ON �����˳���ϸ��.����ID = �����˳����ܱ�.ID INNER JOIN �ⷿ�� ON �����˳���ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.BeActive = 1) AND (��Ʒ��.ID = " + intCommID.ToString() + ") AND (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY �����˳����ܱ�.���� DESC";

            if (dSet.Tables.Contains("��Ʒ��3")) dSet.Tables.Remove("��Ʒ��3");
            sqlDA.Fill(dSet, "��Ʒ��3");

            sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����˳���ϸ��.����, �����˳���ϸ��.����, �����˳���ϸ��.ʵ�ƽ�� FROM �����˳���ϸ�� INNER JOIN �����˳����ܱ� ON �����˳���ϸ��.����ID = �����˳����ܱ�.ID INNER JOIN �ⷿ�� ON �����˳���ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.BeActive = 1) AND (��Ʒ��.ID = " + intCommID.ToString() + ") AND (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY �����˳����ܱ�.���� DESC";

            if (dSet.Tables.Contains("��Ʒ��4")) dSet.Tables.Remove("��Ʒ��4");
            sqlDA.Fill(dSet, "��Ʒ��4");


            sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ���������ϸ��.����, ���������ϸ��.����, ���������ϸ��.������ FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID WHERE (���������ܱ�.BeActive = 1) AND (��Ʒ��.ID = " + intCommID.ToString() + ") AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY ���������ܱ�.���� DESC";

            if (dSet.Tables.Contains("��Ʒ��5")) dSet.Tables.Remove("��Ʒ��5");
            sqlDA.Fill(dSet, "��Ʒ��5");

            sqlComm.CommandText = "SELECT �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ����.���ɱ���, ����.�������, ����.�����, ����.����������, ����.���������� FROM ��Ʒ�� INNER JOIN ���� INNER JOIN �ⷿ�� ON ����.�ⷿID = �ⷿ��.ID ON ��Ʒ��.ID = ����.��ƷID WHERE (��Ʒ��.ID = " + intCommID.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��6")) dSet.Tables.Remove("��Ʒ��6");
            sqlDA.Fill(dSet, "��Ʒ��6");

            sqlComm.CommandText = "SELECT �����˲���ۻ��ܱ�.ID, �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����˲������ϸ��.��������, �����˲������ϸ��.���, �����˲������ϸ��.��� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID INNER JOIN �ⷿ�� ON �����˲������ϸ��.�ⷿID = �ⷿ��.ID WHERE (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") AND (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY �����˲���ۻ��ܱ�.���� DESC";

            if (dSet.Tables.Contains("��Ʒ��7")) dSet.Tables.Remove("��Ʒ��7");
            sqlDA.Fill(dSet, "��Ʒ��7");

            sqlComm.CommandText = "SELECT �����˲���ۻ��ܱ�.ID, �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����˲������ϸ��.��������, �����˲������ϸ��.���, �����˲������ϸ��.��� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID INNER JOIN �ⷿ�� ON �����˲������ϸ��.�ⷿID = �ⷿ��.ID WHERE (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") AND (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY �����˲���ۻ��ܱ�.���� DESC";
            if (dSet.Tables.Contains("��Ʒ��8")) dSet.Tables.Remove("��Ʒ��8");
            sqlDA.Fill(dSet, "��Ʒ��8");

            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["��Ʒ��1"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView2.DataSource = dSet.Tables["��Ʒ��2"];
            dataGridView2.Columns[0].Visible = false;
            dataGridView3.DataSource = dSet.Tables["��Ʒ��3"];
            dataGridView3.Columns[0].Visible = false;
            dataGridView4.DataSource = dSet.Tables["��Ʒ��4"];
            dataGridView4.Columns[0].Visible = false;
            dataGridView5.DataSource = dSet.Tables["��Ʒ��5"];
            dataGridView5.Columns[0].Visible = false;
            dataGridView6.DataSource = dSet.Tables["��Ʒ��6"];
            dataGridView7.DataSource = dSet.Tables["��Ʒ��7"];
            dataGridView7.Columns[0].Visible = false;
            dataGridView8.DataSource = dSet.Tables["��Ʒ��8"];
            dataGridView8.Columns[0].Visible = false;

            dataGridView1.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[10].DefaultCellStyle.Format = "f2";
            dataGridView2.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridView3.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView4.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView4.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView5.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView5.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView6.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridView6.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridView7.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView7.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView8.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView8.Columns[9].DefaultCellStyle.Format = "f2";
            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "��Ʒ�ۺϲ�ѯ��������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text+";��Ʒ��"+textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "��Ʒ�ۺϲ�ѯ��������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "��Ʒ�ۺϲ�ѯ���ⷿ��ϸ��;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, true, intUserLimit);
                    break;

            }

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "��Ʒ�ۺϲ�ѯ��������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "��Ʒ�ۺϲ�ѯ��������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "��Ʒ�ۺϲ�ѯ���ⷿ��ϸ��;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, false, intUserLimit);
                    break;

            }
        }

        private void dataGridV_DoubleClick(object sender, EventArgs e)
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

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {

        }

        private void countfTemp()
        {
            int c = 0, c1 = 0, c2=0;
            int i, j;

            for (i = 1; i <= cTemp.Length; i++)
            {
                cTemp[i - 1] = 0;
                cTemp1[i - 1] = 0;
                cTemp2[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 7; c1 = 10;c2 = 12;
                        break;
                    case 2:
                        c = 7; c1 = 11; c2 = 0;
                        break;
                    case 3:
                        c = 7; c1 = 9; c2 = 10;
                        break;
                    case 4:
                        c = 7; c1 = 9; c2 = 0;
                        break;
                    case 5:
                        c = 7; c1 = 9; c2 = 0;
                        break;
                    case 6:
                        c = 3; c1 = 4; c2 = 0;
                        break;
                    case 7:
                        c = 7; c1 = 9; c2 = 0;
                        break;
                    case 8:
                        c = 7; c1 = 9; c2 = 0;
                        break;
                    default:
                        c = 0;
                        break;
                }

                if (c != 0)
                {

                    for (j = 0; j < dSet.Tables["��Ʒ��" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp[i - 1] += decimal.Parse(dSet.Tables["��Ʒ��" + i.ToString()].Rows[j][c].ToString());
                        }
                        catch
                        {
                        }
                    }
                }
                if (c1 != 0)
                {

                    for (j = 0; j < dSet.Tables["��Ʒ��" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp1[i - 1] += decimal.Parse(dSet.Tables["��Ʒ��" + i.ToString()].Rows[j][c1].ToString());
                        }
                        catch
                        {
                        }
                    }
                }
                else
                    cTemp1[i - 1] = -1;

                if (c2 != 0)
                {

                    for (j = 0; j < dSet.Tables["��Ʒ��" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp2[i - 1] += decimal.Parse(dSet.Tables["��Ʒ��" + i.ToString()].Rows[j][c2].ToString());
                        }
                        catch
                        {
                        }
                    }
                }
                else
                    cTemp2[i - 1] = -1;


            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;
            if (!dSet.Tables.Contains("��Ʒ��" + c1.ToString()))
                return;
            toolStripStatusLabelZH.Text = "����" + dSet.Tables["��Ʒ��" + c1.ToString()].Rows.Count.ToString() + "����¼ �����ϼ�" + cTemp[tabControl1.SelectedIndex].ToString("f0") + " ���ϼ�" + cTemp1[tabControl1.SelectedIndex].ToString("f2") + "Ԫ";

            if (cTemp2[tabControl1.SelectedIndex] != -1)
                toolStripStatusLabelZH.Text += " ë�� " + cTemp2[tabControl1.SelectedIndex].ToString("f2");
        }


    }
}