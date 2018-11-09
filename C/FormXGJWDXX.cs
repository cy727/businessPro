using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace business
{
    public partial class FormXGJWDXX : Form
    {

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();
        private System.Data.DataSet dSetP1 = new DataSet();

        public string strConn = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        public string strSelect = "";

        private int iSupplyCompany = 0;
        private int intHTH = 0;
        private int intBKP = 0;

        private ClassGetInformation cGetInformation;
        private bool isSaved = false;

        private int RowPos;              // Position of currently printing row 
        private bool NewPage;            // Indicates if a new page reached
        private int PageNo;

        private string sGSMC = "";
        private string sGSDZ = "";
        private string sGSDH = "";
        private string sGSCZ = "";
        private string sGSYB = "";
        private string sGSZH = "";
        private string sGSKHYH = "";
        private string sGSSH = "";

        private const int iPageZX = 20; //装箱单个数
        private const int iPageNZX = 10;

        public FormXGJWDXX()
        {
            InitializeComponent();
        }

        private void FormXGJWDXX_Load(object sender, EventArgs e)
        {
            int i;
            this.Top = 1;
            this.Left = 1;


            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 公司名, 地址, 电话, 传真, 税号, 开户银行, 帐号, 邮政编码, 开始时间, 负责人 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                sGSMC = sqldr.GetValue(0).ToString();
                sGSDZ = sqldr.GetValue(1).ToString();
                sGSDH = sqldr.GetValue(2).ToString();
                sGSCZ = sqldr.GetValue(3).ToString();
                sGSYB = sqldr.GetValue(7).ToString();
                sGSZH = sqldr.GetValue(6).ToString();
                sGSKHYH = sqldr.GetValue(5).ToString();
                sGSSH = sqldr.GetValue(4).ToString();
            }
            sqldr.Close();

            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.Text = strUserName;

            //初始化部门列表
            comboBoxBM.SelectedIndexChanged -= comboBoxBM_SelectedIndexChanged;
            sqlComm.CommandText = "SELECT ID, 部门名称 FROM 部门表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("部门表")) dSet.Tables.Remove("部门表");
            sqlDA.Fill(dSet, "部门表");

            DataRow drTemp = dSet.Tables["部门表"].NewRow();
            drTemp[0] = 0;
            drTemp[1] = "全部";
            dSet.Tables["部门表"].Rows.Add(drTemp);

            comboBoxBM.DataSource = dSet.Tables["部门表"];
            comboBoxBM.DisplayMember = "部门名称";
            comboBoxBM.ValueMember = "ID";
            comboBoxBM.SelectedValue = intUserBM;;
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;


            sqlConn.Close();

            //initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }

        private void comboBoxBM_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            sqlConn.Open();
            //初始化员工列表
            if (comboBoxBM.Text.Trim() != "全部")
                sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员姓名, 职员表.职员编号 FROM 职员表 INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (部门表.部门名称 = N'" + comboBoxBM.Text.Trim() + "') AND (职员表.beactive = 1)";
            else
                sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员姓名, 职员表.职员编号 FROM 职员表 INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (职员表.beactive = 1)";

            sqldr = sqlComm.ExecuteReader();
            if (!sqldr.HasRows)
            {
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            sqldr.Close();

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            sqlConn.Close();
             */
        }

        private void textBoxHTH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getBillInformation(3, "") == 0)
            {
                return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iBillCNumber;
                textBoxDWMC.Text = cGetInformation.strBillCName;
                textBoxDWBH.Text = cGetInformation.strBillCCode;
                textBoxHTH.Text = cGetInformation.strBillCode;
                intBKP = cGetInformation.iBillNumber;
                comboBoxBM.SelectedValue = cGetInformation.iBillBMID;
                comboBoxYWY.Text = cGetInformation.sPeopleName;

                strSelect = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库房表.库房编号, 库房表.库房名称, 借物出库明细表.数量, 借物出库明细表.单价, 借物出库明细表.金额, 借物出库明细表.出库金额 FROM 借物出库明细表 INNER JOIN 商品表 ON 借物出库明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID WHERE (借物出库明细表.BeActive = 1) AND (借物出库明细表.表单ID = " + cGetInformation.iBillNumber + ")";

                initDJDtail();
                initdataGridViewDJMX();
            }
        }

        private void initDJDtail()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 借物出库汇总表.单位ID, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.联系电话, 借物出库汇总表.联系人, 借物出库汇总表.收货人, 借物出库汇总表.到站, 借物出库汇总表.运输方式, 借物出库汇总表.详细地址, 借物出库汇总表.物流名称, 借物出库汇总表.单号, 借物出库汇总表.邮政编码, 职员表.职员姓名 AS 业务员, [职员表_1].职员姓名 AS 操作员, 借物出库汇总表.单据编号, 借物出库汇总表.备注 FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 借物出库汇总表.操作员ID = [职员表_1].ID WHERE (借物出库汇总表.ID = " + intBKP.ToString() + ")";

            if (dSet.Tables.Contains("送货表")) dSet.Tables.Remove("送货表");
            sqlDA.Fill(dSet, "送货表");

            if (dSet.Tables["送货表"].Rows.Count < 1)
            {
                textBoxLXDH.Text = "";
                textBoxLXR.Text = "";
                textBoxSHR.Text = "";
                textBoxDZ.Text = "";
                comboBoxYSFS.Text = "";
                textBoxXXDZ.Text = "";
                textBoxWLMC.Text = "";
                textBoxDH.Text = "";
                textBoxYZBM.Text = "";
                labelYYWY.Text = "";
                labelYCZY.Text = "";
                labelYDJBH.Text = "";
                textBoxBZ.Text = "";
                intBKP = 0;
            }
            else
            {
                textBoxLXDH.Text = dSet.Tables["送货表"].Rows[0][3].ToString();
                textBoxLXR.Text = dSet.Tables["送货表"].Rows[0][4].ToString();
                textBoxSHR.Text = dSet.Tables["送货表"].Rows[0][5].ToString();
                textBoxDZ.Text = dSet.Tables["送货表"].Rows[0][6].ToString();
                comboBoxYSFS.Text = dSet.Tables["送货表"].Rows[0][7].ToString();
                textBoxXXDZ.Text = dSet.Tables["送货表"].Rows[0][8].ToString();
                textBoxWLMC.Text = dSet.Tables["送货表"].Rows[0][9].ToString();
                textBoxDH.Text = dSet.Tables["送货表"].Rows[0][10].ToString();
                textBoxYZBM.Text = dSet.Tables["送货表"].Rows[0][11].ToString();
                labelYYWY.Text = dSet.Tables["送货表"].Rows[0][12].ToString();
                labelYCZY.Text = dSet.Tables["送货表"].Rows[0][13].ToString();
                labelYDJBH.Text = dSet.Tables["送货表"].Rows[0][14].ToString();
                textBoxBZ.Text = dSet.Tables["送货表"].Rows[0][15].ToString();

            }
            sqlConn.Close();
        }

        private void initdataGridViewDJMX()
        {
            if (strSelect == "") return;
            sqlConn.Open();
            sqlComm.CommandText = strSelect;

            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据表"];

            sqlConn.Close();


            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";

            countAmount();


        }

        private void textBoxHTH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getBillInformation(30, textBoxHTH.Text.Trim()) == 0)
                {
                    return;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iBillCNumber;
                    textBoxDWMC.Text = cGetInformation.strBillCName;
                    textBoxDWBH.Text = cGetInformation.strBillCCode;
                    textBoxHTH.Text = cGetInformation.strBillCode;
                    intBKP = cGetInformation.iBillNumber;
                    comboBoxBM.SelectedValue = cGetInformation.iBillBMID;
                    comboBoxYWY.Text = cGetInformation.sPeopleName;

                    strSelect = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库房表.库房编号, 库房表.库房名称, 借物出库明细表.数量, 借物出库明细表.单价, 借物出库明细表.金额, 借物出库明细表.出库金额 FROM 借物出库明细表 INNER JOIN 商品表 ON 借物出库明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID WHERE (借物出库明细表.BeActive = 1) AND (借物出库明细表.表单ID = " + cGetInformation.iBillNumber + ")";

                    initDJDtail();
                    initdataGridViewDJMX();
                }
            }
        }

        private void countAmount()
        {
            decimal fSum = 0, fSum1 = 0;
            decimal fCount = 0, fCSum = 0;

            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);
                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);

                fCount += 1;

            }
            labelSLHJ.Text = fCSum.ToString("f0");
            labelJEHJ.Text = fSum.ToString("f2");
            labelCKJE.Text = fSum1.ToString("f2");

            labelDX.Text = cGetInformation.changeDAXIE(labelCKJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();


        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("借物单信息已经保存,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intBKP == 0)
            {
                MessageBox.Show("请选择要修改的单据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (MessageBox.Show("请检查借物信息校对单内容,该制单内容不可更改，是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            string strCount = "", strDateSYS = "", strKey = "ZCC";
            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //得到表单号
                //得到服务器日期
                sqlComm.CommandText = "SELECT GETDATE() AS 日期";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                sqldr.Close();

                //得到日期
                sqlComm.CommandText = "SELECT 时间 FROM 表单计数表 WHERE (时间 = CONVERT(DATETIME, '" + strDateSYS + " 00:00:00', 102))";
                sqldr = sqlComm.ExecuteReader();

                if (sqldr.HasRows)
                    sqldr.Close();
                else //服务器时间不吻合
                {
                    sqldr.Close();
                    //修正日期及计数器
                    sqlComm.CommandText = "UPDATE 表单计数表 SET 时间 = '" + strDateSYS + "', 计数 = 1";
                    sqlComm.ExecuteNonQuery();
                }

                //得到计数器
                sqlComm.CommandText = "SELECT 计数 FROM 表单计数表 WHERE (关键词 = N'" + strKey + "')";
                sqldr = sqlComm.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    strCount = sqldr.GetValue(0).ToString();
                    sqldr.Close();

                    //增加计数器
                    sqlComm.CommandText = "UPDATE 表单计数表 SET 计数 = 计数 + 1 WHERE (关键词 = N'" + strKey + "')";
                    sqlComm.ExecuteNonQuery();
                }
                else
                    sqldr.Close();

                if (strCount != "")
                {
                    strCount = string.Format("{0:D3}", Int32.Parse(strCount));
                    strCount = strKey.ToUpper() + Convert.ToDateTime(strDateSYS).ToString("yyyyMMdd") + strCount;
                }
                else
                {
                    MessageBox.Show("数据错误", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sqlConn.Close();
                    return;
                }

                //表单汇总
                sqlComm.CommandText = "INSERT INTO 借物信息修改表 (单据编号, 修改单据ID, 日期, 业务员ID, 操作员ID, 原备注, 原联系电话, 原联系人, 原收货人, 原到站, 原运输方式, 原详细地址, 原物流名称, 原单号, 原邮政编码, BeActive, 备注, 联系电话, 联系人, 收货人, 到站, 运输方式, 详细地址, 物流名称, 单号, 邮政编码) VALUES (N'" + strCount + "', " + intBKP.ToString() + " , '" + strDateSYS + "', " + intUserID.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + dSet.Tables["送货表"].Rows[0][15].ToString() + "', N'" + dSet.Tables["送货表"].Rows[0][3].ToString() + "', N'" + dSet.Tables["送货表"].Rows[0][4].ToString() + "', N'" + dSet.Tables["送货表"].Rows[0][5].ToString() + "', N'" + dSet.Tables["送货表"].Rows[0][6].ToString() + "', N'" + dSet.Tables["送货表"].Rows[0][7].ToString() + "', N'" + dSet.Tables["送货表"].Rows[0][8].ToString() + "', N'" + dSet.Tables["送货表"].Rows[0][9].ToString() + "', N'" + dSet.Tables["送货表"].Rows[0][10].ToString() + "', N'" + dSet.Tables["送货表"].Rows[0][11].ToString() + "', 1, N'" + textBoxBZ.Text.Trim() + "', N'" + textBoxLXDH.Text.Trim() + "', N'" + textBoxLXR.Text.Trim() + "', N'" + textBoxSHR.Text.Trim() + "', N'" + textBoxDZ.Text.Trim() + "', N'" + comboBoxYSFS.Text.Trim() + "', N'" + textBoxXXDZ.Text.Trim() + "', N'" + textBoxWLMC.Text.Trim() + "', N'" + textBoxDH.Text.Trim() + "', N'" + textBoxYZBM.Text.Trim() + "')";
                sqlComm.ExecuteNonQuery();

                //信息修改
                sqlComm.CommandText = "UPDATE 借物出库汇总表 SET 备注 = N'" + textBoxBZ.Text.Trim() + "', 联系电话 = N'" + textBoxLXDH.Text.Trim() + "', 联系人 = N'" + textBoxLXR.Text.Trim() + "', 收货人 = N'" + textBoxSHR.Text.Trim() + "', 到站 = N'" + textBoxDZ.Text.Trim() + "', 运输方式 = N'" + comboBoxYSFS.Text.Trim() + "', 详细地址 = N'" + textBoxXXDZ.Text.Trim() + "', 物流名称 = N'" + textBoxWLMC.Text.Trim() + "', 单号 = N'" + textBoxDH.Text.Trim() + "', 邮政编码 = N'" + textBoxYZBM.Text.Trim() + "' WHERE (ID = " + intBKP.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                return;
            }
            finally
            {
                sqlConn.Close();
            }

            //MessageBox.Show("借物出库信息校对单保存成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = "借物出库信息校对单：" + labelDJBH.Text;
            isSaved = true;

            bool bClose = false;
            if (MessageBox.Show("借物出库信息校对单保存成功，是否关闭窗口", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                bClose=true;
            }

            if (MessageBox.Show("是否继续开始另一份单据？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
                mdiT.修改借物单据BToolStripMenuItem_Click(null, null);
            }

            if (bClose)
                this.Close();

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "借物出库信息校对单(单据编号:" + labelYDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";合计：" + labelCKJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "借物出库信息校对单(单据编号:" + labelYDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";合计：" + labelCKJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void toolStripButtonPrnFHD_Click(object sender, EventArgs e)
        {
            try
            {

                if (ppd.ShowDialog() != DialogResult.OK)
                    return;

                // Showing the Print Preview Page
                printDoc.BeginPrint += PrintDoc_BeginPrintFHD;
                printDoc.PrintPage += PrintDoc_PrintPageFHD;

                // Printing the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= PrintDoc_BeginPrintFHD;
                printDoc.PrintPage -= PrintDoc_PrintPageFHD;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void toolStripButtonPrnZXD_Click(object sender, EventArgs e)
        {
            try
            {

                if (ppd.ShowDialog() != DialogResult.OK)
                    return;

                // Showing the Print Preview Page
                printDoc.BeginPrint += PrintDoc_BeginPrint;
                printDoc.PrintPage += PrintDoc_PrintPage;

                ppw.Width = 1000;
                ppw.Height = 800;


                // Printing the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= PrintDoc_BeginPrint;
                printDoc.PrintPage -= PrintDoc_PrintPage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }

        }

        private void PrintDoc_BeginPrintFHD(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                PageNo = 1;
                NewPage = true;
                RowPos = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPageFHD(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int iyRow;
            int i;
            iyRow = 0;

            // Formatting the Content of Text Cell to print
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            StrFormat.LineAlignment = StringAlignment.Center;
            StrFormat.Trimming = StringTrimming.EllipsisCharacter;

            StringFormat StrFormatL = new StringFormat();
            StrFormatL.Alignment = StringAlignment.Near;
            StrFormatL.LineAlignment = StringAlignment.Center;
            StrFormatL.Trimming = StringTrimming.EllipsisCharacter;

            StringFormat StrFormatR = new StringFormat();
            StrFormatR.Alignment = StringAlignment.Far;
            StrFormatR.LineAlignment = StringAlignment.Center;
            StrFormatR.Trimming = StringTrimming.EllipsisCharacter;


            System.Drawing.Font _Font22 = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12 = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9 = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12U = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            int iTopM = 90;
            int iLeftM = 160;
            int iWidth1 = 170;
            int iWidth2 = 110;
            int iWidth3 = 100;
            int iWidth4 = 90;
            int iWidth5 = 180;

            int iHeight = 45;

            if (dSetP1.Tables.Contains("PRN1"))
            {
                iTopM = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][0].ToString());
                iLeftM = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][1].ToString());
                iWidth1 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][2].ToString());
                iWidth2 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][3].ToString());
                iWidth3 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][4].ToString());
                iWidth4 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][5].ToString());
                iWidth5 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][6].ToString());

                iHeight = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][7].ToString());
            }


            Brush b = new SolidBrush(Color.Black);

            try
            {
                //发货方式
                e.Graphics.DrawString(comboBoxYSFS.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM, iWidth1, iHeight), StrFormatL);

                //到站
                e.Graphics.DrawString(textBoxDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2, iTopM, iWidth3, iHeight), StrFormatL);

                //发货时间
                e.Graphics.DrawString(labelZDRQ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2 + iWidth3 + iWidth4, iTopM, iWidth5, iHeight), StrFormatL);
                //e.Graphics.DrawString(labelZDRQ.Text, _Font12, b, (decimal)(iLeftM + iWidth1 + iWidth2 + iWidth3 + iWidth4), (decimal)(iTopM), StrFormatL);

                //收货单位
                e.Graphics.DrawString(textBoxDWMC.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM + iHeight, iWidth1 + iWidth2 + iWidth3 + iWidth4 + iWidth5, iHeight), StrFormatL);

                //收货地址
                e.Graphics.DrawString(textBoxXXDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM + 2 * iHeight, iWidth1 + iWidth2 + iWidth3, iHeight), StrFormatL);

                //邮编
                e.Graphics.DrawString(textBoxYZBM.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2 + iWidth3 + iWidth4, iTopM + 2 * iHeight, iWidth5, iHeight), StrFormatL);
                //e.Graphics.DrawString(textBoxYZBM.Text, _Font12, b, (decimal)(iLeftM + iWidth1 + iWidth2 + iWidth3 + iWidth4), (decimal)(iTopM + 2 * iHeight), StrFormatL);

                //收货人
                e.Graphics.DrawString(textBoxSHR.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM + 3 * iHeight, iWidth1, iHeight), StrFormatL);


                //联系电话
                e.Graphics.DrawString(textBoxLXDH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2, iTopM + 3 * iHeight, iWidth3 + iWidth4 + iWidth5, iHeight), StrFormatL);


                //发货人
                e.Graphics.DrawString(comboBoxYWY.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM + 6 * iHeight + 30, iWidth3 + iWidth4 + iWidth5, iHeight), StrFormatL);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void PrintDoc_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                PageNo = 1;
                NewPage = true;
                RowPos = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            int iyRow;
            int i, j;
            iyRow = 0;

            // Formatting the Content of Text Cell to print
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            StrFormat.LineAlignment = StringAlignment.Center;
            StrFormat.Trimming = StringTrimming.EllipsisCharacter;

            StringFormat StrFormatL = new StringFormat();
            StrFormatL.Alignment = StringAlignment.Near;
            StrFormatL.LineAlignment = StringAlignment.Center;
            StrFormatL.Trimming = StringTrimming.EllipsisCharacter;

            StringFormat StrFormatR = new StringFormat();
            StrFormatR.Alignment = StringAlignment.Far;
            StrFormatR.LineAlignment = StringAlignment.Center;
            StrFormatR.Trimming = StringTrimming.EllipsisCharacter;


            System.Drawing.Font _Font22 = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12 = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9 = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12U = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            int iTopM1 = 100;
            int iLeftM1 = 80;
            int iHeight1 = 22;
            int iWidth1 = 700;
            int iWidth2 = 500;


            int iTopM = 218;
            int iLeftM = 95;
            int iLeftM2 = 545;
            int iWidth01 = 220;
            int iWidth02 = 120;
            int iWidth03 = 30;
            int iHeight2 = 40;


            int iHeight12 = 22;
            int iHeight22 = 50;
            int iHeight9 = 17;

            int iLM1 = 60;
            int iLM2 = 460;
            int iLM3 = 710;

            int iX1 = 430;
            int iY1 = 580;
            int iX2 = 200;
            int iY2 = 22;
            int iX3 = 760;

            if (dSetP1.Tables.Contains("PRN2"))
            {

                iTopM1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][0].ToString());
                iLeftM1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][1].ToString());
                iHeight1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][2].ToString());
                iWidth1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][3].ToString());
                iWidth2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][4].ToString());

                iTopM = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][5].ToString());
                iLeftM = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][6].ToString());
                iLeftM2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][7].ToString());
                iWidth01 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][8].ToString());
                iWidth02 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][9].ToString());
                iWidth03 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][10].ToString());
                iHeight2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][11].ToString());


                iHeight12 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][12].ToString());
                iHeight22 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][13].ToString());
                iHeight9 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][14].ToString());

                iLM1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][15].ToString());
                iLM2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][16].ToString());
                iLM3 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][17].ToString());

                iX1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][18].ToString());
                iY1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][19].ToString());
                iX2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][20].ToString());
                iY2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][21].ToString());
                iX3 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][22].ToString());


            }





            //decimal fTemp = 0;
            int iTemp = 0;

            Brush b = new SolidBrush(Color.Black);

            try
            {
                e.Graphics.DrawString("单据编号（" + labelDJBH.Text + "）", _Font12, b, new System.Drawing.RectangleF(iLeftM1, iTopM1, iWidth1, iHeight1), StrFormatL);

                e.Graphics.DrawString(textBoxDWMC.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM1, iTopM1 + iHeight1, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(textBoxSHR.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM2, iTopM1 + iHeight1, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(textBoxLXDH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM3, iTopM1 + iHeight1, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(textBoxXXDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM1, iTopM1 + iHeight1 * 2, iWidth2, iHeight1), StrFormatL);
                //e.Graphics.DrawString(comboBoxYSFS.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM2, iTopM1 + iHeight1 * 2, iWidth2, iHeight1), StrFormatL);
                //e.Graphics.DrawString(textBoxDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM3, iTopM1 + iHeight1 * 2, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(comboBoxYSFS.Text, _Font12, b, (float)(iLeftM1 + iLM2), (float)(iTopM1 + iHeight1 * 2), StrFormatL);
                e.Graphics.DrawString(textBoxDZ.Text, _Font12, b, (float)(iLeftM1 + iLM3), (float)(iTopM1 + iHeight1 * 2), StrFormatL);

                for (i = 0; i < 20; i++)
                {
                    if (RowPos >= dataGridViewDJMX.Rows.Count)
                    {
                        NewPage = false;
                        break;
                    }

                    if (dataGridViewDJMX.Rows[RowPos].IsNewRow)
                    {
                        NewPage = false;
                        break;
                    }

                    if (i < 10)
                    {
                        j = i;
                        iTemp = iLeftM;
                    }
                    else
                    {
                        j = i - 10;
                        iTemp = iLeftM2;
                    }

                    e.Graphics.DrawString(dataGridViewDJMX.Rows[RowPos].Cells[1].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iTemp, iTopM + j * iHeight2, iWidth01, iHeight2), StrFormatL);

                    e.Graphics.DrawString(Convert.ToDecimal(dataGridViewDJMX.Rows[RowPos].Cells[6].Value.ToString()).ToString("f0"), _Font12, b, new System.Drawing.RectangleF(iTemp + iWidth01, iTopM + j * iHeight2, iWidth02, iHeight2), StrFormatL);


                    RowPos++;
                }

                e.Graphics.DrawString(labelZDRQ.Text, _Font12, b, new System.Drawing.RectangleF(iX1, iY1, iX2, iY2), StrFormatL);
                e.Graphics.DrawString(labelZDRQ.Text, _Font12, b, new System.Drawing.RectangleF(iX3, iY1, iX2, iY2), StrFormatL);



                if (NewPage)
                {
                    PageNo++;
                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void toolStripButtonZXDNew_Click(object sender, EventArgs e)
        {

            try
            {

                if (ppd.ShowDialog() != DialogResult.OK)
                    return;
                //printDoc.DefaultPageSettings.PaperSize = printDoc.PrinterSettings.PaperSizes[2]; 
                foreach (System.Drawing.Printing.PaperSize ps in printDoc.PrinterSettings.PaperSizes)
                {
                    if (ps.PaperName == "A3")
                    {
                        printDoc.PrinterSettings.DefaultPageSettings.PaperSize = ps;
                        printDoc.DefaultPageSettings.PaperSize = ps;
                    }
                }
                // Showing the Print Preview Page
                printDoc.BeginPrint += PrintDoc_BeginPrintN;
                printDoc.PrintPage += PrintDoc_PrintPageN;

                ppw.Width = 1000;
                ppw.Height = 800;


                //if (ppw.ShowDialog() != DialogResult.OK)
                //{
                //    printDoc.BeginPrint -= PrintDoc_BeginPrint;
                //    printDoc.PrintPage -= PrintDoc_PrintPage;
                //    return;
                //}



                // Printing the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= PrintDoc_BeginPrintN;
                printDoc.PrintPage -= PrintDoc_PrintPageN;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void PrintDoc_BeginPrintN(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                PageNo = 1;
                NewPage = true;
                RowPos = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPageN(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            int iyRow;
            int iyRow1;
            int i, j;
            iyRow = 0;

            // Formatting the Content of Text Cell to print
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            StrFormat.LineAlignment = StringAlignment.Center;
            StrFormat.Trimming = StringTrimming.EllipsisCharacter;

            StringFormat StrFormatL = new StringFormat();
            StrFormatL.Alignment = StringAlignment.Near;
            StrFormatL.LineAlignment = StringAlignment.Center;
            StrFormatL.Trimming = StringTrimming.EllipsisCharacter;

            StringFormat StrFormatR = new StringFormat();
            StrFormatR.Alignment = StringAlignment.Far;
            StrFormatR.LineAlignment = StringAlignment.Center;
            StrFormatR.Trimming = StringTrimming.EllipsisCharacter;


            System.Drawing.Font _Font22 = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12 = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9 = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9I = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12U = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            //e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(0, 0, 870, 500));

            int iTopM = 10;
            int iLeftM = 10;
            int iLeftM1 = 100;
            int iWidth1 = 30;
            int iWidth2 = 200;
            int iWidth3 = 50;


            int iHeight12 = 22;
            int iHeight22 = 50;
            int iHeight9 = 17;
            int iHeight2 = 40;

            int iPaperWidth = 870;

            if (dSetP1.Tables.Contains("PRN3"))
            {

                iTopM = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][0].ToString());
                iLeftM = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][1].ToString());
                iLeftM1 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][2].ToString());

                iHeight2 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][3].ToString());
                iHeight12 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][4].ToString());
                iHeight22 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][5].ToString());
                iHeight9 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][6].ToString());

                iWidth1 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][7].ToString());
                iWidth2 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][8].ToString());
                iWidth3 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][9].ToString());

                iPaperWidth = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][10].ToString());
            }





            //decimal fTemp = 0;
            int iTemp = 0;
            int iTemp1 = 0;
            bool rTitle;

            Brush b = new SolidBrush(Color.Black);
            try
            {
                //e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM, iTopM, iPaperWidth - 2 * iLeftM, iHeight22));
                e.Graphics.DrawString(sGSMC + "出库单", _Font22, b, new System.Drawing.RectangleF(iLeftM, iTopM, iPaperWidth - 2 * iLeftM, iHeight22), StrFormat);

                iyRow += iTopM + iHeight22;
                e.Graphics.DrawString("单据编号（" + labelDJBH.Text + "）", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iPaperWidth - 2 * iLeftM, iHeight12), StrFormatR);

                iyRow += iHeight12;
                //e.Graphics.DrawString("制单日期：" + labelZDRQ.Text + "", _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iPaperWidth - 2 * iLeftM, iHeight12), StrFormatR);
                //iyRow += iHeight9;

                e.Graphics.DrawString("业　务员:" + comboBoxYWY.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("制单日期：" + labelZDRQ.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);
                iyRow += iHeight9;
                e.Graphics.DrawString("单位名称:" + textBoxDWMC.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("收　货人:" + textBoxSHR.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);
                iyRow += iHeight9;
                e.Graphics.DrawString("收货地址:" + textBoxXXDZ.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("联系电话:" + textBoxLXDH.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);

                iyRow += iHeight9;
                e.Graphics.DrawString("运输方式:" + comboBoxYSFS.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("装　箱人:", _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);
                iyRow += iHeight9;

                //e.Graphics.DrawString(comboBoxYSFS.Text, _Font12, b,(decimal)(iLeftM1 + iLM2), (decimal)(iTopM1 + iHeight1 * 2),StrFormatL);
                //e.Graphics.DrawString(textBoxDZ.Text, _Font12, b, (decimal)(iLeftM1 + iLM3), (decimal)(iTopM1 + iHeight1 * 2), StrFormatL);

                //表头
                e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM, iyRow, iWidth1, iHeight9 + 2));
                e.Graphics.DrawString("序号", _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow + 2, iWidth1, iHeight9), StrFormat);
                e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + iWidth1, iyRow, iWidth2, iHeight9 + 2));
                e.Graphics.DrawString("产品型号", _Font9, b, new System.Drawing.RectangleF(iLeftM + iWidth1, iyRow + 2, iWidth2, iHeight9), StrFormat);
                e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + iWidth1 + iWidth2, iyRow, iWidth3, iHeight9 + 2));
                e.Graphics.DrawString("数量", _Font9, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2, iyRow + 2, iWidth3, iHeight9), StrFormat);
                rTitle = false;
                if (!IsLastRow(RowPos))
                {
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + iWidth1 + iWidth2 + iWidth3, iyRow, iWidth1, iHeight9 + 2));
                    e.Graphics.DrawString("序号", _Font9, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2 + iWidth3, iyRow + 2, iWidth1, iHeight9), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 2 * iWidth1 + iWidth2 + iWidth3, iyRow, iWidth2, iHeight9 + 2));
                    e.Graphics.DrawString("产品型号", _Font9, b, new System.Drawing.RectangleF(iLeftM + 2 * iWidth1 + iWidth2 + iWidth3, iyRow + 2, iWidth2, iHeight9), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 2 * iWidth1 + 2 * iWidth2 + iWidth3, iyRow, iWidth3, iHeight9 + 2));
                    e.Graphics.DrawString("数量", _Font9, b, new System.Drawing.RectangleF(iLeftM + 2 * iWidth1 + 2 * iWidth2 + iWidth3, iyRow + 2, iWidth3, iHeight9), StrFormat);
                    rTitle = true;
                }
                iyRow += iHeight9 + 2;

                for (i = 0; i < iPageZX; i++)
                {
                    //序号
                    if (RowPos >= dataGridViewDJMX.Rows.Count && dataGridViewDJMX.ReadOnly)
                    {
                        NewPage = false;

                        iTemp1 = iyRow + (i / 2) * iHeight9;

                        //右表格
                        if (i % 2 != 0 & rTitle)
                        {
                            iTemp = iLeftM + iWidth1 + iWidth2 + iWidth3;
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp, iTemp1, iWidth1, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1, iTemp1, iWidth2, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9));
                        }

                        break;
                    }

                    if (i % 2 == 0 && !dataGridViewDJMX.Rows[RowPos].IsNewRow)
                    {
                        iTemp = iLeftM;
                        iTemp1 = iyRow + (i / 2) * iHeight9;
                    }
                    else
                    {
                        iTemp = iLeftM + iWidth1 + iWidth2 + iWidth3;
                    }

                    if (RowPos >= dataGridViewDJMX.Rows.Count)
                    {
                        NewPage = false;

                        //右表格
                        if (i % 2 != 0 & rTitle)
                        {
                            iTemp = iLeftM + iWidth1 + iWidth2 + iWidth3;
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp, iTemp1, iWidth1, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1, iTemp1, iWidth2, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9));
                        }

                        break;
                    }

                    if (dataGridViewDJMX.Rows[RowPos].IsNewRow)
                    {
                        NewPage = false;

                        //右表格
                        if (i % 2 != 0 & rTitle)
                        {
                            iTemp = iLeftM + iWidth1 + iWidth2 + iWidth3;
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp, iTemp1, iWidth1, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1, iTemp1, iWidth2, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9));
                        }
                        break;
                    }

                    //序号
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp, iTemp1, iWidth1, iHeight9));
                    e.Graphics.DrawString(((PageNo - 1) * iPageZX + i + 1).ToString(), _Font9I, b, new System.Drawing.RectangleF(iTemp, iTemp1, iWidth1, iHeight9), StrFormat);

                    //表格
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1, iTemp1, iWidth2, iHeight9));
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9));




                    e.Graphics.DrawString(dataGridViewDJMX.Rows[RowPos].Cells[1].Value.ToString(), _Font9, b, new System.Drawing.RectangleF(iTemp + iWidth1, iTemp1, iWidth2, iHeight9), StrFormat);

                    e.Graphics.DrawString(Convert.ToDecimal(dataGridViewDJMX.Rows[RowPos].Cells[6].Value.ToString()).ToString("f0"), _Font9, b, new System.Drawing.RectangleF(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9), StrFormat);


                    if (IsLastRow(RowPos))
                    {
                        NewPage = false;
                    }
                    RowPos++;


                }

                //iyRow = iTemp1+iHeight9+10;
                iyRow += iHeight9 * iPageNZX + 10;
                //页脚
                e.Graphics.DrawString("联系我们：", _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iPaperWidth - 2 * iLeftM, iHeight9), StrFormatL);
                iyRow += iHeight9 + 5;
                e.Graphics.DrawString(sGSDZ, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("http://www.century-twinkle.com", _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);

                iyRow += iHeight9;

                e.Graphics.DrawString("电话：" + sGSDH, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("传真：" + sGSCZ + " 邮编:" + sGSYB, _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);

                //
                iyRow += iHeight9;
                if (!dataGridViewDJMX.ReadOnly)
                    iTemp = (int)Math.Ceiling((decimal)(dataGridViewDJMX.Rows.Count - 1) / (decimal)(iPageZX));
                else
                    iTemp = (int)Math.Ceiling((decimal)(dataGridViewDJMX.Rows.Count) / (decimal)(iPageZX));

                //e.Graphics.DrawString(PageNo.ToString()+"\\" + iTemp.ToString(), _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iPaperWidth - 2 * iLeftM, iHeight9), StrFormatR);
                e.Graphics.DrawString(PageNo.ToString() + "/" + iTemp.ToString(), _Font22, b, new System.Drawing.RectangleF(iLeftM, iTopM, iPaperWidth - 2 * iLeftM, iHeight22), StrFormatR);






                if (NewPage)
                {
                    PageNo++;
                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool IsLastRow(int iRow)
        {
            if (!dataGridViewDJMX.ReadOnly || dataGridViewDJMX.AllowUserToAddRows) //有新行
            {
                if (iRow == dataGridViewDJMX.RowCount - 2)
                    return true;
                else
                    return false;
            }
            else
            {
                if (iRow == dataGridViewDJMX.RowCount - 1)
                    return true;
                else
                    return false;
            }
        }


    }
}