using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXSFLCX : Form
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
        private decimal[] cTemp = new decimal[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private decimal[] cTemp1 = new decimal[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private int intClassID = 0;

        public int LIMITACCESS = 18;

        public FormXSFLCX()
        {
            InitializeComponent();
        }

        private void FormXSFLCX_Load(object sender, EventArgs e)
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
                }
                catch
                {
                    LIMITACCESS = 18;
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
            sqlConn.Close();

            //��ʼ��Ա���б�
            sqlComm.CommandText = "SELECT ID, ְԱ���, ְԱ���� FROM ְԱ�� WHERE (beactive = 1)";

            if (dSet.Tables.Contains("ְԱ��")) dSet.Tables.Remove("ְԱ��");
            sqlDA.Fill(dSet, "ְԱ��");

            if (dSet.Tables.Contains("ְԱ��1")) dSet.Tables.Remove("ְԱ��1");
            sqlDA.Fill(dSet, "ְԱ��1");

            object[] OTemp = new object[3];
            OTemp[0] = 0;
            OTemp[1] = "ȫ��";
            OTemp[2] = "ȫ��";
            dSet.Tables["ְԱ��"].Rows.Add(OTemp);

            object[] OTemp1 = new object[3];
            OTemp1[0] = 0;
            OTemp1[1] = "ȫ��";
            OTemp1[2] = "ȫ��";
            dSet.Tables["ְԱ��1"].Rows.Add(OTemp);


            comboBoxYWY.DataSource = dSet.Tables["ְԱ��"];
            comboBoxYWY.DisplayMember = "ְԱ����";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.SelectedIndex = comboBoxYWY.Items.Count - 1;

            comboBoxCZY.DataSource = dSet.Tables["ְԱ��1"];
            comboBoxCZY.DisplayMember = "ְԱ����";
            comboBoxCZY.ValueMember = "ID";
            comboBoxCZY.SelectedIndex = comboBoxCZY.Items.Count - 1;
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

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
                checkBoxSYDW.Checked = false;
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
                    checkBoxSYDW.Checked = false;
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
                    checkBoxSYDW.Checked = false;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i;
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                cGetInformation.getUnderClassInformation(intClassID);
            }
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, SUM(ë����ͼ.����) AS ����, SUM(ë����ͼ.���) AS ���, SUM(ë����ͼ.ë��) AS ë��, ��Ʒ��.������ FROM ë����ͼ INNER JOIN ��Ʒ�� ON ë����ͼ.��ƷID = ��Ʒ��.ID WHERE (ë����ͼ.BeActive = 1) AND (ë����ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (ë����ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (ë����ͼ.��λID = " + iSupplyCompany.ToString() + ")";
            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.ҵ��ԱID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.����ԱID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                sqlComm.CommandText += " AND ((��Ʒ��.������ = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber;i++)
                    sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }

            sqlComm.CommandText += " GROUP BY ��Ʒ��.������, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ë����ͼ.ë��";



            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");


            sqlComm.CommandText = "SELECT ��λ��.��λ���, ��λ��.��λ����, SUM(ë����ͼ.����) AS ����, SUM(ë����ͼ.���) AS ���, SUM(ë����ͼ.ë��) AS ë�� FROM ë����ͼ INNER JOIN ��λ�� ON ë����ͼ.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ë����ͼ.��ƷID = ��Ʒ��.ID WHERE (ë����ͼ.BeActive = 1) AND (ë����ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (ë����ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (ë����ͼ.��λID = " + iSupplyCompany.ToString() + ")";
            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.ҵ��ԱID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.����ԱID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                sqlComm.CommandText += " AND ((��Ʒ��.������ = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY ë����ͼ.��λID, ��λ��.��λ���, ��λ��.��λ����";

            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");

            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, SUM(ë����ͼ.����) AS ����, SUM(ë����ͼ.���) AS ���, SUM(ë����ͼ.ë��) AS ë�� FROM ë����ͼ INNER JOIN ��Ʒ�� ON ë����ͼ.��ƷID = ��Ʒ��.ID WHERE (ë����ͼ.BeActive = 1) AND (ë����ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (ë����ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (ë����ͼ.��λID = " + iSupplyCompany.ToString() + ")";
            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.ҵ��ԱID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.����ԱID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                sqlComm.CommandText += " AND ((��Ʒ��.������ = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY ë����ͼ.��ƷID, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����";

            if (dSet.Tables.Contains("��Ʒ��3")) dSet.Tables.Remove("��Ʒ��3");
            sqlDA.Fill(dSet, "��Ʒ��3");

            sqlComm.CommandText = "SELECT ��λ��.��λ���, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����,SUM(ë����ͼ.����) AS ����, SUM(ë����ͼ.���) AS ���, SUM(ë����ͼ.ë��) AS ë�� FROM ë����ͼ INNER JOIN ��Ʒ�� ON ë����ͼ.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON ë����ͼ.��λID = ��λ��.ID WHERE (ë����ͼ.BeActive = 1) AND (ë����ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (ë����ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (ë����ͼ.��λID = " + iSupplyCompany.ToString() + ")";
            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.ҵ��ԱID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.����ԱID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                sqlComm.CommandText += " AND ((��Ʒ��.������ = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY ë����ͼ.��ƷID, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��λ��.��λ���, ��λ��.��λ����, ë����ͼ.��λID ";

            if (dSet.Tables.Contains("��Ʒ��4")) dSet.Tables.Remove("��Ʒ��4");
            sqlDA.Fill(dSet, "��Ʒ��4");

            sqlComm.CommandText = "SELECT ְԱ��.ְԱ���� AS ҵ��Ա, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, SUM(ë����ͼ.����) AS ����, SUM(ë����ͼ.���) AS ���, SUM(ë����ͼ.ë��) AS ë�� FROM ë����ͼ INNER JOIN ��Ʒ�� ON ë����ͼ.��ƷID = ��Ʒ��.ID INNER JOIN ְԱ�� ON ë����ͼ.ҵ��ԱID = ְԱ��.ID WHERE (ë����ͼ.BeActive = 1) AND (ë����ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (ë����ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (ë����ͼ.��λID = " + iSupplyCompany.ToString() + ")";
            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.ҵ��ԱID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.����ԱID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                sqlComm.CommandText += " AND ((��Ʒ��.������ = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY ë����ͼ.��ƷID, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ë����ͼ.ҵ��ԱID, ְԱ��.ְԱ���� ";

            if (dSet.Tables.Contains("��Ʒ��5")) dSet.Tables.Remove("��Ʒ��5");
            sqlDA.Fill(dSet, "��Ʒ��5");

            sqlComm.CommandText = "SELECT ë����ͼ.��ID, ë����ͼ.���ݱ��, ë����ͼ.����, ��λ��.��λ���, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, N'', N'', ë����ͼ.����,ë����ͼ.���, ë����ͼ.ë�� FROM ë����ͼ INNER JOIN ��Ʒ�� ON ë����ͼ.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON ��λ��.ID = ë����ͼ.��λID WHERE (ë����ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (ë����ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (ë����ͼ.BeActive = 1) ";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.ҵ��ԱID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.����ԱID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if(iSupplyCompany!=0)
                sqlComm.CommandText += " AND (��λ��.ID = " + iSupplyCompany.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                sqlComm.CommandText += " AND ((��Ʒ��.������ = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }




            if (dSet.Tables.Contains("��Ʒ��6")) dSet.Tables.Remove("��Ʒ��6");
            sqlDA.Fill(dSet, "��Ʒ��6");

            sqlComm.CommandText = "SELECT ְԱ��.ְԱ���� , A.�۵���, SUM(ë����ͼ.����) AS ����,SUM(ë����ͼ.���) AS ���,SUM(ë����ͼ.ë��) AS ë�� FROM ë����ͼ INNER JOIN ��Ʒ�� ON ë����ͼ.��ƷID = ��Ʒ��.ID INNER JOIN ְԱ�� ON ë����ͼ.ҵ��ԱID = ְԱ��.ID INNER JOIN (SELECT ë����ͼ.ҵ��ԱID, COUNT(*) AS �۵��� FROM ë����ͼ INNER JOIN ��Ʒ�� ON ë����ͼ.��ƷID = ��Ʒ��.ID WHERE (ë����ͼ.BeActive = 1) AND (ë����ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (ë����ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.ҵ��ԱID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.����ԱID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND (ë����ͼ.��λID = " + iSupplyCompany.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                sqlComm.CommandText += " AND ((��Ʒ��.������ = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }

            sqlComm.CommandText += " GROUP BY ҵ��ԱID) A ON ë����ͼ.ҵ��ԱID = A.ҵ��ԱID WHERE (ë����ͼ.BeActive = 1) AND (ë����ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (ë����ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.ҵ��ԱID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.����ԱID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND (ë����ͼ.��λID = " + iSupplyCompany.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                sqlComm.CommandText += " AND ((��Ʒ��.������ = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY ë����ͼ.ҵ��ԱID, ְԱ��.ְԱ����, A.�۵��� ORDER BY ְԱ��.ְԱ���� ";


            if (dSet.Tables.Contains("��Ʒ��7")) dSet.Tables.Remove("��Ʒ��7");
            sqlDA.Fill(dSet, "��Ʒ��7");


            sqlComm.CommandText = "SELECT ���ű�.��������, A.�۵���, SUM(ë����ͼ.����) AS ����,SUM(ë����ͼ.���) AS ���, SUM(ë����ͼ.ë��) AS ë�� FROM ë����ͼ INNER JOIN ��Ʒ�� ON ë����ͼ.��ƷID = ��Ʒ��.ID INNER JOIN ���ű� ON ë����ͼ.����ID = ���ű�.ID RIGHT OUTER JOIN (SELECT ë����ͼ.����ID, COUNT(*) AS �۵��� FROM ë����ͼ INNER JOIN ��Ʒ�� ON ë����ͼ.��ƷID = ��Ʒ��.ID WHERE (ë����ͼ.BeActive = 1) AND (ë����ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (ë����ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.ҵ��ԱID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.����ԱID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND (ë����ͼ.��λID = " + iSupplyCompany.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                sqlComm.CommandText += " AND ((��Ʒ��.������ = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }

            sqlComm.CommandText += " GROUP BY ë����ͼ.����ID) A ON ë����ͼ.����ID = A.����ID WHERE (ë����ͼ.BeActive = 1) AND (ë����ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (ë����ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.ҵ��ԱID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (ë����ͼ.����ԱID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND (ë����ͼ.��λID = " + iSupplyCompany.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //����
            {
                sqlComm.CommandText += " AND ((��Ʒ��.������ = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY ë����ͼ.����ID, ���ű�.��������, A.�۵��� ORDER BY ���ű�.�������� ";


            if (dSet.Tables.Contains("��Ʒ��8")) dSet.Tables.Remove("��Ʒ��8");
            sqlDA.Fill(dSet, "��Ʒ��8");


            sqlConn.Close();
            adjustDataView1();
            dataGridView2.DataSource = dSet.Tables["��Ʒ��2"];
            dataGridView2.Columns[2].DefaultCellStyle.Format = "f0"; 
            dataGridView3.DataSource = dSet.Tables["��Ʒ��3"];
            dataGridView3.Columns[2].DefaultCellStyle.Format = "f0"; 
            dataGridView4.DataSource = dSet.Tables["��Ʒ��4"];
            dataGridView4.Columns[4].DefaultCellStyle.Format = "f0"; 
            dataGridView5.DataSource = dSet.Tables["��Ʒ��5"];
            dataGridView5.Columns[3].DefaultCellStyle.Format = "f0"; 
            dataGridView6.DataSource = dSet.Tables["��Ʒ��6"];
            dataGridView6.Columns[9].DefaultCellStyle.Format = "f0"; 
            dataGridView6.Columns[0].Visible = false;
            dataGridView6.Columns[7].Visible = false;
            dataGridView6.Columns[8].Visible = false;
            dataGridView6.Columns[9].Visible = false;
            dataGridView7.DataSource = dSet.Tables["��Ʒ��7"];
            dataGridView7.Columns[1].DefaultCellStyle.Format = "f0";
            dataGridView7.Columns[2].DefaultCellStyle.Format = "f0";

            dataGridView8.DataSource = dSet.Tables["��Ʒ��8"];
            dataGridView8.Columns[1].DefaultCellStyle.Format = "f0";
            dataGridView8.Columns[2].DefaultCellStyle.Format = "f0";
 

            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);

        }

        private void adjustDataView1()
        {
            int i;

            for (i = 0; i < dSet.Tables["��Ʒ��1"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��1"].Rows[i][2].ToString() == "")
                    dSet.Tables["��Ʒ��1"].Rows[i][2] = 0;
                if (dSet.Tables["��Ʒ��1"].Rows[i][3].ToString() == "")
                    dSet.Tables["��Ʒ��1"].Rows[i][3] = 0;
                if (dSet.Tables["��Ʒ��1"].Rows[i][5].ToString() == "")
                    dSet.Tables["��Ʒ��1"].Rows[i][5] = 0;

            }

            int j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[3];
            decimal[] dSum1 = new decimal[3];

            for (t = 0; t < dSum1.Length; t++)
            {
                dSum1[t] = 0;
            }


            sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� ORDER BY �ϼ�����";
            if (dSet.Tables.Contains("��Ʒ�����")) dSet.Tables.Remove("��Ʒ�����");
            sqlDA.Fill(dSet, "��Ʒ�����");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("������", System.Type.GetType("System.String"));
            dTable.Columns.Add("��������", System.Type.GetType("System.String"));
            dTable.Columns.Add("����", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("���", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("ë��", System.Type.GetType("System.Decimal"));


            DataRow[] dtC = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[5];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 2; t < oTemp.Length; t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[5];
                    oTemp1[0] = dtC1[j][1];
                    oTemp1[1] = "����" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    //oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;
                    for (t = 2; t < oTemp1.Length; t++)
                        oTemp1[t] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["��Ʒ��1"].Select("������ = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 2].ToString());

                    }

                    for (t = 2; t < dSum.Length + 2; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length + 2; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }

            object[] oTemp3 = new object[5];
            oTemp3[0] = "�ϼ�";
            oTemp3[1] = "";
            for (t = 2; t < oTemp3.Length; t++)
                oTemp3[t] = dSum1[t - 2];
            dTable.Rows.Add(oTemp3);


            dataGridView1.DataSource = dTable;
            dataGridView1.Columns[2].DefaultCellStyle.Format = "f0"; 


        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "������Ʒ�������;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "���۵�λ����;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "������Ʒ����;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "���۵�λ��Ʒ����;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, true, intUserLimit);
                    break;
                case 4:
                    strT = "����ҵ��Ա��Ʒ����;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, true, intUserLimit);
                    break;
                case 5:
                    strT = "������ϸ;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, true, intUserLimit);
                    break;
                case 6:
                    strT = "����ҵ��Ա����;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView7, strT, true, intUserLimit);
                    break;
                case 7:
                    strT = "���۲��Ż���;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView7, strT, true, intUserLimit);
                    break;

            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "������Ʒ�������;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "���۵�λ����;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "������Ʒ����;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "���۵�λ��Ʒ����;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, false, intUserLimit);
                    break;
                case 4:
                    strT = "����ҵ��Ա��Ʒ����;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, false, intUserLimit);
                    break;
                case 5:
                    strT = "������ϸ;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, false, intUserLimit);
                    break;
                case 6:
                    strT = "����ҵ��Ա����;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView7, strT, false, intUserLimit);
                    break;
                case 7:
                    strT = "���۲��Ż���;" + toolStripStatusLabelC.Text + " ����:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView7, strT, false, intUserLimit);
                    break;


            }
        }

        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= 8; i++)
            {
                cTemp[i - 1] = 0;
                cTemp1[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 0;
                        c1 = 0;
                        break;
                    case 2:
                        c = 3;
                        c1 = 4;
                        break;
                    case 3:
                        c = 3;
                        c1 = 4;
                        break;
                    case 4:
                        c = 5;
                        c1 = 6;
                        break;
                    case 5:
                        c = 4;
                        c1 = 5;
                        break;
                    case 6:
                        c = 10;
                        c1 = 11;
                        break;
                    case 7:
                        c = 3;
                        c1 = 4;
                        break;
                    case 8:
                        c = 3;
                        c1 = 4;
                        break;
                    default:
                        c = 0;
                        c1 = 0;
                        break;
                }

                if (c != 0)
                {

                    for (j = 0; j < dSet.Tables["��Ʒ��" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp[i - 1] += Convert.ToDecimal(dSet.Tables["��Ʒ��" + i.ToString()].Rows[j][c].ToString());
                            cTemp1[i - 1] += Convert.ToDecimal(dSet.Tables["��Ʒ��" + i.ToString()].Rows[j][c1].ToString());
                        }
                        catch
                        {
                        }
                    }
                }


            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;

            if (!dSet.Tables.Contains("��Ʒ��" + c1.ToString())) 
                return;


            if (c1 != 1)
                toolStripStatusLabelC.Text = "����" + dSet.Tables["��Ʒ��" + c1.ToString()].Rows.Count.ToString() + "����¼ ���ϼ�" + cTemp[tabControl1.SelectedIndex].ToString("f2") + "Ԫ ����ϼ�" + cTemp1[tabControl1.SelectedIndex].ToString("f2") + "Ԫ";
            else
                toolStripStatusLabelC.Text = "";
        }

        private void dataGridView6_DoubleClick(object sender, EventArgs e)
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

        private void checkBoxSYDW_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSYDW.Checked)
            {
                textBoxDWBH.Text = "";
                textBoxDWMC.Text = "";
                iSupplyCompany = 0;
            }
        }

        private void btnBY_Click(object sender, EventArgs e)
        {
            System.Globalization.GregorianCalendar cGregorianCalendar=new System.Globalization.GregorianCalendar();

            dateTimePickerS.Value = DateTime.Parse(System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-1 00:00:00");
            dateTimePickerE.Value = DateTime.Parse(System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + cGregorianCalendar.GetDaysInMonth(System.DateTime.Now.Year,System.DateTime.Now.Month).ToString()+" 23:59:59");

        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            DateTime dt = System.DateTime.Now.AddMonths(-1);

            System.Globalization.GregorianCalendar cGregorianCalendar = new System.Globalization.GregorianCalendar();

            dateTimePickerS.Value = DateTime.Parse(System.DateTime.Now.AddMonths(-1).Year.ToString() + "-" + System.DateTime.Now.AddMonths(-1).Month.ToString() + "-1 00:00:00");
            dateTimePickerE.Value = DateTime.Parse(System.DateTime.Now.AddMonths(-1).Year.ToString() + "-" + System.DateTime.Now.AddMonths(-1).Month.ToString() + "-" + cGregorianCalendar.GetDaysInMonth(System.DateTime.Now.AddMonths(-1).Year, System.DateTime.Now.AddMonths(-1).Month).ToString() + " 23:59:59");
        }

        private void textBoxSPLB_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getClassInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                intClassID = cGetInformation.iClassNumber;
                textBoxSPLB.Text = cGetInformation.strClassName;
                checkBoxALL.Checked = false;

            }
        }

        private void textBoxSPLB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getClassInformation(10, textBoxSPLB.Text) == 0) //ʧ��
                {
                    textBoxSPLB.Text = "";
                    intClassID = 0;
                    checkBoxALL.Checked = true;

                }
                else
                {
                    intClassID = cGetInformation.iClassNumber;
                    textBoxSPLB.Text = cGetInformation.strClassName;
                    checkBoxALL.Checked = false;
                }
            }
        }

 
    }
}