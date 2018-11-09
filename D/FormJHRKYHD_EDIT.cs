using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJHRKYHD_EDIT : Form
    {

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";
        public int intDJID = 0;

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private int iSupplyCompany = 0;
        private int intHTH = 0;

        private bool isSaved = false;
        private int iYWY = 0;

        private ClassGetInformation cGetInformation;
        private bool bCheck = true;
        private int iBM = 0;


        public FormJHRKYHD_EDIT()
        {
            InitializeComponent();
        }

        private void FormJHRKYHD_EDIT_Load(object sender, EventArgs e)
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
            sqlComm.CommandText = "SELECT 进货入库汇总表.单据编号, 进货入库汇总表.日期, [职员表_1].职员姓名 AS 操作员, 职员表.职员姓名 AS 业务员, 单位表.单位编号, 单位表.单位名称, 进货入库汇总表.发票号, 进货入库汇总表.支票号, 进货入库汇总表.合同号, 进货入库汇总表.价税合计, 进货入库汇总表.备注, 单位表.ID,进货入库汇总表.业务员ID, 进货入库汇总表.部门ID  FROM 进货入库汇总表 INNER JOIN 职员表 ON 进货入库汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 进货入库汇总表.操作员ID = [职员表_1].ID INNER JOIN 单位表 ON 进货入库汇总表.单位ID = 单位表.ID WHERE (进货入库汇总表.ID = " + intDJID.ToString() + ") AND (进货入库汇总表.BeActive<>0)";
            sqldr=sqlComm.ExecuteReader();

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
                textBoxZPH.Text = sqldr.GetValue(7).ToString();
                textBoxHTH.Text = sqldr.GetValue(8).ToString();
                textBoxBZ.Text = sqldr.GetValue(10).ToString();
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


            sqlComm.CommandText = "SELECT 购进商品制单明细定义表.到货, 进货入库汇总表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 进货入库明细表.数量, 进货入库明细表.单价, 进货入库明细表.金额, 进货入库明细表.扣率, 进货入库明细表.实计金额, 商品表.库存数量, 进货入库明细表.商品ID, 进货入库明细表.库房ID, 进货入库明细表.ID, 进货入库明细表.赠品, 进货入库汇总表.ID AS Expr1, 进货入库明细表.原单据明细ID, 进货入库明细表.原单据ID FROM 进货入库明细表 INNER JOIN  商品表 ON 进货入库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 进货入库明细表.库房ID = 库房表.ID INNER JOIN 进货入库汇总表 ON 进货入库明细表.单据ID = 进货入库汇总表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (进货入库明细表.单据ID = " + intDJID.ToString() + ")";

            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据表"];

            sqlConn.Close();

            
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[8].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[14].Visible = false;
            dataGridViewDJMX.Columns[15].Visible = false;
            dataGridViewDJMX.Columns[16].Visible = false;
            dataGridViewDJMX.Columns[18].Visible = false;
            dataGridViewDJMX.Columns[19].Visible = false;
            dataGridViewDJMX.Columns[20].Visible = false;
            
            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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
            dataGridViewDJMX.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


            countAmount();

        }

        private void countAmount()
        {
            decimal fSum = 0, fSum1 = 0;
            decimal fTemp, fTemp1;
            decimal fCount = 0, fCSum = 0;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;


                //数量

                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                    fTemp = 0;
                else
                    fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                fCSum += fTemp;

                //单价
                if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                //金额
                dataGridViewDJMX.Rows[i].Cells[10].Value = Math.Round(fTemp * fTemp1, 2);

                //扣率
                if (dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[11].Value = 100;
                }

                //实计金额
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                dataGridViewDJMX.Rows[i].Cells[12].Value = fTemp1 * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value.ToString()) / 100;

                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[12].Value);

                fCount += 1;

            }
            labelSLHJ.Text = fCSum.ToString("f0");
            labelJEHJ.Text = fSum.ToString();
            labelSJJE.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();


        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i,j;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE=0,dKCJE1=0,dYSYE=0,dYSYE1=0;

            
            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("进货入库验货单已经冲红,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string strDateSYS;
            cGetInformation.getSystemDateTime();
            strDateSYS = cGetInformation.strSYSDATATIME;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            //查财务
            sqlComm.CommandText = "SELECT 结算付款汇总表.单据编号 FROM 结算付款勾兑表 INNER JOIN 结算付款汇总表 ON 结算付款勾兑表.付款ID = 结算付款汇总表.ID WHERE (结算付款勾兑表.单据编号 = N'" + labelDJBH.Text + "') AND (结算付款勾兑表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                while (sqldr.Read())
                {
                    MessageBox.Show("已有财务勾兑记录,单据号为：" + sqldr.GetValue(0).ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                }
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            sqldr.Close();

            //发票记录
            sqlComm.CommandText = "SELECT 发票号, ID FROM 进货入库汇总表 WHERE (发票号 IS NOT NULL) AND (发票号 NOT LIKE N'不开票%') AND (ID = " + intDJID.ToString() + ") AND (发票号 NOT LIKE N'现金不开票%')";
            sqldr = sqlComm.ExecuteReader();
            bool b=false;
            if (sqldr.HasRows)
            {
                while (sqldr.Read())
                {
                    if (sqldr.GetValue(0).ToString().Trim() != "")
                    {
                        MessageBox.Show("已有发票记录,发票号为：" + sqldr.GetValue(0).ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
           sqlComm.CommandText = "SELECT 日期 from 进货入库汇总表 WHERE (ID = " + intDJID.ToString() + ")";
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
                string sBMID = "NULL";
                if (iBM != 0)
                    sBMID = iBM.ToString();

                sqlComm.CommandText = "UPDATE 进货入库汇总表 SET BeActive = 0 WHERE (ID = "+intDJID.ToString()+")";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 进货入库汇总表 SET 冲红时间 = '" + strDateSYS + "' WHERE (ID = " + intDJID.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //单位应付账
                sqlComm.CommandText = "SELECT 应付账款 FROM 单位表 WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqldr=sqlComm.ExecuteReader();

                dKCJE = 0;
                while (sqldr.Read())
                {
                    dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                }
                sqldr.Close();
                dKCJE1 = dKCJE - Convert.ToDecimal(labelSJJE.Text);
                sqlComm.CommandText = "UPDATE 单位表 SET 应付账款 = " + dKCJE1.ToString() + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //单位历史纪录
                sqlComm.CommandText = "INSERT INTO 单位历史账表 (单位ID, 日期, 单据编号, 摘要, 购进金额, 应付余额, 购进标记, 业务员ID, 冲抵单号, BeActive, 部门ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + labelDJBH.Text + "冲', N'进货入库验货单冲红', -" + labelSJJE.Text.ToString() + ", " + dKCJE1.ToString() + ", 1, " + iYWY.ToString() + ", N'" + labelDJBH.Text + "', 1,"+sBMID+")";
                sqlComm.ExecuteNonQuery();

                //未到货恢复
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    sqlComm.CommandText = "UPDATE 购进商品制单明细表 SET 未到货数量 =未到货数量+" + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[19].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }



                //库存
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    //总库存变更
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[12].Value);
                    dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                    dYSYE1 = -dKCJE1;

                    //总库存变更
                    sqlComm.CommandText = "SELECT 库存数量, 库存成本价,库存金额, 应付金额 FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                    }
                    sqldr.Close();

                    //余额
                    dYSYE += dYSYE1;

                    dKUL -= dKUL1;
                    //dKCJE -= dKCJE1;
                    dKCJE = dKUL * dKCCBJ;

                    sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存成本价 = " + dKCCBJ.ToString() + ", 库存金额=" + dKCJE.ToString() + ", 应付金额=" + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //总账历史纪录
                    sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 入库数量, 入库单价, 入库金额, 总结存数量, 总结存金额, 应付金额, BeActive, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "冲', N'进货入库验货单冲红', -" + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", -" + dKCJE1 + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,"+sBMID+")";
                    sqlComm.ExecuteNonQuery();



                    //分库存更新
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价, 库存金额 ,应付金额 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ") AND (BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                    }
                    sqldr.Close();
                    dKUL -= dKUL1;
                    //dKCJE -= dKCJE1;
                    dKCJE = dKUL * dKCCBJ;

                    //余额
                    dYSYE += dYSYE1;

                    sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存成本价 = " + dKCCBJ.ToString() + ",库存金额=" + dKCJE.ToString() + ", 应付金额=" + dYSYE.ToString() + " WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ") AND (BeActive = 1)";
                    sqlComm.ExecuteNonQuery();

                    //库房账历史纪录
                    sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 入库数量, 入库单价, 入库金额, 库房结存数量, 库房结存金额, 应付金额, BeActive, 部门ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ",'" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "冲', N'进货入库验货单冲红', -" + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", -" + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,"+sBMID+")";
                    sqlComm.ExecuteNonQuery();

                }


                //条码
                sqlComm.CommandText = "DELETE FROM 商品条码表 WHERE (单据编号 = N'" + labelDJBH.Text + "')";
                sqlComm.ExecuteNonQuery();


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

            checkRKView();

            //MessageBox.Show("进货入库验货单冲红成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isSaved = true;

            if (MessageBox.Show("进货入库验货单冲红成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }

        }

        //更新入库标记
        private void checkRKView()
        {
            int i;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {

                    //进货标志
                    if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                    }

                    sqlComm.CommandText = "SELECT 购进商品制单明细表.ID FROM 购进商品制单明细表 INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID WHERE (购进商品制单明细表.未到货数量 <> 0) AND (购进商品制单明细表.表单ID = " + dataGridViewDJMX.Rows[i].Cells[20].Value.ToString() + ") AND (购进商品制单表.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //存在未到货明细
                    {
                        sqldr.Close();
                        sqlComm.CommandText = "UPDATE 购进商品制单表 SET 入库标记 = 0 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[20].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();
                    }
                    else
                    {
                        sqldr.Close();
                        sqlComm.CommandText = "UPDATE 购进商品制单表 SET 入库标记 = 1 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[20].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();
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
        }

        private void FormJHRKYHD_EDIT_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
                return;

            DialogResult dr = MessageBox.Show(this, "单据修改尚未保存，确定要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                    
            string strT = "进货入库验货单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {

            string strT = "进货入库验货单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

    }
}