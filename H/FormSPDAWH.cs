using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPDAWH : Form
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


        private ClassGetInformation cGetInformation;
        private DataView dvSelect;

        public int iVersion = 1;
        
        public FormSPDAWH()
        {
            InitializeComponent();
        }

        private void FormSPDAWH_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            this.Top = 1;
            this.Left = 1;


            initDataView(0);
        }
        private void initDataView(int iSel)
        {
            //初始化列表
            sqlConn.Open();

            sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品编号, 商品表.商品名称, 商品表.助记码, 商品表.最小计量单位, 商品表.进价, 商品表.批发价, 商品表.登录日期, 商品表.商品规格, 商品表.库存上限, 商品表.库存下限, 商品表.合理库存上限, 商品表.合理库存下限, 商品分类表.分类名称, 商品表.分类编号 AS 分类ID ,商品分类表.分类编号 FROM 商品表 LEFT OUTER JOIN 商品分类表 ON 商品表.分类编号 = 商品分类表.ID WHERE (商品表.beactive = 1) ORDER BY 商品表.商品编号";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");

            sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品编号, 商品表.商品名称, 商品表.助记码, 商品表.最小计量单位, 商品表.进价, 商品表.批发价, 商品表.登录日期, 商品表.商品规格, 商品表.库存上限, 商品表.库存下限, 商品表.合理库存上限, 商品表.合理库存下限, 商品分类表.分类名称, 商品表.分类编号 AS 分类ID, 商品分类表.分类编号 FROM 商品表 INNER JOIN 商品分类表 ON 商品表.分类编号 = 商品分类表.ID WHERE (商品表.beactive = 1) AND (商品表.ID = 0) ORDER BY 商品表.商品编号";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");

            //dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
            dvSelect = new DataView(dSet.Tables["商品表"]);
            dataGridViewDJMX.DataSource = dvSelect;
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[14].Visible = false;

            sqlConn.Close();
            toolStripStatusLabelCount.Text = "共有商品" + dataGridViewDJMX.Rows.Count.ToString() + "种";

            if (iSel != 0)
            {
                dataGridViewDJMX.Rows[0].Selected = false;
                int iRow = -1;

                for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == iSel.ToString())
                    {
                        iRow = i;
                        break;
                    }
                }


                if (iRow != -1)
                {
                    dataGridViewDJMX.Rows[iRow].Selected = true;
                    dataGridViewDJMX.FirstDisplayedScrollingRowIndex = iRow;
                }


            }


        }

        private void ToolStripButtonADD_Click(object sender, EventArgs e)
        {
            if (iVersion <= 0)
            {
                if (dataGridViewDJMX.RowCount >= 10)
                {
                    MessageBox.Show("预览版最多可以管理10种商品");
                    return;
                }
            }
            
            dSet.Tables["商品表1"].Clear();
            DataTable dt = dSet.Tables["商品表1"];

            FormSPDAWH_CARD frmSPDAWH_CARD = new FormSPDAWH_CARD();
            frmSPDAWH_CARD.strConn = strConn;
            //frmSPDAWH_CARD.dt = dt;
            frmSPDAWH_CARD.iStyle = 0;


            frmSPDAWH_CARD.ShowDialog();
            initDataView(frmSPDAWH_CARD.iSelect);
        }

        private void toolStripButtonEDIT_Click(object sender, EventArgs e)
        {
            object[] oT = new object[dataGridViewDJMX.ColumnCount];

            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("请选择要修改的商品", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dSet.Tables["商品表1"].Clear();
            DataTable dt = dSet.Tables["商品表1"];

            for (int i = dataGridViewDJMX.SelectedRows.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < oT.Length; j++)
                    oT[j] = dataGridViewDJMX.SelectedRows[i].Cells[j].Value;
                dt.Rows.Add(oT);
            }

            FormSPDAWH_CARD frmSPDAWH_CARD = new FormSPDAWH_CARD();
            frmSPDAWH_CARD.strConn = strConn;
            frmSPDAWH_CARD.dt = dt;
            frmSPDAWH_CARD.iStyle = 1;

            frmSPDAWH_CARD.ShowDialog();
            initDataView(frmSPDAWH_CARD.iSelect);
        }

        private void toolStripButtonDEL_Click(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("请选择要删除的商品", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (MessageBox.Show("是否删除所选内容？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            int i;
            bool bDELALL = true;
            System.Data.SqlClient.SqlTransaction sqlta;

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                for (i = 0; i < dataGridViewDJMX.SelectedRows.Count; i++)
                {
                    if (dataGridViewDJMX.SelectedRows[i].IsNewRow)
                        continue;


                    //使用状态
                    sqlComm.CommandText = "SELECT DISTINCT 商品表.商品名称 FROM 单据明细汇总视图 INNER JOIN 商品表 ON 单据明细汇总视图.商品ID = 商品表.ID WHERE (单据明细汇总视图.BeActive = 1) AND (单据明细汇总视图.商品ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        bDELALL = false;
                        sqldr.Close();
                        continue;
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "UPDATE 商品表 SET beactive = 0 WHERE (ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();


                    sqlComm.CommandText = "UPDATE 库存表 SET BeActive = 0 WHERE (商品ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
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

            if(bDELALL)
                MessageBox.Show("删除完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("所选商品中已有单据保存，不可删除，其余商品已删除", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            initDataView(0);
        }

        private void dataGridViewDJMX_DoubleClick(object sender, EventArgs e)
        {
            toolStripButtonEDIT_Click(null, null);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");

            string strT = "商品档案维护;当前日期：" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");

            string strT = "商品档案维护;当前日期：" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            dvSelect.RowFilter = "";
            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
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
            if (keyData == Keys.F8)
            {
                btnLocation_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (textBoxMC.Text.Trim() == "")
                return;

            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
            if (radioButtonAll.Checked)
            {
                dvSelect.RowFilter = "商品名称 LIKE '%" + textBoxMC.Text.Trim() + "%'";
            }
            if (radioButtonF.Checked)
            {
                dvSelect.RowFilter = "商品名称 LIKE '" + textBoxMC.Text.Trim() + "%'";
            }
            if (radioButtonE.Checked)
            {
                dvSelect.RowFilter = "商品名称 LIKE '%" + textBoxMC.Text.Trim() + "'";
            }
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            if (textBoxMC.Text.Trim() == "")
                return;

            int iRow = -1;
            string sTemp = "";

            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (radioButtonAll.Checked)  //全匹配
                {
                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString();
                    if (sTemp.IndexOf(textBoxMC.Text.Trim()) != -1)
                    {
                        iRow = i;
                        break;
                    }
                }

                if (radioButtonF.Checked) //前匹配
                {
                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString();
                    if (sTemp.IndexOf(textBoxMC.Text.Trim()) == 0)
                    {
                        iRow = i;
                        break;
                    }
                }

                if (radioButtonE.Checked) //后匹配
                {
                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString().Trim();
                    if (sTemp.Length < textBoxMC.Text.Trim().Length)
                        break;

                    if (sTemp.LastIndexOf(textBoxMC.Text.Trim()) == sTemp.Length - textBoxMC.Text.Trim().Length)
                    {
                        iRow = i;
                        break;
                    }
                }


            }


            if (iRow != -1)
            {
                //dataGridViewDWLB.Rows[iRow].Selected = false;
                dataGridViewDJMX.Rows[iRow].Selected = true;
                dataGridViewDJMX.FirstDisplayedScrollingRowIndex = iRow;
            }
            else
            {
                if (dataGridViewDJMX.Rows.Count > 0)
                {
                    dataGridViewDJMX.Rows[0].Selected = true;
                    dataGridViewDJMX.FirstDisplayedScrollingRowIndex = 0;
                }
            }
        }

    }
}