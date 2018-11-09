using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormFPCX : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private int iSupplyCompany = 0;

        private ClassGetInformation cGetInformation;
        public int LIMITACCESS = 18;
        public int LIMITACCESS1 = 5;
        public int LIMITACCESS2 = 10;

        private bool isSaved = false;
        
        public FormFPCX()
        {
            InitializeComponent();
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1000, "") == 0)
            {
                //return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                checkBoxAll.Checked = false;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1200, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                    checkBoxAll.Checked = true;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    checkBoxAll.Checked = false;
                }
            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1300, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                    checkBoxAll.Checked = true;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    checkBoxAll.Checked = false;
                }
            }
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAll.Checked)
            {
                iSupplyCompany = 0;
                textBoxDWBH.Text = "";
                textBoxDWMC.Text = "";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            bool bFP = true;
            decimal dt1, dt2;
            int i;
            string strA,strB,strC;

            switch (comboBoxStyle.SelectedIndex)
            {
                case 0: //购进
                    sqlComm.CommandText = "SELECT 发票汇总表.ID, 发票汇总表.发票号, 发票汇总表.日期, 发票汇总表.原开票金额,发票汇总表.发票总额, 单位表.单位名称, 发票汇总表.发货方式, 发票汇总表.单号, 发票汇总表.备注 FROM 发票汇总表 INNER JOIN 单位表 ON 发票汇总表.单位ID = 单位表.ID WHERE (发票汇总表.BeActive = 1) AND (发票汇总表.日期 >= CONVERT(DATETIME,  '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (发票汇总表.日期 <= CONVERT(DATETIME,  '" + dateTimePickerE.Value.ToShortDateString() + " 00:00:00', 102)) AND (发票汇总表.发票类型 = 0)";

                    if (!(intUserLimit > LIMITACCESS))
                    {
                        sqlComm.CommandText += " AND ((单位表.业务员 = N'" + strUserName + "') OR (发票汇总表.操作员ID = "+intUserID.ToString()+"))";
                    }
                    
                    break;
                case 1:
                    sqlComm.CommandText = "SELECT 发票汇总表.ID, 发票汇总表.发票号, 发票汇总表.日期, 发票汇总表.原开票金额,发票汇总表.发票总额, 单位表.单位名称, 发票汇总表.发货方式, 发票汇总表.单号, 发票汇总表.备注 FROM 发票汇总表 INNER JOIN 单位表 ON 发票汇总表.单位ID = 单位表.ID WHERE (发票汇总表.BeActive = 1) AND (发票汇总表.日期 >= CONVERT(DATETIME,  '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (发票汇总表.日期 <= CONVERT(DATETIME,  '" + dateTimePickerE.Value.ToShortDateString() + " 00:00:00', 102)) AND (发票汇总表.发票类型 = 1)";

                    if (!(intUserLimit > LIMITACCESS))
                    {
                        sqlComm.CommandText += " AND ((单位表.业务员 = N'" + strUserName + "') OR (发票汇总表.操作员ID = " + intUserID.ToString() + "))";
                    }
                    break;
                case 2:
                    sqlComm.CommandText = "SELECT 发票汇总表.ID, 发票汇总表.发票号, 发票汇总表.日期, 发票汇总表.原开票金额,发票汇总表.发票总额, 单位表.单位名称, 发票汇总表.发货方式, 发票汇总表.单号, 发票汇总表.备注 FROM 发票汇总表 INNER JOIN 单位表 ON 发票汇总表.单位ID = 单位表.ID WHERE (发票汇总表.BeActive = 0) AND (发票汇总表.日期 >= CONVERT(DATETIME,  '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (发票汇总表.日期 <= CONVERT(DATETIME,  '" + dateTimePickerE.Value.ToShortDateString() + " 00:00:00', 102)) ";

                    if (!(intUserLimit > LIMITACCESS))
                    {
                        sqlComm.CommandText += " AND ((单位表.业务员 = N'" + strUserName + "') OR (发票汇总表.操作员ID = " + intUserID.ToString() + "))";
                    }
                    break;
                case 3:
                    strA = "(SELECT 进货入库汇总表.单据编号, 进货入库汇总表.ID, 购进商品制单表.单据编号 AS 冲抵单号, 进货入库汇总表.价税合计, 进货入库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 进货入库汇总表.发票号 FROM 进货入库汇总表 INNER JOIN 购进商品制单表 ON 进货入库汇总表.购进ID = 购进商品制单表.ID INNER JOIN 单位表 ON 进货入库汇总表.单位ID = 单位表.ID WHERE (进货入库汇总表.BeActive = 1) AND (进货入库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (进货入库汇总表.发票号 IS NULL OR 进货入库汇总表.发票号 = N'')";
                    if (iSupplyCompany != 0)
                        strA += " AND (进货入库汇总表.单位ID = " + iSupplyCompany.ToString() + ")";

                    if (intUserLimit < LIMITACCESS)
                    {
                        strA += " AND ((进货入库汇总表.业务员ID = " + intUserID.ToString() + ") OR (进货入库汇总表.操作员ID = " + intUserID.ToString() + "))";
                    }
                    strA += ")";

                    strB = "(SELECT 进货退出汇总表.单据编号,进货退出汇总表.ID, 进货退出汇总表.单据编号 AS 冲抵单号, -1.0*进货退出汇总表.价税合计, 进货退出汇总表.日期, 单位表.单位编号,  单位表.单位名称, 进货退出汇总表.发票号 FROM 进货退出汇总表 INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID WHERE (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (进货退出汇总表.发票号 IS NULL OR 进货退出汇总表.发票号 = N'')";
                    if (iSupplyCompany != 0)
                        strB += " AND (进货退出汇总表.单位ID = " + iSupplyCompany.ToString() + ")";
                    if (intUserLimit < LIMITACCESS)
                    {
                        strB += " AND ((进货退出汇总表.业务员ID = " + intUserID.ToString() + ") OR (进货退出汇总表.操作员ID = " + intUserID.ToString() + "))";
                    }
                    strB += ")";


                    strC = "(SELECT 购进退补差价汇总表.单据编号, 购进退补差价汇总表.ID,购进退补差价汇总表.单据编号 AS 冲抵单号, 购进退补差价汇总表.价税合计, 购进退补差价汇总表.日期, 单位表.单位编号,  单位表.单位名称, 购进退补差价汇总表.发票号 FROM 购进退补差价汇总表 INNER JOIN 单位表 ON 购进退补差价汇总表.单位ID = 单位表.ID WHERE (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (购进退补差价汇总表.发票号 IS NULL OR 购进退补差价汇总表.发票号 = N'')";
                    if (iSupplyCompany != 0)
                        strC += " AND (购进退补差价汇总表.单位ID = " + iSupplyCompany.ToString() + ")";
                    if (intUserLimit < LIMITACCESS)
                    {
                        strC += " AND ((购进退补差价汇总表.业务员ID = " + intUserID.ToString() + ") OR (购进退补差价汇总表.操作员ID = " + intUserID.ToString() + "))";
                    }
                    strC += ")";
                    sqlComm.CommandText = strA + " UNION " + strB + " UNION " + strC;
                    bFP = false;
                    break;
                case 4:
                    strA = "(SELECT 销售商品制单表.单据编号,销售商品制单表.ID,  销售商品制单表.价税合计, 销售商品制单表.日期, 单位表.单位编号,  单位表.单位名称, 销售商品制单表.发票号 FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID WHERE (销售商品制单表.BeActive = 1) AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售商品制单表.发票号 IS NULL OR 销售商品制单表.发票号 = N'')";
                    if (iSupplyCompany != 0)
                        strA += " AND (销售商品制单表.单位ID = " + iSupplyCompany.ToString() + ")";
                    if (intUserLimit < LIMITACCESS)
                    {
                        strA += " AND ((销售商品制单表.业务员ID = " + intUserID.ToString() + ") OR (销售商品制单表.操作员ID = " + intUserID.ToString() + "))";
                    }
                    strA += ")";

                    strB = "(SELECT 销售退出汇总表.单据编号, 销售退出汇总表.ID, -1.0*销售退出汇总表.价税合计, 销售退出汇总表.日期, 单位表.单位编号,  单位表.单位名称, 销售退出汇总表.发票号 FROM 销售退出汇总表 INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退出汇总表.发票号 IS NULL OR 销售退出汇总表.发票号 = N'')";
                    if (iSupplyCompany != 0)
                        strB += " AND (销售退出汇总表.单位ID = " + iSupplyCompany.ToString() + ")";
                    if (intUserLimit < LIMITACCESS)
                    {
                        strB += " AND ((销售退出汇总表.业务员ID = " + intUserID.ToString() + ") OR (销售退出汇总表.操作员ID = " + intUserID.ToString() + "))";
                    }
                    strB += ")";


                    strC = "(SELECT 销售退补差价汇总表.单据编号, 销售退补差价汇总表.ID, 销售退补差价汇总表.价税合计, 销售退补差价汇总表.日期, 单位表.单位编号,  单位表.单位名称, 销售退补差价汇总表.发票号 FROM 销售退补差价汇总表 INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退补差价汇总表.发票号 IS NULL OR 销售退补差价汇总表.发票号 = N'')";
                    if (iSupplyCompany != 0)
                        strC += " AND (销售退补差价汇总表.单位ID = " + iSupplyCompany.ToString() + ")";
                    if (intUserLimit < LIMITACCESS)
                    {
                        strC += " AND ((销售退补差价汇总表.业务员ID = " + intUserID.ToString() + ") OR (销售退补差价汇总表.操作员ID = " + intUserID.ToString() + "))";
                    }
                    strC += ")";
                    sqlComm.CommandText = strA + " UNION " + strB + " UNION " + strC;
                    bFP = false;
                    break;

            }

            if (bFP)
            {
                //dataGridViewDJMX.CellDoubleClick += dataGridViewDJMX_CellDoubleClick;
                if (iSupplyCompany != 0)
                    sqlComm.CommandText += "  AND (发票汇总表.单位ID = " + iSupplyCompany.ToString() + ")";

                if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                sqlDA.Fill(dSet, "单据明细表");
                dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

                dataGridViewDJMX.Columns[0].Visible = false;
                dataGridViewDJMX.Columns[1].Visible = true;

                dt1 = 0; dt2 = 0;
                for (i = 0; i < dataGridViewDJMX.RowCount; i++)
                {
                    try
                    {
                        dt1 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[3].Value.ToString());
                        dt2 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[4].Value.ToString());
                    }
                    catch
                    {
                    }
                }

                for (i = 1; i < dataGridViewDJMX.ColumnCount; i++)
                {
                    dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                }
                dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";

                sqlConn.Close();
                toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString() + " 原开票金额合计：" + dt1.ToString("f2") + " 发票总额合计：" + dt2.ToString("f2");
            }
            else
            {
                //dataGridViewDJMX.CellDoubleClick -= dataGridViewDJMX_CellDoubleClick;

                dataGridViewDJMX.DataSource = null;
                if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                sqlDA.Fill(dSet, "单据明细表");
                dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

                dataGridViewDJMX.Columns[0].Visible = true;
                dataGridViewDJMX.Columns[1].Visible = false;

                dt1 = 0;
                for (i = 0; i < dataGridViewDJMX.RowCount; i++)
                {
                    try
                    {
                        if (comboBoxStyle.SelectedIndex == 3)
                        {
                            dt1 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[3].Value.ToString());
                            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
                        }
                        else
                        {
                            dt1 += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[2].Value.ToString());
                            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f2";
                        }
                    }
                    catch
                    {
                    }
                }

                for (i = 1; i < dataGridViewDJMX.ColumnCount; i++)
                {
                    dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                }
                

                sqlConn.Close();
                toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString() + " 金额合计：" + dt1.ToString("f2");
                sqlConn.Close();
            }

        }

        private void FormFPCX_Load(object sender, EventArgs e)
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

            comboBoxStyle.SelectedIndex = 0;
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 公司宣传, 质量目标1, 质量目标2, 质量目标3, 质量目标4, 管理员权限, 总经理权限, 职员权限, 经理权限, 业务员权限 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {

                try
                {
                    LIMITACCESS = int.Parse(sqldr.GetValue(8).ToString());
                }
                catch
                {
                    LIMITACCESS = 15;
                }
            }
            sqldr.Close();
            //得到开始时间
            sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            sqlConn.Close();

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "发票查询;日期：" + labelZDRQ.Text + ";单位名称：" + textBoxDWMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "发票查询;日期：" + labelZDRQ.Text + "单位名称：" + textBoxDWMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
                return;

            if (dataGridViewDJMX.Rows[e.RowIndex].Cells[0].Value.ToString() == "")
                return;

            int iDJID = 0;
            iDJID = Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[0].Value.ToString());

            // 创建此子窗体的一个新实例。
            FormFPKJ childFormFPKJ = new FormFPKJ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormFPKJ.MdiParent = this.MdiParent;

            childFormFPKJ.strConn = strConn;
            childFormFPKJ.iDJID = iDJID;
            childFormFPKJ.isSaved = true;

            childFormFPKJ.intUserID = intUserID;
            childFormFPKJ.intUserLimit = intUserLimit;
            childFormFPKJ.strUserLimit = strUserLimit;
            childFormFPKJ.strUserName = strUserName;

            childFormFPKJ.Show();
        }



        private void dataGridViewDJMX_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count<1)
                return;

            if (dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString() == "")
                return;

            int iDJID = 0;
            bool bHasFP = true;
            try
            {
                iDJID = Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString());
            }
            catch
            {
                bHasFP = false;
            }

            if (bHasFP)
            {
                // 创建此子窗体的一个新实例。
                FormFPKJ childFormFPKJ = new FormFPKJ();
                // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                childFormFPKJ.MdiParent = this.MdiParent;

                childFormFPKJ.strConn = strConn;
                childFormFPKJ.iDJID = iDJID;
                childFormFPKJ.isSaved = true;

                childFormFPKJ.intUserID = intUserID;
                childFormFPKJ.intUserLimit = intUserLimit;
                childFormFPKJ.strUserLimit = strUserLimit;
                childFormFPKJ.strUserName = strUserName;

                childFormFPKJ.Show();
            }
            else //单据明细
            {
                try
                {
                    iDJID = Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[1].Value.ToString());
                }
                catch
                {
                    return;
                }

                DJZX(dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString(), iDJID);
            }
        }


        private void DJZX(string strDJBH, int iDJID)
        {
            string sTemp = "", sTemp1 = "";

            if (strDJBH.Trim() == "")
                return;

            sTemp = strDJBH.Trim().ToUpper();
            sTemp1 = iDJID.ToString();



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
    }
}