using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormTMHTCX : Form
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

        private bool isSaved = false;
        private ClassGetInformation cGetInformation;

        public FormTMHTCX()
        {
            InitializeComponent();
        }

        private void FormTMHTCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            comboBoxLX.Items.Add("购进");
            comboBoxLX.Items.Add("销售");
            //comboBoxLX.Items.Add("借物");

            comboBoxLX.Text = "购进";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTM_Click(object sender, EventArgs e)
        {
            if (textBoxDJBH.Text.Trim() == "")  //全选
            {
                switch (comboBoxLX.Text)
                {
                    case "购进":
                        if (cGetInformation.getBillInformation(51, "") == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;
                    case "销售":
                        if (cGetInformation.getBillInformation(53, "") == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;
                    default:
                        return;

                }
            }
            else
            {

                strDJBH = textBoxDJBH.Text.ToUpper();
                sDJClass = strDJBH.Substring(0, 2);

                switch (strDJBH.Substring(0, 2))
                {
                    case "CG"://购进
                        comboBoxLX.Text = "购进";
                        if (cGetInformation.getBillInformation(511, strDJBH) == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;
                    case "XS"://销售
                        comboBoxLX.Text = "退回";
                        if (cGetInformation.getBillInformation(513, strDJBH) == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;

                    default:
                        MessageBox.Show("没有找到该单据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;


                }

            }

            getBillInfoamtion();
        }

        private void getBillInfoamtion()
        {
            if (textBoxDJBH.Text.Trim() == "")
                return;

            strDJBH = textBoxDJBH.Text.Trim().ToUpper();
            labelWARN.Text = strDJBH;
            sDJClass = strDJBH.Substring(0, 2);

            sqlConn.Open();
            switch (strDJBH.Substring(0, 2))
            {
                case "CG"://购进
                    sqlComm.CommandText = "SELECT 商品条码表.ID, 商品表.商品名称, 商品条码表.条码 FROM 商品表 INNER JOIN 商品条码表 ON 商品表.ID = 商品条码表.商品ID LEFT OUTER JOIN 进货入库汇总表 LEFT OUTER JOIN 采购合同表 RIGHT OUTER JOIN 购进商品制单表 ON 采购合同表.ID = 购进商品制单表.合同ID ON 进货入库汇总表.购进ID = 购进商品制单表.ID ON 商品条码表.单据编号 = 进货入库汇总表.单据编号 WHERE (采购合同表.合同编号 = N'" + strDJBH + "') ORDER BY 商品表.商品名称";
                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                    sqlDA.Fill(dSet, "单据明细表");
                    dataGridViewTM.DataSource = dSet.Tables["单据明细表"];

                    dataGridViewTM.Columns[0].Visible = false;
                    break;

                case "XS"://销售
                    sqlComm.CommandText = "SELECT 商品条码表.ID, 商品表.商品名称, 商品表.商品规格, 商品条码表.条码 FROM 商品表 INNER JOIN 商品条码表 ON 商品表.ID = 商品条码表.商品ID INNER JOIN 销售商品制单表 ON 商品条码表.单据编号 = 销售商品制单表.单据编号 LEFT OUTER JOIN 销售合同表 ON 销售商品制单表.合同ID = 销售合同表.ID WHERE (销售合同表.合同编号 = N'" + strDJBH + "') AND (销售商品制单表.BeActive = 1) ORDER BY 商品表.商品名称";
                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                    sqlDA.Fill(dSet, "单据明细表");
                    dataGridViewTM.DataSource = dSet.Tables["单据明细表"];

                    dataGridViewTM.Columns[0].Visible = false;
                    break;

                default:
                    sDJClass = "";
                    intDJID = 0;
                    textBoxDJBH.Text = "";
                    labelWARN.Text = "";
                    break;

            }
            sqlConn.Close();
            

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

            getBillInfoamtion();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品条码记录(合同编码：" + textBoxDJBH.Text.ToUpper() + ");　";
            PrintDGV.Print_DataGridView(dataGridViewTM, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品条码记录(合同编码：" + textBoxDJBH.Text.ToUpper() + ");　";
            PrintDGV.Print_DataGridView(dataGridViewTM, strT, false, intUserLimit);
        }

        private void textBoxDJBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {

                textBoxDJBH.Text = textBoxDJBH.Text.Trim().ToUpper();
                try
                {
                    int.Parse(textBoxDJBH.Text.Substring(0, 1));
                    switch (comboBoxLX.SelectedIndex)
                    {
                        case 0:
                            textBoxDJBH.Text = "CG" + textBoxDJBH.Text;
                            break;
                        case 1:
                            textBoxDJBH.Text = "XS" + textBoxDJBH.Text;
                            break;

                    }
                }
                catch
                {
                }
                btnTM_Click(null, null);
            }
        }

        private void btnDJCX_Click(object sender, EventArgs e)
        {
            if (textBoxDJBH1.Text.Trim() == "" || textBoxDJBH1.Text.Trim().Length < 13)
            {
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品条码表.ID, 商品表.商品名称, 商品表.商品规格, 商品条码表.条码 FROM 商品条码表 INNER JOIN 商品表 ON 商品条码表.商品ID = 商品表.ID WHERE (商品条码表.单据编号 = N'" + textBoxDJBH1.Text.Trim() + "') ORDER BY 商品表.商品名称";
            if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
            sqlDA.Fill(dSet, "单据明细表");
            dataGridViewTM.DataSource = dSet.Tables["单据明细表"];
            sqlConn.Close();

            dataGridViewTM.Columns[0].Visible = false;
            strDJBH = textBoxDJBH1.Text.Trim().ToUpper();
            labelWARN.Text = strDJBH;
        }

        private void textBoxDJBH1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                textBoxDJBH1.Text = textBoxDJBH1.Text.Trim().ToUpper();


                btnDJCX_Click(null, null);
            }
        }
    }
}
