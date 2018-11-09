using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormYWDWZHYEZ : Form
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

        private int iCompanyID = 0;

        private ClassGetInformation cGetInformation;
        private bool isSaved = false;

        public FormYWDWZHYEZ()
        {
            InitializeComponent();
        }

        private void FormYWDWZHYEZ_Load(object sender, EventArgs e)
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
            comboBoxDWXL.SelectedIndex = 0;
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dTemp=0;

            if (dSet.Tables.Contains("����"))
                dSet.Tables.Remove("����");

            sqlConn.Open();
            if (!dSet.Tables.Contains("����")) //û��
            {
                sqlComm.CommandText = "SELECT ��λ��.��λ���, ��λ��.��λ����, ������.�������, ������.��������, �˻���.�˳�����, �����.������, ��λ��.Ӧ���˿�, ������.�������, ������.��������, �˻ر�.�˻�����, �տ��.������, ��λ��.Ӧ���˿�, ��λ��.Ӧ���˿� AS ��Ӧ���˿�, ��λ��.Ӧ���˿� AS ��Ӧ���˿�,��λ��.�Ƿ����, ��λ��.�Ƿ����� FROM ��λ�� LEFT OUTER JOIN (SELECT SUM(������) AS ������, ��λID FROM ��λ��ʷ�˱� WHERE (BeActive = 1) AND (���ݱ�� LIKE N'%BYS%') GROUP BY ��λID) �տ�� ON  ��λ��.ID = �տ��.��λID LEFT OUTER JOIN (SELECT COUNT(*) AS �˻�����, ��λID FROM ��λ��ʷ�˱� WHERE (BeActive = 1) AND (���ݱ�� LIKE N'%BTH%') GROUP BY ��λID) �˻ر� ON ��λ��.ID = �˻ر�.��λID LEFT OUTER JOIN (SELECT SUM(�������) AS �������, COUNT(*) AS ��������, ��λID FROM ��λ��ʷ�˱� WHERE (BeActive = 1) AND (���ݱ�� LIKE N'%BKP%') GROUP BY ��λID) ������ ON ��λ��.ID = ������.��λID LEFT OUTER JOIN (SELECT SUM(������) AS ������, ��λID    FROM ��λ��ʷ�˱� WHERE (BeActive = 1) AND (���ݱ�� LIKE N'%AYF%') GROUP BY ��λID) ����� ON  ��λ��.ID = �����.��λID LEFT OUTER JOIN (SELECT COUNT(*) AS �˳�����, ��λID FROM ��λ��ʷ�˱� WHERE (BeActive = 1) AND (���ݱ�� LIKE N'%ATH%') GROUP BY ��λID) �˻��� ON ��λ��.ID = �˻���.��λID LEFT OUTER JOIN (SELECT SUM(�������) AS �������, COUNT(*) AS ��������, ��λID FROM ��λ��ʷ�˱� WHERE (BeActive = 1) AND (���ݱ�� LIKE N'%ADH%') GROUP BY ��λID) ������ ON ��λ��.ID = ������.��λID WHERE (��λ��.BeActive = 1)";

                if (!checkBoxALL.Checked && iCompanyID != 0)
                {
                    sqlComm.CommandText += " AND ��λ��.ID="+iCompanyID.ToString();
                }

                sqlDA.Fill(dSet, "����");

                for (i = 0; i < dSet.Tables["����"].Rows.Count; i++)
                {
                    for (j = 3; j < dSet.Tables["����"].Columns.Count; j++)
                    {
                        if (dSet.Tables["����"].Rows[i][j].ToString() == "")
                            dSet.Tables["����"].Rows[i][j] = 0;

                    }

                    dTemp = Convert.ToDecimal(dSet.Tables["����"].Rows[i][6].ToString()) - Convert.ToDecimal(dSet.Tables["����"].Rows[i][11].ToString());
                    if (dTemp < 0)
                    {
                        dSet.Tables["����"].Rows[i][12] = Math.Abs(dTemp);
                        dSet.Tables["����"].Rows[i][13] = 0;
                    }
                    else
                    {
                        dSet.Tables["����"].Rows[i][12] = 0;
                        dSet.Tables["����"].Rows[i][13] = dTemp;
                    }

                }
            }

            DataView dv;
            switch(comboBoxDWXL.SelectedIndex)
            {
                default:                    
                    dv = new DataView(dSet.Tables["����"]);
                    break;
                case 1:
                    dv = new DataView(dSet.Tables["����"], "�Ƿ����=1","",DataViewRowState.CurrentRows);
                    break;
                case 2:
                    dv = new DataView(dSet.Tables["����"], "�Ƿ�����=1", "", DataViewRowState.CurrentRows);
                    break;
                    

            }
            dataGridViewDJMX.DataSource = dv;
            if (!isSaved)
            {
                dataGridViewDJMX.Columns[14].Visible = false;
                dataGridViewDJMX.Columns[15].Visible = false;

                dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[12].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[13].DefaultCellStyle.Format = "f2";
                
                dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f0";
                dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f0";
                dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
                dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f0";

                for (i = 0; i < dataGridViewDJMX.ColumnCount; i++)
                {
                    dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                isSaved = true;
            }
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
            sqlConn.Close();

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "ҵ��λ�ۺ������;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "ҵ��λ�ۺ������;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1000, "") == 0) //ʧ��
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
                if (cGetInformation.getCompanyInformation(1200, textBoxDWMC.Text) == 0) //ʧ��
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
    }
}