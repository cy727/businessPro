using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormZYDAWH : Form
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
        
        public FormZYDAWH()
        {
            InitializeComponent();
        }

        private void FormZYDAWH_Load(object sender, EventArgs e)
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

            sqlComm.CommandText = "SELECT ְԱ��.ID, ְԱ��.ְԱ���, ְԱ��.ְԱ����, ְԱ��.�Ա�, ��λ��.��λ����, ���ű�.��������, ְԱ��.�Ƿ����Ա, ְԱ��.�Ƿ�ҵ��Ա, ְԱ��.���֤��, ְԱ��.ְԱ�绰, ְԱ��.��ͥ��ַ, ְԱ��.��λID, ְԱ��.����ID FROM ְԱ�� LEFT JOIN ��λ�� ON ְԱ��.��λID = ��λ��.ID INNER JOIN ���ű� ON ְԱ��.����ID = ���ű�.ID WHERE (ְԱ��.BeActive = 1)  ORDER BY ְԱ��.ְԱ���";

            if (dSet.Tables.Contains("ְԱ��")) dSet.Tables.Remove("ְԱ��");
            sqlDA.Fill(dSet, "ְԱ��");

            sqlComm.CommandText = "SELECT ְԱ��.ID, ְԱ��.ְԱ���, ְԱ��.ְԱ����, ְԱ��.�Ա�, ��λ��.��λ����, ���ű�.��������, ְԱ��.�Ƿ����Ա, ְԱ��.�Ƿ�ҵ��Ա, ְԱ��.���֤��, ְԱ��.ְԱ�绰, ְԱ��.��ͥ��ַ, ְԱ��.��λID, ְԱ��.����ID FROM ְԱ�� LEFT JOIN ��λ�� ON ְԱ��.��λID = ��λ��.ID INNER JOIN ���ű� ON ְԱ��.����ID = ���ű�.ID WHERE (ְԱ��.BeActive = 1) AND (ְԱ��.ID = 0) ORDER BY ְԱ��.ְԱ���";

            if (dSet.Tables.Contains("ְԱ��1")) dSet.Tables.Remove("ְԱ��1");
            sqlDA.Fill(dSet, "ְԱ��1");

            dataGridViewDJMX.DataSource = dSet.Tables["ְԱ��"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;

            sqlConn.Close();


        }

        private void ToolStripButtonADD_Click(object sender, EventArgs e)
        {
            dSet.Tables["ְԱ��1"].Clear();
            DataTable dt = dSet.Tables["ְԱ��1"];

            FormZYDAWH_CARD frmZYDAWH_CARD = new FormZYDAWH_CARD();
            frmZYDAWH_CARD.strConn = strConn;
            frmZYDAWH_CARD.dt = dt;
            frmZYDAWH_CARD.iStyle = 0;

            frmZYDAWH_CARD.ShowDialog();
            initDataView();
        }

        private void toolStripButtonEDIT_Click(object sender, EventArgs e)
        {
            object[] oT = new object[dataGridViewDJMX.ColumnCount];

            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵ�ְԱ", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dSet.Tables["ְԱ��1"].Clear();
            DataTable dt = dSet.Tables["ְԱ��1"];

            for (int i = dataGridViewDJMX.SelectedRows.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < oT.Length; j++)
                    oT[j] = dataGridViewDJMX.SelectedRows[i].Cells[j].Value;
                dt.Rows.Add(oT);
            }

            FormZYDAWH_CARD frmZYDAWH_CARD = new FormZYDAWH_CARD();
            frmZYDAWH_CARD.strConn = strConn;
            frmZYDAWH_CARD.dt = dt;
            frmZYDAWH_CARD.iStyle = 1;

            frmZYDAWH_CARD.ShowDialog();
            initDataView();
        }

        private void toolStripButtonDEL_Click(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫɾ����ְԱ", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    if (dataGridViewDJMX.SelectedRows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "UPDATE ְԱ�� SET BeActive = 0 WHERE (ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
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

        private void toolStripButtonPASS_Click(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫ�޸������ְԱ", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormZYChangePass frmZYChangePass = new FormZYChangePass();
            frmZYChangePass.strConn = strConn;
            frmZYChangePass.iZYID = Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString());
            frmZYChangePass.strZYName= dataGridViewDJMX.SelectedRows[0].Cells[2].Value.ToString();

            frmZYChangePass.ShowDialog();


        }

        private void dataGridViewDJMX_DoubleClick(object sender, EventArgs e)
        {
            toolStripButtonEDIT_Click(null,null);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");

            string strT = "ְԱ����ά��;��ǰ���ڣ�" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");

            string strT = "ְԱ����ά��;��ǰ���ڣ�" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}