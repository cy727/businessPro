using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKHGXFX : Form
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

        private decimal[] cTemp = new decimal[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0 };
        private decimal[] cTemp1 = new decimal[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0 };

        public FormKHGXFX()
        {
            InitializeComponent();
        }

        private void FormKHGXFX_Load(object sender, EventArgs e)
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
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");

            }
            sqldr.Close();
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            tabControl1.SelectedIndex = 2;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(2, "") == 0)
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
                if (cGetInformation.getCompanyInformation(20, textBoxDWBH.Text.Trim()) == 0)
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
                if (cGetInformation.getCompanyInformation(22, textBoxDWMC.Text.Trim()) == 0)
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

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            //销售制单
            sqlComm.CommandText = "SELECT 销售商品制单表.ID, 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 销售商品制单表.物流名称, 销售商品制单表.单号, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 销售合同表.合同编号, 销售商品制单表.价税合计, 销售商品制单表.备注 FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售商品制单表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 销售商品制单表.操作员ID = 操作员.ID LEFT OUTER JOIN 销售合同表 ON 销售商品制单表.合同ID = 销售合同表.ID WHERE (销售商品制单表.BeActive = 1) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT 销售商品制单表.ID, 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号,单位表.单位名称, 销售商品制单表.价税合计, 销售商品制单表.未付款金额, 销售商品制单表.备注 FROM 单位表 INNER JOIN 销售商品制单表 ON 单位表.ID = 销售商品制单表.单位ID WHERE (销售商品制单表.未付款金额 > 0) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");

            //销售明细
            sqlComm.CommandText = "SELECT 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.数量, 销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.实计金额, 销售商品制单明细表.扣率, 销售商品制单明细表.毛利, 销售商品制单明细表.赠品 FROM 单位表 INNER JOIN 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID ON 单位表.ID = 销售商品制单表.单位ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表2")) dSet.Tables.Remove("商品表2");
            sqlDA.Fill(dSet, "商品表2");

            //销售退回汇总
            sqlComm.CommandText = "SELECT 销售退出汇总表.ID, 销售退出汇总表.单据编号, 销售退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 销售退出汇总表.价税合计, 销售退出汇总表.未付款金额, 销售退出汇总表.备注 FROM 销售退出汇总表 INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退出汇总表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表3")) dSet.Tables.Remove("商品表3");
            sqlDA.Fill(dSet, "商品表3");


            //销售退回明细
            sqlComm.CommandText = "SELECT 销售退出汇总表.单据编号, 销售退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 销售退出明细表.数量, 销售退出明细表.单价, 销售退出明细表.实计金额 FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 销售退出明细表.库房ID = 库房表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退出汇总表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表4")) dSet.Tables.Remove("商品表4");
            sqlDA.Fill(dSet, "商品表4");

            //购进汇总
            sqlComm.CommandText = "SELECT 购进商品制单表.ID, 购进商品制单表.单据编号, 购进商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 购进商品制单表.价税合计, 购进商品制单表.付款方式, 采购合同表.合同编号, 购进商品制单表.备注 FROM 购进商品制单表 INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 购进商品制单表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 购进商品制单表.操作员ID = 操作员.ID LEFT OUTER JOIN 采购合同表 ON 购进商品制单表.合同ID = 采购合同表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表5")) dSet.Tables.Remove("商品表5");
            sqlDA.Fill(dSet, "商品表5");

            //购进明细
            sqlComm.CommandText = "SELECT 进货入库汇总表.单据编号, 购进商品制单表.单据编号 AS 原单据编号, 进货入库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 商品表.商品编号, 商品表.商品名称, 进货入库明细表.数量, 进货入库明细表.单价, 进货入库明细表.金额, 进货入库明细表.扣率, 进货入库明细表.实计金额, 进货入库明细表.赠品 FROM 进货入库明细表 INNER JOIN 进货入库汇总表 ON 进货入库明细表.单据ID = 进货入库汇总表.ID INNER JOIN 单位表 ON 进货入库汇总表.单位ID = 单位表.ID INNER JOIN 库房表 ON 进货入库明细表.库房ID = 库房表.ID INNER JOIN 商品表 ON 进货入库明细表.商品ID = 商品表.ID LEFT OUTER JOIN 购进商品制单表 ON 进货入库汇总表.购进ID = 购进商品制单表.ID WHERE (进货入库汇总表.BeActive = 1) AND (进货入库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";
            if (dSet.Tables.Contains("商品表6")) dSet.Tables.Remove("商品表6");
            sqlDA.Fill(dSet, "商品表6");

            //购进退回汇总
            sqlComm.CommandText = "SELECT 进货退出汇总表.ID, 进货退出汇总表.单据编号, 进货退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 进货退出汇总表.价税合计, 进货退出汇总表.未付款金额, 进货退出汇总表.备注 FROM 进货退出汇总表 INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID WHERE (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (进货退出汇总表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表7")) dSet.Tables.Remove("商品表7");
            sqlDA.Fill(dSet, "商品表7");

            //购进退回明细
            sqlComm.CommandText = "SELECT 进货退出汇总表.单据编号, 进货退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 进货退出明细表.数量, 进货退出明细表.单价, 进货退出明细表.实计金额 FROM 进货退出明细表 INNER JOIN 进货退出汇总表 ON 进货退出明细表.单据ID = 进货退出汇总表.ID INNER JOIN 商品表 ON 进货退出明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 进货退出明细表.库房ID = 库房表.ID INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID WHERE (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (进货退出汇总表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表8")) dSet.Tables.Remove("商品表8");
            sqlDA.Fill(dSet, "商品表8");

            //借物出库汇总
            sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.价税合计, 借物出库汇总表.备注 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表9")) dSet.Tables.Remove("商品表9");
            sqlDA.Fill(dSet, "商品表9");

            //借物出库明细
            sqlComm.CommandText = "SELECT 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 借物出库明细表.数量, 借物出库明细表.单价, 借物出库明细表.金额 FROM 借物出库汇总表 INNER JOIN  单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 借物出库明细表 ON 借物出库汇总表.ID = 借物出库明细表.表单ID INNER JOIN 商品表 ON 借物出库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表10")) dSet.Tables.Remove("商品表10");
            sqlDA.Fill(dSet, "商品表10");

            //销售结算汇总
            sqlComm.CommandText = "SELECT 销售商品制单表.ID, 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号,单位表.单位名称, 销售商品制单表.价税合计, 销售商品制单表.未付款金额, 销售商品制单表.备注 FROM 单位表 INNER JOIN 销售商品制单表 ON 单位表.ID = 销售商品制单表.单位ID WHERE (销售商品制单表.已付款金额 > 0) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表11")) dSet.Tables.Remove("商品表11");
            sqlDA.Fill(dSet, "商品表11");

            //销售结算明细
            sqlComm.CommandText = "SELECT 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.数量, 销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.实计金额, 销售商品制单明细表.扣率, 销售商品制单明细表.毛利, 销售商品制单明细表.赠品 FROM 单位表 INNER JOIN 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID ON 单位表.ID = 销售商品制单表.单位ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1) AND (销售商品制单明细表.已付款金额 > 0)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表12")) dSet.Tables.Remove("商品表12");
            sqlDA.Fill(dSet, "商品表12");

            //销售未结算汇总
            sqlComm.CommandText = "SELECT 销售商品制单表.ID, 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号,单位表.单位名称, 销售商品制单表.价税合计, 销售商品制单表.未付款金额, 销售商品制单表.备注 FROM 单位表 INNER JOIN 销售商品制单表 ON 单位表.ID = 销售商品制单表.单位ID WHERE (销售商品制单表.未付款金额 > 0) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表13")) dSet.Tables.Remove("商品表13");
            sqlDA.Fill(dSet, "商品表13");

            //销售未结算明细
            sqlComm.CommandText = "SELECT 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.数量, 销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.实计金额, 销售商品制单明细表.扣率, 销售商品制单明细表.毛利, 销售商品制单明细表.赠品 FROM 单位表 INNER JOIN 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID ON 单位表.ID = 销售商品制单表.单位ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1) AND (销售商品制单明细表.未付款金额 > 0)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表14")) dSet.Tables.Remove("商品表14");
            sqlDA.Fill(dSet, "商品表14");

            //购进结算汇总
            sqlComm.CommandText = "SELECT 进货入库汇总表.ID,进货入库汇总表.单据编号,进货入库汇总表.日期, 单位表.单位编号,单位表.单位名称,进货入库汇总表.价税合计,进货入库汇总表.未付款金额,进货入库汇总表.备注 FROM 单位表 INNER JOIN 进货入库汇总表 ON 单位表.ID =进货入库汇总表.单位ID WHERE (进货入库汇总表.已付款金额 > 0) AND (进货入库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (进货入库汇总表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表15")) dSet.Tables.Remove("商品表15");
            sqlDA.Fill(dSet, "商品表15");

            //购进结算明细
            sqlComm.CommandText = "SELECT 进货入库汇总表.单据编号,进货入库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 进货入库明细表.数量, 进货入库明细表.单价, 进货入库明细表.金额, 进货入库明细表.实计金额, 进货入库明细表.扣率, 进货入库明细表.毛利, 进货入库明细表.赠品 FROM 单位表 INNER JOIN 进货入库明细表 INNER JOIN 进货入库汇总表 ON 进货入库明细表.单据ID =进货入库汇总表.ID INNER JOIN 商品表 ON 进货入库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 进货入库明细表.库房ID = 库房表.ID ON 单位表.ID =进货入库汇总表.单位ID WHERE (进货入库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (进货入库汇总表.BeActive = 1) AND (进货入库明细表.已付款金额 > 0)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表16")) dSet.Tables.Remove("商品表16");
            sqlDA.Fill(dSet, "商品表16");

            //购进未结算汇总
            sqlComm.CommandText = "SELECT 进货入库汇总表.ID,进货入库汇总表.单据编号,进货入库汇总表.日期, 单位表.单位编号,单位表.单位名称,进货入库汇总表.价税合计,进货入库汇总表.未付款金额,进货入库汇总表.备注 FROM 单位表 INNER JOIN 进货入库汇总表 ON 单位表.ID =进货入库汇总表.单位ID WHERE (进货入库汇总表.未付款金额 > 0) AND (进货入库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (进货入库汇总表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表17")) dSet.Tables.Remove("商品表17");
            sqlDA.Fill(dSet, "商品表17");

            //购进未结算明细
            sqlComm.CommandText = "SELECT 进货入库汇总表.单据编号,进货入库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 进货入库明细表.数量, 进货入库明细表.单价, 进货入库明细表.金额, 进货入库明细表.实计金额, 进货入库明细表.扣率, 进货入库明细表.毛利, 进货入库明细表.赠品 FROM 单位表 INNER JOIN 进货入库明细表 INNER JOIN 进货入库汇总表 ON 进货入库明细表.单据ID =进货入库汇总表.ID INNER JOIN 商品表 ON 进货入库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 进货入库明细表.库房ID = 库房表.ID ON 单位表.ID =进货入库汇总表.单位ID WHERE (进货入库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (进货入库汇总表.BeActive = 1) AND (进货入库明细表.未付款金额 > 0)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表18")) dSet.Tables.Remove("商品表18");
            sqlDA.Fill(dSet, "商品表18");

            //购进退补
            sqlComm.CommandText = "SELECT 购进退补差价汇总表.ID, 购进退补差价汇总表.单据编号, 购进退补差价汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 购进退补差价明细表.补价数量, 购进退补差价明细表.差价, 购进退补差价明细表.金额 FROM 购进退补差价汇总表 INNER JOIN 购进退补差价明细表 ON 购进退补差价汇总表.ID = 购进退补差价明细表.单据ID INNER JOIN 单位表 ON 购进退补差价汇总表.单位ID = 单位表.ID INNER JOIN 库房表 ON 购进退补差价明细表.库房ID = 库房表.ID WHERE (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " ORDER BY 购进退补差价汇总表.日期 DESC";

            if (dSet.Tables.Contains("商品表19")) dSet.Tables.Remove("商品表19");
            sqlDA.Fill(dSet, "商品表19");

            //销售退补
            sqlComm.CommandText = "SELECT 销售退补差价汇总表.ID, 销售退补差价汇总表.单据编号, 销售退补差价汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 销售退补差价明细表.补价数量, 销售退补差价明细表.差价, 销售退补差价明细表.金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID INNER JOIN 库房表 ON 销售退补差价明细表.库房ID = 库房表.ID WHERE (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " ORDER BY 销售退补差价汇总表.日期 DESC";

            if (dSet.Tables.Contains("商品表20")) dSet.Tables.Remove("商品表20");
            sqlDA.Fill(dSet, "商品表20");



            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["商品表1"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView2.DataSource = dSet.Tables["商品表2"];
            dataGridView3.DataSource = dSet.Tables["商品表3"];
            dataGridView3.Columns[0].Visible = false;
            dataGridView4.DataSource = dSet.Tables["商品表4"];
            dataGridView5.DataSource = dSet.Tables["商品表5"];
            dataGridView5.Columns[0].Visible = false;
            dataGridView6.DataSource = dSet.Tables["商品表6"];
            dataGridView7.DataSource = dSet.Tables["商品表7"];
            dataGridView7.Columns[0].Visible = false;
            dataGridView8.DataSource = dSet.Tables["商品表8"];
            dataGridView9.DataSource = dSet.Tables["商品表9"];
            dataGridView9.Columns[0].Visible = false;
            dataGridView10.DataSource = dSet.Tables["商品表10"];
            dataGridView11.DataSource = dSet.Tables["商品表11"];
            dataGridView11.Columns[0].Visible = false;
            dataGridView12.DataSource = dSet.Tables["商品表12"];
            dataGridView13.DataSource = dSet.Tables["商品表13"];
            dataGridView13.Columns[0].Visible = false;
            dataGridView14.DataSource = dSet.Tables["商品表14"];
            dataGridView15.DataSource = dSet.Tables["商品表15"];
            dataGridView15.Columns[0].Visible = false;
            dataGridView16.DataSource = dSet.Tables["商品表16"];
            dataGridView17.DataSource = dSet.Tables["商品表17"];
            dataGridView17.Columns[0].Visible = false;
            dataGridView18.DataSource = dSet.Tables["商品表18"];
            dataGridView19.DataSource = dSet.Tables["商品表19"];
            dataGridView19.Columns[0].Visible = false;
            dataGridView20.DataSource = dSet.Tables["商品表20"];
            dataGridView20.Columns[0].Visible = false;


            dataGridView2.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView4.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView6.Columns[10].DefaultCellStyle.Format = "f0";
            dataGridView8.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView10.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView12.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView14.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView16.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView18.Columns[8].DefaultCellStyle.Format = "f0";

            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "客户购销分析（销售汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "客户购销分析（销售明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "客户购销分析（销售退回汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "客户购销分析（销售退回明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, false, intUserLimit);
                    break;
                case 4:
                    strT = "客户购销分析（购进汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, false, intUserLimit);
                    break;
                case 5:
                    strT = "客户购销分析（购进明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, false, intUserLimit);
                    break;
                case 6:
                    strT = "客户购销分析（购进退回汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView7, strT, false, intUserLimit);
                    break;
                case 7:
                    strT = "客户购销分析（购进退回汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView8, strT, false, intUserLimit);
                    break;
                case 8:
                    strT = "客户购销分析（借物出库汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView9, strT, false, intUserLimit);
                    break;
                case 9:
                    strT = "客户购销分析（借物出库明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView10, strT, false, intUserLimit);
                    break;
                case 10:
                    strT = "客户购销分析（销售结算汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView11, strT, false, intUserLimit);
                    break;
                case 11:
                    strT = "客户购销分析（销售结算明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView12, strT, false, intUserLimit);
                    break;
                case 12:
                    strT = "客户购销分析（销售未结算汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView13, strT, false, intUserLimit);
                    break;
                case 13:
                    strT = "客户购销分析（销售结算未结算明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView14, strT, false, intUserLimit);
                    break;
                case 14:
                    strT = "客户购销分析（购进结算汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView15, strT, false, intUserLimit);
                    break;
                case 15:
                    strT = "客户购销分析（购进结算明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView16, strT, false, intUserLimit);
                    break;
                case 16:
                    strT = "客户购销分析（购进未结算汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView17, strT, false, intUserLimit);
                    break;
                case 17:
                    strT = "客户购销分析（购进未结算明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView18, strT, false, intUserLimit);
                    break;
            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "客户购销分析（销售汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "客户购销分析（销售明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "客户购销分析（销售退回汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "客户购销分析（销售退回明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, true, intUserLimit);
                    break;
                case 4:
                    strT = "客户购销分析（购进汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, true, intUserLimit);
                    break;
                case 5:
                    strT = "客户购销分析（购进明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, true, intUserLimit);
                    break;
                case 6:
                    strT = "客户购销分析（购进退回汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView7, strT, true, intUserLimit);
                    break;
                case 7:
                    strT = "客户购销分析（购进退回汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView8, strT, true, intUserLimit);
                    break;
                case 8:
                    strT = "客户购销分析（借物出库汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView9, strT, true, intUserLimit);
                    break;
                case 9:
                    strT = "客户购销分析（借物出库明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView10, strT, true, intUserLimit);
                    break;
                case 10:
                    strT = "客户购销分析（销售结算汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView11, strT, true, intUserLimit);
                    break;
                case 11:
                    strT = "客户购销分析（销售结算明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView12, strT, true, intUserLimit);
                    break;
                case 12:
                    strT = "客户购销分析（销售未结算汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView13, strT, true, intUserLimit);
                    break;
                case 13:
                    strT = "客户购销分析（销售结算未结算明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView14, strT, true, intUserLimit);
                    break;
                case 14:
                    strT = "客户购销分析（购进结算汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView15, strT, true, intUserLimit);
                    break;
                case 15:
                    strT = "客户购销分析（购进结算明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView16, strT, true, intUserLimit);
                    break;
                case 16:
                    strT = "客户购销分析（购进未结算汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView17, strT, true, intUserLimit);
                    break;
                case 17:
                    strT = "客户购销分析（购进未结算明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView18, strT, true, intUserLimit);
                    break;
            }
        }

        private void dataGridViewV_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgV = (DataGridView)sender;
            if (dgV.RowCount < 1)
                return;

            if (dgV.SelectedRows.Count < 1)
                return;

            string sTemp = "", sTemp1 = "";
            sTemp = dgV.SelectedRows[0].Cells[1].Value.ToString().ToUpper();
            sTemp1 = dgV.SelectedRows[0].Cells[0].Value.ToString();

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

                    childFormKCJWCKDJ.intUserID = intUserID;
                    childFormKCJWCKDJ.intUserLimit = intUserLimit;
                    childFormKCJWCKDJ.strUserLimit = strUserLimit;
                    childFormKCJWCKDJ.strUserName = strUserName;
                    childFormKCJWCKDJ.Show();
                    break;
            }
        }

        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= cTemp.Length; i++)
            {
                cTemp[i - 1] = 0;
                cTemp1[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 10;c1=0;
                        break;
                    case 2:
                        c = 11;c1=8;
                        break;
                    case 3:
                        c = 5;c1=0;
                        break;
                    case 4:
                        c = 10;c1=8;
                        break;
                    case 5:
                        c = 7;c1=0;
                        break;
                    case 6:
                        c = 13;c1=9;
                        break;
                    case 7:
                        c = 5;c1=0;
                        break;
                    case 8:
                        c = 10;c1=8;
                        break;
                    case 9:
                        c = 5;c1=0;
                        break;
                    case 10:
                        c = 10;c1=8;
                        break;
                    case 11:
                        c = 5;c1=0;
                        break;
                    case 12:
                        c = 11;c1=8;
                        break;
                    case 13:
                        c = 5;c1=0;
                        break;
                    case 14:
                        c = 11;c1=8;
                        break;
                    case 15:
                        c = 5;c1=0;
                        break;
                    case 16:
                        c = 11;c1=8;
                        break;
                    case 17:
                        c = 5;c1=0;
                        break;
                    case 18:
                        c = 11;c1=8;
                        break;
                    case 19:
                        c = 9; c1 = 7;
                        break;
                    case 20:
                        c = 9; c1 = 7;
                        break;
                    default:
                        c = 0;
                        break;
                }

                if (c != 0)
                {

                    for (j = 0; j < dSet.Tables["商品表" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp[i - 1] += decimal.Parse(dSet.Tables["商品表" + i.ToString()].Rows[j][c].ToString());
                        }
                        catch
                        {
                        }
                    }
                }
                if (c1 != 0)
                {

                    for (j = 0; j < dSet.Tables["商品表" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp1[i - 1] += decimal.Parse(dSet.Tables["商品表" + i.ToString()].Rows[j][c1].ToString());
                        }
                        catch
                        {
                        }
                    }
                }
                else
                    cTemp1[i - 1] = -1;


            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;
            if (!dSet.Tables.Contains("商品表" + c1.ToString()))
                return;
            toolStripStatusLabelC.Text = "共有" + dSet.Tables["商品表" + c1.ToString()].Rows.Count.ToString() + "条记录 金额合计" + cTemp[tabControl1.SelectedIndex].ToString("f2") + "元";
            if (cTemp1[tabControl1.SelectedIndex].ToString("f0") != "-1")
                toolStripStatusLabelC.Text += " 数量合计 " + cTemp1[tabControl1.SelectedIndex].ToString("f0");

        }

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            iSupplyCompany = 0;
            textBoxDWMC.Text = "";
            textBoxDWBH.Text = "";
        }
    }
}
