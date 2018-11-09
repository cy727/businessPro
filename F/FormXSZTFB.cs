using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXSZTFB : Form
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

        private int intDays = 0;

        private int intCommID = 0;
        private int iCompanyID = 0;

        private string sDT = "";
        private ClassGetInformation cGetInformation;

        private int[] iCount = { 0, 0, 0 };

        public FormXSZTFB()
        {
            InitializeComponent();
        }

        private void FormXSZTFB_Load(object sender, EventArgs e)
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
            sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            string strA = "", strB = "", strC = "", strD = "", strE="";

            strA = "SELECT ������Ʒ�Ƶ���.��λID, COUNT(DISTINCT ������Ʒ�Ƶ���.ID) AS ��������, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS ���۽�� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strA+=" GROUP BY ������Ʒ�Ƶ���.��λID";

            strB = "SELECT �����˳����ܱ�.��λID, COUNT(DISTINCT �����˳����ܱ�.ID) AS �˳�����, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �˳���� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˳����ܱ�.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB += " GROUP BY �����˳����ܱ�.��λID";
/*
            strC = "SELECT �����տ���ܱ�.��λID, COUNT(*) AS �ؿ�����, SUM(�����տ���ϸ��.������) AS �ؿ��� FROM �����տ���ܱ� INNER JOIN �����տ���ϸ�� ON �����տ���ܱ�.ID = �����տ���ϸ��.����ID INNER JOIN �����տ�ұ� ON �����տ���ϸ��.ID = �����տ�ұ�.����ID INNER JOIN ������Ʒ�Ƶ��� ON �����տ�ұ�.���ݱ�� = ������Ʒ�Ƶ���.���ݱ�� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (�����տ���ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����տ���ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����տ���ܱ�.BeActive=1) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (�����տ���ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (�����տ���ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY �����տ���ܱ�.��λID";
*/
            strC = "SELECT ��λID, COUNT(DISTINCT ���ݱ��) AS �ؿ�����, SUM(�Ѹ�����) AS �ؿ��� FROM �տ���ϸ��ͼ WHERE (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (�Ѹ����� <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC += " AND (�տ���ϸ��ͼ.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (�տ���ϸ��ͼ.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (�տ���ϸ��ͼ.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY ��λID";

            /*
            strD = "SELECT ������Ʒ�Ƶ���.��λID, MIN(������Ʒ�Ƶ���.����) AS ���� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (������Ʒ�Ƶ���.δ������ <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strD += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strD += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strD += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strD += " GROUP BY ������Ʒ�Ƶ���.��λID";
            */

            strD = "SELECT ��λID, MIN(����) AS ����  FROM �տ���ϸ��ͼ WHERE (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (δ������ <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strD += " AND (�տ���ϸ��ͼ.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strD += " AND (�տ���ϸ��ͼ.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strD += " AND (�տ���ϸ��ͼ.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strD += " GROUP BY ��λID";

            strE = "SELECT �����˲���ۻ��ܱ�.��λID, COUNT(DISTINCT �����˲���ۻ��ܱ�.ID) AS ��������, SUM(�����˲������ϸ��.���) AS ���۽�� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strE += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strE += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strE += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strE += " GROUP BY �����˲���ۻ��ܱ�.��λID";

            sqlConn.Open();

            sqlComm.CommandText = "SELECT ��λ��.��λ���, ��λ��.��λ����, ���۱�.���۽��, ���۱�.��������, �˻���.�˳����, �˻���.�˳�����, �ؿ��.�ؿ���, �ؿ��.�ؿ����� , 0 AS Ӧ�����, 0 AS �Ƿ����, ���۱�.��������,���۱�.���۽��, �����.����  FROM ��λ�� LEFT OUTER JOIN (" + strC + ") �ؿ�� ON ��λ��.ID = �ؿ��.��λID LEFT OUTER JOIN (" + strB + ") �˻��� ON ��λ��.ID = �˻���.��λID LEFT OUTER JOIN (" + strA + ") ���۱� ON ��λ��.ID = ���۱�.��λID  LEFT OUTER JOIN (" + strD + ") ����� ON ��λ��.ID = �����.��λID LEFT OUTER JOIN (" + strE + ") ���۱� ON ��λ��.ID = ���۱�.��λID WHERE (��λ��.�Ƿ����� = 1) AND (��λ��.BeActive=1)";

            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                sqlComm.CommandText += " AND (��λ��.ID=" + iCompanyID.ToString() + ")";
            }


            if (dSet.Tables.Contains("��λ��")) dSet.Tables.Remove("��λ��");
            sqlDA.Fill(dSet, "��λ��");


            strA = "SELECT ������Ʒ�Ƶ���.ҵ��ԱID, COUNT(DISTINCT ������Ʒ�Ƶ���.ID) AS ��������, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS ���۽�� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strA += " GROUP BY ������Ʒ�Ƶ���.ҵ��ԱID";

            strB = "SELECT �����˳����ܱ�.ҵ��ԱID, COUNT(DISTINCT �����˳����ܱ�.ID) AS �˳�����, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �˳���� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˳����ܱ�.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB += " GROUP BY �����˳����ܱ�.ҵ��ԱID";
/*
            strC = "SELECT �����տ���ܱ�.ҵ��ԱID, COUNT(*) AS �ؿ�����, SUM(�����տ���ϸ��.������) AS �ؿ��� FROM �����տ���ܱ� INNER JOIN �����տ���ϸ�� ON �����տ���ܱ�.ID = �����տ���ϸ��.����ID INNER JOIN �����տ�ұ� ON �����տ���ϸ��.ID = �����տ�ұ�.����ID INNER JOIN ������Ʒ�Ƶ��� ON �����տ�ұ�.���ݱ�� = ������Ʒ�Ƶ���.���ݱ�� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (�����տ���ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����տ���ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����տ���ܱ�.BeActive=1) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (�����տ���ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (�����տ���ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY �����տ���ܱ�.ҵ��ԱID";
 */
            strC = "SELECT ҵ��ԱID, COUNT(DISTINCT ���ݱ��) AS �ؿ�����, SUM(�Ѹ�����) AS �ؿ��� FROM �տ���ϸ��ͼ WHERE (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (�Ѹ����� <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC += " AND (�տ���ϸ��ͼ.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (�տ���ϸ��ͼ.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (�տ���ϸ��ͼ.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY ҵ��ԱID";
 
            /*
            strD = "SELECT ������Ʒ�Ƶ���.ҵ��ԱID, MIN(������Ʒ�Ƶ���.����) AS ���� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (������Ʒ�Ƶ���.δ������ <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strD += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strD += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strD += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strD += " GROUP BY ������Ʒ�Ƶ���.ҵ��ԱID";
            */
            strD = "SELECT ҵ��ԱID, MIN(����) AS ����  FROM �տ���ϸ��ͼ WHERE (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (δ������ <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strD += " AND (�տ���ϸ��ͼ.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strD += " AND (�տ���ϸ��ͼ.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strD += " AND (�տ���ϸ��ͼ.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strD += " GROUP BY ҵ��ԱID";

            strE = "SELECT �����˲���ۻ��ܱ�.ҵ��ԱID, COUNT(DISTINCT �����˲���ۻ��ܱ�.ID) AS ��������, SUM(�����˲������ϸ��.���) AS ���۽�� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strE += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strE += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strE += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strE += " GROUP BY �����˲���ۻ��ܱ�.ҵ��ԱID";


            sqlComm.CommandText = "SELECT ְԱ��.ְԱ���, ְԱ��.ְԱ����, ������.���۽��, ������.��������,�˿��.�˳����, �˿��.�˳�����, �ؿ��.�ؿ���, �ؿ��.�ؿ�����, 0 AS Ӧ�����, 0 AS �Ƿ����, ���۱�.��������,���۱�.���۽��, �����.���� FROM ְԱ�� LEFT OUTER JOIN (" + strC + ") �ؿ�� ON ְԱ��.ID = �ؿ��.ҵ��ԱID LEFT OUTER JOIN (" + strB + ") �˿�� ON ְԱ��.ID = �˿��.ҵ��ԱID LEFT OUTER JOIN (" + strA + ") ������ ON ְԱ��.ID = ������.ҵ��ԱID  LEFT OUTER JOIN (" + strD + ") ����� ON ְԱ��.ID = �����.ҵ��ԱID LEFT OUTER JOIN (" + strE + ") ���۱� ON ְԱ��.ID = ���۱�.ҵ��ԱID  WHERE (ְԱ��.BeActive=1)";
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (ְԱ��.ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            sqlComm.CommandText += " ORDER BY ְԱ��.ְԱ���";

            if (dSet.Tables.Contains("ְԱ��")) dSet.Tables.Remove("ְԱ��");
            sqlDA.Fill(dSet, "ְԱ��");


            strA = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, COUNT(DISTINCT ������Ʒ�Ƶ���.ID) AS ��������, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS ���۽�� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strA += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

            strB = "SELECT �����˳���ϸ��.��ƷID, COUNT(DISTINCT �����˳����ܱ�.ID) AS �˳�����, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �˳���� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˳����ܱ�.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB += " GROUP BY �����˳���ϸ��.��ƷID";
/*
            strC = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, COUNT(*) AS �ؿ�����, SUM(�����տ���ϸ��.������) AS �ؿ��� FROM �����տ���ܱ� INNER JOIN �����տ���ϸ�� ON �����տ���ܱ�.ID = �����տ���ϸ��.����ID INNER JOIN �����տ�ұ� ON �����տ���ϸ��.ID = �����տ�ұ�.����ID INNER JOIN ������Ʒ�Ƶ��� ON �����տ�ұ�.���ݱ�� = ������Ʒ�Ƶ���.���ݱ�� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (�����տ���ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����տ���ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����տ���ܱ�.BeActive=1) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (�����տ���ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (�����տ���ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";
 * 
 */

            strC = "SELECT ��ƷID, COUNT(DISTINCT ���ݱ��) AS �ؿ�����, SUM(�Ѹ�����) AS �ؿ��� FROM �տ���ϸ��ͼ WHERE (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (�Ѹ����� <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC += " AND (�տ���ϸ��ͼ.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (�տ���ϸ��ͼ.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (�տ���ϸ��ͼ.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY ��ƷID";

            /*
            strD = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, MIN(������Ʒ�Ƶ���.����) AS ���� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (������Ʒ�Ƶ���.δ������ <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strD += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strD += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strD += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strD += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";
             */

            strD = "SELECT ��ƷID, MIN(����) AS ����  FROM �տ���ϸ��ͼ WHERE (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (δ������ <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strD += " AND (�տ���ϸ��ͼ.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strD += " AND (�տ���ϸ��ͼ.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strD += " AND (�տ���ϸ��ͼ.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strD += " GROUP BY ��ƷID";

            strE = "SELECT �����˲������ϸ��.��ƷID, COUNT(DISTINCT �����˲���ۻ��ܱ�.ID) AS ��������, SUM(�����˲������ϸ��.���) AS ���۽�� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strE += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strE += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strE += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strE += " GROUP BY �����˲������ϸ��.��ƷID";
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����,��Ʒ��.��Ʒ���,���۱�.���۽��, ���۱�.��������, �˻���.�˳����, �˻���.�˳�����, �ؿ��.�ؿ���, �ؿ��.�ؿ�����, 0 AS Ӧ�����, 0 AS �Ƿ����, ���۱�.��������,���۱�.���۽��, �����.���� FROM (" + strA + ") ���۱� RIGHT OUTER JOIN ��Ʒ�� ON ���۱�.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strB + ") �˻��� ON ��Ʒ��.ID = �˻���.��ƷID LEFT OUTER JOIN (" + strC + ") �ؿ�� ON ��Ʒ��.ID = �ؿ��.��ƷID LEFT OUTER JOIN (" + strD + ") ����� ON ��Ʒ��.ID = �����.��ƷID LEFT OUTER JOIN (" + strE + ") ���۱� ON ��Ʒ��.ID = ���۱�.��ƷID  WHERE (��Ʒ��.beactive = 1) AND (��Ʒ��.��װ��Ʒ = 0)";
                        
            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");


            sqlConn.Close();

            adjustDataView1();
            dataGridViewDJMX1.DataSource = dSet.Tables["��λ��"];
            dataGridViewDJMX1.Columns[12].Visible = false;
            dataGridViewDJMX1.Columns[2].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX1.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX1.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX1.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX1.Columns[6].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX1.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX1.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX1.Columns[9].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX1.Columns[10].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX1.Columns[11].DefaultCellStyle.Format = "f2";

            //dataGridViewDJMX1.Columns[11].Visible = false;

            dataGridViewDJMX2.DataSource = dSet.Tables["ְԱ��"];
            dataGridViewDJMX2.Columns[12].Visible = false;
            dataGridViewDJMX2.Columns[2].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX2.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX2.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX2.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX2.Columns[6].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX2.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX2.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX2.Columns[9].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX2.Columns[10].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX2.Columns[11].DefaultCellStyle.Format = "f2";

            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[12].DefaultCellStyle.Format = "f2";

            tabControl1_SelectedIndexChanged(null,null);

        }

        private void adjustDataView1()
        {
            int i,j;
            TimeSpan ts;
            decimal[] dSUM = { 0,0,0,0,0,0,0,0,0,0};

        //��λ��
            iCount[0]=0;
            for(i=dSet.Tables["��λ��"].Rows.Count-1;i>=0;i--)
            {
                if (dSet.Tables["��λ��"].Rows[i][2].ToString() == "" && dSet.Tables["��λ��"].Rows[i][3].ToString() == "" && dSet.Tables["��λ��"].Rows[i][4].ToString() == "" && dSet.Tables["��λ��"].Rows[i][5].ToString() == "" && dSet.Tables["��λ��"].Rows[i][6].ToString() == "" && dSet.Tables["��λ��"].Rows[i][7].ToString() == "" && dSet.Tables["��λ��"].Rows[i][10].ToString() == "" && dSet.Tables["��λ��"].Rows[i][11].ToString() == "" && dSet.Tables["��λ��"].Rows[i][12].ToString() == "")
                 {
                     dSet.Tables["��λ��"].Rows[i].Delete();
                 }
                 else
                     iCount[0]++;

            }
            dSet.Tables["��λ��"].AcceptChanges();

            for (i = 0; i < dSUM.Length; i++)
            {
                dSUM[i] = 0;
            }
            for (i = 0; i < dSet.Tables["��λ��"].Rows.Count; i++)
            {

                for (j = 2; j <= 7; j++)
                {
                    if (dSet.Tables["��λ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["��λ��"].Rows[i][j] = 0;

                    dSUM[j - 2] += decimal.Parse(dSet.Tables["��λ��"].Rows[i][j].ToString());
                }

                dSet.Tables["��λ��"].Rows[i][8] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][4].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][6].ToString());

                if (dSet.Tables["��λ��"].Rows[i][12].ToString() == "")
                {
                    dSet.Tables["��λ��"].Rows[i][9] = 0;
                }
                else
                {

                    ts = dateTimePickerE.Value-Convert.ToDateTime(dSet.Tables["��λ��"].Rows[i][12].ToString());
                    if(ts.Days<0)
                        dSet.Tables["��λ��"].Rows[i][9] = 0;
                    else
                        dSet.Tables["��λ��"].Rows[i][9] = ts.Days;
                }

                dSUM[7] = Math.Max(decimal.Parse(dSet.Tables["��λ��"].Rows[i][9].ToString()), dSUM[7]);
                for (j = 10; j <= 11; j++)
                {
                    if (dSet.Tables["��λ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["��λ��"].Rows[i][j] = 0;

                    dSUM[j - 2] += decimal.Parse(dSet.Tables["��λ��"].Rows[i][j].ToString());
                }


            }
            DataRow drT1 = dSet.Tables["��λ��"].NewRow();
            drT1[1] = "�ϼ�";
            for (j = 2; j <= 7; j++)
            {
                drT1[j]=dSUM[j - 2];
            }
            drT1[8] = decimal.Parse(drT1[2].ToString()) - decimal.Parse(drT1[4].ToString()) - decimal.Parse(drT1[6].ToString());
            drT1[9] = dSUM[7];
            for (j = 10; j <= 11; j++)
            {
                drT1[j] = dSUM[j - 2];
            }
            dSet.Tables["��λ��"].Rows.Add(drT1);

            //ְԱ��
            iCount[1] = 0;
            for (i = dSet.Tables["ְԱ��"].Rows.Count - 1; i >= 0; i--)
            {
                if (dSet.Tables["ְԱ��"].Rows[i][2].ToString() == "" && dSet.Tables["ְԱ��"].Rows[i][3].ToString() == "" && dSet.Tables["ְԱ��"].Rows[i][4].ToString() == "" && dSet.Tables["ְԱ��"].Rows[i][5].ToString() == "" && dSet.Tables["ְԱ��"].Rows[i][6].ToString() == "" && dSet.Tables["ְԱ��"].Rows[i][7].ToString() == "" && dSet.Tables["ְԱ��"].Rows[i][10].ToString() == "" && dSet.Tables["ְԱ��"].Rows[i][11].ToString() == "" && dSet.Tables["ְԱ��"].Rows[i][12].ToString() == "")
                {
                    dSet.Tables["ְԱ��"].Rows[i].Delete();
                }
                else
                    iCount[1]++;

            }
            dSet.Tables["ְԱ��"].AcceptChanges();

            for (i = 0; i < dSUM.Length; i++)
            {
                dSUM[i] = 0;
            }
            for (i = 0; i < dSet.Tables["ְԱ��"].Rows.Count; i++)
            {

                for (j = 2; j <= 7; j++)
                {
                    if (dSet.Tables["ְԱ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["ְԱ��"].Rows[i][j] = 0;

                    dSUM[j - 2] += decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][j].ToString());
                }

                dSet.Tables["ְԱ��"].Rows[i][8] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][4].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][6].ToString());

                if (dSet.Tables["ְԱ��"].Rows[i][12].ToString() == "")
                {
                    dSet.Tables["ְԱ��"].Rows[i][9] = 0;
                }
                else
                {

                    ts = dateTimePickerE.Value - Convert.ToDateTime(dSet.Tables["ְԱ��"].Rows[i][12].ToString());
                    if (ts.Days < 0)
                        dSet.Tables["ְԱ��"].Rows[i][9] = 0;
                    else
                        dSet.Tables["ְԱ��"].Rows[i][9] = ts.Days;
                }

                dSUM[7] = Math.Max(decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][9].ToString()), dSUM[7]);
                for (j = 10; j <= 11; j++)
                {
                    if (dSet.Tables["ְԱ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["ְԱ��"].Rows[i][j] = 0;

                    dSUM[j - 2] += decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][j].ToString());
                }
            }
            DataRow drT2 = dSet.Tables["ְԱ��"].NewRow();
            drT2[1] = "�ϼ�";
            for (j = 2; j <= 7; j++)
            {
                drT2[j] = dSUM[j - 2];
            }
            for (j = 10; j <= 11; j++)
            {
                drT2[j] = dSUM[j - 2];
            }
            drT2[8] = decimal.Parse(drT2[2].ToString()) - decimal.Parse(drT2[4].ToString()) - decimal.Parse(drT2[6].ToString());
            drT2[9] = dSUM[7];
            dSet.Tables["ְԱ��"].Rows.Add(drT2);


            //��Ʒ��
            iCount[2] = 0;
            for (i = dSet.Tables["��Ʒ��"].Rows.Count - 1; i >= 0; i--)
            {
                if (dSet.Tables["��Ʒ��"].Rows[i][8].ToString() == "" && dSet.Tables["��Ʒ��"].Rows[i][3].ToString() == "" && dSet.Tables["��Ʒ��"].Rows[i][4].ToString() == "" && dSet.Tables["��Ʒ��"].Rows[i][5].ToString() == "" && dSet.Tables["��Ʒ��"].Rows[i][6].ToString() == "" && dSet.Tables["��Ʒ��"].Rows[i][7].ToString() == "" && dSet.Tables["��Ʒ��"].Rows[i][11].ToString() == "" && dSet.Tables["��Ʒ��"].Rows[i][12].ToString() == "" && dSet.Tables["��Ʒ��"].Rows[i][13].ToString() == "")
                {
                    dSet.Tables["��Ʒ��"].Rows[i].Delete();
                }
                else
                    iCount[2]++;

            }
            dSet.Tables["��Ʒ��"].AcceptChanges();

            for (i = 0; i < dSUM.Length; i++)
            {
                dSUM[i] = 0;
            }
            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {

                for (j = 3; j <= 8; j++)
                {
                    if (dSet.Tables["��Ʒ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["��Ʒ��"].Rows[i][j] = 0;

                    dSUM[j - 3] += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][j].ToString());
                }

                dSet.Tables["��Ʒ��"].Rows[i][9] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][5].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][7].ToString());

                if (dSet.Tables["��Ʒ��"].Rows[i][13].ToString() == "")
                {
                    dSet.Tables["��Ʒ��"].Rows[i][10] = 0;
                }
                else
                {

                    ts = dateTimePickerE.Value - Convert.ToDateTime(dSet.Tables["��Ʒ��"].Rows[i][13].ToString());
                    if (ts.Days < 0)
                        dSet.Tables["��Ʒ��"].Rows[i][10] = 0;
                    else
                        dSet.Tables["��Ʒ��"].Rows[i][10] = ts.Days;
                }

                dSUM[7] = Math.Max(decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][10].ToString()), dSUM[7]);
                for (j = 11; j <= 12; j++)
                {
                    if (dSet.Tables["��Ʒ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["��Ʒ��"].Rows[i][j] = 0;

                    dSUM[j - 3] += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][j].ToString());
                }
            }
            DataRow drT3 = dSet.Tables["��Ʒ��"].NewRow();
            drT3[2] = "�ϼ�";
            for (j = 3; j <= 8; j++)
            {
                drT3[j] = dSUM[j - 3];
            }
            drT3[9] = decimal.Parse(drT3[3].ToString()) - decimal.Parse(drT3[5].ToString()) - decimal.Parse(drT3[7].ToString());
            drT3[10] = dSUM[7];
            for (j = 11; j <= 12; j++)
            {
                drT3[j] = dSUM[j - 3];
            }
            dSet.Tables["��Ʒ��"].Rows.Add(drT3);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "����״̬�ֲ���������λ����״̬��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDJMX1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "����״̬�ֲ���ҵ��Ա����״̬��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDJMX2, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "����״̬�ֲ�����Ӫ��Ʒ����״̬��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
                    break;

            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "����״̬�ֲ���������λ����״̬��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDJMX1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "����״̬�ֲ���ҵ��Ա����״̬��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDJMX2, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "����״̬�ֲ�����Ӫ��Ʒ����״̬��;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
                    break;

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
                }

            }
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                iCompanyID = cGetInformation.iCompanyNumber;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1100, textBoxDWBH.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    iCompanyID = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }

            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1300, textBoxDWMC.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    iCompanyID = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }

            }
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabelMXJLS.Text = iCount[tabControl1.SelectedIndex].ToString();
        }

    }
}