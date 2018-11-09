using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJHDJPXZ : Form
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

        public FormJHDJPXZ()
        {
            InitializeComponent();
        }

        private void FormJHDJPXZ_Load(object sender, EventArgs e)
        {
            int i;

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
            sqlConn.Close();


            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;


        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i, j;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ���������ͼ.���ݱ��, ���������ͼ.����, ��λ��.��λ���, ��λ��.��λ����, ���������ͼ.��˰�ϼ�, ���������ͼ.�Ѹ�����, ���������ͼ.δ������ FROM ���������ͼ INNER JOIN ��λ�� ON ���������ͼ.��λID = ��λ��.ID WHERE (���������ͼ.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND  (���������ͼ.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iCompanyID != 0 && !checkBoxALL.Checked)
                sqlComm.CommandText += " AND (��λ��.ID="+iCompanyID.ToString()+")";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];

            decimal fTemp = 0, fTemp1 = 0, fTemp2 = 0;
            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                for (j = 4; j < dSet.Tables["��Ʒ��"].Columns.Count; j++)
                {
                    if (dSet.Tables["��Ʒ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["��Ʒ��"].Rows[i][j] = 0;
                }
                fTemp += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][4].ToString());
                fTemp1 += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][5].ToString());
                fTemp2 += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][6].ToString());
            }

            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];
            
            for (i = 0; i < dataGridViewDJMX.ColumnCount; i++)
            {
                dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            toolStripStatusLabelJEHJ.Text=fTemp.ToString("f2");
            toolStripStatusLabelWFK.Text = fTemp2.ToString("f2");
            toolStripStatusLabelYFK.Text = fTemp1.ToString("f2");
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
            sqlConn.Close();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��������ƽ����;��ǰ���ڣ�" + labelZDRQ.Text ;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
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

        private void printToolStripButton_Click(object sender, EventArgs e)
        {

        }
    }
}