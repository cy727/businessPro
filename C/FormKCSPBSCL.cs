using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKCSPBSCL : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private int intKFID = 0;
        private int intCommID = 0;
        private int intClassID = 0;

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;

        public FormKCSPBSCL()
        {
            InitializeComponent();
        }

        private void FormFormKCSPBSCL_Load(object sender, EventArgs e)
        {
            int i;

            this.Left = 1;
            this.Top = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            if (isSaved)
            {
                //dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                initDJ();
                return;
            }

            sqlConn.Open();

            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.Text = strUserName;

            //初始化部门列表
            comboBoxBM.SelectedIndexChanged -= comboBoxBM_SelectedIndexChanged;
            sqlComm.CommandText = "SELECT 部门名称 FROM 部门表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("部门表")) dSet.Tables.Remove("部门表");
            sqlDA.Fill(dSet, "部门表");
            //comboBoxBM.DataSource = dSet.Tables["部门表"];

            comboBoxBM.Items.Add("全部");
            for (i = 0; i < dSet.Tables["部门表"].Rows.Count; i++)
            {
                comboBoxBM.Items.Add(dSet.Tables["部门表"].Rows[i][0].ToString().Trim());
            }
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;


            sqlConn.Close();

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }
        private void initDJ()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 库存报损汇总表.单据编号, 库存报损汇总表.日期, 职员表.职员姓名 AS 业务员, 操作员.职员姓名, 库存报损汇总表.备注, 库房表.库房编号, 库房表.库房名称 FROM 库存报损汇总表 INNER JOIN 职员表 ON 库存报损汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 库存报损汇总表.操作员ID = 操作员.ID INNER JOIN 库房表 ON 库存报损汇总表.库房ID = 库房表.ID WHERE (库存报损汇总表.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                textBoxPDKF.Text = sqldr.GetValue(5).ToString();
                textBoxKFMC.Text = sqldr.GetValue(6).ToString();

                this.Text = "库存盘点表：" + labelDJBH.Text;
            }
            sqldr.Close();

            comboBoxBM.SelectedIndexChanged -= comboBoxBM_SelectedIndexChanged;
            sqlComm.CommandText = "SELECT 部门表.部门名称 FROM 部门表 INNER JOIN 职员表 ON 部门表.ID = 职员表.岗位ID WHERE (职员表.职员姓名 = N'" + comboBoxYWY.Text + "')";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                comboBoxBM.Text = sqldr.GetValue(0).ToString();
                break;
            }
            sqldr.Close();
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;

            //初始化商品列表
            sqlComm.CommandText = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库存报损明细表.报损数量, 库存报损明细表.成本单价, 库存报损明细表.报损金额, 库存报损明细表.备注, 库存报损明细表.商品ID, 库存报损明细表.原库存数量 FROM 库存报损明细表 INNER JOIN 商品表 ON 库存报损明细表.商品ID = 商品表.ID INNER JOIN 库存报损汇总表 ON 库存报损明细表.单据ID = 库存报损汇总表.ID WHERE (库存报损明细表.单据ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据表"];


            dataGridViewDJMX.Columns[8].Visible = false;

            dataGridViewDJMX.Columns[5].Visible = false;
            dataGridViewDJMX.Columns[6].Visible = false;

            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridViewDJMX.ShowCellErrors = true;
            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;


            sqlConn.Close();
        }



        private void comboBoxBM_SelectedIndexChanged(object sender, EventArgs e)
        {
            sqlConn.Open();
            //初始化员工列表
            if (comboBoxBM.Text.Trim() != "全部")
                sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员姓名, 职员表.职员编号 FROM 职员表 INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (部门表.部门名称 = N'" + comboBoxBM.Text.Trim() + "') AND (职员表.beactive = 1)";
            else
                sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员姓名, 职员表.职员编号 FROM 职员表 INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (职员表.beactive = 1)";

            sqldr = sqlComm.ExecuteReader();
            if (!sqldr.HasRows)
            {
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            sqldr.Close();

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            sqlConn.Close();
        }

        private void textBoxPDKF_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intKFID = cGetInformation.iKFNumber;
                textBoxPDKF.Text = cGetInformation.strKFCode;
                textBoxKFMC.Text = cGetInformation.strKFName;

                initdataGridViewDJMX();

            }
        }

        private void textBoxPDKF_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(10, textBoxPDKF.Text.Trim()) == 0) //失败
                {
                    intKFID = 0;
                    textBoxPDKF.Text = "";
                    textBoxKFMC.Text = "";
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxPDKF.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                    initdataGridViewDJMX();
                }
            }
        }

        private void textBoxKFMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFMC.Text.Trim()) == 0) //失败
                {
                    intKFID = 0;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxPDKF.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                    initdataGridViewDJMX();
                }
            }
        }
        private void initdataGridViewDJMX()
        {
            if (intKFID == 0) //没有库房
            {
                if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
                return;
            }

            strSelect = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库存盘点定义表.盘损数量 AS 报损数量, 库存表.库存成本价 AS 成本单价, 库存盘点定义表.盘损金额 AS 报损金额, 库存盘点定义表.备注, 库存表.商品ID, 库存表.库存数量 FROM 库存表 INNER JOIN 商品表 ON 库存表.商品ID = 商品表.ID CROSS JOIN 库存盘点定义表 WHERE (库存表.库房ID = " + intKFID.ToString() + ")";

            if (intCommID != 0) //商品过滤
            {
                strSelect += " AND (库存表.商品ID = " + intCommID.ToString() + ")";
            }

            if (intClassID != 0) //商品分类过滤
            {
                strSelect += "  AND (商品表.分类编号 = " + intClassID.ToString() + ")";
            }

            sqlConn.Open();
            sqlComm.CommandText = strSelect;

            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据表"];

            sqlConn.Close();

            dataGridViewDJMX.Columns[8].Visible = false;
            dataGridViewDJMX.Columns[0].ReadOnly = true;
            dataGridViewDJMX.Columns[1].ReadOnly = true;
            dataGridViewDJMX.Columns[2].ReadOnly = true;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[5].ReadOnly = true;
            dataGridViewDJMX.Columns[6].ReadOnly = true;
            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].Visible = false;
            dataGridViewDJMX.Columns[6].Visible = false;

        }


        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private bool countAmount()
        {
            decimal fSum = 0;
            decimal fCount = 0, fCSum = 0, fCSum1 = 0;
            //this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                //报损数量标志
                if (dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[4].Value = 0;
                }
                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value)==0)
                    continue;

                //报损金额
                dataGridViewDJMX.Rows[i].Cells[6].Value = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString());


                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value.ToString());
                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString());

                fCount += 1;
            }
            //this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelSLHJ.Text = fCSum.ToString();
            labelJEHJ.Text = fSum.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelJEHJ.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return true;
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i;
            decimal fTemp, fTemp1;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;
            string sTemp="0";

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("库存商品报损表已经保存,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intKFID == 0)
            {
                MessageBox.Show("请选择商品库房", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!countAmount())
            {
                MessageBox.Show("报损明细错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("没有报损商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (MessageBox.Show("请检查库存商品报损表内容，是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;
            saveToolStripButton.Enabled = false;
            string strCount = "", strDateSYS = "", strKey = "CBS";
            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //得到表单号
                //得到服务器日期
                sqlComm.CommandText = "SELECT GETDATE() AS 日期";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                sqldr.Close();

                //得到日期
                sqlComm.CommandText = "SELECT 时间 FROM 表单计数表 WHERE (时间 = CONVERT(DATETIME, '" + strDateSYS + " 00:00:00', 102))";
                sqldr = sqlComm.ExecuteReader();

                if (sqldr.HasRows)
                    sqldr.Close();
                else //服务器时间不吻合
                {
                    sqldr.Close();
                    //修正日期及计数器
                    sqlComm.CommandText = "UPDATE 表单计数表 SET 时间 = '" + strDateSYS + "', 计数 = 1";
                    sqlComm.ExecuteNonQuery();
                }

                //得到计数器
                sqlComm.CommandText = "SELECT 计数 FROM 表单计数表 WHERE (关键词 = N'" + strKey + "')";
                sqldr = sqlComm.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    strCount = sqldr.GetValue(0).ToString();
                    sqldr.Close();

                    //增加计数器
                    sqlComm.CommandText = "UPDATE 表单计数表 SET 计数 = 计数 + 1 WHERE (关键词 = N'" + strKey + "')";
                    sqlComm.ExecuteNonQuery();
                }
                else
                    sqldr.Close();

                if (strCount != "")
                {
                    strCount = string.Format("{0:D3}", Int32.Parse(strCount));
                    strCount = strKey.ToUpper() + Convert.ToDateTime(strDateSYS).ToString("yyyyMMdd") + strCount;
                }
                else
                {
                    MessageBox.Show("数据错误", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sqlConn.Close();
                    return;
                }


                //表单汇总
                sqlComm.CommandText = "INSERT INTO 库存报损汇总表 (单据编号, 日期, 业务员ID, 操作员ID, 备注, 库房ID, 商品ID, 分类ID, 报损数量合计, 报损金额合计, BeActive) VALUES (N'"+strCount+"', '"+strDateSYS+"', "+comboBoxYWY.SelectedValue.ToString()+", "+intUserID.ToString()+", N'"+textBoxBZ.Text+"', "+intKFID.ToString()+", "+intCommID.ToString()+", "+intClassID.ToString()+", "+labelSLHJ.Text+", "+labelJEHJ.Text+", 1)";
                sqlComm.ExecuteNonQuery();


                //取得单据号 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //单据明细
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value) == 0)
                        continue;

                    if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "")
                    {
                        sTemp = "0";
                    }
                    else
                    {
                        sTemp = dataGridViewDJMX.Rows[i].Cells[9].Value.ToString();
                    }

                    sqlComm.CommandText = "INSERT INTO 库存报损明细表 (单据ID, 商品ID, 报损数量, 报损金额, 成本单价, 备注, 原库存数量) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + "',"+sTemp+")";
                    sqlComm.ExecuteNonQuery();



                    //总库存
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价 FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    //库存成本价
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value.ToString());
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString());

                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                    }
                    dKUL -= dKUL1;
                    dKCJE -= dKCJE1;
                    sqldr.Close();

                    sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //历史纪录
                    sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 业务员ID, 单据编号, 摘要, 总结存数量, 总结存金额, 报损数量, 报损单价, 报损金额, BeActive) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'库存商品报损', " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", 1)";
                    sqlComm.ExecuteNonQuery();



                    //更改分库存
                    fTemp = 0;
                    sqlComm.CommandText = "SELECT  库存数量, 库存金额, 库存成本价  FROM 库存表 WHERE (库房ID = " + intKFID.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    //库存成本价
                    dKUL = 0; dKCJE = 0; dKCCBJ = 0;
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                    }
                    sqldr.Close();

                    //库存金额
                    dKUL -= dKUL1;
                    dKCJE -= dKCJE1;

                    sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + " WHERE (库房ID = " + intKFID.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //库房历史纪录
                    sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 业务员ID, 单据编号, 摘要, 库房结存数量, 库房结存金额, 报损数量, 报损单价, 报损金额, BeActive) VALUES (" + intKFID.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'库存商品报损', " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", 1)";
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


            //MessageBox.Show("库存商品报损表保存成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = "库存商品报损表：" + labelDJBH.Text;
            isSaved = true;

            if (MessageBox.Show("库存商品报损表保存成功，是否关闭制单窗口", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }


        }

        private void FormKCSPBSCL_FormClosing(object sender, FormClosingEventArgs e)
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
            string strT = "库存盘点表(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";库房名称：" + textBoxKFMC.Text + ";报损数量合计：" + labelSLHJ.Text + ";报损金额合计：" + labelJEHJ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库存盘点表(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";库房名称：" + textBoxKFMC.Text + ";报损数量合计：" + labelSLHJ.Text + ";报损金额合计：" + labelJEHJ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void textBoxSPLB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getClassInformation(10, textBoxSPLB.Text) == 0) //失败
                {
                    textBoxSPLB.Text = "";
                    intClassID = 0;
                }
                else
                {
                    intClassID = cGetInformation.iClassNumber;
                    textBoxSPLB.Text = cGetInformation.strClassName;
                }
                initdataGridViewDJMX();
            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {

                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0) //失败
                {
                    textBoxSPMC.Text = "";
                    intCommID = 0;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPMC.Text = cGetInformation.strCommName;

                    //得到库房
                    intKFID = cGetInformation.iKFNumber;
                    textBoxPDKF.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;

                }
                initdataGridViewDJMX();
            }
        }





    }
}