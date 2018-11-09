using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJHTCZD_EDIT : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";
        public int intDJID = 0;

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
        
        public FormJHTCZD_EDIT()
        {
            InitializeComponent();
        }

        private void FormJHTCZD_EDIT_Load(object sender, EventArgs e)
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

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 进货退出汇总表.单据编号, 进货退出汇总表.日期, [职员表_1].职员姓名 AS 操作员,职员表.职员姓名 AS 业务员, 单位表.单位编号, 单位表.单位名称, 进货退出汇总表.发票号, 进货退出汇总表.支票号, 进货退出汇总表.合同号, 进货退出汇总表.价税合计, 进货退出汇总表.备注, 单位表.ID,进货退出汇总表.业务员ID, 进货退出汇总表.部门ID FROM 进货退出汇总表 INNER JOIN 职员表 ON 进货退出汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 进货退出汇总表.操作员ID = [职员表_1].ID INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID WHERE (进货退出汇总表.ID = " + intDJID.ToString() + ") AND (进货退出汇总表.BeActive<>0)";
            sqldr = sqlComm.ExecuteReader();

            if (!sqldr.HasRows)
            {
                isSaved = true;
                sqldr.Close();
                sqlConn.Close();
                return;
            }

            while (sqldr.Read())
            {
                if (sqldr.GetValue(13).ToString() != "")
                {
                    try
                    {
                        iBM = int.Parse(sqldr.GetValue(13).ToString());
                    }
                    catch
                    {
                        iBM = 0;
                    }

                }
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelCZY.Text = sqldr.GetValue(2).ToString();
                comboBoxYWY.Text = sqldr.GetValue(3).ToString();
                textBoxDWBH.Text = sqldr.GetValue(4).ToString();
                textBoxDWMC.Text = sqldr.GetValue(5).ToString();
                textBoxFPH.Text = sqldr.GetValue(6).ToString();
                textBoxZPH.Text = sqldr.GetValue(7).ToString();
                textBoxHTH.Text = sqldr.GetValue(8).ToString();
                textBoxBZ.Text = sqldr.GetValue(10).ToString();
                iSupplyCompany = Convert.ToInt32(sqldr.GetValue(11).ToString());
                dDJSUM = Convert.ToDecimal(sqldr.GetValue(9).ToString());
                iYWY = Convert.ToInt32(sqldr.GetValue(12).ToString());
            }

            sqldr.Close();
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
                    this.Text += ":单据冲红";

                    sqlComm.CommandText = "SELECT 购进商品制单明细定义表.保留, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格,商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 进货退出明细表.数量, 进货退出明细表.单价, 进货退出明细表.金额, 进货退出明细表.扣率, 进货退出明细表.实计金额, 进货退出明细表.商品ID, 进货退出明细表.库房ID, 商品表.库存数量, 购进商品制单明细定义表.统计标志, 进货退出明细表.赠品, 进货退出明细表.ID FROM 商品表 INNER JOIN 进货退出明细表 ON 商品表.ID = 进货退出明细表.商品ID INNER JOIN 库房表 ON 进货退出明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (进货退出明细表.单据ID = " + intDJID.ToString() + ")";

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
                    dataGridViewDJMX.Columns[16].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.ShowCellErrors = true;
                    break;
                case 1://修改
                    this.Text += ":单据修改";

                    sqlComm.CommandText = "SELECT 购进商品制单明细定义表.保留, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格,商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 进货退出明细表.数量, 进货退出明细表.单价, 进货退出明细表.金额, 进货退出明细表.扣率, 进货退出明细表.实计金额, 进货退出明细表.商品ID, 进货退出明细表.库房ID, 商品表.库存数量, 购进商品制单明细定义表.统计标志, 进货退出明细表.赠品, 进货退出明细表.ID FROM 商品表 INNER JOIN 进货退出明细表 ON 商品表.ID = 进货退出明细表.商品ID INNER JOIN 库房表 ON 进货退出明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (进货退出明细表.单据ID = " + intDJID.ToString() + ")";

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
                    dataGridViewDJMX.Columns[4].ReadOnly = true;
                    dataGridViewDJMX.Columns[9].ReadOnly = true;
                    dataGridViewDJMX.Columns[11].ReadOnly = true;
                    dataGridViewDJMX.Columns[14].ReadOnly = true;
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
                    dataGridViewDJMX.ShowCellErrors = true;


                    iRowCount = dataGridViewDJMX.Rows.Count - 1;

                    break;
                default:
                    break;
            }

            countAmount();
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

                if (i >= iRowCount)
                    dataGridViewDJMX.Rows[i].Cells[0].Value = 1;

                //库房ID
                if (dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "0" || dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() == "")
                {
                    bCheck = false;
                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "请输入商品库房";
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "请输入商品库房";
                    continue;
                }


                //数量
                if (dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() == "")
                    fTemp = 0;
                else
                    fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);

                if (fTemp > Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[14].Value))
                {
                    dataGridViewDJMX.Rows[i].Cells[7].Value = dataGridViewDJMX.Rows[i].Cells[14].Value;
                    fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);
                }
                fCSum += fTemp;

                //单价
                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);

                //扣率
                if (dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[10].Value = 100;
                }

                //赠品
                if (dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[16].Value = 0;
                }

                //金额
                if (Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[16].Value)) //赠品
                    dataGridViewDJMX.Rows[i].Cells[9].Value = 0;
                else
                    dataGridViewDJMX.Rows[i].Cells[9].Value = Math.Round(fTemp * fTemp1, 2);

                //实计金额
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                dataGridViewDJMX.Rows[i].Cells[11].Value = fTemp1 * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value) / 100;


                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value);

                fCount += 1;

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelSLHJ.Text = fCSum.ToString();
            labelJEHJ.Text = fSum.ToString();
            labelSJJE.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;


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
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = Math.Round(Decimal.Zero, 2);

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
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        getKCL();

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
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
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = Math.Round(Decimal.Zero, 2);
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
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        getKCL();

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    }
                    break;
                case 5: //库房编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].ErrorText = "库房编号输入错误";
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
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        getKCL();
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;
                case 6: //库房名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "库房助记码输入错误";
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
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        getKCL();
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }

                    break;
                case 7:  //商品数量
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut < 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "商品数量输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "商品数量输入类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 8: //商品价格
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;


                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        if (detOut >= 0)
                        {
                            detOut = Math.Round(detOut, 2);
                        }
                        else
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[8].ErrorText = "商品价格输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].ErrorText = "商品数量价格类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 10:  //扣率
                    double dOut = 0.0;
                    if (e.FormattedValue.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 100;
                        break;
                    }
                    if (Double.TryParse(e.FormattedValue.ToString(), out dOut))
                    {
                        if (dOut <= 0 || dOut > 100.0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[10].ErrorText = "商品扣率输入错误，请输入0.01-100.0之间的数字";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].ErrorText = "商品扣率输入错误，请输入0.01-100.0之间的数字";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;



            }
            dataGridViewDJMX.EndEdit();


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
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.decCommZZJJ.ToString();
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iKFNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                    getKCL();

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[5];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }

            if (e.ColumnIndex == 5 || e.ColumnIndex == 6) //库房编号
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
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                    getKCL();

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[7];

                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }

        }

        private void getKCL()
        {

            //未定库房
            if (dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[13].Value.ToString() == "" || dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[13].Value.ToString() == "0")
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[14].Value = 0;
                return;
            }

            //未定商品
            if (dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value.ToString() == "" || dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value.ToString() == "0")
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[14].Value = 0;
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 库存数量 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();

            string strKCL = "";
            while (sqldr.Read())
            {
                strKCL = sqldr.GetValue(0).ToString();
            }
            if (strKCL == "")
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[14].Value = 0;
            }
            else
            {
                dataGridViewDJMX.Rows[dataGridViewDJMX.CurrentCell.RowIndex].Cells[14].Value = Convert.ToDecimal(strKCL);
            }
            sqlConn.Close();
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
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[5];
                                        break;
                                    case 5:
                                    case 6:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    case 7:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                        break;
                                    case 8:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[10];
                                        break;
                                    case 10:
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

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dKUL = 0, dKCCBJ = 0, dML = 0, dSJJE = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dML1 = 0, dSJJE1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;
            System.Data.SqlClient.SqlTransaction sqlta;

            string strDateSYS;
            cGetInformation.getSystemDateTime();
            strDateSYS = cGetInformation.strSYSDATATIME;

            string sBMID = "NULL";
            if (iBM != 0)
                sBMID = iBM.ToString();

            switch (iStyle)
            {
                case 0://冲红
                    //保存完毕
                    if (isSaved)
                    {
                        MessageBox.Show("进货退出制单已经冲红,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    //格式确认
                    if (!countAmount())
                    {
                        MessageBox.Show("进货退出制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }


                    sqlConn.Open();
                    //查财务
                    sqlComm.CommandText = "SELECT 结算付款汇总表.单据编号 FROM 结算付款勾兑表 INNER JOIN 结算付款汇总表 ON 结算付款勾兑表.付款ID = 结算付款汇总表.ID WHERE (结算付款勾兑表.单据编号 = N'" + labelDJBH.Text + "') AND (结算付款勾兑表.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();
                    if (sqldr.HasRows)
                    {
                        while (sqldr.Read())
                        {
                            MessageBox.Show("已有财务勾兑记录,单据号为：" + sqldr.GetValue(0).ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                        sqldr.Close();
                        sqlConn.Close();
                        return;
                    }
                    sqldr.Close();

                    //发票记录
                    sqlComm.CommandText = "SELECT 发票号, ID FROM 进货退出汇总表 WHERE (发票号 IS NOT NULL) AND (发票号 NOT LIKE N'不开票%') AND (ID = " + intDJID.ToString() + ") AND (发票号 NOT LIKE N'现金不开票%')";
                    sqldr = sqlComm.ExecuteReader();
                    bool b=false;
                    if (sqldr.HasRows)
                    {
                        while (sqldr.Read())
                        {
                            if (sqldr.GetValue(0).ToString().Trim() != "")
                            {
                                MessageBox.Show("已有发票记录,发票号为：" + sqldr.GetValue(0).ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                b = true;
                            }
                            break;
                        }
                        if (b)
                        {
                            sqldr.Close();
                            sqlConn.Close();
                            return;
                        }
                    }
                    sqldr.Close();

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
                    sqlComm.CommandText = "SELECT 日期 from 进货退出汇总表 WHERE (ID = " + intDJID.ToString() + ")";
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
                        sqlComm.CommandText = "UPDATE 进货退出汇总表 SET BeActive = 0 WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        sqlComm.CommandText = "UPDATE 进货退出汇总表 SET 冲红时间 = '" + strDateSYS + "' WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //单位应付账
                        sqlComm.CommandText = "SELECT 应付账款 FROM 单位表 WHERE (ID = " + iSupplyCompany.ToString() + ")";
                        sqldr = sqlComm.ExecuteReader();


                        dKCJE = 0;
                        while (sqldr.Read())
                        {
                            dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        }
                        sqldr.Close();
                        dKCJE1 = dKCJE + Convert.ToDecimal(labelSJJE.Text);
                        sqlComm.CommandText = "UPDATE 单位表 SET 应付账款 = " + dKCJE1.ToString() + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                        sqlComm.ExecuteNonQuery();


                        //单位历史纪录
                        sqlComm.CommandText = "INSERT INTO 单位历史账表 (单位ID, 日期, 单据编号, 摘要, 购进金额, 应付余额, 购进标记, 业务员ID, 冲抵单号, BeActive, 部门ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + labelDJBH.Text + "冲', N'进货退出单冲红', " + labelSJJE.Text.ToString() + ", " + dKCJE1.ToString() + ", 1, " + iYWY.ToString() + ", N'" + textBoxHTH.Text + "', 1,"+sBMID+")";
                        sqlComm.ExecuteNonQuery();

                        //库存
                        for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;

                            dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value.ToString());
                            dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value.ToString());
                            dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString());
                            dYSYE1 = dKCJE1;
                            //总库存变更
                            sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 库存金额, 应付金额 FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                            }
                            dKUL += dKUL1;
                            dKCJE += dKCJE1;
                            dYSYE += dYSYE1;
                            sqldr.Close();

                            sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + ", 库存成本价 = " + dKCCBJ.ToString() + ", 应付金额=" + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //总账历史纪录
                            sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 退出数量, 退出单价, 退出金额, 总结存数量, 总结存金额, 应付金额, BeActive, 出库数量, 出库单价, 出库金额, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "冲', N'进货退出制单冲红', -1*" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", -1*" + dKCJE1 + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,-1*" + dKUL1.ToString() + "," + dKCCBJ1.ToString() + ",-1*" + dKCJE1.ToString() + ","+sBMID+")";
                            sqlComm.ExecuteNonQuery();


                            //分库存更新
                            sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 库存金额, 应付金额 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                            }
                            sqldr.Close();
                            dKUL += dKUL1;
                            dKCJE += dKCJE1;
                            dYSYE += dYSYE1;

                            sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + ", 库存成本价 = " + dKCCBJ.ToString() + ", 应付金额=" + dYSYE.ToString() + "  WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                            sqlComm.ExecuteNonQuery();

                            //库房账历史纪录
                            sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 退出数量, 退出单价, 退出金额, 库房结存数量, 库房结存金额, 应付金额, BeActive, 出库数量, 出库单价, 出库金额, 部门ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ",'" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "冲', N'进货退出单冲红', -1*" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", -1*" + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1, -1*" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", -1*" + dKCJE1.ToString() + ","+sBMID+")";
                            sqlComm.ExecuteNonQuery();

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



                    //MessageBox.Show("进货退出制单冲红成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSaved = true;
                    if (MessageBox.Show("进货退出制单冲红成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Close();
                    }

                    break;


                case 1://修改

                    //保存完毕
                    if (isSaved)
                    {
                        MessageBox.Show("进货退出制单制单已经修改,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    //格式确认
                    if (!countAmount())
                    {
                        MessageBox.Show("进货退出制单制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }


                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        //表单汇总
                        sqlComm.CommandText = "UPDATE 进货退出汇总表 SET 价税合计 = " + labelSJJE.Text + " WHERE (ID = " + intDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //应付账
                        sqlComm.CommandText = "SELECT 应付账款 FROM 单位表 WHERE (ID = " + iSupplyCompany.ToString() + ")";
                        sqldr = sqlComm.ExecuteReader();
                        while (sqldr.Read())
                        {
                            dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        }
                        sqldr.Close();
                        dKCJE = dKCJE + Convert.ToDecimal(labelSJJE.Text) - dDJSUM;
                        sqlComm.CommandText = "UPDATE 单位表 SET 应付账款 = " + dKCJE + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //单位历史纪录
                        sqlComm.CommandText = "INSERT INTO 单位历史账表 (单位ID, 日期, 单据编号, 摘要, 购进金额, 应付余额, 购进标记, 业务员ID, 冲抵单号, BeActive, 部门ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + labelDJBH.Text + "改', N'进货退出单修改', " + labelSJJE.Text.ToString() + ", " + dKCJE.ToString() + ", 1, " + iYWY.ToString() + ", N'" + labelDJBH.Text + "', 1,"+sBMID+")";
                        sqlComm.ExecuteNonQuery();

                        //明细&库存 原单据
                        for (i = 0; i < iRowCount; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;

                            if (!Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[0].Value.ToString())) //删除
                            {
                                dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value.ToString());
                                dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value.ToString());
                                dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString());
                                dYSYE1 = dKCJE1;
                                //总库存变更
                                sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 库存金额, 应付金额 FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                                sqldr = sqlComm.ExecuteReader();
                                while (sqldr.Read())
                                {
                                    dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                    dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                    dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                                    dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                                }
                                dKUL += dKUL1;
                                dKCJE += dKCJE1;
                                dYSYE += dYSYE1;
                                sqldr.Close();

                                sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + ", 库存成本价 = " + dKCCBJ.ToString() + ", 应付金额=" + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                                sqlComm.ExecuteNonQuery();

                                //总账历史纪录
                                sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 退出数量, 退出单价, 退出金额, 总结存数量, 总结存金额, 应付金额, BeActive, 出库数量, 出库单价, 出库金额, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "改', N'进货退出制单修改', -1*" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", -1*" + dKCJE1 + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,-1*" + dKUL1.ToString() + "," + dKCCBJ1.ToString() + ",-1*" + dKCJE1.ToString() + ","+sBMID+")";
                                sqlComm.ExecuteNonQuery();


                                //分库存更新
                                sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 库存金额, 应付金额 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                sqldr = sqlComm.ExecuteReader();
                                while (sqldr.Read())
                                {
                                    dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                    dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                    dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                                    dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                                }
                                sqldr.Close();
                                dKUL += dKUL1;
                                dKCJE += dKCJE1;
                                dYSYE += dYSYE1;

                                sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + ", 库存成本价 = " + dKCCBJ.ToString() + ", 应付金额=" + dYSYE.ToString() + "  WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                                sqlComm.ExecuteNonQuery();

                                //库房账历史纪录
                                sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 退出数量, 退出单价, 退出金额, 库房结存数量, 库房结存金额, 应付金额, BeActive, 出库数量, 出库单价, 出库金额, 部门ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ",'" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "改', N'进货退出单修改', -1*" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", -1*" + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1, -1*" + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", -1*" + dKCJE1.ToString() + ","+sBMID+")";
                                sqlComm.ExecuteNonQuery();



                                sqlComm.CommandText = "DELETE FROM 进货退出明细表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[17].Value.ToString() + ")";
                                sqlComm.ExecuteNonQuery();
                            }
                        }

                        //新增明细
                        for (i = iRowCount; i < dataGridViewDJMX.Rows.Count; i++)
                        {
                            if (dataGridViewDJMX.Rows[i].IsNewRow)
                                continue;

                            sqlComm.CommandText = "INSERT INTO 进货退出明细表 (单据ID, 商品ID, 库房ID, 原单据ID, 单价, 金额, 扣率, 实计金额, 未付款金额, BeActive, 数量) VALUES (" + intDJID.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", 0, " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", 1," + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ")";
                            sqlComm.ExecuteNonQuery();


                            dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value.ToString());
                            dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value.ToString());
                            dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString());
                            dYSYE1 = dKCJE1;
                            //总库存变更
                            sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 库存金额, 应付金额 FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                            }
                            dKUL -= dKUL1;
                            dKCJE -= dKCJE1;
                            dYSYE -= dYSYE1;
                            sqldr.Close();

                            sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + ", 库存成本价 = " + dKCCBJ.ToString() + ", 应付金额=" + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ")";
                            sqlComm.ExecuteNonQuery();

                            //总账历史纪录
                            sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 退出数量, 退出单价, 退出金额, 总结存数量, 总结存金额, 应付金额, BeActive, 出库数量, 出库单价, 出库金额, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "改', N'进货退出制单修改', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1 + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1," + dKUL1.ToString() + "," + dKCCBJ1.ToString() + "," + dKCJE1.ToString() + ","+sBMID+")";
                            sqlComm.ExecuteNonQuery();


                            //分库存更新
                            sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 库存金额, 应付金额 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                            sqldr = sqlComm.ExecuteReader();
                            while (sqldr.Read())
                            {
                                dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                                dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                                dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                                dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                            }
                            sqldr.Close();
                            dKUL += dKUL1;
                            dKCJE += dKCJE1;
                            dYSYE += dYSYE1;

                            sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额 = " + dKCJE.ToString() + ", 库存成本价 = " + dKCCBJ.ToString() + ", 应付金额=" + dYSYE.ToString() + "  WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ") AND (BeActive = 1)";
                            sqlComm.ExecuteNonQuery();

                            //库房账历史纪录
                            sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 退出数量, 退出单价, 退出金额, 库房结存数量, 库房结存金额, 应付金额, BeActive, 出库数量, 出库单价, 出库金额, 部门ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ",'" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "改', N'进货退出单修改', " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1, " + dKUL1.ToString() + ", " + dKCCBJ1.ToString() + ", " + dKCJE1.ToString() + ","+sBMID+")";
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




                    //MessageBox.Show("进货退出单修改成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSaved = true;
                    if (MessageBox.Show("进货退出单修改成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Close();
                    }

                    break;





            }
        }

        private void FormJHTCZD_EDIT_FormClosing(object sender, FormClosingEventArgs e)
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
                MessageBox.Show("进货退出单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "进货退出单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("进货退出单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "进货退出单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

    }
}