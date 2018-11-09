using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXSZPCX : Form
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


        private int iSupplyCompany = 0;
        private ClassGetInformation cGetInformation;

        private decimal[] cTemp = new decimal[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

        public FormXSZPCX()
        {
            InitializeComponent();
        }

        private void FormXSZPCX_Load(object sender, EventArgs e)
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
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString()+"-1");

            }
            sqldr.Close();
            sqlConn.Close();
            

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            tabControl1.SelectedIndex = 2;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(2, "") == 0)
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
                if (cGetInformation.getCompanyInformation(20, textBoxDWBH.Text.Trim()) == 0)
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
                if (cGetInformation.getCompanyInformation(22, textBoxDWMC.Text.Trim()) == 0)
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

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���.��˰�ϼ� FROM ��λ�� INNER JOIN ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID ON ��λ��.ID = ������Ʒ�Ƶ���.��λID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���ϸ��.��Ʒ = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (��λ��.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��1")) dSet.Tables.Remove("��Ʒ��1");
            sqlDA.Fill(dSet, "��Ʒ��1");


            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["��Ʒ��1"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[9].DefaultCellStyle.Format = "f0";

            
            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "������Ʒ��ѯ;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "������Ʒ��ѯ;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
        }

        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= 4; i++)
            {
                cTemp[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 10;
                        break;
                    default:
                        c = 0;
                        break;
                }

                if (c != 0)
                {

                    for (j = 0; j < dSet.Tables["��Ʒ��" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp[i - 1] += decimal.Parse(dSet.Tables["��Ʒ��" + i.ToString()].Rows[j][c].ToString());
                        }
                        catch
                        {
                        }
                    }
                }


            }
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;
            if (!dSet.Tables.Contains("��Ʒ��" + c1.ToString()))
                return;
            toolStripStatusLabelC.Text = "����" + dSet.Tables["��Ʒ��" + c1.ToString()].Rows.Count.ToString() + "����¼ ���ϼ�" + cTemp[tabControl1.SelectedIndex].ToString("f2") + "Ԫ";
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgV = (DataGridView)sender;
            if (dgV.RowCount < 1)
                return;

            if (dgV.SelectedRows.Count < 1)
                return;

            string sTemp = "", sTemp1 = "";
            sTemp = dgV.SelectedRows[0].Cells[1].Value.ToString().ToUpper();
            sTemp1 = dgV.SelectedRows[0].Cells[0].Value.ToString();

            switch (sTemp.Substring(0, 2))
            {
                case "CG":
                    // �������Ӵ����һ����ʵ����
                    FormCGHT childFormCGHT = new FormCGHT();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormCGHT.MdiParent = this.MdiParent;

                    childFormCGHT.strConn = strConn;
                    childFormCGHT.iDJID = Convert.ToInt32(sTemp1);
                    childFormCGHT.isSaved = true;

                    childFormCGHT.intUserID = intUserID;
                    childFormCGHT.intUserLimit = intUserLimit;
                    childFormCGHT.strUserLimit = strUserLimit;
                    childFormCGHT.strUserName = strUserName;
                    childFormCGHT.Show();
                    break;
                case "XS":
                    // �������Ӵ����һ����ʵ����
                    FormXSHT childFormXSHT = new FormXSHT();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSHT.MdiParent = this.MdiParent;

                    childFormXSHT.strConn = strConn;
                    childFormXSHT.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSHT.isSaved = true;

                    childFormXSHT.intUserID = intUserID;
                    childFormXSHT.intUserLimit = intUserLimit;
                    childFormXSHT.strUserLimit = strUserLimit;
                    childFormXSHT.strUserName = strUserName;
                    childFormXSHT.Show();
                    break;
            }

            switch (sTemp.Substring(0, 3))
            {
                case "AKP":
                    // �������Ӵ����һ����ʵ����
                    FormGJSPZD childFormGJSPZD = new FormGJSPZD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormGJSPZD.MdiParent = this.MdiParent;

                    childFormGJSPZD.strConn = strConn;
                    childFormGJSPZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormGJSPZD.isSaved = true;

                    childFormGJSPZD.intUserID = intUserID;
                    childFormGJSPZD.intUserLimit = intUserLimit;
                    childFormGJSPZD.strUserLimit = strUserLimit;
                    childFormGJSPZD.strUserName = strUserName;
                    childFormGJSPZD.Show();
                    break;

                case "ADH":
                    // �������Ӵ����һ����ʵ����
                    FormJHRKYHD childFormJHRKYHD = new FormJHRKYHD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormJHRKYHD.MdiParent = this.MdiParent;

                    childFormJHRKYHD.strConn = strConn;
                    childFormJHRKYHD.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHRKYHD.isSaved = true;

                    childFormJHRKYHD.intUserID = intUserID;
                    childFormJHRKYHD.intUserLimit = intUserLimit;
                    childFormJHRKYHD.strUserLimit = strUserLimit;
                    childFormJHRKYHD.strUserName = strUserName;
                    childFormJHRKYHD.Show();
                    break;

                case "ATH":
                    // �������Ӵ����һ����ʵ����
                    FormJHTCZD childFormJHTCZD = new FormJHTCZD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormJHTCZD.MdiParent = this.MdiParent;

                    childFormJHTCZD.strConn = strConn;
                    childFormJHTCZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHTCZD.isSaved = true;

                    childFormJHTCZD.intUserID = intUserID;
                    childFormJHTCZD.intUserLimit = intUserLimit;
                    childFormJHTCZD.strUserLimit = strUserLimit;
                    childFormJHTCZD.strUserName = strUserName;
                    childFormJHTCZD.Show();
                    break;

                case "ATB":
                    // �������Ӵ����һ����ʵ����
                    FormJHTBJDJ childFormJHTBJDJ = new FormJHTBJDJ();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormJHTBJDJ.MdiParent = this.MdiParent;

                    childFormJHTBJDJ.strConn = strConn;
                    childFormJHTBJDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHTBJDJ.isSaved = true;

                    childFormJHTBJDJ.intUserID = intUserID;
                    childFormJHTBJDJ.intUserLimit = intUserLimit;
                    childFormJHTBJDJ.strUserLimit = strUserLimit;
                    childFormJHTBJDJ.strUserName = strUserName;
                    childFormJHTBJDJ.Show();
                    break;

                case "AYF":
                    // �������Ӵ����һ����ʵ����
                    FormYFZKJS childFormYFZKJS = new FormYFZKJS();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormYFZKJS.MdiParent = this.MdiParent;

                    childFormYFZKJS.strConn = strConn;
                    childFormYFZKJS.iDJID = Convert.ToInt32(sTemp1);
                    childFormYFZKJS.isSaved = true;

                    childFormYFZKJS.intUserID = intUserID;
                    childFormYFZKJS.intUserLimit = intUserLimit;
                    childFormYFZKJS.strUserLimit = strUserLimit;
                    childFormYFZKJS.strUserName = strUserName;
                    childFormYFZKJS.Show();
                    break;

                case "BKP":
                    // �������Ӵ����һ����ʵ����
                    FormXSCKZD childFormXSCKZD = new FormXSCKZD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSCKZD.MdiParent = this.MdiParent;

                    childFormXSCKZD.strConn = strConn;
                    childFormXSCKZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSCKZD.isSaved = true;

                    childFormXSCKZD.intUserID = intUserID;
                    childFormXSCKZD.intUserLimit = intUserLimit;
                    childFormXSCKZD.strUserLimit = strUserLimit;
                    childFormXSCKZD.strUserName = strUserName;
                    childFormXSCKZD.Show();
                    break;

                case "BCK":
                    // �������Ӵ����һ����ʵ����
                    FormXSCKJD childFormXSCKJD = new FormXSCKJD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSCKJD.MdiParent = this.MdiParent;

                    childFormXSCKJD.strConn = strConn;
                    childFormXSCKJD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSCKJD.isSaved = true;

                    childFormXSCKJD.intUserID = intUserID;
                    childFormXSCKJD.intUserLimit = intUserLimit;
                    childFormXSCKJD.strUserLimit = strUserLimit;
                    childFormXSCKJD.strUserName = strUserName;
                    childFormXSCKJD.Show();
                    break;

                case "BTH":
                    // �������Ӵ����һ����ʵ����
                    FormXSTHZD childFormXSTHZD = new FormXSTHZD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSTHZD.MdiParent = this.MdiParent;

                    childFormXSTHZD.strConn = strConn;
                    childFormXSTHZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSTHZD.isSaved = true;

                    childFormXSTHZD.intUserID = intUserID;
                    childFormXSTHZD.intUserLimit = intUserLimit;
                    childFormXSTHZD.strUserLimit = strUserLimit;
                    childFormXSTHZD.strUserName = strUserName;
                    childFormXSTHZD.Show();
                    break;

                case "BTB":
                    // �������Ӵ����һ����ʵ����
                    FormXSTBJDJ childFormXSTBJDJ = new FormXSTBJDJ();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSTBJDJ.MdiParent = this.MdiParent;

                    childFormXSTBJDJ.strConn = strConn;
                    childFormXSTBJDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSTBJDJ.isSaved = true;

                    childFormXSTBJDJ.intUserID = intUserID;
                    childFormXSTBJDJ.intUserLimit = intUserLimit;
                    childFormXSTBJDJ.strUserLimit = strUserLimit;
                    childFormXSTBJDJ.strUserName = strUserName;
                    childFormXSTBJDJ.Show();
                    break;

                case "BYS":
                    // �������Ӵ����һ����ʵ����
                    FormYSZKJS childFormYSZKJS = new FormYSZKJS();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormYSZKJS.MdiParent = this.MdiParent;

                    childFormYSZKJS.strConn = strConn;
                    childFormYSZKJS.iDJID = Convert.ToInt32(sTemp1);
                    childFormYSZKJS.isSaved = true;

                    childFormYSZKJS.intUserID = intUserID;
                    childFormYSZKJS.intUserLimit = intUserLimit;
                    childFormYSZKJS.strUserLimit = strUserLimit;
                    childFormYSZKJS.strUserName = strUserName;
                    childFormYSZKJS.Show();
                    break;

                case "CPD":
                    // �������Ӵ����һ����ʵ����
                    FormKCSPPD2 childFormKCSPPD2 = new FormKCSPPD2();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormKCSPPD2.MdiParent = this.MdiParent;

                    childFormKCSPPD2.strConn = strConn;
                    childFormKCSPPD2.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCSPPD2.isSaved = true;

                    childFormKCSPPD2.intUserID = intUserID;
                    childFormKCSPPD2.intUserLimit = intUserLimit;
                    childFormKCSPPD2.strUserLimit = strUserLimit;
                    childFormKCSPPD2.strUserName = strUserName;
                    childFormKCSPPD2.Show();
                    break;

                case "CBS":
                    // �������Ӵ����һ����ʵ����
                    FormKCSPBSCL childFormKCSPBSCL = new FormKCSPBSCL();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormKCSPBSCL.MdiParent = this.MdiParent;

                    childFormKCSPBSCL.strConn = strConn;
                    childFormKCSPBSCL.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCSPBSCL.isSaved = true;

                    childFormKCSPBSCL.intUserID = intUserID;
                    childFormKCSPBSCL.intUserLimit = intUserLimit;
                    childFormKCSPBSCL.strUserLimit = strUserLimit;
                    childFormKCSPBSCL.strUserName = strUserName;
                    childFormKCSPBSCL.Show();
                    break;

                case "CCK":
                    // �������Ӵ����һ����ʵ����
                    FormKCJWCKDJ childFormKCJWCKDJ = new FormKCJWCKDJ();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormKCJWCKDJ.MdiParent = this.MdiParent;

                    childFormKCJWCKDJ.strConn = strConn;
                    childFormKCJWCKDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCJWCKDJ.isSaved = true;

                    childFormKCJWCKDJ.intUserID = intUserID;
                    childFormKCJWCKDJ.intUserLimit = intUserLimit;
                    childFormKCJWCKDJ.strUserLimit = strUserLimit;
                    childFormKCJWCKDJ.strUserName = strUserName;
                    childFormKCJWCKDJ.Show();
                    break;
            }
 
        }
    }
}