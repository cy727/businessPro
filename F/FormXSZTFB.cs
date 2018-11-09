using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXSZTFB : Form
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

        private int intDays = 0;

        private int intCommID = 0;
        private int iCompanyID = 0;

        private string sDT = "";
        private ClassGetInformation cGetInformation;

        private int[] iCount = { 0, 0, 0 };

        public FormXSZTFB()
        {
            InitializeComponent();
        }

        private void FormXSZTFB_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            //得到开始时间
            sqlConn.Open();
            //sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
            sqlComm.CommandText = "SELECT GETDATE()";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();

            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            DataRow drTemp = dSet.Tables["职员表"].NewRow();
            drTemp[0] = 0;
            drTemp[1] = "全部";
            dSet.Tables["职员表"].Rows.Add(drTemp);


            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.Text = strUserName;
            comboBoxYWY.SelectedValue = 0;
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            string strA = "", strB = "", strC = "", strD = "", strE="";

            strA = "SELECT 销售商品制单表.单位ID, COUNT(DISTINCT 销售商品制单表.ID) AS 销售批次, SUM(销售商品制单明细表.实计金额) AS 销售金额 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strA+=" GROUP BY 销售商品制单表.单位ID";

            strB = "SELECT 销售退出汇总表.单位ID, COUNT(DISTINCT 销售退出汇总表.ID) AS 退出批次, SUM(销售退出明细表.实计金额) AS 退出金额 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退出汇总表.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB += " GROUP BY 销售退出汇总表.单位ID";
/*
            strC = "SELECT 结算收款汇总表.单位ID, COUNT(*) AS 回款批次, SUM(结算收款明细表.付款金额) AS 回款金额 FROM 结算收款汇总表 INNER JOIN 结算收款明细表 ON 结算收款汇总表.ID = 结算收款明细表.单据ID INNER JOIN 结算收款勾兑表 ON 结算收款明细表.ID = 结算收款勾兑表.付款ID INNER JOIN 销售商品制单表 ON 结算收款勾兑表.单据编号 = 销售商品制单表.单据编号 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (结算收款汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (结算收款汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (结算收款汇总表.BeActive=1) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (结算收款汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (结算收款汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY 结算收款汇总表.单位ID";
*/
            strC = "SELECT 单位ID, COUNT(DISTINCT 单据编号) AS 回款批次, SUM(已付款金额) AS 回款金额 FROM 收款明细视图 WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (已付款金额 <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC += " AND (收款明细视图.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (收款明细视图.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (收款明细视图.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY 单位ID";

            /*
            strD = "SELECT 销售商品制单表.单位ID, MIN(销售商品制单表.日期) AS 日期 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive=1) AND (销售商品制单表.未付款金额 <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strD += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strD += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strD += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strD += " GROUP BY 销售商品制单表.单位ID";
            */

            strD = "SELECT 单位ID, MIN(日期) AS 日期  FROM 收款明细视图 WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (未付款金额 <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strD += " AND (收款明细视图.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strD += " AND (收款明细视图.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strD += " AND (收款明细视图.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strD += " GROUP BY 单位ID";

            strE = "SELECT 销售退补差价汇总表.单位ID, COUNT(DISTINCT 销售退补差价汇总表.ID) AS 补价批次, SUM(销售退补差价明细表.金额) AS 补价金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退补差价汇总表.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strE += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strE += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strE += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strE += " GROUP BY 销售退补差价汇总表.单位ID";

            sqlConn.Open();

            sqlComm.CommandText = "SELECT 单位表.单位编号, 单位表.单位名称, 销售表.销售金额, 销售表.销售批次, 退货表.退出金额, 退货表.退出批次, 回款表.回款金额, 回款表.回款批次 , 0 AS 应收余额, 0 AS 最长欠款期, 补价表.补价批次,补价表.补价金额, 付款表.日期  FROM 单位表 LEFT OUTER JOIN (" + strC + ") 回款表 ON 单位表.ID = 回款表.单位ID LEFT OUTER JOIN (" + strB + ") 退货表 ON 单位表.ID = 退货表.单位ID LEFT OUTER JOIN (" + strA + ") 销售表 ON 单位表.ID = 销售表.单位ID  LEFT OUTER JOIN (" + strD + ") 付款表 ON 单位表.ID = 付款表.单位ID LEFT OUTER JOIN (" + strE + ") 补价表 ON 单位表.ID = 补价表.单位ID WHERE (单位表.是否销售 = 1) AND (单位表.BeActive=1)";

            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                sqlComm.CommandText += " AND (单位表.ID=" + iCompanyID.ToString() + ")";
            }


            if (dSet.Tables.Contains("单位表")) dSet.Tables.Remove("单位表");
            sqlDA.Fill(dSet, "单位表");


            strA = "SELECT 销售商品制单表.业务员ID, COUNT(DISTINCT 销售商品制单表.ID) AS 销售批次, SUM(销售商品制单明细表.实计金额) AS 销售金额 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strA += " GROUP BY 销售商品制单表.业务员ID";

            strB = "SELECT 销售退出汇总表.业务员ID, COUNT(DISTINCT 销售退出汇总表.ID) AS 退出批次, SUM(销售退出明细表.实计金额) AS 退出金额 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退出汇总表.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB += " GROUP BY 销售退出汇总表.业务员ID";
/*
            strC = "SELECT 结算收款汇总表.业务员ID, COUNT(*) AS 回款批次, SUM(结算收款明细表.付款金额) AS 回款金额 FROM 结算收款汇总表 INNER JOIN 结算收款明细表 ON 结算收款汇总表.ID = 结算收款明细表.单据ID INNER JOIN 结算收款勾兑表 ON 结算收款明细表.ID = 结算收款勾兑表.付款ID INNER JOIN 销售商品制单表 ON 结算收款勾兑表.单据编号 = 销售商品制单表.单据编号 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (结算收款汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (结算收款汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (结算收款汇总表.BeActive=1) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (结算收款汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (结算收款汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY 结算收款汇总表.业务员ID";
 */
            strC = "SELECT 业务员ID, COUNT(DISTINCT 单据编号) AS 回款批次, SUM(已付款金额) AS 回款金额 FROM 收款明细视图 WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (已付款金额 <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC += " AND (收款明细视图.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (收款明细视图.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (收款明细视图.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY 业务员ID";
 
            /*
            strD = "SELECT 销售商品制单表.业务员ID, MIN(销售商品制单表.日期) AS 日期 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive=1) AND (销售商品制单表.未付款金额 <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strD += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strD += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strD += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strD += " GROUP BY 销售商品制单表.业务员ID";
            */
            strD = "SELECT 业务员ID, MIN(日期) AS 日期  FROM 收款明细视图 WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (未付款金额 <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strD += " AND (收款明细视图.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strD += " AND (收款明细视图.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strD += " AND (收款明细视图.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strD += " GROUP BY 业务员ID";

            strE = "SELECT 销售退补差价汇总表.业务员ID, COUNT(DISTINCT 销售退补差价汇总表.ID) AS 补价批次, SUM(销售退补差价明细表.金额) AS 补价金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退补差价汇总表.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strE += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strE += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strE += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strE += " GROUP BY 销售退补差价汇总表.业务员ID";


            sqlComm.CommandText = "SELECT 职员表.职员编号, 职员表.职员姓名, 销出表.销售金额, 销出表.销售批次,退款表.退出金额, 退款表.退出批次, 回款表.回款金额, 回款表.回款批次, 0 AS 应收余额, 0 AS 最长欠款期, 补价表.补价批次,补价表.补价金额, 付款表.日期 FROM 职员表 LEFT OUTER JOIN (" + strC + ") 回款表 ON 职员表.ID = 回款表.业务员ID LEFT OUTER JOIN (" + strB + ") 退款表 ON 职员表.ID = 退款表.业务员ID LEFT OUTER JOIN (" + strA + ") 销出表 ON 职员表.ID = 销出表.业务员ID  LEFT OUTER JOIN (" + strD + ") 付款表 ON 职员表.ID = 付款表.业务员ID LEFT OUTER JOIN (" + strE + ") 补价表 ON 职员表.ID = 补价表.业务员ID  WHERE (职员表.BeActive=1)";
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (职员表.ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            sqlComm.CommandText += " ORDER BY 职员表.职员编号";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");


            strA = "SELECT 销售商品制单明细表.商品ID, COUNT(DISTINCT 销售商品制单表.ID) AS 销售批次, SUM(销售商品制单明细表.实计金额) AS 销售金额 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strA += " GROUP BY 销售商品制单明细表.商品ID";

            strB = "SELECT 销售退出明细表.商品ID, COUNT(DISTINCT 销售退出汇总表.ID) AS 退出批次, SUM(销售退出明细表.实计金额) AS 退出金额 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退出汇总表.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB += " GROUP BY 销售退出明细表.商品ID";
/*
            strC = "SELECT 销售商品制单明细表.商品ID, COUNT(*) AS 回款批次, SUM(结算收款明细表.付款金额) AS 回款金额 FROM 结算收款汇总表 INNER JOIN 结算收款明细表 ON 结算收款汇总表.ID = 结算收款明细表.单据ID INNER JOIN 结算收款勾兑表 ON 结算收款明细表.ID = 结算收款勾兑表.付款ID INNER JOIN 销售商品制单表 ON 结算收款勾兑表.单据编号 = 销售商品制单表.单据编号 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (结算收款汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (结算收款汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (结算收款汇总表.BeActive=1) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (结算收款汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (结算收款汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY 销售商品制单明细表.商品ID";
 * 
 */

            strC = "SELECT 商品ID, COUNT(DISTINCT 单据编号) AS 回款批次, SUM(已付款金额) AS 回款金额 FROM 收款明细视图 WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (已付款金额 <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC += " AND (收款明细视图.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (收款明细视图.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (收款明细视图.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY 商品ID";

            /*
            strD = "SELECT 销售商品制单明细表.商品ID, MIN(销售商品制单表.日期) AS 日期 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive=1) AND (销售商品制单表.未付款金额 <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strD += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strD += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strD += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strD += " GROUP BY 销售商品制单明细表.商品ID";
             */

            strD = "SELECT 商品ID, MIN(日期) AS 日期  FROM 收款明细视图 WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (未付款金额 <> 0) ";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strD += " AND (收款明细视图.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strD += " AND (收款明细视图.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strD += " AND (收款明细视图.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strD += " GROUP BY 商品ID";

            strE = "SELECT 销售退补差价明细表.商品ID, COUNT(DISTINCT 销售退补差价汇总表.ID) AS 补价批次, SUM(销售退补差价明细表.金额) AS 补价金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退补差价汇总表.BeActive=1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strE += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strE += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strE += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strE += " GROUP BY 销售退补差价明细表.商品ID";
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称,商品表.商品规格,销售表.销售金额, 销售表.销售批次, 退货表.退出金额, 退货表.退出批次, 回款表.回款金额, 回款表.回款批次, 0 AS 应收余额, 0 AS 最长欠款期, 补价表.补价批次,补价表.补价金额, 付款表.日期 FROM (" + strA + ") 销售表 RIGHT OUTER JOIN 商品表 ON 销售表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strB + ") 退货表 ON 商品表.ID = 退货表.商品ID LEFT OUTER JOIN (" + strC + ") 回款表 ON 商品表.ID = 回款表.商品ID LEFT OUTER JOIN (" + strD + ") 付款表 ON 商品表.ID = 付款表.商品ID LEFT OUTER JOIN (" + strE + ") 补价表 ON 商品表.ID = 补价表.商品ID  WHERE (商品表.beactive = 1) AND (商品表.组装商品 = 0)";
                        
            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");


            sqlConn.Close();

            adjustDataView1();
            dataGridViewDJMX1.DataSource = dSet.Tables["单位表"];
            dataGridViewDJMX1.Columns[12].Visible = false;
            dataGridViewDJMX1.Columns[2].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX1.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX1.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX1.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX1.Columns[6].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX1.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX1.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX1.Columns[9].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX1.Columns[10].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX1.Columns[11].DefaultCellStyle.Format = "f2";

            //dataGridViewDJMX1.Columns[11].Visible = false;

            dataGridViewDJMX2.DataSource = dSet.Tables["职员表"];
            dataGridViewDJMX2.Columns[12].Visible = false;
            dataGridViewDJMX2.Columns[2].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX2.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX2.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX2.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX2.Columns[6].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX2.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX2.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX2.Columns[9].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX2.Columns[10].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX2.Columns[11].DefaultCellStyle.Format = "f2";

            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[12].DefaultCellStyle.Format = "f2";

            tabControl1_SelectedIndexChanged(null,null);

        }

        private void adjustDataView1()
        {
            int i,j;
            TimeSpan ts;
            decimal[] dSUM = { 0,0,0,0,0,0,0,0,0,0};

        //单位表
            iCount[0]=0;
            for(i=dSet.Tables["单位表"].Rows.Count-1;i>=0;i--)
            {
                if (dSet.Tables["单位表"].Rows[i][2].ToString() == "" && dSet.Tables["单位表"].Rows[i][3].ToString() == "" && dSet.Tables["单位表"].Rows[i][4].ToString() == "" && dSet.Tables["单位表"].Rows[i][5].ToString() == "" && dSet.Tables["单位表"].Rows[i][6].ToString() == "" && dSet.Tables["单位表"].Rows[i][7].ToString() == "" && dSet.Tables["单位表"].Rows[i][10].ToString() == "" && dSet.Tables["单位表"].Rows[i][11].ToString() == "" && dSet.Tables["单位表"].Rows[i][12].ToString() == "")
                 {
                     dSet.Tables["单位表"].Rows[i].Delete();
                 }
                 else
                     iCount[0]++;

            }
            dSet.Tables["单位表"].AcceptChanges();

            for (i = 0; i < dSUM.Length; i++)
            {
                dSUM[i] = 0;
            }
            for (i = 0; i < dSet.Tables["单位表"].Rows.Count; i++)
            {

                for (j = 2; j <= 7; j++)
                {
                    if (dSet.Tables["单位表"].Rows[i][j].ToString() == "")
                        dSet.Tables["单位表"].Rows[i][j] = 0;

                    dSUM[j - 2] += decimal.Parse(dSet.Tables["单位表"].Rows[i][j].ToString());
                }

                dSet.Tables["单位表"].Rows[i][8] = decimal.Parse(dSet.Tables["单位表"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][4].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][6].ToString());

                if (dSet.Tables["单位表"].Rows[i][12].ToString() == "")
                {
                    dSet.Tables["单位表"].Rows[i][9] = 0;
                }
                else
                {

                    ts = dateTimePickerE.Value-Convert.ToDateTime(dSet.Tables["单位表"].Rows[i][12].ToString());
                    if(ts.Days<0)
                        dSet.Tables["单位表"].Rows[i][9] = 0;
                    else
                        dSet.Tables["单位表"].Rows[i][9] = ts.Days;
                }

                dSUM[7] = Math.Max(decimal.Parse(dSet.Tables["单位表"].Rows[i][9].ToString()), dSUM[7]);
                for (j = 10; j <= 11; j++)
                {
                    if (dSet.Tables["单位表"].Rows[i][j].ToString() == "")
                        dSet.Tables["单位表"].Rows[i][j] = 0;

                    dSUM[j - 2] += decimal.Parse(dSet.Tables["单位表"].Rows[i][j].ToString());
                }


            }
            DataRow drT1 = dSet.Tables["单位表"].NewRow();
            drT1[1] = "合计";
            for (j = 2; j <= 7; j++)
            {
                drT1[j]=dSUM[j - 2];
            }
            drT1[8] = decimal.Parse(drT1[2].ToString()) - decimal.Parse(drT1[4].ToString()) - decimal.Parse(drT1[6].ToString());
            drT1[9] = dSUM[7];
            for (j = 10; j <= 11; j++)
            {
                drT1[j] = dSUM[j - 2];
            }
            dSet.Tables["单位表"].Rows.Add(drT1);

            //职员表
            iCount[1] = 0;
            for (i = dSet.Tables["职员表"].Rows.Count - 1; i >= 0; i--)
            {
                if (dSet.Tables["职员表"].Rows[i][2].ToString() == "" && dSet.Tables["职员表"].Rows[i][3].ToString() == "" && dSet.Tables["职员表"].Rows[i][4].ToString() == "" && dSet.Tables["职员表"].Rows[i][5].ToString() == "" && dSet.Tables["职员表"].Rows[i][6].ToString() == "" && dSet.Tables["职员表"].Rows[i][7].ToString() == "" && dSet.Tables["职员表"].Rows[i][10].ToString() == "" && dSet.Tables["职员表"].Rows[i][11].ToString() == "" && dSet.Tables["职员表"].Rows[i][12].ToString() == "")
                {
                    dSet.Tables["职员表"].Rows[i].Delete();
                }
                else
                    iCount[1]++;

            }
            dSet.Tables["职员表"].AcceptChanges();

            for (i = 0; i < dSUM.Length; i++)
            {
                dSUM[i] = 0;
            }
            for (i = 0; i < dSet.Tables["职员表"].Rows.Count; i++)
            {

                for (j = 2; j <= 7; j++)
                {
                    if (dSet.Tables["职员表"].Rows[i][j].ToString() == "")
                        dSet.Tables["职员表"].Rows[i][j] = 0;

                    dSUM[j - 2] += decimal.Parse(dSet.Tables["职员表"].Rows[i][j].ToString());
                }

                dSet.Tables["职员表"].Rows[i][8] = decimal.Parse(dSet.Tables["职员表"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][4].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][6].ToString());

                if (dSet.Tables["职员表"].Rows[i][12].ToString() == "")
                {
                    dSet.Tables["职员表"].Rows[i][9] = 0;
                }
                else
                {

                    ts = dateTimePickerE.Value - Convert.ToDateTime(dSet.Tables["职员表"].Rows[i][12].ToString());
                    if (ts.Days < 0)
                        dSet.Tables["职员表"].Rows[i][9] = 0;
                    else
                        dSet.Tables["职员表"].Rows[i][9] = ts.Days;
                }

                dSUM[7] = Math.Max(decimal.Parse(dSet.Tables["职员表"].Rows[i][9].ToString()), dSUM[7]);
                for (j = 10; j <= 11; j++)
                {
                    if (dSet.Tables["职员表"].Rows[i][j].ToString() == "")
                        dSet.Tables["职员表"].Rows[i][j] = 0;

                    dSUM[j - 2] += decimal.Parse(dSet.Tables["职员表"].Rows[i][j].ToString());
                }
            }
            DataRow drT2 = dSet.Tables["职员表"].NewRow();
            drT2[1] = "合计";
            for (j = 2; j <= 7; j++)
            {
                drT2[j] = dSUM[j - 2];
            }
            for (j = 10; j <= 11; j++)
            {
                drT2[j] = dSUM[j - 2];
            }
            drT2[8] = decimal.Parse(drT2[2].ToString()) - decimal.Parse(drT2[4].ToString()) - decimal.Parse(drT2[6].ToString());
            drT2[9] = dSUM[7];
            dSet.Tables["职员表"].Rows.Add(drT2);


            //商品表
            iCount[2] = 0;
            for (i = dSet.Tables["商品表"].Rows.Count - 1; i >= 0; i--)
            {
                if (dSet.Tables["商品表"].Rows[i][8].ToString() == "" && dSet.Tables["商品表"].Rows[i][3].ToString() == "" && dSet.Tables["商品表"].Rows[i][4].ToString() == "" && dSet.Tables["商品表"].Rows[i][5].ToString() == "" && dSet.Tables["商品表"].Rows[i][6].ToString() == "" && dSet.Tables["商品表"].Rows[i][7].ToString() == "" && dSet.Tables["商品表"].Rows[i][11].ToString() == "" && dSet.Tables["商品表"].Rows[i][12].ToString() == "" && dSet.Tables["商品表"].Rows[i][13].ToString() == "")
                {
                    dSet.Tables["商品表"].Rows[i].Delete();
                }
                else
                    iCount[2]++;

            }
            dSet.Tables["商品表"].AcceptChanges();

            for (i = 0; i < dSUM.Length; i++)
            {
                dSUM[i] = 0;
            }
            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {

                for (j = 3; j <= 8; j++)
                {
                    if (dSet.Tables["商品表"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品表"].Rows[i][j] = 0;

                    dSUM[j - 3] += decimal.Parse(dSet.Tables["商品表"].Rows[i][j].ToString());
                }

                dSet.Tables["商品表"].Rows[i][9] = decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][5].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][7].ToString());

                if (dSet.Tables["商品表"].Rows[i][13].ToString() == "")
                {
                    dSet.Tables["商品表"].Rows[i][10] = 0;
                }
                else
                {

                    ts = dateTimePickerE.Value - Convert.ToDateTime(dSet.Tables["商品表"].Rows[i][13].ToString());
                    if (ts.Days < 0)
                        dSet.Tables["商品表"].Rows[i][10] = 0;
                    else
                        dSet.Tables["商品表"].Rows[i][10] = ts.Days;
                }

                dSUM[7] = Math.Max(decimal.Parse(dSet.Tables["商品表"].Rows[i][10].ToString()), dSUM[7]);
                for (j = 11; j <= 12; j++)
                {
                    if (dSet.Tables["商品表"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品表"].Rows[i][j] = 0;

                    dSUM[j - 3] += decimal.Parse(dSet.Tables["商品表"].Rows[i][j].ToString());
                }
            }
            DataRow drT3 = dSet.Tables["商品表"].NewRow();
            drT3[2] = "合计";
            for (j = 3; j <= 8; j++)
            {
                drT3[j] = dSUM[j - 3];
            }
            drT3[9] = decimal.Parse(drT3[3].ToString()) - decimal.Parse(drT3[5].ToString()) - decimal.Parse(drT3[7].ToString());
            drT3[10] = dSUM[7];
            for (j = 11; j <= 12; j++)
            {
                drT3[j] = dSUM[j - 3];
            }
            dSet.Tables["商品表"].Rows.Add(drT3);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "销售状态分布（销往单位销售状态）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDJMX1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "销售状态分布（业务员销售状态）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDJMX2, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "销售状态分布（经营商品销售状态）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
                    break;

            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "销售状态分布（销往单位销售状态）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDJMX1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "销售状态分布（业务员销售状态）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDJMX2, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "销售状态分布（经营商品销售状态）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
                    break;

            }
        }

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPBH.Text = cGetInformation.strCommCode;
                textBoxSPMC.Text = cGetInformation.strCommName;
            }
        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                }

            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                }

            }
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                iCompanyID = cGetInformation.iCompanyNumber;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1100, textBoxDWBH.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    iCompanyID = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }

            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1300, textBoxDWMC.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    iCompanyID = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }

            }
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabelMXJLS.Text = iCount[tabControl1.SelectedIndex].ToString();
        }

    }
}