using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormWLDWYEDJ : Form
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


        private ClassGetInformation cGetInformation;
        
        public FormWLDWYEDJ()
        {
            InitializeComponent();
        }

        private void FormWLDWYEDJ_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            this.Top = 1;
            this.Left = 1;

            initDataView();
        }

        private void initDataView()
        {
            //��ʼ���б�
            sqlConn.Open();

            sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, Ӧ���˿�, Ӧ���˿�, Ӧ���˿� AS ����Ӧ���˿�, Ӧ���˿�  AS ����Ӧ���˿� FROM ��λ�� WHERE (BeActive = 1)  ORDER BY ��λ���";

            if (dSet.Tables.Contains("��λ��")) dSet.Tables.Remove("��λ��");
            sqlDA.Fill(dSet, "��λ��");
            sqlConn.Close();

            dataGridViewDJMX.DataSource = dSet.Tables["��λ��"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[1].ReadOnly = true;
            dataGridViewDJMX.Columns[2].ReadOnly = true;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[4].ReadOnly = true;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            for (int i = 0; i < dataGridViewDJMX.RowCount; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;
                if (dataGridViewDJMX.Rows[i].Cells[3].Value.ToString()=="")
                    dataGridViewDJMX.Rows[i].Cells[3].Value = 0;
                if (dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[4].Value = 0;
                dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                dataGridViewDJMX.Rows[i].Cells[6].Value = 0;
            }
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void toolStripButtonEDIT_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("�Ƿ�����޸ģ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            int i;
            System.Data.SqlClient.SqlTransaction sqlta;
            decimal dt1 = 0, dt2 = 0;

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() == "")
                        dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                    if (dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() == "")
                        dataGridViewDJMX.Rows[i].Cells[6].Value = 0;

                    dt1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                    if (dt1 != 0)
                    {
                        dt1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[3].Value);
                        sqlComm.CommandText = "UPDATE ��λ�� SET Ӧ���˿� = "+dt1.ToString()+" WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();
                    }

                    dt2 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                    if (dt2 != 0)
                    {
                        dt2 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);
                        sqlComm.CommandText = "UPDATE ��λ�� SET Ӧ���˿� = " + dt2.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();
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
            MessageBox.Show("�Ǽ����", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();

        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("�������ʹ���", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");

            string strT = "������λ���Ǽ�;��ǰ���ڣ�" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");

            string strT = "������λ���Ǽ�;��ǰ���ڣ�" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false);
        }
    }
}