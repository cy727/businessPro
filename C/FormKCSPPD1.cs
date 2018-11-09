using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace business
{
    public partial class FormKCSPPD1 : Form
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

        private int intKFID = 0;
        private int intCommID = 0;
        private int intClassID = 0;

        private ArrayList alKF = new ArrayList();

        private ClassGetInformation cGetInformation;

        private bool isSaved = false;
        
        public FormKCSPPD1()
        {
            InitializeComponent();
        }

        private void FormKCSPPD1_Load(object sender, EventArgs e)
        {
            int i;

            this.Top = 1;
            this.Left = 1;


            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            sqlConn.Open();

            //��ʼ��Ա���б�
            sqlComm.CommandText = "SELECT ID, ְԱ���, ְԱ���� FROM ְԱ�� WHERE (beactive = 1)";

            if (dSet.Tables.Contains("ְԱ��")) dSet.Tables.Remove("ְԱ��");
            sqlDA.Fill(dSet, "ְԱ��");
            comboBoxYWY.DataSource = dSet.Tables["ְԱ��"];
            comboBoxYWY.DisplayMember = "ְԱ����";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.Text = strUserName;

            //��ʼ�������б�
            comboBoxBM.SelectedIndexChanged -= comboBoxBM_SelectedIndexChanged;
            sqlComm.CommandText = "SELECT �������� FROM ���ű� WHERE (beactive = 1)";

            if (dSet.Tables.Contains("���ű�")) dSet.Tables.Remove("���ű�");
            sqlDA.Fill(dSet, "���ű�");
            //comboBoxBM.DataSource = dSet.Tables["���ű�"];

            comboBoxBM.Items.Add("ȫ��");
            for (i = 0; i < dSet.Tables["���ű�"].Rows.Count; i++)
            {
                comboBoxBM.Items.Add(dSet.Tables["���ű�"].Rows[i][0].ToString().Trim());
            }
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;


            sqlConn.Close();

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;
        }

        private void comboBoxBM_SelectedIndexChanged(object sender, EventArgs e)
        {
            sqlConn.Open();
            //��ʼ��Ա���б�
            if (comboBoxBM.Text.Trim() != "ȫ��")
                sqlComm.CommandText = "SELECT ְԱ��.ID, ְԱ��.ְԱ����, ְԱ��.ְԱ��� FROM ְԱ�� INNER JOIN ���ű� ON ְԱ��.����ID = ���ű�.ID WHERE (���ű�.�������� = N'" + comboBoxBM.Text.Trim() + "') AND (ְԱ��.beactive = 1)";
            else
                sqlComm.CommandText = "SELECT ְԱ��.ID, ְԱ��.ְԱ����, ְԱ��.ְԱ��� FROM ְԱ�� INNER JOIN ���ű� ON ְԱ��.����ID = ���ű�.ID WHERE (ְԱ��.beactive = 1)";

            sqldr = sqlComm.ExecuteReader();
            if (!sqldr.HasRows)
            {
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            sqldr.Close();

            if (dSet.Tables.Contains("ְԱ��")) dSet.Tables.Remove("ְԱ��");
            sqlDA.Fill(dSet, "ְԱ��");
            comboBoxYWY.DataSource = dSet.Tables["ְԱ��"];
            comboBoxYWY.DisplayMember = "ְԱ����";
            comboBoxYWY.ValueMember = "ID";
            sqlConn.Close();
        }

        private void textBoxPDKF_DoubleClick(object sender, EventArgs e)
        {
            /*
            if (cGetInformation.getKFInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                intKFID = cGetInformation.iKFNumber;
                textBoxPDKF.Text = cGetInformation.strKFCode;
                textBoxKFMC.Text = cGetInformation.strKFName;

                initdataGridViewDJMX();

            }
             */

            FormSelectKFList frmSelectKFList = new FormSelectKFList();
            frmSelectKFList.strConn = strConn;
            frmSelectKFList.ShowDialog();

            if (frmSelectKFList.bSEL)
            {
                textBoxKFMC.Text = "";
                alKF.Clear();
                for (int i = 0; i < frmSelectKFList.checkedListBoxKF.Items.Count; i++)
                {
                    if (frmSelectKFList.checkedListBoxKF.GetItemChecked(i))
                    {
                        alKF.Add(frmSelectKFList.alKF[i]);
                        textBoxKFMC.Text += " " + frmSelectKFList.checkedListBoxKF.Items[i];
                    }

                }
                initdataGridViewDJMX();
            }
        }

        private void initdataGridViewDJMX()
        {


            if (alKF.Count == 0) //û�пⷿ
            {
                if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
                return;
            }

            int i,j,k;
             decimal fCSum = 0, fSum = 0;


             strSelect = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ����.������� AS �������, ����.�������*����.���ɱ��� AS �����, ����̵㶨���.��ע, ����.��ƷID, ����.�ⷿID, �ⷿ��.�ⷿ���� FROM ���� INNER JOIN ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ����.�ⷿID = �ⷿ��.ID CROSS JOIN ����̵㶨��� WHERE (��Ʒ��.beactive = 1) ";

            for (i = 0; i < alKF.Count; i++)
            {
                if(i==0)
                    strSelect += " AND ((����.�ⷿID = " + alKF[i].ToString() + ")";
                else
                    strSelect += " OR (����.�ⷿID = " + alKF[i].ToString() + ")";
            }
            strSelect += ")";

            if (intCommID != 0) //��Ʒ����
            {
                strSelect += " AND (����.��ƷID = "+intCommID.ToString()+")";
            }

            if (intClassID != 0) //��Ʒ�������
            {
                strSelect += "  AND (��Ʒ��.������ = "+intClassID.ToString()+")";
            }
            sqlConn.Open();
          
            sqlComm.CommandText = strSelect;

            if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
            sqlDA.Fill(dSet, "���ݱ�");
            dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];
            sqlConn.Close();
            
            
            dataGridViewDJMX.Columns[6].Visible = false;
            dataGridViewDJMX.Columns[7].Visible = false;

            dataGridViewDJMX.Columns[4].Visible = false;


            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            fCSum = 0; fSum = 0;
            for (i = 0; i < dSet.Tables["���ݱ�"].Rows.Count; i++)
            {
                if (dSet.Tables["���ݱ�"].Rows[i][3].ToString() == "")
                    dSet.Tables["���ݱ�"].Rows[i][3] = 0;

                if (dSet.Tables["���ݱ�"].Rows[i][4].ToString() == "")
                    dSet.Tables["���ݱ�"].Rows[i][4] = 0;

                fCSum += Convert.ToDecimal(dSet.Tables["���ݱ�"].Rows[i][3]);
                fSum += Convert.ToDecimal(dSet.Tables["���ݱ�"].Rows[i][4]);
            }
            labelSLHJ.Text = Convert.ToInt32(fCSum).ToString();
            labelJEHJ.Text = fSum.ToString();
            toolStripStatusLabelMXJLS.Text = dSet.Tables["���ݱ�"].Rows.Count.ToString();
        }





        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;

            //�������
            if (isSaved)
            {
                MessageBox.Show("����̵���Ѿ�����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (alKF.Count == 0)
            {
                MessageBox.Show("��ѡ����Ʒ�ⷿ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataGridViewDJMX.RowCount <1)
            {
                MessageBox.Show("û���̵���Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (MessageBox.Show("�������̵������,���Ƶ����ݲ��ɸ��ģ��Ƿ�������棿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            saveToolStripButton.Enabled = false;
            string strCount = "", strDateSYS = "", strKey = "CPD";
            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //�õ�����
                //�õ�����������
                sqlComm.CommandText = "SELECT GETDATE() AS ����";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                sqldr.Close();

                //�õ�����
                sqlComm.CommandText = "SELECT ʱ�� FROM �������� WHERE (ʱ�� = CONVERT(DATETIME, '" + strDateSYS + " 00:00:00', 102))";
                sqldr = sqlComm.ExecuteReader();

                if (sqldr.HasRows)
                    sqldr.Close();
                else //������ʱ�䲻�Ǻ�
                {
                    sqldr.Close();
                    //�������ڼ�������
                    sqlComm.CommandText = "UPDATE �������� SET ʱ�� = '" + strDateSYS + "', ���� = 1";
                    sqlComm.ExecuteNonQuery();
                }

                //�õ�������
                sqlComm.CommandText = "SELECT ���� FROM �������� WHERE (�ؼ��� = N'" + strKey + "')";
                sqldr = sqlComm.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    strCount = sqldr.GetValue(0).ToString();
                    sqldr.Close();

                    //���Ӽ�����
                    sqlComm.CommandText = "UPDATE �������� SET ���� = ���� + 1 WHERE (�ؼ��� = N'" + strKey + "')";
                    sqlComm.ExecuteNonQuery();
                }
                else
                    sqldr.Close();

                if (strCount != "")
                {
                    strCount = string.Format("{0:D3}", Int32.Parse(strCount));
                    strCount = strKey.ToUpper() + Convert.ToDateTime(strDateSYS).ToString("yyyyMMdd") + strCount;
                }
                else
                {
                    MessageBox.Show("���ݴ���", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sqlConn.Close();
                    return;
                }

                //������
                sqlComm.CommandText = "INSERT INTO ����̵���ܱ�(���ݱ��, ����, ҵ��ԱID, ����ԱID, �̵���, ��ע, �ⷿID, ��ƷID, ����ID, �����ϼ�, ���ϼ�, ���������ϼ�, ������ϼ�, BeActive) VALUES (N'" + strCount + "', '" + strDateSYS + "', " + comboBoxYWY.SelectedValue.ToString() + ", " + intUserID.ToString() + ", 0, N'" + textBoxBZ.Text.Trim() + "', " + intKFID.ToString() + ", " + intCommID.ToString() + ", " + intClassID.ToString() + ", " + labelSLHJ.Text + ", " + labelJEHJ.Text + ", 0, 0, 1)";
                sqlComm.ExecuteNonQuery();


                //ȡ�õ��ݺ� 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //������ϸ
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "INSERT INTO ����̵���ϸ�� (����ID, ��ƷID, �������, �����, ʵ������, ��ע, �̵��־,�ⷿID) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[3].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ", 0, N'" + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + "', 0, " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }





                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ݿ����" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                saveToolStripButton.Enabled = true;
                return;
            }
            finally
            {
                sqlConn.Close();
            }

            //MessageBox.Show("����̵����ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = "����̵��" + labelDJBH.Text;
            isSaved = true;
            if (MessageBox.Show("����̵����ɹ����Ƿ������ʼ��һ�ݵ��ݣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
                mdiT.׼���̵��AToolStripMenuItem_Click(null, null);
            }


            if (MessageBox.Show("�Ƿ�ر��Ƶ�����", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }


        }

        private void FormKCSPPD1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
            {
                return;
            }

            DialogResult dr = MessageBox.Show(this, "������δ���棬ȷ��Ҫ�˳���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {

            string strT = "����̵��(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";�ⷿ���ƣ�" + textBoxKFMC.Text +";�����ϼƣ�"+labelSLHJ.Text+";���ϼƣ�"+labelJEHJ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "����̵��(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";�ⷿ���ƣ�" + textBoxKFMC.Text + ";�����ϼƣ�" + labelSLHJ.Text + ";���ϼƣ�" + labelJEHJ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }



        private void textBoxSPLB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getClassInformation(20, textBoxSPLB.Text) == 0) //ʧ��
                {
                    textBoxSPLB.Text = "";
                    intClassID = 0;
                }
                else
                {
                    intClassID = cGetInformation.iClassNumber;
                    textBoxSPLB.Text = cGetInformation.strClassName;
                }
                initdataGridViewDJMX();
            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {

                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0) //ʧ��
                {
                    textBoxSPMC.Text = "";
                    intCommID = 0;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPMC.Text = cGetInformation.strCommName;

                }
                initdataGridViewDJMX();
            }
        }

    }
}