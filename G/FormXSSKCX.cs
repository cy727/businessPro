using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXSSKCX : Form
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
        private decimal[] cTemp = new decimal[2] { 0, 0 };

        public int LIMITACCESS1 = 15;

        public FormXSSKCX()
        {
            InitializeComponent();
        }

        private void FormXSSKCX_Load(object sender, EventArgs e)
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
                    LIMITACCESS1 = int.Parse(sqldr.GetValue(8).ToString());
                }
                catch
                {
                    LIMITACCESS1 = 15;
                }
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
            comboBoxYWY.SelectedIndex = comboBoxYWY.Items.Count - 1;

            if (intUserLimit <= LIMITACCESS1)
            {
                comboBoxYWY.SelectedValue = intUserID;
                comboBoxYWY.Enabled = false;
            }

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

            /*
            sqlComm.CommandText = "SELECT 结算收款汇总表.单据编号, 结算收款汇总表.日期, 账簿表.账簿编号, 账簿表.账簿名称, 结算收款明细表.冲应付款, 结算收款明细表.扣率, 结算收款明细表.付款金额, 结算收款明细表.支票号, 结算收款明细表.备注, 结算收款明细表.摘要, 职员表.职员姓名 AS 业务员 FROM 结算收款汇总表 INNER JOIN 结算收款明细表 ON 结算收款汇总表.ID = 结算收款明细表.单据ID INNER JOIN 账簿表 ON 结算收款明细表.账簿ID = 账簿表.ID INNER JOIN 职员表 ON 结算收款汇总表.业务员ID = 职员表.ID INNER JOIN 单位表 ON 结算收款汇总表.单位ID = 单位表.ID WHERE (结算收款汇总表.BeActive = 1) AND (结算收款汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (结算收款汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");
            */


            //未结算汇总
            //sqlComm.CommandText = "SELECT 销售商品制单表.ID, 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号,单位表.单位名称, 销售商品制单表.价税合计, 销售商品制单表.未付款金额, 销售商品制单表.备注 FROM 单位表 INNER JOIN 销售商品制单表 ON 单位表.ID = 销售商品制单表.单位ID WHERE (销售商品制单表.未付款金额 > 0) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";
            //sqlComm.CommandText = "SELECT 收款汇总视图.ID, 收款汇总视图.单据编号, 收款汇总视图.日期, 单位表.单位编号,单位表.单位名称, 收款汇总视图.价税合计, 收款汇总视图.未付款金额, 收款汇总视图.备注 FROM 单位表 INNER JOIN 收款汇总视图 ON 单位表.ID = 收款汇总视图.单位ID WHERE (收款汇总视图.未付款金额 > 0) AND (收款汇总视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (收款汇总视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (收款汇总视图.BeActive = 1)";
            sqlComm.CommandText = "SELECT 收款汇总视图.单据编号, 收款汇总视图.日期, 单位表.单位编号, 单位表.单位名称,职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 收款汇总视图.价税合计, 收款汇总视图.备注,收款汇总视图.未付款金额,收款汇总视图.已付款金额 FROM 收款汇总视图 INNER JOIN 单位表 ON 收款汇总视图.单位ID = 单位表.ID INNER JOIN 职员表 ON 收款汇总视图.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 收款汇总视图.操作员ID = 操作员.ID WHERE (收款汇总视图.BeActive = 1) AND (收款汇总视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (收款汇总视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (收款汇总视图.未付款金额 <> 0)";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (intUserLimit <= LIMITACCESS1)
            {
                sqlComm.CommandText += " AND ((收款汇总视图.业务员ID = " + intUserID.ToString() + ") OR (收款汇总视图.操作员ID = " + intUserID.ToString() + "))";
            }
            else
            {
                if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                    sqlComm.CommandText += " AND 收款汇总视图.业务员ID = " + comboBoxYWY.SelectedValue.ToString();
            }

            if (dSet.Tables.Contains("商品表2")) dSet.Tables.Remove("商品表2");
            sqlDA.Fill(dSet, "商品表2");


            //结算汇总
            //sqlComm.CommandText = "SELECT 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 销售商品制单表.物流名称, 销售商品制单表.单号, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 销售商品制单表.价税合计, 销售商品制单表.备注 FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售商品制单表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 销售商品制单表.操作员ID = 操作员.ID WHERE (销售商品制单表.BeActive = 1) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.已付款金额 > 0)";
            sqlComm.CommandText = "SELECT 收款汇总视图.单据编号, 收款汇总视图.日期, 单位表.单位编号, 单位表.单位名称,职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 收款汇总视图.价税合计, 收款汇总视图.备注,收款汇总视图.未付款金额,收款汇总视图.已付款金额 FROM 收款汇总视图 INNER JOIN 单位表 ON 收款汇总视图.单位ID = 单位表.ID INNER JOIN 职员表 ON 收款汇总视图.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 收款汇总视图.操作员ID = 操作员.ID WHERE (收款汇总视图.BeActive = 1) AND (收款汇总视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (收款汇总视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (收款汇总视图.已付款金额 <> 0)";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";
            //if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
            //    sqlComm.CommandText += " AND 职员表.ID = " + comboBoxYWY.SelectedValue.ToString();
            if (intUserLimit <= LIMITACCESS1)
            {
                sqlComm.CommandText += " AND ((收款汇总视图.业务员ID = " + intUserID.ToString() + ") OR (收款汇总视图.操作员ID = " + intUserID.ToString() + "))";
            }
            else
            {
                if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                    sqlComm.CommandText += " AND 收款汇总视图.业务员ID = " + comboBoxYWY.SelectedValue.ToString();
            }


            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");


            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["商品表1"];
            //dataGridView1.Columns[0].Visible = false;
            dataGridView2.DataSource = dSet.Tables["商品表2"];
            //dataGridView2.Columns[0].Visible = false;
            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);
        }

        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= cTemp.Length; i++)
            {
                cTemp[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 6;
                        break;
                    case 2:
                        c = 6;
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


            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";
            strT = "销售业务查询（" + tabControl1.SelectedTab.Text + "）;当前日期：" + labelZDRQ.Text;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";
            strT = "销售业务查询（" + tabControl1.SelectedTab.Text + "）;当前日期：" + labelZDRQ.Text;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;
            if (!dSet.Tables.Contains("商品表" + c1.ToString()))
                return;
            toolStripStatusLabelC.Text = "共有" + dSet.Tables["商品表" + c1.ToString()].Rows.Count.ToString() + "条记录 金额合计" + cTemp[tabControl1.SelectedIndex].ToString("f2") + "元";
        }

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            iSupplyCompany = 0;
            textBoxDWMC.Text = "";
            textBoxDWBH.Text = "";
        }
    }
}