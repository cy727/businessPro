using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJWCX : Form
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

        public int iSupplyCompany = 0;
        public int intCommID = 0;

        private decimal[] cTemp = new decimal[6] { 0, 0, 0, 0, 0, 0 };
        private decimal[] cTemp1 = new decimal[6] { 0, 0, 0, 0, 0, 0 };
        
        public FormJWCX()
        {
            InitializeComponent();
        }

        private void FormJWCX_Load(object sender, EventArgs e)
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
                if (cGetInformation.getCompanyInformation(1200, textBoxDWBH.Text.Trim()) == 0)
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

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0)
            {
                //return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPMC.Text = cGetInformation.strCommName;
                textBoxSPBH.Text = cGetInformation.strCommCode;
            }
        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0)
                {
                    //return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                }
            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0)
                {
                    //return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            if(intCommID==0)
                sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 借物出库汇总表.价税合计 AS 金额合计, 借物出库汇总表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID WHERE (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 >= 0) AND (借物出库汇总表.BeActive = 1) AND ((借物出库汇总表.冲抵单号ID <> -1) OR (借物出库汇总表.冲抵单号ID IS NULL))";
            else
                sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 借物出库汇总表.价税合计 AS 金额合计, 借物出库汇总表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID INNER JOIN 借物出库明细表 ON 借物出库汇总表.ID = 借物出库明细表.表单ID AND (借物出库明细表.商品ID = " + intCommID + ") AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 >= 0) AND (借物出库汇总表.BeActive = 1) AND ((借物出库汇总表.冲抵单号ID <> -1) OR (借物出库汇总表.冲抵单号ID IS NULL))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " ORDER BY 借物出库汇总表.日期 DESC";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");

            if (intCommID == 0)
                sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 借物出库汇总表.价税合计 AS 金额合计, 借物出库汇总表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID WHERE (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.冲抵单号ID IS NULL) AND (借物出库汇总表.出库金额 < 0) AND (借物出库汇总表.BeActive = 1)";                else
                sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 借物出库汇总表.价税合计 AS 金额合计, 借物出库汇总表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID INNER JOIN 借物出库明细表 ON 借物出库汇总表.ID = 借物出库明细表.表单ID AND (借物出库明细表.商品ID = " + intCommID + ") AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (借物出库汇总表.冲抵单号ID IS NULL) AND (借物出库汇总表.出库金额 < 0) AND (借物出库汇总表.BeActive = 1)";


            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " ORDER BY 借物出库汇总表.日期 DESC";

            if (dSet.Tables.Contains("商品表2")) dSet.Tables.Remove("商品表2");
            sqlDA.Fill(dSet, "商品表2");

            if (intCommID == 0)
                sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 借物出库汇总表.价税合计 AS 金额合计, 借物出库汇总表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID WHERE (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.冲抵单号ID IS NULL) AND (借物出库汇总表.出库金额 >= 0) AND (借物出库汇总表.BeActive = 1)";
            else
                sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 借物出库汇总表.价税合计 AS 金额合计, 借物出库汇总表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID INNER JOIN 借物出库明细表 ON 借物出库汇总表.ID = 借物出库明细表.表单ID AND (借物出库明细表.商品ID = " + intCommID + ") AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (借物出库汇总表.冲抵单号ID IS NULL) AND (借物出库汇总表.出库金额 >= 0) AND (借物出库汇总表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " ORDER BY 借物出库汇总表.日期 DESC";

            if (dSet.Tables.Contains("商品表3")) dSet.Tables.Remove("商品表3");
            sqlDA.Fill(dSet, "商品表3");

            if (intCommID == 0)
                sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 借物出库汇总表.价税合计 AS 金额合计, 借物出库汇总表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID WHERE (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.冲抵单号ID =-1) AND (借物出库汇总表.BeActive = 1)";
            else
                sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 借物出库汇总表.价税合计 AS 金额合计, 借物出库汇总表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID INNER JOIN 借物出库明细表 ON 借物出库汇总表.ID = 借物出库明细表.表单ID AND (借物出库明细表.商品ID = " + intCommID + ") AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))  AND (借物出库汇总表.冲抵单号ID =-1) AND (借物出库汇总表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " ORDER BY 借物出库汇总表.日期 DESC";

            if (dSet.Tables.Contains("商品表4")) dSet.Tables.Remove("商品表4");
            sqlDA.Fill(dSet, "商品表4");


            if (intCommID == 0)
                sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 借物出库汇总表.价税合计 AS 金额合计, 借物出库汇总表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID WHERE (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 < 0) AND (借物出库汇总表.BeActive = 1) AND ((借物出库汇总表.冲抵单号ID <> -1) OR (借物出库汇总表.冲抵单号ID IS NULL))";
            else
                sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 借物出库汇总表.价税合计 AS 金额合计, 借物出库汇总表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID INNER JOIN 借物出库明细表 ON 借物出库汇总表.ID = 借物出库明细表.表单ID AND (借物出库明细表.商品ID = " + intCommID + ") AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 < 0) AND (借物出库汇总表.BeActive = 1) AND ((借物出库汇总表.冲抵单号ID <> -1) OR (借物出库汇总表.冲抵单号ID IS NULL))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " ORDER BY 借物出库汇总表.日期 DESC";

            if (dSet.Tables.Contains("商品表5")) dSet.Tables.Remove("商品表5");
            sqlDA.Fill(dSet, "商品表5");

            if (intCommID == 0)
                sqlComm.CommandText = "SELECT 单位表.单位名称, SUM(借物出库汇总表.价税合计) AS 金额合计,SUM(借物出库汇总表.出库金额) AS 出库金额合计 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID WHERE (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.冲抵单号ID IS NULL) AND (借物出库汇总表.BeActive = 1)";
            else
                sqlComm.CommandText = "SELECT 单位表.单位名称, SUM(借物出库明细表.金额) AS 金额合计, SUM(借物出库明细表.出库金额) AS 出库金额合计 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 借物出库明细表 ON 借物出库汇总表.ID = 借物出库明细表.表单ID WHERE (借物出库汇总表.冲抵单号ID IS NULL) AND (借物出库明细表.商品ID = " + intCommID + ") AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.BeActive = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";
            sqlComm.CommandText += " GROUP BY 单位表.单位名称 ORDER BY 金额合计 DESC";

            if (dSet.Tables.Contains("商品表6")) dSet.Tables.Remove("商品表6");
            sqlDA.Fill(dSet, "商品表6");


            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["商品表1"];
            dataGridView2.DataSource = dSet.Tables["商品表2"];
            dataGridView3.DataSource = dSet.Tables["商品表3"];
            dataGridView4.DataSource = dSet.Tables["商品表4"];
            dataGridView5.DataSource = dSet.Tables["商品表5"];
            dataGridView6.DataSource = dSet.Tables["商品表6"];

            dataGridView1.Columns[0].Visible = false;
            dataGridView2.Columns[0].Visible = false;
            dataGridView3.Columns[0].Visible = false;
            dataGridView4.Columns[0].Visible = false;
            dataGridView5.Columns[0].Visible = false;

            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);
            
        }

        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= 6; i++)
            {
                cTemp[i - 1] = 0;
                cTemp1[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 5;
                        c1 = 6;
                        break;
                    case 2:
                        c = 5;
                        c1 = 6;
                        break;
                    case 3:
                        c = 5;
                        c1 = 6;
                        break;
                    case 4:
                        c = 5;
                        c1 = 6;
                        break;
                    case 5:
                        c = 5;
                        c1 = 6;
                        break;
                    case 6:
                        c = 1;
                        c1 = 2;
                        break;
                    default:
                        c = 0;
                        c1 = 0;
                        break;
                }

                if (c != 0)
                {

                    for (j = 0; j < dSet.Tables["商品表" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp[i - 1] += decimal.Parse(dSet.Tables["商品表" + i.ToString()].Rows[j][c].ToString());
                            cTemp1[i - 1] += decimal.Parse(dSet.Tables["商品表" + i.ToString()].Rows[j][c1].ToString());
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

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "借物查询（借物出库汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "借物查询（借入未充抵查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "借物查询（借出未充抵查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "借物查询（充抵查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, true, intUserLimit);
                    break;
                case 4:
                    strT = "借物查询（借物入库汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, true, intUserLimit);
                    break;
                case 5:
                    strT = "借物查询（未充抵借物出入库单位汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, true, intUserLimit);
                    break;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "借物查询（借物出库汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "借物查询（借入未充抵查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "借物查询（借出未充抵查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "借物查询（充抵查询）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, false, intUserLimit);
                    break;
                case 4:
                    strT = "借物查询（借物入库汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, false, intUserLimit);
                    break;
                case 5:
                    strT = "借物查询（未充抵借物出入库单位汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, false, intUserLimit);
                    break;
            }

        }

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            iSupplyCompany = 0;
            textBoxDWBH.Text = "";
            textBoxDWMC.Text = "";
        }

        private void toolStripButtonASP_Click(object sender, EventArgs e)
        {
            intCommID = 0;
            textBoxSPBH.Text = "";
            textBoxSPMC.Text = "";
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;

            if (!dSet.Tables.Contains("商品表" + c1.ToString()))
                return;


            //toolStripStatusLabelC.Text = "共有" + dSet.Tables["商品表" + c1.ToString()].Rows.Count.ToString() + "条记录 金额合计" + cTemp[tabControl1.SelectedIndex].ToString("f2") + "元 出库金额合计" + cTemp1[tabControl1.SelectedIndex].ToString("f2") + "元";
            toolStripStatusLabelC.Text = "共有" + dSet.Tables["商品表" + c1.ToString()].Rows.Count.ToString() + "条记录 出库金额合计" + cTemp1[tabControl1.SelectedIndex].ToString("f2") + "元";

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridView dv = (DataGridView)sender; ;

            if (dv.SelectedRows.Count < 1)
                return;

            if (dv.SelectedRows[0].Cells[0].Value.ToString() == "")
                return;

            // 创建此子窗体的一个新实例。
            FormKCJWCKDJ childFormKCJWCKDJ = new FormKCJWCKDJ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKCJWCKDJ.MdiParent = this.MdiParent; 

            childFormKCJWCKDJ.strConn = strConn;

            childFormKCJWCKDJ.intUserID = intUserID;
            childFormKCJWCKDJ.intUserLimit = intUserLimit;
            childFormKCJWCKDJ.strUserLimit = strUserLimit;
            childFormKCJWCKDJ.strUserName = strUserName;
            childFormKCJWCKDJ.isSaved = true;
            childFormKCJWCKDJ.iDJID = int.Parse(dv.SelectedRows[0].Cells[0].Value.ToString());
            childFormKCJWCKDJ.Show();

        }


    }
}