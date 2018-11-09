using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormYSZKJS : Form
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
        
        
        public FormYSZKJS()
        {
            InitializeComponent();
        }

        private void FormYSZKJS_Load(object sender, EventArgs e)
        {
            int i;

            this.Top= 1;
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

            DataRow drTemp1 = dSet.Tables["���ű�"].NewRow();
            drTemp1[0] = 0;
            drTemp1[1] = "ȫ��";
            dSet.Tables["���ű�"].Rows.Add(drTemp1);

            comboBoxBM.DataSource = dSet.Tables["���ű�"];
            comboBoxBM.DisplayMember = "��������";
            comboBoxBM.ValueMember = "ID";
            //comboBoxBM.SelectedValue = intUserBM;;
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;

            //��ʼ�������б�
            sqlComm.CommandText = "SELECT �����տ���ϸ��.ID, �˲���.�˲����, �˲���.�˲�����, �����տ���ϸ��.ժҪ, �����տ���ϸ��.��Ӧ����, �˲���.����, �����տ���ϸ��.������, �����տ���ϸ��.֧Ʊ��, �����տ���ϸ��.��ע, �˲���.�˲�ID, �����տ���.���ұ��, �����տ���.���Ҽ�¼ FROM �˲��� INNER JOIN �����տ���ϸ�� ON �˲���.ID = �����տ���ϸ��.�˲�ID CROSS JOIN �����տ��� WHERE (�����տ���ϸ��.ID = 0)";

            if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
            sqlDA.Fill(dSet, "������ϸ��");
            dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

            dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            DataRow drTemp = dSet.Tables["������ϸ��"].NewRow();
            dSet.Tables["������ϸ��"].Rows.Add(drTemp);

            dataGridViewDJMX.Columns[0].Visible = false;
            //
            dataGridViewDJMX.Columns[4].ReadOnly = true;
            dataGridViewDJMX.Columns[9].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[6].ReadOnly = true;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;

            dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

            sqlConn.Close();

            //initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            //labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            dateTimePickerZDRQ.Value=Convert.ToDateTime(strDT);
            labelCZY.Text = strUserName;

            comboBoxGD.SelectedIndex = 1;
            comboBoxBM.Text = "���۲�";

        }

        private void initDJ()
        {
            int iBM = 0;

            toolStripButtonFP.Visible = true;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT �����տ���ܱ�.���ݱ��, �����տ���ܱ�.����, ҵ��Ա.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա, �����տ���ܱ�.��ע, ��λ��.��λ���, ��λ��.��λ����, �����տ���ܱ�.��Ʊ��, ��λ��.˰��, ��λ��.Ӧ���˿�, �����տ���ܱ�.����ID, �����տ���ܱ�.��ע2, �����տ���ܱ�.BeActive FROM ��λ�� INNER JOIN �����տ���ܱ� ON ��λ��.ID = �����տ���ܱ�.��λID INNER JOIN ְԱ�� ҵ��Ա ON �����տ���ܱ�.ҵ��ԱID = ҵ��Ա.ID INNER JOIN ְԱ�� ����Ա ON �����տ���ܱ�.����ԱID = ����Ա.ID WHERE (�����տ���ܱ�.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                //labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy��M��dd��");
                dateTimePickerZDRQ.Value = Convert.ToDateTime(sqldr.GetValue(1).ToString());

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

                if (!bool.Parse(sqldr.GetValue(12).ToString()))
                {
                    labelDJBH.ForeColor = Color.Red;
                }

                comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                textBoxBZ2.Text = sqldr.GetValue(11).ToString();
                textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                textBoxDWMC.Text = sqldr.GetValue(6).ToString();
                textBoxSH.Text = sqldr.GetValue(8).ToString();
                textBoxFPH.Text = sqldr.GetValue(7).ToString();
                textBoxYSYE.Text = sqldr.GetValue(9).ToString();



                this.Text = "Ӧ���˿���㵥��" + labelDJBH.Text;
            }
            sqldr.Close();

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
            sqlComm.CommandText = "SELECT �����տ���ϸ��.ID, �˲���.�˲����, �˲���.�˲�����, �����տ���ϸ��.ժҪ, �����տ���ϸ��.��Ӧ����, �˲���.����, �����տ���ϸ��.������, �����տ���ϸ��.֧Ʊ��, �����տ���ϸ��.��ע, �����տ���ϸ��.�˲�ID, �����տ���.���ұ��, �����տ���.���Ҽ�¼ FROM �˲��� INNER JOIN �����տ���ϸ�� ON �˲���.ID = �����տ���ϸ��.�˲�ID CROSS JOIN �����տ��� WHERE (�����տ���ϸ��.����ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
            sqlDA.Fill(dSet, "������ϸ��");
            dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[9].Visible = false;
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
            dataGridViewDJMX.ShowCellErrors = true;

            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;
            sqlConn.Close();
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
            if (cGetInformation.getCompanyInformation(2, "") == 0)
            {
                //return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                textBoxSH.Text = cGetInformation.strCompanySH;
                //textBoxYSYE.Text = cGetInformation.dCompanyYSZK.ToString();
                textBoxYSYE.Text = getCompanyPay(iSupplyCompany);
                if (dSet.Tables.Contains("���յ��ݹ���"))
                    dSet.Tables.Remove("���յ��ݹ���");
                if (dSet.Tables.Contains("���յ�����ϸ����"))
                    dSet.Tables.Remove("���յ�����ϸ����");
                if (dSet.Tables.Contains("���յ�����ϸ��������"))
                    dSet.Tables.Remove("���յ�����ϸ��������");
                if (dSet.Tables.Contains("������ϸ��")) dSet.Tables["������ϸ��"].Clear();
                dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                DataRow drTemp = dSet.Tables["������ϸ��"].NewRow();
                dSet.Tables["������ϸ��"].Rows.Add(drTemp);
                dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                if (cGetInformation.iBMID != 0)
                    comboBoxBM.SelectedValue = cGetInformation.iBMID;

                comboBoxYWY.Text = cGetInformation.sCompanyYWY;

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
                    textBoxSH.Text = "";
                    textBoxYSYE.Text = "0.00";

                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxSH.Text = cGetInformation.strCompanySH;
                    //textBoxYSYE.Text = cGetInformation.dCompanyYSZK.ToString();
                    textBoxYSYE.Text = getCompanyPay(iSupplyCompany);

                    if (dSet.Tables.Contains("���յ��ݹ���"))
                        dSet.Tables.Remove("���յ��ݹ���");
                    if (dSet.Tables.Contains("���յ�����ϸ����"))
                        dSet.Tables.Remove("���յ�����ϸ����");
                    if (dSet.Tables.Contains("������ϸ��������"))
                        dSet.Tables.Remove("������ϸ��������");
                    if (dSet.Tables.Contains("������ϸ��")) dSet.Tables["������ϸ��"].Clear();
                    dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                    DataRow drTemp = dSet.Tables["������ϸ��"].NewRow();
                    dSet.Tables["������ϸ��"].Rows.Add(drTemp);
                    dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
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
                if (cGetInformation.getCompanyInformation(22, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxSH.Text = "";
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                    textBoxYSYE.Text = "0.00";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxSH.Text = cGetInformation.strCompanySH;
                    //textBoxYSYE.Text = cGetInformation.dCompanyYSZK.ToString();
                    textBoxYSYE.Text = getCompanyPay(iSupplyCompany);

                    if (dSet.Tables.Contains("���յ��ݹ���"))
                        dSet.Tables.Remove("���յ��ݹ���");
                    if (dSet.Tables.Contains("���յ�����ϸ����"))
                        dSet.Tables.Remove("���յ�����ϸ����");
                    if (dSet.Tables.Contains("������ϸ��������"))
                        dSet.Tables.Remove("������ϸ��������");
                    if (dSet.Tables.Contains("������ϸ��")) dSet.Tables["������ϸ��"].Clear();
                    dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                    DataRow drTemp = dSet.Tables["������ϸ��"].NewRow();
                    dSet.Tables["������ϸ��"].Rows.Add(drTemp);
                    dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    if (cGetInformation.iBMID != 0)
                        comboBoxBM.SelectedValue = cGetInformation.iBMID;

                    comboBoxYWY.Text = cGetInformation.sCompanyYWY;
                }
            }
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow || dataGridViewDJMX.RowCount - 1 == e.RowIndex)
            {
               // dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
               // DataRow drTemp = dSet.Tables["������ϸ��"].NewRow();
               // dSet.Tables["������ϸ��"].Rows.Add(drTemp);
               // dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            }
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2) //�˲����
            {
                if (cGetInformation.getZBInformation(1, "") == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.iZBNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strZBName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strZBCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.dZBKL;

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[4];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }
        }

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
           //     return;
            if (isSaved)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            switch (e.ColumnIndex)
            {
                case 1: //�˲����
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getZBInformation(10, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].ErrorText = "�˲�����������";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value.ToString() == cGetInformation.iZBNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.iZBNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strZBName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strZBCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.dZBKL;

                        //if (dataGridViewDJMX.RowCount - 1 == e.RowIndex)
                        //{
                        //    DataRow drTemp = dSet.Tables["������ϸ��"].NewRow();
                        //    dSet.Tables["������ϸ��"].Rows.Add(drTemp);
                        //}

                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;

                case 2: //�˲�����
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                        break;

                    }
                    if (e.FormattedValue.ToString() == dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value.ToString())
                        break;

                    if (cGetInformation.getCommInformation(11, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].ErrorText = "�˲��������������";
                    }
                    else
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value.ToString() == cGetInformation.iZBNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.iZBNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strZBName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strZBCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.dZBKL;

                        //if (dataGridViewDJMX.RowCount - 1 == e.RowIndex)
                        //{
                        //    DataRow drTemp = dSet.Tables["������ϸ��"].NewRow();
                        //    dSet.Tables["������ϸ��"].Rows.Add(drTemp);
                        //}

                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    }
                    break;
                case 4: //��Ӧ����
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;


                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        detOut = Math.Round(detOut, 2);
                        //if (dataGridViewDJMX.RowCount - 1 == e.RowIndex)
                        //{
                        //    DataRow drTemp = dSet.Tables["������ϸ��"].NewRow();
                        //    dSet.Tables["������ϸ��"].Rows.Add(drTemp);
                        //}
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].ErrorText = "��Ʒ�����۸����ʹ���";
                        e.Cancel = true;
                    }
                    break;
                case 5:  //����
                    double dOut = 0.0;
                    if (e.FormattedValue.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 100.00;
                        break;
                    }
                    if (Double.TryParse(e.FormattedValue.ToString(), out dOut))
                    {
                        if (dOut <= 0 || dOut > 100.0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[5].ErrorText = "��Ʒ�����������������0.01-100.00֮�������";
                            e.Cancel = true;
                        }
                        else
                        {
                            //if (dataGridViewDJMX.RowCount - 1 == e.RowIndex)
                            //{
                            //    DataRow drTemp = dSet.Tables["������ϸ��"].NewRow();
                            //    dSet.Tables["������ϸ��"].Rows.Add(drTemp);
                            //}
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].ErrorText = "��Ʒ�����������������0.01-100.00֮�������";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;



            }
            dataGridViewDJMX.EndEdit();
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
                                    case 3:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[4];
                                        break;
                                    case 4:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[5];
                                        break;
                                    case 5:
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

                //�ⷿID
                if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "0")
                {
                    bCheck = false;
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "�������˲����";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "�������˲�������";
                    continue;
                }


                //��Ӧ����
                if (dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[4].Value = 0;

                fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);


                //����
                if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[5].Value = 100;
                }


                //ʵ�ƽ��
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                dataGridViewDJMX.Rows[i].Cells[6].Value = fTemp * fTemp1 / 100;


                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);

                fCount += 1;

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelJEHJ.Text = fSum.ToString();
            labelSJJE.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;
        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("���������ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            DataColumn[] dcKey = new DataColumn[1];
            int rowIndex = 0;
            FormSelectGD frmSelectGD = new FormSelectGD();
            frmSelectGD.strConn = strConn;

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("��ѡ����㵥λ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            toolStripButtonFP_Click_1(null, null);


            if (dataGridViewDJMX.CurrentCell == null)
                dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[0].Cells[4];
            dataGridViewDJMX.BeginEdit(true);
            switch (comboBoxGD.SelectedIndex)
            {
                case 0: //���յ��ݹ���

                    if (!dSet.Tables.Contains("���յ��ݹ���"))  //��ʼ�����ݹ����б�
                    {
                        sqlConn.Open();
                        sqlComm.CommandText = "SELECT �����տ���.����, �տ������ͼ.���ݱ��, �տ������ͼ.����, �տ������ͼ.��˰�ϼ�, �տ������ͼ.�Ѹ�����, �����տ���.��֧�����, �տ������ͼ.δ������, �տ������ͼ.ID, �����տ���.���ұ��, CONVERT(decimal, 0) AS ����� FROM �����տ��� CROSS JOIN �տ������ͼ WHERE (�տ������ͼ.��λID =  "+ iSupplyCompany.ToString() +") AND (�տ������ͼ.δ������ <> 0)";
                        sqlDA.Fill(dSet, "���յ��ݹ���");
                        //dcKey[0]=dSet.Tables["���յ��ݹ���"].Columns[7];
                        //dSet.Tables["���յ��ݹ���"].PrimaryKey = dcKey;

                        sqlConn.Close();
                    }


                    frmSelectGD.iSelectStyle = 1;

                    frmSelectGD.dtSelect = dSet.Tables["���յ��ݹ���"];
                    frmSelectGD.ShowDialog();
                    if (frmSelectGD.iSUMSELECT != 0)
                    {
                        if (dataGridViewDJMX.CurrentCell == null)
                            rowIndex = 0;
                        else
                            rowIndex = dataGridViewDJMX.CurrentCell.RowIndex;


                        dataGridViewDJMX.Rows[rowIndex].Cells[4].Value = frmSelectGD.dSUMJE;

                    }

                    break;

                case 1: //���յ�����ϸ����
                    if (!dSet.Tables.Contains("���յ�����ϸ����"))  //��ʼ�����ݹ����б�
                    {
                        sqlConn.Open();
                        sqlComm.CommandText = "SELECT �����տ���.����, �տ���ϸ��ͼ.���ݱ��, �տ���ϸ��ͼ.����,�տ���ϸ��ͼ.��Ʒ���, �տ���ϸ��ͼ.��Ʒ����, ��Ʒ��.��Ʒ���, �տ���ϸ��ͼ.����, �տ���ϸ��ͼ.ʵ�ƽ��, �տ���ϸ��ͼ.�Ѹ�����, �����տ���.��֧�����, �տ���ϸ��ͼ.δ������, �����տ���.���ұ��, �տ���ϸ��ͼ.����ID, �տ���ϸ��ͼ.ID, �տ���ϸ��ͼ.��ƷID, �տ���ϸ��ͼ.������, �տ���ϸ��ͼ.�ⷿID, CONVERT(decimal, 0) AS �����, �տ���ϸ��ͼ.��ע FROM �տ���ϸ��ͼ INNER JOIN ��Ʒ�� ON �տ���ϸ��ͼ.��ƷID = ��Ʒ��.ID CROSS JOIN �����տ��� WHERE (�տ���ϸ��ͼ.��λID = " + iSupplyCompany.ToString() + ") AND (�տ���ϸ��ͼ.δ������ <> 0)";
                        sqlDA.Fill(dSet, "���յ�����ϸ����");
                        //dcKey[0] = dSet.Tables["���յ�����ϸ����"].Columns[13];
                        //dSet.Tables["���յ�����ϸ����"].PrimaryKey = dcKey;

                        sqlConn.Close();
                    }
                    frmSelectGD.iSelectStyle = 2;

                    frmSelectGD.dtSelect = dSet.Tables["���յ�����ϸ����"];
                    frmSelectGD.ShowDialog();
                    if (frmSelectGD.iSUMSELECT != 0)
                    {
                        if (dataGridViewDJMX.CurrentCell == null)
                            rowIndex = 0;
                        else
                            rowIndex = dataGridViewDJMX.CurrentCell.RowIndex;


                        dataGridViewDJMX.Rows[rowIndex].Cells[4].Value = frmSelectGD.dSUMJE;
                    }
                    break;

                case 2: //������ϸ��������
                    if (!dSet.Tables.Contains("������ϸ��������"))  //��ʼ�����ݹ����б�
                    {
                        sqlConn.Open();
                        sqlComm.CommandText = "SELECT �����տ���.����, �տ���ϸ��ͼ.���ݱ��, �տ���ϸ��ͼ.����, �տ���ϸ��ͼ.��Ʒ���, �տ���ϸ��ͼ.��Ʒ����, ��Ʒ��.��Ʒ���, �տ���ϸ��ͼ.����, �տ���ϸ��ͼ.�Ѹ�������, �����տ���.����������, �տ���ϸ��ͼ.δ��������, �տ���ϸ��ͼ.ʵ�ƽ��, �տ���ϸ��ͼ.�Ѹ�����, �����տ���.��֧�����, �տ���ϸ��ͼ.δ������, �����տ���.���ұ��, �տ���ϸ��ͼ.����ID, �տ���ϸ��ͼ.ID, �տ���ϸ��ͼ.��ƷID, �տ���ϸ��ͼ.������, �տ���ϸ��ͼ.�ⷿID, CONVERT(decimal, 0) AS ����� FROM �տ���ϸ��ͼ INNER JOIN ��Ʒ�� ON �տ���ϸ��ͼ.��ƷID = ��Ʒ��.ID CROSS JOIN �����տ��� WHERE (�տ���ϸ��ͼ.��λID = " + iSupplyCompany.ToString() + ") AND (�տ���ϸ��ͼ.δ������ <> 0)";
                        sqlDA.Fill(dSet, "������ϸ��������");
                        //dcKey[0] = dSet.Tables["������ϸ��������"].Columns[16];
                        //dSet.Tables["������ϸ��������"].PrimaryKey = dcKey;

                        sqlConn.Close();
                    }
                    frmSelectGD.iSelectStyle = 3;

                    frmSelectGD.dtSelect = dSet.Tables["������ϸ��������"];
                    frmSelectGD.ShowDialog();
                    if (frmSelectGD.iSUMSELECT != 0)
                    {
                        if (dataGridViewDJMX.CurrentCell == null)
                            rowIndex = 0;
                        else
                            rowIndex = dataGridViewDJMX.CurrentCell.RowIndex;


                        dataGridViewDJMX.Rows[rowIndex].Cells[4].Value = frmSelectGD.dSUMJE;
                    }
                    break;


                default:
                    MessageBox.Show("��ѡ�񹴶ҷ�ʽ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
            }
            dataGridViewDJMX.EndEdit();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i,j;
            string sTemp = "";
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;

            //�������
            if (isSaved)
            {
                MessageBox.Show("Ӧ���˿���㵥�Ѿ�����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                MessageBox.Show("Ӧ���˿���㵥��ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("û��ѡ��Ӧ���˿������ϸ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if (MessageBox.Show("����Ӧ���˿���㵥���ݣ��Ƿ�������棿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            //    return;

            saveToolStripButton.Enabled = false;
            string strCount = "", strDateSYS = "", strKey = "BYS";
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

                if (textBoxFPH.Text != "")
                    sqlComm.CommandText = "INSERT INTO �����տ���ܱ� (���ݱ��, ԭ����ID, ��λID, ˰��, ҵ��ԱID, ����ԱID, ����, ��Ʊ��, ��Ʊ����, ��ע,     BeActive, ʵ�ƽ��, ����ID, ��ע2) VALUES (N'" + strCount + "', NULL, " + iSupplyCompany.ToString() + ", N'" + textBoxSH.Text + "', " + comboBoxYWY.SelectedValue.ToString() + ", " + intUserID.ToString() + ", '" + dateTimePickerZDRQ.Value.ToShortDateString() + "', N'" + textBoxFPH.Text + "', '" + dateTimePickerZDRQ.Value.ToShortDateString() + "', N'" + textBoxBZ.Text + "', 1, " + labelSJJE.Text + "," + sBMID + " , N'"+textBoxBZ2.Text+"')";
                else
                    sqlComm.CommandText = "INSERT INTO �����տ���ܱ� (���ݱ��, ԭ����ID, ��λID, ˰��, ҵ��ԱID, ����ԱID, ����, ��Ʊ��, ��Ʊ����, ��ע,     BeActive, ʵ�ƽ��, ����ID, ��ע2) VALUES (N'" + strCount + "', NULL, " + iSupplyCompany.ToString() + ", N'" + textBoxSH.Text + "', " + comboBoxYWY.SelectedValue.ToString() + ", " + intUserID.ToString() + ", '" + dateTimePickerZDRQ.Value.ToShortDateString() + "', N'', NULL, N'" + textBoxBZ.Text + "', 1, " + labelSJJE.Text + "," + sBMID + ", N'" + textBoxBZ2.Text + "')";
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
                dKCJE1 = dKCJE - Convert.ToDecimal(labelSJJE.Text);
                sqlComm.CommandText = "UPDATE ��λ�� SET Ӧ���˿� = " + dKCJE1.ToString() + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //��λ��ʷ��¼
                sqlComm.CommandText = "INSERT INTO ��λ��ʷ�˱� (��λID, ����, ���ݱ��, ժҪ, ������, Ӧ�ս��, ���۱��, ҵ��ԱID, ��ֵ���, BeActive, ����ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + strCount + "', N'Ӧ���˿���㵥', " + labelSJJE.Text.ToString() + ", " + dKCJE1.ToString() + ", 1, " + comboBoxYWY.SelectedValue.ToString() + ", N'', 1,"+sBMID+")";
                sqlComm.ExecuteNonQuery();

                //������ϸ
                for (j = 0; j < dataGridViewDJMX.Rows.Count; j++)
                {

                    sqlComm.CommandText = "INSERT INTO �����տ���ϸ�� (����ID, �˲�ID, ֧Ʊ��, ����, ժҪ, ��Ӧ����, ������, ��ע) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[j].Cells[9].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[j].Cells[7].Value.ToString() + "', " + dataGridViewDJMX.Rows[j].Cells[5].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[j].Cells[3].Value.ToString() + "', " + dataGridViewDJMX.Rows[j].Cells[4].Value.ToString() + ", " + dataGridViewDJMX.Rows[j].Cells[6].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[j].Cells[8].Value.ToString() + "')";
                    sqlComm.ExecuteNonQuery();

                    //ȡ�õ��ݺ� 
                    sqlComm.CommandText = "SELECT @@IDENTITY";
                    sqldr = sqlComm.ExecuteReader();
                    sqldr.Read();
                    string sNo = sqldr.GetValue(0).ToString();
                    sqldr.Close();


                    //���Ҽ�¼
                    if (dSet.Tables.Contains("���յ��ݹ���"))
                    {
                        dSet.Tables["���յ��ݹ���"].AcceptChanges();
                        DataRow[] dtTemp1;
                        dtTemp1 = dSet.Tables["���յ��ݹ���"].Select("���ұ��=1");

                        for (i = 0; i < dtTemp1.Length; i++)
                        {
                            sTemp = dtTemp1[i][1].ToString().Substring(0, 3);
                            switch (sTemp)
                            {
                                case "BKP":
                                    if (Convert.ToDecimal(dtTemp1[i][6].ToString()) == 0)
                                        sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET δ������ = " + dtTemp1[i][6].ToString() + ", �Ѹ����� =  " + dtTemp1[i][4].ToString() + ", ������ = 1 , ����ʱ�� = '" + strDateSYS + "' WHERE (ID = " + dtTemp1[i][7].ToString() + ")";
                                    else
                                        sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET δ������ = " + dtTemp1[i][6].ToString() + ", �Ѹ����� =  " + dtTemp1[i][4].ToString() + ", ������ = 0 WHERE (ID = " + dtTemp1[i][7].ToString() + ")";
                                    break;

                                case "BTH":
                                    if (Convert.ToDecimal(dtTemp1[i][6].ToString()) == 0)
                                        sqlComm.CommandText = "UPDATE �����˳����ܱ� SET δ������ = " + dtTemp1[i][6].ToString() + ", �Ѹ����� =  " + dtTemp1[i][4].ToString() + ", ������ = 1 , ����ʱ�� = '" + strDateSYS + "' WHERE (ID = " + dtTemp1[i][7].ToString() + ")";
                                    else
                                        sqlComm.CommandText = "UPDATE �����˳����ܱ� SET δ������ = " + dtTemp1[i][6].ToString() + ", �Ѹ����� =  " + dtTemp1[i][4].ToString() + ", ������ = 0 WHERE (ID = " + dtTemp1[i][7].ToString() + ")";
                                    break;

                                case "BTB":
                                    if (Convert.ToDecimal(dtTemp1[i][6].ToString()) == 0)
                                        sqlComm.CommandText = "UPDATE �����˲���ۻ��ܱ� SET δ������ = " + dtTemp1[i][6].ToString() + ", �Ѹ����� =  " + dtTemp1[i][4].ToString() + ", ������ = 1 , ����ʱ�� = '" + strDateSYS + "' WHERE (ID = " + dtTemp1[i][7].ToString() + ")";
                                    else
                                        sqlComm.CommandText = "UPDATE �����˲���ۻ��ܱ� SET δ������ = " + dtTemp1[i][6].ToString() + ", �Ѹ����� =  " + dtTemp1[i][4].ToString() + ", ������ = 0 WHERE (ID = " + dtTemp1[i][7].ToString() + ")";
                                    break;

                            }
                            sqlComm.ExecuteNonQuery();

                            //����
                            sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��λID, ҵ��ԱID, ���ݱ��, ԭ���ݱ��, ժҪ, ������, Ӧ�ս��, δ�ս��, ���ս��, BeActive, ����ID) VALUES ('" + strDateSYS + "', " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'" + dtTemp1[i][1].ToString() + "', N'Ӧ���˿���㵥', " + dtTemp1[i][9].ToString() + ", " + dtTemp1[i][3].ToString() + ", " + dtTemp1[i][6].ToString() + ", " + dtTemp1[i][4].ToString() + ", 1,"+sBMID+")";
                            sqlComm.ExecuteNonQuery();


                        }



                    }

                    if (dSet.Tables.Contains("���յ�����ϸ����"))
                    {
                        dSet.Tables["���յ�����ϸ����"].AcceptChanges();
                        DataRow[] dtTemp2;
                        dtTemp2 = dSet.Tables["���յ�����ϸ����"].Select("���ұ��=1");

                        for (i = 0; i < dtTemp2.Length; i++)
                        {
                            sTemp = dtTemp2[i][1].ToString().Substring(0, 3);
                            switch (sTemp)
                            {
                                case "BKP":
                                    sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ���ϸ�� SET δ������ = " + dtTemp2[i][10].ToString() + ", �Ѹ����� = " + dtTemp2[i][8].ToString() + " WHERE (ID = " + dtTemp2[i][13].ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET δ������ = δ������ - " + dtTemp2[i][17].ToString() + ", �Ѹ����� =  �Ѹ����� + " + dtTemp2[i][17].ToString() + " WHERE (ID = " + dtTemp2[i][12].ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    break;

                                case "BTH":
                                    sqlComm.CommandText = "UPDATE �����˳���ϸ�� SET δ������ = " + dtTemp2[i][10].ToString() + ", �Ѹ����� = -1*" + dtTemp2[i][8].ToString() + " WHERE (ID = " + dtTemp2[i][13].ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE �����˳����ܱ� SET δ������ = δ������ - (-1.0*" + dtTemp2[i][17].ToString() + "), �Ѹ����� =  �Ѹ����� + (-1*" + dtTemp2[i][17].ToString() + ") WHERE (ID = " + dtTemp2[i][12].ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;

                                case "BTB":
                                    sqlComm.CommandText = "UPDATE �����˲������ϸ�� SET δ������ = " + dtTemp2[i][10].ToString() + ", �Ѹ����� = " + dtTemp2[i][8].ToString() + " WHERE (ID = " + dtTemp2[i][13].ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE �����˲���ۻ��ܱ� SET δ������ = δ������ - " + dtTemp2[i][17].ToString() + ", �Ѹ����� =  �Ѹ����� + " + dtTemp2[i][17].ToString() + " WHERE (ID = " + dtTemp2[i][12].ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    
                                    break;
                            }
                            //sqlComm.ExecuteNonQuery();

                            //���Ҽ�¼
                            sqlComm.CommandText = "INSERT INTO �����տ�ұ� (����ID, ���ҷ�ʽ, ����ID, ���ݱ��, �Ѹ���, BeActive) VALUES (" + sNo + ", 1, " + dtTemp2[i][13].ToString() + ", N'" + dtTemp2[i][1].ToString() + "', " + dtTemp2[i][17].ToString() + ", 1)";
                            sqlComm.ExecuteNonQuery();


                            //�ܿ��
                            dKCJE = Convert.ToDecimal(dtTemp2[i][17].ToString());
                            sqlComm.CommandText = "SELECT  Ӧ�ս��, ���ս�� FROM ��Ʒ�� WHERE (ID = " + dtTemp2[i][14].ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dYSYE1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                            }
                            sqldr.Close();

                            //���
                            dYSYE -= dKCJE;
                            dYSYE1 += dKCJE;

                            dKCJE1 = dYSYE + dYSYE1;


                            sqlComm.CommandText = "UPDATE ��Ʒ�� SET Ӧ�ս��=" + dYSYE.ToString() + ", ���ս��=" + dYSYE1.ToString() + " WHERE (ID = " + dtTemp2[i][14].ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //������ʷ��¼
                            sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ������, Ӧ�ս��, δ�ս��, ���ս��, BeActive, ����ID) VALUES ('" + strDateSYS + "', " + dtTemp2[i][14].ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'Ӧ���˿���㵥', " + dKCJE.ToString() + ", " + dKCJE1.ToString() + ", " + dYSYE.ToString() + ", " + dYSYE1.ToString() + " , 1, "+sBMID+")";
                            sqlComm.ExecuteNonQuery();

                            //�ֿ�����
                            sqlComm.CommandText = "SELECT  Ӧ�ս��, ���ս�� FROM ���� WHERE (�ⷿID = " + dtTemp2[i][16].ToString() + ") AND (��ƷID = " + dtTemp2[i][14].ToString() + ") AND (BeActive = 1)";

                            dKCJE = Convert.ToDecimal(dtTemp2[i][17].ToString());
                            dYSYE = 0; dYSYE1 = 0;
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dYSYE1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                            }
                            sqldr.Close();

                            //���
                            dYSYE -= dKCJE;
                            dYSYE1 += dKCJE;

                            dKCJE1 = dYSYE + dYSYE1;
                            sqlComm.CommandText = "UPDATE ���� SET  Ӧ�ս��=" + dYSYE.ToString() + ", ���ս��=" + dYSYE1.ToString() + " WHERE (�ⷿID = " + dtTemp2[i][16].ToString() + ") AND (��ƷID = " + dtTemp2[i][14].ToString() + ") AND (BeActive = 1)";

                            //�����ʷ��¼
                            sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ������, Ӧ�ս��, δ�ս��, ���ս��, BeActive, ����ID) VALUES (" + dtTemp2[i][16].ToString() + ", '" + strDateSYS + "', " + dtTemp2[i][14].ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'Ӧ���˿���㵥', " + dKCJE.ToString() + ", " + dKCJE1.ToString() + ", " + dYSYE.ToString() + ", " + dYSYE1.ToString() + " , 1,"+sBMID+")";
                            sqlComm.ExecuteNonQuery();
                        }

                    }

                    if (dSet.Tables.Contains("������ϸ��������"))
                    {
                        dSet.Tables["������ϸ��������"].AcceptChanges();
                        DataRow[] dtTemp3;
                        dtTemp3 = dSet.Tables["������ϸ��������"].Select("���ұ��=1");

                        for (i = 0; i < dtTemp3.Length; i++)
                        {
                            sTemp = dtTemp3[i][1].ToString().Substring(0, 3);
                            switch (sTemp)
                            {
                                case "BKP":
                                    sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ���ϸ�� SET δ�������� = " + dtTemp3[i][9].ToString() + ", �Ѹ������� = " + dtTemp3[i][7].ToString() + ", δ������ = " + dtTemp3[i][3].ToString() + ", �Ѹ����� =" + dtTemp3[i][11].ToString() + " WHERE (ID = " + dtTemp3[i][16].ToString() + ")";
                                    break;

                                case "BTH":
                                    sqlComm.CommandText = "UPDATE �����˳���ϸ�� SET δ�������� = " + dtTemp3[i][9].ToString() + ", �Ѹ������� = " + dtTemp3[i][7].ToString() + ", δ������ = " + dtTemp3[i][3].ToString() + ", �Ѹ����� =" + dtTemp3[i][11].ToString() + " WHERE (ID = " + dtTemp3[i][16].ToString();
                                    break;

                                case "BTB":
                                    sqlComm.CommandText = "UPDATE �����˲������ϸ�� SET δ�������� = " + dtTemp3[i][9].ToString() + ", �Ѹ������� = " + dtTemp3[i][7].ToString() + ", δ������ = " + dtTemp3[i][3].ToString() + ", �Ѹ����� =" + dtTemp3[i][11].ToString() + " WHERE (ID = " + dtTemp3[i][16].ToString();
                                    break;
                            }
                            sqlComm.ExecuteNonQuery();


                            //�ܿ��
                            dKCJE = Convert.ToDecimal(dtTemp3[i][20].ToString());
                            sqlComm.CommandText = "SELECT  Ӧ�ս��, ���ս�� FROM ��Ʒ�� WHERE (ID = " + dtTemp3[i][17].ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dYSYE1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                            }
                            sqldr.Close();

                            //���
                            dYSYE -= dKCJE;
                            dYSYE1 += dKCJE;

                            dKCJE1 = dYSYE + dYSYE1;


                            sqlComm.CommandText = "UPDATE ��Ʒ�� SET Ӧ�ս��=" + dYSYE.ToString() + ", ���ս��=" + dYSYE1.ToString() + " WHERE (ID = " + dtTemp3[i][17].ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //������ʷ��¼
                            sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ������, Ӧ�ս��, δ�ս��, ���ս��, BeActive, ����ID) VALUES ('" + strDateSYS + "', " + dtTemp3[i][17].ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'Ӧ���˿���㵥', " + dKCJE.ToString() + ", " + dKCJE1.ToString() + ", " + dYSYE.ToString() + ", " + dYSYE1.ToString() + " , 1,"+sBMID+")";
                            sqlComm.ExecuteNonQuery();

                            //�ֿ�����
                            sqlComm.CommandText = "SELECT  Ӧ�ս��, ���ս�� FROM ���� WHERE (�ⷿID = " + dtTemp3[i][19].ToString() + ") AND (��ƷID = " + dtTemp3[i][17].ToString() + ") AND (BeActive = 1)";

                            dKCJE = Convert.ToDecimal(dtTemp3[i][20].ToString());
                            dYSYE = 0; dYSYE1 = 0;
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dYSYE1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                            }
                            sqldr.Close();

                            //���
                            dYSYE -= dKCJE;
                            dYSYE1 += dKCJE;

                            dKCJE1 = dYSYE + dYSYE1;
                            sqlComm.CommandText = "UPDATE ���� SET  Ӧ�ս��=" + dYSYE.ToString() + ", ���ս��=" + dYSYE1.ToString() + " WHERE (�ⷿID = " + dtTemp3[i][19].ToString() + ") AND (��ƷID = " + dtTemp3[i][17].ToString() + ") AND (BeActive = 1)";

                            //�����ʷ��¼
                            sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ������, Ӧ�ս��, δ�ս��, ���ս��, BeActive, ����ID) VALUES (" + dtTemp3[i][19].ToString() + ", '" + strDateSYS + "', " + dtTemp3[i][17].ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'Ӧ���˿���㵥', " + dKCJE.ToString() + ", " + dKCJE1.ToString() + ", " + dYSYE.ToString() + ", " + dYSYE1.ToString() + " , 1, "+sBMID+")";
                            sqlComm.ExecuteNonQuery();

                        }

                    }
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


            //MessageBox.Show("Ӧ���˿���㵥����ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = "Ӧ���˿���㵥��" + labelDJBH.Text;
            isSaved = true;

            bool bClose = false;
            if (MessageBox.Show("Ӧ���˿���㵥����ɹ����Ƿ�ر��Ƶ�����", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                bClose = true;
            }

            if (MessageBox.Show("Ӧ���˿���㵥����ɹ����Ƿ������ʼ��һ���Ƶ���", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                // �������Ӵ����һ����ʵ����
                FormYSZKJS childFormYSZKJS = new FormYSZKJS();
                // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                childFormYSZKJS.MdiParent = this.MdiParent;

                childFormYSZKJS.strConn = strConn;

                childFormYSZKJS.intUserID = intUserID;
                childFormYSZKJS.intUserLimit = intUserLimit;
                childFormYSZKJS.strUserLimit = strUserLimit;
                childFormYSZKJS.strUserName = strUserName;
                childFormYSZKJS.Show();
            }

            if (bClose)
                this.Close();



        }

        private void toolStripButtonFP_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;

            sqlConn.Open();
            sqlComm.CommandText = "UPDATE �����տ���ܱ� SET ��Ʊ�� = N'" + textBoxFPH.Text + "', ��Ʊ����='" + strDT + "' WHERE (ID = " + iDJID.ToString() + ")";
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();

            MessageBox.Show("��Ʊ�ŵǼ����", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            if (!countAmount())
            {
                MessageBox.Show("Ӧ���˿���㵥��ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "Ӧ���˿���㵥(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + dateTimePickerZDRQ.Value.ToLongDateString() + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelJEHJ.Text + "(��д:" + labelDX.Text + ");����Ʊ�ţ�" + textBoxFPH.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            if (!countAmount())
            {
                MessageBox.Show("Ӧ���˿���㵥��ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "Ӧ���˿���㵥(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + dateTimePickerZDRQ.Value.ToLongDateString() + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelJEHJ.Text + "(��д:" + labelDX.Text + ");����Ʊ�ţ�" + textBoxFPH.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void FormYSZKJS_FormClosing(object sender, FormClosingEventArgs e)
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

        private void toolStripButtonFP_Click_1(object sender, EventArgs e)
        {
            //��ʼ�������б�
            dSet.Tables["������ϸ��"].Clear();
            DataRow drTemp = dSet.Tables["������ϸ��"].NewRow();
            dSet.Tables["������ϸ��"].Rows.Add(drTemp);

            if (dSet.Tables.Contains("���յ�����ϸ����"))
            {
                dSet.Tables.Remove(dSet.Tables["���յ�����ϸ����"]);
            }
        }

        private void comboBoxGD_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxGD.SelectedIndex = 1;
        }

        private string getCompanyPay(int icompanyID)
        {
            string strPay = "0.00";

            sqlConn.Open();
            sqlComm.CommandText = "SELECT SUM(δ������) FROM �տ���ϸ��ͼ WHERE (��λID = " + icompanyID .ToString()+ ")";

            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                try
                {
                    strPay = decimal.Parse(sqldr.GetValue(0).ToString()).ToString("f2");
                }
                catch
                {
                }
            }


            sqlConn.Close();

            return strPay;

        }


    }
}