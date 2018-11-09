using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJHTBJDJ : Form
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

        private int iSupplyCompany = 0;

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;
        private bool bCheck = true;

        public FormJHTBJDJ()
        {
            InitializeComponent();
        }

        private void FormJHTBJDJ_Load(object sender, EventArgs e)
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
                dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
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
            comboBoxBM.SelectedValue = intUserBM;;
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;


            //��ʼ����Ʒ�б�
            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���ϸ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.���� AS ��������, ������Ʒ�Ƶ���ϸ��.���� AS ���, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ��.�ⷿID, ��Ʒ��.������� FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID WHERE (������Ʒ�Ƶ���ϸ��.ID = 0)";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[4].ReadOnly = true;
            dataGridViewDJMX.Columns[9].ReadOnly = true;
            dataGridViewDJMX.Columns[5].ReadOnly = true;
            dataGridViewDJMX.Columns[6].ReadOnly = true;
            dataGridViewDJMX.Columns[12].ReadOnly = true;
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
            dataGridViewDJMX.ShowCellErrors = true;

            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";


            sqlConn.Close();

            //initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;
        }

        private void initDJ()
        {
            int iBM = 0;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.����, ҵ��Ա.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, �����˲���ۻ��ܱ�.��ע, ��λ��.��λ���, ��λ��.��λ����, �����˲���ۻ��ܱ�.��˰�ϼ�, �����˲���ۻ��ܱ�.��Ʊ��, �����˲���ۻ��ܱ�.֧Ʊ��, �����˲���ۻ��ܱ�.����ID, �����˲���ۻ��ܱ�.BeActive FROM �����˲���ۻ��ܱ� INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ҵ��Ա ON �����˲���ۻ��ܱ�.ҵ��ԱID = ҵ��Ա.ID INNER JOIN ְԱ�� ����Ա ON �����˲���ۻ��ܱ�.����ԱID = ����Ա.ID WHERE (�����˲���ۻ��ܱ�.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {

                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy��M��dd��");

                if (sqldr.GetValue(10).ToString() != "")
                {
                    try
                    {
                        iBM = int.Parse(sqldr.GetValue(10).ToString());
                    }
                    catch
                    {
                        iBM = 0;
                    }

                }
                if (!bool.Parse(sqldr.GetValue(11).ToString()))
                {
                    labelDJBH.ForeColor = Color.Red;
                }

                comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                textBoxDWMC.Text = sqldr.GetValue(6).ToString();

                this.Text = "�����˲��۵��ݣ�" + labelDJBH.Text;
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
            sqlComm.CommandText = "SELECT �����˲������ϸ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����˲������ϸ��.��������, �����˲������ϸ��.���, �����˲������ϸ��.���, �����˲������ϸ��.��ƷID, �����˲������ϸ��.�ⷿID, ��Ʒ��.������� FROM �����˲������ϸ�� INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON �����˲������ϸ��.�ⷿID = �ⷿ��.ID WHERE (�����˲������ϸ��.����ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;

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
            dataGridViewDJMX.ShowCellErrors = true;
            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;


            sqlConn.Close();

            dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            dataGridViewDJMX.CellDoubleClick -= dataGridViewDJMX_CellDoubleClick;
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

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(12, textBoxDWMC.Text.Trim()) == 0)
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
                    if (cGetInformation.iBMID != 0)
                        comboBoxBM.SelectedValue = cGetInformation.iBMID;

                    comboBoxYWY.Text = cGetInformation.sCompanyYWY;
                }
            }
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = cGetInformation.iCommNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                    //dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.decCommZZJJ.ToString();

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                    getKCL();
                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[7];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

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
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                    getKCL();

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[7];

                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }
        }

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int iRe = 0;
            if (isSaved)
                return;
            
            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            switch (e.ColumnIndex)
            {
                case 2: //��Ʒ���
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 0; //ID
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }

                    iRe = cGetInformation.getCommInformation(20, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewDJMX.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].ErrorText = "��Ʒ����������";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        } 
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;


                        //dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        getKCL();
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;

                case 1: //��Ʒ����
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                        break;

                    }

                    iRe = cGetInformation.getCommInformation(20, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewDJMX.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //ʧ��
                    //if (cGetInformation.getCommInformation(10, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].ErrorText = "��Ʒ�������������";
                    }
                    else
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        } 
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;

                        getKCL();

                        //dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    }
                    break;
                case 5: //�ⷿ���
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(10, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].ErrorText = "�ⷿ����������";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        } 
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        getKCL();
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;
                case 6: //�ⷿ����
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(20, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "�ⷿ�������������";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        } 
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        getKCL();
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }

                    break;
                case 7:  //��Ʒ����
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut <= 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "��Ʒ���������������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "��Ʒ���������������ʹ���";
                        e.Cancel = true;
                    }
                    break;
                case 8: //��Ʒ�۸�
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;


                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        if (detOut != 0)
                        {
                            detOut = Math.Round(detOut, 2);
                        }
                        else
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[8].ErrorText = "��Ʒ����������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].ErrorText = "��Ʒ������ʹ���";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;



            }
            dataGridViewDJMX.EndEdit();


        }

        private void getKCL()
        {

            //δ���ⷿ
            if (dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[11].Value.ToString() == "" || dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[11].Value.ToString() == "0")
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value = 0;
                return;
            }

            //δ����Ʒ
            if (dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[10].Value.ToString() == "" || dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[10].Value.ToString() == "0")
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value = 0;
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ������� FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[11].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[10].Value.ToString() + ") AND (BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();

            string strKCL = "";
            while (sqldr.Read())
            {
                strKCL = sqldr.GetValue(0).ToString();
            }
            if (strKCL == "")
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value = 0;
            }
            else
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value = Convert.ToDecimal(strKCL);
            }
            sqlConn.Close();
        }


        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("���������ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
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
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    case 5:
                                    case 6:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    case 7:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                        break;
                                    case 8:
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

        //return true ��ȷ  false ����
        private bool countAmount()
        {
            decimal fSum = 0, fSum1 = 0;
            decimal fTemp, fTemp1;
            decimal fCount = 0, fCSum = 0;

            this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            bool bCheck = true;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                //��ƷID
                if (dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() == "0")
                {
                    bCheck = false;
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "��������Ʒ";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "��������Ʒ";
                }

                //�ⷿID
                if (dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() == "0")
                {
                    bCheck = false;
                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "��������Ʒ�ⷿ";
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "��������Ʒ�ⷿ";
                }


                //����
                if (dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[7].Value = 0;
                    fTemp = 0;
                }
                else
                    fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);

                if (fTemp == 0)
                {
                    bCheck = false;
                    dataGridViewDJMX.Rows[i].Cells[7].ErrorText = "��Ʒ������Ҫ����0";
                }

                if (!bCheck)
                {
                    continue;
                }

                //�������
                //if (fTemp > Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[12].Value))
                //{
                //    dataGridViewDJMX.Rows[i].Cells[7].Value = dataGridViewDJMX.Rows[i].Cells[12].Value;
                //    fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);
                //}
                fCSum += fTemp;

                //����
                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[8].Value = 0;
                    fTemp1 = 0;
                }
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);


                //���
                dataGridViewDJMX.Rows[i].Cells[9].Value = Math.Round(fTemp * fTemp1, 2);

                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                fCount += 1;

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelSLHJ.Text = fCSum.ToString();
            labelJEHJ.Text = fSum.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelJEHJ.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;

        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            dataGridViewDJMX.EndEdit();
            int i, j;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;

            //�������
            if (isSaved)
            {
                MessageBox.Show("�����˲��۵����Ѿ�����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("��ѡ��λ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("�����˲��۵�����ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("û��ѡ�񲹼���Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if (MessageBox.Show("��������˲��۵������ݣ��Ƿ�������棿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            //    return;
            saveToolStripButton.Enabled = false;

            string strCount = "", strDateSYS = "", strKey = "ATB";
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
                string sBMID = "NULL";
                if (comboBoxBM.SelectedValue.ToString() != "0")
                    sBMID = comboBoxBM.SelectedValue.ToString();

                sqlComm.CommandText = "INSERT INTO �����˲���ۻ��ܱ� (��λID, ���ݱ��, ����, ��˰�ϼ�, ҵ��ԱID, ����ԱID, BeActive, ��ע, δ������, ����ID, �Ѹ�����) VALUES (" + iSupplyCompany.ToString() + ", N'" + strCount + "', '" + strDateSYS + "', " + labelJEHJ.Text + ", " + comboBoxYWY.SelectedValue.ToString() + ", " + intUserID.ToString() + ", 1, N'" + textBoxBZ.Text.Trim() + "', " + labelJEHJ.Text + "," + sBMID + ",0)";
                   
                sqlComm.ExecuteNonQuery();

                //ȡ�õ��ݺ� 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();


                //��λӦ����
                sqlComm.CommandText = "SELECT Ӧ���˿� FROM ��λ�� WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();

                dKCJE = 0;
                while (sqldr.Read())
                {
                    dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                }
                sqldr.Close();
                dKCJE1 = dKCJE + Convert.ToDecimal(labelJEHJ.Text);
                sqlComm.CommandText = "UPDATE ��λ�� SET Ӧ���˿� = " + dKCJE1.ToString() + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //��λ��ʷ��¼
                sqlComm.CommandText = "INSERT INTO ��λ��ʷ�˱� (��λID, ����, ���ݱ��, ժҪ, �������, Ӧ�����, �������, ҵ��ԱID, ��ֵ���, BeActive, ����ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + strCount + "', N'�����˲��۵���', " + labelJEHJ.Text.ToString() + ", " + dKCJE1.ToString() + ", 1, " + comboBoxYWY.SelectedValue.ToString() + ", N'', 1,"+sBMID+")";
                sqlComm.ExecuteNonQuery();


                //������ϸ
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() == "0") //�޿ⷿ
                    {
                        dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                        continue;
                    }


                    sqlComm.CommandText = "INSERT INTO �����˲������ϸ�� (����ID, ��ƷID, �ⷿID, ��������, ���, ���, BeActive, δ������, δ��������) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", 1, " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();


                }

                //��־��λ
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;
                    dataGridViewDJMX.Rows[i].Cells[0].Value = 1;
                }


                //�ܿ��
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[0].Value) == 0) //�Ѿ������
                        continue;

                    //����õ���ÿ����Ʒ�����
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[0].Value) == 0) //�Ѿ������
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[10].Value == dataGridViewDJMX.Rows[i].Cells[10].Value) //ͬ����Ʒ
                        {
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[9].Value);
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);
                            dataGridViewDJMX.Rows[j].Cells[0].Value = 0;
                        }

                    }
                    dYSYE1 = dKCJE1;
                    dZZJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);

                    //�ܿ����
                    sqlComm.CommandText = "SELECT �������, ���ɱ���, �����, Ӧ����� FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                    }
                    sqldr.Close();

                    //������ɱ���
                    dKCCBJ1 = cGetInformation.countKCCBJ(dKUL, dKCJE, 0, dKCJE1);
                    if (dKCCBJ1 > 0)
                        dKCCBJ = dKCCBJ1;

                    dKCJE = dKUL * dKCCBJ;
                    dYSYE += dYSYE1;

                    //sqlComm.CommandText = "UPDATE ��Ʒ�� SET ���ɱ��� = " + dKCCBJ.ToString() + ", �����=" + dKCJE.ToString() + ", Ӧ�����= " + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ")";
                    sqlComm.CommandText = "UPDATE ��Ʒ�� SET ���ɱ��� = " + dKCCBJ.ToString() + ", �����=" + dKCJE.ToString() + ", Ӧ�����= " + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ")";

                    sqlComm.ExecuteNonQuery();

                    //������ʷ��¼
                    sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, �����˲�������, �����˲��۵���, �����˲��۽��, �ܽ������, �ܽ����, Ӧ�����, BeActive, ����ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'�����˲��۵���', " + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", " + dKCJE1 + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,"+ sBMID +")";
                    sqlComm.ExecuteNonQuery();
                }

                //��־��λ
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    dataGridViewDJMX.Rows[i].Cells[0].Value = 1;
                }

                //�ֿ��
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[0].Value) == 0) //�Ѿ������
                        continue;

                    //����õ���ÿ����Ʒ���
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value); ;
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[0].Value) == 0) //�Ѿ������
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[10].Value == dataGridViewDJMX.Rows[i].Cells[10].Value && dataGridViewDJMX.Rows[j].Cells[11].Value == dataGridViewDJMX.Rows[i].Cells[11].Value) //ͬ����Ʒ��ͬ�����
                        {
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[9].Value);
                            dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value); ;

                            dataGridViewDJMX.Rows[j].Cells[0].Value = 0;
                        }

                    }
                    dYSYE1 = dKCJE1;
                    dZZJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);


                    //�ֿ�����
                    sqlComm.CommandText = "SELECT �������,���ɱ���, �����, Ӧ����� FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ") AND (BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //���ڿ��
                    {
                        sqldr.Read();
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                        sqldr.Close();

                        //������ɱ���
                        dKCCBJ1= cGetInformation.countKCCBJ(dKUL, dKCJE, 0, dKCJE1);
                        if (dKCCBJ1 > 0)
                            dKCCBJ = dKCCBJ1;
                        //dKCJE += dKCJE1;
                        dKCJE = dKUL * dKCCBJ; 
                        dYSYE += dYSYE1;


                        sqlComm.CommandText = "UPDATE ���� SET ���ɱ��� = " + dKCCBJ.ToString() + ",�����=" + dKCJE.ToString() + ", Ӧ�����=" + dYSYE.ToString() + " WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ") AND (BeActive = 1)";
                        sqlComm.ExecuteNonQuery();

                        //�ⷿ����ʷ��¼
                        sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, �����˲�������, �����˲��۵���, �����˲��۽��, �ⷿ�������, �ⷿ�����, Ӧ�����, BeActive, ����ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ",'" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'�����˲��۵���', " + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,"+sBMID+")";
                        sqlComm.ExecuteNonQuery();
                    }
                    else
                        sqldr.Close();

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



            //MessageBox.Show("�����˲��۵��ݱ���ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = "�����˲��۵��ݣ�" + labelDJBH.Text;
            isSaved = true;

            if (MessageBox.Show("�����˲��۵��ݱ���ɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }

 
        }

        private void FormJHTBJDJ_FormClosing(object sender, FormClosingEventArgs e)
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

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("�����˲��۵���ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "�����˲��۵�(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelJEHJ.Text+ "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("�����˲��۵���ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "�����˲��۵�(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelJEHJ.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }


    }
}