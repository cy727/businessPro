using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKCSXXWH : Form
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

        private ClassGetInformation cGetInformation;

        private bool isSaved = false;
        
        public FormKCSXXWH()
        {
            InitializeComponent();
        }

        private void FormKCSXXWH_Load(object sender, EventArgs e)
        {
            int i;

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

            initDataView();

        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAll.Checked) //�ܿⷿ
            {
                intKFID = 0;
                textBoxKFBH.Text = "";
                textBoxKFMC.Text = "";
            }
            else
            {
                if (intKFID == 0)
                {
                    intKFID = 0;
                    textBoxKFBH.Text = "";
                    textBoxKFMC.Text = "";
                    checkBoxAll.CheckedChanged -= checkBoxAll_CheckedChanged;
                    checkBoxAll.Checked = true;
                    checkBoxAll.CheckedChanged += checkBoxAll_CheckedChanged;
                }
            }
            initDataView();
        }

        private void initDataView()
        {
            if (intKFID == 0)
            {
                intKFID = 0;
                textBoxKFBH.Text = "";
                textBoxKFMC.Text = "";
                checkBoxAll.CheckedChanged -= checkBoxAll_CheckedChanged;
                checkBoxAll.Checked = true;
                checkBoxAll.CheckedChanged += checkBoxAll_CheckedChanged;
            }
            else
            {
                checkBoxAll.CheckedChanged -= checkBoxAll_CheckedChanged;
                checkBoxAll.Checked = false;
                checkBoxAll.CheckedChanged += checkBoxAll_CheckedChanged;
            }
            
            sqlConn.Open();
            
            //��ʼ���б�
            if(intKFID==0) //�ܿ��
                sqlComm.CommandText = "SELECT ID, ��Ʒ����, ��Ʒ���, ��Ʒ���, �������, ����������, ����������, ������� FROM ��Ʒ�� WHERE  (��Ʒ��.beactive = 1)";
            else //�ֿ��
                sqlComm.CommandText = "SELECT ��Ʒ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ����.�������, ����.����������, ����.����������, ����.������� FROM ��Ʒ�� INNER JOIN ���� ON ��Ʒ��.ID = ����.��ƷID WHERE (����.�ⷿID = " + intKFID.ToString() + ") AND (��Ʒ��.beactive = 1) AND (��Ʒ��.��װ��Ʒ = 0)";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[1].ReadOnly = true;
            dataGridViewDJMX.Columns[2].ReadOnly = true;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;


            sqlConn.Close();
        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                intKFID = cGetInformation.iKFNumber;
                textBoxKFBH.Text = cGetInformation.strKFCode;
                textBoxKFMC.Text = cGetInformation.strKFName;
            }
            initDataView();
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(10, textBoxKFBH.Text.Trim()) == 0) //ʧ��
                {
                    intKFID = 0;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }
                initDataView();
            }
        }

        private void textBoxKFMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFMC.Text.Trim()) == 0) //ʧ��
                {
                    intKFID = 0;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }
                initDataView();
            }
        }

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            switch (e.ColumnIndex)
            {
               case 4:
               case 5:
               case 6:
               case 7: 
                    decimal intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Decimal.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut < 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "����ֵ�������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "����ֵ�������ʹ���";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;

            }
            dataGridViewDJMX.EndEdit();

        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("���������ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool countAmount()
        {
            decimal fCount = 0;
            bool bCheck = true;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[4].Value = 0;
                }
                if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                }
                if (dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[6].Value = 0;
                }
                if (dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[7].Value = 0;
                }

                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value) < Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value))
                {
                    dataGridViewDJMX.Rows[i].Cells[5].Style.BackColor = Color.LightPink;
                    bCheck = false;
                }
                else
                    dataGridViewDJMX.Rows[i].Cells[5].Style.BackColor = Color.White;

                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value) < Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value))
                {
                    dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.LightPink;
                    bCheck = false;
                }
                else
                    dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.White;

                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value) < Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value))
                {
                    dataGridViewDJMX.Rows[i].Cells[7].Style.BackColor = Color.LightPink;
                    bCheck = false;
                }
                else
                    dataGridViewDJMX.Rows[i].Cells[7].Style.BackColor = Color.White;

                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value) < Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value))
                {
                    dataGridViewDJMX.Rows[i].Cells[4].Style.BackColor = Color.LightPink;
                    bCheck = false;
                }
                else
                    dataGridViewDJMX.Rows[i].Cells[4].Style.BackColor = Color.White;

                fCount++;

            }
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;

        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            Control ctr = (Control)Control.FromHandle(msg.HWnd);

            if (ctr != null)
            {
                if (ctr.GetType() == typeof(System.Windows.Forms.DataGridViewTextBoxEditingControl))
                {
                    DataGridViewTextBoxEditingControl dvTextBoxEC = (DataGridViewTextBoxEditingControl)FromHandle(msg.HWnd);
                    DataGridView dv = (DataGridView)dvTextBoxEC.EditingControlDataGridView;
                    if (dv.Columns.Count > 0)
                    {
                        if (keyData == Keys.Enter)
                        {
                            try
                            {
                                dv.EndEdit();
                                switch (dv.CurrentCell.ColumnIndex)
                                {
                                    case 4:
                                    case 5:
                                    case 6:

                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[dv.CurrentCell.ColumnIndex+1];
                                        break;
                                    case 7:
                                        if (dv.CurrentCell.RowIndex == dv.Rows.Count-1)
                                            dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[4];
                                        else
                                            dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex+1].Cells[4];
                                        break;
                                    default:
                                        break;
                                }
                                dv.BeginEdit(true);
                            }
                            catch (Exception)
                            {
                            }
                            return true;
                        }
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            int i;
            decimal dTemp;

            dataGridViewDJMX.CellValidating-=dataGridViewDJMX_CellValidating;
            for (i = 0; i < dataGridViewDJMX.SelectedRows.Count; i++)
            {
                dTemp=Convert.ToDecimal(dataGridViewDJMX.SelectedRows[i].Cells[4].Value)+numericUpDownQJJG.Value;
                dataGridViewDJMX.SelectedRows[i].Cells[5].Value = dTemp;

                dTemp = Convert.ToDecimal(dataGridViewDJMX.SelectedRows[i].Cells[7].Value) - numericUpDownQJJG.Value;
                if (dTemp < 0)
                    dTemp = 0;

                dataGridViewDJMX.SelectedRows[i].Cells[6].Value = dTemp;
            }
            dataGridViewDJMX.CellValidating+=dataGridViewDJMX_CellValidating;

        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dKUL = 0;
            decimal dKUL1 = 0;
            decimal fTemp = 0, fTemp1 = 0;

            //�������
            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("û����Ʒ��涨��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!countAmount())
            {
                MessageBox.Show("��Ʒ��������޶������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (intKFID == 0)
                    {
                        sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ", ���������� = " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", ���������� = " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", ������� = " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        sqlComm.CommandText = "UPDATE ���� SET ������� = " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ", ���������� = " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", ���������� = " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", ������� = " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + " WHERE (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                    }
                    else
                    {
                        sqlComm.CommandText = "UPDATE ���� SET ������� = " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ", ���������� = " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", ���������� = " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", ������� = " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + " WHERE (�ⷿID = " + intKFID.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();
                    }

                    
                }

                sqlta.Commit();
                isSaved = true;
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

            //MessageBox.Show(" ��������޶����޸ĳɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (MessageBox.Show("��������޶����޸ĳɹ����Ƿ�رմ���", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }


        }

        private void FormKCSXXWH_FormClosing(object sender, FormClosingEventArgs e)
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

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��������޶���;���ڣ�" + labelZDRQ.Text + ";����Ա��" +labelCZY.Text ;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��������޶���;���ڣ�" + labelZDRQ.Text + ";����Ա��" + labelCZY.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }


    }
}