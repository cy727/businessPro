using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormGJFKCX : Form
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

        public FormGJFKCX()
        {
            InitializeComponent();
        }

        private void FormGJFKCX_Load(object sender, EventArgs e)
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
            sqlConn.Open();

            sqlComm.CommandText = "SELECT 付款汇总视图.ID, 付款汇总视图.单据编号, 付款汇总视图.日期, 单位表.单位编号,单位表.单位名称, 付款汇总视图.价税合计, 付款汇总视图.未付款金额, 付款汇总视图.备注 FROM 单位表 INNER JOIN 付款汇总视图 ON 单位表.ID = 付款汇总视图.单位ID WHERE (付款汇总视图.未付款金额 <> 0) AND (付款汇总视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (付款汇总视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (付款汇总视图.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表2")) dSet.Tables.Remove("商品表2");
            sqlDA.Fill(dSet, "商品表2");


            sqlComm.CommandText = "SELECT 付款汇总视图.单据编号, 付款汇总视图.日期, 单位表.单位编号, 单位表.单位名称,职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 付款汇总视图.价税合计, 付款汇总视图.未付款金额, 付款汇总视图.备注 FROM 付款汇总视图 INNER JOIN 单位表 ON 付款汇总视图.单位ID = 单位表.ID INNER JOIN 职员表 ON 付款汇总视图.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 付款汇总视图.操作员ID = 操作员.ID WHERE (付款汇总视图.BeActive = 1) AND (付款汇总视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (付款汇总视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (付款汇总视图.已付款金额 <> 0)";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");


            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["商品表1"];
            //dataGridView1.Columns[0].Visible = false;
            dataGridView2.DataSource = dSet.Tables["商品表2"];
            dataGridView2.Columns[0].Visible = false;
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
                        c = 5;
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
            strT = "购进付款查询（" + tabControl1.SelectedTab.Text + "）;当前日期：" + labelZDRQ.Text;
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
            strT = "购进付款查询（" + tabControl1.SelectedTab.Text + "）;当前日期：" + labelZDRQ.Text;
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

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            iSupplyCompany = 0;
            textBoxDWMC.Text = "";
            textBoxDWBH.Text = "";
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;
            if (!dSet.Tables.Contains("商品表" + c1.ToString()))
                return;
            toolStripStatusLabelC.Text = "共有" + dSet.Tables["商品表" + c1.ToString()].Rows.Count.ToString() + "条记录 金额合计" + cTemp[tabControl1.SelectedIndex].ToString("f2") + "元";
        }

    }
}