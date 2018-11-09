using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormTJTZD2 : Form
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

        private ClassGetInformation cGetInformation;

        private int intHTH = 0;

        private bool isSaved=false;

        public FormTJTZD2()
        {
            InitializeComponent();
        }

        private void FormTJTZD2_Load(object sender, EventArgs e)
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

        }

        private void textBoxHTH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getBillInformation(4, "") == 0)
            {
                return;
            }
            else
            {
                intHTH = cGetInformation.iBillNumber;
                getBillDetail();
            }
        }

        private void getBillDetail()
        {
            if (intHTH == 0)
                return;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ���ݱ��, ����, ��ע FROM ����֪ͨ�����ܱ� WHERE (ID = "+intHTH.ToString()+")";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                textBoxHTH.Text=sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy��M��dd��");
                textBoxBZ.Text = sqldr.GetValue(2).ToString(); 
            }

            sqldr.Close();

            sqlComm.CommandText = "SELECT ����֪ͨ����ϸ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, ����֪ͨ����ϸ��.ԭ����, ����֪ͨ����ϸ��.����, ����֪ͨ����ϸ��.ԭ������, ����֪ͨ����ϸ��.������, ����֪ͨ����ϸ��.��ƷID FROM ����֪ͨ����ϸ�� INNER JOIN ��Ʒ�� ON ����֪ͨ����ϸ��.��ƷID = ��Ʒ��.ID WHERE (����֪ͨ����ϸ��.����ID = "+intHTH.ToString()+")";
            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[9].Visible = false;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[4].ReadOnly = true;
            dataGridViewDJMX.Columns[5].ReadOnly = true;
            dataGridViewDJMX.Columns[7].ReadOnly = true;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;

            sqlConn.Close();

        }

        private void textBoxHTH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getBillInformation(40, "") == 0)
                {
                    return;
                }
                else
                {
                    intHTH = cGetInformation.iBillNumber;
                    getBillDetail();
                }
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            int i, j;


            if (intHTH == 0)
            {
                MessageBox.Show("��ѡ�����֪ͨ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                string strDT;
                cGetInformation.getSystemDateTime();
                strDT = cGetInformation.strSYSDATATIME;
                
                //������
                sqlComm.CommandText = "UPDATE ����֪ͨ�����ܱ� SET ִ�б�� = 1, ִ��ʱ�� = '" + strDT + "' WHERE (ID = "+intHTH.ToString()+")";
                sqlComm.ExecuteNonQuery();

                //ִ��
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������ = " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", ���� = " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ")";
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

            //MessageBox.Show("����֪ͨ��ִ�гɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.btnAccept.Enabled = false;
            isSaved = true;

            if (MessageBox.Show("����֪ͨ��ִ�гɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }


        }

        private void FormTJTZD2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
            {
                return;
            }

            DialogResult dr = MessageBox.Show(this, "������δִ�У�ȷ��Ҫ�˳���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            string strT = "����֪ͨ��(���ݱ��:" + labelDJBH.Text + ");ִ�����ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "����֪ͨ��(���ݱ��:" + labelDJBH.Text + ");ִ�����ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanelContent_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}