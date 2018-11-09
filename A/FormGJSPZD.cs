using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormGJSPZD : Form
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

        //public bool isSaved = true;
        //public int iDJID = 11;

        public bool isSaved = false;
        public int iDJID = 0;
        private bool bCheck = true;

        public int iVersion = 1;

        
        public FormGJSPZD()
        {
            InitializeComponent();
        }

        private void FormGJSPZD_Load(object sender, EventArgs e)
        {
            int i;

            this.Left = 1;
            this.Top = 1;
            textBoxHTH.Focus();

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            
            if (isSaved)
            {
                dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
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
            comboBoxBM.SelectedValue = intUserBM;
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;

            //初始化支付方式
            sqlComm.CommandText = "SELECT ID, 支付方式 FROM 支付方式表";

            if (dSet.Tables.Contains("支付方式表")) dSet.Tables.Remove("支付方式表");
            sqlDA.Fill(dSet, "支付方式表");
            comboBoxZFFS.DataSource = dSet.Tables["支付方式表"];
            comboBoxZFFS.DisplayMember = "支付方式";
            comboBoxZFFS.ValueMember = "ID";
            comboBoxZFFS.Text = "";



            //初始化商品列表
            sqlComm.CommandText = "SELECT 购进商品制单明细定义表.保留, 商品表.商品名称, 商品表.商品编号,商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 购进商品制单明细表.数量, 购进商品制单明细表.单价, 购进商品制单明细表.金额, 购进商品制单明细表.赠品, 购进商品制单明细表.扣率, 购进商品制单明细表.实计金额, 购进商品制单明细表.商品ID, 购进商品制单明细表.库房ID, 商品表.最终进价, 购进商品制单明细表.ID, 商品表.最高进价, 商品表.最低进价, 商品表.库存数量 FROM 购进商品制单明细表 INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 购进商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (购进商品制单明细表.表单ID = 0)";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewGJSPZD.DataSource = dSet.Tables["商品表"];

            dataGridViewGJSPZD.Columns[0].Visible = false;
            dataGridViewGJSPZD.Columns[13].Visible = false;
            dataGridViewGJSPZD.Columns[14].Visible = false;
            dataGridViewGJSPZD.Columns[16].Visible = false;
            dataGridViewGJSPZD.Columns[3].ReadOnly = true;
            dataGridViewGJSPZD.Columns[4].ReadOnly = true;
            dataGridViewGJSPZD.Columns[9].ReadOnly = true;
            dataGridViewGJSPZD.Columns[12].ReadOnly = true;
            dataGridViewGJSPZD.Columns[17].ReadOnly = true;
            dataGridViewGJSPZD.Columns[18].ReadOnly = true;
            dataGridViewGJSPZD.Columns[19].ReadOnly = true;
            dataGridViewGJSPZD.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[18].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[19].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridViewGJSPZD.Columns[5].ReadOnly = true;
            dataGridViewGJSPZD.Columns[6].ReadOnly = true;

            dataGridViewGJSPZD.ShowCellErrors = true;

            dataGridViewGJSPZD.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewGJSPZD.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[12].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[15].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[17].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[18].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[19].DefaultCellStyle.Format = "f0";

            dataGridViewGJSPZD.Columns[15].Visible = false;
            dataGridViewGJSPZD.Columns[17].Visible = false;
            dataGridViewGJSPZD.Columns[18].Visible = false;

            
            sqlConn.Close();

            //initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT=cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;


        }

        private void initDJ()
        {
            int iBM = 0;
            sqlConn.Open();

            sqlComm.CommandText = "SELECT 购进商品制单表.单据编号, 购进商品制单表.日期, 业务员.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 购进商品制单表.备注,单位表.单位编号, 单位表.单位名称, 购进商品制单表.价税合计, 采购合同表.合同编号, 购进商品制单表.发票号, 购进商品制单表.付款方式, 购进商品制单表.部门ID, 购进商品制单表.BeActive FROM 购进商品制单表 INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 业务员 ON 购进商品制单表.业务员ID = 业务员.ID INNER JOIN 职员表 操作员 ON 购进商品制单表.操作员ID = 操作员.ID LEFT OUTER JOIN 采购合同表 ON 购进商品制单表.合同ID = 采购合同表.ID WHERE (购进商品制单表.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                if (sqldr.GetValue(11).ToString() != "")
                {
                    try
                    {
                        iBM = int.Parse(sqldr.GetValue(11).ToString());
                    }
                    catch
                    {
                        iBM = 0;
                    }

                }

                if (!bool.Parse(sqldr.GetValue(12).ToString()))
                {
                    labelDJBH.ForeColor = Color.Red;
                }

                comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                textBoxDWMC.Text = sqldr.GetValue(6).ToString();
                textBoxHTH.Text = sqldr.GetValue(8).ToString();
                textBoxFPH.Text = sqldr.GetValue(9).ToString();
                comboBoxZFFS.Text = sqldr.GetValue(10).ToString();



                this.Text = "购进商品制单：" + labelDJBH.Text;
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
            sqlComm.CommandText = "SELECT 购进商品制单明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 购进商品制单明细表.数量, 购进商品制单明细表.单价, 购进商品制单明细表.金额, 购进商品制单明细表.赠品, 购进商品制单明细表.扣率, 购进商品制单明细表.实计金额, 商品表.ID AS 商品ID, 库房表.ID AS 库房ID, 商品表.最终进价, 购进商品制单明细表.ID AS 保留ID, 商品表.最高进价, 商品表.最低进价, 商品表.库存数量 FROM 购进商品制单明细表 INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 购进商品制单明细表.库房ID = 库房表.ID WHERE (购进商品制单明细表.表单ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewGJSPZD.DataSource = dSet.Tables["商品表"];

            dataGridViewGJSPZD.Columns[0].Visible = false;
            dataGridViewGJSPZD.Columns[13].Visible = false;
            dataGridViewGJSPZD.Columns[14].Visible = false;
            dataGridViewGJSPZD.Columns[16].Visible = false;
            dataGridViewGJSPZD.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[18].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[19].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.ShowCellErrors = true;

            dataGridViewGJSPZD.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewGJSPZD.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[12].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[15].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[17].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[18].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[19].DefaultCellStyle.Format = "f0";

            dataGridViewGJSPZD.Columns[15].Visible = false;
            dataGridViewGJSPZD.Columns[17].Visible = false;
            dataGridViewGJSPZD.Columns[18].Visible = false;


            dataGridViewGJSPZD.ReadOnly = true;
            dataGridViewGJSPZD.AllowUserToAddRows = false;
            dataGridViewGJSPZD.AllowUserToDeleteRows = false;


            sqlConn.Close();

            dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
            dataGridViewGJSPZD.RowValidating -= dataGridViewGJSPZD_RowValidating;
            dataGridViewGJSPZD.CellDoubleClick -= dataGridViewGJSPZD_CellDoubleClick;

            dataGridViewGJSPZD.CellPainting += dataGridViewGJSPZD_CellPainting;
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
                if (cGetInformation.iBMID != 0)
                    comboBoxBM.SelectedValue = cGetInformation.iBMID;

                comboBoxYWY.Text = cGetInformation.sCompanyYWY;

            }
            intHTH = 0;
            textBoxHTH.Text = "";
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(12, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    intHTH = 0;
                    textBoxHTH.Text = "";
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
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

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(10, textBoxDWBH.Text.Trim()) == 0)
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
                    comboBoxYWY.Text = cGetInformation.sCompanyYWY;

                    intHTH = 0;
                    textBoxHTH.Text = "";
                }
            }
        }

        private void comboBoxBM_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            sqlConn.Open();
            //初始化员工列表
            if (comboBoxBM.Text.Trim()!="全部")
                sqlComm.CommandText = "SELECT 职员表.ID,职员表.职员姓名, 职员表.职员编号 FROM 职员表 INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (部门表.部门名称 = N'" + comboBoxBM.Text.Trim() + "') AND (职员表.beactive = 1)";
            else
                sqlComm.CommandText = "SELECT 职员表.ID,职员表.职员姓名, 职员表.职员编号 FROM 职员表 INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (职员表.beactive = 1)";

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
            if (cGetInformation.getBillInformation(50, textBoxHTH.Text.Trim()) == 0)
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
                dataGridViewGJSPZD.Focus();
            }
            
        }

        private void getHTDetail()
        {

            if (intHTH == 0)
                return;

            bCheck = false;



            sqlConn.Open();

            sqlComm.CommandText = "SELECT 购进商品制单明细定义表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 采购合同明细表.数量, 采购合同明细表.单价, 采购合同明细表.总价 AS 金额, 购进商品制单明细定义表.赠品, 购进商品制单明细定义表.扣率, 采购合同明细表.总价 AS 实计金额, 采购合同明细表.商品ID, 购进商品制单明细定义表.库房ID, 商品表.最终进价, 购进商品制单明细定义表.ID AS 保留ID, 商品表.最高进价, 商品表.最低进价, 商品表.库存数量 FROM 采购合同明细表 INNER JOIN 商品表 ON 采购合同明细表.商品ID = 商品表.ID CROSS JOIN 购进商品制单明细定义表 LEFT OUTER JOIN 库房表 ON 购进商品制单明细定义表.库房ID = 库房表.ID WHERE (采购合同明细表.采购合同ID = " + intHTH.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewGJSPZD.DataSource = dSet.Tables["商品表"];


            dataGridViewGJSPZD.Columns[0].Visible = false;
            dataGridViewGJSPZD.Columns[13].Visible = false;
            dataGridViewGJSPZD.Columns[14].Visible = false;
            dataGridViewGJSPZD.Columns[16].Visible = false;
            dataGridViewGJSPZD.Columns[3].ReadOnly = true;
            dataGridViewGJSPZD.Columns[4].ReadOnly = true;
            dataGridViewGJSPZD.Columns[9].ReadOnly = true;
            dataGridViewGJSPZD.Columns[12].ReadOnly = true;
            dataGridViewGJSPZD.Columns[17].ReadOnly = true;
            dataGridViewGJSPZD.Columns[18].ReadOnly = true;
            dataGridViewGJSPZD.Columns[19].ReadOnly = true;
            dataGridViewGJSPZD.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[18].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[19].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


            sqlConn.Close();

            //得到库房
            for (int i = 0; i < dataGridViewGJSPZD.Rows.Count; i++)
            {
                if (dataGridViewGJSPZD.Rows[i].IsNewRow)
                    continue;

                cGetInformation.iCommNumber = Convert.ToInt32(dataGridViewGJSPZD.Rows[i].Cells[13].Value);
                cGetInformation.getCommKF();

                dataGridViewGJSPZD.Rows[i].Cells[5].Value = cGetInformation.strKFCode;
                dataGridViewGJSPZD.Rows[i].Cells[6].Value = cGetInformation.strKFName;
                dataGridViewGJSPZD.Rows[i].Cells[14].Value = cGetInformation.iKFNumber;

            }

            countAmount();
            if (dataGridViewGJSPZD.Rows.Count>0)
                dataGridViewGJSPZD.CurrentCell = dataGridViewGJSPZD.Rows[dataGridViewGJSPZD.Rows.Count-1].Cells[1];

            bCheck = true;


        }

        private void dataGridViewGJSPZD_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                    this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = cGetInformation.decCommZZJJ.ToString();
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[17].Value = cGetInformation.decCommZGJJ.ToString();
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[18].Value = cGetInformation.decCommZDJJ.ToString();
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[19].Value = cGetInformation.decCommKCSL.ToString("f0");

                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;


                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                    dataGridViewGJSPZD.EndEdit();
                    dataGridViewGJSPZD.CurrentCell = dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7];
                    dataGridViewGJSPZD.BeginEdit(true);
                    this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

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
                    this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;

                    dataGridViewGJSPZD.EndEdit();
                    dataGridViewGJSPZD.CurrentCell = dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7];
                    dataGridViewGJSPZD.BeginEdit(true);
                    this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                }
            }
            
        }

        private void dataGridViewGJSPZD_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridViewGJSPZD_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (isSaved)
                return;

            int iRe = 0;

            if (dataGridViewGJSPZD.Rows[e.RowIndex].IsNewRow)
                return;

            if (!bCheck)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);
            switch (e.ColumnIndex)
            {
                case 2: //商品编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[17].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[18].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[19].Value = Math.Round(Decimal.Zero, 0);


                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2); 
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                        break;

                    }

                    iRe = cGetInformation.getCommInformation(20, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewGJSPZD.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewGJSPZD.CancelEdit();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].ErrorText = "商品编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                            break;
                        }
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = cGetInformation.decCommZZJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[17].Value = cGetInformation.decCommZGJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[18].Value = cGetInformation.decCommZDJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[19].Value = cGetInformation.decCommKCSL.ToString("f0");

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                    }
                    break;

                case 1: //商品名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[17].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[18].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[9].Value = Math.Round(Decimal.Zero, 0);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;


                        break;

                    }


                    iRe = cGetInformation.getCommInformation(10, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewGJSPZD.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //失败
                    {
                        e.Cancel = true;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].ErrorText = "商品助记码输入错误";
                    }
                    else
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                            break;
                        }
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = cGetInformation.decCommZZJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[17].Value = cGetInformation.decCommZGJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[18].Value = cGetInformation.decCommZDJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[19].Value = cGetInformation.decCommKCSL.ToString("f0");


                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                    }
                    break;
                case 5: //库房编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = 0;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = "";
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewGJSPZD.CancelEdit();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].ErrorText = "库房编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                            break;
                        }
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                    }
                    break;
                case 6: //库房名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = 0;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = "";
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewGJSPZD.CancelEdit();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].ErrorText = "库房助记码输入错误";
                    }
                    else
                    {

                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                            break;
                        }
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                    }

                    break;
                case 7:  //商品数量
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut <= 0)
                        {
                            dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].ErrorText = "商品数量输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].ErrorText = "商品数量输入类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 8: //商品价格
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;
                    if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value.ToString() == "")
                    {
                        MessageBox.Show("请先输入购进商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                        break;
                    }

                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        if (detOut >= 0)
                        {
                            detOut = Math.Round(detOut, 2);

                            if (detOut.CompareTo(dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value) > 0)
                            {
                                if (MessageBox.Show("商品价格高于最终进价，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                    e.Cancel = true;
                                else
                                {
                                    this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
                                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].Value = detOut;
                                    this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                                }

                            }
                        }
                        else
                        {
                            dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].ErrorText = "商品价格输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].ErrorText = "商品数量价格类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 11:  //扣率
                    double dOut = 0.0;
                    if (e.FormattedValue.ToString() == "")
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[11].Value = 100;
                        break;
                    }
                    if (Double.TryParse(e.FormattedValue.ToString(), out dOut))
                    {
                        if (dOut <0 || dOut > 100.0)
                        {
                            dataGridViewGJSPZD.Rows[e.RowIndex].Cells[11].ErrorText = "商品扣率输入错误，请输入0-100.0之间的数字";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[11].ErrorText = "商品扣率输入错误，请输入0-100.0之间的数字";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;



            }
            dataGridViewGJSPZD.EndEdit();

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
                                dv.EndEdit();
                                switch (dv.CurrentCell.ColumnIndex)
                                {
                                    case 1:
                                    case 2:
                                    case 5:
                                    case 6: 
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    case 7:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                        break;
                                    case 8:
                                        //dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[11];
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[1];
                                        break;
                                    case 11:
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

        private void dataGridViewGJSPZD_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {

            countAmount();
        }

        private bool countAmount()
        {
            decimal fSum = 0, fSum1 = 0; 
            decimal fTemp, fTemp1;
            decimal fCount = 0,fCSum=0;
            bool bCheck = true;

            this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
            cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);

            for (int i = 0; i < dataGridViewGJSPZD.Rows.Count; i++)
            {
                if (dataGridViewGJSPZD.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() == "" || dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() == "0")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[1].ErrorText = "输入所购商品";
                    dataGridViewGJSPZD.Rows[i].Cells[2].ErrorText = "输入所购商品";
                    bCheck = false;
                }

                if (dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[2].ErrorText = "输入所购商品";
                    bCheck = false;
                }
                if (dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[8].ErrorText = "输入所购商品价格";
                    bCheck = false;
                }
                if (dataGridViewGJSPZD.Rows[i].Cells[11].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[11].Value = 100;
                }

                //最高低进价
                if (dataGridViewGJSPZD.Rows[i].Cells[17].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[17].Value = 0;
                }
                if (dataGridViewGJSPZD.Rows[i].Cells[18].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[18].Value = 0;
                }

                if (!bCheck)
                    continue;


                //数量
                if (dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() == "")
                    fTemp = 0;
                else
                    fTemp = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[7].Value);
                fCSum += fTemp;

                //单价
                if (dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[8].Value);

                if (dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() != "" && dataGridViewGJSPZD.Rows[i].Cells[15].Value.ToString()!="")
                {
                    if (Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[8].Value) > Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[15].Value))
                        dataGridViewGJSPZD.Rows[i].Cells[8].Style.BackColor = Color.LightPink;
                    else
                        dataGridViewGJSPZD.Rows[i].Cells[8].Style.BackColor = Color.White;
                }

                //金额
                dataGridViewGJSPZD.Rows[i].Cells[9].Value = Math.Round(fTemp * fTemp1, 2);

                //扣率
                if (dataGridViewGJSPZD.Rows[i].Cells[11].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[11].Value = 100;
                }

                //
                if (dataGridViewGJSPZD.Rows[i].Cells[10].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[10].Value = 0;
                }

                //赠品
                /*
                if (Convert.ToBoolean(dataGridViewGJSPZD.Rows[i].Cells[10].Value))
                {
                    dataGridViewGJSPZD.Rows[i].Cells[11].Value = 0.0;
                }
                 */
                fTemp = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[11].Value);

                //实计金额
                fTemp1 = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[9].Value);

                if (!Convert.ToBoolean(dataGridViewGJSPZD.Rows[i].Cells[10].Value)) //赠品
                    dataGridViewGJSPZD.Rows[i].Cells[12].Value = fTemp * fTemp1/100;
                else
                    dataGridViewGJSPZD.Rows[i].Cells[12].Value = 0;


                fSum += Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[9].Value);
                fSum1 += Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[12].Value);

                fCount += 1;
                
            }
            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
            dataGridViewGJSPZD.EndEdit();

            labelSLHJ.Text = fCSum.ToString("f0");
            labelJEHJ.Text = fSum.ToString("f2");
            labelSJJE.Text = fSum1.ToString("f2");

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;


        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i;
            decimal fZGJJ=0, fZDJJ=0;

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("购进商品制单已经保存,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("请选择单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("没有购进商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("购进商品制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //if (MessageBox.Show("请检查购进商品制单内容，是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            //    return;

            saveToolStripButton.Enabled = false;

            string strCount = "",strDateSYS="",strKey="AKP";
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
                sqlComm.CommandText = "SELECT 计数 FROM 表单计数表 WHERE (关键词 = N'"+strKey+"')";
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
                    if (iVersion <= 0)
                    {
                        if (int.Parse(strCount) > 2)
                        {
                            MessageBox.Show("预览版用户每天只可以做两单", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            sqlConn.Close();
                            return;
                        }
                    }

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

                sqlComm.CommandText = "INSERT INTO 购进商品制单表 (单据编号, 单位ID, 日期, 价税合计, 业务员ID, 操作员ID, 付款方式, 发票号, 合同ID, 备注, 入库标记, BeActive, 部门ID) VALUES (N'" + strCount + "', " + iSupplyCompany.ToString() + ", '" + strDateSYS + "', "+labelSJJE.Text+", "+comboBoxYWY.SelectedValue.ToString()+", "+intUserID.ToString()+", N'"+comboBoxZFFS.Text.Trim()+"', N'"+textBoxFPH.Text.Trim()+"', "+intHTH.ToString()+", N'"+textBoxBZ.Text.Trim()+"', 0, 1, "+sBMID+")";
                sqlComm.ExecuteNonQuery();

                //取得单据号 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //相关合同结束
                if (intHTH != 0)
                {
                    sqlComm.CommandText = "UPDATE 采购合同表 SET 执行标记 = 1 WHERE (ID = " + intHTH.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                //单位历史纪录
                sqlComm.CommandText = "INSERT INTO 单位历史账表 (单位ID, 日期, 单据编号, 摘要, 购进未入库金额, 业务员ID, 冲抵单号, BeActive) VALUES ( " + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + strCount + "', N'购进商品制单', " + labelSJJE.Text + ", "+comboBoxYWY.SelectedValue.ToString()+", N'"+textBoxHTH.Text+"', 1)";
                sqlComm.ExecuteNonQuery();


                //单据明细
                string strTemp = "";
                for (i = 0; i < dataGridViewGJSPZD.Rows.Count; i++)
                {
                    if (dataGridViewGJSPZD.Rows[i].IsNewRow)
                        continue;

                    strTemp = "";
                    if (dataGridViewGJSPZD.Rows[i].Cells[14].Value.ToString() == "")
                        strTemp = "NULL";
                    else
                        strTemp = dataGridViewGJSPZD.Rows[i].Cells[14].Value.ToString();

                    sqlComm.CommandText = "INSERT INTO 购进商品制单明细表 (表单ID, 商品ID, 库房ID, 数量, 单价, 金额, 赠品, 扣率, 实计金额, 未到货数量) VALUES (" + sBillNo + ", " + dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() + ", " + strTemp + ", " + dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[9].Value.ToString() + ", " + Convert.ToInt32(dataGridViewGJSPZD.Rows[i].Cells[10].Value).ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[11].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[12].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() + ")";
                   sqlComm.ExecuteNonQuery();

                    //商品库房历史表
                   sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (日期, 商品ID, 单位ID, 库房ID, 业务员ID, 单据编号, 摘要, 购进数量, 购进单价, 购进金额, BeActive, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[14].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'购进商品制单', " + dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[9].Value.ToString() + ", 1, " + sBMID + ")";
                   sqlComm.ExecuteNonQuery();

                   //商品历史表
                   sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 购进数量, 购进单价, 购进金额, BeActive, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'购进商品制单', " + dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[9].Value.ToString() + ", 1, " + sBMID + ")";
                   sqlComm.ExecuteNonQuery();

                  //商品进价更新
                    fZGJJ=decimal.Parse(dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString());
                    if (fZGJJ < decimal.Parse(dataGridViewGJSPZD.Rows[i].Cells[17].Value.ToString()))
                        fZGJJ = decimal.Parse(dataGridViewGJSPZD.Rows[i].Cells[17].Value.ToString());

                    fZDJJ = decimal.Parse(dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString());
                    if (fZDJJ > decimal.Parse(dataGridViewGJSPZD.Rows[i].Cells[18].Value.ToString()))
                        fZDJJ = decimal.Parse(dataGridViewGJSPZD.Rows[i].Cells[18].Value.ToString());

                    sqlComm.CommandText = "UPDATE 商品表 SET 最高进价 = " + fZGJJ.ToString("f2") + ", 最低进价 = " + fZDJJ.ToString("f2") + ", 最终进价 = " + dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() + " WHERE (ID = " + dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() + ")";
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

            labelDJBH.Text = strCount;
            this.Text = "购进商品制单：" + labelDJBH.Text;
            isSaved = true;

            bool bClose = false;
            //if (MessageBox.Show("购进商品制单保存成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //{
                bClose = true;
            //}

            if (MessageBox.Show("是否继续开始另一份制单？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                // 创建此子窗体的一个新实例。
                FormGJSPZD childFormGJSPZD = new FormGJSPZD();
                // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                childFormGJSPZD.MdiParent = this.MdiParent; 

                childFormGJSPZD.strConn = strConn;

                childFormGJSPZD.intUserID = intUserID;
                childFormGJSPZD.intUserLimit = intUserLimit;
                childFormGJSPZD.strUserLimit = strUserLimit;
                childFormGJSPZD.strUserName = strUserName;
                childFormGJSPZD.Show();
            }


            if (bClose)
                this.Close();

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("购进商品制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "购进商品制单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewGJSPZD, strT, false, intUserLimit);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("购进商品制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "购进商品制单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewGJSPZD, strT, true, intUserLimit);

        }

        private void FormGJSPZD_FormClosing(object sender, FormClosingEventArgs e)
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

        private void textBoxHTH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getBillInformation(510, textBoxHTH.Text.Trim()) == 0)
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
                    dataGridViewGJSPZD.Focus();
                }

            }

        }

        private void toolStripMenuItemUP_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender;
            ContextMenuStrip cmenu = (ContextMenuStrip)menuitem.GetCurrentParent();


            DataGridView dv = (DataGridView)(cmenu.SourceControl);

            if (dv.CurrentCell.RowIndex <= 0 || dv.Rows.Count <= 1 || dv.Rows[dv.CurrentCell.RowIndex].IsNewRow)
                return;

            int i, count = dv.ColumnCount;
            object[] dr1 = new object[count];

            for (i = 0; i < count; i++)
            {
                dr1[i] = dv.Rows[dv.CurrentCell.RowIndex - 1].Cells[i].Value;
            }

            for (i = 0; i < count; i++)
            {
                dv.Rows[dv.CurrentCell.RowIndex - 1].Cells[i].Value = dv.Rows[dv.CurrentCell.RowIndex].Cells[i].Value;
            }


            for (i = 0; i < count; i++)
            {
                dv.Rows[dv.CurrentCell.RowIndex].Cells[i].Value = dr1[i];
            }
            dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex - 1].Cells[dv.CurrentCell.ColumnIndex];

            countAmount();

        }

        private void toolStripMenuItemDOWN_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender;
            ContextMenuStrip cmenu = (ContextMenuStrip)menuitem.GetCurrentParent();


            DataGridView dv = (DataGridView)(cmenu.SourceControl);

            if (dv.CurrentCell.RowIndex >= dv.Rows.Count - 2 || dv.Rows.Count <= 1 || dv.Rows[dv.CurrentCell.RowIndex].IsNewRow)
                return;

            int i, count = dv.ColumnCount;
            object[] dr1 = new object[count];

            for (i = 0; i < count; i++)
            {
                dr1[i] = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[i].Value;
            }

            for (i = 0; i < count; i++)
            {
                dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[i].Value = dv.Rows[dv.CurrentCell.RowIndex].Cells[i].Value;
            }


            for (i = 0; i < count; i++)
            {
                dv.Rows[dv.CurrentCell.RowIndex].Cells[i].Value = dr1[i];
            }
            dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[dv.CurrentCell.ColumnIndex];

            countAmount();
        }

        private void dataGridViewGJSPZD_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex != 8 || e.RowIndex < 0)
                return;


            if (Convert.ToDecimal(dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].Value) > Convert.ToDecimal(dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value))
            {
                e.CellStyle.BackColor = Color.LightPink;
            }


        }






    }
}