using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormBMDAWH : Form
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
        
        public FormBMDAWH()
        {
            InitializeComponent();
        }

        private void FormBMDAWH_Load(object sender, EventArgs e)
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

            sqlComm.CommandText = "SELECT ID, ���ű��, ��������, ������ FROM ���ű� WHERE (BeActive  = 1) ORDER BY ���ű��";

            if (dSet.Tables.Contains("���ű�")) dSet.Tables.Remove("���ű�");
            sqlDA.Fill(dSet, "���ű�");


            dataGridViewLB.DataSource = dSet.Tables["���ű�"];
            dataGridViewLB.Columns[0].Visible = false;
            dataGridViewLB.Columns[3].Visible = false;

            sqlConn.Close();


        }

        private void dataGridViewLB_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewLB.RowCount < 1)
                return;

            if (dataGridViewLB.SelectedRows.Count < 1)
                return;

            textBoxBH.Text = dataGridViewLB.SelectedRows[0].Cells[1].Value.ToString();
            textBoxMC.Text = dataGridViewLB.SelectedRows[0].Cells[2].Value.ToString();
            textBoxZJM.Text = dataGridViewLB.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void textBoxMC_TextChanged(object sender, EventArgs e)
        {
            textBoxZJM.Text = cGetInformation.convertPYSM(textBoxMC.Text);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;

            if (textBoxBH.Text.Trim() == "")
            {
                MessageBox.Show("�������ʹ��������벿�ű��", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxMC.Text.Trim() == "")
            {
                MessageBox.Show("�������ʹ��������벿������", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "INSERT INTO ���ű� (���ű��, ��������, ������, �Ƿ�����, �Ƿ�����, �Ƿ�����, BeActive) VALUES (N'" + textBoxBH.Text.Trim() + "', N'" + textBoxMC.Text.Trim() + "', '" + textBoxZJM.Text.Trim() + "', 1, 1, 1, 1)";
                sqlComm.ExecuteNonQuery();

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
            MessageBox.Show("���Ӳ��ųɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;

            if (dataGridViewLB.RowCount < 1)
                return;

            if (dataGridViewLB.SelectedRows.Count < 1)
                return;


            if (textBoxBH.Text.Trim() == "")
            {
                MessageBox.Show("�������ʹ��������벿�ű��", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxMC.Text.Trim() == "")
            {
                MessageBox.Show("�������ʹ��������벿������", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "UPDATE ���ű� SET ���ű�� = N'" + textBoxBH.Text.Trim() + "', �������� = N'" + textBoxMC.Text.Trim() + "', ������ = '" + textBoxZJM.Text.Trim() + "' WHERE (ID = " + dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString() + ")";
                sqlComm.ExecuteNonQuery();

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
            MessageBox.Show("�޸Ĳ��ųɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;

            if (dataGridViewLB.RowCount < 1)
                return;

            if (dataGridViewLB.SelectedRows.Count < 1)
                return;


            if (MessageBox.Show("�Ƿ�ɾ�����ţ��ù��̲��ɻ���", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                return;
            }


            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "UPDATE ���ű� SET BeActive = 0 WHERE (ID = " + dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString() + ")";
                sqlComm.ExecuteNonQuery();

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
            MessageBox.Show("ɾ�����ųɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();
        }

   }
}