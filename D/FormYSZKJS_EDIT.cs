using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormYSZKJS_EDIT : Form
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

        private int iSupplyCompany = 0;
        private int intHTH = 0;
        private decimal dDJSUM = 0;
        private bool isSaved = false;
        private int iYWY = 0;
        private int iBM = 0;

        private ClassGetInformation cGetInformation;


        public FormYSZKJS_EDIT()
        {
            InitializeComponent();
        }

        private void FormYSZKJS_EDIT_Load(object sender, EventArgs e)
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

            this.Text += ":单据冲红";

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 结算收款汇总表.单据编号, 结算收款汇总表.日期, 职员表.职员姓名,[职员表_1].职员姓名, 单位表.单位编号, 单位表.单位名称, 结算收款汇总表.发票号, 结算收款汇总表.开票日期, 结算收款汇总表.备注, 结算收款汇总表.实计金额, 结算收款汇总表.税号, 结算收款汇总表.单位ID,结算收款汇总表.业务员ID,结算收款汇总表.部门ID FROM 结算收款汇总表 INNER JOIN 职员表 ON 结算收款汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 结算收款汇总表.业务员ID = [职员表_1].ID INNER JOIN 单位表 ON 结算收款汇总表.单位ID = 单位表.ID WHERE (结算收款汇总表.ID =  " + intDJID.ToString() + ") AND (结算收款汇总表.BeActive<>0)";
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

                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelCZY.Text = sqldr.GetValue(2).ToString();
                comboBoxYWY.Text = sqldr.GetValue(3).ToString();
                textBoxDWBH.Text = sqldr.GetValue(4).ToString();
                textBoxDWMC.Text = sqldr.GetValue(5).ToString();
                textBoxFPH.Text = sqldr.GetValue(6).ToString();
                textBoxSH.Text = sqldr.GetValue(10).ToString();

                textBoxBZ.Text = sqldr.GetValue(8).ToString();
                iSupplyCompany = Convert.ToInt32(sqldr.GetValue(11).ToString());
                iYWY = Convert.ToInt32(sqldr.GetValue(12).ToString());
            }

            sqldr.Close();
            if (iBM != 0)
            {
                sqlComm.CommandText = "SELECT 部门名称 FROM 部门表 WHERE (ID = " + iBM.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                    comboBoxBM.Text = sqldr.GetValue(0).ToString();
                    break;
                }
                sqldr.Close();
            }

            sqlComm.CommandText = "SELECT 结算收款明细表.ID, 账簿表.账簿编号, 账簿表.账簿名称, 结算收款明细表.摘要,结算收款明细表.冲应付款, 账簿表.扣率, 结算收款明细表.付款金额, 结算收款明细表.支票号, 结算收款明细表.备注, 账簿表.账簿ID, 结算收款定义表.勾兑标记, 结算收款定义表.勾兑纪录, 结算收款明细表.单据ID FROM 账簿表 INNER JOIN 结算收款明细表 ON 账簿表.ID = 结算收款明细表.账簿ID CROSS JOIN 结算收款定义表 WHERE (结算收款明细表.单据ID = " + intDJID.ToString() + ")";

            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据表"];

            sqlConn.Close();


            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[9].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;
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
            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;


            countAmount();

        }
        //return true 正确  false 错误
        private bool countAmount()
        {
            decimal fSum = 0, fSum1 = 0;
            decimal fTemp, fTemp1;
            decimal fCount = 0, fCSum = 0;

            //this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            bool bCheck = true;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                //库房ID
                if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "0")
                {
                    bCheck = false;
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "请输入账簿编号";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "请输入账簿助记码";
                    continue;
                }


                //冲应收款
                if (dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[4].Value = 0;

                fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);


                //扣率
                if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[5].Value = 100;
                }


                //实计金额
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                dataGridViewDJMX.Rows[i].Cells[6].Value = fTemp * fTemp1 / 100;


                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);

                fCount += 1;

            }
            //this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelJEHJ.Text = fSum.ToString();
            labelSJJE.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;


        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("应收账款结算单已经冲红,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int i,j,k;
            string sTemp = "";
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;
            string strCommID, strKFID;

            string sBMID = "NULL";
            if (iBM != 0)
                sBMID = iBM.ToString();

            System.Data.SqlClient.SqlTransaction sqlta;

            string strDateSYS;
            cGetInformation.getSystemDateTime();
            strDateSYS = cGetInformation.strSYSDATATIME;
            
            sqlConn.Open();
            //得到上次结转时间
            string sSCJZSJ = "";
            sqlComm.CommandText = "SELECT 结算时间,ID FROM 结转汇总表 ORDER BY 结算时间 DESC";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
            }
            sqldr.Close();

            if (sSCJZSJ == "") //没有结算
            {
                sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                sqldr.Close();
            }

            //得到制单日期
            string strDate1 = "";
            sqlComm.CommandText = "SELECT 日期 from 结算收款汇总表 WHERE (ID = " + intDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                strDate1 = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
            }
            sqldr.Close();

            if (DateTime.Parse(strDate1) <= DateTime.Parse(sSCJZSJ)) //有转结记录
            {
                if (MessageBox.Show("制单后已有转结记录：" + sSCJZSJ + "，是否强行冲红？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                {
                    sqlConn.Close();
                    return;
                }
            }
            
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //表单汇总
                sqlComm.CommandText = "UPDATE 结算收款汇总表 SET BeActive = 0 WHERE (ID = " + intDJID.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 结算收款汇总表 SET 冲红时间 = '" + strDateSYS + "' WHERE (ID = " + intDJID.ToString() + ")";
                sqlComm.ExecuteNonQuery();



                //单位应付账
                sqlComm.CommandText = "SELECT 应收账款 FROM 单位表 WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();

                dKCJE = 0;
                while (sqldr.Read())
                {
                    dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                }
                sqldr.Close();
                dKCJE1 = dKCJE + Convert.ToDecimal(labelSJJE.Text);
                sqlComm.CommandText = "UPDATE 单位表 SET 应收账款 = " + dKCJE1.ToString() + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //单位历史纪录
                sqlComm.CommandText = "INSERT INTO 单位历史账表 (单位ID, 日期, 单据编号, 摘要, 应收金额, 业务员ID, 冲抵单号, BeActive, 部门ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + labelDJBH.Text + "冲', N'应收账款结算单冲红', " + dKCJE1.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "', 1,"+sBMID+")";
                sqlComm.ExecuteNonQuery();

                //明细更新
                for (k = 0; k < dataGridViewDJMX.RowCount; k++)
                {
                    sqlComm.CommandText = "SELECT ID, 勾兑方式, 勾兑ID, 单据编号, 已付款, BeActive FROM 结算收款勾兑表 WHERE (勾兑方式 = 1) AND (BeActive = 1) AND (付款ID = " + dataGridViewDJMX.Rows[k].Cells[0].Value.ToString() + ")";

                    if (dSet.Tables.Contains("勾兑表")) dSet.Tables.Remove("勾兑表");
                    sqlDA.Fill(dSet, "勾兑表");

                    for (j = 0; j < dSet.Tables["勾兑表"].Rows.Count; j++)
                    {
                        //回退单据
                        sTemp = dSet.Tables["勾兑表"].Rows[j][3].ToString().Substring(0, 3);
                        strCommID = "0";
                        strKFID = "0";
                        switch (sTemp)
                        {
                            case "BKP":
                                sqlComm.CommandText = "UPDATE 销售商品制单明细表 SET 未付款金额 = 未付款金额 + " + dSet.Tables["勾兑表"].Rows[j][4].ToString() + ", 已付款金额 = 已付款金额 - " + dSet.Tables["勾兑表"].Rows[j][4].ToString() + " WHERE (ID = " + dSet.Tables["勾兑表"].Rows[j][2].ToString() + ")";
                                sqlComm.ExecuteNonQuery();

                                sqlComm.CommandText = "UPDATE 销售商品制单表 SET 未付款金额 = 未付款金额 + " + dSet.Tables["勾兑表"].Rows[j][4].ToString() + ", 已付款金额 =  已付款金额 - " + dSet.Tables["勾兑表"].Rows[j][4].ToString() + " WHERE (单据编号 = N'" + dSet.Tables["勾兑表"].Rows[j][3].ToString() + "')";
                                sqlComm.ExecuteNonQuery();

                                sqlComm.CommandText = "SELECT 商品ID, 库房ID FROM 销售商品制单明细表 WHERE (ID = " + dSet.Tables["勾兑表"].Rows[j][2].ToString() + ")";
                                sqldr = sqlComm.ExecuteReader();
                                while (sqldr.Read())
                                {
                                    strCommID = sqldr.GetValue(0).ToString();
                                    strKFID = sqldr.GetValue(1).ToString();
                                    break;
                                }
                                sqldr.Close();

                                break;

                            case "BTH":
                                sqlComm.CommandText = "UPDATE 销售退出明细表 SET 未付款金额 = 未付款金额 - (" + dSet.Tables["勾兑表"].Rows[j][4].ToString() + "), 已付款金额 = 已付款金额 - (-1*" + dSet.Tables["勾兑表"].Rows[j][4].ToString() + ") WHERE (ID = " + dSet.Tables["勾兑表"].Rows[j][2].ToString() + ")";
                                sqlComm.ExecuteNonQuery();

                                sqlComm.CommandText = "UPDATE 销售退出汇总表 SET 未付款金额 = 未付款金额 - (" + dSet.Tables["勾兑表"].Rows[j][4].ToString() + "), 已付款金额 =  已付款金额 - (-1*" + dSet.Tables["勾兑表"].Rows[j][4].ToString() + ") WHERE (单据编号 = N'" + dSet.Tables["勾兑表"].Rows[j][3].ToString() + "')";
                                sqlComm.ExecuteNonQuery();

                                sqlComm.CommandText = "SELECT 商品ID, 库房ID FROM 销售退出明细表 WHERE (ID = " + dSet.Tables["勾兑表"].Rows[j][2].ToString() + ")";
                                sqldr = sqlComm.ExecuteReader();
                                while (sqldr.Read())
                                {
                                    strCommID = sqldr.GetValue(0).ToString();
                                    strKFID = sqldr.GetValue(1).ToString();
                                    break;
                                }
                                sqldr.Close();

                               break;

                            case "BTB":
                               sqlComm.CommandText = "UPDATE 销售退补差价明细表 SET 未付款金额 = 未付款金额 + " + dSet.Tables["勾兑表"].Rows[j][4].ToString() + ", 已付款金额 = 已付款金额 - " + dSet.Tables["勾兑表"].Rows[j][4].ToString() + " WHERE (ID = " + dSet.Tables["勾兑表"].Rows[j][2].ToString() + ")";
                               sqlComm.ExecuteNonQuery();

                               sqlComm.CommandText = "UPDATE 销售退补差价汇总表 SET 未付款金额 = 未付款金额 + " + dSet.Tables["勾兑表"].Rows[j][4].ToString() + ", 已付款金额 =  已付款金额 - " + dSet.Tables["勾兑表"].Rows[j][4].ToString() + " WHERE (单据编号 = N'" + dSet.Tables["勾兑表"].Rows[j][3].ToString() + "')";
                                sqlComm.ExecuteNonQuery();


                               sqlComm.CommandText = "SELECT 商品ID, 库房ID FROM 销售退补差价明细表 WHERE (ID = " + dSet.Tables["勾兑表"].Rows[j][2].ToString() + ")";
                               sqldr = sqlComm.ExecuteReader();
                               while (sqldr.Read())
                               {
                                   strCommID = sqldr.GetValue(0).ToString();
                                   strKFID = sqldr.GetValue(1).ToString();
                                   break;
                               }
                               sqldr.Close();

                                break;

                        }
                        //
                        sqlComm.CommandText = "UPDATE 结算收款勾兑表 SET BeActive = 0 WHERE (ID = " + dSet.Tables["勾兑表"].Rows[j][0].ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //总库存
                        sqlComm.CommandText = "UPDATE 商品表 SET 应收金额 = 应收金额 +" + dSet.Tables["勾兑表"].Rows[j][4].ToString() + ", 已收金额 = 已收金额 -" + dSet.Tables["勾兑表"].Rows[j][4].ToString() + " WHERE (ID = " + strCommID + ")";
                        sqlComm.ExecuteNonQuery();

                        //分库存
                        sqlComm.CommandText = "UPDATE 库存表 SET  应收金额 = 应收金额 +" + dSet.Tables["勾兑表"].Rows[j][4].ToString() + ", 已收金额 = 已收金额 - " + dSet.Tables["勾兑表"].Rows[j][4].ToString() + " WHERE (库房ID = " + strKFID + ") AND (商品ID = " + strCommID + ") AND (BeActive = 1)";
                        sqlComm.ExecuteNonQuery();


                      //
                    }
                    

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

            //MessageBox.Show("应收账款结算单冲红成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isSaved = true;
            if (MessageBox.Show("应收账款结算单冲红成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }

        }

        private void FormYSZKJS_EDIT_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
            {
                return;
            }

            DialogResult dr = MessageBox.Show(this, "单据尚未保存，确定要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            string strT = "应收账款结算单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelJEHJ.Text + "(大写:" + labelDX.Text + ");发　票号：" + textBoxFPH.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "应收账款结算单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelJEHJ.Text + "(大写:" + labelDX.Text + ");发　票号：" + textBoxFPH.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private string getCompanyPay(int icompanyID)
        {
            string strPay = "0.00";

            sqlConn.Open();
            sqlComm.CommandText = "SELECT SUM(未付款金额) FROM 收款明细视图 WHERE (单位ID = " + icompanyID.ToString() + ")";

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