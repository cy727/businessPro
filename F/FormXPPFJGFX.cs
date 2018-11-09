using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXPPFJGFX : Form
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

        private int intCommID = 0;
        private ClassGetInformation cGetInformation;

        public FormXPPFJGFX()
        {
            InitializeComponent();
        }

        private void FormXPPFJGFX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;


            //�õ���ʼʱ��
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


            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;
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

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0) //ʧ��
                {
                    intCommID = 0;
                    textBoxSPBH.Text = "";
                    textBoxSPMC.Text = "";
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
                    intCommID = 0;
                    textBoxSPBH.Text = "";
                    textBoxSPMC.Text = "";

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

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            //�������
            if (intCommID == 0)
            {
                MessageBox.Show("��ѡ��Ҫ��ѯ����Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��λ��.��λ���, ��λ��.��λ����, ����.����, ����.���, ����.ë��, ����.��������, ����.����ۼ�, ����.����ۼ�, ����.[ë����%], ����.ƽ���ɱ� FROM ��λ�� INNER JOIN (SELECT ������Ʒ�Ƶ���.��λID, SUM(������Ʒ�Ƶ���ϸ��.����) AS ����, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS ���, SUM(������Ʒ�Ƶ���ϸ��.ë��) AS ë��, COUNT(*) AS ��������, MAX(������Ʒ�Ƶ���ϸ��.����) AS ����ۼ�, MIN(������Ʒ�Ƶ���ϸ��.����) AS ����ۼ�, 0.00 AS [ë����%], SUM(������Ʒ�Ƶ���ϸ��.���ɱ��� * ������Ʒ�Ƶ���ϸ��.����) AS ƽ���ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") AND (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) GROUP BY ������Ʒ�Ƶ���.��λID) ���� ON ��λ��.ID = ����.��λID";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            sqlConn.Close();

            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];

            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
            


            toolStripStatusLabelMXJLS.Text = "��¼��:" + dSet.Tables["��Ʒ��"].Rows.Count.ToString() + "  ";

        }

        private void adjustDataView()
        {
            int i;
            decimal iSL = 0, iJYPC = 0;
            decimal dJE = 0,dML=0,dMAX=0,dMIN=0,dMLV,dPJCB=0,dCB=0;

            if (dSet.Tables["��Ʒ��"].Rows.Count > 0)
            {
                if (dSet.Tables["��Ʒ��"].Rows[0][7].ToString() == "")
                    dMIN = 0;
                else
                    dMIN = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[0][7].ToString());
            }


            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��"].Rows[i][2].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][2] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][3].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][3] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][4].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][4] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][5].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][5] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][6].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][6] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][7].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][7] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][8].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][8] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][9].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][9] = 0;

                if (decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][3].ToString()) == 0)
                {
                    dSet.Tables["��Ʒ��"].Rows[i][8] = 0;
                }
                else
                {
                    dSet.Tables["��Ʒ��"].Rows[i][8] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][4].ToString()) / decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][3].ToString()) * 100;
                }

                dCB += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][9].ToString());

                if (decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][2].ToString()) == 0)
                {
                    dSet.Tables["��Ʒ��"].Rows[i][9] = 0;
                }
                else
                {
                    dSet.Tables["��Ʒ��"].Rows[i][9] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][9].ToString()) / decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][2].ToString());
                }

                iSL += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][2].ToString());
                dJE += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][3].ToString());
                dML += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][4].ToString());
                iJYPC += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][5].ToString());
                dMAX = Math.Max(decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][6].ToString()), dMAX);
                dMIN = Math.Min(decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][7].ToString()), dMIN);
                
            }

            DataRow dr = dSet.Tables["��Ʒ��"].NewRow();
            dr[0] = "�ϼ�";
            dr[2] = iSL; dr[3] = dJE; dr[4] = dML; dr[5] = iJYPC; dr[6] = dMAX; dr[7] = dMIN;
            if (decimal.Parse(dr[3].ToString()) == 0)
            {
                dr[8] = 0;
            }
            else
            {
                dr[8] = decimal.Parse(dr[4].ToString()) / decimal.Parse(dr[3].ToString()) * 100;
            }

            if (decimal.Parse(dr[2].ToString()) == 0)
            {
                dr[9] = 0;
            }
            else
            {
                dr[9] = dCB / decimal.Parse(dr[2].ToString());
            }
            dSet.Tables["��Ʒ��"].Rows.Add(dr);

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "����Ʒ�����۸����;��ǰ���ڣ�" + labelZDRQ.Text+";��Ʒ��"+textBoxSPMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "����Ʒ�����۸����;��ǰ���ڣ�" + labelZDRQ.Text + ";��Ʒ��" + textBoxSPMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}