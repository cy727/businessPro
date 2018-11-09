using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKCJWCKDJ_EDIT : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";
        public int intDJID = 0;
        public int intCD = 0;
        public string strCD = "";

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

        private ClassGetInformation cGetInformation;
        private bool bCheck = true;
        private int iBM = 0;

        
        public FormKCJWCKDJ_EDIT()
        {
            InitializeComponent();
        }

        private void FormKCJWCKDJ_EDIT_Load(object sender, EventArgs e)
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

            dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            sqlConn.Open();
            //sqlComm.CommandText = "SELECT ���������ܱ�.��λID, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.��ϵ�绰, ���������ܱ�.��ϵ��, ���������ܱ�.�ջ���, ���������ܱ�.��վ, ���������ܱ�.���䷽ʽ, ���������ܱ�.��ϸ��ַ, ���������ܱ�.��������, ���������ܱ�.����, ���������ܱ�.��������, ְԱ��.ְԱ���� AS ҵ��Ա, [ְԱ��_1].ְԱ���� AS ����Ա, ���������ܱ�.���ݱ��, ���������ܱ�.��ע, ���������ܱ�.��˰�ϼ�,���������ܱ�.ҵ��ԱID, ���������ܱ�.����ID FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���������ܱ�.����ԱID = [ְԱ��_1].ID WHERE (���������ܱ�.ID = " + intDJID.ToString() + ") AND ���������ܱ�.BeActive <> 0 AND (���������ܱ�.��ֵ���ID IS NULL)";
            sqlComm.CommandText = "SELECT ���������ܱ�.��λID, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.��ϵ�绰, ���������ܱ�.��ϵ��, ���������ܱ�.�ջ���, ���������ܱ�.��վ, ���������ܱ�.���䷽ʽ, ���������ܱ�.��ϸ��ַ, ���������ܱ�.��������, ���������ܱ�.����, ���������ܱ�.��������, ְԱ��.ְԱ���� AS ҵ��Ա, [ְԱ��_1].ְԱ���� AS ����Ա, ���������ܱ�.���ݱ��, ���������ܱ�.��ע, ���������ܱ�.��˰�ϼ�,���������ܱ�.ҵ��ԱID, ���������ܱ�.����ID, ���������ܱ�.��ֵ���ID FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���������ܱ�.����ԱID = [ְԱ��_1].ID WHERE (���������ܱ�.ID = " + intDJID.ToString() + ") AND ���������ܱ�.BeActive <> 0 ";

            if (dSet.Tables.Contains("�ͻ���")) dSet.Tables.Remove("�ͻ���");
            sqlDA.Fill(dSet, "�ͻ���");

            if (dSet.Tables["�ͻ���"].Rows.Count < 1)
            {
                textBoxDWBH.Text = "";
                textBoxDWMC.Text = "";
                textBoxLXDH.Text = "";
                textBoxLXR.Text = "";
                textBoxSHR.Text = "";
                textBoxDZ.Text = "";
                comboBoxYSFS.Text = "";
                textBoxXXDZ.Text = "";
                textBoxWLMC.Text = "";
                textBoxDH.Text = "";
                textBoxYZBM.Text = "";
                comboBoxYWY.Text = "";
                labelCZY.Text = "";
                labelDJBH.Text = "";
                textBoxBZ.Text = "";
                iSupplyCompany = 0;
                sqlConn.Close();
                isSaved = true;

                ReturntoolStripButton.Enabled = false;
                saveToolStripButton.Enabled = false;
                return;
            }
            else
            {
                if (dSet.Tables["�ͻ���"].Rows[0][18].ToString() != "")
                {
                    try
                    {
                        iBM = int.Parse(dSet.Tables["�ͻ���"].Rows[0][18].ToString());
                    }
                    catch
                    {
                        iBM = 0;
                    }

                }
                //���
                if (dSet.Tables["�ͻ���"].Rows[0][19].ToString() != "")
                {
                    try
                    {
                        intCD = int.Parse(dSet.Tables["�ͻ���"].Rows[0][19].ToString());
                    }
                    catch
                    {
                        intCD = 0;
                    }
                }
                textBoxLXDH.Text = dSet.Tables["�ͻ���"].Rows[0][3].ToString();
                textBoxLXR.Text = dSet.Tables["�ͻ���"].Rows[0][4].ToString();
                textBoxSHR.Text = dSet.Tables["�ͻ���"].Rows[0][5].ToString();
                textBoxDZ.Text = dSet.Tables["�ͻ���"].Rows[0][6].ToString();
                comboBoxYSFS.Text = dSet.Tables["�ͻ���"].Rows[0][7].ToString();
                textBoxXXDZ.Text = dSet.Tables["�ͻ���"].Rows[0][8].ToString();
                textBoxWLMC.Text = dSet.Tables["�ͻ���"].Rows[0][9].ToString();
                textBoxDH.Text = dSet.Tables["�ͻ���"].Rows[0][10].ToString();
                textBoxYZBM.Text = dSet.Tables["�ͻ���"].Rows[0][11].ToString();
                comboBoxYWY.Text = dSet.Tables["�ͻ���"].Rows[0][12].ToString();
                labelCZY.Text = dSet.Tables["�ͻ���"].Rows[0][13].ToString();
                labelDJBH.Text = dSet.Tables["�ͻ���"].Rows[0][14].ToString();
                textBoxBZ.Text = dSet.Tables["�ͻ���"].Rows[0][15].ToString();
                iSupplyCompany = Convert.ToInt32(dSet.Tables["�ͻ���"].Rows[0][0].ToString());
                textBoxDWBH.Text = dSet.Tables["�ͻ���"].Rows[0][1].ToString();
                textBoxDWMC.Text = dSet.Tables["�ͻ���"].Rows[0][2].ToString();
                dDJSUM = Convert.ToDecimal(dSet.Tables["�ͻ���"].Rows[0][16].ToString());
                iYWY = Convert.ToInt32(dSet.Tables["�ͻ���"].Rows[0][17].ToString());

            }
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
                    bCheck = false;
                    this.Text += ":���ݳ��";
                    saveToolStripButton.Text = "���";

                    sqlComm.CommandText = "SELECT ������Ʒ�����.����, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���,�ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ���������ϸ��.����, ���������ϸ��.����, ���������ϸ��.���, ���������ϸ��.���ɱ���, ���������ϸ��.������, ���������ϸ��.��ע, ���������ϸ��.��ƷID, ���������ϸ��.�ⷿID, ��Ʒ��.�������, ������Ʒ�����.ͳ�Ʊ�־, ��Ʒ��.���ս���, ���������ϸ��.ID FROM ���������ϸ�� INNER JOIN ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN ������Ʒ����� WHERE (���������ϸ��.��ID = " + intDJID.ToString() + ")";

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

                    dataGridViewDJMX.Columns[16].Visible = false;

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

                    dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
                    dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
                    break;
                case 1://�޸�
                    ReturntoolStripButton.Enabled = false;
                    this.Text += ":�����޸�";
                    saveToolStripButton.Text = "�޸�";


                    sqlComm.CommandText = "SELECT ������Ʒ�����.����, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���,�ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ���������ϸ��.����, ���������ϸ��.����, ���������ϸ��.���, ���������ϸ��.���ɱ���, ���������ϸ��.������, ���������ϸ��.��ע, ���������ϸ��.��ƷID, ���������ϸ��.�ⷿID, ��Ʒ��.�������, ������Ʒ�����.ͳ�Ʊ�־, ��Ʒ��.���ս���, ���������ϸ��.ID, ���������ϸ��.���� AS ԭ������, ���������ϸ��.���� AS ԭ������ FROM ���������ϸ�� INNER JOIN ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID CROSS JOIN ������Ʒ����� WHERE (���������ϸ��.��ID = " + intDJID.ToString() + ")";

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
                    dataGridViewDJMX.Columns[8].ReadOnly = true;
                    dataGridViewDJMX.Columns[9].ReadOnly = true;
                    dataGridViewDJMX.Columns[10].ReadOnly = true;
                    dataGridViewDJMX.Columns[14].ReadOnly = true;
                    dataGridViewDJMX.Columns[16].ReadOnly = true;
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
                    dataGridViewDJMX.Columns[18].ReadOnly = true;
                    dataGridViewDJMX.Columns[19].ReadOnly = true;

                    dataGridViewDJMX.ShowCellErrors = true;


                    for (i = 0; i < dataGridViewDJMX.Columns.Count; i++)
                    {
                        dataGridViewDJMX.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    iRowCount = dataGridViewDJMX.Rows.Count - 1;

                    break;
                default:
                    break;
            }
            countAmount();


            dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.RowValidating += dataGridViewDJMX_RowValidating;

           

        }
        private bool countAmount()
        {
            decimal fSum = 0;
            decimal fSum1 = 0;
            decimal fCSum = 0;

            decimal fTemp, fTemp1;
            decimal fCount = 0;
            bool bCheck1 = true;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                if (i >= iRowCount)
                    dataGridViewDJMX.Rows[i].Cells[0].Value = 1;

                if (dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "����������Ʒ";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "����������Ʒ";
                    bCheck1 = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[4].ErrorText = "����������Ʒ�ⷿ";
                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "����������Ʒ�ⷿ";
                    bCheck1 = false;
                }


                if (dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "����������Ʒ����";
                    bCheck1 = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[7].ErrorText = "����������Ʒ�۸�";
                    bCheck1 = false;
                }

                if (!bCheck1)
                    continue;

                //���ɱ�
                if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[9].Value = 0;

                //�����
                if (dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[14].Value = 0;

                if (dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[16].Value = 0;
                //��ɫ��ʾ
                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString()) >= 0) //���
                {
                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value) < Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value))
                        dataGridViewDJMX.Rows[i].Cells[7].Style.BackColor = Color.LightPink;
                    else
                        dataGridViewDJMX.Rows[i].Cells[7].Style.BackColor = Color.White;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value) > Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[14].Value))
                        dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.LightPink;
                    else
                        dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.White;
                }
                else //����
                {
                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value) > Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value))
                        dataGridViewDJMX.Rows[i].Cells[7].Style.BackColor = Color.LightPink;
                    else
                        dataGridViewDJMX.Rows[i].Cells[7].Style.BackColor = Color.White;
                    dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.White;
                }


                //����
                fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                //����
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);

                //���
                dataGridViewDJMX.Rows[i].Cells[8].Value = Math.Round(fTemp * fTemp1, 2);

                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                //������
                dataGridViewDJMX.Rows[i].Cells[10].Value = Math.Round(fTemp * fTemp1, 2);

                if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value))
                {
                    continue;
                }


                fCount += 1;
                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelJEHJ.Text = fSum.ToString();
            labelCKJE.Text = fSum1.ToString();
            labelSLHJ.Text = fCSum.ToString();
            toolStripStatusLabelMXJLS.Text = fCount.ToString();
            labelDX.Text = cGetInformation.changeDAXIE(labelCKJE.Text);

            return bCheck1;

        }

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int iRe = 0;

            if (!bCheck)
                return;

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
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
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
                    //if (cGetInformation.getCommInformation(20, e.FormattedValue.ToString()) == 0) //ʧ��
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
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommKCCBJ.ToString();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.dKCL;
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
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                        break;

                    }

                    iRe = cGetInformation.getCommInformation(10, e.FormattedValue.ToString());
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

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommKCCBJ.ToString();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    }
                    break;
                case 4: //�ⷿ���
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;

                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(10, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].ErrorText = "�ⷿ����������";
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
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;
                case 5: //�ⷿ����
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(20, e.FormattedValue.ToString()) == 0) //ʧ��
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].ErrorText = "�ⷿ�������������";
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
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }

                    break;
                case 6:  //��Ʒ����
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut == 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "��Ʒ�����������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "��Ʒ�����������ʹ���";
                        e.Cancel = true;
                    }
                    break;
                case 7: //��Ʒ�۸�
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;
                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == "" || dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == "0")
                    {
                        MessageBox.Show("��������������Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;
                    }

                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value.ToString() == "" || dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value.ToString() == "0")
                    {
                        MessageBox.Show("��������������Ʒ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;
                    }

                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        if (detOut >= 0)
                        {
                            detOut = Math.Round(detOut, 2);

                            if (Convert.ToDecimal(dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value.ToString()) > 0) //���
                            {
                                if (detOut.CompareTo(dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value) < 0)
                                {
                                    if (MessageBox.Show("��Ʒ�۸���ڳɱ��ۣ��Ƿ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                        e.Cancel = true;
                                    else
                                    {
                                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = detOut;
                                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                                    }

                                }
                            }
                            else //����
                            {
                                if (detOut.CompareTo(dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value) > 0)
                                {
                                    if (MessageBox.Show("��Ʒ�۸���ڳɱ��ۣ��Ƿ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                        e.Cancel = true;
                                    else
                                    {
                                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = detOut;
                                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                                    }

                                }
                            }
                        }
                        else
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "��Ʒ�۸��������";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "��Ʒ�����۸����ʹ���";
                        e.Cancel = true;
                    }
                    break;
                default:
                    break;

            }
            dataGridViewDJMX.EndEdit();


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
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[4];
                                        break;
                                    case 4:
                                    case 5:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[6];
                                        break;
                                    case 6:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    case 7:
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

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommKCCBJ;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = cGetInformation.decCommZZJJ;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == "")
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;

                    cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value));
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.dKCL;

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[4];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }

            if (e.ColumnIndex == 4 || e.ColumnIndex == 5) //�ⷿ���
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
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;

                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == "")
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;

                    cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value));
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.dKCL;
                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[6];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                }
            }


        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;
            int i, j, k;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;


            string strDateSYS;
            cGetInformation.getSystemDateTime();
            strDateSYS = cGetInformation.strSYSDATATIME;
            //������
            string sBMID = "NULL";
            if (iBM != 0)
                sBMID = iBM.ToString();

            switch (iStyle)
            {
                case 0://���
                    //�������
                    if (isSaved)
                    {
                        MessageBox.Show("�����ﵥ�Ѿ����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    //��ʽȷ��
                    if (!countAmount())
                    {
                        MessageBox.Show("�����ﵥ��ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (intCD != 0)
                    {
                        if (intCD != -1)
                        {
                            if (MessageBox.Show("�õ������г�ּ�¼���Ƿ�ǿ�г�죿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                                return;
                        }

                    }


                    sqlConn.Open();

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
                    sqlComm.CommandText = "SELECT ���� from ���������ܱ� WHERE (ID = " + intDJID.ToString() + ")";
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
                        sqlComm.CommandText = "UPDATE ���������ܱ� SET BeActive = 0 WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        sqlComm.CommandText = "UPDATE ���������ܱ� SET ���ʱ�� = '" + strDateSYS + "' WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //���
                        sqlComm.CommandText = "UPDATE ���������ܱ� SET ��ֵ���ID = NULL WHERE (��ֵ���ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();


                        //���
                        for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;


                            //����õ���ÿ����Ʒ���
                            dKUL1 = (-1)*Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                            dKCJE1 = (-1)*Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                            dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                            //�ܿ����
                            sqlComm.CommandText = "SELECT �������, ���ɱ���, ��߽���, ��ͽ���, ���ս���, �����, Ӧ�ս��  FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(5).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(6).ToString());
                            }
                            sqldr.Close();


                            dKUL -= dKUL1;
                            //dKCJE -= dKCJE1;
                            dKCJE = dKUL * dKCCBJ;

                            sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dKUL.ToString() + ", �����=" + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //������ʷ��¼
                            sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ���ﵥ��, ������, ��������, ���ⵥ��, ������, BeActive, �ܽ������, �ܽ����, ����ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'��������Ƶ����', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ","+sBMID+")";
                            sqlComm.ExecuteNonQuery();

                            //�ֿ�����
                            sqlComm.CommandText = "SELECT �������, �����, ���ɱ��� FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                            sqldr = sqlComm.ExecuteReader();

                            if (sqldr.HasRows) //���ڿ��
                            {
                                sqldr.Read();
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString()); //�����
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString()); //���ɱ���
                                sqldr.Close();

                                dKUL -= dKUL1;
                                //dKCJE -= dKCJE1;
                                dKCJE = dKUL * dKCCBJ;

                                sqlComm.CommandText = "UPDATE ���� SET ������� = " + dKUL.ToString() + ", ����� = " + dKCJE.ToString() + " WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                sqlComm.ExecuteNonQuery();


                                //�ⷿ����ʷ��¼
                                sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ���ﵥ��, ������, ��������, ���ⵥ��, ������, BeActive, �ⷿ�������, �ⷿ�����, ����ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'��������Ƶ����', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ","+sBMID+")";
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



                    //MessageBox.Show("�����ﵥ���ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSaved = true;
                    if (MessageBox.Show("�����ﵥ���ɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Close();
                    }
                    break;


                case 1://�޸�

                    //�������
                    if (isSaved)
                    {
                        MessageBox.Show("�����ﵥ�Ѿ��޸�,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    //��ʽȷ��
                    if (!countAmount())
                    {
                        MessageBox.Show("�����ﵥ��ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    //������ϸ
                    dataGridViewDJMX.DataSource = null;
                    for (i = 0; i < iRowCount; i++)
                    {

                        if (!Convert.ToBoolean(dSet.Tables["���ݱ�"].Rows[i][0].ToString())) //������
                        {
                        }
                        else //����
                        {

                            if (dSet.Tables["���ݱ�"].Rows[i][6].ToString() != dSet.Tables["���ݱ�"].Rows[i][18].ToString() || dSet.Tables["���ݱ�"].Rows[i][7].ToString() != dSet.Tables["���ݱ�"].Rows[i][19].ToString()) //���޸�
                            {

                                DataRow drTemp = dSet.Tables["���ݱ�"].NewRow();
                                dSet.Tables["���ݱ�"].Rows.Add(drTemp);

                                for (k = 1; k < dSet.Tables["���ݱ�"].Columns.Count; k++)
                                {
                                    drTemp[k] = dSet.Tables["���ݱ�"].Rows[i][k];
                                }
                                drTemp[0] = 1;



                                dSet.Tables["���ݱ�"].Rows[i][0] = 0;
                                dSet.Tables["���ݱ�"].Rows[i][6] = dSet.Tables["���ݱ�"].Rows[i][18];
                                dSet.Tables["���ݱ�"].Rows[i][7] = dSet.Tables["���ݱ�"].Rows[i][19];

                            }


                        }
                    }
                    dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];
                    //dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[15].Visible = false;
                    dataGridViewDJMX.Columns[16].Visible = false;
                    dataGridViewDJMX.Columns[3].ReadOnly = true;
                    dataGridViewDJMX.Columns[8].ReadOnly = true;
                    dataGridViewDJMX.Columns[9].ReadOnly = true;
                    dataGridViewDJMX.Columns[10].ReadOnly = true;
                    dataGridViewDJMX.Columns[11].ReadOnly = true;
                    dataGridViewDJMX.Columns[14].ReadOnly = true;
                    dataGridViewDJMX.Columns[17].ReadOnly = true;
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
                    dataGridViewDJMX.ShowCellErrors = true;
                    dataGridViewDJMX.Columns[18].ReadOnly = true;
                    dataGridViewDJMX.Columns[19].ReadOnly = true;

                    for (i = 0; i < dataGridViewDJMX.Columns.Count; i++)
                    {
                        dataGridViewDJMX.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }




                    //��ʽȷ��
                    if (!countAmount())
                    {
                        MessageBox.Show("�����ﵥ��ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        //������
                        sqlComm.CommandText = "UPDATE ���������ܱ� SET ��� = " + labelJEHJ.Text + ", ��˰�ϼ� = " + labelJEHJ.Text + ", δ������ = 0, �Ѹ����� = 0 WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();


                        //��ϸ&��� ԭ����
                        for (i = 0; i < iRowCount; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;

                            if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value.ToString())) //ɾ��
                            {

                                //����õ���ÿ����Ʒ���
                                dKUL1 = (-1) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                                dKCJE1 = (-1) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                                dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                                //�ܿ����
                                sqlComm.CommandText = "SELECT �������, ���ɱ���, ��߽���, ��ͽ���, ���ս���, �����, Ӧ�ս��  FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                                sqldr = sqlComm.ExecuteReader();
                                while (sqldr.Read())
                                {
                                    dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                    dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                    dKCJE = Convert.ToDecimal(sqldr.GetValue(5).ToString());
                                    dYSYE = Convert.ToDecimal(sqldr.GetValue(6).ToString());
                                }
                                sqldr.Close();


                                dKUL -= dKUL1;
                                //dKCJE -= dKCJE1;
                                dKCJE = dKUL * dKCCBJ;

                                sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dKUL.ToString() + ", �����=" + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                                sqlComm.ExecuteNonQuery();

                                //������ʷ��¼
                                sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ���ﵥ��, ������, ��������, ���ⵥ��, ������, BeActive, �ܽ������, �ܽ����, ����ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'��������Ƶ��޸�', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ","+sBMID+")";
                                sqlComm.ExecuteNonQuery();

                                //�ֿ�����
                                sqlComm.CommandText = "SELECT �������, �����, ���ɱ��� FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                sqldr = sqlComm.ExecuteReader();

                                if (sqldr.HasRows) //���ڿ��
                                {
                                    sqldr.Read();
                                    dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                    dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString()); //�����
                                    dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString()); //���ɱ���
                                    sqldr.Close();

                                    dKUL -= dKUL1;
                                    //dKCJE -= dKCJE1;
                                    dKCJE = dKUL * dKCCBJ;

                                    sqlComm.CommandText = "UPDATE ���� SET ������� = " + dKUL.ToString() + ", ����� = " + dKCJE.ToString() + " WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                    sqlComm.ExecuteNonQuery();


                                    //�ⷿ����ʷ��¼
                                    sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ���ﵥ��, ������, ��������, ���ⵥ��, ������, BeActive, �ⷿ�������, �ⷿ�����, ����ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'��������Ƶ��޸�', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ","+sBMID+")";
                                    sqlComm.ExecuteNonQuery();
                                }
                                else
                                    sqldr.Close();






                                sqlComm.CommandText = "DELETE FROM ���������ϸ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[17].Value.ToString() + ")";
                                sqlComm.ExecuteNonQuery();
                            }
                        }

                        //������ϸ
                        for (i = iRowCount; i < dataGridViewDJMX.Rows.Count; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;


                            sqlComm.CommandText = "INSERT INTO ���������ϸ�� (��ID, ��ƷID, �ⷿID, ����, ����, ���, ���ɱ���, ������, ��ע, BeActive) VALUES (" + intDJID.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + "', 1)";
                            sqlComm.ExecuteNonQuery();


                            //����õ���ÿ����Ʒ���
                            dKUL1 =  Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                            dKCJE1 =  Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                            dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                            //�ܿ����
                            sqlComm.CommandText = "SELECT �������, ���ɱ���, ��߽���, ��ͽ���, ���ս���, �����, Ӧ�ս��  FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(5).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(6).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                            }
                            sqldr.Close();


                            dKUL -= dKUL1;
                            //dKCJE -= dKCJE1;
                            dKCJE = dKUL * dKCCBJ;

                            sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dKUL.ToString() + ", �����=" + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //������ʷ��¼
                            sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ���ﵥ��, ������, ��������, ���ⵥ��, ������, BeActive, �ܽ������, �ܽ����, ����ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'��������Ƶ��޸�', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ","+sBMID+")";
                            sqlComm.ExecuteNonQuery();

                            //�ֿ�����
                            sqlComm.CommandText = "SELECT �������, �����, ���ɱ��� FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                            sqldr = sqlComm.ExecuteReader();

                            if (sqldr.HasRows) //���ڿ��
                            {
                                sqldr.Read();
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString()); //�����
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString()); //���ɱ���
                                sqldr.Close();

                                dKUL -= dKUL1;
                                //dKCJE -= dKCJE1;
                                dKCJE = dKUL * dKCCBJ;
                                sqlComm.CommandText = "UPDATE ���� SET ������� = " + dKUL.ToString() + ", ����� = " + dKCJE.ToString() + " WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                sqlComm.ExecuteNonQuery();


                                //�ⷿ����ʷ��¼
                                sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ���ﵥ��, ������, ��������, ���ⵥ��, ������, BeActive, �ⷿ�������, �ⷿ�����, ����ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'��������Ƶ��޸�', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ","+sBMID+")";
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
                        return;
                    }
                    finally
                    {
                        sqlConn.Close();
                    }


                    //MessageBox.Show("�����ﵥ�޸ĳɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSaved = true;
                    if (MessageBox.Show("�����ﵥ�޸ĳɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Close();
                    }

                    break;





            }
 
        }

        private void FormKCJWCKDJ_EDIT_FormClosing(object sender, FormClosingEventArgs e)
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
                MessageBox.Show("�����ﵥ��ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "�����ﵥ(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";������ϼƣ�" + labelCKJE.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //��ʽȷ��
            if (!countAmount())
            {
                MessageBox.Show("�����ﵥ��ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "�����ﵥ(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";������ϼƣ�" + labelCKJE.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void ReturntoolStripButton_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;
            int i, j, k;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;


            string strDateSYS;
            cGetInformation.getSystemDateTime();
            strDateSYS = cGetInformation.strSYSDATATIME;


                    //�������
                    if (isSaved)
                    {
                        MessageBox.Show("�����ﵥ�Ѿ�������,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    //��ʽȷ��
                    if (!countAmount())
                    {
                        MessageBox.Show("�����ﵥ��ϸ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        //������
                        sqlComm.CommandText = "UPDATE ���������ܱ� SET BeActive = 1, ��ֵ���ID= " + intDJID .ToString()+ " WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();


                        //���
                        for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;


                            //����õ���ÿ����Ʒ���
                            dKUL1 = (-1) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                            dKCJE1 = (-1) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                            dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                            //�ܿ����
                            sqlComm.CommandText = "SELECT �������, ���ɱ���, ��߽���, ��ͽ���, ���ս���, �����, Ӧ�ս��  FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(5).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(6).ToString());
                            }
                            sqldr.Close();


                            dKUL -= dKUL1;
                            //dKCJE -= dKCJE1;
                            dKCJE = dKUL * dKCCBJ;

                            sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dKUL.ToString() + ", �����=" + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //������ʷ��¼
                            sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ���ﵥ��, ������, ��������, ���ⵥ��, ������, BeActive, �ܽ������, �ܽ����) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'��������Ƶ����', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //�ֿ�����
                            sqlComm.CommandText = "SELECT �������, �����, ���ɱ��� FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                            sqldr = sqlComm.ExecuteReader();

                            if (sqldr.HasRows) //���ڿ��
                            {
                                sqldr.Read();
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString()); //�����
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString()); //���ɱ���
                                sqldr.Close();

                                dKUL -= dKUL1;
                                //dKCJE -= dKCJE1;
                                dKCJE = dKUL * dKCCBJ;

                                sqlComm.CommandText = "UPDATE ���� SET ������� = " + dKUL.ToString() + ", ����� = " + dKCJE.ToString() + " WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                sqlComm.ExecuteNonQuery();


                                //�ⷿ����ʷ��¼
                                sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, ��������, ���ﵥ��, ������, ��������, ���ⵥ��, ������, BeActive, �ⷿ�������, �ⷿ�����) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'��������Ƶ����', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ")";
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
                        return;
                    }
                    finally
                    {
                        sqlConn.Close();
                    }



                    //MessageBox.Show("�����ﵥ���ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSaved = true;
                    if (MessageBox.Show("�����ﵥ��ֳɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Close();
                    }

        }


    }
}