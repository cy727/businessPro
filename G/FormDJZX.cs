using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormDJZX : Form
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

        private int iConstLimit = 18; 

        public FormDJZX()
        {
            InitializeComponent();
        }

        private void FormDJZX_Load(object sender, EventArgs e)
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
                    iConstLimit = int.Parse(sqldr.GetValue(6).ToString());
                }
                catch
                {
                    iConstLimit = 18;
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
            sqlComm.CommandText = "SELECT ID, ְԱ���, ְԱ���� FROM ְԱ�� WHERE (beactive = 1)";

            if (dSet.Tables.Contains("ְԱ��")) dSet.Tables.Remove("ְԱ��");
            sqlDA.Fill(dSet, "ְԱ��");

            object[] OTemp = new object[3];
            OTemp[0] = 0;
            OTemp[1] = "ȫ��";
            OTemp[2] = "ȫ��";
            dSet.Tables["ְԱ��"].Rows.Add(OTemp);

            comboBoxYWY.DataSource = dSet.Tables["ְԱ��"];
            comboBoxYWY.DisplayMember = "ְԱ����";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.SelectedIndex= comboBoxYWY.Items.Count - 1;

            sqlComm.CommandText = "SELECT ID, ְԱ���, ְԱ���� FROM ְԱ�� WHERE (beactive = 1)";

            if (dSet.Tables.Contains("ְԱ��1")) dSet.Tables.Remove("ְԱ��1");
            sqlDA.Fill(dSet, "ְԱ��1");

            object[] OTemp1 = new object[3];
            OTemp1[0] = 0;
            OTemp1[1] = "ȫ��";
            OTemp1[2] = "ȫ��";
            dSet.Tables["ְԱ��1"].Rows.Add(OTemp1);

            comboBoxCZY.DataSource = dSet.Tables["ְԱ��1"];
            comboBoxCZY.DisplayMember = "ְԱ����";
            comboBoxCZY.ValueMember = "ID";
            comboBoxCZY.SelectedIndex = comboBoxCZY.Items.Count - 1;
            sqlConn.Close();

            if (intUserLimit < 11)
            {
                comboBoxYWY.SelectedValue = intUserID;
                comboBoxYWY.Enabled = false;

                comboBoxCZY.SelectedValue = intUserID;
                comboBoxCZY.Enabled = false;
            }

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            comboBoxDJLB.SelectedIndex = 0;
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int c=6;

            sqlConn.Open();
            switch (comboBoxDJLB.SelectedIndex)
            {
                case 0:
                    sqlComm.CommandText = "SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.��ͬ���, �ɹ���ͬ��.ǩ��ʱ��, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, �ɹ���ͬ��.��� FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON �ɹ���ͬ��.����ԱID = ����Ա.ID WHERE (�ɹ���ͬ��.BeActive = 1) AND (�ɹ���ͬ��.ǩ��ʱ�� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�ɹ���ͬ��.ǩ��ʱ�� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString()+" OR "+"����Ա.ID="+comboBoxCZY.SelectedValue.ToString()+")";
                    }


                    if (textBoxDJBH.Text.Trim()!="")
                        sqlComm.CommandText += " AND �ɹ���ͬ��.��ͬ��� LIKE N'%" + textBoxDJBH.Text.Trim()+"%'";

                    sqlComm.CommandText += " ORDER BY  ǩ��ʱ�� DESC";
                    c = 7;

                    break;
                case 1:
                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���,��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ������Ʒ�Ƶ���.��˰�ϼ�, ������Ʒ�Ƶ���.��Ʊ��, ������Ʒ�Ƶ���.��ע FROM ��λ�� INNER JOIN ������Ʒ�Ƶ��� ON ��λ��.ID = ������Ʒ�Ƶ���.��λID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ������Ʒ�Ƶ���.����ԱID = ����Ա.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND ������Ʒ�Ƶ���.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 7;
                    break;



                case 2:
                    sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ���������ܱ�.��˰�ϼ�, ���������ܱ�.��Ʊ��, ���������ܱ�.��ע FROM ��λ�� INNER JOIN ���������ܱ� ON ��λ��.ID = ���������ܱ�.��λID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ���������ܱ�.����ԱID = ����Ա.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND ���������ܱ�.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 7;
                    break;


                case 3:
                    sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, �����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.֧Ʊ��, �����˳����ܱ�.��ע FROM ��λ�� INNER JOIN �����˳����ܱ� ON ��λ��.ID = �����˳����ܱ�.��λID INNER JOIN ְԱ�� ON �����˳����ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON �����˳����ܱ�.����ԱID = ����Ա.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˳����ܱ�.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND �����˳����ܱ�.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 7;
                    break;

                case 4:
                    sqlComm.CommandText = "SELECT �����˲���ۻ��ܱ�.ID, �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, �����˲���ۻ��ܱ�.��˰�ϼ�, �����˲���ۻ��ܱ�.��Ʊ��, �����˲���ۻ��ܱ�.��ע FROM ��λ�� INNER JOIN �����˲���ۻ��ܱ� ON ��λ��.ID = �����˲���ۻ��ܱ�.��λID INNER JOIN ְԱ�� ON �����˲���ۻ��ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON �����˲���ۻ��ܱ�.����ԱID = ����Ա.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND �����˲���ۻ��ܱ�.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 7;
                    break;

                case 5:
                    sqlComm.CommandText = "SELECT ���㸶����ܱ�.ID, ���㸶����ܱ�.���ݱ��, ���㸶����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ���㸶����ܱ�.ʵ�ƽ��, ���㸶����ܱ�.��Ʊ��, ���㸶����ܱ�.��ע, ���㸶����ܱ�.��ע2 FROM ��λ�� INNER JOIN ���㸶����ܱ� ON ��λ��.ID = ���㸶����ܱ�.��λID INNER JOIN ְԱ�� ON ���㸶����ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ���㸶����ܱ�.����ԱID = ����Ա.ID WHERE (���㸶����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���㸶����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���㸶����ܱ�.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND ���㸶����ܱ�.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 7;
                    break;

                case 6:
                    sqlComm.CommandText = "SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.��ͬ���, ���ۺ�ͬ��.ǩ��ʱ��, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա,  ���ۺ�ͬ��.��� FROM ��λ�� INNER JOIN ���ۺ�ͬ�� ON ��λ��.ID = ���ۺ�ͬ��.������λID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ���ۺ�ͬ��.����ԱID = ����Ա.ID WHERE (���ۺ�ͬ��.BeActive = 1) AND (���ۺ�ͬ��.ǩ��ʱ�� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���ۺ�ͬ��.ǩ��ʱ�� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND ���ۺ�ͬ��.��ͬ��� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ǩ��ʱ�� DESC";
                    c = 7;
                    break;

                case 7:
                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ������Ʒ�Ƶ���.��˰�ϼ�, ������Ʒ�Ƶ���.��Ʊ��, ������Ʒ�Ƶ���.��ע FROM ��λ�� INNER JOIN ������Ʒ�Ƶ��� ON ��λ��.ID = ������Ʒ�Ƶ���.��λID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ������Ʒ�Ƶ���.����ԱID = ����Ա.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND ������Ʒ�Ƶ���.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 7;
                    break;

                case 8:
                    sqlComm.CommandText = "SELECT ���۳�����ܱ�.ID, ���۳�����ܱ�.���ݱ��, ���۳�����ܱ�.����,��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա,����Ա.ְԱ���� AS ����Ա, ���۳�����ܱ�.��˰�ϼ�, ���۳�����ܱ�.��Ʊ��, ���۳�����ܱ�.��ע FROM ��λ�� INNER JOIN ���۳�����ܱ� ON ��λ��.ID = ���۳�����ܱ�.��λID INNER JOIN ְԱ�� ON ���۳�����ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ���۳�����ܱ�.����ԱID = ����Ա.ID WHERE (���۳�����ܱ�.BeActive = 1) AND (���۳�����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���۳�����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND ���۳�����ܱ�.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 7;
                    break;

                case 9:
                    sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, �����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.��ע FROM ��λ�� INNER JOIN �����˳����ܱ� ON ��λ��.ID = �����˳����ܱ�.��λID INNER JOIN ְԱ�� ON �����˳����ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON �����˳����ܱ�.����ԱID = ����Ա.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˳����ܱ�.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND �����˳����ܱ�.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 7;
                    break;

                case 10:
                    sqlComm.CommandText = "SELECT �����˲���ۻ��ܱ�.ID, �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, �����˲���ۻ��ܱ�.��˰�ϼ�, �����˲���ۻ��ܱ�.��Ʊ��, �����˲���ۻ��ܱ�.��ע FROM ��λ�� INNER JOIN �����˲���ۻ��ܱ� ON ��λ��.ID = �����˲���ۻ��ܱ�.��λID INNER JOIN ְԱ�� ON �����˲���ۻ��ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON �����˲���ۻ��ܱ�.����ԱID = ����Ա.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND �����˲���ۻ��ܱ�.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 7;
                    break;


                case 11:
                    sqlComm.CommandText = "SELECT �����տ���ܱ�.ID, �����տ���ܱ�.���ݱ��, �����տ���ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, �����տ���ܱ�.ʵ�ƽ��, �����տ���ܱ�.��Ʊ��, �����տ���ܱ�.��ע, �����տ���ܱ�.��ע2 FROM ��λ�� INNER JOIN �����տ���ܱ� ON ��λ��.ID = �����տ���ܱ�.��λID INNER JOIN ְԱ�� ON �����տ���ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON �����տ���ܱ�.����ԱID = ����Ա.ID WHERE (�����տ���ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����տ���ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����տ���ܱ�.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND �����տ���ܱ�.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 7;
                    break;

                case 12:
                    sqlComm.CommandText = "SELECT ����̵���ܱ�.ID, ����̵���ܱ�.���ݱ��, ����̵���ܱ�.����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ����̵���ܱ�.���������ϼ�, ����̵���ܱ�.������ϼ�, ����̵���ܱ�.��ע FROM ְԱ�� INNER JOIN ����̵���ܱ� ON ְԱ��.ID = ����̵���ܱ�.ҵ��ԱID INNER JOIN ְԱ�� ����Ա ON ����̵���ܱ�.����ԱID = ����Ա.ID WHERE (����̵���ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (����̵���ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (����̵���ܱ�.BeActive = 1)";

                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND ����̵���ܱ�.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 7;
                    break;

                case 13:
                    sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ���������ܱ�.������, ���������ܱ�.��˰�ϼ� AS ���۽��, ���������ܱ�.��ע FROM ��λ�� INNER JOIN ���������ܱ� ON ��λ��.ID = ���������ܱ�.��λID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ���������ܱ�.����ԱID = ����Ա.ID WHERE (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND ��λ��.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND ���������ܱ�.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 7;
                    break;

                case 14:
                    sqlComm.CommandText = "SELECT ��汨����ܱ�.ID, ��汨����ܱ�.���ݱ��, ��汨����ܱ�.����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ��汨����ܱ�.���������ϼ�, ��汨����ܱ�.������ϼ�, ��汨����ܱ�.��ע FROM ��汨����ܱ� INNER JOIN ְԱ�� ON ��汨����ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ��汨����ܱ�.����ԱID = ����Ա.ID WHERE (��汨����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (��汨����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (��汨����ܱ�.BeActive = 1)";

                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND ����Ա.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( ְԱ��.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "����Ա.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND ��汨����ܱ�.���ݱ�� LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  ���� DESC";
                    c = 6;
                    break;
                    
            }

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;


            sqlConn.Close();
            dataGridViewDJMX.Focus();

            counttoolStripStatusLabelC(c);
        }

        private void counttoolStripStatusLabelC(int c)
        {
            decimal fTemp;

            fTemp = 0;

            for (int i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                try
                {
                    fTemp += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][c].ToString());
                }
                catch
                {
                }
            }

            toolStripStatusLabelC.Text = "����" + dSet.Tables["��Ʒ��"].Rows.Count.ToString() + "�����ݼ�¼ ���ϼ�"+fTemp.ToString("f2")+"Ԫ";


        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1000, "") == 0)
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
                if (cGetInformation.getCompanyInformation(1100, textBoxDWBH.Text.Trim()) == 0)
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
                if (cGetInformation.getCompanyInformation(1200, textBoxDWMC.Text.Trim()) == 0)
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

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�������֣�" + comboBoxDJLB.Text+ "��;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true,intUserLimit);
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDJMX.RowCount < 1)
                return;

            if (dataGridViewDJMX.SelectedRows.Count < 1)
                return;

            string sTemp = "",sTemp1="";
            
            if(e==null)
            {
                sTemp = dataGridViewDJMX.SelectedRows[0].Cells[1].Value.ToString().ToUpper();
                sTemp1 = dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString();
            }
            else
            {
                sTemp=dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value.ToString().ToUpper();
                sTemp1=dataGridViewDJMX.Rows[e.RowIndex].Cells[0].Value.ToString();
            }

            //if(e.RowIndex<0)
            //    return;

            //if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
            //    return;


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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormGJSPZD.printToolStripButton.Visible = false;
                        childFormGJSPZD.printPreviewToolStripButton.Visible = false;
                    }

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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormJHRKYHD.printToolStripButton.Visible = false;
                        childFormJHRKYHD.printPreviewToolStripButton.Visible = false;
                    }

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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormJHTCZD.printToolStripButton.Visible = false;
                        childFormJHTCZD.printPreviewToolStripButton.Visible = false;
                    }

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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormJHTBJDJ.printToolStripButton.Visible = false;
                        childFormJHTBJDJ.printPreviewToolStripButton.Visible = false;
                    }
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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormYFZKJS.printToolStripButton.Visible = false;
                        childFormYFZKJS.printPreviewToolStripButton.Visible = false;
                    }
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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormXSCKZD.printToolStripButton.Visible = false;
                        childFormXSCKZD.printPreviewToolStripButton.Visible = false;
                    }
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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormXSCKJD.printToolStripButton.Visible = false;
                        childFormXSCKJD.printPreviewToolStripButton.Visible = false;
                    }
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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormXSTHZD.printToolStripButton.Visible = false;
                        childFormXSTHZD.printPreviewToolStripButton.Visible = false;
                    }
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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormXSTBJDJ.printToolStripButton.Visible = false;
                        childFormXSTBJDJ.printPreviewToolStripButton.Visible = false;
                    }
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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormYSZKJS.printToolStripButton.Visible = false;
                        childFormYSZKJS.printPreviewToolStripButton.Visible = false;
                    }
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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormKCSPPD2.printToolStripButton.Visible = false;
                        childFormKCSPPD2.printPreviewToolStripButton.Visible = false;
                    }
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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormKCSPBSCL.printToolStripButton.Visible = false;
                        childFormKCSPBSCL.printPreviewToolStripButton.Visible = false;
                    }
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
                    if (intUserLimit < iConstLimit)
                    {
                        childFormKCJWCKDJ.printToolStripButton.Visible = false;
                        childFormKCJWCKDJ.printPreviewToolStripButton.Visible = false;
                    }
                    childFormKCJWCKDJ.intUserID = intUserID;
                    childFormKCJWCKDJ.intUserLimit = intUserLimit;
                    childFormKCJWCKDJ.strUserLimit = strUserLimit;
                    childFormKCJWCKDJ.strUserName = strUserName;
                    childFormKCJWCKDJ.Show();
                    break;
            }


        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.Add)
            {
                //System.Windows.Forms.SendKeys.Send("{tab}");
                toolStripButtonGD_Click(null, null);//
                return true;
            }

            if (keyData == Keys.Enter && dataGridViewDJMX.Focused)
            {
                //System.Windows.Forms.SendKeys.Send("{tab}");
                dataGridViewDJMX_CellDoubleClick(null, null);//
                return true;
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            iSupplyCompany = 0;
            textBoxDWMC.Text = "";
            textBoxDWBH.Text = "";
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�������֣�" + comboBoxDJLB.Text + "��;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}