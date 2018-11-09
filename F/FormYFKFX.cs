using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormYFKFX : Form
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

        private decimal[] cTemp = new decimal[4] { 0, 0, 0, 0 };


        private int intCommID = 0;
        private int iCompanyID = 0;

        public FormYFKFX()
        {
            InitializeComponent();
        }

        private void FormYFKFX_Load(object sender, EventArgs e)
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
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            comboBoxJE.SelectedIndex = 0;
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            string strA = "";
            string strB = "";
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 付款明细视图.日期, 付款明细视图.单据编号, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, 商品表.商品编号, 商品表.商品名称, 付款明细视图.未付款金额 AS 应付金额 FROM 付款明细视图 INNER JOIN 商品表 ON 付款明细视图.商品ID = 商品表.ID INNER JOIN 职员表 ON 付款明细视图.业务员ID = 职员表.ID INNER JOIN 单位表 ON 付款明细视图.单位ID = 单位表.ID WHERE (付款明细视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (付款明细视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (未付款金额 <> 0) ";

            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                sqlComm.CommandText += " AND (付款明细视图.单位ID = " + iCompanyID.ToString() + ") ";
            }

            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                sqlComm.CommandText += " AND (付款明细视图.商品ID = " + intCommID.ToString() + ") ";
            }

            switch (comboBoxJE.SelectedIndex)
            {
                case 1:
                    sqlComm.CommandText += " AND (付款明细视图.未付款金额 > 0) ";
                    break;
                case 2:
                    sqlComm.CommandText += " AND (付款明细视图.未付款金额 < 0) ";
                    break;

            }

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");

            strB = "SELECT SUM(未付款金额) AS 应付金额 FROM 付款明细视图 WHERE (未付款金额 <> 0)  AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (单位ID = " + iCompanyID.ToString() + ") ";
            }
            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                strB += " AND (商品ID = " + intCommID.ToString() + ") ";
            }
            switch (comboBoxJE.SelectedIndex)
            {
                case 1:
                    strB += " AND (未付款金额 > 0) ";
                    break;
                case 2:
                    strB += " AND (未付款金额 < 0) ";
                    break;

            }
            strA = "SELECT 单位ID, SUM(未付款金额) AS 应付金额 FROM 付款明细视图 WHERE (未付款金额 <> 0)  AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (单位ID = " + iCompanyID.ToString() + ") ";
            }
            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                strA += " AND (商品ID = " + intCommID.ToString() + ") ";
            }
            switch (comboBoxJE.SelectedIndex)
            {
                case 1:
                    strA += " AND (未付款金额 > 0) ";
                    break;
                case 2:
                    strA += " AND (未付款金额 < 0) ";
                    break;
            }

            strA += " GROUP BY 单位ID";

            sqlComm.CommandText = "SELECT 单位表.单位编号, 单位表.单位名称, 应付款.应付金额 AS 应付金额, 应付款.应付金额 AS [所占比重(%)], 总应付款.应付金额 AS 总应付款 FROM 单位表 INNER JOIN (" + strA + ") 应付款 ON 单位表.ID = 应付款.单位ID CROSS JOIN (" + strB + ") 总应付款";

            if (dSet.Tables.Contains("商品表2")) dSet.Tables.Remove("商品表2");
            sqlDA.Fill(dSet, "商品表2");

            strA = "SELECT 业务员ID, SUM(未付款金额) AS 应付金额 FROM 付款明细视图 WHERE (未付款金额 <> 0)  AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (单位ID = " + iCompanyID.ToString() + ") ";
            }
            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                strA += " AND (商品ID = " + intCommID.ToString() + ") ";
            }
            switch (comboBoxJE.SelectedIndex)
            {
                case 1:
                    strA += " AND (未付款金额 > 0) ";
                    break;
                case 2:
                    strA += " AND (未付款金额 < 0) ";
                    break;
            }

            strA += " GROUP BY 业务员ID";

            sqlComm.CommandText = "SELECT 职员表.职员编号, 职员表.职员姓名, 应付款.应付金额 AS 应付金额, 应付款.应付金额 AS [所占比重(%)], 总应付金额.应付金额 FROM 职员表 INNER JOIN (" + strA + ") 应付款 ON 职员表.ID = 应付款.业务员ID CROSS JOIN (" + strB + ") 总应付金额 WHERE (职员表.beactive = 1)";

            if (dSet.Tables.Contains("商品表3")) dSet.Tables.Remove("商品表3");
            sqlDA.Fill(dSet, "商品表3");

            strA = "SELECT 商品ID, SUM(未付款金额) AS 应付金额 FROM 付款明细视图 WHERE (未付款金额 <> 0)  AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (单位ID = " + iCompanyID.ToString() + ") ";
            }
            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                strA += " AND (商品ID = " + intCommID.ToString() + ") ";
            }
            switch (comboBoxJE.SelectedIndex)
            {
                case 1:
                    strA += " AND (未付款金额 > 0) ";
                    break;
                case 2:
                    strA += " AND (未付款金额 < 0) ";
                    break;
            }

            strA += " GROUP BY 商品ID";

            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 应付金额.应付金额 AS 应付金额, 应付金额.应付金额 AS [所占比重(%)], 总应付金额.应付金额 FROM 商品表 INNER JOIN (" + strA + ") 应付金额 ON 商品表.ID = 应付金额.商品ID CROSS JOIN (" + strB + ") 总应付金额 WHERE (商品表.beactive = 1)";

            if (dSet.Tables.Contains("商品表4")) dSet.Tables.Remove("商品表4");
            sqlDA.Fill(dSet, "商品表4");


            sqlConn.Close();

            adjustDataView();
            dataGridViewYSKMX.DataSource = dSet.Tables["商品表1"];
            dataGridViewDW.DataSource = dSet.Tables["商品表2"];
            dataGridViewDW.Columns[4].Visible = false;
            dataGridViewYWY.DataSource = dSet.Tables["商品表3"];
            dataGridViewYWY.Columns[4].Visible = false;
            dataGridViewSP.DataSource = dSet.Tables["商品表4"];
            dataGridViewSP.Columns[4].Visible = false;

            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);
        }

        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= 4; i++)
            {
                cTemp[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 7;
                        break;
                    case 2:
                        c = 2;
                        break;
                    case 3:
                        c = 2;
                        break;
                    case 4:
                        c = 2;
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

        private void adjustDataView()
        {
            int i;


            for (i = 0; i < dSet.Tables["商品表1"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表1"].Rows[i][7].ToString() == "")
                    dSet.Tables["商品表1"].Rows[i][7] = 0;
            }

            for (i = 0; i < dSet.Tables["商品表2"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表2"].Rows[i][4].ToString() == "")
                    dSet.Tables["商品表2"].Rows[i][4] = 0;
                if (dSet.Tables["商品表2"].Rows[i][2].ToString() == "")
                    dSet.Tables["商品表2"].Rows[i][2] = 0;
                if (Convert.ToDecimal(dSet.Tables["商品表2"].Rows[i][4].ToString()) == 0)
                    dSet.Tables["商品表2"].Rows[i][3] = 0;
                else
                    dSet.Tables["商品表2"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["商品表2"].Rows[i][2].ToString()) / Convert.ToDecimal(dSet.Tables["商品表2"].Rows[i][4].ToString()) * 100;

                dSet.Tables["商品表2"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["商品表2"].Rows[i][3]).ToString("f2");
            }

            for (i = 0; i < dSet.Tables["商品表3"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表3"].Rows[i][4].ToString() == "")
                    dSet.Tables["商品表3"].Rows[i][4] = 0;
                if (dSet.Tables["商品表3"].Rows[i][2].ToString() == "")
                    dSet.Tables["商品表3"].Rows[i][2] = 0;
                if (Convert.ToDecimal(dSet.Tables["商品表3"].Rows[i][4].ToString()) == 0)
                    dSet.Tables["商品表3"].Rows[i][3] = 0;
                else
                    dSet.Tables["商品表3"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["商品表3"].Rows[i][2].ToString()) / Convert.ToDecimal(dSet.Tables["商品表3"].Rows[i][4].ToString()) * 100;

                dSet.Tables["商品表3"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["商品表3"].Rows[i][3]).ToString("f2");
            }

            for (i = 0; i < dSet.Tables["商品表4"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表4"].Rows[i][4].ToString() == "")
                    dSet.Tables["商品表4"].Rows[i][4] = 0;
                if (dSet.Tables["商品表4"].Rows[i][2].ToString() == "")
                    dSet.Tables["商品表4"].Rows[i][2] = 0;
                if (Convert.ToDecimal(dSet.Tables["商品表4"].Rows[i][4].ToString()) == 0)
                    dSet.Tables["商品表4"].Rows[i][3] = 0;
                else
                    dSet.Tables["商品表4"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["商品表4"].Rows[i][2].ToString()) / Convert.ToDecimal(dSet.Tables["商品表4"].Rows[i][4].ToString()) * 100;
                dSet.Tables["商品表4"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["商品表4"].Rows[i][3]).ToString("f2");
            }


        }

        private void adjustDataView1()
        {
            int i;

            for (i = 0; i < dSet.Tables["商品表4"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表4"].Rows[i][4].ToString() == "")
                    dSet.Tables["商品表4"].Rows[i][4] = 0;
                if (dSet.Tables["商品表4"].Rows[i][2].ToString() == "")
                    dSet.Tables["商品表4"].Rows[i][2] = 0;
                if (Convert.ToDecimal(dSet.Tables["商品表4"].Rows[i][4].ToString()) == 0)
                    dSet.Tables["商品表4"].Rows[i][3] = 0;
                else
                    dSet.Tables["商品表"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["商品表4"].Rows[i][2].ToString()) / Convert.ToDecimal(dSet.Tables["商品表4"].Rows[i][4].ToString()) * 100;

                dSet.Tables["商品表4"].Rows[i][3] = Convert.ToDecimal(dSet.Tables["商品表4"].Rows[i][3]).ToString("f2");
            }

        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            bool bMX = true;
            int i, j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[2];
            decimal[] dSum1 = new decimal[2];

            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;

            if (MessageBox.Show("是否包含明细？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 应付金额.应付金额, 应付金额.应付金额 AS 所占比重, 总应付金额.总应付金额, 商品表.分类编号 FROM 商品表 LEFT OUTER JOIN (SELECT SUM(销售商品制单明细表.未付款金额) AS 应付金额, 销售商品制单明细表.商品ID FROM 进货入库汇总表 INNER JOIN 销售商品制单明细表 ON 进货入库汇总表.ID = 销售商品制单明细表.表单ID WHERE (进货入库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.日期 <= CONVERT (DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.BeActive = 1) GROUP BY 销售商品制单明细表.商品ID) 应付金额 ON 商品表.ID = 应付金额.商品ID CROSS JOIN (SELECT SUM(销售商品制单明细表.未付款金额) AS 总应付金额 FROM 进货入库汇总表 INNER JOIN 销售商品制单明细表 ON 进货入库汇总表.ID = 销售商品制单明细表.表单ID WHERE (进货入库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (进货入库汇总表.BeActive = 1)) 总应付金额 WHERE (商品表.beactive = 1)";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            sqlConn.Close();

            adjustDataView1();

            sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 ORDER BY 上级分类";
            if (dSet.Tables.Contains("商品分类表")) dSet.Tables.Remove("商品分类表");
            sqlDA.Fill(dSet, "商品分类表");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("商品编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("商品名称", System.Type.GetType("System.String"));
            dTable.Columns.Add("应收金额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("所占比重", System.Type.GetType("System.Decimal"));


            DataRow[] dtC = dSet.Tables["商品分类表"].Select("上级分类 = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[4];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 2; t < oTemp.Length; t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["商品分类表"].Select("上级分类 = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[4];
                    oTemp1[0] = dtC1[j][1];
                    oTemp1[1] = "　　" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    //oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;
                    for (t = 2; t < oTemp1.Length; t++)
                        oTemp1[t] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["商品表4"].Select("分类编号 = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 2].ToString());


                        if (bMX)
                        {
                            object[] oTemp2 = new object[4];
                            for (t = 0; t < oTemp2.Length; t++)
                                oTemp2[t] = dtC2[k][t];
                            oTemp2[1] = "　　　　" + dtC2[k][1];

                            dTable.Rows.Add(oTemp2);
                        }
                    }

                    for (t = 2; t < dSum.Length; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }

            object[] oTemp3 = new object[4];
            oTemp3[0] = "合计";
            oTemp3[1] = "";
            for (t = 2; t < oTemp3.Length; t++)
                oTemp3[t] = dSum1[t - 2];
            dTable.Rows.Add(oTemp3);


            dataGridViewSP.DataSource = dTable;

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "应付款分析（应付款明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewYSKMX, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "应付款分析（单位应付款汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDW, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "应付款分析（业务员应付款汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewYWY, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "应付款分析（商品应付款汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewSP, strT, true, intUserLimit);
                    break;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "应付款分析（应付款明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewYSKMX, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "应付款分析（单位应付款汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewDW, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "应付款分析（业务员应付款汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewYWY, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "应付款分析（商品应付款汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewSP, strT, false, intUserLimit);
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
    }
}