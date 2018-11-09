using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormGJSPZD_EDIT : Form
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

        public int intDJID = 0;

        private int iYWY = 0;
         
        private int iSupplyCompany = 0;
        private int intHTH = 0;
        private decimal dDJSUM = 0;
        private int iRowCount = 0;
        private bool isSaved = false;
        private ClassGetInformation cGetInformation;

        private bool bCheck = true;
        private int iBM = 0;
        private int iHT = 0;


        public FormGJSPZD_EDIT()
        {
            InitializeComponent();
        }

        private void FormGJSPZD_EDIT_Load(object sender, EventArgs e)
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

            this.Text += ":���ݳ��";

            sqlConn.Open();


            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, [ְԱ��_1].ְԱ����, ְԱ��.ְԱ���� AS Expr1, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��Ʊ��, ������Ʒ�Ƶ���.���ʽ, �ɹ���ͬ��.��ͬ���, ������Ʒ�Ƶ���.��˰�ϼ�, ��λ��.ID, ������Ʒ�Ƶ���.��ע,������Ʒ�Ƶ���.ҵ��ԱID, ������Ʒ�Ƶ���.����ID, ������Ʒ�Ƶ���.��ͬID FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ������Ʒ�Ƶ���.����ԱID = [ְԱ��_1].ID LEFT OUTER JOIN �ɹ���ͬ�� ON ������Ʒ�Ƶ���.��ͬID = �ɹ���ͬ��.ID WHERE (������Ʒ�Ƶ���.ID = " + intDJID.ToString() + " AND ������Ʒ�Ƶ���.BeActive<>0 )";
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

                if (sqldr.GetValue(14).ToString() != "")
                {
                    try
                    {
                        iHT = int.Parse(sqldr.GetValue(14).ToString());
                    }
                    catch
                    {
                        iHT = 0;
                    }

                }


                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy��M��dd��");
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelCZY.Text = sqldr.GetValue(2).ToString();

                comboBoxYWY.Text = sqldr.GetValue(3).ToString();
                textBoxDWBH.Text = sqldr.GetValue(4).ToString();
                textBoxDWMC.Text = sqldr.GetValue(5).ToString();
                textBoxFPH.Text = sqldr.GetValue(6).ToString();
                textBoxBZ.Text = sqldr.GetValue(11).ToString();

                iYWY = Convert.ToInt32(sqldr.GetValue(12).ToString());
                iSupplyCompany = Convert.ToInt32(sqldr.GetValue(10).ToString());
                dDJSUM = 0;
                if (sqldr.GetValue(9).ToString()!="")
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

            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���ϸ�����.����, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���,��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.���, ������Ʒ�Ƶ���ϸ��.��Ʒ, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ��.�ⷿID, ��Ʒ��.���ս���, ������Ʒ�Ƶ���ϸ��.ID, ������Ʒ�Ƶ���ϸ��.���� AS ԭ������, ������Ʒ�Ƶ���ϸ��.���� AS ԭ������ FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN �ⷿ�� ON ������Ʒ�Ƶ���ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN ������Ʒ�Ƶ���ϸ����� WHERE (������Ʒ�Ƶ���ϸ��.��ID = "+intDJID.ToString()+")";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewGJSPZD.DataSource = dSet.Tables["��Ʒ��"];
            sqlConn.Close();

            dataGridViewGJSPZD.Columns[0].Visible = false;
            dataGridViewGJSPZD.Columns[13].Visible = false;
            dataGridViewGJSPZD.Columns[14].Visible = false;
            dataGridViewGJSPZD.Columns[16].Visible = false;
            dataGridViewGJSPZD.Columns[3].ReadOnly = true;
            dataGridViewGJSPZD.Columns[4].ReadOnly = true;
            //dataGridViewGJSPZD.Columns[7].ReadOnly = true;
            //dataGridViewGJSPZD.Columns[8].ReadOnly = true;
            dataGridViewGJSPZD.Columns[9].ReadOnly = true;
            dataGridViewGJSPZD.Columns[12].ReadOnly = true;
            dataGridViewGJSPZD.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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
            dataGridViewGJSPZD.Columns[17].ReadOnly = true;
            dataGridViewGJSPZD.Columns[18].ReadOnly = true;
            dataGridViewGJSPZD.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewGJSPZD.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[9].DefaultCellStyle.Format = "f2";

            dataGridViewGJSPZD.ShowCellErrors = true;

            for(i=0;i<dataGridViewGJSPZD.Columns.Count;i++)
            {
                dataGridViewGJSPZD.Columns[i].SortMode=DataGridViewColumnSortMode.NotSortable;
            }

            iRowCount = dataGridViewGJSPZD.Rows.Count - 1;


            countAmount();

            if (dataGridViewGJSPZD.Rows.Count > 0)
                dataGridViewGJSPZD.CurrentCell = dataGridViewGJSPZD.Rows[dataGridViewGJSPZD.Rows.Count - 1].Cells[1];

        }

        private bool countAmount()
        {
            decimal fSum = 0, fSum1 = 0;
            decimal fTemp, fTemp1;
            decimal fCount = 0, fCSum = 0;
            bool bCheck = true;

            this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
            cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);

            for (int i = 0; i < dataGridViewGJSPZD.Rows.Count; i++)
            {
                if (dataGridViewGJSPZD.Rows[i].IsNewRow)
                    continue;

                if (i >= iRowCount)
                    dataGridViewGJSPZD.Rows[i].Cells[0].Value = 1;

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

                if (!bCheck)
                    continue;

                if (!Convert.ToBoolean(dataGridViewGJSPZD.Rows[i].Cells[0].Value))
                {
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

                    dataGridViewGJSPZD.Rows[i].Cells[9].Value = Math.Round(fTemp * fTemp1, 2);
                    continue;
                }


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

                if (dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() != "" && dataGridViewGJSPZD.Rows[i].Cells[15].Value.ToString() != "")
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
                if (Convert.ToBoolean(dataGridViewGJSPZD.Rows[i].Cells[10].Value))
                {
                    dataGridViewGJSPZD.Rows[i].Cells[11].Value = 0.0;
                }
                fTemp = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[11].Value);

                //ʵ�ƽ��
                fTemp1 = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[9].Value);
                dataGridViewGJSPZD.Rows[i].Cells[12].Value = fTemp * fTemp1 / 100;


                fSum += Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[9].Value);
                fSum1 += Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[12].Value);

                fCount += 1;

            }
            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
            dataGridViewGJSPZD.EndEdit();

            labelSLHJ.Text = fCSum.ToString();
            labelJEHJ.Text = fSum.ToString();
            labelSJJE.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;


        }

        private void dataGridViewGJSPZD_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                    this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = cGetInformation.decCommZZJJ.ToString();
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
            int iRe = 0;

            if (!bCheck)
                return;

            if (dataGridViewGJSPZD.Rows[e.RowIndex].IsNewRow)
                return;
            if (e.RowIndex < iRowCount && e.ColumnIndex != 0) return;

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
                    //if (cGetInformation.getCommInformation(20, e.FormattedValue.ToString()) == 0) //ʧ��
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
                    //if (cGetInformation.getCommInformation(10, e.FormattedValue.ToString()) == 0) //ʧ��
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
                        if (dOut <= 0 || dOut > 100.0)
                        {
                            dataGridViewGJSPZD.Rows[e.RowIndex].Cells[11].ErrorText = "��Ʒ�����������������0.01-100.0֮�������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[11].ErrorText = "��Ʒ�����������������0.01-100.0֮�������";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;



            }
            dataGridViewGJSPZD.EndEdit();

        }

        private void dataGridViewGJSPZD_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
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
                                    case 5:
                                    case 6:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    case 7:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                        break;
                                    case 8:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[11];
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



        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i,j=0,k,iCount;
            decimal dTemp=0;

            //�������
            if (isSaved)
            {
                MessageBox.Show("������Ʒ�Ƶ��Ѿ�������,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //
            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            //��ѯ���
            sqlComm.CommandText = "SELECT ���ݱ�� FROM ���������ܱ� WHERE (����ID = "+intDJID.ToString()+") AND (BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                while (sqldr.Read())
                {
                    MessageBox.Show("��������¼,���ݺ�Ϊ��" + sqldr.GetValue(0).ToString(), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                }
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            sqldr.Close();

            //��Ʊ��¼
            sqlComm.CommandText = "SELECT ��Ʊ��, ID FROM ������Ʒ�Ƶ��� WHERE (��Ʊ�� IS NOT NULL) AND (��Ʊ�� NOT LIKE N'����Ʊ%') AND (ID = " + intDJID.ToString() + ") AND (��Ʊ�� NOT LIKE N'�ֽ𲻿�Ʊ%')";
            sqldr = sqlComm.ExecuteReader();
            bool b = false;
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
            string strDate1="";
            sqlComm.CommandText = "SELECT ���� from ������Ʒ�Ƶ��� WHERE (ID = " + intDJID.ToString() + ")";
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
            

            //sqlConn.Close();

            string strCount = "", strDateSYS = "", strKey = "AKP";
            
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            saveToolStripButton.Enabled = false;
            try
            {

                //������
                sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET BeActive = 0 WHERE (ID = " + intDJID.ToString() + ")";
                sqlComm.ExecuteNonQuery();


                //�޸ĺ�ͬδִ��״̬
                //��غ�ͬ����
                if (iHT != 0)
                {
                    sqlComm.CommandText = "UPDATE �ɹ���ͬ�� SET ִ�б�� = 0 WHERE (ID = " + iHT.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                string sBMID = "NULL";
                if (iBM != 0)
                    sBMID = iBM.ToString();
                //�õ�����������
                sqlComm.CommandText = "SELECT GETDATE() AS ����";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                sqldr.Close();


                sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET ���ʱ�� = '" + strDateSYS + "' WHERE (ID = " + intDJID.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //��λ��ʷ��¼
                dTemp = Convert.ToDecimal(labelSJJE.Text);
                sqlComm.CommandText = "INSERT INTO ��λ��ʷ�˱� (��λID, ����, ���ݱ��, ժҪ, ����δ�����, ҵ��ԱID, ��ֵ���, BeActive, ����ID) VALUES ( " + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + labelDJBH.Text + "��', N'������Ʒ�Ƶ����', -1*" + dTemp.ToString() + ", " + iYWY.ToString()  + ", N'" + textBoxHTH.Text + "', 1, "+sBMID+")";
                sqlComm.ExecuteNonQuery();


                //������ϸ
                for (i = 0; i < iRowCount; i++)
                {

                        //sqlComm.CommandText = "DELETE FROM ������Ʒ�Ƶ���ϸ�� WHERE (ID = " + dataGridViewGJSPZD.Rows[i].Cells[16].Value.ToString() + ")";
                        //sqlComm.ExecuteNonQuery();

                        //��Ʒ�ⷿ��ʷ��
                        sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (����, ��ƷID, ��λID, �ⷿID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ��������, �������, BeActive, ����ID) VALUES ('" + strDateSYS + "', " + dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[14].Value.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'������Ʒ�Ƶ��޸�', -1*" + dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() + ", -1*" + dataGridViewGJSPZD.Rows[i].Cells[9].Value.ToString() + ", 1,"+sBMID+")";
                        sqlComm.ExecuteNonQuery();

                        //��Ʒ��ʷ��
                        sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ��������, �������, BeActive, ����ID) VALUES ('" + strDateSYS + "', " + dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'������Ʒ�Ƶ��޸�', -1*" + dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() + ", -1*" + dataGridViewGJSPZD.Rows[i].Cells[9].Value.ToString() + ", 1,"+sBMID+")";
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

            //MessageBox.Show("������Ʒ�Ƶ��޸ĳɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isSaved = true;

            if (MessageBox.Show("������Ʒ�Ƶ����ɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void FormGJSPZD_EDIT_FormClosing(object sender, FormClosingEventArgs e)
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

        private void dataGridViewGJSPZD_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (dataGridViewGJSPZD.CurrentCell.RowIndex < iRowCount)
                e.Cancel = true;
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



    }
}