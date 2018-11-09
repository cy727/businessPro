using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPXSPHFX : Form
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

        private int intCommID = 0;
        private int intKFID = 0;


        private ClassGetInformation cGetInformation;

        public FormSPXSPHFX()
        {
            InitializeComponent();
        }

        private void FormSPXSPHFX_Load(object sender, EventArgs e)
        {
            this.Top = 1;
            this.Left = 1;
            comboBoxOrder.SelectedIndex = 0;

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
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, SUM(销售视图.数量) AS 销售数量, SUM(销售视图.实计金额) AS 销售金额, SUM(销售视图.数量 * 销售视图.库存成本价) AS 出库成本,  SUM(销售视图.实计金额) - SUM(销售视图.数量 * 销售视图.库存成本价) AS 毛利, 0.00 AS [毛利率(%)] FROM 销售视图 INNER JOIN 商品表 ON 销售视图.商品ID = 商品表.ID WHERE (销售视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售视图.BeActive = 1) ";
            if (!checkBoxKF.Checked && intKFID != 0)
            {
                sqlComm.CommandText += " AND 销售视图.库房ID = "+intKFID.ToString();
            }
            if (!checkBoxSP.Checked && intCommID!= 0)
            {
                sqlComm.CommandText += " AND 销售视图.商品ID = " + intCommID.ToString();
            }
            sqlComm.CommandText += " GROUP BY 商品表.商品名称, 商品表.商品编号, 商品表.商品规格";


            switch (comboBoxOrder.SelectedIndex)
            {
                case 0:
                    sqlComm.CommandText += " ORDER BY 销售数量 DESC";
                    break;
                case 1:
                    sqlComm.CommandText += " ORDER BY 销售金额 DESC";
                    break;
                case 2:
                    sqlComm.CommandText += " ORDER BY 毛利 DESC";
                    break;
            }
            sqlComm.CommandText += " , 商品表.商品编号";


            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            sqlConn.Close();

            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            for (int i = 0; i < dataGridViewDJMX.ColumnCount; i++)
            {
                dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            decimal dTemp1=0, dTemp2=0, dTemp3=0,dTemp4=0;
            for (int i = 0; i < dataGridViewDJMX.RowCount; i++)
            {
                try
                {
                    dTemp1 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[3].Value.ToString());
                    dTemp2 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[4].Value.ToString());
                    dTemp3 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString());
                    dTemp4 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString());
                }
                catch
                {
                }

            }
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString() + " 数量：" + dTemp1.ToString("f0") + " 金额：" + dTemp2.ToString("f2") + " 成本：" + dTemp3.ToString("f2") + " 毛利：" + dTemp4.ToString("f2"); 
        }

        private void adjustDataView()
        {
            int i,j;

            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                for (j = 3; j < dSet.Tables["商品表"].Columns.Count; j++)
                {
                    if (dSet.Tables["商品表"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品表"].Rows[i][j] = 0;
                }

                //dSet.Tables["商品表"].Rows[i][5] = decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString());
                if (Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][5].ToString()) == 0)
                    dSet.Tables["商品表"].Rows[i][7] = 0;
                else
                    dSet.Tables["商品表"].Rows[i][7] = Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][6].ToString()) / Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][5].ToString())*100;

            }

        }

        private void adjustDataView1()
        {
            int i, j;

            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                for (j = 2; j < dSet.Tables["商品表"].Columns.Count; j++)
                {
                    if (dSet.Tables["商品表"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品表"].Rows[i][j] = 0;
                }

                //dSet.Tables["商品表"].Rows[i][5] = decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString());
                if (Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][4].ToString()) == 0)
                    dSet.Tables["商品表"].Rows[i][6] = 0;
                else
                    dSet.Tables["商品表"].Rows[i][6] = Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][5].ToString()) / Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][4].ToString()) * 100;

            }

        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            bool bMX = true;
            int i, j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[6];
            decimal[] dSum1 = new decimal[6];
            string strA;


            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;

            if (MessageBox.Show("是否包含明细？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }

            strA = "SELECT 商品表.ID AS 商品ID, SUM(销售视图.数量) AS 销售数量, SUM(销售视图.实计金额) AS 销售金额, SUM(销售视图.数量 * 销售视图.库存成本价) AS 出库成本, SUM(销售视图.实计金额) - SUM(销售视图.数量 * 销售视图.库存成本价) AS 毛利, 0.00 AS 毛利率 FROM 销售视图 INNER JOIN 商品表 ON 销售视图.商品ID = 商品表.ID WHERE (销售视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售视图.BeActive = 1) ";
            if (!checkBoxKF.Checked && intKFID != 0)
            {
                strA += " AND 销售视图.库房ID = " + intKFID.ToString();
            }
            if (!checkBoxSP.Checked && intCommID != 0)
            {
                strA += " AND 销售视图.商品ID = " + intCommID.ToString();
            }
            strA += " GROUP BY 商品表.ID";

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 销售表.销售数量, 销售表.销售金额, 销售表.出库成本, 销售表.毛利,销售表.毛利率 AS [毛利率(%)], 商品表.分类编号 FROM 商品表 LEFT OUTER JOIN ("+strA+") 销售表 ON 商品表.ID = 销售表.商品ID WHERE (商品表.beactive = 1)";
            switch (comboBoxOrder.SelectedIndex)
            {
                case 0:
                    sqlComm.CommandText += " ORDER BY 销售表.销售数量 DESC";
                    break;
                case 1:
                    sqlComm.CommandText += " ORDER BY 销售表.销售金额 DESC";
                    break;
                case 2:
                    sqlComm.CommandText += " ORDER BY 销售表.毛利 DESC";
                    break;
            }

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
            dTable.Columns.Add("销售数量", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("销售金额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("出库成本", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("毛利", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("毛利率(%)", System.Type.GetType("System.Decimal"));


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
                    {
                        dSum[t] = 0;
                    }
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

                    for (t = 2; t < dSum.Length; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length; t++)
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

            for (i = 0; i <dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].Cells[3].Value.ToString() == "")
                    continue;
                if (dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() == "")
                    continue;

                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value.ToString()) == 0)
                {
                    dataGridViewDJMX.Rows[i].Cells[6].Value = 0;
                    continue;
                }
                dataGridViewDJMX.Rows[i].Cells[6].Value = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString()) / Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value.ToString()) * 100;

            }
            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            for (i = 0; i < dataGridViewDJMX.ColumnCount; i++)
            {
                dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            decimal dTemp1 = 0, dTemp2 = 0, dTemp3 = 0;
            for (i = 0; i < dataGridViewDJMX.RowCount; i++)
            {
                try
                {
                    dTemp1 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[2].Value.ToString());
                    dTemp2 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[3].Value.ToString());
                }
                catch
                {
                }

            }
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString() + " 数量：" + dTemp1.ToString("f0") + " 金额：" + dTemp2.ToString("f2");

 
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品销售排行分析;当前日期：" + labelZDRQ.Text ;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品销售排行分析;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(10, "") == 0)
            {
                //return;
            }
            else
            {
                intKFID = cGetInformation.iKFNumber;
                textBoxKFMC.Text = cGetInformation.strKFName;
                textBoxKFBH.Text = cGetInformation.strKFCode;
            }
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(10, textBoxKFBH.Text) == 0)
                {
                    //return;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                }
            }
        }

        private void textBoxKFMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFMC.Text) == 0)
                {
                    //return;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
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

    }
}