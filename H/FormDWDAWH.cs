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

            //初始化地区

            sqlComm.CommandText = "SELECT 地区 FROM 地区表 ORDER BY 地区";
            if (dSet.Tables.Contains("地区名称")) dSet.Tables.Remove("地区名称");
            sqlDA.Fill(dSet, "地区名称");

            comboBoxDQ.DataSource = dSet.Tables["地区名称"];
            comboBoxDQ.DisplayMember = "地区";

            sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 助记码, 是否进货, 是否销售, 税号, 电话, 开户银行, 银行账号, 联系人, 地址, 地区名称, 行业名称, 传真, 邮编, 备注, 登录日期, 联系地址, 收货人, 业务员, 到站名称, 部门ID, 开票电话, 收货电话 FROM 单位表 WHERE (BeActive = 1) ORDER BY 单位编号";

            if (dSet.Tables.Contains("单位表")) dSet.Tables["单位表"].Clear();
            sqlDA.Fill(dSet, "单位表");

            sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 助记码, 是否进货, 是否销售, 税号, 电话, 开户银行, 银行账号, 联系人, 地址, 地区名称, 行业名称, 传真, 邮编, 备注, 登录日期, 联系地址, 收货人, 业务员, 到站名称, 部门ID, 开票电话, 收货电话 FROM 单位表 WHERE (ID = 0) ORDER BY 单位编号";

            if (dSet.Tables.Contains("单位表1")) dSet.Tables.Remove("单位表1");
            sqlDA.Fill(dSet, "单位表1");

            dvSelect = new DataView(dSet.Tables["单位表"]);
            dataGridViewDJMX.DataSource = dvSelect;
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[22].Visible = false;
            setSTAUS();
            sqlConn.Close();

            //initDataView();

        }

        private void setSTAUS()
        {
            toolStripStatusLabelC.Text="单位数量:"+dataGridViewDJMX.RowCount.ToString();
        }

        private void initDataView(int iSel)
        {
            
            //初始化列表
            sqlConn.Open();

            sqlComm.CommandText = "SELECT DISTINCT 地区名称 FROM 单位表 ORDER BY 地区名称";
            if (dSet.Tables.Contains("地区名称")) dSet.Tables.Remove("地区名称");
            sqlDA.Fill(dSet, "地区名称");

            comboBoxDQ.DataSource = dSet.Tables["地区名称"];
            comboBoxDQ.DisplayMember = "地区名称";

            sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 助记码, 是否进货, 是否销售, 税号, 电话, 开户银行, 银行账号, 联系人, 地址, 地区名称, 行业名称, 传真, 邮编, 备注, 登录日期, 联系地址, 收货人, 业务员, 到站名称, 部门ID, 开票电话, 收货电话 FROM 单位表 WHERE (BeActive = 1) ORDER BY 单位编号";

            if (dSet.Tables.Contains("单位表")) dSet.Tables["单位表"].Clear();
            sqlDA.Fill(dSet, "单位表");
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


            dSet.Tables["单位表1"].Clear();
            DataTable dt = dSet.Tables["单位表1"];

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
                MessageBox.Show("请选择要修改的单位", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dSet.Tables["单位表1"].Clear();
            DataTable dt = dSet.Tables["单位表1"];

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
                MessageBox.Show("请选择要删除的单位", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            
            if (MessageBox.Show("是否删除所选内容？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
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

                    sqlComm.CommandText = "UPDATE 单位表 SET BeActive = 0 WHERE (ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                 }

                    sqlta.Commit();
             }
             catch (Exception ex)
             {
                    MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlta.Rollback();
                    return;
              }
              finally
              {
                    sqlConn.Close();
              }
              MessageBox.Show("删除完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            strDT= Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");

            string strT = "单位档案;当前日期：" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true,intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");

            string strT = "单位档案;当前日期：" + strDT;
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
                dvSelect.RowFilter = "单位名称 LIKE '%" + textBoxMC.Text.Trim() + "%'";
            }
            if (radioButtonF.Checked)
            {
                dvSelect.RowFilter = "单位名称 LIKE '" + textBoxMC.Text.Trim() + "%'";
            }
            if (radioButtonE.Checked)
            {
                dvSelect.RowFilter = "单位名称 LIKE '%" + textBoxMC.Text.Trim() + "'"; 
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
                if (radioButtonAll.Checked)  //全匹配
                {
                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString();
                    if (sTemp.IndexOf(textBoxMC.Text.Trim()) != -1)
                    {
                        iRow = i;
                        break;
                    }
                }

                if (radioButtonF.Checked) //前匹配
                {
                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString();
                    if (sTemp.IndexOf(textBoxMC.Text.Trim()) == 0)
                    {
                        iRow = i;
                        break;
                    }
                }

                if (radioButtonE.Checked) //后匹配
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
            dvSelect.RowFilter = "地区名称 LIKE '%" + comboBoxDQ.Text.Trim() + "%'";
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

            if (MessageBox.Show("是否需要同步单位信息？如果采用同步，导入项将数据库同名单位的信息修改，否则将建立新单位", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                bSyn = false;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //得到服务器日期
                sqlComm.CommandText = "SELECT GETDATE() AS 日期";
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


                oledbDataAdapter.Fill(dsCSV, "信息");
                oledbConn.Close();
                int rowCount = Convert.ToInt32(dsCSV.Tables["信息"].Rows.Count.ToString());
                int colCount = Convert.ToInt32(dsCSV.Tables["信息"].Columns.Count.ToString());

                for (i = 0; i < rowCount; i++)
                {
                    sSFXS = dsCSV.Tables["信息"].Rows[i][4].ToString();
                    if (sSFXS == "是") sSFXS = "1";
                    else sSFXS = "0";

                    sSFJH = dsCSV.Tables["信息"].Rows[i][3].ToString();
                    if (sSFJH == "是") sSFJH = "1";
                    else sSFJH = "0";

                    if (bSyn) //同步查重
                    {
                        sqlComm.CommandText = "SELECT ID FROM 单位表 WHERE (单位名称 = N'" + dsCSV.Tables["信息"].Rows[i][1].ToString() + "')";
                        sqldr = sqlComm.ExecuteReader();
                        if (sqldr.HasRows) //有重复
                        {
                            sqldr.Read();
                            sSyn = sqldr.GetValue(0).ToString();
                            sqldr.Close();

                            sqlComm.CommandText = "UPDATE 单位表 SET 单位编号 = '" + dsCSV.Tables["信息"].Rows[i][0].ToString() + "', 单位名称 = N'" + dsCSV.Tables["信息"].Rows[i][1].ToString() + "', 助记码 = '" + dsCSV.Tables["信息"].Rows[i][2].ToString() + "', 是否进货 = " + sSFJH + ", 是否销售 = " + sSFXS + ", 税号 = N'" + dsCSV.Tables["信息"].Rows[i][5].ToString() + "',  电话 = '" + dsCSV.Tables["信息"].Rows[i][6].ToString() + "', 开户银行 = N'" + dsCSV.Tables["信息"].Rows[i][7].ToString() + "', 银行账号 = '" + dsCSV.Tables["信息"].Rows[i][8].ToString() + "', 联系人 = N'" + dsCSV.Tables["信息"].Rows[i][9].ToString() + "', 地址 = N'" + dsCSV.Tables["信息"].Rows[i][10].ToString() + "', 地区名称 = N'" + dsCSV.Tables["信息"].Rows[i][11].ToString() + "', 行业名称 = N'" + dsCSV.Tables["信息"].Rows[i][12].ToString() + "', 传真 = N'" + dsCSV.Tables["信息"].Rows[i][13].ToString() + "', 邮编 = '" + dsCSV.Tables["信息"].Rows[i][14].ToString() + "', 备注 = N'" + dsCSV.Tables["信息"].Rows[i][15].ToString() + "', 联系地址 = N'" + dsCSV.Tables["信息"].Rows[i][16].ToString() + "' WHERE (ID = "+sSyn+")";
                            sqlComm.ExecuteNonQuery();
                            
                        }
                        else
                        {
                            sqldr.Close();

                            sqlComm.CommandText = "INSERT INTO 单位表 (单位编号, 单位名称, 助记码, 是否进货, 是否销售, 税号, 电话, 开户银行, 银行账号, 联系人, 地址, 地区名称, 行业名称, 传真, 邮编, 备注, 登录日期, 联系地址, 应付账款, 应收账款, BeActive) VALUES ('" + dsCSV.Tables["信息"].Rows[i][0].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][1].ToString() + "', '" + dsCSV.Tables["信息"].Rows[i][2].ToString() + "', " + sSFJH + ", " + sSFXS + ", N'" + dsCSV.Tables["信息"].Rows[i][5].ToString() + "', '" + dsCSV.Tables["信息"].Rows[i][6].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][7].ToString() + "', '" + dsCSV.Tables["信息"].Rows[i][8].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][9].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][10].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][11].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][12].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][13].ToString() + "', '" + dsCSV.Tables["信息"].Rows[i][14].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][16].ToString() + "', '" + strDateSYS + "', N'" + dsCSV.Tables["信息"].Rows[i][16].ToString() + "', 0, 0, 1)";
                            sqlComm.ExecuteNonQuery();
                        }
                         
                    }
                    else //不进行同步
                    {
                        sqlComm.CommandText = "INSERT INTO 单位表 (单位编号, 单位名称, 助记码, 是否进货, 是否销售, 税号, 电话, 开户银行, 银行账号, 联系人, 地址, 地区名称, 行业名称, 传真, 邮编, 备注, 登录日期, 联系地址, 应付账款, 应收账款, BeActive) VALUES ('" + dsCSV.Tables["信息"].Rows[i][0].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][1].ToString() + "', '" + dsCSV.Tables["信息"].Rows[i][2].ToString() + "', " + sSFJH + ", " + sSFXS + ", N'" + dsCSV.Tables["信息"].Rows[i][5].ToString() + "', '" + dsCSV.Tables["信息"].Rows[i][6].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][7].ToString() + "', '" + dsCSV.Tables["信息"].Rows[i][8].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][9].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][10].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][11].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][12].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][13].ToString() + "', '" + dsCSV.Tables["信息"].Rows[i][14].ToString() + "', N'" + dsCSV.Tables["信息"].Rows[i][16].ToString() + "', '" + strDateSYS + "', N'" + dsCSV.Tables["信息"].Rows[i][16].ToString() + "', 0, 0, 1)";
                        sqlComm.ExecuteNonQuery();
                    }


                }
                sqlta.Commit();

            }
            catch (Exception ex)
            {
                MessageBox.Show("数据导入失败，请检查数据文件：" + ex.Message.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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