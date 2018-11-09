using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKFSPJCZ : Form
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

        private int intClassID = 0;
        private int iJZID = 0;
        private int iKFID = 0;

        private ClassGetInformation cGetInformation;

        public FormKFSPJCZ()
        {
            InitializeComponent();
        }

        private void FormKFSPJCZ_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            this.Top = 1;
            this.Left = 1;


            //�õ��ϴν�ת
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ����ʱ��,ID FROM ��ת���ܱ� ORDER BY ����ʱ�� DESC";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                iJZID = Convert.ToInt32(sqldr.GetValue(1).ToString());
            }
            sqldr.Close();

            sqlConn.Close();

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;
        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                iKFID = cGetInformation.iKFNumber;
                textBoxKFBH.Text = cGetInformation.strKFCode;
                textBoxKFMC.Text = cGetInformation.strKFName;
            }
        }

        private void textBoxKFMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFMC.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    iKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }

            }
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cGetInformation.getKFInformation(20, textBoxKFBH.Text) == 0) //ʧ��
            {
                return;
            }
            else
            {
                iKFID = cGetInformation.iKFNumber;
                textBoxKFBH.Text = cGetInformation.strKFCode;
                textBoxKFMC.Text = cGetInformation.strKFName;
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
                checkBoxALL.Checked = false;

            }
        }

        private void checkBoxALL_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxALL.Checked)
            {
                intClassID = 0;
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal[] dSUm = new decimal[6];
            if (iKFID == 0)
            {
                MessageBox.Show("��ѡ��Ҫ��ѯ�Ŀⷿ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ��.������, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ����.�������, ����.���ɱ���, ����.�����, ��ת�ⷿ���ܱ�1.��ת����, ��ת�ⷿ���ܱ�1.��ת����, ��ת�ⷿ���ܱ�1.��ת��� FROM ���� INNER JOIN (SELECT * FROM ��ת�ⷿ���ܱ� WHERE (��תID = " + iJZID.ToString() + ")) ��ת�ⷿ���ܱ�1 ON  ����.�ⷿID = ��ת�ⷿ���ܱ�1.�ⷿID AND ����.��ƷID = ��ת�ⷿ���ܱ�1.��ƷID RIGHT OUTER JOIN ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID WHERE (��Ʒ��.beactive = 1) AND (����.�ⷿID = " + iKFID.ToString() + ") AND (��Ʒ��.��װ��Ʒ <> 1)";

            if (intClassID != 0)
                sqlComm.CommandText += " AND (��Ʒ��.������ = "+intClassID.ToString()+")";


            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];

            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
                for (j = 3; j < dSet.Tables["��Ʒ��"].Columns.Count; j++)
                {
                    if (dSet.Tables["��Ʒ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["��Ʒ��"].Rows[i][j] = 0;

                }

            dataGridViewDJMX.Columns[0].Visible=false;
            for (i = 0; i < dataGridViewDJMX.ColumnCount; i++)
            {
                dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            //toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
            sqlConn.Close();

            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";

            for (i = 0; i < 6; i++)
                dSUm[i] = 0;

            for (i = 0; i < dataGridViewDJMX.RowCount; i++)
            {
                for (j = 0; j < dSUm.Length; j++)
                {
                    dSUm[j] += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[4 + j].Value.ToString());
                }
            }

            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString() + " �������:" + dSUm[0].ToString("f0") + " �����:" + dSUm[2].ToString("f2") + " �������:" + dSUm[3].ToString("f0") + " �����:" + dSUm[5].ToString("f2");
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�ⷿ��Ʒ�����;��ǰ���ڣ�" + labelZDRQ.Text + ";�ⷿ��"+textBoxKFMC.Text+";";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�ⷿ��Ʒ�����;��ǰ���ڣ�" + labelZDRQ.Text + ";�ⷿ��" + textBoxKFMC.Text + ";";
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
                    checkBoxALL.Checked = true;

                }
                else
                {
                    intClassID = cGetInformation.iClassNumber;
                    textBoxSPLB.Text = cGetInformation.strClassName;
                    checkBoxALL.Checked = false;
                }
            }
        }
    }
}