using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKFDAWH : Form
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

        public FormKFDAWH()
        {
            InitializeComponent();
        }

        private void FormKFDAWH_Load(object sender, EventArgs e)
        {
            this.Top = 1;
            this.Left = 1;

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

            sqlComm.CommandText = "SELECT ID, �ⷿ���, �ⷿ����, ������, ��� FROM �ⷿ�� WHERE (BeActive = 1)";

            if (dSet.Tables.Contains("�ⷿ��")) dSet.Tables.Remove("�ⷿ��");
            sqlDA.Fill(dSet, "�ⷿ��");

            sqlComm.CommandText = "SELECT ID, �ⷿ���, �ⷿ����, ������, ��� FROM �ⷿ�� WHERE (BeActive = 1) AND (ID = 0)";

            if (dSet.Tables.Contains("�ⷿ��1")) dSet.Tables.Remove("�ⷿ��1");
            sqlDA.Fill(dSet, "�ⷿ��1");

            dataGridViewDJMX.DataSource = dSet.Tables["�ⷿ��"];
            dataGridViewDJMX.Columns[0].Visible = false;

            sqlConn.Close();


        }

        private void ToolStripButtonADD_Click(object sender, EventArgs e)
        {
            dSet.Tables["�ⷿ��1"].Clear();
            DataTable dt = dSet.Tables["�ⷿ��1"];

            FormKFDAWH_CARD frmKFDAWH_CARD = new FormKFDAWH_CARD();
            frmKFDAWH_CARD.strConn = strConn;
            frmKFDAWH_CARD.dt = dt;
            frmKFDAWH_CARD.iStyle = 0;

            frmKFDAWH_CARD.ShowDialog();
            initDataView();
        }

        private void toolStripButtonEDIT_Click(object sender, EventArgs e)
        {
            object[] oT = new object[dataGridViewDJMX.ColumnCount];

            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵĿⷿ", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dSet.Tables["�ⷿ��1"].Clear();
            DataTable dt = dSet.Tables["�ⷿ��1"];

            for (int i = dataGridViewDJMX.SelectedRows.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < oT.Length; j++)
                    oT[j] = dataGridViewDJMX.SelectedRows[i].Cells[j].Value;
                dt.Rows.Add(oT);
            }

            FormKFDAWH_CARD frmKFDAWH_CARD = new FormKFDAWH_CARD();
            frmKFDAWH_CARD.strConn = strConn;
            frmKFDAWH_CARD.dt = dt;
            frmKFDAWH_CARD.iStyle = 1;

            frmKFDAWH_CARD.ShowDialog();
            initDataView();
        }

        private void toolStripButtonDEL_Click(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫɾ���Ŀⷿ", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int i;
            sqlConn.Open();
            sqlComm.CommandText = "SELECT SUM(�������) FROM ���� ";
            for (i = 0; i < dataGridViewDJMX.SelectedRows.Count; i++)
            {
                if (dataGridViewDJMX.SelectedRows[i].IsNewRow)
                    continue;

                if (i == 0)
                    sqlComm.CommandText += " WHERE (�ⷿID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
                else
                    sqlComm.CommandText += " OR (�ⷿID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
            }
            sqldr = sqlComm.ExecuteReader();
            int iC=0;
            while (sqldr.Read())
            {
                if (sqldr.GetValue(0).ToString()!="")
                    iC = int.Parse(sqldr.GetValue(0).ToString());
                break;
            }
            sqldr.Close();

            if (iC == 0)
            {
                if (MessageBox.Show("�Ƿ�ɾ����ѡ���ݣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    sqlConn.Close();
                    return;
                }
            }
            else
            {
                if (MessageBox.Show("�Ƿ�ɾ����ѡ���ݣ��ⷿ������"+iC.ToString()+"�����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    sqlConn.Close();
                    return;
                }

            }

            
            System.Data.SqlClient.SqlTransaction sqlta;


            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                for (i = 0; i < dataGridViewDJMX.SelectedRows.Count; i++)
                {
                    if (dataGridViewDJMX.SelectedRows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "UPDATE �ⷿ�� SET BeActive = 0 WHERE (ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
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

        private void dataGridViewDJMX_DoubleClick(object sender, EventArgs e)
        {
            toolStripButtonEDIT_Click(null, null);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");

            string strT = "�ⷿ����ά��;��ǰ���ڣ�" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");

            string strT = "�ⷿ����ά��;��ǰ���ڣ�" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}