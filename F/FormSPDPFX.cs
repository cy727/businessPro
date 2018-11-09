using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPDPFX : Form
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

        public int intCommID = 0;

        private ClassGetInformation cGetInformation;

        private decimal[] cTemp = new decimal[14] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0,0};
        private decimal[] cTemp1 = new decimal[14] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0, 0, 0, 0};

        public FormSPDPFX()
        {
            InitializeComponent();
        }

        private void FormSPDPFX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

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
            /*
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            sqlConn.Close();
             */

            if (intCommID != 0) //存在初始商品
            {
                if (cGetInformation.getCommInformation(40, intCommID.ToString()) == 0) //失败
                {
                    intCommID = 0;
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                    toolStripButtonGD_Click(null, null);

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
                toolStripButtonGD_Click(null, null);


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
                    toolStripButtonGD_Click(null, null);

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
                    toolStripButtonGD_Click(null, null);

                }

            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {

            //保存完毕
            if (intCommID == 0)
            {
                MessageBox.Show("请选择要查询的商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 助记码, 计量单位, 计量规格, 最小计量单位, 进价, 批发价, 登录日期 FROM 商品表 WHERE (ID = "+intCommID.ToString()+")";
            sqldr = sqlComm.ExecuteReader();
            while(sqldr.Read())
            {
                labelZJM.Text = sqldr.GetValue(0).ToString();
                labelJLDW.Text = sqldr.GetValue(1).ToString();
                labelJLGG.Text = sqldr.GetValue(2).ToString();
                labelZXJLDW.Text = sqldr.GetValue(3).ToString();
                labelJJ.Text = sqldr.GetValue(4).ToString();
                labelPFJ.Text = sqldr.GetValue(5).ToString();
                if (sqldr.GetValue(6).ToString()!="")
                    labelDLRQ.Text = Convert.ToDateTime(sqldr.GetValue(6).ToString()).ToString("yyyy年M月dd日");
                else
                    labelDLRQ.Text = sqldr.GetValue(6).ToString();
            }
            sqldr.Close();
            sqlConn.Close();

            initGJ();
            initXS();
            iniKC();
            iniDataView();
            tabControl1_SelectedIndexChanged(null, null);

        }

        private void iniKC()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 库存金额 FROM 商品表 WHERE (ID = "+intCommID.ToString()+")";
            sqldr = sqlComm.ExecuteReader();
            labelZKCSL.Text = "";
            labelZKCCBJ.Text = "";
            labelZKCJE.Text = "";
            while (sqldr.Read())
            {
                labelZKCSL.Text = sqldr.GetValue(0).ToString() ;
                labelZKCCBJ.Text = sqldr.GetValue(1).ToString();
                labelZKCJE.Text = sqldr.GetValue(2).ToString();
            }
            if (labelZKCSL.Text == "")
                labelZKCSL.Text = "0";
            if (labelZKCCBJ.Text == "")
                labelZKCCBJ.Text = "0";
            if (labelZKCJE.Text == "")
                labelZKCJE.Text = "0";

            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(盘损数量) AS Expr1, SUM(盘损金额) AS Expr2 FROM 库存盘点明细表 WHERE (商品ID =  " + intCommID.ToString() + ") ";
            sqldr = sqlComm.ExecuteReader();
            labelPSSL.Text = "";
            labelPSJE.Text = "";
            while (sqldr.Read())
            {
                labelPSSL.Text = sqldr.GetValue(0).ToString();
                labelPSJE.Text = sqldr.GetValue(1).ToString();
            }
            if (labelPSSL.Text == "")
                labelPSSL.Text = "0";
            if (labelPSJE.Text == "")
                labelPSJE.Text = "0";
            sqldr.Close();


            sqlComm.CommandText = "SELECT SUM(报损数量) AS Expr1, SUM(报损金额) AS Expr2 FROM 库存报损明细表 WHERE (商品ID = " + intCommID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            labelBSSL.Text = "";
            labelBSJE.Text = "";
            while (sqldr.Read())
            {
                labelBSSL.Text = sqldr.GetValue(0).ToString();
                labelBSJE.Text = sqldr.GetValue(1).ToString();
            }
            if (labelBSSL.Text == "")
                labelBSSL.Text = "0";
            if (labelBSJE.Text == "")
                labelBSJE.Text = "0";
            sqldr.Close();

            sqlConn.Close();
        }

        private void iniDataView()
        {
            int i, j;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 库房表.库房编号, 库房表.库房名称, 库存表.库存数量, 库存表.库存金额 FROM 库存表 INNER JOIN 库房表 ON 库存表.库房ID = 库房表.ID WHERE (库存表.商品ID = "+intCommID.ToString()+")";


            if (dSet.Tables.Contains("库存表")) dSet.Tables.Remove("库存表");
            sqlDA.Fill(dSet, "库存表");
            dataGridViewKCFB.DataSource = dSet.Tables["库存表"];

            cTemp[3] = 0; cTemp1[3] = 0;
            for (i = 0; i <= dSet.Tables["库存表"].Rows.Count; i++)
            {
                try
                {
                    cTemp[3] += decimal.Parse(dSet.Tables["库存表"].Rows[i][2].ToString());
                    cTemp1[3] += decimal.Parse(dSet.Tables["库存表"].Rows[i][3].ToString());
                }
                catch
                {
                }
            }



            sqlComm.CommandText = "SELECT 购进商品制单表.ID, 购进商品制单表.单据编号, 购进商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 购进商品制单明细表.数量, 购进商品制单明细表.单价, 购进商品制单明细表.金额 FROM 购进商品制单明细表 INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID INNER JOIN 库房表 ON 购进商品制单明细表.库房ID = 库房表.ID WHERE (购进商品制单明细表.商品ID = " + intCommID.ToString() + ") AND (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 购进商品制单表.日期 DESC";


            if (dSet.Tables.Contains("购进商品制单表")) dSet.Tables.Remove("购进商品制单表");
            sqlDA.Fill(dSet, "购进商品制单表");
            dataGridViewGJMX.DataSource = dSet.Tables["购进商品制单表"];
            dataGridViewGJMX.Columns[0].Visible = false;
            dataGridViewGJMX.Columns[7].DefaultCellStyle.Format = "f0";

            cTemp[4] = 0; cTemp1[4] = 0;
            for (i = 0; i <= dSet.Tables["购进商品制单表"].Rows.Count; i++)
            {
                try
                {
                    cTemp[4] += decimal.Parse(dSet.Tables["购进商品制单表"].Rows[i][7].ToString());
                    cTemp1[4] += decimal.Parse(dSet.Tables["购进商品制单表"].Rows[i][9].ToString());
                }
                catch
                {
                }
            }


            sqlComm.CommandText = "SELECT 销售商品制单表.ID, 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.数量, 销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.扣率, 销售商品制单明细表.实计金额 FROM 单位表 INNER JOIN 库房表 INNER JOIN 销售商品制单明细表 ON 库房表.ID = 销售商品制单明细表.库房ID INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID ON  单位表.ID = 销售商品制单表.单位ID WHERE (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") AND (销售商品制单表.BeActive = 1) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 销售商品制单表.日期 DESC";


            if (dSet.Tables.Contains("销售商品制单表")) dSet.Tables.Remove("销售商品制单表");
            sqlDA.Fill(dSet, "销售商品制单表");
            dataGridViewXSMX.DataSource = dSet.Tables["销售商品制单表"];
            dataGridViewXSMX.Columns[0].Visible = false;
            dataGridViewXSMX.Columns[7].DefaultCellStyle.Format = "f0";

            cTemp[5] = 0; cTemp1[5] = 0;
            for (i = 0; i <= dSet.Tables["销售商品制单表"].Rows.Count; i++)
            {
                try
                {
                    cTemp[5] += decimal.Parse(dSet.Tables["销售商品制单表"].Rows[i][7].ToString());
                    cTemp1[5] += decimal.Parse(dSet.Tables["销售商品制单表"].Rows[i][11].ToString());
                }
                catch
                {
                }
            }

            sqlComm.CommandText = "SELECT 销售商品制单表.ID, 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 职员表.职员姓名 AS 业务员, [职员表_1].职员姓名 AS 操作员, 销售商品制单明细表.数量, 销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.毛利 FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID INNER JOIN 职员表 ON 销售商品制单表.业务员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 销售商品制单表.操作员ID = [职员表_1].ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID WHERE (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") AND (销售商品制单表.BeActive = 1) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单明细表.毛利 <= 0) ORDER BY 销售商品制单表.日期 DESC";


            if (dSet.Tables.Contains("商品历史账表")) dSet.Tables.Remove("商品历史账表");
            sqlDA.Fill(dSet, "商品历史账表");
            for (i = 0; i < dSet.Tables["商品历史账表"].Rows.Count; i++)
                for (j = 3; j < dSet.Tables["商品历史账表"].Columns.Count; j++)
                {
                    if (dSet.Tables["商品历史账表"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品历史账表"].Rows[i][j] = 0;
                }
            dataGridViewCRK.DataSource = dSet.Tables["商品历史账表"];
            dataGridViewCRK.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewCRK.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewCRK.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewCRK.Columns[0].Visible = false;
        


            sqlComm.CommandText = "SELECT 库房表.库房编号, 库房表.库房名称, 库存表.库存数量, 库存表.库存下限, 库存表.合理库存下限, 库存表.合理库存上限, 库存表.库存上限 FROM 库存表 INNER JOIN 库房表 ON 库存表.库房ID = 库房表.ID WHERE (库存表.商品ID = "+intCommID.ToString()+")";


            if (dSet.Tables.Contains("库房表")) dSet.Tables.Remove("库房表");
            sqlDA.Fill(dSet, "库房表");
            for (i = 0; i < dSet.Tables["库房表"].Rows.Count; i++)
                for (j = 2; j < dSet.Tables["库房表"].Columns.Count; j++)
                {
                    if (dSet.Tables["库房表"].Rows[i][j].ToString() == "")
                        dSet.Tables["库房表"].Rows[i][j] = 0;
                }
            dataGridViewCHFX.DataSource = dSet.Tables["库房表"];
            dataGridViewCHFX.Columns[2].DefaultCellStyle.Format = "f0";
            dataGridViewCHFX.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridViewCHFX.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridViewCHFX.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridViewCHFX.Columns[6].DefaultCellStyle.Format = "f0";


            cTemp[7] = 0; cTemp1[7] = 0;
            for (i = 0; i <= dSet.Tables["库房表"].Rows.Count; i++)
            {
                try
                {
                    cTemp[7] += decimal.Parse(dSet.Tables["库房表"].Rows[i][2].ToString());
                }
                catch
                {
                }
            }


            sqlComm.CommandText = "SELECT 借物出库明细表.表单ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期,单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 借物出库明细表.数量, 借物出库明细表.单价, 借物出库明细表.金额 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID WHERE (借物出库明细表.商品ID = " + intCommID.ToString() + ") AND (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.冲抵单号ID IS NULL) ORDER BY 借物出库汇总表.日期 DESC";


            if (dSet.Tables.Contains("借物出库汇总表")) dSet.Tables.Remove("借物出库汇总表");
            sqlDA.Fill(dSet, "借物出库汇总表");
            dataGridViewJW.DataSource = dSet.Tables["借物出库汇总表"];
            dataGridViewJW.Columns[0].Visible = false;
            dataGridViewJW.Columns[7].DefaultCellStyle.Format = "f0";

            cTemp[8] = 0; cTemp1[8] = 0;
            for (i = 0; i <= dSet.Tables["借物出库汇总表"].Rows.Count; i++)
            {
                try
                {
                    cTemp[8] += decimal.Parse(dSet.Tables["借物出库汇总表"].Rows[i][7].ToString());
                }
                catch
                {
                }
            }

            sqlComm.CommandText = "SELECT 借物出库明细表.表单ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期,单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 借物出库明细表.数量, 借物出库明细表.单价, 借物出库明细表.金额 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID WHERE (借物出库明细表.商品ID = " + intCommID.ToString() + ") AND (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.冲抵单号ID = -1) ORDER BY 借物出库汇总表.日期 DESC";


            if (dSet.Tables.Contains("借物出库汇总表1")) dSet.Tables.Remove("借物出库汇总表1");
            sqlDA.Fill(dSet, "借物出库汇总表1");
            dataGridViewJW1.DataSource = dSet.Tables["借物出库汇总表1"];
            dataGridViewJW1.Columns[0].Visible = false;
            dataGridViewJW1.Columns[7].DefaultCellStyle.Format = "f0";

            cTemp[9] = 0; cTemp1[9] = 0;
            for (i = 0; i <= dSet.Tables["借物出库汇总表1"].Rows.Count; i++)
            {
                try
                {
                    cTemp[9] += decimal.Parse(dSet.Tables["借物出库汇总表1"].Rows[i][7].ToString());
                }
                catch
                {
                }
            }

            sqlComm.CommandText = "SELECT 进货退出汇总表.ID, 进货退出汇总表.单据编号, 进货退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 进货退出明细表.数量, 进货退出明细表.单价, 进货退出明细表.金额 FROM 进货退出汇总表 INNER JOIN 进货退出明细表 ON 进货退出汇总表.ID = 进货退出明细表.单据ID INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID INNER JOIN 库房表 ON 进货退出明细表.库房ID = 库房表.ID WHERE (进货退出明细表.商品ID = " + intCommID.ToString() + ") AND (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 进货退出汇总表.日期 DESC";


            if (dSet.Tables.Contains("进货退出汇总表")) dSet.Tables.Remove("进货退出汇总表");
            sqlDA.Fill(dSet, "进货退出汇总表");
            dataGridViewGJTC.DataSource = dSet.Tables["进货退出汇总表"];
            dataGridViewGJTC.Columns[0].Visible = false;
            dataGridViewGJTC.Columns[7].DefaultCellStyle.Format = "f0";

            cTemp[10] = 0; cTemp1[10] = 0;
            for (i = 0; i <= dSet.Tables["进货退出汇总表"].Rows.Count; i++)
            {
                try
                {
                    cTemp[10] += decimal.Parse(dSet.Tables["进货退出汇总表"].Rows[i][7].ToString());
                    cTemp1[10] += decimal.Parse(dSet.Tables["进货退出汇总表"].Rows[i][9].ToString());
                }
                catch
                {
                }
            }

            sqlComm.CommandText = "SELECT 销售退出汇总表.ID, 销售退出汇总表.单据编号, 销售退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 销售退出明细表.数量, 销售退出明细表.单价, 销售退出明细表.金额 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID INNER JOIN 库房表 ON 销售退出明细表.库房ID = 库房表.ID WHERE (销售退出明细表.商品ID = " + intCommID.ToString() + ") AND (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 销售退出汇总表.日期 DESC";


            if (dSet.Tables.Contains("销售退出汇总表")) dSet.Tables.Remove("销售退出汇总表");
            sqlDA.Fill(dSet, "销售退出汇总表");
            dataGridViewXSTH.DataSource = dSet.Tables["销售退出汇总表"];
            dataGridViewXSTH.Columns[0].Visible = false;
            dataGridViewXSTH.Columns[7].DefaultCellStyle.Format = "f0";

            cTemp[11] = 0; cTemp1[11] = 0;
            for (i = 0; i <= dSet.Tables["销售退出汇总表"].Rows.Count; i++)
            {
                try
                {
                    cTemp[11] += decimal.Parse(dSet.Tables["销售退出汇总表"].Rows[i][7].ToString());
                    cTemp1[11] += decimal.Parse(dSet.Tables["销售退出汇总表"].Rows[i][9].ToString());
                }
                catch
                {
                }
            }
            sqlComm.CommandText = "SELECT 购进退补差价汇总表.ID, 购进退补差价汇总表.单据编号, 购进退补差价汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 购进退补差价明细表.补价数量, 购进退补差价明细表.差价, 购进退补差价明细表.金额 FROM 购进退补差价汇总表 INNER JOIN 购进退补差价明细表 ON 购进退补差价汇总表.ID = 购进退补差价明细表.单据ID INNER JOIN 单位表 ON 购进退补差价汇总表.单位ID = 单位表.ID INNER JOIN 库房表 ON 购进退补差价明细表.库房ID = 库房表.ID WHERE (购进退补差价明细表.商品ID = " + intCommID.ToString() + ") AND (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 购进退补差价汇总表.日期 DESC";


            if (dSet.Tables.Contains("购进退补差价汇总表")) dSet.Tables.Remove("购进退补差价汇总表");
            sqlDA.Fill(dSet, "购进退补差价汇总表");
            dataGridViewGJTBJ.DataSource = dSet.Tables["购进退补差价汇总表"];
            dataGridViewGJTBJ.Columns[0].Visible = false;
            dataGridViewGJTBJ.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewGJTBJ.Columns[8].DefaultCellStyle.Format = "f2";

            cTemp[12] = 0; cTemp1[12] = 0;
            for (i = 0; i <= dSet.Tables["购进退补差价汇总表"].Rows.Count; i++)
            {
                try
                {
                    cTemp[12] += decimal.Parse(dSet.Tables["购进退补差价汇总表"].Rows[i][7].ToString());
                    cTemp1[12] += decimal.Parse(dSet.Tables["购进退补差价汇总表"].Rows[i][9].ToString());
                }
                catch
                {
                }
            }

            sqlComm.CommandText = "SELECT 销售退补差价汇总表.ID, 销售退补差价汇总表.单据编号, 销售退补差价汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 销售退补差价明细表.补价数量, 销售退补差价明细表.差价, 销售退补差价明细表.金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID INNER JOIN 库房表 ON 销售退补差价明细表.库房ID = 库房表.ID WHERE (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") AND (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 销售退补差价汇总表.日期 DESC";


            if (dSet.Tables.Contains("销售退补差价汇总表")) dSet.Tables.Remove("销售退补差价汇总表");
            sqlDA.Fill(dSet, "销售退补差价汇总表");
            dataGridViewXSTBJ.DataSource = dSet.Tables["销售退补差价汇总表"];
            dataGridViewXSTBJ.Columns[0].Visible = false;
            dataGridViewXSTBJ.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewXSTBJ.Columns[8].DefaultCellStyle.Format = "f2";

            cTemp[13] = 0; cTemp1[13] = 0;
            for (i = 0; i <= dSet.Tables["销售退补差价汇总表"].Rows.Count; i++)
            {
                try
                {
                    cTemp[13] += decimal.Parse(dSet.Tables["销售退补差价汇总表"].Rows[i][7].ToString());
                    cTemp1[12] += decimal.Parse(dSet.Tables["销售退补差价汇总表"].Rows[i][9].ToString());
                }
                catch
                {
                }
            }

            sqlConn.Close();
        }
        private void iniCHFX()
        {
        }


        private void initGJ()
        {
            int i, j;
            string dTemp = "0", dTemp1 = "0",dTemp2 = "0";
            decimal dt1 = 0, dt2 = 0;

            dataGridViewGJ.Rows.Clear();


            sqlConn.Open();
            sqlComm.CommandText = "SELECT SUM(购进商品制单明细表.数量), SUM(购进商品制单明细表.实计金额) FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID WHERE (购进商品制单明细表.商品ID = " + intCommID.ToString() + ") AND (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(购进数量) AS 购进数量, SUM(购进金额) AS 购进金额 FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%AKP%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp=sqldr.GetValue(0).ToString();
                dTemp1=sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";

            sqldr.Close();

            object[] objTemp = new object[3];
            objTemp[0] = "购进";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");

            dataGridViewGJ.Rows.Add(objTemp);


            sqlComm.CommandText = "SELECT SUM(进货入库明细表.数量), SUM(进货入库明细表.金额) FROM 进货入库汇总表 INNER JOIN 进货入库明细表 ON 进货入库汇总表.ID = 进货入库明细表.单据ID WHERE (进货入库明细表.商品ID = " + intCommID.ToString() + ") AND (进货入库汇总表.BeActive = 1) AND (进货入库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(入库数量), SUM(入库金额) FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%ADH%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            objTemp[0] = "到货";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewGJ.Rows.Add(objTemp);


            sqlComm.CommandText = "SELECT SUM(数量) , SUM(已付款金额), SUM(未付款金额) FROM  付款明细视图 WHERE (商品ID = " + intCommID.ToString() + ") AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(结算数量) , SUM(结算金额) FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%AYF%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
                dTemp2 = sqldr.GetValue(2).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            if (dTemp2 == "")
                dTemp2 = "0";
            sqldr.Close();
            objTemp[0] = "结算";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewGJ.Rows.Add(objTemp);

            dt1 = Convert.ToDecimal(dataGridViewGJ.Rows[0].Cells[1].Value.ToString()) - Convert.ToDecimal(dataGridViewGJ.Rows[2].Cells[1].Value.ToString());
            dt2 = Convert.ToDecimal(dataGridViewGJ.Rows[0].Cells[2].Value.ToString()) - Convert.ToDecimal(dataGridViewGJ.Rows[2].Cells[2].Value.ToString());
            objTemp[0] = "当前应付";
            objTemp[1] = "";
            objTemp[2] = decimal.Parse(dTemp2).ToString("f2");
            dataGridViewGJ.Rows.Add(objTemp);


            sqlComm.CommandText = "SELECT SUM(进货退出明细表.数量), SUM(进货退出明细表.实计金额) FROM 进货退出汇总表 INNER JOIN 进货退出明细表 ON 进货退出汇总表.ID = 进货退出明细表.单据ID WHERE (进货退出明细表.商品ID = " + intCommID.ToString() + ") AND (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(退出数量) , SUM(退出金额) FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%ATH%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            objTemp[0] = "退出";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewGJ.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT SUM(购进退补差价明细表.补价数量), SUM(购进退补差价明细表.金额) FROM 购进退补差价明细表 INNER JOIN 购进退补差价汇总表 ON 购进退补差价明细表.单据ID = 购进退补差价汇总表.ID WHERE (购进退补差价明细表.商品ID = " + intCommID.ToString() + ") AND (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(退出数量) , SUM(退出金额) FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%ATH%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            objTemp[0] = "退补价";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewGJ.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT COUNT(*) FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID WHERE (购进商品制单明细表.商品ID = " + intCommID.ToString() + ") AND (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            //sqlComm.CommandText = "SELECT COUNT(*) AS Expr1 FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%AKP%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            sqldr.Close();
            labelGJPC.Text = dTemp;


            sqlComm.CommandText = "SELECT  COUNT(*) FROM 进货退出汇总表 INNER JOIN 进货退出明细表 ON 进货退出汇总表.ID = 进货退出明细表.单据ID WHERE (进货退出明细表.商品ID = " + intCommID.ToString() + ") AND (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            //sqlComm.CommandText = "SELECT COUNT(*) AS Expr1 FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%ATH%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            sqldr.Close();
            labelTUPC.Text = dTemp;


            sqlComm.CommandText = "SELECT MIN(购进商品制单明细表.单价), MAX(购进商品制单明细表.单价) FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID WHERE (购进商品制单明细表.商品ID = " + intCommID.ToString() + ") AND (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            //sqlComm.CommandText = "SELECT MIN(购进单价) AS Expr1, MAX(购进单价) AS Expr2 FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%AKP%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            labelZDJJ.Text = dTemp;
            labelZGJJ.Text = dTemp1;

            dt1 = Convert.ToDecimal(dataGridViewGJ.Rows[0].Cells[2].Value.ToString());
            dt2 = Convert.ToDecimal(dataGridViewGJ.Rows[0].Cells[1].Value.ToString());
            if (dt2 == 0)
                dt1 = 0;
            else
                dt1 = dt1 / dt2;

            labelPJGJDJ.Text = dt1.ToString("f2");

            dt2 = Convert.ToDecimal(labelGJPC.Text);
            dt1 = Convert.ToDecimal(labelTUPC.Text);
            if (dt2 == 0)
                dt1 = 0;
            else
                dt1 = dt1 / dt2*100;
            labelGJTCL.Text = dt1.ToString();

            sqlConn.Close();

            dataGridViewGJ.Columns[1].DefaultCellStyle.Format = "f0";
            dataGridViewGJ.Columns[2].DefaultCellStyle.Format = "f2";
        }

        private void initXS()
        {
            int i, j;
            string dTemp = "0", dTemp1 = "0",dTemp2="0";
            decimal dt1 = 0, dt2 = 0;

            dataGridViewXS.Rows.Clear();


            sqlConn.Open();
            //sqlComm.CommandText = "SELECT SUM(出库数量) , SUM(出库金额) FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%BKP%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqlComm.CommandText = "SELECT SUM(销售商品制单明细表.数量) AS Expr1, SUM(销售商品制单明细表.实计金额) AS Expr2 FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID WHERE (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") AND (销售商品制单表.BeActive = 1) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";

            sqldr.Close();

            object[] objTemp = new object[3];
            objTemp[0] = "销售";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");

            dataGridViewXS.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT SUM(数量) AS Expr1, SUM(实计金额) AS Expr2 FROM 出库视图    WHERE (商品ID = " + intCommID.ToString() + ")AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";

            sqldr.Close();

            objTemp[0] = "出库";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewXS.Rows.Add(objTemp);


            sqlComm.CommandText = "SELECT SUM(数量) , SUM(已付款金额),  SUM(未付款金额) FROM  收款明细视图 WHERE (商品ID = " + intCommID.ToString() + ") AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(结算数量) , SUM(结算金额) FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%BYS%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
                dTemp2 = sqldr.GetValue(2).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            if (dTemp2 == "")
                dTemp2 = "0";
            sqldr.Close();
            objTemp[0] = "结算";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewXS.Rows.Add(objTemp);

            dt1 = Convert.ToDecimal(dataGridViewXS.Rows[0].Cells[1].Value.ToString()) - Convert.ToDecimal(dataGridViewXS.Rows[2].Cells[1].Value.ToString());
            dt2 = Convert.ToDecimal(dataGridViewXS.Rows[0].Cells[2].Value.ToString()) - Convert.ToDecimal(dataGridViewXS.Rows[2].Cells[2].Value.ToString());
            objTemp[0] = "当前应收";
            objTemp[1] = "";
            objTemp[2] = decimal.Parse(dTemp2).ToString("f2"); 
            dataGridViewXS.Rows.Add(objTemp);


            sqlComm.CommandText = "SELECT SUM(销售退出明细表.数量), SUM(销售退出明细表.实计金额) FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID WHERE (销售退出明细表.商品ID = " + intCommID.ToString() + ") AND (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            //sqlComm.CommandText = "SELECT SUM(退回数量) , SUM(退回金额) FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%BTH%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            objTemp[0] = "退回";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewXS.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT SUM(销售退补差价明细表.补价数量), SUM(销售退补差价明细表.金额) FROM 销售退补差价明细表 INNER JOIN 销售退补差价汇总表 ON 销售退补差价明细表.单据ID = 销售退补差价汇总表.ID WHERE (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") AND (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT SUM(退出数量) , SUM(退出金额) FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%ATH%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            objTemp[0] = "退补价";
            objTemp[1] = decimal.Parse(dTemp).ToString("f0");
            objTemp[2] = decimal.Parse(dTemp1).ToString("f2");
            dataGridViewXS.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT COUNT(*) FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID WHERE (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") AND (销售商品制单表.BeActive = 1) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT COUNT(*) AS Expr1 FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%BKP%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            sqldr.Close();
            labelXSPC.Text = dTemp;

            sqlComm.CommandText = "SELECT COUNT(*) FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID WHERE (销售退出明细表.商品ID = " + intCommID.ToString() + ") AND (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            //sqlComm.CommandText = "SELECT COUNT(*) AS Expr1 FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%BTH%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            sqldr.Close();
            labelTHPC.Text = dTemp;


            sqlComm.CommandText = "SELECT MIN(销售商品制单明细表.单价) AS Expr1, MAX(销售商品制单明细表.单价) AS Expr2 FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID WHERE (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") AND (销售商品制单表.BeActive = 1) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            //sqlComm.CommandText = "SELECT MIN(出库单价) AS Expr1, MAX(出库单价) AS Expr2 FROM 商品历史账表 WHERE (商品ID = " + intCommID.ToString() + ") AND (单据编号 LIKE N'%BKP%')  AND (BeActive = 1) AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                dTemp = sqldr.GetValue(0).ToString();
                dTemp1 = sqldr.GetValue(1).ToString();
            }
            if (dTemp == "")
                dTemp = "0";
            if (dTemp1 == "")
                dTemp1 = "0";
            sqldr.Close();
            labelZGSJ.Text = dTemp1;
            labelZDSJ.Text = dTemp;

            dt1 = Convert.ToDecimal(dataGridViewXS.Rows[0].Cells[2].Value.ToString());
            dt2 = Convert.ToDecimal(dataGridViewXS.Rows[0].Cells[1].Value.ToString());
            if (dt2 == 0)
                dt1 = 0;
            else
                dt1 = dt1 / dt2;

            labelPJXSDJ.Text = dt1.ToString("f2");

            dt2 = Convert.ToDecimal(labelXSPC.Text);
            dt1 = Convert.ToDecimal(labelTUPC.Text);
            if (dt2 == 0)
                dt1 = 0;
            else
                dt1 = dt1 / dt2 * 100;
            labelXSTHL.Text = dt1.ToString();

            sqlConn.Close();
            dataGridViewXS.Columns[1].DefaultCellStyle.Format = "f0";
            dataGridViewXS.Columns[2].DefaultCellStyle.Format = "f2";

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "商品单品分析（购进）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJ, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "商品单品分析（销售）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXS, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "商品单品分析（库存分布）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewKCFB, strT, true, intUserLimit);
                    break;
                case 4:
                    strT = "商品单品分析（购进明细）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJMX, strT, true, intUserLimit);
                    break;
                case 5:
                    strT = "商品单品分析（销往明细）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXSMX, strT, true, intUserLimit);
                    break;
                case 6:
                    strT = "商品单品分析（低于成本销售）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewCRK, strT, true, intUserLimit);
                    break;
                case 7:
                    strT = "商品单品分析（存货分析）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewCHFX, strT, true, intUserLimit);
                    break;
                case 8:
                    strT = "商品单品分析（借物分析）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewJW, strT, true, intUserLimit);
                    break;
                case 9:
                    strT = "商品单品分析（购进退回分析）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJTC, strT, true, intUserLimit);
                    break;
                case 10:
                    strT = "商品单品分析（销售退回分析）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXSTH, strT, true, intUserLimit);
                    break;
            }

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "商品单品分析（购进）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJ, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "商品单品分析（销售）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXS, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "商品单品分析（库存分布）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewKCFB, strT, false, intUserLimit);
                    break;
                case 4:
                    strT = "商品单品分析（购进明细）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJMX, strT, false, intUserLimit);
                    break;
                case 5:
                    strT = "商品单品分析（销往明细）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXSMX, strT, false, intUserLimit);
                    break;
                case 6:
                    strT = "商品单品分析（低于成本销售）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewCRK, strT, false, intUserLimit);
                    break;
                case 7:
                    strT = "商品单品分析（存货分析）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewCHFX, strT, false, intUserLimit);
                    break;
                case 8:
                    strT = "商品单品分析（借物分析）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewJW, strT, false, intUserLimit);
                    break;
                case 9:
                    strT = "商品单品分析（购进退回分析）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJTC, strT, false, intUserLimit);
                    break;
                case 10:
                    strT = "商品单品分析（销售退回分析）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXSTH, strT, false, intUserLimit);
                    break;
            }

        }

        private void dataGridViewMX_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgV = (DataGridView)sender;
            if (dgV.RowCount < 1)
                return;

            if (dgV.SelectedRows.Count < 1)
                return;

            string sTemp = "", sTemp1 = "";
            sTemp = dgV.SelectedRows[0].Cells[1].Value.ToString().ToUpper();
            sTemp1 = dgV.SelectedRows[0].Cells[0].Value.ToString();

            switch (sTemp.Substring(0, 2))
            {
                case "CG":
                    // 创建此子窗体的一个新实例。
                    FormCGHT childFormCGHT = new FormCGHT();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormCGHT.MdiParent = this.MdiParent;

                    childFormCGHT.strConn = strConn;
                    childFormCGHT.iDJID = Convert.ToInt32(sTemp1);
                    childFormCGHT.isSaved = true;

                    childFormCGHT.intUserID = intUserID;
                    childFormCGHT.intUserLimit = intUserLimit;
                    childFormCGHT.strUserLimit = strUserLimit;
                    childFormCGHT.strUserName = strUserName;
                    childFormCGHT.Show();
                    break;
                case "XS":
                    // 创建此子窗体的一个新实例。
                    FormXSHT childFormXSHT = new FormXSHT();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSHT.MdiParent = this.MdiParent;

                    childFormXSHT.strConn = strConn;
                    childFormXSHT.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSHT.isSaved = true;

                    childFormXSHT.intUserID = intUserID;
                    childFormXSHT.intUserLimit = intUserLimit;
                    childFormXSHT.strUserLimit = strUserLimit;
                    childFormXSHT.strUserName = strUserName;
                    childFormXSHT.Show();
                    break;
            }

            switch (sTemp.Substring(0, 3))
            {
                case "AKP":
                    // 创建此子窗体的一个新实例。
                    FormGJSPZD childFormGJSPZD = new FormGJSPZD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormGJSPZD.MdiParent = this.MdiParent;

                    childFormGJSPZD.strConn = strConn;
                    childFormGJSPZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormGJSPZD.isSaved = true;

                    childFormGJSPZD.intUserID = intUserID;
                    childFormGJSPZD.intUserLimit = intUserLimit;
                    childFormGJSPZD.strUserLimit = strUserLimit;
                    childFormGJSPZD.strUserName = strUserName;
                    childFormGJSPZD.Show();
                    break;

                case "ADH":
                    // 创建此子窗体的一个新实例。
                    FormJHRKYHD childFormJHRKYHD = new FormJHRKYHD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormJHRKYHD.MdiParent = this.MdiParent;

                    childFormJHRKYHD.strConn = strConn;
                    childFormJHRKYHD.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHRKYHD.isSaved = true;

                    childFormJHRKYHD.intUserID = intUserID;
                    childFormJHRKYHD.intUserLimit = intUserLimit;
                    childFormJHRKYHD.strUserLimit = strUserLimit;
                    childFormJHRKYHD.strUserName = strUserName;
                    childFormJHRKYHD.Show();
                    break;

                case "ATH":
                    // 创建此子窗体的一个新实例。
                    FormJHTCZD childFormJHTCZD = new FormJHTCZD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormJHTCZD.MdiParent = this.MdiParent;

                    childFormJHTCZD.strConn = strConn;
                    childFormJHTCZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHTCZD.isSaved = true;

                    childFormJHTCZD.intUserID = intUserID;
                    childFormJHTCZD.intUserLimit = intUserLimit;
                    childFormJHTCZD.strUserLimit = strUserLimit;
                    childFormJHTCZD.strUserName = strUserName;
                    childFormJHTCZD.Show();
                    break;

                case "ATB":
                    // 创建此子窗体的一个新实例。
                    FormJHTBJDJ childFormJHTBJDJ = new FormJHTBJDJ();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormJHTBJDJ.MdiParent = this.MdiParent;

                    childFormJHTBJDJ.strConn = strConn;
                    childFormJHTBJDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHTBJDJ.isSaved = true;

                    childFormJHTBJDJ.intUserID = intUserID;
                    childFormJHTBJDJ.intUserLimit = intUserLimit;
                    childFormJHTBJDJ.strUserLimit = strUserLimit;
                    childFormJHTBJDJ.strUserName = strUserName;
                    childFormJHTBJDJ.Show();
                    break;

                case "AYF":
                    // 创建此子窗体的一个新实例。
                    FormYFZKJS childFormYFZKJS = new FormYFZKJS();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormYFZKJS.MdiParent = this.MdiParent;

                    childFormYFZKJS.strConn = strConn;
                    childFormYFZKJS.iDJID = Convert.ToInt32(sTemp1);
                    childFormYFZKJS.isSaved = true;

                    childFormYFZKJS.intUserID = intUserID;
                    childFormYFZKJS.intUserLimit = intUserLimit;
                    childFormYFZKJS.strUserLimit = strUserLimit;
                    childFormYFZKJS.strUserName = strUserName;
                    childFormYFZKJS.Show();
                    break;

                case "BKP":
                    // 创建此子窗体的一个新实例。
                    FormXSCKZD childFormXSCKZD = new FormXSCKZD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSCKZD.MdiParent = this.MdiParent;

                    childFormXSCKZD.strConn = strConn;
                    childFormXSCKZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSCKZD.isSaved = true;

                    childFormXSCKZD.intUserID = intUserID;
                    childFormXSCKZD.intUserLimit = intUserLimit;
                    childFormXSCKZD.strUserLimit = strUserLimit;
                    childFormXSCKZD.strUserName = strUserName;
                    childFormXSCKZD.Show();
                    break;

                case "BCK":
                    // 创建此子窗体的一个新实例。
                    FormXSCKJD childFormXSCKJD = new FormXSCKJD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSCKJD.MdiParent = this.MdiParent;

                    childFormXSCKJD.strConn = strConn;
                    childFormXSCKJD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSCKJD.isSaved = true;

                    childFormXSCKJD.intUserID = intUserID;
                    childFormXSCKJD.intUserLimit = intUserLimit;
                    childFormXSCKJD.strUserLimit = strUserLimit;
                    childFormXSCKJD.strUserName = strUserName;
                    childFormXSCKJD.Show();
                    break;

                case "BTH":
                    // 创建此子窗体的一个新实例。
                    FormXSTHZD childFormXSTHZD = new FormXSTHZD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSTHZD.MdiParent = this.MdiParent;

                    childFormXSTHZD.strConn = strConn;
                    childFormXSTHZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSTHZD.isSaved = true;

                    childFormXSTHZD.intUserID = intUserID;
                    childFormXSTHZD.intUserLimit = intUserLimit;
                    childFormXSTHZD.strUserLimit = strUserLimit;
                    childFormXSTHZD.strUserName = strUserName;
                    childFormXSTHZD.Show();
                    break;

                case "BTB":
                    // 创建此子窗体的一个新实例。
                    FormXSTBJDJ childFormXSTBJDJ = new FormXSTBJDJ();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSTBJDJ.MdiParent = this.MdiParent;

                    childFormXSTBJDJ.strConn = strConn;
                    childFormXSTBJDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSTBJDJ.isSaved = true;

                    childFormXSTBJDJ.intUserID = intUserID;
                    childFormXSTBJDJ.intUserLimit = intUserLimit;
                    childFormXSTBJDJ.strUserLimit = strUserLimit;
                    childFormXSTBJDJ.strUserName = strUserName;
                    childFormXSTBJDJ.Show();
                    break;

                case "BYS":
                    // 创建此子窗体的一个新实例。
                    FormYSZKJS childFormYSZKJS = new FormYSZKJS();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormYSZKJS.MdiParent = this.MdiParent;

                    childFormYSZKJS.strConn = strConn;
                    childFormYSZKJS.iDJID = Convert.ToInt32(sTemp1);
                    childFormYSZKJS.isSaved = true;

                    childFormYSZKJS.intUserID = intUserID;
                    childFormYSZKJS.intUserLimit = intUserLimit;
                    childFormYSZKJS.strUserLimit = strUserLimit;
                    childFormYSZKJS.strUserName = strUserName;
                    childFormYSZKJS.Show();
                    break;

                case "CPD":
                    // 创建此子窗体的一个新实例。
                    FormKCSPPD2 childFormKCSPPD2 = new FormKCSPPD2();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormKCSPPD2.MdiParent = this.MdiParent;

                    childFormKCSPPD2.strConn = strConn;
                    childFormKCSPPD2.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCSPPD2.isSaved = true;

                    childFormKCSPPD2.intUserID = intUserID;
                    childFormKCSPPD2.intUserLimit = intUserLimit;
                    childFormKCSPPD2.strUserLimit = strUserLimit;
                    childFormKCSPPD2.strUserName = strUserName;
                    childFormKCSPPD2.Show();
                    break;

                case "CBS":
                    // 创建此子窗体的一个新实例。
                    FormKCSPBSCL childFormKCSPBSCL = new FormKCSPBSCL();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormKCSPBSCL.MdiParent = this.MdiParent;

                    childFormKCSPBSCL.strConn = strConn;
                    childFormKCSPBSCL.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCSPBSCL.isSaved = true;

                    childFormKCSPBSCL.intUserID = intUserID;
                    childFormKCSPBSCL.intUserLimit = intUserLimit;
                    childFormKCSPBSCL.strUserLimit = strUserLimit;
                    childFormKCSPBSCL.strUserName = strUserName;
                    childFormKCSPBSCL.Show();
                    break;

                case "CCK":
                    // 创建此子窗体的一个新实例。
                    FormKCJWCKDJ childFormKCJWCKDJ = new FormKCJWCKDJ();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormKCJWCKDJ.MdiParent = this.MdiParent;

                    childFormKCJWCKDJ.strConn = strConn;
                    childFormKCJWCKDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCJWCKDJ.isSaved = true;

                    childFormKCJWCKDJ.intUserID = intUserID;
                    childFormKCJWCKDJ.intUserLimit = intUserLimit;
                    childFormKCJWCKDJ.strUserLimit = strUserLimit;
                    childFormKCJWCKDJ.strUserName = strUserName;
                    childFormKCJWCKDJ.Show();
                    break;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 3:
                    toolStripStatusLabelC.Text = "数量合计："+cTemp[3].ToString("f0")+" 金额合计："+cTemp1[3].ToString("f2");
                    break;
                case 4:
                    toolStripStatusLabelC.Text = "数量合计：" + cTemp[4].ToString("f0") + " 金额合计：" + cTemp1[4].ToString("f2");
                    break;
                case 5:
                    toolStripStatusLabelC.Text = "数量合计：" + cTemp[5].ToString("f0") + " 金额合计：" + cTemp1[5].ToString("f2");
                    break;
                case 7:
                    toolStripStatusLabelC.Text = "数量合计：" + cTemp[7].ToString("f0");
                    break;
                case 8:
                    toolStripStatusLabelC.Text = "数量合计：" + cTemp[8].ToString("f0");
                    break;
                case 9:
                    toolStripStatusLabelC.Text = "数量合计：" + cTemp[9].ToString("f0");
                    break;
                case 10:
                    toolStripStatusLabelC.Text = "数量合计：" + cTemp[10].ToString("f0") + " 金额合计：" + cTemp1[10].ToString("f2");
                    break;
                case 11:
                    toolStripStatusLabelC.Text = "数量合计：" + cTemp[11].ToString("f0") + " 金额合计：" + cTemp1[11].ToString("f2");
                    break;
                case 12:
                    toolStripStatusLabelC.Text = "数量合计：" + cTemp[12].ToString("f0") + " 金额合计：" + cTemp1[12].ToString("f2");
                    break;
                case 13:
                    toolStripStatusLabelC.Text = "数量合计：" + cTemp[13].ToString("f0") + " 金额合计：" + cTemp1[13].ToString("f2");
                    break;
                default:
                    toolStripStatusLabelC.Text = "";
                    break;
            }
        }



    }
}