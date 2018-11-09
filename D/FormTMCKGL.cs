using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormTMCKGL : Form
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

        
        public FormTMCKGL()
        {
            InitializeComponent();
        }

        private void FormTMCKGL_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            comboBoxLX.Items.Add("销售");
            comboBoxLX.Items.Add("退回");
            comboBoxLX.Items.Add("盘点");
            comboBoxLX.Items.Add("报损");
            comboBoxLX.Items.Add("借物");

            comboBoxLX.Text = "销售";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {

            if (textBoxDJBH.Text.Trim() == "")  //全选
            {
                switch (comboBoxLX.Text)
                {
                    case "销售":
                        if (!checkBoxW.Checked)
                        {
                            if (cGetInformation.getBillInformation(211, textBoxDJBH.Text.Trim()) == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        else
                        {
                            if (cGetInformation.getBillInformation(80211, "") == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }

                        break;
                    case "退回":
                        if (!checkBoxW.Checked)
                        {
                            if (cGetInformation.getBillInformation(661, textBoxDJBH.Text.Trim()) == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        else
                        {
                            if (cGetInformation.getBillInformation(80066, "") == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        break;
                    case "盘点":
                        if (!checkBoxW.Checked)
                        {
                            if (cGetInformation.getBillInformation(631, textBoxDJBH.Text.Trim()) == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        else
                        {
                            if (cGetInformation.getBillInformation(80063, "") == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        break;
                    case "报损":
                        if (!checkBoxW.Checked)
                        {
                            if (cGetInformation.getBillInformation(641, textBoxDJBH.Text.Trim()) == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        else
                        {
                            if (cGetInformation.getBillInformation(80064, "") == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        break;
                    case "借物":
                        if (!checkBoxW.Checked)
                        {
                            if (cGetInformation.getBillInformation(31, textBoxDJBH.Text.Trim()) == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        else
                        {
                            if (cGetInformation.getBillInformation(80003, "") == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        break;
                    default:
                        return;

                }
            }
            else
            {
                if (textBoxDJBH.Text.Length < 3)
                    return;

                strDJBH = textBoxDJBH.Text.ToUpper();
                sDJClass = strDJBH.Substring(0, 3);

                switch (strDJBH.Substring(0, 3))
                {
                    case "BKP"://销售
                        comboBoxLX.Text = "销售";
                        if (cGetInformation.getBillInformation(20, strDJBH) == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;
                    case "BTH"://销售退回
                        comboBoxLX.Text = "退回";
                        if (cGetInformation.getBillInformation(661, strDJBH) == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;
                    case "CPD": //盘点
                        comboBoxLX.Text = "盘点";
                        if (cGetInformation.getBillInformation(631, strDJBH) == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;
                    case "CBS": //报损
                        comboBoxLX.Text = "报损";
                        if (cGetInformation.getBillInformation(641, strDJBH) == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;
                    case "CCK": //借物
                        comboBoxLX.Text = "借物";
                        if (cGetInformation.getBillInformation(30, strDJBH) == 0)
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
            btnTMDQ_Click(null,null);
        }

        private void getBillInfoamtion()
        {
            if (textBoxDJBH.Text.Trim() == "")
                return;

            strDJBH = textBoxDJBH.Text.Trim().ToUpper();
            sDJClass = strDJBH.Substring(0, 3);

            sqlConn.Open();
            dataGridViewDJMX.SelectionChanged -= dataGridViewDJMX_SelectionChanged;
            switch (strDJBH.Substring(0, 3))
            {
                case "BKP"://销售
                    sqlComm.CommandText = "SELECT 销售商品制单表.单据编号, 单位表.单位名称, 销售商品制单表.日期, 职员表.职员姓名, 销售商品制单表.ID, 销售合同表.合同编号 FROM 销售合同表 RIGHT OUTER JOIN 职员表 INNER JOIN 单位表 INNER JOIN 销售商品制单表 ON 单位表.ID = 销售商品制单表.单位ID ON 职员表.ID = 销售商品制单表.业务员ID ON 销售合同表.ID = 销售商品制单表.合同ID WHERE (销售商品制单表.BeActive = 1) AND (销售商品制单表.单据编号 = N'" + strDJBH + "')";

                    //sqlComm.CommandText = "SELECT 销售出库汇总表.单据编号, 单位表.单位名称, 销售出库汇总表.日期, 职员表.职员姓名, 销售出库汇总表.ID, 销售合同表.合同编号 FROM 销售合同表 INNER JOIN 销售商品制单表 ON 销售合同表.ID = 销售商品制单表.合同ID RIGHT OUTER JOIN 销售出库汇总表 INNER JOIN 单位表 ON 销售出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售出库汇总表.业务员ID = 职员表.ID ON 销售商品制单表.ID = 销售出库汇总表.销售ID WHERE (销售出库汇总表.BeActive = 1) AND (销售出库汇总表.单据编号 = N'" + strDJBH + "')";
                    sqldr = sqlComm.ExecuteReader();

                    if (!sqldr.HasRows)
                    {
                        sqldr.Close();
                        MessageBox.Show("没有找到该单据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        strDJBH = "";
                        break;
                    }

                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelDWMC.Text = sqldr.GetValue(1).ToString();
                        labelRQ.Text = Convert.ToDateTime(sqldr.GetValue(2).ToString()).ToString("yyyy年M月dd日");
                        labelYWY.Text = sqldr.GetValue(3).ToString();
                        intDJID = Convert.ToInt32(sqldr.GetValue(4).ToString());
                        labelHTBH.Text = sqldr.GetValue(5).ToString();
                        break;
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT 商品表.商品名称, 库房表.库房名称, 销售商品制单明细表.数量, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID,销售商品制单明细表.ID, 商品表.商品规格 FROM 销售商品制单明细表 INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID WHERE (销售商品制单明细表.表单ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                    sqlDA.Fill(dSet, "单据明细表");
                    dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

                    dataGridViewDJMX.Columns[3].Visible = false;
                    dataGridViewDJMX.Columns[4].Visible = false;
                    dataGridViewDJMX.Columns[5].Visible = false;

                    dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";
                    break;

                case "ATH"://进货推出
                    sqlComm.CommandText = "SELECT 进货退出汇总表.单据编号, 单位表.单位名称, 进货退出汇总表.日期,职员表.职员姓名,进货退出汇总表.ID FROM 进货退出汇总表 INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 进货退出汇总表.业务员ID = 职员表.ID WHERE (进货退出汇总表.单据编号 = N'" + strDJBH + "') AND (进货退出汇总表.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (!sqldr.HasRows)
                    {
                        sqldr.Close();
                        MessageBox.Show("没有找到该单据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        strDJBH = "";
                        break;
                    }

                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelDWMC.Text = sqldr.GetValue(1).ToString();
                        labelRQ.Text = Convert.ToDateTime(sqldr.GetValue(2).ToString()).ToString("yyyy年M月dd日");
                        labelYWY.Text = sqldr.GetValue(3).ToString();
                        intDJID = Convert.ToInt32(sqldr.GetValue(4).ToString());
                        labelHTBH.Text = "";
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT 商品表.商品名称, 库房表.库房名称, 进货退出明细表.数量, 进货退出明细表.商品ID, 进货退出明细表.库房ID, 进货退出明细表.ID, 商品表.商品规格 FROM 进货退出明细表 INNER JOIN 商品表 ON 进货退出明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 进货退出明细表.库房ID = 库房表.ID WHERE (进货退出明细表.单据ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                    sqlDA.Fill(dSet, "单据明细表");
                    dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

                    dataGridViewDJMX.Columns[3].Visible = false;
                    dataGridViewDJMX.Columns[4].Visible = false;
                    dataGridViewDJMX.Columns[5].Visible = false;

                    dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";
                    break;

                case "CPD": //盘点
                    sqlComm.CommandText = "SELECT 库存盘点汇总表.单据编号, 库房表.库房名称, 库存盘点汇总表.日期,职员表.职员姓名,库存盘点汇总表.ID FROM 库存盘点汇总表 INNER JOIN 库房表 ON 库存盘点汇总表.库房ID = 库房表.ID INNER JOIN 职员表 ON 库存盘点汇总表.业务员ID = 职员表.ID WHERE (库存盘点汇总表.单据编号 = N'" + strDJBH + "') AND (库存盘点汇总表.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (!sqldr.HasRows)
                    {
                        sqldr.Close();
                        MessageBox.Show("没有找到该单据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        strDJBH = "";
                        break;
                    }

                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        //labelDWMC.Text = sqldr.GetValue(1).ToString();
                        labelDWMC.Text = "";
                        labelRQ.Text = Convert.ToDateTime(sqldr.GetValue(2).ToString()).ToString("yyyy年M月dd日");
                        labelYWY.Text = sqldr.GetValue(3).ToString();
                        intDJID = Convert.ToInt32(sqldr.GetValue(4).ToString());
                        labelHTBH.Text = "";
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT 商品表.商品名称, 库房表.库房名称, 库存盘点明细表.盘损数量, 库存盘点明细表.商品ID, 库存盘点明细表.库房ID, 库存盘点明细表.ID, 商品表.商品规格 FROM 库存盘点明细表 INNER JOIN 商品表 ON 库存盘点明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 库存盘点明细表.库房ID = 库房表.ID WHERE (库存盘点明细表.单据ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                    sqlDA.Fill(dSet, "单据明细表");
                    dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

                    dataGridViewDJMX.Columns[3].Visible = false;
                    dataGridViewDJMX.Columns[4].Visible = false;
                    dataGridViewDJMX.Columns[5].Visible = false;

                    dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";

                    break;

                case "CBS": //报损
                    sqlComm.CommandText = "SELECT 库存报损汇总表.单据编号, 库房表.库房名称, 库存报损汇总表.日期,职员表.职员姓名,库存报损汇总表.ID FROM 库存报损汇总表 INNER JOIN 库房表 ON 库存报损汇总表.库房ID = 库房表.ID INNER JOIN 职员表 ON 库存报损汇总表.业务员ID = 职员表.ID WHERE (库存报损汇总表.单据编号 = N'" + strDJBH + "') AND (库存报损汇总表.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (!sqldr.HasRows)
                    {
                        sqldr.Close();
                        MessageBox.Show("没有找到该单据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        strDJBH = "";
                        break;
                    }

                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        //labelDWMC.Text = sqldr.GetValue(1).ToString();
                        labelDWMC.Text = "";
                        labelRQ.Text = Convert.ToDateTime(sqldr.GetValue(2).ToString()).ToString("yyyy年M月dd日");
                        labelYWY.Text = sqldr.GetValue(3).ToString();
                        intDJID = Convert.ToInt32(sqldr.GetValue(4).ToString());
                        labelHTBH.Text = "";
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT 商品表.商品名称, 库房表.库房名称, 库存报损明细表.报损数量, 库存报损明细表.商品ID, 库存报损汇总表.库房ID,库存报损明细表.ID, 商品表.商品规格 FROM 库存报损明细表 INNER JOIN 商品表 ON 库存报损明细表.商品ID = 商品表.ID INNER JOIN 库存报损汇总表 ON 库存报损明细表.单据ID = 库存报损汇总表.ID INNER JOIN 库房表 ON 库存报损汇总表.库房ID = 库房表.ID WHERE     (库存报损明细表.单据ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                    sqlDA.Fill(dSet, "单据明细表");
                    dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

                    dataGridViewDJMX.Columns[3].Visible = false;
                    dataGridViewDJMX.Columns[4].Visible = false;
                    dataGridViewDJMX.Columns[5].Visible = false;

                    dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";

                    break;

                case "CCK": //借物
                    sqlComm.CommandText = "SELECT 借物出库汇总表.单据编号, 单位表.单位名称, 借物出库汇总表.日期,职员表.职员姓名,借物出库汇总表.ID FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID WHERE (借物出库汇总表.单据编号 = N'" + strDJBH + "') AND (借物出库汇总表.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (!sqldr.HasRows)
                    {
                        sqldr.Close();
                        MessageBox.Show("没有找到该单据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        strDJBH = "";
                        break;
                    }

                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelDWMC.Text = sqldr.GetValue(1).ToString();
                        labelRQ.Text = Convert.ToDateTime(sqldr.GetValue(2).ToString()).ToString("yyyy年M月dd日");
                        labelYWY.Text = sqldr.GetValue(3).ToString();
                        intDJID = Convert.ToInt32(sqldr.GetValue(4).ToString());
                        labelHTBH.Text = "";
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT 商品表.商品名称, 库房表.库房名称, 借物出库明细表.数量, 借物出库明细表.商品ID, 借物出库明细表.库房ID, 借物出库明细表.ID, 商品表.商品规格 FROM 借物出库明细表 INNER JOIN 商品表 ON 借物出库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID WHERE (借物出库明细表.表单ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
                    sqlDA.Fill(dSet, "单据明细表");
                    dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

                    dataGridViewDJMX.Columns[3].Visible = false;
                    dataGridViewDJMX.Columns[4].Visible = false;
                    dataGridViewDJMX.Columns[5].Visible = false;

                    dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";

                    break;

                default:
                    sDJClass = "";
                    intDJID = 0;
                    textBoxDJBH.Text = "";
                    break;

            }
            sqlConn.Close();
            dataGridViewDJMX.SelectionChanged += dataGridViewDJMX_SelectionChanged;
            dataGridViewDJMX_SelectionChanged(null, null);
 

        }

        private void inittoolStripStatusLabelTS()
        {
            if (labelDJBH.Text == "")
            {
                toolStripStatusLabelTS.Text = "";
                return;
            }

            int iSum = 0;
            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                try
                {
                    iSum += (int)decimal.Parse(dataGridViewDJMX.Rows[i].Cells[2].Value.ToString());
                }
                catch (Exception e)
                {
                }
            }

            int iTM = 0;
            sqlConn.Open();
            sqlComm.CommandText = "SELECT COUNT(*) FROM 商品条码表 WHERE (单据编号 = N'" + labelDJBH.Text + "') AND (出入库标记 = 1)";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                iTM = int.Parse(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            sqlConn.Close();

            toolStripStatusLabelTS.Text = "数量：" + iSum.ToString() + " 已扫录条码数量：" + iTM.ToString();


        }

        private void inittoolStripStatusLabelTS1()
        {
            if (dataGridViewDJMX.Rows.Count < 1 || dataGridViewDJMX.SelectedRows.Count < 1)
            {
                toolStripStatusLabelTS1.Text = "";
                return;
            }

            int iSum = (int)decimal.Parse(dataGridViewDJMX.SelectedRows[0].Cells[2].Value.ToString());
            int iTM = dataGridViewTM.RowCount;

            if (iSum > iTM)
                toolStripStatusLabelTS1.ForeColor = Color.Red;
            else
                toolStripStatusLabelTS1.ForeColor = Color.Black;
            toolStripStatusLabelTS1.Text = "数量：" + iSum.ToString() + " 已扫录条码数量：" + iTM.ToString();
        }

        private void initTMView(int iSPID, int iKFID, int MXID)
        {
            if (strDJBH == "")
            {
                if (dSet.Tables.Contains("条码表")) dSet.Tables.Remove("条码表");
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ID, 条码, 日期 FROM 商品条码表 WHERE (单据编号 = N'" + strDJBH + "') AND (单据明细ID=" + MXID.ToString() + ") ";

            if (iSPID != 0) //商品过滤
                sqlComm.CommandText += " AND (商品ID = " + iSPID.ToString() + ")AND (库房ID = " + iKFID.ToString() + ")";
            sqlComm.CommandText += " ORDER BY 日期 DESC";

            if (dSet.Tables.Contains("条码表")) dSet.Tables.Remove("条码表");
            sqlDA.Fill(dSet, "条码表");
            dataGridViewTM.DataSource = dSet.Tables["条码表"];
            sqlConn.Close();

            inittoolStripStatusLabelTS1();
            inittoolStripStatusLabelTS();
        }

        private void btnTMDQ_Click(object sender, EventArgs e)
        {
            this.textBoxTM.Focus();
            this.textBoxTM.SelectAll();
        }

        private void dataGridViewDJMX_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                labelSPMC.Text = "";
                return;
            }

            labelSPMC.Text = dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString();

            if (dataGridViewDJMX.SelectedRows[0].Cells[3].Value.ToString() == "")
                dataGridViewDJMX.SelectedRows[0].Cells[3].Value = 0;
            if (dataGridViewDJMX.SelectedRows[0].Cells[4].Value.ToString() == "")
                dataGridViewDJMX.SelectedRows[0].Cells[4].Value = 0;

            initTMView(Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[3].Value), Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[4].Value),Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[5].Value));
            dataGridViewTM.Columns[0].Visible = false;

            textBoxTM.Text = "";
            btnTMDQ_Click(null,null);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            initTMView(0, 0, 0);
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

            dataGridViewDJMX_SelectionChanged(null, null);
      

        }

        private void textBoxTM_KeyPress(object sender, KeyPressEventArgs e)
        {
            string sZY = "";
            int iCount = 0;
            int iSP;

            if (e.KeyChar == (char)Keys.Return)
            {
                if (textBoxTM.Text == "")
                {
                    labelWARN.Text = "";
                    return;
                }

                if (dataGridViewDJMX.SelectedRows.Count < 1)
                {
                    MessageBox.Show("请选择输入条码对应的商品！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxTM.SelectAll();
                    return;
                }

                string strDT;
                cGetInformation.getSystemDateTime();
                strDT = cGetInformation.strSYSDATATIME;

                System.Data.SqlClient.SqlTransaction sqlta;
                sqlConn.Open();
                sqlta = sqlConn.BeginTransaction();
                sqlComm.Transaction = sqlta;
                try
                {

                    //是否有入库记录
                    sqlComm.CommandText = "SELECT ID, 单据编号, 摘要, 日期, 商品ID FROM 商品条码表 WHERE (出入库标记 = 0) AND (条码 = N'" + textBoxTM.Text.ToUpper() + "')";
                    sqldr = sqlComm.ExecuteReader();

                    if (!sqldr.HasRows) //没有入库记录
                    {
                        if (MessageBox.Show("没有该条码入库记录,是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                        {
                            labelWARN.Text = "没有该条码入库记录！";
                            textBoxTM.SelectAll();
                            sqldr.Close();
                            sqlConn.Close();
                            return;
                        }
                    }
                    iSP=0;
                    while (sqldr.Read())
                    {
                        iSP=int.Parse(sqldr.GetValue(4).ToString());
                        if (iSP != int.Parse(dataGridViewDJMX.SelectedRows[0].Cells[3].Value.ToString())) //商品类型不对
                        {
                            labelWARN.Text = "该条码与入库商品无法对应！";
                            textBoxTM.SelectAll();
                            sqldr.Close();
                            sqlConn.Close();
                            return;
                        }
                        break;
                    }

                    sqldr.Close();

                    //是否有出库记录
                    sqlComm.CommandText = "SELECT ID, 单据编号, 摘要, 日期, 出入库标记 FROM 商品条码表 WHERE (条码 = N'" + textBoxTM.Text.ToUpper() + "') ORDER BY 日期 DESC, ID DESC";

                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //有出库记录
                    {
                        sqldr.Read();
                        if (sqldr.GetValue(4).ToString() == "True")
                        {
                            labelWARN.Text = "该条码最终为出库记录，单据编号：" + sqldr.GetValue(1).ToString();
                            textBoxTM.SelectAll();
                            sqldr.Close();
                            sqlConn.Close();
                            return;
                        }
                        /*
                        if (sDJClass != "ATH") //非退货商品
                        {
                            sqldr.Read();
                            labelWARN.Text = "已有该条码出入库记录，单据编号：" + sqldr.GetValue(1).ToString();
                            textBoxTM.SelectAll();
                            sqldr.Close();
                            sqlConn.Close();
                            return;
                        }
                        */
                    }
                    sqldr.Close();

                    //数量校验
                    sqlComm.CommandText = "SELECT COUNT(*) AS 数量 FROM 商品条码表 WHERE (单据编号 = N'" + strDJBH + "') AND (单据明细ID=" + dataGridViewDJMX.SelectedRows[0].Cells[5].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.SelectedRows[0].Cells[3].Value.ToString() + ") AND (出入库标记 = 1)";
                    sqldr = sqlComm.ExecuteReader();
                    
                    iCount = 0;
                    while (sqldr.Read())
                    {
                        iCount = Convert.ToInt32(sqldr.GetValue(0).ToString());
                    }
                    sqldr.Close();

                    //int iTemp = Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[2].Value);
                    if (iCount >= Math.Abs(Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[2].Value)))
                    {
                        labelWARN.Text = "单据商品条码数量已满";
                        textBoxTM.SelectAll();
                        sqldr.Close();
                        sqlConn.Close();
                        return;
                    }


                    //没有入库记录
                    switch (sDJClass)
                    {
                        case "BKP"://销售出库
                            sZY = "销售出库制单";
                            break;
                        case "ATH"://销售退回
                            sZY = "购进退回制单";
                            break;
                        case "CPD": //盘点
                            sZY = "商品盘点";
                            break;
                        case "CBS": //报损
                            sZY = "商品报损";
                            break;
                        case "CCK": //借物
                            sZY = "借物";
                            break;
                    }

                    sqlComm.CommandText = "INSERT INTO 商品条码表 (条码, 商品ID, 库房ID, 单据编号, 摘要, 日期, 出入库标记, 操作员ID, 单据明细ID) VALUES (N'" + textBoxTM.Text + "', " + dataGridViewDJMX.SelectedRows[0].Cells[3].Value.ToString() + ", " + dataGridViewDJMX.SelectedRows[0].Cells[4].Value.ToString() + ", N'" + strDJBH + "', N'" + sZY + "', '" + strDT + "', 1, " + intUserID.ToString() + "," + dataGridViewDJMX.SelectedRows[0].Cells[5].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();


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


                labelWARN.Text = "条码录入成功";
                initTMView(Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[3].Value.ToString()), Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[4].Value.ToString()), Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[5].Value));

                textBoxTM.SelectAll();
            }

        }

        private void textBoxTM_Enter(object sender, EventArgs e)
        {
            textBoxTM.SelectAll();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            if (dSet.Tables.Contains("条码打印表")) dSet.Tables.Remove("条码打印表");
            dSet.Tables.Add("条码打印表");
            dSet.Tables["条码打印表"].Columns.Add("商品条码", System.Type.GetType("System.String"));
            string[] strDRow ={ "" };

            sqlConn.Open();
            for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                strDRow[0] = "商品:" + ":" + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString();
                dSet.Tables["条码打印表"].Rows.Add(strDRow);

                sqlComm.CommandText = "SELECT 条码 FROM 商品条码表 WHERE (商品ID = " + dataGridViewDJMX.Rows[i].Cells[3].Value.ToString() + ") AND (库房ID = " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ") AND (单据编号 = N'" + strDJBH + "')";
                sqldr = sqlComm.ExecuteReader();
                j = 1;
                while (sqldr.Read())
                {
                    strDRow[0] = "　" + j.ToString() + ":" + sqldr.GetValue(0).ToString();
                    dSet.Tables["条码打印表"].Rows.Add(strDRow);
                    j++;
                }
                sqldr.Close();
            }
            sqlConn.Close();

            dataGridViewPR.DataSource = dSet.Tables["条码打印表"];
            string strT = "商品条码表;;";
            PrintDGV.Print_DataGridView(dataGridViewPR, strT, true, intUserLimit);


        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            if (dSet.Tables.Contains("条码打印表")) dSet.Tables.Remove("条码打印表");
            dSet.Tables.Add("条码打印表");
            dSet.Tables["条码打印表"].Columns.Add("商品条码", System.Type.GetType("System.String"));
            string[] strDRow ={ "" };

            sqlConn.Open();
            for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                strDRow[0] = "商品:" + ":" + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString();
                dSet.Tables["条码打印表"].Rows.Add(strDRow);

                sqlComm.CommandText = "SELECT 条码 FROM 商品条码表 WHERE (商品ID = " + dataGridViewDJMX.Rows[i].Cells[3].Value.ToString() + ") AND (库房ID = " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ") AND (单据编号 = N'" + strDJBH + "')";
                sqldr = sqlComm.ExecuteReader();
                j = 1;
                while (sqldr.Read())
                {
                    strDRow[0] = "　" + j.ToString() + ":" + sqldr.GetValue(0).ToString();
                    dSet.Tables["条码打印表"].Rows.Add(strDRow);
                    j++;
                }
                sqldr.Close();
            }
            sqlConn.Close();

            dataGridViewPR.DataSource = dSet.Tables["条码打印表"];
            string strT = "商品条码表;;";
            PrintDGV.Print_DataGridView(dataGridViewPR, strT, false, intUserLimit);


        }

        private void textBoxDJBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                textBoxDJBH.Text = textBoxDJBH.Text.Trim().ToUpper();
                try
                {
                    int.Parse(textBoxDJBH.Text.Substring(0,1));
                    switch(comboBoxLX.SelectedIndex)
                    {
                        case 0:
                            textBoxDJBH.Text = "BKP" + textBoxDJBH.Text;
                            break;
                        case 1:
                            textBoxDJBH.Text = "ATH" + textBoxDJBH.Text;
                            break;
                        case 2:
                            textBoxDJBH.Text = "CPD" + textBoxDJBH.Text;
                            break;
                        case 3:
                            textBoxDJBH.Text = "CBS" + textBoxDJBH.Text;
                            break;
                        case 4:
                            textBoxDJBH.Text = "CCK" + textBoxDJBH.Text;
                            break;


                    }
                }
                catch
                {
                }
                
                btnSelect_Click(null, null);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //return EnterToTab(ref   msg, keyData, true);
            int i;

            if (keyData == Keys.Up)
            {
                i = dataGridViewDJMX.SelectedRows[0].Index;

                if (i == 0)
                    i = dataGridViewDJMX.RowCount - 1;
                else
                    i-- ;

                dataGridViewDJMX.Rows[i].Selected = true;
                dataGridViewDJMX.FirstDisplayedScrollingRowIndex = i;
                return true;
            }

            if (keyData == Keys.Down)
            {
                i = dataGridViewDJMX.SelectedRows[0].Index;

                if (i == dataGridViewDJMX.RowCount - 1)
                    i = 0;
                else
                    i++;

                dataGridViewDJMX.Rows[i].Selected = true;
                dataGridViewDJMX.FirstDisplayedScrollingRowIndex = i;
                

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}