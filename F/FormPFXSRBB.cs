using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormPFXSRBB : Form
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

        public FormPFXSRBB()
        {
            InitializeComponent();
        }

        private void FormPFXSRBB_Load(object sender, EventArgs e)
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

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            string strA = "", strB = "", strC = "", strD = "", strE="";

            strA = "SELECT 销售商品制单表.日期, SUM(销售商品制单明细表.实计金额) AS 销售金额, SUM(销售商品制单明细表.库存成本价 * 销售商品制单明细表.数量) AS 基本出库成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID  WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.BeActive=1)";
            strA += " GROUP BY 销售商品制单表.日期";

            strB = "SELECT 销售退出汇总表.日期, SUM(销售退出明细表.实计金额) AS 退回金额, SUM(销售退出明细表.库存成本价 * 销售退出明细表.数量) AS 退回成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID  WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退出汇总表.BeActive=1)";
            strB += " GROUP BY 销售退出汇总表.日期";

            strC = "SELECT 销售退补差价汇总表.日期, SUM(销售退补差价明细表.金额) AS 销售退补差价金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID  WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退补差价汇总表.BeActive=1)";
            strC += " GROUP BY 销售退补差价汇总表.日期";


            strD = "SELECT SUM(已付款金额) AS 结算金额, 日期 FROM 收款汇总视图  WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            strD += " GROUP BY 日期";

            strE = "SELECT DISTINCT A.日期 FROM ((SELECT 日期 FROM 收款汇总视图 WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ) UNION (SELECT 日期 FROM 销售商品制单表 WHERE (日期 >= CONVERT (DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))) UNION (SELECT 日期 FROM 销售退出汇总表 WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))) UNION (SELECT 日期 FROM 销售退补差价汇总表 WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)))) A ";
            
            sqlConn.Open();
            sqlComm.CommandText = "SELECT CONVERT(varchar(100), 日期表.日期, 23) AS 日期, 出库表.销售金额, 退回表.退回金额, 销售退补差价表.销售退补差价金额,0.0 AS 出库金额, 结算表.结算金额, 0.0 AS 当天应收款, 0.0 AS 毛利, 0.0 AS [毛利率(%)], 0.0 AS 出库成本, 出库表.基本出库成本, 退回表.退回成本 FROM (" + strE + ") 日期表 LEFT OUTER JOIN (" + strA + ") 出库表 ON 日期表.日期 = 出库表.日期 LEFT OUTER JOIN (" + strB + ") 退回表 ON 日期表.日期 = 退回表.日期 LEFT OUTER JOIN (" + strC + ") 销售退补差价表 ON 日期表.日期 = 销售退补差价表.日期 LEFT OUTER JOIN (" + strD + ") 结算表 ON 日期表.日期 = 结算表.日期 ORDER BY 日期表.日期 DESC";


            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            sqlConn.Close();

            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            //dataGridViewDJMX.Columns[10].Visible = false;
            //dataGridViewDJMX.Columns[11].Visible = false;

            dataGridViewDJMX.Columns[1].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";

            dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            int i,j;
            for (i = 0; i < dataGridViewDJMX.ColumnCount; i++)
            {
                dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            
            decimal[] oTemp = new decimal[11];
            for (i = 0; i < oTemp.Length; i++)
                oTemp[i] = 0;

            for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                for (j = 0; j < oTemp.Length; j++)
                {
                    oTemp[j] += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[j+1].Value.ToString());
                }
            }
            if (oTemp[3] == 0)
                oTemp[7] = 0;
            else
                oTemp[7] = oTemp[6] / oTemp[3] * 100;
            object[] oT = new object[12];
            for (j = 0; j < oTemp.Length; j++)
            {
                oT[j + 1] = oTemp[j];
            }
            dSet.Tables["商品表"].Rows.Add(oT);

            dataGridViewDJMX.Rows[dataGridViewDJMX.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Gray;


            toolStripStatusLabelMXJLS.Text = dSet.Tables["商品表"].Rows.Count.ToString();
        }

        private void adjustDataView()
        {
            int i, j;
            TimeSpan ts;
            decimal[] dSUM = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0 };
            decimal dt = 0;

            //商品表

            for (i = 0; i < dSUM.Length; i++)
            {
                dSUM[i] = 0;
            }
            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {

                for (j = 1; j <= 11; j++)
                {
                    if (dSet.Tables["商品表"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品表"].Rows[i][j] = 0;
                }
                
                dSet.Tables["商品表"].Rows[i][4] = decimal.Parse(dSet.Tables["商品表"].Rows[i][1].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][2].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString());//销售

                dSet.Tables["商品表"].Rows[i][6] = decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][5].ToString());//当天应收

                dSet.Tables["商品表"].Rows[i][9] = decimal.Parse(dSet.Tables["商品表"].Rows[i][10].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][11].ToString());//出库成本

                dSet.Tables["商品表"].Rows[i][7] = decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][9].ToString());

                if (decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString()) == 0 || decimal.Parse(dSet.Tables["商品表"].Rows[i][7].ToString()) <= 0)
                    dSet.Tables["商品表"].Rows[i][8] = 0;
                else
                    dSet.Tables["商品表"].Rows[i][8]=decimal.Parse(dSet.Tables["商品表"].Rows[i][7].ToString())/decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString())*100;

                for (j = 1; j <= 11; j++)
                {
                    dSUM[j - 1] += decimal.Parse(dSet.Tables["商品表"].Rows[i][j].ToString());
                }


            }
            /*
            DataRow drT3 = dSet.Tables["商品表"].NewRow();
            drT3[0] = "合计";
            for (j = 1; j <= 11; j++)
            {
                drT3[j] = dSUM[j - 1];
            }

            drT3[4] = decimal.Parse(drT3[1].ToString()) - decimal.Parse(drT3[2].ToString()) - decimal.Parse(drT3[3].ToString());//销售

           drT3[6] = decimal.Parse(drT3[4].ToString()) - decimal.Parse(drT3[5].ToString());//当天应收
           drT3[9] = decimal.Parse(drT3[10].ToString()) + decimal.Parse(drT3[11].ToString());//出库成本
           drT3[7] = decimal.Parse(drT3[4].ToString()) - decimal.Parse(drT3[9].ToString());
           if (decimal.Parse(drT3[4].ToString()) == 0 || decimal.Parse(drT3[7].ToString()) <= 0)
               drT3[8] = 0;
           else
               drT3[8] = decimal.Parse(drT3[7].ToString()) / decimal.Parse(drT3[4].ToString()) * 100;

            dSet.Tables["商品表"].Rows.Add(drT3);
             * */

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "批发销售日报表;当前日期：" + labelZDRQ.Text ;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "批发销售日报表;当前日期：" + labelZDRQ.Text ;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }



    }
}