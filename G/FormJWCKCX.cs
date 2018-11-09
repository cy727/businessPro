using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJWCKCX : Form
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

        private ClassGetInformation cGetInformation;

        public int iSupplyCompany = 0;
        public int intCommID = 0;

        private decimal[] cTemp = new decimal[3] { 0, 0, 0 };
        private decimal[] cTemp1 = new decimal[3] { 0, 0, 0 };

        
        public FormJWCKCX()
        {
            InitializeComponent();
        }

        private void FormJWCKCX_Load(object sender, EventArgs e)
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
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");

            }
            sqldr.Close();
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1000, "") == 0)
            {
                //return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1200, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                }
            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1200, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                }
            }
        }

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0)
            {
                intCommID = 0;
                textBoxSPMC.Text = "";
                textBoxSPBH.Text = "";
                //return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPMC.Text = cGetInformation.strCommName;
                textBoxSPBH.Text = cGetInformation.strCommCode;
            }
        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0)
                {
                    intCommID = 0;
                    textBoxSPMC.Text = "";
                    textBoxSPBH.Text = "";
                    //return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                }
            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0)
                {
                    intCommID = 0;
                    textBoxSPMC.Text = "";
                    textBoxSPBH.Text = "";
                    //return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ���������ϸ��.����, ���������ϸ��.���ɱ���, ���������ϸ��.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID INNER JOIN ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID WHERE (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.BeActive = 1) AND (���������ϸ��.����>0) AND (���������ܱ�.��ֵ���ID IS NULL)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (intCommID != 0)
                sqlComm.CommandText += "AND  (��Ʒ��.ID = " + intCommID.ToString() + ")";



            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");
            dataGridView1.DataSource = dSet.Tables["��Ʒ��1"];
            dataGridView1.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView1.Columns[10].DefaultCellStyle.Format = "f2";

            sqlComm.CommandText = "SELECT ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ���������ϸ��.����, ���������ϸ��.���ɱ���, ���������ϸ��.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID INNER JOIN ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID WHERE (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.BeActive = 1) AND (���������ϸ��.����<0) AND (���������ܱ�.��ֵ���ID IS NULL)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (intCommID != 0)
                sqlComm.CommandText += "AND  (��Ʒ��.ID = " + intCommID.ToString() + ")";



            if (dSet.Tables.Contains("��Ʒ��2")) dSet.Tables.Remove("��Ʒ��2");
            sqlDA.Fill(dSet, "��Ʒ��2");



            dataGridView2.DataSource = dSet.Tables["��Ʒ��2"];
            dataGridView2.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView2.Columns[10].DefaultCellStyle.Format = "f2";

            sqlComm.CommandText = "SELECT ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ���������ϸ��.����, ���������ϸ��.���ɱ���, ���������ϸ��.������, ���������ܱ�.��ע, ���������ܱ�.��������, ���������ܱ�.���� FROM ���������ϸ�� INNER JOIN ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID INNER JOIN ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID WHERE (���������ܱ�.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (���������ܱ�.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (���������ܱ�.BeActive = 1) AND (���������ܱ�.��ֵ���ID = -1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (intCommID != 0)
                sqlComm.CommandText += "AND  (��Ʒ��.ID = " + intCommID.ToString() + ")";



            if (dSet.Tables.Contains("��Ʒ��3")) dSet.Tables.Remove("��Ʒ��3");
            sqlDA.Fill(dSet, "��Ʒ��3");

            dataGridView3.DataSource = dSet.Tables["��Ʒ��3"];
            dataGridView3.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView3.Columns[10].DefaultCellStyle.Format = "f2";
            sqlConn.Close();

            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);

            //toolStripStatusLabelMXJLS.Text = dSet.Tables["��Ʒ��3"].Rows.Count.ToString();
        }

        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= 3; i++)
            {
                cTemp[i - 1] = 0;
                cTemp1[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 8;
                        c1 = 10;
                        break;
                    case 2:
                        c = 8;
                        c1 = 10;
                        break;
                    case 3:
                        c = 8;
                        c1 = 10;
                        break;

                    default:
                        c = 0;
                        c1 = 0;
                        break;
                }

                if (c != 0)
                {

                    for (j = 0; j < dSet.Tables["��Ʒ��" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp[i - 1] += decimal.Parse(dSet.Tables["��Ʒ��" + i.ToString()].Rows[j][c].ToString());
                            cTemp1[i - 1] += decimal.Parse(dSet.Tables["��Ʒ��" + i.ToString()].Rows[j][c1].ToString());
                        }
                        catch
                        {
                        }
                    }
                }


            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��������ѯ;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��������ѯ;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;

            if (!dSet.Tables.Contains("��Ʒ��" + c1.ToString()))
                return;


            toolStripStatusLabelMXJLS.Text = "����" + dSet.Tables["��Ʒ��" + c1.ToString()].Rows.Count.ToString() + "����¼ �����ϼ�" + cTemp[tabControl1.SelectedIndex].ToString("f0") + " ������ϼ�" + cTemp1[tabControl1.SelectedIndex].ToString("f2") + "Ԫ";
        }


    }
}