using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormYJXSCX : Form
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
        private int intClassID = 0;
        private ClassGetInformation cGetInformation;
        

        public FormYJXSCX()
        {
            InitializeComponent();
        }

        private void FormYJXSCX_Load(object sender, EventArgs e)
        {
            this.Left = 1;
            this.Top = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            numericUpDownND.Value = Convert.ToDateTime(strDT).Year;

            numericUpDownQSYF.Maximum = decimal.Parse(Convert.ToDateTime(strDT).Month.ToString());
        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            int i,j;
            bool bMX = true;

            int iM = 12;
            int iM1 = 12;

            if (System.DateTime.Now.Year == (int)numericUpDownND.Value)
            {
                if (checkBoxDY.Checked) //当月统计
                {
                    iM = System.DateTime.Now.Month;
                }
                else //不统计当月
                {
                    iM = System.DateTime.Now.Month - 1;
                }

                iM1 = iM - int.Parse(numericUpDownQSYF.Value.ToString()) + 1;
                if (iM1 == 0)
                    iM1 = iM;

                
            }

            if (iM == 0 || System.DateTime.Now.Year < (int)numericUpDownND.Value)
            {
                MessageBox.Show("没有统计数据。");
                return;
            }



            if (MessageBox.Show("是否包含明细？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }

            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                cGetInformation.getUnderClassInformation(intClassID);
            }

            sqlConn.Open();
            /*
            sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品名称,商品表.商品规格, ";
            for(i=1;i<13;i++)
            {
                if (!checkBoxDY.Checked && System.DateTime.Now.Year == (int)numericUpDownND.Value && i == System.DateTime.Now.Month) //本年度不统计当月
                {
                    sqlComm.CommandText += "0 AS '" + i.ToString() + "月',";
                }
                else
                {
                    sqlComm.CommandText += "SUM(CASE WHEN MONTH(销售出库汇总表.日期)="+i.ToString()+" THEN 销售出库明细表.数量 END) AS '" + i.ToString() + "月',";
                }
            }

            sqlComm.CommandText += "SUM(销售出库明细表.数量) AS 合计, 商品表.分类编号 FROM 销售出库明细表 INNER JOIN  销售出库汇总表 ON 销售出库明细表.单据ID = 销售出库汇总表.ID INNER JOIN 商品表 ON 销售出库明细表.商品ID = 商品表.ID WHERE (销售出库汇总表.BeActive = 1)  AND YEAR(销售出库汇总表.日期)=" + numericUpDownND.Value.ToString("f0");
            if (!checkBoxDY.Checked && System.DateTime.Now.Year == (int)numericUpDownND.Value) //本年度不统计当月
            {
                sqlComm.CommandText += " AND MONTH(销售出库汇总表.日期)<>"+System.DateTime.Now.Month.ToString();
            }
            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                sqlComm.CommandText += " AND (商品表.ID = "+intCommID.ToString()+")" ;
            }
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY 商品表.ID, 商品表.商品名称,商品表.商品规格,商品表.分类编号 ";
             */

            sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品名称,商品表.商品规格, ";
            for (i = 1; i < 13; i++)
            {
                if (!checkBoxDY.Checked && System.DateTime.Now.Year == (int)numericUpDownND.Value && i == System.DateTime.Now.Month) //本年度不统计当月
                {
                    sqlComm.CommandText += "0 AS '" + i.ToString() + "月',";
                }
                else
                {
                    sqlComm.CommandText += "SUM(CASE WHEN MONTH(销售视图.日期)=" + i.ToString() + " THEN 销售视图.数量 END) AS '" + i.ToString() + "月',";
                }
            }

            sqlComm.CommandText += "SUM(销售视图.数量) AS 合计, 商品表.分类编号 FROM 销售视图 INNER JOIN 商品表 ON 销售视图.商品ID = 商品表.ID WHERE (销售视图.BeActive = 1)  AND YEAR(销售视图.日期)=" + numericUpDownND.Value.ToString("f0");
            if (!checkBoxDY.Checked && System.DateTime.Now.Year == (int)numericUpDownND.Value) //本年度不统计当月
            {
                sqlComm.CommandText += " AND MONTH(销售视图.日期)<>" + System.DateTime.Now.Month.ToString();
            }
            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                sqlComm.CommandText += " AND (商品表.ID = " + intCommID.ToString() + ")";
            }
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY 商品表.ID, 商品表.商品名称,商品表.商品规格,商品表.分类编号 ";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");

            sqlConn.Close();


            decimal dTemp = 0, dTemp1 = 0; ;
            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                for (j = 3; j < 16; j++)
                {
                    if (dSet.Tables["商品表"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品表"].Rows[i][j] = 0;
                }

                dTemp += Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][15]);
            }
            labelHJ.Text = dTemp.ToString("f0");
            toolStripStatusLabelMXJLS.Text = dSet.Tables["商品表"].Rows.Count.ToString("");

            int k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[13];
            decimal[] dSum1 = new decimal[13];

            int[] iR = new int[150];
            int[] iR1 = new int[150];
            int iRC = 0, iRC1 = 0;

            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;


            sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 WHERE (BeActive = 1)";
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                if (cGetInformation.intUpClassNumber == 0)
                {
                    sqlComm.CommandText += " AND ((ID = " + intClassID.ToString() + ")";
                    for (i = 0; i < cGetInformation.intUnderClassNumber; i++) 
                        sqlComm.CommandText += " OR (ID = " + cGetInformation.intUnderClass[i].ToString() + ")";
                    sqlComm.CommandText += ") ";
                }
                else
                {
                    sqlComm.CommandText += " AND ((ID = " + cGetInformation.intUpClassNumber.ToString() + ")";
                    sqlComm.CommandText += " OR (ID = " + intClassID.ToString() + ")";
                    sqlComm.CommandText += ") ";

                }
            }
            sqlComm.CommandText += " ORDER BY 上级分类";
            if (dSet.Tables.Contains("商品分类表")) dSet.Tables.Remove("商品分类表");
            sqlDA.Fill(dSet, "商品分类表");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("商品名称", System.Type.GetType("System.String"));
            dTable.Columns.Add("商品规格", System.Type.GetType("System.String"));
            dTable.Columns.Add("1月", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("2月", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("3月", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("4月", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("5月", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("6月", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("7月", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("8月", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("9月", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("10月", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("11月", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("12月", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("合计", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("月均", System.Type.GetType("System.Decimal"));


            DataRow[] dtC = dSet.Tables["商品分类表"].Select("上级分类 = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[16];
                oTemp[0] = dtC[i][2];
                oTemp[1] = "";

                for (t = 2; t < oTemp.Length; t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                //上级分类行
                iRow0 = dTable.Rows.Count - 1;
                iR[iRC] = iRow0;
                iRC++;

                DataRow[] dtC1 = dSet.Tables["商品分类表"].Select("上级分类 = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[16];
                    oTemp1[0] = dtC1[j][2];
                    oTemp1[1] = "";
                    
                    //oTemp1[8] = dtC1[j][0];
                    //oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;
                    for (t = 2; t < oTemp1.Length; t++)
                        oTemp1[t] = 0;

                    dTable.Rows.Add(oTemp1);
                    //二级分类行
                    iRow1 = dTable.Rows.Count - 1;
                    iR1[iRC1] = iRow1;
                    iRC1++;

                    DataRow[] dtC2 = dSet.Tables["商品表"].Select("分类编号 = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                        {
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 3].ToString());
                        }


                        if (bMX)
                        {
                            object[] oTemp2 = new object[16];
                            for (t = 0; t < oTemp2.Length; t++)
                                oTemp2[t] = dtC2[k][t+1];
                            oTemp2[0] = "　　　　" + dtC2[k][1];
                            oTemp2[1] = dtC2[k][2];
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
            
            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[12].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[13].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[14].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[15].DefaultCellStyle.Format = "f1";
            for (i = 0; i < iRC; i++)
            {
                dataGridViewDJMX.Rows[iR[i]].DefaultCellStyle.BackColor = Color.DarkGray;
            }

            decimal fM = 0;
            for (i = 0; i < iRC1; i++)
            {
                dataGridViewDJMX.Rows[iR1[i]].DefaultCellStyle.BackColor = Color.Pink;
            }

            for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                fM = decimal.Parse(dataGridViewDJMX.Rows[i].Cells[14].Value.ToString())/(decimal)iM1;
                dataGridViewDJMX.Rows[i].Cells[15].Value = fM.ToString();
            }

            for (i = 2; i < iM+2; i++)
            {
                dataGridViewDJMX.Columns[i].Visible = true;
            }

            for (i = iM + 2; i < 14; i++)
            {
                dataGridViewDJMX.Columns[i].Visible = false;
            }

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
                checkBoxALL.Checked = false;

            }
        }

        private void textBoxSPLB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getClassInformation(10, textBoxSPLB.Text) == 0) //失败
                {
                    textBoxSPLB.Text = "";
                    intClassID = 0;
                    checkBoxALL.Checked = true;

                }
                else
                {
                    intClassID = cGetInformation.iClassNumber;
                    textBoxSPLB.Text = cGetInformation.strClassName;
                    checkBoxALL.Checked = false;
                }
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "月均销售;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "月均销售;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }
    }
}
