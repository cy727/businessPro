using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSelectHT : Form
    {
        public string strConn = "";
        public string strSelectText = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iHTNumber = 0;
        public string strHTCode = "";
        public int iSelectStyle = 0;
        public string strHTSearch = "";

        public int iCompanyNumber = 0;
        public string strCompanyName = "";
        public string strCompanyCode = "";

        private DataView dvCommSelect;

        
        public FormSelectHT()
        {
            InitializeComponent();
        }

        private void FormSelectHT_Load(object sender, EventArgs e)
        {
            switch (iSelectStyle)
            {
                case 0: //������ͬ
                    strSelectText = "SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, �ɹ���ͬ��.ǩ��ʱ�� FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 1) ORDER BY �ɹ���ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 1:  //�ѽ��������˻�������ͬ
                    strSelectText = "SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, �ɹ���ͬ��.ǩ��ʱ�� FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 0) AND (�ɹ���ͬ��.�˻���� = 0) ORDER BY �ɹ���ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 2:  //���ۺ�ͬ
                    strSelectText = "SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.������λID, ���ۺ�ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, ���ۺ�ͬ��.ǩ��ʱ�� FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (���ۺ�ͬ��.BeActive = 1) ORDER BY ���ۺ�ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 3:  //�ѳ��������˻����ۺ�ͬ
                    strSelectText = "SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.������λID, ���ۺ�ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, ���ۺ�ͬ��.ǩ��ʱ�� FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (���ۺ�ͬ��.BeActive = 0) AND (���ۺ�ͬ��.�˻���� = 0) ORDER BY ���ۺ�ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 4://���к�ͬ
                    strSelectText = "(SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.������λID, ���ۺ�ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, ���ۺ�ͬ��.ǩ��ʱ�� FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (���ۺ�ͬ��.BeActive = 1)) UNION (SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, �ɹ���ͬ��.ǩ��ʱ�� FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 1))";
                    break;

                case 10: //������ͬ
                    strSelectText = "SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, �ɹ���ͬ��.ǩ��ʱ�� FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 1) AND (�ɹ���ͬ��.��ͬ��� LIKE '%" + strHTSearch + "%') ORDER BY �ɹ���ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 11:  //�ѽ��������˻�������ͬ
                    strSelectText = "SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, �ɹ���ͬ��.ǩ��ʱ�� FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 0) AND (�ɹ���ͬ��.�˻���� = 0) AND (�ɹ���ͬ��.��ͬ��� LIKE '%" + strHTSearch + "%') ORDER BY �ɹ���ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 12:  //���ۺ�ͬ
                    strSelectText = "SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.������λID, ���ۺ�ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, ���ۺ�ͬ��.ǩ��ʱ�� FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (���ۺ�ͬ��.BeActive = 1) AND (���ۺ�ͬ��.��ͬ��� LIKE '%" + strHTSearch + "%') ORDER BY �ɹ���ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 13:  //�ѳ��������˻����ۺ�ͬ
                    strSelectText = "SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.������λID, ���ۺ�ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, ���ۺ�ͬ��.ǩ��ʱ�� FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (���ۺ�ͬ��.BeActive = 0) AND (���ۺ�ͬ��.�˻���� = 0) AND (���ۺ�ͬ��.��ͬ��� LIKE '%" + strHTSearch + "%') ORDER BY ���ۺ�ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 14://���к�ͬ
                    strSelectText = "(SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.������λID, ���ۺ�ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, ���ۺ�ͬ��.ǩ��ʱ�� FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (���ۺ�ͬ��.BeActive = 1) AND (���ۺ�ͬ��.��ͬ��� LIKE '%" + strHTSearch + "%') UNION (SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, �ɹ���ͬ��.ǩ��ʱ�� FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 1)) AND (���ۺ�ͬ��.��ͬ��� LIKE '%" + strHTSearch + "%')";
                    break;

                case 100:  //����̵�׼����
                    strSelectText = "SELECT ����̵���ܱ�.ID, ����̵���ܱ�.�ⷿID, ����̵���ܱ�.���ݱ��, ����̵���ܱ�.����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ְԱ��.ְԱ����, ����̵���ܱ�.��ע FROM ����̵���ܱ�  LEFT OUTER JOIN �ⷿ�� ON ����̵���ܱ�.�ⷿID = �ⷿ��.ID INNER JOIN   ְԱ�� ON ����̵���ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (����̵���ܱ�.BeActive = 1) AND (����̵���ܱ�.�̵��� = 0)";
                    this.Text = "ѡ���̵㴦��";
                    groupBox1.Text = "�̵��б�";
                    break;
                case 110:  //����̵�׼����
                    strSelectText = "SELECT ����̵���ܱ�.ID, ����̵���ܱ�.�ⷿID, ����̵���ܱ�.���ݱ��, ����̵���ܱ�.����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ְԱ��.ְԱ����, ����̵���ܱ�.��ע FROM ����̵���ܱ�  LEFT OUTER JOIN �ⷿ�� ON ����̵���ܱ�.�ⷿID = �ⷿ��.ID INNER JOIN   ְԱ�� ON ����̵���ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (����̵���ܱ�.BeActive = 1) AND (����̵���ܱ�.�̵��� = 0) AND (����̵���ܱ�.���ݱ�� LIKE '%" + strHTSearch + "%')";
                    this.Text = "ѡ���̵㴦��";
                    groupBox1.Text = "�̵��б�";
                    break;
                default:
                    strSelectText = "SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ���, ��λ��.��λ����, ��λ��.��λ���, ְԱ��.ְԱ���� AS ҵ��Ա, �ɹ���ͬ��.ǩ��ʱ�� FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 1) ORDER BY �ɹ���ͬ��.ǩ��ʱ�� DESC";
                    break;

            }

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            //��ʼ����λ�б�
            sqlComm.CommandText = strSelectText;
            sqlConn.Open();
            if (dSet.Tables.Contains("��ͬ��")) dSet.Tables.Remove("��ͬ��");
            sqlDA.Fill(dSet, "��ͬ��");

            //dataGridViewHT.DataSource = dSet.Tables["��ͬ��"];
            dvCommSelect = new DataView(dSet.Tables["��ͬ��"]);
            dataGridViewHT.DataSource = dvCommSelect;

            dataGridViewHT.Columns[0].Visible = false;
            dataGridViewHT.Columns[1].Visible = false;
            dataGridViewHT.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            sqlConn.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iHTNumber = 0;
            iCompanyNumber = 0;
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dataGridViewHT.SelectedRows.Count < 1)
            {
                iCompanyNumber = 0;
                iHTNumber = 0;
                this.Close();
                return; ;
            }
            iCompanyNumber = Int32.Parse(dataGridViewHT.SelectedRows[0].Cells[1].Value.ToString());
            strCompanyName = dataGridViewHT.SelectedRows[0].Cells[3].Value.ToString();
            strCompanyCode = dataGridViewHT.SelectedRows[0].Cells[4].Value.ToString();

            iHTNumber = Int32.Parse(dataGridViewHT.SelectedRows[0].Cells[0].Value.ToString());
            strHTCode = dataGridViewHT.SelectedRows[0].Cells[2].Value.ToString();

            this.Close();
        }

        private void dataGridViewHT_DoubleClick(object sender, EventArgs e)
        {
            btnSelect_Click(null, null);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            dvCommSelect.RowFilter = "";
            dvCommSelect.RowStateFilter = DataViewRowState.CurrentRows;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (textBoxHTH.Text.Trim() == "" && textBoxDW.Text.Trim() == "")
                return;

            dvCommSelect.RowStateFilter = DataViewRowState.CurrentRows;
            if (textBoxHTH.Text.Trim() != "")
            {
                dvCommSelect.RowFilter = "��ͬ��� LIKE '%" + textBoxHTH.Text.Trim().ToUpper() + "%'";
            }
            if (textBoxDW.Text.Trim() != "")
            {
                dvCommSelect.RowFilter = "��λ���� LIKE '%" + textBoxDW.Text.Trim().ToUpper() + "%'";
            }

        }
        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.F9)
            {
                btnAll_Click(null, null);
                return true;
            }
            if (keyData == Keys.F7)
            {
                btnSearch_Click(null, null);
                return true;
            }

            if (keyData == Keys.Enter && dataGridViewHT.Focused)
            {
                btnSelect_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}