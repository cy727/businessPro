using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPTJCX : Form
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

        private ClassGetInformation cGetInformation;

        public FormSPTJCX()
        {
            InitializeComponent();
        }

        private void FormSPTJCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Left = 1;
            this.Top = 1;

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

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 调价通知单明细表.原进价, 调价通知单明细表.进价, 调价通知单明细表.原批发价, 调价通知单明细表.批发价 FROM 调价通知单明细表 INNER JOIN 商品表 ON 调价通知单明细表.商品ID = 商品表.ID INNER JOIN 调价通知单汇总表 ON 调价通知单明细表.单据ID = 调价通知单汇总表.ID WHERE (调价通知单汇总表.执行标记 = 0) AND (调价通知单汇总表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (调价通知单汇总表.日期 <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102))";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");

            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 调价通知单明细表.原进价, 调价通知单明细表.进价, 调价通知单明细表.原批发价, 调价通知单明细表.批发价 FROM 调价通知单明细表 INNER JOIN 商品表 ON 调价通知单明细表.商品ID = 商品表.ID INNER JOIN 调价通知单汇总表 ON 调价通知单明细表.单据ID = 调价通知单汇总表.ID WHERE (调价通知单汇总表.执行标记 = 1) AND (调价通知单汇总表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (调价通知单汇总表.日期 <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102))";

            if (dSet.Tables.Contains("商品表2")) dSet.Tables.Remove("商品表2");
            sqlDA.Fill(dSet, "商品表2");

            sqlConn.Close();

            dataGridView1.DataSource = dSet.Tables["商品表1"];
            dataGridView2.DataSource = dSet.Tables["商品表2"];
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "商品调价查询（为变动价格调整查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "商品调价查询（价格调整查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "商品调价查询（为变动价格调整查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "商品调价查询（价格调整查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
            }
        }


    }
}