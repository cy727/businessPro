using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXSTHZD : Form
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
        private int intHTH = 0;
        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;
        private bool bCheck = true;

        public FormXSTHZD()
        {
            InitializeComponent();
        }

        private void FormXSTHZD_Load(object sender, EventArgs e)
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

            //初始化商品列表
            sqlComm.CommandText = "SELECT 销售商品制单明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.数量, 销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.扣率, 销售商品制单明细表.实计金额, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID, 销售商品制单明细表.数量 AS 库存量, 购进商品制单明细定义表.统计标志, 销售商品制单明细表.赠品  FROM 销售商品制单明细表 INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (销售商品制单明细表.ID = 0)";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[15].Visible = false;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[4].ReadOnly = true;
            dataGridViewDJMX.Columns[9].ReadOnly = true;
            dataGridViewDJMX.Columns[11].ReadOnly = true;
            dataGridViewDJMX.Columns[14].ReadOnly = true;

            dataGridViewDJMX.Columns[5].ReadOnly = true;
            dataGridViewDJMX.Columns[6].ReadOnly = true;
            
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
            dataGridViewDJMX.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[16].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;

            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[14].DefaultCellStyle.Format = "f0";


            sqlConn.Close();

            //initHTDefault();
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
            sqlComm.CommandText = "SELECT 销售退出汇总表.单据编号, 销售退出汇总表.日期, 业务员.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 销售退出汇总表.备注, 单位表.单位编号, 单位表.单位名称, 销售退出汇总表.价税合计, 销售退出汇总表.发票号, 销售退出汇总表.支票号, 销售退出汇总表.部门ID, 销售退出汇总表.BeActive FROM 销售退出汇总表 INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID INNER JOIN 职员表 业务员 ON 销售退出汇总表.业务员ID = 业务员.ID INNER JOIN 职员表 操作员 ON 销售退出汇总表.操作员ID = 操作员.ID WHERE (销售退出汇总表.ID = " + iDJID.ToString() + ")";
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

                comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                textBoxDWMC.Text = sqldr.GetValue(6).ToString();
                textBoxFPH.Text = sqldr.GetValue(8).ToString();
                textBoxZPH.Text = sqldr.GetValue(9).ToString();

                this.Text = "销售退出制单：" + labelDJBH.Text;
            }
            sqldr.Close();
            /*
            comboBoxBM.SelectedIndexChanged -= comboBoxBM_SelectedIndexChanged;
            sqlComm.CommandText = "SELECT 部门表.部门名称 FROM 部门表 INNER JOIN 职员表 ON 部门表.ID = 职员表.岗位ID WHERE (职员表.职员姓名 = N'" + comboBoxYWY.Text + "')";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                comboBoxBM.Text = sqldr.GetValue(0).ToString();
                break;
            }
            sqldr.Close();
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;
             */
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
            sqlComm.CommandText = "SELECT 销售退出明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 销售退出明细表.数量, 销售退出明细表.单价, 销售退出明细表.金额, 销售退出明细表.扣率, 销售退出明细表.实计金额, 销售退出明细表.商品ID, 销售退出明细表.库房ID, 商品表.库存数量, 销售退出明细表.ID AS Expr1, 销售退出明细表.赠品, 销售退出明细表.ID AS Expr2 FROM 销售退出明细表 INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 销售退出明细表.库房ID = 库房表.ID WHERE (销售退出明细表.单据ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[15].Visible = false;
            dataGridViewDJMX.Columns[17].Visible = false;

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
            dataGridViewDJMX.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[16].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[14].DefaultCellStyle.Format = "f0";

            dataGridViewDJMX.ShowCellErrors = true;
            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;


            sqlConn.Close();

            dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            dataGridViewDJMX.CellDoubleClick -= dataGridViewDJMX_CellDoubleClick;
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

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(2, "") == 0)
            {
                //return;
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
            intHTH = 0;
            textBoxHTH.Text = "";
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
                    intHTH = 0;
                    textBoxHTH.Text = "";
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
            }
        }

        private void textBoxHTH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getBillInformation(53, textBoxHTH.Text.Trim()) == 0)
            {
                textBoxHTH.Text = "";
                intHTH = 0;
                return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iBillCNumber;
                textBoxDWMC.Text = cGetInformation.strBillCName;
                textBoxDWBH.Text = cGetInformation.strBillCCode;
                textBoxHTH.Text = cGetInformation.strBillCode;
                intHTH = cGetInformation.iBillNumber;
                comboBoxBM.SelectedValue = cGetInformation.iBillBMID;
                comboBoxYWY.Text = cGetInformation.sPeopleName;

                getHTDetail();
                dataGridViewDJMX.Focus();
            }
            /*
            FormSelectHT frmSelectHT = new FormSelectHT();
            frmSelectHT.strConn = strConn;
            frmSelectHT.iSelectStyle = 3;
            frmSelectHT.ShowDialog();

            if (frmSelectHT.iCompanyNumber == 0)
            {
                textBoxHTH.Text = "";
                intHTH = 0;
            }
            else
            {
                iSupplyCompany = frmSelectHT.iCompanyNumber;
                textBoxDWBH.Text = frmSelectHT.strCompanyCode;
                textBoxDWMC.Text = frmSelectHT.strCompanyName;
                intHTH = frmSelectHT.iHTNumber;
                textBoxHTH.Text = frmSelectHT.strHTCode;

                getHTDetail();
            }*/
        }

        private void getHTDetail()
        {

            if (intHTH == 0)
                return;

            bCheck = false;
            sqlConn.Open();

            //sqlComm.CommandText = "SELECT 购进商品制单明细定义表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 购进商品制单明细定义表.库房编号, 购进商品制单明细定义表.库房名称, 销售合同明细表.数量, 销售合同明细表.单价, 销售合同明细表.总价 AS 金额, 购进商品制单明细定义表.扣率, 销售合同明细表.总价 AS 实计金额, 销售合同明细表.商品ID, 购进商品制单明细定义表.库房ID, 购进商品制单明细定义表.库存数量,  购进商品制单明细定义表.统计标志, 购进商品制单明细定义表.赠品 FROM 购进商品制单明细定义表 CROSS JOIN 销售合同明细表 INNER JOIN 商品表 ON 销售合同明细表.商品ID = 商品表.ID WHERE (销售合同明细表.销售合同ID = " + intHTH.ToString() + ")"
            
            sqlComm.CommandText = "SELECT 购进商品制单明细定义表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 销售合同明细表.数量, 销售合同明细表.单价, 销售合同明细表.总价 AS 金额, 购进商品制单明细定义表.扣率, 销售合同明细表.总价 AS 实计金额,销售合同明细表.商品ID, 商品分类表.库房ID, 购进商品制单明细定义表.库存数量, 购进商品制单明细定义表.统计标志, 购进商品制单明细定义表.赠品 FROM 商品分类表 INNER JOIN 库房表 ON 商品分类表.库房ID = 库房表.ID RIGHT OUTER JOIN 销售合同明细表 INNER JOIN 商品表 ON 销售合同明细表.商品ID = 商品表.ID ON 商品分类表.ID = 商品表.分类编号 CROSS JOIN 购进商品制单明细定义表 WHERE (销售合同明细表.销售合同ID = " + intHTH.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables["商品表"].Clear();
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
            //dataGridViewDJMX..Refresh();


            sqlConn.Close();
            //countAmount();

            if (dataGridViewDJMX.Rows.Count > 0)
                dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[dataGridViewDJMX.Rows.Count - 1].Cells[1];
            bCheck = true;
        }

        private void getKCL()
        {

            //未定库房
            if (dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[13].Value.ToString() == "" || dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[13].Value.ToString() == "0")
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[14].Value = 0;
                return;
            }

            //未定商品
            if (dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value.ToString() == "" || dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value.ToString() == "0")
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[14].Value = 0;
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 库存数量 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();

            string strKCL = "";
            while (sqldr.Read())
            {
                strKCL = sqldr.GetValue(0).ToString();
            }
            if (strKCL == "")
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[14].Value = 0;
            }
            else
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[14].Value = Convert.ToDecimal(strKCL);
            }
            sqlConn.Close();
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2) //商品编号
            {
                if (cGetInformation.getCommInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.iCommNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.decCommZZJJ.ToString();

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                    getKCL();

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[5];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }

            if (e.ColumnIndex == 5 || e.ColumnIndex == 6) //库房编号
            {
                if (cGetInformation.getKFInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                    getKCL();

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[7];

                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }
        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
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
                                if (dv.Rows[dv.CurrentCell.RowIndex].IsNewRow)
                                    return true;
                                dv.EndEdit();
                                switch (dv.CurrentCell.ColumnIndex)
                                {
                                    case 1:
                                    case 2:
                                        //dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[5];
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    //case 5:
                                    //case 6:
                                    //    dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                    //    break;
                                    case 7:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                        break;
                                    case 8:
                                    //    dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[10];
                                    //    break;
                                    //case 10:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[1];
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

        //return true 正确  false 错误
        private bool countAmount()
        {
            decimal fSum = 0, fSum1 = 0;
            decimal fTemp, fTemp1;
            decimal fCount = 0, fCSum = 0;

            this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            bool bCheck = true;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                //库房ID
                if (dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "0" )
                {
                    bCheck = false;
                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "请输入商品库房";
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "请输入商品库房";
                    bCheck = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[12].Value = 0;

                if (dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() == "0")
                {
                    bCheck = false;
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "请输入商品";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "请输入商品";
                }


                //数量
                if (dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() == "")
                    fTemp = 0;
                else
                    fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);

                if (fTemp == 0)
                {
                    dataGridViewDJMX.Rows[i].Cells[7].ErrorText = "商品数量必须大于0";
                    bCheck = false; 
                }

                if (!bCheck)
                    continue;

                fCSum += fTemp;

                //单价
                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);

                //扣率
                if (dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[10].Value = 100;
                }

                //赠品
                if (dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[16].Value = 0;
                }

                //金额
                if (Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[16].Value)) //赠品
                    dataGridViewDJMX.Rows[i].Cells[9].Value = 0;
                else
                    dataGridViewDJMX.Rows[i].Cells[9].Value = Math.Round(fTemp * fTemp1, 2);

                //实计金额
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                dataGridViewDJMX.Rows[i].Cells[11].Value = fTemp1 * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value) / 100;


                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value);

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

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int iRe = 0;

            if (!bCheck)
                return;
            if (isSaved)
                return;

            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            switch (e.ColumnIndex)
            {
                case 2: //商品编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = Math.Round(Decimal.Zero, 2);

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }

                    iRe = cGetInformation.getCommInformation(20, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewDJMX.CancelEdit();
                        return;
                    }
                    if (iRe == 0) //失败
                    //if (cGetInformation.getCommInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].ErrorText = "商品编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.decCommZZJJ.ToString();

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        getKCL();

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;

                case 1: //商品名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                        break;

                    }

                    iRe = cGetInformation.getCommInformation(10, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewDJMX.CancelEdit();
                        return;
                    }
                    if (iRe == 0) //失败
                    //if (cGetInformation.getCommInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].ErrorText = "商品助记码输入错误";
                    }
                    else
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.decCommZZJJ.ToString();

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        getKCL();

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    }
                    break;
                case 5: //库房编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].ErrorText = "库房编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        } 
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        getKCL();
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;
                case 6: //库房名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "库房助记码输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        } 
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        getKCL();
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }

                    break;
                case 7:  //商品数量
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut < 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "商品数量输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "商品数量输入类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 8: //商品价格
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;


                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        if (detOut >= 0)
                        {
                            detOut = Math.Round(detOut, 2);
                        }
                        else
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[8].ErrorText = "商品价格输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].ErrorText = "商品数量价格类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 10:  //扣率
                    double dOut = 0.0;
                    if (e.FormattedValue.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 100;
                        break;
                    }
                    if (Double.TryParse(e.FormattedValue.ToString(), out dOut))
                    {
                        if (dOut <= 0 || dOut > 100.0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[10].ErrorText = "商品扣率输入错误，请输入0.01-100.0之间的数字";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].ErrorText = "商品扣率输入错误，请输入0.01-100.0之间的数字";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;



            }
            dataGridViewDJMX.EndEdit();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;
            decimal dKCCBJTemp = 0;

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("销售退回制单已经保存,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("请选择单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("销售退回制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("没有选择退回商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if (MessageBox.Show("请检查销售退回制单内容，是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            //    return;

            saveToolStripButton.Enabled = false;
            string strCount = "", strDateSYS = "", strKey = "BTH";
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

                sqlComm.CommandText = "INSERT INTO 销售退出汇总表 (单位ID, 单据编号, 日期, 发票号, 支票号, 价税合计, 业务员ID, BeActive, 合同号, 操作员ID, 备注, 未付款金额, 部门ID) VALUES (" + iSupplyCompany.ToString() + ", N'" + strCount + "', '" + strDateSYS + "', N'" + textBoxFPH.Text + "', N'" + textBoxZPH.Text + "', " + labelSJJE.Text + ", " + comboBoxYWY.SelectedValue.ToString() + ", 1, N'" + textBoxHTH.Text.Trim() + "', " + intUserID.ToString() + ", N'" + textBoxBZ.Text + "', " + labelSJJE.Text + ","+sBMID+")";
                sqlComm.ExecuteNonQuery();

                //取得单据号 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //合同退出
                sqlComm.CommandText = "UPDATE 销售合同表 SET 退货标记 = 1 WHERE (ID = " + intHTH.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //单位应付账
                sqlComm.CommandText = "SELECT 应收账款 FROM 单位表 WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();

                dKCJE = 0;
                while (sqldr.Read())
                {
                    dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                }
                sqldr.Close();
                dKCJE1 = dKCJE - Convert.ToDecimal(labelSJJE.Text);
                sqlComm.CommandText = "UPDATE 单位表 SET 应收账款 = " + dKCJE1.ToString() + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //单位历史纪录
                sqlComm.CommandText = "INSERT INTO 单位历史账表 (单位ID, 日期, 单据编号, 摘要, 销出金额, 应收金额, 销售标记, 业务员ID, 冲抵单号, BeActive, 部门ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + strCount + "', N'销售退货单', -" + labelSJJE.Text.ToString() + ", " + dKCJE1.ToString() + ", 1, " + comboBoxYWY.SelectedValue.ToString() + ", N'" + textBoxHTH.Text + "', 1,"+sBMID+")";
                sqlComm.ExecuteNonQuery();

                //单据明细
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "0") //无库房
                    {
                        dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                        continue;
                    }

                    //库存成本价
                    sqlComm.CommandText = "SELECT 库存成本价 FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    dKCCBJTemp = 0;
                    while (sqldr.Read())
                    {
                        dKCCBJTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "INSERT INTO 销售退出明细表 (单据ID, 商品ID, 库房ID, 原单据ID, 单价, 金额, 扣率, 实计金额, 未付款金额, BeActive, 数量,未付款数量,库存成本价) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", 0, " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", 1," + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + "," + dKCCBJTemp.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                }

                //标志复位
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;
                    dataGridViewDJMX.Rows[i].Cells[15].Value = 1;
                }


                //总库存
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[15].Value) == 0) //已经计算过
                        continue;

                    //计算该单的每个商品库存金额
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value);

                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[15].Value) == 0) //已经计算过
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[12].Value == dataGridViewDJMX.Rows[i].Cells[12].Value) //同种商品
                        {
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[7].Value);
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[11].Value);
                            dataGridViewDJMX.Rows[j].Cells[15].Value = 0;
                        }

                    }
                    dKCCBJ1 = dKCJE1 / dKUL1;
                    dYSYE1 = dKCJE1;

                    //总库存变更
                    sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 库存金额, 应收金额 FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                    }
                    sqldr.Close();

                    //计算库存成本价
                    //dKCCBJ1 = cGetInformation.countKCCBJ(dKUL, dKCJE, dKUL1, dKCJE1);
                    //if (dKCCBJ1 > 0)
                    //    dKCCBJ = dKCCBJ1;
                    dZZJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString());

                    dKUL += dKUL1;
                    //dKCJE += dKCJE1;
                    dKCJE = dKUL * dKCCBJ;
                    dYSYE -= dYSYE1;

                    //sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存成本价=" + dKCCBJ.ToString() + ", 库存金额=" + dKCJE.ToString() + ", 应收金额= " + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                    sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额=" + dKCJE.ToString() + ", 应收金额= " + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //总账历史纪录
                    sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 退回数量, 退回单价, 退回金额, 总结存数量, 总结存金额, 应收金额, BeActive, 入库数量, 入库单价, 入库金额, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'销售退回单', " + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", " + dKCJE1 + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1," + dKUL1.ToString() + "," + dZZJJ1.ToString() + "," + dKCJE1.ToString() + ","+sBMID+")";
                    sqlComm.ExecuteNonQuery();
                }

                //标志复位
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    dataGridViewDJMX.Rows[i].Cells[15].Value = 1;
                }

                //分库存
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[15].Value) == 0) //已经计算过
                        continue;

                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value);

                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[15].Value) == 0) //已经计算过
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[12].Value == dataGridViewDJMX.Rows[i].Cells[12].Value && dataGridViewDJMX.Rows[j].Cells[13].Value == dataGridViewDJMX.Rows[i].Cells[13].Value) //同种商品，同样库存
                        {
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[7].Value);
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[11].Value);
                            dataGridViewDJMX.Rows[j].Cells[15].Value = 0;
                        }
                    }
                    dKCCBJ1 = dKCJE1 / dKUL1;
                    dYSYE1 = dKCJE1;

                   
                    //分库存更新
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价, 应收金额 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //存在库存
                    {
                        sqldr.Read();
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                        sqldr.Close();

                        //计算库存成本价
                        //dKCCBJ = cGetInformation.countKCCBJ(dKUL, dKCCBJ, dKUL1, dKCCBJ1);
                        //dKCCBJ = cGetInformation.countKCCBJ(dKUL, dKCJE, dKUL1, dKCJE1);
                        //if (dKCCBJ < 0)
                        //    dKCCBJ = dKCCBJ1;
                        dKUL += dKUL1;
                        //dKCJE += dKCJE1;
                        dKCJE = dKUL * dKCCBJ;
                        dYSYE -= dYSYE1;
                        dZZJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString());

                        //sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存成本价 = " + dKCCBJ.ToString() + ",库存金额=" + dKCJE.ToString() + ", 应收金额=" + dYSYE.ToString() + " WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                        sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额=" + dKCJE.ToString() + ", 应收金额=" + dYSYE.ToString() + " WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";

                    }
                    else //没有库存，增加库存
                    {
                        sqldr.Close();
                        dKUL = dKUL1;
                        dKCCBJ = dKCCBJ1;
                        dKCJE = dKCJE1;
                        dYSYE = (-1) * dYSYE1;
                        sqlComm.CommandText = "INSERT INTO 库存表 (库房ID, 商品ID, 库存数量, 库存成本价, 库存金额, 应收金额, BeActive) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + dKUL.ToString() + ", " + dKCCBJ.ToString() + ","+dKCJE.ToString()+", "+dYSYE.ToString()+", 1)";
                    }
                    sqlComm.ExecuteNonQuery();

                    //库房账历史纪录
                    sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 退回数量, 退回单价, 退回金额, 库房结存数量, 库房结存金额, 应收金额, BeActive, 入库数量, 入库单价, 入库金额, 部门ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ",'" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'销售退回单', " + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1, " + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", " + dKCJE1.ToString() + ","+sBMID+")";
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

            //MessageBox.Show("销售退回制单保存成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = "销售退回制单：" + labelDJBH.Text;
            isSaved = true;

            bool bClose = false;
            if (MessageBox.Show("销售退回制单保存成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                bClose = true;
            }

            //if (MessageBox.Show("是否继续开始另一份单据？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //{
            //    MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
            //    mdiT.销售退回制单EToolStripMenuItem_Click(null, null);
            //}

            if (bClose)
                this.Close();
 
        }

        private void FormXSTHZD_FormClosing(object sender, FormClosingEventArgs e)
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

        private void printToolStripButton_Click(object sender, EventArgs e)
        {

            if (!countAmount())
            {
                MessageBox.Show("销售退回验货单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "销售退回单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("销售退回验货单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "销售退回单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
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
                    intHTH = 0;
                    textBoxHTH.Text = "";
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
            }
        }

        private void textBoxHTH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {

                if (cGetInformation.getBillInformation(513, textBoxHTH.Text.Trim()) == 0)
                {
                    textBoxHTH.Text = "";
                    intHTH = 0;
                    return;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iBillCNumber;
                    textBoxDWMC.Text = cGetInformation.strBillCName;
                    textBoxDWBH.Text = cGetInformation.strBillCCode;
                    textBoxHTH.Text = cGetInformation.strBillCode;
                    intHTH = cGetInformation.iBillNumber;
                    comboBoxBM.SelectedValue = cGetInformation.iBillBMID;
                    comboBoxYWY.Text = cGetInformation.sPeopleName;
                    
                    //getCompanyInfoDetail();
                    getHTDetail();
                    dataGridViewDJMX.Focus();
                }
            }

                /*

            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getBillInformation(513, textBoxHTH.Text.Trim()) == 0)
                {
                    textBoxHTH.Text = "";
                    intHTH = 0;
                    return;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iBillCNumber;
                    textBoxDWMC.Text = cGetInformation.strBillCName;
                    textBoxDWBH.Text = cGetInformation.strBillCCode;
                    textBoxHTH.Text = cGetInformation.strBillCode;
                    intHTH = cGetInformation.iBillNumber;

                    getHTDetail();
                    dataGridViewDJMX.Focus();
                }
            }*/
            
        }




    }
}