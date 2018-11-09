using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKCJWCKDJ_EDIT : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";
        public int intDJID = 0;
        public int intCD = 0;
        public string strCD = "";

        public int iStyle = 0;

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private int iSupplyCompany = 0;
        private int iRowCount = 0;
        private bool isSaved = false;

        private decimal dDJSUM = 0;
        private int iYWY = 0;

        private ClassGetInformation cGetInformation;
        private bool bCheck = true;
        private int iBM = 0;

        
        public FormKCJWCKDJ_EDIT()
        {
            InitializeComponent();
        }

        private void FormKCJWCKDJ_EDIT_Load(object sender, EventArgs e)
        {
            int i;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            if (intDJID == 0)
                return;

            dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            sqlConn.Open();
            //sqlComm.CommandText = "SELECT 借物出库汇总表.单位ID, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.联系电话, 借物出库汇总表.联系人, 借物出库汇总表.收货人, 借物出库汇总表.到站, 借物出库汇总表.运输方式, 借物出库汇总表.详细地址, 借物出库汇总表.物流名称, 借物出库汇总表.单号, 借物出库汇总表.邮政编码, 职员表.职员姓名 AS 业务员, [职员表_1].职员姓名 AS 操作员, 借物出库汇总表.单据编号, 借物出库汇总表.备注, 借物出库汇总表.价税合计,借物出库汇总表.业务员ID, 借物出库汇总表.部门ID FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 借物出库汇总表.操作员ID = [职员表_1].ID WHERE (借物出库汇总表.ID = " + intDJID.ToString() + ") AND 借物出库汇总表.BeActive <> 0 AND (借物出库汇总表.冲抵单号ID IS NULL)";
            sqlComm.CommandText = "SELECT 借物出库汇总表.单位ID, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.联系电话, 借物出库汇总表.联系人, 借物出库汇总表.收货人, 借物出库汇总表.到站, 借物出库汇总表.运输方式, 借物出库汇总表.详细地址, 借物出库汇总表.物流名称, 借物出库汇总表.单号, 借物出库汇总表.邮政编码, 职员表.职员姓名 AS 业务员, [职员表_1].职员姓名 AS 操作员, 借物出库汇总表.单据编号, 借物出库汇总表.备注, 借物出库汇总表.价税合计,借物出库汇总表.业务员ID, 借物出库汇总表.部门ID, 借物出库汇总表.冲抵单号ID FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 借物出库汇总表.操作员ID = [职员表_1].ID WHERE (借物出库汇总表.ID = " + intDJID.ToString() + ") AND 借物出库汇总表.BeActive <> 0 ";

            if (dSet.Tables.Contains("送货表")) dSet.Tables.Remove("送货表");
            sqlDA.Fill(dSet, "送货表");

            if (dSet.Tables["送货表"].Rows.Count < 1)
            {
                textBoxDWBH.Text = "";
                textBoxDWMC.Text = "";
                textBoxLXDH.Text = "";
                textBoxLXR.Text = "";
                textBoxSHR.Text = "";
                textBoxDZ.Text = "";
                comboBoxYSFS.Text = "";
                textBoxXXDZ.Text = "";
                textBoxWLMC.Text = "";
                textBoxDH.Text = "";
                textBoxYZBM.Text = "";
                comboBoxYWY.Text = "";
                labelCZY.Text = "";
                labelDJBH.Text = "";
                textBoxBZ.Text = "";
                iSupplyCompany = 0;
                sqlConn.Close();
                isSaved = true;

                ReturntoolStripButton.Enabled = false;
                saveToolStripButton.Enabled = false;
                return;
            }
            else
            {
                if (dSet.Tables["送货表"].Rows[0][18].ToString() != "")
                {
                    try
                    {
                        iBM = int.Parse(dSet.Tables["送货表"].Rows[0][18].ToString());
                    }
                    catch
                    {
                        iBM = 0;
                    }

                }
                //冲抵
                if (dSet.Tables["送货表"].Rows[0][19].ToString() != "")
                {
                    try
                    {
                        intCD = int.Parse(dSet.Tables["送货表"].Rows[0][19].ToString());
                    }
                    catch
                    {
                        intCD = 0;
                    }
                }
                textBoxLXDH.Text = dSet.Tables["送货表"].Rows[0][3].ToString();
                textBoxLXR.Text = dSet.Tables["送货表"].Rows[0][4].ToString();
                textBoxSHR.Text = dSet.Tables["送货表"].Rows[0][5].ToString();
                textBoxDZ.Text = dSet.Tables["送货表"].Rows[0][6].ToString();
                comboBoxYSFS.Text = dSet.Tables["送货表"].Rows[0][7].ToString();
                textBoxXXDZ.Text = dSet.Tables["送货表"].Rows[0][8].ToString();
                textBoxWLMC.Text = dSet.Tables["送货表"].Rows[0][9].ToString();
                textBoxDH.Text = dSet.Tables["送货表"].Rows[0][10].ToString();
                textBoxYZBM.Text = dSet.Tables["送货表"].Rows[0][11].ToString();
                comboBoxYWY.Text = dSet.Tables["送货表"].Rows[0][12].ToString();
                labelCZY.Text = dSet.Tables["送货表"].Rows[0][13].ToString();
                labelDJBH.Text = dSet.Tables["送货表"].Rows[0][14].ToString();
                textBoxBZ.Text = dSet.Tables["送货表"].Rows[0][15].ToString();
                iSupplyCompany = Convert.ToInt32(dSet.Tables["送货表"].Rows[0][0].ToString());
                textBoxDWBH.Text = dSet.Tables["送货表"].Rows[0][1].ToString();
                textBoxDWMC.Text = dSet.Tables["送货表"].Rows[0][2].ToString();
                dDJSUM = Convert.ToDecimal(dSet.Tables["送货表"].Rows[0][16].ToString());
                iYWY = Convert.ToInt32(dSet.Tables["送货表"].Rows[0][17].ToString());

            }
            if (iBM != 0)
            {
                sqlComm.CommandText = "SELECT 部门名称 FROM 部门表 WHERE (ID = " + iBM.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                    comboBoxBM.Text = sqldr.GetValue(0).ToString();
                    break;
                }
                sqldr.Close();
            }


            switch (iStyle)
            {
                case 0: //冲红
                    bCheck = false;
                    this.Text += ":单据冲红";
                    saveToolStripButton.Text = "冲红";

                    sqlComm.CommandText = "SELECT 销售商品定义表.保留, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格,库房表.库房编号, 库房表.库房名称, 借物出库明细表.数量, 借物出库明细表.单价, 借物出库明细表.金额, 借物出库明细表.库存成本价, 借物出库明细表.出库金额, 借物出库明细表.备注, 借物出库明细表.商品ID, 借物出库明细表.库房ID, 商品表.库存数量, 销售商品定义表.统计标志, 商品表.最终进价, 借物出库明细表.ID FROM 借物出库明细表 INNER JOIN 商品表 ON 借物出库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID CROSS JOIN 销售商品定义表 WHERE (借物出库明细表.表单ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
                    sqlDA.Fill(dSet, "单据表");
                    dataGridViewDJMX.DataSource = dSet.Tables["单据表"];

                    sqlConn.Close();

                    dataGridViewDJMX.ReadOnly = true;
                    dataGridViewDJMX.AllowUserToDeleteRows = false;
                    dataGridViewDJMX.AllowUserToAddRows = false;

                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[9].Visible = false;
                    dataGridViewDJMX.Columns[12].Visible = false;
                    dataGridViewDJMX.Columns[13].Visible = false;
                    dataGridViewDJMX.Columns[14].Visible = false;
                    dataGridViewDJMX.Columns[15].Visible = false;
                    dataGridViewDJMX.Columns[17].Visible = false;

                    dataGridViewDJMX.Columns[16].Visible = false;

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

                    dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
                    dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
                    dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
                    break;
                case 1://修改
                    ReturntoolStripButton.Enabled = false;
                    this.Text += ":单据修改";
                    saveToolStripButton.Text = "修改";


                    sqlComm.CommandText = "SELECT 销售商品定义表.保留, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格,库房表.库房编号, 库房表.库房名称, 借物出库明细表.数量, 借物出库明细表.单价, 借物出库明细表.金额, 借物出库明细表.库存成本价, 借物出库明细表.出库金额, 借物出库明细表.备注, 借物出库明细表.商品ID, 借物出库明细表.库房ID, 商品表.库存数量, 销售商品定义表.统计标志, 商品表.最终进价, 借物出库明细表.ID, 借物出库明细表.数量 AS 原单数量, 借物出库明细表.单价 AS 原单单价 FROM 借物出库明细表 INNER JOIN 商品表 ON 借物出库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID CROSS JOIN 销售商品定义表 WHERE (借物出库明细表.表单ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
                    sqlDA.Fill(dSet, "单据表");
                    dataGridViewDJMX.DataSource = dSet.Tables["单据表"];

                    sqlConn.Close();

                    //dataGridViewDJMX.Columns[0].Visible = false;

                    dataGridViewDJMX.Columns[12].Visible = false;
                    dataGridViewDJMX.Columns[13].Visible = false;
                    dataGridViewDJMX.Columns[15].Visible = false;
                    dataGridViewDJMX.Columns[17].Visible = false;
                    dataGridViewDJMX.Columns[3].ReadOnly = true;
                    dataGridViewDJMX.Columns[8].ReadOnly = true;
                    dataGridViewDJMX.Columns[9].ReadOnly = true;
                    dataGridViewDJMX.Columns[10].ReadOnly = true;
                    dataGridViewDJMX.Columns[14].ReadOnly = true;
                    dataGridViewDJMX.Columns[16].ReadOnly = true;
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
                    dataGridViewDJMX.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[16].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[18].ReadOnly = true;
                    dataGridViewDJMX.Columns[19].ReadOnly = true;

                    dataGridViewDJMX.ShowCellErrors = true;


                    for (i = 0; i < dataGridViewDJMX.Columns.Count; i++)
                    {
                        dataGridViewDJMX.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    iRowCount = dataGridViewDJMX.Rows.Count - 1;

                    break;
                default:
                    break;
            }
            countAmount();


            dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.RowValidating += dataGridViewDJMX_RowValidating;

           

        }
        private bool countAmount()
        {
            decimal fSum = 0;
            decimal fSum1 = 0;
            decimal fCSum = 0;

            decimal fTemp, fTemp1;
            decimal fCount = 0;
            bool bCheck1 = true;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                if (i >= iRowCount)
                    dataGridViewDJMX.Rows[i].Cells[0].Value = 1;

                if (dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "输入所借商品";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "输入所借商品";
                    bCheck1 = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[4].ErrorText = "输入所借商品库房";
                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "输入所借商品库房";
                    bCheck1 = false;
                }


                if (dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "输入所借商品数量";
                    bCheck1 = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[7].ErrorText = "输入所借商品价格";
                    bCheck1 = false;
                }

                if (!bCheck1)
                    continue;

                //库存成本
                if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[9].Value = 0;

                //库存量
                if (dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[14].Value = 0;

                if (dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[16].Value = 0;
                //颜色表示
                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value.ToString()) >= 0) //借出
                {
                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value) < Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value))
                        dataGridViewDJMX.Rows[i].Cells[7].Style.BackColor = Color.LightPink;
                    else
                        dataGridViewDJMX.Rows[i].Cells[7].Style.BackColor = Color.White;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value) > Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[14].Value))
                        dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.LightPink;
                    else
                        dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.White;
                }
                else //借入
                {
                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value) > Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value))
                        dataGridViewDJMX.Rows[i].Cells[7].Style.BackColor = Color.LightPink;
                    else
                        dataGridViewDJMX.Rows[i].Cells[7].Style.BackColor = Color.White;
                    dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.White;
                }


                //数量
                fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                //单价
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);

                //金额
                dataGridViewDJMX.Rows[i].Cells[8].Value = Math.Round(fTemp * fTemp1, 2);

                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                //出库金额
                dataGridViewDJMX.Rows[i].Cells[10].Value = Math.Round(fTemp * fTemp1, 2);

                if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value))
                {
                    continue;
                }


                fCount += 1;
                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelJEHJ.Text = fSum.ToString();
            labelCKJE.Text = fSum1.ToString();
            labelSLHJ.Text = fCSum.ToString();
            toolStripStatusLabelMXJLS.Text = fCount.ToString();
            labelDX.Text = cGetInformation.changeDAXIE(labelCKJE.Text);

            return bCheck1;

        }

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int iRe = 0;

            if (!bCheck)
                return;

            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
                return;
            if (e.RowIndex < iRowCount && e.ColumnIndex != 0) return;


            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            switch (e.ColumnIndex)
            {
                case 2: //商品编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    iRe = cGetInformation.getCommInformation(20, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewDJMX.CancelEdit();
                        return;
                    }
                    if (iRe == 0) //失败
                    //if (cGetInformation.getCommInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].ErrorText = "商品编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommKCCBJ.ToString();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;

                case 1: //商品名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                        break;

                    }

                    iRe = cGetInformation.getCommInformation(10, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewDJMX.CancelEdit();
                        return;
                    }
                    if (iRe == 0) //失败
                    //if (cGetInformation.getCommInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].ErrorText = "商品助记码输入错误";
                    }
                    else
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommKCCBJ.ToString();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    }
                    break;
                case 4: //库房编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;

                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].ErrorText = "库房编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;
                case 5: //库房名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].ErrorText = "库房助记码输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }

                    break;
                case 6:  //商品数量
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut == 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "商品数量输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "商品数量输入类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 7: //商品价格
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;
                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == "" || dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value.ToString() == "0")
                    {
                        MessageBox.Show("请先输入所借商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;
                    }

                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value.ToString() == "" || dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value.ToString() == "0")
                    {
                        MessageBox.Show("请先输入所借商品数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;
                    }

                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        if (detOut >= 0)
                        {
                            detOut = Math.Round(detOut, 2);

                            if (Convert.ToDecimal(dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value.ToString()) > 0) //借出
                            {
                                if (detOut.CompareTo(dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value) < 0)
                                {
                                    if (MessageBox.Show("商品价格低于成本价，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                        e.Cancel = true;
                                    else
                                    {
                                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = detOut;
                                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                                    }

                                }
                            }
                            else //借入
                            {
                                if (detOut.CompareTo(dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value) > 0)
                                {
                                    if (MessageBox.Show("商品价格高于成本价，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                        e.Cancel = true;
                                    else
                                    {
                                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = detOut;
                                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                                    }

                                }
                            }
                        }
                        else
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "商品价格输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "商品数量价格类型错误";
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
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[4];
                                        break;
                                    case 4:
                                    case 5:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[6];
                                        break;
                                    case 6:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    case 7:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[11];
                                        break;
                                    case 11:
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

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex < iRowCount) return;

            if (e.ColumnIndex == 1 || e.ColumnIndex == 2) //商品编号
            {
                if (cGetInformation.getCommInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.iCommNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommKCCBJ;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = cGetInformation.decCommZZJJ;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == "")
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;

                    cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value));
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.dKCL;

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[4];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }

            if (e.ColumnIndex == 4 || e.ColumnIndex == 5) //库房编号
            {
                if (cGetInformation.getKFInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;

                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == "")
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;

                    cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value));
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.dKCL;
                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[6];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                }
            }


        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;
            int i, j, k;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;


            string strDateSYS;
            cGetInformation.getSystemDateTime();
            strDateSYS = cGetInformation.strSYSDATATIME;
            //表单汇总
            string sBMID = "NULL";
            if (iBM != 0)
                sBMID = iBM.ToString();

            switch (iStyle)
            {
                case 0://冲红
                    //保存完毕
                    if (isSaved)
                    {
                        MessageBox.Show("库存借物单已经冲红,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    //格式确认
                    if (!countAmount())
                    {
                        MessageBox.Show("库存借物单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (intCD != 0)
                    {
                        if (intCD != -1)
                        {
                            if (MessageBox.Show("该单据已有冲抵记录，是否强行冲红？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                                return;
                        }

                    }


                    sqlConn.Open();

                    //得到上次结转时间
                    string sSCJZSJ = "";
                    sqlComm.CommandText = "SELECT 结算时间,ID FROM 结转汇总表 ORDER BY 结算时间 DESC";
                    sqldr = sqlComm.ExecuteReader();
                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                    }
                    sqldr.Close();

                    if (sSCJZSJ == "") //没有结算
                    {
                        sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
                        sqldr = sqlComm.ExecuteReader();
                        while (sqldr.Read())
                        {
                            sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                        }
                        sqldr.Close();
                    }

                    //得到制单日期
                    string strDate1 = "";
                    sqlComm.CommandText = "SELECT 日期 from 借物出库汇总表 WHERE (ID = " + intDJID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    while (sqldr.Read())
                    {
                        strDate1 = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                    }
                    sqldr.Close();

                    if (DateTime.Parse(strDate1) <= DateTime.Parse(sSCJZSJ)) //有转结记录
                    {
                        if (MessageBox.Show("制单后已有转结记录：" + sSCJZSJ + "，是否强行冲红？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                        {
                            sqlConn.Close();
                            return;
                        }
                    }
            
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {

                        //表单汇总
                        sqlComm.CommandText = "UPDATE 借物出库汇总表 SET BeActive = 0 WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        sqlComm.CommandText = "UPDATE 借物出库汇总表 SET 冲红时间 = '" + strDateSYS + "' WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //冲抵
                        sqlComm.CommandText = "UPDATE 借物出库汇总表 SET 冲抵单号ID = NULL WHERE (冲抵单号ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();


                        //库存
                        for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;


                            //计算该单的每个商品库存
                            dKUL1 = (-1)*Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                            dKCJE1 = (-1)*Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                            dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                            //总库存变更
                            sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 最高进价, 最低进价, 最终进价, 库存金额, 应收金额  FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(5).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(6).ToString());
                            }
                            sqldr.Close();


                            dKUL -= dKUL1;
                            //dKCJE -= dKCJE1;
                            dKCJE = dKUL * dKCCBJ;

                            sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额=" + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //总账历史纪录
                            sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 借物数量, 借物单价, 借物金额, 出库数量, 出库单价, 出库金额, BeActive, 总结存数量, 总结存金额, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "冲', N'借物出库制单冲红', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ","+sBMID+")";
                            sqlComm.ExecuteNonQuery();

                            //分库存更新
                            sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                            sqldr = sqlComm.ExecuteReader();

                            if (sqldr.HasRows) //存在库存
                            {
                                sqldr.Read();
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString()); //库存金额
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString()); //库存成本价
                                sqldr.Close();

                                dKUL -= dKUL1;
                                //dKCJE -= dKCJE1;
                                dKCJE = dKUL * dKCCBJ;

                                sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + " WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                sqlComm.ExecuteNonQuery();


                                //库房账历史纪录
                                sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 借物数量, 借物单价, 借物金额, 出库数量, 出库单价, 出库金额, BeActive, 库房结存数量, 库房结存金额, 部门ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "冲', N'借物出库制单冲红', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ","+sBMID+")";
                                sqlComm.ExecuteNonQuery();
                            }
                            else
                                sqldr.Close();
                        }

                        //条码
                        sqlComm.CommandText = "DELETE FROM 商品条码表 WHERE (单据编号 = N'" + labelDJBH.Text + "')";
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



                    //MessageBox.Show("库存借物单冲红成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSaved = true;
                    if (MessageBox.Show("库存借物单冲红成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Close();
                    }
                    break;


                case 1://修改

                    //保存完毕
                    if (isSaved)
                    {
                        MessageBox.Show("库存借物单已经修改,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    //格式确认
                    if (!countAmount())
                    {
                        MessageBox.Show("库存借物单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    //单据明细
                    dataGridViewDJMX.DataSource = null;
                    for (i = 0; i < iRowCount; i++)
                    {

                        if (!Convert.ToBoolean(dSet.Tables["单据表"].Rows[i][0].ToString())) //不保留
                        {
                        }
                        else //保留
                        {

                            if (dSet.Tables["单据表"].Rows[i][6].ToString() != dSet.Tables["单据表"].Rows[i][18].ToString() || dSet.Tables["单据表"].Rows[i][7].ToString() != dSet.Tables["单据表"].Rows[i][19].ToString()) //已修改
                            {

                                DataRow drTemp = dSet.Tables["单据表"].NewRow();
                                dSet.Tables["单据表"].Rows.Add(drTemp);

                                for (k = 1; k < dSet.Tables["单据表"].Columns.Count; k++)
                                {
                                    drTemp[k] = dSet.Tables["单据表"].Rows[i][k];
                                }
                                drTemp[0] = 1;



                                dSet.Tables["单据表"].Rows[i][0] = 0;
                                dSet.Tables["单据表"].Rows[i][6] = dSet.Tables["单据表"].Rows[i][18];
                                dSet.Tables["单据表"].Rows[i][7] = dSet.Tables["单据表"].Rows[i][19];

                            }


                        }
                    }
                    dataGridViewDJMX.DataSource = dSet.Tables["单据表"];
                    //dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[15].Visible = false;
                    dataGridViewDJMX.Columns[16].Visible = false;
                    dataGridViewDJMX.Columns[3].ReadOnly = true;
                    dataGridViewDJMX.Columns[8].ReadOnly = true;
                    dataGridViewDJMX.Columns[9].ReadOnly = true;
                    dataGridViewDJMX.Columns[10].ReadOnly = true;
                    dataGridViewDJMX.Columns[11].ReadOnly = true;
                    dataGridViewDJMX.Columns[14].ReadOnly = true;
                    dataGridViewDJMX.Columns[17].ReadOnly = true;
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
                    dataGridViewDJMX.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.ShowCellErrors = true;
                    dataGridViewDJMX.Columns[18].ReadOnly = true;
                    dataGridViewDJMX.Columns[19].ReadOnly = true;

                    for (i = 0; i < dataGridViewDJMX.Columns.Count; i++)
                    {
                        dataGridViewDJMX.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }




                    //格式确认
                    if (!countAmount())
                    {
                        MessageBox.Show("库存借物单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        //表单汇总
                        sqlComm.CommandText = "UPDATE 借物出库汇总表 SET 金额 = " + labelJEHJ.Text + ", 价税合计 = " + labelJEHJ.Text + ", 未付款金额 = 0, 已付款金额 = 0 WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();


                        //明细&库存 原单据
                        for (i = 0; i < iRowCount; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;

                            if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value.ToString())) //删除
                            {

                                //计算该单的每个商品库存
                                dKUL1 = (-1) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                                dKCJE1 = (-1) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                                dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                                //总库存变更
                                sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 最高进价, 最低进价, 最终进价, 库存金额, 应收金额  FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                                sqldr = sqlComm.ExecuteReader();
                                while (sqldr.Read())
                                {
                                    dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                    dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                    dKCJE = Convert.ToDecimal(sqldr.GetValue(5).ToString());
                                    dYSYE = Convert.ToDecimal(sqldr.GetValue(6).ToString());
                                }
                                sqldr.Close();


                                dKUL -= dKUL1;
                                //dKCJE -= dKCJE1;
                                dKCJE = dKUL * dKCCBJ;

                                sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额=" + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                                sqlComm.ExecuteNonQuery();

                                //总账历史纪录
                                sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 借物数量, 借物单价, 借物金额, 出库数量, 出库单价, 出库金额, BeActive, 总结存数量, 总结存金额, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "改', N'借物出库制单修改', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ","+sBMID+")";
                                sqlComm.ExecuteNonQuery();

                                //分库存更新
                                sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                sqldr = sqlComm.ExecuteReader();

                                if (sqldr.HasRows) //存在库存
                                {
                                    sqldr.Read();
                                    dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                    dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString()); //库存金额
                                    dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString()); //库存成本价
                                    sqldr.Close();

                                    dKUL -= dKUL1;
                                    //dKCJE -= dKCJE1;
                                    dKCJE = dKUL * dKCCBJ;

                                    sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + " WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                    sqlComm.ExecuteNonQuery();


                                    //库房账历史纪录
                                    sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 借物数量, 借物单价, 借物金额, 出库数量, 出库单价, 出库金额, BeActive, 库房结存数量, 库房结存金额, 部门ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "改', N'借物出库制单修改', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ","+sBMID+")";
                                    sqlComm.ExecuteNonQuery();
                                }
                                else
                                    sqldr.Close();






                                sqlComm.CommandText = "DELETE FROM 借物出库明细表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[17].Value.ToString() + ")";
                                sqlComm.ExecuteNonQuery();
                            }
                        }

                        //新增明细
                        for (i = iRowCount; i < dataGridViewDJMX.Rows.Count; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;


                            sqlComm.CommandText = "INSERT INTO 借物出库明细表 (表单ID, 商品ID, 库房ID, 数量, 单价, 金额, 库存成本价, 出库金额, 备注, BeActive) VALUES (" + intDJID.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + "', 1)";
                            sqlComm.ExecuteNonQuery();


                            //计算该单的每个商品库存
                            dKUL1 =  Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                            dKCJE1 =  Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                            dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                            //总库存变更
                            sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 最高进价, 最低进价, 最终进价, 库存金额, 应收金额  FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(5).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(6).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                            }
                            sqldr.Close();


                            dKUL -= dKUL1;
                            //dKCJE -= dKCJE1;
                            dKCJE = dKUL * dKCCBJ;

                            sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额=" + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //总账历史纪录
                            sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 借物数量, 借物单价, 借物金额, 出库数量, 出库单价, 出库金额, BeActive, 总结存数量, 总结存金额, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "改', N'借物出库制单修改', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ","+sBMID+")";
                            sqlComm.ExecuteNonQuery();

                            //分库存更新
                            sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                            sqldr = sqlComm.ExecuteReader();

                            if (sqldr.HasRows) //存在库存
                            {
                                sqldr.Read();
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString()); //库存金额
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString()); //库存成本价
                                sqldr.Close();

                                dKUL -= dKUL1;
                                //dKCJE -= dKCJE1;
                                dKCJE = dKUL * dKCCBJ;
                                sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + " WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                sqlComm.ExecuteNonQuery();


                                //库房账历史纪录
                                sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 借物数量, 借物单价, 借物金额, 出库数量, 出库单价, 出库金额, BeActive, 库房结存数量, 库房结存金额, 部门ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "改', N'借物出库制单修改', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ","+sBMID+")";
                                sqlComm.ExecuteNonQuery();
                            }
                            else
                                sqldr.Close();


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


                    //MessageBox.Show("库存借物单修改成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSaved = true;
                    if (MessageBox.Show("库存借物单修改成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Close();
                    }

                    break;





            }
 
        }

        private void FormKCJWCKDJ_EDIT_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
                return;

            DialogResult dr = MessageBox.Show(this, "单据修改尚未保存，确定要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("库存借物单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "库存借物单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";出库金额合计：" + labelCKJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("库存借物单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "库存借物单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";出库金额合计：" + labelCKJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void ReturntoolStripButton_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;
            int i, j, k;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;


            string strDateSYS;
            cGetInformation.getSystemDateTime();
            strDateSYS = cGetInformation.strSYSDATATIME;


                    //保存完毕
                    if (isSaved)
                    {
                        MessageBox.Show("库存借物单已经冲抵完毕,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    //格式确认
                    if (!countAmount())
                    {
                        MessageBox.Show("库存借物单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        //表单汇总
                        sqlComm.CommandText = "UPDATE 借物出库汇总表 SET BeActive = 1, 冲抵单号ID= " + intDJID .ToString()+ " WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();


                        //库存
                        for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;


                            //计算该单的每个商品库存
                            dKUL1 = (-1) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                            dKCJE1 = (-1) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                            dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                            //总库存变更
                            sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 最高进价, 最低进价, 最终进价, 库存金额, 应收金额  FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(5).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(6).ToString());
                            }
                            sqldr.Close();


                            dKUL -= dKUL1;
                            //dKCJE -= dKCJE1;
                            dKCJE = dKUL * dKCCBJ;

                            sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额=" + dKCJE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //总账历史纪录
                            sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 借物数量, 借物单价, 借物金额, 出库数量, 出库单价, 出库金额, BeActive, 总结存数量, 总结存金额) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "冲', N'借物出库制单冲红', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //分库存更新
                            sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                            sqldr = sqlComm.ExecuteReader();

                            if (sqldr.HasRows) //存在库存
                            {
                                sqldr.Read();
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString()); //库存金额
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString()); //库存成本价
                                sqldr.Close();

                                dKUL -= dKUL1;
                                //dKCJE -= dKCJE1;
                                dKCJE = dKUL * dKCCBJ;

                                sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + " WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                sqlComm.ExecuteNonQuery();


                                //库房账历史纪录
                                sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 借物数量, 借物单价, 借物金额, 出库数量, 出库单价, 出库金额, BeActive, 库房结存数量, 库房结存金额) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "冲', N'借物出库制单冲红', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + ")";
                                sqlComm.ExecuteNonQuery();
                            }
                            else
                                sqldr.Close();
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



                    //MessageBox.Show("库存借物单冲红成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSaved = true;
                    if (MessageBox.Show("库存借物单冲抵成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Close();
                    }

        }


    }
}