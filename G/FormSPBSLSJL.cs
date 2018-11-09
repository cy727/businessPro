using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPBSLSJL : Form
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

        
        public FormSPBSLSJL()
        {
            InitializeComponent();
        }

        private void FormSPBSLSJL_Load(object sender, EventArgs e)
        {
            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

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

            sqlComm.CommandText = "SELECT 库存报损汇总表.单据编号, 库存报损汇总表.日期, 商品表.商品编号, 商品表.商品名称, 库存报损明细表.报损数量, 库存报损明细表.报损金额, 库房表.库房编号, 库房表.库房名称 FROM 库存报损明细表 INNER JOIN 库存报损汇总表 ON 库存报损明细表.单据ID = 库存报损汇总表.ID INNER JOIN 商品表 ON 库存报损明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 库存报损汇总表.库房ID = 库房表.ID WHERE (库存报损汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (库存报损汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (库存报损汇总表.BeActive=1)";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");

            sqlConn.Close();
            adjustDataView1();
            dataGridView1.DataSource = dSet.Tables["商品表1"];
            dataGridView1.Columns[4].DefaultCellStyle.Format = "f0";

        }

        private void adjustDataView1()
        {
            int i, j;
            decimal dTemp1 = 0, dTemp2 = 0; ;

            for (i = 0; i < dSet.Tables["商品表1"].Rows.Count; i++)
            {
                for (j = 4; j <6; j++)
                {
                    if (dSet.Tables["商品表1"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品表1"].Rows[i][j] = 0;
                }
                dTemp1 += Convert.ToDecimal(dSet.Tables["商品表1"].Rows[i][5]);
                dTemp2 += Convert.ToDecimal(dSet.Tables["商品表1"].Rows[i][4]);
            }
            labelBSJEHJ.Text = dTemp1.ToString("f2");
            labelSLHJ.Text = dTemp2.ToString("f0");
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

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 库存报损汇总表.单据编号, 库存报损汇总表.日期, 商品表.商品编号, 商品表.商品名称, 库存报损明细表.报损数量, 库存报损明细表.报损金额, 库房表.库房编号, 库房表.库房名称, 商品表.分类编号 FROM 库存报损明细表 INNER JOIN 库存报损汇总表 ON 库存报损明细表.单据ID = 库存报损汇总表.ID INNER JOIN 商品表 ON 库存报损明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 库存报损汇总表.库房ID = 库房表.ID WHERE (库存报损汇总表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (库存报损汇总表.日期 <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102))";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");
            sqlConn.Close();

            adjustDataView1();

            sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 ORDER BY 上级分类";
            if (dSet.Tables.Contains("商品分类表")) dSet.Tables.Remove("商品分类表");
            sqlDA.Fill(dSet, "商品分类表");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("单据编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("日期", System.Type.GetType("System.String"));
            dTable.Columns.Add("商品编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("商品名称", System.Type.GetType("System.String"));
            dTable.Columns.Add("报损数量", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("报损金额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("库房编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("库房名称", System.Type.GetType("System.String"));


            DataRow[] dtC = dSet.Tables["商品分类表"].Select("上级分类 = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[8];
                oTemp[2] = dtC[i][1];
                oTemp[3] = dtC[i][2];
                oTemp[0] = "";
                oTemp[1] = "";

                for (t = 4; t < oTemp.Length - 2; t++)
                    oTemp[t] = 0;



                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["商品分类表"].Select("上级分类 = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[8];
                    oTemp1[2] = dtC1[j][1];
                    oTemp1[3] = "　　" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    //oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;
                    for (t = 4; t < oTemp1.Length - 2; t++)
                        oTemp1[t] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["商品表1"].Select("分类编号 = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 4].ToString());

                        object[] oTemp2 = new object[8];
                        for (t = 0; t < oTemp2.Length; t++)
                            oTemp2[t] = dtC2[k][t];
                        oTemp2[3] = "　　　　" + dtC2[k][1];

                        dTable.Rows.Add(oTemp2);
                    }

                    for (t = 4; t < dSum.Length + 4; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 4];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 4; t < dSum.Length + 4; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }

            dataGridView1.DataSource = dTable;
            dataGridView1.Columns[4].DefaultCellStyle.Format = "f0";


        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品报损历史查询;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品报损历史查询;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
        }
    }
}