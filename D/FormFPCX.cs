using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormFPCX : Form
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
        public int LIMITACCESS = 18;
        public int LIMITACCESS1 = 5;
        public int LIMITACCESS2 = 10;

        private bool isSaved = false;
        
        public FormFPCX()
        {
            InitializeComponent();
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
                checkBoxAll.Checked = false;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1200, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                    checkBoxAll.Checked = true;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    checkBoxAll.Checked = false;
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
                    checkBoxAll.Checked = true;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    checkBoxAll.Checked = false;
                }
            }
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAll.Checked)
            {
                iSupplyCompany = 0;
                textBoxDWBH.Text = "";
                textBoxDWMC.Text = "";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            bool bFP = true;
            decimal dt1, dt2;
            int i;
            string strA,strB,strC;

            switch (comboBoxStyle.SelectedIndex)
            {
                case 0: //����
                    sqlComm.CommandText = "SELECT ��Ʊ���ܱ�.ID, ��Ʊ���ܱ�.��Ʊ��, ��Ʊ���ܱ�.����, ��Ʊ���ܱ�.ԭ��Ʊ���,��Ʊ���ܱ�.��Ʊ�ܶ�, ��λ��.��λ����, ��Ʊ���ܱ�.������ʽ, ��Ʊ���ܱ�.����, ��Ʊ���ܱ�.��ע FROM ��Ʊ���ܱ� INNER JOIN ��λ�� ON ��Ʊ���ܱ�.��λID = ��λ��.ID WHERE (��Ʊ���ܱ�.BeActive = 1) AND (��Ʊ���ܱ�.���� >= CONVERT(DATETIME,  '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (��Ʊ���ܱ�.���� <= CONVERT(DATETIME,  '" + dateTimePickerE.Value.ToShortDateString() + " 00:00:00', 102)) AND (��Ʊ���ܱ�.��Ʊ���� = 0)";

                    if (!(intUserLimit > LIMITACCESS))
                    {
                        sqlComm.CommandText += " AND ((��λ��.ҵ��Ա = N'" + strUserName + "') OR (��Ʊ���ܱ�.����ԱID = "+intUserID.ToString()+"))";
                    }
                    
                    break;
                case 1:
                    sqlComm.CommandText = "SELECT ��Ʊ���ܱ�.ID, ��Ʊ���ܱ�.��Ʊ��, ��Ʊ���ܱ�.����, ��Ʊ���ܱ�.ԭ��Ʊ���,��Ʊ���ܱ�.��Ʊ�ܶ�, ��λ��.��λ����, ��Ʊ���ܱ�.������ʽ, ��Ʊ���ܱ�.����, ��Ʊ���ܱ�.��ע FROM ��Ʊ���ܱ� INNER JOIN ��λ�� ON ��Ʊ���ܱ�.��λID = ��λ��.ID WHERE (��Ʊ���ܱ�.BeActive = 1) AND (��Ʊ���ܱ�.���� >= CONVERT(DATETIME,  '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (��Ʊ���ܱ�.���� <= CONVERT(DATETIME,  '" + dateTimePickerE.Value.ToShortDateString() + " 00:00:00', 102)) AND (��Ʊ���ܱ�.��Ʊ���� = 1)";

                    if (!(intUserLimit > LIMITACCESS))
                    {
                        sqlComm.CommandText += " AND ((��λ��.ҵ��Ա = N'" + strUserName + "') OR (��Ʊ���ܱ�.����ԱID = " + intUserID.ToString() + "))";
                    }
                    break;
                case 2:
                    sqlComm.CommandText = "SELECT ��Ʊ���ܱ�.ID, ��Ʊ���ܱ�.��Ʊ��, ��Ʊ���ܱ�.����, ��Ʊ���ܱ�.ԭ��Ʊ���,��Ʊ���ܱ�.��Ʊ�ܶ�, ��λ��.��λ����, ��Ʊ���ܱ�.������ʽ, ��Ʊ���ܱ�.����, ��Ʊ���ܱ�.��ע FROM ��Ʊ���ܱ� INNER JOIN ��λ�� ON ��Ʊ���ܱ�.��λID = ��λ��.ID WHERE (��Ʊ���ܱ�.BeActive = 0) AND (��Ʊ���ܱ�.���� >= CONVERT(DATETIME,  '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (��Ʊ���ܱ�.���� <= CONVERT(DATETIME,  '" + dateTimePickerE.Value.ToShortDateString() + " 00:00:00', 102)) ";

                    if (!(intUserLimit > LIMITACCESS))
                    {
                        sqlComm.CommandText += " AND ((��λ��.ҵ��Ա = N'" + strUserName + "') OR (��Ʊ���ܱ�.����ԱID = " + intUserID.ToString() + "))";
                    }
                    break;
                case 3:
                    strA = "(SELECT ���������ܱ�.���ݱ��, ���������ܱ�.ID, ������Ʒ�Ƶ���.���ݱ�� AS ��ֵ���, ���������ܱ�.��˰�ϼ�, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.��Ʊ�� FROM ���������ܱ� INNER JOIN ������Ʒ�Ƶ��� ON ���������ܱ�.����ID = ������Ʒ�Ƶ���.ID INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.��Ʊ�� IS NULL OR ���������ܱ�.��Ʊ�� = N'')";
                    if (iSupplyCompany != 0)
                        strA += " AND (���������ܱ�.��λID = " + iSupplyCompany.ToString() + ")";

                    if (intUserLimit < LIMITACCESS)
                    {
                        strA += " AND ((���������ܱ�.ҵ��ԱID = " + intUserID.ToString() + ") OR (���������ܱ�.����ԱID = " + intUserID.ToString() + "))";
                    }
                    strA += ")";

                    strB = "(SELECT �����˳����ܱ�.���ݱ��,�����˳����ܱ�.ID, �����˳����ܱ�.���ݱ�� AS ��ֵ���, -1.0*�����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.����, ��λ��.��λ���,  ��λ��.��λ����, �����˳����ܱ�.��Ʊ�� FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˳����ܱ�.��Ʊ�� IS NULL OR �����˳����ܱ�.��Ʊ�� = N'')";
                    if (iSupplyCompany != 0)
                        strB += " AND (�����˳����ܱ�.��λID = " + iSupplyCompany.ToString() + ")";
                    if (intUserLimit < LIMITACCESS)
                    {
                        strB += " AND ((�����˳����ܱ�.ҵ��ԱID = " + intUserID.ToString() + ") OR (�����˳����ܱ�.����ԱID = " + intUserID.ToString() + "))";
                    }
                    strB += ")";


                    strC = "(SELECT �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.ID,�����˲���ۻ��ܱ�.���ݱ�� AS ��ֵ���, �����˲���ۻ��ܱ�.��˰�ϼ�, �����˲���ۻ��ܱ�.����, ��λ��.��λ���,  ��λ��.��λ����, �����˲���ۻ��ܱ�.��Ʊ�� FROM �����˲���ۻ��ܱ� INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.��Ʊ�� IS NULL OR �����˲���ۻ��ܱ�.��Ʊ�� = N'')";
                    if (iSupplyCompany != 0)
                        strC += " AND (�����˲���ۻ��ܱ�.��λID = " + iSupplyCompany.ToString() + ")";
                    if (intUserLimit < LIMITACCESS)
                    {
                        strC += " AND ((�����˲���ۻ��ܱ�.ҵ��ԱID = " + intUserID.ToString() + ") OR (�����˲���ۻ��ܱ�.����ԱID = " + intUserID.ToString() + "))";
                    }
                    strC += ")";
                    sqlComm.CommandText = strA + " UNION " + strB + " UNION " + strC;
                    bFP = false;
                    break;
                case 4:
                    strA = "(SELECT ������Ʒ�Ƶ���.���ݱ��,������Ʒ�Ƶ���.ID,  ������Ʒ�Ƶ���.��˰�ϼ�, ������Ʒ�Ƶ���.����, ��λ��.��λ���,  ��λ��.��λ����, ������Ʒ�Ƶ���.��Ʊ�� FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.��Ʊ�� IS NULL OR ������Ʒ�Ƶ���.��Ʊ�� = N'')";
                    if (iSupplyCompany != 0)
                        strA += " AND (������Ʒ�Ƶ���.��λID = " + iSupplyCompany.ToString() + ")";
                    if (intUserLimit < LIMITACCESS)
                    {
                        strA += " AND ((������Ʒ�Ƶ���.ҵ��ԱID = " + intUserID.ToString() + ") OR (������Ʒ�Ƶ���.����ԱID = " + intUserID.ToString() + "))";
                    }
                    strA += ")";

                    strB = "(SELECT �����˳����ܱ�.���ݱ��, �����˳����ܱ�.ID, -1.0*�����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.����, ��λ��.��λ���,  ��λ��.��λ����, �����˳����ܱ�.��Ʊ�� FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˳����ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˳����ܱ�.��Ʊ�� IS NULL OR �����˳����ܱ�.��Ʊ�� = N'')";
                    if (iSupplyCompany != 0)
                        strB += " AND (�����˳����ܱ�.��λID = " + iSupplyCompany.ToString() + ")";
                    if (intUserLimit < LIMITACCESS)
                    {
                        strB += " AND ((�����˳����ܱ�.ҵ��ԱID = " + intUserID.ToString() + ") OR (�����˳����ܱ�.����ԱID = " + intUserID.ToString() + "))";
                    }
                    strB += ")";


                    strC = "(SELECT �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.ID, �����˲���ۻ��ܱ�.��˰�ϼ�, �����˲���ۻ��ܱ�.����, ��λ��.��λ���,  ��λ��.��λ����, �����˲���ۻ��ܱ�.��Ʊ�� FROM �����˲���ۻ��ܱ� INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (�����˲���ۻ��ܱ�.��Ʊ�� IS NULL OR �����˲���ۻ��ܱ�.��Ʊ�� = N'')";
                    if (iSupplyCompany != 0)
                        strC += " AND (�����˲���ۻ��ܱ�.��λID = " + iSupplyCompany.ToString() + ")";
                    if (intUserLimit < LIMITACCESS)
                    {
                        strC += " AND ((�����˲���ۻ��ܱ�.ҵ��ԱID = " + intUserID.ToString() + ") OR (�����˲���ۻ��ܱ�.����ԱID = " + intUserID.ToString() + "))";
                    }
                    strC += ")";
                    sqlComm.CommandText = strA + " UNION " + strB + " UNION " + strC;
                    bFP = false;
                    break;

            }

            if (bFP)
            {
                //dataGridViewDJMX.CellDoubleClick += dataGridViewDJMX_CellDoubleClick;
                if (iSupplyCompany != 0)
                    sqlComm.CommandText += "  AND (��Ʊ���ܱ�.��λID = " + iSupplyCompany.ToString() + ")";

                if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
                sqlDA.Fill(dSet, "������ϸ��");
                dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

                dataGridViewDJMX.Columns[0].Visible = false;
                dataGridViewDJMX.Columns[1].Visible = true;

                dt1 = 0; dt2 = 0;
                for (i = 0; i < dataGridViewDJMX.RowCount; i++)
                {
                    try
                    {
                        dt1 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[3].Value.ToString());
                        dt2 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[4].Value.ToString());
                    }
                    catch
                    {
                    }
                }

                for (i = 1; i < dataGridViewDJMX.ColumnCount; i++)
                {
                    dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                }
                dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";

                sqlConn.Close();
                toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString() + " ԭ��Ʊ���ϼƣ�" + dt1.ToString("f2") + " ��Ʊ�ܶ�ϼƣ�" + dt2.ToString("f2");
            }
            else
            {
                //dataGridViewDJMX.CellDoubleClick -= dataGridViewDJMX_CellDoubleClick;

                dataGridViewDJMX.DataSource = null;
                if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
                sqlDA.Fill(dSet, "������ϸ��");
                dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

                dataGridViewDJMX.Columns[0].Visible = true;
                dataGridViewDJMX.Columns[1].Visible = false;

                dt1 = 0;
                for (i = 0; i < dataGridViewDJMX.RowCount; i++)
                {
                    try
                    {
                        if (comboBoxStyle.SelectedIndex == 3)
                        {
                            dt1 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[3].Value.ToString());
                            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
                        }
                        else
                        {
                            dt1 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[2].Value.ToString());
                            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f2";
                        }
                    }
                    catch
                    {
                    }
                }

                for (i = 1; i < dataGridViewDJMX.ColumnCount; i++)
                {
                    dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                }
                

                sqlConn.Close();
                toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString() + " ���ϼƣ�" + dt1.ToString("f2");
                sqlConn.Close();
            }

        }

        private void FormFPCX_Load(object sender, EventArgs e)
        {
            this.Left = 1;
            this.Top = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            comboBoxStyle.SelectedIndex = 0;
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��˾����, ����Ŀ��1, ����Ŀ��2, ����Ŀ��3, ����Ŀ��4, ����ԱȨ��, �ܾ���Ȩ��, ְԱȨ��, ����Ȩ��, ҵ��ԱȨ�� FROM ϵͳ������";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {

                try
                {
                    LIMITACCESS = int.Parse(sqldr.GetValue(8).ToString());
                }
                catch
                {
                    LIMITACCESS = 15;
                }
            }
            sqldr.Close();
            //�õ���ʼʱ��
            sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            sqlConn.Close();

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��Ʊ��ѯ;���ڣ�" + labelZDRQ.Text + ";��λ���ƣ�" + textBoxDWMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��Ʊ��ѯ;���ڣ�" + labelZDRQ.Text + "��λ���ƣ�" + textBoxDWMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
                return;

            if (dataGridViewDJMX.Rows[e.RowIndex].Cells[0].Value.ToString() == "")
                return;

            int iDJID = 0;
            iDJID = Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[0].Value.ToString());

            // �������Ӵ����һ����ʵ����
            FormFPKJ childFormFPKJ = new FormFPKJ();
            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
            childFormFPKJ.MdiParent = this.MdiParent;

            childFormFPKJ.strConn = strConn;
            childFormFPKJ.iDJID = iDJID;
            childFormFPKJ.isSaved = true;

            childFormFPKJ.intUserID = intUserID;
            childFormFPKJ.intUserLimit = intUserLimit;
            childFormFPKJ.strUserLimit = strUserLimit;
            childFormFPKJ.strUserName = strUserName;

            childFormFPKJ.Show();
        }



        private void dataGridViewDJMX_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count<1)
                return;

            if (dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString() == "")
                return;

            int iDJID = 0;
            bool bHasFP = true;
            try
            {
                iDJID = Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString());
            }
            catch
            {
                bHasFP = false;
            }

            if (bHasFP)
            {
                // �������Ӵ����һ����ʵ����
                FormFPKJ childFormFPKJ = new FormFPKJ();
                // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                childFormFPKJ.MdiParent = this.MdiParent;

                childFormFPKJ.strConn = strConn;
                childFormFPKJ.iDJID = iDJID;
                childFormFPKJ.isSaved = true;

                childFormFPKJ.intUserID = intUserID;
                childFormFPKJ.intUserLimit = intUserLimit;
                childFormFPKJ.strUserLimit = strUserLimit;
                childFormFPKJ.strUserName = strUserName;

                childFormFPKJ.Show();
            }
            else //������ϸ
            {
                try
                {
                    iDJID = Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[1].Value.ToString());
                }
                catch
                {
                    return;
                }

                DJZX(dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString(), iDJID);
            }
        }


        private void DJZX(string strDJBH, int iDJID)
        {
            string sTemp = "", sTemp1 = "";

            if (strDJBH.Trim() == "")
                return;

            sTemp = strDJBH.Trim().ToUpper();
            sTemp1 = iDJID.ToString();



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
    }
}