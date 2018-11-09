using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormTJTZD1 : Form
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

        public bool isSaved = false;
        public int iDJID = 2;

        public FormTJTZD1()
        {
            InitializeComponent();
        }

        private void FormTJTZD1_Load(object sender, EventArgs e)
        {
            int i;
            
            this.Left = 1;
            this.Top = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            if (isSaved)
            {
                dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                initDJ();
                return;
            }


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


            //��ʼ����Ʒ�б�
            sqlComm.CommandText = "SELECT ����֪ͨ����ϸ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, ����֪ͨ����ϸ��.ԭ����, ����֪ͨ����ϸ��.����, ����֪ͨ����ϸ��.ԭ������, ����֪ͨ����ϸ��.������, ����֪ͨ����ϸ��.��ƷID FROM ����֪ͨ����ϸ�� INNER JOIN ��Ʒ�� ON ����֪ͨ����ϸ��.��ƷID = ��Ʒ��.ID WHERE (����֪ͨ����ϸ��.ID = 0)";

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

            //initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;
        }

        private void initDJ()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ����֪ͨ�����ܱ�.���ݱ��, ����֪ͨ�����ܱ�.����, [ְԱ��_1].ְԱ����, ְԱ��.ְԱ����, ����֪ͨ�����ܱ�.��ע, ����֪ͨ�����ܱ�.ID FROM ����֪ͨ�����ܱ� INNER JOIN ְԱ�� [ְԱ��_1] ON ����֪ͨ�����ܱ�.ҵ��ԱID = [ְԱ��_1].ID INNER JOIN ְԱ�� ON ����֪ͨ�����ܱ�.����ԱID = ְԱ��.ID WHERE (����֪ͨ�����ܱ�.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy��M��dd��");

                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();


                this.Text = "����֪ͨ����" + labelDJBH.Text;
            }
            sqldr.Close();

            //��ʼ����Ʒ�б�
            sqlComm.CommandText = "SELECT ����֪ͨ����ϸ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, ����֪ͨ����ϸ��.ԭ����, ����֪ͨ����ϸ��.����, ����֪ͨ����ϸ��.ԭ������, ����֪ͨ����ϸ��.������, ����֪ͨ����ϸ��.��ƷID FROM ����֪ͨ����ϸ�� INNER JOIN ��Ʒ�� ON ����֪ͨ����ϸ��.��ƷID = ��Ʒ��.ID WHERE (����֪ͨ����ϸ��.����ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[9].Visible = false;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;

            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;

            sqlConn.Close();
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

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2) //��Ʒ���
            {
                if (cGetInformation.getCommInformation(1, "") == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.iCommNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.decCommJJ;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.decCommJJ;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommPFJ;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.decCommPFJ;

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[6];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }

        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("���������ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                    case 1:
                                    case 2:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[6];
                                        break;
                                    case 6:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                        break;
                                    case 8:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[1];
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

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            decimal dOut = 0;

            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            switch (e.ColumnIndex)
            {
                case 2: //��Ʒ���
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";

                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getCommInformation(20, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].ErrorText = "��Ʒ����������";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value.ToString() == cGetInformation.iCommNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.decCommJJ;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.decCommJJ;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommPFJ;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.decCommPFJ;

                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;

                case 1: //��Ʒ����
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                        break;

                    }

                    if (cGetInformation.getCommInformation(10, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].ErrorText = "��Ʒ�������������";
                    }
                    else
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value.ToString() == cGetInformation.iCommNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.decCommJJ;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.decCommJJ;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommPFJ;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.decCommPFJ;

                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    }
                    break;
                case 6:  //����
                    dOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Decimal.TryParse(e.FormattedValue.ToString(), out dOut))
                    {
                        if (dOut < 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "��Ʒ�����������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "��Ʒ�����������ʹ���";
                        e.Cancel = true;
                    }
                    break;
                case 8:  //������
                    dOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Decimal.TryParse(e.FormattedValue.ToString(), out dOut))
                    {
                        if (dOut < 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "��Ʒ�������������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "��Ʒ�������������ʹ���";
                        e.Cancel = true;
                    }
                    break;
                default:
                    break;

            }
            dataGridViewDJMX.EndEdit();

        }

        private bool countAmount()
        {
            bool bCheck = true;
            decimal fCount = 0;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "���������Ʒ";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "���������Ʒ";
                    bCheck = false;
                }


                if (dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "���������Ʒ����";
                    bCheck = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[8].ErrorText = "���������Ʒ������";
                    bCheck = false;
                }

                if (!bCheck)
                    continue;

                fCount++;

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            toolStripStatusLabelMXJLS.Text = fCount.ToString();
            return bCheck;
        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dKUL = 0;
            decimal dKUL1 = 0;

            //�������
            if (isSaved)
            {
                MessageBox.Show("����֪ͨ���Ѿ�����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("û�е���֪ͨ����Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("����֪ͨ����ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("�������֪ͨ�����ݣ��Ƿ�������棿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;



            string strCount = "", strDateSYS = "", strKey = "ETJ";
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
                sqlComm.CommandText = "INSERT INTO ����֪ͨ�����ܱ� (���ݱ��, ����, ҵ��ԱID, ����ԱID, ��ע, BeActive, ִ�б��) VALUES (N'" + strCount + "', '" + strDateSYS + "', " + comboBoxYWY.SelectedValue.ToString() + ", " + intUserID.ToString() + ", N'" + textBoxBZ.Text + "', 1, 0)";
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

                    sqlComm.CommandText = "INSERT INTO ����֪ͨ����ϸ�� (����ID, ��ƷID, ԭ����, ����, ԭ������, ������) VALUES ("+sBillNo+", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ")";
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

            MessageBox.Show("����֪ͨ������ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = "����֪ͨ����" + labelDJBH.Text;
            isSaved = true;

            if (MessageBox.Show("����֪ͨ������ɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }

        }

        private void FormTJTZD1_FormClosing(object sender, FormClosingEventArgs e)
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
            if (!countAmount())
            {
                MessageBox.Show("����֪ͨ����ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "����֪ͨ��(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            if (!countAmount())
            {
                MessageBox.Show("����֪ͨ����ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "����֪ͨ��(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }


    }
}