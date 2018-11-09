using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using System.Data.OleDb;

namespace business
{
    public partial class FormDWDAWH : Form
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

        private DataView dvSelect;

        private int iSupplyCompany = 0;

        public FormDWDAWH()
        {
            InitializeComponent();
        }

        private void FormDWDAWH_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            sqlConn.Open();

            //��ʼ������

            sqlComm.CommandText = "SELECT ���� FROM ������ ORDER BY ����";
            if (dSet.Tables.Contains("��������")) dSet.Tables.Remove("��������");
            sqlDA.Fill(dSet, "��������");

            comboBoxDQ.DataSource = dSet.Tables["��������"];
            comboBoxDQ.DisplayMember = "����";

            sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ������, �Ƿ����, �Ƿ�����, ˰��, �绰, ��������, �����˺�, ��ϵ��, ��ַ, ��������, ��ҵ����, ����, �ʱ�, ��ע, ��¼����, ��ϵ��ַ, �ջ���, ҵ��Ա, ��վ����, ����ID, ��Ʊ�绰, �ջ��绰 FROM ��λ�� WHERE (BeActive = 1) ORDER BY ��λ���";

            if (dSet.Tables.Contains("��λ��")) dSet.Tables["��λ��"].Clear();
            sqlDA.Fill(dSet, "��λ��");

            sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ������, �Ƿ����, �Ƿ�����, ˰��, �绰, ��������, �����˺�, ��ϵ��, ��ַ, ��������, ��ҵ����, ����, �ʱ�, ��ע, ��¼����, ��ϵ��ַ, �ջ���, ҵ��Ա, ��վ����, ����ID, ��Ʊ�绰, �ջ��绰 FROM ��λ�� WHERE (ID = 0) ORDER BY ��λ���";

            if (dSet.Tables.Contains("��λ��1")) dSet.Tables.Remove("��λ��1");
            sqlDA.Fill(dSet, "��λ��1");

            dvSelect = new DataView(dSet.Tables["��λ��"]);
            dataGridViewDJMX.DataSource = dvSelect;
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[22].Visible = false;
            setSTAUS();
            sqlConn.Close();

            //initDataView();

        }

        private void setSTAUS()
        {
            toolStripStatusLabelC.Text="��λ����:"+dataGridViewDJMX.RowCount.ToString();
        }

        private void initDataView(int iSel)
        {
            
            //��ʼ���б�
            sqlConn.Open();

            sqlComm.CommandText = "SELECT DISTINCT �������� FROM ��λ�� ORDER BY ��������";
            if (dSet.Tables.Contains("��������")) dSet.Tables.Remove("��������");
            sqlDA.Fill(dSet, "��������");

            comboBoxDQ.DataSource = dSet.Tables["��������"];
            comboBoxDQ.DisplayMember = "��������";

            sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ������, �Ƿ����, �Ƿ�����, ˰��, �绰, ��������, �����˺�, ��ϵ��, ��ַ, ��������, ��ҵ����, ����, �ʱ�, ��ע, ��¼����, ��ϵ��ַ, �ջ���, ҵ��Ա, ��վ����, ����ID, ��Ʊ�绰, �ջ��绰 FROM ��λ�� WHERE (BeActive = 1) ORDER BY ��λ���";

            if (dSet.Tables.Contains("��λ��")) dSet.Tables["��λ��"].Clear();
            sqlDA.Fill(dSet, "��λ��");
            sqlConn.Close();

            setSTAUS();

            if (dataGridViewDJMX.Rows.Count < 1)
                return;
            if (iSel != 0)
            {
                dataGridViewDJMX.Rows[0].Selected = false;
                int iRow = -1;

                for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                   if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == iSel.ToString())
                   {
                       iRow = i;
                       break;
                    }
                }


                if (iRow != -1)
                {
                    dataGridViewDJMX.Rows[iRow].Selected = true;
                    dataGridViewDJMX.FirstDisplayedScrollingRowIndex = iRow;
                }


            }

        }


        private void ToolStripButtonADD_Click(object sender, EventArgs e)
        {


            dSet.Tables["��λ��1"].Clear();
            DataTable dt = dSet.Tables["��λ��1"];

            FormDWDAWH_CARD frmDWDAWH_CARD = new FormDWDAWH_CARD();
            frmDWDAWH_CARD.strConn = strConn;
            frmDWDAWH_CARD.dt = dt;
            frmDWDAWH_CARD.iStyle = 0;

            frmDWDAWH_CARD.ShowDialog();
            initDataView(frmDWDAWH_CARD.iSelect);
        }

        private void toolStripButtonEDIT_Click(object sender, EventArgs e)
        {
            object[] oT = new object[dataGridViewDJMX.ColumnCount];

            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵĵ�λ", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dSet.Tables["��λ��1"].Clear();
            DataTable dt = dSet.Tables["��λ��1"];

            for (int i = dataGridViewDJMX.SelectedRows.Count-1; i >=0; i--)
            {
                for(int j=0;j<oT.Length;j++)
                    oT[j]= dataGridViewDJMX.SelectedRows[i].Cells[j].Value;
                dt.Rows.Add(oT);
            }

            FormDWDAWH_CARD frmDWDAWH_CARD = new FormDWDAWH_CARD();
            frmDWDAWH_CARD.strConn = strConn;
            frmDWDAWH_CARD.dt = dt;
            frmDWDAWH_CARD.iStyle = 1;

            frmDWDAWH_CARD.ShowDialog();
            initDataView(frmDWDAWH_CARD.iSelect);

        }

        private void toolStripButtonDEL_Click(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫɾ���ĵ�λ", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            
            if (MessageBox.Show("�Ƿ�ɾ����ѡ���ݣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            int i;
            System.Data.SqlClient.SqlTransaction sqlta;

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                for (i = 0; i < dataGridViewDJMX.SelectedRows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                    sqlComm.CommandText = "UPDATE ��λ�� SET BeActive = 0 WHERE (ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
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
              MessageBox.Show("ɾ�����", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
              initDataView(0);
 
        }

        private void dataGridViewDJMX_DoubleClick(object sender, EventArgs e)
        {
            toolStripButtonEDIT_Click(null, null);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT= Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");

            string strT = "��λ����;��ǰ���ڣ�" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true,intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");

            string strT = "��λ����;��ǰ���ڣ�" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false,intUserLimit);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            dvSelect.RowFilter = "";
            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (textBoxMC.Text.Trim() == "")
                return;

            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
            if (radioButtonAll.Checked)
            {
                dvSelect.RowFilter = "��λ���� LIKE '%" + textBoxMC.Text.Trim() + "%'";
            }
            if (radioButtonF.Checked)
            {
                dvSelect.RowFilter = "��λ���� LIKE '" + textBoxMC.Text.Trim() + "%'";
            }
            if (radioButtonE.Checked)
            {
                dvSelect.RowFilter = "��λ���� LIKE '%" + textBoxMC.Text.Trim() + "'"; 
            }
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            if (textBoxMC.Text.Trim() == "")
                return;

            int iRow = -1;
            string sTemp = "";

            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (radioButtonAll.Checked)  //ȫƥ��
                {
                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString();
                    if (sTemp.IndexOf(textBoxMC.Text.Trim()) != -1)
                    {
                        iRow = i;
                        break;
                    }
                }

                if (radioButtonF.Checked) //ǰƥ��
                {
                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString();
                    if (sTemp.IndexOf(textBoxMC.Text.Trim()) == 0)
                    {
                        iRow = i;
                        break;
                    }
                }

                if (radioButtonE.Checked) //��ƥ��
                {
                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString().Trim();
                    if (sTemp.Length < textBoxMC.Text.Trim().Length)
                        break;

                    if (sTemp.LastIndexOf(textBoxMC.Text.Trim()) == sTemp.Length - textBoxMC.Text.Trim().Length)
                    {
                        iRow = i;
                        break;
                    }
                }


            }


            if (iRow != -1)
            {
                //dataGridViewDWLB.Rows[iRow].Selected = false;
                dataGridViewDJMX.Rows[iRow].Selected = true;
                dataGridViewDJMX.FirstDisplayedScrollingRowIndex = iRow;
            }
            else
            {
                if (dataGridViewDJMX.Rows.Count > 0)
                {
                    dataGridViewDJMX.Rows[0].Selected = true;
                    dataGridViewDJMX.FirstDisplayedScrollingRowIndex = 0;
                }
            }

        }

        private void btnDQ_Click(object sender, EventArgs e)
        {
            if (comboBoxDQ.Text.Trim() == "")
                return;

            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
            dvSelect.RowFilter = "�������� LIKE '%" + comboBoxDQ.Text.Trim() + "%'";
        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.F9)
            {
                btnAll_Click(null, null);
                return true;
            }
            if (keyData == Keys.F7)
            {
                btnSearch_Click(null, null);
                return true;
            }
            if (keyData == Keys.F8)
            {
                btnLocation_Click(null, null);
                return true;
            }
            if (keyData == Keys.F10)
            {
                btnDQ_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            int i, j;
            int istart, iend, itemp;
            string sSFXS, sSFJH;
            string strDateSYS = "2000-1-1";
            string sSyn="0";
            DateTime dtTemp;
            DataSet dsCSV = new DataSet();
            bool bSyn = true;

            OpenFileDialog openFileDialogOutput = new OpenFileDialog();
            openFileDialogOutput.Filter = "EXCEL files(*.xls)|*.xls|2007 EXCEL files(*.xlss)|*.xlss";//
            openFileDialogOutput.FilterIndex = 0;
            openFileDialogOutput.RestoreDirectory = true;

            if (openFileDialogOutput.ShowDialog() != DialogResult.OK) return;

            string FullFileName = openFileDialogOutput.FileName.ToString();
            FileInfo info = new FileInfo(FullFileName);


            //string strOledbConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + FullFileName + ";" + "Extended Properties=Excel 8.0;";
            string strOledbConn = "provider=Microsoft.Jet.OLEDB.4.0;" + "data source=" + FullFileName + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
            System.Data.OleDb.OleDbConnection oledbConn = new System.Data.OleDb.OleDbConnection(strOledbConn);

            if (MessageBox.Show("�Ƿ���Ҫͬ����λ��Ϣ���������ͬ������������ݿ�ͬ����λ����Ϣ�޸ģ����򽫽����µ�λ", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                bSyn = false;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //�õ�����������
                sqlComm.CommandText = "SELECT GETDATE() AS ����";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                sqldr.Close();

                oledbConn.Open();
                string strExcel = "";
                System.Data.OleDb.OleDbDataAdapter oledbDataAdapter = null;

                DataTable dt = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string tableName = dt.Rows[0][2].ToString().Trim();
                strExcel = "select * from [" + tableName + "]";

                oledbDataAdapter = new System.Data.OleDb.OleDbDataAdapter(strExcel, oledbConn);


                oledbDataAdapter.Fill(dsCSV, "��Ϣ");
                oledbConn.Close();
                int rowCount = Convert.ToInt32(dsCSV.Tables["��Ϣ"].Rows.Count.ToString());
                int colCount = Convert.ToInt32(dsCSV.Tables["��Ϣ"].Columns.Count.ToString());

                for (i = 0; i < rowCount; i++)
                {
                    sSFXS = dsCSV.Tables["��Ϣ"].Rows[i][4].ToString();
                    if (sSFXS == "��") sSFXS = "1";
                    else sSFXS = "0";

                    sSFJH = dsCSV.Tables["��Ϣ"].Rows[i][3].ToString();
                    if (sSFJH == "��") sSFJH = "1";
                    else sSFJH = "0";

                    if (bSyn) //ͬ������
                    {
                        sqlComm.CommandText = "SELECT ID FROM ��λ�� WHERE (��λ���� = N'" + dsCSV.Tables["��Ϣ"].Rows[i][1].ToString() + "')";
                        sqldr = sqlComm.ExecuteReader();
                        if (sqldr.HasRows) //���ظ�
                        {
                            sqldr.Read();
                            sSyn = sqldr.GetValue(0).ToString();
                            sqldr.Close();

                            sqlComm.CommandText = "UPDATE ��λ�� SET ��λ��� = '" + dsCSV.Tables["��Ϣ"].Rows[i][0].ToString() + "', ��λ���� = N'" + dsCSV.Tables["��Ϣ"].Rows[i][1].ToString() + "', ������ = '" + dsCSV.Tables["��Ϣ"].Rows[i][2].ToString() + "', �Ƿ���� = " + sSFJH + ", �Ƿ����� = " + sSFXS + ", ˰�� = N'" + dsCSV.Tables["��Ϣ"].Rows[i][5].ToString() + "',  �绰 = '" + dsCSV.Tables["��Ϣ"].Rows[i][6].ToString() + "', �������� = N'" + dsCSV.Tables["��Ϣ"].Rows[i][7].ToString() + "', �����˺� = '" + dsCSV.Tables["��Ϣ"].Rows[i][8].ToString() + "', ��ϵ�� = N'" + dsCSV.Tables["��Ϣ"].Rows[i][9].ToString() + "', ��ַ = N'" + dsCSV.Tables["��Ϣ"].Rows[i][10].ToString() + "', �������� = N'" + dsCSV.Tables["��Ϣ"].Rows[i][11].ToString() + "', ��ҵ���� = N'" + dsCSV.Tables["��Ϣ"].Rows[i][12].ToString() + "', ���� = N'" + dsCSV.Tables["��Ϣ"].Rows[i][13].ToString() + "', �ʱ� = '" + dsCSV.Tables["��Ϣ"].Rows[i][14].ToString() + "', ��ע = N'" + dsCSV.Tables["��Ϣ"].Rows[i][15].ToString() + "', ��ϵ��ַ = N'" + dsCSV.Tables["��Ϣ"].Rows[i][16].ToString() + "' WHERE (ID = "+sSyn+")";
                            sqlComm.ExecuteNonQuery();
                            
                        }
                        else
                        {
                            sqldr.Close();

                            sqlComm.CommandText = "INSERT INTO ��λ�� (��λ���, ��λ����, ������, �Ƿ����, �Ƿ�����, ˰��, �绰, ��������, �����˺�, ��ϵ��, ��ַ, ��������, ��ҵ����, ����, �ʱ�, ��ע, ��¼����, ��ϵ��ַ, Ӧ���˿�, Ӧ���˿�, BeActive) VALUES ('" + dsCSV.Tables["��Ϣ"].Rows[i][0].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][1].ToString() + "', '" + dsCSV.Tables["��Ϣ"].Rows[i][2].ToString() + "', " + sSFJH + ", " + sSFXS + ", N'" + dsCSV.Tables["��Ϣ"].Rows[i][5].ToString() + "', '" + dsCSV.Tables["��Ϣ"].Rows[i][6].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][7].ToString() + "', '" + dsCSV.Tables["��Ϣ"].Rows[i][8].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][9].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][10].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][11].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][12].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][13].ToString() + "', '" + dsCSV.Tables["��Ϣ"].Rows[i][14].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][16].ToString() + "', '" + strDateSYS + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][16].ToString() + "', 0, 0, 1)";
                            sqlComm.ExecuteNonQuery();
                        }
                         
                    }
                    else //������ͬ��
                    {
                        sqlComm.CommandText = "INSERT INTO ��λ�� (��λ���, ��λ����, ������, �Ƿ����, �Ƿ�����, ˰��, �绰, ��������, �����˺�, ��ϵ��, ��ַ, ��������, ��ҵ����, ����, �ʱ�, ��ע, ��¼����, ��ϵ��ַ, Ӧ���˿�, Ӧ���˿�, BeActive) VALUES ('" + dsCSV.Tables["��Ϣ"].Rows[i][0].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][1].ToString() + "', '" + dsCSV.Tables["��Ϣ"].Rows[i][2].ToString() + "', " + sSFJH + ", " + sSFXS + ", N'" + dsCSV.Tables["��Ϣ"].Rows[i][5].ToString() + "', '" + dsCSV.Tables["��Ϣ"].Rows[i][6].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][7].ToString() + "', '" + dsCSV.Tables["��Ϣ"].Rows[i][8].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][9].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][10].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][11].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][12].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][13].ToString() + "', '" + dsCSV.Tables["��Ϣ"].Rows[i][14].ToString() + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][16].ToString() + "', '" + strDateSYS + "', N'" + dsCSV.Tables["��Ϣ"].Rows[i][16].ToString() + "', 0, 0, 1)";
                        sqlComm.ExecuteNonQuery();
                    }


                }
                sqlta.Commit();

            }
            catch (Exception ex)
            {
                MessageBox.Show("���ݵ���ʧ�ܣ����������ļ���" + ex.Message.ToString(), "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //throw ex;
            }
            finally
            {
                sqlConn.Close();
            }
            initDataView(0);
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
                textBoxMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
            }
            selectCompany();
        }

        private void selectCompany()
        {
            if (iSupplyCompany == 0)
                return;
            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
            dvSelect.RowFilter = "ID =" + iSupplyCompany.ToString() ;
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(10, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                }
            }
            selectCompany();
        }

        private void textBoxMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(12, textBoxMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                }
            }
            selectCompany();
        }
    }
}