using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPXSHKFX : Form
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

        private int intClassID = 0;
        private ClassGetInformation cGetInformation;

        public FormSPXSHKFX()
        {
            InitializeComponent();
        }

        private void FormSPXSHKFX_Load(object sender, EventArgs e)
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
        }

        private void textBoxSPLB_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getClassInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intClassID = cGetInformation.iClassNumber;
                textBoxSPLB.Text = cGetInformation.strClassName;
                checkBoxAll.Checked = false;

            }
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAll.Checked)
            {
                intClassID = 0;
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品表.库存数量, 商品表.库存数量*商品表.库存成本价 AS 库存金额, 付款表.销售金额, 付款表.回款金额, 付款表.应收余额 FROM 商品表 LEFT OUTER JOIN (SELECT 商品ID, SUM(实计金额) AS 销售金额, SUM(已付款金额) AS 回款金额, SUM(未付款金额) AS 应收余额 FROM 收款明细视图 WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) GROUP BY 商品ID) 付款表 ON 商品表.ID = 付款表.商品ID WHERE (商品表.beactive = 1)";

            if (intClassID != 0)
            {
                sqlComm.CommandText += " AND (商品表.分类编号 = " + intClassID.ToString() + ")";
            }

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            sqlConn.Close();
            
            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f2";

            toolStripStatusLabelMXJLS.Text = (dataGridViewDJMX.RowCount-1).ToString();
        }

        private void adjustDataView()
        {
            int i;
            decimal dT1 = 0, dT2 = 0, dT3 = 0, dT4 = 0, dT5 = 0;

            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表"].Rows[i][4].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][4] = 0;
                if (dSet.Tables["商品表"].Rows[i][5].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][5] = 0;
                if (dSet.Tables["商品表"].Rows[i][6].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][6] = 0;

                dT1 += decimal.Parse(dSet.Tables["商品表"].Rows[i][2].ToString());
                dT2 += decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString());
                dT3 += decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString());
                dT4 += decimal.Parse(dSet.Tables["商品表"].Rows[i][5].ToString());
                dT5 += decimal.Parse(dSet.Tables["商品表"].Rows[i][6].ToString());
            }

            object[] oTemp = new object[7];
            oTemp[0] = "合计";
            oTemp[1] = "";
            oTemp[2] = dT1;
            oTemp[3] = dT2;
            oTemp[4] = dT3;
            oTemp[5] = dT4;
            oTemp[6] = dT5;
            dSet.Tables["商品表"].Rows.Add(oTemp);
        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            bool bMX = true;
            int i, j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[5];
            decimal[] dSum1 = new decimal[5];

            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;

            if (MessageBox.Show("是否包含明细？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }

            sqlConn.Open();
            //sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品表.库存数量, 商品表.库存金额,商品表.结转金额 AS 可销天数, 商品表.结转金额 AS 占库存比重, 商品表.结转金额 AS 占压资金, 商品表.结转金额 AS 平均库存, 商品表.结转金额 AS 日均出库数量, 商品表.结转金额 AS 日均销售数量, 商品表.结转金额 AS 周转天数, 商品表.结转金额 AS 年周转次数, 商品表.分类编号, 结算表.出库数量, 结算表.销售数量, 结算表.应付金额, 结算表.应收金额, 总库存.库存数量 AS Expr1, 总库存.库存金额 AS Expr2, 结算表.总结存数量, 结算表.笔数 FROM 商品表 LEFT OUTER JOIN (SELECT SUM(出库数量) AS 出库数量, SUM(销售数量) AS 销售数量, SUM(应付金额) AS 应付金额, SUM(应收金额) AS 应收金额, 商品ID, SUM(总结存数量) AS 总结存数量, COUNT(*) AS 笔数 FROM 商品历史账表 WHERE (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) GROUP BY 商品ID) 结算表 ON 商品表.ID = 结算表.商品ID CROSS JOIN (SELECT SUM(库存数量) AS 库存数量, SUM(库存金额) AS 库存金额 FROM 商品表) 总库存 WHERE (商品表.beactive = 1)";
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品表.库存数量, 商品表.库存金额, 付款表.销售金额, 付款表.回款金额, 付款表.应收余额,  商品表.分类编号 FROM 商品表 LEFT OUTER JOIN (SELECT 商品ID, SUM(实计金额) AS 销售金额, SUM(已付款金额) AS 回款金额, SUM(未付款金额) AS 应收余额 FROM 收款明细视图 WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) GROUP BY 商品ID) 付款表 ON 商品表.ID = 付款表.商品ID WHERE (商品表.beactive = 1)";



            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            sqlConn.Close();

            adjustDataView();

            sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 ORDER BY 上级分类";
            if (dSet.Tables.Contains("商品分类表")) dSet.Tables.Remove("商品分类表");
            sqlDA.Fill(dSet, "商品分类表");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("商品编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("商品名称", System.Type.GetType("System.String"));
            dTable.Columns.Add("库存数量", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("库存金额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("销售金额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("回款金额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("应收余额", System.Type.GetType("System.Decimal"));

            DataRow[] dtC = dSet.Tables["商品分类表"].Select("上级分类 = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[7];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 2; t < oTemp.Length; t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["商品分类表"].Select("上级分类 = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[7];
                    oTemp1[0] = dtC1[j][1];
                    oTemp1[1] = "　　" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    //oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;
                    for (t = 2; t < oTemp1.Length; t++)
                        oTemp1[t] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["商品表"].Select("分类编号 = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 2].ToString());


                        if (bMX)
                        {
                            object[] oTemp2 = new object[7];
                            for (t = 0; t < oTemp2.Length; t++)
                                oTemp2[t] = dtC2[k][t];
                            oTemp2[1] = "　　　　" + dtC2[k][1];

                            dTable.Rows.Add(oTemp2);
                        }
                    }

                    for (t = 2; t < dSum.Length+2; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length+2; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }

            object[] oTemp3 = new object[7];
            oTemp3[0] = "合计";
            oTemp3[1] = "";
            for (t = 2; t < oTemp3.Length; t++)
                oTemp3[t] = dSum1[t - 2];
            dTable.Rows.Add(oTemp3);


            dataGridViewDJMX.DataSource = dTable;
            toolStripStatusLabelMXJLS.Text = dSet.Tables["商品表"].Rows.Count.ToString();
 
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品销售回款分析;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品销售回款分析;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void textBoxSPLB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getClassInformation(10, textBoxSPLB.Text) == 0) //失败
                {
                    textBoxSPLB.Text = "";
                    intClassID = 0;
                    checkBoxAll.Checked = true;

                }
                else
                {
                    intClassID = cGetInformation.iClassNumber;
                    textBoxSPLB.Text = cGetInformation.strClassName;
                    checkBoxAll.Checked = false;
                }
            }
        }


    }
}