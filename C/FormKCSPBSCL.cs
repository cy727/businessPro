using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKCSPBSCL : Form
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

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;

        public FormKCSPBSCL()
        {
            InitializeComponent();
        }

        private void FormFormKCSPBSCL_Load(object sender, EventArgs e)
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
                //dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
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
            sqlComm.CommandText = "SELECT ��汨����ܱ�.���ݱ��, ��汨����ܱ�.����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ����, ��汨����ܱ�.��ע, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ���� FROM ��汨����ܱ� INNER JOIN ְԱ�� ON ��汨����ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ��汨����ܱ�.����ԱID = ����Ա.ID INNER JOIN �ⷿ�� ON ��汨����ܱ�.�ⷿID = �ⷿ��.ID WHERE (��汨����ܱ�.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy��M��dd��");

                comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                textBoxPDKF.Text = sqldr.GetValue(5).ToString();
                textBoxKFMC.Text = sqldr.GetValue(6).ToString();

                this.Text = "����̵��" + labelDJBH.Text;
            }
            sqldr.Close();

            comboBoxBM.SelectedIndexChanged -= comboBoxBM_SelectedIndexChanged;
            sqlComm.CommandText = "SELECT ���ű�.�������� FROM ���ű� INNER JOIN ְԱ�� ON ���ű�.ID = ְԱ��.��λID WHERE (ְԱ��.ְԱ���� = N'" + comboBoxYWY.Text + "')";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                comboBoxBM.Text = sqldr.GetValue(0).ToString();
                break;
            }
            sqldr.Close();
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;

            //��ʼ����Ʒ�б�
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, ��汨����ϸ��.��������, ��汨����ϸ��.�ɱ�����, ��汨����ϸ��.������, ��汨����ϸ��.��ע, ��汨����ϸ��.��ƷID, ��汨����ϸ��.ԭ������� FROM ��汨����ϸ�� INNER JOIN ��Ʒ�� ON ��汨����ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��汨����ܱ� ON ��汨����ϸ��.����ID = ��汨����ܱ�.ID WHERE (��汨����ϸ��.����ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
            sqlDA.Fill(dSet, "���ݱ�");
            dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];


            dataGridViewDJMX.Columns[8].Visible = false;

            dataGridViewDJMX.Columns[5].Visible = false;
            dataGridViewDJMX.Columns[6].Visible = false;

            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

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

        private void textBoxPDKF_DoubleClick(object sender, EventArgs e)
        {
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
        }

        private void textBoxPDKF_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(10, textBoxPDKF.Text.Trim()) == 0) //ʧ��
                {
                    intKFID = 0;
                    textBoxPDKF.Text = "";
                    textBoxKFMC.Text = "";
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxPDKF.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                    initdataGridViewDJMX();
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
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxPDKF.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                    initdataGridViewDJMX();
                }
            }
        }
        private void initdataGridViewDJMX()
        {
            if (intKFID == 0) //û�пⷿ
            {
                if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
                return;
            }

            strSelect = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, ����̵㶨���.�������� AS ��������, ����.���ɱ��� AS �ɱ�����, ����̵㶨���.������ AS ������, ����̵㶨���.��ע, ����.��ƷID, ����.������� FROM ���� INNER JOIN ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID CROSS JOIN ����̵㶨��� WHERE (����.�ⷿID = " + intKFID.ToString() + ")";

            if (intCommID != 0) //��Ʒ����
            {
                strSelect += " AND (����.��ƷID = " + intCommID.ToString() + ")";
            }

            if (intClassID != 0) //��Ʒ�������
            {
                strSelect += "  AND (��Ʒ��.������ = " + intClassID.ToString() + ")";
            }

            sqlConn.Open();
            sqlComm.CommandText = strSelect;

            if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
            sqlDA.Fill(dSet, "���ݱ�");
            dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];

            sqlConn.Close();

            dataGridViewDJMX.Columns[8].Visible = false;
            dataGridViewDJMX.Columns[0].ReadOnly = true;
            dataGridViewDJMX.Columns[1].ReadOnly = true;
            dataGridViewDJMX.Columns[2].ReadOnly = true;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[5].ReadOnly = true;
            dataGridViewDJMX.Columns[6].ReadOnly = true;
            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].Visible = false;
            dataGridViewDJMX.Columns[6].Visible = false;

        }


        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("���������ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private bool countAmount()
        {
            decimal fSum = 0;
            decimal fCount = 0, fCSum = 0, fCSum1 = 0;
            //this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                //����������־
                if (dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[4].Value = 0;
                }
                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value)==0)
                    continue;

                //������
                dataGridViewDJMX.Rows[i].Cells[6].Value = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString());


                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value.ToString());
                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString());

                fCount += 1;
            }
            //this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelSLHJ.Text = fCSum.ToString();
            labelJEHJ.Text = fSum.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelJEHJ.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return true;
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i;
            decimal fTemp, fTemp1;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;
            string sTemp="0";

            //�������
            if (isSaved)
            {
                MessageBox.Show("�����Ʒ������Ѿ�����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intKFID == 0)
            {
                MessageBox.Show("��ѡ����Ʒ�ⷿ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!countAmount())
            {
                MessageBox.Show("������ϸ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("û�б�����Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (MessageBox.Show("��������Ʒ��������ݣ��Ƿ�������棿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;
            saveToolStripButton.Enabled = false;
            string strCount = "", strDateSYS = "", strKey = "CBS";
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
                sqlComm.CommandText = "INSERT INTO ��汨����ܱ� (���ݱ��, ����, ҵ��ԱID, ����ԱID, ��ע, �ⷿID, ��ƷID, ����ID, ���������ϼ�, ������ϼ�, BeActive) VALUES (N'"+strCount+"', '"+strDateSYS+"', "+comboBoxYWY.SelectedValue.ToString()+", "+intUserID.ToString()+", N'"+textBoxBZ.Text+"', "+intKFID.ToString()+", "+intCommID.ToString()+", "+intClassID.ToString()+", "+labelSLHJ.Text+", "+labelJEHJ.Text+", 1)";
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

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value) == 0)
                        continue;

                    if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "")
                    {
                        sTemp = "0";
                    }
                    else
                    {
                        sTemp = dataGridViewDJMX.Rows[i].Cells[9].Value.ToString();
                    }

                    sqlComm.CommandText = "INSERT INTO ��汨����ϸ�� (����ID, ��ƷID, ��������, ������, �ɱ�����, ��ע, ԭ�������) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + "',"+sTemp+")";
                    sqlComm.ExecuteNonQuery();



                    //�ܿ��
                    sqlComm.CommandText = "SELECT �������, �����, ���ɱ��� FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    //���ɱ���
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value.ToString());
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString());

                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                    }
                    dKUL -= dKUL1;
                    dKCJE -= dKCJE1;
                    sqldr.Close();

                    sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dKUL.ToString() + ", ����� = " + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //��ʷ��¼
                    sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, �ܽ������, �ܽ����, ��������, ���𵥼�, ������, BeActive) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'�����Ʒ����', " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", 1)";
                    sqlComm.ExecuteNonQuery();



                    //���ķֿ��
                    fTemp = 0;
                    sqlComm.CommandText = "SELECT  �������, �����, ���ɱ���  FROM ���� WHERE (�ⷿID = " + intKFID.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    //���ɱ���
                    dKUL = 0; dKCJE = 0; dKCCBJ = 0;
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                    }
                    sqldr.Close();

                    //�����
                    dKUL -= dKUL1;
                    dKCJE -= dKCJE1;

                    sqlComm.CommandText = "UPDATE ���� SET ������� = " + dKUL.ToString() + ", ����� = " + dKCJE.ToString() + " WHERE (�ⷿID = " + intKFID.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //�ⷿ��ʷ��¼
                    sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, �ⷿ�������, �ⷿ�����, ��������, ���𵥼�, ������, BeActive) VALUES (" + intKFID.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'�����Ʒ����', " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", 1)";
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


            //MessageBox.Show("�����Ʒ�������ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = "�����Ʒ�����" + labelDJBH.Text;
            isSaved = true;

            if (MessageBox.Show("�����Ʒ�������ɹ����Ƿ�ر��Ƶ�����", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }


        }

        private void FormKCSPBSCL_FormClosing(object sender, FormClosingEventArgs e)
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
            string strT = "����̵��(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";�ⷿ���ƣ�" + textBoxKFMC.Text + ";���������ϼƣ�" + labelSLHJ.Text + ";������ϼƣ�" + labelJEHJ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "����̵��(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";�ⷿ���ƣ�" + textBoxKFMC.Text + ";���������ϼƣ�" + labelSLHJ.Text + ";������ϼƣ�" + labelJEHJ.Text;
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

                    //�õ��ⷿ
                    intKFID = cGetInformation.iKFNumber;
                    textBoxPDKF.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;

                }
                initdataGridViewDJMX();
            }
        }





    }
}