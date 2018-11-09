using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPKCYZFX : Form
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
        private int intCommID = 0;

        private string strDT0="",strDT1="",strDT2="",strDT3="";
        private ClassGetInformation cGetInformation;

        public FormSPKCYZFX()
        {
            InitializeComponent();
        }

        private void FormSPKCYZFX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            string strTTemp = "";

            //得到开始时间
            sqlConn.Open();
            //sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
            sqlComm.CommandText = "SELECT GETDATE()";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
                //dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");
                dateTimePickerS.Value = DateTime.Parse(sqldr.GetValue(0).ToString()).AddDays(-30);

            }
            sqldr.Close();
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            strDT0 = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT0).ToString("yyyy年M月dd日");

            strDT0 = dateTimePickerE.Value.ToShortDateString();
            strTTemp = dateTimePickerS.Value.ToShortDateString();

            strDT1 = Convert.ToDateTime(strDT0).AddDays(-5).ToShortDateString();
            if (Convert.ToDateTime(strDT1) < Convert.ToDateTime(strTTemp))
                strDT1 = strTTemp;
            strDT2 = Convert.ToDateTime(strDT0).AddDays(-10).ToShortDateString();
            if (Convert.ToDateTime(strDT2) < Convert.ToDateTime(strTTemp))
                strDT2 = strTTemp;
            strDT3 = Convert.ToDateTime(strDT0).AddDays(-30).ToShortDateString();
            if (Convert.ToDateTime(strDT3) < Convert.ToDateTime(strTTemp))
                strDT3 = strTTemp;
            labelCZY.Text = strUserName;


        }

        private void checkBoxALL_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxALL.Checked)
            {
                intClassID = 0;
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

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i;
            string strTTemp = "";

            cGetInformation.getSystemDateTime();
            strDT0 = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT0).ToString("yyyy年M月dd日");

            strDT0 = dateTimePickerE.Value.ToShortDateString();
            strTTemp = dateTimePickerS.Value.ToShortDateString();

            strDT1 = Convert.ToDateTime(strDT0).AddDays(-5).ToShortDateString();
            if (Convert.ToDateTime(strDT1) < Convert.ToDateTime(strTTemp))
                strDT1 = strTTemp;
            strDT2 = Convert.ToDateTime(strDT0).AddDays(-10).ToShortDateString();
            if (Convert.ToDateTime(strDT2) < Convert.ToDateTime(strTTemp))
                strDT2 = strTTemp;
            strDT3 = Convert.ToDateTime(strDT0).AddDays(-30).ToShortDateString();
            if (Convert.ToDateTime(strDT3) < Convert.ToDateTime(strTTemp))
                strDT3 = strTTemp;


            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                cGetInformation.getUnderClassInformation(intClassID);
            }

            string strA = "SELECT SUM(购进商品制单明细表.数量) AS 数量, 购进商品制单明细表.商品ID, MIN(商品表.分类编号) AS 分类编号 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '"+strDT0+" 23:59:59', 102)) AND (购进商品制单表.日期 > CONVERT(DATETIME, '"+strDT1+" 23:59:59', 102)) AND (商品表.beactive = 1) ";
            strA +=" GROUP BY 购进商品制单明细表.商品ID";

            string strB = "SELECT SUM(购进商品制单明细表.数量) AS 数量, 购进商品制单明细表.商品ID, MIN(商品表.分类编号) AS 分类编号 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + strDT1 + " 23:59:59', 102)) AND (购进商品制单表.日期 > CONVERT(DATETIME, '" + strDT2 + " 23:59:59', 102)) AND (商品表.beactive = 1) ";
            strB += " GROUP BY 购进商品制单明细表.商品ID";

            string strC = "SELECT SUM(购进商品制单明细表.数量) AS 数量, 购进商品制单明细表.商品ID, MIN(商品表.分类编号) AS 分类编号 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + strDT2 + " 23:59:59', 102)) AND (购进商品制单表.日期 > CONVERT(DATETIME, '" + strDT3 + " 23:59:59', 102)) AND (商品表.beactive = 1) ";
            strC += " GROUP BY 购进商品制单明细表.商品ID";

            string strD = "SELECT SUM(购进商品制单明细表.数量) AS 数量, 购进商品制单明细表.商品ID, MIN(商品表.分类编号) AS 分类编号 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + strDT3 + " 23:59:59', 102)) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (商品表.beactive = 1) ";
            strD += " GROUP BY 购进商品制单明细表.商品ID";



            sqlComm.CommandText = "SELECT 商品表.商品名称, 商品表.商品规格, 商品表.商品编号, 商品表.库存金额, 商品表.库存数量, [5天].数量  AS [0-5天], [10天].数量 AS [6-10天], [30天].数量 AS [11-30天], [30天以上].数量 AS [30天以上], 商品表.分类编号 FROM 商品表 LEFT OUTER JOIN (" + strD + ") [30天以上] ON  商品表.ID = [30天以上].商品ID LEFT OUTER JOIN (" + strC + ") [30天] ON 商品表.ID = [30天].商品ID LEFT OUTER JOIN (" + strB + ") [10天] ON 商品表.ID = [10天].商品ID LEFT OUTER JOIN (" + strA + ") [5天] ON 商品表.ID = [5天].商品ID WHERE (商品表.beactive = 1)";

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


            sqlConn.Open();
            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            sqlConn.Close();

            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[9].Visible = false;
            

            adjust();
        }

        private void adjust()
        {
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
        }

        private void adjustDataView()
        {
            int i; 
            decimal dTemp0, dTemp1, dTemp2, dTemp3;

            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表"].Rows[i][4].ToString() == "")
                {
                    dSet.Tables["商品表"].Rows[i][4] = 0;
                    dSet.Tables["商品表"].Rows[i][5] = 0;
                    dSet.Tables["商品表"].Rows[i][6] = 0;
                    dSet.Tables["商品表"].Rows[i][7] = 0;
                    dSet.Tables["商品表"].Rows[i][8] = 0;
                    continue;
                }

                dTemp1 = Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][4]);
                if (dSet.Tables["商品表"].Rows[i][5].ToString() == "")
                {
                    dSet.Tables["商品表"].Rows[i][5] = 0;
                }
                dTemp0 = dTemp1 - Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][5]);
                if (dTemp0 < 0)
                {
                    dSet.Tables["商品表"].Rows[i][5] = dTemp1;
                    dSet.Tables["商品表"].Rows[i][6] = 0;
                    dSet.Tables["商品表"].Rows[i][7] = 0;
                    dSet.Tables["商品表"].Rows[i][8] = 0;
                    continue;
                }

                dTemp1 = dTemp0;
                if (dSet.Tables["商品表"].Rows[i][6].ToString() == "")
                {
                    dSet.Tables["商品表"].Rows[i][6] = 0;
                }
                dTemp0 = dTemp1 - Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][6]);
                if (dTemp0 < 0)
                {
                    dSet.Tables["商品表"].Rows[i][6] = dTemp1;
                    dSet.Tables["商品表"].Rows[i][7] = 0;
                    dSet.Tables["商品表"].Rows[i][8] = 0;
                    continue;
                }

                dTemp1 = dTemp0;
                if (dSet.Tables["商品表"].Rows[i][7].ToString() == "")
                {
                    dSet.Tables["商品表"].Rows[i][7] = 0;
                }
                dTemp0 = dTemp1 - Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][7]);
                if (dTemp0 < 0)
                {
                    dSet.Tables["商品表"].Rows[i][7] = dTemp1;
                    dSet.Tables["商品表"].Rows[i][8] = 0;
                    continue;
                }

                dSet.Tables["商品表"].Rows[i][8] = dTemp0;

            }
        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            bool bMX = true;
            int i,j,k;
            int iRow0,iRow1;
            decimal []dSum= new decimal[6];
            decimal[] dSum1 = new decimal[6];

            string strTTemp = "";

            cGetInformation.getSystemDateTime();
            strDT0 = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT0).ToString("yyyy年M月dd日");

            strDT0 = dateTimePickerE.Value.ToShortDateString();
            strTTemp = dateTimePickerS.Value.ToShortDateString();

            strDT1 = Convert.ToDateTime(strDT0).AddDays(-5).ToShortDateString();
            if (Convert.ToDateTime(strDT1) < Convert.ToDateTime(strTTemp))
                strDT1 = strTTemp;
            strDT2 = Convert.ToDateTime(strDT0).AddDays(-10).ToShortDateString();
            if (Convert.ToDateTime(strDT2) < Convert.ToDateTime(strTTemp))
                strDT2 = strTTemp;
            strDT3 = Convert.ToDateTime(strDT0).AddDays(-30).ToShortDateString();
            if (Convert.ToDateTime(strDT3) < Convert.ToDateTime(strTTemp))
                strDT3 = strTTemp;

            dSum1[0] = 0; dSum1[2] = 0; dSum1[3] = 0; dSum1[4] = 0; dSum1[5] = 0;

            if (MessageBox.Show("是否包含明细？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }


            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                cGetInformation.getUnderClassInformation(intClassID);
            }

            string strA = "SELECT SUM(购进商品制单明细表.数量) AS 数量, 购进商品制单明细表.商品ID, MIN(商品表.分类编号) AS 分类编号 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + strDT0 + " 23:59:59', 102)) AND (购进商品制单表.日期 > CONVERT(DATETIME, '" + strDT1 + " 23:59:59', 102)) AND (商品表.beactive = 1) ";
            strA += " GROUP BY 购进商品制单明细表.商品ID";

            string strB = "SELECT SUM(购进商品制单明细表.数量) AS 数量, 购进商品制单明细表.商品ID, MIN(商品表.分类编号) AS 分类编号 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + strDT1 + " 23:59:59', 102)) AND (购进商品制单表.日期 > CONVERT(DATETIME, '" + strDT2 + " 23:59:59', 102)) AND (商品表.beactive = 1) ";
            strB += " GROUP BY 购进商品制单明细表.商品ID";

            string strC = "SELECT SUM(购进商品制单明细表.数量) AS 数量, 购进商品制单明细表.商品ID, MIN(商品表.分类编号) AS 分类编号 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + strDT2 + " 23:59:59', 102)) AND (购进商品制单表.日期 > CONVERT(DATETIME, '" + strDT3 + " 23:59:59', 102)) AND (商品表.beactive = 1) ";
            strC += " GROUP BY 购进商品制单明细表.商品ID";

            string strD = "SELECT SUM(购进商品制单明细表.数量) AS 数量, 购进商品制单明细表.商品ID, MIN(商品表.分类编号) AS 分类编号 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + strDT3 + " 23:59:59', 102)) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (商品表.beactive = 1) ";
            strD += " GROUP BY 购进商品制单明细表.商品ID";



            sqlComm.CommandText = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.库存金额, 商品表.库存数量, [5天].数量  AS [0-5天], [10天].数量 AS [6-10天], [30天].数量 AS [11-30天], [30天以上].数量 AS [30天以上], 商品表.分类编号 FROM 商品表 LEFT OUTER JOIN (" + strD + ") [30天以上] ON  商品表.ID = [30天以上].商品ID LEFT OUTER JOIN (" + strC + ") [30天] ON 商品表.ID = [30天].商品ID LEFT OUTER JOIN (" + strB + ") [10天] ON 商品表.ID = [10天].商品ID LEFT OUTER JOIN (" + strA + ") [5天] ON 商品表.ID = [5天].商品ID WHERE (商品表.beactive = 1)";

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


            sqlConn.Open();
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
            dTable.Columns.Add("0-5天", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("6-10天", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("11-30天", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("30天以上", System.Type.GetType("System.Int32"));
            //dTable.Columns.Add("分类编号", System.Type.GetType("System.Int32"));

            DataRow []dtC=dSet.Tables["商品分类表"].Select("上级分类 = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[8];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];
                //oTemp[8] = dtC[i][0];
                oTemp[2] = 0; oTemp[3] = 0; oTemp[4] = 0; oTemp[5] = 0; oTemp[6] = 0; oTemp[7] = 0;


                dTable.Rows.Add(oTemp);
                iRow0=dTable.Rows.Count-1;
                
                DataRow []dtC1=dSet.Tables["商品分类表"].Select("上级分类 = '0,"+dtC[i][0]+"'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[8];
                    oTemp1[0] = dtC1[j][1];
                    oTemp1[1] = "　　" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["商品表"].Select("分类编号 = " + dtC1[j][0]);

                    dSum[0] = 0; dSum[1] = 0; dSum[2] = 0; dSum[3] = 0; dSum[4] = 0; dSum[5] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {
                        dSum[0] += Convert.ToDecimal(dtC2[k][2].ToString());
                        dSum[1] += Convert.ToDecimal(dtC2[k][3].ToString());
                        dSum[2] += Convert.ToDecimal(dtC2[k][4].ToString());
                        dSum[3] += Convert.ToDecimal(dtC2[k][5].ToString());
                        dSum[4] += Convert.ToDecimal(dtC2[k][6].ToString());
                        dSum[5] += Convert.ToDecimal(dtC2[k][7].ToString());

                        if (bMX)
                        {
                            object[] oTemp2 = new object[8];
                            oTemp2[0] = dtC2[k][0];
                            oTemp2[1] = "　　　　" + dtC2[k][1];
                            oTemp2[2] = dtC2[k][2];
                            oTemp2[3] = dtC2[k][3];
                            oTemp2[4] = dtC2[k][4];
                            oTemp2[5] = dtC2[k][5];
                            oTemp2[6] = dtC2[k][6];
                            oTemp2[7] = dtC2[k][7];
                            //oTemp2[8] = dtC2[k][8];

                            dTable.Rows.Add(oTemp2);
                        }
                    }
                    dTable.Rows[iRow1][2] = dSum[0];
                    dTable.Rows[iRow1][3] = dSum[1];
                    dTable.Rows[iRow1][4] = dSum[2];
                    dTable.Rows[iRow1][5] = dSum[3];
                    dTable.Rows[iRow1][6] = dSum[4];
                    dTable.Rows[iRow1][7] = dSum[5];

                    dSum1[0] += dSum[0]; dSum1[1] += dSum[1]; dSum1[2] += dSum[2]; dSum1[3] += dSum[3]; dSum1[4] += dSum[4];
                    dSum1[5] += dSum[5];



                    dTable.Rows[iRow0][2] = Convert.ToDecimal(dTable.Rows[iRow0][2]) + Convert.ToDecimal(dTable.Rows[iRow1][2]);
                    dTable.Rows[iRow0][3] = Convert.ToDecimal(dTable.Rows[iRow0][3]) + Convert.ToDecimal(dTable.Rows[iRow1][3]);
                    dTable.Rows[iRow0][4] = Convert.ToDecimal(dTable.Rows[iRow0][4]) + Convert.ToDecimal(dTable.Rows[iRow1][4]);
                    dTable.Rows[iRow0][5] = Convert.ToDecimal(dTable.Rows[iRow0][5]) + Convert.ToDecimal(dTable.Rows[iRow1][5]);
                    dTable.Rows[iRow0][6] = Convert.ToDecimal(dTable.Rows[iRow0][6]) + Convert.ToDecimal(dTable.Rows[iRow1][6]);
                    dTable.Rows[iRow0][7] = Convert.ToDecimal(dTable.Rows[iRow0][7]) + Convert.ToDecimal(dTable.Rows[iRow1][7]);
                }


            }

            object[] oTemp3 = new object[8];
            oTemp3[0] = "合计";
            oTemp3[1] = "";
            oTemp3[2] = dSum1[0];
            oTemp3[3] = dSum1[1];
            oTemp3[4] = dSum1[2];
            oTemp3[5] = dSum1[3];
            oTemp3[6] = dSum1[4];
            oTemp3[7] = dSum1[5];
            dTable.Rows.Add(oTemp3);


            dataGridViewDJMX.DataSource = dTable;
            adjust();

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品库存压占分析;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品库存压占分析;当前日期：" + labelZDRQ.Text;
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