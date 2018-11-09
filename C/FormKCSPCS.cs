using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKCSPCS : Form
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

        private int intKFID = 0;
        private int intCommID = 0;
        private int intCZZ = 0;

        private decimal dKC = 0;
        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;
        
        public FormKCSPCS()
        {
            InitializeComponent();
        }

        private void FormKCSPCS_Load(object sender, EventArgs e)
        {
            int i;

            this.Top = 1;
            this.Left = 1;

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
            sqlComm.CommandText = "SELECT 库存商品拆散汇总表.单据编号, 库存商品拆散汇总表.日期, [职员表_1].职员姓名, 操作员.职员姓名 AS 操作员, 库存商品拆散汇总表.备注, 库房表.库房编号, 库房表.库房名称, 库存商品组装汇总表.商品编号, 库存商品组装汇总表.商品名称, 库存商品拆散汇总表.拆散数量 FROM 库存商品拆散汇总表  INNER JOIN 职员表 操作员 ON 库存商品拆散汇总表.操作员ID = 操作员.ID INNER JOIN 职员表 [职员表_1] ON 库存商品拆散汇总表.业务员ID = [职员表_1].ID INNER JOIN 库存商品组装汇总表 ON  库存商品拆散汇总表.组装单据ID = 库存商品组装汇总表.ID INNER JOIN 库房表 ON 库存商品组装汇总表.成品库房ID = 库房表.ID WHERE (库存商品拆散汇总表.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                textBoxKFBH.Text = sqldr.GetValue(5).ToString();
                textBoxKFMC.Text = sqldr.GetValue(6).ToString();

                textBoxSPBH.Text = sqldr.GetValue(7).ToString();
                textBoxSPMC.Text = sqldr.GetValue(8).ToString();
                numericUpDownCPSL.Value = Convert.ToDecimal(sqldr.GetValue(9).ToString());


                this.Text = "库存商品拆散制单：" + labelDJBH.Text;
            }
            sqldr.Close();

            //初始化商品列表
            sqlComm.CommandText = "SELECT 库存商品组装明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库房表.库房编号, 库房表.库房名称, 库存商品组装明细表.组件数量, 库存商品组装明细表.成本单价, 库存商品组装明细表.成本金额, 库存商品组装明细表.备注, 库存商品组装明细表.组件ID, 库存商品组装明细表.库房ID, 商品表.库存数量, 库存商品组装定义表.统计标志 FROM 库存商品组装明细表 INNER JOIN 商品表 ON 库存商品组装明细表.组件ID = 商品表.ID INNER JOIN 库房表 ON 库存商品组装明细表.库房ID = 库房表.ID INNER JOIN 库存商品拆散汇总表 ON  库存商品组装明细表.ID = 库存商品拆散汇总表.组装单据ID CROSS JOIN 库存商品组装定义表 WHERE (库存商品拆散汇总表.ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(101, "") == 0) //失败
            {
                return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPBH.Text = cGetInformation.strCommCode;
                textBoxSPMC.Text = cGetInformation.strCommName;


            }

            getCPZZDetail();
        }

        private void getCPZZDetail()
        {
            if (intCommID == 0)
                return;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品表.库存数量, 商品表.库存成本价, 库房表.库房编号, 库房表.库房名称, 库存商品组装汇总表.组装费用, 库存商品组装汇总表.备注, 库存商品组装汇总表.ID, 库存商品组装汇总表.成品库房ID FROM 商品表 INNER JOIN 库存商品组装汇总表 ON 商品表.ID = 库存商品组装汇总表.商品ID INNER JOIN 库房表 ON 库存商品组装汇总表.成品库房ID = 库房表.ID WHERE (商品表.ID = " + intCommID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();

            if (!sqldr.HasRows)
            {
                intCommID = 0;
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            sqldr.Read();
            textBoxSPBH.Text = sqldr.GetValue(0).ToString();
            textBoxSPMC.Text = sqldr.GetValue(1).ToString();
            numericUpDownCPSL.Maximum = Convert.ToDecimal(sqldr.GetValue(2).ToString());
            numericUpDownCPSL.Value= Convert.ToDecimal(sqldr.GetValue(2).ToString());
            dKC = Convert.ToDecimal(sqldr.GetValue(2).ToString());
            labelSPCB.Text = sqldr.GetValue(3).ToString();
            textBoxKFBH.Text = sqldr.GetValue(4).ToString();
            textBoxKFMC.Text = sqldr.GetValue(5).ToString();
            numericUpDownZZFY.Value = Convert.ToDecimal(sqldr.GetValue(6).ToString());
            intCZZ = Int32.Parse(sqldr.GetValue(8).ToString());
            intKFID = Int32.Parse(sqldr.GetValue(9).ToString());
            sqldr.Close();

            //明细
            sqlComm.CommandText = "SELECT 库存商品组装明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库房表.库房编号, 库房表.库房名称, 库存商品组装明细表.组件数量, 库存商品组装明细表.成本单价, 库存商品组装明细表.成本金额, 库存商品组装明细表.备注, 库存商品组装明细表.组件ID, 库存商品组装明细表.库房ID, 商品表.库存数量, 库存商品组装定义表.统计标志 FROM 库存商品组装明细表 INNER JOIN 商品表 ON 库存商品组装明细表.组件ID = 商品表.ID INNER JOIN 库房表 ON 库存商品组装明细表.库房ID = 库房表.ID CROSS JOIN 库存商品组装定义表 WHERE (库存商品组装明细表.单据ID = "+intCZZ.ToString()+")";
            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据表"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString();


            sqlConn.Close();
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(102, textBoxSPMC.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;


                }

                getCPZZDetail();
            }
        }


        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(102, textBoxSPBH.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;


                }

                getCPZZDetail();
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dKUL = 0, dKCCBJ = 0, dML = 0, dSJJE = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dML1 = 0, dSJJE1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;
            decimal fTemp = 0, fTemp1 = 0, fTemp2=0;

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("商品拆散单已经保存,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intCommID == 0)
            {
                MessageBox.Show("请选择拆散商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (numericUpDownCPSL.Value == 0)
            {
                MessageBox.Show("请输入拆散成品数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }




            if (MessageBox.Show("请检查商品拆散单内容,该制单内容不可更改，是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            string strCount = "", strDateSYS = "", strKey = "CCS";
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

                //标志复位
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    dataGridViewDJMX.Rows[i].Cells[13].Value = 1;
                }
                //总库存
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[13].Value) == 0) //已经计算过
                        continue;

                    //计算该单的每个商品库存,金额
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[13].Value) == 0) //已经计算过
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[10].Value == dataGridViewDJMX.Rows[i].Cells[10].Value) //同种商品
                        {
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[6].Value);
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                            dataGridViewDJMX.Rows[j].Cells[13].Value = 0;
                        }

                    }
                    dKUL1 = dKUL1 * numericUpDownCPSL.Value;
                    dKCJE1 = dKCJE1 * numericUpDownCPSL.Value;

                    //总库存变更
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价 FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                    }
                    sqldr.Close();

                    dKUL += dKUL1;
                    dKCJE += dKUL1;

                    sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额=" + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //商品历史纪录
                    sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 业务员ID, 单据编号, 摘要, 组装数量, 组装单价, 组装金额, 总结存数量, 总结存金额, BeActive) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'库存商品拆散', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", 1)";
                    sqlComm.ExecuteNonQuery();
                }

                //标志复位
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    dataGridViewDJMX.Rows[i].Cells[13].Value = 1;
                }

                //分库存
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[13].Value) == 0) //已经计算过
                        continue;

                    //计算该单的每个商品数量
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[13].Value) == 0) //已经计算过
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[10].Value == dataGridViewDJMX.Rows[i].Cells[10].Value && dataGridViewDJMX.Rows[j].Cells[11].Value == dataGridViewDJMX.Rows[i].Cells[11].Value) //同种商品，同样库存
                        {
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[6].Value);
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                            dataGridViewDJMX.Rows[j].Cells[13].Value = 0;
                        }

                    }
                    dKUL1 = dKUL1 * numericUpDownCPSL.Value;
                    dKCJE1 = dKCJE1 * numericUpDownCPSL.Value;

                    //分库存更新
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ") AND (BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //存在库存
                    {
                        sqldr.Read();
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        sqldr.Close();

                        dKUL += dKUL1;
                        dKCJE += dKCJE1;
                        sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额=" + dKCJE.ToString() + " WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ") AND (BeActive = 1)";
                        sqlComm.ExecuteNonQuery();

                        //商品库房历史纪录
                        sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 业务员ID, 单据编号, 摘要, 组装数量, 组装单价, 组装金额, 库房结存数量, 库房结存金额, BeActive) VALUES (" + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'库存商品拆散', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", 1)";
                        sqlComm.ExecuteNonQuery();

                    }

                }

                if (dKC == numericUpDownCPSL.Value) //全部拆散
                {
                    sqlComm.CommandText = "UPDATE 商品表 SET beactive = 0 WHERE (ID = "+intCommID.ToString()+")";
                    sqlComm.ExecuteNonQuery();
                    sqlComm.CommandText = "DELETE FROM 库存表 WHERE (库房ID = "+intKFID.ToString()+") AND (商品ID = "+intCommID.ToString()+")";
                    sqlComm.ExecuteNonQuery();
                }



                //表单汇总
                sqlComm.CommandText = "INSERT INTO 库存商品拆散汇总表 (单据编号, 日期, 组装单据ID, 拆散数量, 备注, BeActive, 操作员ID, 业务员ID) VALUES (N'"+strCount+"', '"+strDateSYS+"', "+intCZZ.ToString()+", "+Convert.ToInt32(numericUpDownCPSL.Value).ToString()+", N'"+textBoxBZ.Text+"', 1, "+intUserID.ToString()+", "+comboBoxYWY.SelectedValue.ToString()+")";
                sqlComm.ExecuteNonQuery();


                //商品历史纪录
                dKUL1 = numericUpDownCPSL.Value;
                dKCCBJ1 = Convert.ToDecimal(labelSPCB.Text);
                dKCJE1 = dKUL * dKCCBJ;

                dKUL = dKC;
                dKUL -= dKUL1;
                dKCJE = dKUL * dKCCBJ1;


                sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 业务员ID, 单据编号, 摘要, 出库数量, 出库单价, 出库金额, 总结存数量, 总结存金额, BeActive) VALUES ('" + strDateSYS + "', " + intCommID.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'库存商品拆散', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCJE1.ToString() + ", 1)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 业务员ID, 单据编号, 摘要, 出库数量, 出库单价, 出库金额, 库房结存数量, 库房结存金额, BeActive) VALUES (" + intKFID.ToString() + ", '" + strDateSYS + "', " + intCommID.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'库存商品拆散', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCJE1.ToString() + ", 1)";
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

            //MessageBox.Show(" 库存商品拆散单保存成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = " 库存商品拆散制单：" + labelDJBH.Text;
            isSaved = true;

            if (MessageBox.Show("库存商品拆散单保存成功，是否关闭制单窗口", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }

        }

        private void FormKCSPCS_FormClosing(object sender, FormClosingEventArgs e)
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
            string strT = "库存商品拆散制单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";商品名称：" + textBoxSPMC.Text + "(编号:" + textBoxSPBH.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库存商品拆散制单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";商品名称：" + textBoxSPMC.Text + "(编号:" + textBoxSPBH.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}