using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJHFLCX : Form
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


        private int iSupplyCompany = 0;
        private ClassGetInformation cGetInformation;
        private decimal[] cTemp = new decimal[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        private decimal[] cTemp1 = new decimal[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        
        public FormJHFLCX()
        {
            InitializeComponent();
        }

        private void FormJHFLCX_Load(object sender, EventArgs e)
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


            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");

            if (dSet.Tables.Contains("职员表1")) dSet.Tables.Remove("职员表1");
            sqlDA.Fill(dSet, "职员表1");

            object[] OTemp = new object[3];
            OTemp[0] = 0;
            OTemp[1] = "全部";
            OTemp[2] = "全部";
            dSet.Tables["职员表"].Rows.Add(OTemp);

            object[] OTemp1 = new object[3];
            OTemp1[0] = 0;
            OTemp1[1] = "全部";
            OTemp1[2] = "全部";
            dSet.Tables["职员表1"].Rows.Add(OTemp1);


            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.SelectedIndex = comboBoxYWY.Items.Count - 1;

            comboBoxCZY.DataSource = dSet.Tables["职员表1"];
            comboBoxCZY.DisplayMember = "职员姓名";
            comboBoxCZY.ValueMember = "ID";
            comboBoxCZY.SelectedIndex = comboBoxCZY.Items.Count - 1;
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1, "") == 0)
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
                if (cGetInformation.getCompanyInformation(10, textBoxDWBH.Text.Trim()) == 0)
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
                if (cGetInformation.getCompanyInformation(12, textBoxDWMC.Text.Trim()) == 0)
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

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, SUM(购进商品制单明细表.数量) AS 数量, SUM(购进商品制单明细表.金额) AS 金额, 商品表.分类编号 FROM 购进商品制单明细表 INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (购进商品制单表.单位ID = " + iSupplyCompany.ToString() + ")";
            if(Convert.ToInt32(comboBoxYWY.SelectedValue)!=0)
                sqlComm.CommandText += " AND (购进商品制单表.业务员ID = " + comboBoxYWY.SelectedValue.ToString()+ ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (购进商品制单表.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";

            sqlComm.CommandText += " GROUP BY 商品表.分类编号, 商品表.商品编号, 商品表.商品名称";



            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");


            sqlComm.CommandText = "SELECT 单位表.单位编号, 单位表.单位名称, SUM(购进商品制单明细表.数量) AS 数量, SUM(购进商品制单明细表.金额) AS 金额 FROM 购进商品制单明细表 INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102)) ";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (购进商品制单表.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (购进商品制单表.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";

            sqlComm.CommandText += " GROUP BY 购进商品制单表.单位ID, 单位表.单位编号, 单位表.单位名称";

            if (dSet.Tables.Contains("商品表2")) dSet.Tables.Remove("商品表2");
            sqlDA.Fill(dSet, "商品表2");

            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, SUM(购进商品制单明细表.数量) AS 数量, SUM(购进商品制单明细表.金额) AS 金额 FROM 购进商品制单明细表 INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102)) ";
            
            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (购进商品制单表.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (购进商品制单表.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";

            sqlComm.CommandText += " GROUP BY 购进商品制单明细表.商品ID, 商品表.商品编号, 商品表.商品名称";

            if (dSet.Tables.Contains("商品表3")) dSet.Tables.Remove("商品表3");
            sqlDA.Fill(dSet, "商品表3");

            sqlComm.CommandText = "SELECT 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称,SUM(购进商品制单明细表.数量) AS 数量, SUM(购进商品制单明细表.金额) AS 金额 FROM 购进商品制单明细表 INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102)) ";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (购进商品制单表.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (购进商品制单表.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";

            sqlComm.CommandText += " GROUP BY 购进商品制单明细表.商品ID, 商品表.商品编号, 商品表.商品名称, 单位表.单位编号, 单位表.单位名称, 购进商品制单表.单位ID ";

            if (dSet.Tables.Contains("商品表4")) dSet.Tables.Remove("商品表4");
            sqlDA.Fill(dSet, "商品表4");

            sqlComm.CommandText = "SELECT 职员表.职员姓名 AS 业务员, 商品表.商品编号, 商品表.商品名称, SUM(购进商品制单明细表.数量) AS 数量, SUM(购进商品制单明细表.金额) AS 金额 FROM 购进商品制单明细表 INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID INNER JOIN 职员表 ON 购进商品制单表.业务员ID = 职员表.ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102)) ";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (购进商品制单表.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (购进商品制单表.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";

            sqlComm.CommandText += " GROUP BY 购进商品制单明细表.商品ID, 商品表.商品编号, 商品表.商品名称, 购进商品制单表.业务员ID, 职员表.职员姓名 ";

            if (dSet.Tables.Contains("商品表5")) dSet.Tables.Remove("商品表5");
            sqlDA.Fill(dSet, "商品表5");


            sqlConn.Close();
            adjustDataView1();
            dataGridView2.DataSource = dSet.Tables["商品表2"];
            dataGridView3.DataSource = dSet.Tables["商品表3"];
            dataGridView4.DataSource = dSet.Tables["商品表4"];
            dataGridView5.DataSource = dSet.Tables["商品表5"];
            dataGridView2.Columns[2].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[2].DefaultCellStyle.Format = "f0";
            dataGridView4.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridView5.Columns[3].DefaultCellStyle.Format = "f0";

            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);

        }

        private void adjustDataView1()
        {
            int i;

            for (i = 0; i < dSet.Tables["商品表1"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表1"].Rows[i][2].ToString() == "")
                    dSet.Tables["商品表1"].Rows[i][2] = 0;
                if (dSet.Tables["商品表1"].Rows[i][3].ToString() == "")
                    dSet.Tables["商品表1"].Rows[i][3] = 0;

            }

            int j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[2];
            decimal[] dSum1 = new decimal[2];
            decimal[] dSum2 = new decimal[2];

            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;


            sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 ORDER BY 上级分类";
            if (dSet.Tables.Contains("商品分类表")) dSet.Tables.Remove("商品分类表");
            sqlDA.Fill(dSet, "商品分类表");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("分类编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("分类名称", System.Type.GetType("System.String"));
            dTable.Columns.Add("数量", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("金额", System.Type.GetType("System.Decimal"));


            DataRow[] dtC = dSet.Tables["商品分类表"].Select("上级分类 = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[4];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 0; t < dSum2.Length; t++)
                    dSum2[t] = 0;

                DataRow[] dtC3 = dSet.Tables["商品表1"].Select("分类编号 = " + dtC[i][0]);
                for (k = 0; k < dtC3.Length; k++)
                {

                    for (t = 0; t < dSum2.Length; t++)
                        dSum2[t] += Convert.ToDecimal(dtC3[k][t + 2].ToString());

                }
                for (t = 2; t < oTemp.Length; t++)
                    oTemp[t] = dSum2[t-2];

                for (t = 0; t < dSum2.Length; t++)
                    dSum1[t] += dSum2[t];




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

                    DataRow[] dtC2 = dSet.Tables["商品表1"].Select("分类编号 = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 2].ToString());

                    }

                    for (t = 2; t < dSum.Length+2; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length+2; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }

            object[] oTemp3 = new object[4];
            oTemp3[0] = "合计";
            oTemp3[1] = "";
            for (t = 2; t < oTemp3.Length; t++)
                oTemp3[t] = dSum1[t - 2];
            dTable.Rows.Add(oTemp3);


            dataGridView1.DataSource = dTable;
            dataGridView1.Columns[2].DefaultCellStyle.Format = "f0";


        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "进货分类查询（按商品分类汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "进货分类查询（按单位汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "进货分类查询（按商品汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "进货分类查询（按单位商品汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, true, intUserLimit);
                    break;
                case 4:
                    strT = "进货分类查询（按业务员商品汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, true, intUserLimit);
                    break;

            }

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "进货分类查询（按商品分类汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "进货分类查询（按单位汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "进货分类查询（按商品汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "进货分类查询（按单位商品汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, false, intUserLimit);
                    break;
                case 4:
                    strT = "进货分类查询（按业务员商品汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, false, intUserLimit);
                    break;

            }

        }

        private void countfTemp()
        {
            int c = 0;
            int c1 = 0;
            int i, j;

            for (i = 1; i <= 5; i++)
            {
                cTemp[i - 1] = 0;
                cTemp1[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 0;
                        c1 = 0;
                        break;
                    case 2:
                        c = 3;
                        c1 = 2;
                        break;
                    case 3:
                        c = 3;
                        c1 = 2;
                        break;
                    case 4:
                        c = 5;
                        c1 = 4;
                        break;
                    case 5:
                        c = 4;
                        c1 = 3;
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
                            cTemp[i - 1] += Convert.ToDecimal(dSet.Tables["商品表" + i.ToString()].Rows[j][c].ToString());
                        }
                        catch
                        {
                        }
                    }
                }

                if (c1 != 0)
                {

                    for (j = 0; j < dSet.Tables["商品表" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp1[i - 1] += Convert.ToDecimal(dSet.Tables["商品表" + i.ToString()].Rows[j][c1].ToString());
                        }
                        catch
                        {
                        }
                    }
                }

            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;
            if (!dSet.Tables.Contains("商品表" + c1.ToString()))
                return;

            if (c1 != 1)
                toolStripStatusLabelC.Text = "共有" + dSet.Tables["商品表" + c1.ToString()].Rows.Count.ToString() + "条记录 数量合计" + cTemp1[tabControl1.SelectedIndex].ToString("f0") + " 金额合计" + cTemp[tabControl1.SelectedIndex].ToString() + "元";
            else
                toolStripStatusLabelC.Text = "";
        }
    }
}