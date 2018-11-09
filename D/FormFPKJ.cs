using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormFPKJ : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private int iSupplyCompany = 0;

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;

        public FormFPKJ()
        {
            InitializeComponent();
        }

        private void FormFPKJ_Load(object sender, EventArgs e)
        {
            int i;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            if (isSaved)
            {
                //dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                initDJ();
                return;
            }

            sqlConn.Open();

            //�õ���ʼʱ��
            sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();

            //��ʼ�������б�
            /*
            sqlComm.CommandText = "SELECT ��Ʊ��ϸ��.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.��˰�ϼ� AS ԭ��Ʊ�ܶ�, ��Ʊ��ϸ��.��Ʊ�ܶ�, ��Ʊ��ϸ��.������ʽ, ��Ʊ��ϸ��.����, ��Ʊ��ϸ��.����ID, ��Ʊ��ϸ��.��ע1, ��Ʊ��ϸ��.��ע2 FROM ��Ʊ��ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ��Ʊ��ϸ��.����ID = ������Ʒ�Ƶ���.ID WHERE (��Ʊ��ϸ��.ID = 0)";

            if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
            sqlDA.Fill(dSet, "������ϸ��");
            dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[6].Visible = false;
            dataGridViewDJMX.Columns[2].ReadOnly = true;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;
             */
            DataTable dTable = new DataTable();
            dTable.Columns.Add("����ID", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("���ID", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("���ݱ��", System.Type.GetType("System.String"));
            dTable.Columns.Add("��ֱ��", System.Type.GetType("System.String"));
            dTable.Columns.Add("ԭ��Ʊ�ܶ�", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("��Ʊ�ܶ�", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("������ʽ", System.Type.GetType("System.String"));
            dTable.Columns.Add("����", System.Type.GetType("System.String"));
            dTable.Columns.Add("��ע1", System.Type.GetType("System.String"));
            dTable.Columns.Add("��ע2", System.Type.GetType("System.String"));
            dataGridViewDJMX.DataSource = dTable;

            sqlConn.Close();

            //initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            comboBoxStyle.SelectedIndex = 0;
            comboBoxGD.SelectedIndex = 0;

        }


        private void initDJ()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʊ���ܱ�.��Ʊ��, ��Ʊ���ܱ�.����, ����Ա.ְԱ����, ��Ʊ���ܱ�.��ע, ��λ��.��λ���, ��λ��.��λ����, ��Ʊ���ܱ�.������ʽ, ��Ʊ���ܱ�.����, ��Ʊ���ܱ�.ԭ��Ʊ���, ��Ʊ���ܱ�.��Ʊ�ܶ�, ��Ʊ���ܱ�.��Ʊ���� FROM ��Ʊ���ܱ� INNER JOIN ��λ�� ON ��Ʊ���ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ����Ա ON ��Ʊ���ܱ�.����ԱID = ����Ա.ID WHERE (��Ʊ���ܱ�.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                textBoxFPH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy��M��dd��");

                labelCZY.Text = sqldr.GetValue(2).ToString();
                textBoxBZ.Text = sqldr.GetValue(3).ToString();
                textBoxDWBH.Text = sqldr.GetValue(4).ToString();
                textBoxDWMC.Text = sqldr.GetValue(5).ToString();

                comboBoxFHFS.Text = sqldr.GetValue(6).ToString();
                textBoxDH.Text = sqldr.GetValue(7).ToString();
                comboBoxStyle.SelectedIndex = Convert.ToInt32(sqldr.GetValue(10).ToString());

                this.Text = "��Ʊ���ߣ�" + textBoxFPH.Text;
            }
            sqldr.Close();

            //��ʼ����ϸ�б�
            comboBoxGD.SelectedIndex = 0;
            sqlComm.CommandText = "SELECT ����ID, ���ID, ���ݱ��, ��ֱ��, ԭ��Ʊ�ܶ�, ��Ʊ�ܶ�, ������ʽ, ����, ��ע1, ��ע2 FROM ��Ʊ��ϸ�� WHERE (��ƱID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
            sqlDA.Fill(dSet, "������ϸ��");
            dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[1].Visible = false;
           
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;

            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;


            sqlConn.Close();
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

                if (dSet.Tables.Contains("���յ��ݹ���"))
                    dSet.Tables.Remove("���յ��ݹ���");
                if (dSet.Tables.Contains("���յ�����ϸ����"))
                    dSet.Tables.Remove("���յ�����ϸ����");
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
                    if (dSet.Tables.Contains("���յ��ݹ���"))
                        dSet.Tables.Remove("���յ��ݹ���");
                    if (dSet.Tables.Contains("���յ�����ϸ����"))
                        dSet.Tables.Remove("���յ�����ϸ����");
                }
            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1300, textBoxDWMC.Text.Trim()) == 0)
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
                    if (dSet.Tables.Contains("���յ��ݹ���"))
                        dSet.Tables.Remove("���յ��ݹ���");
                    if (dSet.Tables.Contains("���յ�����ϸ����"))
                        dSet.Tables.Remove("���յ�����ϸ����");
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i;

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("��ѡ����Ӧ��Ʊ��λ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable dTable = new DataTable();
            dTable.Columns.Add("����ID", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("���ID", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("���ݱ��", System.Type.GetType("System.String"));
            dTable.Columns.Add("��ֱ��", System.Type.GetType("System.String"));
            dTable.Columns.Add("ԭ��Ʊ�ܶ�", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("��Ʊ�ܶ�", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("������ʽ", System.Type.GetType("System.String"));
            dTable.Columns.Add("����", System.Type.GetType("System.String"));
            dTable.Columns.Add("��ע1", System.Type.GetType("System.String"));
            dTable.Columns.Add("��ע2", System.Type.GetType("System.String"));


            switch (comboBoxStyle.SelectedIndex)
            {
                case 0: //����

                    switch (comboBoxGD.SelectedIndex)
                    {
                        case 0: //���յ���

                            if (dSet.Tables.Contains("���յ��ݹ���"))  //��ʼ�����ݹ����б�
                                dSet.Tables.Remove("���յ��ݹ���");
                            sqlConn.Open();
                            sqlComm.CommandText = "(SELECT ��Ʊ�����.ѡ��, ���������ܱ�.���ݱ��,������Ʒ�Ƶ���.���ݱ�� AS ��ֵ���, ���������ܱ�.��˰�ϼ�, ���������ܱ�.ID, ���������ܱ�.����ID,���������ܱ�.��˰�ϼ� AS ��Ʊ�ܶ�, ���������ܱ�.����, '' AS ��ע1, '' AS ��ע2 FROM ���������ܱ� INNER JOIN ������Ʒ�Ƶ��� ON ���������ܱ�.����ID = ������Ʒ�Ƶ���.ID CROSS JOIN ��Ʊ����� WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� >= CONVERT(DATETIME,  '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.��λID = " + iSupplyCompany.ToString() + ") AND ((���������ܱ�.��Ʊ�� IS NULL) OR (���������ܱ�.��Ʊ�� = N''))) UNION (SELECT ��Ʊ�����.ѡ��, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.���ݱ�� AS [AS ��ֵ���], -1*�����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.ID, �����˳����ܱ�.ID AS Expr1, -1*�����˳����ܱ�.��˰�ϼ� AS ��Ʊ�ܶ�, �����˳����ܱ�.����, '' AS ��ע1, '' AS ��ע2  FROM ��Ʊ����� CROSS JOIN �����˳����ܱ� WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + "  23:59:59', 102)) AND (�����˳����ܱ�.��λID = " + iSupplyCompany.ToString() + ") AND (�����˳����ܱ�.��Ʊ�� IS NULL OR �����˳����ܱ�.��Ʊ�� = N'')) UNION (SELECT ��Ʊ�����.ѡ��, �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.���ݱ�� AS [AS ��ֵ���], �����˲���ۻ��ܱ�.��˰�ϼ�, �����˲���ۻ��ܱ�.ID, �����˲���ۻ��ܱ�.ID AS Expr1, �����˲���ۻ��ܱ�.��˰�ϼ� AS ��Ʊ�ܶ�, �����˲���ۻ��ܱ�.����, '' AS ��ע1, '' AS ��ע2  FROM ��Ʊ����� CROSS JOIN �����˲���ۻ��ܱ� WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + "  23:59:59', 102)) AND (�����˲���ۻ��ܱ�.��λID = " + iSupplyCompany.ToString() + ") AND (�����˲���ۻ��ܱ�.��Ʊ�� IS NULL OR �����˲���ۻ��ܱ�.��Ʊ�� = N''))";
                            sqlDA.Fill(dSet, "���յ��ݹ���");

                            sqlConn.Close();
                            

                            FormSelectGDFP frmSelectGDFP=new FormSelectGDFP();
                            frmSelectGDFP.iSelectStyle = 1;

                            frmSelectGDFP.dtSelect = dSet.Tables["���յ��ݹ���"];
                            frmSelectGDFP.ShowDialog();

                            if (true)//frmSelectGDFP.dSUMJE >= 0)
                            {
                                /*
                                DataRow []dr=dSet.Tables["���յ��ݹ���"].Select("ѡ��=1");


                                for (i = 0; i < dr.Length; i++)
                                {
                                    object[] oTemp = new object[10];
                                    oTemp[0] = dr[i][4];
                                    oTemp[1] = dr[i][5];
                                    oTemp[2] = dr[i][1]; 
                                    oTemp[3] = dr[i][2]; 
                                    oTemp[4] = dr[i][3]; 
                                    oTemp[5] = dr[i][6]; 
                                    oTemp[6] = ""; 
                                    oTemp[7] = "";
                                    oTemp[8] = "";
                                    oTemp[9] = "";
                                    dTable.Rows.Add(oTemp);
                                }
                                */

                                for (i = 0; i < dSet.Tables["���յ��ݹ���"].Rows.Count; i++)
                                {
                                    if (bool.Parse(dSet.Tables["���յ��ݹ���"].Rows[i][0].ToString()))
                                    {
                                        object[] oTemp = new object[10];
                                        oTemp[0] = dSet.Tables["���յ��ݹ���"].Rows[i][4];
                                        oTemp[1] = dSet.Tables["���յ��ݹ���"].Rows[i][5];
                                        oTemp[2] = dSet.Tables["���յ��ݹ���"].Rows[i][1];
                                        oTemp[3] = dSet.Tables["���յ��ݹ���"].Rows[i][2];
                                        oTemp[4] = dSet.Tables["���յ��ݹ���"].Rows[i][3];
                                        oTemp[5] = dSet.Tables["���յ��ݹ���"].Rows[i][6];
                                        oTemp[6] = "";
                                        oTemp[7] = "";
                                        oTemp[8] = "";
                                        oTemp[9] = "";
                                        dTable.Rows.Add(oTemp);
                                    }

                                }
                                dataGridViewDJMX.DataSource = dTable;
                                
                                dataGridViewDJMX.Columns[0].Visible = false;
                                dataGridViewDJMX.Columns[1].Visible = false;
                                dataGridViewDJMX.Columns[6].Visible = false;
                                dataGridViewDJMX.Columns[7].Visible = false;
                                dataGridViewDJMX.Columns[2].Visible = true;
                                dataGridViewDJMX.Columns[3].Visible = true;
                                dataGridViewDJMX.Columns[4].Visible = true;
                                dataGridViewDJMX.Columns[5].Visible = true;
                                dataGridViewDJMX.Columns[8].Visible = true;
                                dataGridViewDJMX.Columns[9].Visible = true;
                                dataGridViewDJMX.Columns[2].ReadOnly = true;
                                dataGridViewDJMX.Columns[3].ReadOnly = true;
                                dataGridViewDJMX.Columns[4].ReadOnly = true;
                               // dataGridViewDJMX.Columns[5].ReadOnly = true;

                                dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.AllowUserToAddRows = false;
                                dataGridViewDJMX.AllowUserToDeleteRows = false;
                            }

                            break;
                        case 1://������Ʒ

                            if (dSet.Tables.Contains("���յ�����ϸ����"))  //��ʼ�����ݹ����б�
                                dSet.Tables.Remove("���յ�����ϸ����");
                            sqlConn.Open();
                            sqlComm.CommandText = "SELECT ��Ʊ�����.ѡ��, ���������ܱ�.���ݱ��, ������Ʒ�Ƶ���.���ݱ�� AS ��ֵ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ���������ϸ��.ʵ�ƽ��, ���������ܱ�.ID, ���������ܱ�.����ID, ���������ϸ��.ʵ�ƽ�� AS ��Ʊ�ܶ�, ���������ϸ��.��ƷID, ���������ܱ�.����  FROM ���������ܱ� INNER JOIN ������Ʒ�Ƶ��� ON ���������ܱ�.����ID = ������Ʒ�Ƶ���.ID INNER JOIN ���������ϸ�� ON ���������ܱ�.ID = ���������ϸ��.����ID INNER JOIN ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID CROSS JOIN ��Ʊ����� WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME,   '" + dateTimePickerE.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.��λID = " + iSupplyCompany.ToString() + ") AND ((���������ܱ�.��Ʊ�� IS NULL) OR (���������ܱ�.��Ʊ�� = N''))";
                            sqlDA.Fill(dSet, "���յ�����ϸ����");

                            sqlConn.Close();


                            FormSelectGDFP frmSelectGDFP1 = new FormSelectGDFP();
                            frmSelectGDFP1.iSelectStyle = 2;

                            frmSelectGDFP1.dtSelect = dSet.Tables["���յ�����ϸ����"];
                            frmSelectGDFP1.ShowDialog();

                            if (frmSelectGDFP1.dSUMJE >= 0)
                            {
                                DataView dt = new DataView(dSet.Tables["���յ�����ϸ����"]);
                                dt.RowFilter = "ѡ��=1";
                                dataGridViewDJMX.DataSource = dt;
                                dataGridViewDJMX.Columns[0].Visible = false;
                                dataGridViewDJMX.Columns[6].Visible = false;
                                dataGridViewDJMX.Columns[7].Visible = false;
                                dataGridViewDJMX.Columns[9].Visible = false;
                                dataGridViewDJMX.Columns[1].ReadOnly = true;
                                dataGridViewDJMX.Columns[2].ReadOnly = true;
                                dataGridViewDJMX.Columns[3].ReadOnly = true;
                                dataGridViewDJMX.Columns[4].ReadOnly = true;
                                dataGridViewDJMX.Columns[5].ReadOnly = true;

                                dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.AllowUserToAddRows = false;
                                dataGridViewDJMX.AllowUserToDeleteRows = false;
                            }

                            break; 
                    }
                    break;

                case 1: //����

                    switch (comboBoxGD.SelectedIndex)
                    {
                        case 0: //���յ���
                            if (dSet.Tables.Contains("���յ��ݹ���"))  //��ʼ�����ݹ����б�
                                dSet.Tables.Remove("���յ��ݹ���");
                            sqlConn.Open();
                            sqlComm.CommandText = "(SELECT ��Ʊ�����.ѡ��, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.��˰�ϼ�, ������Ʒ�Ƶ���.��˰�ϼ� AS ��Ʊ�ܶ�, ������Ʒ�Ƶ���.���䷽ʽ, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���.ID,������Ʒ�Ƶ���.����, '' AS ��ע1, '' AS ��ע2   FROM ������Ʒ�Ƶ��� CROSS JOIN ��Ʊ����� WHERE (������Ʒ�Ƶ���.��Ʊ�� IS NULL) AND (������Ʒ�Ƶ���.��λID = " + iSupplyCompany.ToString() + ") AND  (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND  (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + "  23:59:59', 102)) AND  (������Ʒ�Ƶ���.BeActive = 1) AND ((������Ʒ�Ƶ���.��Ʊ�� IS NULL) OR (������Ʒ�Ƶ���.��Ʊ�� = N''))) UNION (SELECT ��Ʊ�����.ѡ��, �����˳����ܱ�.���ݱ��, -1*�����˳����ܱ�.��˰�ϼ�, -1*�����˳����ܱ�.��˰�ϼ� AS ��Ʊ�ܶ�, '' AS ���䷽ʽ, '' AS ����, �����˳����ܱ�.ID, �����˳����ܱ�.����, '' AS ��ע1, '' AS ��ע2  FROM ��Ʊ����� CROSS JOIN �����˳����ܱ� WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + "  23:59:59', 102)) AND (�����˳����ܱ�.��λID = " + iSupplyCompany.ToString() + ") AND (�����˳����ܱ�.��Ʊ�� IS NULL OR �����˳����ܱ�.��Ʊ�� = N'')) UNION (SELECT ��Ʊ�����.ѡ��, �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.��˰�ϼ�, �����˲���ۻ��ܱ�.��˰�ϼ� AS ��Ʊ�ܶ�, '' AS ���䷽ʽ, '' AS ����, �����˲���ۻ��ܱ�.ID, �����˲���ۻ��ܱ�.����, '' AS ��ע1, '' AS ��ע2  FROM ��Ʊ����� CROSS JOIN �����˲���ۻ��ܱ� WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.��λID = " + iSupplyCompany.ToString() + ") AND (�����˲���ۻ��ܱ�.��Ʊ�� IS NULL OR �����˲���ۻ��ܱ�.��Ʊ�� = N'')) ";
                            sqlDA.Fill(dSet, "���յ��ݹ���");

                            sqlConn.Close();


                            FormSelectGDFP frmSelectGDFP3 = new FormSelectGDFP();
                            frmSelectGDFP3.iSelectStyle = 3;

                            frmSelectGDFP3.dtSelect = dSet.Tables["���յ��ݹ���"];
                            frmSelectGDFP3.ShowDialog();

                            if (true)//frmSelectGDFP3.dSUMJE >= 0)
                            {
                                /*
                                DataRow[] dr = dSet.Tables["���յ��ݹ���"].Select("ѡ��=1");


                                for (i = 0; i < dr.Length; i++)
                                {
                                    object[] oTemp = new object[10];
                                    oTemp[0] = dr[i][6];
                                    oTemp[1] = dr[i][6];
                                    oTemp[2] = dr[i][1];
                                    oTemp[3] = dr[i][1];
                                    oTemp[4] = dr[i][2];
                                    oTemp[5] = dr[i][2];
                                    oTemp[6] = dr[i][4];
                                    oTemp[7] = dr[i][5];
                                    oTemp[8] = "";
                                    oTemp[9] = "";
                                    dTable.Rows.Add(oTemp);
                                }
                                 * */

                                for (i = 0; i < dSet.Tables["���յ��ݹ���"].Rows.Count; i++)
                                {
                                    if (bool.Parse(dSet.Tables["���յ��ݹ���"].Rows[i][0].ToString()))
                                    {
                                        object[] oTemp = new object[10];
                                        oTemp[0] = dSet.Tables["���յ��ݹ���"].Rows[i][6];
                                        oTemp[1] = dSet.Tables["���յ��ݹ���"].Rows[i][6];
                                        oTemp[2] = dSet.Tables["���յ��ݹ���"].Rows[i][1];
                                        oTemp[3] = dSet.Tables["���յ��ݹ���"].Rows[i][1];
                                        oTemp[4] = dSet.Tables["���յ��ݹ���"].Rows[i][2];
                                        oTemp[5] = dSet.Tables["���յ��ݹ���"].Rows[i][2];
                                        oTemp[6] = dSet.Tables["���յ��ݹ���"].Rows[i][4];
                                        oTemp[7] = dSet.Tables["���յ��ݹ���"].Rows[i][5];
                                        oTemp[8] = "";
                                        oTemp[9] = "";
                                        dTable.Rows.Add(oTemp);
                                    }

                                }

                                dataGridViewDJMX.DataSource = dTable;

                                dataGridViewDJMX.Columns[0].Visible = false;
                                dataGridViewDJMX.Columns[1].Visible = false;
                                dataGridViewDJMX.Columns[3].Visible = false;
                                dataGridViewDJMX.Columns[2].Visible = true;
                                dataGridViewDJMX.Columns[4].Visible = true;
                                dataGridViewDJMX.Columns[5].Visible = true;
                                dataGridViewDJMX.Columns[6].Visible = true;
                                dataGridViewDJMX.Columns[7].Visible = true;
                                dataGridViewDJMX.Columns[8].Visible = true;
                                dataGridViewDJMX.Columns[9].Visible = true;
                                //dataGridViewDJMX.Columns[7].Visible = false;
                                dataGridViewDJMX.Columns[2].ReadOnly = true;
                                dataGridViewDJMX.Columns[3].ReadOnly = true;
                                dataGridViewDJMX.Columns[4].ReadOnly = true;
                                //dataGridViewDJMX.Columns[5].ReadOnly = true;

                                dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.AllowUserToAddRows = false;
                                dataGridViewDJMX.AllowUserToDeleteRows = false;

                            }

                            break;

                        case 1: //������Ʒ

                            if (dSet.Tables.Contains("���յ�����ϸ����"))  //��ʼ�����ݹ����б�
                                dSet.Tables.Remove("���յ�����ϸ����");
                            sqlConn.Open();
                            sqlComm.CommandText = "SELECT ��Ʊ�����.ѡ��, ������Ʒ�Ƶ���.���ݱ��, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ�� AS ��Ʊ���, ������Ʒ�Ƶ���.���䷽ʽ, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���.���� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID CROSS JOIN ��Ʊ����� WHERE (������Ʒ�Ƶ���.��Ʊ�� IS NULL) AND (������Ʒ�Ƶ���.��λID = " + iSupplyCompany.ToString() + ") AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND ((������Ʒ�Ƶ���.BeActive = 1) OR (������Ʒ�Ƶ���.��Ʊ�� <> N''))";
                            sqlDA.Fill(dSet, "���յ�����ϸ����");

                            sqlConn.Close();


                            FormSelectGDFP frmSelectGDFP4 = new FormSelectGDFP();
                            frmSelectGDFP4.iSelectStyle = 4;

                            frmSelectGDFP4.dtSelect = dSet.Tables["���յ�����ϸ����"];
                            frmSelectGDFP4.ShowDialog();

                            if (frmSelectGDFP4.dSUMJE != 0)
                            {
                                DataView dt = new DataView(dSet.Tables["���յ�����ϸ����"]);
                                dt.RowFilter = "ѡ��=1";
                                dataGridViewDJMX.DataSource = dt;
                                dataGridViewDJMX.Columns[0].Visible = false;
                                dataGridViewDJMX.Columns[8].Visible = false;
                                dataGridViewDJMX.Columns[9].Visible = false;

                                dataGridViewDJMX.Columns[1].ReadOnly = true;
                                dataGridViewDJMX.Columns[2].ReadOnly = true;
                                dataGridViewDJMX.Columns[3].ReadOnly = true;
                                dataGridViewDJMX.Columns[4].ReadOnly = true;
                                dataGridViewDJMX.Columns[6].ReadOnly = true;
                                dataGridViewDJMX.Columns[7].ReadOnly = true;

                                dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.AllowUserToAddRows = false;
                                dataGridViewDJMX.AllowUserToDeleteRows = false;
                            }
                            break; 
                    }

                    break;

            }
            countAmount();
        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("���������ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //return true ��ȷ  false ����
        private bool countAmount()
        {
            decimal fSum = 0, fSum1 = 0;

            bool bCheck = true;

            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                switch (comboBoxStyle.SelectedIndex)
                {
                    case 0: //����

                        switch (comboBoxGD.SelectedIndex)
                        {
                            case 0: //���յ���
                                if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString()=="")
                                {
                                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                                }

                                /*
                                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value) < 0)
                                {
                                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "���������0������";
                                    bCheck = false;

                                }
                                else
                                {
                                 */
                                    fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);
                                    fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                                //}

                                break;

                            case 1://������Ʒ

                                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                                {
                                    dataGridViewDJMX.Rows[i].Cells[8].Value = 0;
                                }

                                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value) < 0)
                                {
                                    dataGridViewDJMX.Rows[i].Cells[8].Value = 0;
                                    dataGridViewDJMX.Rows[i].Cells[8].ErrorText = "���������0������";
                                    bCheck = false;

                                }
                                else
                                {
                                    fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                                    fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                                }
                                break;
                        }
                        break;

                    case 1:

                        switch (comboBoxGD.SelectedIndex)
                        {
                            case 0: //���յ���
                                if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() == "")
                                {
                                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                                }

                                /*
                                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value) < 0)
                                {
                                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "���������0������";
                                    bCheck = false;

                                }
                                else
                                {
                                 */
                                    fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);
                                    fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                                //}
                                break;

                            case 1: //������Ʒ

                                if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() == "")
                                {
                                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                                }

                                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value) <= 0)
                                {
                                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "���������0������";
                                    bCheck = false;

                                }
                                else
                                {
                                    fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);
                                    fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                                }

                                break;

                        }

                        break;

                }
            }
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString();
            labelJEHJ.Text = fSum.ToString();
            labelSJJE.Text = fSum1.ToString();
            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);


            return bCheck;


        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            string strDT;
            string sTemp;

            cGetInformation.getSystemDateTime();
            //strDT = cGetInformation.strSYSDATATIME;
            strDT = dateTimePickerKPRQ.Value.ToShortDateString();

            //�������
            if (isSaved)
            {
                MessageBox.Show("��Ʊ�Ѿ����ߣ���Ʊ��Ϊ��" + textBoxFPH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (textBoxFPH.Text.Trim() == "")
            {
                MessageBox.Show("�����뷢Ʊ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (iSupplyCompany == 0)
            {
                MessageBox.Show("��ѡ��λ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!countAmount())
            {
                MessageBox.Show("��Ʊ��ϸ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (dataGridViewDJMX.RowCount<1)
            {
                MessageBox.Show("û��ѡ��Ʊ��Ŀ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("���鷢Ʊ���ݣ��Ƿ�������棿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;


            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();

            //��Ʊ�Ų���
            try
            {
                sTemp = textBoxFPH.Text.Trim().Substring(0, 3);
            }
            catch
            {
                sTemp = "";
            }

            if (textBoxFPH.Text.Trim() != "�ֽ𲻿�Ʊ" && sTemp != "����Ʊ")
            {
                sqlComm.CommandText = "SELECT ��Ʊ���ܱ�.ID, ��Ʊ���ܱ�.��Ʊ��, ��λ��.��λ����, ��Ʊ���ܱ�.��Ʊ�ܶ�, ��Ʊ���ܱ�.���� FROM ��Ʊ���ܱ� INNER JOIN ��λ�� ON ��Ʊ���ܱ�.��λID = ��λ��.ID WHERE (��Ʊ���ܱ�.��Ʊ�� = N'" + textBoxFPH.Text + "') AND (��Ʊ���ܱ�.BeActive <> 0)";
                sqldr = sqlComm.ExecuteReader();

                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    MessageBox.Show("��Ʊ���ظ���" + sqldr.GetValue(2).ToString() + "(" + sqldr.GetValue(4).ToString() + " ��" + sqldr.GetValue(3).ToString() + ")", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    sqlConn.Close();
                    return;
                }
                sqldr.Close();
            }
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                //������
                sqlComm.CommandText = "INSERT INTO ��Ʊ���ܱ� (��Ʊ��, ��λID, ��ע, ������ʽ, ����, ����ԱID, ԭ��Ʊ���, ��Ʊ�ܶ�, BeActive, ��Ʊ����, ����) VALUES (N'" + textBoxFPH.Text + "', " + iSupplyCompany.ToString() + ", N'" + textBoxBZ.Text + "', N'" + comboBoxFHFS.Text + "', N'" + textBoxDH.Text + "', " + intUserID.ToString() + ", " + labelJEHJ.Text + ", " + labelSJJE.Text + ", 1, " + comboBoxStyle.SelectedIndex.ToString() + ", '" + strDT + "')";
                sqlComm.ExecuteNonQuery();

                //ȡ�õ��ݺ� 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //��ϸ
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    switch (comboBoxStyle.SelectedIndex)
                    {
                        case 0: //����

                            switch (comboBoxGD.SelectedIndex)
                            {
                                case 0: //���յ���
                                    sqlComm.CommandText = "INSERT INTO ��Ʊ��ϸ�� (��ƱID, ����ID, ���ID, ���ݱ��, ��ֱ��, ԭ��Ʊ�ܶ�, ��Ʊ�ܶ�, ������ʽ, ����, ��ע1, ��ע2) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[1].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[2].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[3].Value.ToString() + "', " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + "')";
                                    sqlComm.ExecuteNonQuery();

                                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString().Substring(0, 3);
                                    switch (sTemp)
                                    {
                                        case "ADH":

                                            sqlComm.CommandText = "UPDATE ���������ܱ� SET ��Ʊ�� = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();

                                            sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET ��Ʊ�� = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[1].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();
                                            break;
                                        case "ATH":

                                            sqlComm.CommandText = "UPDATE �����˳����ܱ� SET ��Ʊ�� = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();
                                            break;
                                        case "ATB":

                                            sqlComm.CommandText = "UPDATE �����˲���ۻ��ܱ� SET ��Ʊ�� = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();
                                            break;

                                        default:
                                            break;
                                    }


                                    break;
                                case 1://������Ʒ
                                    sqlComm.CommandText = "INSERT INTO ��Ʊ��ϸ�� (��ƱID, ���ID, ����ID, ԭ��Ʊ�ܶ�, ��Ʊ�ܶ�, ������ʽ, ����, ��ƷID, ���ݱ��) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", N'', N''," + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ",N'" + dataGridViewDJMX.Rows[i].Cells[1].Value.ToString() + "')";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE ���������ܱ� SET ��Ʊ�� = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET ��Ʊ�� = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();


                                    break;
                            }
                            break;

                        case 1: //����

                            switch (comboBoxGD.SelectedIndex)
                            {
                                case 0: //���յ���
                                    sqlComm.CommandText = "INSERT INTO ��Ʊ��ϸ�� (��ƱID, ����ID, ���ID, ���ݱ��, ��ֱ��, ԭ��Ʊ�ܶ�, ��Ʊ�ܶ�, ������ʽ, ����, ��ע1, ��ע2) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[1].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[2].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[3].Value.ToString() + "', " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + "')";
                                    sqlComm.ExecuteNonQuery();

                                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString().Substring(0, 3);
                                    switch (sTemp)
                                    {
                                        case "BKP":

                                            sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET ��Ʊ�� = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();
                                            break;
                                        case "BTH":

                                            sqlComm.CommandText = "UPDATE �����˳����ܱ� SET ��Ʊ�� = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();
                                            break;
                                        case "BTB":

                                            sqlComm.CommandText = "UPDATE �����˲���ۻ��ܱ� SET ��Ʊ�� = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();
                                            break;

                                        default:
                                            break;
                                    }
                                    break;
                                case 1://������Ʒ
                                    sqlComm.CommandText = "INSERT INTO ��Ʊ��ϸ�� (��ƱID, ���ID, ����ID, ԭ��Ʊ�ܶ�, ��Ʊ�ܶ�, ������ʽ, ����, ��ƷID, ���ݱ��) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[4].Cells[2].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + "'," + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ",N'" + dataGridViewDJMX.Rows[i].Cells[1].Value.ToString() + "')";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET ��Ʊ�� = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                            }
                            break;

                    }

                }



                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ݿ����" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                return;
            }
            finally
            {
                sqlConn.Close();
            }

            bool bClose = false;
            if (MessageBox.Show("��Ʊ���߳ɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                bClose = true;
            }

            //MessageBox.Show("��Ʊ���߳ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isSaved = true;
            textBoxFPH.Enabled = false;
            if (MessageBox.Show("�Ƿ������Ʊ���ߣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
                mdiT.���߷�ƱAToolStripMenuItem_Click(null, null);
            }


            if (bClose)
                this.Close();
        }

        private void FormFPKJ_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
                return;

            DialogResult dr = MessageBox.Show(this, "�����޸���δ���棬ȷ��Ҫ�˳���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("��Ʊ������ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "��Ʊ����(��Ʊ��:" + textBoxFPH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" +  labelCZY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��Ʊ��" + labelSJJE.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("��Ʊ������ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "��Ʊ����(��Ʊ��:" + textBoxFPH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + labelCZY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��Ʊ��" + labelSJJE.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }



    }
}