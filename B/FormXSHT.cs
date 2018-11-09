using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Microsoft.Office.Interop.Word;

namespace business
{
    public partial class FormXSHT : Form
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

        public bool isSaved = false;
        public bool isLimit = false;
        public int iDJID = 0;

        private int RowPos;              // Position of currently printing row 
        private bool NewPage;            // Indicates if a new page reached
        private int PageNo;              // Number of pages to print

        private DateTime dtStart = Convert.ToDateTime("1999-1-1");

        private string sGSMC = "";
        private string sGSDZ = "";
        private string sGSDH = "";
        private string sGSCZ = "";
        private string sGSYB = "";
        private string sGSZH = "";
        private string sGSKHYH = "";
        private string sGSSH = "";

        public int LIMITACCESS = 18;

        public int iVersion = 1;
        private string sK = "", sM1 = "", sM2 = "", sM3 = "", sM4 = "";

        public FormXSHT()
        {
            InitializeComponent();
        }

        private void FormXSHT_Load(object sender, EventArgs e)
        {
            this.Left = 1;
            this.Top = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);


            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��˾����, ����Ŀ��1, ����Ŀ��2, ����Ŀ��3, ����Ŀ��4, ����ԱȨ��, �ܾ���Ȩ��, ְԱȨ��, ����Ȩ��, ҵ��ԱȨ�� FROM ϵͳ������";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                sK = sqldr.GetValue(0).ToString();
                sM1 = sqldr.GetValue(1).ToString();
                sM2 = sqldr.GetValue(2).ToString();
                sM3 = sqldr.GetValue(3).ToString();
                sM4 = sqldr.GetValue(4).ToString();

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

            //��ʼ��Ա���б�
            sqlComm.CommandText = "SELECT ID, ְԱ���, ְԱ���� FROM ְԱ�� WHERE (beactive = 1)";

            if (dSet.Tables.Contains("ְԱ��")) dSet.Tables.Remove("ְԱ��");
            sqlDA.Fill(dSet, "ְԱ��");
            comboBoxYWY.DataSource = dSet.Tables["ְԱ��"];
            comboBoxYWY.DisplayMember = "ְԱ����";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.Text = strUserName;
            labelCZY.Text = strUserName;

            //�õ���ʼʱ��
            sqlComm.CommandText = "SELECT ��˾��, ��ַ, �绰, ����, ˰��, ��������, �ʺ�, ��������, ��ʼʱ��, ������ FROM ϵͳ������";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                sGSMC = sqldr.GetValue(0).ToString();
                sGSDZ = sqldr.GetValue(1).ToString();
                sGSDH = sqldr.GetValue(2).ToString();
                sGSCZ = sqldr.GetValue(3).ToString();
                sGSYB = sqldr.GetValue(7).ToString();
                sGSZH = sqldr.GetValue(6).ToString();
                sGSKHYH = sqldr.GetValue(5).ToString();
                sGSSH = sqldr.GetValue(4).ToString();
                dtStart = Convert.ToDateTime(sqldr.GetValue(8).ToString());
                textBoxFZR1.Text = sqldr.GetValue(9).ToString();
            }
            sqldr.Close();

            //��ʼ����Ʒ�б�
            sqlComm.CommandText = "SELECT ��Ʒ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ���ۺ�ͬ��ϸ��.����, ���ۺ�ͬ��ϸ��.����, ���ۺ�ͬ��ϸ��.�ܼ�, ���ۺ�ͬ��ϸ��.��ע, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ������λ, ��Ʒ��.���ɱ��� FROM ���ۺ�ͬ��ϸ�� INNER JOIN ��Ʒ�� ON ���ۺ�ͬ��ϸ��.��ƷID = ��Ʒ��.ID WHERE (���ۺ�ͬ��ϸ��.ID = 0)";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewHT.DataSource = dSet.Tables["��Ʒ��"];
            dataGridViewHT.Columns[0].Visible = false;
            dataGridViewHT.Columns[5].ReadOnly = true;
            dataGridViewHT.Columns[7].ReadOnly = true;
            dataGridViewHT.Columns[8].ReadOnly = true;
            dataGridViewHT.Columns[9].ReadOnly = true;
            dataGridViewHT.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewHT.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewHT.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.ShowCellErrors = true;
            if (intUserLimit < LIMITACCESS)
            {
                dataGridViewHT.Columns[9].Visible = false;
            }


            sqlConn.Close();
            if (isSaved)
            {
                dataGridViewHT.CellValidating -= dataGridViewHT_CellValidating;
                initDJ();
                return;
            }

            //initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");

            initHTDefault();
        }
        private void initDJ()
        {

            dataGridViewHT.CellValidating -= dataGridViewHT_CellValidating;
            dataGridViewHT.RowValidating -= dataGridViewHT_RowValidating;
            dataGridViewHT.CellDoubleClick -= dataGridViewHT_CellDoubleClick;
            comboBoxYWY.SelectedIndexChanged -= comboBoxYWY_SelectedIndexChanged;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��λ��.��λ���, ��λ��.��λ����, ҵ��Ա.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ���ۺ�ͬ��.ǩ���ص�, ���ۺ�ͬ��.ǩ��ʱ��, ���ۺ�ͬ��.����Ҫ��, ���ۺ�ͬ��.��������, ���ۺ�ͬ��.��������, ���ۺ�ͬ��.�����Ʒ, ���ۺ�ͬ��.���䷽ʽ, ���ۺ�ͬ��.������, ���ۺ�ͬ��.���óе�, ���ۺ�ͬ��.�����ص�, ���ۺ�ͬ��.�ֻ�����ʱ��, ���ۺ�ͬ��.�ֻ����ʽ, ���ۺ�ͬ��.�ڻ�����ʱ��, ���ۺ�ͬ��.Ԥ�����, ���ۺ�ͬ��.���Ӧ�����, ���ۺ�ͬ��.�ڻ����ʽ, ���ۺ�ͬ��.ΥԼ����, ���ۺ�ͬ��.�ٲ�ίԱ��, ���ۺ�ͬ��.����Լ������, ���ۺ�ͬ��.��ͬ��Ч��, ���ۺ�ͬ��.��ͬ���, ��λ��.ID, ���ۺ�ͬ��.������λ����, ���ۺ�ͬ��.����˰��, ���ۺ�ͬ��.�����绰, ���ۺ�ͬ��.������������, ���ۺ�ͬ��.���������˺�, ���ۺ�ͬ��.������ϵ��, ���ۺ�ͬ��.������ַ, ���ۺ�ͬ��.��������, ���ۺ�ͬ��.�����ʱ�, ���ۺ�ͬ��.�跽��λ����, ���ۺ�ͬ��.�跽˰��, ���ۺ�ͬ��.�跽�绰, ���ۺ�ͬ��.�跽��������, ���ۺ�ͬ��.�跽�����˺�, ���ۺ�ͬ��.�跽��ϵ��, ���ۺ�ͬ��.�跽��ַ, ���ۺ�ͬ��.�跽����, ���ۺ�ͬ��.�跽�ʱ�, ���ۺ�ͬ��.BeActive FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ҵ��Ա ON ���ۺ�ͬ��.ҵ��ԱID = ҵ��Ա.ID INNER JOIN ְԱ�� ����Ա ON ���ۺ�ͬ��.����ԱID = ����Ա.ID WHERE (���ۺ�ͬ��.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                textBoxDWBH.Text = sqldr.GetValue(0).ToString();
                textBoxDWMC.Text = sqldr.GetValue(1).ToString();
                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxQDDD.Text = sqldr.GetValue(4).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(5).ToString()).ToString("yyyy��M��dd��");
                textBoxZLYQ.Text = sqldr.GetValue(6).ToString();
                textBoxYYQX.Text = sqldr.GetValue(7).ToString();
                textBoxFZTJ.Text = sqldr.GetValue(8).ToString();
                textBoxSJBP.Text = sqldr.GetValue(9).ToString();
                comboBoxYSFS.Text = sqldr.GetValue(10).ToString();
                textBoxYSZ.Text = sqldr.GetValue(11).ToString();
                comboBoxFYCD.Text = sqldr.GetValue(12).ToString();
                textBoxJHDD.Text = sqldr.GetValue(13).ToString();
                textBoxXHJHSJ.Text=sqldr.GetValue(14).ToString();
                textBoxXHFKFS.Text = sqldr.GetValue(15).ToString();
                textBoxQHJHSJ.Text= sqldr.GetValue(16).ToString();
                textBoxYFJE.Text= sqldr.GetValue(17).ToString();
                textBoxTHYFYE.Text = sqldr.GetValue(18).ToString();
                textBoxQHFKFS.Text = sqldr.GetValue(19).ToString();
                textBoxWYZR.Text = sqldr.GetValue(20).ToString();
                textBoxZCWYH.Text = sqldr.GetValue(21).ToString();
                textBoxQTYDSX.Text = sqldr.GetValue(22).ToString();
                textBoxHTYXQ.Text= sqldr.GetValue(23).ToString();
                labelHTH.Text = sqldr.GetValue(24).ToString();

                iSupplyCompany = Convert.ToInt32(sqldr.GetValue(25).ToString());

                textBoxSH.Text = sqldr.GetValue(36).ToString();
                textBoxGSDH.Text = sqldr.GetValue(37).ToString();
                textBoxKHYH.Text = sqldr.GetValue(38).ToString();
                textBoxZH.Text = sqldr.GetValue(39).ToString();
                textBoxFZR.Text = sqldr.GetValue(40).ToString();
                textBoxGSDZ.Text = sqldr.GetValue(41).ToString();
                textBoxGSCZ.Text = sqldr.GetValue(42).ToString();
                textBoxYZBM.Text = sqldr.GetValue(43).ToString();

                sGSMC = sqldr.GetValue(26).ToString();
                sGSSH = sqldr.GetValue(27).ToString();
                sGSDH = sqldr.GetValue(28).ToString();
                sGSKHYH = sqldr.GetValue(29).ToString();
                sGSZH = sqldr.GetValue(30).ToString();
                textBoxFZR1.Text = sqldr.GetValue(31).ToString();
                sGSDZ = sqldr.GetValue(32).ToString();
                sGSCZ = sqldr.GetValue(33).ToString();
                sGSYB = sqldr.GetValue(34).ToString();

                if (!bool.Parse(sqldr.GetValue(44).ToString()))
                {
                    labelHTH.ForeColor = Color.Red;
                }

                this.Text = "���ۺ�ͬ��" + labelHTH.Text;
            }
            sqldr.Close();

            //��ʼ����Ʒ�б�
            sqlComm.CommandText = "SELECT ���ۺ�ͬ��ϸ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ���ۺ�ͬ��ϸ��.����, ���ۺ�ͬ��ϸ��.����, ���ۺ�ͬ��ϸ��.�ܼ�, ���ۺ�ͬ��ϸ��.��ע, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, ��Ʒ��.���ɱ��� FROM ���ۺ�ͬ��ϸ�� INNER JOIN ��Ʒ�� ON ���ۺ�ͬ��ϸ��.��ƷID = ��Ʒ��.ID WHERE (���ۺ�ͬ��ϸ��.���ۺ�ͬID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewHT.DataSource = dSet.Tables["��Ʒ��"];
            dataGridViewHT.Columns[0].Visible = false;
            dataGridViewHT.ReadOnly = true;
            dataGridViewHT.AllowUserToAddRows = false;
            dataGridViewHT.AllowUserToDeleteRows = false;
            dataGridViewHT.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[4].DefaultCellStyle.Format = "f2";

            dataGridViewHT.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewHT.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            if (intUserLimit < LIMITACCESS)
            {
                dataGridViewHT.Columns[9].Visible = false;
            }

            sqlConn.Close();
            //getCompanyInfoDetail();



            countAmount();
            dataGridViewHT.CellPainting += dataGridViewHT_CellPainting;

        }



        private void initHTDefault()
        {
            textBoxZLYQ.Text = "ԭ����׼";
            textBoxYYQX.Text = "�յ�����Ҽ����";
            textBoxFZTJ.Text = "Ҽ��";
            textBoxSJBP.Text = "����ṩ";
            textBoxJHDD.Text = "����";
            textBoxZCWYH.Text = "����";
            textBoxQDDD.Text = "����";
            textBoxXHJHSJ.Text = "�ֿ��ֻ�";
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(2, "") == 0)
            {
                return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
            }
            getCompanyInfoDetail();
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
                getCompanyInfoDetail();
                comboBoxFYCD.Focus();
            }
        }

        private void getCompanyInfoDetail()
        {
            if (iSupplyCompany == 0)
            {
                textBoxDWBH.Text = "";
                textBoxDWMC.Text = "";
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��λ���, ��λ����, ˰��, �绰, ��������, �����˺�, ��ϵ��, ��ַ, ����, �ʱ�, ��ϵ��ַ, ҵ��Ա FROM ��λ�� WHERE (ID = " + iSupplyCompany.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                textBoxDWBH.Text = sqldr.GetValue(0).ToString();
                textBoxDWMC.Text = sqldr.GetValue(1).ToString();
                textBoxFZR.Text = sqldr.GetValue(6).ToString();
                textBoxYZBM.Text = sqldr.GetValue(9).ToString();
                textBoxGSDZ.Text = sqldr.GetValue(10).ToString();
                textBoxGSDH.Text = sqldr.GetValue(3).ToString();
                textBoxGSCZ.Text = sqldr.GetValue(8).ToString();
                textBoxSH.Text = sqldr.GetValue(2).ToString();
                textBoxKHYH.Text = sqldr.GetValue(4).ToString();
                textBoxZH.Text = sqldr.GetValue(5).ToString();
                comboBoxYWY.Text = sqldr.GetValue(11).ToString().Trim();
            }
            sqlConn.Close();
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
                getCompanyInfoDetail();
                comboBoxFYCD.Focus();
            }
        }

        private void dataGridViewHT_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2) //��Ʒ���
            {
                if (cGetInformation.getCommInformation(1, "") == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    this.dataGridViewHT.CellValidating -= dataGridViewHT_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewHT);
                    dataGridViewHT.Rows[e.RowIndex].Cells[0].Value = cGetInformation.iCommNumber;
                    dataGridViewHT.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewHT.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewHT.Rows[e.RowIndex].Cells[7].Value = cGetInformation.strCommGG;
                    dataGridViewHT.Rows[e.RowIndex].Cells[8].Value = cGetInformation.strCommCount;
                    dataGridViewHT.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommKCCBJ;
                    //dataGridViewHT.Rows[e.RowIndex].Cells[4].Value = Math.Round(Decimal.Zero, 2);
                    dataGridViewHT.Rows[e.RowIndex].Cells[4].Value = cGetInformation.decCommPFJ;
                    dataGridViewHT.EndEdit();
                    dataGridViewHT.CurrentCell = dataGridViewHT.Rows[e.RowIndex].Cells[3];
                    dataGridViewHT.BeginEdit(false);
                    this.dataGridViewHT.CellValidating += dataGridViewHT_CellValidating;

                }
            }
        }

        private void dataGridViewHT_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int iRe = 0;
            if (isSaved)
                return;
            if (dataGridViewHT.Rows[e.RowIndex].IsNewRow)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewHT);
            switch (e.ColumnIndex)
            {
                case 2: //��Ʒ���
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewHT.CellValidating -= dataGridViewHT_CellValidating;

                        dataGridViewHT.Rows[e.RowIndex].Cells[0].Value = 0;
                        dataGridViewHT.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewHT.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewHT.Rows[e.RowIndex].Cells[7].Value = "";
                        dataGridViewHT.Rows[e.RowIndex].Cells[8].Value = "";
                        dataGridViewHT.Rows[e.RowIndex].Cells[4].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewHT.Rows[e.RowIndex].Cells[9].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewHT.CellValidating += dataGridViewHT_CellValidating;
                        break;

                    }

                    iRe = cGetInformation.getCommInformation(20, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewHT.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //ʧ��
                    //if (cGetInformation.getCommInformation(20, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewHT.CancelEdit();
                        dataGridViewHT.Rows[e.RowIndex].Cells[2].ErrorText = "��Ʒ����������";
                    }
                    else
                    {

                        this.dataGridViewHT.CellValidating -= dataGridViewHT_CellValidating;

                        if (dataGridViewHT.Rows[e.RowIndex].Cells[0].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewHT.CellValidating += dataGridViewHT_CellValidating;
                            break;
                        } 

                        dataGridViewHT.Rows[e.RowIndex].Cells[0].Value = cGetInformation.iCommNumber;
                        dataGridViewHT.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewHT.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewHT.Rows[e.RowIndex].Cells[7].Value = cGetInformation.strCommGG;
                        dataGridViewHT.Rows[e.RowIndex].Cells[8].Value = cGetInformation.strCommCount;
                        //dataGridViewHT.Rows[e.RowIndex].Cells[4].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewHT.Rows[e.RowIndex].Cells[4].Value = cGetInformation.decCommPFJ;
                        dataGridViewHT.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommKCCBJ;
                        this.dataGridViewHT.CellValidating += dataGridViewHT_CellValidating;

                    }
                    break;

                case 1: //��Ʒ����
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewHT.CellValidating -= dataGridViewHT_CellValidating;

                        dataGridViewHT.Rows[e.RowIndex].Cells[0].Value = 0;
                        dataGridViewHT.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewHT.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewHT.Rows[e.RowIndex].Cells[7].Value = "";
                        dataGridViewHT.Rows[e.RowIndex].Cells[8].Value = "";
                        dataGridViewHT.Rows[e.RowIndex].Cells[4].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewHT.Rows[e.RowIndex].Cells[9].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewHT.CellValidating += dataGridViewHT_CellValidating;
                        break;

                    }

                    iRe = cGetInformation.getCommInformation(10, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewHT.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //ʧ��
                    //if (cGetInformation.getCommInformation(10, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        dataGridViewHT.Rows[e.RowIndex].Cells[1].ErrorText = "��Ʒ�������������";
                    }
                    else
                    {
                        this.dataGridViewHT.CellValidating -= dataGridViewHT_CellValidating;

                        if (dataGridViewHT.Rows[e.RowIndex].Cells[0].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewHT.CellValidating += dataGridViewHT_CellValidating;
                            break;
                        } 
                        dataGridViewHT.Rows[e.RowIndex].Cells[0].Value = cGetInformation.iCommNumber;
                        dataGridViewHT.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewHT.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewHT.Rows[e.RowIndex].Cells[7].Value = cGetInformation.strCommGG;
                        dataGridViewHT.Rows[e.RowIndex].Cells[8].Value = cGetInformation.strCommCount;
                        //dataGridViewHT.Rows[e.RowIndex].Cells[4].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewHT.Rows[e.RowIndex].Cells[4].Value = cGetInformation.decCommPFJ;

                        dataGridViewHT.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommKCCBJ;
                        this.dataGridViewHT.CellValidating += dataGridViewHT_CellValidating;
                    }
                    break;

                case 3:  //��Ʒ����
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut <= 0)
                        {
                            dataGridViewHT.Rows[e.RowIndex].Cells[3].ErrorText = "��Ʒ�����������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewHT.Rows[e.RowIndex].Cells[3].ErrorText = "��Ʒ�����������ʹ���";
                        e.Cancel = true;
                    }
                    break;
                case 4: //��Ʒ�۸�
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;
                    if (dataGridViewHT.Rows[e.RowIndex].Cells[9].Value.ToString() == "")
                    {
                        MessageBox.Show("���������ͬ��Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.dataGridViewHT.CellValidating -= dataGridViewHT_CellValidating;
                        dataGridViewHT.Rows[e.RowIndex].Cells[4].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewHT.CellValidating += dataGridViewHT_CellValidating;
                        break;
                    }

                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        if (detOut >= 0)
                        {
                            detOut = Math.Round(detOut, 2);

                            if (detOut.CompareTo(dataGridViewHT.Rows[e.RowIndex].Cells[9].Value) <= 0)
                            {
                                if (MessageBox.Show("��Ʒ�۸���ڿ��ɱ��ۣ��Ƿ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                    e.Cancel = true;
                                else
                                {
                                    this.dataGridViewHT.CellValidating -= dataGridViewHT_CellValidating;
                                    dataGridViewHT.Rows[e.RowIndex].Cells[4].Value = detOut;
                                    this.dataGridViewHT.CellValidating += dataGridViewHT_CellValidating;
                                }

                            }
                        }
                        else
                        {
                            dataGridViewHT.Rows[e.RowIndex].Cells[4].ErrorText = "��Ʒ�۸��������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewHT.Rows[e.RowIndex].Cells[4].ErrorText = "��Ʒ�����۸����ʹ���";
                        e.Cancel = true;
                    }
                    break;
                case 6:
                    if (e.FormattedValue.ToString().Length > 100)
                    {
                        dataGridViewHT.Rows[e.RowIndex].Cells[6].ErrorText = "��ע����";
                        e.Cancel = true;
                    }
                    break;
                default:
                    break;



            }
            dataGridViewHT.EndEdit();

        }

        private bool countAmount()
        {
            decimal fSum=0;
            decimal fTemp, fTemp1;
            decimal fCount = 0;
            bool bCheck = true;

            isLimit = true;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewHT);
            this.dataGridViewHT.CellValidating -= dataGridViewHT_CellValidating;

        
            for (int i = 0; i < dataGridViewHT.Rows.Count; i++)
            {
                if (dataGridViewHT.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewHT.Rows[i].Cells[0].Value.ToString() == "" || dataGridViewHT.Rows[i].Cells[0].Value.ToString() == "0")
                {
                    dataGridViewHT.Rows[i].Cells[1].ErrorText = "����������Ʒ";
                    dataGridViewHT.Rows[i].Cells[2].ErrorText = "����������Ʒ";
                    bCheck = false;
                }

                if (dataGridViewHT.Rows[i].Cells[3].Value.ToString() == "")
                {
                    dataGridViewHT.Rows[i].Cells[3].ErrorText = "����������Ʒ����";
                    bCheck = false;
                }

                if (dataGridViewHT.Rows[i].Cells[4].Value.ToString() == "")
                {
                    dataGridViewHT.Rows[i].Cells[4].ErrorText = "����������Ʒ�۸�";
                    bCheck = false;
                }

                if (!bCheck)
                    continue;

                if (dataGridViewHT.Rows[i].Cells[3].Value.ToString() == "")
                    fTemp = 0;
                else
                    fTemp = Convert.ToDecimal(dataGridViewHT.Rows[i].Cells[3].Value);

                if (dataGridViewHT.Rows[i].Cells[4].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewHT.Rows[i].Cells[4].Value);

                if (Convert.ToDecimal(dataGridViewHT.Rows[i].Cells[4].Value) <= Convert.ToDecimal(dataGridViewHT.Rows[i].Cells[9].Value))
                {
                    dataGridViewHT.Rows[i].Cells[4].Style.BackColor = Color.LightPink;
                    isLimit = false;
                }
                else
                    dataGridViewHT.Rows[i].Cells[4].Style.BackColor = Color.White;

                dataGridViewHT.Rows[i].Cells[5].Value = Math.Round(fTemp * fTemp1, 2);

                fCount += 1;
                fSum += Convert.ToDecimal(dataGridViewHT.Rows[i].Cells[5].Value);
            }
            
            dataGridViewHT.EndEdit();

            labelJEHJ.Text = fSum.ToString("f2");
            labelHTMXJL.Text = fCount.ToString();
            labelDX.Text = cGetInformation.changeDAXIE(labelJEHJ.Text);
            this.dataGridViewHT.CellValidating += dataGridViewHT_CellValidating;

            return bCheck;

        }

        private void dataGridViewHT_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("���������ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridViewHT_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        protected override bool ProcessCmdKey(ref  Message msg, Keys keyData)
        {
            //return EnterToTab(ref   msg, keyData, true);

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
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[3];
                                        break;
                                    case 2:
                                    case 3:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[dv.CurrentCell.ColumnIndex + 1];
                                        break;
                                    case 4:
                                        //dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[6];
                                        //break;
                                    //case 6:
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

                        if (keyData == Keys.F6)
                        {
                            //dv.Rows.Insert(dv.CurrentCell.RowIndex+1, 1);
                            DataRow drTemp = dSet.Tables["��Ʒ��"].NewRow();
                            dSet.Tables["��Ʒ��"].Rows.InsertAt(drTemp, dv.CurrentCell.RowIndex + 1);
                            return true;
                        }

                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i;


            //�������
            if (isSaved)
            {
                MessageBox.Show("��ͬ�Ѿ�����,��ͬ��Ϊ��" + labelHTH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("��ѡ���ͬ��λ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (labelHTMXJL.Text == "0")
            {
                MessageBox.Show("û�к�ͬ��Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("��ͬ��ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("�����ͬ����,�Ƿ�������棿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            string strLimitPass = "";
            sqlConn.Open();
            sqlComm.CommandText = "SELECT Ȩ���� FROM Ȩ�����";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                strLimitPass = sqldr.GetValue(0).ToString();
                break;
            }
            sqldr.Close();
            sqlConn.Close();

            if (strLimitPass.Trim() != "")
            {
                if (!isLimit && intUserLimit < LIMITACCESS) //Ȩ�޹���
                {
                    FormLACCESS frmLACCESS = new FormLACCESS();
                    frmLACCESS.strPass = strLimitPass.Trim();
                    frmLACCESS.ShowDialog();
                    if (!frmLACCESS.isAccept)
                        return;
                }
            }

            saveToolStripButton.Enabled = false;
            string strCount = "", strDateSYS = "", strKey = "XS";
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
                    if (iVersion <= 0)
                    {
                        if (int.Parse(strCount) > 2)
                        {
                            MessageBox.Show("Ԥ�����û�ÿ��ֻ����������", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            sqlConn.Close();
                            return;
                        }
                    }

                    strCount = string.Format("{0:D3}", Int32.Parse(strCount));
                    strCount = strKey.ToUpper() + Convert.ToDateTime(strDateSYS).ToString("yyyyMMdd") + strCount;
                }
                else
                {
                    MessageBox.Show("���ݴ���", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sqlConn.Close();
                    return;
                }

                string ss1 = "NULL", ss2 = "NULL", ss3 = "NULL";

                if (textBoxXHJHSJ.Text.Trim() != "")
                    ss1 = "N'" + textBoxXHJHSJ.Text.Trim() + "'";

                if (textBoxQHJHSJ.Text.Trim() != "")
                    ss2 = "N'" + textBoxQHJHSJ.Text.Trim() + "'";

                if (textBoxHTYXQ.Text.Trim() != "")
                    ss3 = "N'" + textBoxHTYXQ.Text.Trim() + "'";

                sqlComm.CommandText = "INSERT INTO ���ۺ�ͬ�� (��ͬ���, ������λID, ҵ��ԱID, ����ԱID, ǩ���ص�, ǩ��ʱ��, ����Ҫ��, ��������, ��������, �����Ʒ, ���䷽ʽ, ������, ���óе�, �����ص�, �ֻ�����ʱ��, �ֻ����ʽ, �ڻ�����ʱ��, Ԥ�����, ���Ӧ�����, �ڻ����ʽ, ΥԼ����, �ٲ�ίԱ��, ����Լ������, ��ͬ��Ч��, BeActive, ���, ������λ����, ����˰��, �����绰, ������������, ���������˺�, ������ϵ��, ������ַ, ��������, �����ʱ�, �跽��λ����, �跽˰��, �跽�绰, �跽��������, �跽�����˺�, �跽��ϵ��, �跽��ַ, �跽����, �跽�ʱ�) VALUES (N'" + strCount + "', " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", " + intUserID.ToString() + ", N'" + textBoxQDDD.Text + "', '" + cGetInformation.strSYSDATATIME + "', N'" + textBoxZLYQ.Text + "', N'" + textBoxYYQX.Text + "', N'" + textBoxFZTJ.Text + "', N'" + textBoxSJBP.Text + "', N'" + comboBoxYSFS.Text + "', N'" + textBoxYSZ.Text + "', N'" + comboBoxFYCD.Text + "', N'" + textBoxJHDD.Text + "', " + ss1 + ", N'" + textBoxXHFKFS.Text + "', " + ss2 + ", N'" + textBoxYFJE.Text.Trim() + "', N'" + textBoxTHYFYE.Text.Trim() + "', N'" + textBoxQHFKFS.Text + "', N'" + textBoxWYZR.Text + "', N'" + textBoxZCWYH.Text + "', N'" + textBoxQTYDSX.Text + "', " + ss3 + ", 1, " + labelJEHJ.Text + ", N'" + sGSMC + "', N'" + sGSSH + "', N'" + sGSDH + "', N'" + sGSKHYH + "', '" + sGSZH + "', N'" + textBoxFZR1.Text + "', N'" + sGSDZ + "', N'" + sGSCZ + "', '" + sGSYB + "', N'" + textBoxDWMC.Text + "', N'" + textBoxSH.Text + "', N'" + textBoxGSDH.Text + "', N'" + textBoxKHYH.Text + "', '" + textBoxZH.Text + "', N'" + textBoxFZR.Text + "', N'" + textBoxGSDZ.Text + "', N'" + textBoxGSCZ.Text + "', '" + textBoxYZBM.Text + "')";
                sqlComm.ExecuteNonQuery();

                //ȡ�ú�ͬ�� 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //��ͬ��ϸ
                for (i = 0; i < dataGridViewHT.Rows.Count; i++)
                {
                    if (dataGridViewHT.Rows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "INSERT INTO ���ۺ�ͬ��ϸ�� (���ۺ�ͬID, ��ƷID, ����, ����, �ܼ�, ��ע) VALUES (" + sBillNo + ", " + dataGridViewHT.Rows[i].Cells[0].Value.ToString() + ", " + dataGridViewHT.Rows[i].Cells[3].Value.ToString() + ", " + dataGridViewHT.Rows[i].Cells[4].Value.ToString() + ", " + dataGridViewHT.Rows[i].Cells[5].Value.ToString() + ", N'" + dataGridViewHT.Rows[i].Cells[6].Value.ToString() + "')";
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

            labelHTH.Text = strCount;
            isSaved = true;

            if (MessageBox.Show("��ͬ����ɹ����Ƿ��ӡ��", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                printPreviewToolStripButton_Click(null, null);
            }

            bool bClose = false;
            if (MessageBox.Show("�Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                bClose = true;
            }


            if (MessageBox.Show("�Ƿ������ʼ��һ�ݺ�ͬ��", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
                mdiT.���ۺ�ͬ�Ƶ�AToolStripMenuItem_Click(null, null);
            }

            if (bClose)
                this.Close();

        }

        private void FormXSHT_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
                return;

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
            try
            {

                if (ppd.ShowDialog() != DialogResult.OK)
                    return;

                // Showing the Print Preview Page
                printDoc.BeginPrint += PrintDoc_BeginPrint;
                printDoc.PrintPage += PrintDoc_PrintPage;

                ppw.Width = 1000;
                ppw.Height = 800;
                if (ppw.ShowDialog() != DialogResult.OK)
                {
                    printDoc.BeginPrint -= PrintDoc_BeginPrint;
                    printDoc.PrintPage -= PrintDoc_PrintPage;
                    return;
                }

                // Printing the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= PrintDoc_BeginPrint;
                printDoc.PrintPage -= PrintDoc_PrintPage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��ӡʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {

                if (ppd.ShowDialog() != DialogResult.OK)
                    return;

                // Showing the Print Preview Page
                printDoc.BeginPrint += PrintDoc_BeginPrint;
                printDoc.PrintPage += PrintDoc_PrintPage;

                // Printing the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= PrintDoc_BeginPrint;
                printDoc.PrintPage -= PrintDoc_PrintPage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��ӡʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void PrintDoc_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                PageNo = 1;
                NewPage = true;
                RowPos = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��ӡʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int iyRow;
            int i;
            iyRow = 0;

            // Formatting the Content of Text Cell to print
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            StrFormat.LineAlignment = StringAlignment.Center;
            StrFormat.Trimming = StringTrimming.EllipsisCharacter;

            StringFormat StrFormatL = new StringFormat();
            StrFormatL.Alignment = StringAlignment.Near;
            StrFormatL.LineAlignment = StringAlignment.Center;
            StrFormatL.Trimming = StringTrimming.EllipsisCharacter;

            StringFormat StrFormatR = new StringFormat();
            StrFormatR.Alignment = StringAlignment.Far;
            StrFormatR.LineAlignment = StringAlignment.Center;
            StrFormatR.Trimming = StringTrimming.EllipsisCharacter;

            StringFormat StrFormatL1 = new StringFormat();
            StrFormatL1.Alignment = StringAlignment.Near;
            StrFormatL1.LineAlignment = StringAlignment.Near;
            StrFormatL1.Trimming = StringTrimming.EllipsisCharacter;


            System.Drawing.Font _Font22 = new System.Drawing.Font("����", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12 = new System.Drawing.Font("����", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9 = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12U = new System.Drawing.Font("����", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            int iTopM = 20;
            int iLeftM = 60;
            int iWidth = 700;
            int iWidth_R = 260;
            int iWidth_L = 440;

            int iHeight12 = 22;
            int iHeight22 = 50;
            int iHeight9 = 17;

            float fTemp = 0;

            int iRowsPerPage = 39;
            int iPages = 0;

            Brush b = new SolidBrush(Color.Black);

            int iTemp = 0;

            foreach (ToolStripMenuItem tsMI in toolStripDropDownButtonBJ.DropDownItems)
            {
                if (tsMI.Checked)
                {
                    iTopM = int.Parse(tsMI.Text);
                    break;
                }
            }

            try
            {
                if (PageNo == 1) //��ҳ
                {
                    //e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM, iTopM, iWidth, iHeight22));
                    e.Graphics.DrawString("���ۺ�ͬ", _Font22, b, new System.Drawing.RectangleF(iLeftM, iTopM, iWidth, iHeight22), StrFormat);
                    iyRow += iTopM + iHeight22;

                    e.Graphics.DrawString("�跽��", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth_L, iHeight12), StrFormatL);
                    e.Graphics.DrawString(textBoxDWMC.Text, _Font12U, b, new System.Drawing.RectangleF(iLeftM + 45, iyRow, iWidth_L, iHeight12), StrFormatL);
                    e.Graphics.DrawString("��ͬ��ţ�" + labelHTH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth_L, iyRow, iWidth_R, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawString("������"+sGSMC, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth_L, iHeight12), StrFormatL);
                    e.Graphics.DrawString("ǩ���ص㣺" + textBoxQDDD.Text+"�����Ƶ���"+labelCZY.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth_L, iyRow, iWidth_R, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawString("ǩ��ʱ�䣺" + labelZDRQ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth_L, iyRow, iWidth_R, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawString("һ����Ʒ���ơ��ͺš����ҡ����������ۡ����", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                    e.Graphics.DrawString("���", _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 260, iHeight12));
                    e.Graphics.DrawString("��Ʒ����", _Font12, b, new System.Drawing.RectangleF(iLeftM + 60, iyRow, 260, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 320, iyRow, 80, iHeight12));
                    e.Graphics.DrawString("����", _Font12, b, new System.Drawing.RectangleF(iLeftM + 320, iyRow, 80, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 400, iyRow, 100, iHeight12));
                    e.Graphics.DrawString("����", _Font12, b, new System.Drawing.RectangleF(iLeftM + 400, iyRow, 100, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 100, iHeight12));
                    e.Graphics.DrawString("�ܼ�", _Font12, b, new System.Drawing.RectangleF(iLeftM + 500, iyRow, 100, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 600, iyRow, 100, iHeight12));
                    e.Graphics.DrawString("��ע", _Font12, b, new System.Drawing.RectangleF(iLeftM + 600, iyRow, 100, iHeight12), StrFormat);
                    iyRow += iHeight12;

                    if (Convert.ToInt32(labelHTMXJL.Text) <= 6) //��ҳ
                    {
                        for (i = 0; i < dataGridViewHT.Rows.Count; i++)
                        {
                            if (dataGridViewHT.Rows[i].IsNewRow)
                                continue;
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                            e.Graphics.DrawString(Convert.ToString(i + 1), _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 260, iHeight12));
                            e.Graphics.DrawString(dataGridViewHT.Rows[i].Cells[1].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iLeftM + 60, iyRow, 260, iHeight12), StrFormat);
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 320, iyRow, 80, iHeight12));
                            e.Graphics.DrawString(dataGridViewHT.Rows[i].Cells[3].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iLeftM + 320, iyRow, 80, iHeight12), StrFormat);
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 400, iyRow, 100, iHeight12));
                            e.Graphics.DrawString(Convert.ToDecimal(dataGridViewHT.Rows[i].Cells[4].Value.ToString()).ToString("f2"), _Font12, b, new System.Drawing.RectangleF(iLeftM + 400, iyRow, 100, iHeight12), StrFormat);
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 100, iHeight12));
                            e.Graphics.DrawString(Convert.ToDecimal(dataGridViewHT.Rows[i].Cells[5].Value.ToString()).ToString("f2"), _Font12, b, new System.Drawing.RectangleF(iLeftM + 500, iyRow, 100, iHeight12), StrFormat);
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 600, iyRow, 100, iHeight12));
                            e.Graphics.DrawString(dataGridViewHT.Rows[i].Cells[6].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iLeftM + 600, iyRow, 100, iHeight12), StrFormat);
                            iyRow += iHeight12;


                        }
                    }
                    else //��ҳ
                    {
                        //iPages = (int)Math.Ceiling((decimal)(dataGridViewHT.Rows.Count)/(decimal)(iRowsPerPage));
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                        e.Graphics.DrawString("1", _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 260, iHeight12));
                        e.Graphics.DrawString("�����ͬ�嵥", _Font12, b, new System.Drawing.RectangleF(iLeftM + 60, iyRow, 200, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 320, iyRow, 80, iHeight12));
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 400, iyRow, 100, iHeight12));
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 100, iHeight12));
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 600, iyRow, 100, iHeight12));

                        iyRow += iHeight12;


                    }
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                    e.Graphics.DrawString("�ϼ�", _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 440, iHeight12));
                    e.Graphics.DrawString(labelDX.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + 80, iyRow, 360, iHeight12), StrFormatL);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 200, iHeight12));
                    e.Graphics.DrawString("��" + labelJEHJ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + 520, iyRow, 240, iHeight12), StrFormatL);

                    iyRow += iHeight12 * 2;
                    //iyRow = 450;

                    e.Graphics.DrawString("��������Ҫ�󡢼�����׼��", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("��������Ҫ�󡢼�����׼��", _Font12).Width;
                    e.Graphics.DrawString(textBoxZLYQ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("�����跽�Բ�Ʒ�ͺš������������������ޣ�", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("�����跽�Բ�Ʒ�ͺš������������������ޣ�", _Font12).Width;
                    e.Graphics.DrawString(textBoxYYQX.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("�ġ�������������������������������ޣ�", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("�ġ�������������������������������ޣ�", _Font12).Width;
                    e.Graphics.DrawString(textBoxFZTJ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("�塢�����Ʒ�����������������Ӧ������", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("�塢�����Ʒ�����������������Ӧ������", _Font12).Width;
                    e.Graphics.DrawString(textBoxSJBP.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("�������䷽ʽ��" + comboBoxYSFS.Text + "��������������", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("�������䷽ʽ����" + comboBoxYSFS.Text + "������������������", _Font12).Width;
                    e.Graphics.DrawString(textBoxYSZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("�ߡ����óе���", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("�ߡ����óе���", _Font12).Width;
                    e.Graphics.DrawString(comboBoxFYCD.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth / 2, iyRow + iHeight12 - 6));


                    e.Graphics.DrawString("�����ص㣺", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("�����ص㣺", _Font12).Width;
                    e.Graphics.DrawString(textBoxJHDD.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow, iWidth / 2 - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));

                    iyRow += iHeight12;

                    e.Graphics.DrawString("�ˡ��ֻ�������ʱ�䣺", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("�ˡ��ֻ�������ʱ�䣺", _Font12).Width;

                    string ss1 = textBoxXHJHSJ.Text.Trim();

                    try
                    {
                        ss1 = DateTime.Parse(textBoxXHJHSJ.Text.Trim()).ToString("yyyy��M��dd��");
                    }
                    catch
                    {
                        ss1 = textBoxXHJHSJ.Text.Trim();
                    }
                    e.Graphics.DrawString(ss1, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);

                    //if (dateTimePicker.Value > dtStart)
                    //    e.Graphics.DrawString(dateTimePicker.Value.ToString("yyyy��M��dd��"), _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawString(textBoxXHJHSJ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth / 2, iyRow + iHeight12 - 6));

                    e.Graphics.DrawString("���ʽ��", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("���ʽ��", _Font12).Width;
                    e.Graphics.DrawString(textBoxXHFKFS.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow, iWidth / 2 - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;


                    e.Graphics.DrawString("�š��ڻ�������ʱ�䣺", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("�š��ڻ�������ʱ�䣺", _Font12).Width;
                    try
                    {
                        ss1 = DateTime.Parse(textBoxQHJHSJ.Text.Trim()).ToString("yyyy��M��dd��");
                    }
                    catch
                    {
                        ss1 = textBoxQHJHSJ.Text.Trim();
                    }

                    e.Graphics.DrawString(ss1, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);

                    //if (dateTimePickerQHJHSJ.Value > dtStart)
                    //    e.Graphics.DrawString(dateTimePickerQHJHSJ.Value.ToString("yyyy��M��dd��"), _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawString(textBoxQHJHSJ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth / 2, iyRow + iHeight12 - 6));

                    e.Graphics.DrawString("Ԥ����", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("Ԥ������", _Font12).Width;
                    e.Graphics.DrawString(textBoxYFJE.Text.Trim(), _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow, iWidth / 2 - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("�������Ӧ����", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("�������Ӧ����", _Font12).Width;
                    e.Graphics.DrawString(textBoxTHYFYE.Text.Trim(), _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth / 2, iyRow + iHeight12 - 6));

                    e.Graphics.DrawString("���ʽ��", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("���ʽ����", _Font12).Width;
                    e.Graphics.DrawString(textBoxQHFKFS.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow, iWidth / 2 - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;


                    e.Graphics.DrawString("ʮ��ΥԼ���Σ�", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("ʮ��ΥԼ���Σ�", _Font12).Width;
                    e.Graphics.DrawString(textBoxWYZR.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));

                    iyRow += iHeight12;

                    e.Graphics.DrawString("ʮһ�������ͬ���׵ķ�ʽ��", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    iyRow += iHeight12;
                    e.Graphics.DrawString("����ִ�к�ͬ�������飬�ɵ�����˫��Э�̽����Э�̲��ɣ�˫��ͬ����" + textBoxZCWYH.Text + "�ٲ�ίԱ���ٲá�", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawString("ʮ��������Լ�����", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("ʮ��������Լ�����", _Font12).Width;
                    e.Graphics.DrawString(textBoxQTYDSX.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("ʮ������ͬ��Ч�ڣ�", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("ʮ������ͬ��Ч�ڣ�", _Font12).Width;
                    //e.Graphics.DrawString(dateTimePickerHTYXQ.Value.ToString("yyyy��M��dd��"), _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawString(textBoxHTYXQ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);

                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12 + iHeight9;

                    //iyRow = 780;

                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM - 4, iyRow, iWidth + 8, 322));
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM, iyRow + 4, iWidth / 2, 314));
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + iWidth / 2, iyRow + 4, iWidth / 2, 314));


                    iyRow += 10;

                    e.Graphics.DrawString("����", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2, iHeight12), StrFormat);
                    e.Graphics.DrawString("�跽", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2, iHeight12), StrFormat);
                    iyRow += iHeight12;

                    fTemp = e.Graphics.MeasureString("��˾���ƣ�", _Font12).Width;
                    /*
                    e.Graphics.DrawString("��˾���ƣ�"+sGSMC, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("��˾���ƣ�" + textBoxDWMC.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;
                     */
                    e.Graphics.DrawString("��˾���ƣ�", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString(sGSMC, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow + 3, iWidth / 2 - 5 - fTemp, iHeight12 * 2), StrFormatL1);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 4)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 * 2 - 4)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 * 2 - 6));
                    e.Graphics.DrawString("��˾���ƣ�", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString(textBoxDWMC.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow + 3, iWidth / 2 - 5 - fTemp, iHeight12 * 2), StrFormatL1);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 4)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 * 2 - 4)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 * 2 - 6));
                    iyRow += iHeight12 * 2;

                    //e.Graphics.DrawString("�����ˣ���"+ textBoxFZR1.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString("�����ˣ���" + comboBoxYWY.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString("�����ˣ���" + textBoxFZR.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawString("�����£�", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString("�����£�", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawString("��˾��ַ��", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString(sGSDZ, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow+3, iWidth / 2 - 5 - fTemp, iHeight12 * 2), StrFormatL1);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 4)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 * 2 - 4)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 * 2 - 6));
                    e.Graphics.DrawString("��˾��ַ��", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString(textBoxGSDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow+3, iWidth / 2 - 5 - fTemp, iHeight12*2), StrFormatL1);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 4)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 * 2 - 4)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 * 2 - 6));
                    iyRow += iHeight12 * 2;

                    /*
                    e.Graphics.DrawString("��˾�绰��"+sGSDH, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("��˾�绰��" + textBoxGSDH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                     */
                    e.Graphics.DrawString("��˾�绰��", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString(sGSDH, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow + 3, iWidth / 2 - 5 - fTemp, iHeight12 * 2), StrFormatL1);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 4)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 * 2 - 4)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 * 2 - 6));
                    e.Graphics.DrawString("��˾�绰��", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString(textBoxGSDH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow + 3, iWidth / 2 - 5 - fTemp, iHeight12 * 2), StrFormatL1);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 4)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 * 2 - 4)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 * 2 - 6));
                    iyRow += iHeight12 * 2;


                    e.Graphics.DrawString("��˾���棺"+sGSCZ, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("��˾���棺" + textBoxGSCZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("˰�����ţ�"+sGSSH, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("˰�����ţ�" + textBoxSH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("�������У�"+sGSKHYH, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("�������У�" + textBoxKHYH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("�ʡ����ţ�"+sGSZH, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("�ʡ����ţ�" + textBoxZH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("�������룺"+sGSYB, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("�������룺" + textBoxYZBM.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));

                    iyRow = e.PageSettings.PaperSize.Height - 80;

                    //fTemp = e.Graphics.MeasureString("�Զ��������ң��������", _Font9).Width;
                    fTemp = e.Graphics.MeasureString(sK, _Font9).Width;
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM), (int)(iHeight9 / 2 + iyRow)), new Point((int)(iLeftM + (iWidth - fTemp) / 2), (int)(iHeight9 / 2 + iyRow)));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + (iWidth + fTemp) / 2), (int)(iHeight9 / 2 + iyRow)), new Point(iLeftM + iWidth - 5, (int)(iHeight9 / 2 + iyRow)));
                    //e.Graphics.DrawString("�Զ��������ң��������", _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight9), StrFormat);
                    e.Graphics.DrawString(sK, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight9), StrFormat);
                    iyRow += iHeight9;
                    e.Graphics.DrawString(sM1, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, (int)(iWidth / 4), iHeight9), StrFormat);
                    e.Graphics.DrawString(sM2, _Font9, b, new System.Drawing.RectangleF((int)(iLeftM + iWidth / 4), iyRow, (int)(iWidth / 4), iHeight9), StrFormat);
                    e.Graphics.DrawString(sM3, _Font9, b, new System.Drawing.RectangleF((int)(iLeftM + iWidth / 2), iyRow, (int)(iWidth / 4), iHeight9), StrFormat);
                    e.Graphics.DrawString(sM4, _Font9, b, new System.Drawing.RectangleF((int)(iLeftM + iWidth * 3 / 4), iyRow, (int)(iWidth / 4), iHeight9), StrFormat);

                    if (Convert.ToInt32(labelHTMXJL.Text) > 6) //
                    {
                        PageNo++;
                        e.HasMorePages = true;
                        return;
                    }
                }
                else //��ϸ
                {
                    iPages = (int)Math.Ceiling((decimal)(dataGridViewHT.Rows.Count) / (decimal)(iRowsPerPage));
                    e.Graphics.DrawString("���ۺ�ͬ��Ʒ�嵥", _Font22, b, new System.Drawing.RectangleF(iLeftM, iTopM, iWidth, iHeight22), StrFormat);
                    iyRow += iTopM + iHeight22;
                    e.Graphics.DrawString("��ͬ��ţ�" + labelHTH.Text + "��", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormat);
                    iyRow += iHeight12;

                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                    e.Graphics.DrawString("���", _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 260, iHeight12));
                    e.Graphics.DrawString("��Ʒ����", _Font12, b, new System.Drawing.RectangleF(iLeftM + 60, iyRow, 260, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 320, iyRow, 80, iHeight12));
                    e.Graphics.DrawString("����", _Font12, b, new System.Drawing.RectangleF(iLeftM + 320, iyRow, 80, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 400, iyRow, 100, iHeight12));
                    e.Graphics.DrawString("����", _Font12, b, new System.Drawing.RectangleF(iLeftM + 400, iyRow, 100, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 100, iHeight12));
                    e.Graphics.DrawString("�ܼ�", _Font12, b, new System.Drawing.RectangleF(iLeftM + 500, iyRow, 100, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 600, iyRow, 100, iHeight12));
                    e.Graphics.DrawString("��ע", _Font12, b, new System.Drawing.RectangleF(iLeftM + 600, iyRow, 100, iHeight12), StrFormat);
                    iyRow += iHeight12;

                    for (i = 0; i < iRowsPerPage; i++)
                    {
                        if (RowPos >= dataGridViewHT.Rows.Count) //����
                        {
                            NewPage = false;
                            break;
                        }

                        if (dataGridViewHT.Rows[RowPos].IsNewRow)
                        {
                            break;
                        }



                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                        e.Graphics.DrawString(Convert.ToString(RowPos + 1), _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 260, iHeight12));
                        e.Graphics.DrawString(dataGridViewHT.Rows[RowPos].Cells[1].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iLeftM + 60, iyRow, 260, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 320, iyRow, 80, iHeight12));
                        e.Graphics.DrawString(dataGridViewHT.Rows[RowPos].Cells[3].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iLeftM + 320, iyRow, 80, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 400, iyRow, 100, iHeight12));
                        e.Graphics.DrawString(Convert.ToDecimal(dataGridViewHT.Rows[RowPos].Cells[4].Value.ToString()).ToString("f2"), _Font12, b, new System.Drawing.RectangleF(iLeftM + 400, iyRow, 100, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 100, iHeight12));
                        e.Graphics.DrawString(Convert.ToDecimal(dataGridViewHT.Rows[RowPos].Cells[5].Value.ToString()).ToString("f2"), _Font12, b, new System.Drawing.RectangleF(iLeftM + 500, iyRow, 100, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 600, iyRow, 100, iHeight12));
                        e.Graphics.DrawString(dataGridViewHT.Rows[RowPos].Cells[6].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iLeftM + 600, iyRow, 100, iHeight12), StrFormat);
                        iyRow += iHeight12;
                        RowPos++;
                    }
                    iTemp = iyRow;
                    iyRow = e.PageSettings.PaperSize.Height - 70;

                    e.Graphics.DrawString("��" + (PageNo - 1).ToString() + "ҳ����" + iPages.ToString() + "ҳ", _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight9), StrFormatR);
                    iyRow += iHeight9;

                    fTemp = e.Graphics.MeasureString(sK, _Font9).Width;
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM), (int)(iHeight9 / 2 + iyRow)), new Point((int)(iLeftM + (iWidth - fTemp) / 2), (int)(iHeight9 / 2 + iyRow)));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + (iWidth + fTemp) / 2), (int)(iHeight9 / 2 + iyRow)), new Point(iLeftM + iWidth - 5, (int)(iHeight9 / 2 + iyRow)));
                    e.Graphics.DrawString(sK, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight9), StrFormat);
                    iyRow += iHeight9;
                    e.Graphics.DrawString(sM1, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, (int)(iWidth / 4), iHeight9), StrFormat);
                    e.Graphics.DrawString(sM2, _Font9, b, new System.Drawing.RectangleF((int)(iLeftM + iWidth / 4), iyRow, (int)(iWidth / 4), iHeight9), StrFormat);
                    e.Graphics.DrawString(sM3, _Font9, b, new System.Drawing.RectangleF((int)(iLeftM + iWidth / 2), iyRow, (int)(iWidth / 4), iHeight9), StrFormat);
                    e.Graphics.DrawString(sM4, _Font9, b, new System.Drawing.RectangleF((int)(iLeftM + iWidth * 3 / 4), iyRow, (int)(iWidth / 4), iHeight9), StrFormat);

                    //���ϼ�
                    if (RowPos >= dataGridViewHT.Rows.Count - 1)
                    {
                        iyRow = iTemp;

                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                        e.Graphics.DrawString("�ϼ�", _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 440, iHeight12));
                        e.Graphics.DrawString(labelDX.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + 80, iyRow, 420, iHeight12), StrFormatL);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 200, iHeight12));
                        e.Graphics.DrawString("��" + labelJEHJ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + 520, iyRow, 180, iHeight12), StrFormatL);
                        e.HasMorePages = false;

                    }
                    else
                    {

                        if (NewPage)
                        {
                            PageNo++;
                            e.HasMorePages = true;
                        }
                        else
                        {
                            e.HasMorePages = false;
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��ӡʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxXHJHSJ_Validating(object sender, CancelEventArgs e)
        {
            /*
            if (textBoxXHJHSJ.Text.Trim() == "")
            {
                this.errorProviderHT.Clear();
                return;
            }

            try
            {
                DateTime.Parse(textBoxXHJHSJ.Text.Trim());
                this.errorProviderHT.Clear();
            }
            catch
            {
                this.errorProviderHT.SetError(this.textBoxXHJHSJ, "��������Ч���ڣ����磺2000-12-31");
                e.Cancel = true;
            }
             */
        }

        private void textBoxQHJHSJ_Validating(object sender, CancelEventArgs e)
        {
            /*
            if (textBoxQHJHSJ.Text.Trim() == "")
            {
                this.errorProviderHT.Clear();
                return;
            }

            try
            {
                DateTime.Parse(textBoxQHJHSJ.Text.Trim());
                this.errorProviderHT.Clear();
            }
            catch
            {
                this.errorProviderHT.SetError(this.textBoxQHJHSJ, "��������Ч���ڣ����磺2000-12-31");
                e.Cancel = true;
            }
             */
        }

        private void textBoxHTYXQ_Validating(object sender, CancelEventArgs e)
        {
            /*
            if (textBoxHTYXQ.Text.Trim() == "")
            {
                this.errorProviderHT.Clear();
                return;
            }

            try
            {
                DateTime.Parse(textBoxHTYXQ.Text.Trim());
                this.errorProviderHT.Clear();
            }
            catch
            {
                this.errorProviderHT.SetError(this.textBoxHTYXQ, "��������Ч���ڣ����磺2000-12-31");
                e.Cancel = true;
            }
             */
        }

        private void toolStripButtonWord_Click(object sender, EventArgs e)
        {
            int i, j;
            int iNUMROW = 0;

            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("���ۺ�ͬ��ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }



            SaveFileDialog saveFileDialogOutput = new SaveFileDialog();
            saveFileDialogOutput.Filter = "WORD files(*.doc)|*.doc";//
            saveFileDialogOutput.FilterIndex = 0;
            saveFileDialogOutput.RestoreDirectory = true;

            if (saveFileDialogOutput.ShowDialog() != DialogResult.OK) return;

            try
            {

                string FullFileName = saveFileDialogOutput.FileName.ToString();
                FileInfo info = new FileInfo(FullFileName);
 

                Microsoft.Office.Interop.Word.Application docApp = new Microsoft.Office.Interop.Word.ApplicationClass();
                object missingValue = Type.Missing;

                object template = Directory.GetCurrentDirectory() + "\\Sample\\xshtmb1.dot";

                _Document doc = docApp.Documents.Add(ref template, ref missingValue, ref missingValue, ref missingValue);

                string bookmarkName = "HTBH";
                object oBookmarkName = bookmarkName;
                Microsoft.Office.Interop.Word.Range rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = labelHTH.Text;
                object oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GF";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxDWMC.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "XF";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSMC;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "QDDD";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxQDDD.Text+"�����Ƶ���"+labelCZY.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "QDSJ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = labelZDRQ.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "MXBG";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;

                if (dataGridViewHT.Rows[dataGridViewHT.Rows.Count-1].IsNewRow)
                {
                    iNUMROW = dataGridViewHT.Rows.Count - 1;
                }
                else
                    iNUMROW = dataGridViewHT.Rows.Count;

                Table tbMX = doc.Tables.Add(rng, iNUMROW + 2, 6, ref missingValue, ref missingValue);

                tbMX.Borders[WdBorderType.wdBorderLeft].LineStyle = WdLineStyle.wdLineStyleSingle;
                tbMX.Borders[WdBorderType.wdBorderLeft].LineWidth = WdLineWidth.wdLineWidth050pt;

                tbMX.Borders[WdBorderType.wdBorderRight].LineStyle = WdLineStyle.wdLineStyleSingle;
                tbMX.Borders[WdBorderType.wdBorderRight].LineWidth = WdLineWidth.wdLineWidth050pt;

                tbMX.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
                tbMX.Borders[WdBorderType.wdBorderTop].LineWidth = WdLineWidth.wdLineWidth050pt;

                tbMX.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
                tbMX.Borders[WdBorderType.wdBorderBottom].LineWidth = WdLineWidth.wdLineWidth050pt;

                tbMX.Borders[WdBorderType.wdBorderHorizontal].LineStyle = WdLineStyle.wdLineStyleSingle;
                tbMX.Borders[WdBorderType.wdBorderHorizontal].LineWidth = WdLineWidth.wdLineWidth050pt;

                tbMX.Borders[WdBorderType.wdBorderVertical].LineStyle = WdLineStyle.wdLineStyleSingle;
                tbMX.Borders[WdBorderType.wdBorderVertical].LineWidth = WdLineWidth.wdLineWidth050pt;

                tbMX.Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                tbMX.Range.Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
                //tbMX.Range.w



                tbMX.Cell(1, 1).Range.Text = "���";
                tbMX.Cell(1, 2).Range.Text = "��Ʒ����";
                tbMX.Cell(1, 3).Range.Text = "����";
                tbMX.Cell(1, 4).Range.Text = "����";
                tbMX.Cell(1, 5).Range.Text = "�ܼ�";
                tbMX.Cell(1, 6).Range.Text = "��ע";

                tbMX.Columns[1].SetWidth(35, WdRulerStyle.wdAdjustNone);
                tbMX.Columns[2].SetWidth(115, WdRulerStyle.wdAdjustNone);
                tbMX.Columns[3].SetWidth(50, WdRulerStyle.wdAdjustNone);
                tbMX.Columns[4].SetWidth(80, WdRulerStyle.wdAdjustNone);
                tbMX.Columns[5].SetWidth(80, WdRulerStyle.wdAdjustNone);
                tbMX.Columns[6].SetWidth(60, WdRulerStyle.wdAdjustNone);


                for (i = 0; i < iNUMROW; i++)
                {
                    if (dataGridViewHT.Rows[i].IsNewRow)
                    {
                        continue;
                    }

                    tbMX.Cell(i + 2, 1).Range.Text = (i + 1).ToString();
                    tbMX.Cell(i + 2, 2).Range.Text = dataGridViewHT.Rows[i].Cells[1].Value.ToString();
                    tbMX.Cell(i + 2, 3).Range.Text = dataGridViewHT.Rows[i].Cells[3].Value.ToString();
                    tbMX.Cell(i + 2, 4).Range.Text = decimal.Parse(dataGridViewHT.Rows[i].Cells[4].Value.ToString()).ToString("f2");
                    tbMX.Cell(i + 2, 5).Range.Text = decimal.Parse(dataGridViewHT.Rows[i].Cells[5].Value.ToString()).ToString("f2");
                    tbMX.Cell(i + 2, 6).Range.Text = dataGridViewHT.Rows[i].Cells[6].Value.ToString();

                }

                i = iNUMROW + 2;
                tbMX.Cell(i, 1).Range.Text = "�ϼ�";
                tbMX.Cell(i, 2).Merge(tbMX.Cell(i, 3));
                tbMX.Cell(i, 2).Merge(tbMX.Cell(i, 3));
                tbMX.Cell(i, 3).Merge(tbMX.Cell(i, 4));
                tbMX.Cell(i, 2).Range.Text = labelDX.Text;
                tbMX.Cell(i, 3).Range.Text = decimal.Parse(labelJEHJ.Text).ToString("f2"); ;


                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "ZLYQ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxZLYQ.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);


                bookmarkName = "YYQX";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxYYQX.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "FZTJ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxFZTJ.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);


                bookmarkName = "SJBP";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxSJBP.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "YSFS";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = comboBoxYSFS.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "YSZ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxYSZ.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "FYCD";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = comboBoxFYCD.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "JHDD";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxJHDD.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                string ss1 = textBoxXHJHSJ.Text.Trim();

                try
                {
                    ss1 = DateTime.Parse(textBoxXHJHSJ.Text.Trim()).ToString("yyyy��M��dd��");
                }
                catch
                {
                    ss1 = textBoxXHJHSJ.Text.Trim();
                }

                bookmarkName = "XHJHSJ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = ss1;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "XHFKFS";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxXHFKFS.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                try
                {
                    ss1 = DateTime.Parse(textBoxQHJHSJ.Text.Trim()).ToString("yyyy��M��dd��");
                }
                catch
                {
                    ss1 = textBoxQHJHSJ.Text.Trim();
                }

                bookmarkName = "QHJHSJ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = ss1;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "YFJE";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxYFJE.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "THYFYE";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxTHYFYE.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "QHFKFS";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxQHFKFS.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "WYZR";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxWYZR.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "ZCWYH";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxZCWYH.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "QTYDSX";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxQTYDSX.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "QTYDSX";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxQTYDSX.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);


                try
                {
                    ss1 = DateTime.Parse(textBoxHTYXQ.Text.Trim()).ToString("yyyy��M��dd��");
                }
                catch
                {
                    ss1 = textBoxHTYXQ.Text.Trim();
                }
                bookmarkName = "HTYXQ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = ss1;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GSMC1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSMC;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GSMC2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxDWMC.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "FZR1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                //rng.Text = textBoxFZR1.Text;
                rng.Text = comboBoxYWY.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "FZR2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxFZR.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GSDZ1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSDZ;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);


                bookmarkName = "GSDZ2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxGSDZ.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GSDH1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSDH;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);


                bookmarkName = "GSDH2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxGSDH.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GSCZ1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSCZ;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GSCZ2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxGSCZ.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "SH1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSSH;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "SH2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxSH.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "KHYH1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSKHYH;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "KHYH2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxKHYH.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "ZH1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSZH;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "ZH2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxZH.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "YZBM1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSYB;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "YZBM2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxYZBM.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);
                docApp.Visible = true;
                doc.PrintPreview();

                object fileName = FullFileName;
                doc.SaveAs(ref fileName, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue);
                MessageBox.Show("��ͬ�������");

                doc.Close(ref missingValue, ref missingValue, ref missingValue);
                docApp.Quit(ref missingValue, ref missingValue, ref missingValue);



                if (doc != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                    doc = null;
                }
                if (docApp != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(docApp);
                    docApp = null;
                }
                GC.Collect();
                //KillWordProcess();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "���ʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem tsMI in toolStripDropDownButtonBJ.DropDownItems)
            {
                tsMI.Checked = false;
            }

            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.Checked = true;

        }

        private void comboBoxYWY_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxFZR1.Text = comboBoxYWY.Text;
        }
        private void toolStripMenuItemUP_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender;
            ContextMenuStrip cmenu = (ContextMenuStrip)menuitem.GetCurrentParent();


            DataGridView dv = (DataGridView)(cmenu.SourceControl);

            if (dv.CurrentCell.RowIndex <= 0 || dv.Rows.Count <= 1 || dv.Rows[dv.CurrentCell.RowIndex].IsNewRow)
                return;

            int i, count = dv.ColumnCount;
            object[] dr1 = new object[count];

            for (i = 0; i < count; i++)
            {
                dr1[i] = dv.Rows[dv.CurrentCell.RowIndex - 1].Cells[i].Value;
            }

            for (i = 0; i < count; i++)
            {
                dv.Rows[dv.CurrentCell.RowIndex - 1].Cells[i].Value = dv.Rows[dv.CurrentCell.RowIndex].Cells[i].Value;
            }


            for (i = 0; i < count; i++)
            {
                dv.Rows[dv.CurrentCell.RowIndex].Cells[i].Value = dr1[i];
            }
            dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex - 1].Cells[dv.CurrentCell.ColumnIndex];

            countAmount();

        }

        private void toolStripMenuItemDOWN_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender;
            ContextMenuStrip cmenu = (ContextMenuStrip)menuitem.GetCurrentParent();


            DataGridView dv = (DataGridView)(cmenu.SourceControl);

            if (dv.CurrentCell.RowIndex >= dv.Rows.Count - 2 || dv.Rows.Count <= 1 || dv.Rows[dv.CurrentCell.RowIndex].IsNewRow)
                return;

            int i, count = dv.ColumnCount;
            object[] dr1 = new object[count];

            for (i = 0; i < count; i++)
            {
                dr1[i] = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[i].Value;
            }

            for (i = 0; i < count; i++)
            {
                dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[i].Value = dv.Rows[dv.CurrentCell.RowIndex].Cells[i].Value;
            }


            for (i = 0; i < count; i++)
            {
                dv.Rows[dv.CurrentCell.RowIndex].Cells[i].Value = dr1[i];
            }
            dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[dv.CurrentCell.ColumnIndex];

            countAmount();
        }

        private void dataGridViewHT_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex != 4 || e.RowIndex < 0)
                return;


            if (Convert.ToDecimal(dataGridViewHT.Rows[e.RowIndex].Cells[4].Value) < Convert.ToDecimal(dataGridViewHT.Rows[e.RowIndex].Cells[9].Value))
            {
                e.CellStyle.BackColor = Color.LightPink;
            }
        }




    }
}