using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKCSPCS : Form
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

        private int intKFID = 0;
        private int intCommID = 0;
        private int intCZZ = 0;

        private decimal dKC = 0;
        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;
        
        public FormKCSPCS()
        {
            InitializeComponent();
        }

        private void FormKCSPCS_Load(object sender, EventArgs e)
        {
            int i;

            this.Top = 1;
            this.Left = 1;

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
            sqlComm.CommandText = "SELECT �����Ʒ��ɢ���ܱ�.���ݱ��, �����Ʒ��ɢ���ܱ�.����, [ְԱ��_1].ְԱ����, ����Ա.ְԱ���� AS ����Ա, �����Ʒ��ɢ���ܱ�.��ע, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����Ʒ��װ���ܱ�.��Ʒ���, �����Ʒ��װ���ܱ�.��Ʒ����, �����Ʒ��ɢ���ܱ�.��ɢ���� FROM �����Ʒ��ɢ���ܱ�  INNER JOIN ְԱ�� ����Ա ON �����Ʒ��ɢ���ܱ�.����ԱID = ����Ա.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����Ʒ��ɢ���ܱ�.ҵ��ԱID = [ְԱ��_1].ID INNER JOIN �����Ʒ��װ���ܱ� ON  �����Ʒ��ɢ���ܱ�.��װ����ID = �����Ʒ��װ���ܱ�.ID INNER JOIN �ⷿ�� ON �����Ʒ��װ���ܱ�.��Ʒ�ⷿID = �ⷿ��.ID WHERE (�����Ʒ��ɢ���ܱ�.ID = " + iDJID.ToString() + ")";
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


                this.Text = "�����Ʒ��ɢ�Ƶ���" + labelDJBH.Text;
            }
            sqldr.Close();

            //��ʼ����Ʒ�б�
            sqlComm.CommandText = "SELECT �����Ʒ��װ��ϸ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����Ʒ��װ��ϸ��.�������, �����Ʒ��װ��ϸ��.�ɱ�����, �����Ʒ��װ��ϸ��.�ɱ����, �����Ʒ��װ��ϸ��.��ע, �����Ʒ��װ��ϸ��.���ID, �����Ʒ��װ��ϸ��.�ⷿID, ��Ʒ��.�������, �����Ʒ��װ�����.ͳ�Ʊ�־ FROM �����Ʒ��װ��ϸ�� INNER JOIN ��Ʒ�� ON �����Ʒ��װ��ϸ��.���ID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON �����Ʒ��װ��ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN �����Ʒ��ɢ���ܱ� ON  �����Ʒ��װ��ϸ��.ID = �����Ʒ��ɢ���ܱ�.��װ����ID CROSS JOIN �����Ʒ��װ����� WHERE (�����Ʒ��ɢ���ܱ�.ID = " + iDJID.ToString() + ")";

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

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(101, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPBH.Text = cGetInformation.strCommCode;
                textBoxSPMC.Text = cGetInformation.strCommName;


            }

            getCPZZDetail();
        }

        private void getCPZZDetail()
        {
            if (intCommID == 0)
                return;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.�������, ��Ʒ��.���ɱ���, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����Ʒ��װ���ܱ�.��װ����, �����Ʒ��װ���ܱ�.��ע, �����Ʒ��װ���ܱ�.ID, �����Ʒ��װ���ܱ�.��Ʒ�ⷿID FROM ��Ʒ�� INNER JOIN �����Ʒ��װ���ܱ� ON ��Ʒ��.ID = �����Ʒ��װ���ܱ�.��ƷID INNER JOIN �ⷿ�� ON �����Ʒ��װ���ܱ�.��Ʒ�ⷿID = �ⷿ��.ID WHERE (��Ʒ��.ID = " + intCommID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();

            if (!sqldr.HasRows)
            {
                intCommID = 0;
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            sqldr.Read();
            textBoxSPBH.Text = sqldr.GetValue(0).ToString();
            textBoxSPMC.Text = sqldr.GetValue(1).ToString();
            numericUpDownCPSL.Maximum = Convert.ToDecimal(sqldr.GetValue(2).ToString());
            numericUpDownCPSL.Value= Convert.ToDecimal(sqldr.GetValue(2).ToString());
            dKC = Convert.ToDecimal(sqldr.GetValue(2).ToString());
            labelSPCB.Text = sqldr.GetValue(3).ToString();
            textBoxKFBH.Text = sqldr.GetValue(4).ToString();
            textBoxKFMC.Text = sqldr.GetValue(5).ToString();
            numericUpDownZZFY.Value = Convert.ToDecimal(sqldr.GetValue(6).ToString());
            intCZZ = Int32.Parse(sqldr.GetValue(8).ToString());
            intKFID = Int32.Parse(sqldr.GetValue(9).ToString());
            sqldr.Close();

            //��ϸ
            sqlComm.CommandText = "SELECT �����Ʒ��װ��ϸ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����Ʒ��װ��ϸ��.�������, �����Ʒ��װ��ϸ��.�ɱ�����, �����Ʒ��װ��ϸ��.�ɱ����, �����Ʒ��װ��ϸ��.��ע, �����Ʒ��װ��ϸ��.���ID, �����Ʒ��װ��ϸ��.�ⷿID, ��Ʒ��.�������, �����Ʒ��װ�����.ͳ�Ʊ�־ FROM �����Ʒ��װ��ϸ�� INNER JOIN ��Ʒ�� ON �����Ʒ��װ��ϸ��.���ID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON �����Ʒ��װ��ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN �����Ʒ��װ����� WHERE (�����Ʒ��װ��ϸ��.����ID = "+intCZZ.ToString()+")";
            if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
            sqlDA.Fill(dSet, "���ݱ�");
            dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;
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
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString();


            sqlConn.Close();
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(102, textBoxSPMC.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;


                }

                getCPZZDetail();
            }
        }


        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(102, textBoxSPBH.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;


                }

                getCPZZDetail();
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dKUL = 0, dKCCBJ = 0, dML = 0, dSJJE = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dML1 = 0, dSJJE1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;
            decimal fTemp = 0, fTemp1 = 0, fTemp2=0;

            //�������
            if (isSaved)
            {
                MessageBox.Show("��Ʒ��ɢ���Ѿ�����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intCommID == 0)
            {
                MessageBox.Show("��ѡ���ɢ��Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (numericUpDownCPSL.Value == 0)
            {
                MessageBox.Show("�������ɢ��Ʒ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }




            if (MessageBox.Show("������Ʒ��ɢ������,���Ƶ����ݲ��ɸ��ģ��Ƿ�������棿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            string strCount = "", strDateSYS = "", strKey = "CCS";
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

                    //����õ���ÿ����Ʒ���,���
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
                    dKCJE1 = dKCJE1 * numericUpDownCPSL.Value;

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

                    dKUL += dKUL1;
                    dKCJE += dKUL1;

                    sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dKUL.ToString() + ", �����=" + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //��Ʒ��ʷ��¼
                    sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, ��װ����, ��װ����, ��װ���, �ܽ������, �ܽ����, BeActive) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'�����Ʒ��ɢ', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", 1)";
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

                        dKUL += dKUL1;
                        dKCJE += dKCJE1;
                        sqlComm.CommandText = "UPDATE ���� SET ������� = " + dKUL.ToString() + ", �����=" + dKCJE.ToString() + " WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ") AND (BeActive = 1)";
                        sqlComm.ExecuteNonQuery();

                        //��Ʒ�ⷿ��ʷ��¼
                        sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, ��װ����, ��װ����, ��װ���, �ⷿ�������, �ⷿ�����, BeActive) VALUES (" + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'�����Ʒ��ɢ', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", 1)";
                        sqlComm.ExecuteNonQuery();

                    }

                }

                if (dKC == numericUpDownCPSL.Value) //ȫ����ɢ
                {
                    sqlComm.CommandText = "UPDATE ��Ʒ�� SET beactive = 0 WHERE (ID = "+intCommID.ToString()+")";
                    sqlComm.ExecuteNonQuery();
                    sqlComm.CommandText = "DELETE FROM ���� WHERE (�ⷿID = "+intKFID.ToString()+") AND (��ƷID = "+intCommID.ToString()+")";
                    sqlComm.ExecuteNonQuery();
                }



                //������
                sqlComm.CommandText = "INSERT INTO �����Ʒ��ɢ���ܱ� (���ݱ��, ����, ��װ����ID, ��ɢ����, ��ע, BeActive, ����ԱID, ҵ��ԱID) VALUES (N'"+strCount+"', '"+strDateSYS+"', "+intCZZ.ToString()+", "+Convert.ToInt32(numericUpDownCPSL.Value).ToString()+", N'"+textBoxBZ.Text+"', 1, "+intUserID.ToString()+", "+comboBoxYWY.SelectedValue.ToString()+")";
                sqlComm.ExecuteNonQuery();


                //��Ʒ��ʷ��¼
                dKUL1 = numericUpDownCPSL.Value;
                dKCCBJ1 = Convert.ToDecimal(labelSPCB.Text);
                dKCJE1 = dKUL * dKCCBJ;

                dKUL = dKC;
                dKUL -= dKUL1;
                dKCJE = dKUL * dKCCBJ1;


                sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ���ⵥ��, ������, �ܽ������, �ܽ����, BeActive) VALUES ('" + strDateSYS + "', " + intCommID.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'�����Ʒ��ɢ', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCJE1.ToString() + ", 1)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ���ⵥ��, ������, �ⷿ�������, �ⷿ�����, BeActive) VALUES (" + intKFID.ToString() + ", '" + strDateSYS + "', " + intCommID.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'�����Ʒ��ɢ', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCJE1.ToString() + ", 1)";
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

            //MessageBox.Show(" �����Ʒ��ɢ������ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = " �����Ʒ��ɢ�Ƶ���" + labelDJBH.Text;
            isSaved = true;

            if (MessageBox.Show("�����Ʒ��ɢ������ɹ����Ƿ�ر��Ƶ�����", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }

        }

        private void FormKCSPCS_FormClosing(object sender, FormClosingEventArgs e)
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
            string strT = "�����Ʒ��ɢ�Ƶ�(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��Ʒ���ƣ�" + textBoxSPMC.Text + "(���:" + textBoxSPBH.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�����Ʒ��ɢ�Ƶ�(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��Ʒ���ƣ�" + textBoxSPMC.Text + "(���:" + textBoxSPBH.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}