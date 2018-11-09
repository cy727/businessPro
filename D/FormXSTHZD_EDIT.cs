using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXSTHZD_EDIT : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";
        public int intDJID = 0;

        public int iStyle = 0;

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private int iSupplyCompany = 0;
        private int iRowCount = 0;
        private bool isSaved = false;

        private decimal dDJSUM = 0;
        private int iYWY = 0;

        private int iBM = 0;

        private ClassGetInformation cGetInformation;

        public FormXSTHZD_EDIT()
        {
            InitializeComponent();
        }

        private void FormXSTHZD_EDIT_Load(object sender, EventArgs e)
        {
            int i;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            if (intDJID == 0)
                return;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, [ְԱ��_1].ְԱ���� AS ����Ա,ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.��Ʊ��, �����˳����ܱ�.֧Ʊ��, �����˳����ܱ�.��ͬ��, �����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.��ע, ��λ��.ID, �����˳����ܱ�.ҵ��ԱID,�����˳����ܱ�.����ID FROM �����˳����ܱ� INNER JOIN ְԱ�� ON �����˳����ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����˳����ܱ�.����ԱID = [ְԱ��_1].ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.ID = " + intDJID.ToString() + ") AND (�����˳����ܱ�.BeActive<>0)";
            sqldr = sqlComm.ExecuteReader();


            if (!sqldr.HasRows)
            {
                isSaved = true;
                sqldr.Close();
                sqlConn.Close();
                return;
            }

            while (sqldr.Read())
            {
                if (sqldr.GetValue(13).ToString() != "")
                {
                    try
                    {
                        iBM = int.Parse(sqldr.GetValue(13).ToString());
                    }
                    catch
                    {
                        iBM = 0;
                    }

                }

                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy��M��dd��");
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelCZY.Text = sqldr.GetValue(2).ToString();
                comboBoxYWY.Text = sqldr.GetValue(3).ToString();
                textBoxDWBH.Text = sqldr.GetValue(4).ToString();
                textBoxDWMC.Text = sqldr.GetValue(5).ToString();
                textBoxFPH.Text = sqldr.GetValue(6).ToString();
                textBoxZPH.Text = sqldr.GetValue(7).ToString();
                textBoxHTH.Text = sqldr.GetValue(8).ToString();
                textBoxBZ.Text = sqldr.GetValue(10).ToString();
                iSupplyCompany = Convert.ToInt32(sqldr.GetValue(11).ToString());
                iYWY = Convert.ToInt32(sqldr.GetValue(12).ToString());
                dDJSUM = Convert.ToDecimal(sqldr.GetValue(9).ToString());
            }

            sqldr.Close();
            if (iBM != 0)
            {
                sqlComm.CommandText = "SELECT �������� FROM ���ű� WHERE (ID = " + iBM.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                    comboBoxBM.Text = sqldr.GetValue(0).ToString();
                    break;
                }
                sqldr.Close();
            }

            switch (iStyle)
            {
                case 0: //���
                    this.Text += ":���ݳ��";

                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���ϸ�����.����, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���,��Ʒ��.��С������λ AS ��λ, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����˳���ϸ��.����, �����˳���ϸ��.����, �����˳���ϸ��.���, �����˳���ϸ��.����, �����˳���ϸ��.ʵ�ƽ��, �����˳���ϸ��.��ƷID, �����˳���ϸ��.�ⷿID, ��Ʒ��.�������, ������Ʒ�Ƶ���ϸ�����.ͳ�Ʊ�־, �����˳���ϸ��.��Ʒ, �����˳���ϸ��.ID FROM ��Ʒ�� INNER JOIN �����˳���ϸ�� ON ��Ʒ��.ID = �����˳���ϸ��.��ƷID INNER JOIN �ⷿ�� ON �����˳���ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN ������Ʒ�Ƶ���ϸ����� WHERE (�����˳���ϸ��.����ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
                    sqlDA.Fill(dSet, "���ݱ�");
                    dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];

                    sqlConn.Close();

                    dataGridViewDJMX.ReadOnly = true;
                    dataGridViewDJMX.AllowUserToDeleteRows = false;
                    dataGridViewDJMX.AllowUserToAddRows = false;

                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[9].Visible = false;
                    dataGridViewDJMX.Columns[12].Visible = false;
                    dataGridViewDJMX.Columns[13].Visible = false;
                    dataGridViewDJMX.Columns[14].Visible = false;
                    dataGridViewDJMX.Columns[15].Visible = false;
                    dataGridViewDJMX.Columns[17].Visible = false;

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
                    dataGridViewDJMX.Columns[16].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.ShowCellErrors = true;
                    break;
                case 1://�޸�
                    this.Text += ":�����޸�";

                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���ϸ�����.����, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���,��Ʒ��.��С������λ AS ��λ, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, �����˳���ϸ��.����, �����˳���ϸ��.����, �����˳���ϸ��.���, �����˳���ϸ��.����, �����˳���ϸ��.ʵ�ƽ��, �����˳���ϸ��.��ƷID, �����˳���ϸ��.�ⷿID, ��Ʒ��.�������, ������Ʒ�Ƶ���ϸ�����.ͳ�Ʊ�־, �����˳���ϸ��.��Ʒ, �����˳���ϸ��.ID FROM ��Ʒ�� INNER JOIN �����˳���ϸ�� ON ��Ʒ��.ID = �����˳���ϸ��.��ƷID INNER JOIN �ⷿ�� ON �����˳���ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN ������Ʒ�Ƶ���ϸ����� WHERE (�����˳���ϸ��.����ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
                    sqlDA.Fill(dSet, "���ݱ�");
                    dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];

                    sqlConn.Close();

                    //dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[12].Visible = false;
                    dataGridViewDJMX.Columns[13].Visible = false;
                    dataGridViewDJMX.Columns[15].Visible = false;
                    dataGridViewDJMX.Columns[17].Visible = false;
                    dataGridViewDJMX.Columns[3].ReadOnly = true;
                    dataGridViewDJMX.Columns[4].ReadOnly = true;
                    dataGridViewDJMX.Columns[9].ReadOnly = true;
                    dataGridViewDJMX.Columns[11].ReadOnly = true;
                    dataGridViewDJMX.Columns[14].ReadOnly = true;
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
                    dataGridViewDJMX.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[16].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
                    dataGridViewDJMX.ShowCellErrors = true;


                    iRowCount = dataGridViewDJMX.Rows.Count - 1;

                    break;
                default:
                    break;
            }

            countAmount();

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

                if (i >= iRowCount)
                    dataGridViewDJMX.Rows[i].Cells[0].Value = 1;

                //�ⷿID
                if (dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "0" || dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() == "")
                {
                    bCheck = false;
                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "��������Ʒ�ⷿ";
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "��������Ʒ�ⷿ";
                    continue;
                }


                //����
                if (dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() == "")
                    fTemp = 0;
                else
                    fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);

                //if (fTemp > Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[14].Value))
                //{
                //    dataGridViewDJMX.Rows[i].Cells[7].Value = dataGridViewDJMX.Rows[i].Cells[14].Value;
                //    fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);
               // }
                fCSum += fTemp;

                //����
                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);

                //����
                if (dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[10].Value = 100;
                }

                //��Ʒ
                if (dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[16].Value = 0;
                }

                //���
                if (Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[16].Value)) //��Ʒ
                    dataGridViewDJMX.Rows[i].Cells[9].Value = 0;
                else
                    dataGridViewDJMX.Rows[i].Cells[9].Value = Math.Round(fTemp * fTemp1, 2);

                //ʵ�ƽ��
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                dataGridViewDJMX.Rows[i].Cells[11].Value = fTemp1 * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value) / 100;


                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value);

                fCount += 1;

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelSLHJ.Text = fCSum.ToString();
            labelJEHJ.Text = fSum.ToString();
            labelSJJE.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;


        }

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
                return;

            if (e.RowIndex < iRowCount && e.ColumnIndex != 0) return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            switch (e.ColumnIndex)
            {
                case 2: //��Ʒ���
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = Math.Round(Decimal.Zero, 2);

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getCommInformation(20, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].ErrorText = "��Ʒ����������";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        getKCL();

                        //dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;

                case 1: //��Ʒ����
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                        break;

                    }

                    if (cGetInformation.getCommInformation(10, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].ErrorText = "��Ʒ�������������";
                    }
                    else
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
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

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;
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

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
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

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;
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

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
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
                        if (intOut < 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "��Ʒ�����������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "��Ʒ�����������ʹ���";
                        e.Cancel = true;
                    }
                    break;
                case 8: //��Ʒ�۸�
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;


                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        if (detOut >= 0)
                        {
                            detOut = Math.Round(detOut, 2);
                        }
                        else
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[8].ErrorText = "��Ʒ�۸��������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].ErrorText = "��Ʒ�����۸����ʹ���";
                        e.Cancel = true;
                    }
                    break;
                case 10:  //����
                    double dOut = 0.0;
                    if (e.FormattedValue.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 100;
                        break;
                    }
                    if (Double.TryParse(e.FormattedValue.ToString(), out dOut))
                    {
                        if (dOut <= 0 || dOut > 100.0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[10].ErrorText = "��Ʒ�����������������0.01-100.0֮�������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].ErrorText = "��Ʒ�����������������0.01-100.0֮�������";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;



            }
            dataGridViewDJMX.EndEdit();

        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("���������ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex < iRowCount) return;

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
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.iCommNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.decCommZZJJ.ToString();
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                    getKCL();

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[5];
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
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
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
        private void getKCL()
        {

            //δ���ⷿ
            if (dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[13].Value.ToString() == "" || dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[13].Value.ToString() == "0")
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[14].Value = 0;
                return;
            }

            //δ����Ʒ
            if (dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value.ToString() == "" || dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value.ToString() == "0")
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[14].Value = 0;
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ������� FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();

            string strKCL = "";
            while (sqldr.Read())
            {
                strKCL = sqldr.GetValue(0).ToString();
            }
            if (strKCL == "")
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[14].Value = 0;
            }
            else
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[14].Value = Convert.ToDecimal(strKCL);
            }
            sqlConn.Close();
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
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[5];
                                        break;
                                    case 5:
                                    case 6:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    case 7:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                        break;
                                    case 8:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[10];
                                        break;
                                    case 10:
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

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            System.Data.SqlClient.SqlTransaction sqlta;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;
            decimal dKCCBJTemp = 0;
            
            string strDateSYS;
            cGetInformation.getSystemDateTime();
            strDateSYS = cGetInformation.strSYSDATATIME;
            string sBMID = "NULL";
            if (iBM != 0)
                sBMID = iBM.ToString();

            switch (iStyle)
            {
                case 0://���
                    //�������
                    if (isSaved)
                    {
                        MessageBox.Show("�����˻��Ƶ��Ѿ����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    //��ʽȷ��
                    if (!countAmount())
                    {
                        MessageBox.Show("�����˻��Ƶ���ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }


                    sqlConn.Open();
                    //�����
                    sqlComm.CommandText = "SELECT �����տ���ܱ�.���ݱ�� FROM �����տ�ұ� INNER JOIN �����տ���ܱ� ON �����տ�ұ�.����ID = �����տ���ܱ�.ID WHERE (�����տ�ұ�.���ݱ�� = N'" + labelDJBH.Text + "') AND (�����տ�ұ�.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();
                    if (sqldr.HasRows)
                    {
                        while (sqldr.Read())
                        {
                            MessageBox.Show("���в��񹴶Ҽ�¼,���ݺ�Ϊ��" + sqldr.GetValue(0).ToString(), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                        sqldr.Close();
                        sqlConn.Close();
                        return;
                    }
                    sqldr.Close();

                    //��Ʊ��¼
                    sqlComm.CommandText = "SELECT ��Ʊ��, ID FROM �����˳����ܱ� WHERE (��Ʊ�� IS NOT NULL) AND (��Ʊ�� NOT LIKE N'����Ʊ%') AND (ID = " + intDJID.ToString() + ") AND (��Ʊ�� NOT LIKE N'�ֽ𲻿�Ʊ%')";
                    sqldr = sqlComm.ExecuteReader();
                    bool b=false;
                    if (sqldr.HasRows)
                    {
                        while (sqldr.Read())
                        {
                            if (sqldr.GetValue(0).ToString().Trim() != "")
                            {
                                MessageBox.Show("���з�Ʊ��¼,��Ʊ��Ϊ��" + sqldr.GetValue(0).ToString(), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                b = true;
                            }
                            break;
                        }
                        if (b)
                        {
                            sqldr.Close();
                            sqlConn.Close();
                            return;
                        }
                    }
                    sqldr.Close();


                    //�õ��ϴν�תʱ��
                    string sSCJZSJ = "";
                    sqlComm.CommandText = "SELECT ����ʱ��,ID FROM ��ת���ܱ� ORDER BY ����ʱ�� DESC";
                    sqldr = sqlComm.ExecuteReader();
                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                    }
                    sqldr.Close();

                    if (sSCJZSJ == "") //û�н���
                    {
                        sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
                        sqldr = sqlComm.ExecuteReader();
                        while (sqldr.Read())
                        {
                            sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                        }
                        sqldr.Close();
                    }

                    //�õ��Ƶ�����
                    string strDate1 = "";
                    sqlComm.CommandText = "SELECT ���� from �����˳����ܱ� WHERE (ID = " + intDJID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    while (sqldr.Read())
                    {
                        strDate1 = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                    }
                    sqldr.Close();

                    if (DateTime.Parse(strDate1) <= DateTime.Parse(sSCJZSJ)) //��ת���¼
                    {
                        if (MessageBox.Show("�Ƶ�������ת���¼��" + sSCJZSJ + "���Ƿ�ǿ�г�죿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                        {
                            sqlConn.Close();
                            return;
                        }
                    }
            

                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        //������
                        sqlComm.CommandText = "UPDATE �����˳����ܱ� SET BeActive = 0 WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        sqlComm.CommandText = "UPDATE �����˳����ܱ� SET ���ʱ�� = '" + strDateSYS + "' WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //��λӦ����
                        sqlComm.CommandText = "SELECT Ӧ���˿� FROM ��λ�� WHERE (ID = " + iSupplyCompany.ToString() + ")";
                        sqldr = sqlComm.ExecuteReader();

                        dKCJE = 0;
                        while (sqldr.Read())
                        {
                            dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        }
                        sqldr.Close();
                        dKCJE1 = dKCJE + Convert.ToDecimal(labelSJJE.Text);
                        sqlComm.CommandText = "UPDATE ��λ�� SET Ӧ���˿� = " + dKCJE1.ToString() + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //��λ��ʷ��¼
                        sqlComm.CommandText = "INSERT INTO ��λ��ʷ�˱� (��λID, ����, ���ݱ��, ժҪ, �������, Ӧ�ս��, ���۱��, ҵ��ԱID, ��ֵ���, BeActive, ����ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + labelDJBH.Text + "��', N'�����˻ص����', " + labelSJJE.Text.ToString() + ", " + dKCJE1.ToString() + ", 1, " + iYWY.ToString() + ", N'" + labelDJBH.Text + "', 1,"+sBMID+")";
                        sqlComm.ExecuteNonQuery();

                        //���
                        for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;

                            //����õ���ÿ����Ʒ�����
                            dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);
                            dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                            dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value);

                            dYSYE1 = dKCJE1;

                            //�ܿ����
                            sqlComm.CommandText = "SELECT �������, ���ɱ���, �����, Ӧ�ս�� FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                            }
                            sqldr.Close();


                            dKUL -= dKUL1;
                            //dKCJE -= dKCJE1;
                            dKCJE = dKUL * dKCCBJ;
                            dYSYE += dYSYE1;

                            sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dKUL.ToString() + ", ���ɱ���=" + dKCCBJ.ToString() + ", �����=" + dKCJE.ToString() + ", Ӧ�ս��= " + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //������ʷ��¼
                            sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, �˻�����, �˻ص���, �˻ؽ��, �ܽ������, �ܽ����, Ӧ�ս��, BeActive, �������, ��ⵥ��, �����, ����ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'�����˻ص����', -" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", -" + dKCJE1 + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,-" + dKUL1.ToString() + "," + dKCCBJ1.ToString() + ",-" + dKCJE1.ToString() + ","+sBMID+")";
                            sqlComm.ExecuteNonQuery();

                    //�ֿ�����
                    sqlComm.CommandText = "SELECT �������, �����, ���ɱ���, Ӧ�ս�� FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //���ڿ��
                    {
                        sqldr.Read();
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                        sqldr.Close();


                        dKUL -= dKUL1;
                        //dKCJE -= dKCJE1;
                        dKCJE = dKUL * dKCCBJ;
                        dYSYE += dYSYE1;

                        sqlComm.CommandText = "UPDATE ���� SET ������� = " + dKUL.ToString() + ", ���ɱ��� = " + dKCCBJ.ToString() + ",�����=" + dKCJE.ToString() + ", Ӧ�ս��=" + dYSYE.ToString() + " WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                        sqlComm.ExecuteNonQuery();
                        
                        //�ⷿ����ʷ��¼
                        sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, �˻�����, �˻ص���, �˻ؽ��, �ⷿ�������, �ⷿ�����, Ӧ�ս��, BeActive, �������, ��ⵥ��, �����, ����ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ",'" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'�����˻ص����', -" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ",-" + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1, -" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", -" + dKCJE1.ToString() + ","+sBMID+")";
                        sqlComm.ExecuteNonQuery();
                        }
                        else
                            sqldr.Close();

                    }
                        //����
                        sqlComm.CommandText = "DELETE FROM ��Ʒ����� WHERE (���ݱ�� = N'" + labelDJBH.Text + "')";
                        sqlComm.ExecuteNonQuery();

                        sqlta.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("���ݿ����" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqlta.Rollback();
                        return;
                    }
                    finally
                    {
                        sqlConn.Close();
                    }

                    //MessageBox.Show("�����˳��Ƶ����ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSaved = true;
                    if (MessageBox.Show("�����˳��Ƶ����ɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Close();
                    }


                    break;


                case 1://�޸�

                    //�������
                    if (isSaved)
                    {
                        MessageBox.Show("�����˳��Ƶ��Ƶ��Ѿ��޸�,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    //��ʽȷ��
                    if (!countAmount())
                    {
                        MessageBox.Show("�����˳��Ƶ��Ƶ���ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }


                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        //������
                        sqlComm.CommandText = "UPDATE �����˳����ܱ� SET ��˰�ϼ� = " + labelSJJE.Text + " WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //Ӧ����
                        sqlComm.CommandText = "SELECT Ӧ���˿� FROM ��λ�� WHERE (ID = " + iSupplyCompany.ToString() + ")";
                        sqldr = sqlComm.ExecuteReader();
                        while (sqldr.Read())
                        {
                            dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        }
                        sqldr.Close();
                        dKCJE = dKCJE + Convert.ToDecimal(labelSJJE.Text) - dDJSUM;
                        sqlComm.CommandText = "UPDATE ��λ�� SET Ӧ���˿� = " + dKCJE + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //��λ��ʷ��¼
                        sqlComm.CommandText = "INSERT INTO ��λ��ʷ�˱� (��λID, ����, ���ݱ��, ժҪ, �������, Ӧ�ս��, ���۱��, ҵ��ԱID, ��ֵ���, BeActive, ����ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + labelDJBH.Text + "��', N'�����˻ص��޸�', " + labelSJJE.Text.ToString() + ", " + dKCJE.ToString() + ", 1, " + iYWY.ToString() + ", N'" + labelDJBH.Text + "', 1,"+sBMID+")";
                        sqlComm.ExecuteNonQuery();



                        //��ϸ&��� ԭ����
                        for (i = 0; i < iRowCount; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;

                            if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value.ToString())) //ɾ��
                            {

                                //����õ���ÿ����Ʒ�����
                                dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);
                                dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                                dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value);

                                dYSYE1 = dKCJE1;

                                //�ܿ����
                                sqlComm.CommandText = "SELECT �������, ���ɱ���, �����, Ӧ�ս�� FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                                sqldr = sqlComm.ExecuteReader();
                                while (sqldr.Read())
                                {
                                    dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                    dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                    dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                                    dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                                }
                                sqldr.Close();


                                dKUL -= dKUL1;
                                //dKCJE -= dKCJE1;
                                dKCJE = dKUL * dKCCBJ;
                                dYSYE += dYSYE1;

                                sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dKUL.ToString() + ", ���ɱ���=" + dKCCBJ.ToString() + ", �����=" + dKCJE.ToString() + ", Ӧ�ս��= " + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                                sqlComm.ExecuteNonQuery();

                                //������ʷ��¼
                                sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, �˻�����, �˻ص���, �˻ؽ��, �ܽ������, �ܽ����, Ӧ�ս��, BeActive, �������, ��ⵥ��, �����, ����ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'�����˻ص��޸�', -" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", -" + dKCJE1 + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,-" + dKUL1.ToString() + "," + dKCCBJ1.ToString() + ",-" + dKCJE1.ToString() + ","+sBMID+")";
                                sqlComm.ExecuteNonQuery();

                                //�ֿ�����
                                sqlComm.CommandText = "SELECT �������, �����, ���ɱ���, Ӧ�ս�� FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                sqldr = sqlComm.ExecuteReader();

                                if (sqldr.HasRows) //���ڿ��
                                {
                                    sqldr.Read();
                                    dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                    dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                                    dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                    dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                                    sqldr.Close();


                                    dKUL -= dKUL1;
                                    //dKCJE -= dKCJE1;
                                    dKCJE = dKUL * dKCCBJ;
                                    dYSYE += dYSYE1;

                                    sqlComm.CommandText = "UPDATE ���� SET ������� = " + dKUL.ToString() + ", ���ɱ��� = " + dKCCBJ.ToString() + ",�����=" + dKCJE.ToString() + ", Ӧ�ս��=" + dYSYE.ToString() + " WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                    sqlComm.ExecuteNonQuery();

                                    //�ⷿ����ʷ��¼
                                    sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, �˻�����, �˻ص���, �˻ؽ��, �ⷿ�������, �ⷿ�����, Ӧ�ս��, BeActive, �������, ��ⵥ��, �����, ����ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ",'" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'�����˻ص��޸�', -" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ",-" + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1, -" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", -" + dKCJE1.ToString() + ","+sBMID+")";
                                    sqlComm.ExecuteNonQuery();
                                }
                                else
                                    sqldr.Close();


                                sqlComm.CommandText = "DELETE FROM �����˳���ϸ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[17].Value.ToString() + ")";
                                sqlComm.ExecuteNonQuery();
                            }
                        }

                        //������ϸ
                        for (i = iRowCount; i < dataGridViewDJMX.Rows.Count; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;

                            //���ɱ���
                            sqlComm.CommandText = "SELECT ���ɱ��� FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            dKCCBJTemp = 0;
                            while (sqldr.Read())
                            {
                                dKCCBJTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                            }
                            sqldr.Close();

                            sqlComm.CommandText = "INSERT INTO �����˳���ϸ�� (����ID, ��ƷID, �ⷿID, ԭ����ID, ����, ���, ����, ʵ�ƽ��, δ������, BeActive, ����,���ɱ���) VALUES (" + intDJID.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", 0, " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", 1," + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + "," + dKCCBJTemp.ToString()+ ")";
                            sqlComm.ExecuteNonQuery();


                            //����õ���ÿ����Ʒ�����
                            dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);
                            dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                            dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value);

                            dYSYE1 = dKCJE1;

                            //�ܿ����
                            sqlComm.CommandText = "SELECT �������, ���ɱ���, �����, Ӧ�ս�� FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                            }
                            sqldr.Close();


                            dKUL += dKUL1;
                            //dKCJE += dKCJE1;
                            dKCJE = dKUL * dKCCBJ;
                            dYSYE -= dYSYE1;

                            sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dKUL.ToString() + ", ���ɱ���=" + dKCCBJ.ToString() + ", �����=" + dKCJE.ToString() + ", Ӧ�ս��= " + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //������ʷ��¼
                            sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, �˻�����, �˻ص���, �˻ؽ��, �ܽ������, �ܽ����, Ӧ�ս��, BeActive, �������, ��ⵥ��, �����, ����ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'�����˻ص��޸�', -" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", -" + dKCJE1 + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,-" + dKUL1.ToString() + "," + dKCCBJ1.ToString() + ",-" + dKCJE1.ToString() + ","+sBMID+")";
                            sqlComm.ExecuteNonQuery();

                            //�ֿ�����
                            sqlComm.CommandText = "SELECT �������, �����, ���ɱ���, Ӧ�ս�� FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                            sqldr = sqlComm.ExecuteReader();

                            if (sqldr.HasRows) //���ڿ��
                            {
                                sqldr.Read();
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                                sqldr.Close();


                                dKUL += dKUL1;
                                //dKCJE += dKCJE1;
                                dKCJE = dKUL * dKCCBJ;
                                dYSYE -= dYSYE1;

                                sqlComm.CommandText = "UPDATE ���� SET ������� = " + dKUL.ToString() + ", ���ɱ��� = " + dKCCBJ.ToString() + ",�����=" + dKCJE.ToString() + ", Ӧ�ս��=" + dYSYE.ToString() + " WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                sqlComm.ExecuteNonQuery();

                                //�ⷿ����ʷ��¼
                                sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, �˻�����, �˻ص���, �˻ؽ��, �ⷿ�������, �ⷿ�����, Ӧ�ս��, BeActive, �������, ��ⵥ��, �����, ����ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ",'" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'�����˻ص��޸�', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + "," + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1, " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ","+sBMID+")";
                                sqlComm.ExecuteNonQuery();
                            }


                        }


                        sqlta.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("���ݿ����" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqlta.Rollback();
                        return;
                    }
                    finally
                    {
                        sqlConn.Close();
                    }

                    //MessageBox.Show("�����˻ص��޸ĳɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSaved = true;
                    if (MessageBox.Show("�����˻ص��޸ĳɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Close();
                    }


                    break;





            }
 
        }

        private void FormXSTHZD_EDIT_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
                return;

            DialogResult dr = MessageBox.Show(this, "�����޸���δ���棬ȷ��Ҫ�˳���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("�����˻��������ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "�����˻ص�(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelSJJE.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("�����˻��������ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "�����˻ص�(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelSJJE.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }



    }
}