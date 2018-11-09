using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormTJTZD2 : Form
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

        private ClassGetInformation cGetInformation;

        private int intHTH = 0;

        private bool isSaved=false;

        public FormTJTZD2()
        {
            InitializeComponent();
        }

        private void FormTJTZD2_Load(object sender, EventArgs e)
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

        }

        private void textBoxHTH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getBillInformation(4, "") == 0)
            {
                return;
            }
            else
            {
                intHTH = cGetInformation.iBillNumber;
                getBillDetail();
            }
        }

        private void getBillDetail()
        {
            if (intHTH == 0)
                return;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 单据编号, 日期, 备注 FROM 调价通知单汇总表 WHERE (ID = "+intHTH.ToString()+")";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                textBoxHTH.Text=sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");
                textBoxBZ.Text = sqldr.GetValue(2).ToString(); 
            }

            sqldr.Close();

            sqlComm.CommandText = "SELECT 调价通知单明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 调价通知单明细表.原进价, 调价通知单明细表.进价, 调价通知单明细表.原批发价, 调价通知单明细表.批发价, 调价通知单明细表.商品ID FROM 调价通知单明细表 INNER JOIN 商品表 ON 调价通知单明细表.商品ID = 商品表.ID WHERE (调价通知单明细表.单据ID = "+intHTH.ToString()+")";
            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[9].Visible = false;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[4].ReadOnly = true;
            dataGridViewDJMX.Columns[5].ReadOnly = true;
            dataGridViewDJMX.Columns[7].ReadOnly = true;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;

            sqlConn.Close();

        }

        private void textBoxHTH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getBillInformation(40, "") == 0)
                {
                    return;
                }
                else
                {
                    intHTH = cGetInformation.iBillNumber;
                    getBillDetail();
                }
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            int i, j;


            if (intHTH == 0)
            {
                MessageBox.Show("请选择调价通知单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                string strDT;
                cGetInformation.getSystemDateTime();
                strDT = cGetInformation.strSYSDATATIME;
                
                //表单汇总
                sqlComm.CommandText = "UPDATE 调价通知单汇总表 SET 执行标记 = 1, 执行时间 = '" + strDT + "' WHERE (ID = "+intHTH.ToString()+")";
                sqlComm.ExecuteNonQuery();

                //执行
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "UPDATE 商品表 SET 批发价 = " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", 进价 = " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

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

            //MessageBox.Show("调价通知单执行成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.btnAccept.Enabled = false;
            isSaved = true;

            if (MessageBox.Show("调价通知单执行成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }


        }

        private void FormTJTZD2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
            {
                return;
            }

            DialogResult dr = MessageBox.Show(this, "单据尚未执行，确定要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            string strT = "调价通知单(单据编号:" + labelDJBH.Text + ");执行日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "调价通知单(单据编号:" + labelDJBH.Text + ");执行日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanelContent_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}