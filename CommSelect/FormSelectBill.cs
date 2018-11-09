using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSelectBill : Form
    {
        public string strConn = "";
        public string strSelectText = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iBillNumber = 0;
        public string strBillCode = "";
        public int iBillCNumber = 0;
        public string strBillCCode = "";
        public string strBillCName = "";

        public int iPeopleNumber = 0;
        public string sPeopleName = "";

        public int iBillBMID = 0;

        private DataView dvSelect;

        public bool bShowDW = true;

        public FormSelectBill()
        {
            InitializeComponent();
        }

        private void FormSelectBill_Load(object sender, EventArgs e)
        {
            if (strSelectText == "")
            {
                this.Close();
            }

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            //初始化单据列表
            sqlComm.CommandText = strSelectText;
            sqlConn.Open();
            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");

            dvSelect = new DataView(dSet.Tables["单据表"]);
            dataGridViewBILL.DataSource = dvSelect;

            dataGridViewBILL.Columns[0].Visible = false;
            dataGridViewBILL.Columns[1].Visible = false;
            dataGridViewBILL.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewBILL.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewBILL.Columns[8].Visible = false;
            dataGridViewBILL.Columns[10].Visible = false;

            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.Text = "";

            sqlConn.Close();

            if (!bShowDW)
            {
                textBoxDWMC.Text = "";
                textBoxDWMC.Enabled = false;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iBillNumber = 0;
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dataGridViewBILL.SelectedRows.Count < 1)
            {
                iBillNumber = 0;
                this.Close();
                return; ;
            }
            iBillNumber = Int32.Parse(dataGridViewBILL.SelectedRows[0].Cells[0].Value.ToString());
            iBillCNumber = Int32.Parse(dataGridViewBILL.SelectedRows[0].Cells[1].Value.ToString());
            strBillCode = dataGridViewBILL.SelectedRows[0].Cells[2].Value.ToString();
            strBillCCode = dataGridViewBILL.SelectedRows[0].Cells[3].Value.ToString();
            strBillCName = dataGridViewBILL.SelectedRows[0].Cells[4].Value.ToString();

            try
            {
                iPeopleNumber = int.Parse(dataGridViewBILL.SelectedRows[0].Cells[8].Value.ToString());
            }
            catch
            {
                iPeopleNumber = 0;
            }
            sPeopleName = dataGridViewBILL.SelectedRows[0].Cells[9].Value.ToString();

            try
            {
                iBillBMID = int.Parse(dataGridViewBILL.SelectedRows[0].Cells[10].Value.ToString());
            }
            catch
            {
                iBillBMID = 0;
            }

            this.Close();
        }

        private void dataGridViewBILL_DoubleClick(object sender, EventArgs e)
        {
            btnSelect_Click(null,null);
        }

        private void textBoxKSSJ_Validating(object sender, CancelEventArgs e)
        {
            if (textBoxKSSJ.Text.Trim() == "")
            {
                this.errorProviderS.Clear();
                return;
            }

            try
            {
                DateTime.Parse(textBoxKSSJ.Text.Trim());
                this.errorProviderS.Clear();
            }
            catch
            {
                this.errorProviderS.SetError(this.textBoxKSSJ, "请输入有效日期，例如：2000-12-31");
                e.Cancel = true;
            }
        }

        private void textBoxJSSJ_Validating(object sender, CancelEventArgs e)
        {
            if (textBoxJSSJ.Text.Trim() == "")
            {
                this.errorProviderS.Clear();
                return;
            }

            try
            {
                DateTime.Parse(textBoxJSSJ.Text.Trim());
                this.errorProviderS.Clear();
            }
            catch
            {
                this.errorProviderS.SetError(this.textBoxJSSJ, "请输入有效日期，例如：2000-12-31");
                e.Cancel = true;
            }
        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.F9)
            {
                buttonQX_Click(null, null);
                return true;
            }
            if (keyData == Keys.F7)
            {
                buttonSX_Click(null, null);
                return true;
            }
            if (keyData == Keys.Enter && dataGridViewBILL.Focused)
            {
                //System.Windows.Forms.SendKeys.Send("{tab}");
                btnSelect_Click(null,null);//弹出修改窗口
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void buttonSX_Click(object sender, EventArgs e)
        {
            string sTemp = "";
            bool bFirst=true;

            if (textBoxDWMC.Text.Trim() != "")
            {
                sTemp = "单位名称 LIKE '%" + textBoxDWMC.Text.Trim() + "%'";
                bFirst = false;
            }

            if(textBoxDJBH.Text.Trim()!="")
            {
                if (bFirst) //第一次
                {
                    sTemp += "单据编号 LIKE '%" + textBoxDJBH.Text.Trim() + "%'";
                    bFirst = false;
                }
                else
                    sTemp += " AND 单据编号 LIKE '%" + textBoxDJBH.Text.Trim() + "%'";
            }

            if (comboBoxYWY.Text.Trim() != "")
            {
                if (bFirst) //第一次
                {
                    sTemp += "业务员 = '" + comboBoxYWY.Text.Trim() + "'";
                    bFirst = false;
                }
                else
                    sTemp += " AND 业务员 = '" + comboBoxYWY.Text.Trim() + "'";
            }

            if (textBoxKSSJ.Text.Trim() != "")
            {
                if (bFirst) //第一次
                {
                    sTemp += "日期 >= '"+textBoxKSSJ.Text.Trim()+"'";
                    bFirst = false;
                }
                else
                    sTemp += " AND 日期 >= '" + textBoxKSSJ.Text.Trim() + "'";
            }

            if (textBoxJSSJ.Text.Trim() != "")
            {
                if (bFirst) //第一次
                {
                    sTemp += "日期 <= '" + textBoxJSSJ.Text.Trim() + "'";
                    bFirst = false;
                }
                else
                    sTemp += " AND 日期 <= '" + textBoxJSSJ.Text.Trim() + "'";
            }

            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
            dvSelect.RowFilter = sTemp;

            dataGridViewBILL.Focus();
        }

        private void buttonQX_Click(object sender, EventArgs e)
        {
            dvSelect.RowFilter = "";
            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
            dataGridViewBILL.Focus();
        }

        private void textBoxS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                buttonSX_Click(null, null);
            }
        }





    }
}