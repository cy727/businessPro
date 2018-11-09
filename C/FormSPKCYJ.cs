using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPKCYJ : Form
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
        public int LIMITACCESS = 18;
        
        public FormSPKCYJ()
        {
            InitializeComponent();
        }

        private void FormSPKCYJ_Load(object sender, EventArgs e)
        {
            int i;

            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);


            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��˾����, ����Ŀ��1, ����Ŀ��2, ����Ŀ��3, ����Ŀ��4, ����ԱȨ��, �ܾ���Ȩ��, ְԱȨ��, ����Ȩ��, ҵ��ԱȨ�� FROM ϵͳ������";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {

                try
                {
                    LIMITACCESS = int.Parse(sqldr.GetValue(6).ToString());
                }
                catch
                {
                    LIMITACCESS = 18;
                }
            }
            sqldr.Close();
            sqlConn.Close();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            comboBoxStyle.SelectedIndex = 0;

            //initDataView();
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
            //initDataView();
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
            //initDataView();
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
                //initDataView();
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
                    btnSelect.Focus();
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
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }
                //initDataView();
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
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            switch (comboBoxStyle.SelectedIndex)
            {
                case 0: //��������
                    if (intKFID == 0) //�ܿ��
                        sqlComm.CommandText = "SELECT ��Ʒ����, ��Ʒ���, ��Ʒ���, �������, �����, �������, ����������, ����������, ������� FROM ��Ʒ�� WHERE (beactive = 1) AND (������� > �������) AND (��װ��Ʒ = 0)";
                    else //�ֿ��
                        sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ����.�������, ����.�����,  ����.�������, ����.����������, ����.����������, ����.������� FROM ���� INNER JOIN ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID WHERE (����.�ⷿID = " + intKFID.ToString() + ") AND (��Ʒ��.beactive = 1) AND (����.������� > ����.�������) AND (��Ʒ��.��װ��Ʒ = 0)";
                    break;

                case 1: //�ٽ�����
                    if (intKFID == 0) //�ܿ��
                        sqlComm.CommandText = "SELECT ��Ʒ����, ��Ʒ���, ��Ʒ���, �������, �����, �������, ����������, ����������, ������� FROM ��Ʒ�� WHERE (beactive = 1) AND (������� <= �������) AND (������� > ����������) AND (��װ��Ʒ = 0) ";
                    else //�ֿ��
                        sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ����.�������, ����.�����, ����.�������, ����.����������, ����.����������, ����.������� FROM ���� INNER JOIN ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID WHERE (����.�ⷿID = " + intKFID.ToString() + ") AND (��Ʒ��.beactive = 1) AND (����.������� <= ����.�������) AND (����.������� > ����.����������) AND (��Ʒ��.��װ��Ʒ = 0)";
                    break;

                case 2: //��������
                    if (intKFID == 0) //�ܿ��
                        sqlComm.CommandText = "SELECT ��Ʒ����, ��Ʒ���, ��Ʒ���, �������, �����, �������, ����������, ����������, ������� FROM ��Ʒ�� WHERE (beactive = 1) AND (������� < �������) AND (��װ��Ʒ = 0)";
                    else //�ֿ��
                        sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ����.�������, ����.�����, ����.�������, ����.����������, ����.����������, ����.������� FROM ���� INNER JOIN ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID WHERE (����.�ⷿID = " + intKFID.ToString() + ") AND (��Ʒ��.beactive = 1) AND (����.������� < ����.�������) AND (��Ʒ��.��װ��Ʒ = 0)";
                    break;

                case 3: //�ٽ�����
                    if (intKFID == 0) //�ܿ��
                        sqlComm.CommandText = "SELECT ��Ʒ����, ��Ʒ���, ��Ʒ���, �������, �����, �������, ����������, ����������, ������� FROM ��Ʒ�� WHERE (beactive = 1) AND (������� >= �������) AND (������� < ����������) AND (��װ��Ʒ = 0) ";
                    else //�ֿ��
                        sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ����.�������, ����.�����, ����.�������, ����.����������, ����.����������, ����.������� FROM ���� INNER JOIN ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID WHERE (����.�ⷿID = " + intKFID.ToString() + ") AND (��Ʒ��.beactive = 1) AND (����.������� >= ����.�������) AND (����.������� < ����.����������) AND (��Ʒ��.��װ��Ʒ = 0)";
                    break;

                case 5: //�����
                    if (intKFID == 0) //�ܿ��
                        sqlComm.CommandText = "SELECT ��Ʒ����, ��Ʒ���, ��Ʒ���, �������, �����, �������, ����������, ����������, ������� FROM ��Ʒ�� WHERE (beactive = 1) AND (������� < 0) AND (��װ��Ʒ = 0)";
                    else //�ֿ��
                        sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ����.�������, ����.�����, ����.�������, ����.����������, ����.����������, ����.������� FROM ���� INNER JOIN ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID WHERE (����.�ⷿID = " + intKFID.ToString() + ") AND (��Ʒ��.beactive = 1) AND (����.������� < 0) AND (��Ʒ��.��װ��Ʒ = 0)";
                    break;


                case 4: //������
                    if (intKFID == 0) //�ܿ��
                        sqlComm.CommandText = "SELECT ��Ʒ����, ��Ʒ���, ��Ʒ���, �������, �����, �������, ����������, ����������, ������� FROM ��Ʒ�� WHERE (beactive = 1) AND (������� >= ����������) AND (������� <= ����������)  AND (��װ��Ʒ = 0)";
                    else //�ֿ��
                        sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ����.�������, ����.�����, ����.�������, ����.����������, ����.����������, ����.������� FROM ���� INNER JOIN ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID WHERE (����.�ⷿID = " + intKFID.ToString() + ") AND (��Ʒ��.beactive = 1) AND (����.������� >= ����.����������) AND (����.������� <= ����.����������) AND (��Ʒ��.��װ��Ʒ = 0)";
                    break;

                default:
                    MessageBox.Show("��ѡ����Ʒ���Ԥ��������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    sqlConn.Close();
                    return;


            }

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];

            if (intUserLimit < LIMITACCESS)
                dataGridViewDJMX.Columns[4].Visible = false;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            sqlConn.Close();
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString();
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�����ƷԤ��;���ڣ�" + labelZDRQ.Text + ";"+comboBoxStyle.Text+";����Ա��" + labelCZY.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�����ƷԤ��;���ڣ�" + labelZDRQ.Text + ";" + comboBoxStyle.Text + ";����Ա��" + labelCZY.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }
    }
}