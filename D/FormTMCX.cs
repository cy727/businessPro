using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormTMCX : Form
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

        private string strDJBH = "";
        private int intDJID = 0;
        private string sDJClass = "";

        private int intCommID = 0;
        private int intCommID1 = 0;
        System.Data.DataTable dTable = new System.Data.DataTable();

        private bool isSaved = false;
        private ClassGetInformation cGetInformation;
        
        public FormTMCX()
        {
            InitializeComponent();
        }

        private void FormTMCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

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

            dTable.Columns.Add("��Ʒ���", System.Type.GetType("System.String"));
            dTable.Columns.Add("��Ʒ����", System.Type.GetType("System.String"));
            dTable.Columns.Add("����", System.Type.GetType("System.String"));
            dTable.Columns.Add("����", System.Type.GetType("System.String"));
            dTable.Columns.Add("ժҪ", System.Type.GetType("System.String"));
            dTable.Columns.Add("���ݱ��", System.Type.GetType("System.String"));
            
            //dataGridViewKCTM.DataSource = dTable;
        }

        private void btnTM_Click(object sender, EventArgs e)
        {
            this.textBoxTM.Focus();
            this.textBoxTM.SelectAll();
        }

        private void textBoxTM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                textBoxTM.Text = textBoxTM.Text.ToUpper().Trim();

                if (textBoxTM.Text == "")
                {
                    labelWARN.ForeColor = Color.Red;
                    labelWARN.Text = "��¼����Ʒ����";
                    return;
                }

                initTmVIEW();

                textBoxTM.SelectAll();
            }

        }

        private void initTmVIEW()
        {
            //�Ƿ�������¼
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ�����.ID, ��Ʒ�����.���ݱ��, ��Ʒ�����.ժҪ, ��Ʒ�����.����, ��Ʒ�����.������� AS ����, ְԱ��.ְԱ���� AS ����Ա FROM ��Ʒ����� INNER JOIN ְԱ�� ON ��Ʒ�����.����ԱID = ְԱ��.ID WHERE (��Ʒ�����.���� = N'" + textBoxTM.Text.ToUpper() + "') ORDER BY ��Ʒ�����.����";
            if (dSet.Tables.Contains("�����")) dSet.Tables.Remove("�����");
            sqlDA.Fill(dSet, "�����");
            dataGridViewTM.DataSource = dSet.Tables["�����"];
            dataGridViewTM.Columns[0].Visible = false;

            if (dSet.Tables["�����"].Rows.Count < 1)
            {
                labelWARN.ForeColor = Color.Red;
                labelWARN.Text = "û����ؼ�¼";
            }
            else
            {
                labelWARN.ForeColor = Color.Green;
                labelWARN.Text = "��ȡ����ɹ�";
            }
            sqlConn.Close();
        }


        private void textBoxTM_Enter(object sender, EventArgs e)
        {
            textBoxTM.SelectAll();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                string strT = "��Ʒ�����¼(" + textBoxTM.Text.ToUpper() + ");��";
                PrintDGV.Print_DataGridView(dataGridViewTM, strT, true, intUserLimit);
            }
            if (tabControl1.SelectedIndex == 1)
            {
                string strT = "��Ʒ��������¼(" + textBoxSPMC.Text.ToUpper() + ");��";
                PrintDGV.Print_DataGridView(dataGridViewKCTM, strT, true, intUserLimit);
            }
            if (tabControl1.SelectedIndex == 2)
            {
                string strT = "�����¼(" + textBoxSPMC1.Text.ToUpper() + ");��";
                PrintDGV.Print_DataGridView(dataGridViewJL, strT, true, intUserLimit);
            }

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                string strT = "��Ʒ�����¼(" + textBoxTM.Text.ToUpper() + ");��";
                PrintDGV.Print_DataGridView(dataGridViewTM, strT, false, intUserLimit);
            }
            if (tabControl1.SelectedIndex == 1)
            {
                string strT = "��Ʒ��������¼(" + textBoxSPMC.Text.ToUpper() + ");��";
                PrintDGV.Print_DataGridView(dataGridViewKCTM, strT, false, intUserLimit);
            }
            if (tabControl1.SelectedIndex == 2)
            {
                string strT = "�����¼(" + textBoxSPMC1.Text.ToUpper() + ");��";
                PrintDGV.Print_DataGridView(dataGridViewJL, strT, false, intUserLimit);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewTM.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫɾ�������룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("�Ƿ�Ҫɾ��ѡ�������룿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                //��ϸ
                for (int i = 0; i < dataGridViewTM.SelectedRows.Count; i++)
                {

                    sqlComm.CommandText = "DELETE FROM ��Ʒ����� WHERE (ID = " + dataGridViewTM.SelectedRows[i].Cells[0].Value.ToString() + ")";
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
            MessageBox.Show("ɾ��������ϣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initTmVIEW();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxSP_DoubleClick(object sender, EventArgs e)
        {

        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;

                }

            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;

                }

            }
        }

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPBH.Text = cGetInformation.strCommCode;
                textBoxSPMC.Text = cGetInformation.strCommName;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i, j, k;

            sqlConn.Open();
            //��Ʒ����
            sqlComm.CommandText = "SELECT DISTINCT ����, ��ƷID FROM ��Ʒ�����";
            if(!checkBoxAll.Checked)
                sqlComm.CommandText +=" WHERE (��ƷID = "+intCommID.ToString()+")";
            if (dSet.Tables.Contains("��Ʒ�����")) dSet.Tables.Remove("��Ʒ�����");
            sqlDA.Fill(dSet, "��Ʒ�����");

            if (dSet.Tables.Contains("��������")) dSet.Tables.Remove("��������");
            toolStripProgressBarP.Maximum = dSet.Tables["��Ʒ�����"].Rows.Count;
            for (i = 0; i < dSet.Tables["��Ʒ�����"].Rows.Count; i++)
            {
                toolStripProgressBarP.Value = i;
                sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ�����.����, ��Ʒ�����.����, ��Ʒ�����.ժҪ, ��Ʒ�����.���ݱ�� FROM ��Ʒ����� INNER JOIN ��Ʒ�� ON ��Ʒ�����.��ƷID = ��Ʒ��.ID WHERE (��Ʒ�����.ID =(SELECT MAX(ID) AS Expr1 FROM ��Ʒ����� AS ��Ʒ�����_1 WHERE (���� = '" + dSet.Tables["��Ʒ�����"].Rows[i][0].ToString() + "')  AND (���� <= CONVERT(DATETIME, '" + dateTimePickerEnd.Value.ToShortDateString() + " 12:59:59', 102)))) AND (��Ʒ�����.������� = 0) ";
                sqlDA.Fill(dSet, "��������");

            }
            toolStripProgressBarP.Value = toolStripProgressBarP.Maximum;
            sqlConn.Close();
            dataGridViewKCTM.DataSource = dSet.Tables["��������"];
            //dataGridViewKCTM.Columns[6].Visible = false;
            toolStripStatusLabelS.Text = "����" + dataGridViewKCTM .RowCount.ToString()+ "����¼";


            
        }

        private void textBoxSPBH1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH1.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    intCommID1 = cGetInformation.iCommNumber;
                    textBoxSPBH1.Text = cGetInformation.strCommCode;
                    textBoxSPMC1.Text = cGetInformation.strCommName;
                }

            }
        }

        private void textBoxSPMC1_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                intCommID1 = cGetInformation.iCommNumber;
                textBoxSPBH1.Text = cGetInformation.strCommCode;
                textBoxSPMC1.Text = cGetInformation.strCommName;
            }
        }

        private void textBoxSPBH1_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                intCommID1 = cGetInformation.iCommNumber;
                textBoxSPBH1.Text = cGetInformation.strCommCode;
                textBoxSPMC1.Text = cGetInformation.strCommName;
            }
        }

        private void textBoxSPMC1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC1.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    intCommID1 = cGetInformation.iCommNumber;
                    textBoxSPBH1.Text = cGetInformation.strCommCode;
                    textBoxSPMC1.Text = cGetInformation.strCommName;

                }

            }
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {

            sqlConn.Open();
            //��Ʒ����
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ�����.����, ��Ʒ�����.����, ��Ʒ�����.ժҪ, ��Ʒ�����.���ݱ�� FROM ��Ʒ����� INNER JOIN ��Ʒ�� ON ��Ʒ�����.��ƷID = ��Ʒ��.ID WHERE     (��Ʒ�����.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (��Ʒ�����.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 12:59:59', 102)) ";
            if(!checkBoxAll1.Checked)
                sqlComm.CommandText +=" AND ��Ʒ��.ID="+intCommID1.ToString();

            if(!checkBoxCK.Checked || !checkBoxRK.Checked)
            {
                if(!checkBoxCK.Checked)
                    sqlComm.CommandText +=" AND (��Ʒ�����.������� = 0) ";

                if (!checkBoxRK.Checked)
                    sqlComm.CommandText +=" AND (��Ʒ�����.������� = 1) ";
            }

            if (dSet.Tables.Contains("�����¼��")) dSet.Tables.Remove("�����¼��");
            sqlDA.Fill(dSet, "�����¼��");
            sqlConn.Close();
            dataGridViewJL.DataSource = dSet.Tables["�����¼��"];
            toolStripStatusLabelS.Text = "����" + dataGridViewJL.RowCount.ToString() + "����¼";

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(tabControl1.SelectedIndex==0)
                toolStripStatusLabelS.Text = "";
            if (tabControl1.SelectedIndex == 1)
                toolStripStatusLabelS.Text = "����" + dataGridViewKCTM.RowCount.ToString() + "����¼";
            if (tabControl1.SelectedIndex == 2)
                toolStripStatusLabelS.Text = "����" + dataGridViewJL.RowCount.ToString() + "����¼";

        }
    }
}