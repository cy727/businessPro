using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPDAWH : Form
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
        private DataView dvSelect;

        public int iVersion = 1;
        
        public FormSPDAWH()
        {
            InitializeComponent();
        }

        private void FormSPDAWH_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            this.Top = 1;
            this.Left = 1;


            initDataView(0);
        }
        private void initDataView(int iSel)
        {
            //��ʼ���б�
            sqlConn.Open();

            sqlComm.CommandText = "SELECT ��Ʒ��.ID, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.������, ��Ʒ��.��С������λ, ��Ʒ��.����, ��Ʒ��.������, ��Ʒ��.��¼����, ��Ʒ��.��Ʒ���, ��Ʒ��.�������, ��Ʒ��.�������, ��Ʒ��.����������, ��Ʒ��.����������, ��Ʒ�����.��������, ��Ʒ��.������ AS ����ID ,��Ʒ�����.������ FROM ��Ʒ�� LEFT OUTER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID WHERE (��Ʒ��.beactive = 1) ORDER BY ��Ʒ��.��Ʒ���";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");

            sqlComm.CommandText = "SELECT ��Ʒ��.ID, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.������, ��Ʒ��.��С������λ, ��Ʒ��.����, ��Ʒ��.������, ��Ʒ��.��¼����, ��Ʒ��.��Ʒ���, ��Ʒ��.�������, ��Ʒ��.�������, ��Ʒ��.����������, ��Ʒ��.����������, ��Ʒ�����.��������, ��Ʒ��.������ AS ����ID, ��Ʒ�����.������ FROM ��Ʒ�� INNER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID WHERE (��Ʒ��.beactive = 1) AND (��Ʒ��.ID = 0) ORDER BY ��Ʒ��.��Ʒ���";

            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");

            //dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];
            dvSelect = new DataView(dSet.Tables["��Ʒ��"]);
            dataGridViewDJMX.DataSource = dvSelect;
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[14].Visible = false;

            sqlConn.Close();
            toolStripStatusLabelCount.Text = "������Ʒ" + dataGridViewDJMX.Rows.Count.ToString() + "��";

            if (iSel != 0)
            {
                dataGridViewDJMX.Rows[0].Selected = false;
                int iRow = -1;

                for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == iSel.ToString())
                    {
                        iRow = i;
                        break;
                    }
                }


                if (iRow != -1)
                {
                    dataGridViewDJMX.Rows[iRow].Selected = true;
                    dataGridViewDJMX.FirstDisplayedScrollingRowIndex = iRow;
                }


            }


        }

        private void ToolStripButtonADD_Click(object sender, EventArgs e)
        {
            if (iVersion <= 0)
            {
                if (dataGridViewDJMX.RowCount >= 10)
                {
                    MessageBox.Show("Ԥ���������Թ���10����Ʒ");
                    return;
                }
            }
            
            dSet.Tables["��Ʒ��1"].Clear();
            DataTable dt = dSet.Tables["��Ʒ��1"];

            FormSPDAWH_CARD frmSPDAWH_CARD = new FormSPDAWH_CARD();
            frmSPDAWH_CARD.strConn = strConn;
            //frmSPDAWH_CARD.dt = dt;
            frmSPDAWH_CARD.iStyle = 0;


            frmSPDAWH_CARD.ShowDialog();
            initDataView(frmSPDAWH_CARD.iSelect);
        }

        private void toolStripButtonEDIT_Click(object sender, EventArgs e)
        {
            object[] oT = new object[dataGridViewDJMX.ColumnCount];

            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵ���Ʒ", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dSet.Tables["��Ʒ��1"].Clear();
            DataTable dt = dSet.Tables["��Ʒ��1"];

            for (int i = dataGridViewDJMX.SelectedRows.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < oT.Length; j++)
                    oT[j] = dataGridViewDJMX.SelectedRows[i].Cells[j].Value;
                dt.Rows.Add(oT);
            }

            FormSPDAWH_CARD frmSPDAWH_CARD = new FormSPDAWH_CARD();
            frmSPDAWH_CARD.strConn = strConn;
            frmSPDAWH_CARD.dt = dt;
            frmSPDAWH_CARD.iStyle = 1;

            frmSPDAWH_CARD.ShowDialog();
            initDataView(frmSPDAWH_CARD.iSelect);
        }

        private void toolStripButtonDEL_Click(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫɾ������Ʒ", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (MessageBox.Show("�Ƿ�ɾ����ѡ���ݣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            int i;
            bool bDELALL = true;
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


                    //ʹ��״̬
                    sqlComm.CommandText = "SELECT DISTINCT ��Ʒ��.��Ʒ���� FROM ������ϸ������ͼ INNER JOIN ��Ʒ�� ON ������ϸ������ͼ.��ƷID = ��Ʒ��.ID WHERE (������ϸ������ͼ.BeActive = 1) AND (������ϸ������ͼ.��ƷID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        bDELALL = false;
                        sqldr.Close();
                        continue;
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "UPDATE ��Ʒ�� SET beactive = 0 WHERE (ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();


                    sqlComm.CommandText = "UPDATE ���� SET BeActive = 0 WHERE (��ƷID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
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

            if(bDELALL)
                MessageBox.Show("ɾ�����", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("��ѡ��Ʒ�����е��ݱ��棬����ɾ����������Ʒ��ɾ��", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            initDataView(0);
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

            string strT = "��Ʒ����ά��;��ǰ���ڣ�" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");

            string strT = "��Ʒ����ά��;��ǰ���ڣ�" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            dvSelect.RowFilter = "";
            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
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
            if (keyData == Keys.F8)
            {
                btnLocation_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (textBoxMC.Text.Trim() == "")
                return;

            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
            if (radioButtonAll.Checked)
            {
                dvSelect.RowFilter = "��Ʒ���� LIKE '%" + textBoxMC.Text.Trim() + "%'";
            }
            if (radioButtonF.Checked)
            {
                dvSelect.RowFilter = "��Ʒ���� LIKE '" + textBoxMC.Text.Trim() + "%'";
            }
            if (radioButtonE.Checked)
            {
                dvSelect.RowFilter = "��Ʒ���� LIKE '%" + textBoxMC.Text.Trim() + "'";
            }
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            if (textBoxMC.Text.Trim() == "")
                return;

            int iRow = -1;
            string sTemp = "";

            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (radioButtonAll.Checked)  //ȫƥ��
                {
                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString();
                    if (sTemp.IndexOf(textBoxMC.Text.Trim()) != -1)
                    {
                        iRow = i;
                        break;
                    }
                }

                if (radioButtonF.Checked) //ǰƥ��
                {
                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString();
                    if (sTemp.IndexOf(textBoxMC.Text.Trim()) == 0)
                    {
                        iRow = i;
                        break;
                    }
                }

                if (radioButtonE.Checked) //��ƥ��
                {
                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString().Trim();
                    if (sTemp.Length < textBoxMC.Text.Trim().Length)
                        break;

                    if (sTemp.LastIndexOf(textBoxMC.Text.Trim()) == sTemp.Length - textBoxMC.Text.Trim().Length)
                    {
                        iRow = i;
                        break;
                    }
                }


            }


            if (iRow != -1)
            {
                //dataGridViewDWLB.Rows[iRow].Selected = false;
                dataGridViewDJMX.Rows[iRow].Selected = true;
                dataGridViewDJMX.FirstDisplayedScrollingRowIndex = iRow;
            }
            else
            {
                if (dataGridViewDJMX.Rows.Count > 0)
                {
                    dataGridViewDJMX.Rows[0].Selected = true;
                    dataGridViewDJMX.FirstDisplayedScrollingRowIndex = 0;
                }
            }
        }

    }
}