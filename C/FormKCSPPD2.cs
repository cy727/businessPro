using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKCSPPD2 : Form
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

        private int intPDID = 0;
        private int intKFID = 0;

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;
        
        public FormKCSPPD2()
        {
            InitializeComponent();
        }


        private void FormKCSPPD2_Load(object sender, EventArgs e)
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

            sqlComm.CommandText = "SELECT �������� FROM ���ű� WHERE (beactive = 1)";

            if (dSet.Tables.Contains("���ű�")) dSet.Tables.Remove("���ű�");
            sqlDA.Fill(dSet, "���ű�");
            //comboBoxBM.DataSource = dSet.Tables["���ű�"];

            comboBoxBM.Items.Add("ȫ��");
            for (i = 0; i < dSet.Tables["���ű�"].Rows.Count; i++)
            {
                comboBoxBM.Items.Add(dSet.Tables["���ű�"].Rows[i][0].ToString().Trim());
            }

            //��ϸ
            sqlComm.CommandText = "SELECT ����̵㶨���.��¼ѡ��, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ����̵���ϸ��.ʵ������, ����̵���ϸ��.�������, ����̵���ϸ��.�����, ����̵㶨���.��������, ����̵㶨���.������, ����̵���ϸ��.��ע, ����̵���ϸ��.ID, ����̵���ϸ��.��ƷID FROM ����̵���ϸ�� INNER JOIN ��Ʒ�� ON ����̵���ϸ��.��ƷID = ��Ʒ��.ID CROSS JOIN ����̵㶨��� WHERE (����̵���ϸ��.����ID = 0)";

            if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
            sqlDA.Fill(dSet, "���ݱ�");
            dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];

            dataGridViewDJMX.Columns[6].Visible = false;
            dataGridViewDJMX.Columns[8].Visible = false;

            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[1].ReadOnly = true;
            dataGridViewDJMX.Columns[2].ReadOnly = true;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[5].ReadOnly = true;
            dataGridViewDJMX.Columns[6].ReadOnly = true;
            dataGridViewDJMX.Columns[7].ReadOnly = true;
            dataGridViewDJMX.Columns[8].ReadOnly = true;
            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            sqlConn.Close();

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;
        }

        private void initDJ()
        {
            checkBoxAll.CheckedChanged -= checkBoxAll_CheckedChanged;
            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            sqlConn.Open();
            //sqlComm.CommandText = "SELECT ����̵���ܱ�.���ݱ��, ����̵���ܱ�.����, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ����, ����̵���ܱ�.��ע, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ���� FROM ����̵���ܱ� INNER JOIN ְԱ�� ON ����̵���ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ����̵���ܱ�.����ԱID = ����Ա.ID INNER JOIN �ⷿ�� ON ����̵���ܱ�.�ⷿID = �ⷿ��.ID WHERE (����̵���ܱ�.ID = " + iDJID.ToString() + ")";
            sqlComm.CommandText = "SELECT ����̵���ܱ�.���ݱ��, ����̵���ܱ�.����, ְԱ��_1.ְԱ���� AS ҵ��Ա, ְԱ��_1.ְԱ���� AS ����Ա, ����̵���ܱ�.��ע FROM ����̵���ܱ� INNER JOIN ְԱ�� AS ְԱ��_1 ON ����̵���ܱ�.ҵ��ԱID = ְԱ��_1.ID INNER JOIN ְԱ�� AS ְԱ��_2 ON ����̵���ܱ�.����ԱID = ְԱ��_2.ID WHERE (����̵���ܱ�.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy��M��dd��");

                comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                //textBoxPDKF.Text = sqldr.GetValue(5).ToString();
                //textBoxKFMC.Text = sqldr.GetValue(6).ToString();

                this.Text = "����̵��" + labelDJBH.Text;
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT ���ű�.�������� FROM ���ű� INNER JOIN ְԱ�� ON ���ű�.ID = ְԱ��.��λID WHERE (ְԱ��.ְԱ���� = N'" + comboBoxYWY.Text + "')";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                comboBoxBM.Text = sqldr.GetValue(0).ToString();
                break;
            }
            sqldr.Close();


            //��ʼ����Ʒ�б�
            //sqlComm.CommandText = "SELECT ����̵㶨���.��¼ѡ��, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ����̵���ϸ��.ʵ������, ����̵���ϸ��.�������, ����̵���ϸ��.�����, ����̵���ϸ��.��������, ����̵���ϸ��.������, ����̵���ϸ��.��ע, ����̵���ϸ��.ID, ����̵���ϸ��.��ƷID, ��Ʒ��.���ɱ��� FROM ����̵���ϸ�� INNER JOIN ��Ʒ�� ON ����̵���ϸ��.��ƷID = ��Ʒ��.ID CROSS JOIN ����̵㶨��� WHERE (����̵���ϸ��.����ID = " + iDJID.ToString() + ")";
            sqlComm.CommandText = "SELECT ����̵���ϸ��.�̵��־, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ����̵���ϸ��.ʵ������, ����̵���ϸ��.�������, ����̵���ϸ��.�����, ����̵���ϸ��.��������, ����̵���ϸ��.������, ����̵���ϸ��.��ע, ����̵���ϸ��.ID, ����̵���ϸ��.��ƷID, ��Ʒ��.���ɱ���, ����̵���ϸ��.�ⷿID, �ⷿ��.�ⷿ���� FROM ����̵���ϸ�� INNER JOIN ��Ʒ�� ON ����̵���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ����̵���ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN ����̵㶨��� WHERE (����̵���ϸ��.����ID = " + iDJID.ToString() + ")";


            if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
            sqlDA.Fill(dSet, "���ݱ�");
            dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];

            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[0].Visible = false;

            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridViewDJMX.ShowCellErrors = true;
            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;


            sqlConn.Close();

            decimal fSum = 0, fSum1 = 0;
            decimal fCount = 0, fCSum = 0, fCSum1 = 0;
            //this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                //��¼ѡ���־
                if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                }
                if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() != "1")
                    continue;
                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString());
                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString());

                fCSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value.ToString());
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString());
                fCount += 1;

            }
            //this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelSLHJ.Text = fCSum.ToString();
            labelJEHJ.Text = fSum.ToString();
            labelPSSLHJ.Text = fCSum1.ToString();
            labelPSJEHJ.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelPSJEHJ.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();


        }

        private void textBoxPDBH_DoubleClick(object sender, EventArgs e)
        {
            FormSelectHT frmSelectHT = new FormSelectHT();
            frmSelectHT.strConn = strConn;
            frmSelectHT.iSelectStyle = 100;
            frmSelectHT.ShowDialog();
            intPDID = frmSelectHT.iHTNumber;

            getCPDDetail();

        }

        private void getCPDDetail()
        {
            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            if (intPDID == 0)
                return;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ����̵���ܱ�.���ݱ��, ����̵���ܱ�.����, ְԱ��_1.ְԱ���� AS ҵ��Ա, ְԱ��_1.ְԱ���� AS ����Ա, ����̵���ܱ�.��ע FROM ����̵���ܱ� INNER JOIN ְԱ�� AS ְԱ��_1 ON ����̵���ܱ�.ҵ��ԱID = ְԱ��_1.ID INNER JOIN ְԱ�� AS ְԱ��_2 ON ����̵���ܱ�.����ԱID = ְԱ��_2.ID WHERE (����̵���ܱ�.ID = " + intPDID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();

            if (!sqldr.HasRows)
            {
                intPDID = 0;
                sqldr.Close();
                sqlConn.Close();
                return;
            } 
            sqldr.Read();
            labelDJBH.Text = sqldr.GetValue(0).ToString();
            labelZDRQ.Text = sqldr.GetValue(1).ToString();
            comboBoxYWY.Text = sqldr.GetValue(2).ToString();
            textBoxBZ.Text = sqldr.GetValue(4).ToString();
            //textBoxPDKF.Text = sqldr.GetValue(5).ToString();
            //textBoxKFMC.Text = sqldr.GetValue(6).ToString();
            //intKFID = Int32.Parse(sqldr.GetValue(7).ToString());
            sqldr.Close();

            //��ϸ
            //sqlComm.CommandText = "SELECT ����̵㶨���.��¼ѡ��, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ����̵���ϸ��.ʵ������, ����̵���ϸ��.�������, ����̵���ϸ��.�����, ����̵㶨���.��������, ����̵㶨���.������, ����̵���ϸ��.��ע, ����̵���ϸ��.ID, ����̵���ϸ��.��ƷID, ��Ʒ��.���ɱ��� FROM ����̵���ϸ�� INNER JOIN ��Ʒ�� ON ����̵���ϸ��.��ƷID = ��Ʒ��.ID CROSS JOIN ����̵㶨��� WHERE (����̵���ϸ��.����ID = " + intPDID.ToString() + ")";

            sqlComm.CommandText = "SELECT ����̵㶨���.��¼ѡ��, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ����̵���ϸ��.ʵ������, ����̵���ϸ��.�������, ����̵���ϸ��.�����, ����̵㶨���.��������, ����̵㶨���.������, ����̵���ϸ��.��ע, ����̵���ϸ��.ID, ����̵���ϸ��.��ƷID, ��Ʒ��.���ɱ���, ����̵���ϸ��.�ⷿID, �ⷿ��.�ⷿ���� FROM ����̵���ϸ�� INNER JOIN ��Ʒ�� ON ����̵���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ����̵���ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN ����̵㶨��� WHERE (����̵���ϸ��.����ID = " + intPDID.ToString() + ")";


            if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
            sqlDA.Fill(dSet, "���ݱ�");
            dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[12].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            

            sqlConn.Close();
            //countAmount();
            dataGridViewDJMX.RowValidating += dataGridViewDJMX_RowValidating;


            
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.Rows.Count < 0)
                return;

            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;

            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                dataGridViewDJMX.Rows[i].Cells[0].Value = checkBoxAll.Checked;
                dataGridViewDJMX.EndEdit();
            }

            countAmount();
            dataGridViewDJMX.RowValidating += dataGridViewDJMX_RowValidating;
        }

        private bool countAmount()
        {

            decimal fSum = 0, fSum1 = 0;
            decimal fTemp=0, fTemp1;
            decimal fCount = 0, fCSum = 0, fCSum1=0;
            //this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                //��¼ѡ���־
                if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                }
                if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value))
                    continue;


                //ʵ������
                if (dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[4].Value = 0;

                //��������
                dataGridViewDJMX.Rows[i].Cells[7].Value = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString()) - Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value.ToString());
                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString()) == 0) //�������Ϊ0
                {
                    fTemp = 0;
                }
                else
                {
                    fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString()) / Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString());
                }

                //������
                fTemp1 = Convert.ToDecimal(Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value.ToString()).ToString("f2"));
                if(!isSaved)
                    dataGridViewDJMX.Rows[i].Cells[8].Value = fTemp1 * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[12].Value.ToString());


                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString());
                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString());

                fCSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value.ToString());
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString());
                fCount += 1;

            }
            //this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelSLHJ.Text = fCSum.ToString();
            labelJEHJ.Text = fSum.ToString();
            labelPSSLHJ.Text = fCSum1.ToString();
            labelPSJEHJ.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelPSJEHJ.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return true;
        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("���������ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i;
            decimal fTemp,fTemp1;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;

            //�������
            if (isSaved)
            {
                MessageBox.Show("����̵����ݵ�¼���,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intPDID == 0)
            {
                MessageBox.Show("��ѡ�����̵��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            countAmount();

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("û��ѡ���¼", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("������ʵ������,�Ƿ�������棿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;
            saveToolStripButton.Enabled = false;
            string strCount = "", strDateSYS = "";
            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //���ݻ���
                cGetInformation.getSystemDateTime();
                strDateSYS = cGetInformation.strSYSDATATIME;

                sqlComm.CommandText = "UPDATE ����̵���ܱ� SET ���������ϼ� = " + labelPSSLHJ.Text + ", ������ϼ� = " + labelPSJEHJ.Text + ", �̵�ʱ�� = '" + strDateSYS + "',  �̵��� = 1 WHERE (ID = " + intPDID.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //������ϸ
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    //��¼ѡ���־
                    if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value))
                        continue;

                    sqlComm.CommandText = "UPDATE ����̵���ϸ�� SET ʵ������ = " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ", ��ע = N'" + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + "', �̵��־ = 1 , ��������=" + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ", ������=" + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();


                    //�ܿ��
                    fTemp = 0; fTemp1 = 0;
                    sqlComm.CommandText = "SELECT �������, �����, ���ɱ��� FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    //���ɱ���
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value.ToString());
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString());

                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                    }
                    dKUL-=dKUL1;
                    //dKCJE -= dKCJE1;
                    dKCJE = dKCCBJ * dKUL;
                    sqldr.Close();

                    sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dKUL.ToString() + ", ����� = " + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //��ʷ��¼
                    sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, �ܽ������, �ܽ����, ��������, ���𵥼�, ������, BeActive) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", "+comboBoxYWY.SelectedValue.ToString()+", N'"+labelDJBH.Text+"', N'����̵��', "+dKUL.ToString()+", "+dKCJE.ToString()+", "+dKUL1.ToString()+", "+dKCCBJ.ToString()+", "+dKCJE1.ToString()+", 1)";
                    sqlComm.ExecuteNonQuery();



                    //���ķֿ��
                    fTemp=0;
                    //sqlComm.CommandText = "SELECT  �������, �����, ���ɱ���  FROM ���� WHERE (�ⷿID = " + intKFID.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ")";
                    sqlComm.CommandText = "SELECT  �������, �����, ���ɱ���  FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ")";
                    sqldr=sqlComm.ExecuteReader();

                    //���ɱ���
                    dKUL = 0; dKCJE = 0; dKCCBJ = 0;
                    while(sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                    }
                    sqldr.Close();
                    
                    //�����
                    dKUL -= dKUL1;
                    //dKCJE -= dKCJE1;
                    dKCJE = dKCCBJ * dKUL;

                    sqlComm.CommandText = "UPDATE ���� SET ������� = " + dKUL.ToString() + ", ����� = " + dKCJE.ToString() + " WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //�ⷿ��ʷ��¼
                    //sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, �ⷿ�������, �ⷿ�����, ��������, ���𵥼�, ������, BeActive) VALUES (" + intKFID.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + labelDJBH.Text + "', N'����̵��', " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", 1)";
                    sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ҵ��ԱID, ���ݱ��, ժҪ, �ⷿ�������, �ⷿ�����, ��������, ���𵥼�, ������, BeActive) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + labelDJBH.Text + "', N'����̵��', " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", 1)";
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


            //MessageBox.Show("����̵����ݵ�¼���", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //labelDJBH.Text = strCount;
            this.Text = "����̵����ݵ�¼��" + labelDJBH.Text;
            isSaved = true;

            if (MessageBox.Show("����̵����ݵ�¼��ϣ��Ƿ������ʼ��һ�ݵ��ݣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
                mdiT.ʵ�����ݵ�¼CToolStripMenuItem_Click(null, null);
            }

            if (MessageBox.Show("�Ƿ�ر��Ƶ�����", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }


        }

        private void FormKCSPPD2_FormClosing(object sender, FormClosingEventArgs e)
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

            string strT = "����̵��(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";�ⷿ���ƣ�" + textBoxKFMC.Text + ";���������ϼƣ�" + labelPSSLHJ.Text + ";������ϼƣ�" + labelPSJEHJ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "����̵��(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";�ⷿ���ƣ�" + textBoxKFMC.Text + ";���������ϼƣ�" + labelPSSLHJ.Text + ";������ϼƣ�" + labelPSJEHJ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void textBoxPDBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                FormSelectHT frmSelectHT = new FormSelectHT();
                frmSelectHT.strConn = strConn;
                frmSelectHT.iSelectStyle = 110;
                frmSelectHT.strHTSearch = textBoxPDBH.Text.Trim();

                frmSelectHT.ShowDialog();
                intPDID = frmSelectHT.iHTNumber;

                getCPDDetail();
                dataGridViewDJMX.Focus();
            }
        }
    }
}