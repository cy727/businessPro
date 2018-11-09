using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKCSPPD2 : Form
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

        private int intPDID = 0;
        private int intKFID = 0;

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;
        
        public FormKCSPPD2()
        {
            InitializeComponent();
        }


        private void FormKCSPPD2_Load(object sender, EventArgs e)
        {
            int i;

            this.Top = 1;
            this.Left = 1;


            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            if (isSaved)
            {
                //dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                initDJ();
                return;
            }

            sqlConn.Open();

            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.Text = strUserName;

            //初始化部门列表

            sqlComm.CommandText = "SELECT 部门名称 FROM 部门表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("部门表")) dSet.Tables.Remove("部门表");
            sqlDA.Fill(dSet, "部门表");
            //comboBoxBM.DataSource = dSet.Tables["部门表"];

            comboBoxBM.Items.Add("全部");
            for (i = 0; i < dSet.Tables["部门表"].Rows.Count; i++)
            {
                comboBoxBM.Items.Add(dSet.Tables["部门表"].Rows[i][0].ToString().Trim());
            }

            //明细
            sqlComm.CommandText = "SELECT 库存盘点定义表.记录选择, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 库存盘点明细表.实盘数量, 库存盘点明细表.结存数量, 库存盘点明细表.结存金额, 库存盘点定义表.盘损数量, 库存盘点定义表.盘损金额, 库存盘点明细表.备注, 库存盘点明细表.ID, 库存盘点明细表.商品ID FROM 库存盘点明细表 INNER JOIN 商品表 ON 库存盘点明细表.商品ID = 商品表.ID CROSS JOIN 库存盘点定义表 WHERE (库存盘点明细表.单据ID = 0)";

            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据表"];

            dataGridViewDJMX.Columns[6].Visible = false;
            dataGridViewDJMX.Columns[8].Visible = false;

            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[1].ReadOnly = true;
            dataGridViewDJMX.Columns[2].ReadOnly = true;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[5].ReadOnly = true;
            dataGridViewDJMX.Columns[6].ReadOnly = true;
            dataGridViewDJMX.Columns[7].ReadOnly = true;
            dataGridViewDJMX.Columns[8].ReadOnly = true;
            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            sqlConn.Close();

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }

        private void initDJ()
        {
            checkBoxAll.CheckedChanged -= checkBoxAll_CheckedChanged;
            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            sqlConn.Open();
            //sqlComm.CommandText = "SELECT 库存盘点汇总表.单据编号, 库存盘点汇总表.日期, 职员表.职员姓名 AS 业务员, 操作员.职员姓名, 库存盘点汇总表.备注, 库房表.库房编号, 库房表.库房名称 FROM 库存盘点汇总表 INNER JOIN 职员表 ON 库存盘点汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 库存盘点汇总表.操作员ID = 操作员.ID INNER JOIN 库房表 ON 库存盘点汇总表.库房ID = 库房表.ID WHERE (库存盘点汇总表.ID = " + iDJID.ToString() + ")";
            sqlComm.CommandText = "SELECT 库存盘点汇总表.单据编号, 库存盘点汇总表.日期, 职员表_1.职员姓名 AS 业务员, 职员表_1.职员姓名 AS 操作员, 库存盘点汇总表.备注 FROM 库存盘点汇总表 INNER JOIN 职员表 AS 职员表_1 ON 库存盘点汇总表.业务员ID = 职员表_1.ID INNER JOIN 职员表 AS 职员表_2 ON 库存盘点汇总表.操作员ID = 职员表_2.ID WHERE (库存盘点汇总表.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                //textBoxPDKF.Text = sqldr.GetValue(5).ToString();
                //textBoxKFMC.Text = sqldr.GetValue(6).ToString();

                this.Text = "库存盘点表：" + labelDJBH.Text;
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT 部门表.部门名称 FROM 部门表 INNER JOIN 职员表 ON 部门表.ID = 职员表.岗位ID WHERE (职员表.职员姓名 = N'" + comboBoxYWY.Text + "')";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                comboBoxBM.Text = sqldr.GetValue(0).ToString();
                break;
            }
            sqldr.Close();


            //初始化商品列表
            //sqlComm.CommandText = "SELECT 库存盘点定义表.记录选择, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 库存盘点明细表.实盘数量, 库存盘点明细表.结存数量, 库存盘点明细表.结存金额, 库存盘点明细表.盘损数量, 库存盘点明细表.盘损金额, 库存盘点明细表.备注, 库存盘点明细表.ID, 库存盘点明细表.商品ID, 商品表.库存成本价 FROM 库存盘点明细表 INNER JOIN 商品表 ON 库存盘点明细表.商品ID = 商品表.ID CROSS JOIN 库存盘点定义表 WHERE (库存盘点明细表.单据ID = " + iDJID.ToString() + ")";
            sqlComm.CommandText = "SELECT 库存盘点明细表.盘点标志, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 库存盘点明细表.实盘数量, 库存盘点明细表.结存数量, 库存盘点明细表.结存金额, 库存盘点明细表.盘损数量, 库存盘点明细表.盘损金额, 库存盘点明细表.备注, 库存盘点明细表.ID, 库存盘点明细表.商品ID, 商品表.库存成本价, 库存盘点明细表.库房ID, 库房表.库房名称 FROM 库存盘点明细表 INNER JOIN 商品表 ON 库存盘点明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 库存盘点明细表.库房ID = 库房表.ID CROSS JOIN 库存盘点定义表 WHERE (库存盘点明细表.单据ID = " + iDJID.ToString() + ")";


            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据表"];

            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[0].Visible = false;

            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridViewDJMX.ShowCellErrors = true;
            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;


            sqlConn.Close();

            decimal fSum = 0, fSum1 = 0;
            decimal fCount = 0, fCSum = 0, fCSum1 = 0;
            //this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                //记录选择标志
                if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                }
                if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() != "1")
                    continue;
                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString());
                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString());

                fCSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value.ToString());
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString());
                fCount += 1;

            }
            //this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelSLHJ.Text = fCSum.ToString();
            labelJEHJ.Text = fSum.ToString();
            labelPSSLHJ.Text = fCSum1.ToString();
            labelPSJEHJ.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelPSJEHJ.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();


        }

        private void textBoxPDBH_DoubleClick(object sender, EventArgs e)
        {
            FormSelectHT frmSelectHT = new FormSelectHT();
            frmSelectHT.strConn = strConn;
            frmSelectHT.iSelectStyle = 100;
            frmSelectHT.ShowDialog();
            intPDID = frmSelectHT.iHTNumber;

            getCPDDetail();

        }

        private void getCPDDetail()
        {
            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            if (intPDID == 0)
                return;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 库存盘点汇总表.单据编号, 库存盘点汇总表.日期, 职员表_1.职员姓名 AS 业务员, 职员表_1.职员姓名 AS 操作员, 库存盘点汇总表.备注 FROM 库存盘点汇总表 INNER JOIN 职员表 AS 职员表_1 ON 库存盘点汇总表.业务员ID = 职员表_1.ID INNER JOIN 职员表 AS 职员表_2 ON 库存盘点汇总表.操作员ID = 职员表_2.ID WHERE (库存盘点汇总表.ID = " + intPDID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();

            if (!sqldr.HasRows)
            {
                intPDID = 0;
                sqldr.Close();
                sqlConn.Close();
                return;
            } 
            sqldr.Read();
            labelDJBH.Text = sqldr.GetValue(0).ToString();
            labelZDRQ.Text = sqldr.GetValue(1).ToString();
            comboBoxYWY.Text = sqldr.GetValue(2).ToString();
            textBoxBZ.Text = sqldr.GetValue(4).ToString();
            //textBoxPDKF.Text = sqldr.GetValue(5).ToString();
            //textBoxKFMC.Text = sqldr.GetValue(6).ToString();
            //intKFID = Int32.Parse(sqldr.GetValue(7).ToString());
            sqldr.Close();

            //明细
            //sqlComm.CommandText = "SELECT 库存盘点定义表.记录选择, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 库存盘点明细表.实盘数量, 库存盘点明细表.结存数量, 库存盘点明细表.结存金额, 库存盘点定义表.盘损数量, 库存盘点定义表.盘损金额, 库存盘点明细表.备注, 库存盘点明细表.ID, 库存盘点明细表.商品ID, 商品表.库存成本价 FROM 库存盘点明细表 INNER JOIN 商品表 ON 库存盘点明细表.商品ID = 商品表.ID CROSS JOIN 库存盘点定义表 WHERE (库存盘点明细表.单据ID = " + intPDID.ToString() + ")";

            sqlComm.CommandText = "SELECT 库存盘点定义表.记录选择, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 库存盘点明细表.实盘数量, 库存盘点明细表.结存数量, 库存盘点明细表.结存金额, 库存盘点定义表.盘损数量, 库存盘点定义表.盘损金额, 库存盘点明细表.备注, 库存盘点明细表.ID, 库存盘点明细表.商品ID, 商品表.库存成本价, 库存盘点明细表.库房ID, 库房表.库房名称 FROM 库存盘点明细表 INNER JOIN 商品表 ON 库存盘点明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 库存盘点明细表.库房ID = 库房表.ID CROSS JOIN 库存盘点定义表 WHERE (库存盘点明细表.单据ID = " + intPDID.ToString() + ")";


            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据表"];
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[12].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            

            sqlConn.Close();
            //countAmount();
            dataGridViewDJMX.RowValidating += dataGridViewDJMX_RowValidating;


            
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.Rows.Count < 0)
                return;

            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;

            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                dataGridViewDJMX.Rows[i].Cells[0].Value = checkBoxAll.Checked;
                dataGridViewDJMX.EndEdit();
            }

            countAmount();
            dataGridViewDJMX.RowValidating += dataGridViewDJMX_RowValidating;
        }

        private bool countAmount()
        {

            decimal fSum = 0, fSum1 = 0;
            decimal fTemp=0, fTemp1;
            decimal fCount = 0, fCSum = 0, fCSum1=0;
            //this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                //记录选择标志
                if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                }
                if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value))
                    continue;


                //实盘数量
                if (dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[4].Value = 0;

                //盘损数量
                dataGridViewDJMX.Rows[i].Cells[7].Value = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString()) - Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value.ToString());
                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString()) == 0) //结存数量为0
                {
                    fTemp = 0;
                }
                else
                {
                    fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString()) / Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString());
                }

                //盘损金额
                fTemp1 = Convert.ToDecimal(Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value.ToString()).ToString("f2"));
                if(!isSaved)
                    dataGridViewDJMX.Rows[i].Cells[8].Value = fTemp1 * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[12].Value.ToString());


                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value.ToString());
                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString());

                fCSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value.ToString());
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString());
                fCount += 1;

            }
            //this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelSLHJ.Text = fCSum.ToString();
            labelJEHJ.Text = fSum.ToString();
            labelPSSLHJ.Text = fCSum1.ToString();
            labelPSJEHJ.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelPSJEHJ.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return true;
        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i;
            decimal fTemp,fTemp1;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("库存盘点数据登录完毕,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intPDID == 0)
            {
                MessageBox.Show("请选择库存盘点表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            countAmount();

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("没有选择记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("请检查库存实盘内容,是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;
            saveToolStripButton.Enabled = false;
            string strCount = "", strDateSYS = "";
            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //单据汇总
                cGetInformation.getSystemDateTime();
                strDateSYS = cGetInformation.strSYSDATATIME;

                sqlComm.CommandText = "UPDATE 库存盘点汇总表 SET 盘损数量合计 = " + labelPSSLHJ.Text + ", 盘损金额合计 = " + labelPSJEHJ.Text + ", 盘点时间 = '" + strDateSYS + "',  盘点标记 = 1 WHERE (ID = " + intPDID.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //单据明细
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    //记录选择标志
                    if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value))
                        continue;

                    sqlComm.CommandText = "UPDATE 库存盘点明细表 SET 实盘数量 = " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ", 备注 = N'" + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + "', 盘点标志 = 1 , 盘损数量=" + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ", 盘损金额=" + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();


                    //总库存
                    fTemp = 0; fTemp1 = 0;
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价 FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    //库存成本价
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value.ToString());
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString());

                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                    }
                    dKUL-=dKUL1;
                    //dKCJE -= dKCJE1;
                    dKCJE = dKCCBJ * dKUL;
                    sqldr.Close();

                    sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //历史纪录
                    sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 业务员ID, 单据编号, 摘要, 总结存数量, 总结存金额, 盘损数量, 盘损单价, 盘损金额, BeActive) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", "+comboBoxYWY.SelectedValue.ToString()+", N'"+labelDJBH.Text+"', N'库存盘点表', "+dKUL.ToString()+", "+dKCJE.ToString()+", "+dKUL1.ToString()+", "+dKCCBJ.ToString()+", "+dKCJE1.ToString()+", 1)";
                    sqlComm.ExecuteNonQuery();



                    //更改分库存
                    fTemp=0;
                    //sqlComm.CommandText = "SELECT  库存数量, 库存金额, 库存成本价  FROM 库存表 WHERE (库房ID = " + intKFID.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ")";
                    sqlComm.CommandText = "SELECT  库存数量, 库存金额, 库存成本价  FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ")";
                    sqldr=sqlComm.ExecuteReader();

                    //库存成本价
                    dKUL = 0; dKCJE = 0; dKCCBJ = 0;
                    while(sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                    }
                    sqldr.Close();
                    
                    //库存金额
                    dKUL -= dKUL1;
                    //dKCJE -= dKCJE1;
                    dKCJE = dKCCBJ * dKUL;

                    sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + " WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //库房历史纪录
                    //sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 业务员ID, 单据编号, 摘要, 库房结存数量, 库房结存金额, 盘损数量, 盘损单价, 盘损金额, BeActive) VALUES (" + intKFID.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + labelDJBH.Text + "', N'库存盘点表', " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", 1)";
                    sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 业务员ID, 单据编号, 摘要, 库房结存数量, 库房结存金额, 盘损数量, 盘损单价, 盘损金额, BeActive) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + labelDJBH.Text + "', N'库存盘点表', " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", 1)";
                    sqlComm.ExecuteNonQuery();

                }
                
                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                saveToolStripButton.Enabled = true;
                return;
            }
            finally
            {
                sqlConn.Close();
            }


            //MessageBox.Show("库存盘点数据登录完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //labelDJBH.Text = strCount;
            this.Text = "库存盘点数据登录：" + labelDJBH.Text;
            isSaved = true;

            if (MessageBox.Show("库存盘点数据登录完毕，是否继续开始另一份单据？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
                mdiT.实盘数据登录CToolStripMenuItem_Click(null, null);
            }

            if (MessageBox.Show("是否关闭制单窗口", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }


        }

        private void FormKCSPPD2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
            {
                return;
            }

            DialogResult dr = MessageBox.Show(this, "单据尚未保存，确定要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {

            string strT = "库存盘点表(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";库房名称：" + textBoxKFMC.Text + ";盘损数量合计：" + labelPSSLHJ.Text + ";盘损金额合计：" + labelPSJEHJ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库存盘点表(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";库房名称：" + textBoxKFMC.Text + ";盘损数量合计：" + labelPSSLHJ.Text + ";盘损金额合计：" + labelPSJEHJ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void textBoxPDBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                FormSelectHT frmSelectHT = new FormSelectHT();
                frmSelectHT.strConn = strConn;
                frmSelectHT.iSelectStyle = 110;
                frmSelectHT.strHTSearch = textBoxPDBH.Text.Trim();

                frmSelectHT.ShowDialog();
                intPDID = frmSelectHT.iHTNumber;

                getCPDDetail();
                dataGridViewDJMX.Focus();
            }
        }
    }
}