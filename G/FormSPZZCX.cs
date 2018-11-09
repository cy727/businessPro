using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPZZCX : Form
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

        public FormSPZZCX()
        {
            InitializeComponent();
        }

        private void FormSPZZCX_Load(object sender, EventArgs e)
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

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 库存商品组装汇总表.单据编号, 库存商品组装汇总表.日期, 库存商品组装汇总表.商品数量, 操作员.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员 FROM 库存商品组装汇总表 INNER JOIN 库房表 ON 库存商品组装汇总表.成品库房ID = 库房表.ID INNER JOIN 商品表 ON 库存商品组装汇总表.商品ID = 商品表.ID INNER JOIN 职员表 操作员 ON 库存商品组装汇总表.操作员ID = 操作员.ID INNER JOIN 职员表 [职员表_1] ON 库存商品组装汇总表.业务员ID = [职员表_1].ID WHERE (库存商品组装汇总表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (库存商品组装汇总表.日期 <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102))";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");

            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 库存商品组装汇总表.单据编号, 库存商品组装汇总表.日期, 库房表.库房编号, 库房表.库房名称, 库存商品组装明细表.组件数量, 库存商品组装明细表.成本单价, 库存商品组装明细表.成本金额 FROM 库存商品组装汇总表 INNER JOIN 库存商品组装明细表 ON 库存商品组装汇总表.ID = 库存商品组装明细表.单据ID INNER JOIN 商品表 ON 库存商品组装明细表.组件ID = 商品表.ID INNER JOIN 库房表 ON 库存商品组装明细表.库房ID = 库房表.ID WHERE (库存商品组装汇总表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (库存商品组装汇总表.日期 <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102))";

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
                    strT = "商品组装查询（组装成品查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "商品组装查询（零件组成查询）;当前日期：" + labelZDRQ.Text;
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
                    strT = "商品组装查询（组装成品查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "商品组装查询（零件组成查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
            }
        }

    }
}