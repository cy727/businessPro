using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{

    public partial class FormBZXG : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";

        public int iDJLX = -1;
        public int intDJID = 0;

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private ClassGetInformation cGetInformation;

        private bool bCheck = true;
        private int iBM = 0;

        public FormBZXG()
        {
            InitializeComponent();
        }

        private void FormBZXG_Load(object sender, EventArgs e)
        {
            int i;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            if (iDJLX == -1 || intDJID == 0)
            {
                this.Close();
                return;
            }


            sqlConn.Open();

            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            //comboBoxYWY.Text = strUserName;

            switch (iDJLX)
            {
                case 0://进货入库单
                    labelDJ.Text = "购进商品制单";
                    sqlComm.CommandText = "SELECT 购进商品制单表.单据编号, 购进商品制单表.日期, 业务员.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 购进商品制单表.备注,单位表.单位编号, 单位表.单位名称, 购进商品制单表.价税合计, 采购合同表.合同编号, 购进商品制单表.发票号, 购进商品制单表.付款方式, 购进商品制单表.部门ID, 购进商品制单表.金额 FROM 购进商品制单表 INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 业务员 ON 购进商品制单表.业务员ID = 业务员.ID INNER JOIN 职员表 操作员 ON 购进商品制单表.操作员ID = 操作员.ID LEFT OUTER JOIN 采购合同表 ON 购进商品制单表.合同ID = 采购合同表.ID WHERE (购进商品制单表.ID = " + intDJID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                        comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                        labelCZY.Text = sqldr.GetValue(3).ToString();
                        textBoxBZ.Text = sqldr.GetValue(4).ToString();
                        textBoxBZXG.Text = sqldr.GetValue(4).ToString();

                        textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                        textBoxDWMC.Text = sqldr.GetValue(6).ToString();

                        if (sqldr.GetValue(11).ToString() != "")
                        {
                            try
                            {
                                iBM = int.Parse(sqldr.GetValue(11).ToString());
                            }
                            catch
                            {
                                iBM = 0;
                            }

                        }

                        labelSJJE.Text = sqldr.GetValue(7).ToString();
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

                    //初始化商品列表
                    sqlComm.CommandText = "SELECT 购进商品制单明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 购进商品制单明细表.数量, 购进商品制单明细表.单价, 购进商品制单明细表.金额, 购进商品制单明细表.赠品, 购进商品制单明细表.扣率, 购进商品制单明细表.实计金额, 商品表.ID AS 商品ID, 库房表.ID AS 库房ID, 商品表.最终进价, 购进商品制单明细表.ID AS 保留ID, 商品表.最高进价, 商品表.最低进价, 商品表.库存数量 FROM 购进商品制单明细表 INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 购进商品制单明细表.库房ID = 库房表.ID WHERE (购进商品制单明细表.表单ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
                    sqlDA.Fill(dSet, "商品表");
                    dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[13].Visible = false;
                    dataGridViewDJMX.Columns[14].Visible = false;
                    dataGridViewDJMX.Columns[16].Visible = false;

                    dataGridViewDJMX.Columns["最终进价"].Visible = false;
                    dataGridViewDJMX.Columns["最高进价"].Visible = false;
                    dataGridViewDJMX.Columns["最低进价"].Visible = false;

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
                    dataGridViewDJMX.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[18].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[19].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.ShowCellErrors = true;

                    dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
                    dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[12].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[15].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[17].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[18].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[19].DefaultCellStyle.Format = "f0";

                    toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
                    break;
                case 1://销售出库单
                    labelDJ.Text = "销售出库制单";
                    sqlComm.CommandText = "SELECT 销售商品制单表.单据编号, 销售商品制单表.日期, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 销售商品制单表.备注, 单位表.单位编号, 单位表.单位名称, 销售合同表.合同编号, 销售商品制单表.联系电话, 销售商品制单表.联系人, 销售商品制单表.收货人, 销售商品制单表.到站, 销售商品制单表.运输方式, 销售商品制单表.详细地址, 销售商品制单表.物流名称, 销售商品制单表.单号, 销售商品制单表.邮政编码, 销售商品制单表.部门ID, 销售商品制单表.价税合计 FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售商品制单表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 销售商品制单表.操作员ID = 操作员.ID LEFT OUTER JOIN 销售合同表 ON 销售商品制单表.合同ID = 销售合同表.ID WHERE (销售商品制单表.ID = " + intDJID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                        if (sqldr.GetValue(17).ToString() != "")
                        {
                            try
                            {
                                iBM = int.Parse(sqldr.GetValue(17).ToString());
                            }
                            catch
                            {
                                iBM = 0;
                            }

                        }

                        //comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                        comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                        labelCZY.Text = sqldr.GetValue(3).ToString();
                        textBoxBZ.Text = sqldr.GetValue(4).ToString();
                        textBoxBZXG.Text = sqldr.GetValue(4).ToString();
                        textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                        textBoxDWMC.Text = sqldr.GetValue(6).ToString();

                        labelSJJE.Text = sqldr.GetValue(18).ToString();

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

                    //初始化商品列表
                    sqlComm.CommandText = "SELECT 销售商品制单明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.数量, 销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.库存成本价, 销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价 AS 成本金额, 销售商品制单明细表.毛利, 销售商品制单明细表.赠品, 销售商品制单明细表.扣率, 销售商品制单明细表.实计金额, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID, 商品表.库存数量, 库房表.ID AS 统计标记 FROM 销售商品制单明细表 INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID WHERE (销售商品制单明细表.表单ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
                    sqlDA.Fill(dSet, "商品表");
                    dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[15].Visible = false;
                    dataGridViewDJMX.Columns[16].Visible = false;
                    dataGridViewDJMX.Columns[18].Visible = false;

                    dataGridViewDJMX.Columns["库存成本价"].Visible = false;
                    dataGridViewDJMX.Columns["成本金额"].Visible = false;
                    dataGridViewDJMX.Columns["毛利"].Visible = false;

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
                    dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
                    dataGridViewDJMX.ShowCellErrors = true;

                    dataGridViewDJMX.ReadOnly = true;
                    dataGridViewDJMX.AllowUserToAddRows = false;
                    dataGridViewDJMX.AllowUserToDeleteRows = false;

                    toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();

                    break;
                case 2://购进退出单
                    labelDJ.Text = "购进退出单";
                    sqlComm.CommandText = "SELECT 进货退出汇总表.单据编号, 进货退出汇总表.日期, 业务员.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 进货退出汇总表.备注, 单位表.单位编号, 单位表.单位名称, 进货退出汇总表.价税合计, 进货退出汇总表.发票号, 进货退出汇总表.支票号, 进货退出汇总表.部门ID FROM 进货退出汇总表 INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID INNER JOIN 职员表 业务员 ON 进货退出汇总表.业务员ID = 业务员.ID INNER JOIN 职员表 操作员 ON 进货退出汇总表.操作员ID = 操作员.ID WHERE (进货退出汇总表.ID = " + intDJID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                        if (sqldr.GetValue(10).ToString() != "")
                        {
                            try
                            {
                                iBM = int.Parse(sqldr.GetValue(10).ToString());
                            }
                            catch
                            {
                                iBM = 0;
                            }

                        }

                        //comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                        comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                        labelCZY.Text = sqldr.GetValue(3).ToString();
                        textBoxBZ.Text = sqldr.GetValue(4).ToString();
                        textBoxBZXG.Text = sqldr.GetValue(4).ToString();
                        textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                        textBoxDWMC.Text = sqldr.GetValue(6).ToString();

                        labelSJJE.Text = sqldr.GetValue(7).ToString();

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

                    //初始化商品列表
                    sqlComm.CommandText = "SELECT 进货退出明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 进货退出明细表.数量, 进货退出明细表.单价, 进货退出明细表.金额, 进货退出明细表.扣率, 进货退出明细表.实计金额, 进货退出明细表.商品ID, 进货退出明细表.库房ID, 商品表.库存数量, 进货退出明细表.ID AS Expr1, 进货退出明细表.赠品, 进货退出明细表.ID AS Expr2 FROM 进货退出明细表 INNER JOIN 商品表 ON 进货退出明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 进货退出明细表.库房ID = 库房表.ID WHERE (进货退出明细表.单据ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
                    sqlDA.Fill(dSet, "商品表");
                    dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[12].Visible = false;
                    dataGridViewDJMX.Columns[13].Visible = false;
                    dataGridViewDJMX.Columns[15].Visible = false;
                    dataGridViewDJMX.Columns[17].Visible = false;

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
                    dataGridViewDJMX.ShowCellErrors = true;

                    dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
                    dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[14].DefaultCellStyle.Format = "f0";


                    toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();

                    break;
                case 3://销售退回单
                    labelDJ.Text = "销售退出制单";
                    sqlComm.CommandText = "SELECT 销售退出汇总表.单据编号, 销售退出汇总表.日期, 业务员.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 销售退出汇总表.备注, 单位表.单位编号, 单位表.单位名称, 销售退出汇总表.价税合计, 销售退出汇总表.发票号, 销售退出汇总表.支票号, 销售退出汇总表.部门ID FROM 销售退出汇总表 INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID INNER JOIN 职员表 业务员 ON 销售退出汇总表.业务员ID = 业务员.ID INNER JOIN 职员表 操作员 ON 销售退出汇总表.操作员ID = 操作员.ID WHERE (销售退出汇总表.ID = " + intDJID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                        if (sqldr.GetValue(10).ToString() != "")
                        {
                            try
                            {
                                iBM = int.Parse(sqldr.GetValue(10).ToString());
                            }
                            catch
                            {
                                iBM = 0;
                            }

                        }

                        //comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                        comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                        labelCZY.Text = sqldr.GetValue(3).ToString();
                        textBoxBZ.Text = sqldr.GetValue(4).ToString();
                        textBoxBZXG.Text = sqldr.GetValue(4).ToString();
                        textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                        textBoxDWMC.Text = sqldr.GetValue(6).ToString();

                        labelSJJE.Text = sqldr.GetValue(7).ToString();
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

                    //初始化商品列表
                    sqlComm.CommandText = "SELECT 销售退出明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 销售退出明细表.数量, 销售退出明细表.单价, 销售退出明细表.金额, 销售退出明细表.扣率, 销售退出明细表.实计金额, 销售退出明细表.商品ID, 销售退出明细表.库房ID, 商品表.库存数量, 销售退出明细表.ID AS Expr1, 销售退出明细表.赠品, 销售退出明细表.ID AS Expr2 FROM 销售退出明细表 INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 销售退出明细表.库房ID = 库房表.ID WHERE (销售退出明细表.单据ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
                    sqlDA.Fill(dSet, "商品表");
                    dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[12].Visible = false;
                    dataGridViewDJMX.Columns[13].Visible = false;
                    dataGridViewDJMX.Columns[15].Visible = false;
                    dataGridViewDJMX.Columns[17].Visible = false;

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

                    dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
                    dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[14].DefaultCellStyle.Format = "f0";


                    toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
                    break;
                case 4://应付账款单
                    labelDJ.Text = "应付账款结算单";
                    sqlComm.CommandText = "SELECT 结算付款汇总表.单据编号, 结算付款汇总表.日期, 业务员.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 结算付款汇总表.备注, 单位表.单位编号, 单位表.单位名称, 结算付款汇总表.发票号, 单位表.税号, 单位表.应付账款, 结算付款汇总表.部门ID,  结算付款汇总表.备注2, 结算付款汇总表.实计金额 FROM 单位表 INNER JOIN 结算付款汇总表 ON 单位表.ID = 结算付款汇总表.单位ID INNER JOIN 职员表 业务员 ON 结算付款汇总表.业务员ID = 业务员.ID INNER JOIN 职员表 操作员 ON 结算付款汇总表.操作员ID = 操作员.ID WHERE (结算付款汇总表.ID = " + intDJID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        //labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");
                        labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                        if (sqldr.GetValue(10).ToString() != "")
                        {
                            try
                            {
                                iBM = int.Parse(sqldr.GetValue(10).ToString());
                            }
                            catch
                            {
                                iBM = 0;
                            }

                        }

                        //comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                        comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                        labelCZY.Text = sqldr.GetValue(3).ToString();
                        textBoxBZ.Text = sqldr.GetValue(4).ToString();
                        textBoxBZXG.Text = sqldr.GetValue(4).ToString();
                        textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                        textBoxDWMC.Text = sqldr.GetValue(6).ToString();
                        textBoxBZ2.Text = sqldr.GetValue(11).ToString();
                        textBoxBZ2XG.Text = sqldr.GetValue(11).ToString();

                        labelSJJE.Text = sqldr.GetValue(12).ToString(); 
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

                    //初始化商品列表
                    sqlComm.CommandText = "SELECT 结算付款明细表.ID, 账簿表.账簿编号, 账簿表.账簿名称, 结算付款明细表.摘要, 结算付款明细表.冲应付款, 账簿表.扣率, 结算付款明细表.付款金额, 结算付款明细表.支票号, 结算付款明细表.备注, 结算付款明细表.账簿ID, 结算付款定义表.勾兑标记, 结算付款定义表.勾兑纪录 FROM 账簿表 INNER JOIN 结算付款明细表 ON 账簿表.ID = 结算付款明细表.账簿ID CROSS JOIN 结算付款定义表 WHERE (结算付款明细表.单据ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                    sqlDA.Fill(dSet, "单据明细表");
                    dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[9].Visible = false;
                    dataGridViewDJMX.Columns[10].Visible = false;
                    dataGridViewDJMX.Columns[11].Visible = false;
                    dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.ShowCellErrors = true;

                    toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
                    break;
                case 5://应收账款单
                    labelDJ.Text = "应收账款结算单";
                    sqlComm.CommandText = "SELECT 结算收款汇总表.单据编号, 结算收款汇总表.日期, 业务员.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 结算收款汇总表.备注, 单位表.单位编号, 单位表.单位名称, 结算收款汇总表.发票号, 单位表.税号, 单位表.应收账款, 结算收款汇总表.部门ID, 结算收款汇总表.备注2, 结算收款汇总表.实计金额 FROM 单位表 INNER JOIN 结算收款汇总表 ON 单位表.ID = 结算收款汇总表.单位ID INNER JOIN 职员表 业务员 ON 结算收款汇总表.业务员ID = 业务员.ID INNER JOIN 职员表 操作员 ON 结算收款汇总表.操作员ID = 操作员.ID WHERE (结算收款汇总表.ID = " + intDJID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        //labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");
                        labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                        if (sqldr.GetValue(10).ToString() != "")
                        {
                            try
                            {
                                iBM = int.Parse(sqldr.GetValue(10).ToString());
                            }
                            catch
                            {
                                iBM = 0;
                            }

                        }

                        //comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                        comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                        labelCZY.Text = sqldr.GetValue(3).ToString();
                        textBoxBZ.Text = sqldr.GetValue(4).ToString();
                        textBoxBZXG.Text = sqldr.GetValue(4).ToString();
                        textBoxBZ2.Text = sqldr.GetValue(11).ToString();
                        textBoxBZ2XG.Text = sqldr.GetValue(11).ToString();

                        textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                        textBoxDWMC.Text = sqldr.GetValue(6).ToString();

                        labelSJJE.Text = sqldr.GetValue(12).ToString(); 
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

                    //初始化商品列表
                    sqlComm.CommandText = "SELECT 结算收款明细表.ID, 账簿表.账簿编号, 账簿表.账簿名称, 结算收款明细表.摘要, 结算收款明细表.冲应付款, 账簿表.扣率, 结算收款明细表.付款金额, 结算收款明细表.支票号, 结算收款明细表.备注, 结算收款明细表.账簿ID, 结算收款定义表.勾兑标记, 结算收款定义表.勾兑纪录 FROM 账簿表 INNER JOIN 结算收款明细表 ON 账簿表.ID = 结算收款明细表.账簿ID CROSS JOIN 结算收款定义表 WHERE (结算收款明细表.单据ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                    sqlDA.Fill(dSet, "单据明细表");
                    dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[9].Visible = false;
                    dataGridViewDJMX.Columns[10].Visible = false;
                    dataGridViewDJMX.Columns[11].Visible = false;
                    dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.ShowCellErrors = true;

                    toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
                    break;
                case 6://借物出库单
                    labelDJ.Text = "借物出库单";
                    sqlComm.CommandText = "SELECT 借物出库汇总表.单据编号, 借物出库汇总表.日期, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 借物出库汇总表.备注, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.联系电话, 借物出库汇总表.联系人, 借物出库汇总表.收货人, 借物出库汇总表.到站, 借物出库汇总表.运输方式, 借物出库汇总表.详细地址, 借物出库汇总表.物流名称, 借物出库汇总表.单号, 借物出库汇总表.邮政编码, 借物出库汇总表.部门ID, 借物出库汇总表.出库金额 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 借物出库汇总表.操作员ID = 操作员.ID WHERE (借物出库汇总表.ID = " + intDJID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        if (sqldr.GetValue(16).ToString() != "")
                        {
                            try
                            {
                                iBM = int.Parse(sqldr.GetValue(16).ToString());
                            }
                            catch
                            {
                                iBM = 0;
                            }

                        }

                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                        //comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                        comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                        labelCZY.Text = sqldr.GetValue(3).ToString();
                        textBoxBZ.Text = sqldr.GetValue(4).ToString();
                        textBoxBZXG.Text = sqldr.GetValue(4).ToString();
                        textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                        textBoxDWMC.Text = sqldr.GetValue(6).ToString();

                        labelSJJE.Text = sqldr.GetValue(17).ToString();
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
                    //初始化商品列表
                    sqlComm.CommandText = "SELECT 借物出库明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库房表.库房编号, 库房表.库房名称, 借物出库明细表.数量, 借物出库明细表.单价, 借物出库明细表.金额, 借物出库明细表.库存成本价 AS 成本单价, 借物出库明细表.出库金额, 借物出库明细表.备注, 借物出库明细表.商品ID, 借物出库明细表.库房ID, 商品表.库存数量, 销售商品定义表.统计标志, 商品表.最终进价 FROM 借物出库明细表 INNER JOIN 商品表 ON 借物出库明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID CROSS JOIN 销售商品定义表 WHERE (借物出库明细表.表单ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
                    sqlDA.Fill(dSet, "商品表");
                    dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[12].Visible = false;
                    dataGridViewDJMX.Columns[13].Visible = false;
                    dataGridViewDJMX.Columns[15].Visible = false;

                    dataGridViewDJMX.Columns["成本单价"].Visible = false;
                    dataGridViewDJMX.Columns["出库金额"].Visible = false;
                    dataGridViewDJMX.Columns["最终进价"].Visible = false;

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
                    dataGridViewDJMX.ShowCellErrors = true;

                    dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
                    dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[16].DefaultCellStyle.Format = "f2";                    
                    toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
                    break;
                case 7: //进货入库验货单
                    labelDJ.Text = "进货入库验货单";
                    sqlComm.CommandText = "SELECT 进货入库汇总表.单据编号, 进货入库汇总表.日期, 业务员.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 进货入库汇总表.备注, 单位表.单位编号, 单位表.单位名称, 进货入库汇总表.价税合计, 购进商品制单表.单据编号 AS 购进单号, 进货入库汇总表.发票号, 进货入库汇总表.支票号, 进货入库汇总表.部门ID FROM 进货入库汇总表 INNER JOIN 单位表 ON 进货入库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 业务员 ON 进货入库汇总表.业务员ID = 业务员.ID INNER JOIN 职员表 操作员 ON 进货入库汇总表.操作员ID = 操作员.ID LEFT OUTER JOIN 购进商品制单表 ON 进货入库汇总表.购进ID = 购进商品制单表.ID WHERE (进货入库汇总表.ID = " + intDJID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                        if (sqldr.GetValue(11).ToString() != "")
                        {
                            try
                            {
                                iBM = int.Parse(sqldr.GetValue(11).ToString());
                            }
                            catch
                            {
                                iBM = 0;
                            }

                        }

                        //comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                        comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                        labelCZY.Text = sqldr.GetValue(3).ToString();
                        textBoxBZ.Text = sqldr.GetValue(4).ToString();
                        textBoxBZXG.Text = sqldr.GetValue(4).ToString();
                        textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                        textBoxDWMC.Text = sqldr.GetValue(6).ToString();
                        labelSJJE.Text = sqldr.GetValue(7).ToString();

                        this.Text = "进货入库验货单：" + labelDJBH.Text;
                    }
                    sqldr.Close();

                    /*
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
                        */
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

                    //初始化商品列表
                    sqlComm.CommandText = "SELECT CONVERT(bit, 1) AS 到货, 购进商品制单表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称,进货入库明细表.数量, 进货入库明细表.单价, 进货入库明细表.金额, 进货入库明细表.扣率, 进货入库明细表.实计金额, 进货入库明细表.数量 AS 未到货数量, 进货入库明细表.商品ID, 进货入库明细表.库房ID, 进货入库明细表.ID AS Expr1, 进货入库明细表.赠品, 进货入库明细表.ID AS Expr2 FROM 进货入库明细表 INNER JOIN 商品表 ON 进货入库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 进货入库明细表.库房ID = 库房表.ID INNER JOIN 购进商品制单表 ON 进货入库明细表.原单据ID = 购进商品制单表.ID WHERE (进货入库明细表.单据ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
                    sqlDA.Fill(dSet, "商品表");
                    dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
                    dataGridViewDJMX.Columns[14].Visible = false;
                    dataGridViewDJMX.Columns[15].Visible = false;
                    dataGridViewDJMX.Columns[16].Visible = false;
                    dataGridViewDJMX.Columns[18].Visible = false;
                    dataGridViewDJMX.Columns[13].Visible = false;


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
                    dataGridViewDJMX.ShowCellErrors = true;

                    dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
                    dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[12].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[13].DefaultCellStyle.Format = "f0";

                    dataGridViewDJMX.ReadOnly = true;
                    dataGridViewDJMX.AllowUserToAddRows = false;
                    dataGridViewDJMX.AllowUserToDeleteRows = false;
                    toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();

                    break;

                case 8: //销售出库校对单
                    labelDJ.Text = "销售出库校对单";
                    sqlComm.CommandText = "SELECT 销售出库汇总表.单据编号, 销售出库汇总表.日期, 职员表.职员姓名, [职员表_1].职员姓名 AS Expr1, 销售出库汇总表.备注, 单位表.单位编号, 单位表.单位名称, 销售出库汇总表.发票号, 销售出库汇总表.支票号, 合同号,销售出库汇总表.部门ID,销售出库汇总表.价税合计 FROM 销售出库汇总表 INNER JOIN 职员表 ON 销售出库汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 销售出库汇总表.操作员ID = [职员表_1].ID INNER JOIN 单位表 ON 销售出库汇总表.单位ID = 单位表.ID WHERE (销售出库汇总表.ID = " + intDJID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                        if (sqldr.GetValue(10).ToString() != "")
                        {
                            try
                            {
                                iBM = int.Parse(sqldr.GetValue(10).ToString());
                            }
                            catch
                            {
                                iBM = 0;
                            }

                        }

                        comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                        labelCZY.Text = sqldr.GetValue(3).ToString();
                        textBoxBZ.Text = sqldr.GetValue(4).ToString();
                        textBoxBZXG.Text = sqldr.GetValue(4).ToString();
                        textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                        textBoxDWMC.Text = sqldr.GetValue(6).ToString();
                        labelSJJE.Text = sqldr.GetValue(11).ToString();

                        this.Text = "销售出库校对单：" + labelDJBH.Text;
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


                    //初始化商品列表
                    sqlComm.CommandText = "SELECT CONVERT(bit, 1) AS 校对, 销售商品制单表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 销售出库明细表.数量, 销售出库明细表.单价, 销售出库明细表.金额, 销售出库明细表.扣率, 销售出库明细表.实计金额, 销售出库明细表.数量 AS 未出库数量, 销售出库明细表.商品ID, 销售出库明细表.库房ID, 销售出库明细表.ID, 销售出库明细表.赠品, 销售出库明细表.单据ID, 销售出库明细表.毛利 FROM 销售出库明细表 INNER JOIN 商品表 ON 销售出库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 销售出库明细表.库房ID = 库房表.ID INNER JOIN 销售商品制单表 ON 销售出库明细表.原单据ID = 销售商品制单表.ID WHERE (销售出库明细表.单据ID = " + intDJID.ToString() + ")";


                     if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
                    sqlDA.Fill(dSet, "商品表");
                    dataGridViewDJMX.DataSource = dSet.Tables["商品表"];


                    dataGridViewDJMX.Columns[14].Visible = false;
                    dataGridViewDJMX.Columns[15].Visible = false;
                    dataGridViewDJMX.Columns[16].Visible = false;
                    dataGridViewDJMX.Columns[18].Visible = false;

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
                    dataGridViewDJMX.Columns[19].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                    dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
                    dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[12].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[19].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[13].DefaultCellStyle.Format = "f0";


                    dataGridViewDJMX.ReadOnly = true;
                    dataGridViewDJMX.AllowUserToAddRows = false;
                    dataGridViewDJMX.AllowUserToDeleteRows = false;
                    toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();

                    break;
                default:
                    break;
            }

            sqlConn.Close();
           
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i;
            System.Data.SqlClient.SqlTransaction sqlta;

            switch (iDJLX)
            {
                case 0://进货入库单
                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        if(comboBoxYWY.SelectedValue.ToString()=="")
                            sqlComm.CommandText = "UPDATE 购进商品制单表 SET 备注 = N'" + textBoxBZXG.Text + "' WHERE (ID = "+intDJID.ToString()+")";
                        else
                            sqlComm.CommandText = "UPDATE 购进商品制单表 SET 备注 = N'" + textBoxBZXG.Text + "', 业务员ID="+comboBoxYWY.SelectedValue.ToString()+"  WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //历史纪录
                        sqlComm.CommandText = "INSERT INTO 备注修改记录表 (日期, 操作员ID, 单号, 原备注, 备注, 原备注2, 备注2) VALUES (GETDATE(), "+intUserID.ToString()+", N'"+labelDJBH.Text+"', N'"+textBoxBZ.Text+"', N'"+textBoxBZXG.Text+"', N'"+textBoxBZ2.Text+"', N'"+textBoxBZ2XG.Text+"')";
                        sqlComm.ExecuteNonQuery();

                        sqlta.Commit();
                        MessageBox.Show("单据备注修改完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    break;
                case 1://销售出库单
                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        if (comboBoxYWY.SelectedValue.ToString() == "")
                            sqlComm.CommandText = "UPDATE 销售商品制单表 SET 备注 = N'" + textBoxBZXG.Text + "' WHERE (ID = " + intDJID.ToString() + ")";
                        else
                            sqlComm.CommandText = "UPDATE 销售商品制单表 SET 备注 = N'" + textBoxBZXG.Text + "', 业务员ID=" + comboBoxYWY.SelectedValue.ToString() + "  WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //历史纪录
                        sqlComm.CommandText = "INSERT INTO 备注修改记录表 (日期, 操作员ID, 单号, 原备注, 备注, 原备注2, 备注2) VALUES (GETDATE(), "+intUserID.ToString()+", N'"+labelDJBH.Text+"', N'"+textBoxBZ.Text+"', N'"+textBoxBZXG.Text+"', N'"+textBoxBZ2.Text+"', N'"+textBoxBZ2XG.Text+"')";
                        sqlComm.ExecuteNonQuery();

                        sqlta.Commit();
                        MessageBox.Show("单据备注修改完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    break;
                case 2://购进退出单
                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        if (comboBoxYWY.SelectedValue.ToString() == "")
                            sqlComm.CommandText = "UPDATE 进货退出汇总表 SET 备注 = N'" + textBoxBZXG.Text + "' WHERE (ID = " + intDJID.ToString() + ")";
                        else
                            sqlComm.CommandText = "UPDATE 进货退出汇总表 SET 备注 = N'" + textBoxBZXG.Text + "', 业务员ID=" + comboBoxYWY.SelectedValue.ToString() + " WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //历史纪录
                        sqlComm.CommandText = "INSERT INTO 备注修改记录表 (日期, 操作员ID, 单号, 原备注, 备注, 原备注2, 备注2) VALUES (GETDATE(), "+intUserID.ToString()+", N'"+labelDJBH.Text+"', N'"+textBoxBZ.Text+"', N'"+textBoxBZXG.Text+"', N'"+textBoxBZ2.Text+"', N'"+textBoxBZ2XG.Text+"')";
                        sqlComm.ExecuteNonQuery();

                        sqlta.Commit();
                        MessageBox.Show("单据备注修改完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    break;
                case 3://销售退回单
                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        if (comboBoxYWY.SelectedValue.ToString() == "")
                            sqlComm.CommandText = "UPDATE 销售退出汇总表 SET 备注 = N'" + textBoxBZXG.Text + "' WHERE (ID = " + intDJID.ToString() + ")";
                        else
                            sqlComm.CommandText = "UPDATE 销售退出汇总表 SET 备注 = N'" + textBoxBZXG.Text + "', 业务员ID=" + comboBoxYWY.SelectedValue.ToString() + "  WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //历史纪录
                        sqlComm.CommandText = "INSERT INTO 备注修改记录表 (日期, 操作员ID, 单号, 原备注, 备注, 原备注2, 备注2) VALUES (GETDATE(), "+intUserID.ToString()+", N'"+labelDJBH.Text+"', N'"+textBoxBZ.Text+"', N'"+textBoxBZXG.Text+"', N'"+textBoxBZ2.Text+"', N'"+textBoxBZ2XG.Text+"')";
                        sqlComm.ExecuteNonQuery();

                        sqlta.Commit();
                        MessageBox.Show("单据备注修改完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    break;
                case 4://应付账款单
                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        if (comboBoxYWY.SelectedValue.ToString() == "")
                            sqlComm.CommandText = "UPDATE 结算付款汇总表 SET 备注 = N'" + textBoxBZXG.Text + "', 备注2 = N'" + textBoxBZ2XG.Text + "' WHERE (ID = " + intDJID.ToString() + ")";
                        else
                            sqlComm.CommandText = "UPDATE 结算付款汇总表 SET 备注 = N'" + textBoxBZXG.Text + "', 备注2 = N'" + textBoxBZ2XG.Text + "', 业务员ID=" + comboBoxYWY.SelectedValue.ToString() + "  WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //历史纪录
                        sqlComm.CommandText = "INSERT INTO 备注修改记录表 (日期, 操作员ID, 单号, 原备注, 备注, 原备注2, 备注2) VALUES (GETDATE(), "+intUserID.ToString()+", N'"+labelDJBH.Text+"', N'"+textBoxBZ.Text+"', N'"+textBoxBZXG.Text+"', N'"+textBoxBZ2.Text+"', N'"+textBoxBZ2XG.Text+"')";
                        sqlComm.ExecuteNonQuery();

                        sqlta.Commit();
                        MessageBox.Show("单据备注修改完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    break;
                case 5://应收账款单
                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        if (comboBoxYWY.SelectedValue.ToString() == "")
                            sqlComm.CommandText = "UPDATE 结算收款汇总表 SET 备注 = N'" + textBoxBZXG.Text + "', 备注2 = N'" + textBoxBZ2XG.Text + "' WHERE (ID = " + intDJID.ToString() + ")";
                        else
                            sqlComm.CommandText = "UPDATE 结算收款汇总表 SET 备注 = N'" + textBoxBZXG.Text + "', 备注2 = N'" + textBoxBZ2XG.Text + "', 业务员ID=" + comboBoxYWY.SelectedValue.ToString() + "  WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //历史纪录
                        sqlComm.CommandText = "INSERT INTO 备注修改记录表 (日期, 操作员ID, 单号, 原备注, 备注, 原备注2, 备注2) VALUES (GETDATE(), "+intUserID.ToString()+", N'"+labelDJBH.Text+"', N'"+textBoxBZ.Text+"', N'"+textBoxBZXG.Text+"', N'"+textBoxBZ2.Text+"', N'"+textBoxBZ2XG.Text+"')";
                        sqlComm.ExecuteNonQuery();

                        sqlta.Commit();
                        MessageBox.Show("单据备注修改完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    break;
                case 6://借物出库单
                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        if (comboBoxYWY.SelectedValue.ToString() == "")
                            sqlComm.CommandText = "UPDATE 借物出库汇总表 SET 备注 = N'" + textBoxBZXG.Text + "' WHERE (ID = " + intDJID.ToString() + ")";
                        else
                            sqlComm.CommandText = "UPDATE 借物出库汇总表 SET 备注 = N'" + textBoxBZXG.Text + "', 业务员ID=" + comboBoxYWY.SelectedValue.ToString() + "  WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //历史纪录
                        sqlComm.CommandText = "INSERT INTO 备注修改记录表 (日期, 操作员ID, 单号, 原备注, 备注, 原备注2, 备注2) VALUES (GETDATE(), "+intUserID.ToString()+", N'"+labelDJBH.Text+"', N'"+textBoxBZ.Text+"', N'"+textBoxBZXG.Text+"', N'"+textBoxBZ2.Text+"', N'"+textBoxBZ2XG.Text+"')";
                        sqlComm.ExecuteNonQuery();

                        sqlta.Commit();
                        MessageBox.Show("单据备注修改完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    break;
                case 7://进货验货单
                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        if (comboBoxYWY.SelectedValue.ToString() == "")
                            sqlComm.CommandText = "UPDATE 进货入库汇总表 SET 备注 = N'" + textBoxBZXG.Text + "' WHERE (ID = " + intDJID.ToString() + ")";
                        else
                            sqlComm.CommandText = "UPDATE 进货入库汇总表 SET 备注 = N'" + textBoxBZXG.Text + "', 业务员ID=" + comboBoxYWY.SelectedValue.ToString() + " WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //历史纪录
                        sqlComm.CommandText = "INSERT INTO 备注修改记录表 (日期, 操作员ID, 单号, 原备注, 备注, 原备注2, 备注2) VALUES (GETDATE(), " + intUserID.ToString() + ", N'" + labelDJBH.Text + "', N'" + textBoxBZ.Text + "', N'" + textBoxBZXG.Text + "', N'" + textBoxBZ2.Text + "', N'" + textBoxBZ2XG.Text + "')";
                        sqlComm.ExecuteNonQuery();

                        sqlta.Commit();
                        MessageBox.Show("单据备注修改完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    break;
                case 8://销售验货单
                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        if (comboBoxYWY.SelectedValue.ToString() == "")
                            sqlComm.CommandText = "UPDATE 销售出库汇总表 SET 备注 = N'" + textBoxBZXG.Text + "' WHERE (ID = " + intDJID.ToString() + ")";
                        else
                            sqlComm.CommandText = "UPDATE 销售出库汇总表 SET 备注 = N'" + textBoxBZXG.Text + "', 业务员ID=" + comboBoxYWY.SelectedValue.ToString() + "   WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //历史纪录
                        sqlComm.CommandText = "INSERT INTO 备注修改记录表 (日期, 操作员ID, 单号, 原备注, 备注, 原备注2, 备注2) VALUES (GETDATE(), " + intUserID.ToString() + ", N'" + labelDJBH.Text + "', N'" + textBoxBZ.Text + "', N'" + textBoxBZXG.Text + "', N'" + textBoxBZ2.Text + "', N'" + textBoxBZ2XG.Text + "')";
                        sqlComm.ExecuteNonQuery();

                        sqlta.Commit();
                        MessageBox.Show("单据备注修改完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    break;
                default:
                    break;

            }

            this.Close();
        }
    }
}
