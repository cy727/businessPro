using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSelectHT : Form
    {
        public string strConn = "";
        public string strSelectText = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iHTNumber = 0;
        public string strHTCode = "";
        public int iSelectStyle = 0;
        public string strHTSearch = "";

        public int iCompanyNumber = 0;
        public string strCompanyName = "";
        public string strCompanyCode = "";

        private DataView dvCommSelect;

        
        public FormSelectHT()
        {
            InitializeComponent();
        }

        private void FormSelectHT_Load(object sender, EventArgs e)
        {
            switch (iSelectStyle)
            {
                case 0: //进货合同
                    strSelectText = "SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 采购合同表.签订时间 FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 1) ORDER BY 采购合同表.签订时间 DESC";
                    break;
                case 1:  //已进货，非退货进货合同
                    strSelectText = "SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 采购合同表.签订时间 FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 0) AND (采购合同表.退货标记 = 0) ORDER BY 采购合同表.签订时间 DESC";
                    break;
                case 2:  //销售合同
                    strSelectText = "SELECT 销售合同表.ID, 销售合同表.供方单位ID, 销售合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 销售合同表.签订时间 FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID WHERE (销售合同表.BeActive = 1) ORDER BY 销售合同表.签订时间 DESC";
                    break;
                case 3:  //已出货，非退货销售合同
                    strSelectText = "SELECT 销售合同表.ID, 销售合同表.供方单位ID, 销售合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 销售合同表.签订时间 FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID WHERE (销售合同表.BeActive = 0) AND (销售合同表.退货标记 = 0) ORDER BY 销售合同表.签订时间 DESC";
                    break;
                case 4://所有合同
                    strSelectText = "(SELECT 销售合同表.ID, 销售合同表.供方单位ID, 销售合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 销售合同表.签订时间 FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID WHERE (销售合同表.BeActive = 1)) UNION (SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 采购合同表.签订时间 FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 1))";
                    break;

                case 10: //进货合同
                    strSelectText = "SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 采购合同表.签订时间 FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 1) AND (采购合同表.合同编号 LIKE '%" + strHTSearch + "%') ORDER BY 采购合同表.签订时间 DESC";
                    break;
                case 11:  //已进货，非退货进货合同
                    strSelectText = "SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 采购合同表.签订时间 FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 0) AND (采购合同表.退货标记 = 0) AND (采购合同表.合同编号 LIKE '%" + strHTSearch + "%') ORDER BY 采购合同表.签订时间 DESC";
                    break;
                case 12:  //销售合同
                    strSelectText = "SELECT 销售合同表.ID, 销售合同表.供方单位ID, 销售合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 销售合同表.签订时间 FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID WHERE (销售合同表.BeActive = 1) AND (销售合同表.合同编号 LIKE '%" + strHTSearch + "%') ORDER BY 采购合同表.签订时间 DESC";
                    break;
                case 13:  //已出货，非退货销售合同
                    strSelectText = "SELECT 销售合同表.ID, 销售合同表.供方单位ID, 销售合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 销售合同表.签订时间 FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID WHERE (销售合同表.BeActive = 0) AND (销售合同表.退货标记 = 0) AND (销售合同表.合同编号 LIKE '%" + strHTSearch + "%') ORDER BY 销售合同表.签订时间 DESC";
                    break;
                case 14://所有合同
                    strSelectText = "(SELECT 销售合同表.ID, 销售合同表.供方单位ID, 销售合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 销售合同表.签订时间 FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID WHERE (销售合同表.BeActive = 1) AND (销售合同表.合同编号 LIKE '%" + strHTSearch + "%') UNION (SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 采购合同表.签订时间 FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 1)) AND (销售合同表.合同编号 LIKE '%" + strHTSearch + "%')";
                    break;

                case 100:  //库存盘点准备表
                    strSelectText = "SELECT 库存盘点汇总表.ID, 库存盘点汇总表.库房ID, 库存盘点汇总表.单据编号, 库存盘点汇总表.日期, 库房表.库房编号, 库房表.库房名称, 职员表.职员姓名, 库存盘点汇总表.备注 FROM 库存盘点汇总表  LEFT OUTER JOIN 库房表 ON 库存盘点汇总表.库房ID = 库房表.ID INNER JOIN   职员表 ON 库存盘点汇总表.业务员ID = 职员表.ID WHERE (库存盘点汇总表.BeActive = 1) AND (库存盘点汇总表.盘点标记 = 0)";
                    this.Text = "选择盘点处理";
                    groupBox1.Text = "盘点列表";
                    break;
                case 110:  //库存盘点准备表
                    strSelectText = "SELECT 库存盘点汇总表.ID, 库存盘点汇总表.库房ID, 库存盘点汇总表.单据编号, 库存盘点汇总表.日期, 库房表.库房编号, 库房表.库房名称, 职员表.职员姓名, 库存盘点汇总表.备注 FROM 库存盘点汇总表  LEFT OUTER JOIN 库房表 ON 库存盘点汇总表.库房ID = 库房表.ID INNER JOIN   职员表 ON 库存盘点汇总表.业务员ID = 职员表.ID WHERE (库存盘点汇总表.BeActive = 1) AND (库存盘点汇总表.盘点标记 = 0) AND (库存盘点汇总表.单据编号 LIKE '%" + strHTSearch + "%')";
                    this.Text = "选择盘点处理";
                    groupBox1.Text = "盘点列表";
                    break;
                default:
                    strSelectText = "SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号, 单位表.单位名称, 单位表.单位编号, 职员表.职员姓名 AS 业务员, 采购合同表.签订时间 FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 1) ORDER BY 采购合同表.签订时间 DESC";
                    break;

            }

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            //初始化单位列表
            sqlComm.CommandText = strSelectText;
            sqlConn.Open();
            if (dSet.Tables.Contains("合同表")) dSet.Tables.Remove("合同表");
            sqlDA.Fill(dSet, "合同表");

            //dataGridViewHT.DataSource = dSet.Tables["合同表"];
            dvCommSelect = new DataView(dSet.Tables["合同表"]);
            dataGridViewHT.DataSource = dvCommSelect;

            dataGridViewHT.Columns[0].Visible = false;
            dataGridViewHT.Columns[1].Visible = false;
            dataGridViewHT.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewHT.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            sqlConn.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iHTNumber = 0;
            iCompanyNumber = 0;
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dataGridViewHT.SelectedRows.Count < 1)
            {
                iCompanyNumber = 0;
                iHTNumber = 0;
                this.Close();
                return; ;
            }
            iCompanyNumber = Int32.Parse(dataGridViewHT.SelectedRows[0].Cells[1].Value.ToString());
            strCompanyName = dataGridViewHT.SelectedRows[0].Cells[3].Value.ToString();
            strCompanyCode = dataGridViewHT.SelectedRows[0].Cells[4].Value.ToString();

            iHTNumber = Int32.Parse(dataGridViewHT.SelectedRows[0].Cells[0].Value.ToString());
            strHTCode = dataGridViewHT.SelectedRows[0].Cells[2].Value.ToString();

            this.Close();
        }

        private void dataGridViewHT_DoubleClick(object sender, EventArgs e)
        {
            btnSelect_Click(null, null);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            dvCommSelect.RowFilter = "";
            dvCommSelect.RowStateFilter = DataViewRowState.CurrentRows;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (textBoxHTH.Text.Trim() == "" && textBoxDW.Text.Trim() == "")
                return;

            dvCommSelect.RowStateFilter = DataViewRowState.CurrentRows;
            if (textBoxHTH.Text.Trim() != "")
            {
                dvCommSelect.RowFilter = "合同编号 LIKE '%" + textBoxHTH.Text.Trim().ToUpper() + "%'";
            }
            if (textBoxDW.Text.Trim() != "")
            {
                dvCommSelect.RowFilter = "单位名称 LIKE '%" + textBoxDW.Text.Trim().ToUpper() + "%'";
            }

        }
        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.F9)
            {
                btnAll_Click(null, null);
                return true;
            }
            if (keyData == Keys.F7)
            {
                btnSearch_Click(null, null);
                return true;
            }

            if (keyData == Keys.Enter && dataGridViewHT.Focused)
            {
                btnSelect_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}