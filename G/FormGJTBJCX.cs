using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormGJTBJCX : Form
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

        public FormGJTBJCX()
        {
            InitializeComponent();
        }

        private void FormGJTBJCX_Load(object sender, EventArgs e)
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

        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1, "") == 0)
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
                if (cGetInformation.getCompanyInformation(10, textBoxDWBH.Text.Trim()) == 0)
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
                if (cGetInformation.getCompanyInformation(12, textBoxDWMC.Text.Trim()) == 0)
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
            int i;
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 购进退补差价汇总表.单据编号, 购进退补差价汇总表.日期, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 购进退补差价汇总表.价税合计 FROM 购进退补差价汇总表 INNER JOIN 单位表 ON 购进退补差价汇总表.单位ID = 单位表.ID INNER JOIN 职员表 职员表 ON 购进退补差价汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 购进退补差价汇总表.操作员ID = 操作员.ID WHERE (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (购进退补差价汇总表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");


            sqlComm.CommandText = "SELECT 购进退补差价汇总表.单据编号, 购进退补差价汇总表.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 购进退补差价明细表.补价数量, 购进退补差价明细表.差价, 购进退补差价明细表.金额 FROM 购进退补差价汇总表 INNER JOIN 单位表 ON 购进退补差价汇总表.单位ID = 单位表.ID INNER JOIN 购进退补差价明细表 ON 购进退补差价汇总表.ID = 购进退补差价明细表.单据ID INNER JOIN 商品表 ON 购进退补差价明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 购进退补差价明细表.库房ID = 库房表.ID WHERE (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (购进退补差价汇总表.BeActive = 1)  AND (购进退补差价明细表.未付款金额 <> 0)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表2")) dSet.Tables.Remove("商品表2");
            sqlDA.Fill(dSet, "商品表2");

            sqlComm.CommandText = "SELECT 购进退补差价汇总表.单据编号, 购进退补差价汇总表.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 购进退补差价明细表.补价数量, 购进退补差价明细表.差价, 购进退补差价明细表.金额 FROM 购进退补差价汇总表 INNER JOIN 单位表 ON 购进退补差价汇总表.单位ID = 单位表.ID INNER JOIN 购进退补差价明细表 ON 购进退补差价汇总表.ID = 购进退补差价明细表.单据ID INNER JOIN 商品表 ON 购进退补差价明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 购进退补差价明细表.库房ID = 库房表.ID WHERE (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (购进退补差价汇总表.BeActive = 1) AND (购进退补差价明细表.已付款金额 <> 0)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表3")) dSet.Tables.Remove("商品表3");
            sqlDA.Fill(dSet, "商品表3");

            sqlComm.CommandText = "SELECT 购进退补差价汇总表.单据编号, 购进退补差价汇总表.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 购进退补差价明细表.补价数量, 购进退补差价明细表.差价, 购进退补差价明细表.金额 FROM 购进退补差价汇总表 INNER JOIN 单位表 ON 购进退补差价汇总表.单位ID = 单位表.ID INNER JOIN 购进退补差价明细表 ON 购进退补差价汇总表.ID = 购进退补差价明细表.单据ID INNER JOIN 商品表 ON 购进退补差价明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 购进退补差价明细表.库房ID = 库房表.ID WHERE (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (购进退补差价汇总表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";
            if (dSet.Tables.Contains("商品表4")) dSet.Tables.Remove("商品表4");
            sqlDA.Fill(dSet, "商品表4");

            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["商品表1"];
            dataGridView2.DataSource = dSet.Tables["商品表2"];
            dataGridView3.DataSource = dSet.Tables["商品表3"];
            dataGridView4.DataSource = dSet.Tables["商品表4"];

            decimal dSUM;
            dSUM = 0;

            for (i = 0; i < dSet.Tables["商品表1"].Rows.Count; i++)
            {
                try
                {
                    dSUM += decimal.Parse(dSet.Tables["商品表1"].Rows[i][6].ToString());
                }
                catch
                {
                }
            }
            labelJEHJ.Text = dSUM.ToString("f2");
            tabControl1_SelectedIndexChanged(null, null);


        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "购进退补价查询（购进退补价汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "购进退补价查询（退补价未结算明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "购进退补价查询（退补价结算明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "购进退补价查询（购进退补价明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, true, intUserLimit);
                    break;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "购进退补价查询（购进退补价汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "购进退补价查询（退补价未结算明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "购进退补价查询（退补价结算明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "购进退补价查询（购进退补价明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, false, intUserLimit);
                    break;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabelMXJLS.Text = dSet.Tables["商品表" + (tabControl1.SelectedIndex + 1).ToString()].Rows.Count.ToString();
        }
    }
}