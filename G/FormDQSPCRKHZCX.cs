using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormDQSPCRKHZCX : Form
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

        private int iJZID = 0;
        private int intCommID = 0;

        private string SDTS0 = "", SDTS1 = "";

        private ClassGetInformation cGetInformation;
        
        public FormDQSPCRKHZCX()
        {
            InitializeComponent();
        }

        private void FormDQSPCRKHZCX_Load(object sender, EventArgs e)
        {
            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            intCommID = 0;
            textBoxSPMC.Text = "";
            textBoxSPBH.Text = "";

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            //得到上次结转
            //得到开始时间
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                SDTS0 = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT 结算时间,ID FROM 结转汇总表 ORDER BY 结算时间 DESC";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                iJZID = Convert.ToInt32(sqldr.GetValue(1).ToString());
                SDTS1 = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
            }
            if (iJZID == 0)
                SDTS1 = SDTS0;

            sqlConn.Close();

        }

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0)
            {
                intCommID = 0;
                textBoxSPMC.Text = "";
                textBoxSPBH.Text = "";
                //return;
            }
            else
            {
                intCommID = 0;
                textBoxSPMC.Text = "";
                textBoxSPBH.Text = "";

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
                    intCommID = 0;
                    textBoxSPMC.Text = "";
                    textBoxSPBH.Text = "";

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
                    intCommID = 0;
                    textBoxSPMC.Text = "";
                    textBoxSPBH.Text = "";

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
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 结转.结转数量, 结转.结转金额, 商品表.库存数量 AS 结存数量, 商品表.库存金额 AS 结存金额, 出入库.入库数量, 出入库.入库金额, 出入库.出库数量, 出入库.出库金额 FROM 商品表 LEFT OUTER JOIN (SELECT SUM(入库数量) AS 入库数量, SUM(入库金额) AS 入库金额, SUM(出库数量) AS 出库数量, SUM(出库金额) AS 出库金额, 商品ID FROM 商品历史账表 WHERE (日期 > CONVERT(DATETIME, '" + SDTS1 + " 00:00:00', 102)) GROUP BY 商品ID) 出入库 ON 商品表.ID = 出入库.商品ID LEFT OUTER JOIN (SELECT 结转数量, 结转金额, 商品ID FROM 结转进销存汇总表 WHERE (ID = " + iJZID .ToString()+ ")) 结转 ON 商品表.ID = 结转.商品ID WHERE (商品表.beactive = 1)";

            if (intCommID != 0)
                sqlComm.CommandText += " AND (商品表.ID = " + intCommID.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");

            sqlConn.Close();
            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
        }

        private void adjustDataView()
        {

            decimal dTemp0 = 0, dTemp1 = 0, dTemp2 = 0, dTemp3 = 0;
            
            for (int i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                for (int j = 2; j < 10; j++)
                {
                    if (dSet.Tables["商品表"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品表"].Rows[i][j] = 0;
                }

                dTemp0 += Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][7]);
                dTemp1 += Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][9]);
                dTemp2 += Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][5]);
                dTemp3 += Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][3]);

            }
            labelRKJEHJ.Text = dTemp0.ToString("f2");
            labelCKJEHJ.Text = dTemp1.ToString("f0");
            labelJCJEHJ.Text = dTemp2.ToString("f0");
            labelJZJEHJ.Text = dTemp3.ToString("f0");
            toolStripStatusLabelMXJLS.Text=dSet.Tables["商品表"].Rows.Count.ToString();


        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            int i;
            bool bMX = true;

            if (MessageBox.Show("是否包含明细？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 结转.结转数量, 结转.结转金额, 商品表.库存数量 AS 结存数量, 商品表.库存金额 AS 结存金额, 出入库.入库数量, 出入库.入库金额, 出入库.出库数量, 出入库.出库金额, 商品表.分类编号 FROM 商品表 LEFT OUTER JOIN (SELECT SUM(入库数量) AS 入库数量, SUM(入库金额) AS 入库金额, SUM(出库数量) AS 出库数量, SUM(出库金额) AS 出库金额, 商品ID FROM 商品历史账表 WHERE (日期 > CONVERT(DATETIME, '" + SDTS1 + " 00:00:00', 102)) GROUP BY 商品ID) 出入库 ON 商品表.ID = 出入库.商品ID LEFT OUTER JOIN (SELECT 结转数量, 结转金额, 商品ID FROM 结转进销存汇总表 WHERE (ID = " + iJZID.ToString() + ")) 结转 ON 商品表.ID = 结转.商品ID WHERE (商品表.beactive = 1)";


            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");

            sqlConn.Close();
            adjustDataView();

            int j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[8];
            decimal[] dSum1 = new decimal[8];

            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;


            sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 ORDER BY 上级分类";
            if (dSet.Tables.Contains("商品分类表")) dSet.Tables.Remove("商品分类表");
            sqlDA.Fill(dSet, "商品分类表");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("商品编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("商品名称", System.Type.GetType("System.String"));
            dTable.Columns.Add("结转数量", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("结转金额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("结存数量", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("结存金额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("入库数量", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("入库金额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("出库数量", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("出库金额", System.Type.GetType("System.Decimal"));



            DataRow[] dtC = dSet.Tables["商品分类表"].Select("上级分类 = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[10];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 2; t < oTemp.Length; t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["商品分类表"].Select("上级分类 = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[10];
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
                        {
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 2].ToString());
                        }


                        if (bMX)
                        {
                            object[] oTemp2 = new object[10];
                            for (t = 0; t < oTemp2.Length; t++)
                                oTemp2[t] = dtC2[k][t];
                            oTemp2[1] = "　　　　" + dtC2[k][1];

                            dTable.Rows.Add(oTemp2);
                        }
                    }

                    for (t = 2; t < dSum.Length + 2; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length + 2; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }



            dataGridViewDJMX.DataSource = dTable;
 
        }

       private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "当期商品初入库汇总表;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "当期商品初入库汇总表;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

    }
}