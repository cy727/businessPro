using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKCSPZZ : Form
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
        private int intClassID = 0;

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;

        public FormKCSPZZ()
        {
            InitializeComponent();
        }

        private void FormKCSPZZ_Load(object sender, EventArgs e)
        {
            int i;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            this.Top = 1;
            this.Left = 1;


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


            //��ʼ������б�
            sqlComm.CommandText = "SELECT �����Ʒ��װ��ϸ��.ID, ��Ʒ��.��Ʒ����,��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���,�ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����Ʒ��װ��ϸ��.�������, �����Ʒ��װ��ϸ��.�ɱ�����, �����Ʒ��װ��ϸ��.�ɱ����, �����Ʒ��װ��ϸ��.��ע, �����Ʒ��װ��ϸ��.���ID, �����Ʒ��װ��ϸ��.�ⷿID, �����Ʒ��װ�����.�����, �����Ʒ��װ�����.ͳ�Ʊ�־ FROM �����Ʒ��װ��ϸ�� INNER JOIN ��Ʒ�� ON �����Ʒ��װ��ϸ��.���ID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON �����Ʒ��װ��ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN �����Ʒ��װ����� WHERE (�����Ʒ��װ��ϸ��.ID = 0)";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[7].ReadOnly = true;
            dataGridViewDJMX.Columns[8].ReadOnly = true;
            dataGridViewDJMX.Columns[12].ReadOnly = true;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;

            sqlConn.Close();

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;
        }

        private void initDJ()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT �����Ʒ��װ���ܱ�.���ݱ��, �����Ʒ��װ���ܱ�.����, ְԱ��.ְԱ����, ����Ա.ְԱ���� AS ����Ա, �����Ʒ��װ���ܱ�.��ע, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����Ʒ��װ���ܱ�.��Ʒ���, �����Ʒ��װ���ܱ�.��Ʒ����, �����Ʒ��װ���ܱ�.��Ʒ����, �����Ʒ��װ���ܱ�.��װ���� FROM �����Ʒ��װ���ܱ� INNER JOIN �ⷿ�� ON �����Ʒ��װ���ܱ�.��Ʒ�ⷿID = �ⷿ��.ID INNER JOIN ְԱ�� ����Ա ON �����Ʒ��װ���ܱ�.����ԱID = ����Ա.ID INNER JOIN ְԱ�� ON �����Ʒ��װ���ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (�����Ʒ��װ���ܱ�.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy��M��dd��");

                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                textBoxKFBH.Text = sqldr.GetValue(5).ToString();
                textBoxKFMC.Text = sqldr.GetValue(6).ToString();

                textBoxSPBH.Text = sqldr.GetValue(7).ToString();
                textBoxSPMC.Text = sqldr.GetValue(8).ToString();
                numericUpDownCPSL.Value = Convert.ToDecimal(sqldr.GetValue(9).ToString());
                numericUpDownZZFY.Value = Convert.ToDecimal(sqldr.GetValue(10).ToString());



                this.Text = "�����Ʒ��װ�Ƶ���" + labelDJBH.Text;
            }
            sqldr.Close();

            //��ʼ����Ʒ�б�
            sqlComm.CommandText = "SELECT �����Ʒ��װ��ϸ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����Ʒ��װ��ϸ��.�������, �����Ʒ��װ��ϸ��.�ɱ�����, �����Ʒ��װ��ϸ��.�ɱ����, �����Ʒ��װ��ϸ��.��ע, �����Ʒ��װ��ϸ��.���ID, �����Ʒ��װ��ϸ��.�ⷿID, ��Ʒ��.�������, �����Ʒ��װ�����.ͳ�Ʊ�־ FROM �����Ʒ��װ��ϸ�� INNER JOIN ��Ʒ�� ON �����Ʒ��װ��ϸ��.���ID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON �����Ʒ��װ��ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN �����Ʒ��װ����� WHERE (�����Ʒ��װ��ϸ��.����ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFBH.Text.Trim()) == 0) //ʧ��
                {
                    intKFID = 0;
                    textBoxKFBH.Text = "";
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }
            }
        }

        private void textBoxKFMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFMC.Text.Trim()) == 0) //ʧ��
                {
                    intKFID = 0;
                    textBoxKFMC.Text = "";
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }
            }
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
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = cGetInformation.iCommNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommKCCBJ;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = Math.Round(Decimal.Zero, 2);
                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value.ToString() == "")
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 0;

                    cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value));
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCL;

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[4];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }

            if (e.ColumnIndex == 5 || e.ColumnIndex == 4) //�ⷿ���
            {
                if (cGetInformation.getKFInformation(1, "") == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;

                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value.ToString() == "")
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 0;

                    cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value));
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCL;
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
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[4];
                                        break;
                                    case 4:
                                    case 5:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[6];
                                        break;
                                    case 6:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[9];
                                        break;
                                    case 9:
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

        private bool countAmount()
        {
            decimal fSum = 0;
            decimal fSum1 = 0;
            decimal fCSum = 0;

            decimal fTemp, fTemp1;
            decimal fCount = 0;
            bool bCheck = true;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "������װ��Ʒ���";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "������װ��Ʒ���";
                    bCheck = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[4].ErrorText = "���������Ʒ�ⷿ";
                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "���������Ʒ�ⷿ";
                    bCheck = false;
                }


                if (dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "���������Ʒ����";
                    bCheck = false;
                }

                if (!bCheck)
                    continue;

                //�ɱ�����
                if (dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[7].Value = 0;
                }

                //�����
                if (dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[12].Value = 0;

                //��ɫ��ʾ
                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value)*numericUpDownCPSL.Value > Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[12].Value))
                    dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.LightPink;
                else
                    dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.White;


                //����
                fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                //����
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);

                //���
                dataGridViewDJMX.Rows[i].Cells[8].Value = Math.Round(fTemp * fTemp1, 2);

                fCount += 1;
                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelJEHJ.Text = fSum.ToString();
            labelSLHJ.Text = fCSum.ToString();
            toolStripStatusLabelMXJLS.Text = fCount.ToString();
            fSum1 = fSum + numericUpDownZZFY.Value;
            labelSPCB.Text = fSum1.ToString();
            return bCheck;

        }

        private void numericUpDownZZFY_ValueChanged(object sender, EventArgs e)
        {
            countAmount();
        }

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            switch (e.ColumnIndex)
            {
                case 2: //��Ʒ���
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
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

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value.ToString() == cGetInformation.iCommNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommKCCBJ.ToString();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;


                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;

                case 1: //��Ʒ����
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
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

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value.ToString() == cGetInformation.iCommNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommKCCBJ.ToString();

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;


                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    }
                    break;
                case 4: //�ⷿ���
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;

                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(10, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].ErrorText = "�ⷿ����������";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value.ToString() == cGetInformation.iKFNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;
                case 5: //�ⷿ����
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(20, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].ErrorText = "�ⷿ�������������";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value.ToString() == cGetInformation.iKFNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }

                    break;
                case 6:  //��Ʒ����
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut <= 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "��������������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "��������������ʹ���";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;

            }
            dataGridViewDJMX.EndEdit();


        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dKUL = 0, dKCCBJ = 0, dML = 0, dSJJE = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dML1 = 0, dSJJE1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;
            decimal fTemp=0,fTemp1=0;

            //�������
            if (isSaved)
            {
                MessageBox.Show("��Ʒ��װ���Ѿ�����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intClassID == 0)
            {
                MessageBox.Show("��������װ��Ʒ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (numericUpDownCPSL.Value == 0)
            {
                MessageBox.Show("��������װ��Ʒ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (textBoxSPBH.Text == "")
            {
                MessageBox.Show("��������װ��Ʒ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (textBoxSPMC.Text == "")
            {
                MessageBox.Show("��������װ��Ʒ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intKFID == 0)
            {
                MessageBox.Show("��ѡ���Ʒ�ⷿ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("��Ʒ��װ��ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("û����Ʒ��װ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (MessageBox.Show("������Ʒ��װ������,���Ƶ����ݲ��ɸ��ģ��Ƿ�������棿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;



            string strCount = "", strDateSYS = "", strKey = "CZZ";
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

                //��־��λ
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    dataGridViewDJMX.Rows[i].Cells[13].Value = 1;
                }

                //�ܿ��
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[13].Value) == 0) //�Ѿ������
                        continue;

                    //����õ���ÿ����Ʒ���
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[13].Value) == 0) //�Ѿ������
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[10].Value == dataGridViewDJMX.Rows[i].Cells[10].Value) //ͬ����Ʒ
                        {
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[6].Value);
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                            dataGridViewDJMX.Rows[j].Cells[13].Value = 0;
                        }

                    }
                    dKUL1 = dKUL1 * numericUpDownCPSL.Value;
                    dKCJE1=dKCJE1 * numericUpDownCPSL.Value;

                    //�ܿ����
                    sqlComm.CommandText = "SELECT �������, �����, ���ɱ��� FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                    }
                    sqldr.Close();

                    dKUL -= dKUL1;
                    dKCJE -= dKUL1;

                    sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dKUL.ToString() + ", �����="+dKCJE.ToString()+" WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //��Ʒ��ʷ��¼
                    sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, ��װ����, ��װ����, ��װ���, �ܽ������, �ܽ����, BeActive) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'�����Ʒ��װ', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", 1)";
                    sqlComm.ExecuteNonQuery();
                }

                //��־��λ
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    dataGridViewDJMX.Rows[i].Cells[13].Value = 1;
                }

                //�ֿ��
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[13].Value) == 0) //�Ѿ������
                        continue;

                    //����õ���ÿ����Ʒ����
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[13].Value) == 0) //�Ѿ������
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[10].Value == dataGridViewDJMX.Rows[i].Cells[10].Value && dataGridViewDJMX.Rows[j].Cells[11].Value == dataGridViewDJMX.Rows[i].Cells[11].Value) //ͬ����Ʒ��ͬ�����
                        {
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[6].Value);
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);

                            dataGridViewDJMX.Rows[j].Cells[13].Value = 0;
                        }

                    }
                    dKUL1 = dKUL1 * numericUpDownCPSL.Value;
                    dKCJE1 = dKCJE1 * numericUpDownCPSL.Value;

                    //�ֿ�����
                    sqlComm.CommandText = "SELECT �������, �����, ���ɱ��� FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ") AND (BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //���ڿ��
                    {
                        sqldr.Read();
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        sqldr.Close();

                        dKUL -= dKUL1;
                        dKCJE -= dKCJE1;
                        sqlComm.CommandText = "UPDATE ���� SET ������� = " + dKUL.ToString() + ", �����=" + dKCJE.ToString() + " WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ") AND (BeActive = 1)";
                        sqlComm.ExecuteNonQuery();

                        //��Ʒ�ⷿ��ʷ��¼
                        sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, ��װ����, ��װ����, ��װ���, �ⷿ�������, �ⷿ�����, BeActive) VALUES (" + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'�����Ʒ��װ', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", 1)";
                        sqlComm.ExecuteNonQuery();

                    }
                    sqldr.Close();

                }

                //��Ʒ����
                fTemp=Convert.ToDecimal(labelSPCB.Text)*numericUpDownCPSL.Value;
                sqlComm.CommandText = "INSERT INTO ��Ʒ�� (��Ʒ���, ��Ʒ����, �������, ���ɱ���, �����, ���ս���, ��߽���, ��ͽ���, ��װ��Ʒ, beactive, ������) VALUES (N'" + textBoxSPBH.Text + "', N'" + textBoxSPMC.Text + "', " + numericUpDownCPSL.Value.ToString() + ", " + labelSPCB.Text + ", " + fTemp.ToString() + ", " + labelSPCB.Text + ", " + labelSPCB.Text + ", " + labelSPCB.Text + ", 1, 1, " + intClassID.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //ȡ����ƷID
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sSPID = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //������
                sqlComm.CommandText = "INSERT INTO �����Ʒ��װ���ܱ� (���ݱ��, ��Ʒ�ⷿID, ��ƷID, ��Ʒ���, ��Ʒ����, ��Ʒ����, ��װ����, ��ע, BeActive, ����, ����ԱID, ҵ��ԱID) VALUES (N'" + strCount + "' ," + intKFID.ToString() + ", " + sSPID + ", N'" + textBoxSPBH.Text + "', N'" + textBoxSPMC.Text + "', " + numericUpDownCPSL.Value.ToString() + ", " + numericUpDownZZFY.Value.ToString() + ", N'" + textBoxBZ.Text + "', 1, '" + strDateSYS + "', "+intUserID.ToString()+", "+comboBoxYWY.SelectedValue.ToString()+")";
                sqlComm.ExecuteNonQuery();

                //ȡ�õ��ݺ� 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //����ϸ
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "INSERT INTO �����Ʒ��װ��ϸ�� (����ID, ���ID, �ⷿID, �������, �ɱ�����, �ɱ����, ��ע) VALUES ("+sBillNo+", "+dataGridViewDJMX.Rows[i].Cells[10].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[11].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[6].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[7].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[8].Value.ToString()+", N'"+dataGridViewDJMX.Rows[i].Cells[9].Value.ToString()+"')";
                    sqlComm.ExecuteNonQuery();
                }

                //���
                sqlComm.CommandText = "INSERT INTO ���� (�ⷿID, ��ƷID, �������, �����, ���ɱ���, BeActive) VALUES (" + intKFID.ToString() + ", " + sSPID + ", " + numericUpDownCPSL.Value.ToString() + ", " + fTemp .ToString()+ ", "+labelSPCB.Text+", 1)";
                sqlComm.ExecuteNonQuery();

                //��Ʒ��ʷ��¼
                dKUL1 = numericUpDownCPSL.Value;
                dKCCBJ1 = Convert.ToDecimal(labelSPCB.Text);
                dKCJE1 = dKUL * dKCCBJ;

                sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, �������, ��ⵥ��, �����, �ܽ������, �ܽ����, BeActive) VALUES ('" + strDateSYS + "', " + sSPID + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'�����Ʒ��װ', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCJE1.ToString() + ", 1)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, �������, ��ⵥ��, �����, �ⷿ�������, �ⷿ�����, BeActive) VALUES (" + intKFID.ToString() + ", '" + strDateSYS + "', " + sSPID + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'�����Ʒ��װ', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCJE1.ToString() + ", 1)";
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

            //MessageBox.Show(" �����Ʒ��װ�Ƶ�����ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = " �����Ʒ��װ�Ƶ���" + labelDJBH.Text;
            isSaved = true;


            if (MessageBox.Show("�����Ʒ��װ�Ƶ�����ɹ����Ƿ�ر��Ƶ�����", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }


        }


        private void numericUpDownCPSL_ValueChanged(object sender, EventArgs e)
        {
            countAmount();
        }

        private void FormKCSPZZ_FormClosing(object sender, FormClosingEventArgs e)
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
            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("�����Ʒ��װ�Ƶ���ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "�����Ʒ��װ�Ƶ�(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��Ʒ���ƣ�" + textBoxSPMC.Text + "(���:" + textBoxSPBH.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("�����Ʒ��װ�Ƶ���ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "�����Ʒ��װ�Ƶ�(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��Ʒ���ƣ�" + textBoxSPMC.Text + "(���:" + textBoxSPBH.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void textBoxSPLB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getClassInformation(10, textBoxSPLB.Text) == 0) //ʧ��
                {
                    textBoxSPLB.Text = "";
                    intClassID = 0;
                }
                else
                {
                    intClassID = cGetInformation.iClassNumber;
                    textBoxSPLB.Text = cGetInformation.strClassName;

                    //�õ�ȱʡ��
                    if (intClassID == 0)
                        return;

                    sqlConn.Open();
                    sqlComm.CommandText = "SELECT ��Ʒ�����.ID, ��Ʒ�����.������, ��Ʒ�����.��������, ��Ʒ�����.�ⷿID, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ���� FROM ��Ʒ����� INNER JOIN �ⷿ�� ON ��Ʒ�����.�ⷿID = �ⷿ��.ID WHERE (��Ʒ�����.BeActive = 1) AND (��Ʒ�����.ID = " + intClassID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    while (sqldr.Read())
                    {
                        intKFID = Convert.ToInt32(sqldr.GetValue(3).ToString());
                        textBoxKFBH.Text = sqldr.GetValue(4).ToString();
                        textBoxKFMC.Text = sqldr.GetValue(5).ToString();
                    }
                    sqldr.Close();
                    sqlConn.Close();
                }
            }
        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                sqlConn.Open();
                sqlComm.CommandText = "SELECT ��Ʒ���, ��Ʒ����, ��Ʒ��� FROM ��Ʒ�� WHERE (��Ʒ��� = N'" + textBoxSPBH.Text + "') AND (beactive = 1)";
                sqldr = sqlComm.ExecuteReader();

                if (sqldr.HasRows) //�����ظ�
                {
                    sqldr.Read();
                    MessageBox.Show("��װ��Ʒ�����ظ�����Ʒ����Ϊ��" + sqldr.GetValue(1).ToString() + "�����" + sqldr.GetValue(2).ToString(), "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxSPBH.Text = "";
                }
                sqlConn.Close();
            }
        }

        private void textBoxSPLB_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getClassInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                intClassID = cGetInformation.iClassNumber;
                textBoxSPLB.Text = cGetInformation.strClassName;

                //�õ�ȱʡ��
                if (intClassID == 0)
                    return;

                sqlConn.Open();
                sqlComm.CommandText = "SELECT ��Ʒ�����.ID, ��Ʒ�����.������, ��Ʒ�����.��������, ��Ʒ�����.�ⷿID, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ���� FROM ��Ʒ����� INNER JOIN �ⷿ�� ON ��Ʒ�����.�ⷿID = �ⷿ��.ID WHERE (��Ʒ�����.BeActive = 1) AND (��Ʒ�����.ID = " + intClassID .ToString()+ ")";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    intKFID = Convert.ToInt32(sqldr.GetValue(3).ToString());
                    textBoxKFBH.Text = sqldr.GetValue(4).ToString();
                    textBoxKFMC.Text = sqldr.GetValue(5).ToString();
                }
                sqldr.Close();
                sqlConn.Close();
            }
        }



    }
}