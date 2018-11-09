using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPKCJGFX : Form
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

        private int intDays = 0;
        private int intMonths = 0;


        private string sDT = "";
        private ClassGetInformation cGetInformation;

        private int intCommID = 0;
        decimal dSumJE = 0;


        public FormSPKCJGFX()
        {
            InitializeComponent();
        }

        private void FormSPKCJGFX_Load(object sender, EventArgs e)
        {
            int i;

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
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-1-1");

            }
            sqldr.Close();
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i;
            //得到结束时间
            string sDTE = Convert.ToDateTime(sDT).Year.ToString() + "-" + Convert.ToDateTime(sDT).Month.ToString() + "-1";

            TimeSpan dtTemp = Convert.ToDateTime(sDTE) - dateTimePickerS.Value;
            intDays = dtTemp.Days;
            
            intMonths = (Convert.ToDateTime(sDTE).Year - dateTimePickerS.Value.Year) * 12 + (Convert.ToDateTime(sDTE).Month - dateTimePickerS.Value.Month); //得到月数

            //intDays--;
            if (intMonths <= 0)
            {
                MessageBox.Show("请调整开始时间到一个月以上");
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT SUM(库存金额) AS 总计 FROM 商品表 WHERE (beactive = 1)";
            sqldr = sqlComm.ExecuteReader();

            sqldr.Read();
            dSumJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());//库存金额合计
            sqldr.Close();


            if (checkBoxALLSP.Checked || intCommID == 0)
                sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品表.库存数量, 商品表.库存金额, 0.0 AS 可销月数, 商品表.库存金额 AS [占库存比重(%)], 0.0 AS 日均出库数量, 销量表.销量 FROM 商品表 LEFT OUTER JOIN (SELECT SUM(销售商品制单明细表.数量) AS 销量, 销售商品制单明细表.商品ID FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.BeActive = 1) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + sDTE + " 00:00:00', 102)) GROUP BY 销售商品制单明细表.商品ID) 销量表 ON 商品表.ID = 销量表.商品ID WHERE (商品表.beactive = 1)";
            else
                sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品表.库存数量, 商品表.库存金额, 0.0 AS 可销月数, 商品表.库存金额 AS [占库存比重(%)], 0.0 AS 日均出库数量, 销量表.销量 FROM 商品表 INNER JOIN (SELECT SUM(销售商品制单明细表.数量) AS 销量, 销售商品制单明细表.商品ID FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.BeActive = 1) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + sDTE + " 00:00:00', 102)) GROUP BY 销售商品制单明细表.商品ID) 销量表 ON 商品表.ID = 销量表.商品ID WHERE (商品表.beactive = 1) AND (商品表.ID=" + intCommID.ToString() + ") ";


            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            sqlConn.Close();

            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString();

            dataGridViewDJMX.Columns[7].Visible=false;
            dataGridViewDJMX.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f1";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f3";

            
            
        }

        private void adjustDataView()
        {
            int i;
            decimal dTemp;

            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表"].Rows[i][2].ToString() == "0") //数量
                    dSet.Tables["商品表"].Rows[i][3] = 0;

                if (dSet.Tables["商品表"].Rows[i][7].ToString() == "") //销量
                    dSet.Tables["商品表"].Rows[i][7]=0;

                if(dSet.Tables["商品表"].Rows[i][7].ToString()!="0")
                    dSet.Tables["商品表"].Rows[i][4]=(Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][2].ToString())*Convert.ToDecimal(intMonths)/Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][7].ToString()));
                else
                    dSet.Tables["商品表"].Rows[i][4]=0;

                if(dSumJE!=0)
                    dSet.Tables["商品表"].Rows[i][5]=Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][3].ToString())/dSumJE*100;
                else
                    dSet.Tables["商品表"].Rows[i][5]=0;


                if (intDays != 0)
                    //dSet.Tables["商品表"].Rows[i][6]=(Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][7])/Convert.ToDecimal(intDays)); 调整一月22天
                    dSet.Tables["商品表"].Rows[i][6] = (Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][7]) / Convert.ToDecimal(intDays))*30/22;
                else
                    dSet.Tables["商品表"].Rows[i][6] = 0;



            }
        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            bool bMX = true;
            int i, j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[10];
            decimal[] dSum1 = new decimal[10];

            for(t=0;t<dSum1.Length;t++)
                dSum1[t] = 0; 

            if (Convert.ToDateTime(sDT).AddDays(-1) < dateTimePickerS.Value)
            {
                MessageBox.Show("开始时间错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("是否包含明细？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }
            TimeSpan dtTemp = Convert.ToDateTime(sDT) - dateTimePickerS.Value;
            intDays = dtTemp.Days;//得到天数

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品表.库存数量, 商品表.库存金额,商品表.结转金额 AS 可销天数, 商品表.结转金额 AS 占库存比重, 商品表.结转金额 AS 占压资金, 商品表.结转金额 AS 平均库存, 商品表.结转金额 AS 日均出库数量, 商品表.结转金额 AS 日均销售数量, 商品表.结转金额 AS 周转天数, 商品表.结转金额 AS 年周转次数, 商品表.分类编号, 结算表.出库数量, 结算表.销售数量, 结算表.应付金额, 结算表.应收金额, 总库存.库存数量 AS Expr1, 总库存.库存金额 AS Expr2, 结算表.总结存数量, 结算表.笔数 FROM 商品表 LEFT OUTER JOIN (SELECT SUM(出库数量) AS 出库数量, SUM(销售数量) AS 销售数量, SUM(应付金额) AS 应付金额, SUM(应收金额) AS 应收金额, 商品ID, SUM(总结存数量) AS 总结存数量, COUNT(*) AS 笔数 FROM 商品历史账表 WHERE (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) GROUP BY 商品ID) 结算表 ON 商品表.ID = 结算表.商品ID CROSS JOIN (SELECT SUM(库存数量) AS 库存数量, SUM(库存金额) AS 库存金额 FROM 商品表) 总库存 WHERE (商品表.beactive = 1)";


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
            dTable.Columns.Add("库存金额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("库存数量", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("可销天数", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("占库存比重", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("占压资金", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("平均库存", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("日均出库数量", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("日均销售数量", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("周转天数", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("年周转次数", System.Type.GetType("System.Int32"));

            DataRow[] dtC = dSet.Tables["商品分类表"].Select("上级分类 = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[12];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 2; t < oTemp.Length;t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["商品分类表"].Select("上级分类 = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[12];
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
                            dSum[t] += Convert.ToDecimal(dtC2[k][t+2].ToString()); 


                        if (bMX)
                        {
                            object[] oTemp2 = new object[12];
                            for (t = 0; t < oTemp2.Length; t++)
                                oTemp2[t] = dtC2[k][t];
                            oTemp2[1] = "　　　　" + dtC2[k][1];

                            dTable.Rows.Add(oTemp2);
                        }
                    }

                    for (t = 2; t < dSum.Length; t++)
                        dTable.Rows[iRow1][t] = dSum[t-2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }

            object[] oTemp3 = new object[12];
            oTemp3[0] = "合计";
            oTemp3[1] = "";
            for (t = 2; t < oTemp3.Length; t++)
                oTemp3[t] = dSum1[t-2];
            dTable.Rows.Add(oTemp3);


            dataGridViewDJMX.DataSource = dTable;
 

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品库存结构分析;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品库存结构分析;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
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
    }
}