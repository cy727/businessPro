using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormTMCX : Form
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

        private string strDJBH = "";
        private int intDJID = 0;
        private string sDJClass = "";

        private int intCommID = 0;
        private int intCommID1 = 0;
        System.Data.DataTable dTable = new System.Data.DataTable();

        private bool isSaved = false;
        private ClassGetInformation cGetInformation;
        
        public FormTMCX()
        {
            InitializeComponent();
        }

        private void FormTMCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            sqlConn.Open();
            //sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
            sqlComm.CommandText = "SELECT GETDATE()";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqlConn.Close();

            dTable.Columns.Add("商品编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("商品名称", System.Type.GetType("System.String"));
            dTable.Columns.Add("条码", System.Type.GetType("System.String"));
            dTable.Columns.Add("日期", System.Type.GetType("System.String"));
            dTable.Columns.Add("摘要", System.Type.GetType("System.String"));
            dTable.Columns.Add("单据编号", System.Type.GetType("System.String"));
            
            //dataGridViewKCTM.DataSource = dTable;
        }

        private void btnTM_Click(object sender, EventArgs e)
        {
            this.textBoxTM.Focus();
            this.textBoxTM.SelectAll();
        }

        private void textBoxTM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                textBoxTM.Text = textBoxTM.Text.ToUpper().Trim();

                if (textBoxTM.Text == "")
                {
                    labelWARN.ForeColor = Color.Red;
                    labelWARN.Text = "请录入商品条码";
                    return;
                }

                initTmVIEW();

                textBoxTM.SelectAll();
            }

        }

        private void initTmVIEW()
        {
            //是否有入库记录
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品条码表.ID, 商品条码表.单据编号, 商品条码表.摘要, 商品条码表.日期, 商品条码表.出入库标记 AS 出库, 职员表.职员姓名 AS 操作员 FROM 商品条码表 INNER JOIN 职员表 ON 商品条码表.操作员ID = 职员表.ID WHERE (商品条码表.条码 = N'" + textBoxTM.Text.ToUpper() + "') ORDER BY 商品条码表.日期";
            if (dSet.Tables.Contains("条码表")) dSet.Tables.Remove("条码表");
            sqlDA.Fill(dSet, "条码表");
            dataGridViewTM.DataSource = dSet.Tables["条码表"];
            dataGridViewTM.Columns[0].Visible = false;

            if (dSet.Tables["条码表"].Rows.Count < 1)
            {
                labelWARN.ForeColor = Color.Red;
                labelWARN.Text = "没有相关记录";
            }
            else
            {
                labelWARN.ForeColor = Color.Green;
                labelWARN.Text = "读取条码成功";
            }
            sqlConn.Close();
        }


        private void textBoxTM_Enter(object sender, EventArgs e)
        {
            textBoxTM.SelectAll();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                string strT = "商品条码记录(" + textBoxTM.Text.ToUpper() + ");　";
                PrintDGV.Print_DataGridView(dataGridViewTM, strT, true, intUserLimit);
            }
            if (tabControl1.SelectedIndex == 1)
            {
                string strT = "商品库存条码记录(" + textBoxSPMC.Text.ToUpper() + ");　";
                PrintDGV.Print_DataGridView(dataGridViewKCTM, strT, true, intUserLimit);
            }
            if (tabControl1.SelectedIndex == 2)
            {
                string strT = "条码记录(" + textBoxSPMC1.Text.ToUpper() + ");　";
                PrintDGV.Print_DataGridView(dataGridViewJL, strT, true, intUserLimit);
            }

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                string strT = "商品条码记录(" + textBoxTM.Text.ToUpper() + ");　";
                PrintDGV.Print_DataGridView(dataGridViewTM, strT, false, intUserLimit);
            }
            if (tabControl1.SelectedIndex == 1)
            {
                string strT = "商品库存条码记录(" + textBoxSPMC.Text.ToUpper() + ");　";
                PrintDGV.Print_DataGridView(dataGridViewKCTM, strT, false, intUserLimit);
            }
            if (tabControl1.SelectedIndex == 2)
            {
                string strT = "条码记录(" + textBoxSPMC1.Text.ToUpper() + ");　";
                PrintDGV.Print_DataGridView(dataGridViewJL, strT, false, intUserLimit);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewTM.SelectedRows.Count < 1)
            {
                MessageBox.Show("请选择要删除的条码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("是否要删除选定的条码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                //明细
                for (int i = 0; i < dataGridViewTM.SelectedRows.Count; i++)
                {

                    sqlComm.CommandText = "DELETE FROM 商品条码表 WHERE (ID = " + dataGridViewTM.SelectedRows[i].Cells[0].Value.ToString() + ")";
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
            MessageBox.Show("删除条码完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initTmVIEW();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxSP_DoubleClick(object sender, EventArgs e)
        {

        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;

                }

            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;

                }

            }
        }

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPBH.Text = cGetInformation.strCommCode;
                textBoxSPMC.Text = cGetInformation.strCommName;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i, j, k;

            sqlConn.Open();
            //商品条码
            sqlComm.CommandText = "SELECT DISTINCT 条码, 商品ID FROM 商品条码表";
            if(!checkBoxAll.Checked)
                sqlComm.CommandText +=" WHERE (商品ID = "+intCommID.ToString()+")";
            if (dSet.Tables.Contains("商品条码表")) dSet.Tables.Remove("商品条码表");
            sqlDA.Fill(dSet, "商品条码表");

            if (dSet.Tables.Contains("库存条码表")) dSet.Tables.Remove("库存条码表");
            toolStripProgressBarP.Maximum = dSet.Tables["商品条码表"].Rows.Count;
            for (i = 0; i < dSet.Tables["商品条码表"].Rows.Count; i++)
            {
                toolStripProgressBarP.Value = i;
                sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品条码表.条码, 商品条码表.日期, 商品条码表.摘要, 商品条码表.单据编号 FROM 商品条码表 INNER JOIN 商品表 ON 商品条码表.商品ID = 商品表.ID WHERE (商品条码表.ID =(SELECT MAX(ID) AS Expr1 FROM 商品条码表 AS 商品条码表_1 WHERE (条码 = '" + dSet.Tables["商品条码表"].Rows[i][0].ToString() + "')  AND (日期 <= CONVERT(DATETIME, '" + dateTimePickerEnd.Value.ToShortDateString() + " 12:59:59', 102)))) AND (商品条码表.出入库标记 = 0) ";
                sqlDA.Fill(dSet, "库存条码表");

            }
            toolStripProgressBarP.Value = toolStripProgressBarP.Maximum;
            sqlConn.Close();
            dataGridViewKCTM.DataSource = dSet.Tables["库存条码表"];
            //dataGridViewKCTM.Columns[6].Visible = false;
            toolStripStatusLabelS.Text = "共有" + dataGridViewKCTM .RowCount.ToString()+ "条记录";


            
        }

        private void textBoxSPBH1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH1.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID1 = cGetInformation.iCommNumber;
                    textBoxSPBH1.Text = cGetInformation.strCommCode;
                    textBoxSPMC1.Text = cGetInformation.strCommName;
                }

            }
        }

        private void textBoxSPMC1_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intCommID1 = cGetInformation.iCommNumber;
                textBoxSPBH1.Text = cGetInformation.strCommCode;
                textBoxSPMC1.Text = cGetInformation.strCommName;
            }
        }

        private void textBoxSPBH1_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intCommID1 = cGetInformation.iCommNumber;
                textBoxSPBH1.Text = cGetInformation.strCommCode;
                textBoxSPMC1.Text = cGetInformation.strCommName;
            }
        }

        private void textBoxSPMC1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC1.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID1 = cGetInformation.iCommNumber;
                    textBoxSPBH1.Text = cGetInformation.strCommCode;
                    textBoxSPMC1.Text = cGetInformation.strCommName;

                }

            }
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {

            sqlConn.Open();
            //商品条码
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品条码表.条码, 商品条码表.日期, 商品条码表.摘要, 商品条码表.单据编号 FROM 商品条码表 INNER JOIN 商品表 ON 商品条码表.商品ID = 商品表.ID WHERE     (商品条码表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (商品条码表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 12:59:59', 102)) ";
            if(!checkBoxAll1.Checked)
                sqlComm.CommandText +=" AND 商品表.ID="+intCommID1.ToString();

            if(!checkBoxCK.Checked || !checkBoxRK.Checked)
            {
                if(!checkBoxCK.Checked)
                    sqlComm.CommandText +=" AND (商品条码表.出入库标记 = 0) ";

                if (!checkBoxRK.Checked)
                    sqlComm.CommandText +=" AND (商品条码表.出入库标记 = 1) ";
            }

            if (dSet.Tables.Contains("条码记录表")) dSet.Tables.Remove("条码记录表");
            sqlDA.Fill(dSet, "条码记录表");
            sqlConn.Close();
            dataGridViewJL.DataSource = dSet.Tables["条码记录表"];
            toolStripStatusLabelS.Text = "共有" + dataGridViewJL.RowCount.ToString() + "条记录";

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(tabControl1.SelectedIndex==0)
                toolStripStatusLabelS.Text = "";
            if (tabControl1.SelectedIndex == 1)
                toolStripStatusLabelS.Text = "共有" + dataGridViewKCTM.RowCount.ToString() + "条记录";
            if (tabControl1.SelectedIndex == 2)
                toolStripStatusLabelS.Text = "共有" + dataGridViewJL.RowCount.ToString() + "条记录";

        }
    }
}