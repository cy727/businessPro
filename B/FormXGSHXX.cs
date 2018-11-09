using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace business
{
    public partial class FormXGSHXX : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();
        private System.Data.DataSet dSetP1 = new DataSet();

        public string strConn = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        public string strSelect = "";

        private int iSupplyCompany = 0;
        private int intHTH = 0;
        private int intBKP = 0;

        private ClassGetInformation cGetInformation;

        private bool isSaved = false;

        private int RowPos;              // Position of currently printing row 
        private bool NewPage;            // Indicates if a new page reached

        private string sGSMC = "";
        private string sGSDZ = "";
        private string sGSDH = "";
        private string sGSCZ = "";
        private string sGSYB = "";
        private string sGSZH = "";
        private string sGSKHYH = "";
        private string sGSSH = "";

        private const int iPageZX = 20; //װ�䵥����
        private const int iPageNZX = 10;
        private int PageNo;

        public int LIMITACCESS = 18;

        public FormXGSHXX()
        {
            InitializeComponent();
        }

        private void FormXGSHXX_Load(object sender, EventArgs e)
        {
            int i;

            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            string dFileName = Directory.GetCurrentDirectory() + "\\print1.xml";


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
            }
            sqldr.Close();
;

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
            comboBoxBM.SelectedValue = intUserBM;;
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;


            sqlConn.Close();

            //initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

        }

        private void comboBoxBM_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
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
             */

        }

        private void textBoxHTH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getBillInformation(211, "") == 0)
            //if (cGetInformation.getBillInformation(2, "") == 0)
            {
                return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iBillCNumber;
                textBoxDWMC.Text = cGetInformation.strBillCName;
                textBoxDWBH.Text = cGetInformation.strBillCCode;
                textBoxHTH.Text = cGetInformation.strBillCode;
                intBKP = cGetInformation.iBillNumber;
                comboBoxBM.SelectedValue = cGetInformation.iBillBMID;
                comboBoxYWY.Text = cGetInformation.sPeopleName;

                //strSelect = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ,�ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.δ�������� AS ��������,  ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.δ��������, ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ��.�ⷿID, ������Ʒ�Ƶ���ϸ��.ID AS ��ϸID, ������Ʒ�Ƶ���ϸ��.��Ʒ, ������Ʒ�Ƶ���.ID AS ����ID, ������Ʒ�Ƶ���ϸ��.ë�� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN ������Ʒ�Ƶ���ϸ����� WHERE (������Ʒ�Ƶ���ϸ��.δ�������� > 0) AND (������Ʒ�Ƶ���.ID = " + cGetInformation.iBillNumber + ") ORDER BY ������Ʒ�Ƶ���.���ݱ��";

                strSelect = "SELECT ������Ʒ�Ƶ���ϸ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.���ɱ���, ������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ��� AS �ɱ����, ������Ʒ�Ƶ���ϸ��.ë��, ������Ʒ�Ƶ���ϸ��.��Ʒ, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ��.�ⷿID, ��Ʒ��.�������, �ⷿ��.ID AS ͳ�Ʊ�� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID WHERE (������Ʒ�Ƶ���ϸ��.��ID = " + cGetInformation.iBillNumber + ")";


                initDJDtail();
                initdataGridViewDJMX();
            }
        }

        private void initDJDtail()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.��λID, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��ϵ�绰, ������Ʒ�Ƶ���.��ϵ��, ������Ʒ�Ƶ���.�ջ���, ������Ʒ�Ƶ���.��վ, ������Ʒ�Ƶ���.���䷽ʽ, ������Ʒ�Ƶ���.��ϸ��ַ, ������Ʒ�Ƶ���.��������, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���.��������, ְԱ��.ְԱ���� AS ҵ��Ա, [ְԱ��_1].ְԱ���� AS ����Ա, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.��ע FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ������Ʒ�Ƶ���.����ԱID = [ְԱ��_1].ID WHERE (������Ʒ�Ƶ���.ID = " + intBKP.ToString() + ")";

            if (dSet.Tables.Contains("�ͻ���")) dSet.Tables.Remove("�ͻ���");
            sqlDA.Fill(dSet, "�ͻ���");

            if (dSet.Tables["�ͻ���"].Rows.Count < 1)
            {
                textBoxLXDH.Text = "";
                textBoxLXR.Text = "";
                textBoxSHR.Text = "";
                textBoxDZ.Text = "";
                comboBoxYSFS.Text = "";
                textBoxXXDZ.Text = "";
                textBoxWLMC.Text = "";
                textBoxDH.Text = "";
                textBoxYZBM.Text = "";
                labelYYWY.Text = "";
                labelYCZY.Text = "";
                labelYDJBH.Text = "";
                textBoxBZ.Text = "";
                intBKP = 0;
            }
            else
            {
                textBoxLXDH.Text = dSet.Tables["�ͻ���"].Rows[0][3].ToString();
                textBoxLXR.Text = dSet.Tables["�ͻ���"].Rows[0][4].ToString();
                textBoxSHR.Text = dSet.Tables["�ͻ���"].Rows[0][5].ToString();
                textBoxDZ.Text = dSet.Tables["�ͻ���"].Rows[0][6].ToString();
                comboBoxYSFS.Text = dSet.Tables["�ͻ���"].Rows[0][7].ToString();
                textBoxXXDZ.Text = dSet.Tables["�ͻ���"].Rows[0][8].ToString();
                textBoxWLMC.Text = dSet.Tables["�ͻ���"].Rows[0][9].ToString();
                textBoxDH.Text = dSet.Tables["�ͻ���"].Rows[0][10].ToString();
                textBoxYZBM.Text = dSet.Tables["�ͻ���"].Rows[0][11].ToString();
                labelYYWY.Text = dSet.Tables["�ͻ���"].Rows[0][12].ToString();
                labelYCZY.Text = dSet.Tables["�ͻ���"].Rows[0][13].ToString();
                labelYDJBH.Text = dSet.Tables["�ͻ���"].Rows[0][14].ToString();
                textBoxBZ.Text = dSet.Tables["�ͻ���"].Rows[0][15].ToString();
                comboBoxYWY.Text = dSet.Tables["�ͻ���"].Rows[0][12].ToString();

            }
            sqlConn.Close();
        }

        private void initdataGridViewDJMX()
        {
            if (strSelect == "") return;
            sqlConn.Open();
            sqlComm.CommandText = strSelect;

            if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
            sqlDA.Fill(dSet, "���ݱ�");
            dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];

            sqlConn.Close();

            /*
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[16].Visible = false;

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
            dataGridViewDJMX.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[15].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            */

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[15].Visible = false;
            dataGridViewDJMX.Columns[16].Visible = false;
            dataGridViewDJMX.Columns[18].Visible = false;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
            countAmount();

            if (intUserLimit < LIMITACCESS)
            {
                dataGridViewDJMX.Columns[9].Visible = false;
                dataGridViewDJMX.Columns[10].Visible = false;
                dataGridViewDJMX.Columns[11].Visible = false;
            }


            //dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            //dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            //dataGridViewDJMX.CellDoubleClick -= dataGridViewDJMX_CellDoubleClick;


        }

        private void textBoxHTH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                //if (cGetInformation.getBillInformation(20, textBoxHTH.Text.Trim()) == 0)
                if (cGetInformation.getBillInformation(2011, textBoxHTH.Text.Trim()) == 0)
                {
                    return;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iBillCNumber;
                    textBoxDWMC.Text = cGetInformation.strBillCName;
                    textBoxDWBH.Text = cGetInformation.strBillCCode;
                    textBoxHTH.Text = cGetInformation.strBillCode;
                    intBKP = cGetInformation.iBillNumber;
                    comboBoxBM.SelectedValue = cGetInformation.iBillBMID;
                    comboBoxYWY.Text = cGetInformation.sPeopleName;

                    //strSelect = "SELECT ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ,�ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.δ�������� AS ��������,  ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.δ��������, ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ��.�ⷿID, ������Ʒ�Ƶ���ϸ��.ID AS ��ϸID, ������Ʒ�Ƶ���ϸ��.��Ʒ, ������Ʒ�Ƶ���.ID AS ����ID, ������Ʒ�Ƶ���ϸ��.ë�� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN ������Ʒ�Ƶ���ϸ����� WHERE (������Ʒ�Ƶ���ϸ��.δ�������� > 0) AND (������Ʒ�Ƶ���.ID = " + cGetInformation.iBillNumber + ") ORDER BY ������Ʒ�Ƶ���.���ݱ��";
                    strSelect = "SELECT ������Ʒ�Ƶ���ϸ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.���ɱ���, ������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ��� AS �ɱ����, ������Ʒ�Ƶ���ϸ��.ë��, ������Ʒ�Ƶ���ϸ��.��Ʒ, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ��.�ⷿID, ��Ʒ��.�������, �ⷿ��.ID AS ͳ�Ʊ�� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID WHERE (������Ʒ�Ƶ���ϸ��.��ID = " + cGetInformation.iBillNumber + ")";

                    initDJDtail();
                    initdataGridViewDJMX();
                }
            }
        }

        private void countAmount()
        {
            decimal fSum = 0, fSum1 = 0;
            decimal fCount = 0, fCSum = 0;

            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[14].Value);

                fCount += 1;

            }
            labelSLHJ.Text = fCSum.ToString("f0");
            labelJEHJ.Text = fSum.ToString("f2");
            labelSJJE.Text = fSum1.ToString("f2");

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();


        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {

            //�������
            if (isSaved)
            {
                MessageBox.Show("�ͻ���Ϣ�Ѿ�����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intBKP == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //if (MessageBox.Show("�����ͻ���ϢУ�Ե�����,���Ƶ����ݲ��ɸ��ģ��Ƿ�������棿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            //    return;
            saveToolStripButton.Enabled = false;
            string strCount = "", strDateSYS = "", strKey = "ZXG";
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
                sqlComm.CommandText = "INSERT INTO �ͻ���Ϣ�޸ı� (���ݱ��, �޸ĵ���ID, ����, ҵ��ԱID, ����ԱID, ԭ��ע, ԭ��ϵ�绰, ԭ��ϵ��, ԭ�ջ���, ԭ��վ, ԭ���䷽ʽ, ԭ��ϸ��ַ, ԭ��������, ԭ����, ԭ��������, BeActive, ��ע, ��ϵ�绰, ��ϵ��, �ջ���, ��վ, ���䷽ʽ, ��ϸ��ַ, ��������, ����, ��������) VALUES (N'" + strCount + "', " + intBKP.ToString() + " , '" + strDateSYS + "', " + intUserID.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + dSet.Tables["�ͻ���"].Rows[0][15].ToString() + "', N'" + dSet.Tables["�ͻ���"].Rows[0][3].ToString() + "', N'" + dSet.Tables["�ͻ���"].Rows[0][4].ToString() + "', N'" + dSet.Tables["�ͻ���"].Rows[0][5].ToString() + "', N'" + dSet.Tables["�ͻ���"].Rows[0][6].ToString() + "', N'" + dSet.Tables["�ͻ���"].Rows[0][7].ToString() + "', N'" + dSet.Tables["�ͻ���"].Rows[0][8].ToString() + "', N'" + dSet.Tables["�ͻ���"].Rows[0][9].ToString() + "', N'" + dSet.Tables["�ͻ���"].Rows[0][10].ToString() + "', N'" + dSet.Tables["�ͻ���"].Rows[0][11].ToString() + "', 1, N'" + textBoxBZ.Text.Trim() + "', N'" + textBoxLXDH.Text.Trim() + "', N'" + textBoxLXR.Text.Trim() + "', N'" + textBoxSHR.Text.Trim() + "', N'" + textBoxDZ.Text.Trim() + "', N'" + comboBoxYSFS.Text.Trim() + "', N'" + textBoxXXDZ.Text.Trim() + "', N'" + textBoxWLMC.Text.Trim() + "', N'" + textBoxDH.Text.Trim() + "', N'" + textBoxYZBM.Text.Trim() + "')";
                sqlComm.ExecuteNonQuery();

                //��Ϣ�޸�
                sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET ��ע = N'" + textBoxBZ.Text.Trim() + "', ��ϵ�绰 = N'" + textBoxLXDH.Text.Trim() + "', ��ϵ�� = N'" + textBoxLXR.Text.Trim() + "', �ջ��� = N'" + textBoxSHR.Text.Trim() + "', ��վ = N'" + textBoxDZ.Text.Trim() + "', ���䷽ʽ = N'" + comboBoxYSFS.Text.Trim() + "', ��ϸ��ַ = N'" + textBoxXXDZ.Text.Trim() + "', �������� = N'" + textBoxWLMC.Text.Trim() + "', ���� = N'" + textBoxDH.Text.Trim() + "', �������� = N'" + textBoxYZBM.Text.Trim() + "' WHERE (ID = "+intBKP.ToString()+")";
                sqlComm.ExecuteNonQuery();

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

            //MessageBox.Show("�ͻ���ϢУ�Ե�����ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = "�ͻ���ϢУ�Ե���" + labelDJBH.Text;
            isSaved = true;

            bool bClose = false;
            //if (MessageBox.Show("�ͻ���ϢУ�Ե�����ɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //{
                bClose = true;
            //}

            if (MessageBox.Show("�Ƿ������ʼ��һ�ݵ��ݣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
                mdiT.�޸��ͻ���ϢDToolStripMenuItem_Click(null, null);
            }

            if (bClose)
                this.Close();
        }

        private void FormXGSHXX_FormClosing(object sender, FormClosingEventArgs e)
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
            string strT = "���۳����Ƶ�(���ݱ��:" + labelYDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelSJJE.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "���۳����Ƶ�(���ݱ��:" + labelYDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelSJJE.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void toolStripButtonPrnFHD_Click(object sender, EventArgs e)
        {
            try
            {

                if (ppd.ShowDialog() != DialogResult.OK)
                    return;

                // Showing the Print Preview Page
                printDoc.BeginPrint += PrintDoc_BeginPrintFHD;
                printDoc.PrintPage += PrintDoc_PrintPageFHD;

                // Printing the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= PrintDoc_BeginPrintFHD;
                printDoc.PrintPage -= PrintDoc_PrintPageFHD;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��ӡʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void toolStripButtonPrnZXD_Click(object sender, EventArgs e)
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

        private void PrintDoc_BeginPrintFHD(object sender, System.Drawing.Printing.PrintEventArgs e)
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

        private void PrintDoc_PrintPageFHD(object sender, System.Drawing.Printing.PrintPageEventArgs e)
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


            System.Drawing.Font _Font22 = new System.Drawing.Font("����", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12 = new System.Drawing.Font("����", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9 = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12U = new System.Drawing.Font("����", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            int iTopM = 90;
            int iLeftM = 160;
            int iWidth1 = 170;
            int iWidth2 = 110;
            int iWidth3 = 100;
            int iWidth4 = 90;
            int iWidth5 = 180;

            int iHeight = 45;

            if (dSetP1.Tables.Contains("PRN1"))
            {
                iTopM = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][0].ToString());
                iLeftM = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][1].ToString());
                iWidth1 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][2].ToString());
                iWidth2 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][3].ToString());
                iWidth3 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][4].ToString());
                iWidth4 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][5].ToString());
                iWidth5 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][6].ToString());

                iHeight = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][7].ToString());
            }


            Brush b = new SolidBrush(Color.Black);

            try
            {
                //������ʽ
                e.Graphics.DrawString(comboBoxYSFS.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM, iWidth1, iHeight), StrFormatL);

                //��վ
                e.Graphics.DrawString(textBoxDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2, iTopM, iWidth3, iHeight), StrFormatL);

                //����ʱ��
                e.Graphics.DrawString(labelZDRQ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2 + iWidth3 + iWidth4, iTopM, iWidth5, iHeight), StrFormatL);
                //e.Graphics.DrawString(labelZDRQ.Text, _Font12, b, (decimal)(iLeftM + iWidth1 + iWidth2 + iWidth3 + iWidth4), (decimal)(iTopM), StrFormatL);

                //�ջ���λ
                e.Graphics.DrawString(textBoxDWMC.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM + iHeight, iWidth1 + iWidth2 + iWidth3 + iWidth4 + iWidth5, iHeight), StrFormatL);

                //�ջ���ַ
                e.Graphics.DrawString(textBoxXXDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM + 2 * iHeight, iWidth1 + iWidth2 + iWidth3, iHeight), StrFormatL);

                //�ʱ�
                e.Graphics.DrawString(textBoxYZBM.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2 + iWidth3 + iWidth4, iTopM + 2 * iHeight, iWidth5, iHeight), StrFormatL);
                //e.Graphics.DrawString(textBoxYZBM.Text, _Font12, b, (decimal)(iLeftM + iWidth1 + iWidth2 + iWidth3 + iWidth4), (decimal)(iTopM + 2 * iHeight), StrFormatL);

                //�ջ���
                e.Graphics.DrawString(textBoxSHR.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM + 3 * iHeight, iWidth1, iHeight), StrFormatL);


                //��ϵ�绰
                e.Graphics.DrawString(textBoxLXDH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2, iTopM + 3 * iHeight, iWidth3 + iWidth4 + iWidth5, iHeight), StrFormatL);


                //������
                e.Graphics.DrawString(comboBoxYWY.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM + 6 * iHeight + 30, iWidth3 + iWidth4 + iWidth5, iHeight), StrFormatL);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��ӡʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            int i, j;
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


            System.Drawing.Font _Font22 = new System.Drawing.Font("����", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12 = new System.Drawing.Font("����", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9 = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12U = new System.Drawing.Font("����", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            int iTopM1 = 100;
            int iLeftM1 = 80;
            int iHeight1 = 22;
            int iWidth1 = 700;
            int iWidth2 = 500;


            int iTopM = 218;
            int iLeftM = 95;
            int iLeftM2 = 545;
            int iWidth01 = 220;
            int iWidth02 = 120;
            int iWidth03 = 30;
            int iHeight2 = 40;


            int iHeight12 = 22;
            int iHeight22 = 50;
            int iHeight9 = 17;

            int iLM1 = 60;
            int iLM2 = 460;
            int iLM3 = 710;

            int iX1 = 430;
            int iY1 = 580;
            int iX2 = 200;
            int iY2 = 22;
            int iX3 = 760;

            if (dSetP1.Tables.Contains("PRN2"))
            {

                iTopM1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][0].ToString());
                iLeftM1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][1].ToString());
                iHeight1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][2].ToString());
                iWidth1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][3].ToString());
                iWidth2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][4].ToString());

                iTopM = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][5].ToString());
                iLeftM = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][6].ToString());
                iLeftM2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][7].ToString());
                iWidth01 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][8].ToString());
                iWidth02 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][9].ToString());
                iWidth03 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][10].ToString());
                iHeight2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][11].ToString());


                iHeight12 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][12].ToString());
                iHeight22 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][13].ToString());
                iHeight9 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][14].ToString());

                iLM1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][15].ToString());
                iLM2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][16].ToString());
                iLM3 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][17].ToString());

                iX1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][18].ToString());
                iY1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][19].ToString());
                iX2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][20].ToString());
                iY2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][21].ToString());
                iX3 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][22].ToString());


            }





            //decimal fTemp = 0;
            int iTemp = 0;

            Brush b = new SolidBrush(Color.Black);


            try
            {
                e.Graphics.DrawString("���ݱ�ţ�" + labelDJBH.Text + "��", _Font12, b, new System.Drawing.RectangleF(iLeftM1, iTopM1, iWidth1, iHeight1), StrFormatL);

                e.Graphics.DrawString(textBoxDWMC.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM1, iTopM1 + iHeight1, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(textBoxSHR.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM2, iTopM1 + iHeight1, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(textBoxLXDH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM3, iTopM1 + iHeight1, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(textBoxXXDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM1, iTopM1 + iHeight1 * 2, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(comboBoxYSFS.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM2, iTopM1 + iHeight1 * 2, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(textBoxDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM3, iTopM1 + iHeight1 * 2, iWidth2, iHeight1 * 2), StrFormatL);
                //e.Graphics.DrawString(comboBoxYSFS.Text, _Font12, b,(decimal)(iLeftM1 + iLM2), (decimal)(iTopM1 + iHeight1 * 2),StrFormatL);
                //e.Graphics.DrawString(textBoxDZ.Text, _Font12, b, (decimal)(iLeftM1 + iLM3), (decimal)(iTopM1 + iHeight1 * 2), StrFormatL);

                for (i = 0; i < 20; i++)
                {
                    if (RowPos >= dataGridViewDJMX.Rows.Count)
                    {
                        NewPage = false;
                        break;
                    }

                    if (dataGridViewDJMX.Rows[RowPos].IsNewRow)
                    {
                        NewPage = false;
                        break;
                    }

                    if (i < 10)
                    {
                        j = i;
                        iTemp = iLeftM;
                    }
                    else
                    {
                        j = i - 10;
                        iTemp = iLeftM2;
                    }

                    e.Graphics.DrawString(dataGridViewDJMX.Rows[RowPos].Cells[1].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iTemp, iTopM + j * iHeight2, iWidth01, iHeight2), StrFormatL);

                    e.Graphics.DrawString(Convert.ToDecimal(dataGridViewDJMX.Rows[RowPos].Cells[6].Value.ToString()).ToString("f0"), _Font12, b, new System.Drawing.RectangleF(iTemp + iWidth01, iTopM + j * iHeight2, iWidth02, iHeight2), StrFormatL);


                    RowPos++;
                }

                e.Graphics.DrawString(labelZDRQ.Text, _Font12, b, new System.Drawing.RectangleF(iX1, iY1, iX2, iY2), StrFormatL);
                e.Graphics.DrawString(labelZDRQ.Text, _Font12, b, new System.Drawing.RectangleF(iX3, iY1, iX2, iY2), StrFormatL);



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


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��ӡʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripButtonZXDNew_Click(object sender, EventArgs e)
        {

            try
            {

                if (ppd.ShowDialog() != DialogResult.OK)
                    return;
                //printDoc.DefaultPageSettings.PaperSize = printDoc.PrinterSettings.PaperSizes[2]; 
                foreach (System.Drawing.Printing.PaperSize ps in printDoc.PrinterSettings.PaperSizes)
                {
                    if (ps.PaperName == "A3")
                    {
                        printDoc.PrinterSettings.DefaultPageSettings.PaperSize = ps;
                        printDoc.DefaultPageSettings.PaperSize = ps;
                    }
                }
                // Showing the Print Preview Page
                printDoc.BeginPrint += PrintDoc_BeginPrintN;
                printDoc.PrintPage += PrintDoc_PrintPageN;

                ppw.Width = 1000;
                ppw.Height = 800;


                //if (ppw.ShowDialog() != DialogResult.OK)
                //{
                //    printDoc.BeginPrint -= PrintDoc_BeginPrint;
                //    printDoc.PrintPage -= PrintDoc_PrintPage;
                //    return;
                //}



                // Printing the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= PrintDoc_BeginPrintN;
                printDoc.PrintPage -= PrintDoc_PrintPageN;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��ӡʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void PrintDoc_BeginPrintN(object sender, System.Drawing.Printing.PrintEventArgs e)
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

        private void PrintDoc_PrintPageN(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            int iyRow;
            int iyRow1;
            int i, j;
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


            System.Drawing.Font _Font22 = new System.Drawing.Font("����", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12 = new System.Drawing.Font("����", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9 = new System.Drawing.Font("����", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9I = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12U = new System.Drawing.Font("����", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            //e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(0, 0, 870, 500));

            int iTopM = 10;
            int iLeftM = 10;
            int iLeftM1 = 100;
            int iWidth1 = 30;
            int iWidth2 = 200;
            int iWidth3 = 50;


            int iHeight12 = 22;
            int iHeight22 = 50;
            int iHeight9 = 17;
            int iHeight2 = 40;

            int iPaperWidth = 870;

            if (dSetP1.Tables.Contains("PRN3"))
            {

                iTopM = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][0].ToString());
                iLeftM = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][1].ToString());
                iLeftM1 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][2].ToString());

                iHeight2 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][3].ToString());
                iHeight12 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][4].ToString());
                iHeight22 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][5].ToString());
                iHeight9 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][6].ToString());

                iWidth1 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][7].ToString());
                iWidth2 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][8].ToString());
                iWidth3 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][9].ToString());

                iPaperWidth = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][10].ToString());
            }





            //decimal fTemp = 0;
            int iTemp = 0;
            int iTemp1 = 0;
            bool rTitle;

            Brush b = new SolidBrush(Color.Black);
            try
            {
                //e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM, iTopM, iPaperWidth - 2 * iLeftM, iHeight22));
                e.Graphics.DrawString(sGSMC + "���ⵥ", _Font22, b, new System.Drawing.RectangleF(iLeftM, iTopM, iPaperWidth - 2 * iLeftM, iHeight22), StrFormat);

                iyRow += iTopM + iHeight22;
                e.Graphics.DrawString("���ݱ�ţ�" + labelDJBH.Text + "��", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iPaperWidth - 2 * iLeftM, iHeight12), StrFormatR);

                iyRow += iHeight12;
                //e.Graphics.DrawString("�Ƶ����ڣ�" + labelZDRQ.Text + "", _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iPaperWidth - 2 * iLeftM, iHeight12), StrFormatR);
                //iyRow += iHeight9;

                e.Graphics.DrawString("ҵ����Ա:" + comboBoxYWY.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("�Ƶ����ڣ�" + labelZDRQ.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);
                iyRow += iHeight9;
                e.Graphics.DrawString("��λ����:" + textBoxDWMC.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("�ա�����:" + textBoxSHR.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);
                iyRow += iHeight9;
                e.Graphics.DrawString("�ջ���ַ:" + textBoxXXDZ.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("��ϵ�绰:" + textBoxLXDH.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);

                iyRow += iHeight9;
                e.Graphics.DrawString("���䷽ʽ:" + comboBoxYSFS.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("װ������:", _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);
                iyRow += iHeight9;

                //e.Graphics.DrawString(comboBoxYSFS.Text, _Font12, b,(decimal)(iLeftM1 + iLM2), (decimal)(iTopM1 + iHeight1 * 2),StrFormatL);
                //e.Graphics.DrawString(textBoxDZ.Text, _Font12, b, (decimal)(iLeftM1 + iLM3), (decimal)(iTopM1 + iHeight1 * 2), StrFormatL);

                //��ͷ
                e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM, iyRow, iWidth1, iHeight9 + 2));
                e.Graphics.DrawString("���", _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow + 2, iWidth1, iHeight9), StrFormat);
                e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + iWidth1, iyRow, iWidth2, iHeight9 + 2));
                e.Graphics.DrawString("��Ʒ�ͺ�", _Font9, b, new System.Drawing.RectangleF(iLeftM + iWidth1, iyRow + 2, iWidth2, iHeight9), StrFormat);
                e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + iWidth1 + iWidth2, iyRow, iWidth3, iHeight9 + 2));
                e.Graphics.DrawString("����", _Font9, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2, iyRow + 2, iWidth3, iHeight9), StrFormat);
                rTitle = false;
                if (!IsLastRow(RowPos))
                {
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + iWidth1 + iWidth2 + iWidth3, iyRow, iWidth1, iHeight9 + 2));
                    e.Graphics.DrawString("���", _Font9, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2 + iWidth3, iyRow + 2, iWidth1, iHeight9), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 2 * iWidth1 + iWidth2 + iWidth3, iyRow, iWidth2, iHeight9 + 2));
                    e.Graphics.DrawString("��Ʒ�ͺ�", _Font9, b, new System.Drawing.RectangleF(iLeftM + 2 * iWidth1 + iWidth2 + iWidth3, iyRow + 2, iWidth2, iHeight9), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 2 * iWidth1 + 2 * iWidth2 + iWidth3, iyRow, iWidth3, iHeight9 + 2));
                    e.Graphics.DrawString("����", _Font9, b, new System.Drawing.RectangleF(iLeftM + 2 * iWidth1 + 2 * iWidth2 + iWidth3, iyRow + 2, iWidth3, iHeight9), StrFormat);
                    rTitle = true;
                }
                iyRow += iHeight9 + 2;

                for (i = 0; i < iPageZX; i++)
                {
                    //���
                    if (RowPos >= dataGridViewDJMX.Rows.Count && dataGridViewDJMX.ReadOnly)
                    {
                        NewPage = false;

                        iTemp1 = iyRow + (i / 2) * iHeight9;

                        //�ұ��
                        if (i % 2 != 0 & rTitle)
                        {
                            iTemp = iLeftM + iWidth1 + iWidth2 + iWidth3;
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp, iTemp1, iWidth1, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1, iTemp1, iWidth2, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9));
                        }

                        break;
                    }

                    if (i % 2 == 0 && !dataGridViewDJMX.Rows[RowPos].IsNewRow)
                    {
                        iTemp = iLeftM;
                        iTemp1 = iyRow + (i / 2) * iHeight9;
                    }
                    else
                    {
                        iTemp = iLeftM + iWidth1 + iWidth2 + iWidth3;
                    }

                    if (RowPos >= dataGridViewDJMX.Rows.Count)
                    {
                        NewPage = false;

                        //�ұ��
                        if (i % 2 != 0 & rTitle)
                        {
                            iTemp = iLeftM + iWidth1 + iWidth2 + iWidth3;
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp, iTemp1, iWidth1, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1, iTemp1, iWidth2, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9));
                        }

                        break;
                    }

                    if (dataGridViewDJMX.Rows[RowPos].IsNewRow)
                    {
                        NewPage = false;

                        //�ұ��
                        if (i % 2 != 0 & rTitle)
                        {
                            iTemp = iLeftM + iWidth1 + iWidth2 + iWidth3;
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp, iTemp1, iWidth1, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1, iTemp1, iWidth2, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9));
                        }
                        break;
                    }

                    //���
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp, iTemp1, iWidth1, iHeight9));
                    e.Graphics.DrawString(((PageNo - 1) * iPageZX + i + 1).ToString(), _Font9I, b, new System.Drawing.RectangleF(iTemp, iTemp1, iWidth1, iHeight9), StrFormat);

                    //���
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1, iTemp1, iWidth2, iHeight9));
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9));




                    e.Graphics.DrawString(dataGridViewDJMX.Rows[RowPos].Cells[1].Value.ToString(), _Font9, b, new System.Drawing.RectangleF(iTemp + iWidth1, iTemp1, iWidth2, iHeight9), StrFormat);

                    e.Graphics.DrawString(Convert.ToDecimal(dataGridViewDJMX.Rows[RowPos].Cells[6].Value.ToString()).ToString("f0"), _Font9, b, new System.Drawing.RectangleF(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9), StrFormat);


                    if (IsLastRow(RowPos))
                    {
                        NewPage = false;
                    }
                    RowPos++;


                }

                //iyRow = iTemp1+iHeight9+10;
                iyRow += iHeight9 * iPageNZX + 10;
                //ҳ��
                e.Graphics.DrawString("��ϵ���ǣ�", _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iPaperWidth - 2 * iLeftM, iHeight9), StrFormatL);
                iyRow += iHeight9 + 5;
                e.Graphics.DrawString(sGSDZ, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("http://www.century-twinkle.com", _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);

                iyRow += iHeight9;

                e.Graphics.DrawString("�绰��" + sGSDH, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("���棺" + sGSCZ + " �ʱ�:" + sGSYB, _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);

                //
                iyRow += iHeight9;
                if (!dataGridViewDJMX.ReadOnly)
                    iTemp = (int)Math.Ceiling((decimal)(dataGridViewDJMX.Rows.Count - 1) / (decimal)(iPageZX));
                else
                    iTemp = (int)Math.Ceiling((decimal)(dataGridViewDJMX.Rows.Count) / (decimal)(iPageZX));

                //e.Graphics.DrawString(PageNo.ToString()+"\\" + iTemp.ToString(), _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iPaperWidth - 2 * iLeftM, iHeight9), StrFormatR);
                e.Graphics.DrawString(PageNo.ToString() + "/" + iTemp.ToString(), _Font22, b, new System.Drawing.RectangleF(iLeftM, iTopM, iPaperWidth - 2 * iLeftM, iHeight22), StrFormatR);






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


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��ӡʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool IsLastRow(int iRow)
        {
            if (!dataGridViewDJMX.ReadOnly || dataGridViewDJMX.AllowUserToAddRows) //������
            {
                if (iRow == dataGridViewDJMX.RowCount - 2)
                    return true;
                else
                    return false;
            }
            else
            {
                if (iRow == dataGridViewDJMX.RowCount - 1)
                    return true;
                else
                    return false;
            }
        }

    }
}