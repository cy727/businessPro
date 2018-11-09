using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormFPZF : Form
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

        private int iFPH = 0;
        private string strFPH = "";

        private ClassGetInformation cGetInformation;

        public FormFPZF()
        {
            InitializeComponent();
        }


        private void FormFPZF_Load(object sender, EventArgs e)
        {
            int i;
            
            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            comboBoxStyle.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (textBoxFPH.Text == "")
            {
                textBoxFPH.Text = strFPH;
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 发票汇总表.ID, 发票汇总表.发票号, 单位表.单位编号, 单位表.单位名称, 发票汇总表.备注, 发票汇总表.发货方式, 发票汇总表.单号, 发票汇总表.发票类型, 发票汇总表.日期, 发票汇总表.原开票金额, 发票汇总表.发票总额 FROM 发票汇总表 INNER JOIN 单位表 ON 发票汇总表.单位ID = 单位表.ID WHERE (发票汇总表.BeActive = 1) AND (发票汇总表.发票号 = N'" + textBoxFPH.Text + "')";
            if (dSet.Tables.Contains("发票表")) dSet.Tables.Remove("发票表");
            sqlDA.Fill(dSet, "发票表");

            if (dSet.Tables["发票表"].Rows.Count < 1) //没有发票
            {
                textBoxFPH.Text = strFPH;
                sqlConn.Close();
                return;
            }


            if (dSet.Tables["发票表"].Rows.Count == 1) //只有一个发票
            {
                iFPH = Int32.Parse(dSet.Tables["发票表"].Rows[0][0].ToString());
            }
            else //多个发票
            {
                sqlComm.CommandText = "SELECT 发票汇总表.ID, 发票汇总表.单位ID, 发票汇总表.发票号, 单位表.单位编号, 单位表.单位名称, 发票汇总表.日期, 发票汇总表.发票总额, 发票汇总表.备注, 发票汇总表.操作员ID, 职员表.职员姓名, 发票汇总表.操作员ID AS 操作员 FROM 发票汇总表 INNER JOIN 单位表 ON 发票汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 发票汇总表.操作员ID = 职员表.ID WHERE (发票汇总表.BeActive = 1) AND (发票汇总表.发票号 = N'" + textBoxFPH.Text + "')";
                FormSelectBill frmSelectBill = new FormSelectBill();
                frmSelectBill.strConn = strConn;
                frmSelectBill.strSelectText = sqlComm.CommandText;
                frmSelectBill.bShowDW = true;

                frmSelectBill.ShowDialog();

                if (frmSelectBill.iBillNumber == 0)
                {
                    sqlConn.Close();
                    return;
                }
                else
                {
                    iFPH = frmSelectBill.iBillNumber;
                }
            }

            sqlComm.CommandText = "SELECT 发票汇总表.ID, 发票汇总表.发票号, 单位表.单位编号, 单位表.单位名称, 发票汇总表.备注, 发票汇总表.发货方式, 发票汇总表.单号, 发票汇总表.发票类型, 发票汇总表.日期, 发票汇总表.原开票金额, 发票汇总表.发票总额 FROM 发票汇总表 INNER JOIN 单位表 ON 发票汇总表.单位ID = 单位表.ID WHERE (发票汇总表.BeActive = 1) AND (发票汇总表.ID = " + iFPH.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();

            if (!sqldr.HasRows)
            {
                textBoxFPH.Text = strFPH;
                sqldr.Close();
                sqlConn.Close();
                return;
            }

            while (sqldr.Read())
            {
                iFPH = Convert.ToInt32(sqldr.GetValue(0).ToString());
                strFPH = sqldr.GetValue(1).ToString();
                textBoxDWBH.Text = sqldr.GetValue(2).ToString();
                textBoxDWMC.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                comboBoxFHFS.Text = sqldr.GetValue(5).ToString();
                textBoxDH.Text=  sqldr.GetValue(6).ToString();
                comboBoxStyle.SelectedIndex = Convert.ToInt32(sqldr.GetValue(7).ToString());
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(8).ToString()).ToString("yyyy年M月dd日");
                labelJEHJ.Text = sqldr.GetValue(9).ToString();
                labelSJJE.Text = sqldr.GetValue(10).ToString();
                labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            }
            sqldr.Close();

            switch (comboBoxStyle.SelectedIndex)
            {
                case 0:
                    //初始化明细列表
                    sqlComm.CommandText = "SELECT 单据ID, 冲抵ID, 单据编号, 冲抵编号, 原开票总额, 发票总额, 发货方式, 单号, 备注1, 备注2, ID FROM 发票明细表 WHERE  (发票明细表.发票ID = " + iFPH.ToString() + ")";

                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                    sqlDA.Fill(dSet, "单据明细表");
                    dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[1].Visible = false;
                    dataGridViewDJMX.Columns[10].Visible = false;

                    dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                    break;
                case 1:
                    sqlComm.CommandText = "SELECT 单据ID, 冲抵ID, 单据编号, 冲抵编号, 原开票总额, 发票总额, 发货方式, 单号, 备注1, 备注2, ID FROM 发票明细表 WHERE  (发票明细表.发票ID = " + iFPH.ToString() + ")";

                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                    sqlDA.Fill(dSet, "单据明细表");
                    dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[1].Visible = false;
                    dataGridViewDJMX.Columns[10].Visible = false;

                    dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    break;

            }

            sqlConn.Close();
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            string sTemp;

            if (iFPH == 0)
            {
                MessageBox.Show("请选择要作废的发票", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("是否作废选定的发票？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;


            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                //表单汇总
                sqlComm.CommandText = "UPDATE 发票汇总表 SET BeActive = 0 WHERE (ID = "+iFPH.ToString()+")";
                sqlComm.ExecuteNonQuery();

                //明细
                for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                   sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString().Substring(0, 3);
                    switch (comboBoxStyle.SelectedIndex)
                    {
                        case 0:
                            switch (sTemp)
                            {
                                case "ADH":

                                    sqlComm.CommandText = "UPDATE 进货入库汇总表 SET 发票号 = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE 购进商品制单表 SET 发票号 = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[1].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                                case "ATH":

                                    sqlComm.CommandText = "UPDATE 进货退出汇总表 SET 发票号 = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                                case "ATB":

                                    sqlComm.CommandText = "UPDATE 购进退补差价汇总表 SET 发票号 = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                                default:
                                    break;
                            }

                            break;

                        case 1:
                            switch (sTemp)
                            {
                                case "BKP":

                                    sqlComm.CommandText = "UPDATE 销售商品制单表 SET 发票号 = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                                case "BTH":

                                    sqlComm.CommandText = "UPDATE 销售退出汇总表 SET 发票号 = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                                case "BTB":

                                    sqlComm.CommandText = "UPDATE 销售退补差价汇总表 SET 发票号 = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                                default:
                                    break;
                            }
                            break;

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



            MessageBox.Show("发票成功废除", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            iFPH = 0;
            strFPH = "";
            textBoxDWBH.Text = "";
            textBoxDWMC.Text = "";
            textBoxBZ.Text = "";
            comboBoxFHFS.Text = "";
            textBoxDH.Text = "";
            comboBoxStyle.SelectedIndex = 0;
            labelZDRQ.Text = "";
            labelJEHJ.Text = "0";
            labelSJJE.Text = "0";
            labelDX.Text = "零";
            if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");



        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "发票查询;日期：" + labelZDRQ.Text + ";单位名称：" + textBoxDWMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "发票查询;日期：" + labelZDRQ.Text + ";单位名称：" + textBoxDWMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}