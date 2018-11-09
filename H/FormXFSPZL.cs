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
    public partial class FormXFSPZL : Form
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

        public int intKFID = 0;
        
        public FormXFSPZL()
        {
            InitializeComponent();
        }

        private void FormXFSPZL_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;


            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            toolStripComboBoxLB.SelectedIndex = 0;
            toolStripButtonGD_Click(null, null);
        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(1, "") == 0)
            {
                //return;
            }
            else
            {
                intKFID = cGetInformation.iKFNumber;
                textBoxKFMC.Text = cGetInformation.strKFName;
                textBoxKFBH.Text = cGetInformation.strKFCode;
                initKFView();

            }
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFBH.Text) == 0)
                {
                    intKFID = 0;
                    textBoxKFMC.Text = "";
                    textBoxKFBH.Text = "";
                    //return;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    initKFView();

                }
            }
        }

        private void textBoxKFWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFMC.Text) == 0)
                {
                    intKFID = 0;
                    textBoxKFMC.Text = "";
                    textBoxKFBH.Text = "";
                    //return;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    initKFView();
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            if (intKFID == 0 || toolStripComboBoxLB.SelectedIndex == 0)
            {
                sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ���㸶����.���������� AS �·�����, ��Ʒ��.�������, ��Ʒ��.���ɱ���, ��Ʒ��.�����, ��Ʒ��.ID FROM ��Ʒ�� CROSS JOIN ���㸶���� WHERE (��Ʒ��.beactive = 1)";
            }
            else
            {
                sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ���㸶����.���������� AS �·�����, ��Ʒ��.�������, ��Ʒ��.���ɱ���, ��Ʒ��.�����, ��Ʒ��.ID FROM ��Ʒ�� INNER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID CROSS JOIN ���㸶���� WHERE (��Ʒ��.beactive = 1) AND (��Ʒ�����.�ⷿID = " + intKFID.ToString() + ")";
            }

            
            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");

            dataGridView2.DataSource = dSet.Tables["��Ʒ��"];

            dataGridView2.Columns[7].Visible = false;
            dataGridView2.Columns[0].ReadOnly = false;
            dataGridView2.Columns[1].ReadOnly = false;
            dataGridView2.Columns[2].ReadOnly = false;
            dataGridView2.Columns[4].ReadOnly = false;
            dataGridView2.Columns[5].ReadOnly = false;
            dataGridView2.Columns[6].ReadOnly = false;



            sqlConn.Close();

        }

        private void initKFView()
        {
            if(intKFID==0)
            {
                MessageBox.Show("�����·��ⷿ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sqlConn.Open();

            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ����.�������, ����.�����,  ��Ʒ��.ID FROM ���� INNER JOIN  ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID WHERE (����.�ⷿID = " + intKFID.ToString() + ") AND (����.BeActive = 1)";
            if (dSet.Tables.Contains("����")) dSet.Tables.Remove("����");
            sqlDA.Fill(dSet, "����");

            dataGridView1.DataSource = dSet.Tables["����"];
            dataGridView1.Columns[4].Visible = false;
            sqlConn.Close();
        }

        private void toolStripButtonXF_Click(object sender, EventArgs e)
        {
            int i;
            decimal dTemp1 = 0, dTemp2 = 0;
            decimal fTemp = 0, fTemp1 = 0;
            bool bNo = false;

            if (intKFID == 0)
            {
                MessageBox.Show("�����·��ⷿ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataGridView2.SelectedRows.Count < 1)
            {
                MessageBox.Show("ѡ���·���Ʒ������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //������ϸ
                for (i = 0; i < dataGridView2.SelectedRows.Count; i++)
                {
                    if (dataGridView2.SelectedRows[i].IsNewRow)
                        continue;

                    if (dataGridView2.SelectedRows[i].Cells[3].Value.ToString()=="")
                        dataGridView2.SelectedRows[i].Cells[3].Value=0;
                    if (dataGridView2.SelectedRows[i].Cells[4].Value.ToString()=="")
                        dataGridView2.SelectedRows[i].Cells[4].Value=0;
                    if (dataGridView2.SelectedRows[i].Cells[5].Value.ToString()=="")
                        dataGridView2.SelectedRows[i].Cells[5].Value=0;
                    if (dataGridView2.SelectedRows[i].Cells[6].Value.ToString()=="")
                        dataGridView2.SelectedRows[i].Cells[6].Value=0;

                    //�ܿ��
                    dTemp1 = Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[4].Value.ToString()) + Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[3].Value.ToString());
                    dTemp2 = Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[6].Value.ToString()) + Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[3].Value.ToString()) * Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[5].Value.ToString());


                    sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dTemp1.ToString() + ", ����� = " + dTemp1.ToString() + "*[���ɱ���] WHERE (ID = " + dataGridView2.SelectedRows[i].Cells[7].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //���ķֿ��
                    fTemp = 0; fTemp1 = 0;
                    sqlComm.CommandText = "SELECT �������, ����� FROM ���� WHERE (�ⷿID = " + intKFID.ToString() + ") AND (��ƷID = " + dataGridView2.SelectedRows[i].Cells[7].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    //���ɱ���
                    bNo = false;
                    while (sqldr.Read())
                    {
                        fTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        fTemp1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        bNo = true;
                    }
                    sqldr.Close();

                    dTemp1 = fTemp + Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[3].Value.ToString());
                    dTemp2 = fTemp1 + Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[3].Value.ToString()) * Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[5].Value.ToString());

                    if(!bNo) //û�п��
                        sqlComm.CommandText = "INSERT INTO ���� (�ⷿID, ��ƷID, �������, �����, ���ɱ���, BeActive, �������, �������, ����������, ����������) VALUES (" + intKFID.ToString() + ", " + dataGridView2.SelectedRows[i].Cells[7].Value.ToString() + ", " + dTemp1.ToString() + ", " + dTemp1.ToString() + "*" + dataGridView2.SelectedRows[i].Cells[5].Value.ToString() + ", " + dataGridView2.SelectedRows[i].Cells[5].Value.ToString() + ", 1, 0, 0, 0, 0)";
                    else //���ڿ��
                        sqlComm.CommandText = "UPDATE ���� SET ������� = " + dTemp1.ToString() + ", ����� = " + dTemp1.ToString() + "*[���ɱ���] WHERE (�ⷿID = " + intKFID.ToString() + ") AND (��ƷID = " + dataGridView2.SelectedRows[i].Cells[7].Value.ToString() + ")";
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


            MessageBox.Show("��Ʒ�·ŵ��ⷿ���", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initKFView();
            toolStripButtonGD_Click(null, null);
        }

        private void toolStripButtonSC_Click(object sender, EventArgs e)
        {
            int i;
            decimal dTemp1 = 0, dTemp2 = 0;
            decimal fTemp = 0, fTemp1 = 0;
            bool bNo = false;

            if (intKFID == 0)
            {
                MessageBox.Show("������Ʒ�ⷿ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageBox.Show("ѡ��ɾ����Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //������ϸ
                for (i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    if (dataGridView1.SelectedRows[i].IsNewRow)
                        continue;

                    if (dataGridView1.SelectedRows[i].Cells[2].Value.ToString() == "")
                        dataGridView1.SelectedRows[i].Cells[2].Value = 0;
                    if (dataGridView1.SelectedRows[i].Cells[3].Value.ToString() == "")
                        dataGridView1.SelectedRows[i].Cells[3].Value = 0;

                    if (dataGridView1.SelectedRows[i].Cells[2].Value.ToString() != "0")
                    {
                        MessageBox.Show("��Ʒ" + dataGridView1.SelectedRows[i].Cells[1].Value.ToString() + "��治Ϊ0,�޷�ɾ��");
                        continue;
                    }

                    sqlComm.CommandText = "DELETE ���� WHERE (�ⷿID = " + intKFID.ToString() + ") AND (��ƷID = " + dataGridView1.SelectedRows[i].Cells[4].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    /*

                    //�ֿ��
                    sqlComm.CommandText = "UPDATE ���� SET ������� = 0, ����� = 0, BeActive = 0 WHERE (�ⷿID = " + intKFID.ToString() + ") AND (��ƷID = " + dataGridView1.SelectedRows[i].Cells[4].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();


                    //�����ܿ��
                    fTemp = 0; fTemp1 = 0;
                    sqlComm.CommandText = "SELECT �������, ����� FROM ��Ʒ�� WHERE (ID = " + dataGridView1.SelectedRows[i].Cells[4].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    //���ɱ���
                    while (sqldr.Read())
                    {
                        fTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        fTemp1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                    }
                    sqldr.Close();

                    dTemp1 = fTemp - Convert.ToDecimal(dataGridView1.SelectedRows[i].Cells[2].Value.ToString());
                    dTemp2 = fTemp1 - Convert.ToDecimal(dataGridView1.SelectedRows[i].Cells[3].Value.ToString());

                    sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dTemp1.ToString() + ", ����� = " + dTemp2.ToString() + " WHERE (ID = " + dataGridView1.SelectedRows[i].Cells[4].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                    */

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


            MessageBox.Show("�ⷿ��Ʒɾ�����", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initKFView();
 
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�ⷿ��Ʒ�б�;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�ⷿ��Ʒ�б�;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
        }

        private void toolStripButtonSPExcel_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("������Ʒ��ʼ���޷���������ֻ�ڳ�ʼ�����ݿ���ʹ��(��֤��Ʒ�Ϳ����ڶ��޼�¼)���Ƿ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)!=DialogResult.Yes)
                return;

            int i;
            string strDateSYS = "2000-1-1";
            DataSet dsCSV = new DataSet();
            string sID = "";

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


                if (dsCSV.Tables.Contains("��Ϣ")) dSet.Tables.Remove("��Ϣ");
                oledbDataAdapter.Fill(dsCSV, "��Ϣ");
                oledbConn.Close();
                int rowCount = Convert.ToInt32(dsCSV.Tables["��Ϣ"].Rows.Count.ToString());
                int colCount = Convert.ToInt32(dsCSV.Tables["��Ϣ"].Columns.Count.ToString());

                string sFLBH, sSPBH, sSPMC, sSPGG, sZJM;
                int iSX=0,iXX=0,iHLSX=0,iHLXX=0;
                decimal dJJ=0,dPFJ=0,dKC=0,dKCJE=0,dKCCBJ=0;
                int iFLID=0,iKFID=0;
                

                for (i = 0; i < rowCount; i++)
                {
                    //������
                    if (dsCSV.Tables["��Ϣ"].Rows[i][0].ToString() == "")
                        continue;

                    sqlComm.CommandText = "SELECT ID, �ⷿID FROM ��Ʒ����� WHERE (������ = N'"+dsCSV.Tables["��Ϣ"].Rows[i][0].ToString()+"') AND (BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (!sqldr.HasRows)
                    {
                        sqldr.Close();
                        continue;
                    }

                    while (sqldr.Read())
                    {
                        iFLID = int.Parse(sqldr.GetValue(0).ToString());
                        iKFID = int.Parse(sqldr.GetValue(1).ToString());
                        break;
                    }
                    sqldr.Close();

                    sSPBH = dsCSV.Tables["��Ϣ"].Rows[i][1].ToString().Trim();
                    sSPMC = dsCSV.Tables["��Ϣ"].Rows[i][2].ToString().Trim();
                    sSPGG = dsCSV.Tables["��Ϣ"].Rows[i][3].ToString().Trim();
                    sZJM = dsCSV.Tables["��Ϣ"].Rows[i][4].ToString().Trim();


                    if (sSPBH.Trim() == "" || sSPMC.Trim() == "")
                        continue;

                    if (dsCSV.Tables["��Ϣ"].Rows[i][5].ToString().Trim() == "")
                        iSX = 0;
                    else
                        iSX = int.Parse(dsCSV.Tables["��Ϣ"].Rows[i][5].ToString().Trim());

                    if (dsCSV.Tables["��Ϣ"].Rows[i][6].ToString().Trim() == "")
                        iXX = 0;
                    else
                        iXX = int.Parse(dsCSV.Tables["��Ϣ"].Rows[i][6].ToString().Trim());

                    if (dsCSV.Tables["��Ϣ"].Rows[i][7].ToString().Trim() == "")
                        iHLSX = 0;
                    else
                        iHLSX = int.Parse(dsCSV.Tables["��Ϣ"].Rows[i][7].ToString().Trim());

                    if (dsCSV.Tables["��Ϣ"].Rows[i][8].ToString().Trim() == "")
                        iHLXX = 0;
                    else
                        iHLXX = int.Parse(dsCSV.Tables["��Ϣ"].Rows[i][7].ToString().Trim());

                    if (dsCSV.Tables["��Ϣ"].Rows[i][9].ToString().Trim() == "")
                        dJJ = 0;
                    else
                        dJJ = decimal.Parse(dsCSV.Tables["��Ϣ"].Rows[i][9].ToString().Trim());

                    if (dsCSV.Tables["��Ϣ"].Rows[i][10].ToString().Trim() == "")
                        dPFJ = 0;
                    else
                        dPFJ = decimal.Parse(dsCSV.Tables["��Ϣ"].Rows[i][10].ToString().Trim());

                    if (dsCSV.Tables["��Ϣ"].Rows[i][11].ToString().Trim() == "")
                        dKC = 0;
                    else
                        dKC = decimal.Parse(dsCSV.Tables["��Ϣ"].Rows[i][11].ToString().Trim());

                    if (dsCSV.Tables["��Ϣ"].Rows[i][12].ToString().Trim() == "")
                        dKCCBJ = 0;
                    else
                        dKCCBJ = decimal.Parse(dsCSV.Tables["��Ϣ"].Rows[i][12].ToString().Trim());

                    dKCJE=dKC*dKCCBJ;

                    sqlComm.CommandText = "INSERT INTO ��Ʒ�� (��Ʒ���, ��Ʒ����, ������, ��С������λ, ����, ��˰����, ������, ��˰������, �������, ���ɱ���, �����, ������, ��߽���, ��ͽ���, ���ս���, ��ת����, ��ת����, ��ת���, ��ת����, ��¼����, �������, �������, ����������, ����������, ��װ��Ʒ, beactive, Ӧ�����, �Ѹ����, Ӧ�ս��, ���ս��, ������, ��Ʒ���) VALUES (N'" + sSPBH + "', N'" + sSPMC + "', N'" + sZJM + "', null, " + dJJ.ToString() + ", " + dJJ.ToString() + ", " + dPFJ.ToString() + ", " + dPFJ.ToString() + ", " + dKC.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE.ToString() + ", 0, 0, 0, 0, 0, 0, 0, 0, '" + strDateSYS + "', " + iSX.ToString() + ", " + iXX.ToString() + ", " + iHLSX.ToString() + ", " + iHLXX.ToString() + ", 0, 1, 0, 0, 0, 0, " + iFLID.ToString() + ", N'" + sSPGG + "')";

                    sqlComm.ExecuteNonQuery();

                    //ȡ�ú� 
                    sqlComm.CommandText = "SELECT @@IDENTITY";
                    sqldr = sqlComm.ExecuteReader();
                    sqldr.Read();
                    sID = sqldr.GetValue(0).ToString();
                    sqldr.Close();


                    //�·�
                    sqlComm.CommandText = "INSERT INTO ���� (�ⷿID, ��ƷID, �������, �����, ���ɱ���, BeActive, �������, �������, ����������, ����������) VALUES (" + iKFID.ToString() + ", " + sID + ", " + dKC.ToString() + ", " + dKCJE.ToString() + ", " + dKCCBJ.ToString() + ", 1, " + iSX.ToString() + ", " + iXX.ToString() + ", " + iHLSX.ToString() + ", " + iHLXX.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                sqlta.Commit();
                MessageBox.Show("���ݵ�����ϣ��������ݡ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        }

        private void toolStripButtonTMDR_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("��ʼ����Ʒ���뵼�룬��ȷ�����ݿ���û�е�����Ϣ,�Ƿ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;

            int i;
            string strDateSYS = "2000-1-1";
            DataSet dsCSV = new DataSet();
            string sID = "";

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

                if (dsCSV.Tables.Contains("������Ϣ")) dSet.Tables.Remove("������Ϣ");
                oledbDataAdapter.Fill(dsCSV, "������Ϣ");
                oledbConn.Close();
                int rowCount = Convert.ToInt32(dsCSV.Tables["������Ϣ"].Rows.Count.ToString());
                int colCount = Convert.ToInt32(dsCSV.Tables["������Ϣ"].Columns.Count.ToString());

                int iSPID = 0, iKFID = 0;


                for (i = 0; i < rowCount; i++)
                {
                    if (dsCSV.Tables["������Ϣ"].Rows[i][0].ToString().Trim() == "")
                        continue;

                    //������
                    if (dsCSV.Tables["������Ϣ"].Rows[i][1].ToString().Trim() == "")
                        continue;

                    sqlComm.CommandText = "SELECT ��Ʒ��.ID, ��Ʒ��.��Ʒ���, ��Ʒ�����.�ⷿID FROM ��Ʒ�� INNER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID WHERE (��Ʒ��.beactive = 1) AND (��Ʒ�����.BeActive = 1) AND (��Ʒ��.��Ʒ��� = N'" + dsCSV.Tables["������Ϣ"].Rows[i][1].ToString() + "')";
                    sqldr = sqlComm.ExecuteReader();

                    if (!sqldr.HasRows)
                    {
                        sqldr.Close();
                        continue;
                    }

                    while (sqldr.Read())
                    {
                        iSPID = int.Parse(sqldr.GetValue(0).ToString());
                        iKFID = int.Parse(sqldr.GetValue(2).ToString());
                        break;
                    }
                    sqldr.Close();



                    sqlComm.CommandText = "INSERT INTO ��Ʒ����� (����, ��ƷID, �ⷿID, ���ݱ��, ժҪ, ����, �������, ����ԱID, ������ϸID) VALUES (N'" + dsCSV.Tables["������Ϣ"].Rows[i][0].ToString().Trim() + "', " + iSPID.ToString() + ", " + iKFID.ToString() + ", N'000000000', N'�ⷿ��ʼ���뵼��', CONVERT(DATETIME, '" + strDateSYS + " 00:00:00', 102), 0, "+intUserID.ToString()+", 0)";

                    sqlComm.ExecuteNonQuery();

                }

                sqlta.Commit();
                MessageBox.Show("���ݵ�����ϣ��������ݡ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        }
    }
}