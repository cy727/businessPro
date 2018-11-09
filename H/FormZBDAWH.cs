using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormZBDAWH : Form
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

        public FormZBDAWH()
        {
            InitializeComponent();
        }

        private void FormZBDAWH_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            initDataView();
        }

        private void initDataView()
        {
            //��ʼ���б�
            sqlConn.Open();

            sqlComm.CommandText = "SELECT ID, �˲����, �˲�����, ������, �Ƿ��֧��, ��ʾ��Ϣ FROM �˲��� WHERE (BeActive = 1)";

            if (dSet.Tables.Contains("�˲���")) dSet.Tables.Remove("�˲���");
            sqlDA.Fill(dSet, "�˲���");

            sqlComm.CommandText = "SELECT ID, �˲����, �˲�����, ������, �Ƿ��֧��, ��ʾ��Ϣ FROM �˲��� WHERE (BeActive = 1) AND (ID = 0)";

            if (dSet.Tables.Contains("�˲���1")) dSet.Tables.Remove("�˲���1");
            sqlDA.Fill(dSet, "�˲���1");

            dataGridViewDJMX.DataSource = dSet.Tables["�˲���"];
            dataGridViewDJMX.Columns[0].Visible = false;

            sqlConn.Close();


        }

        private void ToolStripButtonADD_Click(object sender, EventArgs e)
        {
            dSet.Tables["�˲���1"].Clear();
            DataTable dt = dSet.Tables["�˲���1"];

            FormZBDAWH_CARD frmZBDAWH_CARD = new FormZBDAWH_CARD();
            frmZBDAWH_CARD.strConn = strConn;
            frmZBDAWH_CARD.dt = dt;
            frmZBDAWH_CARD.iStyle = 0;

            frmZBDAWH_CARD.ShowDialog();
            initDataView();
        }

        private void toolStripButtonEDIT_Click(object sender, EventArgs e)
        {
            object[] oT = new object[dataGridViewDJMX.ColumnCount];

            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵ��˲�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dSet.Tables["�˲���1"].Clear();
            DataTable dt = dSet.Tables["�˲���1"];

            for (int i = dataGridViewDJMX.SelectedRows.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < oT.Length; j++)
                    oT[j] = dataGridViewDJMX.SelectedRows[i].Cells[j].Value;
                dt.Rows.Add(oT);
            }

            FormZBDAWH_CARD frmZBDAWH_CARD = new FormZBDAWH_CARD();
            frmZBDAWH_CARD.strConn = strConn;
            frmZBDAWH_CARD.dt = dt;
            frmZBDAWH_CARD.iStyle = 1;

            frmZBDAWH_CARD.ShowDialog();
            initDataView();
        }

        private void toolStripButtonDEL_Click(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫɾ�����˲�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (MessageBox.Show("�Ƿ�ɾ����ѡ���ݣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            int i;
            System.Data.SqlClient.SqlTransaction sqlta;

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                for (i = 0; i < dataGridViewDJMX.SelectedRows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "UPDATE �˲��� SET BeActive = 0 WHERE (ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
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
            MessageBox.Show("ɾ�����", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");

            string strT = "�ʲ�����ά��;��ǰ���ڣ�" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");

            string strT = "�ʲ�����ά��;��ǰ���ڣ�" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}