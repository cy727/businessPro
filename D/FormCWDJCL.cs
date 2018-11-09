using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormCWDJCL : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";
        //0,冲红 1,修改
        public int iStyle = 0;

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private int intDJID = 0;
        private int iSupplyCompany = 0;


        private ClassGetInformation cGetInformation;

        private bool isSaved = false;

        private int iConstLimit = 18; 
        
        public FormCWDJCL()
        {
            InitializeComponent();
        }

        private void FormCWDJCL_Load(object sender, EventArgs e)
        {

            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

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
                dateTimePicker1.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");

            }
            sqldr.Close();
            sqlConn.Close();

            switch (iStyle)
            {
                case 0: //冲红
                    comboBoxDJLB.Items.Add("购进商品单");
                    comboBoxDJLB.Items.Add("进货入库单");
                    comboBoxDJLB.Items.Add("销售出库单");
                    comboBoxDJLB.Items.Add("销售校对单");
                    comboBoxDJLB.Items.Add("购进退出单");
                    comboBoxDJLB.Items.Add("销售退回单");
                    comboBoxDJLB.Items.Add("应付账款单");
                    comboBoxDJLB.Items.Add("应收账款单");
                    comboBoxDJLB.Items.Add("借物出库单");
                    comboBoxDJLB.Items.Add("购进合同");
                    comboBoxDJLB.Items.Add("销售合同");
                    comboBoxDJLB.SelectedIndex = 0;
                    this.Text += ":冲红";
                    btnEdit.Text = "冲红";
                    break;
                case 1://修改
                    //comboBoxDJLB.Items.Add("进货订货单");
                    //comboBoxDJLB.Items.Add("销售出库单");
                    //comboBoxDJLB.Items.Add("购进退出单");
                    //comboBoxDJLB.Items.Add("销售退回单");
                    //comboBoxDJLB.Items.Add("借物出库单");
                    comboBoxDJLB.Items.Add("购进合同");
                    comboBoxDJLB.Items.Add("销售合同");

                    comboBoxDJLB.SelectedIndex = 0;
                    this.Text += ":修改";
                    btnEdit.Text = "修改";

                    break;
                case 2://备注
                    comboBoxDJLB.Items.Add("购进商品制单");
                    comboBoxDJLB.Items.Add("销售出库单");
                    comboBoxDJLB.Items.Add("购进退出单");
                    comboBoxDJLB.Items.Add("销售退回单");
                    comboBoxDJLB.Items.Add("应付账款单");
                    comboBoxDJLB.Items.Add("应收账款单");
                    comboBoxDJLB.Items.Add("借物出库单");
                    comboBoxDJLB.Items.Add("进货入库验货单");
                    comboBoxDJLB.Items.Add("销售出库校对单");
                    //comboBoxDJLB.Items.Add("购进合同");
                    //comboBoxDJLB.Items.Add("销售合同");

                    comboBoxDJLB.SelectedIndex = 0;
                    this.Text += ":备注修改";
                    btnEdit.Text = "备注修改";

                    break;
                default:
                    break;
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(100, "") == 0)
            {
                //return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
            }

        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1100, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
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
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }
            }

        }

        private void btnAccepy_Click(object sender, EventArgs e)
        {
            string strTemp = "";

            switch (iStyle)
            {
                case 0://冲红
                    switch (comboBoxDJLB.SelectedIndex)
                    {
                        case 0://进货入库单
                            sqlComm.CommandText = "SELECT 购进商品制单表.ID, 购进商品制单表.单据编号, 购进商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 购进商品制单表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 购进商品制单表 INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID INNER JOIN  职员表 ON 购进商品制单表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 购进商品制单表.业务员ID = [职员表_1].ID WHERE (购进商品制单表.BeActive = 1)";
                            strTemp = "购进商品制单表";
                            break;
                        case 1://进货入库单
                            sqlComm.CommandText = "SELECT 进货入库汇总表.ID, 进货入库汇总表.单据编号, 进货入库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 进货入库汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 进货入库汇总表 INNER JOIN 单位表 ON 进货入库汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 进货入库汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 进货入库汇总表.业务员ID = [职员表_1].ID WHERE (进货入库汇总表.BeActive = 1)";
                            strTemp = "进货入库汇总表";
                            break;
                        case 2://销售出库单
                            sqlComm.CommandText = "SELECT 销售商品制单表.ID, 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 销售商品制单表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN  职员表 ON 销售商品制单表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 销售商品制单表.业务员ID = [职员表_1].ID WHERE (销售商品制单表.BeActive = 1)";
                            strTemp = "销售商品制单表";
                            break;
                        case 3://销售校对单
                            sqlComm.CommandText = "SELECT 销售出库汇总表.ID, 销售出库汇总表.单据编号, 销售出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 销售出库汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 销售出库汇总表 INNER JOIN 单位表 ON 销售出库汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 销售出库汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 销售出库汇总表.业务员ID = [职员表_1].ID WHERE (销售出库汇总表.BeActive = 1)";
                            strTemp = "销售出库汇总表";
                            break;
                        case 4://购进退出单
                            sqlComm.CommandText = "SELECT 进货退出汇总表.ID, 进货退出汇总表.单据编号, 进货退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 进货退出汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 进货退出汇总表 INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 进货退出汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 进货退出汇总表.业务员ID = [职员表_1].ID WHERE (进货退出汇总表.BeActive = 1)";
                            strTemp = "进货退出汇总表";
                            break;
                        case 5://销售退回单
                            sqlComm.CommandText = "SELECT 销售退出汇总表.ID, 销售退出汇总表.单据编号, 销售退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 销售退出汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 销售退出汇总表 INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 销售退出汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 销售退出汇总表.业务员ID = [职员表_1].ID WHERE (销售退出汇总表.BeActive = 1)";
                            strTemp = "销售退出汇总表";
                            break;
                        case 6://应付账款单
                            sqlComm.CommandText = "SELECT 结算付款汇总表.ID, 结算付款汇总表.单据编号, 结算付款汇总表.日期,单位表.单位编号, 单位表.单位名称, 结算付款汇总表.实计金额, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 结算付款汇总表 INNER JOIN 单位表 ON 结算付款汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 结算付款汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 结算付款汇总表.业务员ID = [职员表_1].ID WHERE (结算付款汇总表.BeActive = 1)";
                            strTemp = "结算付款汇总表";
                            break;
                        case 7://应收账款单
                            sqlComm.CommandText = "SELECT 结算收款汇总表.ID, 结算收款汇总表.单据编号, 结算收款汇总表.日期,单位表.单位编号, 单位表.单位名称, 结算收款汇总表.实计金额, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 结算收款汇总表 INNER JOIN 单位表 ON 结算收款汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 结算收款汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 结算收款汇总表.业务员ID = [职员表_1].ID WHERE (结算收款汇总表.BeActive = 1)";
                            strTemp = "结算收款汇总表";
                            break;
                        case 8://借物出库单
                            //sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 借物出库汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 借物出库汇总表.业务员ID = [职员表_1].ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.冲抵单号ID IS NULL)";
                            sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 借物出库汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 借物出库汇总表.业务员ID = [职员表_1].ID WHERE (借物出库汇总表.BeActive = 1)";
                            strTemp = "借物出库汇总表";
                            break;
                        case 9://购进合同
                            sqlComm.CommandText = "SELECT 采购合同表.ID, 采购合同表.合同编号, 采购合同表.签订时间, 单位表.单位编号, 单位表.单位名称, 采购合同表.金额, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员 FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 采购合同表.操作员ID = 操作员.ID WHERE (采购合同表.BeActive = 1) AND (采购合同表.执行标记 = 0)";
                            strTemp = "采购合同表";
                            break;
                        case 10://销售合同
                            sqlComm.CommandText = "SELECT 销售合同表.ID, 销售合同表.合同编号, 销售合同表.签订时间, 单位表.单位编号, 单位表.单位名称, 销售合同表.金额, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员 FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 销售合同表.操作员ID = 操作员.ID WHERE (销售合同表.BeActive = 1) AND (销售合同表.执行标记 = 0)";
                            strTemp = "销售合同表";
                            break;

                    }
                    break;

                case 1: //修改
                    switch (comboBoxDJLB.SelectedIndex)
                    {
                            /*
                        case 0://进货订货单
                            sqlComm.CommandText = "SELECT 购进商品制单表.ID, 购进商品制单表.单据编号, 购进商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 购进商品制单表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 购进商品制单表 INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID INNER JOIN  职员表 ON 购进商品制单表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 购进商品制单表.业务员ID = [职员表_1].ID WHERE (购进商品制单表.BeActive = 1)";
                            strTemp = "购进商品制单表";
                            break;
                        case 1://销售出库单
                            sqlComm.CommandText = "SELECT 销售商品制单表.ID, 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 销售商品制单表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN  职员表 ON 销售商品制单表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 销售商品制单表.业务员ID = [职员表_1].ID WHERE (销售商品制单表.BeActive = 1)";
                            strTemp = "销售商品制单表";
                            break;
                        case 2://购进退出单
                            sqlComm.CommandText = "SELECT 进货退出汇总表.ID, 进货退出汇总表.单据编号, 进货退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 进货退出汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 进货退出汇总表 INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 进货退出汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 进货退出汇总表.业务员ID = [职员表_1].ID WHERE (进货退出汇总表.BeActive = 1)";
                            strTemp = "进货退出汇总表";
                            break;
                        case 3://销售退回单
                            sqlComm.CommandText = "SELECT 销售退出汇总表.ID, 销售退出汇总表.单据编号, 销售退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 销售退出汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 销售退出汇总表 INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 销售退出汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 销售退出汇总表.业务员ID = [职员表_1].ID WHERE (销售退出汇总表.BeActive = 1)";
                            strTemp = "销售退出汇总表";
                            break;
                        case 4://借物出库单
                            sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 借物出库汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 借物出库汇总表.业务员ID = [职员表_1].ID WHERE (借物出库汇总表.BeActive = 1)";
                            strTemp = "借物出库汇总表";
                            break;
                             */
                        case 0://购进合同
                            sqlComm.CommandText = "SELECT 采购合同表.ID, 采购合同表.合同编号, 采购合同表.签订时间, 单位表.单位编号, 单位表.单位名称, 采购合同表.金额, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员 FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 采购合同表.操作员ID = 操作员.ID WHERE (采购合同表.BeActive = 1) AND (采购合同表.执行标记 = 0)";
                            strTemp = "采购合同表";
                            break;

                        case 1://销售合同
                            sqlComm.CommandText = "SELECT 销售合同表.ID, 销售合同表.合同编号, 销售合同表.签订时间, 单位表.单位编号, 单位表.单位名称, 销售合同表.金额, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员 FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 销售合同表.操作员ID = 操作员.ID WHERE (销售合同表.BeActive = 1)AND (销售合同表.执行标记 = 0)";
                            strTemp = "销售合同表";
                            break;
                    }
                    break;

                case 2: //备注
                    switch (comboBoxDJLB.SelectedIndex)
                    {
                        case 0://进货订货单
                            sqlComm.CommandText = "SELECT 购进商品制单表.ID, 购进商品制单表.单据编号, 购进商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 购进商品制单表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员, 购进商品制单表.备注 FROM 购进商品制单表 INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID INNER JOIN  职员表 ON 购进商品制单表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 购进商品制单表.业务员ID = [职员表_1].ID WHERE (购进商品制单表.BeActive = 1)";
                            strTemp = "购进商品制单表";
                            break;
                        case 1://销售出库单
                            sqlComm.CommandText = "SELECT 销售商品制单表.ID, 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 销售商品制单表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员, 销售商品制单表.备注 FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN  职员表 ON 销售商品制单表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 销售商品制单表.业务员ID = [职员表_1].ID WHERE (销售商品制单表.BeActive = 1)";
                            strTemp = "销售商品制单表";
                            break;
                        case 2://购进退出单
                            sqlComm.CommandText = "SELECT 进货退出汇总表.ID, 进货退出汇总表.单据编号, 进货退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 进货退出汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员, 进货退出汇总表.备注 FROM 进货退出汇总表 INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 进货退出汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 进货退出汇总表.业务员ID = [职员表_1].ID WHERE (进货退出汇总表.BeActive = 1)";
                            strTemp = "进货退出汇总表";
                            break;
                        case 3://销售退回单
                            sqlComm.CommandText = "SELECT 销售退出汇总表.ID, 销售退出汇总表.单据编号, 销售退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 销售退出汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员, 销售退出汇总表.备注 FROM 销售退出汇总表 INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 销售退出汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 销售退出汇总表.业务员ID = [职员表_1].ID WHERE (销售退出汇总表.BeActive = 1)";
                            strTemp = "销售退出汇总表";
                            break;
                        case 4://应付账款单
                            sqlComm.CommandText = "SELECT 结算付款汇总表.ID, 结算付款汇总表.单据编号, 结算付款汇总表.日期,单位表.单位编号, 单位表.单位名称, 结算付款汇总表.实计金额, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员, 结算付款汇总表.备注, 结算付款汇总表.备注2 FROM 结算付款汇总表 INNER JOIN 单位表 ON 结算付款汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 结算付款汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 结算付款汇总表.业务员ID = [职员表_1].ID WHERE (结算付款汇总表.BeActive = 1)";
                            strTemp = "结算付款汇总表";
                            break;
                        case 5://应收账款单
                            sqlComm.CommandText = "SELECT 结算收款汇总表.ID, 结算收款汇总表.单据编号, 结算收款汇总表.日期,单位表.单位编号, 单位表.单位名称, 结算收款汇总表.实计金额, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员, 结算收款汇总表.备注, 结算收款汇总表.备注2 FROM 结算收款汇总表 INNER JOIN 单位表 ON 结算收款汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 结算收款汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 结算收款汇总表.业务员ID = [职员表_1].ID WHERE (结算收款汇总表.BeActive = 1)";
                            strTemp = "结算收款汇总表";
                            break;
                        case 6://借物出库单
                            sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员, 借物出库汇总表.备注 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 借物出库汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 借物出库汇总表.业务员ID = [职员表_1].ID WHERE (借物出库汇总表.BeActive = 1)";
                            strTemp = "借物出库汇总表";
                            break;
                        case 7://进货入库验货单
                            sqlComm.CommandText = "SELECT 进货入库汇总表.ID, 进货入库汇总表.单据编号, 进货入库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 进货入库汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员, 进货入库汇总表.备注 FROM 进货入库汇总表 INNER JOIN 单位表 ON 进货入库汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 进货入库汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 进货入库汇总表.业务员ID = [职员表_1].ID WHERE (进货入库汇总表.BeActive = 1)";
                            strTemp = "进货入库汇总表";
                            break;

                        case 8://销售出库校对单
                            sqlComm.CommandText = "SELECT 销售出库汇总表.ID, 销售出库汇总表.单据编号, 销售出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 销售出库汇总表.价税合计, [职员表_1].职员姓名 AS 业务员, 职员表.职员姓名 AS 操作员, 销售出库汇总表.备注 FROM 销售出库汇总表 INNER JOIN 单位表 ON 销售出库汇总表.单位ID = 单位表.ID INNER JOIN  职员表 ON 销售出库汇总表.操作员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 销售出库汇总表.业务员ID = [职员表_1].ID WHERE (销售出库汇总表.BeActive = 1)";
                            strTemp = "销售出库汇总表";
                            break;
                    }
                    break;
                default:
                    return;
            }

            if (iSupplyCompany != 0)
            {
                sqlComm.CommandText += " AND (单位表.ID = "+iSupplyCompany.ToString()+")"; 
            }

            if (textBoxDJBH.Text!= "")
            {
                if ((iStyle == 0 && comboBoxDJLB.SelectedIndex >= 9) || (iStyle == 1 && comboBoxDJLB.SelectedIndex >= 0))
                    sqlComm.CommandText += " AND (" + strTemp + ".合同编号 LIKE N'%" + textBoxDJBH.Text + "%')";
                else
                    sqlComm.CommandText += " AND (" + strTemp + ".单据编号 LIKE N'%" + textBoxDJBH .Text+ "%')";
            }

            
            if (!checkBoxNo1.Checked)
            {
                if ((iStyle == 0 && comboBoxDJLB.SelectedIndex >= 9) || (iStyle == 1 && comboBoxDJLB.SelectedIndex >= 0))
                    sqlComm.CommandText += " AND (" + strTemp + ".签订时间 >= CONVERT(DATETIME, '" + dateTimePicker1.Value.ToShortDateString() + " 00:00:00', 102))";
                else
                    sqlComm.CommandText += " AND (" + strTemp + ".日期 >= CONVERT(DATETIME, '"+dateTimePicker1.Value.ToShortDateString()+" 00:00:00', 102))";
            }

            if (!checkBoxNo2.Checked)
            {
                if ((iStyle == 0 && comboBoxDJLB.SelectedIndex >= 9) || (iStyle == 1 && comboBoxDJLB.SelectedIndex >= 0))
                    sqlComm.CommandText += " AND (" + strTemp + ".签订时间 <= CONVERT(DATETIME, '" + dateTimePicker2.Value.ToShortDateString() + " 00:00:00', 102))";
                else
                    sqlComm.CommandText += " AND (" + strTemp + ".日期 <= CONVERT(DATETIME, '" + dateTimePicker2.Value.ToShortDateString() + " 00:00:00', 102))";

            }

            if ((iStyle == 0 && comboBoxDJLB.SelectedIndex >= 9) || (iStyle == 1 && comboBoxDJLB.SelectedIndex >= 0))
                sqlComm.CommandText += " ORDER BY  签订时间 DESC";
            else
                sqlComm.CommandText += " ORDER BY  日期 DESC";
            /*
            if (!checkBoxNo1.Checked)
            {
                    sqlComm.CommandText += " AND (" + strTemp + ".签订时间 >= CONVERT(DATETIME, '" + dateTimePicker1.Value.ToShortDateString() + " 00:00:00', 102))";
            }

            if (!checkBoxNo2.Checked)
            {
                    sqlComm.CommandText += " AND (" + strTemp + ".签订时间 <= CONVERT(DATETIME, '" + dateTimePicker2.Value.ToShortDateString() + " 23:59:59', 102))";
            }
            */

            sqlConn.Open();
            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据表"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            sqlConn.Close();


            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString();
            dataGridViewDJMX.Focus();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
  
            //保存完毕
            if (dataGridViewDJMX.SelectedRows.Count<1)
            {
                MessageBox.Show("请选择要调整的单据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            intDJID = Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString());

            switch (iStyle)
            {
                case 0://冲红
                    switch (comboBoxDJLB.SelectedIndex)
                    {
                        case 0://进货订货单
                            // 创建此子窗体的一个新实例。
                            FormGJSPZD_EDIT childFormGJSPZD = new FormGJSPZD_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormGJSPZD.MdiParent = this.MdiParent;

                            childFormGJSPZD.strConn = strConn;
                            childFormGJSPZD.intDJID = intDJID;
                            childFormGJSPZD.intUserID = intUserID;
                            childFormGJSPZD.intUserLimit = intUserLimit;
                            childFormGJSPZD.strUserLimit = strUserLimit;
                            childFormGJSPZD.strUserName = strUserName;

                            if (intUserLimit < iConstLimit)
                            {
                                childFormGJSPZD.printToolStripButton.Visible = false;
                                childFormGJSPZD.printPreviewToolStripButton.Visible = false;
                            }
                            childFormGJSPZD.Show();
                            break;
                        case 1://进货入库单

                            // 创建此子窗体的一个新实例。
                            FormJHRKYHD_EDIT childFormJHRKYHD = new FormJHRKYHD_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormJHRKYHD.MdiParent = this.MdiParent;

                            childFormJHRKYHD.strConn = strConn;
                            childFormJHRKYHD.intDJID = intDJID;
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
                        case 2://销售出库单
                            // 创建此子窗体的一个新实例。
                            FormXSCKZD_EDIT childFormXSCKZD = new FormXSCKZD_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormXSCKZD.MdiParent = this.MdiParent;

                            childFormXSCKZD.strConn = strConn;
                            childFormXSCKZD.intDJID = intDJID;
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
                        case 3://销售校对单
                            // 创建此子窗体的一个新实例。
                            FormXSCKJD_EDIT childFormXSCKJD = new FormXSCKJD_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormXSCKJD.MdiParent = this.MdiParent;

                            childFormXSCKJD.strConn = strConn;
                            childFormXSCKJD.intDJID = intDJID;
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
                        case 4://购进退出单
                            // 创建此子窗体的一个新实例。
                            FormJHTCZD_EDIT childFormJHTCZD = new FormJHTCZD_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormJHTCZD.MdiParent = this.MdiParent;

                            childFormJHTCZD.strConn = strConn;
                            childFormJHTCZD.intDJID = intDJID;
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
                        case 5://销售退回单
                            // 创建此子窗体的一个新实例。
                            FormXSTHZD_EDIT childFormXSTHZD = new FormXSTHZD_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormXSTHZD.MdiParent = this.MdiParent;

                            childFormXSTHZD.strConn = strConn;
                            childFormXSTHZD.intDJID = intDJID;
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
                        case 6://应付账款单
                            // 创建此子窗体的一个新实例。
                            FormYFZKJS_EDIT childFormYFZKJS = new FormYFZKJS_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormYFZKJS.MdiParent = this.MdiParent;

                            childFormYFZKJS.strConn = strConn;
                            childFormYFZKJS.intDJID = intDJID;
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
                        case 7://应付账款单
                            // 创建此子窗体的一个新实例。
                            FormYSZKJS_EDIT childFormYSZKJS = new FormYSZKJS_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormYSZKJS.MdiParent = this.MdiParent;

                            childFormYSZKJS.strConn = strConn;
                            childFormYSZKJS.intDJID = intDJID;
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
                        case 8://借物出库单
                            // 创建此子窗体的一个新实例。
                            FormKCJWCKDJ_EDIT childFormKCJWCKDJ = new FormKCJWCKDJ_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormKCJWCKDJ.MdiParent = this.MdiParent;

                            childFormKCJWCKDJ.strConn = strConn;
                            childFormKCJWCKDJ.intDJID = intDJID;
                            childFormKCJWCKDJ.iStyle = 0;
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

                        case 9://采购合同
                            // 创建此子窗体的一个新实例。
                            FormCGHT_EDIT childFormCGHT = new FormCGHT_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormCGHT.MdiParent = this.MdiParent;

                            childFormCGHT.strConn = strConn;
                            childFormCGHT.iDJID= intDJID;
                            childFormCGHT.iStyle = 0;

                            childFormCGHT.intUserID = intUserID;
                            childFormCGHT.intUserLimit = intUserLimit;
                            childFormCGHT.strUserLimit = strUserLimit;
                            childFormCGHT.strUserName = strUserName;
                            childFormCGHT.Show();
                            break;

                        case 10://销售合同
                            // 创建此子窗体的一个新实例。
                            FormXSHT_EDIT childFormXSHT = new FormXSHT_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormXSHT.MdiParent = this.MdiParent;

                            childFormXSHT.strConn = strConn;
                            childFormXSHT.iDJID = intDJID;
                            childFormXSHT.iStyle = 0;

                            childFormXSHT.intUserID = intUserID;
                            childFormXSHT.intUserLimit = intUserLimit;
                            childFormXSHT.strUserLimit = strUserLimit;
                            childFormXSHT.strUserName = strUserName;
                            childFormXSHT.Show();
                            break;
                    }
                    break;

                case 1: //修改
                    switch (comboBoxDJLB.SelectedIndex)
                    {
                            /*
                        case 0://进货订货单
                            // 创建此子窗体的一个新实例。
                            FormGJSPZD_EDIT childFormGJSPZD = new FormGJSPZD_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormGJSPZD.MdiParent = this.MdiParent;

                            childFormGJSPZD.strConn = strConn;
                            childFormGJSPZD.intDJID = intDJID;
                            childFormGJSPZD.intUserID = intUserID;
                            childFormGJSPZD.intUserLimit = intUserLimit;
                            childFormGJSPZD.strUserLimit = strUserLimit;
                            childFormGJSPZD.strUserName = strUserName;
                            childFormGJSPZD.Show();
                            break;
                        case 1://销售出库单
                            // 创建此子窗体的一个新实例。
                            FormXSCKZD_EDIT childFormXSCKZD = new FormXSCKZD_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormXSCKZD.MdiParent = this.MdiParent;

                            childFormXSCKZD.strConn = strConn;
                            childFormXSCKZD.intDJID = intDJID;
                            childFormXSCKZD.iStyle=1;

                            childFormXSCKZD.intUserID = intUserID;
                            childFormXSCKZD.intUserLimit = intUserLimit;
                            childFormXSCKZD.strUserLimit = strUserLimit;
                            childFormXSCKZD.strUserName = strUserName;
                            childFormXSCKZD.Show();
                            break;

                        case 2://购进退出单
                            // 创建此子窗体的一个新实例。
                            FormJHTCZD_EDIT childFormJHTCZD = new FormJHTCZD_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormJHTCZD.MdiParent = this.MdiParent;

                            childFormJHTCZD.strConn = strConn;
                            childFormJHTCZD.intDJID = intDJID;
                            childFormJHTCZD.iStyle = 1;

                            childFormJHTCZD.intUserID = intUserID;
                            childFormJHTCZD.intUserLimit = intUserLimit;
                            childFormJHTCZD.strUserLimit = strUserLimit;
                            childFormJHTCZD.strUserName = strUserName;
                            childFormJHTCZD.Show();
                            break;

                        case 3://销售退回单
                            // 创建此子窗体的一个新实例。
                            FormXSTHZD_EDIT childFormXSTHZD = new FormXSTHZD_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormXSTHZD.MdiParent = this.MdiParent;

                            childFormXSTHZD.strConn = strConn;
                            childFormXSTHZD.intDJID = intDJID;
                            childFormXSTHZD.iStyle = 1;

                            childFormXSTHZD.intUserID = intUserID;
                            childFormXSTHZD.intUserLimit = intUserLimit;
                            childFormXSTHZD.strUserLimit = strUserLimit;
                            childFormXSTHZD.strUserName = strUserName;
                            childFormXSTHZD.Show();
                            break;
                        case 4://借物出库单
                            // 创建此子窗体的一个新实例。
                            FormKCJWCKDJ_EDIT childFormKCJWCKDJ = new FormKCJWCKDJ_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormKCJWCKDJ.MdiParent = this.MdiParent;

                            childFormKCJWCKDJ.strConn = strConn;
                            childFormKCJWCKDJ.intDJID = intDJID;
                            childFormKCJWCKDJ.iStyle = 1;

                            childFormKCJWCKDJ.intUserID = intUserID;
                            childFormKCJWCKDJ.intUserLimit = intUserLimit;
                            childFormKCJWCKDJ.strUserLimit = strUserLimit;
                            childFormKCJWCKDJ.strUserName = strUserName;
                            childFormKCJWCKDJ.Show();                                
                            break;
                             * */
                        case 0://采购合同
                            // 创建此子窗体的一个新实例。
                            FormCGHT_EDIT childFormCGHT = new FormCGHT_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormCGHT.MdiParent = this.MdiParent;

                            childFormCGHT.strConn = strConn;
                            childFormCGHT.iDJID = intDJID;
                            childFormCGHT.iStyle = 1;

                            childFormCGHT.intUserID = intUserID;
                            childFormCGHT.intUserLimit = intUserLimit;
                            childFormCGHT.strUserLimit = strUserLimit;
                            childFormCGHT.strUserName = strUserName;
                            childFormCGHT.Show();
                            break;

                        case 1://采购合同
                            // 创建此子窗体的一个新实例。
                            FormXSHT_EDIT childFormXSHT = new FormXSHT_EDIT();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormXSHT.MdiParent = this.MdiParent;

                            childFormXSHT.strConn = strConn;
                            childFormXSHT.iDJID = intDJID;
                            childFormXSHT.iStyle = 1;

                            childFormXSHT.intUserID = intUserID;
                            childFormXSHT.intUserLimit = intUserLimit;
                            childFormXSHT.strUserLimit = strUserLimit;
                            childFormXSHT.strUserName = strUserName;
                            childFormXSHT.Show();
                            break;
                    }
                    break;
                case 2://备注
                    switch (comboBoxDJLB.SelectedIndex)
                    {
                        case 0://进货入库单

                            // 创建此子窗体的一个新实例。
                            FormBZXG childFormBZXG = new FormBZXG();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormBZXG.MdiParent = this.MdiParent;

                            childFormBZXG.strConn = strConn;
                            childFormBZXG.iDJLX = 0;

                            childFormBZXG.intUserID = intUserID;
                            childFormBZXG.intDJID = intDJID;
                            childFormBZXG.intUserLimit = intUserLimit;
                            childFormBZXG.strUserLimit = strUserLimit;
                            childFormBZXG.strUserName = strUserName;
                            childFormBZXG.Show();
                            break;
                        case 1://销售出库单

                            // 创建此子窗体的一个新实例。
                            FormBZXG childFormBZXG1 = new FormBZXG();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormBZXG1.MdiParent = this.MdiParent;

                            childFormBZXG1.strConn = strConn;
                            childFormBZXG1.iDJLX = 1;

                            childFormBZXG1.intUserID = intUserID;
                            childFormBZXG1.intDJID = intDJID;
                            childFormBZXG1.intUserLimit = intUserLimit;
                            childFormBZXG1.strUserLimit = strUserLimit;
                            childFormBZXG1.strUserName = strUserName;
                            childFormBZXG1.Show();
                            break;
                        case 2://购进退出单
                            // 创建此子窗体的一个新实例。
                            FormBZXG childFormBZXG2 = new FormBZXG();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormBZXG2.MdiParent = this.MdiParent;

                            childFormBZXG2.strConn = strConn;
                            childFormBZXG2.iDJLX = 2;

                            childFormBZXG2.intUserID = intUserID;
                            childFormBZXG2.intDJID = intDJID;
                            childFormBZXG2.intUserLimit = intUserLimit;
                            childFormBZXG2.strUserLimit = strUserLimit;
                            childFormBZXG2.strUserName = strUserName;
                            childFormBZXG2.Show();
                            break;
                        case 3://销售退回单
                            // 创建此子窗体的一个新实例。
                            FormBZXG childFormBZXG3 = new FormBZXG();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormBZXG3.MdiParent = this.MdiParent;

                            childFormBZXG3.strConn = strConn;
                            childFormBZXG3.iDJLX = 3;

                            childFormBZXG3.intUserID = intUserID;
                            childFormBZXG3.intDJID = intDJID;
                            childFormBZXG3.intUserLimit = intUserLimit;
                            childFormBZXG3.strUserLimit = strUserLimit;
                            childFormBZXG3.strUserName = strUserName;
                            childFormBZXG3.Show();
                            break;
                        case 4://应付账款单
                            // 创建此子窗体的一个新实例。
                            FormBZXG childFormBZXG4 = new FormBZXG();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormBZXG4.MdiParent = this.MdiParent;

                            childFormBZXG4.strConn = strConn;
                            childFormBZXG4.iDJLX = 4;

                            childFormBZXG4.intUserID = intUserID;
                            childFormBZXG4.intDJID = intDJID;
                            childFormBZXG4.intUserLimit = intUserLimit;
                            childFormBZXG4.strUserLimit = strUserLimit;
                            childFormBZXG4.strUserName = strUserName;
                            childFormBZXG4.Show(); ;
                            break;
                        case 5://应付账款单
                            // 创建此子窗体的一个新实例。
                            FormBZXG childFormBZXG5 = new FormBZXG();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormBZXG5.MdiParent = this.MdiParent;

                            childFormBZXG5.strConn = strConn;
                            childFormBZXG5.iDJLX = 5;

                            childFormBZXG5.intUserID = intUserID;
                            childFormBZXG5.intDJID = intDJID;
                            childFormBZXG5.intUserLimit = intUserLimit;
                            childFormBZXG5.strUserLimit = strUserLimit;
                            childFormBZXG5.strUserName = strUserName;
                            childFormBZXG5.Show();
                            break;
                        case 6://借物出库单
                            // 创建此子窗体的一个新实例。
                            FormBZXG childFormBZXG6 = new FormBZXG();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormBZXG6.MdiParent = this.MdiParent;

                            childFormBZXG6.strConn = strConn;
                            childFormBZXG6.iDJLX = 6;

                            childFormBZXG6.intUserID = intUserID;
                            childFormBZXG6.intDJID = intDJID;
                            childFormBZXG6.intUserLimit = intUserLimit;
                            childFormBZXG6.strUserLimit = strUserLimit;
                            childFormBZXG6.strUserName = strUserName;
                            childFormBZXG6.Show();
                            break;
                        case 7://借物出库单
                            // 创建此子窗体的一个新实例。
                            FormBZXG childFormBZXG7 = new FormBZXG();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormBZXG7.MdiParent = this.MdiParent;

                            childFormBZXG7.strConn = strConn;
                            childFormBZXG7.iDJLX = 7;

                            childFormBZXG7.intUserID = intUserID;
                            childFormBZXG7.intDJID = intDJID;
                            childFormBZXG7.intUserLimit = intUserLimit;
                            childFormBZXG7.strUserLimit = strUserLimit;
                            childFormBZXG7.strUserName = strUserName;
                            childFormBZXG7.Show();
                            break;
                        case 8://借物出库单
                            // 创建此子窗体的一个新实例。
                            FormBZXG childFormBZXG8 = new FormBZXG();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormBZXG8.MdiParent = this.MdiParent;

                            childFormBZXG8.strConn = strConn;
                            childFormBZXG8.iDJLX = 8;

                            childFormBZXG8.intUserID = intUserID;
                            childFormBZXG8.intDJID = intDJID;
                            childFormBZXG8.intUserLimit = intUserLimit;
                            childFormBZXG8.strUserLimit = strUserLimit;
                            childFormBZXG8.strUserName = strUserName;
                            childFormBZXG8.Show();
                            break;
                            /*
                        case 7://采购合同
                            // 创建此子窗体的一个新实例。
                            FormBZXG childFormBZXG7 = new FormBZXG();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormBZXG7.MdiParent = this.MdiParent;

                            childFormBZXG7.strConn = strConn;
                            childFormBZXG7.iDJLX = 7;

                            childFormBZXG7.intUserID = intUserID;
                            childFormBZXG7.intDJID = intDJID;
                            childFormBZXG7.intUserLimit = intUserLimit;
                            childFormBZXG7.strUserLimit = strUserLimit;
                            childFormBZXG7.strUserName = strUserName;
                            childFormBZXG7.Show();
                            break;

                        case 8://销售合同
                            // 创建此子窗体的一个新实例。
                            FormBZXG childFormBZXG8 = new FormBZXG();
                            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                            childFormBZXG8.MdiParent = this.MdiParent;

                            childFormBZXG8.strConn = strConn;
                            childFormBZXG8.iDJLX = 8;

                            childFormBZXG8.intUserID = intUserID;
                            childFormBZXG8.intDJID = intDJID;
                            childFormBZXG8.intUserLimit = intUserLimit;
                            childFormBZXG8.strUserLimit = strUserLimit;
                            childFormBZXG8.strUserName = strUserName;
                            childFormBZXG8.Show();
                            break;
                          */
                    }
                    break;
                default:
                    return;
            }
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
             btnEdit_Click(null,null);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "错误单据处理;";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true,intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "错误单据处理;";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);

        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && dataGridViewDJMX.Focused)
            {
                //System.Windows.Forms.SendKeys.Send("{tab}");
                btnEdit_Click(null, null);//
                return true;
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void comboBoxDJLB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                btnAccepy.Focus();
            }
        }
    }
}