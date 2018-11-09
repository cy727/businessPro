using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormGJYWCX : Form
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

        private decimal[] cTemp=new decimal[]{0,0,0,0,0,0,0,0,0,0,0,0};
        private decimal[] cTemp1 = new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0 };
        
        public FormGJYWCX()
        {
            InitializeComponent();
        }

        private void FormGJYWCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

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
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

        }


        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1, "") == 0)
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
                if (cGetInformation.getCompanyInformation(10, textBoxDWBH.Text.Trim()) == 0)
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
                if (cGetInformation.getCompanyInformation(12, textBoxDWMC.Text.Trim()) == 0)
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
            sqlComm.CommandText = "SELECT ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ���������ܱ�.��˰�ϼ�, ���������ܱ�.��ע,���������ܱ�.δ������ FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ���������ܱ�.����ԱID = ����Ա.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.δ������ <> 0)";
            //sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, δ����.����, δ����.���, ���������ܱ�.��ע, ���������ܱ�.��Ʊ�� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN (SELECT SUM(δ������) AS ���, SUM(δ��������) AS ����, ����ID FROM ���������ϸ�� GROUP BY ����ID HAVING (SUM(δ������) <> 0) AND (SUM(δ��������) <> 0)) δ���� ON ���������ܱ�.ID = δ����.����ID WHERE (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (��λ��.ID = "+iSupplyCompany.ToString()+")";

            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");



            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.��Ʒ, ������Ʒ�Ƶ���.��Ʊ�� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��4")) dSet.Tables.Remove("��Ʒ��4");
            sqlDA.Fill(dSet, "��Ʒ��4");

            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ������Ʒ�Ƶ���.��˰�ϼ�, ������Ʒ�Ƶ���.���ʽ, �ɹ���ͬ��.��ͬ���, ������Ʒ�Ƶ���.��ע, ������Ʒ�Ƶ���.��Ʊ�� FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ������Ʒ�Ƶ���.����ԱID = ����Ա.ID LEFT OUTER JOIN �ɹ���ͬ�� ON ������Ʒ�Ƶ���.��ͬID = �ɹ���ͬ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��3")) dSet.Tables.Remove("��Ʒ��3");
            sqlDA.Fill(dSet, "��Ʒ��3");

            sqlComm.CommandText = "SELECT ���������ܱ�.���ݱ��, ������Ʒ�Ƶ���.���ݱ�� AS ԭ���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ���������ϸ��.����, ���������ϸ��.����, ���������ϸ��.���, ���������ϸ��.����, ���������ϸ��.ʵ�ƽ��, ���������ϸ��.��Ʒ, ���������ܱ�.��Ʊ�� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.����ID = ���������ܱ�.ID INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN ������Ʒ�Ƶ��� ON ���������ܱ�.����ID = ������Ʒ�Ƶ���.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            if (dSet.Tables.Contains("��Ʒ��7")) dSet.Tables.Remove("��Ʒ��7");
            sqlDA.Fill(dSet, "��Ʒ��7");

            //sqlComm.CommandText = "SELECT ��λ��.��λ���, ��Ʒ��ʷ�˱�.����, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��ʷ�˱�.���ݱ��, ��Ʒ��ʷ�˱�.ԭ���ݱ��, ְԱ��.ְԱ���� AS ҵ��Ա, ��Ʒ��ʷ�˱�.��Ʊ���, ��Ʒ��ʷ�˱�.�Ѹ���� FROM ��Ʒ��ʷ�˱� INNER JOIN ��λ�� ON ��Ʒ��ʷ�˱�.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ��Ʒ��ʷ�˱�.��ƷID = ��Ʒ��.ID INNER JOIN ְԱ�� ON ��Ʒ��ʷ�˱�.ҵ��ԱID = ְԱ��.ID WHERE (��Ʒ��ʷ�˱�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (��Ʒ��ʷ�˱�.BeActive = 1) AND (��Ʒ��ʷ�˱�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (��λ��.��λ��� LIKE '%AYF%')";

            sqlComm.CommandText = "SELECT ���������ܱ�.���ݱ�� AS ��ֵ���, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ���������ϸ��.����, ���������ϸ��.����, ���������ϸ��.���, ���������ϸ��.�Ѹ�����, ���������ϸ��.δ������ FROM ���������ܱ� INNER JOIN ���������ϸ�� ON ���������ܱ�.ID = ���������ϸ��.����ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID WHERE (���������ϸ��.�Ѹ����� <> 0) AND (���������ܱ�.BeActive = 1) AND  (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            if (dSet.Tables.Contains("��Ʒ��6")) dSet.Tables.Remove("��Ʒ��6");
            sqlDA.Fill(dSet, "��Ʒ��6");

            sqlComm.CommandText = "SELECT �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��λ��.��λ���, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.��Ʒ, ������Ʒ�Ƶ���ϸ��.δ�������� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID LEFT OUTER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID WHERE (������Ʒ�Ƶ���ϸ��.δ�������� > 0) AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 23:59:59', 102)) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102))";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            if (dSet.Tables.Contains("��Ʒ��8")) dSet.Tables.Remove("��Ʒ��8");
            sqlDA.Fill(dSet, "��Ʒ��8");


//          sqlComm.CommandText = "SELECT ���㸶����ܱ�.���ݱ��, ���������ܱ�.���ݱ�� AS ��ֵ���, ���㸶����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����,���㸶����ܱ�.ʵ�ƽ��, ���㸶����ܱ�.��ע AS ��ע1, ���㸶����ܱ�.��ע2 FROM ���㸶����ܱ� INNER JOIN ��λ�� ON ���㸶����ܱ�.��λID = ��λ��.ID LEFT OUTER JOIN ���������ܱ� ON ���㸶����ܱ�.ԭ����ID = ���������ܱ�.ID WHERE (���㸶����ܱ�.BeActive = 1) AND (���㸶����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���㸶����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqlComm.CommandText = "SELECT ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ���������ܱ�.��˰�ϼ�, ���������ܱ�.��ע,���������ܱ�.δ������ FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ���������ܱ�.����ԱID = ����Ա.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.�Ѹ����� <> 0)";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            if (dSet.Tables.Contains("��Ʒ��5")) dSet.Tables.Remove("��Ʒ��5");
            sqlDA.Fill(dSet, "��Ʒ��5");

            sqlComm.CommandText = "SELECT ���������ܱ�.���ݱ�� AS ��ֵ���, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ���������ϸ��.����, ���������ϸ��.����, ���������ϸ��.���, ���������ϸ��.δ������, ���������ϸ��.�Ѹ����� FROM ���������ܱ� INNER JOIN ���������ϸ�� ON ���������ܱ�.ID = ���������ϸ��.����ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID WHERE (���������ϸ��.δ������ <> 0) AND (���������ܱ�.BeActive = 1) AND  (���������ܱ�.���� >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 23:59:59', 102))";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");

            sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա, �����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.��ע, �����˳����ܱ�.��Ʊ�� FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����˳����ܱ�.ҵ��ԱID = [ְԱ��_1].ID INNER JOIN ְԱ�� ON �����˳����ܱ�.����ԱID = ְԱ��.ID WHERE (�����˳����ܱ�.BeActive = 1) AND  (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            if (dSet.Tables.Contains("��Ʒ��9")) dSet.Tables.Remove("��Ʒ��9");
            sqlDA.Fill(dSet, "��Ʒ��9");

            sqlComm.CommandText = "SELECT �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �����˳���ϸ��.����, �����˳���ϸ��.����, �����˳���ϸ��.ʵ�ƽ�� FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN �ⷿ�� ON �����˳���ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID WHERE (�����˳����ܱ�.BeActive = 1) AND  (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            if (dSet.Tables.Contains("��Ʒ��10")) dSet.Tables.Remove("��Ʒ��10");
            sqlDA.Fill(dSet, "��Ʒ��10");

            sqlComm.CommandText = "SELECT �����˲���ۻ��ܱ�.ID, �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �����˲���ۻ��ܱ�.��˰�ϼ�, ְԱ��.ְԱ���� AS ҵ��Ա, �����˲���ۻ��ܱ�.��ע FROM �����˲���ۻ��ܱ� INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����˲���ۻ��ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            if (dSet.Tables.Contains("��Ʒ��11")) dSet.Tables.Remove("��Ʒ��11");
            sqlDA.Fill(dSet, "��Ʒ��11");


            sqlComm.CommandText = "SELECT �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ְԱ��.ְԱ���� AS ҵ��Ա, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, �����˲������ϸ��.��������, �����˲������ϸ��.��� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����˲���ۻ��ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND  (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            if (dSet.Tables.Contains("��Ʒ��12")) dSet.Tables.Remove("��Ʒ��12");
            sqlDA.Fill(dSet, "��Ʒ��12");

            sqlConn.Close();
            /*
            dataGridView1.DataSource = dSet.Tables["��Ʒ��1"];
            dataGridView1.Columns[0].Visible=false;
            dataGridView4.DataSource = dSet.Tables["��Ʒ��2"];
            dataGridView3.DataSource = dSet.Tables["��Ʒ��3"];
            dataGridView3.Columns[0].Visible=false;

            dataGridView7.DataSource = dSet.Tables["��Ʒ��4"];
            dataGridView6.DataSource = dSet.Tables["��Ʒ��5"];
            dataGridView8.DataSource = dSet.Tables["��Ʒ��6"];
            dataGridView5.DataSource = dSet.Tables["��Ʒ��7"];
            dataGridView2.DataSource = dSet.Tables["��Ʒ��8"];
            dataGridView9.DataSource = dSet.Tables["��Ʒ��9"];
            dataGridView9.Columns[0].Visible = false;
            dataGridView10.DataSource = dSet.Tables["��Ʒ��10"];
            dataGridView1.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridView4.Columns[8].DefaultCellStyle.Format = "f0"; 
            dataGridView7.Columns[9].DefaultCellStyle.Format = "f0";
            dataGridView8.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[8].DefaultCellStyle.Format = "f0"; 
             */
            dataGridView1.DataSource = dSet.Tables["��Ʒ��1"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView4.DataSource = dSet.Tables["��Ʒ��4"];
            dataGridView3.DataSource = dSet.Tables["��Ʒ��3"];
            dataGridView3.Columns[0].Visible = false;

            dataGridView2.DataSource = dSet.Tables["��Ʒ��2"];
            dataGridView5.DataSource = dSet.Tables["��Ʒ��5"];
            dataGridView6.DataSource = dSet.Tables["��Ʒ��6"];
            dataGridView7.DataSource = dSet.Tables["��Ʒ��7"];
            dataGridView8.DataSource = dSet.Tables["��Ʒ��8"];
            dataGridView9.DataSource = dSet.Tables["��Ʒ��9"];
            dataGridView9.Columns[0].Visible = false;
            dataGridView10.DataSource = dSet.Tables["��Ʒ��10"];
            dataGridView11.DataSource = dSet.Tables["��Ʒ��11"];
            dataGridView3.Columns[11].Visible = false;
            dataGridView12.DataSource = dSet.Tables["��Ʒ��12"];
            dataGridView1.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridView4.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView7.Columns[9].DefaultCellStyle.Format = "f0";
            dataGridView8.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[8].DefaultCellStyle.Format = "f0";

            dataGridView2.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView4.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView8.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView10.Columns[9].DefaultCellStyle.Format = "f2";

            dataGridView11.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridView11.Columns[0].Visible = false;
            dataGridView12.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView12.Columns[9].DefaultCellStyle.Format = "f2";
            
            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT="";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "����ҵ���ѯ������δ������ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "����ҵ���ѯ�������Ƶ���ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "����ҵ���ѯ�������Ƶ����ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "����ҵ���ѯ������������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView7, strT, true, intUserLimit);
                    break;
                case 4:
                    strT = "����ҵ���ѯ������������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, true, intUserLimit);
                    break;
                case 5:
                    strT = "����ҵ���ѯ������δ������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView8, strT, true, intUserLimit);
                    break;
                case 6:
                    strT = "����ҵ���ѯ������������ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, true, intUserLimit);
                    break;
                case 7:
                    strT = "����ҵ���ѯ������δ������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
            }

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "����ҵ���ѯ������δ������ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "����ҵ���ѯ�������Ƶ���ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "����ҵ���ѯ�������Ƶ����ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "����ҵ���ѯ������������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView7, strT, false, intUserLimit);
                    break;
                case 4:
                    strT = "����ҵ���ѯ������������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, false, intUserLimit);
                    break;
                case 5:
                    strT = "����ҵ���ѯ������δ������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView8, strT, false, intUserLimit);
                    break;
                case 6:
                    strT = "����ҵ���ѯ������������ܣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, false, intUserLimit);
                    break;
                case 7:
                    strT = "����ҵ���ѯ������δ������ϸ��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
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

        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= cTemp.Length; i++)
            {
                cTemp[i - 1] = 0;
                cTemp1[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 6;c1 = 0;
                        break;
                    case 2:
                        c = 10;c1 = 8;
                        break;
                    case 3:
                        c = 7;c1 = 0;
                        break;
                    case 4:
                        c = 10;c1 = 8;
                        break;
                    case 5:
                        c = 6;c1 = 0;
                        break;
                    case 6:
                        c = 10;c1 = 8;
                        break;
                    case 7:
                        c = 11;c1 = 9;
                        break;
                    case 8:
                        c = 10;c1 = 8;
                        break;
                    case 9:
                        c = 7;c1 = 0;
                        break;
                    case 10:
                        c = 10;c1 = 8;
                        break;
                    case 11:
                        c = 5; c1 = 0;
                        break;
                    case 12:
                        c = 9; c1 = 8;
                        break;
                    default:
                        c = 0;c1 = 0;
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


            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;
            if (!dSet.Tables.Contains("��Ʒ��" + c1.ToString()))
                return;
            toolStripStatusLabelC.Text = "����" + dSet.Tables["��Ʒ��" + c1.ToString()].Rows.Count.ToString() + "����¼ ���ϼ�" + cTemp[tabControl1.SelectedIndex].ToString("f2") + "Ԫ";

            if (cTemp1[tabControl1.SelectedIndex].ToString("f0") != "-1")
                toolStripStatusLabelC.Text += " �����ϼ� " + cTemp1[tabControl1.SelectedIndex].ToString("f0");
        }

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            iSupplyCompany = 0;
            textBoxDWMC.Text = "";
            textBoxDWBH.Text = "";
        }



    }
}