using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormDJZX : Form
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

        private int iSupplyCompany = 0;
        private ClassGetInformation cGetInformation;

        private int iConstLimit = 18; 

        public FormDJZX()
        {
            InitializeComponent();
        }

        private void FormDJZX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;



            //得到开始时间
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 公司宣传, 质量目标1, 质量目标2, 质量目标3, 质量目标4, 管理员权限, 总经理权限, 职员权限, 经理权限, 业务员权限 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {

                try
                {
                    iConstLimit = int.Parse(sqldr.GetValue(6).ToString());
                }
                catch
                {
                    iConstLimit = 18;
                }
            }
            sqldr.Close();

            //sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
            sqlComm.CommandText = "SELECT GETDATE()";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");

            }
            sqldr.Close();

            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");

            object[] OTemp = new object[3];
            OTemp[0] = 0;
            OTemp[1] = "全部";
            OTemp[2] = "全部";
            dSet.Tables["职员表"].Rows.Add(OTemp);

            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.SelectedIndex= comboBoxYWY.Items.Count - 1;

            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表1")) dSet.Tables.Remove("职员表1");
            sqlDA.Fill(dSet, "职员表1");

            object[] OTemp1 = new object[3];
            OTemp1[0] = 0;
            OTemp1[1] = "全部";
            OTemp1[2] = "全部";
            dSet.Tables["职员表1"].Rows.Add(OTemp1);

            comboBoxCZY.DataSource = dSet.Tables["职员表1"];
            comboBoxCZY.DisplayMember = "职员姓名";
            comboBoxCZY.ValueMember = "ID";
            comboBoxCZY.SelectedIndex = comboBoxCZY.Items.Count - 1;
            sqlConn.Close();

            if (intUserLimit < 11)
            {
                comboBoxYWY.SelectedValue = intUserID;
                comboBoxYWY.Enabled = false;

                comboBoxCZY.SelectedValue = intUserID;
                comboBoxCZY.Enabled = false;
            }

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            comboBoxDJLB.SelectedIndex = 0;
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int c=6;

            sqlConn.Open();
            switch (comboBoxDJLB.SelectedIndex)
            {
                case 0:
                    sqlComm.CommandText = "SELECT 采购合同表.ID, 采购合同表.合同编号, 采购合同表.签订时间, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 采购合同表.金额 FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 采购合同表.操作员ID = 操作员.ID WHERE (采购合同表.BeActive = 1) AND (采购合同表.签订时间 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (采购合同表.签订时间 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString()+" OR "+"操作员.ID="+comboBoxCZY.SelectedValue.ToString()+")";
                    }


                    if (textBoxDJBH.Text.Trim()!="")
                        sqlComm.CommandText += " AND 采购合同表.合同编号 LIKE N'%" + textBoxDJBH.Text.Trim()+"%'";

                    sqlComm.CommandText += " ORDER BY  签订时间 DESC";
                    c = 7;

                    break;
                case 1:
                    sqlComm.CommandText = "SELECT 购进商品制单表.ID, 购进商品制单表.单据编号, 购进商品制单表.日期, 单位表.单位编号,单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 购进商品制单表.价税合计, 购进商品制单表.发票号, 购进商品制单表.备注 FROM 单位表 INNER JOIN 购进商品制单表 ON 单位表.ID = 购进商品制单表.单位ID INNER JOIN 职员表 ON 购进商品制单表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 购进商品制单表.操作员ID = 操作员.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 购进商品制单表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 7;
                    break;



                case 2:
                    sqlComm.CommandText = "SELECT 进货入库汇总表.ID, 进货入库汇总表.单据编号, 进货入库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 进货入库汇总表.价税合计, 进货入库汇总表.发票号, 进货入库汇总表.备注 FROM 单位表 INNER JOIN 进货入库汇总表 ON 单位表.ID = 进货入库汇总表.单位ID INNER JOIN 职员表 ON 进货入库汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 进货入库汇总表.操作员ID = 操作员.ID WHERE (进货入库汇总表.BeActive = 1) AND (进货入库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 进货入库汇总表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 7;
                    break;


                case 3:
                    sqlComm.CommandText = "SELECT 进货退出汇总表.ID, 进货退出汇总表.单据编号, 进货退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 进货退出汇总表.价税合计, 进货退出汇总表.支票号, 进货退出汇总表.备注 FROM 单位表 INNER JOIN 进货退出汇总表 ON 单位表.ID = 进货退出汇总表.单位ID INNER JOIN 职员表 ON 进货退出汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 进货退出汇总表.操作员ID = 操作员.ID WHERE (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (进货退出汇总表.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 进货退出汇总表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 7;
                    break;

                case 4:
                    sqlComm.CommandText = "SELECT 购进退补差价汇总表.ID, 购进退补差价汇总表.单据编号, 购进退补差价汇总表.日期, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 购进退补差价汇总表.价税合计, 购进退补差价汇总表.发票号, 购进退补差价汇总表.备注 FROM 单位表 INNER JOIN 购进退补差价汇总表 ON 单位表.ID = 购进退补差价汇总表.单位ID INNER JOIN 职员表 ON 购进退补差价汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 购进退补差价汇总表.操作员ID = 操作员.ID WHERE (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (购进退补差价汇总表.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 购进退补差价汇总表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 7;
                    break;

                case 5:
                    sqlComm.CommandText = "SELECT 结算付款汇总表.ID, 结算付款汇总表.单据编号, 结算付款汇总表.日期, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 结算付款汇总表.实计金额, 结算付款汇总表.发票号, 结算付款汇总表.备注, 结算付款汇总表.备注2 FROM 单位表 INNER JOIN 结算付款汇总表 ON 单位表.ID = 结算付款汇总表.单位ID INNER JOIN 职员表 ON 结算付款汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 结算付款汇总表.操作员ID = 操作员.ID WHERE (结算付款汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (结算付款汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (结算付款汇总表.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 结算付款汇总表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 7;
                    break;

                case 6:
                    sqlComm.CommandText = "SELECT 销售合同表.ID, 销售合同表.合同编号, 销售合同表.签订时间, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员,  销售合同表.金额 FROM 单位表 INNER JOIN 销售合同表 ON 单位表.ID = 销售合同表.供方单位ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 销售合同表.操作员ID = 操作员.ID WHERE (销售合同表.BeActive = 1) AND (销售合同表.签订时间 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售合同表.签订时间 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 销售合同表.合同编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  签订时间 DESC";
                    c = 7;
                    break;

                case 7:
                    sqlComm.CommandText = "SELECT 销售商品制单表.ID, 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 销售商品制单表.价税合计, 销售商品制单表.发票号, 销售商品制单表.备注 FROM 单位表 INNER JOIN 销售商品制单表 ON 单位表.ID = 销售商品制单表.单位ID INNER JOIN 职员表 ON 销售商品制单表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 销售商品制单表.操作员ID = 操作员.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 销售商品制单表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 7;
                    break;

                case 8:
                    sqlComm.CommandText = "SELECT 销售出库汇总表.ID, 销售出库汇总表.单据编号, 销售出库汇总表.日期,单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员,操作员.职员姓名 AS 操作员, 销售出库汇总表.价税合计, 销售出库汇总表.发票号, 销售出库汇总表.备注 FROM 单位表 INNER JOIN 销售出库汇总表 ON 单位表.ID = 销售出库汇总表.单位ID INNER JOIN 职员表 ON 销售出库汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 销售出库汇总表.操作员ID = 操作员.ID WHERE (销售出库汇总表.BeActive = 1) AND (销售出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 销售出库汇总表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 7;
                    break;

                case 9:
                    sqlComm.CommandText = "SELECT 销售退出汇总表.ID, 销售退出汇总表.单据编号, 销售退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 销售退出汇总表.价税合计, 销售退出汇总表.备注 FROM 单位表 INNER JOIN 销售退出汇总表 ON 单位表.ID = 销售退出汇总表.单位ID INNER JOIN 职员表 ON 销售退出汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 销售退出汇总表.操作员ID = 操作员.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退出汇总表.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 销售退出汇总表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 7;
                    break;

                case 10:
                    sqlComm.CommandText = "SELECT 销售退补差价汇总表.ID, 销售退补差价汇总表.单据编号, 销售退补差价汇总表.日期, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 销售退补差价汇总表.价税合计, 销售退补差价汇总表.发票号, 销售退补差价汇总表.备注 FROM 单位表 INNER JOIN 销售退补差价汇总表 ON 单位表.ID = 销售退补差价汇总表.单位ID INNER JOIN 职员表 ON 销售退补差价汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 销售退补差价汇总表.操作员ID = 操作员.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退补差价汇总表.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 销售退补差价汇总表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 7;
                    break;


                case 11:
                    sqlComm.CommandText = "SELECT 结算收款汇总表.ID, 结算收款汇总表.单据编号, 结算收款汇总表.日期, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 结算收款汇总表.实计金额, 结算收款汇总表.发票号, 结算收款汇总表.备注, 结算收款汇总表.备注2 FROM 单位表 INNER JOIN 结算收款汇总表 ON 单位表.ID = 结算收款汇总表.单位ID INNER JOIN 职员表 ON 结算收款汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 结算收款汇总表.操作员ID = 操作员.ID WHERE (结算收款汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (结算收款汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (结算收款汇总表.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 结算收款汇总表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 7;
                    break;

                case 12:
                    sqlComm.CommandText = "SELECT 库存盘点汇总表.ID, 库存盘点汇总表.单据编号, 库存盘点汇总表.日期, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 库存盘点汇总表.盘损数量合计, 库存盘点汇总表.盘损金额合计, 库存盘点汇总表.备注 FROM 职员表 INNER JOIN 库存盘点汇总表 ON 职员表.ID = 库存盘点汇总表.业务员ID INNER JOIN 职员表 操作员 ON 库存盘点汇总表.操作员ID = 操作员.ID WHERE (库存盘点汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (库存盘点汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (库存盘点汇总表.BeActive = 1)";

                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 库存盘点汇总表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 7;
                    break;

                case 13:
                    sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 借物出库汇总表.出库金额, 借物出库汇总表.价税合计 AS 销售金额, 借物出库汇总表.备注 FROM 单位表 INNER JOIN 借物出库汇总表 ON 单位表.ID = 借物出库汇总表.单位ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 借物出库汇总表.操作员ID = 操作员.ID WHERE (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.BeActive = 1)";
                    if (iSupplyCompany != 0)
                        sqlComm.CommandText += " AND 单位表.ID=" + iSupplyCompany.ToString();
                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 借物出库汇总表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 7;
                    break;

                case 14:
                    sqlComm.CommandText = "SELECT 库存报损汇总表.ID, 库存报损汇总表.单据编号, 库存报损汇总表.日期, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 库存报损汇总表.报损数量合计, 库存报损汇总表.报损金额合计, 库存报损汇总表.备注 FROM 库存报损汇总表 INNER JOIN 职员表 ON 库存报损汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 库存报损汇总表.操作员ID = 操作员.ID WHERE (库存报损汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (库存报损汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (库存报损汇总表.BeActive = 1)";

                    if (intUserLimit >= 11)
                    {
                        if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
                        if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                            sqlComm.CommandText += " AND 操作员.ID = " + comboBoxCZY.SelectedValue.ToString();
                    }
                    else
                    {
                        sqlComm.CommandText += " AND ( 职员表.ID = " + comboBoxYWY.SelectedValue.ToString() + " OR " + "操作员.ID=" + comboBoxCZY.SelectedValue.ToString() + ")";
                    }
                    if (textBoxDJBH.Text.Trim() != "")
                        sqlComm.CommandText += " AND 库存报损汇总表.单据编号 LIKE N'%" + textBoxDJBH.Text.Trim() + "%'";

                    sqlComm.CommandText += " ORDER BY  日期 DESC";
                    c = 6;
                    break;
                    
            }

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;


            sqlConn.Close();
            dataGridViewDJMX.Focus();

            counttoolStripStatusLabelC(c);
        }

        private void counttoolStripStatusLabelC(int c)
        {
            decimal fTemp;

            fTemp = 0;

            for (int i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                try
                {
                    fTemp += decimal.Parse(dSet.Tables["商品表"].Rows[i][c].ToString());
                }
                catch
                {
                }
            }

            toolStripStatusLabelC.Text = "共有" + dSet.Tables["商品表"].Rows.Count.ToString() + "条单据记录 金额合计"+fTemp.ToString("f2")+"元";


        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1000, "") == 0)
            {
                //return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1100, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                }
            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1200, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                }
            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "单据再现（" + comboBoxDJLB.Text+ "）;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true,intUserLimit);
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDJMX.RowCount < 1)
                return;

            if (dataGridViewDJMX.SelectedRows.Count < 1)
                return;

            string sTemp = "",sTemp1="";
            
            if(e==null)
            {
                sTemp = dataGridViewDJMX.SelectedRows[0].Cells[1].Value.ToString().ToUpper();
                sTemp1 = dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString();
            }
            else
            {
                sTemp=dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value.ToString().ToUpper();
                sTemp1=dataGridViewDJMX.Rows[e.RowIndex].Cells[0].Value.ToString();
            }

            //if(e.RowIndex<0)
            //    return;

            //if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
            //    return;


            switch (sTemp.Substring(0, 2))
            {
                case "CG":
                    // 创建此子窗体的一个新实例。
                    FormCGHT childFormCGHT = new FormCGHT();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormCGHT.MdiParent = this.MdiParent;

                    childFormCGHT.strConn = strConn;
                    childFormCGHT.iDJID = Convert.ToInt32(sTemp1);
                    childFormCGHT.isSaved = true;

                    childFormCGHT.intUserID = intUserID;
                    childFormCGHT.intUserLimit = intUserLimit;
                    childFormCGHT.strUserLimit = strUserLimit;
                    childFormCGHT.strUserName = strUserName;
                    childFormCGHT.Show();
                    break;
                case "XS":
                    // 创建此子窗体的一个新实例。
                    FormXSHT childFormXSHT = new FormXSHT();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSHT.MdiParent = this.MdiParent;

                    childFormXSHT.strConn = strConn;
                    childFormXSHT.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSHT.isSaved = true;

                    childFormXSHT.intUserID = intUserID;
                    childFormXSHT.intUserLimit = intUserLimit;
                    childFormXSHT.strUserLimit = strUserLimit;
                    childFormXSHT.strUserName = strUserName;
                    childFormXSHT.Show();
                    break;
            }

            switch (sTemp.Substring(0, 3))
            {
                case "AKP":
                    // 创建此子窗体的一个新实例。
                    FormGJSPZD childFormGJSPZD = new FormGJSPZD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormGJSPZD.MdiParent = this.MdiParent;

                    childFormGJSPZD.strConn = strConn;
                    childFormGJSPZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormGJSPZD.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormGJSPZD.printToolStripButton.Visible = false;
                        childFormGJSPZD.printPreviewToolStripButton.Visible = false;
                    }

                    childFormGJSPZD.intUserID = intUserID;
                    childFormGJSPZD.intUserLimit = intUserLimit;
                    childFormGJSPZD.strUserLimit = strUserLimit;
                    childFormGJSPZD.strUserName = strUserName;
                    childFormGJSPZD.Show();
                    break;

                case "ADH":
                    // 创建此子窗体的一个新实例。
                    FormJHRKYHD childFormJHRKYHD = new FormJHRKYHD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormJHRKYHD.MdiParent = this.MdiParent;

                    childFormJHRKYHD.strConn = strConn;
                    childFormJHRKYHD.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHRKYHD.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormJHRKYHD.printToolStripButton.Visible = false;
                        childFormJHRKYHD.printPreviewToolStripButton.Visible = false;
                    }

                    childFormJHRKYHD.intUserID = intUserID;
                    childFormJHRKYHD.intUserLimit = intUserLimit;
                    childFormJHRKYHD.strUserLimit = strUserLimit;
                    childFormJHRKYHD.strUserName = strUserName;
                    childFormJHRKYHD.Show();
                    break;

                case "ATH":
                    // 创建此子窗体的一个新实例。
                    FormJHTCZD childFormJHTCZD = new FormJHTCZD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormJHTCZD.MdiParent = this.MdiParent;

                    childFormJHTCZD.strConn = strConn;
                    childFormJHTCZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHTCZD.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormJHTCZD.printToolStripButton.Visible = false;
                        childFormJHTCZD.printPreviewToolStripButton.Visible = false;
                    }

                    childFormJHTCZD.intUserID = intUserID;
                    childFormJHTCZD.intUserLimit = intUserLimit;
                    childFormJHTCZD.strUserLimit = strUserLimit;
                    childFormJHTCZD.strUserName = strUserName;
                    childFormJHTCZD.Show();
                    break;

                case "ATB":
                    // 创建此子窗体的一个新实例。
                    FormJHTBJDJ childFormJHTBJDJ = new FormJHTBJDJ();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormJHTBJDJ.MdiParent = this.MdiParent;

                    childFormJHTBJDJ.strConn = strConn;
                    childFormJHTBJDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHTBJDJ.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormJHTBJDJ.printToolStripButton.Visible = false;
                        childFormJHTBJDJ.printPreviewToolStripButton.Visible = false;
                    }
                    childFormJHTBJDJ.intUserID = intUserID;
                    childFormJHTBJDJ.intUserLimit = intUserLimit;
                    childFormJHTBJDJ.strUserLimit = strUserLimit;
                    childFormJHTBJDJ.strUserName = strUserName;
                    childFormJHTBJDJ.Show();
                    break;

                case "AYF":
                    // 创建此子窗体的一个新实例。
                    FormYFZKJS childFormYFZKJS = new FormYFZKJS();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormYFZKJS.MdiParent = this.MdiParent;

                    childFormYFZKJS.strConn = strConn;
                    childFormYFZKJS.iDJID = Convert.ToInt32(sTemp1);
                    childFormYFZKJS.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormYFZKJS.printToolStripButton.Visible = false;
                        childFormYFZKJS.printPreviewToolStripButton.Visible = false;
                    }
                    childFormYFZKJS.intUserID = intUserID;
                    childFormYFZKJS.intUserLimit = intUserLimit;
                    childFormYFZKJS.strUserLimit = strUserLimit;
                    childFormYFZKJS.strUserName = strUserName;
                    childFormYFZKJS.Show();
                    break;

                case "BKP":
                    // 创建此子窗体的一个新实例。
                    FormXSCKZD childFormXSCKZD = new FormXSCKZD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSCKZD.MdiParent = this.MdiParent;

                    childFormXSCKZD.strConn = strConn;
                    childFormXSCKZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSCKZD.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormXSCKZD.printToolStripButton.Visible = false;
                        childFormXSCKZD.printPreviewToolStripButton.Visible = false;
                    }
                    childFormXSCKZD.intUserID = intUserID;
                    childFormXSCKZD.intUserLimit = intUserLimit;
                    childFormXSCKZD.strUserLimit = strUserLimit;
                    childFormXSCKZD.strUserName = strUserName;
                    childFormXSCKZD.Show();
                    break;

                case "BCK":
                    // 创建此子窗体的一个新实例。
                    FormXSCKJD childFormXSCKJD = new FormXSCKJD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSCKJD.MdiParent = this.MdiParent;

                    childFormXSCKJD.strConn = strConn;
                    childFormXSCKJD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSCKJD.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormXSCKJD.printToolStripButton.Visible = false;
                        childFormXSCKJD.printPreviewToolStripButton.Visible = false;
                    }
                    childFormXSCKJD.intUserID = intUserID;
                    childFormXSCKJD.intUserLimit = intUserLimit;
                    childFormXSCKJD.strUserLimit = strUserLimit;
                    childFormXSCKJD.strUserName = strUserName;
                    childFormXSCKJD.Show();
                    break;

                case "BTH":
                    // 创建此子窗体的一个新实例。
                    FormXSTHZD childFormXSTHZD = new FormXSTHZD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSTHZD.MdiParent = this.MdiParent;

                    childFormXSTHZD.strConn = strConn;
                    childFormXSTHZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSTHZD.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormXSTHZD.printToolStripButton.Visible = false;
                        childFormXSTHZD.printPreviewToolStripButton.Visible = false;
                    }
                    childFormXSTHZD.intUserID = intUserID;
                    childFormXSTHZD.intUserLimit = intUserLimit;
                    childFormXSTHZD.strUserLimit = strUserLimit;
                    childFormXSTHZD.strUserName = strUserName;
                    childFormXSTHZD.Show();
                    break;

                case "BTB":
                    // 创建此子窗体的一个新实例。
                    FormXSTBJDJ childFormXSTBJDJ = new FormXSTBJDJ();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSTBJDJ.MdiParent = this.MdiParent;

                    childFormXSTBJDJ.strConn = strConn;
                    childFormXSTBJDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSTBJDJ.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormXSTBJDJ.printToolStripButton.Visible = false;
                        childFormXSTBJDJ.printPreviewToolStripButton.Visible = false;
                    }
                    childFormXSTBJDJ.intUserID = intUserID;
                    childFormXSTBJDJ.intUserLimit = intUserLimit;
                    childFormXSTBJDJ.strUserLimit = strUserLimit;
                    childFormXSTBJDJ.strUserName = strUserName;
                    childFormXSTBJDJ.Show();
                    break;

                case "BYS":
                    // 创建此子窗体的一个新实例。
                    FormYSZKJS childFormYSZKJS = new FormYSZKJS();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormYSZKJS.MdiParent = this.MdiParent;

                    childFormYSZKJS.strConn = strConn;
                    childFormYSZKJS.iDJID = Convert.ToInt32(sTemp1);
                    childFormYSZKJS.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormYSZKJS.printToolStripButton.Visible = false;
                        childFormYSZKJS.printPreviewToolStripButton.Visible = false;
                    }
                    childFormYSZKJS.intUserID = intUserID;
                    childFormYSZKJS.intUserLimit = intUserLimit;
                    childFormYSZKJS.strUserLimit = strUserLimit;
                    childFormYSZKJS.strUserName = strUserName;
                    childFormYSZKJS.Show();
                    break;

                case "CPD":
                    // 创建此子窗体的一个新实例。
                    FormKCSPPD2 childFormKCSPPD2 = new FormKCSPPD2();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormKCSPPD2.MdiParent = this.MdiParent;

                    childFormKCSPPD2.strConn = strConn;
                    childFormKCSPPD2.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCSPPD2.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormKCSPPD2.printToolStripButton.Visible = false;
                        childFormKCSPPD2.printPreviewToolStripButton.Visible = false;
                    }
                    childFormKCSPPD2.intUserID = intUserID;
                    childFormKCSPPD2.intUserLimit = intUserLimit;
                    childFormKCSPPD2.strUserLimit = strUserLimit;
                    childFormKCSPPD2.strUserName = strUserName;
                    childFormKCSPPD2.Show();
                    break;

                case "CBS":
                    // 创建此子窗体的一个新实例。
                    FormKCSPBSCL childFormKCSPBSCL = new FormKCSPBSCL();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormKCSPBSCL.MdiParent = this.MdiParent;

                    childFormKCSPBSCL.strConn = strConn;
                    childFormKCSPBSCL.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCSPBSCL.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormKCSPBSCL.printToolStripButton.Visible = false;
                        childFormKCSPBSCL.printPreviewToolStripButton.Visible = false;
                    }
                    childFormKCSPBSCL.intUserID = intUserID;
                    childFormKCSPBSCL.intUserLimit = intUserLimit;
                    childFormKCSPBSCL.strUserLimit = strUserLimit;
                    childFormKCSPBSCL.strUserName = strUserName;
                    childFormKCSPBSCL.Show();
                    break;

                case "CCK":
                    // 创建此子窗体的一个新实例。
                    FormKCJWCKDJ childFormKCJWCKDJ = new FormKCJWCKDJ();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormKCJWCKDJ.MdiParent = this.MdiParent;

                    childFormKCJWCKDJ.strConn = strConn;
                    childFormKCJWCKDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCJWCKDJ.isSaved = true;
                    if (intUserLimit < iConstLimit)
                    {
                        childFormKCJWCKDJ.printToolStripButton.Visible = false;
                        childFormKCJWCKDJ.printPreviewToolStripButton.Visible = false;
                    }
                    childFormKCJWCKDJ.intUserID = intUserID;
                    childFormKCJWCKDJ.intUserLimit = intUserLimit;
                    childFormKCJWCKDJ.strUserLimit = strUserLimit;
                    childFormKCJWCKDJ.strUserName = strUserName;
                    childFormKCJWCKDJ.Show();
                    break;
            }


        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.Add)
            {
                //System.Windows.Forms.SendKeys.Send("{tab}");
                toolStripButtonGD_Click(null, null);//
                return true;
            }

            if (keyData == Keys.Enter && dataGridViewDJMX.Focused)
            {
                //System.Windows.Forms.SendKeys.Send("{tab}");
                dataGridViewDJMX_CellDoubleClick(null, null);//
                return true;
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            iSupplyCompany = 0;
            textBoxDWMC.Text = "";
            textBoxDWBH.Text = "";
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "单据再现（" + comboBoxDJLB.Text + "）;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}