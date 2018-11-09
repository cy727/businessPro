using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormYSZKJS : Form
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

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;
        
        
        public FormYSZKJS()
        {
            InitializeComponent();
        }

        private void FormYSZKJS_Load(object sender, EventArgs e)
        {
            int i;

            this.Top= 1;
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

            DataRow drTemp1 = dSet.Tables["部门表"].NewRow();
            drTemp1[0] = 0;
            drTemp1[1] = "全部";
            dSet.Tables["部门表"].Rows.Add(drTemp1);

            comboBoxBM.DataSource = dSet.Tables["部门表"];
            comboBoxBM.DisplayMember = "部门名称";
            comboBoxBM.ValueMember = "ID";
            //comboBoxBM.SelectedValue = intUserBM;;
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;

            //初始化单据列表
            sqlComm.CommandText = "SELECT 结算收款明细表.ID, 账簿表.账簿编号, 账簿表.账簿名称, 结算收款明细表.摘要, 结算收款明细表.冲应付款, 账簿表.扣率, 结算收款明细表.付款金额, 结算收款明细表.支票号, 结算收款明细表.备注, 账簿表.账簿ID, 结算收款定义表.勾兑标记, 结算收款定义表.勾兑纪录 FROM 账簿表 INNER JOIN 结算收款明细表 ON 账簿表.ID = 结算收款明细表.账簿ID CROSS JOIN 结算收款定义表 WHERE (结算收款明细表.ID = 0)";

            if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
            sqlDA.Fill(dSet, "单据明细表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

            dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            DataRow drTemp = dSet.Tables["单据明细表"].NewRow();
            dSet.Tables["单据明细表"].Rows.Add(drTemp);

            dataGridViewDJMX.Columns[0].Visible = false;
            //
            dataGridViewDJMX.Columns[4].ReadOnly = true;
            dataGridViewDJMX.Columns[9].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[6].ReadOnly = true;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;

            dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

            sqlConn.Close();

            //initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            //labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            dateTimePickerZDRQ.Value=Convert.ToDateTime(strDT);
            labelCZY.Text = strUserName;

            comboBoxGD.SelectedIndex = 1;
            comboBoxBM.Text = "销售部";

        }

        private void initDJ()
        {
            int iBM = 0;

            toolStripButtonFP.Visible = true;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 结算收款汇总表.单据编号, 结算收款汇总表.日期, 业务员.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 结算收款汇总表.备注, 单位表.单位编号, 单位表.单位名称, 结算收款汇总表.发票号, 单位表.税号, 单位表.应收账款, 结算收款汇总表.部门ID, 结算收款汇总表.备注2, 结算收款汇总表.BeActive FROM 单位表 INNER JOIN 结算收款汇总表 ON 单位表.ID = 结算收款汇总表.单位ID INNER JOIN 职员表 业务员 ON 结算收款汇总表.业务员ID = 业务员.ID INNER JOIN 职员表 操作员 ON 结算收款汇总表.操作员ID = 操作员.ID WHERE (结算收款汇总表.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                //labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");
                dateTimePickerZDRQ.Value = Convert.ToDateTime(sqldr.GetValue(1).ToString());

                if (sqldr.GetValue(10).ToString() != "")
                {
                    try
                    {
                        iBM = int.Parse(sqldr.GetValue(10).ToString());
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
                textBoxBZ2.Text = sqldr.GetValue(11).ToString();
                textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                textBoxDWMC.Text = sqldr.GetValue(6).ToString();
                textBoxSH.Text = sqldr.GetValue(8).ToString();
                textBoxFPH.Text = sqldr.GetValue(7).ToString();
                textBoxYSYE.Text = sqldr.GetValue(9).ToString();



                this.Text = "应收账款结算单：" + labelDJBH.Text;
            }
            sqldr.Close();

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
            sqlComm.CommandText = "SELECT 结算收款明细表.ID, 账簿表.账簿编号, 账簿表.账簿名称, 结算收款明细表.摘要, 结算收款明细表.冲应付款, 账簿表.扣率, 结算收款明细表.付款金额, 结算收款明细表.支票号, 结算收款明细表.备注, 结算收款明细表.账簿ID, 结算收款定义表.勾兑标记, 结算收款定义表.勾兑纪录 FROM 账簿表 INNER JOIN 结算收款明细表 ON 账簿表.ID = 结算收款明细表.账簿ID CROSS JOIN 结算收款定义表 WHERE (结算收款明细表.单据ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
            sqlDA.Fill(dSet, "单据明细表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[9].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;

            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;
            sqlConn.Close();
        }



        private void comboBoxBM_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            sqlConn.Open();

            //初始化员工列表
            if (comboBoxBM.Text.Trim() != "全部")
                sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员姓名, 职员表.职员编号 FROM 职员表 INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (部门表.部门名称 = N'" + comboBoxBM.Text.Trim() + "') AND (职员表.beactive = 1)";
            else
                sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员姓名, 职员表.职员编号 FROM 职员表 INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (职员表.beactive = 1)";

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

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(2, "") == 0)
            {
                //return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                textBoxSH.Text = cGetInformation.strCompanySH;
                //textBoxYSYE.Text = cGetInformation.dCompanyYSZK.ToString();
                textBoxYSYE.Text = getCompanyPay(iSupplyCompany);
                if (dSet.Tables.Contains("按照单据勾兑"))
                    dSet.Tables.Remove("按照单据勾兑");
                if (dSet.Tables.Contains("按照单据明细勾兑"))
                    dSet.Tables.Remove("按照单据明细勾兑");
                if (dSet.Tables.Contains("按照单据明细数量勾兑"))
                    dSet.Tables.Remove("按照单据明细数量勾兑");
                if (dSet.Tables.Contains("单据明细表")) dSet.Tables["单据明细表"].Clear();
                dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                DataRow drTemp = dSet.Tables["单据明细表"].NewRow();
                dSet.Tables["单据明细表"].Rows.Add(drTemp);
                dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                if (cGetInformation.iBMID != 0)
                    comboBoxBM.SelectedValue = cGetInformation.iBMID;

                comboBoxYWY.Text = cGetInformation.sCompanyYWY;

            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(20, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                    textBoxSH.Text = "";
                    textBoxYSYE.Text = "0.00";

                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxSH.Text = cGetInformation.strCompanySH;
                    //textBoxYSYE.Text = cGetInformation.dCompanyYSZK.ToString();
                    textBoxYSYE.Text = getCompanyPay(iSupplyCompany);

                    if (dSet.Tables.Contains("按照单据勾兑"))
                        dSet.Tables.Remove("按照单据勾兑");
                    if (dSet.Tables.Contains("按照单据明细勾兑"))
                        dSet.Tables.Remove("按照单据明细勾兑");
                    if (dSet.Tables.Contains("按照明细数量勾兑"))
                        dSet.Tables.Remove("按照明细数量勾兑");
                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables["单据明细表"].Clear();
                    dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                    DataRow drTemp = dSet.Tables["单据明细表"].NewRow();
                    dSet.Tables["单据明细表"].Rows.Add(drTemp);
                    dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    if (cGetInformation.iBMID != 0)
                        comboBoxBM.SelectedValue = cGetInformation.iBMID;

                    comboBoxYWY.Text = cGetInformation.sCompanyYWY;

                }
            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(22, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxSH.Text = "";
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                    textBoxYSYE.Text = "0.00";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxSH.Text = cGetInformation.strCompanySH;
                    //textBoxYSYE.Text = cGetInformation.dCompanyYSZK.ToString();
                    textBoxYSYE.Text = getCompanyPay(iSupplyCompany);

                    if (dSet.Tables.Contains("按照单据勾兑"))
                        dSet.Tables.Remove("按照单据勾兑");
                    if (dSet.Tables.Contains("按照单据明细勾兑"))
                        dSet.Tables.Remove("按照单据明细勾兑");
                    if (dSet.Tables.Contains("按照明细数量勾兑"))
                        dSet.Tables.Remove("按照明细数量勾兑");
                    if (dSet.Tables.Contains("单据明细表")) dSet.Tables["单据明细表"].Clear();
                    dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                    DataRow drTemp = dSet.Tables["单据明细表"].NewRow();
                    dSet.Tables["单据明细表"].Rows.Add(drTemp);
                    dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    if (cGetInformation.iBMID != 0)
                        comboBoxBM.SelectedValue = cGetInformation.iBMID;

                    comboBoxYWY.Text = cGetInformation.sCompanyYWY;
                }
            }
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow || dataGridViewDJMX.RowCount - 1 == e.RowIndex)
            {
               // dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
               // DataRow drTemp = dSet.Tables["单据明细表"].NewRow();
               // dSet.Tables["单据明细表"].Rows.Add(drTemp);
               // dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            }
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2) //账簿编号
            {
                if (cGetInformation.getZBInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.iZBNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strZBName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strZBCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.dZBKL;

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[4];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }
        }

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
           //     return;
            if (isSaved)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            switch (e.ColumnIndex)
            {
                case 1: //账簿编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getZBInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].ErrorText = "账簿编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value.ToString() == cGetInformation.iZBNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.iZBNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strZBName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strZBCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.dZBKL;

                        //if (dataGridViewDJMX.RowCount - 1 == e.RowIndex)
                        //{
                        //    DataRow drTemp = dSet.Tables["单据明细表"].NewRow();
                        //    dSet.Tables["单据明细表"].Rows.Add(drTemp);
                        //}

                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;

                case 2: //账簿名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                        break;

                    }
                    if (e.FormattedValue.ToString() == dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value.ToString())
                        break;

                    if (cGetInformation.getCommInformation(11, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].ErrorText = "账簿助记码输入错误";
                    }
                    else
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value.ToString() == cGetInformation.iZBNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.iZBNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strZBName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strZBCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.dZBKL;

                        //if (dataGridViewDJMX.RowCount - 1 == e.RowIndex)
                        //{
                        //    DataRow drTemp = dSet.Tables["单据明细表"].NewRow();
                        //    dSet.Tables["单据明细表"].Rows.Add(drTemp);
                        //}

                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    }
                    break;
                case 4: //冲应付款
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;


                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        detOut = Math.Round(detOut, 2);
                        //if (dataGridViewDJMX.RowCount - 1 == e.RowIndex)
                        //{
                        //    DataRow drTemp = dSet.Tables["单据明细表"].NewRow();
                        //    dSet.Tables["单据明细表"].Rows.Add(drTemp);
                        //}
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].ErrorText = "商品数量价格类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 5:  //扣率
                    double dOut = 0.0;
                    if (e.FormattedValue.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 100.00;
                        break;
                    }
                    if (Double.TryParse(e.FormattedValue.ToString(), out dOut))
                    {
                        if (dOut <= 0 || dOut > 100.0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[5].ErrorText = "商品扣率输入错误，请输入0.01-100.00之间的数字";
                            e.Cancel = true;
                        }
                        else
                        {
                            //if (dataGridViewDJMX.RowCount - 1 == e.RowIndex)
                            //{
                            //    DataRow drTemp = dSet.Tables["单据明细表"].NewRow();
                            //    dSet.Tables["单据明细表"].Rows.Add(drTemp);
                            //}
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].ErrorText = "商品扣率输入错误，请输入0.01-100.00之间的数字";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;



            }
            dataGridViewDJMX.EndEdit();
            countAmount();

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
                                    case 3:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[4];
                                        break;
                                    case 4:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[5];
                                        break;
                                    case 5:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    case 7:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                        break;
                                    case 8:
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


        //return true 正确  false 错误
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

                //库房ID
                if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "0")
                {
                    bCheck = false;
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "请输入账簿编号";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "请输入账簿助记码";
                    continue;
                }


                //冲应付款
                if (dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[4].Value = 0;

                fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);


                //扣率
                if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[5].Value = 100;
                }


                //实计金额
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                dataGridViewDJMX.Rows[i].Cells[6].Value = fTemp * fTemp1 / 100;


                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);

                fCount += 1;

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelJEHJ.Text = fSum.ToString();
            labelSJJE.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;
        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            DataColumn[] dcKey = new DataColumn[1];
            int rowIndex = 0;
            FormSelectGD frmSelectGD = new FormSelectGD();
            frmSelectGD.strConn = strConn;

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("请选择结算单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            toolStripButtonFP_Click_1(null, null);


            if (dataGridViewDJMX.CurrentCell == null)
                dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[0].Cells[4];
            dataGridViewDJMX.BeginEdit(true);
            switch (comboBoxGD.SelectedIndex)
            {
                case 0: //按照单据勾兑

                    if (!dSet.Tables.Contains("按照单据勾兑"))  //初始化单据勾兑列表
                    {
                        sqlConn.Open();
                        sqlComm.CommandText = "SELECT 结算收款定义表.结清, 收款汇总视图.单据编号, 收款汇总视图.日期, 收款汇总视图.价税合计, 收款汇总视图.已付款金额, 结算收款定义表.将支付金额, 收款汇总视图.未付款金额, 收款汇总视图.ID, 结算收款定义表.勾兑标记, CONVERT(decimal, 0) AS 付款额 FROM 结算收款定义表 CROSS JOIN 收款汇总视图 WHERE (收款汇总视图.单位ID =  "+ iSupplyCompany.ToString() +") AND (收款汇总视图.未付款金额 <> 0)";
                        sqlDA.Fill(dSet, "按照单据勾兑");
                        //dcKey[0]=dSet.Tables["按照单据勾兑"].Columns[7];
                        //dSet.Tables["按照单据勾兑"].PrimaryKey = dcKey;

                        sqlConn.Close();
                    }


                    frmSelectGD.iSelectStyle = 1;

                    frmSelectGD.dtSelect = dSet.Tables["按照单据勾兑"];
                    frmSelectGD.ShowDialog();
                    if (frmSelectGD.iSUMSELECT != 0)
                    {
                        if (dataGridViewDJMX.CurrentCell == null)
                            rowIndex = 0;
                        else
                            rowIndex = dataGridViewDJMX.CurrentCell.RowIndex;


                        dataGridViewDJMX.Rows[rowIndex].Cells[4].Value = frmSelectGD.dSUMJE;

                    }

                    break;

                case 1: //按照单据明细勾兑
                    if (!dSet.Tables.Contains("按照单据明细勾兑"))  //初始化单据勾兑列表
                    {
                        sqlConn.Open();
                        sqlComm.CommandText = "SELECT 结算收款定义表.结清, 收款明细视图.单据编号, 收款明细视图.日期,收款明细视图.商品编号, 收款明细视图.商品名称, 商品表.商品规格, 收款明细视图.数量, 收款明细视图.实计金额, 收款明细视图.已付款金额, 结算收款定义表.将支付金额, 收款明细视图.未付款金额, 结算收款定义表.勾兑标记, 收款明细视图.单据ID, 收款明细视图.ID, 收款明细视图.商品ID, 收款明细视图.分类编号, 收款明细视图.库房ID, CONVERT(decimal, 0) AS 付款额, 收款明细视图.备注 FROM 收款明细视图 INNER JOIN 商品表 ON 收款明细视图.商品ID = 商品表.ID CROSS JOIN 结算收款定义表 WHERE (收款明细视图.单位ID = " + iSupplyCompany.ToString() + ") AND (收款明细视图.未付款金额 <> 0)";
                        sqlDA.Fill(dSet, "按照单据明细勾兑");
                        //dcKey[0] = dSet.Tables["按照单据明细勾兑"].Columns[13];
                        //dSet.Tables["按照单据明细勾兑"].PrimaryKey = dcKey;

                        sqlConn.Close();
                    }
                    frmSelectGD.iSelectStyle = 2;

                    frmSelectGD.dtSelect = dSet.Tables["按照单据明细勾兑"];
                    frmSelectGD.ShowDialog();
                    if (frmSelectGD.iSUMSELECT != 0)
                    {
                        if (dataGridViewDJMX.CurrentCell == null)
                            rowIndex = 0;
                        else
                            rowIndex = dataGridViewDJMX.CurrentCell.RowIndex;


                        dataGridViewDJMX.Rows[rowIndex].Cells[4].Value = frmSelectGD.dSUMJE;
                    }
                    break;

                case 2: //按照明细数量勾兑
                    if (!dSet.Tables.Contains("按照明细数量勾兑"))  //初始化单据勾兑列表
                    {
                        sqlConn.Open();
                        sqlComm.CommandText = "SELECT 结算收款定义表.结清, 收款明细视图.单据编号, 收款明细视图.日期, 收款明细视图.商品编号, 收款明细视图.商品名称, 商品表.商品规格, 收款明细视图.数量, 收款明细视图.已付款数量, 结算收款定义表.将付款数量, 收款明细视图.未付款数量, 收款明细视图.实计金额, 收款明细视图.已付款金额, 结算收款定义表.将支付金额, 收款明细视图.未付款金额, 结算收款定义表.勾兑标记, 收款明细视图.单据ID, 收款明细视图.ID, 收款明细视图.商品ID, 收款明细视图.分类编号, 收款明细视图.库房ID, CONVERT(decimal, 0) AS 付款额 FROM 收款明细视图 INNER JOIN 商品表 ON 收款明细视图.商品ID = 商品表.ID CROSS JOIN 结算收款定义表 WHERE (收款明细视图.单位ID = " + iSupplyCompany.ToString() + ") AND (收款明细视图.未付款金额 <> 0)";
                        sqlDA.Fill(dSet, "按照明细数量勾兑");
                        //dcKey[0] = dSet.Tables["按照明细数量勾兑"].Columns[16];
                        //dSet.Tables["按照明细数量勾兑"].PrimaryKey = dcKey;

                        sqlConn.Close();
                    }
                    frmSelectGD.iSelectStyle = 3;

                    frmSelectGD.dtSelect = dSet.Tables["按照明细数量勾兑"];
                    frmSelectGD.ShowDialog();
                    if (frmSelectGD.iSUMSELECT != 0)
                    {
                        if (dataGridViewDJMX.CurrentCell == null)
                            rowIndex = 0;
                        else
                            rowIndex = dataGridViewDJMX.CurrentCell.RowIndex;


                        dataGridViewDJMX.Rows[rowIndex].Cells[4].Value = frmSelectGD.dSUMJE;
                    }
                    break;


                default:
                    MessageBox.Show("请选择勾兑方式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
            }
            dataGridViewDJMX.EndEdit();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i,j;
            string sTemp = "";
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("应收账款结算单已经保存,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("请选择单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("应收账款结算单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("没有选择应收账款结算明细项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if (MessageBox.Show("请检查应收账款结算单内容，是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            //    return;

            saveToolStripButton.Enabled = false;
            string strCount = "", strDateSYS = "", strKey = "BYS";
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

                if (textBoxFPH.Text != "")
                    sqlComm.CommandText = "INSERT INTO 结算收款汇总表 (单据编号, 原单据ID, 单位ID, 税号, 业务员ID, 操作员ID, 日期, 发票号, 开票日期, 备注,     BeActive, 实计金额, 部门ID, 备注2) VALUES (N'" + strCount + "', NULL, " + iSupplyCompany.ToString() + ", N'" + textBoxSH.Text + "', " + comboBoxYWY.SelectedValue.ToString() + ", " + intUserID.ToString() + ", '" + dateTimePickerZDRQ.Value.ToShortDateString() + "', N'" + textBoxFPH.Text + "', '" + dateTimePickerZDRQ.Value.ToShortDateString() + "', N'" + textBoxBZ.Text + "', 1, " + labelSJJE.Text + "," + sBMID + " , N'"+textBoxBZ2.Text+"')";
                else
                    sqlComm.CommandText = "INSERT INTO 结算收款汇总表 (单据编号, 原单据ID, 单位ID, 税号, 业务员ID, 操作员ID, 日期, 发票号, 开票日期, 备注,     BeActive, 实计金额, 部门ID, 备注2) VALUES (N'" + strCount + "', NULL, " + iSupplyCompany.ToString() + ", N'" + textBoxSH.Text + "', " + comboBoxYWY.SelectedValue.ToString() + ", " + intUserID.ToString() + ", '" + dateTimePickerZDRQ.Value.ToShortDateString() + "', N'', NULL, N'" + textBoxBZ.Text + "', 1, " + labelSJJE.Text + "," + sBMID + ", N'" + textBoxBZ2.Text + "')";
                sqlComm.ExecuteNonQuery();

                //取得单据号 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //单位应收账
                sqlComm.CommandText = "SELECT 应收账款 FROM 单位表 WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();

                dKCJE = 0;
                while (sqldr.Read())
                {
                    dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                }
                sqldr.Close();
                dKCJE1 = dKCJE - Convert.ToDecimal(labelSJJE.Text);
                sqlComm.CommandText = "UPDATE 单位表 SET 应收账款 = " + dKCJE1.ToString() + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //单位历史纪录
                sqlComm.CommandText = "INSERT INTO 单位历史账表 (单位ID, 日期, 单据编号, 摘要, 收入金额, 应收金额, 销售标记, 业务员ID, 冲抵单号, BeActive, 部门ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + strCount + "', N'应收账款结算单', " + labelSJJE.Text.ToString() + ", " + dKCJE1.ToString() + ", 1, " + comboBoxYWY.SelectedValue.ToString() + ", N'', 1,"+sBMID+")";
                sqlComm.ExecuteNonQuery();

                //单据明细
                for (j = 0; j < dataGridViewDJMX.Rows.Count; j++)
                {

                    sqlComm.CommandText = "INSERT INTO 结算收款明细表 (单据ID, 账簿ID, 支票号, 扣率, 摘要, 冲应付款, 付款金额, 备注) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[j].Cells[9].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[j].Cells[7].Value.ToString() + "', " + dataGridViewDJMX.Rows[j].Cells[5].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[j].Cells[3].Value.ToString() + "', " + dataGridViewDJMX.Rows[j].Cells[4].Value.ToString() + ", " + dataGridViewDJMX.Rows[j].Cells[6].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[j].Cells[8].Value.ToString() + "')";
                    sqlComm.ExecuteNonQuery();

                    //取得单据号 
                    sqlComm.CommandText = "SELECT @@IDENTITY";
                    sqldr = sqlComm.ExecuteReader();
                    sqldr.Read();
                    string sNo = sqldr.GetValue(0).ToString();
                    sqldr.Close();


                    //勾兑纪录
                    if (dSet.Tables.Contains("按照单据勾兑"))
                    {
                        dSet.Tables["按照单据勾兑"].AcceptChanges();
                        DataRow[] dtTemp1;
                        dtTemp1 = dSet.Tables["按照单据勾兑"].Select("勾兑标记=1");

                        for (i = 0; i < dtTemp1.Length; i++)
                        {
                            sTemp = dtTemp1[i][1].ToString().Substring(0, 3);
                            switch (sTemp)
                            {
                                case "BKP":
                                    if (Convert.ToDecimal(dtTemp1[i][6].ToString()) == 0)
                                        sqlComm.CommandText = "UPDATE 销售商品制单表 SET 未付款金额 = " + dtTemp1[i][6].ToString() + ", 已付款金额 =  " + dtTemp1[i][4].ToString() + ", 付款标记 = 1 , 结清时间 = '" + strDateSYS + "' WHERE (ID = " + dtTemp1[i][7].ToString() + ")";
                                    else
                                        sqlComm.CommandText = "UPDATE 销售商品制单表 SET 未付款金额 = " + dtTemp1[i][6].ToString() + ", 已付款金额 =  " + dtTemp1[i][4].ToString() + ", 付款标记 = 0 WHERE (ID = " + dtTemp1[i][7].ToString() + ")";
                                    break;

                                case "BTH":
                                    if (Convert.ToDecimal(dtTemp1[i][6].ToString()) == 0)
                                        sqlComm.CommandText = "UPDATE 销售退出汇总表 SET 未付款金额 = " + dtTemp1[i][6].ToString() + ", 已付款金额 =  " + dtTemp1[i][4].ToString() + ", 付款标记 = 1 , 结清时间 = '" + strDateSYS + "' WHERE (ID = " + dtTemp1[i][7].ToString() + ")";
                                    else
                                        sqlComm.CommandText = "UPDATE 销售退出汇总表 SET 未付款金额 = " + dtTemp1[i][6].ToString() + ", 已付款金额 =  " + dtTemp1[i][4].ToString() + ", 付款标记 = 0 WHERE (ID = " + dtTemp1[i][7].ToString() + ")";
                                    break;

                                case "BTB":
                                    if (Convert.ToDecimal(dtTemp1[i][6].ToString()) == 0)
                                        sqlComm.CommandText = "UPDATE 销售退补差价汇总表 SET 未付款金额 = " + dtTemp1[i][6].ToString() + ", 已付款金额 =  " + dtTemp1[i][4].ToString() + ", 付款标记 = 1 , 结清时间 = '" + strDateSYS + "' WHERE (ID = " + dtTemp1[i][7].ToString() + ")";
                                    else
                                        sqlComm.CommandText = "UPDATE 销售退补差价汇总表 SET 未付款金额 = " + dtTemp1[i][6].ToString() + ", 已付款金额 =  " + dtTemp1[i][4].ToString() + ", 付款标记 = 0 WHERE (ID = " + dtTemp1[i][7].ToString() + ")";
                                    break;

                            }
                            sqlComm.ExecuteNonQuery();

                            //总帐
                            sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 单位ID, 业务员ID, 单据编号, 原单据编号, 摘要, 结算金额, 应收金额, 未收金额, 已收金额, BeActive, 部门ID) VALUES ('" + strDateSYS + "', " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'" + dtTemp1[i][1].ToString() + "', N'应收账款结算单', " + dtTemp1[i][9].ToString() + ", " + dtTemp1[i][3].ToString() + ", " + dtTemp1[i][6].ToString() + ", " + dtTemp1[i][4].ToString() + ", 1,"+sBMID+")";
                            sqlComm.ExecuteNonQuery();


                        }



                    }

                    if (dSet.Tables.Contains("按照单据明细勾兑"))
                    {
                        dSet.Tables["按照单据明细勾兑"].AcceptChanges();
                        DataRow[] dtTemp2;
                        dtTemp2 = dSet.Tables["按照单据明细勾兑"].Select("勾兑标记=1");

                        for (i = 0; i < dtTemp2.Length; i++)
                        {
                            sTemp = dtTemp2[i][1].ToString().Substring(0, 3);
                            switch (sTemp)
                            {
                                case "BKP":
                                    sqlComm.CommandText = "UPDATE 销售商品制单明细表 SET 未付款金额 = " + dtTemp2[i][10].ToString() + ", 已付款金额 = " + dtTemp2[i][8].ToString() + " WHERE (ID = " + dtTemp2[i][13].ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE 销售商品制单表 SET 未付款金额 = 未付款金额 - " + dtTemp2[i][17].ToString() + ", 已付款金额 =  已付款金额 + " + dtTemp2[i][17].ToString() + " WHERE (ID = " + dtTemp2[i][12].ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    break;

                                case "BTH":
                                    sqlComm.CommandText = "UPDATE 销售退出明细表 SET 未付款金额 = " + dtTemp2[i][10].ToString() + ", 已付款金额 = -1*" + dtTemp2[i][8].ToString() + " WHERE (ID = " + dtTemp2[i][13].ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE 销售退出汇总表 SET 未付款金额 = 未付款金额 - (-1.0*" + dtTemp2[i][17].ToString() + "), 已付款金额 =  已付款金额 + (-1*" + dtTemp2[i][17].ToString() + ") WHERE (ID = " + dtTemp2[i][12].ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;

                                case "BTB":
                                    sqlComm.CommandText = "UPDATE 销售退补差价明细表 SET 未付款金额 = " + dtTemp2[i][10].ToString() + ", 已付款金额 = " + dtTemp2[i][8].ToString() + " WHERE (ID = " + dtTemp2[i][13].ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE 销售退补差价汇总表 SET 未付款金额 = 未付款金额 - " + dtTemp2[i][17].ToString() + ", 已付款金额 =  已付款金额 + " + dtTemp2[i][17].ToString() + " WHERE (ID = " + dtTemp2[i][12].ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    
                                    break;
                            }
                            //sqlComm.ExecuteNonQuery();

                            //勾兑记录
                            sqlComm.CommandText = "INSERT INTO 结算收款勾兑表 (付款ID, 勾兑方式, 勾兑ID, 单据编号, 已付款, BeActive) VALUES (" + sNo + ", 1, " + dtTemp2[i][13].ToString() + ", N'" + dtTemp2[i][1].ToString() + "', " + dtTemp2[i][17].ToString() + ", 1)";
                            sqlComm.ExecuteNonQuery();


                            //总库存
                            dKCJE = Convert.ToDecimal(dtTemp2[i][17].ToString());
                            sqlComm.CommandText = "SELECT  应收金额, 已收金额 FROM 商品表 WHERE (ID = " + dtTemp2[i][14].ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dYSYE1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                            }
                            sqldr.Close();

                            //余额
                            dYSYE -= dKCJE;
                            dYSYE1 += dKCJE;

                            dKCJE1 = dYSYE + dYSYE1;


                            sqlComm.CommandText = "UPDATE 商品表 SET 应收金额=" + dYSYE.ToString() + ", 已收金额=" + dYSYE1.ToString() + " WHERE (ID = " + dtTemp2[i][14].ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //总账历史纪录
                            sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 结算金额, 应收金额, 未收金额, 已收金额, BeActive, 部门ID) VALUES ('" + strDateSYS + "', " + dtTemp2[i][14].ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'应收账款结算单', " + dKCJE.ToString() + ", " + dKCJE1.ToString() + ", " + dYSYE.ToString() + ", " + dYSYE1.ToString() + " , 1, "+sBMID+")";
                            sqlComm.ExecuteNonQuery();

                            //分库存更新
                            sqlComm.CommandText = "SELECT  应收金额, 已收金额 FROM 库存表 WHERE (库房ID = " + dtTemp2[i][16].ToString() + ") AND (商品ID = " + dtTemp2[i][14].ToString() + ") AND (BeActive = 1)";

                            dKCJE = Convert.ToDecimal(dtTemp2[i][17].ToString());
                            dYSYE = 0; dYSYE1 = 0;
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dYSYE1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                            }
                            sqldr.Close();

                            //余额
                            dYSYE -= dKCJE;
                            dYSYE1 += dKCJE;

                            dKCJE1 = dYSYE + dYSYE1;
                            sqlComm.CommandText = "UPDATE 库存表 SET  应收金额=" + dYSYE.ToString() + ", 已收金额=" + dYSYE1.ToString() + " WHERE (库房ID = " + dtTemp2[i][16].ToString() + ") AND (商品ID = " + dtTemp2[i][14].ToString() + ") AND (BeActive = 1)";

                            //库存历史纪录
                            sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 结算金额, 应收金额, 未收金额, 已收金额, BeActive, 部门ID) VALUES (" + dtTemp2[i][16].ToString() + ", '" + strDateSYS + "', " + dtTemp2[i][14].ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'应收账款结算单', " + dKCJE.ToString() + ", " + dKCJE1.ToString() + ", " + dYSYE.ToString() + ", " + dYSYE1.ToString() + " , 1,"+sBMID+")";
                            sqlComm.ExecuteNonQuery();
                        }

                    }

                    if (dSet.Tables.Contains("按照明细数量勾兑"))
                    {
                        dSet.Tables["按照明细数量勾兑"].AcceptChanges();
                        DataRow[] dtTemp3;
                        dtTemp3 = dSet.Tables["按照明细数量勾兑"].Select("勾兑标记=1");

                        for (i = 0; i < dtTemp3.Length; i++)
                        {
                            sTemp = dtTemp3[i][1].ToString().Substring(0, 3);
                            switch (sTemp)
                            {
                                case "BKP":
                                    sqlComm.CommandText = "UPDATE 销售商品制单明细表 SET 未付款数量 = " + dtTemp3[i][9].ToString() + ", 已付款数量 = " + dtTemp3[i][7].ToString() + ", 未付款金额 = " + dtTemp3[i][3].ToString() + ", 已付款金额 =" + dtTemp3[i][11].ToString() + " WHERE (ID = " + dtTemp3[i][16].ToString() + ")";
                                    break;

                                case "BTH":
                                    sqlComm.CommandText = "UPDATE 销售退出明细表 SET 未付款数量 = " + dtTemp3[i][9].ToString() + ", 已付款数量 = " + dtTemp3[i][7].ToString() + ", 未付款金额 = " + dtTemp3[i][3].ToString() + ", 已付款金额 =" + dtTemp3[i][11].ToString() + " WHERE (ID = " + dtTemp3[i][16].ToString();
                                    break;

                                case "BTB":
                                    sqlComm.CommandText = "UPDATE 销售退补差价明细表 SET 未付款数量 = " + dtTemp3[i][9].ToString() + ", 已付款数量 = " + dtTemp3[i][7].ToString() + ", 未付款金额 = " + dtTemp3[i][3].ToString() + ", 已付款金额 =" + dtTemp3[i][11].ToString() + " WHERE (ID = " + dtTemp3[i][16].ToString();
                                    break;
                            }
                            sqlComm.ExecuteNonQuery();


                            //总库存
                            dKCJE = Convert.ToDecimal(dtTemp3[i][20].ToString());
                            sqlComm.CommandText = "SELECT  应收金额, 已收金额 FROM 商品表 WHERE (ID = " + dtTemp3[i][17].ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dYSYE1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                            }
                            sqldr.Close();

                            //余额
                            dYSYE -= dKCJE;
                            dYSYE1 += dKCJE;

                            dKCJE1 = dYSYE + dYSYE1;


                            sqlComm.CommandText = "UPDATE 商品表 SET 应收金额=" + dYSYE.ToString() + ", 已收金额=" + dYSYE1.ToString() + " WHERE (ID = " + dtTemp3[i][17].ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //总账历史纪录
                            sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 结算金额, 应收金额, 未收金额, 已收金额, BeActive, 部门ID) VALUES ('" + strDateSYS + "', " + dtTemp3[i][17].ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'应收账款结算单', " + dKCJE.ToString() + ", " + dKCJE1.ToString() + ", " + dYSYE.ToString() + ", " + dYSYE1.ToString() + " , 1,"+sBMID+")";
                            sqlComm.ExecuteNonQuery();

                            //分库存更新
                            sqlComm.CommandText = "SELECT  应收金额, 已收金额 FROM 库存表 WHERE (库房ID = " + dtTemp3[i][19].ToString() + ") AND (商品ID = " + dtTemp3[i][17].ToString() + ") AND (BeActive = 1)";

                            dKCJE = Convert.ToDecimal(dtTemp3[i][20].ToString());
                            dYSYE = 0; dYSYE1 = 0;
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dYSYE1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                            }
                            sqldr.Close();

                            //余额
                            dYSYE -= dKCJE;
                            dYSYE1 += dKCJE;

                            dKCJE1 = dYSYE + dYSYE1;
                            sqlComm.CommandText = "UPDATE 库存表 SET  应收金额=" + dYSYE.ToString() + ", 已收金额=" + dYSYE1.ToString() + " WHERE (库房ID = " + dtTemp3[i][19].ToString() + ") AND (商品ID = " + dtTemp3[i][17].ToString() + ") AND (BeActive = 1)";

                            //库存历史纪录
                            sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 结算金额, 应收金额, 未收金额, 已收金额, BeActive, 部门ID) VALUES (" + dtTemp3[i][19].ToString() + ", '" + strDateSYS + "', " + dtTemp3[i][17].ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'应收账款结算单', " + dKCJE.ToString() + ", " + dKCJE1.ToString() + ", " + dYSYE.ToString() + ", " + dYSYE1.ToString() + " , 1, "+sBMID+")";
                            sqlComm.ExecuteNonQuery();

                        }

                    }
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


            //MessageBox.Show("应收账款结算单保存成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = "应收账款结算单：" + labelDJBH.Text;
            isSaved = true;

            bool bClose = false;
            if (MessageBox.Show("应收账款结算单保存成功，是否关闭制单窗口", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                bClose = true;
            }

            if (MessageBox.Show("应收账款结算单保存成功，是否继续开始另一份制单？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                // 创建此子窗体的一个新实例。
                FormYSZKJS childFormYSZKJS = new FormYSZKJS();
                // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                childFormYSZKJS.MdiParent = this.MdiParent;

                childFormYSZKJS.strConn = strConn;

                childFormYSZKJS.intUserID = intUserID;
                childFormYSZKJS.intUserLimit = intUserLimit;
                childFormYSZKJS.strUserLimit = strUserLimit;
                childFormYSZKJS.strUserName = strUserName;
                childFormYSZKJS.Show();
            }

            if (bClose)
                this.Close();



        }

        private void toolStripButtonFP_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;

            sqlConn.Open();
            sqlComm.CommandText = "UPDATE 结算收款汇总表 SET 发票号 = N'" + textBoxFPH.Text + "', 开票日期='" + strDT + "' WHERE (ID = " + iDJID.ToString() + ")";
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();

            MessageBox.Show("发票号登记完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            if (!countAmount())
            {
                MessageBox.Show("应收账款结算单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "应收账款结算单(单据编号:" + labelDJBH.Text + ");制单日期：" + dateTimePickerZDRQ.Value.ToLongDateString() + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelJEHJ.Text + "(大写:" + labelDX.Text + ");发　票号：" + textBoxFPH.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            if (!countAmount())
            {
                MessageBox.Show("应收账款结算单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "应收账款结算单(单据编号:" + labelDJBH.Text + ");制单日期：" + dateTimePickerZDRQ.Value.ToLongDateString() + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelJEHJ.Text + "(大写:" + labelDX.Text + ");发　票号：" + textBoxFPH.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void FormYSZKJS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
                return;

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

        private void toolStripButtonFP_Click_1(object sender, EventArgs e)
        {
            //初始化单据列表
            dSet.Tables["单据明细表"].Clear();
            DataRow drTemp = dSet.Tables["单据明细表"].NewRow();
            dSet.Tables["单据明细表"].Rows.Add(drTemp);

            if (dSet.Tables.Contains("按照单据明细勾兑"))
            {
                dSet.Tables.Remove(dSet.Tables["按照单据明细勾兑"]);
            }
        }

        private void comboBoxGD_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxGD.SelectedIndex = 1;
        }

        private string getCompanyPay(int icompanyID)
        {
            string strPay = "0.00";

            sqlConn.Open();
            sqlComm.CommandText = "SELECT SUM(未付款金额) FROM 收款明细视图 WHERE (单位ID = " + icompanyID .ToString()+ ")";

            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                try
                {
                    strPay = decimal.Parse(sqldr.GetValue(0).ToString()).ToString("f2");
                }
                catch
                {
                }
            }


            sqlConn.Close();

            return strPay;

        }


    }
}