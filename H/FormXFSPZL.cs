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
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
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
                sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 结算付款定义表.将付款数量 AS 下发数量, 商品表.库存数量, 商品表.库存成本价, 商品表.库存金额, 商品表.ID FROM 商品表 CROSS JOIN 结算付款定义表 WHERE (商品表.beactive = 1)";
            }
            else
            {
                sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 结算付款定义表.将付款数量 AS 下发数量, 商品表.库存数量, 商品表.库存成本价, 商品表.库存金额, 商品表.ID FROM 商品表 INNER JOIN 商品分类表 ON 商品表.分类编号 = 商品分类表.ID CROSS JOIN 结算付款定义表 WHERE (商品表.beactive = 1) AND (商品分类表.库房ID = " + intKFID.ToString() + ")";
            }

            
            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");

            dataGridView2.DataSource = dSet.Tables["商品表"];

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
                MessageBox.Show("输入下发库房", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sqlConn.Open();

            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 库存表.库存数量, 库存表.库存金额,  商品表.ID FROM 库存表 INNER JOIN  商品表 ON 库存表.商品ID = 商品表.ID WHERE (库存表.库房ID = " + intKFID.ToString() + ") AND (库存表.BeActive = 1)";
            if (dSet.Tables.Contains("库存表")) dSet.Tables.Remove("库存表");
            sqlDA.Fill(dSet, "库存表");

            dataGridView1.DataSource = dSet.Tables["库存表"];
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
                MessageBox.Show("输入下发库房", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataGridView2.SelectedRows.Count < 1)
            {
                MessageBox.Show("选择下发商品和数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //单据明细
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

                    //总库存
                    dTemp1 = Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[4].Value.ToString()) + Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[3].Value.ToString());
                    dTemp2 = Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[6].Value.ToString()) + Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[3].Value.ToString()) * Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[5].Value.ToString());


                    sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dTemp1.ToString() + ", 库存金额 = " + dTemp1.ToString() + "*[库存成本价] WHERE (ID = " + dataGridView2.SelectedRows[i].Cells[7].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //更改分库存
                    fTemp = 0; fTemp1 = 0;
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额 FROM 库存表 WHERE (库房ID = " + intKFID.ToString() + ") AND (商品ID = " + dataGridView2.SelectedRows[i].Cells[7].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    //库存成本价
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

                    if(!bNo) //没有库存
                        sqlComm.CommandText = "INSERT INTO 库存表 (库房ID, 商品ID, 库存数量, 库存金额, 库存成本价, BeActive, 库存上限, 库存下限, 合理库存上限, 合理库存下限) VALUES (" + intKFID.ToString() + ", " + dataGridView2.SelectedRows[i].Cells[7].Value.ToString() + ", " + dTemp1.ToString() + ", " + dTemp1.ToString() + "*" + dataGridView2.SelectedRows[i].Cells[5].Value.ToString() + ", " + dataGridView2.SelectedRows[i].Cells[5].Value.ToString() + ", 1, 0, 0, 0, 0)";
                    else //存在库存
                        sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dTemp1.ToString() + ", 库存金额 = " + dTemp1.ToString() + "*[库存成本价] WHERE (库房ID = " + intKFID.ToString() + ") AND (商品ID = " + dataGridView2.SelectedRows[i].Cells[7].Value.ToString() + ")";
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


            MessageBox.Show("商品下放到库房完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("输入商品库房", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageBox.Show("选择删除商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //单据明细
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
                        MessageBox.Show("商品" + dataGridView1.SelectedRows[i].Cells[1].Value.ToString() + "库存不为0,无法删除");
                        continue;
                    }

                    sqlComm.CommandText = "DELETE 库存表 WHERE (库房ID = " + intKFID.ToString() + ") AND (商品ID = " + dataGridView1.SelectedRows[i].Cells[4].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    /*

                    //分库存
                    sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = 0, 库存金额 = 0, BeActive = 0 WHERE (库房ID = " + intKFID.ToString() + ") AND (商品ID = " + dataGridView1.SelectedRows[i].Cells[4].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();


                    //更改总库存
                    fTemp = 0; fTemp1 = 0;
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额 FROM 商品表 WHERE (ID = " + dataGridView1.SelectedRows[i].Cells[4].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    //库存成本价
                    while (sqldr.Read())
                    {
                        fTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        fTemp1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                    }
                    sqldr.Close();

                    dTemp1 = fTemp - Convert.ToDecimal(dataGridView1.SelectedRows[i].Cells[2].Value.ToString());
                    dTemp2 = fTemp1 - Convert.ToDecimal(dataGridView1.SelectedRows[i].Cells[3].Value.ToString());

                    sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dTemp1.ToString() + ", 库存金额 = " + dTemp2.ToString() + " WHERE (ID = " + dataGridView1.SelectedRows[i].Cells[4].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                    */

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


            MessageBox.Show("库房商品删除完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initKFView();
 
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库房商品列表;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库房商品列表;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
        }

        private void toolStripButtonSPExcel_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("销售商品初始化无法撤销，请只在初始化数据库里使用(保证商品和库存表内都无记录)，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)!=DialogResult.Yes)
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


                if (dsCSV.Tables.Contains("信息")) dSet.Tables.Remove("信息");
                oledbDataAdapter.Fill(dsCSV, "信息");
                oledbConn.Close();
                int rowCount = Convert.ToInt32(dsCSV.Tables["信息"].Rows.Count.ToString());
                int colCount = Convert.ToInt32(dsCSV.Tables["信息"].Columns.Count.ToString());

                string sFLBH, sSPBH, sSPMC, sSPGG, sZJM;
                int iSX=0,iXX=0,iHLSX=0,iHLXX=0;
                decimal dJJ=0,dPFJ=0,dKC=0,dKCJE=0,dKCCBJ=0;
                int iFLID=0,iKFID=0;
                

                for (i = 0; i < rowCount; i++)
                {
                    //分类编号
                    if (dsCSV.Tables["信息"].Rows[i][0].ToString() == "")
                        continue;

                    sqlComm.CommandText = "SELECT ID, 库房ID FROM 商品分类表 WHERE (分类编号 = N'"+dsCSV.Tables["信息"].Rows[i][0].ToString()+"') AND (BeActive = 1)";
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

                    sSPBH = dsCSV.Tables["信息"].Rows[i][1].ToString().Trim();
                    sSPMC = dsCSV.Tables["信息"].Rows[i][2].ToString().Trim();
                    sSPGG = dsCSV.Tables["信息"].Rows[i][3].ToString().Trim();
                    sZJM = dsCSV.Tables["信息"].Rows[i][4].ToString().Trim();


                    if (sSPBH.Trim() == "" || sSPMC.Trim() == "")
                        continue;

                    if (dsCSV.Tables["信息"].Rows[i][5].ToString().Trim() == "")
                        iSX = 0;
                    else
                        iSX = int.Parse(dsCSV.Tables["信息"].Rows[i][5].ToString().Trim());

                    if (dsCSV.Tables["信息"].Rows[i][6].ToString().Trim() == "")
                        iXX = 0;
                    else
                        iXX = int.Parse(dsCSV.Tables["信息"].Rows[i][6].ToString().Trim());

                    if (dsCSV.Tables["信息"].Rows[i][7].ToString().Trim() == "")
                        iHLSX = 0;
                    else
                        iHLSX = int.Parse(dsCSV.Tables["信息"].Rows[i][7].ToString().Trim());

                    if (dsCSV.Tables["信息"].Rows[i][8].ToString().Trim() == "")
                        iHLXX = 0;
                    else
                        iHLXX = int.Parse(dsCSV.Tables["信息"].Rows[i][7].ToString().Trim());

                    if (dsCSV.Tables["信息"].Rows[i][9].ToString().Trim() == "")
                        dJJ = 0;
                    else
                        dJJ = decimal.Parse(dsCSV.Tables["信息"].Rows[i][9].ToString().Trim());

                    if (dsCSV.Tables["信息"].Rows[i][10].ToString().Trim() == "")
                        dPFJ = 0;
                    else
                        dPFJ = decimal.Parse(dsCSV.Tables["信息"].Rows[i][10].ToString().Trim());

                    if (dsCSV.Tables["信息"].Rows[i][11].ToString().Trim() == "")
                        dKC = 0;
                    else
                        dKC = decimal.Parse(dsCSV.Tables["信息"].Rows[i][11].ToString().Trim());

                    if (dsCSV.Tables["信息"].Rows[i][12].ToString().Trim() == "")
                        dKCCBJ = 0;
                    else
                        dKCCBJ = decimal.Parse(dsCSV.Tables["信息"].Rows[i][12].ToString().Trim());

                    dKCJE=dKC*dKCCBJ;

                    sqlComm.CommandText = "INSERT INTO 商品表 (商品编号, 商品名称, 助记码, 最小计量单位, 进价, 含税进价, 批发价, 含税批发价, 库存数量, 库存成本价, 库存金额, 库存件数, 最高进价, 最低进价, 最终进价, 结转数量, 结转件数, 结转金额, 结转单价, 登录日期, 库存上限, 库存下限, 合理库存上限, 合理库存下限, 组装商品, beactive, 应付金额, 已付金额, 应收金额, 已收金额, 分类编号, 商品规格) VALUES (N'" + sSPBH + "', N'" + sSPMC + "', N'" + sZJM + "', null, " + dJJ.ToString() + ", " + dJJ.ToString() + ", " + dPFJ.ToString() + ", " + dPFJ.ToString() + ", " + dKC.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE.ToString() + ", 0, 0, 0, 0, 0, 0, 0, 0, '" + strDateSYS + "', " + iSX.ToString() + ", " + iXX.ToString() + ", " + iHLSX.ToString() + ", " + iHLXX.ToString() + ", 0, 1, 0, 0, 0, 0, " + iFLID.ToString() + ", N'" + sSPGG + "')";

                    sqlComm.ExecuteNonQuery();

                    //取得号 
                    sqlComm.CommandText = "SELECT @@IDENTITY";
                    sqldr = sqlComm.ExecuteReader();
                    sqldr.Read();
                    sID = sqldr.GetValue(0).ToString();
                    sqldr.Close();


                    //下发
                    sqlComm.CommandText = "INSERT INTO 库存表 (库房ID, 商品ID, 库存数量, 库存金额, 库存成本价, BeActive, 库存上限, 库存下限, 合理库存上限, 合理库存下限) VALUES (" + iKFID.ToString() + ", " + sID + ", " + dKC.ToString() + ", " + dKCJE.ToString() + ", " + dKCCBJ.ToString() + ", 1, " + iSX.ToString() + ", " + iXX.ToString() + ", " + iHLSX.ToString() + ", " + iHLXX.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                sqlta.Commit();
                MessageBox.Show("数据导入完毕，请检查数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        }

        private void toolStripButtonTMDR_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("初始化商品条码导入，请确定数据库内没有单据信息,是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
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

                if (dsCSV.Tables.Contains("条码信息")) dSet.Tables.Remove("条码信息");
                oledbDataAdapter.Fill(dsCSV, "条码信息");
                oledbConn.Close();
                int rowCount = Convert.ToInt32(dsCSV.Tables["条码信息"].Rows.Count.ToString());
                int colCount = Convert.ToInt32(dsCSV.Tables["条码信息"].Columns.Count.ToString());

                int iSPID = 0, iKFID = 0;


                for (i = 0; i < rowCount; i++)
                {
                    if (dsCSV.Tables["条码信息"].Rows[i][0].ToString().Trim() == "")
                        continue;

                    //分类编号
                    if (dsCSV.Tables["条码信息"].Rows[i][1].ToString().Trim() == "")
                        continue;

                    sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品编号, 商品分类表.库房ID FROM 商品表 INNER JOIN 商品分类表 ON 商品表.分类编号 = 商品分类表.ID WHERE (商品表.beactive = 1) AND (商品分类表.BeActive = 1) AND (商品表.商品编号 = N'" + dsCSV.Tables["条码信息"].Rows[i][1].ToString() + "')";
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



                    sqlComm.CommandText = "INSERT INTO 商品条码表 (条码, 商品ID, 库房ID, 单据编号, 摘要, 日期, 出入库标记, 操作员ID, 单据明细ID) VALUES (N'" + dsCSV.Tables["条码信息"].Rows[i][0].ToString().Trim() + "', " + iSPID.ToString() + ", " + iKFID.ToString() + ", N'000000000', N'库房初始条码导入', CONVERT(DATETIME, '" + strDateSYS + " 00:00:00', 102), 0, "+intUserID.ToString()+", 0)";

                    sqlComm.ExecuteNonQuery();

                }

                sqlta.Commit();
                MessageBox.Show("数据导入完毕，请检查数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        }
    }
}