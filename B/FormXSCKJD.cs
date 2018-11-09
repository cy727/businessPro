using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXSCKJD : Form
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
        private int intBKP = 0;

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;
        private bool bCheck = true;

        public FormXSCKJD()
        {
            InitializeComponent();
        }

        private void FormXSCKJD_Load(object sender, EventArgs e)
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
                dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
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

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }

        private void initDJ()
        {
            int iBM = 0;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 销售出库汇总表.单据编号, 销售出库汇总表.日期, 职员表.职员姓名, [职员表_1].职员姓名 AS Expr1, 销售出库汇总表.备注, 单位表.单位编号, 单位表.单位名称, 销售出库汇总表.发票号, 销售出库汇总表.支票号, 合同号,销售出库汇总表.部门ID,销售出库汇总表.BeActive FROM 销售出库汇总表 INNER JOIN 职员表 ON 销售出库汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 销售出库汇总表.操作员ID = [职员表_1].ID INNER JOIN 单位表 ON 销售出库汇总表.单位ID = 单位表.ID WHERE (销售出库汇总表.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                if (sqldr.GetValue(10).ToString() != "")
                {
                    try
                    {
                        iBM = int.Parse(sqldr.GetValue(10).ToString());
                    }
                    catch
                    {
                        iBM = 0;
                    }

                }
                if (!bool.Parse(sqldr.GetValue(11).ToString()))
                {
                    labelDJBH.ForeColor = Color.Red;
                }

                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                textBoxDWMC.Text = sqldr.GetValue(6).ToString();
                textBoxZPH.Text = sqldr.GetValue(8).ToString();
                textBoxFPH.Text = sqldr.GetValue(7).ToString();
                textBoxHTH.Text = sqldr.GetValue(9).ToString();

                this.Text = "销售出库校对单：" + labelDJBH.Text;
            }
            sqldr.Close();

            if (iBM != 0)
            {
                comboBoxBM.SelectedIndexChanged -= comboBoxBM_SelectedIndexChanged;
                sqlComm.CommandText = "SELECT 部门名称 FROM 部门表 WHERE (ID = " + iBM.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                    comboBoxBM.Text = sqldr.GetValue(0).ToString();
                    break;
                }
                sqldr.Close();
                comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;
            }


            //初始化商品列表
            sqlComm.CommandText = "SELECT CONVERT(bit, 1) AS 校对, 销售商品制单表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 销售出库明细表.数量, 销售出库明细表.单价, 销售出库明细表.金额, 销售出库明细表.扣率, 销售出库明细表.实计金额, 销售出库明细表.数量 AS 未出库数量, 销售出库明细表.商品ID, 销售出库明细表.库房ID, 销售出库明细表.ID, 销售出库明细表.赠品, 销售出库明细表.单据ID, 销售出库明细表.毛利 FROM 销售出库明细表 INNER JOIN 商品表 ON 销售出库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 销售出库明细表.库房ID = 库房表.ID INNER JOIN 销售商品制单表 ON 销售出库明细表.原单据ID = 销售商品制单表.ID WHERE (销售出库明细表.单据ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];


            dataGridViewDJMX.Columns[14].Visible = false;
            dataGridViewDJMX.Columns[15].Visible = false;
            dataGridViewDJMX.Columns[16].Visible = false;
            dataGridViewDJMX.Columns[18].Visible = false;
            dataGridViewDJMX.Columns[19].Visible = false;

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
            dataGridViewDJMX.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[19].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[12].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[19].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[13].DefaultCellStyle.Format = "f0";


            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;


            sqlConn.Close();

            dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            //dataGridViewDJMX.CellDoubleClick -= dataGridViewDJMX_CellDoubleClick;
            countAmount();
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

        private void initdataGridViewDJMX()
        {
            if (strSelect == "") return;
            sqlConn.Open();
            sqlComm.CommandText = strSelect;

            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据表"];

            sqlConn.Close();


            dataGridViewDJMX.Columns[14].Visible = false;
            dataGridViewDJMX.Columns[15].Visible = false;
            dataGridViewDJMX.Columns[16].Visible = false;
            dataGridViewDJMX.Columns[1].ReadOnly = true;
            dataGridViewDJMX.Columns[2].ReadOnly = true;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[4].ReadOnly = true;
            dataGridViewDJMX.Columns[5].ReadOnly = true;
            dataGridViewDJMX.Columns[6].ReadOnly = true;
            dataGridViewDJMX.Columns[7].ReadOnly = true;
            dataGridViewDJMX.Columns[9].ReadOnly = true;
            dataGridViewDJMX.Columns[10].ReadOnly = true;
            dataGridViewDJMX.Columns[11].ReadOnly = true;
            dataGridViewDJMX.Columns[12].ReadOnly = true;
            dataGridViewDJMX.Columns[13].ReadOnly = true;
            dataGridViewDJMX.Columns[17].ReadOnly = true;
            dataGridViewDJMX.Columns[19].ReadOnly = true;
            dataGridViewDJMX.Columns[18].Visible = false;
            dataGridViewDJMX.Columns[20].Visible = false;
            dataGridViewDJMX.Columns[19].Visible = false;

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
            dataGridViewDJMX.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[19].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[12].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[19].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[13].DefaultCellStyle.Format = "f0";


            dataGridViewDJMX.ShowCellErrors = true;
            checkBoxAll.Checked = false;

            dataGridViewDJMX.Focus();
            if(dataGridViewDJMX.RowCount>0)
            {
                dataGridViewDJMX.CurrentCell = dataGridViewDJMX[0, 0];
            }

        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(2, "") == 0)
            {
                return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                if (cGetInformation.iBMID != 0)
                    comboBoxBM.SelectedValue = cGetInformation.iBMID;

                comboBoxYWY.Text = cGetInformation.sCompanyYWY;
            }
            strSelect = "SELECT 购进商品制单明细定义表.到货 AS 校对, 销售商品制单表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.未出库数量 AS 出库数量, 销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.扣率, 销售商品制单明细表.实计金额, 销售商品制单明细表.未出库数量, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID, 销售商品制单明细表.ID AS 明细ID, 销售商品制单明细表.赠品, 销售商品制单表.ID AS 汇总ID, 销售商品制单明细表.毛利, 销售商品制单明细表.ID FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (销售商品制单表.单位ID = " + iSupplyCompany.ToString() + ") AND (销售商品制单明细表.未出库数量 > 0) ORDER BY 销售商品制单表.单据编号";

            initdataGridViewDJMX();

        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(20, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                    return;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    if (cGetInformation.iBMID != 0)
                        comboBoxBM.SelectedValue = cGetInformation.iBMID;

                    comboBoxYWY.Text = cGetInformation.sCompanyYWY;
                }

                strSelect = "SELECT 购进商品制单明细定义表.到货 AS 校对, 销售商品制单表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.未出库数量 AS 出库数量, 销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.扣率, 销售商品制单明细表.实计金额, 销售商品制单明细表.未出库数量, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID, 销售商品制单明细表.ID AS 明细ID, 销售商品制单明细表.赠品, 销售商品制单表.ID AS 汇总ID, 销售商品制单明细表.毛利,销售商品制单明细表.ID FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (销售商品制单表.单位ID = " + iSupplyCompany.ToString() + ") AND (销售商品制单明细表.未出库数量 > 0) ORDER BY 销售商品制单表.单据编号";

                initdataGridViewDJMX();
            }

        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(22, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                    return;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    if (cGetInformation.iBMID != 0)
                        comboBoxBM.SelectedValue = cGetInformation.iBMID;

                    comboBoxYWY.Text = cGetInformation.sCompanyYWY;
                }
                strSelect = "SELECT 购进商品制单明细定义表.到货 AS 校对, 销售商品制单表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.未出库数量 AS 出库数量, 销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.扣率, 销售商品制单明细表.实计金额, 销售商品制单明细表.未出库数量, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID, 销售商品制单明细表.ID AS 明细ID, 销售商品制单明细表.赠品, 销售商品制单表.ID AS 汇总ID, 销售商品制单明细表.毛利,销售商品制单明细表.ID FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (销售商品制单表.单位ID = " + iSupplyCompany.ToString() + ") AND (销售商品制单明细表.未出库数量 > 0) ORDER BY 销售商品制单表.单据编号";

                initdataGridViewDJMX();
            }

        }

        private void textBoxHTH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getBillInformation(2, "") == 0)
            {
                textBoxHTH.Text = "";
                intBKP = 0;
                return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iBillCNumber;
                textBoxDWMC.Text = cGetInformation.strBillCName;
                textBoxDWBH.Text = cGetInformation.strBillCCode;
                textBoxHTH.Text = cGetInformation.strBillCode;
                comboBoxBM.SelectedValue = cGetInformation.iBillBMID;
                comboBoxYWY.Text = cGetInformation.sPeopleName;
                intBKP = cGetInformation.iBillNumber;

                sqlComm.CommandText = "SELECT 销售商品制单表.备注 FROM 销售商品制单表 WHERE (销售商品制单表.ID = " + cGetInformation.iBillNumber + ")";
                sqlConn.Open();
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    textBoxBZ.Text = sqldr.GetValue(0).ToString();
                }
                sqldr.Close();
                sqlConn.Close();

                strSelect = "SELECT 购进商品制单明细定义表.到货 AS 校对, 销售商品制单表.单据编号,商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位,库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.未出库数量 AS 出库数量,  销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.扣率, 销售商品制单明细表.实计金额, 销售商品制单明细表.未出库数量, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID, 销售商品制单明细表.ID AS 明细ID, 销售商品制单明细表.赠品, 销售商品制单表.ID AS 汇总ID, 销售商品制单明细表.毛利,销售商品制单明细表.ID FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (销售商品制单明细表.未出库数量 > 0) AND (销售商品制单表.ID = " + cGetInformation.iBillNumber + ") ORDER BY 销售商品制单表.单据编号";

                initdataGridViewDJMX();
            }
        }




        private void textBoxHTH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getBillInformation(20, textBoxHTH.Text.Trim()) == 0)
                {
                    textBoxHTH.Text = "";
                    textBoxBZ.Text = "";
                    intBKP = 0;
                    return;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iBillCNumber;
                    textBoxDWMC.Text = cGetInformation.strBillCName;
                    textBoxDWBH.Text = cGetInformation.strBillCCode;
                    textBoxHTH.Text = cGetInformation.strBillCode;
                    comboBoxBM.SelectedValue = cGetInformation.iBillBMID;
                    comboBoxYWY.Text = cGetInformation.sPeopleName;
                    intBKP = cGetInformation.iBillNumber;

                    sqlComm.CommandText = "SELECT 销售商品制单表.备注 FROM 销售商品制单表 WHERE (销售商品制单表.ID = " + cGetInformation.iBillNumber + ")";
                    sqlConn.Open();
                    sqldr = sqlComm.ExecuteReader();

                    while (sqldr.Read())
                    {
                        textBoxBZ.Text = sqldr.GetValue(0).ToString();
                    }
                    sqldr.Close();
                    sqlConn.Close();


                    strSelect = "SELECT 购进商品制单明细定义表.到货 AS 校对, 销售商品制单表.单据编号,商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位,库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.未出库数量 AS 出库数量,  销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.扣率, 销售商品制单明细表.实计金额, 销售商品制单明细表.未出库数量, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID, 销售商品制单明细表.ID AS 明细ID, 销售商品制单明细表.赠品, 销售商品制单表.ID AS 汇总ID, 销售商品制单明细表.毛利,销售商品制单明细表.ID FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (销售商品制单明细表.未出库数量 > 0) AND (销售商品制单表.ID = " + cGetInformation.iBillNumber + ") ORDER BY 销售商品制单表.单据编号";

                    initdataGridViewDJMX();
                }
            }

        }

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
                return;
            if (isSaved)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            switch (e.ColumnIndex)
            {
                case 8:  //商品数量
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut <= 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[8].ErrorText = "商品出库校对数量输入错误";
                            e.Cancel = true;
                        }
                        else
                        {
                            if (intOut > Int32.Parse(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString()))
                            {
                                this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                                dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value;
                                this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            }
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].ErrorText = "商品出库校对数量输入类型错误";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;



            }
            dataGridViewDJMX.EndEdit();
        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            //return EnterToTab(ref   msg, keyData, true);

            Control ctr = (Control)Control.FromHandle(msg.HWnd);

            if (ctr != null)
            {
                if (ctr.GetType() == typeof(System.Windows.Forms.DataGridViewTextBoxEditingControl))
                {
                    DataGridViewTextBoxEditingControl dvTextBoxEC = (DataGridViewTextBoxEditingControl)FromHandle(msg.HWnd);
                    DataGridView dv = (DataGridView)dvTextBoxEC.EditingControlDataGridView;
                    if (dv.Columns.Count > 0)
                    {
                        if (keyData == Keys.Enter)
                        {
                            try
                            {
                                dv.EndEdit();
                                switch (dv.CurrentCell.ColumnIndex)
                                {
                                    case 0:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                        break;
                                    case 8:
                                        if (dv.CurrentCell.RowIndex == dv.RowCount - 1)
                                            dv.CurrentCell = dv.Rows[0].Cells[0];
                                        else
                                            dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[0];
                                        break;
                                    default:
                                        break;
                                }
                                dv.BeginEdit(true);
                            }
                            catch (Exception)
                            {
                            }
                            return true;
                        }

                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool countAmount()
        {
            decimal fSum = 0, fSum1 = 0;
            decimal fTemp, fTemp1;
            decimal fCount = 0, fCSum = 0;
            bool bCheck = true;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                }

                if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value.ToString()))
                    continue;


                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "输入出库校验数量";
                    bCheck = false;
                }


                if (!bCheck)
                    continue;


                //数量
                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                    fTemp = 0;
                else
                    fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                fCSum += fTemp;

                //单价
                if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                //赠品
                if (dataGridViewDJMX.Rows[i].Cells[17].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[17].Value = 0;


                //金额
                if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[17].Value))
                    dataGridViewDJMX.Rows[i].Cells[10].Value =Math.Round(fTemp * fTemp1, 2);
                else
                    dataGridViewDJMX.Rows[i].Cells[10].Value = 0;

                //扣率
                if (dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[11].Value = 100;
                }

                //毛利
                if (dataGridViewDJMX.Rows[i].Cells[19].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[19].Value = 0;
                }
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[19].Value) / Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[13].Value) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                dataGridViewDJMX.Rows[i].Cells[19].Value = fTemp1;

                //实计金额
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                dataGridViewDJMX.Rows[i].Cells[12].Value = fTemp1 * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value.ToString()) / 100;

                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                //fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[12].Value);

                fCount += 1;

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelSLHJ.Text = fCSum.ToString("f0");
            labelJEHJ.Text = fSum.ToString("f2");
            labelSJJE.Text = fSum1.ToString("f2");

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;


        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.Rows.Count < 0)
                return;

            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                dataGridViewDJMX.Rows[i].Cells[0].Value = checkBoxAll.Checked;
                dataGridViewDJMX.EndEdit();
            }

            countAmount();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i,j;
            decimal dKUL = 0 ;
            decimal dKUL1 = 0;

            textBoxHTH.Focus();
            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("销售出库校对单已经保存,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("请选择销售单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (intBKP == 0)
            {
                MessageBox.Show("请选择销售单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("销售销售出库校对单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("没有销售出库校对单商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if (MessageBox.Show("请检查销售销售出库校对单内容,是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            //    return;

            saveToolStripButton.Enabled = false;

            string strCount = "", strDateSYS = "", strKey = "BCK";
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
                string sBMID = "NULL";
                if (comboBoxBM.SelectedValue.ToString() != "0")
                    sBMID = comboBoxBM.SelectedValue.ToString();

                sqlComm.CommandText = "INSERT INTO 销售出库汇总表 (单位ID, 单据编号, 日期, 发票号, 支票号, 价税合计, 业务员ID, BeActive, 操作员ID, 未付款金额, 已付款金额, 付款标记, 备注, 合同号, 销售ID, 部门ID) VALUES (" + iSupplyCompany.ToString() + ", N'" + strCount + "', '" + strDateSYS + "', N'" + textBoxFPH.Text + "', N'" + textBoxZPH.Text + "', " + labelSJJE.Text + ", " + comboBoxYWY.SelectedValue.ToString() + ", 1, " + intUserID.ToString() + ", " + labelSJJE.Text + ", 0, 0, N'" + textBoxBZ.Text + "', N'" + textBoxHTH.Text + "', " + intBKP.ToString() + ","+sBMID+")";
                sqlComm.ExecuteNonQuery();

                //取得单据号 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //单据明细
                string strTemp = "";
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    //出库标志
                    if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                    }
                    if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value))
                        continue;

                    strTemp = "";
                    if (dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() == "0") //无校对
                    {
                        dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                        continue;
                    }

                    if (Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[17].Value)) //赠品
                        sqlComm.CommandText = "INSERT INTO 销售出库明细表 (单据ID, 商品ID, 库房ID, 原单据ID, 数量, 单价, 金额, 扣率, 赠品, 实计金额, BeActive, 未付款金额, 已付款金额, 未付款数量, 已付款数量, 毛利, 原单据明细ID) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[18].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + Convert.ToInt32(dataGridViewDJMX.Rows[i].Cells[17].Value).ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", 1, 0, 0, 0, 0, 0, " + dataGridViewDJMX.Rows[i].Cells[20].Value.ToString() + ")";
                    else
                        sqlComm.CommandText = "INSERT INTO 销售出库明细表 (单据ID, 商品ID, 库房ID, 原单据ID, 数量, 单价, 金额, 扣率, 赠品, 实计金额, BeActive, 未付款金额, 已付款金额, 未付款数量, 已付款数量, 毛利, 原单据明细ID) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[18].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + Convert.ToInt32(dataGridViewDJMX.Rows[i].Cells[17].Value).ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", 1, " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", 0," + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", 0, " + dataGridViewDJMX.Rows[i].Cells[19].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[20].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    sqlComm.CommandText = "UPDATE 销售商品制单明细表 SET 未出库数量 =未出库数量-" + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                }
                //应付账
                //sqlComm.CommandText = "UPDATE 单位表 SET 应收账款 = 应收账款 + " + labelSJJE.Text + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                //sqlComm.ExecuteNonQuery();

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
            //changeKC();
            checkRKView();

            //MessageBox.Show("销售出库校对单保存成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = "销售出库校对单：" + labelDJBH.Text;
            isSaved = true;

            bool bClose = false;
            //if (MessageBox.Show("销售出库校对单保存成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //{
                bClose = true;
            //}

            if (MessageBox.Show("销售出库校对单保存成功，是否继续开始另一份单据？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
                mdiT.销售出库校对CToolStripMenuItem_Click(null, null);
            }


            if (bClose)
                this.Close();
        }

        //更新入库标记
        private void checkRKView()
        {
            int i;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    //进货标志
                    if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value))
                        continue;

                    sqlComm.CommandText = "SELECT 销售商品制单明细表.ID FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID WHERE (销售商品制单明细表.未出库数量 <> 0) AND (销售商品制单明细表.表单ID = " + dataGridViewDJMX.Rows[i].Cells[18].Value.ToString() + ") AND (销售商品制单表.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //存在未出货明细
                    {
                        sqldr.Close();
                    }
                    else
                    {
                        sqldr.Close();
                        sqlComm.CommandText = "UPDATE 销售商品制单表 SET 出库标记 = 1 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[18].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();
                    }
                }
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
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("销售出库校对单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "销售出库校对单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("销售出库校对单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "销售出库校对单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

  
    }
}