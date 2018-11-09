using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPZHCX : Form
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


        private int intCommID = 0, iJZID=0;
        private string SDTS0 = "", SDTS1 = "";

        private decimal[] cTemp = new decimal[8] { 0, 0, 0, 0, 0, 0, 0, 0};
        private decimal[] cTemp1 = new decimal[8] { 0, 0, 0, 0, 0, 0, 0, 0};
        private decimal[] cTemp2 = new decimal[8] { 0, 0, 0, 0, 0, 0, 0, 0};

        private ClassGetInformation cGetInformation;

        public FormSPZHCX()
        {
            InitializeComponent();
        }

        private void FormSPZHCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            //得到上次结转

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
            if (intCommID == 0)
            {
                MessageBox.Show("请选择查询商品。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 结转.结转数量, 结转.结转金额, 商品表.库存数量, 商品表.库存金额, 商品表.库存成本价 FROM 商品表 LEFT OUTER JOIN (SELECT 结转数量, 结转金额, 商品ID FROM 结转进销存汇总表 WHERE (ID = " + iJZID.ToString()+ ") AND (商品ID = "+intCommID.ToString()+")) 结转 ON 商品表.ID = 结转.商品ID WHERE (商品表.ID = "+intCommID.ToString()+")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelSQJZSL.Text = sqldr.GetValue(0).ToString();
                labelSQJZJE.Text = sqldr.GetValue(1).ToString();
                labelZKCSL.Text = sqldr.GetValue(2).ToString();
                labelJZKCJE.Text = sqldr.GetValue(3).ToString();
                labelCBDJ.Text = sqldr.GetValue(4).ToString();
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT 销售商品制单表.ID, 销售商品制单表.单据编号, 销售商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.数量, 销售商品制单明细表.单价, 销售商品制单明细表.扣率, 销售商品制单明细表.实计金额, 销售商品制单明细表.赠品, 销售商品制单明细表.毛利 FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID INNER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID WHERE (销售商品制单表.BeActive = 1) AND (商品表.ID = " + intCommID.ToString() + ") AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 销售商品制单表.日期 DESC";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");


            sqlComm.CommandText = "SELECT 购进商品制单表.ID, 购进商品制单表.单据编号, 购进商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 购进商品制单明细表.数量, 购进商品制单明细表.单价, 购进商品制单明细表.金额, 购进商品制单明细表.扣率, 购进商品制单明细表.实计金额 FROM 商品表 INNER JOIN 单位表 INNER JOIN 库房表 INNER JOIN 购进商品制单明细表 ON 库房表.ID = 购进商品制单明细表.库房ID INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID ON 单位表.ID = 购进商品制单表.单位ID ON 商品表.ID = 购进商品制单明细表.商品ID WHERE (购进商品制单表.BeActive = 1) AND (商品表.ID = " + intCommID.ToString() + ") AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 购进商品制单表.日期 DESC";

            if (dSet.Tables.Contains("商品表2")) dSet.Tables.Remove("商品表2");
            sqlDA.Fill(dSet, "商品表2");



            sqlComm.CommandText = "SELECT 销售退出汇总表.ID, 销售退出汇总表.单据编号, 销售退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 销售退出明细表.数量, 销售退出明细表.单价, 销售退出明细表.实计金额, (销售退出明细表.实计金额-销售退出明细表.数量*销售退出明细表.库存成本价) AS 毛利 FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID INNER JOIN 库房表 ON 销售退出明细表.库房ID = 库房表.ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.BeActive = 1) AND (商品表.ID = " + intCommID.ToString() + ") AND (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 销售退出汇总表.日期 DESC";

            if (dSet.Tables.Contains("商品表3")) dSet.Tables.Remove("商品表3");
            sqlDA.Fill(dSet, "商品表3");

            sqlComm.CommandText = "SELECT 进货退出汇总表.ID, 进货退出汇总表.单据编号, 进货退出汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 进货退出明细表.数量, 进货退出明细表.单价, 进货退出明细表.实计金额 FROM 进货退出明细表 INNER JOIN 进货退出汇总表 ON 进货退出明细表.单据ID = 进货退出汇总表.ID INNER JOIN 库房表 ON 进货退出明细表.库房ID = 库房表.ID INNER JOIN 商品表 ON 进货退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID WHERE (进货退出汇总表.BeActive = 1) AND (商品表.ID = " + intCommID.ToString() + ") AND (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 进货退出汇总表.日期 DESC";

            if (dSet.Tables.Contains("商品表4")) dSet.Tables.Remove("商品表4");
            sqlDA.Fill(dSet, "商品表4");


            sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 借物出库明细表.数量, 借物出库明细表.单价, 借物出库明细表.出库金额 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID INNER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID INNER JOIN 商品表 ON 借物出库明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID WHERE (借物出库汇总表.BeActive = 1) AND (商品表.ID = " + intCommID.ToString() + ") AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 借物出库汇总表.日期 DESC";

            if (dSet.Tables.Contains("商品表5")) dSet.Tables.Remove("商品表5");
            sqlDA.Fill(dSet, "商品表5");

            sqlComm.CommandText = "SELECT 库房表.库房编号, 库房表.库房名称, 库存表.库存成本价, 库存表.库存数量, 库存表.库存金额, 库存表.合理库存上限, 库存表.合理库存下限 FROM 商品表 INNER JOIN 库存表 INNER JOIN 库房表 ON 库存表.库房ID = 库房表.ID ON 商品表.ID = 库存表.商品ID WHERE (商品表.ID = " + intCommID.ToString() + ")";

            if (dSet.Tables.Contains("商品表6")) dSet.Tables.Remove("商品表6");
            sqlDA.Fill(dSet, "商品表6");

            sqlComm.CommandText = "SELECT 购进退补差价汇总表.ID, 购进退补差价汇总表.单据编号, 购进退补差价汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 购进退补差价明细表.补价数量, 购进退补差价明细表.差价, 购进退补差价明细表.金额 FROM 购进退补差价汇总表 INNER JOIN 购进退补差价明细表 ON 购进退补差价汇总表.ID = 购进退补差价明细表.单据ID INNER JOIN 单位表 ON 购进退补差价汇总表.单位ID = 单位表.ID INNER JOIN 库房表 ON 购进退补差价明细表.库房ID = 库房表.ID WHERE (购进退补差价明细表.商品ID = " + intCommID.ToString() + ") AND (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 购进退补差价汇总表.日期 DESC";

            if (dSet.Tables.Contains("商品表7")) dSet.Tables.Remove("商品表7");
            sqlDA.Fill(dSet, "商品表7");

            sqlComm.CommandText = "SELECT 销售退补差价汇总表.ID, 销售退补差价汇总表.单据编号, 销售退补差价汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 销售退补差价明细表.补价数量, 销售退补差价明细表.差价, 销售退补差价明细表.金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID INNER JOIN 库房表 ON 销售退补差价明细表.库房ID = 库房表.ID WHERE (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") AND (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ORDER BY 销售退补差价汇总表.日期 DESC";
            if (dSet.Tables.Contains("商品表8")) dSet.Tables.Remove("商品表8");
            sqlDA.Fill(dSet, "商品表8");

            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["商品表1"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView2.DataSource = dSet.Tables["商品表2"];
            dataGridView2.Columns[0].Visible = false;
            dataGridView3.DataSource = dSet.Tables["商品表3"];
            dataGridView3.Columns[0].Visible = false;
            dataGridView4.DataSource = dSet.Tables["商品表4"];
            dataGridView4.Columns[0].Visible = false;
            dataGridView5.DataSource = dSet.Tables["商品表5"];
            dataGridView5.Columns[0].Visible = false;
            dataGridView6.DataSource = dSet.Tables["商品表6"];
            dataGridView7.DataSource = dSet.Tables["商品表7"];
            dataGridView7.Columns[0].Visible = false;
            dataGridView8.DataSource = dSet.Tables["商品表8"];
            dataGridView8.Columns[0].Visible = false;

            dataGridView1.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[10].DefaultCellStyle.Format = "f2";
            dataGridView2.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridView3.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView4.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView4.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView5.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView5.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView6.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridView6.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridView7.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView7.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView8.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridView8.Columns[9].DefaultCellStyle.Format = "f2";
            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "商品综合查询（销售明细）;当前日期：" + labelZDRQ.Text+";商品："+textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "商品综合查询（购进明细）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "商品综合查询（库房明细）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, true, intUserLimit);
                    break;

            }

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "商品综合查询（销售明细）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "商品综合查询（购进明细）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "商品综合查询（库房明细）;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, false, intUserLimit);
                    break;

            }
        }

        private void dataGridV_DoubleClick(object sender, EventArgs e)
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

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {

        }

        private void countfTemp()
        {
            int c = 0, c1 = 0, c2=0;
            int i, j;

            for (i = 1; i <= cTemp.Length; i++)
            {
                cTemp[i - 1] = 0;
                cTemp1[i - 1] = 0;
                cTemp2[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 7; c1 = 10;c2 = 12;
                        break;
                    case 2:
                        c = 7; c1 = 11; c2 = 0;
                        break;
                    case 3:
                        c = 7; c1 = 9; c2 = 10;
                        break;
                    case 4:
                        c = 7; c1 = 9; c2 = 0;
                        break;
                    case 5:
                        c = 7; c1 = 9; c2 = 0;
                        break;
                    case 6:
                        c = 3; c1 = 4; c2 = 0;
                        break;
                    case 7:
                        c = 7; c1 = 9; c2 = 0;
                        break;
                    case 8:
                        c = 7; c1 = 9; c2 = 0;
                        break;
                    default:
                        c = 0;
                        break;
                }

                if (c != 0)
                {

                    for (j = 0; j < dSet.Tables["商品表" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp[i - 1] += decimal.Parse(dSet.Tables["商品表" + i.ToString()].Rows[j][c].ToString());
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
                            cTemp1[i - 1] += decimal.Parse(dSet.Tables["商品表" + i.ToString()].Rows[j][c1].ToString());
                        }
                        catch
                        {
                        }
                    }
                }
                else
                    cTemp1[i - 1] = -1;

                if (c2 != 0)
                {

                    for (j = 0; j < dSet.Tables["商品表" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp2[i - 1] += decimal.Parse(dSet.Tables["商品表" + i.ToString()].Rows[j][c2].ToString());
                        }
                        catch
                        {
                        }
                    }
                }
                else
                    cTemp2[i - 1] = -1;


            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;
            if (!dSet.Tables.Contains("商品表" + c1.ToString()))
                return;
            toolStripStatusLabelZH.Text = "共有" + dSet.Tables["商品表" + c1.ToString()].Rows.Count.ToString() + "条记录 数量合计" + cTemp[tabControl1.SelectedIndex].ToString("f0") + " 金额合计" + cTemp1[tabControl1.SelectedIndex].ToString("f2") + "元";

            if (cTemp2[tabControl1.SelectedIndex] != -1)
                toolStripStatusLabelZH.Text += " 毛利 " + cTemp2[tabControl1.SelectedIndex].ToString("f2");
        }


    }
}