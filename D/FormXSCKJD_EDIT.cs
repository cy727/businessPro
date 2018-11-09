using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXSCKJD_EDIT : Form
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

        public FormXSCKJD_EDIT()
        {
            InitializeComponent();
        }

        private void FormXSCKJD_EDIT_Load(object sender, EventArgs e)
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
            sqlComm.CommandText = "SELECT 销售出库汇总表.单据编号, 销售出库汇总表.日期, [职员表_1].职员姓名 AS 操作员, 职员表.职员姓名 AS 业务员, 单位表.单位编号, 单位表.单位名称, 销售出库汇总表.发票号, 销售出库汇总表.支票号, 销售出库汇总表.合同号, 销售出库汇总表.价税合计, 销售出库汇总表.备注, 单位表.ID,销售出库汇总表.业务员ID, 销售出库汇总表.部门ID  FROM 销售出库汇总表 INNER JOIN 职员表 ON 销售出库汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 销售出库汇总表.操作员ID = [职员表_1].ID INNER JOIN 单位表 ON 销售出库汇总表.单位ID = 单位表.ID WHERE (销售出库汇总表.ID = " + intDJID.ToString() + ") AND (销售出库汇总表.BeActive<>0)";
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


            sqlComm.CommandText = "SELECT 购进商品制单明细定义表.到货, 销售出库汇总表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 销售出库明细表.数量, 销售出库明细表.单价, 销售出库明细表.金额, 销售出库明细表.扣率, 销售出库明细表.实计金额, 商品表.库存数量, 销售出库明细表.商品ID, 销售出库明细表.库房ID, 销售出库明细表.ID, 销售出库明细表.赠品, 销售出库汇总表.ID AS Expr1, 销售出库明细表.原单据明细ID, 销售出库明细表.原单据ID FROM 销售出库明细表 INNER JOIN  商品表 ON 销售出库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 销售出库明细表.库房ID = 库房表.ID INNER JOIN 销售出库汇总表 ON 销售出库明细表.单据ID = 销售出库汇总表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (销售出库明细表.单据ID = " + intDJID.ToString() + ")";

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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dKUL = 0;
            decimal dKUL1 = 0;

            textBoxHTH.Focus();
            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("销售出库校对单已经保存,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("没有销售出库校对单商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //if (MessageBox.Show("请检查销售销售出库校对单内容,是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            //    return;

            saveToolStripButton.Enabled = false;

            string strCount = "", strDateSYS = "", strKey = "BCK";
            System.Data.SqlClient.SqlTransaction sqlta;
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
            sqlComm.CommandText = "SELECT 日期 from 销售出库汇总表 WHERE (ID = " + intDJID.ToString() + ")";
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

                sqlComm.CommandText = "UPDATE 销售出库汇总表 SET BeActive = 0 WHERE (ID = " + intDJID.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 销售出库汇总表 SET 冲红时间 = '" + strDateSYS + "' WHERE (ID = " + intDJID.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //未到货恢复
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    sqlComm.CommandText = "UPDATE  销售商品制单明细表 SET 未出库数量 =未出库数量+" + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[19].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();

                saveToolStripButton.Enabled = true;
                return;
            }
            finally
            {
                sqlConn.Close();
            }
            checkRKView();

            isSaved = true;

            if (MessageBox.Show("销售出库校对单冲红成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
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
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    //进货标志

                    sqlComm.CommandText = "SELECT 销售商品制单明细表.ID FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID WHERE (销售商品制单明细表.未出库数量 <> 0) AND (销售商品制单明细表.表单ID = " + dataGridViewDJMX.Rows[i].Cells[20].Value.ToString() + ") AND (销售商品制单表.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //存在未出货明细
                    {
                        sqldr.Close();
                        sqlComm.CommandText = "UPDATE 销售商品制单表 SET 出库标记 = 0 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[20].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();
                    }
                    else
                    {
                        sqldr.Close();
                        sqlComm.CommandText = "UPDATE 销售商品制单表 SET 出库标记 = 1 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[20].Value.ToString() + ")";
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

        private void printToolStripButton_Click(object sender, EventArgs e)
        {

        }
    }
}
