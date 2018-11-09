using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJHRKYHD : Form
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
        private int intAKP = 0;

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;
        private bool bCheck = true;
        
        public FormJHRKYHD()
        {
            InitializeComponent();
        }

        private void FormJHRKYHD_Load(object sender, EventArgs e)
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
            sqlComm.CommandText = "SELECT 进货入库汇总表.单据编号, 进货入库汇总表.日期, 业务员.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 进货入库汇总表.备注, 单位表.单位编号, 单位表.单位名称, 进货入库汇总表.价税合计, 购进商品制单表.单据编号 AS 购进单号, 进货入库汇总表.发票号, 进货入库汇总表.支票号, 进货入库汇总表.部门ID, 进货入库汇总表.BeActive FROM 进货入库汇总表 INNER JOIN 单位表 ON 进货入库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 业务员 ON 进货入库汇总表.业务员ID = 业务员.ID INNER JOIN 职员表 操作员 ON 进货入库汇总表.操作员ID = 操作员.ID LEFT OUTER JOIN 购进商品制单表 ON 进货入库汇总表.购进ID = 购进商品制单表.ID WHERE (进货入库汇总表.ID = " + iDJID.ToString() + ")";
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
                textBoxZPH.Text = sqldr.GetValue(10).ToString();

                this.Text = "进货入库验货单：" + labelDJBH.Text;
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
            sqlComm.CommandText = "SELECT CONVERT(bit, 1) AS 到货, 购进商品制单表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称,进货入库明细表.数量, 进货入库明细表.单价, 进货入库明细表.金额, 进货入库明细表.扣率, 进货入库明细表.实计金额, 进货入库明细表.数量 AS 未到货数量, 进货入库明细表.商品ID, 进货入库明细表.库房ID, 进货入库明细表.ID AS Expr1, 进货入库明细表.赠品, 进货入库明细表.ID AS Expr2 FROM 进货入库明细表 INNER JOIN 商品表 ON 进货入库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 进货入库明细表.库房ID = 库房表.ID INNER JOIN 购进商品制单表 ON 进货入库明细表.原单据ID = 购进商品制单表.ID WHERE (进货入库明细表.单据ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
            dataGridViewDJMX.Columns[14].Visible = false;
            dataGridViewDJMX.Columns[15].Visible = false;
            dataGridViewDJMX.Columns[16].Visible = false;
            dataGridViewDJMX.Columns[18].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;


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
            dataGridViewDJMX.ShowCellErrors = true;

            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[12].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[13].DefaultCellStyle.Format = "f0";

            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;


            sqlConn.Close();

            dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            dataGridViewDJMX.CellDoubleClick -= dataGridViewDJMX_CellDoubleClick;
            countAmount();
        }

        private void comboBoxBM_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            sqlConn.Open();
            //初始化员工列表
            if (comboBoxBM.Text.Trim() != "全部")
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
            dataGridViewDJMX.Columns[9].ReadOnly = true;
            dataGridViewDJMX.Columns[10].ReadOnly = true;
            dataGridViewDJMX.Columns[11].ReadOnly = true;
            dataGridViewDJMX.Columns[12].ReadOnly = true;
            dataGridViewDJMX.Columns[13].ReadOnly = true;
            dataGridViewDJMX.Columns[17].ReadOnly = true;
            dataGridViewDJMX.Columns[18].Visible= false;
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

            dataGridViewDJMX.ShowCellErrors = true;
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[12].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[13].DefaultCellStyle.Format = "f0";

            checkBoxAll.Checked = false;

            //得到库房
            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() != "" && dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() != "0")
                    continue;

                cGetInformation.iCommNumber = Convert.ToInt32(dataGridViewDJMX.Rows[i].Cells[14].Value);
                cGetInformation.getCommKF();

                dataGridViewDJMX.Rows[i].Cells[6].Value = cGetInformation.strKFCode;
                dataGridViewDJMX.Rows[i].Cells[7].Value = cGetInformation.strKFName;
                dataGridViewDJMX.Rows[i].Cells[15].Value = cGetInformation.iKFNumber;
            }

        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1, "") == 0)
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
            strSelect = "SELECT 购进商品制单明细定义表.到货, 购进商品制单表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 购进商品制单明细表.未到货数量 AS 到货数量, 购进商品制单明细表.单价, 购进商品制单明细表.金额, 购进商品制单明细表.扣率, 购进商品制单明细表.实计金额, 购进商品制单明细表.未到货数量, 购进商品制单明细表.商品ID,购进商品制单明细表.库房ID, 购进商品制单明细表.ID,购进商品制单明细表.赠品,购进商品制单表.ID, 购进商品制单明细表.ID FROM 购进商品制单明细表 INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 购进商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (购进商品制单表.单位ID = " + iSupplyCompany + ")  AND (购进商品制单明细表.未到货数量 > 0) ORDER BY 购进商品制单表.单据编号";

            initdataGridViewDJMX();

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

                strSelect = "SELECT 购进商品制单明细定义表.到货, 购进商品制单表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 购进商品制单明细表.未到货数量 AS 到货数量, 购进商品制单明细表.单价, 购进商品制单明细表.金额, 购进商品制单明细表.扣率, 购进商品制单明细表.实计金额, 购进商品制单明细表.未到货数量, 购进商品制单明细表.商品ID,购进商品制单明细表.库房ID, 购进商品制单明细表.ID,购进商品制单明细表.赠品,购进商品制单表.ID, 购进商品制单明细表.ID   FROM 购进商品制单明细表 INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 购进商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (购进商品制单表.单位ID = " + iSupplyCompany + ") AND (购进商品制单表.入库标记 = 0) AND (购进商品制单表.BeActive = 1)  AND (购进商品制单明细表.未到货数量 > 0) ORDER BY 购进商品制单表.单据编号";

                initdataGridViewDJMX();
                dataGridViewDJMX.Focus();
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
                strSelect = "SELECT 购进商品制单明细定义表.到货, 购进商品制单表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 购进商品制单明细表.未到货数量 AS 到货数量, 购进商品制单明细表.单价, 购进商品制单明细表.金额, 购进商品制单明细表.扣率, 购进商品制单明细表.实计金额, 购进商品制单明细表.未到货数量, 购进商品制单明细表.商品ID,购进商品制单明细表.库房ID, 购进商品制单明细表.ID ,购进商品制单明细表.赠品,购进商品制单表.ID, 购进商品制单明细表.ID  FROM 购进商品制单明细表 INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 购进商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (购进商品制单表.单位ID = " + iSupplyCompany + ") AND (购进商品制单表.入库标记 = 0) AND (购进商品制单表.BeActive = 1)  AND (购进商品制单明细表.未到货数量 > 0) ORDER BY 购进商品制单表.单据编号";

                initdataGridViewDJMX();
                dataGridViewDJMX.Focus();
            }
            

        }

        private void textBoxHTH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getBillInformation(1, "") == 0)
            {
                textBoxHTH.Text = "";
                intAKP = 0;
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

                intAKP = cGetInformation.iBillNumber;

                sqlComm.CommandText = "SELECT 购进商品制单表.备注 FROM 购进商品制单表 WHERE (购进商品制单表.ID = " + cGetInformation.iBillNumber + ")";
                sqlConn.Open();
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    textBoxBZ.Text = sqldr.GetValue(0).ToString();
                }
                sqldr.Close();
                sqlConn.Close();

                strSelect = "SELECT 购进商品制单明细定义表.到货, 购进商品制单表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 购进商品制单明细表.未到货数量 AS 到货数量, 购进商品制单明细表.单价, 购进商品制单明细表.金额, 购进商品制单明细表.扣率, 购进商品制单明细表.实计金额, 购进商品制单明细表.未到货数量, 购进商品制单明细表.商品ID,购进商品制单明细表.库房ID, 购进商品制单明细表.ID,购进商品制单明细表.赠品, 购进商品制单表.ID, 购进商品制单明细表.ID FROM 购进商品制单明细表 INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 购进商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (购进商品制单明细表.表单ID = " + cGetInformation.iBillNumber + ") AND (购进商品制单表.入库标记 = 0) AND (购进商品制单表.BeActive = 1)  AND (购进商品制单明细表.未到货数量 > 0) ORDER BY 购进商品制单表.单据编号";

                initdataGridViewDJMX();

            }

        }

        private void textBoxHTH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getBillInformation(10, textBoxHTH.Text.Trim()) == 0)
                {
                    textBoxHTH.Text = "";
                    intAKP = 0;
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

                    intAKP = cGetInformation.iBillNumber;

                    sqlComm.CommandText = "SELECT 购进商品制单表.备注 FROM 购进商品制单表 WHERE (购进商品制单表.ID = " + cGetInformation.iBillNumber + ")";
                    sqlConn.Open();
                    sqldr = sqlComm.ExecuteReader();

                    while (sqldr.Read())
                    {
                        textBoxBZ.Text = sqldr.GetValue(0).ToString();
                    }
                    sqldr.Close();
                    sqlConn.Close();

                    strSelect = "SELECT 购进商品制单明细定义表.到货, 购进商品制单表.单据编号, 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 购进商品制单明细表.未到货数量 AS 到货数量, 购进商品制单明细表.单价, 购进商品制单明细表.金额, 购进商品制单明细表.扣率, 购进商品制单明细表.实计金额, 购进商品制单明细表.未到货数量, 购进商品制单明细表.商品ID,购进商品制单明细表.库房ID, 购进商品制单明细表.ID,购进商品制单明细表.赠品,购进商品制单表.ID, 购进商品制单明细表.ID FROM 购进商品制单明细表 INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 购进商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (购进商品制单明细表.表单ID = " + cGetInformation.iBillNumber + ") AND (购进商品制单表.入库标记 = 0) AND (购进商品制单表.BeActive = 1)  AND (购进商品制单明细表.未到货数量 > 0) ORDER BY 购进商品制单表.单据编号";

                    initdataGridViewDJMX();
                }
                dataGridViewDJMX.Focus();
            }
        }



        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 6 || e.ColumnIndex == 7) //库房编号
            {
                if (cGetInformation.getKFInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = cGetInformation.iKFNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.strKFName;

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[8];
                    dataGridViewDJMX.BeginEdit(false);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

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
                case 6: //库房编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "库房编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.strKFName;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;
                case 7: //库房名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "库房助记码输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.strKFName;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }

                    break;
                case 8:  //商品数量
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut <= 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[8].ErrorText = "商品到货数量输入错误";
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
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].ErrorText = "商品到货数量输入类型错误";
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
                                    case 6:
                                    case 7:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                        break;
                                    case 8:
                                        if (dv.CurrentCell.RowIndex==dv.RowCount-1)
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

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

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

                //进货标志
                if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                }
                if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value))
                    continue;

                cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                if (dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "输入购进商品库房";
                    dataGridViewDJMX.Rows[i].Cells[7].ErrorText = "输入购进商品库房";
                    bCheck = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[14].Value = 0;

                if (dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() == "0")
                {
                    bCheck = false;
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "请输入商品";
                    dataGridViewDJMX.Rows[i].Cells[3].ErrorText = "请输入商品";
                }


                //数量
                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[8].Value = 0;


                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "输入入库数量";
                    bCheck = false;
                }

                if (!bCheck)
                    continue;



                fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                fCSum += fTemp;

                //单价
                if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                //金额
                dataGridViewDJMX.Rows[i].Cells[10].Value = Math.Round(fTemp * fTemp1, 2);

                //扣率
                if (dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[11].Value = 100;
                }

                //实计金额
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);

                if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[17].Value)) //赠品
                    dataGridViewDJMX.Rows[i].Cells[12].Value = fTemp1 * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value.ToString()) / 100;
                else
                    dataGridViewDJMX.Rows[i].Cells[12].Value = 0;
                

                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
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

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i,j;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE=0,dKCJE1=0,dYSYE=0,dYSYE1=0;

             textBoxHTH.Focus();

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("进货入库验货单已经保存,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("请选择单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (intAKP == 0)
            {
                MessageBox.Show("请选择购货单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (!countAmount())
            {
                MessageBox.Show("进货入库验货单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("没有选择入库商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if (MessageBox.Show("请检查进货入库验货单内容，是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
           //     return;


            saveToolStripButton.Enabled = false;

            string strCount = "", strDateSYS = "", strKey = "ADH";
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

                sqlComm.CommandText = "INSERT INTO 进货入库汇总表 (单位ID, 单据编号, 日期, 发票号, 支票号, 价税合计, 业务员ID, BeActive, 操作员ID, 未付款金额, 已付款金额, 付款标记, 备注, 购进ID, 部门ID) VALUES (" + iSupplyCompany.ToString() + ", N'" + strCount + "', '" + strDateSYS + "', N'" + textBoxFPH.Text + "', N'" + textBoxZPH.Text + "', " + labelSJJE.Text + ", " + comboBoxYWY.SelectedValue.ToString() + ", 1, " + intUserID.ToString() + ", " + labelSJJE.Text + ", 0, 0, N'" + textBoxBZ.Text + "', " + intAKP.ToString() + ","+sBMID+")";
                sqlComm.ExecuteNonQuery();

                //取得单据号 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //单位应付账
                sqlComm.CommandText = "SELECT 应付账款 FROM 单位表 WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqldr=sqlComm.ExecuteReader();

                dKCJE = 0;
                while (sqldr.Read())
                {
                    dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                }
                sqldr.Close();
                dKCJE1 = dKCJE + Convert.ToDecimal(labelSJJE.Text);
                sqlComm.CommandText = "UPDATE 单位表 SET 应付账款 = " + dKCJE1.ToString() + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //单位历史纪录
                sqlComm.CommandText = "INSERT INTO 单位历史账表 (单位ID, 日期, 单据编号, 摘要, 购进金额, 应付余额, 购进标记, 业务员ID, 冲抵单号, BeActive, 部门ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + strCount + "', N'进货入库验货单', " + labelSJJE.Text.ToString() + ", " + dKCJE1.ToString() + ", 1, " + comboBoxYWY.SelectedValue.ToString() + ", N'" + textBoxHTH.Text + "', 1,"+sBMID+")";
                sqlComm.ExecuteNonQuery();


                //单据明细
                string strTemp = "";
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    //进货标志
                    if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                    }
                    if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value))
                        continue;

                    strTemp = "";
                    if (dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() == "0") //无库房
                    {
                        dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                        continue;
                    }

                    if (Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[17].Value)) //赠品
                        sqlComm.CommandText = "INSERT INTO 进货入库明细表 (单据ID, 商品ID, 库房ID, 原单据ID, 数量, 单价, 金额, 扣率, 赠品, 实计金额, BeActive, 未付款金额, 已付款金额, 未付款数量, 已付款数量, 原单据明细ID) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[18].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + Convert.ToInt32(dataGridViewDJMX.Rows[i].Cells[17].Value).ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", 1, 0, 0, 0, 0," + dataGridViewDJMX.Rows[i].Cells[19].Value.ToString() + ")";
                    else
                        sqlComm.CommandText = "INSERT INTO 进货入库明细表 (单据ID, 商品ID, 库房ID, 原单据ID, 数量, 单价, 金额, 扣率, 赠品, 实计金额, BeActive, 未付款金额, 已付款金额, 未付款数量, 已付款数量, 原单据明细ID) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[18].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + Convert.ToInt32(dataGridViewDJMX.Rows[i].Cells[17].Value).ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", 1, " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", 0," + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", 0, " + dataGridViewDJMX.Rows[i].Cells[19].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    sqlComm.CommandText = "UPDATE 购进商品制单明细表 SET 未到货数量 =未到货数量-" + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                }

                //标志复位
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    dataGridViewDJMX.Rows[i].Cells[16].Value = 1;
                }


                //总库存
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    //进货标志
                    if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                    }

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[16].Value) == 0) //已经计算过
                        continue;

                    //计算该单的每个商品库存金额
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[12].Value);
                    dZGJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                    dZDJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                    dZZJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[16].Value) == 0) //已经计算过
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[14].Value == dataGridViewDJMX.Rows[i].Cells[14].Value) //同种商品
                        {
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[8].Value);
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[12].Value);
                            if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[9].Value) > dZGJJ1)
                                dZGJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[9].Value);

                            if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[9].Value) < dZDJJ1)
                                dZGJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[9].Value);
                            dZZJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[9].Value);

                            dataGridViewDJMX.Rows[j].Cells[16].Value = 0;
                        }

                    }
                    dKCCBJ1 = dKCJE1 / dKUL1;
                    dYSYE1=dKCJE1;

                    //总库存变更
                    sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 最高进价, 最低进价, 最终进价, 库存金额, 应付金额 FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dZGJJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        dZDJJ = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                        dZZJJ = Convert.ToDecimal(sqldr.GetValue(4).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(5).ToString());
                        dYSYE=Convert.ToDecimal(sqldr.GetValue(6).ToString());
                    }
                    sqldr.Close();

                    //计算库存成本价
                    dKCCBJ = cGetInformation.countKCCBJ(dKUL, dKCJE, dKUL1, dKCJE1);
                    if (dKCCBJ < 0)
                        dKCCBJ = dKCCBJ1;

                    //最高
                    if (dZGJJ1 > dZGJJ)
                        dZGJJ = dZGJJ1;
                    if (dZDJJ1 < dZDJJ)
                        dZDJJ1 = dZDJJ;
                    //最终
                    dZZJJ = dZZJJ1;

                    //余额
                    dYSYE+=dYSYE1;

                    dKUL += dKUL1;
                    dKCJE += dKCJE1;
                    sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存成本价 = " + dKCCBJ.ToString() + ", 最高进价 = " + dZGJJ.ToString() + ", 最低进价 = " + dZDJJ.ToString() + ", 最终进价 = " + dZZJJ.ToString() + ", 库存金额=" + dKCJE.ToString() + ", 应付金额=" + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //总账历史纪录
                    sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 入库数量, 入库单价, 入库金额, 总结存数量, 总结存金额, 应付金额, BeActive, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'进货入库验货单', " + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", " + dKCJE1 + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,"+sBMID+")";
                    sqlComm.ExecuteNonQuery();

                }

                //标志复位
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    //进货标志
                    if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                    }
                    dataGridViewDJMX.Rows[i].Cells[16].Value = 1;
                }

                //分库存
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[16].Value) == 0) //已经计算过
                        continue;

                    //进货标志
                    if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                    }
                    //计算该单的每个商品库存金额
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[12].Value);
                    dZGJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                    dZDJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                    dZZJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[16].Value) == 0) //已经计算过
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[14].Value == dataGridViewDJMX.Rows[i].Cells[14].Value && dataGridViewDJMX.Rows[j].Cells[15].Value == dataGridViewDJMX.Rows[i].Cells[15].Value) //同种商品，同样库存
                        {
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[8].Value);
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[12].Value);
                            if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[9].Value) > dZGJJ1)
                                dZGJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[9].Value);

                            if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[9].Value) < dZDJJ1)
                                dZGJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[9].Value);
                            dZZJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[9].Value);

                            dataGridViewDJMX.Rows[j].Cells[16].Value = 0;
                        }

                    }
                    dKCCBJ1 = dKCJE1 / dKUL1;
                    dYSYE1 = dKCJE1;

                    //分库存更新
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价, 应付金额 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ") AND (BeActive = 1)";
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
                        dKCCBJ = cGetInformation.countKCCBJ(dKUL, dKCJE, dKUL1, dKCJE1);
                        if (dKCCBJ < 0)
                            dKCCBJ = dKCCBJ1;
                        dKUL += dKUL1;
                        dKCJE += dKCJE1;
                        dYSYE+=dYSYE1;

                        sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存成本价 = " + dKCCBJ.ToString() + ",库存金额="+dKCJE.ToString()+", 应付金额="+dYSYE.ToString()+" WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ") AND (BeActive = 1)";

                    }
                    else //没有库存，增加库存
                    {
                        sqldr.Close();
                        dKUL = dKUL1;
                        dKCCBJ = dKCCBJ1;
                        dKCJE += dKCJE1;
                        dYSYE=dKCJE;

                        sqlComm.CommandText = "INSERT INTO 库存表 (库房ID, 商品ID, 库存数量, 库存成本价,库存金额,应付金额,BeActive) VALUES (" + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", " + dKUL.ToString() + ", " + dKCCBJ.ToString() + ", "+dKCJE.ToString()+","+dYSYE.ToString()+",1)";
                    }
                    sqlComm.ExecuteNonQuery();

                    //库房账历史纪录
                    sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 入库数量, 入库单价, 入库金额, 库房结存数量, 库房结存金额, 应付金额, BeActive, 部门ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ",'" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'进货入库验货单', " + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,"+sBMID+")";
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

            //changeKC();
            checkRKView();

            labelDJBH.Text = strCount;
            this.Text = "进货入库验货单：" + labelDJBH.Text;
            isSaved = true;

            bool bClose = false;
            //if (MessageBox.Show("进货入库验货单保存成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //{
                bClose = true;
            //}

            if (MessageBox.Show("进货入库验货单保存成功，是否继续开始另一份制单？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {

                // 创建此子窗体的一个新实例。
                FormJHRKYHD childFormJHRKYHD = new FormJHRKYHD();
                // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                childFormJHRKYHD.MdiParent = this.MdiParent;

                childFormJHRKYHD.strConn = strConn;

                childFormJHRKYHD.intUserID = intUserID;
                childFormJHRKYHD.intUserLimit = intUserLimit;
                childFormJHRKYHD.strUserLimit = strUserLimit;
                childFormJHRKYHD.strUserName = strUserName;
                childFormJHRKYHD.Show();

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
                    if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                    }

                    sqlComm.CommandText = "SELECT 购进商品制单明细表.ID FROM 购进商品制单明细表 INNER JOIN 购进商品制单表 ON 购进商品制单明细表.表单ID = 购进商品制单表.ID WHERE (购进商品制单明细表.未到货数量 <> 0) AND (购进商品制单明细表.表单ID = "+dataGridViewDJMX.Rows[i].Cells[18].Value.ToString()+") AND (购进商品制单表.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //存在未到货明细
                    {
                        sqldr.Close();
                    }
                    else
                    {
                        sqldr.Close();
                        sqlComm.CommandText = "UPDATE 购进商品制单表 SET 入库标记 = 1 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[18].Value.ToString() + ")";
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

        private void FormJHRKYHD_FormClosing(object sender, FormClosingEventArgs e)
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
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("进货入库验货单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "进货入库验货单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("进货入库验货单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "进货入库验货单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

    }

}