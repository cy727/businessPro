using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormGJSPZD : Form
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
        private int intHTH = 0;
        private ClassGetInformation cGetInformation;

        //public bool isSaved = true;
        //public int iDJID = 11;

        public bool isSaved = false;
        public int iDJID = 0;
        private bool bCheck = true;

        public int iVersion = 1;

        
        public FormGJSPZD()
        {
            InitializeComponent();
        }

        private void FormGJSPZD_Load(object sender, EventArgs e)
        {
            int i;

            this.Left = 1;
            this.Top = 1;
            textBoxHTH.Focus();

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            
            if (isSaved)
            {
                dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
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
            sqlComm.CommandText = "SELECT ID, �������� FROM ���ű� WHERE (beactive = 1)";

            if (dSet.Tables.Contains("���ű�")) dSet.Tables.Remove("���ű�");
            sqlDA.Fill(dSet, "���ű�");

            DataRow drTemp = dSet.Tables["���ű�"].NewRow();
            drTemp[0] = 0;
            drTemp[1] = "ȫ��";
            dSet.Tables["���ű�"].Rows.Add(drTemp);

            comboBoxBM.DataSource = dSet.Tables["���ű�"];
            comboBoxBM.DisplayMember = "��������";
            comboBoxBM.ValueMember = "ID";
            comboBoxBM.SelectedValue = intUserBM;
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;

            //��ʼ��֧����ʽ
            sqlComm.CommandText = "SELECT ID, ֧����ʽ FROM ֧����ʽ��";

            if (dSet.Tables.Contains("֧����ʽ��")) dSet.Tables.Remove("֧����ʽ��");
            sqlDA.Fill(dSet, "֧����ʽ��");
            comboBoxZFFS.DataSource = dSet.Tables["֧����ʽ��"];
            comboBoxZFFS.DisplayMember = "֧����ʽ";
            comboBoxZFFS.ValueMember = "ID";
            comboBoxZFFS.Text = "";



            //��ʼ����Ʒ�б�
            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���ϸ�����.����, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���,��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.��Ʒ, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ��.�ⷿID, ��Ʒ��.���ս���, ������Ʒ�Ƶ���ϸ��.ID, ��Ʒ��.��߽���, ��Ʒ��.��ͽ���, ��Ʒ��.������� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN ������Ʒ�Ƶ���ϸ����� WHERE (������Ʒ�Ƶ���ϸ��.��ID = 0)";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewGJSPZD.DataSource = dSet.Tables["��Ʒ��"];

            dataGridViewGJSPZD.Columns[0].Visible = false;
            dataGridViewGJSPZD.Columns[13].Visible = false;
            dataGridViewGJSPZD.Columns[14].Visible = false;
            dataGridViewGJSPZD.Columns[16].Visible = false;
            dataGridViewGJSPZD.Columns[3].ReadOnly = true;
            dataGridViewGJSPZD.Columns[4].ReadOnly = true;
            dataGridViewGJSPZD.Columns[9].ReadOnly = true;
            dataGridViewGJSPZD.Columns[12].ReadOnly = true;
            dataGridViewGJSPZD.Columns[17].ReadOnly = true;
            dataGridViewGJSPZD.Columns[18].ReadOnly = true;
            dataGridViewGJSPZD.Columns[19].ReadOnly = true;
            dataGridViewGJSPZD.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[18].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[19].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridViewGJSPZD.Columns[5].ReadOnly = true;
            dataGridViewGJSPZD.Columns[6].ReadOnly = true;

            dataGridViewGJSPZD.ShowCellErrors = true;

            dataGridViewGJSPZD.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewGJSPZD.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[12].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[15].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[17].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[18].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[19].DefaultCellStyle.Format = "f0";

            dataGridViewGJSPZD.Columns[15].Visible = false;
            dataGridViewGJSPZD.Columns[17].Visible = false;
            dataGridViewGJSPZD.Columns[18].Visible = false;

            
            sqlConn.Close();

            //initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT=cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;


        }

        private void initDJ()
        {
            int iBM = 0;
            sqlConn.Open();

            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ҵ��Ա.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, ������Ʒ�Ƶ���.��ע,��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��˰�ϼ�, �ɹ���ͬ��.��ͬ���, ������Ʒ�Ƶ���.��Ʊ��, ������Ʒ�Ƶ���.���ʽ, ������Ʒ�Ƶ���.����ID, ������Ʒ�Ƶ���.BeActive FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ҵ��Ա ON ������Ʒ�Ƶ���.ҵ��ԱID = ҵ��Ա.ID INNER JOIN ְԱ�� ����Ա ON ������Ʒ�Ƶ���.����ԱID = ����Ա.ID LEFT OUTER JOIN �ɹ���ͬ�� ON ������Ʒ�Ƶ���.��ͬID = �ɹ���ͬ��.ID WHERE (������Ʒ�Ƶ���.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy��M��dd��");

                if (sqldr.GetValue(11).ToString() != "")
                {
                    try
                    {
                        iBM = int.Parse(sqldr.GetValue(11).ToString());
                    }
                    catch
                    {
                        iBM = 0;
                    }

                }

                if (!bool.Parse(sqldr.GetValue(12).ToString()))
                {
                    labelDJBH.ForeColor = Color.Red;
                }

                comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                textBoxDWMC.Text = sqldr.GetValue(6).ToString();
                textBoxHTH.Text = sqldr.GetValue(8).ToString();
                textBoxFPH.Text = sqldr.GetValue(9).ToString();
                comboBoxZFFS.Text = sqldr.GetValue(10).ToString();



                this.Text = "������Ʒ�Ƶ���" + labelDJBH.Text;
            }
            sqldr.Close();

            /*
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
             */
            if (iBM != 0)
            {
                comboBoxBM.SelectedIndexChanged -= comboBoxBM_SelectedIndexChanged;
                sqlComm.CommandText = "SELECT �������� FROM ���ű� WHERE (ID = " + iBM.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                    comboBoxBM.Text = sqldr.GetValue(0).ToString();
                    break;
                }
                sqldr.Close();
                comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;
            }


            //��ʼ����Ʒ�б�
            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���ϸ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.��Ʒ, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ��Ʒ��.ID AS ��ƷID, �ⷿ��.ID AS �ⷿID, ��Ʒ��.���ս���, ������Ʒ�Ƶ���ϸ��.ID AS ����ID, ��Ʒ��.��߽���, ��Ʒ��.��ͽ���, ��Ʒ��.������� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID WHERE (������Ʒ�Ƶ���ϸ��.��ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewGJSPZD.DataSource = dSet.Tables["��Ʒ��"];

            dataGridViewGJSPZD.Columns[0].Visible = false;
            dataGridViewGJSPZD.Columns[13].Visible = false;
            dataGridViewGJSPZD.Columns[14].Visible = false;
            dataGridViewGJSPZD.Columns[16].Visible = false;
            dataGridViewGJSPZD.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[18].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[19].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.ShowCellErrors = true;

            dataGridViewGJSPZD.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewGJSPZD.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[12].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[15].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[17].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[18].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[19].DefaultCellStyle.Format = "f0";

            dataGridViewGJSPZD.Columns[15].Visible = false;
            dataGridViewGJSPZD.Columns[17].Visible = false;
            dataGridViewGJSPZD.Columns[18].Visible = false;


            dataGridViewGJSPZD.ReadOnly = true;
            dataGridViewGJSPZD.AllowUserToAddRows = false;
            dataGridViewGJSPZD.AllowUserToDeleteRows = false;


            sqlConn.Close();

            dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
            dataGridViewGJSPZD.RowValidating -= dataGridViewGJSPZD_RowValidating;
            dataGridViewGJSPZD.CellDoubleClick -= dataGridViewGJSPZD_CellDoubleClick;

            dataGridViewGJSPZD.CellPainting += dataGridViewGJSPZD_CellPainting;
        }


        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            
            if (cGetInformation.getCompanyInformation(1, "") == 0)
            {
                //return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                if (cGetInformation.iBMID != 0)
                    comboBoxBM.SelectedValue = cGetInformation.iBMID;

                comboBoxYWY.Text = cGetInformation.sCompanyYWY;

            }
            intHTH = 0;
            textBoxHTH.Text = "";
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(12, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    intHTH = 0;
                    textBoxHTH.Text = "";
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    if (cGetInformation.iBMID != 0)
                        comboBoxBM.SelectedValue = cGetInformation.iBMID;

                    comboBoxYWY.Text = cGetInformation.sCompanyYWY;

                }
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(10, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                    intHTH = 0;
                    textBoxHTH.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    comboBoxYWY.Text = cGetInformation.sCompanyYWY;

                    intHTH = 0;
                    textBoxHTH.Text = "";
                }
            }
        }

        private void comboBoxBM_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            sqlConn.Open();
            //��ʼ��Ա���б�
            if (comboBoxBM.Text.Trim()!="ȫ��")
                sqlComm.CommandText = "SELECT ְԱ��.ID,ְԱ��.ְԱ����, ְԱ��.ְԱ��� FROM ְԱ�� INNER JOIN ���ű� ON ְԱ��.����ID = ���ű�.ID WHERE (���ű�.�������� = N'" + comboBoxBM.Text.Trim() + "') AND (ְԱ��.beactive = 1)";
            else
                sqlComm.CommandText = "SELECT ְԱ��.ID,ְԱ��.ְԱ����, ְԱ��.ְԱ��� FROM ְԱ�� INNER JOIN ���ű� ON ְԱ��.����ID = ���ű�.ID WHERE (ְԱ��.beactive = 1)";

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
             */
        }



        private void textBoxHTH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getBillInformation(50, textBoxHTH.Text.Trim()) == 0)
            {
                textBoxHTH.Text = "";
                intHTH = 0;
                return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iBillCNumber;
                textBoxDWMC.Text = cGetInformation.strBillCName;
                textBoxDWBH.Text = cGetInformation.strBillCCode;
                textBoxHTH.Text = cGetInformation.strBillCode;
                intHTH = cGetInformation.iBillNumber;
                comboBoxBM.SelectedValue = cGetInformation.iBillBMID;
                comboBoxYWY.Text = cGetInformation.sPeopleName;

                getHTDetail();
                dataGridViewGJSPZD.Focus();
            }
            
        }

        private void getHTDetail()
        {

            if (intHTH == 0)
                return;

            bCheck = false;



            sqlConn.Open();

            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���ϸ�����.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �ɹ���ͬ��ϸ��.����, �ɹ���ͬ��ϸ��.����, �ɹ���ͬ��ϸ��.�ܼ� AS ���, ������Ʒ�Ƶ���ϸ�����.��Ʒ, ������Ʒ�Ƶ���ϸ�����.����, �ɹ���ͬ��ϸ��.�ܼ� AS ʵ�ƽ��, �ɹ���ͬ��ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ�����.�ⷿID, ��Ʒ��.���ս���, ������Ʒ�Ƶ���ϸ�����.ID AS ����ID, ��Ʒ��.��߽���, ��Ʒ��.��ͽ���, ��Ʒ��.������� FROM �ɹ���ͬ��ϸ�� INNER JOIN ��Ʒ�� ON �ɹ���ͬ��ϸ��.��ƷID = ��Ʒ��.ID CROSS JOIN ������Ʒ�Ƶ���ϸ����� LEFT OUTER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ�����.�ⷿID = �ⷿ��.ID WHERE (�ɹ���ͬ��ϸ��.�ɹ���ͬID = " + intHTH.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewGJSPZD.DataSource = dSet.Tables["��Ʒ��"];


            dataGridViewGJSPZD.Columns[0].Visible = false;
            dataGridViewGJSPZD.Columns[13].Visible = false;
            dataGridViewGJSPZD.Columns[14].Visible = false;
            dataGridViewGJSPZD.Columns[16].Visible = false;
            dataGridViewGJSPZD.Columns[3].ReadOnly = true;
            dataGridViewGJSPZD.Columns[4].ReadOnly = true;
            dataGridViewGJSPZD.Columns[9].ReadOnly = true;
            dataGridViewGJSPZD.Columns[12].ReadOnly = true;
            dataGridViewGJSPZD.Columns[17].ReadOnly = true;
            dataGridViewGJSPZD.Columns[18].ReadOnly = true;
            dataGridViewGJSPZD.Columns[19].ReadOnly = true;
            dataGridViewGJSPZD.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[18].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[19].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


            sqlConn.Close();

            //�õ��ⷿ
            for (int i = 0; i < dataGridViewGJSPZD.Rows.Count; i++)
            {
                if (dataGridViewGJSPZD.Rows[i].IsNewRow)
                    continue;

                cGetInformation.iCommNumber = Convert.ToInt32(dataGridViewGJSPZD.Rows[i].Cells[13].Value);
                cGetInformation.getCommKF();

                dataGridViewGJSPZD.Rows[i].Cells[5].Value = cGetInformation.strKFCode;
                dataGridViewGJSPZD.Rows[i].Cells[6].Value = cGetInformation.strKFName;
                dataGridViewGJSPZD.Rows[i].Cells[14].Value = cGetInformation.iKFNumber;

            }

            countAmount();
            if (dataGridViewGJSPZD.Rows.Count>0)
                dataGridViewGJSPZD.CurrentCell = dataGridViewGJSPZD.Rows[dataGridViewGJSPZD.Rows.Count-1].Cells[1];

            bCheck = true;


        }

        private void dataGridViewGJSPZD_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                    this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = cGetInformation.decCommZZJJ.ToString();
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[17].Value = cGetInformation.decCommZGJJ.ToString();
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[18].Value = cGetInformation.decCommZDJJ.ToString();
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[19].Value = cGetInformation.decCommKCSL.ToString("f0");

                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;


                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                    dataGridViewGJSPZD.EndEdit();
                    dataGridViewGJSPZD.CurrentCell = dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7];
                    dataGridViewGJSPZD.BeginEdit(true);
                    this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                }
            }

            if (e.ColumnIndex == 5 || e.ColumnIndex == 6) //�ⷿ���
            {
                if (cGetInformation.getKFInformation(1, "") == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;

                    dataGridViewGJSPZD.EndEdit();
                    dataGridViewGJSPZD.CurrentCell = dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7];
                    dataGridViewGJSPZD.BeginEdit(true);
                    this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                }
            }
            
        }

        private void dataGridViewGJSPZD_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("���������ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridViewGJSPZD_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (isSaved)
                return;

            int iRe = 0;

            if (dataGridViewGJSPZD.Rows[e.RowIndex].IsNewRow)
                return;

            if (!bCheck)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);
            switch (e.ColumnIndex)
            {
                case 2: //��Ʒ���
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[17].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[18].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[19].Value = Math.Round(Decimal.Zero, 0);


                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2); 
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                        break;

                    }

                    iRe = cGetInformation.getCommInformation(20, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewGJSPZD.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewGJSPZD.CancelEdit();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].ErrorText = "��Ʒ����������";
                    }
                    else
                    {

                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                            break;
                        }
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = cGetInformation.decCommZZJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[17].Value = cGetInformation.decCommZGJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[18].Value = cGetInformation.decCommZDJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[19].Value = cGetInformation.decCommKCSL.ToString("f0");

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                    }
                    break;

                case 1: //��Ʒ����
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[17].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[18].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[9].Value = Math.Round(Decimal.Zero, 0);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;


                        break;

                    }


                    iRe = cGetInformation.getCommInformation(10, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewGJSPZD.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //ʧ��
                    {
                        e.Cancel = true;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].ErrorText = "��Ʒ�������������";
                    }
                    else
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                            break;
                        }
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = cGetInformation.decCommZZJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[17].Value = cGetInformation.decCommZGJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[18].Value = cGetInformation.decCommZDJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[19].Value = cGetInformation.decCommKCSL.ToString("f0");


                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                    }
                    break;
                case 5: //�ⷿ���
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = 0;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = "";
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(10, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewGJSPZD.CancelEdit();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].ErrorText = "�ⷿ����������";
                    }
                    else
                    {

                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                            break;
                        }
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                    }
                    break;
                case 6: //�ⷿ����
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = 0;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = "";
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(20, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewGJSPZD.CancelEdit();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].ErrorText = "�ⷿ�������������";
                    }
                    else
                    {

                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                            break;
                        }
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                    }

                    break;
                case 7:  //��Ʒ����
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut <= 0)
                        {
                            dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].ErrorText = "��Ʒ�����������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].ErrorText = "��Ʒ�����������ʹ���";
                        e.Cancel = true;
                    }
                    break;
                case 8: //��Ʒ�۸�
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;
                    if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value.ToString() == "")
                    {
                        MessageBox.Show("�������빺����Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                        break;
                    }

                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        if (detOut >= 0)
                        {
                            detOut = Math.Round(detOut, 2);

                            if (detOut.CompareTo(dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value) > 0)
                            {
                                if (MessageBox.Show("��Ʒ�۸�������ս��ۣ��Ƿ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                    e.Cancel = true;
                                else
                                {
                                    this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
                                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].Value = detOut;
                                    this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                                }

                            }
                        }
                        else
                        {
                            dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].ErrorText = "��Ʒ�۸��������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].ErrorText = "��Ʒ�����۸����ʹ���";
                        e.Cancel = true;
                    }
                    break;
                case 11:  //����
                    double dOut = 0.0;
                    if (e.FormattedValue.ToString() == "")
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[11].Value = 100;
                        break;
                    }
                    if (Double.TryParse(e.FormattedValue.ToString(), out dOut))
                    {
                        if (dOut <0 || dOut > 100.0)
                        {
                            dataGridViewGJSPZD.Rows[e.RowIndex].Cells[11].ErrorText = "��Ʒ�����������������0-100.0֮�������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[11].ErrorText = "��Ʒ�����������������0-100.0֮�������";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;



            }
            dataGridViewGJSPZD.EndEdit();

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
                                    case 5:
                                    case 6: 
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    case 7:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                        break;
                                    case 8:
                                        //dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[11];
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[1];
                                        break;
                                    case 11:
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

        private void dataGridViewGJSPZD_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {

            countAmount();
        }

        private bool countAmount()
        {
            decimal fSum = 0, fSum1 = 0; 
            decimal fTemp, fTemp1;
            decimal fCount = 0,fCSum=0;
            bool bCheck = true;

            this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
            cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);

            for (int i = 0; i < dataGridViewGJSPZD.Rows.Count; i++)
            {
                if (dataGridViewGJSPZD.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() == "" || dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() == "0")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[1].ErrorText = "����������Ʒ";
                    dataGridViewGJSPZD.Rows[i].Cells[2].ErrorText = "����������Ʒ";
                    bCheck = false;
                }

                if (dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[2].ErrorText = "����������Ʒ";
                    bCheck = false;
                }
                if (dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[8].ErrorText = "����������Ʒ�۸�";
                    bCheck = false;
                }
                if (dataGridViewGJSPZD.Rows[i].Cells[11].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[11].Value = 100;
                }

                //��ߵͽ���
                if (dataGridViewGJSPZD.Rows[i].Cells[17].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[17].Value = 0;
                }
                if (dataGridViewGJSPZD.Rows[i].Cells[18].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[18].Value = 0;
                }

                if (!bCheck)
                    continue;


                //����
                if (dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() == "")
                    fTemp = 0;
                else
                    fTemp = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[7].Value);
                fCSum += fTemp;

                //����
                if (dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[8].Value);

                if (dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() != "" && dataGridViewGJSPZD.Rows[i].Cells[15].Value.ToString()!="")
                {
                    if (Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[8].Value) > Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[15].Value))
                        dataGridViewGJSPZD.Rows[i].Cells[8].Style.BackColor = Color.LightPink;
                    else
                        dataGridViewGJSPZD.Rows[i].Cells[8].Style.BackColor = Color.White;
                }

                //���
                dataGridViewGJSPZD.Rows[i].Cells[9].Value = Math.Round(fTemp * fTemp1, 2);

                //����
                if (dataGridViewGJSPZD.Rows[i].Cells[11].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[11].Value = 100;
                }

                //
                if (dataGridViewGJSPZD.Rows[i].Cells[10].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[10].Value = 0;
                }

                //��Ʒ
                /*
                if (Convert.ToBoolean(dataGridViewGJSPZD.Rows[i].Cells[10].Value))
                {
                    dataGridViewGJSPZD.Rows[i].Cells[11].Value = 0.0;
                }
                 */
                fTemp = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[11].Value);

                //ʵ�ƽ��
                fTemp1 = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[9].Value);

                if (!Convert.ToBoolean(dataGridViewGJSPZD.Rows[i].Cells[10].Value)) //��Ʒ
                    dataGridViewGJSPZD.Rows[i].Cells[12].Value = fTemp * fTemp1/100;
                else
                    dataGridViewGJSPZD.Rows[i].Cells[12].Value = 0;


                fSum += Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[9].Value);
                fSum1 += Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[12].Value);

                fCount += 1;
                
            }
            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
            dataGridViewGJSPZD.EndEdit();

            labelSLHJ.Text = fCSum.ToString("f0");
            labelJEHJ.Text = fSum.ToString("f2");
            labelSJJE.Text = fSum1.ToString("f2");

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;


        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i;
            decimal fZGJJ=0, fZDJJ=0;

            //�������
            if (isSaved)
            {
                MessageBox.Show("������Ʒ�Ƶ��Ѿ�����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("��ѡ��λ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("û�й�����Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("������Ʒ�Ƶ���ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //if (MessageBox.Show("���鹺����Ʒ�Ƶ����ݣ��Ƿ�������棿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            //    return;

            saveToolStripButton.Enabled = false;

            string strCount = "",strDateSYS="",strKey="AKP";
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
                sqlComm.CommandText = "SELECT ���� FROM �������� WHERE (�ؼ��� = N'"+strKey+"')";
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

                //������
                string sBMID = "NULL";
                if (comboBoxBM.SelectedValue.ToString() != "0")
                    sBMID = comboBoxBM.SelectedValue.ToString();

                sqlComm.CommandText = "INSERT INTO ������Ʒ�Ƶ��� (���ݱ��, ��λID, ����, ��˰�ϼ�, ҵ��ԱID, ����ԱID, ���ʽ, ��Ʊ��, ��ͬID, ��ע, �����, BeActive, ����ID) VALUES (N'" + strCount + "', " + iSupplyCompany.ToString() + ", '" + strDateSYS + "', "+labelSJJE.Text+", "+comboBoxYWY.SelectedValue.ToString()+", "+intUserID.ToString()+", N'"+comboBoxZFFS.Text.Trim()+"', N'"+textBoxFPH.Text.Trim()+"', "+intHTH.ToString()+", N'"+textBoxBZ.Text.Trim()+"', 0, 1, "+sBMID+")";
                sqlComm.ExecuteNonQuery();

                //ȡ�õ��ݺ� 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //��غ�ͬ����
                if (intHTH != 0)
                {
                    sqlComm.CommandText = "UPDATE �ɹ���ͬ�� SET ִ�б�� = 1 WHERE (ID = " + intHTH.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                //��λ��ʷ��¼
                sqlComm.CommandText = "INSERT INTO ��λ��ʷ�˱� (��λID, ����, ���ݱ��, ժҪ, ����δ�����, ҵ��ԱID, ��ֵ���, BeActive) VALUES ( " + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + strCount + "', N'������Ʒ�Ƶ�', " + labelSJJE.Text + ", "+comboBoxYWY.SelectedValue.ToString()+", N'"+textBoxHTH.Text+"', 1)";
                sqlComm.ExecuteNonQuery();


                //������ϸ
                string strTemp = "";
                for (i = 0; i < dataGridViewGJSPZD.Rows.Count; i++)
                {
                    if (dataGridViewGJSPZD.Rows[i].IsNewRow)
                        continue;

                    strTemp = "";
                    if (dataGridViewGJSPZD.Rows[i].Cells[14].Value.ToString() == "")
                        strTemp = "NULL";
                    else
                        strTemp = dataGridViewGJSPZD.Rows[i].Cells[14].Value.ToString();

                    sqlComm.CommandText = "INSERT INTO ������Ʒ�Ƶ���ϸ�� (��ID, ��ƷID, �ⷿID, ����, ����, ���, ��Ʒ, ����, ʵ�ƽ��, δ��������) VALUES (" + sBillNo + ", " + dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() + ", " + strTemp + ", " + dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[9].Value.ToString() + ", " + Convert.ToInt32(dataGridViewGJSPZD.Rows[i].Cells[10].Value).ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[11].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[12].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() + ")";
                   sqlComm.ExecuteNonQuery();

                    //��Ʒ�ⷿ��ʷ��
                   sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (����, ��ƷID, ��λID, �ⷿID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ��������, �������, BeActive, ����ID) VALUES ('" + strDateSYS + "', " + dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[14].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'������Ʒ�Ƶ�', " + dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[9].Value.ToString() + ", 1, " + sBMID + ")";
                   sqlComm.ExecuteNonQuery();

                   //��Ʒ��ʷ��
                   sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ��������, �������, BeActive, ����ID) VALUES ('" + strDateSYS + "', " + dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'������Ʒ�Ƶ�', " + dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[9].Value.ToString() + ", 1, " + sBMID + ")";
                   sqlComm.ExecuteNonQuery();

                  //��Ʒ���۸���
                    fZGJJ=decimal.Parse(dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString());
                    if (fZGJJ < decimal.Parse(dataGridViewGJSPZD.Rows[i].Cells[17].Value.ToString()))
                        fZGJJ = decimal.Parse(dataGridViewGJSPZD.Rows[i].Cells[17].Value.ToString());

                    fZDJJ = decimal.Parse(dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString());
                    if (fZDJJ > decimal.Parse(dataGridViewGJSPZD.Rows[i].Cells[18].Value.ToString()))
                        fZDJJ = decimal.Parse(dataGridViewGJSPZD.Rows[i].Cells[18].Value.ToString());

                    sqlComm.CommandText = "UPDATE ��Ʒ�� SET ��߽��� = " + fZGJJ.ToString("f2") + ", ��ͽ��� = " + fZDJJ.ToString("f2") + ", ���ս��� = " + dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() + " WHERE (ID = " + dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() + ")";
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

            labelDJBH.Text = strCount;
            this.Text = "������Ʒ�Ƶ���" + labelDJBH.Text;
            isSaved = true;

            bool bClose = false;
            //if (MessageBox.Show("������Ʒ�Ƶ�����ɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //{
                bClose = true;
            //}

            if (MessageBox.Show("�Ƿ������ʼ��һ���Ƶ���", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                // �������Ӵ����һ����ʵ����
                FormGJSPZD childFormGJSPZD = new FormGJSPZD();
                // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                childFormGJSPZD.MdiParent = this.MdiParent; 

                childFormGJSPZD.strConn = strConn;

                childFormGJSPZD.intUserID = intUserID;
                childFormGJSPZD.intUserLimit = intUserLimit;
                childFormGJSPZD.strUserLimit = strUserLimit;
                childFormGJSPZD.strUserName = strUserName;
                childFormGJSPZD.Show();
            }


            if (bClose)
                this.Close();

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("������Ʒ�Ƶ���ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "������Ʒ�Ƶ�(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelSJJE.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewGJSPZD, strT, false, intUserLimit);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("������Ʒ�Ƶ���ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "������Ʒ�Ƶ�(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelSJJE.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewGJSPZD, strT, true, intUserLimit);

        }

        private void FormGJSPZD_FormClosing(object sender, FormClosingEventArgs e)
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

        private void textBoxHTH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getBillInformation(510, textBoxHTH.Text.Trim()) == 0)
                {
                    textBoxHTH.Text = "";
                    intHTH = 0;
                    return;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iBillCNumber;
                    textBoxDWMC.Text = cGetInformation.strBillCName;
                    textBoxDWBH.Text = cGetInformation.strBillCCode;
                    textBoxHTH.Text = cGetInformation.strBillCode;
                    intHTH = cGetInformation.iBillNumber;
                    comboBoxBM.SelectedValue = cGetInformation.iBillBMID;
                    comboBoxYWY.Text = cGetInformation.sPeopleName;

                    getHTDetail();
                    dataGridViewGJSPZD.Focus();
                }

            }

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

        private void dataGridViewGJSPZD_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex != 8 || e.RowIndex < 0)
                return;


            if (Convert.ToDecimal(dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].Value) > Convert.ToDecimal(dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value))
            {
                e.CellStyle.BackColor = Color.LightPink;
            }


        }






    }
}