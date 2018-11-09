using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXSYWCX : Form
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


        private int iSupplyCompany = 0;
        private ClassGetInformation cGetInformation;

        private decimal[] cTemp = new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0 };
        private decimal[] cTemp1 = new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0 };

        private decimal[] cTemp2 = new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private decimal[] cTemp3 = new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public int LIMITACCESS = 18;
        public int LIMITACCESS1 = 5;
        public int LIMITACCESS2 = 10;

        public FormXSYWCX()
        {
            InitializeComponent();
        }

        private void FormXSYWCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            //�õ���ʼʱ��
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��˾����, ����Ŀ��1, ����Ŀ��2, ����Ŀ��3, ����Ŀ��4, ����ԱȨ��, �ܾ���Ȩ��, ְԱȨ��, ����Ȩ��, ҵ��ԱȨ�� FROM ϵͳ������";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {

                try
                {
                    LIMITACCESS = int.Parse(sqldr.GetValue(6).ToString());
                    LIMITACCESS1 = int.Parse(sqldr.GetValue(7).ToString());
                    LIMITACCESS2 = int.Parse(sqldr.GetValue(9).ToString());
                }
                catch
                {
                    LIMITACCESS = 18;
                    LIMITACCESS1 = 5;
                    LIMITACCESS2 = 10;
                }
            }
            sqldr.Close();
            //sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
            sqlComm.CommandText = "SELECT GETDATE()";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");

            }
            sqldr.Close();

            //��ʼ��Ա���б�
            sqlComm.CommandText = "SELECT ID, ְԱ���� FROM ְԱ�� WHERE (beactive = 1)";

            if (dSet.Tables.Contains("ְԱ��")) dSet.Tables.Remove("ְԱ��");
            sqlDA.Fill(dSet, "ְԱ��");
            DataRow drTemp = dSet.Tables["ְԱ��"].NewRow();
            drTemp[0] = 0;
            drTemp[1] = "ȫ��";
            dSet.Tables["ְԱ��"].Rows.Add(drTemp);


            comboBoxYWY.DataSource = dSet.Tables["ְԱ��"];
            comboBoxYWY.DisplayMember = "ְԱ����";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.Text = strUserName;
            comboBoxYWY.SelectedValue = 0;
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            tabControl1.SelectedIndex = 2;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(2, "") == 0)
            {
                //return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(20, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                }
            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(22, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            //δ�������
            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���,��λ��.��λ����, ������Ʒ�Ƶ���.��˰�ϼ�, ������Ʒ�Ƶ���.δ������, ������Ʒ�Ƶ���.��ע, ������Ʒ�Ƶ���.��Ʊ�� FROM ��λ�� INNER JOIN ������Ʒ�Ƶ��� ON ��λ��.ID = ������Ʒ�Ƶ���.��λID WHERE (������Ʒ�Ƶ���.δ������ <> 0) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) ";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            if (intUserLimit < LIMITACCESS2)
            {
                sqlComm.CommandText += " AND ((������Ʒ�Ƶ���.ҵ��ԱID=" + intUserID.ToString() + ") OR (������Ʒ�Ƶ���.����ԱID=" + intUserID.ToString() + "))";
            }
            sqlComm.CommandText += " ORDER BY ����";
            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");

            //δ������ϸ
            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ë��, ������Ʒ�Ƶ���ϸ��.��Ʒ FROM ��λ�� INNER JOIN ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID ON ��λ��.ID = ������Ʒ�Ƶ���.��λID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���ϸ��.δ������ <> 0) ";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            if (intUserLimit < LIMITACCESS2)
            {
                sqlComm.CommandText += " AND ((������Ʒ�Ƶ���.ҵ��ԱID=" + intUserID.ToString() + ") OR (������Ʒ�Ƶ���.����ԱID=" + intUserID.ToString() + "))";
            }
            sqlComm.CommandText += " ORDER BY ������Ʒ�Ƶ���ϸ��.����";

            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");

            //�Ƶ�����
            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��������, ������Ʒ�Ƶ���.����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ���ۺ�ͬ��.��ͬ���, ������Ʒ�Ƶ���.��˰�ϼ�, ������Ʒ�Ƶ���.��ע, ������Ʒ�Ƶ���.��Ʊ�� FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ������Ʒ�Ƶ���.����ԱID = ����Ա.ID LEFT OUTER JOIN ���ۺ�ͬ�� ON ������Ʒ�Ƶ���.��ͬID = ���ۺ�ͬ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";

            //sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��������, ������Ʒ�Ƶ���.����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ���ۺ�ͬ��.��ͬ���, ������Ʒ�Ƶ���.��˰�ϼ�, ������Ʒ�Ƶ���.��ע, ������Ʒ�Ƶ���.��Ʊ��, �����տ���ܱ�.��ע AS �տע1, �����տ���ܱ�.��ע2 AS �տע2 FROM ���ۺ�ͬ�� RIGHT OUTER JOIN ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� AS ����Ա ON ������Ʒ�Ƶ���.����ԱID = ����Ա.ID LEFT OUTER JOIN �����տ�ұ� INNER JOIN �����տ���ϸ�� ON �����տ�ұ�.����ID = �����տ���ϸ��.ID INNER JOIN �����տ���ܱ� ON �����տ���ϸ��.����ID = �����տ���ܱ�.ID ON ������Ʒ�Ƶ���.���ݱ�� = �����տ�ұ�.���ݱ�� ON ���ۺ�ͬ��.ID = ������Ʒ�Ƶ���.��ͬID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            if (intUserLimit < LIMITACCESS2)
            {
                sqlComm.CommandText += " AND ((������Ʒ�Ƶ���.ҵ��ԱID=" + intUserID.ToString() + ") OR (������Ʒ�Ƶ���.����ԱID=" + intUserID.ToString() + "))";
            }
            sqlComm.CommandText += " ORDER BY ������Ʒ�Ƶ���.����";
            if (dSet.Tables.Contains("��Ʒ��3")) dSet.Tables.Remove("��Ʒ��3");
            sqlDA.Fill(dSet, "��Ʒ��3");

            //�Ƶ���ϸ
            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ë��, ������Ʒ�Ƶ���ϸ��.��Ʒ, ������Ʒ�Ƶ���ϸ��.����*������Ʒ�Ƶ���ϸ��.���ɱ��� AS ���۳ɱ� FROM ��λ�� INNER JOIN ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID ON ��λ��.ID = ������Ʒ�Ƶ���.��λID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            if (intUserLimit < LIMITACCESS2)
            {
                sqlComm.CommandText += " AND ((������Ʒ�Ƶ���.ҵ��ԱID=" + intUserID.ToString() + ") OR (������Ʒ�Ƶ���.����ԱID=" + intUserID.ToString() + "))";
            }
            sqlComm.CommandText += " ORDER BY ������Ʒ�Ƶ���.����";
            if (dSet.Tables.Contains("��Ʒ��4")) dSet.Tables.Remove("��Ʒ��4");
            sqlDA.Fill(dSet, "��Ʒ��4");

            //�������
            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��������, ������Ʒ�Ƶ���.����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ������Ʒ�Ƶ���.��˰�ϼ�, ������Ʒ�Ƶ���.��ע FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ������Ʒ�Ƶ���.����ԱID = ����Ա.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.�Ѹ����� <> 0) ";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            if (intUserLimit < LIMITACCESS2)
            {
                sqlComm.CommandText += " AND (��λ��.ҵ��Ա = N'" + strUserName + "') ";
            }
            sqlComm.CommandText += " ORDER BY ������Ʒ�Ƶ���.����";
            if (dSet.Tables.Contains("��Ʒ��5")) dSet.Tables.Remove("��Ʒ��5");
            sqlDA.Fill(dSet, "��Ʒ��5");

            //������ϸ
           //sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.δ������, ������Ʒ�Ƶ���ϸ��.�Ѹ�����, ������Ʒ�Ƶ���ϸ��.��Ʒ FROM ��λ�� INNER JOIN ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID ON ��λ��.ID = ������Ʒ�Ƶ���.��λID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���ϸ��.�Ѹ����� <> 0)";

            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.δ������, ������Ʒ�Ƶ���ϸ��.�Ѹ�����, ������Ʒ�Ƶ���ϸ��.��Ʒ, �����տ���ϸ��.����ID, �����տ���ܱ�.��ע AS ���㱸ע FROM ��λ�� INNER JOIN ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID ON ��λ��.ID = ������Ʒ�Ƶ���.��λID INNER JOIN �����տ�ұ� ON ������Ʒ�Ƶ���.���ݱ�� = �����տ�ұ�.���ݱ�� AND ������Ʒ�Ƶ���ϸ��.ID = �����տ�ұ�.����ID INNER JOIN �����տ���ϸ�� ON �����տ�ұ�.����ID = �����տ���ϸ��.ID INNER JOIN �����տ���ܱ� ON �����տ���ϸ��.����ID = �����տ���ܱ�.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���ϸ��.�Ѹ����� <> 0) AND (�����տ���ܱ�.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            if (intUserLimit < LIMITACCESS2)
            {
                sqlComm.CommandText += " AND ((������Ʒ�Ƶ���.ҵ��ԱID=" + intUserID.ToString() + ") OR (������Ʒ�Ƶ���.����ԱID=" + intUserID.ToString() + "))";
            }
            sqlComm.CommandText += " ORDER BY ������Ʒ�Ƶ���.����";
            if (dSet.Tables.Contains("��Ʒ��6")) dSet.Tables.Remove("��Ʒ��6");
            sqlDA.Fill(dSet, "��Ʒ��6");

            //У����ϸ
            sqlComm.CommandText = "SELECT ���۳�����ܱ�.���ݱ��, ���۳�����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ���۳�����ϸ��.����, ���۳�����ϸ��.���, ���۳�����ϸ��.����, ���۳�����ϸ��.ʵ�ƽ��, ���۳�����ϸ��.��Ʒ, ���۳�����ϸ��.ë�� FROM ���۳�����ܱ� INNER JOIN ���۳�����ϸ�� ON ���۳�����ܱ�.ID = ���۳�����ϸ��.����ID INNER JOIN ��Ʒ�� ON ���۳�����ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ���۳�����ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��λ�� ON ���۳�����ܱ�.��λID = ��λ��.ID WHERE (���۳�����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���۳�����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���۳�����ܱ�.BeActive = 1) ";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (���۳�����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            if (intUserLimit < LIMITACCESS2)
            {
                sqlComm.CommandText += " AND ((���۳�����ܱ�.ҵ��ԱID=" + intUserID.ToString() + ") OR (���۳�����ܱ�.����ԱID=" + intUserID.ToString() + "))";
            }
            sqlComm.CommandText += " ORDER BY ���۳�����ܱ�.����";
            if (dSet.Tables.Contains("��Ʒ��7")) dSet.Tables.Remove("��Ʒ��7");
            sqlDA.Fill(dSet, "��Ʒ��7");


            //δУ����ϸ
            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ë��, ������Ʒ�Ƶ���ϸ��.��Ʒ FROM ��λ�� INNER JOIN ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID ON ��λ��.ID = ������Ʒ�Ƶ���.��λID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���ϸ��.δ�������� > 0) ";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            if (intUserLimit < LIMITACCESS2)
            {
                sqlComm.CommandText += " AND ((������Ʒ�Ƶ���.ҵ��ԱID=" + intUserID.ToString() + ") OR (������Ʒ�Ƶ���.����ԱID=" + intUserID.ToString() + "))";
            }
            sqlComm.CommandText += " ORDER BY ������Ʒ�Ƶ���.����";
            if (dSet.Tables.Contains("��Ʒ��8")) dSet.Tables.Remove("��Ʒ��8");
            sqlDA.Fill(dSet, "��Ʒ��8");


            //�����˻ػ���
            sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.δ������, �����˳����ܱ�.��ע, �����˳����ܱ�.��Ʊ�� FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˳����ܱ�.BeActive = 1) ";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            if (intUserLimit < LIMITACCESS2)
            {
                sqlComm.CommandText += " AND ((�����˳����ܱ�.ҵ��ԱID=" + intUserID.ToString() + ") OR (�����˳����ܱ�.����ԱID=" + intUserID.ToString() + "))";
            }
            sqlComm.CommandText += " ORDER BY �����˳����ܱ�.����";
            if (dSet.Tables.Contains("��Ʒ��9")) dSet.Tables.Remove("��Ʒ��9");
            sqlDA.Fill(dSet, "��Ʒ��9");

            //�����˻���ϸ
            sqlComm.CommandText = "SELECT �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����˳���ϸ��.����, �����˳���ϸ��.����, �����˳���ϸ��.ʵ�ƽ�� FROM �����˳���ϸ�� INNER JOIN �����˳����ܱ� ON �����˳���ϸ��.����ID = �����˳����ܱ�.ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON �����˳���ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˳����ܱ�.BeActive = 1) ";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            if (intUserLimit < LIMITACCESS2)
            {
                sqlComm.CommandText += " AND ((�����˳����ܱ�.ҵ��ԱID=" + intUserID.ToString() + ") OR (�����˳����ܱ�.����ԱID=" + intUserID.ToString() + "))";
            }
            sqlComm.CommandText += " ORDER BY �����˳����ܱ�.����";
            if (dSet.Tables.Contains("��Ʒ��10")) dSet.Tables.Remove("��Ʒ��10");
            sqlDA.Fill(dSet, "��Ʒ��10");

            sqlComm.CommandText = "SELECT �����˲���ۻ��ܱ�.ID, �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �����˲���ۻ��ܱ�.��˰�ϼ�, ְԱ��.ְԱ���� AS ҵ��Ա, �����˲���ۻ��ܱ�.��ע FROM �����˲���ۻ��ܱ� INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����˲���ۻ��ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            if (intUserLimit < LIMITACCESS2)
            {
                sqlComm.CommandText += " AND ((�����˲���ۻ��ܱ�.ҵ��ԱID=" + intUserID.ToString() + ") OR (�����˲���ۻ��ܱ�.����ԱID=" + intUserID.ToString() + "))";
            }
            sqlComm.CommandText += " ORDER BY �����˲���ۻ��ܱ�.����";
            if (dSet.Tables.Contains("��Ʒ��11")) dSet.Tables.Remove("��Ʒ��11");
            sqlDA.Fill(dSet, "��Ʒ��11");


            sqlComm.CommandText = "SELECT �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, �����˲������ϸ��.��������, �����˲������ϸ��.��� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����˲���ۻ��ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND  (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            if (intUserLimit < LIMITACCESS2)
            {
                sqlComm.CommandText += " AND ((�����˲���ۻ��ܱ�.ҵ��ԱID=" + intUserID.ToString() + ") OR (�����˲���ۻ��ܱ�.����ԱID=" + intUserID.ToString() + "))";
            }
            sqlComm.CommandText += " ORDER BY �����˲���ۻ��ܱ�.����";
            if (dSet.Tables.Contains("��Ʒ��12")) dSet.Tables.Remove("��Ʒ��12");
            sqlDA.Fill(dSet, "��Ʒ��12");

            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["��Ʒ��1"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView2.DataSource = dSet.Tables["��Ʒ��2"];
            dataGridView3.DataSource = dSet.Tables["��Ʒ��3"];
            dataGridView3.Columns[0].Visible = false;
            dataGridView4.DataSource = dSet.Tables["��Ʒ��4"];
            if(intUserLimit<10)
                dataGridView4.Columns[15].Visible = false;
            dataGridView5.DataSource = dSet.Tables["��Ʒ��5"];
            dataGridView6.DataSource = dSet.Tables["��Ʒ��6"];
            dataGridView7.DataSource = dSet.Tables["��Ʒ��7"];
            dataGridView8.DataSource = dSet.Tables["��Ʒ��8"];
            dataGridView9.DataSource = dSet.Tables["��Ʒ��9"];
            dataGridView9.Columns[0].Visible = false;
            dataGridView10.DataSource = dSet.Tables["��Ʒ��10"];
            dataGridView11.DataSource = dSet.Tables["��Ʒ��11"];
            //dataGridView3.Columns[11].Visible = false;
            dataGridView12.DataSource = dSet.Tables["��Ʒ��12"];

            dataGridView4.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView7.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView6.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView8.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[8].DefaultCellStyle.Format = "f0";

            dataGridView11.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridView11.Columns[0].Visible = false;
            dataGridView12.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView12.Columns[9].DefaultCellStyle.Format = "f2";

            //Ȩ�޿���
            if (intUserLimit < LIMITACCESS)
            {
                dataGridView2.Columns[13].Visible = false;
                dataGridView4.Columns[13].Visible = false;
                dataGridView4.Columns[15].Visible = false;
                dataGridView7.Columns[13].Visible = false;
                dataGridView8.Columns[13].Visible = false;
            }
            dataGridView6.Columns[15].Visible = false;


            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";
            strT = "����ҵ���ѯ��" + tabControl1.SelectedTab.Text + "��;��ǰ���ڣ�" + labelZDRQ.Text;


            try
            {
                Control[] ctrT=this.tabControl1.SelectedTab.Controls.Find("dataGridView" + (tabControl1.SelectedIndex + 1).ToString(), true);

                if (ctrT.Length > 0)
                {
                    DataGridView dgv = (DataGridView)ctrT[0];
                    PrintDGV.Print_DataGridView(dgv, strT, false, intUserLimit);
                }
            }
            catch
            {
            }

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";
            strT = "����ҵ���ѯ��" + tabControl1.SelectedTab.Text + "��;��ǰ���ڣ�" + labelZDRQ.Text;


            try
            {
                Control[] ctrT = this.tabControl1.SelectedTab.Controls.Find("dataGridView" + (tabControl1.SelectedIndex + 1).ToString(), true);

                if (ctrT.Length > 0)
                {
                    DataGridView dgv = (DataGridView)ctrT[0];
                    PrintDGV.Print_DataGridView(dgv, strT, true, intUserLimit);
                }
            }
            catch
            {
            }

        }

        private void dataGridV_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgV = (DataGridView)sender;
            if (dgV.RowCount < 1)
                return;

            if (intUserLimit < LIMITACCESS1)
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

        private void countfTemp()
        {
            int c = 0, c1 = 0, c2=0, c3=0;
            int i, j;

            for (i = 1; i <= cTemp.Length; i++)
            {
                cTemp[i - 1] = 0;
                cTemp1[i - 1] = 0;
                cTemp2[i - 1] = 0;
                cTemp3[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 5;c1 = 0;c2 = 0;c3 = 0;
                        break;
                    case 2:
                        c = 11;c1 = 8;;c2 = 0;c3 = 0;
                        break;
                    case 3:
                        c = 10;c1 = 0;;c2 = 0;c3 = 0;
                        break;
                    case 4:
                        c = 11;c1 = 8;;c2 = 13;c3 = 15;
                        break;
                    case 5:
                        c = 8;c1 = 0;;c2 = 0;c3 = 0;
                        break;
                    case 6:
                        c = 11;c1 = 8;;c2 = 0;c3 = 0;
                        break;
                    case 7:
                        c = 11;c1 = 8;;c2 = 0;c3 = 0;
                        break;
                    case 8:
                        c = 11;c1 = 8;;c2 = 0;c3 = 0;
                        break;
                    case 9:
                        c = 5;c1 = 0;;c2 = 0;c3 = 0;
                        break;
                    case 10:
                        c = 10;c1 = 8;;c2 = 0;c3 = 0;
                        break;
                    case 11:
                        c = 5; c1 = 0;;c2 = 0;c3 = 0;
                        break;
                    case 12:
                        c = 9; c1 = 8;;c2 = 0;c3 = 0;
                        break;
                    default:
                        c = 0;c1 = 0;;c2 = 0;c3 = 0;
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

                if (c3 != 0)
                {

                    for (j = 0; j < dSet.Tables["��Ʒ��" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp3[i - 1] += decimal.Parse(dSet.Tables["��Ʒ��" + i.ToString()].Rows[j][c3].ToString());
                        }
                        catch
                        {
                        }
                    }
                }
                else
                    cTemp3[i - 1] = -1;



            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;
            if (!dSet.Tables.Contains("��Ʒ��" + c1.ToString()))
                return;
            toolStripStatusLabelC.Text = "����" + dSet.Tables["��Ʒ��" + c1.ToString()].Rows.Count.ToString() + "����¼ ���ϼ�" + cTemp[tabControl1.SelectedIndex].ToString("f2") + "Ԫ";

            if(cTemp1[tabControl1.SelectedIndex].ToString("f0")!="-1")
                toolStripStatusLabelC.Text += " �����ϼ� " + cTemp1[tabControl1.SelectedIndex].ToString("f0");

            if (intUserLimit >= LIMITACCESS)
            {
                if (cTemp2[tabControl1.SelectedIndex].ToString("f0") != "-1")
                    toolStripStatusLabelC.Text += " ë���ϼ� " + cTemp2[tabControl1.SelectedIndex].ToString("f2");

                if (cTemp3[tabControl1.SelectedIndex].ToString("f0") != "-1")
                    toolStripStatusLabelC.Text += " �ɱ��ϼ� " + cTemp3[tabControl1.SelectedIndex].ToString("f2");

            }

        }

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            iSupplyCompany = 0;
            textBoxDWMC.Text = "";
            textBoxDWBH.Text = "";
        }


    }
}